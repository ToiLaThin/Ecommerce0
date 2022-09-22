using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce0.Global;
using Ecommerce0.Modals;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Ecommerce0.Pages.Shop
{
    public class CartModel : PageModel
    {
        private ApplicationDbContext _db;

        public List<CartItem> Cart { get; set; }

        public double TotalPrice { get; set; }
        public CartModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet()
        {
            string sessionCart = HttpContext.Session.GetString("cart");
            TotalPrice = 0;
            if(sessionCart !=null)
            {
                //chuyển ngược chuỗi json(value của key là cart) được mã hóa về lại thành List CartItem
                Cart = JsonConvert.DeserializeObject<List<CartItem>>(sessionCart);               
                TotalPrice = Cart.Sum(cIt => cIt.Product.Price * cIt.Quantity);
            }
            else
            {
                Cart = new List<CartItem>();
            }
            
            GlobalVar.CartItemCount= Cart.Count();

        }

        private int isExist(int bookId,List<CartItem> items)
        {
            for(var i = 0; i< items.Count; i++)
                if (items[i].Product.Id == bookId)
                    return i;
            return -1;
        }

        public IActionResult OnGetRemove(int id)
        {
            string sessionCart = HttpContext.Session.GetString("cart");
            List<CartItem> items = JsonConvert.DeserializeObject<List<CartItem>>(sessionCart);
            int idBook = isExist(id, items);
            if ( idBook != -1)
                items.RemoveAt(idBook);
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(items));
            return RedirectToPage("Cart");
        }
        public IActionResult OnGetAddCartItem(int id)
        {
            string sessionCart = HttpContext.Session.GetString("cart");
            List<CartItem> items = JsonConvert.DeserializeObject<List<CartItem>>(sessionCart);
            int idBook = isExist(id, items);
            if (idBook != -1)
                items[idBook].Quantity += 1;
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(items));
            return RedirectToPage("Cart");
        }
        public IActionResult OnGetSubtractCartItem(int id)
        {
            string sessionCart = HttpContext.Session.GetString("cart");
            List<CartItem> items = JsonConvert.DeserializeObject<List<CartItem>>(sessionCart);
            int idBook = isExist(id, items);
            if (idBook != -1)
                if (items[idBook].Quantity > 1)
                    items[idBook].Quantity -= 1;
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(items));
            return RedirectToPage("Cart");
        }

        //phai la OnGetBuyNow neu khong se bi loi objecReference is set to null
        public IActionResult OnGetBuyNow(int id)
        {
            string sessionCart = HttpContext.Session.GetString("cart");
            Book book = _db.Books.Include(b => b.Category).AsNoTracking().SingleOrDefault(p => p.Id == id);
            if (sessionCart == null) // chua co cart nao
            {
                List<CartItem> items = new List<CartItem>();
                items.Add(new CartItem() {
                    Product = book,
                    Quantity = 1
                });
                //cart co value la 1 json string đc mã hóa từ 1 CartItem
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(items));
                
            } 
            else
            {
                //đẫ có sản phẩm trong cart rồi
                List<CartItem> items = JsonConvert.DeserializeObject<List<CartItem>>(sessionCart);
                int idCheckBookExisted = isExist(id, items);
                if(idCheckBookExisted == -1) {
                    items.Add(new CartItem()
                    {
                        Product = book,
                        Quantity = 1
                    });
                }
                else
                {
                    items[idCheckBookExisted].Quantity += 1;
                }
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(items));
            }
            //sau do goi OnGet
            //nhu vay Cart chi gan 1 lan trong OnGet()
            return RedirectToPage("Cart");
        }
    }
}
