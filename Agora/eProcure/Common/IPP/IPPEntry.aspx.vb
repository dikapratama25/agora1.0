Imports AgoraLegacy
Imports eProcure.Component
Imports System.Text.RegularExpressions

Imports System.Drawing
Imports System.Windows.Forms.MessageBox

Public Class IPPEntry
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim dsDoc As New DataSet
    Dim aryDoc As New ArrayList
    Dim dsIPPDoc As New DataSet
    Dim objDB As New EAD.DBCom
    Dim objGlobal As New AppGlobals
    Dim objDoc As New IPPMain
    Dim dsIPPDocDetails As New DataSet
    Dim strMsg As String
    Dim strCoyType As String
    Dim isResident As Boolean = False
    Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID") 'Modified for IPP GST Stage 2A - CH - 5 Feb 2015

    Protected WithEvents txtTemp As System.Web.UI.HtmlControls.HtmlInputText
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ddlDocType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlPaymentType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlCurrency As System.Web.UI.WebControls.DropDownList
    Protected WithEvents rdbWHTComp As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rdbWHTVendor As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rdbNoWHT As System.Web.UI.WebControls.RadioButton
    Protected WithEvents txtNoWHTReason As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtWHT As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVendor As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnGetAdd As System.Web.UI.WebControls.Button
    Protected WithEvents btnSelCostAlloc As System.Web.UI.WebControls.Button
    Protected WithEvents txtVenAddL1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVenAddL2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVenAddL3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVenAddPostcode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVenAddCity As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVenAddState As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVenAddCountry As System.Web.UI.WebControls.TextBox
    Protected WithEvents hidButton As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hidButton1 As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents btnremoveline As System.Web.UI.WebControls.Button
    Protected WithEvents btnremoveSubDocLine As System.Web.UI.WebControls.Button
    Protected WithEvents btnremoveSubDoc As System.Web.UI.WebControls.Button
    Protected WithEvents hidbtnremoveSubDoc As System.Web.UI.WebControls.Button
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents btneditline As System.Web.UI.WebControls.Button
    Protected WithEvents lblPaymentMethod As System.Web.UI.WebControls.Label
    Protected WithEvents txtDocNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPaymentAmt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDocDate As System.Web.UI.HtmlControls.HtmlInputText
    Protected WithEvents txtPONo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtIntRemark As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtLateReason As System.Web.UI.WebControls.TextBox
    Protected WithEvents hidLateSubmit As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidlineremove As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button
    Protected WithEvents cmdAddLine As System.Web.UI.WebControls.Button
    Protected WithEvents cmdVoid As System.Web.UI.WebControls.Button
    Protected WithEvents hid4 As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidTotal As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents vldsum As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents Image As New System.Web.UI.WebControls.Image
    Protected WithEvents dtgApprvFlow As System.Web.UI.WebControls.DataGrid
    Protected WithEvents hidApprflow As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trLateReason As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents divInvDetail As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents btnContinue As System.Web.UI.WebControls.Button
    Protected WithEvents cmdSubDoc As System.Web.UI.WebControls.Button
    Protected WithEvents divContinue As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents hidBtnContinue As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hidBtnContinue2 As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents btnhidden3 As System.Web.UI.WebControls.Button
    Protected WithEvents hidBtnContinueSubmit As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents btnhidden4 As System.Web.UI.WebControls.Button
    Protected WithEvents dtgInvDetail As System.Web.UI.WebControls.DataGrid
    Protected WithEvents btnhidden5 As System.Web.UI.WebControls.Button
    Protected WithEvents hid5 As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lblBankNameAccountNo As System.Web.UI.WebControls.Label
    Protected WithEvents label8 As System.Web.UI.WebControls.Label
    Protected WithEvents label10 As System.Web.UI.WebControls.Label
    Protected WithEvents btnContinueMultiGL As System.Web.UI.WebControls.Button
    Protected WithEvents hidBtnContinueMultiGL As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents btnhidden6 As System.Web.UI.WebControls.Button
    Protected WithEvents btnhidden9 As System.Web.UI.WebControls.Button
    Protected WithEvents btnEditSubDoc As System.Web.UI.WebControls.Button
    Protected WithEvents cmdMultiGL As System.Web.UI.WebControls.Button
    Protected WithEvents rbtnCoyType As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents rbtnMasterDoc As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents txtPRCSSentDate As System.Web.UI.HtmlControls.HtmlInputText
    Protected WithEvents txtPRCSReceivedDate As System.Web.UI.HtmlControls.HtmlInputText
    Protected WithEvents lblDocDueDate As System.Web.UI.WebControls.Label
    Protected WithEvents divPRCSReceivedDate As System.Web.UI.HtmlControls.HtmlGenericControl
    'Protected WithEvents divPRCSSentDate As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents txtBeneficiaryDetails As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblBeneficiaryDetailsMan As System.Web.UI.WebControls.Label
    Protected WithEvents lblCreditTerm As System.Web.UI.WebControls.Label
    Protected WithEvents hidVendorIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hid6 As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidVendorIndexOld As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents dtgSubDoc As System.Web.UI.WebControls.DataGrid
    Protected WithEvents divSubDoc As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents hidResidenceType As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidVendorId As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidPaymentType As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidGSTCode As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidIsGST As System.Web.UI.WebControls.HiddenField
    Protected WithEvents hidexceedCutOffDt As System.Web.UI.WebControls.HiddenField
    'Zulham 17042015 IPP GST Stage 2B
    Protected WithEvents hidCountry As System.Web.UI.HtmlControls.HtmlInputHidden
    'Zulham 21/12/2015 (Stage 4 Phase 2)
    Protected WithEvents hidEmpId As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents txtTotalAmountNoGST As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtGSTAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents hidPaymentAmount As System.Web.UI.HtmlControls.HtmlInputHidden
    'Zulham 10/04/2018 - PAMB
    Protected WithEvents cmdUpload As System.Web.UI.WebControls.Button
    Protected WithEvents File1Int As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents pnlAttach As System.Web.UI.WebControls.Panel
    Protected WithEvents hidFileIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    'Zulham 21112018
    Protected WithEvents lblPaymentMethodFull As System.Web.UI.WebControls.Label
    'Zulham 27032019
    Protected WithEvents cmdMultiInvoice As System.Web.UI.WebControls.Button

    Public Enum EnumDocDetail
        icAction = 0
        icSNO = 1
        ichidSNO = 2
        icPayFor = 3
        icReimbursement = 4
        icRefNo = 5 '4
        icDescription = 6 '5
        icUOM = 7 '6
        icQTY = 8 '7
        icUnitPrice = 9 '8
        icAmt = 10 '9
        icGSTAmount = 11 '7 '6
        icInputTax = 12 '8 '7
        icOutputTax = 13 '9 '8
        icGLCode = 14 '11 '10
        icAssetGrp = 15 '12 '11
        icAssetSubGrp = 16 '13 '12

        'Jules 2018.08.09
        'icHOBR = 18 '15 '14
        icGift = 18
        'End modification.

        'Zulham 24102018
        'icCostAlloc = 17 '14 '13
        icCostCenter = 17

    End Enum


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_AbortTransaction(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.AbortTransaction

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region


    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        strNewCSS = "true"
        ViewState("itemTotal") = 0
        ViewState("itemGSTTotal") = 0
        ViewState("GSTTotalInput") = 0.0
        ViewState("GSTTotalOutput") = 0.0
        'Zulham 11/02/2015 Case 8317
        ViewState("GSTTotalIM1IM3") = 0.0

        SetGridProperty(dtgInvDetail, "N")
        SetGridProperty(Me.dtgSubDoc, "Y")
        MyBase.Page_Load(sender, e)
        Dim LateSubmit As String
        Dim objDoc As New IPPMain
        Dim strMsgVoid As String = "Document No. for the Vendor cannot be reused again. Are you sure than you want to Void?"
        Dim strLimit, strIsResident As String
        Dim strVenPayMethod As String

        ViewState("lineno") = 0
        Session("mode") = Request.QueryString("mode")

        Session("DocNo") = Request.QueryString("index")
        Session("Status") = Request.QueryString("status")
        Session("urlrefereripp") = Request.QueryString("urlrefereripp")

        'Zulham 05042019 - REQ018
        If Not Session("MultiInvoices") Is Nothing Then
            Session("MultiInvoices") = Nothing
            Response.Redirect(dDispatcher.direct("IPP", "IPPList.aspx"))
        End If

        If Session("Status") <> "" Then
            Image.ImageUrl = dDispatcher.direct("Plugins/images", "collapse_up.gif")
            renderIPPApprFlow()

        Else
            Me.hidApprflow.Style("display") = "none"
            dtgApprvFlow.Enabled = False

        End If

        If Session("Action") = "" Then
            Session("Action") = "New"
        End If

        Dim blnIPPOfficer As Boolean
        Dim blnIPPAO As Boolean
        Dim blnIPPOfficerS As Boolean
        Dim objUsers As New Users

        blnIPPOfficer = objUsers.checkUserFixedRole("'IPP Officer(F)'")
        blnIPPAO = objUsers.checkUserFixedRole("'IPP Approving Officer'")
        blnIPPOfficerS = objUsers.checkUserFixedRole("'IPP Officer'")
        ViewState("IPPOfficer") = blnIPPOfficer

        If blnIPPOfficerS = True Or blnIPPAO = True Then
            divPRCSReceivedDate.Visible = False
        Else
            divPRCSReceivedDate.Visible = True
            txtPRCSSentDate.Value = DateTime.Today
            txtPRCSReceivedDate.Value = DateTime.Today
        End If

        If Not Page.IsPostBack Then
            Session("SubDocument") = Nothing
            cmdAddLine.Style("display") = "none"
            cmdSave.Style("display") = "none"
            cmdMultiGL.Style("display") = "none"
            cmdMultiGL.Visible = False
            trLateReason.Style.Item("display") = "none"

            'Zulham 18/02/2016 - IPP Stage 4 Phase 2
            'Autocalculate payment_amount
            Me.txtTotalAmountNoGST.Attributes("onblur") = "javascript: var box1 = document.getElementById(""txtTotalAmountNoGST"");" &
            "var box2 = document.getElementById(""txtGSTAmount"");" &
            "if (box1.value == ''){box1.value=0};" &
            "if (box2.value == ''){box2.value=0};" &
            "var box3 = parseFloat(box1.value.replace(',','')) + parseFloat(box2.value.replace(',',''));" &
            "box3 = box3.toFixed(2);" &
            "$(""#txtPaymentAmt"").val(box3)"

            Me.txtGSTAmount.Attributes("onblur") = "javascript: var box1 = document.getElementById(""txtTotalAmountNoGST"");" &
            "var box2 = document.getElementById(""txtGSTAmount"");" &
            "if (box1.value == ''){box1.value=0};" &
            "if (box2.value == ''){box2.value=0};" &
            "var box3 = parseFloat(box1.value.replace(',','')) + parseFloat(box2.value.replace(',',''));" &
            "box3 = box3.toFixed(2);" &
            "$(""#txtPaymentAmt"").val(box3)"

            'Zulham 10/04/2018 - PAMB
            Dim objIPP As New IPP

            If Session("strItem") Is Nothing Then
                'Delete those temp records created in the previous session (incase user exit IE without proper log off)
                objIPP.deleteAttachment(Session.SessionID, "IPP")
            End If
            cmdUpload.Attributes.Add("onclick", "return checkDocFile('doc','" & File1Int.ClientID & "');")
            displayAttachFile()
            'End

            If Session("mode") = "modify" Then

                'Zulham 23042019 - REQ018
                Dim strInvoiceNo = objDB.GetVal("select im_invoice_no from invoice_mstr where im_invoice_index = " & Session("DocNo"))
                Dim venIndex = objDB.GetVal("select im_s_coy_id from invoice_mstr where im_invoice_index = " & Session("DocNo"))
                objIPP.updateInvoiceMstrTotal(strInvoiceNo, venIndex)

                Session("Action") = "Edit"
                GenerateTab()
                PopulateForDDL()
                populateLateReason()
                DisplayIPPDocInfo()
                'strCoyType = objDB.Get1Column("ipp_company", "ic_coy_type", " WHERE ic_index = '" & ViewState("venidx") & "' AND ic_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'")
                strCoyType = objDB.Get1Column("ipp_company", "ic_coy_type", " WHERE ic_index = '" & ViewState("venidx") & "'")

                PopulateVenTypeAhead(strCoyType)
                rbtnCoyType.SelectedValue = strCoyType
                hid4.Value = txtDocNo.Text
                bindgrid()
                btnContinue.Style("display") = "none"
                btnContinueMultiGL.Style("display") = "none"
                btnContinueMultiGL.Visible = False
                'Zulham 27030219
                cmdMultiInvoice.Visible = False
                cmdSave.Style("display") = ""
                cmdMultiGL.Style("display") = "on"
                If Not rbtnCoyType.SelectedValue = "E" Then cmdMultiGL.Visible = True Else cmdMultiGL.Visible = False
                hid5.Value = removeGSTCode(txtVendor.Text)
                If rbtnMasterDoc.SelectedValue.ToString = "1" And Me.dtgInvDetail.Items.Count > 0 Then
                    cmdSubDoc.Visible = True
                    If dtgSubDoc.Items.Count > 0 Then
                        divSubDoc.Visible = True
                    End If
                Else
                    If Not Session("DocNo") Is Nothing Then
                        If Common.parseNull(objDB.GetVal("select if(im_ind1 is null, '', im_ind1) as 'im_ind1' from invoice_mstr where im_invoice_index =" & Session("DocNo"))).ToString.ToUpper = "Y" Then
                            cmdSubDoc.Visible = True
                            If dtgSubDoc.Items.Count > 0 Then
                                divSubDoc.Visible = True
                            End If
                            rbtnMasterDoc.SelectedValue = 1
                        Else
                            cmdSubDoc.Visible = False
                            divSubDoc.Visible = False
                        End If
                    Else
                        cmdSubDoc.Visible = False
                        divSubDoc.Visible = False
                    End If
                End If
            Else
                Session("Action") = "New"
                Session("aryDoc") = Nothing
                strCoyType = rbtnCoyType.SelectedValue
                GenerateTab()
                PopulateForDDL()
                PopulateVenTypeAhead(strCoyType)
                LateSubmit = objDoc.getLateSubmitCheck
                ViewState("LateSubmit") = LateSubmit
                Me.cmdSubmit.Attributes.Add("onClick", "return late_submission(" & LateSubmit & ");")
                Me.hidBtnContinue.Attributes.Add("onClick", "return checkduplication();")
                Me.hidBtnContinueMultiGL.Attributes.Add("onClick", "return checkduplicationMultiGL();")
                cmdSubmit.Style("display") = "none"
                cmdVoid.Style("display") = "none"
                btnContinue.Style("display") = "on"
                btnContinueMultiGL.Style("display") = "on"
                btnContinueMultiGL.Visible = True
                'Zulham 27030219
                cmdMultiInvoice.Visible = True
                hid5.Value = removeGSTCode(txtVendor.Text)
                If rbtnMasterDoc.SelectedValue.ToString = "1" And Me.dtgInvDetail.Items.Count > 0 Then
                    cmdSubDoc.Visible = True
                    If dtgSubDoc.Items.Count > 0 Then
                        divSubDoc.Visible = True
                    End If
                Else
                    If Not Session("DocNo") Is Nothing Then
                        If Common.parseNull(objDB.GetVal("select if(im_ind1 is null, '', im_ind1) as 'im_ind1' from invoice_mstr where im_invoice_index =" & Session("DocNo"))).ToString.ToUpper.ToUpper = "Y" Then
                            cmdSubDoc.Visible = True
                            If dtgSubDoc.Items.Count > 0 Then
                                divSubDoc.Visible = True
                                rbtnMasterDoc.SelectedValue = 1
                            End If
                        Else
                            cmdSubDoc.Visible = False
                            divSubDoc.Visible = False
                        End If
                    Else
                        cmdSubDoc.Visible = False
                        divSubDoc.Visible = False
                    End If
                End If
            End If

        Else

            Session("SelCurrency") = ddlCurrency.SelectedValue
            strCoyType = rbtnCoyType.SelectedValue
            PopulateVenTypeAhead(strCoyType)
            If Not hid6.Value = "" Then
                'repopulate the comp details
                Dim dsCompDetail As New DataSet
                Dim objIPPMain As New IPPMain
                ViewState("oldvenidx") = hidVendorIndex.Value
                ViewState("venidx") = hid6.Value

                If txtVendor.Text <> "" Then
                    dsCompDetail = objIPPMain.GetVendorAddress(Common.Parse(removeGSTCode(txtVendor.Text)), ViewState("IPPOfficer"), ViewState("venidx"))

                    'Zulham 27112018
                    'If rbtnCoyType.SelectedValue = "V" Then
                    If txtDocDate.Value <> "" Then
                            lblCreditTerm.Text = dsCompDetail.Tables(0).Rows(0)("IC_CREDIT_TERMS") & " days"
                            'Zulham 03112018
                            If Not Common.parseNull(dsCompDetail.Tables(0).Rows(0)("IC_CREDIT_TERMS"), 0) = 0 Then
                                lblDocDueDate.Text = DateTime.Parse(txtDocDate.Value).AddDays(dsCompDetail.Tables(0).Rows(0)("IC_CREDIT_TERMS"))
                            End If
                        End If
                    'Else

                    'End If

                    txtVenAddL1.Text = dsCompDetail.Tables(0).Rows(0)("IC_ADDR_LINE1")
                    txtVenAddL2.Text = dsCompDetail.Tables(0).Rows(0)("IC_ADDR_LINE2")
                    txtVenAddL3.Text = dsCompDetail.Tables(0).Rows(0)("IC_ADDR_LINE3")
                    txtVenAddPostcode.Text = dsCompDetail.Tables(0).Rows(0)("IC_POSTCODE")
                    txtVenAddCity.Text = dsCompDetail.Tables(0).Rows(0)("IC_CITY")
                    txtVenAddState.Text = dsCompDetail.Tables(0).Rows(0)("IC_STATE2")
                    txtVenAddCountry.Text = dsCompDetail.Tables(0).Rows(0)("IC_COUNTRY2")
                    'Zulham 03112018
                    lblPaymentMethod.Text = Common.parseNull(dsCompDetail.Tables(0).Rows(0)("IC_PAYMENT_METHOD"))
                    'Zulham 02/03/2015 IPP GST Stage 2B
                    If lblPaymentMethod.Text.Trim.ToLower = "nostro" Then
                        lblPaymentMethod.Text = "Nostro Expenses"
                    End If

                    'Zulham 27112018
                    Select Case lblPaymentMethod.Text
                        Case "IBG"
                            lblPaymentMethodFull.Text = "LOCAL BANK TRANSFER-(RM)"
                        Case "TT"
                            lblPaymentMethodFull.Text = "TELEGRAPHIC TRANSFER-(FOREIGN CURRENCY)"
                        Case "BC"
                            lblPaymentMethodFull.Text = "CHEQUE-(RM)"
                        Case "BD"
                            lblPaymentMethodFull.Text = "BANK DRAFT-(FOREIGN CURRENCY)"
                        Case "CO"
                            lblPaymentMethodFull.Text = "CASHIER'S ORDER-(RM)"
                        Case ""
                            lblPaymentMethodFull.Text = ""
                    End Select

                    'Zulham 03112018
                    If Common.parseNull(dsCompDetail.Tables(0).Rows(0)("IC_PAYMENT_METHOD")) = "BC" Then
                        label8.Text = "Bank Code :"
                        lblBankNameAccountNo.Text = dsCompDetail.Tables(0).Rows(0)("IC_bank_code")
                    Else
                        label8.Text = "Bank Code[Bank A/C No.] :"
                        'Zulham 17042015 IPP GST Stage 2B
                        If Not dsCompDetail.Tables(0).Rows(0)("IC_bank_acct").ToString.Length = 0 Then
                            lblBankNameAccountNo.Text = dsCompDetail.Tables(0).Rows(0)("IC_bank_code") & "[" & dsCompDetail.Tables(0).Rows(0)("IC_bank_acct") & "]"
                        Else
                            'Zulham 12/10/2015 - added conditions
                            'lblBankNameAccountNo.Text = ""
                            If Not dsCompDetail.Tables(0).Rows(0)("IC_bank_code") Is Nothing Then
                                If Not dsCompDetail.Tables(0).Rows(0)("IC_bank_code").ToString.Trim.Length = 0 Then
                                    lblBankNameAccountNo.Text = dsCompDetail.Tables(0).Rows(0)("IC_bank_code") & "[]"
                                End If
                            End If
                        End If
                    End If

                    'Residents or Non
                    Dim IsResident = ""
                    IsResident = objDB.GetVal("SELECT ic_resident_type FROM IPP_company WHERE ic_index =  '" & Common.Parse(ViewState("venidx")) & "'")
                    If IsResident.ToString.Trim = "Y" Then
                        rdbWHTComp.Enabled = False
                        rdbWHTVendor.Enabled = False
                        rdbNoWHT.Enabled = False
                        txtWHT.Enabled = False
                        txtNoWHTReason.Enabled = False
                        Me.ddlCurrency.Enabled = False
                        rdbWHTComp.Checked = False
                        rdbWHTVendor.Checked = False
                        rdbNoWHT.Checked = False
                        txtWHT.Text = ""
                    Else
                        rdbWHTComp.Enabled = True
                        rdbWHTVendor.Enabled = True
                        rdbNoWHT.Enabled = True
                        txtWHT.Enabled = True
                        txtNoWHTReason.Enabled = True
                        Me.ddlCurrency.Enabled = True
                    End If
                    'End

                    'IPP Stage 2A - CH 5 Mar 2015
                    'Zulham 25032015 Case 8603
                    'Zulham 14112018
                    If Common.parseNull(dsCompDetail.Tables(0).Rows(0)("IC_PAYMENT_METHOD")) = "RENTAS" Or Common.parseNull(dsCompDetail.Tables(0).Rows(0)("IC_PAYMENT_METHOD")) = "NOSTRO" _
                        Or Common.parseNull(dsCompDetail.Tables(0).Rows(0)("IC_PAYMENT_METHOD")) = "TT" Or Common.parseNull(dsCompDetail.Tables(0).Rows(0)("IC_PAYMENT_METHOD")) = "BD" Then
                        'lblBeneficiaryDetailsMan.Visible = True
                        lblBeneficiaryDetailsMan.Style("display") = ""
                        txtBeneficiaryDetails.Enabled = True
                        Me.ddlCurrency.Enabled = True
                    Else
                        'lblBeneficiaryDetailsMan.Visible = False
                        lblBeneficiaryDetailsMan.Style("display") = "none"
                        txtBeneficiaryDetails.Enabled = True
                        Me.ddlCurrency.Enabled = False
                    End If
                End If

                Session("dsCompDetail") = dsCompDetail
                If Session("Action") = "Edit" Then
                    bindgrid()
                End If

                ''Zulham 10/04/2018 - PAMB
                displayAttachFile()
                ''End

            End If
            If Session("Action") = "Edit" Then
                btnContinue.Style("display") = "none"
                btnContinueMultiGL.Style("display") = "none"
                btnContinueMultiGL.Visible = False
                'Zulham 27030219
                cmdMultiInvoice.Visible = False
            End If
            Me.hidBtnContinue2.Attributes.Add("onClick", "return checkduplication2();")
            Me.hidBtnContinueSubmit.Attributes.Add("onClick", "return checkduplication3();")
            If rbtnMasterDoc.SelectedValue.ToString = "1" And Me.dtgInvDetail.Items.Count > 0 Then
                cmdSubDoc.Visible = True
                If dtgSubDoc.Items.Count > 0 Then
                    divSubDoc.Visible = True
                End If
            Else
                cmdSubDoc.Visible = False
                If dtgSubDoc.Items.Count > 0 Then
                    divSubDoc.Visible = True
                End If
            End If
        End If

        LateSubmit = objDoc.getLateSubmitCheck
        ViewState("LateSubmit") = LateSubmit
        If hidLateSubmit.Value = "" Then
            Me.cmdSubmit.Attributes.Add("onClick", "return late_submission(" & LateSubmit & ");")
        Else
            Me.cmdSubmit.Attributes.Remove("onClick")
        End If

        Me.txtWHT.Attributes.Add("onkeypress", "return isNumberKey(event);")
        Me.txtDocNo.Attributes.Add("onkeypress", "return isNumberCharKey(event);")
        Me.txtPaymentAmt.Attributes.Add("onkeypress", "return isDecimalKey(event);")
        Me.txtPaymentAmt.Attributes.Add("onblur", "return isValidDecimalCheck();")
        Me.cmdVoid.Attributes.Add("onclick", "return confirm('" & strMsgVoid & "');")

        ''Zulham 18/02/2015 - IPP STAGE 4 PHASE 2
        Me.txtTotalAmountNoGST.Attributes.Add("onkeypress", "return isDecimalKey(event);")
        'Me.txtTotalAmountNoGST.Attributes.Add("onblur", "return isValidDecimalCheck();")
        Me.txtGSTAmount.Attributes.Add("onkeypress", "return isDecimalKey(event);")
        'Me.txtGSTAmount.Attributes.Add("onblur", "return isValidDecimalCheck();")

        'Zulham 17042015 IPP GST Stage 2B
        'Zulham 07112018
        If lblPaymentMethod.Text = "TT" Or (lblPaymentMethod.Text.ToUpper.Contains("NOSTRO") And Me.ddlCurrency.SelectedValue.Trim <> "MYR") _
            Or lblPaymentMethod.Text = "BD" Then
            Me.ddlCurrency.Enabled = True
        Else
            Me.ddlCurrency.SelectedValue = "MYR"
            Me.ddlCurrency.Enabled = False
        End If

        'Residents or Non
        If Not hidVendorId.Value Is Nothing Then
            If ViewState("venidx") Is Nothing Then hidVendorId.Value = "" Else hidVendorId.Value = ViewState("venidx")
        Else
            ViewState("venidx") = 0
        End If
        strIsResident = objDB.GetVal("SELECT ic_resident_type FROM IPP_company WHERE ic_index =  '" & Common.Parse(ViewState("venidx")) & "'")
        If strIsResident.ToString.Trim = "Y" Then
            rdbWHTComp.Enabled = False
            rdbWHTVendor.Enabled = False
            rdbNoWHT.Enabled = False
            txtWHT.Enabled = False
            txtNoWHTReason.Enabled = False
            isResident = True
            rdbWHTComp.Checked = False
            rdbWHTVendor.Checked = False
            rdbNoWHT.Checked = False
            txtWHT.Text = ""
        Else
            isResident = False
        End If
        'End

        If lblPaymentMethod.Text = "IBG" Then
            strLimit = objDB.GetVal("SELECT IF(IFNULL(ip_param_value,'')='',0,ip_param_value) AS ip_param_value FROM IPP_parameter WHERE  ip_param = 'IBG_LIMIT' and ip_coy_id =  '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'")
            If txtPaymentAmt.Text <> "" Then
                If CDbl(txtPaymentAmt.Text) > CDbl(strLimit) Then
                    lblPaymentMethod.Text = "RENTAS"
                End If
            End If
        End If

        'IPP Stage 2A - CH 5 Mar 2015
        'Zulham 25032015 Case 8603
        'Zulham 07112018
        If lblPaymentMethod.Text = "TT" Or lblPaymentMethod.Text = "RENTAS" Or lblPaymentMethod.Text.ToUpper.Contains("NOSTRO") _
            Or lblPaymentMethod.Text = "BD" Then
            'lblBeneficiaryDetailsMan.Visible = True
            lblBeneficiaryDetailsMan.Style("display") = ""
            txtBeneficiaryDetails.Enabled = True
        Else
            'lblBeneficiaryDetailsMan.Visible = False
            lblBeneficiaryDetailsMan.Style("display") = "none"
            txtBeneficiaryDetails.Enabled = True
        End If

        If Session("mode") = "new" And Session("Action") = "New" Then
            cmdVoid.Style("display") = "none"
        Else
            cmdVoid.Style("display") = "on"
        End If

        If Request.QueryString("frm") = "dashboard" Then
            lnkBack.NavigateUrl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx")

        ElseIf Request.QueryString("urlrefereripp") = "IPPList" Then
            lnkBack.NavigateUrl = dDispatcher.direct("IPP", "IPPList.aspx")

        ElseIf Request.QueryString("urlrefereripp") = "IPPEnq" Then
            lnkBack.NavigateUrl = dDispatcher.direct("IPP", "IPPEnq.aspx")

        Else
            lnkBack.Visible = False
        End If

        If vldsum.InnerHtml <> "" Then
            vldsum.InnerHtml = ""
        End If

        'Check for GST
        Dim docDate = txtDocDate.Value
        Dim createdDate = Common.FormatWheelDate(WheelDateFormat.LongDate, Date.Today)
        Dim _cutoffDate = System.Configuration.ConfigurationManager.AppSettings.Get("GstCutOffDate")
        'Zulham 13082015
        'Additional condition for company's effective date
        Dim getVendor = ""
        getVendor = removeGSTCode(txtVendor.Text)
        'Dim effectiveDate = objDB.GetVal("SELECT IFNULL(ic_gst_eff_date, '') FROM IPP_COMPANY WHERE ic_coy_name = '" & Common.Parse(getVendor) & "'")
        Dim _exceedCutOffDt As String = ""
        Dim strIsGst As String
        'If Trim(effectiveDate) <> "" Then
        If CDate(createdDate) >= CDate(_cutoffDate) Then ' And CDate(createdDate) >= CDate(effectiveDate) Then
            _exceedCutOffDt = "Yes"
            ViewState("_exceedCutOffDt") = "Yes"
            If txtVendor.Text <> "" And docDate <> "" Then
                'Dim getVendor = ""
                'getVendor = removeGSTCode(txtVendor.Text)
                Dim GSTRegNo = objDB.GetVal("SELECT IFNULL(IC_TAX_REG_NO, '') FROM IPP_COMPANY WHERE ic_coy_name = '" & Common.Parse(getVendor) & "'")
                If GSTRegNo <> "" And docDate = "" Then
                    strIsGst = "Yes"
                ElseIf GSTRegNo <> "" And CDate(docDate) < CDate(_cutoffDate) Then 'And CDate(docDate) < CDate(effectiveDate) Then
                    strIsGst = "No"
                ElseIf GSTRegNo <> "" And CDate(docDate) >= CDate(_cutoffDate) Then 'And CDate(docDate) >= CDate(effectiveDate) Then
                    strIsGst = "Yes"
                Else
                    strIsGst = "No"
                End If
            Else
                strIsGst = "Yes"
            End If
        Else
            strIsGst = "No"
        End If
        'Else
        'strIsGst = "No"
        'End If

        'Zulham 19082015
        If CDate(createdDate) >= CDate(_cutoffDate) Then
            _exceedCutOffDt = "Yes"
        End If

        ViewState("_exceedCutOffDt") = _exceedCutOffDt
        ViewState("strIsGst") = strIsGst
        hidIsGST.Value = strIsGst
        hidexceedCutOffDt.Value = _exceedCutOffDt
        Session("NewPoInfo") = Nothing
        Session("ItemDeleted") = Nothing
        If strIsGst = "Yes" Then
            dtgInvDetail.Columns(4).Visible = True
            dtgInvDetail.Columns(11).Visible = True
            dtgInvDetail.Columns(12).Visible = True
            dtgInvDetail.Columns(13).Visible = True
            dtgSubDoc.Columns(4).Visible = True
        ElseIf _exceedCutOffDt = "Yes" Then
            dtgInvDetail.Columns(4).Visible = True
            dtgInvDetail.Columns(11).Visible = True
            dtgInvDetail.Columns(12).Visible = True
            dtgInvDetail.Columns(13).Visible = True
            dtgSubDoc.Columns(4).Visible = True
        Else
            dtgInvDetail.Columns(4).Visible = False
            dtgInvDetail.Columns(11).Visible = False
            dtgInvDetail.Columns(12).Visible = False
            dtgInvDetail.Columns(13).Visible = False
            dtgSubDoc.Columns(4).Visible = False
        End If
        'End

        'Zulham 18/02/2016 - IPP Stage 4 Phase 2
        'Re-set pament amount value
        If Me.txtTotalAmountNoGST.Text.Trim = "" Then
            txtTotalAmountNoGST.Text = "0.00"
        End If
        If Me.txtGSTAmount.Text.Trim = "" Then
            txtGSTAmount.Text = "0.00"
        End If
        If txtPaymentAmt.Text.Trim = "" Or txtPaymentAmt.Text.Trim = "0.00" Then
            txtPaymentAmt.Text = CDec(txtTotalAmountNoGST.Text) + CDec(txtGSTAmount.Text)
            txtPaymentAmt.Text = FormatNumber(txtPaymentAmt.Text, 2)
        End If

        'Zulham 03/03/2016 - IPP Stage 4 Phase 2
        'Re-set payment amount value
        If Page.IsPostBack Then
            If Me.txtTotalAmountNoGST.Text.Trim = "" Then
                txtTotalAmountNoGST.Text = "0.00"
            End If
            If Me.txtGSTAmount.Text.Trim = "" Then
                txtGSTAmount.Text = "0.00"
            End If

            txtPaymentAmt.Text = CDec(txtTotalAmountNoGST.Text) + CDec(txtGSTAmount.Text)
            txtPaymentAmt.Text = FormatNumber(txtPaymentAmt.Text, 2)
        End If

        'Zulham PAMB - 11/04/2018
        If Not hidFileIndex.Value = "" Then
            removeAttachment()
        End If

    End Sub
    Sub populateLateReason()
        Dim objdb As New EAD.DBCom
        Dim latereason As String

        If Session("DocNo") Is Nothing Or Session("DocNo") = 0 Then
            'Jules 2018.07.09 - Allow "\" and "#" in Document No.
            If ViewState("oldvenidx") <> ViewState("venidx") And ViewState("oldvenidx") <> "" Then
                Session("DocNo") = objdb.GetVal("SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & Replace(Replace(hid4.Value, "\", "\\"), "#", "\#") & "' and im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' and im_s_coy_id = '" & ViewState("oldvenidx") & "'")
            Else
                Session("DocNo") = objdb.GetVal("SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & Replace(Replace(hid4.Value, "\", "\\"), "#", "\#") & "' and im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' and im_s_coy_id = '" & ViewState("venidx") & "'")
            End If
        End If
        latereason = objdb.Get1ColumnCheckNull("invoice_mstr", "im_late_reason", " WHERE im_invoice_index='" & Session("DocNo") & "'")

        If latereason = "" Then
            trLateReason.Style.Item("display") = "none"
        Else
            trLateReason.Style.Item("display") = ""
            txtLateReason.Text = latereason
        End If
    End Sub
    Private Sub PopulateForDDL()

        objGlobal.FillCodeTable(ddlCurrency, CodeTable.Currency)


        Dim DocTypeArray(4) As String
        Dim strDocType As String
        DocTypeArray(0) = "Invoice"
        DocTypeArray(1) = "Bill"
        DocTypeArray(2) = "Credit Note"
        DocTypeArray(3) = "Debit Note"
        DocTypeArray(4) = "Letter"

        For Each strDocType In DocTypeArray
            ddlDocType.Items.Add(strDocType)
        Next

        If ddlDocType.SelectedIndex < 0 Then
            ddlDocType.SelectedValue = DocTypeArray(0)
        End If

    End Sub
    Sub PopulateVenTypeAhead(Optional ByVal strCoyType As String = "")

        Dim ventypeahead As String
        Dim i, count As Integer
        Dim vencontent, content2 As String
        Dim strCompID As String
        Dim vtypeahead As String

        If strCoyType = "V" Then
            vtypeahead = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=IPPCOY&coytype=V")
        ElseIf strCoyType = "E" Then
            vtypeahead = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=IPPCOY&coytype=E")
        Else
            vtypeahead = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=IPPCOY&coytype=")
        End If

        'Zulham 21/12/2015 (Stage 4 Phase 2)
        'added field hidEmpId 
        'if(data[11]==""E""){vendorName + "" ("" + empId + "")"";$(""#txtVendor"").val(vendorName)
        '        "$(""#hidEmpId"").val(data[14]);" & vbCrLf & _

        'Zulham 18032019
        vencontent &= "$(""#txtVendor"").autocomplete(""" & vtypeahead & "&compid=" & strCompID & """, {" & vbCrLf &
        "width: 200," & vbCrLf &
        "scroll: true," & vbCrLf &
        "selectFirst: false" & vbCrLf &
        "});" & vbCrLf &
        "var vendIdx = document.getElementById(""hidVendorId"").value;" & vbCrLf &
        "var vendIdx2 = document.getElementById(""hid6"").value;" & vbCrLf &
        "if(vendIdx == """")" & vbCrLf &
        "{" & vbCrLf &
        "};" & vbCrLf &
        "$(""#txtVendor"").result(function(event,data,item) {" & vbCrLf &
        "if (data)" & vbCrLf &
        "$(""#hid6"").val(data[1]);" & vbCrLf &
        "$(""#hidResidenceType"").val(data[11]);" & vbCrLf &
        "$(""#hidPaymentType"").val(data[9]);" & vbCrLf &
        "$(""#txtVendor"").val(data[0]);" & vbCrLf &
        "$(""#txtVenAddL1"").val(data[2]);" & vbCrLf &
        "$(""#txtVenAddL2"").val(data[3]);" & vbCrLf &
        "$(""#txtVenAddL3"").val(data[4]);" & vbCrLf &
        "$(""#txtVenAddPostcode"").val(data[5]);" & vbCrLf &
        "$(""#txtVenAddCity"").val(data[6]);" & vbCrLf &
        "$(""#txtVenAddState"").val(data[7]);" & vbCrLf &
        "$(""#txtVenAddCountry"").val(data[8]);" & vbCrLf &
        "//$(""#ddlCurrency"").attr('selectedIndex', 13);" & vbCrLf &
        "document.getElementById(""ddlCurrency"").disabled=true;" & vbCrLf &
        "if(data[9] !== 'NOSTRO'){$(""#lblPaymentMethod"").text(data[9])}else{$(""#lblPaymentMethod"").text('Nostro Expenses')};" & vbCrLf &
        "if(data[10] !== ""[]""){$(""#lblBankNameAccountNo"").text(data[10]);}else{$(""#lblBankNameAccountNo"").text("""");}" & vbCrLf &
        "switch(data[9]){" & vbCrLf &
        "case 'TT':$(""#lblPaymentMethodFull"").text('TELEGRAPHIC TRANSFER-(FOREIGN CURRENCY)');break;" & vbCrLf &
        "case 'IBG':$(""#lblPaymentMethodFull"").text('LOCAL BANK TRANSFER-(RM)');break;" & vbCrLf &
        "case 'BC':$(""#lblPaymentMethodFull"").text('CHEQUE-(RM)');break;" & vbCrLf &
        "case 'BD':$(""#lblPaymentMethodFull"").text('BANK DRAFT-(FOREIGN CURRENCY)');break;" & vbCrLf &
        "case 'CO':$(""#lblPaymentMethodFull"").text(""CASHIER'S ORDER-(RM)"");break;" & vbCrLf &
        "default:$(""#lblPaymentMethodFull"").text("""");break;" & vbCrLf &
        "}" & vbCrLf &
        "$(""#hidGSTCode"").val(data[12]);" & vbCrLf &
        "$(""#hidCountry"").val(data[8]);" & vbCrLf &
        "$(""#hidEmpId"").val(data[14]);" & vbCrLf &
        "var gstCode = document.getElementById(""hidGSTCode"").value;" & vbCrLf &
        "var vendorName = document.getElementById(""txtVendor"").value;" & vbCrLf &
        "var empId = document.getElementById(""hidEmpId"").value;" & vbCrLf &
        "if(gstCode != """" ){ vendorName = vendorName + "" ("" + gstCode + "")"";$(""#txtVendor"").val(vendorName);}else{if(data[15]==""E""){vendorName = vendorName + "" ("" + empId + "")"";$(""#txtVendor"").val(vendorName)}};" & vbCrLf &
        "});" & vbCrLf &
        "$(""#txtVendor"").blur(function() {" & vbCrLf &
        "var hidven = document.getElementById(""hid6"").value;" & vbCrLf &
        "if(hidven == """")" & vbCrLf &
        "{" & vbCrLf &
        "$(""#txtVendor"").val("""");" & vbCrLf &
        "$(""#txtVenAddL1"").val("""");" & vbCrLf &
        "$(""#txtVenAddL2"").val("""");" & vbCrLf &
        "$(""#txtVenAddL3"").val("""");" & vbCrLf &
        "$(""#txtVenAddPostcode"").val("""");" & vbCrLf &
        "$(""#txtVenAddCity"").val("""");" & vbCrLf &
        "$(""#txtVenAddState"").val("""");" & vbCrLf &
        "$(""#txtVenAddCountry"").val("""");" & vbCrLf &
        "$//(""#ddlCurrency"").attr('selectedIndex', 13);" & vbCrLf &
        "$(""#ddlCurrency"").attr('disabled', 'disabled');" & vbCrLf &
        "$(""#lblPaymentMethod"").text("""");" & vbCrLf &
        "$(""#lblBankNameAccountNo"").text("""");" & vbCrLf &
        "$(""#txtPaymentAmt"").val("""");" & vbCrLf &
        "$(""#hidGSTCode"").val("""");" & vbCrLf &
        "$(""#hidEmpId"").val("""");" & vbCrLf &
        "$(""#lblPaymentMethodFull"").val("""");" & vbCrLf &
        "}" & vbCrLf &
        "var hidRes = document.getElementById(""hidResidenceType"").value;" & vbCrLf &
        "if(hidRes == ""N""){" & vbCrLf &
        "}else{" & vbCrLf &
        "};" & vbCrLf &
        "var payType = document.getElementById(""hidPaymentType"").value;" & vbCrLf &
        "var hidCountry = document.getElementById(""hidCountry"").value;" & vbCrLf &
        "if(payType == ""RENTAS"" || payType == ""TT"" || payType == ""NOSTRO""){" & vbCrLf &
        "document.getElementById(""lblBeneficiaryDetailsMan"").style.display = """";" & vbCrLf &
        "}else{" & vbCrLf &
        "document.getElementById(""lblBeneficiaryDetailsMan"").style.display = ""none"";" & vbCrLf &
        "};" & vbCrLf &
        "if(payType == ""TT"" || (payType == ""NOSTRO"" && hidCountry !== ""MY"") || (payType == ""BD"" && hidCountry !== ""MY"")){" & vbCrLf &
        "document.getElementById(""ddlCurrency"").disabled=false;" & vbCrLf &
        "}else{document.getElementById(""ddlCurrency"").disabled=true;//$(""#ddlCurrency"").attr('selectedIndex', 13);" & vbCrLf &
        "};" & vbCrLf &
        "});" & vbCrLf

        ' for edit purpose

        If Session("Action") = "Edit" Then
            ventypeahead = "<script language=""javascript"">" & vbCrLf &
                      "<!--" & vbCrLf &
                        "$(document).ready(function(){" & vbCrLf &
                        vencontent & vbCrLf &
                        "});" & vbCrLf &
                        "-->" & vbCrLf &
                        "</script>"
        Else
            ventypeahead = "<script language=""javascript"">" & vbCrLf &
          "<!--" & vbCrLf &
            "$(document).ready(function(){" & vbCrLf &
            vencontent & vbCrLf &
            "});" & vbCrLf &
            "-->" & vbCrLf &
            "</script>"
        End If

        Session("ventypeahead") = ventypeahead
        If Not hid6.Value = "" Then ViewState("venidx") = hid6.Value

    End Sub
    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        Session("w_AddPO_tabs") = "<div class=""t_entity""><ul>" &
        "<li><div class=""space""></div></li>" &
                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("IPP", "IPPEntry.aspx", "mode=new&pageid=" & strPageId) & """><span>E2P Document</span></a></li>" &
                    "<li><div class=""space""></div></li>" &
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPP", "IPPList.aspx", "type=Listing&mode=new&pageid=" & strPageId) & """><span>E2P Document Listing</span></a></li>" &
                    "<li><div class=""space""></div></li>" &
                    "</ul><div></div></div>"
    End Sub


    Private Sub rdbNoWHT_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbNoWHT.CheckedChanged
        rdbWHTComp.Checked = False
        rdbWHTVendor.Checked = False
        txtWHT.Enabled = False
        txtNoWHTReason.Enabled = True
        txtWHT.Text = ""
        If Session("mode") = "modify" Then
            bindgrid()
        End If
    End Sub

    Private Sub rdbWHTComp_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbWHTComp.CheckedChanged
        rdbNoWHT.Checked = False
        rdbWHTVendor.Checked = False
        txtNoWHTReason.Enabled = False
        txtWHT.Enabled = True
        txtNoWHTReason.Text = ""
        If Session("mode") = "modify" Then
            bindgrid()
        End If
    End Sub

    Private Sub rdbWHTVendor_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbWHTVendor.CheckedChanged
        rdbNoWHT.Checked = False
        rdbWHTComp.Checked = False
        txtNoWHTReason.Enabled = False
        txtWHT.Enabled = True
        txtNoWHTReason.Text = ""
        If Session("mode") = "modify" Then
            bindgrid()
        End If
    End Sub

    Private Sub btnGetAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetAdd.Click

        Dim dsCompDetail As New DataSet
        Dim objIPPMain As New IPPMain
        ViewState("oldvenidx") = hidVendorIndex.Value
        ViewState("venidx") = hid6.Value

        If txtVendor.Text <> "" Then
            dsCompDetail = objIPPMain.GetVendorAddress(Common.Parse(removeGSTCode(txtVendor.Text)), ViewState("IPPOfficer"), ViewState("venidx"))

            'Zulham 27112018
            'If rbtnCoyType.SelectedValue = "V" Then
            If txtDocDate.Value <> "" Then
                lblCreditTerm.Text = dsCompDetail.Tables(0).Rows(0)("IC_CREDIT_TERMS") & " days"
                lblDocDueDate.Text = DateTime.Parse(txtDocDate.Value).AddDays(dsCompDetail.Tables(0).Rows(0)("IC_CREDIT_TERMS"))
            Else
                vldsum.InnerHtml = "<li>Document Date " & objGlobal.GetErrorMessage("00001") & "</li>"
                txtVendor.Text = ""
                Exit Sub
            End If
            'Else

            'End If

            txtVenAddL1.Text = dsCompDetail.Tables(0).Rows(0)("IC_ADDR_LINE1")
            txtVenAddL2.Text = dsCompDetail.Tables(0).Rows(0)("IC_ADDR_LINE2")
            txtVenAddL3.Text = dsCompDetail.Tables(0).Rows(0)("IC_ADDR_LINE3")
            txtVenAddPostcode.Text = dsCompDetail.Tables(0).Rows(0)("IC_POSTCODE")
            txtVenAddCity.Text = dsCompDetail.Tables(0).Rows(0)("IC_CITY")
            txtVenAddState.Text = dsCompDetail.Tables(0).Rows(0)("IC_STATE2")
            txtVenAddCountry.Text = dsCompDetail.Tables(0).Rows(0)("IC_COUNTRY2")
            'Zulham 05112018
            lblPaymentMethod.Text = Common.parseNull(dsCompDetail.Tables(0).Rows(0)("IC_PAYMENT_METHOD"))

            'Zulham 21112018
            Select Case lblPaymentMethod.Text
                Case "IBG"
                    lblPaymentMethodFull.Text = "LOCAL BANK TRANSFER-(RM)"
                Case "TT"
                    lblPaymentMethodFull.Text = "TELEGRAPHIC TRANSFER-(FOREIGN CURRENCY)"
                Case "BC"
                    lblPaymentMethodFull.Text = "CHEQUE-(RM)"
                Case "BD"
                    lblPaymentMethodFull.Text = "BANK DRAFT-(FOREIGN CURRENCY)"
                Case "CO"
                    lblPaymentMethodFull.Text = "CASHIER'S ORDER-(RM)"
                Case ""
                    lblPaymentMethodFull.Text = ""
            End Select


            'Zulham 05112018
            If Common.parseNull(dsCompDetail.Tables(0).Rows(0)("IC_PAYMENT_METHOD")) = "BC" Then
                label8.Text = "Bank Code :"
                lblBankNameAccountNo.Text = dsCompDetail.Tables(0).Rows(0)("IC_bank_code")
            Else
                label8.Text = "Bank Code[Bank A/C No.] :"
                lblBankNameAccountNo.Text = dsCompDetail.Tables(0).Rows(0)("IC_bank_code") & "[" & dsCompDetail.Tables(0).Rows(0)("IC_bank_acct") & "]"

            End If

            'Residents or Non
            Dim strIsResident = ""
            strIsResident = objDB.GetVal("SELECT ic_resident_type FROM IPP_company WHERE ic_index =  '" & Common.Parse(ViewState("venidx")) & "'")
            If strIsResident.ToString.Trim = "Y" Then
                'rdbWHTComp.Enabled = False
                'rdbWHTVendor.Enabled = False
                'rdbNoWHT.Enabled = False
                txtWHT.Enabled = False
                txtNoWHTReason.Enabled = False
                Me.ddlCurrency.Enabled = False
            Else
                rdbWHTComp.Enabled = True
                rdbWHTVendor.Enabled = True
                rdbNoWHT.Enabled = True
                txtWHT.Enabled = True
                txtNoWHTReason.Enabled = True
                Me.ddlCurrency.Enabled = True
            End If
            'End

            'IPP Stage 2A - CH - 5 Mar 2015
            'Zulham 25032015 Case 8603
            'Zulham 07112018
            If Common.parseNull(dsCompDetail.Tables(0).Rows(0)("IC_PAYMENT_METHOD")) = "RENTAS" Or Common.parseNull(dsCompDetail.Tables(0).Rows(0)("IC_PAYMENT_METHOD")) = "NOSTRO" _
                Or Common.parseNull(dsCompDetail.Tables(0).Rows(0)("IC_PAYMENT_METHOD")) = "TT" Or Common.parseNull(dsCompDetail.Tables(0).Rows(0)("IC_PAYMENT_METHOD")) = "BD" Then
                'lblBeneficiaryDetailsMan.Visible = True
                lblBeneficiaryDetailsMan.Style("display") = ""
                txtBeneficiaryDetails.Enabled = True
                Me.ddlCurrency.Enabled = True
            Else
                'lblBeneficiaryDetailsMan.Visible = False
                lblBeneficiaryDetailsMan.Style("display") = "none"
                txtBeneficiaryDetails.Enabled = True
                Me.ddlCurrency.Enabled = False
            End If
        End If

        Session("dsCompDetail") = dsCompDetail
        If Session("Action") = "Edit" Then
            bindgrid()
        End If

    End Sub
    Private Sub btnhidden3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhidden3.Click
        bindgrid()
        btnContinue.Style("display") = "none"
        btnContinueMultiGL.Style("display") = "none"
        btnContinueMultiGL.Visible = False
    End Sub
    Private Sub SaveIPPDoc(ByVal strFrm As String, Optional ByVal trigger As String = "", Optional ByVal mode As String = "")
        Dim dtDoc, dtSubDoc As New DataTable
        Dim dsDoc As New DataSet
        Dim objDoc As New IPPMain
        Dim CompIdx, OldCompIdx As Integer
        Dim dtDocDetail As New DataTable
        Dim dsDocDetail As New DataSet
        Dim btnAction As String
        Dim strLimit As String
        Dim dsCompDetail As New DataSet
        Dim strVenPayMethod As String
        'zulham 18042018 PAMB
        Dim objIPP As New IPP

        'Zulham 03/03/2016 - IPP Stage 4 Phase 2
        'Re-set payment amount value
        If Me.txtTotalAmountNoGST.Text.Trim = "" Then
            txtTotalAmountNoGST.Text = "0.00"
        End If
        If Me.txtGSTAmount.Text.Trim = "" Then
            txtGSTAmount.Text = "0.00"
        End If

        txtPaymentAmt.Text = CDec(txtTotalAmountNoGST.Text) + CDec(txtGSTAmount.Text)
        txtPaymentAmt.Text = FormatNumber(txtPaymentAmt.Text, 2)


        If vldsum.InnerHtml <> "" Then
            vldsum.InnerHtml = ""
        End If

        If hidVendorIndex.Value = "" Then
            If ViewState("oldvenidx") <> ViewState("venidx") And ViewState("oldvenidx") <> "" Then
                hidVendorIndex.Value = ViewState("oldvenidx")
            Else
                hidVendorIndex.Value = ViewState("venidx")
            End If
        End If

        'Zulham 17042018 - PAMB
        If ViewState("oldvenidx") = "" And ViewState("DocNo") IsNot Nothing Then
            If ViewState("DocNo").ToString.Length > 0 Then
                ViewState("oldvenidx") = objDB.GetVal("SELECT im_s_coy_id FROM invoice_mstr WHERE im_invoice_index = " & ViewState("DocNo"))
            End If
        ElseIf ViewState("oldvenidx") <> "" And ViewState("DocNo") IsNot Nothing Then 'if, by any chances, the user just typed-in the vend's name
            If ViewState("DocNo").ToString.Length > 0 Then
                ViewState("oldvenidx") = objDB.GetVal("SELECT im_s_coy_id FROM invoice_mstr WHERE im_invoice_index = " & ViewState("DocNo"))
            End If
        End If
        'End

        'Check the selected company
        Dim compName = objDB.GetVal("select count(*) from ipp_company where ic_coy_name = '" & Common.Parse(removeGSTCode(Me.txtVendor.Text.Trim)) & "' and ic_index =" & hidVendorIndex.Value)
        If compName = 0 Then
            hidVendorIndex.Value = objDB.GetVal("select ic_index from ipp_company where ic_coy_name = '" & Common.Parse(removeGSTCode(Me.txtVendor.Text.Trim)) & "'")
            ViewState("venidx") = hidVendorIndex.Value
        End If
        'End

        dsCompDetail = objDoc.GetVendorAddress(Common.Parse(removeGSTCode(txtVendor.Text)), ViewState("IPPOfficer"), hidVendorIndex.Value)

        If validateField(strFrm) Then
            If validateInput() Then
                dtDoc.Columns.Add("DocType", Type.GetType("System.String"))
                dtDoc.Columns.Add("DocNo", Type.GetType("System.String"))
                dtDoc.Columns.Add("DocDate", Type.GetType("System.DateTime"))
                dtDoc.Columns.Add("LateSubmit", Type.GetType("System.String"))
                dtDoc.Columns.Add("ManualPONo", Type.GetType("System.String"))
                dtDoc.Columns.Add("VendorName", Type.GetType("System.String"))
                dtDoc.Columns.Add("VenCompIDX", Type.GetType("System.String"))
                dtDoc.Columns.Add("VenAddrLine1", Type.GetType("System.String"))
                dtDoc.Columns.Add("VenAddrLine2", Type.GetType("System.String"))
                dtDoc.Columns.Add("VenAddrLine3", Type.GetType("System.String"))
                dtDoc.Columns.Add("VenAddrPostCode", Type.GetType("System.String"))
                dtDoc.Columns.Add("VenAddrState", Type.GetType("System.String"))
                dtDoc.Columns.Add("VenAddrCity", Type.GetType("System.String"))
                dtDoc.Columns.Add("VenAddrCountry", Type.GetType("System.String"))
                dtDoc.Columns.Add("CurrencyCode", Type.GetType("System.String"))
                dtDoc.Columns.Add("PaymentAmt", Type.GetType("System.Decimal"))
                dtDoc.Columns.Add("PaymentMethod", Type.GetType("System.String"))
                dtDoc.Columns.Add("WHT", Type.GetType("System.String"))
                dtDoc.Columns.Add("WHTOpt", Type.GetType("System.String"))
                dtDoc.Columns.Add("NoWHTReason", Type.GetType("System.String"))
                dtDoc.Columns.Add("InternalRemark", Type.GetType("System.String"))
                dtDoc.Columns.Add("ExchangeRate", Type.GetType("System.String"))
                dtDoc.Columns.Add("LateReason", Type.GetType("System.String"))
                dtDoc.Columns.Add("OldVenCompIDX", Type.GetType("System.String"))
                dtDoc.Columns.Add("BankCode", Type.GetType("System.String"))
                dtDoc.Columns.Add("BankAccount", Type.GetType("System.String"))
                dtDoc.Columns.Add("DocDueDate", Type.GetType("System.String"))
                dtDoc.Columns.Add("PRCSSentDate", Type.GetType("System.String"))
                dtDoc.Columns.Add("PRCSReceivedDate", Type.GetType("System.String"))
                dtDoc.Columns.Add("BeneficiaryDetails", Type.GetType("System.String"))
                dtDoc.Columns.Add("MasterDocument", Type.GetType("System.String"))
                dtDoc.Columns.Add("CompanyCategory", Type.GetType("System.String"))
                dtDoc.Columns.Add("ResidentType", Type.GetType("System.String"))
                dtDoc.Columns.Add("EmpId", Type.GetType("System.String"))
                'Zulham 26/01/2016 - IPP GST Stage 4 Phase 2
                dtDoc.Columns.Add("TotalAmtNoGST", Type.GetType("System.Decimal"))
                dtDoc.Columns.Add("GSTAmt", Type.GetType("System.Decimal"))

                Dim dtr As DataRow
                dtr = dtDoc.NewRow()

                Select Case (ddlDocType.SelectedItem.Value)
                    Case "Invoice"
                        dtr("DocType") = "INV"
                    Case "Bill"
                        dtr("DocType") = "BILL"
                    Case "Credit Note"
                        dtr("DocType") = "CN"
                    Case "Debit Note"
                        dtr("DocType") = "DN"
                    Case "Letter"
                        dtr("DocType") = "LETTER"
                End Select

                dtr("DocNo") = txtDocNo.Text
                dtr("DocDate") = txtDocDate.Value

                If DateDiff("d", txtDocDate.Value, DateTime.Now) > ViewState("LateSubmit") And hidLateSubmit.Value <> "" Then
                    dtr("LateSubmit") = hidLateSubmit.Value
                Else
                    dtr("LateSubmit") = ""
                End If

                dtr("ManualPONo") = txtPONo.Text
                dtr("VendorName") = Common.Parse(removeGSTCode(txtVendor.Text))

                'Get empNo
                If rbtnCoyType.SelectedValue = "E" Then
                    Dim empNo = objDB.GetVal("SELECT IFNULL(ic_additional_1,'') 'ic_additional_1' FROM ipp_company WHERE ic_coy_name = '" & Common.Parse(removeGSTCode(Me.txtVendor.Text.Trim)) & "'")
                    dtr("EmpId") = empNo
                Else
                    dtr("EmpId") = ""
                End If
                'End

                CompIdx = ViewState("venidx")
                If ViewState("oldvenidx") <> ViewState("venidx") And ViewState("oldvenidx") <> "" Then
                    OldCompIdx = ViewState("oldvenidx")
                Else
                    OldCompIdx = CompIdx
                End If
                dtr("VenCompIDX") = CompIdx
                Session("VenCompIDX") = CompIdx
                dtr("VenAddrLine1") = dsCompDetail.Tables(0).Rows(0)("ic_addr_line1") 'txtVenAddL1.Text
                dtr("VenAddrLine2") = dsCompDetail.Tables(0).Rows(0)("ic_addr_line2") 'txtVenAddL2.Text
                dtr("VenAddrLine3") = dsCompDetail.Tables(0).Rows(0)("ic_addr_line3") 'txtVenAddL3.Text
                dtr("VenAddrPostCode") = dsCompDetail.Tables(0).Rows(0)("ic_postcode") 'txtVenAddPostcode.Text
                dtr("VenAddrState") = dsCompDetail.Tables(0).Rows(0)("ic_state") 'txtVenAddState.Text
                dtr("VenAddrCity") = dsCompDetail.Tables(0).Rows(0)("ic_city") 'txtVenAddCity.Text
                dtr("VenAddrCountry") = dsCompDetail.Tables(0).Rows(0)("ic_country") 'txtVenAddCountry.Text

                dtr("CurrencyCode") = ddlCurrency.SelectedValue
                dtr("PaymentAmt") = txtPaymentAmt.Text
                dtr("OldVenCompIDX") = Common.parseNull(OldCompIdx)
                dtr("BankCode") = dsCompDetail.Tables(0).Rows(0)("IC_bank_code")
                dtr("BankAccount") = dsCompDetail.Tables(0).Rows(0)("IC_bank_acct")
                'Zulham 05112018
                lblPaymentMethod.Text = Common.parseNull(dsCompDetail.Tables(0).Rows(0)("ic_payment_method"))

                If lblPaymentMethod.Text = "CASA" Then
                    strLimit = objDB.GetVal("SELECT IF(IFNULL(ip_param_value,'')='',0,ip_param_value) AS ip_param_value FROM IPP_parameter WHERE ip_param = 'CASA_LIMIT' and ip_coy_id =  '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'")
                ElseIf lblPaymentMethod.Text = "IBG" Then
                    strLimit = objDB.GetVal("SELECT IF(IFNULL(ip_param_value,'')='',0,ip_param_value) AS ip_param_value FROM IPP_parameter WHERE  ip_param = 'IBG_LIMIT' and ip_coy_id =  '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'")
                End If
                strVenPayMethod = objDB.GetVal("SELECT ic_payment_method FROM ipp_company WHERE ic_index = '" & CompIdx & "'")

                If lblPaymentMethod.Text = "CASA" Or lblPaymentMethod.Text = "IBG" Then
                    If CDbl(txtPaymentAmt.Text) > CDbl(strLimit) Then
                        dtr("PaymentMethod") = "RENTAS"
                        lblPaymentMethod.Text = "RENTAS"
                    Else
                        dtr("PaymentMethod") = lblPaymentMethod.Text
                    End If
                ElseIf lblPaymentMethod.Text = "RENTAS" And strVenPayMethod = "IBG" Then

                    strLimit = objDB.GetVal("SELECT IF(IFNULL(ip_param_value,'')='',0,ip_param_value) AS ip_param_value FROM IPP_parameter WHERE ip_param = CONCAT('" & strVenPayMethod & "' , '_LIMIT') and ip_coy_id =  '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'")

                    If CDbl(txtPaymentAmt.Text) < CDbl(strLimit) Then
                        dtr("PaymentMethod") = strVenPayMethod
                        lblPaymentMethod.Text = strVenPayMethod
                    Else
                        dtr("PaymentMethod") = lblPaymentMethod.Text
                    End If

                Else
                    dtr("PaymentMethod") = lblPaymentMethod.Text
                End If

                'Jules 2018.07.06 - Comment out withholding tax.
                'Zulham 15/05/2017
                'always set the withholding value to null for local company
                'If txtWHT.Text = "" Or hidResidenceType.Value = "Y" Then
                'dtr("WHT") = "NULL"
                'Else
                '    dtr("WHT") = txtWHT.Text
                'End If

                'If rdbWHTComp.Checked Then
                '    dtr("WHTOpt") = "1"
                '    dtr("NoWHTReason") = ""
                'ElseIf rdbWHTVendor.Checked Then
                '    dtr("WHTOpt") = "2"
                '    dtr("NoWHTReason") = ""
                'ElseIf rdbNoWHT.Checked Then
                '    dtr("WHTOpt") = "3"
                '    dtr("NoWHTReason") = txtNoWHTReason.Text
                'Else
                '    dtr("WHTOpt") = ""
                '    dtr("NoWHTReason") = ""
                'End If
                dtr("WHT") = "NULL"
                dtr("WHTOpt") = ""
                dtr("NoWHTReason") = ""
                'End modification.

                'Zulham 15/05/2017
                'always set the withholding value to null for local company
                If hidResidenceType.Value = "Y" Or isResident = True Then
                    dtr("WHTOpt") = ""
                    dtr("NoWHTReason") = ""
                End If

                dtr("InternalRemark") = txtIntRemark.Text
                dtr("ExchangeRate") = "NULL"

                If lblDocDueDate.Text = "" Then
                    dtr("DocDueDate") = "NULL"
                Else
                    If dsCompDetail.Tables(0).Rows(0)("IC_CREDIT_TERMS") Is DBNull.Value Then
                        lblDocDueDate.Text = txtDocDate.Value
                    Else
                        lblDocDueDate.Text = DateTime.Parse(txtDocDate.Value).AddDays(dsCompDetail.Tables(0).Rows(0)("IC_CREDIT_TERMS"))
                    End If
                    dtr("DocDueDate") = lblDocDueDate.Text
                End If

                If txtPRCSSentDate.Value = "" Then
                    dtr("PRCSSentDate") = "NULL"
                Else
                    dtr("PRCSSentDate") = txtPRCSSentDate.Value
                End If

                If txtPRCSReceivedDate.Value = "" Then
                    dtr("PRCSReceivedDate") = "NULL"
                Else
                    dtr("PRCSReceivedDate") = txtPRCSReceivedDate.Value
                End If

                dtr("BeneficiaryDetails") = txtBeneficiaryDetails.Text

                dtr("MasterDocument") = Me.rbtnMasterDoc.SelectedItem.Text.Substring(0, 1).ToUpper
                Dim CompanyCategory = objDB.GetVal("SELECT IF(ic_company_category IS NULL,'',ic_company_category) AS 'ic_company_category' FROM ipp_company WHERE ic_index = '" & CompIdx & "'")
                dtr("CompanyCategory") = Common.parseNull(CompanyCategory)
                Dim ResidentType = objDB.GetVal("SELECT ic_resident_type FROM ipp_company WHERE ic_index = '" & CompIdx & "'")
                dtr("ResidentType") = Common.parseNull(ResidentType)

                'Zulham 26/01/2016 - IPP Stage 4 Phase 2
                If Me.txtTotalAmountNoGST.Text.Trim = "" Then Me.txtTotalAmountNoGST.Text = 0.0
                If Me.txtGSTAmount.Text.Trim = "" Then Me.txtGSTAmount.Text = 0.0
                dtr("TotalAmtNoGST") = Me.txtTotalAmountNoGST.Text
                dtr("GSTAmt") = Me.txtGSTAmount.Text

                dtDoc.Rows.Add(dtr)
                dsDoc.Tables.Add(dtDoc)

                'Zulham 26032015 Case 8603
                If lblPaymentMethod.Text.ToUpper.Contains("NOSTRO") Then lblPaymentMethod.Text = "Nostro Expenses"

                If validateField(strFrm) And validateInput() Then

                    If strFrm = "Save" Then
                        objDoc.SaveIPPDocHeader(dsDoc, True, hid4.Value)
                        'Zulham 07/03/2016 - IPP Stage 4 Phase 2
                        'Skip message box for edit
                        If Not mode = "edit" Then
                            strMsg = objGlobal.GetErrorMessage("00003")
                            If Not trigger = "cmdAddLine" Then Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                        End If

                    ElseIf strFrm = "Update" Then
                        objDoc.SaveIPPDocHeader(dsDoc, True, hid4.Value, True)
                    Else
                        If strFrm <> "Submit" Then
                            objDoc.SaveIPPDocHeader(dsDoc)
                        End If
                    End If

                    'Zulham PAMB - 11/04/2018
                    If objIPP.getIPPTempAttach(Session.SessionID, "I").Tables(0).Rows.Count > 0 Then
                        'get the invoice index

                        'Jules 2018.07.09 - Allow "\" and "#" in Document No.
                        'Dim invoiceIndex = objDB.GetVal("select im_invoice_index from invoice_mstr where im_s_coy_id = '" & ViewState("venidx") & "' and im_invoice_no ='" & Me.txtDocNo.Text & "'")
                        Dim invoiceIndex = objDB.GetVal("select im_invoice_index from invoice_mstr where im_s_coy_id = '" & ViewState("venidx") & "' and im_invoice_no ='" & Replace(Replace(txtDocNo.Text, "\", "\\"), "#", "\#") & "'") 'End modification.
                        objIPP.updateAttachment(invoiceIndex, Session.SessionID)
                    End If
                    'End

                    If strFrm = "Submit" Then

                        'Zulham 30012019
                        Dim strCoyCode = ""
                        strCoyCode = objDB.GetVal("SELECT ic_other_b_coy_code FROM IPP_company WHERE ic_index =  '" & Common.Parse(ViewState("venidx")) & "'")
                        If strCoyCode.ToString.Trim = "" Then
                            vldsum.InnerHtml = "<li> " & "Vendor Code doesn't exist for this company. Please contact System Administrator." & "</li>"
                            Exit Sub
                        End If

                        'Zulham 04/03/2016 - IPP Stage 4 Phase 2
                        'Check for amount discrepancies between item details amount and header amount
                        Dim dt As New DataTable
                        'Zulham 31102018
                        dt = objDoc.getItemDetailsAmtandGST(ViewState("docno"), ViewState("venidx"))
                        If Not dt Is Nothing Then
                            If Not dt.Rows.Count = 0 Then
                                'Zulham 09102018 - PAMB SST
                                If FormatNumber(txtTotalAmountNoGST.Text.Trim, 2) <> dt.Rows(0).Item(1) Then
                                    vldsum.InnerHtml = "<li>" & "Invalid Total Amount Excluding SST." & "</li>"
                                    Exit Sub
                                End If
                                If FormatNumber(Me.txtGSTAmount.Text.Trim, 2) <> dt.Rows(0).Item(0) Then
                                    vldsum.InnerHtml = "<li>" & "Invalid SST Amount." & "</li>"
                                    Exit Sub
                                End If
                            End If
                        End If

                        'Jules 2018.07.09 - Allow "\" and "#" in Document No.
                        'Zulham 19/02/2016 - IPP Stage 4 Phase 2
                        'CN/DN can't precede the invoice it's tied to
                        'Zulham 05/01/2016 - IPP STAGE 4 Phase 2
                        'check for invoices status that are tied to CN/DN
                        'If objDoc.checkStatus(Me.txtDocNo.Text, ViewState("venidx"), ViewState("status")) = False Then
                        If objDoc.checkStatus(Replace(Replace(txtDocNo.Text, "\", "\\"), "#", "\#"), ViewState("venidx"), ViewState("status")) = False Then 'End modification.
                            Common.NetMsgbox(Me, "Credit Note/Debit Note cannot precede the invoices it's tied to.", MsgBoxStyle.Exclamation)
                            Exit Sub
                        End If

                        Dim amt As String
                        objDoc.SaveIPPDocHeader(dsDoc, True, hid4.Value)
                        hid4.Value = txtDocNo.Text

                        'Zulham 11/02/2015 Case 8317
                        'recalculate everything
                        If ViewState("GSTTotalIM1IM3") Is Nothing Then
                            For Each dgItemFF As DataGridItem In Me.dtgInvDetail.Items
                                If Not dgItemFF.Cells(EnumDocDetail.icInputTax) Is Nothing Then
                                    Dim inputTax = dgItemFF.Cells(EnumDocDetail.icInputTax).Text
                                    If inputTax.ToString.Trim.Contains("IM1") Or inputTax.ToString.Trim.Contains("IM3") Then
                                        If Not dgItemFF.Cells(EnumDocDetail.icGSTAmount) Is Nothing Then
                                            ViewState("GSTTotalIM1IM3") += dgItemFF.Cells(EnumDocDetail.icGSTAmount).Text
                                        End If
                                    End If
                                End If
                            Next
                        ElseIf ViewState("GSTTotalIM1IM3") = 0.0 Then
                            For Each dgItemFF As DataGridItem In Me.dtgInvDetail.Items
                                If Not dgItemFF.Cells(EnumDocDetail.icInputTax) Is Nothing Then
                                    Dim inputTax = dgItemFF.Cells(EnumDocDetail.icInputTax).Text
                                    If inputTax.ToString.Trim.Contains("IM1") Or inputTax.ToString.Trim.Contains("IM3") Then
                                        If Not dgItemFF.Cells(EnumDocDetail.icGSTAmount) Is Nothing Then
                                            ViewState("GSTTotalIM1IM3") += dgItemFF.Cells(EnumDocDetail.icGSTAmount).Text
                                        End If
                                    End If
                                End If
                            Next
                        End If


                        If ViewState("GSTTotalIM1IM3") Is Nothing Then ViewState("GSTTotalIM1IM3") = 0
                        amt = ViewState("itemTotalAmt") - ViewState("GSTTotalIM1IM3")

                        amt = (Format(CDbl(amt), "#.00"))

                        If amt <> (Format(CDbl(txtPaymentAmt.Text), "#.00")) Then
                            vldsum.InnerHtml = "<li>" & objGlobal.GetErrorMessage("00035") & "</li>"
                            Exit Sub
                        End If

                        'Zulham 11/02/2015 Case 8317
                        If Not ViewState("SubDocAmt") Is Nothing Then
                            If Not CDec(ViewState("SubDocAmt")) = CDec(0) Then
                                'Zulham 20052015 IPP GST Stage 1
                                'Set the decimal places to two for both sides
                                'Zulham 21082015 - Minus im1/im3 total
                                If FormatNumber(CDec(ViewState("itemTotalAmt")) - CDec(ViewState("GSTTotalIM1IM3")), 2) <> FormatNumber(CDec(ViewState("SubDocAmt")), 2) Then
                                    vldsum.InnerHtml = "<li>" & "Sub Document Total must tally with items total." & "</li>"
                                    Exit Sub
                                End If
                            End If
                        End If

                        Dim dsapplist As New DataSet

                        'Zulham 26062018 - PAMB
                        Dim strIsResident = ""
                        If isResident Then
                            strIsResident = "Y"
                        Else
                            strIsResident = "N"
                        End If

                        'Zulham 21092018 - PAMB
                        'UAT U00003 
                        Dim objAdmin As New Admin
                        Dim exchangeRate As Decimal = 1
                        Dim dsExchangeRate As New DataSet : dsExchangeRate = objAdmin.getexrate(ddlCurrency.SelectedValue, "")
                        If Not dsExchangeRate.Tables(0).Rows.Count = 0 Then
                            dsapplist = objDoc.getIPPApprovalWorkflowList("E2P", strIsResident, CDec(Me.txtPaymentAmt.Text), CDec(dsExchangeRate.Tables(0).Rows(0).Item(2)))  'Zulham 11072018 - PAMB
                            exchangeRate = CDec(dsExchangeRate.Tables(0).Rows(0).Item(2))
                        Else
                            dsapplist = objDoc.getIPPApprovalWorkflowList("E2P", strIsResident, CDec(Me.txtPaymentAmt.Text))
                            exchangeRate = 1
                        End If
                        'End

                        If dsapplist.Tables(0).Rows.Count = 0 Then

                            strMsg = objGlobal.GetErrorMessage("00034")
                            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)

                        ElseIf dsapplist.Tables(0).Rows.Count = 1 Then

                            'Jules 2018.07.09 - Allow "\" and "#" in Document No.
                            'Zulham 26092018 - PAMB
                            'UAT U00003
                            If objDoc.SaveIPPDoc(dsDoc, strFrm, Session("Action"), Session("DocNo"), Replace(Replace(hid4.Value, "\", "\\"), "#", "\#"), dsapplist.Tables(0).Rows(0).Item("AGB_GRP_INDEX").ToString, "E2P", strIsResident, exchangeRate) Then
                                strMsg = objGlobal.GetErrorMessage("00024")
                                strMsg = "Document " & strMsg
                                If Session("urlrefereripp") Is Nothing Or Session("urlrefereripp") = "" Then
                                    'Common.NetMsgbox(Me, strMsg, dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId), MsgBoxStyle.Information)
                                    'Zulham 23/12/2015 (IPP Phase 2 Stage 4)
                                    'If there's only one approval, stay at the page and show the entered data
                                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                                    'Disable the buttons
                                    Me.cmdSave.Enabled = False
                                    Me.btnContinue.Enabled = False
                                    Me.btnContinueMultiGL.Enabled = False
                                    Me.cmdSubmit.Enabled = False
                                    Me.cmdVoid.Enabled = False
                                    Me.cmdAddLine.Enabled = False
                                    Me.cmdMultiGL.Enabled = False
                                    Me.cmdSubDoc.Enabled = False
                                Else
                                    'Common.NetMsgbox(Me, strMsg, dDispatcher.direct("IPP", "IPPList.aspx", "pageid=" & strPageId), MsgBoxStyle.Information)
                                    'Zulham 16/02/2016 (IPP Phase 2 Stage 4)
                                    'If there's only one approval, stay at the page and show the entered data
                                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                                    'Disable the buttons
                                    Me.cmdSave.Enabled = False
                                    Me.btnContinue.Enabled = False
                                    Me.btnContinueMultiGL.Enabled = False
                                    Me.cmdSubmit.Enabled = False
                                    Me.cmdVoid.Enabled = False
                                    Me.cmdAddLine.Enabled = False
                                    Me.cmdMultiGL.Enabled = False
                                    Me.cmdSubDoc.Enabled = False
                                End If
                                Session("urlrefereripp") = Nothing
                            Else
                                strMsg = objGlobal.GetErrorMessage("00002")
                                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                                Exit Sub
                            End If

                        Else
                            Session("dsDOC") = dsDoc
                            'Zulham 11072018 - PAMB
                            Response.Redirect(dDispatcher.direct("IPP", "IPPApprovalSetup.aspx", "pageid=" & strPageId & "&docno=" & Server.UrlEncode(txtDocNo.Text) & "&frm=" & Server.UrlEncode(strFrm) & "&olddocno=" & Server.UrlEncode(hid4.Value) & "&venIdx=" & ViewState("venidx")))

                            Exit Sub
                        End If

                    End If
                End If
                ' For continue button
                hid5.Value = removeGSTCode(txtVendor.Text)
                ViewState("SaveFailed") = "No"
            End If
        Else
            ViewState("SaveFailed") = "Yes"
        End If
    End Sub
    Private Function validateInput(Optional ByVal a As String = "") As Boolean
        Dim count, i, k As Integer
        Dim ds, dsCAD As New DataSet
        Dim compidx As Integer
        Dim dsIPPDocDetailsCount, j As Integer
        Dim strscript As New System.Text.StringBuilder
        Dim invidx As Integer
        Dim intCostAllocIndex As Integer
        Dim decTtlPct As Decimal
        Dim DocIndex As String

        'Zulham 18/02/2016 - IPP Stage 4 Phase 2
        'Re-set pament amount value
        If Me.txtTotalAmountNoGST.Text.Trim = "" Then
            txtTotalAmountNoGST.Text = 0
        End If
        If Me.txtGSTAmount.Text.Trim = "" Then
            txtGSTAmount.Text = 0
        End If
        If txtPaymentAmt.Text.Trim = "" Or txtPaymentAmt.Text.Trim = "0.00" Then
            txtPaymentAmt.Text = CDec(txtTotalAmountNoGST.Text) + CDec(txtGSTAmount.Text)
            txtPaymentAmt.Text = FormatNumber(txtPaymentAmt.Text, 2)
        End If

        If (Session("DocNo") Is Nothing Or Session("DocNo") = 0) And Session("Action") = "New" Then
            'Jules 2018.07.09 - Allow "\" and "#" in Document No.
            If ViewState("oldvenidx") <> ViewState("venidx") And ViewState("oldvenidx") <> "" Then
                DocIndex = objDB.GetVal("SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & Replace(Replace(hid4.Value, "\", "\\"), "#", "\#") & "' and im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' and im_s_coy_id = '" & ViewState("oldvenidx") & "'")
            Else
                DocIndex = objDB.GetVal("SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & Replace(Replace(hid4.Value, "\", "\\"), "#", "\#") & "' and im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' and im_s_coy_id = '" & ViewState("venidx") & "'")
            End If
        Else
            DocIndex = Session("DocNo")
        End If
        compidx = ViewState("venidx")

        'Zulham PAMB - 11/04/2018
        'If Not objDoc.IsAlphaNum(txtDocNo.Text) Then
        '    If a = "" Then
        '        strMsg = objGlobal.GetErrorMessage("00032")
        '        strMsg = "Document No. " & strMsg
        '        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
        '        Return False
        '    Else
        '        Return False
        '    End If
        'End If
        'End

        'Jules 2018.07.09
        'If objDB.Exist("SELECT '*' FROM invoice_mstr WHERE im_invoice_index <> '" & DocIndex & "' and im_invoice_no = '" & txtDocNo.Text & "' AND im_s_coy_id = '" & compidx & "' AND im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'") > 0 Then
        If objDB.Exist("SELECT '*' FROM invoice_mstr WHERE im_invoice_index <> '" & DocIndex & "' and im_invoice_no = '" & Replace(txtDocNo.Text, "\", "\\") & "' AND im_s_coy_id = '" & compidx & "' AND im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'") > 0 Then 'End modification.
            If a = "" Then
                strMsg = objGlobal.GetErrorMessage("00002")
                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                Return False
            Else
                Return False
            End If
        End If

        Return True

    End Function
    Private Function validateField(Optional ByVal strform As String = "") As Boolean
        Dim count, i As Integer
        Dim ds As New DataSet
        Dim strSql = ""

        'Zulham 18/02/2016 - IPP Stage 4 Phase 2
        'Re-set payment amount value
        If Me.txtTotalAmountNoGST.Text.Trim = "" Then
            txtTotalAmountNoGST.Text = 0
        End If
        If Me.txtGSTAmount.Text.Trim = "" Then
            txtGSTAmount.Text = 0
        End If
        If txtPaymentAmt.Text.Trim = "" Or txtPaymentAmt.Text.Trim = "0.00" Then
            txtPaymentAmt.Text = CDec(txtTotalAmountNoGST.Text) + CDec(txtGSTAmount.Text)
            txtPaymentAmt.Text = FormatNumber(txtPaymentAmt.Text, 2)
        End If

        'Modified for IPP GST Stage 2A - CH - 9 Feb 2015
        Dim strCoyId As String
        If strDefIPPCompID = "" Then
            strCoyId = HttpContext.Current.Session("CompanyID")
        Else
            strCoyId = strDefIPPCompID
        End If

        If txtDocNo.Text = "" Then
            vldsum.InnerHtml = "<li>Document No. " & objGlobal.GetErrorMessage("00001") & "</li>"
            Return False
        ElseIf txtDocDate.Value = "" Then
            vldsum.InnerHtml = "<li>Document Date " & objGlobal.GetErrorMessage("00001") & "</li>"
            Return False
        ElseIf txtDocDate.Value > System.DateTime.Today Then
            vldsum.InnerHtml = "<li>Document Date " & objGlobal.GetErrorMessage("00325") & "</li>"
            Return False
        ElseIf txtVendor.Text = "" Then
            vldsum.InnerHtml = "<li>Vendor " & objGlobal.GetErrorMessage("00001") & "</li>"
            Return False
        ElseIf objDB.Exist("SELECT IC_INDEX FROM IPP_COMPANY WHERE IC_INDEX = '" & ViewState("venidx") & "' AND (IC_COY_TYPE = 'V' OR IC_COY_TYPE = 'E') AND IC_STATUS = 'A' AND IC_COY_ID = '" & Common.Parse(strCoyId) & "'") = 0 Then
            vldsum.InnerHtml = "<li>" & objGlobal.GetErrorMessage("00340") & "</li>"
            Return False
        ElseIf txtPaymentAmt.Text = "" Then
            vldsum.InnerHtml = "<li>Payment Amount " & objGlobal.GetErrorMessage("00001") & "</li>"
            Return False
        End If

        Dim strIsResident = ""
        strIsResident = objDB.GetVal("SELECT ic_resident_type FROM IPP_company WHERE ic_index =  '" & Common.Parse(ViewState("venidx")) & "'")

        'Zulham 05112018
        If lblPaymentMethod.Text.Trim = "" Then
            vldsum.InnerHtml = "<li> " & "Payment Mode doesn't exist for this company. Please contact System Administrator." & "</li>"
            Return False
        End If
        If lblCreditTerm.Text.Trim = "" And txtDocDate.Value.Trim.Length > 0 Then
            vldsum.InnerHtml = "<li> " & "Credit Term doesn't exist for this company. Please contact System Administrator." & "</li>"
            Return False
        End If

        'Zulham 17042015 IPP GST STAGE 2B
        'Zulham 07112018
        If (lblPaymentMethod.Text = "TT" Or lblPaymentMethod.Text = "BD") And ddlCurrency.SelectedValue = "MYR" Then
            vldsum.InnerHtml = "<li> " & objGlobal.GetErrorMessage("00040").ToString.Replace("TT", lblPaymentMethod.Text) & "</li>"
            Return False
        End If

        'Jules 2018.07.06 - Removed Withholding Tax.
        'If lblPaymentMethod.Text = "TT" And ddlCurrency.SelectedValue <> "MYR" And txtWHT.Text = "" Then
        '    If rdbNoWHT.Checked <> True And strIsResident.ToString.ToUpper = "N" Then
        '        vldsum.InnerHtml = "<li>Withholding Tax " & objGlobal.GetErrorMessage("00001") & "</li>"
        '        Return False
        '    End If
        'End If

        'If rdbNoWHT.Checked <> True And strIsResident.ToString.ToUpper = "N" And txtWHT.Text = "" Then
        '    vldsum.InnerHtml = "<li>Withholding Tax " & objGlobal.GetErrorMessage("00001") & "</li>"
        '    Return False
        'End If

        'If strIsResident.ToString.ToUpper = "N" And txtWHT.Text = "" And (rdbWHTComp.Checked = True Or rdbWHTVendor.Checked = True) Then
        '    vldsum.InnerHtml = "<li>Withholding Tax " & objGlobal.GetErrorMessage("00001") & "</li>"
        '    Return False
        'End If

        'If strIsResident.ToString.ToUpper = "N" And txtWHT.Text = "" And rdbNoWHT.Checked = True And txtNoWHTReason.Text = "" Then
        '    vldsum.InnerHtml = "<li> " & objGlobal.GetErrorMessage("00042") & "</li>"
        '    Return False
        'End If

        'If strIsResident.ToString.ToUpper = "N" And txtWHT.Text <> "" And (rdbWHTComp.Checked = False And rdbWHTVendor.Checked = False) Then
        '    vldsum.InnerHtml = "<li>" & objGlobal.GetErrorMessage("00041") & "</li>"
        '    Return False
        'End If
        'End commented block.

        'Zulham 16102018 - PAMB 
        'If (lblPaymentMethod.Text = "TT" Or lblPaymentMethod.Text = "RENTAS" Or lblPaymentMethod.Text.ToUpper.Contains("NOSTRO")) And txtBeneficiaryDetails.Text = "" Then
        '    vldsum.InnerHtml = "<li>Beneficiary Details " & objGlobal.GetErrorMessage("00001") & "</li>"
        '    Return False
        'End If


        If dtgInvDetail.Items.Count < 1 And strform = "Submit" Then
            vldsum.InnerHtml = "<li>" & objGlobal.GetErrorMessage("00131") & "</li>"
            Return False
        End If

        'Zulham 31102018
        'If Not ViewState("strIsGst") Is Nothing Then
        '    If ViewState("strIsGst") = "Yes" Then
        '        For Each item As DataGridItem In dtgInvDetail.Items
        '            If item.Cells(12).Text.Contains("IM2") And isResident = True Then
        '                vldsum.InnerHtml = "<li>Invalid GST Input Tax for Resident Vendor.</li>"
        '                Return False
        '            End If
        '        Next
        '    End If
        'End If

        'Checking for the validity of the input & output tax code
        If ViewState("oldvenidx") <> ViewState("venidx") And ViewState("oldvenidx") <> "" Then
            ds = objDoc.GetIPPDocDetails(ViewState("docno"), ViewState("oldvenidx"), , hid4.Value)
        Else
            ds = objDoc.GetIPPDocDetails(ViewState("docno"), ViewState("venidx"), , hid4.Value)
        End If
        If ds.Tables(0).Rows.Count = 0 Then
            ds = objDoc.GetIPPDocDetails(ViewState("docno"), ViewState("venidx"), , hid4.Value)
        End If
        If Not ds.Tables(0).Rows.Count = 0 Then
            If ViewState("strIsGst") = "Yes" Then
                For rowcount As Integer = 0 To ds.Tables(0).Rows.Count - 1
                    If strIsResident = "Y" Then
                        'Zulham 31102018
                        'If ds.Tables(0).Rows(rowcount).Item("id_gst_input_tax_code").ToString.ToUpper.Contains("IM2") Then
                        '    vldsum.InnerHtml = "<li>Invalid GST Input Tax for Resident Vendor.</li>"
                        '    Return False
                        'End If
                        'If Not ds.Tables(0).Rows(rowcount).Item("id_gst_output_tax_code").ToString.ToUpper.Contains("N/A") And
                        'Not ds.Tables(0).Rows(rowcount).Item("id_gst_output_tax_code").ToString.Trim.Trim = "" Then
                        '    'Zulham 07052015 IPP GST Stage 1
                        '    'TX4 Issue
                        '    'Zulham 05/04/2016 - IM5/IM6 Enhancement
                        '    If Not ds.Tables(0).Rows(rowcount).Item("id_gst_input_tax_code").ToString.ToUpper.Contains("TX4") _
                        '    And Not ds.Tables(0).Rows(rowcount).Item("id_gst_input_tax_code").ToString.ToUpper.Contains("IM5") Then
                        '        vldsum.InnerHtml = "<li>Invalid GST Output Tax for Resident Vendor.</li>"
                        '        Return False
                        '    End If
                        'End If

                        ''Jules 2018.08.08
                        'If Not ds.Tables(0).Rows(rowcount).Item("id_pay_for") Is Nothing AndAlso ds.Tables(0).Rows(rowcount).Item("id_pay_for").ToString <> "" Then
                        '    If Not ds.Tables(0).Rows(rowcount).Item("id_category") Is Nothing AndAlso ds.Tables(0).Rows(rowcount).Item("id_category").ToString.Contains("Life") Then
                        '        Dim strCountry = objDB.GetVal("SELECT ic_country FROM ipp_company WHERE ic_other_b_coy_code = '" & ds.Tables(0).Rows(rowcount).Item("id_pay_for").ToString.Trim & "' and ic_coy_id ='" & strCoyId & "'")
                        '        If strCountry <> "" AndAlso strCountry <> "MY" AndAlso Not ds.Tables(0).Rows(rowcount).Item("id_gst_input_tax_code").ToString.ToUpper.Contains("TXRE") Then
                        '            vldsum.InnerHtml = "<li>Invalid GST Input Tax for Foreign Inter-co.</li>"
                        '            Return False
                        '        End If
                        '    End If
                        'End If

                        If Not ds.Tables(0).Rows(rowcount).Item("id_category") Is Nothing OrElse ds.Tables(0).Rows(rowcount).Item("id_b_gl_code") IsNot Nothing OrElse ds.Tables(0).Rows(rowcount).Item("id_gift") IsNot Nothing Then
                            Dim strGLType = ""
                            strGLType = objDB.GetVal("SELECT IFNULL(cbg_b_gl_type,"""") 'cbg_b_gl_type' FROM company_b_gl_code  WHERE cbg_b_gl_code = '" & ds.Tables(0).Rows(rowcount).Item("id_b_gl_code").ToString.Trim & "'")
                            If strGLType = "CAP" Then
                                'Zulham 31102018
                                'If (ds.Tables(0).Rows(rowcount).Item("id_category").ToString.Trim = "Life" AndAlso Not ds.Tables(0).Rows(rowcount).Item("id_gst_input_tax_code").ToString.ToUpper.Contains("TXC1")) OrElse
                                '    (ds.Tables(0).Rows(rowcount).Item("id_category").ToString.Trim = "Non-Life" AndAlso Not ds.Tables(0).Rows(rowcount).Item("id_gst_input_tax_code").ToString.ToUpper.Contains("TXC2")) OrElse
                                '    (ds.Tables(0).Rows(rowcount).Item("id_category").ToString.Trim = "Mixed" AndAlso Not ds.Tables(0).Rows(rowcount).Item("id_gst_input_tax_code").ToString.ToUpper.Contains("TXCG")) Then
                                '    vldsum.InnerHtml = "<li>Invalid GST Input Tax for Resident Vendor.</li>"
                                '    Return False
                                'End If
                                If ds.Tables(0).Rows(rowcount).Item("id_gift").ToString.Contains("Y") Then
                                    vldsum.InnerHtml = "<li>CAPEX item cannot be Gift.</li>"
                                    Return False
                                End If
                                'Zulham 31102018
                                'ElseIf strGLType = "BLC" Then
                                '    If ds.Tables(0).Rows(rowcount).Item("id_category").ToString.Trim = "Life" AndAlso Not ds.Tables(0).Rows(rowcount).Item("id_gst_input_tax_code").ToString.ToUpper.Contains("BK") Then
                                '        vldsum.InnerHtml = "<li>Invalid GST Input Tax for Resident Vendor.</li>"
                                '        Return False
                                '    End If
                                'Else
                                '    If ds.Tables(0).Rows(rowcount).Item("id_category").ToString.Trim = "Life" AndAlso ds.Tables(0).Rows(rowcount).Item("id_gift").ToString.Contains("Y") AndAlso Not ds.Tables(0).Rows(rowcount).Item("id_gst_input_tax_code").ToString.ToUpper.Contains("FG") Then
                                '        vldsum.InnerHtml = "<li>Invalid GST Input Tax for Resident Vendor.</li>"
                                '        Return False
                                '    End If
                            End If
                        End If
                        'End modification.
                        'Zulham 31102018
                        'Else
                        'If (ds.Tables(0).Rows(rowcount).Item("id_gst_output_tax_code").ToString.ToUpper.Contains("N/A") Or
                        'ds.Tables(0).Rows(rowcount).Item("id_gst_output_tax_code").ToString.Trim = "") And Not _
                        '(ds.Tables(0).Rows(rowcount).Item("id_gst_input_tax_code").ToString.ToUpper.Contains("IM1") Or
                        'ds.Tables(0).Rows(rowcount).Item("id_gst_input_tax_code").ToString.ToUpper.Contains("IM3")) Then
                        '    If ds.Tables(0).Rows(rowcount).Item("id_gst_reimb").ToString.ToUpper.Trim = "R" Then
                        '        vldsum.InnerHtml = "<li>Invalid GST Output Tax for Non Resident Vendor.</li>"
                        '        Return False
                        '    End If
                        'End If
                    End If
                Next
            End If
        End If
        'End

        If strform = "Submit" Then
            If Me.rbtnMasterDoc.SelectedIndex = 0 And Me.dtgSubDoc.Items.Count = 0 Then
                Common.NetMsgbox(Me, "Please add sub-document data.", MsgBoxStyle.Information)
                Return False
            End If
            Dim SubDocTotal As Double = 0.0
            If Not Session("SubDocument") Is Nothing Then
                If Not CType(Session("SubDocument"), ArrayList).Count = 0 Then
                    For rowcount As Integer = 0 To CType(Session("SubDocument"), ArrayList).Count - 1
                        SubDocTotal += CType(Session("SubDocument"), ArrayList)(rowcount)(3)
                        If Not ViewState("_exceedCutOffDt") Is Nothing And Not ViewState("strIsGst") Is Nothing Then
                            If ViewState("_exceedCutOffDt") = "Yes" And ViewState("strIsGst") = "Yes" Then
                                SubDocTotal = SubDocTotal + CType(Session("SubDocument"), ArrayList)(rowcount)(5)
                            End If
                        End If
                    Next
                    ViewState("SubDocAmt") = SubDocTotal
                    'Zulham 12022015 Case 8317
                    'If CDec(Math.Round(SubDocTotal, 2)) <> CDec(Math.Round(CType(Me.txtPaymentAmt.Text, Double), 2)) Then
                    '    Common.NetMsgbox(Me, "Sub Document Total must tally with items total.", MsgBoxStyle.Information)
                    '    Return False
                    'End If
                End If
            End If

            'Zulham 26062018 - PAMB
            'Get group Id
            'Checks for users assigned under the group
            'Checks for users' approval limit
            Dim blnVerifiers = objDoc.checkApprovalLimit(IIf(isResident = True, "Y", "N"), CDec(txtPaymentAmt.Text))
            If Not blnVerifiers Then
                Common.NetMsgbox(Me, "There's no existing workflow. Please add one.", MsgBoxStyle.Information)
                Return False
            End If
            'End
        End If

        'SubDocNo and InvNo validation
        'Dim result = objDB.GetVal("select isd_sub_doc_index from ipp_sub_doc where isd_doc_no = '" & txtDocNo.Text & "' limit 1")
        'If Not result = "" Then
        '    Common.NetMsgbox(Me, "Master Document No. must not exist in Sub Document", MsgBoxStyle.Information)
        '    Return False
        '    Exit Function
        'End If

        'Jules 2018.07.09 - Allow "\" and "#" in Document No.
        'Same vendor - two diff invoices - sub doc no in one of the invoices <> the other invoice no
        strSql = "SELECT im_invoice_index, IM_INVOICE_NO FROM invoice_mstr WHERE im_invoice_index IN (SELECT DISTINCT(isd_mstr_doc_index) FROM ipp_SUB_DOC WHERE " &
         "ISD_DOC_NO LIKE '" & Common.Parse(Replace(Replace(txtDocNo.Text, "\", "\\"), "#", "\#")) & "') AND im_s_coy_id = '" & Common.Parse(Me.hid6.Value) & "' AND im_b_coy_id = '" & Common.Parse(Common.Parse(HttpContext.Current.Session("CompanyID"))) & "' AND IM_PO_INDEX IS NULL"
        ds = objDB.FillDs(strSql)
        If ds.Tables(0).Rows.Count > 0 Then
            Common.NetMsgbox(Me, "Master Document No. must not equal to Sub Description No.")
            Session("SubDocument") = Nothing
            Exit Function
        End If

        'Jules 2018.07.09 - Allow "\" and "#" in Document No.
        'Same vendor - two diff invoices - sub doc no in one of the invoices <> sub doc no in the other invoice no
        strSql = "SELECT IM_INVOICE_NO FROM invoice_mstr WHERE im_invoice_index IN (SELECT DISTINCT(isd_mstr_doc_index) FROM ipp_SUB_DOC WHERE ISD_DOC_NO LIKE '" & Common.parseNull(Replace(txtDocNo.Text, "\", "\\")).ToString & "') AND " &
                 "im_s_coy_id = '" & Common.Parse(Me.hid6.Value) & "' AND im_b_coy_id = '" & Common.Parse(Common.Parse(HttpContext.Current.Session("CompanyID"))) & "' AND IM_PO_INDEX IS NULL"
        ds = objDB.FillDs(strSql)
        If ds.Tables(0).Rows.Count > 0 Then
            Common.NetMsgbox(Me, "Sub Document No must not already exist in the system.")
            Session("SubDocument") = Nothing
            Exit Function
        End If

        'Jules 2018.07.09 - Allow "\" and "#" in Document No.
        'Zulham 19052015 IPP GST Stage 1
        'Additional condition for invoice duplication between two login companies
        Dim loginComp As String
        loginComp = ""
        loginComp = objDB.GetVal("select im_b_coy_id from invoice_mstr where im_s_coy_id = '" & ViewState("venidx") & "' and im_invoice_no = '" & Replace(txtDocNo.Text, "\", "\\") & "'")
        If Not loginComp = "" Then
            If loginComp.ToUpper.Trim <> Common.Parse(Common.Parse(HttpContext.Current.Session("CompanyID"))).Trim.ToUpper Then
                Common.NetMsgbox(Me, "An invoice with similar document no and vendor already exist for " & loginComp.ToUpper & ".")
                Exit Function
            End If
        End If

        Return True
    End Function
    Private Function validateField2() As Boolean
        Dim count, i As Integer
        Dim ds As New DataSet
        Dim strSql = ""

        'Zulham 28/04/2016 - Check for payment type first before proceeding
        Dim strLimit, strVenPayMethod As String
        If lblPaymentMethod.Text = "CASA" Then
            strLimit = objDB.GetVal("SELECT IF(IFNULL(ip_param_value,'')='',0,ip_param_value) AS ip_param_value FROM IPP_parameter WHERE ip_param = 'CASA_LIMIT' and ip_coy_id =  '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'")
        ElseIf lblPaymentMethod.Text = "IBG" Then
            strLimit = objDB.GetVal("SELECT IF(IFNULL(ip_param_value,'')='',0,ip_param_value) AS ip_param_value FROM IPP_parameter WHERE  ip_param = 'IBG_LIMIT' and ip_coy_id =  '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'")
        End If
        strVenPayMethod = objDB.GetVal("SELECT ifnull(ic_payment_method,'') FROM ipp_company WHERE ic_index = '" & ViewState("venidx") & "'")
        If lblPaymentMethod.Text = "CASA" Or lblPaymentMethod.Text = "IBG" Then
            If CDbl(txtPaymentAmt.Text) > CDbl(strLimit) Then
                lblPaymentMethod.Text = "RENTAS"
            End If
        ElseIf lblPaymentMethod.Text = "RENTAS" And strVenPayMethod = "IBG" Then
            strLimit = objDB.GetVal("SELECT IF(IFNULL(ip_param_value,'')='',0,ip_param_value) AS ip_param_value FROM IPP_parameter WHERE ip_param = CONCAT('" & strVenPayMethod & "' , '_LIMIT') and ip_coy_id =  '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'")
            If CDbl(txtPaymentAmt.Text) < CDbl(strLimit) Then
                lblPaymentMethod.Text = strVenPayMethod
            End If
        End If

        'Zulham 05112018
        If lblPaymentMethod.Text.Trim = "" Then
            vldsum.InnerHtml = "<li> " & "Payment Mode doesn't exist for this company. Please contact System Administrator." & "</li>"
            Return False
        End If
        If lblCreditTerm.Text.Trim = "" And txtDocDate.Value.Trim.Length > 0 Then
            vldsum.InnerHtml = "<li> " & "Credit Term doesn't exist for this company. Please contact System Administrator." & "</li>"
            Return False
        End If

        'Zulham 18/02/2016 - IPP Stage 4 Phase 2
        'Re-set pament amount value
        If Me.txtTotalAmountNoGST.Text.Trim = "" Then
            txtTotalAmountNoGST.Text = 0
        End If
        If Me.txtGSTAmount.Text.Trim = "" Then
            txtGSTAmount.Text = 0
        End If
        If txtPaymentAmt.Text.Trim = "" Or txtPaymentAmt.Text.Trim = "0.00" Then
            txtPaymentAmt.Text = CDec(txtTotalAmountNoGST.Text) + CDec(txtGSTAmount.Text)
            txtPaymentAmt.Text = FormatNumber(txtPaymentAmt.Text, 2)
        End If

        'Modified for IPP GST Stage 2A - CH - 5 Feb 2015
        Dim strCoyId As String
        If strDefIPPCompID = "" Then
            strCoyId = HttpContext.Current.Session("CompanyID")
        Else
            strCoyId = strDefIPPCompID
        End If

        If txtDocNo.Text = "" Then
            vldsum.InnerHtml = "<li>Document No. " & objGlobal.GetErrorMessage("00001") & "</li>"
            Return False
        ElseIf txtDocDate.Value = "" Then
            vldsum.InnerHtml = "<li>Document Date " & objGlobal.GetErrorMessage("00001") & "</li>"
            Return False
        ElseIf txtDocDate.Value > System.DateTime.Today Then
            vldsum.InnerHtml = "<li>Document Date " & objGlobal.GetErrorMessage("00325") & "</li>"
            Return False
        ElseIf txtVendor.Text = "" Then
            vldsum.InnerHtml = "<li>Vendor " & objGlobal.GetErrorMessage("00001") & "</li>"
            Return False
            'Modified for IPP GST Stage 2A - CH - 5 Feb 2015
        ElseIf objDB.Exist("SELECT IC_INDEX FROM IPP_COMPANY WHERE IC_INDEX = '" & ViewState("venidx") & "' AND (IC_COY_TYPE = 'V' OR IC_COY_TYPE = 'E') AND IC_STATUS = 'A' AND IC_COY_ID = '" & Common.Parse(strCoyId) & "'") = 0 Then
            vldsum.InnerHtml = "<li>" & objGlobal.GetErrorMessage("00340") & "</li>"
            Return False
        ElseIf txtPaymentAmt.Text = "" Then
            vldsum.InnerHtml = "<li>Payment Amount " & objGlobal.GetErrorMessage("00001") & "</li>"
            Return False
        End If

        Dim strIsResident = ""
        strIsResident = objDB.GetVal("SELECT ic_resident_type FROM IPP_company WHERE ic_index =  '" & Common.Parse(ViewState("venidx")) & "'")

        'Zulham 16102018 - PAMB
        'If (lblPaymentMethod.Text = "TT" Or lblPaymentMethod.Text = "RENTAS" Or lblPaymentMethod.Text.ToUpper.Contains("NOSTRO")) And txtBeneficiaryDetails.Text = "" Then
        '    vldsum.InnerHtml = "<li>Beneficiary Details " & objGlobal.GetErrorMessage("00001") & "</li>"
        '    Return False
        'End If

        'Zulham 17042015 IPP GST STAGE 2B
        'Zulham 07112018
        If (lblPaymentMethod.Text = "TT" Or lblPaymentMethod.Text = "BD") And ddlCurrency.SelectedValue = "MYR" Then
            If lblPaymentMethod.Text.ToUpper.Contains("NOSTRO") Then
                vldsum.InnerHtml = "<li> " & objGlobal.GetErrorMessage("00040").Replace("TT", "NOSTRO") & "</li>"
            ElseIf lblPaymentMethod.Text.ToUpper.Contains("BD") Then
                vldsum.InnerHtml = "<li> " & objGlobal.GetErrorMessage("00040").Replace("TT", "BD") & "</li>"
            Else
                vldsum.InnerHtml = "<li> " & objGlobal.GetErrorMessage("00040") & "</li>"
            End If
            Return False
        End If

        'Jules 2018.07.06 - Removed Withholding Tax.
        'If rdbNoWHT.Checked <> True And strIsResident.ToString.ToUpper = "N" And txtWHT.Text = "" Then
        '    vldsum.InnerHtml = "<li>Withholding Tax " & objGlobal.GetErrorMessage("00001") & "</li>"
        '    Return False
        'End If

        'If strIsResident.ToString.ToUpper = "N" And txtWHT.Text = "" And (rdbWHTComp.Checked = True Or rdbWHTVendor.Checked = True) Then
        '    vldsum.InnerHtml = "<li>Withholding Tax " & objGlobal.GetErrorMessage("00001") & "</li>"
        '    Return False
        'End If

        'If strIsResident.ToString.ToUpper = "N" And txtWHT.Text = "" And rdbNoWHT.Checked = True And txtNoWHTReason.Text = "" Then
        '    vldsum.InnerHtml = "<li> " & objGlobal.GetErrorMessage("00042") & "</li>"
        '    Return False
        'End If

        'If strIsResident.ToString.ToUpper = "N" And txtWHT.Text <> "" And (rdbWHTComp.Checked = False And rdbWHTVendor.Checked = False) Then
        '    vldsum.InnerHtml = "<li>" & objGlobal.GetErrorMessage("00041") & "</li>"
        '    Return False
        'End If


        'If lblPaymentMethod.Text = "TT" And ddlCurrency.SelectedValue <> "MYR" And txtWHT.Text = "" Then
        '    If rdbNoWHT.Checked <> True And strIsResident.ToString.ToUpper = "N" Then
        '        vldsum.InnerHtml = "<li>Withholding Tax " & objGlobal.GetErrorMessage("00001") & "</li>"
        '        Return False
        '    End If
        'End If
        'End commented block.

        'Zulham PAMB - 11/04/2018
        'If Not objDoc.IsAlphaNum(txtDocNo.Text) Then
        '    strMsg = objGlobal.GetErrorMessage("00032")
        '    strMsg = "Document No. " & strMsg
        '    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
        '    Return False
        'End If
        'End

        'SubDocNo and InvNo validation
        'Dim result = objDB.GetVal("select isd_sub_doc_index from ipp_sub_doc where isd_doc_no = '" & txtDocNo.Text & "' limit 1")
        'If Not result = "" Then
        '    Common.NetMsgbox(Me, "Master Document No. must not exist in Sub Document", MsgBoxStyle.Information)
        '    Return False
        '    Exit Function
        'End If

        'Jules 2018.07.09 - Allow "\" and "#" in Document No.
        'Same vendor - two diff invoices - sub doc no in one of the invoices <> the other invoice no
        strSql = "SELECT im_invoice_index, IM_INVOICE_NO FROM invoice_mstr WHERE im_invoice_index IN (SELECT DISTINCT(isd_mstr_doc_index) FROM ipp_SUB_DOC WHERE " &
                 "ISD_DOC_NO LIKE '" & Common.Parse(Replace(Replace(txtDocNo.Text, "\", "\\"), "#", "\#")) & "') AND im_s_coy_id = '" & Common.Parse(Me.hid6.Value) & "' AND im_b_coy_id = '" & Common.Parse(Common.Parse(HttpContext.Current.Session("CompanyID"))) & "' AND IM_PO_INDEX IS NULL"
        ds = objDB.FillDs(strSql)
        If ds.Tables(0).Rows.Count > 0 Then
            Common.NetMsgbox(Me, "Master Document No. must not equal to Sub Document No.")
            Session("SubDocument") = Nothing
            Exit Function
        End If

        'Jules 2018.07.09 - Allow "\" and "#" in Document No.
        'Same vendor - two diff invoices - sub doc no in one of the invoices <> sub doc no in the other invoice no
        strSql = "SELECT IM_INVOICE_NO FROM invoice_mstr WHERE im_invoice_index IN (SELECT DISTINCT(isd_mstr_doc_index) FROM ipp_SUB_DOC WHERE ISD_DOC_NO LIKE '" & Common.parseNull(Replace(txtDocNo.Text, "\", "\\")).ToString & "') AND " &
                 "im_s_coy_id = '" & Common.Parse(Me.hid6.Value) & "' AND im_b_coy_id = '" & Common.Parse(Common.Parse(HttpContext.Current.Session("CompanyID"))) & "' AND IM_PO_INDEX IS NULL"
        ds = objDB.FillDs(strSql)
        If ds.Tables(0).Rows.Count > 0 Then
            Common.NetMsgbox(Me, "Sub Document No must not already exist in the system.")
            Session("SubDocument") = Nothing
            Exit Function
        End If

        'Jules 2018.07.09 - Allow "\" and "#" in Document No.
        'Zulham 07052015 IPP GST Stage 1
        'Additional condition for invoice duplication between two login companies
        Dim loginComp As String
        loginComp = ""
        loginComp = objDB.GetVal("select im_b_coy_id from invoice_mstr where im_s_coy_id = '" & ViewState("venidx") & "' and im_invoice_no = '" & Replace(txtDocNo.Text, "\", "\\") & "'")
        If Not loginComp = "" Then
            If loginComp.ToUpper.Trim <> Common.Parse(Common.Parse(HttpContext.Current.Session("CompanyID"))).Trim.ToUpper Then
                Common.NetMsgbox(Me, "An invoice with similar document no and vendor already exist for " & loginComp.ToUpper & ".")
                Exit Function
            End If
        End If

        Return True
    End Function
    Private Sub cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Dim strscript As New System.Text.StringBuilder
        strscript.Append("<script language=""javascript"">")
        strscript.Append("document.getElementById('btnhidden4').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script7", strscript.ToString())
    End Sub
    Private Sub btnhidden4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhidden4.Click
        SaveIPPDoc("Submit")
        bindgrid()

        ''AddRowTotal()
        ''AddSubDocRowTotal(ViewState("SubDocAmt"))
    End Sub
    Private Sub cmdVoid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdVoid.Click
        Dim strDocNo As String

        strDocNo = txtDocNo.Text

        'Jules 2018.07.09 - Allow "\" and "#" in Document No.
        'Zulham 19/02/2016 - IPP Stage 4 Phase 2
        'Cant void invoice if it's tied to cn/dn
        Dim result As String = ""
        result = objDB.GetVal("SELECT '*' FROM invoice_details, invoicE_mstr WHERE id_ref_no = '" & Replace(txtDocNo.Text, "\", "\\") & "' AND im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " &
        "AND im_invoice_no = id_invoice_no AND im_s_coy_id = id_s_coy_id AND im_invoice_status NOT IN ('15') and im_s_coy_id = '" & ViewState("venidx") & "'")
        If Not result.Trim.Length = 0 Then
            Common.NetMsgbox(Me, "This invoice is tied to a Credit Note/Debit Note.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If


        If objDoc.VoidIPPDoc(Session("DocNo")) Then
            strMsg = objGlobal.GetErrorMessage("00033")
            Common.NetMsgbox(Me, strMsg, dDispatcher.direct("IPP", "IPPList.aspx", "pageid=" & strPageId), MsgBoxStyle.Information)
        End If

    End Sub
    Private Sub DisplayIPPDocInfo()
        Dim dsIPPDoc As DataSet
        Dim DtDiff As Long

        dsIPPDoc = objDoc.GetIPPDoc(Session("DocNo"))
        Session("dsIPPDOC") = dsIPPDoc
        ViewState("docno") = dsIPPDoc.Tables(0).Rows(0)("IM_INVOICE_NO")
        ViewState("venidx") = dsIPPDoc.Tables(0).Rows(0)("IM_S_COY_ID")
        If dsIPPDoc.Tables(0).Rows(0)("IM_INVOICE_TYPE") = "INV" Then
            Me.ddlDocType.SelectedValue = "Invoice"
        ElseIf dsIPPDoc.Tables(0).Rows(0)("IM_INVOICE_TYPE") = "CN" Then
            Me.ddlDocType.SelectedValue = "Credit Note"
        ElseIf dsIPPDoc.Tables(0).Rows(0)("IM_INVOICE_TYPE") = "DN" Then
            Me.ddlDocType.SelectedValue = "Debit Note"
        ElseIf dsIPPDoc.Tables(0).Rows(0)("IM_INVOICE_TYPE") = "BILL" Then
            Me.ddlDocType.SelectedValue = "Bill"
        ElseIf dsIPPDoc.Tables(0).Rows(0)("IM_INVOICE_TYPE") = "LETTER" Then
            Me.ddlDocType.SelectedValue = "Letter"
        End If
        Me.txtDocNo.Text = dsIPPDoc.Tables(0).Rows(0)("IM_INVOICE_NO")
        Me.txtDocDate.Value = Common.FormatWheelDate(WheelDateFormat.ShortDate, dsIPPDoc.Tables(0).Rows(0)("IM_DOC_DATE"))
        Me.ddlCurrency.SelectedValue = dsIPPDoc.Tables(0).Rows(0)("IM_CURRENCY_CODE")
        Me.txtPaymentAmt.Text = Format(dsIPPDoc.Tables(0).Rows(0)("IM_INVOICE_TOTAL"), "#,###,0.00")
        Me.txtPONo.Text = dsIPPDoc.Tables(0).Rows(0)("IM_IPP_PO")

        If dsIPPDoc.Tables(0).Rows(0)("IM_WITHHOLDING_TAX") Is System.DBNull.Value Then
            Me.txtWHT.Text = ""
        Else
            Me.txtWHT.Text = CInt(dsIPPDoc.Tables(0).Rows(0)("IM_WITHHOLDING_TAX"))
        End If

        If dsIPPDoc.Tables(0).Rows(0)("IM_WITHHOLDING_OPT") <> "" Then
            Select Case dsIPPDoc.Tables(0).Rows(0)("IM_WITHHOLDING_OPT")
                Case 1
                    rdbWHTComp.Checked = True
                Case 2
                    rdbWHTVendor.Checked = True
                Case 3
                    rdbNoWHT.Checked = True
            End Select
        End If

        'Zulham 27/01/2016 - IPP Stage 4 Phase 2
        If Not Common.parseNull(dsIPPDoc.Tables(0).Rows(0)("im_invoice_gst"), "").ToString.Trim.Length = 0 Then
            Me.txtGSTAmount.Text = Format(Common.parseNull(dsIPPDoc.Tables(0).Rows(0)("im_invoice_gst"), ""), "#,###,0.00")
        End If
        If Not Common.parseNull(dsIPPDoc.Tables(0).Rows(0)("im_invoice_excl_gst"), "").ToString.Trim.Trim.Length = 0 Then
            Me.txtTotalAmountNoGST.Text = Format(Common.parseNull(dsIPPDoc.Tables(0).Rows(0)("im_invoice_excl_gst"), ""), "#,###,0.00")
        End If


        Me.txtNoWHTReason.Text = dsIPPDoc.Tables(0).Rows(0)("IM_WITHHOLDING_REMARKS")
        If dsIPPDoc.Tables(0).Rows(0)("IM_REMARK") Is System.DBNull.Value Then
            Me.txtIntRemark.Text = ""
        Else
            Me.txtIntRemark.Text = dsIPPDoc.Tables(0).Rows(0)("IM_REMARK")
        End If

        'Zulham 22/02/2016 - IPP Stage 4 phase 2
        ViewState("status") = dsIPPDoc.Tables(0).Rows(0).Item("IM_INVOICE_STATUS")

        Me.lblDocDueDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dsIPPDoc.Tables(0).Rows(0)("IM_DUE_DATE"))
        Me.txtPRCSSentDate.Value = Common.FormatWheelDate(WheelDateFormat.ShortDate, dsIPPDoc.Tables(0).Rows(0)("IM_PRCS_SENT"))
        Me.txtPRCSReceivedDate.Value = Common.FormatWheelDate(WheelDateFormat.ShortDate, dsIPPDoc.Tables(0).Rows(0)("IM_PRCS_RECV"))

        If dsIPPDoc.Tables(0).Rows(0)("IM_INVOICE_STATUS") <> "14" Then '14 = Rejected
            If dsIPPDoc.Tables(0).Rows(0)("IM_PAYMENT_TERM") = "BC" Then
                label8.Text = "Bank Code :"
                Me.lblBankNameAccountNo.Text = dsIPPDoc.Tables(0).Rows(0)("im_bank_code")
            Else
                label8.Text = "Bank Code[Bank A/C No.] :"
                Me.lblBankNameAccountNo.Text = dsIPPDoc.Tables(0).Rows(0)("im_bank_code") & "[" & dsIPPDoc.Tables(0).Rows(0)("im_bank_acct") & "]"

            End If
            Me.lblPaymentMethod.Text = dsIPPDoc.Tables(0).Rows(0)("IM_PAYMENT_TERM")

            'Zulham 21112018
            Select Case lblPaymentMethod.Text
                Case "IBG"
                    lblPaymentMethodFull.Text = "LOCAL BANK TRANSFER-(RM)"
                Case "TT"
                    lblPaymentMethodFull.Text = "TELEGRAPHIC TRANSFER-(FOREIGN CURRENCY)"
                Case "BC"
                    lblPaymentMethodFull.Text = "CHEQUE-(RM)"
                Case "BD"
                    lblPaymentMethodFull.Text = "BANK DRAFT-(FOREIGN CURRENCY)"
                Case "CO"
                    lblPaymentMethodFull.Text = "CASHIER'S ORDER-(RM)"
                Case ""
                    lblPaymentMethodFull.Text = ""
            End Select

            'Zulham 02/03/2015 IPP GST Stage 2B
            If lblPaymentMethod.Text.Trim.ToLower = "nostro" Then
                lblPaymentMethod.Text = "Nostro Expenses"
            End If

            'Get the GST Reg Code if there's any
            Dim strGSTCode = ""
            Dim GST As New GST
            Dim createdDate = Common.FormatWheelDate(WheelDateFormat.LongDate, Date.Today)
            Dim _cutoffDate = System.Configuration.ConfigurationManager.AppSettings.Get("GstCutOffDate")
            If CDate(createdDate) > CDate(_cutoffDate) Then strGSTCode = GST.chkGST_ForIPP(ViewState("venidx"))

            If Not Common.parseNull(strGSTCode) = "" Then
                Me.txtVendor.Text = dsIPPDoc.Tables(0).Rows(0)("IM_S_COY_NAME") & "(" & strGSTCode & ")"
            Else
                'Zulham 23/12/2015 (IPP Stage 4 Phase 2)
                'added empId after the emp's name
                Dim strCoyType = objDB.Get1Column("ipp_company", "ic_coy_type", " WHERE ic_index = '" & ViewState("venidx") & "'")
                If strCoyType = "E" Then
                    Dim empId = objDB.GetVal("select ifnull(ic_business_reg_no,'') from ipp_company where ic_index = '" & ViewState("venidx") & "'")
                    Me.txtVendor.Text = dsIPPDoc.Tables(0).Rows(0)("IM_S_COY_NAME") & "(" & empId & ")"
                Else
                    Me.txtVendor.Text = dsIPPDoc.Tables(0).Rows(0)("IM_S_COY_NAME")
                End If
            End If

            hid6.Value = dsIPPDoc.Tables(0).Rows(0)("IM_S_COY_ID")

            Me.txtVenAddL1.Text = dsIPPDoc.Tables(0).Rows(0)("IM_ADDR_LINE1")
            Me.txtVenAddL2.Text = dsIPPDoc.Tables(0).Rows(0)("IM_ADDR_LINE2")
            Me.txtVenAddL3.Text = dsIPPDoc.Tables(0).Rows(0)("IM_ADDR_LINE3")
            Me.txtVenAddPostcode.Text = dsIPPDoc.Tables(0).Rows(0)("IM_POSTCODE")
            Me.txtVenAddCity.Text = dsIPPDoc.Tables(0).Rows(0)("IM_CITY")
            Me.txtVenAddState.Text = dsIPPDoc.Tables(0).Rows(0)("IM_STATE2")
            Me.txtVenAddCountry.Text = dsIPPDoc.Tables(0).Rows(0)("IM_COUNTRY2")
            If lblDocDueDate.Text <> "" Then
                DtDiff = DateDiff(DateInterval.Day, CDate(txtDocDate.Value), CDate(lblDocDueDate.Text))
                Me.lblCreditTerm.Text = DtDiff & " days"
            Else
                Me.lblCreditTerm.Text = ""
            End If
        Else
            If dsIPPDoc.Tables(0).Rows(0)("IM_PAYMENT_TERM") = "BC" Then
                label8.Text = "Bank Code :"
                Me.lblBankNameAccountNo.Text = dsIPPDoc.Tables(0).Rows(0)("ic_bank_code")
            Else
                label8.Text = "Bank Code[Bank A/C No.] :"
                Me.lblBankNameAccountNo.Text = dsIPPDoc.Tables(0).Rows(0)("ic_bank_code") & "[" & dsIPPDoc.Tables(0).Rows(0)("ic_bank_acct") & "]"

            End If
            'Zulham 051102018
            Me.lblPaymentMethod.Text = Common.parseNull(dsIPPDoc.Tables(0).Rows(0)("IC_PAYMENT_METHOD"))

            'Zulham 21112018
            Select Case lblPaymentMethod.Text
                Case "IBG"
                    lblPaymentMethodFull.Text = "LOCAL BANK TRANSFER-(RM)"
                Case "TT"
                    lblPaymentMethodFull.Text = "TELEGRAPHIC TRANSFER-(FOREIGN CURRENCY)"
                Case "BC"
                    lblPaymentMethodFull.Text = "CHEQUE-(RM)"
                Case "BD"
                    lblPaymentMethodFull.Text = "BANK DRAFT-(FOREIGN CURRENCY)"
                Case "CO"
                    lblPaymentMethodFull.Text = "CASHIER'S ORDER-(RM)"
                Case ""
                    lblPaymentMethodFull.Text = ""
            End Select

            'Zulham 02/03/2015 IPP GST Stage 2B
            If lblPaymentMethod.Text.Trim.ToLower = "nostro" Then
                lblPaymentMethod.Text = "Nostro Expenses"
            End If

            'Me.txtVendor.Text = dsIPPDoc.Tables(0).Rows(0)("IC_COY_NAME")
            Dim strGSTCode = ""
            Dim GST As New GST
            Dim createdDate = Common.FormatWheelDate(WheelDateFormat.LongDate, Date.Today)
            Dim _cutoffDate = System.Configuration.ConfigurationManager.AppSettings.Get("GstCutOffDate")
            If CDate(createdDate) > CDate(_cutoffDate) Then strGSTCode = GST.chkGST_ForIPP(ViewState("venidx"))

            If Not Common.parseNull(strGSTCode) = "" Then
                Me.txtVendor.Text = dsIPPDoc.Tables(0).Rows(0)("IM_S_COY_NAME") & "(" & strGSTCode & ")"
            Else
                'Me.txtVendor.Text = dsIPPDoc.Tables(0).Rows(0)("IM_S_COY_NAME")
                'Zulham 23/12/2015 (IPP Stage 4 Phase 2)
                'added empId after the emp's name
                Dim strCoyType = objDB.Get1Column("ipp_company", "ic_coy_type", " WHERE ic_index = '" & ViewState("venidx") & "'")
                If strCoyType = "E" Then
                    Dim empId = objDB.GetVal("select ifnull(ic_business_reg_no,'') from ipp_company where ic_index = '" & ViewState("venidx") & "'")
                    Me.txtVendor.Text = dsIPPDoc.Tables(0).Rows(0)("IM_S_COY_NAME") & "(" & empId & ")"
                Else
                    Me.txtVendor.Text = dsIPPDoc.Tables(0).Rows(0)("IM_S_COY_NAME")
                End If
            End If

            Me.txtVenAddL1.Text = dsIPPDoc.Tables(0).Rows(0)("IC_ADDR_LINE1")
            Me.txtVenAddL2.Text = dsIPPDoc.Tables(0).Rows(0)("IC_ADDR_LINE2")
            Me.txtVenAddL3.Text = dsIPPDoc.Tables(0).Rows(0)("IC_ADDR_LINE3")
            Me.txtVenAddPostcode.Text = dsIPPDoc.Tables(0).Rows(0)("IC_POSTCODE")
            Me.txtVenAddCity.Text = dsIPPDoc.Tables(0).Rows(0)("IC_CITY")
            Me.txtVenAddState.Text = dsIPPDoc.Tables(0).Rows(0)("IC_STATE")
            Me.txtVenAddCountry.Text = dsIPPDoc.Tables(0).Rows(0)("IC_COUNTRY")
            Me.lblCreditTerm.Text = dsIPPDoc.Tables(0).Rows(0)("IC_CREDIT_TERMS") & " days"
            hid6.Value = dsIPPDoc.Tables(0).Rows(0)("IM_S_COY_ID")
        End If
        Me.hidVendorIndex.Value = dsIPPDoc.Tables(0).Rows(0)("IM_S_COY_ID")
        ViewState("oldvenidx") = Me.hidVendorIndex.Value
        ViewState("venidx") = Me.hidVendorIndex.Value
    End Sub
    Private Sub renderIPPApprFlow()
        Dim objDoc As New IPPMain

        'Zulham 07032019
        Dim ds As DataSet = objDoc.getE2PApprFlow(Session("DocNo"), Common.Parse(HttpContext.Current.Session("CompanyID")))

        intPageRecordCnt = ds.Tables(0).Rows.Count
        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression2")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression2") <> "" Then dvViewSample.Sort += " DESC"

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            dtgApprvFlow.DataSource = ds.Tables(0).DefaultView
            dtgApprvFlow.DataBind()
        Else
            dtgApprvFlow.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If
        objDoc = Nothing
    End Sub

    Private Sub dtgApprvFlow_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgApprvFlow.ItemDataBound
        Dim dblInvoiceAmount, prevAppType As String

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim intTotalCell, intLoop As Integer

            If Not IsDBNull(dv("FA_ACTION_DATE")) Then
                e.Item.Cells(2).Text = dv("FA_ACTION_DATE")
            End If
        End If
    End Sub

    Private Sub btnContinue_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnContinue.Click
        Dim compidx As Integer
        Dim x As Integer
        Dim strscript As New System.Text.StringBuilder
        Session("SelectedSubDesc") = Nothing
        Session("rowIdx") = Nothing
        If vldsum.InnerHtml <> "" Then
            vldsum.InnerHtml = ""
        End If

        If validateField2() Then

            If Session("Action") = "New" Then
                compidx = ViewState("venidx")

                'Jules 2018.07.09 - Allow "\" and "#" in Document No.
                'If objDoc.CheckDuplicatedEntry(compidx, txtDocNo.Text) Then
                If objDoc.CheckDuplicatedEntry(compidx, Replace(txtDocNo.Text, "\", "\\")) Then 'End modification.

                    strMsg = objGlobal.GetErrorMessage("00002")
                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    cmdSave.Style("display") = "none"

                    'Jules 2018.07.09 - Allow "\" and "#" in Document No.
                    'ElseIf objDoc.CheckDuplicatedEntry2(txtPaymentAmt.Text, Common.ConvertDate(txtDocDate.Value), compidx, txtDocNo.Text) Then 'Prompt user there is invoice with same amount, vendor, doc date whether want to continue                 
                ElseIf objDoc.CheckDuplicatedEntry2(txtPaymentAmt.Text, Common.ConvertDate(txtDocDate.Value), compidx, Replace(txtDocNo.Text, "\", "\\")) Then 'Prompt user there is invoice with same amount, vendor, doc date whether want to continue                 
                    strscript.Append("<script language=""javascript"">")
                    strscript.Append("document.getElementById('hidBtnContinue').click();")
                    strscript.Append("</script>")
                    RegisterStartupScript("script3", strscript.ToString())
                Else
                    Dim strFileName As String
                    SaveIPPDoc("")
                    hid4.Value = txtDocNo.Text
                    cmdSubmit.Style("display") = "on"
                    btnContinue.Style("display") = "none"
                    btnContinueMultiGL.Style("display") = "none"
                    btnContinueMultiGL.Visible = False
                    'Zulham 27030219
                    cmdMultiInvoice.Visible = False
                    strscript.Append("<script language=""javascript"">")
                    strFileName = dDispatcher.direct("IPP", "IPPEntryPop.aspx", "pageid=" & strPageId & "&docno=" & Server.UrlEncode(txtDocNo.Text) & "&doctype=" & Server.UrlEncode(ddlDocType.SelectedItem.Text) & "&vencomp=" & Server.UrlEncode(removeGSTCode(txtVendor.Text)) & "&olddocno=" & Server.UrlEncode(hid4.Value) & "&venIdx=" & Server.UrlEncode(ViewState("venidx")) & "&MasterDoc=" & Me.rbtnMasterDoc.SelectedItem.Text & "&isResident=" & isResident & "&ic_index=" & ViewState("venidx")) 'Feb 28, 2014
                    strFileName = Server.UrlEncode(strFileName)
                    strscript.Append("ShowDialog(""" & dDispatcher.direct("IPP", "Dialog.aspx", "page=" & strFileName) & """,'600px');")
                    strscript.Append("document.getElementById('btnhidden3').click();")
                    strscript.Append("</script>")
                    RegisterStartupScript("script4", strscript.ToString())
                    cmdSave.Style("display") = ""
                End If

            End If

        End If
    End Sub

    Sub bindgrid()
        Try
            Dim objDoc As New IPPMain
            Dim DocIndex As String
            Dim ds As DataSet
            Dim objIPPDetail As New IPPDetails
            ViewState("docno") = Replace(Replace(txtDocNo.Text, "\", "\\"), "#", "\#")
            If Session("DocNo") Is Nothing Or Session("DocNo") = 0 Then
                If ViewState("oldvenidx") <> ViewState("venidx") And ViewState("oldvenidx") <> "" Then
                    DocIndex = objDB.GetVal("SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & Replace(Replace(hid4.Value, "\", "\\"), "#", "\#") & "' and im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' and im_s_coy_id = '" & ViewState("oldvenidx") & "'")
                Else
                    DocIndex = objDB.GetVal("SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & Replace(Replace(hid4.Value, "\", "\\"), "#", "\#") & "' and im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' and im_s_coy_id = '" & ViewState("venidx") & "'")
                End If
                If DocIndex.ToString = "" Then
                    DocIndex = objDB.GetVal("SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & Replace(Replace(hid4.Value, "\", "\\"), "#", "\#") & "' and im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' and im_s_coy_id = '" & ViewState("venidx") & "'")
                End If
            Else
                DocIndex = Session("DocNo")
            End If
            ViewState("DocNo") = DocIndex

            'Zulham 19082015
            Session("DocNo") = DocIndex

            If ViewState("oldvenidx") <> ViewState("venidx") And ViewState("oldvenidx") <> "" Then
                ds = objDoc.GetIPPDocDetails(ViewState("docno"), ViewState("oldvenidx"), , hid4.Value)
            Else
                ds = objDoc.GetIPPDocDetails(ViewState("docno"), ViewState("venidx"), , hid4.Value)
            End If
            If ds.Tables(0).Rows.Count = 0 Then
                ds = objDoc.GetIPPDocDetails(ViewState("docno"), ViewState("venidx"), , hid4.Value)
            End If
            Dim dsDocHeaderInfo As DataSet = objDoc.GetIPPDoc(DocIndex)

            If dsDocHeaderInfo.Tables(0).Rows(0)("IM_IND1").ToString.ToUpper = "Y" Then
                Me.rbtnMasterDoc.SelectedIndex = 0
            Else
                Me.rbtnMasterDoc.SelectedIndex = 1
            End If

            If dsDocHeaderInfo.Tables(0).Rows(0)("IM_INVOICE_STATUS") <> "14" Then '14 = Rejected
                If dsDocHeaderInfo.Tables(0).Rows(0)("IM_PAYMENT_TERM") = "BC" Then
                    If removeGSTCode(txtVendor.Text) <> hid5.Value And hid5.Value <> "" Then
                    Else
                        label8.Text = "Bank Code :"
                        Me.lblBankNameAccountNo.Text = dsDocHeaderInfo.Tables(0).Rows(0)("im_bank_code")
                    End If

                Else
                    If removeGSTCode(txtVendor.Text) <> hid5.Value And hid5.Value <> "" Then
                    Else
                        label8.Text = "Bank Code[Bank A/C No.] :"
                        'Zulham 17042015 IPP GST Stage 2B
                        If Not dsDocHeaderInfo.Tables(0).Rows(0)("im_bank_acct").ToString.Length = 0 Then
                            Me.lblBankNameAccountNo.Text = dsDocHeaderInfo.Tables(0).Rows(0)("im_bank_code") & "[" & dsDocHeaderInfo.Tables(0).Rows(0)("im_bank_acct") & "]"
                        Else
                            'Zulham 12/10/2015 - added conditions
                            'lblBankNameAccountNo.Text = ""
                            If Not dsDocHeaderInfo.Tables(0).Rows(0)("IC_bank_code") Is Nothing Then
                                If Not dsDocHeaderInfo.Tables(0).Rows(0)("IC_bank_code").ToString.Trim.Length = 0 Then
                                    lblBankNameAccountNo.Text = dsDocHeaderInfo.Tables(0).Rows(0)("IC_bank_code") & "[]"
                                End If
                            End If
                        End If
                    End If


                End If
                'Zulham 26032015 Case 8603
                If Not Page.IsPostBack Then
                    Me.txtBeneficiaryDetails.Text = Common.parseNull(dsDocHeaderInfo.Tables(0).Rows(0)("im_remarks2"))
                End If

            Else
                If dsDocHeaderInfo.Tables(0).Rows(0)("IM_PAYMENT_TERM") = "BC" Then
                    If removeGSTCode(txtVendor.Text) <> hid5.Value And hid5.Value <> "" Then
                    Else
                        label8.Text = "Bank Code :"
                        Me.lblBankNameAccountNo.Text = dsDocHeaderInfo.Tables(0).Rows(0)("ic_bank_code")
                    End If

                Else
                    If removeGSTCode(txtVendor.Text) <> hid5.Value And hid5.Value <> "" Then
                    Else
                        label8.Text = "Bank Code[Bank A/C No.] :"
                        Me.lblBankNameAccountNo.Text = dsDocHeaderInfo.Tables(0).Rows(0)("ic_bank_code") & "[" & dsDocHeaderInfo.Tables(0).Rows(0)("ic_bank_acct") & "]"
                    End If
                End If
                Me.txtBeneficiaryDetails.Text = Common.parseNull(dsDocHeaderInfo.Tables(0).Rows(0)("im_remarks2"))
            End If

            intPageRecordCnt = ds.Tables(0).Rows.Count

            Dim dvViewSample As DataView
            dvViewSample = ds.Tables(0).DefaultView
            dvViewSample.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewSample.Sort += " DESC"

            'Bind SubDoc grid if there's any
            If Not Session("SubDocument") Is Nothing Then
                If Not Me.rbtnMasterDoc.SelectedIndex = 1 Then
                    Dim totalAmt As Double : totalAmt = 0.0
                    Dim dt As New DataTable : dt = createSubDocdt()
                    If Not CType(Session("SubDocument"), ArrayList).Count = 0 Then
                        For i As Integer = 0 To CType(Session("SubDocument"), ArrayList).Count - 1
                            Dim row As DataRow = dt.NewRow
                            row("LineNo") = CType(Session("SubDocument"), ArrayList)(i)(0)
                            row("isd_doc_no") = CType(Session("SubDocument"), ArrayList)(i)(1)
                            row("isd_doc_date") = CType(Session("SubDocument"), ArrayList)(i)(2)
                            row("isd_doc_amt") = Format(CDbl(CType(Session("SubDocument"), ArrayList)(i)(3)), "#,###.00")
                            row("isd_sub_doc_index") = CType(Session("SubDocument"), ArrayList)(i)(4)
                            If Not CType(Session("SubDocument"), ArrayList)(i)(5).ToString = "0.0000" Then
                                If CDec(CType(Session("SubDocument"), ArrayList)(i)(5).ToString) < CDec(1) Then
                                    'Zulham 19082015
                                    If CType(Session("SubDocument"), ArrayList)(i)(5).ToString = "0" Then
                                        row("isd_doc_gst_value") = "0.00"
                                    Else
                                        row("isd_doc_gst_value") = CType(Session("SubDocument"), ArrayList)(i)(5).ToString.Substring(0, CType(Session("SubDocument"), ArrayList)(i)(5).ToString.Length - 2)
                                    End If
                                Else
                                    row("isd_doc_gst_value") = Format(CDbl(CType(Session("SubDocument"), ArrayList)(i)(5)), "#,###.00")
                                End If
                                totalAmt += CDbl(CType(Session("SubDocument"), ArrayList)(i)(3)) + CDbl(CType(Session("SubDocument"), ArrayList)(i)(5))
                            Else
                                row("isd_doc_gst_value") = ("0.00")
                                totalAmt += CDbl(CType(Session("SubDocument"), ArrayList)(i)(3))
                            End If
                            dt.Rows.Add(row)
                        Next
                        dt.AcceptChanges()
                    Else
                        dt = Nothing
                    End If
                    dvViewSample = dt.DefaultView
                    dvViewSample.Sort = ViewState("SortExpression")
                    If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewSample.Sort += " DESC"
                    resetDatagridPageIndex(dtgSubDoc, dvViewSample)
                    Me.dtgSubDoc.DataSource = dvViewSample
                    Me.dtgSubDoc.DataBind()
                    AddSubDocRowTotal(totalAmt)
                    divSubDoc.Visible = True
                    ViewState("PageCount") = dtgSubDoc.PageCount
                End If
            Else
                If Not Me.rbtnMasterDoc.SelectedIndex = 1 Then
                    Dim dsSubDoc As New DataSet : dsSubDoc = objDoc.GetSubDoc(ViewState("DocNo"))
                    Dim totalAmt As Double : totalAmt = 0.0
                    Dim rownum As Integer = 1
                    Dim dt As New DataTable : dt = createSubDocdt()
                    If dsSubDoc.Tables(0).Rows.Count > 0 Then
                        Dim arySubDoc As New ArrayList
                        Try
                            For count As Integer = 0 To dsSubDoc.Tables(0).Rows.Count - 1
                                arySubDoc.Add(New String() {count + 1, dsSubDoc.Tables(0).Rows(count).Item("isd_doc_no").ToString, dsSubDoc.Tables(0).Rows(count).Item("isd_doc_date").ToString,
                                dsSubDoc.Tables(0).Rows(count).Item("isd_doc_amt").ToString, CInt(dsSubDoc.Tables(0).Rows(count).Item("isd_sub_doc_index")), dsSubDoc.Tables(0).Rows(count).Item("isd_doc_gst_value").ToString})
                            Next
                        Catch ex As Exception
                        End Try
                        Session("SubDocument") = arySubDoc
                        For i As Integer = 0 To CType(Session("SubDocument"), ArrayList).Count - 1
                            Dim row As DataRow = dt.NewRow
                            row("LineNo") = CType(Session("SubDocument"), ArrayList)(i)(0)
                            row("isd_doc_no") = CType(Session("SubDocument"), ArrayList)(i)(1)
                            row("isd_doc_date") = CType(Session("SubDocument"), ArrayList)(i)(2)
                            row("isd_doc_amt") = Format(CDbl(CType(Session("SubDocument"), ArrayList)(i)(3)), "#,###.00")
                            row("isd_sub_doc_index") = CType(Session("SubDocument"), ArrayList)(i)(4)
                            If Not CType(Session("SubDocument"), ArrayList)(i)(5).ToString = "0.0000" Then
                                If CDec(CType(Session("SubDocument"), ArrayList)(i)(5).ToString) < CDec(1) Then
                                    row("isd_doc_gst_value") = CType(Session("SubDocument"), ArrayList)(i)(5).ToString.Substring(0, CType(Session("SubDocument"), ArrayList)(i)(5).ToString.Length - 2)
                                Else
                                    row("isd_doc_gst_value") = Format(CDbl(CType(Session("SubDocument"), ArrayList)(i)(5)), "#,###.00")
                                End If
                                totalAmt += CDbl(CType(Session("SubDocument"), ArrayList)(i)(3)) + CDbl(CType(Session("SubDocument"), ArrayList)(i)(5))
                            Else
                                row("isd_doc_gst_value") = ("0.00")
                                totalAmt += CDbl(CType(Session("SubDocument"), ArrayList)(i)(3))
                            End If
                            dt.Rows.Add(row)
                        Next
                        dt.AcceptChanges()
                        dvViewSample = dt.DefaultView
                        dvViewSample.Sort = ViewState("SortExpression")
                        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewSample.Sort += " DESC"
                        resetDatagridPageIndex(dtgSubDoc, dvViewSample)
                        Me.dtgSubDoc.DataSource = dvViewSample
                        Me.dtgSubDoc.DataBind()
                        AddSubDocRowTotal(totalAmt)
                        divSubDoc.Visible = True
                        ViewState("PageCount") = dtgSubDoc.PageCount
                    Else
                        divSubDoc.Visible = False
                    End If
                Else
                    divSubDoc.Visible = False
                End If
            End If

            'End

            '//bind datagrid

            'IPP Gst Stage 1 issue - CH - 12 Mar 2015
            ViewState("lineno") = 0
            ViewState("GSTTotalInput") = 0.0
            ViewState("GSTTotalOutput") = 0.0

            If intPageRecordCnt > 0 Then
                dtgInvDetail.DataSource = ds.Tables(0).DefaultView
                dtgInvDetail.DataBind()
            Else
                'Zulham 25052015 IPP GST Stage 1
                'If there's no data, show nothing
                dtgInvDetail.DataSource = Nothing
                dtgInvDetail.DataBind()
                If Not Page.IsPostBack Then
                    Common.NetMsgbox(Me, MsgNoRecord)
                End If
            End If
            objDoc = Nothing
            cmdAddLine.Style("display") = ""
            cmdMultiGL.Style("display") = ""
            If Not rbtnCoyType.SelectedValue = "E" Then
                cmdMultiGL.Visible = True
            Else
                cmdMultiGL.Visible = False
                Me.rbtnMasterDoc.Enabled = False
            End If
            If rbtnMasterDoc.SelectedValue.ToString = "1" And Me.dtgInvDetail.Items.Count > 0 Then
                cmdSubDoc.Visible = True
                If dtgSubDoc.Items.Count > 0 Then
                    divSubDoc.Visible = True
                End If
            Else
                cmdSubDoc.Visible = False
                divSubDoc.Visible = False
            End If
            AddRowTotal()
        Catch ex As Exception

        End Try
    End Sub
    Sub dtgSubDoc_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgSubDoc.PageIndexChanged
        dtgSubDoc.CurrentPageIndex = e.NewPageIndex
        bindgrid()
    End Sub
    Private Sub cmdAddLine_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddLine.Click
        Session("SelectedSubDesc") = Nothing
        Session("rowIdx") = Nothing
        Dim strscript As New System.Text.StringBuilder
        Dim rowcount As String
        Dim objipp As New IPPMain
        Dim strFileName As String
        If validateInput() Then
            'Save first
            SaveIPPDoc("Save", "cmdAddLine")

            'Jules 2018.07.09 - Allow "\" and "#" in Document No.
            'rowcount = objDB.GetMax("INVOICE_DETAILS", "ID_INVOICE_LINE", " WHERE ID_INVOICE_NO = '" & txtDocNo.Text & "' AND ID_S_COY_ID = '" & ViewState("venidx") & "'")
            rowcount = objDB.GetMax("INVOICE_DETAILS", "ID_INVOICE_LINE", " WHERE ID_INVOICE_NO = '" & Replace(txtDocNo.Text, "\", "\\") & "' AND ID_S_COY_ID = '" & ViewState("venidx") & "'") 'End modification.
            'Zulham 07112018
            If ViewState("SaveFailed") <> "Yes" Then
                strscript.Append("<script language=""javascript"">")
                strFileName = dDispatcher.direct("IPP", "IPPEntryPop.aspx", "pageid=" & strPageId & "&docno=" & Server.UrlEncode(txtDocNo.Text) & "&doctype=" & Server.UrlEncode(ddlDocType.SelectedItem.Text) & "&rowcount=" & rowcount & "&vencomp=" & Server.UrlEncode(hid5.Value) & "&olddocno=" & Server.UrlEncode(hid4.Value) & "&MasterDoc=" & Me.rbtnMasterDoc.SelectedItem.Text & "&isResident=" & isResident & "&ic_index=" & ViewState("venidx"))
                strFileName = Server.UrlEncode(strFileName)
                strscript.Append("ShowDialog(""" & dDispatcher.direct("IPP", "Dialog.aspx", "page=" & strFileName) & """,'520px');")
                strscript.Append("document.getElementById('btnhidden3').click();")
                strscript.Append("</script>")
                RegisterStartupScript("script5", strscript.ToString())
            End If
        End If
    End Sub

    Private Sub dtgInvDetail_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgInvDetail.ItemCreated
        Grid_ItemCreated(dtgInvDetail, e)
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(EnumDocDetail.ichidSNO).Style("display") = "none"
            If ddlDocType.SelectedItem.Value <> "Credit Note" And ddlDocType.SelectedItem.Value <> "Debit Note" Then
                e.Item.Cells(EnumDocDetail.icRefNo).Style("display") = "none"
            End If
        End If

    End Sub

    Private Sub dtgInvDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgInvDetail.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Try
                Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
                Dim LineNo As Label
                Dim cmdremove, cmdedit As HtmlGenericControl
                cmdremove = e.Item.FindControl("cmdremove")
                cmdremove.InnerHtml = "<img src='" & dDispatcher.direct("Plugins/Images", "i_delete2.gif") & "' />"
                cmdremove.Attributes.Add("onclick", "remove('" & dv("ID_INVOICE_LINE") & "')")

                cmdedit = e.Item.FindControl("cmdedit")
                cmdedit.InnerHtml = "<img src='" & dDispatcher.direct("Plugins/Images", "i_edit2.gif") & "' />"


                cmdedit.Attributes.Add("onclick", "edit('" & dv("ID_INVOICE_LINE") & "')")

                LineNo = e.Item.FindControl("LineNo")
                ViewState("lineno") = ViewState("lineno") + 1
                LineNo.Text = ViewState("lineno")
                e.Item.Cells(EnumDocDetail.ichidSNO).Style("display") = "none"
                e.Item.Cells(EnumDocDetail.icAmt).Text = (Format(CDbl(e.Item.Cells(EnumDocDetail.icAmt).Text), "#,###.00"))
                e.Item.Cells(EnumDocDetail.icUnitPrice).Text = (Format(CDbl(e.Item.Cells(EnumDocDetail.icUnitPrice).Text), "#,###.00"))
                If ddlDocType.SelectedItem.Value <> "Credit Note" And ddlDocType.SelectedItem.Value <> "Debit Note" Then
                    e.Item.Cells(EnumDocDetail.icRefNo).Style("display") = "none"
                End If
                e.Item.Cells(EnumDocDetail.icGLCode).Text = dv("ID_B_GL_CODE") & ":" & dv("CBG_B_GL_DESC")

                If UCase(Common.parseNull(dv("ID_PAY_FOR"))) = UCase(Session("CompanyId")) Then
                    e.Item.Cells(EnumDocDetail.icPayFor).Text = "Own Company"
                Else
                    e.Item.Cells(EnumDocDetail.icPayFor).Text = Common.parseNull(dv("ID_PAY_FOR"))
                    e.Item.Cells(EnumDocDetail.icPayFor).ToolTip = objDB.GetVal("SELECT ic_coy_name FROM ipp_company WHERE ic_other_b_coy_code = '" & dv("ID_PAY_FOR") & "'")
                End If

                'Zulham 24102018
                Dim costCenterDesc = objDB.GetVal("SELECT cc_CC_desc FROM cost_centre WHERE cc_coy_id = '" & Session("CompanyID") & "' and cc_cc_code = '" & Common.parseNull(dv("ID_COST_CENTER")) & "'")
                e.Item.Cells(EnumDocDetail.icCostCenter).Text = Common.parseNull(dv("ID_COST_CENTER")) & ":" & IIf(Common.parseNull(dv("ID_COST_CENTER_DESC")) = "", costCenterDesc, Common.parseNull(dv("ID_COST_CENTER_DESC")))

                If Common.parseNull(dv("ID_GST_REIMB")) = "R" Then
                    e.Item.Cells(EnumDocDetail.icReimbursement).Text = "Reimbursement"
                ElseIf Common.parseNull(dv("ID_GST_REIMB")) = "D" Then
                    e.Item.Cells(EnumDocDetail.icReimbursement).Text = "Disbursement"
                Else
                    e.Item.Cells(EnumDocDetail.icReimbursement).Text = "N/A"
                End If

                Dim GST As New GST
                Dim result As New DataSet

                If Not Common.parseNull(dv("ID_GST_INPUT_TAX_CODE")) = "" Then
                    If Not dv("ID_GST_INPUT_TAX_CODE").ToString = "0" Then
                        e.Item.Cells(EnumDocDetail.icInputTax).Text = Common.parseNull(dv("INPUT_GST"), "N/A")
                        'result = GST.GetTaxCode_forIPP()
                        'For record As Integer = 0 To result.Tables(0).Rows.Count - 1
                        '    If dv("ID_GST_INPUT_TAX_CODE").ToString.Trim = result.Tables(0).Rows(record).Item(1) Then
                        '        e.Item.Cells(EnumDocDetail.icInputTax).Text = result.Tables(0).Rows(record).Item(0).ToString.Trim
                        '    End If
                        'Next
                        'Zulham 08102018 - PAMB SST
                        If e.Item.Cells(EnumDocDetail.icInputTax).Text.Contains("(") Then
                            ViewState("GSTTotalInput") += CDec(dv("ID_GST_VALUE"))

                            'Zulham 11/02/2015 Case 8317
                            'get the value for im1,im3
                            If (e.Item.Cells(EnumDocDetail.icInputTax).Text.Contains("IM1") _
                            Or e.Item.Cells(EnumDocDetail.icInputTax).Text.Contains("IM3")) Then
                                ViewState("GSTTotalIM1IM3") += CDec(dv("ID_GST_VALUE"))
                            End If

                        End If
                    Else
                        e.Item.Cells(EnumDocDetail.icInputTax).Text = "N/A"
                    End If
                Else
                    e.Item.Cells(EnumDocDetail.icInputTax).Text = "N/A"
                End If
                If Not Common.parseNull(dv("ID_GST_OUTPUT_TAX_CODE")) = "" Then
                    e.Item.Cells(EnumDocDetail.icOutputTax).Text = Common.parseNull(dv("OUTPUT_GST"), "N/A")
                    'result = GST.GetTaxCode_forIPP()
                    'For record As Integer = 0 To result.Tables(0).Rows.Count - 1
                    '    If dv("ID_GST_OUTPUT_TAX_CODE").ToString.Trim = result.Tables(0).Rows(record).Item(1) Then
                    '        e.Item.Cells(EnumDocDetail.icOutputTax).Text = result.Tables(0).Rows(record).Item(0).ToString.Trim
                    '    End If
                    'Next
                    'Zulham 08102018 - PAMB SST
                    If e.Item.Cells(EnumDocDetail.icOutputTax).Text.Contains("(") Then

                        'Zulham 06052015 IPP GST Stage 1
                        If Not dv("ID_GST_INPUT_TAX_CODE").ToString.Trim.Contains("TX4") Then
                            ViewState("GSTTotalOutput") += CDec(dv("ID_GST_VALUE"))
                        End If

                    End If
                Else
                    e.Item.Cells(EnumDocDetail.icOutputTax).Text = "N/A"
                End If

                'Jules 2018.08.08
                If Common.parseNull(dv("id_gift")) = "" Then
                    e.Item.Cells(EnumDocDetail.icGift).Text = ""
                ElseIf dv("id_gift").ToString.Contains("N") Then
                    e.Item.Cells(EnumDocDetail.icGift).Text = "No"
                Else
                    e.Item.Cells(EnumDocDetail.icGift).Text = "Yes"
                End If
                'End modification.

                If Common.parseNull(dv("ID_GST_VALUE")) = "" Then
                    e.Item.Cells(EnumDocDetail.icGSTAmount).Text = "0.00"
                ElseIf dv("ID_GST_VALUE") = "" Then
                    e.Item.Cells(EnumDocDetail.icGSTAmount).Text = "0.00"
                End If

            Catch ex As Exception

            End Try
        End If

    End Sub
    Private Sub btnremoveline_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnremoveline.Click
        Dim slineno As String
        Dim objipp As New IPPMain
        Dim venidx, strCoyId As String
        'IPP Gst Stage 2A - CH - 11 Feb 2015
        If strDefIPPCompID = "" Then
            strCoyId = HttpContext.Current.Session("CompanyID")
        Else
            strCoyId = strDefIPPCompID
        End If
        If objDB.Exist("SELECT IC_INDEX FROM IPP_COMPANY WHERE IC_COY_NAME = '" & Common.Parse(removeGSTCode(txtVendor.Text)) & "' AND (IC_COY_TYPE = 'V' OR IC_COY_TYPE = 'E') AND IC_STATUS = 'A' AND IC_COY_ID = '" & Common.Parse(strCoyId) & "'") = 0 Then
            vldsum.InnerHtml = "<li>" & objGlobal.GetErrorMessage("00340") & "</li>"
            Exit Sub
        End If
        'Zulham 24/11/2015 - Changed the status from 'I' to 'A'
        'venidx = objipp.getIPPCompanyIndex(Common.Parse(hid5.Value), ViewState("IPPOfficer"), "I")
        venidx = objipp.getIPPCompanyIndex(Common.Parse(hid5.Value), ViewState("IPPOfficer"), "A")

        slineno = Request.Form("hidlinepointer")
        objipp.DelIPPDocDetail(slineno, Replace(Replace(hid4.Value, "\", "\\"), "#", "\#"), venidx)
        bindgrid()
    End Sub
    Private Sub btnremoveSubDocLine_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnremoveSubDocLine.Click
        Dim slineno As String
        Dim objipp As New IPPMain
        Dim venidx As String
        slineno = Request.Form("hidlinepointer")

        If Not slineno.ToString.Length = 0 Then objipp.DeleteSubDoc(slineno)
        'Remove deleted item from the array
        If Not Session("SubDocument") Is Nothing Then
            For i As Integer = 0 To CType(Session("SubDocument"), ArrayList).Count - 1
                If Not CType(Session("SubDocument"), ArrayList)(i)(4).ToString.Trim.Length = 0 Then
                    If slineno.ToString.Trim = CType(Session("SubDocument"), ArrayList)(i)(4).ToString.Trim Then
                        CType(Session("SubDocument"), ArrayList).RemoveAt(i)
                        Exit For
                    End If
                Else
                    If slineno.ToString.Trim = CType(Session("SubDocument"), ArrayList)(i)(1).ToString.Trim Then
                        CType(Session("SubDocument"), ArrayList).RemoveAt(i)
                        Exit For
                    End If
                End If
            Next
        End If

        bindgrid()
    End Sub
    Private Sub btnremoveSubDoc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnremoveSubDoc.Click
        Dim slineno As String
        Dim objipp As New IPPMain
        Dim venidx As String
        Dim strPayFor, strSQL, strAryQuery(0) As String
        strSQL = "delete from ipp_sub_doc where isd_mstr_doc_index = " & ViewState("DocNo")
        Common.Insert2Ary(strAryQuery, strSQL)
        strSQL = "update invoice_mstr set im_ind1 = 'N' where im_invoice_index = " & ViewState("DocNo")
        Common.Insert2Ary(strAryQuery, strSQL)
        objDB.BatchExecute(strAryQuery)
        bindgrid()
    End Sub
    Sub addCell(ByRef row As DataGridItem)
        Dim cell As TableCell = New TableCell
        row.Cells.Add(cell)
    End Sub
    Sub AddRowTotal()
        'adding totals row
        Dim total As String
        Dim objipp As New IPPMain
        Dim intL, intColToRemain As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim intTotalCol As Integer
        Dim venidx As String
        If hid4.Value = "" Then
            hid4.Value = txtDocNo.Text
        End If
        If ViewState("oldvenidx") <> ViewState("venidx") And ViewState("oldvenidx") <> "" Then
            venidx = ViewState("oldvenidx")
        Else
            venidx = ViewState("venidx")
        End If

        total = objipp.GetDocDetailTotalAmt(Replace(Replace(hid4.Value, "\", "\\"), "#", "\#"), venidx)
        If total <> "" Then
            total = total + CDec(ViewState("GSTTotalInput")) - CDec(ViewState("GSTTotalOutput"))
            ViewState("itemTotal") = total
            ViewState("itemTotalAmt") = total
            intTotalCol = EnumDocDetail.icCostCenter - 1

            For intL = 0 To intTotalCol
                addCell(row)
            Next

            Dim txtTotal As New Label
            txtTotal.ID = "TotalAmt"
            txtTotal.Text = (Format(CDbl(total), "#,###.00"))
            'Zulham 15/02/2016 - IPP GST Stage 4 Phase 2
            'Zulham 05102018 - PAMB SST
            row.Cells(EnumDocDetail.icGSTAmount).Controls.Add(txtTotal)
            row.Cells(EnumDocDetail.icGSTAmount).HorizontalAlign = HorizontalAlign.Right
            row.Cells(EnumDocDetail.icAmt).Text = "Total"
            row.Cells(EnumDocDetail.icAmt).Font.Bold = True
            If ddlDocType.SelectedItem.Text.Trim = "Credit Note" Or ddlDocType.SelectedItem.Text.Trim = "Debit Note" Then
                row.Cells(EnumDocDetail.icInputTax).Controls.Add(txtTotal)
                row.Cells(EnumDocDetail.icInputTax).HorizontalAlign = HorizontalAlign.Right
                row.Cells(EnumDocDetail.icAmt).Text = ""
                row.Cells(EnumDocDetail.icGSTAmount).Text = "Total"
                row.Cells(EnumDocDetail.icGSTAmount).Font.Bold = True
            End If
            dtgInvDetail.Controls(0).Controls.Add(row)
        End If

    End Sub

    Private Sub btnhidden5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhidden5.Click
        SaveIPPDoc("")
        cmdSubmit.Style("display") = "on"
        cmdSave.Style("display") = ""
        btnContinue.Style("display") = "none"
        btnContinueMultiGL.Style("display") = "none"
        btnContinueMultiGL.Visible = False
        'Zulham 27030219
        cmdMultiInvoice.Visible = False
        Dim strFileName As String
        Dim strscript As New System.Text.StringBuilder
        hid4.Value = txtDocNo.Text
        cmdSubmit.Style("display") = "on"
        btnContinue.Style("display") = "none"
        btnContinueMultiGL.Style("display") = "none"
        btnContinueMultiGL.Visible = False
        strscript.Append("<script language=""javascript"">")
        strFileName = dDispatcher.direct("IPP", "IPPEntryPop.aspx", "pageid=" & strPageId & "&docno=" & Server.UrlEncode(txtDocNo.Text) & "&doctype=" & Server.UrlEncode(ddlDocType.SelectedItem.Text) & "&vencomp=" & Server.UrlEncode(removeGSTCode(txtVendor.Text)) & "&olddocno=" & Server.UrlEncode(hid4.Value) & "&venIdx=" & Server.UrlEncode(ViewState("venidx")) & "&MasterDoc=" & Me.rbtnMasterDoc.SelectedItem.Text & "&isResident=" & isResident & "&ic_index=" & ViewState("venidx"))
        strFileName = Server.UrlEncode(strFileName)
        strscript.Append("ShowDialog(""" & dDispatcher.direct("IPP", "Dialog.aspx", "page=" & strFileName) & """,'520px');")
        strscript.Append("document.getElementById('btnhidden3').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script4", strscript.ToString())
        cmdSave.Style("display") = ""



    End Sub

    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        SaveIPPDoc("Save")

        If validateInput("a") Then
            If Not ViewState("SaveFailed") = "Yes" Then ViewState("oldvenidx") = ViewState("venidx")
            hidVendorIndex.Value = ViewState("venidx")
        End If

        ViewState("docno") = txtDocNo.Text
        hid4.Value = txtDocNo.Text
        bindgrid()

    End Sub
    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dtgInvDetail.SortCommand
        Grid_SortCommand(sender, e)
        dtgInvDetail.CurrentPageIndex = 0
        bindgrid()
    End Sub

    Private Sub btnhidden9_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhidden9.Click
        Dim strscript As New System.Text.StringBuilder
        Dim slineno As String
        Dim objipp As New IPPMain
        Dim strFileName, strCoyId As String

        'Zulham 07/03/2016 - IPP Stage 4 Phase 2
        'Save header data first
        SaveIPPDoc("Save", "", "edit")

        'IPP Gst Stage 2A - CH - 11 Feb 2015
        If strDefIPPCompID = "" Then
            strCoyId = HttpContext.Current.Session("CompanyID")
        Else
            strCoyId = strDefIPPCompID
        End If
        If objDB.Exist("SELECT IC_INDEX FROM IPP_COMPANY WHERE IC_COY_NAME = '" & Common.Parse(removeGSTCode(txtVendor.Text)) & "' AND (IC_COY_TYPE = 'V' OR IC_COY_TYPE = 'E') AND IC_STATUS = 'A' AND IC_COY_ID = '" & Common.Parse(strCoyId) & "'") = 0 Then
            vldsum.InnerHtml = "<li>" & objGlobal.GetErrorMessage("00340") & "</li>"
            Exit Sub
        End If

        'If objDB.Exist("SELECT IC_INDEX FROM IPP_COMPANY WHERE IC_COY_NAME = '" & Common.Parse(removeGSTCode(txtVendor.Text)) & "' AND (IC_COY_TYPE = 'V' OR IC_COY_TYPE = 'E') AND IC_STATUS = 'A' AND IC_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'") = 0 Then
        '    vldsum.InnerHtml = "<li>" & objGlobal.GetErrorMessage("00340") & "</li>"
        '    Exit Sub
        'End If

        slineno = Request.Form("hidlinepointer")
        strscript.Append("<script language=""javascript"">")
        strFileName = dDispatcher.direct("IPP", "IPPEntryPop.aspx", "pageid=" & strPageId & "&docno=" & Server.UrlEncode(txtDocNo.Text) & "&doctype=" & Server.UrlEncode(ddlDocType.SelectedItem.Text) & "&Lineno=" & slineno & "&action=edit&vencomp=" & Server.UrlEncode(hid5.Value) & "&olddocno=" & Server.UrlEncode(hid4.Value) & "&MasterDoc=" & Me.rbtnMasterDoc.SelectedItem.Text & "&isResident=" & isResident & "&ic_index=" & ViewState("venidx"))
        strFileName = Server.UrlEncode(strFileName)
        strscript.Append("ShowDialog(""" & dDispatcher.direct("IPP", "Dialog.aspx", "page=" & strFileName) & """,'520px');")
        strscript.Append("document.getElementById('btnhidden3').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script9", strscript.ToString())
    End Sub

    Private Sub btnContinueMultiGL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnContinueMultiGL.Click
        Dim compidx As Integer
        Dim x As Integer
        Dim strscript As New System.Text.StringBuilder
        'IPP Gst Stage 2A - CH - 11 Feb 2015
        Dim strCoyId As String
        If strDefIPPCompID = "" Then
            strCoyId = HttpContext.Current.Session("CompanyID")
        Else
            strCoyId = strDefIPPCompID
        End If

        If vldsum.InnerHtml <> "" Then
            vldsum.InnerHtml = ""
        End If
        If validateField2() Then
            If Session("Action") = "New" Then
                'IPP Gst Stage 2A - CH - 11 Feb 2015
                'If objDB.Exist("SELECT IC_INDEX FROM IPP_COMPANY WHERE IC_COY_NAME = '" & Common.Parse(removeGSTCode(txtVendor.Text)) & "' AND (IC_COY_TYPE = 'V' OR IC_COY_TYPE = 'E') AND IC_STATUS = 'A' AND IC_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'") > 0 Then
                '    compidx = objDoc.getIPPCompanyIndex(Common.Parse(removeGSTCode(txtVendor.Text)), ViewState("IPPOfficer"))
                'End If
                If objDB.Exist("SELECT IC_INDEX FROM IPP_COMPANY WHERE IC_COY_NAME = '" & Common.Parse(removeGSTCode(txtVendor.Text)) & "' AND (IC_COY_TYPE = 'V' OR IC_COY_TYPE = 'E') AND IC_STATUS = 'A' AND IC_COY_ID = '" & Common.Parse(strCoyId) & "'") > 0 Then
                    compidx = objDoc.getIPPCompanyIndex(Common.Parse(removeGSTCode(txtVendor.Text)), ViewState("IPPOfficer"))
                End If

                'Jules 2018.07.09 - Allow "\" and "#" in Document No.
                'If objDoc.CheckDuplicatedEntry(compidx, txtDocNo.Text) Then
                If objDoc.CheckDuplicatedEntry(compidx, Replace(txtDocNo.Text, "\", "\\")) Then 'End modification.
                    strMsg = objGlobal.GetErrorMessage("00002")
                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    cmdSave.Style("display") = "none"

                    'Jules 2018.07.09 - Allow "\" and "#" in Document No.
                    'ElseIf objDoc.CheckDuplicatedEntry2(txtPaymentAmt.Text, Common.ConvertDate(txtDocDate.Value), compidx, txtDocNo.Text) Then 'Prompt user there is invoice with same amount, vendor, doc date whether want to continue
                ElseIf objDoc.CheckDuplicatedEntry2(txtPaymentAmt.Text, Common.ConvertDate(txtDocDate.Value), compidx, Replace(Replace(txtDocNo.Text, "\", "\\"), "#", "\#")) Then 'Prompt user there is invoice with same amount, vendor, doc date whether want to continue 'End modification.
                    strscript.Append("<script language=""javascript"">")
                    strscript.Append("document.getElementById('hidBtnContinueMultiGL').click();")
                    strscript.Append("</script>")
                    RegisterStartupScript("script30", strscript.ToString())
                Else
                    Dim strFileName As String
                    SaveIPPDoc("")
                    hid4.Value = txtDocNo.Text
                    cmdSubmit.Style("display") = "on"
                    btnContinue.Style("display") = "none"
                    btnContinueMultiGL.Style("display") = "none"
                    btnContinueMultiGL.Visible = False
                    'Zulham 27030219
                    cmdMultiInvoice.Visible = False
                    strscript.Append("<script language=""javascript"">")
                    'Zulham 10102018 - PAMB 
                    strFileName = dDispatcher.direct("IPP", "IPPBatchUpload.aspx", "pageid=" & strPageId & "&docno=" & Server.UrlEncode(txtDocNo.Text) & "&doctype=" & Server.UrlEncode(ddlDocType.SelectedItem.Text) & "&vencomp=" & Server.UrlEncode(removeGSTCode(txtVendor.Text)) & "&olddocno=" & Server.UrlEncode(hid4.Value) & "&docdate=" & Server.UrlEncode(txtDocDate.Value) & "&mstrDoc=" & Me.rbtnMasterDoc.SelectedItem.Text)
                    strFileName = Server.UrlEncode(strFileName)
                    strscript.Append("ShowDialog(""" & dDispatcher.direct("IPP", "Dialog.aspx", "page=" & strFileName) & """,'520px');")
                    strscript.Append("document.getElementById('btnhidden3').click();")
                    strscript.Append("</script>")
                    RegisterStartupScript("script41", strscript.ToString())
                    cmdSave.Style("display") = ""
                End If
            Else
                Dim strFileName As String
                hid4.Value = txtDocNo.Text
                cmdSubmit.Style("display") = "on"
                btnContinue.Style("display") = "none"
                btnContinueMultiGL.Style("display") = "none"
                btnContinueMultiGL.Visible = False
                'Zulham 27030219
                cmdMultiInvoice.Visible = False
                strscript.Append("<script language=""javascript"">")
                'Zulham 10102018 - PAMB 
                strFileName = dDispatcher.direct("IPP", "IPPBatchUpload.aspx", "pageid=" & strPageId & "&docno=" & Server.UrlEncode(txtDocNo.Text) & "&doctype=" & Server.UrlEncode(ddlDocType.SelectedItem.Text) & "&vencomp=" & Server.UrlEncode(removeGSTCode(txtVendor.Text)) & "&olddocno=" & Server.UrlEncode(hid4.Value) & "&docdate=" & Server.UrlEncode(txtDocDate.Value) & "&mstrDoc=" & Me.rbtnMasterDoc.SelectedItem.Text)
                strFileName = Server.UrlEncode(strFileName)
                strscript.Append("ShowDialog(""" & dDispatcher.direct("IPP", "Dialog.aspx", "page=" & strFileName) & """,'520px');")
                strscript.Append("document.getElementById('btnhidden3').click();")
                strscript.Append("</script>")
                RegisterStartupScript("script42", strscript.ToString())
                cmdSave.Style("display") = ""
            End If
        End If
    End Sub

    Private Sub btnhidden6_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhidden6.Click
        SaveIPPDoc("")
        cmdSubmit.Style("display") = "on"
        cmdSave.Style("display") = ""
        btnContinue.Style("display") = "none"
        btnContinueMultiGL.Style("display") = "none"
        btnContinueMultiGL.Visible = False
        'Zulham 27030219
        cmdMultiInvoice.Visible = False
        Dim strFileName As String
        Dim strscript As New System.Text.StringBuilder
        hid4.Value = txtDocNo.Text
        cmdSubmit.Style("display") = "on"
        btnContinue.Style("display") = "none"
        btnContinueMultiGL.Style("display") = "none"
        btnContinueMultiGL.Visible = False
        strscript.Append("<script language=""javascript"">")
        'Zulham 10102018 - PAMB 
        strFileName = dDispatcher.direct("IPP", "IPPBatchUpload.aspx", "pageid=" & strPageId & "&docno=" & Server.UrlEncode(txtDocNo.Text) & "&doctype=" & Server.UrlEncode(ddlDocType.SelectedItem.Text) & "&vencomp=" & Server.UrlEncode(removeGSTCode(txtVendor.Text)) & "&olddocno=" & Server.UrlEncode(hid4.Value) & "&docdate=" & Server.UrlEncode(txtDocDate.Value))
        strFileName = Server.UrlEncode(strFileName)
        strscript.Append("ShowDialog(""" & dDispatcher.direct("IPP", "Dialog.aspx", "page=" & strFileName) & """,'420px');")
        strscript.Append("document.getElementById('btnhidden3').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script40", strscript.ToString())
        cmdSave.Style("display") = ""
    End Sub

    Private Sub cmdMultiGL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdMultiGL.Click
        Dim strscript As New System.Text.StringBuilder
        Dim rowcount As String
        Dim objipp As New IPPMain
        Dim strFileName As String

        SaveIPPDoc("Update")
        If validateInput("a") Then
            ViewState("oldvenidx") = ViewState("venidx")
            hidVendorIndex.Value = ViewState("venidx")
            strscript.Append("<script language=""javascript"">")
            'Zulham 10102018 - PAMB 
            strFileName = dDispatcher.direct("IPP", "IPPBatchUpload.aspx", "pageid=" & strPageId & "&docno=" & Server.UrlEncode(txtDocNo.Text) & "&doctype=" & Server.UrlEncode(ddlDocType.SelectedItem.Text) & "&vencomp=" & Server.UrlEncode(removeGSTCode(txtVendor.Text)) & "&olddocno=" & Server.UrlEncode(hid4.Value) & "&docdate=" & Server.UrlEncode(txtDocDate.Value) & "&venidx=" & ViewState("oldvenidx") & "&mstrDoc=" & Me.rbtnMasterDoc.SelectedItem.Text)
            strFileName = Server.UrlEncode(strFileName)
            strscript.Append("ShowDialog(""" & dDispatcher.direct("IPP", "Dialog.aspx", "page=" & strFileName) & """,'520px');")
            strscript.Append("document.getElementById('btnhidden3').click();")
            strscript.Append("</script>")
            RegisterStartupScript("script411", strscript.ToString())
            cmdSave.Style("display") = ""
        End If
    End Sub
    'Zulham 27032019
    Private Sub cmdMultiInvoice_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdMultiInvoice.Click
        Dim strscript As New System.Text.StringBuilder
        Dim strFileName As String

        If txtVendor.Text.Trim = "" And hid6.Value.Trim = "" Then
            Common.NetMsgbox(Me, "Please select vendor.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        'Zulham 22042019 
        strscript.Append("<script language=""javascript"">")
        strFileName = dDispatcher.direct("IPP", "IPPInvoicesBatchUpload.aspx", "pageid=" & strPageId & "&vencomp=" & Server.UrlEncode(removeGSTCode(txtVendor.Text)) & "&venType=" & rbtnCoyType.SelectedValue)
        strFileName = Server.UrlEncode(strFileName)
        strscript.Append("ShowDialog(""" & dDispatcher.direct("IPP", "Dialog.aspx", "page=" & strFileName) & """,'520px');")
        strscript.Append("document.getElementById('btnhidden3').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script101", strscript.ToString())

    End Sub


    Private Sub rbtnCoyType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnCoyType.SelectedIndexChanged
        PopulateVenTypeAhead(rbtnCoyType.SelectedValue)
        If rbtnCoyType.SelectedValue = "E" Then
            Me.rbtnMasterDoc.SelectedValue = 0
            Me.rbtnMasterDoc.Enabled = False
            Me.btnContinueMultiGL.Visible = False
            'Zulham 27030219
            cmdMultiInvoice.Visible = False
            Me.cmdMultiGL.Visible = False
        Else
            Me.rbtnMasterDoc.Enabled = True
            Me.btnContinueMultiGL.Visible = True
            'Zulham 27030219
            cmdMultiInvoice.Visible = True
            Me.cmdMultiGL.Visible = True
        End If
    End Sub

    Private Sub dtgSubDoc_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgSubDoc.ItemCreated
    End Sub

    Private Sub dtgSubDoc_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgSubDoc.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Try
                Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
                Dim LineNo As Label
                Dim cmdremove, cmdedit As HtmlGenericControl
                cmdremove = e.Item.FindControl("cmdremovesubdoc")
                cmdremove.InnerHtml = "<img src='" & dDispatcher.direct("Plugins/Images", "i_delete2.gif") & "' />"
                If dv("isd_sub_doc_index").ToString.Trim.Length > 0 Then
                    cmdremove.Attributes.Add("onclick", "removeSubDoc('" & dv("isd_sub_doc_index") & "')")
                Else
                    cmdremove.Attributes.Add("onclick", "removeSubDoc('" & dv("LineNo") & "')") 'For unsaved data, use line no instead
                End If

                cmdedit = e.Item.FindControl("cmdeditsubdoc")
                cmdedit.InnerHtml = "<img src='" & dDispatcher.direct("Plugins/Images", "i_edit2.gif") & "' />"
                cmdedit.Attributes.Add("onclick", "editSubDoc('" & dv("LineNo") & "')") 'For unsaved data, use line no instead
                dtgSubDoc.Columns(6).ItemStyle.ForeColor = Color.White
                dtgSubDoc.Columns(6).Visible = False
                dtgSubDoc.Columns(3).ItemStyle.HorizontalAlign = HorizontalAlign.Right
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub btnEditSubDoc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditSubDoc.Click
        Dim strscript As New System.Text.StringBuilder
        Dim slineno As String
        Dim objipp As New IPPMain
        Dim strFileName As String

        Session("DocNo_Amt") = txtDocNo.Text & "|" & Me.txtPaymentAmt.Text & "|" & ViewState("DocNo")
        slineno = Request.Form("hidlinepointer")
        strscript.Append("<script language=""javascript"">")

        'Jules 2018.07.11
        'strFileName = dDispatcher.direct("IPP", "IPPSubDocument.aspx", "pageid=" & strPageId & "&docno=" & txtDocNo.Text & "&doctype=" & Server.UrlEncode(ddlDocType.SelectedItem.Text) & "&LineNo=" & slineno & "&action=edit&vencomp=" & Server.UrlEncode(hid5.Value) & "&olddocno=" & hid4.Value & "&TotalAmt=" & Me.txtPaymentAmt.Text)
        strFileName = dDispatcher.direct("IPP", "IPPSubDocument.aspx", "doctype=" & Server.UrlEncode(ddlDocType.SelectedItem.Text) & "&LineNo=" & slineno & "&action=edit&vencomp=" & Server.UrlEncode(hid5.Value) &  "&TotalAmt=" & Me.txtPaymentAmt.Text)
        strFileName = Server.UrlEncode(strFileName)
        strscript.Append("showModalDialog(""" & dDispatcher.direct("IPP", "Dialog.aspx", "page=" & strFileName) & """,'600px');")
        strscript.Append("document.getElementById('btnhidden3').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script9", strscript.ToString())
    End Sub

    Private Sub cmdSubDoc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSubDoc.Click
        Dim strscript As New System.Text.StringBuilder
        Dim strFileName As String
        strscript.Append("<script language=""javascript"">")
        Session("DocNo_Amt") = txtDocNo.Text & "|" & Me.txtPaymentAmt.Text & "|" & ViewState("DocNo")
        strFileName = dDispatcher.direct("IPP", "IPPSubDocument.aspx")
        strscript.Append("showModalDialog(""" & dDispatcher.direct("IPP", "Dialog.aspx", "page=" & strFileName) & """);")
        strscript.Append("document.getElementById('btnhidden3').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script5", strscript.ToString())
    End Sub

    Private Function createSubDocdt() As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("LineNo", Type.GetType("System.String"))
        dt.Columns.Add("isd_doc_no", Type.GetType("System.String"))
        dt.Columns.Add("isd_doc_date", Type.GetType("System.String"))
        dt.Columns.Add("isd_doc_amt", Type.GetType("System.String"))
        dt.Columns.Add("isd_sub_doc_index", Type.GetType("System.String"))
        dt.Columns.Add("isd_doc_gst_value", Type.GetType("System.String"))
        dt.AcceptChanges()
        Return dt
    End Function

    Private Sub rbtnMasterDoc_selectedindexchanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnMasterDoc.SelectedIndexChanged
        If Not Me.rbtnMasterDoc.SelectedIndex = 1 Then
            If Not Session("SubDocument") Is Nothing Then
                Dim totalAmt As Double = 0.0
                Dim aryList As New ArrayList : aryList = CType(Session("SubDocument"), ArrayList)
                For i As Integer = 0 To CType(Session("SubDocument"), ArrayList).Count - 1
                    totalAmt += aryList(i)(3)
                Next
                AddSubDocRowTotal(totalAmt)
            End If
        Else
            If Me.dtgSubDoc.Items.Count > 0 Then
                If Not Page.ClientScript.IsStartupScriptRegistered(Me.GetType(), "alertscript") Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "removeSubDoc_All();", True)
                End If
            End If
            cmdSubDoc.Visible = False
            divSubDoc.Visible = False
        End If
        AddRowTotal()
    End Sub

    Sub AddSubDocRowTotal(Optional ByVal totalAmt As String = "")

        'adding totals row
        Dim total As String
        Dim item As DataGridItem
        Dim objipp As New IPPMain
        Dim intL, intColToRemain As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim intTotalCol As Integer
        Dim venidx As String
        If hid4.Value = "" Then
            hid4.Value = txtDocNo.Text
        End If

        If Me.dtgSubDoc.Items.Count > 0 Then
            For intL = 0 To 4
                addCell(row)
            Next
            Dim txtTotal As New Label
            txtTotal.ID = "TotalAmt"
            txtTotal.Text = (Format(CDbl(totalAmt), "#,###.00"))
            row.Cells(3).Controls.Add(txtTotal)
            row.Cells(3).HorizontalAlign = HorizontalAlign.Right
            row.Cells(2).HorizontalAlign = HorizontalAlign.Right
            row.Cells(2).Text = "Total:"
            row.Cells(2).Font.Bold = True
            row.Cells(3).Font.Bold = True
            dtgSubDoc.Controls(0).Controls.Add(row)
        End If

    End Sub

    Public Function removeGSTCode(ByVal strComp As String) As String

        'get the Comp
        If Not hid6.Value Is Nothing Then
            If Not hid6.Value.Trim.Length = 0 Then
                'mimi 2018-04-24 : remove hardcode HLB to PAMB
                strComp = objDB.GetVal("SELECT IFNULL(ic_coy_name, '') 'ic_coy_name' FROM IPP_COMPANY WHERE IC_INDEX = '" & Common.parseNull(hid6.Value) & "' AND (IC_COY_TYPE = 'V' OR IC_COY_TYPE = 'E') AND IC_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "'")
                'strComp = objDB.GetVal("SELECT IFNULL(ic_coy_name, '') 'ic_coy_name' FROM IPP_COMPANY WHERE IC_INDEX = '" & Common.parseNull(hid6.Value) & "' AND (IC_COY_TYPE = 'V' OR IC_COY_TYPE = 'E') AND IC_COY_ID = 'hlb'")
                Return strComp.Trim
            Else
                Return strComp.Trim
            End If
            Return strComp.Trim
        End If

    End Function
#Region "PAMB"
    Private Sub cmdUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpload.Click
        If File1Int.Value <> "" Then
            Dim objFile As New FileManagement
            Dim objPR As New PR

            ' Restrict user upload size
            'Dim dblMaxFileSize As Double = Val(objDB.GetVal("SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_TYPE = 'PR' AND SP_PARAM_NAME = 'DocAttachSize'"))
            Dim dblMaxFileSize As Double = Session("FileSize")

            Dim sFileName As String
            sFileName = System.IO.Path.GetFileName(File1Int.PostedFile.FileName)

            If Len(sFileName) > 50 Then
                Common.NetMsgbox(Me, "File name exceeds 46 character")
            ElseIf File1Int.PostedFile.ContentLength > 0 And File1Int.PostedFile.ContentLength / 1024 <= dblMaxFileSize Then
                If Session("mode") = "new" Then
                    objFile.FileUpload(File1Int, EnumUploadType.DocAttachmentTemp, "IPP", EnumUploadFrom.FrontOff, Session.SessionID, , , , , , "I")
                ElseIf Session("mode") = "modify" Then
                    objFile.FileUpload(File1Int, EnumUploadType.DocAttachmentTemp, "IPP", EnumUploadFrom.FrontOff, ViewState("DocNo"), , , , , , "I")
                End If
            ElseIf File1Int.PostedFile.ContentLength = 0 Then
                Common.NetMsgbox(Me, "0 byte document or file not found")
            Else
                Common.NetMsgbox(Me, "File exceeds maximum file size")
            End If

            objPR = Nothing
            objFile = Nothing
        End If

        displayAttachFile()
        bindgrid()

    End Sub
    Private Sub displayAttachFile()

        Dim objIPP As New IPP
        Dim dsAttach As New DataSet
        Dim drvAttach As DataView
        Dim i As Integer
        Dim strFile, strFile1, strURL, strTemp As String

        If Session("mode") = "new" Then
            dsAttach = objIPP.getIPPTempAttach(Session.SessionID, "I")
            If dsAttach.Tables(0).Rows.Count = 0 Then
                If Not ViewState("venidx") Is Nothing Then
                    'Jules 2018.07.05 - Allow "\" and "#" in Document No.
                    'Dim invoiceIndex = objDB.GetVal("select im_invoice_index from invoice_mstr where im_s_coy_id = '" & ViewState("venidx") & "' and im_invoice_no ='" & Me.txtDocNo.Text & "'")
                    Dim invoiceIndex = objDB.GetVal("select im_invoice_index from invoice_mstr where im_s_coy_id = '" & ViewState("venidx") & "' and im_invoice_no ='" & Replace(Replace(txtDocNo.Text, "\", "\\"), "#", "\#") & "'")
                    'End modification.

                    dsAttach = objIPP.getIPPTempAttach(invoiceIndex, "I")
                End If
            End If
        ElseIf Session("mode") = "modify" Then
            dsAttach = objIPP.getIPPTempAttach(Session("DocNo"), "I")
        End If

        pnlAttach.Controls.Clear()
        drvAttach = dsAttach.Tables(0).DefaultView
        If drvAttach.Count > 0 Then
            For i = 0 To drvAttach.Count - 1
                strFile = drvAttach(i)("CDA_ATTACH_FILENAME")
                strFile1 = drvAttach(i)("CDA_HUB_FILENAME")
                Dim objFile As New FileManagement
                'Zulham 19072018 - PAMB
                strURL = objFile.getAttachPathE2P(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DocAttachment, "IPP", EnumUploadFrom.FrontOff)
                objFile = Nothing
                Dim lblBr As New Label
                Dim lblFile As New Label
                Dim lnk As New ImageButton
                lblFile.Text = "&nbsp;" & i + 1 & ") " & strURL & " (" & drvAttach(i)("CDA_FILESIZE") & "KB) "
                lblBr.Text = "<BR>"
                lnk.ImageUrl = dDispatcher.direct("Plugins/images", "i_delete2.gif")
                lnk.ID = drvAttach(i)("CDA_ATTACH_INDEX")
                lnk.CausesValidation = False

                lnk.Attributes.Add("onclick", "removeFile('" & lnk.ID & "')")

                pnlAttach.Controls.Add(lblFile)

                If drvAttach(i)("CDA_STATUS") = "N" Then
                    pnlAttach.Controls.Add(lnk)
                End If

                pnlAttach.Controls.Add(lblBr)
            Next
        Else
            Dim lblFile As New Label
            lblFile.Text = "No Files Attached"
            pnlAttach.Controls.Add(lblFile)
        End If
        objIPP = Nothing

    End Sub
    Private Sub removeAttachment()
        Dim objPO As New PurchaseOrder_Buyer

        objPO.delete_Attachment_Temp(hidFileIndex.Value)
        displayAttachFile()
        bindgrid()
        hidFileIndex.Value = ""
        objPO = Nothing

    End Sub
#End Region


End Class
