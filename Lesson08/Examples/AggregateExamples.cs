using System.Collections.Immutable;

namespace Playground.Lesson08.Examples;

public static class AggregateExamples
{
    public static void RunExamples()
    {
        Console.WriteLine("=== Aggregate Examples ===\n");

        Example1_BasicSum();
        Example2_BuildingAList();
        Example3_AccumulatingState();
        Example4_NestedAggregateSimple();
        Example5_NestedAggregateWithTuples();
        Example6_CardDealingPattern();
    }

    // Example 1: Basic sum using Aggregate
    static void Example1_BasicSum()
    {
        Console.WriteLine("--- Example 1: Basic Sum ---");
        
        // Range(1, 5) generates: 1, 2, 3, 4, 5
        var sum = Enumerable.Range(1, 5)
            .Aggregate(
                0,  // Initial value (seed)
                (accumulator, currentNumber) => accumulator + currentNumber
            );
        
        Console.WriteLine($"Sum of 1-5: {sum}");
        Console.WriteLine("Breakdown: 0 + 1 + 2 + 3 + 4 + 5 = 15\n");
    }

    // Example 2: Building a list of strings
    static void Example2_BuildingAList()
    {
        Console.WriteLine("--- Example 2: Building a List ---");
        
        var result = Enumerable.Range(1, 5)
            .Aggregate(
                ImmutableList<string>.Empty,  // Start with empty list
                (list, number) => list.Add($"Item {number}")
            );
        
        Console.WriteLine($"Built list: {string.Join(", ", result)}");
        Console.WriteLine();
    }

    // Example 3: Accumulating state with tuple
    static void Example3_AccumulatingState()
    {
        Console.WriteLine("--- Example 3: Accumulating State with Tuple ---");
        
        // Count how many iterations and sum the numbers
        var (count, sum) = Enumerable.Range(1, 5)
            .Aggregate(
                (Count: 0, Sum: 0),  // Initial state
                (state, number) => (state.Count + 1, state.Sum + number)
            );
        
        Console.WriteLine($"Count: {count}, Sum: {sum}");
        Console.WriteLine();
    }

    // Example 4: Simple nested aggregate
    static void Example4_NestedAggregateSimple()
    {
        Console.WriteLine("--- Example 4: Simple Nested Aggregate ---");
        
        // For each number 1-3, multiply it by each number 1-2
        var results = Enumerable.Range(1, 3)
            .Aggregate(
                ImmutableList<string>.Empty,
                (outerList, outerNumber) =>
                {
                    Console.WriteLine($"  Outer iteration: {outerNumber}");
                    
                    // Inner aggregate for each outer iteration
                    var innerResults = Enumerable.Range(1, 2)
                        .Aggregate(
                            ImmutableList<string>.Empty,
                            (innerList, innerNumber) =>
                            {
                                var product = outerNumber * innerNumber;
                                Console.WriteLine($"    Inner: {outerNumber} × {innerNumber} = {product}");
                                return innerList.Add($"{outerNumber}×{innerNumber}={product}");
                            });
                    
                    return outerList.AddRange(innerResults);
                });
        
        Console.WriteLine($"All results: {string.Join(", ", results)}\n");
    }

    // Example 5: Nested aggregate maintaining state
    static void Example5_NestedAggregateWithTuples()
    {
        Console.WriteLine("--- Example 5: Nested Aggregate with State ---");
        
        var players = ImmutableList.Create("Alice", "Bob", "Charlie");
        
        // Deal 3 "rounds" where each player gets one number per round
        var (finalCounter, playersWithNumbers) = Enumerable.Range(1, 3)
            .Aggregate(
                (Counter: 0, Players: players.Select(p => (Name: p, Numbers: ImmutableList<int>.Empty)).ToImmutableList()),
                (outerState, round) =>
                {
                    Console.WriteLine($"  Round {round}:");
                    
                    // Inner aggregate: give one number to each player
                    var (newCounter, updatedPlayers) = outerState.Players
                        .Aggregate(
                            (Counter: outerState.Counter, Players: ImmutableList<(string Name, ImmutableList<int> Numbers)>.Empty),
                            (innerState, player) =>
                            {
                                var nextNumber = innerState.Counter + 1;
                                var updatedPlayer = (player.Name, player.Numbers.Add(nextNumber));
                                Console.WriteLine($"    {player.Name} gets {nextNumber}");
                                
                                return (nextNumber, innerState.Players.Add(updatedPlayer));
                            });
                    
                    return (newCounter, updatedPlayers);
                });
        
        Console.WriteLine("\nFinal results:");
        foreach (var player in playersWithNumbers)
        {
            Console.WriteLine($"  {player.Name}: [{string.Join(", ", player.Numbers)}]");
        }
        Console.WriteLine();
    }

    // Example 6: Card dealing pattern (simplified)
    static void Example6_CardDealingPattern()
    {
        Console.WriteLine("--- Example 6: Card Dealing Pattern (Simplified) ---");
        
        var deck = ImmutableList.Create(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
        var players = ImmutableList.Create("Alice", "Bob", "Charlie");
        
        Console.WriteLine($"Initial deck: [{string.Join(", ", deck)}]");
        Console.WriteLine($"Players: {string.Join(", ", players)}\n");
        
        // Deal 3 cards to each player, one at a time
        var (remainingDeck, playersWithCards) = Enumerable.Range(1, 3)
            .Aggregate(
                (Deck: deck, Players: players.Select(p => (Name: p, Hand: ImmutableList<int>.Empty)).ToImmutableList()),
                (state, cardRound) =>
                {
                    Console.WriteLine($"Dealing round {cardRound}:");
                    
                    // Deal one card to each player
                    var (deckAfterRound, updatedPlayers) = state.Players
                        .Aggregate(
                            (Deck: state.Deck, Players: ImmutableList<(string Name, ImmutableList<int> Hand)>.Empty),
                            (innerState, player) =>
                            {
                                // Draw top card
                                var card = innerState.Deck.Last();
                                var newDeck = innerState.Deck.RemoveAt(innerState.Deck.Count - 1);
                                
                                // Add to player's hand
                                var updatedPlayer = (player.Name, player.Hand.Add(card));
                                Console.WriteLine($"  {player.Name} gets {card}");
                                
                                return (newDeck, innerState.Players.Add(updatedPlayer));
                            });
                    
                    return (deckAfterRound, updatedPlayers);
                });
        
        Console.WriteLine($"\nRemaining deck: [{string.Join(", ", remainingDeck)}]");
        Console.WriteLine("\nFinal hands:");
        foreach (var player in playersWithCards)
        {
            Console.WriteLine($"  {player.Name}: [{string.Join(", ", player.Hand)}]");
        }
        Console.WriteLine();
    }
}
