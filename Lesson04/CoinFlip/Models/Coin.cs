using System.Collections.Immutable;

namespace PlayGround.Lesson04.CoinFlip.Models;

public enum CoinSide { Heads = 0, Tails }

public record Coin(CoinSide FaceUp);

public static class CoinExtensions
{
    private static readonly Random _random = new Random();

    public static Coin Flip(this Coin coin)
    {
        var _random = new Random();
        var newSide = (CoinSide)_random.Next((int)CoinSide.Heads, (int)CoinSide.Tails + 1);
        return coin with { FaceUp = newSide };
    }

    public static bool ValidateGuess(this Coin coin, CoinSide PlayerGuess) => coin.FaceUp == PlayerGuess;
}
