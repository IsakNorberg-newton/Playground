using System.Collections.Immutable;
using PlayGround.Extensions;

namespace Playground.Projects.Poker.Models;
public record CardDeck (ImmutableList<Card> cards)
{
    public CardDeck(): this(ImmutableList<Card>.Empty){}

    public override string ToString()
    {
        (string sRet, int i) = ("", 0);
        foreach (var card in cards)
        {
            sRet += $"{card,-9}";
            if ((i++ + 1) % 13 == 0)
                sRet += "\n";
        }
        return sRet;
    }
    public static CardDeck Create()   
    {
        var cards = new List<Card>();
        cards.Clear();
        for (CardSuit c = CardSuit.Clubs; c <= CardSuit.Spades; c++)
        {
            for (CardRank v = CardRank.Two; v <= CardRank.Ace; v++)
            {
                cards.Add(new Card (c, v));
            }
        }
        return new CardDeck(cards.ToImmutableList());
    }
}