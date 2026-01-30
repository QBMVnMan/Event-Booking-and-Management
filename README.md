# Event-Booking-and-Management
Event Booking and Management Platform

## Overview
This repository has been reorganized into a microservices-style **monorepo**. It contains several backend services (skeletons), an API gateway, shared libraries, and the existing Angular frontend.

**Key folders**

- `services/` - backend microservices (EventService, UserService, BookingService, PaymentService)
- `api-gateway/` - API Gateway project (Ocelot or YARP can be used here)
- `shared/` - shared libraries and contracts
- `frontend/` - Angular frontend (`event-booking-client`)
- `docker-compose.yml` - orchestrate services for local development

---

## Prerequisites ✅

- Git (>= 2.25)
- Docker & Docker Compose (v2+) - for local multi-service runs
- .NET SDK (10.x) - for building and running services
- Node.js (16+) and npm or pnpm - for the Angular frontend
- (Optional) Angular CLI (`npm i -g @angular/cli`) if you plan to run/debug the frontend locally

Check versions:

```bash
git --version
docker --version
dotnet --info
node --version
npm --version
```

---

## Clone the repo and create a branch 🧭

```bash
# Clone (SSH)
git clone git@github.com:QBMVnMan/Event-Booking-and-Management.git
cd Event-Booking-and-Management
# Create a feature branch
git switch -c feat/your-feature
```

---

## Build & run (backend only) ⚙️

A solution that aggregates the services is included: `EventBookingMicroservices.slnx`.

```bash
# Build all backend projects
dotnet build EventBookingMicroservices.slnx

# Run a specific service (example: EventService)
dotnet run --project services/EventService/src/EventService.Api
```

Notes:
- If you only need to work on a single service, use `dotnet run --project` on that service's project file.
- The frontend project is an Angular project and is built separately with Node tooling.

---

## Run everything with Docker (recommended for local dev) 🐳

The repository includes `docker-compose.yml` that builds and runs all services.

```bash
# Build & run in foreground
docker compose up --build

# Or run in background
docker compose up --build -d

# Stop and remove containers
docker compose down
```

Default service ports (local compose):
- API Gateway - http://localhost:5000
- EventService - http://localhost:5001
- UserService - http://localhost:5002
- BookingService - http://localhost:5003
- PaymentService - http://localhost:5004

To rebuild only one service:

```bash
docker compose up --build --no-deps --force-recreate <service-name>
```

---

## Frontend (Angular) 🚀

The frontend lives at `frontend/event-booking-client`.

```bash
cd frontend/event-booking-client
npm ci
npm start    # starts dev server (project configured to use SPA proxy when backend runs locally)
# or
ng serve      # if you use the Angular CLI
```

If the app fails to build due to the Visual Studio JavaScript SDK when running `dotnet build` in this environment, build and run it separately with `npm ci` and `npm start` as shown above.

---

## Environment variables & configuration 🔧

Services support common environment settings (example):

- `ASPNETCORE_ENVIRONMENT` (Development, Production)
- `ConnectionStrings__Default` (database connection string)
- `JWT__Key` / `JWT__Issuer` (if JWT auth is implemented)

You can set env vars in `docker-compose.yml` or pass them when running locally:

```bash
ASPNETCORE_ENVIRONMENT=Development dotnet run --project services/UserService/src/UserService.Api
```

---

## Health checks & quick API smoke tests ❤️

Services should expose a health endpoint (e.g., `/health` or `/healthz`). Example curl:

```bash
curl -f http://localhost:5001/health || echo "EventService unhealthy"
curl http://localhost:5002/health
```

Also try the sample WeatherForecast endpoint shipped with templates:

```bash
curl http://localhost:5001/WeatherForecast
```

---

## Authentication (local dev flow) 🔐

A minimal JWT auth flow is implemented for local development:

1. Register a user (example):

```bash
curl -X POST http://localhost:5002/api/auth/register -H "Content-Type: application/json" -d '{"username":"alice","password":"password"}'
```

2. Login to get a token:

```bash
curl -X POST http://localhost:5002/api/auth/login -H "Content-Type: application/json" -d '{"username":"alice","password":"password"}'
# Response: { "accessToken": "<token>", "tokenType": "Bearer" }
```

3. Call a protected gateway endpoint:

```bash
curl http://localhost:5000/api/protected -H "Authorization: Bearer <token>"
```

Notes:
- The dev JWT signing key is present in `appsettings.Development.json` for `UserService` and `ApiGateway`. Replace with a secure secret (env var or secret store) in production.
- This is a minimal, demo-friendly implementation. For production, use a full-featured identity provider (OIDC) or a hardened token service.

---

## Tests and code quality ✅

- Run backend tests (per-project):

```bash
# Example (if tests exist)
dotnet test services/UserService/tests
```

- Add unit/integration tests for new features and require tests on PRs.

---

## CI Example (GitHub Actions) ⚙️

You can add a simple workflow `.github/workflows/ci.yml` to build and test each service on PR:

```yaml
name: CI
on: [push, pull_request]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '10.x'
      - name: Build
        run: dotnet build EventBookingMicroservices.slnx --no-restore
      - name: Test
        run: dotnet test --no-build --verbosity normal
```

---

## Adding a new service (recommended workflow) ➕

1. Create folder and scaffold webapi:

```bash
mkdir -p services/NewService/src
dotnet new webapi -n NewService.Api -o services/NewService/src/NewService.Api --framework net10.0
```

2. Add shared references and Dockerfile; update `docker-compose.yml` and add the project to `EventBookingMicroservices.slnx`.

3. Implement functionality, add tests, and open a PR.

---

## CONTRIBUTING & PR checklist ✅

- Create a branch: `feat/` / `fix/` / `chore/` style
- Add tests for new behavior
- Run `dotnet build` and `dotnet test` locally
- Run `docker compose up --build` if your change touches containerized services
- Include a clear PR description and request reviewers

---

## Troubleshooting & notes ⚠️

- **Frontend SDK issue during `dotnet build`**: If you see errors about `Microsoft.VisualStudio.JavaScript.Sdk`, build the frontend with `npm ci` and run it with `npm start`. The backend `EventBookingMicroservices.slnx` contains only .NET projects and should build fine.
- **Ports conflict**: stop local services or change ports in `docker-compose.yml` or the project's `launchSettings.json`.

---

## Contact / Support

If you need help with the migration or CI setup, open an issue or tag `@maintainers` on the PR and we'll assist.

---

If you'd like, I can:
- open a PR with this README update (I will), and
- merge it after you confirm — or automatically merge if you prefer.
