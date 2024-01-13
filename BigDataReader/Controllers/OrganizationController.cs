using BigDataReader.Models;
using BigDataReader.Services.OrganizationService;

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
        public async Task<IActionResult> UploadAsync([FromBody] List<OrganizationModel> data)
        {
            var succeeded = await _organizationService.UploadAsync(data);
            if (!succeeded)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete("{organizationId}")]
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