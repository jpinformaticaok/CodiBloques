using Newtonsoft.Json;

namespace CodiBloques.Services;

/// <summary>
/// Servicio encargado de guardar y cargar proyectos CodiBloques (.codi)
/// </summary>
public class ProyectoService
{
    public const string ExtensionProyecto = ".codi";
    public const string FiltroProyecto = "Proyecto CodiBloques (*.codi)|*.codi|Todos los archivos (*.*)|*.*";

    /// <summary>Guarda el XML de Blockly en un archivo .codi</summary>
    public async Task GuardarAsync(string rutaArchivo, string xmlBlockly)
    {
        var proyecto = new ProyectoData
        {
            Version = "1.0",
            FechaGuardado = DateTime.Now,
            XmlBlockly = xmlBlockly
        };
        string json = JsonConvert.SerializeObject(proyecto, Formatting.Indented);
        await File.WriteAllTextAsync(rutaArchivo, json);
    }

    /// <summary>Carga un archivo .codi y retorna el XML de Blockly</summary>
    public async Task<string> CargarAsync(string rutaArchivo)
    {
        string contenido = await File.ReadAllTextAsync(rutaArchivo);

        // Compatibilidad: si es XML directo (formato antiguo) lo devuelve tal cual
        if (contenido.TrimStart().StartsWith("<xml"))
            return contenido;

        // Formato JSON moderno
        var proyecto = JsonConvert.DeserializeObject<ProyectoData>(contenido)
            ?? throw new InvalidDataException("El archivo no es un proyecto CodiBloques válido.");

        return proyecto.XmlBlockly;
    }
}

public class ProyectoData
{
    public string Version { get; set; } = "1.0";
    public DateTime FechaGuardado { get; set; }
    public string XmlBlockly { get; set; } = string.Empty;
}
