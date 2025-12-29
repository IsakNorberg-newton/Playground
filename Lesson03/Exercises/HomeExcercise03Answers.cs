using System.Text.Json.Serialization;
using Models.Friends;
using Models.Music;
using PlayGround.Extensions;
using PlayGround.Generics;
using Seido.Utilities.SeedGenerator;

namespace Playground.Lesson03;

public static class HomeExercise03Answers
{
    public static void Entry(string[] args = null)
    {      
        System.Console.WriteLine("=== Lesson 03 Home Exercises ===\n");
        System.Console.WriteLine("Functional Programming Extensions with Friend Models\n");
        
        var (_, friendNoAddress, friendNoPets, friendNoQuotes, friendHasMany) = HomeExercise03Init.GetData(true);
        System.Console.WriteLine($"Loaded Friends: \n1. {friendNoAddress}\n2. {friendNoPets}\n3. {friendNoQuotes}\n4. {friendHasMany}\n");
        
        // Exercise 1: Map Extension - Transform friend data
        MapExercises(friendNoAddress, friendNoPets, friendNoQuotes, friendHasMany);
        
        // Exercise 2: Tap Extension - Side effects and logging
        TapExercises(friendNoAddress, friendNoPets, friendNoQuotes, friendHasMany);
        
        // Exercise 3: Fork Extension - Parallel transformations
        ForkExercises(friendNoAddress, friendNoPets, friendNoQuotes, friendHasMany);
        
        // Exercise 4: Alt Extension - Fallback operations
        AltExercises(friendNoAddress, friendNoPets, friendNoQuotes, friendHasMany);
        
        // Exercise 5: Compose Extension - Function composition
        ComposeExercises(friendNoAddress, friendNoPets, friendNoQuotes, friendHasMany);
        
        // Exercise 6: Maybe Monad - Null safety handling
        MaybeExercises(friendNoAddress, friendNoPets, friendNoQuotes, friendHasMany);
        
        // Exercise 7: Either Monad - Error handling
        EitherExercises(friendNoAddress, friendNoPets, friendNoQuotes, friendHasMany);
        
        System.Console.WriteLine("\n=== End of Lesson 03 Home Exercises ===");
    }

    #region Exercise 1: Map Extension
    private static void MapExercises(Friend friendNoAddress, Friend friendNoPets, Friend friendNoQuotes, Friend friendHasMany)
    {
        System.Console.WriteLine("\n--- Exercise 1: Map Extension ---");
        System.Console.WriteLine("Transform friend data using Map extension");

        var n = friendHasMany.Map(f => $"{f.FirstName} {f.LastName}");
        System.Console.WriteLine(n);
        }
    #endregion

    #region Exercise 2: Tap Extension
    private static void TapExercises(Friend friendNoAddress, Friend friendNoPets, Friend friendNoQuotes, Friend friendHasMany)
    {
        System.Console.WriteLine("\n--- Exercise 2: Tap Extension ---");
        System.Console.WriteLine("Add side effects and logging using Tap extension");
    }
    #endregion

    #region Exercise 3: Fork Extension
    private static void ForkExercises(Friend friendNoAddress, Friend friendNoPets, Friend friendNoQuotes, Friend friendHasMany)
    {
        System.Console.WriteLine("\n--- Exercise 3: Fork Extension ---");
        System.Console.WriteLine("Perform parallel operations using Fork extension");
    
        var s = friendHasMany.Fork(
            f => f.Pets.Select(p => p.Name).DistinctBy(p => p),
            f => f.Quotes.Select(q => q.Author).DistinctBy(p => p),
            (pnames, anames) => $"{friendHasMany.FirstName} has these pets {string.Join(",",pnames)} and likes {string.Join(",",anames)}"
        );

        System.Console.WriteLine(s);


    }
    #endregion

    #region Exercise 4: Alt Extension  
    private static void AltExercises(Friend friendNoAddress, Friend friendNoPets, Friend friendNoQuotes, Friend friendHasMany)
    {
        System.Console.WriteLine("\n--- Exercise 4: Alt Extension ---");
        System.Console.WriteLine("Provide fallback values using Alt extension");
    }
    #endregion

    #region Exercise 5: Compose Extension
    private static void ComposeExercises(Friend friendNoAddress, Friend friendNoPets, Friend friendNoQuotes, Friend friendHasMany)
    {
        System.Console.WriteLine("\n--- Exercise 5: Compose Extension ---");
        System.Console.WriteLine("Chain functions using Compose extension");

        Func<Friend, string> getFullName = e => $"{e.FirstName} {e.LastName}";
        Func<string, string> formatName = name => name.ToUpper();
        var nameFormatter = getFullName.Compose(formatName);

        Func<string, string>  titledName = name => $"Mr/Ms: {name}";
        var fullNameWithTitle = nameFormatter.Compose(titledName);

        Func<string, string>  greetName = name => $"Hello, {name}!";
        var greetFullName = fullNameWithTitle.Compose(greetName);

        System.Console.WriteLine(greetFullName(friendHasMany));
    }
    #endregion

    #region Exercise 6: Maybe Monad
    private static void MaybeExercises(Friend friendNoAddress, Friend friendNoPets, Friend friendNoQuotes, Friend friendHasMany)
    {
        System.Console.WriteLine("\n--- Exercise 6: Maybe Monad ---");
        System.Console.WriteLine("Handle nullable values safely using Maybe monad");
    }
    #endregion

    #region Exercise 7: Either Monad
    private static void EitherExercises(Friend friendNoAddress, Friend friendNoPets, Friend friendNoQuotes, Friend friendHasMany)
    {
        System.Console.WriteLine("\n--- Exercise 7: Either Monad ---");
        System.Console.WriteLine("Handle success/error cases using Either monad");
    }
   #endregion
}