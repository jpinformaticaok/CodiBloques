/*
 * CodiBloques - Parches al bundle gui.js para corregir:
 * 1. addSprite: inyectar projectVersion en el sprite de la biblioteca
 * 2. sound preview: pasar dataFormat (extensión) al storage.load
 *
 * Uso: node tools/patch-gui-bundle.cjs
 */
const fs = require('fs');
const path = require('path');

const GUI_JS = path.join(__dirname, '..', 'gui', 'gui.js');

let src = fs.readFileSync(GUI_JS, 'utf8');

let changed = 0;

// ===== FIX 1: SpriteLibrary.handleItemSelect - agregar projectVersion antes de addSprite =====
// Buscar: handleItemSelect(item) { ... this.props.vm.addSprite(JSON.stringify(item)).then(...
// Inyectar: item.projectVersion = 3; antes del stringify
const fix1Pattern = /handleItemSelect\(item\)\s*\{[^}]*?this\.props\.vm\.addSprite\(JSON\.stringify\(item\)\)/;
const fix1Match = src.match(fix1Pattern);
if (fix1Match) {
  const idx = src.indexOf(fix1Match[0]);
  const replacement = fix1Match[0].replace(
    'this.props.vm.addSprite(JSON.stringify(item))',
    'item.projectVersion = 3; this.props.vm.addSprite(JSON.stringify(item))'
  );
  src = src.slice(0, idx) + replacement + src.slice(idx + fix1Match[0].length);
  console.log('FIX 1 aplicado: projectVersion en SpriteLibrary');
  changed++;
} else {
  console.log('FIX 1: patrón no encontrado (quizá ya parcheado)');
}

// ===== FIX 2: SoundLibrary.handleItemMouseEnter - pasar dataFormat a storage.load =====
// Buscar: vm.runtime.storage.load(vm.runtime.storage.AssetType.Sound, md5)
// Reemplazar con: vm.runtime.storage.load(vm.runtime.storage.AssetType.Sound, md5, soundItem.dataFormat || 'wav')
const fix2Pattern = /vm\.runtime\.storage\.load\(vm\.runtime\.storage\.AssetType\.Sound,\s*md5\)/g;
const fix2Matches = src.match(fix2Pattern);
if (fix2Matches && fix2Matches.length > 0) {
  src = src.replace(fix2Pattern, "vm.runtime.storage.load(vm.runtime.storage.AssetType.Sound, md5, soundItem.dataFormat || 'wav')");
  console.log(`FIX 2 aplicado: dataFormat en ${fix2Matches.length} ocurrencias`);
  changed++;
} else {
  console.log('FIX 2: patrón no encontrado (quizá ya parcheado)');
}

// ===== FIX 3: SoundLibrary.handleItemMouseEnter - soundItem._md5 incluye extensión, extraer md5 y dataFormat =====
// El código actual: const md5ext = soundItem._md5; const idParts = md5ext.split('.'); const md5 = idParts[0];
// Ya separa la extensión. El dataFormat está en soundItem.format (vacío) o inferido de la extensión.
// El storage.load espera dataFormat como 'wav', 'mp3', etc.
// Usar la extensión del _md5.

if (changed > 0) {
  fs.writeFileSync(GUI_JS, src);
  console.log(`\nBundle parcheado (${changed} fixes).`);
} else {
  console.log('\nSin cambios (bundle ya parcheado o patrones cambiaron).');
}