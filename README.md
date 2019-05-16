# OnlineBookStore
Online Book Store

.NET Core Webapplication which is used for purchasing books online. User can register and login to site for placing order. On successful login user will be able to search for books and add required books to shopping cart.

Application Structure
=========================
Applicaiton is distributed into multiple solutions. All core solutions are developed in .NET Core 2.2 to make them platform independent.
Below are list of solutions available in project and their purpose.

BookStore.API 
=============
This is api part of solution which will be exposed to third party systems to consume data. Once configured and launced applicaiton will display all endpoints available using swagger UI.

BookStore.Common 
=================
This solution contains all domain models and repository contracts required for bookstore application.

BookStore.DataStore 
====================
This solution contains database mapping part. In this solution SQLLite is been used as database. This can be changed easily by couple of configuration changes as solution is using Entity Framework Core for dataaccess logic. This solution contains all core repository classes which provide CRUD funcitonality for domain entities.

BookStore.Service 
=================
This solution is single point where all repositories are been used and combined into single service. This service will be further used by API for delegating user requests. Services solution also contains helper metods required for object conversions.

BookStore.Web 
=============
This is thin client applicaiton developed in .NET Framework. Most of communicaiton will backend api is been handled using JQUERY ajax functionality.


Pre-Requisites
---------------
1. Visual Studio 2019 
2. .Net Core 2.2


Building And Running Solution Locally
=====================================
1. Clone repository from GIT Hub
2. Restore all packages
3. Build Solutions
4. To start api solution navigate to BooksTore.Api folder. Run CMD --> dotnet run BookStore.Api.csproj
5. To start web UI solution Run visual studio
