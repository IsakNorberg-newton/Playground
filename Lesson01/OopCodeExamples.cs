namespace Playground.Lesson01;

public static class OopCodeExamples
{
        //Inheritance and Polymorphism
    public class Animal
    {
        public virtual string Speak() => "Some sound";
    }
    public class Dog : Animal
    {
        public override string Speak() => "Woof!";
    }

    //Abstraction with Interfaces
    public interface IShape
    {
        double Area();
    }
    public class Circle : IShape
    {
        public double Radius { get; }
        public Circle(double radius) => Radius = radius;
        public double Area() => Math.PI * Radius * Radius;
    }
    
    public static void RunExamples()
    {
        //Encapsulation with Classes        
        var account = new BankAccount();
        account.Deposit(100);
        account.Withdraw(30);

        System.Console.WriteLine($"Balance: {account.GetBalance()}"); // Balance: 70

        //Inheritance and Polymorphism
        Animal pet1 = new Animal();
        Animal pet2 = new Dog();

        System.Console.WriteLine(pet1.Speak()); // Some sound
        System.Console.WriteLine(pet2.Speak()); // Woof!


        //Abstraction with Interfaces
        IShape shape = new Circle(5);

        System.Console.WriteLine($"Area: {shape.Area()}"); // Area: 78.53981633974483
        //System.Console.WriteLine($"Radius: {shape.Radius}"); // Not accessible through IShape
    }

    //Encapsulation with Classes
    public class BankAccount
    {
        private decimal balance;

        public void Deposit(decimal amount) => balance += amount;
        public void Withdraw(decimal amount) => balance -= amount;
        public decimal GetBalance() => balance;
    }
}
