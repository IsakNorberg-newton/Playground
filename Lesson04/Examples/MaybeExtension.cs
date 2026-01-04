using System.Reflection.Metadata.Ecma335;
using Models.Employees;
using Models.Music;
using PlayGround.Extensions;
using PlayGround.Generics;
using Seido.Utilities.SeedGenerator;

namespace Playground.Lesson04;

public static class MaybeExtension
{
    public static void RunExamples()
    {
        Console.WriteLine("\n=== Maybe Extension Methodss Examples ===\n");
        
        EmployeeBindExamples();
        MusicGroupBindExamples();

        Console.WriteLine("\n=== End of Maybe Extension Methods Examples ===\n");
    }
    
    static void EmployeeBindExamples()
    {
        Console.WriteLine("--- Employee Bind Examples ---");
        
        var seeder = new SeedGenerator();
        var employees = seeder.ItemsToList<Employee>(100);
        var noVets = employees.Where(e => e.Role != WorkRole.Veterinarian).ToList();
        var noManagement = employees.Where(e => e.Role != WorkRole.Management).ToList();
        var nullCreditCards = employees.Select(e => e with { CreditCards = null }).ToList();

        Console.WriteLine($"Working with {employees.Count} employees");
        
        // Example 1: Find first veterinarian and bind to create specialized email
        //var vetEmailResult = noVets.ToMaybe()  //just to show the graceful Nothing handling
        var vetEmailResult = employees.ToMaybe()
            .Bind(empList => empList.FirstOrDefault(e => e.Role == WorkRole.Veterinarian))
            .Tap(vet => Console.WriteLine($"Found Veterinarian: {vet.FirstName} {vet.LastName}"))
            .Bind(vet => $"dr.{vet.FirstName.ToLower()}.{vet.LastName.ToLower()}@vetclinic.com");
        
        DisplayResult("Veterinarian Email", vetEmailResult);
        
        // Example 2: Find senior management and bind to calculate team budget
        var managementBudgetResult = noManagement.ToMaybe()
            .Bind(empList => empList.Where(e => e.Role == WorkRole.Management && 
                (DateTime.Now.Year - e.HireDate.Year) >= 5).ToList())
            .Bind(managers => $"Senior Management Team: {managers.Count} managers, Budget: ${managers.Count * 120000:N0}");
        

        DisplayResult("Management Budget", managementBudgetResult);
        
        // Example 3: Find employees with most credit cards and bind to risk assessment
        //var creditRiskResult = nullCreditCards.ToMaybe() //just to show the graceful Error handling
        var creditRiskResult = employees.ToMaybe()
            .Bind(empList => empList.OrderByDescending(e => e.CreditCards.Count).Take(3).ToList())
            .Bind(topCreditUsers => {
                var riskProfiles = topCreditUsers.Select(emp => 
                    $"{emp.FirstName} {emp.LastName}: {emp.CreditCards.Count} cards ({emp.Role})").ToList();
                return $"High Credit Users:\n  " + string.Join("\n  ", riskProfiles);
            });
        
        DisplayResult("Credit Risk Assessment", creditRiskResult);
        
        // Example 4: Find newest employees and bind to onboarding group
        var onboardingResult = employees.ToMaybe()
            //.Bind<List<Employee>,List<Employee>>(empList => null) //just to show the graceful Error handling
            .Bind(empList => empList.Where(e => DateTime.Now.Year - e.HireDate.Year < 1).ToList())
            .Bind(newHires => {

                var roleDistribution = newHires.GroupBy(e => e.Role)
                    .Select(g => (WorkRole: g.Key,  Count: g.Count()))
                    .ToList();
                
                return roleDistribution;
            });
        
        //Showing detailed onboarding groups when tuple is involved
        Console.WriteLine($"Onboarding Groups: {onboardingResult switch {
            Something<List<(WorkRole WorkRole, int Count)>> success => $"New Hires ({success.Value.Sum(r => r.Count)}): {string.Join(", ", success.Value.Select(r => $"{r.WorkRole}({r.Count})"))}",
            Nothing<List<(WorkRole WorkRole, int Count)>> => "No data available",
            Error<List<(WorkRole WorkRole, int Count)>> error => $"Error - {error.CapturedError.Message}",
            _ => "Unknown state"
        }}");


        // Example 5: Calculate department statistics and bind to efficiency report  
        var efficiencyResult = employees.ToMaybe()
            //.Bind<List<Employee>, List<IGrouping<WorkRole, Employee>>>(empList => throw new Exception("Simulated failure during processing")) //just to show the graceful Error handling
            .Bind(empList => empList.GroupBy(e => e.Role).ToList())
            .Bind(departments => {
                var stats = departments.Select(dept => {
                    var avgTenure = dept.Average(e => DateTime.Now.Year - e.HireDate.Year);
                    var avgCards = dept.Average(e => e.CreditCards.Count);
                    return $"{dept.Key}: {dept.Count()} emp, {avgTenure:F1}yr avg, {avgCards:F1} cards avg";
                }).ToList();
                
                return "Department Efficiency:\n  " + string.Join("\n  ", stats);
            });
        
        DisplayResult("Efficiency Report", efficiencyResult);
        
        Console.WriteLine();
    }
    
    static void MusicGroupBindExamples()
    {
        Console.WriteLine("--- MusicGroup Bind Examples ---");
        
        var seeder = new SeedGenerator();
        var musicGroups = seeder.ItemsToList<MusicGroup>(100);
        
        Console.WriteLine($"Working with {musicGroups.Count} music groups");
        
        // Example 1: Find top-selling groups and bind to industry ranking
        var industryRankingResult = musicGroups.ToMaybe()
            .Bind(groups => groups.OrderByDescending(g => g.Albums.Sum(a => a.CopiesSold)).Take(5).ToList())
            .Bind(topGroups => {
                var rankings = topGroups.Select((group, index) => {
                    var totalSales = group.Albums.Sum(a => a.CopiesSold);
                    return $"#{index + 1}: {group.Name} ({group.Genre}) - {totalSales:N0} copies";
                }).ToList();
                
                return "Industry Top 5:\n  " + string.Join("\n  ", rankings);
            });
        
        DisplayResult("Industry Rankings", industryRankingResult);
        
        // Example 2: Filter by genre and bind to calculate market analysis
        var genreAnalysisResult = musicGroups.ToMaybe()
            .Bind(groups => groups.Where(g => g.Genre == MusicGenre.Rock).ToList())
            .Bind(rockGroups => {

                var totalRevenue = rockGroups.Sum(g => g.Albums.Sum(a => a.CopiesSold * 12.99m));
                var avgAlbumsPerGroup = rockGroups.Average(g => g.Albums.Count);
                var veteranGroups = rockGroups.Count(g => DateTime.Now.Year - g.EstablishedYear > 20);
                
                return $"Rock Market: {rockGroups.Count} groups, ${totalRevenue:N0} revenue, " +
                       $"{avgAlbumsPerGroup:F1} avg albums, {veteranGroups} veterans";
            });
        
        DisplayResult("Rock Genre Analysis", genreAnalysisResult);
        
        // Example 3: Find most productive groups and bind to collaboration potential
        var collaborationResult = musicGroups.ToMaybe()
            .Bind(groups => groups.Where(g => g.Albums.Count >= 5 && g.Artists.Count >= 3).ToList())
            .Bind(productiveGroups => {
                var collaborations = productiveGroups.Select(group => {
                    var productivity = (float)group.Albums.Count / Math.Max(1, DateTime.Now.Year - group.EstablishedYear);
                    return $"{group.Name}: {group.Artists.Count} artists, {productivity:F2} albums/year";
                }).ToList();
                
                return $"Collaboration Candidates ({collaborations.Count}):\n  " + string.Join("\n  ", collaborations);
            });
        
        DisplayResult("Collaboration Potential", collaborationResult);
        
        // Example 4: Analyze decade trends and bind to festival booking strategy
        var festivalStrategyResult = musicGroups.ToMaybe()
            .Bind(groups => groups.GroupBy(g => (DateTime.Now.Year - g.EstablishedYear) / 10 * 10).ToList())
            .Bind(decades => {
                var strategies = decades.Select(decade => {
                    var decadeLabel = decade.Key switch {
                        0 => "New Artists (0-9 years)",
                        10 => "Established (10-19 years)", 
                        20 => "Veterans (20-29 years)",
                        _ => $"Legacy ({decade.Key}+ years)"
                    };
                    
                    var totalSales = decade.Sum(g => g.Albums.Sum(a => a.CopiesSold));
                    var avgSales = decade.Average(g => g.Albums.Sum(a => a.CopiesSold));
                    
                    return $"{decadeLabel}: {decade.Count()} groups, {avgSales:N0} avg sales";
                }).ToList();
                
                return "Festival Booking Strategy:\n  " + string.Join("\n  ", strategies);
            });
        
        DisplayResult("Festival Strategy", festivalStrategyResult);
        
        // Example 5: Find cross-genre opportunities and bind to streaming playlist recommendations
        var playlistResult = musicGroups.ToMaybe()
            .Bind(groups => groups.GroupBy(g => g.Genre).ToList())
            .Bind(genreGroups => {
                var recommendations = genreGroups.Select(genre => {
                    var topPerformer = genre.OrderByDescending(g => g.Albums.Sum(a => a.CopiesSold)).First();
                    var groupCount = genre.Count();
                    var totalTracks = genre.Sum(g => g.Albums.Sum(a => a.Tracks.Count));
                    
                    var playlistStrategy = genre.Key switch {
                        MusicGenre.Rock => "Main Rock playlists + Classic Rock crossover",
                        MusicGenre.Jazz => "Jazz Focus + Smooth Jazz blends",
                        MusicGenre.Blues => "Blues Traditional + Blues Rock fusion",  
                        MusicGenre.Metal => "Metal Core + Progressive crossovers",
                        _ => "Genre-specific + Indie Discovery"
                    };
                    
                    return $"{genre.Key} ({groupCount} groups, {totalTracks} tracks): {playlistStrategy}" +
                           $" | Top: {topPerformer.Name}";
                }).ToList();
                
                return "Streaming Recommendations:\n  " + string.Join("\n  ", recommendations);
            });
        
        DisplayResult("Playlist Strategy", playlistResult);
        
        Console.WriteLine();
    }

    
    private static void DisplayResult<T>(string operation, Maybe<T> result)
    {
        Console.WriteLine($"{operation}: {result switch {
            Something<T> success => $"{success.Value}",
            Nothing<T> => "No data available",
            Error<T> error => $"Error - {error.CapturedError.Message}",
            _ => "Unknown state"
        }}");
    }
}

