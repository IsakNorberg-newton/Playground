# LINQ Conversion and Export Operations

## Overview

Conversion operations in LINQ transform sequences between different collection types and interfaces. They enable materialization (converting lazy queries into concrete collections), type casting, and changing the compile-time interface of sequences. These operations are crucial for performance optimization, interoperability, and controlling execution timing.

## Materialization Operations

### ToList - List Collection

```csharp
var empList = employees.Take(10).ToList();
System.Console.WriteLine($"ToList count: {empList.Count}");
```

**What it does:**
- Executes the query **immediately**
- Stores results in `List<T>`
- Enables efficient random access and modification

**Signature:**
```csharp
List<TSource> ToList<TSource>(this IEnumerable<TSource> source)
```

**When to use:**
```csharp
// Multiple enumeration needed
var list = query.ToList();
Process(list);
Analyze(list);  // No re-execution

// Need indexing
var list = query.ToList();
var third = list[2];  // O(1) access

// Need mutation
var list = query.ToList();
list.Add(newItem);
list.RemoveAt(0);
```

### ToArray - Array

```csharp
var empArray = employees.Skip(10).Take(5).ToArray();
System.Console.WriteLine($"ToArray length: {empArray.Length}");
```

**What it does:**
- Executes query immediately
- Stores results in fixed-size array
- Most efficient for iteration

**Signature:**
```csharp
TSource[] ToArray<TSource>(this IEnumerable<TSource> source)
```

**ToList vs ToArray:**
| Feature | ToList | ToArray |
|---------|--------|---------|
| Mutability | Can add/remove | Fixed size |
| Access | `list[i]` | `array[i]` |
| Iteration speed | Fast | Fastest |
| Memory | Slightly more | Minimal |
| Flexibility | More | Less |

**When to use ToArray:**
```csharp
// Fixed size collection
var snapshot = data.ToArray();

// Maximum performance iteration
foreach (var item in array) { }  // Slightly faster than List

// Interop with APIs requiring arrays
public void Process(Employee[] employees) { }
Process(query.ToArray());
```

## Dictionary Operations

### ToDictionary - Key-Value Mapping

```csharp
var dict = employees
    .SelectMany(e => e.CreditCards.Select(cc => new { cc.Issuer, Employee = e }))
    .GroupBy(x => x.Issuer)
    .ToDictionary(
        g => g.Key,
        g => g.Select(x => x.Employee)
              .Where(emp => emp.CreditCards.Any(cc => cc.Issuer == g.Key))
              .ToList()
    );

if (dict.Any())
{
    var sampleIssuer = dict.Keys.First();
    System.Console.WriteLine($"ToDictionary by issuer example: found issuer {sampleIssuer} with {dict[sampleIssuer].Count} employees");
}
```

**What it does:**
- Creates `Dictionary<TKey, TValue>`
- Keys must be unique (throws on duplicate)
- O(1) key lookup

**Signature:**
```csharp
Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(
    this IEnumerable<TSource> source,
    Func<TSource, TKey> keySelector)

Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(
    this IEnumerable<TSource> source,
    Func<TSource, TKey> keySelector,
    Func<TSource, TElement> elementSelector)

Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(
    this IEnumerable<TSource> source,
    Func<TSource, TKey> keySelector,
    Func<TSource, TElement> elementSelector,
    IEqualityComparer<TKey> comparer)
```

**Basic usage:**
```csharp
// Key = EmployeeId, Value = Employee
var byId = employees.ToDictionary(e => e.EmployeeId);

// Key = EmployeeId, Value = FullName
var names = employees.ToDictionary(
    e => e.EmployeeId,
    e => $"{e.FirstName} {e.LastName}");
```

**Exception on duplicates:**
```csharp
// ❌ Throws ArgumentException if duplicate keys
var dict = employees.ToDictionary(e => e.Role);  // Multiple employees per role!

// ✅ Use GroupBy for multiple values per key
var grouped = employees.GroupBy(e => e.Role)
    .ToDictionary(g => g.Key, g => g.ToList());
```

### ToLookup - Multi-Value Dictionary

```csharp
var lookup = employees.ToLookup(e => e.Role);
// O(1) access, allows multiple values per key
```

**Covered in detail in LINQ_Grouping.md**

## Interface Conversion Operations

### AsEnumerable - To IEnumerable<T>

```csharp
var asEnum = employees
    .Where(e => e.HireDate.Year > 2010)
    .AsEnumerable();

System.Console.WriteLine($"AsEnumerable deferred sequence type: {asEnum.GetType().Name}");
```

**What it does:**
- Changes compile-time type to `IEnumerable<T>`
- Does **not** force execution
- Does **not** create new sequence
- Simply casts the reference

**Signature:**
```csharp
IEnumerable<TSource> AsEnumerable<TSource>(this IEnumerable<TSource> source)
```

**When to use:**
```csharp
// Force LINQ-to-Objects on IQueryable
var query = dbContext.Employees
    .Where(e => e.Department == "IT")  // SQL query
    .AsEnumerable()                     // Switch to LINQ-to-Objects
    .Where(e => ComplexClientMethod(e)); // Client-side evaluation

// Hide specific collection methods
List<int> list = new List<int> { 1, 2, 3 };
IEnumerable<int> sequence = list.AsEnumerable();
// sequence.Add(4);  // Compile error - only IEnumerable interface
```

### AsQueryable - To IQueryable<T>

```csharp
var asQuery = employees
    .AsQueryable()
    .Where(e => e.Role == WorkRole.Veterinarian)
    .AsQueryable();

System.Console.WriteLine($"AsQueryable sequence type: {asQuery.GetType().Name}");
```

**What it does:**
- Changes compile-time type to `IQueryable<T>`
- Wraps `IEnumerable<T>` in queryable wrapper
- Does **not** create query provider for in-memory sequences
- Enables expression tree building

**Signature:**
```csharp
IQueryable<TElement> AsQueryable<TElement>(this IEnumerable<TElement> source)
```

**Important distinction:**
```csharp
// ❌ Doesn't magically create SQL provider
var inMemory = employees.ToList();
var query = inMemory.AsQueryable();  // Still in-memory, just IQueryable interface

// ✅ Real query provider (EF Core)
var query = dbContext.Employees.AsQueryable();  // Translates to SQL
```

## IEnumerable vs IQueryable

### Key Differences

| Aspect | IEnumerable<T> | IQueryable<T> |
|--------|----------------|---------------|
| Namespace | System.Collections.Generic | System.Linq |
| Execution | LINQ-to-Objects (in-memory) | Provider-specific (SQL, etc.) |
| Query type | Delegates (Func<>) | Expression trees (Expression<>) |
| Deferred | Yes | Yes |
| Where | `Where(Func<T, bool>)` | `Where(Expression<Func<T, bool>>)` |
| Use case | In-memory collections | Databases, remote services |

### How They Work

**IEnumerable<T>:**
```csharp
// Delegate - executed in .NET
IEnumerable<Employee> query = employees.Where(e => e.HireDate.Year > 2020);
// Predicate is C# delegate executed for each item
```

**IQueryable<T>:**
```csharp
// Expression tree - translated to SQL
IQueryable<Employee> query = dbContext.Employees.Where(e => e.HireDate.Year > 2020);
// Predicate is expression tree translated to: WHERE YEAR(HireDate) > 2020
```

### When to Use Each

**Use IEnumerable (AsEnumerable):**
- Working with in-memory collections
- After fetching data from database
- When provider can't translate operation
- Need client-side functions

```csharp
var results = dbContext.Employees
    .Where(e => e.Department == "IT")   // SQL
    .AsEnumerable()                      // Switch to in-memory
    .Where(e => CustomValidation(e));   // Client-side C# method
```

**Use IQueryable:**
- Working with databases (EF Core, LINQ to SQL)
- Remote data sources (OData)
- Want provider to translate queries
- Push filtering/sorting to data source

```csharp
var results = dbContext.Employees
    .Where(e => e.Department == "IT")     // SQL WHERE clause
    .OrderBy(e => e.LastName)             // SQL ORDER BY
    .Take(10)                             // SQL TOP 10
    .ToList();                            // Execute query
```

## Functional Programming Concepts

### 1. Materialization vs Lazy Evaluation

```csharp
// Lazy - not executed
var query = employees.Where(e => e.HireDate.Year > 2020);

// Materialized - executed immediately
var list = employees.Where(e => e.HireDate.Year > 2020).ToList();
```

### 2. Snapshots

Materialization creates point-in-time snapshot:

```csharp
var original = new List<int> { 1, 2, 3 };
var snapshot = original.AsEnumerable();  // View
var materialized = original.ToList();    // Copy

original.Add(4);

snapshot.Count();      // 4 - sees changes
materialized.Count();  // 3 - snapshot before change
```

### 3. Composition Control

Interface conversion controls query composition:

```csharp
IQueryable<Employee> dbQuery = dbContext.Employees;  // Database

// Compose on database
var filtered = dbQuery.Where(e => e.Role == WorkRole.Veterinarian);  // SQL

// Switch to in-memory
var inMemory = filtered.AsEnumerable();

// Compose in-memory
var processed = inMemory.Where(e => ComplexLogic(e));  // C#
```

### 4. Immutability

Conversions create new collections (don't modify source):

```csharp
var original = employees;
var list = original.ToList();

list.Add(newEmployee);
// original unchanged
```

## Conversion Patterns

### Pattern 1: Avoid Multiple Enumeration

```csharp
// ❌ Bad - enumerates twice
var query = employees.Where(predicate);
var count = query.Count();
var items = query.ToList();

// ✅ Good - enumerate once
var items = employees.Where(predicate).ToList();
var count = items.Count;
```

### Pattern 2: Lookup Table

```csharp
// Quick lookups
var byId = employees.ToDictionary(e => e.EmployeeId);
var employee = byId[targetId];  // O(1)
```

### Pattern 3: Switch to Client-Side

```csharp
// Database operations
var dbResults = dbContext.Orders
    .Where(o => o.Date >= startDate)
    .AsEnumerable()  // Switch to client
    .Where(o => ComplexClientLogic(o))  // Client-side
    .ToList();
```

### Pattern 4: Cache Results

```csharp
// Expensive query
private List<Employee> _cachedEmployees;
public List<Employee> GetEmployees()
{
    return _cachedEmployees ??= 
        expensiveQuery.ToList();
}
```

### Pattern 5: Interface Hiding

```csharp
public IEnumerable<Item> GetItems()
{
    var list = GetInternalList();
    return list.AsEnumerable();  // Hide List methods
}
```

## Performance Considerations

### Materialization Cost

```csharp
// ❌ Materializes entire sequence
var all = hugeSequence.ToList();

// ✅ Only materializes needed elements
var some = hugeSequence.Take(100).ToList();
```

### Dictionary Overhead

```csharp
// Efficient for repeated lookups
var dict = employees.ToDictionary(e => e.Id);
for (int i = 0; i < 1000; i++)
{
    var emp = dict[someId];  // O(1)
}

// Less efficient for single lookup
var dict = employees.ToDictionary(e => e.Id);
var emp = dict[someId];  // Overhead of building dictionary
```

### List vs Array Performance

```csharp
// Slight performance difference
var array = Enumerable.Range(1, 1000000).ToArray();
var list = Enumerable.Range(1, 1000000).ToList();

// Array iteration: ~1-5% faster
// List iteration: Slightly slower but more flexible
```

## Common Mistakes

### 1. Materializing Too Early

```csharp
// ❌ Bad - loads all data before filtering
var all = dbContext.Employees.ToList();
var filtered = all.Where(e => e.Department == "IT");

// ✅ Good - filters in database
var filtered = dbContext.Employees
    .Where(e => e.Department == "IT")
    .ToList();
```

### 2. Duplicate Keys in ToDictionary

```csharp
// ❌ Throws on duplicate
var dict = employees.ToDictionary(e => e.Role);

// ✅ Use GroupBy or ToLookup
var byRole = employees.ToLookup(e => e.Role);
```

### 3. Assuming AsQueryable Creates Provider

```csharp
var list = new List<Employee>();
var query = list.AsQueryable();
// Still executes in-memory, not in database!
```

### 4. Multiple Materialization

```csharp
// ❌ Executes query twice
var query = dbContext.Employees.Where(predicate);
var list1 = query.ToList();
var list2 = query.ToList();

// ✅ Materialize once
var list = query.ToList();
```

## Conclusion

LINQ conversion and export operations control execution and collection types:

- ✅ **ToList**: Materialize to mutable list
- ✅ **ToArray**: Materialize to fixed-size array
- ✅ **ToDictionary**: Create key-value lookup
- ✅ **AsEnumerable**: Switch to LINQ-to-Objects
- ✅ **AsQueryable**: Expose IQueryable interface
- ✅ **Immediate execution**: Materialization operations execute immediately
- ✅ **Performance control**: Choose when and how to execute queries

The examples in [LinqConversionExport.cs](LinqConversionExport.cs) demonstrate:
- ToList and ToArray for materialization
- ToDictionary for creating issuer-to-employees mapping
- AsEnumerable and AsQueryable for interface conversion
- Comments explaining IEnumerable vs IQueryable differences

Conversion operations are essential for controlling query execution timing, optimizing performance by choosing appropriate collection types, and managing the boundary between provider-translated queries and in-memory operations.
