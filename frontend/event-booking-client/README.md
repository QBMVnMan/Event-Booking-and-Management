# EventBookingAndManagementClient

This project was generated using [Angular CLI](https://github.com/angular/angular-cli) version 19.2.19.

## Development server

To start a local development server, run:

```bash
ng serve
```

Once the server is running, open your browser and navigate to `http://localhost:4200/`.

## Building

To build the project run:

```bash
ng build
```

## Running unit tests

To execute unit tests with the [Karma](https://karma-runner.github.io) test runner, use the following command:

```bash
ng test
```

---

## Recent frontend changes (Ticketbox-style homepage)

What changed:

- Added a simple Ticketbox-like homepage layout and components:
  - `EventService` to fetch events from your API gateway
  - `app-event-card` reusable component for event cards
  - `EventDetailComponent` and route `events/:id`
  - Search input wired to query the events endpoint
- Unit test updated: `app.component.spec.ts` now tests `/api/events/featured` and `/api/events` requests

How to run (development):

```bash
cd frontend/event-booking-client
npm ci
npm start   # runs the angular dev server (see package.json for start:default)
```

How to build (production):

```bash
npm run build
```

How to test (local Chrome):

```bash
npm test -- --watch=false
```

Notes for CI / containerized environments:

- Karma requires a headless browser. In CI you can either provide `CHROME_BIN` to a Chromium binary, or use a Puppeteer-based Chromium. I can add a `test:ci` script that uses `puppeteer` if you want tests to run inside the container.
- The frontend expects the following API endpoints (adjust proxy or gateway accordingly):
  - `GET /api/events/featured` → featured events
  - `GET /api/events` → event list (supports `category` and `q` query params)

Files changed (frontend):

- `src/app/app.component.ts`, `src/app/app.component.html`, `src/app/app.component.css`
- `src/app/event.service.ts`
- `src/app/event-card.component.*`
- `src/app/event-detail.component.*`
- `src/app/app-routing.module.ts`, `src/app/app.module.ts`
- `src/app/app.component.spec.ts`

Suggested commit message:

```
feat(frontend): add Ticketbox-style homepage, EventService, event components and routing
```

For more information on using the Angular CLI, including detailed command references, visit the [Angular CLI Overview and Command Reference](https://angular.dev/tools/cli) page.
