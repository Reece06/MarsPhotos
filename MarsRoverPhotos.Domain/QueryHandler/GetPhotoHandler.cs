using MarsRoverPhotos.Domain.Entities.Dto;
using MarsRoverPhotos.Domain.Interface;
using MarsRoverPhotos.Domain.Query;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MarsRoverPhotos.Domain.QueryHandler
{
    public class GetPhotoHandler : IQueryHandlerAsync<GetPhoto, PhotoResult>
    {
        private readonly IFileService _fileService;

        public GetPhotoHandler(IFileService fileService)
        {
            _fileService = fileService;
        }

        public async Task<PhotoResult> HandleQuery(GetPhoto query)
        {
            if(query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if(string.IsNullOrEmpty(query.FileName))
            {
                throw new ArgumentException("File Name is Empty");
            }

            //filtering datetime from 2010 to present since  rover was launched 2011
            if (query.Earth_Date > DateTime.Now || query.Earth_Date < new DateTime(2010, 1, 1))
            {
                throw new ArgumentException("Date must be from 2010 - Present");
            }

            var stream = await _fileService.GetPhotoStream(query.Earth_Date, query.FileName);
            if (stream == null)
            {
                throw new FileNotFoundException("File does not exist");
            }

            var result = new PhotoResult
            {
                PhotoStream = stream,
                FileName = query.FileName
            };

            return result;
        }
    }
}
