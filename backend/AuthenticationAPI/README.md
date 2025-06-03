# Project Management Web API

A robust, scalable, and secure RESTful API for managing employees, payroll, leave requests, and organizational roles. Built with ASP.NET Core 9, Entity Framework Core, and JWT authentication, this API is designed for HR and project management systems.

## Features

- **User Authentication & Authorization**
  - Register, login, and manage users with JWT-based authentication.
  - Google OAuth integration.
  - Role-based access control (Admin, Employee, etc.).

- **Employee Management**
  - CRUD operations for employees, departments, and job titles.
  - Assign roles and manage employee details.

- **Leave Management**
  - Submit, approve, and track leave requests.
  - Allocate leave types and periods.
  - View personal and all leave records.

- **Payroll Management**
  - Calculate payroll dynamically based on salary scales, allowances, and deductions.
  - Manage salary progressions and steps.
  - Generate payroll records for employees.

- **Training & Performance**
  - Track employee training and performance reviews.

- **Notifications & Audit Logs**
  - Send notifications to employees.
  - Maintain audit logs for key actions.

- **API Documentation**
  - Swagger/OpenAPI and Scalar UI for interactive API exploration.

## Technologies Used

- **Backend:** ASP.NET Core 9
- **ORM:** Entity Framework Core (PostgreSQL, SQL Server, SQLite)
- **Authentication:** JWT, Google OAuth
- **API Docs:** Swagger, Scalar.AspNetCore
- **Other:** Microsoft Identity, Npgsql, Newtonsoft.Json

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- PostgreSQL or SQL Server (or use SQLite for local development)

### Setup

1. **Clone the repository:**
   ```bash
   git clone <your-repo-url>
   cd Project-management-web-API/backend/AuthenticationAPI
   ```

2. **Configure the database:**
   - Update the `appsettings.json` with your database connection string under `ConnectionStrings`.
   - By default, the project supports PostgreSQL (`NeonConnection`), SQL Server (`DefaultConnection`), and SQLite (uncomment in `Program.cs` if needed).

3. **Set environment variables (optional for secrets):**
   - JWT settings, email credentials, and Google OAuth client details are in `appsettings.json`. For production, use environment variables or [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets).

4. **Run database migrations:**
   ```bash
   dotnet ef database update
   ```

5. **Run the API:**
   ```bash
   dotnet run
   ```
   The API will be available at `http://localhost:4500` (see `launchSettings.json`).

6. **Explore the API:**
   - Swagger UI: `http://localhost:4500/swagger`
   - Scalar UI: `http://localhost:4500/scalar/v1`

### Example API Endpoints

- `POST /api/account/register` – Register a new user
- `POST /api/account/login` – Login and receive JWT
- `GET /api/employee/employees` – List all employees
- `POST /api/employee/create-a-employee` – Create a new employee
- `GET /api/leaverequests` – List all leave requests
- `POST /api/leaverequests` – Submit a leave request
- `GET /api/payroll/calculate-dynamic/{employeeId}` – Calculate payroll for an employee

### Configuration

Key settings in `appsettings.json`:
- `JWTSetting`: Security key, issuer, audience, expiry
- `ConnectionStrings`: Database connection strings
- `EmailSetting`: SMTP credentials for password reset
- `Authentication:Google`: Google OAuth client ID/secret

---



