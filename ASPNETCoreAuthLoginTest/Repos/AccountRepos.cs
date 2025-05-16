using Microsoft.Data.Sqlite;
using  ASPNETCoreAuthLoginTest.Models;
namespace ASPNETCoreAuthLoginTest.Repositorys
{
    public class AccountRepos
    {
        private readonly string _connectionString;

        public AccountRepos(string connectionString)
        {
            _connectionString = connectionString;
        }
        public UserLoginDto? GetUserDto(string accountName, UserRole role)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();
                string query = "";
                if (role == UserRole.Teacher)
                {
                    query = @"
                    SELECT t.Name AS TeacherName, u.PasswordHash, u.Role,u.AccountName
                    FROM Users u
                    JOIN Teachers t ON u.TeacherID = t.TeacherID
                    WHERE u.Role = 'Teacher' AND u.AccountName = @accountName";
                }
                else {
                    query = @"
                    SELECT s.Name AS StudentName, u.PasswordHash, u.Role,u.AccountName
                    FROM Users u
                    JOIN Students s ON u.StudentID = s.StudentID
                    WHERE u.Role = 'Student' AND u.AccountName = @accountName";
                }
                

                using var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@accountName", accountName);

                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new UserLoginDto
                    {
                        UserName = reader.GetString(0),
                        PasswordHash = reader.GetString(1),
                        Role = reader.GetString(2),
                        AccountName = reader.GetString(3)
                    };
                }
                else {
                    return null;
                }

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

        public UserLoginDto? GetUserByUsername(string accountName, UserRole role)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();
                string query = "";
                if (role == UserRole.Teacher)
                {
                    query = @"
                    SELECT t.Name AS TeacherName, u.PasswordHash, u.Role
                    FROM Users u
                    JOIN Teachers t ON u.TeacherID = t.TeacherID
                    WHERE u.Role = 'Teacher' AND t.Name = @username";
                }
                else
                {
                    query = @"
                    SELECT s.Name AS StudentName, u.PasswordHash, u.Role
                    FROM Users u
                    JOIN Students s ON u.StudentID = s.StudentID
                    WHERE u.Role = 'Student' AND s.Name = @username";
                }


                using var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@username", accountName);

                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new UserLoginDto
                    {
                        UserName = reader.GetString(0),
                        PasswordHash = reader.GetString(1),
                        Role = reader.GetString(2), AccountName = reader.GetString(3)
                    };
                }
                else
                {
                    return null;
                }

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
