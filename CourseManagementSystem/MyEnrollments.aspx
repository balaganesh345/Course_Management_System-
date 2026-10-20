<%@ Page Title="My Enrolled Courses" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyEnrollments.aspx.cs" Inherits="CourseManagementSystem.MyEnrollmentsPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header -->
    <div class="page-header-card d-flex flex-column flex-md-row justify-content-between align-items-md-center gap-3">
        <div>
            <h2 class="h4 fw-bold mb-1 text-slate-800">
                <i class="bi bi-mortarboard-fill text-success me-2"></i>My Enrolled Courses
            </h2>
            <p class="text-muted small mb-0">Track and manage the courses you are currently registered in.</p>
        </div>
        <div>
            <a href="Index.aspx" class="btn btn-outline-primary fw-semibold px-4">
                <i class="bi bi-collection me-1"></i> Browse More Courses
            </a>
        </div>
    </div>

    <!-- Message Alert -->
    <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert alert-dismissible fade show shadow-sm" role="alert">
        <asp:Label ID="lblMessage" runat="server" />
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    </asp:Panel>

    <!-- Grid Card -->
    <div class="card card-custom overflow-hidden mb-4">
        <div class="card-header bg-white py-3 px-4 d-flex justify-content-between align-items-center">
            <h5 class="mb-0 fw-bold fs-6">
                Enrolled Courses <span class="badge bg-success-subtle text-success ms-1">
                    <asp:Literal ID="litCount" runat="server" Text="0" />
                </span>
            </h5>
            <span class="text-muted small">Active student enrollment records</span>
        </div>

        <div class="table-responsive">
            <asp:GridView ID="gvEnrollments" runat="server" AutoGenerateColumns="False"
                CssClass="table table-hover table-custom mb-0 align-middle" GridLines="None" DataKeyNames="EnrollmentId"
                OnRowCommand="gvEnrollments_RowCommand">

                <HeaderStyle CssClass="bg-light" />

                <Columns>
                    <asp:TemplateField HeaderText="Course & Category">
                        <ItemTemplate>
                            <div class="d-flex flex-column">
                                <span class="fw-bold text-dark"><%# Eval("CourseName") %></span>
                                <div class="mt-1 d-flex align-items-center gap-2">
                                    <span class="badge bg-primary-subtle text-primary border border-primary-subtle font-monospace"><%# Eval("CourseCode") %></span>
                                    <span class="badge bg-light text-secondary border"><%# Eval("Category") %></span>
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

                    <asp:TemplateField HeaderText="Course Schedule">
                        <ItemTemplate>
                            <div class="small">
                                <div><span class="text-muted">Start:</span> <%# string.Format("{0:MMM dd, yyyy}", Eval("StartDate")) %></div>
                                <div><span class="text-muted">End:</span> <%# string.Format("{0:MMM dd, yyyy}", Eval("EndDate")) %></div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Enrolled On">
                        <ItemTemplate>
                            <span class="small text-muted">
                                <i class="bi bi-calendar-check me-1 text-success"></i>
                                <%# string.Format("{0:MMM dd, yyyy}", Eval("EnrollmentDate")) %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <span class="badge bg-success-subtle text-success border border-success-subtle px-2 py-1">
                                <i class="bi bi-check-circle-fill me-1"></i><%# Eval("Status") %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Actions" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <div class="btn-group btn-group-sm" role="group">
                                <a href='<%# "Details.aspx?id=" + Eval("CourseId") %>' class="btn btn-outline-secondary" title="View Course Details">
                                    <i class="bi bi-eye me-1"></i> Details
                                </a>
                                <asp:LinkButton ID="btnDrop" runat="server" CommandName="DropCourse" CommandArgument='<%# Eval("EnrollmentId") %>' 
                                    CssClass="btn btn-outline-danger" OnClientClick="return confirm('Are you sure you want to drop this course enrollment?');" title="Drop Course">
                                    <i class="bi bi-x-circle me-1"></i> Drop
                                </asp:LinkButton>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <EmptyDataTemplate>
                    <div class="p-5 text-center">
                        <i class="bi bi-journal-x fs-1 text-muted mb-3 d-block"></i>
                        <h6 class="fw-bold text-slate-700">No Enrolled Courses Found</h6>
                        <p class="text-muted small mb-3">You have not enrolled in any academic courses yet.</p>
                        <a href="Index.aspx" class="btn btn-sm btn-primary">
                            <i class="bi bi-search me-1"></i> Browse & Enroll in Courses
                        </a>
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
