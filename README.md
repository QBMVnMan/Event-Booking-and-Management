# Event Booking and Management — Quick Start 🚀

Minimal microservices monorepo:  
- .NET 10 backend services  
- API Gateway  
- Shared Contracts  
- Angular frontend  

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

**Use the gateway (5000) for all client calls.**

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
npm ci
npm start
# or: ng serve
```

→ Open http://localhost:4200

(Frontend should call the gateway at http://localhost:5000 — make sure CORS allows it.)

## Run without Docker (individual services)

Build whole solution once:

```bash
dotnet build EventBookingMicroservices.slnx
```

Run services separately (from root):

```bash
# Terminal 1 — UserService
dotnet run --project services/UserService/src/UserService.Api

# Terminal 2 — EventService
dotnet run --project services/EventService/src/EventService.Api

# Terminal 3 — Gateway
dotnet run --project api-gateway/src/ApiGateway
```

Frontend as above.

## Important Notes

- **JWT keys** are currently in `appsettings.Development.json` — **development only**.  
  Never use these in production. Switch to env vars / secrets manager.

- **Database**: in-memory (or SQLite) for now — data resets on restart.  
  No persistent DB container yet.

- **Main entry point**: always use the gateway (`localhost:5000`), not direct service ports.

- **Frontend → Backend**: ensure the Angular app points to `http://localhost:5000` (check `environment.ts` or proxy config).

## Troubleshooting (quick)

- Nothing on port 5000? → `docker compose logs -f gateway`  
- Port conflict? → stop other apps or change ports in `docker-compose.yml`  
- 401 / CORS? → check gateway CORS policy + token header forwarding  
- Frontend blank / API errors? → browser console + network tab

Feel free to open issues if something breaks.

Good luck coding!  
Bùi Minh Quân 
