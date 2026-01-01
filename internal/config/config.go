// internal/config/config.go
package config

import (
	"os"

	"gopkg.in/yaml.v3"
)

type Config struct {
	Web     WebConfig     `yaml:"web"`
	CCTV    CCTVConfig    `yaml:"cctv"`
	License LicenseConfig `yaml:"license"`
}

type WebConfig struct {
	Port int `yaml:"port"`
}

type CCTVConfig struct {
	NVRIP   string `yaml:"nvr_ip"`
	NVRPort int    `yaml:"nvr_port"`
}

type LicenseConfig struct {
	MaxTotalDevices    int    `yaml:"max_total_devices"`
	MaxPerScale        int    `yaml:"max_per_scale"`
	MachineFingerprint string `yaml:"machine_fingerprint"`
}

func Load(path string) (*Config, error) {
	data, err := os.ReadFile(path)
	if err != nil {
		return nil, err
	}

	var cfg Config
	if err := yaml.Unmarshal(data, &cfg); err != nil {
		return nil, err
	}

	return &cfg, nil
}
