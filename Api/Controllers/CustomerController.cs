using Microsoft.AspNetCore.Mvc;
using Application.Dto;
using Application.Manager.Interface;
using Core.Domain;
using Application.Extension;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : SingleKeyController<ICustomerManager,CustomerDto,CustomerDomain,int>
{
    private readonly ILogger<CustomerController> _logger;

    public CustomerController(
        ILogger<CustomerController> logger,
        ICustomerManager manager
    )
    {
        _logger = logger;
        Manager = manager;
    }

    [HttpGet("page")]
    public IActionResult PageQuery([FromQuery] string? firstName,[FromQuery] int? page, [FromQuery] int? pageSize)
    {
       var result = Manager.PageQuery(firstName, page, pageSize);
        return Ok(new{ Count=result.count,List=result.list.Select(x => x.AutoMap<CustomerDto,CustomerDomain>())});
    }
}
