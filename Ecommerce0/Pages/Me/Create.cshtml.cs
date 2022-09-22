using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce0.Modals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce0.Pages.Me
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public Book Book { get; set; }
        public List<SelectListItem> Options { get; set; }

        [BindProperty]
        public int Cat { get; set; }
        //Cat là prop để lưu id của 
        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task OnGet()
        {
            Options = await _db.Categories.Select(c =>
                new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name.ToString()
                }).ToListAsync();
        }

        public async Task<IActionResult> OnPost()
        {
            Category selectedCetegory = await _db.Categories.FindAsync(Cat);
            this.Book.Category = selectedCetegory; 
            await this._db.Books.AddAsync(Book);
            await this._db.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
