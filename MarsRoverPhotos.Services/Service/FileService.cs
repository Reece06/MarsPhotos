using MarsRoverPhotos.Domain.Entities;
using MarsRoverPhotos.Domain.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MarsRoverPhotos.Services.Service
{
    public class FileService : IFileService
    {
        protected readonly string path;
        protected HttpClient _httpClient;
        public FileService(string path)
        {
            this.path = path;
            _httpClient = new HttpClient();
        }

        public async Task SaveImageToPath(Photo photo)
        {
            var uri = new Uri(photo.Img_Src);
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, photo.Img_Src);
            var response = await _httpClient.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                throw new FileNotFoundException("File path not found!");
            }
            var imageBytes = await _httpClient.GetByteArrayAsync(uri);

            // Create file path and ensure directory exists
            var datePath = $"{path}\\{photo.Earth_Date:yyyy-MM-dd}\\";
            var completePath = Path.Combine(datePath, $"{photo.Camera.Name}-{photo.Id}.JPG");
            Directory.CreateDirectory(datePath);

            await File.WriteAllBytesAsync(completePath, imageBytes);
        }

        public async Task<IList<DateTime>> ReadFileDates(IFormFile file)
        {
            var dateList = new List<DateTime>();
            using var read = new StreamReader(file.OpenReadStream());

            while (read.Peek() >= 0)
            {
                var line = await read.ReadLineAsync();
                
                if (DateTime.TryParse(line, out var date))
                {
                    dateList.Add(date);
                }
            }

            return dateList;
        }

        public IList<string> AvailableDates()
        {
            var dates = Directory.GetDirectories(path).ToList();

            return dates;
        }
    }
}
