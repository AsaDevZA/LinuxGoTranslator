package main

import (
	"flag"
	"log"

	internal_config "github.com/AsaDevZA/LinuxGoTranslator/internal/config"
	internal_web "github.com/AsaDevZA/LinuxGoTranslator/internal/web"
)

var cfg internal_config.Config

func main() {
	// Parse command line flags
	listenPort := flag.String("p", "14000", "UDP port to listen on")
	nvrAddress := flag.String("n", "192.168.1.100:14000", "NVR address and port")

	flag.Parse()

	// Load configuration
	err := internal_config.LoadConfig(&cfg)
	if err != nil {
		log.Fatalf("Error loading config: %v", err)
	}

	cfg.ListenPort = *listenPort
	cfg.NVRAddress = *nvrAddress

	// Start UDP listener
	go startUDPListener(cfg.ListenPort)

	// Start HTTP server
	internal_web.StartWebServer()

	// Run the application
	select {} // Blocks forever
}
