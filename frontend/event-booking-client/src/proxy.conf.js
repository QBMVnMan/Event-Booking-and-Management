const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:7225';

const PROXY_CONFIG = [
  {
    context: [
      "/weatherforecast",
    ],
    target,
    secure: false
  },
  {
    context: ["/api"],
    // point at the gateway; the default Docker compose gateway port is 5000.
    // you can override with API_GATEWAY_PORT=xxxx if you happen to run the
    // backend on a different port during development.
    target: process.env.API_GATEWAY_PORT ? `http://localhost:${process.env.API_GATEWAY_PORT}` : 'http://localhost:5000',
    secure: false,
    changeOrigin: true,
    logLevel: 'debug',
    onError(err, req, res) {
      // swallow connection-refused errors so the dev server doesn't spam the
      // console; the app will still see a 502 and our EventService will apply
      // its own fallback logic.
      console.error('Proxy error:', err.message || err);
      if (!res.headersSent) {
        res.writeHead(502, { 'Content-Type': 'application/json' });
      }
      res.end(JSON.stringify({ error: 'backend unavailable' }));
    }
  }
]

module.exports = PROXY_CONFIG;
