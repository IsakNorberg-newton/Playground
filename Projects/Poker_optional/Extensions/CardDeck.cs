using System.Collections.Immutable;
using Playground.Projects.Poker.Models;
using PlayGround.Extensions;

namespace Playground.Projects.Poker.Extensions;

public static class CardDeckExtensions
{
    public static CardDeck Shuffle(this CardDeck deck)
    {
        if (deck.cards.Count <= 0) return deck;

        var rnd = new Random();
        var shuffledDeck = deck.cards.OrderBy(c => rnd.Next()).ToImmutableList();
        return new CardDeck(shuffledDeck);
    }
    
    public static CardDeck Sort(this CardDeck deck, Func<IEnumerable<Card>, IOrderedEnumerable<Card>> sortFunc)
    {
        if (deck.cards.Count <= 0) return deck;
        return deck.Map( deck => deck with { cards = sortFunc(deck.cards).ToImmutableList() });  
    }
  
    public static CardDeck Keep(this CardDeck deck, Func<Card, bool> predicate)
    {
        if (deck.cards.Count <= 0) return deck;
        return deck.Map( deck => deck with { cards = deck.cards.Where(predicate).ToImmutableList() });  
    }

    public static CardDeck Remove(this CardDeck deck, Func<Card, bool> predicate)
    {
        if (deck.cards.Count <= 0) return deck;
        return deck.Map( deck => deck with { cards = deck.cards.Where(c => !predicate(c)).ToImmutableList() });  
    }

    public static CardDeck Draw(this CardDeck deck, out Card drawnCard) 
    {
        drawnCard = deck.cards.Last();
        return deck.Map( deck => deck with { cards = deck.cards.RemoveAt(deck.cards.Count - 1) });  
    }
    public static CardDeck AddToBottom(this CardDeck deck, Card card) 
    {
        var newDeck = deck.cards.Insert(0, card);
        return deck.Map( deck => deck with { cards = newDeck });  
    }
    public static CardDeck AddToBottom(this CardDeck deck, IEnumerable<Card> cards) 
    {
        var newDeck = deck.cards.InsertRange(0, cards);
        return deck.Map( deck => deck with { cards = newDeck });  
    }
    public static CardDeck AddToTop(this CardDeck deck, Card card) 
    {
        var newDeck = deck.cards.Add(card);
        return deck.Map( deck => deck with { cards = newDeck });  
    }
    public static CardDeck AddToTop(this CardDeck deck, IEnumerable<Card> cards) 
    {
        var newDeck = deck.cards.AddRange(cards);
        return deck.Map( deck => deck with { cards = newDeck });  
    }

    public static CardDeck AddDeck(this CardDeck deck, CardDeck otherDeck) 
    {
        var combinedCards = deck.cards.AddRange(otherDeck.cards);
        return deck.Map( deck => deck with { cards = combinedCards });  
    }

    public static CardDeck RemoveDuplicates(this CardDeck deck) 
    {
        var uniqueCards = deck.cards.Distinct().ToImmutableList();
        return deck.Map( deck => deck with { cards = uniqueCards });  
    }
}
