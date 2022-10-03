using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ecommerce0.Modals;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce0.Pages.Me
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public List<SelectListItem> CategoryOptions { get; set; }

        [BindProperty]
        public Book Book { get; set; }

        [BindProperty]
        public int Cat { get; set; }

        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task OnGet(int id)
        {
            var chosenBook = await _db.Books.FindAsync(id);
            Book = chosenBook;

            CategoryOptions = await _db.Categories.Select(c =>
                new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name.ToString()
                }).ToListAsync();
        }
        public async Task<IActionResult> OnPost()
        {
            Category selectedCategory = await _db.Categories.FindAsync(Cat);

            var bookToEdit = await _db.Books.FindAsync(Book.Id);
            bookToEdit.Image = Book.Image;
            bookToEdit.Price = Book.Price;
            bookToEdit.Title = Book.Title;
            bookToEdit.Description = Book.Description;
            bookToEdit.Category = selectedCategory;
            bookToEdit.ModifiedDate = Book.ModifiedDate;
            await _db.SaveChangesAsync();
            return RedirectToPage("index");
        }
    }
}
