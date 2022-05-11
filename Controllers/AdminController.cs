using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;

        }
        [Authorize(Policy = "RequiredAdminRole")]
        [HttpGet("user-with-roles")]
        public async Task<ActionResult> GetUserWithRoles()
        {
            var users = await __userManager.Users
                .Include(r => UserRole)
                .ThenInclude(r => Role)
                .OrderBy(u => Username)
                .Select(u => new {
                    u.Id,
                    Username= u.Username,
                    Roles = u.UserRole.Select(r => Role.Name).ToList()
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpPost("edit-role/{username}")]
        public async Task<ActionResult> EditRoles(string username, [FromQuery] string roles) {
            var selectRoles =roles.Split(",").ToArray();
            
            var user = await _userManager.FindByNameAsync(username);

            if(user == null) return NotFound("COuld not find user");

            var userRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user, selectRoles.Except(userRoles));

            if (!result.Succeeded) return BadRequest("Failed to add to roles");

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectRoles));

            if(!result.Succeeded) return BadRequest("Failed to remove from roles");

            return Ok(await _userManager.GetRolesAsync(user));
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photo-to-moderate")]
        public ActionResult GetPhotosForModeration()
        {
            return Ok("Admins or moderators can see this");
        }
    }
}