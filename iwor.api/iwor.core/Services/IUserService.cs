using System.Threading.Tasks;
using iwor.core.Entities;

namespace iwor.core.Services
{
    public interface IUserService
    {
        Task<UserProfile> GetUserProfile(string userId);
        Task<UserProfile> UpdateProfile(UserProfile profile);
    }
}