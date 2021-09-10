Imports AgoraLegacy
Imports eProcure.Component

Public Class InvTaxCode
    Inherits AgoraLegacy.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents dtgCustomField As System.Web.UI.WebControls.DataGrid
    Protected WithEvents custom As System.Web.UI.HtmlControls.HtmlGenericControl

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)

        If Not IsPostBack Then
            lblTitle.Text = "Select Default SST Tax Code (Purchase)"
            hidID.Value = Request.QueryString("id")
            BindTaxCode()
        End If
    End Sub

    Sub BindTaxCode()
        Dim objGlobal As New AppGlobals
        objGlobal.FillTaxCode(ddlTaxCode, , "P", , False)
    End Sub
End Class
