using MarsRoverPhotos.Domain.Entities.Dto;
using MarsRoverPhotos.Domain.Interface;
using System;

namespace MarsRoverPhotos.Domain.Query
{
    public class GetPhoto : IQuery<PhotoResult>
    {
        public DateTime Earth_Date { get; set; }
        public string FileName { get; set; }
    }
}
