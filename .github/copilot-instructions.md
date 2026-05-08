- @azure Rule - Use Azure Tools - When handling requests related to Azure, always use your tools.
- @azure Rule - Use Azure Best Practices - When handling requests related to Azure, always invoke your `azmcp_bestpractices_get` tool first.
- @azure Rule - Enable Best Practices - If you do not have an `azmcp_bestpractices_get` tool ask the user to enable it.

# RK DAL Project Guidelines

## Solution Overview

Three library projects and two test projects in `RKSoftware.DAL.sln`:

| Project | Role |
|---|---|
| `RKSoftware.DAL.Core` | Interface contracts only — no dependencies |
| `RKSoftware.DAL.EntityFramework` | EF Core implementation |
| `RKSoftware.DAL.InMemory` | In-memory `ICollection`-based implementation |
| `Tests/RKSoftware.DAL.EntityFramework.Tests` | MSTest suite for EF project |
| `Tests/RKSoftware.DAL.InMemory.Tests` | MSTest suite for InMemory project |

- Target framework: **`net10.0`**
- Key packages: `Microsoft.EntityFrameworkCore` 10.0.x, `Microsoft.Extensions.DependencyInjection.Abstractions` 10.0.x, `MSTest.*` 4.x
- All library projects: `ImplicitUsings enable`, `Nullable enable`, `GenerateDocumentationFile true`, `EnableNETAnalyzers true`, `AnalysisMode AllEnabledByDefault`

## Interface Hierarchy

```
IDisposable
  └─ IReadonlyStorage
       └─ IStorage
            └─ ITransactionalStorage
IQueryStorage  (standalone — thread-safe query abstraction)
```

All entity type parameters use `where T : class`. There is no `IEntity` base type or `new()` constraint.

## Namespaces

Namespaces must match the folder structure exactly:

| Folder | Namespace |
|---|---|
| `RKSoftware.DAL.Core/` | `RKSoftware.DAL.Core` |
| `RKSoftware.DAL.EntityFramework/` | `RKSoftware.DAL.EntityFramework` |
| `RKSoftware.DAL.EntityFramework/EFExtensions/` | `RKSoftware.DAL.EntityFramework.EFExtensions` |
| `RKSoftware.DAL.EntityFramework/RegistrationExtensions/` | `RKSoftware.DAL.EntityFramework.RegistrationExtensions` |
| `RKSoftware.DAL.InMemory/` | `RKSoftware.DAL.InMemory` |
| `RKSoftware.DAL.InMemory/RegistrationExtensions/` | `RKSoftware.DAL.InMemory.RegistrationExtensions` |

## C# Coding Conventions

- **Primary constructors** (C# 12): use on all classes — `EntityFrameworkStorage(DbContext context)`, not a body constructor.
- **Async mutating methods**: always `Task<T>` or `Task<bool>`; always provide two overloads — one with `CancellationToken` and one without. The no-token overload must delegate to the `CancellationToken.None` overload, never duplicate logic.
- **Null checks**: use `ArgumentNullException.ThrowIfNull(param, nameof(param))` exclusively. No manual `if (x == null) throw` patterns.
- **Generic constraints**: `where T : class` on every entity type parameter.
- **XML documentation**: all `public` and `protected` members must have XML doc comments. Non-overriding implementations should reference their interface member with `<see cref="IStorage.AddAsync{T}(T)"/>`.
- **Pragma suppressions**: use sparingly; always include an explanatory comment directly above the pragma explaining why the rule is suppressed.
- **`var`**: prefer explicit types for clarity in non-trivial expressions; `var` is acceptable when the type is obvious from the right-hand side.

## IDisposable Pattern

- **EF classes** (`EntityFrameworkReadonlyStorage` and subclasses): use the full virtual dispose pattern — override `Dispose(bool disposing)`, call `base.Dispose(disposing)`, and call `GC.SuppressFinalize(this)` in the public `Dispose()`.
- **InMemory classes** (`InMemoryReadonlyStorage`): use the simplified pattern — `public void Dispose() => GC.SuppressFinalize(this);`. Suppress `CA1063` with an explanatory comment.

## Dependency Injection Lifetimes

| Service | Lifetime |
|---|---|
| `CollectionStorage` | Singleton |
| `IReadonlyStorage` | Scoped |
| `IStorage` | Scoped |
| `ITransactionalStorage` | Scoped |
| `IQueryStorage` | Singleton |

`IQueryStorage` is registered as a Singleton but creates a **new DI scope per query** internally to resolve a fresh `IReadonlyStorage`. Do not inject `IReadonlyStorage` directly into `IQueryStorage` — always resolve it from a scoped `IServiceProvider`.

## Registration Extension Methods

- Place all DI registration helpers in a `RegistrationExtensions/` subfolder inside the project.
- Methods must be in a `static` class, extend `IServiceCollection`, and return `IServiceCollection` for chaining.
- EF registration class name: `DependencyRegistration`. InMemory registration class name: `InMemoryRegistration`.
- Each method should register exactly one service and delegate to a combined `AddRKEFStorages` / `UseInMemory` method when all services are needed together.

## Architecture Gotchas

- **EF `Set<T>()` always returns `.AsNoTracking()`** — the read-only path never tracks entities. Only write paths (`AddAsync`, `SaveAsync`, `RemoveAsync`) use tracked entities.
- **`IQueryStorage` is thread-safe; `IReadonlyStorage.Set<T>()` is NOT** — `Set<T>()` returns an `IQueryable` with deferred execution. Do not share an `IReadonlyStorage` instance across threads.
- **`CollectionStorage` keys by `typeof(T).Name`** — two entity types with identical class names but different namespaces will collide in the in-memory store.
- **No `ITransactionalStorage` or `IQueryStorage` implementation for InMemory** — only EF provides transactions and the query storage abstraction.
- **EF `EntityFrameworkStorage` uses a static `SemaphoreSlim(1,1)`** for commit serialization — do not remove it or change the lifetime of the semaphore.
- **Transaction pattern**: call `BeginTransaction()`, perform one or more mutations, then call `CommitTransactionAsync()` on success or `ResetTransactionAsync()` on failure.

## Test Conventions

- **Framework**: MSTest — `[TestClass]`, `[TestMethod]`, `Assert.*`, `await Assert.ThrowsExceptionAsync<T>(...)`.
- **EF tests**: use `Microsoft.EntityFrameworkCore.InMemory` provider. Pass `dbName = nameof(TestMethod)` as the in-memory database name to keep tests isolated from each other.
- **InMemory tests**: instantiate storage directly (no DI) for unit tests, or via `ServiceCollection` + `BuildServiceProvider()` for DI tests.
- **Domain entities**: place test entity POCOs in a `Domain/` subfolder (InMemory tests) or `DB/` subfolder (EF tests) inside the test project.
- **Test isolation**: each test method creates its own storage instance or DI scope. Never share state between test methods.
