Imports AgoraLegacy
Imports SSO.Component

Public Class RegenerateAllPassword
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents dtgUser As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdSend As System.Web.UI.WebControls.Button

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

        If Not IsPostBack Then
            ' for testing purpose only
            'Dim strsql As String
            'Dim ds As DataSet
            'Dim objDb As New  EAD.DBCom
            'strsql = "SELECT UM_COY_ID, UM_USER_ID, UM_USER_NAME, UM_EMAIL, UM_STATUS, CM_STATUS, CM_COY_NAME "
            'strsql &= "FROM COMPANY_MSTR "
            'strsql &= "INNER JOIN USER_MSTR ON CM_COY_ID = UM_COY_ID AND CM_DELETED <> 'Y' "
            'strsql &= "WHERE UM_DELETED <> 'Y' "
            'ds = objDb.FillDs(strsql)
            'dtgUser.DataSource = ds.Tables(0).DefaultView
            'dtgUser.DataBind()
            'objDb = Nothing
        End If
    End Sub

    Private Sub sendmail()
        Dim strsql As String
        Dim ds As DataSet
        Dim i, cnt As Integer

        Dim objPwd As New RandomPassword
        Dim objMail As New Email
        Dim objAppMail As New AppMail
        Dim objUser As New Users
        Dim objCommon As New AgoraLegacy.Common

        Dim strNewPwd As String
        Dim strBody, strCC, strTo, strSubject As String

        strNewPwd = objPwd.GenerateAutoPwd()
        Dim objDb As New  EAD.DBCom
        strsql = "SELECT UM_COY_ID, UM_USER_ID, UM_USER_NAME, UM_EMAIL, UM_STATUS, CM_STATUS, CM_COY_NAME "
        strsql &= "FROM COMPANY_MSTR "
        strsql &= "INNER JOIN USER_MSTR ON CM_COY_ID = UM_COY_ID AND CM_DELETED <> 'Y' "
        strsql &= "WHERE UM_DELETED <> 'Y' "
        strsql &= " AND UM_STATUS = 'A' AND CM_STATUS = 'A' "

        'Jules 2018.11.16 - Batch 3
        strsql &= " AND  CAST(um_ent_date AS DATE) BETWEEN '2018-11-10' AND NOW() AND um_coy_id='" & HttpContext.Current.Session("CompanyId") & "' "

        'strsql &= " AND UM_STATUS = 'A' AND CM_STATUS = 'A' AND um_user_id NOT LIKE 'ESA%' AND UM_EMAIL NOT LIKE '%prudential%' AND um_coy_id='pamb' " 'Test batch

        ds = objDb.FillDs(strsql)
        cnt = ds.Tables(0).Rows.Count

        For i = 0 To cnt - 1
            strNewPwd = objPwd.GenerateAutoPwd()
            If objUser.UpdatePassword(ds.Tables(0).Rows(i)("UM_USER_ID"), strNewPwd, ds.Tables(0).Rows(i)("UM_COY_ID"), True) Then
                ' if company is active and the use is also active, then send mail to them
                If ds.Tables(0).Rows(i)("UM_STATUS") = "A" And ds.Tables(0).Rows(i)("CM_STATUS") = "A" Then
                    'objAppMail.Subject = "Agora : Change Password"
                    objAppMail.Subject = "Agora: New User Account Creation"

                    objAppMail.MailTo = ds.Tables(0).Rows(i)("UM_EMAIL")

                    strBody = "Dear " & ds.Tables(0).Rows(i)("UM_USER_NAME") & ", <BR>"
                    'strBody &= "<P>Below is the newly generated temporary password."
                    'strBody &= "<P>Please use this temporary user password to login.<BR><P>"
                    strBody &= "<P>Your user account has been created by your Admin.<BR>"
                    strBody &= "<P>To access the application, please use the following login information.<BR><P>"

                    strBody &= "Company Name           : " & ds.Tables(0).Rows(i)("CM_COY_NAME")
                    strBody &= "<BR>Company ID             : " & ds.Tables(0).Rows(i)("UM_COY_ID")
                    strBody &= "<BR>User ID                : " & ds.Tables(0).Rows(i)("UM_USER_ID")
                    strBody &= "<BR>New Temporary Password : " & strNewPwd
                    strBody &= "<BR>Expiry Date            : " & objUser.GetPwdExpireDt(True) & "<BR>"

                    strBody &= "<P>To access, please login to https://pru2pay.com.my/<BR>"

                    strBody &= "<P>" & objCommon.EmailFooter & "<BR><P>" & objCommon.EmailCompGen

                    objAppMail.Body = strBody
                    objAppMail.SendMail()
                End If
            End If
        Next

        objCommon = Nothing
        objMail = Nothing
        objPwd = Nothing
        objAppMail = Nothing
        objUser = Nothing
    End Sub

    Private Sub cmdSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSend.Click
        sendmail()
    End Sub
End Class
