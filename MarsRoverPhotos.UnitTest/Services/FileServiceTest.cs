using MarsRoverPhotos.Domain.Entities;
using MarsRoverPhotos.Domain.Interface;
using MarsRoverPhotos.Services.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MarsRoverPhotos.UnitTest.Services
{
    [TestClass]
    public class FileServiceTest
    {
        private IFileService fileService;

        [TestInitialize]
        public void Initialize()
        {
            fileService = new FileService("C:\\images\\rover");
        }

        [TestMethod]
        public async Task SaveFileFromSource_Success()
        {
            var photo = new Photo
            {
                Id = 10121,
                Earth_Date = new DateTime(2015, 06, 03),
                Camera = new Camera
                {
                    Name = "FHAZ"
                },
                Img_Src = "http://mars.jpl.nasa.gov/msl-raw-images/proj/msl/redops/ods/surface/sol/01004/opgs/edr/fcam/FLB_486615455EDR_F0481570FHAZ00323M_.JPG"

            };
            await fileService.SaveImageToPath(photo);

            var completePath = Path.Combine($"C:\\images\\rover\\{ photo.Earth_Date:yyyy-MM-dd}\\", $"{photo.Camera.Name}-{photo.Id}.JPG");
            Assert.IsTrue(File.Exists(completePath));
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public async Task SaveFileFromSource_NoResult()
        {
            var photo = new Photo
            {
                Id = 10121,
                Earth_Date = new DateTime(2015, 06, 03),
                Camera = new Camera
                {
                    Name = "FHAZ"
                },
                Img_Src = "http://mars.jpl.nasa.gov/msl-raw-images/proj/msl/redops/ods/surface/sol/01004/opgs/edr/fcam/FLB_486615455EDR_F0323M_.JPG"

            };
            await fileService.SaveImageToPath(photo);
        }

        [TestMethod]
        public async Task UploadFile()
        {
            var file = new Mock<IFormFile>();
            var path = Directory.GetCurrentDirectory();
            path = path.Trim().Replace("\\bin\\Debug\\netcoreapp3.1", "\\Files\\Sample.txt");

            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(File.OpenRead(path));
            writer.Flush();
            ms.Position = 0;

            file.Setup(_ => _.OpenReadStream())
                .Returns(ms)
                .Verifiable();

            var inputFile = file.Object;
            var output = await fileService.ReadFileDates(inputFile);

            file.Verify();
        }
    }
}
