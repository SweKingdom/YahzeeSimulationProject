using System.Collections.Immutable;

namespace Playground.Projects.Yahtzee.Models;

// Tracks a single players scores across all 13 categories for a full game.
public record ScoreCard
{
    // Used to determine which categories count toward the upper section bonus
    private static readonly ImmutableHashSet<string> UpperSection =
        ImmutableHashSet.Create("Ones", "Twos", "Threes", "Fours", "Fives", "Sixes");

    public ImmutableDictionary<string, int> Scores { get; init; } =
        ImmutableDictionary<string, int>.Empty;

    // True if the player hasn't filled this category yet
    public bool IsAvailable(string category) => !Scores.ContainsKey(category);

    public ScoreCard AddScore(string category, int score) =>
        this with { Scores = Scores.Add(category, score) };
    
    // Sum of only the upper section categories
    public int UpperSectionTotal =>
        Scores.Where(kvp => UpperSection.Contains(kvp.Key)).Sum(kvp => kvp.Value);

    // 35 bonus points if the player scored at least 63 in the upper section
    public int UpperBonus => UpperSectionTotal >= 63 ? 35 : 0;

    public int YahtzeeBonusCount { get; init; } = 0;

    public ScoreCard AddYahtzeeBonus() => this with { YahtzeeBonusCount = YahtzeeBonusCount + 1 };

    public int YahtzeeBonusTotal => YahtzeeBonusCount * 100;

    public int Total => Scores.Values.Sum() + UpperBonus + YahtzeeBonusTotal;
}