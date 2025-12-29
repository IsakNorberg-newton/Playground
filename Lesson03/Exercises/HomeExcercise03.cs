using Models.Friends;
using Models.Music;
using PlayGround.Extensions;
using PlayGround.Generics;
using Seido.Utilities.SeedGenerator;

namespace Playground.Lesson03;

public static class HomeExercise03
{
    public static void Entry(string[] args = null)
    {      
        System.Console.WriteLine("=== Lesson 03 Home Exercises ===\n");
        System.Console.WriteLine("Functional Programming Extensions with Friend Models\n");
        
        var (_, friendNoAddress, friendNoPets, friendNoQuotes, friendHasMany) = HomeExercise03Init.GetData(false);
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

        // 1. Single Transformation: Extract full name from friendHasMany
        var fullName = friendHasMany.Map(f => $"{f.FirstName} {f.LastName}");
        System.Console.WriteLine($"1. Full Name: {fullName}");

        // 2. Conditional Mapping: Calculate age information from birthday data
        var ageInfo = friendHasMany.Map(f => f.Birthday.HasValue 
            ? $"{DateTime.Now.Year - f.Birthday.Value.Year} years old" 
            : "Birthday unknown");
        System.Console.WriteLine($"2. Age Info: {ageInfo}");

        // 3. Multiple Transformations: Convert names to uppercase and email to lowercase
        var updatedFriend = friendNoAddress.Map(
            f => f with { FirstName = char.ToUpper(f.FirstName[0]) + f.FirstName.Substring(1).ToLower() }, // do only f.FirstName.ToUpper() if one wants the whole name to be uppercase
            f => f with { LastName = char.ToUpper(f.LastName[0]) + f.LastName.Substring(1).ToLower() }, // do only f.LastName.ToUpper() if one wants the whole name to be uppercase
            f => f with { Email = f.Email.ToLower() }
        );
        System.Console.WriteLine($"3. Updated Friend: {updatedFriend.FirstName} {updatedFriend.LastName} ({updatedFriend.Email})");

        // 4. Complex Mapping: Create anonymous object summary
        var summary = friendHasMany.Map(f => new {
            FullName = $"{f.FirstName} {f.LastName}",
            HasAddress = f.Address != null,
            PetCount = f.Pets.Count,
            QuoteCount = f.Quotes.Count,
            ContactEmail = f.Email
        });
        System.Console.WriteLine($"4. Summary: {summary.FullName}, Address: {(summary.HasAddress ? "Yes" : "No")}, Pets: {summary.PetCount}, Quotes: {summary.QuoteCount}, Email: {summary.ContactEmail}");
    }
    #endregion

    #region Exercise 2: Tap Extension
    private static void TapExercises(Friend friendNoAddress, Friend friendNoPets, Friend friendNoQuotes, Friend friendHasMany)
    {
        System.Console.WriteLine("\n--- Exercise 2: Tap Extension ---");
        System.Console.WriteLine("Add side effects and logging using Tap extension");

        // 1. Logging Chain: Create a processing chain using friendHasMany
        friendHasMany
            .Tap(f => System.Console.WriteLine($"Processing friend: {f.FirstName} {f.LastName}"))
            .Tap(f => System.Console.WriteLine($"Email: {f.Email}"))
            .Tap(f => System.Console.WriteLine($"Address availability: {(f.Address != null ? "Available" : "Missing")}"))
            .Tap(f => System.Console.WriteLine($"Pet count: {f.Pets.Count}"))
            .Tap(f => System.Console.WriteLine($"Quote count: {f.Quotes.Count}"));

        // 2. Validation Logging: Use friendNoAddress to create a validation chain
        friendNoAddress
            .Tap(f => System.Console.WriteLine("Validation start..."))
            .Tap(f => {
                if (f.Address == null) System.Console.WriteLine("Warning: Missing address information");
            })
            .Tap(f => System.Console.WriteLine($"Pet information: {(f.Pets.Count > 0 ? $"{f.Pets.Count} pets found" : "No pets found")}"));
    }
    #endregion

    #region Exercise 3: Fork Extension
    private static void ForkExercises(Friend friendNoAddress, Friend friendNoPets, Friend friendNoQuotes, Friend friendHasMany)
    {
        System.Console.WriteLine("\n--- Exercise 3: Fork Extension ---");
        System.Console.WriteLine("Perform parallel operations using Fork extension");

        // 1. Parallel Data Extraction: Extract pet names and unique quote authors
        var petAndQuoteSummary = friendHasMany.Fork(
            f => string.Join(", ", f.Pets.Select(p => p.Name)),
            f => string.Join(", ", f.Quotes.Select(q => q.Author).Distinct()),
            (pets, authors) => $"Pets: [{pets}] | Quote Authors: [{authors}]"
        );
        System.Console.WriteLine($"1. Summary: {petAndQuoteSummary}");

        // 2. Score Calculations: Contact score and Social score
        var scoreAnalysis = friendHasMany.Fork(
            f => (f.Address != null ? 1 : 0) + (!string.IsNullOrEmpty(f.Email) ? 1 : 0),
            f => f.Pets.Count + f.Quotes.Count,
            (contactScore, socialScore) => $"Contact Score: {contactScore}, Social Score: {socialScore}"
        );
        System.Console.WriteLine($"2. Analysis: {scoreAnalysis}");

        // 3. Comparison Analysis: Check pet and address status
        var comparison = friendNoAddress.Fork(
            f => f.Pets.Count > 0 ? "Has Pets" : "No Pets",
            f => f.Address != null ? "Has Address" : "No Address",
            (petStatus, addrStatus) => $"{petStatus} and {addrStatus}"
        );
        System.Console.WriteLine($"3. Status: {comparison}");
    }
    #endregion

    #region Exercise 4: Alt Extension  
    private static void AltExercises(Friend friendNoAddress, Friend friendNoPets, Friend friendNoQuotes, Friend friendHasMany)
    {
        System.Console.WriteLine("\n--- Exercise 4: Alt Extension ---");
        System.Console.WriteLine("Provide fallback values using Alt extension");

        // 1. Address Fallbacks: try street, then city, then fallback string
        var addrInfo = friendNoAddress.Alt(
            f => f.Address?.StreetAddress,
            f => f.Address?.City,
            f => "No address available"
        );
        System.Console.WriteLine($"1. Address Info: {addrInfo}");

        // 2. Pet Information: try first pet, then count, then fallback
        var petInfo = friendNoPets.Alt(
            f => f.Pets.FirstOrDefault()?.Name,
            f => f.Pets.Count > 0 ? $"{f.Pets.Count} pets" : null,
            f => "No pets found"
        );
        System.Console.WriteLine($"2. Pet Info: {petInfo}");

        // 3. Quote Information: try first quote text, then author, then fallback
        var quoteInfo = friendNoQuotes.Alt(
            f => f.Quotes.FirstOrDefault()?.QuoteText,
            f => f.Quotes.FirstOrDefault()?.Author,
            f => "No quotes available"
        );
        System.Console.WriteLine($"3. Quote Info: {quoteInfo}");

        // 4. Contact Method: prioritize email, then address, then fallback
        var contactMethod = friendHasMany.Alt(
            f => f.Email,
            f => f.Address?.StreetAddress,
            f => "No contact method available"
        );
        System.Console.WriteLine($"4. Contact Method: {contactMethod}");
    }
    #endregion

    #region Exercise 5: Compose Extension
    private static void ComposeExercises(Friend friendNoAddress, Friend friendNoPets, Friend friendNoQuotes, Friend friendHasMany)
    {
        System.Console.WriteLine("\n--- Exercise 5: Compose Extension ---");
        System.Console.WriteLine("Chain functions using Compose extension");

        // 1. Greeting Pipeline: Extracts name, uppercase, adds title, adds greeting
        Func<Friend, string> getFullName = f => $"{f.FirstName} {f.LastName}";
        Func<string, string> toUpper = s => s.ToUpper();
        Func<string, string> addTitle = s => $"Mr./Ms. {s}";
        Func<string, string> addGreeting = s => $"Hello, {s}";

        var greetingPipeline = getFullName
            .Compose(toUpper)
            .Compose(addTitle)
            .Compose(addGreeting);

        System.Console.WriteLine($"1. Greeting: {greetingPipeline(friendHasMany)}");

        // 2. Pet Analysis Pipeline: count -> format -> add prefix
        Func<Friend, int> getPetCount = f => f.Pets.Count;
        Func<int, string> formatCount = c => $"Count: {c}";
        Func<string, string> addPetLabel = s => $"Pets {s}";

        var petPipeline = getPetCount
            .Compose(formatCount)
            .Compose(addPetLabel);

        System.Console.WriteLine($"2. Pet Analysis: {petPipeline(friendHasMany)}");

        // 3. Email Domain Analysis: email -> extract domain -> format
        Func<Friend, string> getEmail = f => f.Email;
        Func<string, string> extractDomain = e => e.Contains('@') ? e.Split('@')[1] : "unknown";
        Func<string, string> formatDomain = d => $"Domain: {d}";

        var domainPipeline = getEmail
            .Compose(extractDomain)
            .Compose(formatDomain);

        System.Console.WriteLine($"3. Email Analysis: {domainPipeline(friendNoAddress)}");
    }
    #endregion

    #region Exercise 6: Maybe Monad
    private static void MaybeExercises(Friend friendNoAddress, Friend friendNoPets, Friend friendNoQuotes, Friend friendHasMany)
    {
        System.Console.WriteLine("\n--- Exercise 6: Maybe Monad ---");
        System.Console.WriteLine("Handle nullable values safely using Maybe monad");

        // 1. Address Handling
        System.Console.WriteLine("1. Address Handling:");
        DisplayMaybeResult(GetAddressInfo(friendNoAddress));
        DisplayMaybeResult(GetAddressInfo(friendHasMany));

        // 2. Age Calculation
        System.Console.WriteLine("\n2. Age Calculation:");
        DisplayMaybeResult(CalculateAge(friendHasMany));

        // 3. Pet Selection
        System.Console.WriteLine("\n3. Pet Selection:");
        DisplayMaybeResult(GetFavoritePet(friendNoPets));
        DisplayMaybeResult(GetFavoritePet(friendHasMany));

        // 4. Quote Selection
        System.Console.WriteLine("\n4. Quote Selection:");
        DisplayMaybeResult(GetRandomQuote(friendNoQuotes));
        DisplayMaybeResult(GetRandomQuote(friendHasMany));
    }

    private static Maybe<string> GetAddressInfo(Friend f)
    {
        try {
            if (f.Address == null) return new Nothing<string>();
            return new Something<string>($"{f.Address.StreetAddress}, {f.Address.City}");
        }
        catch (Exception e) { return new Error<string>(e); }
    }

    private static Maybe<int> CalculateAge(Friend f)
    {
        try {
            if (!f.Birthday.HasValue) return new Nothing<int>();
            var today = DateTime.Today;
            var age = today.Year - f.Birthday.Value.Year;
            if (f.Birthday.Value.Date > today.AddYears(-age)) age--;
            return new Something<int>(age);
        }
        catch (Exception e) { return new Error<int>(e); }
    }

    private static Maybe<string> GetFavoritePet(Friend f)
    {
        try {
            if (f.Pets.Count == 0) return new Nothing<string>();
            var p = f.Pets[new Random().Next(f.Pets.Count)];
            return new Something<string>($"{p.Name} ({p.Kind}, Mood: {p.Mood})");
        }
        catch (Exception e) { return new Error<string>(e); }
    }

    private static Maybe<string> GetRandomQuote(Friend f)
    {
        try {
            if (f.Quotes.Count == 0) return new Nothing<string>();
            var q = f.Quotes[new Random().Next(f.Quotes.Count)];
            return new Something<string>($"\"{q.QuoteText}\" - {q.Author}");
        }
        catch (Exception e) { return new Error<string>(e); }
    }

    private static void DisplayMaybeResult<T>(Maybe<T> result)
    {
        switch (result)
        {
            case Something<T> s: System.Console.WriteLine($"[SUCCESS] Value: {s.Value}"); break;
            case Nothing<T>: System.Console.WriteLine("[INFO] No data available"); break;
            case Error<T> e: System.Console.WriteLine($"[ERROR] {e.CapturedError.Message}"); break;
        }
    }
    #endregion

    #region Exercise 7: Either Monad
    private static void EitherExercises(Friend friendNoAddress, Friend friendNoPets, Friend friendNoQuotes, Friend friendHasMany)
    {
        System.Console.WriteLine("\n--- Exercise 7: Either Monad ---");
        System.Console.WriteLine("Handle success/error cases using Either monad");

        // 1. Friend Validation
        System.Console.WriteLine("1. Friend Validation:");
        DisplayEitherResult(ValidateFriend(friendNoAddress));
        DisplayEitherResult(ValidateFriend(friendHasMany));

        // 2. Email Validation
        System.Console.WriteLine("\n2. Email Validation:");
        DisplayEitherResult(ValidateEmail("valid.email@test.com"));
        DisplayEitherResult(ValidateEmail("invalid-email"));

        // 3. Social Score Calculation
        System.Console.WriteLine("\n3. Social Score Calculation:");
        DisplayEitherResult(CalculateSocialScore(friendNoPets));
        DisplayEitherResult(CalculateSocialScore(friendHasMany));
    }

    private static Either<string, string> ValidateFriend(Friend f)
    {
        var errors = new List<string>();
        if (string.IsNullOrEmpty(f.FirstName)) errors.Add("Missing First Name");
        if (string.IsNullOrEmpty(f.LastName)) errors.Add("Missing Last Name");
        if (string.IsNullOrEmpty(f.Email)) errors.Add("Missing Email");
        if (f.Address == null) errors.Add("Missing Address");

        if (errors.Any()) return new Left<string, string>(string.Join(", ", errors));
        return new Right<string, string>($"Friend {f.FirstName} is valid");
    }

    private static Either<string, string> ValidateEmail(string email)
    {
        if (string.IsNullOrEmpty(email)) return new Left<string, string>("Email is empty");
        if (!email.Contains("@")) return new Left<string, string>("Email missing @ symbol");
        return new Right<string, string>($"Email {email} is valid");
    }

    private static Either<string, int> CalculateSocialScore(Friend f)
    {
        int score = 0;
        if (!string.IsNullOrEmpty(f.FirstName)) score += 10;
        if (!string.IsNullOrEmpty(f.Email)) score += 10;
        if (f.Address != null) score += 20;
        if (f.Birthday.HasValue) score += 10;
        score += f.Pets.Count * 5;
        score += f.Quotes.Count * 3;

        if (score < 30) return new Left<string, int>($"Score {score} is too low (minimum 30)");
        return new Right<string, int>(score);
    }

    private static void DisplayEitherResult<TLeft, TRight>(Either<TLeft, TRight> result)
    {
        switch (result)
        {
            case Right<TLeft, TRight> r: System.Console.WriteLine($"[OK] {r.Value}"); break;
            case Left<TLeft, TRight> l: System.Console.WriteLine($"[FAIL] {l.Value}"); break;
        }
    }
   #endregion
}