# CodiBloques / Scratch 3.0 (portable, offline)

Aplicacion de escritorio Windows portable y **totalmente offline** para crear,
editar y ejecutar proyectos Scratch 3.0 (`.sb3`) sin conexion a internet.
Pensada para alumnos sin conectividad: incluye las bibliotecas de imagenes y
sonidos empaquetadas.

## Desarrollador de esta version

**Juan Pablo Aguero** — https://github.com/jpinformaticaok

Basado en Scratch GUI de Scratch Foundation (licencia AGPL-3.0).
Scratch es marca de Scratch Foundation. Esta version independiente se
distribuye como **CodiBloques**, sin usar marcas oficiales en el producto.

## Uso

- `npm install` — instala dependencias (requiere internet una sola vez).
- `npm start` — ejecuta en modo desarrollo (necesita carpeta `gui/`).
- `npm run dist` — genera `dist/CodiBloques-Portable-1.0.0.exe` (portable, sin instalacion ni admin).
- `npm run dist:dir` — build sin empaquetar para pruebas.

## Offline

- La GUI vive en `gui/` (build de scratch-gui) y se sirve por HTTP solo en
  `127.0.0.1` con puerto efimero. Nada sale a internet.
- Guardar/cargar proyectos es local (archivos `.sb3`).
- Idioma predeterminado: espanol (`?locale=es` en primera ejecucion). Luego se
  respeta el idioma que elija el usuario.

## Plugins (ej. Arduino)

La app expone la ruta local `/plugins/` (carpeta `extensions/`) y el objeto
`window.CodiBloques = { app, version, author, github }` para que futuras
extensiones (placas Arduino por USB-serial con Firmata) se carguen offline.
Ver `extensions/arduino/README.md`.

## Diagnostico

- Log de la aplicacion: `%TEMP%\codibloques-exec.log` (fecha/hora, puerto,
  pagina cargada y cada cierre, con el motivo: `cambios sin guardar = true/false`,
  `cierre confirmado`, `usuario cancelo la salida`).
- Si la app no cierra: se intercepta el evento `close` antes del
  `beforeunload` de Scratch y se pregunta en espanol ("Salir sin guardar" /
  "Seguir trabajando"). Confirmar cierra siempre; hay una red de seguridad
  (`will-prevent-unload`) para que nunca quede imposible de salir.
- Si `npm run dist` falla con `Cannot create symbolic link ... winCodeSign`:
  es la extraccion del paquete de firma, que requiere privilegios de symlinks
  en Windows. Solucion: extraerlo una vez a mano en
  `%LOCALAPPDATA%\electron-builder\Cache\winCodeSign\winCodeSign-2.6.0`
  (`7za x -bd <descarga>.7z -o<destino>`; solo fallan 2 symlinks de macOS,
  que no hacen falta en Windows) y volver a correr `npm run dist`.
  La firma se omite igualmente: `no signing info identified, signing is skipped`.

## Estructura

```
CodiBloques/
  desktop/main.cjs      Wrapper Electron (ventana + servidor local 127.0.0.1)
  gui/                  Build offline de scratch-gui (no se versiona completo)
  extensions/arduino/   Punto de extension para placas Arduino
  electron-builder.yml  Config portable x64
  dist/                 Salida del .exe portable
```
