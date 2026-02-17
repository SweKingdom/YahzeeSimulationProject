using System.Collections.Immutable;

namespace Playground.Lesson10.Exercises;

public static class AggregateExercises1Answers
{
    public static void RunAnswers()
    {
        Console.WriteLine("=== Aggregate Exercises - ANSWERS ===\n");

        Exercise1_CarryLoop1ToLoop3_Answer();
        Exercise2_CarryLoop3ToResultLoop1_Answer();
        Exercise3_ModifyInLoop3AndCarryToResult_Answer();
    }

    // Exercise 1 ANSWER: Carry "Loop1" all the way to the Loop3 action
    static void Exercise1_CarryLoop1ToLoop3_Answer()
    {
        Console.WriteLine("--- Exercise 1 ANSWER: Carry Loop1 Identifier to Loop3 ---");
        
        // Solution: Expand the state tuple to include the Loop1 identifier as we pass it down
        var resultLoop1 = Enumerable.Range(1, 3)
            .Aggregate(
                ("Loop1", ImmutableList<string>.Empty),
                (stateLoop1, idxLoop1) =>
                {
                    Console.WriteLine($"Loop1 iteration: {idxLoop1}");
                    
                    // Pass Loop1 identifier into Loop2's initial state
                    var resultLoop2 = Enumerable.Range(1, 3)
                        .Aggregate(
                            (stateLoop1.Item1, "Loop2", ImmutableList<string>.Empty), // Now includes Loop1 identifier
                            (stateLoop2, idxLoop2) =>
                            {
                                Console.WriteLine($"   Loop2 iteration: {idxLoop2}");

                                // Pass Loop1 identifier into Loop3's initial state
                                var resultLoop3 = Enumerable.Range(1, 3)
                                    .Aggregate(
                                        (stateLoop2.Item1, "Loop3", ImmutableList<string>.Empty), // Now includes Loop1 identifier
                                        (stateLoop3, idxLoop3) =>
                                        {
                                            // Now we can access the Loop1 identifier in Loop3!
                                            Console.WriteLine($"      Loop3 iteration: {idxLoop3} - Outer loop was: {stateLoop3.Item1}");

                                            var newStateLoop3 = (stateLoop3.Item1, stateLoop3.Item2, stateLoop3.Item3.Add($"{idxLoop1} - {idxLoop2} - {idxLoop3}"));
                                            return newStateLoop3;
                                        });

                                var newStateLoop2 = (stateLoop2.Item1, stateLoop2.Item2, stateLoop2.Item3.AddRange(resultLoop3.Item3));
                                return newStateLoop2;
                            });
                    
                    var newStateLoop1 = (stateLoop1.Item1, stateLoop1.Item2.AddRange(resultLoop2.Item3));
                    return newStateLoop1;
                });
        
        Console.WriteLine($"\nAll results: {resultLoop1.Item1}\n{string.Join("\n", resultLoop1.Item2)}\n");
    }

    // Exercise 2 ANSWER: Carry "Loop3" all the way to resultLoop1
    static void Exercise2_CarryLoop3ToResultLoop1_Answer()
    {
        Console.WriteLine("--- Exercise 2 ANSWER: Carry Loop3 Identifier to resultLoop1 ---");
        
        // Solution: Replace the Loop identifiers as we return up the chain
        var resultLoop1 = Enumerable.Range(1, 3)
            .Aggregate(
                ("Loop1", ImmutableList<string>.Empty),
                (stateLoop1, idxLoop1) =>
                {
                    Console.WriteLine($"Loop1 iteration: {idxLoop1}");
                    
                    var resultLoop2 = Enumerable.Range(1, 3)
                        .Aggregate(
                            ("Loop2", ImmutableList<string>.Empty),
                            (stateLoop2, idxLoop2) =>
                            {
                                Console.WriteLine($"   Loop2 iteration: {idxLoop2}");

                                var resultLoop3 = Enumerable.Range(1, 3)
                                    .Aggregate(
                                        ("Loop3", ImmutableList<string>.Empty),
                                        (stateLoop3, idxLoop3) =>
                                        {
                                            Console.WriteLine($"      Loop3 iteration: {idxLoop3}");

                                            var newStateLoop3 = (stateLoop3.Item1, stateLoop3.Item2.Add($"{idxLoop1} - {idxLoop2} - {idxLoop3}"));
                                            return newStateLoop3;
                                        });

                                // Return Loop3's identifier instead of Loop2's
                                var newStateLoop2 = (resultLoop3.Item1, stateLoop2.Item2.AddRange(resultLoop3.Item2));
                                return newStateLoop2;
                            });
                    
                    // Return Loop3's identifier (which came from Loop2) instead of Loop1's
                    var newStateLoop1 = (resultLoop2.Item1, stateLoop1.Item2.AddRange(resultLoop2.Item2));
                    return newStateLoop1;
                });
        
        Console.WriteLine($"\nAll results: {resultLoop1.Item1}\n{string.Join("\n", resultLoop1.Item2)}\n");
        Console.WriteLine($"Final identifier is: {resultLoop1.Item1} (should be 'Loop3')");
    }

    // Exercise 3 ANSWER: Modify "Loop1" in Loop3 action to "Loop3 Modified" and carry all the way to resultLoop1
    static void Exercise3_ModifyInLoop3AndCarryToResult_Answer()
    {
        Console.WriteLine("--- Exercise 3 ANSWER: Modify Loop1 to 'Loop3 Modified' in Loop3 and Carry to Result ---");
        
        // Solution: Combine Exercise 1 (pass down) and Exercise 2 (pass up) with a modification in Loop3
        var resultLoop1 = Enumerable.Range(1, 3)
            .Aggregate(
                ("Loop1", ImmutableList<string>.Empty),
                (stateLoop1, idxLoop1) =>
                {
                    Console.WriteLine($"Loop1 iteration: {idxLoop1}");
                    
                    var resultLoop2 = Enumerable.Range(1, 3)
                        .Aggregate(
                            (stateLoop1.Item1, "Loop2", ImmutableList<string>.Empty), // Pass Loop1 down
                            (stateLoop2, idxLoop2) =>
                            {
                                Console.WriteLine($"   Loop2 iteration: {idxLoop2}");

                                var resultLoop3 = Enumerable.Range(1, 3)
                                    .Aggregate(
                                        (stateLoop2.Item1, "Loop3", ImmutableList<string>.Empty), // Pass Loop1 down
                                        (stateLoop3, idxLoop3) =>
                                        {
                                            Console.WriteLine($"      Loop3 iteration: {idxLoop3} - Received: {stateLoop3.Item1}");

                                            // Modify the Loop1 identifier to "Loop3 Modified"
                                            var modifiedIdentifier = "Loop3 Modified";
                                            Console.WriteLine($"      Modifying '{stateLoop3.Item1}' to '{modifiedIdentifier}'");

                                            var newStateLoop3 = (modifiedIdentifier, stateLoop3.Item2, stateLoop3.Item3.Add($"{idxLoop1} - {idxLoop2} - {idxLoop3}"));
                                            return newStateLoop3;
                                        });

                                // Pass the modified identifier up from Loop3
                                var newStateLoop2 = (resultLoop3.Item1, stateLoop2.Item2, stateLoop2.Item3.AddRange(resultLoop3.Item3));
                                return newStateLoop2;
                            });
                    
                    // Pass the modified identifier up from Loop2
                    var newStateLoop1 = (resultLoop2.Item1, stateLoop1.Item2.AddRange(resultLoop2.Item3));
                    return newStateLoop1;
                });
        
        Console.WriteLine($"\nAll results: {resultLoop1.Item1}\n{string.Join("\n", resultLoop1.Item2)}\n");
        Console.WriteLine($"Final identifier is: {resultLoop1.Item1} (should be 'Loop3 Modified')");
    }
}
