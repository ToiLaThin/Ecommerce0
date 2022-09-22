using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce0.Modals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ecommerce0.Pages.Me
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public Book Book { get; set; }
        public async Task OnGet(int id)
        {
            var bookFromDb = await _db.Books.FindAsync(id);
            this.Book = bookFromDb;
        }

        public async Task<IActionResult> OnPost()
        {
            var bookToDel = await _db.Books.FindAsync(Book.Id);
            _db.Books.Remove(bookToDel);
            await _db.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
