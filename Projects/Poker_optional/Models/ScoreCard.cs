using System.Collections.Immutable;
using Playground.Projects.Poker.Extensions;

namespace Playground.Projects.Poker.Models;

public record ScoreCard(ImmutableDictionary<string, ImmutableList<PokerHand>> Scores)
{    
    public ScoreCard() : this(ImmutableDictionary<string, ImmutableList<PokerHand>>.Empty){}       
    public override string ToString()
    {
        var sRet = "Hand results each round:";
        foreach (var kvp in Scores)
        {
            sRet += $"\n  {kvp.Key}: {string.Join(", ", kvp.Value.Select(v => v.GetPokerRank().GetType().Name))}";
        }
        return sRet;
    }
}

public static class ScoreCardExtensions
{
    public static ScoreCard AddScore(this ScoreCard scoreCard, Player player, PokerHand score)
    {
        if (scoreCard.Scores.ContainsKey(player.Name))
        {
             return scoreCard with { Scores = scoreCard.Scores.SetItem(player.Name, scoreCard.Scores[player.Name].Add(score))};
        }
        else if (scoreCard.Scores.ContainsKey(player.Name))     
        {
            return scoreCard with { Scores = scoreCard.Scores.SetItem(player.Name, scoreCard.Scores[player.Name].Add(score))};
        }

        return scoreCard with { Scores = scoreCard.Scores.SetItem(player.Name, ImmutableList.Create(score))};
    }

    public static IEnumerable<string> DetermineRoundWinners(this ScoreCard scoreCard, int roundNumber)
    {
        // Get all players who have at least roundNumber hands
        var playersInRound = scoreCard.Scores
            .Where(kvp => kvp.Value.Count >= roundNumber)
            .Select(kvp => new Player(kvp.Key, kvp.Value[roundNumber - 1]))
            .ToList();

        if (!playersInRound.Any())
            return Enumerable.Empty<string>();

        return playersInRound.DetermineWinners().Select(p => p.Name);
    }

    public static ImmutableDictionary<int, IEnumerable<string>> DetermineAllRoundWinners(this ScoreCard scoreCard)
    {
        if (!scoreCard.Scores.Any())
            return ImmutableDictionary<int, IEnumerable<string>>.Empty;

        // Find the maximum number of rounds played
        var maxRounds = scoreCard.Scores.Max(kvp => kvp.Value.Count);

        // For each round, determine the winners using DetermineRoundWinners
        return Enumerable.Range(1, maxRounds)
            .ToImmutableDictionary(
                round => round,
                round => scoreCard.DetermineRoundWinners(round)
            );
    }
}

