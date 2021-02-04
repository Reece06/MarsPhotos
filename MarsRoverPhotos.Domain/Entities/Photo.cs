using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRoverPhotos.Domain.Entities
{
    public class Photo
    {
        public int Id { get; set; }
        public int Sol { get; set; }
        public Camera Camera { get; set; }
        public string Img_Src { get; set; }
        public DateTime Earth_Date { get; set; }
        public Rover Rover { get; set; }
    }

    public class PhotosList
    {
        public IList<Photo> Photos { get; set; }
    }
}
