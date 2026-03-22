/*
 * CodiBloques - Firmware para Arduino
 * =====================================
 * Cargá este sketch en tu Arduino usando el Arduino IDE.
 * Permite que CodiBloques controle los pines del Arduino
 * a través del puerto serial con comandos JSON.
 *
 * Comandos soportados (JSON por línea):
 *   {"cmd":"dw","pin":13,"valor":1}    → digitalWrite(13, HIGH)
 *   {"cmd":"dw","pin":13,"valor":0}    → digitalWrite(13, LOW)
 *   {"cmd":"dr","pin":2}               → digitalRead(2)
 *   {"cmd":"ar","pin":0}               → analogRead(A0)
 *   {"cmd":"aw","pin":9,"valor":128}   → analogWrite(9, 128) PWM
 *   {"cmd":"sv","pin":9,"valor":90}    → servo.write(90)
 *   {"cmd":"pm","pin":2,"valor":1}     → pinMode(2, OUTPUT)
 *   {"cmd":"pm","pin":2,"valor":0}     → pinMode(2, INPUT)
 *
 * Respuestas:
 *   {"ok":true}
 *   {"ok":true,"valor":512}
 *   {"ok":false,"error":"pin invalido"}
 */

#include <Servo.h>

// Servos (máximo 8)
Servo servos[8];
int servoPines[8] = {-1,-1,-1,-1,-1,-1,-1,-1};

// Buffer para leer comandos
String inputBuffer = "";
bool commandReady = false;

void setup() {
  Serial.begin(9600);
  // LED incorporado como pin de prueba
  pinMode(LED_BUILTIN, OUTPUT);
  // Señal de inicio
  Serial.println("{\"ok\":true,\"msg\":\"CodiBloques listo\"}");
}

void loop() {
  // Leer caracteres del serial
  while (Serial.available()) {
    char c = (char)Serial.read();
    if (c == '\n' || c == '\r') {
      if (inputBuffer.length() > 0) {
        commandReady = true;
        break;
      }
    } else {
      inputBuffer += c;
    }
  }

  // Procesar comando si está listo
  if (commandReady) {
    procesarComando(inputBuffer);
    inputBuffer = "";
    commandReady = false;
  }
}

void procesarComando(String json) {
  // Parser JSON manual simple (sin librerías externas)
  String cmd   = extraerValor(json, "cmd");
  int pin      = extraerNumero(json, "pin");
  int valor    = extraerNumero(json, "valor");

  if (cmd == "") {
    // Modo texto libre: reenviar el mensaje tal cual
    Serial.println("{\"ok\":true,\"echo\":\"" + json + "\"}");
    return;
  }

  if (cmd == "dw") {
    // digitalWrite
    if (pin < 0 || pin > 53) { enviarError("pin invalido"); return; }
    digitalWrite(pin, valor == 1 ? HIGH : LOW);
    enviarOk();

  } else if (cmd == "dr") {
    // digitalRead
    if (pin < 0 || pin > 53) { enviarError("pin invalido"); return; }
    int lectura = digitalRead(pin);
    Serial.println("{\"ok\":true,\"valor\":" + String(lectura) + "}");

  } else if (cmd == "ar") {
    // analogRead
    if (pin < 0 || pin > 15) { enviarError("pin invalido"); return; }
    int lectura = analogRead(pin);
    Serial.println("{\"ok\":true,\"valor\":" + String(lectura) + "}");

  } else if (cmd == "aw") {
    // analogWrite (PWM)
    if (pin < 0 || pin > 53) { enviarError("pin invalido"); return; }
    valor = constrain(valor, 0, 255);
    analogWrite(pin, valor);
    enviarOk();

  } else if (cmd == "sv") {
    // Servo write
    if (pin < 0 || pin > 53) { enviarError("pin invalido"); return; }
    valor = constrain(valor, 0, 180);
    int idx = obtenerIndiceServo(pin);
    if (idx == -1) {
      idx = agregarServo(pin);
      if (idx == -1) { enviarError("max servos alcanzado"); return; }
    }
    servos[idx].write(valor);
    enviarOk();

  } else if (cmd == "pm") {
    // pinMode
    if (pin < 0 || pin > 53) { enviarError("pin invalido"); return; }
    pinMode(pin, valor == 1 ? OUTPUT : INPUT);
    enviarOk();

  } else {
    enviarError("comando desconocido: " + cmd);
  }
}

// ── Helpers de servo ─────────────────────────────────────
int obtenerIndiceServo(int pin) {
  for (int i = 0; i < 8; i++)
    if (servoPines[i] == pin) return i;
  return -1;
}

int agregarServo(int pin) {
  for (int i = 0; i < 8; i++) {
    if (servoPines[i] == -1) {
      servos[i].attach(pin);
      servoPines[i] = pin;
      return i;
    }
  }
  return -1;
}

// ── Helpers de respuesta ──────────────────────────────────
void enviarOk() {
  Serial.println("{\"ok\":true}");
}

void enviarError(String msg) {
  Serial.println("{\"ok\":false,\"error\":\"" + msg + "\"}");
}

// ── Parser JSON simple ────────────────────────────────────
String extraerValor(String json, String clave) {
  String buscar = "\"" + clave + "\":\"";
  int idx = json.indexOf(buscar);
  if (idx == -1) return "";
  idx += buscar.length();
  int fin = json.indexOf("\"", idx);
  if (fin == -1) return "";
  return json.substring(idx, fin);
}

int extraerNumero(String json, String clave) {
  String buscar = "\"" + clave + "\":";
  int idx = json.indexOf(buscar);
  if (idx == -1) return -1;
  idx += buscar.length();
  // Saltar espacios
  while (idx < json.length() && json[idx] == ' ') idx++;
  // Leer dígitos
  String num = "";
  while (idx < json.length() && (isDigit(json[idx]) || json[idx] == '-')) {
    num += json[idx++];
  }
  return num.length() > 0 ? num.toInt() : -1;
}
