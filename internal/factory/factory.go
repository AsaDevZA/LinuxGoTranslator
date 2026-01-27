// internal/factory/factory.go
package factory

import (
	"github.com/AsaDevZA/LinuxGoTranslator/internal/license"
	"github.com/AsaDevZA/LinuxGoTranslator/internal/parsers"
)

type Parser interface {
	Parse(data []byte) (*parsers.UniversalData, error)
}

type ParserFactory struct {
	storeType license.StoreType
}

func NewParserFactory(storeType license.StoreType) *ParserFactory {
	return &ParserFactory{storeType: storeType}
}

func (f *ParserFactory) GetParser(provider string) Parser {
	switch f.storeType {
	case license.StoreTypePickNPay:
		return f.getPickNPayParser(provider)
	case license.StoreTypeSpar:
		return f.getSparParser(provider)
	case license.StoreTypeArch:
		return f.getArchParser(provider)
	default:
		return nil
	}
}

func (f *ParserFactory) getPickNPayParser(provider string) Parser {
	switch provider {
	case "vbspos":
		return &parsers.VBSParser{}
	default:
		return nil
	}
}

func (f *ParserFactory) getSparParser(provider string) Parser {
	switch provider {
	case "spar":
		return &parsers.SparParser{}
	default:
		return nil
	}
}

func (f *ParserFactory) getArchParser(provider string) Parser {
	switch provider {
	case "ashida":
		return &parsers.AshidaParser{}
	default:
		return nil
	}
}
