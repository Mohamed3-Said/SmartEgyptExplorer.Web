using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction.Services.DashboardServices;
using Shared.DTOS.DashboardDTOs;
using Shared.DTOS.DashboardDTOs.DashboardAuthDTOs;
using Shared.DTOS.DashboardDTOs.OwnerDTOs;
using System.Security.Claims;

namespace Dashboard.API.Controllers
{
    [ApiController]
    [Route("api/owner")]
    [Authorize(Roles = "Owner")]
    public class OwnerController : ControllerBase
    {
        private readonly IDashboardOwnerService _ownerService;
        private readonly IWebHostEnvironment _env;

        public OwnerController(IDashboardOwnerService ownerService, IWebHostEnvironment env)
        {
            _ownerService = ownerService;
            _env = env;
        }

        private int GetOwnerId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // GET api/owner/status
        [HttpGet("status")]
        public async Task<IActionResult> GetStatus()
        {
            var result = await _ownerService.GetStatusAsync(GetOwnerId());
            return Ok(result);
        }

        // GET api/owner/profile
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var result = await _ownerService.GetProfileAsync(GetOwnerId());
            return Ok(result);
        }

        // GET api/owner/services
        [HttpGet("services")]
        public async Task<IActionResult> GetServices()
        {
            var result = await _ownerService.GetServicesAsync(GetOwnerId());
            return Ok(result);
        }

        // POST api/owner/services
        [HttpPost("services")]
        public async Task<IActionResult> AddService(
            [FromForm] OwnerServiceCreateDto dto,
            IFormFile? image)
        {
            string? imageUrl = null;
            if (image != null && image.Length > 0)
                imageUrl = await SaveFileAsync(image, "services");

            var result = await _ownerService.AddServiceAsync(GetOwnerId(), dto, imageUrl);
            return Ok(result);
        }

        // PUT api/owner/services/{id}
        [HttpPut("services/{id}")]
        public async Task<IActionResult> UpdateService(
            int id,
            [FromForm] OwnerServiceCreateDto dto,
            IFormFile? image)
        {
            string? imageUrl = null;
            if (image != null && image.Length > 0)
                imageUrl = await SaveFileAsync(image, "services");

            await _ownerService.UpdateServiceAsync(GetOwnerId(), id, dto, imageUrl);
            return Ok(new { message = "Service updated." });
        }

        // DELETE api/owner/services/{id}
        [HttpDelete("services/{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            await _ownerService.DeleteServiceAsync(GetOwnerId(), id);
            return Ok(new { message = "Service deleted." });
        }

        [HttpPut("profile/photo")]
        public async Task<IActionResult> UploadPhoto(IFormFile photo)
        {
            var ownerId = GetOwnerId();

            var photoUrl = await SaveFileAsync(photo, "photos");

            await _ownerService.UpdatePhotoAsync(ownerId, photoUrl);

            return Ok(new { photoUrl });
        }

        [HttpPut("hotel-details")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> UpdateHotelDetails([FromBody] HotelDetailsUpdateDto dto)
        {
            var userId = GetOwnerId();

            await _ownerService.UpdateHotelDetailsAsync(userId, dto);

            return Ok(new { message = "Hotel details updated successfully" });
        }
        private async Task<string> SaveFileAsync(IFormFile file, string folder)
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", folder);
            Directory.CreateDirectory(uploadsFolder);
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
            return $"/uploads/{folder}/{fileName}";
        }
    }
}
