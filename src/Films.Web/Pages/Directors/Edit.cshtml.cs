using Films.Domain.Entities;
using Films.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Films.Web.Pages.Directors;

public class EditModel : PageModel
{
    private readonly IDirectorService _directorService;

    public EditModel(IDirectorService directorService)
    {
        _directorService = directorService;
    }

    [BindProperty]
    public DirectorInputModel DirectorInput { get; set; } = new();

    public class DirectorInputModel
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

        var director = await _directorService.GetByIdAsync(id.Value);

        if (director == null)
        {
            return NotFound();
        }

        DirectorInput = new DirectorInputModel
        {
            Id = director.Id,
            FirstName = director.FirstName,
            LastName = director.LastName
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var director = new Director
        {
            Id = DirectorInput.Id,
            FirstName = DirectorInput.FirstName,
            LastName = DirectorInput.LastName,
            ModifiedBy = "System"
        };

        await _directorService.UpdateAsync(DirectorInput.Id, director);

        return RedirectToPage("./Index");
    }
}
