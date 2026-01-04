# Coin Flip Exercises - Exploring Models and Extensions

## Overview
These exercises will help you explore the Coin Flip models and extensions through hands-on practice. Each exercise should be typed into the `CoinFlipGame.RunSimulation()` method. Work through them progressively to build understanding of functional programming patterns.

## Getting Started
All exercises should be implemented inside the `RunSimulation()` method in the `CoinFlipGame` class. You can comment out previous exercises as you work through new ones, or keep building on them.

---

## Exercise 1: Basic Coin Operations
**Objective**: Explore the `Coin` record and its basic operations.

**What to implement**:
- Create a coin with heads facing up
- Flip the coin multiple times (3-4 times)
- Print the result of each flip
- Observe that the original coin remains unchanged (immutability)
- Notice how each flip creates a new coin instance

**Key concepts to observe**:
- Immutable record behavior
- How the `Flip()` extension method works
- State changes through new object creation

---

## Exercise 2: Coin Validation and Extension Methods
**Objective**: Practice using the `ValidateGuess` extension method.

**What to implement**:
- Create a test coin with a known state (heads or tails)
- Test the `ValidateGuess` method with both correct and incorrect guesses
- Flip the coin and test validation again
- Print results showing whether guesses were correct or wrong

**Key concepts to observe**:
- Extension methods in action
- Boolean return values from validation
- How validation works with different coin states

---

## Exercise 3: Working with Players
**Objective**: Explore the `Player` record and understand immutable updates.

**What to implement**:
- Create two players with different starting coin states
- Display both players and their coin states
- Update one player's coin by flipping it using the `with` expression
- Compare the original and updated player objects
- Observe that the original player remains unchanged

**Key concepts to observe**:
- Record types and their properties
- The `with` expression for immutable updates
- How immutability affects object references

---

## Exercise 4: Basic ScoreCard Operations
**Objective**: Learn how to use the `ScoreCard` and its extension methods.

**What to implement**:
- Create an empty ScoreCard and two players
- Add scores for both players using the `AddScore` extension method
- Add additional points to one player (to see score accumulation)
- Display the ScoreCard after each update
- Verify that the original ScoreCard remains unchanged

**Key concepts to observe**:
- Extension methods for domain operations
- Immutable collections (`ImmutableDictionary`)
- Score accumulation through functional updates

---

## Exercise 5: Using Tap Extension for Side Effects
**Objective**: Practice using the `Tap` extension for logging without affecting the main flow.

**What to implement**:
- Create an initial coin
- Chain multiple coin flips using method chaining
- Use `Tap` between each flip to log the current state
- Show that `Tap` doesn't change the data flow
- Store the final result and display it

**Key concepts to observe**:
- Method chaining with fluent interfaces
- Side effects vs main data flow
- How `Tap` enables logging without breaking the chain

---

## Exercise 6: Functional Iteration with Range and Aggregate
**Objective**: Use functional patterns to perform multiple operations.

**What to implement**:
- Use `Enumerable.Range` to create a sequence of numbers
- Use `Aggregate` to flip a coin multiple times functionally
- Use `Tap` within the aggregate to log each flip
- Create a separate example that generates multiple random flips
- Count heads and tails using LINQ operations

```csharp
// Use Range and Aggregate to flip coin 5 times functionally
var startingCoin = new Coin(CoinSide.Heads);
var result = Enumerable.Range(1, 5)
    .Aggregate(startingCoin, (currentCoin, flipNumber) => 
        currentCoin
            .Flip()
            .Tap(coin => Console.WriteLine($"Flip {flipNumber}: {coin.FaceUp}")));

Console.WriteLine($"Final coin state: {result}");
```

**Key concepts to observe**:
- Functional iteration vs imperative loops
- `Aggregate` for state transformation
- LINQ operations for data analysis
- Combining functional patterns

---

## Exercise 7: Multiple Players with Functional Patterns
**Objective**: Work with collections of players using LINQ and functional patterns.

**What to implement**:
- Create an `ImmutableList` of 4 players with different coin states
- Display all initial players
- Use `Select` to flip all players' coins functionally
- Convert back to `ImmutableList` to maintain immutability
- Use `GroupBy` to count how many players have heads vs tails
- Display the distribution statistics

**Key concepts to observe**:
- Working with immutable collections
- LINQ transformations on collections
- Functional data aggregation

---

## Exercise 8: Simple Guessing Game
**Objective**: Combine all concepts to create a basic guessing game.

**What to implement**:
- Create players and an empty ScoreCard
- Use `Enumerable.Range` to create multiple game rounds
- For each round, have each player make a random guess
- Flip their coins and validate the guesses
- Use `Aggregate` to update the ScoreCard functionally
- Display the final scores

**Key concepts to observe**:
- Combining multiple functional patterns
- Random number generation in functional context
- State management through aggregation

---

## Exercise 9: Advanced Functional Composition
**Objective**: Create more complex functional pipelines and compositions.

**What to implement**:
- Create a higher-order function for player turns
- Use tuples to return multiple values from functions
- Apply the function across multiple players and rounds
- Use functional composition to build complex game logic
- Display results using functional transformations

**Key concepts to observe**:
- Higher-order functions and function composition
- Tuple deconstruction and pattern matching
- Complex functional pipelines

---

## Exercise 10: Statistical Analysis
**Objective**: Use functional programming for data analysis and statistics.

**What to implement**:
- Generate 1000 coin flips for statistical analysis
- Use `GroupBy` to analyze flip distribution
- Calculate percentages and statistics functionally
- Implement streak detection using `Aggregate`
- Find longest streaks of heads and tails
- Display comprehensive statistical report

**Key concepts to observe**:
- Large-scale data processing with functional patterns
- Statistical calculations using LINQ
- Complex aggregations for pattern detection

---

## Bonus Challenge: Create Your Own Extension Methods

**Objective**: Practice creating extension methods for existing types.

**What to implement**:
- Create a `FlipMultiple` extension that flips a coin N times
- Create a `ToEmoji` extension that converts coin state to emoji
- Create a `FlipAndValidate` extension that combines flipping with guess validation
- Test your extensions with the existing models

**Implementation approach**:
- Create a new static class for your extensions
- Use proper extension method syntax with `this` parameter
- Apply functional patterns within your extensions
- Chain your extensions with existing ones

**Example usage concepts**:
- `coin.FlipMultiple(5).ToEmoji()`
- `coin.FlipAndValidate(CoinSide.Heads)`
- Integration with `Tap` and other monadic extensions

## Learning Objectives Summary

After completing these exercises, you should understand:

1. **Immutable Records**: How Coin and Player records work
2. **Extension Methods**: How to use and create extension methods
3. **Functional Composition**: Chaining operations with Tap and other extensions
4. **LINQ Operations**: Using Select, Aggregate, GroupBy for data manipulation
5. **Immutable Collections**: Working with ImmutableList and functional updates
6. **Side Effects**: Proper handling of side effects with Tap
7. **Random Operations**: Handling randomness in a functional way
8. **Statistical Analysis**: Using functional programming for data analysis

## Tips for Success

- **Work progressively**: Each exercise builds understanding for the next
- **Focus on patterns**: Notice how functional patterns eliminate imperative code
- **Experiment freely**: Modify parameters and approaches to deepen understanding
- **Observe immutability**: Pay attention to how objects never change, only get replaced
- **Think declaratively**: Express *what* you want, not *how* to get it
- **Use the answer file**: Reference `CoinFlipExerciseAnswers.cs` if you get stuck
- **Chain operations**: Practice method chaining and fluent interfaces
- **Combine patterns**: See how `Tap`, `Aggregate`, and LINQ work together

**Reference the complete implementations**:
- All exercises are fully implemented in `CoinFlipExerciseAnswers.cs`
- Use `CoinFlipExerciseAnswers.RunAllExercises()` to see all solutions
- Use `CoinFlipExerciseAnswers.RunExercise(n)` for individual exercises

Happy coding with functional patterns!