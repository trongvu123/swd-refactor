﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SonicStore.Business.Dto;
using SonicStore.Business.Service.CheckoutService;
using SonicStore.Business.Service.VnPayService;
using SonicStore.Repository.Entity;
namespace SonicStore.Areas.SonicStore.Controllers.CheckoutManage
{
    [Authorize(Roles = "customer")]
    [Area("SonicStore")]
    public class CheckoutController : Controller
    {
        private readonly ICheckoutService _checkoutService;
        private readonly IVnPayService _vnPayService;
        public CheckoutController(
            ICheckoutService checkoutService,
            IVnPayService vnPayService)
        {
            _checkoutService = checkoutService;
            _vnPayService = vnPayService;
        }

        [HttpGet("checkout")]
        public async Task<IActionResult> CheckoutScreen()
        {
            var userJson = HttpContext.Session.GetString("user");
            if (string.IsNullOrEmpty(userJson))
                return RedirectToAction("Login", "Account", new { area = "" });

            var user = JsonConvert.DeserializeObject<User>(userJson);
            var address = await _checkoutService.GetUserAddressActive(user.Id);

            var listJson = HttpContext.Session.GetString("listCheckout");
            if (string.IsNullOrEmpty(listJson))
                return View(new List<Cart>());

            var cartIds = JsonConvert.DeserializeObject<List<int>>(listJson);
            var cartItems = await _checkoutService.GetCartIncludeInventoryAndProductHaveAddressCondition(cartIds);

            ViewBag.totalPrice = cartItems.Sum(i => i.Quantity * i.Storage.SalePrice);
            ViewBag.userSession = user;
            ViewBag.addressUser = address;
            return View(cartItems);
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromForm] CheckoutRequestDto request)
        {
            var result = await _checkoutService.ProcessCheckoutAsync(request, HttpContext);

            if (!result.Success)
            {
                TempData["Message"] = result.Message;
                return View("CheckoutScreen");
            }

            if (!string.IsNullOrEmpty(result.PaymentUrl))
                return Redirect(result.PaymentUrl);

            TempData["Message"] = result.Message;
            return View("PaymentCallBack");
        }

        //[HttpPost("buy-now")]
        //public async Task<IActionResult> BuyNow([FromBody] BuyNowRequestDto request)
        //{
        //    var result = await _checkoutService.ProcessBuyNowAsync(request, HttpContext);

        //    if (!result.Success)
        //        return Json(new { status = false, message = result.Message });

        //    if (!string.IsNullOrEmpty(result.PaymentUrl))
        //        return Json(new { status = true, url = result.PaymentUrl });

        //    return Json(new { status = true });
        //}

        [HttpPost("checkout-order")]
        public async Task<IActionResult> CheckOutOrder([FromForm] string payment)
        {
            if (payment == "cod")
            {
                var listJson = HttpContext.Session.GetString("listCheckout");
                if (!string.IsNullOrEmpty(listJson))
                {
                    var userJson = HttpContext.Session.GetString("user");
                    var user = JsonConvert.DeserializeObject<Repository.Entity.User>(userJson);
                    var cartIds = JsonConvert.DeserializeObject<List<int>>(listJson);
                    await _checkoutService.ProcessPaymentCODSuccessAsync(cartIds, user.Id.ToString(), "COD");
                    HttpContext.Session.Remove("listCheckout");
                     HttpContext.Session.CommitAsync();
                }
                //return View("CheckoutCODScreen");
                TempData["Message"] = "Đơn hàng của bạn đang chờ duyệt";
                return View("PaymentCallBack");
            }
            else
            {



                var listJson = HttpContext.Session.GetString("listCheckout");
                List<Cart> listToCheckout = new List<Cart>();
                double Amount = 0;
                var listSession = JsonConvert.DeserializeObject<List<int>>(listJson);
                foreach (var item in _checkoutService.GetCartIncludeInventoryAndProduct())
                {
                    foreach (var i in listSession)
                    {
                        if (item.Id == i)
                        {
                            Amount += (int)item.Quantity * (double)item.Storage.SalePrice;

                        }
                    }
                }
                var _user = HttpContext.Session.GetString("user");
                Repository.Entity.User user = JsonConvert.DeserializeObject<User>(_user);
                var model = new VnPayRequestModel
                {
                    Fullname = user.FullName,
                    Description = "index",
                    Amount = Amount,
                    CreatedDate = DateTime.Now,
                    OrderId = 1

                };


                return Redirect(_vnPayService.CreatePaymentURL(HttpContext, model));
            }
        }

        [HttpGet("PaymentFailMessage")]
        public IActionResult PaymentFail()
        {
            return View();
        }

        [HttpGet("/Cart/PaymentCallBack")]
        public async Task<IActionResult> PaymentCallBack()
        {
            if (string.IsNullOrEmpty(Request.QueryString.ToString()))
                return RedirectToAction("ErrorScreen", "Error");

            var response = _vnPayService.PaymentExecute(Request.Query);
            if (response == null || response.VnPayResponseCode != "00")
            {
                TempData["Message"] = "Thanh toán không thành công";
                return RedirectToAction("PaymentFail");
            }

            var userJson = HttpContext.Session.GetString("user");
            if (string.IsNullOrEmpty(userJson))
                return RedirectToAction("Login", "Account", new { area = "" });

            var user = JsonConvert.DeserializeObject<Repository.Entity.User>(userJson);

            var listJson = HttpContext.Session.GetString("listCheckout");
            if (!string.IsNullOrEmpty(listJson))
            {
                var cartIds = JsonConvert.DeserializeObject<List<int>>(listJson);
                await _checkoutService.ProcessPaymentVnPAYSuccessAsync(cartIds, user.Id.ToString(), "VNPAY");
                HttpContext.Session.Remove("listCheckout");
                HttpContext.Session.CommitAsync();
            }


            TempData["Message"] = "Thanh toán thành công";
            return View();
        }
    }
}
