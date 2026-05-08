# Event Booking Frontend - Angular 19

**Ticketbox.vn-inspired event booking application built with Angular 19**

This is the SPA (Single Page Application) frontend for the Event Booking and Management platform.

---

## 📋 Quick Start

### Prerequisites

- **Node.js 18+** – [Download](https://nodejs.org/)
- **npm 9+** – comes with Node.js
- **Git** – [Download](https://git-scm.com/download)

Verify installations:
```bash
node --version
npm --version
```

### Installation & Development

```bash
# 1. Navigate to frontend directory
cd frontend/event-booking-client

# 2. Install dependencies
npm install

# 3. Start development server
npm start
```

The app opens automatically at **http://localhost:4200** (or similar).

**Note:** Make sure the **API Gateway is running on port 5000** for API calls to work. See main [README.md](../../README.md#-quick-start-recommended-docker) for backend setup.

---

## 🎯 Features

✅ **Home Page**
- Hero section with featured events slider
- Event category filters
- Responsive event grid
- Search bar (integrated with backend)

✅ **Event Details**
- Full event information
- Image poster display
- Price information
- "Book Now" button

✅ **Booking Flow**
- Ticket type selection (Standard, VIP, Premium)
- Quantity selector
- Real-time price calculation
- Checkout summary

✅ **Responsive Design**
- Mobile-first approach
- Works on desktop, tablet, and phone
- Ticketbox.vn-inspired clean design

✅ **UI/UX**
- Blue-white accent color palette (#1976d2)
- Smooth hover/transition effects
- Intuitive navigation
- Clean typography

---

## 📁 Project Structure

```
frontend/event-booking-client/
├── src/
│   ├── app/
│   │   ├── home/                          # Home page component
│   │   │   ├── home.component.ts         # Logic: featured, categories, events
│   │   │   ├── home.component.html       # Template: hero, filters, grid
│   │   │   └── home.component.scss       # Styles: responsive layout
│   │   │
│   │   ├── booking/                       # Booking flow component
│   │   │   ├── booking.component.ts      # Ticket selection & payment
│   │   │   ├── booking.component.html    # Ticket types & quantity
│   │   │   └── booking.component.scss    # Booking UI styles
│   │   │
│   │   ├── event-detail.component.ts     # Event detail page
│   │   ├── event-detail.component.html
│   │   ├── event-detail.component.css
│   │   │
│   │   ├── event-card.component.ts       # Reusable event card
│   │   ├── event-card.component.html
│   │   ├── event-card.component.css
│   │   │
│   │   ├── event.service.ts              # API calls to backend
│   │   ├── app.component.ts              # Main app container
│   │   ├── app.component.html            # Header + footer
│   │   ├── app.module.ts                 # App module configuration
│   │   └── app-routing.module.ts         # Route definitions
│   │
│   ├── styles.css                        # Global styles
│   ├── main.ts                           # App bootstrap
│   ├── proxy.conf.js                     # API proxy config
│   └── index.html
│
├── package.json                          # Dependencies & scripts
├── tsconfig.json                         # TypeScript config
├── angular.json                          # Angular build config
└── README.md                             # This file
```

---

## 🔧 Available Commands

### Development

```bash
# Start dev server with hot reload
npm start

# Alternative (explicit):
ng serve --ssl --proxy-config src/proxy.conf.js --open
```

### Production Build

```bash
# Create optimized production build
npm run build

# Output: dist/event_booking_and_management.client/
```

### Testing

```bash
# Run unit tests (watch mode - needs Chrome)
npm test

# Run tests headless (CI-friendly)
ng test --watch=false --browsers=ChromeHeadless
```

### Code Quality

```bash
# Run linter
npm run lint

# Fix lint issues
ng lint --fix
```

### Clean & Debug

```bash
# Clear build cache
npm run clean

# Reinstall dependencies (clean install)
npm ci
```

---

## 🔗 API Integration

### Endpoints Used

The frontend expects these API endpoints from the backend:

```
GET  /api/events           → List all events (with optional filters)
GET  /api/events/featured  → Featured events for hero section
GET  /api/events/:id       → Single event detail
POST /api/bookings         → Create a booking (future)
```

### Proxy Configuration

By default, all `/api/*` requests are forwarded to **http://localhost:5000** (API Gateway) via `src/proxy.conf.js`.

To change the API gateway port:
```bash
# Set environment variable before starting
export API_GATEWAY_PORT=8000
npm start
```

The proxy ensures **no CORS issues in development**.

### EventService (API Client)

Located at `src/app/event.service.ts`:

```typescript
// Fetch featured events
eventService.getFeatured()  // GET /api/events/featured

// Get all events (optional filters)
eventService.getEvents(category?, query?)  // GET /api/events?category=...&q=...
```

---

## 🎨 Styling & Customization

### Global Styles

Edit `src/styles.css` for global colors, fonts, and utilities.

**Color Palette (Ticketbox.vn-inspired):**
- Primary Blue: `#1976d2`
- Secondary Blue: `#42a5f5`
- Accent Gray: `#f8f9fa`
- Text Dark: `#333`
- Text Light: `#666`

### Component Styles

Each component has its own `.scss` file:
- `home.component.scss` – hero, categories, event grid
- `booking.component.scss` – ticket selection, checkout
- `event-detail.component.css` – event info display

### Responsive Breakpoints

```scss
@media (max-width: 768px) {
  // Mobile styles
}

@media (max-width: 480px) {
  // Smaller mobile styles
}
```

---

## 🚀 Deployment

### Build for Production

```bash
npm run build -- --configuration production
```

Output directory: `dist/event_booking_and_management.client/`

### Host Static Files

Upload the `dist/` folder to any static hosting service:
- Apache/Nginx
- AWS S3 + CloudFront
- Vercel / Netlify
- GitHub Pages

**Example (Nginx):**
```nginx
server {
    listen 80;
    server_name example.com;
    
    location / {
        root /var/www/html/dist/event_booking_and_management.client;
        try_files $uri $uri/ /index.html;
    }
    
    location /api {
        proxy_pass http://api-gateway:5000;
    }
}
```

---

## 🐛 Troubleshooting

| Issue | Solution |
|-------|----------|
| **npm ERR! 404 Not Found** | Run `npm ci` instead of `npm install` for exact versions |
| **Angular dev server won't start** | Delete `node_modules` + `package-lock.json`, then `npm ci` |
| **Cannot GET / (blank page)** | Check browser console (F12) for errors; verify backend is running |
| **API calls fail (404/502)** | Ensure API Gateway is running on port 5000 |
| **CORS errors** | The proxy should handle this; check `src/proxy.conf.js` |
| **Port 4200 already in use** | Run on different port: `ng serve --port 4300` |
| **TypeScript errors** | Update tsconfig.json: ensure `lib: ["ES2022", "dom"]` |

### Debug Mode

Open browser DevTools (F12):

1. **Network tab** – check API calls to `/api/*`
2. **Console tab** – look for error messages
3. **Application tab** – inspect localStorage/sessionStorage

---

## 📚 Resources

- [Angular 19 Documentation](https://angular.dev)
- [TypeScript Handbook](https://www.typescriptlang.org/docs/)
- [SCSS/Sass Documentation](https://sass-lang.com/documentation)
- [Main Project README](../../README.md)

---

## ✅ Known Issues & Fixes

✅ **Recently Fixed:**
- TypeScript config issues (noImplicitAny, emitDecoratorMetadata)
- Component styling (styleUrl → styleUrls)
- Module resolution (node16 for proper @angular/* imports)
- API proxy configuration for dev server

---

## 💡 Tips for Beginners

1. **Always start backend first** – Frontend depends on API Gateway
2. **Check the console** (F12) for error clues
3. **Use VS Code extensions** – Angular Language Service, Prettier
4. **Read the Network tab** (F12) to debug API calls
5. **Hot reload works** – Save files to see changes instantly
6. **Build before deploying** – `npm run build` creates optimized output

---

## 🤝 Contributing

Found a bug? Want to add a feature?

1. Create a new branch: `git checkout -b feature/your-feature`
2. Make changes and commit: `git commit -m 'Add your feature'`
3. Push to GitHub: `git push origin feature/your-feature`
4. Open a Pull Request

---

## 📄 License

MIT License - Feel free to use this project for learning!

---

## 👤 Support

Need help? Check:
1. This README's **Troubleshooting** section
2. Main [README.md](../../README.md) for backend setup
3. Browser console (F12) for JavaScript errors
4. Network tab (F12) for API failures

Happy coding! 🎉
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

