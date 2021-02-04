using MarsRoverPhotos.Domain.Interface;
using Microsoft.AspNetCore.Http;

namespace MarsRoverPhotos.Domain.Command
{
    public class ProcessUploadedFile : ICommand<int>
    {
        public IFormFile UploadFile { get; set; }
    }
}
