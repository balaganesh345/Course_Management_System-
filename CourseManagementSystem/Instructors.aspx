<%@ Page Title="Instructors Directory" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Instructors.aspx.cs" Inherits="CourseManagementSystem.InstructorsPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .instructor-avatar {
            width: 48px;
            height: 48px;
            border-radius: 50%;
            background: linear-gradient(135deg, #3b82f6 0%, #1d4ed8 100%);
            color: #ffffff;
            display: flex;
            align-items: center;
            justify-content: center;
            font-weight: 700;
            font-size: 1.15rem;
            box-shadow: 0 2px 6px rgba(59, 130, 246, 0.3);
        }
        .course-pill {
            font-size: 0.8rem;
            padding: 0.25rem 0.65rem;
            border-radius: 20px;
            background-color: #f1f5f9;
            color: #334155;
            text-decoration: none;
            border: 1px solid #e2e8f0;
            display: inline-flex;
            align-items: center;
            gap: 0.35rem;
            transition: all 0.2s ease;
        }
        .course-pill:hover {
            background-color: #e2e8f0;
            color: #0f172a;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header -->
    <div class="page-header-card d-flex flex-column flex-md-row justify-content-between align-items-md-center gap-3">
        <div>
            <h2 class="h4 fw-bold mb-1 text-slate-800">
                <i class="bi bi-person-video3 text-primary me-2"></i>Instructors Directory & Overview
            </h2>
            <p class="text-muted small mb-0">
                View all registered faculty instructors, assigned courses, active students, and guided completions.
            </p>
        </div>
        <div class="d-flex gap-2">
            <a href="Index.aspx" class="btn btn-outline-secondary px-3">
                <i class="bi bi-arrow-left me-1"></i> Back to Courses
            </a>
            <a href="Enrollments.aspx" class="btn btn-outline-primary px-3">
                <i class="bi bi-award me-1"></i> View Enrollments
            </a>
        </div>
    </div>

    <!-- Message Alert -->
    <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert alert-dismissible fade show shadow-sm" role="alert">
        <asp:Label ID="lblMessage" runat="server" />
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    </asp:Panel>

    <!-- KPI Summary Metrics -->
    <div class="row g-3 mb-4">
        <div class="col-12 col-sm-6 col-lg-3">
            <div class="card card-custom p-3 border-start border-primary border-4 h-100">
                <span class="text-muted small fw-semibold text-uppercase">Total Instructors</span>
                <h3 class="fw-bold text-primary mt-1 mb-0">
                    <asp:Literal ID="litTotalInstructors" runat="server" Text="0" />
                </h3>
            </div>
        </div>
        <div class="col-12 col-sm-6 col-lg-3">
            <div class="card card-custom p-3 border-start border-info border-4 h-100">
                <span class="text-muted small fw-semibold text-uppercase">Assigned Courses</span>
                <h3 class="fw-bold text-info mt-1 mb-0">
                    <asp:Literal ID="litTotalAssignedCourses" runat="server" Text="0" />
                </h3>
            </div>
        </div>
        <div class="col-12 col-sm-6 col-lg-3">
            <div class="card card-custom p-3 border-start border-success border-4 h-100">
                <span class="text-muted small fw-semibold text-uppercase">Total Students Enrolled</span>
                <h3 class="fw-bold text-success mt-1 mb-0">
                    <asp:Literal ID="litTotalStudents" runat="server" Text="0" />
                </h3>
            </div>
        </div>
        <div class="col-12 col-sm-6 col-lg-3">
            <div class="card card-custom p-3 border-start border-warning border-4 h-100">
                <span class="text-muted small fw-semibold text-uppercase">Courses Completed</span>
                <h3 class="fw-bold text-warning-emphasis mt-1 mb-0">
                    <asp:Literal ID="litTotalCompletions" runat="server" Text="0" />
                </h3>
            </div>
        </div>
    </div>

    <!-- Filter Card -->
    <div class="card card-custom mb-4 p-3 p-md-4">
        <div class="row g-3 align-items-end">
            <div class="col-12 col-md-8">
                <label for="<%= txtSearch.ClientID %>" class="form-label fw-semibold small text-muted">Search Instructors</label>
                <div class="input-group">
                    <span class="input-group-text bg-light border-end-0 text-muted">
                        <i class="bi bi-search"></i>
                    </span>
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control border-start-0"
                        Placeholder="Search by instructor name, username, email, or course name..." />
                </div>
            </div>
            <div class="col-6 col-md-2">
                <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-primary w-100 fw-semibold"
                    OnClick="btnFilter_Click" />
            </div>
            <div class="col-6 col-md-2">
                <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-outline-secondary w-100"
                    OnClick="btnReset_Click" />
            </div>
        </div>
    </div>

    <!-- Instructors List Table -->
    <div class="card card-custom shadow-sm overflow-hidden mb-5">
        <div class="card-header bg-white py-3 px-4 d-flex justify-content-between align-items-center border-bottom">
            <div>
                <h5 class="mb-0 fw-bold fs-6 text-slate-800">
                    <i class="bi bi-people-fill text-primary me-2"></i>Present Faculty Instructors
                </h5>
                <span class="text-muted small">
                    Showing <asp:Literal ID="litCount" runat="server" Text="0" /> active instructors in the system
                </span>
            </div>
            <a href="Register.aspx" class="btn btn-sm btn-outline-primary fw-semibold">
                <i class="bi bi-person-plus me-1"></i> Register New Instructor
            </a>
        </div>

        <div class="table-responsive">
            <asp:GridView ID="gvInstructors" runat="server" AutoGenerateColumns="false"
                CssClass="table table-hover align-middle mb-0" GridLines="None"
                EmptyDataText="No instructors found matching your criteria."
                OnRowDataBound="gvInstructors_RowDataBound">
                <EmptyDataRowStyle CssClass="text-center text-muted p-5 fst-italic" />
                <HeaderStyle CssClass="table-light text-slate-700 text-uppercase small fw-semibold border-bottom" />
                <Columns>
                    <%-- Instructor Name & Avatar --%>
                    <asp:TemplateField HeaderText="Instructor Profile">
                        <ItemTemplate>
                            <div class="d-flex align-items-center gap-3 py-1">
                                <div class="instructor-avatar flex-shrink-0">
                                    <%# GetInitials(Eval("FullName").ToString()) %>
                                </div>
                                <div>
                                    <div class="fw-bold text-dark"><%# Eval("FullName") %></div>
                                    <div class="small text-muted">
                                        <i class="bi bi-at"></i><%# Eval("Username") %> &bull;
                                        <i class="bi bi-envelope me-1"></i><%# Eval("Email") %>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- Courses Taught --%>
                    <asp:TemplateField HeaderText="Assigned Courses Taught">
                        <ItemTemplate>
                            <asp:PlaceHolder ID="phCoursesTaught" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- Total Courses Count --%>
                    <asp:TemplateField HeaderText="Courses" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center">
                        <ItemTemplate>
                            <span class="badge bg-primary-subtle text-primary border border-primary-subtle px-2 py-1 fw-semibold fs-6">
                                <%# Eval("TotalCourses") %>
                            </span>
                            <div class="small text-muted mt-1"><%# Eval("ActiveCourses") %> Active</div>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- Students Enrolled --%>
                    <asp:TemplateField HeaderText="Enrolled Students" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center">
                        <ItemTemplate>
                            <span class="badge bg-success-subtle text-success border border-success-subtle px-2 py-1 fw-semibold fs-6">
                                <%# Eval("TotalStudents") %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- Course Completions --%>
                    <asp:TemplateField HeaderText="Completions" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center">
                        <ItemTemplate>
                            <span class="badge bg-warning-subtle text-warning-emphasis border border-warning-subtle px-2 py-1 fw-semibold fs-6">
                                <%# Eval("TotalCompletions") %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- Joined Date --%>
                    <asp:TemplateField HeaderText="Joined" ItemStyle-CssClass="text-nowrap small text-muted">
                        <ItemTemplate>
                            <%# string.Format("{0:MMM dd, yyyy}", Eval("CreatedAt")) %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- Actions --%>
                    <asp:TemplateField HeaderText="Actions" ItemStyle-CssClass="text-end" HeaderStyle-CssClass="text-end">
                        <ItemTemplate>
                            <a href='<%# "Index.aspx?search=" + Server.UrlEncode(Eval("FullName").ToString()) %>' class="btn btn-sm btn-outline-primary px-2" title="View courses by this instructor">
                                <i class="bi bi-collection me-1"></i> Courses
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
