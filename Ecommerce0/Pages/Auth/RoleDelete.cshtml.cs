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
    public class RoleDeleteModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleDeleteModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public class InputModel
        {
            [Required]
            public string ID { set; get; }
            public string Name { set; get; }
        }
        [BindProperty]
        public InputModel Input { set; get; }
        [BindProperty]
        public bool isConfirmed { set; get; }
        public void OnGet() => NotFound("Không thấy");
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return NotFound("Không xóa được");
            var role = await _roleManager.FindByIdAsync(Input.ID);
            if (role == null)
            {
                return NotFound("Không thấy role cần xóa");
            }
            ModelState.Clear();

            if (isConfirmed){
                await _roleManager.DeleteAsync(role); //Xóa
                return RedirectToPage("Index");
                return RedirectToPage("RoleIndex");
            }
            else
            {
                //ban đầu là gọi OnPost nhưng isConfirmed = false
                //để đi vào luồng này và gán giá trị cho bindedProperty
                Input.Name = role.Name;
                isConfirmed = true;
                return Page();
            }
        }
    }
}
