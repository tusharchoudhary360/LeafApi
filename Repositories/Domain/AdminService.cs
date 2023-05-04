using AuthApi.Models.Domain;
using AuthApi.Models.DTO;
using AuthApi.Repositories.Abstract;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AuthApi.Repositories.Domain
{
    public class AdminService:IAdminService
    {
        private readonly DatabaseContext _context;
        public AdminService(DatabaseContext context)
        {
            _context = context;
        }
        //get all user details 
        public async Task<List<AllUsers>?> GetAllUsers()
        {
            var user = await _context.AllUsers.ToListAsync();
            return user;
        }

        // get single user datails 
        public async Task<AllUsers?> GetSingleUser(int id)
        {
            var user = await _context.AllUsers.FindAsync(id);
            if (user == null)
            {
                return null;
            }
            return user;
        }

        public async Task<int> DeleteUser(string email)
        {
            SqlConnection sqlCon = null;
            String SqlconString = Models.Url.sqlConnectionString;

            using (sqlCon = new SqlConnection(SqlconString))
            {
                sqlCon.Open();
                SqlCommand sql_cmnd = new SqlCommand("DeleteUser_SP", sqlCon);
                sql_cmnd.CommandType = CommandType.StoredProcedure;
                sql_cmnd.Parameters.AddWithValue("@UserEmail", SqlDbType.NVarChar).Value = email;
                int result = sql_cmnd.ExecuteNonQuery();
                sqlCon.Close();

                return result; //1  "User Deleted, 0 Query ran but no row was affected
            }
        }
    }
}
