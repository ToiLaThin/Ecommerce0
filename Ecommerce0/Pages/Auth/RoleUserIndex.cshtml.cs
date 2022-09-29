using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ecommerce0.Pages.Auth
{
    public class RoleUserIndexModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public RoleUserIndexModel(RoleManager<IdentityRole> roleManager,
                          UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public class UserInList : IdentityUser
        {
            // Liệt kê các Role của User ví dụ: "Admin,Editor" ...
            public string Roles { set; get; }
        }

        public List<UserInList> Users;
        public int totalPages { set; get; }

        public IActionResult OnPost() => NotFound("Cấm post");

        public async Task<IActionResult> OnGet()
        {
            Users = (from u in _userManager.Users
                          orderby u.UserName
                          select new UserInList()
                          {
                              Id = u.Id,
                              UserName = u.UserName,
                          }).ToList();

            foreach (var user in Users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.Roles = string.Join(",", roles.ToList());
            }

            return Page();
        }
    }
}
