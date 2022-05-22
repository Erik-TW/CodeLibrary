# CodeLibrary

prerequisites: 
.NET 6
SQL Server 2019

In order to run the run the project, you need to add your own connection string to appsettings.json in the "DefaultConnection" property.
You do not need to run any migrations or database update scripts. The code will create the database when needed.

The easiest way to populate the database with data is to send a POST request to https://host:port/api/books/multiple with the contents of books.json as the body. 

All test cases have ben run and works for me. 