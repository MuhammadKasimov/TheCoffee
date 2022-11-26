using System.Threading.Tasks;

namespace StarBucks.Service.Interfaces
{
    public interface IAuthService
    {
        ValueTask<string> GenerateToken(string username, string password);
    }
}
