# Scuttle

A modern discussion application that takes inspiration from (Reddit)[https://reddit.com]. Built with C# and .NET 9, following Clean Architecture principles and Test-Driven Development.

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4)
![C#](https://img.shields.io/badge/C%23-12.0-239120)
![EF Core](https://img.shields.io/badge/EF_Core-9.0-purple)

## 🚀 Features

- **User Authentication**: Secure JWT-based authentication
- **Posts and Discussions**: Create, view, and interact with posts
- **Communities**: Join and participate in topic-focused communities
- **Voting System**: Upvote and downvote content
- **Comments**: Nested comment threads for rich discussions

## 🛠️ Tech Stack

### Backend
- **Language**: C# 12
- **Framework**: .NET 9
- **Architecture**: Clean Architecture
- **Data**: Entity Framework, SQL Server
- **Authentication**: JWT (JSON Web Token)
- **Patterns**: CQRS (MediatR)

### Development Approach
- **Test-Driven Development**: xUnit for testing
- **API Documentation**: OpenAPI
- **Dependency Injection**: .NET DI container

## 📐 Architecture

Scuttle follows Clean Architecture principles with clear seperation of concerns:

```
Scuttle.sln
├── src/
│   ├── Scuttle.Domain/                  # Core business logic and entities
│   ├── Scuttle.Application/             # Application logic and use cases
│   ├── Scuttle.Infrastructure/          # External dependencies implementation
│   └── Scuttle.API/                     # Web API presentation layer
├── tests/
│   ├── Scuttle.Domain.Tests/            # Domain layer unit tests
│   ├── Scuttle.Application.Tests/       # Application layer unit tests
│   ├── Scuttle.Infrastructure.Tests/    # Infrastructure layer unit tests
│   └── Scuttle.API.Tests/               # API layer tests
└── docs/                                # Documentation
```

### Benefits
- **Maintainability**: Clear seperation of concerns makes code easier to maintain
- **Testability**: Domain logic is isolated and easily testable
- **Flexibility**: Infrastructure details can cahnge without affecting business logic
- **Scalability**: Components can be scaled independently

## 🔄 CQRS with MediatR

Scuttle uses the Command Query Responsibility Segregation pattern with MediatR:

- **Commands**: Represent operations that change state
- **Queries**: Represent operations that return data without changing state
- **Handlers**: Process commands and queries
- **Notifications**: Enable pub/sub 

## 🚀 Getting Started

### Prerequisites

- .NET 9 SDK
- SQL Server (or any EF Core compatible database)
- IDE

### Installation
1. Clone the repo:
```bash
git clone https://github.com/scrapcode/scuttle.git
cd scuttle
```
2. Restore packages: `dotnet restore`
3. Update database connection string in `appsettings.json`
4. Apply migrations:
```bash
cd src/Scuttle.API
dotnet ef database update
```
5. Run: `dotnet run`