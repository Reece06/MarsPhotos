using MarsRoverPhotos.Domain.Command;
using MarsRoverPhotos.Domain.CommandHandler;
using MarsRoverPhotos.Domain.Entities;
using MarsRoverPhotos.Domain.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRoverPhotos.UnitTest.Handlers
{
    [TestClass]
    public class ProcessUploadFileHandlerTest
    {
        private ICommandHandler<ProcessUploadedFile, int> _handler;
        private Mock<IFileService> fileService;
        private Mock<INasaService> nasaService;

        [TestInitialize]
        public void InitializeTest()
        {
            fileService = new Mock<IFileService>();
            nasaService = new Mock<INasaService>();
            _handler = new ProcessUploadFileHandler(fileService.Object, nasaService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task UploadFile_NullParam()
        {
            _ = await _handler.HandleProcess(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task UploadFile_NoFileTypeMatch()
        {
            var file = new Mock<IFormFile>();

            file.Setup(x => x.FileName).Returns("sample.docx");

            var command = new ProcessUploadedFile
            {
                UploadFile = file.Object
            };
            _ = await _handler.HandleProcess(command);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task UploadFile_NoDatesInFile()
        {
            var file = new Mock<IFormFile>();

            file.Setup(x => x.FileName).Returns("sample.txt");
            fileService.Setup(x => x.ReadFileDates(It.IsAny<IFormFile>())).ReturnsAsync(new List<DateTime>());

            var command = new ProcessUploadedFile
            {
                UploadFile = file.Object
            };
            _ = await _handler.HandleProcess(command);
        }

        [TestMethod]
        public async Task UploadFile_Success_NoFilesToDownload()
        {
            var file = new Mock<IFormFile>();
            var listDate = new List<DateTime>() { DateTime.Now };

            file.Setup(x => x.FileName).Returns("sample.txt").Verifiable();
            fileService.Setup(x => x.ReadFileDates(It.IsAny<IFormFile>())).ReturnsAsync(listDate).Verifiable();
            nasaService.Setup(x => x.GetRoverPhotosByDate(listDate.FirstOrDefault())).ReturnsAsync(new PhotosList() { Photos = new List<Photo>() }).Verifiable();


            var command = new ProcessUploadedFile
            {
                UploadFile = file.Object
            };
            var result = await _handler.HandleProcess(command);

            fileService.Verify();
            nasaService.Verify();
            file.Verify();
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public async Task UploadFile_Success_FilesDownloaded()
        {
            var file = new Mock<IFormFile>();
            var listDate = new List<DateTime>() { new DateTime(2015, 06, 03) };
            var photoList = new PhotosList()
            {
                Photos = new List<Photo>()
                    {
                        new Photo
                        {
                            Id = 10121,
                            Earth_Date = new DateTime(2015, 06, 03),
                            Camera = new Camera
                            {
                                Name = "FHAZ"
                            },
                            Img_Src = "http://mars.jpl.nasa.gov/msl-raw-images/proj/msl/redops/ods/surface/sol/01004/opgs/edr/fcam/FLB_486615455EDR_F0481570FHAZ00323M_.JPG"

                        }
                    }
            };

            file.Setup(x => x.FileName).Returns("sample.txt").Verifiable();
            fileService.Setup(x => x.ReadFileDates(It.IsAny<IFormFile>())).ReturnsAsync(listDate).Verifiable();
            nasaService.Setup(x => x.GetRoverPhotosByDate(listDate.FirstOrDefault()))
                .ReturnsAsync(photoList).Verifiable();
            fileService.Setup(x => x.SaveImageToPath(It.IsAny<Photo>())).Verifiable();


            var command = new ProcessUploadedFile
            {
                UploadFile = file.Object
            };
            var result = await _handler.HandleProcess(command);

            fileService.Verify();
            nasaService.Verify();
            file.Verify();
            Assert.AreEqual(1, result);
        }


    }
}
