# TTManagement System

A comprehensive Task and Team Management System built with .NET 8 Web API. This application allows organizations to manage users, teams, and tasks with role-based access control (Admin, Manager, Employee) and task workflows.

## 🚀 Features

*   **User Management**: Create, update, and manage users with specific roles.
*   **Team Management**: Organize users into teams.
*   **Task Management**: Create tasks, assign them to users/teams, set due dates, and track status (Todo, InProgress, Done).
*   **Advanced Search**: Filter tasks by status, assignee, team, due date, and sort results.
*   **Authentication & Authorization**: Secure JWT-based authentication with Role-Based Access Control (RBAC).
*   **Validation**: Robust request validation using FluentValidation.
*   **Logging**: Centralized structured logging with Serilog (Console & File).
*   **Documentation**: Interactive API documentation with Swagger UI.

## 🛠️ Tech Stack

*   **Framework**: .NET 8 Web API (LTS)
*   **Database**: SQL Server 2022 (via Docker)
*   **ORM**: Entity Framework Core 8
*   **Architecture**: CQRS (Command Query Responsibility Segregation) with MediatR
*   **Validation**: FluentValidation
*   **Mapping**: AutoMapper
*   **Logging**: Serilog
*   **Testing**: MSTest & Moq

## 📋 Prerequisites

*   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
*   [Docker Desktop](https://www.docker.com/products/docker-desktop)

## 🏁 Getting Started

### 1. Start the Database
Run the following command to start a SQL Server container in Docker:

```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Passw0rd" -p 1433:1433 --name ttmanagement-sql -d mcr.microsoft.com/mssql/server:2022-latest
```

### 2. Configure the Application
The application is pre-configured for local development in `appsettings.Development.json`.
*   **Database**: Connects to `localhost,1433` with the credentials set above.
*   **Logging**: Logs are saved to the `Logs/` directory.

### 3. Apply Migrations
Initialize the database and seed default data:

```bash
dotnet ef database update
```

### 4. Run the Application
Start the API:

```bash
dotnet run
```

The application will start on **http://localhost:5026**.

## 📖 API Documentation

Access the Swagger UI to explore and test endpoints interactively:

👉 **http://localhost:5026/swagger**

### 🔐 Authentication Flow
1.  Go to `POST /api/Auth/login` in Swagger.
2.  Use one of the **Default Credentials** below to get a JWT `token`.
3.  Click the **Authorize** button (green lock icon) at the top.
4.  Enter: `Bearer <YOUR_TOKEN>`
5.  Click **Authorize**. Now you can access protected endpoints.

## 👤 Default Credentials

The database is seeded with the following users:

| Role | Email | Password | Permissions |
| :--- | :--- | :--- | :--- |
| **Admin** | `admin@demo.com` | `Admin123!` | Full access to Users, Teams, and Tasks. |
| **Manager** | `manager@demo.com` | `Manager123!` | Can create/edit Tasks. View Users/Teams. |
| **Employee** | `employee@demo.com` | `Employee123!` | View assigned tasks. Update task status. |

## 🧪 Running Tests

Execute the unit test suite:

```bash
dotnet test
```
