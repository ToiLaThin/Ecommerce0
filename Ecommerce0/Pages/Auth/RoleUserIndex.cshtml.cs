using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce0.Modals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ecommerce0.Pages.Auth
{
    [Authorize(Roles = "Admin")]
    public class RoleUserIndexModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<MyIdentityUser> _userManager;

        public RoleUserIndexModel(RoleManager<IdentityRole> roleManager,
                          UserManager<MyIdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public class UserInList : MyIdentityUser
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
