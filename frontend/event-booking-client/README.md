# EventBookingAndManagementClient

This project was generated using [Angular CLI](https://github.com/angular/angular-cli) version 19.2.19.

## Development server

To start a local development server with API proxy, run:

```bash
npm start
```

Or explicitly with proxy configuration:

```bash
ng serve --proxy-config src/proxy.conf.js
```

Once the server is running, open your browser and navigate to `http://localhost:4200/`.

## Building

To build the project run:

```bash
npm run build
```

## Running unit tests

To execute unit tests with the [Karma](https://karma-runner.github.io) test runner, use the following command:

```bash
ng test
```

---

## Frontend Structure (Ticketbox-style event booking UI)

### Overview

The frontend is a Ticketbox-inspired event booking interface with:
- Event listing and search
- Featured events carousel
- Category filtering
- Event detail page with routing
- Responsive grid layout with basic CSS

### Key Components

- **AppComponent** – main layout shell (header, search, footer)
- **HomeComponent** – page container with hero, categories, and event grid
- **EventService** – API integration for fetching events from backend
- **EventDetailComponent** – individual event detail page

### API Integration

The frontend connects to your API gateway via a proxy configuration (`src/proxy.conf.js`):

**Expected endpoints:**
- `GET /api/events/featured` – returns featured events
- `GET /api/events` – returns all events (supports `category` and `q` query params)

**Dev server proxy:**
- All `/api/*` requests are forwarded to `http://localhost:8080` during development
- Ensure your API gateway is running on port 8080 or update `src/proxy.conf.js`

### Recent updates

- ✅ Fixed `styleUrl` → `styleUrls` in AppComponent (CSS now loads correctly)
- ✅ Consolidated duplicate EventService files → single source of truth at `src/app/event.service.ts`
- ✅ Updated HomeComponent to use root EventService with proper method names
- ✅ Fixed HTML compilation errors (encoded `@` in email, removed redundant optional chaining)
- ✅ Added missing `search()` method in AppComponent
- ✅ Added comprehensive global CSS for layout and components
- ✅ Verified successful production build (no errors)

### How to run locally

**Development (with hot reload and proxy):**

```bash
cd frontend/event-booking-client
npm install        # if needed
npm start          # runs ng serve with proxy config
```

**Production build:**

```bash
npm run build
# outputs to dist/event_booking_and_management.client
```

**Run tests:**

```bash
npm test -- --watch=false   # headless
npm test                      # watch mode (requires Chrome)
```

### Files Modified

**Core components:**
- `src/app/app.component.ts` – main layout, event loading, search
- `src/app/app.component.html` – header, footer, home container
- `src/app/app.component.css` – layout styles

**Services & data:**
- `src/app/event.service.ts` – consolidated API service

**Pages & features:**
- `src/app/home/home.component.ts|html|scss` – event listing page
- `src/app/event-detail.component.ts|html|css` – event detail page
- `src/app/app-routing.module.ts` – routing configuration
- `src/app/app.module.ts` – module imports and declarations

**Config & styles:**
- `src/styles.css` – global styles (header, footer, grid, cards)
- `src/proxy.conf.js` – dev server proxy to API gateway
- `src/environments/environment.ts` – API configuration
- `angular.json` – build configuration

**Tests:**
- `src/app/app.component.spec.ts` – updated to test API calls

### Notes for CI/CD

- **Build:** `npm run build` – outputs a production-ready bundle
- **Testing in CI:** Karma requires a headless browser. Provide `CHROME_BIN` env var or install Chromium. Consider using Puppeteer for containerized environments.
- **Proxy:** The proxy configuration is only active during `ng serve` development. For production, update your API base URL or reverse proxy accordingly.

---

For more information on using the Angular CLI, including detailed command references, visit the [Angular CLI Overview and Command Reference](https://angular.dev/tools/cli) page.

