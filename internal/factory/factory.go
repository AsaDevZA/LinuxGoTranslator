// internal/factory/factory.go
package factory

import "github.com/AsaDevZA/LinuxGoTranslator/internal/parsers"

type Parser interface {
	Parse(data []byte) (*parsers.UniversalData, error)
}

type ParserFactory struct{}

func NewParserFactory() *ParserFactory {
	return &ParserFactory{}
}

func (f *ParserFactory) GetParser(provider string) Parser {
	switch provider {
	case "vbspos":
		return &parsers.VBSParser{}
	case "spar":
		return &parsers.SparParser{}
	case "ashida":
		return &parsers.AshidaParser{}
	default:
		return nil
	}
}
