using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iwor.core.Entities;

namespace iwor.core.Services
{
    public interface IUserService
    {
        Task<UserProfile> GetUserProfile(string userId);
    }
}
