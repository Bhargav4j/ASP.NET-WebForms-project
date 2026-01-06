using Films.Domain.Entities;
using Films.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Films.Web.Pages.Directors;

public class CreateModel : PageModel
{
    private readonly IDirectorService _directorService;

    public CreateModel(IDirectorService directorService)
    {
        _directorService = directorService;
    }

    [BindProperty]
    public DirectorInputModel DirectorInput { get; set; } = new();

    public class DirectorInputModel
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

        var director = new Director
        {
            FirstName = DirectorInput.FirstName,
            LastName = DirectorInput.LastName,
            CreatedBy = "System"
        };

        await _directorService.CreateAsync(director);

        return RedirectToPage("./Index");
    }
}
