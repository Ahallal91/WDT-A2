# Assignment 1 - COSC2277 - S3811836:
https://github.com/rmit-wdt-fs-2021/s3811836_a2

## What is this repo for?

This repo contains an MVC ASP.NET Core 5.0 Web Based banking application.This assignment used a code first approach to create the tables for the database. The back-end is written using EF-Core and C#, whilst the front-end makes use of Razor Syntax. All data loaded into the Cloud server is populated via a REST API which returns a JSON format.

## Justify the use of a record in models
Reference: https://devblogs.microsoft.com/dotnet/c-9-0-on-the-record/
Records were used in the Models for Customer, Transactions and Login. The advantage for using records for these particular models is that the objects are passed by value, using less resources in the system, and furthermore ensures that these objects cannot be changed without copying the values of the original object and making a new object. For these particular models there are two main cases where the user might want to change these objects, but they won't happen very often. These are: Updating password, Updating customer contact information such as Address, City etc. All other times when these objects are used in the system, they should never need to be changed, using a record ensures this. For transactions a record is especially helpful as normally there would not be a need to modify a transaction in the system once it had occured, and, we can expect a bank to incur many transactions, meaning that there may be less resources used when accessing transactions if they are a record rather than a normal mutable class.

## Repository Pattern (detailed explanation in API project)
Using the repository pattern for the WebAPI allows the API to seperate the logic without affecting the
outside users of the API itself. By using an interface to dictate how the managers are implemented, 
we can ensure that each manager class is very similar in nature, making the API easier to use. The managers keep all of the complex code for performing the logic such as filtering the code and calling the database itself. These are then seperated from the controllers which are what outside users are directly accessing. This means we can change logic inside the manager class without it affecting outside users. It also means although we can implement code that the interface specifies for each manager, we can decide how the code is called in the controller. For example we may not want certain people to be able to edit logins for customers, however they may be able to view them, but we want admins to be able to edit so we can implemented that logic in the controller with certain access rights. As the main logic for editing the logins is in the manager class it can easily be controlled on how it is accessed and what is available to the outside world.

References:
MCBALoginWithExample week 6 project (specific areas are referenced in comments of project)
Other tutorial examples are referenced in the specific comments of where the code is used.
Alicia Hallal s3811836 Assignment 1 Library 'Utilities' was imported to use in Webservice and validation
ViewModel reference: https://docs.microsoft.com/en-us/aspnet/core/mvc/views/overview?view=aspnetcore-5.0
Piggy Bank image: https://undraw.co
MSDN documentation for Identity API