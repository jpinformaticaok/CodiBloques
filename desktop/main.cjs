/*
 * CodiBloques / Scratch 3.0 - Ventana portable offline
 * Desarrollador de esta version: Juan Pablo Aguero
 * GitHub: https://github.com/jpinformaticaok
 *
 * Basado en Scratch GUI de Scratch Foundation (AGPL-3.0).
 * Este wrapper sirve el build local de scratch-gui por HTTP en 127.0.0.1
 * (puerto efimero) para que la app sea 100% offline: crear, editar y
 * ejecutar proyectos .sb3 sin internet. Las bibliotecas de imagenes y
 * sonidos viajan empaquetadas en resources/scratch-gui.
 */
const http = require('node:http');
const path = require('node:path');
const fs = require('node:fs');
const os = require('node:os');
const { app, BrowserWindow, shell, dialog, Menu, session } = require('electron');

const APP_NAME = 'CodiBloques';
const APP_VERSION = app.isPackaged
  ? app.getVersion()
  : (require('../package.json').version || 'dev');
const AUTHOR = 'Juan Pablo Aguero';
const GITHUB_URL = 'https://github.com/jpinformaticaok';

const LOG_FILE = path.join(os.tmpdir(), 'codibloques-exec.log');
function log(msg) {
  try { fs.appendFileSync(LOG_FILE, `[${new Date().toISOString()}] ${msg}\n`); } catch {}
}
process.on('uncaughtException', (err) => {
  log('uncaughtException: ' + (err && err.stack ? err.stack : err));
  try { dialog.showErrorBox(`${APP_NAME}: error`, String((err && err.stack) || err)); } catch {}
});
process.on('unhandledRejection', (err) => {
  log('unhandledRejection: ' + (err && err.stack ? err.stack : err));
});

// Marca de primera ejecucion: solo la primera vez forzamos ?locale=es.
// Despues se respeta el idioma que elija el usuario (queda en localStorage).
function isFirstRun() {
  const flag = path.join(app.getPath('userData'), '.codibloques-instalado');
  if (fs.existsSync(flag)) return false;
  try {
    fs.mkdirSync(app.getPath('userData'), { recursive: true });
    fs.writeFileSync(flag, new Date().toISOString());
  } catch {}
  return true;
}

function resolveGuiDir() {
  if (app.isPackaged) {
    // electron-builder extraResources: gui -> resources/scratch-gui
    const candidates = [
      path.join(process.resourcesPath, 'scratch-gui'),
      path.join(process.resourcesPath, 'app.asar.unpacked'),
    ];
    for (const c of candidates) {
      if (fs.existsSync(path.join(c, 'index.html'))) return c;
    }
    return candidates[0];
  }
  return path.join(__dirname, '..', 'gui');
}

function resolveExtensionsDir() {
  if (app.isPackaged) return path.join(process.resourcesPath, 'extensions');
  return path.join(__dirname, '..', 'extensions');
}

const MIME = {
  '.html': 'text/html; charset=utf-8',
  '.js': 'text/javascript; charset=utf-8',
  '.mjs': 'text/javascript; charset=utf-8',
  '.css': 'text/css; charset=utf-8',
  '.json': 'application/json; charset=utf-8',
  '.png': 'image/png',
  '.jpg': 'image/jpeg',
  '.jpeg': 'image/jpeg',
  '.gif': 'image/gif',
  '.ico': 'image/x-icon',
  '.svg': 'image/svg+xml',
  '.wav': 'audio/wav',
  '.mp3': 'audio/mpeg',
  '.ogg': 'audio/ogg',
  '.hex': 'application/octet-stream',
};

const GUI_DIR = resolveGuiDir();
const EXT_DIR = resolveExtensionsDir();

function serveRoot(req, res, root) {
  let url;
  try {
    url = decodeURIComponent(new URL(req.url, 'http://127.0.0.1').pathname);
  } catch {
    res.writeHead(400).end();
    return true;
  }
  if (url.endsWith('/')) url += 'index.html';
  const filePath = path.resolve(root, '.' + url);
  if (!filePath.startsWith(root)) {
    res.writeHead(403).end();
    return true;
  }
  if (!fs.existsSync(filePath) || !fs.statSync(filePath).isFile()) return false;
  fs.readFile(filePath, (err, data) => {
    if (err) {
      res.writeHead(404).end('Not found');
      return;
    }
    const ext = path.extname(filePath).toLowerCase();
    res.writeHead(200, {
      'Content-Type': MIME[ext] || 'application/octet-stream',
      'Cache-Control': 'no-store',
    });
    res.end(data);
  });
  return true;
}

const server = http.createServer((req, res) => {
  const pathname = (() => {
    try { return new URL(req.url, 'http://127.0.0.1').pathname; } catch { return req.url; }
  })();
  // /plugins/* -> carpeta de plugins (ej. Arduino). Punto de extension offline.
  if (pathname === '/plugins' || pathname.startsWith('/plugins/')) {
    const rel = pathname.replace(/^\/plugins/, '') || '/index.json';
    const fakeReq = { url: rel };
    if (serveRoot(fakeReq, res, EXT_DIR)) return;
    res.writeHead(404, { 'Content-Type': 'application/json; charset=utf-8' });
    res.end(JSON.stringify({ app: APP_NAME, plugins: [] }));
    return;
  }
  if (serveRoot(req, res, GUI_DIR)) return;
  res.writeHead(404).end('Not found');
});

function showAbout() {
  dialog.showMessageBoxSync({
    type: 'info',
    title: `Acerca de ${APP_NAME}`,
    message: `${APP_NAME} / Scratch 3.0 (portable, offline) - v${APP_VERSION}`,
    detail:
      `Editor de bloques Scratch 3.0 que funciona sin internet.\n\n` +
      `Desarrollador de esta version: ${AUTHOR}\nGitHub: ${GITHUB_URL}\n\n` +
      `Basado en Scratch GUI de Scratch Foundation (AGPL-3.0).\n` +
      `Scratch es marca de Scratch Foundation.`,
  });
}

function buildMenu() {
  const template = [
    {
      label: APP_NAME,
      submenu: [
        { label: `Acerca de ${APP_NAME}`, click: showAbout },
        { type: 'separator' },
        { label: 'Recargar', accelerator: 'CmdOrCtrl+R', role: 'reload' },
        { label: 'Salir', accelerator: 'Alt+F4', role: 'quit' },
      ],
    },
    {
      label: 'Ayuda',
      submenu: [
        {
          label: 'GitHub del desarrollador',
          click: () => shell.openExternal(GITHUB_URL),
        },
      ],
    },
  ];
  Menu.setApplicationMenu(Menu.buildFromTemplate(template));
}

/**
 * Scratch instala window.onbeforeunload = leavePageConfirm, que devuelve true
 * cuando hay cambios sin guardar. Electron respeta ese bloqueo y CANCELA el
 * cierre sin mostrar ninguna ventana: por eso la app "no se cierra" y hay que
 * matar el proceso a mano.
 *
 * Se intercepta el evento "close" (que siempre llega), se consulta al renderer
 * si hay cambios sin guardar y recien ahi se decide:
 *   - sin cambios  -> se cierra directo.
 *   - con cambios  -> aviso en espanol; si el usuario confirma, se sale con
 *     win.destroy(), que no ejecuta beforeunload y por lo tanto no se traba.
 */
function interceptClose(win) {
  let forceClosing = false;
  let asking = false;

  // Red de seguridad: si Electron aun asi llega al bloqueo de beforeunload,
  // se lo ignora para que la app nunca quede imposible de salir.
  win.on('will-prevent-unload', (event) => {
    log('will-prevent-unload: se ignora el bloqueo de Scratch');
    event.preventDefault();
  });

  win.on('close', (e) => {
    if (forceClosing) return; // ya decidido: dejar cerrar
    e.preventDefault(); // corta ANTES del beforeunload de Scratch
    if (asking) return; // ya hay un aviso abierto en pantalla

    // Consulta al renderer: leavePageConfirm devuelve true si hay cambios.
    const PROBE =
      `(() => { try { const ev = {}; ` +
      `const r = typeof window.onbeforeunload === 'function' ? window.onbeforeunload(ev) : false; ` +
      `return r === true || ev.returnValue === true; } catch (err) { return false; } })()`;

    const doClose = () => {
      forceClosing = true;
      log('cierre confirmado');
      win.destroy(); // no ejecuta beforeunload: sale siempre
    };

    asking = true;
    win.webContents
      .executeJavaScript(PROBE, true)
      .catch(() => false)
      .then((unsaved) => {
        asking = false;
        log('close: cambios sin guardar = ' + unsaved);
        if (!unsaved) {
          doClose();
          return null;
        }
        return dialog
          .showMessageBox(win, {
            type: 'warning',
            title: APP_NAME,
            message: 'Hay cambios sin guardar.',
            detail:
              'Si salis ahora, se pierden los cambios que no guardaste en un archivo .sb3.\n\n' +
              'Para conservarlos usa Archivo > Guardar en tu computadora.',
            buttons: ['Salir sin guardar', 'Seguir trabajando'],
            defaultId: 1,
            cancelId: 1,
            noLink: true,
          })
          .then((res) => {
            if (res.response === 0) doClose();
            else log('close: usuario cancelo la salida');
          });
      })
      .catch((err) => {
        // Red de seguridad: ante cualquier error la app DEBE poder salir.
        log('close: error consultando cambios, se fuerza salida: ' + err);
        asking = false;
        doClose();
      });
  });
}

function interceptSaves(win) {
  // "Guardar en tu compu" descarga un .sb3 via blob. Scratch nunca limpia
  // projectChanged al hacerlo, por lo que leavePageConfirm seguia devolviendo
  // true y la app seguia preguntando "¿salir sin guardar?" aunque acabaras de
  // guardar. Se escucha la descarga y, SOLO si completa (no si se cancela el
  // dialogo), se limpia el flag en el store de Redux del renderer.
  session.defaultSession.on('will-download', (_event, item) => {
    // El renderer levanto el flag justo antes de disparar la descarga.
    win.webContents
      .executeJavaScript('window.__codibloquesPendingSaveToComputer === true', true)
      .catch(() => false)
      .then((pending) => {
        if (!pending) return; // no es un "Guardar en tu compu"
        item.once('done', (_ev, state) => {
          if (state === 'completed') {
            log('save-to-computer: descarga completada, projectChanged = false');
            win.webContents
              .executeJavaScript(
                `(() => { try { ` +
                  `if (!window.__codibloquesStore) return 'no-store'; ` +
                  `window.__codibloquesStore.dispatch({ type: 'scratch-gui/project-changed/SET_PROJECT_CHANGED', changed: false }); ` +
                  `window.__codibloquesPendingSaveToComputer = false; ` +
                  `return 'ok'; ` +
                  `} catch (err) { return 'err:' + err; } })()`,
                true
              )
              .then((r) => log('save-to-computer: dispatch result = ' + r))
              .catch((err) => log('save-to-computer: error dispatch = ' + err));
          } else {
            // Cancelada o fallida: el proyecto sigue "modificado".
            log('save-to-computer: descarga no completada (state=' + state + ')');
            win.webContents
              .executeJavaScript('window.__codibloquesPendingSaveToComputer = false', true)
              .catch(() => {});
          }
        });
      });
  });
}

function createWindow(port, forceEs) {
  const win = new BrowserWindow({
    width: 1280,
    height: 800,
    minWidth: 900,
    minHeight: 650,
    autoHideMenuBar: true,
    backgroundColor: '#4c97ff',
    title: `${APP_NAME} / Scratch 3.0`,
    webPreferences: {
      sandbox: true,
      contextIsolation: true,
      nodeIntegration: false,
    },
  });
  win.removeMenu();
  // La app arranca maximizada (request del usuario).
  win.maximize();
  interceptClose(win);
  interceptSaves(win);
  win.webContents.setWindowOpenHandler(({ url }) => {
    if (url.startsWith('http://') || url.startsWith('https://')) shell.openExternal(url);
    return { action: 'deny' };
  });
  win.webContents.on('did-fail-load', (_e, code, desc, url) => {
    log(`did-fail-load code=${code} desc=${desc} url=${url}`);
  });
  // Expone identidad de la app al renderer (para futuros plugins Arduino).
  win.webContents.on('did-finish-load', () => {
    win.webContents
      .executeJavaScript(
        `window.CodiBloques = Object.assign(window.CodiBloques || {}, ` +
          JSON.stringify({ app: APP_NAME, version: APP_VERSION, author: AUTHOR, github: GITHUB_URL }) +
          `);`
      )
      .catch((e) => log('bridge error: ' + e));
  });
  const base = `http://127.0.0.1:${port}/index.html`;
  win.loadURL(forceEs ? `${base}?locale=es` : base)
    .then(() => log('index.html cargado OK'))
    .catch((err) => log('loadURL error: ' + ((err && err.stack) || err)));
  return win;
}

app.whenReady().then(() => {
  buildMenu();
  const forceEs = isFirstRun(); // espanol argentino/latino por defecto
  log(`${APP_NAME} v${APP_VERSION} por ${AUTHOR} (${GITHUB_URL}) gui=${GUI_DIR}`);
  if (!fs.existsSync(path.join(GUI_DIR, 'index.html'))) {
    dialog.showErrorBox(
      `${APP_NAME}: falta el contenido`,
      `No se encontro GUI offline en:\n${GUI_DIR}\n\nReinstala la app portable.`
    );
    app.quit();
    return;
  }
  server.listen(0, '127.0.0.1', () => {
    const port = server.address().port;
    log(`server escuchando en 127.0.0.1:${port}`);
    createWindow(port, forceEs);
  });
});

app.on('window-all-closed', () => {
  server.close();
  app.quit();
});
