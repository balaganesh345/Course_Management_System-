namespace CourseManagementSystem
{
    public partial class DetailsPage
    {
        protected global::System.Web.UI.WebControls.Literal litCourseCode;
        protected global::System.Web.UI.WebControls.Literal litCategory;
        protected global::System.Web.UI.WebControls.Literal litStatusBadge;
        protected global::System.Web.UI.WebControls.Literal litCourseName;
        protected global::System.Web.UI.WebControls.Literal litInstructor;
        protected global::System.Web.UI.WebControls.Literal litCreatedAt;
        protected global::System.Web.UI.WebControls.PlaceHolder phAdminDetailsActions;
        protected global::System.Web.UI.WebControls.HyperLink lnkEdit;
        protected global::System.Web.UI.WebControls.HyperLink lnkDelete;
        protected global::System.Web.UI.WebControls.HyperLink lnkStudyPreview;
        protected global::System.Web.UI.WebControls.PlaceHolder phStudentDetailsActions;
        protected global::System.Web.UI.WebControls.Button btnEnrollDetails;
        protected global::System.Web.UI.WebControls.Label lblEnrolledBadge;
        protected global::System.Web.UI.WebControls.HyperLink lnkStudentStudy;
        protected global::System.Web.UI.WebControls.Panel pnlMessage;
        protected global::System.Web.UI.WebControls.Label lblMessage;
        protected global::System.Web.UI.WebControls.Literal litFee;
        protected global::System.Web.UI.WebControls.Literal litDuration;
        protected global::System.Web.UI.WebControls.Literal litStartDate;
        protected global::System.Web.UI.WebControls.Literal litEndDate;
        protected global::System.Web.UI.WebControls.Literal litDescription;

        // Materials
        protected global::System.Web.UI.WebControls.PlaceHolder phMaterialsSection;
        protected global::System.Web.UI.WebControls.PlaceHolder phAddMaterialAdmin;
        protected global::System.Web.UI.WebControls.RadioButtonList rblAddMaterialType;
        protected global::System.Web.UI.WebControls.TextBox txtAddMaterialTitle;
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl divTextContent;
        protected global::System.Web.UI.WebControls.TextBox txtAddMaterialText;
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl divFileUpload;
        protected global::System.Web.UI.WebControls.FileUpload fuAddMaterialFile;
        protected global::System.Web.UI.WebControls.Button btnAddMaterial;
        protected global::System.Web.UI.WebControls.Repeater rptDetailsMaterials;
        protected global::System.Web.UI.WebControls.Label lblNoMaterials;
        protected global::System.Web.UI.WebControls.PlaceHolder phNotEnrolledNotice;

        // Quiz Management
        protected global::System.Web.UI.WebControls.PlaceHolder phQuizManagement;
        protected global::System.Web.UI.WebControls.TextBox txtQuizQ1;
        protected global::System.Web.UI.WebControls.TextBox txtQuizQ1A;
        protected global::System.Web.UI.WebControls.TextBox txtQuizQ1B;
        protected global::System.Web.UI.WebControls.TextBox txtQuizQ1C;
        protected global::System.Web.UI.WebControls.TextBox txtQuizQ1D;
        protected global::System.Web.UI.WebControls.DropDownList ddlQuizQ1Ans;
        protected global::System.Web.UI.WebControls.TextBox txtQuizQ2;
        protected global::System.Web.UI.WebControls.TextBox txtQuizQ2A;
        protected global::System.Web.UI.WebControls.TextBox txtQuizQ2B;
        protected global::System.Web.UI.WebControls.TextBox txtQuizQ2C;
        protected global::System.Web.UI.WebControls.TextBox txtQuizQ2D;
        protected global::System.Web.UI.WebControls.DropDownList ddlQuizQ2Ans;
        protected global::System.Web.UI.WebControls.Button btnSaveQuizSettings;

        // Student Quiz Banner
        protected global::System.Web.UI.WebControls.PlaceHolder phStudentQuizLink;
        protected global::System.Web.UI.WebControls.Literal litStudentQuizStatus;

        // Enrolled Students List
        protected global::System.Web.UI.WebControls.PlaceHolder phAdminEnrolledStudents;
        protected global::System.Web.UI.WebControls.Literal litCourseEnrolledCount;
        protected global::System.Web.UI.WebControls.GridView gvCourseStudents;
    }
}
