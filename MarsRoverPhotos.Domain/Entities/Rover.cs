using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRoverPhotos.Domain.Entities
{
    public class Rover
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Landing_Date { get; set; }
        public DateTime Launch_Date { get; set; }
        public string Status { get; set; }
    }
}
