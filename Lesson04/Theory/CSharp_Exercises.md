# C# Functional Programming Exercises

## Generics Exercises

### Exercise 1: Generic Result Type (Easy)
Create a generic `Result<T>` type that represents either success with a value or failure with an error message (similar to functional programming's Either/Result types).

**Tasks:**
1. Create `Result<T>` with:
   - `IsSuccess` property
   - `Value` property (only accessible if success)
   - `Error` property (only accessible if failure)
   - Static methods `Success(T value)` and `Failure(string error)`
2. Create extension methods:
   - `Map<TResult>(Func<T, TResult> mapper)` - transforms success value
   - `Bind<TResult>(Func<T, Result<TResult>> binder)` - chains operations
3. Use it to implement a safe division operation that returns `Result<double>`

**Expected concepts:**
- Generic type parameters
- Type constraints
- Generic methods
- Extension methods on generic types

---

### Exercise 2: Generic Collection Operations (Medium)
Implement a generic `Paginated<T>` collection with built-in pagination support.

**Tasks:**
1. Create `Paginated<T>` class with:
   - `IReadOnlyList<T> Items`
   - `int PageNumber`, `int PageSize`, `int TotalCount`, `int TotalPages`
   - Generic constructor that takes `IEnumerable<T>` and pagination parameters
2. Implement generic methods:
   - `Map<TResult>(Func<T, TResult> mapper)` - returns `Paginated<TResult>`
   - `Filter(Func<T, bool> predicate)` - returns filtered `Paginated<T>`
3. Add a static generic method `Create<T>(IEnumerable<T> source, int pageNumber, int pageSize)` where T : IComparable<T>

**Expected concepts:**
- Generic classes
- Type constraints (class, struct, IComparable<T>, new())
- Generic methods with multiple type parameters
- Covariance/contravariance concepts

---
