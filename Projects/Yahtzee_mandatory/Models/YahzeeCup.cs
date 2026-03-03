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
    
    // Returns the highest-scoring valid combination for the current dice
    public YahzeeCup GetYahtzeeCombination() =>
        dice.Count != 5
            ? new NoCombination() { dice = this.dice }
            : GetAllValidCombinations()
                .OrderByDescending(c => c.Score)
                .First();

    // Returns all combinations that are valid for the current dice roll
    public ImmutableList<YahzeeCup> GetAllValidCombinations()
    {
        bool isThreeOfAKind = dicePipGroups.Any(g => g.Count() >= 3);
        bool isFourOfAKind  = dicePipGroups.Any(g => g.Count() >= 4);
        bool isFullHouse    = dicePipGroups.Any(g => g.Count() == 3) && dicePipGroups.Any(g => g.Count() == 2);
        bool isSmallStraight = straightsCombinations
            .Where(kvp => kvp.Key.StartsWith("SmallStraight"))
            .Any(kvp => kvp.Value.All(pip => sortedDicePips.Contains(pip)));
        bool isLargeStraight = straightsCombinations
            .Where(kvp => kvp.Key.StartsWith("LargeStraight"))
            .Any(kvp => kvp.Value.All(pip => sortedDicePips.Contains(pip)));
        bool isYahtzee = dicePipGroups.Any(g => g.Count() == 5);

        var combos = new List<YahzeeCup>();
        if (isYahtzee)      combos.Add(new Yahtzee()       { dice = dice });
        if (isLargeStraight)combos.Add(new LargeStraight() { dice = dice });
        if (isSmallStraight)combos.Add(new SmallStraight() { dice = dice });
        if (isFullHouse)    combos.Add(new FullHouse()     { dice = dice });
        if (isFourOfAKind)  combos.Add(new FourOfAKind()   { dice = dice });
        if (isThreeOfAKind) combos.Add(new ThreeOfAKind()  { dice = dice });
        // Upper section and Chance are always valid (score naturally handles 0 when no matching dice)
        combos.Add(new Sixes()  { dice = dice });
        combos.Add(new Fives()  { dice = dice });
        combos.Add(new Fours()  { dice = dice });
        combos.Add(new Threes() { dice = dice });
        combos.Add(new Twos()   { dice = dice });
        combos.Add(new Ones()   { dice = dice });
        combos.Add(new Chance() { dice = dice });
        return combos.ToImmutableList();
    }
    
    // Returns the best scoring combo still available on the scorecard, or a sacrifice category
    public (string Category, int Score) GetBestAvailableCombo(ScoreCard scoreCard)
    {
        var best = GetAllValidCombinations()
            .Where(c => scoreCard.IsAvailable(c.GetType().Name))
            .OrderByDescending(c => c.Score)
            .FirstOrDefault();

        if (best != null)
            return (best.GetType().Name, best.Score);

        // All scoring combos used — sacrifice any remaining available category (score 0)
        var sacrifice = new[]
            { "Chance", "Ones", "Twos", "Threes", "Fours", "Fives", "Sixes",
                "ThreeOfAKind", "FourOfAKind", "FullHouse", "SmallStraight", "LargeStraight", "Yahtzee" }
            .FirstOrDefault(cat => scoreCard.IsAvailable(cat));

        return (sacrifice ?? "Chance", 0);
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