// internal/multicast/server.go
package multicast

import (
	"context"
	"errors"
	"log"
	"net"
	"strings"
	"time"

	"github.com/AsaDevZA/LinuxGoTranslator/internal/factory"
	"github.com/AsaDevZA/LinuxGoTranslator/internal/sender"
	"golang.org/x/net/ipv4"
)

type MulticastServer struct {
	conn     *net.UDPConn
	factory  *factory.ParserFactory
	sender   *sender.UDPSender
	shutdown chan struct{}
}

func NewMulticastServer(multicastIP string, port int, f *factory.ParserFactory, s *sender.UDPSender) *MulticastServer {
	groupAddr := &net.UDPAddr{IP: net.ParseIP(multicastIP), Port: port}

	conn, err := net.ListenMulticastUDP("udp", nil, groupAddr)
	if err != nil {
		log.Fatalf("Multicast listen error: %v", err)
	}

	p := ipv4.NewPacketConn(conn)
	p.SetMulticastLoopback(true)

	return &MulticastServer{
		conn:     conn,
		factory:  f,
		sender:   s,
		shutdown: make(chan struct{}),
	}
}

func (s *MulticastServer) Run(ctx context.Context) {
	buf := make([]byte, 4096)

	for {
		select {
		case <-s.shutdown:
			return
		case <-ctx.Done():
			return
		default:
			n, remote, err := s.conn.ReadFromUDP(buf)
			if err != nil {
				if errors.Is(err, net.ErrClosed) || strings.Contains(err.Error(), "use of closed") {
					return
				}
				log.Printf("Multicast read error: %v", err)
				continue
			}

			data := make([]byte, n)
			copy(data, buf[:n])

			go s.processPacket(data, remote)
		}
	}
}

func (s *MulticastServer) processPacket(data []byte, remote *net.UDPAddr) {
	provider := "vbspos"
	parser := s.factory.GetParser(provider)
	if parser == nil {
		log.Printf("No parser for vbspos from %s", remote)
		return
	}

	universal, err := parser.Parse(data)
	if err != nil {
		log.Printf("VBS parse error from %s: %v", remote, err)
		return
	}

	// Temporarily always forward (remove condition to debug)
	if err := s.sender.Send(universal); err != nil {
		log.Printf("Send error: %v", err)
		return
	}
	log.Printf("VBS processed → CCTV (till: %s, from: %s)", universal.TillID, remote)
}

func (s *MulticastServer) Shutdown(ctx context.Context) {
	select {
	case s.shutdown <- struct{}{}:
	default:
	}
	time.Sleep(100 * time.Millisecond)
	s.conn.Close()
}
