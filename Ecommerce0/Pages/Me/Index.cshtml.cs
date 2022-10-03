using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ecommerce0.Modals;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce0.Pages.Me
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        //for dependency injection, khong cần tạo object ApplicationDbContext rồi truyền vào constructor
        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        //[BindProperty]
        //public List<int> CheckedTagsId { get; set; }

        [BindProperty]
        public int SelectedCategoryId { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public List<Book> Books { get; set; }
        public List<Book> Top10NewestBooks { get; set; }
        public async Task OnGet()
        {
            this.Books = await _db.Books.Include(b => b.Category).OrderByDescending(b => b.ModifiedDate).ToListAsync();
            this.Top10NewestBooks = await _db.Books.Include(b => b.Category).OrderByDescending(b => b.ModifiedDate).Take(10).ToListAsync();
            this.Categories = await _db.Categories.Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.Name.ToString()
            }).ToListAsync();
        }

        //là url được xmlhttprequest trỏ tới nó nhận tham số và trả về các book dưới dạng json
        public JsonResult OnGetPagination(int pSize, int pNum)
        {
            int skip = (pNum - 1) * pSize;
            int take = pSize;

            var books = _db.Books.Skip(skip).Take(take).Include(b => b.Category).OrderByDescending(b => b.ModifiedDate).ToList();
            return new JsonResult(books);

        }

        public JsonResult OnGetFilterByCate(int CategoryId)
        {
            var booksOfCate = _db.Books.Include(b => b.Category).Where(b => b.Category.Id == CategoryId);
            return new JsonResult(booksOfCate);
        }

        public JsonResult OnGetTagsByCate(int CategoryId)
        {
            var tagsOfCate = _db.TagSubCategories.Include(ts => ts.Tag).Where(ts => ts.IdCategory == CategoryId).Select(ts => ts.Tag);
            return new JsonResult(tagsOfCate);
        }


        public JsonResult OnGetBooksOfTagsAndCate(int CategoryId,string checkedTagsIdStr)
        {
            string[] CheckedTagsIdStrLst = checkedTagsIdStr.Split(",");
            List<int> CheckedTagsId = new List<int>();
            for(int i=0;i< CheckedTagsIdStrLst.Length;i++)
            {
                CheckedTagsId.Add(Convert.ToInt32(CheckedTagsIdStrLst[i].ToString()));
            }

            //Solution links: https://stackoverflow.com/questions/39022615/linq-filter-posts-by-list-of-tags
            List<int> idBooksOfCheckedTags = _db.ProductTags
                                                .Where(pt => CheckedTagsId.Contains(pt.IdTag))
                                                .GroupBy(pt => pt.IdProduct).Where(g => g.Count() == CheckedTagsId.Count)
                                                .Select(g => g.Key)
                                                .Distinct().ToList();
            var booksOfTagsAndCate = _db.Books.Include(b => b.Category).Where(b => b.Category.Id == CategoryId).Where(bOC => idBooksOfCheckedTags.Contains(bOC.Id));

            return new JsonResult(booksOfTagsAndCate);
        }
    }
}
