using SingleSignOn.Domain.Models;
using SingleSignOn.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SingleSignOn.Service.Interfaces
{
    public interface IAccountService
    {
        Task<ApplicationUser> Validate(string username, string password);
        Task<bool> Create(ApplicationUser applicationUser);
        

    }
}
