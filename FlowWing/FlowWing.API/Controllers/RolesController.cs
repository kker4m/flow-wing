using FlowWing.Business.Abstract;
using FlowWing.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlowWing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    [EnableCors("AllowAll")]
    public class RolesController : ControllerBase
    {
        private IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        ///<summary>
        ///Get all roles
        ///</summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            
            var roles = await _roleService.GetAllAsync();
            return Ok(roles);
        }


        ///<summary>
        ///Add a role
        ///</summary>
        ///<param name="roleName">Role name</param> 
        /// <returns></returns>
        [HttpPost]        
        public async Task<IActionResult> AddRole([FromBody] string roleName)
        {
            Role role = new Role
            {
                Name = roleName,
                CreationDate = DateTime.Now
            };
            await _roleService.AddAsync(role);
            return Ok();
        }


        ///<summary>
        ///Delete role by id
        ///</summary>
        ///<param name="id">Role id</param>
        ///<returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            await _roleService.DeleteAsync(id);
            return Ok();
        }
    }
}
