Imports AgoraLegacy
Imports System.Text.RegularExpressions
Imports eProcure.Component


Public Class usUserFTN
    Inherits AgoraLegacy.AppBaseClass
    Dim objDb As New EAD.DBCom
    Dim strMode As String
    Dim lGrpIndex As Long
    Dim ctx As Web.HttpContext = Web.HttpContext.Current
    Protected WithEvents hidlistbox As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents hidrb As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents rbRole As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents chkMassAppr As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblAction As System.Web.UI.WebControls.Label
    Dim dDispatcher As New AgoraLegacy.dispatcher
    'Dim userEmailOri As String
    Dim blnEmailChange As Boolean = False

#Region " Object Declaration "
    Protected WithEvents txtName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtUser As System.Web.UI.WebControls.TextBox
    Protected WithEvents vName As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vUser As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vPassword As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents cklChemist As System.Web.UI.WebControls.CheckBoxList
    Protected WithEvents cklCategory As System.Web.UI.WebControls.CheckBoxList
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents txtStatus As System.Web.UI.WebControls.TextBox
    Protected WithEvents lnkUnlock As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Panel1 As System.Web.UI.WebControls.Panel
    Protected WithEvents hplAdd As System.Web.UI.WebControls.HyperLink
    Protected WithEvents LnkResetPwd As System.Web.UI.WebControls.LinkButton
    Protected WithEvents divtbl As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents cboCust As System.Web.UI.WebControls.DropDownList
    Protected WithEvents imgClientInfo As System.Web.UI.WebControls.Image
    Protected WithEvents lblClientID As System.Web.UI.WebControls.Label
    Protected WithEvents lblClientName As System.Web.UI.WebControls.Label
    Protected WithEvents lblContractPpl As System.Web.UI.WebControls.Label
    Protected WithEvents lblTel As System.Web.UI.WebControls.Label
    Protected WithEvents lblFaxNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblEmail As System.Web.UI.WebControls.Label
    Protected WithEvents lblAddress As System.Web.UI.WebControls.Label
    Protected WithEvents lblCustName As System.Web.UI.WebControls.Label
    Protected WithEvents txtEmail As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCompany As System.Web.UI.WebControls.TextBox
    Protected WithEvents rdAct As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rdDeAct As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chkAccLock As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    Protected WithEvents cmdModify As System.Web.UI.WebControls.Button
    Protected WithEvents Button2 As System.Web.UI.WebControls.Button
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdGeneratePwd As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents cboUserGrp As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtPhone As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFax As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblPwdExp As System.Web.UI.WebControls.Label
    Protected WithEvents lstUGAvail As System.Web.UI.WebControls.ListBox
    Protected WithEvents cmdLeft As System.Web.UI.WebControls.Button
    Protected WithEvents cmdRight As System.Web.UI.WebControls.Button
    Protected WithEvents lstUGSelected As System.Web.UI.WebControls.ListBox
    Protected WithEvents cboDeptName As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lnkSetDelAdd As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents txtDesignation As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAppLimit As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPOAppLimit As System.Web.UI.WebControls.TextBox
    Protected WithEvents vldUserGrp As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents chkMassApp As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lnkFinVwDept As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkMassApp As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents revUserId As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents Table2 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents trApprovalLimit As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lnkSetDelAddBuyer As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkSetBillAddBuyer As System.Web.UI.WebControls.LinkButton
    Protected WithEvents hidPageCount As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents txtInvAppLimit As System.Web.UI.WebControls.TextBox
    Protected WithEvents trInvoiceApprovalLimit As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trMassAppr As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trDept As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txtHP As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdadd As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents vldsummary As System.Web.UI.WebControls.ValidationSummary



#End Region

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
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        lnkSetBillAddBuyer.Enabled = False
        lnkSetDelAddBuyer.Enabled = False
        lnkSetDelAdd.Enabled = False
        lnkMassApp.Enabled = False
        lnkFinVwDept.Enabled = False
        cmdSave.Enabled = False
        cmdGeneratePwd.Enabled = False
        cmdDelete.Enabled = False
        Dim alButtonList As ArrayList
        If ViewState("Mode") = "modify" Then
            alButtonList = New ArrayList
            alButtonList.Add(cmdSave)
            alButtonList.Add(cmdGeneratePwd)
            alButtonList.Add(lnkSetBillAddBuyer)
            alButtonList.Add(lnkSetDelAddBuyer)
            alButtonList.Add(lnkSetDelAdd)
            alButtonList.Add(lnkMassApp)
            alButtonList.Add(lnkFinVwDept)
            htPageAccess.Add("update", alButtonList)
            alButtonList = New ArrayList
            alButtonList.Add(cmdDelete)
            htPageAccess.Add("delete", alButtonList)
        ElseIf ViewState("Mode") = "add" Then
            alButtonList = New ArrayList
            alButtonList.Add(cmdSave)
            alButtonList.Add(lnkSetBillAddBuyer)
            alButtonList.Add(lnkSetDelAddBuyer)
            alButtonList.Add(lnkSetDelAdd)
            alButtonList.Add(lnkMassApp)
            alButtonList.Add(lnkFinVwDept)
            htPageAccess.Add("add", alButtonList)
        End If
        CheckButtonAccess()
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)

        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        If Not Page.IsPostBack Then
            isNotPostBack()
            GenerateTab()
        End If
        cmdDelete.Attributes.Add("onclick", "return confirm('" & MsgForDeleteButton & "');")
        lnkBack.NavigateUrl = dDispatcher.direct("Users", "usSearchUser.aspx", "pageid=" & strPageId)
    End Sub

    Private Sub isNotPostBack()
        lblMsg.Text = ""
        'viewState("Mode") = Me.Request.QueryString("mode")
        If ViewState("Mode") Is Nothing Then ViewState("Mode") = Me.Request.QueryString("mode")
        ViewState("UserId") = Me.Request.QueryString("userid")

        Dim objUser As New Users
        Dim objUserDetails As New User
        Dim objCom As New Companies

        Dim companytype As String = objCom.GetCompanyType

        If companytype.ToUpper = "VENDOR" Then
            txtAppLimit.Text = 0
            txtInvAppLimit.Text = 0
            trApprovalLimit.Visible = False
            trPOApprovalLimit.Visible = False
            trInvoiceApprovalLimit.Visible = False
            trDept.Visible = False
            ViewState("side") = "V"
        ElseIf companytype.ToUpper = "BUYER" Or companytype.ToUpper = "BOTH" Then
            'trApprovalLimit.Visible = True
            'trInvoiceApprovalLimit.Visible = True
            Dim intDept As Integer
            Dim objDBAccess As New EAD.DBCom
            intDept = objDBAccess.GetCount("company_dept_mstr", " where cdm_coy_id = '" & Session("CompanyID") & "' and cdm_deleted = 'N'")
            If intDept = 0 Then trDept.Visible = False
            ViewState("side") = "B"
            Me.lblAction.Text = "<b>=></b> Step 1: Create, modify or delete User Account.<br />Step 2: Assign Delivery Address to selected User Account.<br>Step 3: Assign Billing Address to selected User Account.<br>Step 4: Assign Commodity Type to selected User Account."
        End If

        If ViewState("Mode") = "add" Then
            clearTextBox()
            txtAppLimit.Text = 0
            txtPOAppLimit.Text = 0
            txtInvAppLimit.Text = 0
            lblHeader.Text = "Add User Account"
            txtUser.Enabled = True
            cmdReset.Value = "Clear"
            cmdDelete.Visible = False
            cmdGeneratePwd.Visible = False
            chkAccLock.Enabled = False
            cmdReset.Visible = False
            cmdadd.Visible = False
        ElseIf ViewState("Mode") = "modify" Then
            lblHeader.Text = "Modify User Account"
            cmdReset.Value = "Reset"
            txtUser.Text = ViewState("UserId")
            txtUser.Enabled = False
            objUserDetails = objUser.GetUserDetails(ViewState("UserId"), Session("CompanyId"))
            cmdadd.Visible = True
        End If
        Populate(objUserDetails)
        'If Session("Env") = "FTN" Then
        '    Me.hidlistbox.Style("display") = "none"
        '    If ViewState("side") = "B" Then
        '        Me.hidrb.Style("display") = ""
        '        If ViewState("Mode") = "add" Then ShowLink("FTN (PO),FTN (FO)")
        '    Else
        '        Me.hidrb.Style("display") = "none"
        '    End If
        'Else
        '    Me.hidlistbox.Style("display") = ""
        '    Me.hidrb.Style("display") = "none"
        'End If
        Me.hidlistbox.Style("display") = "none"
        If ViewState("side") = "B" Then
            Me.hidrb.Style("display") = ""
            If ViewState("Mode") = "add" Then ShowLink("FTN (PO),FTN (FO)")
        Else
            Me.hidrb.Style("display") = "none"
        End If
    End Sub

    Private Sub clearTextBox()
        Me.txtAppLimit.Text = ""
        Me.txtPOAppLimit.Text = ""
        Me.txtInvAppLimit.Text = ""
        Me.txtDesignation.Text = ""
        Me.txtEmail.Text = ""
        Session("userEmailOri") = ""
        Me.txtFax.Text = ""
        Me.txtName.Text = ""
        Me.txtPhone.Text = ""
        Me.txtUser.Text = ""

        Me.lstUGSelected.Items.Clear()
        Me.rdAct.Checked = True
        Me.rdDeAct.Checked = False
        Me.chkAccLock.Checked = False

        Me.lnkFinVwDept.Visible = False
        Me.lnkMassApp.Visible = False
        Me.lnkSetBillAddBuyer.Visible = False
        Me.lnkSetDelAdd.Visible = False
        Me.lnkSetDelAddBuyer.Visible = False
        Me.chkMassAppr.Checked = False
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim objUser As New Users
        Dim objcomp As New Companies
        Dim objUserDetail As New User
        Dim objPwd As New RandomPassword
        Dim objMail As New Email
        Dim strErrMsg As String
        Dim i As Integer
        Dim strTmpGrp As String

        objUserDetail = GetInput()
        'If Session("Env") <> "FTN" Then
        '    Dim spUsrGrp As Array
        '    spUsrGrp = Split(objUserDetail.UserGroup, ",")

        '    For i = 0 To spUsrGrp.Length - 1
        '        strTmpGrp = objUser.GetFixedRole(spUsrGrp(i))
        '        If strTmpGrp = "Purchasing Officer" Then
        '            objUserDetail.strFixedRole = strTmpGrp
        '        End If
        '    Next
        'End If

        objUserDetail.Password = objPwd.GenerateAutoPwd()
        objUserDetail.NextExpiredDt = objUser.GetPwdExpireDt(True)

        If ValidateFields(strErrMsg) Then
            If Page.IsValid Then
                If ViewState("Mode") = "add" Then
                    If objUser.AddUser(objUserDetail, False, EnumAppPackage.eProcure.ToString) Then
                        objMail.sendNotification(EmailType.NewUserAccount, objUserDetail.UserID, objUserDetail.CompanyID, "", "", "", objUserDetail.Password)
                    End If
                    Common.NetMsgbox(Me, objUser.Message, MsgBoxStyle.Information)
                    If objUser.Message = "Record saved." Then
                        ViewState("Mode") = "modify"
                        txtUser.Enabled = False
                        cmdadd.Visible = True
                    End If
                    lblMsg.Visible = False
                    'ViewState("Mode") = "modify"
                    'txtUser.Enabled = False
                ElseIf ViewState("Mode") = "modify" Then
                    If txtEmail.Text <> Session("userEmailOri") Then
                        blnEmailChange = True
                    End If

                    If objUser.UpdateUser(objUserDetail, False, EnumAppPackage.eProcure.ToString, False, blnEmailChange) Then

                    End If
                    Session("userEmailOri") = txtEmail.Text
                    Common.NetMsgbox(Me, objUser.Message, MsgBoxStyle.Information)
                    lblMsg.Visible = False
                End If

                If objUser.IsRoleSelected(objUserDetail.UserGroup, FixedRole.Store_Keeper) And Not objUser.IsRoleSelected(objUserDetail.UserGroup, FixedRole.Second_Level_Receiver) Then
                    objcomp.DelUsrLocation(ViewState("UserId"), 2)
                End If

                If Not objUser.IsRoleSelected(objUserDetail.UserGroup, FixedRole.Store_Keeper) And objUser.IsRoleSelected(objUserDetail.UserGroup, FixedRole.Second_Level_Receiver) Then
                    objcomp.DelUsrLocation(ViewState("UserId"), 1)
                End If

                If lnkMassApp.Visible = False Then
                    If objUser.IsRoleSelected(objUserDetail.UserGroup, FixedRole.Approving_Officer) Then
                        objUser.UpdateMassApproval(ViewState("UserId"), False)
                    ElseIf objUser.IsRoleSelected(objUserDetail.UserGroup, FixedRole.Finance_Officer) Then
                        objUser.UpdateInvoiceMassApproval(ViewState("UserId"), False)
                    End If
                End If

                If lnkFinVwDept.Visible = False Then
                    objcomp.DelFinDeptViewing(ViewState("UserId"))
                    objcomp.DelFinDeptViewing(ViewState("UserId"), "0", True)
                End If
            Else
                lblMsg.Visible = True
                lblMsg.Text = strErrMsg
            End If
        Else
            lblMsg.Visible = True
            lblMsg.Text = strErrMsg
        End If
    End Sub

    Private Sub ShowLink(ByVal pUSerGroup As String)
        Dim objUser As New Users
        Dim objComp As New Companies

        If objUser.IsRoleSelected(pUSerGroup, FixedRole.Store_Keeper) Or objUser.IsRoleSelected(pUSerGroup, FixedRole.Second_Level_Receiver) Then
            lnkSetDelAdd.Visible = True
            If objUser.IsRoleSelected(pUSerGroup, FixedRole.Store_Keeper) And objUser.IsRoleSelected(pUSerGroup, FixedRole.Second_Level_Receiver) Then
                lnkSetDelAdd.Text = "[ Set Delivery address - Store Keeper/2nd Level Receiving ]"
            ElseIf objUser.IsRoleSelected(pUSerGroup, FixedRole.Store_Keeper) Then
                lnkSetDelAdd.Text = "[ Set Delivery address - Store Keeper ]"
            ElseIf objUser.IsRoleSelected(pUSerGroup, FixedRole.Second_Level_Receiver) Then
                lnkSetDelAdd.Text = "[ Set Delivery address - 2nd Level Receiving ]"
            End If
        Else
            lnkSetDelAdd.Visible = False
        End If

        If objUser.IsRoleSelected(pUSerGroup, FixedRole.Buyer) Or objUser.IsRoleSelected(pUSerGroup, FixedRole.Purchasing_Officer) Or objUser.IsRoleSelected(pUSerGroup, FixedRole.Purchasing_Manager) Then
            Dim i1, i2, i3, i4 As Integer
            i1 = IIf(objUser.IsRoleSelected(pUSerGroup, FixedRole.Buyer), 1, 0)
            i2 = IIf(objUser.IsRoleSelected(pUSerGroup, FixedRole.Purchasing_Officer), 2, 0)
            i3 = IIf(objUser.IsRoleSelected(pUSerGroup, FixedRole.Purchasing_Manager), 4, 0)
            i4 = i1 + i2 + i3

            lnkSetDelAddBuyer.Visible = True
            lnkSetBillAddBuyer.Visible = True

            Select Case i4
                Case 1
                    lnkSetDelAddBuyer.Text = "[ Set Delivery address - Buyer ]"
                    lnkSetBillAddBuyer.Text = "[ Set Billing address - Buyer ]"
                Case 2
                    lnkSetDelAddBuyer.Text = "[ Set Delivery address - Purchasing Officer ]"
                    lnkSetBillAddBuyer.Text = "[ Set Billing address - Purchasing Officer ]"
                Case 3
                    lnkSetDelAddBuyer.Text = "[ Set Delivery address - Buyer/Purchasing Officer ]"
                    lnkSetBillAddBuyer.Text = "[ Set Billing address - Buyer/Purchasing Officer ]"
                Case 4
                    lnkSetDelAddBuyer.Text = "[ Set Delivery address - Purchasing Manager ]"
                    lnkSetBillAddBuyer.Text = "[ Set Billing address - Purchasing Manager ]"
                Case 5
                    lnkSetDelAddBuyer.Text = "[ Set Delivery address - Buyer/Purchasing Manager ]"
                    lnkSetBillAddBuyer.Text = "[ Set Billing address - Buyer/Purchasing Manager ]"
                Case 6
                    lnkSetDelAddBuyer.Text = "[ Set Delivery address - Purchasing Officer/Purchasing Manager ]"
                    lnkSetBillAddBuyer.Text = "[ Set Billing address - Purchasing Officer/Purchasing Manager ]"
                Case 7
                    lnkSetDelAddBuyer.Text = "[ Set Delivery address - Buyer/Purchasing Officer/Purchasing Manager ]"
                    lnkSetBillAddBuyer.Text = "[ Set Billing address - Buyer/Purchasing Officer/Purchasing Manager ]"
            End Select
        Else
            lnkSetDelAddBuyer.Visible = False
            lnkSetBillAddBuyer.Visible = False
        End If

        If (objUser.IsRoleSelected(pUSerGroup, FixedRole.Finance_Manager) Or objUser.IsRoleSelected(pUSerGroup, FixedRole.Finance_Officer)) And objComp.GetFinMode(False) = "Y" Then
            lnkFinVwDept.Visible = True
        Else
            lnkFinVwDept.Visible = False
        End If

        If objUser.IsRoleSelected(pUSerGroup, FixedRole.Finance_Officer) Then
            txtInvAppLimit.Enabled = True
        Else
            txtInvAppLimit.Text = 0
            txtInvAppLimit.Enabled = False
        End If

        'If Session("Env") <> "FTN" Then
        '    If objUser.IsRoleSelected(pUSerGroup, FixedRole.Approving_Officer) Or objUser.IsRoleSelected(pUSerGroup, FixedRole.Finance_Officer) Then
        '        lnkMassApp.Visible = True
        '    Else
        '        lnkMassApp.Visible = False
        '    End If
        'Else
        '    If objUser.IsRoleSelected(pUSerGroup, FixedRole.Approving_Officer) Then
        '        lnkMassApp.Visible = True
        '    Else
        '        lnkMassApp.Visible = False
        '    End If
        'End If
        If objUser.IsRoleSelected(pUSerGroup, FixedRole.Approving_Officer) Then
            lnkMassApp.Visible = True
        Else
            lnkMassApp.Visible = False
        End If

    End Sub

    Private Sub cmdGeneratePwd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGeneratePwd.Click
        Dim objPwd As New RandomPassword
        Dim objMail As New Email
        Dim objUser As New Users
        Dim objUserDetail As New User
        Dim strNewPwd As String

        strNewPwd = objPwd.GenerateAutoPwd()
        objUserDetail = objUser.GetUserDetails(viewstate("UserId"), Session("CompanyId"))

        'User temporary password from Login policy
        If objUser.UpdatePassword(viewstate("UserId"), strNewPwd, Session("CompanyId"), True) Then
            objMail.sendNotification(EmailType.ChangePwd, viewstate("UserId"), Session("CompanyID"), "", "", "", strNewPwd)
            'SendMail(objUserDetail.Email, GetMailFormat(objUserDetail.UserID, strNewPwd, objUser.GetPwdExpireDt(True)), "New Generated Password")
            lblPwdExp.Text = objUser.GetPwdExpireDt(True)
            Common.NetMsgbox(Me, "Password generated.", MsgBoxStyle.Information)
        End If
    End Sub

    Private Function GetInput() As User
        Dim objUserDetail As New User

        objUserDetail.UserID = txtUser.Text
        objUserDetail.Name = txtName.Text
        objUserDetail.DeptID = cboDeptName.SelectedValue

        'If Session("Env") <> "FTN" Then
        '    objUserDetail.UserGroup = Lst2String(lstUGSelected, False, False)
        'Else
        '    If ViewState("side") = "B" Then
        '        Select Case rbRole.SelectedItem.Value
        '            Case "BUYER" 'Buyer will have the buyer roles - Added by Joon on 04 Jul 2011
        '                objUserDetail.UserGroup = "FTN (Buyer)"
        '                objUserDetail.strFixedRole = "Buyer"
        '                trMassAppr.Visible = False
        '                chkMassAppr.Checked = False
        '            Case "PO" 'Purchasing Officer will have the buyer & FO roles
        '                objUserDetail.UserGroup = "FTN (PO),FTN (FO)"
        '                objUserDetail.strFixedRole = "Purchasing Officer"
        '                trMassAppr.Visible = False
        '                chkMassAppr.Checked = False
        '            Case "PM" 'Purchasing Manager will have the PM & AO roles
        '                'Michelle - 12/9/2011 (826) Allow FTN (PM) to have Report Admin roles
        '                objUserDetail.UserGroup = "FTN (AO),FTN (PM),FTN (RA)"
        '                'objUserDetail.UserGroup = "FTN (AO),FTN (PM)"
        '                trMassAppr.Visible = True
        '            Case "AO" 'Approval Officer will have the AO, Buyer Admin, FM & Report Admin roles
        '                objUserDetail.UserGroup = "FTN (AO),FTN (IO),FTN (Buyer Admin),FTN (FM),FTN (RA)"
        '                trMassAppr.Visible = True
        '            Case "SK" 'Storekeeper will only have the SK roles
        '                objUserDetail.UserGroup = "FTN (SK),FTN (QC)"
        '                trMassAppr.Visible = False
        '                chkMassAppr.Checked = False
        '        End Select
        '    Else
        '        objUserDetail.UserGroup = "FTN (V),FTN(VA)"
        '    End If
        'End If

        If ViewState("side") = "B" Then
            Select Case rbRole.SelectedItem.Value
                Case "BUYER" 'Buyer will have the buyer roles - Added by Joon on 04 Jul 2011
                    objUserDetail.UserGroup = "FTN (Buyer)"
                    objUserDetail.strFixedRole = "Buyer"
                    trMassAppr.Visible = False
                    chkMassAppr.Checked = False
                Case "PO" 'Purchasing Officer will have the buyer & FO roles
                    objUserDetail.UserGroup = "FTN (PO),FTN (FO)"
                    objUserDetail.strFixedRole = "Purchasing Officer"
                    trMassAppr.Visible = False
                    chkMassAppr.Checked = False
                Case "PM" 'Purchasing Manager will have the PM & AO roles
                    'Michelle - 12/9/2011 (826) Allow FTN (PM) to have Report Admin roles
                    objUserDetail.UserGroup = "FTN (AO),FTN (PM),FTN (RA)"
                    'objUserDetail.UserGroup = "FTN (AO),FTN (PM)"
                    trMassAppr.Visible = True
                Case "AO" 'Approval Officer will have the AO, Buyer Admin, FM & Report Admin roles
                    objUserDetail.UserGroup = "FTN (AO),FTN (IO),FTN (Buyer Admin),FTN (FM),FTN (RA)"
                    trMassAppr.Visible = True
                Case "SK" 'Storekeeper will only have the SK roles
                    objUserDetail.UserGroup = "FTN (SK),FTN (QC)"
                    trMassAppr.Visible = False
                    chkMassAppr.Checked = False
            End Select
        Else
            objUserDetail.UserGroup = "FTN (V),FTN(VA)"
        End If

        objUserDetail.Email = txtEmail.Text
        objUserDetail.PhoneNo = txtPhone.Text
        objUserDetail.FaxNo = txtFax.Text
        objUserDetail.CompanyID = Session("CompanyId")
        objUserDetail.Designation = txtDesignation.Text
        objUserDetail.ApproveLimit = txtAppLimit.Text
        objUserDetail.POApproveLimit = txtPOAppLimit.Text
        objUserDetail.InvoiceApproveLimit = txtInvAppLimit.Text
        objUserDetail.NextExpiredDt = lblPwdExp.Text
        objUserDetail.PageCount = hidPageCount.Value

        If rdAct.Checked = True Then
            objUserDetail.Status = "A"
        Else
            objUserDetail.Status = "I"
        End If
        If chkAccLock.Checked = True Then
            objUserDetail.DeleteInd = "S"
        Else
            objUserDetail.DeleteInd = "N"
        End If

        If chkMassAppr.Checked Then
            objUserDetail.MassApp = "Y"
        Else
            objUserDetail.MassApp = "N"
        End If

        GetInput = objUserDetail

    End Function

    Private Sub cmdLeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLeft.Click
        Dim item As ListItem
        Dim tempItem() As ListItem
        Dim i As Integer
        Dim objUSer As New Users

        For Each item In lstUGSelected.Items
            If item.Selected = True Then
                lstUGAvail.Items.Insert(0, item)
                ReDim Preserve tempItem(i)
                tempItem(i) = item
                i = i + 1
            End If
        Next

        If lstUGSelected.SelectedIndex <> -1 Then
            For i = 0 To tempItem.Length - 1
                lstUGSelected.Items.Remove(tempItem(i))
            Next
        End If

        Dim objUserDetails As New User
        objUserDetails = GetInput()
        ShowLink(objUserDetails.UserGroup)
        Common.SortListControl(lstUGAvail, True)
    End Sub

    Private Sub cmdRight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRight.Click
        Dim item As ListItem
        Dim tempItem() As ListItem
        Dim objUser As Users
        Dim i As Integer

        For Each item In lstUGAvail.Items
            If item.Selected = True Then
                lstUGSelected.Items.Insert(0, item)
                ReDim Preserve tempItem(i)
                tempItem(i) = item
                i = i + 1
            End If
        Next

        If lstUGAvail.SelectedIndex <> -1 Then
            For i = 0 To tempItem.Length - 1
                lstUGAvail.Items.Remove(tempItem(i))
            Next
        End If

        Dim objUserDetails As New User
        objUserDetails = GetInput()
        ShowLink(objUserDetails.UserGroup)
        Common.SortListControl(lstUGSelected, True)
    End Sub

    Public Sub Populate(ByVal pUser As User)
        Dim sWhere As String
        Dim objComp As New Companies
        Dim objCompDetails As New Company
        Dim objUser As New Users
        Dim dv As DataView

        objCompDetails = objComp.GetCompanyDetails(Session("CompanyId"))

        If viewstate("Mode") = "add" Then
            lblHeader.Text = "Add User Account"
            hidPageCount.Value = "10"

            If objCompDetails.CoyType = "BOTH" Then
                sWhere = " UGM_TYPE IN ('VENDOR','BUYER')"
            ElseIf objCompDetails.CoyType = "VENDOR" Then
                sWhere = " UGM_TYPE='VENDOR'"
            ElseIf objCompDetails.CoyType = "BUYER" Then
                sWhere = " UGM_TYPE='BUYER'"
            ElseIf objCompDetails.CoyType = "HUB" Then
                sWhere = " UGM_TYPE='HUB'"
            End If
            sWhere &= " AND UGM_DELETED<>'Y' AND UGM_FIXED_ROLE <> 'Super Admin' "

            Common.FillDefault(lstUGAvail, "USER_GROUP_MSTR", "UGM_USRGRP_NAME", "UGM_USRGRP_ID", sWhere)
            Common.SortListControl(lstUGAvail, True)
            cboDeptName.Items.Clear()
            Common.FillDefault(cboDeptName, "COMPANY_DEPT_MSTR", "CDM_DEPT_NAME", "CDM_DEPT_CODE", "---Select---", " CDM_DELETED<>'Y' AND CDM_COY_ID='" & Session("CompanyId") & "'")
            lblPwdExp.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, objUser.GetPwdExpireDt(True))

        ElseIf ViewState("Mode") = "modify" Then
            lblHeader.Text = "Modify User Account"

            txtUser.Text = pUser.UserID
            txtName.Text = pUser.Name
            txtEmail.Text = pUser.Email
            Session("userEmailOri") = pUser.Email
            txtPhone.Text = pUser.PhoneNo
            txtFax.Text = pUser.FaxNo
            txtDesignation.Text = pUser.Designation
            txtAppLimit.Text = pUser.ApproveLimit
            txtPOAppLimit.Text = pUser.POApproveLimit
            txtInvAppLimit.Text = pUser.InvoiceApproveLimit
            lblPwdExp.Text = pUser.NextExpiredDt
            hidPageCount.Value = pUser.PageCount

            If pUser.MassApp = "Y" Then
                chkMassAppr.Checked = True
            Else
                chkMassAppr.Checked = False
            End If

            'Bind and selected
            cboDeptName.Items.Clear()
            Common.FillDefault(cboDeptName, "COMPANY_DEPT_MSTR", "CDM_DEPT_NAME", "CDM_DEPT_CODE", "---Select---", "CDM_DELETED='N' AND CDM_COY_ID='" & Session("CompanyId") & "'")
            Common.SelDdl(pUser.DeptID, cboDeptName, True, True)

            'If Session("Env") <> "FTN" Then
            '    Common.FillLst(lstUGSelected, "UGM_USRGRP_NAME", "UGM_USRGRP_ID", objUser.GetUserGroup(pUser.UserID, objCompDetails.CoyType, False))
            '    Common.SortListControl(lstUGSelected, True)
            '    Common.FillLst(lstUGAvail, "UGM_USRGRP_NAME", "UGM_USRGRP_ID", objUser.GetUserGroup(pUser.UserID, objCompDetails.CoyType, False, True))
            '    Common.SortListControl(lstUGAvail, True)
            'Else
            '    dv = objUser.GetUserGroup(pUser.UserID, objCompDetails.CoyType, False)
            '    trMassAppr.Visible = False
            '    If dv.Count() > 0 Then
            '        Dim i As Integer
            '        For i = 0 To dv.Count() - 1
            '            If dv.Item(i).Item(1) = "FTN (Buyer)" Then 'Added by Joon on 04 Jul 2011
            '                rbRole.SelectedValue = "BUYER"
            '                Exit For
            '            ElseIf dv.Item(i).Item(1) = "FTN (PO)" Then
            '                rbRole.SelectedValue = "PO"
            '                Exit For
            '            ElseIf dv.Item(i).Item(1) = "FTN (PM)" Then
            '                rbRole.SelectedValue = "PM"
            '                trMassAppr.Visible = True
            '                Exit For
            '            ElseIf dv.Item(i).Item(1) = "FTN (AO)" Then
            '                rbRole.SelectedValue = "AO"
            '                trMassAppr.Visible = True
            '                Exit For
            '            ElseIf dv.Item(i).Item(1) = "FTN (SK)" Then
            '                rbRole.SelectedValue = "SK"
            '                Exit For

            '            End If
            '        Next
            '    End If
            'End If
            dv = objUser.GetUserGroup(pUser.UserID, objCompDetails.CoyType, False, , , True)
            trMassAppr.Visible = False
            If dv.Count() > 0 Then
                Dim i As Integer
                For i = 0 To dv.Count() - 1
                    If dv.Item(i).Item(1) = "FTN (Buyer)" Then 'Added by Joon on 04 Jul 2011
                        rbRole.SelectedValue = "BUYER"
                        Exit For
                    ElseIf dv.Item(i).Item(1) = "FTN (PO)" Then
                        rbRole.SelectedValue = "PO"
                        Exit For
                    ElseIf dv.Item(i).Item(1) = "FTN (PM)" Then
                        rbRole.SelectedValue = "PM"
                        trMassAppr.Visible = True
                        Exit For
                    ElseIf dv.Item(i).Item(1) = "FTN (AO)" Then
                        rbRole.SelectedValue = "AO"
                        trMassAppr.Visible = True
                        Exit For
                    ElseIf dv.Item(i).Item(1) = "FTN (SK)" Then
                        rbRole.SelectedValue = "SK"
                        Exit For

                    End If
                Next
            End If
            ShowLink(pUser.UserGroup)

            Dim objUserRole As New UserRoles
            Dim strRole As String = objUserRole.get_UserRole
            Dim strFixedRole As String = objUserRole.get_UserFixedRole

            'If UCase(strRole) <> FixedRole.Admin.ToString.ToUpper Then
            If UCase(strFixedRole) <> FixedRole.Super_Admin.ToString.Replace("_", " ").ToUpper Then
                If objUser.IsRoleSelected(pUser.UserGroup, FixedRole.Buyer_Administrator) Or _
                            objUser.IsRoleSelected(pUser.UserGroup, FixedRole.Vendor_Administrator) Then
                    lstUGAvail.Enabled = False
                    lstUGSelected.Enabled = False
                    cmdRight.Enabled = False
                    cmdLeft.Enabled = False
                End If
            End If

            If pUser.Status = "A" Then
                rdAct.Checked = True
            ElseIf pUser.Status = "I" Then  'Delete_Ind, Check for company active status
                rdDeAct.Checked = True
            End If
            chkAccLock.Checked = IIf(pUser.DeleteInd = "S", True, False)
        End If
    End Sub

    Private Sub SetDDL(ByRef pCombo As DropDownList, ByVal pText As String)
        Dim item As ListItem
        For Each item In pCombo.Items
            If item.Value = pText Then
                item.Selected = True
                Exit For
            End If
        Next
    End Sub

    Private Function ValidateFields(ByRef psErrMsg As String) As Boolean

        Dim objUsers As New Users
        If txtUser.Text = "" Then
            psErrMsg &= "<LI type=square>User Id is required field.<br>"
        Else
            If Not Regex.IsMatch(txtUser.Text, "^[a-zA-Z0-9_]+$") Then
                psErrMsg &= "<LI type=square>Invalid User Id<br>"
            End If
        End If

        If txtName.Text = "" Then
            psErrMsg &= "<LI type=square>Name is required.<br>"
        End If

        If txtEmail.Text = "" Then
            psErrMsg &= "<LI type=square>Email is required.<br>"
        Else
            Dim strMsg As String
            If Not IsEmail(strMsg, txtEmail.Text) Then
                psErrMsg &= "<LI type=square>" & strMsg & "<br>"
            End If
        End If

        If txtDesignation.Text = "" Then
            psErrMsg &= "<LI type=square>Designation is required.<br>"
        End If

        'If Session("Env") <> "FTN" Then
        '    If Not objUsers.IsTieToAnyUserGrp(Session("CompanyId"), txtUser.Text, "eProcure") And lstUGSelected.Items.Count <= 0 Then
        '        psErrMsg &= "<LI type=square>User group is required.<br>"
        '    End If

        '    Dim lstItem As ListItem
        '    For Each lstItem In lstUGSelected.Items
        '        If objUsers.isOtherAppAdmin(txtUser.Text, Session("CompanyId"), "eProcure", lstItem.Value) Then
        '            psErrMsg &= "<LI type=square>User is not allowed to carry multiple admin role in the system. Please select another user group.<br>"
        '        End If
        '    Next

        '    objUsers = Nothing

        '    If txtAppLimit.Text = "" Then
        '        psErrMsg &= "<LI type=square>Approval limit is required.<br>"
        '    Else
        '        If Not IsCost(txtAppLimit.Text) Then
        '            'Michelle (20/9/2010) - Replace the error message
        '            'psErrMsg &= "<LI type=square>Approval limit is expecting currency.<br>"
        '            psErrMsg &= "<LI type=square>Maximum 2 decimal places for approval limit.<br>"
        '        End If
        '    End If

        '    If txtPOAppLimit.Text = "" Then
        '        psErrMsg &= "<LI type=square>PO Approval limit is required.<br>"
        '    Else
        '        If Not IsCost(txtPOAppLimit.Text) Then
        '            'Michelle (20/9/2010) - Replace the error message
        '            'psErrMsg &= "<LI type=square>Approval limit is expecting currency.<br>"
        '            psErrMsg &= "<LI type=square>Maximum 2 decimal places for approval limit.<br>"
        '        End If
        '    End If

        '    'To check whether the PR App Limit is <= to previous approving officer an all the approval he is involved in

        '    If Not ChkLimit("PR", "P") Then
        '        psErrMsg &= "<LI type=square>PR Approval Limit must > PREVIOUS approval limit in: " & _
        '                    AppGrpName(lGrpIndex) & ".<br>"
        '    End If

        '    'To check whether the PR App Limit is > to previous approving officer an all the approval he is involved in
        '    If Not ChkLimit("PR", "N") Then
        '        psErrMsg &= "<LI type=square>PR Approval Limit must < NEXT approval Limit in: " & _
        '                    AppGrpName(lGrpIndex) & ".<br>"
        '    End If

        '    If Not ChkLimit("PO", "P") Then
        '        psErrMsg &= "<LI type=square>PR Approval Limit must > PREVIOUS approval limit in: " & _
        '                    AppGrpName(lGrpIndex) & ".<br>"
        '    End If

        '    If Not ChkLimit("PO", "N") Then
        '        psErrMsg &= "<LI type=square>PR Approval Limit must < NEXT approval Limit in: " & _
        '                    AppGrpName(lGrpIndex) & ".<br>"
        '    End If

        '    If txtInvAppLimit.Text = "" Then
        '        psErrMsg &= "<LI type=square>Invoice approval limit is required.<br>"
        '    Else
        '        If Not IsCost(txtInvAppLimit.Text) Then
        '            'Michelle (20/9/2010) - Replace the error message
        '            'psErrMsg &= "<LI type=square>Invoice approval limit is expecting currency.<br>"
        '            psErrMsg &= "<LI type=square>Maximum 2 decimal places for Invoice approval limit.<br>"
        '        End If
        '    End If

        '    'To check whether the Inv App Limit is <= to previous approving officer an all the approval he is involved in
        '    If Not ChkLimit("INV", "P") Then
        '        psErrMsg &= "<LI type=square>Invoice Approval Limit must > PREVIOUS approval limit in: " & _
        '                    AppGrpName(lGrpIndex) & ".<br>"
        '    End If

        '    'To check whether the Inv App Limit is > to previous approving officer an all the approval he is involved in
        '    If Not ChkLimit("INV", "N") Then
        '        psErrMsg &= "<LI type=square>Invoice Approval Limit must < NEXT approval limit in: " & _
        '                    AppGrpName(lGrpIndex) & ".<br>"
        '    End If
        'End If

        If psErrMsg <> "" Then
            Return False
        Else
            Return True
        End If
    End Function
    Private Function ChkLimit(ByVal AppType As String, ByVal ChkType As String) As Boolean
        Dim strsql, strsql1 As String
        Dim intSeq, intCnt As Integer
        Dim decAppLim, decComLim As Decimal

        If AppType = "PR" Then
            strsql = "Select * from approval_grp_ao, approval_grp_mstr where aga_ao = '" & txtUser.Text & _
                     "' AND agm_grp_index = aga_grp_index and agm_type = 'PR' and agm_coy_id ='" & Session("CompanyId") & "'"

            decComLim = Convert.ToDecimal(txtAppLimit.Text)
        ElseIf AppType = "PO" Then
            strsql = "Select * from approval_grp_ao, approval_grp_mstr where aga_ao = '" & txtUser.Text & _
                     "' AND agm_grp_index = aga_grp_index and agm_type = 'PO' and agm_coy_id ='" & Session("CompanyId") & "'"

            decComLim = Convert.ToDecimal(txtPOAppLimit.Text)
        Else 'For Invoice, check for the FO approval flow
            strsql = "Select * from approval_grp_fo, approval_grp_mstr where agfo_fo = '" & txtUser.Text & _
                    "' AND agm_grp_index = agfo_grp_index and agm_type = 'INV' and agm_coy_id ='" & Session("CompanyId") & "'"

            decComLim = Convert.ToDecimal(txtInvAppLimit.Text)

        End If

        Dim tDS As DataSet = objDb.FillDs(strsql)
        For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
            If AppType = "PR" Then
                intSeq = tDS.Tables(0).Rows(j).Item("AGA_SEQ")
                lGrpIndex = tDS.Tables(0).Rows(j).Item("AGA_GRP_INDEX")
            Else
                intSeq = tDS.Tables(0).Rows(j).Item("AGFO_SEQ")
                lGrpIndex = tDS.Tables(0).Rows(j).Item("AGFO_GRP_INDEX")
            End If

            If ChkType = "P" Then   'Check for the previous limit
                If intSeq > 1 Then 'If he is the 1st approval, then no need to check
                    'If AppType = "PR" Then
                    '    strsql1 = "Select um_app_limit from approval_grp_ao, user_mstr, approval_grp_mstr " & _
                    '               " where aga_grp_index = " & lGrpIndex & " and aga_seq = " & intSeq - 1 & _
                    '               " AND UM_STATUS ='A' AND UM_DELETED='N' AND UM_COY_ID ='" & Session("CompanyId") & "'" & _
                    '               " and um_user_id = aga_ao" & _
                    '               " and aga_grp_index = agm_grp_index and agm_coy_id = '" & Session("CompanyId") & "'"

                    'Else
                    '    strsql1 = "Select um_invoice_app_limit from approval_grp_fo, user_mstr, approval_grp_mstr " & _
                    '              " where agfo_grp_index = " & lGrpIndex & " and agfo_seq = " & intSeq - 1 & _
                    '              " AND UM_STATUS ='A' AND UM_DELETED='N' AND UM_COY_ID ='" & Session("CompanyId") & "'" & _
                    '              " and um_user_id = agfo_fo" & _
                    '              " and agfo_grp_index = agm_grp_index and agm_coy_id = '" & Session("CompanyId") & "'"
                    'End If

                    If AppType = "PR" Then
                        strsql1 = "Select um_app_limit from approval_grp_ao, user_mstr, approval_grp_mstr " & _
                                   " where aga_grp_index = " & lGrpIndex & " and aga_seq = " & intSeq - 1 & _
                                   " AND UM_STATUS ='A' AND UM_DELETED='N' AND UM_COY_ID ='" & Session("CompanyId") & "'" & _
                                   " and um_user_id = aga_ao" & _
                                   " and aga_grp_index = agm_grp_index and agm_type = 'PR' and agm_coy_id = '" & Session("CompanyId") & "'"

                    ElseIf AppType = "PO" Then
                        strsql1 = "Select um_po_app_limit, 0 as um_po_app_limit from approval_grp_ao, user_mstr, approval_grp_mstr " & _
                                   " where aga_grp_index = " & lGrpIndex & " and aga_seq = " & intSeq - 1 & _
                                   " AND UM_STATUS ='A' AND UM_DELETED='N' AND UM_COY_ID ='" & Session("CompanyId") & "'" & _
                                   " and um_user_id = aga_ao" & _
                                   " and aga_grp_index = agm_grp_index and agm_type = 'PO' and agm_coy_id = '" & Session("CompanyId") & "'"
                    Else
                        strsql1 = "Select um_invoice_app_limit from approval_grp_fo, user_mstr, approval_grp_mstr " & _
                                  " where agfo_grp_index = " & lGrpIndex & " and agfo_seq = " & intSeq - 1 & _
                                  " AND UM_STATUS ='A' AND UM_DELETED='N' AND UM_COY_ID ='" & Session("CompanyId") & "'" & _
                                  " and um_user_id = agfo_fo" & _
                                  " and agfo_grp_index = agm_grp_index and agm_type = 'INV' and agm_coy_id = '" & Session("CompanyId") & "'"
                    End If

                    decAppLim = objDb.GetVal(strsql1)
                    If decComLim < decAppLim Then

                        Return False
                    End If
                End If
            Else 'Check for the next limit
                intCnt = 0
                'If AppType = "PR" Then
                '    '  intCnt = objDb.GetCount("approval_grp_ao, user_mstr", " where aga_grp_index = " & lGrpIndex & " and aga_seq = " & intSeq + 1 & " AND UM_STATUS ='A' AND UM_DELETED='N' AND UM_COY_ID ='" & Session("CompanyId") & "' and um_user_id = '" & txtUser.Text & "'")
                '    strsql1 = "Select um_app_limit from approval_grp_ao, user_mstr, approval_grp_mstr " & _
                '               " where aga_grp_index = " & lGrpIndex & " and aga_seq = " & intSeq + 1 & _
                '               " AND UM_STATUS ='A' AND UM_DELETED='N' AND UM_COY_ID ='" & Session("CompanyId") & "'" & _
                '               " and um_user_id = aga_ao" & _
                '               " and aga_grp_index = agm_grp_index and agm_coy_id = '" & Session("CompanyId") & "'"
                'Else
                '    strsql1 = "Select um_invoice_app_limit from approval_grp_fo, user_mstr, approval_grp_mstr " & _
                '              " where agfo_grp_index = " & lGrpIndex & " and agfo_seq = " & intSeq + 1 & _
                '              " AND UM_STATUS ='A' AND UM_DELETED='N' AND UM_COY_ID ='" & Session("CompanyId") & "'" & _
                '              " and um_user_id = agfo_fo" & _
                '              " and agfo_grp_index = agm_grp_index and agm_coy_id = '" & Session("CompanyId") & "'"
                '    '   intCnt = objDb.GetCount("approval_grp_fo, user_mstr", " where agfo_grp_index = " & lGrpIndex & " and agfo_seq = " & intSeq + 1 & " AND UM_STATUS ='A' AND UM_DELETED='N' AND UM_COY_ID ='" & Session("CompanyId") & "' and um_user_id = '" & txtUser.Text & "'")
                'End If

                If AppType = "PR" Then
                    '  intCnt = objDb.GetCount("approval_grp_ao, user_mstr", " where aga_grp_index = " & lGrpIndex & " and aga_seq = " & intSeq + 1 & " AND UM_STATUS ='A' AND UM_DELETED='N' AND UM_COY_ID ='" & Session("CompanyId") & "' and um_user_id = '" & txtUser.Text & "'")
                    strsql1 = "Select um_app_limit from approval_grp_ao, user_mstr, approval_grp_mstr " & _
                               " where aga_grp_index = " & lGrpIndex & " and aga_seq = " & intSeq + 1 & _
                               " AND UM_STATUS ='A' AND UM_DELETED='N' AND UM_COY_ID ='" & Session("CompanyId") & "'" & _
                               " and um_user_id = aga_ao" & _
                               " and aga_grp_index = agm_grp_index and agm_type = 'PR' and agm_coy_id = '" & Session("CompanyId") & "'"
                ElseIf AppType = "PO" Then
                    '  intCnt = objDb.GetCount("approval_grp_ao, user_mstr", " where aga_grp_index = " & lGrpIndex & " and aga_seq = " & intSeq + 1 & " AND UM_STATUS ='A' AND UM_DELETED='N' AND UM_COY_ID ='" & Session("CompanyId") & "' and um_user_id = '" & txtUser.Text & "'")
                    strsql1 = "Select um_po_app_limit from approval_grp_ao, user_mstr, approval_grp_mstr " & _
                               " where aga_grp_index = " & lGrpIndex & " and aga_seq = " & intSeq + 1 & _
                               " AND UM_STATUS ='A' AND UM_DELETED='N' AND UM_COY_ID ='" & Session("CompanyId") & "'" & _
                               " and um_user_id = aga_ao" & _
                               " and aga_grp_index = agm_grp_index and agm_type = 'PO' and agm_coy_id = '" & Session("CompanyId") & "'"
                Else
                    strsql1 = "Select um_invoice_app_limit from approval_grp_fo, user_mstr, approval_grp_mstr " & _
                              " where agfo_grp_index = " & lGrpIndex & " and agfo_seq = " & intSeq + 1 & _
                              " AND UM_STATUS ='A' AND UM_DELETED='N' AND UM_COY_ID ='" & Session("CompanyId") & "'" & _
                              " and um_user_id = agfo_fo" & _
                              " and agfo_grp_index = agm_grp_index and agm_type = 'INV' and agm_coy_id = '" & Session("CompanyId") & "'"
                    '   intCnt = objDb.GetCount("approval_grp_fo, user_mstr", " where agfo_grp_index = " & lGrpIndex & " and agfo_seq = " & intSeq + 1 & " AND UM_STATUS ='A' AND UM_DELETED='N' AND UM_COY_ID ='" & Session("CompanyId") & "' and um_user_id = '" & txtUser.Text & "'")
                End If


                '  If objDb.GetCount(strsql1) > 0 Then 'If he is the last approval, then no need to check
                '              If intCnt > 0 Then  'If he is the last approval, then no need to check
                If objDb.GetVal(strsql1) <> "" Then
                    decAppLim = objDb.GetVal(strsql1)
                    If decAppLim > 0 Then  'ie he is not the last approving officer
                        If decComLim > decAppLim Then

                            Return False
                        End If

                    End If
                Else
                    decAppLim = 0
                End If


            End If
        Next
        Return True
    End Function
    Private Function AppGrpName(ByVal GrpIndex As Long) As String
        Dim strsql As String

        strsql = "Select agm_grp_name from approval_grp_mstr where agm_grp_index = " & GrpIndex
        AppGrpName = objDb.GetVal(strsql)
    End Function

    Private Sub lnkMassApp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkMassApp.Click
        Dim strFileName As String

        Dim objUserDetails As New User
        objUserDetails = GetInput()

        Dim strscript As New System.Text.StringBuilder
        strscript.Append("<script language=""javascript"">")
        strscript.Append("ShowDialog('" & dDispatcher.direct("Users", "Dialog.aspx", "page=" & Server.UrlEncode(dDispatcher.direct("Users", "usMassApp.aspx.aspx", "&pageid=" & strPageId & "&userid=" + txtUser.Text + "&usergroup=" + Server.UrlEncode(objUserDetails.UserGroup)))) & "','400px');")


        strscript.Append("</script>")
        RegisterStartupScript("script3", strscript.ToString())
    End Sub

    Private Sub lnkSetDelAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkSetDelAdd.Click
        Dim strFileName As String

        Dim strscript As New System.Text.StringBuilder
        strscript.Append("<script language=""javascript"">")
        strFileName = dDispatcher.direct("Users", "usDeliveryAddr.aspx", "userid=" & txtUser.Text & "&role=SK&addrtype=D&pageid=" & strPageId)
        strFileName = Server.UrlEncode(strFileName)
        strscript.Append("ShowDialog('" & dDispatcher.direct("Users", "Dialog.aspx", "page=" & strFileName) & "','530px');")

        strscript.Append("</script>")
        RegisterStartupScript("script3", strscript.ToString())
    End Sub

    Private Sub lnkFinVwDept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkFinVwDept.Click
        Dim strFileName As String

        Dim strscript As New System.Text.StringBuilder
        strscript.Append("<script language=""javascript"">")
        strscript.Append("ShowDialog('" + dDispatcher.direct("Users", "Dialog.aspx", dDispatcher.direct("Users", "usFinVwDept.aspx", "&pageid=" & strPageId & "&userid=" + txtUser.Text)) + "','530px');")
        strscript.Append("</script>")
        RegisterStartupScript("script3", strscript.ToString())
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim strMsg As String
        Dim objUser As New Users
        Dim strRedirect As String
        If objUser.DelUser(viewstate("UserId"), False) Then
            EnableInput(False)
        End If
        strRedirect = "" & dDispatcher.direct("Users", "usSearchUser.aspx", "pageid=" & strPageId)
        Common.NetMsgbox(Me, objUser.Message, strRedirect, MsgBoxStyle.Information)
    End Sub

    Private Sub EnableInput(ByVal pBoo As Boolean)
        txtUser.Enabled = pBoo
        txtName.Enabled = pBoo
        cboDeptName.Enabled = pBoo
        txtEmail.Enabled = pBoo
        txtPhone.Enabled = pBoo
        txtFax.Enabled = pBoo
        txtDesignation.Enabled = pBoo
        txtAppLimit.Enabled = pBoo
        txtPOAppLimit.Enabled = pBoo
        txtInvAppLimit.Enabled = pBoo
        lnkSetDelAdd.Enabled = pBoo
        lnkMassApp.Enabled = pBoo
        lnkFinVwDept.Enabled = pBoo
        cmdSave.Enabled = pBoo
        cmdGeneratePwd.Enabled = pBoo
        cmdDelete.Enabled = pBoo
        cmdReset.Disabled = Not pBoo
        cmdRight.Enabled = pBoo
        cmdLeft.Enabled = pBoo
        lstUGAvail.Enabled = pBoo
        lstUGSelected.Enabled = pBoo
        rdAct.Enabled = pBoo
        rdDeAct.Enabled = pBoo
        chkAccLock.Enabled = pBoo
    End Sub

    Private Sub lnkSetDelAddBuyer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkSetDelAddBuyer.Click
        Dim strFileName As String

        Dim strscript As New System.Text.StringBuilder
        strscript.Append("<script language=""javascript"">")
        strFileName = dDispatcher.direct("Users", "usDeliveryAddr.aspx", "userid=" & txtUser.Text & "&role=B&addrtype=D&pageid=" & strPageId)
        strFileName = Server.UrlEncode(strFileName)
        strscript.Append("ShowDialog('" & dDispatcher.direct("Users", "Dialog.aspx", "page=" & strFileName) & "','530px');")
        strscript.Append("</script>")
        RegisterStartupScript("script3", strscript.ToString())
    End Sub

    Private Sub lnkSetBillAddBuyer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkSetBillAddBuyer.Click
        Dim strFileName As String

        Dim strscript As New System.Text.StringBuilder
        strscript.Append("<script language=""javascript"">")
        strFileName = dDispatcher.direct("Users", "usDeliveryAddr.aspx", "userid=" & txtUser.Text & "&role=B&addrtype=B&pageid=" & strPageId)
        strFileName = Server.UrlEncode(strFileName)
        strscript.Append("ShowDialog('" & dDispatcher.direct("Users", "Dialog.aspx", "page=" & strFileName) & "','530px');")
        strscript.Append("</script>")
        RegisterStartupScript("script3", strscript.ToString())
    End Sub

    Private Sub cmdReset_ServerClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.ServerClick
        isNotPostBack()
    End Sub

    Public Sub rbRole_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbRole.SelectedIndexChanged
        Dim objUserDetails As New User
        objUserDetails = GetInput()
        ShowLink(objUserDetails.UserGroup)

    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        'If ViewState("side") = "B" Then
        '    Session("w_User_tabs") = "<div class=""t_entity""><ul>" & _
        '"<li><div class=""space""></div></li>" & _
        '                     "<li><a class=""t_entity_btn_selected"" href=""usSearchUser.aspx?pageid=" & strPageId & """><span>User Account</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                     "<li><a class=""t_entity_btn"" href=""usDeliveryAddr.aspx?addrtype=D&pageid=" & strPageId & """><span>Delivery Address</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                     "<li><a class=""t_entity_btn"" href=""usDeliveryAddr.aspx?addrtype=B&pageid=" & strPageId & """><span>Billing Address</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                   "</ul><div></div></div>"
        'Else
        '    Session("w_User_tabs") = "<div class=""t_entity""><ul>" & _
        '"<li><div class=""space""></div></li>" & _
        '                     "<li><a class=""t_entity_btn_selected"" href=""usSearchUser.aspx?pageid=" & strPageId & """><span>User Account</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        ' "</ul><div></div></div>"

        'End If
        If ViewState("side") = "B" Then
            Session("w_User_tabs") = "<div class=""t_entity""><ul>" & _
                           "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Users", "usSearchUser.aspx", "pageid=" & strPageId) & """><span>User Account</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usDeliveryAddr.aspx", "addrtype=D&pageid=" & strPageId) & """><span>Delivery Address</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usDeliveryAddr.aspx", "addrtype=B&pageid=" & strPageId) & """><span>Billing Address</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usCommodity.aspx", "pageid=" & strPageId) & """><span>Commodity Assignment</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                            "</ul><div></div></div>"
        Else
            Session("w_User_tabs") = "<div class=""t_entity""><ul>" & _
            "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Users", "usSearchUser.aspx", "pageid=" & strPageId) & """><span>User Account</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
         "</ul><div></div></div>"

        End If
    End Sub

    Private Sub cmdadd_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.ServerClick
        ViewState("Mode") = "add"
        txtUser.Enabled = True
        isNotPostBack()
    End Sub

End Class