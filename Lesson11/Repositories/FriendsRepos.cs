using System.Runtime.InteropServices.Marshalling;
using Models.Dto;
using Models.Friends;
using Newtonsoft.Json;
using PlayGround.Extensions;

namespace Playground.Lesson11.Repositories;
public class FriendsRepos
{
    private HttpClient _httpClient = null;

    public FriendsRepos()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://friends.api.public.seido.se/api/")
        };
    }

    public async Task<ResponsePageDto<Friend>> ReadFriendsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize) 
    {
        string uri = $"friends/read?seeded={seeded}&flat={flat}&filter={filter}&pagenr={pageNumber}&pagesize={pageSize}";

        //Send the HTTP Message and await the response
        HttpResponseMessage response = await _httpClient.GetAsync(uri);

        //Throw an exception if the response is not successful
        await response.EnsureSuccessStatusMessage();

        //Get the resonse data
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ResponsePageDto<Friend>>(s);
        return resp;
    }
    public async Task<ResponseItemDto<Friend>> ReadFriendAsync(Guid id, bool flat)
    {
        string uri = $"friends/readitem?id={id}&flat={flat}";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.GetAsync(uri);

        //Throw an exception if the response is not successful
        await response.EnsureSuccessStatusMessage();

        //Get the response body
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ResponseItemDto<Friend>>(s);
        return resp;
    }
    public async Task<ResponseItemDto<Friend>> DeleteFriendAsync(Guid id)
    {
        string uri = $"friends/deleteitem/{id}";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.DeleteAsync(uri);

        //Throw an exception if the response is not successful
        await response.EnsureSuccessStatusMessage();

        //Get the response body
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ResponseItemDto<Friend>>(s);
        return resp;
    }
}

