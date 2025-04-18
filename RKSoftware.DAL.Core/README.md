# RKSoftware.DAL.Core

[![NuGet](https://img.shields.io/nuget/v/RKSoftware.DAL.Core.svg)](https://www.nuget.org/packages/RKSoftware.DAL.Core/)
[![License](https://img.shields.io/badge/license-MIT%20OR%20Apache--2.0-blue.svg)](https://opensource.org/licenses)

## Overview

**RKSoftware.DAL.Core** is a .NET library that provides core contracts for implementing a Data Access Layer (DAL) in .NET projects. It is designed to be lightweight, flexible, and compatible with .NET 9.0.

This package is part of the RK Software Systems ecosystem and serves as the foundation for building robust and reusable DAL implementations.

## Features

- Core contracts for DAL implementations.
- Compatible with .NET 9.0.
- Designed for extensibility and ease of use.
- Fully documented and adheres to modern .NET coding standards.

## Installation

You can install the package via NuGet:

```
dotnet add package RKSoftware.DAL.Core --version 9.0.1
```

Or via the NuGet Package Manager in Visual Studio.

## Usage

This library contains interfaces that need to be implemented by your DAL classes according to the underlying data source (e.g., SQL Server, MongoDB, etc.). The core interfaces include:
- `IQueryStorage`:
  - This service is used to query Readonly storage.
  - Implementation should be thread-safe.
- `IReadonlyStorage` - This storage can be used to perform only READ operations
- `IStorage`:
  - This service is used as a READ / WRITE Storage abstraction
  - This service implements `IReadonlyStorage`
- `ITransactionalStorage`:
  - This service abstracts storage that supports Transactions.
  - Inherits from `IStorage`

# Documentation

The XML documentation file is generated during the build process and can be found in the `bin` directory of your project. It provides detailed information about the available classes, interfaces, and methods.

## Repository

The source code for this package is available on GitHub:

[RKSoftware.DAL GitHub Repository](https://github.com/rk-software-systems/rk-dal)

Feel free to contribute, report issues, or suggest features.

## License

This package is licensed under the **MIT OR Apache-2.0** license. You may choose either license to use this package. See the [LICENSE](https://opensource.org/licenses) file for more details.


## Tags

- DAL
- RKSoftware
- .NET 9
- Data Access Layer