using Microsoft.Extensions.Configuration;
using SingleSignOn.DataAccessLayer.Interfaces;
using SingleSignOn.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace SingleSignOn.Data.Repositories
{
    public class UserRepository : BaseRepository<ApplicationUser>, IUserRepository
    {
        public UserRepository(IConfiguration configuration)
           : base(configuration)
        {
        }

        public async Task<ApplicationUser> GetApplicationUserByCredential(string username, string password)
        {
            using (var command = new SqlCommand("SELECT * FROM Users WHERE Username = @Username And Password = @Password"))
            {
                command.Parameters.Add("@Username", SqlDbType.VarChar, 20).Value  = username;
                command.Parameters.Add("@Password", SqlDbType.VarChar, 20).Value = password;
                return await GetRecord(command);
            }
        }

        public async Task<bool> AddUser(ApplicationUser applicationUser)
        {
            using (var command = new SqlCommand("INSERT INTO Users (Id, Username, Password, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn, DisplayName, Role, SubjectId) " +
                                                "VALUES (@Id, @Username, @Password, @CreatedBy, @CreatedOn, @ModifiedBy, @ModifiedOn, @DisplayName, @Role, @SubjectId)"))
            {
                command.Parameters.Add(new SqlParameter("@Id", Guid.NewGuid()));
                command.Parameters.Add(new SqlParameter("@Username", applicationUser.Username));
                command.Parameters.Add(new SqlParameter("@Password", applicationUser.Password));
                command.Parameters.Add(new SqlParameter("@CreatedBy", applicationUser.CreatedBy));
                command.Parameters.Add(new SqlParameter("@CreatedOn", applicationUser.CreatedOn));
                command.Parameters.Add(new SqlParameter("@ModifiedBy", applicationUser.ModifiedBy));
                command.Parameters.Add(new SqlParameter("@ModifiedOn", applicationUser.ModifiedOn));
                command.Parameters.Add(new SqlParameter("@DisplayName", applicationUser.DisplayName));
                command.Parameters.Add(new SqlParameter("@Role", applicationUser.Role));
                command.Parameters.Add(new SqlParameter("@SubjectId", applicationUser.SubjectId));
                await SaveRecord(command);
            }
            return true;
        }

        public async Task<ApplicationUser> FindUserBySubjectId(string subjectId)
        {
            ApplicationUser user = null;
            using (var command = new SqlCommand("SELECT * FROM [Users] WHERE [SubjectId] = @Subjectid;"))
            {
                command.Parameters.Add(new SqlParameter("@Subjectid", subjectId));
                user = await GetRecord(command);
            }
            return user;
        }


        public override ApplicationUser PopulateRecord(SqlDataReader reader)
        {
            if (!reader.IsDBNull(0))
            {
                return new ApplicationUser
                {
                    Id = reader.GetGuid(0),
                    Username = reader.GetString(1),
                    Password = reader.GetString(2),
                    CreatedBy = reader.GetString(3),
                    CreatedOn = reader.GetDateTime(4),
                    ModifiedBy = reader.GetString(5),
                    ModifiedOn = reader.GetDateTime(6),
                    DisplayName = reader.GetString(7),
                    Role = reader.GetString(8),
                    SubjectId = reader.GetString(9)
                };
            }

            return new ApplicationUser{};
        }
    }
}
