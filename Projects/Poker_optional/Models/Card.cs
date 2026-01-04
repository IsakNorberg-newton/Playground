using Microsoft.Extensions.Logging;

namespace Playground.Projects.Poker.Models;

// Poker suit order, Spades highest
public enum CardSuit { Clubs = 0, Diamonds, Hearts, Spades}
// Poker Value order
public enum CardRank { Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Knight, Queen, King, Ace }


public record Card (CardSuit Suit, CardRank Rank)
{
	public override string ToString() => Suit switch
	{
		CardSuit.Clubs => $"{'\x2663'} {Rank}",
		CardSuit.Diamonds => $"{'\x2666'} {Rank}",
		CardSuit.Hearts => $"{'\x2665'} {Rank}",
		_ => $"{'\x2660'} {Rank}" // Spades
	};
}