DROP TABLE IF EXISTS Grades;
DROP TABLE IF EXISTS Users;
DROP TABLE IF EXISTS Courses;
DROP TABLE IF EXISTS Teachers;
DROP TABLE IF EXISTS Students;

PRAGMA foreign_keys = ON;

-- 1. 建立 Students 資料表
CREATE TABLE IF NOT EXISTS Students (
    StudentID INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Grade TEXT,
    Class TEXT
);

-- 2. 建立 Teachers 資料表
CREATE TABLE IF NOT EXISTS Teachers (
    TeacherID INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Subject TEXT,
    LeadClass TEXT
);

-- 3. 建立 Courses 資料表（需要先有 Teachers）
CREATE TABLE IF NOT EXISTS Courses (
    CourseID INTEGER PRIMARY KEY AUTOINCREMENT,
    CourseName TEXT NOT NULL,
    TeacherID INTEGER NOT NULL,
    FOREIGN KEY (TeacherID) REFERENCES Teachers(TeacherID)
);

-- 4. 建立 Users 資料表（需要先有 Students 和 Teachers）
CREATE TABLE IF NOT EXISTS Users (
    UserID INTEGER PRIMARY KEY AUTOINCREMENT,
    AccountName TEXT NOT NULL UNIQUE,
    Email TEXT NOT NULL,
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

-- 5. 建立 Grades 資料表（需要 Students、Courses、Teachers）
CREATE TABLE IF NOT EXISTS Grades (
    GradeID INTEGER PRIMARY KEY AUTOINCREMENT,
    StudentID INTEGER NOT NULL,
    CourseID INTEGER NOT NULL,
    Score REAL,
    LastUpdateTime DATETIME NOT NULL DEFAULT (datetime('now')),
    UpdatedBy INTEGER NOT NULL,
    FOREIGN KEY (StudentID) REFERENCES Students(StudentID),
    FOREIGN KEY (CourseID) REFERENCES Courses(CourseID),
    FOREIGN KEY (UpdatedBy) REFERENCES Teachers(TeacherID),
    UNIQUE (StudentID, CourseID)
);

-- 6. 建立索引
CREATE INDEX IF NOT EXISTS IX_Users_AccountName ON Users(AccountName);
CREATE INDEX IF NOT EXISTS IX_Users_StudentID ON Users(StudentID);
CREATE INDEX IF NOT EXISTS IX_Users_TeacherID ON Users(TeacherID);
CREATE INDEX IF NOT EXISTS IX_Courses_TeacherID ON Courses(TeacherID);
CREATE INDEX IF NOT EXISTS IX_Grades_StudentID ON Grades(StudentID);
CREATE INDEX IF NOT EXISTS IX_Grades_CourseID ON Grades(CourseID);
CREATE INDEX IF NOT EXISTS IX_Grades_UpdatedBy ON Grades(UpdatedBy);
CREATE INDEX IF NOT EXISTS IX_Users_Email ON Users(Email);

-- 開啟外鍵支援（SQLite 預設是關的）
PRAGMA foreign_keys = ON;

-- 插入 Students 測試資料
INSERT INTO Students (Name, Grade, Class) VALUES
('王小明', '七年級', 'A班'),
('李小華', '八年級', 'B班');

-- 插入 Teachers 測試資料
INSERT INTO Teachers (Name, Subject) VALUES
('張老師', '數學'),
('陳老師', '自然');

-- 插入 Courses 測試資料（關聯 TeacherID）
INSERT INTO Courses (CourseName, TeacherID) VALUES
('代數課程', 1),  -- 張老師
('生物課程', 2),  -- 陳老師
('微積分課程',1),  -- 張老師
('生物實驗課程',2);  -- 陳老師
-- 插入 Users 測試資料（分別對應學生與老師）
INSERT INTO Users (AccountName, PasswordHash,Email, Role, StudentID, TeacherID, LastLoginTime) VALUES
('student01', '$2a$11$bX3apy3i3kcxnC4eAtgC2.jGDj/C6VcYr6T33Fv1xihL.T6yhaRNm','s1@gmail.com', 'Student', 1, NULL, '2025-05-14 09:00:00'),
('student02', 'hash_pass_02','s2@gmail.com', 'Student', 2, NULL, '2025-05-14 09:30:00'),
('teacher01', '$2a$11$bX3apy3i3kcxnC4eAtgC2.jGDj/C6VcYr6T33Fv1xihL.T6yhaRNm','t1@gmail.com', 'Teacher', NULL, 1, '2025-05-14 10:00:00'),
('teacher02', 'hash_pass_04','t2@gmail.com', 'Teacher', NULL, 2, '2025-05-14 10:30:00');

-- 插入 Grades 測試資料（關聯 StudentID、CourseID、UpdatedBy）
INSERT INTO Grades (StudentID, CourseID, Score, LastUpdateTime, UpdatedBy) VALUES
(1, 1, 88.5, '2025-05-13 14:00:00', 1), -- 王小明的代數課，由張老師輸入
(2, 2, 92.0, '2025-05-13 15:00:00', 2); -- 李小華的生物課，由陳老師輸入




-- 更多 Students
INSERT INTO Students (Name, Grade, Class) VALUES
('林小玉', '七年級', 'C班'),
('周大偉', '九年級', 'A班'),
('許安安', '八年級', 'C班');

-- 更多 Teachers
INSERT INTO Teachers (Name, Subject) VALUES
('黃老師', '英文'),
('吳老師', '歷史');

-- 更多 Courses（關聯老師）
INSERT INTO Courses (CourseName, TeacherID) VALUES
('英文閱讀', 3),  -- 黃老師
('世界歷史', 4),  -- 吳老師
('數學進階', 1);  -- 張老師（再次任教）

-- 更多 Users（學生與老師帳號）
INSERT INTO Users (AccountName, PasswordHash, Email, Role, StudentID, TeacherID, LastLoginTime) VALUES
('student03', 'hash_pass_05','s3@gmail.com', 'Student', 3, NULL, '2025-05-14 11:00:00'),
('student04', 'hash_pass_06','s4@gmail.com', 'Student', 4, NULL, '2025-05-14 11:30:00'),
('student05', 'hash_pass_07','s5@gmail.com', 'Student', 5, NULL, '2025-05-14 12:00:00'),
('teacher03', 'hash_pass_08','t3@gmail.com', 'Teacher', NULL, 3, '2025-05-14 12:30:00'),
('teacher04', 'hash_pass_09','t4@gmail.com', 'Teacher', NULL, 4, '2025-05-14 13:00:00');

-- 更多 Grades（成績資料）
INSERT INTO Grades (StudentID, CourseID, Score, LastUpdateTime, UpdatedBy) VALUES
(3, 3, 76.0, '2025-05-13 16:00:00', 3),  -- 林小玉，英文閱讀，黃老師
(4, 4, 84.0, '2025-05-13 16:30:00', 4),  -- 周大偉，世界歷史，吳老師
(1, 5, 90.0, '2025-05-13 17:00:00', 1),  -- 王小明，數學進階，張老師
(5, 3, 95.0, '2025-05-13 17:30:00', 3);  -- 許安安，英文閱讀，黃老師



-- 新增 Students（七年級 2 人、八年級 2 人、九年級 1 人）
INSERT INTO Students (Name, Grade, Class) VALUES
('陳小天', '七年級', 'B班'),
('張佳怡', '七年級', 'A班'),
('高文豪', '八年級', 'A班'),
('林雨萱', '八年級', 'B班'),
('李大仁', '九年級', 'C班');

-- 新增 Users（這些學生的登入資料）
-- 假設已有 5 位學生，從 StudentID 6 開始
INSERT INTO Users (AccountName, PasswordHash, Email, Role, StudentID, TeacherID, LastLoginTime) VALUES
('student06', 'hash_pass_10', 's6@gmail.com', 'Student', 6, NULL, '2025-05-15 09:00:00'),
('student07', 'hash_pass_11', 's7@gmail.com', 'Student', 7, NULL, '2025-05-15 09:15:00'),
('student08', 'hash_pass_12', 's8@gmail.com', 'Student', 8, NULL, '2025-05-15 09:30:00'),
('student09', 'hash_pass_13', 's9@gmail.com', 'Student', 9, NULL, '2025-05-15 09:45:00'),
('student10', 'hash_pass_14', 's10@gmail.com', 'Student', 10, NULL, '2025-05-15 10:00:00');

-- 新增成績（依照不同課程與老師輸入）
-- 假設 CourseID：1=代數課程, 2=生物課程, 3=英文閱讀, 4=世界歷史, 5=數學進階
-- 假設 UpdatedBy：1=張老師, 2=陳老師, 3=黃老師, 4=吳老師
INSERT INTO Grades (StudentID, CourseID, Score, LastUpdateTime, UpdatedBy) VALUES
(6, 1, 78.0, '2025-05-15 14:00:00', 1), -- 陳小天，代數課程
(6, 3, 85.0, '2025-05-15 14:30:00', 3), -- 陳小天，英文閱讀
(7, 1, 88.0, '2025-05-15 15:00:00', 1), -- 張佳怡，代數課程
(7, 5, 92.0, '2025-05-15 15:30:00', 1), -- 張佳怡，數學進階
(8, 2, 81.0, '2025-05-15 16:00:00', 2), -- 高文豪，生物課程
(8, 3, 89.0, '2025-05-15 16:30:00', 3), -- 高文豪，英文閱讀
(9, 4, 77.5, '2025-05-15 17:00:00', 4), -- 林雨萱，世界歷史
(10, 4, 90.5, '2025-05-15 17:30:00', 4); -- 李大仁，世界歷史
