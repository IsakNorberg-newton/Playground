namespace Playground.Lesson02;

public static class Records
{
    // Traditional class for comparison
    public class PersonClass
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public int Age { get; init; }

        public PersonClass(string firstName, string lastName, int age)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
        }
    }

    // Simple record with positional syntax
    public record Person(string FirstName, string LastName, int Age);

    // Record with additional properties and methods
    public record Employee(string FirstName, string LastName, int Age, string Department)
    {
        public string FullName => $"{FirstName} {LastName}";
        
        public string GetInfo() => $"{FullName}, Age: {Age}, Dept: {Department}";
    }

    // Record with validation
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

    // Inheritance with records
    public record Vehicle(string Make, string Model, int Year);
    public record Car(string Make, string Model, int Year, int Doors) : Vehicle(Make, Model, Year);
    public record Motorcycle(string Make, string Model, int Year, string Type) : Vehicle(Make, Model, Year);

    // Record struct (value type)
    public record struct Point(double X, double Y)
    {
        public double DistanceFromOrigin => Math.Sqrt(X * X + Y * Y);
    }

    public static void RunExamples()
    {
        Console.WriteLine("\n=== Records Examples ===\n");

        // Example 1: Basic record creation and value equality
        Console.WriteLine("1. Basic Records and Value Equality:");
        var person1 = new Person("John", "Doe", 30);
        var person2 = new Person("John", "Doe", 30);
        var person3 = new Person("Jane", "Smith", 25);

        Console.WriteLine($"person1: {person1}");
        Console.WriteLine($"person2: {person2}");
        Console.WriteLine($"person1 == person2: {person1 == person2}"); // True - value equality
        Console.WriteLine($"person1 == person3: {person1 == person3}"); // False
        Console.WriteLine();

        // Example 2: Comparing with class (reference equality)
        Console.WriteLine("2. Class vs Record Equality:");
        var classObj1 = new PersonClass("John", "Doe", 30);
        var classObj2 = new PersonClass("John", "Doe", 30);
        Console.WriteLine($"Class equality (same values): {classObj1 == classObj2}"); // False - reference equality
        Console.WriteLine($"Record equality (same values): {person1 == person2}"); // True - value equality
        Console.WriteLine();

        // Example 3: Non-destructive mutation with 'with' expression
        Console.WriteLine("3. Non-destructive Mutation (with expression):");
        var employee1 = new Employee("Alice", "Johnson", 28, "Engineering");
        Console.WriteLine($"Original: {employee1.GetInfo()}");
        
        var employee2 = employee1 with { Age = 29 };
        Console.WriteLine($"After birthday: {employee2.GetInfo()}");
        Console.WriteLine($"Original unchanged: {employee1.GetInfo()}");
        
        var employee3 = employee1 with { Department = "Management", Age = 30 };
        Console.WriteLine($"Promoted: {employee3.GetInfo()}");
        Console.WriteLine();

        // Example 4: Deconstruction
        Console.WriteLine("4. Deconstruction:");
        var (firstName, lastName, age) = person1;
        Console.WriteLine($"Deconstructed: FirstName={firstName}, LastName={lastName}, Age={age}");
        Console.WriteLine();

        // Example 5: Records with validation and computed properties
        Console.WriteLine("5. Records with Validation and Computed Properties:");
        var temp1 = new Temperature(25);
        Console.WriteLine($"25°C = {temp1.Fahrenheit:F1}°F = {temp1.Kelvin:F2}K");
        
        var temp2 = new Temperature(0);
        Console.WriteLine($"0°C = {temp2.Fahrenheit:F1}°F = {temp2.Kelvin:F2}K");
        
        try
        {
            var invalidTemp = new Temperature(-300); // Below absolute zero
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Validation error: {ex.Message}");
        }
        Console.WriteLine();

        // Example 6: Record inheritance
        Console.WriteLine("6. Record Inheritance:");
        Vehicle vehicle = new Vehicle("Toyota", "Camry", 2022);
        Car car = new Car("Honda", "Civic", 2023, 4);
        Motorcycle bike = new Motorcycle("Harley", "Sportster", 2021, "Cruiser");

        Console.WriteLine($"Vehicle: {vehicle}");
        Console.WriteLine($"Car: {car}");
        Console.WriteLine($"Motorcycle: {bike}");
        
        // Polymorphism
        Vehicle[] vehicles = { vehicle, car, bike };
        Console.WriteLine("\nVehicles from 2022 or later:");
        foreach (var v in vehicles.Where(v => v.Year >= 2022))
        {
            Console.WriteLine($"  {v.Make} {v.Model} ({v.Year})");
        }
        Console.WriteLine();

        // Example 7: Record structs (value types)
        Console.WriteLine("7. Record Structs (Value Types):");
        var point1 = new Point(3, 4);
        var point2 = new Point(3, 4);
        Console.WriteLine($"point1: {point1}, Distance: {point1.DistanceFromOrigin:F2}");
        Console.WriteLine($"point1 == point2: {point1 == point2}");
        
        var point3 = point1 with { X = 6, Y = 8 };
        Console.WriteLine($"point3: {point3}, Distance: {point3.DistanceFromOrigin:F2}");
        Console.WriteLine();

        // Example 8: Collections of records
        Console.WriteLine("8. Collections of Records:");
        var employees = new List<Employee>
        {
            new("Bob", "Smith", 35, "Sales"),
            new("Carol", "White", 42, "Engineering"),
            new("Dave", "Brown", 28, "Sales"),
            new("Eve", "Green", 31, "Marketing")
        };

        Console.WriteLine("Employees by Department:");
        var byDepartment = employees
            .GroupBy(e => e.Department)
            .OrderBy(g => g.Key);

        foreach (var group in byDepartment)
        {
            Console.WriteLine($"  {group.Key}: {string.Join(", ", group.Select(e => e.FullName))}");
        }
        Console.WriteLine();

        // Example 9: Record ToString() override
        Console.WriteLine("9. Default ToString() Behavior:");
        Console.WriteLine($"Person: {person1}");
        Console.WriteLine($"Employee: {employee1}");
        Console.WriteLine($"Temperature: {temp1}");
        Console.WriteLine();

        Console.WriteLine("\n=== End of Records Examples ===\n");
    }
}
