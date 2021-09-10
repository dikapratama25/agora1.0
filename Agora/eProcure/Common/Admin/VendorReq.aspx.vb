Imports AgoraLegacy
Imports eProcure.Component

Public Class VendorReq

    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents txt_CountryCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_CountryDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents vldCountryCode As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents lblCode As System.Web.UI.WebControls.Label
    Protected WithEvents lblDesc As System.Web.UI.WebControls.Label
    Protected WithEvents vldCountryDesc As System.Web.UI.WebControls.RequiredFieldValidator
    'Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton

    Dim strCode As String
    Dim objAdmin As New Admin()
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button
    Dim strType, strTypeDesc As String
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
        'strType = "CU"
        Select Case strType
            Case "CT"
                lblTitle.Text = "New Country"
                lblHeader.Text = "Request For New Country"
                lblCode.Text = "&nbsp;Country Code"
                lblDesc.Text = "&nbsp;Country Description"
                strTypeDesc = "Country"
            Case "CU"
                lblTitle.Text = "New Currency"
                lblHeader.Text = "Request For New Currency"
                lblCode.Text = "&nbsp;Currency Code"
                lblDesc.Text = "&nbsp;Currency Description"
                strTypeDesc = "Currency"
            Case "UOM"
                lblTitle.Text = "New UOM"
                lblHeader.Text = "Request For New UOM"
                lblCode.Text = "&nbsp;UOM Code"
                lblDesc.Text = "&nbsp;UOM Description"
                strTypeDesc = "UOM"
        End Select
        vldCountryCode.ErrorMessage = lblCode.Text & " is required field."
        vldCountryDesc.ErrorMessage = lblDesc.Text & " is required field."
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click

        Dim strDCode As String
        Dim strDName As String
        Dim strMsg As String
        Dim intMsgNo As Integer

        Dim objAdmin As New Admin
        strDCode = Common.parseNull(Me.txt_CountryCode.Text)
        strDName = Common.parseNull(Me.txt_CountryDesc.Text)

        intMsgNo = objAdmin.addVendorReq(strDCode, strDName, strType, "N")

        Select Case intMsgNo
            Case WheelMsgNum.Save
                strMsg = "Request has been submitted."
                txt_CountryCode.Text = ""
                txt_CountryDesc.Text = ""
            Case WheelMsgNum.Duplicate
                strMsg = "Request for " & strTypeDesc & " '" & strDCode & "' is already raised."
            Case WheelMsgNum.NotSave
                strMsg = "Request not submitted."
        End Select
        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
        objAdmin = Nothing
    End Sub


End Class
