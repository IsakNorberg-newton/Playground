# LINQ Generation Operations

## Overview

Generation operations in LINQ create sequences from scratch without requiring an existing collection. These operations enable functional sequence construction, allowing you to generate ranges, repeat values, or create empty sequences programmatically. They're essential for testing, initialization, and creating data in a declarative, composable manner.

## The Empty Operation

### Creating Empty Sequences

```csharp
var empty = Enumerable.Empty<Employee>();
System.Console.WriteLine($"Empty<Employee> count: {empty.Count()}, any: {empty.Any()}");
```

**What it does:**
- Creates an empty sequence of specified type
- Returns cached singleton instance (efficient)
- Type parameter specifies element type

**Signature:**
```csharp
IEnumerable<TResult> Empty<TResult>()
```

**Use cases:**
```csharp
// Default/initial value
IEnumerable<Employee> employees = Enumerable.Empty<Employee>();

// Avoid null
public IEnumerable<Item> GetItems() => 
    condition ? actualItems : Enumerable.Empty<Item>();

// Start of accumulation
var result = categories
    .Select(c => c.Items ?? Enumerable.Empty<Item>())
    .Aggregate((acc, items) => acc.Concat(items));
```

**Why not just `new List<T>()`?**
```csharp
// ❌ Allocates new list
var empty1 = new List<Employee>();

// ✅ Returns cached singleton (no allocation)
var empty2 = Enumerable.Empty<Employee>();
```

## The Range Operation

### Generating Integer Sequences

```csharp
var seeder = new SeedGenerator();
var cards = Enumerable.Range(1, 5)
    .Select(i => new CreditCard().Seed(seeder))
    .ToList();

System.Console.WriteLine($"Range(1,5) created credit cards: {cards.Count}");
foreach (var c in cards)
    System.Console.WriteLine($" - {c.Issuer} {c.Number} exp {c.ExpirationMonth}/{c.ExpirationYear} Id={c.CreditCardId}");
```

**What it does:**
- Generates sequence of consecutive integers
- Start: first integer
- Count: number of integers
- Deferred execution (generates on enumeration)

**Signature:**
```csharp
IEnumerable<int> Range(int start, int count)
```

**Parameters:**
- `start`: First integer in sequence
- `count`: Number of integers to generate

**Examples:**
```csharp
// 1, 2, 3, 4, 5
Enumerable.Range(1, 5)

// 0, 1, 2, 3, 4
Enumerable.Range(0, 5)

// 10, 11, 12
Enumerable.Range(10, 3)

// Empty (count = 0)
Enumerable.Range(1, 0)
```

### Common Patterns with Range

```csharp
// Generate indices
var indices = Enumerable.Range(0, list.Count);

// Create test data
var testUsers = Enumerable.Range(1, 100)
    .Select(i => new User { Id = i, Name = $"User{i}" })
    .ToList();

// Pagination
var pages = Enumerable.Range(0, totalPages)
    .Select(pageIndex => GetPage(pageIndex));

// Initialize array
var array = Enumerable.Range(0, 10)
    .Select(i => i * 2)
    .ToArray();
```

### Range vs Imperative Loop

```csharp
// LINQ declarative
var rangeLinq = Enumerable.Range(1, 5).ToList();

// Imperative
var rangeImperative = new List<int>();
for (int i = 1; i <= 5; i++)
{
    rangeImperative.Add(i);
}

System.Console.WriteLine($"Range equals imperative: {rangeLinq.SequenceEqual(rangeImperative)}");
```

**Benefits of Range:**
- ✅ Declarative intent
- ✅ Composable with other LINQ operations
- ✅ Lazy evaluation
- ✅ Less error-prone (no off-by-one errors)

## The Repeat Operation

### Repeating Values

```csharp
var sample = employees.FirstOrDefault();
if (sample is not null)
{
    var repeated = Enumerable.Repeat(sample, 3).ToList();
    System.Console.WriteLine($"Repeat sample 3 times: count={repeated.Count}, all equal by value: {repeated.All(r => r == sample)}");
}
```

**What it does:**
- Generates sequence with same value repeated
- Value: element to repeat
- Count: number of times
- Deferred execution

**Signature:**
```csharp
IEnumerable<TResult> Repeat<TResult>(TResult element, int count)
```

**Examples:**
```csharp
// [0, 0, 0, 0, 0]
Enumerable.Repeat(0, 5)

// ["NA", "NA", "NA"]
Enumerable.Repeat("NA", 3)

// [defaultEmployee, defaultEmployee]
Enumerable.Repeat(defaultEmployee, 2)
```

### Important: Reference vs Value

```csharp
// Reference type - SAME instance repeated
var employee = new Employee { Id = 1 };
var repeated = Enumerable.Repeat(employee, 3);

repeated.ElementAt(0) == repeated.ElementAt(1);  // true - same reference
repeated.ElementAt(0).Id = 99;
repeated.ElementAt(1).Id;  // 99 - all share same instance!
```

### Common Patterns with Repeat

```csharp
// Initialize with default values
var defaults = Enumerable.Repeat(0.0, 100).ToArray();

// Padding
var padded = actualValues.Concat(Enumerable.Repeat(0, requiredLength - actualValues.Count()));

// Fill pattern
var pattern = Enumerable.Repeat(new[] { 1, 2, 3 }, 5)
    .SelectMany(x => x);  // [1,2,3,1,2,3,1,2,3,1,2,3,1,2,3]

// Default values for missing data
var withDefaults = ids
    .Select(id => cache.GetValueOrDefault(id) ?? defaultValue);
```

### Repeat vs Imperative Loop

```csharp
if (sample is not null)
{
    // LINQ declarative
    var repeatLinq = Enumerable.Repeat(sample, 3).ToList();
    
    // Imperative
    var repeatImperative = new List<Employee>();
    for (int i = 0; i < 3; i++)
    {
        repeatImperative.Add(sample);
    }
    
    System.Console.WriteLine($"Repeat equals imperative (SequenceEqual): {repeatLinq.SequenceEqual(repeatImperative)}");
}
```

## Functional Programming Concepts

### 1. Generator Pattern

Generation operations implement the **generator pattern**:

```csharp
// Lazy sequence generation
var numbers = Enumerable.Range(0, 1000000);  // Not generated yet!

var firstFive = numbers.Take(5);  // Still not generated

foreach (var n in firstFive)  // NOW generates (only 5 numbers)
{
    Console.WriteLine(n);
}
```

### 2. Infinite Sequences (Conceptually)

While `Range` and `Repeat` require count, the pattern supports infinite sequences:

```csharp
// Custom infinite generator
public static IEnumerable<int> Fibonacci()
{
    int a = 0, b = 1;
    while (true)
    {
        yield return a;
        (a, b) = (b, a + b);
    }
}

// Safe to use with Take
var first10Fib = Fibonacci().Take(10);
```

### 3. Composability

Generation operations compose seamlessly:

```csharp
// Generate, transform, filter
var result = Enumerable.Range(1, 100)
    .Select(n => n * n)           // Square
    .Where(n => n % 2 == 0)       // Even squares
    .Take(10)                     // First 10
    .ToList();
```

### 4. Declarative Construction

Express **what** to create, not **how**:

```csharp
// Declarative
var items = Enumerable.Range(1, 10)
    .Select(i => new Item { Id = i, Name = $"Item {i}" });

// Imperative (what AND how)
var items = new List<Item>();
for (int i = 1; i <= 10; i++)
{
    items.Add(new Item { Id = i, Name = $"Item {i}" });
}
```

### 5. Purity

Generation operations are referentially transparent:

```csharp
var seq1 = Enumerable.Range(1, 5);
var seq2 = Enumerable.Range(1, 5);
// seq1 and seq2 produce same elements (though different instances)
```

## Generation Operation Patterns

### Pattern 1: Test Data Generation

```csharp
var testData = Enumerable.Range(1, 100)
    .Select(i => new TestEntity
    {
        Id = i,
        Name = $"Test{i}",
        Value = i * 10,
        CreatedDate = DateTime.Now.AddDays(-i)
    })
    .ToList();
```

### Pattern 2: Chunking with Range

```csharp
var totalItems = 1000;
var pageSize = 50;
var pageCount = (totalItems + pageSize - 1) / pageSize;

var pages = Enumerable.Range(0, pageCount)
    .Select(pageIndex => GetPage(pageIndex, pageSize));
```

### Pattern 3: Array Initialization

```csharp
// 2D array with default values
var grid = Enumerable.Range(0, rows)
    .Select(row => Enumerable.Repeat(defaultValue, cols).ToArray())
    .ToArray();
```

### Pattern 4: Sequence Padding

```csharp
var padded = original
    .Concat(Enumerable.Repeat(paddingValue, targetLength - original.Count()))
    .Take(targetLength);
```

### Pattern 5: Index Pairing with Zip

```csharp
var indexed = Enumerable.Range(0, items.Count())
    .Zip(items, (index, item) => new { Index = index, Item = item });
```

### Pattern 6: Batch Processing

```csharp
var batches = Enumerable.Range(0, (totalCount + batchSize - 1) / batchSize)
    .Select(batchIndex => 
        items.Skip(batchIndex * batchSize).Take(batchSize));
```

### Pattern 7: Lookup Table Generation

```csharp
var lookupTable = Enumerable.Range(0, 256)
    .ToDictionary(i => i, i => ComputeValue(i));
```

### Pattern 8: Empty as Default

```csharp
public IEnumerable<Item> GetItems(Category category)
{
    return category?.Items ?? Enumerable.Empty<Item>();
}
```

## Performance Considerations

### Empty Efficiency

```csharp
// ✅ Singleton - no allocation
var empty1 = Enumerable.Empty<int>();
var empty2 = Enumerable.Empty<int>();
// ReferenceEquals(empty1, empty2) may be true (implementation detail)

// ❌ Allocates new list
var empty3 = new List<int>();
```

### Range Laziness

```csharp
// ✅ O(1) - no generation yet
var range = Enumerable.Range(1, 1000000);

// ✅ O(n) where n = elements actually enumerated
var first10 = range.Take(10).ToList();  // Only generates 10

// ❌ O(n) - generates all
var all = range.ToList();  // Generates 1,000,000
```

### Repeat Reference Sharing

```csharp
// Repeat doesn't create copies - shares reference
var repeated = Enumerable.Repeat(new LargeObject(), 1000);
// Only ONE LargeObject instance exists
```

## Common Mistakes

### 1. Off-By-One with Range

```csharp
// ❌ Wrong - generates 0, 1, 2, 3, 4 (5 items, not up to 5)
Enumerable.Range(0, 5)

// ✅ Correct - count is number of items
Enumerable.Range(0, 5)  // [0, 1, 2, 3, 4]
Enumerable.Range(1, 5)  // [1, 2, 3, 4, 5]
```

### 2. Modifying Repeated Reference

```csharp
var obj = new MutableObject { Value = 0 };
var repeated = Enumerable.Repeat(obj, 10).ToList();

// ❌ Modifies ALL elements (same reference)
repeated[0].Value = 99;
repeated[5].Value;  // 99 - all affected!

// ✅ Create separate instances
var separate = Enumerable.Range(1, 10)
    .Select(_ => new MutableObject { Value = 0 })
    .ToList();
```

### 3. Materializing Unnecessarily

```csharp
// ❌ Bad - generates all then filters
var items = Enumerable.Range(1, 1000000).ToList().Where(n => n % 2 == 0);

// ✅ Good - filters during generation
var items = Enumerable.Range(1, 1000000).Where(n => n % 2 == 0);
```

### 4. Forgetting Deferred Execution

```csharp
var query = Enumerable.Range(1, 10).Select(i => 
{
    Console.WriteLine($"Generating {i}");
    return i;
});

// Nothing printed yet!

var list = query.ToList();  // NOW it prints
```

### 5. Range Count vs End

```csharp
// ❌ Wrong thinking - count is not end value
Enumerable.Range(1, 10)  // Not 1 to 10
// Generates: 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 (10 items starting at 1)

// ✅ Correct understanding
Enumerable.Range(start: 1, count: 10)  // 10 items starting at 1
```

## Comparison with Other Languages

### Python

```python
# range (like LINQ Range)
range(1, 6)  # [1, 2, 3, 4, 5]

# repeat (like LINQ Repeat)
[value] * 5  # [value, value, value, value, value]
```

### JavaScript

```javascript
// Array.from with range
Array.from({length: 5}, (_, i) => i + 1)  // [1, 2, 3, 4, 5]

// fill (like Repeat)
Array(5).fill(value)  // [value, value, value, value, value]
```

### Haskell

```haskell
-- Range
[1..5]  -- [1, 2, 3, 4, 5]

-- Repeat (infinite!)
repeat value  -- [value, value, value, ...]
take 5 (repeat value)  -- [value, value, value, value, value]
```

## Advanced Patterns

### Custom Generators

```csharp
// Random sequence
public static IEnumerable<int> RandomSequence(int count, int min, int max)
{
    var random = new Random();
    return Enumerable.Range(0, count)
        .Select(_ => random.Next(min, max));
}

// Date range
public static IEnumerable<DateTime> DateRange(DateTime start, int days)
{
    return Enumerable.Range(0, days)
        .Select(offset => start.AddDays(offset));
}

// Fibonacci
public static IEnumerable<int> Fibonacci(int count)
{
    return Enumerable.Range(0, count)
        .Aggregate(
            new { Current = 0, Next = 1, Sequence = new List<int>() },
            (acc, _) =>
            {
                acc.Sequence.Add(acc.Current);
                return new { Current = acc.Next, Next = acc.Current + acc.Next, acc.Sequence };
            })
        .Sequence;
}
```

## Conclusion

LINQ generation operations provide declarative sequence construction:

- ✅ **Empty**: Create empty sequences efficiently
- ✅ **Range**: Generate integer sequences
- ✅ **Repeat**: Create sequences with repeated values
- ✅ **Lazy**: Deferred execution with on-demand generation
- ✅ **Composable**: Integrate seamlessly with other LINQ operations
- ✅ **Declarative**: Express what to generate, not how

The examples in [LinqGeneration.cs](LinqGeneration.cs) demonstrate:
- Empty for creating empty typed sequences
- Range to generate credit cards with sequential processing
- Repeat to duplicate sample employees
- Comparison with imperative loops showing equivalence

Generation operations are essential for test data creation, initialization, and functional sequence construction without requiring source collections. They enable declarative, lazy, and composable sequence generation patterns.
