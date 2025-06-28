# AdForm-SQL-API

## Descryption
A .NET API project with PostgreSQL and Docker for [Order Management SQL Exercise](https://github.com/erinev/order-management-sql-exercise) by AdForm

## Technology used
1. PostgreSQL 17.
2. DBeaver: a database management system.
3. Docker.
4. Visual Studio 2022.
5. .NET 8.0
6. xUnit test.
7. Moq .NET: a data mocking tool.
8. JavaScript and [Faker](https://fakerjs.dev/) library for generating realistic mock data and SQL scripts to upload the data to PostgreSQL database

## Tasks and time duration
Database set up: 30 min.</br>
Mock data generation and upload to database: 1 hour.</br>
LinQ-to-Entity queries and model creation: 4 hour.</br>
Creating and setting dockert containers: 4 hours.</br>
Swagger API documentation: 1h.</br>
Unit tests: 6h.</br>

### DB Scafold
dotnet ef dbcontext scaffold "Server=localhost;Port=5432;UserId=postgres;Password=admin;Database=AdFormSQL;" Npgsql.EntityFrameworkCore.PostgreSQL -o AdFormDB