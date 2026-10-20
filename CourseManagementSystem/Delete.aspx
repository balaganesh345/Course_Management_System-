<%@ Page Title="Delete Course" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Delete.aspx.cs" Inherits="CourseManagementSystem.DeletePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header -->
    <div class="page-header-card d-flex justify-content-between align-items-center">
        <div>
            <h2 class="h4 fw-bold mb-1 text-danger">
                <i class="bi bi-exclamation-triangle-fill me-2"></i>Delete Course Confirmation
            </h2>
            <p class="text-muted small mb-0">Are you sure you want to permanently delete this course from the system?</p>
        </div>
        <div>
            <a href="Index.aspx" class="btn btn-outline-secondary px-3">
                <i class="bi bi-arrow-left me-1"></i> Back to Catalog
            </a>
        </div>
    </div>

    <!-- Error/Notification Message -->
    <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert alert-danger" role="alert">
        <asp:Label ID="lblMessage" runat="server" />
    </asp:Panel>

    <!-- Details Card -->
    <div class="card card-custom p-4 p-md-5 mb-5 border-danger">
        <asp:HiddenField ID="hfCourseId" runat="server" />

        <div class="alert alert-warning d-flex align-items-center gap-2 mb-4" role="alert">
            <i class="bi bi-exclamation-octagon-fill fs-4 text-warning"></i>
            <div>
                <strong>Warning:</strong> Deleting this course cannot be undone. All associated data will be removed.
            </div>
        </div>

        <dl class="row mb-4">
            <dt class="col-sm-3 text-muted">Course Code:</dt>
            <dd class="col-sm-9 fw-bold"><asp:Literal ID="litCourseCode" runat="server" /></dd>

            <dt class="col-sm-3 text-muted">Course Name:</dt>
            <dd class="col-sm-9 fw-bold text-dark"><asp:Literal ID="litCourseName" runat="server" /></dd>

            <dt class="col-sm-3 text-muted">Category:</dt>
            <dd class="col-sm-9"><asp:Literal ID="litCategory" runat="server" /></dd>

            <dt class="col-sm-3 text-muted">Instructor:</dt>
            <dd class="col-sm-9"><asp:Literal ID="litInstructor" runat="server" /></dd>

            <dt class="col-sm-3 text-muted">Tuition Fee:</dt>
            <dd class="col-sm-9 text-success fw-bold"><asp:Literal ID="litFee" runat="server" /></dd>

            <dt class="col-sm-3 text-muted">Start - End Dates:</dt>
            <dd class="col-sm-9"><asp:Literal ID="litDates" runat="server" /></dd>
        </dl>

        <div class="d-flex gap-3 border-top pt-3">
            <asp:Button ID="btnConfirmDelete" runat="server" Text="Yes, Permanently Delete" CssClass="btn btn-danger px-4 py-2 fw-semibold" OnClick="btnConfirmDelete_Click" />
            <a href="Index.aspx" class="btn btn-outline-secondary px-4 py-2">Cancel</a>
        </div>
    </div>
</asp:Content>
