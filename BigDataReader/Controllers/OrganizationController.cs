using BigDataReader.Models;
using BigDataReader.Services.OrganizationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BigDataReader.Controllers
{
    public class OrganizationController : ApiController
    {
        private readonly IOrganizationService _organizationService;

        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpGet("{organizationId}")]
        [Authorize]
        public async Task<IActionResult> GetAsync(string organizationId)
        {
            var model = await _organizationService.GetAsync(organizationId);
            if (model is null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpPost("upload")]
        [Authorize]
        public async Task<IActionResult> UploadAsync([FromBody] List<OrganizationModel> data)
        {
            var succeeded = await _organizationService.UploadAsync(data);
            if (!succeeded)
            {
                return BadRequest();
            }

            return Ok();
        }
        [HttpGet("top-organizations")]
        [Authorize]
        public async Task<IActionResult> GetOrganizationsWithMostEmployees()
        {
            var model = await _organizationService.GetOrganizationsWithMostEmployees();
            if (model is null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("employee-by-industry")]
        public async Task<IActionResult> AggregateEmployeeCountByIndustry()
        {
            var model = await _organizationService.AggregateEmployeeCountByIndustry();
            if (model is null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpDelete("{organizationId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(string organizationId)
        {
            var succeeded = await _organizationService.DeleteAsync(organizationId);
            if (!succeeded)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}