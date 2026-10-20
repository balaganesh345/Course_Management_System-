<%@ Page Title="Courses Catalog" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Index.aspx.cs" Inherits="CourseManagementSystem.IndexPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header -->
    <div class="page-header-card d-flex flex-column flex-md-row justify-content-between align-items-md-center gap-3">
        <div>
            <h2 class="h4 fw-bold mb-1 text-slate-800">
                <i class="bi bi-journal-bookmark-fill text-primary me-2"></i>Course Catalog
            </h2>
            <p class="text-muted small mb-0">
                <asp:Literal ID="litSubHeader" runat="server" Text="Search, filter, and browse all academic courses." />
            </p>
        </div>
        <div class="d-flex gap-2">
            <asp:PlaceHolder ID="phAdminActions" runat="server" Visible="false">
                <a href="Create.aspx" class="btn btn-primary fw-semibold px-4 shadow-sm">
                    <i class="bi bi-plus-circle me-1"></i> Add New Course
                </a>
                <a href="Enrollments.aspx" class="btn btn-outline-secondary px-3">
                    <i class="bi bi-people me-1"></i> View Enrollments
                </a>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="phStudentActions" runat="server" Visible="false">
                <a href="MyEnrollments.aspx" class="btn btn-success fw-semibold px-4 shadow-sm">
                    <i class="bi bi-mortarboard me-1"></i> My Enrolled Courses
                </a>
            </asp:PlaceHolder>
        </div>
    </div>

    <!-- Feedback Message Banner -->
    <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert alert-dismissible fade show shadow-sm" role="alert">
        <asp:Label ID="lblMessage" runat="server" />
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    </asp:Panel>

    <!-- Filters Card -->
    <div class="card card-custom mb-4 p-3 p-md-4">
        <div class="row g-3 align-items-end">
            <!-- Search Keyword -->
            <div class="col-12 col-md-4">
                <label for="<%= txtSearch.ClientID %>" class="form-label fw-semibold small text-muted">Search Keyword</label>
                <div class="input-group">
                    <span class="input-group-text bg-light border-end-0 text-muted">
                        <i class="bi bi-search"></i>
                    </span>
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control border-start-0"
                        Placeholder="Search by name, code, instructor..." />
                </div>
            </div>

            <!-- Category Filter -->
            <div class="col-6 col-md-3">
                <label for="<%= ddlCategory.ClientID %>" class="form-label fw-semibold small text-muted">Category</label>
                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select">
                    <asp:ListItem Text="All Categories" Value="" />
                </asp:DropDownList>
            </div>

            <!-- Status Filter (For Admin) -->
            <div class="col-6 col-md-2" id="divStatusFilter" runat="server">
                <label for="<%= ddlStatus.ClientID %>" class="form-label fw-semibold small text-muted">Status</label>
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select">
                    <asp:ListItem Text="All Status" Value="" />
                    <asp:ListItem Text="Active Only" Value="1" />
                    <asp:ListItem Text="Inactive Only" Value="0" />
                </asp:DropDownList>
            </div>

            <!-- Sort By -->
            <div class="col-12 col-md-3">
                <label for="<%= ddlSort.ClientID %>" class="form-label fw-semibold small text-muted">Sort By</label>
                <asp:DropDownList ID="ddlSort" runat="server" CssClass="form-select">
                    <asp:ListItem Text="Course Name (A-Z)" Value="NameAsc" />
                    <asp:ListItem Text="Course Name (Z-A)" Value="NameDesc" />
                    <asp:ListItem Text="Fee: Low to High" Value="FeeAsc" />
                    <asp:ListItem Text="Fee: High to Low" Value="FeeDesc" />
                    <asp:ListItem Text="Start Date (Earliest)" Value="DateAsc" />
                    <asp:ListItem Text="Start Date (Latest)" Value="DateDesc" />
                </asp:DropDownList>
            </div>

            <!-- Action Buttons -->
            <div class="col-12 d-flex justify-content-end gap-2 pt-2 border-top">
                <asp:Button ID="btnReset" runat="server" Text="Reset Filters"
                    CssClass="btn btn-outline-secondary px-3" OnClick="btnReset_Click" />
                <asp:Button ID="btnFilter" runat="server" Text="Apply Filter"
                    CssClass="btn btn-primary px-4 fw-semibold" OnClick="btnFilter_Click" />
            </div>
        </div>
    </div>

    <!-- Courses Grid Card -->
    <div class="card card-custom overflow-hidden mb-4">
        <div class="card-header bg-white py-3 px-4 d-flex justify-content-between align-items-center">
            <h5 class="mb-0 fw-bold fs-6">
                Courses Listing <span class="badge bg-secondary-subtle text-secondary ms-1">
                    <asp:Literal ID="litCount" runat="server" Text="0" />
                </span>
            </h5>
            <span class="text-muted small">Showing up to 6 records per page</span>
        </div>

        <div class="table-responsive">
            <asp:GridView ID="gvCourses" runat="server" AutoGenerateColumns="False"
                CssClass="table table-hover table-custom mb-0 align-middle" GridLines="None" DataKeyNames="CourseId"
                AllowPaging="True" PageSize="6" OnPageIndexChanging="gvCourses_PageIndexChanging"
                OnRowCommand="gvCourses_RowCommand" OnRowDataBound="gvCourses_RowDataBound">

                <HeaderStyle CssClass="bg-light" />
                <PagerStyle CssClass="p-3 bg-white text-center" HorizontalAlign="Center" />

                <Columns>
                    <asp:TemplateField HeaderText="Course & Category">
                        <ItemTemplate>
                            <div class="d-flex flex-column">
                                <span class="fw-bold text-dark"><%# Eval("CourseName") %></span>
                                <div class="mt-1 d-flex align-items-center gap-2">
                                    <span class="badge bg-primary-subtle text-primary border border-primary-subtle font-monospace">
                                        <%# Eval("CourseCode") %>
                                    </span>
                                    <span class="badge bg-light text-secondary border">
                                        <%# Eval("Category") %>
                                    </span>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Instructor">
                        <ItemTemplate>
                            <div class="d-flex align-items-center gap-2">
                                <i class="bi bi-person-circle text-muted fs-5"></i>
                                <span class="fw-medium"><%# Eval("InstructorName") %></span>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Fee & Duration">
                        <ItemTemplate>
                            <div>
                                <span class="fw-bold text-success"><%# string.Format("{0:C2}", Eval("CourseFee")) %></span>
                                <div class="text-muted small"><i class="bi bi-clock me-1"></i><%# Eval("DurationInWeeks") %> weeks</div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Schedule">
                        <ItemTemplate>
                            <div class="small">
                                <div><span class="text-muted">Start:</span> <%# string.Format("{0:MMM dd, yyyy}", Eval("StartDate")) %></div>
                                <div><span class="text-muted">End:</span> <%# string.Format("{0:MMM dd, yyyy}", Eval("EndDate")) %></div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Convert.ToBoolean(Eval("IsActive"))
                                ? "<span class='badge-status-active'><i class='bi bi-check-circle-fill me-1'></i>Active</span>"
                                : "<span class='badge-status-inactive'><i class='bi bi-x-circle-fill me-1'></i>Inactive</span>"
                            %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Actions" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <div class="btn-group btn-group-sm" role="group">
                                <!-- Details Link (Available for all) -->
                                <a href='<%# "Details.aspx?id=" + Eval("CourseId") %>' class="btn btn-outline-secondary" title="View Details">
                                    <i class="bi bi-eye"></i> Details
                                </a>

                                <!-- Admin Action Buttons -->
                                <asp:PlaceHolder ID="phAdminGridActions" runat="server">
                                    <a href='<%# "Edit.aspx?id=" + Eval("CourseId") %>' class="btn btn-outline-primary" title="Edit Course">
                                        <i class="bi bi-pencil"></i>
                                    </a>
                                    <a href='<%# "Delete.aspx?id=" + Eval("CourseId") %>' class="btn btn-outline-danger" title="Delete Course">
                                        <i class="bi bi-trash"></i>
                                    </a>
                                </asp:PlaceHolder>

                                <!-- Student Enrollment Actions -->
                                <asp:PlaceHolder ID="phStudentGridActions" runat="server">
                                    <asp:PlaceHolder ID="phEnrolledBadge" runat="server" Visible='<%# Convert.ToInt32(Eval("IsEnrolled")) == 1 %>'>
                                        <span class="badge bg-success-subtle text-success border border-success-subtle px-2 py-1 ms-1 d-inline-flex align-items-center">
                                            <i class="bi bi-check-circle-fill me-1"></i> Enrolled
                                        </span>
                                    </asp:PlaceHolder>
                                    <asp:PlaceHolder ID="phEnrollButton" runat="server" Visible='<%# Convert.ToInt32(Eval("IsEnrolled")) == 0 && Convert.ToBoolean(Eval("IsActive")) %>'>
                                        <asp:LinkButton ID="btnEnroll" runat="server" CommandName="EnrollCourse" CommandArgument='<%# Eval("CourseId") %>'
                                            CssClass="btn btn-success fw-semibold ms-1">
                                            <i class="bi bi-plus-circle me-1"></i> Enroll
                                        </asp:LinkButton>
                                    </asp:PlaceHolder>
                                    <asp:PlaceHolder ID="phClosedBadge" runat="server" Visible='<%# Convert.ToInt32(Eval("IsEnrolled")) == 0 && !Convert.ToBoolean(Eval("IsActive")) %>'>
                                        <span class="badge bg-secondary-subtle text-secondary px-2 py-1 ms-1">Closed</span>
                                    </asp:PlaceHolder>
                                </asp:PlaceHolder>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <EmptyDataTemplate>
                    <div class="p-5 text-center">
                        <i class="bi bi-inbox fs-1 text-muted mb-3 d-block"></i>
                        <h6 class="fw-bold text-slate-700">No Courses Found</h6>
                        <p class="text-muted small mb-3">No courses matched your search or filter settings.</p>
                        <asp:PlaceHolder ID="phEmptyCreate" runat="server">
                            <a href="Create.aspx" class="btn btn-sm btn-primary">
                                <i class="bi bi-plus-circle me-1"></i> Create First Course
                            </a>
                        </asp:PlaceHolder>
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>
</asp:Content>