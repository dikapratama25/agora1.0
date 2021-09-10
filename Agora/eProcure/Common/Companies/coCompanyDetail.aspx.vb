Imports AgoraLegacy
Imports eProcure.Component

Public Class coCompanyDetail
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents Label17 As System.Web.UI.WebControls.Label
    Protected WithEvents txtCoyID As System.Web.UI.WebControls.TextBox
    Protected WithEvents rfv_txtCoyID As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label16 As System.Web.UI.WebControls.Label
    Protected WithEvents txtCoyName As System.Web.UI.WebControls.TextBox
    Protected WithEvents rfv_txtCoyName As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rdStatus As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents Label15 As System.Web.UI.WebControls.Label
    Protected WithEvents cboCompType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboParentCoy As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboLicensePackage As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtUserLicense As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSubStart As System.Web.UI.WebControls.TextBox
    Protected WithEvents rfv_txtSubStart As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents txtSubEnd As System.Web.UI.WebControls.TextBox
    Protected WithEvents rfv_txtSubEnd As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents txtAddress As System.Web.UI.WebControls.TextBox
    Protected WithEvents rfv_address As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents txtAddress2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAddress3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents txtCity As System.Web.UI.WebControls.TextBox
    Protected WithEvents rfv_city As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents cbostate As System.Web.UI.WebControls.DropDownList
    Protected WithEvents rfv_state As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents txtPostCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents rev_postcode As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents rfv_postcode As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents cbocountry As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlYear As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlCurrency As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlOwnership As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlBusiness As System.Web.UI.WebControls.DropDownList
    ' Protected WithEvents ddlCommodity As System.Web.UI.WebControls.DropDownList
    Protected WithEvents rfv_country As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label8 As System.Web.UI.WebControls.Label
    Protected WithEvents txtPhone As System.Web.UI.WebControls.TextBox
    Protected WithEvents rfv_phone As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label9 As System.Web.UI.WebControls.Label
    Protected WithEvents txtFax As System.Web.UI.WebControls.TextBox
    Protected WithEvents rfv_fax As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label10 As System.Web.UI.WebControls.Label
    Protected WithEvents txtEmail As System.Web.UI.WebControls.TextBox
    Protected WithEvents rev_email As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents rfv_email As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label11 As System.Web.UI.WebControls.Label
    Protected WithEvents txtBusinessRegNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCoyLongName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtWebSites As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPaid As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtOwnership As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtOrgCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents rfv_BRegNo As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents txtAccountNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBankCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBankName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtTrans As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBranchCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents cbocurrency As System.Web.UI.WebControls.DropDownList
    Protected WithEvents rfv_currency As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Image1 As System.Web.UI.WebControls.Image
    Protected WithEvents lblLogo As System.Web.UI.WebControls.Label
    Protected WithEvents lblComLogo As System.Web.UI.WebControls.Label
    Protected WithEvents lnkTC As System.Web.UI.WebControls.HyperLink
    Protected WithEvents txtGSTRegNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblDayOfLastStatus As System.Web.UI.WebControls.Label
    Protected WithEvents rdGSTCalcBy As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents rdBCM As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents rdFinMode As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents Label12 As System.Web.UI.WebControls.Label
    Protected WithEvents cbo_PayTerm As System.Web.UI.WebControls.DropDownList
    Protected WithEvents PayTerm As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label13 As System.Web.UI.WebControls.Label
    Protected WithEvents cbo_PayMeth As System.Web.UI.WebControls.DropDownList
    Protected WithEvents PayMethod As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label14 As System.Web.UI.WebControls.Label
    Protected WithEvents txt_PwdDur As System.Web.UI.WebControls.TextBox
    Protected WithEvents PwdDur As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents revPwdDur As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents rdPrivateLbl As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents rdSkin As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents chktraining As System.Web.UI.WebControls.CheckBox
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents cmdsave As System.Web.UI.WebControls.Button
    Protected WithEvents vldsumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents Hubadmin1 As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents cmdCompLogo As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents cmdTCBrow As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents tdVendor As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Hubadmin2 As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents trPay As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Hubadmin3 As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents PreTemp As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hdn_pageid As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lblAttach As System.Web.UI.WebControls.Label
    Protected WithEvents txtReportUser As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBCMSetting As System.Web.UI.WebControls.TextBox
    Protected WithEvents rdInvAppr As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents rdInvMultiAppr As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents rdBA As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents Image2 As System.Web.UI.WebControls.Image
    Protected WithEvents txtCommodity As System.Web.UI.WebControls.TextBox
    Protected WithEvents hidCommodity As System.Web.UI.HtmlControls.HtmlInputHidden

   
    
   
    Dim objFile As New FileManagement


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
      
        cmdSave.Enabled = False
        Dim alButtonList As ArrayList
        If viewstate("Mode") = "modify" Then
            alButtonList = New ArrayList
            alButtonList.Add(cmdSave)
            htPageAccess.Add("update", alButtonList)
            alButtonList = New ArrayList
            'no add button in buyer hub
        End If
        lblComLogo.Text = "Recommended dimension is 130(W) x 70(H) pixels."
        CheckButtonAccess()

        'cmdClear.Enabled = blnCanAdd Or blnCanUpdate Or blnCanDelete
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        strNewCSS = "both"
        MyBase.Page_Load(sender, e)
        viewstate("Mode") = Request.Params("mode")


        If Not IsPostBack Then
            isNotPostBack()
            GenerateTab()
        End If
        cmdReset.CausesValidation = False
        cmdsave.Attributes.Add("onclick", "return checkFileType('img','" & cmdCompLogo.ClientID & "', 'doc','" & cmdTCBrow.ClientID & "');")
        hdn_pageid.Value = strPageId
        Image2.ImageUrl = dDispatcher.direct("Plugins/images", "collapse_up.gif")
    End Sub

    Private Sub isNotPostBack()
        vldsumm.ShowSummary = False
        Dim ii_ddl, ii_ddl2, jj_ddl As Integer
        ii_ddl2 = 0
        jj_ddl = Year(Date.Now)
        For ii_ddl = jj_ddl To (jj_ddl - 100) Step -1

            ddlYear.Items.Insert(ii_ddl2, New ListItem(ii_ddl))

            ii_ddl2 = ii_ddl2 + 1
        Next
        Dim lstItem As New ListItem
        lstItem.Value = ""
        lstItem.Text = "---Select---"
        ddlYear.Items.Insert(0, lstItem)

        If ViewState("Mode") = "add" Then
            clearTextBox()
            PopulateCodeTable()
            lblHeader.Text = "Add company"
        Else
            PopulateCodeTable()
            txtCoyID.Enabled = False
            Populate()
            lblHeader.Text = "Modify company"
        End If

        'ddlOwnership.Attributes.Add("onchange", "Change()")

    End Sub

    Private Sub clearTextBox()
        Me.txt_PwdDur.Text = ""
        Me.txtAccountNo.Text = ""
        Me.txtAddress.Text = ""
        Me.txtAddress2.Text = ""
        Me.txtAddress3.Text = ""
        Me.txtBankCode.Text = ""
        Me.txtBankName.Text = ""
        Me.txtTrans.Text = ""
        Me.txtBranchCode.Text = ""
        Me.txtBusinessRegNo.Text = ""
        Me.txtCity.Text = ""
        'Me.txtCompanyLogo.Text = ""
        Me.txtCoyID.Text = ""
        Me.txtCoyName.Text = ""
        Me.txtEmail.Text = ""
        Me.txtFax.Text = ""
        Me.txtGSTRegNo.Text = ""
        Me.txtPhone.Text = ""
        Me.txtPostCode.Text = ""
        Me.txtSubEnd.Text = ""
        Me.txtSubStart.Text = ""
        Me.txtSubStart.Text = ""
        Me.txtUserLicense.Text = ""
        Me.txtReportUser.Text = ""


        Me.rdStatus.SelectedValue = "A"
        Me.cboCompType.SelectedIndex = 0
        Me.rdPrivateLbl.SelectedValue = "N"
        Me.rdSkin.SelectedValue = "1"
    End Sub

    Private Sub PopulateCodeTable()
        Dim objGlobal As New AppGlobals

        objGlobal.FillCompany(cboParentCoy, "Ehub")
        objGlobal.FillCodeTable(cbocurrency, CodeTable.Currency)
        objGlobal.FillCodeTable(cbostate, CodeTable.State)
        objGlobal.FillCodeTable(cbocountry, CodeTable.Country)
        objGlobal.FillCodeTable(cbo_PayTerm, CodeTable.PaymentTerm)
        objGlobal.FillCodeTable(cbo_PayMeth, CodeTable.PaymentMethod)
        objGlobal.FillCodeTable(ddlCurrency, CodeTable.Currency)
        objGlobal.FillCodeTable(ddlOwnerShip, CodeTable.OwnerShip)
        objGlobal.FillCodeTable(ddlBusiness, CodeTable.Business)
        'objGlobal.FillCommodityType(ddlCommodity)

    End Sub
    'Private Sub ShowHubInfo(ByVal pShow As Boolean)
    '    If pShow Then
    '        Hubadmin1.Style.Item("Display") = ""
    '        Hubadmin2.Style.Item("Display") = ""
    '        Hubadmin3.Style.Item("Display") = ""
    '        cboLicensePackage.Enabled = True
    '        cboLicenseUser.Enabled = True
    '        txtSubStart.Enabled = True
    '        txtSubEnd.Enabled = True
    '        txtCoyID.Enabled = True
    '    End If
    'End Sub
    Private Sub Populate()
        Dim objComp As New Companies
        Dim objCompDetails As New Company
        Dim objDB As New EAD.DBCom

        objCompDetails = objComp.GetCompanyDetails(Session("CompanyId"))
        If Not objCompDetails Is Nothing Then
            txtCoyID.Text = objCompDetails.CoyId
            txtCoyName.Text = objCompDetails.CoyName
            txtAccountNo.Text = objCompDetails.AccountNo
            txtBankCode.Text = objCompDetails.BankCode
            txtBankName.Text = objCompDetails.BankName
            txtTrans.Text = objCompDetails.TransNo
            txtBranchCode.Text = objCompDetails.BranchCode
            txtAddress.Text = objCompDetails.Address1
            txtAddress2.Text = objCompDetails.Address2
            txtAddress3.Text = objCompDetails.Address3
            txtCity.Text = objCompDetails.City
            txtPostCode.Text = objCompDetails.PostCode
            txtPhone.Text = objCompDetails.Phone
            txtFax.Text = objCompDetails.Fax
            txtEmail.Text = objCompDetails.Email
            'txtCompanyLogo.Text = objCompDetails.CoyLogo
            txtBusinessRegNo.Text = objCompDetails.BusinessRegNo
            txtGSTRegNo.Text = objCompDetails.TaxRegNo
            txt_PwdDur.Text = objCompDetails.PwdDuration
            'Michelle (20/9/2010) - To remove the time
            txtSubStart.Text = Left(objCompDetails.SubStart, 10)
            txtSubEnd.Text = Left(objCompDetails.SubEnd, 10)
            txtBusinessRegNo.Text = objCompDetails.BusinessRegNo
            txtGSTRegNo.Text = objCompDetails.TaxRegNo
            'txtDtLastStatus.Text = Format(objCompDetails.GSTDateLastStatus, "dd/MM/yyyy")
            If objCompDetails.GSTDateLastStatus = "" Then
                lblDayOfLastStatus.Text = ""
            Else
                Dim intDays As Integer = Date.Now.Subtract(CDate(objCompDetails.GSTDateLastStatus)).Days
                lblDayOfLastStatus.Text = Format(CDate(objCompDetails.GSTDateLastStatus), "dd/MM/yyyy") & " (" & intDays & " Days Lapsed)"
            End If
            'txtTC.Text = objCompDetails.TC
            txtUserLicense.Text = objCompDetails.LicenseUsers
            txtReportUser.Text = objCompDetails.ReportUsers

            rdStatus.SelectedValue = objCompDetails.Status
            rdGSTCalcBy.SelectedValue = objCompDetails.TaxCalBy
            txtBCMSetting.Text = IIf(objCompDetails.BCMSetting = "0", "3", objCompDetails.BCMSetting)
            rdBCM.SelectedValue = IIf(objCompDetails.BCMSetting = "0", 0, 1)
            rdFinMode.SelectedValue = objCompDetails.FinDeptMode
            rdInvAppr.SelectedValue = objCompDetails.InvAppr

            rdPrivateLbl.SelectedValue = objCompDetails.PrivLabeling
            rdSkin.SelectedValue = objCompDetails.Skins
            chktraining.Checked = IIf(objCompDetails.TrainDemo = "Y", True, False)
            '----- New Code Adding for 1 PR to Multiple POs on 16.08.2007 By Praveen
            rdInvMultiAppr.SelectedValue = objCompDetails.MultiInvAppr
            rdBA.SelectedValue = objCompDetails.BACanPO
            '---End  The Code

            txtCoyLongName.Text = objCompDetails.CoyLongName
            txtWebSites.Text = objCompDetails.WebSites
            If Not IsDBNull(objCompDetails.PaidUpCapital) And objCompDetails.PaidUpCapital <> "" Then
                txtPaid.Text = Format(CDbl(objCompDetails.PaidUpCapital), "#0.00") 'CStr(objCompDetails.PaidUpCapital)
            End If
            txtOwnership.Text = objCompDetails.OwnershipOthers
            txtOrgCode.Text = objCompDetails.OrgCode

            Common.SelDdl(Common.parseNull(objCompDetails.Country), cbocountry, True, True)
            '' ''If objCompDetails.Country <> "MY" Then
            '' ''    Dim objGlobal As New AppGlobals
            '' ''    objGlobal.FillState(cbostate, cbocountry.SelectedItem.Value)
            '' ''    objGlobal = Nothing
            '' ''End If

            Dim objGlobal As New AppGlobals
            objGlobal.FillState(cbostate, cbocountry.SelectedItem.Value)
            objGlobal = Nothing

            Common.SelDdl(Common.parseNull(objCompDetails.State), cbostate, True, True)
            Common.SelDdl(Common.parseNull(objCompDetails.Currency), cbocurrency, True, True)

            'Jules 2018.11.07 - To cater for vendors without default values.
            If Common.parseNull(objCompDetails.PaymentTerm) <> "" Then
                Common.SelDdl(Common.parseNull(objCompDetails.PaymentTerm), cbo_PayTerm, True, True)
            End If

            If Common.parseNull(objCompDetails.PaymentMethod) <> "" Then
                Common.SelDdl(Common.parseNull(objCompDetails.PaymentMethod), cbo_PayMeth, True, True)
            End If
            'End modification.

            Common.SelDdl(Common.parseNull(objCompDetails.ParentCoy), cboParentCoy, True, True)
            Common.SelDdl(Common.parseNull(objCompDetails.CoyType), cboCompType, True, True)

            Common.SelDdl(Common.parseNull(objCompDetails.RegCurrency), ddlCurrency, True, True)
            Common.SelDdl(Common.parseNull(objCompDetails.Ownership), ddlOwnership, True, True)
            Common.SelDdl(Common.parseNull(objCompDetails.Business), ddlBusiness, True, True)
            If Not IsDBNull(objCompDetails.Commodity) And objCompDetails.Commodity <> "" Then
                hidCommodity.Value = objCompDetails.Commodity
                Dim CommName As String = objDB.GetVal("SELECT CT_NAME FROM COMMODITY_TYPE WHERE CT_ID = '" & hidCommodity.Value & "'")
                txtCommodity.Text = CommName
                '           ddlCommodity.SelectedItem = objCompDetails.Commodity
                'Common.SelDdl(Common.parseNull(objCompDetails.Commodity), ddlCommodity, True, True)
            End If

            If Not IsDBNull(objCompDetails.RegYear) Then ddlYear.SelectedValue = objCompDetails.RegYear
            If ddlOwnership.SelectedItem.Text = "Others" Then
                txtOwnership.Enabled = True
            Else
                txtOwnership.Text = ""
                txtOwnership.Enabled = True
            End If
            If objCompDetails.Package <> "" Then
                cboLicensePackage.Items.Add(objCompDetails.Package)
            Else
                cboLicensePackage.Items.Add("---Select---")
            End If

            If objCompDetails.CoyType = "VENDOR" Then
                'tdVendor.Style.Item("Display") = ""
                trPay.Style.Item("Display") = "none"
                PreTemp.Value = "Preview Invoice Template"
                ViewState("Side") = objCompDetails.CoyType
            ElseIf objCompDetails.CoyType = "BUYER" Then
                trPay.Style.Item("Display") = ""
                tdVendor.Style.Item("Display") = "none"
                ViewState("CoyLogo") = objCompDetails.CoyLogo
                ViewState("Side") = objCompDetails.CoyType
            ElseIf objCompDetails.CoyType = "BOTH" Then
                'tdVendor.Style.Item("Display") = ""
                trPay.Style.Item("Display") = ""
                ViewState("Side") = objCompDetails.CoyType
            End If

            DislayLogo()
            ViewState("CoyLogo") = objCompDetails.CoyLogo
            ViewState("Actual_TC") = objCompDetails.Actual_TC
            ViewState("TC") = objCompDetails.TC

            ' strURL = "<A HREF=../FileDownload.aspx?file=" & Server.UrlEncode(strFile1) & "&actualfile=" & Server.UrlEncode(strFile) & "&type=" & EnumDownLoadType.DocAttachment & "&doctype=PR>" & strFile & "</A>"
            If objCompDetails.Actual_TC <> "" Then
                lnkTC.Visible = True
                'lnkTC.Text = objCompDetails.Actual_TC
                '*************************meilai 25/2/05****************************
                'lnkTC.NavigateUrl = "../FileDownLoad.aspx?file=" & Server.UrlEncode(objCompDetails.TC) & "&actualfile=" & Server.UrlEncode(objCompDetails.Actual_TC) & "&type=" & EnumDownLoadType.TermAndCond
                lnkTC.Text = objFile.getAttachPath(Server.UrlEncode(objCompDetails.Actual_TC), objCompDetails.Actual_TC, Server.UrlEncode(objCompDetails.TC), EnumDownLoadType.TermAndCond, "", EnumUploadFrom.FrontOff)
                '*************************meilai************************************
            Else
                lnkTC.Visible = False
            End If
        End If
    End Sub

    Private Function GetInput() As Company
        Dim objCompDetails As New Company
        objCompDetails.CoyId = txtCoyID.Text
        objCompDetails.CoyName = txtCoyName.Text
        objCompDetails.AccountNo = txtAccountNo.Text
        objCompDetails.BankCode = txtBankCode.Text
        objCompDetails.BankName = txtBankName.Text
        objCompDetails.TransNo = txtTrans.Text
        objCompDetails.BranchCode = txtBranchCode.Text
        objCompDetails.Address1 = txtAddress.Text
        objCompDetails.Address2 = txtAddress2.Text
        objCompDetails.Address3 = txtAddress3.Text
        objCompDetails.City = txtCity.Text
        objCompDetails.PostCode = txtPostCode.Text
        objCompDetails.Phone = txtPhone.Text
        objCompDetails.Fax = txtFax.Text
        objCompDetails.Email = txtEmail.Text
        'objCompDetails.CoyLogo = txtCompanyLogo.Text
        objCompDetails.BusinessRegNo = txtBusinessRegNo.Text
        objCompDetails.TaxRegNo = txtGSTRegNo.Text
        objCompDetails.PwdDuration = txt_PwdDur.Text
        objCompDetails.SubStart = txtSubStart.Text
        objCompDetails.SubEnd = txtSubEnd.Text
        objCompDetails.BusinessRegNo = txtBusinessRegNo.Text
        objCompDetails.TaxRegNo = txtGSTRegNo.Text
        'objCompDetails.TC = txtTC.Text
        objCompDetails.CoyLongName = txtCoyLongName.Text
        objCompDetails.WebSites = txtWebSites.Text
        objCompDetails.RegYear = ddlYear.SelectedValue
        objCompDetails.RegCurrency = ddlCurrency.SelectedValue
        objCompDetails.PaidUpCapital = txtPaid.Text
        objCompDetails.Ownership = ddlOwnership.SelectedValue
        objCompDetails.OwnershipOthers = txtOwnership.Text
        objCompDetails.Business = ddlBusiness.SelectedValue
        objCompDetails.Commodity = hidCommodity.Value
        objCompDetails.OrgCode = txtOrgCode.Text

        objCompDetails.Status = rdStatus.SelectedValue
        objCompDetails.TaxCalBy = rdGSTCalcBy.SelectedValue
        If rdBCM.SelectedValue = "0" Then
            objCompDetails.BCMSetting = rdBCM.SelectedValue
        Else
            objCompDetails.BCMSetting = txtBCMSetting.Text
        End If
        objCompDetails.FinDeptMode = rdFinMode.SelectedValue
        objCompDetails.InvAppr = rdInvAppr.SelectedValue
        objCompDetails.PrivLabeling = rdPrivateLbl.SelectedValue
        objCompDetails.Skins = rdSkin.SelectedValue
        objCompDetails.TrainDemo = IIf(chktraining.Checked, "Y", "N")

        objCompDetails.LicenseUsers = txtUserLicense.Text
        objCompDetails.ReportUsers = IIf(txtReportUser.Text = "", "0", txtReportUser.Text)
        objCompDetails.Country = cbocountry.SelectedValue
        objCompDetails.State = cbostate.SelectedValue
        objCompDetails.Currency = cbocurrency.SelectedValue
        objCompDetails.PaymentTerm = cbo_PayTerm.SelectedValue
        objCompDetails.PaymentMethod = cbo_PayMeth.SelectedValue
        objCompDetails.ParentCoy = cboParentCoy.SelectedValue
        objCompDetails.CoyType = cboCompType.SelectedValue
        '-----New code for Adding 1PR To Multiple POs by Praveen on 16.08.2007
        objCompDetails.MultiInvAppr = rdInvMultiAppr.SelectedValue
        objCompDetails.BACanPO = rdBA.SelectedValue
        '---End  The Code 

        If cboLicensePackage.SelectedValue <> "---Select---" Then
            objCompDetails.Package = cboLicensePackage.SelectedValue
        End If

        '//Added By Moo, for company Logo
        'ViewState("CoyLogo") = objCompDetails.CoyLogo
        'ViewState("Actual_TC") = objCompDetails.Actual_TC
        'ViewState("TC") = objCompDetails.TC
        Dim objFile As New FileManagement
        Dim strFile As String
        If cmdCompLogo.PostedFile.FileName <> "" Then

            Dim objDB As New EAD.DBCom
            Dim dblMaxFileSize As Double = Val(objDB.GetVal("SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_TYPE = 'PR' AND SP_PARAM_NAME = 'ImageAttachSize'"))

            Dim sFileName As String
            sFileName = System.IO.Path.GetFileName(cmdCompLogo.PostedFile.FileName)

            If Len(sFileName) > 50 Then
                Throw New Exception("File name exceeds 46 character")
            ElseIf cmdCompLogo.PostedFile.ContentLength > 0 And cmdCompLogo.PostedFile.ContentLength / 1024 <= dblMaxFileSize Then
                strFile = objFile.uploadCoyLogo(cmdCompLogo, EnumUploadFrom.FrontOff, Session("CompanyId"))
                objCompDetails.CoyLogo = strFile
                ViewState("CoyLogo") = objCompDetails.CoyLogo
            ElseIf cmdCompLogo.PostedFile.ContentLength = 0 Then
                Throw New Exception("0 byte document or file not found")
            Else
                Throw New Exception("File exceeds maximum file size")
            End If
        Else
            objCompDetails.CoyLogo = ViewState("CoyLogo")
        End If

        Dim strActualTC, strHubTC As String
        'strFile = objFile.FileUpload(cmdTCBrow, EnumUploadType.TermAndCond, "", EnumUploadFrom.FrontOff, "")
        If cmdTCBrow.PostedFile.FileName <> "" Then

            Dim objDB As New EAD.DBCom
            'Dim dblMaxFileSize As Double = Val(objDB.GetVal("SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_TYPE = 'PR' AND SP_PARAM_NAME = 'DocAttachSize'"))

            Dim dblMaxFileSize As Double = Session("FileSize")

            Dim sFileName As String
            sFileName = System.IO.Path.GetFileName(cmdTCBrow.PostedFile.FileName)

            If Len(sFileName) > 50 Then
                Throw New Exception("File name exceeds 46 character")
            ElseIf cmdTCBrow.PostedFile.ContentLength > 0 And cmdTCBrow.PostedFile.ContentLength / 1024 <= dblMaxFileSize Then
                objFile.uploadCoyTC(cmdTCBrow, EnumUploadFrom.FrontOff, Session("CompanyId"), strActualTC, strHubTC)
                'lnkTC.Text = strActualTC
                objCompDetails.Actual_TC = strActualTC
                objCompDetails.TC = strHubTC
                ViewState("Actual_TC") = objCompDetails.Actual_TC
                ViewState("TC") = objCompDetails.TC
            ElseIf cmdTCBrow.PostedFile.ContentLength = 0 Then
                Throw New Exception("0 byte document or file not found")
            Else
                Throw New Exception("File exceeds maximum file size")
            End If
        Else
            objCompDetails.Actual_TC = ViewState("Actual_TC")
            objCompDetails.TC = ViewState("TC")
        End If

        If objCompDetails.Actual_TC <> "" Then
            'lnkTC.Text = objCompDetails.Actual_TC
            '**************meilai 25/2/05******************************
            'lnkTC.NavigateUrl = "../FileDownLoad.aspx?file=" & Server.UrlEncode(objCompDetails.TC) & "&actualfile=" & Server.UrlEncode(objCompDetails.Actual_TC) & "&type=" & EnumDownLoadType.TermAndCond
            lnkTC.Text = objFile.getAttachPath(Server.UrlEncode(objCompDetails.Actual_TC), objCompDetails.Actual_TC, Server.UrlEncode(objCompDetails.TC), EnumDownLoadType.TermAndCond, "", EnumUploadFrom.FrontOff)
            '**************meilai**************************************
        End If

        objFile = Nothing
        Return objCompDetails
    End Function

    Private Sub cmdsave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdsave.Click
        vldsumm.ShowSummary = True
        Dim strErrMsg As String
        Dim objComp As New Companies
        Dim objCompDetails As New Company

        If Page.IsValid Then
            Try
                objCompDetails = GetInput()

                'Dim strApp() As String

                If ViewState("Mode") = "add" Then
                    objComp.AddCompany(objCompDetails)
                ElseIf ViewState("Mode") = "modify" Then
                    'objComp.UpdateCompany(objCompDetails, strApp)
                    objComp.UpdateCompany(objCompDetails)
                End If

                DislayLogo()
                Populate()
                Common.NetMsgbox(Me, objComp.Message, MsgBoxStyle.Information)
            Catch ex As Exception
                Common.NetMsgbox(Me, ex.Message)
            End Try
        End If
    End Sub

    Private Sub cbocountry_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbocountry.SelectedIndexChanged
        Dim objGlobal As New AppGlobals
        'If cbocountry.SelectedIndex <> -1 Then-
        objGlobal.FillState(cbostate, cbocountry.SelectedItem.Value)
        'End If
    End Sub

    Sub DislayLogo()
        Dim objFile As New FileManagement
        Dim strImgSrc As String
        strImgSrc = objFile.getCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyId"))
        If strImgSrc <> "" Then
            Image1.Visible = True
            Image1.ImageUrl = strImgSrc
            lblLogo.Visible = False
        Else
            Image1.Visible = False
            lblLogo.Visible = True
        End If
        objFile = Nothing
    End Sub

    Private Sub cmdReset_ServerClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.ServerClick
        isNotPostBack()
    End Sub
    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        If ViewState("Side") = "BUYER" Then
            '    Session("w_CompanyDet_tabs") = "<div class=""t_entity""><ul>" & _
            '    "<li><div class=""space""></div></li>" & _
            '                     "<li><a class=""t_entity_btn_selected"" href=""coCompanyDetail.aspx?mode=modify&pageid=" & strPageId & """><span>Company Details</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                     "<li><a class=""t_entity_btn"" href=""../Admin/BComParam.aspx?side=BUYER&pageid=" & strPageId & """><span>Parameters</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                     "<li><a class=""t_entity_btn"" href=""../Admin/AddressMaster.aspx?side=BUYER&type=D&mod=T&pageid=" & strPageId & """><span>Delivery Address</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                     "<li><a class=""t_entity_btn"" href=""../Admin/AddressMaster.aspx?side=BUYER&type=B&mod=T&pageid=" & strPageId & """><span>Billing Address</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                     "<li><a class=""t_entity_btn"" href=""../Admin/DeptSetup.aspx?side=BUYER&pageid=" & strPageId & """><span>Department</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                     "</ul><div></div></div>"
            'Else
            '    Session("w_CompanyDet_tabs") = "<div class=""t_entity""><ul>" & _
            '    "<li><div class=""space""></div></li>" & _
            '                     "<li><a class=""t_entity_btn_selected"" href=""coCompanyDetail.aspx?side=VENDOR&mode=modify&pageid=" & strPageId & """><span>Company Details</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                     "<li><a class=""t_entity_btn"" href=""../Admin/BComVendor.aspx?side=VENDOR&pageid=" & strPageId & """><span>Parameters</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                        "<li><a class=""t_entity_btn"" href=""SalesInfo.aspx?side=VENDOR&pageid=" & strPageId & """><span>Sales Info</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                        "<li><a class=""t_entity_btn"" href=""SoftwareApp.aspx?side=VENDOR&pageid=" & strPageId & """><span>Software</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                        "<li><a class=""t_entity_btn"" href=""QualityStd.aspx?side=VENDOR&pageid=" & strPageId & """><span>Quality Standards</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '"</ul><div></div></div>"
            'Session("w_CompanyDet_tabs") = "<div class=""t_entity""><ul>" & _
            '"<li><div class=""space""></div></li>" & _
            '                       "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Companies", "coCompanyDetail.aspx", "side=BUYER&mode=modify" & "&pageid=" & strPageId) & """><span>Company Details</span></a></li>" & _
            '                       "<li><div class=""space""></div></li>" & _
            '                       "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "BComParam.aspx", "side=BUYER" & "&pageid=" & strPageId) & """><span>Parameters</span></a></li>" & _
            '                       "<li><div class=""space""></div></li>" & _
            '                       "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=D&mod=T" & "&pageid=" & strPageId) & """><span>Delivery Address</span></a></li>" & _
            '                       "<li><div class=""space""></div></li>" & _
            '                       "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=B&mod=T" & "&pageid=" & strPageId) & """><span>Billing Address</span></a></li>" & _
            '                       "<li><div class=""space""></div></li>" & _
            '                       "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "DeptSetup.aspx", "side=BUYER" & "&pageid=" & strPageId) & """><span>Department</span></a></li>" & _
            '                       "<li><div class=""space""></div></li>" & _
            '     "</ul><div></div></div>"
            Session("w_CompanyDet_tabs") = "<div class=""t_entity""><ul>" & _
                        "<li><div class=""space""></div></li>" & _
                       "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Companies", "coCompanyDetail.aspx", "side=BUYER&mode=modify" & "&pageid=" & strPageId) & """><span>Company Details</span></a></li>" & _
                       "<li><div class=""space""></div></li>" & _
                       "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "BComParam.aspx", "side=BUYER" & "&pageid=" & strPageId) & """><span>Parameters</span></a></li>" & _
                       "<li><div class=""space""></div></li>" & _
                       "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=D&mod=T" & "&pageid=" & strPageId) & """><span>Delivery Address</span></a></li>" & _
                       "<li><div class=""space""></div></li>" & _
                       "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=B&mod=T" & "&pageid=" & strPageId) & """><span>Billing Address</span></a></li>" & _
                       "<li><div class=""space""></div></li>" & _
                       "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "DeptSetup.aspx", "side=BUYER" & "&pageid=" & strPageId) & """><span>Department</span></a></li>" & _
                       "<li><div class=""space""></div></li>" & _
     "</ul><div></div></div>"
        Else
            Session("w_CompanyDet_tabs") = "<div class=""t_entity""><ul>" & _
                             "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Companies", "coCompanyDetail.aspx", "side=VENDOR&mode=modify" & "&pageid=" & strPageId) & """><span>Company Details</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "BComVendor.aspx", "side=VENDOR" & "&pageid=" & strPageId) & """><span>Parameters</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "SalesInfo.aspx", "side=VENDOR" & "&pageid=" & strPageId) & """><span>Sales Info</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "SoftwareApp.aspx", "side=VENDOR" & "&pageid=" & strPageId) & """><span>Software</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "QualityStd.aspx", "side=VENDOR" & "&pageid=" & strPageId) & """><span>Quality Standards</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
        "</ul><div></div></div>"

        End If
    End Sub


    'Private Sub ddlOwnership_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOwnership.SelectedIndexChanged
    '    If ddlOwnership.SelectedItem.Text = "Others" Then
    '        txtOwnership.Enabled = True
    '    Else
    '        txtOwnership.Enabled = False
    '        txtOwnership.Text = ""
    '    End If
    'End Sub
End Class
