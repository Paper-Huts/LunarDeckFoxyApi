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
        //private readonly LinkGenerator _linkGenerator;
        private readonly IConfiguration _configuration;

        public AuthController(AuthenticationServices service, LinkGenerator linkGenerator, IConfiguration configuration)
        {
            _authServices = service;
            //_linkGenerator = linkGenerator;
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
        public async Task<ActionResult<UserModel>> CreateNewUser([FromBody] AuthModel signUpUser)
        {
            // if user does not have a name, retuen bad request
            if (signUpUser.Name == "") return BadRequest("User must have a name");

            // if user does not provide passwords or provides a short password
            if (signUpUser.Password.Length < 8 || signUpUser.Password == "") return BadRequest("User must provide a valid password");

            // if user passwords do not match, return bad request
            if (signUpUser.Password != signUpUser.ConfirmPassword) return BadRequest("Passwords must match");

            // if user does not exist, create new user
            var newUser = new UserModel()
            {
                Name = signUpUser.Name,
                Email = signUpUser.Email ?? "",
                PhoneNumber = signUpUser.PhoneNumber ?? "",
                PasswordHash = Encryptor.MD5Hash(signUpUser.Password)
            }; 

            // check if user exists
            var userCheck = new UserModel();

            if (newUser.Email != "")
            {
                userCheck = await _authServices.GetUserByEmailCredentialsAsync(newUser);
            }

            if (newUser.PhoneNumber != "")
            {
                userCheck = await _authServices.GetUserByPhoneNumberCredentialsAsync(newUser);
            }

            if (userCheck != null) return BadRequest("User already exists");

            try
            {
                UserModel newAuthenticatedUser = null;

                await _authServices.CreateAsync(newUser);

                // if user signed up with email
                if (newUser.Email != "")
                {
                    newAuthenticatedUser = await _authServices.GetUserByEmailCredentialsAsync(newUser);
                }

                // if user signed up with phone number
                if (newUser.PhoneNumber != "")
                {
                    newAuthenticatedUser = await _authServices.GetUserByPhoneNumberCredentialsAsync(newUser);
                }

                if (newAuthenticatedUser != null)
                {
                    return Ok(new UserModel()
                    {
                        Id = newAuthenticatedUser.Id,
                        Name = newAuthenticatedUser.Name,
                        Email = newAuthenticatedUser.Email ?? "",
                        PhoneNumber = newAuthenticatedUser.PhoneNumber ?? "",
                        JwtToken = new JwtTokenBuilder(_configuration).Build(newAuthenticatedUser),
                        CreatedAt = newAuthenticatedUser.CreatedAt,
                        LastUpdatedAt = newAuthenticatedUser.LastUpdatedAt,
                        LunarId = newAuthenticatedUser.LunarId ?? "",
                    });
                }

                return BadRequest("Sign up unsuccessful. Please try again");
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
        public async Task<ActionResult<UserModel>> LogInUser([FromBody] AuthModel loginUser)
        {
            // if user does not provide passwords or provides a short password
            if (loginUser.Password.Length < 8 || loginUser.Password == "") return BadRequest("User must provide an acceptable password");

            // created new user model
            var userCheck = new UserModel()
            {
                Email = loginUser.Email ?? "",
                PhoneNumber = loginUser.PhoneNumber ?? "",
                PasswordHash = Encryptor.MD5Hash(loginUser.Password)
            };

            try
            {
                UserModel authenticatedUser = null;

                // if email available
                if (loginUser.Email != "")
                {
                    authenticatedUser = await _authServices.GetUserByEmailCredentialsAsync(userCheck);

                    // check if correct password was entered
                    //if (authenticatedUser.PasswordHash != loginPWHash)
                    //    return BadRequest("Incorrect user password");
                }

                // if phone number available
                if (loginUser.PhoneNumber != "")
                {

                    // get user from DB
                    authenticatedUser = await _authServices.GetUserByPhoneNumberCredentialsAsync(userCheck);

                    // check if correct password was entered
                    //if (authenticatedUser.PasswordHash != loginPWHash)
                    //    return BadRequest("Incorrect user password");
                }

                // add JWT token to authenticated user
                if (authenticatedUser != null)
                {
                    return Ok(new UserModel()
                    {
                        Id = authenticatedUser.Id,
                        Name = authenticatedUser.Name,
                        Email = authenticatedUser.Email ?? "",
                        PhoneNumber = authenticatedUser.PhoneNumber ?? "",
                        JwtToken = new JwtTokenBuilder(_configuration).Build(authenticatedUser),
                        CreatedAt = authenticatedUser.CreatedAt,
                        LastUpdatedAt = authenticatedUser.LastUpdatedAt,
                        LunarId = authenticatedUser.LunarId ?? "",
                    });
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
