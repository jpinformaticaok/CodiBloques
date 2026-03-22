namespace CodiBloques.Forms;

partial class FormPlantillas
{
    private System.ComponentModel.IContainer components = null;
    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        pnlTop        = new Panel();
        lblTitulo     = new Label();
        lblSubtitulo  = new Label();
        txtBuscar     = new TextBox();
        btnCerrar     = new Button();
        pnlPlantillas = new FlowLayoutPanel();
        lblMensaje    = new Label();

        SuspendLayout();

        this.Text            = "CodiBloques — Plantillas de proyectos";
        this.ClientSize      = new Size(880, 580);
        this.MinimumSize     = new Size(700, 480);
        this.StartPosition   = FormStartPosition.CenterParent;
        this.BackColor       = Color.FromArgb(15, 15, 35);
        this.Load           += FormPlantillas_Load;

        // Panel superior
        pnlTop.Dock      = DockStyle.Top;
        pnlTop.Height    = 80;
        pnlTop.BackColor = Color.FromArgb(20, 20, 50);
        pnlTop.Padding   = new Padding(20, 10, 20, 10);

        lblTitulo.Text      = "📋 Plantillas de proyectos";
        lblTitulo.Font      = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.White;
        lblTitulo.Location  = new Point(20, 10);
        lblTitulo.AutoSize  = true;

        lblSubtitulo.Text      = "Elegí una plantilla para empezar rápido";
        lblSubtitulo.Font      = new Font("Segoe UI", 9F);
        lblSubtitulo.ForeColor = Color.FromArgb(150, 150, 200);
        lblSubtitulo.Location  = new Point(22, 42);
        lblSubtitulo.AutoSize  = true;

        txtBuscar.PlaceholderText = "🔍 Buscar plantilla...";
        txtBuscar.Location        = new Point(560, 25);
        txtBuscar.Size            = new Size(200, 26);
        txtBuscar.Font            = new Font("Segoe UI", 10F);
        txtBuscar.BackColor       = Color.FromArgb(30, 30, 60);
        txtBuscar.ForeColor       = Color.White;
        txtBuscar.BorderStyle     = BorderStyle.FixedSingle;
        txtBuscar.TextChanged    += txtBuscar_TextChanged;

        btnCerrar.Text      = "✕";
        btnCerrar.Location  = new Point(840, 20);
        btnCerrar.Size      = new Size(30, 30);
        btnCerrar.FlatStyle = FlatStyle.Flat;
        btnCerrar.ForeColor = Color.White;
        btnCerrar.BackColor = Color.FromArgb(80, 30, 30);
        btnCerrar.FlatAppearance.BorderColor = Color.FromArgb(120, 40, 40);
        btnCerrar.Click    += btnCerrar_Click;

        pnlTop.Controls.AddRange(new Control[] { lblTitulo, lblSubtitulo, txtBuscar, btnCerrar });

        // Panel de plantillas
        pnlPlantillas.Dock          = DockStyle.Fill;
        pnlPlantillas.AutoScroll    = true;
        pnlPlantillas.Padding       = new Padding(10);
        pnlPlantillas.BackColor     = Color.FromArgb(15, 15, 35);
        pnlPlantillas.FlowDirection = FlowDirection.LeftToRight;
        pnlPlantillas.WrapContents  = true;

        lblMensaje.Text      = "Cargando plantillas...";
        lblMensaje.Font      = new Font("Segoe UI", 10F);
        lblMensaje.ForeColor = Color.Gray;
        lblMensaje.Location  = new Point(20, 20);
        lblMensaje.AutoSize  = true;

        this.Controls.Add(pnlPlantillas);
        this.Controls.Add(lblMensaje);
        this.Controls.Add(pnlTop);

        ResumeLayout(false);
    }

    private Panel pnlTop;
    private Label lblTitulo, lblSubtitulo, lblMensaje;
    private TextBox txtBuscar;
    private Button btnCerrar;
    private FlowLayoutPanel pnlPlantillas;
}
