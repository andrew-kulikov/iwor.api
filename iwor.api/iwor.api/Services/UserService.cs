using System.Threading.Tasks;
using iwor.DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace iwor.api.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task Authenticate(string email, string password)
        {
            
        }
    }
}