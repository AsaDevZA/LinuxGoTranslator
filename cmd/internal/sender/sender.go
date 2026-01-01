package sender

import (
	"net"
)

func SendToNVR(data *models.UniversalData, address string) error {
	// Serialize data to send
	sendData, err := SerializeData(data)
	if err != nil {
		return err
	}

	// Create UDP connection to NVR
	addr, err := net.ResolveUDPAddr("udp", address)
	if err != nil {
		return err
	}

	conn, err := net.DialUDP("udp", nil, addr)
	if err != nil {
		return err
	}
	defer conn.Close()

	_, err = conn.Write(sendData)
	return err
}
