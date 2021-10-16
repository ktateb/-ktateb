using Microsoft.AspNetCore.Http;

namespace Model.User.Inputs
{
    public class UpdatePhoto
    {
        public IFormFile PictureUrl { get; set; }
    }
}