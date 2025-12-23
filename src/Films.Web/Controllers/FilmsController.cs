using Films.Application.DTOs;
using Films.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Films.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilmsController : ControllerBase
{
    private readonly IFilmService _filmService;

    public FilmsController(IFilmService filmService)
    {
        _filmService = filmService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FilmDto>>> GetAll()
    {
        var films = await _filmService.GetAllAsync();
        return Ok(films);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FilmDto>> GetById(int id)
    {
        var film = await _filmService.GetByIdAsync(id);
        return film == null ? NotFound() : Ok(film);
    }

    [HttpPost]
    public async Task<ActionResult<FilmDto>> Create(FilmCreateDto dto)
    {
        var created = await _filmService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, FilmUpdateDto dto)
    {
        await _filmService.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _filmService.DeleteAsync(id);
        return NoContent();
    }
}
