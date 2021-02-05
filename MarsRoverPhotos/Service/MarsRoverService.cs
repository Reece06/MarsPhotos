using MarsRoverPhotos.Domain.Command;
using MarsRoverPhotos.Domain.Entities.Dto;
using MarsRoverPhotos.Domain.Interface;
using MarsRoverPhotos.Domain.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarsRoverPhotos.Service
{
    //[ApiController]
    //[Route("api/[controller]")]
    public class MarsRoverService // : ControllerBase
    {

        private readonly ICommandHandler<ProcessUploadedFile, UploadFileResult> _processUploadHandler;
        private readonly IQueryHandlerAsync<GetPhoto, PhotoResult> _getPhotoHandler;
        private readonly IQueryHandler<GetDateListAndFiles, DateListResult> _getDateListAndFilesHandler;

        public MarsRoverService(
            ICommandHandler<ProcessUploadedFile, UploadFileResult> processUploa
            , IQueryHandlerAsync<GetPhoto, PhotoResult> getPhoto
            , IQueryHandler<GetDateListAndFiles, DateListResult> _getDateListAndFiles)
        {
            _processUploadHandler = processUploa;
            _getPhotoHandler = getPhoto;
            _getDateListAndFilesHandler = _getDateListAndFiles;
        }

        //[HttpPost("uploadFile")]
        public async Task<UploadFileResult> UploadTextFile(IFormFile file)
        {
            var processUpload = new ProcessUploadedFile
            {
                UploadFile = file
            };

            var result = await _processUploadHandler.HandleProcess(processUpload);
            return result;
        }

        //[HttpGet("getbydate")]
        public async Task<PhotoResult> GetPhotoByName([FromQuery]GetPhoto getPhoto)
        {
            var result = await _getPhotoHandler.HandleQuery(getPhoto);
            return result;
        }

        //[HttpGet("getavailablefiles")]
        public async Task<DateListResult> GetDateAndFiles()
        {
            var result = await Task.FromResult(_getDateListAndFilesHandler.HandleQuery(new GetDateListAndFiles()));
            return result;
        }
    }
}
