# RKSoftware.DAL.EntityFramework

[![NuGet](https://img.shields.io/nuget/v/RKSoftware.DAL.EntityFramework.svg)](https://www.nuget.org/packages/RKSoftware.DAL.EntityFramework/)
[![License](https://img.shields.io/badge/license-MIT%20OR%20Apache--2.0-blue.svg)](https://opensource.org/licenses)

## Overview

**RKSoftware.DAL.EntityFramework** is a .NET library that provides an implementation of the `RKSoftware.DAL.Core` contracts using Entity Framework Core. It enables developers to leverage the power of Entity Framework Core while adhering to the abstractions defined in the `RKSoftware.DAL.Core` package.

This package is part of the RK Software Systems ecosystem and is designed to simplify the integration of Entity Framework Core into your Data Access Layer (DAL).

## Features

- Full implementation of `RKSoftware.DAL.Core` contracts using Entity Framework Core.
- Supports transactional operations with `ITransactionalStorage`.
- Simplifies CRUD operations with `IStorage` and `IReadonlyStorage`.
- Compatible with .NET 9.0.
- Extensible and easy to integrate with existing projects.

## Installation

You can install the package via NuGet:

```
dotnet add package RKSoftware.DAL.EntityFramework --version 9.0.1
```

Or via the NuGet Package Manager in Visual Studio.

## Usage

To use this package, reference it in your project and configure it with your Entity Framework Core `DbContext`. Below is an example of how to set up and use the `RKSoftware.DAL.EntityFramework` package:

### Example

```
using System; using System.Threading.Tasks; 
using Microsoft.EntityFrameworkCore; 
using RKSoftware.DAL.EntityFramework;
using RKSoftware.DAL.Core;

public class MyDbContext : DbContext { public DbSet<MyEntity> MyEntities { get; set; } }

public class MyEntity { public int Id { get; set; } public string Name { get; set; } }

public class Example { 
  private readonly ITransactionalStorage _transactionalStorage;

  public Example(ITransactionalStorage transactionalStorage)
  {
      _transactionalStorage = transactionalStorage;
  }

  public async Task PerformOperationsAsync()
  {
      try
      {
          // Begin a transaction
          _transactionalStorage.BeginTransaction();
  
          // Add an entity
          var entity = new MyEntity { Id = 1, Name = "Sample Entity" };
          await _transactionalStorage.AddAsync(entity);
  
          // Commit the transaction
          await _transactionalStorage.CommitTransactionAsync();
          Console.WriteLine("Transaction committed successfully.");
      }
      catch (Exception ex)
      {
          Console.WriteLine($"An error occurred: {ex.Message}");
  
          // Rollback the transaction
          await _transactionalStorage.ResetTransactionAsync();
          Console.WriteLine("Transaction rolled back.");
      }
  }
}
```


### Configuration

To use `RKSoftware.DAL.EntityFramework`, you can register its services in your dependency injection container using the provided extension methods from the `DependencyRegistration` class. Below is an example:

```csharp
// Add your DbContext 
services.AddDbContext<MyDbContext>(options => options.UseSqlServer("YourConnectionString"));

// Register RKSoftware.DAL.EntityFramework services 
services.AddRKEFStorages();
```

#### Available Registration Methods

- **`AddRKEFReadonlyStorage`**: Registers the `IReadonlyStorage` implementation.
- **`AddRKEFQueryStorage`**: Registers the `IQueryStorage` implementation.
- **`AddRKEFStorage`**: Registers the `IStorage` and `ITransactionalStorage` implementations.
- **`AddRKEFStorages`**: Registers all the above implementations in one call.

Choose the appropriate method based on your requirements. For most use cases, `AddRKEFStorages` is sufficient as it registers all available storage implementations.


## Documentation

For more details about the core contracts, refer to the [RKSoftware.DAL.Core documentation](https://github.com/rk-software-systems/rk-dal).

## Repository

The source code for this package is available on GitHub:

[RKSoftware.DAL GitHub Repository](https://github.com/rk-software-systems/rk-dal)

Feel free to contribute, report issues, or suggest features.

## License

This package is licensed under the **MIT OR Apache-2.0** license. You may choose either license to use this package. See the [LICENSE](https://opensource.org/licenses) file for more details.


## Tags

- DAL
- RKSoftware
- Entity Framework
- .NET 9
- Data Access Layer