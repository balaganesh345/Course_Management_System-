<%@ Page Title="Create Course" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Create.aspx.cs" Inherits="CourseManagementSystem.CreatePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header -->
    <div class="page-header-card d-flex justify-content-between align-items-center">
        <div>
            <h2 class="h4 fw-bold mb-1 text-slate-800">
                <i class="bi bi-file-earmark-plus-fill text-primary me-2"></i>Create New Course
            </h2>
            <p class="text-muted small mb-0">Fill in the course parameters and schedule details below.</p>
        </div>
        <div>
            <a href="Index.aspx" class="btn btn-outline-secondary px-3">
                <i class="bi bi-arrow-left me-1"></i> Back to Catalog
            </a>
        </div>
    </div>

    <!-- Error/Notification Message -->
    <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert alert-danger alert-dismissible fade show" role="alert">
        <asp:Label ID="lblMessage" runat="server" />
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    </asp:Panel>

    <!-- Validation Summary -->
    <asp:ValidationSummary ID="valSummary" runat="server" 
        CssClass="alert alert-danger shadow-sm py-2 px-4" 
        HeaderText="<strong>Please correct the following errors:</strong>" />

    <!-- Form Container Card -->
    <div class="card card-custom p-4 p-md-5 mb-5">
        <div class="row g-4">
            <div class="col-12 border-bottom pb-2">
                <h5 class="fw-bold text-primary mb-0">
                    <i class="bi bi-info-circle me-1"></i> Course Identity
                </h5>
            </div>

            <!-- Course Name -->
            <div class="col-12 col-md-8">
                <label for="<%= txtCourseName.ClientID %>" class="form-label fw-semibold">
                    Course Name <span class="text-danger">*</span>
                </label>
                <asp:TextBox ID="txtCourseName" runat="server" CssClass="form-control" MaxLength="100" Placeholder="e.g. ASP.NET Web Forms & C# Foundations" />
                <asp:RequiredFieldValidator ID="rfvCourseName" runat="server" 
                    ControlToValidate="txtCourseName" 
                    ErrorMessage="Course Name is required." 
                    CssClass="text-danger small d-block mt-1" 
                    Display="Dynamic" />
            </div>

            <!-- Course Code -->
            <div class="col-12 col-md-4">
                <label for="<%= txtCourseCode.ClientID %>" class="form-label fw-semibold">
                    Course Code <span class="text-danger">*</span>
                </label>
                <asp:TextBox ID="txtCourseCode" runat="server" CssClass="form-control text-uppercase" MaxLength="20" Placeholder="e.g. CS-201" />
                <asp:RequiredFieldValidator ID="rfvCourseCode" runat="server" 
                    ControlToValidate="txtCourseCode" 
                    ErrorMessage="Course Code is required." 
                    CssClass="text-danger small d-block mt-1" 
                    Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revCourseCode" runat="server" 
                    ControlToValidate="txtCourseCode" 
                    ValidationExpression="^[A-Za-z0-9\-_]+$" 
                    ErrorMessage="Course Code can only contain letters, numbers, hyphens, and underscores." 
                    CssClass="text-danger small d-block mt-1" 
                    Display="Dynamic" />
            </div>

            <!-- Category -->
            <div class="col-12 col-md-6">
                <label for="<%= txtCategory.ClientID %>" class="form-label fw-semibold">
                    Category <span class="text-danger">*</span>
                </label>
                <asp:TextBox ID="txtCategory" runat="server" CssClass="form-control" MaxLength="50" Placeholder="e.g. Software Development, Web Development, Database" />
                <asp:RequiredFieldValidator ID="rfvCategory" runat="server" 
                    ControlToValidate="txtCategory" 
                    ErrorMessage="Category is required." 
                    CssClass="text-danger small d-block mt-1" 
                    Display="Dynamic" />
            </div>

            <!-- Instructor Name -->
            <div class="col-12 col-md-6">
                <label for="<%= txtInstructor.ClientID %>" class="form-label fw-semibold">
                    Instructor Name <span class="text-danger">*</span>
                </label>
                <asp:TextBox ID="txtInstructor" runat="server" CssClass="form-control" MaxLength="100" Placeholder="e.g. Dr. Robert Smith" />
                <asp:RequiredFieldValidator ID="rfvInstructor" runat="server" 
                    ControlToValidate="txtInstructor" 
                    ErrorMessage="Instructor Name is required." 
                    CssClass="text-danger small d-block mt-1" 
                    Display="Dynamic" />
            </div>

            <!-- Schedule & Logistics -->
            <div class="col-12 border-bottom pb-2 pt-3">
                <h5 class="fw-bold text-primary mb-0">
                    <i class="bi bi-calendar-event me-1"></i> Logistics & Pricing
                </h5>
            </div>

            <!-- Duration in Weeks -->
            <div class="col-12 col-md-3">
                <label for="<%= txtDuration.ClientID %>" class="form-label fw-semibold">
                    Duration (Weeks) <span class="text-danger">*</span>
                </label>
                <asp:TextBox ID="txtDuration" runat="server" CssClass="form-control" TextMode="Number" Text="8" min="1" max="520" />
                <asp:RequiredFieldValidator ID="rfvDuration" runat="server" 
                    ControlToValidate="txtDuration" 
                    ErrorMessage="Duration in weeks is required." 
                    CssClass="text-danger small d-block mt-1" 
                    Display="Dynamic" />
                <asp:RangeValidator ID="rvDuration" runat="server" 
                    ControlToValidate="txtDuration" 
                    Type="Integer" 
                    MinimumValue="1" 
                    MaximumValue="520" 
                    ErrorMessage="Duration must be between 1 and 520 weeks." 
                    CssClass="text-danger small d-block mt-1" 
                    Display="Dynamic" />
            </div>

            <!-- Course Fee -->
            <div class="col-12 col-md-3">
                <label for="<%= txtFee.ClientID %>" class="form-label fw-semibold">
                    Course Fee ($) <span class="text-danger">*</span>
                </label>
                <div class="input-group">
                    <span class="input-group-text">$</span>
                    <asp:TextBox ID="txtFee" runat="server" CssClass="form-control" Text="0.00" />
                </div>
                <asp:RequiredFieldValidator ID="rfvFee" runat="server" 
                    ControlToValidate="txtFee" 
                    ErrorMessage="Course Fee is required." 
                    CssClass="text-danger small d-block mt-1" 
                    Display="Dynamic" />
                <asp:RangeValidator ID="rvFee" runat="server" 
                    ControlToValidate="txtFee" 
                    Type="Double" 
                    MinimumValue="0.00" 
                    MaximumValue="1000000.00" 
                    ErrorMessage="Course Fee must be between $0.00 and $1,000,000.00." 
                    CssClass="text-danger small d-block mt-1" 
                    Display="Dynamic" />
            </div>

            <!-- Start Date -->
            <div class="col-12 col-md-3">
                <label for="<%= txtStartDate.ClientID %>" class="form-label fw-semibold">
                    Start Date <span class="text-danger">*</span>
                </label>
                <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" TextMode="Date" />
                <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" 
                    ControlToValidate="txtStartDate" 
                    ErrorMessage="Start Date is required." 
                    CssClass="text-danger small d-block mt-1" 
                    Display="Dynamic" />
            </div>

            <!-- End Date -->
            <div class="col-12 col-md-3">
                <label for="<%= txtEndDate.ClientID %>" class="form-label fw-semibold">
                    End Date <span class="text-danger">*</span>
                </label>
                <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" TextMode="Date" />
                <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" 
                    ControlToValidate="txtEndDate" 
                    ErrorMessage="End Date is required." 
                    CssClass="text-danger small d-block mt-1" 
                    Display="Dynamic" />
                <asp:CompareValidator ID="cvEndDate" runat="server" 
                    ControlToValidate="txtEndDate" 
                    ControlToCompare="txtStartDate" 
                    Operator="GreaterThan" 
                    Type="Date" 
                    ErrorMessage="End Date must be after the Start Date." 
                    CssClass="text-danger small d-block mt-1" 
                    Display="Dynamic" />
            </div>

            <!-- Description -->
            <div class="col-12">
                <label for="<%= txtDescription.ClientID %>" class="form-label fw-semibold">Course Description</label>
                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" MaxLength="500" Placeholder="Provide detailed course objectives..." />
                <span class="text-muted small">Maximum 500 characters.</span>
            </div>

            <!-- Active Status -->
            <div class="col-12">
                <div class="form-check form-switch fs-5">
                    <asp:CheckBox ID="chkIsActive" runat="server" Checked="true" CssClass="form-check-input" />
                    <label class="form-check-label fs-6 fw-semibold ms-2" for="<%= chkIsActive.ClientID %>">
                        Activate this course immediately for student enrollment
                    </label>
                </div>
            </div>

            <!-- Form Actions -->
            <div class="col-12 pt-3 border-top d-flex gap-3">
                <asp:Button ID="btnSave" runat="server" Text="Save & Publish Course" CssClass="btn btn-primary px-4 py-2 fw-semibold" OnClick="btnSave_Click" />
                <a href="Index.aspx" class="btn btn-outline-secondary px-4 py-2">Cancel</a>
            </div>
        </div>
    </div>
</asp:Content>
