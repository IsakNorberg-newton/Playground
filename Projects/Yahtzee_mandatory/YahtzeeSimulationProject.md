# Yahtzee Round Simulation Project

## Overview
Implement a functional Yahtzee round simulation that demonstrates functional programming patterns in C#. This project will test your understanding of monadic extensions, immutable data structures, and declarative programming approaches while building a complete dice game simulation.

## Objectives
- Apply functional programming patterns to game logic
- Use existing monadic extensions effectively
- Minimize imperative code and maximize declarative patterns
- Work with immutable data structures
- Implement complex game state management functionally
- Handle probabilistic game mechanics (dice rolling and decision making)

## Project Requirements

### G-Grade Level (Pass)

#### 1. Round Management
- **Each round should have each player roll their Yahtzee cup**
  - Use functional patterns to handle dice rolling
  - Maintain immutable state throughout the rolling process
  - Apply monadic extensions to chain operations

#### 2. Combination Detection
- **Modify the GetYahtzeeCombination() method to return the best possible combination (highest points)**
  - Analyze all possible scoring combinations for a roll
  - Select the combination that yields the maximum points
  - Handle edge cases where multiple combinations are possible

#### 3. Round Results
- **After each round, print each player's Yahtzee combination and the winner**
  - Display rolls and selected combinations in a readable format
  - Determine and announce the round winner based on points scored
  - Handle ties appropriately

#### 4. Game Structure
- **Implement multiple rounds (13 rounds for a full game)**
  - Each player should fill in their scorecard after each round
  - Ensure proper game flow and state management
  - Use functional iteration patterns instead of imperative loops

#### 5. Score Tracking
- **Implement a ScoreCard to track each player's progress**
  - Create scorecard with all combination types:
    - **Upper Section**: Ones, Twos, Threes, Fours, Fives, Sixes
    - **Lower Section**: Three of a Kind, Four of a Kind, Full House, Small Straight, Large Straight, Yahtzee, Chance

#### 6. Final Results
- **At the end of the simulation, print final scores and overall winner**
  - Calculate total scores including bonuses
  - Determine the player with the highest total score
  - Display comprehensive final scorecard for each player

### VG-Grade Level (Pass with Distinction)

#### 7. Advanced Rolling Mechanics
- **Extend simulation to allow up to 3 rolls per turn**
  - After first roll, randomly decide which dice to keep (1-5 dice)
  - Re-roll the remaining dice
  - Repeat for a potential third roll
  - Use functional patterns to model the decision-making process

#### 8. Strategic Decision Making
- **Modify GetYahtzeeCombination() to return a descending list of possible combinations based on scores**
  - Analyze all valid scoring combinations for a roll
  - Return combinations sorted by score (highest first)
  - Include all applicable combinations, not just the best one

#### 9. ScoreCard Box Selection
- **ScoreCard should fill the free box matching the alternative with highest score**
  - Check which boxes are still available on the scorecard
  - Select the highest-scoring combination that corresponds to an available box
  - Handle cases where the best combination's box is already filled
  - Each player should choose which box to fill based on their roll and best possible combination
  - Once a box is filled, it cannot be used again
  - Use functional approaches for decision algorithms

#### 10. Advanced Scoring
- **Implement Yahtzee Bonus system**
  - If Yahtzee box is filled with 50 points and player rolls additional Yahtzees
  - Award 100 points per extra Yahtzee (standard rules)
  - Handle joker rules where extra Yahtzees can be used in other categories
  - Maintain bonus tracking throughout the game

### Technical Requirements

#### Use Existing Infrastructure
- Leverage the existing `YahzeeCup`, `CupOfDice`, and `Player` classes
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
  - Use functional composition for complex logic

- **Maximize declarative code using LINQ and extension methods**
  - Use LINQ for data transformations and queries
  - Chain operations using method chaining
  - Leverage existing extension methods
  - Handle randomness functionally

#### Example Code Structure
```csharp
public static void RunYahtzeeSimulation()
{
    var players = ImmutableList.Create(
        new Player("Alice", new YahzeeCup()),
        new Player("Bob", new YahzeeCup()),
        new Player("Charlie", new YahzeeCup())
    );
    
    var initialGameState = new GameState(players, new ScoreCard());
    
    // Your implementation here using functional patterns
    // Consider using recursive functions or LINQ aggregation
    // Handle 13 rounds with immutable state updates
}
```

## Implementation Tips

### Getting Started
1. Study the existing code structure and monadic extensions
2. Plan your data flow using immutable transformations
3. Design your ScoreCard and GameState as immutable records
4. Start with basic rolling mechanics before adding strategy

### Functional Programming Principles
- **Pure functions**: Functions should not have side effects (except for `Tap`)
- **Immutability**: Prefer creating new objects rather than modifying existing ones
- **Function composition**: Build complex operations from simple functions
- **Declarative style**: Express what you want, not how to get it

### Yahtzee-Specific Patterns
```csharp
// Use Tap for game state logging
.Tap(gameState => Console.WriteLine($"Round {gameState.CurrentRound} results"))

// Use Map for state transformations
.Map(gameState => gameState.NextRound())

// Use Fork for parallel player actions
.Fork(
    gameState => ProcessPlayerRoll(gameState, player1),
    gameState => ProcessPlayerRoll(gameState, player2),
    (p1State, p2State) => CombineGameStates(p1State, p2State)
)

// Use LINQ for scoring calculations
var bestCombination = possibleCombinations
    .Where(combo => scoreCard.IsBoxAvailable(combo.Type))
    .OrderByDescending(combo => combo.Score)
    .FirstOrDefault();
```


## Deliverables

### G-Grade Level
- Complete implementation in the `YahzeeGame.RunSimulation()` method
- Working 13-round Yahtzee simulation with all basic features
- Proper scorecard implementation with all categories
- Clean, functional code demonstrating mastery of the patterns

- **Code submission via public GitHub repository**:
  - Provide a link to a public GitHub repository containing your complete solution
  - Send link to martin_lenart@icloud.com
  - Repository must be cloneable and contain all necessary files
  - Code must compile, build, and run successfully after cloning
  - README file should include clear instructions for running the simulation
  - Code must clearly demonstrate all G-grade requirements through execution 


### VG-Grade Level  
- All G-grade requirements plus:
- Advanced rolling mechanics with up to 3 rolls per turn
- Strategic dice selection algorithms
- Yahtzee bonus system implementation
- Enhanced game complexity while maintaining functional style
- **Enhanced GitHub repository requirements**:
  - All G-grade repository requirements must be met
  - Code must clearly demonstrate all VG-grade features through execution
  - Repository should include documentation explaining advanced features
  - Example runs should showcase the enhanced mechanics and scoring

## Evaluation Criteria

### G-Grade Level
- **Functionality**: Does the basic simulation work correctly?
- **Functional Style**: Effective use of functional programming patterns
- **Code Quality**: Clean, readable, well-structured code
- **Use of Extensions**: Proper application of existing monadic extensions
- **Immutability**: Consistent use of immutable data structures
- **Game Logic**: Correct implementation of simplfied Yahtzee rules and scoring as outlined

### VG-Grade Level
- All G-grade criteria plus:
- **Advanced Mechanics**: Proper implementation of multi-roll turns
- **Strategic Logic**: Intelligent dice selection algorithms
- **Bonus System**: Correct Yahtzee bonus implementation
- **Complexity Management**: Maintaining clean functional style with increased complexity
- **Game Logic**: Correct implementation of Yahtzee rules and scoring

Good luck with your implementation!

---

## Appendix: Yahtzee Scoring Rules

### Upper Section
- **Ones**: Sum of all dice showing 1
- **Twos**: Sum of all dice showing 2  
- **Threes**: Sum of all dice showing 3
- **Fours**: Sum of all dice showing 4
- **Fives**: Sum of all dice showing 5
- **Sixes**: Sum of all dice showing 6
- **Bonus**: 35 points if upper section total ≥ 63 points

### Lower Section
- **Three of a Kind**: At least 3 dice the same → Sum of all dice
- **Four of a Kind**: At least 4 dice the same → Sum of all dice
- **Full House**: 3 of one number, 2 of another → 25 points
- **Small Straight**: 4 consecutive numbers → 30 points
- **Large Straight**: 5 consecutive numbers → 40 points
- **Yahtzee**: All 5 dice the same → 50 points
- **Chance**: Any combination → Sum of all dice

### Special Rules
- **Yahtzee Bonus**: 100 points for each additional Yahtzee after the first

### Scoring Examples
- Roll: [3,3,3,2,1] → Best options: Three of a Kind (12 pts), Threes (9 pts), Chance (12 pts)
- Roll: [1,2,3,4,5] → Large Straight (40 pts)
- Roll: [6,6,6,6,6] → Yahtzee (50 pts)
- Roll: [4,4,4,2,2] → Full House (25 pts)