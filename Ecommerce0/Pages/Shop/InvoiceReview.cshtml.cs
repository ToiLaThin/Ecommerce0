using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce0.Modals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Ecommerce0.Pages.Shop
{
    [Authorize]
    public class InvoiceReviewModel : PageModel
    {
        public List<CartItem> CartItems;

        public InvoiceReviewModel()
        {            
        }
        public void OnGet()
        {
            string sessionCart = HttpContext.Session.GetString("cart");
            if (sessionCart != null)
            {
                CartItems = JsonConvert.DeserializeObject<List<CartItem>>(sessionCart);
            }
            else
                CartItems = new List<CartItem>();
        }
    }
}
