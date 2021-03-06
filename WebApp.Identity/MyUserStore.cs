using Dapper;
using Microsoft.AspNetCore.Identity;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace WebApp.Identity
{
    public class MyUserStore : IUserStore<MyUser>, IUserPasswordStore<MyUser>
    {
        public async Task<IdentityResult> CreateAsync(MyUser user, CancellationToken cancellationToken)
        {
            using (var connection = GetDbconnection())
            {
                await connection.ExecuteAsync(
                    "insert into Users([Id]," +
                    "[UserName]," +
                    "[NormalizedUserName]," +
                    "[PasswordHash]," +
                    "[Email]," +
                    "[Cpf]," +
                    "[Nome]," +
                    "[Telefone]) " +
                    "Values(@id,@username,@normalizeUserName,@passwordHash,@email,@cpf,@nome,@telefone)",
                   new
                   {
                       id = user.Id,
                       userName = user.UserName,
                       normalizeUserName = user.NormalizeUserName,
                       passwordHash = user.PasswordHash,
                       email = user.Email,
                       cpf = user.Cpf,
                       nome = user.Nome,
                       telefone = user.Telefone,
                   });
            }
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(MyUser user, CancellationToken cancellationToken)
        {
            using (var connection = GetDbconnection())
            {
                await connection.ExecuteAsync("delete from Users where Id = @id",
                   new
                   {
                       id = user.Id
                   });
            }
            return IdentityResult.Success;
        }

        public void Dispose()
        {
           
        }

        public static DbConnection GetDbconnection()
        {
            var connection = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=master;Data Source=LAPTOP-7CO7RVCN\\SQLEXPRESS");
            //var connection = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=master;Data Source=LAPTOP-7CO7RVCN\SQLEXPRESS");
            connection.Open();
            return connection;
        }
        public async Task<MyUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            using (var connection = GetDbconnection())
            {
                return await connection.QueryFirstOrDefaultAsync<MyUser>(
                    "select * from Users where Id = @id",
                    new { id = userId });
            }
        }

        public async Task<MyUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            using (var connection = GetDbconnection())
            {
                return await connection.QueryFirstOrDefaultAsync<MyUser>(
                    "select * from Users where normalizedUserName = @name",
                    new { name = normalizedUserName });
            }
        }

        public Task<string> GetNormalizedUserNameAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizeUserName);
        }

        public Task<string> GetEmailAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<string> GetUserIdAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(MyUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizeUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetEmail(MyUser user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(MyUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(MyUser user, CancellationToken cancellationToken)
        {
            using (var connection = GetDbconnection())
            {
                await connection.ExecuteAsync(
                    "update Users " +
                    "set [Id] = @id," +
                    "[Username] = @userName," +
                    "[NormalizedUsedName] = @normalizeUserName," +
                    "[PasswordHash] = @passwordHash" +
                    "where [Id] = @id",
                   new
                   {
                       id = user.Id,
                       userName = user.UserName,
                       normalizeUserName = user.NormalizeUserName,
                       passwordHash = user.PasswordHash,
                   });
            }
            return IdentityResult.Success;
        }

        public Task SetPasswordHashAsync(MyUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }
    }
}
