import os
import matplotlib
matplotlib.use('Agg')
import matplotlib.pyplot as plt
import matplotlib.patches as patches

# Ensure diagrams directory
out_dir = r"D:\7 Semester\c# and .NET\Course_management\CourseManagementSystem\screenshots"
os.makedirs(out_dir, exist_ok=True)

# -------------------------------------------------------------
# 1. System Architecture Diagram (Figure 3.1)
# -------------------------------------------------------------
def create_architecture_diagram():
    fig, ax = plt.subplots(figsize=(10, 7), dpi=300)
    ax.set_xlim(0, 10)
    ax.set_ylim(0, 10)
    ax.axis('off')

    # Title
    ax.text(5, 9.6, "System Architecture: 3-Tier Layered Architecture", 
            ha='center', va='center', fontsize=14, fontweight='bold', color='#1e3a8a')

    # Layer 1: Presentation Layer
    p1 = patches.FancyBboxPatch((0.8, 7.3), 8.4, 1.8, boxstyle="round,pad=0.2", 
                                edgecolor='#1e40af', facecolor='#eff6ff', lw=2)
    ax.add_patch(p1)
    ax.text(1.2, 8.7, "1. Presentation Layer (Client Tier)", fontsize=11, fontweight='bold', color='#1e3a8a')
    ax.text(5, 8.0, "Web Browser (Chrome / Edge / Firefox) | HTML5 | CSS3 | JavaScript\nBootstrap 5.3.3 UI Framework | ASP.NET Web Forms (.aspx / Site.Master)\nClient-Side Responsive UI & Form Validation Controls", 
            ha='center', va='center', fontsize=9.5, color='#1e293b')

    # Arrow 1 <-> 2
    ax.annotate('', xy=(5, 6.7), xytext=(5, 7.2),
                arrowprops=dict(arrowstyle='<->', color='#2563eb', lw=2))
    ax.text(5.2, 6.95, "HTTP / HTTPS Requests & Responses", fontsize=8.5, color='#475569', style='italic')

    # Layer 2: Application / Business Logic Layer
    p2 = patches.FancyBboxPatch((0.8, 4.3), 8.4, 2.3, boxstyle="round,pad=0.2", 
                                edgecolor='#047857', facecolor='#ecfdf5', lw=2)
    ax.add_patch(p2)
    ax.text(1.2, 6.2, "2. Application / Business Logic Layer (Server Tier)", fontsize=11, fontweight='bold', color='#065f46')
    ax.text(5, 5.25, "Microsoft .NET Framework 4.8 | IIS Express Web Server\nC# Server-Side Code-Behind (.aspx.cs) & Page Lifecycle Handlers\nRole-Based Session State Management (Admin, Instructor, Student)\nCourse Catalog Engine | Enrollment Controller | Quiz Evaluator", 
            ha='center', va='center', fontsize=9.5, color='#1e293b')

    # Arrow 2 <-> 3
    ax.annotate('', xy=(5, 3.6), xytext=(5, 4.2),
                arrowprops=dict(arrowstyle='<->', color='#059669', lw=2))
    ax.text(5.2, 3.9, "Data Access Calls (DBHelper.cs)", fontsize=8.5, color='#475569', style='italic')

    # Layer 3: Data Access & Database Tier
    p3 = patches.FancyBboxPatch((0.8, 0.7), 8.4, 2.7, boxstyle="round,pad=0.2", 
                                edgecolor='#b45309', facecolor='#fffbeb', lw=2)
    ax.add_patch(p3)
    ax.text(1.2, 3.0, "3. Data Access & Persistence Layer (Database Tier)", fontsize=11, fontweight='bold', color='#92400e')
    
    # Sub box: ADO.NET
    sub1 = patches.FancyBboxPatch((1.2, 1.1), 3.5, 1.5, boxstyle="round,pad=0.1", 
                                 edgecolor='#d97706', facecolor='#ffffff', lw=1.5)
    ax.add_patch(sub1)
    ax.text(2.95, 2.2, "ADO.NET Data Access (DBHelper)", ha='center', fontsize=9, fontweight='bold', color='#92400e')
    ax.text(2.95, 1.6, "SqlConnection | SqlCommand\nSqlDataAdapter | DataTable\nParameterized SQL (@params)", ha='center', fontsize=8, color='#334155')

    # Sub box: SQL Server LocalDB
    sub2 = patches.FancyBboxPatch((5.3, 1.1), 3.5, 1.5, boxstyle="round,pad=0.1", 
                                 edgecolor='#d97706', facecolor='#ffffff', lw=1.5)
    ax.add_patch(sub2)
    ax.text(7.05, 2.2, "SQL Server LocalDB", ha='center', fontsize=9, fontweight='bold', color='#92400e')
    ax.text(7.05, 1.6, "Database: CourseManagementDb\nUsers, Courses, Enrollments,\nCourseMaterials, CourseQuizzes", ha='center', fontsize=8, color='#334155')

    # Arrow between ADO.NET and SQL Server
    ax.annotate('', xy=(5.2, 1.85), xytext=(4.8, 1.85),
                arrowprops=dict(arrowstyle='<->', color='#d97706', lw=1.5))

    plt.tight_layout()
    fig.savefig(os.path.join(out_dir, "figure_3_1_system_architecture.png"), dpi=300)
    plt.close()
    print("Created figure_3_1_system_architecture.png")

# -------------------------------------------------------------
# 2. Use Case Diagram (Figure 3.2)
# -------------------------------------------------------------
def create_use_case_diagram():
    fig, ax = plt.subplots(figsize=(10, 8), dpi=300)
    ax.set_xlim(0, 10)
    ax.set_ylim(0, 10)
    ax.axis('off')

    ax.text(5, 9.6, "Use Case Diagram: Course Management System", 
            ha='center', va='center', fontsize=14, fontweight='bold', color='#1e3a8a')

    # Boundary Box
    boundary = patches.FancyBboxPatch((2.2, 0.4), 5.6, 8.8, boxstyle="round,pad=0.1", 
                                     edgecolor='#334155', facecolor='#f8fafc', lw=2)
    ax.add_patch(boundary)
    ax.text(5.0, 9.0, "Course Management System Boundary", ha='center', fontsize=10.5, fontweight='bold', color='#475569')

    # Use cases list
    ucs = [
        ("UC1: Authenticate & Select Role (Admin / Inst / Student)", 8.2),
        ("UC2: View Course Catalog & Filter/Search Courses", 7.4),
        ("UC3: Create New Course Record", 6.6),
        ("UC4: Edit & Update Existing Course Details", 5.8),
        ("UC5: Delete Course Record with Confirmation", 5.0),
        ("UC6: Upload Course Materials (Files & Text Modules)", 4.2),
        ("UC7: Enroll in Available Course", 3.4),
        ("UC8: Access Study Room & View/Download Materials", 2.6),
        ("UC9: Take Interactive 2-Question Course Quiz", 1.8),
        ("UC10: Track Student Enrollments & Course Completions", 1.0)
    ]

    uc_centers = {}
    for name, y in ucs:
        uc_box = patches.Ellipse((5.0, y), 4.8, 0.58, edgecolor='#2563eb', facecolor='#dbeafe', lw=1.5)
        ax.add_patch(uc_box)
        ax.text(5.0, y, name, ha='center', va='center', fontsize=8, fontweight='semibold', color='#1e3a8a')
        uc_centers[name[:3]] = (2.6, y, 7.4, y)

    # Actor 1: Administrator (Left side, top)
    ax.text(1.0, 7.6, "Administrator", ha='center', fontsize=10, fontweight='bold', color='#1e293b')
    ax.plot([1.0], [7.1], marker='o', markersize=14, color='#1e3a8a') # Head
    ax.plot([1.0, 1.0], [6.95, 6.3], color='#1e3a8a', lw=2.5) # Body
    ax.plot([0.6, 1.4], [6.65, 6.65], color='#1e3a8a', lw=2) # Arms
    ax.plot([1.0, 0.7], [6.3, 5.7], color='#1e3a8a', lw=2) # Left leg
    ax.plot([1.0, 1.3], [6.3, 5.7], color='#1e3a8a', lw=2) # Right leg

    # Actor 2: Instructor (Left side, middle/bottom)
    ax.text(1.0, 4.3, "Instructor", ha='center', fontsize=10, fontweight='bold', color='#1e293b')
    ax.plot([1.0], [3.8], marker='o', markersize=14, color='#d97706') # Head
    ax.plot([1.0, 1.0], [3.65, 3.0], color='#d97706', lw=2.5) # Body
    ax.plot([0.6, 1.4], [3.35, 3.35], color='#d97706', lw=2) # Arms
    ax.plot([1.0, 0.7], [3.0, 2.4], color='#d97706', lw=2) # Left leg
    ax.plot([1.0, 1.3], [3.0, 2.4], color='#d97706', lw=2) # Right leg

    # Actor 3: Student (Right side, center)
    ax.text(9.0, 4.8, "Student", ha='center', fontsize=10, fontweight='bold', color='#1e293b')
    ax.plot([9.0], [4.3], marker='o', markersize=14, color='#059669') # Head
    ax.plot([9.0, 9.0], [4.15, 3.5], color='#059669', lw=2.5) # Body
    ax.plot([8.6, 9.4], [3.85, 3.85], color='#059669', lw=2) # Arms
    ax.plot([9.0, 8.7], [3.5, 2.9], color='#059669', lw=2) # Left leg
    ax.plot([9.0, 9.3], [3.5, 2.9], color='#059669', lw=2) # Right leg

    # Admin lines: UC1, UC2, UC3, UC4, UC5, UC6, UC10
    admin_targets = [8.2, 7.4, 6.6, 5.8, 5.0, 4.2, 1.0]
    for ty in admin_targets:
        ax.plot([1.4, 2.6], [6.65, ty], color='#1e3a8a', lw=1.2, linestyle='-')

    # Instructor lines: UC1, UC2, UC4, UC6, UC10
    inst_targets = [8.2, 7.4, 5.8, 4.2, 1.0]
    for ty in inst_targets:
        ax.plot([1.4, 2.6], [3.35, ty], color='#d97706', lw=1.2, linestyle='-')

    # Student lines: UC1, UC2, UC7, UC8, UC9
    student_targets = [8.2, 7.4, 3.4, 2.6, 1.8]
    for ty in student_targets:
        ax.plot([8.6, 7.4], [3.85, ty], color='#059669', lw=1.2, linestyle='-')

    plt.tight_layout()
    fig.savefig(os.path.join(out_dir, "figure_3_2_use_case_diagram.png"), dpi=300)
    plt.close()
    print("Created figure_3_2_use_case_diagram.png")

# -------------------------------------------------------------
# 3. Context Level DFD (Level 0) (Figure 3.3)
# -------------------------------------------------------------
def create_dfd_context():
    fig, ax = plt.subplots(figsize=(10, 6.5), dpi=300)
    ax.set_xlim(0, 10)
    ax.set_ylim(0, 10)
    ax.axis('off')

    ax.text(5, 9.5, "Data Flow Diagram: Context Level (Level 0)", 
            ha='center', va='center', fontsize=14, fontweight='bold', color='#1e3a8a')

    # Central Process: 0.0 Course Management System
    central_proc = patches.Circle((5, 5), 1.6, edgecolor='#1e40af', facecolor='#dbeafe', lw=2.5)
    ax.add_patch(central_proc)
    ax.text(5, 5.4, "0.0", ha='center', va='center', fontsize=12, fontweight='bold', color='#1e3a8a')
    ax.text(5, 4.8, "Course Management\nSystem", ha='center', va='center', fontsize=11, fontweight='bold', color='#1e3a8a')

    # Entity 1: Administrator (Top Left)
    admin_box = patches.Rectangle((0.8, 6.8), 2.4, 1.4, edgecolor='#1e293b', facecolor='#f1f5f9', lw=2)
    ax.add_patch(admin_box)
    ax.text(2.0, 7.5, "Administrator", ha='center', va='center', fontsize=10.5, fontweight='bold', color='#1e293b')

    # Entity 2: Instructor (Bottom Left)
    inst_box = patches.Rectangle((0.8, 1.8), 2.4, 1.4, edgecolor='#1e293b', facecolor='#f1f5f9', lw=2)
    ax.add_patch(inst_box)
    ax.text(2.0, 2.5, "Instructor", ha='center', va='center', fontsize=10.5, fontweight='bold', color='#1e293b')

    # Entity 3: Student (Right Side)
    student_box = patches.Rectangle((6.8, 4.3), 2.4, 1.4, edgecolor='#1e293b', facecolor='#f1f5f9', lw=2)
    ax.add_patch(student_box)
    ax.text(8.0, 5.0, "Student", ha='center', va='center', fontsize=10.5, fontweight='bold', color='#1e293b')

    # Data flows Admin -> System
    ax.annotate('', xy=(3.8, 6.0), xytext=(2.8, 6.8),
                arrowprops=dict(arrowstyle='->', color='#1e3a8a', lw=1.5))
    ax.text(2.5, 6.5, "Course Data, Materials,\nQuiz Setup, Updates", fontsize=7.5, color='#1e3a8a')

    # Data flows System -> Admin
    ax.annotate('', xy=(2.2, 6.7), xytext=(3.6, 5.4),
                arrowprops=dict(arrowstyle='->', color='#1e3a8a', lw=1.5, linestyle='--'))
    ax.text(3.1, 5.7, "Course Reports, Enrolled\nStudent Lists, KPIs", fontsize=7.5, color='#475569')

    # Data flows Inst -> System
    ax.annotate('', xy=(3.8, 4.0), xytext=(2.8, 3.2),
                arrowprops=dict(arrowstyle='->', color='#d97706', lw=1.5))
    ax.text(2.5, 3.6, "Course Lessons, Quizzes,\nMaterial Uploads", fontsize=7.5, color='#92400e')

    # Data flows System -> Inst
    ax.annotate('', xy=(2.4, 3.3), xytext=(3.6, 4.4),
                arrowprops=dict(arrowstyle='->', color='#d97706', lw=1.5, linestyle='--'))
    ax.text(2.2, 4.4, "Student Completion Data,\nAssigned Courses", fontsize=7.5, color='#475569')

    # Data flows Student -> System
    ax.annotate('', xy=(6.5, 5.3), xytext=(6.8, 5.3),
                arrowprops=dict(arrowstyle='->', color='#059669', lw=1.5))
    ax.text(6.7, 5.7, "Registration, Enrollment Request,\nQuiz Submissions", fontsize=7.5, color='#065f46')

    # Data flows System -> Student
    ax.annotate('', xy=(6.8, 4.7), xytext=(6.5, 4.7),
                arrowprops=dict(arrowstyle='->', color='#059669', lw=1.5, linestyle='--'))
    ax.text(6.7, 4.0, "Catalog, Study Notes, File Downloads,\nQuiz Scores, Completion Status", fontsize=7.5, color='#475569')

    plt.tight_layout()
    fig.savefig(os.path.join(out_dir, "figure_3_3_dfd_context_level0.png"), dpi=300)
    plt.close()
    print("Created figure_3_3_dfd_context_level0.png")

# -------------------------------------------------------------
# 4. Level 1 Data Flow Diagram (Figure 3.4)
# -------------------------------------------------------------
def create_dfd_level1():
    fig, ax = plt.subplots(figsize=(10, 8), dpi=300)
    ax.set_xlim(0, 10)
    ax.set_ylim(0, 10)
    ax.axis('off')

    ax.text(5, 9.6, "Data Flow Diagram: Level 1 (Functional Deconstruction)", 
            ha='center', va='center', fontsize=14, fontweight='bold', color='#1e3a8a')

    # External Entities
    e_user = patches.Rectangle((0.6, 7.8), 1.8, 1.0, edgecolor='#1e293b', facecolor='#f1f5f9', lw=2)
    ax.add_patch(e_user)
    ax.text(1.5, 8.3, "User\n(Admin/Student)", ha='center', va='center', fontsize=9, fontweight='bold')

    # Process 1.0: Authentication
    p1 = patches.Circle((4.0, 8.3), 0.8, edgecolor='#2563eb', facecolor='#dbeafe', lw=2)
    ax.add_patch(p1)
    ax.text(4.0, 8.5, "1.0", ha='center', fontsize=9, fontweight='bold', color='#1e3a8a')
    ax.text(4.0, 8.1, "Login &\nRegister", ha='center', fontsize=8, color='#1e3a8a')

    # Data Store D1: Users
    d1 = patches.Rectangle((7.0, 7.9), 2.4, 0.8, edgecolor='#059669', facecolor='#ecfdf5', lw=1.5)
    ax.add_patch(d1)
    ax.text(8.2, 8.3, "D1: Users Table", ha='center', va='center', fontsize=9, fontweight='bold', color='#065f46')

    # Process 2.0: Course Management
    p2 = patches.Circle((4.0, 6.2), 0.8, edgecolor='#2563eb', facecolor='#dbeafe', lw=2)
    ax.add_patch(p2)
    ax.text(4.0, 6.4, "2.0", ha='center', fontsize=9, fontweight='bold', color='#1e3a8a')
    ax.text(4.0, 6.0, "Course\nCRUD", ha='center', fontsize=8, color='#1e3a8a')

    # Data Store D2: Courses
    d2 = patches.Rectangle((7.0, 5.8), 2.4, 0.8, edgecolor='#059669', facecolor='#ecfdf5', lw=1.5)
    ax.add_patch(d2)
    ax.text(8.2, 6.2, "D2: Courses Table", ha='center', va='center', fontsize=9, fontweight='bold', color='#065f46')

    # Process 3.0: Enrollment Management
    p3 = patches.Circle((4.0, 4.1), 0.8, edgecolor='#2563eb', facecolor='#dbeafe', lw=2)
    ax.add_patch(p3)
    ax.text(4.0, 4.3, "3.0", ha='center', fontsize=9, fontweight='bold', color='#1e3a8a')
    ax.text(4.0, 3.9, "Enrollment\n& Tracking", ha='center', fontsize=8, color='#1e3a8a')

    # Data Store D3: Enrollments
    d3 = patches.Rectangle((7.0, 3.7), 2.4, 0.8, edgecolor='#059669', facecolor='#ecfdf5', lw=1.5)
    ax.add_patch(d3)
    ax.text(8.2, 4.1, "D3: Enrollments", ha='center', va='center', fontsize=9, fontweight='bold', color='#065f46')

    # Process 4.0: Course Study & Materials
    p4 = patches.Circle((4.0, 2.0), 0.8, edgecolor='#2563eb', facecolor='#dbeafe', lw=2)
    ax.add_patch(p4)
    ax.text(4.0, 2.2, "4.0", ha='center', fontsize=9, fontweight='bold', color='#1e3a8a')
    ax.text(4.0, 1.8, "Materials &\nQuizzes", ha='center', fontsize=8, color='#1e3a8a')

    # Data Store D4: CourseMaterials & D5: CourseQuizzes
    d4 = patches.Rectangle((7.0, 1.6), 2.4, 0.8, edgecolor='#059669', facecolor='#ecfdf5', lw=1.5)
    ax.add_patch(d4)
    ax.text(8.2, 2.0, "D4/D5: Materials/Quizzes", ha='center', va='center', fontsize=8.5, fontweight='bold', color='#065f46')

    # Connectors
    # User -> P1
    ax.annotate('', xy=(3.2, 8.3), xytext=(2.4, 8.3), arrowprops=dict(arrowstyle='<->', color='#1e3a8a', lw=1.5))
    # P1 -> D1
    ax.annotate('', xy=(7.0, 8.3), xytext=(4.8, 8.3), arrowprops=dict(arrowstyle='<->', color='#1e3a8a', lw=1.5))

    # User -> P2
    ax.annotate('', xy=(3.3, 6.5), xytext=(2.0, 7.8), arrowprops=dict(arrowstyle='<->', color='#1e3a8a', lw=1.5))
    # P2 -> D2
    ax.annotate('', xy=(7.0, 6.2), xytext=(4.8, 6.2), arrowprops=dict(arrowstyle='<->', color='#1e3a8a', lw=1.5))

    # User -> P3
    ax.annotate('', xy=(3.3, 4.4), xytext=(1.8, 7.8), arrowprops=dict(arrowstyle='<->', color='#1e3a8a', lw=1.5))
    # P3 -> D3
    ax.annotate('', xy=(7.0, 4.1), xytext=(4.8, 4.1), arrowprops=dict(arrowstyle='<->', color='#1e3a8a', lw=1.5))

    # User -> P4
    ax.annotate('', xy=(3.3, 2.3), xytext=(1.5, 7.8), arrowprops=dict(arrowstyle='<->', color='#1e3a8a', lw=1.5))
    # P4 -> D4
    ax.annotate('', xy=(7.0, 2.0), xytext=(4.8, 2.0), arrowprops=dict(arrowstyle='<->', color='#1e3a8a', lw=1.5))
    # P4 -> D3 (marks completion)
    ax.annotate('', xy=(7.0, 3.8), xytext=(4.7, 2.4), arrowprops=dict(arrowstyle='->', color='#b45309', lw=1.5, linestyle='--'))
    ax.text(5.5, 3.2, "Update Score & Completion", fontsize=7.5, color='#b45309')

    plt.tight_layout()
    fig.savefig(os.path.join(out_dir, "figure_3_4_dfd_level1.png"), dpi=300)
    plt.close()
    print("Created figure_3_4_dfd_level1.png")

# -------------------------------------------------------------
# 5. Application Flowchart (Figure 3.5)
# -------------------------------------------------------------
def create_application_flowchart():
    fig, ax = plt.subplots(figsize=(10, 8.5), dpi=300)
    ax.set_xlim(0, 10)
    ax.set_ylim(0, 10)
    ax.axis('off')

    ax.text(5, 9.7, "Application Workflow & Navigation Flowchart", 
            ha='center', va='center', fontsize=14, fontweight='bold', color='#1e3a8a')

    # Start
    start = patches.FancyBboxPatch((4.0, 9.0), 2.0, 0.5, boxstyle="round,pad=0.1", 
                                  edgecolor='#1e293b', facecolor='#e2e8f0', lw=2)
    ax.add_patch(start)
    ax.text(5.0, 9.25, "Start (Default.aspx)", ha='center', va='center', fontsize=9, fontweight='bold')

    # Decision 1: Has Session?
    d1 = patches.Polygon([[5, 8.7], [6.2, 8.2], [5, 7.7], [3.8, 8.2]], 
                         edgecolor='#2563eb', facecolor='#dbeafe', lw=1.5)
    ax.add_patch(d1)
    ax.text(5.0, 8.2, "Session\nActive?", ha='center', va='center', fontsize=8, fontweight='bold')

    # Arrow Start -> D1
    ax.annotate('', xy=(5, 8.7), xytext=(5, 9.0), arrowprops=dict(arrowstyle='->', lw=1.5))

    # No -> Login.aspx / Register.aspx
    login_box = patches.Rectangle((0.6, 7.8), 2.4, 0.8, edgecolor='#b45309', facecolor='#fef3c7', lw=1.5)
    ax.add_patch(login_box)
    ax.text(1.8, 8.2, "Login.aspx /\nRegister.aspx", ha='center', va='center', fontsize=8.5, fontweight='bold', color='#92400e')
    ax.annotate('', xy=(3.0, 8.2), xytext=(3.8, 8.2), arrowprops=dict(arrowstyle='<-', lw=1.5))
    ax.text(3.3, 8.35, "No", fontsize=8, fontweight='bold', color='#b45309')

    # Yes -> Index.aspx
    index_box = patches.Rectangle((3.8, 6.6), 2.4, 0.8, edgecolor='#1e40af', facecolor='#eff6ff', lw=2)
    ax.add_patch(index_box)
    ax.text(5.0, 7.0, "Index.aspx\n(Course Catalog)", ha='center', va='center', fontsize=9, fontweight='bold', color='#1e3a8a')
    ax.annotate('', xy=(5, 7.4), xytext=(5, 7.7), arrowprops=dict(arrowstyle='->', lw=1.5))
    ax.text(5.1, 7.55, "Yes", fontsize=8, fontweight='bold', color='#1e40af')

    # Login connects back to Index.aspx after successful auth
    ax.annotate('', xy=(3.8, 7.0), xytext=(1.8, 7.8), arrowprops=dict(arrowstyle='->', color='#059669', lw=1.5, linestyle='--'))
    ax.text(2.3, 7.3, "Auth OK", fontsize=7.5, color='#059669', fontweight='bold')

    # Role Decision
    d2 = patches.Polygon([[5, 6.3], [6.3, 5.8], [5, 5.3], [3.7, 5.8]], 
                         edgecolor='#2563eb', facecolor='#dbeafe', lw=1.5)
    ax.add_patch(d2)
    ax.text(5.0, 5.8, "User\nRole?", ha='center', va='center', fontsize=8, fontweight='bold')
    ax.annotate('', xy=(5, 6.3), xytext=(5, 6.6), arrowprops=dict(arrowstyle='->', lw=1.5))

    # Branch Admin (Left): Create / Edit / Delete / Instructors
    admin_ops = patches.FancyBboxPatch((0.5, 3.4), 3.0, 1.6, boxstyle="round,pad=0.1", 
                                      edgecolor='#1e40af', facecolor='#f8fafc', lw=1.5)
    ax.add_patch(admin_ops)
    ax.text(2.0, 4.6, "Administrator Operations", ha='center', fontsize=8.5, fontweight='bold', color='#1e3a8a')
    ax.text(2.0, 3.9, "- Create.aspx (Add Course)\n- Edit.aspx (Modify Course)\n- Delete.aspx (Remove Record)\n- Instructors.aspx & Enrollments.aspx", 
            ha='center', fontsize=7.5, color='#334155')
    ax.annotate('', xy=(2.0, 5.0), xytext=(3.7, 5.8), arrowprops=dict(arrowstyle='->', lw=1.5))
    ax.text(2.7, 5.6, "Admin", fontsize=8, fontweight='bold', color='#1e3a8a')

    # Branch Student (Right): Enroll / MyEnrollments / Study & Quiz
    student_ops = patches.FancyBboxPatch((6.5, 3.4), 3.0, 1.6, boxstyle="round,pad=0.1", 
                                        edgecolor='#059669', facecolor='#f8fafc', lw=1.5)
    ax.add_patch(student_ops)
    ax.text(8.0, 4.6, "Student Operations", ha='center', fontsize=8.5, fontweight='bold', color='#065f46')
    ax.text(8.0, 3.9, "- Click Enroll in Course\n- MyEnrollments.aspx (Active List)\n- CourseStudy.aspx (Text & Files)\n- Take 2-Question Quiz & Submit", 
            ha='center', fontsize=7.5, color='#334155')
    ax.annotate('', xy=(8.0, 5.0), xytext=(6.3, 5.8), arrowprops=dict(arrowstyle='->', lw=1.5))
    ax.text(7.2, 5.6, "Student", fontsize=8, fontweight='bold', color='#059669')

    # Database updates
    db_box = patches.Rectangle((3.5, 2.0), 3.0, 1.0, edgecolor='#d97706', facecolor='#fef3c7', lw=1.5)
    ax.add_patch(db_box)
    ax.text(5.0, 2.5, "Database Operations (DBHelper)\nINSERT / UPDATE / DELETE / SELECT\n(SQL Server LocalDB)", 
            ha='center', va='center', fontsize=8, fontweight='bold', color='#92400e')

    ax.annotate('', xy=(3.8, 3.0), xytext=(2.5, 3.4), arrowprops=dict(arrowstyle='->', lw=1.2))
    ax.annotate('', xy=(6.2, 3.0), xytext=(7.5, 3.4), arrowprops=dict(arrowstyle='->', lw=1.2))

    # End
    end_box = patches.FancyBboxPatch((4.0, 0.6), 2.0, 0.5, boxstyle="round,pad=0.1", 
                                    edgecolor='#1e293b', facecolor='#e2e8f0', lw=2)
    ax.add_patch(end_box)
    ax.text(5.0, 0.85, "Logout / End", ha='center', va='center', fontsize=9, fontweight='bold')
    ax.annotate('', xy=(5, 1.1), xytext=(5, 2.0), arrowprops=dict(arrowstyle='->', lw=1.5))

    plt.tight_layout()
    fig.savefig(os.path.join(out_dir, "figure_3_5_application_flowchart.png"), dpi=300)
    plt.close()
    print("Created figure_3_5_application_flowchart.png")

# -------------------------------------------------------------
# 6. Entity Relationship Diagram (Figure 3.6)
# -------------------------------------------------------------
def create_er_diagram():
    fig, ax = plt.subplots(figsize=(10, 8), dpi=300)
    ax.set_xlim(0, 10)
    ax.set_ylim(0, 10)
    ax.axis('off')

    ax.text(5, 9.6, "Entity Relationship (ER) Diagram: CourseManagementDb", 
            ha='center', va='center', fontsize=14, fontweight='bold', color='#1e3a8a')

    # Entity 1: Users
    u_box = patches.Rectangle((0.6, 6.2), 2.6, 2.8, edgecolor='#1e40af', facecolor='#eff6ff', lw=2)
    ax.add_patch(u_box)
    ax.text(1.9, 8.7, "Users", ha='center', fontsize=11, fontweight='bold', color='#1e3a8a')
    ax.plot([0.6, 3.2], [8.4, 8.4], color='#1e40af', lw=1.5)
    u_attrs = [
        "PK: UserId (INT, IDENTITY)",
        "Username (NVARCHAR(50), UQ)",
        "Password (NVARCHAR(100))",
        "FullName (NVARCHAR(100))",
        "Email (NVARCHAR(100))",
        "Role (NVARCHAR(20))",
        "CreatedAt (DATETIME)"
    ]
    for i, a in enumerate(u_attrs):
        ax.text(0.7, 8.1 - i*0.28, a, fontsize=7.5, color='#1e293b')

    # Entity 2: Courses
    c_box = patches.Rectangle((6.8, 5.4), 2.8, 3.8, edgecolor='#047857', facecolor='#ecfdf5', lw=2)
    ax.add_patch(c_box)
    ax.text(8.2, 8.9, "Courses", ha='center', fontsize=11, fontweight='bold', color='#065f46')
    ax.plot([6.8, 9.6], [8.6, 8.6], color='#047857', lw=1.5)
    c_attrs = [
        "PK: CourseId (INT, IDENTITY)",
        "CourseCode (NVARCHAR(20), UQ)",
        "CourseName (NVARCHAR(100))",
        "Description (NVARCHAR(500))",
        "InstructorName (NVARCHAR(100))",
        "Category (NVARCHAR(50))",
        "DurationInWeeks (INT)",
        "CourseFee (DECIMAL(18,2))",
        "StartDate (DATETIME)",
        "EndDate (DATETIME)",
        "IsActive (BIT)",
        "CreatedAt (DATETIME)"
    ]
    for i, a in enumerate(c_attrs):
        ax.text(6.9, 8.3 - i*0.25, a, fontsize=7.2, color='#1e293b')

    # Entity 3: Enrollments (Junction table)
    e_box = patches.Rectangle((3.6, 4.4), 2.8, 2.6, edgecolor='#b45309', facecolor='#fffbeb', lw=2)
    ax.add_patch(e_box)
    ax.text(5.0, 6.7, "Enrollments", ha='center', fontsize=11, fontweight='bold', color='#92400e')
    ax.plot([3.6, 6.4], [6.4, 6.4], color='#b45309', lw=1.5)
    e_attrs = [
        "PK: EnrollmentId (INT)",
        "FK: UserId (INT) -> Users",
        "FK: CourseId (INT) -> Courses",
        "EnrollmentDate (DATETIME)",
        "Status (NVARCHAR(20))",
        "IsCompleted (BIT)",
        "CompletionDate (DATETIME)",
        "QuizScore (INT)",
        "QuizTotal (INT)"
    ]
    for i, a in enumerate(e_attrs):
        ax.text(3.7, 6.1 - i*0.22, a, fontsize=7.2, color='#1e293b')

    # Entity 4: CourseMaterials
    m_box = patches.Rectangle((6.8, 0.8), 2.8, 2.7, edgecolor='#7c3aed', facecolor='#f5f3ff', lw=2)
    ax.add_patch(m_box)
    ax.text(8.2, 3.2, "CourseMaterials", ha='center', fontsize=10.5, fontweight='bold', color='#5b21b6')
    ax.plot([6.8, 9.6], [2.9, 2.9], color='#7c3aed', lw=1.5)
    m_attrs = [
        "PK: MaterialId (INT)",
        "FK: CourseId (INT) -> Courses",
        "MaterialType (NVARCHAR(20))",
        "Title (NVARCHAR(200))",
        "TextContent (NVARCHAR(MAX))",
        "FileName (NVARCHAR(255))",
        "FilePath (NVARCHAR(500))",
        "UploadedBy (NVARCHAR(100))",
        "CreatedAt (DATETIME)"
    ]
    for i, a in enumerate(m_attrs):
        ax.text(6.9, 2.65 - i*0.22, a, fontsize=7.2, color='#1e293b')

    # Entity 5: CourseQuizzes
    q_box = patches.Rectangle((0.6, 0.8), 2.7, 2.7, edgecolor='#c026d3', facecolor='#fdf4ff', lw=2)
    ax.add_patch(q_box)
    ax.text(1.95, 3.2, "CourseQuizzes", ha='center', fontsize=10.5, fontweight='bold', color='#86198f')
    ax.plot([0.6, 3.3], [2.9, 2.9], color='#c026d3', lw=1.5)
    q_attrs = [
        "PK: QuizId (INT)",
        "FK: CourseId (INT) -> Courses",
        "QuestionNumber (INT, 1 or 2)",
        "QuestionText (NVARCHAR(500))",
        "OptionA, OptionB (NVARCHAR)",
        "OptionC, OptionD (NVARCHAR)",
        "CorrectOption (CHAR(1))"
    ]
    for i, a in enumerate(q_attrs):
        ax.text(0.7, 2.65 - i*0.22, a, fontsize=7.2, color='#1e293b')

    # Relationships & Cardinalities
    # Users (1) -> (M) Enrollments
    ax.plot([3.2, 3.6], [7.0, 6.0], color='#1e40af', lw=1.5)
    ax.text(3.3, 7.1, "1", fontsize=9, fontweight='bold', color='#1e40af')
    ax.text(3.5, 6.2, "M", fontsize=9, fontweight='bold', color='#1e40af')

    # Courses (1) -> (M) Enrollments
    ax.plot([6.8, 6.4], [7.0, 6.0], color='#047857', lw=1.5)
    ax.text(6.7, 7.1, "1", fontsize=9, fontweight='bold', color='#047857')
    ax.text(6.45, 6.2, "M", fontsize=9, fontweight='bold', color='#047857')

    # Courses (1) -> (M) CourseMaterials
    ax.plot([8.2, 8.2], [5.4, 3.5], color='#7c3aed', lw=1.5)
    ax.text(8.3, 5.1, "1", fontsize=9, fontweight='bold', color='#7c3aed')
    ax.text(8.3, 3.7, "M", fontsize=9, fontweight='bold', color='#7c3aed')

    # Courses (1) -> (M) CourseQuizzes
    ax.plot([6.8, 3.3], [5.6, 2.2], color='#c026d3', lw=1.5, linestyle=':')
    ax.text(6.6, 5.4, "1", fontsize=9, fontweight='bold', color='#c026d3')
    ax.text(3.5, 2.4, "M", fontsize=9, fontweight='bold', color='#c026d3')

    plt.tight_layout()
    fig.savefig(os.path.join(out_dir, "figure_3_6_entity_relationship_diagram.png"), dpi=300)
    plt.close()
    print("Created figure_3_6_entity_relationship_diagram.png")

if __name__ == "__main__":
    create_architecture_diagram()
    create_use_case_diagram()
    create_dfd_context()
    create_dfd_level1()
    create_application_flowchart()
    create_er_diagram()
    print("All diagrams generated successfully!")
