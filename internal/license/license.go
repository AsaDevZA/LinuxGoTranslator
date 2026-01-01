// internal/license/license.go
package license

import (
	"fmt"
	"net"

	"github.com/AsaDevZA/LinuxGoTranslator/internal/config"
)

func Validate(cfg config.LicenseConfig) error {
	currentMAC, err := getPrimaryMAC()
	if err != nil {
		return err
	}

	if currentMAC != cfg.MachineFingerprint {
		return fmt.Errorf("machine fingerprint mismatch")
	}

	return nil
}

func getPrimaryMAC() (string, error) {
	ifaces, err := net.Interfaces()
	if err != nil {
		return "", err
	}
	for _, iface := range ifaces {
		if iface.Flags&net.FlagUp == 0 || iface.Flags&net.FlagLoopback != 0 {
			continue
		}
		addrs, err := iface.Addrs()
		if err != nil {
			continue
		}
		if len(addrs) > 0 {
			return iface.HardwareAddr.String(), nil
		}
	}
	return "", fmt.Errorf("no suitable interface found")
}
