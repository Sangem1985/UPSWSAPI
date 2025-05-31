using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UPSWSWebAPIs.BAL.Implementations;
using UPSWSWebAPIs.BAL.Interfaces;
using static UPSWSWebAPIs.Models.MasterModel;

namespace UPSWSWebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : Controller
    {
        public readonly IMasterBAL _masterBAL;
        public MasterController(IMasterBAL masterBAL)
        {
            _masterBAL = masterBAL;
        }
        [HttpGet("GetAllDistricts")]
      //  [Authorize]
        public async Task<IActionResult> GetAllDistricts()
        {
            try
            {
                var result = await _masterBAL.GetDistricts();
                var resultJson = JsonSerializer.Deserialize<List<District>>(result);
                return Ok(resultJson);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
