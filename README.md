# Event-Booking-and-Management
A small monorepo for an event booking platform (microservices-style).

Quick goal: make it trivial to clone, run locally, and experiment with the microservices skeleton.

---

## One-line quick start

Clone, build and run all services with Docker Compose (recommended):

```bash
git clone git@github.com:QBMVnMan/Event-Booking-and-Management.git
cd Event-Booking-and-Management
docker compose up --build
```

API Gateway: http://localhost:5000
EventService: http://localhost:5001
UserService: http://localhost:5002

---

## Prerequisites

- Git
- Docker & Docker Compose (v2+)
- .NET SDK 10.x (for building/running individual services)
- Node.js 16+ and npm (for frontend development)

---

## Clone and run locally (short)

1. Clone repo:

```bash
git clone git@github.com:QBMVnMan/Event-Booking-and-Management.git
cd Event-Booking-and-Management
```

2a. Run everything with Docker Compose (recommended):

```bash
docker compose up --build
```

2b. Or run a single service locally:

```bash
# Build and run (example)
dotnet run --project services/UserService/src/UserService.Api --urls "http://localhost:5002"
```

3. Frontend (separate):

```bash
cd frontend/event-booking-client
npm ci
npm start
```

> Note: The frontend may require the Angular CLI or local Node toolchain. The backend solution is independent and can be built with dotnet commands.

---

## Authentication (dev flow)

A minimal JWT-based auth is available for local testing.

1. Register a user:

```bash
curl -X POST http://localhost:5002/api/auth/register -H "Content-Type: application/json" -d '{"username":"alice","password":"password"}'
```

2. Login to get a token:

```bash
curl -X POST http://localhost:5002/api/auth/login -H "Content-Type: application/json" -d '{"username":"alice","password":"password"}'
# Response: { "accessToken": "<token>", "tokenType": "Bearer" }
```

3. Call a protected API via the gateway:

```bash
curl http://localhost:5000/api/protected -H "Authorization: Bearer <token>"
```

**Important**: This is a dev-friendly implementation (in-memory users + dev signing key). Use a proper identity provider and secure key storage for production.

---

## Development notes

- Solution file: `EventBookingMicroservices.slnx` contains service projects.
- Shared DTOs and small helpers are in `shared/Contracts` for cross-service contracts.
- Add new services under `services/` and update `docker-compose.yml` if needed.

---

## Troubleshooting

- If `dotnet build` errors for the frontend project about `Microsoft.VisualStudio.JavaScript.Sdk`, build the frontend separately with npm:

```bash
cd frontend/event-booking-client
npm ci
npm run build
```

- If ports collide, stop the conflicting service or change ports in `docker-compose.yml`.

---

## Contributing

- Branch: `feat/..`, `fix/..`, `chore/..`
- Add tests and update the README when adding features
- Open a PR and request a reviewer

---

If you want, I can add a "Get started" script or a small helper script that does the clone -> docker compose up flow automatically for first-time setup.