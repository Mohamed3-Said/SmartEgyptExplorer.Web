using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction.Services.DashboardServices;
using ServiceAbstraction.Services.IMobileService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/tourguides")]
    public class TourGuidesController : ControllerBase
    {
        private readonly ITourGuideService _service;

        public TourGuidesController(ITourGuideService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetTourGuides()
        {
            var result = await _service.GetTourGuidesAsync();

            return Ok(result);
        }
    }
}
