using MarsRoverPhotos.Domain.Entities.Dto;
using MarsRoverPhotos.Domain.Interface;
using MarsRoverPhotos.Domain.Query;
using MarsRoverPhotos.Domain.QueryHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace MarsRoverPhotos.UnitTest.Handlers
{
    [TestClass]
    public class GetDateListAndFilesTest
    {

        private IQueryHandler<GetDateListAndFiles, DateListResult> _handler;
        private Mock<IFileService> fileService;
        [TestInitialize]
        public void TestInitialize()
        {

            fileService = new Mock<IFileService>();
            _handler = new GetDateListAndFilesHandler(fileService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetDateList_NullArgument()
        {
            _ = _handler.HandleQuery(null);
        }

        [TestMethod]
        public void GetDateList_EmptyResult()
        {
            fileService.Setup(x => x.AvailableDates()).Returns(new List<string>()).Verifiable();
            var query = new GetDateListAndFiles();

            var result = _handler.HandleQuery(query);

            Assert.IsNull(result);
            fileService.Verify();
        }

        [TestMethod]
        public void GetDateList_EmptyResultNull()
        {
            fileService.Setup(x => x.AvailableDates()).Returns(null as IList<string>).Verifiable();
            var query = new GetDateListAndFiles();

            var result = _handler.HandleQuery(query);

            Assert.IsNull(result);
            fileService.Verify();
        }

        [TestMethod]
        public void GetDateList_WithPhotos()
        {
            var listPhoto = new List<string>() { "2016-6-3" };
            fileService.Setup(x => x.AvailableDates()).Returns(listPhoto).Verifiable();
            fileService.Setup(x => x.AvailablePhotos(It.IsAny<string>())).Returns(new PhotoList { DateString = "2016-6-3" , Photos = new List<string>() { "test"} });
            
            var result = _handler.HandleQuery(new GetDateListAndFiles());

            Assert.IsNotNull(result);
            Assert.IsTrue(result.DateList.Count > 0);
            fileService.Verify();
        }

        [TestMethod]
        public void GetDateList_WithoutPhotos()
        {
            var listPhoto = new List<string>() { "2016-6-3" };
            fileService.Setup(x => x.AvailableDates()).Returns(listPhoto).Verifiable();
            fileService.Setup(x => x.AvailablePhotos(It.IsAny<string>())).Returns(new PhotoList { DateString = "2016-6-3", Photos = new List<string>() });

            var result = _handler.HandleQuery(new GetDateListAndFiles());

            Assert.IsNotNull(result);
            Assert.IsTrue(result.DateList.Count == 0);
            fileService.Verify();
        }
    }
}
