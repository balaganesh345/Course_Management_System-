<%@ Page Title="Course Study & Quiz" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CourseStudy.aspx.cs" Inherits="CourseManagementSystem.CourseStudyPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .study-header {
            background: linear-gradient(135deg, #1e3a8a 0%, #2563eb 100%);
            color: #ffffff;
            border-radius: 12px;
            padding: 2rem;
            margin-bottom: 2rem;
        }
        .material-card {
            border: 1px solid #e2e8f0;
            border-radius: 12px;
            transition: transform 0.15s ease, box-shadow 0.15s ease;
            background: #ffffff;
        }
        .material-card:hover {
            box-shadow: 0 4px 12px rgba(0,0,0,0.06);
        }
        .quiz-card {
            border: 1px solid #cbd5e1;
            border-radius: 12px;
            background: #ffffff;
        }
        .quiz-option-label {
            display: block;
            padding: 0.75rem 1rem;
            border: 1px solid #e2e8f0;
            border-radius: 8px;
            margin-bottom: 0.5rem;
            cursor: pointer;
            transition: all 0.15s ease;
        }
        .quiz-option-label:hover {
            background-color: #f1f5f9;
            border-color: #cbd5e1;
        }
        .certificate-card {
            background: linear-gradient(135deg, #f0fdf4 0%, #dcfce7 100%);
            border: 2px solid #86efac;
            border-radius: 16px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header Banner -->
    <div class="study-header shadow-sm d-flex flex-column flex-md-row justify-content-between align-items-md-center gap-3">
        <div>
            <div class="d-flex align-items-center gap-2 mb-2">
                <span class="badge bg-white text-primary font-monospace fw-bold px-2 py-1">
                    <asp:Literal ID="litCourseCode" runat="server" />
                </span>
                <span class="badge bg-white bg-opacity-25 text-white border border-white border-opacity-25 px-2 py-1">
                    <asp:Literal ID="litCategory" runat="server" />
                </span>
            </div>
            <h1 class="h3 fw-bold mb-1 text-white">
                <asp:Literal ID="litCourseName" runat="server" />
            </h1>
            <p class="text-white-50 small mb-0">
                <i class="bi bi-person-circle me-1"></i> Instructor: <strong><asp:Literal ID="litInstructor" runat="server" /></strong> &bull;
                <i class="bi bi-clock me-1"></i> <asp:Literal ID="litDuration" runat="server" /> weeks duration
            </p>
        </div>
        <div class="d-flex gap-2">
            <a href="Details.aspx?id=<%= Request.QueryString["id"] %>" class="btn btn-outline-light px-3">
                <i class="bi bi-info-circle me-1"></i> Course Info
            </a>
            <a href="MyEnrollments.aspx" class="btn btn-light fw-semibold text-primary px-3 shadow-sm">
                <i class="bi bi-mortarboard me-1"></i> My Courses
            </a>
        </div>
    </div>

    <!-- Alert / Message Box -->
    <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert alert-dismissible fade show shadow-sm" role="alert">
        <asp:Label ID="lblMessage" runat="server" />
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    </asp:Panel>

    <!-- NOT ENROLLED RESTRICTION PANEL -->
    <asp:Panel ID="pnlRestricted" runat="server" Visible="false">
        <div class="card card-custom p-5 text-center my-4">
            <div class="d-inline-flex align-items-center justify-content-center bg-warning bg-opacity-10 text-warning rounded-circle mx-auto mb-3" style="width:72px; height:72px;">
                <i class="bi bi-lock-fill fs-1"></i>
            </div>
            <h4 class="fw-bold text-slate-800">Enrollment Required</h4>
            <p class="text-muted max-w-md mx-auto mb-4" style="max-width: 500px;">
                Access to learning materials (text lessons and files) and the course quiz is exclusively available to enrolled students. Please enroll to unlock full course access.
            </p>
            <div class="d-flex justify-content-center gap-2">
                <asp:Button ID="btnEnrollNow" runat="server" Text="Enroll in this Course Now" CssClass="btn btn-success px-4 py-2 fw-semibold shadow-sm" OnClick="btnEnrollNow_Click" />
                <a href="Index.aspx" class="btn btn-outline-secondary px-4 py-2">Back to Catalog</a>
            </div>
        </div>
    </asp:Panel>

    <!-- ENROLLED STUDY CONTENT & QUIZ PANEL -->
    <asp:Panel ID="pnlStudyContent" runat="server" Visible="true">
        <!-- COURSE COMPLETION STATUS CARD (IF COMPLETED) -->
        <asp:Panel ID="pnlCompletedBadge" runat="server" Visible="false" CssClass="certificate-card p-4 p-md-5 mb-4 shadow-sm text-center">
            <div class="d-inline-flex align-items-center justify-content-center bg-success text-white rounded-circle p-3 mb-3 shadow">
                <i class="bi bi-award-fill fs-2"></i>
            </div>
            <h3 class="fw-bold text-success mb-1">Course Successfully Completed!</h3>
            <p class="text-success-emphasis mb-3">
                You have passed the 2-question course quiz and completed this academic curriculum.
            </p>
            <div class="d-flex justify-content-center align-items-center gap-3 flex-wrap">
                <span class="badge bg-success fs-6 px-3 py-2">
                    <i class="bi bi-check-circle-fill me-1"></i> Score: <asp:Literal ID="litCompletedScore" runat="server" /> / 2
                </span>
                <span class="badge bg-white text-dark border fs-6 px-3 py-2">
                    <i class="bi bi-calendar-check me-1 text-success"></i> Completed On: <asp:Literal ID="litCompletedDate" runat="server" />
                </span>
            </div>
        </asp:Panel>

        <!-- SECTION 1: COURSE MATERIALS (TEXT & FILES) -->
        <div class="card card-custom mb-5 shadow-sm overflow-hidden">
            <div class="card-header bg-white py-3 px-4 d-flex justify-content-between align-items-center border-bottom">
                <div>
                    <h5 class="mb-0 fw-bold fs-6 text-slate-800">
                        <i class="bi bi-folder2-open text-primary me-2"></i>Course Learning Materials & Resources
                    </h5>
                    <span class="text-muted small">Read lesson texts and download supplementary files prepared by your instructor</span>
                </div>
                <span class="badge bg-primary-subtle text-primary border border-primary-subtle px-3 py-2">
                    <asp:Literal ID="litMaterialsCount" runat="server" Text="0 items" />
                </span>
            </div>

            <div class="p-4">
                <!-- Text Lessons Repeater -->
                <h6 class="fw-bold text-slate-700 mb-3">
                    <i class="bi bi-file-text-fill text-info me-1"></i> Text Lessons & Study Notes
                </h6>
                <asp:Repeater ID="rptTextMaterials" runat="server">
                    <ItemTemplate>
                        <div class="card material-card p-4 mb-3">
                            <div class="d-flex justify-content-between align-items-start mb-2">
                                <h6 class="fw-bold text-dark mb-0">
                                    <i class="bi bi-journal-text text-primary me-2"></i><%# Eval("Title") %>
                                </h6>
                                <div class="d-flex align-items-center gap-2">
                                    <span class="badge bg-light text-secondary border small">
                                        <%# string.Format("{0:MMM dd, yyyy}", Eval("CreatedAt")) %>
                                    </span>
                                    <a href='<%# "DownloadMaterial.aspx?id=" + Eval("MaterialId") %>' target="_blank" class="btn btn-sm btn-outline-secondary py-0 px-2 small" title="Open text document in new tab">
                                        <i class="bi bi-box-arrow-up-right me-1"></i> Open in Tab
                                    </a>
                                </div>
                            </div>
                            <div class="text-slate-700 small lh-lg" style="white-space: pre-line;">
                                <%# Eval("TextContent") %>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Label ID="lblNoTextMaterials" runat="server" Visible="false" CssClass="text-muted small d-block mb-4 fst-italic">
                    No text lesson modules posted yet for this course.
                </asp:Label>

                <!-- Course Files Section (Open in Browser) -->
                <h6 class="fw-bold text-slate-700 mt-4 mb-3">
                    <i class="bi bi-file-earmark-text text-primary me-1"></i> Course Files & Reference Documents (Opens in Browser)
                </h6>
                <div class="row g-3">
                    <asp:Repeater ID="rptFileMaterials" runat="server">
                        <ItemTemplate>
                            <div class="col-12 col-md-6">
                                <div class="card material-card p-3 h-100 d-flex flex-row justify-content-between align-items-center">
                                    <div class="d-flex align-items-center gap-3 overflow-hidden">
                                        <div class="bg-primary-subtle text-primary p-2 rounded-3 flex-shrink-0">
                                            <i class="bi bi-file-earmark-text-fill fs-3"></i>
                                        </div>
                                        <div class="overflow-hidden">
                                            <div class="fw-bold text-dark text-truncate" title='<%# Eval("Title") %>'><%# Eval("Title") %></div>
                                            <div class="small text-muted text-truncate"><%# Eval("FileName") %></div>
                                        </div>
                                    </div>
                                    <a href='<%# "DownloadMaterial.aspx?id=" + Eval("MaterialId") %>' target="_blank" class="btn btn-sm btn-outline-primary flex-shrink-0 ms-2">
                                        <i class="bi bi-box-arrow-up-right me-1"></i> Open File
                                    </a>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <asp:Label ID="lblNoFileMaterials" runat="server" Visible="false" CssClass="text-muted small d-block mt-2 fst-italic">
                    No files uploaded yet for this course.
                </asp:Label>
            </div>
        </div>

        <!-- SECTION 2: TWO-QUESTION COURSE QUIZ -->
        <div class="card card-custom mb-5 shadow-sm overflow-hidden" id="quizSection">
            <div class="card-header bg-white py-3 px-4 d-flex justify-content-between align-items-center border-bottom">
                <div>
                    <h5 class="mb-0 fw-bold fs-6 text-slate-800">
                        <i class="bi bi-patch-question-fill text-warning me-2"></i>Course Assessment Quiz (2 Questions)
                    </h5>
                    <span class="text-muted small">Complete both questions correctly to finish this course and earn your completion status</span>
                </div>
                <span class="badge bg-warning-subtle text-warning-emphasis border border-warning-subtle px-3 py-2 fw-semibold">
                    <i class="bi bi-check2-circle me-1"></i> 2 Questions
                </span>
            </div>

            <div class="p-4 p-md-5">
                <!-- Question 1 -->
                <div class="quiz-card p-4 mb-4 border">
                    <div class="d-flex align-items-center gap-2 mb-3">
                        <span class="badge bg-primary rounded-pill px-3 py-2 fw-bold">Question 1 of 2</span>
                    </div>
                    <h6 class="fw-bold text-dark mb-3">
                        <asp:Literal ID="litQ1Text" runat="server" />
                    </h6>
                    <asp:RadioButtonList ID="rblQ1" runat="server" CssClass="w-100" RepeatLayout="UnorderedList" />
                    <asp:RequiredFieldValidator ID="rfvQ1" runat="server" ControlToValidate="rblQ1"
                        ErrorMessage="Please answer Question 1." CssClass="text-danger small mt-1 d-block" Display="Dynamic" ValidationGroup="QuizGroup" />
                </div>

                <!-- Question 2 -->
                <div class="quiz-card p-4 mb-4 border">
                    <div class="d-flex align-items-center gap-2 mb-3">
                        <span class="badge bg-primary rounded-pill px-3 py-2 fw-bold">Question 2 of 2</span>
                    </div>
                    <h6 class="fw-bold text-dark mb-3">
                        <asp:Literal ID="litQ2Text" runat="server" />
                    </h6>
                    <asp:RadioButtonList ID="rblQ2" runat="server" CssClass="w-100" RepeatLayout="UnorderedList" />
                    <asp:RequiredFieldValidator ID="rfvQ2" runat="server" ControlToValidate="rblQ2"
                        ErrorMessage="Please answer Question 2." CssClass="text-danger small mt-1 d-block" Display="Dynamic" ValidationGroup="QuizGroup" />
                </div>

                <!-- Submit Quiz Button -->
                <div class="d-flex justify-content-between align-items-center pt-3 border-top">
                    <span class="text-muted small">
                        <i class="bi bi-info-circle me-1"></i> Your submission will update your course completion status for admin and instructor viewing.
                    </span>
                    <asp:Button ID="btnSubmitQuiz" runat="server" Text="Submit Quiz & Complete Course" 
                        CssClass="btn btn-primary px-4 py-2 fw-bold shadow-sm" ValidationGroup="QuizGroup" OnClick="btnSubmitQuiz_Click" />
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
