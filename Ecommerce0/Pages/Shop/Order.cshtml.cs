using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce0.Modals;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Ecommerce0.Pages.Shop
{
    public class OrderModel : PageModel
    {
        public ApplicationDbContext _db;
        [BindProperty]
        public Order WaitingOrder { get; set; }
        public OrderModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet() { }

        //khong sử dụng được vì khi submit form ở front-end thì PageModal đã mất hết dữ liệu Waiting Order
        //public async Task<IActionResult> OnGetOrder()
        //{
        //    if (!User.Identity.IsAuthenticated)
        //        return RedirectToPage("/auth/logon");
        //    else
        //    {
        //        var user = _db.Users.SingleOrDefault(u => u.UserName.Equals(User.Identity.Name));
        //        WaitingOrder = new Order
        //        {
        //            OrderName = "Your Order",
        //            CustomerId = user.Id,
        //            OrderedOn = DateTime.Now,              
        //        };
        //        //create a waiting order but do not add to the db yet. only do so when we confirm phone and address

        //        List<CartItem> cartItems = JsonConvert.DeserializeObject<List<CartItem>>(HttpContext.Session.GetString("cart"));
        //        if (cartItems != null)
        //            WaitingOrderDetails = new List<OrderDetail>();
        //            foreach(var cartItem in cartItems){
        //                var orderDetail = new OrderDetail
        //                {
        //                    OrderId = WaitingOrder.Id,
        //                    BookId = cartItem.Product.Id,
        //                    PricePerUnit = cartItem.Product.Price,
        //                    Quantity = cartItem.Quantity,
        //                    TotalItemPrice = cartItem.Quantity * cartItem.Product.Price
        //                };
        //                WaitingOrderDetails.Add(orderDetail);
        //            }
        //        //create a list of waiting order detail but do not add to the db yet. only do so when we confirm phone and addre
        //        //REdirectToPage giống như load lại toàn bộ trang vậy nên Waitng Order sẽ bị mất toàn bộ dữ liêu -> Page()
        //        return Page();
        //    }
        //}

        public async Task<IActionResult> OnPostOrderConfirmed()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToPage("/auth/logon");
            else
            {
                var user = _db.Users.SingleOrDefault(u => u.UserName.Equals(User.Identity.Name));
                WaitingOrder.OrderName = "Your Order";
                WaitingOrder.CustomerId = user.Id;
                WaitingOrder.OrderedOn = DateTime.Now;
                await _db.Orders.AddAsync(WaitingOrder);
                await _db.SaveChangesAsync();

                List<CartItem> cartItems = JsonConvert.DeserializeObject<List<CartItem>>(HttpContext.Session.GetString("cart"));
                if (cartItems != null)
                    foreach (var cartItem in cartItems)
                    {
                        var orderDetail = new OrderDetail
                        {
                            OrderId = WaitingOrder.Id,
                            BookId = cartItem.Product.Id,
                            PricePerUnit = cartItem.Product.Price,
                            Quantity = cartItem.Quantity,
                            TotalItemPrice = cartItem.Quantity * cartItem.Product.Price
                        };
                        await _db.OrderDetails.AddAsync(orderDetail);
                        await _db.SaveChangesAsync();
                    }
                return RedirectToPage("/shop/InvoiceConfirmed");
            }
        }

    }
}
