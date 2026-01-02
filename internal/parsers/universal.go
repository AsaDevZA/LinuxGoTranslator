// internal/parsers/universal.go
package parsers

import (
	"encoding/xml"
	"log"
	"time"

	"github.com/AsaDevZA/LinuxGoTranslator/internal/web"
)

type UniversalData struct {
	Timestamp time.Time
	Raw       []byte
	TillID    string
}

type POSEvent struct {
	StoreCode    string `xml:"storeCode"`
	Till         string `xml:"till"`
	TxnNumber    string `xml:"txnNumber"`
	OperatorCode struct {
		Name  string `xml:"name,attr"`
		Value string `xml:"value"`
	} `xml:"operatorCode"`
	SequenceNumber string `xml:"sequenceNumber"`
	Date           string `xml:"date"`
	Time           string `xml:"time"`
	Event          struct {
		Type       string `xml:"type,attr"`
		Amount     string `xml:"amount"`
		TenderName string `xml:"tenderName"`
	} `xml:"event"`
}

type VBSParser struct{}

func (p *VBSParser) Parse(data []byte) (*UniversalData, error) {
	var event POSEvent
	if err := xml.Unmarshal(data, &event); err != nil {
		return nil, err
	}

	content := string(data)
	log.Printf("VBS POS received (till %s):\n%s", event.Till, content)
	web.Broadcast(content, "vbspos")

	return &UniversalData{
		Timestamp: time.Now(),
		Raw:       data,
		TillID:    event.Till,
	}, nil
}

type SparParser struct{}

func (p *SparParser) Parse(data []byte) (*UniversalData, error) {
	content := string(data)
	log.Printf("SPAR POS raw data received (len=%d):\n%s", len(data), content)
	web.Broadcast(content, "spar")

	return &UniversalData{
		Timestamp: time.Now(),
		Raw:       data,
	}, nil
}

type AshidaParser struct{}

func (p *AshidaParser) Parse(data []byte) (*UniversalData, error) {
	content := string(data)
	log.Printf("ASHIDA raw data received (len=%d):\n%s", len(data), content)
	web.Broadcast(content, "ashida")

	return &UniversalData{
		Timestamp: time.Now(),
		Raw:       data,
	}, nil
}
