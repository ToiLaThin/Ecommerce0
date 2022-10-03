using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ecommerce0.Pages.Auth
{
    [Authorize(Roles ="Admin")]
    public class RoleCreateModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleCreateModel(RoleManager<IdentityRole> roleManager) 
        {
            _roleManager = roleManager;
        }

        public class InputModel
        {
            public string ID { set; get; }
            [Required(ErrorMessage = "Phải nhập tên role")]
            [Display(Name = "Tên của Role")]
            [StringLength(100, ErrorMessage = "{0} dài {2} đến {1} ký tự.", MinimumLength = 3)]
            public string Name { set; get; }
        }

        [BindProperty]
        public InputModel Input { set; get; }

        // Không cho truy cập trang mặc định mà không có handler
        public IActionResult OnGet() => NotFound("Không thấy");
        public IActionResult OnPost() => NotFound("Không thấy");
        public IActionResult OnPostCreateRoleReady()
        {
            ModelState.Clear();
            return Page();
        }

        public async Task<IActionResult> OnPostAddRole()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else
            {
                var newRole = new IdentityRole(Input.Name);
                var roleCreateResult = await _roleManager.CreateAsync(newRole);
                if(roleCreateResult.Succeeded)
                {
                    return RedirectToPage("/auth/RoleIndex");
                }
            }
            return Page();
        }
    }
}
