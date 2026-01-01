// cmd/main.go
package main

import (
	"context"
	"flag"
	"fmt"
	"log"
	"net"
	"net/http"
	"os"
	"os/signal"
	"syscall"
	"time"

	"github.com/AsaDevZA/LinuxGoTranslator/internal/config"
	"github.com/AsaDevZA/LinuxGoTranslator/internal/factory"
	"github.com/AsaDevZA/LinuxGoTranslator/internal/license"
	"github.com/AsaDevZA/LinuxGoTranslator/internal/sender"
	"github.com/AsaDevZA/LinuxGoTranslator/internal/web"
)

func main() {
	configPath := flag.String("config", "configs/default.yaml", "path to config file")
	flag.Parse()

	log.SetFlags(log.LstdFlags | log.Lshortfile)
	log.Println("POS Metadata Translator starting...")

	cfg, err := config.Load(*configPath)
	if err != nil {
		log.Fatalf("Failed to load config: %v", err)
	}

	if err := license.Validate(cfg.License); err != nil {
		log.Fatalf("License validation failed: %v", err)
	}
	log.Printf("License valid. Max devices: %d", cfg.License.MaxTotalDevices)

	ctx, cancel := context.WithCancel(context.Background())
	defer cancel()

	udpServers := startUDPServers(ctx, cfg)
	webServer := startWebServer(ctx, cfg)

	sigChan := make(chan os.Signal, 1)
	signal.Notify(sigChan, syscall.SIGINT, syscall.SIGTERM)
	<-sigChan

	log.Println("Shutdown signal received...")
	cancel()

	shutdownCtx, shutdownCancel := context.WithTimeout(context.Background(), 10*time.Second)
	defer shutdownCancel()

	for _, srv := range udpServers {
		srv.Shutdown(shutdownCtx)
	}

	if err := webServer.Shutdown(shutdownCtx); err != nil {
		log.Printf("Web server shutdown error: %v", err)
	}

	log.Println("Service stopped cleanly.")
}

func startUDPServers(ctx context.Context, cfg *config.Config) []*UDPServer {
	var servers []*UDPServer

	// Placeholder ports - replace with config-driven list later
	ports := []int{41000, 41001, 44000, 44001}

	for _, port := range ports {
		addr := &net.UDPAddr{IP: net.ParseIP("0.0.0.0"), Port: port}
		conn, err := net.ListenUDP("udp", addr)
		if err != nil {
			log.Printf("Failed to listen on UDP %d: %v", port, err)
			continue
		}

		srv := &UDPServer{
			conn:     conn,
			cfg:      cfg,
			factory:  factory.NewParserFactory(),
			sender:   sender.NewUDPSender(cfg.CCTV.NVRIP, cfg.CCTV.NVRPort),
			shutdown: make(chan struct{}),
		}

		go srv.run(ctx)
		servers = append(servers, srv)
		log.Printf("UDP listener started on :%d", port)
	}

	return servers
}

func startWebServer(ctx context.Context, cfg *config.Config) *http.Server {
	mux := http.NewServeMux()
	web.SetupHandlers(mux, cfg)

	srv := &http.Server{
		Addr:    fmt.Sprintf(":%d", cfg.Web.Port),
		Handler: mux,
	}

	go func() {
		log.Printf("Web interface listening on http://0.0.0.0:%d", cfg.Web.Port)
		if err := srv.ListenAndServe(); err != nil && err != http.ErrServerClosed {
			log.Printf("Web server error: %v", err)
		}
	}()

	return srv
}

type UDPServer struct {
	conn     *net.UDPConn
	cfg      *config.Config
	factory  *factory.ParserFactory
	sender   *sender.UDPSender
	shutdown chan struct{}
}

func (s *UDPServer) run(ctx context.Context) {
	buf := make([]byte, 4096)

	for {
		select {
		case <-ctx.Done():
			s.conn.Close()
			return
		case <-s.shutdown:
			s.conn.Close()
			return
		default:
			n, remoteAddr, err := s.conn.ReadFromUDP(buf)
			if err != nil {
				if !isClosed(err) {
					log.Printf("UDP read error: %v", err)
				}
				continue
			}

			data := make([]byte, n)
			copy(data, buf[:n])

			go s.processPacket(data, remoteAddr)
		}
	}
}

func (s *UDPServer) processPacket(data []byte, remote *net.UDPAddr) {
	// Placeholder provider detection
	provider := "spar"

	parser := s.factory.GetParser(provider)
	if parser == nil {
		log.Printf("No parser for provider %s from %s", provider, remote)
		return
	}

	universal, err := parser.Parse(data)
	if err != nil {
		log.Printf("Parse error from %s: %v", remote, err)
		return
	}

	if err := s.sender.Send(universal); err != nil {
		log.Printf("Send error: %v", err)
		return
	}

	log.Printf("Processed packet from %s → CCTV (provider: %s)", remote, provider)
}

func (s *UDPServer) Shutdown(ctx context.Context) {
	select {
	case s.shutdown <- struct{}{}:
	default:
	}
	s.conn.Close()
}

func isClosed(err error) bool {
	return err.Error() == "use of closed network connection"
}
