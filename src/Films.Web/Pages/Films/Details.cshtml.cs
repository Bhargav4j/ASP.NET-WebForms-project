using Films.Domain.Entities;
using Films.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Films.Web.Pages.Films;

public class DetailsModel : PageModel
{
    private readonly IFilmService _filmService;

    public DetailsModel(IFilmService filmService)
    {
        _filmService = filmService;
    }

    public Film? Film { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Film = await _filmService.GetByIdAsync(id.Value);

        if (Film == null)
        {
            return NotFound();
        }

        return Page();
    }
}
