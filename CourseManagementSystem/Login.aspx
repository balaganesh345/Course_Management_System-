<%@ Page Title="Sign In" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CourseManagementSystem.LoginPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .auth-card {
            max-width: 460px;
            margin: 2rem auto;
            border-radius: 16px;
            box-shadow: 0 10px 25px -5px rgba(0, 0, 0, 0.08), 0 8px 10px -6px rgba(0, 0, 0, 0.04);
            border: 1px solid #e2e8f0;
            overflow: hidden;
            background: #ffffff;
        }
        .auth-header {
            background: linear-gradient(135deg, #1e3a8a 0%, #3b82f6 100%);
            color: #ffffff;
            padding: 2.25rem 2rem 1.75rem 2rem;
            text-align: center;
        }
        .demo-badge {
            cursor: pointer;
            transition: all 0.2s ease;
        }
        .demo-badge:hover {
            transform: translateY(-1px);
            filter: brightness(0.95);
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="auth-card">
        <!-- Card Header -->
        <div class="auth-header">
            <div class="d-inline-flex align-items-center justify-content-center bg-white bg-opacity-25 rounded-circle p-3 mb-2">
                <i class="bi bi-shield-lock-fill fs-2 text-white"></i>
            </div>
            <h3 class="fw-bold mb-1">Account Login</h3>
            <p class="text-white-50 small mb-0">Sign in as an Administrator or a Student</p>
        </div>

        <!-- Form Body -->
        <div class="p-4 p-md-5">
            <!-- Message Panel -->
            <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert alert-danger py-2 small" role="alert">
                <asp:Label ID="lblMessage" runat="server" />
            </asp:Panel>

            <!-- Role Selector -->
            <div class="mb-3">
                <label class="form-label fw-semibold small text-muted">Select Role <span class="text-danger">*</span></label>
                <div class="d-flex gap-2">
                    <asp:RadioButtonList ID="rblRole" runat="server" RepeatDirection="Horizontal" CssClass="d-flex gap-4">
                        <asp:ListItem Text="<i class='bi bi-person-gear me-1'></i> Admin" Value="Admin" Selected="True" />
                        <asp:ListItem Text="<i class='bi bi-backpack me-1'></i> Student" Value="Student" />
                    </asp:RadioButtonList>
                </div>
            </div>

            <!-- Username -->
            <div class="mb-3">
                <label for="<%= txtUsername.ClientID %>" class="form-label fw-semibold small text-muted">Username</label>
                <div class="input-group">
                    <span class="input-group-text bg-light text-muted border-end-0">
                        <i class="bi bi-person"></i>
                    </span>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control border-start-0" Placeholder="Enter your username" />
                </div>
                <asp:RequiredFieldValidator ID="rfvUsername" runat="server" 
                    ControlToValidate="txtUsername" 
                    ErrorMessage="Username is required." 
                    CssClass="text-danger small mt-1 d-block" 
                    Display="Dynamic" />
            </div>

            <!-- Password -->
            <div class="mb-4">
                <label for="<%= txtPassword.ClientID %>" class="form-label fw-semibold small text-muted">Password</label>
                <div class="input-group">
                    <span class="input-group-text bg-light text-muted border-end-0">
                        <i class="bi bi-key"></i>
                    </span>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control border-start-0" Placeholder="Enter your password" />
                </div>
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                    ControlToValidate="txtPassword" 
                    ErrorMessage="Password is required." 
                    CssClass="text-danger small mt-1 d-block" 
                    Display="Dynamic" />
            </div>

            <!-- Submit Button -->
            <div class="d-grid mb-3">
                <asp:Button ID="btnLogin" runat="server" Text="Sign In" CssClass="btn btn-primary py-2 fw-semibold shadow-sm" OnClick="btnLogin_Click" />
            </div>

            <!-- Student Registration Link -->
            <div class="text-center pt-2 border-top">
                <p class="text-muted small mb-0">
                    Are you a new student? 
                    <a href="Register.aspx" class="text-primary fw-semibold text-decoration-none">Register for an account</a>
                </p>
            </div>

            <!-- Quick Demo Credentials Box -->
            <!-- <div class="bg-light p-3 rounded-3 border mt-4">
                <div class="d-flex align-items-center gap-1 mb-2">
                    <i class="bi bi-info-circle-fill text-primary"></i>
                    <strong class="small text-dark">Pre-configured Demo Credentials:</strong>
                </div>
                <div class="d-flex flex-column gap-1 small">
                    <div class="d-flex justify-content-between align-items-center">
                        <span><strong>Admin:</strong> <code class="text-primary">admin</code> / <code class="text-primary">admin123</code></span>
                        <asp:Button ID="btnDemoAdmin" runat="server" Text="Fill Admin" CssClass="btn btn-xs btn-outline-primary py-0 px-2" CausesValidation="false" OnClick="btnDemoAdmin_Click" />
                    </div>
                    <div class="d-flex justify-content-between align-items-center mt-1">
                        <span><strong>Student:</strong> <code class="text-success">student</code> / <code class="text-success">student123</code></span>
                        <asp:Button ID="btnDemoStudent" runat="server" Text="Fill Student" CssClass="btn btn-xs btn-outline-success py-0 px-2" CausesValidation="false" OnClick="btnDemoStudent_Click" />
                    </div>
                </div>
            </div> -->
        </div>
    </div>
</asp:Content>
