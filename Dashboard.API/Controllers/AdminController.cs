using DomainLayer.DashboardModule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceAbstraction.Services.DashboardServices;

namespace Dashboard.API.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IDashboardAdminService _adminService;

        public AdminController(IDashboardAdminService adminService)
        {
            _adminService = adminService;
        }
     
        // ─── Pending Applications ───────────────────────────

        [HttpGet("applications/restaurants")]
        public async Task<IActionResult> GetPendingRestaurants()
        {
            var result = await _adminService.GetPendingAsync("Restaurant");
            return Ok(result);
        }

        [HttpGet("applications/hotels")]
        public async Task<IActionResult> GetPendingHotels()
        {
            var result = await _adminService.GetPendingAsync("Hotel");
            return Ok(result);
        }

        [HttpGet("applications/tourguides")]
        public async Task<IActionResult> GetPendingTourGuides()
        {
            var result = await _adminService.GetPendingAsync("TourGuide");
            return Ok(result);
        }

        [HttpPost("applications/{id}/approve")]
        public async Task<IActionResult> Approve(int id)
        {
            await _adminService.ApproveAsync(id);
            return Ok(new { message = "Application approved." });
        }

        [HttpPost("applications/{id}/reject")]
        public async Task<IActionResult> Reject(int id)
        {
            await _adminService.RejectAsync(id);
            return Ok(new { message = "Application rejected." });
        }

        // ─── Approved Restaurants ───────────────────────────

        [HttpGet("restaurants")]
        public async Task<IActionResult> GetRestaurants(
            string? search, int page = 1, int pageSize = 10)
        {
            var result = await _adminService.GetApprovedAsync(
                "Restaurant", search, page, pageSize);
            return Ok(result);
        }

        [HttpPut("restaurants/{id}/hide")]
        public async Task<IActionResult> HideRestaurant(int id)
        {
            await _adminService.HideAsync(id);
            return Ok(new { message = "Visibility toggled." });
        }

        [HttpDelete("restaurants/{id}")]
        public async Task<IActionResult> DeleteRestaurant(int id)
        {
            await _adminService.DeleteAsync(id);
            return Ok(new { message = "Restaurant deleted." });
        }

        // ─── Approved Hotels ────────────────────────────────

        [HttpGet("hotels")]
        public async Task<IActionResult> GetHotels(
            string? search, int page = 1, int pageSize = 10)
        {
            var result = await _adminService.GetApprovedAsync(
                "Hotel", search, page, pageSize);
            return Ok(result);
        }

        [HttpPut("hotels/{id}/hide")]
        public async Task<IActionResult> HideHotel(int id)
        {
            await _adminService.HideAsync(id);
            return Ok(new { message = "Visibility toggled." });
        }

        [HttpDelete("hotels/{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            await _adminService.DeleteAsync(id);
            return Ok(new { message = "Hotel deleted." });
        }

        // ─── Tour Guides ─────────────────────────────────────

        [HttpGet("tourguides")]
        public async Task<IActionResult> GetTourGuides(
            string? search, int page = 1, int pageSize = 10)
        {
            var result = await _adminService.GetApprovedAsync(
                "TourGuide", search, page, pageSize);
            return Ok(result);
        }

        [HttpPut("tourguides/{id}/hide")]
        public async Task<IActionResult> HideTourGuide(int id)
        {
            await _adminService.HideAsync(id);
            return Ok(new { message = "Visibility toggled." });
        }

        [HttpDelete("tourguides/{id}")]
        public async Task<IActionResult> DeleteTourGuide(int id)
        {
            await _adminService.DeleteAsync(id);
            return Ok(new { message = "Tour guide deleted." });
        }
    }
}
