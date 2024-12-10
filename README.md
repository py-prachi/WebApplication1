WebApplication1 is a demo ASP.NET Core MVC application designed to showcase the MVC structure along with its unit testing and coverage reporting.

## Overview
This application implements two primary flows:

#### Item Management: 
Includes an ItemController that interacts directly with an MS SQL Server database.

#### Order Management
Includes an OrderController and a dedicated service layer to handle business logic.
The project demonstrates unit testing practices, including:

Using an In-Memory Database for testing data interactions.
Mocking Services to isolate and test specific layers.
Additionally, code coverage is assessed using Coverlet and ReportGenerator, generating an HTML file for coverage visualization.

## Features

#### Item Flow

CRUD operations handled by ItemController.
Database interactions using MS SQL Server for production and In-Memory Database for tests.
#### Order Flow

Includes an OrderController that communicates with an OrderService for business logic.
Tests for OrderController mock the OrderService.
Tests for OrderService use the In-Memory Database.

#### Code Coverage Reporting
Coverage is calculated using Coverlet.
An HTML report is generated with ReportGenerator.

#### Tech Stack
    Framework: ASP.NET Core 6.0+ / .NET 9.0
    Database: MS SQL Server (production) and In-Memory Database (tests)
Testing: xUnit, Moq, Coverlet, ReportGenerator
Tools: Rider, DBeaver, Command-line tools for testing and coverage
Getting Started
Follow these steps to set up and run the project locally.

**Prerequisites**
.NET SDK (version 6.0 or higher)
SQL Server
Rider or Visual Studio
ReportGenerator

**Installation**

**Clone the Repository:**

git clone  https://github.com/py-prachi/WebApplication1.git

cd WebApplication1

**Restore NuGet Packages:**

dotnet restore

**Set Up the Database:**

Update the connection string in appsettings.json to point to your MS SQL Server instance.

**Run migrations:**

dotnet ef database update

**Run the Application:**

dotnet run
The application will be available at http://localhost:5214

**Running Tests**
To execute the unit tests:
dotnet test --collect:"XPlat Code Coverage"

**Generating Code Coverage Report**
Use Coverlet to collect coverage data:


dotnet test --collect:"XPlat Code Coverage"
Generate an HTML coverage report using ReportGenerator:


reportgenerator -reports:"./TestResults/*/coverage.cobertura.xml" -targetdir:"coverageresults" -reporttypes:Html
The report will be available in the coverageresults directory.

**Directory Structure**

WebApplication1/
├── WebApplication1/
│   ├── Controllers/         # MVC Controllers (e.g., ItemController, OrderController)
│   ├── Services/            # Service layer (e.g., OrderService)
│   ├── Models/              # Data models
│   ├── Views/               # Razor Views
│   └── appsettings.json     # Application configuration
├── WebApplication1.Tests/
│   ├── Controllers/         # Unit tests for Controllers
│   │   ├── ItemsControllerTest.cs
│   │   └── OrderControllerTest.cs
│   ├── Services/            # Unit tests for Services
│   ├── TestResults/         # Test results including coverage
│   └── coverageResult/      # Generated coverage reports
└── README.md                # Project documentation


