using Microsoft.AspNetCore.Mvc;
using Application.Dto;
using Application.Manager.Interface;
using Core.Domain;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : SingleKeyController<CustomerDto,CustomerDomain,int>
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
}
