using System.Collections.Immutable;
using System.Diagnostics;

namespace Playground.Lesson11.Exercises;

public static class AsyncExercise1
{
    public static async Task RunExercisesAsync()
    {
        Console.WriteLine("=== Async Exercises ===\n");

        await Exercise1_CreateTasksAsync();
    }

    static async Task Exercise1_CreateTasksAsync()
    {
        var watch = new Stopwatch();
        watch.Start();

        //Create Task t1
        //Your Code

        //Create Task t2
        //Your Code

        //Create Task t3
        //Your Code

        //await Task.WhenAll(t1, t2, t3);

        watch.Stop();
        Console.WriteLine($"Main terminated. Execution time: {watch.ElapsedMilliseconds}ms");
    }

    // Exercises
    //1. Create and start a Task t1 that loops 5 times and in each loop prints out "Hello{i} from Thread1" and sleeps 2 second
    //2. Create and start a Task t2 that loops 10 times and in each loop prints out "Hello{i} from Thread2" and sleeps 1 second
    //3. Create and start a Task t3 that loops 15 times and in each loop prints out "Hello{i} from Thread3" and sleeps 0,5 second
    //4. Change the order of execution using await t1; so that t2 and t3 starts after t1 has completed execution
    //5. Experiment by changig the await t1, await t2, and await t3 and see how execution times changes
}
