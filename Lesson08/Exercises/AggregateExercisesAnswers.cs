namespace Playground.Lesson08.Exercises;

public static class AggregateExercisesAnswers
{
    public static void Exercise1_BasicProduct()
    {
        Console.WriteLine("=== Exercise 1: Basic Product ===\n");
        
        var numbers = Enumerable.Range(1, 5);
        
        var product = numbers.Aggregate(
            seed: 1,
            func: (accumulator, current) =>
            {
                var result = accumulator * current;
                Console.WriteLine($"Step: {accumulator} * {current} = {result}");
                return result;
            });
        
        Console.WriteLine($"\nFinal Product: {product}");
        Console.WriteLine();
    }

    public static void Exercise2_BuildingDictionary()
    {
        Console.WriteLine("=== Exercise 2: Building a Dictionary ===\n");
        
        var numbers = Enumerable.Range(1, 5);
        
        var dictionary = numbers.Aggregate(
            seed: new Dictionary<int, int>(),
            func: (dict, current) =>
            {
                dict[current] = current * current;
                Console.WriteLine($"Added: {current} -> {current * current}");
                return dict;
            });
        
        Console.WriteLine("\nFinal Dictionary:");
        foreach (var kvp in dictionary)
        {
            Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
        }
        Console.WriteLine();
    }

    public static void Exercise3_AccumulatingStateWithList()
    {
        Console.WriteLine("=== Exercise 3: Accumulating State with List ===\n");
        
        var numbers = Enumerable.Range(1, 10);
        
        var result = numbers.Aggregate(
            seed: (Count: 0, Sum: 0, Sums: new List<int>()),
            func: (state, current) =>
            {
                var newSum = state.Sum + current;
                state.Sums.Add(newSum);
                var newCount = state.Count + 1;
                Console.WriteLine($"Iteration {newCount}: Added {current}, Sum = {newSum}");
                return (newCount, newSum, state.Sums);
            });
        
        Console.WriteLine($"\nTotal Iterations: {result.Count}");
        Console.WriteLine($"Final Sum: {result.Sum}");
        Console.WriteLine($"Intermediate Sums: [{string.Join(", ", result.Sums)}]");
        Console.WriteLine();
    }

    public static void Exercise4_NestedAggregateWithLists()
    {
        Console.WriteLine("=== Exercise 4: Nested Aggregate with Lists ===\n");
        
        var outerNumbers = Enumerable.Range(1, 3);
        
        var results = outerNumbers.Aggregate(
            seed: new List<List<string>>(),
            func: (outerList, outerNum) =>
            {
                var innerNumbers = Enumerable.Range(1, 2);
                
                var innerList = innerNumbers.Aggregate(
                    seed: new List<string>(),
                    func: (innerAcc, innerNum) =>
                    {
                        var multiplication = $"{outerNum} × {innerNum} = {outerNum * innerNum}";
                        innerAcc.Add(multiplication);
                        return innerAcc;
                    });
                
                outerList.Add(innerList);
                Console.WriteLine($"Outer number {outerNum}: [{string.Join(", ", innerList)}]");
                return outerList;
            });
        
        Console.WriteLine("\nAll Results:");
        for (int i = 0; i < results.Count; i++)
        {
            Console.WriteLine($"  Set {i + 1}: [{string.Join(", ", results[i])}]");
        }
        Console.WriteLine();
    }

    public static void Exercise5_NestedAggregateWithState()
    {
        Console.WriteLine("=== Exercise 5: Nested Aggregate with State ===\n");
        
        var rounds = Enumerable.Range(1, 3);
        var deck = Enumerable.Range(1, 10).ToList();
        
        var gameState = rounds.Aggregate(
            seed: (
                Deck: deck,
                Player1: new List<int>(),
                Player2: new List<int>(),
                Player3: new List<int>()
            ),
            func: (state, round) =>
            {
                Console.WriteLine($"Round {round}:");
                
                var players = new[] { "Player1", "Player2", "Player3" };
                
                var updatedState = players.Aggregate(
                    seed: state,
                    func: (innerState, player) =>
                    {
                        if (innerState.Deck.Count == 0)
                        {
                            Console.WriteLine($"  {player}: No cards left in deck");
                            return innerState;
                        }
                        
                        var card = innerState.Deck[0];
                        var newDeck = innerState.Deck.Skip(1).ToList();
                        
                        var newState = player switch
                        {
                            "Player1" => (newDeck, innerState.Player1.Append(card).ToList(), innerState.Player2, innerState.Player3),
                            "Player2" => (newDeck, innerState.Player1, innerState.Player2.Append(card).ToList(), innerState.Player3),
                            "Player3" => (newDeck, innerState.Player1, innerState.Player2, innerState.Player3.Append(card).ToList()),
                            _ => innerState
                        };
                        
                        Console.WriteLine($"  {player} draws card {card}");
                        return newState;
                    });
                
                Console.WriteLine();
                return updatedState;
            });
        
        Console.WriteLine("Final Hands:");
        Console.WriteLine($"Player 1: [{string.Join(", ", gameState.Player1)}]");
        Console.WriteLine($"Player 2: [{string.Join(", ", gameState.Player2)}]");
        Console.WriteLine($"Player 3: [{string.Join(", ", gameState.Player3)}]");
        Console.WriteLine($"Remaining Deck: [{string.Join(", ", gameState.Deck)}]");
        Console.WriteLine();
    }

    public static void Exercise6_CardDealingPattern()
    {
        Console.WriteLine("=== Exercise 6: Card Dealing Pattern ===\n");
        
        var deck = Enumerable.Range(1, 52).ToList();
        var numPlayers = 4;
        var cardsPerPlayer = 5;
        
        var dealState = Enumerable.Range(0, numPlayers).Aggregate(
            seed: (Deck: deck, Hands: new List<List<int>>()),
            func: (state, playerIndex) =>
            {
                var hand = Enumerable.Range(0, cardsPerPlayer).Aggregate(
                    seed: (PlayerDeck: state.Deck, PlayerHand: new List<int>()),
                    func: (innerState, _) =>
                    {
                        if (innerState.PlayerDeck.Count == 0)
                            return innerState;
                        
                        var card = innerState.PlayerDeck[0];
                        var newDeck = innerState.PlayerDeck.Skip(1).ToList();
                        var newHand = innerState.PlayerHand.Append(card).ToList();
                        
                        return (newDeck, newHand);
                    });
                
                var newHands = state.Hands.Append(hand.PlayerHand).ToList();
                Console.WriteLine($"Player {playerIndex + 1} dealt: [{string.Join(", ", hand.PlayerHand)}]");
                
                return (hand.PlayerDeck, newHands);
            });
        
        Console.WriteLine($"\nRemaining cards in deck: {dealState.Deck.Count}");
        Console.WriteLine($"Remaining deck: [{string.Join(", ", dealState.Deck.Take(10))}...]");
        Console.WriteLine();
    }
}
