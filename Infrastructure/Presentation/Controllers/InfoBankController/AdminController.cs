using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction.Services;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly IDataSeederService _seederService;
    private readonly IWebHostEnvironment _env;

    public AdminController(IDataSeederService seederService, IWebHostEnvironment env)
    {
        _seederService = seederService;
        _env = env;
    }

    [HttpPost("seed-infobank")]
    public async Task<IActionResult> SeedInfoBank(
    IFormFile attractions,
    IFormFile attractionRatings,
    IFormFile hotels,
    IFormFile restaurants,
    IFormFile foodRecipes)
    {
        // حفظ الـ files مؤقتاً في الـ temp folder
        var tempFolder = Path.Combine(Path.GetTempPath(), "csv-seed-" + Guid.NewGuid());
        Directory.CreateDirectory(tempFolder);

        try
        {
            // حفظ كل file بالاسم الصح
            await SaveFile(attractions, tempFolder, "Egypt_Attractions.csv");
            await SaveFile(attractionRatings, tempFolder, "Egypt_Attractions_Ratings.csv");
            await SaveFile(hotels, tempFolder, "Egypt_Hotels.csv");
            await SaveFile(restaurants, tempFolder, "Egyptian_Restaurants.csv");
            await SaveFile(foodRecipes, tempFolder, "Egypt_Food_Recipe_Data.csv");

            var result = await _seederService.SeedAllAsync(tempFolder);
            return Ok(new { message = result });
        }
        finally
        {
            // امسح الـ temp folder بعد الـ seeding
            if (Directory.Exists(tempFolder))
                Directory.Delete(tempFolder, true);
        }
    }

    private async Task SaveFile(IFormFile file, string folder, string fileName)
    {
        var path = Path.Combine(folder, fileName);
        using var stream = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(stream);
    }
}