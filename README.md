# AdForm-SQL-API

## Descryption
A .NET API project with PostgreSQL and Docker for [Order Management SQL Exercise](https://github.com/erinev/order-management-sql-exercise) by AdForm

## Technology used
1. PostgreSQL 17
2. DBeaver: a database management system

## Tasks and time duration
Database set up: 30 min.</br>
Mock data generation and upload to database: 1 hour.</br>
LinQ-to-Entity queries and model creation: 4 hour.</br>

### DB Scafold
dotnet ef dbcontext scaffold "Server=localhost;Port=5432;UserId=postgres;Password=admin;Database=AdFormSQL;" Npgsql.EntityFrameworkCore.PostgreSQL -o AdFormDB