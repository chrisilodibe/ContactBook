using ContactBook.Model.ViewModel;
using ContactBook.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ContactBook.Core.Interfaces
{
    public interface ICrudRepository
    {
        Task<bool> CreateNewUserAsync(PostNewUserViewModel model, ModelStateDictionary modelState);
        Task<bool> UpdateUserAsync(string userId, PutViewModel model);
        Task<PaginatedViewModel> GetAllUserAsync(int page, int pagesize);
        Task<bool> DeleteUserAsync(string userId);
        Task<User> GetUserByidAsync(string userId);
        Task<User> GetUserByEmailAsync(string email);

        Task<string> UploadUserImage(string userId, IFormFile file);
    }
}
