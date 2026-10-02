# Plugin Arduino (offline) — plan

Objetivo: conectar placas Arduino (Uno/Nano/Mega) por USB-serial sin internet.

## Diseno previsto

1. Firmware en la placa: StandardFirmata (o firmware propio documentado).
2. Puente serie en Electron: `node-serialport` en el proceso main, expuesto a
   la GUI solo por la ruta local `/plugins/arduino/*` (nunca internet).
3. Bloques: digitalWrite/digitalRead, analogRead, PWM, servo, ultrasonico.
4. Drivers USB (CH340/CP210x) se instalan una sola vez aparte (requiere admin
   esa unica vez); el uso diario del portable no necesita admin.

## Estado actual

Carpeta reservada. El wrapper ya sirve `/plugins/` y expone
`window.CodiBloques` para enganchar la extension cuando se implemente.

## Manifiesto de ejemplo

Ver `index.json` de esta carpeta.
