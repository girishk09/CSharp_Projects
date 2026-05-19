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
   git clone https://github.com/yourusername/library-management-system.git
   cd library-management-system
   ```

2. Open the backend project in Visual Studio 2022.

3. Update the connection string in `appsettings.json` with your SQL Server details:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=LibraryDB;Trusted_Connection=True;"
   }
   ```

4. Apply Entity Framework Core migrations to set up the database:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

5. Run the backend:
   ```bash
   dotnet run
   ```

The API will start at `https://localhost:7000` (or the port configured in your launch settings).

---

### Frontend Setup

1. Navigate to the frontend folder:
   ```bash
   cd frontend
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
library-management-system/
├── backend/
│   ├── Controllers/
│   ├── Models/
│   ├── Data/
│   └── appsettings.json
└── frontend/
    ├── src/
    │   ├── app/
    │   └── environments/
    └── package.json
```

---

## Author

Kapu Girish Kumar Reddy  
[GitHub](https://github.com/girishk09) · [LinkedIn](https://linkedin.com/in/girishreddyk)
