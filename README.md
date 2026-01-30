# Event Booking and Management — Quick Start 🚀

A minimal microservices monorepo (backend services, API gateway, shared contracts, Angular frontend).

Prerequisites:
- Git
- Docker & Docker Compose
- .NET 10 SDK (for local builds)
- Node.js + npm (for frontend)

Quick start (recommended: Docker):
```bash
git clone git@github.com:QBMVnMan/Event-Booking-and-Management.git
cd Event-Booking-and-Management
docker compose up --build -d
docker compose ps
```

Useful service ports (local compose):
- API Gateway: http://localhost:5000
- EventService: http://localhost:5001
- UserService: http://localhost:5002

Local dev auth (demo):
1) Register:
```bash
curl -X POST http://localhost:5002/api/auth/register -H "Content-Type: application/json" -d '{"username":"alice","password":"password"}'
```
2) Login:
```bash
curl -X POST http://localhost:5002/api/auth/login -H "Content-Type: application/json" -d '{"username":"alice","password":"password"}'
# Returns accessToken
```
3) Call protected gateway endpoint:
```bash
curl http://localhost:5000/api/protected -H "Authorization: Bearer <token>"
```

Notes:
- Development JWT keys are in `appsettings.Development.json`. Use env vars/secret store in production.
- To work on one service without Docker:
  - `dotnet build EventBookingMicroservices.slnx`
  - `dotnet run --project services/<Name>/src/<Name>.Api`
- Frontend: `cd frontend/event-booking-client && npm ci && npm start`

