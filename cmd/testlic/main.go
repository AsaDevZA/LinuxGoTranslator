package main

import (
	"fmt"
	"github.com/AsaDevZA/LinuxGoTranslator/internal/license"
)

func main() {
	lic := &license.LicenseData{
		StoreType:         license.StoreTypePickNPay,
		POSChannels:       10,
		ScaleChannels:     64,
		LPRChannels:       4,
		AdvancedReporting: true,
		CloudStorage:      true,
	}
	key, err := license.EncryptLicense(lic, "f0:d4:15:05:36:e2")
	if err != nil {
		fmt.Println("Error:", err)
		return
	}
	fmt.Println(key)
}
