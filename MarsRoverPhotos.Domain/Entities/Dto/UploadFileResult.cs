using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRoverPhotos.Domain.Entities.Dto
{
    public class UploadFileResult
    {
        public int ProcessedDates { get; set; }
        public int UnprocessedDates { get; set; }
    }
}
