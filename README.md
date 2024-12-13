
# **User Management API**

![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-7.0-blue) ![EF Core](https://img.shields.io/badge/Entity%20Framework-Core-orange) ![Serilog](https://img.shields.io/badge/Logging-Serilog-green) ![Swagger](https://img.shields.io/badge/API%20Docs-Swagger-brightgreen)

A robust ASP.NET Core Web API designed to manage user records with advanced middleware features for logging, error handling, and rate limiting. This project is ideal for learning or deploying simple yet effective user management solutions.

---

## **Features**
- **CRUD Operations**:
  - Create, Read, Update, and Delete users.
- **Custom Middleware**:
  - **Logging Middleware**: Logs HTTP requests and responses.
  - **Error Handling Middleware**: Provides consistent JSON responses for errors.
  - **Rate Limiting Middleware**: Controls excessive requests.
- **Token-based Authentication** *(optional)*:
  - Secures API endpoints for authorized users.
- **In-memory Database**:
  - Perfect for quick setup and testing.
- **Comprehensive API Documentation**:
  - Powered by Swagger (OpenAPI).

---

## **Technology Stack**
- **Backend**: ASP.NET Core 7.0
- **Database**: In-memory Database (EF Core)
- **Logging**: Serilog
- **API Documentation**: Swagger (OpenAPI)
- **Middleware**: Custom Exception, Logging, and Rate Limiting

---

## **Setup and Installation**

### **Prerequisites**
- [.NET SDK](https://dotnet.microsoft.com/download) (Version 7.0 or higher)
- [Git](https://git-scm.com/)
- Any API testing tool (e.g., Postman or HTTPie)

### **Steps**
1. **Clone the Repository**:
   ```bash
   git clone https://github.com/summerxrt/UserManagementAPI.git
   cd UserManagementAPI
   ```

2. **Build the Project**:
   ```bash
   dotnet build
   ```

3. **Run the Application**:
   ```bash
   dotnet run
   ```

4. **Access Swagger Documentation**:
   Navigate to:
   ```
   https://localhost:<port>/swagger
   ```
   Replace `<port>` with the actual port displayed in the terminal.

---

## **Usage**

### **Endpoints**
| Method | Endpoint           | Description              |
|--------|--------------------|--------------------------|
| GET    | `/api/users`       | Retrieve all users       |
| GET    | `/api/users/{id}`  | Retrieve a user by ID    |
| POST   | `/api/users`       | Add a new user           |
| PUT    | `/api/users/{id}`  | Update an existing user  |
| DELETE | `/api/users/{id}`  | Delete a user by ID      |

---

## **Testing**
You can test the API using [Postman](https://www.postman.com/) or the built-in Swagger UI.

### **Example Test Case**
**Retrieve All Users**:
1. Send a `GET` request to:
   ```
   https://localhost:<port>/api/users
   ```
2. Expected Response:
   ```json
   [
     {
       "id": 1,
       "name": "Alice",
       "email": "alice@example.com",
       "role": "Admin"
     },
     {
       "id": 2,
       "name": "Bob",
       "email": "bob@example.com",
       "role": "User"
     }
   ]
   ```

---

## **Contributing**
Contributions are welcome! 🎉
- Fork the repository.
- Create a new branch for your feature or bug fix.
- Submit a pull request with a clear explanation of changes.

---

## **License**
This project is licensed under the [MIT License](LICENSE).
