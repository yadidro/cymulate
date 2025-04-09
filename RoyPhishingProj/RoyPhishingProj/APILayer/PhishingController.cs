using Microsoft.AspNetCore.Mvc;
using RoyPhishingProj.BusinessLogicLayer;
using System.Net;
using System.Text.Json;
using RoyPhishingProj.BusinessLogicLayer.request;
using Microsoft.AspNetCore.Authorization;

namespace RoyPhishingProj.APILayer
{
    [ApiController]
    [Route("phishing")]
    public class PhishingController : ControllerBase
    {
        private readonly IPhishingEmailService _phishingEmailService;

        public PhishingController(IPhishingEmailService phishingEmailService)
        {
            _phishingEmailService = phishingEmailService;
        }

        [HttpPost("send")]
        [ProducesResponseType(typeof(ResponseData), (int)HttpStatusCode.OK)]
        //[Authorize]  //on comment since jwt not working for some reason
        public async Task<IActionResult> SendPhishingEmailAsync([FromBody] PhishingRequest request)
        {
            var result = await _phishingEmailService.SendPhishingEmailAsync(request.Name, request.Email);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            var response = new ResponseData() { Response = "Email sent!" };
            return Ok(response);
        }

        [HttpPost("Update")]
        [ProducesResponseType(typeof(ResponseData), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdatePhishingEmailClickedAsync([FromQuery] string hashEmail, [FromQuery] string name)
        {
            if (string.IsNullOrEmpty(hashEmail) || string.IsNullOrEmpty(name))
            {
                return BadRequest("Invalid request parameters.");
            }

            var result = await _phishingEmailService.UpdatePhishingEmailClickedAsync(name, hashEmail);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            var response = new ResponseData(){Response = "Update is done!" };
            return Ok(response);
        }

        [HttpGet]
       // [Authorize] 
        public async Task<IActionResult> GetAllPhishingAttempts()
        {
            var result = await _phishingEmailService.GetAllPhishingAttempts();
           
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            var jsonString = JsonSerializer.Serialize(result.Data);

            var response = new ResponseData() { Response = jsonString };
            return Ok(response);
        }
    }
}