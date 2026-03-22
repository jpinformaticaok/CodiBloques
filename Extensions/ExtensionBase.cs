namespace CodiBloques.Extensions;

/// <summary>
/// Interfaz base que deben implementar todas las extensiones de CodiBloques.
/// Permite agregar nuevas categorías de bloques (ej: Arduino, sensores, etc.)
/// </summary>
public interface IExtensionCodiBloques
{
    /// <summary>Nombre visible de la extensión</summary>
    string Nombre { get; }

    /// <summary>Descripción breve de la extensión</summary>
    string Descripcion { get; }

    /// <summary>Versión de la extensión</summary>
    string Version { get; }

    /// <summary>
    /// Retorna el JSON de definición de bloques para Blockly
    /// (bloque personalizado en formato toolbox JSON de Blockly v10+)
    /// </summary>
    string ObtenerDefinicionBloques();

    /// <summary>
    /// Retorna el código JavaScript que define los generadores de código
    /// para cada bloque de esta extensión
    /// </summary>
    string ObtenerGeneradoresCodigo();

    /// <summary>
    /// Inicialización de la extensión (ej: detectar puerto COM para Arduino)
    /// </summary>
    Task InicializarAsync();
}

/// <summary>
/// Clase base con implementación por defecto para facilitar la creación de extensiones
/// </summary>
public abstract class ExtensionBase : IExtensionCodiBloques
{
    public abstract string Nombre { get; }
    public abstract string Descripcion { get; }
    public virtual string Version => "1.0.0";

    public abstract string ObtenerDefinicionBloques();
    public abstract string ObtenerGeneradoresCodigo();

    public virtual Task InicializarAsync() => Task.CompletedTask;
}
