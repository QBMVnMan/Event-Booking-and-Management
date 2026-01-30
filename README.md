# Event-Booking-and-Management
Event Booking and Management Platform

## Technologies Used
- Angular 19
- .NET 9.0
- Node.js

## Features
- Multi-language (Vietnamese/English)
- Dark mode support
- Role-based access (Admin/Users)
- Categories, and pagination
- User registration and login (Forgot Password UI only)
- Browse and book events (1 ticket per click)
- Show "Booked" label on booked events
- Payments methods
- Event details with full info
- Booking confirmation screen
- Admin panel for managing events (CRUD)
- Responsive design
## Monorepo structure (refactor-to-microservices)

This repository has been reorganized into a microservices-style monorepo. Key folders:

- `services/` - backend microservices (EventService, UserService, BookingService, PaymentService)
- `api-gateway/` - API Gateway project (place for Ocelot / YARP)
- `shared/` - shared libraries and contracts
- `frontend/` - Angular frontend (`event-booking-client`)
- `docker-compose.yml` - run all services locally via Docker

## Quick start (local, docker)

1. Build and run services locally with Docker Compose:

```bash
docker compose up --build
```

2. API Gateway: http://localhost:5000
3. EventService: http://localhost:5001

Note: this is an initial refactor; services are skeleton projects. Replace and extract code from the old monolith into the service folders incrementally.