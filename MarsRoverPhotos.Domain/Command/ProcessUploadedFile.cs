using MarsRoverPhotos.Domain.Entities.Dto;
using MarsRoverPhotos.Domain.Interface;
using Microsoft.AspNetCore.Http;

namespace MarsRoverPhotos.Domain.Command
{
    public class ProcessUploadedFile : ICommand<UploadFileResult>
    {
        public IFormFile UploadFile { get; set; }
    }
}
