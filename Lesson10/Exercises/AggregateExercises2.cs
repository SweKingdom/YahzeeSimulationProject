using System.Collections.Immutable;

namespace Playground.Lesson10.Exercises;

public static class AggregateExercises2
{
    public static void RunExercises()
    {
        Console.WriteLine("=== Aggregate Exercises 2 - State Modification ===\n");

        Exercise1_RemoveNumbersInLoop3();
        Exercise2_DistributeNumbersToPlayers();
    }

    // Exercise 1: Replace "Loop1" with numbers, pass them to Loop3, remove last number, carry modified list back to resultLoop1
    // Goal: Thread a mutable state (list of numbers) down to Loop3, modify it there, and return the modified state
    static void Exercise1_RemoveNumbersInLoop3()
    {
        Console.WriteLine("--- Exercise 1: Remove Numbers in Loop3 and Carry Back to Result ---");
        
        var numbers = ImmutableList.Create(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27);
        
        Console.WriteLine($"Initial numbers: [{string.Join(", ", numbers)}]");
        Console.WriteLine($"Total numbers: {numbers.Count}\n");
        
        // TODO: 
        // 1. Replace "Loop1" with the numbers list
        // 2. Pass numbers down through Loop2 to Loop3
        // 3. In Loop3, remove the last number from the list
        // 4. Pass the modified numbers list back up to resultLoop1
        
        var resultLoop1 = Enumerable.Range(1, 3)
            .Aggregate(
                ("Loop1", ImmutableList<string>.Empty),    // TODO: Replace "Loop1" with numbers list
                (stateLoop1, idxLoop1) =>
                {
                    Console.WriteLine($"Loop1 iteration: {idxLoop1}");
                    
                    var resultLoop2 = Enumerable.Range(1, 3)
                        .Aggregate(
                            ("Loop2", ImmutableList<string>.Empty), // TODO: Include numbers from Loop1
                            (stateLoop2, idxLoop2) =>
                            {
                                Console.WriteLine($"   Loop2 iteration: {idxLoop2}");

                                var resultLoop3 = Enumerable.Range(1, 3)
                                    .Aggregate(
                                        ("Loop3", ImmutableList<string>.Empty), // TODO: Include numbers from Loop2
                                        (stateLoop3, idxLoop3) =>
                                        {
                                            Console.WriteLine($"      Loop3 iteration: {idxLoop3}");
                                            
                                            // TODO: Remove the last number from the numbers list
                                            // Hint: Use .RemoveAt(numbers.Count - 1) on an ImmutableList

                                            var newStateLoop3 = (stateLoop3.Item1, stateLoop3.Item2.Add($"{idxLoop1} - {idxLoop2} - {idxLoop3}"));
                                            return newStateLoop3;
                                        });

                                // TODO: Pass the modified numbers list up from Loop3
                                var newStateLoop2 = (stateLoop2.Item1, stateLoop2.Item2.AddRange(resultLoop3.Item2));
                                return newStateLoop2;
                            });
                    
                    // TODO: Pass the modified numbers list up from Loop2
                    var newStateLoop1 = (stateLoop1.Item1, stateLoop1.Item2.AddRange(resultLoop2.Item2));
                    return newStateLoop1;
                });
        
        // TODO: Display the final numbers list (should have 27 numbers removed)
        Console.WriteLine($"\nAll results: {resultLoop1.Item1}\n{string.Join("\n", resultLoop1.Item2)}\n");
        // Expected: resultLoop1 should contain the numbers list with all numbers removed
    }

    // Exercise 2: Pass both numbers and players to Loop3, remove last number and give it to a player
    // Goal: Thread two mutable states (numbers and players) through nested loops
    static void Exercise2_DistributeNumbersToPlayers()
    {
        Console.WriteLine("--- Exercise 2: Distribute Numbers to Players in Loop3 ---");
        
        var numbers = ImmutableList.Create(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27);
        var players = ImmutableList.Create(
            ("Alice", ImmutableList<int>.Empty), 
            ("Bob", ImmutableList<int>.Empty), 
            ("Charlie", ImmutableList<int>.Empty)
        );
        
        Console.WriteLine($"Initial numbers: [{string.Join(", ", numbers)}]");
        Console.WriteLine($"Initial players: {string.Join(", ", players.Select(p => $"{p.Item1}: []"))}\n");
        
        // TODO:
        // 1. Pass both numbers and players down to Loop3
        // 2. In Loop3, remove the last number from numbers
        // 3. Give the removed number to a player (distribute round-robin based on iteration)
        // 4. Pass both modified lists back up to resultLoop1
        
        var resultLoop1 = Enumerable.Range(1, 3)
            .Aggregate(
                ("Loop1", numbers, players, ImmutableList<string>.Empty),    // TODO: Include both numbers and players
                (stateLoop1, idxLoop1) =>
                {
                    Console.WriteLine($"Loop1 iteration: {idxLoop1}");
                    
                    var resultLoop2 = Enumerable.Range(1, 3)
                        .Aggregate(
                            ("Loop2", ImmutableList<int>.Empty, ImmutableList<(string, ImmutableList<int>)>.Empty, ImmutableList<string>.Empty), // TODO: Pass numbers and players from Loop1
                            (stateLoop2, idxLoop2) =>
                            {
                                Console.WriteLine($"   Loop2 iteration: {idxLoop2}");

                                var resultLoop3 = Enumerable.Range(1, 3)
                                    .Aggregate(
                                        ("Loop3", ImmutableList<int>.Empty, ImmutableList<(string, ImmutableList<int>)>.Empty, ImmutableList<string>.Empty), // TODO: Pass numbers and players from Loop2
                                        (stateLoop3, idxLoop3) =>
                                        {
                                            Console.WriteLine($"      Loop3 iteration: {idxLoop3}");
                                            
                                            // TODO: 
                                            // 1. Get the last number from the numbers list
                                            // 2. Remove it from numbers
                                            // 3. Determine which player gets it (round-robin or modulo logic)
                                            // 4. Add the number to that player's list
                                            
                                            // Hint: Use a counter to determine player index: (counter % 3)
                                            // Hint: To update a player's numbers: players.SetItem(playerIdx, (name, numbers.Add(drawnNumber)))

                                            var newStateLoop3 = (stateLoop3.Item1, stateLoop3.Item2, stateLoop3.Item3, stateLoop3.Item4.Add($"{idxLoop1} - {idxLoop2} - {idxLoop3}"));
                                            return newStateLoop3;
                                        });

                                // TODO: Pass modified numbers and players up from Loop3
                                var newStateLoop2 = (stateLoop2.Item1, stateLoop2.Item2, stateLoop2.Item3, stateLoop2.Item4.AddRange(resultLoop3.Item4));
                                return newStateLoop2;
                            });
                    
                    // TODO: Pass modified numbers and players up from Loop2
                    var newStateLoop1 = (stateLoop1.Item1, stateLoop1.Item2, stateLoop1.Item3, stateLoop1.Item4.AddRange(resultLoop2.Item4));
                    return newStateLoop1;
                });
        
        // TODO: Display final state of both numbers and players
        Console.WriteLine($"\nAll results: {resultLoop1.Item1}\n{string.Join("\n", resultLoop1.Item4)}\n");
        // Expected: numbers should be empty, players should have the numbers distributed among them
    }
}
