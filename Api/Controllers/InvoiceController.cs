using Microsoft.AspNetCore.Mvc;
using Application.Dto;
using Application.Manager.Interface;
using Core.Domain;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class InvoiceController : SingleKeyController<InvoiceDto,InvoiceDomain,int>
{

    private readonly ILogger<InvoiceController> _logger;

    public InvoiceController(
        ILogger<InvoiceController> logger,
        IInvoiceManager manager
    )
    {
        _logger = logger;
        Manager = manager;
    }
}
