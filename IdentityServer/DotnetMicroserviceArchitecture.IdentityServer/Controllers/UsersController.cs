using DotnetMicroserviceArchitecture.Core.Dtos;
using DotnetMicroserviceArchitecture.IdentityServer.DTOs;
using DotnetMicroserviceArchitecture.IdentityServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace DotnetMicroserviceArchitecture.IdentityServer.Controllers
{
    [Authorize(LocalApi.PolicyName)]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpDTO signUpDTO)
        {
            var user = new ApplicationUser
            {
                UserName = signUpDTO.UserName,
                Email = signUpDTO.Email,
                City = signUpDTO.City
            };

            var result = await _userManager.CreateAsync(user, signUpDTO.Password).ConfigureAwait(false);
            if (!result.Succeeded)
                return BadRequest(Response<NoContent>.Fail(result.Errors.Select(x => x.Description).ToList(), (int)HttpStatusCode.NotFound));

            return Ok(Response<NoContent>.Success((int)HttpStatusCode.NoContent));
        }
    }
}
