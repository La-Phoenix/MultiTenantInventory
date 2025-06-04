# MultiTenantInventory

A production-grade, multi-tenant backend system built with ASP.NET Core, PostgreSQL, Docker, and Clean Architecture. Includes support for domain-driven design, integration testing, and scalable patterns for SaaS applications.

---

## ?? Architecture

This project follows **Clean Architecture** with a strict separation of concerns:

MultiTenantInventory/
??? src/
? ??? MultiTenantInventory.API/ # ASP.NET Core Web API
? ??? MultiTenantInventory.Application/ # Application Layer (Use cases, DTOs, validation)
? ??? MultiTenantInventory.Domain/ # Domain Layer (Entities, interfaces)
? ??? MultiTenantInventory.Infrastructure/# EF Core, PostgreSQL, external dependencies
??? tests/
? ??? MultiTenantInventory.UnitTests/ # Unit tests (e.g., services, validators)
? ??? MultiTenantInventory.IntegrationTests/ # Integration tests with Testcontainers
??? docker-compose.yml # PostgreSQL and App containerization
??? MultiTenantInventory.sln


---

## ??? Technologies Used

- **.NET 8**
- **PostgreSQL** with **EF Core**
- **MediatR** for CQRS and decoupling
- **FluentValidation** for input validation
- **Docker** + `docker-compose`
- **Testcontainers** + **xUnit** for integration testing
- **Serilog** for structured logging
- **Clean Architecture** best practices

---