package factory

type Parser interface {
	Parse(data []byte) (*models.UniversalData, error)
}

var parsers = make(map[string]Parser)

func RegisterParser(provider string, parser Parser) {
	parsers[provider] = parser
}

func GetParser(provider string) Parser {
	parser, exists := parsers[provider]
	if !exists {
		return nil
	}
	return parser
}
