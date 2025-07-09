# Indg Image API

This project is a .NET 8 Web API that supports full CRUD operations for images, including upload, retrieval with metadata, serving resized variations (e.g., thumbnails), and deletion of images and their variations.

It supports basic validation, and has logging integrated for request tracking and error monitoring.

---

## 🔧 Running the Service Locally

### 🧱 Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/products/docker-desktop/) (optional)
- A tool like Postman or cURL to test the endpoints
- SQLite (already handled by EF Core; no installation needed)
- SQLite Viewer (optional, to inspect the local database)
  - [DB Browser for SQLite](https://sqlitebrowser.org/)

---

### ▶️ Run Locally without Docker

1. **Clone the repository**
   ```bash
   git clone https://github.com/miesro/indg-image-api.git
   cd indg-image-api/src
   ```

2. **Restore dependencies and build**
   ```bash
   dotnet restore
   dotnet build
   ```

3. **Apply EF Core migrations**  
   This creates a local SQLite database file.
   ```bash
   dotnet ef database update --project Indg.DataAccess
   ```

4. **Run the API**
   ```bash
   dotnet run --project Indg.ImageAPI
   ```

5. **Access Swagger documentation**
   ```
   http://localhost:5000/swagger
   ```

---

### ▶️ Run Locally with Docker

1. **Build the Docker image**
   ```bash
   docker build -t indg-imageapi:dev -f Docker/Dockerfile .
   ```

2. **Run the container**
   ```bash
   docker run -d -p 8080:80 --name imageapi-dev indg-imageapi:dev
   ```

3. **Access the API**
   ```
   http://localhost:8080/swagger
   ```

---

## ☁️ Deployment to Azure

This project supports deployment to Azure using Docker and GitHub Actions.

---

### 1. **Authentication via Azure Service Principal**

Your GitHub repository must include the following secret:

- `AZURE_CREDENTIALS`: a JSON string with your Azure service principal, including `clientId`, `clientSecret`, `subscriptionId`, and `tenantId`.

You can generate it using the Azure CLI:

```bash
az ad sp create-for-rbac --name "credentials" --role contributor \
  --scopes /subscriptions/<subscription-id>/resourceGroups/<resource-group> \
  --sdk-auth
```

Copy the JSON output and save it as the `AZURE_CREDENTIALS` secret in GitHub.

---

### 2. **CI/CD with GitHub Actions**

This repository includes a workflow at:

```
.github/workflows/deploy.yml
```

The workflow:
- Authenticates to Azure using `AZURE_CREDENTIALS`
- Builds a Docker image and pushes it to Azure Container Registry (ACR)
- Deploys the image to your Azure Web App for Containers

> ✅ Ensure your Azure Web App is configured to pull from ACR.

---

### 🔁 Replace the following placeholders in your `deploy.yml`:

```yaml
env:
  ACR_NAME: azure-container-registry-name              # Replace with your actual ACR name
  IMAGE_NAME: indgimageapi                             # Docker image name (can match your project)
  WEBAPP_NAME: azure-app-service-for-container         # Replace with your Azure Web App name
  ACR_LOGIN_SERVER: azure-container-registry-name.azurecr.io  # Matches your ACR name + .azurecr.io
```

These values must reflect your actual Azure setup to ensure proper build and deployment.

---

## 🛠️ Making Database Changes (EF Core Migrations)

This project uses [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/) with SQLite.

### ➕ Add a New Migration

1. Modify your entity models or the `DbContext`.
2. Create a new migration using:
   ```bash
   dotnet ef migrations add <MigrationName> --project Indg.DataAccess
   ```

### 🧪 Apply Migrations Locally

Run this command to update the local SQLite database:
```bash
dotnet ef database update --project Indg.DataAccess
```

### 📦 Apply Migrations in Production

In production environments, you can:
- Trigger migrations manually during deployment.
- Or integrate a migration step in CI/CD.
