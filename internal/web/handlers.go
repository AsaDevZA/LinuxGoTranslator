// internal/web/handlers.go
package web

import (
	"net/http"

	"github.com/AsaDevZA/LinuxGoTranslator/internal/config"
)

func SetupHandlers(mux *http.ServeMux, cfg *config.Config) {
	mux.HandleFunc("/", func(w http.ResponseWriter, r *http.Request) {
		w.Header().Set("Content-Type", "text/html")
		w.Write([]byte(`
			<h1>CCTV Metadata Translator</h1>
			<p>Configuration dashboard (under construction)</p>
		`))
	})
}
