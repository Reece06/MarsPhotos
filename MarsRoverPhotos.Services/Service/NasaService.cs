using MarsRoverPhotos.Domain.Entities;
using MarsRoverPhotos.Domain.Interface;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MarsRoverPhotos.Services.Service
{
    public class NasaService : INasaService
    {
        protected HttpClient _client;
        protected readonly string token;
        protected const string roverAPI = "rovers/curiosity/photos";

        public NasaService(string baseUrl, string token)
        {
            _client = new HttpClient { BaseAddress = new Uri(baseUrl) };
            this.token = token;
        }

        public async Task<PhotosList> GetRoverPhotosByDate(DateTime date)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{roverAPI}?earth_date={date:yyyy-MM-dd}&api_key={token}");
            var response = await _client.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            try
            {
                var stringContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PhotosList>(stringContent);

            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
