package config

import (
	"encoding/json"
	"os"
)

type Config struct {
	ListenPort string `json:"listen_port"`
	NVRAddress string `json:"nvr_address"`
	// Add other configuration fields here
}

func LoadConfig(cfg *Config) error {
	// Load configuration from file
	data, err := os.ReadFile("configs/config.yaml")
	if err != nil {
		return err
	}

	// Unmarshal JSON/YAML configuration
	return json.Unmarshal(data, cfg)
}
