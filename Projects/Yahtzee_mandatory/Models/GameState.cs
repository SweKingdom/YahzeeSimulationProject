using System.Collections.Immutable;

namespace Playground.Projects.Yahtzee.Models;

public record PlayerState(Player Player, ScoreCard ScoreCard, string LastCategory = "", int LastScore = 0);

public record GameState(ImmutableList<PlayerState> PlayerStates, int CurrentRound);