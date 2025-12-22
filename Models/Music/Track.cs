using System.Text.RegularExpressions;
using Seido.Utilities.SeedGenerator;

namespace Models.Music;

public record Track (
    Guid TrackId,
    string Name,
    int DurationSeconds,
    bool Seeded = false
) : ISeed<Track>
{
    public Track() : this(default, default, default) {}

    #region randomly seed this instance
    public virtual Track Seed(SeedGenerator seedGenerator)
    {
        var quote = seedGenerator.Quote.Quote;
        var firstSentence = Regex.Match(quote, @"^.*?\S[.!?,-:]").Value;

        var ret = new Track(
            Guid.NewGuid(),
            firstSentence,
            seedGenerator.Next(120, 420),
            true
        );
        return ret;
    }
    #endregion
}
