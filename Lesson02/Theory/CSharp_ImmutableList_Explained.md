# C# ImmutableList: A Functional Programming Perspective

## Introduction

`ImmutableList<T>` is a collection type that, once created, cannot be modified. Instead of changing the list, operations return a new list with the modifications applied. This is a fundamental concept in functional programming where immutability prevents side effects and makes code more predictable and thread-safe.

## Why Immutability Matters in Functional Programming

Functional programming emphasizes:
- **No side effects**: Functions don't modify existing data
- **Predictability**: Same input always produces same output
- **Thread safety**: Immutable data can be safely shared between threads
- **Time travel**: Keep historical versions of data
- **Easier reasoning**: No hidden state changes

Immutable collections enable these principles by ensuring data cannot change after creation.

## Mutable vs. Immutable Collections

### Mutable List (Standard Approach)

```csharp
var items = new List<string> { "Apples", "Bananas" };
items.Add("Oranges");  // Modifies the original list
// items is now ["Apples", "Bananas", "Oranges"]
```

**Problem**: The original list is modified, potentially affecting other parts of the code that reference it.

### Immutable List (Functional Approach)

```csharp
var items = ImmutableList.Create("Apples", "Bananas");
var newItems = items.Add("Oranges");  // Returns a new list
// items is still ["Apples", "Bananas"]
// newItems is ["Apples", "Bananas", "Oranges"]
```

**Benefit**: The original list is preserved. Each operation returns a new list.

## The ShoppingCart Example

The example demonstrates combining C# records (immutable by default) with `ImmutableList<T>`:

```csharp
public record ShoppingCart(
    string Owner,                     // Direct property
    DateTime CreatedDate,              // Direct property
    ImmutableList<string> Items        // Immutable list
);
```

**Why This Design:**
- `record` provides immutability for the object itself
- `ImmutableList<string>` provides immutability for the collection
- Together, they create a fully immutable data structure

### Creating an Initial Cart

```csharp
var cart1 = new ShoppingCart(
    Owner: "Alice",
    CreatedDate: DateTime.Now,
    Items: ImmutableList.Create("Apples", "Bananas")
);

Console.WriteLine($"Cart1 Owner: {cart1.Owner}");
Console.WriteLine($"Cart1 Created: {cart1.CreatedDate}");
Console.WriteLine("Cart1 Items: " + string.Join(", ", cart1.Items));
// Output: Cart1 Owner: Alice
//         Cart1 Created: <timestamp>
//         Cart1 Items: Apples, Bananas
```

**Key Points:**
- `ImmutableList.Create()` creates an immutable list from initial values
- All properties are read-only
- The cart is a complete immutable snapshot of the shopping state

### Adding Items - Non-Destructive Update

```csharp
var cart2 = cart1 with
{
    Items = cart1.Items.Add("Oranges")
};

Console.WriteLine("\nCart2 Items: " + string.Join(", ", cart2.Items));
// Output: Cart2 Items: Apples, Bananas, Oranges
```

**What Happens:**
1. `cart1.Items.Add("Oranges")` returns a **new** `ImmutableList` with the added item
2. `cart1 with { Items = ... }` creates a **new** `ShoppingCart` record with the new list
3. `cart1` remains completely unchanged

**This is the functional programming pattern:**
- Don't modify existing data
- Create new versions with changes applied
- Original data preserved for reference or rollback

### Changing Owner - Preserving Items

```csharp
var cart3 = cart2 with
{
    Owner = "Bob"
};

Console.WriteLine($"\nCart3 Owner: {cart3.Owner}");
Console.WriteLine("Cart3 Items: " + string.Join(", ", cart3.Items));
// Output: Cart3 Owner: Bob
//         Cart3 Items: Apples, Bananas, Oranges
```

**Key Points:**
- Only the `Owner` property changes
- The `Items` list is shared between `cart2` and `cart3` (structural sharing)
- Because it's immutable, sharing is safe and efficient

## How ImmutableList Works

### Structural Sharing

`ImmutableList<T>` uses a tree structure internally, allowing it to share most of its structure between versions:

```csharp
var list1 = ImmutableList.Create(1, 2, 3);
var list2 = list1.Add(4);
```

Instead of copying all elements:
- `list1` and `list2` share the nodes for [1, 2, 3]
- Only the new node for 4 is created
- Both lists appear independent but share memory efficiently

**Benefits:**
- Memory efficient despite immutability
- Fast operations (not O(n) copies)
- Safe to share across threads

### Common Operations

All operations return a new list:

```csharp
var original = ImmutableList.Create("A", "B");

var added = original.Add("C");           // ["A", "B", "C"]
var removed = original.Remove("A");      // ["B"]
var inserted = original.Insert(1, "X"); // ["A", "X", "B"]
var replaced = original.SetItem(0, "Z");// ["Z", "B"]

// original is still ["A", "B"]
```

## Functional Programming Benefits

### 1. No Side Effects

```csharp
void ProcessCart(ShoppingCart cart)
{
    var updatedCart = cart with 
    { 
        Items = cart.Items.Add("NewItem") 
    };
    
    // cart is unchanged - no side effects!
    // Caller's cart reference is safe
}
```

Traditional mutable collections would allow `ProcessCart` to modify the caller's data unexpectedly.

### 2. Time Travel / History

```csharp
var history = new List<ShoppingCart>();

var cart = new ShoppingCart("Alice", DateTime.Now, ImmutableList<string>.Empty);
history.Add(cart);

cart = cart with { Items = cart.Items.Add("Apples") };
history.Add(cart);

cart = cart with { Items = cart.Items.Add("Bananas") };
history.Add(cart);

// Can access any previous state
Console.WriteLine("Initial state: " + string.Join(", ", history[0].Items));
Console.WriteLine("After first add: " + string.Join(", ", history[1].Items));
Console.WriteLine("After second add: " + string.Join(", ", history[2].Items));
```

Each version is preserved independently—perfect for undo/redo functionality.

### 3. Thread Safety

```csharp
// Safe to share across threads
var sharedCart = new ShoppingCart("Alice", DateTime.Now, 
    ImmutableList.Create("Apples"));

// Thread 1
Task.Run(() => {
    var cart2 = sharedCart with { Items = sharedCart.Items.Add("Bananas") };
});

// Thread 2
Task.Run(() => {
    var cart3 = sharedCart with { Items = sharedCart.Items.Add("Oranges") };
});

// No locks needed - sharedCart never changes
```

### 4. Pure Functions

```csharp
ShoppingCart AddItem(ShoppingCart cart, string item) =>
    cart with { Items = cart.Items.Add(item) };

ShoppingCart RemoveItem(ShoppingCart cart, string item) =>
    cart with { Items = cart.Items.Remove(item) };
```

These functions are **pure**:
- Same inputs always produce same outputs
- No side effects
- Easy to test
- Easy to reason about

### 5. Composition

```csharp
var cart = new ShoppingCart("Alice", DateTime.Now, ImmutableList<string>.Empty);

var updatedCart = cart
    with { Items = cart.Items.Add("Apples") }
    with { Items = cart.Items.Add("Bananas") }
    with { Items = cart.Items.Add("Oranges") };
```

Or more elegantly:

```csharp
ShoppingCart AddItems(ShoppingCart cart, params string[] items) =>
    cart with { Items = cart.Items.AddRange(items) };

var cart = new ShoppingCart("Alice", DateTime.Now, ImmutableList<string>.Empty);
var updatedCart = AddItems(cart, "Apples", "Bananas", "Oranges");
```

## Key ImmutableList Methods

| Method | Description | Returns |
|--------|-------------|---------|
| `Create(...)` | Create initial list | `ImmutableList<T>` |
| `Add(item)` | Add item to end | New list with item |
| `AddRange(items)` | Add multiple items | New list with items |
| `Remove(item)` | Remove first occurrence | New list without item |
| `RemoveAt(index)` | Remove at index | New list without item |
| `Insert(index, item)` | Insert at position | New list with inserted item |
| `SetItem(index, item)` | Replace at index | New list with replacement |
| `Clear()` | Remove all items | Empty list |

All methods return a **new** `ImmutableList<T>`.

## Performance Considerations

### When to Use ImmutableList

✅ **Good for:**
- Data that changes infrequently
- Sharing data across threads
- Maintaining history/versioning
- Functional programming patterns
- Domain models and DTOs

❌ **Avoid when:**
- Frequent modifications (use `List<T>` or `ImmutableList.Builder`)
- Very large collections with many updates
- Performance-critical tight loops

### ImmutableList.Builder for Many Changes

If you need many modifications, use a builder:

```csharp
var builder = ImmutableList.CreateBuilder<string>();
builder.Add("A");
builder.Add("B");
builder.Add("C");
// ... many operations ...
var immutableList = builder.ToImmutable(); // Finalize
```

This amortizes the cost of modifications.

## Comparison with Other Collection Types

| Type | Mutability | Thread Safe | Use Case |
|------|-----------|-------------|----------|
| `List<T>` | Mutable | No | General purpose, frequent changes |
| `ImmutableList<T>` | Immutable | Yes | Functional programming, shared data |
| `ImmutableArray<T>` | Immutable | Yes | Fixed-size sequences |
| `ReadOnlyCollection<T>` | Wrapper | Depends | Read-only view of mutable collection |

**Note**: `ReadOnlyCollection<T>` is just a wrapper—the underlying collection can still change. `ImmutableList<T>` guarantees no changes.

## Other Immutable Collections in .NET

The `System.Collections.Immutable` namespace provides a complete suite of immutable collection types, each optimized for different use cases:

| Collection | Optimized For | Best Use Case |
|------------|---------------|---------------|
| `ImmutableList<T>` | Sequential access, insertions | General-purpose list, frequent updates |
| `ImmutableArray<T>` | Random access, iteration | Fixed-size, read-heavy scenarios |
| `ImmutableDictionary<TKey, TValue>` | Key lookups | Configuration, lookup tables |
| `ImmutableHashSet<T>` | Membership testing | Unique collections, set operations |
| `ImmutableQueue<T>` | FIFO operations | Task queues, message processing |
| `ImmutableStack<T>` | LIFO operations | Undo/redo, recursion |
| `ImmutableSortedDictionary<TKey, TValue>` | Sorted key access | Ordered mappings |
| `ImmutableSortedSet<T>` | Sorted unique elements | Ordered sets |

All immutable collections:
- Are thread-safe by design
- Use structural sharing for efficiency
- Have corresponding `.Builder` types for batch modifications
- Follow the same pattern: operations return new collections

## Combining with Records

The `with` expression and immutable collections work beautifully together:

```csharp
public record ShoppingCart(
    string Owner,
    DateTime CreatedDate,
    ImmutableList<string> Items
);

// Fluent updates
var cart = new ShoppingCart("Alice", DateTime.Now, ImmutableList<string>.Empty)
    with { Items = ImmutableList.Create("Apples") }
    with { Items = cart.Items.Add("Bananas") };
```

**Pattern**: Records + ImmutableList = Fully immutable domain models

## Practical Patterns

### State Management

```csharp
public record AppState(
    string CurrentUser,
    ImmutableList<string> Notifications,
    ImmutableList<string> RecentSearches
);

// Redux-style state updates
AppState AddNotification(AppState state, string notification) =>
    state with { Notifications = state.Notifications.Add(notification) };
```

### Domain-Driven Design

```csharp
public record Order(
    Guid Id,
    string Customer,
    ImmutableList<OrderLine> Lines,
    decimal Total
);

public record OrderLine(string Product, int Quantity, decimal Price);
```

### Event Sourcing

```csharp
public record CartSnapshot(
    DateTime Timestamp,
    ShoppingCart Cart
);

var snapshots = ImmutableList<CartSnapshot>.Empty;
snapshots = snapshots.Add(new CartSnapshot(DateTime.Now, cart));
```

## Conclusion

The example in [ImmutableList.cs](Lesson02/ImmutableList.cs) demonstrates:

1. **Creating immutable lists** with `ImmutableList.Create()`
2. **Combining records with immutable collections** for fully immutable data structures
3. **Non-destructive updates** using `with` expression and `.Add()`
4. **Preserving original data** while creating modified versions

Key benefits for functional programming:

- ✅ **Immutability**: Data never changes after creation
- ✅ **No side effects**: Functions don't modify inputs
- ✅ **Thread safety**: Safe to share across threads
- ✅ **Structural sharing**: Memory efficient despite copies
- ✅ **Time travel**: Keep historical versions
- ✅ **Pure functions**: Predictable, testable code

`ImmutableList<T>` enables true functional programming in C# by ensuring that data transformations create new versions rather than modifying existing data. Combined with records, this provides a powerful foundation for building reliable, maintainable applications using functional programming principles.

Understanding immutable collections is essential for:
- Writing thread-safe concurrent code
- Implementing state management patterns (like Redux)
- Building functional domain models
- Preventing bugs from unexpected mutations
- Creating composable, pure functions
