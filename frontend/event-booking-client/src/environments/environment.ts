export const environment = {
  production: false,
  // during development the Angular CLI dev server uses proxy.conf.js
  // to forward `/api` requests to the backend gateway. The gateway
  // normally listens on 5000 (docker or local run), so we keep the
  // same base URL for builds that run without the dev server proxy.
  apiUrl: 'http://localhost:5000'
};
