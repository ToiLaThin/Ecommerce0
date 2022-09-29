using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ecommerce0.Pages.Auth
{
    public class RoleUpdateModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleUpdateModel(RoleManager<IdentityRole> roleManager)
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
        public void OnGet() => NotFound("Không thấy");
        public void OnPost() => NotFound("Không thấy");

        public async Task<IActionResult> OnPostRoleUpdateReady()
        {
            if (Input.ID == null)
                return Page();
            var RoleFinded = await _roleManager.FindByIdAsync(Input.ID);
            if(RoleFinded != null)
            {
                Input.Name = RoleFinded.Name;
                ViewData["Title"] = "Cập nhật role : " + Input.Name;
                ModelState.Clear();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateRole()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            else
            {
                // CẬP NHẬT
                if (Input.ID == null)
                {
                    ModelState.Clear();
                    return Page();
                }
                var result = await _roleManager.FindByIdAsync(Input.ID);
                if (result != null)
                {
                    result.Name = Input.Name;
                    // Cập nhật tên Role
                    var roleUpdateResult = await _roleManager.UpdateAsync(result);
                    if (roleUpdateResult.Succeeded)
                    {
                        return RedirectToPage("/Auth/RoleIndex");
                    }
                    return Page();
                }
                return Page();
            }
        }
    }
}
