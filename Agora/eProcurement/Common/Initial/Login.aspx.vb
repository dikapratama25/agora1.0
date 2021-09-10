Imports ERFP.Components
Imports SSO.Component
Imports System.Web.Security
Imports AgoraLegacy
Imports Access
Imports System
Imports System.Xml

Public Class LWebForm1
    Inherits System.Web.UI.Page
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Protected WithEvents hidBtnContinue As System.Web.UI.HtmlControls.HtmlInputButton

    Private Sub GetLoginFromBilling()
        Dim bError As Boolean = False
        Dim UserDetails As User
        Dim strArray(0) As String
        Dim statusFrmBill, amtDue As String
        If bError = False Then
            txtCompID.Text = Request.QueryString("cid")
            txtUserID.Text = Request.QueryString("uid")
            statusFrmBill = Request.QueryString("status")
            amtDue = Request.QueryString("amtDue")

            Dim objUser As New Users
            Dim objUserDetails As New User
            Dim objDb As New EAD.DBCom
            Dim objUsers As New Users
            Dim getPassword = objUser.GetUserPassword(Request.QueryString("uid"), Request.QueryString("cid"), Request.QueryString("user"), Replace(Date.Today(), "/", ""))
            If getPassword Is Nothing Then
                lblMsg.Text = "Login fail. Please contact your administrator."
                Exit Sub
            End If
            Dim sUser_ID As String = objUser.SingleLogin(Request.QueryString("uid"), getPassword, Request.QueryString("cid"))

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
                            Response.Redirect("ChgPassword.aspx?modes=F&status=N&userid=" & Trim(txtUserID.Text) & "&coyid=" & Trim(txtCompID.Text))
                        End If
                    End If

                    'Pwd expired
                    If DateDiff(DateInterval.Day, Today, UserDetails.NextExpiredDt) < 0 Then
                        Response.Redirect("ChgPassword.aspx?modes=F&status=E&userid=" & Trim(txtUserID.Text) & "&coyid=" & Trim(txtCompID.Text))
                        Exit Sub
                    End If

                    PreLogin(UserDetails, objComp, statusFrmBill)
                    'Do action to the access status
                    'If Session("Env") = "FTN" Then
                    '    If Session("CompanyType") <> "Buyer-FTN" And Session("CompanyType") <> "Vendor" Then
                    '        lblMsg.Text = "This company is not belong to this URL"
                    '        Exit Sub
                    '    End If
                    'ElseIf Session("Env") = "SMB" Then
                    '    If Session("CompanyType") <> "Buyer-SMB" And Session("CompanyType") <> "Vendor" Then
                    '        lblMsg.Text = "This company is not belong to this URL"
                    '        Exit Sub
                    '    End If
                    'Else
                    '    If Session("CompanyType") <> "Buyer-Enterprise" And Session("CompanyType") <> "Vendor" Then
                    '        lblMsg.Text = "This company is not belong to this URL"
                    '        Exit Sub
                    '    End If
                    'End If
                    If Session("CompanyType") <> "Buyer-" & Session("Env") & "" And Session("CompanyType") <> "Vendor" Then
                        lblMsg.Text = "This company does not belong to this URL"
                        Exit Sub
                    End If

                    Response.Cookies("aAccessStatus").Value = Session("AccessStatus")
                    If Session("AccessStatus") = "Active" Then
                        'Michelle (21/5/2013) - Issue 1934
                        If amtDue > "0" Then
                            Response.Cookies("aGracePeriod").Value = "True"
                        Else
                            Response.Cookies("aGracePeriod").Value = "False"
                        End If

                        Response.Redirect("default.aspx")
                    ElseIf Session("AccessStatus") = "Limited" Then
                        Response.Redirect("default.aspx")
                    ElseIf Session("AccessStatus") = "Suspend" Then
                        lblMsg.Text = "Your account is inactive. Please contact your administrator."
                        Session.Clear()
                    End If
                Else
                    objUsers.Log_UserActivity_NonSession(strArray, Trim(txtCompID.Text), (txtUserID.Text), WheelModule.SecurityControl, WheelUserActivity.Login, "Fail")
                    objDb.BatchExecute(strArray)
                    lblMsg.Text = objUser.Message
                End If
            End If
        End If
    End Sub
    Private Sub GetLogin()
        Dim bError As Boolean = False
        Dim UserDetails As User
        Dim strArray(0) As String
        Dim strMsgBox As String
        Dim strscript As New System.Text.StringBuilder

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
            Dim objDb As New EAD.DBCom
            Dim objUsers As New Users
            Dim sUser_ID As String = objUser.Login(Trim(txtUserID.Text), Trim(txtPassword.Text), Trim(txtCompID.Text))

            If sUser_ID = "@Connection" Then
                lblMsg.Text = "Database connection error. Please contact your administrator."
            Else
                If sUser_ID <> "" Then

                    UserDetails = objUser.GetUpdateUserDetails(sUser_ID, Trim(txtCompID.Text)) 'Lookup the customer's full account details
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
                            Response.Redirect("ChgPassword.aspx?modes=F&status=N&userid=" & Trim(txtUserID.Text) & "&coyid=" & Trim(txtCompID.Text))
                        End If
                    End If

                    'Pwd expired
                    If DateDiff(DateInterval.Day, Today, UserDetails.NextExpiredDt) < 0 Then
                        Response.Redirect("ChgPassword.aspx?modes=F&status=E&userid=" & Trim(txtUserID.Text) & "&coyid=" & Trim(txtCompID.Text))
                        Exit Sub
                    End If

                    PreLogin(UserDetails, objComp)
                    'Do action to the access status                    
                    'If Session("Env") = "FTN" Then
                    '    If Session("CompanyType") <> "Buyer-FTN" And Session("CompanyType") <> "Vendor" Then
                    '        lblMsg.Text = "Invalid URL for this company"
                    '        Exit Sub
                    '    End If
                    'ElseIf Session("Env") = "SMB" Then
                    '    If Session("CompanyType") <> "Buyer-SMB" And Session("CompanyType") <> "Vendor" Then
                    '        lblMsg.Text = "This company is not belong to this URL"
                    '        Exit Sub
                    '    End If
                    'Else
                    '    If Session("CompanyType") <> "Buyer-Enterprise" And Session("CompanyType") <> "Vendor" Then
                    '        lblMsg.Text = "Invalid URL for this company"
                    '        Exit Sub
                    '    End If
                    'End If
                    If Session("CompanyType") <> "Buyer-" & Session("Env") & "" And Session("CompanyType") <> "Vendor" Then
                        lblMsg.Text = "This company is not belong to this URL"
                        Exit Sub
                    End If

                    Response.Cookies("aAccessStatus").Value = Session("AccessStatus")
                    Response.Cookies("aGracePeriod").Value = "False"
                    If Session("AccessStatus") = "Active" Then
                        If Session("AmtDue") > "0" Then
                            Response.Cookies("aGracePeriod").Value = "True"
                            'strMsgBox = "Dear customer, you have outstanding amount of " & Session("Curr") & " " & Session("AmtDue") & """& vbCrLf & """ & "and your account will be turned to limited access in " & Session("Day") & " day/days."
                            'strMsgBox &= """& vbCrLf & """ & """& vbCrLf & """ & "Please top up your prepaid balance to clear the outstanding amount " & """& vbCrLf & """ & "within the grace period to prevent service interruption."
                            'Common.NetMsgbox(Me, strMsgBox, dDispatcher.direct("Initial", "default.aspx"), MsgBoxStyle.Information)
                            'Common.NetMsgbox(strMsgBox, dDispatcher.direct("Initial", "default.aspx"), MsgBoxStyle.Information)

                            strscript.Append("<script language=""javascript"">")
                            strscript.Append("document.getElementById('hidBtnContinue').click();")
                            strscript.Append("</script>")
                            RegisterStartupScript("script3", strscript.ToString())


                        Else
                            'UserDetails.LastLogin = "#12:00:00 AM#"
                            Dim strTempPath As String = Server.MapPath("../Report/Temp/ContractCatalogueListing_" & HttpContext.Current.Session("CompanyId") & "_" & HttpContext.Current.Session("UserId") & ".xls")



                            'Dim strCheckForAgreement As Boolean
                            Dim strFileName2 As String
                            If UserDetails.PolicyAgreeDt = "" Then
                                'If strCheckForAgreement = True Then
                                strscript.Append("<script language=""javascript"">")
                                strFileName2 = dDispatcher.direct("Initial", "Policy.aspx?&userid=" & Trim(txtUserID.Text) & "&cid=" & Trim(txtCompID.Text) & "")
                                strFileName2 = Server.UrlEncode(strFileName2)
                                'strscript.Append("PopWindow('" & dDispatcher.direct("DO", "LotDeliveryOrder.aspx", strName) & "');")
                                strscript.Append("ShowDialog('" & dDispatcher.direct("Initial", "Dialog.aspx", "page=" & strFileName2) & "','400px');")
                                '' ''strscript.Append("ShowDialog('Policy.aspx','400px');")
                                strscript.Append("document.getElementById('btnhidden1').click();")
                                strscript.Append("</script>")
                                RegisterStartupScript("script3", strscript.ToString())
                            Else
                                Response.Redirect("default.aspx")
                            End If
                        End If
                    ElseIf Session("AccessStatus") = "Limited" Then
                        'strMsgBox = "Dear customer, your account had been turned to limited access due to" & """& vbCrLf & """ & "there is outstanding invoice pending in Billing System."
                        'strMsgBox &= """& vbCrLf & """ & """& vbCrLf & """ & "Please top up your prepaid balance to clear the outstanding amount to" & """& vbCrLf & """ & "resume the service and continue using AGORA."
                        'Common.NetMsgbox(Me, strMsgBox, dDispatcher.direct("Initial", "default.aspx"), MsgBoxStyle.Information)

                        strscript.Append("<script language=""javascript"">")
                        strscript.Append("document.getElementById('hidBtnContinue').click();")
                        strscript.Append("</script>")
                        RegisterStartupScript("script4", strscript.ToString())
                    ElseIf Session("AccessStatus") = "Suspend" Then
                        lblMsg.Text = "Your account has been suspended. Please contact your administrator."
                        Session.Clear()
                    End If
                    'tnc
                Else
                    objUsers.Log_UserActivity_NonSession(strArray, Trim(txtCompID.Text), (txtUserID.Text), WheelModule.SecurityControl, WheelUserActivity.Login, "Fail")
                    objDb.BatchExecute(strArray)
                    lblMsg.Text = objUser.Message
                End If
            End If
        End If
    End Sub

    Public Sub btnHidden1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHidden1.Click
        Dim dDispatcher As New AgoraLegacy.dispatcher
        'Dim blnCheckAgree As String = "Y"
        If Session("Decline") = "Agree" Then
            Response.Redirect("default.aspx")
        Else
            Response.Redirect(dDispatcher.direct("Initial", "Login.aspx"))
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'tnc: check for unauthenticate and timeout, then redirect to login page.
        Dim strURL As String = Trim(Request.QueryString("ReturnUrl"))
        If Not (strURL = "") Then
            If InStr(strURL, "SPP.aspx") > 0 Then
                Dim dDispatcher As New AgoraLegacy.dispatcher
                Response.Redirect(dDispatcher.direct("Initial", "Login.aspx"))
            End If
            If ShowSessionTimeOutMsgBox(strURL) = True Then
                NetMsgbox("You session has expired, please login again.", "Login.aspx", MsgBoxStyle.Information)
            Else
                RedirectToParent()
            End If
        End If

        Dim env As String
        env = ConfigurationManager.AppSettings.Get("Env")
        If env = "SMB" Then
            lblTitle.Text = "Small Medium Business"
        ElseIf env = "Enterprise" Then
            lblTitle.Text = "Enterprise Business"
        End If

        PopulateLogo()

        Dim strMaintenance As String
        strMaintenance = ConfigurationManager.AppSettings.Get("Maintenance")

        If strMaintenance <> "" Then
            lblMsg2.Text = strMaintenance            
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

            txtPassword.MaxLength = Common.GetPasswordMaxLength(New EAD.DBCom)
            If Request.QueryString("user") <> "" And Request.QueryString("uid") <> "" And Request.QueryString("cid") <> "" Then
                GetLoginFromBilling()
            End If
        Else
            GetLogin()
        End If
        Me.hidBtnContinue.Attributes.Add("onClick", "return ShowMsg('" & Session("Curr") & "', '" & Session("AmtDue") & "', '" & Session("Day") & "', '" & Session("AccessStatus") & "'); ")
        'lnkBrowser.Attributes.Add("OnClick", "window.open('UpdateBrowser.htm',  '', 'resizable=no,scrollbars=yes,left=100,top=100,width=600,height=300,status=no,menubar=no');")
        'lnkBrowser.NavigateUrl = "#"
    End Sub

    Private Sub PreLogin(ByVal pUserDetails As User, ByVal pCoyDetails As Companies, Optional ByVal pStatusFrmBilling As String = "")
        'Zulham 05112018
        Dim objDB As New EAD.DBCom
        Session("UserId") = Common.Parse(Trim(txtUserID.Text))
        Session("UserName") = pUserDetails.Name
        Session("CompanyId") = Common.Parse(Trim(txtCompID.Text))
        Session("CompanyName") = pCoyDetails.GetCompanyName(Trim(txtCompID.Text))
        Session("Last_Login") = Common.FormatWheelDate(WheelDateFormat.LongDateTimeDay, pUserDetails.LastLogin)
        'Michelle (6/10/2010) - To get the Environment
        Session("Env") = ConfigurationManager.AppSettings.Get("Env")
        Session("FileSize") = ConfigurationManager.AppSettings.Get("FileSize")

        Dim StandAloneFlag As String
        StandAloneFlag = ConfigurationManager.AppSettings.Get("StandAlone")
        'Check Access from billing
        If StandAloneFlag = "Yes" Then
            Session("AccessStatus") = "Active"
        Else
            Dim objaccess As New CheckAccess
            Dim status, companytype As String

            'Michelle (20/3/2013) - Issue 1876
            'status = objaccess.getAccess(Common.Parse(Trim(txtCompID.Text)))
            'Michelle (21/5/2013) - Issue 1934
            Dim billingDB = ConfigurationManager.AppSettings.Get("BillingDB")
            If pStatusFrmBilling <> "" Then
                status = pStatusFrmBilling
            Else
                Dim strxml, amt_due, ostd_day, curr As String
                Dim doc As New Xml.XmlDocument
                strxml = objaccess.getAccess(Common.Parse(Trim(txtCompID.Text)))
                strxml = strxml.Replace("&", "&#38;")
                doc.LoadXml(strxml)
                status = doc.GetElementsByTagName("status").Item(0).InnerText
                'Zulham 05112018

                If Common.parseNull(status).Trim = "" Then
                    status = objDB.GetVal("SELECT STATUS 
                                            FROM " & billingDB & ".accs_mstrs 
                                            WHERE comp_id ='" & Session("CompanyId") & "'")
                End If
                curr = doc.GetElementsByTagName("curr").Item(0).InnerText
                amt_due = doc.GetElementsByTagName("amt_due").Item(0).InnerText
                ostd_day = doc.GetElementsByTagName("day").Item(0).InnerText
                If amt_due > 0 Then
                    Session("AmtDue") = FormatNumber(amt_due)
                    Session("Day") = ostd_day
                    Session("Curr") = curr
                End If
                'Me.hidBtnContinue.Attributes.Add("onClick", "return ShowMsg('s');")
            End If

            Select Case status
                Case 0
                    Session("AccessStatus") = "Suspend"
                Case 1
                    Session("AccessStatus") = "Active"
                Case 2
                    Session("AccessStatus") = "Limited"
            End Select
            companytype = objaccess.getCompanyType(Session("CompanyId"))
            'Zulham 05112018

            If Common.parseNull(companytype).Trim = "" Then
                companytype = objDB.GetVal("SELECT procure_type 
                                        FROM " & billingDB & ".accs_mstrs 
                                        INNER JOIN billing.hubs_screens ON accs_mstrs.procure_id = hubs_screens.procure_id 
                                        WHERE comp_id='" & Session("CompanyId") & "'")
            End If
            Session("CompanyType") = companytype
        End If



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
        vbs = vbs & vbLf & "parent.location=""login.aspx"""
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
    Sub PopulateLogo()
        Dim agoraLogo, toplogo, partnerLogo, ftnLogo, strateqLogo, bottomText, verisignLogo, dellLogo As String
        ftnLogo = "<img src=""" & dDispatcher.direct("Plugins/Images", "MyFairTradeNet_Login.gif") & """ alt="""" />"
        'Zulham 18102018 - PAMB
        strateqLogo = "<img src=""" & dDispatcher.direct("Plugins/Images", "4255.jpg") & """ alt="""" />"
        agoraLogo = "<img src=""" & dDispatcher.direct("Plugins/Images", "Agora.png") & """ alt="""" />"
        verisignLogo = "<img src=""" & dDispatcher.direct("Plugins/Images", "GlobalSignSealLogo.png") & """ alt="""" style=""width:130px;"" />"
        dellLogo = "<img src=""" & dDispatcher.direct("Plugins/Images", "Dell_Logo.jpg") & """ alt="""" />"
        Dim env, WebEntSaaS, WebSMBSaaS, WebBilling As String
        env = ConfigurationManager.AppSettings.Get("Env")
        WebEntSaaS = ConfigurationManager.AppSettings.Get("WebEnterpriseSaaS")
        WebSMBSaaS = ConfigurationManager.AppSettings.Get("WebSMBSaaS")
        WebBilling = ConfigurationManager.AppSettings.Get("WebBilling")
        If env = "FTN" Then
            toplogo = "<div style=""float:left;cursor:pointer; "" onclick=""window.open('http://www.myfairtradenet.com','FTN','menubar=yes,toolbar=yes,resizeable=yes,resizable=1, scrollbars=1, width=1000, height=500')"">" & ftnLogo & "</div>"
            partnerLogo = "<tr>"
            partnerLogo &= "<td style='width:33%; height: 100px;'>"
            partnerLogo &= "<div class=""download_ext"" onclick=""window.open('http://www.strateq-bizhub.com','FTN','menubar=yes,toolbar=yes,resizeable=yes,resizable=1, scrollbars=1, width=1000, height=500')"">" & strateqLogo & "</div>"
            partnerLogo &= "</td>"
            'partnerLogo &= "<td style='width:33%; height: 100px;'>"
            'partnerLogo &= "<div class=""download_ext"">" & dellLogo & "</div>"
            'partnerLogo &= "</td>"
            partnerLogo &= "<td style='width:33%; height: 100px;'>"
            'partnerLogo &= "<div class='download_ext' onclick=""window.open('https://sealinfo.verisign.com/splash?form_file=fdf/splash.fdf&dn=apps.strateq-bizhub.com&lang=en','FTN','menubar=yes,toolbar=yes,resizeable=yes,resizable=1, scrollbars=1, width=1000, height=500')"">" & verisignLogo & "</div>"
            partnerLogo &= "<div class='download_ext' onclick=""window.open('https://www.globalsign.com/en/ssl/secure-site-seal/','FTN','menubar=yes,toolbar=yes,resizeable=yes,resizable=1, scrollbars=1, width=1000, height=500')"">" & verisignLogo & "</div>"
            partnerLogo &= "</td>"
            partnerLogo &= "</tr>"
            Hyperlink1.NavigateUrl = "http://www.myfairtradenet.com/web/"
            Hyperlink1.Text = "myFairTradeNet"
            Hyperlink2.NavigateUrl = "http://www.myfairtradenet.com/web/contactus.html"
            bottomText = "myFairTradeNet 2012 All Rights Reserved"
            Hyperlink7.Visible = False
            Hyperlink8.Visible = False
            Hyperlink9.Visible = False
            span1.Visible = False
        Else
            'toplogo = "<div style=""float:left; "" onclick=""window.open('http://www.strateq-bizhub.com','FTN','menubar=yes,toolbar=yes,resizeable=yes,resizable=1, scrollbars=1, width=1000, height=500')"">" & agoraLogo & "</div>"
            toplogo = "<div style=""float:left;"">" & agoraLogo & "</div>"
            partnerLogo = "<tr>"
            partnerLogo &= "<td style='width:33%; height: 100px;'>"
            'Zulham 18102018 - PAMB
            partnerLogo &= "<div class=""download_ext"" "">" & strateqLogo & "</div>"
            partnerLogo &= "</td>"
            'partnerLogo &= "<td style='width:33%; height: 100px;'>"
            'partnerLogo &= "<div class='download_ext2'>" & dellLogo & "</div>"
            'partnerLogo &= "</td>"
            partnerLogo &= "<td style='width:33%; height: 100px;'>"
            'partnerLogo &= "<div class='download_ext' onclick=""window.open('https://sealinfo.verisign.com/splash?form_file=fdf/splash.fdf&dn=apps.strateq-bizhub.com&lang=en','FTN','menubar=yes,toolbar=yes,resizeable=yes,resizable=1, scrollbars=1, width=1000, height=500')"">" & verisignLogo & "</div>"
            partnerLogo &= "<div class='download_ext' onclick=""window.open('https://www.globalsign.com/en/ssl/secure-site-seal/','FTN','menubar=yes,toolbar=yes,resizeable=yes,resizable=1, scrollbars=1, width=1000, height=500')"">" & verisignLogo & "</div>"
            partnerLogo &= "</td>"
            partnerLogo &= "</tr>"
            Hyperlink1.NavigateUrl = "http://www.strateq-bizhub.com/web4/index.html"
            Hyperlink1.Text = "Strateq Biz-Hub"
            Hyperlink2.NavigateUrl = "http://www.strateq-bizhub.com/web4/contact_information.html"

            Hyperlink3.Visible = False
            Hyperlink4.Visible = False

            Hyperlink7.NavigateUrl = WebEntSaaS
            Hyperlink8.NavigateUrl = WebSMBSaaS
            Hyperlink9.NavigateUrl = WebBilling
            bottomText = "Strateq BusinessHub Sdn Bhd 2012 All Rights Reserved"
        End If
        Session("topLogo") = toplogo
        Session("partnerLogo") = partnerLogo
        Session("bottomText") = bottomText






    End Sub
End Class