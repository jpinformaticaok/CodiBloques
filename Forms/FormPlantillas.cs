using Newtonsoft.Json;

namespace CodiBloques.Forms;

public partial class FormPlantillas : Form
{
    public string? PlantillaSeleccionada { get; private set; }
    public string? XmlSeleccionado       { get; private set; }

    private readonly string _carpetaPlantillas;
    private List<PlantillaInfo> _plantillas = new();

    public FormPlantillas()
    {
        InitializeComponent();
        _carpetaPlantillas = Path.Combine(Application.StartupPath, "wwwroot", "plantillas");
    }

    private async void FormPlantillas_Load(object sender, EventArgs e)
    {
        await CargarPlantillas();
    }

    private async Task CargarPlantillas()
    {
        try
        {
            string jsonPath = Path.Combine(_carpetaPlantillas, "plantillas.json");
            string json     = await File.ReadAllTextAsync(jsonPath);
            _plantillas     = JsonConvert.DeserializeObject<List<PlantillaInfo>>(json) ?? new();
            MostrarPlantillas(_plantillas);
        }
        catch (Exception ex)
        {
            lblMensaje.Text = $"Error al cargar plantillas: {ex.Message}";
        }
    }

    private void MostrarPlantillas(List<PlantillaInfo> lista)
    {
        pnlPlantillas.Controls.Clear();
        foreach (var p in lista)
        {
            var tarjeta = CrearTarjeta(p);
            pnlPlantillas.Controls.Add(tarjeta);
        }
    }

    private Panel CrearTarjeta(PlantillaInfo p)
    {
        var panel = new Panel
        {
            Size      = new Size(200, 150),
            Margin    = new Padding(8),
            BackColor = Color.FromArgb(30, 30, 60),
            Cursor    = Cursors.Hand,
            Tag       = p
        };
        panel.Paint += (s, e) =>
        {
            e.Graphics.DrawRectangle(new Pen(Color.FromArgb(60, 60, 120), 1),
                0, 0, panel.Width - 1, panel.Height - 1);
        };

        var lblEmoji = new Label
        {
            Text      = p.Emoji,
            Font      = new Font("Segoe UI Emoji", 28F),
            Location  = new Point(10, 10),
            Size      = new Size(50, 45),
            TextAlign = ContentAlignment.MiddleCenter
        };

        var lblNombre = new Label
        {
            Text      = p.Nombre,
            Font      = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = Color.White,
            Location  = new Point(65, 12),
            Size      = new Size(128, 20),
            TextAlign = ContentAlignment.MiddleLeft
        };

        var lblCategoria = new Label
        {
            Text      = $"{p.Categoria} · {p.Nivel}",
            Font      = new Font("Segoe UI", 8F),
            ForeColor = Color.FromArgb(100, 180, 255),
            Location  = new Point(65, 34),
            Size      = new Size(128, 16)
        };

        var lblDesc = new Label
        {
            Text      = p.Descripcion,
            Font      = new Font("Segoe UI", 8.5F),
            ForeColor = Color.FromArgb(180, 180, 200),
            Location  = new Point(10, 62),
            Size      = new Size(180, 50),
            TextAlign = ContentAlignment.TopLeft
        };

        var btnUsar = new Button
        {
            Text      = "Usar esta plantilla →",
            Font      = new Font("Segoe UI", 8.5F),
            ForeColor = Color.White,
            BackColor = Color.FromArgb(60, 60, 140),
            FlatStyle = FlatStyle.Flat,
            Location  = new Point(10, 118),
            Size      = new Size(180, 26),
            Tag       = p
        };
        btnUsar.FlatAppearance.BorderColor = Color.FromArgb(100, 100, 200);
        btnUsar.Click += BtnUsar_Click;

        panel.Controls.AddRange(new Control[] { lblEmoji, lblNombre, lblCategoria, lblDesc, btnUsar });

        // Hover effect
        panel.MouseEnter += (s, e2) => panel.BackColor = Color.FromArgb(40, 40, 80);
        panel.MouseLeave += (s, e2) => panel.BackColor = Color.FromArgb(30, 30, 60);

        return panel;
    }

    private async void BtnUsar_Click(object? sender, EventArgs e)
    {
        if (sender is not Button btn || btn.Tag is not PlantillaInfo p) return;

        try
        {
            string ruta = Path.Combine(_carpetaPlantillas, p.Archivo);
            string json = await File.ReadAllTextAsync(ruta);
            var   data  = JsonConvert.DeserializeObject<dynamic>(json);
            XmlSeleccionado       = data?.XmlBlockly?.ToString() ?? "";
            PlantillaSeleccionada = p.Nombre;
            DialogResult          = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"No se pudo cargar la plantilla:\n{ex.Message}",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void txtBuscar_TextChanged(object sender, EventArgs e)
    {
        string busq = txtBuscar.Text.ToLower();
        var filtradas = string.IsNullOrWhiteSpace(busq)
            ? _plantillas
            : _plantillas.Where(p =>
                p.Nombre.ToLower().Contains(busq) ||
                p.Categoria.ToLower().Contains(busq) ||
                p.Nivel.ToLower().Contains(busq)).ToList();
        MostrarPlantillas(filtradas);
    }

    private void btnCerrar_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}

public class PlantillaInfo
{
    public string Archivo     { get; set; } = "";
    public string Nombre      { get; set; } = "";
    public string Descripcion { get; set; } = "";
    public string Categoria   { get; set; } = "";
    public string Nivel       { get; set; } = "";
    public string Emoji       { get; set; } = "📄";
}
