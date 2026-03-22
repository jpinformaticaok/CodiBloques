namespace CodiBloques.Forms;

partial class FormPrincipal
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPrincipal));
        menuStrip1 = new MenuStrip();
        mnuArchivo = new ToolStripMenuItem();
        mnuNuevo = new ToolStripMenuItem();
        mnuAbrir = new ToolStripMenuItem();
        mnuGuardar = new ToolStripMenuItem();
        mnuGuardarComo = new ToolStripMenuItem();
        mnuSeparador1 = new ToolStripSeparator();
        mnuExportarCodigo = new ToolStripMenuItem();
        mnuSeparador2 = new ToolStripSeparator();
        mnuSalir = new ToolStripMenuItem();
        mnuEditar = new ToolStripMenuItem();
        mnuDeshacer = new ToolStripMenuItem();
        mnuRehacer = new ToolStripMenuItem();
        mnuVer = new ToolStripMenuItem();
        mnuMostrarCodigo = new ToolStripMenuItem();
        mnuMostrarConsola = new ToolStripMenuItem();
        mnuMostrarDepuracion = new ToolStripMenuItem();
        mnuSeparador3 = new ToolStripSeparator();
        mnuPantallaCompleta = new ToolStripMenuItem();
        mnuAyuda = new ToolStripMenuItem();
        mnuAcercaDe = new ToolStripMenuItem();
        toolStrip1 = new ToolStrip();
        btnNuevo = new ToolStripButton();
        btnAbrir = new ToolStripButton();
        btnGuardar = new ToolStripButton();
        btnGuardarComo = new ToolStripButton();
        sep1 = new ToolStripSeparator();
        btnEjecutar = new ToolStripButton();
        btnDetener = new ToolStripButton();
        btnDepurar = new ToolStripButton();
        btnPasoSiguiente = new ToolStripButton();
        sep2 = new ToolStripSeparator();
        btnExportarCodigo = new ToolStripButton();
        sep3 = new ToolStripSeparator();
        lblZoomLabel = new ToolStripLabel();
        btnZoomMenos = new ToolStripButton();
        lblZoom = new ToolStripLabel();
        btnZoomMas = new ToolStripButton();
        btnZoomReset = new ToolStripButton();
        sep4 = new ToolStripSeparator();
        btnPantallaCompleta = new ToolStripButton();
        sep5 = new ToolStripSeparator();
        btnArduino = new ToolStripButton();
        sep6 = new ToolStripSeparator();
        btnEditorDisfraces = new ToolStripButton();
        sep7 = new ToolStripSeparator();
        btnGrabador = new ToolStripButton();
        sep8 = new ToolStripSeparator();
        btnPlantillas = new ToolStripButton();
        btnTutoriales = new ToolStripButton();
        sep9 = new ToolStripSeparator();
        btnAcercaDe = new ToolStripButton();
        splitDerecha = new SplitContainer();
        webViewBlockly = new Microsoft.Web.WebView2.WinForms.WebView2();
        tabsInferior = new TabControl();
        tabCodigo = new TabPage();
        txtCodigoGenerado = new RichTextBox();
        tabConsola = new TabPage();
        pnlConsola = new Panel();
        txtConsola = new RichTextBox();
        btnLimpiarConsola = new Button();
        tabDepuracion = new TabPage();
        pnlDebug = new Panel();
        dgvVariables = new DataGridView();
        colNombre = new DataGridViewTextBoxColumn();
        colTipo = new DataGridViewTextBoxColumn();
        colValor = new DataGridViewTextBoxColumn();
        lblDebugInfo = new Label();
        statusStrip1 = new StatusStrip();
        lblEstado = new ToolStripStatusLabel();
        lblInfoBloque = new ToolStripStatusLabel();
        lblVersion = new ToolStripStatusLabel();
        menuStrip1.SuspendLayout();
        toolStrip1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitDerecha).BeginInit();
        splitDerecha.Panel1.SuspendLayout();
        splitDerecha.Panel2.SuspendLayout();
        splitDerecha.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)webViewBlockly).BeginInit();
        tabsInferior.SuspendLayout();
        tabCodigo.SuspendLayout();
        tabConsola.SuspendLayout();
        pnlConsola.SuspendLayout();
        tabDepuracion.SuspendLayout();
        pnlDebug.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvVariables).BeginInit();
        statusStrip1.SuspendLayout();
        SuspendLayout();
        // 
        // menuStrip1
        // 
        menuStrip1.Items.AddRange(new ToolStripItem[] { mnuArchivo, mnuEditar, mnuVer, mnuAyuda });
        menuStrip1.Location = new Point(0, 0);
        menuStrip1.Name = "menuStrip1";
        menuStrip1.Size = new Size(1370, 24);
        menuStrip1.TabIndex = 2;
        // 
        // mnuArchivo
        // 
        mnuArchivo.DropDownItems.AddRange(new ToolStripItem[] { mnuNuevo, mnuAbrir, mnuGuardar, mnuGuardarComo, mnuSeparador1, mnuExportarCodigo, mnuSeparador2, mnuSalir });
        mnuArchivo.Name = "mnuArchivo";
        mnuArchivo.Size = new Size(60, 20);
        mnuArchivo.Text = "&Archivo";
        // 
        // mnuNuevo
        // 
        mnuNuevo.Name = "mnuNuevo";
        mnuNuevo.ShortcutKeys = Keys.Control | Keys.N;
        mnuNuevo.Size = new Size(269, 22);
        mnuNuevo.Text = "&Nuevo";
        mnuNuevo.Click += btnNuevo_Click;
        // 
        // mnuAbrir
        // 
        mnuAbrir.Name = "mnuAbrir";
        mnuAbrir.ShortcutKeys = Keys.Control | Keys.O;
        mnuAbrir.Size = new Size(269, 22);
        mnuAbrir.Text = "&Abrir...";
        mnuAbrir.Click += btnAbrir_Click;
        // 
        // mnuGuardar
        // 
        mnuGuardar.Name = "mnuGuardar";
        mnuGuardar.ShortcutKeys = Keys.Control | Keys.S;
        mnuGuardar.Size = new Size(269, 22);
        mnuGuardar.Text = "&Guardar";
        mnuGuardar.Click += btnGuardar_Click;
        // 
        // mnuGuardarComo
        // 
        mnuGuardarComo.Name = "mnuGuardarComo";
        mnuGuardarComo.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
        mnuGuardarComo.Size = new Size(269, 22);
        mnuGuardarComo.Text = "Guardar &como...";
        mnuGuardarComo.Click += btnGuardarComo_Click;
        // 
        // mnuSeparador1
        // 
        mnuSeparador1.Name = "mnuSeparador1";
        mnuSeparador1.Size = new Size(266, 6);
        // 
        // mnuExportarCodigo
        // 
        mnuExportarCodigo.Name = "mnuExportarCodigo";
        mnuExportarCodigo.Size = new Size(269, 22);
        mnuExportarCodigo.Text = "&Exportar proyecto...";
        mnuExportarCodigo.Click += btnExportarCodigo_Click;
        // 
        // mnuSeparador2
        // 
        mnuSeparador2.Name = "mnuSeparador2";
        mnuSeparador2.Size = new Size(266, 6);
        // 
        // mnuSalir
        // 
        mnuSalir.Name = "mnuSalir";
        mnuSalir.ShortcutKeys = Keys.Alt | Keys.F4;
        mnuSalir.Size = new Size(269, 22);
        mnuSalir.Text = "&Salir";
        mnuSalir.Click += mnuSalir_Click;
        // 
        // mnuEditar
        // 
        mnuEditar.DropDownItems.AddRange(new ToolStripItem[] { mnuDeshacer, mnuRehacer });
        mnuEditar.Name = "mnuEditar";
        mnuEditar.Size = new Size(49, 20);
        mnuEditar.Text = "&Editar";
        // 
        // mnuDeshacer
        // 
        mnuDeshacer.Name = "mnuDeshacer";
        mnuDeshacer.ShortcutKeys = Keys.Control | Keys.Z;
        mnuDeshacer.Size = new Size(163, 22);
        mnuDeshacer.Text = "&Deshacer";
        // 
        // mnuRehacer
        // 
        mnuRehacer.Name = "mnuRehacer";
        mnuRehacer.ShortcutKeys = Keys.Control | Keys.Y;
        mnuRehacer.Size = new Size(163, 22);
        mnuRehacer.Text = "&Rehacer";
        // 
        // mnuVer
        // 
        mnuVer.DropDownItems.AddRange(new ToolStripItem[] { mnuMostrarCodigo, mnuMostrarConsola, mnuMostrarDepuracion, mnuSeparador3, mnuPantallaCompleta });
        mnuVer.Name = "mnuVer";
        mnuVer.Size = new Size(35, 20);
        mnuVer.Text = "&Ver";
        // 
        // mnuMostrarCodigo
        // 
        mnuMostrarCodigo.Checked = true;
        mnuMostrarCodigo.CheckOnClick = true;
        mnuMostrarCodigo.CheckState = CheckState.Checked;
        mnuMostrarCodigo.Name = "mnuMostrarCodigo";
        mnuMostrarCodigo.Size = new Size(266, 22);
        mnuMostrarCodigo.Text = "Mostrar &código generado";
        mnuMostrarCodigo.Click += mnuMostrarCodigo_Click;
        // 
        // mnuMostrarConsola
        // 
        mnuMostrarConsola.Checked = true;
        mnuMostrarConsola.CheckOnClick = true;
        mnuMostrarConsola.CheckState = CheckState.Checked;
        mnuMostrarConsola.Name = "mnuMostrarConsola";
        mnuMostrarConsola.Size = new Size(266, 22);
        mnuMostrarConsola.Text = "Mostrar &consola";
        mnuMostrarConsola.Click += mnuMostrarConsola_Click;
        // 
        // mnuMostrarDepuracion
        // 
        mnuMostrarDepuracion.Checked = true;
        mnuMostrarDepuracion.CheckOnClick = true;
        mnuMostrarDepuracion.CheckState = CheckState.Checked;
        mnuMostrarDepuracion.Name = "mnuMostrarDepuracion";
        mnuMostrarDepuracion.Size = new Size(266, 22);
        mnuMostrarDepuracion.Text = "Mostrar &depuración";
        mnuMostrarDepuracion.Click += mnuMostrarDepuracion_Click;
        // 
        // mnuSeparador3
        // 
        mnuSeparador3.Name = "mnuSeparador3";
        mnuSeparador3.Size = new Size(263, 6);
        // 
        // mnuPantallaCompleta
        // 
        mnuPantallaCompleta.Name = "mnuPantallaCompleta";
        mnuPantallaCompleta.ShortcutKeys = Keys.F11;
        mnuPantallaCompleta.Size = new Size(266, 22);
        mnuPantallaCompleta.Text = "Pantalla completa del escenario";
        mnuPantallaCompleta.Click += mnuPantallaCompleta_Click;
        // 
        // mnuAyuda
        // 
        mnuAyuda.DropDownItems.AddRange(new ToolStripItem[] { mnuAcercaDe });
        mnuAyuda.Name = "mnuAyuda";
        mnuAyuda.Size = new Size(53, 20);
        mnuAyuda.Text = "A&yuda";
        // 
        // mnuAcercaDe
        // 
        mnuAcercaDe.Name = "mnuAcercaDe";
        mnuAcercaDe.Size = new Size(205, 22);
        mnuAcercaDe.Text = "&Acerca de CodiBloques...";
        mnuAcercaDe.Click += btnAcercaDe_Click;
        // 
        // toolStrip1
        // 
        toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
        toolStrip1.Items.AddRange(new ToolStripItem[] { btnNuevo, btnAbrir, btnGuardar, btnGuardarComo, sep1, btnEjecutar, btnDetener, btnDepurar, btnPasoSiguiente, sep2, btnExportarCodigo, sep3, lblZoomLabel, btnZoomMenos, lblZoom, btnZoomMas, btnZoomReset, sep4, btnPantallaCompleta, sep5, btnArduino, sep6, btnEditorDisfraces, sep7, btnGrabador, sep8, btnPlantillas, btnTutoriales, sep9, btnAcercaDe });
        toolStrip1.Location = new Point(0, 24);
        toolStrip1.Name = "toolStrip1";
        toolStrip1.Padding = new Padding(4, 2, 4, 2);
        toolStrip1.Size = new Size(1370, 28);
        toolStrip1.TabIndex = 1;
        // 
        // btnNuevo
        // 
        btnNuevo.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnNuevo.Name = "btnNuevo";
        btnNuevo.Size = new Size(46, 21);
        btnNuevo.Text = "Nuevo";
        btnNuevo.Click += btnNuevo_Click;
        // 
        // btnAbrir
        // 
        btnAbrir.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnAbrir.Name = "btnAbrir";
        btnAbrir.Size = new Size(37, 21);
        btnAbrir.Text = "Abrir";
        btnAbrir.Click += btnAbrir_Click;
        // 
        // btnGuardar
        // 
        btnGuardar.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnGuardar.Name = "btnGuardar";
        btnGuardar.Size = new Size(53, 21);
        btnGuardar.Text = "Guardar";
        btnGuardar.Click += btnGuardar_Click;
        // 
        // btnGuardarComo
        // 
        btnGuardarComo.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnGuardarComo.Name = "btnGuardarComo";
        btnGuardarComo.Size = new Size(87, 21);
        btnGuardarComo.Text = "Guardar como";
        btnGuardarComo.Click += btnGuardarComo_Click;
        // 
        // sep1
        // 
        sep1.Name = "sep1";
        sep1.Size = new Size(6, 24);
        // 
        // btnEjecutar
        // 
        btnEjecutar.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnEjecutar.Enabled = false;
        btnEjecutar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnEjecutar.ForeColor = Color.DarkGreen;
        btnEjecutar.Name = "btnEjecutar";
        btnEjecutar.Size = new Size(77, 21);
        btnEjecutar.Text = "▶ Ejecutar";
        btnEjecutar.ToolTipText = "Ejecutar programa (bandera verde)";
        btnEjecutar.Click += btnEjecutar_Click;
        // 
        // btnDetener
        // 
        btnDetener.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnDetener.ForeColor = Color.DarkRed;
        btnDetener.Name = "btnDetener";
        btnDetener.Size = new Size(67, 21);
        btnDetener.Text = "⏹ Detener";
        btnDetener.Click += btnDetener_Click;
        // 
        // btnDepurar
        // 
        btnDepurar.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnDepurar.Enabled = false;
        btnDepurar.Name = "btnDepurar";
        btnDepurar.Size = new Size(68, 21);
        btnDepurar.Text = "🐞 Depurar";
        btnDepurar.Click += btnDepurar_Click;
        // 
        // btnPasoSiguiente
        // 
        btnPasoSiguiente.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnPasoSiguiente.Enabled = false;
        btnPasoSiguiente.Name = "btnPasoSiguiente";
        btnPasoSiguiente.Size = new Size(49, 21);
        btnPasoSiguiente.Text = "→ Paso";
        btnPasoSiguiente.Click += btnPasoSiguiente_Click;
        // 
        // sep2
        // 
        sep2.Name = "sep2";
        sep2.Size = new Size(6, 24);
        // 
        // btnExportarCodigo
        // 
        btnExportarCodigo.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnExportarCodigo.Name = "btnExportarCodigo";
        btnExportarCodigo.Size = new Size(95, 21);
        btnExportarCodigo.Text = "Exportar código";
        btnExportarCodigo.Click += btnExportarCodigo_Click;
        // 
        // sep3
        // 
        sep3.Name = "sep3";
        sep3.Size = new Size(6, 24);
        // 
        // lblZoomLabel
        // 
        lblZoomLabel.Name = "lblZoomLabel";
        lblZoomLabel.Size = new Size(42, 21);
        lblZoomLabel.Text = "Zoom:";
        // 
        // btnZoomMenos
        // 
        btnZoomMenos.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnZoomMenos.Name = "btnZoomMenos";
        btnZoomMenos.Size = new Size(23, 21);
        btnZoomMenos.Text = "−";
        btnZoomMenos.ToolTipText = "Reducir zoom";
        btnZoomMenos.Click += btnZoomMenos_Click;
        // 
        // lblZoom
        // 
        lblZoom.Name = "lblZoom";
        lblZoom.Size = new Size(35, 21);
        lblZoom.Text = "100%";
        // 
        // btnZoomMas
        // 
        btnZoomMas.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnZoomMas.Name = "btnZoomMas";
        btnZoomMas.Size = new Size(23, 21);
        btnZoomMas.Text = "+";
        btnZoomMas.ToolTipText = "Ampliar zoom";
        btnZoomMas.Click += btnZoomMas_Click;
        // 
        // btnZoomReset
        // 
        btnZoomReset.Name = "btnZoomReset";
        btnZoomReset.Size = new Size(23, 21);
        btnZoomReset.Text = "↺";
        btnZoomReset.ToolTipText = "Restablecer zoom (100%)";
        btnZoomReset.Click += btnZoomReset_Click;
        // 
        // sep4
        // 
        sep4.Name = "sep4";
        sep4.Size = new Size(6, 24);
        // 
        // btnPantallaCompleta
        // 
        btnPantallaCompleta.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnPantallaCompleta.Name = "btnPantallaCompleta";
        btnPantallaCompleta.Size = new Size(74, 21);
        btnPantallaCompleta.Text = "⛶ Escenario";
        btnPantallaCompleta.ToolTipText = "Pantalla completa del escenario (F11)";
        btnPantallaCompleta.Click += mnuPantallaCompleta_Click;
        // 
        // sep5
        // 
        sep5.Name = "sep5";
        sep5.Size = new Size(6, 24);
        // 
        // btnArduino
        // 
        btnArduino.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnArduino.ForeColor = Color.Gray;
        btnArduino.Name = "btnArduino";
        btnArduino.Size = new Size(68, 21);
        btnArduino.Text = "🔌 Arduino";
        btnArduino.ToolTipText = "Configurar conexión con Arduino";
        btnArduino.Click += btnArduino_Click;
        // 
        // sep6
        // 
        sep6.Name = "sep6";
        sep6.Size = new Size(6, 24);
        // 
        // btnEditorDisfraces
        // 
        btnEditorDisfraces.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnEditorDisfraces.Name = "btnEditorDisfraces";
        btnEditorDisfraces.Size = new Size(57, 21);
        btnEditorDisfraces.Text = "🎨 Editor";
        btnEditorDisfraces.ToolTipText = "Abrir editor de disfraces (dibujá tus propios sprites)";
        btnEditorDisfraces.Click += btnEditorDisfraces_Click;
        // 
        // sep7
        // 
        sep7.Name = "sep7";
        sep7.Size = new Size(6, 24);
        // 
        // btnGrabador
        // 
        btnGrabador.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnGrabador.Name = "btnGrabador";
        btnGrabador.Size = new Size(66, 21);
        btnGrabador.Text = "🎙️ Sonidos";
        btnGrabador.ToolTipText = "Abrir grabador de sonidos";
        btnGrabador.Click += btnGrabador_Click;
        // 
        // sep8
        // 
        sep8.Name = "sep8";
        sep8.Size = new Size(6, 24);
        // 
        // btnPlantillas
        // 
        btnPlantillas.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnPlantillas.Name = "btnPlantillas";
        btnPlantillas.Size = new Size(71, 21);
        btnPlantillas.Text = "📋 Plantillas";
        btnPlantillas.ToolTipText = "Abrir una plantilla de proyecto";
        btnPlantillas.Click += btnPlantillas_Click;
        // 
        // btnTutoriales
        // 
        btnTutoriales.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnTutoriales.Name = "btnTutoriales";
        btnTutoriales.Size = new Size(76, 21);
        btnTutoriales.Text = "📚 Tutoriales";
        btnTutoriales.ToolTipText = "Ver tutoriales interactivos";
        btnTutoriales.Click += btnTutoriales_Click;
        // 
        // sep9
        // 
        sep9.Name = "sep9";
        sep9.Size = new Size(6, 24);
        // 
        // btnAcercaDe
        // 
        btnAcercaDe.Alignment = ToolStripItemAlignment.Right;
        btnAcercaDe.Name = "btnAcercaDe";
        btnAcercaDe.Size = new Size(23, 21);
        btnAcercaDe.Text = "?";
        btnAcercaDe.Click += btnAcercaDe_Click;
        // 
        // splitDerecha
        // 
        splitDerecha.Dock = DockStyle.Fill;
        splitDerecha.Location = new Point(0, 52);
        splitDerecha.Name = "splitDerecha";
        splitDerecha.Orientation = Orientation.Horizontal;
        // 
        // splitDerecha.Panel1
        // 
        splitDerecha.Panel1.Controls.Add(webViewBlockly);
        splitDerecha.Panel1MinSize = 200;
        // 
        // splitDerecha.Panel2
        // 
        splitDerecha.Panel2.Controls.Add(tabsInferior);
        splitDerecha.Panel2MinSize = 100;
        splitDerecha.Size = new Size(1370, 673);
        splitDerecha.SplitterDistance = 471;
        splitDerecha.SplitterWidth = 5;
        splitDerecha.TabIndex = 0;
        // 
        // webViewBlockly
        // 
        webViewBlockly.AllowExternalDrop = true;
        webViewBlockly.CreationProperties = null;
        webViewBlockly.DefaultBackgroundColor = Color.White;
        webViewBlockly.Dock = DockStyle.Fill;
        webViewBlockly.Location = new Point(0, 0);
        webViewBlockly.Name = "webViewBlockly";
        webViewBlockly.Size = new Size(1370, 471);
        webViewBlockly.TabIndex = 0;
        webViewBlockly.ZoomFactor = 1D;
        // 
        // tabsInferior
        // 
        tabsInferior.Controls.Add(tabCodigo);
        tabsInferior.Controls.Add(tabConsola);
        tabsInferior.Controls.Add(tabDepuracion);
        tabsInferior.Dock = DockStyle.Fill;
        tabsInferior.Font = new Font("Segoe UI", 9F);
        tabsInferior.Location = new Point(0, 0);
        tabsInferior.Name = "tabsInferior";
        tabsInferior.SelectedIndex = 0;
        tabsInferior.Size = new Size(1370, 197);
        tabsInferior.TabIndex = 0;
        // 
        // tabCodigo
        // 
        tabCodigo.Controls.Add(txtCodigoGenerado);
        tabCodigo.Location = new Point(4, 24);
        tabCodigo.Name = "tabCodigo";
        tabCodigo.Size = new Size(1362, 169);
        tabCodigo.TabIndex = 0;
        tabCodigo.Text = "📄 Código generado";
        // 
        // txtCodigoGenerado
        // 
        txtCodigoGenerado.BackColor = Color.FromArgb(30, 30, 30);
        txtCodigoGenerado.Dock = DockStyle.Fill;
        txtCodigoGenerado.Font = new Font("Consolas", 10F);
        txtCodigoGenerado.ForeColor = Color.FromArgb(220, 220, 170);
        txtCodigoGenerado.Location = new Point(0, 0);
        txtCodigoGenerado.Name = "txtCodigoGenerado";
        txtCodigoGenerado.ReadOnly = true;
        txtCodigoGenerado.Size = new Size(1362, 169);
        txtCodigoGenerado.TabIndex = 0;
        txtCodigoGenerado.Text = "";
        txtCodigoGenerado.WordWrap = false;
        // 
        // tabConsola
        // 
        tabConsola.Controls.Add(pnlConsola);
        tabConsola.Location = new Point(4, 24);
        tabConsola.Name = "tabConsola";
        tabConsola.Size = new Size(1362, 169);
        tabConsola.TabIndex = 1;
        tabConsola.Text = "💬 Consola";
        // 
        // pnlConsola
        // 
        pnlConsola.Controls.Add(txtConsola);
        pnlConsola.Controls.Add(btnLimpiarConsola);
        pnlConsola.Dock = DockStyle.Fill;
        pnlConsola.Location = new Point(0, 0);
        pnlConsola.Name = "pnlConsola";
        pnlConsola.Size = new Size(1362, 169);
        pnlConsola.TabIndex = 0;
        // 
        // txtConsola
        // 
        txtConsola.BackColor = Color.Black;
        txtConsola.Dock = DockStyle.Fill;
        txtConsola.Font = new Font("Consolas", 9.5F);
        txtConsola.ForeColor = Color.LightGreen;
        txtConsola.Location = new Point(0, 0);
        txtConsola.Name = "txtConsola";
        txtConsola.ReadOnly = true;
        txtConsola.ScrollBars = RichTextBoxScrollBars.Vertical;
        txtConsola.Size = new Size(1362, 143);
        txtConsola.TabIndex = 0;
        txtConsola.Text = "";
        // 
        // btnLimpiarConsola
        // 
        btnLimpiarConsola.Dock = DockStyle.Bottom;
        btnLimpiarConsola.Location = new Point(0, 143);
        btnLimpiarConsola.Name = "btnLimpiarConsola";
        btnLimpiarConsola.Size = new Size(1362, 26);
        btnLimpiarConsola.TabIndex = 1;
        btnLimpiarConsola.Text = "Limpiar";
        btnLimpiarConsola.Click += btnLimpiarConsola_Click;
        // 
        // tabDepuracion
        // 
        tabDepuracion.Controls.Add(pnlDebug);
        tabDepuracion.Location = new Point(4, 24);
        tabDepuracion.Name = "tabDepuracion";
        tabDepuracion.Size = new Size(1362, 169);
        tabDepuracion.TabIndex = 2;
        tabDepuracion.Text = "🐞 Depuración";
        // 
        // pnlDebug
        // 
        pnlDebug.Controls.Add(dgvVariables);
        pnlDebug.Controls.Add(lblDebugInfo);
        pnlDebug.Dock = DockStyle.Fill;
        pnlDebug.Location = new Point(0, 0);
        pnlDebug.Name = "pnlDebug";
        pnlDebug.Size = new Size(1362, 169);
        pnlDebug.TabIndex = 0;
        // 
        // dgvVariables
        // 
        dgvVariables.AllowUserToAddRows = false;
        dgvVariables.BackgroundColor = Color.White;
        dgvVariables.Columns.AddRange(new DataGridViewColumn[] { colNombre, colTipo, colValor });
        dgvVariables.Dock = DockStyle.Fill;
        dgvVariables.Location = new Point(0, 22);
        dgvVariables.Name = "dgvVariables";
        dgvVariables.ReadOnly = true;
        dgvVariables.RowHeadersVisible = false;
        dgvVariables.Size = new Size(1362, 147);
        dgvVariables.TabIndex = 0;
        // 
        // colNombre
        // 
        colNombre.HeaderText = "Nombre";
        colNombre.Name = "colNombre";
        colNombre.ReadOnly = true;
        colNombre.Width = 150;
        // 
        // colTipo
        // 
        colTipo.HeaderText = "Tipo";
        colTipo.Name = "colTipo";
        colTipo.ReadOnly = true;
        // 
        // colValor
        // 
        colValor.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colValor.HeaderText = "Valor";
        colValor.Name = "colValor";
        colValor.ReadOnly = true;
        // 
        // lblDebugInfo
        // 
        lblDebugInfo.Dock = DockStyle.Top;
        lblDebugInfo.Location = new Point(0, 0);
        lblDebugInfo.Name = "lblDebugInfo";
        lblDebugInfo.Size = new Size(1362, 22);
        lblDebugInfo.TabIndex = 1;
        lblDebugInfo.Text = "Variables en tiempo de ejecución:";
        // 
        // statusStrip1
        // 
        statusStrip1.Items.AddRange(new ToolStripItem[] { lblEstado, lblInfoBloque, lblVersion });
        statusStrip1.Location = new Point(0, 725);
        statusStrip1.Name = "statusStrip1";
        statusStrip1.Size = new Size(1370, 24);
        statusStrip1.TabIndex = 3;
        // 
        // lblEstado
        // 
        lblEstado.Name = "lblEstado";
        lblEstado.Size = new Size(65, 19);
        lblEstado.Text = "Iniciando...";
        // 
        // lblInfoBloque
        // 
        lblInfoBloque.Name = "lblInfoBloque";
        lblInfoBloque.Size = new Size(1179, 19);
        lblInfoBloque.Spring = true;
        // 
        // lblVersion
        // 
        lblVersion.BorderSides = ToolStripStatusLabelBorderSides.Left;
        lblVersion.Name = "lblVersion";
        lblVersion.Size = new Size(111, 19);
        lblVersion.Text = "CodiBloques v1.0.0";
        // 
        // FormPrincipal
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1370, 749);
        Controls.Add(splitDerecha);
        Controls.Add(toolStrip1);
        Controls.Add(menuStrip1);
        Controls.Add(statusStrip1);
        Icon = (Icon)resources.GetObject("$this.Icon");
        KeyPreview = true;
        MainMenuStrip = menuStrip1;
        MinimumSize = new Size(900, 600);
        Name = "FormPrincipal";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "CodiBloques";
        WindowState = FormWindowState.Maximized;
        Load += FormPrincipal_Load;
        menuStrip1.ResumeLayout(false);
        menuStrip1.PerformLayout();
        toolStrip1.ResumeLayout(false);
        toolStrip1.PerformLayout();
        splitDerecha.Panel1.ResumeLayout(false);
        splitDerecha.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitDerecha).EndInit();
        splitDerecha.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)webViewBlockly).EndInit();
        tabsInferior.ResumeLayout(false);
        tabCodigo.ResumeLayout(false);
        tabConsola.ResumeLayout(false);
        pnlConsola.ResumeLayout(false);
        tabDepuracion.ResumeLayout(false);
        pnlDebug.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvVariables).EndInit();
        statusStrip1.ResumeLayout(false);
        statusStrip1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    // ── Declaraciones ─────────────────────────────────────────────────────────
    private MenuStrip menuStrip1;
    private ToolStripMenuItem mnuArchivo, mnuNuevo, mnuAbrir, mnuGuardar, mnuGuardarComo;
    private ToolStripMenuItem mnuExportarCodigo, mnuSalir;
    private ToolStripMenuItem mnuEditar, mnuDeshacer, mnuRehacer;
    private ToolStripMenuItem mnuVer, mnuMostrarCodigo, mnuMostrarConsola, mnuMostrarDepuracion, mnuPantallaCompleta;
    private ToolStripMenuItem mnuAyuda, mnuAcercaDe;
    private ToolStripSeparator mnuSeparador1, mnuSeparador2, mnuSeparador3;

    private ToolStrip toolStrip1;
    private ToolStripButton btnNuevo, btnAbrir, btnGuardar, btnGuardarComo;
    private ToolStripButton btnEjecutar, btnDetener, btnDepurar, btnPasoSiguiente;
    private ToolStripButton btnExportarCodigo, btnZoomReset, btnPantallaCompleta, btnArduino, btnEditorDisfraces, btnGrabador, btnPlantillas, btnTutoriales, btnAcercaDe;
    private ToolStripLabel lblZoomLabel, lblZoom;
    private ToolStripSeparator sep1, sep2, sep3, sep4, sep5, sep6, sep7, sep8, sep9;
    private ToolStripButton btnZoomMenos, btnZoomMas;

    private SplitContainer splitDerecha;
    private Microsoft.Web.WebView2.WinForms.WebView2 webViewBlockly;

    private TabControl tabsInferior;
    private TabPage tabCodigo, tabConsola, tabDepuracion;
    private RichTextBox txtCodigoGenerado, txtConsola;
    private Panel pnlConsola, pnlDebug;
    private Button btnLimpiarConsola;
    private Label lblDebugInfo;
    private DataGridView dgvVariables;
    private DataGridViewTextBoxColumn colNombre, colTipo, colValor;

    private StatusStrip statusStrip1;
    private ToolStripStatusLabel lblEstado, lblInfoBloque, lblVersion;
}
