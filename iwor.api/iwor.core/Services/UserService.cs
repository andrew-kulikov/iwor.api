using System.Threading.Tasks;
using AutoMapper;
using iwor.core.Entities;
using Microsoft.AspNetCore.Identity;

namespace iwor.core.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<UserProfile> GetUserProfile(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var profile = _mapper.Map<UserProfile>(user);

            return profile;
        }

        public async Task<UserProfile> UpdateProfile(UserProfile profile)
        {
            var user = _mapper.Map<ApplicationUser>(profile);

            var result = await  _userManager.UpdateAsync(user);

            return result.Succeeded ? profile : null;
        }
    }
}