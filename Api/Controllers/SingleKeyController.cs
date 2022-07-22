using Application.Dto;
using Application.Extension;
using Application.Manager.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route ("[controller]")]
public class SingleKeyController<TIManagger,TDto, TDomain, TKey> : ControllerBase
where TDomain : class, new ()
where TDto : class, new () 
where TIManagger:IManager<TDomain, TKey> {
    protected TIManagger Manager { get; set; }

    [HttpGet]
    public virtual IActionResult GetAll () {
        IEnumerable<TDto>? result = Manager.GetAll ().Select (x => x.AutoMap<TDto, TDomain> ());
        return Ok (result);
    }

    [HttpGet ("{id}")]
    public virtual IActionResult GetById ([FromRoute] TKey id) {
        TDto? result = Manager.GetById (id).AutoMap<TDto, TDomain> ();
        return Ok (result);
    }

    [HttpPost]
    public virtual IActionResult Add ([FromBody] TDto dto) {
        TDto? result = Manager.Add (dto.AutoMap<TDomain, TDto> ()).AutoMap<TDto, TDomain> ();
        return Ok (result);
    }

    [HttpPost ("range")]
    public virtual IActionResult Add ([FromBody] IEnumerable<TDto> dtos) {
        IEnumerable<TDto>? result = Manager.Add (dtos.Select (x => x.AutoMap<TDomain, TDto> ())).Select (x => x.AutoMap<TDto, TDomain> ());
        return Ok (result);
    }

    [HttpPut]
    public virtual IActionResult Update ([FromBody] TDto dto) {
        try {
            TDto? result = Manager.Update (dto.AutoMap<TDomain, TDto> ()).AutoMap<TDto, TDomain> ();
            return Ok (result);
        } catch (Exception ex) {
            return BadRequest (ex.Message);
        }
    }

    [HttpPut ("range")]
    public virtual IActionResult Update ([FromBody] IEnumerable<TDto> dto) {
        try {
            IEnumerable<TDto>? result = Manager.Update (dto.Select(x => x.AutoMap<TDomain, TDto> ())).Select (x => x.AutoMap<TDto, TDomain> ());
            return Ok (result);
        } catch (Exception ex) {
            return BadRequest (ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public virtual IActionResult Update ([FromRoute] TKey id) {
        try {
            Manager.Delete (id);
            return Ok ();
        } catch (Exception ex) {
            return BadRequest (ex.Message);
        }
    }

    [HttpDelete("range")]
    public virtual IActionResult Update ([FromHeader] IEnumerable<TKey> ids) {
        try {
            Manager.Delete (ids);
            return Ok ();
        } catch (Exception ex) {
            return BadRequest (ex.Message);
        }
    }
}