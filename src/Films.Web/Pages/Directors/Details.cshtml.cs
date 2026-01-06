using Films.Domain.Entities;
using Films.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Films.Web.Pages.Directors;

public class DetailsModel : PageModel
{
    private readonly IDirectorService _directorService;

    public DetailsModel(IDirectorService directorService)
    {
        _directorService = directorService;
    }

    public Director? Director { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Director = await _directorService.GetByIdAsync(id.Value);

        if (Director == null)
        {
            return NotFound();
        }

        return Page();
    }
}
