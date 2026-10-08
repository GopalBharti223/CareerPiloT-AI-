# CareerPilot AI 🚀

CareerPilot AI is an AI-powered career assistance web application that I built to help users manage their profiles, analyze resumes, and evaluate their compatibility with job descriptions.

The project was built as a hands-on full-stack .NET application to practice real-world development concepts including ASP.NET Core Web API, C#, Entity Framework Core, PostgreSQL, JWT authentication, file handling, email services, React, and external AI API integration.

The application is deployed and available online.

---

## 🌐 Live Demo & Project Links

| Resource            | Link                                                   |
| ------------------- | ------------------------------------------------------ |
| 🚀 Live Application | https://careerpilot-frontend-o977.onrender.com         |
| 📚 Swagger API      | https://careerpilot-ai-spdz.onrender.com/swagger       |
| 💻 Frontend GitHub  | https://github.com/GopalBharti223/CareerPilot-Frontend |
| ⚙️ Backend GitHub   | https://github.com/GopalBharti223/CareerPiloT-AI-      |

---

## ✨ Features

### 🔐 Authentication & User Management

* User registration
* User login
* JWT authentication
* OTP/email verification
* Forgot password
* Password reset using OTP
* Password hashing
* Protected API endpoints
* User profile management
* Change password
* Authorization and user ownership checks

### 📄 Resume Management

* Upload PDF resumes
* PDF file validation
* Resume history
* Resume analysis
* Resume deletion
* User-specific resume access
* ATS score generation
* AI-powered resume feedback

### 🤖 AI Resume Analysis

CareerPilot AI uses an external AI API to analyze uploaded resumes.

The analysis provides:

* ATS score
* Resume summary
* Detected skills
* Missing skills
* Improvement suggestions
* AI-generated feedback

The resume analysis flow is:

```text
Resume Upload
      ↓
PDF Validation
      ↓
PDF Text Extraction
      ↓
AI Analysis
      ↓
JSON Response
      ↓
Database Storage
      ↓
Frontend Result
```

### 🎯 Job Matching

Users can compare their resume against a specific job description.

The Job Matching feature accepts:

* Resume
* Job title
* Company name
* Job description

It then provides:

* Job match score
* Matched skills
* Missing skills
* Matched keywords
* Missing keywords
* Experience match
* Suggestions for improvement

Job matching history is also stored and associated with the authenticated user.

### 📧 Email & Password Services

* OTP email verification
* Password reset emails
* Gmail SMTP integration

---

## 🏗️ Architecture

CareerPilot AI follows a frontend-backend architecture:

```text
                    CareerPilot AI
                         │
          ┌──────────────┴──────────────┐
          │                             │
     React Frontend               ASP.NET Core API
          │                             │
          │                       Service Layer
          │                             │
          │                       Entity Framework
          │                             │
          │                       PostgreSQL / Neon
          │
          └────────────── API ───────────┘
                         │
                    Groq AI API
```

### Backend Flow

```text
Controller
    ↓
DTO
    ↓
Service Layer
    ↓
Entity Framework Core
    ↓
PostgreSQL
```

For AI-powered features:

```text
Controller
    ↓
Service
    ↓
PDF / Job Description Processing
    ↓
Groq API
    ↓
JSON Response
    ↓
DTO / Model
    ↓
Database
    ↓
API Response
```

---

## 🛠️ Tech Stack

### Backend

* C#
* ASP.NET Core Web API
* Entity Framework Core
* PostgreSQL
* Npgsql
* JWT Authentication
* DTOs
* Dependency Injection
* Service-based architecture
* Swagger / OpenAPI

### Frontend

* React
* Vite
* JavaScript
* HTML
* CSS

### AI & External Services

* Groq API — AI-powered resume and job matching analysis
* Gmail SMTP — email and OTP functionality
* PdfPig — PDF text extraction

### Database & Deployment

* PostgreSQL
* Neon — cloud PostgreSQL database
* Render — frontend and backend deployment
* Git
* GitHub

---

## 🔒 Security & Validation

The application implements several security and validation mechanisms:

* JWT-based authentication
* `[Authorize]` protected endpoints
* User ID extraction from JWT claims
* Resume ownership validation
* Job matching ownership validation
* Password hashing
* Required-field validation
* Email validation
* Password validation
* PDF extension validation
* PDF magic-byte validation
* File size validation
* Protected user-specific history
* Secrets kept outside the source code

---

## 🗄️ Database

CareerPilot AI uses PostgreSQL with Entity Framework Core.

The main entities include:

```text
Users
  │
  └── Resumes
        │
        └── ResumeAnalyses

Users
  │
  └── JobMatchingHistories
```

The application uses EF Core for database access and migrations.

---

## 📁 Backend Structure

The backend follows a controller/service/DTO-based structure.

```text
CareerPilot AI Backend
│
├── Controllers
│   ├── AuthController
│   ├── ProfileController
│   ├── ResumeController
│   └── JobMatchingController
│
├── DTOs
│
├── Models
│
├── Data
│
├── Services
│   ├── AuthService
│   ├── ProfileService
│   ├── PasswordService
│   ├── ResumeService
│   ├── ResumeAnalyzerService
│   └── JobMatchingService
│
├── Migrations
│
├── Program.cs
└── CareerPilot AI.csproj
```

---

## 🎨 Frontend

The frontend is a separate React/Vite application.

It communicates with the ASP.NET Core backend using REST APIs.

The frontend includes functionality for:

* Registration
* Login
* Profile management
* Resume upload
* Resume history
* Resume analysis
* Job matching
* Job matching history
* Password recovery

Authentication is handled using JWT tokens stored on the client side, with protected API requests automatically including the bearer token.

---

## ☁️ Deployment

The current deployment architecture is:

```text
React / Vite
     │
     ▼
   Render
     │
     │ REST API
     ▼
ASP.NET Core Web API
     │
     ├──────────► Groq API
     │
     └──────────► Neon PostgreSQL
```

### Deployment Services

* **Frontend:** Render
* **Backend:** Render
* **Database:** Neon PostgreSQL
* **Source Control:** GitHub
* **API Documentation:** Swagger / OpenAPI

---

## 🧪 Testing & Development

During development, I tested the application through:

* Swagger
* Browser-based frontend testing
* Authentication and authorization flows
* Resume upload and analysis
* Resume history
* User ownership checks
* Job matching
* Job matching history
* HTTP status code validation
* Database operations
* Deployment testing on Render

I also debugged issues involving:

* CORS
* JWT authentication
* API authorization
* PDF file handling
* AI JSON responses
* PostgreSQL `DateTime` handling
* Render deployment behavior

---

## 🚀 Current Status

**Status: Completed Portfolio Project**

The main CareerPilot AI functionality has been implemented and deployed.

Currently available:

* Authentication
* OTP verification
* Password recovery
* Profile management
* Resume upload
* Resume analysis
* ATS scoring
* AI resume feedback
* Resume history
* Resume deletion
* Job matching
* Job matching history
* JWT authorization
* PostgreSQL database integration
* React frontend
* Swagger API
* Cloud deployment

The project may receive improvements in the future, but the current version represents the completed portfolio implementation.

---

## 📚 What I Learned

Building CareerPilot AI gave me practical experience with:

* ASP.NET Core Web API
* C#
* REST API development
* Entity Framework Core
* PostgreSQL
* JWT authentication
* Authorization
* DTO-based API design
* Dependency Injection
* Service-layer architecture
* File upload and validation
* PDF processing
* Email services
* External API integration
* AI API integration
* React and Vite
* Git and GitHub
* Render deployment
* Neon PostgreSQL
* API debugging
* Error handling
* Application security

---

## 👨‍💻 Author

**Gopal Bharti**

GitHub:
https://github.com/GopalBharti223

---

## 📌 Note

CareerPilot AI is a personal learning and portfolio project built to gain practical experience in full-stack .NET and AI-powered application development.

The application is intended as a portfolio/demo project rather than a production SaaS platform.
