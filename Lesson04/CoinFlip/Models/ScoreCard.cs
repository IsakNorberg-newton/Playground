using System.Collections.Immutable;

namespace PlayGround.Lesson04.CoinFlip.Models;

public record ScoreCard(ImmutableDictionary<Player, int> Scores)
{    
    public ScoreCard() : this(ImmutableDictionary<Player, int>.Empty){}       
    public override string ToString()
    {
        var sRet = "Flip Results:";
        foreach (var kvp in Scores)
        {
            sRet += $"\n  {kvp.Key}: {kvp.Value}";
        }
        return sRet;
    }
}

public static class ScoreCardExtensions
{
    public static ScoreCard AddScore(this ScoreCard scoreCard, Player player, int score)
    {
        if (scoreCard.Scores.ContainsKey(player))
        {
            return scoreCard with { Scores = scoreCard.Scores.SetItem(player, scoreCard.Scores[player] + score) };
        }

        return scoreCard with { Scores = scoreCard.Scores.SetItem(player, score)};
    }
}
