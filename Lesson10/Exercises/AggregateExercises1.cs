using System.Collections.Immutable;

namespace Playground.Lesson10.Exercises;

public static class AggregateExercises1
{
    public static void RunExercises()
    {
        Console.WriteLine("=== Aggregate Exercises ===\n");

        Exercise1_CarryLoop1ToLoop3();
        Exercise2_CarryLoop3ToResultLoop1();
        Exercise3_ModifyInLoop3AndCarryToResult();
    }

    // Exercise 1: Carry "Loop1" all the way to the Loop3 action
    // Goal: Pass the "Loop1" string from the outermost loop down to Loop3 so it can be used in Loop3's action
    static void Exercise1_CarryLoop1ToLoop3()
    {
        Console.WriteLine("--- Exercise 1: Carry Loop1 Identifier to Loop3 ---");
        
        // TODO: Modify the state to carry "Loop1" from Loop1 down through Loop2 to Loop3
        // Hint: You'll need to include the Loop1 identifier in the state passed to Loop2 and Loop3
        
        var resultLoop1 = Enumerable.Range(1, 3)
            .Aggregate(
                ("Loop1", ImmutableList<string>.Empty),    // Initial state for Loop1
                (stateLoop1, idxLoop1) =>
                {
                    Console.WriteLine($"Loop1 iteration: {idxLoop1}");
                    
                    // TODO: Pass stateLoop1.Item1 into Loop2's state
                    var resultLoop2 = Enumerable.Range(1, 3)
                        .Aggregate(
                            ("Loop2", ImmutableList<string>.Empty), // TODO: Include Loop1 identifier here
                            (stateLoop2, idxLoop2) =>
                            {
                                Console.WriteLine($"   Loop2 iteration: {idxLoop2}");

                                // TODO: Pass the Loop1 identifier into Loop3's state
                                var resultLoop3 = Enumerable.Range(1, 3)
                                    .Aggregate(
                                        ("Loop3", ImmutableList<string>.Empty), // TODO: Include Loop1 identifier here
                                        (stateLoop3, idxLoop3) =>
                                        {
                                            // TODO: Use the Loop1 identifier here (e.g., print it or add it to the result)
                                            Console.WriteLine($"      Loop3 iteration: {idxLoop3}");
                                            // Expected output should show "Loop1" is accessible here

                                            var newStateLoop3 = (stateLoop3.Item1, stateLoop3.Item2.Add($"{idxLoop1} - {idxLoop2} - {idxLoop3}"));
                                            return newStateLoop3;
                                        });

                                var newStateLoop2 = (stateLoop2.Item1, stateLoop2.Item2.AddRange(resultLoop3.Item2));
                                return newStateLoop2;
                            });
                    
                    var newStateLoop1 = (stateLoop1.Item1, stateLoop1.Item2.AddRange(resultLoop2.Item2));
                    return newStateLoop1;
                });
        
        Console.WriteLine($"\nAll results: {resultLoop1.Item1}\n{string.Join("\n", resultLoop1.Item2)}\n");
    }

    // Exercise 2: Carry "Loop3" all the way to resultLoop1
    // Goal: Pass the "Loop3" string from the innermost loop up to the final result
    static void Exercise2_CarryLoop3ToResultLoop1()
    {
        Console.WriteLine("--- Exercise 2: Carry Loop3 Identifier to resultLoop1 ---");
        
        // TODO: Modify the return values to carry "Loop3" from Loop3 up through Loop2 to Loop1
        // Hint: You'll need to include the Loop3 identifier in the results returned from Loop3 and Loop2
        
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

                                // TODO: When returning from Loop2, include the Loop3 identifier somehow
                                var newStateLoop2 = (stateLoop2.Item1, stateLoop2.Item2.AddRange(resultLoop3.Item2));
                                return newStateLoop2;
                            });
                    
                    // TODO: When returning from Loop1, include the Loop3 identifier that came from Loop2
                    var newStateLoop1 = (stateLoop1.Item1, stateLoop1.Item2.AddRange(resultLoop2.Item2));
                    return newStateLoop1;
                });
        
        // TODO: resultLoop1 should now contain the "Loop3" identifier
        Console.WriteLine($"\nAll results: {resultLoop1.Item1}\n{string.Join("\n", resultLoop1.Item2)}\n");
        // Expected: resultLoop1.Item1 should be "Loop3" instead of "Loop1"
    }

    // Exercise 3: Modify "Loop1" in Loop3 action to "Loop3 Modified" and carry all the way to resultLoop1
    // Goal: Take "Loop1" from the outer loop, modify it in Loop3, and carry the modified value back to the final result
    static void Exercise3_ModifyInLoop3AndCarryToResult()
    {
        Console.WriteLine("--- Exercise 3: Modify Loop1 to 'Loop3 Modified' in Loop3 and Carry to Result ---");
        
        // TODO: 
        // 1. Pass "Loop1" from Loop1 down to Loop3 (like Exercise 1)
        // 2. In Loop3, modify it to "Loop3 Modified"
        // 3. Pass the modified value back up to resultLoop1 (like Exercise 2)
        
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

                                            // TODO: Access the "Loop1" value and modify it to "Loop3 Modified"
                                            var newStateLoop3 = (stateLoop3.Item1, stateLoop3.Item2.Add($"{idxLoop1} - {idxLoop2} - {idxLoop3}"));
                                            return newStateLoop3;
                                        });

                                // TODO: Pass the modified value up from Loop3
                                var newStateLoop2 = (stateLoop2.Item1, stateLoop2.Item2.AddRange(resultLoop3.Item2));
                                return newStateLoop2;
                            });
                    
                    // TODO: Pass the modified value up from Loop2
                    var newStateLoop1 = (stateLoop1.Item1, stateLoop1.Item2.AddRange(resultLoop2.Item2));
                    return newStateLoop1;
                });
        
        // TODO: resultLoop1.Item1 should now be "Loop3 Modified" instead of "Loop1"
        Console.WriteLine($"\nAll results: {resultLoop1.Item1}\n{string.Join("\n", resultLoop1.Item2)}\n");
        // Expected: resultLoop1.Item1 should be "Loop3 Modified"
    }
}
