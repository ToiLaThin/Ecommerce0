using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce0.Modals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce0.Pages.Cat
{
    public class ListByCateModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public IEnumerable<Book> BooksOfCate { get; set; }

        public Category Category { get; set; }

        public ListByCateModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task OnGet(int cateId) 
        {
            //trong Where thì truy xuất Category.Id được nhưng khi gán ra biến thì Category = null
            this.BooksOfCate = await _db.Books.Where(b => b.Category != null)
                                        .Where(b => b.Category.Id == cateId)
                                        .Include(b => b.Category)
                                        .OrderBy(b => b.Title) 
                                        .ToListAsync();

            this.Category = await _db.Categories.FindAsync(cateId);
        }
    }
}
