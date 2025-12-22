# LINQ Projection Operations

## Overview

Projection in LINQ transforms elements from one form to another. It's the functional programming concept of **mapping**—applying a transformation function to each element in a sequence. Projection is essential for shaping data, extracting properties, and flattening nested structures.

## The Select Operation

### Basic Projection

```csharp
var projected = employees.Select(e => new { FullName = $"{e.FirstName} {e.LastName}", e.Role });
System.Console.WriteLine("Projected employees (FullName, Role):");
projected.Take(10).ToList().ForEach(p => System.Console.WriteLine($"{p.FullName} - {p.Role}"));
```

**What it does:**
- Transforms each `Employee` into an anonymous type
- Creates a new shape with `FullName` and `Role` properties
- Returns `IEnumerable<TAnonymous>`

**Functional Characteristics:**
- **Map operation**: One-to-one transformation
- **Non-destructive**: Original collection unchanged
- **Type transformation**: `IEnumerable<Employee>` → `IEnumerable<Anonymous>`

### How Select Works

Behind the scenes:
```csharp
public static IEnumerable<TResult> Select<TSource, TResult>(
    this IEnumerable<TSource> source,
    Func<TSource, TResult> selector)
{
    foreach (var item in source)
    {
        yield return selector(item);
    }
}
```

It's a **higher-order function** that applies a transformation to each element.

## Select Transformation Patterns

### 1. Anonymous Types

```csharp
var projected = employees.Select(e => new 
{ 
    FullName = $"{e.FirstName} {e.LastName}", 
    e.Role 
});
```

**Benefits:**
- No need to create a class
- IntelliSense support
- Strongly typed
- Concise syntax

### 2. Existing Types

```csharp
var names = employees.Select(e => e.FirstName);  // IEnumerable<string>
var dates = employees.Select(e => e.HireDate);   // IEnumerable<DateTime>
```

### 3. Named Types

```csharp
public record EmployeeSummary(string Name, WorkRole Role);

var summaries = employees.Select(e => new EmployeeSummary(
    $"{e.FirstName} {e.LastName}",
    e.Role
));
```

### 4. With Index

```csharp
var indexed = employees.Select((e, index) => new 
{ 
    Position = index + 1,
    Name = $"{e.FirstName} {e.LastName}"
});
```

The second overload provides the element's index.

## The SelectMany Operation

### Flattening Nested Collections

```csharp
var allCards = employees.SelectMany(e => e.CreditCards, 
    (e, card) => new 
    { 
        Employee = $"{e.FirstName} {e.LastName}", 
        card.Number, 
        card.Issuer 
    });
System.Console.WriteLine("All credit cards (Employee - Number - Issuer) (showing 10):");
allCards.Take(10).ToList().ForEach(c => System.Console.WriteLine($"{c.Employee} - {c.Number} - {c.Issuer}"));
```

**What it does:**
- Each employee has a collection of credit cards
- `SelectMany` flattens all cards into a single sequence
- Combines employee data with each card
- Returns `IEnumerable<Anonymous>` with one item per card

### Understanding SelectMany

**Without SelectMany (Select creates nested structure):**
```csharp
// This gives IEnumerable<IEnumerable<CreditCard>>
var nested = employees.Select(e => e.CreditCards);

// Must iterate twice
foreach (var cards in nested)
{
    foreach (var card in cards)
    {
        // Access card
    }
}
```

**With SelectMany (flattens to single sequence):**
```csharp
// This gives IEnumerable<CreditCard>
var flat = employees.SelectMany(e => e.CreditCards);

// Iterate once
foreach (var card in flat)
{
    // Access card directly
}
```

### SelectMany with Result Selector

```csharp
employees.SelectMany(
    e => e.CreditCards,                     // Collection selector
    (e, card) => new { e.FirstName, card }  // Result selector (combines)
);
```

**Parameters:**
1. **Collection selector**: Function that returns a collection for each element
2. **Result selector**: Function that combines the outer element with each inner element

## Projection Use Cases

### 1. Data Transfer Objects (DTOs)

```csharp
public record EmployeeDto(string Name, string Role, int CardCount);

var dtos = employees.Select(e => new EmployeeDto(
    $"{e.FirstName} {e.LastName}",
    e.Role.ToString(),
    e.CreditCards.Count
));
```

### 2. Property Extraction

```csharp
var firstNames = employees.Select(e => e.FirstName);
var roles = employees.Select(e => e.Role).Distinct();
```

### 3. Calculated Fields

```csharp
var withSeniority = employees.Select(e => new
{
    e.FirstName,
    e.LastName,
    YearsEmployed = (DateTime.Now - e.HireDate).Days / 365
});
```

### 4. Flattening Hierarchies

```csharp
// Get all cards from all employees in one flat list
var allCards = employees.SelectMany(e => e.CreditCards);

// Count total cards across all employees
var totalCards = employees.SelectMany(e => e.CreditCards).Count();
```

## Functional Programming Concepts

### 1. Map (Select)

In functional programming, `Select` is the **map** operation:

```csharp
// Map transforms each element
var doubled = numbers.Select(n => n * 2);
var squared = numbers.Select(n => n * n);
```

**Characteristics:**
- One-to-one transformation
- Preserves sequence length
- Type can change

### 2. FlatMap (SelectMany)

`SelectMany` is the **flatMap** operation:

```csharp
// Each element maps to multiple elements, then flattened
var words = sentences.SelectMany(s => s.Split(' '));
```

**Characteristics:**
- One-to-many transformation
- Flattens nested structures
- Changes sequence length

### 3. Composition

Projections compose naturally:

```csharp
var result = employees
    .Where(e => e.Role == WorkRole.Veterinarian)
    .Select(e => new { e.FirstName, e.LastName })
    .OrderBy(e => e.LastName);
```

Each operation returns `IEnumerable`, enabling chaining.

### 4. Immutability

Projection creates new sequences without modifying originals:

```csharp
var original = employees;
var projected = employees.Select(e => e.FirstName);

// original is completely unchanged
// projected is a new view
```

## Select vs SelectMany

| Aspect | Select | SelectMany |
|--------|--------|------------|
| **Purpose** | Transform elements | Flatten collections |
| **Input** | One element | One element with nested collection |
| **Output** | One element | Multiple elements (flattened) |
| **Result Length** | Same as source | Sum of all inner collections |
| **Use Case** | Shape/transform data | Flatten hierarchies |

## Practical Patterns

### Pattern 1: Combining Select and SelectMany

```csharp
// Get all cards with employee name
var cardsWithOwner = employees
    .SelectMany(e => e.CreditCards
        .Select(card => new { Owner = e.FirstName, Card = card }));
```

### Pattern 2: Conditional Projection

```csharp
var masked = employees.Select(e => new
{
    e.FirstName,
    e.LastName,
    CardNumbers = e.CreditCards.Select(c => MaskCardNumber(c.Number))
});
```

### Pattern 3: Nested SelectMany

```csharp
// Get all cards from all employees in all departments
var allCards = departments
    .SelectMany(d => d.Employees)
    .SelectMany(e => e.CreditCards);
```

### Pattern 4: Query Syntax

LINQ query syntax uses `select` for projection:

```csharp
var query = from e in employees
            select new { e.FirstName, e.LastName };

// Equivalent to:
var query = employees.Select(e => new { e.FirstName, e.LastName });
```

## Performance Considerations

### Select
- **O(n)**: Processes each element once
- **Lazy**: Only executes when enumerated
- **Minimal overhead**: Just calls selector function

### SelectMany
- **O(n * m)**: n outer elements × m inner elements (average)
- **Lazy**: Only executes when enumerated
- **Flattening overhead**: Creates single sequence from multiple

## Common Patterns

### Pattern: Extract Single Property

```csharp
var emails = users.Select(u => u.Email);
```

### Pattern: Transform to DTO

```csharp
var dtos = entities.Select(e => new EntityDto
{
    Id = e.Id,
    Name = e.Name,
    Status = e.IsActive ? "Active" : "Inactive"
});
```

### Pattern: Flatten Nested Data

```csharp
var allItems = orders.SelectMany(o => o.Items);
```

### Pattern: Cross Join with SelectMany

```csharp
var combinations = colors.SelectMany(c => sizes, (c, s) => new { Color = c, Size = s });
```

## Common Mistakes

### 1. Using Select Instead of SelectMany

```csharp
// ❌ Wrong - creates nested IEnumerable<IEnumerable<Card>>
var cards = employees.Select(e => e.CreditCards);

// ✅ Correct - creates flat IEnumerable<Card>
var cards = employees.SelectMany(e => e.CreditCards);
```

### 2. Forgetting to Materialize

```csharp
// ❌ Query executed twice
var query = employees.Select(e => ExpensiveOperation(e));
var count = query.Count();      // Executes query
var first = query.First();      // Executes query again

// ✅ Execute once
var results = employees.Select(e => ExpensiveOperation(e)).ToList();
var count = results.Count;
var first = results.First();
```

### 3. Side Effects in Selector

```csharp
// ❌ Bad - side effects in selector
var query = employees.Select(e => 
{
    Console.WriteLine(e.FirstName);  // Side effect!
    return e;
});

// Query may execute multiple times, causing duplicate logs
```

## Conclusion

Projection operations are fundamental to functional programming in C#:

- ✅ **Select** maps elements one-to-one (map operation)
- ✅ **SelectMany** flattens nested collections (flatMap operation)
- ✅ **Immutable**: Original data unchanged
- ✅ **Composable**: Chain with other operations
- ✅ **Type-safe**: Compiler-checked transformations

The examples in [LinqProjecting.cs](LinqProjecting.cs) show:
- Creating anonymous types with computed properties
- Flattening nested collections (credit cards from employees)
- Combining data from parent and child objects

Projection is the cornerstone of data transformation in LINQ, enabling you to reshape data declaratively and compose complex transformations from simple operations.
