using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce0.Modals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ecommerce0.Pages.Auth
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        //Lay thông tin User và load lên form
        private ApplicationDbContext _db;
        private UserManager<MyIdentityUser> _userManager;
        private SignInManager<MyIdentityUser> _signInManager;
        private IHostingEnvironment _environment;
        [BindProperty]
        public MyIdentityUser CurrUser { get; set; }
        public String[] IconPaths { get; set; }
        public ProfileModel(ApplicationDbContext db, UserManager<MyIdentityUser> userManager, SignInManager<MyIdentityUser> signInManager, IHostingEnvironment environment)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment;
        }
        public void OnGet()
        {
            CurrUser = _db.Users.Where(u => u.UserName.Equals(User.Identity.Name)).SingleOrDefault();
            IconPaths = Directory.GetFiles(Path.Combine(_environment.WebRootPath, "assets", "img"));
            for (int i = 0; i < IconPaths.Length; i++){
                IconPaths[i] = Path.GetRelativePath(_environment.WebRootPath, IconPaths[i]);
            }

        }

        public async Task<IActionResult> OnPostAsync(string Id)
        {
            var userToEdit = await _userManager.FindByIdAsync(Id);
            var userToEditInMyDb = await _db.Users.FindAsync(Id);
            if (ModelState.IsValid)
            {
                userToEdit.UserName = CurrUser.UserName;
                userToEdit.PhoneNumber = CurrUser.PhoneNumber;
                userToEdit.Email = CurrUser.Email;
                userToEdit.AvatarPath = CurrUser.AvatarPath;
                await _userManager.UpdateAsync(userToEdit);

                //update in Users table not just AspNetUsers
                userToEditInMyDb.UserName = CurrUser.UserName;
                userToEditInMyDb.PhoneNumber = CurrUser.PhoneNumber;
                userToEditInMyDb.Email = CurrUser.Email;
                userToEditInMyDb.AvatarPath = CurrUser.AvatarPath;
                await _db.SaveChangesAsync();

                //vấn đề là HttpContext.User.Identity.Name không cập nhật cho đến khi logout nên nếu return Page() thì sẽ CurrUser sẽ là null

                await _signInManager.SignOutAsync();
                return RedirectToPage("/me/index");
            }
            //no way it is invalid because we use type for input so it validated before go to this handler
            else
            {
                return Page();
            }
        }
    }
}
