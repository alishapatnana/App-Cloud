using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetApp_API.Models;
using TweetApp_API.Models.Dtos.UserDto;
using TweetApp_API.Services;

namespace TweetApp_API.Controllers
{
    [Route("api/v1.0/tweets")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserCredentials credentials)
        {
            var response = await _userService.UserLogin(credentials);
            if (response != null)
            {
                return Ok(response);
            }
            return StatusCode(404, new { errorMessage = "Invalid Credentials!" });
        }

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="userDetail"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto userDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool? isEmailAlreadyExist = await _userService.IsEmailIdAlreadyExist(userDetail.EmailId);
            if (isEmailAlreadyExist != null && isEmailAlreadyExist == true)
            {
                return StatusCode(400, new { error = $"{userDetail.EmailId} is already registered." });
            }
            else if (isEmailAlreadyExist == null)
            {
                return StatusCode(500, new { error = "Some internal error occurred!" });
            }
            var response = await _userService.RegisterUserAsync(userDetail);
            if (response == false)
            {
                return StatusCode(500, new { error = $"Some internal error occurred while registering the userid {userDetail.EmailId}" });
            }
            return StatusCode(201, new { userDetail.EmailId });
        }

        /// <summary>
        /// Get list of all users
        /// </summary>
        /// <returns></returns>
        [HttpGet("users/all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _userService.GetAllUsersAsync();
            return Ok(response);
        }

        /// <summary>
        /// Get single user detail
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet("user/search/{userName}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(string userName)
        {
            var response = await _userService.GetUserAsync(userName);
            if (response != null) { return Ok(response); }
            return StatusCode(404, new { error = $"{userName} not found!" });
        }

        /// <summary>
        /// Reset password
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="credentials"></param>
        /// <returns></returns>
        [HttpPut("resetPassword/{userId}")]
        public async Task<IActionResult> ResetPassword(string userId, [FromBody] ResetPasswordDto credentials)
        {
            var isUserExist = await _userService.IsEmailIdAlreadyExist(userId);
            if (isUserExist != null && isUserExist == true)
            {
                var response = await _userService.UserLogin(new UserCredentials { EmailId = userId, Password = credentials.OldPassword });
                if (response == null)
                {
                    var result = await _userService.ResetPassword(userId, credentials.NewPassword);
                    if (result)
                    {
                        return Ok(new { Message = "Passord reset successfull" });
                    }
                    else
                    {
                        return StatusCode(500, new { errorMessage = "Some internal error occured!" });
                    }
                }

            }
            return StatusCode(404, new { errorMessage = "UserId doesn't exist!" });
        }

        /// <summary>
        /// Verify Security credentials
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        [HttpPut("forgot")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto credentials)
        {
            var isUserExist = await _userService.IsEmailIdAlreadyExist(credentials.EmailId);
            if (isUserExist != null && isUserExist == true)
            {
                var response = await _userService.ValidateSecurityCredential(credentials);
                return Ok(response);
            }
            return BadRequest();
        }

        /// <summary>
        /// Reset Forgotted Password
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        [HttpPut("reset/{userId}")]
        public async Task<IActionResult> UpdatePassword(string userId, [FromBody] ResetPasswordDto newPassword)
        {
            var result = await _userService.ResetPassword(userId, newPassword.NewPassword);
            return Ok(result);
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userDetail"></param>
        /// <returns></returns>
        [HttpPut("updateProfile/{userId}", Name = "UpdateProfile")]
        public async Task<IActionResult> UpdateProfile(string userId, [FromBody] CreateUserDto userDetail)
        {
            var result = await _userService.UpdateUser(userId, userDetail);
            return Ok(result);
        }

        /// <summary>
        /// Get users with similar search Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("users/search/{id}", Name = "GetUsersById")]
        public async Task<IActionResult> GetUsersById(string id)
        {
            var response = await _userService.GetUsersByIdAsync(id);
            if (response != null) { return Ok(response); }
            return StatusCode(404, new { error = $"{id} not found!" });
        }
        /// <summary>
        /// Send Mail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("users/mail/{id}", Name = "SendMail")]
        public async Task<IActionResult> SendMail(string id)
        {
            var response = await _userService.SendMail(id);
            if (response != null) { return Ok(response); }
            return StatusCode(404, new { error = $"{id} email not send!" });
        }
    }
}
