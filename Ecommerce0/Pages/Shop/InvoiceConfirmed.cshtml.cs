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
    public class InvoiceConfirmedModel : PageModel
    {
        private ApplicationDbContext _db;
        public InvoiceConfirmedModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet()
        {
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(new List<CartItem>()));
            Global.GlobalVar.CartItemCount = 0;
        }

        public async Task<IActionResult> OnGetConfirmed()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToPage("/auth/logon");
            else
            {
                //Add new invoice
                var user = _db.Users.SingleOrDefault(u => u.UserName.Equals(User.Identity.Name));
                var invoice = new Invoice
                {
                    InvoiceName = "Your invoice",
                    //PaymentStatus đã có giá trị mặc định rồi nên không cần gán
                    CustomerId = user.Id,
                    CreatedOn = DateTime.Now
                    
                };
                await _db.Invoices.AddAsync(invoice);
                await _db.SaveChangesAsync();

                //Add invoice detail
                List<CartItem> cartItems = JsonConvert.DeserializeObject<List<CartItem>>(HttpContext.Session.GetString("cart"));
                if(cartItems != null) {
                    foreach(var cartItem in cartItems)
                    {
                        var invoiceDetail = new InvoiceDetail
                        {
                            InvoiceId = invoice.Id,
                            BookId = cartItem.Product.Id,
                            PricePerUnit = cartItem.Product.Price,
                            Quantity = cartItem.Quantity,
                            TotalItemPrice = cartItem.Quantity * cartItem.Product.Price
                        };
                        await _db.InvoiceDetails.AddAsync(invoiceDetail);
                        await _db.SaveChangesAsync();
                    }                    
                }
                //return Page khác return RedirectToPage
                //return Page thì ko gọi OnGet còn RedirectToPage thì có
                return RedirectToPage("/shop/invoiceConfirmed");
            }
        }
    }
}
