namespace Playground.Lesson11;
public static class Main11
{
    public static void Entry(string[] args = null)
    {
        System.Console.WriteLine("Hello Lesson 11!");
        Examples.IO_Isolation.RunExamples();
    }
    public static async Task EntryAsync(string[] args = null)
    {
        System.Console.WriteLine("Hello Lesson 11 Async!");
        await Exercises.AsyncExercise1.RunExercisesAsync();
        await Exercises.AsyncExercise2.RunExercisesAsync();

        await Examples.WebApiAccess.RunExamples();
        await Examples.IO_IsolationAsync.RunExamplesAsync();

        await Exercises.IO_IsolationExercise.RunExerciseAsync();
    }
}
