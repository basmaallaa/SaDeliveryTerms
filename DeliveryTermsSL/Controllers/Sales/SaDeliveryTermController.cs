using DeliveryTermsBL.IServices.ISalesService;
using DeliveryTermsBL.Models.Sales;
using DeliveryTermsSL.Filters;
using DeliveryTermsSL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryTermsSL.Controllers.Sales
{
    [Authorize]
    [Route("api/Sales/[controller]")]
    [ApiController]
    [AuthorizeCompany]
    [ApiExplorerSettings(GroupName = "Sales")]
    public class SaDeliveryTermController : ControllerBase
    {
        private readonly ISaDeliveryTermService _service;
        private readonly ICurrentUserService _currentUserService;

        public SaDeliveryTermController(ISaDeliveryTermService service, ICurrentUserService currentUserService)
        {
            _service = service;
            _currentUserService = currentUserService;
        }

        private bool TryGetCurrentUserId(out int userId)
        {
            userId = _currentUserService.UserId ?? 0;
            return userId > 0;
        }

        [HttpGet("GetSaDeliveryTerms")]
        public IActionResult GetSaDeliveryTerms([FromQuery] string culture = "en")
        {
            return Ok(_service.GetAll(culture));
        }

        [HttpGet("GetSaDeliveryTerm/{code:int}")]
        public IActionResult GetSaDeliveryTerm(int code)
        {
            return Ok(_service.GetByCode(code));
        }

        [HttpPost("AddSaDeliveryTerm")]
        public IActionResult AddSaDeliveryTerm(SaDeliveryTermModel model)
        {
            if (!TryGetCurrentUserId(out var userId)) return Unauthorized();

            return Ok(_service.Add(model, userId));
        }

        [HttpPut("EditSaDeliveryTerm")]
        public IActionResult EditSaDeliveryTerm(SaDeliveryTermModel model)
        {
            if (!TryGetCurrentUserId(out var userId)) return Unauthorized();
            return Ok(_service.Edit(model, userId));
        }

        [HttpDelete("DeleteSaDeliveryTerm/{code:int}")]
        public IActionResult DeleteSaDeliveryTerm(int code)
        {
            if (!TryGetCurrentUserId(out _)) return Unauthorized();
            return Ok(_service.Delete(code));
        }
    }
}
