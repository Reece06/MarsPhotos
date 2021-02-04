using MarsRoverPhotos.Domain.Entities.Dto;
using MarsRoverPhotos.Domain.Interface;
using MarsRoverPhotos.Domain.Query;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarsRoverPhotos.Domain.QueryHandler
{
    public class GetDateListAndFilesHandler : IQueryHandler<GetDateListAndFiles, DateListResult>
    {
        private readonly IFileService _fileService;
        public GetDateListAndFilesHandler(IFileService fileService)
        {
            _fileService = fileService;
        }
        public DateListResult HandleQuery(GetDateListAndFiles query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            var dateList = _fileService.AvailableDates();
            if(dateList == null || dateList.Count == 0)
            {
                return null;
            }

            var result = new DateListResult { 
                DateList = new List<PhotoList>()
            };

            foreach(var date in dateList)
            {
                var photo = _fileService.AvailablePhotos(date);
                if(photo == null || photo.Photos == null || photo.Photos.Count == 0)
                {
                    continue;
                }

                result.DateList.Add(photo);
            }

            return result;
        }
    }
}
