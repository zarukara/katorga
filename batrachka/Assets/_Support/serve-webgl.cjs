// Local preview only. No packages required: node Assets/_Support/serve-webgl.cjs
const http = require('node:http');
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '../../Builds/WebGL');
const port = Number(process.argv[2] || 8000);
const mime = { '.html': 'text/html; charset=utf-8', '.js': 'application/javascript', '.css': 'text/css', '.wasm': 'application/wasm', '.png': 'image/png', '.ico': 'image/x-icon' };
http.createServer((request, response) => {
  let file;
  try {
    const pathname = decodeURIComponent(new URL(request.url, 'http://localhost').pathname);
    file = path.resolve(root, '.' + (pathname === '/' ? '/index.html' : pathname));
    if (!file.startsWith(root + path.sep)) throw new Error('Invalid path');
  } catch { response.writeHead(400).end(); return; }
  fs.stat(file, (error, stat) => {
    if (error || !stat.isFile()) { response.writeHead(404).end('Build not found'); return; }
    response.writeHead(200, { 'Content-Type': mime[path.extname(file)] || 'application/octet-stream', 'Cache-Control': 'no-store' });
    fs.createReadStream(file).on('error', () => response.destroy()).pipe(response);
  });
}).listen(port, '127.0.0.1', () => console.log(`RTS: http://127.0.0.1:${port}`));
