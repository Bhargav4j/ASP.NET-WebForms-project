using Films.Application.DTOs;
using Films.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Films.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActorsController : ControllerBase
{
    private readonly IActorService _actorService;

    public ActorsController(IActorService actorService)
    {
        _actorService = actorService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ActorDto>>> GetAll()
    {
        var actors = await _actorService.GetAllAsync();
        return Ok(actors);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ActorDto>> GetById(int id)
    {
        var actor = await _actorService.GetByIdAsync(id);
        return actor == null ? NotFound() : Ok(actor);
    }

    [HttpPost]
    public async Task<ActionResult<ActorDto>> Create(ActorCreateDto dto)
    {
        var created = await _actorService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ActorUpdateDto dto)
    {
        await _actorService.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _actorService.DeleteAsync(id);
        return NoContent();
    }
}
