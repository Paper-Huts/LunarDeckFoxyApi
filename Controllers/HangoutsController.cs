using LunarDeckFoxyApi.Models;
using LunarDeckFoxyApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LunarDeckFoxyApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class HangoutsController : ControllerBase
    {
        private readonly HangoutsServices _hangoutServices;
        private readonly IConfiguration _configuration;

        public HangoutsController(HangoutsServices services, IConfiguration configuration)
        {
            _hangoutServices = services;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<List<HangoutModel>>> Get()
        {

            try
            {
                var hangouts = await _hangoutServices.GetAsync();

                return Ok(hangouts);
            }
            catch (System.Exception)
            {

                throw;
            }

            //return Ok(new List<string>{ "hangout 1", "Hangout 2"});
        }
    }
}
