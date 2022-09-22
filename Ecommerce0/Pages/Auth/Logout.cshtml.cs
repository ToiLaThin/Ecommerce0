using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ecommerce0.Pages.Auth
{
    public class LogoutModel : PageModel
    {
        public SignInManager<IdentityUser> SignInManager { get; set; }

        public LogoutModel(SignInManager<IdentityUser> signInManager)
        {
            SignInManager = signInManager;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostLogOutAsync()
        {
            await SignInManager.SignOutAsync();
            return RedirectToPage("/me/index");
        }

        public async Task<IActionResult> OnPostNoLogOutAsync()
        {
            return RedirectToPage("/me/index");
        }
    }
}
