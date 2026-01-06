using Films.Domain.Entities;
using Films.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Films.Web.Pages.Actors;

public class DeleteModel : PageModel
{
    private readonly IActorService _actorService;

    public DeleteModel(IActorService actorService)
    {
        _actorService = actorService;
    }

    [BindProperty]
    public Actor? Actor { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Actor = await _actorService.GetByIdAsync(id.Value);

        if (Actor == null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        await _actorService.DeleteAsync(id.Value);

        return RedirectToPage("./Index");
    }
}
