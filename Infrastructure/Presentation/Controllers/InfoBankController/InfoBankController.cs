using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction.Services.InfoBankIService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers.InfoBankController
{
  //  [Authorize]
    [ApiController]
    [Route("api/infobank")]
    public class InfoBankController : ControllerBase
    {
        private readonly IAttractionService _attractionService;
        private readonly IHotelService _hotelService;
        private readonly IRestaurantService _restaurantService;
        private readonly IFoodRecipeService _foodRecipeService;
        public InfoBankController(
            IAttractionService attractionService,
            IHotelService hotelService,
            IRestaurantService restaurantService,
            IFoodRecipeService foodRecipeService)
        {
            _attractionService = attractionService;
            _hotelService = hotelService;
            _restaurantService = restaurantService;
            _foodRecipeService = foodRecipeService;
        }

        // GET api/infobank/attractions?city=Cairo&category=museum
        [HttpGet("attractions")]
        public async Task<IActionResult> GetAttractions(
    int page = 1,
    int pageSize = 10,
    string? city = null,
    string? category = null)
        {
            var result = await _attractionService.GetAllAsync(city, category, page, pageSize);
            return Ok(result);
        }

        // GET api/infobank/attractions/1
        [HttpGet("attractions/{id}")]
        public async Task<IActionResult> GetAttraction(int id)
        {
            var attraction = await _attractionService.GetByIdAsync(id);
            if (attraction == null) return NotFound();
            return Ok(attraction);
        }

        // GET api/infobank/hotels?city=Cairo
        [HttpGet("hotels")]
        public async Task<IActionResult> GetHotels(
            int page = 1,
            int pageSize = 10,
            string? city = null)
        {
            var result = await _hotelService.GetHotelsAsync(page, pageSize, city);
            return Ok(result);
        }

        // GET api/infobank/hotels/1
        [HttpGet("hotels/{id}")]
        public async Task<IActionResult> GetHotel(int id)
        {
            var hotel = await _hotelService.GetByIdAsync(id);
            if (hotel == null) return NotFound();
            return Ok(hotel);
        }


        // GET api/infobank/restaurants?city=Cairo&category=seafood
        [HttpGet("restaurants")]
        public async Task<IActionResult> GetRestaurants(
            int page = 1, int pageSize = 10,
            string? city = null, string? category = null)
        {
            var result = await _restaurantService.GetAllAsync(city, category, page, pageSize);
            return Ok(result);
        }

        // GET api/infobank/recipes?category=vegan
        [HttpGet("recipes")]
        public async Task<IActionResult> GetRecipes(
            int page = 1, int pageSize = 10,
            string? category = null)
        {
            var result = await _foodRecipeService.GetAllAsync(category, page, pageSize);
            return Ok(result);
        }

        // GET api/infobank/recipes/1
        [HttpGet("recipes/{id}")]
        public async Task<IActionResult> GetRecipe(int id)
        {
            var recipe = await _foodRecipeService.GetByIdAsync(id);
            if (recipe == null) return NotFound();
            return Ok(recipe);
        }

    }
}
