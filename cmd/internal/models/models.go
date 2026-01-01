package models

import "time"

type UniversalData struct {
	Timestamp time.Time
	Data      map[string]interface{}
}
