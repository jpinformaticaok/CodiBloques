using System.IO.Ports;
using Newtonsoft.Json;

namespace CodiBloques.Arduino;

/// <summary>
/// Servicio que gestiona la comunicación serial con Arduino.
/// Protocolo: comandos JSON por línea → {"cmd":"...", "pin":N, "valor":N}
/// Respuestas: JSON por línea → {"ok":true, "valor":N}
/// </summary>
public class ArduinoService : IDisposable
{
    private SerialPort? _puerto;
    private readonly Queue<string> _colaMensajes = new();
    private readonly SemaphoreSlim _semaforo = new(1, 1);
    private bool _conectado = false;
    private string _nombrePuerto = "";

    public bool Conectado => _conectado;
    public string PuertoActual => _nombrePuerto;

    // Evento para datos recibidos libremente (modo serial básico)
    public event EventHandler<string>? DatoRecibido;
    public event EventHandler<string>? ErrorOcurrido;

    // ── Conexión ──────────────────────────────────────────────────────────────

    public async Task<bool> ConectarAsync(string puerto, int baudRate = 9600)
    {
        try
        {
            await DesconectarAsync();

            _puerto = new SerialPort(puerto, baudRate)
            {
                ReadTimeout  = 2000,
                WriteTimeout = 2000,
                NewLine      = "\n"
            };

            _puerto.DataReceived += Puerto_DataReceived;
            _puerto.Open();

            // Esperar a que Arduino se reinicie (resetea al abrir el puerto)
            await Task.Delay(2000);

            _conectado    = true;
            _nombrePuerto = puerto;
            return true;
        }
        catch (Exception ex)
        {
            ErrorOcurrido?.Invoke(this, $"Error al conectar a {puerto}: {ex.Message}");
            _conectado = false;
            return false;
        }
    }

    public async Task DesconectarAsync()
    {
        if (_puerto != null)
        {
            try
            {
                if (_puerto.IsOpen) _puerto.Close();
                _puerto.Dispose();
            }
            catch { }
            _puerto = null;
        }
        _conectado = false;
        _nombrePuerto = "";
        await Task.CompletedTask;
    }

    // ── Comandos principales ──────────────────────────────────────────────────

    /// <summary>Escribe HIGH o LOW en un pin digital</summary>
    public async Task<bool> DigitalWriteAsync(int pin, bool valor)
    {
        var cmd = new { cmd = "dw", pin, valor = valor ? 1 : 0 };
        return await EnviarComandoAsync(cmd);
    }

    /// <summary>Lee el valor de un pin digital (true = HIGH)</summary>
    public async Task<bool> DigitalReadAsync(int pin)
    {
        var cmd = new { cmd = "dr", pin };
        var resp = await EnviarComandoConRespuestaAsync(cmd);
        return resp?["valor"]?.ToObject<int>() == 1;
    }

    /// <summary>Lee el valor analógico de un pin (0-1023)</summary>
    public async Task<int> AnalogReadAsync(int pin)
    {
        var cmd = new { cmd = "ar", pin };
        var resp = await EnviarComandoConRespuestaAsync(cmd);
        return resp?["valor"]?.ToObject<int>() ?? 0;
    }

    /// <summary>Escribe un valor PWM en un pin analógico (0-255)</summary>
    public async Task<bool> AnalogWriteAsync(int pin, int valor)
    {
        valor = Math.Clamp(valor, 0, 255);
        var cmd = new { cmd = "aw", pin, valor };
        return await EnviarComandoAsync(cmd);
    }

    /// <summary>Mueve un servo a un ángulo (0-180°)</summary>
    public async Task<bool> ServoWriteAsync(int pin, int angulo)
    {
        angulo = Math.Clamp(angulo, 0, 180);
        var cmd = new { cmd = "sv", pin, valor = angulo };
        return await EnviarComandoAsync(cmd);
    }

    /// <summary>Configura un pin como INPUT o OUTPUT</summary>
    public async Task<bool> PinModeAsync(int pin, string modo)
    {
        var cmd = new { cmd = "pm", pin, valor = modo.ToUpper() == "OUTPUT" ? 1 : 0 };
        return await EnviarComandoAsync(cmd);
    }

    /// <summary>Envía un texto libre por serial</summary>
    public async Task<bool> EnviarTextoAsync(string texto)
    {
        if (_puerto == null || !_puerto.IsOpen) return false;
        try
        {
            await _semaforo.WaitAsync();
            _puerto.WriteLine(texto);
            return true;
        }
        catch (Exception ex)
        {
            ErrorOcurrido?.Invoke(this, $"Error al enviar: {ex.Message}");
            return false;
        }
        finally { _semaforo.Release(); }
    }

    /// <summary>Lee una línea de texto del serial (timeout 3s)</summary>
    public async Task<string> LeerLineaAsync()
    {
        if (_puerto == null || !_puerto.IsOpen) return "";
        return await Task.Run(() =>
        {
            try { return _puerto.ReadLine().Trim(); }
            catch { return ""; }
        });
    }

    // ── Comunicación interna ──────────────────────────────────────────────────

    private async Task<bool> EnviarComandoAsync(object comando)
    {
        if (_puerto == null || !_puerto.IsOpen) return false;
        try
        {
            await _semaforo.WaitAsync();
            string json = JsonConvert.SerializeObject(comando);
            _puerto.WriteLine(json);
            return true;
        }
        catch (Exception ex)
        {
            ErrorOcurrido?.Invoke(this, $"Error de comunicación: {ex.Message}");
            _conectado = false;
            return false;
        }
        finally { _semaforo.Release(); }
    }

    private async Task<Newtonsoft.Json.Linq.JObject?> EnviarComandoConRespuestaAsync(object comando)
    {
        if (_puerto == null || !_puerto.IsOpen) return null;
        try
        {
            await _semaforo.WaitAsync();
            string json = JsonConvert.SerializeObject(comando);
            _puerto.WriteLine(json);

            // Esperar respuesta con timeout
            string resp = await Task.Run(() =>
            {
                try { return _puerto.ReadLine().Trim(); }
                catch { return "{}"; }
            });

            return JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(resp);
        }
        catch (Exception ex)
        {
            ErrorOcurrido?.Invoke(this, $"Error de comunicación: {ex.Message}");
            return null;
        }
        finally { _semaforo.Release(); }
    }

    private void Puerto_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        try
        {
            if (_puerto == null || !_puerto.IsOpen) return;
            string linea = _puerto.ReadLine().Trim();
            if (!string.IsNullOrEmpty(linea))
                DatoRecibido?.Invoke(this, linea);
        }
        catch { }
    }

    // ── Utilidades ────────────────────────────────────────────────────────────

    /// <summary>Devuelve los puertos COM disponibles en el sistema</summary>
    public static string[] ObtenerPuertos() => SerialPort.GetPortNames();

    public void Dispose()
    {
        _puerto?.Close();
        _puerto?.Dispose();
        _semaforo.Dispose();
    }
}
