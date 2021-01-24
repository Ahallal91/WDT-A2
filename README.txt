# Assignment 1 - COSC2277 - S3811836:
https://github.com/rmit-wdt-fs-2021/s3811836_a2

## What is this repo for?

This repo contains an MVC ASP.NET Core 5.0 Web Based banking application.This assignment used a code first approach to create the tables for the database. The back-end is written using EF-Core and C#, whilst the front-end makes use of Razor Syntax. All data loaded into the Cloud server is populated via a REST API which returns a JSON format.

## Justify the use of a record in models
Reference: https://devblogs.microsoft.com/dotnet/c-9-0-on-the-record/
Records were used in the Models for Customer, Transactions and Login. The advantage for using records for these particular models is that the objects are passed by value, using less resources in the system, and furthermore ensures that these objects cannot be changed without copying the values of the original object and making a new object. For these particular models there are two main cases where the user might want to change these objects, but they won't happen very often. These are: Updating password, Updating customer contact information such as Address, City etc. All other times when these objects are used in the system, they should never need to be changed, using a record ensures this. For transactions a record is especially helpful as normally there would not be a need to modify a transaction in the system once it had occured, and, we can expect a bank to incur many transactions, meaning that there may be less resources used when accessing transactions if they are a record rather than a normal mutable class.



References:
MCBALoginWithExample week 6 project (specific areas are referenced in comments of project)
Alicia Hallal s3811836 Assignment 1 Library 'Utilities' was imported to use in Webservice and validation
ViewModel reference: https://docs.microsoft.com/en-us/aspnet/core/mvc/views/overview?view=aspnetcore-5.0
Piggy Bank image: https://undraw.co