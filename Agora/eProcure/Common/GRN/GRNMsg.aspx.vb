Imports AgoraLegacy
Imports eProcure.Component
Public Class GRNMsg
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher = New dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents lblDONum As System.Web.UI.WebControls.Label
    Protected WithEvents lblSts As System.Web.UI.WebControls.Label
    Protected WithEvents lblHD As System.Web.UI.WebControls.Label
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents Pass As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents Fail As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents lblGRNNum As System.Web.UI.WebControls.Label
    Protected WithEvents lblLevel As System.Web.UI.WebControls.Label
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CheckButtonAccess(True)
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        Dim StrDONo, strHD, strGRNNo As String
        Dim blnPass As Boolean

        StrDONo = Me.Request.QueryString("DONo")
        strHD = Me.Request.QueryString("HD")
        strGRNNo = Me.Request.QueryString("GRNNo")
        'blnPass = Me.Request.QueryString("Pass")
        lblDONum.Text = StrDONo
        lblHeader.Text = strHD
        lblGRNNum.Text = strGRNNo
        lblHD.Text = strHD

        If strHD = "" Then
            lblHeader.Text = "Goods Receipt Note Generation"
        End If
        lblTitle.Text = lblHeader.Text
        '**************meilai 15/2/2005*****************
        lblLevel.Text = Request.QueryString("LevelType")
        '**************meilai***************************
        Pass.Style.Item("Display") = "inline"
        Fail.Style.Item("Display") = "none"
        '//remark by moo
        '//hv to check code first
        'If StrDONo = "" And blnPass = False Then
        '    Pass.Style.Item("Display") = "none"
        '    Fail.Style.Item("Display") = "inline"

        'Else
        '    Fail.Style.Item("Display") = "none"
        '    Pass.Style.Item("Display") = "Inline"
        'End If
        If strGRNNo = "error" Then
            Pass.Style.Item("Display") = "none"
            Fail.Style.Item("Display") = "inline"
        Else
            Fail.Style.Item("Display") = "none"
            Pass.Style.Item("Display") = "Inline"
        End If
        'lnkBack.NavigateUrl = "GRNSearch.aspx?pageid=" & strPageId & "&type=" & Request.Params("type")
        lnkBack.NavigateUrl = dDispatcher.direct("GRN", "GRNSearch.aspx", "pageid=" & strPageId & "&type=" & Request.Params("type"))
    End Sub


End Class
