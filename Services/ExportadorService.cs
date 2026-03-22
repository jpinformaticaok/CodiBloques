using Newtonsoft.Json;

namespace CodiBloques.Services;

public class ExportadorService
{
    private readonly string _wwwroot;

    public ExportadorService()
    {
        _wwwroot = Path.Combine(Application.StartupPath, "wwwroot");
    }

    public async Task ExportarHtmlAsync(string xmlBlockly, string rutaDestino, OpcionesExportacion opciones)
    {
        string htmlBase  = await File.ReadAllTextAsync(Path.Combine(_wwwroot, "index.html"));
        string htmlFinal = ProcesarHtml(htmlBase, xmlBlockly, opciones);
        await File.WriteAllTextAsync(rutaDestino, htmlFinal, System.Text.Encoding.UTF8);
    }

    public async Task ExportarPaqueteHtmlAsync(string xmlBlockly, string carpetaDestino, OpcionesExportacion opciones)
    {
        Directory.CreateDirectory(carpetaDestino);

        string htmlBase  = await File.ReadAllTextAsync(Path.Combine(_wwwroot, "index.html"));
        string htmlFinal = ProcesarHtmlParaExe(htmlBase, xmlBlockly, opciones);
        await File.WriteAllTextAsync(Path.Combine(carpetaDestino, "index.html"), htmlFinal);

        string blocklyLocal = Path.Combine(_wwwroot, "blockly.min.js");
        if (File.Exists(blocklyLocal))
            File.Copy(blocklyLocal, Path.Combine(carpetaDestino, "blockly.min.js"), true);

        string bat =
            "@echo off\r\n" +
            "title " + EscaparBat(opciones.NombreProyecto) + "\r\n" +
            "start \"\" \"%~dp0index.html\"\r\n";
        await File.WriteAllTextAsync(Path.Combine(carpetaDestino, "Ejecutar.bat"), bat);

        string readme =
            "# " + opciones.NombreProyecto + "\r\n" +
            "Creado con CodiBloques\r\n" +
            "Autor: " + opciones.Autor + "\r\n\r\n" +
            "## Como ejecutar\r\n" +
            "Hace doble clic en Ejecutar.bat\r\n" +
            "O abri index.html en tu navegador.\r\n";
        await File.WriteAllTextAsync(Path.Combine(carpetaDestino, "LEEME.txt"), readme);
    }

    public async Task ExportarExeAsync(string xmlBlockly, string rutaDestino, OpcionesExportacion opciones)
    {
        string carpetaTemp = Path.Combine(Path.GetTempPath(), "CodiBloques_Export_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(carpetaTemp);

        try
        {
            string blocklyJs = Path.Combine(_wwwroot, "blockly.min.js");
            if (File.Exists(blocklyJs))
                File.Copy(blocklyJs, Path.Combine(carpetaTemp, "blockly.min.js"));

            string htmlBase  = await File.ReadAllTextAsync(Path.Combine(_wwwroot, "index.html"));
            string htmlFinal = ProcesarHtmlParaExe(htmlBase, xmlBlockly, opciones);
            await File.WriteAllTextAsync(Path.Combine(carpetaTemp, "proyecto.html"), htmlFinal);

            bool compilado = await CompilarLauncher(rutaDestino, carpetaTemp, opciones);

            if (!compilado)
            {
                string carpetaFallback = Path.ChangeExtension(rutaDestino, null);
                await ExportarPaqueteHtmlAsync(xmlBlockly, carpetaFallback, opciones);
            }
        }
        finally
        {
            try { Directory.Delete(carpetaTemp, true); } catch { }
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private string ProcesarHtml(string html, string xmlBlockly, OpcionesExportacion opciones)
    {
        string blocklyLocal = Path.Combine(_wwwroot, "blockly.min.js");
        if (File.Exists(blocklyLocal))
            html = html.Replace(
                "https://cdn.jsdelivr.net/npm/blockly@10.4.3/blockly.min.js",
                "blockly.min.js");

        if (opciones.SoloEscenario)
            html = html.Replace("id=\"blocklyDiv\"", "id=\"blocklyDiv\" style=\"display:none\"");

        return html;
    }

    private string ProcesarHtmlParaExe(string html, string xmlBlockly, OpcionesExportacion opciones)
    {
        html = ProcesarHtml(html, xmlBlockly, opciones);

        string xmlEscapado = xmlBlockly
            .Replace("\\", "\\\\")
            .Replace("\"", "\\\"")
            .Replace("\n", "\\n")
            .Replace("\r", "");

        string autoEjecutarJs = opciones.AutoEjecutar
            ? "setTimeout(function(){ window.codiBlockly.ejecutar(); }, 500);"
            : "";

        string autoload =
            "// Auto-carga del proyecto exportado\n" +
            "setTimeout(function() {\n" +
            "  try {\n" +
            "    var xml = \"" + xmlEscapado + "\";\n" +
            "    if(xml && window.codiBlockly) {\n" +
            "      window.codiBlockly.cargarXml(xml);\n" +
            "      " + autoEjecutarJs + "\n" +
            "    }\n" +
            "  } catch(e) { console.error('Error al cargar proyecto:', e); }\n" +
            "}, 800);\n";

        html = html.Replace("</script>\n</body>", autoload + "</script>\n</body>");

        string titulo = EscaparHtml(opciones.NombreProyecto) + " — CodiBloques";
        string meta =
            "<meta name=\"project-name\" content=\"" + EscaparHtml(opciones.NombreProyecto) + "\" />\n" +
            "<meta name=\"project-author\" content=\"" + EscaparHtml(opciones.Autor) + "\" />\n" +
            "<title>" + titulo + "</title>";

        html = html.Replace("<title>CodiBloques</title>", meta);
        return html;
    }

    private async Task<bool> CompilarLauncher(string exeDestino, string carpetaTemp, OpcionesExportacion opciones)
    {
        string[] posiblesCsc = {
            @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe",
            @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe"
        };

        string? csc = posiblesCsc.FirstOrDefault(File.Exists);
        if (csc == null) return false;

        string nombreEsc = EscaparCs(opciones.NombreProyecto);
        string codigo =
            "using System;\n" +
            "using System.IO;\n" +
            "using System.Windows.Forms;\n" +
            "using Microsoft.Web.WebView2.WinForms;\n" +
            "using Microsoft.Web.WebView2.Core;\n\n" +
            "[STAThread]\n" +
            "static void Main() {\n" +
            "    Application.EnableVisualStyles();\n" +
            "    var form = new Form {\n" +
            "        Text = \"" + nombreEsc + "\",\n" +
            "        ClientSize = new System.Drawing.Size(1024, 720),\n" +
            "        WindowState = FormWindowState.Maximized\n" +
            "    };\n" +
            "    var wv = new WebView2 { Dock = DockStyle.Fill };\n" +
            "    form.Controls.Add(wv);\n" +
            "    form.Load += async (s,e) => {\n" +
            "        await wv.EnsureCoreWebView2Async();\n" +
            "        wv.CoreWebView2.SetVirtualHostNameToFolderMapping(\"app\",\n" +
            "            Path.GetDirectoryName(Application.ExecutablePath),\n" +
            "            CoreWebView2HostResourceAccessKind.Allow);\n" +
            "        wv.CoreWebView2.Navigate(\"https://app/proyecto.html\");\n" +
            "    };\n" +
            "    Application.Run(form);\n" +
            "}\n";

        string archivoCs = Path.Combine(carpetaTemp, "launcher.cs");
        await File.WriteAllTextAsync(archivoCs, codigo);

        try
        {
            var psi = new System.Diagnostics.ProcessStartInfo(csc,
                "/target:winexe /out:\"" + exeDestino + "\" \"" + archivoCs + "\"")
            {
                RedirectStandardOutput = true,
                RedirectStandardError  = true,
                UseShellExecute        = false,
                CreateNoWindow         = true
            };
            var proc = System.Diagnostics.Process.Start(psi)!;
            await proc.WaitForExitAsync();
            return proc.ExitCode == 0 && File.Exists(exeDestino);
        }
        catch { return false; }
    }

    private static string EscaparHtml(string s) =>
        s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");

    private static string EscaparCs(string s) =>
        s.Replace("\\", "\\\\").Replace("\"", "\\\"");

    private static string EscaparBat(string s) =>
        s.Replace("%", "%%").Replace("\"", "\\\"");
}

public class OpcionesExportacion
{
    public string NombreProyecto { get; set; } = "MiProyecto";
    public string Autor          { get; set; } = "";
    public bool   AutoEjecutar   { get; set; } = true;
    public bool   SoloEscenario  { get; set; } = false;
}
