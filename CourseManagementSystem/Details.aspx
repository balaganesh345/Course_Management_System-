<%@ Page Title="Course Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="CourseManagementSystem.DetailsPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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
        <div class="d-flex gap-2">
            <asp:PlaceHolder ID="phAdminDetailsActions" runat="server">
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
            </asp:PlaceHolder>
            <a href="Index.aspx" class="btn btn-outline-secondary px-3">
                <i class="bi bi-arrow-left me-1"></i> Back to Catalog
            </a>
        </div>
    </div>

    <!-- Error/Notification Message -->
    <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert alert-danger" role="alert">
        <asp:Label ID="lblMessage" runat="server" />
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

    <!-- Enrolled Students (Visible to Admin only) -->
    <asp:PlaceHolder ID="phAdminEnrolledStudents" runat="server" Visible="false">
        <div class="card card-custom overflow-hidden mb-5">
            <div class="card-header bg-white py-3 px-4 d-flex justify-content-between align-items-center">
                <h5 class="mb-0 fw-bold fs-6">
                    <i class="bi bi-people-fill text-primary me-2"></i>Students Enrolled in this Course
                    <span class="badge bg-primary-subtle text-primary ms-1">
                        <asp:Literal ID="litCourseEnrolledCount" runat="server" Text="0" />
                    </span>
                </h5>
                <span class="text-muted small">Course Roster</span>
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
                        <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <span class="badge bg-success-subtle text-success border border-success-subtle px-2 py-1">
                                    <i class="bi bi-check-circle-fill me-1"></i><%# Eval("Status") %>
                                </span>
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
