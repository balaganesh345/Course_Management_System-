namespace CourseManagementSystem
{
    public partial class EditPage
    {
        protected global::System.Web.UI.WebControls.HyperLink lnkDetails;
        protected global::System.Web.UI.WebControls.Panel pnlMessage;
        protected global::System.Web.UI.WebControls.Label lblMessage;
        protected global::System.Web.UI.WebControls.ValidationSummary valSummary;
        protected global::System.Web.UI.WebControls.HiddenField hfCourseId;
        protected global::System.Web.UI.WebControls.TextBox txtCourseName;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvCourseName;
        protected global::System.Web.UI.WebControls.TextBox txtCourseCode;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvCourseCode;
        protected global::System.Web.UI.WebControls.RegularExpressionValidator revCourseCode;
        protected global::System.Web.UI.WebControls.TextBox txtCategory;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvCategory;
        protected global::System.Web.UI.WebControls.TextBox txtInstructor;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvInstructor;
        protected global::System.Web.UI.WebControls.TextBox txtDuration;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvDuration;
        protected global::System.Web.UI.WebControls.RangeValidator rvDuration;
        protected global::System.Web.UI.WebControls.TextBox txtFee;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvFee;
        protected global::System.Web.UI.WebControls.RangeValidator rvFee;
        protected global::System.Web.UI.WebControls.TextBox txtStartDate;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvStartDate;
        protected global::System.Web.UI.WebControls.TextBox txtEndDate;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvEndDate;
        protected global::System.Web.UI.WebControls.CompareValidator cvEndDate;
        protected global::System.Web.UI.WebControls.TextBox txtDescription;
        protected global::System.Web.UI.WebControls.CheckBox chkIsActive;
        protected global::System.Web.UI.WebControls.Button btnUpdate;
    }
}
