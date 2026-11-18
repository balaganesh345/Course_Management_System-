<%@ Page Title="Course Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="CourseManagementSystem.DetailsPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .material-item {
            border: 1px solid #e2e8f0;
            border-radius: 10px;
            padding: 1rem 1.25rem;
            margin-bottom: 0.75rem;
            background: #ffffff;
            transition: all 0.2s ease;
        }
        .material-item:hover {
            border-color: #cbd5e1;
            box-shadow: 0 2px 8px rgba(0,0,0,0.04);
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header -->
    <div class="page-header-card d-flex flex-column flex-md-row justify-content-between align-items-md-center gap-3">
        <div>
            <div class="d-flex align-items-center gap-2 mb-2">
                <span class="badge bg-primary-subtle text-primary border border-primary-subtle font-monospace px-2 py-1">
                    <asp:Literal ID="litCourseCode" runat="server" />
                </span>
                <span class="badge bg-light text-secondary border px-2 py-1">
                    <asp:Literal ID="litCategory" runat="server" />
                </span>
                <asp:Literal ID="litStatusBadge" runat="server" />
            </div>
            <h2 class="h3 fw-bold mb-1 text-slate-900">
                <asp:Literal ID="litCourseName" runat="server" />
            </h2>
            <p class="text-muted small mb-0">
                <i class="bi bi-person-circle me-1"></i> Instructor: <strong><asp:Literal ID="litInstructor" runat="server" /></strong> &bull; Created: <asp:Literal ID="litCreatedAt" runat="server" />
            </p>
        </div>
        <div class="d-flex gap-2 flex-wrap">
            <asp:PlaceHolder ID="phAdminDetailsActions" runat="server">
                <asp:HyperLink ID="lnkStudyPreview" runat="server" CssClass="btn btn-outline-primary px-3">
                    <i class="bi bi-journal-check me-1"></i> Study / Quiz Preview
                </asp:HyperLink>
                <asp:HyperLink ID="lnkEdit" runat="server" CssClass="btn btn-primary px-3 shadow-sm">
                    <i class="bi bi-pencil me-1"></i> Edit Course
                </asp:HyperLink>
                <asp:HyperLink ID="lnkDelete" runat="server" CssClass="btn btn-outline-danger px-3">
                    <i class="bi bi-trash me-1"></i> Delete
                </asp:HyperLink>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="phStudentDetailsActions" runat="server" Visible="false">
                <asp:Button ID="btnEnrollDetails" runat="server" Text="Enroll in this Course" 
                    CssClass="btn btn-success px-4 fw-semibold shadow-sm" OnClick="btnEnrollDetails_Click" />
                <asp:Label ID="lblEnrolledBadge" runat="server" Visible="false"
                    CssClass="badge bg-success-subtle text-success border border-success-subtle px-3 py-2 fs-6 d-inline-flex align-items-center">
                    <i class="bi bi-check-circle-fill me-1"></i> You are Enrolled
                </asp:Label>
                <asp:HyperLink ID="lnkStudentStudy" runat="server" Visible="false" CssClass="btn btn-primary px-4 fw-semibold shadow-sm">
                    <i class="bi bi-journal-bookmark-fill me-1"></i> Study & Quiz Room
                </asp:HyperLink>
            </asp:PlaceHolder>
            <a href="Index.aspx" class="btn btn-outline-secondary px-3">
                <i class="bi bi-arrow-left me-1"></i> Back to Catalog
            </a>
        </div>
    </div>

    <!-- Error/Notification Message -->
    <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert alert-dismissible fade show shadow-sm" role="alert">
        <asp:Label ID="lblMessage" runat="server" />
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    </asp:Panel>

    <!-- Key Metrics Row -->
    <div class="row g-4 mb-4">
        <div class="col-12 col-sm-6 col-lg-3">
            <div class="card card-custom p-4 border-start border-primary border-4 h-100">
                <span class="text-muted small fw-semibold text-uppercase">Course Tuition Fee</span>
                <h3 class="fw-bold text-primary mt-2 mb-0">
                    <asp:Literal ID="litFee" runat="server" />
                </h3>
            </div>
        </div>

        <div class="col-12 col-sm-6 col-lg-3">
            <div class="card card-custom p-4 border-start border-info border-4 h-100">
                <span class="text-muted small fw-semibold text-uppercase">Duration</span>
                <h3 class="fw-bold text-dark mt-2 mb-0">
                    <asp:Literal ID="litDuration" runat="server" /> <span class="fs-6 fw-normal text-muted">weeks</span>
                </h3>
            </div>
        </div>

        <div class="col-12 col-sm-6 col-lg-3">
            <div class="card card-custom p-4 border-start border-success border-4 h-100">
                <span class="text-muted small fw-semibold text-uppercase">Course Start Date</span>
                <h3 class="fw-bold text-dark mt-2 mb-0 fs-4">
                    <asp:Literal ID="litStartDate" runat="server" />
                </h3>
            </div>
        </div>

        <div class="col-12 col-sm-6 col-lg-3">
            <div class="card card-custom p-4 border-start border-warning border-4 h-100">
                <span class="text-muted small fw-semibold text-uppercase">Course End Date</span>
                <h3 class="fw-bold text-dark mt-2 mb-0 fs-4">
                    <asp:Literal ID="litEndDate" runat="server" />
                </h3>
            </div>
        </div>
    </div>

    <!-- Course Syllabus & Description Card -->
    <div class="card card-custom p-4 p-md-5 mb-4">
        <h5 class="fw-bold text-slate-800 mb-3 border-bottom pb-2">
            <i class="bi bi-card-text text-primary me-2"></i>Course Description & Objectives
        </h5>
        <div class="lead fs-6 text-slate-700 lh-lg">
            <asp:Literal ID="litDescription" runat="server" />
        </div>
    </div>

    <!-- COURSE MATERIALS SECTION (FILE & TEXT) -->
    <asp:PlaceHolder ID="phMaterialsSection" runat="server">
        <div class="card card-custom p-4 p-md-5 mb-4 shadow-sm">
            <div class="d-flex justify-content-between align-items-center border-bottom pb-3 mb-4">
                <div>
                    <h5 class="fw-bold text-slate-800 mb-1">
                        <i class="bi bi-folder-fill text-primary me-2"></i>Course Learning Materials (File & Text)
                    </h5>
                    <p class="text-muted small mb-0">Text lessons, study notes, and downloadable files associated with this course.</p>
                </div>
            </div>

            <!-- Admin & Instructor Add Material Box -->
            <asp:PlaceHolder ID="phAddMaterialAdmin" runat="server" Visible="false">
                <div class="bg-light p-4 rounded-3 border mb-4">
                    <h6 class="fw-bold text-primary mb-3">
                        <i class="bi bi-plus-circle-fill me-1"></i> Add Course Material (File / Text)
                    </h6>
                    <div class="row g-3">
                        <div class="col-12 col-md-3">
                            <label class="form-label small fw-semibold text-muted">Material Type</label>
                            <asp:RadioButtonList ID="rblAddMaterialType" runat="server" RepeatDirection="Horizontal" CssClass="d-flex gap-3" AutoPostBack="true" OnSelectedIndexChanged="rblAddMaterialType_SelectedIndexChanged">
                                <asp:ListItem Text="<i class='bi bi-file-text me-1'></i> Text Lesson" Value="Text" Selected="True" />
                                <asp:ListItem Text="<i class='bi bi-file-earmark-arrow-up me-1'></i> Upload File" Value="File" />
                            </asp:RadioButtonList>
                        </div>
                        <div class="col-12 col-md-9">
                            <label class="form-label small fw-semibold text-muted">Material Title <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtAddMaterialTitle" runat="server" CssClass="form-control" Placeholder="e.g. Chapter 1 Notes or Lecture Slides" />
                        </div>
                        <div class="col-12" id="divTextContent" runat="server">
                            <label class="form-label small fw-semibold text-muted">Lesson / Text Content</label>
                            <asp:TextBox ID="txtAddMaterialText" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" Placeholder="Enter lecture notes, instructions, or reading content here..." />
                        </div>
                        <div class="col-12" id="divFileUpload" runat="server" style="display:none;">
                            <label class="form-label small fw-semibold text-muted">Upload Document / File (PDF, DOCX, ZIP, TXT, etc.)</label>
                            <asp:FileUpload ID="fuAddMaterialFile" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-12 text-end">
                            <asp:Button ID="btnAddMaterial" runat="server" Text="Save Course Material" CssClass="btn btn-primary px-4 fw-semibold" OnClick="btnAddMaterial_Click" />
                        </div>
                    </div>
                </div>
            </asp:PlaceHolder>

            <!-- Student Not Enrolled Notice -->
            <asp:PlaceHolder ID="phNotEnrolledNotice" runat="server" Visible="false">
                <div class="alert alert-warning d-flex align-items-center gap-2 mb-0">
                    <i class="bi bi-lock-fill fs-4 text-warning"></i>
                    <div>
                        <strong>Course materials and quiz are locked.</strong> You must enroll in this course to read text lessons, download files, and take the quiz.
                    </div>
                </div>
            </asp:PlaceHolder>

            <!-- Materials List -->
            <asp:Repeater ID="rptDetailsMaterials" runat="server" OnItemCommand="rptDetailsMaterials_ItemCommand">
                <ItemTemplate>
                    <div class="material-item d-flex flex-column flex-md-row justify-content-between align-items-md-center gap-3">
                        <div class="d-flex align-items-start gap-3">
                            <div class="p-2 rounded-3 mt-1 <%# Eval("MaterialType").ToString() == "File" ? "bg-success-subtle text-success" : "bg-primary-subtle text-primary" %>">
                                <i class="<%# Eval("MaterialType").ToString() == "File" ? "bi bi-file-earmark-arrow-down-fill fs-4" : "bi bi-journal-text fs-4" %>"></i>
                            </div>
                            <div>
                                <div class="d-flex align-items-center gap-2">
                                    <h6 class="fw-bold text-dark mb-0"><%# Eval("Title") %></h6>
                                    <span class="badge <%# Eval("MaterialType").ToString() == "File" ? "bg-success" : "bg-primary" %> small">
                                        <%# Eval("MaterialType") %>
                                    </span>
                                </div>
                                <div class="small text-muted mt-1">
                                    <%# Eval("MaterialType").ToString() == "File" ? "File: " + Eval("FileName") : (Eval("TextContent").ToString().Length > 100 ? Eval("TextContent").ToString().Substring(0, 100) + "..." : Eval("TextContent")) %>
                                </div>
                            </div>
                        </div>
                        <div class="d-flex align-items-center gap-2">
                            <%# Eval("MaterialType").ToString() == "File" ? 
                                "<a href='DownloadMaterial.aspx?id=" + Eval("MaterialId") + "' target='_blank' class='btn btn-sm btn-outline-primary'><i class='bi bi-box-arrow-up-right me-1'></i> Open File</a>" : 
                                "<a href='DownloadMaterial.aspx?id=" + Eval("MaterialId") + "' target='_blank' class='btn btn-sm btn-outline-secondary'><i class='bi bi-box-arrow-up-right me-1'></i> View Text</a>" 
                            %>
                            <asp:LinkButton ID="btnDeleteMaterial" runat="server" CommandName="DeleteMaterial" CommandArgument='<%# Eval("MaterialId") %>'
                                Visible='<%# UserRole == "Admin" || UserRole == "Instructor" %>' CssClass="btn btn-sm btn-outline-danger"
                                OnClientClick="return confirm('Are you sure you want to remove this course material?');">
                                <i class="bi bi-trash"></i>
                            </asp:LinkButton>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <asp:Label ID="lblNoMaterials" runat="server" Visible="false" CssClass="text-muted small fst-italic">
                No materials have been added to this course yet.
            </asp:Label>
        </div>
    </asp:PlaceHolder>

    <!-- COURSE QUIZ SECTION (ADMIN/INSTRUCTOR MANAGE + STUDENT ACCESS) -->
    <div class="card card-custom p-4 p-md-5 mb-4 shadow-sm">
        <div class="d-flex justify-content-between align-items-center border-bottom pb-3 mb-4">
            <div>
                <h5 class="fw-bold text-slate-800 mb-1">
                    <i class="bi bi-patch-question-fill text-warning me-2"></i>Course Assessment Quiz (2 Questions)
                </h5>
                <p class="text-muted small mb-0">Every student must complete this 2-question quiz to finish the course and obtain course completion.</p>
            </div>
            <span class="badge bg-warning-subtle text-warning-emphasis border border-warning-subtle px-3 py-2 fw-semibold">
                Only 2 Questions
            </span>
        </div>

        <!-- Student Quiz Status Banner -->
        <asp:PlaceHolder ID="phStudentQuizLink" runat="server" Visible="false">
            <div class="bg-light p-4 rounded-3 border d-flex flex-column flex-md-row justify-content-between align-items-md-center gap-3">
                <div>
                    <h6 class="fw-bold mb-1">
                        <asp:Literal ID="litStudentQuizStatus" runat="server" />
                    </h6>
                    <p class="text-muted small mb-0">Study the text and file materials, then take the 2-question quiz to register completion.</p>
                </div>
                <a href='<%# "CourseStudy.aspx?id=" + _courseId + "#quizSection" %>' class="btn btn-primary px-4 fw-semibold shadow-sm">
                    <i class="bi bi-arrow-right-circle me-1"></i> Launch Study & Quiz Room
                </a>
            </div>
        </asp:PlaceHolder>

        <!-- Admin / Instructor Manage Quiz (Two Questions) -->
        <asp:PlaceHolder ID="phQuizManagement" runat="server" Visible="false">
            <div class="accordion" id="accordionQuiz">
                <div class="accordion-item border rounded-3 mb-3 overflow-hidden">
                    <h2 class="accordion-header" id="headingQ1">
                        <button class="accordion-button fw-bold" type="button" data-bs-toggle="collapse" data-bs-target="#collapseQ1">
                            <span class="badge bg-primary me-2">Question 1 of 2</span> Configure First Question & Options
                        </button>
                    </h2>
                    <div id="collapseQ1" class="accordion-collapse collapse show">
                        <div class="accordion-body bg-light">
                            <div class="mb-3">
                                <label class="form-label small fw-semibold text-muted">Question 1 Text <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtQuizQ1" runat="server" CssClass="form-control" Placeholder="e.g. Which page lifecycle event in ASP.NET occurs first?" />
                            </div>
                            <div class="row g-2 mb-3">
                                <div class="col-12 col-md-6">
                                    <label class="form-label small fw-semibold text-muted">Option A</label>
                                    <asp:TextBox ID="txtQuizQ1A" runat="server" CssClass="form-control" />
                                </div>
                                <div class="col-12 col-md-6">
                                    <label class="form-label small fw-semibold text-muted">Option B</label>
                                    <asp:TextBox ID="txtQuizQ1B" runat="server" CssClass="form-control" />
                                </div>
                                <div class="col-12 col-md-6">
                                    <label class="form-label small fw-semibold text-muted">Option C</label>
                                    <asp:TextBox ID="txtQuizQ1C" runat="server" CssClass="form-control" />
                                </div>
                                <div class="col-12 col-md-6">
                                    <label class="form-label small fw-semibold text-muted">Option D</label>
                                    <asp:TextBox ID="txtQuizQ1D" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="mb-0">
                                <label class="form-label small fw-semibold text-muted">Correct Option for Question 1 <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlQuizQ1Ans" runat="server" CssClass="form-select w-auto">
                                    <asp:ListItem Text="Option A" Value="A" />
                                    <asp:ListItem Text="Option B" Value="B" Selected="True" />
                                    <asp:ListItem Text="Option C" Value="C" />
                                    <asp:ListItem Text="Option D" Value="D" />
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="accordion-item border rounded-3 mb-3 overflow-hidden">
                    <h2 class="accordion-header" id="headingQ2">
                        <button class="accordion-button fw-bold collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseQ2">
                            <span class="badge bg-primary me-2">Question 2 of 2</span> Configure Second Question & Options
                        </button>
                    </h2>
                    <div id="collapseQ2" class="accordion-collapse collapse">
                        <div class="accordion-body bg-light">
                            <div class="mb-3">
                                <label class="form-label small fw-semibold text-muted">Question 2 Text <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtQuizQ2" runat="server" CssClass="form-control" Placeholder="e.g. Why are parameterized SQL queries recommended?" />
                            </div>
                            <div class="row g-2 mb-3">
                                <div class="col-12 col-md-6">
                                    <label class="form-label small fw-semibold text-muted">Option A</label>
                                    <asp:TextBox ID="txtQuizQ2A" runat="server" CssClass="form-control" />
                                </div>
                                <div class="col-12 col-md-6">
                                    <label class="form-label small fw-semibold text-muted">Option B</label>
                                    <asp:TextBox ID="txtQuizQ2B" runat="server" CssClass="form-control" />
                                </div>
                                <div class="col-12 col-md-6">
                                    <label class="form-label small fw-semibold text-muted">Option C</label>
                                    <asp:TextBox ID="txtQuizQ2C" runat="server" CssClass="form-control" />
                                </div>
                                <div class="col-12 col-md-6">
                                    <label class="form-label small fw-semibold text-muted">Option D</label>
                                    <asp:TextBox ID="txtQuizQ2D" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="mb-0">
                                <label class="form-label small fw-semibold text-muted">Correct Option for Question 2 <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlQuizQ2Ans" runat="server" CssClass="form-select w-auto">
                                    <asp:ListItem Text="Option A" Value="A" />
                                    <asp:ListItem Text="Option B" Value="B" Selected="True" />
                                    <asp:ListItem Text="Option C" Value="C" />
                                    <asp:ListItem Text="Option D" Value="D" />
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="text-end mt-3">
                <asp:Button ID="btnSaveQuizSettings" runat="server" Text="Update 2-Question Quiz" CssClass="btn btn-success px-4 fw-bold shadow-sm" OnClick="btnSaveQuizSettings_Click" />
            </div>
        </asp:PlaceHolder>
    </div>

    <!-- Enrolled Students & Completion Status (Visible to Admin & Instructor) -->
    <asp:PlaceHolder ID="phAdminEnrolledStudents" runat="server" Visible="false">
        <div class="card card-custom overflow-hidden mb-5">
            <div class="card-header bg-white py-3 px-4 d-flex justify-content-between align-items-center">
                <h5 class="mb-0 fw-bold fs-6">
                    <i class="bi bi-mortarboard-fill text-primary me-2"></i>Students Enrolled & Course Completion Tracking
                    <span class="badge bg-primary-subtle text-primary ms-1">
                        <asp:Literal ID="litCourseEnrolledCount" runat="server" Text="0" />
                    </span>
                </h5>
                <span class="text-muted small">Course Roster & Completion</span>
            </div>

            <div class="table-responsive">
                <asp:GridView ID="gvCourseStudents" runat="server" AutoGenerateColumns="False"
                    CssClass="table table-hover table-custom mb-0 align-middle" GridLines="None">
                    <HeaderStyle CssClass="bg-light" />
                    <Columns>
                        <asp:TemplateField HeaderText="Student">
                            <ItemTemplate>
                                <div class="d-flex align-items-center gap-2">
                                    <div class="bg-primary-subtle text-primary rounded-circle d-flex align-items-center justify-content-center" style="width:32px; height:32px; font-weight:700; font-size: 0.85rem;">
                                        <%# Eval("FullName").ToString().Substring(0, 1).ToUpper() %>
                                    </div>
                                    <div>
                                        <div class="fw-bold text-dark"><%# Eval("FullName") %></div>
                                        <div class="small text-muted"><%# Eval("Email") %> (@<%# Eval("Username") %>)</div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Enrolled Date">
                            <ItemTemplate>
                                <span class="small text-muted"><%# string.Format("{0:MMM dd, yyyy HH:mm}", Eval("EnrollmentDate")) %></span>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Course & Quiz Completion" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%# Convert.ToBoolean(Eval("IsCompleted")) ? 
                                    "<span class='badge bg-success-subtle text-success border border-success-subtle px-2 py-1'><i class='bi bi-check-circle-fill me-1'></i>Completed</span>" +
                                    "<div class='mt-1'><span class='badge bg-primary px-2 py-1'>Quiz: " + Eval("QuizScore") + " / 2</span></div>" +
                                    "<div class='small text-muted mt-1'>" + (Eval("CompletionDate") != DBNull.Value ? string.Format("{0:MMM dd, yyyy}", Eval("CompletionDate")) : "") + "</div>" :
                                    "<span class='badge bg-warning-subtle text-warning-emphasis border border-warning-subtle px-2 py-1'><i class='bi bi-hourglass-split me-1'></i>In Progress</span>"
                                %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="p-4 text-center text-muted">
                            <i class="bi bi-person-x fs-2 d-block mb-1"></i>
                            No students have enrolled in this course yet.
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>
    </asp:PlaceHolder>
</asp:Content>
