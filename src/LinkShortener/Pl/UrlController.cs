using LinkShortener.Pl.Interfaces;
using LinkShortener.Pl.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LinkShortener.Controllers;

[ApiController]
[Route("/")]
public class UrlController : ControllerBase
{
    private readonly ILinkService _bllProvider;

    public UrlController(ILinkService bllProvider)
    {
        _bllProvider = bllProvider;
    }

    [HttpGet("{url}")]
    public new ActionResult Redirect(string url)
    {
        try
        {
            var fullUrl = _bllProvider.GetLongLink(url);
            return Redirect(fullUrl);
        }
        catch (Exception e)
        {
            Log.Information(e.Message);
            return BadRequest("Something went wrong.");
        }
    }

    [HttpPost]
    public ActionResult<CreateShortUrlResponse> CreateShort(CreateShortUrlRequest request)
    {
        try
        {
            var shortUrl = _bllProvider.StoreLongLink(request.Name);
            return Ok(new CreateShortUrlResponse { ShortLink = shortUrl });
        }
        catch (Exception e)
        {
            Log.Information(e.Message);
            return BadRequest("Something went wrong.");
        }
    }
}