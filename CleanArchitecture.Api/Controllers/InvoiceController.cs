using Microsoft.AspNetCore.Mvc;
using Application.Dto;
using Application.Repository.Interface;
using Core.Domain;
using DAL.Entity;

namespace CleanArchitecture.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class InvoiceController : SingleKeyController<IInvoiceRepository,InvoiceDto,InvoiceDomain,InvoiceEntity,int>
{

    private readonly ILogger<InvoiceController> _logger;

    public InvoiceController(
        ILogger<InvoiceController> logger,
        IInvoiceRepository manager
    )
    {
        _logger = logger;
        Repository = manager;
    }
}
