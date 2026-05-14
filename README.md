# Event Booking and Management Platform
**Modern microservices-based event ticketing platform inspired by Ticketbox.vn**

A full-stack event booking system with .NET 10 microservices backend and Angular 19 frontend.

## 📋 Project Status

✅ **Latest Updates:**
- Fixed Angular 19 TypeScript configuration (`tsconfig.json`)
- Cleaned up API Gateway (removed demo weather endpoints)
- Implemented Ticketbox.vn-inspired frontend design
- Added BookingComponent with ticket selection flow
- Responsive mobile-first UI with SCSS
- JWT authentication configured in gateway

---

## 🏗️ Architecture

**Backend (C# .NET 10 Microservices):**
- **API Gateway** (port 5000) – entry point, request routing, JWT validation
- **EventService** (port 5001) – event management
- **UserService** (port 5002) – user authentication & profiles
- **BookingService** (port 5003) – ticket bookings
- **PaymentService** (port 5004) – payment processing

**Frontend (Angular 19):**
- Ticketbox.vn inspired UI design
- Responsive grid layout with SCSS
- Home page with hero, categories, event grid
- Event detail & booking flow

**Database:**
- PostgreSQL (persistent)

---

## 📦 Prerequisites

Before you start, install:

1. **Git** – [Download](https://git-scm.com/download)
2. **Docker & Docker Compose** – [Download](https://www.docker.com/products/docker-desktop)
3. **Node.js 18+** – [Download](https://nodejs.org/) (for frontend development)
4. **.NET 10 SDK** (optional) – [Download](https://dotnet.microsoft.com/download) (only if running services locally without Docker)

**Verify installations:**
```bash
git --version
docker --version
docker-compose --version
node --version
npm --version
```

---

## 🚀 Quick Start (Recommended: Docker)

### Step 1: Clone the repository

```bash
git clone https://github.com/QBMVnMan/Event-Booking-and-Management.git
cd Event-Booking-and-Management
```

### Step 2: Start all services with Docker

Make sure **Docker Desktop is running**, then:

```bash
docker compose up --build -d
```

**Wait 3-6 minutes** for services to initialize, there is a **LOT** of warnings, ignore them for now. Check status:

```bash
docker compose ps
```

You should see all services running (STATUS: Up). 

### Step 3: Access the services

| Service | URL |
|---------|-----|
| **Frontend** | http://localhost:4200 |
| **API Gateway** | http://localhost:5000 |
| **EventService** | http://localhost:5001 |
| **UserService** | http://localhost:5002 |
| **BookingService** | http://localhost:5003 |
| **PaymentService** | http://localhost:5004 |
| **PostgreSQL** | localhost:5432 |

### Step 4: Run the frontend dev server

In a **new terminal**:

```bash
cd frontend/event-booking-client
npm install          # install dependencies (first time only)
npm start            # start Angular dev server (port 4200)
```

The app opens at **https://127.0.0.1:50068** (or similar high port). Look for `Local:` in the terminal output.

✅ **You're done!** The frontend automatically proxies `/api` calls to the gateway on port 5000.

---

## 🛠️ Running Locally Without Docker

### Prerequisites for local run:

1. **PostgreSQL running** (local install or Docker)
2. **.NET 10 SDK** installed
3. **Node.js 18+**

### Step 1: Database setup

**Option A: PostgreSQL in Docker (easy)**
```bash
docker run --name postgres-event-booking \
  -e POSTGRES_PASSWORD=password123 \
  -e POSTGRES_DB=eventbooking \
  -p 5432:5432 \
  -d postgres:16
```

**Option B: Local PostgreSQL**

Install PostgreSQL, then create database:
```sql
CREATE DATABASE eventbooking;
-- Connection: localhost:5432, user: postgres, password: your_password
```

### Step 2: Run backend services

Open **5 separate terminals**:

```bash
# Terminal 1 – API Gateway
dotnet run --project api-gateway/src/ApiGateway --urls http://localhost:5000

# Terminal 2 – EventService
dotnet run --project services/EventService/src/EventService.Api --urls http://localhost:5001

# Terminal 3 – UserService
dotnet run --project services/UserService/src/UserService.Api --urls http://localhost:5002

# Terminal 4 – BookingService  
dotnet run --project services/BookingService/src/BookingService.Api --urls http://localhost:5003

# Terminal 5 – PaymentService
dotnet run --project services/PaymentService/src/PaymentService.Api --urls http://localhost:5004
```

### Step 3: Run frontend

```bash
cd frontend/event-booking-client
npm install
npm start
```

Open browser to **http://localhost:4200**

---

## 📝 Project Structure

```
Event-Booking-and-Management/
├── api-gateway/                 # .NET gateway (routes to microservices)
│   └── src/ApiGateway/Program.cs
├── services/
│   ├── EventService/            # Event CRUD, featured events
│   ├── UserService/             # User auth, profiles
│   ├── BookingService/          # Ticket bookings
│   └── PaymentService/          # Payment processing
├── frontend/
│   └── event-booking-client/    # Angular 19 SPA
│       ├── src/app/
│       │   ├── home/            # Home page (hero, categories, grid)
│       │   ├── booking/         # Ticket selection & booking flow
│       │   ├── event-detail.component.ts
│       │   └── event.service.ts # API integration
│       └── package.json
├── shared/
│   └── Contracts/               # Shared DTO models
├── docker-compose.yml           # Docker services definition
└── README.md
```

---

## 🔐 Authentication (JWT)

The API Gateway validates JWT tokens for protected endpoints.

### Manual test (via curl):

```bash
# 1. Login
curl -X POST http://localhost:5000/api/users/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin123"}'

# Response: {"accessToken": "eyJ0eXAi..."}

# 2. Use token to call protected endpoint
curl http://localhost:5000/api/events \
  -H "Authorization: Bearer eyJ0eXAi..."
```

---

## 🔧 Troubleshooting

| Issue | Solution |
|-------|----------|
| **Port already in use** | Kill the process on that port or use different ports in docker-compose.yml |
| **Services not starting** | Check `docker compose logs SERVICE_NAME` |
| **Frontend shows "Cannot GET /"** | Make sure frontend is running with `npm start` |
| **API calls fail (CORS/404)** | Verify API Gateway is running on port 5000 |
| **Database connection error** | Ensure PostgreSQL is running and connection string matches |
| **Angular build errors** | Delete `node_modules` and reinstall: `npm ci` |
| **.NET 10 Docker mmap errors** | The Dockerfiles are configured with .NET 10 compatibility fixes. If build fails, ensure Docker has 4GB+ RAM and try: `DOCKER_BUILDKIT=1 docker compose up --build -d` |

### Check service logs:

```bash
# View logs for a specific service
docker compose logs event-service

# View all logs with timestamps
docker compose logs --timestamps

# Stream live logs
docker compose logs -f
```

### .NET 10 Docker Configuration

This project uses .NET 10 with optimized Docker configurations to resolve mmap compatibility issues:

- **Environment Variables**: Memory management and telemetry disabled
- **Build Arguments**: BuildKit enabled for better caching
- **Health Checks**: Services wait for dependencies before starting
- **Build Optimization**: Parallel restore disabled, minimal verbosity

If you encounter Docker build issues with .NET 10, the configurations are already in place.

---

## 📚 Frontend Development

See [frontend/event-booking-client/README.md](frontend/event-booking-client/README.md) for Angular-specific setup.

**Common tasks:**
```bash
cd frontend/event-booking-client

npm start              # dev server (http://localhost:4200)
npm run build          # production build
npm test               # run unit tests
npm run lint           # run linter
```

---

## ⚙️ Backend Configuration

### API Gateway environment variables (optional):

- `JWT_KEY` – Secret key for JWT tokens (default: in appsettings)
- `JWT_ISSUER` – Token issuer (default: "EventBooking")
- `JWT_AUDIENCE` – Token audience (default: "EventBooking")

### Database connection string:

**Default (Docker):**
```
Host=postgres;Port=5432;Database=eventbooking;Username=postgres;Password=password123
```

**Local PostgreSQL:**
Edit each service's `appsettings.json` connection string.

---

## 🚢 Deployment

### Docker deployment:

```bash
# Build and push images
docker compose build

# Deploy to production
docker compose -f docker-compose.yml up -d
```

### Frontend (static hosting):

```bash
cd frontend/event-booking-client
npm run build
# Deploy dist/event_booking_and_management.client/ to your web server
```

---

## 📄 License

MIT License - feel free to use this project for learning and development!

---

## 👤 Contributors

- **Bùi Minh Quân** – Project Lead

---

## ❓ Need Help?

1. Check [Troubleshooting](#-troubleshooting) section above
2. Review service logs: `docker compose logs SERVICE_NAME`
3. Open an issue on GitHub

Happy coding! 🚀

**First, ensure PostgreSQL is running** (see Database Setup above).

Build the whole solution once:

```bash
dotnet build EventBookingMicroservices.slnx
```

**Important:** By default, the .NET launch profiles use random ports (e.g. 5083, 5238, etc.).
For the frontend proxy and microservices to work together, you must run each service on the correct port using `--urls`:

```bash
# Terminal 1 — UserService (port 5002)
dotnet run --project services/UserService/src/UserService.Api --urls http://localhost:5002

# Terminal 2 — EventService (port 5001)
dotnet run --project services/EventService/src/EventService.Api --urls http://localhost:5001

# Terminal 3 — BookingService (port 5003)
dotnet run --project services/BookingService/src/BookingService.Api --urls http://localhost:5003

# Terminal 4 — PaymentService (port 5004)
dotnet run --project services/PaymentService/src/PaymentService.Api --urls http://localhost:5004

# Terminal 5 — API Gateway (port 5000)
dotnet run --project api-gateway/src/ApiGateway --urls http://localhost:5000
```

Frontend as above.

## Important Notes

- **JWT keys** are currently in `appsettings.Development.json` — **development only**.  
  Never use these in production. Switch to env vars / secrets manager.

- **Database**: in-memory (or SQLite) for now — data resets on restart.  
  No persistent DB container yet.

- **Main entry point**: always use the gateway (`localhost:5000`), not direct service ports.

- **Frontend → Backend**: ensure the Angular app points to `http://localhost:5000` (check `environment.ts` or proxy config).

## Frontend build verification

I verified the Angular app builds cleanly in this repository. To reproduce locally or in CI:

```bash
cd frontend/event-booking-client
npm ci
npm run build -- --configuration production
# output will be in ./dist/event_booking_and_management.client
```

Note: the backend solutions no longer contain the frontend `.esproj` to avoid MSBuild attempting to load Visual Studio-only SDKs during `dotnet build` in CI or Docker. Build and deploy the frontend separately (Node/ng build or the provided Dockerfile if you add one).



## Troubleshooting (quick)

- Nothing on port 5000? → Make sure you started the API Gateway with `--urls http://localhost:5000`.
- Nothing on port 5001/5002/5003/5004? → Start each service with the correct `--urls` as above.
- Port conflict? → Stop other apps or change ports in `docker-compose.yml` and your run commands.
- 401 / CORS? → Check gateway CORS policy + token header forwarding.
- Frontend blank / API errors? → Browser console + network tab.
- **`ECONNREFUSED` in dev‑server log?** → The proxy is trying to reach `http://localhost:5000` (the API gateway). Make sure the gateway/service process is running on the correct port (see above) or adjust `API_GATEWAY_PORT` / `environment.apiUrl` accordingly. The Angular client now falls back to sample data when the backend isn’t available.

**Tip:** You can override the proxy target for `/api` by setting the `API_GATEWAY_PORT` environment variable before running `npm start` in the frontend.

Feel free to open issues if something breaks.

Good luck coding!  
Bùi Minh Quân 
