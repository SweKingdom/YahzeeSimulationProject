using System.Collections.Immutable;
using Models.Dto;
using Models.Music;
using Models.OpenWeather;
using Newtonsoft.Json;
using PlayGround.Extensions;
using Seido.Utilities.SeedGenerator;

namespace Playground.Lesson11.Examples;

public static class IO_Isolation
{
    public static void RunExamples()
    {
        Console.WriteLine("=== IO Isolation Examples ===\n");
    
        // Lets create separate files containing music groups
        // new SeedGenerator().ItemsToList<MusicGroup>(50).SerializeJson("musicGroups1.json");
        // new SeedGenerator().ItemsToList<MusicGroup>(500).SerializeJson("musicGroups2.json");
        // new SeedGenerator().ItemsToList<MusicGroup>(5000).SerializeJson("musicGroups3.json");
    
        ExampleMusicGroups();
    }

    private static void ExampleMusicGroups()
    {   
        // Example of how to generate random data to test the code without having to change the code that processes the data, just by changing the HOF
        var testHOF = () => new SeedGenerator().ItemsToList<MusicGroup>(50);

        // Example of how to read data from 3 different files without changing the code that processes the data, just by changing the HOF
        var HOF1 = () => new List<MusicGroup>().DeSerializeJson("musicGroups1.json");
        var HOF2 = () => new List<MusicGroup>().DeSerializeJson("musicGroups2.json");
        var HOF3 = () => new List<MusicGroup>().DeSerializeJson("musicGroups3.json");

        //Example of how to call the Web API directly from the HOF (not recommended, but shows how we can isolate the IO code in one place)
        var HOF4 = () => {
            string uri = $"musicgroups/read?seeded={true}&flat={false}&filter={""}&pagenr={0}&pagesize={50}";
            var _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://music.api.public.seido.se/api/")
            };

            //Send the HTTP Message and await the response
            HttpResponseMessage response = _httpClient.GetAsync(uri).Result;

            //Throw an exception if the response is not successful
            response.EnsureSuccessStatusCode();

            //Get the resonse data
            string s = response.Content.ReadAsStringAsync().Result;
            var resp = JsonConvert.DeserializeObject<ResponsePageDto<MusicGroup>>(s);
            return resp.PageItems;
        };



        // From Lesson02/Exercises/HomeExercise02Answers.cs        
        // Q7: Find artists who appear in Jazz groups with albums that sold over 500,000 copies
        System.Console.WriteLine("\nArtists in successful Jazz groups:");
        

        var successfulJazzArtists = IO_IsolationHOF.ReadMusicGroups(HOF4)
            .Where(g => g.Genre == MusicGenre.Jazz)
            .Where(g => g.Albums.Any(a => a.CopiesSold > 500_000))
            .SelectMany(g => g.Artists)
            .Distinct()
            .Select(a => (a.FirstName, a.LastName, a.BirthDay));
        successfulJazzArtists.ToList().ForEach(a => System.Console.WriteLine(a));
        
        // Q8: For each genre, find the music group with the most albums and show album count
        System.Console.WriteLine("\nQ8: Most prolific group per genre:");
        var mostProlificByGenre = IO_IsolationHOF.ReadMusicGroups(HOF4)
            .GroupBy(g => g.Genre)
            .Select(grp => 
                grp.OrderByDescending(g => g.Albums.Count).First()
            )
            .Select(g => (g.Genre, g.Name, AlbumCount: g.Albums.Count));
        mostProlificByGenre.ToList().ForEach(g => System.Console.WriteLine(g));
   }


    public static class IO_IsolationHOF
    {
        public static IEnumerable<MusicGroup> ReadMusicGroups(Func <IEnumerable<MusicGroup>> readFunc)
        {
            return readFunc();
        }   
    }
}
