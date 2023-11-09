using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ContactBook.Core.Interfaces;
using ContactBook.Model;
using ContactBook.Model.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Core.implementations
{
    public class CrudRepository : ICrudRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly Cloudinary _cloudinary;

        public CrudRepository(UserManager<User> userManager, Cloudinary cloudinary)
        {
            _userManager = userManager;
            _cloudinary = cloudinary;
        }

        public async Task<bool> CreateNewUserAsync(PostNewUserViewModel model, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return false;
            }
            else
            {
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,

                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        modelState.AddModelError(string.Empty, error.Description);
                    }
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public async Task<bool> UpdateUserAsync(string userId, PutViewModel model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.PasswordHash = model.Password;
            user.UserName = model.UserName;

            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }


        public async Task<PaginatedViewModel> GetAllUserAsync(int page, int pagesize)
        {
            var totalusers = await _userManager.Users.CountAsync();
            var totalpages = (int)Math.Ceiling(totalusers / (double)pagesize);

            page = Math.Max(1, Math.Min(totalpages, page));

            var users = await _userManager.Users
                .OrderBy(x => x.Id)
                .Skip((page - 1) * pagesize)
                .Take(pagesize)
                .ToListAsync();

            return new PaginatedViewModel
            {
                TotalUsers = totalusers,
                CurrentPage = page,
                PageSize = pagesize,
                Users = users
            };
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<User> GetUserByidAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<string> UploadUserImage(string userId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return "Invalid data";
            }
            const int MaxFileSize = 300 * 1024;
            if (file.Length > MaxFileSize)
            {
                return "File size exceeds the maximum limit (300kb)";
            }
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                return "Only jpg, jpeg, png files are allowed";
            }
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                return "User not found";
            }
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName,file.OpenReadStream())
            };
            var uploadImage = await _cloudinary.UploadAsync(uploadParams);
            if(uploadImage.Error != null)
            {
                return "Error Uploading image through cloudinary";
            }
            user.ImageURL = uploadImage.Url.ToString();
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return "Failed to update user image";
            }
            return "File updated successfully";

        }
    }
}
