using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ASPNETCoreAuthLoginTest.Models
{
    public class loginDataModel
    {
        public class Student
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // 設定 StudentID 為主鍵且自動產生
            public int StudentID { get; set; }

            [Required]
            [MaxLength(50)] // 設定 Name 欄位最大長度
            public required string Name { get; set; }

            [MaxLength(20)] // 設定 Grade 欄位最大長度
            public string? Grade { get; set; }

            [MaxLength(20)] // 設定 Class 欄位最大長度
            public string? Class { get; set; }

            // 導覽屬性：一個學生可以有多筆成績
            public ICollection<Grade>? Grades { get; set; }
            // 導覽屬性: 一個學生只能有一個User
            public required User User { get; set; }
        }

        // Teachers 資料表對應的 Model
        public class Teacher
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // 設定 TeacherID 為主鍵且自動產生
            public required int TeacherID { get; set; }

            [Required]
            [MaxLength(50)] // 設定 Name 欄位最大長度
            public required string Name { get; set; }

            [MaxLength(50)] // 設定 Subject 欄位最大長度
            public required string Subject { get; set; }

            // 導覽屬性：一個老師可以教授多門課程
            public ICollection<Course>? Courses { get; set; }
            // 導覽屬性: 一個老師只能有一個User
            public required User User { get; set; }
        }

        // Courses 資料表對應的 Model
        public class Course
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // 設定 CourseID 為主鍵且自動產生
            public required int CourseID { get; set; }

            [Required]
            [MaxLength(100)] // 設定 CourseName 欄位最大長度
            public required string CourseName { get; set; }

            [Required]
            [ForeignKey("Teacher")] // 設定 TeacherID 為外鍵，關聯到 Teacher 表格
            public required int TeacherID { get; set; }

            // 導覽屬性：所屬的老師
            public required Teacher Teacher { get; set; }

            // 導覽屬性：一門課程可以有多筆成績
            public ICollection<Grade>? Grades { get; set; }
        }

        // Users 資料表對應的 Model
        public class User
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // 設定 UserID 為主鍵且自動產生
            public required int UserID { get; set; }

            [Required]
            [MaxLength(50)] // 設定 Username 欄位最大長度
            public required string Username { get; set; }

            [Required]
            [MaxLength(255)]  // 實際長度根據你的雜湊算法調整
            public required string PasswordHash { get; set; }

            [Required]
            [MaxLength(10)] //设定 Role 欄位最大長度
            public required string Role { get; set; }

            // 使用可為 null 的 int?
            [ForeignKey("Student")]
            public int? StudentID { get; set; }

            [ForeignKey("Teacher")]
            public int? TeacherID { get; set; }

            public DateTime? LastLoginTime { get; set; }

            // 導覽屬性：關聯的學生
            public Student? Student { get; set; }

            // 導覽屬性：關聯的老師
            public Teacher? Teacher { get; set; }
        }

        // Grades 資料表對應的 Model
        public class Grade
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // 設定 GradeID 為主鍵且自動產生
            public required int GradeID { get; set; }

            [Required]
            [ForeignKey("Student")] // 設定 StudentID 為外鍵，關聯到 Student 表格
            public required int StudentID { get; set; }

            [Required]
            [ForeignKey("Course")] // 設定 CourseID 為外鍵，關聯到 Course 表格
            public required int CourseID { get; set; }

            [Required]
            public required double Score { get; set; } // 使用 double 更符合成績的表示

            [Required]
            public required DateTime LastUpdateTime { get; set; }

            [Required]
            [ForeignKey("Teacher")] // 設定 UpdatedBy 為外鍵，關聯到 Teacher 表格
            public required int UpdatedBy { get; set; }

            // 導覽屬性：所屬的學生
            public required Student Student { get; set; }

            // 導覽屬性：所屬的課程
            public required Course Course { get; set; }

            // 導覽屬性：更新成績的老師
            public required Teacher UpdatedByTeacher { get; set; }
        }
    }
}
