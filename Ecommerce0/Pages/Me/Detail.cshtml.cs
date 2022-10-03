using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce0.Modals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce0.Pages.Me
{
    [AllowAnonymous]
    public class DetailModel : PageModel
    {
        
        private readonly ApplicationDbContext _db;
        public Book Book { get; set; }
        public DetailModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public void OnGet(int id)
        {
            var book = _db.Books.Where(b => b.Id == id).Include(b => b.Category).FirstOrDefault();
            Book = book;         

        }
    }
}
