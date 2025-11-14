@echo off
echo Creating PersonalDetailsDB database...
echo.
echo Please enter your MySQL root password when prompted:
echo.
"C:\Program Files\MySQL\MySQL Server 9.5\bin\mysql.exe" -u root -p -e "CREATE DATABASE IF NOT EXISTS PersonalDetailsDB; SHOW DATABASES;"
echo.
echo Database creation complete!
pause
