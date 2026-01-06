using Films.Domain.Entities;
using Films.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Films.Web.Pages.Users;

public class IndexModel : PageModel
{
    private readonly IUserService _userService;

    public IndexModel(IUserService userService)
    {
        _userService = userService;
    }

    public IEnumerable<User> Users { get; set; } = new List<User>();

    public async Task OnGetAsync()
    {
        Users = await _userService.GetAllAsync();
    }
}
