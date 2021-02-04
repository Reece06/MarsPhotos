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

            var datePath = $"{path}\\{photo.Earth_Date:yyyy-MM-dd}\\";
            var completePath = Path.Combine(datePath, $"{photo.Camera.Name}-{photo.Id}.JPG");

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, photo.Img_Src);
            var response = await _httpClient.SendAsync(requestMessage);
            if (!response.IsSuccessStatusCode)
            {
                throw new FileNotFoundException("File path not found!");
            }

            if (File.Exists(completePath))
            {
                return;
            }
            var imageBytes = await _httpClient.GetByteArrayAsync(uri);

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

        public async Task<byte[]> GetPhotoStream(DateTime date, string fileName)
        {
            var filePath = $"{path}\\{date:yyyy-MM-dd}\\{fileName}";
            if (!File.Exists(filePath))
            {
                return null;
            }

            return await File.ReadAllBytesAsync(filePath);
        }

        public IList<string> AvailableDates()
        {
            if (!Directory.Exists(path))
            {
                return null;
            }
            List<string> dates = Directory.GetDirectories(path)
                .Select(x => x.Split('\\').LastOrDefault())
                .OrderBy(x => x).ToList();

            return dates;
        }

        public Domain.Entities.Dto.PhotoList AvailablePhotos(string datePath)
        {
            var fullPath = $"{ path}\\{ datePath}";
            if (!Directory.Exists(fullPath))
            {
                return null;
            }

            var photoList = new Domain.Entities.Dto.PhotoList
            {
                Photos = Directory.GetFiles(fullPath)
                    .Select(x => x.Split('\\').LastOrDefault())
                    .OrderBy(x => x).ToList(),
                DateString = datePath
            };

            return photoList;
        }
    }
}
