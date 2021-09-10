Imports System.Data.SqlClient
Imports AgoraLegacy
'Imports Microsoft.Web.UI.WebControls

Public Class _default
    Inherits System.Web.UI.Page

    Dim aTX123CoyId As String
    Dim aTX123UserId As String
    Dim aTotalPackage As Integer
    Protected WithEvents lblUserAndComp As System.Web.UI.WebControls.Label
    Protected WithEvents lblLastLogOn As System.Web.UI.WebControls.Label
    Protected WithEvents Panel1 As System.Web.UI.WebControls.Panel
    'Protected TreeView1 As Microsoft.Web.UI.WebControls.TreeView
    Protected WithEvents tree As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents imgMenu As System.Web.UI.HtmlControls.HtmlImage
    Protected WithEvents cmdSignOut As System.Web.UI.WebControls.Button
    Dim pack As New tx123_COMP.package
    Dim dDispatcher As New AgoraLegacy.dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents displayCredentials As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents divDisplay As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents divMenu As System.Web.UI.HtmlControls.HtmlGenericControl

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here

        Dim ctx As HttpContext = HttpContext.Current

        'aTX123CoyId = Request.Cookies("aUser").Value()
        aTX123CoyId = User.Identity.Name()
        aTX123UserId = Request.Cookies("aHideUserId").Value
        aTotalPackage = Request.Cookies("aTotalPackage").Value
        pack.getPackageDetails(aTX123CoyId, aTX123UserId)
        If aTotalPackage = 1 Then
            ctx.Response.Redirect(pack.packageURL.GetValue(0))
        Else
            'to prevent showing the orange color eprocurement while there is no package
            If aTotalPackage = 0 Then
                ctx.Response.Redirect(dDispatcher.direct("Initial", "Login.aspx"))
            End If
            divMenu.InnerHtml = genHTMLLink()

            'verify authentication
            If User.Identity.IsAuthenticated Then
                'display Credential information
                displayCredentials.InnerHtml = "" '"Current User : <b>" & User.Identity.Name & "</b>" & _
                '            "<br><br>Authentication Used : <b>" & User.Identity.AuthenticationType & "</b>"
            Else
                'Display Error Message
                displayCredentials.InnerHtml = "<font class='errmsg'>Sorry, You have not been authenticated.</font>"
            End If

            ' ai chu add on 28/10/2005
            lblLastLogOn.Text = Session("Last_Login")
            lblUserAndComp.Text = "Welcome, " & Session("UserName") & " from " & Session("CompanyName")
            'imgMenu.Attributes.Add("Onclick", "Display()")
        End If

    End Sub

    Private Function genHTMLLink() As String
        Dim strOutput As String
        Dim i As Integer
        strOutput = "<table align='left' border='0' width='60%'>"

        For i = 0 To aTotalPackage - 1
            'strOutput += "<td align='center'><a href =" & pack.packageURL.GetValue(i) & ">" & _
            '            pack.packageName.GetValue(i) & "</a></td>"
            strOutput &= "<tr><td width=10%>&nbsp;</td><td width=10%><a href=" & pack.packageURL.GetValue(i) & ">"
            'strOutput &= "<IMG border=0 src='images\t_" & pack.packageName.GetValue(i) & ".gif' "
            strOutput &= "<IMG border=0 src='images\t_" & pack.packageID.GetValue(i) & ".gif' "
            strOutput &= "alt='Click here to go to " & pack.packageName.GetValue(i) & "' >"
            strOutput &= "</a>" & "</td><td valign='middle' width=80%><B><font size=3>" & pack.packageName.GetValue(i) & "</font></B></td>"
            strOutput &= "</td></tr>"
        Next

        ' logout icon
        strOutput &= "<tr><td width=10%>&nbsp;</td><td width=10%><a href='" & dDispatcher.direct("Initial", "SignOut.aspx") & "'><IMG border=0 src='images\t_logout.gif' alt='Click here to logout.'></a></td><td valign='middle'><B><font size=3>Logout</font></B></td></tr>"
        strOutput += "</table>"
        Return strOutput
    End Function

    Private Sub cmdSignOut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSignOut.Click
        Response.Redirect(dDispatcher.direct("Initial", "SignOut.aspx"))
    End Sub
End Class
