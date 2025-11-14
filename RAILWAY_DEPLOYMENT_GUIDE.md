# Railway Deployment Guide

## Complete Guide to Deploy Personal Details API to Railway

---

## Prerequisites

1. **GitHub Account** - Create one at https://github.com
2. **Railway Account** - Sign up at https://railway.app (use GitHub to sign in)
3. **Git installed** - Download from https://git-scm.com/downloads

---

## Part 1: Prepare Your Project for Deployment

### ✅ Files Already Created

The following files have been created for Railway deployment:

1. **Dockerfile** - Containerizes your application
2. **.dockerignore** - Excludes unnecessary files from Docker build
3. **.gitignore** - Excludes build artifacts from Git
4. **appsettings.Production.json** - Production configuration
5. **Updated Program.cs** - Configured for environment variables

---

## Part 2: Push Your Code to GitHub

### Step 1: Initialize Git Repository

Open Command Prompt in your project folder:

```bash
cd C:\Users\amjath-20976\Desktop\PersonalDetailsAPI
git init
```

### Step 2: Add All Files

```bash
git add .
```

### Step 3: Commit Your Code

```bash
git commit -m "Initial commit - Personal Details API"
```

### Step 4: Create GitHub Repository

1. Go to https://github.com/new
2. Repository name: `PersonalDetailsAPI` (or any name you prefer)
3. Description: `Personal Details Management API`
4. **Keep it Public** (or Private - both work)
5. **Do NOT** initialize with README, .gitignore, or license
6. Click **"Create repository"**

### Step 5: Push to GitHub

Copy the commands shown on GitHub (replace with your repository URL):

```bash
git remote add origin https://github.com/YOUR_USERNAME/PersonalDetailsAPI.git
git branch -M main
git push -u origin main
```

**Note:** Replace `YOUR_USERNAME` with your actual GitHub username.

---

## Part 3: Deploy to Railway

### Step 1: Create Railway Project

1. Go to https://railway.app
2. Click **"Login"** (sign in with GitHub)
3. Click **"New Project"**
4. Select **"Deploy from GitHub repo"**
5. If prompted, authorize Railway to access your repositories
6. Select your **PersonalDetailsAPI** repository

### Step 2: Add MySQL Database

1. In your Railway project, click **"+ New"**
2. Select **"Database"**
3. Choose **"Add MySQL"**
4. Railway will automatically provision a MySQL database

### Step 3: Configure Environment Variables

1. Click on your **API service** (PersonalDetailsAPI)
2. Go to the **"Variables"** tab
3. Click **"+ New Variable"**

Add the following environment variables:

#### **DATABASE_URL**
```
mysql://root:YOUR_PASSWORD@mysql.railway.internal:3306/railway
```

**To get the correct value:**
1. Click on the **MySQL** service in your Railway project
2. Go to **"Connect"** tab
3. Look for **"Database Connection URL"**
4. Copy the **Private URL** (looks like: `mysql://root:xxx@mysql.railway.internal:3306/railway`)
5. Paste it as the value for **DATABASE_URL**

#### **ASPNETCORE_ENVIRONMENT** (Optional)
```
Production
```

#### **ALLOWED_ORIGINS** (Optional - for CORS)
```
*
```

Or specify your frontend URL:
```
https://your-frontend.com,https://another-domain.com
```

### Step 4: Deploy

1. Railway will automatically detect the Dockerfile
2. It will start building and deploying your application
3. Wait for the deployment to complete (usually 2-5 minutes)
4. Watch the build logs in the **"Deployments"** tab

### Step 5: Run Database Migrations

Once the application is deployed:

1. Click on your API service
2. Go to the **"Settings"** tab
3. Scroll down to **"Service Settings"**
4. Copy the **"Public Networking"** URL (e.g., `https://your-app-name.up.railway.app`)

**Run migrations using Railway CLI:**

Install Railway CLI:
```bash
npm i -g @railway/cli
```

Login to Railway:
```bash
railway login
```

Link to your project:
```bash
cd C:\Users\amjath-20976\Desktop\PersonalDetailsAPI
railway link
```

Run migrations:
```bash
railway run dotnet ef database update
```

**Alternative: Run migrations from your local machine**

Update your local `appsettings.json` temporarily with the Railway MySQL connection string, then:

```bash
dotnet ef database update
```

(Remember to change it back after!)

### Step 6: Enable Public URL

1. In your API service settings
2. Under **"Networking"**, click **"Generate Domain"**
3. Railway will assign a public URL like: `https://personaldetailsapi-production.up.railway.app`

---

## Part 4: Access Your Deployed API

### Your API URLs:

**Base URL:**
```
https://your-app-name.up.railway.app
```

**Swagger Documentation:**
```
https://your-app-name.up.railway.app
```

**API Endpoints:**
```
GET    https://your-app-name.up.railway.app/api/personaldetails
POST   https://your-app-name.up.railway.app/api/personaldetails
GET    https://your-app-name.up.railway.app/api/personaldetails/{id}
PUT    https://your-app-name.up.railway.app/api/personaldetails/{id}
DELETE https://your-app-name.up.railway.app/api/personaldetails/{id}
```

---

## Part 5: Test Your Deployed API

### Using cURL:

```bash
curl https://your-app-name.up.railway.app/api/personaldetails
```

### Using Browser:

Open:
```
https://your-app-name.up.railway.app
```

You should see the Swagger UI!

### Test POST Request:

```bash
curl -X POST https://your-app-name.up.railway.app/api/personaldetails \
  -H "Content-Type: application/json" \
  -d '{
    "fullName": "Test User",
    "phoneNumber": "9876543210",
    "residentialStatus": 0,
    "address": "123 Test Street"
  }'
```

---

## Part 6: Update Your Deployment

When you make changes to your code:

### Step 1: Commit Changes

```bash
git add .
git commit -m "Description of changes"
```

### Step 2: Push to GitHub

```bash
git push
```

### Step 3: Automatic Deployment

Railway will automatically:
- Detect the push
- Rebuild your Docker container
- Deploy the new version
- Zero-downtime deployment

---

## Common Issues & Solutions

### Issue 1: Build Fails

**Solution:**
- Check the build logs in Railway
- Ensure all NuGet packages are restored
- Verify Dockerfile syntax

### Issue 2: Database Connection Error

**Solution:**
1. Verify `DATABASE_URL` environment variable is correct
2. Check that MySQL service is running
3. Ensure the database URL uses `mysql.railway.internal` for private networking

### Issue 3: Migrations Not Applied

**Solution:**
Run migrations manually:
```bash
railway run dotnet ef database update
```

Or create a one-time job:
```bash
railway run --service your-api-service dotnet ef database update
```

### Issue 4: Port Binding Issues

**Solution:**
The Dockerfile is already configured to use port 8080 (Railway's default).
If issues persist, check Railway service logs.

### Issue 5: CORS Errors

**Solution:**
Add your frontend URL to `ALLOWED_ORIGINS` environment variable:
```
https://your-frontend.com
```

---

## Environment Variables Reference

| Variable | Required | Example | Description |
|----------|----------|---------|-------------|
| `DATABASE_URL` | Yes | `mysql://root:xxx@mysql.railway.internal:3306/railway` | MySQL connection string from Railway |
| `ASPNETCORE_ENVIRONMENT` | No | `Production` | Sets the environment (Development/Production) |
| `ALLOWED_ORIGINS` | No | `*` or `https://example.com` | CORS allowed origins |

---

## Monitoring & Logs

### View Logs:

1. Go to your Railway project
2. Click on your API service
3. Click **"Deployments"** tab
4. Click on the latest deployment
5. View real-time logs

### Monitor Resource Usage:

1. Go to your API service
2. Check **"Metrics"** tab
3. Monitor CPU, Memory, and Network usage

---

## Costs

**Railway Free Tier:**
- $5 of free usage per month
- Suitable for development and small projects
- No credit card required to start

**Paid Plans:**
- Usage-based pricing
- Only pay for what you use
- Approximately $5-20/month for small APIs

---

## Backup & Database Management

### Backup Database:

```bash
railway run mysqldump -u root -p railway > backup.sql
```

### Restore Database:

```bash
railway run mysql -u root -p railway < backup.sql
```

### Access MySQL CLI:

```bash
railway connect MySQL
```

---

## Custom Domain (Optional)

### Add Your Own Domain:

1. Go to your API service in Railway
2. Click **"Settings"**
3. Under **"Domains"**, click **"Custom Domain"**
4. Enter your domain (e.g., `api.yourdomain.com`)
5. Add the CNAME record to your DNS provider:
   - Type: CNAME
   - Name: api (or your subdomain)
   - Value: (Railway will provide this)

---

## Security Best Practices

1. ✅ **Never commit passwords** - Use environment variables
2. ✅ **Use HTTPS only** - Railway provides this automatically
3. ✅ **Restrict CORS** - Set specific allowed origins in production
4. ✅ **Regular backups** - Backup your database regularly
5. ✅ **Monitor logs** - Check for suspicious activity
6. ✅ **Keep dependencies updated** - Regular security updates

---

## Quick Reference Commands

```bash
# Initialize Git
git init

# Commit changes
git add .
git commit -m "message"

# Push to GitHub
git push

# Install Railway CLI
npm i -g @railway/cli

# Login to Railway
railway login

# Link project
railway link

# Run migrations
railway run dotnet ef database update

# View logs
railway logs

# Connect to MySQL
railway connect MySQL
```

---

## Support & Resources

- **Railway Docs:** https://docs.railway.app
- **Railway Discord:** https://discord.gg/railway
- **.NET Deployment:** https://railway.app/template/dotnet

---

## Summary Checklist

- [ ] Code pushed to GitHub
- [ ] Railway project created
- [ ] MySQL database added to Railway
- [ ] Environment variables configured (`DATABASE_URL`)
- [ ] Application deployed successfully
- [ ] Database migrations applied
- [ ] Public URL generated
- [ ] Swagger UI accessible
- [ ] API endpoints tested
- [ ] Frontend updated with new API URL (if applicable)

---

**Your API is now deployed and accessible worldwide! 🚀**

For questions or issues, check Railway logs or refer to this guide.
