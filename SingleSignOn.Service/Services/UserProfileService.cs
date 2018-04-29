using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using SingleSignOn.DataAccessLayer.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SingleSignOn.Service.Services
{
    public class UserProfileService : IProfileService
    {
        protected readonly IUserRepository _userRepository;

        public UserProfileService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context) // used to bind claim by getting data from DB
        {
            if (context.RequestedClaimTypes.Any())
            {
                var user = _userRepository.FindUserBySubjectId(context.Subject.GetSubjectId());
                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim("name", user.Result.Username),
                        new Claim("role", user.Result.Role)
                    };
                    context.IssuedClaims.AddRange(claims);
                }
            }
            return Task.FromResult(0);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            var user = _userRepository.FindUserBySubjectId(context.Subject.GetSubjectId());
            context.IsActive = !(user is null);
            return Task.FromResult(0);
        }
    }
}
