// internal/web/handlers.go
package web

import (
	"encoding/json"
	"fmt"
	"html/template"
	"net"
	"net/http"
	"os"
	"os/exec"
	"strconv"
	"sync"
	"time"

	"github.com/AsaDevZA/LinuxGoTranslator/internal/config"
	"github.com/AsaDevZA/LinuxGoTranslator/internal/license"
	"github.com/gorilla/mux"
	"github.com/gorilla/websocket"
	"gopkg.in/yaml.v3"
)

var (
	upgrader = websocket.Upgrader{CheckOrigin: func(r *http.Request) bool { return true }}

	clients   = make(map[*websocket.Conn]bool)
	clientsMu sync.Mutex

	messages   []Message
	messagesMu sync.Mutex

	cfgGlobal         *config.Config
	configPath        string
	licenseDataGlobal *license.LicenseData

	maxMsgs = 20
)

type Message struct {
	Timestamp string `json:"timestamp"`
	Provider  string `json:"provider"`
	Content   string `json:"content"`
}

func SetGlobalConfig(cfg *config.Config, path string) {
	cfgGlobal = cfg
	configPath = path

	licData, err := license.Validate(cfg.License)
	if err == nil {
		licenseDataGlobal = licData
	}
}

// Broadcast sends a message to all connected WebSocket clients
func Broadcast(content string, provider string) {
	m := Message{time.Now().Format("15:04:05"), provider, content}
	messagesMu.Lock()
	messages = append(messages, m)
	if len(messages) > maxMsgs {
		messages = messages[len(messages)-maxMsgs:]
	}
	messagesMu.Unlock()

	data, _ := json.Marshal(m)
	clientsMu.Lock()
	for c := range clients {
		_ = c.WriteMessage(websocket.TextMessage, data)
	}
	clientsMu.Unlock()
}

// wsHandler handles WebSocket connections for live dashboard
func wsHandler(w http.ResponseWriter, r *http.Request) {
	conn, err := upgrader.Upgrade(w, r, nil)
	if err != nil {
		return
	}
	clientsMu.Lock()
	clients[conn] = true
	clientsMu.Unlock()

	messagesMu.Lock()
	for _, m := range messages {
		data, _ := json.Marshal(m)
		conn.WriteMessage(websocket.TextMessage, data)
	}
	messagesMu.Unlock()

	defer func() {
		clientsMu.Lock()
		delete(clients, conn)
		clientsMu.Unlock()
		conn.Close()
	}()

	for {
		if _, _, err := conn.ReadMessage(); err != nil {
			break
		}
	}
}

// basicAuth wraps handlers with HTTP Basic Authentication
func basicAuth(next http.HandlerFunc) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		u, p, ok := r.BasicAuth()
		if !ok || u != cfgGlobal.Admin.Username || p != cfgGlobal.Admin.Password {
			w.Header().Set("WWW-Authenticate", `Basic realm="Restricted"`)
			http.Error(w, "Unauthorized", http.StatusUnauthorized)
			return
		}
		next(w, r)
	}
}

// ──────────────────────────────
// Dashboard Handler (/)
// ──────────────────────────────
func dashboardHandler(w http.ResponseWriter, r *http.Request) {
	tmpl := template.Must(template.New("dash").Parse(dashboardHTML))

	posChannels := 0
	scaleChannels := 0
	lprChannels := 0
	advancedReporting := false
	cloudStorage := false
	storeType := "Unlicensed"

	if licenseDataGlobal != nil {
		posChannels = licenseDataGlobal.POSChannels
		scaleChannels = licenseDataGlobal.ScaleChannels
		lprChannels = licenseDataGlobal.LPRChannels
		advancedReporting = licenseDataGlobal.AdvancedReporting
		cloudStorage = licenseDataGlobal.CloudStorage
		storeType = license.GetStoreTypeName(licenseDataGlobal.StoreType)
	}

	data := struct {
		Fingerprint       string
		POSChannels       int
		ScaleChannels     int
		LPRChannels       int
		AdvancedReporting bool
		CloudStorage      bool
		StoreType         string
	}{
		Fingerprint:       cfgGlobal.License.MachineFingerprint,
		POSChannels:       posChannels,
		ScaleChannels:     scaleChannels,
		LPRChannels:       lprChannels,
		AdvancedReporting: advancedReporting,
		CloudStorage:      cloudStorage,
		StoreType:         storeType,
	}
	tmpl.Execute(w, data)
}

// ──────────────────────────────
// Config Handler (/config)
// ──────────────────────────────
func configHandler(w http.ResponseWriter, r *http.Request) {
	if r.Method == "POST" {
		if err := r.ParseForm(); err != nil {
			http.Error(w, "Bad form", 400)
			return
		}

		nvrIP := r.FormValue("cctv_nvr_ip")
		if !isValidIP(nvrIP) {
			http.Error(w, "Invalid NVR IP address", 400)
			return
		}

		multicastIP := r.FormValue("vbspos_multicast_ip")
		if !isValidIP(multicastIP) {
			http.Error(w, "Invalid Multicast IP address", 400)
			return
		}

		newCfg := config.Config{
			Web:    config.WebConfig{Port: atoi(r.FormValue("web_port"), cfgGlobal.Web.Port)},
			CCTV:   config.CCTVConfig{NVRIP: nvrIP, NVRPort: atoi(r.FormValue("cctv_nvr_port"), cfgGlobal.CCTV.NVRPort)},
			DSTORE: config.DSTOREConfig{ServerMainPort: atoi(r.FormValue("dstore_main"), cfgGlobal.DSTORE.ServerMainPort), ServerSecondPort: atoi(r.FormValue("dstore_second"), cfgGlobal.DSTORE.ServerSecondPort), ServerAuxPort: atoi(r.FormValue("dstore_aux"), cfgGlobal.DSTORE.ServerAuxPort)},
			VBSPOS: config.VBSPOSConfig{MulticastIP: multicastIP, VbsMainPort: atoi(r.FormValue("vbspos_port"), cfgGlobal.VBSPOS.VbsMainPort)},
			License: config.LicenseConfig{
				MachineFingerprint: r.FormValue("license_fingerprint"),
				UnlockKey:          r.FormValue("unlock_key"),
			},
			IPMaps:      cfgGlobal.IPMaps,
			DeviceTypes: cfgGlobal.DeviceTypes,
			SiteDetails: cfgGlobal.SiteDetails,
			Admin:       cfgGlobal.Admin,
		}

		data, _ := yaml.Marshal(newCfg)
		os.WriteFile(configPath, data, 0644)

		w.Header().Set("Content-Type", "text/html")
		w.Write([]byte(configSavedHTML))
		go restartService()
		return
	}

	tmpl := template.Must(template.New("config").Parse(configHTML))
	tmpl.Execute(w, map[string]interface{}{
		"WebPort":            cfgGlobal.Web.Port,
		"CCTVNVRIP":          cfgGlobal.CCTV.NVRIP,
		"CCTVNVRPort":        cfgGlobal.CCTV.NVRPort,
		"DSTOREMain":         cfgGlobal.DSTORE.ServerMainPort,
		"DSTORESecond":       cfgGlobal.DSTORE.ServerSecondPort,
		"DSTOREAux":          cfgGlobal.DSTORE.ServerAuxPort,
		"VBSPOSMulticastIP":  cfgGlobal.VBSPOS.MulticastIP,
		"VBSPOSPort":         cfgGlobal.VBSPOS.VbsMainPort,
		"LicenseFingerprint": cfgGlobal.License.MachineFingerprint,
		"UnlockKey":          cfgGlobal.License.UnlockKey,
	})
}

// ──────────────────────────────
// Mappings Handler (/mappings)
// ──────────────────────────────
func mappingsHandler(w http.ResponseWriter, r *http.Request) {
	if r.Method == "POST" {
		if err := r.ParseMultipartForm(32 << 20); err != nil {
			if err := r.ParseForm(); err != nil {
				http.Error(w, "Bad form", 400)
				return
			}
		}

		action := r.FormValue("action")

		if action == "add" {
			if len(cfgGlobal.IPMaps) >= getMaxMappings() {
				w.Header().Set("Content-Type", "application/json")
				w.WriteHeader(http.StatusBadRequest)
				json.NewEncoder(w).Encode(map[string]string{"error": fmt.Sprintf("Maximum %d mappings reached", getMaxMappings())})
				return
			}

			newIP := r.FormValue("new_ip")
			if !isValidIP(newIP) {
				w.Header().Set("Content-Type", "application/json")
				w.WriteHeader(400)
				json.NewEncoder(w).Encode(map[string]string{"error": "Invalid IP address"})
				return
			}

			newPort := atoi(r.FormValue("new_port"), 0)
			if newPort < 1 || newPort > 65535 {
				w.Header().Set("Content-Type", "application/json")
				w.WriteHeader(400)
				json.NewEncoder(w).Encode(map[string]string{"error": "Invalid port number"})
				return
			}

			newType := atoi(r.FormValue("new_type"), 0)

			cfgGlobal.IPMaps = append(cfgGlobal.IPMaps, config.IPMap{
				IPAddress: newIP,
				Port:      newPort,
				Type:      newType,
			})

			w.Header().Set("Content-Type", "application/json")
			json.NewEncoder(w).Encode(map[string]string{"success": "Mapping added"})
			return

		} else if action == "delete" {
			index := atoi(r.FormValue("index"), -1)
			if index >= 0 && index < len(cfgGlobal.IPMaps) {
				cfgGlobal.IPMaps = append(cfgGlobal.IPMaps[:index], cfgGlobal.IPMaps[index+1:]...)
			}

			w.Header().Set("Content-Type", "application/json")
			json.NewEncoder(w).Encode(map[string]string{"success": "Mapping deleted"})
			return

		} else if action == "save" {
			var newMaps []config.IPMap
			for i := 0; i < len(cfgGlobal.IPMaps); i++ {
				ipKey := "ip_" + strconv.Itoa(i)
				portKey := "port_" + strconv.Itoa(i)
				typeKey := "type_" + strconv.Itoa(i)

				ip := r.FormValue(ipKey)
				if !isValidIP(ip) {
					http.Error(w, "Invalid IP address at index "+strconv.Itoa(i), 400)
					return
				}

				port := atoi(r.FormValue(portKey), 0)
				if port < 1 || port > 65535 {
					http.Error(w, "Invalid port number at index "+strconv.Itoa(i), 400)
					return
				}

				mapType := atoi(r.FormValue(typeKey), 0)

				newMaps = append(newMaps, config.IPMap{
					IPAddress: ip,
					Port:      port,
					Type:      mapType,
				})
			}
			cfgGlobal.IPMaps = newMaps

			newCfg := config.Config{
				Web:         cfgGlobal.Web,
				CCTV:        cfgGlobal.CCTV,
				DSTORE:      cfgGlobal.DSTORE,
				VBSPOS:      cfgGlobal.VBSPOS,
				License:     cfgGlobal.License,
				IPMaps:      cfgGlobal.IPMaps,
				DeviceTypes: cfgGlobal.DeviceTypes,
				SiteDetails: cfgGlobal.SiteDetails,
				Admin:       cfgGlobal.Admin,
			}

			data, _ := yaml.Marshal(newCfg)
			os.WriteFile(configPath, data, 0644)

			w.Header().Set("Content-Type", "text/html")
			w.Write([]byte(configSavedHTML))
			go restartService()
			return
		}
	}

	tmpl := template.Must(template.New("mappings").Parse(mappingsHTML))
	tmpl.Execute(w, map[string]interface{}{
		"IPMaps":      cfgGlobal.IPMaps,
		"DeviceTypes": cfgGlobal.DeviceTypes,
		"MaxMaps":     getMaxMappings(),
		"CanAdd":      len(cfgGlobal.IPMaps) < getMaxMappings(),
		"MapCount":    len(cfgGlobal.IPMaps),
	})
}

// ──────────────────────────────
// Site Details Handler (/sitedetails)
// ──────────────────────────────
func siteDetailsHandler(w http.ResponseWriter, r *http.Request) {
	if r.Method == "POST" {
		if err := r.ParseForm(); err != nil {
			http.Error(w, "Bad form", 400)
			return
		}

		cfgGlobal.SiteDetails.SiteName = r.FormValue("site_name")
		cfgGlobal.SiteDetails.Branch = r.FormValue("branch")
		cfgGlobal.SiteDetails.SiteLocation = r.FormValue("site_location")
		cfgGlobal.SiteDetails.OwnerName = r.FormValue("owner_name")
		cfgGlobal.SiteDetails.OwnerSurname = r.FormValue("owner_surname")
		cfgGlobal.SiteDetails.ContactCell = r.FormValue("contact_cell")
		cfgGlobal.SiteDetails.ContactEmail = r.FormValue("contact_email")

		newCfg := config.Config{
			Web:         cfgGlobal.Web,
			CCTV:        cfgGlobal.CCTV,
			DSTORE:      cfgGlobal.DSTORE,
			VBSPOS:      cfgGlobal.VBSPOS,
			License:     cfgGlobal.License,
			IPMaps:      cfgGlobal.IPMaps,
			DeviceTypes: cfgGlobal.DeviceTypes,
			SiteDetails: cfgGlobal.SiteDetails,
			Admin:       cfgGlobal.Admin,
		}

		data, _ := yaml.Marshal(newCfg)
		os.WriteFile(configPath, data, 0644)

		w.Header().Set("Content-Type", "text/html")
		w.Write([]byte(configSavedHTML))
		go restartService()
		return
	}

	tmpl := template.Must(template.New("sitedetails").Parse(siteDetailsHTML))
	tmpl.Execute(w, map[string]interface{}{
		"SiteName":     cfgGlobal.SiteDetails.SiteName,
		"Branch":       cfgGlobal.SiteDetails.Branch,
		"SiteLocation": cfgGlobal.SiteDetails.SiteLocation,
		"OwnerName":    cfgGlobal.SiteDetails.OwnerName,
		"OwnerSurname": cfgGlobal.SiteDetails.OwnerSurname,
		"ContactCell":  cfgGlobal.SiteDetails.ContactCell,
		"ContactEmail": cfgGlobal.SiteDetails.ContactEmail,
	})
}

// ──────────────────────────────
// Helpers
// ──────────────────────────────
func atoi(s string, def int) int {
	v, err := strconv.Atoi(s)
	if err != nil {
		return def
	}
	return v
}

func isValidIP(ip string) bool {
	return net.ParseIP(ip) != nil
}

func restartService() {
	time.Sleep(800 * time.Millisecond)
	cmd := exec.Command(os.Args[0], os.Args[1:]...)
	cmd.Stdout = os.Stdout
	cmd.Stderr = os.Stderr
	cmd.Stdin = os.Stdin
	cmd.Start()
	os.Exit(0)
}

func getMaxMappings() int {
	if licenseDataGlobal != nil {
		return licenseDataGlobal.ScaleChannels
	}
	return 64
}

// ──────────────────────────────
// Setup Handlers
// ──────────────────────────────
func SetupHandlers(m *http.ServeMux, cfg *config.Config) {
	SetGlobalConfig(cfg, "configs/default.yaml")
	r := mux.NewRouter()
	r.HandleFunc("/", dashboardHandler)
	r.HandleFunc("/ws", wsHandler)
	r.HandleFunc("/config", basicAuth(configHandler))
	r.HandleFunc("/mappings", basicAuth(mappingsHandler))
	r.HandleFunc("/sitedetails", basicAuth(siteDetailsHandler))
	m.Handle("/", r)
}

// ──────────────────────────────
// HTML Templates (inline)
// ──────────────────────────────

const dashboardHTML = `<!DOCTYPE html>
<html lang="en">
<head>
<meta charset="UTF-8">
<title>Live Dashboard</title>
<style>
body{font-family:'Segoe UI',Arial,sans-serif;background:#0d1117;color:#e6edf3;margin:0;display:flex;min-height:100vh;}
nav{width:240px;background:#161b22;border-right:1px solid #30363d;padding:24px;}
nav h3{color:#58a6ff;margin-bottom:24px;font-size:1.4rem;}
nav a{color:#8b949e;display:block;margin:16px 0;text-decoration:none;font-size:1.1rem;}
nav a:hover{color:#58a6ff;}
main{flex:1;padding:32px;max-width:1200px;}
h1{color:#58a6ff;margin:0 0 24px;}
.license-info{display:grid;grid-template-columns:repeat(auto-fit,minmax(200px,1fr));gap:16px;margin-bottom:32px;}
.license-card{background:#161b22;border:1px solid #30363d;border-radius:8px;padding:16px;display:flex;align-items:center;gap:12px;}
.license-card svg{flex-shrink:0;}
.license-card .info{display:flex;flex-direction:column;}
.license-card .label{color:#8b949e;font-size:0.85rem;margin-bottom:4px;}
.license-card .value{color:#e6edf3;font-size:1.1rem;font-weight:600;}
#log{background:#161b22;border:1px solid #30363d;border-radius:10px;padding:20px;height:70vh;overflow-y:auto;}
.msg{margin:12px 0;padding:12px;border-radius:8px;background:#0d1117;}
.spar{border-left:4px solid #238636;}
.vbspos{border-left:4px solid #fefe0bff;}
.ashida{border-left:4px solid #b083e9;}
time{color:#8b949e;font-size:0.9rem;}
</style>
</head>
<body>
<nav>
<h3>POS Translator</h3>
<a href="/">Live Dashboard</a>
<a href="/config">Site Config</a>
<a href="/sitedetails">Site Details</a>
<a href="/mappings">NVR Mappings</a>
</nav>
<main>
<h1>Live POS Events - {{.StoreType}}</h1>
<div class="license-info">
<div class="license-card">
<svg width="32" height="32" viewBox="0 0 32 32" fill="none"><ellipse cx="16" cy="10" rx="6" ry="7" stroke="#58a6ff" stroke-width="2" fill="none"/><path d="M10 16c0-3 2.5-5 6-5s6 2 6 5v8c0 2-1 3-3 3h-6c-2 0-3-1-3-3v-8z" stroke="#58a6ff" stroke-width="2" fill="none"/><path d="M13 18h1m4 0h1m-6 3h1m4 0h1m-6 3h1m4 0h1" stroke="#58a6ff" stroke-width="1.5"/></svg>
<div class="info">
<div class="label">Fingerprint</div>
<div class="value" style="color:#58a6ff;">{{.Fingerprint}}</div>
</div>
</div>
<div class="license-card">
<svg width="32" height="32" viewBox="0 0 32 32" fill="none"><rect x="8" y="4" width="16" height="24" rx="2" stroke="#238636" stroke-width="2" fill="none"/><rect x="11" y="8" width="10" height="12" rx="1" stroke="#238636" stroke-width="1.5" fill="none"/><line x1="13" y1="11" x2="19" y2="11" stroke="#238636" stroke-width="1.5"/><line x1="13" y1="14" x2="19" y2="14" stroke="#238636" stroke-width="1.5"/><line x1="13" y1="17" x2="17" y2="17" stroke="#238636" stroke-width="1.5"/><rect x="13" y="23" width="6" height="2" rx="1" fill="#238636"/></svg>
<div class="info">
<div class="label">POS Channels</div>
<div class="value" style="color:#238636;">{{.POSChannels}}</div>
</div>
</div>
<div class="license-card">
<svg width="32" height="32" viewBox="0 0 32 32" fill="none"><rect x="6" y="12" width="20" height="12" rx="2" stroke="#f85149" stroke-width="2" fill="none"/><path d="M10 12v-2c0-3 2-5 6-5s6 2 6 5v2" stroke="#f85149" stroke-width="2" fill="none"/><rect x="10" y="16" width="4" height="4" rx="1" fill="#f85149"/><rect x="18" y="16" width="4" height="4" rx="1" fill="#f85149"/><line x1="14" y1="18" x2="18" y2="18" stroke="#f85149" stroke-width="2"/></svg>
<div class="info">
<div class="label">Scale Channels</div>
<div class="value" style="color:#f85149;">{{.ScaleChannels}}</div>
</div>
</div>
<div class="license-card">
<svg width="32" height="32" viewBox="0 0 32 32" fill="none"><rect x="4" y="10" width="24" height="12" rx="2" stroke="#e6edf3" stroke-width="2" fill="none"/><rect x="8" y="14" width="4" height="4" rx="0.5" fill="#e6edf3"/><rect x="13" y="14" width="2" height="4" rx="0.5" fill="#e6edf3"/><rect x="16" y="14" width="4" height="4" rx="0.5" fill="#e6edf3"/><rect x="21" y="14" width="3" height="4" rx="0.5" fill="#e6edf3"/></svg>
<div class="info">
<div class="label">LPR Channels</div>
<div class="value" style="color:#e6edf3;">{{.LPRChannels}}</div>
</div>
</div>
{{if .AdvancedReporting}}
<div class="license-card">
<svg width="32" height="32" viewBox="0 0 32 32" fill="none"><rect x="6" y="4" width="20" height="24" rx="2" stroke="#a371f7" stroke-width="2" fill="none"/><line x1="10" y1="10" x2="22" y2="10" stroke="#a371f7" stroke-width="1.5"/><line x1="10" y1="14" x2="22" y2="14" stroke="#a371f7" stroke-width="1.5"/><line x1="10" y1="18" x2="18" y2="18" stroke="#a371f7" stroke-width="1.5"/><rect x="10" y="21" width="4" height="4" fill="#a371f7"/><rect x="15" y="23" width="4" height="2" fill="#a371f7"/><rect x="20" y="22" width="2" height="3" fill="#a371f7"/></svg>
<div class="info">
<div class="label">Advanced Reporting</div>
<div class="value" style="color:#a371f7;">Enabled</div>
</div>
</div>
{{end}}
{{if .CloudStorage}}
<div class="license-card">
<svg width="32" height="32" viewBox="0 0 32 32" fill="none"><path d="M24 18c2.2 0 4-1.8 4-4s-1.8-4-4-4c0-3.3-2.7-6-6-6-2.6 0-4.8 1.7-5.6 4-2.8 0.2-5 2.5-5 5.4 0 3 2.4 5.4 5.4 5.4h11.2z" stroke="#ffa657" stroke-width="2" fill="none"/><path d="M16 22v6m-3-3l3 3 3-3" stroke="#ffa657" stroke-width="2"/></svg>
<div class="info">
<div class="label">Cloud Storage</div>
<div class="value" style="color:#ffa657;">Enabled</div>
</div>
</div>
{{end}}
</div>
<div id="log"></div>
</main>
<script>
const protocol = window.location.protocol === 'https:' ? 'wss:' : 'ws:';
const ws=new WebSocket(protocol+'//'+location.host+'/ws');
const log=document.getElementById('log');
ws.onmessage=e=>{
 const m=JSON.parse(e.data);
 const div=document.createElement('div');
 div.className='msg '+m.provider;
 div.innerHTML='<time>'+m.timestamp+'</time> <b>['+m.provider+']</b><br><pre>'+m.content.replace(/</g,'&lt;').replace(/>/g,'&gt;')+'</pre>';
 log.appendChild(div);
 log.scrollTop=log.scrollHeight;
};
</script>
</body>
</html>`

const configHTML = `<!DOCTYPE html>
<html lang="en">
<head>
<meta charset="UTF-8">
<title>Site Configuration</title>
<style>
body{font-family:'Segoe UI',Arial,sans-serif;background:#0d1117;color:#e6edf3;margin:0;display:flex;min-height:100vh;}
nav{width:240px;background:#161b22;border-right:1px solid #30363d;padding:24px;}
nav h3{color:#58a6ff;margin-bottom:24px;font-size:1.4rem;}
nav a{color:#8b949e;display:block;margin:16px 0;text-decoration:none;font-size:1.1rem;}
nav a:hover{color:#58a6ff;}
main{flex:1;padding:32px;max-width:900px;}
h1{color:#58a6ff;margin:0 0 32px;}
form{display:grid;grid-template-columns:1fr 2fr;gap:16px;}
label{justify-self:end;align-self:center;font-weight:600;color:#8b949e;}
input{padding:10px;background:#0d1117;color:#e6edf3;border:1px solid #30363d;border-radius:6px;font-size:1rem;width:100%;}
textarea{padding:10px;background:#0d1117;color:#e6edf3;border:1px solid #30363d;border-radius:6px;font-size:1rem;width:100%;font-family:monospace;}
.buttons{grid-column:1 / -1;text-align:center;margin-top:32px;}
button{padding:12px 28px;margin:0 12px;border:none;border-radius:8px;cursor:pointer;font-weight:600;}
button[type=submit]{background:#238636;color:#fff;}
button.cancel{background:#30363d;color:#e6edf3;}
button:hover{opacity:0.9;}
</style>
</head>
<body>
<nav>
<h3>POS Translator</h3>
<a href="/">Live Dashboard</a>
<a href="/config">Site Config</a>
<a href="/sitedetails">Site Details</a>
<a href="/mappings">NVR Mappings</a>
</nav>
<main>
<h1>Site Configuration</h1>
<form method="POST">
<h3 style="grid-column:1 / -1;color:#8b949e;margin:24px 0 8px;">Web</h3>
<label>Port:</label><input type="number" name="web_port" value="{{.WebPort}}" required min="1" max="65535">

<h3 style="grid-column:1 / -1;color:#8b949e;margin:24px 0 8px;">CCTV</h3>
<label>NVR IP:</label><input type="text" name="cctv_nvr_ip" value="{{.CCTVNVRIP}}" required>
<label>NVR Port:</label><input type="number" name="cctv_nvr_port" value="{{.CCTVNVRPort}}" required min="1" max="65535">

<h3 style="grid-column:1 / -1;color:#8b949e;margin:24px 0 8px;">DSTORE</h3>
<label>Main Port:</label><input type="number" name="dstore_main" value="{{.DSTOREMain}}" min="1" max="65535">
<label>Second Port:</label><input type="number" name="dstore_second" value="{{.DSTORESecond}}" min="1" max="65535">
<label>Aux Port:</label><input type="number" name="dstore_aux" value="{{.DSTOREAux}}" min="1" max="65535">

<h3 style="grid-column:1 / -1;color:#8b949e;margin:24px 0 8px;">VBS POS</h3>
<label>Multicast IP:</label><input type="text" name="vbspos_multicast_ip" value="{{.VBSPOSMulticastIP}}" required>
<label>Port:</label><input type="number" name="vbspos_port" value="{{.VBSPOSPort}}" required min="1" max="65535">

<h3 style="grid-column:1 / -1;color:#8b949e;margin:24px 0 8px;">License</h3>
<label>Fingerprint:</label><input type="text" name="license_fingerprint" value="{{.LicenseFingerprint}}" readonly style="background:#0d1117;cursor:not-allowed;">
<label>Unlock Key:</label><textarea name="unlock_key" rows="3">{{.UnlockKey}}</textarea>

<div class="buttons">
<button type="submit">Save & Restart</button>
<button type="button" class="cancel" onclick="window.location.href='/'">Cancel</button>
</div>
</form>
</main>
</body>
</html>`

const configSavedHTML = `<!DOCTYPE html>
<html lang="en">
<head>
<meta charset="UTF-8">
<title>Configuration Updated</title>
<style>
body{font-family:'Segoe UI',Arial,sans-serif;background:#0d1117;color:#e6edf3;margin:0;display:flex;min-height:100vh;}
nav{width:240px;background:#161b22;border-right:1px solid #30363d;padding:24px;}
nav h3{color:#58a6ff;margin-bottom:24px;font-size:1.4rem;}
nav a{color:#8b949e;display:block;margin:16px 0;text-decoration:none;font-size:1.1rem;}
nav a:hover{color:#58a6ff;}
main{flex:1;padding:32px;display:flex;justify-content:center;align-items:center;}
.container{background:#161b22;border-radius:12px;padding:40px;max-width:500px;text-align:center;box-shadow:0 10px 30px rgba(0,0,0,0.5);border:1px solid #30363d;}
h1{color:#58a6ff;margin-bottom:20px;font-size:1.8rem;}
p{font-size:1.1rem;margin:15px 0;color:#8b949e;}
.highlight{color:#f78166;font-weight:600;}
</style>
</head>
<body>
<nav>
<h3>POS Translator</h3>
<a href="/">Live Dashboard</a>
<a href="/config">Site Config</a>
<a href="/sitedetails">Site Details</a>
<a href="/mappings">NVR Mappings</a>
</nav>
<main>
<div class="container">
<h1>Configuration Updated</h1>
<p>Changes saved successfully.</p>
<p>Service is restarting...</p>
<p class="highlight">Please refresh the page manually in a few seconds (use the new port if it was changed).</p>
</div>
</main>
</body>
</html>`

const mappingsHTML = `<!DOCTYPE html>
<html lang="en">
<head>
<meta charset="UTF-8">
<title>NVR Mappings</title>
<style>
body{font-family:'Segoe UI',Arial,sans-serif;background:#0d1117;color:#e6edf3;margin:0;display:flex;min-height:100vh;}
nav{width:240px;background:#161b22;border-right:1px solid #30363d;padding:24px;}
nav h3{color:#58a6ff;margin-bottom:24px;font-size:1.4rem;}
nav a{color:#8b949e;display:block;margin:16px 0;text-decoration:none;font-size:1.1rem;}
nav a:hover{color:#58a6ff;}
main{flex:1;padding:32px;max-width:1200px;}
h1{color:#58a6ff;margin:0 0 24px;}
.info{color:#8b949e;margin-bottom:24px;font-size:1rem;}
.message{padding:12px 20px;border-radius:8px;margin-bottom:16px;font-weight:600;}
.message.success{background:#238636;color:#fff;}
.message.error{background:#da3633;color:#fff;}
form{margin-bottom:32px;}
table{width:100%;border-collapse:collapse;margin-bottom:24px;}
th,td{padding:12px;border:1px solid #30363d;}
th{background:#161b22;color:#58a6ff;font-weight:600;}
td{background:#0d1117;color:#e6edf3;}
input[type=text],input[type=number],select{padding:8px;background:#0d1117;color:#e6edf3;border:1px solid #30363d;border-radius:6px;font-size:0.95rem;width:100%;}
select{cursor:pointer;}
select option{background:#0d1117;color:#e6edf3;}
button{padding:10px 24px;margin:4px;border:none;border-radius:8px;cursor:pointer;font-weight:600;font-size:0.95rem;}
button[type=submit]{background:#238636;color:#fff;}
button.delete{background:#da3633;color:#fff;padding:8px 16px;}
button.add{background:#1f6feb;color:#fff;}
button.cancel{background:#30363d;color:#e6edf3;}
button:hover{opacity:0.9;}
button:disabled{opacity:0.5;cursor:not-allowed;}
.add-section{background:#161b22;border:1px solid #30363d;border-radius:10px;padding:24px;margin-bottom:24px;}
.add-section h2{color:#58a6ff;margin:0 0 16px;font-size:1.3rem;}
.add-form{display:grid;grid-template-columns:2fr 1fr 2fr auto;gap:12px;align-items:end;}
.add-form label{display:block;color:#8b949e;font-weight:600;margin-bottom:6px;font-size:0.9rem;}
.form-group{display:flex;flex-direction:column;}
</style>
</head>
<body>
<nav>
<h3>POS Translator</h3>
<a href="/">Live Dashboard</a>
<a href="/config">Site Config</a>
<a href="/sitedetails">Site Details</a>
<a href="/mappings">NVR Mappings</a>
</nav>
<main>
<h1>NVR Mappings</h1>
<div id="messageArea"></div>
<div class="info">Mappings: <span id="mapCount">{{.MapCount}}</span> / {{.MaxMaps}}</div>

{{if .CanAdd}}
<div class="add-section" id="addSection">
<h2>Add New Mapping</h2>
<form id="addForm" class="add-form" onsubmit="addMapping(event)">
<div class="form-group">
<label>IP Address</label>
<input type="text" id="new_ip" name="new_ip" placeholder="192.168.1.100" required pattern="^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$">
</div>
<div class="form-group">
<label>Port</label>
<input type="number" id="new_port" name="new_port" placeholder="8000" required min="1" max="65535">
</div>
<div class="form-group">
<label>Device Type</label>
<select id="new_type" name="new_type" required>
<option value="">Select Type...</option>
{{range .DeviceTypes}}
<option value="{{.ID}}">{{.Name}} - {{.Description}}</option>
{{end}}
</select>
</div>
<button type="submit" class="add">Add Mapping</button>
</form>
</div>
{{else}}
<div class="info" style="color:#f78166;">Maximum number of mappings ({{.MaxMaps}}) reached. Delete a mapping to add a new one.</div>
{{end}}

<form method="POST" id="saveForm">
<input type="hidden" name="action" value="save">
<table id="mappingsTable">
<tr>
<th>IP Address</th>
<th>Port</th>
<th>Device Type</th>
<th>Actions</th>
</tr>
{{range $index, $map := .IPMaps}}
<tr data-index="{{$index}}">
<td><input type="text" name="ip_{{$index}}" value="{{$map.IPAddress}}" required pattern="^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$"></td>
<td><input type="number" name="port_{{$index}}" value="{{$map.Port}}" required min="1" max="65535"></td>
<td>
<select name="type_{{$index}}" required>
{{range $.DeviceTypes}}
<option value="{{.ID}}" {{if eq .ID $map.Type}}selected{{end}}>{{.Name}} - {{.Description}}</option>
{{end}}
</select>
</td>
<td>
<button type="button" class="delete" onclick="deleteMapping({{$index}})">Delete</button>
</td>
</tr>
{{end}}
</table>
<button type="submit">Save & Restart Service</button>
<button type="button" class="cancel" onclick="window.location.href='/'">Cancel</button>
</form>
</main>
<script>
function showMessage(msg, isError) {
const area = document.getElementById('messageArea');
const div = document.createElement('div');
div.className = 'message ' + (isError ? 'error' : 'success');
div.textContent = msg;
area.appendChild(div);
setTimeout(() => div.remove(), 5000);
}

function addMapping(event) {
event.preventDefault();
const form = document.getElementById('addForm');
const formData = new FormData(form);
formData.append('action', 'add');

fetch('/mappings', {
method: 'POST',
body: formData
})
.then(res => {
if (!res.ok) {
return res.json().then(data => {
throw new Error(data.error || 'Failed to add mapping');
});
}
return res.json();
})
.then(data => {
if (data.error) {
showMessage(data.error, true);
} else {
showMessage('Mapping added successfully', false);
form.reset();
setTimeout(() => location.reload(), 1000);
}
})
.catch(err => showMessage(err.message || 'Error adding mapping', true));
}

function deleteMapping(index) {
if (!confirm('Are you sure you want to delete this mapping?')) return;

const formData = new FormData();
formData.append('action', 'delete');
formData.append('index', index);

fetch('/mappings', {
method: 'POST',
body: formData
})
.then(res => {
if (!res.ok) {
return res.json().then(data => {
throw new Error(data.error || 'Failed to delete mapping');
});
}
return res.json();
})
.then(data => {
if (data.error) {
showMessage(data.error, true);
} else {
showMessage('Mapping deleted successfully', false);
setTimeout(() => location.reload(), 1000);
}
})
.catch(err => showMessage(err.message || 'Error deleting mapping', true));
}
</script>
</body>
</html>`

const siteDetailsHTML = `<!DOCTYPE html>
<html lang="en">
<head>
<meta charset="UTF-8">
<title>Site Details</title>
<style>
body{font-family:'Segoe UI',Arial,sans-serif;background:#0d1117;color:#e6edf3;margin:0;display:flex;min-height:100vh;}
nav{width:240px;background:#161b22;border-right:1px solid #30363d;padding:24px;}
nav h3{color:#58a6ff;margin-bottom:24px;font-size:1.4rem;}
nav a{color:#8b949e;display:block;margin:16px 0;text-decoration:none;font-size:1.1rem;}
nav a:hover{color:#58a6ff;}
main{flex:1;padding:32px;max-width:900px;}
h1{color:#58a6ff;margin:0 0 32px;}
form{display:grid;grid-template-columns:1fr 2fr;gap:16px;}
label{justify-self:end;align-self:center;font-weight:600;color:#8b949e;}
input{padding:10px;background:#0d1117;color:#e6edf3;border:1px solid #30363d;border-radius:6px;font-size:1rem;width:100%;}
.buttons{grid-column:1 / -1;text-align:center;margin-top:32px;}
button{padding:12px 28px;margin:0 12px;border:none;border-radius:8px;cursor:pointer;font-weight:600;}
button[type=submit]{background:#238636;color:#fff;}
button.cancel{background:#30363d;color:#e6edf3;}
button:hover{opacity:0.9;}
</style>
</head>
<body>
<nav>
<h3>POS Translator</h3>
<a href="/">Live Dashboard</a>
<a href="/config">Site Config</a>
<a href="/sitedetails">Site Details</a>
<a href="/mappings">NVR Mappings</a>
</nav>
<main>
<h1>Site Details</h1>
<form method="POST">
<h3 style="grid-column:1 / -1;color:#8b949e;margin:24px 0 8px;">Site Information</h3>
<label>Site Name:</label><input type="text" name="site_name" value="{{.SiteName}}" required minlength="1">
<label>Branch:</label><input type="text" name="branch" value="{{.Branch}}" required minlength="1">
<label>Site Location:</label><input type="text" name="site_location" value="{{.SiteLocation}}" required minlength="1">

<h3 style="grid-column:1 / -1;color:#8b949e;margin:24px 0 8px;">Owner Details</h3>
<label>Name:</label><input type="text" name="owner_name" value="{{.OwnerName}}" required minlength="1">
<label>Surname:</label><input type="text" name="owner_surname" value="{{.OwnerSurname}}" required minlength="1">
<label>Contact Cell:</label><input type="tel" name="contact_cell" value="{{.ContactCell}}" required pattern="^[\+]?[(]?[0-9]{1,4}[)]?[-\s\.]?[(]?[0-9]{1,4}[)]?[-\s\.]?[0-9]{1,9}$" title="Please enter a valid phone number">
<label>Contact Email:</label><input type="email" name="contact_email" value="{{.ContactEmail}}" required pattern="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$" title="Please enter a valid email address">

<div class="buttons">
<button type="submit">Save & Restart</button>
<button type="button" class="cancel" onclick="window.location.href='/'">Cancel</button>
</div>
</form>
</main>
</body>
</html>`
