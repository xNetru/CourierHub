# CourierHub
This team project was created as a part of course *Tworzenie aplikacji webowych z wykorzystaniem .NET Framework* on Warsaw University of Technology.

All projects were build on .NET 7.0 (excluding *CourierHub.IntegrationTest*, which was build on .NET 6.0).


## Project structure
The solution for CourierHub is composed of 9 projects.

### Web App:
- **CourierHub.Cloud** - a library with classes for accessing cloud services, such as Azure Blob Storage and Azure Communication Email Service
- **CourierHub.Client** - blazor frontend consisting mostly of razor pages and blazor components
- **CourierHub.Server** - backend full of controllers used by frontend and different REST Api implementations
- **CourierHub.Shared** - project containing shared classes e.g. models, enums or database context

### Web Api:
- **CourierHubWebApi** - REST Api containing models, controllers, services and middlewares

**Note:** *Web Api also depends on CourierHub.Shared to eliminate code redundancy.*

### Unit tests:
- **CourierHub.Test** - backend controllers tests written with xUnit framework and UI test with bUnit framework
- **CourierHubWebApi.Test** - xUnit tests for api controllers and services

### Integration tests:
- **CourierHub.DatabaseTest** - couple tests with scenarios testing integration with database via Entity Framework
- **CourierHub.IntegrationTest** - a few tests with scenarios covering flow from the backend controllers up to the REST Api (and back), written with Gherkin and SpecFlow framework

## Gitflow
1. Contributor makes own branch, where he pushes their changes.
2. Contributor makes a pull request to merge their branch with **dev**, one team member is required to accept it.
3. When pull request is accepted, **development** pipeline builds projects and runs unit tests.
4. At some point pull request to **master** branch is made, requires green light from two team members, requester is required to run integration tests.
5. When pull request is accepted, two deployment pipelines are triggered - **deployment-server** and **deployment-api**.
6. Artifacts published by said pipelines are used by two release pipelines - **release-server** and **release-api** - that deploy new version of build to azure app services.

## Required cloud resources
Our whole projects was configured using services availble at Azure Portal.

Database:
- Azure SQL server
- Azure SQL database

Storage:
- Azure (Blob) Storage account

Email:
- Azure Communication Service
- Azure Email Communication Service
- Azure Email Communication Services Domain

Deployment:
- Azure App Service plan
- 2 × Azure App Services (Web App and Api App)


## CourierHub Team
- Bartosz Kaczorowski
- Maksymilian Czapski
- Kamil Bańkowski