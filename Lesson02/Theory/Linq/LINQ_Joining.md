# LINQ Joining Operations

## Overview

Joining operations in LINQ combine elements from two sequences based on matching keys. They're equivalent to SQL joins and enable relational data operations in a functional, type-safe manner. LINQ provides three main join operations: `Join` (inner join), `GroupJoin` (left outer join producing groups), and `Zip` (element-wise pairing).

## The Join Operation (Inner Join)

### Basic Inner Join

```csharp
var roleInfos = new[]
{
    new { Role = WorkRole.Veterinarian, Description = "Animal Doctor", Department = "Medical" },
    new { Role = WorkRole.Management, Description = "Manager", Department = "Administration" },
    new { Role = WorkRole.AnimalCare, Description = "Caretaker", Department = "Operations" }
};

var employeesWithInfo = employees.Join(
    roleInfos,
    emp => emp.Role,             // outer key selector
    info => info.Role,           // inner key selector
    (emp, info) => new           // result selector
    {
        emp.FirstName,
        emp.LastName,
        emp.Role,
        info.Description,
        info.Department
    });

System.Console.WriteLine("Employees with role information:");
foreach (var e in employeesWithInfo.Take(10))
{
    System.Console.WriteLine($"{e.FirstName} {e.LastName} - {e.Role} ({e.Description}, {e.Department})");
}
```

**What it does:**
- Matches elements from two sequences based on key equality
- Only includes matches (inner join)
- Result contains combined data from both sequences

**Signature:**
```csharp
IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(
    this IEnumerable<TOuter> outer,
    IEnumerable<TInner> inner,
    Func<TOuter, TKey> outerKeySelector,
    Func<TInner, TKey> innerKeySelector,
    Func<TOuter, TInner, TResult> resultSelector)
```

**Parameters:**
- `outer`: First sequence (employees)
- `inner`: Second sequence (roleInfos)
- `outerKeySelector`: Extract key from outer element (`emp => emp.Role`)
- `innerKeySelector`: Extract key from inner element (`info => info.Role`)
- `resultSelector`: Combine matching elements (`(emp, info) => new { ... }`)

### How It Works

```
Employees:                 RoleInfos:
- John (Vet)              - Vet → "Animal Doctor"
- Mary (Manager)          - Manager → "Manager"
- Bob (Vet)               - Care → "Caretaker"
- Alice (Care)

Join on Role:
1. John (Vet) + Vet info → John, "Animal Doctor"
2. Mary (Manager) + Manager info → Mary, "Manager"
3. Bob (Vet) + Vet info → Bob, "Animal Doctor"
4. Alice (Care) + Care info → Alice, "Caretaker"
```

**Key characteristics:**
- **Inner join**: Only matching pairs
- **Equi-join**: Based on equality (`==`)
- **One-to-many**: Multiple outer elements can match same inner element

## The GroupJoin Operation

### Left Outer Join with Grouping

```csharp
var roleGroups = roleInfos.GroupJoin(
    employees,
    info => info.Role,           // outer key selector
    emp => emp.Role,             // inner key selector
    (info, emps) => new          // result selector (group)
    {
        info.Role,
        info.Description,
        Employees = emps.Take(5).Select(e => $"{e.FirstName} {e.LastName}").ToList(),
        Count = emps.Count()
    });

System.Console.WriteLine("Roles with employees:");
foreach (var rg in roleGroups)
{
    System.Console.WriteLine($"{rg.Role} ({rg.Description}) - {rg.Count} employees (showing up to 5):");
    foreach (var empName in rg.Employees)
        System.Console.WriteLine($"  {empName}");
}
```

**What it does:**
- For each element in outer sequence, creates a group of matching inner elements
- Always includes outer element (even with no matches)
- Like SQL `LEFT OUTER JOIN` but groups matches

**Signature:**
```csharp
IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(
    this IEnumerable<TOuter> outer,
    IEnumerable<TInner> inner,
    Func<TOuter, TKey> outerKeySelector,
    Func<TInner, TKey> innerKeySelector,
    Func<TOuter, IEnumerable<TInner>, TResult> resultSelector)
```

**Key difference from Join:**
- `Join` result selector: `(TOuter, TInner) => TResult` (one-to-one)
- `GroupJoin` result selector: `(TOuter, IEnumerable<TInner>) => TResult` (one-to-many)

### How It Works

```
RoleInfos:                 Employees:
- Vet info                - John (Vet)
- Manager info            - Bob (Vet)
- Care info               - Mary (Manager)
                          - Alice (Care)

GroupJoin:
1. Vet info + [John, Bob] → { Role: Vet, Employees: [John, Bob] }
2. Manager info + [Mary] → { Role: Manager, Employees: [Mary] }
3. Care info + [Alice] → { Role: Care, Employees: [Alice] }
```

### GroupJoin vs Join

| Feature | Join | GroupJoin |
|---------|------|-----------|
| Result per outer | One per match | One (with group) |
| No match behavior | Excluded | Included (empty group) |
| Result selector | `(outer, inner)` | `(outer, IEnumerable<inner>)` |
| SQL equivalent | `INNER JOIN` | `LEFT OUTER JOIN` + `GROUP BY` |

### Simulating Left Outer Join

```csharp
// GroupJoin + SelectMany = Left Outer Join
var leftJoin = roleInfos
    .GroupJoin(
        employees,
        info => info.Role,
        emp => emp.Role,
        (info, emps) => new { info, emps })
    .SelectMany(
        x => x.emps.DefaultIfEmpty(),
        (x, emp) => new
        {
            Role = x.info.Role,
            Description = x.info.Description,
            EmployeeName = emp != null ? $"{emp.FirstName} {emp.LastName}" : "No Employee"
        });
```

## The Zip Operation

### Element-wise Pairing

```csharp
var indices = Enumerable.Range(1, employees.Count());
var indexedEmployees = indices.Zip(
    employees,
    (index, emp) => $"{index}. {emp.FirstName} {emp.LastName}");

System.Console.WriteLine("Indexed employees:");
foreach (var ie in indexedEmployees.Take(10))
{
    System.Console.WriteLine(ie);
}
```

**What it does:**
- Pairs elements by position (1st with 1st, 2nd with 2nd, etc.)
- Stops when either sequence ends
- No key matching - purely positional

**Signature:**
```csharp
IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(
    this IEnumerable<TFirst> first,
    IEnumerable<TSecond> second,
    Func<TFirst, TSecond, TResult> resultSelector)
```

### How It Works

```
Sequence 1:  [1,    2,    3,    4,    5]
Sequence 2:  [A,    B,    C,    D]
             ↓     ↓     ↓     ↓
Zip:         [1,A] [2,B] [3,C] [4,D]
             (5 has no pair - excluded)
```

**Length of result:**
```csharp
result.Count() == Math.Min(first.Count(), second.Count())
```

### Zip Without Result Selector (C# 9+)

```csharp
// Returns tuples
var pairs = list1.Zip(list2);  // IEnumerable<(T1, T2)>

foreach (var (first, second) in pairs)
{
    Console.WriteLine($"{first} paired with {second}");
}
```

## Functional Programming Concepts

### 1. Relational Composition

Joins compose relationships between data:

```csharp
// Employees table + Roles table → Enriched view
var enriched = employees.Join(
    roles,
    e => e.RoleId,
    r => r.Id,
    (e, r) => new { e.Name, r.Title });
```

This is functional composition at the data level.

### 2. Cartesian Product (Without Join)

```csharp
// All combinations (avoid for large sequences!)
var allPairs = sequence1.SelectMany(
    a => sequence2.Select(
        b => new { a, b }));
```

Join is an optimized, key-filtered cartesian product.

### 3. Key-Based Lookup

Join internally builds a lookup table (like `ToLookup`):

```csharp
// Conceptual implementation
var lookup = inner.ToLookup(innerKeySelector);
var result = outer.SelectMany(outerElement =>
{
    var key = outerKeySelector(outerElement);
    var matches = lookup[key];
    return matches.Select(innerElement => 
        resultSelector(outerElement, innerElement));
});
```

### 4. Type Safety

LINQ joins are type-safe at compile time:

```csharp
// Compiler ensures key types match
var joined = employees.Join(
    roles,
    e => e.RoleId,     // Must return comparable type
    r => r.Id,         // Must return same type
    (e, r) => ...);
```

SQL joins can fail at runtime; LINQ joins fail at compile time.

### 5. Lazy Evaluation

Joins are deferred:

```csharp
var query = employees.Join(
    roles,
    e => e.RoleId,
    r => r.Id,
    (e, r) => new { e, r });  // Not executed

foreach (var item in query)  // NOW it joins
{
    // Process items
}
```

## Query Syntax for Joins

### Join in Query Syntax

```csharp
var query = from emp in employees
            join info in roleInfos on emp.Role equals info.Role
            select new
            {
                emp.FirstName,
                emp.LastName,
                info.Description
            };
```

**Equivalent method syntax:**
```csharp
var query = employees.Join(
    roleInfos,
    emp => emp.Role,
    info => info.Role,
    (emp, info) => new
    {
        emp.FirstName,
        emp.LastName,
        info.Description
    });
```

### GroupJoin in Query Syntax

```csharp
var query = from info in roleInfos
            join emp in employees on info.Role equals emp.Role into empGroup
            select new
            {
                info.Role,
                info.Description,
                Employees = empGroup
            };
```

**The `into` clause** creates the group.

### Multiple Joins

```csharp
var query = from emp in employees
            join role in roles on emp.RoleId equals role.Id
            join dept in departments on role.DeptId equals dept.Id
            select new
            {
                emp.Name,
                role.Title,
                dept.Name
            };
```

**Equivalent method syntax:**
```csharp
var query = employees
    .Join(roles, e => e.RoleId, r => r.Id, (e, r) => new { e, r })
    .Join(departments, x => x.r.DeptId, d => d.Id, (x, d) => new
    {
        Name = x.e.Name,
        Title = x.r.Title,
        DeptName = d.Name
    });
```

## Common Join Patterns

### Pattern 1: Enrichment

Add lookup data to entities:

```csharp
var enriched = orders.Join(
    customers,
    o => o.CustomerId,
    c => c.Id,
    (o, c) => new
    {
        o.OrderId,
        o.Total,
        CustomerName = c.Name
    });
```

### Pattern 2: Filtering

Join to filter:

```csharp
// Only employees in specific roles
var activeRoles = new[] { WorkRole.Veterinarian, WorkRole.Management };
var filtered = employees.Join(
    activeRoles,
    e => e.Role,
    r => r,
    (e, r) => e);
```

### Pattern 3: Many-to-Many

Through junction table:

```csharp
var employeeSkills = employees
    .Join(employeeSkillMap, e => e.Id, m => m.EmployeeId, (e, m) => new { e, m })
    .Join(skills, x => x.m.SkillId, s => s.Id, (x, s) => new
    {
        x.e.Name,
        SkillName = s.Name
    });
```

### Pattern 4: Self-Join

Join table to itself:

```csharp
// Find employees hired in same year
var sameYear = employees.Join(
    employees,
    e1 => e1.HireDate.Year,
    e2 => e2.HireDate.Year,
    (e1, e2) => new { e1, e2 })
    .Where(x => x.e1.Id < x.e2.Id);  // Avoid duplicates
```

### Pattern 5: Aggregate After Join

```csharp
var roleStats = employees
    .Join(roleInfos, e => e.Role, i => i.Role, (e, i) => new { e, i })
    .GroupBy(x => x.i.Department)
    .Select(g => new
    {
        Department = g.Key,
        Count = g.Count(),
        AvgCards = g.Average(x => x.e.CreditCards.Count)
    });
```

### Pattern 6: Composite Keys

```csharp
// Join on multiple fields
var joined = seq1.Join(
    seq2,
    x => new { x.Key1, x.Key2 },  // Anonymous type as key
    y => new { Key1 = y.K1, Key2 = y.K2 },  // Property names must match!
    (x, y) => new { x, y });
```

## Performance Considerations

### Join Performance

```csharp
// Efficient - builds hash table of inner sequence
var result = largeSequence.Join(
    smallSequence,
    large => large.Key,
    small => small.Key,
    (large, small) => ...);
```

**Complexity:**
- Time: O(n + m) where n = outer length, m = inner length
- Space: O(m) for inner sequence lookup

**Tip:** Put smaller sequence as `inner` for better performance.

### Multiple Enumerations

```csharp
// ❌ Bad - inner sequence enumerated multiple times
var result = outer.SelectMany(o =>
    inner.Where(i => i.Key == o.Key)
         .Select(i => new { o, i }));

// ✅ Better - Join enumerates inner once
var result = outer.Join(
    inner,
    o => o.Key,
    i => i.Key,
    (o, i) => new { o, i });
```

### Materialization

```csharp
// If inner sequence is expensive query
var innerList = expensiveQuery.ToList();  // Materialize once

var result = outer.Join(
    innerList,  // Reusable
    o => o.Key,
    i => i.Key,
    (o, i) => ...);
```

## Common Mistakes

### 1. Wrong Key Types

```csharp
// ❌ Compile error - key types don't match
var result = employees.Join(
    roles,
    e => e.RoleId,      // returns int
    r => r.Name,        // returns string
    (e, r) => ...);
```

### 2. Multiple Matches Misunderstanding

```csharp
// Join produces ALL matching pairs
var roles = new[] { role1, role2 };
var employees = new[] { emp1, emp2, emp3 };  // emp1 and emp2 have role1

var result = employees.Join(
    roles,
    e => e.Role,
    r => r.Id,
    (e, r) => ...);

// Result has 2 items (emp1+role1, emp2+role1), not 3
```

### 3. Composite Key Property Names

```csharp
// ❌ Wrong - property names must match exactly
var result = seq1.Join(
    seq2,
    x => new { x.A, x.B },
    y => new { y.A, C = y.B },  // 'C' != 'B'
    (x, y) => ...);

// ✅ Correct - names match
var result = seq1.Join(
    seq2,
    x => new { x.A, x.B },
    y => new { y.A, y.B },
    (x, y) => ...);
```

### 4. Confusing Join and GroupJoin

```csharp
// GroupJoin returns ONE item per outer element
var groups = outer.GroupJoin(inner, ...);
groups.Count() == outer.Count()  // Always true

// Join returns items for ALL matching pairs
var pairs = outer.Join(inner, ...);
pairs.Count() >= outer.Count()  // Can be more if multiple matches
```

### 5. Zip Length Assumption

```csharp
var seq1 = Enumerable.Range(1, 100);
var seq2 = Enumerable.Range(1, 10);

var zipped = seq1.Zip(seq2, (a, b) => ...);
zipped.Count() == 10  // Not 100! Stops at shorter sequence
```

## Comparison with SQL

### INNER JOIN

```sql
-- SQL
SELECT e.FirstName, e.LastName, r.Description
FROM Employees e
INNER JOIN Roles r ON e.RoleId = r.Id
```

```csharp
// LINQ
var query = employees.Join(
    roles,
    e => e.RoleId,
    r => r.Id,
    (e, r) => new { e.FirstName, e.LastName, r.Description });
```

### LEFT OUTER JOIN

```sql
-- SQL
SELECT r.Role, r.Description, e.FirstName, e.LastName
FROM Roles r
LEFT OUTER JOIN Employees e ON r.Id = e.RoleId
```

```csharp
// LINQ (via GroupJoin + SelectMany)
var query = roles
    .GroupJoin(employees, r => r.Id, e => e.RoleId, (r, emps) => new { r, emps })
    .SelectMany(x => x.emps.DefaultIfEmpty(), (x, e) => new
    {
        x.r.Role,
        x.r.Description,
        FirstName = e?.FirstName,
        LastName = e?.LastName
    });
```

## Conclusion

LINQ joining operations provide powerful relational data composition:

- ✅ **Join**: Inner join matching pairs
- ✅ **GroupJoin**: Left outer join with grouping
- ✅ **Zip**: Positional element pairing
- ✅ **Type-safe**: Compile-time checking
- ✅ **Efficient**: O(n + m) performance with hash lookups
- ✅ **Composable**: Combine with other LINQ operations
- ✅ **Functional**: Declarative relationship definition

The examples in [LinqJoining.cs](LinqJoining.cs) demonstrate:
- Join employees with role information (inner join)
- GroupJoin roles with employee groups
- Zip indices with employees for numbering

Joins are essential for combining related data from multiple sources in a type-safe, declarative, and functionally composable manner. They bring relational database concepts to in-memory object sequences.
