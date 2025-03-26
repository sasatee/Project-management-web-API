# Project Management Web API

A robust ASP.NET Core Web API for project management with authentication, authorization, and comprehensive HR management features.

## Features

- 🔐 **Authentication & Authorization**
  - JWT-based authentication
  - Role-based authorization (Admin, User, Employee)
  - Google OAuth integration
  - Password reset functionality
  - Email verification

- 👥 **User Management**
  - User registration
  - User login
  - Password management
  - User profile management
  - Role assignment

- 🔑 **Security Features**
  - Secure password hashing
  - JWT token authentication
  - Role-based access control
  - Email verification
  - Password reset via email

- 👨‍💼 **HR Management**
  - Employee management
  - Department management
  - Job title management
  - Leave management
    - Leave types
    - Leave requests
    - Leave allocations
  - Payroll management
  - Category group management

## Prerequisites

- .NET 7.0 or later
- SQL Server
- Google Cloud Platform account (for Google OAuth)
- Docker (optional, for containerized deployment)

## Configuration

1. Update the `appsettings.json` with your configuration:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your_connection_string"
  },
  "JWTSetting": {
    "securityKey": "your_security_key",
    "ValidIssuer": "your_issuer",
    "ValidAudience": "your_audience"
  },
  "EmailSetting": {
    "ResetUrl": "your_reset_url",
    "password": "your_email_password",
    "supportEmail": "your_support_email"
  },
  "Authentication": {
    "Google": {
      "ClientId": "your_google_client_id",
      "ClientSecret": "your_google_client_secret"
    }
  }
}
```

2. Set up Google OAuth:
   - Create a project in Google Cloud Console
   - Configure OAuth 2.0 credentials
   - Add authorized redirect URIs
   - Update the ClientId and ClientSecret in appsettings.json

## Installation

### Local Development

1. Clone the repository:
```bash
git clone https://github.com/sasatee/Project-management-web-API.git
```

2. Navigate to the project directory:
```bash
cd Project-management-web-API
```

3. Restore dependencies:
```bash
dotnet restore
```

4. Apply database migrations:
```bash
dotnet ef database update
```

5. Run the application:
```bash
dotnet run
```

### Docker Deployment

1. Build the Docker image:
```bash
docker build -t project-management-api .
```

2. Run the container:
```bash
docker run -d -p 5000:80 --name project-management-api project-management-api
```

## API Endpoints

### Authentication
- `POST /api/account/register` - Register a new user
- `POST /api/account/login` - Login with email and password
- `GET /api/account/google-login` - Initiate Google OAuth login
- `GET /api/account/google-response` - Handle Google OAuth callback
- `POST /api/account/forgot-password` - Request password reset
- `POST /api/account/reset-password` - Reset password with token
- `GET /api/account/detail` - Get current user details
- `GET /api/account/details` - Get all users (Admin only)
- `POST /api/account/change-password` - Change user password

### Employee Management
- `GET /api/employee` - Get all employees
- `GET /api/employee/{id}` - Get employee by ID
- `POST /api/employee` - Create new employee
- `PUT /api/employee/{id}` - Update employee
- `DELETE /api/employee/{id}` - Delete employee

### Department Management
- `GET /api/department` - Get all departments
- `GET /api/department/{id}` - Get department by ID
- `POST /api/department` - Create new department
- `PUT /api/department/{id}` - Update department
- `DELETE /api/department/{id}` - Delete department

### Leave Management
- `GET /api/leave-type` - Get all leave types
- `GET /api/leave-request` - Get all leave requests
- `GET /api/leave-allocation` - Get all leave allocations
- `POST /api/leave-request` - Create new leave request
- `PUT /api/leave-request/{id}` - Update leave request
- `POST /api/leave-allocation` - Create leave allocation
- `PUT /api/leave-allocation/{id}` - Update leave allocation

### Job Title Management
- `GET /api/job-title` - Get all job titles
- `GET /api/job-title/{id}` - Get job title by ID
- `POST /api/job-title` - Create new job title
- `PUT /api/job-title/{id}` - Update job title
- `DELETE /api/job-title/{id}` - Delete job title

### Payroll Management
- `GET /api/payroll` - Get all payroll records
- `GET /api/payroll/{id}` - Get payroll by ID
- `POST /api/payroll` - Create new payroll record
- `PUT /api/payroll/{id}` - Update payroll record
- `DELETE /api/payroll/{id}` - Delete payroll record

### Category Group Management
- `GET /api/category-group` - Get all category groups
- `GET /api/category-group/{id}` - Get category group by ID
- `POST /api/category-group` - Create new category group
- `PUT /api/category-group/{id}` - Update category group
- `DELETE /api/category-group/{id}` - Delete category group

## Database Schema

The application uses Entity Framework Core with the following main entities:
- `AppUser` - User information and authentication
- `IdentityRole` - Role management
- `IdentityUserRole` - User-Role relationships
- `Employee` - Employee information
- `Department` - Department information
- `JobTitle` - Job title information
- `LeaveType` - Leave type definitions
- `LeaveRequest` - Leave request records
- `LeaveAllocation` - Leave allocation records
- `Payroll` - Payroll information
- `CategoryGroup` - Category group information

## Repository Pattern

The application implements the Repository pattern for data access:
- `IRepository<T>` - Generic repository interface
- `Repository<T>` - Generic repository implementation
- Specific repositories for each entity:
  - `EmployeeRepository`
  - `LeaveRequestRepository`
  - `LeaveAllocationRepository`
  - `LeaveTypeRepository`

## Security Considerations

- All sensitive data is stored securely
- Passwords are hashed using ASP.NET Core Identity
- JWT tokens are used for authentication
- Role-based authorization is implemented
- Google OAuth integration follows security best practices
- API endpoints are protected with appropriate authorization attributes

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- ASP.NET Core Identity
- Entity Framework Core
- JWT Authentication
- Google OAuth
- Docker
- Repository Pattern 