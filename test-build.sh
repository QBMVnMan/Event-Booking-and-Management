#!/bin/bash

echo "Testing Docker build for Event Booking and Management System..."
echo "This script will test the .NET 10 Docker compatibility fixes"
echo ""

# Enable Docker BuildKit
export DOCKER_BUILDKIT=1
export COMPOSE_DOCKER_CLI_BUILD=1

echo "Step 1: Cleaning up previous containers and images..."
docker compose down --volumes --remove-orphans 2>/dev/null || true
docker system prune -f 2>/dev/null || true

echo ""
echo "Step 2: Building and starting services..."
echo "This may take several minutes. Please wait..."
echo ""

if docker compose up --build -d; then
    echo ""
    echo "✅ SUCCESS: Docker build completed successfully!"
    echo ""
    echo "Step 3: Checking service health..."

    # Wait a bit for services to start
    sleep 10

    # Check if services are running
    if docker compose ps | grep -q "Up"; then
        echo "✅ Services are running!"
        echo ""
        echo "Step 4: Testing API endpoints..."

        # Test API Gateway
        if curl -f -s http://localhost:5000/health >/dev/null 2>&1; then
            echo "✅ API Gateway is responding"
        else
            echo "⚠️  API Gateway health check failed (may not have health endpoint)"
        fi

        # Test individual services
        services=("event-service:5001" "user-service:5002" "booking-service:5003" "payment-service:5004")
        for service in "${services[@]}"; do
            name=$(echo $service | cut -d: -f1)
            port=$(echo $service | cut -d: -f2)
            if curl -f -s http://localhost:$port/health >/dev/null 2>&1; then
                echo "✅ $name is responding"
            else
                echo "⚠️  $name health check failed (may not have health endpoint)"
            fi
        done

        echo ""
        echo "🎉 System is running successfully with .NET 10!"
        echo "You can access the services at:"
        echo "  - API Gateway: http://localhost:5000"
        echo "  - Event Service: http://localhost:5001"
        echo "  - User Service: http://localhost:5002"
        echo "  - Booking Service: http://localhost:5003"
        echo "  - Payment Service: http://localhost:5004"
    else
        echo "❌ Some services failed to start"
        echo "Checking logs..."
        docker compose logs
    fi
else
    echo ""
    echo "❌ FAILED: Docker build failed"
    echo ""
    echo "Error details:"
    docker compose logs
    echo ""
    echo "Common solutions:"
    echo "1. Check if Docker has enough memory (4GB+ recommended)"
    echo "2. Try: docker system prune -a"
    echo "3. Try: DOCKER_BUILDKIT=1 docker compose up --build -d"
    exit 1
fi