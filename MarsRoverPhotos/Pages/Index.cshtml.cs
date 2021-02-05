using MarsRoverPhotos.Domain.Entities.Dto;
using MarsRoverPhotos.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MarsRoverPhotos.Pages
{
    public class IndexModel : PageModel
    {
        private readonly MarsRoverService service;
        [BindProperty]
        public IFormFile Upload { get; set; }
        public UploadFileResult Result { get; set; }

        public IndexModel( MarsRoverService service)
        {
            this.service = service;
        }

        public void OnGet()
        {

        }


        public async Task OnPostAsync()
        {
           Result = await service.UploadTextFile(Upload);
        }
    }
}
