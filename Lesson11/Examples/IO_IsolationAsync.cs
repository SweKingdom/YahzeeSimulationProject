using System.Collections.Immutable;
using Models.Music;
using Models.OpenWeather;
using PlayGround.Extensions;
using Seido.Utilities.SeedGenerator;

namespace Playground.Lesson11.Examples;

public static class IO_IsolationAsync
{
    public static async Task RunExamplesAsync()
    {
        Console.WriteLine("=== IO Isolation Examples ===\n");
        await ExampleMusicGroupsAsync();
        await ExampleOpenWeatherAsync("Copenhagen"); 
    }

    private static async Task ExampleMusicGroupsAsync()
    {   
        var testHOF = () => Task.FromResult<IEnumerable<MusicGroup>>(new SeedGenerator().ItemsToList<MusicGroup>(50));
        var fileHOF = () => Task.FromResult<IEnumerable<MusicGroup>>(new List<MusicGroup>().DeSerializeJson("musicGroups3.json"));
        var webApiHOF = IO_WebApiAccessHOFAsync.ReadMusicGroupsAsync;

        // From Lesson02/Exercises/HomeExercise02Answers.cs        
        // Q7: Find artists who appear in Jazz groups with albums that sold over 500,000 copies
        System.Console.WriteLine("\nArtists in successful Jazz groups:");

        var successfulJazzArtists = (await IO_IsolationHOFAsync.ReadMusicGroups(testHOF))
            .Where(g => g.Genre == MusicGenre.Jazz)
            .Where(g => g.Albums.Any(a => a.CopiesSold > 500_000))
            .SelectMany(g => g.Artists)
            .Distinct()
            .Select(a => (a.FirstName, a.LastName, a.BirthDay));
        successfulJazzArtists.ToList().ForEach(a => System.Console.WriteLine(a));
        
        // Q8: For each genre, find the music group with the most albums and show album count
        System.Console.WriteLine("\nQ8: Most prolific group per genre:");
        var mostProlificByGenre = (await IO_IsolationHOFAsync.ReadMusicGroups(testHOF))
            .GroupBy(g => g.Genre)
            .Select(grp => 
                grp.OrderByDescending(g => g.Albums.Count).First()
            )
            .Select(g => (g.Genre, g.Name, AlbumCount: g.Albums.Count));
        mostProlificByGenre.ToList().ForEach(g => System.Console.WriteLine(g));
   }

    private static async Task ExampleOpenWeatherAsync(string city)
    {
        var testHOF = (string c) => Task.FromResult(new Forecast().Seed(new SeedGenerator()) with { City = c });
        var fileHOF = (string c) => Task.FromResult<Forecast>(new Forecast().DeSerializeJson("singaporeForecast.json"));
        var webApiHOF = IO_WebApiAccessHOFAsync.ReadWeatherForecastAsync;

        var forecast = await IO_IsolationHOFAsync.ReadWeatherForecastAsync(city, fileHOF);          

        Console.WriteLine($"\nWeather forecast for {forecast.City}:");
        foreach (var item in forecast.Items)
        {
            Console.WriteLine($"{item.DateTime}: {item.Description}, {item.Temperature}°C, {item.WindSpeed} m/s");
        }
    }

    public static class IO_IsolationHOFAsync
    {
        public static async Task<IEnumerable<MusicGroup>> ReadMusicGroups(Func <Task<IEnumerable<MusicGroup>>> readFunc)
        {
            return await readFunc();
        }   
        public static async Task<Forecast> ReadWeatherForecastAsync(string city, Func <string, Task<Forecast>> readFunc)
        {
            return await readFunc(city);
        }   
    }

    public static class IO_WebApiAccessHOFAsync
    {
        public static async Task<IEnumerable<MusicGroup>> ReadMusicGroupsAsync()
        {
            var repos = new Repositories.MusicGroupRepos();
            var musicGroups = await repos.ReadMusicGroupsAsync(true, true, "", 1, 10);
            musicGroups = await repos.ReadMusicGroupsAsync(true, false, "", 0, musicGroups.DbItemsCount);
            return musicGroups.PageItems;
        }

        public static async Task<Forecast> ReadWeatherForecastAsync(string city)
        {
            var repos = new Repositories.OpenWeatherRepos();    
            var forecast = await repos.GetForecastAsync(city);
            return forecast;
        }
    }
}
