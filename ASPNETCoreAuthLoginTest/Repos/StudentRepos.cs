using ASPNETCoreAuthLoginTest.Models;
using Microsoft.Data.Sqlite;

namespace ASPNETCoreAuthLoginTest.Repos
{
    public class StudentRepos: IUserRepos<StudentProfileViewModel>
    {
        private readonly string _connectionString;

        public StudentRepos(string connectionString)
        {
            _connectionString = connectionString;
        }
        public StudentProfileViewModel? GetUserProfile(string accountName)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();
                string query = @"
                SELECT s.Name AS StudentName, u.Role,u.AccountName,s.Grade,s.Class
                FROM Users u
                JOIN Students s ON u.StudentID = s.StudentID
                WHERE u.Role = 'Student' AND u.AccountName = @accountName";

                using var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@accountName", accountName);

                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new StudentProfileViewModel
                    {
                        UserName = reader.GetString(0),
                        Role = reader.GetString(1),
                        AccountName = reader.GetString(2),
                        Grade = reader.IsDBNull(3) ? "無" : reader.GetString(3),
                        Class = reader.IsDBNull(4) ? "無" : reader.GetString(4),
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
