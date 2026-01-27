// internal/license/license.go
package license

import (
	"crypto/aes"
	"crypto/cipher"
	"crypto/sha256"
	"encoding/base64"
	"encoding/binary"
	"fmt"
	"net"

	"github.com/AsaDevZA/LinuxGoTranslator/internal/config"
)

type StoreType int

const (
	StoreTypePickNPay StoreType = 1
	StoreTypeSpar     StoreType = 2
	StoreTypeArch     StoreType = 3
)

type LicenseData struct {
	StoreType         StoreType
	POSChannels       int
	ScaleChannels     int
	LPRChannels       int
	AdvancedReporting bool
	CloudStorage      bool
}

func Validate(cfg config.LicenseConfig) (*LicenseData, error) {
	currentMAC, err := getPrimaryMAC()
	if err != nil {
		return &LicenseData{
			StoreType:         0,
			POSChannels:       0,
			ScaleChannels:     0,
			LPRChannels:       0,
			AdvancedReporting: false,
			CloudStorage:      false,
		}, nil
	}

	if cfg.UnlockKey == "" {
		return &LicenseData{
			StoreType:         0,
			POSChannels:       0,
			ScaleChannels:     0,
			LPRChannels:       0,
			AdvancedReporting: false,
			CloudStorage:      false,
		}, nil
	}

	licenseData, err := DecryptLicense(cfg.UnlockKey, currentMAC)
	if err != nil {
		return &LicenseData{
			StoreType:         0,
			POSChannels:       0,
			ScaleChannels:     0,
			LPRChannels:       0,
			AdvancedReporting: false,
			CloudStorage:      false,
		}, nil
	}

	return licenseData, nil
}

func EncryptLicense(data *LicenseData, fingerprint string) (string, error) {
	payload := make([]byte, 16)
	payload[0] = byte(data.StoreType)
	binary.BigEndian.PutUint32(payload[1:5], uint32(data.POSChannels))
	binary.BigEndian.PutUint32(payload[5:9], uint32(data.ScaleChannels))
	binary.BigEndian.PutUint32(payload[9:13], uint32(data.LPRChannels))
	if data.AdvancedReporting {
		payload[13] = 1
	}
	if data.CloudStorage {
		payload[14] = 1
	}

	key := deriveKey(fingerprint)
	block, err := aes.NewCipher(key)
	if err != nil {
		return "", err
	}

	ciphertext := make([]byte, aes.BlockSize+len(payload))
	iv := ciphertext[:aes.BlockSize]
	copy(iv, key[:aes.BlockSize])

	stream := cipher.NewCFBEncrypter(block, iv)
	stream.XORKeyStream(ciphertext[aes.BlockSize:], payload)

	return base64.StdEncoding.EncodeToString(ciphertext), nil
}

func DecryptLicense(unlockKey, fingerprint string) (*LicenseData, error) {
	ciphertext, err := base64.StdEncoding.DecodeString(unlockKey)
	if err != nil {
		return nil, err
	}

	key := deriveKey(fingerprint)
	block, err := aes.NewCipher(key)
	if err != nil {
		return nil, err
	}

	if len(ciphertext) < aes.BlockSize {
		return nil, fmt.Errorf("ciphertext too short")
	}

	iv := ciphertext[:aes.BlockSize]
	ciphertext = ciphertext[aes.BlockSize:]

	stream := cipher.NewCFBDecrypter(block, iv)
	payload := make([]byte, len(ciphertext))
	stream.XORKeyStream(payload, ciphertext)

	if len(payload) < 16 {
		return nil, fmt.Errorf("invalid payload")
	}

	data := &LicenseData{
		StoreType:         StoreType(payload[0]),
		POSChannels:       int(binary.BigEndian.Uint32(payload[1:5])),
		ScaleChannels:     int(binary.BigEndian.Uint32(payload[5:9])),
		LPRChannels:       int(binary.BigEndian.Uint32(payload[9:13])),
		AdvancedReporting: payload[13] == 1,
		CloudStorage:      payload[14] == 1,
	}

	return data, nil
}

func deriveKey(fingerprint string) []byte {
	hash := sha256.Sum256([]byte(fingerprint))
	return hash[:]
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

func GetPrimaryMAC() (string, error) {
	return getPrimaryMAC()
}

func GetStoreTypeName(st StoreType) string {
	switch st {
	case StoreTypePickNPay:
		return "Pick n Pay"
	case StoreTypeSpar:
		return "Spar"
	case StoreTypeArch:
		return "Arch Software"
	default:
		return "Unknown"
	}
}
