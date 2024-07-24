using Dapper;
using dbs.Context;
using dbs.Models;

namespace dbs.Repositories
{

    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAll();
        Task<Account> GetById(int id);
        Task<Account> GetByEmail(string email);
        Task Create(Account account);
        Task Update(Account account);
        Task Delete(int id);
    }

    public class UserRepository : IAccountRepository
    {
        private DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Account>> GetAll()
        {
            using var connection = _context.CreateConnection();
            var sql = """
            SELECT * FROM Users
        """;
            return await connection.QueryAsync<Account>(sql);
        }

        public async Task<Account> GetById(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = """
            SELECT * FROM Users 
            WHERE Id = @id
        """;
            return await connection.QuerySingleOrDefaultAsync<Account>(sql, new { id });
        }

        public async Task<Account> GetByEmail(string email)
        {
            using var connection = _context.CreateConnection();
            var sql = """
            SELECT * FROM Users 
            WHERE Email = @email
        """;
            return await connection.QuerySingleOrDefaultAsync<Account>(sql, new { email });
        }

        public async Task Create(Account account)
        {
            using var connection = _context.CreateConnection();
            var sql = """
            INSERT INTO Users (Title, FirstName, LastName, Email, Role, PasswordHash)
            VALUES (@Title, @FirstName, @LastName, @Email, @Role, @PasswordHash)
        """;
            await connection.ExecuteAsync(sql, account);
        }

        public async Task Update(Account account)
        {
            using var connection = _context.CreateConnection();
            var sql = """
            UPDATE Users 
            SET Title = @Title,
                FirstName = @FirstName,
                LastName = @LastName, 
                Email = @Email, 
                Role = @Role, 
                PasswordHash = @PasswordHash
            WHERE Id = @Id
        """;
            await connection.ExecuteAsync(sql, account);
        }

        public async Task Delete(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = $"DELETE FROM Users WHERE Id = @id";
            await connection.ExecuteAsync(sql, new { id });
        }
    }
}
