using AuthApi.Models;
using AuthApi.Models.DTO;
using AuthApi.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AuthApi.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
       
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        
        }

        [HttpGet]
        public IActionResult GetDataAsync()
        {
            var userClaims = HttpContext.User.Claims.ToList();
            string msg = "Data from admin controller";
            return Ok(new Status(200, "Success", msg));
        }

        [HttpGet]
        public async Task<ActionResult<List<AllUsers>>> GetAllUsers()
        {
            var result = await _adminService.GetAllUsers();
            foreach (AllUsers res in result)
            {
                if (res.ProfileImage != null)
                {
                    string img = $"{Models.Url.apiUrl}/Resources/ProfileImages/{res.ProfileImage}";
                    res.ProfileImage = img;
                }
            }
            return Ok(new Status(200, "Success", result));
        }

       
        [HttpGet("{id}")]
        public async Task<ActionResult<AllUsers>> GetSingleUser(int id)
        {
            var result = await _adminService.GetSingleUser(id);
            if (result is null)
            {
                return Ok(new Status(404, "User not exist", null));
            }
            if (result.ProfileImage != null)
            {
                string img = $"{Models.Url.apiUrl}/Resources/ProfileImages/{result.ProfileImage}";
                result.ProfileImage = img;
            }
            return Ok(new Status(200, "Success", result));
        }
        /*
        [MapToApiVersion("2.0")]
        [HttpGet("{id}")]
        public async Task<ActionResult<AllUsers>> DeleteUser(int id)
        {
            var result = await _adminService.GetSingleUser(id);
            if (result is null)
            {
                return Ok(new StatusNew(404, "User not exist"));
            }
            if (result.ProfileImage != null)
            {
                string img = $"{Models.Url.apiUrl}/Resources/ProfileImages/{result.ProfileImage}";
                result.ProfileImage = img;
            }
            return Ok(new Status(200, "Success", result));
        }
        */

        [HttpPost]
        public async Task<IActionResult> DeleteUser(JustEmail model)
        {
            int result = await _adminService.DeleteUser(model.Email);
            if (result == 0)
            {
                return Ok(new Status(404, "Either User Already Deleted or User Does not exist or Something went Wrong!!!", null));
            }
            return Ok(new Status(200, "User Deleted Successfully!!!", null));
        }
    }
}
