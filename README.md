# .NET Clean Architecture – Enterprise Starter

This repository provides a production-ready Clean Architecture template for .NET applications.

It is designed for real-world enterprise environments where:
- Authentication is handled by an external IAM system
- JWT access tokens are validated (no login logic in the API)
- Auditing, background jobs, and request logging are critical
- MediatR and CQRS are used for application flow
- Angular (standalone APIs) is used on the frontend

## Features
- Clean Architecture (Domain / Application / Infrastructure / API)
- JWT authentication (external IAM compatible)
- Claims & role-based authorization
- Global exception handling
- Request & audit logging
- MediatR + CQRS
- Background jobs (Hangfire)
- Soft delete & auditing
