using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Client_For_Messenger.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenStore _tokenStore;

        public ApiService(HttpClient httpClient, TokenStore tokenStore)
        {
            _httpClient = httpClient;
            _tokenStore = tokenStore;
        }

        private async Task AddAuthorizationHeader()
        {
            var storedToken = await _tokenStore.GetAsync();

            _httpClient.DefaultRequestHeaders.Authorization = null;

            if (storedToken != null && !string.IsNullOrEmpty(storedToken.AccessToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", storedToken.AccessToken);
            }
        }

        public async Task<HttpResponseMessage> RequestGet(string EndPoint)
        {
            await AddAuthorizationHeader(); 
            return await _httpClient.GetAsync(EndPoint);
        }

        public async Task<HttpResponseMessage> RequestPost<T>(string EndPoint, T data)
        {
            await AddAuthorizationHeader();
            return await _httpClient.PostAsJsonAsync(EndPoint, data);
        }

    }
}
