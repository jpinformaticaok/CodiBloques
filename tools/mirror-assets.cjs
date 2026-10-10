/*
 * CodiBloques - Espejo offline de assets de la biblioteca Scratch.
 * Desarrollador: Juan Pablo Aguero (https://github.com/jpinformaticaok)
 *
 * La GUI de Scratch carga disfraces/fondos/sonidos de la biblioteca desde
 * la CDN (https://assets.scratch.mit.edu/...). Este script descarga UNA vez
 * (con internet) todos los archivos referenciados por las bibliotecas
 * embebidas en gui/gui.js y los guarda en gui/static/local-assets/.
 * Luego tools/patch-asset-host.cjs reescribe la URL a relativa para que la
 * app portable funcione 100% offline.
 *
 * Uso: node tools/mirror-assets.cjs
 */
const fs = require('node:fs');
const path = require('node:path');

const GUI_JS = path.join(__dirname, '..', 'gui', 'gui.js');
const OUT_DIR = path.join(__dirname, '..', 'gui', 'static', 'local-assets');
const ASSET_BASE = 'https://assets.scratch.mit.edu/internalapi/asset/';
const ICON_BASE = 'https://cdn.assets.scratch.mit.edu/internalapi/asset/';
const CONCURRENCY = 8;

async function download(url, dest) {
  for (let attempt = 1; attempt <= 3; attempt++) {
    try {
      const res = await fetch(url);
      if (!res.ok) throw new Error(`HTTP ${res.status}`);
      const buf = Buffer.from(await res.arrayBuffer());
      if (buf.length === 0) throw new Error('vacio');
      fs.writeFileSync(dest, buf);
      return buf.length;
    } catch (err) {
      if (attempt === 3) throw err;
      await new Promise((r) => setTimeout(r, 1000 * attempt));
    }
  }
}

async function main() {
  const src = fs.readFileSync(GUI_JS, 'utf8');

  // md5ext de bibliotecas: "md5ext":"<32hex>.<ext>"
  const libFiles = new Map(); // filename -> url
  const md5re = /"md5ext":"([0-9a-f]{32}\.[a-zA-Z0-9]+)"/g;
  let m;
  while ((m = md5re.exec(src)) !== null) {
    libFiles.set(m[1], `${ASSET_BASE}${m[1]}/get/`);
  }
  // Iconos de extensiones: "iconMd5":"<...>"
  const iconre = /"iconMd5":"([^"]+)"/g;
  while ((m = iconre.exec(src)) !== null) {
    if (!libFiles.has(m[1])) libFiles.set(m[1], `${ICON_BASE}${m[1]}/get/`);
  }

  fs.mkdirSync(OUT_DIR, { recursive: true });
  const entries = [...libFiles.entries()];
  console.log(`archivos unicos a espejar: ${entries.length}`);

  let ok = 0, skipped = 0, failed = 0, bytes = 0;
  const errors = [];
  let i = 0;
  async function worker() {
    while (i < entries.length) {
      const [name, url] = entries[i++];
      const dest = path.join(OUT_DIR, name);
      if (fs.existsSync(dest) && fs.statSync(dest).size > 0) {
        skipped++;
        bytes += fs.statSync(dest).size;
        continue;
      }
      try {
        const n = await download(url, dest);
        bytes += n;
        ok++;
      } catch (err) {
        failed++;
        errors.push(`${name} <- ${url} :: ${err.message}`);
      }
      if ((ok + skipped + failed) % 100 === 0) {
        console.log(`  progreso ${ok + skipped + failed}/${entries.length}`);
      }
    }
  }
  await Promise.all(Array.from({ length: CONCURRENCY }, worker));

  console.log(`OK nuevos: ${ok}, ya existian: ${skipped}, fallidos: ${failed}`);
  console.log(`total espejo: ${(bytes / 1048576).toFixed(1)} MB en ${OUT_DIR}`);
  if (errors.length) {
    console.log('FALLOS:');
    errors.slice(0, 30).forEach((e) => console.log('  ' + e));
  }
  if (failed > 0) process.exitCode = 1;
}

main();
