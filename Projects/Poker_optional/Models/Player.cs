using System.Collections.Immutable;
using Microsoft.Extensions.Logging;

namespace Playground.Projects.Poker.Models;

public record Player (string Name, PokerHand Hand)
{
	public override string ToString() => $"Player: {Name}, Hand: {Hand}";
}
