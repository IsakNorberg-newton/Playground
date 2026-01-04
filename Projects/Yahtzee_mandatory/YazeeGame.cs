using System.Collections.Immutable;
using Playground.Projects.Yahtzee.Extensions;
using Playground.Projects.Yahtzee.Models;
using PlayGround.Extensions;

namespace Playground.Projects.Yahtzee;

public static class YahzeeGame
{
    public static void RunSimulation()
    {
        Console.WriteLine("Testing the Cup of Dice.");

        var cupOfDice = new CupOfDice(10);
        Console.WriteLine($"Cup with 10 dice: {cupOfDice}\n");   

        cupOfDice = new CupOfDice(2);
        Console.WriteLine($"Cup with 2 dice: {cupOfDice}\n"); 


        var yahzeeCup = new YahzeeCup();
        Console.WriteLine($"Yahzee Cup with 5 dice: {yahzeeCup}\n");  

        Enumerable.Range(1, 10)
            .Aggregate(yahzeeCup, (currentCup, i) => 
            {
                var newCup = currentCup
                .ShakeAndRoll();

                newCup.Tap(cup => Console.WriteLine($"Yahzee Cup with 5 dice: {cup}"))
                .GetYahtzeeCombination()
                .Tap(ycombo =>  Console.WriteLine($"Yahzee Combination: {ycombo.GetType().Name}, Score: {ycombo.Score}\n"));
                
                return newCup;
            });  

        ImmutableList<Player> players = ImmutableList.Create(
            new Player("Alice", new YahzeeCup()),   
            new Player("Bob", new YahzeeCup()),
            new Player("Diana", new YahzeeCup()))

            .Tap(p => Console.WriteLine(string.Join("\n", p.Select(pl => $"{pl.Name} has Yahtzee cup: {pl.YahzeeCup}"))));


        System.Console.WriteLine("\nYahtzee Round Simulation:");
        System.Console.WriteLine("Your code should implement the Yahtzee round simulation below.");
        System.Console.WriteLine("========================");

        // Implement a Yahtzee round simulation here using functional patterns

        // use existing monadic extensions and functional patterns
        // minimize imperative code, maximize declative code using LINQ and extension methods

    }
}