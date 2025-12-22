using System.Collections.Immutable;
using Seido.Utilities.SeedGenerator;

namespace Models.Music;

public record Album (
    Guid AlbumId,
    string Name,
    int ReleaseYear,
    long CopiesSold,
    ImmutableList<Track> Tracks,
    bool Seeded = false
) : ISeed<Album>
{
    public Album() : this(default, default, default, default, default) {}

    #region randomly seed this instance
    public virtual Album Seed(SeedGenerator seedGenerator)
    {
        //generate 5 to 15 tracks
        var tracks = seedGenerator.ItemsToList<Track>(seedGenerator.Next(5, 16));

        var ret = new Album(
            Guid.NewGuid(),
            seedGenerator.MusicAlbumName,
            seedGenerator.Next(1970, 2024),
            seedGenerator.Next(1_000, 1_000_000),
            tracks.ToImmutableList(),
            true
        );
        return ret;
    }
    #endregion
}


