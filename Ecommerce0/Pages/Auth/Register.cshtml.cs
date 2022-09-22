using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce0.Modals;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ecommerce0.Pages
{
    public class RegisterModel : PageModel
    {
        private ApplicationDbContext _db;
        [BindProperty]
        public RegisterInfo RegisterInfo { get; set; }

        private readonly UserManager<IdentityUser> userManager;
        public SignInManager<IdentityUser> SignInManager { get; }

        public RegisterModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext db)
        {
            _db = db;
            this.userManager = userManager;
            this.SignInManager = signInManager;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid){
                var user = new IdentityUser()
                {
                    UserName = RegisterInfo.UserName,
                    Email = RegisterInfo.Email
                };

                var result = await userManager.CreateAsync(user, RegisterInfo.Password);
                if(result.Succeeded) {
                    //thêm user vừa tạo vào bảng Users
                    await _db.Users.AddAsync(user);
                    await _db.SaveChangesAsync();
                    await SignInManager.SignInAsync(user, false);
                    return RedirectToPage("/me/index");
                }
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }
            return Page();
        }
    }
}
