using CodiBloques.Arduino;

namespace CodiBloques.Forms;

public partial class FormArduino : Form
{
    private readonly ArduinoService _arduino;
    private bool _monitorActivo = false;

    public FormArduino(ArduinoService arduino)
    {
        InitializeComponent();
        _arduino = arduino;
        _arduino.DatoRecibido  += Arduino_DatoRecibido;
        _arduino.ErrorOcurrido += Arduino_ErrorOcurrido;
    }

    private void FormArduino_Load(object sender, EventArgs e)
    {
        RefrescarPuertos();
        ActualizarEstado();
    }

    private void RefrescarPuertos()
    {
        cboPuertos.Items.Clear();
        var puertos = ArduinoService.ObtenerPuertos();
        if (puertos.Length == 0)
        {
            cboPuertos.Items.Add("(No se encontraron puertos)");
            cboPuertos.SelectedIndex = 0;
            btnConectar.Enabled = false;
        }
        else
        {
            cboPuertos.Items.AddRange(puertos);
            cboPuertos.SelectedIndex = 0;
            btnConectar.Enabled = true;
        }
    }

    private void ActualizarEstado()
    {
        if (_arduino.Conectado)
        {
            lblEstado.Text      = $"✅ Conectado a {_arduino.PuertoActual}";
            lblEstado.ForeColor = Color.DarkGreen;
            btnConectar.Text    = "Desconectar";
            cboPuertos.Enabled  = false;
            cboBaudRate.Enabled = false;
            grpMonitor.Enabled  = true;
            grpPines.Enabled    = true;
        }
        else
        {
            lblEstado.Text      = "⭕ Desconectado";
            lblEstado.ForeColor = Color.DarkRed;
            btnConectar.Text    = "Conectar";
            cboPuertos.Enabled  = true;
            cboBaudRate.Enabled = true;
            grpMonitor.Enabled  = false;
            grpPines.Enabled    = false;
        }
    }

    // ── Conexión ──────────────────────────────────────────────────────────────

    private async void btnConectar_Click(object sender, EventArgs e)
    {
        btnConectar.Enabled = false;
        lblEstado.Text = "Conectando...";
        lblEstado.ForeColor = Color.DarkOrange;

        if (_arduino.Conectado)
        {
            await _arduino.DesconectarAsync();
        }
        else
        {
            string puerto   = cboPuertos.SelectedItem?.ToString() ?? "";
            int baudRate    = int.Parse(cboBaudRate.SelectedItem?.ToString() ?? "9600");
            bool resultado  = await _arduino.ConectarAsync(puerto, baudRate);

            if (!resultado)
            {
                MessageBox.Show(
                    $"No se pudo conectar a {puerto}.\n\n" +
                    "Verificá que:\n• Arduino esté conectado\n• El puerto sea correcto\n• No esté en uso por otro programa",
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        ActualizarEstado();
        btnConectar.Enabled = true;
    }

    private void btnRefrescar_Click(object sender, EventArgs e) => RefrescarPuertos();

    // ── Monitor serial ────────────────────────────────────────────────────────

    private void btnIniciarMonitor_Click(object sender, EventArgs e)
    {
        _monitorActivo = !_monitorActivo;
        btnIniciarMonitor.Text = _monitorActivo ? "⏹ Detener monitor" : "▶ Iniciar monitor";
    }

    private async void btnEnviarSerial_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtEnviar.Text)) return;
        await _arduino.EnviarTextoAsync(txtEnviar.Text);
        AgregarMonitor($"→ {txtEnviar.Text}", Color.Cyan);
        txtEnviar.Clear();
    }

    private void txtEnviar_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter) btnEnviarSerial_Click(sender, e);
    }

    private void btnLimpiarMonitor_Click(object sender, EventArgs e) => txtMonitor.Clear();

    private void Arduino_DatoRecibido(object? sender, string dato)
    {
        if (_monitorActivo)
            AgregarMonitor($"← {dato}", Color.LightGreen);
    }

    private void Arduino_ErrorOcurrido(object? sender, string error)
    {
        AgregarMonitor($"⚠ {error}", Color.Orange);
        if (InvokeRequired) Invoke(ActualizarEstado);
        else ActualizarEstado();
    }

    private void AgregarMonitor(string texto, Color color)
    {
        if (InvokeRequired) { Invoke(() => AgregarMonitor(texto, color)); return; }
        int ini = txtMonitor.TextLength;
        txtMonitor.AppendText($"[{DateTime.Now:HH:mm:ss}] {texto}{Environment.NewLine}");
        txtMonitor.Select(ini, texto.Length + 12);
        txtMonitor.SelectionColor = color;
        txtMonitor.SelectionLength = 0;
        txtMonitor.ScrollToCaret();
    }

    // ── Control de pines ──────────────────────────────────────────────────────

    private async void btnDigitalHigh_Click(object sender, EventArgs e)
    {
        int pin = (int)nudPinDigital.Value;
        bool ok = await _arduino.DigitalWriteAsync(pin, true);
        AgregarMonitor($"Pin {pin} → HIGH {(ok?"✓":"✗")}", ok ? Color.LightGreen : Color.Orange);
    }

    private async void btnDigitalLow_Click(object sender, EventArgs e)
    {
        int pin = (int)nudPinDigital.Value;
        bool ok = await _arduino.DigitalWriteAsync(pin, false);
        AgregarMonitor($"Pin {pin} → LOW {(ok?"✓":"✗")}", ok ? Color.LightGreen : Color.Orange);
    }

    private async void btnDigitalRead_Click(object sender, EventArgs e)
    {
        int pin   = (int)nudPinDigital.Value;
        bool val  = await _arduino.DigitalReadAsync(pin);
        AgregarMonitor($"Pin {pin} = {(val?"HIGH":"LOW")}", Color.Cyan);
        lblLecturaDigital.Text = val ? "HIGH" : "LOW";
    }

    private async void btnAnalogRead_Click(object sender, EventArgs e)
    {
        int pin = (int)nudPinAnalogico.Value;
        int val = await _arduino.AnalogReadAsync(pin);
        AgregarMonitor($"A{pin} = {val} ({val*100/1023}%)", Color.Cyan);
        lblLecturaAnalogica.Text = $"{val} / 1023";
        barAnalogico.Value = Math.Min(100, val * 100 / 1023);
    }

    private async void btnServo_Click(object sender, EventArgs e)
    {
        int pin    = (int)nudPinServo.Value;
        int angulo = (int)nudAnguloServo.Value;
        bool ok    = await _arduino.ServoWriteAsync(pin, angulo);
        AgregarMonitor($"Servo pin {pin} → {angulo}° {(ok?"✓":"✗")}", ok ? Color.LightGreen : Color.Orange);
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        _arduino.DatoRecibido  -= Arduino_DatoRecibido;
        _arduino.ErrorOcurrido -= Arduino_ErrorOcurrido;
        base.OnFormClosing(e);
    }
}
