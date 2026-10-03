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

  // ===== FIX 4/5: Traducción al español rioplatense (bloques "es" y "es-419") =====
  // Scratch trae varios textos en inglés incluso dentro de los bloques de idioma
  // español: el diálogo de borrado (FIX 4) y el modal "Debug" de la barra de
  // menú con sus 13 secciones (FIX 5). Se reemplazan SOLO dentro de esos dos
  // bloques de locale para no tocar el resto de los idiomas.
  const TRANSLATIONS = {
    // --- FIX 4: diálogo de confirmación de borrado ---
    'gui.gui.shouldDeleteSprite': '¿Seguro que querés borrar este objeto?',
    'gui.gui.shouldDeleteCostume': '¿Seguro que querés borrar este disfraz?',
    'gui.gui.shouldDeleteSound': '¿Seguro que querés borrar este sonido?',
    'gui.gui.deleteAssetHeading': 'Confirmar borrado',
    'gui.gui.confirm': 'sí',
    'gui.gui.cancel': 'no',

    // --- FIX 5: modal DEBUG (barra de menú + secciones) ---
    'gui.menuBar.debug': 'Depuración',
    'gui.debugModal.title': 'Depuración | Cómo salir del apuro',
    'gui.debugModal.readAloud.title': 'Leé en voz alta',
    'gui.debugModal.readAloud.description1':
      'Mientras leés tu código en voz alta, pensá desde la perspectiva de la computadora.',
    'gui.debugModal.readAloud.description2': '¿Estás incluyendo pasos que no existen?',
    'gui.debugModal.readAloud.description3': '¿Tus instrucciones son claras?',
    'gui.debugModal.readAloud.description4':
      'Si algo necesita reiniciarse cada vez que se ejecuta el programa, ¿esas instrucciones están incluidas?',
    'gui.debugModal.breakItDown.title': 'Dividí en partes',
    'gui.debugModal.breakItDown.description1':
      'Separá los bloques en partes más chicas (o secuencias) y hacé clic para ver qué hace cada una.',
    'gui.debugModal.breakItDown.description2':
      'Cuando las secuencias más chicas funcionen como esperás, volvé a sumarlas al programa principal.',
    'gui.debugModal.breakItDown.description3': 'Ese proceso se llama descomposición.',
    'gui.debugModal.slowItDown.title': 'Ralentizalo',
    'gui.debugModal.slowItDown.description1':
      'La computadora ejecuta tu programa tan rápido que a veces cuesta seguirlo con los ojos.',
    'gui.debugModal.slowItDown.description2':
      'Agregá bloques de espera o de «esperar hasta» temporales para ralentizar la secuencia. Así tenés tiempo de analizar si una parte funcionó o no.',
    'gui.debugModal.slowItDown.description3':
      'Quitá esos bloques de espera cuando tu código funcione.',
    'gui.debugModal.addSoundCheckpoints.title': 'Agregá puntos de control con sonido',
    'gui.debugModal.addSoundCheckpoints.description1':
      'Como en la estrategia Ralentizalo, podés agregar sonidos distintos con el bloque «reproducir hasta terminar» en puntos clave para probar tu secuencia.',
    'gui.debugModal.addSoundCheckpoints.description2':
      'Si un sonido no se reproduce, el error puede estar antes de ese bloque. Si el sonido se reproduce, el error probablemente esté después.',
    'gui.debugModal.addSoundCheckpoints.description3':
      'Quitá los sonidos cuando tu código funcione.',
    'gui.debugModal.tinkerWithBlockOrder.title': 'Modificá el orden de los bloques',
    'gui.debugModal.tinkerWithBlockOrder.description1':
      'Probá ajustar el orden o la secuencia de los bloques.',
    'gui.debugModal.tinkerWithBlockOrder.description2': '¿Qué tiene que pasar primero?',
    'gui.debugModal.tinkerWithBlockOrder.description3': '¿Qué pasa en segundo lugar?',
    'gui.debugModal.tinkerWithBlockOrder.description4':
      '¿Los valores o los objetos necesitan reiniciarse antes de que se ejecute la siguiente parte del código?',
    'gui.debugModal.tinkerWithBlockOrder.description5':
      'Probá usar bloques dentro de un bucle o de una instrucción condicional, en lugar de afuera de ellos.',
    'gui.debugModal.toLoopOrNot.title': '¿Repetir o no repetir?',
    'gui.debugModal.toLoopOrNot.description1':
      'Si usás bloques de control como «por siempre» y «repetir», revisá que todos los bloques dentro del bucle tengan que estar ahí, o si falta un bloque (como «esperar») para reiniciar la acción o ajustar el tiempo. ¿Querés que tu bucle se repita por siempre o cierta cantidad de veces? ¿Algo debería cortar la repetición?',
    'gui.debugModal.toLoopOrNot.description2':
      '¿Quizás no estás usando un bucle cuando deberías? Por ejemplo, si usás un bloque de instrucción condicional como «si entonces», ¿el programa solo necesita verificar si es verdadero o falso una vez? ¿O necesita verificar continuamente, en cuyo caso querrías poner tu instrucción condicional dentro de un bucle «por siempre»?',
    'gui.debugModal.timingAndParallelism.title': 'Pensá en el tiempo y el paralelismo',
    'gui.debugModal.timingAndParallelism.sectionTitle': 'Tiempo y paralelismo',
    'gui.debugModal.timingAndParallelism.description1':
      '¿Tenés varios eventos tratando de ejecutarse al mismo tiempo? Si dos secuencias están programadas para arrancar a la vez, puede haber comportamientos impredecibles.',
    'gui.debugModal.timingAndParallelism.description2':
      'Agregá esperas cortas, avisos o interacción del usuario (como hacer clic o presionar una tecla) para ver si eso afecta el resultado.',
    'gui.debugModal.thinkAboutBlockOptions.title': 'Pensá en las opciones de los bloques',
    'gui.debugModal.thinkAboutBlockOptions.description1':
      '¿Hay un bloque parecido pero distinto que puedas usar?',
    'gui.debugModal.thinkAboutBlockOptions.description2':
      'Algunos bloques se parecen pero se comportan de forma distinta, como «fijar» frente a «cambiar» o «reproducir hasta terminar» frente a «iniciar».',
    'gui.debugModal.thinkAboutBlockOptions.description3':
      'Probá usar un bloque parecido en lugar del que tenés, y mirá si eso afecta el resultado.',
    'gui.debugModal.checkTheValues.title': 'Revisá los valores',
    'gui.debugModal.checkTheValues.description1':
      'Si usás variables o bloques de reporte, revisá el valor en el momento en que se ejecuta la secuencia de código.',
    'gui.debugModal.checkTheValues.description2':
      '¿Todos los objetos deben controlar una variable, o solo uno?',
    'gui.debugModal.checkTheValues.description3':
      '¿Dónde se reinicia el valor? ¿Dónde se modifica?',
    'gui.debugModal.checkCodeSequence.title': 'Revisá la secuencia de código',
    'gui.debugModal.checkCodeSequence.description1':
      'Revisá que tu secuencia de código esté pegada al objeto correcto o al fondo, si corresponde.',
    'gui.debugModal.checkCodeSequence.description2':
      'Si necesitás mover tu código a otro objeto, hacé clic y arrastralo hasta que estés sobre el objeto correcto. Soltalo cuando el objeto se mueva.',
    'gui.debugModal.checkCodeSequence.description3':
      'También podés usar tu Mochila (abajo de la pantalla) para guardar y mover tu código o recursos.',
    'gui.debugModal.commentYourCode.title': 'Comentá tu código',
    'gui.debugModal.commentYourCode.description1':
      'Agregar comentarios a tu código ayuda a otros a entenderlo. También te ayuda a recordar cómo funciona cuando vuelvas a él más adelante.',
    'gui.debugModal.commentYourCode.description2':
      'Hacé clic derecho en el área de guiones para «Agregar comentario». Usá un lenguaje cotidiano para explicar qué hace un bloque, o una secuencia chica de bloques.',
    'gui.debugModal.takeABreak.title': 'Hacé una pausa y alejate',
    'gui.debugModal.takeABreak.description1':
      'A veces, pasar demasiado tiempo concentrado en un problema puede ser contraproducente y frustrante.',
    'gui.debugModal.takeABreak.description2':
      'Hacé una pausa y alejate de la pantalla para despejarte. Después de descansar, enfocarte en otra cosa o tomar un poco de agua, podés encarar el problema con ojos frescos.',
    'gui.debugModal.askForHelp.title': 'Pedí ayuda',
    'gui.debugModal.askForHelp.description1':
      'Si seguís trabado, podés pedirle ayuda a un compañero. Buscá un estudio de depuración o de ayuda, compartí tu proyecto y pedí ayuda en un comentario o en las notas del proyecto.',
    'gui.debugModal.askForHelp.description2':
      'Pedile a una, dos o tres personas que prueben tu código, ¡porque cada persona puede tener otras perspectivas u otras soluciones!'
  };

  // Encuentra los límites de un bloque de locale: '\n  "es": { ... \n  "otro": {'
  // Las claves internas del bloque sangran 4 espacios, por lo que '\n  "' (2 espacios)
  // identifica únicamente el inicio de un bloque de locale de nivel superior.
  const localeBlockRange = (text, locale) => {
    const start = text.indexOf('\n  "' + locale + '": {');
    if (start < 0) return null;
    const end = text.indexOf('\n  "', start + 5);
    return end < 0 ? { start, end: text.length } : { start, end };
  };

  const escapeRegex = (s) => s.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');

  let translatedTotal = 0;
  let alreadyTotal = 0;
  for (const locale of ['es', 'es-419']) {
    const range = localeBlockRange(src, locale);
    if (!range) {
      console.log(`${bundleName} - FIX 4/5: locale ${locale} no encontrado`);
      continue;
    }
    let seg = src.slice(range.start, range.end);
    let segChanged = 0;
    let segAlready = 0;
    for (const [key, value] of Object.entries(TRANSLATIONS)) {
      // El valor actual puede contener comillas escapadas (\") o escapes de
      // barra invertida, por eso se usa un patrón que las respeta.
      const re = new RegExp('("' + escapeRegex(key) + '":\\s*")((?:[^"\\\\]|\\\\.)*)(")');
      const m = seg.match(re);
      if (!m) continue;
      const encoded = JSON.stringify(value).slice(1, -1);
      if (m[2] === encoded) {
        segAlready++;
        continue;
      }
      seg = seg.replace(re, (_all, p1, _old, p3) => p1 + encoded + p3);
      segChanged++;
    }
    if (segChanged > 0) {
      src = src.slice(0, range.start) + seg + src.slice(range.end);
      console.log(
        `${bundleName} - FIX 4/5: ${locale} traducido (${segChanged} texto/s, ${segAlready} ya estaban)`
      );
      translatedTotal += segChanged;
    } else {
      console.log(
        `${bundleName} - FIX 4/5: ${locale} ya estaba traducido (${segAlready} texto/s)`
      );
    }
    alreadyTotal += segAlready;
  }
  if (translatedTotal > 0) changed++;
  if (translatedTotal + alreadyTotal < Object.keys(TRANSLATIONS).length * 2) {
    console.log(
      `${bundleName} - FIX 4/5 aviso: solo ${translatedTotal + alreadyTotal} de ` +
        `${Object.keys(TRANSLATIONS).length * 2} textos encontrados`
    );
  }

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
