using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Register.Api.Dto.Request;
using Register.Api.Services.Interfaces;

namespace Register.Api.Controllers
{
    //[Authorize]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class ClientAppController : Controller
    {
        readonly IClientService _clientAppService;

        public ClientAppController(IClientService clientAppService)
        {
            _clientAppService = clientAppService;
        }

        [HttpPost]
        public dynamic AddClientForApp([FromBody] CreateUserRequestDto createUserRequestDto)
        {
            try
            {
                var result = _clientAppService.CreateAccountUser(createUserRequestDto);

                return Ok(result);
            }
            catch (Exception ex)
            {
                //TODO: remover o OK
                return BadRequest($"ERROR \n {ex.Message} \n {ex.StackTrace}");
            }
        }

        [HttpPost]
        [Authorize]
        public dynamic UpdateAddressClient([FromBody] ClientShippingAddressRequestDto clientShippingAddressRequestDto)
        {
            try
            {
                var result = _clientAppService.UpdateAccountWithAddress(clientShippingAddressRequestDto);

                return Ok(result);
            }
            catch (Exception ex)
            {
                //TODO: remover o OK
                return BadRequest($"ERROR \n {ex.Message} \n {ex.StackTrace}");
            }
        }

        [HttpGet]
        [Authorize]
        public dynamic Client()
        {
            try
            {
                var result = _clientAppService.GetClient();

                return Ok(result);
            }
            catch (Exception ex)
            {
                //TODO: remover o OK
                return BadRequest($"ERROR \n {ex.Message} \n {ex.StackTrace}");
            }
        }
    }
}
