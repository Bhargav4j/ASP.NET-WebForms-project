using Films.Domain.Entities;
using Films.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Films.Web.Pages.Users;

public class CreateModel : PageModel
{
    private readonly IUserService _userService;

    public CreateModel(IUserService userService)
    {
        _userService = userService;
    }

    [BindProperty]
    public UserInputModel UserInput { get; set; } = new();

    public class UserInputModel
    {
        [Required]
        [StringLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

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

        var user = new User
        {
            Username = UserInput.Username,
            Email = UserInput.Email,
            Password = UserInput.Password,
            FirstName = UserInput.FirstName,
            LastName = UserInput.LastName,
            CreatedBy = "System"
        };

        await _userService.CreateAsync(user);

        return RedirectToPage("./Index");
    }
}
