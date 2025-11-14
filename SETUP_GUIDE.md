# Quick Setup Guide

## Prerequisites
- .NET 8 SDK installed
- MySQL Server installed and running

## Step-by-Step Setup

### 1. Configure Database Connection

Edit `appsettings.json` and update the connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=PersonalDetailsDB;User=root;Password=YOUR_PASSWORD;"
  }
}
```

Replace `YOUR_PASSWORD` with your MySQL root password.

### 2. Create Database

Open MySQL command line or MySQL Workbench and run:

```sql
CREATE DATABASE PersonalDetailsDB;
```

### 3. Install EF Core Tools

```bash
dotnet tool install --global dotnet-ef
```

### 4. Navigate to Project Directory

```bash
cd C:\Users\amjath-20976\Desktop\PersonalDetailsAPI
```

### 5. Restore Dependencies

```bash
dotnet restore
```

### 6. Create and Apply Migration

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 7. Build the Project

```bash
dotnet build
```

### 8. Run the Application

```bash
dotnet run
```

### 9. Access the API

Open your browser and navigate to:
- **Swagger UI**: https://localhost:5001

## Quick Test

Once the application is running, you can test it using Swagger:

1. Go to https://localhost:5001
2. Click on `POST /api/personaldetails`
3. Click "Try it out"
4. Use this sample JSON:

```json
{
  "fullName": "Test User",
  "phoneNumber": "9876543210",
  "dateOfBirth": "1990-01-01",
  "casteGroup": "General",
  "aadharNumber": "123456789012",
  "qualification": "Graduate",
  "bloodGroup": "O+",
  "occupation": "Employed",
  "occupationDetail": "Engineer",
  "monthlyIncome": "Between25000And50000",
  "residentialStatus": "OwnHouse",
  "address": "Test Address",
  "emailId": "test@example.com",
  "fatherName": "Father Name",
  "fatherOccupation": "Business",
  "motherName": "Mother Name",
  "motherOccupation": "Homemaker",
  "numberOfWives": 1,
  "numberOfChildren": 0,
  "wifeDetails": [
    {
      "name": "Wife Name",
      "dateOfBirth": "1992-01-01",
      "occupation": "Teacher",
      "native": "City",
      "caste": "General",
      "qualification": "Graduate",
      "bloodGroup": "A+",
      "maritalStatus": "Married"
    }
  ],
  "childDetails": []
}
```

5. Click "Execute"
6. You should get a 201 Created response

## Troubleshooting

### MySQL Connection Issues

If you get connection errors:
1. Verify MySQL is running: `mysql --version`
2. Test connection: `mysql -u root -p`
3. Check if database exists: `SHOW DATABASES;`

### Build Errors

If build fails:
```bash
dotnet clean
dotnet restore
dotnet build
```

### Migration Errors

If migrations fail:
```bash
dotnet ef database drop
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Next Steps

- Read the full README.md for complete documentation
- Explore all API endpoints in Swagger UI
- Test GET, PUT, DELETE operations
- Implement authentication if needed
- Deploy to production server

## Common Commands

```bash
# Run the application
dotnet run

# Build in release mode
dotnet build -c Release

# Watch mode (auto-restart on changes)
dotnet watch run

# Run tests (if added)
dotnet test

# Create new migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update

# Rollback migration
dotnet ef database update PreviousMigrationName

# List migrations
dotnet ef migrations list

# Remove last migration
dotnet ef migrations remove
```

## API Endpoints Summary

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/personaldetails` | Get all (paginated) |
| GET | `/api/personaldetails/{id}` | Get by ID |
| POST | `/api/personaldetails` | Create new |
| PUT | `/api/personaldetails/{id}` | Update existing |
| DELETE | `/api/personaldetails/{id}` | Soft delete |

## Default Query Parameters

- `pageNumber`: 1 (default)
- `pageSize`: 10 (default)
- `searchTerm`: null (optional)

Example: `GET /api/personaldetails?pageNumber=2&pageSize=20&searchTerm=john`
