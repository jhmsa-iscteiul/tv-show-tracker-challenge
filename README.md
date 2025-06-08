# TV Shows API

![.NET](https://img.shields.io/badge/.NET-8-blue)
![Docker](https://img.shields.io/badge/Docker-Container-blue)
![SQL Server](https://img.shields.io/badge/Database-SQL%20Server-green)

A RESTful Web API built with **.NET 8** that fetches information about TV shows and stores it in a **SQL Server** database running inside a Docker container.

---

## Overview

This API provides access to detailed TV show data including shows, episodes, actors, and Tv Shows genres.
The API provides the possibility of filtering and sorting it's data and includes a Background Worker that Fetchets Data from the https://www.tvmaze.com/api API and saves on a SQL Server
in wich the API feeds from

---

## Technology Stack

| Technology        | Description                            |
|-------------------|------------------------------------|
| .NET 8            | Backend Web API framework           |
| SQL Server        | Database for storing TV show data   |
| Docker            | Containerization for SQL Server     |
| Entity Framework Core | ORM for database interaction     |

---

## Database Structure

#Main Data Tables

| Table   | Description                            |
|---------|--------------------------------------|
| Actors  | Stores actor information              |
| Shows   | Stores generic information about TV shows |
| Genres  | Stores all genres in the dataset      |
| Episodes| Stores detailed information about episodes |
| Users   | Stores API user information and credentials |

#Join Tables

| Table   | Description                            |
|---------|--------------------------------------|
| ActorShow  | Results from a Many-to-many relationship between Actors table and Shows table|
| GenreShow   | Results from a Many-to-many relationship between Genre table and Shows table |
| UserFavorite  | Results from a Many-to-many relationship between Users table and Shows table linking users to their favorite shows|
| Episodes| Stores detailed information about episodes |
| Users   | Stores API user information and credentials |

---

## Features

- Fetch and manage TV show data via a Background workers that searches for more updated Tv Show information from the source API.
- Store and retrieve actors, genres, shows, and episodes.
- User management for API access control.
- Runs with a SQL Server instance inside a Docker container for easy deployment.

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/get-started)

### Running the Database

Run the following command to start a SQL Server container:

```bash
docker run -d --name sqlserver-container -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong!Pass123" -p 1433:1433 my-sqlserver-image
``` 
If the previous command doesn't work try

```bash
docker run -d --name sqlserver-container -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong\!Pass123" -p 1433:1433 my-sqlserver-image
``` 
### Running the API

```bash
dotnet run
```

### To test the API via swagger access: 

http://localhost/swagger


## Key Feutures not implemented and known issues

  - Endpoint for a User to Add and Remove Favorite TvShows is not implemented
  - No authentication system as yet been implemented
  - The necessary and required Unit Tests were not implemented.

