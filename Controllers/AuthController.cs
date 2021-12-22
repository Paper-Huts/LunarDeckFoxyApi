using LunarDeckFoxyApi.Authentication;
using LunarDeckFoxyApi.Models;
using LunarDeckFoxyApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace LunarDeckFoxyApi.Controllers
{

    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthenticationServices _authServices;
        private readonly LinkGenerator _linkGenerator;
        private readonly IConfiguration _configuration;

        public AuthController(AuthenticationServices service, LinkGenerator linkGenerator, IConfiguration configuration)
        {
            _authServices = service;
            _linkGenerator = linkGenerator;
            _configuration = configuration;
        }

        /// <summary>
        /// Creates a new user account with provided user information.
        /// Requires: email or phone number in the user object
        /// </summary>
        /// <param name="signUpUser"></param>
        /// <returns>An authenticated user with JWT session token</returns>
        [AllowAnonymous]
        [HttpPost("sign-up")]
        public async Task<ActionResult<User>> CreateNewUser([FromBody] User signUpUser)
        {
            // if user does not have a name, retuen bad request
            if (signUpUser.Name == null) return BadRequest("User must have a name");

            // if user does not provide passwords or provides a short password
            //if (signUpUser.Password.Length < 8 || signUpUser.Password == null) return BadRequest("User must provide a password");

            // if user passwords do not match, return bad request
            //if (signUpUser.Password != signUpUser.ConfirmPassword) return BadRequest("Passwords must match");

            // check if user exists
            var user = new User();

            if (signUpUser.Email != null)
            {
                user = await _authServices.GetUserByEmailCredentialsAsync(signUpUser);
            }

            if (signUpUser.PhoneNumber != null)
            {
                user = await _authServices.GetUserByEmailCredentialsAsync(signUpUser);
            }

            if (user != null) return BadRequest("User already exists");

            // if user does not exist, create new user
            
            try
            {
                await _authServices.CreateAsync(signUpUser);

                var newUser = await _authServices.GetUserByEmailCredentialsAsync(signUpUser);

                newUser.JwtToken = new JwtTokenBuilder(_configuration).Build(newUser);

                // only use location for non-auth requests
                //var location = _linkGenerator.GetPathByAction("LogInUser", "Auth", newUser);

                return Ok(newUser);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Checks if user exists using provided email or password in <c>loginUser</c> object
        /// </summary>
        /// <param name="loginUser"></param>
        /// <returns>A user object with JWT token or bad request if unsuccessful</returns>
        [AllowAnonymous]
        [HttpPost("log-in")]
        public async Task<ActionResult<User>> LogInUser([FromBody] User loginUser)
        {

            try
            {
                // if email available, log in with email
                if (loginUser.Email != null)
                {
                    var authenticatedUser = await _authServices.GetUserByEmailCredentialsAsync(loginUser);

                    // get user from DB and add JWT token to authenticated user
                    authenticatedUser.JwtToken = new JwtTokenBuilder(_configuration).Build(authenticatedUser);

                    if (authenticatedUser != null) return Ok(authenticatedUser);
                }

                // if phone number available, log in
                if (loginUser.PhoneNumber != null)
                {
                    var authenticatedUser = await _authServices.GetUserByPhoneNumberCredentialsAsync(loginUser);

                    // get user from DB and add JWT token to authenticated user
                    authenticatedUser.JwtToken = new JwtTokenBuilder(_configuration).Build(authenticatedUser);

                    if (authenticatedUser != null) return Ok(authenticatedUser);
                }

                return BadRequest("Email or phone number is required to log in. Try again");

            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
