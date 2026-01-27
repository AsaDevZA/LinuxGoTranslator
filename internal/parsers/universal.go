// internal/parsers/universal.go
package parsers

import (
	"encoding/xml"
	"fmt"
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
		Type           string `xml:"type,attr"`
		Amount         string `xml:"amount"`
		TenderName     string `xml:"tenderName"`
		DepartmentCode struct {
			Name  string `xml:"name,attr"`
			Value string `xml:"value"`
		} `xml:"departmentCode"`
		Description string `xml:"description"`
		Quantity    string `xml:"quantity"`
		Plu         string `xml:"plu"`
		Sku         string `xml:"sku"`
		UnitPrice   string `xml:"unitPrice"`
	} `xml:"event"`
}

func (p *POSEvent) ToString() string {
	switch p.Event.Type {
	case "LOGON":
		return fmt.Sprintf("Till# %s UserName %s UserID %s %s", p.Till, p.OperatorCode.Name, p.OperatorCode.Value, p.Event.Type)
	case "LOGON INVALID":
		return p.Event.Type
	case "SESSION OVERRIDE":
		return p.Event.Type
	case "LOGOFF":
		return fmt.Sprintf("Till# %s Tx# %s UserName %s UserID %s Seq# %s %s", p.Till, p.TxnNumber, p.OperatorCode.Name, p.OperatorCode.Value, p.SequenceNumber, p.Event.Type)
	case "OPEN CASH DRAWER":
		return fmt.Sprintf("%s CashDrawerstatus %s", p.Event.Type, p.Event.Amount)
	case "REMOVE CASH DRAWER":
		return fmt.Sprintf("%s CashDrawerstatus %s", p.Event.Type, p.Event.Amount)
	case "SECURE":
		return p.Event.Type
	case "AUTHORISATION SUCCESSFUL":
		return p.Event.Type
	case "SECURE UNLOCK":
		return p.Event.Type
	case "AUTHORISATION FAILED":
		return p.Event.Type
	case "ITEM SALE":
		return fmt.Sprintf("Till# %s Tx# %s %s Qty %s Amount R%s", p.Till, p.TxnNumber, p.Event.Description, p.Event.Quantity, p.Event.Amount)
	case "TENDER":
		return fmt.Sprintf("Amount R%s Tendername %s", p.Event.Amount, p.Event.TenderName)
	case "TRANSACTION TOTAL":
		return fmt.Sprintf("Till# %s Tx# %s %s Amount R%s", p.Till, p.TxnNumber, p.Event.Type, p.Event.Amount)
	case "CHANGE":
		return fmt.Sprintf("Till# %s Tx# %s %s Amount R%s", p.Till, p.TxnNumber, p.Event.Type, p.Event.Amount)
	case "TRANSACTION COMPLETE":
		return fmt.Sprintf("Till# %s Tx# %s %s", p.Till, p.TxnNumber, p.Event.Type)
	case "ITEM UNKNOWN":
		return fmt.Sprintf("Till# %s Tx# %s %s", p.Till, p.TxnNumber, p.Event.Type)
	case "ACCOUNT PAYMENT":
		return fmt.Sprintf("Till# %s Tx# %s %s Amount R%s", p.Till, p.TxnNumber, p.Event.Type, p.Event.Amount)
	case "TRANSACTION RECALL":
		return fmt.Sprintf("Till# %s Tx# %s %s", p.Till, p.TxnNumber, p.Event.Type)
	case "ITEM REFUND":
		return fmt.Sprintf("Till# %s Tx# %s %s Qty %s Amount R%s", p.Till, p.TxnNumber, p.Event.Description, p.Event.Quantity, p.Event.Amount)
	case "ITEM VOID":
		return fmt.Sprintf("Till# %s Tx# %s %s Qty %s Amount R%s", p.Till, p.TxnNumber, p.Event.Description, p.Event.Quantity, p.Event.Amount)
	case "ITEM PRICE OVERRIDE":
		return fmt.Sprintf("Till# %s Tx# %s %s Qty %s Amount R%s", p.Till, p.TxnNumber, p.Event.Description, p.Event.Quantity, p.Event.Amount)
	case "ITEM DISCOUNT":
		return fmt.Sprintf("Till# %s Tx# %s %s Qty %s Amount R%s", p.Till, p.TxnNumber, p.Event.Description, p.Event.Quantity, p.Event.Amount)
	case "TRANSACTION LOYALTY":
		return fmt.Sprintf("Till# %s Tx# %s %s", p.Till, p.TxnNumber, p.Event.Type)
	case "TRANSACTION DISCOUNT":
		return fmt.Sprintf("Till# %s Tx# %s %s Amount R%s", p.Till, p.TxnNumber, p.Event.Type, p.Event.Amount)
	case "TRANSACTION SUSPEND":
		return fmt.Sprintf("Till# %s Tx# %s %s", p.Till, p.TxnNumber, p.Event.Type)
	case "TRANSACTION VOID":
		return fmt.Sprintf("Till# %s Tx# %s %s", p.Till, p.TxnNumber, p.Event.Type)
	case "TRANSACTION SEARCH":
		return fmt.Sprintf("Till# %s Tx# %s %s", p.Till, p.TxnNumber, p.Event.Type)
	case "TRANSACTION REPRINT":
		return fmt.Sprintf("Till# %s Tx# %s %s", p.Till, p.TxnNumber, p.Event.Type)
	case "ENQUIRY PRICE":
		return fmt.Sprintf("Till# %s Tx# %s %s", p.Till, p.TxnNumber, p.Event.Type)
	case "ENQUIRY GIFT CARD BALANCE":
		return fmt.Sprintf("Till# %s Tx# %s %s", p.Till, p.TxnNumber, p.Event.Type)
	case "MONEY TRANSFER DEPOSIT":
		return fmt.Sprintf("Till# %s Tx# %s %s", p.Till, p.TxnNumber, p.Event.Type)
	case "MONEY TRANSFER WITHDRAWAL":
		return fmt.Sprintf("Till# %s Tx# %s %s", p.Till, p.TxnNumber, p.Event.Type)
	case "MANAGER MENU":
		return fmt.Sprintf("Till# %s Tx# %s %s", p.Till, p.TxnNumber, p.Event.Type)
	case "PASSWORD FORCE":
		return fmt.Sprintf("Till# %s Tx# %s %s", p.Till, p.TxnNumber, p.Event.Type)
	case "VAS FAILED":
		return fmt.Sprintf("Till# %s Tx# %s %s", p.Till, p.TxnNumber, p.Event.Type)
	case "TENDER VOID":
		return fmt.Sprintf("Till# %s Tx# %s %s Amount R%s Tendername %s", p.Till, p.TxnNumber, p.Event.Type, p.Event.Amount, p.Event.TenderName)
	default:
		return ""
	}
}

type VBSParser struct{}

func (p *VBSParser) Parse(data []byte) (*UniversalData, error) {
	var event POSEvent
	if err := xml.Unmarshal(data, &event); err != nil {
		return nil, err
	}

	content := event.ToString()
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
