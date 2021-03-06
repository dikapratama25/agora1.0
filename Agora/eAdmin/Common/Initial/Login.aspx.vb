Imports ERFP.Components
Imports SSO.Component
Imports System.Web.Security
Imports AgoraLegacy

Public Class Login
    Inherits System.Web.UI.Page
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents txtUserID As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCompID As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPassword As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkCookies As System.Web.UI.WebControls.CheckBox
    Dim dDispatcher As New AgoraLegacy.dispatcher

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

    Private Sub GetLogin()
        Dim bError As Boolean = False
        Dim UserDetails As User
        Dim strArray(0) As String

        If chkCookies.Checked = True Then
            Response.Cookies("aUserId").Value = Trim(txtUserID.Text)
            Response.Cookies("aCompanyId").Value = Trim(txtCompID.Text)
            Response.Cookies("auserId").Expires = DateTime.Now.AddMonths(1)
            Response.Cookies("aCompanyId").Expires = DateTime.Now.AddMonths(1)
        Else
            Response.Cookies("aUserId").Expires = DateTime.Now.AddDays(-1)
            Response.Cookies("aCompanyId").Expires = DateTime.Now.AddDays(-1)
        End If

        If Trim(txtCompID.Text) = "" Then lblMsg.Text = "Please enter company id." : bError = True
        If Trim(txtUserID.Text) = "" Then lblMsg.Text = "Please enter user id." : bError = True
        If Trim(txtPassword.Text) = "" Then lblMsg.Text = "Please enter password." : bError = True
        If Trim(txtUserID.Text) = "" And txtPassword.Text = "" Then lblMsg.Text = "Please enter user id and password." : bError = True

        If bError = False Then
            Dim objUser As New Users
            Dim objUserDetails As New User
            Dim objDb As New  EAD.DBCom
            Dim objUsers As New Users
            Dim sUser_ID As String = objUser.Login(Trim(txtUserID.Text), Trim(txtPassword.Text), Trim(txtCompID.Text))

            If sUser_ID = "@Connection" Then
                lblMsg.Text = "Database connection error. Please contact your administrator."
            Else
                If sUser_ID <> "" Then

                    UserDetails = objUser.GetUserDetails(sUser_ID, Trim(txtCompID.Text)) 'Lookup the customer's full account details
                    objUser.UpdateLoginDt(UserDetails) 'Update login time
                    objUsers.Log_UserActivity_NonSession(strArray, Trim(txtCompID.Text), sUser_ID, WheelModule.SecurityControl, WheelUserActivity.Login, "Success")
                    objDb.BatchExecute(strArray)
                    'Session("UserDetails") = UserDetails
                    Dim objLP As New LoginPolicy
                    Dim objLPolicy As New LoginPolicy
                    Dim objComp As New Companies
                    Dim iAccDormant As Integer = objLPolicy.getParamValue("ACCOUNT_DORMANT")

                    'Pwd suspend
                    If UserDetails.Status = "S" Then
                        Exit Sub
                    End If

                    'Pwd dormant
                    If UserDetails.LastLogin <> "#12:00:00 AM#" AndAlso DateDiff(DateInterval.Day, UserDetails.LastLogin, Today) > iAccDormant Then
                        If objLP.getParamInd("ACCOUNT_DORMANT") = "YES" Then
                            'lblMsg.Text = "Your user account has been locked."
                            ' Spec from moo, no need to change user account to inactive for account exceed dormant period
                            'lblMsg.Text = "Your account is inactive. Please contact your administrator."
                            'Call objUser.ActivateUserAccount(txtCompID.Text, sUser_ID, False)
                            lblMsg.Text = "Your account is inactive. Please try to login again."
                            Exit Sub
                        End If
                    End If

                    'tnc: form authentication and authorization
                    Dim aTX123CoyId As String
                    Dim aTX123UserId As String
                    Dim aTotalPackage As Integer
                    Dim pack As New tx123_COMP.package
                    Dim i As Integer
                    Dim strCookies As String
                    Dim strPackageUrl As String
                    Dim strPackageID As String

                    aTX123CoyId = Trim(txtCompID.Text)
                    aTX123UserId = Trim(txtUserID.Text)
                    aTotalPackage = pack.getCountTotalPackages(aTX123CoyId, aTX123UserId)
                    pack.getPackageDetails(aTX123CoyId, aTX123UserId)
                    For i = 0 To aTotalPackage - 1
                        If i = aTotalPackage - 1 Then
                            strCookies += pack.packageName.GetValue(i)
                            strPackageUrl += pack.packageURL.GetValue(i)
                            strPackageID += pack.packageID.GetValue(i)
                        Else
                            strCookies += pack.packageName.GetValue(i) & "|"
                            strPackageUrl += pack.packageURL.GetValue(i) & "|"
                            strPackageID += pack.packageID.GetValue(i) & "|"
                        End If
                    Next

                    Response.Cookies("aHideUserId").Value = Trim(txtUserID.Text) 'tnc
                    Response.Cookies("aHideCompanyId").Value = Trim(txtCompID.Text)
                    Response.Cookies("aPackageName").Value = strCookies
                    Response.Cookies("aPackageUrl").Value = strPackageUrl
                    Response.Cookies("aPackageID").Value = strPackageID
                    Response.Cookies("aTotalPackage").Value = aTotalPackage
                    Response.Cookies("aLastLogin").Value = UserDetails.LastLogin
                    FormsAuthentication.SetAuthCookie(Trim(txtCompID.Text), False)
                    'FormsAuthentication.RedirectFromLoginPage(txtCompID.Text, False)
                    'tnc end

                    'first time login aft account created
                    If UserDetails.NewPwdInd = "Y" Then
                        If objLP.getParamInd("NEW_LOGIN") = "YES" Then
                            'Response.Redirect("ChgPassword.aspx?modes=F&status=N&userid=" & Trim(txtUserID.Text) & "&coyid=" & Trim(txtCompID.Text))
                            Response.Redirect(dDispatcher.direct("Initial", "ChgPassword.aspx", "modes=F&status=N&userid=" & Trim(txtUserID.Text) & "&coyid=" & Trim(txtCompID.Text)))
                        End If
                    End If

                    'Pwd expired
                    If DateDiff(DateInterval.Day, Today, UserDetails.NextExpiredDt) < 0 Then
                        'Response.Redirect("ChgPassword.aspx?modes=F&status=E&userid=" & Trim(txtUserID.Text) & "&coyid=" & Trim(txtCompID.Text))
                        Response.Redirect(dDispatcher.direct("Initial", "ChgPassword.aspx", "modes=F&status=E&userid=" & Trim(txtUserID.Text) & "&coyid=" & Trim(txtCompID.Text)))
                        Exit Sub
                    End If

                    PreLogin(UserDetails, objComp)
                    'Response.Redirect("Index.aspx") 'tnc
                    Response.Redirect(dDispatcher.direct("Initial", "Index.aspx")) 'tnc
                Else
                    objUsers.Log_UserActivity_NonSession(strArray, Trim(txtCompID.Text), (txtUserID.Text), WheelModule.SecurityControl, WheelUserActivity.Login, "Fail")
                    objDb.BatchExecute(strArray)
                    lblMsg.Text = objUser.Message
                End If
            End If
        End If
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'tnc: check for unauthenticate and timeout, then redirect to login page.
        Dim strURL As String = Trim(Request.QueryString("ReturnUrl"))
        If Not (strURL = "") Then
            If ShowSessionTimeOutMsgBox(strURL) = True Then
                'NetMsgbox("You session has expired, please login again.", "Login.aspx", MsgBoxStyle.Information)
                NetMsgbox("You session has expired, please login again.", dDispatcher.direct("Initial", "Login.aspx"), MsgBoxStyle.Information)
            Else
                RedirectToParent()
            End If
        End If

        If Not Page.IsPostBack Then
            RemoveRedirCookie()
            ClearUpSession()

            If Not Request.Cookies("aCompanyId") Is Nothing AndAlso Not Request.Cookies("aUserId") Is Nothing Then
                Dim aCookieCoy As HttpCookie = Request.Cookies("aCompanyId")
                txtCompID.Text = Server.HtmlEncode(aCookieCoy.Value)
                Dim aCookieUserId As HttpCookie = Request.Cookies("aUserId")
                txtUserID.Text = Server.HtmlEncode(aCookieUserId.Value)
                chkCookies.Checked = True
                'Else
                '    viewstate("UserID") = Request.Params("userid")
                '    txtUserID.Text = viewstate("UserID")
            End If

            txtPassword.MaxLength = Common.GetPasswordMaxLength(New  EAD.DBCom)
        Else
            GetLogin()
        End If
        'lnkBrowser.Attributes.Add("OnClick", "window.open('UpdateBrowser.htm',  '', 'resizable=no,scrollbars=yes,left=100,top=100,width=600,height=300,status=no,menubar=no');")
        'lnkBrowser.NavigateUrl = "#"
    End Sub

    Private Sub PreLogin(ByVal pUserDetails As User, ByVal pCoyDetails As Companies)
        Session("UserId") = Common.Parse(Trim(txtUserID.Text))
        Session("UserName") = pUserDetails.Name
        Session("CompanyId") = Common.Parse(Trim(txtCompID.Text))
        Session("CompanyName") = pCoyDetails.GetCompanyName(Trim(txtCompID.Text))
        Session("Last_Login") = Common.FormatWheelDate(WheelDateFormat.LongDateTimeDay, pUserDetails.LastLogin)
        'Michelle (6/10/2010) - To get the Environment
        Session("Env") = System.Configuration.ConfigurationSettings.AppSettings.Get("Env")
        Session("FileSize") = System.Configuration.ConfigurationSettings.AppSettings.Get("FileSize")

        Session("WheelScript") = "<script type=""text/javascript"" src = """ & dDispatcher.direct("Plugins/Include", "WheelScript.js") & """></script>"
        Session("eRFPScript") = "<script type=""text/javascript"" src = """ & dDispatcher.direct("Plugins/Include", "eRFPScript.js") & """></script>"
        Session("PNGFix") = "<script defer type=""text/javascript"" src=""" & dDispatcher.direct("Plugins/Include", "PNGFix.js") & """></script>"
        Session("eRFPCDClock") = "<script type=""text/javascript"" src = """ & dDispatcher.direct("Plugins/Include", "eRFPCDClock.js") & """></script>"
        Session("AutoComplete") = "<script type=""text/javascript"" src= """ & dDispatcher.direct("Plugins/Include", "JqueryAutoComplete.js") & """></script>"
        'Response.Redirect("Index.aspx") 'tnc
    End Sub

    Private Sub ClearUpSession()
        Session.Abandon()
        Session("UserId") = Nothing
        Session("UserName") = Nothing
        Session("CompanyId") = Nothing
        Session("CompanyName") = Nothing
        Session("Last_Login") = Nothing
        Session("Env") = Nothing

        Response.Cookies.Add(New HttpCookie("ASP.NET_SessionId", ""))

        'To make sure that the client clears the session ID cookie, respond to the client to tell
        'it that we have responded. To do this, set another cookie.
        AddRedirCookie()
        'Response.Redirect(Request.Path)
    End Sub

    Private Sub AddRedirCookie()
        Dim ticket As FormsAuthenticationTicket
        ticket = New FormsAuthenticationTicket(1, "Test", DateTime.Now, DateTime.Now.AddSeconds(5), False, "")

        Dim encryptedText As String = FormsAuthentication.Encrypt(ticket)
        Response.Cookies.Add(New HttpCookie("__LOGINCOOKIE__", encryptedText))
    End Sub

    Private Sub RemoveRedirCookie()
        'If viewstate("SessionID") Is Nothing Then
        Request.Cookies.Remove("__LOGINCOOKIE__")
        'End If
        Response.Cookies.Add(New HttpCookie("__LOGINCOOKIE__", ""))
    End Sub

    Private Sub RedirectToParent()
        Dim vbs As String
        vbs = vbs & "<script language=""vbs"">"
        'vbs = vbs & vbLf & "parent.location=""login.aspx"""
        vbs = vbs & vbLf & "parent.location=" & dDispatcher.direct("Initial", "login.aspx") & """"
        vbs = vbs & "</script>"
        Dim rndKey As New Random
        Me.RegisterStartupScript(rndKey.Next.ToString, vbs)
    End Sub

    Private Sub NetMsgbox(ByVal msg As String, ByVal pRedirect As String, Optional ByVal style As MsgBoxStyle = MsgBoxStyle.Exclamation, Optional ByVal title As String = "eProcurement")
        Dim vbs As String
        vbs = vbs & "<script language=""vbs"">"
        vbs = vbs & "Call MsgBox(""" & msg & """, " & style & ", """ & title & """)"
        vbs = vbs & vbLf & "parent.location=""" & pRedirect & """"
        vbs = vbs & "</script>"
        Dim rndKey As New Random
        Me.RegisterStartupScript(rndKey.Next.ToString, vbs)
    End Sub

    Private Function ShowSessionTimeOutMsgBox(ByVal url As String) As Boolean
        Dim countLastSlash As Integer
        If Not (url = "") Then
            countLastSlash = url.LastIndexOf("/")
            url = LCase(url.Substring(countLastSlash + 1))
            Select Case url
                Case "login.aspx", "default.aspx", "index.aspx", "forgotpwd.aspx"
                    Return False
                    Exit Function
                Case Else
                    Return True
                    Exit Function
            End Select
        Else
            Return False
        End If
    End Function
End Class