using Playground.Projects.Poker;
using Playground.Projects.Yahtzee;

namespace Playground.Projects;
public static class YahtzeeProject
{
    public static void Entry(string[] args = null)
    {      
        Console.WriteLine("YAHTZEE SIMULATION (Advanced functional patterns)");
        YahzeeGame.RunSimulation();
    }
}

public static class PokerProject
{
    public static void Entry(string[] args = null)
    {
        // Poker Simulation
        Console.WriteLine("\nPOKER SIMULATION (Advanced functional patterns)");
        PokerGame.RunSimulation();
    }
}
