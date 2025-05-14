-- 插入學生資料
INSERT INTO Students (Name, Grade, Class)
VALUES 
    ('小明', '三年級', '甲班'),
    ('小華', '三年級', '乙班');

-- 插入老師資料
INSERT INTO Teachers (Name, Subject)
VALUES 
    ('王老師', '數學'),
    ('李老師', '自然');

-- 插入課程資料（對應老師）
INSERT INTO Courses (CourseName, TeacherID)
VALUES 
    ('數學課', 1),
    ('自然課', 2);

-- 插入使用者資料（學生角色）student01-password=123456
INSERT INTO Users (Username, PasswordHash, Role, StudentID, LastLoginTime)
VALUES 
    ('student01', '$2a$11$bX3apy3i3kcxnC4eAtgC2.jGDj/C6VcYr6T33Fv1xihL.T6yhaRNm', 'Student', 1, datetime('now')),
    ('student02', 'hashed_password_2', 'Student', 2, datetime('now'));

-- 插入使用者資料（老師角色
INSERT INTO Users (Username, PasswordHash, Role, TeacherID, LastLoginTime)
VALUES 
    ('teacher01', 'hashed_password_3', 'Teacher', 1, datetime('now')),
    ('teacher02', 'hashed_password_4', 'Teacher', 2, datetime('now'));

-- 插入成績資料（王老師幫小明改數學成績，李老師幫小華改自然）
INSERT INTO Grades (StudentID, CourseID, Score, LastUpdateTime, UpdatedBy)
VALUES 
    (1, 1, 95.5, datetime('now'), 1),
    (2, 2, 88.0, datetime('now'), 2);
