using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ecommerce0.Modals;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce0.Pages.Me
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        //for dependency injection, khong cần tạo object ApplicationDbContext rồi truyền vào constructor
        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public List<Book> Books { get; set; }
        public List<Book> Top10NewestBooks { get; set; }
        public async Task OnGet()
        {
            this.Books =  await _db.Books.Include(b => b.Category).OrderByDescending(b => b.ModifiedDate).ToListAsync();
            this.Top10NewestBooks =  await _db.Books.Include(b => b.Category).OrderByDescending(b => b.ModifiedDate).Take(10).ToListAsync();
        }

        //là url được xmlhttprequest trỏ tới nó nhận tham số và trả về các book dưới dạng json
        public IActionResult OnGetPagination(int pSize,int pNum)
        {
            int skip = (pNum - 1) * pSize;
            int take = pSize;

            var books = _db.Books.Skip(skip).Take(take).Include(b => b.Category).OrderByDescending(b => b.ModifiedDate).ToList();
            return new JsonResult(books);
            
        }
    }
}
