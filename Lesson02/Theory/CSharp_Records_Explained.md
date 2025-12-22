# C# Records: A Functional Programming Perspective

## Introduction

Records in C# are a special type introduced in C# 9.0 (and enhanced in C# 10.0) that provide built-in functionality for creating immutable reference types with value-based equality. They are particularly valuable in functional programming paradigms, where immutability and value semantics are fundamental principles.

## Why Records Matter in Functional Programming

Functional programming emphasizes:
- **Immutability**: Data that doesn't change after creation
- **Value Semantics**: Equality based on data content, not reference
- **Pure Functions**: Functions without side effects
- **Data Transformation**: Creating new data rather than modifying existing data

Records align perfectly with these principles by providing:
1. Concise syntax for immutable data structures
2. Built-in value-based equality
3. Non-destructive mutation through `with` expressions
4. Pattern matching support

## Key Features of Records

### 1. Value-Based Equality

**Traditional Classes** use reference equality by default:

```csharp
var classObj1 = new PersonClass("John", "Doe", 30);
var classObj2 = new PersonClass("John", "Doe", 30);
Console.WriteLine(classObj1 == classObj2); // False - different references
```

**Records** use value equality automatically:

```csharp
var person1 = new Person("John", "Doe", 30);
var person2 = new Person("John", "Doe", 30);
Console.WriteLine(person1 == person2); // True - same values
```

This is crucial in functional programming where we care about the *data itself*, not where it's stored in memory.

### 2. Positional Syntax (Primary Constructor)

Records support a concise declaration syntax:

```csharp
public record Person(string FirstName, string LastName, int Age);
```

This single line generates:
- A constructor with three parameters
- Public properties with `init` accessors (immutable after construction)
- Proper `Equals()`, `GetHashCode()`, and `ToString()` implementations
- A `Deconstruct` method for pattern matching

Compare this to the traditional class approach which requires significantly more boilerplate code.

### 3. Immutability with `init` Accessors

Properties in records use `init` accessors, meaning they can only be set during object initialization:

```csharp
var person = new Person("John", "Doe", 30);
// person.Age = 31; // Compile error!
```

This enforces immutability, a cornerstone of functional programming that prevents side effects and makes code more predictable.

### 4. Non-Destructive Mutation (`with` Expression)

The `with` expression allows you to create a new record with some properties changed, without modifying the original:

```csharp
var employee1 = new Employee("Alice", "Johnson", 28, "Engineering");
var employee2 = employee1 with { Age = 29 };

// employee1 is unchanged: Alice Johnson, Age: 28
// employee2 is new object: Alice Johnson, Age: 29
```

This is a functional programming pattern called "copy-and-update" or "persistent data structures". Instead of mutating existing data, we create new versions, preserving the original.

### 5. Deconstruction

Records automatically provide a `Deconstruct` method:

```csharp
var person = new Person("John", "Doe", 30);
var (firstName, lastName, age) = person;
```

This enables pattern matching and makes it easy to extract values from records, a common need in functional transformations.

## Advanced Features

### Records with Additional Members

Records can have additional properties, methods, and validation:

```csharp
public record Employee(string FirstName, string LastName, int Age, string Department)
{
    public string FullName => $"{FirstName} {LastName}";
    
    public string GetInfo() => $"{FullName}, Age: {Age}, Dept: {Department}";
}
```

### Validation in Records

You can add validation logic in the constructor:

```csharp
public record Temperature
{
    public double Celsius { get; init; }
    
    public Temperature(double celsius)
    {
        if (celsius < -273.15)
            throw new ArgumentException("Temperature cannot be below absolute zero");
        Celsius = celsius;
    }
    
    public double Fahrenheit => (Celsius * 9 / 5) + 32;
    public double Kelvin => Celsius + 273.15;
}
```

This ensures data integrity while maintaining immutability.

### Record Inheritance

Records support inheritance, allowing you to create hierarchies of related types:

```csharp
public record Vehicle(string Make, string Model, int Year);
public record Car(string Make, string Model, int Year, int Doors) 
    : Vehicle(Make, Model, Year);
public record Motorcycle(string Make, string Model, int Year, string Type) 
    : Vehicle(Make, Model, Year);
```

This enables polymorphism while maintaining value semantics:

```csharp
Vehicle[] vehicles = { vehicle, car, bike };
var recent = vehicles.Where(v => v.Year >= 2022);
```

### Record Structs (Value Types)

C# 10 introduced `record struct`, which provides record features for value types:

```csharp
public record struct Point(double X, double Y)
{
    public double DistanceFromOrigin => Math.Sqrt(X * X + Y * Y);
}
```

Record structs are stack-allocated and have the same value semantics as regular records, but with the performance characteristics of structs.

## Functional Programming Patterns with Records

### 1. Domain Modeling

Records excel at modeling domain entities with clear, immutable data structures:

```csharp
public record Employee(string FirstName, string LastName, int Age, string Department);
```

### 2. Data Transformation Pipelines

Records work seamlessly with LINQ and functional transformations:

```csharp
var byDepartment = employees
    .GroupBy(e => e.Department)
    .OrderBy(g => g.Key);
```

### 3. State Management

In functional programming, state changes are represented as new immutable instances:

```csharp
// Instead of mutating:
// employee.Age++; // ❌ Not possible with records

// We create new state:
var olderEmployee = employee with { Age = employee.Age + 1 }; // ✅
```

### 4. Safe Sharing

Because records are immutable, they can be safely shared between different parts of your application without risk of unexpected modifications:

```csharp
var sharedEmployee = employee;
var modifiedEmployee = sharedEmployee with { Department = "Management" };
// sharedEmployee is unchanged, no side effects!
```

### 5. Collections and Aggregations

Records are ideal for collections because their value equality makes them work correctly with LINQ operations, grouping, and deduplication:

```csharp
var uniquePersons = persons.Distinct(); // Works correctly due to value equality
var isPresent = persons.Contains(targetPerson); // Compares by value
```

## Benefits in Functional Programming

1. **Thread Safety**: Immutable objects are inherently thread-safe
2. **Predictability**: No hidden side effects from mutation
3. **Testability**: Easier to test pure functions with immutable inputs
4. **Reasoning**: Easier to understand code when data doesn't change
5. **Temporal Logic**: Can maintain multiple versions of data across time
6. **Hash-Safe**: Safe to use as dictionary keys (hashcode won't change)

## When to Use Records

✅ **Use records when:**
- Modeling immutable data/domain entities
- Creating Data Transfer Objects (DTOs)
- Implementing value objects in Domain-Driven Design
- Working with functional programming patterns
- You need value-based equality
- Creating configuration objects

❌ **Avoid records when:**
- You need mutable state
- Working with entity framework entities (though possible, requires care)
- You need reference equality semantics
- Modeling objects with complex identity logic

## Comparison Summary

| Feature | Class | Record | Record Struct |
|---------|-------|--------|---------------|
| Type | Reference | Reference | Value |
| Default Equality | Reference | Value | Value |
| Immutability | Manual | Built-in (`init`) | Built-in (`init`) |
| `with` Expression | ❌ | ✅ | ✅ |
| Inheritance | ✅ | ✅ | ❌ |
| Deconstruction | Manual | Automatic | Automatic |
| Heap/Stack | Heap | Heap | Stack |

## Conclusion

C# records are a powerful feature that brings functional programming principles into C#. They reduce boilerplate code while encouraging immutability and value-based thinking. By providing built-in support for immutable data structures with value semantics, records make it easier to write functional-style code that is:

- **More maintainable**: Less boilerplate, clearer intent
- **Safer**: Immutability prevents bugs from unexpected mutations
- **More expressive**: Concise syntax focuses on what matters—the data
- **Better aligned with functional paradigms**: Natural support for immutability and value semantics

In the context of the provided examples, records demonstrate how C# embraces modern programming paradigms while maintaining the language's object-oriented roots, offering developers the best of both worlds.
