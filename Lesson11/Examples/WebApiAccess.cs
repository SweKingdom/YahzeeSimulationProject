using PlayGround.Extensions;

namespace Playground.Lesson11.Examples;

public static class WebApiAccess
{
    public static async Task RunExamples()
    {
        Console.WriteLine("=== Web API Access Examples ===\n");
        await ExampleMusicWebApiAsync();
        Console.WriteLine("\n---\n");
        await ExampleFriendsWebApiAsync();
        Console.WriteLine("\n---\n");

        //Uncomment this line after adding your OpenWeather API key in OpenWeatherRepos.cs
        //await ExampleOpenWeatherWebApiAsync(); 
    }

    private static async Task ExampleMusicWebApiAsync()
    {
        var repos = new Repositories.MusicGroupRepos();
        var musicGroups = await repos.ReadMusicGroupsAsync(true, true, "", 1, 10);
        // musicGroups = await repos.ReadMusicGroupsAsync(true, false, "", 0, musicGroups.DbItemsCount);
        Console.WriteLine($"Read {musicGroups.PageItems.Count} music groups");

        var musicGroup = repos.ReadMusicGroupAsync(musicGroups.PageItems[0].MusicGroupId, false).Result;
        Console.WriteLine($"Read music group: {musicGroup.Item.Name}");

        var artists = musicGroup.Item.Artists;
        Console.WriteLine($"Music group has {artists.Count} artists");
        System.Console.WriteLine($"Artists:\n{string.Join("\n", artists.Select(a => $"{a.FirstName} {a.LastName}"))}");        

        var albums = musicGroup.Item.Albums;
        Console.WriteLine($"Music group has {albums.Count} albums");
        System.Console.WriteLine($"Albums:\n{string.Join("\n", albums.Select(a => a.Name))}");  
    }

    private static async Task ExampleFriendsWebApiAsync()
    {
        var repos = new Repositories.FriendsRepos();
        var friends = await repos.ReadFriendsAsync(true, true, "", 1, 10);
        // friends = await repos.ReadFriendsAsync(true, false, "", 0, friends.DbItemsCount);
        Console.WriteLine($"Read {friends.PageItems.Count} friends");

        var friend = await repos.ReadFriendAsync(friends.PageItems[0].FriendId, false);
        Console.WriteLine($"Read friend: {friend.Item.FirstName} {friend.Item.LastName}");

        var pets = friend.Item.Pets;
        Console.WriteLine($"Friend has {pets.Count} pets");
        System.Console.WriteLine($"Pets:\n{string.Join("\n", pets.Select(p => $"{p.Name} ({p.Kind})"))}");        

        var quotes = friend.Item.Quotes;
        Console.WriteLine($"Friend likes {quotes.Count} quotes");
        System.Console.WriteLine($"Quotes:\n{string.Join("\n", quotes.Select(q => $"{q.QuoteText} - {q.Author}"))}");  
    }


    private static async Task ExampleOpenWeatherWebApiAsync()
    {
        var repos = new Repositories.OpenWeatherRepos();
        var forecast = await repos.GetForecastAsync("Singapore");
        Console.WriteLine($"Weather forecast for {forecast.City}:");
        foreach (var item in forecast.Items)
        {
            Console.WriteLine($"{item.DateTime}: {item.Description}, {item.Temperature}°C, {item.WindSpeed} m/s");
        }

        //Remeber the extension method to serialize to json? Let's save the forecast to a file, to later use it for IO isolation:
        //forecast.SerializeJson("singaporeForecast.json");
    }
}
