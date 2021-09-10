Imports AgoraLegacy
Imports eProcure.Component
Public Class AddWarrantyTerms
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSelect As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents lblValue As System.Web.UI.WebControls.Label
    Protected WithEvents txtValue As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents hidID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidType As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label

    Dim strType As String
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)

        strType = Me.Request.QueryString("type")
        hidType.Value = strType
        Select Case strType
            Case "E"
                lblTitle.Text = "Add Delivery Lead Time "
                lblHeader.Text = "Add Delivery Lead Time (days) "
                lblValue.Text = "Add Delivery Lead Time (days) :"
                'Michelle (2/8/2010) - Hide the lblMsg
                ' lblMsg.Text = "[enter zero for ex_stock]"
            Case "W"
                lblTitle.Text = "Add Warranty Terms "
                lblHeader.Text = "Add Warranty Terms (months) "
                lblValue.Text = "Add Warranty Terms (months) :"
                lblMsg.Text = ""
        End Select
        hidID.Value = Request.QueryString("id")
    End Sub

    Private Sub txtValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtValue.TextChanged
        If txtValue.Text <> "" Then
            cmdSelect.Disabled = False
        End If
    End Sub
End Class
