# Poker Round Simulation Project

## Overview
Implement a functional Poker round simulation that demonstrates functional programming patterns in C#. This project will test your understanding of monadic extensions, immutable data structures, and declarative programming approaches.

## Objectives
- Apply functional programming patterns to game logic
- Use existing monadic extensions effectively
- Minimize imperative code and maximize declarative patterns
- Work with immutable data structures
- Implement complex game state management functionally

## Project Requirements

### Core Implementation

#### 1. Round Management
- **One round should deal 5 cards to each player**
  - Use functional patterns to distribute cards from the deck
  - Maintain immutable state throughout the dealing process
  - Handle the case where there aren't enough cards remaining

#### 2. Game Flow
- **Continue rounds as long as there are enough cards in the deck**
  - Implement using recursive or LINQ-based iteration
  - Avoid imperative loops where possible
  - Use monadic extensions to chain operations

#### 3. Round Results
- **After each round, print each player's hand and the winner**
  - Display hands in a readable format
  - Determine and announce the round winner
  - Use the existing `GetPokerRank()` method for hand evaluation

#### 4. Score Tracking
- **Implement a ScoreCard to track each player's wins**
  - Create an immutable score tracking mechanism
  - Update scores functionally after each round
  - Maintain historical data if needed

#### 5. Final Results
- **At the end of the simulation, print final scores and overall winner**
  - Aggregate all round results
  - Determine the player with the most wins
  - Handle tie scenarios appropriately

#### 6. Advanced Hand Comparison
- **Implement detailed hand comparison for same-rank scenarios**
  - The existing `GetPokerRank()` method determines basic poker rankings but doesn't handle tie-breaking
  - Extend the `PokerHand` and poker rank records to compare hands of the same rank
  - Examples: If two players have One Pair, the player with the higher pair wins
  - If pairs are equal, compare the highest remaining cards
  - Handle all poker hand types with proper tie-breaking logic

### Technical Requirements

#### Use Existing Infrastructure
- Leverage the existing `CardDeck`, `Player`, and `PokerHand` classes
- Utilize monadic extensions from the `Extensions` folder:
  - `Tap` for side effects (logging/printing)
  - `Map` for transformations
  - `Fork` for parallel operations
  - `Alt` for alternative flows
  - `Compose` for function composition

#### Functional Patterns
- **Minimize imperative code**
  - Avoid mutable variables where possible
  - Use immutable collections (`ImmutableList`, etc.)
  - Prefer expressions over statements

- **Maximize declarative code using LINQ and extension methods**
  - Use LINQ for data transformations and queries
  - Chain operations using method chaining
  - Leverage existing extension methods

#### Example Code Structure
```csharp
public static void RunPokerSimulation()
{
    var initialDeck = CardDeck.Create().Shuffle();
    var players = ImmutableList.Create(
        new Player("Alice", new PokerHand()),
        new Player("Bob", new PokerHand()),
        new Player("Charlie", new PokerHand())
    );
    var scoreCard = new ScoreCard();

    // Your implementation here using functional patterns
    // Consider using recursive functions or LINQ aggregation
}
```

### Advanced Hand Comparison Details

Since detailed hand comparison is required, here are the specific tie-breaking rules to implement:

#### Comparison Logic by Hand Type
You must implement tie-breaking logic for all poker hand types. The general principle is to compare the most significant elements first, then move to less significant elements if needed. For example:
- Pairs/triplets/quadruplets are compared by their rank first
- Remaining cards (kickers) are compared in descending order
- Straights and straight flushes are compared by their highest card

**See the Appendix for detailed tie-breaking rules for each hand type.**

#### Implementation Approach
- Extend the `PokerHand` record with comparison methods
- Create a comprehensive ranking system that handles all scenarios
- Use functional composition to build complex comparison logic
- Consider implementing `IComparable<PokerHand>` for clean integration

#### Example Scenarios to Handle
- Two players with One Pair: `[K♠, K♣, 9♥, 5♦, 2♠]` vs `[Q♠, Q♣, A♥, 7♦, 3♠]`
- Two players with Flush: `[A♠, J♠, 9♠, 7♠, 5♠]` vs `[K♠, Q♠, 10♠, 8♠, 6♠]`

## Implementation Tips

### Getting Started
1. Study the existing code structure and monadic extensions
2. Plan your data flow using immutable transformations
3. Start with a simple implementation and add complexity incrementally
4. Test each component individually before integration

### Functional Programming Principles
- **Pure functions**: Functions should not have side effects (except for `Tap`)
- **Immutability**: Prefer creating new objects rather than modifying existing ones
- **Function composition**: Build complex operations from simple functions
- **Declarative style**: Express what you want, not how to get it

### Common Patterns to Use
```csharp
// Use Tap for side effects (logging)
.Tap(result => Console.WriteLine($"Round result: {result}"))

// Use Map for transformations
.Map(players => players.Select(DealCardsToPlayer))

// Use Fork for parallel operations
.Fork(
    deck => deck.DealTo(player1),
    deck => deck.DealTo(player2),
    (p1Result, p2Result) => CombineResults(p1Result, p2Result)
)

// Use LINQ for data operations
players.Where(p => p.Hand.Cards.Count == 5)
       .OrderByDescending(p => p.Hand.GetPokerRank())
       .First()
```

## Deliverables
- Complete implementation in the `PokerGame.RunSimulation()` method
- Working round simulation with all required features
- Clean, functional code that demonstrates mastery of the patterns
- Enhanced hand comparison system with proper tie-breaking logic

## Evaluation Criteria
- **Functionality**: Does the simulation work correctly?
- **Functional Style**: Effective use of functional programming patterns
- **Code Quality**: Clean, readable, well-structured code
- **Use of Extensions**: Proper application of existing monadic extensions
- **Immutability**: Consistent use of immutable data structures
- **Hand Comparison**: Comprehensive tie-breaking logic for all hand types

Good luck with your implementation!

---

## Appendix: Poker Hand Tie-Breaking Rules

When two or more players have the same poker hand rank, use these detailed tie-breaking rules to determine the winner:

### 1. High Card
- Compare the highest card in each hand
- If tied, compare the second highest, then third, etc.
- **Example**: `[A♠, J♣, 9♥, 7♦, 2♠]` beats `[K♠, Q♣, J♥, 8♦, 5♠]` (Ace high vs King high)

### 2. One Pair
- First, compare the rank of the pairs
- If pairs are equal, compare the remaining three cards in descending order
- **Example**: `[K♠, K♣, 9♥, 7♦, 2♠]` beats `[K♥, K♦, 9♠, 6♣, 3♥]` (same pair, but 7 > 6)

### 3. Two Pair
- First, compare the higher pair
- If higher pairs are equal, compare the lower pair
- If both pairs are equal, compare the remaining card (kicker)
- **Example**: `[A♠, A♣, 5♥, 5♦, 9♠]` beats `[K♠, K♣, Q♥, Q♦, A♥]` (Aces over Fives vs Kings over Queens)

### 4. Three of a Kind
- First, compare the rank of the triplets
- If triplets are equal, compare the remaining two cards in descending order
- **Example**: `[Q♠, Q♣, Q♥, 8♦, 7♠]` beats `[Q♦, Q♥, Q♠, 8♣, 6♥]` (same triplet, but 7 > 6)

### 5. Straight
- Compare the highest card in the straight
- **Special case**: A-2-3-4-5 (wheel straight) is the lowest straight, with 5 as the high card
- **Example**: `[10♠, J♣, Q♥, K♦, A♠]` beats `[9♠, 10♣, J♥, Q♦, K♠]`

### 6. Flush
- Compare all five cards in descending order
- **Example**: `[A♠, J♠, 9♠, 7♠, 5♠]` beats `[K♠, Q♠, J♠, 10♠, 9♠]` (Ace high vs King high)

### 7. Full House
- First, compare the rank of the triplets
- If triplets are equal, compare the rank of the pairs
- **Example**: `[8♠, 8♣, 8♥, K♦, K♠]` beats `[8♦, 8♥, 8♠, Q♣, Q♥]` (same triplet, but Kings over Queens)

### 8. Four of a Kind
- First, compare the rank of the quadruplets
- If quadruplets are equal, compare the remaining card (kicker)
- **Example**: `[A♠, A♣, A♥, A♦, 9♠]` beats `[K♠, K♣, K♥, K♦, A♥]` (Aces vs Kings)

### 9. Straight Flush
- Compare the highest card in the straight flush
- **Special case**: A-2-3-4-5 of the same suit is the lowest straight flush
- **Example**: `[J♠, Q♠, K♠, A♠, 10♠]` (royal flush) beats `[9♠, 10♠, J♠, Q♠, K♠]`

### 10. Royal Flush
- All royal flushes are equal (10-J-Q-K-A of the same suit)
- In the extremely rare case of multiple royal flushes, it's a tie

### Implementation Notes
- Card ranks in ascending order: 2, 3, 4, 5, 6, 7, 8, 9, 10, J, Q, K, A
- Suits are typically not used for comparison (all suits are equal in poker)
- When comparing multiple cards, always compare from highest to lowest
- If all comparable elements are equal, the hands tie (split pot)