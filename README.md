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

## Estructura

```
CodiBloques/
  desktop/main.cjs      Wrapper Electron (ventana + servidor local 127.0.0.1)
  gui/                  Build offline de scratch-gui (no se versiona completo)
  extensions/arduino/   Punto de extension para placas Arduino
  electron-builder.yml  Config portable x64
  dist/                 Salida del .exe portable
```
