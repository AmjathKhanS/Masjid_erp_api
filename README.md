# Personal Details API

A comprehensive .NET 8 Web API for managing personal details with family information including wife and children details. This API supports CRUD operations, pagination, search/filter capabilities, soft delete, and audit fields.

## Features

- **CRUD Operations**: Create, Read, Update, Delete personal details
- **Pagination**: Efficient data retrieval with pagination support
- **Search/Filter**: Search by name, phone, email, or Aadhar number
- **Soft Delete**: Records are marked as deleted, not permanently removed
- **Audit Fields**: Automatic tracking of CreatedDate, UpdatedDate, and DeletedDate
- **Data Validation**: Comprehensive validation using FluentValidation
- **Swagger Documentation**: Interactive API documentation
- **Global Error Handling**: Consistent error responses across the API
- **MySQL Database**: Entity Framework Core with MySQL

## Technology Stack

- **.NET 8.0**
- **Entity Framework Core 8.0**
- **MySQL/MariaDB** (via Pomelo.EntityFrameworkCore.MySql)
- **AutoMapper** - Object mapping
- **FluentValidation** - Input validation
- **Swagger/OpenAPI** - API documentation

## Project Structure

```
PersonalDetailsAPI/
├── Controllers/
│   └── PersonalDetailsController.cs       # API endpoints
├── Data/
│   ├── ApplicationDbContext.cs            # EF Core DbContext
│   └── MappingProfile.cs                  # AutoMapper configuration
├── Middleware/
│   └── ExceptionMiddleware.cs             # Global error handling
├── Models/
│   ├── DTOs/                              # Data Transfer Objects
│   │   ├── ApiResponse.cs
│   │   ├── PersonalDetailDto.cs
│   │   ├── CreatePersonalDetailDto.cs
│   │   ├── UpdatePersonalDetailDto.cs
│   │   ├── WifeDetailDto.cs
│   │   ├── ChildDetailDto.cs
│   │   └── PaginatedResponse.cs
│   ├── Entities/                          # Database entities
│   │   ├── BaseEntity.cs
│   │   ├── PersonalDetail.cs
│   │   ├── WifeDetail.cs
│   │   └── ChildDetail.cs
│   └── Enums/                             # Enumerations
│       ├── OccupationType.cs
│       ├── ResidentialStatus.cs
│       ├── Gender.cs
│       ├── MaritalStatus.cs
│       └── IncomeRange.cs
├── Repositories/
│   ├── IPersonalDetailRepository.cs       # Repository interface
│   └── PersonalDetailRepository.cs        # Repository implementation
├── Services/
│   ├── IPersonalDetailService.cs          # Service interface
│   └── PersonalDetailService.cs           # Service implementation
├── Validators/
│   ├── CreatePersonalDetailDtoValidator.cs
│   ├── UpdatePersonalDetailDtoValidator.cs
│   ├── WifeDetailDtoValidator.cs
│   └── ChildDetailDtoValidator.cs
├── appsettings.json                       # Configuration
└── Program.cs                             # Application entry point
```

## Prerequisites

1. **.NET 8 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
2. **MySQL Server** (8.0 or higher) or **MariaDB** (10.5 or higher)
3. **IDE** (Visual Studio 2022, VS Code, or Rider)

## Installation & Setup

### Step 1: Clone or Extract the Project

The project is located at: `C:\Users\amjath-20976\Desktop\PersonalDetailsAPI`

### Step 2: Configure MySQL Database

1. **Install MySQL/MariaDB** if not already installed

2. **Create the database**:
   ```sql
   CREATE DATABASE PersonalDetailsDB;
   ```

3. **Update Connection String** in `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Port=3306;Database=PersonalDetailsDB;User=root;Password=your_password_here;"
     }
   }
   ```

   Replace:
   - `localhost` - with your MySQL server address
   - `3306` - with your MySQL port (default is 3306)
   - `root` - with your MySQL username
   - `your_password_here` - with your MySQL password

### Step 3: Install .NET Tools (EF Core CLI)

```bash
dotnet tool install --global dotnet-ef
```

### Step 4: Create Database Migration

Navigate to the project directory and run:

```bash
cd C:\Users\amjath-20976\Desktop\PersonalDetailsAPI
dotnet ef migrations add InitialCreate
```

### Step 5: Update Database

Apply the migration to create database tables:

```bash
dotnet ef database update
```

### Step 6: Restore NuGet Packages

```bash
dotnet restore
```

### Step 7: Build the Project

```bash
dotnet build
```

### Step 8: Run the Application

```bash
dotnet run
```

The API will start and be available at:
- **HTTPS**: `https://localhost:5001`
- **HTTP**: `http://localhost:5000`
- **Swagger UI**: `https://localhost:5001` (root URL)

## API Endpoints

### Base URL
```
https://localhost:5001/api/personaldetails
```

### Endpoints Overview

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/personaldetails` | Get all personal details (with pagination & search) |
| GET | `/api/personaldetails/{id}` | Get a specific personal detail by ID |
| POST | `/api/personaldetails` | Create a new personal detail |
| PUT | `/api/personaldetails/{id}` | Update an existing personal detail |
| DELETE | `/api/personaldetails/{id}` | Delete a personal detail (soft delete) |

### Detailed Endpoint Documentation

#### 1. Get All Personal Details

```http
GET /api/personaldetails?pageNumber=1&pageSize=10&searchTerm=john
```

**Query Parameters:**
- `pageNumber` (optional, default: 1) - Page number
- `pageSize` (optional, default: 10) - Number of items per page
- `searchTerm` (optional) - Search by name, phone, email, or Aadhar

**Response:**
```json
{
  "success": true,
  "message": "Personal details retrieved successfully",
  "data": {
    "data": [...],
    "totalRecords": 100,
    "pageNumber": 1,
    "pageSize": 10,
    "totalPages": 10,
    "hasPreviousPage": false,
    "hasNextPage": true
  },
  "errors": null
}
```

#### 2. Get Personal Detail by ID

```http
GET /api/personaldetails/1
```

**Response:**
```json
{
  "success": true,
  "message": "Personal detail retrieved successfully",
  "data": {
    "id": 1,
    "fullName": "John Doe",
    "phoneNumber": "9876543210",
    "alternateNumber": "9876543211",
    "dateOfBirth": "1990-05-15T00:00:00Z",
    "casteGroup": "General",
    "aadharNumber": "123456789012",
    "qualification": "B.Tech",
    "marriageDate": "2015-06-20T00:00:00Z",
    "bloodGroup": "B+",
    "occupation": "Employed",
    "occupationDetail": "Software Engineer",
    "monthlyIncome": "Between50000And100000",
    "residentialStatus": "OwnHouse",
    "address": "123 Main St, City, State",
    "emailId": "john.doe@example.com",
    "fatherName": "James Doe",
    "fatherOccupation": "Retired",
    "fatherDeathYear": null,
    "motherName": "Jane Doe",
    "motherOccupation": "Homemaker",
    "motherDeathYear": null,
    "numberOfWives": 1,
    "numberOfChildren": 2,
    "feedback": "Everything is good",
    "wifeDetails": [...],
    "childDetails": [...],
    "createdDate": "2025-01-15T10:30:00Z",
    "updatedDate": null
  },
  "errors": null
}
```

#### 3. Create Personal Detail

```http
POST /api/personaldetails
Content-Type: application/json
```

**Request Body:**
```json
{
  "fullName": "John Doe",
  "phoneNumber": "9876543210",
  "alternateNumber": "9876543211",
  "dateOfBirth": "1990-05-15",
  "casteGroup": "General",
  "aadharNumber": "123456789012",
  "qualification": "B.Tech",
  "marriageDate": "2015-06-20",
  "bloodGroup": "B+",
  "occupation": "Employed",
  "occupationDetail": "Software Engineer",
  "monthlyIncome": "Between50000And100000",
  "residentialStatus": "OwnHouse",
  "address": "123 Main St, City, State",
  "emailId": "john.doe@example.com",
  "fatherName": "James Doe",
  "fatherOccupation": "Retired",
  "fatherDeathYear": null,
  "motherName": "Jane Doe",
  "motherOccupation": "Homemaker",
  "motherDeathYear": null,
  "numberOfWives": 1,
  "numberOfChildren": 2,
  "feedback": "Everything is good",
  "wifeDetails": [
    {
      "name": "Alice Doe",
      "dateOfBirth": "1992-08-10",
      "occupation": "Teacher",
      "native": "City Name",
      "caste": "General",
      "qualification": "M.Ed",
      "bloodGroup": "A+",
      "maritalStatus": "Married"
    }
  ],
  "childDetails": [
    {
      "name": "Child One",
      "gender": "Male",
      "dateOfBirth": "2016-03-15",
      "qualification": "5th Grade",
      "maritalStatus": "Single",
      "bloodGroup": "B+",
      "isPhysicallyChallenged": false
    },
    {
      "name": "Child Two",
      "gender": "Female",
      "dateOfBirth": "2018-07-20",
      "qualification": "3rd Grade",
      "maritalStatus": "Single",
      "bloodGroup": "A+",
      "isPhysicallyChallenged": false
    }
  ]
}
```

**Response:** (201 Created)
```json
{
  "success": true,
  "message": "Personal detail created successfully",
  "data": { ... },
  "errors": null
}
```

#### 4. Update Personal Detail

```http
PUT /api/personaldetails/1
Content-Type: application/json
```

**Request Body:** Same as Create (without ID)

**Response:** (200 OK)
```json
{
  "success": true,
  "message": "Personal detail updated successfully",
  "data": { ... },
  "errors": null
}
```

#### 5. Delete Personal Detail

```http
DELETE /api/personaldetails/1
```

**Response:** (200 OK)
```json
{
  "success": true,
  "message": "Personal detail deleted successfully",
  "data": null,
  "errors": null
}
```

## Data Model

### Enums

**OccupationType:**
- Employed
- SelfEmployed
- Unemployed
- Student
- Retired

**ResidentialStatus:**
- OwnHouse
- RentalHouse
- Other

**IncomeRange:**
- Below10000
- Between10000And25000
- Between25000And50000
- Between50000And100000
- Above100000

**Gender:**
- Male
- Female
- Other

**MaritalStatus:**
- Single
- Married
- Divorced
- Widowed

## Validation Rules

### Personal Details
- **Full Name**: Required, max 100 characters
- **Phone Number**: Required, exactly 10 digits
- **Alternate Number**: Optional, exactly 10 digits if provided
- **Date of Birth**: Required, must be in the past
- **Aadhar Number**: Required, exactly 12 digits
- **Email**: Required, valid email format, max 100 characters
- **Number of Wives**: 1-2
- **Number of Children**: 0-4
- **Wife Details**: Count must match NumberOfWives
- **Child Details**: Count must match NumberOfChildren

### Wife Details
- **Name**: Required, max 100 characters
- **Date of Birth**: Required, must be in the past
- **All other fields**: Required with appropriate length limits

### Child Details
- **Name**: Required, max 100 characters
- **Gender**: Required, valid enum value
- **Date of Birth**: Required, must be in the past
- **All other fields**: Required with appropriate length limits

## Testing with Swagger

1. Start the application
2. Navigate to `https://localhost:5001` in your browser
3. You'll see the Swagger UI with all available endpoints
4. Click on any endpoint to expand it
5. Click "Try it out" button
6. Fill in the required parameters/body
7. Click "Execute" to test the endpoint

## Testing with Postman/cURL

### cURL Example - Create Personal Detail

```bash
curl -X POST https://localhost:5001/api/personaldetails \
  -H "Content-Type: application/json" \
  -d '{
    "fullName": "John Doe",
    "phoneNumber": "9876543210",
    "dateOfBirth": "1990-05-15",
    "casteGroup": "General",
    "aadharNumber": "123456789012",
    "qualification": "B.Tech",
    "bloodGroup": "B+",
    "occupation": "Employed",
    "occupationDetail": "Software Engineer",
    "monthlyIncome": "Between50000And100000",
    "residentialStatus": "OwnHouse",
    "address": "123 Main St",
    "emailId": "john@example.com",
    "fatherName": "James Doe",
    "fatherOccupation": "Retired",
    "motherName": "Jane Doe",
    "motherOccupation": "Homemaker",
    "numberOfWives": 1,
    "numberOfChildren": 0,
    "wifeDetails": [],
    "childDetails": []
  }'
```

## Database Schema

The API automatically creates the following tables:

### PersonalDetails
- Id (Primary Key)
- FullName, PhoneNumber, AlternateNumber
- DateOfBirth, CasteGroup, AadharNumber
- Qualification, MarriageDate, BloodGroup
- Occupation, OccupationDetail, MonthlyIncome
- ResidentialStatus, Address, EmailId
- FatherName, FatherOccupation, FatherDeathYear
- MotherName, MotherOccupation, MotherDeathYear
- NumberOfWives, NumberOfChildren, Feedback
- CreatedDate, UpdatedDate, IsDeleted, DeletedDate

### WifeDetails
- Id (Primary Key)
- PersonalDetailId (Foreign Key)
- Name, DateOfBirth, Occupation
- Native, Caste, Qualification
- BloodGroup, MaritalStatus
- CreatedDate, UpdatedDate, IsDeleted, DeletedDate

### ChildDetails
- Id (Primary Key)
- PersonalDetailId (Foreign Key)
- Name, Gender, DateOfBirth
- Qualification, MaritalStatus, BloodGroup
- IsPhysicallyChallenged
- CreatedDate, UpdatedDate, IsDeleted, DeletedDate

## Common Issues & Troubleshooting

### Issue 1: Database Connection Error
**Error:** "Unable to connect to any of the specified MySQL hosts"

**Solution:**
1. Ensure MySQL service is running
2. Verify connection string in appsettings.json
3. Check MySQL username and password
4. Ensure database exists

### Issue 2: Migration Errors
**Error:** "Build failed"

**Solution:**
```bash
dotnet clean
dotnet build
dotnet ef migrations add InitialCreate
```

### Issue 3: Port Already in Use
**Error:** "Address already in use"

**Solution:**
- Change the port in `Properties/launchSettings.json`
- Or stop the process using the port

### Issue 4: Validation Errors
**Error:** 400 Bad Request with validation messages

**Solution:**
- Check the error messages in the response
- Ensure all required fields are provided
- Verify data formats (phone: 10 digits, Aadhar: 12 digits, etc.)
- Ensure NumberOfWives matches WifeDetails count
- Ensure NumberOfChildren matches ChildDetails count

## Development

### Adding New Migrations

When you modify entity models, create a new migration:

```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

### Reverting Migrations

```bash
dotnet ef migrations remove
```

### Viewing Applied Migrations

```bash
dotnet ef migrations list
```

## Production Deployment

### Configuration Changes

1. Update `appsettings.json` with production database credentials
2. Set `ASPNETCORE_ENVIRONMENT=Production`
3. Enable HTTPS with proper SSL certificates
4. Configure CORS for specific origins (not AllowAll)
5. Update logging configuration
6. Consider adding authentication/authorization

### Publishing the Application

```bash
dotnet publish -c Release -o ./publish
```

## Security Considerations

1. **Connection Strings**: Never commit passwords to source control
2. **API Keys**: Use environment variables or Azure Key Vault
3. **CORS**: Restrict to specific domains in production
4. **Input Validation**: FluentValidation is already configured
5. **SQL Injection**: EF Core parameterizes queries automatically
6. **Authentication**: Consider adding JWT authentication for production

## Support & Contact

For issues or questions:
- Email: support@example.com
- Documentation: This README file
- Swagger UI: Available at root URL when running the application

## License

[Specify your license here]

## Version History

- **v1.0.0** (2025-01-15)
  - Initial release
  - CRUD operations
  - Pagination and search
  - Soft delete
  - Audit fields
  - FluentValidation
  - Swagger documentation
