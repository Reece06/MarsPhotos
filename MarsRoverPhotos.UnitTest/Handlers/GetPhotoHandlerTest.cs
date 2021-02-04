using MarsRoverPhotos.Domain.Entities.Dto;
using MarsRoverPhotos.Domain.Interface;
using MarsRoverPhotos.Domain.Query;
using MarsRoverPhotos.Domain.QueryHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MarsRoverPhotos.UnitTest.Handlers
{
    [TestClass]
    public class GetPhotoHandlerTest
    {
        private IQueryHandlerAsync<GetPhoto, PhotoResult> _handler;
        private Mock<IFileService> fileService;

        [TestInitialize]
        public void InitializeTest()
        {
            fileService = new Mock<IFileService>();
            _handler = new GetPhotoHandler(fileService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetPhoto_NullParam()
        {
            _ = await _handler.HandleQuery(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetPhoto_EmptyFileName()
        {
            var query = new GetPhoto();
            _ = await _handler.HandleQuery(query);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetPhoto_DateMin()
        {
            var query = new GetPhoto { FileName = "test.jpg"};
            _ = await _handler.HandleQuery(query);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public async Task GetPhoto_FileNotFound()
        {
            fileService.Setup(x => x.GetPhotoStream(It.IsAny<DateTime>(), It.IsAny<string>())).ReturnsAsync(null as byte[]).Verifiable();
            var query = new GetPhoto { FileName = "test.jpg", Earth_Date = new DateTime(2015, 6, 3) };
            _ = await _handler.HandleQuery(query);
            fileService.Verify();
        }

        [TestMethod]
        public async Task GetPhoto_Success()
        {
            fileService.Setup(x => x.GetPhotoStream(It.IsAny<DateTime>(), It.IsAny<string>())).ReturnsAsync(new byte[] { 32, 23 }).Verifiable();
            var query = new GetPhoto { FileName = "test.jpg", Earth_Date = new DateTime(2015,6,3) };
            var result = await _handler.HandleQuery(query);

            Assert.IsNotNull(result);
            fileService.Verify();
        }

    }
}
