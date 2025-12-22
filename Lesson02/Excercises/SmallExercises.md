# C# Functional Programming Exercises

## Records Exercises

### Exercise 1: Immutable Shopping Cart (Easy)
Create a `ShoppingCartItem` record with properties: `ProductId`, `ProductName`, `Quantity`, and `UnitPrice`. Then create a `ShoppingCart` record that contains an `ImmutableList<ShoppingCartItem>`.

**Tasks:**
1. Create methods using `with` expressions to:
   - Add an item to the cart
   - Update the quantity of an existing item
   - Remove an item from the cart
2. Calculate the total price of all items in the cart
3. Demonstrate that the original cart remains unchanged after each operation

**Expected concepts:**
- Record declaration with primary constructor
- Immutable collections
- `with` expressions for creating modified copies
- Value equality

---

## Tuples Exercises

### Exercise 1: Multiple Return Values (Easy)
Write methods that use tuples to return multiple values without creating custom types.

**Tasks:**
1. Create a method `AnalyzeNumbers(int[] numbers)` that returns a tuple with:
   - Min value
   - Max value
   - Average
   - Count of even numbers
2. Create a method `SplitName(string fullName)` that returns `(firstName, lastName)`
3. Use tuple deconstruction to extract and display the values

**Expected concepts:**
- Tuple declaration (multiple syntaxes)
- Named tuple elements
- Deconstruction
- Tuple as return type


---

## Pattern Matching Exercises

### Exercise 1: Shape Calculator (Easy)
Create a shape hierarchy and use pattern matching to calculate areas and perimeters.

**Tasks:**
1. Define shape types: `Circle`, `Rectangle`, `Triangle` (use records)
2. Implement a method `CalculateArea(object shape)` using pattern matching:
   - Type patterns for different shapes
   - Property patterns to extract dimensions
3. Implement `Describe(object shape)` that returns different descriptions using pattern matching with `when` clauses

**Expected concepts:**
- Type patterns
- Property patterns
- Pattern combinators (and, or)
- `when` guards

---

## Generics Exercises

### Exercise 1: Generic Box Container (Easy)
Create a simple generic `Box<T>` class that can store any type of value.

**Tasks:**
1. Create a `Box<T>` class with:
   - A private field to store the value
   - A constructor that takes a value of type T
   - A `GetValue()` method that returns the stored value
   - An `IsEmpty()` method that checks if the value is null (for reference types)
2. Create instances of Box for different types:
   - `Box<int>` storing an integer
   - `Box<string>` storing a string
   - `Box<List<int>>` storing a list of integers
3. Print the values from each box

**Expected concepts:**
- Basic generic type declaration with `<T>`
- Using generic type parameter in fields and methods
- Creating instances of generic types
- Understanding that one generic class works with any type

**Example:**
```csharp
var intBox = new Box<int>(42);
var stringBox = new Box<string>("Hello");
Console.WriteLine(intBox.GetValue()); // 42
Console.WriteLine(stringBox.GetValue()); // Hello
```

---

### Exercise 2: Generic Pair Container (Easy-Medium)
Extend the Box concept to create a generic `Pair<TFirst, TSecond>` class that can store two values of different types.

**Tasks:**
1. Create a `Pair<TFirst, TSecond>` class with:
   - Two private fields to store the first and second values
   - A constructor that takes both values
   - Properties `First` and `Second` to access the values
   - A `Swap()` method that returns a new `Pair<TSecond, TFirst>` with the values swapped
   - A `Map<TNewFirst, TNewSecond>(Func<TFirst, TNewFirst> mapFirst, Func<TSecond, TNewSecond> mapSecond)` method that transforms both values
2. Create a generic static method `Create<T1, T2>(T1 first, T2 second)` that creates a pair
3. Create instances demonstrating:
   - `Pair<int, string>` storing an ID and name
   - `Pair<string, DateTime>` storing an event name and date
   - Use the `Swap()` method to reverse a pair
   - Use the `Map()` method to transform both values (e.g., convert int to string and string to uppercase)

**Expected concepts:**
- Multiple generic type parameters (`<TFirst, TSecond>`)
- Generic methods with multiple type parameters
- Creating new generic instances from transformations
- Type parameter naming conventions
- Understanding that different type parameters are independent

**Example:**
```csharp
var idName = Pair.Create(1, "John");
var swapped = idName.Swap(); // Pair<string, int>("John", 1)
var transformed = idName.Map(id => $"ID-{id}", name => name.ToUpper()); 
// Pair<string, string>("ID-1", "JOHN")
```

---
## Extensions Exercises

### Exercise 1: String Manipulation Extensions (Easy)
Create useful string extension methods following functional programming principles.

**Tasks:**
1. Create extension methods for `string`:
   - `Truncate(int maxLength, string suffix = "...")` - truncate with ellipsis
   - `IsNullOrEmpty()` - fluent version of string.IsNullOrEmpty
   - `ToTitleCase()` - capitalize first letter of each word
   - `CountOccurrences(char character)` - count character occurrences
2. Chain multiple extensions together to demonstrate fluent API
3. Ensure all methods are pure (no side effects)

**Expected concepts:**
- Extension method syntax (`this` parameter)
- Method chaining
- Immutability (return new values)
- Null handling

---

### Exercise 2: LINQ-Style Collection Extensions (Medium)
Implement custom LINQ-style extension methods for `IEnumerable<T>`.

**Tasks:**
1. Create extension methods:
   - `Chunk<T>(int size)` - split sequence into chunks (if not using .NET 6+)
   - `DistinctBy<T, TKey>(Func<T, TKey> keySelector)` - distinct by property
   - `MaxBy<T, TKey>(Func<T, TKey> keySelector)` - element with max property value
   - `Interleave<T>(IEnumerable<T> other)` - alternate elements from two sequences
2. Ensure all methods use deferred execution (yield return)
3. Write unit test-style demonstrations showing the methods work correctly

**Expected concepts:**
- Generic extension methods
- Deferred execution with `yield return`
- `IEnumerable<T>` manipulation
- Functional composition

---

## Enumerables Implementation Exercises

### Exercise 1: Custom Enumerable with Yield (Easy)
Implement custom enumerables using `yield return`.

**Tasks:**
1. Create a method `GenerateFibonacci(int count)` that yields Fibonacci numbers
2. Create a method `RepeatUntil<T>(T value, Func<T, bool> predicate)` that repeats a value until predicate is true
3. Create a method `Scan<T>(IEnumerable<T> source, Func<T, T, T> accumulator)` that yields intermediate accumulation results (like Aggregate but returns all steps)
4. Demonstrate lazy evaluation by adding Console.WriteLine statements showing when values are generated

**Expected concepts:**
- `yield return` for lazy evaluation
- IEnumerable<T> implementation
- Iterator pattern
- Deferred execution

---

### Exercise 2: Custom Enumerable Class (Medium)
Implement a custom `RangeEnumerable` class that manually implements `IEnumerable<T>` and `IEnumerator<T>`.

**Tasks:**
1. Create a `RangeEnumerable` class that generates a range of integers
   - Implement `IEnumerable<int>` and `IEnumerator<int>`
   - Support `start`, `end`, and `step` parameters
   - Handle both ascending and descending ranges
2. Add methods:
   - `Reverse()` - returns new RangeEnumerable with reversed direction
   - `Where(Func<int, bool> predicate)` - returns filtered enumerable
3. Demonstrate that:
   - Multiple enumerations work independently
   - Disposal is handled correctly
   - State is maintained per enumerator instance

**Expected concepts:**
- Manual IEnumerable<T> implementation
- IEnumerator<T> state management
- GetEnumerator() pattern
- Disposal and resource management
- Enumeration state isolation

---
