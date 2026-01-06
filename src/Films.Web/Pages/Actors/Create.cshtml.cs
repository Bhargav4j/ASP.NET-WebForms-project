using Films.Domain.Entities;
using Films.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Films.Web.Pages.Actors;

public class CreateModel : PageModel
{
    private readonly IActorService _actorService;

    public CreateModel(IActorService actorService)
    {
        _actorService = actorService;
    }

    [BindProperty]
    public ActorInputModel ActorInput { get; set; } = new();

    public class ActorInputModel
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var actor = new Actor
        {
            FirstName = ActorInput.FirstName,
            LastName = ActorInput.LastName,
            CreatedBy = "System"
        };

        await _actorService.CreateAsync(actor);

        return RedirectToPage("./Index");
    }
}
