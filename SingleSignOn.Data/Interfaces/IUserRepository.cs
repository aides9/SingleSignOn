using SingleSignOn.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SingleSignOn.DataAccessLayer.Interfaces
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetApplicationUserByCredential(string username, string password);

        Task<ApplicationUser> FindUserBySubjectId(string subjectId);

        Task<bool> AddUser(ApplicationUser applicationUser);

    }
}
