using System.Threading.Tasks;

namespace iwor.api.Services
{
    public interface IUserService
    {
        Task Authenticate(string email, string password);
    }
}