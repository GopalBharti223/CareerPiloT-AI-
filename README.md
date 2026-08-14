# CareerPilot AI

CareerPilot AI is a career-focused web application that I built to help users manage their profiles and resumes and get AI-based feedback on their resumes.

I built this project as a hands-on .NET project to learn and practice real-world application development. While working on it, I worked with ASP.NET Core, Entity Framework Core, SQL Server, authentication, file handling, email services, and an external AI API.

The project is still under development, so I’m continuing to improve the existing features and add new ones.

---

## Features

### User & Authentication

* User registration
* Login
* OTP verification
* JWT authentication
* Forgot password
* Password reset using OTP
* Change password
* User profile management

### Resume

* Upload resumes
* Resume history
* Resume analysis
* ATS score
* AI-based resume feedback

### Other

* Email functionality
* SQL Server database integration
* Entity Framework Core migrations
* DTO-based request/response handling
* Service-based application structure

---

## Tech Stack

### Backend

* C#
* ASP.NET Core
* ASP.NET Core MVC
* Web API
* Entity Framework Core
* SQL Server
* JWT Authentication

### Frontend / UI

* HTML
* CSS
* JavaScript
* Razor Views
* Bootstrap

### External Services

* Groq API — used for AI-based resume analysis
* Gmail SMTP — used for sending emails

### Development Tools

* Visual Studio
* SQL Server Management Studio (SSMS)
* Git
* GitHub

---

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
│
├── Program.cs
├── CareerPilot AI.csproj
└── CareerPilot AI.slnx
```

---

## Current Status

**Status: In Development 🚧**

The main functionality of the application is working, and the project is currently being improved.

### Currently Working

* Registration and login
* OTP verification
* JWT authentication
* Forgot/reset password
* Profile management
* Change password
* Resume upload
* Resume history
* Resume analysis
* ATS score generation
* AI-based resume feedback
* Email functionality
* SQL Server integration
* Entity Framework Core migrations

### Currently Improving

* Resume analysis and ATS scoring
* User interface and overall user experience
* Career-related features
* Validation and error handling
* Code structure and cleanup
* Testing different application flows

---

## Setup

### Prerequisites

Before running the project, make sure you have:

* .NET SDK
* Visual Studio
* SQL Server / SQL Server Express
* SQL Server Management Studio (SSMS)

### 1. Clone the repository

```bash
git clone https://github.com/GopalBharti223/CareerPiloT-AI-.git
```

Then open the project folder.

### 2. Open the project

Open:

```text
CareerPilot AI.slnx
```

in Visual Studio.

### 3. Configure SQL Server

The application uses SQL Server.

Create a database named:

```text
CareerPilotAI
```

The connection string should be configured according to your local SQL Server instance.

For example:

```text
Server=.\SQLEXPRESS;Database=CareerPilotAI;Trusted_Connection=True;TrustServerCertificate=True;
```

### 4. Configure secrets

The project uses a few values that should **not** be stored in GitHub:

* JWT key
* Groq API key
* Email password / SMTP credentials

For local development, configure these using **ASP.NET Core User Secrets** or another local configuration method.

The actual secret values are intentionally not included in this repository.

### 5. Apply Entity Framework migrations

From the project directory:

```bash
dotnet ef database update
```

### 6. Run the application

You can run the application from Visual Studio or use:

```bash
dotnet run
```

---

## Future Improvements

Some improvements I plan to work on:

* Improve ATS scoring accuracy
* Improve AI resume analysis
* Add more detailed career suggestions
* Improve the dashboard
* Improve UI/UX
* Add better validation and error handling
* Add more career-related features
* Improve testing
* Prepare the application for deployment
* Improve production configuration

---

## What I Learned

Building CareerPilot AI gave me hands-on practice with several parts of .NET development, including:

* ASP.NET Core application development
* MVC and Web API
* C# programming
* Entity Framework Core
* SQL Server
* Database migrations
* JWT authentication
* Authorization
* DTOs
* File uploads
* Email services
* External API integration
* Service-based architecture
* Git and GitHub

This project is still a work in progress, and I’ll continue improving it as I learn more.

---

## Author

**Gopal Bharti**

GitHub: [GopalBharti223](https://github.com/GopalBharti223)

---

## Note

This project was built as a personal learning and portfolio project. Some features and implementation details may change as development continues.
