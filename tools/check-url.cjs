fetch('http://127.0.0.1:8605/static/local-assets/809d9b47347a6af2860e7a3a35bce057.svg')
  .then(async (r) => console.log('status:', r.status, 'type:', r.headers.get('content-type'), 'len:', (await r.arrayBuffer()).byteLength))
  .catch((e) => console.log('ERR:', e.message));
