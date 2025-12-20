# LINQ Conversion and Import Operations

## Overview

Import conversion operations in LINQ enable filtering and casting sequences based on type. They're essential for working with heterogeneous collections, polymorphic hierarchies, and scenarios where you need to extract elements of specific types from mixed sequences. These operations provide type-safe ways to work with inheritance and interface implementations.

## Setup: Working with Derived Types

The example uses employee hierarchies:

```csharp
// Generate temp employees (derived type)
var seeder = new SeedGenerator();
var temps = seeder.ItemsToList<TempEmployee>(100);

// Mix regular employees and temp employees
var empSlice = employees.Take(30).ToList();
var tempSlice = temps.Take(20).ToList();

var mixedEmployees = new List<Employee>(empSlice.Count + tempSlice.Count);
// Interleave both types
```

This creates a mixed sequence:
```
[Employee, TempEmployee, Employee, TempEmployee, ...]
```

## The OfType Operation

### Filtering by Type

```csharp
var onlyDerived = mixedEmployees.OfType<TempEmployee>();

System.Console.WriteLine("OfType<TempEmployee> results (derived instances):");
foreach (var e in onlyDerived)
    System.Console.WriteLine($"{e.FirstName} {e.LastName} - {e.Role} [{e.GetType().Name}]");
```

**What it does:**
- Filters sequence to elements of specified type
- Returns only elements where `element is T` is true
- Safe - never throws
- Skips elements that don't match

**Signature:**
```csharp
IEnumerable<TResult> OfType<TResult>(this IEnumerable source)
```

**How it works:**
```csharp
// Conceptual implementation
public static IEnumerable<TResult> OfType<TResult>(this IEnumerable source)
{
    foreach (var item in source)
    {
        if (item is TResult result)
            yield return result;
    }
}
```

**Type checking:**
```csharp
// Mixed sequence
object[] mixed = { 1, "text", 2, 3.14, "more", 4 };

// Extract only strings
var strings = mixed.OfType<string>();  // ["text", "more"]

// Extract only integers
var ints = mixed.OfType<int>();  // [1, 2, 4]

// Extract only doubles
var doubles = mixed.OfType<double>();  // [3.14]
```

### Inheritance and Interfaces

```csharp
// Base class and derived
class Animal { }
class Dog : Animal { }
class Cat : Animal { }

Animal[] animals = { new Dog(), new Cat(), new Dog() };

// Only dogs
var dogs = animals.OfType<Dog>();  // [Dog, Dog]

// Interface filtering
IEnumerable<IComparable> values = new object[] { 1, "text", 3.14, 5 };
var comparables = values.OfType<IComparable>();  // All (all implement IComparable)
var numbers = values.OfType<int>();  // [1, 5]
```

## The Cast Operation

### Type Casting

```csharp
var castBack = onlyDerived.Cast<Employee>();

System.Console.WriteLine("Cast<TempEmployee> -> Cast<Employee> results:");
foreach (var e in castBack)
{
    System.Console.WriteLine($"{e.FirstName} {e.LastName} - {e.Role} [{e.GetType().Name}]");
    // System.Console.WriteLine(e.FinalDate); // Not accessible from Employee reference
}
```

**What it does:**
- Casts each element to specified type
- **Throws InvalidCastException** if cast fails
- Use when you're certain all elements are castable

**Signature:**
```csharp
IEnumerable<TResult> Cast<TResult>(this IEnumerable source)
```

**How it works:**
```csharp
// Conceptual implementation
public static IEnumerable<TResult> Cast<TResult>(this IEnumerable source)
{
    foreach (var item in source)
    {
        yield return (TResult)item;  // Throws if cast invalid
    }
}
```

**Upcasting (safe):**
```csharp
// Derived to base (always safe)
IEnumerable<TempEmployee> tempEmployees = GetTempEmployees();
IEnumerable<Employee> employees = tempEmployees.Cast<Employee>();
// All TempEmployee instances are Employees
```

**Downcasting (dangerous):**
```csharp
// Base to derived (might fail)
IEnumerable<Employee> employees = GetEmployees();
IEnumerable<TempEmployee> temps = employees.Cast<TempEmployee>();
// ❌ Throws if any Employee is not TempEmployee
```

## OfType vs Cast

### Key Differences

| Feature | OfType | Cast |
|---------|--------|------|
| Behavior | Filters matching types | Casts all elements |
| Invalid cast | Skips element | Throws exception |
| Return count | ≤ source count | = source count |
| Safety | Safe | Can throw |
| Use when | Uncertain of types | Certain all types match |

### Examples

```csharp
object[] mixed = { 1, "text", 2, "more" };

// OfType - safe filtering
var strings = mixed.OfType<string>();  // ["text", "more"]
var ints = mixed.OfType<int>();        // [1, 2]

// Cast - throws on mismatch
var strings = mixed.Cast<string>();  // ❌ Throws on 1
var ints = mixed.Cast<int>();        // ❌ Throws on "text"
```

### When to Use Each

**Use OfType when:**
- Working with heterogeneous collections
- Not all elements are target type
- Want to extract specific types
- Safety is important

```csharp
// Mixed collection
IEnumerable<Animal> animals = GetAnimals();

// Extract only dogs
var dogs = animals.OfType<Dog>();
```

**Use Cast when:**
- Know all elements are target type
- Want exception if assumption wrong (fail fast)
- Upcasting (always safe)

```csharp
// Non-generic collection
ArrayList list = GetOldStyleList();

// All are strings
var strings = list.Cast<string>();

// Upcast derived to base
IEnumerable<TempEmployee> temps = GetTemps();
IEnumerable<Employee> employees = temps.Cast<Employee>();
```

## Functional Programming Concepts

### 1. Type-Based Filtering

`OfType` implements a type-level filter:

```csharp
// Value-based filter
var adults = people.Where(p => p.Age >= 18);

// Type-based filter
var tempEmployees = employees.OfType<TempEmployee>();
```

### 2. Pattern Matching Alternative

In modern C#, can use pattern matching:

```csharp
// OfType<TempEmployee>
var temps1 = employees.OfType<TempEmployee>();

// Equivalent with pattern matching
var temps2 = employees.Where(e => e is TempEmployee)
                      .Select(e => (TempEmployee)e);

// Or with switch expression
var temps3 = employees.Select(e => e switch
{
    TempEmployee temp => temp,
    _ => null
}).Where(t => t is not null);
```

### 3. Type Safety

These operations maintain type safety at runtime:

```csharp
// Compile-time type: Employee
// Runtime type: might be TempEmployee

foreach (Employee emp in employees)
{
    // Can't access TempEmployee.FinalDate
}

// Filter to runtime type
foreach (TempEmployee temp in employees.OfType<TempEmployee>())
{
    // Can access TempEmployee.FinalDate
    Console.WriteLine(temp.FinalDate);
}
```

### 4. Covariance Support

Works with covariant interfaces:

```csharp
IEnumerable<Dog> dogs = GetDogs();
IEnumerable<Animal> animals = dogs;  // Covariant cast

// Can filter back
var dogsAgain = animals.OfType<Dog>();
```

## Type Conversion Patterns

### Pattern 1: Extract Specific Implementations

```csharp
// Get all IDisposable items
var disposables = collection.OfType<IDisposable>();
foreach (var d in disposables)
    d.Dispose();
```

### Pattern 2: Process by Type

```csharp
var shapes = GetShapes();  // Circle, Square, Triangle

var circles = shapes.OfType<Circle>();
var squares = shapes.OfType<Square>();
var triangles = shapes.OfType<Triangle>();

var totalCircleArea = circles.Sum(c => c.Area());
```

### Pattern 3: Legacy Collection Conversion

```csharp
// Old non-generic collection
ArrayList legacy = GetLegacyData();

// Convert to typed sequence
var typed = legacy.Cast<MyType>();

// Process with LINQ
var result = typed.Where(item => item.IsActive)
                  .OrderBy(item => item.Name)
                  .ToList();
```

### Pattern 4: Upcast for Polymorphism

```csharp
IEnumerable<TempEmployee> temps = GetTempEmployees();

// Treat as base class
IEnumerable<Employee> allEmployees = temps.Cast<Employee>()
    .Concat(GetRegularEmployees());
```

### Pattern 5: Safe Downcast

```csharp
// Instead of unsafe cast
var temps = employees.Cast<TempEmployee>();  // Might throw

// Safe alternative
var temps = employees.OfType<TempEmployee>();  // Filters safely
```

### Pattern 6: Type-Based Dispatching

```csharp
foreach (var item in mixedCollection)
{
    switch (item)
    {
        case TempEmployee temp:
            ProcessTemp(temp);
            break;
        case Employee emp:
            ProcessEmployee(emp);
            break;
    }
}

// Or with OfType
var temps = mixedCollection.OfType<TempEmployee>();
var employees = mixedCollection.OfType<Employee>()
    .Except(temps.Cast<Employee>());
```

## Working with Inheritance Hierarchies

### Single-Level Hierarchy

```csharp
class Employee { }
class TempEmployee : Employee { }

IEnumerable<Employee> all = GetAll();

// Extract derived types
var temps = all.OfType<TempEmployee>();
var regular = all.Except(temps.Cast<Employee>());
```

### Multi-Level Hierarchy

```csharp
class Employee { }
class Manager : Employee { }
class Executive : Manager { }

IEnumerable<Employee> all = GetAll();

// Extract managers (includes executives)
var managers = all.OfType<Manager>();

// Extract only executives
var executives = all.OfType<Executive>();

// Extract managers but not executives
var managersOnly = managers.Except(executives.Cast<Manager>());
```

### Interface-Based

```csharp
interface ITemporary { DateTime EndDate { get; } }

class Employee { }
class TempEmployee : Employee, ITemporary { }
class Contractor : Employee, ITemporary { }

IEnumerable<Employee> all = GetAll();

// All temporary workers
var temporary = all.OfType<ITemporary>();

// Only temp employees
var temps = all.OfType<TempEmployee>();

// Only contractors
var contractors = all.OfType<Contractor>();
```

## Performance Considerations

### OfType Performance

```csharp
// O(n) - must check each element
var filtered = collection.OfType<TargetType>();

// Type check is fast (is operator)
// But still must iterate all elements
```

### Cast Performance

```csharp
// O(n) - casts each element
var casted = collection.Cast<TargetType>();

// Cast itself is fast
// But still must process all elements
```

### Materialization

```csharp
// ❌ Multiple enumerations
var temps = employees.OfType<TempEmployee>();
var count = temps.Count();
var list = temps.ToList();  // Filters again

// ✅ Filter once
var temps = employees.OfType<TempEmployee>().ToList();
var count = temps.Count;
```

## Common Mistakes

### 1. Using Cast When OfType Needed

```csharp
// Mixed types
var mixed = GetMixedEmployees();

// ❌ Throws on regular employees
var temps = mixed.Cast<TempEmployee>();

// ✅ Filters safely
var temps = mixed.OfType<TempEmployee>();
```

### 2. Forgetting Runtime Type

```csharp
Employee emp = GetTempEmployee();  // Returns TempEmployee

// ❌ Compile-time type is Employee
var date = emp.FinalDate;  // Compile error

// ✅ Cast to access derived members
if (emp is TempEmployee temp)
    var date = temp.FinalDate;
```

### 3. Assuming Count Unchanged

```csharp
var employees = GetEmployees();  // 100 items

// ❌ Wrong assumption
var temps = employees.OfType<TempEmployee>();
// temps.Count() might be 0-100, not necessarily 100
```

### 4. Unnecessary Cast

```csharp
// Already correct type
IEnumerable<TempEmployee> temps = GetTemps();

// ❌ Unnecessary
var employees = temps.Cast<TempEmployee>();

// ✅ Use as-is or upcast if needed
var employees = temps.Cast<Employee>();
```

## Integration with Modern C#

### Pattern Matching

```csharp
// OfType + pattern matching
var results = collection
    .OfType<Employee>()
    .Select(e => e switch
    {
        TempEmployee temp => $"Temp: {temp.EndDate}",
        Manager mgr => $"Manager: {mgr.Department}",
        _ => $"Employee: {e.Name}"
    });
```

### Is-Pattern

```csharp
// Instead of OfType + Cast
foreach (var item in collection)
{
    if (item is TempEmployee temp)
    {
        // Use temp directly
        Console.WriteLine(temp.FinalDate);
    }
}
```

### Type Patterns in LINQ

```csharp
var temps = employees
    .Where(e => e is TempEmployee)
    .Cast<TempEmployee>();

// Equivalent to
var temps = employees.OfType<TempEmployee>();
```

## Comparison with Other Languages

### Java

```java
// Stream filter by type
stream.filter(TempEmployee.class::isInstance)
      .map(TempEmployee.class::cast)

// LINQ equivalent
employees.OfType<TempEmployee>()
```

### Python

```python
# Filter by type
temps = [e for e in employees if isinstance(e, TempEmployee)]

# LINQ equivalent
temps = employees.OfType<TempEmployee>()
```

## Conclusion

LINQ import conversion operations provide type-safe ways to work with polymorphic collections:

- ✅ **OfType<T>**: Safe filtering by type (no exceptions)
- ✅ **Cast<T>**: Type casting (throws on mismatch)
- ✅ **Type safety**: Runtime type checking
- ✅ **Polymorphism**: Work with inheritance hierarchies
- ✅ **Safe extraction**: Filter derived types from base sequences

The examples in [LinqConversionImport.cs](LinqConversionImport.cs) demonstrate:
- OfType to extract TempEmployee instances from mixed sequence
- Cast to upcast filtered results back to base type
- Type information preserved in runtime
- Safe handling of derived type members

Import conversion operations are essential for working with inheritance, handling heterogeneous collections, and safely extracting specific types from polymorphic sequences in a declarative, type-safe manner.
