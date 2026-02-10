using Microsoft.Extensions.Logging;

namespace Playground.Projects.Poker.Models;
public record PokerHand : CardDeck
{
    IEnumerable<IGrouping<CardSuit, Card>> suits =>  this.cards.GroupBy(card => card.Suit);
    IEnumerable<IGrouping<CardRank, Card>> ranks => this.cards.GroupBy(card => card.Rank);
    IOrderedEnumerable<CardRank> sortedRanks => this.cards.Select(c => c.Rank).OrderBy(r => r);

    bool isRanksSequential { get 
    {
        var sRanks = sortedRanks.ToList();
        return Enumerable.Range(0, 4).All(i => sRanks[i + 1] - sRanks[i] == 1);
    }}

    public PokerHand (){}

    public PokerHand GetPokerRank() 
    {
        if (cards.Count != 5)
            return new NoPokerRank() {cards = this.cards};

        bool isFlush = suits.Any(suit => suit.Count() == 5);
        bool isThreeOfAKind = ranks.Any(group => group.Count() >= 3);
        bool isFourOfAKind = ranks.Any(group => group.Count() == 4);
        bool isFullHouse = ranks.Any(group => group.Count() == 3) && ranks.Any(group => group.Count() == 2);
        bool isTwoPair = ranks.Count(group => group.Count() == 2) == 2;
        bool isOnePair = ranks.Any(group => group.Count() == 2);
        bool isRoyalFlush = isFlush && isRanksSequential && sortedRanks.Last() == CardRank.Ace;
        bool isStraightFlush = isFlush && isRanksSequential && !isRoyalFlush;
        bool isStraight = isRanksSequential && !isFlush;

        return (isRoyalFlush, isStraightFlush, isFourOfAKind, isFullHouse, isFlush, isStraight, isThreeOfAKind, isTwoPair, isOnePair) switch
        {
            (true, _, _, _, _, _, _, _, _) => new RoyalFlush() {cards = this.cards},
            (_, true, _, _, _, _, _, _, _) => new StraightFlush() {cards = this.cards},
            (_, _, true, _, _, _, _, _, _) => new FourOfAKind() {cards = this.cards},
            (_, _, _, true, _, _, _, _, _) => new FullHouse() {cards = this.cards},
            (_, _, _, _, true, _, _, _, _) => new Flush() {cards = this.cards},
            (_, _, _, _, _, true, _, _, _) => new Straight() {cards = this.cards},
            (_, _, _, _, _, _, true, _, _) => new ThreeOfAKind() {cards = this.cards},
            (_, _, _, _, _, _, _, true, _) => new TwoPair() {cards = this.cards},
            (_, _, _, _, _, _, _, _, true) => new OnePair() {cards = this.cards},
            _ => new HighCard() {cards = this.cards}
        };  
    }

    public int GetPokerRankValue => this switch
        {
            RoyalFlush => 10,
            StraightFlush => 9,
            FourOfAKind => 8,
            FullHouse => 7,
            Flush => 6,
            Straight => 5,
            ThreeOfAKind => 4,
            TwoPair => 3,
            OnePair => 2,
            HighCard => 1,
            NoPokerRank => 0,
            _ => 0
        };
}
//Disciminators for poker hand types
public record RoyalFlush : PokerHand
{
}
public record StraightFlush : PokerHand
{
}
public record FourOfAKind : PokerHand
{
}
public record FullHouse : PokerHand
{
}
public record Flush : PokerHand
{
}
public record Straight : PokerHand
{
}
public record ThreeOfAKind : PokerHand
{
}
public record TwoPair : PokerHand
{
}
public record OnePair : PokerHand
{
}
public record HighCard : PokerHand
{
}
public record NoPokerRank : PokerHand
{
}

