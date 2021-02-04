using MarsRoverPhotos.Domain.Command;
using MarsRoverPhotos.Domain.Entities.Dto;
using MarsRoverPhotos.Domain.Interface;
using MarsRoverPhotos.Domain.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MarsRoverPhotos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarsRoverController : ControllerBase
    {

        private readonly ICommandHandler<ProcessUploadedFile, UploadFileResult> _processUploadHandler;
        private readonly IQueryHandlerAsync<GetPhoto, PhotoResult> _getPhotoHandler;
        private readonly IQueryHandler<GetDateListAndFiles, DateListResult> _getDateListAndFilesHandler;

        public MarsRoverController(
            ICommandHandler<ProcessUploadedFile, UploadFileResult> processUploa
            ,IQueryHandlerAsync<GetPhoto, PhotoResult> getPhoto
            ,IQueryHandler<GetDateListAndFiles, DateListResult> _getDateListAndFiles)
        {
            _processUploadHandler = processUploa;
            _getPhotoHandler = getPhoto;
            _getDateListAndFilesHandler = _getDateListAndFiles;
        }

        [HttpPost("uploadFile")]
        [Produces("application/json")]
        public async Task<IActionResult> UploadTextFile()
        {
            try
            {
                var files = Request.Form.Files;
                var processUpload = new ProcessUploadedFile
                {
                    UploadFile = files.FirstOrDefault()
                };

                var result = await _processUploadHandler.HandleProcess(processUpload);
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }

        [HttpGet("getbydate")]
        public async Task<IActionResult> GetPhotoByName([FromQuery]GetPhoto getPhoto)
        {
            try
            {
                var result = await _getPhotoHandler.HandleQuery(getPhoto);
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }

        [HttpGet("getavailablefiles")]
        public IActionResult GetDateAndFiles()
        {
            try
            {
                var result = _getDateListAndFilesHandler.HandleQuery(new GetDateListAndFiles());
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }
    }
}
