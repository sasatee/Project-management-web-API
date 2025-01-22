using AuthenticationAPI.DTOs;
using AuthenticationAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AuthenticationAPI.Controllers
{
    //[Authorize(Roles = "Admin")]
    [ApiController]
    [Route("/api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;

        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto createRoleDto)
        {

            if (string.IsNullOrEmpty(createRoleDto.RoleName))
            {
                return BadRequest(new { isSuccess = false, message = "Role name is required." });



            }

            var roleExist = await _roleManager.RoleExistsAsync(createRoleDto.RoleName);

            if (roleExist) {
                return BadRequest(new { isSuccess = false, message = "Role already exist." });
            }
            var roleResult = await _roleManager.CreateAsync(new IdentityRole(createRoleDto.RoleName));

            if (roleResult.Succeeded)
            {
                return Ok(new { isSuccess = true, message = "Role has been created successfully" });
            }

            return BadRequest(new { isSuccess = false, message = "Role has not been created." });

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleReponseDto>>> GetAllRoles()
        {




            var roles = await _roleManager.Roles.ToListAsync();//fetch all roles 
            var roleResponseDtos = new List<RoleReponseDto>();


            foreach (var role in roles)
            {
                //fetch users for each role asynchronously
                var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);

                roleResponseDtos.Add(new RoleReponseDto
                {
                    Id = role.Id,
                    Name = role.Name,
                    TotalUsers = usersInRole.Count
                });
            }
            return Ok(new { isSuccess = true, message = "Get all roles available", data = roleResponseDtos });


        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            //find role by id
            var role = await _roleManager.FindByIdAsync(id);

            if (role is null)
            {
                return NotFound(new { isSuccess = false, message = "Role not found" });
            }

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return Ok(new { message = "Role deleted successfully", isSuccess = true });
            }

            return BadRequest((new { message = "Role not been deleted successfully", isSuccess = false }));

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole([FromBody] CreateRoleDto createRoleDto, string id)
        {
            //find role by id
            var role = await _roleManager.FindByIdAsync(id);

            if (role is null)
            {
                return NotFound(new { isSuccess = false, message = "Role not found" });
            }
            //update the role's name 
            role.Name = createRoleDto.RoleName;

            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                return Ok(new { message = "Role updated successfully", isSuccess = true , role});
            }

            return BadRequest((new { message = "Role not been updated", isSuccess = false }));

        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignRole([FromBody] RoleAssignedDto roleAssignedDto)
        {
            var user = await _userManager.FindByIdAsync(roleAssignedDto.UserId);

            if (user is null)
            {
                return NotFound(new { isSuccess = false, message = "User not found." });
            }

            var role = await _roleManager.FindByIdAsync(roleAssignedDto.RoleId);

            if (role is null)
            {
                return NotFound(new { isSuccess = false, message = "Role not found." });
            }

            var result = await _userManager.AddToRoleAsync(user, role.Name!);
            if (result.Succeeded)
            {
                return Ok(new { isSuccess = true, message = "Role assigned sucessfully." });
            }

             var error =result.Errors.FirstOrDefault();
            return BadRequest(new {isSuccess= false, error=error});
        }

    }
}
