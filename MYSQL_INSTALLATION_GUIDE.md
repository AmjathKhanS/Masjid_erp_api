# MySQL Installation Guide for Windows

## Step-by-Step Installation Instructions

### Step 1: Download MySQL Installer

1. Open your web browser and go to: **https://dev.mysql.com/downloads/installer/**

2. You'll see two options:
   - **mysql-installer-web-community** (~2MB) - Downloads packages during installation
   - **mysql-installer-community** (~400MB) - Includes all packages

3. Click **Download** on either version (I recommend the web version for faster download)

4. You may see a login/sign-up page - **Click "No thanks, just start my download"** at the bottom

### Step 2: Run the Installer

1. Locate the downloaded file (usually in `Downloads` folder)
2. **Double-click** `mysql-installer-community-x.x.xx.msi`
3. If you see a security warning, click **"Yes"** to allow

### Step 3: Choose Setup Type

1. Select **"Developer Default"** (recommended) or **"Server only"** (minimal)
2. Click **Next**

### Step 4: Check Requirements

1. The installer will check for required software (Visual C++ Redistributables)
2. If anything is missing, click **"Execute"** to install them
3. Click **Next** when done

### Step 5: Installation

1. Review the products to be installed
2. Click **Execute** to start installation
3. Wait for all components to install (this may take 5-10 minutes)
4. Click **Next** when all items show green checkmarks

### Step 6: Product Configuration

#### MySQL Server Configuration

1. **Type and Networking**
   - Config Type: **Development Computer**
   - Port: **3306** (default - keep this)
   - Click **Next**

2. **Authentication Method**
   - Select: **"Use Strong Password Encryption for Authentication"** (Recommended)
   - Click **Next**

3. **Accounts and Roles** - IMPORTANT!
   - Set a **Root Password**: Enter a password you'll remember
   - **WRITE THIS PASSWORD DOWN!** You'll need it for the API
   - Example: `MySecurePassword123!`
   - Re-type the password to confirm
   - (Optional) You can add additional user accounts, but not necessary
   - Click **Next**

4. **Windows Service**
   - Configure MySQL Server as Windows Service: **Checked**
   - Service Name: **MySQL80** (default)
   - Start the MySQL Server at System Startup: **Checked**
   - Click **Next**

5. **Server File Permissions**
   - Select default option
   - Click **Next**

6. **Apply Configuration**
   - Click **Execute**
   - Wait for all steps to complete
   - Click **Finish**

### Step 7: MySQL Router Configuration (if prompted)

- Click **Next** → **Finish** (default settings are fine)

### Step 8: Connect to Server

1. You may be asked to connect to the server
2. Enter the **root password** you created
3. Click **Check** to verify connection
4. Click **Next** → **Execute** → **Finish**

### Step 9: Complete Installation

1. Click **Next**
2. Uncheck **"Start MySQL Workbench"** (unless you want to use it)
3. Click **Finish**

## Step 10: Verify MySQL Installation

Open **Command Prompt** and run:

```bash
mysql --version
```

You should see something like: `mysql  Ver 8.0.xx for Win64`

## Step 11: Create the Database

### Option A: Using Command Line

1. Open **Command Prompt**
2. Login to MySQL:
   ```bash
   mysql -u root -p
   ```
3. Enter your root password when prompted
4. Create the database:
   ```sql
   CREATE DATABASE PersonalDetailsDB;
   ```
5. Verify it was created:
   ```sql
   SHOW DATABASES;
   ```
6. Exit MySQL:
   ```sql
   EXIT;
   ```

### Option B: Using MySQL Workbench (if installed)

1. Open **MySQL Workbench**
2. Click on **Local instance MySQL80**
3. Enter your root password
4. Click on the **database icon** or run this query:
   ```sql
   CREATE DATABASE PersonalDetailsDB;
   ```
5. Click the **lightning bolt icon** to execute

## Step 12: Update Your API Configuration

1. Open `C:\Users\amjath-20976\Desktop\PersonalDetailsAPI\appsettings.json`

2. Update the connection string with your password:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Port=3306;Database=PersonalDetailsDB;User=root;Password=YOUR_PASSWORD_HERE;"
     }
   }
   ```

3. Replace `YOUR_PASSWORD_HERE` with the password you set during installation

4. Save the file

## Step 13: Apply Database Migrations

Open **Command Prompt** and run:

```bash
cd C:\Users\amjath-20976\Desktop\PersonalDetailsAPI
dotnet ef database update
```

You should see:
```
Build succeeded.
Applying migration...
Done.
```

## Step 14: Run Your API

```bash
dotnet run
```

## Step 15: Test the API

Open your browser and go to:
- **https://localhost:7159** (or whatever port is shown in the terminal)

You should now see the Swagger UI without errors!

---

## Troubleshooting

### Issue: "mysql is not recognized as an internal or external command"

**Solution:** MySQL wasn't added to PATH. You can either:
- Restart your computer
- Or use the full path: `"C:\Program Files\MySQL\MySQL Server 8.0\bin\mysql.exe" -u root -p`

### Issue: "Access denied for user 'root'@'localhost'"

**Solution:** Wrong password. Try:
1. Reset MySQL root password using MySQL Installer
2. Or verify the password you're using

### Issue: "Can't connect to MySQL server on 'localhost'"

**Solution:** MySQL service isn't running:
1. Press **Windows + R**
2. Type: `services.msc`
3. Find **MySQL80** service
4. Right-click → **Start**

### Issue: Port 3306 already in use

**Solution:** Another MySQL instance is running:
- Check if you have another MySQL installation
- Or change the port during installation

---

## Quick Reference

**MySQL Service Management:**

Start MySQL:
```bash
net start MySQL80
```

Stop MySQL:
```bash
net stop MySQL80
```

**Login to MySQL:**
```bash
mysql -u root -p
```

**Common MySQL Commands:**
```sql
SHOW DATABASES;                    -- List all databases
USE PersonalDetailsDB;             -- Select database
SHOW TABLES;                       -- List all tables
DESCRIBE PersonalDetails;          -- Show table structure
SELECT * FROM PersonalDetails;     -- View data
```

---

## After Installation

Once MySQL is installed and the database is created:

1. Update `appsettings.json` with your MySQL password
2. Run: `dotnet ef database update`
3. Run: `dotnet run`
4. Access: `https://localhost:7159`

Your API is now ready to use with MySQL!
