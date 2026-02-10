using System.Collections.Immutable;
using Playground.Projects.Poker.Models;

namespace Playground.Projects.Poker.Extensions;

public static class PlayerExtensions
{

    public static IEnumerable<Player> DetermineWinners(this IEnumerable<Player> players)
    {
        if (!players.Any())
            return Enumerable.Empty<Player>();

        var rankedHands = players
            .Select(player => new { Hand = player.Hand.GetPokerRank(), Rank = player.Hand.GetPokerRank().GetPokerRankValue})
            .ToList();

        var maxRank = rankedHands.Max(x => x.Rank);
        return players.Where(p => p.Hand.GetPokerRank().GetPokerRankValue == maxRank);
    }
}
