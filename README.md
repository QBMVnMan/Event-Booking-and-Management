# Event Booking and Management 
Modern microservices-based event ticketing platform inspired by **Ticketbox.vn**.

- .NET 10 microservices (Event, User, Booking, Payment, API Gateway)
- Angular 19 frontend with Ticketbox-style UI
- PostgreSQL persistent database
- Docker-first development & deployment

Current status: basic auth + skeleton structure working. Core booking logic in progress.

## Prerequisites

- Git  
- Docker & Docker Compose  
- .NET 10 SDK (optional — local builds only)  
- Node.js 18+ + npm (frontend)

## Quick Start (recommended — Docker)

```bash
git clone https://github.com/QBMVnMan/Event-Booking-and-Management.git
# or: git clone git@github.com:QBMVnMan/Event-Booking-and-Management.git   (SSH)

cd Event-Booking-and-Management
 
# Remember to open Docker Desktop first
docker compose up --build -d
docker compose ps
```

Wait ~30–60 seconds for services to be ready.

**Exposed ports (localhost):**

- API Gateway     → http://localhost:5000  
- EventService    → http://localhost:5001  
- UserService     → http://localhost:5002  
- PostgreSQL → localhost:5432 (user: postgres, password: Postgres123!, db: eventbooking)

**Use the gateway (5000) for all client calls.**

## Database Setup (PostgreSQL)
The project now uses persistent PostgreSQL. Data survives container restarts.

- Automatic (Docker)
PostgreSQL is included in docker-compose.yml. All services automatically connect to it.

- Manual Connection String (if needed)
Each service uses this connection string by default:
```bash
"ConnectionStrings": {
  "DefaultConnection": "Host=postgres;Port=5432;Database=eventbooking;Username=postgres;Password=Postgres123!;Include Error Detail=true"
}
```
For local PostgreSQL (outside Docker), change Host=postgres to Host=localhost.

## Running Migrations (First Time Only)
```bash
# Run once after first start
docker exec -it event-booking-and-management-user-service dotnet ef database update
docker exec -it event-booking-and-management-event-service dotnet ef database update
docker exec -it event-booking-and-management-booking-service dotnet ef database update
docker exec -it event-booking-and-management-payment-service dotnet ef database update
```
Or run locally in each service folder:
```bash
cd services/UserService/src/UserService.Api && dotnet ef database update
```

## Local Dev Auth Demo (via gateway)

1. **Register**

```bash
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"username":"alice","password":"Password123!"}'
```

2. **Login** → copy `accessToken`

```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"alice","password":"Password123!"}'
```

3. **Call protected endpoint** (via gateway)

```bash
curl http://localhost:5000/api/protected \
  -H "Authorization: Bearer <paste-your-token-here>"
```

## Run Frontend

```bash
cd frontend/event-booking-client
npm ci           # install deps the first time or when package.json changes
npm start         # starts the Angular dev server over HTTPS
# or: ng serve --ssl --proxy-config proxy.conf.js
```

The CLI chooses a high port by default (`https://127.0.0.1:50068/` in our setup).  Look for
`Local:` line in the console after the app compiles and open that URL in your browser.

All `/api` requests from the SPA are forwarded to the API gateway on port 5000 via
`proxy.conf.js`. No additional CORS settings are required for local development.

>The frontend run port is unrelated to the backend ports; the proxy target is
controlled by `API_GATEWAY_PORT` (defaults to `5000`).

(For production builds you would host the compiled output behind the gateway or
another web server; see the `frontend/` README for build details.)

## Run without Docker (individual services)


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
