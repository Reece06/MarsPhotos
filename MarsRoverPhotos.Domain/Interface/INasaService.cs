using MarsRoverPhotos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarsRoverPhotos.Domain.Interface
{
    public interface INasaService
    {
        Task<PhotosList> GetRoverPhotosByDate(DateTime date);
    }
}
