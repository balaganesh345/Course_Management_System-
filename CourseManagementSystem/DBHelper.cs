using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CourseManagementSystem
{
    /// <summary>
    /// ADO.NET Data Access Layer connecting to SQL Server for Course Management.
    /// Implements connection management, schema initialization, and parameterized queries.
    /// </summary>
    public static class DBHelper
    {
        private static readonly string ConnectionString = 
            ConfigurationManager.ConnectionStrings["CourseManagementDbConnection"]?.ConnectionString;

        private static bool _isDbInitialized = false;
        private static readonly object _initLock = new object();

        public static SqlConnection GetConnection()
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                throw new InvalidOperationException("Connection string 'CourseManagementDbConnection' not found in Web.config.");
            }

            EnsureDatabaseInitialized();
            return new SqlConnection(ConnectionString);
        }

        /// <summary>
        /// Automatically ensures that Courses, Users, and Enrollments tables exist,
        /// and seeds default Admin, Student, and sample course records if missing.
        /// </summary>
        public static void EnsureDatabaseInitialized()
        {
            if (_isDbInitialized) return;

            lock (_initLock)
            {
                if (_isDbInitialized) return;

                try
                {
                    using (var conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();

                        string initSql = @"
                            -- 1. Users Table
                            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
                            BEGIN
                                CREATE TABLE Users (
                                    UserId INT IDENTITY(1,1) PRIMARY KEY,
                                    Username NVARCHAR(50) NOT NULL UNIQUE,
                                    Password NVARCHAR(100) NOT NULL,
                                    FullName NVARCHAR(100) NOT NULL,
                                    Email NVARCHAR(100) NOT NULL,
                                    Role NVARCHAR(20) NOT NULL, -- 'Admin' or 'Student'
                                    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
                                );
                            END

                            -- 2. Seed Default Admin, Instructor & Student
                            IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = 'admin')
                            BEGIN
                                INSERT INTO Users (Username, Password, FullName, Email, Role)
                                VALUES ('admin', 'admin123', 'System Administrator', 'admin@system.local', 'Admin');
                            END

                            IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = 'instructor')
                            BEGIN
                                INSERT INTO Users (Username, Password, FullName, Email, Role)
                                VALUES ('instructor', 'instructor123', 'Dr. Robert Smith', 'instructor@system.local', 'Instructor');
                            END

                            IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = 'student')
                            BEGIN
                                INSERT INTO Users (Username, Password, FullName, Email, Role)
                                VALUES ('student', 'student123', 'Demo Student', 'student@system.local', 'Student');
                            END

                            -- 3. Courses Table
                            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Courses' AND xtype='U')
                            BEGIN
                                CREATE TABLE Courses (
                                    CourseId INT IDENTITY(1,1) PRIMARY KEY,
                                    CourseName NVARCHAR(100) NOT NULL,
                                    CourseCode NVARCHAR(20) NOT NULL UNIQUE,
                                    Description NVARCHAR(500) NULL,
                                    InstructorName NVARCHAR(100) NOT NULL,
                                    Category NVARCHAR(50) NOT NULL,
                                    DurationInWeeks INT NOT NULL,
                                    CourseFee DECIMAL(18,2) NOT NULL,
                                    StartDate DATETIME NOT NULL,
                                    EndDate DATETIME NOT NULL,
                                    IsActive BIT NOT NULL DEFAULT 1,
                                    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
                                );
                            END

                            -- 4. Seed Initial Courses if table is empty
                            IF NOT EXISTS (SELECT TOP 1 1 FROM Courses)
                            BEGIN
                                INSERT INTO Courses (CourseName, CourseCode, Description, InstructorName, Category, DurationInWeeks, CourseFee, StartDate, EndDate, IsActive, CreatedAt)
                                VALUES 
                                ('ASP.NET Web Forms & C# Foundations', 'CS-301', 'Comprehensive deep dive into building enterprise Web Forms applications with C# and ADO.NET.', 'Dr. Robert Smith', 'Software Development', 10, 499.00, DATEADD(day, 5, GETDATE()), DATEADD(day, 75, GETDATE()), 1, GETDATE()),
                                ('Microsoft SQL Server & Database Design', 'DB-201', 'Master relational database architecture, indexing, stored procedures, and T-SQL optimization.', 'Sarah Jenkins', 'Database Systems', 8, 350.00, DATEADD(day, 10, GETDATE()), DATEADD(day, 66, GETDATE()), 1, GETDATE()),
                                ('Full Stack Web Development with Bootstrap 5', 'WEB-101', 'Learn responsive UI development, semantic HTML5, modern CSS, and JavaScript fundamentals.', 'Alex Rivera', 'Web Development', 12, 550.00, DATEADD(day, 14, GETDATE()), DATEADD(day, 98, GETDATE()), 1, GETDATE()),
                                ('Cloud Architecture & Microservices', 'CLOUD-401', 'Architect resilient cloud-native solutions, containerization with Docker, and CI/CD pipelines.', 'Emily Watson', 'Cloud Computing', 6, 600.00, DATEADD(day, 20, GETDATE()), DATEADD(day, 62, GETDATE()), 1, GETDATE());
                            END

                            -- 5. Enrollments Table with Course Completion & Quiz Tracking
                            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Enrollments' AND xtype='U')
                            BEGIN
                                CREATE TABLE Enrollments (
                                    EnrollmentId INT IDENTITY(1,1) PRIMARY KEY,
                                    UserId INT NOT NULL,
                                    CourseId INT NOT NULL,
                                    EnrollmentDate DATETIME NOT NULL DEFAULT GETDATE(),
                                    Status NVARCHAR(20) NOT NULL DEFAULT 'Enrolled',
                                    IsCompleted BIT NOT NULL DEFAULT 0,
                                    CompletionDate DATETIME NULL,
                                    QuizScore INT NULL,
                                    QuizTotal INT NULL DEFAULT 2,
                                    CONSTRAINT FK_Enrollments_Users FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
                                    CONSTRAINT FK_Enrollments_Courses FOREIGN KEY (CourseId) REFERENCES Courses(CourseId) ON DELETE CASCADE,
                                    CONSTRAINT UQ_User_Course UNIQUE(UserId, CourseId)
                                );
                            END
                            ELSE
                            BEGIN
                                IF COL_LENGTH('Enrollments', 'IsCompleted') IS NULL
                                    ALTER TABLE Enrollments ADD IsCompleted BIT NOT NULL DEFAULT 0;
                                IF COL_LENGTH('Enrollments', 'CompletionDate') IS NULL
                                    ALTER TABLE Enrollments ADD CompletionDate DATETIME NULL;
                                IF COL_LENGTH('Enrollments', 'QuizScore') IS NULL
                                    ALTER TABLE Enrollments ADD QuizScore INT NULL;
                                IF COL_LENGTH('Enrollments', 'QuizTotal') IS NULL
                                    ALTER TABLE Enrollments ADD QuizTotal INT NULL DEFAULT 2;
                            END

                            -- 6. Course Materials Table (File Uploads & Text Content)
                            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='CourseMaterials' AND xtype='U')
                            BEGIN
                                CREATE TABLE CourseMaterials (
                                    MaterialId INT IDENTITY(1,1) PRIMARY KEY,
                                    CourseId INT NOT NULL,
                                    MaterialType NVARCHAR(20) NOT NULL, -- 'File' or 'Text'
                                    Title NVARCHAR(200) NOT NULL,
                                    TextContent NVARCHAR(MAX) NULL,
                                    FileName NVARCHAR(255) NULL,
                                    FilePath NVARCHAR(500) NULL,
                                    FileSize INT NULL,
                                    UploadedBy NVARCHAR(100) NULL,
                                    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
                                    CONSTRAINT FK_Materials_Courses FOREIGN KEY (CourseId) REFERENCES Courses(CourseId) ON DELETE CASCADE
                                );
                            END

                            -- 7. Course Quizzes Table (Exactly Two Questions per Course)
                            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='CourseQuizzes' AND xtype='U')
                            BEGIN
                                CREATE TABLE CourseQuizzes (
                                    QuizId INT IDENTITY(1,1) PRIMARY KEY,
                                    CourseId INT NOT NULL,
                                    QuestionNumber INT NOT NULL, -- 1 or 2
                                    QuestionText NVARCHAR(500) NOT NULL,
                                    OptionA NVARCHAR(255) NOT NULL,
                                    OptionB NVARCHAR(255) NOT NULL,
                                    OptionC NVARCHAR(255) NOT NULL,
                                    OptionD NVARCHAR(255) NOT NULL,
                                    CorrectOption CHAR(1) NOT NULL, -- 'A', 'B', 'C', 'D'
                                    CONSTRAINT FK_Quizzes_Courses FOREIGN KEY (CourseId) REFERENCES Courses(CourseId) ON DELETE CASCADE,
                                    CONSTRAINT UQ_Course_Question UNIQUE(CourseId, QuestionNumber)
                                );
                            END

                            -- 8. Seed Initial Materials & 2-Question Quizzes for Sample Courses if empty
                            IF NOT EXISTS (SELECT TOP 1 1 FROM CourseMaterials)
                            BEGIN
                                DECLARE @c1 INT = (SELECT TOP 1 CourseId FROM Courses WHERE CourseCode = 'CS-301');
                                DECLARE @c2 INT = (SELECT TOP 1 CourseId FROM Courses WHERE CourseCode = 'DB-201');

                                IF @c1 IS NOT NULL
                                BEGIN
                                    INSERT INTO CourseMaterials (CourseId, MaterialType, Title, TextContent, UploadedBy, CreatedAt)
                                    VALUES 
                                    (@c1, 'Text', 'Module 1: Introduction to ASP.NET Web Forms Architecture', 
                                     'Welcome to ASP.NET Web Forms. In this module, we examine the page lifecycle events: Page_Init, Page_Load, Control Events, and Page_PreRender. Understanding ViewState and Server Controls enables rapid enterprise application development.', 
                                     'Dr. Robert Smith', GETDATE()),
                                    (@c1, 'Text', 'Module 2: ADO.NET and Database Connectivity Best Practices', 
                                     'In this module, you will learn to use SqlConnection, SqlCommand with parameterized queries (@params) to prevent SQL Injection, and SqlDataAdapter to populate DataTables and bind to GridViews securely.', 
                                     'Dr. Robert Smith', GETDATE());
                                END

                                IF @c2 IS NOT NULL
                                BEGIN
                                    INSERT INTO CourseMaterials (CourseId, MaterialType, Title, TextContent, UploadedBy, CreatedAt)
                                    VALUES 
                                    (@c2, 'Text', 'Lesson 1: Relational Database Normalization & Keys', 
                                     'Database normalization is the process of structuring a relational database in accordance with normal forms (1NF, 2NF, 3NF) in order to reduce data redundancy and improve data integrity.', 
                                     'Sarah Jenkins', GETDATE());
                                END
                            END

                            IF NOT EXISTS (SELECT TOP 1 1 FROM CourseQuizzes)
                            BEGIN
                                DECLARE @q_c1 INT = (SELECT TOP 1 CourseId FROM Courses WHERE CourseCode = 'CS-301');
                                DECLARE @q_c2 INT = (SELECT TOP 1 CourseId FROM Courses WHERE CourseCode = 'DB-201');
                                DECLARE @q_c3 INT = (SELECT TOP 1 CourseId FROM Courses WHERE CourseCode = 'WEB-101');
                                DECLARE @q_c4 INT = (SELECT TOP 1 CourseId FROM Courses WHERE CourseCode = 'CLOUD-401');

                                IF @q_c1 IS NOT NULL
                                BEGIN
                                    INSERT INTO CourseQuizzes (CourseId, QuestionNumber, QuestionText, OptionA, OptionB, OptionC, OptionD, CorrectOption)
                                    VALUES 
                                    (@q_c1, 1, 'Which page lifecycle event in ASP.NET occurs first during initial request processing?', 
                                     'Page_Load', 'Page_Init', 'Page_PreRender', 'Page_Unload', 'B'),
                                    (@q_c1, 2, 'Why are parameterized SQL queries strongly recommended in ADO.NET?', 
                                     'To decrease memory usage', 'To prevent SQL Injection attacks', 'To automatically encrypt the database', 'To avoid using connection strings', 'B');
                                END

                                IF @q_c2 IS NOT NULL
                                BEGIN
                                    INSERT INTO CourseQuizzes (CourseId, QuestionNumber, QuestionText, OptionA, OptionB, OptionC, OptionD, CorrectOption)
                                    VALUES 
                                    (@q_c2, 1, 'Which SQL constraint uniquely identifies each row in a database table and disallows NULL values?', 
                                     'FOREIGN KEY', 'CHECK CONSTRAINT', 'PRIMARY KEY', 'DEFAULT', 'C'),
                                    (@q_c2, 2, 'Which normal form eliminates partial dependency on a composite primary key?', 
                                     'First Normal Form (1NF)', 'Second Normal Form (2NF)', 'Third Normal Form (3NF)', 'Boyce-Codd Normal Form (BCNF)', 'B');
                                END

                                IF @q_c3 IS NOT NULL
                                BEGIN
                                    INSERT INTO CourseQuizzes (CourseId, QuestionNumber, QuestionText, OptionA, OptionB, OptionC, OptionD, CorrectOption)
                                    VALUES 
                                    (@q_c3, 1, 'In Bootstrap 5, which CSS class creates a responsive flexible grid container?', 
                                     'container-fluid', 'row', 'd-grid', 'col-12', 'B'),
                                    (@q_c3, 2, 'Which HTML5 semantic element is most suitable for encapsulating primary navigation links?', 
                                     '<section>', '<header>', '<nav>', '<aside>', 'C');
                                END

                                IF @q_c4 IS NOT NULL
                                BEGIN
                                    INSERT INTO CourseQuizzes (CourseId, QuestionNumber, QuestionText, OptionA, OptionB, OptionC, OptionD, CorrectOption)
                                    VALUES 
                                    (@q_c4, 1, 'What is the primary benefit of containerizing applications using Docker?', 
                                     'Replaces the operating system kernel', 'Guarantees uniform execution across different environments', 'Eliminates need for source code', 'Provides automatic database indexing', 'B'),
                                    (@q_c4, 2, 'In cloud computing, what does CI/CD stand for?', 
                                     'Cloud Infrastructure / Cloud Deployment', 'Continuous Integration / Continuous Delivery', 'Centralized Identity / Centralized Data', 'Containerized Isolation / Container Distribution', 'B');
                                END
                            END
                        ";

                        using (var cmd = new SqlCommand(initSql, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                    _isDbInitialized = true;
                }
                catch (Exception)
                {
                    // If DB is offline or creation fails at startup, let callers capture specific SQL exceptions
                }
            }
        }

        /// <summary>
        /// Executes a SELECT query with parameters and returns a populated DataTable.
        /// </summary>
        public static DataTable ExecuteSelect(string query, SqlParameter[] parameters = null)
        {
            var dt = new DataTable();
            using (var conn = GetConnection())
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// Executes an INSERT, UPDATE, or DELETE parameterized command and returns affected rows count.
        /// </summary>
        public static int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            using (var conn = GetConnection())
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Executes a scalar query returning a single value (e.g., COUNT or SCOPE_IDENTITY).
        /// </summary>
        public static object ExecuteScalar(string query, SqlParameter[] parameters = null)
        {
            using (var conn = GetConnection())
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }
    }
}
