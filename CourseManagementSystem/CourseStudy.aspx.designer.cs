namespace CourseManagementSystem
{
    public partial class CourseStudyPage
    {
        protected global::System.Web.UI.WebControls.Literal litCourseCode;
        protected global::System.Web.UI.WebControls.Literal litCategory;
        protected global::System.Web.UI.WebControls.Literal litCourseName;
        protected global::System.Web.UI.WebControls.Literal litInstructor;
        protected global::System.Web.UI.WebControls.Literal litDuration;
        protected global::System.Web.UI.WebControls.Panel pnlMessage;
        protected global::System.Web.UI.WebControls.Label lblMessage;
        protected global::System.Web.UI.WebControls.Panel pnlRestricted;
        protected global::System.Web.UI.WebControls.Button btnEnrollNow;
        protected global::System.Web.UI.WebControls.Panel pnlStudyContent;
        protected global::System.Web.UI.WebControls.Panel pnlCompletedBadge;
        protected global::System.Web.UI.WebControls.Literal litCompletedScore;
        protected global::System.Web.UI.WebControls.Literal litCompletedDate;
        protected global::System.Web.UI.WebControls.Literal litMaterialsCount;
        protected global::System.Web.UI.WebControls.Repeater rptTextMaterials;
        protected global::System.Web.UI.WebControls.Label lblNoTextMaterials;
        protected global::System.Web.UI.WebControls.Repeater rptFileMaterials;
        protected global::System.Web.UI.WebControls.Label lblNoFileMaterials;
        protected global::System.Web.UI.WebControls.Literal litQ1Text;
        protected global::System.Web.UI.WebControls.RadioButtonList rblQ1;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvQ1;
        protected global::System.Web.UI.WebControls.Literal litQ2Text;
        protected global::System.Web.UI.WebControls.RadioButtonList rblQ2;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvQ2;
        protected global::System.Web.UI.WebControls.Button btnSubmitQuiz;
    }
}
