Imports SSO.Component
Imports AgoraLegacy
Public Class Header
    Inherits System.Web.UI.Page
    Protected WithEvents a As System.Web.UI.HtmlControls.HtmlAnchor
    Protected WithEvents lblHeaderLink As System.Web.UI.WebControls.Label
    Protected WithEvents lblWelcome As System.Web.UI.WebControls.Label
    Protected WithEvents lblLastlogin As System.Web.UI.WebControls.Label

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

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        genHeader()
    End Sub

    Private Sub genHeader() 'optional header
        'Dim packageName() As String = {"eProcure", "eRFP", "eAuction", "eContract"}
        'Dim authPackageName() As String
        'Dim authPackageUrl() As String
        'Dim totalpackage As Integer
        'Dim j As Integer
        Dim strOutput As String = ""

        'authPackageName = Request.Cookies("aPackageName").Value.Split("|")
        'authPackageUrl = Request.Cookies("aPackageUrl").Value.Split("|")
        'totalpackage = Request.Cookies("aTotalPackage").Value

        'If totalpackage > 1 Then
        '    strOutput = "<table align='center' border='0' class='alltable'><tr width='100%'>"

        '    For j = 0 To authPackageName.Length - 1
        '        strOutput &= "<td align='center'><a target='_parent' href =" & authPackageUrl(j) & ">"
        '        strOutput &= "<IMG border=0 src='images\t_" & authPackageName.GetValue(j) & ".gif' "
        '        strOutput &= "alt='Click here to go to " & authPackageName.GetValue(j) & "' >"
        '        strOutput &= "</a></td>"
        '    Next

        '    strOutput &= "</tr></table>"
        'End If
        'lblHeaderLink.Text = strOutput
        If Not Page.IsPostBack Then
            lblWelcome.Text = "Welcome, " & Session("UserName") & " from " & Session("CompanyName")
            lblLastlogin.Text = "Last Log On: " & Session("Last_Login")
        End If



    End Sub
End Class
