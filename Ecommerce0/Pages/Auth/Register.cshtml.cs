using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce0.Modals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ecommerce0.Pages
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private ApplicationDbContext _db;
        [BindProperty]
        public RegisterInfo RegisterInfo { get; set; }

        private readonly UserManager<MyIdentityUser> userManager;
        public SignInManager<MyIdentityUser> SignInManager { get; }

        public RegisterModel(UserManager<MyIdentityUser> userManager, SignInManager<MyIdentityUser> signInManager, ApplicationDbContext db)
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
                var user = new MyIdentityUser()
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
