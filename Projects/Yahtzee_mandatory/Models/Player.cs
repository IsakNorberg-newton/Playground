using System.Collections.Immutable;
using Microsoft.Extensions.Logging;

namespace Playground.Projects.Yahtzee.Models;

public record Player (string Name, YahzeeCup YahzeeCup)
{
	public override string ToString() => $"Player: {Name}, Hand: {YahzeeCup}";
}
