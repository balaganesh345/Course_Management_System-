<%@ Page Title="Student Registration" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="CourseManagementSystem.RegisterPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .auth-card {
            max-width: 520px;
            margin: 2rem auto;
            border-radius: 16px;
            box-shadow: 0 10px 25px -5px rgba(0, 0, 0, 0.08), 0 8px 10px -6px rgba(0, 0, 0, 0.04);
            border: 1px solid #e2e8f0;
            overflow: hidden;
            background: #ffffff;
        }
        .auth-header {
            background: linear-gradient(135deg, #059669 0%, #10b981 100%);
            color: #ffffff;
            padding: 2.25rem 2rem 1.75rem 2rem;
            text-align: center;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="auth-card">
        <!-- Header -->
        <div class="auth-header">
            <div class="d-inline-flex align-items-center justify-content-center bg-white bg-opacity-25 rounded-circle p-3 mb-2">
                <i class="bi bi-person-plus-fill fs-2 text-white"></i>
            </div>
            <h3 class="fw-bold mb-1">Student Registration</h3>
            <p class="text-white-50 small mb-0">Create your student account to enroll and manage your courses</p>
        </div>

        <!-- Body -->
        <div class="p-4 p-md-5">
            <!-- Message Panel -->
            <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert alert-danger py-2 small" role="alert">
                <asp:Label ID="lblMessage" runat="server" />
            </asp:Panel>

            <asp:ValidationSummary ID="valSummary" runat="server" 
                CssClass="alert alert-danger py-2 small mb-3" 
                HeaderText="<strong>Please correct the following:</strong>" />

            <!-- Full Name -->
            <div class="mb-3">
                <label for="<%= txtFullName.ClientID %>" class="form-label fw-semibold small text-muted">Full Name <span class="text-danger">*</span></label>
                <div class="input-group">
                    <span class="input-group-text bg-light text-muted border-end-0">
                        <i class="bi bi-card-text"></i>
                    </span>
                    <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control border-start-0" Placeholder="e.g. Alex Johnson" MaxLength="100" />
                </div>
                <asp:RequiredFieldValidator ID="rfvFullName" runat="server" 
                    ControlToValidate="txtFullName" 
                    ErrorMessage="Full Name is required." 
                    CssClass="text-danger small mt-1 d-block" 
                    Display="Dynamic" />
            </div>

            <!-- Username -->
            <div class="mb-3">
                <label for="<%= txtUsername.ClientID %>" class="form-label fw-semibold small text-muted">Username <span class="text-danger">*</span></label>
                <div class="input-group">
                    <span class="input-group-text bg-light text-muted border-end-0">
                        <i class="bi bi-person"></i>
                    </span>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control border-start-0" Placeholder="Choose a unique username" MaxLength="50" />
                </div>
                <asp:RequiredFieldValidator ID="rfvUsername" runat="server" 
                    ControlToValidate="txtUsername" 
                    ErrorMessage="Username is required." 
                    CssClass="text-danger small mt-1 d-block" 
                    Display="Dynamic" />
            </div>

            <!-- Email -->
            <div class="mb-3">
                <label for="<%= txtEmail.ClientID %>" class="form-label fw-semibold small text-muted">Email Address <span class="text-danger">*</span></label>
                <div class="input-group">
                    <span class="input-group-text bg-light text-muted border-end-0">
                        <i class="bi bi-envelope"></i>
                    </span>
                    <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="form-control border-start-0" Placeholder="alex@example.com" MaxLength="100" />
                </div>
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                    ControlToValidate="txtEmail" 
                    ErrorMessage="Email address is required." 
                    CssClass="text-danger small mt-1 d-block" 
                    Display="Dynamic" />
            </div>

            <!-- Password -->
            <div class="mb-3">
                <label for="<%= txtPassword.ClientID %>" class="form-label fw-semibold small text-muted">Password <span class="text-danger">*</span></label>
                <div class="input-group">
                    <span class="input-group-text bg-light text-muted border-end-0">
                        <i class="bi bi-key"></i>
                    </span>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control border-start-0" Placeholder="Minimum 6 characters" />
                </div>
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                    ControlToValidate="txtPassword" 
                    ErrorMessage="Password is required." 
                    CssClass="text-danger small mt-1 d-block" 
                    Display="Dynamic" />
            </div>

            <!-- Confirm Password -->
            <div class="mb-4">
                <label for="<%= txtConfirmPassword.ClientID %>" class="form-label fw-semibold small text-muted">Confirm Password <span class="text-danger">*</span></label>
                <div class="input-group">
                    <span class="input-group-text bg-light text-muted border-end-0">
                        <i class="bi bi-check2-circle"></i>
                    </span>
                    <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control border-start-0" Placeholder="Re-enter password" />
                </div>
                <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" 
                    ControlToValidate="txtConfirmPassword" 
                    ErrorMessage="Confirm Password is required." 
                    CssClass="text-danger small mt-1 d-block" 
                    Display="Dynamic" />
                <asp:CompareValidator ID="cvPassword" runat="server" 
                    ControlToValidate="txtConfirmPassword" 
                    ControlToCompare="txtPassword" 
                    ErrorMessage="Passwords do not match." 
                    CssClass="text-danger small mt-1 d-block" 
                    Display="Dynamic" />
            </div>

            <!-- Register Button -->
            <div class="d-grid mb-3">
                <asp:Button ID="btnRegister" runat="server" Text="Complete Student Registration" CssClass="btn btn-success py-2 fw-semibold shadow-sm" OnClick="btnRegister_Click" />
            </div>

            <div class="text-center pt-2 border-top">
                <p class="text-muted small mb-0">
                    Already registered? 
                    <a href="Login.aspx" class="text-primary fw-semibold text-decoration-none">Sign in here</a>
                </p>
            </div>
        </div>
    </div>
</asp:Content>
