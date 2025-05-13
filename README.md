# 📚 Book Library API & Frontend

This is a full-stack Book Library project designed to help you manage personal books that you own, love, or want to read. It features a **.NET 8 Web API backend** and a **React frontend** using **Material UI**. The solution is fully containerized and ready for **deployment to Azure App Service using Linux containers**.

---

## 🌐 Features

### ✅ Backend (.NET 8 Web API)
- List All Books
- Search books by **author**, **ISBN**, or **status** (`own`, `love`, `want`)
- Add, update, and delete books
- CORS-enabled for frontend access
- Dockerized and ready to run in **Azure App Service**
- SQL Server integration using Entity Framework Core
- Support for pagination

### ✅ Frontend (React + Material UI)
- List All Books
- Search form with inputs for author, ISBN, and status (dropdown)
- Results displayed in a responsive table
- API communication using Axios
- Clear error handling and input filtering
- Support for pagination

---

## 🛠 Technologies Used

- **.NET 8**
- **Entity Framework Core**
- **SQL Server 2022 (via Docker)**
- **React 18**
- **Material UI v5**
- **Axios**
- **Docker & Docker Compose**

---

## 🚀 Getting Started (Local)

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [Node.js](https://nodejs.org/)
- [Docker](https://www.docker.com/)

---

### 🐳 Run via Docker Compose

From the project root:

```bash
docker-compose up --build
```

Backend: http://localhost:8080
Swagger UI: http://localhost:8080/swagger

💻 Run the Frontend

In a separate terminal:

```bash
cd book-library-frontend
npm install
npm start
```


Visit: http://localhost:3000

🧪 Example Book Object

```bash

{
  "title": "Clean Code",
  "firstName": "Robert",
  "lastName": "Martin",
  "isbn": "9780132350884",
  "type": "Paperback",
  "totalCopies": 1,
  "copiesInUse": 0,
  "category": "Software",
  "status": "love"
}

```

🐳 Dockerfile

```dockerfile

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY BookLibraryAPI.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 80
ENTRYPOINT ["dotnet", "BookLibraryAPI.dll"]

```



☁️ Deploying to Azure (Docker)

This app is ready to be deployed as a Linux container on Azure App Service.

1. Build and tag the Docker image

```bash

docker build -t book-library-api .

```

```bash

docker tag book-library-api myregistry.azurecr.io/book-library-api:v1
docker push myregistry.azurecr.io/book-library-api:v1


```

```bash

az webapp create \
  --resource-group MyGroup \
  --plan MyPlan \
  --name BookLibraryApp \
  --deployment-container-image-name myregistry.azurecr.io/book-library-api:v1

```




