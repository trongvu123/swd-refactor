using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SonicStore.Business.Service.PromotionService;
using SonicStore.Repository.Entity;

namespace SonicStore.Areas.SonicStore.Controllers.PromotionManage
{
    [Area("SonicStore")]
    public class PromotionController : Controller
    {
        private readonly IPromotionService _promotionService;

        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        [HttpGet("promotion")]
        public async Task<IActionResult> PromotionScreen()
        {
            var userJson = HttpContext.Session.GetString("user");
            var userSession = JsonConvert.DeserializeObject<User>(userJson);

            var promotions = await _promotionService.GetListPromotionByUser(userSession.Id);

            return View(promotions);
        }

        [HttpGet("promotion/add")]
        public IActionResult AddPromotion()
        {
            return View();
        }

        [HttpPost("promotion/add")]
        public async Task<IActionResult> AddPromotion(Promotion promotion)
        {
            var userJson = HttpContext.Session.GetString("user");
            var userSession = JsonConvert.DeserializeObject<User>(userJson);
            if (ModelState.IsValid)
            {
 

                promotion.CreatedBy = userSession.Id;
                promotion.UpdatedBy = userSession.Id;
                promotion.CreatedAt = DateTime.Now;
                promotion.UpdatedAt = DateTime.Now;

                await _promotionService.AddPromotion(promotion);
                return RedirectToAction("PromotionScreen");
            }
            return View(promotion);
        }

        [HttpGet("promotion/edit/{id}")]
        public async Task<IActionResult> EditPromotion(int id)
        {
            var promotion = await _promotionService.FindPromotionById(id);
            if (promotion == null)
            {
                return NotFound();
            }


            return View(promotion);
        }

        [HttpPost("promotion/edit/{id}")]
        public async Task<IActionResult> EditPromotion(int id, Promotion promotion)
        {
            if (id != promotion.PromotionId)
            {
                return BadRequest();
            }
            var userJson = HttpContext.Session.GetString("user");
            var userSession = JsonConvert.DeserializeObject<User>(userJson);
            if (ModelState.IsValid)
            {

                promotion.UpdatedBy = userSession.Id;
                promotion.UpdatedAt = DateTime.Now;

                await _promotionService.UpdatePromotion(promotion);
                return RedirectToAction("PromotionScreen");
            }
            return View(promotion);
        }

        [HttpGet("promotion/delete/{id}")]
        public async Task<IActionResult> DeletePromotion(int id)
        {
            var promotion = await _promotionService.FindPromotionById(id);
            if (promotion == null)
            {
                return NotFound();
            }

            var userJson = HttpContext.Session.GetString("user");
            var userSession = JsonConvert.DeserializeObject<User>(userJson);

            await _promotionService.DeletePromotion(id);
            return RedirectToAction("PromotionScreen");
        }
    }
}