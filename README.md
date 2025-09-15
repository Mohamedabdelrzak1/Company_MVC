# Company_MVC

A full-stack ASP.NET MVC web application built with Entity Framework Core and 3-Tier Architecture, following Clean Architecture principles. The project uses the Repository Pattern and Unit of Work for maintainability, with ASP.NET Identity for authentication and authorization.

🚀 Features

User Authentication & Authorization: Secure login with ASP.NET Identity and role-based access control.

Password Recovery: SMTP email integration for password reset.

Company Management: CRUD operations for managing company data.

Responsive UI: Modern Razor views with Bootstrap.

Validation: Client-side validation with jQuery and server-side data annotations.

Scalable Architecture: 3-Tier structure (Presentation, Business Logic, Data Access) with Dependency Injection.

DTO & Mapping: AutoMapper for clean data transfer between layers.

🛠️ Tech Stack

ASP.NET MVC  .NET 8 

Entity Framework Core

ASP.NET Identity

AutoMapper

SQL Server

Bootstrap & jQuery

📂 Project Architecture

Presentation Layer: ASP.NET MVC (Views, Controllers)

Business Layer: Services, DTOs, AutoMapper profiles

Data Layer: EF Core, Repository Pattern, Unit of Work

🔑 Authentication & Security

Role-based access (Admin / User).

Secure password hashing (ASP.NET Identity).

Email-based password recovery via SMTP.

📸 Screenshots (Optional)

Add UI screenshots here if available.

ج
📊 Future Enhancements

Advanced dashboard with charts/analytics.

Two-factor authentication.

Docker support for containerized deployment.

🤝 Contributing

Pull requests are welcome. For major changes, open an issue first to discuss what you’d like to add.

🛡 License

This project is licensed under the MIT License.
