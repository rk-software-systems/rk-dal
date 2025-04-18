# RKSoftware.DAL.InMemory

[![NuGet](https://img.shields.io/nuget/v/RKSoftware.DAL.InMemory.svg)](https://www.nuget.org/packages/RKSoftware.DAL.InMemory/)
[![License](https://img.shields.io/badge/license-MIT%20OR%20Apache--2.0-blue.svg)](https://opensource.org/licenses)

## Overview

**RKSoftware.DAL.InMemory** is a .NET library that provides an implementation of the `RKSoftware.DAL.Core` contracts using in-memory storage. It is designed for lightweight, fast, and temporary data storage, making it ideal for testing, prototyping, or scenarios where persistence is not required.

This package is part of the RK Software Systems ecosystem and provides a simple, in-memory alternative to other storage implementations.

## Features

- Full implementation of `RKSoftware.DAL.Core` contracts using in-memory storage.
- Supports transactional operations with `ITransactionalStorage`.
- Simplifies CRUD operations with `IStorage` and `IReadonlyStorage`.
- Compatible with .NET 9.0.
- Lightweight and easy to integrate with existing projects.

## Installation

You can install the package via NuGet:

```
dotnet add package RKSoftware.DAL.InMemory --version 9.0.1
```

Or via the NuGet Package Manager in Visual Studio.

## Usage

To use this package, reference it in your project and configure it with the provided in-memory storage implementation. Below is an example of how to set up and use the `RKSoftware.DAL.InMemory` package:

### Example

```
public class Example 
{ 
  private readonly IStorage _storage;

  public Example(IStorage storage)
  {
      _storage = storage;
  }

  public async Task PerformOperationsAsync()
  {
      try
      {
          // Add an entity
          var entity = new MyEntity { Id = 1, Name = "Sample Entity" };
          await _storage.AddAsync(entity);
      }
      catch (Exception ex)
      {
          Console.WriteLine($"An error occurred: {ex.Message}");
      }
  }
}
```


### Configuration

To use `RKSoftware.DAL.InMemory`, you can register its services in your dependency injection container. Below is an example of how to configure it in an ASP.NET Core application:

```
using Microsoft.Extensions.DependencyInjection; 
using RKSoftware.DAL.InMemory;

var services = new ServiceCollection();
// Register RKSoftware.DAL.InMemory services 
services.UseInMemory()
  .AddReadonlyStorage()
  .AddStorage();
```

#### Available Registration Methods

**UseInMemory**: Initializes the in-memory storage configuration.
**AddReadonlyStorage**: Registers read-only storage.
**AddStorage**: Registers full CRUD storage.
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
- In-Memory
- .NET 9
- Data Access Layer