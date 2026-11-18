<%@ Page Title="Student Enrollments" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Enrollments.aspx.cs" Inherits="CourseManagementSystem.EnrollmentsPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header -->
    <div class="page-header-card d-flex flex-column flex-md-row justify-content-between align-items-md-center gap-3">
        <div>
            <h2 class="h4 fw-bold mb-1 text-slate-800">
                <i class="bi bi-mortarboard-fill text-primary me-2"></i>Student Enrollments & Course Completions
            </h2>
            <p class="text-muted small mb-0">Monitor student registrations, view course completion progress, and inspect quiz results.</p>
        </div>
        <div class="d-flex gap-2">
            <a href="Index.aspx" class="btn btn-outline-secondary px-3">
                <i class="bi bi-arrow-left me-1"></i> Back to Courses
            </a>
        </div>
    </div>

    <!-- Message Alert -->
    <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert alert-dismissible fade show shadow-sm" role="alert">
        <asp:Label ID="lblMessage" runat="server" />
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    </asp:Panel>

    <!-- Summary KPI Cards -->
    <div class="row g-3 mb-4">
        <div class="col-12 col-sm-6 col-lg-3">
            <div class="card card-custom p-3 border-start border-primary border-4 h-100">
                <span class="text-muted small fw-semibold text-uppercase">Total Enrollments</span>
                <h3 class="fw-bold text-primary mt-1 mb-0">
                    <asp:Literal ID="litTotalCount" runat="server" Text="0" />
                </h3>
            </div>
        </div>
        <div class="col-12 col-sm-6 col-lg-3">
            <div class="card card-custom p-3 border-start border-success border-4 h-100">
                <span class="text-muted small fw-semibold text-uppercase">Completed Courses</span>
                <h3 class="fw-bold text-success mt-1 mb-0">
                    <asp:Literal ID="litCompletedCount" runat="server" Text="0" />
                </h3>
            </div>
        </div>
        <div class="col-12 col-sm-6 col-lg-3">
            <div class="card card-custom p-3 border-start border-warning border-4 h-100">
                <span class="text-muted small fw-semibold text-uppercase">In Progress</span>
                <h3 class="fw-bold text-warning-emphasis mt-1 mb-0">
                    <asp:Literal ID="litInProgressCount" runat="server" Text="0" />
                </h3>
            </div>
        </div>
        <div class="col-12 col-sm-6 col-lg-3">
            <div class="card card-custom p-3 border-start border-info border-4 h-100">
                <span class="text-muted small fw-semibold text-uppercase">Completion Rate</span>
                <h3 class="fw-bold text-info mt-1 mb-0">
                    <asp:Literal ID="litCompletionRate" runat="server" Text="0%" />
                </h3>
            </div>
        </div>
    </div>

    <!-- Filters Card -->
    <div class="card card-custom mb-4 p-3 p-md-4">
        <div class="row g-3 align-items-end">
            <!-- Search by Student -->
            <div class="col-12 col-md-4">
                <label for="<%= txtStudentSearch.ClientID %>" class="form-label fw-semibold small text-muted">Search Student</label>
                <div class="input-group">
                    <span class="input-group-text bg-light border-end-0 text-muted">
                        <i class="bi bi-search"></i>
                    </span>
                    <asp:TextBox ID="txtStudentSearch" runat="server" CssClass="form-control border-start-0"
                        Placeholder="Search student name, email, or username..." />
                </div>
            </div>

            <!-- Filter by Course -->
            <div class="col-12 col-md-3">
                <label for="<%= ddlCourseFilter.ClientID %>" class="form-label fw-semibold small text-muted">Filter by Course</label>
                <asp:DropDownList ID="ddlCourseFilter" runat="server" CssClass="form-select">
                    <asp:ListItem Text="All Courses" Value="" />
                </asp:DropDownList>
            </div>

            <!-- Filter by Completion Status -->
            <div class="col-12 col-md-3">
                <label for="<%= ddlCompletionFilter.ClientID %>" class="form-label fw-semibold small text-muted">Completion Status</label>
                <asp:DropDownList ID="ddlCompletionFilter" runat="server" CssClass="form-select">
                    <asp:ListItem Text="All Statuses" Value="" />
                    <asp:ListItem Text="Completed Courses Only" Value="Completed" />
                    <asp:ListItem Text="In Progress Only" Value="InProgress" />
                </asp:DropDownList>
            </div>

            <!-- Buttons -->
            <div class="col-12 col-md-2 d-flex gap-2">
                <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-primary px-3 fw-semibold flex-grow-1" OnClick="btnFilter_Click" />
                <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-outline-secondary px-3" OnClick="btnReset_Click" />
            </div>
        </div>
    </div>

    <!-- Table Card -->
    <div class="card card-custom overflow-hidden mb-4">
        <div class="card-header bg-white py-3 px-4 d-flex justify-content-between align-items-center">
            <h5 class="mb-0 fw-bold fs-6">
                Enrollment & Completion Records <span class="badge bg-primary-subtle text-primary ms-1">
                    <asp:Literal ID="litCount" runat="server" Text="0" />
                </span>
            </h5>
            <span class="text-muted small">Live database records</span>
        </div>

        <div class="table-responsive">
            <asp:GridView ID="gvAllEnrollments" runat="server" AutoGenerateColumns="False"
                CssClass="table table-hover table-custom mb-0 align-middle" GridLines="None" DataKeyNames="EnrollmentId"
                AllowPaging="True" PageSize="10" OnPageIndexChanging="gvAllEnrollments_PageIndexChanging">

                <HeaderStyle CssClass="bg-light" />
                <PagerStyle CssClass="p-3 bg-white text-center" HorizontalAlign="Center" />

                <Columns>
                    <asp:TemplateField HeaderText="Student Details">
                        <ItemTemplate>
                            <div class="d-flex align-items-center gap-2">
                                <div class="bg-primary-subtle text-primary rounded-circle d-flex align-items-center justify-content-center" style="width:36px; height:36px; font-weight:700;">
                                    <%# Eval("FullName").ToString().Substring(0, 1).ToUpper() %>
                                </div>
                                <div>
                                    <div class="fw-bold text-dark"><%# Eval("FullName") %></div>
                                    <div class="small text-muted">
                                        <i class="bi bi-envelope me-1"></i><%# Eval("Email") %> &bull; <span class="badge bg-light text-secondary border">@<%# Eval("Username") %></span>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Enrolled Course">
                        <ItemTemplate>
                            <div>
                                <span class="fw-bold text-dark"><%# Eval("CourseName") %></span>
                                <div class="mt-1 d-flex gap-2">
                                    <span class="badge bg-primary-subtle text-primary border border-primary-subtle font-monospace"><%# Eval("CourseCode") %></span>
                                    <span class="badge bg-light text-secondary border"><%# Eval("Category") %></span>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Instructor">
                        <ItemTemplate>
                            <span class="small"><i class="bi bi-person me-1"></i><%# Eval("InstructorName") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Registration Date">
                        <ItemTemplate>
                            <span class="small text-muted">
                                <i class="bi bi-calendar3 me-1"></i>
                                <%# string.Format("{0:MMM dd, yyyy HH:mm}", Eval("EnrollmentDate")) %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Course & Quiz Completion" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Convert.ToBoolean(Eval("IsCompleted")) ? 
                                "<span class='badge bg-success-subtle text-success border border-success-subtle px-2 py-1'><i class='bi bi-check-circle-fill me-1'></i>Course Completed</span>" +
                                "<div class='mt-1'><span class='badge bg-primary px-2 py-1'>Quiz Score: " + Eval("QuizScore") + " / " + Eval("QuizTotal") + "</span></div>" +
                                "<div class='small text-muted mt-1'><i class='bi bi-clock-history me-1'></i>" + (Eval("CompletionDate") != DBNull.Value ? string.Format("{0:MMM dd, yyyy HH:mm}", Eval("CompletionDate")) : "") + "</div>" :
                                "<span class='badge bg-warning-subtle text-warning-emphasis border border-warning-subtle px-2 py-1'><i class='bi bi-hourglass-split me-1'></i>In Progress</span><div class='small text-muted mt-1'>Quiz pending</div>"
                            %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Actions" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <a href='<%# "Details.aspx?id=" + Eval("CourseId") %>' class="btn btn-sm btn-outline-secondary" title="View Course Details">
                                <i class="bi bi-eye me-1"></i> Course Details
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <EmptyDataTemplate>
                    <div class="p-5 text-center">
                        <i class="bi bi-people fs-1 text-muted mb-3 d-block"></i>
                        <h6 class="fw-bold text-slate-700">No Student Records Found</h6>
                        <p class="text-muted small mb-0">No records matched your search or filter selection.</p>
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
