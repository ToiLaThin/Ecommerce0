using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce0.Pages.Auth
{
    [Authorize(Roles = "Admin")]
    public class RoleUserAssignModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public RoleUserAssignModel(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public class InputModel
        {
            [Required]
            public string ID { set; get; }
            public string Name { set; get; }

            public string[] RoleNames { set; get; }

        }

        [BindProperty]
        public InputModel Input { set; get; }

        [BindProperty]
        public bool isConfirmed { set; get; }

        public IActionResult OnGet() => NotFound("Không thấy");

        public List<string> AllRoles { set; get; } = new List<string>();

        public async Task<IActionResult> OnPost()
        {


            var user = await _userManager.FindByIdAsync(Input.ID);
            if (user == null)
            {
                return NotFound("Không thấy role cần xóa");
            }
            //Tất cả role gắn với user hiện tại 
            var userRoles = await _userManager.GetRolesAsync(user);

            //Tất cả role trong AspNetRoles table
            var allroles = await _roleManager.Roles.ToListAsync();
            allroles.ForEach((r) => {
                AllRoles.Add(r.Name);
            });

            if (!isConfirmed)
            {
                Input.RoleNames = userRoles.ToArray();
                isConfirmed = true;
                ModelState.Clear();
            }
            else
            {
                // Update add and remove
                if (Input.RoleNames == null) Input.RoleNames = new string[] { };
                foreach (var rolename in userRoles)
                {
                    if (Input.RoleNames.Contains(rolename)) continue;
                    await _userManager.RemoveFromRoleAsync(user, rolename);
                }
                foreach (var rolename in Input.RoleNames)
                {
                    if (userRoles.Contains(rolename)) continue;
                    await _userManager.AddToRoleAsync(user, rolename);
                }
                return RedirectToPage("/Auth/RoleUserIndex");

            }

            Input.Name = user.UserName;
            return Page();
        }
    }
}
