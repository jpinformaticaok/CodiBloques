namespace CodiBloques.Forms;

partial class FormExportar
{
    private System.ComponentModel.IContainer components = null;
    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        pnlTop             = new Panel();
        lblTitulo          = new Label();
        lblSubtitulo       = new Label();
        pnlOpciones        = new Panel();
        lblNombreLabel     = new Label();
        txtNombre          = new TextBox();
        lblAutorLabel      = new Label();
        txtAutor           = new TextBox();
        chkAutoEjecutar    = new CheckBox();
        chkSoloEscenario   = new CheckBox();
        grpFormatos        = new GroupBox();
        pnlHtml            = new Panel();
        lblHtmlTitulo      = new Label();
        lblHtmlDesc        = new Label();
        btnExportarHtml    = new Button();
        pnlPaquete         = new Panel();
        lblPaqTitulo       = new Label();
        lblPaqDesc         = new Label();
        btnExportarPaquete = new Button();
        pnlExe             = new Panel();
        lblExeTitulo       = new Label();
        lblExeDesc         = new Label();
        btnExportarExe     = new Button();
        lblEstado          = new Label();
        btnCerrar          = new Button();

        SuspendLayout();

        this.Text          = "CodiBloques — Exportar proyecto";
        this.ClientSize    = new Size(560, 520);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox   = false;
        this.StartPosition = FormStartPosition.CenterParent;
        this.BackColor     = Color.FromArgb(240, 242, 248);
        this.Load         += FormExportar_Load;

        // Panel superior
        pnlTop.Dock      = DockStyle.Top;
        pnlTop.Height    = 70;
        pnlTop.BackColor = Color.FromArgb(60, 60, 120);
        pnlTop.Padding   = new Padding(20, 12, 20, 12);

        lblTitulo.Text      = "📦 Exportar proyecto";
        lblTitulo.Font      = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.White;
        lblTitulo.Location  = new Point(20, 10);
        lblTitulo.AutoSize  = true;

        lblSubtitulo.Text      = "Compartí tu proyecto con otros sin necesitar CodiBloques";
        lblSubtitulo.Font      = new Font("Segoe UI", 9F);
        lblSubtitulo.ForeColor = Color.FromArgb(180, 200, 255);
        lblSubtitulo.Location  = new Point(22, 40);
        lblSubtitulo.AutoSize  = true;

        pnlTop.Controls.AddRange(new Control[] { lblTitulo, lblSubtitulo });

        // Panel opciones
        pnlOpciones.Location  = new Point(16, 82);
        pnlOpciones.Size      = new Size(528, 80);
        pnlOpciones.BackColor = Color.White;
        pnlOpciones.BorderStyle = BorderStyle.FixedSingle;

        lblNombreLabel.Text     = "Nombre del proyecto:";
        lblNombreLabel.Location = new Point(12, 14);
        lblNombreLabel.AutoSize = true;
        lblNombreLabel.Font     = new Font("Segoe UI", 9F);

        txtNombre.Location  = new Point(140, 10);
        txtNombre.Size      = new Size(160, 23);
        txtNombre.Font      = new Font("Segoe UI", 9.5F);

        lblAutorLabel.Text     = "Autor:";
        lblAutorLabel.Location = new Point(320, 14);
        lblAutorLabel.AutoSize = true;
        lblAutorLabel.Font     = new Font("Segoe UI", 9F);

        txtAutor.Location = new Point(360, 10);
        txtAutor.Size     = new Size(155, 23);
        txtAutor.Font     = new Font("Segoe UI", 9.5F);

        chkAutoEjecutar.Text     = "Ejecutar automáticamente al abrir";
        chkAutoEjecutar.Location = new Point(12, 44);
        chkAutoEjecutar.AutoSize = true;
        chkAutoEjecutar.Checked  = true;
        chkAutoEjecutar.Font     = new Font("Segoe UI", 9F);

        chkSoloEscenario.Text     = "Solo mostrar el escenario (sin bloques)";
        chkSoloEscenario.Location = new Point(260, 44);
        chkSoloEscenario.AutoSize = true;
        chkSoloEscenario.Font     = new Font("Segoe UI", 9F);

        pnlOpciones.Controls.AddRange(new Control[] {
            lblNombreLabel, txtNombre, lblAutorLabel, txtAutor, chkAutoEjecutar, chkSoloEscenario });

        // Grupo formatos
        grpFormatos.Text     = "Formato de exportación";
        grpFormatos.Location = new Point(16, 174);
        grpFormatos.Size     = new Size(528, 280);
        grpFormatos.Font     = new Font("Segoe UI", 9F, FontStyle.Bold);

        // Tarjeta HTML
        pnlHtml.Location    = new Point(12, 24);
        pnlHtml.Size        = new Size(500, 70);
        pnlHtml.BackColor   = Color.FromArgb(245, 248, 255);
        pnlHtml.BorderStyle = BorderStyle.FixedSingle;

        lblHtmlTitulo.Text      = "🌐 Página Web (.html)";
        lblHtmlTitulo.Font      = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblHtmlTitulo.Location  = new Point(12, 8);
        lblHtmlTitulo.AutoSize  = true;
        lblHtmlTitulo.ForeColor = Color.FromArgb(30, 80, 160);

        lblHtmlDesc.Text      = "Un archivo HTML que se abre en cualquier navegador. Ideal para compartir online.";
        lblHtmlDesc.Font      = new Font("Segoe UI", 8.5F);
        lblHtmlDesc.Location  = new Point(12, 28);
        lblHtmlDesc.Size      = new Size(360, 32);
        lblHtmlDesc.ForeColor = Color.DimGray;

        btnExportarHtml.Text      = "Exportar HTML →";
        btnExportarHtml.Location  = new Point(392, 18);
        btnExportarHtml.Size      = new Size(100, 30);
        btnExportarHtml.FlatStyle = FlatStyle.Flat;
        btnExportarHtml.BackColor = Color.FromArgb(30, 80, 160);
        btnExportarHtml.ForeColor = Color.White;
        btnExportarHtml.Font      = new Font("Segoe UI", 8.5F);
        btnExportarHtml.Click    += btnExportarHtml_Click;
        pnlHtml.Controls.AddRange(new Control[] { lblHtmlTitulo, lblHtmlDesc, btnExportarHtml });

        // Tarjeta Paquete
        pnlPaquete.Location    = new Point(12, 106);
        pnlPaquete.Size        = new Size(500, 70);
        pnlPaquete.BackColor   = Color.FromArgb(245, 255, 248);
        pnlPaquete.BorderStyle = BorderStyle.FixedSingle;

        lblPaqTitulo.Text      = "📁 Paquete ejecutable (.bat + .html)";
        lblPaqTitulo.Font      = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblPaqTitulo.Location  = new Point(12, 8);
        lblPaqTitulo.AutoSize  = true;
        lblPaqTitulo.ForeColor = Color.FromArgb(20, 100, 40);

        lblPaqDesc.Text      = "Carpeta con HTML + Ejecutar.bat. Funciona en cualquier PC con Windows y Edge.";
        lblPaqDesc.Font      = new Font("Segoe UI", 8.5F);
        lblPaqDesc.Location  = new Point(12, 28);
        lblPaqDesc.Size      = new Size(360, 32);
        lblPaqDesc.ForeColor = Color.DimGray;

        btnExportarPaquete.Text      = "Exportar →";
        btnExportarPaquete.Location  = new Point(392, 18);
        btnExportarPaquete.Size      = new Size(100, 30);
        btnExportarPaquete.FlatStyle = FlatStyle.Flat;
        btnExportarPaquete.BackColor = Color.FromArgb(20, 100, 40);
        btnExportarPaquete.ForeColor = Color.White;
        btnExportarPaquete.Font      = new Font("Segoe UI", 8.5F);
        btnExportarPaquete.Click    += btnExportarPaquete_Click;
        pnlPaquete.Controls.AddRange(new Control[] { lblPaqTitulo, lblPaqDesc, btnExportarPaquete });

        // Tarjeta EXE
        pnlExe.Location    = new Point(12, 188);
        pnlExe.Size        = new Size(500, 70);
        pnlExe.BackColor   = Color.FromArgb(255, 248, 240);
        pnlExe.BorderStyle = BorderStyle.FixedSingle;

        lblExeTitulo.Text      = "⚙️ Ejecutable Windows (.exe)";
        lblExeTitulo.Font      = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblExeTitulo.Location  = new Point(12, 8);
        lblExeTitulo.AutoSize  = true;
        lblExeTitulo.ForeColor = Color.FromArgb(140, 60, 0);

        lblExeDesc.Text      = "Archivo .exe standalone. Requiere .NET Framework. Si falla, exporta el paquete HTML.";
        lblExeDesc.Font      = new Font("Segoe UI", 8.5F);
        lblExeDesc.Location  = new Point(12, 28);
        lblExeDesc.Size      = new Size(360, 32);
        lblExeDesc.ForeColor = Color.DimGray;

        btnExportarExe.Text      = "Exportar .exe →";
        btnExportarExe.Location  = new Point(392, 18);
        btnExportarExe.Size      = new Size(100, 30);
        btnExportarExe.FlatStyle = FlatStyle.Flat;
        btnExportarExe.BackColor = Color.FromArgb(140, 60, 0);
        btnExportarExe.ForeColor = Color.White;
        btnExportarExe.Font      = new Font("Segoe UI", 8.5F);
        btnExportarExe.Click    += btnExportarExe_Click;
        pnlExe.Controls.AddRange(new Control[] { lblExeTitulo, lblExeDesc, btnExportarExe });

        grpFormatos.Controls.AddRange(new Control[] { pnlHtml, pnlPaquete, pnlExe });

        // Estado y botón cerrar
        lblEstado.Text      = "";
        lblEstado.Location  = new Point(16, 466);
        lblEstado.Size      = new Size(430, 22);
        lblEstado.Font      = new Font("Segoe UI", 9F);
        lblEstado.ForeColor = Color.DimGray;

        btnCerrar.Text      = "Cerrar";
        btnCerrar.Location  = new Point(460, 460);
        btnCerrar.Size      = new Size(84, 30);
        btnCerrar.FlatStyle = FlatStyle.Flat;
        btnCerrar.Click    += btnCerrar_Click;

        this.Controls.AddRange(new Control[] {
            pnlTop, pnlOpciones, grpFormatos, lblEstado, btnCerrar });

        ResumeLayout(false);
    }

    private Panel pnlTop, pnlOpciones, pnlHtml, pnlPaquete, pnlExe;
    private Label lblTitulo, lblSubtitulo, lblNombreLabel, lblAutorLabel;
    private Label lblHtmlTitulo, lblHtmlDesc, lblPaqTitulo, lblPaqDesc, lblExeTitulo, lblExeDesc;
    private Label lblEstado;
    private TextBox txtNombre, txtAutor;
    private CheckBox chkAutoEjecutar, chkSoloEscenario;
    private GroupBox grpFormatos;
    private Button btnExportarHtml, btnExportarPaquete, btnExportarExe, btnCerrar;
}
