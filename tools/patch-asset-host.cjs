/*
 * CodiBloques - Parche offline del host de assets.
 * Desarrollador: Juan Pablo Aguero (https://github.com/jpinformaticaok)
 *
 * Reescribe en los bundles compilados de la GUI (gui.js, player.js,
 * blocksonly.js, compatibilitytesting.js) la URL de descarga de assets de
 * biblioteca desde la CDN de Scratch a una ruta RELATIVA local:
 *   washing:  https://assets.scratch.mit.edu/internalapi/asset/<id>.<ext>/get/
 *   nueva:    static/local-assets/<id>.<ext>
 * La ruta relativa se resuelve contra http://127.0.0.1:<puerto>/ que sirve
 * el wrapper Electron, por lo que funciona con puerto efimero y sin internet.
 * Requiere haber corrido antes tools/mirror-assets.cjs.
 *
 * Uso: node tools/patch-asset-host.cjs [--check]
 *   --check solo informa si los bundles ya estan parcheados (exit 1 si no).
 */
const fs = require('node:fs');
const path = require('node:path');

const GUI_DIR = path.join(__dirname, '..', 'gui');
const BUNDLES = ['gui.js', 'player.js', 'blocksonly.js', 'compatibilitytesting.js'];

const REPLACEMENTS = [
  {
    name: 'asset-get',
    // return "".concat(this.assetHost, "/internalapi/asset/").concat(asset.assetId, ".").concat(asset.dataFormat, "/get/");
    pattern: /""\.concat\(this\.assetHost, "\/internalapi\/asset\/"\)\.concat\(asset\.assetId, "[^"]*"\)\.concat\(asset\.dataFormat, "[^"]*"\)/g,
    replacement: '"static/local-assets/".concat(asset.assetId, ".").concat(asset.dataFormat)',
  },
  {
    name: 'asset-get-broken',
    // Repara aplicacion previa con "" residual: return """static/local-assets/"...
    pattern: /"""static\/local-assets\/"\.concat\(asset\.assetId, "[^"]*"\)\.concat\(asset\.dataFormat\)/g,
    replacement: '"static/local-assets/".concat(asset.assetId, ".").concat(asset.dataFormat)',
  },
  {
    name: 'extension-icon',
    // "https://cdn.assets.scratch.mit.edu/internalapi/asset/".concat(iconMd5, "/get/")
    pattern: /"https:\/\/cdn\.assets\.scratch\.mit\.edu\/internalapi\/asset\/"\.concat\(iconMd5, "\/get\/"\)/g,
    replacement: '"static/local-assets/".concat(iconMd5)',
  },
];

const checkOnly = process.argv.includes('--check');
let dirty = false;

for (const bundle of BUNDLES) {
  const file = path.join(GUI_DIR, bundle);
  if (!fs.existsSync(file)) {
    console.log(`(omitido, no existe) ${bundle}`);
    continue;
  }
  let src = fs.readFileSync(file, 'utf8');
  let changed = false;
  for (const r of REPLACEMENTS) {
    const count = (src.match(r.pattern) || []).length;
    if (count > 0) {
      if (!checkOnly) src = src.replace(r.pattern, r.replacement);
      console.log(`${bundle}: ${r.name} x${count}${checkOnly ? ' (pendiente)' : ' parcheado'}`);
      changed = true;
    }
  }
  if (src.includes('static/local-assets/')) {
    console.log(`${bundle}: ya contiene ruta local`);
  }
  if (changed) {
    dirty = true;
    if (!checkOnly) fs.writeFileSync(file, src);
  }
}

// Aviso si el espejo local esta incompleto
const mirror = path.join(GUI_DIR, 'static', 'local-assets');
if (fs.existsSync(mirror)) {
  const n = fs.readdirSync(mirror).length;
  console.log(`espejo local: ${n} archivos en static/local-assets/`);
  if (n < 1000) console.log('AVISO: espejo incompleto, corre node tools/mirror-assets.cjs');
} else {
  console.log('AVISO: no existe static/local-assets/, corre node tools/mirror-assets.cjs');
}

if (checkOnly && dirty) {
  console.log('RESULTADO: hay bundles sin parchear');
  process.exit(1);
}
if (!checkOnly) console.log(dirty ? 'PARCHE APLICADO' : 'Sin cambios (ya parcheado)');
