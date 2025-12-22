using System.Collections.Immutable;

namespace Playground.Lesson02;

public static class ImmutableLists
{
    public record ShoppingCart(
        string Owner,                     // Direct property
        DateTime CreatedDate,              // Direct property
        ImmutableList<string> Items        // Immutable list
    );

    public static void RunExamples()
    {
        Console.WriteLine("\n=== Immutable Lists Examples ===\n");

        // Initial cart
        var cart1 = new ShoppingCart(
            Owner: "Alice",
            CreatedDate: DateTime.Now,
            Items: System.Collections.Immutable.ImmutableList.Create("Apples", "Bananas")
        );

        Console.WriteLine($"Cart1 Owner: {cart1.Owner}");
        Console.WriteLine($"Cart1 Created: {cart1.CreatedDate}");
        Console.WriteLine("Cart1 Items: " + string.Join(", ", cart1.Items));
        // Output: Cart1 Owner: Alice
        //         Cart1 Created: <timestamp>
        //         Cart1 Items: Apples, Bananas

        // Create a new cart with an extra item (immutability preserved)
        var cart2 = cart1 with
        {
            Items = cart1.Items.Add("Oranges")
        };

        Console.WriteLine("\nCart2 Items: " + string.Join(", ", cart2.Items));
        // Output: Cart2 Items: Apples, Bananas, Oranges

        // Create another cart with a different owner
        var cart3 = cart2 with
        {
            Owner = "Bob"
        };

        Console.WriteLine($"\nCart3 Owner: {cart3.Owner}");
        Console.WriteLine("Cart3 Items: " + string.Join(", ", cart3.Items));
        // Output: Cart3 Owner: Bob
        //         Cart3 Items: Apples, Bananas, Oranges

        Console.WriteLine("\n=== End of Immutable Lists Examples ===\n");
    }
}
