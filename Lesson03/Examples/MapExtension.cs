using Seido.Utilities.SeedGenerator;
using Models.Employees;
using Models.Music;
using PlayGround.Extensions;

namespace Playground.Lesson03;

public static class MapExtension
{
    public static void RunExamples()
    {
        Console.WriteLine("\n=== Map Extension Method Examples ===\n");
        
        EmployeeMapExamples();
        CreditCardMapExamples();
        MusicGroupMapExamples();
        AlbumMapExamples();
        ArtistMapExamples();
        TrackMapExamples();

        Console.WriteLine("\n=== End of Map Extension Method Examples ===\n");
    }

        static void EmployeeMapExamples()
    {
        Console.WriteLine("--- Employee Map Examples ---");
        
        var seeder = new SeedGenerator();
        var employee = new Employee().Seed(seeder);
        
        // Example 1: Map to extract full name
        var fullName = employee.Map(e => $"{e.FirstName} {e.LastName}");
        Console.WriteLine($"Full Name: {fullName}");
        
        // Example 2: Map to calculate years of service
        var yearsOfService = employee.Map(e => DateTime.Now.Year - e.HireDate.Year);
        Console.WriteLine($"Years of Service: {yearsOfService}");
        
        // Example 3: Map to anonymous type with computed properties
        var employeeSummary = employee.Map(e =>  
        (
            Name: $"{e.FirstName} {e.LastName}",
            Role: e.Role.ToString(),
            CardCount: e.CreditCards.Count,
            Tenure: $"{DateTime.Now.Year - e.HireDate.Year} years"
        ));
        Console.WriteLine($"Summary: {employeeSummary.Name}, {employeeSummary.Role}, {employeeSummary.CardCount} cards, {employeeSummary.Tenure}");
        
        // Example 4: Sequential transformations using Map with params
        var updatedEmployee = employee.Map(
            e => e with { FirstName = e.FirstName.ToUpper() },
            e => e with { LastName = e.LastName.ToUpper() },
            e => e with { Role = WorkRole.Management }
        );
        Console.WriteLine($"Updated Employee: {updatedEmployee.FirstName} {updatedEmployee.LastName}, {updatedEmployee.Role}");
        
        Console.WriteLine();
    }

    static void CreditCardMapExamples()
    {
        Console.WriteLine("--- CreditCard Map Examples ---");
        
        var seeder = new SeedGenerator();
        var card = new CreditCard().Seed(seeder);
        
        // Example 1: Map to masked number
        var maskedNumber = card.Map(c => $"****-****-****-{c.Number.Substring(c.Number.Length - 4)}");
        Console.WriteLine($"Masked Number: {maskedNumber}");
        
        // Example 2: Map to expiration date string
        var expirationDate = card.Map(c => $"{c.ExpirationMonth}/{c.ExpirationYear}");
        Console.WriteLine($"Expires: {expirationDate}");
        
        // Example 3: Map to check if expired
        var isExpired = card.Map(c => 
        {
            var expYear = int.Parse(c.ExpirationYear);
            var expMonth = int.Parse(c.ExpirationMonth);
            var expDate = new DateTime(2000 + expYear, expMonth, 1);
            return expDate < DateTime.Now;
        });
        Console.WriteLine($"Is Expired: {isExpired}");
        
        // Example 4: Sequential transformations to update card details
        var updatedCard = card.Map(
            c => c with { ExpirationYear = "30" },
            c => c with { ExpirationMonth = "12" },
            c => c with { Issuer = CardIssuer.Visa }
        );
        Console.WriteLine($"Updated Card: {updatedCard.Issuer}, Expires {updatedCard.ExpirationMonth}/{updatedCard.ExpirationYear}");
        
        Console.WriteLine();
    }

    static void MusicGroupMapExamples()
    {
        Console.WriteLine("--- MusicGroup Map Examples ---");
        
        var seeder = new SeedGenerator();
        var group = new MusicGroup().Seed(seeder);
        
        // Example 1: Map to group info string
        var groupInfo = group.Map(g => 
            $"{g.Name} ({g.Genre}) - Est. {g.EstablishedYear}");
        Console.WriteLine($"Group Info: {groupInfo}");
        
        // Example 2: Map to calculate age
        var yearsActive = group.Map(g => DateTime.Now.Year - g.EstablishedYear);
        Console.WriteLine($"Years Active: {yearsActive}");
        
        // Example 3: Map to statistics object
        var statistics = group.Map(g => new
        {
            GroupName = g.Name,
            TotalAlbums = g.Albums.Count,
            TotalArtists = g.Artists.Count,
            TotalTracks = g.Albums.Sum(a => a.Tracks.Count),
            AverageTracksPerAlbum = g.Albums.Any() ? g.Albums.Average(a => a.Tracks.Count) : 0
        });
        Console.WriteLine($"Stats: {statistics.GroupName} has {statistics.TotalAlbums} albums, {statistics.TotalArtists} artists, {statistics.TotalTracks} total tracks");
        
        // Example 4: Sequential transformations
        var updatedGroup = group.Map(
            g => g with { Genre = MusicGenre.Jazz },
            g => g with { Name = $"The {g.Name}" },
            g => g with { EstablishedYear = g.EstablishedYear + 5 }
        );
        Console.WriteLine($"Updated Group: {updatedGroup.Name}, {updatedGroup.Genre}, Est. {updatedGroup.EstablishedYear}");
        
        Console.WriteLine();
    }

    static void AlbumMapExamples()
    {
        Console.WriteLine("--- Album Map Examples ---");
        
        var seeder = new SeedGenerator();
        var album = new Album().Seed(seeder);
        
        // Example 1: Map to album title with year
        var titleWithYear = album.Map(a => $"{a.Name} ({a.ReleaseYear})");
        Console.WriteLine($"Album: {titleWithYear}");
        
        // Example 2: Map to calculate total duration
        var totalDuration = album.Map(a => 
        {
            var totalSeconds = a.Tracks.Sum(t => t.DurationSeconds);
            var minutes = totalSeconds / 60;
            var seconds = totalSeconds % 60;
            return $"{minutes}m {seconds}s";
        });
        Console.WriteLine($"Total Duration: {totalDuration}");
        
        // Example 3: Map to sales category
        var salesCategory = album.Map(a => a.CopiesSold switch
        {
            > 500_000 => "Platinum",
            > 100_000 => "Gold",
            > 50_000 => "Silver",
            _ => "Standard"
        });
        Console.WriteLine($"Sales Category: {salesCategory} ({album.CopiesSold:N0} copies)");
        
        // Example 4: Sequential transformations
        var updatedAlbum = album.Map(
            a => a with { Name = $"{a.Name} - Remastered" },
            a => a with { ReleaseYear = a.ReleaseYear + 10 },
            a => a with { CopiesSold = a.CopiesSold * 2 }
        );
        Console.WriteLine($"Updated Album: {updatedAlbum.Name}, {updatedAlbum.ReleaseYear}, {updatedAlbum.CopiesSold:N0} sold");
        
        Console.WriteLine();
    }

    static void ArtistMapExamples()
    {
        Console.WriteLine("--- Artist Map Examples ---");
        
        var seeder = new SeedGenerator();
        var artist = new Artist().Seed(seeder);
        
        // Example 1: Map to stage name
        var stageName = artist.Map(a => $"{a.FirstName} {a.LastName}");
        Console.WriteLine($"Stage Name: {stageName}");
        
        // Example 2: Map to calculate age (if birthday available)
        var ageInfo = artist.Map(a => 
            a.BirthDay.HasValue 
                ? $"{DateTime.Now.Year - a.BirthDay.Value.Year} years old"
                : "Age unknown");
        Console.WriteLine($"Age: {ageInfo}");
        
        // Example 3: Map to artist profile
        var profile = artist.Map(a => new
        {
            FullName = $"{a.FirstName} {a.LastName}",
            BirthYear = a.BirthDay?.Year.ToString() ?? "Unknown",
            HasBirthday = a.BirthDay.HasValue
        });
        Console.WriteLine($"Profile: {profile.FullName}, Born: {profile.BirthYear}");
        
        // Example 4: Sequential transformations
        var updatedArtist = artist.Map(
            a => a with { FirstName = a.FirstName.ToUpper() },
            a => a with { LastName = a.LastName.ToUpper() },
            a => a with { BirthDay = new DateTime(1980, 1, 1) }
        );
        Console.WriteLine($"Updated Artist: {updatedArtist.FirstName} {updatedArtist.LastName}, Born: {updatedArtist.BirthDay:yyyy-MM-dd}");
        
        Console.WriteLine();
    }

    static void TrackMapExamples()
    {
        Console.WriteLine("--- Track Map Examples ---");
        
        var seeder = new SeedGenerator();
        var track = new Track().Seed(seeder);
        
        // Example 1: Map to formatted duration
        var formattedDuration = track.Map(t => 
        {
            var minutes = t.DurationSeconds / 60;
            var seconds = t.DurationSeconds % 60;
            return $"{minutes}:{seconds:D2}";
        });
        Console.WriteLine($"Track: {track.Name} - Duration: {formattedDuration}");
        
        // Example 2: Map to track length category
        var lengthCategory = track.Map(t => t.DurationSeconds switch
        {
            < 180 => "Short",
            < 300 => "Medium",
            < 420 => "Long",
            _ => "Epic"
        });
        Console.WriteLine($"Length Category: {lengthCategory}");
        
        // Example 3: Map to display info
        var displayInfo = track.Map(t => new
        {
            Title = t.Name.Length > 30 ? $"{t.Name.Substring(0, 27)}..." : t.Name,
            Minutes = t.DurationSeconds / 60,
            Seconds = t.DurationSeconds % 60
        });
        Console.WriteLine($"Display: {displayInfo.Title} ({displayInfo.Minutes}:{displayInfo.Seconds:D2})");
        
        // Example 4: Sequential transformations
        var updatedTrack = track.Map(
            t => t with { Name = $"{t.Name} (Live)" },
            t => t with { DurationSeconds = t.DurationSeconds + 30 },
            t => t with { TrackId = Guid.NewGuid() }
        );
        Console.WriteLine($"Updated Track: {updatedTrack.Name}, Duration: {updatedTrack.DurationSeconds}s");
        
        Console.WriteLine();
    }
}