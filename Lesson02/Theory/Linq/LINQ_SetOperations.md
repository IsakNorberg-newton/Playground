# LINQ Set Operations

## Overview

Set operations in LINQ treat sequences as mathematical sets, providing operations like union, intersection, difference, and duplicate removal. These operations enable powerful data comparison and combination patterns. In functional programming, sets represent collections where each element is unique, and set operations are fundamental for data analysis and manipulation.

## The Concat Operation

### Appending Sequences

```csharp
var first20 = employees.Take(20).ToList();
var vets = employees.Where(e => e.Role == WorkRole.Veterinarian).ToList();

var concat = first20.Concat(vets);
System.Console.WriteLine("Concat first20 + vets (showing 10):");
concat.Take(10).ToList().ForEach(e => 
    System.Console.WriteLine($"{e.FirstName} {e.LastName} - {e.Role}"));
```

**What it does:**
- Appends second sequence to first
- **May contain duplicates**
- Preserves order: first sequence, then second
- Deferred execution

**Signature:**
```csharp
IEnumerable<TSource> Concat<TSource>(
    this IEnumerable<TSource> first,
    IEnumerable<TSource> second)
```

**Not a true set operation** because it allows duplicates.

## The Union Operation

### Distinct Union

```csharp
var unionById = first20.Select(e => e.EmployeeId)
    .Union(vets.Select(e => e.EmployeeId));
System.Console.WriteLine($"Union (by EmployeeId) count: {unionById.Count()}");
```

**What it does:**
- Combines two sequences
- **Removes duplicates**
- Returns distinct elements from both sequences
- Mathematical set union: A ∪ B

**Signature:**
```csharp
IEnumerable<TSource> Union<TSource>(
    this IEnumerable<TSource> first,
    IEnumerable<TSource> second)

IEnumerable<TSource> Union<TSource>(
    this IEnumerable<TSource> first,
    IEnumerable<TSource> second,
    IEqualityComparer<TSource> comparer)
```

**How it works:**
```
Set 1: [1, 2, 3, 4]
Set 2: [3, 4, 5, 6]
Union: [1, 2, 3, 4, 5, 6]  // All unique elements
```

### Equality Comparison

**Default behavior:**
- Uses `GetHashCode()` and `Equals()`
- For reference types: reference equality (unless overridden)
- For value types: value equality
- For records: value equality (records override Equals)

**In the example:**
```csharp
// Employee is reference type, so Union uses reference equality
// Project to EmployeeId (Guid) for value-based union
var unionById = first20.Select(e => e.EmployeeId)
    .Union(vets.Select(e => e.EmployeeId));
```

## The Intersect Operation

### Common Elements

```csharp
var intersectById = first20.Select(e => e.EmployeeId)
    .Intersect(vets.Select(e => e.EmployeeId));
System.Console.WriteLine($"Intersect (by EmployeeId) count: {intersectById.Count()}");
```

**What it does:**
- Returns elements present in **both** sequences
- Removes duplicates
- Mathematical set intersection: A ∩ B

**Signature:**
```csharp
IEnumerable<TSource> Intersect<TSource>(
    this IEnumerable<TSource> first,
    IEnumerable<TSource> second)

IEnumerable<TSource> Intersect<TSource>(
    this IEnumerable<TSource> first,
    IEnumerable<TSource> second,
    IEqualityComparer<TSource> comparer)
```

**How it works:**
```
Set 1: [1, 2, 3, 4]
Set 2: [3, 4, 5, 6]
Intersect: [3, 4]  // Elements in both
```

**Use cases:**
- Find common items between two lists
- Identify overlap between groups
- Filter one list by membership in another

## The Except Operation

### Set Difference

```csharp
var exceptById = first20.Select(e => e.EmployeeId)
    .Except(vets.Select(e => e.EmployeeId));
System.Console.WriteLine($"Except (first20 ≠ vets) count: {exceptById.Count()}");
```

**What it does:**
- Returns elements in first sequence but **not** in second
- Removes duplicates
- Mathematical set difference: A - B or A \ B

**Signature:**
```csharp
IEnumerable<TSource> Except<TSource>(
    this IEnumerable<TSource> first,
    IEnumerable<TSource> second)

IEnumerable<TSource> Except<TSource>(
    this IEnumerable<TSource> first,
    IEnumerable<TSource> second,
    IEqualityComparer<TSource> comparer)
```

**How it works:**
```
Set 1: [1, 2, 3, 4]
Set 2: [3, 4, 5, 6]
Except: [1, 2]  // Elements in Set 1 but not Set 2
```

**Important:** Order matters!
```csharp
var diff1 = set1.Except(set2);  // Items in set1 only
var diff2 = set2.Except(set1);  // Items in set2 only
// diff1 != diff2
```

## The Distinct Operation

### Removing Duplicates

```csharp
var distinctConcat = concat
    .GroupBy(e => e.EmployeeId)
    .Select(g => g.First());
System.Console.WriteLine($"Distinct after concat (by EmployeeId) count: {distinctConcat.Count()}");
```

**What it does:**
- Removes duplicate elements
- Returns unique elements from a sequence
- Preserves first occurrence order

**Signature:**
```csharp
IEnumerable<TSource> Distinct<TSource>(
    this IEnumerable<TSource> source)

IEnumerable<TSource> Distinct<TSource>(
    this IEnumerable<TSource> source,
    IEqualityComparer<TSource> comparer)
```

**How it works:**
```
Sequence: [1, 2, 2, 3, 1, 4, 3]
Distinct: [1, 2, 3, 4]
```

### Manual Distinct by Key

In the example, manual distinct is used because `Employee` uses reference equality:

```csharp
// Manual distinct by EmployeeId
var distinctConcat = concat
    .GroupBy(e => e.EmployeeId)
    .Select(g => g.First());
```

**Alternative (C# 6+):**
```csharp
var distinct = concat.DistinctBy(e => e.EmployeeId);
```

## Functional Programming Concepts

### 1. Mathematical Set Theory

LINQ set operations implement mathematical set theory:

| Operation | Math Symbol | LINQ Method | Description |
|-----------|-------------|-------------|-------------|
| Union | A ∪ B | `Union` | All elements from both |
| Intersection | A ∩ B | `Intersect` | Elements in both |
| Difference | A - B | `Except` | Elements in A, not B |
| Distinct | - | `Distinct` | Unique elements |

### 2. Immutability

Set operations don't modify original sequences:

```csharp
var set1 = new[] { 1, 2, 3 };
var set2 = new[] { 3, 4, 5 };
var union = set1.Union(set2);

// set1 and set2 unchanged
// union is a new sequence
```

### 3. Composability

Set operations compose with other LINQ operations:

```csharp
var result = employees
    .Where(e => e.HireDate.Year > 2010)      // Filter
    .Select(e => e.Role)                     // Project
    .Distinct()                              // Set operation
    .OrderBy(r => r)                         // Order
    .ToList();                               // Materialize
```

### 4. Lazy Evaluation

Most set operations use deferred execution:

```csharp
var union = set1.Union(set2);  // Not executed yet

foreach (var item in union)    // NOW it executes
{
    // Process items
}
```

**Exception:** `Distinct`, `Union`, `Intersect`, and `Except` must buffer results to check for duplicates, but they still use deferred execution (don't execute until enumerated).

### 5. Referential Transparency

Pure functional operations without side effects:

```csharp
var result1 = seq1.Union(seq2);
var result2 = seq1.Union(seq2);
// result1 and result2 produce same elements
```

## Set Operation Patterns

### Pattern 1: Remove Duplicates

```csharp
var unique = items.Distinct();
```

### Pattern 2: Combine Lists Without Duplicates

```csharp
var combined = list1.Union(list2).Union(list3);
```

### Pattern 3: Find Common Items

```csharp
var common = list1.Intersect(list2).Intersect(list3);
```

### Pattern 4: Filter by Exclusion

```csharp
// All employees except those in specific roles
var excludedRoles = new[] { WorkRole.Manager, WorkRole.Intern };
var filtered = employees.Except(
    employees.Where(e => excludedRoles.Contains(e.Role)));

// Better approach
var filtered = employees.Where(e => !excludedRoles.Contains(e.Role));
```

### Pattern 5: Symmetric Difference

Elements in either set but not both:

```csharp
// (A - B) ∪ (B - A)
var symmetricDiff = set1.Except(set2).Union(set2.Except(set1));
```

### Pattern 6: Distinct by Key

```csharp
// C# 6+
var distinctEmployees = employees.DistinctBy(e => e.EmployeeId);

// Older versions
var distinctEmployees = employees
    .GroupBy(e => e.EmployeeId)
    .Select(g => g.First());
```

### Pattern 7: Set Containment

Check if one set is subset of another:

```csharp
// Is set1 subset of set2?
var isSubset = !set1.Except(set2).Any();

// Is set1 superset of set2?
var isSuperset = !set2.Except(set1).Any();
```

### Pattern 8: Append Without Duplicates

```csharp
// Add new items only if not already present
var updated = existing.Union(newItems);
```

## Equality and Comparison

### Default Equality

```csharp
// Value types: value equality
var nums1 = new[] { 1, 2, 3 };
var nums2 = new[] { 2, 3, 4 };
var union = nums1.Union(nums2);  // [1, 2, 3, 4]

// Reference types: reference equality
var emp1 = new Employee { Id = 1, Name = "John" };
var emp2 = new Employee { Id = 1, Name = "John" };
var emps1 = new[] { emp1 };
var emps2 = new[] { emp2 };
var union = emps1.Union(emps2);  // [emp1, emp2] - different references!
```

### Custom Equality Comparer

```csharp
public class EmployeeIdComparer : IEqualityComparer<Employee>
{
    public bool Equals(Employee x, Employee y) =>
        x?.EmployeeId == y?.EmployeeId;
    
    public int GetHashCode(Employee obj) =>
        obj?.EmployeeId.GetHashCode() ?? 0;
}

// Use custom comparer
var union = employees1.Union(employees2, new EmployeeIdComparer());
```

### Projection Approach (Simpler)

```csharp
// Instead of custom comparer, project to comparable values
var unionIds = employees1.Select(e => e.EmployeeId)
    .Union(employees2.Select(e => e.EmployeeId));

// Get employees from IDs
var unionEmployees = employees
    .Where(e => unionIds.Contains(e.EmployeeId));
```

## Performance Considerations

### Buffering

Set operations must buffer data:

```csharp
// Must store elements to check for duplicates
var distinct = hugeSequence.Distinct();  // O(n) space

// Union, Intersect, Except also buffer
```

### Hash-Based Implementation

LINQ uses hash tables for O(1) lookup:

```csharp
// Time: O(n + m)
// Space: O(n) for first sequence
var intersect = seq1.Intersect(seq2);
```

**Complexity:**
| Operation | Time | Space |
|-----------|------|-------|
| `Concat` | O(1) | O(1) |
| `Union` | O(n + m) | O(n) |
| `Intersect` | O(n + m) | O(n) |
| `Except` | O(n + m) | O(n) |
| `Distinct` | O(n) | O(n) |

Where n = first sequence length, m = second sequence length.

### Materialization

Consider materializing expensive queries:

```csharp
// ❌ Bad - expensive query executed twice
var union = expensiveQuery1.Union(expensiveQuery2);

// ✅ Better - materialize once
var list1 = expensiveQuery1.ToList();
var list2 = expensiveQuery2.ToList();
var union = list1.Union(list2);
```

## Common Mistakes

### 1. Reference vs Value Equality

```csharp
// ❌ Won't work as expected for reference types
var distinct = employees.Distinct();  // Uses reference equality

// ✅ Project to value type
var distinct = employees
    .GroupBy(e => e.EmployeeId)
    .Select(g => g.First());

// ✅ Or use DistinctBy (C# 6+)
var distinct = employees.DistinctBy(e => e.EmployeeId);
```

### 2. Except Order Matters

```csharp
var diff1 = set1.Except(set2);  // Items in set1 only
var diff2 = set2.Except(set1);  // Items in set2 only

// diff1 != diff2
```

### 3. Concat vs Union

```csharp
// Concat allows duplicates
var concat = list1.Concat(list2);
concat.Count() == list1.Count() + list2.Count()  // Always true

// Union removes duplicates
var union = list1.Union(list2);
union.Count() <= list1.Count() + list2.Count()  // Can be less
```

### 4. Empty Sequences

```csharp
// Safe - set operations handle empty sequences
var empty = Enumerable.Empty<int>();
var result = empty.Union(new[] { 1, 2, 3 });  // [1, 2, 3]
```

### 5. Multiple Enumerations

```csharp
// ❌ Enumerates twice
var union = sequence.Union(otherSequence);
var count1 = union.Count();
var count2 = union.Count();

// ✅ Materialize once
var unionList = sequence.Union(otherSequence).ToList();
var count1 = unionList.Count;
var count2 = unionList.Count;
```

## Comparison with SQL

### UNION

```sql
-- SQL UNION (distinct)
SELECT Name FROM Table1
UNION
SELECT Name FROM Table2
```

```csharp
// LINQ
var union = table1.Select(t => t.Name)
    .Union(table2.Select(t => t.Name));
```

### UNION ALL

```sql
-- SQL UNION ALL (allows duplicates)
SELECT Name FROM Table1
UNION ALL
SELECT Name FROM Table2
```

```csharp
// LINQ
var unionAll = table1.Select(t => t.Name)
    .Concat(table2.Select(t => t.Name));
```

### INTERSECT

```sql
-- SQL
SELECT Name FROM Table1
INTERSECT
SELECT Name FROM Table2
```

```csharp
// LINQ
var intersect = table1.Select(t => t.Name)
    .Intersect(table2.Select(t => t.Name));
```

### EXCEPT

```sql
-- SQL
SELECT Name FROM Table1
EXCEPT
SELECT Name FROM Table2
```

```csharp
// LINQ
var except = table1.Select(t => t.Name)
    .Except(table2.Select(t => t.Name));
```

### DISTINCT

```sql
-- SQL
SELECT DISTINCT Name FROM Table
```

```csharp
// LINQ
var distinct = table.Select(t => t.Name).Distinct();
```

## Conclusion

LINQ set operations provide powerful data manipulation based on mathematical set theory:

- ✅ **Concat**: Append sequences (allows duplicates)
- ✅ **Union**: Combine with distinct elements
- ✅ **Intersect**: Common elements only
- ✅ **Except**: Elements in first but not second
- ✅ **Distinct**: Remove duplicates
- ✅ **Functional**: Immutable, composable operations
- ✅ **Efficient**: Hash-based O(n) implementations

The examples in [LinqSetOps.cs](LinqSetOps.cs) demonstrate:
- Concat for appending with potential duplicates
- Union for distinct combination by EmployeeId
- Intersect to find common employees
- Except to find differences
- Manual Distinct using GroupBy

Set operations are essential for comparing collections, removing duplicates, and performing data reconciliation tasks in a declarative, functional style.
