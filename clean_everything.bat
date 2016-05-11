@echo off
rem Pulisce tutti i progetti dai file generati dalla compilazione

rmdir /S /Q bin

rmdir /S /Q Biglietti\bin
rmdir /S /Q Biglietti\obj

rmdir /S /Q Controls\bin
rmdir /S /Q Controls\obj

rmdir /S /Q Database\bin
rmdir /S /Q Database\obj
rmdir /S /Q Database\sql

rmdir /S /Q Documentazione\bin
rmdir /S /Q Documentazione\obj
rmdir /S /Q Documentazione\help

rmdir /S /Q IndianaPark\bin
rmdir /S /Q IndianaPark\sql
rmdir /S /Q IndianaPark\obj
rmdir /S /Q IndianaPark\help

rmdir /S /Q Documentazione\help

rmdir /S /Q "SQL Server\bin"
rmdir /S /Q "SQL Server\obj"
rmdir /S /Q "SQL Server\sql"

rmdir /S /Q LicenseManager\Debug
rmdir /S /Q LicenseManager\Release

rmdir /S /Q PercorsiAvventura\bin
rmdir /S /Q PercorsiAvventura\obj

rmdir /S /Q Persistency\bin
rmdir /S /Q Persistency\obj

rmdir /S /Q PluginSystem\bin
rmdir /S /Q PluginSystem\obj

rmdir /S /Q PowerFan\bin
rmdir /S /Q PowerFan\obj

rmdir /S /Q Testing\bin
rmdir /S /Q Testing\obj
