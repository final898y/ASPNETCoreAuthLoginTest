using ASPNETCoreAuthLoginTest.Models;
using Microsoft.Data.Sqlite;

namespace ASPNETCoreAuthLoginTest.Repos
{
    public class TeacherRepos:IUserRepos<TeacherProfileViewModel>
    {
        private readonly string _connectionString;

        public TeacherRepos(string connectionString)
        {
            _connectionString = connectionString;
        }
        public TeacherProfileViewModel? GetUserProfile(string accountName)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();
                string query = @"
                SELECT t.Name AS TeacherName, u.Role,u.AccountName,t.Subject,t.LeadClass
                FROM Users u
                JOIN Teachers t ON u.TeacherID = t.TeacherID
                WHERE u.Role = 'Teacher' AND u.AccountName = @accountName";

                using var command = new SqliteCommand(query, connection);
                //command.Parameters.AddWithValue("@accountName", accountName);
                command.Parameters.Add("@accountName", SqliteType.Text).Value = accountName;

                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Console.WriteLine($"AccountName received: {reader.GetString(0)}");

                    return new TeacherProfileViewModel
                    {

                        UserName = reader.GetString(0),
                        Role = reader.GetString(1),
                        AccountName = reader.GetString(2),
                        Subject = reader.IsDBNull(3) ? "無" : reader.GetString(3),
                        LeadClass = reader.IsDBNull(4) ? "無" : reader.GetString(4),
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
