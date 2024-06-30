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

    [HttpGet("{customerId}")]
    public FileStreamResult GetFile([FromRoute]Guid customerId)
    {
        return File(_image.GetImage(customerId.ToString()),"image/jpeg");
    }
}