using CodiBloques.Arduino;
using CodiBloques.Services;
using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json;

namespace CodiBloques.Forms;

public partial class FormPrincipal : Form
{
    private readonly ProyectoService _proyectoService;
    private readonly ArduinoService  _arduino;
    private bool    _proyectoModificado = false;
    private string? _archivoActual = null;
    private bool    _enPantallaCompleta = false;
    private int     _splitterDistanciaAnterior = 550;
    private int     _nivelZoom = 100;

    public FormPrincipal()
    {
        InitializeComponent();
        _proyectoService = new ProyectoService();
        _arduino         = new ArduinoService();
        InicializarEventos();
        ConfigurarTitulo();
    }

    private void InicializarEventos()
    {
        this.FormClosing += FormPrincipal_FormClosing;
        webViewBlockly.NavigationCompleted += WebView_NavigationCompleted;
        this.KeyDown += FormPrincipal_KeyDown;
        this.Shown += (s, e) =>
        {
            splitDerecha.SplitterDistance = (int)(ClientSize.Height * 0.72);
        };
        _arduino.DatoRecibido  += (s, d) => AgregarConsola("[Arduino] " + d, Color.Cyan);
        _arduino.ErrorOcurrido += (s, err) => AgregarConsola("[Arduino Error] " + err, Color.Orange);
    }

    private void ConfigurarTitulo(string? nombreProyecto = null)
    {
        string base_ = "CodiBloques";
        this.Text = nombreProyecto != null
            ? base_ + " - " + nombreProyecto + (_proyectoModificado ? " *" : "")
            : base_ + " - Nuevo proyecto" + (_proyectoModificado ? " *" : "");
    }

    // ─── WebView ──────────────────────────────────────────────────────────────

    private async void FormPrincipal_Load(object sender, EventArgs e)
    {
        await InicializarWebView();
    }

    private async Task InicializarWebView()
    {
        try
        {
            string wwwroot = Path.Combine(Application.StartupPath, "wwwroot");
            string cache   = Path.Combine(Application.StartupPath, "WebView2Cache");

            var opciones = new CoreWebView2EnvironmentOptions
            {
                AdditionalBrowserArguments = "--disable-features=ThirdPartyStoragePartitioning"
            };

            var env = await CoreWebView2Environment.CreateAsync(null, cache, opciones);
            await webViewBlockly.EnsureCoreWebView2Async(env);

            webViewBlockly.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            webViewBlockly.CoreWebView2.Settings.IsScriptEnabled = true;
            webViewBlockly.CoreWebView2.Settings.IsWebMessageEnabled = true;
            webViewBlockly.CoreWebView2.Settings.AreDevToolsEnabled = true;

            webViewBlockly.CoreWebView2.SetVirtualHostNameToFolderMapping(
                "codiapp", wwwroot, CoreWebView2HostResourceAccessKind.Allow);

            webViewBlockly.CoreWebView2.WebMessageReceived += WebView_WebMessageReceived;
            webViewBlockly.CoreWebView2.Navigate("https://codiapp/index.html");
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Error al inicializar:\n" + ex.Message + "\n\nAsegurate de tener instalado Microsoft Edge WebView2 Runtime.",
                "CodiBloques - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void WebView_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
    {
        if (InvokeRequired) { Invoke(() => WebView_NavigationCompleted(sender, e)); return; }
        if (e.IsSuccess)
        {
            lblEstado.Text = "Listo";
            lblEstado.ForeColor = SystemColors.ControlText;
            btnEjecutar.Enabled = true;
            btnDepurar.Enabled  = true;
            ActualizarIndicadorArduino();
        }
        else
        {
            lblEstado.Text = "Error al cargar (" + e.WebErrorStatus + ")";
            lblEstado.ForeColor = Color.Red;
        }
    }

    // ─── Mensajes desde JS ────────────────────────────────────────────────────

    private async void WebView_WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
    {
        try
        {
            var msg = JsonConvert.DeserializeObject<dynamic>(e.WebMessageAsJson);
            if (msg == null) return;
            string tipo = msg.tipo?.ToString() ?? "";

            switch (tipo)
            {
                case "codigoGenerado":
                    MostrarCodigo(msg.codigo?.ToString() ?? "");
                    break;
                case "consolaMsg":
                    AgregarConsola(msg.texto?.ToString() ?? "", Color.LightGreen);
                    break;
                case "error":
                    AgregarConsola("❌ " + (msg.descripcion?.ToString() ?? ""), Color.Red);
                    MostrarError(msg.descripcion?.ToString() ?? "");
                    break;
                case "proyectoModificado":
                    _proyectoModificado = true;
                    if (InvokeRequired) Invoke(() => ConfigurarTitulo(_archivoActual));
                    else ConfigurarTitulo(_archivoActual);
                    break;
                case "arduino_conectar":
                    await ProcesarArduinoConectar(msg);
                    break;
                case "arduino_dw":
                    await ProcesarArduinoDW(msg);
                    break;
                case "arduino_dr":
                    await ProcesarArduinoDR(msg);
                    break;
                case "arduino_ar":
                    await ProcesarArduinoAR(msg);
                    break;
                case "arduino_aw":
                    await ProcesarArduinoAW(msg);
                    break;
                case "arduino_sv":
                    await ProcesarArduinoSV(msg);
                    break;
                case "arduino_enviar":
                    await ProcesarArduinoEnviar(msg);
                    break;
                case "arduino_leer":
                    await ProcesarArduinoLeer(msg);
                    break;
            }
        }
        catch { }
    }

    // ─── Handlers Arduino ─────────────────────────────────────────────────────

    private async Task ProcesarArduinoConectar(dynamic msg)
    {
        string puerto = msg.puerto?.ToString() ?? "";
        int baud = int.TryParse(msg.baud?.ToString(), out int b) ? b : 9600;
        bool ok = await _arduino.ConectarAsync(puerto, baud);
        ActualizarIndicadorArduino();
        AgregarConsola(ok ? "✅ Arduino conectado a " + puerto : "❌ No se pudo conectar a " + puerto,
            ok ? Color.Cyan : Color.Orange);
        await ResponderArduino(msg.id?.ToString(), ok, ok ? 1 : 0);
    }

    private async Task ProcesarArduinoDW(dynamic msg)
    {
        int pin = (int)(msg.pin ?? 13);
        bool val = (int)(msg.valor ?? 0) == 1;
        bool ok = await _arduino.DigitalWriteAsync(pin, val);
        await ResponderArduino(msg.id?.ToString(), ok, val ? 1 : 0);
    }

    private async Task ProcesarArduinoDR(dynamic msg)
    {
        int pin = (int)(msg.pin ?? 0);
        bool val = await _arduino.DigitalReadAsync(pin);
        await ResponderArduino(msg.id?.ToString(), true, val ? 1 : 0);
    }

    private async Task ProcesarArduinoAR(dynamic msg)
    {
        int pin = (int)(msg.pin ?? 0);
        int val = await _arduino.AnalogReadAsync(pin);
        await ResponderArduino(msg.id?.ToString(), true, val);
    }

    private async Task ProcesarArduinoAW(dynamic msg)
    {
        int pin = (int)(msg.pin ?? 0);
        int val = (int)(msg.valor ?? 0);
        bool ok = await _arduino.AnalogWriteAsync(pin, val);
        await ResponderArduino(msg.id?.ToString(), ok, val);
    }

    private async Task ProcesarArduinoSV(dynamic msg)
    {
        int pin = (int)(msg.pin ?? 9);
        int ang = (int)(msg.valor ?? 90);
        bool ok = await _arduino.ServoWriteAsync(pin, ang);
        await ResponderArduino(msg.id?.ToString(), ok, ang);
    }

    private async Task ProcesarArduinoEnviar(dynamic msg)
    {
        string texto = msg.texto?.ToString() ?? "";
        bool ok = await _arduino.EnviarTextoAsync(texto);
        await ResponderArduino(msg.id?.ToString(), ok, 0);
    }

    private async Task ProcesarArduinoLeer(dynamic msg)
    {
        string linea = await _arduino.LeerLineaAsync();
        string json = JsonConvert.SerializeObject(new { tipo = "arduino_resp", id = msg.id?.ToString(), ok = true, texto = linea });
        await webViewBlockly.ExecuteScriptAsync("window._arduinoRespuesta && window._arduinoRespuesta(" + JsonConvert.SerializeObject(json) + ")");
    }

    private async Task ResponderArduino(string? id, bool ok, int valor)
    {
        string json = JsonConvert.SerializeObject(new { tipo = "arduino_resp", id, ok, valor });
        await webViewBlockly.ExecuteScriptAsync("window._arduinoRespuesta && window._arduinoRespuesta(" + JsonConvert.SerializeObject(json) + ")");
    }

    private void ActualizarIndicadorArduino()
    {
        if (InvokeRequired) { Invoke(ActualizarIndicadorArduino); return; }
        if (_arduino.Conectado)
        {
            btnArduino.Text      = "🔌 " + _arduino.PuertoActual;
            btnArduino.ForeColor = Color.DarkGreen;
            lblVersion.Text      = "CodiBloques v1.0.0  |  Arduino: " + _arduino.PuertoActual;
        }
        else
        {
            btnArduino.Text      = "🔌 Arduino";
            btnArduino.ForeColor = Color.Gray;
            lblVersion.Text      = "CodiBloques v1.0.0";
        }
    }

    // ─── Helpers UI ───────────────────────────────────────────────────────────

    private void MostrarCodigo(string codigo)
    {
        if (InvokeRequired) { Invoke(() => MostrarCodigo(codigo)); return; }
        txtCodigoGenerado.Text = codigo;
    }

    private void MostrarError(string desc)
    {
        if (InvokeRequired) { Invoke(() => MostrarError(desc)); return; }
        lblEstado.Text = "Error: " + desc;
        lblEstado.ForeColor = Color.Red;
    }

    // ─── Ver / ocultar paneles ────────────────────────────────────────────────

    private void mnuMostrarCodigo_Click(object sender, EventArgs e)
    {
        if (mnuMostrarCodigo.Checked && !tabsInferior.TabPages.Contains(tabCodigo))
            tabsInferior.TabPages.Insert(0, tabCodigo);
        else if (!mnuMostrarCodigo.Checked && tabsInferior.TabPages.Contains(tabCodigo))
            tabsInferior.TabPages.Remove(tabCodigo);
        splitDerecha.Panel2Collapsed = tabsInferior.TabPages.Count == 0;
    }

    private void mnuMostrarConsola_Click(object sender, EventArgs e)
    {
        if (mnuMostrarConsola.Checked && !tabsInferior.TabPages.Contains(tabConsola))
            tabsInferior.TabPages.Add(tabConsola);
        else if (!mnuMostrarConsola.Checked && tabsInferior.TabPages.Contains(tabConsola))
            tabsInferior.TabPages.Remove(tabConsola);
        splitDerecha.Panel2Collapsed = tabsInferior.TabPages.Count == 0;
    }

    private void mnuMostrarDepuracion_Click(object sender, EventArgs e)
    {
        if (mnuMostrarDepuracion.Checked && !tabsInferior.TabPages.Contains(tabDepuracion))
            tabsInferior.TabPages.Add(tabDepuracion);
        else if (!mnuMostrarDepuracion.Checked && tabsInferior.TabPages.Contains(tabDepuracion))
            tabsInferior.TabPages.Remove(tabDepuracion);
        splitDerecha.Panel2Collapsed = tabsInferior.TabPages.Count == 0;
    }

    // ─── Pantalla completa ────────────────────────────────────────────────────

    private void mnuPantallaCompleta_Click(object sender, EventArgs e)
    {
        _enPantallaCompleta = !_enPantallaCompleta;
        if (_enPantallaCompleta)
        {
            _splitterDistanciaAnterior = splitDerecha.SplitterDistance;
            splitDerecha.Panel2Collapsed = true;
            toolStrip1.Visible   = false;
            menuStrip1.Visible   = false;
            statusStrip1.Visible = false;
            mnuPantallaCompleta.Text = "Salir de pantalla completa";
            btnPantallaCompleta.Text = "✕ Salir F11";
        }
        else
        {
            toolStrip1.Visible   = true;
            menuStrip1.Visible   = true;
            statusStrip1.Visible = true;
            splitDerecha.Panel2Collapsed = tabsInferior.TabPages.Count == 0;
            if (!splitDerecha.Panel2Collapsed)
                splitDerecha.SplitterDistance = _splitterDistanciaAnterior;
            mnuPantallaCompleta.Text = "Pantalla completa del escenario";
            btnPantallaCompleta.Text = "⛶ Escenario";
            lblEstado.Text = "Listo";
        }
    }

    private void FormPrincipal_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F11)
        {
            mnuPantallaCompleta_Click(sender ?? this, EventArgs.Empty);
            e.Handled = true;
        }
        if (e.KeyCode == Keys.Escape && _enPantallaCompleta)
        {
            mnuPantallaCompleta_Click(sender ?? this, EventArgs.Empty);
            e.Handled = true;
        }
    }

    // ─── Plantillas ───────────────────────────────────────────────────────────

    private async void btnPlantillas_Click(object sender, EventArgs e)
    {
        using var f = new FormPlantillas();
        if (f.ShowDialog(this) == DialogResult.OK && f.XmlSeleccionado != null)
        {
            string xmlPlantilla = f.XmlSeleccionado;
            xmlPlantilla = xmlPlantilla.Replace("\\", "\\\\");
            xmlPlantilla = xmlPlantilla.Replace("'", "\\'");
            xmlPlantilla = xmlPlantilla.Replace("\n", "\\n");
            xmlPlantilla = xmlPlantilla.Replace("\r", "");
            string script = "codiBlockly.cargarXml('" + xmlPlantilla + "')";
            await webViewBlockly.ExecuteScriptAsync(script);
            _archivoActual = f.PlantillaSeleccionada;
            _proyectoModificado = false;
            ConfigurarTitulo(_archivoActual);
            lblEstado.Text = "Plantilla cargada: " + (f.PlantillaSeleccionada ?? "");
        }
    }

    // ─── Tutoriales ───────────────────────────────────────────────────────────

    private void btnTutoriales_Click(object sender, EventArgs e)
    {
        _ = webViewBlockly.ExecuteScriptAsync("window.abrirTutoriales && window.abrirTutoriales()");
    }

    // ─── Grabador de sonidos ─────────────────────────────────────────────────

    private void btnGrabador_Click(object sender, EventArgs e)
    {
        _ = webViewBlockly.ExecuteScriptAsync("window.abrirGrabador && window.abrirGrabador()");
        splitDerecha.Panel2Collapsed = false;
        if (splitDerecha.SplitterDistance > ClientSize.Height - 300)
            splitDerecha.SplitterDistance = ClientSize.Height - 300;
    }

    // ─── Editor de disfraces ──────────────────────────────────────────────────

    private void btnEditorDisfraces_Click(object sender, EventArgs e)
    {
        _ = webViewBlockly.ExecuteScriptAsync("window.abrirEditorDisfraces && window.abrirEditorDisfraces()");
        splitDerecha.Panel2Collapsed = false;
        if (splitDerecha.SplitterDistance > ClientSize.Height - 350)
            splitDerecha.SplitterDistance = ClientSize.Height - 350;
    }

    // ─── Arduino ─────────────────────────────────────────────────────────────

    private void btnArduino_Click(object sender, EventArgs e)
    {
        using var f = new FormArduino(_arduino);
        f.ShowDialog(this);
        ActualizarIndicadorArduino();
    }

    // ─── Archivo ──────────────────────────────────────────────────────────────

    private async void btnNuevo_Click(object sender, EventArgs e)
    {
        if (_proyectoModificado)
        {
            var r = MessageBox.Show("¿Guardar cambios antes de crear un nuevo proyecto?",
                "CodiBloques", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (r == DialogResult.Cancel) return;
            if (r == DialogResult.Yes) await GuardarProyecto();
        }
        await webViewBlockly.ExecuteScriptAsync("codiBlockly.limpiarArea()");
        _archivoActual = null;
        _proyectoModificado = false;
        txtCodigoGenerado.Clear();
        txtConsola.Clear();
        ConfigurarTitulo();
    }

    private async void btnAbrir_Click(object sender, EventArgs e)
    {
        using var d = new OpenFileDialog
        {
            Title      = "Abrir proyecto CodiBloques",
            Filter     = "Proyecto CodiBloques (*.codi)|*.codi|Todos los archivos (*.*)|*.*",
            DefaultExt = "codi"
        };
        if (d.ShowDialog() != DialogResult.OK) return;
        try
        {
            string xml = await File.ReadAllTextAsync(d.FileName);
            string esc = xml.Replace("\\", "\\\\").Replace("'", "\\'")
                            .Replace("\n", "\\n").Replace("\r", "");
            await webViewBlockly.ExecuteScriptAsync("codiBlockly.cargarXml('" + esc + "')");
            _archivoActual = Path.GetFileNameWithoutExtension(d.FileName);
            _proyectoModificado = false;
            ConfigurarTitulo(_archivoActual);
            lblEstado.Text = "Proyecto cargado";
        }
        catch (Exception ex)
        {
            MessageBox.Show("No se pudo abrir:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void btnGuardar_Click(object sender, EventArgs e)     => await GuardarProyecto();
    private async void btnGuardarComo_Click(object sender, EventArgs e)  => await GuardarProyectoComo();

    private async Task GuardarProyecto()
    {
        if (_archivoActual == null) { await GuardarProyectoComo(); return; }
        await GuardarEnRuta(_archivoActual);
    }

    private async Task GuardarProyectoComo()
    {
        using var d = new SaveFileDialog
        {
            Title      = "Guardar proyecto",
            Filter     = "Proyecto CodiBloques (*.codi)|*.codi|Todos (*.*)|*.*",
            DefaultExt = "codi",
            FileName   = _archivoActual ?? "MiProyecto"
        };
        if (d.ShowDialog() != DialogResult.OK) return;
        await GuardarEnRuta(d.FileName);
        _archivoActual = Path.GetFileNameWithoutExtension(d.FileName);
        ConfigurarTitulo(_archivoActual);
    }

    private async Task GuardarEnRuta(string ruta)
    {
        try
        {
            string xml = await webViewBlockly.ExecuteScriptAsync("codiBlockly.obtenerXml()");
            xml = xml.Trim('"');
            xml = xml.Replace("\\n", "\n");
            xml = xml.Replace("\\\"", "\"");
            xml = xml.Replace("\\/", "/");
            if (!ruta.EndsWith(".codi")) ruta += ".codi";
            await File.WriteAllTextAsync(ruta, xml);
            _proyectoModificado = false;
            ConfigurarTitulo(_archivoActual);
            lblEstado.Text = "Guardado";
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al guardar:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ─── Exportar ─────────────────────────────────────────────────────────────

    private async void btnExportarCodigo_Click(object sender, EventArgs e)
    {
        try
        {
            string xml = await webViewBlockly.ExecuteScriptAsync("codiBlockly.obtenerXml()");
            xml = xml.Trim('"');
            xml = xml.Replace("\\n", "\n");
            xml = xml.Replace("\\\"", "\"");
            xml = xml.Replace("\\/", "/");
            using var frm = new FormExportar(xml);
            frm.ShowDialog(this);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ─── Ejecución ────────────────────────────────────────────────────────────

    private async void btnEjecutar_Click(object sender, EventArgs e)
    {
        txtConsola.Clear();
        tabsInferior.SelectedTab = tabCodigo;
        lblEstado.Text = "Ejecutando...";
        lblEstado.ForeColor = Color.DarkGreen;

        string codigo = await webViewBlockly.ExecuteScriptAsync("Blockly.JavaScript.workspaceToCode(workspace)");
        codigo = codigo.Trim('"');
        codigo = codigo.Replace("\\n", "\n");
        codigo = codigo.Replace("\\\"", "\"");
        codigo = codigo.Replace("\\/", "/");
        codigo = codigo.Replace("\\\\", "\\");
        txtCodigoGenerado.Text = codigo;

        try { await webViewBlockly.ExecuteScriptAsync("codiBlockly.ejecutar()"); }
        catch (Exception ex) { AgregarConsola("Error: " + ex.Message, Color.Red); }
    }

    private async void btnDetener_Click(object sender, EventArgs e)
    {
        await webViewBlockly.ExecuteScriptAsync("codiBlockly.detener()");
        lblEstado.Text = "Detenido";
        lblEstado.ForeColor = Color.DarkOrange;
    }

    // ─── Depuración ───────────────────────────────────────────────────────────

    private bool _modoDebug = false;

    private async void btnDepurar_Click(object sender, EventArgs e)
    {
        _modoDebug = !_modoDebug;
        if (_modoDebug)
        {
            btnDepurar.Text = "⏹ Detener debug";
            btnPasoSiguiente.Enabled = true;
            tabsInferior.SelectedTab = tabDepuracion;
            await webViewBlockly.ExecuteScriptAsync("codiBlockly.iniciarDebug()");
            lblEstado.Text = "Depurando";
        }
        else
        {
            btnDepurar.Text = "🐞 Depurar";
            btnPasoSiguiente.Enabled = false;
            await webViewBlockly.ExecuteScriptAsync("codiBlockly.detenerDebug()");
            lblEstado.Text = "Listo";
        }
    }

    private async void btnPasoSiguiente_Click(object sender, EventArgs e)
        => await webViewBlockly.ExecuteScriptAsync("codiBlockly.pasoSiguiente()");

    // ─── Consola ──────────────────────────────────────────────────────────────

    public void AgregarConsola(string texto, Color? color = null)
    {
        if (InvokeRequired) { Invoke(() => AgregarConsola(texto, color)); return; }
        int ini = txtConsola.TextLength;
        txtConsola.AppendText(texto + Environment.NewLine);
        if (color.HasValue)
        {
            txtConsola.Select(ini, texto.Length);
            txtConsola.SelectionColor = color.Value;
            txtConsola.SelectionLength = 0;
        }
        txtConsola.ScrollToCaret();
    }

    private void btnLimpiarConsola_Click(object sender, EventArgs e) => txtConsola.Clear();

    // ─── Zoom ─────────────────────────────────────────────────────────────────

    private async void btnZoomMenos_Click(object sender, EventArgs e)
    {
        _nivelZoom = Math.Max(25, _nivelZoom - 10);
        lblZoom.Text = _nivelZoom + "%";
        await webViewBlockly.ExecuteScriptAsync("codiBlockly.setZoom(" + (_nivelZoom / 100.0) + ")");
    }

    private async void btnZoomMas_Click(object sender, EventArgs e)
    {
        _nivelZoom = Math.Min(200, _nivelZoom + 10);
        lblZoom.Text = _nivelZoom + "%";
        await webViewBlockly.ExecuteScriptAsync("codiBlockly.setZoom(" + (_nivelZoom / 100.0) + ")");
    }

    private async void btnZoomReset_Click(object sender, EventArgs e)
    {
        _nivelZoom = 100;
        lblZoom.Text = "100%";
        await webViewBlockly.ExecuteScriptAsync("codiBlockly.setZoom(1)");
    }

    // ─── Ayuda / Salir ────────────────────────────────────────────────────────

    private void mnuSalir_Click(object sender, EventArgs e) => Application.Exit();

    private void btnAcercaDe_Click(object sender, EventArgs e)
    {
        using var f = new FormAcercaDe();
        f.ShowDialog(this);
    }

    // ─── Cierre ───────────────────────────────────────────────────────────────

    private async void FormPrincipal_FormClosing(object? sender, FormClosingEventArgs e)
    {
        _ = _arduino.DesconectarAsync();
        if (_proyectoModificado)
        {
            var r = MessageBox.Show("Hay cambios sin guardar. ¿Guardar antes de salir?",
                "CodiBloques", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (r == DialogResult.Cancel) { e.Cancel = true; return; }
            if (r == DialogResult.Yes)
            {
                e.Cancel = true;
                await GuardarProyecto();
                Application.Exit();
            }
        }
    }
}
