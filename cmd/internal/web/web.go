package web

import (
	"fmt"
	"log"
	"net/http"
)

func StartWebServer() {
	mux := http.NewServeMux()
	mux.HandleFunc("/", handleDashboard)
	mux.HandleFunc("/login", handleLogin)

	log.Println("Web server started on :8080")
	http.ListenAndServe(":8080", mux)
}

func handleDashboard(w http.ResponseWriter, r *http.Request) {
	fmt.Fprintf(w, "Dashboard")
}

func handleLogin(w http.ResponseWriter, r *http.Request) {
	fmt.Fprintf(w, "Login Page")
}
