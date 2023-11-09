using ContactBook.Model.ViewModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Core.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> RegisterUserAsync(RegisterViewModel model, ModelStateDictionary modelState, string role);
        Task<string> LoginAsync(LoginViewModel model);
    }
}
