Imports AgoraLegacy
Imports System.Configuration
Imports AppGlobals
Imports SSO.Component

Namespace AgoraLegacy
    Public Class Email_Ext

        Public Function SendMailByDS(ByVal ds As DataSet, ByVal strBody As String, ByVal strSubject As String, ByVal strRole As String)
            Dim i As Integer
            Dim strCC, strTo, strContent As String
            Dim blnAttn As Boolean = False
            Dim objMail As New AppMail
            Dim strExtra As String = ""
            Dim strLine As String
            Dim strEmailType As String
            Dim objCommon As New Common
            Dim objUsers As New Users

            Dim dtAttn As New DataTable
            dtAttn.Columns.Add("UserId", Type.GetType("System.String"))
            dtAttn.Columns.Add("UserName", Type.GetType("System.String"))
            dtAttn.Columns.Add("Email", Type.GetType("System.String"))
            dtAttn.Columns.Add("CoyName", Type.GetType("System.String"))
            Dim dtr As DataRow

            If Not ds Is Nothing Then
                If ds.Tables.Count > 0 Then
                    If ds.Tables("COMPANY").Rows.Count > 0 Then
                        If ds.Tables("COMPANY").Rows(0)("CM_DELETED") <> "Y" And ds.Tables("COMPANY").Rows(0)("CM_STATUS") = "A" Then
                            For i = 0 To ds.Tables("USERTO").Rows.Count - 1
                                If Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_EMAIL")) <> "" And Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_STATUS")) = "A" Then
                                    blnAttn = True
                                    strTo = Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_EMAIL")) & "; "
                                    strContent = "Dear " & Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_USER_NAME")) & " (" & strRole & "), <BR>" & strBody

                                    objMail.MailTo = strTo
                                    If UCase(ds.Tables("COMPANY").Rows(0)("CM_COY_TYPE")) <> "BUYER" Then
                                        strCC = objUsers.getUserEmailCCList(ds.Tables("USERTO").Rows(i)("UM_COY_ID"), ds.Tables("USERTO").Rows(i)("UM_USER_ID"))

                                        If strCC <> "" Then
                                            objMail.MailCc = strCC
                                        End If
                                    End If

                                    objMail.Body = strContent & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen
                                    objMail.Subject = "Agora : " & strSubject
                                    objMail.SendMail()
                                Else
                                    dtr = dtAttn.NewRow()
                                    dtr("UserId") = Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_USER_ID"))
                                    dtr("UserName") = Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_USER_NAME"))
                                    dtr("Email") = Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_EMAIL"))
                                    dtr("CoyName") = Common.parseNull(ds.Tables("USERTO").Rows(i)("CM_COY_NAME"))
                                    dtAttn.Rows.Add(dtr)
                                End If
                            Next

                            If ds.Tables.Count = 3 And blnAttn Then
                                For i = 0 To ds.Tables("USERCC").Rows.Count - 1
                                    If Common.parseNull(ds.Tables("USERCC").Rows(i)("UM_EMAIL")) <> "" And Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_STATUS")) = "A" Then
                                        strCC &= Common.parseNull(ds.Tables("USERCC").Rows(i)("UM_EMAIL")) & "; "
                                        strContent = "Dear " & Common.parseNull(ds.Tables("USERCC").Rows(i)("UM_USER_NAME")) & ", <BR>" & strBody

                                        objMail.MailTo = strTo
                                        objMail.Body = strContent & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen
                                        objMail.Subject = "Agora : " & strSubject
                                        objMail.SendMail()
                                    Else
                                        dtr = dtAttn.NewRow()
                                        dtr("UserId") = Common.parseNull(ds.Tables("USERCC").Rows(i)("UM_USER_ID"))
                                        dtr("UserName") = Common.parseNull(ds.Tables("USERCC").Rows(i)("UM_USER_NAME"))
                                        dtr("Email") = Common.parseNull(ds.Tables("USERCC").Rows(i)("UM_EMAIL"))
                                        dtr("CoyName") = Common.parseNull(ds.Tables("USERCC").Rows(i)("CM_COY_NAME"))
                                        dtAttn.Rows.Add(dtr)
                                    End If
                                Next
                            End If
                        Else ' company not active
                            strEmailType = "4"
                            GoTo GetEmailContent
                        End If
                    End If ' ds.Tables("COMPANY").Rows.Count > 0
                End If ' ds.Tables.Count > 0

                If Not blnAttn Then
                    If dtAttn.Rows.Count = 0 Then
                        strEmailType = "3"
                    Else
                        strEmailType = "2"
                    End If
                End If

GetEmailContent:
                strLine = Strings.StrDup(objCommon.EmailCompGen.Length, ".")
                If strEmailType <> "1" Then
                    Select Case strEmailType
                        Case "2"
                            objMail.MailTo = ConfigurationSettings.AppSettings("EmailToHubAdmin")  ' EmailHub
                            strExtra = "Dear Hub Admin, <BR>"
                            For i = 0 To dtAttn.Rows.Count - 1
                                strExtra &= "User account for " & dtAttn.Rows(i)("UserId") & " (" & dtAttn.Rows(i)("UserId") & ", " & dtAttn.Rows(i)("Email") & ") of " & dtAttn.Rows(i)("CoyName") & " company has been deactivated. <BR>"
                            Next

                            objMail.Body = strExtra & "<P><U>Email Sent Un-Successfully:</U><BR><P>" & strLine & "<BR><P>" & strBody & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen & "<BR><P>" & strLine
                            'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                            '    objMail.Body = strExtra & "<P><U>Email Sent Un-Successfully:</U><BR><P>" & strLine & "<BR><P>" & strBody & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen & "<BR><P>" & strLine

                            'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                            '    objMail.Body = strExtra & "<P><U>Email Sent Un-Successfully:</U><BR><P>" & strLine & "<BR><P>" & strBody & "<P>" & objCommon.EmailFooterENT & objCommon.EmailCompGen & "<BR><P>" & strLine

                            'End If

                            objMail.Subject = "Agora : Deactivated User - " & strSubject
                            objMail.SendMail()
                        Case "3"
                            strExtra = "Dear Hub Admin, <BR>"
                            strExtra &= "User not found from " & ds.Tables("COMPANY").Rows(0)("CM_COY_NAME") & ". <BR>"
                            objMail.MailTo = ConfigurationSettings.AppSettings("EmailToHubAdmin") 'EmailHub 

                            objMail.Body = strExtra & "<P><U>Email Sent Un-Successfully:</U><BR><P>" & strLine & "<BR><P>" & strBody & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen & "<BR><P>" & strLine

                            objMail.Subject = "Agora : No User Exists - " & strSubject
                            objMail.SendMail()
                        Case "4"
                            strExtra = "Dear Hub Admin, <BR>"
                            strExtra &= "<P>The company " & ds.Tables("COMPANY").Rows(0)("CM_COY_NAME") & " has been deactived. <BR> "
                            objMail.MailTo = ConfigurationSettings.AppSettings("EmailToHubAdmin") ' EmailHub 

                            objMail.Body = strExtra & "<P><U>Email Sent Un-Successfully:</U><BR><P>" & strLine & "<BR><P>" & strBody & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen & "<BR><P>" & strLine

                            objMail.Subject = "Agora : Deactivated Company - " & strSubject
                            objMail.SendMail()
                    End Select
                End If
            End If ' ds is nothing
        End Function

        Public Function sendStkLevelPO(ByVal pEmailType As EmailType_Ext, ByVal strUserId As String, _
                                            ByVal strBCoyId As String, ByVal strSCoyId As String, _
                                            ByVal strDocNo As String, ByVal strDocNo2 As String, _
                                            Optional ByVal pPara1 As String = "", _
                                            Optional ByVal pPara2 As String = "", _
                                            Optional ByVal pPara3 As String = "")

            Dim ds As New DataSet
            Dim ds1 As New DataSet
            Dim strBody, strSubject As String
            Dim objUser As New User
            Dim objUsers As New Users
            Dim i, j As Integer
            Dim blnAttn As Boolean = False
            Dim objComp As New Company
            Dim objComps As New Companies
            Dim objMail As New AppMail
            Dim strExtra As String = ""
            Dim strRole As String
            Dim objCommon As New Common

            Select Case pEmailType
                Case EmailType_Ext.SafetyLevel ' SK to 2nd 
                    strSubject = "Safety Level Reached"
                    ds = objUsers.getUserEmailList(FixedRole.Purchasing_Officer, strBCoyId)
                    strRole = "Purchasing Officer"
                    strBody = "<P>Please be informed that safety level for item (" & pPara1 & ") is reached.<BR>"
                    strBody &= "<P>Thank you.<BR>"

                Case EmailType_Ext.MaxInventory ' Vendor to Buyer
                    strSubject = "Max Inventory Reached"
                    ds = objUsers.getUserEmailList(FixedRole.Purchasing_Officer, strBCoyId)
                    strRole = "Purchasing Officer"
                    strBody = "<P>Please be informed that max inventory for item (" & pPara1 & ") is reached.<BR>"
                    strBody &= "<P>Thank you.<BR>"

                Case EmailType_Ext.ReorderQuantityLevel ' Vendor to SK
                    strSubject = "Reorder Quantity Level Reached"
                    ds = objUsers.getUserEmailList(FixedRole.Purchasing_Officer, strBCoyId)
                    strRole = "Purchasing Officer"
                    strBody = "<P>Please be informed that reorder quantity level for item (" & pPara1 & ") is reached.<BR>"
                    strBody &= "<P>Thank you.<BR>"

            End Select

            If Not blnAttn Then  ' email already been sent above
                SendMailByDS(ds, strBody, strSubject, strRole)
            End If

            objUser = Nothing
            objUsers = Nothing
            objComp = Nothing
            objComps = Nothing

        End Function
    End Class
End Namespace

