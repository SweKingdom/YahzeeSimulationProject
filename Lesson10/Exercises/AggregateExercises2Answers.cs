using System.Collections.Immutable;

namespace Playground.Lesson10.Exercises;

public static class AggregateExercises2Answers
{
    public static void RunAnswers()
    {
        Console.WriteLine("=== Aggregate Exercises 2 - ANSWERS ===\n");

        //Exercise1_RemoveNumbersInLoop3_Answer();
        Exercise2_DistributeNumbersToPlayers_Answer();
    }

    // Exercise 1 ANSWER: Replace "Loop1" with numbers, pass them to Loop3, remove last number, carry modified list back
    static void Exercise1_RemoveNumbersInLoop3_Answer()
    {
        Console.WriteLine("--- Exercise 1 ANSWER: Remove Numbers in Loop3 and Carry Back to Result ---");
        
        var numbers = ImmutableList.Create(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27);
        
        Console.WriteLine($"Initial numbers: [{string.Join(", ", numbers)}]");
        Console.WriteLine($"Total numbers: {numbers.Count}\n");
        
        // Solution: Replace "Loop1" with numbers and thread them through all loops
        var resultLoop1 = Enumerable.Range(1, 3)
            .Aggregate(
                (numbers, ImmutableList<string>.Empty),    // State is now: (numbers, results)
                (stateLoop1, idxLoop1) =>
                {
                    Console.WriteLine($"Loop1 iteration: {idxLoop1}, Numbers count: {stateLoop1.Item1.Count}");
                    
                    var resultLoop2 = Enumerable.Range(1, 3)
                        .Aggregate(
                            (stateLoop1.Item1, ImmutableList<string>.Empty), // Pass numbers from Loop1
                            (stateLoop2, idxLoop2) =>
                            {
                                Console.WriteLine($"   Loop2 iteration: {idxLoop2}, Numbers count: {stateLoop2.Item1.Count}");

                                var resultLoop3 = Enumerable.Range(1, 3)
                                    .Aggregate(
                                        (stateLoop2.Item1, ImmutableList<string>.Empty), // Pass numbers from Loop2
                                        (stateLoop3, idxLoop3) =>
                                        {
                                            var currentNumbers = stateLoop3.Item1;
                                            
                                            // Remove the last number
                                            var numberToRemove = currentNumbers.Count > 0 ? currentNumbers[currentNumbers.Count - 1] : -1;
                                            var updatedNumbers = currentNumbers.Count > 0 
                                                ? currentNumbers.RemoveAt(currentNumbers.Count - 1) 
                                                : currentNumbers;
                                            
                                            Console.WriteLine($"      Loop3 iteration: {idxLoop3}, Removed: {numberToRemove}, Remaining: {updatedNumbers.Count}");

                                            var newStateLoop3 = (updatedNumbers, stateLoop3.Item2.Add($"{idxLoop1} - {idxLoop2} - {idxLoop3}"));
                                            return newStateLoop3;
                                        });

                                // Pass the modified numbers list up from Loop3
                                var newStateLoop2 = (resultLoop3.Item1, stateLoop2.Item2.AddRange(resultLoop3.Item2));
                                return newStateLoop2;
                            });
                    
                    // Pass the modified numbers list up from Loop2
                    var newStateLoop1 = (resultLoop2.Item1, stateLoop1.Item2.AddRange(resultLoop2.Item2));
                    return newStateLoop1;
                });
        
        Console.WriteLine($"\nFinal numbers remaining: [{string.Join(", ", resultLoop1.Item1)}]");
        Console.WriteLine($"Numbers removed: {27 - resultLoop1.Item1.Count}");
    }

    // Exercise 2 ANSWER: Pass both numbers and players to Loop3, remove last number and give it to a player
    static void Exercise2_DistributeNumbersToPlayers_Answer()
    {
        Console.WriteLine("--- Exercise 2 ANSWER: Distribute Numbers to Players in Loop3 ---");
        
        var numbers = ImmutableList.Create(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27);
        var players = ImmutableList.Create(
            ("Alice", ImmutableList<int>.Empty), 
            ("Bob", ImmutableList<int>.Empty), 
            ("Charlie", ImmutableList<int>.Empty)
        );
        
        Console.WriteLine($"Initial numbers: [{string.Join(", ", numbers)}]");
        Console.WriteLine($"Initial players: {string.Join(", ", players.Select(p => $"{p.Item1}: []"))}\n");
        
        // Solution: Thread both numbers and players through all loops, with a counter for distribution
        var resultLoop1 = Enumerable.Range(1, 3)
            .Aggregate(
                (numbers, players),    // State: (numbers, players)
                (stateLoop1, idxLoop1) =>
                {
                    Console.WriteLine($"Loop1 iteration: {idxLoop1}");
                    
                    var resultLoop2 = Enumerable.Range(1, 3)
                        .Aggregate(
                            (stateLoop1.Item1, stateLoop1.Item2), // Pass all state from Loop1
                            (stateLoop2, idxLoop2) =>
                            {
                                Console.WriteLine($"   Loop2 iteration: {idxLoop2}");

                                var resultLoop3 = Enumerable.Range(1, 3)
                                    .Aggregate(
                                        (stateLoop2.Item1, stateLoop2.Item2), // Pass all state from Loop2
                                        (stateLoop3, idxLoop3) =>
                                        {
                                            var currentNumbers = stateLoop3.Item1;
                                            var currentPlayers = stateLoop3.Item2;
                                            
                                            // Remove the last number if available
                                            if (currentNumbers.Count > 0)
                                            {
                                                var drawnNumber = currentNumbers[currentNumbers.Count - 1];
                                                var updatedNumbers = currentNumbers.RemoveAt(currentNumbers.Count - 1);
                                                
                                                // Add the drawn number to the player's list
                                                var playerIndex = idxLoop3 - 1;
                                                var updatedPlayer = (currentPlayers[playerIndex].Item1, currentPlayers[playerIndex].Item2.Add(drawnNumber));
                                                var updatedPlayers = currentPlayers.SetItem(playerIndex, updatedPlayer);
                                                
                                                Console.WriteLine($"      Loop3 iteration: {idxLoop3} - {updatedPlayers[playerIndex].Item1} receives {drawnNumber}");
                                                
                                                var newStateLoop3 = (updatedNumbers, updatedPlayers);
                                                return newStateLoop3;
                                            }
                                            else
                                            {
                                                Console.WriteLine($"      Loop3 iteration: {idxLoop3} - No numbers left!");
                                                var newStateLoop3 = (currentNumbers, currentPlayers);
                                                return newStateLoop3;
                                            }
                                        });

                                // Pass modified state up from Loop3
                                var newStateLoop2 = (resultLoop3.Item1, resultLoop3.Item2);
                                return newStateLoop2;
                            });
                    
                    // Pass modified state up from Loop2
                    var newStateLoop1 = (resultLoop2.Item1, resultLoop2.Item2);
                    return newStateLoop1;
                });
        
        Console.WriteLine($"\nFinal numbers remaining: [{string.Join(", ", resultLoop1.Item1)}]");
        Console.WriteLine($"Numbers distributed: {27 - resultLoop1.Item1.Count}");
        Console.WriteLine($"\nFinal player states:");
        foreach (var (name, nums) in resultLoop1.Item2)
        {
            Console.WriteLine($"  {name}: [{string.Join(", ", nums)}] (Count: {nums.Count})");
        }
    }
}
