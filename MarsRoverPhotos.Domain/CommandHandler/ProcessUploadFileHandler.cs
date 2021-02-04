using MarsRoverPhotos.Domain.Command;
using MarsRoverPhotos.Domain.Entities;
using MarsRoverPhotos.Domain.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRoverPhotos.Domain.CommandHandler
{
    public class ProcessUploadFileHandler : ICommandHandler<ProcessUploadedFile, int>
    {
        private readonly IFileService _fileService;
        private readonly INasaService _nasaService;
        private readonly string[] allowedeTypes = { ".txt" };

        public ProcessUploadFileHandler(IFileService fileService, INasaService nasaService)
        {
            _fileService = fileService;
            _nasaService = nasaService;
        }

        public async Task<int> HandleProcess(ProcessUploadedFile command)
        {
            var exceptionsList = new List<Exception>();
            if(command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (!allowedeTypes.Contains(Path.GetExtension(command.UploadFile.FileName), StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Only .txt files are allowed!");
            }

            var dates = await _fileService.ReadFileDates(command.UploadFile);

            if(dates.Count == 0)
            {
                throw new Exception("No Dates Found!");
            }

            var result = await ProcessDates(dates, exceptionsList);

            return result;
        }

        private async Task<int> ProcessDates(IList<DateTime> dates, IList<Exception> exceptionsList)
        {
            var processedDateCount = 0;
            foreach (var date in dates)
            {
                PhotosList photos;
                try
                {
                    photos = await _nasaService.GetRoverPhotosByDate(date);
                }
                catch (Exception ex)
                {
                    exceptionsList.Add(ex);
                    continue;
                }

                if (photos == null || photos.Photos.Count == 0)
                {
                    continue;
                }

                foreach (var photo in photos.Photos)
                {
                    try
                    {
                        await _fileService.SaveImageToPath(photo);
                    }
                    catch (Exception ex)
                    {
                        exceptionsList.Add(ex);
                        continue;
                    }
                }
                processedDateCount++;
            }

            return processedDateCount;
        }
    }
}
