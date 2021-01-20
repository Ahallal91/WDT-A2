# Assignment 1 - COSC2277 - S3811836:
https://github.com/rmit-wdt-fs-2021/s3811836_a2

## What is this repo for?

This repo contains an MVC ASP.NET Core 5.0 Web Based banking application.This assignment used a code first approach to create the tables for the database. The back-end is written using EF-Core and C#, whilst the front-end makes use of Razor Syntax. All data loaded into the Cloud server is populated via a REST API which returns a JSON format.

## Justify the use of a record in models
Reference: https://devblogs.microsoft.com/dotnet/c-9-0-on-the-record/
Records were used in the Models for Customer and Login. The advantage for using records for these particular models is it means the Customer and Login objects are passed by value, using less resources in the system, and furthermore ensures that these objects cannot be changed without copying the values of the original object and making a new object. For these particular models there are two main cases where the user might want to change these objects, but they won't happen very often. These are: Updating password, Updating customer contact information such as Address, City etc. All other times when these objects are used in the system, they should never need to be changed. By keeping login and customer information immutable, we can ensure that the objects are only changed when they need to be rather than by accident.


