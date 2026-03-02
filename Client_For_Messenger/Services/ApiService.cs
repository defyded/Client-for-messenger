using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using static System.Net.WebRequestMethods;

namespace Client_For_Messenger.Services
{
    public class ApiService
    {
        private string Host = "https://localhost";
        private int Port = 7007;
        private HttpClient _hhtpClient;
       
        public ApiService()
        {
            _hhtpClient = new HttpClient() 
            { 
                BaseAddress = new Uri($"{Host}:{Port}")
            };
        }

        public async Task<HttpResponseMessage> RequestPost<T>(string EndPoint, T data) 
        {
            return await _hhtpClient.PostAsJsonAsync(EndPoint, data); //ToDo Остальные методы HTTP аналогично сделать
        }
    }
}
