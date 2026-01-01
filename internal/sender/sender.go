// internal/sender/sender.go
package sender

import (
	"net"

	"github.com/AsaDevZA/LinuxGoTranslator/internal/parsers"
)

type UDPSender struct {
	conn *net.UDPConn
}

func NewUDPSender(ip string, port int) *UDPSender {
	addr := &net.UDPAddr{IP: net.ParseIP(ip), Port: port}
	conn, err := net.DialUDP("udp", nil, addr)
	if err != nil {
		panic(err)
	}
	return &UDPSender{conn: conn}
}

func (s *UDPSender) Send(data *parsers.UniversalData) error {
	payload := []byte("example metadata") // placeholder
	_, err := s.conn.Write(payload)
	return err
}
