namespace Playground.Lesson01;

public static class ImperativeVsDeclarative2
{   
    public class Item
    {
        public string Name { get; }
        public decimal Price { get; }
        public decimal Discount { get; }

        public Item(string name, decimal price, decimal discount)
        {
            Name = name;
            Price = price;
            Discount = discount;
        }
    }

    public static void RunExamples()
    {
        //Imperative vs Declarative Example2
        var cart = new List<Item>
        {
            new Item("Book", 12.99m, 0.10m),   // 10% discount
            new Item("Pen", 1.50m, 0.0m),      // no discount
            new Item("Notebook", 5.00m, 0.05m) // 5% discount
        };

        decimal taxRate = 0.25m; // 25% VAT

        decimal total = 0;

        foreach (var item in cart)
        {
            decimal discountedPrice = item.Price * (1 - item.Discount);
            decimal taxedPrice = discountedPrice * (1 + taxRate);
            total += taxedPrice;
        }

        Console.WriteLine($"Total: {total:F2}");

        //Declarative Style
        total = cart.Aggregate(0m, (acc, item) =>
        {
            var discounted = item.Price * (1 - item.Discount);
            var taxed = discounted * (1 + taxRate);
            return acc + taxed;
        });

        Console.WriteLine($"Total: {total:F2}");
    }
}
