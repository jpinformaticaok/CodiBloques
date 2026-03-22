using CodiBloques.Services;

namespace CodiBloques.Forms;

public partial class FormExportar : Form
{
    private readonly string _xmlBlockly;
    private readonly ExportadorService _exportador;

    public FormExportar(string xmlBlockly)
    {
        InitializeComponent();
        _xmlBlockly = xmlBlockly;
        _exportador = new ExportadorService();
    }

    private void FormExportar_Load(object sender, EventArgs e)
    {
        txtNombre.Text = "MiProyecto";
        txtAutor.Text  = Environment.UserName;
    }

    private async void btnExportarHtml_Click(object sender, EventArgs e)
    {
        using var dlg = new SaveFileDialog
        {
            Title      = "Exportar como HTML",
            Filter     = "Página web (*.html)|*.html",
            DefaultExt = "html",
            FileName   = txtNombre.Text
        };
        if (dlg.ShowDialog() != DialogResult.OK) return;

        await EjecutarExportacion(async () =>
        {
            var opts = ObtenerOpciones();
            await _exportador.ExportarHtmlAsync(_xmlBlockly, dlg.FileName, opts);
            MostrarExito($"HTML exportado:\n{dlg.FileName}");
        });
    }

    private async void btnExportarPaquete_Click(object sender, EventArgs e)
    {
        using var dlg = new FolderBrowserDialog
        {
            Description  = "Elegí la carpeta donde guardar el paquete",
            UseDescriptionForTitle = true
        };
        if (dlg.ShowDialog() != DialogResult.OK) return;

        string carpeta = Path.Combine(dlg.SelectedPath, txtNombre.Text);

        await EjecutarExportacion(async () =>
        {
            var opts = ObtenerOpciones();
            await _exportador.ExportarPaqueteHtmlAsync(_xmlBlockly, carpeta, opts);
            MostrarExito($"Paquete exportado en:\n{carpeta}\n\nAbrí 'Ejecutar.bat' para correr el proyecto.");
        });
    }

    private async void btnExportarExe_Click(object sender, EventArgs e)
    {
        using var dlg = new SaveFileDialog
        {
            Title      = "Exportar como ejecutable",
            Filter     = "Ejecutable (*.exe)|*.exe",
            DefaultExt = "exe",
            FileName   = txtNombre.Text
        };
        if (dlg.ShowDialog() != DialogResult.OK) return;

        lblEstado.Text = "Compilando... esto puede tardar unos segundos.";
        lblEstado.ForeColor = Color.DarkOrange;

        await EjecutarExportacion(async () =>
        {
            var opts = ObtenerOpciones();
            await _exportador.ExportarExeAsync(_xmlBlockly, dlg.FileName, opts);

            if (File.Exists(dlg.FileName))
                MostrarExito($"Ejecutable creado:\n{dlg.FileName}");
            else
            {
                // Si no se pudo compilar, ofrecer paquete HTML
                var r = MessageBox.Show(
                    "No se pudo crear el .exe (se necesita .NET Framework instalado).\n\n" +
                    "¿Exportar como paquete HTML en su lugar? Funciona igual abriendo 'Ejecutar.bat'.",
                    "CodiBloques", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (r == DialogResult.Yes)
                {
                    string carpeta = Path.ChangeExtension(dlg.FileName, null);
                    await _exportador.ExportarPaqueteHtmlAsync(_xmlBlockly, carpeta, opts);
                    MostrarExito($"Paquete exportado en:\n{carpeta}");
                }
            }
        });
    }

    private async Task EjecutarExportacion(Func<Task> accion)
    {
        btnExportarHtml.Enabled    = false;
        btnExportarPaquete.Enabled = false;
        btnExportarExe.Enabled     = false;
        lblEstado.Text             = "Exportando...";
        lblEstado.ForeColor        = Color.DarkOrange;

        try
        {
            await accion();
        }
        catch (Exception ex)
        {
            lblEstado.Text      = $"Error: {ex.Message}";
            lblEstado.ForeColor = Color.Red;
            MessageBox.Show($"Error al exportar:\n{ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnExportarHtml.Enabled    = true;
            btnExportarPaquete.Enabled = true;
            btnExportarExe.Enabled     = true;
        }
    }

    private void MostrarExito(string mensaje)
    {
        lblEstado.Text      = "✓ Exportación completada";
        lblEstado.ForeColor = Color.DarkGreen;
        MessageBox.Show(mensaje, "¡Exportado!", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private OpcionesExportacion ObtenerOpciones() => new()
    {
        NombreProyecto = txtNombre.Text.Trim().Length > 0 ? txtNombre.Text.Trim() : "MiProyecto",
        Autor          = txtAutor.Text.Trim(),
        AutoEjecutar   = chkAutoEjecutar.Checked,
        SoloEscenario  = chkSoloEscenario.Checked
    };

    private void btnCerrar_Click(object sender, EventArgs e) => Close();
}
