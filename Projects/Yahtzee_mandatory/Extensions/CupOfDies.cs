using System.Collections.Immutable;
using Playground.Projects.Yahtzee.Models;

namespace Playground.Projects.Yahtzee.Extensions;

public static class CupOfDiceExtensions
{
    public static CupOfDice ShakeAndRoll(this CupOfDice cup)
    {
        if (cup.dice.Count <= 0) return cup;

        var _random = new Random();
        var rolledDies = cup.dice
            .Select(_ => new Die((DiePip)_random.Next((int)DiePip.One, (int)DiePip.Six + 1)))
            .ToImmutableList();

        return cup with { dice = rolledDies };
    }

    public static YahzeeCup ShakeAndRoll(this YahzeeCup cup)
    {
        if (cup.dice.Count <= 0) return cup;

        var _random = new Random();
        var rolledDies = cup.dice
            .Select(_ => new Die((DiePip)_random.Next((int)DiePip.One, (int)DiePip.Six + 1)))
            .ToImmutableList();

        return cup with { dice = rolledDies };
    }
}
