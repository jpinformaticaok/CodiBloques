namespace CodiBloques.Forms;

partial class FormAcercaDe
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        pnlTop      = new Panel();
        lblNombre   = new Label();
        lblSlogan   = new Label();
        pnlInfo     = new Panel();
        lblVersion  = new Label();
        lblDesc     = new Label();
        lblBlockly  = new Label();
        lblWebView  = new Label();
        btnCerrar   = new Button();

        SuspendLayout();

        // Form
        this.Text = "Acerca de CodiBloques";
        this.ClientSize = new Size(420, 320);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.StartPosition = FormStartPosition.CenterParent;

        // Panel superior
        pnlTop.Dock = DockStyle.Top;
        pnlTop.Height = 90;
        pnlTop.BackColor = Color.FromArgb(60, 60, 120);

        lblNombre.Text = "CodiBloques";
        lblNombre.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
        lblNombre.ForeColor = Color.White;
        lblNombre.AutoSize = true;
        lblNombre.Location = new Point(20, 10);

        lblSlogan.Text = "Programación visual por bloques en español";
        lblSlogan.Font = new Font("Segoe UI", 9.5F);
        lblSlogan.ForeColor = Color.FromArgb(180, 200, 255);
        lblSlogan.AutoSize = true;
        lblSlogan.Location = new Point(22, 56);

        pnlTop.Controls.Add(lblNombre);
        pnlTop.Controls.Add(lblSlogan);

        // Panel info
        pnlInfo.Dock = DockStyle.Fill;
        pnlInfo.Padding = new Padding(20, 16, 20, 10);

        lblVersion.Text = "Versión 1.0.0";
        lblVersion.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblVersion.Location = new Point(20, 106);
        lblVersion.AutoSize = true;

        lblDesc.Text = "Entorno de programación visual por bloques para\neducación, desarrollado con Blockly y .NET 8.";
        lblDesc.Font = new Font("Segoe UI", 9.5F);
        lblDesc.Location = new Point(20, 132);
        lblDesc.AutoSize = true;
        lblDesc.ForeColor = Color.DimGray;

        lblBlockly.Text = "Powered by Google Blockly";
        lblBlockly.Font = new Font("Segoe UI", 9F);
        lblBlockly.ForeColor = Color.FromArgb(70, 130, 180);
        lblBlockly.Location = new Point(20, 200);
        lblBlockly.AutoSize = true;

        lblWebView.Text = "Usando Microsoft Edge WebView2";
        lblWebView.Font = new Font("Segoe UI", 9F);
        lblWebView.ForeColor = Color.DimGray;
        lblWebView.Location = new Point(20, 222);
        lblWebView.AutoSize = true;

        btnCerrar.Text = "Cerrar";
        btnCerrar.Size = new Size(90, 30);
        btnCerrar.Location = new Point(310, 270);
        btnCerrar.Click += btnCerrar_Click;

        this.Controls.Add(pnlTop);
        this.Controls.Add(lblVersion);
        this.Controls.Add(lblDesc);
        this.Controls.Add(lblBlockly);
        this.Controls.Add(lblWebView);
        this.Controls.Add(btnCerrar);

        ResumeLayout(false);
    }

    private Panel pnlTop;
    private Label lblNombre, lblSlogan, lblVersion, lblDesc, lblBlockly, lblWebView;
    private Panel pnlInfo;
    private Button btnCerrar;
}
