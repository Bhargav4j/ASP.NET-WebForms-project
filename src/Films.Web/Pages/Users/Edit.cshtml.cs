using Films.Domain.Entities;
using Films.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Films.Web.Pages.Users;

public class EditModel : PageModel
{
    private readonly IUserService _userService;

    public EditModel(IUserService userService)
    {
        _userService = userService;
    }

    [BindProperty]
    public UserInputModel UserInput { get; set; } = new();

    public class UserInputModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;

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

        var user = await _userService.GetByIdAsync(id.Value);

        if (user == null)
        {
            return NotFound();
        }

        UserInput = new UserInputModel
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = new User
        {
            Id = UserInput.Id,
            Username = UserInput.Username,
            Email = UserInput.Email,
            FirstName = UserInput.FirstName,
            LastName = UserInput.LastName,
            Password = string.Empty,
            ModifiedBy = "System"
        };

        await _userService.UpdateAsync(UserInput.Id, user);

        return RedirectToPage("./Index");
    }
}
