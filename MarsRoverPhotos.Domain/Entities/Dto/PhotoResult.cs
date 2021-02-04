using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRoverPhotos.Domain.Entities.Dto
{
    public class PhotoResult
    {
        public byte[] PhotoStream { get; set; }
        public string FileName { get; set; }
    }
}
