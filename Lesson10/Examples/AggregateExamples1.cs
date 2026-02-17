using System.Collections.Immutable;

namespace Playground.Lesson10.Examples;

public static class AggregateExamples1
{
    public static void RunExamples()
    {
        Console.WriteLine("=== Nested Aggregate Examples ===\n");

        Example_NestedAggregate();
        Example_NestedForLoops();
    }


    // Example 4: Nested aggregate with state accumulation
    static void Example_NestedAggregate()
    {
        Console.WriteLine("--- Example 4: Nested Aggregate with State Accumulation ---");
        
        // Loop1
        var resultLoop1 = Enumerable.Range(1, 3)
            .Aggregate(
                ("Loop1", ImmutableList<string>.Empty),    // Initial state for Loop1
                (stateLoop1, idxLoop1) =>
                {
                    //Loop1 action
                    Console.WriteLine($"Loop1 iteration: {idxLoop1}");
                    
                    // Loop2
                    var resultLoop2 = Enumerable.Range(1, 3)
                        .Aggregate(
                            ("Loop2", ImmutableList<string>.Empty), // Initial state for Loop2
                            (stateLoop2, idxLoop2) =>
                            {
                                // Loop2 action
                                Console.WriteLine($"   Loop2 iteration: {idxLoop2}");

                                // Loop3
                                var resultLoop3 = Enumerable.Range(1, 3)
                                    .Aggregate(
                                        ("Loop3", ImmutableList<string>.Empty), // Initial state for Loop3
                                        (stateLoop3, idxLoop3) =>
                                        {
                                            // Loop3 action
                                            Console.WriteLine($"      Loop3 iteration: {idxLoop3}");

                                            // Update Loop3 state
                                            var newStateLoop3 = (stateLoop3.Item1, stateLoop3.Item2.Add($"{idxLoop1} - {idxLoop2} - {idxLoop3}"));
                                            return newStateLoop3;
                                        });

                                // Update Loop2 state
                                var newStateLoop2 = (stateLoop2.Item1, stateLoop2.Item2.AddRange(resultLoop3.Item2));
                                return newStateLoop2;
                            });
                    
                    //Update Loop1 state
                    var newStateLoop1 = (stateLoop1.Item1, stateLoop1.Item2.AddRange(resultLoop2.Item2));
                    return newStateLoop1;
                });
        
         Console.WriteLine($"\nAll results: {resultLoop1.Item1}\n{string.Join("\n", resultLoop1.Item2)}\n");
    }

    // Example 4b: Same logic using traditional for loops
    static void Example_NestedForLoops()
    {
        Console.WriteLine("--- Example 4b: For Loops Equivalent ---");
        
        // Loop1 - Initialize state
        var resultLoop1 = ("Loop1", ImmutableList<string>.Empty);
        
        for (int idxLoop1 = 1; idxLoop1 <= 3; idxLoop1++)
        {
            // Loop1 action
            Console.WriteLine($"Loop1 iteration: {idxLoop1}");
            
            // Loop2 - Initialize state
            var resultLoop2 = ("Loop2", ImmutableList<string>.Empty);
            
            for (int idxLoop2 = 1; idxLoop2 <= 3; idxLoop2++)
            {
                // Loop2 action
                Console.WriteLine($"   Loop2 iteration: {idxLoop2}");
                
                // Loop3 - Initialize state
                var resultLoop3 = ("Loop3", ImmutableList<string>.Empty);
                
                for (int idxLoop3 = 1; idxLoop3 <= 3; idxLoop3++)
                {
                    // Loop3 action
                    Console.WriteLine($"      Loop3 iteration: {idxLoop3}");
                    
                    // Update Loop3 state
                    resultLoop3 = (resultLoop3.Item1, resultLoop3.Item2.Add($"{idxLoop1} - {idxLoop2} - {idxLoop3}"));
                }
                
                // Update Loop2 state
                resultLoop2 = (resultLoop2.Item1, resultLoop2.Item2.AddRange(resultLoop3.Item2));
            }
            
            // Update Loop1 state
            resultLoop1 = (resultLoop1.Item1, resultLoop1.Item2.AddRange(resultLoop2.Item2));
        }
        
        Console.WriteLine($"\nAll results: {resultLoop1.Item1}\n{string.Join("\n", resultLoop1.Item2)}\n");
    }
}
