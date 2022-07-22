using Microsoft.AspNetCore.Mvc;
using Application.Dto;
using Application.Repository.Interface;
using Core.Domain;
using Application.Extension;
using DAL.Entity;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class InvoiceItemController : SingleKeyController<IInvoiceItemRepository,InvoiceItemDto,InvoiceItemDomain,InvoiceItemEntity,int>
{

    private readonly ILogger<InvoiceItemController> _logger;

    public InvoiceItemController(
        ILogger<InvoiceItemController> logger,
        IInvoiceItemRepository manager
    )
    {
        _logger = logger;
        Repository = manager;
    }

    public override IActionResult Add ([FromBody] InvoiceItemDto dto) {
        InvoiceItemDto? result = Repository.Add (dto.AutoMap<InvoiceItemDomain, InvoiceItemDto> ()).AutoMap<InvoiceItemDto, InvoiceItemDomain> ();
        return Ok (result);
    }
}
