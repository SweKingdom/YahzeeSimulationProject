using System.Collections.Immutable;
using System.Diagnostics;

namespace Playground.Lesson11.Exercises;

public static class AsyncExercise2
{
    public static async Task RunExercisesAsync()
    {
        Console.WriteLine("=== Async Exercises ===\n");

        await Exercise2_TaskErrorHandlingAsync();
    }

    static async Task Exercise2_TaskErrorHandlingAsync()
    {
        List<string> results = new List<string>();
        List<Task<string>> tasks = new List<Task<string>>();
        try
        {
            Console.WriteLine("Syncron calls");
            results.Add(SayHello("Good Morning", 10, 1000, false));
            results.Add(SayHello("Good Afternoon", 5, 2000, true));
            results.Add(SayHello("Good Evening", 15, 500, false));

            //Ex3 - make the calls to SayHelloAsync
            Console.WriteLine("\n\nAsyncron calls");

            //Ex3 - Wait for all tasks to complete
        }
        catch (Exception ex)
        {
            //Your code
            Console.WriteLine(ex.Message);
            foreach (var item in tasks)
            {
                Console.WriteLine(item.IsFaulted);
            }
        }
        finally
        {
            Console.WriteLine("Main terminated");

            Console.WriteLine("Syncron calls results");
            foreach (var result in results)
            {
                Console.WriteLine(result);
            }

            Console.WriteLine("Asyncron calls results");
            foreach (var item in tasks)
            {
                if (item.IsCompletedSuccessfully)
                Console.WriteLine(item.Result);
            }
        }
    }

    //Ex2 - complete below async declaration of SayHello 
    //static public Task<string> SayHelloAsync(string message, int iterations, int msDelay, bool causeError = false) => ....
    static public string SayHello(string message, int iterations, int msDelay, bool causeError = false)
    {
        var rnd = new Random();
        int errorIteration = rnd.Next(0, iterations);
        for (int i = 0; i< iterations; i++)
        {
            Console.WriteLine($"{i,4}:{message}");
            Task.Delay(msDelay);
            
            if (causeError && (i == errorIteration))
            {
                throw new Exception($"Error saying: {message}");
            }
        }
        return $"All good saying: {message}";
    }

    //Exercise:
    //1. Run above code and set causeError flag to true for SayHello calls Good Morning, Good Afternoon, Good Evening
    //   - discuss and understand the Error handling
    //   - set all causeError to false to continue to Ex2

    //2. Make SayHello asyncronous by having it run in it's own Task
    //   - finish the declaration above

    //3. Call SayHelloAsync for Good Morning, Good Afternoon, Good Evening
    //   - let them run as parallell tasks and wait for all to finish
    //   - cause some errors in all three SayHelloAsync and see how the errorhandling is processed
}
