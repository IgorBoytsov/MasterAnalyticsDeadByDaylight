using DBDAnalytics.Shared.Contracts.Requests.Shared;
using Microsoft.AspNetCore.Http;

namespace Shared.Api
{
    public static class ControllerExtensions
    {
        public static FileInput? ToFileInput(IFormFile? formFile)
        {
            FileInput? imageInput = null;
            if (formFile is not null)
            {
                imageInput = new FileInput(
                    formFile.OpenReadStream(),
                    formFile.FileName,
                    formFile.ContentType);

                return imageInput;
            }

            return imageInput;
        }
    }
}