using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using SingleSignOn.DataAccessLayer.Interfaces;
using SingleSignOn.Domain.Models;
using SingleSignOn.Service.Interfaces;
using SingleSignOn.Service.Models;
using System.Threading.Tasks;

namespace SingleSignOn.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly IIdentityServerInteractionService _interaction;
        private IAuthenticationSchemeProvider _schemeProvider;
        private IUserRepository _userRepository;
        public AccountService(
           IIdentityServerInteractionService interaction,
           IAuthenticationSchemeProvider schemeProvider,
           IUserRepository userRepository)
        {
            _interaction = interaction;
            _schemeProvider = schemeProvider;
            _userRepository = userRepository;
        }

        public async Task<ApplicationUser> Validate(string username, string password)
        {
            return await _userRepository.GetApplicationUserByCredential(username, password);
        }

        public async Task<bool> Create(ApplicationUser applicationUser)
        {
            return await _userRepository.AddUser(applicationUser);
        }

    }
}
