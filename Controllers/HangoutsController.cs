using LunarDeckFoxyApi.Models;
using LunarDeckFoxyApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LunarDeckFoxyApi.Controllers
{
    [Route("api/[controller]")]
    public class HangoutsController : ControllerBase
    {
        private readonly HangoutsServices _hangoutServices;

        public HangoutsController(HangoutsServices services)
        {
            _hangoutServices = services;
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
