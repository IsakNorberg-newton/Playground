using Models.Music;
using PlayGround.Extensions;
using Seido.Utilities.SeedGenerator;

namespace Playground.Lesson02;

public static class Excericee0201
{
    public static void Entry(string[] args = null)
    {   
#region Generate Data
        System.Console.WriteLine("=== LINQ Music Groups Exercises ===\n");
        var seedGenerator = new SeedGenerator();
    
        //See how clean it becomes when I made the Serialization to Json
        var musicgroups = seedGenerator.ItemsToList<MusicGroup>(50);
        //musicgroups.SerializeJson("musicgroups.json");
        
        //var musicgroups = new List<MusicGroup>();
        //musicgroups = musicgroups.DeSerializeJson<MusicGroup>("musicgroups.json");



#endregion
#region Easy

        // EASY: Basic Filtering & Projection (Questions 1-3)

#endregion
#region Question 1
        // Q1: Find all Rock music groups
        // Use: Where, Select
        // Expected: Filter by Genre == MusicGenre.Rock, project to group names
        System.Console.WriteLine("Q1: All Rock music groups:");
        var rockGroups = musicgroups.Where(g => g.Genre == MusicGenre.Rock).Select(g => g.Name);
        foreach (var name in rockGroups) Console.WriteLine($"- {name}");
#endregion        

#region Question 2
        // Q2: Get the names of all albums released after 2010
        // Use: SelectMany, Where, Select
        // Expected: Flatten all albums, filter by year, project to album names
        System.Console.WriteLine("\nQ2: Albums released after 2010:");
        var recentAlbums = musicgroups.SelectMany(g => g.Albums)
                                      .Where(a => a.ReleaseYear > 2010)
                                      .Select(a => a.Name);
        foreach (var name in recentAlbums) Console.WriteLine($"- {name}");
#endregion

#region Question 3
        // Q3: Find the first 5 music groups ordered by establishment year
        // Use: OrderBy, Take
        // Expected: Sort by EstablishedYear ascending, take first 5
        System.Console.WriteLine("\nQ3: First 5 oldest music groups:");
        var oldestGroups = musicgroups.OrderBy(g => g.EstablishedYear).Take(5);
        foreach (var g in oldestGroups) Console.WriteLine($"- {g.Name} ({g.EstablishedYear})");
#endregion
#region Medium

        // MEDIUM: Aggregation & Grouping (Questions 4-6)

#endregion
#region Question 4        
        // Q4: Calculate total copies sold across all albums for each genre
        // Use: GroupBy, Sum, Select
        // Expected: Group by Genre, sum CopiesSold from all albums in each group
        System.Console.WriteLine("\nQ4: Total copies sold by genre:");
        var copiesByGenre = musicgroups.GroupBy(g => g.Genre)
                                        .Select(grp => new { 
                                            Genre = grp.Key, 
                                            TotalSold = grp.Sum(g => g.Albums.Sum(a => a.CopiesSold)) 
                                        });
        foreach (var item in copiesByGenre) Console.WriteLine($"- {item.Genre}: {item.TotalSold:N0}");
#endregion

#region Question 5
        // Q5: Find all music groups that have more than 3 artists
        // Use: Where, Count
        // Expected: Filter groups where Artists.Count > 3
        System.Console.WriteLine("\nQ5: Groups with more than 3 artists:");
        var largeGroups = musicgroups.Where(g => g.Artists.Count > 3);
        foreach (var g in largeGroups) Console.WriteLine($"- {g.Name} ({g.Artists.Count} artists)");
#endregion

#region Question 6
        // Q6: Get the average number of tracks per album for each music group
        // Use: Select, SelectMany, Average
        // Expected: For each group, calculate average track count across their albums
        System.Console.WriteLine("\nQ6: Average tracks per album by group:");
        var avgTracks = musicgroups.Select(g => new { 
                                        g.Name, 
                                        AvgTracks = g.Albums.Any() ? g.Albums.Average(a => a.Tracks.Count) : 0 
                                    });
        foreach (var item in avgTracks) Console.WriteLine($"- {item.Name}: {item.AvgTracks:F1} tracks/album");
#endregion

#region Advanced

        // ADVANCED: Complex Queries & Multiple Operations (Questions 7-10)

#endregion
#region Question 7
        // Q7: Find artists who appear in Jazz groups with albums that sold over 500,000 copies
        // Use: Where, SelectMany, Distinct/DistinctBy
        // Expected: Filter Jazz groups, get albums with high sales, flatten artists, remove duplicates
        System.Console.WriteLine("\nQ7: Artists in successful Jazz groups:");
        var successfulJazzArtists = musicgroups
                                    .Where(g => g.Genre == MusicGenre.Jazz && g.Albums.Any(a => a.CopiesSold > 500_000))
                                    .SelectMany(g => g.Artists)
                                    .DistinctBy(art => art.ArtistId);
        foreach (var art in successfulJazzArtists) Console.WriteLine($"- {art.FirstName} {art.LastName}");
#endregion
        
#region Question 8
        // Q8: For each genre, find the music group with the most albums and show album count
        // Use: GroupBy, Select, OrderByDescending, First
        // Expected: Group by genre, for each genre find group with max album count
        System.Console.WriteLine("\nQ8: Most prolific group per genre:");
        var mostProlificByGenre = musicgroups.GroupBy(g => g.Genre)
                                             .Select(grp => grp.OrderByDescending(g => g.Albums.Count).First()); // I would use FirstOrDefault() for safety if I had my way.
        foreach (var g in mostProlificByGenre) Console.WriteLine($"- {g.Genre}: {g.Name} ({g.Albums.Count} albums)");
#endregion
#region Question 8 Alternative with extra safety
        // Q8: For each genre, find the music group with the most albums and show album count
        // Use: GroupBy, Select, OrderByDescending, FirstOrDefault
        // Expected: Group by genre, for each genre find group with max album count
        System.Console.WriteLine("\nQ8 (Alternative with extra safety): Most prolific group per genre: ");
        var mostProlificByGenreAlt = musicgroups.GroupBy(g => g.Genre)
                                                .Select(grp => grp.OrderByDescending(g => g.Albums.Count).FirstOrDefault());
        foreach (var g in mostProlificByGenreAlt) Console.WriteLine($"- {g.Genre}: {g.Name} ({g.Albums.Count} albums)");
#endregion

#region Question 9
        // Q9: Find the top 10 longest tracks across all music groups with their group and album info
        // Use: SelectMany (nested), OrderByDescending, Take, Select
        // Expected: Flatten to tracks with context, sort by duration, take 10
        System.Console.WriteLine("\nQ9: Top 10 longest tracks:");
        var longestTracks = musicgroups.SelectMany(g => g.Albums.SelectMany(a => a.Tracks.Select(t => new { 
                                            GroupName = g.Name, 
                                            AlbumName = a.Name, 
                                            Track = t 
                                        })))
                                       .OrderByDescending(x => x.Track.DurationSeconds)
                                       .Take(10);
        foreach (var x in longestTracks) Console.WriteLine($"- {x.Track.Name} ({x.Track.DurationSeconds}s) - {x.AlbumName} by {x.GroupName}");
#endregion

#region Question 10
        // Q10: Calculate percentage of albums per decade (1970s, 1980s, etc.) for Metal groups
        // Use: Where, SelectMany, GroupBy, Count, Select with calculations
        // Expected: Filter Metal groups, group albums by decade, calculate percentages
        System.Console.WriteLine("\nQ10: Album distribution by decade for Metal groups:");
        var metalAlbums = musicgroups.Where(g => g.Genre == MusicGenre.Metal).SelectMany(g => g.Albums).ToList();
        var totalMetalAlbums = metalAlbums.Count;
        if (totalMetalAlbums > 0)
        {
            var metalByDecade = metalAlbums.GroupBy(a => (a.ReleaseYear / 10) * 10)
                                           .Select(grp => new { 
                                               Decade = grp.Key, 
                                               Percentage = (double)grp.Count() / totalMetalAlbums * 100 
                                           })
                                           .OrderBy(x => x.Decade);
            foreach (var item in metalByDecade) Console.WriteLine($"- {item.Decade}s: {item.Percentage:F1}%");
        }
        else
        {
            Console.WriteLine("- No metal albums found.");
        }
#endregion

#region Question 11
        // Q11: Find music groups where all albums have at least one track longer than 5 minutes
        // Use: All, Any, SelectMany
        System.Console.WriteLine("\nQ11: Groups where all albums have at least one track > 5 minutes:");
        var longTrackGroups = musicgroups.Where(g => g.Albums.All(a => a.Tracks.Any(t => t.DurationSeconds > 300)));
        foreach (var g in longTrackGroups) Console.WriteLine($"- {g.Name}");
#endregion

#region Question 12
        // Q12: Create a lookup of artists by their birth decade (only for artists with known birthdays)
        // Use: Where, ToLookup, GroupBy
        System.Console.WriteLine("\nQ12: Artists by birth decade (Lookup):");
        var artistsByDecade = musicgroups.SelectMany(g => g.Artists)
                                         .Where(a => a.BirthDay.HasValue)
                                         .ToLookup(a => (a.BirthDay.Value.Year / 10) * 10);
        foreach (var decade in artistsByDecade.OrderBy(d => d.Key))
        {
            Console.WriteLine($"{decade.Key}s: {decade.Count()} artists");
        }
#endregion

#region Question 13
        // Q13: Find pairs of music groups established in the same year
        // Use: Join or GroupBy, SelectMany
        System.Console.WriteLine("\nQ13: Groups established in the same year:");
        var groupsSameYear = musicgroups.GroupBy(g => g.EstablishedYear)
                                        .Where(grp => grp.Count() > 1)
                                        .OrderBy(grp => grp.Key);
        foreach (var grp in groupsSameYear)
        {
            Console.WriteLine($"- {grp.Key}: {string.Join(", ", grp.Take(3).Select(g => g.Name))}{(grp.Count() > 3 ? "..." : "")}");
        }
#endregion

#region Question 14
        // Q14: Calculate the "productivity score" for each group (total tracks * average copies sold / years active)
        // (total tracks * average copies sold / years active)
        // Use: Select, SelectMany, Sum, Average, complex calculations
        System.Console.WriteLine("\nQ14: Top 5 productivity scores (tracks * avg sales / years active):");
        var currentYear = DateTime.Now.Year; // Use a current year as a parameter instead of DateTime.Now.Year inside a real function
        var productivityScores = musicgroups.Select(g => new { 
                                                g.Name, 
                                                Score = (double)(g.Albums.Sum(a => a.Tracks.Count) * (g.Albums.Any() ? g.Albums.Average(a => a.CopiesSold) : 0)) 
                                                        / Math.Max(1, currentYear - g.EstablishedYear) 
                                            })
                                            .OrderByDescending(x => x.Score)
                                            .Take(5);
        foreach (var item in productivityScores) Console.WriteLine($"- {item.Name}: {item.Score:N0}");
#endregion

#region Question 15
        // Q15: Find the album with the most diverse track durations (highest standard deviation)
        // Use: SelectMany, Select, complex aggregation
        System.Console.WriteLine("\nQ15: Album with most diverse track durations (Highest SD):");
        var albumWithMostDiverseTracks = musicgroups.SelectMany(g => g.Albums.Select(a => new { GroupName = g.Name, Album = a }))
                                                    .Where(x => x.Album.Tracks.Count > 1)
                                                    .Select(x => {
                                                        var durations = x.Album.Tracks.Select(t => (double)t.DurationSeconds).ToList();
                                                        var avg = durations.Average();
                                                        var stdDev = Math.Sqrt(durations.Average(d => Math.Pow(d - avg, 2)));
                                                        return new { x.GroupName, x.Album.Name, StdDev = stdDev };
                                                    })
                                                    .OrderByDescending(x => x.StdDev)
                                                    .FirstOrDefault();
        if (albumWithMostDiverseTracks != null)
        {
            Console.WriteLine($"- {albumWithMostDiverseTracks.Name} by {albumWithMostDiverseTracks.GroupName} (SD: {albumWithMostDiverseTracks.StdDev:F2}s)");
        }
#endregion
    }
}