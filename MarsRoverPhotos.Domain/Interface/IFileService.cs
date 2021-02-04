using MarsRoverPhotos.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarsRoverPhotos.Domain.Interface
{
    public interface IFileService
    {
        Task SaveImageToPath(Photo photo);
        Task<IList<DateTime>> ReadFileDates(IFormFile file);
        Task<byte[]> GetPhotoStream(DateTime date, string fileName);
        IList<string> AvailableDates();
        Entities.Dto.PhotoList AvailablePhotos(string datePath);
    }
}
