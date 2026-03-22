namespace CodiBloques.Forms;

partial class FormArduino
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        // Controles de conexión
        pnlConexion      = new Panel();
        lblPuerto        = new Label();
        cboPuertos       = new ComboBox();
        lblBaudRate      = new Label();
        cboBaudRate      = new ComboBox();
        btnConectar      = new Button();
        btnRefrescar     = new Button();
        lblEstado        = new Label();

        // Monitor serial
        grpMonitor       = new GroupBox();
        txtMonitor       = new RichTextBox();
        pnlEnviar        = new Panel();
        txtEnviar        = new TextBox();
        btnEnviarSerial  = new Button();
        btnIniciarMonitor= new Button();
        btnLimpiarMonitor= new Button();

        // Control de pines
        grpPines         = new GroupBox();
        tabPines         = new TabControl();
        tabDigital       = new TabPage();
        tabAnalogico     = new TabPage();
        tabServo         = new TabPage();

        // Pines digitales
        lblPinDigital    = new Label();
        nudPinDigital    = new NumericUpDown();
        btnDigitalHigh   = new Button();
        btnDigitalLow    = new Button();
        btnDigitalRead   = new Button();
        lblLecturaDigital= new Label();

        // Pines analógicos
        lblPinAnalogico  = new Label();
        nudPinAnalogico  = new NumericUpDown();
        btnAnalogRead    = new Button();
        lblLecturaAnalogica = new Label();
        barAnalogico     = new ProgressBar();

        // Servo
        lblPinServo      = new Label();
        nudPinServo      = new NumericUpDown();
        lblAnguloServo   = new Label();
        nudAnguloServo   = new NumericUpDown();
        btnServo         = new Button();

        SuspendLayout();

        // Form
        this.Text = "CodiBloques — Conexión Arduino";
        this.ClientSize = new Size(580, 620);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.StartPosition = FormStartPosition.CenterParent;
        this.Load += FormArduino_Load;

        // ── Panel conexión ────────────────────────────────────────────────────
        pnlConexion.Location = new Point(10, 10);
        pnlConexion.Size = new Size(555, 80);
        pnlConexion.BorderStyle = BorderStyle.FixedSingle;
        pnlConexion.BackColor = Color.FromArgb(240, 248, 255);

        lblPuerto.Text = "Puerto:"; lblPuerto.Location = new Point(10, 12); lblPuerto.AutoSize = true;
        cboPuertos.Location = new Point(60, 8); cboPuertos.Size = new Size(100, 23); cboPuertos.DropDownStyle = ComboBoxStyle.DropDownList;

        lblBaudRate.Text = "Baud:"; lblBaudRate.Location = new Point(170, 12); lblBaudRate.AutoSize = true;
        cboBaudRate.Location = new Point(205, 8); cboBaudRate.Size = new Size(80, 23); cboBaudRate.DropDownStyle = ComboBoxStyle.DropDownList;
        cboBaudRate.Items.AddRange(new object[]{"9600","19200","38400","57600","115200"});
        cboBaudRate.SelectedIndex = 0;

        btnRefrescar.Text = "↺"; btnRefrescar.Location = new Point(295, 7); btnRefrescar.Size = new Size(30, 25);
        btnRefrescar.Click += btnRefrescar_Click;

        btnConectar.Text = "Conectar"; btnConectar.Location = new Point(335, 7); btnConectar.Size = new Size(90, 25);
        btnConectar.BackColor = Color.FromArgb(0, 150, 80); btnConectar.ForeColor = Color.White;
        btnConectar.FlatStyle = FlatStyle.Flat;
        btnConectar.Click += btnConectar_Click;

        lblEstado.Text = "⭕ Desconectado"; lblEstado.Location = new Point(10, 45);
        lblEstado.AutoSize = true; lblEstado.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblEstado.ForeColor = Color.DarkRed;

        pnlConexion.Controls.AddRange(new Control[]{ lblPuerto, cboPuertos, lblBaudRate, cboBaudRate, btnRefrescar, btnConectar, lblEstado });

        // ── Monitor serial ────────────────────────────────────────────────────
        grpMonitor.Text = "Monitor serial";
        grpMonitor.Location = new Point(10, 100);
        grpMonitor.Size = new Size(555, 240);
        grpMonitor.Enabled = false;

        txtMonitor.Location = new Point(10, 22);
        txtMonitor.Size = new Size(530, 160);
        txtMonitor.ReadOnly = true;
        txtMonitor.BackColor = Color.Black;
        txtMonitor.ForeColor = Color.LightGreen;
        txtMonitor.Font = new Font("Consolas", 9F);
        txtMonitor.ScrollBars = RichTextBoxScrollBars.Vertical;

        pnlEnviar.Location = new Point(10, 188);
        pnlEnviar.Size = new Size(530, 30);

        txtEnviar.Location = new Point(0, 3); txtEnviar.Size = new Size(310, 23);
        txtEnviar.Font = new Font("Consolas", 9F);
        txtEnviar.KeyDown += txtEnviar_KeyDown;

        btnEnviarSerial.Text = "Enviar"; btnEnviarSerial.Location = new Point(316, 2); btnEnviarSerial.Size = new Size(65, 25);
        btnEnviarSerial.Click += btnEnviarSerial_Click;

        btnIniciarMonitor.Text = "▶ Monitor"; btnIniciarMonitor.Location = new Point(387, 2); btnIniciarMonitor.Size = new Size(80, 25);
        btnIniciarMonitor.Click += btnIniciarMonitor_Click;

        btnLimpiarMonitor.Text = "Limpiar"; btnLimpiarMonitor.Location = new Point(473, 2); btnLimpiarMonitor.Size = new Size(60, 25);
        btnLimpiarMonitor.Click += btnLimpiarMonitor_Click;

        pnlEnviar.Controls.AddRange(new Control[]{ txtEnviar, btnEnviarSerial, btnIniciarMonitor, btnLimpiarMonitor });
        grpMonitor.Controls.AddRange(new Control[]{ txtMonitor, pnlEnviar });

        // ── Control de pines ──────────────────────────────────────────────────
        grpPines.Text = "Control de pines";
        grpPines.Location = new Point(10, 350);
        grpPines.Size = new Size(555, 220);
        grpPines.Enabled = false;

        tabPines.Location = new Point(10, 20); tabPines.Size = new Size(530, 185);

        // Tab digital
        tabDigital.Text = "Digital";
        ((System.ComponentModel.ISupportInitialize)nudPinDigital).BeginInit();
        lblPinDigital.Text = "Pin digital:"; lblPinDigital.Location = new Point(10, 20); lblPinDigital.AutoSize = true;
        nudPinDigital.Location = new Point(90, 16); nudPinDigital.Size = new Size(55, 23);
        nudPinDigital.Minimum = 0; nudPinDigital.Maximum = 53; nudPinDigital.Value = 13;
        ((System.ComponentModel.ISupportInitialize)nudPinDigital).EndInit();

        btnDigitalHigh.Text = "HIGH (encender)"; btnDigitalHigh.Location = new Point(10, 55); btnDigitalHigh.Size = new Size(130, 30);
        btnDigitalHigh.BackColor = Color.FromArgb(0,180,0); btnDigitalHigh.ForeColor = Color.White; btnDigitalHigh.FlatStyle = FlatStyle.Flat;
        btnDigitalHigh.Click += btnDigitalHigh_Click;

        btnDigitalLow.Text = "LOW (apagar)"; btnDigitalLow.Location = new Point(150, 55); btnDigitalLow.Size = new Size(120, 30);
        btnDigitalLow.BackColor = Color.FromArgb(180,0,0); btnDigitalLow.ForeColor = Color.White; btnDigitalLow.FlatStyle = FlatStyle.Flat;
        btnDigitalLow.Click += btnDigitalLow_Click;

        btnDigitalRead.Text = "Leer pin"; btnDigitalRead.Location = new Point(280, 55); btnDigitalRead.Size = new Size(90, 30);
        btnDigitalRead.Click += btnDigitalRead_Click;

        lblLecturaDigital.Text = "---"; lblLecturaDigital.Location = new Point(380, 62);
        lblLecturaDigital.AutoSize = true; lblLecturaDigital.Font = new Font("Segoe UI", 12F, FontStyle.Bold);

        tabDigital.Controls.AddRange(new Control[]{ lblPinDigital, nudPinDigital, btnDigitalHigh, btnDigitalLow, btnDigitalRead, lblLecturaDigital });

        // Tab analógico
        tabAnalogico.Text = "Analógico";
        ((System.ComponentModel.ISupportInitialize)nudPinAnalogico).BeginInit();
        lblPinAnalogico.Text = "Pin analógico (A):"; lblPinAnalogico.Location = new Point(10, 20); lblPinAnalogico.AutoSize = true;
        nudPinAnalogico.Location = new Point(130, 16); nudPinAnalogico.Size = new Size(55, 23);
        nudPinAnalogico.Minimum = 0; nudPinAnalogico.Maximum = 15; nudPinAnalogico.Value = 0;
        ((System.ComponentModel.ISupportInitialize)nudPinAnalogico).EndInit();

        btnAnalogRead.Text = "Leer valor"; btnAnalogRead.Location = new Point(10, 55); btnAnalogRead.Size = new Size(100, 30);
        btnAnalogRead.Click += btnAnalogRead_Click;

        lblLecturaAnalogica.Text = "--- / 1023"; lblLecturaAnalogica.Location = new Point(120, 62);
        lblLecturaAnalogica.AutoSize = true; lblLecturaAnalogica.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

        barAnalogico.Location = new Point(10, 100); barAnalogico.Size = new Size(480, 20);
        barAnalogico.Minimum = 0; barAnalogico.Maximum = 100;

        tabAnalogico.Controls.AddRange(new Control[]{ lblPinAnalogico, nudPinAnalogico, btnAnalogRead, lblLecturaAnalogica, barAnalogico });

        // Tab servo
        tabServo.Text = "Servo";
        ((System.ComponentModel.ISupportInitialize)nudPinServo).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudAnguloServo).BeginInit();

        lblPinServo.Text = "Pin servo:"; lblPinServo.Location = new Point(10, 20); lblPinServo.AutoSize = true;
        nudPinServo.Location = new Point(75, 16); nudPinServo.Size = new Size(55, 23);
        nudPinServo.Minimum = 0; nudPinServo.Maximum = 53; nudPinServo.Value = 9;

        lblAnguloServo.Text = "Ángulo (0-180°):"; lblAnguloServo.Location = new Point(10, 55); lblAnguloServo.AutoSize = true;
        nudAnguloServo.Location = new Point(115, 51); nudAnguloServo.Size = new Size(60, 23);
        nudAnguloServo.Minimum = 0; nudAnguloServo.Maximum = 180; nudAnguloServo.Value = 90;

        btnServo.Text = "Mover servo"; btnServo.Location = new Point(10, 90); btnServo.Size = new Size(120, 30);
        btnServo.BackColor = Color.FromArgb(0,120,200); btnServo.ForeColor = Color.White; btnServo.FlatStyle = FlatStyle.Flat;
        btnServo.Click += btnServo_Click;

        ((System.ComponentModel.ISupportInitialize)nudPinServo).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudAnguloServo).EndInit();

        tabServo.Controls.AddRange(new Control[]{ lblPinServo, nudPinServo, lblAnguloServo, nudAnguloServo, btnServo });

        tabPines.TabPages.AddRange(new TabPage[]{ tabDigital, tabAnalogico, tabServo });
        grpPines.Controls.Add(tabPines);

        // Agregar al form
        this.Controls.AddRange(new Control[]{ pnlConexion, grpMonitor, grpPines });

        ResumeLayout(false);
    }

    // Declaraciones
    private Panel pnlConexion;
    private Label lblPuerto, lblBaudRate, lblEstado;
    private ComboBox cboPuertos, cboBaudRate;
    private Button btnConectar, btnRefrescar;

    private GroupBox grpMonitor;
    private RichTextBox txtMonitor;
    private Panel pnlEnviar;
    private TextBox txtEnviar;
    private Button btnEnviarSerial, btnIniciarMonitor, btnLimpiarMonitor;

    private GroupBox grpPines;
    private TabControl tabPines;
    private TabPage tabDigital, tabAnalogico, tabServo;

    private Label lblPinDigital, lblLecturaDigital;
    private NumericUpDown nudPinDigital;
    private Button btnDigitalHigh, btnDigitalLow, btnDigitalRead;

    private Label lblPinAnalogico, lblLecturaAnalogica;
    private NumericUpDown nudPinAnalogico;
    private Button btnAnalogRead;
    private ProgressBar barAnalogico;

    private Label lblPinServo, lblAnguloServo;
    private NumericUpDown nudPinServo, nudAnguloServo;
    private Button btnServo;
}
