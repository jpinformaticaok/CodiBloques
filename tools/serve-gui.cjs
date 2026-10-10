/*
 * CodiBloques - Servidor estatico de prueba para la GUI offline.
 * Uso: node tools/serve-gui.cjs [puerto]
 * Solo para desarrollo/pruebas. La app real usa desktop/main.cjs.
 */
const http = require('node:http');
const path = require('node:path');
const fs = require('node:fs');

const ROOT = path.join(__dirname, '..', 'gui');
const PORT = Number(process.argv[2] || 8605);
const MIME = {
  '.html': 'text/html; charset=utf-8',
  '.js': 'text/javascript; charset=utf-8',
  '.css': 'text/css; charset=utf-8',
  '.json': 'application/json; charset=utf-8',
  '.png': 'image/png',
  '.jpg': 'image/jpeg',
  '.svg': 'image/svg+xml',
  '.wav': 'audio/wav',
  '.mp3': 'audio/mpeg',
  '.ogg': 'audio/ogg',
};

http
  .createServer((req, res) => {
    let url;
    try {
      url = decodeURIComponent(new URL(req.url, 'http://127.0.0.1').pathname);
    } catch {
      res.writeHead(400).end();
      return;
    }
    if (url.endsWith('/')) url += 'index.html';
    const filePath = path.resolve(ROOT, '.' + url);
    if (!filePath.startsWith(ROOT) || !fs.existsSync(filePath)) {
      res.writeHead(404).end('Not found: ' + url);
      return;
    }
    fs.readFile(filePath, (err, data) => {
      if (err) {
        res.writeHead(404).end();
        return;
      }
      res.writeHead(200, { 'Content-Type': MIME[path.extname(filePath).toLowerCase()] || 'application/octet-stream' });
      res.end(data);
    });
  })
  .listen(PORT, '127.0.0.1', () => console.log(`GUI de prueba en http://127.0.0.1:${PORT}/`));
