-- ============================================================================
-- Course Management System - Database Initialization Script
-- SQL Server / LocalDB Schema with Roles (Admin, Student) & Enrollments
-- ============================================================================

-- Use CourseManagementDb or master to ensure DB exists
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'CourseManagementDb')
BEGIN
    CREATE DATABASE CourseManagementDb;
END
GO

USE CourseManagementDb;
GO

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
GO

-- 2. Seed Default Admin & Student Accounts
IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = 'admin')
BEGIN
    INSERT INTO Users (Username, Password, FullName, Email, Role)
    VALUES ('admin', 'admin123', 'System Administrator', 'admin@system.local', 'Admin');
END

IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = 'student')
BEGIN
    INSERT INTO Users (Username, Password, FullName, Email, Role)
    VALUES ('student', 'student123', 'Demo Student', 'student@system.local', 'Student');
END
GO

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
GO

-- 4. Seed Initial Sample Courses
IF NOT EXISTS (SELECT TOP 1 1 FROM Courses)
BEGIN
    INSERT INTO Courses (CourseName, CourseCode, Description, InstructorName, Category, DurationInWeeks, CourseFee, StartDate, EndDate, IsActive, CreatedAt)
    VALUES 
    ('ASP.NET Web Forms & C# Foundations', 'CS-301', 'Comprehensive deep dive into building enterprise Web Forms applications with C# and ADO.NET.', 'Dr. Robert Smith', 'Software Development', 10, 499.00, DATEADD(day, 5, GETDATE()), DATEADD(day, 75, GETDATE()), 1, GETDATE()),
    ('Microsoft SQL Server & Database Design', 'DB-201', 'Master relational database architecture, indexing, stored procedures, and T-SQL optimization.', 'Sarah Jenkins', 'Database Systems', 8, 350.00, DATEADD(day, 10, GETDATE()), DATEADD(day, 66, GETDATE()), 1, GETDATE()),
    ('Full Stack Web Development with Bootstrap 5', 'WEB-101', 'Learn responsive UI development, semantic HTML5, modern CSS, and JavaScript fundamentals.', 'Alex Rivera', 'Web Development', 12, 550.00, DATEADD(day, 14, GETDATE()), DATEADD(day, 98, GETDATE()), 1, GETDATE()),
    ('Cloud Architecture & Microservices', 'CLOUD-401', 'Architect resilient cloud-native solutions, containerization with Docker, and CI/CD pipelines.', 'Emily Watson', 'Cloud Computing', 6, 600.00, DATEADD(day, 20, GETDATE()), DATEADD(day, 62, GETDATE()), 0, GETDATE());
END
GO

-- 5. Enrollments Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Enrollments' AND xtype='U')
BEGIN
    CREATE TABLE Enrollments (
        EnrollmentId INT IDENTITY(1,1) PRIMARY KEY,
        UserId INT NOT NULL,
        CourseId INT NOT NULL,
        EnrollmentDate DATETIME NOT NULL DEFAULT GETDATE(),
        Status NVARCHAR(20) NOT NULL DEFAULT 'Enrolled',
        CONSTRAINT FK_Enrollments_Users FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
        CONSTRAINT FK_Enrollments_Courses FOREIGN KEY (CourseId) REFERENCES Courses(CourseId) ON DELETE CASCADE,
        CONSTRAINT UQ_User_Course UNIQUE(UserId, CourseId)
    );
END
GO

-- Quick verification query
SELECT 'Users' AS TableName, COUNT(*) AS TotalRecords FROM Users
UNION ALL
SELECT 'Courses', COUNT(*) FROM Courses
UNION ALL
SELECT 'Enrollments', COUNT(*) FROM Enrollments;
GO
