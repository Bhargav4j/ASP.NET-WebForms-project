using Films.Domain.Entities;
using Films.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Films.Web.Pages.Actors;

public class IndexModel : PageModel
{
    private readonly IActorService _actorService;

    public IndexModel(IActorService actorService)
    {
        _actorService = actorService;
    }

    public IEnumerable<Actor> Actors { get; set; } = new List<Actor>();

    public async Task OnGetAsync()
    {
        Actors = await _actorService.GetAllAsync();
    }
}
