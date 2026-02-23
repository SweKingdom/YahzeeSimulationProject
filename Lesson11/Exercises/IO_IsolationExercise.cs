using System.Collections.Immutable;
using Models.Friends;
using PlayGround.Extensions;
using Seido.Utilities.SeedGenerator;

namespace Playground.Lesson11.Exercises;

public static class IO_IsolationExercise
{
    public static async Task RunExerciseAsync()
    {
        Console.WriteLine("=== IO Isolation Exercise ===\n");
        await ExerciseFriendsAsync();
    }

    private static async Task ExerciseFriendsAsync()
    {   
        //var testHOF = () => ...
        //var fileHOF = () => ...
        //var webApiHOF = ...

        // Show number of friends in each country, ordered by count descending 
        System.Console.WriteLine("\nNumber of friends in each country:");

        // Show number of pets owned by friends in each city, ordered by count descending
        System.Console.WriteLine("\nNumber of pets owned by friends in each city:");

        // Show most popular quote among friends, and how many friends like that quote
        System.Console.WriteLine("\nMost popular quote among friends:");   
   }

    public static class IO_WebApiAccessHOFAsync
    {
        public static async Task<IEnumerable<Friend>> ReadFriendsAsync()
        {
            var repos = new Repositories.FriendsRepos();
            var friends = await repos.ReadFriendsAsync(true, true, "", 1, 10);
            friends = await repos.ReadFriendsAsync(true, false, "", 0, friends.DbItemsCount);
            return friends.PageItems;
        }
    }
   // Exercises:
   // 1. Analyze Lesson11/Examples/IO_IsolationAsync.cs and implement the HOFs for the Friends model
   // 2. Use the testHOF to implement the three queries above
   // 3. Store a set of friends from the testHOF in a JSON file and implement the fileHOF to read from that file, then run the queries again using the fileHOF
   // 4. Use the IO_WebApiAccessHOFAsync to fetch friends data from the web API and run the queries again using the webApiHOF   

}
