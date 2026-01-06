using Films.Domain.Entities;
using Films.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Films.Web.Pages.Users;

public class DetailsModel : PageModel
{
    private readonly IUserService _userService;

    public DetailsModel(IUserService userService)
    {
        _userService = userService;
    }

    public User? User { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        User = await _userService.GetByIdAsync(id.Value);

        if (User == null)
        {
            return NotFound();
        }

        return Page();
    }
}
