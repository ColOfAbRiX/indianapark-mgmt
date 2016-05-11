@echo off
rem Installa sull'istanza SQLServer locale il database del progetto

sqlcmd -S LOCALHOST\SQLEXPRESS -U sa -P ColoFabrix -d master -i IndianaPark\sql\release\server_deploy.sql
sqlcmd -S LOCALHOST\SQLEXPRESS -U sa -P ColoFabrix -d master -i IndianaPark\sql\release\database_deploy.sql