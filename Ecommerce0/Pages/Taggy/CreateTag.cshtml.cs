using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce0.Modals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce0.Pages.Taggy
{
    public class CreateTagModel : PageModel
    {
        private ApplicationDbContext _db;

        [BindProperty]
        public Tag Tag { get; set; }

        public TagCategory TagCategory { get; set; }

        [BindProperty]
        public int CatId { get; set; }

        public List<SelectListItem> Options { get; set; }
        public CreateTagModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task OnGet()
        {
            Options = await _db.Categories.Select(c => 
                new SelectListItem {
                    Value = c.Id.ToString(),
                    Text = c.Name.ToString()
                }).ToListAsync();
        }

        public async Task<ActionResult> OnPost()
        {
            await _db.Tags.AddAsync(Tag);

            Category selectedCategory = await _db.Categories.FindAsync(CatId);
            this.TagCategory = new TagCategory();
            this.TagCategory.Category = selectedCategory;
            this.TagCategory.IdCategory = CatId;
            this.TagCategory.IdTag = Tag.Id;
            this.TagCategory.Tag = this.Tag;

            await _db.TagSubCategories.AddAsync(this.TagCategory);
            await _db.SaveChangesAsync();

            return RedirectToPage("/me/index");
        }

    }
}
