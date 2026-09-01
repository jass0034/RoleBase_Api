# RoleBase API - Docker & Render Deployment Guide

## 📋 Prerequisites
- Docker & Docker Compose installed
- Git installed
- Render account (https://render.com)
- SQL Server database (local, Azure, or cloud)

---

## 🐳 Local Docker Setup

### 1. Build Docker Image
```bash
docker build -t rolebase-api:latest .
```

### 2. Run with Docker Compose (includes SQL Server)
```bash
docker-compose up -d
```

This will:
- Build the API image
- Start SQL Server container
- Start the API on `http://localhost:8080`
- Swagger UI on `http://localhost:8080/swagger`

### 3. Check Logs
```bash
docker-compose logs -f rolebase-api
```

### 4. Stop Containers
```bash
docker-compose down
```

---

## 🚀 Deploy to Render

### Step 1: Prepare Your Repository
```bash
# Make sure all files are committed
git add .
git commit -m "Add Docker configuration"
git push origin master
```

### Step 2: Create Render Account & Web Service
1. Go to https://render.com
2. Sign up with GitHub
3. Click "New" → "Web Service"
4. Connect your GitHub repository (`https://github.com/jass0034/RoleBase_Api`)
5. Select `master` branch

### Step 3: Configure Deployment Settings

**In Render Dashboard:**

| Setting | Value |
|---------|-------|
| Name | `rolebase-api` |
| Environment | `Docker` |
| Plan | `Free` (or paid as needed) |
| Runtime | Auto-detected |
| Build Command | Leave empty (uses Dockerfile) |
| Start Command | Leave empty (uses Dockerfile ENTRYPOINT) |

### Step 4: Add Environment Variables

In **Environment** section, add:

```
ASPNETCORE_URLS=http://+:8080
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=Server=YOUR_DB_SERVER;Database=RoleBase_Api;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True;MultipleActiveResultSets=true
Appsettings__Secret=your-jwt-secret-key-here
TwilioSettings__AccountSid=your-twilio-account-sid
TwilioSettings__AuthToken=your-twilio-auth-token
TwilioSettings__PhoneNumber=+1xxxxxxxxxx
```

### Step 5: Deploy
- Click "Create Web Service"
- Render will automatically build and deploy
- Check deployment logs in Render dashboard

### Step 6: Verify Deployment
- Your app will be at: `https://rolebase-api.onrender.com`
- Swagger UI: `https://rolebase-api.onrender.com/swagger`

---

## 🗄️ Database Setup on Render

### Option 1: Use External SQL Server
- Use Azure SQL Database, AWS RDS, or similar
- Update connection string in environment variables

### Option 2: Use Render PostgreSQL (if you migrate)
Currently, Render doesn't officially support SQL Server. You would need to:
1. Migrate from SQL Server to PostgreSQL
2. Update your `Program.cs` to use PostgreSQL provider
3. Update migration files

---

## 📝 Environment Variables for Render

**Required Variables:**
```
ConnectionStrings__DefaultConnection=Your_Database_Connection_String
Appsettings__Secret=Your_JWT_Secret_Key
```

**Optional (Twilio):**
```
TwilioSettings__AccountSid=Your_Account_SID
TwilioSettings__AuthToken=Your_Auth_Token
TwilioSettings__PhoneNumber=Your_Twilio_Number
```

---

## 🔍 Troubleshooting

### Database Connection Issues
- Check connection string format
- Verify database server is accessible from Render
- Ensure firewall allows connections

### Build Failures
- Check Render logs for exact error
- Ensure all dependencies are restored
- Verify .NET 10 is available in SDK

### Port Issues
- Render automatically assigns PORT environment variable
- Application must listen on port `8080`
- ASPNETCORE_URLS should be: `http://+:8080`

---

## 🔄 Continuous Deployment

### Auto-Deploy from GitHub
1. Connect your GitHub repo to Render
2. Enable "Auto-Deploy" in Render settings
3. Every push to `master` will trigger deployment

### Manual Redeploy
1. Go to your service in Render dashboard
2. Click "Redeploy"

---

## 📊 Monitoring

- **Logs**: View in Render Dashboard → Service → Logs
- **Metrics**: View CPU, Memory, Network usage
- **Restarts**: Check service restart history

---

## 🛠️ Local Development Tips

```bash
# Run with Docker Compose for full stack
docker-compose up

# Rebuild after code changes
docker-compose up --build

# Clean up
docker-compose down -v
```

---

## ⚠️ Important Notes

1. **Secrets**: Never commit real secrets. Use environment variables on Render.
2. **Database**: For production, use a dedicated database service.
3. **Port**: Make sure your application listens on the PORT provided by Render (default 8080).
4. **Health Checks**: Render will check `/swagger` endpoint to verify service health.

---

## ✅ Deployment Checklist

- [ ] Docker files created (Dockerfile, .dockerignore)
- [ ] All code committed to GitHub
- [ ] Render account created
- [ ] Web service connected to GitHub repo
- [ ] Environment variables configured
- [ ] Database connection tested
- [ ] Service deployed successfully
- [ ] Swagger UI accessible at deployed URL
- [ ] Auto-deploy enabled (optional)

---

For more help, visit: https://render.com/docs
