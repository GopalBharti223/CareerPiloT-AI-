# CareerPilot AI

CareerPilot AI is a career assistance web application I built using ASP.NET Core.

The main idea behind the project is to help users manage their career-related information and get useful feedback on their resumes. Users can create an account, manage their profile, upload resumes, and get an AI-based resume analysis.

I built this project mainly to get hands-on experience with ASP.NET Core, Web API development, Entity Framework Core, SQL Server, authentication, and working with an AI API.

## Features

* User registration and login
* OTP verification
* Password reset
* JWT-based authentication
* User profile management
* Change password
* Resume upload
* Resume history
* Resume analysis
* ATS score
* AI-generated resume feedback
* Email notifications
* SQL Server database
* Entity Framework Core migrations

## Tech Stack

### Backend

* C#
* ASP.NET Core
* ASP.NET Core MVC / Web API
* Entity Framework Core
* SQL Server
* JWT Authentication

### Other

* Groq API for AI-based resume analysis
* SMTP / Gmail for email functionality
* Bootstrap
* HTML / CSS / JavaScript

## Project Structure

```text
CareerPilot AI
│
├── Controllers
├── DTOs
├── Data
├── Migrations
├── Models
├── Services
├── Views
├── wwwroot
├── Program.cs
└── CareerPilot AI.csproj
```

## How to Run the Project

### 1. Clone the repository

```bash
git clone https://github.com/GopalBharti223/CareerPiloT-AI-.git
```

### 2. Open the project

Open `CareerPilot AI.slnx` in Visual Studio.

### 3. Configure SQL Server

The project uses SQL Server.

Create a database named:

```text
CareerPilotAI
```

Update the connection string according to your local SQL Server setup.

### 4. Configure application secrets

The project requires configuration for:

* JWT key
* Groq API key
* Email account / SMTP password

These values are intentionally not included in this repository.

For local development, add them using ASP.NET Core User Secrets or your preferred local configuration method.

### 5. Apply migrations

Run:

```bash
dotnet ef database update
```

### 6. Run the project

Run the project from Visual Studio or use:

```bash
dotnet run
```

## Notes

This is a personal learning/project application, and I am continuing to improve it as I learn more about .NET development.

Some parts of the project may change as new features and improvements are added.

## What I Learned

While building CareerPilot AI, I got practical experience with:

* Building ASP.NET Core applications
* Creating APIs and controllers
* Working with Entity Framework Core
* Designing database relationships
* Creating and applying migrations
* JWT authentication and authorization
* DTOs and model validation
* File uploads
* Sending emails from an application
* Integrating an external AI API
* Organizing services and application logic
* Using Git and GitHub for version control
