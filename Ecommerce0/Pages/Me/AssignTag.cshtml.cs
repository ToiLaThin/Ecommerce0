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
    public class AssignTagModel : PageModel
    {

        private ApplicationDbContext _db;

        [BindProperty]
        public Book Book { get; set; }

        public List<TagCategory> TagCategories { get; set; }

        [BindProperty]
        public List<int> CheckedTagsId { get; set; }

        public List<ProductTag> ProductTags { get; set; }
        public AssignTagModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool ProductHadTag(int tagId)
        {
            foreach(var pt in ProductTags)
                if (pt.IdTag == tagId)
                    return true;
            return false;
        }

        public async Task OnGet(int IdProduct)
        {
            var books = _db.Books.Include(b => b.Category);
            var theBook = await books.Where(b => b.Id == IdProduct)
                                     .FirstOrDefaultAsync();
            int IdCateOfProduct = theBook.Category.Id;
            Book = theBook;
            ProductTags = await _db.ProductTags.Include(pt => pt.Tag)
                                               .Where(pt => pt.IdProduct == IdProduct)
                                               .Select(pt => pt)
                                               .ToListAsync();
            //trên form sẽ tự nhận giá trị id của book(product) vì đã bindProperty

            var tagCategories = await _db.TagSubCategories.Include(ts => ts.Tag).Where(ts => ts.IdCategory == IdCateOfProduct).ToListAsync();
            TagCategories = tagCategories;
        }

        //Dùng để trả tên tag cho frontend
        public async Task<ContentResult> OnGetTagName(int tagId)
        {
            var theTagName = _db.Tags.Where(t => t.Id == tagId).Select(t => t.TagName).Single();
            return Content(theTagName);

        }
        public async Task<ActionResult> OnPost()
        {
            ProductTag productTag = new ProductTag();
            for(int i=0; i< CheckedTagsId.Count;i++)
            {
                productTag.IdProduct = Book.Id;
                productTag.IdTag = CheckedTagsId[i];
                productTag.Product = await _db.Books.Include(b => b.Category).Where(b => b.Id == productTag.IdProduct).FirstOrDefaultAsync();
                productTag.Tag = await _db.Tags.FindAsync(productTag.IdTag);
                await _db.ProductTags.AddAsync(productTag);                
                await _db.SaveChangesAsync();
                productTag = new ProductTag();
                //Quan trọng để refresh lại cột Id, nếu không refresh lại thì không thêm vào được vì Id là Identity , nếu chèn vô hơn
                //2 bản ghi thì sẽ gây lỗi trùng Id,
            }
            return RedirectToPage("/me/index");
        }

        
    }
}
