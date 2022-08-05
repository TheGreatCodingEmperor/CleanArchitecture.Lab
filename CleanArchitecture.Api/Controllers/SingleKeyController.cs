using Application.Dto;
using Application.Extension;
using Microsoft.AspNetCore.Mvc;
using Application.Repository.Interface;

namespace CleanArchitecture.Api.Controllers;

// [ApiController]
// [Route ("[controller]")]
public class SingleKeyController<TIRepository,TDto, TDomain,TEntity, TKey> : ControllerBase
where TDomain : class, new ()
where TEntity : class, new ()
where TDto : class, new () 
where TIRepository:IRepository<TDomain,TEntity, TKey> {
    protected TIRepository Repository { get; set; }

    [HttpGet]
    public virtual IActionResult GetAll () {
        IEnumerable<TDto>? result = Repository.GetAll ().Select (x => ToDto(x));
        return Ok (result);
    }

    [HttpGet ("{id}")]
    public virtual IActionResult GetById ([FromRoute] TKey id) {
        TDto? result = ToDto(Repository.FindById (id));
        return Ok (result);
    }

    [HttpPost]
    public virtual IActionResult Add ([FromBody] TDto dto) {
        TDto? result = ToDto(Repository.Add (dto.AutoMap<TDomain, TDto> ()));
        return Ok (result);
    }

    [HttpPost ("range")]
    public virtual IActionResult Add ([FromBody] IEnumerable<TDto> dtos) {
        IEnumerable<TDto>? result = Repository.Add (dtos.Select (x => x.AutoMap<TDomain, TDto> ())).Select (x => ToDto(x));
        return Ok (result);
    }

    [HttpPut]
    public virtual IActionResult Update ([FromBody] TDto dto) {
        try {
            TDto? result = ToDto(Repository.Update (dto.AutoMap<TDomain, TDto> ()));
            return Ok (result);
        } catch (Exception ex) {
            return BadRequest (ex.Message);
        }
    }

    [HttpPut ("range")]
    public virtual IActionResult Update ([FromBody] IEnumerable<TDto> dto) {
        try {
            IEnumerable<TDto>? result = Repository.Update (dto.Select(x => x.AutoMap<TDomain, TDto> ())).Select (x => x.AutoMap<TDto, TDomain> ());
            return Ok (result);
        } catch (Exception ex) {
            return BadRequest (ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public virtual IActionResult Update ([FromRoute] TKey id) {
        try {
            Repository.Delete (id);
            return Ok ();
        } catch (Exception ex) {
            return BadRequest (ex.Message);
        }
    }

    [HttpDelete("range")]
    public virtual IActionResult Update ([FromHeader] IEnumerable<TKey> ids) {
        try {
            Repository.Delete (ids);
            return Ok ();
        } catch (Exception ex) {
            return BadRequest (ex.Message);
        }
    }

    [NonAction]
    public virtual TDto ToDto(TDomain domain){
        return domain.AutoMap<TDto,TDomain>();
    }
}