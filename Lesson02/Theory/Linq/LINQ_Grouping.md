# LINQ Grouping Operations

## Overview

Grouping in LINQ organizes elements into groups based on a key. It's equivalent to SQL's `GROUP BY` and is essential for categorizing data, computing per-group statistics, and analyzing data by categories. Grouping is a powerful functional programming pattern that transforms flat sequences into hierarchical structures.

## The GroupBy Operation

### Basic Grouping

```csharp
var byRole = employees.GroupBy(e => e.Role)
    .Select(g => new { Role = g.Key, Count = g.Count(), Sample = g.Take(5).ToList() });

System.Console.WriteLine("Employees grouped by Role:");
foreach (var grp in byRole)
{
    System.Console.WriteLine($"{grp.Role} - {grp.Count} employees (showing up to 5):");
    foreach (var emp in grp.Sample)
        System.Console.WriteLine($"  {emp.FirstName} {emp.LastName} - Hired: {emp.HireDate:d}");
}
```

**What it does:**
- Groups employees by their `Role`
- Returns `IEnumerable<IGrouping<WorkRole, Employee>>`
- Each group has a `Key` (the role) and contains all employees with that role

**Key Concepts:**
- **Key**: The value to group by (`WorkRole`)
- **IGrouping<TKey, TElement>**: Represents one group
- **g.Key**: Access the grouping key
- **g**: The collection of elements in that group

### Understanding IGrouping

`GroupBy` returns `IEnumerable<IGrouping<TKey, TElement>>`:

```csharp
public interface IGrouping<out TKey, out TElement> : IEnumerable<TElement>
{
    TKey Key { get; }
}
```

Each `IGrouping` is both:
1. **A key** (`g.Key`)
2. **A sequence** of elements in that group

## Grouping with Transformations

### Group and Aggregate

```csharp
var byHireYear = employees.GroupBy(e => e.HireDate.Year)
    .OrderBy(g => g.Key)
    .Select(g => new 
    { 
        Year = g.Key, 
        Count = g.Count(), 
        Names = g.Select(e => $"{e.FirstName} {e.LastName}").Take(3) 
    });

System.Console.WriteLine("Employees grouped by Hire Year (showing first 3 names):");
foreach (var y in byHireYear.Take(10))
{
    System.Console.WriteLine($"{y.Year}: {y.Count} employees");
    foreach (var n in y.Names)
        System.Console.WriteLine($"  {n}");
}
```

**What it does:**
- Groups by hire year
- Orders groups by year
- For each group: counts employees and gets sample names
- Demonstrates composition: grouping + ordering + projection

## The ToLookup Operation

### Immediate Grouping

```csharp
var lookupByRole = employees.ToLookup(e => e.Role);
System.Console.WriteLine("ToLookup by Role:");
foreach (var roleGroup in lookupByRole)
{
    System.Console.WriteLine($"{roleGroup.Key} - {roleGroup.Count()} employees (showing up to 3):");
    foreach (var emp in roleGroup.Take(3))
        System.Console.WriteLine($"  {emp.FirstName} {emp.LastName} - Hired: {emp.HireDate:d}");
}
```

**What it does:**
- Creates an `ILookup<TKey, TElement>`
- **Immediately executes** (not lazy like `GroupBy`)
- Provides fast key-based lookup

### GroupBy vs ToLookup

| GroupBy | ToLookup |
|---------|----------|
| Deferred execution | Immediate execution |
| Returns `IEnumerable<IGrouping<K,V>>` | Returns `ILookup<K,V>` |
| Must enumerate to use | Ready to use immediately |
| Evaluated each time iterated | Evaluated once |
| For LINQ queries | For repeated lookups |

**When to use ToLookup:**
```csharp
var lookup = employees.ToLookup(e => e.Role);

// Efficient repeated queries
var vets = lookup[WorkRole.Veterinarian];
var managers = lookup[WorkRole.Management];
var careStaff = lookup[WorkRole.AnimalCare];
```

**When to use GroupBy:**
```csharp
// One-time query
var grouped = employees
    .GroupBy(e => e.Role)
    .Select(g => new { Role = g.Key, Count = g.Count() });
```

## Grouping Patterns

### Pattern 1: Count Per Group

```csharp
var counts = employees
    .GroupBy(e => e.Role)
    .Select(g => new { Role = g.Key, Count = g.Count() });
```

### Pattern 2: Sum Per Group

```csharp
var cardsByRole = employees
    .GroupBy(e => e.Role)
    .Select(g => new 
    { 
        Role = g.Key, 
        TotalCards = g.Sum(emp => emp.CreditCards.Count) 
    });
```

### Pattern 3: Average Per Group

```csharp
var avgSeniorityByRole = employees
    .GroupBy(e => e.Role)
    .Select(g => new 
    { 
        Role = g.Key, 
        AvgYearsEmployed = g.Average(emp => 
            (DateTime.Now - emp.HireDate).TotalDays / 365) 
    });
```

### Pattern 4: Top N Per Group

```csharp
var topByRole = employees
    .GroupBy(e => e.Role)
    .Select(g => new 
    { 
        Role = g.Key, 
        MostSenior = g.OrderBy(emp => emp.HireDate).Take(3) 
    });
```

### Pattern 5: Filter Groups

```csharp
// Only groups with more than 10 employees
var largeGroups = employees
    .GroupBy(e => e.Role)
    .Where(g => g.Count() > 10);
```

### Pattern 6: Multiple Grouping Keys

```csharp
// Group by year AND role
var byYearAndRole = employees
    .GroupBy(e => new { Year = e.HireDate.Year, e.Role })
    .Select(g => new 
    { 
        g.Key.Year, 
        g.Key.Role, 
        Count = g.Count() 
    });
```

## Functional Programming Concepts

### 1. Transformation

Grouping transforms a flat sequence into a hierarchical structure:

```csharp
// Flat
IEnumerable<Employee> employees

// Hierarchical
IEnumerable<IGrouping<WorkRole, Employee>> groups
```

### 2. Composability

Grouping composes with other operations:

```csharp
var result = employees
    .Where(e => e.HireDate.Year >= 2020)    // Filter
    .GroupBy(e => e.Role)                    // Group
    .OrderBy(g => g.Count())                 // Order groups
    .Select(g => new { g.Key, g.Count() });  // Project
```

### 3. Immutability

Grouping doesn't modify the original sequence:

```csharp
var original = employees;
var grouped = employees.GroupBy(e => e.Role);

// original is unchanged
// grouped provides a different view
```

### 4. Lazy Evaluation

`GroupBy` uses deferred execution:

```csharp
var query = employees.GroupBy(e => e.Role);  // Not executed yet

foreach (var group in query)  // NOW it groups
{
    // Process groups
}
```

## Grouping with Query Syntax

LINQ query syntax supports grouping:

```csharp
var query = from e in employees
            group e by e.Role into roleGroup
            select new { Role = roleGroup.Key, Count = roleGroup.Count() };
```

**Equivalent method syntax:**
```csharp
var query = employees
    .GroupBy(e => e.Role)
    .Select(roleGroup => new { Role = roleGroup.Key, Count = roleGroup.Count() });
```

**The `into` clause** creates a range variable for the group.

## Performance Considerations

### GroupBy
- **O(n)**: Must process all elements
- **Memory**: Buffers all elements (not streamable)
- **Deferred**: Executes when enumerated
- **Multiple enumeration**: Re-groups each time

### ToLookup
- **O(n)**: Processes all elements once
- **Memory**: Stores all groups in memory
- **Immediate**: Executes immediately
- **Fast lookups**: O(1) key access

**Choose based on usage:**
- One-time grouping → `GroupBy`
- Repeated key lookups → `ToLookup`

## Common Patterns

### SQL-Style Aggregation

```csharp
// SELECT Role, COUNT(*), AVG(Year) 
// FROM Employees 
// GROUP BY Role

var stats = employees
    .GroupBy(e => e.Role)
    .Select(g => new 
    {
        Role = g.Key,
        Count = g.Count(),
        AvgYear = g.Average(e => e.HireDate.Year)
    });
```

### Nested Grouping

```csharp
// Group by role, then by year within each role
var nested = employees
    .GroupBy(e => e.Role)
    .Select(roleGroup => new
    {
        Role = roleGroup.Key,
        ByYear = roleGroup
            .GroupBy(e => e.HireDate.Year)
            .Select(yearGroup => new 
            { 
                Year = yearGroup.Key, 
                Count = yearGroup.Count() 
            })
    });
```

### Hierarchical Reports

```csharp
var report = employees
    .GroupBy(e => e.Role)
    .Select(roleGroup => new
    {
        Role = roleGroup.Key,
        TotalEmployees = roleGroup.Count(),
        TotalCards = roleGroup.Sum(e => e.CreditCards.Count),
        OldestHireDate = roleGroup.Min(e => e.HireDate),
        NewestHireDate = roleGroup.Max(e => e.HireDate),
        SampleEmployees = roleGroup.Take(5).Select(e => e.FirstName)
    });
```

## Common Mistakes

### 1. Accessing Group Directly

```csharp
// ❌ Wrong - group is IEnumerable, must enumerate
var names = groups.FirstOrDefault(g => g.Key == WorkRole.Veterinarian).FirstName;

// ✅ Correct - access elements in group
var vets = groups.FirstOrDefault(g => g.Key == WorkRole.Veterinarian);
if (vets != null)
{
    var names = vets.Select(e => e.FirstName);
}
```

### 2. Multiple Enumeration

```csharp
var groups = employees.GroupBy(e => e.Role);

// ❌ Expensive - groups twice
var count1 = groups.Count();
var count2 = groups.Sum(g => g.Count());

// ✅ Better - materialize once
var groupsList = employees.GroupBy(e => e.Role).ToList();
var count1 = groupsList.Count;
var count2 = groupsList.Sum(g => g.Count());
```

### 3. Forgetting Key in Multi-Key Grouping

```csharp
// ❌ Anonymous type requires Key access
var multi = employees.GroupBy(e => new { e.Role, Year = e.HireDate.Year });
// How to access later? Must use .Key.Role and .Key.Year

foreach (var g in multi)
{
    Console.WriteLine($"{g.Key.Role} - {g.Key.Year}");
}
```

## Conclusion

Grouping operations in LINQ provide powerful data categorization:

- ✅ **GroupBy**: Deferred execution grouping
- ✅ **ToLookup**: Immediate execution with fast lookups
- ✅ **IGrouping<TKey, TElement>**: Each group is both key and sequence
- ✅ **Composable**: Combine with aggregation, filtering, ordering
- ✅ **Functional**: Transforms flat data into hierarchical structure

The examples in [LinqGrouping.cs](LinqGrouping.cs) show:
- Grouping by role with counts and samples
- Grouping by computed key (hire year) with ordering
- Using ToLookup for efficient repeated queries

Grouping is essential for data analysis, reporting, and any scenario where you need to categorize and aggregate data by common characteristics.
