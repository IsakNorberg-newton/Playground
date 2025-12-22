using Seido.Utilities.SeedGenerator;

namespace Models.Music;

public record Artist (
    Guid ArtistId,
    string FirstName,
    string LastName,
    DateTime? BirthDay,
    bool Seeded = false
) : ISeed<Artist>
{
    public Artist() : this(default, default, default, default) {}

    #region randomly seed this instance
    public virtual Artist Seed(SeedGenerator seedGenerator)
    {
        var ret = new Artist(
            Guid.NewGuid(),
            seedGenerator.FirstName,
            seedGenerator.LastName,
            seedGenerator.Bool ? seedGenerator.DateAndTime(1940, 1990) : null,
            true
        );
        return ret;
    }
    #endregion
}


