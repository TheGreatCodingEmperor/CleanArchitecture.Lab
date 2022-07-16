using Microsoft.AspNetCore.Mvc;
using Application.Dto;
using Application.Manager.Interface;
using Core.Domain;
using Application.Extension;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class InvoiceItemController : SingleKeyController<IInvoiceItemManager,InvoiceItemDto,InvoiceItemDomain,int>
{

    private readonly ILogger<InvoiceItemController> _logger;

    public InvoiceItemController(
        ILogger<InvoiceItemController> logger,
        IInvoiceItemManager manager
    )
    {
        _logger = logger;
        Manager = manager;
    }

    public override IActionResult Add ([FromBody] InvoiceItemDto dto) {
        InvoiceItemDto? result = Manager.Add (dto.AutoMap<InvoiceItemDomain, InvoiceItemDto> ()).AutoMap<InvoiceItemDto, InvoiceItemDomain> ();
        return Ok (result);
    }
}
