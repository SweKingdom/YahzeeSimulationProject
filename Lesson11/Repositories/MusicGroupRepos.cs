using System.Runtime.InteropServices.Marshalling;
using Models.Dto;
using Models.Music;
using Newtonsoft.Json;
using PlayGround.Extensions;

namespace Playground.Lesson11.Repositories;
public class MusicGroupRepos
{
    private HttpClient _httpClient = null;

    public MusicGroupRepos()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://music.api.public.seido.se/api/")
        };
    }

    public async Task<ResponsePageDto<MusicGroup>> ReadMusicGroupsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize) 
    {
        string uri = $"musicgroups/read?seeded={seeded}&flat={flat}&filter={filter}&pagenr={pageNumber}&pagesize={pageSize}";

        //Send the HTTP Message and await the response
        HttpResponseMessage response = await _httpClient.GetAsync(uri);

        //Throw an exception if the response is not successful
        await response.EnsureSuccessStatusMessage();

        //Get the resonse data
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ResponsePageDto<MusicGroup>>(s);
        return resp;
    }
    public async Task<ResponseItemDto<MusicGroup>> ReadMusicGroupAsync(Guid id, bool flat)
    {
        string uri = $"musicgroups/readitem?id={id}&flat={flat}";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.GetAsync(uri);

        //Throw an exception if the response is not successful
        await response.EnsureSuccessStatusMessage();

        //Get the response body
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ResponseItemDto<MusicGroup>>(s);
        return resp;
    }
    public async Task<ResponseItemDto<MusicGroup>> DeleteMusicGroupAsync(Guid id)
    {
        string uri = $"musicgroups/deleteitem/{id}";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.DeleteAsync(uri);

        //Throw an exception if the response is not successful
        await response.EnsureSuccessStatusMessage();

        //Get the response body
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ResponseItemDto<MusicGroup>>(s);
        return resp;
    }
}

