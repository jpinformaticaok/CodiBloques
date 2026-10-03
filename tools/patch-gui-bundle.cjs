/*
 * CodiBloques - Parches a los bundles de la GUI (gui.js, player.js, ...)
 *
 * FIX 1: SpriteLibrary.handleItemSelect - inyectar projectVersion = 3 antes de addSprite
 *         (scratch-parser rechaza sprites de la biblioteca sin projectVersion)
 * FIX 2: SoundLibrary - pasar dataFormat (extensión) al storage.load de la preview
 * FIX 3: Deshabilitar FetchWorkerTool porque el chunk chunks/fetch-worker.<hash>.js
 *         NO existe en el build. new Worker() no lanza error síncrono, por lo que
 *         isGetSupported devolvía true y la petición quedaba colgada para siempre
 *         (ni sprites, ni sonidos, ni fondos se agregaban al escenario).
 *         Se fuerza false para que ProxyTool use FetchTool (fetch normal) con las
 *         URLs locales static/local-assets/.
 *
 * Uso: node tools/patch-gui-bundle.cjs
 * El script es idempotente: puede ejecutarse varias veces sin duplicar parches.
 */
const fs = require('fs');
const path = require('path');

const GUI_DIR = path.join(__dirname, '..', 'gui');
const BUNDLES = ['gui.js', 'player.js', 'blocksonly.js', 'compatibilitytesting.js'];

let grandTotal = 0;

for (const bundleName of BUNDLES) {
  const bundlePath = path.join(GUI_DIR, bundleName);
  if (!fs.existsSync(bundlePath)) {
    console.log(`${bundleName}: no existe, omitido`);
    continue;
  }

  let src = fs.readFileSync(bundlePath, 'utf8');
  const original = src;
  let changed = 0;

  // ===== FIX 1: projectVersion antes de addSprite =====
  if (src.includes('item.projectVersion = 3; this.props.vm.addSprite')) {
    console.log(`${bundleName} - FIX 1: ya parcheado`);
  } else {
    // Contexto único: randomize_sprite_position + addSprite en SpriteLibrary.handleItemSelect
    const fix1From = '(0,_lib_randomize_sprite_position__WEBPACK_IMPORTED_MODULE_6__["default"])(item);\n    this.props.vm.addSprite(JSON.stringify(item))';
    const fix1To = '(0,_lib_randomize_sprite_position__WEBPACK_IMPORTED_MODULE_6__["default"])(item);\n    item.projectVersion = 3;\n    this.props.vm.addSprite(JSON.stringify(item))';
    if (src.includes(fix1From)) {
      src = src.split(fix1From).join(fix1To);
      console.log(`${bundleName} - FIX 1: projectVersion inyectado`);
      changed++;
    } else {
      console.log(`${bundleName} - FIX 1: patrón no encontrado (no aplica o ya parcheado)`);
    }
  }

  // ===== FIX 2: dataFormat en la preview de sonidos =====
  const fix2Pattern = /vm\.runtime\.storage\.load\(vm\.runtime\.storage\.AssetType\.Sound,\s*md5\)/g;
  const fix2Matches = src.match(fix2Pattern);
  if (fix2Matches) {
    src = src.replace(
      fix2Pattern,
      "vm.runtime.storage.load(vm.runtime.storage.AssetType.Sound, md5, soundItem.dataFormat || 'wav')"
    );
    console.log(`${bundleName} - FIX 2: dataFormat en ${fix2Matches.length} ocurrencia(s)`);
    changed++;
  } else if (src.includes("soundItem.dataFormat || 'wav'")) {
    console.log(`${bundleName} - FIX 2: ya parcheado`);
  } else {
    console.log(`${bundleName} - FIX 2: patrón no encontrado (no aplica)`);
  }

  // ===== FIX 3: deshabilitar FetchWorkerTool (chunk ausente) =====
  const fix3Pattern = /return typeof Worker !== 'undefined' && this\._workerSupport\.fetch && !this\._supportError;/g;
  const fix3Matches = src.match(fix3Pattern);
  if (fix3Matches) {
    src = src.replace(
      fix3Pattern,
      'return false; /* CodiBloques: chunk fetch-worker ausente, forzar FetchTool */'
    );
    console.log(`${bundleName} - FIX 3: FetchWorkerTool deshabilitado (${fix3Matches.length} ocurrencia(s))`);
    changed++;
  } else if (src.includes('CodiBloques: chunk fetch-worker ausente')) {
    console.log(`${bundleName} - FIX 3: ya parcheado`);
  } else {
    console.log(`${bundleName} - FIX 3: patrón no encontrado (no aplica)`);
  }

  // ===== FIX 4: Traducción al español rioplatense del diálogo de borrado =====
  // Scratch trae estos textos en inglés incluso en los bloques "es" y "es-419".
  // Se reemplazan SOLO dentro de esos dos bloques de locale para no tocar el resto.
  const confirmMessages = [
    ['gui.gui.shouldDeleteSprite', '¿Seguro que querés borrar este objeto?'],
    ['gui.gui.shouldDeleteCostume', '¿Seguro que querés borrar este disfraz?'],
    ['gui.gui.shouldDeleteSound', '¿Seguro que querés borrar este sonido?'],
    ['gui.gui.deleteAssetHeading', 'Confirmar borrado'],
    ['gui.gui.confirm', 'sí'],
    ['gui.gui.cancel', 'no']
  ];

  // Encuentra los límites de un bloque de locale: '\n  "es": { ... \n  "otro": {'
  // Las claves internas del bloque sangran 4 espacios, por lo que '\n  "' (2 espacios)
  // identifica únicamente el inicio de un bloque de locale de nivel superior.
  const localeBlockRange = (text, locale) => {
    const start = text.indexOf('\n  "' + locale + '": {');
    if (start < 0) return null;
    const end = text.indexOf('\n  "', start + 5);
    return end < 0 ? { start, end: text.length } : { start, end };
  };

  let confirmChanged = 0;
  for (const locale of ['es', 'es-419']) {
    const range = localeBlockRange(src, locale);
    if (!range) {
      console.log(`${bundleName} - FIX 4: locale ${locale} no encontrado`);
      continue;
    }
    let seg = src.slice(range.start, range.end);
    let segChanged = 0;
    for (const [key, value] of confirmMessages) {
      const re = new RegExp('("' + key.replace(/\./g, '\\.') + '":\\s*")[^"]*(")');
      if (re.test(seg)) {
        seg = seg.replace(re, '$1' + value + '$2');
        segChanged++;
      }
    }
    if (segChanged > 0) {
      src = src.slice(0, range.start) + seg + src.slice(range.end);
      console.log(`${bundleName} - FIX 4: ${locale} traducido (${segChanged} texto/s)`);
      confirmChanged += segChanged;
    } else {
      console.log(`${bundleName} - FIX 4: ${locale} ya estaba traducido`);
    }
  }
  if (confirmChanged > 0) changed++;

  if (changed > 0) {
    fs.writeFileSync(bundlePath, src);
    console.log(`${bundleName}: ${changed} parche(s) aplicado(s)\n`);
    grandTotal += changed;
  } else {
    console.log(`${bundleName}: sin cambios\n`);
  }
}

console.log(
  grandTotal > 0
    ? `Listo: ${grandTotal} parche(s) aplicados.`
    : 'Sin cambios: los bundles ya estaban parcheados.'
);
