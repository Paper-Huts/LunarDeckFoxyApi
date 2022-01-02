using LunarDeckFoxyApi.Models;
using LunarDeckFoxyApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LunarDeckFoxyApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class HangoutsController : ControllerBase
    {
        private readonly HangoutsServices _hangoutServices;
        //private readonly LinkGenerator _linkGenerator;

        public HangoutsController(HangoutsServices services, LinkGenerator linkGenerator)
        {
            _hangoutServices = services;
            //_linkGenerator = linkGenerator;
        }

        /// <summary>
        /// Gets all hangouts
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<HangoutModel>>> Get()
        {

            try
            {
                var hangouts = await _hangoutServices.GetAsync();

                return Ok(hangouts);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Gets a hangout by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A hangout</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<HangoutModel>> Get(string id)
        {
            try
            {
                HangoutModel model = await _hangoutServices.GetAsync(id);

                if (model == null) return NotFound("Hangout not found.");
                
                return Ok(model);
                
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// Creates a new hangout
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Status code and link to new hangout if successful</returns>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] HangoutModel model)
        {

            if (model.Name == "")
            {
                return BadRequest("A new hangout must have a name");
            }

            try
            {
                await _hangoutServices.CreateAsync(model);
                return Ok(model);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
