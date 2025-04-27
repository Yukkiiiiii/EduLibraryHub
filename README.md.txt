# EduLibraryHub

A demo digital school library built with ASP.NET Core 8.0 and Entity Framework Core. EduLibraryHub lets students and staff create accounts, search and filter the catalog of books, and borrow books online in a simple, intuitive interface.

## Features

- **User Accounts**  
  - Email/password authentication with ASP.NET Core Identity  
  - Role support (e.g. Admin, Student)

- **Book Search & Filter**  
  - Full-text search by title, author
  - Filter by genre and tags

- **Online Borrowing**  
  - borrow books directly from the app  
  - Track current borrows

- **Database Seeding**  
  - Sample users and books are auto-seeded on first run  

---

## 🛠️ Tech Stack

- **Backend:**  
  - ASP.NET Core 8.0 (Web API + MVC + Razor Pages)  
  - Entity Framework Core 8.0 (SQL Server & SQLite providers)  
  - ASP.NET Core Identity for authentication & roles  

- **Frontend:**  
  - Razor Views (HTML, Bootstrap CSS)  
  - JavaScript for client-side interactions  

---

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)  
- [EF Core CLI tools](https://docs.microsoft.com/ef/core/cli/dotnet)  
  ```bash
  dotnet tool install --global dotnet-ef

- To run in inside visual studio simply run the following command in the package manager console:
  ```bash
  Update-Database