using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ContactBook.Core.Interfaces;
using ContactBook.Core.implementations;
using ContactBook.Data.Migrations;
using ContactBook.Model;
using ContactBook.Model.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Security.Claims;

namespace ContactBook.Controllers.CRUD
{
    [Route("api/[controller]")]
    [ApiController]
    public class CRUDController : ControllerBase
    {
        private readonly ICrudRepository _crudrepository;
        private readonly UserManager<User> _userManager;

        public CRUDController(ICrudRepository crudRepository, UserManager<User> userManager)
        {
            _crudrepository = crudRepository;
            _userManager = userManager;
        }

        
        [HttpPost("add-new-user")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddNewUser([FromBody] PostNewUserViewModel model)
        {
            
                var result = await _crudrepository.CreateNewUserAsync(model, ModelState);
                if (!result)
                {
                    return BadRequest(ModelState);
                }
                return Ok(new
                {
                    Message = "User created successfully"
                });

        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] PutViewModel model)
        {
            var userUpdate = await _crudrepository.UpdateUserAsync(id, model);
            if (!userUpdate)
            {
                return BadRequest(new
                {
                    Message = "Update failed"
                });
            }
            return Ok(new
            {
                Message = "User Updated successfully"
            });

        }

      


        [HttpGet("all-users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers(int page, int pageSize)
        {   
                var paginatedResult = await _crudrepository.GetAllUserAsync(page, pageSize);
                return Ok(paginatedResult);
            
        }





        [HttpGet("email")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _crudrepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound(new
                {
                    Message = "User not found"
                });
            }
            return Ok($"User '{user.UserName}' was found ");
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _crudrepository.GetUserByidAsync(id);
            if (user == null)
            {
                return NotFound(new
                {
                    Message = "User not found"
                });
            }
            return Ok($"User '{user.UserName}' was found ");
        }


        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser( string id)
        {
          
                var userDeleted = await _crudrepository.DeleteUserAsync(id);
                if (userDeleted == null)
                {
                    return BadRequest(new
                    {
                        Message = "User not found or failed to delete user"
                    });
                }
                return Ok(new
                {
                    Message = "User deleted successfully"
                });
           
        }

        [HttpPatch("image/{id}")]
        public async Task<IActionResult> UpUserLoadImage(string id, IFormFile image)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { Messsage = "User not found" });
            }
            if (image == null)
            {
                return BadRequest(new { Messsage = "image file is required" });
            }
            if (image.Length <= 0)
            {
                return BadRequest(new { Messsage = "image file is empty" });
            }
            var upload = await _crudrepository.UploadUserImage(id,image);

            if (upload != "File updated successfully")
            {
                return BadRequest(new { Messsage = upload });
            }
            return Ok(new { Messsage = "user image updated successfully" });
        }
    }
}
