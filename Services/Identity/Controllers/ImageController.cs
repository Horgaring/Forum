using System.Net.Mime;
using Identityserver.Services;
using Microsoft.AspNetCore.Mvc;

namespace Identityserver.Controllers;

[Route("images")]
public class ImageController: Controller
{
    private readonly IImageService _image;

    public ImageController(IImageService image)
    {
        _image = image;
    }

    [HttpGet("images/{customerId}")]
    public FileStreamResult GetFile([FromRoute]string customerId)
    {
        return File(_image.GetImage(customerId),"image/jpeg");
    }
}