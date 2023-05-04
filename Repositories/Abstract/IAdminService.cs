using AuthApi.Models.DTO;

namespace AuthApi.Repositories.Abstract
{
    public interface IAdminService
    {
        Task<List<AllUsers>?> GetAllUsers();
        Task<AllUsers?> GetSingleUser(int id);
        Task<int> DeleteUser(string emailid);
    }
}
