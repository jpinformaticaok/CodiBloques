/*
 * CodiBloques - Parche del logo superior-izquierdo.
 * Cambia la imagen y el link a GitHub del desarrollador.
 *
 * Uso:
 *   1. Pon tu logo SVG en build/codibloques-logo.svg
 *   2. Ejecuta: node tools/patch-logo.cjs
 *
 * Requisitos del logo:
 *   - Formato: SVG (vectorial, escala perfecto)
 *   - Proporción: ~2.67:1 (ancho:alto) - igual que el original (66.1×24.7)
 *     Ejemplos: 200×75px, 400×150px, 133×50px
 *   - Fondo transparente
 *   - Colores que funcionen sobre fondo oscuro (la barra es azul #4C97FF)
 */
const fs = require('fs');
const path = require('path');

const GUI_JS = path.join(__dirname, '..', 'gui', 'gui.js');
const LOGO_SVG = path.join(__dirname, '..', 'build', 'codibloques-logo.svg');
const GITHUB_URL = 'https://github.com/jpinformaticaok';

if (!fs.existsSync(LOGO_SVG)) {
  console.log('ERROR: No existe', LOGO_SVG);
  console.log('');
  console.log('Crea tu logo SVG con estas especificaciones:');
  console.log('  - Formato: .svg (vectorial)');
  console.log('  - Proporción: ~2.67:1 (ancho:alto)');
  console.log('    Ej: 200x75, 400x150, 267x100, 133x50');
  console.log('  - Fondo transparente');
  console.log('  - Colores claros (la barra de menú es azul #4C97FF)');
  console.log('');
  console.log('Guárdalo en:', LOGO_SVG);
  process.exit(1);
}

const logoSvg = fs.readFileSync(LOGO_SVG, 'utf8');
const logoBase64 = Buffer.from(logoSvg).toString('base64');
const logoDataUrl = `data:image/svg+xml;base64,${logoBase64}`;

let src = fs.readFileSync(GUI_JS, 'utf8');

let changed = 0;

// FIX 1: Cambiar el logo por defecto en MenuBar.defaultProps
// Buscar: logo: _scratch_logo_svg__WEBPACK_IMPORTED_MODULE_44__,
const fix1Pattern = /logo:\s*_scratch_logo_svg__WEBPACK_IMPORTED_MODULE_\d+__,/;
if (fix1Pattern.test(src)) {
  src = src.replace(fix1Pattern, `logo: "${logoDataUrl}",`);
  console.log('FIX 1 aplicado: logo por defecto actualizado');
  changed++;
} else {
  console.log('FIX 1: patrón no encontrado (quizá ya parcheado)');
}

// FIX 2: Cambiar onClickLogo en el entry point (playground)
// Buscar: const onClickLogo = () => { window.location = 'https://scratch.mit.edu'; };
const fix2Pattern = /const onClickLogo = \(\) => \{\s*window\.location = 'https:\/\/scratch\.mit\.edu';\s*\};/;
if (fix2Pattern.test(src)) {
  src = src.replace(fix2Pattern, `const onClickLogo = () => { window.location = '${GITHUB_URL}'; };`);
  console.log(`FIX 2 aplicado: link cambiado a ${GITHUB_URL}`);
  changed++;
} else {
  console.log('FIX 2: patrón no encontrado (quizá ya parcheado)');
}

// FIX 3: Cambiar el link en stage-header (fullscreen mode)
// Buscar: href: "https://scratch.mit.edu"
const fix3Pattern = /href:\s*"https:\/\/scratch\.mit\.edu"/g;
const fix3Matches = src.match(fix3Pattern);
if (fix3Matches) {
  src = src.replace(fix3Pattern, `href: "${GITHUB_URL}"`);
  console.log(`FIX 3 aplicado: ${fix3Matches.length} link(s) de fullscreen actualizados`);
  changed++;
} else {
  console.log('FIX 3: patrón no encontrado (quizá ya parcheado)');
}

if (changed > 0) {
  fs.writeFileSync(GUI_JS, src);
  console.log(`\nBundle parcheado (${changed} fixes).`);
  console.log('Recompila con: npm run dist');
} else {
  console.log('\nSin cambios (bundle ya parcheado o patrones cambiaron).');
}