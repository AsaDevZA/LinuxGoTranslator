package config

import (
	"os"

	"gopkg.in/yaml.v3"
)

// Config represents the full application configuration
type Config struct {
	Web         WebConfig     `yaml:"web"`
	CCTV        CCTVConfig    `yaml:"cctv"`
	DSTORE      DSTOREConfig  `yaml:"dstore"`
	VBSPOS      VBSPOSConfig  `yaml:"vbspos"`
	License     LicenseConfig `yaml:"license"`
	IPMaps      []IPMap       `yaml:"ipmaps"` // NVR mappings
	DeviceTypes []DeviceType  `yaml:"device_types"`
	SiteDetails SiteDetails   `yaml:"site_details"`
	Admin       AdminConfig   `yaml:"admin"`
}

type WebConfig struct {
	Port int `yaml:"port"`
}

type CCTVConfig struct {
	NVRIP   string `yaml:"nvr_ip"`
	NVRPort int    `yaml:"nvr_port"`
}

type DSTOREConfig struct {
	ServerMainPort   int `yaml:"servermain_port"`
	ServerSecondPort int `yaml:"serversecond_port"`
	ServerAuxPort    int `yaml:"serveraux_port"`
}

type VBSPOSConfig struct {
	MulticastIP string `yaml:"multicast_ip"`
	VbsMainPort int    `yaml:"vbspos_port"`
}

type LicenseConfig struct {
	MaxTotalDevices    int    `yaml:"max_total_devices"`
	MaxPerScale        int    `yaml:"max_per_scale"`
	MachineFingerprint string `yaml:"machine_fingerprint"`
}

// IPMap represents a single NVR mapping
type IPMap struct {
	IPAddress string `yaml:"ipaddress"`
	Port      int    `yaml:"port"`
	Type      int    `yaml:"type"` // Brand or provider type
}

// DeviceType represents a device type configuration
type DeviceType struct {
	ID          int    `yaml:"id"`
	Name        string `yaml:"name"`
	Description string `yaml:"description"`
}

// SiteDetails represents site and client information
type SiteDetails struct {
	SiteName     string `yaml:"site_name"`
	Branch       string `yaml:"branch"`
	SiteLocation string `yaml:"site_location"`
	OwnerName    string `yaml:"owner_name"`
	OwnerSurname string `yaml:"owner_surname"`
	ContactCell  string `yaml:"contact_cell"`
	ContactEmail string `yaml:"contact_email"`
}

// AdminConfig represents admin credentials
type AdminConfig struct {
	Username string `yaml:"username"`
	Password string `yaml:"password"`
}

// Load reads YAML config from file
func Load(path string) (*Config, error) {
	data, err := os.ReadFile(path)
	if err != nil {
		return nil, err
	}
	var cfg Config
	if err := yaml.Unmarshal(data, &cfg); err != nil {
		return nil, err
	}

	if len(cfg.DeviceTypes) == 0 {
		cfg.DeviceTypes = GetDefaultDeviceTypes()
	}

	if cfg.Admin.Username == "" {
		cfg.Admin.Username = "admin"
	}
	if cfg.Admin.Password == "" {
		cfg.Admin.Password = "admin"
	}

	return &cfg, nil
}

// GetDefaultDeviceTypes returns the default device types
func GetDefaultDeviceTypes() []DeviceType {
	return []DeviceType{
		{ID: 1, Name: "Terioka UDP", Description: "Terioka UDP Protocol"},
		{ID: 2, Name: "Terioka TCP", Description: "Terioka TCP Protocol"},
		{ID: 3, Name: "Ishida", Description: "Ishida Standard Protocol"},
		{ID: 4, Name: "Ishida BC4000", Description: "Ishida BC4000 Protocol"},
	}
}

// GetDeviceTypeName returns the name of a device type by ID
func (c *Config) GetDeviceTypeName(id int) string {
	for _, dt := range c.DeviceTypes {
		if dt.ID == id {
			return dt.Name
		}
	}
	return "Unknown"
}

// GetDeviceTypeDescription returns the description of a device type by ID
func (c *Config) GetDeviceTypeDescription(id int) string {
	for _, dt := range c.DeviceTypes {
		if dt.ID == id {
			return dt.Description
		}
	}
	return ""
}
