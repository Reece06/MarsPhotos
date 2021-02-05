using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarsRoverPhotos.Domain.Entities;
using MarsRoverPhotos.Domain.Entities.Dto;
using MarsRoverPhotos.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace MarsRoverPhotos.Pages
{
    public class PhotosModel : PageModel
    {
        private readonly MarsRoverService service;
        public DateListResult result;
        public PhotoList photoList;

        public PhotosModel( MarsRoverService service)
        {
            this.service = service;
        }

        public async Task OnGetAsync()
        {
            result = await service.GetDateAndFiles();
        }

        public async Task<byte[]> GetPhotoByte(string date, string fileName)
        {
            var query = new Domain.Query.GetPhoto { Earth_Date = DateTime.Parse(date), FileName = fileName };
            var response = await service.GetPhotoByName(query);
            return response.PhotoStream;
        }

        public void SetDateString(PhotoList photoList)
        {
            this.photoList = photoList;
        }
    }
}
