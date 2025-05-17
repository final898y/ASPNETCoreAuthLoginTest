using Microsoft.AspNetCore.Mvc;
using Dapper;


using System.Data;
using Microsoft.Data.Sqlite;
using ASPNETCoreAuthLoginTest.Models;
using static System.Formats.Asn1.AsnWriter;

namespace ASPNETCoreAuthLoginTest.Controllers
{
    public class ScoreController : Controller
    {
        private readonly string _connectionString;

        public ScoreController(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Default");
        }

        public async Task<IActionResult> InputScores()
        {
            var userName = User.FindFirst("FullName")?.Value;

            try
            {
                using var connection = new SqliteConnection(_connectionString);
                await connection.OpenAsync();

                string query = @"
                SELECT c.CourseID, c.CourseName 
                FROM Courses c
                JOIN Teachers t ON c.TeacherID = t.TeacherID
                WHERE t.Name = @Name";

                var courses = (await connection.QueryAsync<Course>(query, new { Name = userName })).ToList();

                var viewModel = new InputScoresViewModel
                {
                    Courses = courses
                };

                return View(viewModel);
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"SQLite error: {ex.Message}");
                return View(new InputScoresViewModel());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return View(new InputScoresViewModel());
            }
        }

        // Partial View 載入課程學生 + 成績
        public async Task<IActionResult> GetStudentsByCourse(int courseId)
        {
            string sql = @"
                SELECT 
                    s.StudentID, 
                    s.Name, 
                    g.Score 
                FROM Students s
                LEFT JOIN Grades g ON s.StudentID = g.StudentID AND g.CourseID = @CourseID
            ";
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            // 使用 Dapper 的非同步查詢方法
            var students = (await connection.QueryAsync<StudentScoreInputViewModel>(sql, new { CourseID = courseId })).ToList();

            ViewBag.CourseID = courseId;
            return PartialView("_StudentScoreForm", students);
        }

        // 儲存成績（更新資料庫）
        [HttpPost]
        public async Task<IActionResult> SaveScores(List<StudentScoreInputViewModel> scores, int courseId)
        {
            var userName = User.FindFirst("FullName")?.Value;
            string sqltoFindTeacherID = @"
                SELECT TeacherID
                FROM Teachers
                WHERE Name = @username
            ";
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            // 正確查詢老師ID
            int teacherID = await connection.QuerySingleOrDefaultAsync<int>(sqltoFindTeacherID, new
            {
                username = userName,
            });

            if (teacherID == 0)
            {
                // 找不到老師的處理（可回傳錯誤或給預設值）
                TempData["Error"] = "找不到對應的老師資料。";
                return RedirectToAction("InputScores");
            }

            // 資料庫更新成績語法（存在就更新，不存在就新增）
            string sql = @"
            INSERT INTO Grades (StudentID, CourseID, Score, UpdatedBy)
            VALUES (@StudentID, @CourseID, @Score, @UpdatedBy)
            ON CONFLICT(StudentID, CourseID)
            DO UPDATE SET
                Score = excluded.Score,
                LastUpdateTime = datetime('now'),
                UpdatedBy = excluded.UpdatedBy;
            ";

            foreach (var score in scores)
            {
                await connection.ExecuteAsync(sql, new
                {
                    score.StudentID,
                    CourseID = courseId,
                    score.Score,
                    UpdatedBy = teacherID
                });
            }

            TempData["Success"] = "成績儲存成功！";
            return RedirectToAction("InputScores");
        }



    }
}
