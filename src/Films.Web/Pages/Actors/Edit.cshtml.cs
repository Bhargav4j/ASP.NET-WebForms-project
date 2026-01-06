using Films.Domain.Entities;
using Films.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Films.Web.Pages.Actors;

public class EditModel : PageModel
{
    private readonly IActorService _actorService;

    public EditModel(IActorService actorService)
    {
        _actorService = actorService;
    }

    [BindProperty]
    public ActorInputModel ActorInput { get; set; } = new();

    public class ActorInputModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;
    }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var actor = await _actorService.GetByIdAsync(id.Value);

        if (actor == null)
        {
            return NotFound();
        }

        ActorInput = new ActorInputModel
        {
            Id = actor.Id,
            FirstName = actor.FirstName,
            LastName = actor.LastName
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var actor = new Actor
        {
            Id = ActorInput.Id,
            FirstName = ActorInput.FirstName,
            LastName = ActorInput.LastName,
            ModifiedBy = "System"
        };

        await _actorService.UpdateAsync(ActorInput.Id, actor);

        return RedirectToPage("./Index");
    }
}
