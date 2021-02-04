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
        IList<string> AvailableDates();
    }
}
