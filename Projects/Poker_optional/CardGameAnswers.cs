using System.Collections.Immutable;
using Playground.Projects.Poker.Extensions;
using Playground.Projects.Poker.Models;
using PlayGround.Extensions;

namespace Playground.Projects.Poker;

public static class PokerGameAnswers
{
    public static void RunSimulation()
    {
        Console.WriteLine("Testing the Deck.");

        var smallDeck = CardDeck.Create()
            .Tap(deck => Console.WriteLine("Shuffled Deck:\n" + deck))
            
            .Shuffle()
            .Tap(deck => Console.WriteLine("Shuffled Deck:\n" + deck))

            .Sort(cards => cards.OrderBy(c => c.Suit).ThenBy(c => c.Rank))
            .Tap(deck => Console.WriteLine("Sorted Deck:\n" + deck))
            
            .Keep(c => c.Rank switch { 
                CardRank.Two or CardRank.Three or CardRank.Five or CardRank.Seven => true, 
                _ => false })
            .Tap(deck => Console.WriteLine("Kept Deck (Only 2,3,5,7):\n" + deck))

            .Remove(c => c.Suit == CardSuit.Hearts)
            .Tap(deck => Console.WriteLine("Removed Hearts:\n" + deck))

            .Fork(deck => deck.AddToTop(new Card(CardSuit.Spades, CardRank.Ace)),
                  deck => deck.AddToBottom(new Card(CardSuit.Diamonds, CardRank.King)),
                  (topAdded, bottomAdded) => bottomAdded.AddDeck(topAdded).RemoveDuplicates())
            .Tap(deck => Console.WriteLine("Added Cards and Removed Duplicates:\n" + deck))

            .Draw(out var drawnCard)
            .Tap(deck => Console.WriteLine($"Drew Card: {drawnCard}\nRemaining Deck:\n" + deck));


        Console.WriteLine("Testing Player.");

        ImmutableList<Player> players = ImmutableList.Create(
            new Player("Alice", new PokerHand()), 
            new Player("Bob", new PokerHand()),
            new Player("Diana", new PokerHand()))

            .Tap(p => Console.WriteLine(string.Join(", ", p.Select(pl => $"{pl.Name} has {pl.Hand.cards.Count} cards on hand"))));   


        System.Console.WriteLine("\nPoker Round Simulation:");
        System.Console.WriteLine("Your code should implement the Poker round simulation below.");
        System.Console.WriteLine("========================");
    }
}