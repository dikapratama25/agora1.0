Imports AgoraLegacy
Imports System.Text.RegularExpressions
Imports eProcure.Component



Public Class usUser
    Inherits AgoraLegacy.AppBaseClass
    Dim objDb As New EAD.DBCom
    Dim strMode As String
    Dim lGrpIndex As Long
    Dim ctx As Web.HttpContext = Web.HttpContext.Current
    Protected WithEvents hidlistbox As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents hidrb As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents rbRole As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents chkMassAppr As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkInvMassAppr As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkMrsMassAppr As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkSpot As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkStock As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkMro As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblSpot As System.Web.UI.WebControls.Label
    Protected WithEvents lblStock As System.Web.UI.WebControls.Label
    Protected WithEvents lblMro As System.Web.UI.WebControls.Label
    Protected WithEvents lblAction As System.Web.UI.WebControls.Label
    Protected WithEvents trStockType As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents divchk1 As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents divchk2 As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents divchk3 As System.Web.UI.HtmlControls.HtmlGenericControl
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
    Protected WithEvents lbl1 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl3 As System.Web.UI.WebControls.Label
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
    Protected WithEvents trPOApprovalLimit As System.Web.UI.HtmlControls.HtmlTableRow
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
        Dim objUser_Ext As New Users_Ext
        Dim objUserDetails As New User
        Dim objCom As New Companies

        Dim companytype As String = objCom.GetCompanyType

        If companytype.ToUpper = "VENDOR" Then
            txtAppLimit.Text = 0
            txtPOAppLimit.Text = 0
            txtInvAppLimit.Text = 0
            trApprovalLimit.Visible = False
            trPOApprovalLimit.Visible = False
            trInvoiceApprovalLimit.Visible = False
            trDept.Visible = False
            ViewState("side") = "V"
        ElseIf companytype.ToUpper = "BUYER" Or companytype.ToUpper = "BOTH" Then
            trApprovalLimit.Visible = True
            trPOApprovalLimit.Visible = True
            trInvoiceApprovalLimit.Visible = True
            Dim intDept As Integer
            Dim objDBAccess As New EAD.DBCom
            intDept = objDBAccess.GetCount("company_dept_mstr", " where cdm_coy_id = '" & Session("CompanyID") & "' and cdm_deleted = 'N'")
            If intDept = 0 Then trDept.Visible = False
            ViewState("side") = "B"
            'Me.lblAction.Text = "<b>=></b> Step 1: Create, modify or delete User Account.<br />Step 2: Assign Delivery Address to selected User Account.<br>Step 3: Assign Billing Address to selected User Account."
            Me.lblAction.Text = "<b>=></b> Step 1: Create, modify or delete User Account.<br />Step 2: Assign Delivery Address to selected User Account.<br>Step 3: Assign Billing Address to selected User Account.<br>Step 4: Assign Finance Viewing Department to selected User Account.<br>Step 5: Assign Commodity Type to selected User Account."
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
            Session("SCEmailOnOff") = objUserDetails.SCEmailOnOff
            cmdadd.Visible = True
        End If
        Populate(objUserDetails)
        'If Session("Env") = "FTN" Then
        '    '  Me.hidlistbox.Style("display") = "none"
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
        Me.hidlistbox.Style("display") = ""
        Me.hidrb.Style("display") = "none"
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
        Dim objGlobal As New AppGlobals
        Dim objcomp As New Companies
        Dim objUserDetail As New User
        Dim objPwd As New RandomPassword
        Dim objMail As New Email
        Dim strErrMsg As String
        Dim i As Integer
        Dim strTmpGrp As String

        objUserDetail = GetInput(False)

        Dim spUsrGrp As Array
        spUsrGrp = Split(objUserDetail.UserGroup, ",")

        For i = 0 To spUsrGrp.Length - 1
            strTmpGrp = objUser.GetFixedRole(spUsrGrp(i))
            If strTmpGrp = "Purchasing Officer" Then
                objUserDetail.strFixedRole = strTmpGrp
            End If
        Next

        objUserDetail.Password = objPwd.GenerateAutoPwd()
        objUserDetail.NextExpiredDt = objUser.GetPwdExpireDt(True)

        If ValidateFields(strErrMsg) Then
            If Page.IsValid Then
                If ViewState("Mode") = "add" Then
                    If objUser.AddUser(objUserDetail, False, EnumAppPackage.eProcure.ToString) Then
                        ViewState("UserId") = objUserDetail.UserID
                        objMail.sendNotification(EmailType.NewUserAccount, objUserDetail.UserID, objUserDetail.CompanyID, "", "", "", objUserDetail.Password)
                    End If
                    Common.NetMsgbox(Me, objUser.Message, MsgBoxStyle.Information)
                    If objUser.Message = "Record saved." Then
                        ViewState("Mode") = "modify"
                        txtUser.Enabled = False
                        cmdadd.Visible = True
                    End If
                    lblMsg.Visible = False

                ElseIf ViewState("Mode") = "modify" Then

                    If txtEmail.Text <> Session("userEmailOri") Then
                        blnEmailChange = True
                    End If

                    If objUser.UpdateUser(objUserDetail, False, EnumAppPackage.eProcure.ToString, False, blnEmailChange) Then

                    End If
                    Session("userEmailOri") = txtEmail.Text
                    Common.NetMsgbox(Me, objUser.Message, MsgBoxStyle.Information)
                    'Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00003"), MsgBoxStyle.Information)
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

                objUser.UpdateMassApproval(ViewState("UserId"), IIf(objUserDetail.MassApp = "Y", True, False))
                objUser.UpdateInvoiceMassApproval(ViewState("UserId"), IIf(objUserDetail.InvoiceMassApp = "Y", True, False))
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

        'mimi 2018-05-07 : enabled for E2P
        'If objUser.IsRoleSelected(pUSerGroup, FixedRole.Finance_Officer) Then
        '    txtInvAppLimit.Enabled = True
        'Else
        '    txtInvAppLimit.Text = 0
        '    txtInvAppLimit.Enabled = False
        'End If
        'end

        If objUser.IsRoleSelected(pUSerGroup, FixedRole.Approving_Officer) Or objUser.IsRoleSelected(pUSerGroup, FixedRole.Finance_Officer) Then
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
        objUserDetail = objUser.GetUserDetails(ViewState("UserId"), Session("CompanyId"))

        'User temporary password from Login policy
        If objUser.UpdatePassword(ViewState("UserId"), strNewPwd, Session("CompanyId"), True) Then
            objMail.sendNotification(EmailType.ChangePwd, ViewState("UserId"), Session("CompanyID"), "", "", "", strNewPwd)
            'SendMail(objUserDetail.Email, GetMailFormat(objUserDetail.UserID, strNewPwd, objUser.GetPwdExpireDt(True)), "New Generated Password")
            lblPwdExp.Text = objUser.GetPwdExpireDt(True)
            Common.NetMsgbox(Me, "Password generated.", MsgBoxStyle.Information)
        End If
    End Sub

    Private Function GetInput(Optional ByVal BlnChk As Boolean = True) As User
        Dim objUserDetail As New User
        Dim strTmpGrp As String
        Dim objUser As New Users

        objUserDetail.UserID = txtUser.Text
        objUserDetail.Name = txtName.Text
        objUserDetail.DeptID = cboDeptName.SelectedValue

        objUserDetail.UserGroup = Lst2String(lstUGSelected, False, False)
        If ViewState("side") = "B" Then
            trMassAppr.Visible = False
            divchk1.Style("display") = "none"
            divchk2.Style("display") = "none"
            divchk3.Style("display") = "none"
            trStockType.Visible = False
            If objUser.IsRoleSelected(objUserDetail.UserGroup, FixedRole.Approving_Officer) Then
                trMassAppr.Visible = True
                lbl1.Text = "Mass Approval authorise approving officer to approve more than one PR/PO/IR at a time"
                lbl1.Visible = True
                'chkMassAppr.Visible = True
                divchk1.Style("display") = ""
            Else
                chkMassAppr.Checked = False
            End If
            If objUser.IsRoleSelected(objUserDetail.UserGroup, FixedRole.Finance_Officer) Then
                trMassAppr.Visible = True
                lbl2.Text = "Mass Approval authorise approving officer to approve more than one Invoice at a time"
                lbl2.Visible = True
                'chkInvMassAppr.Visible = True
                divchk2.Style("display") = ""
            Else
                chkInvMassAppr.Checked = False
            End If
            If objUser.IsRoleSelected(objUserDetail.UserGroup, FixedRole.Store_Keeper) Then
                trMassAppr.Visible = True
                lbl3.Text = "Mass Approval authorise approving officer to approve more than one MRS at a time"
                lbl3.Visible = True
                divchk3.Style("display") = ""
            Else
                chkMrsMassAppr.Checked = False
            End If
            If objUser.IsRoleSelected(objUserDetail.UserGroup, FixedRole.Purchasing_Manager) Or objUser.IsRoleSelected(objUserDetail.UserGroup, FixedRole.Purchasing_Officer) Then
                trStockType.Visible = True
                chkSpot.Visible = True
                chkStock.Visible = True
                chkMro.Visible = True
                lblSpot.Visible = True
                lblStock.Visible = True
                lblMro.Visible = True
                If BlnChk = True Then
                    chkSpot.Checked = True
                    chkStock.Checked = True
                    chkMro.Checked = True
                End If
                'End If      
            Else
                chkSpot.Checked = True
                chkStock.Checked = True
                chkMro.Checked = True
            End If
        Else
            objUserDetail.UserGroup = Lst2String(lstUGSelected, False, False)
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

        objUserDetail.SCEmailOnOff = Session("SCEmailOnOff")

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
        If chkInvMassAppr.Checked Then
            objUserDetail.InvoiceMassApp = "Y"
        Else
            objUserDetail.InvoiceMassApp = "N"
        End If
        If chkMrsMassAppr.Checked Then
            objUserDetail.MrsMassApp = "Y"
        Else
            objUserDetail.MrsMassApp = "N"
        End If

        If chkSpot.Checked Then
            objUserDetail.StockTypeSpot = "Y"
        Else
            objUserDetail.StockTypeSpot = "N"
        End If
        If chkStock.Checked Then
            objUserDetail.StockTypeStock = "Y"
        Else
            objUserDetail.StockTypeStock = "N"
        End If
        If chkMro.Checked Then
            objUserDetail.StockTypeMro = "Y"
        Else
            objUserDetail.StockTypeMro = "N"
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

        If ViewState("Mode") = "add" Then
            lblHeader.Text = "Add User Account"
            hidPageCount.Value = "10"

            'Added condition for other company's types
            If objCompDetails.CoyType = "BOTH" Then
                sWhere = " UGM_TYPE IN ('VENDOR','BUYER')"
            ElseIf objCompDetails.CoyType = "VENDOR" Then
                sWhere = " UGM_TYPE='VENDOR'"
            ElseIf objCompDetails.CoyType = "BUYER" Then
                sWhere = " UGM_TYPE='BUYER'"
            ElseIf objCompDetails.CoyType = "HUB" Then
                sWhere = " UGM_TYPE='HUB'"
            Else
                sWhere = "UGM_TYPE='VENDOR'"
            End If
            sWhere &= " AND UGM_DELETED<>'Y' AND UGM_FIXED_ROLE <> 'Super Admin' AND SUBSTRING(UGM_USRGRP_ID,1,3) <> 'FTN'"

            Common.FillDefault(lstUGAvail, "USER_GROUP_MSTR", "UGM_USRGRP_NAME", "UGM_USRGRP_ID", sWhere)
            Common.SortListControl(lstUGAvail, True)
            cboDeptName.Items.Clear()
            'Zulham 17072018 - PAMB
            Common.FillDefault(cboDeptName, "COMPANY_DEPT_MSTR", "CDM_DEPT_NAME", "CDM_DEPT_CODE", "---Select---", " CDM_DELETED<>'Y' AND CDM_COY_ID='" & Session("CompanyId") & "' GROUP BY CDM_DEPT_CODE ")
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
            If pUser.InvoiceMassApp = "Y" Then
                chkInvMassAppr.Checked = True
            Else
                chkInvMassAppr.Checked = False
            End If
            If pUser.MrsMassApp = "Y" Then
                chkMrsMassAppr.Checked = True
            Else
                chkMrsMassAppr.Checked = False
            End If
            If pUser.StockTypeSpot = "Y" Then
                chkSpot.Checked = True
            Else
                chkSpot.Checked = False
            End If
            If pUser.StockTypeStock = "Y" Then
                chkStock.Checked = True
            Else
                chkStock.Checked = False
            End If
            If pUser.StockTypeMro = "Y" Then
                chkMro.Checked = True
            Else
                chkMro.Checked = False
            End If
            'Bind and selected
            cboDeptName.Items.Clear()
            'Zulham 17072018 - PAMB
            Common.FillDefault(cboDeptName, "COMPANY_DEPT_MSTR", "CDM_DEPT_NAME", "CDM_DEPT_CODE", "---Select---", "CDM_DELETED='N' AND CDM_COY_ID='" & Session("CompanyId") & "' GROUP BY CDM_DEPT_CODE ")
            Common.SelDdl(pUser.DeptID, cboDeptName, True, True)
            Common.FillLst(lstUGSelected, "UGM_USRGRP_NAME", "UGM_USRGRP_ID", objUser.GetUserGroup(pUser.UserID, objCompDetails.CoyType, False))
            Common.SortListControl(lstUGSelected, True)
            Common.FillLst(lstUGAvail, "UGM_USRGRP_NAME", "UGM_USRGRP_ID", objUser.GetUserGroup(pUser.UserID, objCompDetails.CoyType, False, True, True))
            Common.SortListControl(lstUGAvail, True)

            'Added by Joon on 05th July 2011
            trMassAppr.Visible = False
            divchk1.Style("display") = "none"
            divchk2.Style("display") = "none"
            divchk3.Style("display") = "none"
            trStockType.Visible = False
            chkSpot.Visible = False
            chkStock.Visible = False
            chkMro.Visible = False
            lblSpot.Visible = False
            lblStock.Visible = False
            lblMro.Visible = False
            If objUser.IsRoleSelected(pUser.UserGroup, FixedRole.Approving_Officer) Then
                'blnMassAppr = True
                trMassAppr.Visible = True
                lbl1.Text = "Mass Approval authorise approving officer to approve more than one PR/PO at a time"
                'lbl1.Visible = True
                'chkMassAppr.Visible = True
                divchk1.Style("display") = ""
            End If
            If objUser.IsRoleSelected(pUser.UserGroup, FixedRole.Finance_Officer) Then
                'blnInvMassAppr = True
                trMassAppr.Visible = True
                lbl2.Text = "Mass Approval authorise approving officer to approve more than one Invoice at a time"
                'lbl2.Visible = True
                'chkInvMassAppr.Visible = True
                divchk2.Style("display") = ""
            End If
            If objUser.IsRoleSelected(pUser.UserGroup, FixedRole.Store_Keeper) Then
                'blnInvMassAppr = True
                trMassAppr.Visible = True
                lbl3.Text = "Mass Approval authorise approving officer to approve more than one MRS at a time"
                'lbl3.Visible = True
                'chkMrsMassAppr.Visible = True
                divchk3.Style("display") = ""
            End If
            If objUser.IsRoleSelected(pUser.UserGroup, FixedRole.Purchasing_Manager) Or objUser.IsRoleSelected(pUser.UserGroup, FixedRole.Purchasing_Officer) Then
                trStockType.Visible = True
                chkSpot.Visible = True
                chkStock.Visible = True
                chkMro.Visible = True
                lblSpot.Visible = True
                lblStock.Visible = True
                lblMro.Visible = True
            End If
            ShowLink(pUser.UserGroup)

            Dim objUserRole As New UserRoles
            Dim strRole As String = objUserRole.get_UserRole
            Dim strFixedRole As String = objUserRole.get_UserFixedRole

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
        Dim count As Integer = 0
        Dim i As Integer
        Dim str As String
        Dim blnPOLimit, blnPRLimit, blnInvLimit As Boolean

        For i = 0 To txtPOAppLimit.Text.Length - 1
            str = txtPOAppLimit.Text.Substring(i, 1)
            If str = "." Then
                count = count + 1
            End If
        Next
        If count >= 2 Then
            psErrMsg &= "<LI type=square>Invalid PO Approval Limit.<br>"
            Return False
        End If
        count = 0

        For i = 0 To txtAppLimit.Text.Length - 1
            str = txtAppLimit.Text.Substring(i, 1)
            If str = "." Then
                count = count + 1
            End If
        Next
        If count >= 2 Then
            psErrMsg &= "<LI type=square>Invalid PR Approval Limit.<br>"
            Return False
        End If
        count = 0

        For i = 0 To txtInvAppLimit.Text.Length - 1
            str = txtInvAppLimit.Text.Substring(i, 1)
            If str = "." Then
                count = count + 1
            End If
        Next
        If count >= 2 Then
            psErrMsg &= "<LI type=square>Invalid Invoice Approval Limit.<br>"
            Return False
        End If
        count = 0


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
        '        'Michelle (4/5/2012) - Issue 1604
        '        'psErrMsg &= "<LI type=square>User group is required.<br>"
        '        If Not objUsers.checkUserFixedRole("'SUPER ADMIN'", txtUser.Text) Then
        '            psErrMsg &= "<LI type=square>User group is required.<br>"
        '        End If
        '    End If
        '    Dim lstItem As ListItem
        '    For Each lstItem In lstUGSelected.Items
        '        If objUsers.isOtherAppAdmin(txtUser.Text, Session("CompanyId"), "eProcure", lstItem.Value) Then
        '            psErrMsg &= "<LI type=square>User is not allowed to carry multiple admin role in the system. Please select another user group.<br>"
        '        End If
        '    Next

        '    objUsers = Nothing

        '    If txtAppLimit.Text = "" Then
        '        psErrMsg &= "<LI type=square>PR Approval limit is required.<br>"
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
        '        psErrMsg &= "<LI type=square>PO Approval Limit must > PREVIOUS approval limit in: " & _
        '                    AppGrpName(lGrpIndex) & ".<br>"
        '    End If

        '    If Not ChkLimit("PO", "N") Then
        '        psErrMsg &= "<LI type=square>PO Approval Limit must < NEXT approval Limit in: " & _
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

        If Not objUsers.IsTieToAnyUserGrp(Session("CompanyId"), txtUser.Text, "eProcure") And lstUGSelected.Items.Count <= 0 Then
            'Michelle (4/5/2012) - Issue 1604
            'psErrMsg &= "<LI type=square>User group is required.<br>"
            If Not objUsers.checkUserFixedRole("'SUPER ADMIN'", txtUser.Text) Then
                psErrMsg &= "<LI type=square>User group is required.<br>"
            End If
        End If
        Dim lstItem As ListItem
        For Each lstItem In lstUGSelected.Items
            If objUsers.isOtherAppAdmin(txtUser.Text, Session("CompanyId"), "eProcure", lstItem.Value) Then
                psErrMsg &= "<LI type=square>User is not allowed to carry multiple admin role in the system. Please select another user group.<br>"
            End If
        Next

        objUsers = Nothing

        blnPRLimit = True
        If txtAppLimit.Text = "" Then
            psErrMsg &= "<LI type=square>PR Approval limit is required.<br>"
        Else
            If IsNumeric(txtAppLimit.Text) Then
                If Not IsCost(txtAppLimit.Text) Then
                    'Michelle (20/9/2010) - Replace the error message
                    'psErrMsg &= "<LI type=square>Approval limit is expecting currency.<br>"
                    psErrMsg &= "<LI type=square>Maximum 2 decimal places for approval limit.<br>"
                End If
            Else
                psErrMsg &= "<LI type=square>Invalid PR approval limit.<br>"
                blnPRLimit = False
            End If
            
        End If

        blnPOLimit = True
        If txtPOAppLimit.Text = "" Then
            psErrMsg &= "<LI type=square>PO Approval limit is required.<br>"
        Else
            If IsNumeric(txtPOAppLimit.Text) Then
                If Not IsCost(txtPOAppLimit.Text) Then
                    'Michelle (20/9/2010) - Replace the error message
                    'psErrMsg &= "<LI type=square>Approval limit is expecting currency.<br>"
                    psErrMsg &= "<LI type=square>Maximum 2 decimal places for approval limit.<br>"
                End If
            Else
                psErrMsg &= "<LI type=square>Invalid PO approval limit.<br>"
                blnPOLimit = False
            End If
            
        End If

        'To check whether the PR App Limit is <= to previous approving officer an all the approval he is involved in

        If blnPRLimit = True Then
            If Not ChkLimit("PR", "P") Then
                psErrMsg &= "<LI type=square>PR Approval Limit must > PREVIOUS approval limit in: " & _
                            AppGrpName(lGrpIndex) & ".<br>"
            End If

            'To check whether the PR App Limit is > to previous approving officer an all the approval he is involved in
            If Not ChkLimit("PR", "N") Then
                psErrMsg &= "<LI type=square>PR Approval Limit must < NEXT approval Limit in: " & _
                            AppGrpName(lGrpIndex) & ".<br>"
            End If
        End If
        
        If blnPOLimit = True Then
            If Not ChkLimit("PO", "P") Then
                psErrMsg &= "<LI type=square>PO Approval Limit must > PREVIOUS approval limit in: " & _
                            AppGrpName(lGrpIndex) & ".<br>"
            End If

            If Not ChkLimit("PO", "N") Then
                psErrMsg &= "<LI type=square>PO Approval Limit must < NEXT approval Limit in: " & _
                            AppGrpName(lGrpIndex) & ".<br>"
            End If
        End If
        
        blnInvLimit = True
        If txtInvAppLimit.Text = "" Then
            psErrMsg &= "<LI type=square>Invoice approval limit is required.<br>"
        Else
            If IsNumeric(txtInvAppLimit.Text) Then
                If Not IsCost(txtInvAppLimit.Text) Then
                    'Michelle (20/9/2010) - Replace the error message
                    'psErrMsg &= "<LI type=square>Invoice approval limit is expecting currency.<br>"
                    psErrMsg &= "<LI type=square>Maximum 2 decimal places for Invoice approval limit.<br>"
                End If
            Else
                psErrMsg &= "<LI type=square>Invalid Invoice approval limit.<br>"
                blnInvLimit = False
            End If
        End If

        If blnInvLimit = True Then
            'To check whether the Inv App Limit is <= to previous approving officer an all the approval he is involved in
            If Not ChkLimit("INV", "P") Then
                psErrMsg &= "<LI type=square>Invoice Approval Limit must > PREVIOUS approval limit in: " & _
                            AppGrpName(lGrpIndex) & ".<br>"
            End If

            'To check whether the Inv App Limit is > to previous approving officer an all the approval he is involved in
            If Not ChkLimit("INV", "N") Then
                psErrMsg &= "<LI type=square>Invoice Approval Limit must < NEXT approval limit in: " & _
                            AppGrpName(lGrpIndex) & ".<br>"
            End If
        End If

        If chkSpot.Visible = True Or chkStock.Visible = True Or chkMro.Visible = True Then
            If chkSpot.Checked = False And chkStock.Checked = False And chkMro.Checked = False Then
                psErrMsg &= "<LI type=square>Please assign at least 1 item type to the user.<br>"
            End If
        End If

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
            If AppType = "PR" Or AppType = "PO" Then
                intSeq = tDS.Tables(0).Rows(j).Item("AGA_SEQ")
                lGrpIndex = tDS.Tables(0).Rows(j).Item("AGA_GRP_INDEX")
            Else
                intSeq = tDS.Tables(0).Rows(j).Item("AGFO_SEQ")
                lGrpIndex = tDS.Tables(0).Rows(j).Item("AGFO_GRP_INDEX")
            End If

            If ChkType = "P" Then   'Check for the previous limit
                If intSeq > 1 Then 'If he is the 1st approval, then no need to check
                    If AppType = "PR" Then
                        strsql1 = "Select ifnull(um_app_limit,0) as um_app_limit from approval_grp_ao, user_mstr, approval_grp_mstr " & _
                                   " where aga_grp_index = " & lGrpIndex & " and aga_seq = " & intSeq - 1 & _
                                   " AND UM_STATUS ='A' AND UM_DELETED='N' AND UM_COY_ID ='" & Session("CompanyId") & "'" & _
                                   " and um_user_id = aga_ao" & _
                                   " and aga_grp_index = agm_grp_index and agm_type = 'PR' and agm_coy_id = '" & Session("CompanyId") & "'"

                    ElseIf AppType = "PO" Then
                        strsql1 = "Select ifnull(um_po_app_limit,0) as um_po_app_limit, 0 as um_po_app_limit from approval_grp_ao, user_mstr, approval_grp_mstr " & _
                                   " where aga_grp_index = " & lGrpIndex & " and aga_seq = " & intSeq - 1 & _
                                   " AND UM_STATUS ='A' AND UM_DELETED='N' AND UM_COY_ID ='" & Session("CompanyId") & "'" & _
                                   " and um_user_id = aga_ao" & _
                                   " and aga_grp_index = agm_grp_index and agm_type = 'PO' and agm_coy_id = '" & Session("CompanyId") & "'"
                    Else
                        strsql1 = "Select ifnull(um_invoice_app_limit,0) as um_invoice_app_limit from approval_grp_fo, user_mstr, approval_grp_mstr " & _
                                  " where agfo_grp_index = " & lGrpIndex & " and agfo_seq = " & intSeq - 1 & _
                                  " AND UM_STATUS ='A' AND UM_DELETED='N' AND UM_COY_ID ='" & Session("CompanyId") & "'" & _
                                  " and um_user_id = agfo_fo" & _
                                  " and agfo_grp_index = agm_grp_index and agm_type = 'INV' and agm_coy_id = '" & Session("CompanyId") & "'"
                    End If

                    Dim temp As String
                    temp = objDb.GetVal(strsql1)
                    If temp = "" Then
                        decAppLim = 0
                    Else
                        decAppLim = CDec(temp)
                    End If
                    'decAppLim = objDb.GetVal(strsql1)
                    If decComLim < decAppLim Then
                        Return False
                    End If
                End If
            Else 'Check for the next limit
                intCnt = 0
                If AppType = "PR" Then
                    '  intCnt = objDb.GetCount("approval_grp_ao, user_mstr", " where aga_grp_index = " & lGrpIndex & " and aga_seq = " & intSeq + 1 & " AND UM_STATUS ='A' AND UM_DELETED='N' AND UM_COY_ID ='" & Session("CompanyId") & "' and um_user_id = '" & txtUser.Text & "'")
                    strsql1 = "Select ifnull(um_app_limit,0) as um_app_limit from approval_grp_ao, user_mstr, approval_grp_mstr " & _
                               " where aga_grp_index = " & lGrpIndex & " and aga_seq = " & intSeq + 1 & _
                               " AND UM_STATUS ='A' AND UM_DELETED='N' AND UM_COY_ID ='" & Session("CompanyId") & "'" & _
                               " and um_user_id = aga_ao" & _
                               " and aga_grp_index = agm_grp_index and agm_type = 'PR' and agm_coy_id = '" & Session("CompanyId") & "'"
                ElseIf AppType = "PO" Then
                    '  intCnt = objDb.GetCount("approval_grp_ao, user_mstr", " where aga_grp_index = " & lGrpIndex & " and aga_seq = " & intSeq + 1 & " AND UM_STATUS ='A' AND UM_DELETED='N' AND UM_COY_ID ='" & Session("CompanyId") & "' and um_user_id = '" & txtUser.Text & "'")
                    strsql1 = "Select ifnull(um_po_app_limit,0) as um_po_app_limit from approval_grp_ao, user_mstr, approval_grp_mstr " & _
                               " where aga_grp_index = " & lGrpIndex & " and aga_seq = " & intSeq + 1 & _
                               " AND UM_STATUS ='A' AND UM_DELETED='N' AND UM_COY_ID ='" & Session("CompanyId") & "'" & _
                               " and um_user_id = aga_ao" & _
                               " and aga_grp_index = agm_grp_index and agm_type = 'PO' and agm_coy_id = '" & Session("CompanyId") & "'"
                Else
                    strsql1 = "Select ifnull(um_invoice_app_limit,0) as um_invoice_app_limit from approval_grp_fo, user_mstr, approval_grp_mstr " & _
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
        If objUser.DelUser(ViewState("UserId"), False) Then
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
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usFinVwDept.aspx", "pageid=" & strPageId) & """><span>Finance Viewing Department</span></a></li>" & _
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

    Private Sub cmdadd_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdadd.ServerClick
        ViewState("Mode") = "add"
        txtUser.Enabled = True
        trStockType.Visible = False
        isNotPostBack()
    End Sub
End Class

