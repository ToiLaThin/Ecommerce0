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
        [BindProperty]
        public Book Book { get; set; }

        [BindProperty]
        public Review CreatedReview { get; set; }

        [BindProperty]
        public string CurrentUserId { get; set; }

        public List<Review> TopFiveRecentReviewsOfThisProduct { get; set; }
        public DetailModel(ApplicationDbContext db)
        {
            _db = db;
            CreatedReview = new Review();
        }

        public void OnGet(int id)
        {
            var book = _db.Books.Where(b => b.Id == id).Include(b => b.Category).FirstOrDefault();
            Book = book;

            if(User.Identity.IsAuthenticated)
            {
                var currUser = _db.Users.Where(u => u.UserName.Equals(User.Identity.Name)).SingleOrDefault();
                CurrentUserId = currUser.Id;
            }    

            TopFiveRecentReviewsOfThisProduct = _db.Reviews.Include(rv => rv.User).Where(rv => rv.BookId == Book.Id).OrderByDescending(rv => rv.UpdatedOn).Take(5).ToList();
        }

        public async Task<IActionResult> OnPost()
        {
            var newReview = new Review
            {
                BookId = Book.Id,
                CustomerId = CurrentUserId,
                ReviewDetail = CreatedReview.ReviewDetail,
                UpdatedOn = DateTime.Now
            };
            CreatedReview = newReview;
            await _db.Reviews.AddAsync(CreatedReview);
            await _db.SaveChangesAsync();
            return RedirectToPage("/me/detail",new { id = Book.Id });
        }

        public async Task<JsonResult> OnGetLoadMoreReviews(string reviewsToSkipCountStr,string bookIdStr)
        {
            int bookId = Convert.ToInt32(bookIdStr);
            int reviewsToSkipCount = Convert.ToInt32(reviewsToSkipCountStr);
            List<Review> reviewsToReturn = await _db.Reviews.Include(rv => rv.User).Where(rv => rv.BookId == bookId)
                                                        .OrderByDescending(rv => rv.UpdatedOn)
                                                        .Skip(reviewsToSkipCount).Take(5).ToListAsync();

            return new JsonResult(reviewsToReturn);
        }
    }
}
