using Microsoft.AspNetCore.Mvc;
using Application.Dto;
using Application.Repository.Interface;
using Core.Domain;
using Application.Extension;
using DAL.Entity;

namespace CleanArchitecture.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : SingleKeyController<ICustomerRepository,CustomerDto,CustomerDomain,CustomerEntity,int>
{
    private readonly ILogger<CustomerController> _logger;

    public CustomerController(
        ILogger<CustomerController> logger,
        ICustomerRepository manager
    )
    {
        _logger = logger;
        Repository = manager;
    }

    [HttpGet("page")]
    public IActionResult PageQuery([FromQuery] string? firstName,[FromQuery] int? page, [FromQuery] int? pageSize)
    {
       var result = Repository.PageQuery(firstName, page, pageSize);
        return Ok(new{ Count=result.count,List=result.list.Select(x => x.AutoMap<CustomerDto,CustomerDomain>())});
    }
}
