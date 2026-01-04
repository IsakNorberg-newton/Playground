namespace PlayGround.Lesson04.CoinFlip.Models;

public record Player(string Name, Coin Coin)
{
    public override string ToString() => $"Player: {Name}, Coin: {Coin}";
}