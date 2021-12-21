using LunarDeckFoxyApi.Models;
using LunarDeckFoxyApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace LunarDeckFoxyApi.Controllers
{

    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthenticationServices _authServices;
        private readonly LinkGenerator _linkGenerator;

        public AuthController(AuthenticationServices service, LinkGenerator linkGenerator)
        {
            _authServices = service;
            _linkGenerator = linkGenerator;
        }

        /// <summary>
        /// Creates a new user account with provided user information.
        /// Requires: email or phone number in the user object
        /// </summary>
        /// <param name="signUpUser"></param>
        /// <returns>An authenticated user with JWT session token</returns>
        [HttpPost("sign-up")]
        public async Task<ActionResult<User>> CreateNewUser([FromBody] User signUpUser)
        {

            if (signUpUser.Name == null) return BadRequest("User must have a name");

            // check if user exists

            var user = await _authServices.GetUserByEmailCredentialsAsync(signUpUser);

            if (user != null) return BadRequest("User already exists");

            //
            
            try
            {
                await _authServices.CreateAsync(signUpUser);

                var location = _linkGenerator.GetPathByAction("LogInUser", "Auth", signUpUser);

                return Created(location,await _authServices.GetUserByEmailCredentialsAsync(signUpUser));
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("log-in")]
        public async Task<ActionResult<User>> LogInUser([FromBody] User loginUser)
        {
            if (loginUser.Email == null && loginUser.PhoneNumber == null) 
                return BadRequest("Email or phone number is required to log in");

            try
            {
                // if email available, log in with email
                if (loginUser.Email != null)
                {
                    var authenticatedUser = await _authServices.GetUserByEmailCredentialsAsync(loginUser);

                    // TODO: create and add JWT token to authenticated user

                    if (authenticatedUser != null) return Ok(authenticatedUser);
                }

                // if phone number available, log in
                if (loginUser.PhoneNumber != null)
                {
                    var authenticatedUser = await _authServices.GetUserByPhoneNumberCredentialsAsync(loginUser);

                    // TODO: create and add JWT token to authenticated user

                    if (authenticatedUser != null) return Ok(authenticatedUser);
                }

                return BadRequest("Unsuccessful log in with email or phone number. Try again");

            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
