# 🎓 Course Management System (CMS)

A full-featured, responsive web application for managing academic courses, student registrations, and enrollments built with **ASP.NET Web Forms (.NET Framework 4.8)**, **C#**, **ADO.NET**, **Microsoft SQL Server**, and **Bootstrap 5**.

---

## 📌 Repository Summary

> **Short Description (for GitHub About section):**  
> *A responsive Course Management System built with ASP.NET Web Forms, C#, ADO.NET, and SQL Server featuring role-based access control (Admin & Student), full course CRUD, and student enrollment workflows.*

**Suggested Repository Topics/Tags:**  
`csharp` `asp-net` `webforms` `dotnet-framework` `sql-server` `ado-net` `bootstrap5` `course-management-system` `student-portal` `crud-application`

---

## 🚀 Key Features

### 🔐 Authentication & Role-Based Access Control (RBAC)
- **User Registration & Login**: Secure account creation with dedicated roles (`Admin` and `Student`).
- **Session-Based Authentication**: Context-aware navigation bars and protected actions based on user identity and role.

### 📚 Course Management (Admin)
- **Full CRUD Operations**: Create, view details, update, and delete courses.
- **Rich Course Attributes**: Track course code, name, category, instructor, duration (weeks), course fee, start/end dates, and active status.
- **Search, Filter & Sort**: Real-time filtering by category, search by keywords (title, code, instructor), and sorting by name, fee, or duration.

### 📝 Student Enrollment Workflow (Student)
- **Course Catalog & Exploration**: Students can browse active courses and view full syllabus and instructor details.
- **One-Click Enrollment**: Seamless self-enrollment into open courses with duplicate-enrollment prevention.
- **My Enrollments Dashboard**: Dedicated portal for students to track enrolled courses, enrollment dates, and status.
- **Admin Enrollment Oversight**: Centralized view for administrators to track and manage student enrollments across all courses.

### 🎨 Modern & Responsive User Interface
- Styled with **Bootstrap 5.3** and **Bootstrap Icons**.
- Clean typography using Google Fonts (**Plus Jakarta Sans**).
- Stat cards, status badges, and mobile-friendly responsive layouts.

---

## 🛠️ Tech Stack & Architecture

- **Backend:** C# (.NET Framework 4.8), ASP.NET Web Forms
- **Data Access:** ADO.NET (`SqlConnection`, `SqlCommand`, parameterized queries for SQL injection protection)
- **Database:** Microsoft SQL Server / LocalDB
- **Frontend:** HTML5, CSS3, JavaScript, Bootstrap 5.3.3, Bootstrap Icons
- **Web Server:** IIS Express / IIS

---

## 🗄️ Database Schema

The database consists of three relational tables:

1. **`Users`**: Stores credentials, full name, email, and user role (`Admin` or `Student`).
2. **`Courses`**: Stores course metadata, pricing, duration, schedules, and active state.
3. **`Enrollments`**: Junction table managing student-course relationships (`UserId`, `CourseId`, `EnrollmentDate`, `Status`).

---

## ⚙️ Getting Started

### Prerequisites
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (with **ASP.NET and web development** workload)
- .NET Framework 4.8 Developer Pack
- Microsoft SQL Server (LocalDB, Express, or Developer edition)

### Setup Instructions

1. **Clone the Repository**
   ```bash
   git clone https://github.com/your-username/Course-Management-System.git
   cd Course-Management-System
   ```

2. **Initialize the Database**
   - Open **SQL Server Management Studio (SSMS)** or Visual Studio **SQL Server Object Explorer**.
   - Execute the script located at:
     ```
     CourseManagementSystem/DatabaseSetup.sql
     ```
   - This creates `CourseManagementDb`, the required tables, foreign keys, and seed data.

3. **Configure the Connection String**
   - Open `CourseManagementSystem/Web.config`.
   - Update the `CourseManagementDb` connection string to point to your SQL Server instance if needed:
     ```xml
     <connectionStrings>
       <add name="CourseManagementDb" 
            connectionString="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CourseManagementDb;Integrated Security=True;" 
            providerName="System.Data.SqlClient" />
     </connectionStrings>
     ```

4. **Run the Application**
   - Open `CourseManagementSystem.sln` in Visual Studio.
   - Set `Index.aspx` or `Default.aspx` as the Start Page.
   - Press **F5** or click **IIS Express** to build and run.

---

## 🔑 Default Credentials (Seed Data)

The initialization script pre-populates two demo accounts:

| Role | Username | Password | Access Level |
| :--- | :--- | :--- | :--- |
| **Admin** | `admin` | `admin123` | Full Course CRUD & All Enrollments |
| **Student** | `student` | `student123` | Course Browsing & Enrollment Dashboard |

---

## 📂 Project Structure

```text
CourseManagementSystem/
├── App_Data/
├── bin/
├── Properties/
├── Create.aspx / .cs          # Add new course (Admin)
├── Edit.aspx / .cs            # Modify course details (Admin)
├── Delete.aspx / .cs          # Course deletion confirmation (Admin)
├── Details.aspx / .cs         # Detailed course view
├── Index.aspx / .cs           # Course catalog with search & filters
├── Enrollments.aspx / .cs     # Global enrollment management (Admin)
├── MyEnrollments.aspx / .cs   # Student enrollment dashboard (Student)
├── Login.aspx / .cs           # User login portal
├── Register.aspx / .cs        # Student registration page
├── Logout.aspx / .cs          # Session termination
├── Site.Master / .cs          # Main responsive layout template
├── DBHelper.cs                # ADO.NET helper for SQL operations
├── DatabaseSetup.sql          # DB schema & seed data SQL script
└── Web.config                 # Application settings & connection strings
```

---

## 📄 License
This project is licensed under the [MIT License](LICENSE).
