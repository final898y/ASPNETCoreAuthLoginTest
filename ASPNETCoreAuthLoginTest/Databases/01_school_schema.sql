-- 啟用 SQLite 外鍵支援（必加）
PRAGMA foreign_keys = ON;

-- 先刪除資料表，依照依賴順序由 Grades → Users → Courses → Teachers → Students
DROP TABLE IF EXISTS Grades;
DROP TABLE IF EXISTS Users;
DROP TABLE IF EXISTS Courses;
DROP TABLE IF EXISTS Teachers;
DROP TABLE IF EXISTS Students;

-- 建立 Students 資料表：學生資訊
CREATE TABLE IF NOT EXISTS Students (
    StudentID INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Grade TEXT,
    Class TEXT
);

-- 建立 Teachers 資料表：老師資訊
CREATE TABLE IF NOT EXISTS Teachers (
    TeacherID INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Subject TEXT
);

-- 建立 Courses 資料表：課程資訊，需關聯老師
CREATE TABLE IF NOT EXISTS Courses (
    CourseID INTEGER PRIMARY KEY AUTOINCREMENT,
    CourseName TEXT NOT NULL,
    TeacherID INTEGER NOT NULL,
    FOREIGN KEY (TeacherID) REFERENCES Teachers(TeacherID)
);

-- 建立 Users 資料表：登入帳號資訊，分學生或老師
CREATE TABLE IF NOT EXISTS Users (
    UserID INTEGER PRIMARY KEY AUTOINCREMENT,
    AccountName TEXT NOT NULL UNIQUE,
    PasswordHash TEXT NOT NULL,
    Role TEXT NOT NULL CHECK(Role IN ('Student', 'Teacher')),
    StudentID INTEGER,
    TeacherID INTEGER,
    LastLoginTime DATETIME,
    CHECK (
        (Role = 'Student' AND StudentID IS NOT NULL AND TeacherID IS NULL) OR
        (Role = 'Teacher' AND TeacherID IS NOT NULL AND StudentID IS NULL)
    ),
    FOREIGN KEY (StudentID) REFERENCES Students(StudentID),
    FOREIGN KEY (TeacherID) REFERENCES Teachers(TeacherID)
);

-- 建立 Grades 資料表：學生成績，紀錄由哪位老師更新
CREATE TABLE IF NOT EXISTS Grades (
    GradeID INTEGER PRIMARY KEY AUTOINCREMENT,
    StudentID INTEGER NOT NULL,
    CourseID INTEGER NOT NULL,
    Score REAL NOT NULL,
    LastUpdateTime DATETIME NOT NULL,
    UpdatedBy INTEGER NOT NULL,
    FOREIGN KEY (StudentID) REFERENCES Students(StudentID),
    FOREIGN KEY (CourseID) REFERENCES Courses(CourseID),
    FOREIGN KEY (UpdatedBy) REFERENCES Teachers(TeacherID)
);

-- 建立索引以加速查詢（建議針對經常查詢欄位建立索引）
CREATE INDEX IF NOT EXISTS IX_Users_AccountName ON Users(AccountName);
CREATE INDEX IF NOT EXISTS IX_Users_StudentID ON Users(StudentID);
CREATE INDEX IF NOT EXISTS IX_Users_TeacherID ON Users(TeacherID);
CREATE INDEX IF NOT EXISTS IX_Courses_TeacherID ON Courses(TeacherID);
CREATE INDEX IF NOT EXISTS IX_Grades_StudentID ON Grades(StudentID);
CREATE INDEX IF NOT EXISTS IX_Grades_CourseID ON Grades(CourseID);
CREATE INDEX IF NOT EXISTS IX_Grades_UpdatedBy ON Grades(UpdatedBy);
