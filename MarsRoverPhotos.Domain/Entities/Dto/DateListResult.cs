using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRoverPhotos.Domain.Entities.Dto
{
    public class DateListResult
    {
        public List<PhotoList> DateList { get; set; }
    }

    public class PhotoList
    {
        public string DateString { get; set; }
        public List<string> Photos { get; set; }
    }
}
