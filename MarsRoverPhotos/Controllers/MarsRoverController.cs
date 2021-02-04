using MarsRoverPhotos.Domain.Command;
using MarsRoverPhotos.Domain.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace MarsRoverPhotos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarsRoverController : ControllerBase
    {

        private readonly ILogger<MarsRoverController> _logger;
        private ICommandHandler<ProcessUploadedFile,int> _processUploadHandler;

        public MarsRoverController(ILogger<MarsRoverController> logger, ICommandHandler<ProcessUploadedFile, int> processUploadHandler)
        {
            _logger = logger;
            _processUploadHandler = processUploadHandler;
        }

        [HttpPost("uploadFile")]

        public async Task<int> UploadTextFile()
        {
            var files = Request.Form.Files;
            var processUpload = new ProcessUploadedFile
            {
                UploadFile = files.FirstOrDefault()
            };

            var result = await _processUploadHandler.HandleProcess(processUpload);
            return result;
        }
    }
}
