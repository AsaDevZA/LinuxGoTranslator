// internal/parsers/universal.go
package parsers

import "time"

type UniversalData struct {
	Timestamp time.Time
	Raw       []byte
}

type SparParser struct{}

func (p *SparParser) Parse(data []byte) (*UniversalData, error) {
	return &UniversalData{Timestamp: time.Now(), Raw: data}, nil
}

type AshidaParser struct{}

func (p *AshidaParser) Parse(data []byte) (*UniversalData, error) {
	return &UniversalData{Timestamp: time.Now(), Raw: data}, nil
}
