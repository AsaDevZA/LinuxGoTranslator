// internal/web/handlers.go
package web

import (
	"encoding/json"
	"html/template"
	"io/ioutil"
	"net/http"
	"os"
	"os/exec"
	"strconv"
	"sync"
	"time"

	"github.com/AsaDevZA/LinuxGoTranslator/internal/config"
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

	cfgGlobal  *config.Config
	configPath string

	adminUser = "admin"
	adminPass = "P@ssword01!"
	maxMsgs   = 20
)

type Message struct {
	Timestamp string `json:"timestamp"`
	Provider  string `json:"provider"`
	Content   string `json:"content"`
}

func SetGlobalConfig(cfg *config.Config, path string) {
	cfgGlobal = cfg
	configPath = path
}

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

func basicAuth(next http.HandlerFunc) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		u, p, ok := r.BasicAuth()
		if !ok || u != adminUser || p != adminPass {
			w.Header().Set("WWW-Authenticate", `Basic realm="Restricted"`)
			http.Error(w, "Unauthorized", http.StatusUnauthorized)
			return
		}
		next(w, r)
	}
}

func dashboardHandler(w http.ResponseWriter, r *http.Request) {
	tmpl := template.Must(template.New("dash").Parse(`
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>POS Translator – Live Dashboard</title>
  <style>
    body{font-family:'Segoe UI',Arial,sans-serif;background:#0d1117;color:#e6edf3;margin:0;display:flex;min-height:100vh;}
    nav{width:240px;background:#161b22;border-right:1px solid #30363d;padding:24px;}
    nav h3{color:#58a6ff;margin-bottom:24px;font-size:1.4rem;}
    nav a{color:#8b949e;display:block;margin:16px 0;text-decoration:none;font-size:1.1rem;}
    nav a:hover{color:#58a6ff;}
    main{flex:1;padding:32px;max-width:1200px;}
    h1{color:#58a6ff;margin:0 0 24px;}
    .license-info{color:#8b949e;margin-bottom:32px;font-size:1.1rem;}
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
    <a href="/mappings">NVR Mappings</a>
  </nav>
  <main>
    <h1>Live POS Events</h1>
    <div class="license-info">License: Max Devices {{.MaxDevices}} | Fingerprint: {{.Fingerprint}}</div>
    <div id="log"></div>
  </main>
  <script>
    const ws=new WebSocket('ws://'+location.host+'/ws');
    const log=document.getElementById('log');
    ws.onmessage=e=>{const m=JSON.parse(e.data);const div=document.createElement('div');div.className='msg '+m.provider;div.innerHTML='<time>'+m.timestamp+'</time> <b>['+m.provider+']</b><br><pre>'+m.content.replace(/</g,'&lt;').replace(/>/g,'&gt;')+'</pre>';log.appendChild(div);log.scrollTop=log.scrollHeight;}
  </script>
</body>
</html>`))

	data := struct {
		MaxDevices  int
		Fingerprint string
	}{
		MaxDevices:  cfgGlobal.License.MaxTotalDevices,
		Fingerprint: cfgGlobal.License.MachineFingerprint,
	}
	tmpl.Execute(w, data)
}

func configHandler(w http.ResponseWriter, r *http.Request) {
	if r.Method == "POST" {
		if err := r.ParseForm(); err != nil {
			http.Error(w, "Bad form", 400)
			return
		}

		newCfg := config.Config{
			Web:     config.WebConfig{Port: atoi(r.FormValue("web_port"), cfgGlobal.Web.Port)},
			CCTV:    config.CCTVConfig{NVRIP: r.FormValue("cctv_nvr_ip"), NVRPort: atoi(r.FormValue("cctv_nvr_port"), cfgGlobal.CCTV.NVRPort)},
			DSTORE:  config.DSTOREConfig{ServerMainPort: atoi(r.FormValue("dstore_main"), cfgGlobal.DSTORE.ServerMainPort), ServerSecondPort: atoi(r.FormValue("dstore_second"), cfgGlobal.DSTORE.ServerSecondPort), ServerAuxPort: atoi(r.FormValue("dstore_aux"), cfgGlobal.DSTORE.ServerAuxPort)},
			VBSPOS:  config.VBSPOSConfig{MulticastIP: r.FormValue("vbspos_multicast_ip"), VbsMainPort: atoi(r.FormValue("vbspos_port"), cfgGlobal.VBSPOS.VbsMainPort)},
			License: config.LicenseConfig{MaxTotalDevices: atoi(r.FormValue("license_max_total"), cfgGlobal.License.MaxTotalDevices), MaxPerScale: atoi(r.FormValue("license_max_scale"), cfgGlobal.License.MaxPerScale), MachineFingerprint: r.FormValue("license_fingerprint")},
		}

		data, _ := yaml.Marshal(newCfg)
		ioutil.WriteFile(configPath, data, 0644)

		w.Header().Set("Content-Type", "text/html")
		w.Write([]byte(`
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Configuration Updated</title>
  <style>
    body{font-family:'Segoe UI',Arial,sans-serif;background:#0d1117;color:#e6edf3;margin:0;display:flex;justify-content:center;align-items:center;min-height:100vh;}
    .container{background:#161b22;border-radius:12px;padding:40px;max-width:500px;text-align:center;box-shadow:0 10px 30px rgba(0,0,0,0.5);border:1px solid #30363d;}
    h1{color:#58a6ff;margin-bottom:20px;font-size:1.8rem;}
    p{font-size:1.1rem;margin:15px 0;color:#8b949e;}
    .highlight{color:#f78166;font-weight:600;}
  </style>
</head>
<body>
  <div class="container">
    <h1>Configuration Updated</h1>
    <p>Changes saved successfully.</p>
    <p>Service is restarting...</p>
    <p class="highlight">Please refresh the page manually in a few seconds<br>(use the new port if it was changed).</p>
  </div>
</body>
</html>`))
		go restartService()
		return
	}

	tmpl := template.Must(template.New("config").Parse(`
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
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
    <a href="/mappings">NVR Mappings</a>
  </nav>
  <main>
    <h1>Site Configuration</h1>
    <form method="POST">
      <h3 style="grid-column:1 / -1;color:#8b949e;margin:24px 0 8px;">Web</h3>
      <label>Port:</label><input type="number" name="web_port" value="{{.WebPort}}" required min="1" max="65535">

      <h3 style="grid-column:1 / -1;color:#8b949e;margin:24px 0 8px;">CCTV</h3>
      <label>NVR IP:</label><input type="text" name="cctv_nvr_ip" value="{{.CCTVNVRIP}}" required pattern="^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$" title="Valid IPv4 address">
      <label>NVR Port:</label><input type="number" name="cctv_nvr_port" value="{{.CCTVNVRPort}}" required min="1" max="65535">

      <h3 style="grid-column:1 / -1;color:#8b949e;margin:24px 0 8px;">DSTORE</h3>
      <label>Main Port:</label><input type="number" name="dstore_main" value="{{.DSTOREMain}}" min="1" max="65535">
      <label>Second Port:</label><input type="number" name="dstore_second" value="{{.DSTORESecond}}" min="1" max="65535">
      <label>Aux Port:</label><input type="number" name="dstore_aux" value="{{.DSTOREAux}}" min="1" max="65535">

      <h3 style="grid-column:1 / -1;color:#8b949e;margin:24px 0 8px;">VBS POS</h3>
      <label>Multicast IP:</label><input type="text" name="vbspos_multicast_ip" value="{{.VBSPOSMulticastIP}}" required pattern="^(?:22[4-9]|23[0-9])\.(?:[0-9]{1,3}\.){2}[0-9]{1,3}$" title="Valid multicast IPv4 (224.0.0.0 – 239.255.255.255)">
      <label>Port:</label><input type="number" name="vbspos_port" value="{{.VBSPOSPort}}" required min="1" max="65535">

      <h3 style="grid-column:1 / -1;color:#8b949e;margin:24px 0 8px;">License</h3>
      <label>Max Total Devices:</label><input type="number" name="license_max_total" value="{{.LicenseMaxTotal}}" required min="1">
      <label>Max Per Scale:</label><input type="number" name="license_max_scale" value="{{.LicenseMaxScale}}" required min="1">
      <label>Fingerprint:</label><input type="text" name="license_fingerprint" value="{{.LicenseFingerprint}}" required pattern="[0-9a-fA-F:]+">

      <div class="buttons">
        <button type="submit">Save & Restart</button>
        <button type="button" class="cancel" onclick="window.location.href='/'">Cancel</button>
      </div>
    </form>
  </main>
</body>
</html>`))

	tmpl.Execute(w, map[string]interface{}{
		"WebPort":            cfgGlobal.Web.Port,
		"CCTVNVRIP":          cfgGlobal.CCTV.NVRIP,
		"CCTVNVRPort":        cfgGlobal.CCTV.NVRPort,
		"DSTOREMain":         cfgGlobal.DSTORE.ServerMainPort,
		"DSTORESecond":       cfgGlobal.DSTORE.ServerSecondPort,
		"DSTOREAux":          cfgGlobal.DSTORE.ServerAuxPort,
		"VBSPOSMulticastIP":  cfgGlobal.VBSPOS.MulticastIP,
		"VBSPOSPort":         cfgGlobal.VBSPOS.VbsMainPort,
		"LicenseMaxTotal":    cfgGlobal.License.MaxTotalDevices,
		"LicenseMaxScale":    cfgGlobal.License.MaxPerScale,
		"LicenseFingerprint": cfgGlobal.License.MachineFingerprint,
	})
}

func mappingsHandler(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "text/html")
	w.Write([]byte(`
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>NVR Mappings</title>
  <style>
    body{font-family:'Segoe UI',Arial,sans-serif;background:#0d1117;color:#e6edf3;margin:0;display:flex;min-height:100vh;}
    nav{width:240px;background:#161b22;border-right:1px solid #30363d;padding:24px;}
    nav h3{color:#58a6ff;margin-bottom:24px;font-size:1.4rem;}
    nav a{color:#8b949e;display:block;margin:16px 0;text-decoration:none;font-size:1.1rem;}
    nav a:hover{color:#58a6ff;}
    main{flex:1;padding:32px;}
    h1{color:#58a6ff;margin:0 0 32px;}
  </style>
</head>
<body>
  <nav>
    <h3>POS Translator</h3>
    <a href="/">Live Dashboard</a>
    <a href="/config">Site Config</a>
    <a href="/mappings">NVR Mappings</a>
  </nav>
  <main>
    <h1>NVR Mappings</h1>
    <p>Coming soon...</p>
  </main>
</body>
</html>`))
}

func atoi(s string, def int) int {
	v, err := strconv.Atoi(s)
	if err != nil {
		return def
	}
	return v
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

func SetupHandlers(m *http.ServeMux, cfg *config.Config) {
	SetGlobalConfig(cfg, "configs/default.yaml")
	r := mux.NewRouter()
	r.HandleFunc("/", dashboardHandler)
	r.HandleFunc("/ws", wsHandler)
	r.HandleFunc("/config", basicAuth(configHandler))
	r.HandleFunc("/mappings", basicAuth(mappingsHandler))
	m.Handle("/", r)
}
