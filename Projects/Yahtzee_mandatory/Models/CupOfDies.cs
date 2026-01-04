using System.Collections.Immutable;

namespace Playground.Projects.Yahtzee.Models;


public record CupOfDice (int NrOfDice, ImmutableList<Die> dice)
{
    public CupOfDice(int NrOfDice): this(NrOfDice, ImmutableList<Die>.Empty)
    {
        if (NrOfDice < 0)
            throw new ArgumentException("Number of dies must be non-negative");

        var _random = new Random();
        dice = Enumerable.Range(0, NrOfDice)
            .Select(_ => new Die((DiePip)_random.Next((int)DiePip.One, (int)DiePip.Six + 1)))
            .ToImmutableList();
    }

    public override string ToString()
    {
        (string sRet, int i) = ("", 0);
        foreach (var die in dice)
        {
            sRet += $"{die,-9}";
        }
        return sRet;
    }
}
