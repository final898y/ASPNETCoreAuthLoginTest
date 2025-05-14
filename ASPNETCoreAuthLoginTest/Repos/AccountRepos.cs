using Microsoft.Data.Sqlite;
using loginTest.Models;
using static ASPNETCoreAuthLoginTest.Models.loginDataModel;
namespace ASPNETCoreAuthLoginTest.Repositorys
{
    public class AccountRepos
    {
        private readonly string _connectionString;

        public AccountRepos(string connectionString)
        {
            _connectionString = connectionString;
        }
        public UserDto? GetTeacherUserDto(string username)
        {
            if (string.IsNullOrEmpty(username))
                return null;

            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();

                const string query = @"
                    SELECT t.Name AS TeacherName, u.PasswordHash, u.Role
                    FROM Users u
                    JOIN Teachers t ON u.TeacherID = t.TeacherID
                    WHERE u.Role = 'Teacher' AND u.Username = @username";

                using var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@username", username);

                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new UserDto
                    {
                        Username = reader.GetString(0),
                        PasswordHash = reader.GetString(1),
                        Role = reader.GetString(2)
                    };
                }

                return null;
            }
            catch (SqliteException ex)
            {
                // Log the exception (logging implementation would be added based on your setup)
                Console.WriteLine($"SQLite error: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                // Log unexpected errors
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return null;
            }
        }

    }
}
