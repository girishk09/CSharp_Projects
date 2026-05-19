# Library Management System

A full-stack web application for managing books, authors, and library members. Built with ASP.NET Core Web API on the backend and Angular on the frontend.

---

## Features

- Add, view, update, and delete books
- Add, view, update, and delete authors
- Manage library members
- Secure REST API endpoints with JWT authentication
- Role-based access control (Admin and User roles)
- Paginated data retrieval for efficient browsing

---

## Tech Stack

| Layer | Technology |
|---|---|
| Backend | ASP.NET Core Web API, C# |
| Frontend | Angular, Bootstrap |
| Database | SQL Server |
| ORM | Entity Framework Core |
| Auth | JWT (JSON Web Tokens) |

---

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (v6 or above)
- [Node.js and npm](https://nodejs.org/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or VS Code

---

### Backend Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/girishk09/CSharp_Projects.git
   cd CSharp_Projects
   ```

2. Open the `Library_Backend` project in Visual Studio 2022.

3. Apply Entity Framework Core migrations to set up the database (first time only). Open the Package Manager Console in Visual Studio and run:
   ```
   Add-Migration InitialCreate
   Update-Database
   ```

4. Run the backend using the Debug button in Visual Studio (or press `F5`).

The API will start at the port configured in your launch settings.

---

### Frontend Setup

1. Navigate to the frontend folder:
   ```bash
   cd Library_UI
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Start the Angular development server:
   ```bash
   ng serve
   ```

4. Open your browser and go to `http://localhost:4200`

---

## Project Structure

```
CSharp_Projects/
├── Library_Backend/
│   ├── Controllers/
│   ├── Models/
│   ├── Data/
│   └── appsettings.json
├── Library_UI/
│   ├── src/
│   │   ├── app/
│   │   └── environments/
│   └── package.json
└── Library_UnitTests/
```

---

## Author

Kapu Girish Kumar Reddy  
[GitHub](https://github.com/girishk09) · [LinkedIn](https://linkedin.com/in/girishreddyk)
