# LINQ Quantifier Operations

## Overview

Quantifier operations in LINQ answer boolean questions about sequences: "Do **all** elements satisfy a condition?", "Does **any** element satisfy it?", "Does the sequence **contain** this item?", or "Are two sequences **equal**?". These operations implement logical quantification from predicate logic and are essential for validation, existence checks, and sequence comparison.

## The All Operation

### Universal Quantification

```csharp
var allHaveFirstName = employees.All(e => !string.IsNullOrWhiteSpace(e.FirstName));
System.Console.WriteLine($"All have first names: {allHaveFirstName}");
```

**What it does:**
- Returns `true` if **all** elements satisfy the predicate
- Returns `true` for empty sequence (vacuous truth)
- Short-circuits on first `false`

**Signature:**
```csharp
bool All<TSource>(
    this IEnumerable<TSource> source,
    Func<TSource, bool> predicate)
```

**Mathematical equivalent:**
```
∀x ∈ S : P(x)
"For all x in sequence S, predicate P(x) is true"
```

**How it works:**
```csharp
// Conceptual implementation
public static bool All<T>(this IEnumerable<T> source, Func<T, bool> predicate)
{
    foreach (var item in source)
    {
        if (!predicate(item))
            return false;  // Short-circuit: found counter-example
    }
    return true;  // All passed
}
```

**Short-circuit behavior:**
```csharp
var allPositive = new[] { 1, 2, 3, -1, 5 }.All(n => n > 0);
// Checks: 1 ✓, 2 ✓, 3 ✓, -1 ✗ → returns false immediately
// Never checks 5
```

**Empty sequence:**
```csharp
var empty = Enumerable.Empty<int>();
var allPositive = empty.All(n => n > 0);  // true (vacuous truth)
```

## The Any Operation

### Existential Quantification

```csharp
var anyVeterinarians = employees.Any(e => e.Role == WorkRole.Veterinarian);
System.Console.WriteLine($"Any veterinarians: {anyVeterinarians}");
```

**What it does:**
- Returns `true` if **at least one** element satisfies the predicate
- Returns `false` for empty sequence
- Short-circuits on first `true`

**Signature:**
```csharp
bool Any<TSource>(this IEnumerable<TSource> source)
bool Any<TSource>(
    this IEnumerable<TSource> source,
    Func<TSource, bool> predicate)
```

**Mathematical equivalent:**
```
∃x ∈ S : P(x)
"There exists x in sequence S such that predicate P(x) is true"
```

**Without predicate:**
```csharp
var hasElements = employees.Any();  // Checks if sequence is non-empty
```

**Short-circuit behavior:**
```csharp
var hasNegative = new[] { 1, 2, -1, 4, 5 }.Any(n => n < 0);
// Checks: 1 ✗, 2 ✗, -1 ✓ → returns true immediately
// Never checks 4 or 5
```

### Any vs Count

```csharp
// ❌ Bad - enumerates entire sequence
if (items.Count() > 0)

// ✅ Good - stops at first element
if (items.Any())

// ❌ Bad - counts all matches
if (items.Count(predicate) > 0)

// ✅ Good - stops at first match
if (items.Any(predicate))
```

## The Contains Operation

### Membership Testing

```csharp
var sample = employees.FirstOrDefault();
if (sample is not null)
{
    var containsSample = employees.Contains(sample);
    System.Console.WriteLine($"Contains first employee (by record equality): {containsSample}");
}
```

**What it does:**
- Checks if sequence contains specific element
- Uses equality comparison (`Equals()` and `GetHashCode()`)
- Short-circuits when element found

**Signature:**
```csharp
bool Contains<TSource>(
    this IEnumerable<TSource> source,
    TSource value)

bool Contains<TSource>(
    this IEnumerable<TSource> source,
    TSource value,
    IEqualityComparer<TSource> comparer)
```

**Equality behavior:**
```csharp
// Value types: value equality
var numbers = new[] { 1, 2, 3 };
numbers.Contains(2);  // true

// Reference types: reference equality (unless overridden)
var emp1 = new Employee { Id = 1 };
var emp2 = new Employee { Id = 1 };
var list = new[] { emp1 };
list.Contains(emp2);  // false (different references)

// Records: value equality (records override Equals)
var person1 = new Person { Name = "John" };
var person2 = new Person { Name = "John" };
var people = new[] { person1 };
people.Contains(person2);  // true (value equality)
```

**In the example:**
```csharp
// Employee is defined in the code - check if it's a record
var sample = employees.FirstOrDefault();
var containsSample = employees.Contains(sample);
// Works because records have value equality
```

## The SequenceEqual Operation

### Sequence Comparison

```csharp
var firstThree = employees.Take(3).ToList();
var sameOrder = new List<Employee>(firstThree);
var differentOrder = firstThree.AsEnumerable().Reverse().ToList();

System.Console.WriteLine($"SequenceEqual (same order): {firstThree.SequenceEqual(sameOrder)}");
System.Console.WriteLine($"SequenceEqual (different order): {firstThree.SequenceEqual(differentOrder)}");
```

**What it does:**
- Compares two sequences element-by-element
- Returns `true` if same length, same order, same values
- Uses equality comparison

**Signature:**
```csharp
bool SequenceEqual<TSource>(
    this IEnumerable<TSource> first,
    IEnumerable<TSource> second)

bool SequenceEqual<TSource>(
    this IEnumerable<TSource> first,
    IEnumerable<TSource> second,
    IEqualityComparer<TSource> comparer)
```

**Requirements for true:**
1. Same number of elements
2. Elements in same order
3. Corresponding elements equal

**Examples:**
```csharp
var seq1 = new[] { 1, 2, 3 };
var seq2 = new[] { 1, 2, 3 };
var seq3 = new[] { 1, 3, 2 };
var seq4 = new[] { 1, 2 };

seq1.SequenceEqual(seq2);  // true - same elements, same order
seq1.SequenceEqual(seq3);  // false - different order
seq1.SequenceEqual(seq4);  // false - different length
```

**Short-circuit behavior:**
```csharp
var equal = seq1.SequenceEqual(seq2);
// Compares pair by pair:
// seq1[0] == seq2[0]? ✓
// seq1[1] == seq2[1]? ✓
// seq1[2] == seq2[2]? ✓
// Both sequences exhausted? ✓
// → true

var notEqual = seq1.SequenceEqual(seq3);
// seq1[0] == seq3[0]? ✓
// seq1[1] == seq3[1]? ✗ (2 != 3)
// → false immediately
```

## Functional Programming Concepts

### 1. Predicate Logic

Quantifiers implement formal logic:

| Logic | Symbol | LINQ | Negation |
|-------|--------|------|----------|
| Universal | ∀ | `All` | `!All(x => P(x))` or `Any(x => !P(x))` |
| Existential | ∃ | `Any` | `!Any(x => P(x))` or `All(x => !P(x))` |

**De Morgan's Laws:**
```csharp
// ¬(∀x: P(x)) ≡ ∃x: ¬P(x)
!items.All(x => P(x)) == items.Any(x => !P(x))

// ¬(∃x: P(x)) ≡ ∀x: ¬P(x)
!items.Any(x => P(x)) == items.All(x => !P(x))
```

### 2. Short-Circuit Evaluation

Quantifiers use lazy evaluation and stop early:

```csharp
var result = items.Any(x => ExpensivePredicate(x));
// Stops at first true - doesn't check all items
```

This is crucial for performance and infinite sequences:

```csharp
// Works even on infinite sequence
var hasNegative = InfiniteSequence().Any(n => n < 0);
// Stops at first negative
```

### 3. Vacuous Truth

Empty sequences have special logical behavior:

```csharp
var empty = Enumerable.Empty<int>();

empty.All(x => x > 1000);   // true  (vacuously true)
empty.Any(x => x > 1000);   // false (no witness)
empty.Contains(5);          // false (no elements)
```

**Why `All` returns true for empty:**
- "All students passed" is true if there are no students
- No counter-example exists
- Mathematical convention: ∀x ∈ ∅: P(x) is true

### 4. Referential Transparency

Quantifiers are pure functions (no side effects):

```csharp
var result1 = items.All(predicate);
var result2 = items.All(predicate);
// result1 == result2 (same inputs → same output)
```

### 5. Composability

Quantifiers compose with other LINQ operations:

```csharp
var allHaveCards = employees
    .Where(e => e.Role == WorkRole.Veterinarian)
    .All(e => e.CreditCards.Any());
```

## Quantifier Patterns

### Pattern 1: Validation

```csharp
// All items valid?
var allValid = items.All(item => item.IsValid());

// Any invalid?
var hasInvalid = items.Any(item => !item.IsValid());
```

### Pattern 2: Existence Check

```csharp
// Instead of FirstOrDefault != null
if (items.Any(x => x.Id == targetId))
{
    // Found
}
```

### Pattern 3: Empty Check

```csharp
// Better than Count() == 0
if (!items.Any())
{
    // Empty
}

if (items.Any())
{
    // Non-empty
}
```

### Pattern 4: Subset Testing

```csharp
// Is set1 subset of set2?
var isSubset = set1.All(item => set2.Contains(item));

// Is set1 disjoint from set2?
var isDisjoint = !set1.Any(item => set2.Contains(item));
```

### Pattern 5: Negation

```csharp
// None match predicate
var noneMatch = !items.Any(predicate);
// Or
var noneMatch = items.All(x => !predicate(x));

// Not all match
var notAllMatch = !items.All(predicate);
// Or
var notAllMatch = items.Any(x => !predicate(x));
```

### Pattern 6: Complex Conditions

```csharp
// All employees have cards AND are senior
var condition = employees.All(e => 
    e.CreditCards.Any() && 
    (DateTime.Now - e.HireDate).TotalDays > 365);
```

### Pattern 7: Comparison

```csharp
// Check if two processed sequences are equal
var equal = list1
    .OrderBy(x => x)
    .SequenceEqual(list2.OrderBy(x => x));
```

## Performance Considerations

### Short-Circuiting

```csharp
// Efficient - stops early
var hasMatch = hugeSequence.Any(predicate);

// Less efficient - checks all
var count = hugeSequence.Count(predicate);
var hasMatch = count > 0;
```

### Any() vs Count()

```csharp
// ❌ Bad - O(n)
if (items.Count() > 0)

// ✅ Good - O(1) or early exit
if (items.Any())

// ❌ Bad - O(n)
if (items.Where(predicate).Count() > 0)

// ✅ Good - early exit
if (items.Any(predicate))
```

### Contains() Optimization

```csharp
// For ICollection<T>, Contains is O(1) or O(n) depending on collection
var list = new List<int> { 1, 2, 3 };
list.Contains(2);  // O(n) for List

var set = new HashSet<int> { 1, 2, 3 };
set.Contains(2);  // O(1) for HashSet

// For repeated contains checks, use HashSet
var lookup = items.ToHashSet();
if (lookup.Contains(target))  // O(1)
```

### SequenceEqual Complexity

```csharp
// O(n) where n = length of shorter sequence
var equal = seq1.SequenceEqual(seq2);
// Stops at first mismatch
```

## Common Mistakes

### 1. Using Count When Any Suffices

```csharp
// ❌ Wrong - enumerates all
if (items.Count() > 0)

// ✅ Correct - stops at first
if (items.Any())
```

### 2. Negating Incorrectly

```csharp
// ❌ Wrong logic
var noneValid = !items.All(x => x.IsValid());
// This means "not all are valid" (some might still be valid)

// ✅ Correct - "none are valid"
var noneValid = !items.Any(x => x.IsValid());
// Or
var noneValid = items.All(x => !x.IsValid());
```

### 3. Forgetting Reference Equality

```csharp
var emp1 = new Employee { Id = 1 };
var emp2 = new Employee { Id = 1 };
var employees = new[] { emp1 };

// ❌ Returns false (different references)
employees.Contains(emp2);

// ✅ Compare by ID
employees.Any(e => e.Id == emp2.Id);
```

### 4. Order Dependency in SequenceEqual

```csharp
var set1 = new[] { 1, 2, 3 };
var set2 = new[] { 3, 2, 1 };

// ❌ false - order matters
set1.SequenceEqual(set2);

// ✅ Order first if order shouldn't matter
set1.OrderBy(x => x).SequenceEqual(set2.OrderBy(x => x));

// ✅ Or use set comparison
set1.ToHashSet().SetEquals(set2);
```

### 5. Multiple Enumeration

```csharp
// ❌ Bad - enumerates twice
if (query.Any())
{
    var first = query.First();  // Enumerates again
}

// ✅ Better
var first = query.FirstOrDefault();
if (first != null)
{
    // Use first
}
```

## Comparison with Other Languages

### SQL

```sql
-- EXISTS (Any with predicate)
SELECT * FROM Orders WHERE EXISTS (
    SELECT 1 FROM OrderDetails WHERE OrderId = Orders.Id
)

-- NOT EXISTS (no equivalent, use negation)
SELECT * FROM Orders WHERE NOT EXISTS (...)
```

```csharp
// LINQ
var ordersWithDetails = orders.Where(o => 
    orderDetails.Any(od => od.OrderId == o.Id));
```

### Python

```python
# all()
all(x > 0 for x in numbers)

# any()
any(x < 0 for x in numbers)

# in (Contains)
5 in numbers
```

### JavaScript

```javascript
// every (All)
numbers.every(x => x > 0)

// some (Any)
numbers.some(x => x < 0)

// includes (Contains)
numbers.includes(5)
```

## Truth Tables

### All

| Sequence | Predicate Results | `All` |
|----------|-------------------|-------|
| [] | [] | true |
| [T] | [true] | true |
| [F] | [false] | false |
| [T, T] | [true, true] | true |
| [T, F] | [true, false] | false |

### Any

| Sequence | Predicate Results | `Any` |
|----------|-------------------|-------|
| [] | [] | false |
| [T] | [true] | true |
| [F] | [false] | false |
| [T, F] | [true, false] | true |
| [F, F] | [false, false] | false |

## Conclusion

LINQ quantifier operations provide declarative boolean queries on sequences:

- ✅ **All**: Universal quantification (∀) - all elements satisfy condition
- ✅ **Any**: Existential quantification (∃) - at least one element satisfies condition  
- ✅ **Contains**: Membership testing - sequence includes element
- ✅ **SequenceEqual**: Equality - sequences have same elements in same order
- ✅ **Short-circuit**: Stop as soon as answer is determined
- ✅ **Logical**: Implement predicate logic and De Morgan's laws
- ✅ **Efficient**: Better than Count() for existence checks

The examples in [LinqQuantifier.cs](LinqQuantifier.cs) demonstrate:
- All to verify every employee has a first name
- Any to check if any veterinarians exist
- Contains with record equality for membership testing
- SequenceEqual comparing same and different orderings

Quantifiers are essential for validation, existence checks, and logical queries on collections in a declarative, efficient, and functionally pure manner.
