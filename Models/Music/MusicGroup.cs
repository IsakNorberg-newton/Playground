using System.Collections.Immutable;
using Seido.Utilities.SeedGenerator;

namespace Models.Music;

public enum MusicGenre {Rock, Blues, Jazz, Metal}
public record MusicGroup (
    Guid MusicGroupId,
    string Name,
    int EstablishedYear,
    MusicGenre Genre,
    ImmutableList<Album> Albums,
    ImmutableList<Artist> Artists,
    bool Seeded = false
) : ISeed<MusicGroup>
{
    public MusicGroup() : this(default, default, default, default, default, default) {}

    #region randomly seed this instance
    public virtual MusicGroup Seed(SeedGenerator seedGenerator)
    {
        //generate 1 to 10 albums
        var albums = seedGenerator.ItemsToList<Album>(seedGenerator.Next(1, 11));
        //generate 1 to 5 artists
        var artists = seedGenerator.ItemsToList<Artist>(seedGenerator.Next(1, 6));

        var ret = new MusicGroup(
            Guid.NewGuid(),
            seedGenerator.MusicGroupName,
            seedGenerator.Next(1970, 2024),
            seedGenerator.FromEnum<MusicGenre>(),
            albums.ToImmutableList(),
            artists.ToImmutableList(),
            true
        );
        return ret;
    }
    #endregion
}


