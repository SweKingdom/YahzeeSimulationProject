using System.Collections.Immutable;
using Playground.Projects.Yahtzee.Extensions;
using Playground.Projects.Yahtzee.Models;
using PlayGround.Extensions;

namespace Playground.Projects.Yahtzee;

public static class YahzeeGame
{
    public static void RunSimulation()
    {
        Console.WriteLine("Testing the Cup of Dice.");

        var cupOfDice = new CupOfDice(10);
        Console.WriteLine($"Cup with 10 dice: {cupOfDice}\n");   

        cupOfDice = new CupOfDice(2);
        Console.WriteLine($"Cup with 2 dice: {cupOfDice}\n"); 
        
        var yahzeeCup = new YahzeeCup();
        Console.WriteLine($"Yahzee Cup with 5 dice: {yahzeeCup}\n");  
        
        Func<YahzeeCup, YahzeeCup> rollAndPickBest =
            new Func<YahzeeCup, YahzeeCup>(cup => cup.ShakeAndRoll())
                .Compose(cup => cup.GetYahtzeeCombination());
        
        Enumerable.Range(1, 10)
            .Aggregate(yahzeeCup, (currentCup, i) =>
                currentCup
                    .Map(rollAndPickBest)
                    .Tap(combo => Console.WriteLine($"Yahzee Combination: {combo.GetType().Name}, Score: {combo.Score}\n"))
            ); 

        ImmutableList<Player> players = ImmutableList.Create(
            new Player("Alice", new YahzeeCup()),   
            new Player("Bob", new YahzeeCup()),
            new Player("Diana", new YahzeeCup()))

            .Tap(p => Console.WriteLine(string.Join("\n", p.Select(pl => $"{pl.Name} has Yahtzee cup: {pl.YahzeeCup}"))));


        System.Console.WriteLine("\nYahtzee Round Simulation:");
        System.Console.WriteLine("Your code should implement the Yahtzee round simulation below.");
        System.Console.WriteLine("========================");
        
        Enumerable.Range(1, 13)
            .Aggregate(
                new GameState(
                    players.Select(p => new PlayerState(p, new ScoreCard())).ToImmutableList(),
                    1),
                (state, round) =>
                    state
                        .Tap(_ => Console.WriteLine($"\n--- Round {round} of 13 ---"))
                        .Map(PlayRound) // every player rolls and scores
                        .Tap(s => PrintRoundResults(s, round))
                        .Map(s => s with { CurrentRound = round + 1 })
            )
            .Tap(PrintFinalResults);

    }
    
    private static GameState PlayRound(GameState state) =>
        state with
        {
            PlayerStates = state.PlayerStates
                .Select(ProcessPlayerTurn)
                .ToImmutableList()
        };
    
    private static PlayerState ProcessPlayerTurn(PlayerState ps) =>
        ps.Player.YahzeeCup
            .ShakeAndRoll()
            .Fork(
                cup  => cup,
                cup  => cup.GetBestAvailableCombo(ps.ScoreCard),
                (cup, combo) => BuildPlayerState(ps, cup, combo)
            );
    
    private static PlayerState BuildPlayerState(
        PlayerState ps, YahzeeCup cup, (string Category, int Score) combo)
    {
        bool rolledYahtzee    = cup.dice.GroupBy(d => d.Pip).Any(g => g.Count() == 5);
        bool yahtzeeAlreadyWon = ps.ScoreCard.Scores.TryGetValue("Yahtzee", out var ys) && ys == 50;

        var scoreCard = ps.ScoreCard.AddScore(combo.Category, combo.Score);
        if (rolledYahtzee && yahtzeeAlreadyWon)
            scoreCard = scoreCard.AddYahtzeeBonus();
        
        return new PlayerState(ps.Player with { YahzeeCup = cup }, scoreCard, combo.Category, combo.Score);
    }
    
    private static void PrintRoundResults(GameState state, int round)
    {
        state.PlayerStates
            .Tap(ps => ps.ToList().ForEach(p =>
                Console.WriteLine(
                    $"  {p.Player.Name,-10}: [{p.Player.YahzeeCup}]  ->  {p.LastCategory} ({p.LastScore} pts)")));

        var maxScore = state.PlayerStates.Max(p => p.LastScore);

        var winners = state.PlayerStates.Where(p => p.LastScore == maxScore).ToList();
        var msg = winners.Count == 1
            ? $"  >> Round {round} winner: {winners[0].Player.Name} with {winners[0].LastScore} pts!"
            : $"  >> Round {round}: Tie! ({winners[0].LastScore} pts each)";
        Console.WriteLine(msg);
    }
    
    private static readonly string[] CategoryOrder =
    {
        "Ones", "Twos", "Threes", "Fours", "Fives", "Sixes",
        "ThreeOfAKind", "FourOfAKind", "FullHouse",
        "SmallStraight", "LargeStraight", "Yahtzee", "Chance"
    };
    
    private static void PrintFinalResults(GameState finalState)
    {
        Console.WriteLine("\n" + new string('=', 55));
        Console.WriteLine("FINAL SCORES");
        Console.WriteLine(new string('=', 55));

        finalState.PlayerStates
            .OrderByDescending(ps => ps.ScoreCard.Total)
            .Tap(ranked => ranked.ToList().ForEach(ps =>
            {
                Console.WriteLine($"\n{ps.Player.Name}:");
                ps.ScoreCard.Scores
                    .OrderBy(kvp => Array.IndexOf(CategoryOrder, kvp.Key))
                    .ToList()
                    .ForEach(kvp => Console.WriteLine($"  {kvp.Key,-20}: {kvp.Value,3} pts"));
                Console.WriteLine($"  {"Upper Section",-20}: {ps.ScoreCard.UpperSectionTotal,3} pts");
                Console.WriteLine($"  {"Upper Bonus",-20}: {ps.ScoreCard.UpperBonus,3} pts");
                if (ps.ScoreCard.YahtzeeBonusCount > 0)
                    Console.WriteLine($"  {"Yahtzee Bonus x" + ps.ScoreCard.YahtzeeBonusCount,-20}: {ps.ScoreCard.YahtzeeBonusTotal,3} pts");
                Console.WriteLine($"  {"TOTAL",-20}: {ps.ScoreCard.Total,3} pts");
            }));

        finalState.PlayerStates
            .MaxBy(ps => ps.ScoreCard.Total)
            .Tap(winner =>
            {
                Console.WriteLine("\n" + new string('=', 55));
                Console.WriteLine($"WINNER: {winner.Player.Name} with {winner.ScoreCard.Total} points!");
            });
    }
}