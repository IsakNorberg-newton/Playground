using Seido.Utilities.SeedGenerator;
using Models.Employees;
using Models.Music;
using PlayGround.Extensions;

namespace Playground.Lesson03;

public static class ComposeExtension
{
    public static void RunExamples()
    {
        Console.WriteLine("\n=== Compose Extension Method Examples ===\n");
        
        EmployeeComposeExamples();
        MusicGroupComposeExamples();
        AdvancedCompositionExamples();

        Console.WriteLine("\n=== End of Compose Extension Method Examples ===\n");
    }

    static void EmployeeComposeExamples()
    {
        Console.WriteLine("--- Employee Compose Examples ---");
        
        var seeder = new SeedGenerator();
        var employee = new Employee().Seed(seeder);
        
        Console.WriteLine($"Original Employee: {employee.FirstName} {employee.LastName}, Role: {employee.Role}, Hire Date: {employee.HireDate:yyyy-MM-dd}");
        
        // Example 1: Compose employee name extraction with formatting
        Func<Employee, string> getFullName = e => $"{e.FirstName} {e.LastName}";
        Func<string, string> formatName = name => name.ToUpper();
        var nameFormatter = getFullName.Compose(formatName);

        var formattedName = nameFormatter(employee);
        Console.WriteLine($"Formatted Name: {formattedName}");
        
        // Example 2: Compose tenure calculation with category assignment
        Func<Employee, int> calculateTenure = e => DateTime.Now.Year - e.HireDate.Year;
        Func<int, string> categorizeExperience = years => years switch
        {
            >= 15 => "Veteran Employee",
            >= 10 => "Senior Employee", 
            >= 5 => "Experienced Employee",
            >= 2 => "Regular Employee",
            _ => "New Employee"
        };
        var experienceClassifier = calculateTenure.Compose(categorizeExperience);

        var experienceLevel = experienceClassifier(employee);
        Console.WriteLine($"Experience Level: {experienceLevel} ({calculateTenure(employee)} years)");
        
        // Example 3: Compose credit card count with risk assessment
        Func<Employee, int> getCreditCardCount = e => e.CreditCards.Count;
        Func<int, string> assessCreditRisk = count => count switch
        {
            0 => "No Credit Risk Data",
            1 => "Low Credit Risk",
            2 => "Moderate Credit Risk",
            >= 3 => "High Credit Activity",
            _ => "Unknown"
        };
        
        var creditRiskAnalyzer = getCreditCardCount.Compose(assessCreditRisk);
        var riskAssessment = creditRiskAnalyzer(employee);
        Console.WriteLine($"Credit Risk Assessment: {riskAssessment} ({getCreditCardCount(employee)} cards)");
        
        // Example 4: Compose role extraction with salary estimation
        Func<Employee, WorkRole> getRole = e => e.Role;
        Func<WorkRole, decimal> estimateSalary = role => role switch
        {
            WorkRole.Management => 85000m,
            WorkRole.Veterinarian => 75000m,
            WorkRole.ProgramCoordinator => 60000m,
            WorkRole.AnimalCare => 45000m,
            WorkRole.Maintenance => 40000m,
            _ => 35000m
        };
        
        var salaryEstimator = getRole.Compose(estimateSalary);
        var estimatedSalary = salaryEstimator(employee);
        Console.WriteLine($"Estimated Salary: ${estimatedSalary:N0} for {getRole(employee)}");
        
        // Example 5: Chain multiple compositions - employee profile builder
        Func<Employee, (string name, int tenure, WorkRole role)> extractBasics = 
            e => ($"{e.FirstName} {e.LastName}", DateTime.Now.Year - e.HireDate.Year, e.Role);
        
        Func<(string name, int tenure, WorkRole role), string> buildProfile = 
            data => $"Profile: {data.name} | {data.role} | {data.tenure} years experience | " +
                   $"Level: {(data.tenure >= 10 ? "Senior" : data.tenure >= 5 ? "Mid" : "Junior")}";
        
        var profileBuilder = extractBasics.Compose(buildProfile);
        var employeeProfile = profileBuilder(employee);
        Console.WriteLine($"Employee Profile: {employeeProfile}");
        
        Console.WriteLine();
    }

    static void MusicGroupComposeExamples()
    {
        Console.WriteLine("--- MusicGroup Compose Examples ---");
        
        var seeder = new SeedGenerator();
        var musicGroup = new MusicGroup().Seed(seeder);
        
        Console.WriteLine($"Original Group: {musicGroup.Name}, Genre: {musicGroup.Genre}, Est: {musicGroup.EstablishedYear}, Albums: {musicGroup.Albums.Count}");
        
        // Example 1: Compose name extraction with branding
        Func<MusicGroup, string> getGroupName = g => g.Name;
        Func<string, string> addBranding = name => $"🎵 {name} 🎵";
        
        var brandingFormatter = getGroupName.Compose(addBranding);
        var brandedName = brandingFormatter(musicGroup);
        Console.WriteLine($"Branded Name: {brandedName}");
        
        // Example 2: Compose career span calculation with era classification
        Func<MusicGroup, int> calculateCareerSpan = g => DateTime.Now.Year - g.EstablishedYear;
        Func<int, string> classifyEra = years => years switch
        {
            >= 40 => "Legendary Era",
            >= 30 => "Classic Era",
            >= 20 => "Veteran Era",
            >= 10 => "Established Era",
            >= 5 => "Modern Era",
            _ => "New Era"
        };
        
        var eraClassifier = calculateCareerSpan.Compose(classifyEra);
        var musicalEra = eraClassifier(musicGroup);
        Console.WriteLine($"Musical Era: {musicalEra} ({calculateCareerSpan(musicGroup)} years active)");
        
        // Example 3: Compose total sales calculation with success tier
        Func<MusicGroup, long> calculateTotalSales = g => g.Albums.Sum(a => a.CopiesSold);
        Func<long, string> determineSuccessTier = sales => sales switch
        {
            >= 10_000_000 => "Diamond Success",
            >= 5_000_000 => "Multi-Platinum Success",
            >= 1_000_000 => "Platinum Success",
            >= 500_000 => "Gold Success",
            >= 100_000 => "Silver Success",
            >= 10_000 => "Bronze Success",
            _ => "Emerging Success"
        };
        
        var successAnalyzer = calculateTotalSales.Compose(determineSuccessTier);
        var successLevel = successAnalyzer(musicGroup);
        Console.WriteLine($"Success Level: {successLevel} ({calculateTotalSales(musicGroup):N0} total sales)");
        
        // Example 4: Compose genre extraction with market positioning
        Func<MusicGroup, MusicGenre> getGenre = g => g.Genre;
        Func<MusicGenre, string> analyzeMarketPosition = genre => genre switch
        {
            MusicGenre.Rock => "Mainstream Appeal - High Commercial Potential",
            MusicGenre.Jazz => "Niche Market - High Artistic Recognition",
            MusicGenre.Blues => "Traditional Market - Steady Fanbase",
            MusicGenre.Metal => "Specialized Market - Dedicated Following",
            _ => "Unknown Market Position"
        };
        
        var marketAnalyzer = getGenre.Compose(analyzeMarketPosition);
        var marketPosition = marketAnalyzer(musicGroup);
        Console.WriteLine($"Market Position: {marketPosition}");
        
        // Example 5: Compose productivity metrics with band classification
        Func<MusicGroup, double> calculateProductivity = g => 
            g.Albums.Count > 0 ? (double)g.Albums.Sum(a => a.Tracks.Count) / g.Albums.Count : 0;
        
        Func<double, string> classifyProductivity = avgTracks => avgTracks switch
        {
            >= 15 => "Prolific Band - High Output",
            >= 12 => "Productive Band - Consistent Output",
            >= 10 => "Standard Band - Regular Output",
            >= 8 => "Focused Band - Quality Over Quantity",
            > 0 => "Selective Band - Minimal Output",
            _ => "No Output Data"
        };
        
        var productivityClassifier = calculateProductivity.Compose(classifyProductivity);
        var productivityLevel = productivityClassifier(musicGroup);
        Console.WriteLine($"Productivity Level: {productivityLevel} ({calculateProductivity(musicGroup):F1} avg tracks/album)");
        
        // Example 6: Complex composition chain - comprehensive band analysis
        Func<MusicGroup, (string name, MusicGenre genre, int years, int albums, long sales)> extractMetrics =
            g => (g.Name, g.Genre, DateTime.Now.Year - g.EstablishedYear, g.Albums.Count, g.Albums.Sum(a => a.CopiesSold));
        
        Func<(string name, MusicGenre genre, int years, int albums, long sales), string> generateAnalysis = 
            data => $"Band Analysis: {data.name} ({data.genre}) has been active for {data.years} years, " +
                   $"released {data.albums} albums, sold {data.sales:N0} copies. " +
                   $"Status: {(data.sales > 1_000_000 && data.years > 15 ? "Hall of Fame Candidate" : 
                             data.sales > 500_000 && data.years > 10 ? "Established Success" :
                             data.albums > 5 && data.years > 5 ? "Growing Artist" : "Emerging Talent")}";
        
        var comprehensiveAnalyzer = extractMetrics.Compose(generateAnalysis);
        var fullAnalysis = comprehensiveAnalyzer(musicGroup);
        Console.WriteLine($"Comprehensive Analysis: {fullAnalysis}");
        
        Console.WriteLine();
    }

    static void AdvancedCompositionExamples()
    {
        Console.WriteLine("--- Advanced Composition Examples ---");
        
        var seeder = new SeedGenerator();
        var employee = new Employee().Seed(seeder);
        var musicGroup = new MusicGroup().Seed(seeder);
        
        // Example 1: Multi-step composition pipeline for employee evaluation
        Func<Employee, int> getTenure = e => DateTime.Now.Year - e.HireDate.Year;
        Func<int, double> calculateExperienceMultiplier = years => 1.0 + (years * 0.05);
        Func<double, string> formatMultiplier = mult => $"{mult:P0}";
        
        var experiencePipeline = getTenure
            .Compose(calculateExperienceMultiplier)
            .Compose(formatMultiplier);
        
        var experienceBonus = experiencePipeline(employee);
        Console.WriteLine($"Employee {employee.FirstName} {employee.LastName} experience multiplier: {experienceBonus}");
        
        // Example 2: Genre-based recommendation pipeline
        Func<MusicGroup, MusicGenre> extractGenre = g => g.Genre;
        Func<MusicGenre, string[]> getRelatedGenres = genre => genre switch
        {
            MusicGenre.Rock => new[] { "Alternative Rock", "Classic Rock", "Hard Rock" },
            MusicGenre.Jazz => new[] { "Smooth Jazz", "Fusion", "Bebop" },
            MusicGenre.Blues => new[] { "Delta Blues", "Chicago Blues", "Electric Blues" },
            MusicGenre.Metal => new[] { "Heavy Metal", "Progressive Metal", "Death Metal" },
            _ => new[] { "Unknown" }
        };
        Func<string[], string> formatRecommendations = genres => $"Recommended subgenres: {string.Join(", ", genres)}";
        
        var recommendationPipeline = extractGenre
            .Compose(getRelatedGenres)
            .Compose(formatRecommendations);
        
        var recommendations = recommendationPipeline(musicGroup);
        Console.WriteLine($"For {musicGroup.Name} ({musicGroup.Genre}): {recommendations}");
        
        // Example 3: Composing with conditional logic
        Func<Employee, (WorkRole role, int tenure)> getEmployeeMetrics = 
            e => (e.Role, DateTime.Now.Year - e.HireDate.Year);
        
        Func<(WorkRole role, int tenure), string> determinePromotion = 
            data => data switch
            {
                { role: WorkRole.AnimalCare, tenure: >= 5 } => "Eligible for Senior Animal Care position",
                { role: WorkRole.AnimalCare, tenure: >= 3 } => "Eligible for Lead Animal Care position", 
                { role: WorkRole.Veterinarian, tenure: >= 8 } => "Eligible for Chief Veterinarian position",
                { role: WorkRole.ProgramCoordinator, tenure: >= 6 } => "Eligible for Senior Coordinator position",
                { role: WorkRole.Maintenance, tenure: >= 10 } => "Eligible for Maintenance Supervisor position",
                _ => "No promotion opportunities at this time"
            };
        
        var promotionAnalyzer = getEmployeeMetrics.Compose(determinePromotion);
        var promotionStatus = promotionAnalyzer(employee);
        Console.WriteLine($"Promotion Status for {employee.FirstName} {employee.LastName}: {promotionStatus}");
        
        // Example 4: Composition with transformation and aggregation
        Func<MusicGroup, Album[]> getTopAlbums = g => 
            g.Albums.OrderByDescending(a => a.CopiesSold).Take(3).ToArray();
        
        Func<Album[], string> summarizeTopAlbums = albums => 
            albums.Length == 0 ? "No albums available" :
            $"Top albums: {string.Join(", ", albums.Select(a => $"{a.Name} ({a.CopiesSold:N0} sold)"))}";
        
        var topAlbumSummarizer = getTopAlbums.Compose(summarizeTopAlbums);
        var topAlbumsSummary = topAlbumSummarizer(musicGroup);
        Console.WriteLine($"Best sellers for {musicGroup.Name}: {topAlbumsSummary}");
        
        Console.WriteLine();
    }
}