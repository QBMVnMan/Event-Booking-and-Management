# Multi-stage Dockerfile for Angular app
# Build stage
FROM node:18-alpine AS build
WORKDIR /app
COPY package.json package-lock.json* ./
COPY angular.json tsconfig.* ./
COPY src ./src
COPY public ./public

RUN npm ci --silent
RUN npm run build -- --configuration production

# Production stage - serve with nginx
FROM nginx:stable-alpine AS production
COPY --from=build /app/dist/event_booking_and_management.client /usr/share/nginx/html
COPY nginx/default.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
