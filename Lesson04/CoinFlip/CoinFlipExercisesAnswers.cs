using System.Collections.Immutable;
using PlayGround.Extensions;
using PlayGround.Lesson04.CoinFlip.Models;

namespace PlayGround.Lesson04.CoinFlip;

public static class CoinFlipExercisesAnswers
{
    public static void Exercise1_BasicCoinOperations()
    {
        Console.WriteLine("=== Exercise 1: Basic Coin Operations ===");

        // Create a coin with heads facing up
        var coin = new Coin(CoinSide.Heads);
        Console.WriteLine($"Initial coin: {coin}");

        // Flip the coin a few times
        var flippedCoin1 = coin.Flip();
        var flippedCoin2 = flippedCoin1.Flip();
        var flippedCoin3 = flippedCoin2.Flip();

        Console.WriteLine($"After flip 1: {flippedCoin1}");
        Console.WriteLine($"After flip 2: {flippedCoin2}");
        Console.WriteLine($"After flip 3: {flippedCoin3}");

        // Notice how each flip creates a new coin (immutability)
        Console.WriteLine($"Original coin unchanged: {coin}");
    }

    public static void Exercise2_CoinValidation()
    {
        Console.WriteLine("\n=== Exercise 2: Coin Validation ===");

        var testCoin = new Coin(CoinSide.Heads);
        Console.WriteLine($"Test coin shows: {testCoin.FaceUp}");

        // Test different guesses
        var headGuess = testCoin.ValidateGuess(CoinSide.Heads);
        var tailGuess = testCoin.ValidateGuess(CoinSide.Tails);

        Console.WriteLine($"Guessing Heads: {(headGuess ? "CORRECT!" : "WRONG!")}");
        Console.WriteLine($"Guessing Tails: {(tailGuess ? "CORRECT!" : "WRONG!")}");

        // Try with a flipped coin
        var flippedCoin = testCoin.Flip();
        Console.WriteLine($"\nFlipped coin shows: {flippedCoin.FaceUp}");
        var newGuess = flippedCoin.ValidateGuess(CoinSide.Heads);
        Console.WriteLine($"Guessing Heads on flipped coin: {(newGuess ? "CORRECT!" : "WRONG!")}");
    }

    public static void Exercise3_WorkingWithPlayers()
    {
        Console.WriteLine("\n=== Exercise 3: Working with Players ===");

        // Create players with different starting coin states
        var alice = new Player("Alice", new Coin(CoinSide.Heads));
        var bob = new Player("Bob", new Coin(CoinSide.Tails));

        Console.WriteLine($"Players created:");
        Console.WriteLine($"  {alice}");
        Console.WriteLine($"  {bob}");

        // Update player's coin (creates new player due to immutability)
        var aliceWithFlippedCoin = alice with { Coin = alice.Coin.Flip() };
        Console.WriteLine($"\nAlice after coin flip:");
        Console.WriteLine($"  Original Alice: {alice}");
        Console.WriteLine($"  Updated Alice: {aliceWithFlippedCoin}");
    }

    public static void Exercise4_ScoreCardOperations()
    {
        Console.WriteLine("\n=== Exercise 4: ScoreCard Operations ===");

        var scoreCard = new ScoreCard();
        var player1 = new Player("Player1", new Coin(CoinSide.Heads));
        var player2 = new Player("Player2", new Coin(CoinSide.Tails));

        Console.WriteLine($"Initial scorecard: {scoreCard}");

        // Add scores using extension method
        var updatedScoreCard1 = scoreCard.AddScore(player1, 3);
        var updatedScoreCard2 = updatedScoreCard1.AddScore(player2, 5);
        var updatedScoreCard3 = updatedScoreCard2.AddScore(player1, 2); // Add to existing score

        Console.WriteLine($"After adding scores:");
        Console.WriteLine(updatedScoreCard3);

        // Notice original scorecard is unchanged (immutability)
        Console.WriteLine($"Original scorecard unchanged: {scoreCard}");
    }

    public static void Exercise5_UsingTapExtension()
    {
        Console.WriteLine("\n=== Exercise 5: Using Tap Extension ===");

        var initialCoin = new Coin(CoinSide.Heads);

        // Chain flips with Tap for logging
        var finalCoin = initialCoin
            .Tap(coin => Console.WriteLine($"Starting with: {coin}"))
            .Flip()
            .Tap(coin => Console.WriteLine($"After first flip: {coin}"))
            .Flip()
            .Tap(coin => Console.WriteLine($"After second flip: {coin}"))
            .Flip()
            .Tap(coin => Console.WriteLine($"After third flip: {coin}"));

        Console.WriteLine($"Final result: {finalCoin}");
    }

    public static void Exercise6_FunctionalIteration()
    {
        Console.WriteLine("\n=== Exercise 6: Functional Iteration ===");

        var startingCoin = new Coin(CoinSide.Heads);

        // Use Range and Aggregate to flip coin 5 times functionally
        var result = Enumerable.Range(1, 5)
            .Aggregate(startingCoin, (currentCoin, flipNumber) => 
                currentCoin
                    .Flip()
                    .Tap(coin => Console.WriteLine($"Flip {flipNumber}: {coin.FaceUp}")));

        Console.WriteLine($"Final coin state: {result}");

        // Count heads and tails functionally
        var flipResults = Enumerable.Range(1, 10)
            .Select(i => new Coin(CoinSide.Heads).Flip().FaceUp)
            .ToList();

        var headsCount = flipResults.Count(side => side == CoinSide.Heads);
        var tailsCount = flipResults.Count(side => side == CoinSide.Tails);

        Console.WriteLine($"In 10 flips: {headsCount} Heads, {tailsCount} Tails");
    }

    public static void Exercise7_MultiplePlayers()
    {
        Console.WriteLine("\n=== Exercise 7: Multiple Players ===");

        var players = ImmutableList.Create(
            new Player("Alice", new Coin(CoinSide.Heads)),
            new Player("Bob", new Coin(CoinSide.Tails)),
            new Player("Charlie", new Coin(CoinSide.Heads)),
            new Player("Diana", new Coin(CoinSide.Tails))
        );

        Console.WriteLine("Initial players:");
        players.ForEach(player => Console.WriteLine($"  {player}"));

        // Have all players flip their coins functionally
        var playersAfterFlip = players
            .Select(player => player with { Coin = player.Coin.Flip() })
            .ToImmutableList();

        Console.WriteLine("\nAfter all players flip:");
        playersAfterFlip.ForEach(player => Console.WriteLine($"  {player}"));

        // Count coins by side functionally
        var coinCounts = playersAfterFlip
            .GroupBy(player => player.Coin.FaceUp)
            .ToDictionary(group => group.Key, group => group.Count());

        Console.WriteLine($"\nCoin distribution: Heads={coinCounts.GetValueOrDefault(CoinSide.Heads, 0)}, " +
                          $"Tails={coinCounts.GetValueOrDefault(CoinSide.Tails, 0)}");
    }

    public static void Exercise8_SimpleGuessingGame()
    {
        Console.WriteLine("\n=== Exercise 8: Simple Guessing Game ===");

        var gamePlayers = ImmutableList.Create(
            new Player("Alice", new Coin(CoinSide.Heads)),
            new Player("Bob", new Coin(CoinSide.Heads))
        );

        var gameScoreCard = new ScoreCard();
        var random = new Random();

        // Play 3 rounds functionally
        var finalScores = Enumerable.Range(1, 3)
            .Aggregate(gameScoreCard, (currentScores, round) =>
            {
                Console.WriteLine($"\n--- Round {round} ---");
                
                return gamePlayers.Aggregate(currentScores, (scores, player) =>
                {
                    // Player makes a random guess
                    var guess = random.Next(2) == 0 ? CoinSide.Heads : CoinSide.Tails;
                    
                    // Flip the coin
                    var flippedCoin = player.Coin.Flip();
                    
                    // Check if guess was correct
                    var isCorrect = flippedCoin.ValidateGuess(guess);
                    var points = isCorrect ? 1 : 0;
                    
                    Console.WriteLine($"{player.Name}: Guessed {guess}, Got {flippedCoin.FaceUp}, " +
                                    $"{(isCorrect ? "CORRECT" : "WRONG")} (+{points} point)");
                    
                    return scores.AddScore(player, points);
                });
            });

        Console.WriteLine("\nFinal Scores:");
        Console.WriteLine(finalScores);
    }

    public static void Exercise9_AdvancedFunctionalComposition()
    {
        Console.WriteLine("\n=== Exercise 9: Advanced Functional Composition ===");

        // Create a function to simulate a player's turn
        Func<Player, Random, (Player player, bool correct, int points)> PlayTurn = (player, rng) =>
        {
            var guess = rng.Next(2) == 0 ? CoinSide.Heads : CoinSide.Tails;
            var flippedCoin = player.Coin.Flip();
            var updatedPlayer = player with { Coin = flippedCoin };
            var isCorrect = flippedCoin.ValidateGuess(guess);
            var points = isCorrect ? 1 : 0;
            
            return (updatedPlayer, isCorrect, points);
        };

        var advancedPlayers = ImmutableList.Create(
            new Player("Expert", new Coin(CoinSide.Heads)),
            new Player("Novice", new Coin(CoinSide.Tails))
        );

        var gameRandom = new Random(42); // Fixed seed for reproducible results

        // Simulate multiple turns with functional composition
        var gameResults = advancedPlayers
            .Select(player => 
                Enumerable.Range(1, 5)
                    .Aggregate(
                        (player: player, totalPoints: 0, correctGuesses: 0),
                        (state, turn) =>
                        {
                            var (updatedPlayer, correct, points) = PlayTurn(state.player, gameRandom);
                            return (
                                player: updatedPlayer,
                                totalPoints: state.totalPoints + points,
                                correctGuesses: state.correctGuesses + (correct ? 1 : 0)
                            );
                        }))
            .ToList();

        // Display results functionally
        gameResults.ForEach(result => 
            Console.WriteLine($"{result.player.Name}: {result.correctGuesses}/5 correct " +
                             $"({result.correctGuesses / 5.0 * 100:F1}%)"));
    }

    public static void Exercise10_StatisticalAnalysis()
    {
        Console.WriteLine("\n=== Exercise 10: Statistical Analysis ===");

        // Generate a large number of coin flips for statistical analysis
        var statisticalFlips = Enumerable.Range(1, 1000)
            .Select(_ => new Coin(CoinSide.Heads).Flip().FaceUp)
            .ToList();

        // Analyze results functionally
        var stats = statisticalFlips
            .GroupBy(side => side)
            .ToDictionary(
                group => group.Key,
                group => new { 
                    Count = group.Count(), 
                    Percentage = group.Count() / 1000.0 * 100 
                });

        Console.WriteLine("Statistical Analysis of 1000 Coin Flips:");
        foreach (var stat in stats)
        {
            Console.WriteLine($"{stat.Key}: {stat.Value.Count} flips ({stat.Value.Percentage:F1}%)");
        }

        // Find longest streaks functionally
        var streaks = statisticalFlips
            .Aggregate(
                new List<(CoinSide side, int length)>(),
                (streakList, currentSide) =>
                {
                    if (streakList.Count == 0 || streakList.Last().side != currentSide)
                    {
                        streakList.Add((currentSide, 1));
                    }
                    else
                    {
                        var lastIndex = streakList.Count - 1;
                        streakList[lastIndex] = (currentSide, streakList[lastIndex].length + 1);
                    }
                    return streakList;
                });

        var longestHeadsStreak = streaks.Where(s => s.side == CoinSide.Heads).Max(s => s.length);
        var longestTailsStreak = streaks.Where(s => s.side == CoinSide.Tails).Max(s => s.length);

        Console.WriteLine($"Longest Heads streak: {longestHeadsStreak}");
        Console.WriteLine($"Longest Tails streak: {longestTailsStreak}");
    }

    /// <summary>
    /// Runs all exercises in sequence
    /// </summary>
    public static void RunAllExercises()
    {
        Console.WriteLine("========================================");
        Console.WriteLine("   COIN FLIP EXERCISES - FULL SUITE");
        Console.WriteLine("========================================");

        Exercise1_BasicCoinOperations();
        Exercise2_CoinValidation();
        Exercise3_WorkingWithPlayers();
        Exercise4_ScoreCardOperations();
        Exercise5_UsingTapExtension();
        Exercise6_FunctionalIteration();
        Exercise7_MultiplePlayers();
        Exercise8_SimpleGuessingGame();
        Exercise9_AdvancedFunctionalComposition();
        Exercise10_StatisticalAnalysis();

        Console.WriteLine("\n========================================");
        Console.WriteLine("   ALL EXERCISES COMPLETED!");
        Console.WriteLine("========================================");
    }

    /// <summary>
    /// Runs a specific exercise by number
    /// </summary>
    /// <param name="exerciseNumber">Exercise number (1-10)</param>
    public static void RunExercise(int exerciseNumber)
    {
        switch (exerciseNumber)
        {
            case 1:
                Exercise1_BasicCoinOperations();
                break;
            case 2:
                Exercise2_CoinValidation();
                break;
            case 3:
                Exercise3_WorkingWithPlayers();
                break;
            case 4:
                Exercise4_ScoreCardOperations();
                break;
            case 5:
                Exercise5_UsingTapExtension();
                break;
            case 6:
                Exercise6_FunctionalIteration();
                break;
            case 7:
                Exercise7_MultiplePlayers();
                break;
            case 8:
                Exercise8_SimpleGuessingGame();
                break;
            case 9:
                Exercise9_AdvancedFunctionalComposition();
                break;
            case 10:
                Exercise10_StatisticalAnalysis();
                break;
            default:
                Console.WriteLine($"Exercise {exerciseNumber} not found. Available exercises: 1-10");
                break;
        }
    }
}

/// <summary>
/// Custom extension methods for bonus challenge
/// </summary>
public static class CustomCoinExtensions
{
    /// <summary>
    /// Flips a coin multiple times
    /// </summary>
    /// <param name="coin">The coin to flip</param>
    /// <param name="times">Number of times to flip</param>
    /// <returns>The final coin state after all flips</returns>
    public static Coin FlipMultiple(this Coin coin, int times)
    {
        return Enumerable.Range(1, times)
            .Aggregate(coin, (current, _) => current.Flip());
    }
    
    /// <summary>
    /// Converts coin to emoji representation
    /// </summary>
    /// <param name="coin">The coin to convert</param>
    /// <returns>Emoji representation of the coin</returns>
    public static string ToEmoji(this Coin coin)
    {
        return coin.FaceUp == CoinSide.Heads ? "👑" : "🪙";
    }

    /// <summary>
    /// Flips coin and returns both the result and whether it matches a prediction
    /// </summary>
    /// <param name="coin">The coin to flip</param>
    /// <param name="prediction">The predicted outcome</param>
    /// <returns>Tuple containing the flipped coin and whether the prediction was correct</returns>
    public static (Coin flippedCoin, bool predictionCorrect) FlipAndValidate(this Coin coin, CoinSide prediction)
    {
        var flipped = coin.Flip();
        var correct = flipped.ValidateGuess(prediction);
        return (flipped, correct);
    }
}