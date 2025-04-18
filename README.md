# RKSoftware.DAL

This repository provides a comprehensive Data Access Layer (DAL) solution for .NET 9 applications. It includes multiple projects designed to offer flexibility and scalability for various data storage implementations.

## Projects

### 1. **RKSoftware.DAL.Core**
- **Description**: This project contains the core contracts for implementing a Data Access Layer. It defines the foundational interfaces and abstractions that can be used across different storage implementations.
- **Target Framework**: .NET 9
- **Key Features**:
  - Provides reusable contracts for DAL implementations.
  - Ensures consistency and standardization across different storage solutions.
- **Dependencies**: None.

---

### 2. **RKSoftware.DAL.EntityFramework**
- **Description**: This project implements the `RKSoftware.DAL.Core` contracts using Entity Framework Core. It is designed for applications that require robust and scalable database solutions.
- **Target Framework**: .NET 9
- **Key Features**:
  - Leverages Entity Framework Core for database interactions.
  - Supports advanced querying and database management features.
- **Dependencies**:
  - `Microsoft.EntityFrameworkCore` (v9.0.4)
  - References `RKSoftware.DAL.Core`.

---

### 3. **RKSoftware.DAL.InMemory**
- **Description**: This project provides an in-memory implementation of the `RKSoftware.DAL.Core` contracts. It is ideal for testing and lightweight applications that do not require a persistent database.
- **Target Framework**: .NET 9
- **Key Features**:
  - Uses in-memory `ICollection` for data storage.
  - Simplifies testing by eliminating the need for a database.
- **Dependencies**:
  - `Microsoft.Extensions.DependencyInjection.Abstractions` (v9.0.4)
  - References `RKSoftware.DAL.Core`.

---

## Licensing
This repository is licensed under the MIT or Apache-2.0 license, allowing for flexible use in both open-source and commercial projects.

## Additional Information
For more details, visit the [official repository page](https://github.com/rk-software-systems/rk-dal).
