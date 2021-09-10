Imports AgoraLegacy
Imports System.Web.HttpServerUtility
Imports SSO.Component
Imports System.IO
Imports System.Web.Mail

Imports System.Text.RegularExpressions
Public Class MassEmail
    Inherits System.Web.UI.Page
    Private strPageId As String
    Private strCallFrom As String
    Dim dDispatcher As New AgoraLegacy.dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents cmdSendMail As System.Web.UI.WebControls.Button
    Protected WithEvents optBuyers As System.Web.UI.WebControls.RadioButton
    Protected WithEvents optVendors As System.Web.UI.WebControls.RadioButton
    Protected WithEvents optUsers As System.Web.UI.WebControls.RadioButton
    Protected WithEvents txtFrom As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtTo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSubject As System.Web.UI.WebControls.TextBox
    Protected WithEvents optApprovingOfficer As System.Web.UI.WebControls.RadioButton
    Protected WithEvents optRoles As System.Web.UI.WebControls.RadioButton
    Protected WithEvents cboRoles As System.Web.UI.WebControls.DropDownList
    Protected WithEvents optOthers As System.Web.UI.WebControls.RadioButton
    Protected WithEvents trTo As System.Web.UI.HtmlControls.HtmlTableRow

    Protected WithEvents rfvFrom As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rfvSubject As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldsumm As System.Web.UI.WebControls.ValidationSummary

    Protected WithEvents rfvSendTo As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents revFrom As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents Requiredfieldvalidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents lstcontent As System.Web.UI.WebControls.ListBox
    Protected WithEvents txtRTB As System.Web.UI.WebControls.TextBox
    Protected WithEvents rfvContent As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents btnAddAttachment As System.Web.UI.WebControls.Button
    Protected WithEvents btnRemAttach As System.Web.UI.WebControls.Button
    Protected WithEvents txtFile As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents chkAttach As System.Web.UI.WebControls.CheckBoxList

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
        Dim strURL As String
        strPageId = Page.Request.QueryString("pageid")

        If System.Configuration.ConfigurationSettings.AppSettings("SSLHttp") Then
            strURL = "https://" & Request.ServerVariables("HTTP_Host") & "/"
        Else
            strURL = "http://" & Request.ServerVariables("HTTP_Host") & "/"
        End If

        'strURL &= Request.ApplicationPath & "/TimeOut.aspx?pageid=" & strPageId
        strURL &= Request.ApplicationPath & dDispatcher.direct("Initial", "/TimeOut.aspx", "pageid=" & strPageId)

        If CStr(Session("UserId")) = String.Empty Then
            Response.Redirect(strURL)
        End If
        strCallFrom = Request.ServerVariables("Path_Info") & "?" & Request.ServerVariables("QUERY_STRING")
        Response.AddHeader(" REFRESH", " " & CStr(CInt(Session.Timeout + 1) * 60) & ";url=" & strCallFrom)


        Dim objUser As New Users
        Dim strCSSPath As String

        'strCSSPath = Request.ApplicationPath & "/css/STYLES.CSS"
        strCSSPath = Request.ApplicationPath & dDispatcher.direct("Plugins/CSS", "STYLES.CSS")
        Response.Write("<link type='text/css' href='" & strCSSPath & "'>")
        If Me.optOthers.Checked = True Then
            rfvSendTo.Enabled = True
        Else
            rfvSendTo.Enabled = False
        End If


        If optRoles.Checked = True Then
            cboRoles.Enabled = True
        Else
            cboRoles.SelectedIndex = -1
            cboRoles.Enabled = False
            'cboRoles.SelectedItem.Text = "---Select---"

        End If



        If Not Page.IsPostBack Then
            Session("Attachment") = Nothing
            txtFrom.Text = ConfigurationSettings.AppSettings("HubAdminEmail")
            optBuyers.Checked = True
            txtTo.Enabled = False
            'Else
            '    If optBuyers.Checked = True Or optRoles.Checked = True Or optUsers.Checked = True Or optApprovingOfficer.Checked = True Or optVendors.Checked = True Then
            '        rfvSTo.Enabled = False
            '    Else
            '        rfvSTo.Enabled = True

            '    End If
            cboRoles.DataSource = objUser.SearchUserGroup("", "")
            cboRoles.DataTextField = "UGM_USRGRP_NAME"
            cboRoles.DataValueField = "UGM_USRGRP_ID"
            cboRoles.DataBind()

            'kk.remark.24/11/2004.Optional to add a text in the default ddl
            'If "UGM_USRGRP_NAME" <> "" Then
            '    Dim lsRole As New ListItem
            '    'lsRole.Text = "UGM_USRGRP_NAME"
            '    If "UGM_USRGRP_NAME" = "---Select---" Then
            '        lsRole.Value = ""
            '    End If
            '    cboRoles.Items.Insert(0, lsRole)

            'End If

            Dim lstItem As New ListItem
            lstItem.Value = ""
            lstItem.Text = "---Select---"
            cboRoles.Items.Insert(0, lstItem)

        End If

        With Me
            .cmdSendMail.Attributes.Add("onclick", "getHTML();")
            .optBuyers.Attributes.Add("onclick", "getHTML();")
            .optVendors.Attributes.Add("onclick", "getHTML();")
            .optUsers.Attributes.Add("onclick", "getHTML();")
            .optApprovingOfficer.Attributes.Add("onclick", "getHTML();")
            .optRoles.Attributes.Add("onclick", "getHTML();")
            .optOthers.Attributes.Add("onclick", "getHTML();")
            .btnAddAttachment.Attributes.Add("onclick", "getHTML();")
            .btnRemAttach.Attributes.Add("onclick", "getHTML();")
        End With

    End Sub

    'Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    MyBase.Page_Load(sender, e)
    '    Dim objUser As New Users
    '    Dim strCSSPath As String

    '    strCSSPath = Request.ApplicationPath & "/css/STYLES.CSS"
    '    Response.Write("<link type='text/css' href='" & strCSSPath & "'>")
    '    If Me.optOthers.Checked = True Then
    '        rfvSendTo.Enabled = True
    '    Else
    '        rfvSendTo.Enabled = False
    '    End If


    '    If optRoles.Checked = True Then
    '        cboRoles.Enabled = True
    '    Else
    '        cboRoles.SelectedIndex = -1
    '        cboRoles.Enabled = False
    '        'cboRoles.SelectedItem.Text = "---Select---"

    '    End If



    '    If Not Page.IsPostBack Then
    '        Session("Attachment") = Nothing
    '        txtFrom.Text = ConfigurationSettings.AppSettings("HubAdminEmail")
    '        optBuyers.Checked = True
    '        txtTo.Enabled = False
    '        'Else
    '        '    If optBuyers.Checked = True Or optRoles.Checked = True Or optUsers.Checked = True Or optApprovingOfficer.Checked = True Or optVendors.Checked = True Then
    '        '        rfvSTo.Enabled = False
    '        '    Else
    '        '        rfvSTo.Enabled = True

    '        '    End If
    '        cboRoles.DataSource = objUser.SearchUserGroup("", "")
    '        cboRoles.DataTextField = "UGM_USRGRP_NAME"
    '        cboRoles.DataValueField = "UGM_USRGRP_ID"
    '        cboRoles.DataBind()

    '        'kk.remark.24/11/2004.Optional to add a text in the default ddl
    '        'If "UGM_USRGRP_NAME" <> "" Then
    '        '    Dim lsRole As New ListItem
    '        '    'lsRole.Text = "UGM_USRGRP_NAME"
    '        '    If "UGM_USRGRP_NAME" = "---Select---" Then
    '        '        lsRole.Value = ""
    '        '    End If
    '        '    cboRoles.Items.Insert(0, lsRole)

    '        'End If

    '        Dim lstItem As New ListItem
    '        lstItem.Value = ""
    '        lstItem.Text = "---Select---"
    '        cboRoles.Items.Insert(0, lstItem)

    '    End If

    '    With Me
    '        .cmdSendMail.Attributes.Add("onclick", "getHTML();")
    '        .optBuyers.Attributes.Add("onclick", "getHTML();")
    '        .optVendors.Attributes.Add("onclick", "getHTML();")
    '        .optUsers.Attributes.Add("onclick", "getHTML();")
    '        .optApprovingOfficer.Attributes.Add("onclick", "getHTML();")
    '        .optRoles.Attributes.Add("onclick", "getHTML();")
    '        .optOthers.Attributes.Add("onclick", "getHTML();")
    '        .btnAddAttachment.Attributes.Add("onclick", "getHTML();")
    '        .btnRemAttach.Attributes.Add("onclick", "getHTML();")
    '    End With

    'End Sub

    Private Function validateEmailTo() As Boolean
        If txtTo.Text <> "" Then
            Dim strAry() As String
            Dim strAry2() As String
            Dim i, j As Integer
            strAry = txtTo.Text.Split(";")
            For i = 0 To strAry.Length - 1
                strAry2 = strAry(i).Split(",")
                For j = 0 To strAry2.Length - 1
                    If Not Regex.IsMatch(strAry2(j), "\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*") Then
                        validateEmailTo = False
                        Exit Function
                    End If
                Next
            Next
        End If
        validateEmailTo = True
    End Function

    Private Sub optVendors_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optVendors.CheckedChanged
        CheckChanged(Not optVendors.Checked)
    End Sub

    Private Sub cmdSendMail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSendMail.Click
        If Page.IsValid Then
            If validateEmailTo() Then
                lblMsg.Text = ""
                If txtFrom.Text <> "" And txtSubject.Text <> "" And txtRTB.Text <> "" Then
                    RoleSelected()
                End If
            Else
                lblMsg.Visible = True
                'lblMsg.Text = "fail"
                lblMsg.Text = "<ul type='disc'><li>" & "Invalid To.<ul type='disc'></ul></li></ul>"
                'strMsg1 = "<li>" & strMsg1 & "<ul type='disc'></ul></li>"
            End If
        Else
            If rfvSendTo.Enabled And txtTo.Text <> "" Then
                If Not validateEmailTo() Then
                    lblMsg.Visible = True
                    lblMsg.Text = "<ul type='disc'><li>" & "Invalid Email.<ul type='disc'></ul></li></ul>"
                    'Else
                    'lblMsg.Visible = False
                End If
            End If
            If Not txtTo.Text <> "" Then
                lblMsg.Visible = False
            End If
            'lblMsg.Visible = False
        End If
    End Sub

    Private Function CheckChanged(ByVal bln As Boolean)
        txtTo.Enabled = bln
        txtTo.Text = ""
        'If bln Then

        'End If
        'If optOthers.Checked Then
        '    txtTo.Enabled = bln
        'Else
        '    txtTo.Text = ""
        '    trTo.Visible = False
        'End If
    End Function

    'Public Sub RoleSelected(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Private Function RoleSelected(Optional ByVal pPage As Integer = -1) As String

        'If optOthers.Checked Then
        '    txtTo.Text = txtTo.Text
        'Else
        '    txtTo.Text = ""
        '    trTo.Visible = False
        'End If

        Dim objUser As New Users
        Dim ds As New DataSet
        Dim role As String
        Dim strFrom As String
        Dim strTo As String
        Dim strSubject As String
        Dim strContent As String
        Dim Attachment As New ArrayList

        strFrom = txtFrom.Text
        strTo = txtTo.Text
        strSubject = txtSubject.Text
        strContent = txtRTB.Text

        'strContent = Server.HtmlEncode(strContent)
        While (strContent.IndexOf(vbCrLf) > 0)
            strContent = strContent.Substring(0, strContent.IndexOf(vbCrLf)) & "<BR>" & strContent.Substring(strContent.IndexOf(vbCrLf) + 2, strContent.Length - strContent.IndexOf(vbCrLf) - 2)
        End While

        If Not Session("Attachment") Is Nothing Then
            Attachment = Session("Attachment")
        End If

        If optBuyers.Checked Then
            'viewstate("email") = objUser.SendMassMail(FixedRole.Buyer, strFrom, strTo, strSubject, strContent)
            'txtTo.Text = "s_meilai@yahoo.com"
            'rfvSTo.IsValid = False
            objUser.SendMassMail(FixedRole.Buyer, strFrom, strTo, strSubject, strContent, Attachment)

        ElseIf optVendors.Checked Then
            'rfvSTo.IsValid = False
            objUser.SendMassMail(FixedRole.Vendor, strFrom, strTo, strSubject, strContent, Attachment)

        ElseIf optUsers.Checked Then
            'rfvSTo.IsValid = False
            objUser.SendMassMail(FixedRole.All_Role, strFrom, strTo, strSubject, strContent, Attachment)

        ElseIf optApprovingOfficer.Checked Then
            'rfvSTo.IsValid = False
            objUser.SendMassMail(FixedRole.Approving_Officer, strFrom, strTo, strSubject, strContent, Attachment)
        ElseIf optRoles.Checked Then
            'rfvSTo.IsValid = False
            objUser.SendMassMail(FixedRole.Vendor_Administrator, strFrom, strTo, strSubject, strContent, Attachment)

        ElseIf optOthers.Checked Then
            'Dim objMM As New MailMessage
            'rfvSTo.IsValid = True
            'If txtSubject.Text <> "" Then
            'objUser.SendMassMail(FixedRole.Buyer, strFrom, strTo, strSubject, Server.HtmlEncode(strContent))
            Dim aa As String
            aa = objUser.SendMassMail(FixedRole.Buyer, strFrom, strTo, strSubject, strContent, Attachment)
            'Common.NetMsgbox(Me, "Mail has been sent successfully.", MsgBoxStyle.Information)
            'End If
            'objMM.To = strTo
            'objMM.From = strFrom
            'objMM.Priority = MailPriority.Normal
            'objMM.Subject = strSubject
            'objMM.BodyFormat = MailFormat.Html
            'objMM.Body = "<html><body>" & strContent & "</body></html>"

            'If strAttachment <> "" Then
            '    Dim sFilePath As String
            '    For Each sFilePath In strAttachment.Split(",")
            '        '//Use Virtual path, so this function can be called from anywhere
            '        'sFilePath = Current.Server.MapPath(Current.Request.ApplicationPath & "/Attach/" & sFileName)
            '        If File.Exists(sFilePath) Then
            '            Dim objMA As New MailAttachment(sFilePath)
            '            objMM.Attachments.Add(objMA)
            '        End If
            '    Next
            'End If

            'SmtpMail.SmtpServer = "10.20.30.18"
            'SmtpMail.Send(objMM)


        End If
        Common.NetMsgbox(Me, "Mail has been sent successfully.", MsgBoxStyle.Information)


    End Function

    Private Sub optApprovingOfficer_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optApprovingOfficer.CheckedChanged
        CheckChanged(Not optApprovingOfficer.Checked)
    End Sub

    Private Sub optBuyers_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optBuyers.CheckedChanged
        CheckChanged(Not optBuyers.Checked)
    End Sub

    Private Sub optOthers_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optOthers.CheckedChanged
        CheckChanged(optOthers.Checked)
        'Me.txtTo.Text = "patrick@10.20.30.18"
    End Sub

    Private Sub optRoles_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optRoles.CheckedChanged
        CheckChanged(Not optRoles.Checked)
    End Sub

    Private Sub optUsers_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optUsers.CheckedChanged
        CheckChanged(Not optUsers.Checked)
    End Sub

    Private Sub btnAddAttachment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddAttachment.Click
        Dim strFilePath As String
        Dim lstItem As New ListItem
        Dim Attachment As New ArrayList

        Try
            If Not Me.txtFile.PostedFile Is Nothing And Me.txtFile.PostedFile.ContentLength > 0 Then
                If Not Session("Attachment") Is Nothing Then
                    Attachment = Session("Attachment")
                End If
                Attachment.Add(Me.txtFile.PostedFile)
                strFilePath = Me.txtFile.Value
                lstItem.Text = Path.GetFileName(strFilePath)
                lstItem.Value = strFilePath
                Me.chkAttach.Items.Add(lstItem)
                Session("Attachment") = Attachment
            End If
        Catch ex As Exception
            Exit Sub
        End Try

    End Sub

    Private Sub btnRemAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemAttach.Click
        Dim lstItemColl As New ListItemCollection
        Dim lstItem As New ListItem
        Dim aryCollList As New System.Collections.ArrayList
        Dim Attachment As New ArrayList

        If Me.chkAttach.Items.Count > 0 Then

            If Not Session("Attachment") Is Nothing Then
                Attachment = Session("Attachment")
            End If

            For intCnt As Integer = 0 To Me.chkAttach.Items.Count - 1
                If Me.chkAttach.Items(intCnt).Selected Then
                    aryCollList.Add(Attachment(intCnt))
                    lstItemColl.Add(Me.chkAttach.Items(intCnt))
                End If
            Next

            For Each lstItem In lstItemColl
                Me.chkAttach.Items.Remove(lstItem)
            Next

            For intCnt As Integer = 0 To aryCollList.Count - 1
                Attachment.Remove(aryCollList(intcnt))
            Next

            Session("Attachment") = Attachment
        End If
    End Sub
End Class
