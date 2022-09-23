using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ecommerce0.Global;
using Newtonsoft.Json.Linq;

namespace Ecommerce0.Pages.Shop
{
    public class MomoModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnGetPayment()
        {
            string endpoint = "https://payment.momo.vn/v2/gateway/api/create";
            string partnerCode = "MOMOI0LX20220922";
            string accessKey = "5DuZliGMfIjEIiQs";
            string serectkey = "JREEY5yK0azGS6YJZV5LQXB2wYR9SN8J";
            string orderInfo = "BiOrderInfo";
            string redirectUrl = "https://localhost:44347/shop/InvoiceConfirmed";
            string ipnUrl = "https://localhost:44347/shop/InvoiceConfirmed";
            string requestType = "captureWallet";

            string amount = "400000";
            string orderId = Guid.NewGuid().ToString();
            string requestId = Guid.NewGuid().ToString();
            string extraData = "";

            //Before sign HMAC SHA256 signature
            string rawHash = "accessKey=" + accessKey +
                "&amount=" + amount +
                "&extraData=" + extraData +
                "&ipnUrl=" + ipnUrl +
                "&orderId=" + orderId +
                "&orderInfo=" + orderInfo +
                "&partnerCode=" + partnerCode +
                "&redirectUrl=" + redirectUrl +
                "&requestId=" + requestId +
                "&requestType=" + requestType
                ;

            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "partnerName", "Test" },
                { "storeId", "MomoTestStore" },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderId },
                { "orderInfo", orderInfo },
                { "redirectUrl", redirectUrl },
                { "ipnUrl", ipnUrl },
                { "lang", "en" },
                { "extraData", extraData },
                { "requestType", requestType },
                { "signature", signature }

            };
            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

            JObject jmessage = JObject.Parse(responseFromMomo);
            return Redirect(jmessage.GetValue("payUrl").ToString());
        }
    }
}
