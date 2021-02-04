using MarsRoverPhotos.Domain.Interface;
using MarsRoverPhotos.Services.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MarsRoverPhotos.UnitTest.Services
{
    [TestClass]
    public class NasaServiceTests
    {
        private INasaService nasaService;

        [TestInitialize]
        public void Initialize()
        {
            nasaService = new NasaService("https://api.nasa.gov/mars-photos/api/v1/", "EcS17Zq2NlsWViMSXhgI1Ifx273Z1Ie6fBYAwOX8");
        }

        [TestMethod]
        public async  Task GetGetRoverPhotosByDate_NoResult()
        {
            var date = new DateTime(2021, 2, 4);
            var result = await nasaService.GetRoverPhotosByDate(date);

            Assert.AreEqual(0, result.Photos.Count);
        }

        [TestMethod]
        public async Task GetGetRoverPhotosByDate_WithResult()
        {
            var date = new DateTime(2015 ,6 , 3);
            var result = await nasaService.GetRoverPhotosByDate(date);

            Assert.IsTrue( result.Photos.Count > 0);
        }
    }
}
