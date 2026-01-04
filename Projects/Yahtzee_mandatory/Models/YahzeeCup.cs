using System.Collections.Immutable;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography;

namespace Playground.Projects.Yahtzee.Models;

public record YahzeeCup : CupOfDice
{
    IEnumerable<IGrouping<DiePip, Die>> dicePipGroups =>  this.dice.GroupBy(d => d.Pip);
    IOrderedEnumerable<DiePip> sortedDicePips => this.dice.Select(d => d.Pip).OrderBy(v => v);
    
    Dictionary<string, List<DiePip>> straightsCombinations { get 
    {
        return new Dictionary<string, List<DiePip>>
        {
            {"SmallStraight1", new List<DiePip> {DiePip.One, DiePip.Two, DiePip.Three, DiePip.Four}},
            {"SmallStraight2", new List<DiePip> {DiePip.Two, DiePip.Three, DiePip.Four, DiePip.Five}},
            {"SmallStraight3", new List<DiePip> {DiePip.Three, DiePip.Four, DiePip.Five, DiePip.Six}},
            {"LargeStraight1", new List<DiePip> {DiePip.One, DiePip.Two, DiePip.Three, DiePip.Four, DiePip.Five}},
            {"LargeStraight2", new List<DiePip> {DiePip.Two, DiePip.Three, DiePip.Four, DiePip.Five, DiePip.Six}}
        };
    }}

    public override string ToString() =>base.ToString();
    
    public int Score =>this switch
        {
            Ones => this.dice.Where(d => d.Pip == DiePip.One).Sum(d => (int)d.Pip),
            Twos => this.dice.Where(d => d.Pip == DiePip.Two).Sum(d => (int)d.Pip),
            Threes => this.dice.Where(d => d.Pip == DiePip.Three).Sum(d => (int)d.Pip),
            Fours => this.dice.Where(d => d.Pip == DiePip.Four).Sum(d => (int)d.Pip),
            Fives => this.dice.Where(d => d.Pip == DiePip.Five).Sum(d => (int)d.Pip),
            Sixes => this.dice.Where(d => d.Pip == DiePip.Six).Sum(d => (int)d.Pip),
            ThreeOfAKind => this.dice.Sum(d => (int)d.Pip),
            FourOfAKind => this.dice.Sum(d => (int)d.Pip),
            FullHouse => 25,
            SmallStraight => 30,
            LargeStraight => 40,
            Yahtzee => 50,
            Chance => this.dice.Sum(d => (int)d.Pip),
            _ => 0
        };
    public YahzeeCup () : base(5)
    {}

    public YahzeeCup GetYahtzeeCombination()
    {
        if (dice.Count() != 5)
            return new NoCombination() {dice = this.dice};

        bool isOnes = sortedDicePips.Any(pip => pip == DiePip.One);
        bool isTwos = sortedDicePips.Any(pip => pip == DiePip.Two);
        bool isThrees = sortedDicePips.Any(pip => pip == DiePip.Three);
        bool isFours = sortedDicePips.Any(pip => pip == DiePip.Four);
        bool isFives = sortedDicePips.Any(pip => pip == DiePip.Five);
        bool isSixes = sortedDicePips.Any(pip => pip == DiePip.Six);


        bool isThreeOfAKind = dicePipGroups.Any(g => g.Count() >= 3);
        bool isFourOfAKind = dicePipGroups.Any(group => group.Count() == 4);
        bool isFullHouse = dicePipGroups.Any(group => group.Count() == 3) && dicePipGroups.Any(group => group.Count() == 2);
       
        bool isSmallStraight =  straightsCombinations
            .Where(kvp => kvp.Key.StartsWith("SmallStraight"))
            .Any(kvp => kvp.Value.All(pip => sortedDicePips.Contains(pip)));
       
        bool isLargeStraight =  straightsCombinations
            .Where(kvp => kvp.Key.StartsWith("LargeStraight"))
            .Any(kvp => kvp.Value.All(pip => sortedDicePips.Contains(pip)));

        bool isYahtzee = dicePipGroups.Any(g => g.Count() == 5);

        return (isYahtzee, isLargeStraight, isSmallStraight, isFullHouse, isFourOfAKind, isThreeOfAKind,
             isSixes, isFives, isFours, isThrees, isTwos, isOnes) switch
        {
            (true, _, _, _, _, _, _, _, _, _, _, _) => new Yahtzee() {dice = this.dice},
            (_, true, _, _, _, _, _, _, _, _, _, _) => new LargeStraight () {dice = this.dice},
            (_, _, true, _, _, _, _, _, _, _, _, _) => new SmallStraight() {dice = this.dice},
            (_, _, _, true, _, _, _, _, _, _, _, _) => new FullHouse() {dice = this.dice},
            (_, _, _, _, true, _, _, _, _, _, _, _) => new FourOfAKind() {dice = this.dice},
            (_, _, _, _, _, true, _, _, _, _, _, _) => new ThreeOfAKind() {dice = this.dice},
            (_, _, _, _, _, _, true, _, _, _, _, _) => new Sixes() {dice = this.dice},
            (_, _, _, _, _, _, _, true, _, _, _, _) => new Fives() {dice = this.dice},
            (_, _, _, _, _, _, _, _, true, _, _, _) => new Fours() {dice = this.dice},
            (_, _, _, _, _, _, _, _, _, true, _, _) => new Threes() {dice = this.dice},
            (_, _, _, _, _, _, _, _, _, _, true, _) => new Twos() {dice = this.dice},
            (_, _, _, _, _, _, _, _, _, _, _, true) => new Ones() {dice = this.dice},
            _ => new Chance() {dice = this.dice}
        };  
    }
}

//Disciminators for yahtzee combinations
public record Yahtzee : YahzeeCup
{
}
public record LargeStraight : YahzeeCup
{
}
public record SmallStraight : YahzeeCup
{
}
public record FullHouse : YahzeeCup
{
}
public record FourOfAKind : YahzeeCup
{
}
public record ThreeOfAKind : YahzeeCup
{
}
public record Sixes : YahzeeCup
{
}
public record Fives : YahzeeCup
{
}
public record Fours : YahzeeCup
{
}
public record Threes : YahzeeCup
{
}
public record Twos : YahzeeCup
{
}
public record Ones : YahzeeCup
{
}
public record Chance : YahzeeCup
{
}
public record NoCombination : YahzeeCup
{
}