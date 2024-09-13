using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Subscribers.Contracts.Subscribers;
using Subscribers.Models.Subscribers;
using Subscribers.Services.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Subscribers.WebApi.Controllers;

[ApiController]
[Microsoft.AspNetCore.Components.Route("api")]
public class SubscribersController(ISubscribersService subscribersService, IMapper mapper) : ControllerBase
{
    private readonly ISubscribersService _subscribersService = subscribersService;
    private readonly IMapper _mapper = mapper;

    [SwaggerOperation(description: "Import subscribers expiration date from CSV file")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Subscribers expiration date are imported")]
    [HttpPost("v1/[controller]/import")]

    public async Task<IActionResult> ImportSubscribers([FromForm] FileImportViewModel file)
    {
        await _subscribersService.ImportSubscribers(file.FormFile);

        return Ok();
    }

    [SwaggerOperation(description: "Get subscribers by search parameter")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Subscribers is retrieved", typeof(List<SubscriberViewModel>))]
    [HttpGet("v1/[controller]")]
    public async Task<IActionResult> GetList([FromQuery] SubscriberSearchViewModel search)
    {
        var response = await _subscribersService.Search(_mapper.Map<SubscriberSearch>(search));

        return Ok(_mapper.Map<List<SubscriberViewModel>>(response));
    }
}
