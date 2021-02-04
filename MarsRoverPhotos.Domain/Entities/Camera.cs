using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRoverPhotos.Domain.Entities
{
    public class Camera
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Rover_Id { get; set; }
        public string Full_Name { get; set; }
    }
}
