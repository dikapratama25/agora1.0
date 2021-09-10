'Zulham 30-01-2015 IPP-GST Stage 2A
Imports AgoraLegacy
Imports eProcure.Component
Imports System.Text.RegularExpressions

Imports System.Drawing
Imports System.Windows.Forms.MessageBox

Public Class BillingEntry
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim dsDoc As New DataSet
    Dim aryDoc As New ArrayList
    Dim dsIPPDoc As New DataSet
    Dim objDB As New EAD.DBCom
    Dim objGlobal As New AppGlobals
    Dim objDoc As New Billing
    Dim objIPPMain As New IPPMain
    Dim dsIPPDocDetails As New DataSet
    Dim strMsg As String
    Dim strCoyType As String
    Dim isResident As Boolean = False
    Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID") 'Modified for IPP GST Stage 2A - CH - 13 Feb 2015

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
    Protected WithEvents txtNoTaxTotal As System.Web.UI.WebControls.TextBox
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


    'Zulham 20/06/2017
    'IPP Stage 3
    Public Enum EnumDocDetail
        icAction = 0
        icSNO = 1
        ichidSNO = 2
        icRefNo = 3
        icDescription = 4
        icUOM = 5
        icQTY = 6
        icUnitPrice = 7
        icAmt = 8
        icGSTAmount = 9
        icInputTax = 10
        icOutputTax = 11

        'Jules 2018.05.02 - PAMB Scrum 2
        'icGLCode = 12
        'icHOBR = 13
        'icCostCenter = 14
        icCategory = 12
        icGLCode = 13
        icAnalysisCode1 = 14
        icAnalysisCode2 = 15
        icAnalysisCode3 = 16
        icAnalysisCode4 = 17
        icAnalysisCode5 = 18
        icAnalysisCode8 = 19
        icAnalysisCode9 = 20
        icCostCenter = 21
        'End modification.
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

        SetGridProperty(dtgInvDetail, "N")
        'SetGridProperty(Me.dtgSubDoc, "Y")
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

        If Not Page.IsPostBack Then
            Session("SubDocument") = Nothing
            cmdAddLine.Style("display") = "none"
            cmdSave.Style("display") = "none"
            trLateReason.Style.Item("display") = "none"
            If Session("mode") = "modify" Then
                cmdAddLine.Style("display") = ""
                Session("Action") = "Edit"
                GenerateTab()
                PopulateForDDL()
                'populateLateReason()
                DisplayIPPDocInfo()
                strCoyType = objDB.Get1Column("ipp_company", "ic_coy_type", " WHERE ic_index = '" & ViewState("venidx") & "' AND ic_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'")
                PopulateVenTypeAhead(strCoyType)
                If strCoyType = "V" Or strCoyType = "C" Then
                    rbtnCoyType.SelectedValue = "C"
                ElseIf strCoyType = "B" Then
                    rbtnCoyType.SelectedValue = "R"
                Else
                    rbtnCoyType.SelectedValue = strCoyType
                End If
                'rbtnCoyType.SelectedValue = strCoyType
                hid4.Value = txtDocNo.Text
                ViewState("GSTTotalInput") = 0.0
                bindgrid()
                btnContinue.Style("display") = "none"
                cmdSave.Style("display") = ""
                hid5.Value = removeGSTCode(txtVendor.Text)
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
                hid5.Value = removeGSTCode(txtVendor.Text)
            End If

        Else
            If ddlCurrency.SelectedValue = "" Then
                Session("SelCurrency") = "MYR"
                ddlCurrency.SelectedValue = Session("SelCurrency")
                ddlCurrency.Enabled = False
            Else
                Session("SelCurrency") = ddlCurrency.SelectedValue
                If Not ddlCurrency.SelectedValue.ToUpper = "MYR" Then ddlCurrency.Enabled = True
                'ddlCurrency.Enabled = True
            End If
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

                    If rbtnCoyType.SelectedValue = "V" Then
                        If txtDocDate.Value <> "" Then
                            lblCreditTerm.Text = dsCompDetail.Tables(0).Rows(0)("IC_CREDIT_TERMS") & " days"
                            lblDocDueDate.Text = DateTime.Parse(txtDocDate.Value).AddDays(dsCompDetail.Tables(0).Rows(0)("IC_CREDIT_TERMS"))
                        End If
                    Else

                    End If

                    txtVenAddL1.Text = dsCompDetail.Tables(0).Rows(0)("IC_ADDR_LINE1")
                    txtVenAddL2.Text = dsCompDetail.Tables(0).Rows(0)("IC_ADDR_LINE2")
                    txtVenAddL3.Text = dsCompDetail.Tables(0).Rows(0)("IC_ADDR_LINE3")
                    txtVenAddPostcode.Text = dsCompDetail.Tables(0).Rows(0)("IC_POSTCODE")
                    txtVenAddCity.Text = dsCompDetail.Tables(0).Rows(0)("IC_CITY")
                    txtVenAddState.Text = dsCompDetail.Tables(0).Rows(0)("IC_STATE2")
                    txtVenAddCountry.Text = dsCompDetail.Tables(0).Rows(0)("IC_COUNTRY2")
                End If

                Session("dsCompDetail") = dsCompDetail
                If Session("Action") = "Edit" Then
                    ViewState("GSTTotalInput") = 0.0
                    bindgrid()
                End If
            End If
            If Session("Action") = "Edit" Then
                btnContinue.Style("display") = "none"
            End If
            Me.hidBtnContinue2.Attributes.Add("onClick", "return checkduplication2();")
            Me.hidBtnContinueSubmit.Attributes.Add("onClick", "return checkduplication3();")
        End If

        LateSubmit = objDoc.getLateSubmitCheck
        ViewState("LateSubmit") = LateSubmit
        If hidLateSubmit.Value = "" Then
            Me.cmdSubmit.Attributes.Add("onClick", "return late_submission(" & LateSubmit & ");")
        Else
            Me.cmdSubmit.Attributes.Remove("onClick")
        End If

        'Me.txtWHT.Attributes.Add("onkeypress", "return isNumberKey(event);")
        Me.txtDocNo.Attributes.Add("onkeypress", "return isNumberCharKey(event);")
        Me.txtPaymentAmt.Attributes.Add("onkeypress", "return isDecimalKey(event);")
        Me.txtPaymentAmt.Attributes.Add("onblur", "return isValidDecimalCheck();")
        Me.cmdVoid.Attributes.Add("onclick", "return confirm('" & strMsgVoid & "');")

        'Residents or Non
        If Not hidVendorId.Value Is Nothing Then
            If ViewState("venidx") Is Nothing Then hidVendorId.Value = "" Else hidVendorId.Value = ViewState("venidx")
        Else
            ViewState("venidx") = 0
        End If
        strIsResident = objDB.GetVal("SELECT ic_resident_type FROM IPP_company WHERE ic_index =  '" & Common.Parse(ViewState("venidx")) & "'")
        If strIsResident.ToString.Trim = "Y" Then
            isResident = True
            ddlCurrency.Enabled = False
        Else
            isResident = False
            ddlCurrency.Enabled = True
        End If
        'End

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
        Dim docDate As String = ""
        docDate = objDB.GetVal("select ifnull(im_created_on,'') from invoice_mstr where im_s_coy_id = '" & Common.parseNull(hid6.Value) & "' and im_invoice_no = '" & Common.Parse(txtDocNo.Text) & "'")
        Dim createdDate = Common.FormatWheelDate(WheelDateFormat.LongDate, Date.Today)
        Dim _cutoffDate = System.Configuration.ConfigurationManager.AppSettings.Get("GstCutOffDate")

        Dim _exceedCutOffDt As String = ""
        Dim strIsGst As String
        If CDate(createdDate) >= CDate(_cutoffDate) Then
            _exceedCutOffDt = "Yes"
            ViewState("_exceedCutOffDt") = "Yes"
            If txtVendor.Text <> "" And docDate <> "" Then
                Dim getVendor = ""
                If txtVendor.Text.Contains("(") Then
                    getVendor = txtVendor.Text.Split("(")(0).ToString.Trim
                Else
                    getVendor = txtVendor.Text
                End If
                Dim GSTRegNo = objDB.GetVal("SELECT IFNULL(IC_TAX_REG_NO, '') FROM IPP_COMPANY WHERE ic_index = '" & hid6.Value & "'")
                If GSTRegNo <> "" And docDate = "" Then
                    strIsGst = "Yes"
                ElseIf GSTRegNo <> "" And CDate(docDate) < CDate(_cutoffDate) Then
                    strIsGst = "No"
                ElseIf GSTRegNo <> "" And CDate(docDate) >= CDate(_cutoffDate) Then
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
        ViewState("_exceedCutOffDt") = _exceedCutOffDt
        ViewState("strIsGst") = strIsGst
        hidIsGST.Value = strIsGst
        hidexceedCutOffDt.Value = _exceedCutOffDt
        Session("NewPoInfo") = Nothing
        Session("ItemDeleted") = Nothing
        Dim Asset As New PurchaseOrder_Buyer
        If strIsGst = "Yes" Then
            dtgInvDetail.Columns(4).Visible = True
            dtgInvDetail.Columns(11).Visible = True
            dtgInvDetail.Columns(12).Visible = True
            dtgInvDetail.Columns(13).Visible = True
        ElseIf _exceedCutOffDt = "Yes" Then
            dtgInvDetail.Columns(4).Visible = True
            dtgInvDetail.Columns(11).Visible = True
            dtgInvDetail.Columns(12).Visible = True
            dtgInvDetail.Columns(13).Visible = True
        Else
            dtgInvDetail.Columns(4).Visible = False
            dtgInvDetail.Columns(11).Visible = False
            dtgInvDetail.Columns(12).Visible = False
            dtgInvDetail.Columns(13).Visible = False
        End If
        'End

        'Zulham 15042015 IPP GST Stage 2A
        'If ddlCurrency.SelectedItem.Text.ToUpper.Contains("MALAYSIA") Then
        '    ddlCurrency.Enabled = False
        'End If

    End Sub
    Sub populateLateReason()
        Dim objdb As New EAD.DBCom
        Dim latereason As String

        If Session("DocNo") Is Nothing Or Session("DocNo") = 0 Then
            If ViewState("oldvenidx") <> ViewState("venidx") And ViewState("oldvenidx") <> "" Then
                Session("DocNo") = objdb.GetVal("SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & hid4.Value & "' and im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' and im_s_coy_id = '" & ViewState("oldvenidx") & "'")
            Else
                Session("DocNo") = objdb.GetVal("SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & hid4.Value & "' and im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' and im_s_coy_id = '" & ViewState("venidx") & "'")
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


        Dim DocTypeArray(7) As String
        Dim strDocType As String
        DocTypeArray(0) = "Invoice"
        DocTypeArray(1) = "Non-Invoice"
        'Zulham 19/06/2017
        'IPP Stage 3
        DocTypeArray(2) = "Debit Note"
        DocTypeArray(3) = "Credit Note"
        DocTypeArray(4) = "Debit Advice"
        DocTypeArray(5) = "Credit Advice"
        DocTypeArray(6) = "Debit Note(Non-Invoice)"
        DocTypeArray(7) = "Credit Note(Non-Invoice)"


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

        'CoyType c=customer, r=related comp, e=employee
        'Zulham 18/08/2017 - IPP Stage 3
        If Me.ddlDocType.SelectedItem.Text = "Non-Invoice" Or Me.ddlDocType.SelectedItem.Text = "Debit Note(Non-Invoice)" _
        Or Me.ddlDocType.SelectedItem.Text = "Credit Note(Non-Invoice)" Then
            If strCoyType = "C" Or strCoyType = "V" Then
                vtypeahead = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=IPPCOY&coytype=V&module=billing&Nostro=Yes")
            ElseIf strCoyType = "E" Then
                vtypeahead = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=IPPCOY&coytype=E&module=billing&Nostro=Yes")
            Else
                vtypeahead = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=IPPCOY&coytype=R&module=billing&Nostro=Yes")
            End If
        Else
            If strCoyType = "C" Or strCoyType = "V" Then
                vtypeahead = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=IPPCOY&coytype=V&module=billing&Nostro=No")
            ElseIf strCoyType = "E" Then
                vtypeahead = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=IPPCOY&coytype=E&module=billing&Nostro=No")
            Else
                vtypeahead = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=IPPCOY&coytype=R&module=billing&Nostro=No")
            End If
        End If

        '
        'Zulham 15/02/2015 8317
        vencontent &= "$(""#txtVendor"").autocomplete(""" & vtypeahead & "&compid=" & strCompID & """, {" & vbCrLf & _
        "width: 200," & vbCrLf & _
        "scroll: true," & vbCrLf & _
        "selectFirst: false" & vbCrLf & _
        "});" & vbCrLf & _
        "var vendIdx = document.getElementById(""hidVendorId"").value;" & vbCrLf & _
        "var vendIdx2 = document.getElementById(""hid6"").value;" & vbCrLf & _
        "if(vendIdx == """")" & vbCrLf & _
        "{" & vbCrLf & _
        "};" & vbCrLf & _
        "$(""#txtVendor"").result(function(event,data,item) {" & vbCrLf & _
        "if (data)" & vbCrLf & _
        "$(""#hid6"").val(data[1]);" & vbCrLf & _
        "$(""#hidResidenceType"").val(data[11]);" & vbCrLf & _
        "$(""#hidPaymentType"").val(data[9]);" & vbCrLf & _
        "$(""#txtVendor"").val(data[0]);" & vbCrLf & _
        "$(""#txtVenAddL1"").val(data[2]);" & vbCrLf & _
        "$(""#txtVenAddL2"").val(data[3]);" & vbCrLf & _
        "$(""#txtVenAddL3"").val(data[4]);" & vbCrLf & _
        "$(""#txtVenAddPostcode"").val(data[5]);" & vbCrLf & _
        "$(""#txtVenAddCity"").val(data[6]);" & vbCrLf & _
        "$(""#txtVenAddState"").val(data[7]);" & vbCrLf & _
        "$(""#txtVenAddCountry"").val(data[8]);" & vbCrLf & _
        "$(""#ddlCurrency"").attr('selectedIndex', 13);" & vbCrLf & _
        "" & vbCrLf & _
        "if (data[8] == 'MY'){document.getElementById(""ddlCurrency"").disabled=true;}else{document.getElementById(""ddlCurrency"").disabled=false;}" & vbCrLf & _
        "if (data[13] == 'Y'){document.getElementById(""ddlCurrency"").disabled=false;}" & vbCrLf & _
        "$(""#lblPaymentMethod"").text(data[9]);" & vbCrLf & _
        "$(""#lblBankNameAccountNo"").text(data[10]);" & vbCrLf & _
        "$(""#hidGSTCode"").val(data[12]);" & vbCrLf & _
        "var gstCode = document.getElementById(""hidGSTCode"").value;" & vbCrLf & _
        "var vendorName = document.getElementById(""txtVendor"").value;" & vbCrLf & _
        "if(gstCode != """" ){ vendorName = vendorName + "" ("" + gstCode + "")"";$(""#txtVendor"").val(vendorName);};" & vbCrLf & _
        "});" & vbCrLf & _
        "$(""#txtVendor"").blur(function() {" & vbCrLf & _
        "var hidven = document.getElementById(""hid6"").value;" & vbCrLf & _
        "if(hidven == """")" & vbCrLf & _
        "{" & vbCrLf & _
        "$(""#txtVendor"").val("""");" & vbCrLf & _
        "$(""#txtVenAddL1"").val("""");" & vbCrLf & _
        "$(""#txtVenAddL2"").val("""");" & vbCrLf & _
        "$(""#txtVenAddL3"").val("""");" & vbCrLf & _
        "$(""#txtVenAddPostcode"").val("""");" & vbCrLf & _
        "$(""#txtVenAddCity"").val("""");" & vbCrLf & _
        "$(""#txtVenAddState"").val("""");" & vbCrLf & _
        "$(""#txtVenAddCountry"").val("""");" & vbCrLf & _
        "$(""#ddlCurrency"").attr('selectedIndex', 13);" & vbCrLf & _
        "//$(""#ddlCurrency"").attr('disabled', 'disabled');" & vbCrLf & _
        "$(""#lblPaymentMethod"").text("""");" & vbCrLf & _
        "$(""#lblBankNameAccountNo"").text("""");" & vbCrLf & _
        "$(""#txtPaymentAmt"").val("""");" & vbCrLf & _
        "$(""#hidGSTCode"").val("""");" & vbCrLf & _
        "$(""#rdbWHTComp"").attr('disabled', 'disabled');" & vbCrLf & _
        "$(""#rdbWHTVendor"").attr('disabled', 'disabled');" & vbCrLf & _
        "$(""#rdbNoWHT"").attr('disabled', 'disabled');" & vbCrLf & _
        "$(""#txtWHT"").val("""");" & vbCrLf & _
        "}" & vbCrLf & _
        "//var hidRes = document.getElementById(""hidResidenceType"").value;" & vbCrLf & _
        "//if(hidRes == ""N""){" & vbCrLf & _
        "//document.getElementById(""rdbWHTComp"").disabled = false;" & vbCrLf & _
        "//document.getElementById(""rdbWHTVendor"").disabled = false;" & vbCrLf & _
        "//document.getElementById(""rdbNoWHT"").disabled = false;" & vbCrLf & _
        "//document.getElementById(""txtWHT"").disabled = false;" & vbCrLf & _
        "//}else{" & vbCrLf & _
        "//document.getElementById(""rdbWHTComp"").disabled = true;" & vbCrLf & _
        "//document.getElementById(""rdbWHTVendor"").disabled = true;" & vbCrLf & _
        "//document.getElementById(""rdbNoWHT"").disabled = true;" & vbCrLf & _
        "//document.getElementById(""rdbWHTComp"").checked = false;" & vbCrLf & _
        "//document.getElementById(""rdbWHTVendor"").checked = false;" & vbCrLf & _
        "//document.getElementById(""rdbNoWHT"").checked = false;" & vbCrLf & _
        "//document.getElementById(""txtWHT"").disabled = true;" & vbCrLf & _
        "//};" & vbCrLf & _
        "//var payType = document.getElementById(""hidPaymentType"").value;" & vbCrLf & _
        "//if(payType == ""TT""){" & vbCrLf & _
        "//document.getElementById(""ddlCurrency"").disabled=false;" & vbCrLf & _
        "//}else{document.getElementById(""ddlCurrency"").disabled=true;$(""#ddlCurrency"").attr('selectedIndex', 13);};" & vbCrLf & _
        "});" & vbCrLf

        ' for edit purpose

        If Session("Action") = "Edit" Then
            ventypeahead = "<script language=""javascript"">" & vbCrLf & _
                      "<!--" & vbCrLf & _
                        "$(document).ready(function(){" & vbCrLf & _
                        vencontent & vbCrLf & _
                        "});" & vbCrLf & _
                        "-->" & vbCrLf & _
                        "</script>"
        Else
            ventypeahead = "<script language=""javascript"">" & vbCrLf & _
          "<!--" & vbCrLf & _
            "$(document).ready(function(){" & vbCrLf & _
            vencontent & vbCrLf & _
            "});" & vbCrLf & _
            "-->" & vbCrLf & _
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

        Session("w_AddPO_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Billing", "BillingEntry.aspx", "mode=new&pageid=" & strPageId) & """><span>Billing Document</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Billing", "BillingList.aspx", "type=Listing&mode=new&pageid=" & strPageId) & """><span>Billing Document Listing</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "</ul><div></div></div>"
    End Sub


    Private Sub rdbNoWHT_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbNoWHT.CheckedChanged
        rdbWHTComp.Checked = False
        rdbWHTVendor.Checked = False
        txtWHT.Enabled = False
        txtNoWHTReason.Enabled = True
        txtWHT.Text = ""
    End Sub

    Private Sub rdbWHTComp_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbWHTComp.CheckedChanged
        rdbNoWHT.Checked = False
        rdbWHTVendor.Checked = False
        txtNoWHTReason.Enabled = False
        txtWHT.Enabled = True
        txtNoWHTReason.Text = ""
    End Sub

    Private Sub rdbWHTVendor_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbWHTVendor.CheckedChanged
        rdbNoWHT.Checked = False
        rdbWHTComp.Checked = False
        txtNoWHTReason.Enabled = False
        txtWHT.Enabled = True
        txtNoWHTReason.Text = ""
    End Sub

    Private Sub btnGetAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetAdd.Click

        Dim dsCompDetail As New DataSet
        Dim objIPPMain As New IPPMain
        ViewState("oldvenidx") = hidVendorIndex.Value
        ViewState("venidx") = hid6.Value

        If txtVendor.Text <> "" Then
            dsCompDetail = objIPPMain.GetVendorAddress(Common.Parse(removeGSTCode(txtVendor.Text)), ViewState("IPPOfficer"), ViewState("venidx"))

            If rbtnCoyType.SelectedValue = "V" Then
                If txtDocDate.Value <> "" Then
                    lblCreditTerm.Text = dsCompDetail.Tables(0).Rows(0)("IC_CREDIT_TERMS") & " days"
                    lblDocDueDate.Text = DateTime.Parse(txtDocDate.Value).AddDays(dsCompDetail.Tables(0).Rows(0)("IC_CREDIT_TERMS"))
                Else
                    vldsum.InnerHtml = "<li>Document Date " & objGlobal.GetErrorMessage("00001") & "</li>"
                    txtVendor.Text = ""
                    Exit Sub
                End If
            Else

            End If

            txtVenAddL1.Text = dsCompDetail.Tables(0).Rows(0)("IC_ADDR_LINE1")
            txtVenAddL2.Text = dsCompDetail.Tables(0).Rows(0)("IC_ADDR_LINE2")
            txtVenAddL3.Text = dsCompDetail.Tables(0).Rows(0)("IC_ADDR_LINE3")
            txtVenAddPostcode.Text = dsCompDetail.Tables(0).Rows(0)("IC_POSTCODE")
            txtVenAddCity.Text = dsCompDetail.Tables(0).Rows(0)("IC_CITY")
            txtVenAddState.Text = dsCompDetail.Tables(0).Rows(0)("IC_STATE2")
            txtVenAddCountry.Text = dsCompDetail.Tables(0).Rows(0)("IC_COUNTRY2")
            lblPaymentMethod.Text = dsCompDetail.Tables(0).Rows(0)("IC_PAYMENT_METHOD")



            If dsCompDetail.Tables(0).Rows(0)("IC_PAYMENT_METHOD") = "BC" Then
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

            If dsCompDetail.Tables(0).Rows(0)("IC_PAYMENT_METHOD") = "RENTAS" Or dsCompDetail.Tables(0).Rows(0)("IC_PAYMENT_METHOD") = "TT" Then
                lblBeneficiaryDetailsMan.Visible = True
                txtBeneficiaryDetails.Enabled = True
                Me.ddlCurrency.Enabled = True
            Else
                lblBeneficiaryDetailsMan.Visible = False
                txtBeneficiaryDetails.Enabled = True
                Me.ddlCurrency.Enabled = False
            End If
        End If

        Session("dsCompDetail") = dsCompDetail
        If Session("Action") = "Edit" Then
            ViewState("GSTTotalInput") = 0.0
            bindgrid()
        End If

    End Sub
    Private Sub btnhidden3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhidden3.Click
        ViewState("GSTTotalInput") = 0.0
        'Zulham 21/06/2017
        'IPP Stage 3
        ViewState("lineno") = 0
        bindgrid()
        btnContinue.Style("display") = "none"
        'btnContinueMultiGL.Style("display") = "none"
        'btnContinueMultiGL.Visible = False
    End Sub
    Private Sub SaveBillingDoc(ByVal strFrm As String, Optional ByVal trigger As String = "")
        Dim dtDoc, dtSubDoc As New DataTable
        Dim dsDoc As New DataSet
        Dim objDoc As New Billing
        Dim objIPPMain As New IPPMain
        Dim CompIdx, OldCompIdx As Integer
        Dim dtDocDetail As New DataTable
        Dim dsDocDetail As New DataSet
        Dim btnAction As String
        Dim strLimit As String
        Dim dsCompDetail As New DataSet
        Dim strVenPayMethod As String

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

        If ViewState("oldvenidx") = "" And ViewState("DocNo") IsNot Nothing Then
            ViewState("oldvenidx") = objDB.GetVal("SELECT bm_s_coy_id FROM billing_mstr WHERE bm_invoice_index = " & ViewState("DocNo"))
        ElseIf ViewState("oldvenidx") <> "" And ViewState("DocNo") IsNot Nothing Then 'if, by any chances, the user just typed-in the vend's name
            ViewState("oldvenidx") = objDB.GetVal("SELECT bm_s_coy_id FROM billing_mstr WHERE bm_invoice_index = " & ViewState("DocNo"))
        End If

        'Check the selected company
        Dim compName = objDB.GetVal("select count(*) from ipp_company where ic_coy_name = '" & Common.Parse(removeGSTCode(Me.txtVendor.Text.Trim)) & "' and ic_index =" & hidVendorIndex.Value)
        If compName = 0 Then
            hidVendorIndex.Value = objDB.GetVal("select ic_index from ipp_company where ic_coy_name = '" & Common.Parse(removeGSTCode(Me.txtVendor.Text.Trim)) & "'")
            ViewState("venidx") = hidVendorIndex.Value
        End If
        'End

        dsCompDetail = objIPPMain.GetVendorAddress(Common.Parse(removeGSTCode(txtVendor.Text)), ViewState("IPPOfficer"), hidVendorIndex.Value)

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
                dtDoc.Columns.Add("InternalRemark", Type.GetType("System.String"))
                dtDoc.Columns.Add("ExchangeRate", Type.GetType("System.String"))
                dtDoc.Columns.Add("LateReason", Type.GetType("System.String"))
                dtDoc.Columns.Add("OldVenCompIDX", Type.GetType("System.String"))
                dtDoc.Columns.Add("BankCode", Type.GetType("System.String"))
                dtDoc.Columns.Add("BankAccount", Type.GetType("System.String"))
                dtDoc.Columns.Add("DocDueDate", Type.GetType("System.String"))
                dtDoc.Columns.Add("BeneficiaryDetails", Type.GetType("System.String"))
                dtDoc.Columns.Add("MasterDocument", Type.GetType("System.String"))
                dtDoc.Columns.Add("CompanyCategory", Type.GetType("System.String"))
                dtDoc.Columns.Add("ResidentType", Type.GetType("System.String"))
                dtDoc.Columns.Add("EmpId", Type.GetType("System.String"))
                dtDoc.Columns.Add("AmtWithoutGST", Type.GetType("System.Decimal"))

                Dim dtr As DataRow
                dtr = dtDoc.NewRow()

                'Zulham 19/06/2017
                'IPP Stage 3
                Select Case (ddlDocType.SelectedItem.Value)
                    Case "Invoice"
                        dtr("DocType") = "INV"
                    Case "Non-Invoice"
                        dtr("DocType") = "NON"
                    Case "Debit Note"
                        dtr("DocType") = "DN"
                    Case "Credit Note"
                        dtr("DocType") = "CN"
                    Case "Debit Advice"
                        dtr("DocType") = "DA"
                    Case "Credit Advice"
                        dtr("DocType") = "CA"
                    Case "Debit Note(Non-Invoice)"
                        dtr("DocType") = "DNN"
                    Case "Credit Note(Non-Invoice)"
                        dtr("DocType") = "CNN"
                End Select

                'Zulham 25062015 - Changed the way the billing invoice no is gotten
                If txtDocNo.Text.Trim <> "" Then dtr("DocNo") = txtDocNo.Text.Trim

                dtr("ManualPONo") = "" 'txtPONo.Text
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

                dtr("InternalRemark") = txtIntRemark.Text
                dtr("ExchangeRate") = "NULL"

                dtr("DocDueDate") = "NULL"

                dtr("BeneficiaryDetails") = ""
                dtr("MasterDocument") = ""
                Dim CompanyCategory = objDB.GetVal("SELECT IF(ic_company_category IS NULL,'',ic_company_category) AS 'ic_company_category' FROM ipp_company WHERE ic_index = '" & CompIdx & "'")
                dtr("CompanyCategory") = Common.parseNull(CompanyCategory)
                Dim ResidentType = objDB.GetVal("SELECT ic_resident_type FROM ipp_company WHERE ic_index = '" & CompIdx & "'")
                dtr("ResidentType") = Common.parseNull(ResidentType)
                If Me.txtNoTaxTotal.Text.Trim.Length = 0 Then Me.txtNoTaxTotal.Text = 0
                dtr("AmtWithoutGST") = Me.txtNoTaxTotal.Text
                dtDoc.Rows.Add(dtr)
                dsDoc.Tables.Add(dtDoc)

                If validateField(strFrm) And validateInput() Then

                    If strFrm = "Save" Then
                        'Zulham 25062015 - Added txtdocno.text for returned billing invoice no
                        objDoc.SaveBillingDocHeader(dsDoc, True, hid4.Value, , txtDocNo.Text)
                        strMsg = objGlobal.GetErrorMessage("00003")
                        If Not trigger = "cmdAddLine" Then Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    ElseIf strFrm = "Update" Then
                        objDoc.SaveBillingDocHeader(dsDoc, True, hid4.Value, True)
                    Else
                        If strFrm <> "Submit" Then
                            'Zulham 25062015 - Added txtdocno.text for returned billing invoice no
                            objDoc.SaveBillingDocHeader(dsDoc, , , , txtDocNo.Text)
                        End If
                    End If

                    If strFrm = "Submit" Then
                        Dim amt As String
                        objDoc.SaveBillingDocHeader(dsDoc, True, hid4.Value)
                        hid4.Value = txtDocNo.Text
                        amt = ViewState("itemTotalAmt")
                        amt = (Format(CDbl(amt), "#.00"))
                        Dim IsResident = ""
                        IsResident = objDB.GetVal("SELECT ic_resident_type FROM IPP_company WHERE ic_index =  '" & Common.Parse(ViewState("venidx")) & "'")
                        'If IsResident = "Y" Then
                        If amt <> (Format(CDbl(txtPaymentAmt.Text), "#.00")) Then
                            vldsum.InnerHtml = "<li>" & objGlobal.GetErrorMessage("00035") & "</li>"
                            Exit Sub
                        End If
                        'End If
                        Dim dsapplist As New DataSet
                        dsapplist = objDoc.getBillingApprovalWorkflowList()

                        If dsapplist.Tables(0).Rows.Count = 0 Then

                            strMsg = objGlobal.GetErrorMessage("00034")
                            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)

                        ElseIf dsapplist.Tables(0).Rows.Count = 1 Then

                            If objDoc.SaveBillingDoc(dsDoc, strFrm, Session("Action"), Session("DocNo"), hid4.Value, dsapplist.Tables(0).Rows(0).Item("AGB_GRP_INDEX").ToString) Then
                                strMsg = objGlobal.GetErrorMessage("00024")
                                strMsg = "Document " & strMsg
                                If Session("urlrefereripp") Is Nothing Or Session("urlrefereripp") = "" Then
                                    Common.NetMsgbox(Me, strMsg, dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId), MsgBoxStyle.Information)
                                Else
                                    Common.NetMsgbox(Me, strMsg, dDispatcher.direct("Billing", "BillingList.aspx", "pageid=" & strPageId), MsgBoxStyle.Information)
                                End If
                                Session("urlrefereripp") = Nothing
                            Else
                                strMsg = objGlobal.GetErrorMessage("00002")
                                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                                Exit Sub
                            End If
                        Else
                            Session("dsDOC") = dsDoc
                            Response.Redirect(dDispatcher.direct("Billing", "BillingApprovalSetup.aspx", "pageid=" & strPageId & "&docno=" & txtDocNo.Text & "&frm=" & strFrm & "&olddocno=" & hid4.Value))
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


        If (Session("DocNo") Is Nothing Or Session("DocNo") = 0) And Session("Action") = "New" Then
            If ViewState("oldvenidx") <> ViewState("venidx") And ViewState("oldvenidx") <> "" Then
                DocIndex = objDB.GetVal("SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & hid4.Value & "' and im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' and im_s_coy_id = '" & ViewState("oldvenidx") & "'")
            Else
                DocIndex = objDB.GetVal("SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & hid4.Value & "' and im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' and im_s_coy_id = '" & ViewState("venidx") & "'")
            End If
        Else
            DocIndex = Session("DocNo")
        End If
        compidx = ViewState("venidx")
        If Not objIPPMain.IsAlphaNum(txtDocNo.Text) Then
            If a = "" Then
                strMsg = objGlobal.GetErrorMessage("00032")
                strMsg = "Document No. " & strMsg
                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                Return False
            Else
                Return False
            End If

        End If

        If objDB.Exist("SELECT '*' FROM invoice_mstr WHERE im_invoice_index <> '" & DocIndex & "' and im_invoice_no = '" & txtDocNo.Text & "' AND im_s_coy_id = '" & compidx & "' AND im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'") > 0 Then
            If a = "" Then
                strMsg = objGlobal.GetErrorMessage("00002")
                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                Return False
            Else
                Return False
            End If
        End If

        'Zulham 02072015 - HLB-IPP Stage 4(CR)
        'On billing doc, first select customer and select vendor. Then change radio button selection to employee and click on add line
        'Pop entry screen will display error page
        Dim validCoy As String = ""
        Dim coyType As String = ""
        If Me.rbtnCoyType.SelectedValue = "C" Then
            validCoy = objDB.GetVal("SELECT count(*) FROM IPP_company WHERE ic_index =  '" & Common.Parse(ViewState("venidx")) & "' and ic_coy_type ='V' ")
            coyType = "Vendor"
        ElseIf Me.rbtnCoyType.SelectedValue = "R" Then
            validCoy = objDB.GetVal("SELECT count(*) FROM IPP_company WHERE ic_index =  '" & Common.Parse(ViewState("venidx")) & "' and ic_coy_type ='B' ")
            coytype = "Related Company"
        ElseIf Me.rbtnCoyType.SelectedValue = "E" Then
            validCoy = objDB.GetVal("SELECT count(*) FROM IPP_company WHERE ic_index =  '" & Common.Parse(ViewState("venidx")) & "' and ic_coy_type ='E' ")
            coytype = "Employee"
        End If
        If validCoy = "0" Then
            vldsum.InnerHtml = "<li>" & "Invalid " & coytype & "." & "</li>"
            Return False
        End If

        Return True

    End Function
    Private Function validateField(Optional ByVal strform As String = "") As Boolean
        Dim count, i As Integer
        Dim ds As New DataSet
        Dim strSql = ""
        'Modified for IPP GST Stage 2A - CH - 13 Feb 2015
        Dim strCoyId As String
        If strDefIPPCompID = "" Then
            strCoyId = HttpContext.Current.Session("CompanyID")
        Else
            strCoyId = strDefIPPCompID
        End If

        'If txtDocNo.Text = "" Then
        '    'vldsum.InnerHtml = "<li>Document No. " & objGlobal.GetErrorMessage("00001") & "</li>"
        '    'Return False
        'Else
        If txtVendor.Text = "" Then
            vldsum.InnerHtml = "<li>Vendor " & objGlobal.GetErrorMessage("00001") & "</li>"
            Return False
            'Modified for IPP GST Stage 2A - CH - 13 Feb 2015
        ElseIf objDB.Exist("SELECT IC_INDEX FROM IPP_COMPANY WHERE IC_INDEX = '" & ViewState("venidx") & "' AND (IC_COY_TYPE = 'V' OR IC_COY_TYPE = 'B' OR IC_COY_TYPE = 'E') AND IC_STATUS = 'A' AND IC_COY_ID = '" & Common.Parse(strCoyId) & "'") = 0 Then
            vldsum.InnerHtml = "<li>" & objGlobal.GetErrorMessage("00340") & "</li>"
            Return False
        ElseIf txtPaymentAmt.Text = "" Then
            vldsum.InnerHtml = "<li>Payment Amount " & objGlobal.GetErrorMessage("00001") & "</li>"
            Return False
        End If
        Dim strIsResident = ""
        Dim strIsNostro = ""
        strIsResident = objDB.GetVal("SELECT ic_resident_type FROM IPP_company WHERE ic_index =  '" & Common.Parse(ViewState("venidx")) & "'")
        strIsNostro = objDB.GetVal("SELECT ifnull(ic_payment_method,'') 'ic_payment_method' FROM IPP_company WHERE ic_index =  '" & Common.Parse(ViewState("venidx")) & "'")
        'Zulham 15/02/2015 8317
        If strIsNostro = "NOSTRO" Then
            If Not ddlCurrency.SelectedItem.Text.Contains("Malaysia") And strIsNostro = "NOSTRO" And strIsResident = "Y" Then
                vldsum.InnerHtml = "<li>" & "Invalid currency selection." & "</li>"
                Return False
            ElseIf ddlCurrency.SelectedItem.Text.Contains("Malaysia") And strIsNostro = "NOSTRO" And strIsResident = "N" Then
                vldsum.InnerHtml = "<li>" & "Invalid currency selection." & "</li>"
                Return False
            ElseIf ddlCurrency.SelectedItem.Text.Contains("Malaysia") And strIsResident = "N" Then
                vldsum.InnerHtml = "<li>" & "Invalid currency selection." & "</li>"
                Return False
            End If
            'ElseIf strIsNostro = "N" Then
            '    If ddlDocType.SelectedItem.Text.ToUpper = "NON-INVOICE" Then
            '        vldsum.InnerHtml = "<li>" & "Invalid document type selection." & "</li>"
            '        Return False
            '    End If
        ElseIf ddlCurrency.SelectedItem.Text.Contains("Malaysia") And strIsResident = "N" Then
            vldsum.InnerHtml = "<li>" & "Invalid currency selection." & "</li>"
            ddlCurrency.Enabled = True
            Return False
        End If

        If dtgInvDetail.Items.Count < 1 And strform = "Submit" Then
            vldsum.InnerHtml = "<li>" & objGlobal.GetErrorMessage("00131") & "</li>"
            Return False
        End If
        If Me.txtNoTaxTotal.Text.Trim.Length = 0 Then
            vldsum.InnerHtml = "<li>" & "Please insert amount wihout tax." & "</li>"
            Return False
        End If

        'Zulham 02072015 - HLB-IPP Stage 4(CR)
        'On billing doc, first select customer and select vendor. Then change radio button selection to employee and click on add line
        'Pop entry screen tu akan display error page
        Dim validCoy As String = ""
        Dim coyType As String = ""
        If Me.rbtnCoyType.SelectedValue = "C" Then
            validCoy = objDB.GetVal("SELECT count(*) FROM IPP_company WHERE ic_index =  '" & Common.Parse(ViewState("venidx")) & "' and ic_coy_type ='V' ")
            coyType = "Vendor"
        ElseIf Me.rbtnCoyType.SelectedValue = "R" Then
            validCoy = objDB.GetVal("SELECT count(*) FROM IPP_company WHERE ic_index =  '" & Common.Parse(ViewState("venidx")) & "' and ic_coy_type ='B' ")
            coyType = "Related Company"
        ElseIf Me.rbtnCoyType.SelectedValue = "E" Then
            validCoy = objDB.GetVal("SELECT count(*) FROM IPP_company WHERE ic_index =  '" & Common.Parse(ViewState("venidx")) & "' and ic_coy_type ='E' ")
            coyType = "Employee"
        End If
        If validCoy = "0" Then
            vldsum.InnerHtml = "<li>" & "Invalid " & coyType & "." & "</li>"
            Return False
        End If

        'Zulham 22./06/2017
        'IPP Stage 3
        If strform.ToUpper = "SUBMIT" And (Me.ddlDocType.SelectedItem.Text = "Credit Note" Or Me.ddlDocType.SelectedItem.Text = "Debit Note" _
        Or Me.ddlDocType.SelectedItem.Text = "Debit Note(Non-Invoice)" Or Me.ddlDocType.SelectedItem.Text = "Credit Note(Non-Invoice)") Then
            Dim dsDetails As New DataSet
            strSql = "SELECT DISTINCT '*' FROM billing_mstr a, billing_details b " & _
                     "WHERE(a.bm_invoice_no = b.bm_ref_no) " & _
                     "AND b.bm_invoice_no = '" & Me.txtDocNo.Text & "' " & _
                     "AND b.bm_s_coy_id = '" & Common.Parse(ViewState("venidx")) & "' " & _
                     "AND a.bm_b_coy_id = '" & Common.Parse(strCoyId) & "' " & _
                     "AND a.bm_invoice_status in ('4','1')"
            dsDetails = objDB.FillDs(strSql)
            If Not dsDetails Is Nothing Then
                If Not dsDetails.Tables(0).Rows.Count = 0 Then
                    Common.NetMsgbox(Me, "Credit Note/Debit Note cannot precede the invoices it's tied to.", MsgBoxStyle.Exclamation)
                    Return False
                End If
            End If
        End If

        Return True
    End Function
    Private Function validateField2() As Boolean
        Dim count, i As Integer
        Dim ds As New DataSet
        Dim strSql = ""
        'Modified for IPP GST Stage 2A - CH - 13 Feb 2015
        Dim strCoyId As String
        If strDefIPPCompID = "" Then
            strCoyId = HttpContext.Current.Session("CompanyID")
        Else
            strCoyId = strDefIPPCompID
        End If

        Dim strIsResident = ""
        Dim strIsNostro = ""
        strIsResident = objDB.GetVal("SELECT ic_resident_type FROM IPP_company WHERE ic_index =  '" & Common.Parse(ViewState("venidx")) & "'")
        strIsNostro = objDB.GetVal("SELECT ifnull(ic_nostro_flag,'') 'ic_nostro_flag' FROM IPP_company WHERE ic_index =  '" & Common.Parse(ViewState("venidx")) & "'")

        'If txtDocNo.Text = "" Then
        '    'vldsum.InnerHtml = "<li>Document No. " & objGlobal.GetErrorMessage("00001") & "</li>"
        '    'Return False
        'Else
        If txtDocDate.Value = "" Then
            vldsum.InnerHtml = "<li>Document Date " & objGlobal.GetErrorMessage("00001") & "</li>"
            Return False
        ElseIf txtDocDate.Value > System.DateTime.Today Then
            vldsum.InnerHtml = "<li>Document Date " & objGlobal.GetErrorMessage("00325") & "</li>"
            Return False
        ElseIf txtVendor.Text = "" Then
            vldsum.InnerHtml = "<li>Vendor " & objGlobal.GetErrorMessage("00001") & "</li>"
            Return False
            'Modified for IPP GST Stage 2A - CH - 13 Feb 2015
        ElseIf objDB.Exist("SELECT IC_INDEX FROM IPP_COMPANY WHERE IC_INDEX = '" & ViewState("venidx") & "' AND (IC_COY_TYPE = 'V' OR IC_COY_TYPE = 'B' OR IC_COY_TYPE = 'E') AND IC_STATUS = 'A' AND IC_COY_ID = '" & Common.Parse(strCoyId) & "'") = 0 Then
            vldsum.InnerHtml = "<li>" & objGlobal.GetErrorMessage("00340") & "</li>"
            Return False
        ElseIf txtPaymentAmt.Text = "" Then
            vldsum.InnerHtml = "<li>Payment Amount " & objGlobal.GetErrorMessage("00001") & "</li>"
            Return False
        End If


        strIsResident = objDB.GetVal("SELECT ic_resident_type FROM IPP_company WHERE ic_index =  '" & Common.Parse(ViewState("venidx")) & "'")

        'If (lblPaymentMethod.Text = "TT" Or lblPaymentMethod.Text = "RENTAS") And txtBeneficiaryDetails.Text = "" Then
        '    vldsum.InnerHtml = "<li>Beneficiary Details " & objGlobal.GetErrorMessage("00001") & "</li>"
        '    Return False
        'End If

        If lblPaymentMethod.Text = "TT" And ddlCurrency.SelectedValue = "MYR" Then
            vldsum.InnerHtml = "<li> " & objGlobal.GetErrorMessage("00040") & "</li>"
            Return False
        End If

        'If rdbNoWHT.Checked <> True And strIsResident.ToString.ToUpper = "N" And txtWHT.Text = "" Then
        '    vldsum.InnerHtml = "<li>Withholding Tax " & objGlobal.GetErrorMessage("00001") & "</li>"
        '    Return False
        'End If

        'If strIsResident.ToString.ToUpper = "N" And txtWHT.Text = "" And (rdbWHTComp.Checked = True Or rdbWHTVendor.Checked = True) Then
        '    vldsum.InnerHtml = "<li>Withholding Tax " & objGlobal.GetErrorMessage("00001") & "</li>"
        '    Return False
        'End If

        If strIsResident.ToString.ToUpper = "N" And txtWHT.Text = "" And rdbNoWHT.Checked = True And txtNoWHTReason.Text = "" Then
            vldsum.InnerHtml = "<li> " & objGlobal.GetErrorMessage("00042") & "</li>"
            Return False
        End If

        If strIsResident.ToString.ToUpper = "N" And txtWHT.Text <> "" And (rdbWHTComp.Checked = False And rdbWHTVendor.Checked = False) Then
            vldsum.InnerHtml = "<li>" & objGlobal.GetErrorMessage("00041") & "</li>"
            Return False
        End If


        'If lblPaymentMethod.Text = "TT" And ddlCurrency.SelectedValue <> "MYR" And txtWHT.Text = "" Then
        '    If rdbNoWHT.Checked <> True And strIsResident.ToString.ToUpper = "N" Then
        '        vldsum.InnerHtml = "<li>Withholding Tax " & objGlobal.GetErrorMessage("00001") & "</li>"
        '        Return False
        '    End If
        'End If

        If Not objIPPMain.IsAlphaNum(txtDocNo.Text) Then

            strMsg = objGlobal.GetErrorMessage("00032")
            strMsg = "Document No. " & strMsg
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
            Return False

        End If

        ''Same vendor - two diff invoices - sub doc no in one of the invoices <> the other invoice no
        'strSql = "SELECT im_invoice_index, IM_INVOICE_NO FROM invoice_mstr WHERE im_invoice_index IN (SELECT DISTINCT(isd_mstr_doc_index) FROM ipp_SUB_DOC WHERE " & _
        '         "ISD_DOC_NO LIKE '" & Common.Parse(Me.txtDocNo.Text) & "') AND im_s_coy_id = '" & Common.Parse(Me.hid6.Value) & "' AND im_b_coy_id = '" & Common.Parse(Common.Parse(HttpContext.Current.Session("CompanyID"))) & "' AND IM_PO_INDEX IS NULL"
        'ds = objDB.FillDs(strSql)
        'If ds.Tables(0).Rows.Count > 0 Then
        '    Common.NetMsgbox(Me, "Master Document No. must not equal to Sub Document No.")
        '    Session("SubDocument") = Nothing
        '    Exit Function
        'End If

        ''Same vendor - two diff invoices - sub doc no in one of the invoices <> sub doc no in the other invoice no
        'strSql = "SELECT IM_INVOICE_NO FROM invoice_mstr WHERE im_invoice_index IN (SELECT DISTINCT(isd_mstr_doc_index) FROM ipp_SUB_DOC WHERE ISD_DOC_NO LIKE '" & Common.parseNull(txtDocNo.Text).ToString & "') AND " & _
        '         "im_s_coy_id = '" & Common.Parse(Me.hid6.Value) & "' AND im_b_coy_id = '" & Common.Parse(Common.Parse(HttpContext.Current.Session("CompanyID"))) & "' AND IM_PO_INDEX IS NULL"
        'ds = objDB.FillDs(strSql)
        'If ds.Tables(0).Rows.Count > 0 Then
        '    Common.NetMsgbox(Me, "Sub Document No must not already exist in the system.")
        '    Session("SubDocument") = Nothing
        '    Exit Function
        'End If

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
        SaveBillingDoc("Submit")
        ViewState("GSTTotalInput") = 0.0

        'Zulham 21/06/2017
        'IPP Stage 3
        ViewState("lineno") = 0

        bindgrid()
    End Sub
    Private Sub cmdVoid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdVoid.Click
        Dim strDocNo As String
        Dim objBilling As New Billing
        strDocNo = txtDocNo.Text

        'Zulham 22/06/2017
        'IPP Stage 3
        Dim strSql As String
        Dim dsResult As New DataSet
        strSql = "SELECT '*' FROM billing_mstr a, billing_details b " & _
                "WHERE(a.bm_invoice_no = b.bm_invoice_no) " & _
                "AND  a.bm_s_coy_id = b.bm_s_coy_id " & _
                "AND bm_ref_no = '" & Me.txtDocNo.Text & "' " & _
                "AND bm_invoice_status <> '5' " & _
                "and bm_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " & _
                "LIMIT 1"
        dsResult = objDB.FillDs(strSql)
        If Not dsResult Is Nothing Then
            If Not dsResult.Tables(0).Rows.Count = 0 Then
                Common.NetMsgbox(Me, "This billing is tied to a Credit Note/Debit Note.", MsgBoxStyle.Exclamation)
                Exit Sub
            End If
        End If

        If objBilling.VoidBillingDoc(Session("DocNo")) Then
            strMsg = objGlobal.GetErrorMessage("00033")
            Common.NetMsgbox(Me, strMsg, dDispatcher.direct("Billing", "BillingList.aspx", "pageid=" & strPageId), MsgBoxStyle.Information)
        End If

    End Sub
    Private Sub DisplayIPPDocInfo()
        Dim dsIPPDoc As DataSet
        Dim DtDiff As Long

        dsIPPDoc = objDoc.GetBillingDoc(Session("DocNo"))
        Session("dsIPPDOC") = dsIPPDoc
        ViewState("docno") = dsIPPDoc.Tables(0).Rows(0)("BM_INVOICE_NO")
        ViewState("venidx") = dsIPPDoc.Tables(0).Rows(0)("bM_S_COY_ID")

        'Zulham 19/06/2017
        'IPP Stage 3
        If dsIPPDoc.Tables(0).Rows(0)("bM_INVOICE_TYPE") = "INV" Then
            Me.ddlDocType.SelectedValue = "Invoice"
        ElseIf dsIPPDoc.Tables(0).Rows(0)("bM_INVOICE_TYPE") = "NON" Then
            Me.ddlDocType.SelectedValue = "Non-Invoice"
        ElseIf dsIPPDoc.Tables(0).Rows(0)("bM_INVOICE_TYPE") = "DN" Then
            Me.ddlDocType.SelectedValue = "Debit Note"
        ElseIf dsIPPDoc.Tables(0).Rows(0)("bM_INVOICE_TYPE") = "CN" Then
            Me.ddlDocType.SelectedValue = "Credit Note"
        ElseIf dsIPPDoc.Tables(0).Rows(0)("bM_INVOICE_TYPE") = "DA" Then
            Me.ddlDocType.SelectedValue = "Debit Advice"
        ElseIf dsIPPDoc.Tables(0).Rows(0)("bM_INVOICE_TYPE") = "CA" Then
            Me.ddlDocType.SelectedValue = "Credit Advice"
        ElseIf dsIPPDoc.Tables(0).Rows(0)("bM_INVOICE_TYPE") = "DNN" Then
            Me.ddlDocType.SelectedValue = "Debit Note(Non-Invoice)"
        ElseIf dsIPPDoc.Tables(0).Rows(0)("bM_INVOICE_TYPE") = "CNN" Then
            Me.ddlDocType.SelectedValue = "Credit Note(Non-Invoice)"
        End If

        Me.txtDocNo.Text = dsIPPDoc.Tables(0).Rows(0)("bM_INVOICE_NO")
        Me.ddlCurrency.SelectedValue = dsIPPDoc.Tables(0).Rows(0)("bM_CURRENCY_CODE")
        If Me.ddlCurrency.SelectedValue = "MYR" Then Me.ddlCurrency.Enabled = False

        Me.txtPaymentAmt.Text = Format(dsIPPDoc.Tables(0).Rows(0)("bM_INVOICE_TOTAL"), "#,###,0.00")
        If Not dsIPPDoc.Tables(0).Rows(0)("bM_INVOICE_WTH_TOTAL") Is DBNull.Value Then
            Me.txtNoTaxTotal.Text = Format(dsIPPDoc.Tables(0).Rows(0)("bM_INVOICE_WTH_TOTAL"), "#,###,0.00")
        Else
            Me.txtNoTaxTotal.Text = "0.00"
        End If
        hid6.Value = dsIPPDoc.Tables(0).Rows(0)("bM_S_COY_ID")

        If dsIPPDoc.Tables(0).Rows(0)("bM_REMARK") Is System.DBNull.Value Then
            Me.txtIntRemark.Text = ""
        Else
            Me.txtIntRemark.Text = dsIPPDoc.Tables(0).Rows(0)("bM_REMARK")
        End If

        If dsIPPDoc.Tables(0).Rows(0)("bM_INVOICE_STATUS") <> "4" Then '14 = Rejected
            'Get the GST Reg Code if there's any
            Dim strGSTCode = ""
            Dim GST As New GST
            Dim createdDate = Common.FormatWheelDate(WheelDateFormat.LongDate, Date.Today)
            Dim _cutoffDate = System.Configuration.ConfigurationManager.AppSettings.Get("GstCutOffDate")
            If CDate(createdDate) > CDate(_cutoffDate) Then strGSTCode = GST.chkGST_ForIPP(ViewState("venidx"))

            If Not Common.parseNull(strGSTCode) = "" Then
                Me.txtVendor.Text = dsIPPDoc.Tables(0).Rows(0)("bM_S_COY_NAME") & "(" & strGSTCode & ")"
            Else
                Me.txtVendor.Text = dsIPPDoc.Tables(0).Rows(0)("bM_S_COY_NAME")
            End If

            Me.txtVenAddL1.Text = dsIPPDoc.Tables(0).Rows(0)("bM_ADDR_LINE1")
            Me.txtVenAddL2.Text = dsIPPDoc.Tables(0).Rows(0)("bM_ADDR_LINE2")
            Me.txtVenAddL3.Text = dsIPPDoc.Tables(0).Rows(0)("bM_ADDR_LINE3")
            Me.txtVenAddPostcode.Text = dsIPPDoc.Tables(0).Rows(0)("bM_POSTCODE")
            Me.txtVenAddCity.Text = dsIPPDoc.Tables(0).Rows(0)("bM_CITY")
            Me.txtVenAddState.Text = dsIPPDoc.Tables(0).Rows(0)("bM_STATE")
            Me.txtVenAddCountry.Text = dsIPPDoc.Tables(0).Rows(0)("bM_COUNTRY")
        Else
            'Get the GST Reg Code if there's any
            Dim strGSTCode = ""
            Dim GST As New GST
            Dim createdDate = Common.FormatWheelDate(WheelDateFormat.LongDate, Date.Today)
            Dim _cutoffDate = System.Configuration.ConfigurationManager.AppSettings.Get("GstCutOffDate")
            If CDate(createdDate) > CDate(_cutoffDate) Then strGSTCode = GST.chkGST_ForIPP(ViewState("venidx"))

            If Not Common.parseNull(strGSTCode) = "" Then
                Me.txtVendor.Text = dsIPPDoc.Tables(0).Rows(0)("bM_S_COY_NAME") & "(" & strGSTCode & ")"
            Else
                Me.txtVendor.Text = dsIPPDoc.Tables(0).Rows(0)("bM_S_COY_NAME")
            End If
            'Me.txtVendor.Text = dsIPPDoc.Tables(0).Rows(0)("IC_COY_NAME")
            Me.txtVenAddL1.Text = dsIPPDoc.Tables(0).Rows(0)("IC_ADDR_LINE1")
            Me.txtVenAddL2.Text = dsIPPDoc.Tables(0).Rows(0)("IC_ADDR_LINE2")
            Me.txtVenAddL3.Text = dsIPPDoc.Tables(0).Rows(0)("IC_ADDR_LINE3")
            Me.txtVenAddPostcode.Text = dsIPPDoc.Tables(0).Rows(0)("IC_POSTCODE")
            Me.txtVenAddCity.Text = dsIPPDoc.Tables(0).Rows(0)("IC_CITY")
            Me.txtVenAddState.Text = dsIPPDoc.Tables(0).Rows(0)("IC_STATE")
            Me.txtVenAddCountry.Text = dsIPPDoc.Tables(0).Rows(0)("IC_COUNTRY")
        End If
        Me.hidVendorIndex.Value = dsIPPDoc.Tables(0).Rows(0)("bM_S_COY_ID")
        ViewState("oldvenidx") = Me.hidVendorIndex.Value
        ViewState("venidx") = Me.hidVendorIndex.Value
    End Sub
    Private Sub renderIPPApprFlow()
        Dim objDoc As New Billing

        'Dim ds As DataSet = objDoc.getIPPApprFlow(Session("DocNo"), Common.Parse(HttpContext.Current.Session("CompanyID")))
        Dim ds As DataSet = objDoc.getbillingApprFlow(Session("DocNo"), Common.Parse(HttpContext.Current.Session("CompanyID")))

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

        If Session("Action") = "New" Then
            If Not validateField("") Then Exit Sub
            compidx = ViewState("venidx")
            If objIPPMain.CheckDuplicatedEntry(compidx, txtDocNo.Text) Then
                strMsg = objGlobal.GetErrorMessage("00002")
                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                cmdSave.Style("display") = "none"
            ElseIf objIPPMain.CheckDuplicatedEntry2(txtPaymentAmt.Text, Common.ConvertDate(Date.Now), compidx, txtDocNo.Text) Then 'Prompt user there is invoice with same amount, vendor, doc date whether want to continue                 
                strscript.Append("<script language=""javascript"">")
                strscript.Append("document.getElementById('hidBtnContinue').click();")
                strscript.Append("</script>")
                RegisterStartupScript("script3", strscript.ToString())
            Else
                Dim strFileName As String
                SaveBillingDoc("")
                hid4.Value = txtDocNo.Text
                cmdSubmit.Style("display") = "on"
                btnContinue.Style("display") = "none"
                strscript.Append("<script language=""javascript"">")
                If rbtnCoyType.SelectedValue = "C" Or rbtnCoyType.SelectedValue = "V" Then
                    strFileName = dDispatcher.direct("Billing", "BillingEntryPop.aspx", "pageid=" & strPageId & "&coytype=V&docno=" & txtDocNo.Text & "&doctype=" & Server.UrlEncode(ddlDocType.SelectedItem.Text) & "&vencomp=" & Server.UrlEncode(removeGSTCode(txtVendor.Text)) & "&olddocno=" & hid4.Value & "&venIdx=" & Server.UrlEncode(ViewState("venidx")) & "&isResident=" & isResident) 'Feb 28, 2014
                ElseIf rbtnCoyType.SelectedValue = "R" Then
                    strFileName = dDispatcher.direct("Billing", "BillingEntryPop.aspx", "pageid=" & strPageId & "&coytype=B&docno=" & txtDocNo.Text & "&doctype=" & Server.UrlEncode(ddlDocType.SelectedItem.Text) & "&vencomp=" & Server.UrlEncode(removeGSTCode(txtVendor.Text)) & "&olddocno=" & hid4.Value & "&venIdx=" & Server.UrlEncode(ViewState("venidx")) & "&isResident=" & isResident) 'Feb 28, 2014
                Else
                    strFileName = dDispatcher.direct("Billing", "BillingEntryPop.aspx", "pageid=" & strPageId & "&coytype=" & rbtnCoyType.SelectedValue & "&docno=" & txtDocNo.Text & "&doctype=" & Server.UrlEncode(ddlDocType.SelectedItem.Text) & "&vencomp=" & Server.UrlEncode(removeGSTCode(txtVendor.Text)) & "&olddocno=" & hid4.Value & "&venIdx=" & Server.UrlEncode(ViewState("venidx")) & "&isResident=" & isResident) 'Feb 28, 2014
                End If
                strFileName = Server.UrlEncode(strFileName)
                strscript.Append("ShowDialog(""" & dDispatcher.direct("IPP", "Dialog.aspx", "page=" & strFileName) & """,'520px');")
                strscript.Append("document.getElementById('btnhidden3').click();")
                strscript.Append("</script>")
                RegisterStartupScript("script4", strscript.ToString())
                cmdSave.Style("display") = ""
            End If
        End If

        'End If
    End Sub

    Sub bindgrid()
        Try
            Dim objDoc As New Billing
            Dim DocIndex As String
            Dim ds As DataSet
            Dim objIPPDetail As New IPPDetails
            ViewState("docno") = txtDocNo.Text
            If Session("DocNo") Is Nothing Or Session("DocNo") = 0 Then
                If ViewState("oldvenidx") <> ViewState("venidx") And ViewState("oldvenidx") <> "" Then
                    DocIndex = objDB.GetVal("SELECT bm_invoice_index FROM billing_mstr WHERE bm_invoice_no = '" & hid4.Value & "' and bm_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' and bm_s_coy_id = '" & ViewState("oldvenidx") & "'")
                Else
                    DocIndex = objDB.GetVal("SELECT bm_invoice_index FROM billing_mstr WHERE bm_invoice_no = '" & hid4.Value & "' and bm_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' and bm_s_coy_id = '" & ViewState("venidx") & "'")
                End If
                If DocIndex.ToString = "" Then
                    DocIndex = objDB.GetVal("SELECT im_invoice_index FROM billing_mstr WHERE bm_invoice_no = '" & hid4.Value & "' and bm_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' and bm_s_coy_id = '" & ViewState("venidx") & "'")
                End If
            Else
                DocIndex = Session("DocNo")
            End If
            ViewState("DocNo") = DocIndex
            If ViewState("oldvenidx") <> ViewState("venidx") And ViewState("oldvenidx") <> "" Then
                ds = objDoc.GetBillingDocDetails(ViewState("docno"), ViewState("oldvenidx"), , hid4.Value)
            Else
                ds = objDoc.GetBillingDocDetails(ViewState("docno"), ViewState("venidx"), , hid4.Value)
            End If
            If ds.Tables(0).Rows.Count = 0 Then
                ds = objDoc.GetBillingDocDetails(ViewState("docno"), ViewState("venidx"), , hid4.Value)
            End If
            Dim dsDocHeaderInfo As DataSet = objDoc.GetBillingDoc(DocIndex)


            If dsDocHeaderInfo.Tables(0).Rows(0)("bM_INVOICE_STATUS") <> "4" Then '14 = Rejected
                'If dsDocHeaderInfo.Tables(0).Rows(0)("IM_PAYMENT_TERM") = "BC" Then
                If removeGSTCode(txtVendor.Text) <> hid5.Value And hid5.Value <> "" Then
                Else
                End If
            Else
                If removeGSTCode(txtVendor.Text) <> hid5.Value And hid5.Value <> "" Then
                Else
                End If

            End If

            intPageRecordCnt = ds.Tables(0).Rows.Count

            Dim dvViewSample As DataView
            dvViewSample = ds.Tables(0).DefaultView
            dvViewSample.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewSample.Sort += " DESC"

            ViewState("GSTTotalOutput") = 0.0
            '//bind datagrid
            If intPageRecordCnt > 0 Then
                dtgInvDetail.DataSource = ds.Tables(0).DefaultView
                dtgInvDetail.DataBind()
            Else
                'Zulham 13042015 GST Stage 2A
                dtgInvDetail.DataSource = Nothing
                dtgInvDetail.DataBind()
                If Not Page.IsPostBack Then
                    Common.NetMsgbox(Me, MsgNoRecord)
                End If
            End If
            objDoc = Nothing
            cmdAddLine.Style("display") = ""
            AddRowTotal()
        Catch ex As Exception

        End Try
    End Sub
    Sub dtgSubDoc_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgSubDoc.PageIndexChanged
        dtgSubDoc.CurrentPageIndex = e.NewPageIndex
        ViewState("GSTTotalInput") = 0.0
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
            SaveBillingDoc("Save", "cmdAddLine")
            rowcount = objDB.GetMax("billing_DETAILS", "BM_INVOICE_LINE", " WHERE BM_INVOICE_NO = '" & txtDocNo.Text & "' AND BM_S_COY_ID = '" & ViewState("venidx") & "'")
            strscript.Append("<script language=""javascript"">")
            If rbtnCoyType.SelectedValue = "C" Or rbtnCoyType.SelectedValue = "V" Then
                strFileName = dDispatcher.direct("Billing", "BillingEntryPop.aspx", "pageid=" & strPageId & "&coytype=V&docno=" & txtDocNo.Text & "&doctype=" & Server.UrlEncode(ddlDocType.SelectedItem.Text) & "&rowcount=" & rowcount & "&vencomp=" & Server.UrlEncode(removeGSTCode(hid5.Value)) & "&olddocno=" & hid4.Value & "&isResident=" & isResident)
            ElseIf rbtnCoyType.SelectedValue = "R" Then
                strFileName = dDispatcher.direct("Billing", "BillingEntryPop.aspx", "pageid=" & strPageId & "&coytype=B&docno=" & txtDocNo.Text & "&doctype=" & Server.UrlEncode(ddlDocType.SelectedItem.Text) & "&rowcount=" & rowcount & "&vencomp=" & Server.UrlEncode(removeGSTCode(hid5.Value)) & "&olddocno=" & hid4.Value & "&isResident=" & isResident)
            Else
                strFileName = dDispatcher.direct("Billing", "BillingEntryPop.aspx", "pageid=" & strPageId & "&coytype=" & rbtnCoyType.SelectedValue & "&docno=" & txtDocNo.Text & "&doctype=" & Server.UrlEncode(ddlDocType.SelectedItem.Text) & "&rowcount=" & rowcount & "&vencomp=" & Server.UrlEncode(removeGSTCode(hid5.Value)) & "&olddocno=" & hid4.Value & "&isResident=" & isResident)
            End If
            strFileName = Server.UrlEncode(strFileName)
            strscript.Append("ShowDialog(""" & dDispatcher.direct("IPP", "Dialog.aspx", "page=" & strFileName) & """,'520px');")
            strscript.Append("document.getElementById('btnhidden3').click();")
            strscript.Append("</script>")
            RegisterStartupScript("script5", strscript.ToString())
        End If
    End Sub

    Private Sub dtgInvDetail_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgInvDetail.ItemCreated
        Grid_ItemCreated(dtgInvDetail, e)
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(EnumDocDetail.ichidSNO).Style("display") = "none"

            'Zulham 21/06/2017
            'IPP Stage 3
            If ddlDocType.SelectedItem.Value <> "Credit Note" And ddlDocType.SelectedItem.Value <> "Debit Note" _
            And ddlDocType.SelectedItem.Value <> "Debit Note(Non-Invoice)" And ddlDocType.SelectedItem.Value <> "Credit Note(Non-Invoice)" Then
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
                cmdremove.Attributes.Add("onclick", "remove('" & dv("BM_INVOICE_LINE") & "')")

                cmdedit = e.Item.FindControl("cmdedit")
                cmdedit.InnerHtml = "<img src='" & dDispatcher.direct("Plugins/Images", "i_edit2.gif") & "' />"


                cmdedit.Attributes.Add("onclick", "edit('" & dv("BM_INVOICE_LINE") & "')")

                LineNo = e.Item.FindControl("LineNo")
                ViewState("lineno") = ViewState("lineno") + 1
                LineNo.Text = ViewState("lineno")
                e.Item.Cells(EnumDocDetail.ichidSNO).Style("display") = "none"
                e.Item.Cells(EnumDocDetail.icAmt).Text = (Format(CDbl(e.Item.Cells(EnumDocDetail.icAmt).Text), "#,###.00"))
                e.Item.Cells(EnumDocDetail.icUnitPrice).Text = (Format(CDbl(e.Item.Cells(EnumDocDetail.icUnitPrice).Text), "#,###.00"))

                'Zulham 20/06/2017
                'IPP Stage 3
                If ddlDocType.SelectedItem.Value <> "Credit Note" And ddlDocType.SelectedItem.Value <> "Debit Note" _
                And ddlDocType.SelectedItem.Value <> "Debit Note(Non-Invoice)" And ddlDocType.SelectedItem.Value <> "Credit Note(Non-Invoice)" _
                Then
                    e.Item.Cells(EnumDocDetail.icRefNo).Style("display") = "none"
                End If

                e.Item.Cells(EnumDocDetail.icGLCode).Text = dv("BM_B_GL_CODE") & ":" & dv("CBG_B_GL_DESC")


                Dim GST As New GST
                Dim result As New DataSet

                If Not Common.parseNull(dv("BM_GST_INPUT_TAX_CODE")) = "" Then
                    If Not dv("BM_GST_INPUT_TAX_CODE").ToString = "0" Then
                        result = GST.GetTaxCode_forIPP()
                        For record As Integer = 0 To result.Tables(0).Rows.Count - 1
                            If dv("BM_GST_INPUT_TAX_CODE").ToString.Trim = result.Tables(0).Rows(record).Item(1) Then
                                e.Item.Cells(EnumDocDetail.icInputTax).Text = result.Tables(0).Rows(record).Item(0).ToString.Trim
                            End If
                        Next
                        If e.Item.Cells(EnumDocDetail.icInputTax).Text.Contains("(") _
                         And Not e.Item.Cells(EnumDocDetail.icInputTax).Text.Contains("0") Then
                            ViewState("GSTTotalInput") += CDec(dv("BM_GST_VALUE"))
                        End If
                    Else
                        e.Item.Cells(EnumDocDetail.icInputTax).Text = "N/A"
                    End If
                Else
                    e.Item.Cells(EnumDocDetail.icInputTax).Text = "N/A"
                End If
                If Not Common.parseNull(dv("BM_GST_OUTPUT_TAX_CODE")) = "" Then
                    result = GST.GetTaxCode_forIPP()
                    For record As Integer = 0 To result.Tables(0).Rows.Count - 1
                        If dv("BM_GST_OUTPUT_TAX_CODE").ToString.Trim = result.Tables(0).Rows(record).Item(1) Then
                            e.Item.Cells(EnumDocDetail.icOutputTax).Text = result.Tables(0).Rows(record).Item(0).ToString.Trim
                        End If
                    Next
                    If e.Item.Cells(EnumDocDetail.icOutputTax).Text.Contains("(") _
                        And Not e.Item.Cells(EnumDocDetail.icOutputTax).Text.Contains("0") Then
                        ViewState("GSTTotalOutput") += CDec(dv("BM_GST_VALUE"))
                    End If
                Else
                    e.Item.Cells(EnumDocDetail.icOutputTax).Text = "N/A"
                End If

                If Not dv("BM_GST_VALUE").ToString.Trim.Length = "0" Then
                    If CDec(dv("BM_GST_VALUE")) = CDec(0) Then
                        e.Item.Cells(EnumDocDetail.icGSTAmount).Text = "0.00"
                    End If
                ElseIf dv("BM_GST_VALUE") = "" Then
                    e.Item.Cells(EnumDocDetail.icGSTAmount).Text = "0.00"
                End If

                'Jules 2018.05.02 - PAMB Scrum 2
                e.Item.Cells(EnumDocDetail.icAnalysisCode1).Text = dv("BM_ANALYSIS_CODE1") & ":" & dv("AC_ANALYSIS_CODE1")
                e.Item.Cells(EnumDocDetail.icAnalysisCode2).Text = dv("BM_ANALYSIS_CODE2") & ":" & dv("AC_ANALYSIS_CODE2")
                e.Item.Cells(EnumDocDetail.icAnalysisCode3).Text = dv("BM_ANALYSIS_CODE3") & ":" & dv("AC_ANALYSIS_CODE3")
                e.Item.Cells(EnumDocDetail.icAnalysisCode4).Text = dv("BM_ANALYSIS_CODE4") & ":" & dv("AC_ANALYSIS_CODE4")
                e.Item.Cells(EnumDocDetail.icAnalysisCode5).Text = dv("BM_ANALYSIS_CODE5") & ":" & dv("AC_ANALYSIS_CODE5")
                e.Item.Cells(EnumDocDetail.icAnalysisCode8).Text = dv("BM_ANALYSIS_CODE8") & ":" & dv("AC_ANALYSIS_CODE8")
                e.Item.Cells(EnumDocDetail.icAnalysisCode9).Text = dv("BM_ANALYSIS_CODE9") & ":" & dv("AC_ANALYSIS_CODE9")
                'End modification.
            Catch ex As Exception

            End Try
        End If

    End Sub
    Private Sub btnremoveline_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnremoveline.Click
        Dim slineno As String
        Dim objIPP As New IPPMain
        Dim objBilling As New Billing
        Dim venidx As String

        'Zulham 12/03/2015 IPP GST Stage 2A
        Dim strCoyId As String
        If strDefIPPCompID = "" Then
            strCoyId = HttpContext.Current.Session("CompanyID")
        Else
            strCoyId = strDefIPPCompID
        End If

        If rbtnCoyType.SelectedValue = "C" Or rbtnCoyType.SelectedValue = "V" Then
            If objDB.Exist("SELECT IC_INDEX FROM IPP_COMPANY WHERE IC_COY_NAME = '" & Common.Parse(removeGSTCode(txtVendor.Text)) & "' AND IC_COY_TYPE = 'V' AND IC_STATUS = 'A' AND IC_COY_ID = '" & Common.Parse(strCoyId) & "'") = 0 Then
                vldsum.InnerHtml = "<li>" & objGlobal.GetErrorMessage("00340") & "</li>"
                Exit Sub
            End If
        ElseIf rbtnCoyType.SelectedValue = "R" Then
            If objDB.Exist("SELECT IC_INDEX FROM IPP_COMPANY WHERE IC_COY_NAME = '" & Common.Parse(removeGSTCode(txtVendor.Text)) & "' AND IC_COY_TYPE = 'B' AND IC_STATUS = 'A' AND IC_COY_ID = '" & Common.Parse(strCoyId) & "'") = 0 Then
                vldsum.InnerHtml = "<li>" & objGlobal.GetErrorMessage("00340") & "</li>"
                Exit Sub
            End If
        Else
            If objDB.Exist("SELECT IC_INDEX FROM IPP_COMPANY WHERE IC_COY_NAME = '" & Common.Parse(removeGSTCode(txtVendor.Text)) & "' AND IC_COY_TYPE = '" & rbtnCoyType.SelectedValue & "' AND IC_STATUS = 'A' AND IC_COY_ID = '" & Common.Parse(strCoyId) & "'") = 0 Then
                vldsum.InnerHtml = "<li>" & objGlobal.GetErrorMessage("00340") & "</li>"
                Exit Sub
            End If
        End If

        'venidx = objipp.getIPPCompanyIndex(Common.Parse(hid5.Value), ViewState("IPPOfficer"), "I")
        If rbtnCoyType.SelectedValue = "C" Or rbtnCoyType.SelectedValue = "V" Then
            venidx = objBilling.getIPPCompIndex(Common.Parse(hid5.Value), "V", "I")
        ElseIf rbtnCoyType.SelectedValue = "R" Then
            venidx = objBilling.getIPPCompIndex(Common.Parse(hid5.Value), "B", "I")
        Else
            venidx = objBilling.getIPPCompIndex(Common.Parse(hid5.Value), rbtnCoyType.SelectedValue, "I")
        End If
        slineno = Request.Form("hidlinepointer")
        objBilling.DelBillingDocDetail(slineno, hid4.Value, venidx)
        ViewState("GSTTotalInput") = 0.0
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
        ViewState("GSTTotalInput") = 0.0
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
        ViewState("GSTTotalInput") = 0.0
        bindgrid()
    End Sub
    Sub addCell(ByRef row As DataGridItem)
        Dim cell As TableCell = New TableCell
        row.Cells.Add(cell)
    End Sub
    Sub AddRowTotal()
        'adding totals row
        Dim total As String
        Dim objipp As New Billing
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

        total = objipp.GetBillingDetailTotalAmt(hid4.Value, venidx)
        If total <> "" Then
            total = total + CDec(ViewState("GSTTotalOutput"))
            ViewState("itemTotal") = total
            ViewState("itemTotalAmt") = total
            intTotalCol = EnumDocDetail.icCostCenter - 1

            For intL = 0 To intTotalCol
                addCell(row)
            Next

            Dim txtTotal As New Label
            txtTotal.ID = "TotalAmt"
            txtTotal.Text = (Format(CDbl(total), "#,###.00"))
            row.Cells(7).Controls.Add(txtTotal)
            row.Cells(7).HorizontalAlign = HorizontalAlign.Right
            row.Cells(6).Text = "Total"
            row.Cells(6).Font.Bold = True
            dtgInvDetail.Controls(0).Controls.Add(row)
        End If

    End Sub

    Private Sub btnhidden5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhidden5.Click
        SaveBillingDoc("")
        cmdSubmit.Style("display") = "on"
        cmdSave.Style("display") = ""
        btnContinue.Style("display") = "none"
        btnContinueMultiGL.Style("display") = "none"
        btnContinueMultiGL.Visible = False
        Dim strFileName As String
        Dim strscript As New System.Text.StringBuilder
        hid4.Value = txtDocNo.Text
        cmdSubmit.Style("display") = "on"
        btnContinue.Style("display") = "none"
        btnContinueMultiGL.Style("display") = "none"
        btnContinueMultiGL.Visible = False
        strscript.Append("<script language=""javascript"">")
        strFileName = dDispatcher.direct("IPP", "IPPEntryPop.aspx", "pageid=" & strPageId & "&docno=" & txtDocNo.Text & "&doctype=" & Server.UrlEncode(ddlDocType.SelectedItem.Text) & "&vencomp=" & Server.UrlEncode(removeGSTCode(txtVendor.Text)) & "&olddocno=" & hid4.Value & "&venIdx=" & Server.UrlEncode(ViewState("venidx")) & "&MasterDoc=" & Me.rbtnMasterDoc.SelectedItem.Text & "&isResident=" & isResident)
        strFileName = Server.UrlEncode(strFileName)
        strscript.Append("ShowDialog(""" & dDispatcher.direct("IPP", "Dialog.aspx", "page=" & strFileName) & """,'520px');")
        strscript.Append("document.getElementById('btnhidden3').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script4", strscript.ToString())
        cmdSave.Style("display") = ""



    End Sub

    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        SaveBillingDoc("Save")

        If validateInput("a") Then
            If Not ViewState("SaveFailed") = "Yes" Then ViewState("oldvenidx") = ViewState("venidx")
            hidVendorIndex.Value = ViewState("venidx")
        End If

        ViewState("docno") = txtDocNo.Text
        hid4.Value = txtDocNo.Text
        ViewState("GSTTotalInput") = 0.0
        'Zulham 21/06/2017
        'IPP Stage 3
        ViewState("lineno") = 0
        bindgrid()

    End Sub
    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dtgInvDetail.SortCommand
        Grid_SortCommand(sender, e)
        dtgInvDetail.CurrentPageIndex = 0
        ViewState("GSTTotalInput") = 0.0
        bindgrid()
    End Sub

    Private Sub btnhidden9_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhidden9.Click
        Dim strscript As New System.Text.StringBuilder
        Dim slineno As String
        Dim objipp As New IPPMain
        Dim strFileName As String

        'Zulham 21/06/2017
        'IPP Stage 3
        Me.SaveBillingDoc("Update")

        'Zulham 12/03/2015 IPP GST Stage 2A
        Dim strCoyId As String
        If strDefIPPCompID = "" Then
            strCoyId = HttpContext.Current.Session("CompanyID")
        Else
            strCoyId = strDefIPPCompID
        End If

        If rbtnCoyType.SelectedValue = "C" Or rbtnCoyType.SelectedValue = "V" Then
            If objDB.Exist("SELECT IC_INDEX FROM IPP_COMPANY WHERE IC_COY_NAME = '" & Common.Parse(removeGSTCode(txtVendor.Text)) & "' AND IC_COY_TYPE = 'V' AND IC_STATUS = 'A' AND IC_COY_ID = '" & Common.Parse(strCoyId) & "'") = 0 Then
                vldsum.InnerHtml = "<li>" & objGlobal.GetErrorMessage("00340") & "</li>"
                Exit Sub
            End If
        ElseIf rbtnCoyType.SelectedValue = "R" Then
            If objDB.Exist("SELECT IC_INDEX FROM IPP_COMPANY WHERE IC_COY_NAME = '" & Common.Parse(removeGSTCode(txtVendor.Text)) & "' AND IC_COY_TYPE = 'B' AND IC_STATUS = 'A' AND IC_COY_ID = '" & Common.Parse(strCoyId) & "'") = 0 Then
                vldsum.InnerHtml = "<li>" & objGlobal.GetErrorMessage("00340") & "</li>"
                Exit Sub
            End If
        Else
            If objDB.Exist("SELECT IC_INDEX FROM IPP_COMPANY WHERE IC_COY_NAME = '" & Common.Parse(removeGSTCode(txtVendor.Text)) & "' AND IC_COY_TYPE = '" & rbtnCoyType.SelectedValue & "' AND IC_STATUS = 'A' AND IC_COY_ID = '" & Common.Parse(strCoyId) & "'") = 0 Then
                vldsum.InnerHtml = "<li>" & objGlobal.GetErrorMessage("00340") & "</li>"
                Exit Sub
            End If
        End If
        
        strPageId = 10
        slineno = Request.Form("hidlinepointer")
        strscript.Append("<script language=""javascript"">")
        If rbtnCoyType.SelectedValue = "C" Or rbtnCoyType.SelectedValue = "V" Then
            strFileName = dDispatcher.direct("Billing", "BillingEntryPop.aspx", "pageid=" & strPageId & "&coytype=V&docno=" & txtDocNo.Text & "&doctype=" & Server.UrlEncode(ddlDocType.SelectedItem.Text) & "&Lineno=" & slineno & "&action=edit&vencomp=" & Server.UrlEncode(hid5.Value) & "&olddocno=" & hid4.Value & "&isResident=" & isResident)
        ElseIf rbtnCoyType.SelectedValue = "R" Then
            strFileName = dDispatcher.direct("Billing", "BillingEntryPop.aspx", "pageid=" & strPageId & "&coytype=B&docno=" & txtDocNo.Text & "&doctype=" & Server.UrlEncode(ddlDocType.SelectedItem.Text) & "&Lineno=" & slineno & "&action=edit&vencomp=" & Server.UrlEncode(hid5.Value) & "&olddocno=" & hid4.Value & "&isResident=" & isResident)
        Else
            strFileName = dDispatcher.direct("Billing", "BillingEntryPop.aspx", "pageid=" & strPageId & "&coytype=" & rbtnCoyType.SelectedValue & "&docno=" & txtDocNo.Text & "&doctype=" & Server.UrlEncode(ddlDocType.SelectedItem.Text) & "&Lineno=" & slineno & "&action=edit&vencomp=" & Server.UrlEncode(hid5.Value) & "&olddocno=" & hid4.Value & "&isResident=" & isResident)
        End If
        strFileName = Server.UrlEncode(strFileName)
        strscript.Append("ShowDialog(""" & dDispatcher.direct("IPP", "Dialog.aspx", "page=" & strFileName) & """,'520px');")
        strscript.Append("document.getElementById('btnhidden3').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script9", strscript.ToString())
    End Sub

    Private Sub btnContinueMultiGL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnContinueMultiGL.Click
        Dim compidx As Integer
        'Dim x As Integer
        Dim objBill As New Billing
        Dim strscript As New System.Text.StringBuilder

        If vldsum.InnerHtml <> "" Then
            vldsum.InnerHtml = ""
        End If
        If validateField2() Then
            If Session("Action") = "New" Then
                If rbtnCoyType.SelectedValue = "C" Or rbtnCoyType.SelectedValue = "V" Then
                    If objDB.Exist("SELECT IC_INDEX FROM IPP_COMPANY WHERE IC_COY_NAME = '" & Common.Parse(removeGSTCode(txtVendor.Text)) & "' AND IC_COY_TYPE = 'V' AND IC_STATUS = 'A' AND IC_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'") > 0 Then
                        compidx = objBill.getIPPCompIndex(Common.Parse(removeGSTCode(txtVendor.Text)), "V")
                    End If
                ElseIf rbtnCoyType.SelectedValue = "R" Then
                    If objDB.Exist("SELECT IC_INDEX FROM IPP_COMPANY WHERE IC_COY_NAME = '" & Common.Parse(removeGSTCode(txtVendor.Text)) & "' AND IC_COY_TYPE = 'B' AND IC_STATUS = 'A' AND IC_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'") > 0 Then
                        compidx = objBill.getIPPCompIndex(Common.Parse(removeGSTCode(txtVendor.Text)), "B")
                    End If
                Else
                    If objDB.Exist("SELECT IC_INDEX FROM IPP_COMPANY WHERE IC_COY_NAME = '" & Common.Parse(removeGSTCode(txtVendor.Text)) & "' AND IC_COY_TYPE = '" & rbtnCoyType.SelectedValue & "' AND IC_STATUS = 'A' AND IC_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'") > 0 Then
                        'compidx = objIPPMain.getIPPCompanyIndex(Common.Parse(removeGSTCode(txtVendor.Text)), ViewState("IPPOfficer"))
                        compidx = objBill.getIPPCompIndex(Common.Parse(removeGSTCode(txtVendor.Text)), rbtnCoyType.SelectedValue)
                    End If
                End If
                

                If objIPPMain.CheckDuplicatedEntry(compidx, txtDocNo.Text) Then
                    strMsg = objGlobal.GetErrorMessage("00002")
                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    cmdSave.Style("display") = "none"
                ElseIf objIPPMain.CheckDuplicatedEntry2(txtPaymentAmt.Text, Common.ConvertDate(txtDocDate.Value), compidx, txtDocNo.Text) Then 'Prompt user there is invoice with same amount, vendor, doc date whether want to continue
                    strscript.Append("<script language=""javascript"">")
                    strscript.Append("document.getElementById('hidBtnContinueMultiGL').click();")
                    strscript.Append("</script>")
                    RegisterStartupScript("script30", strscript.ToString())
                Else
                    Dim strFileName As String
                    SaveBillingDoc("")
                    hid4.Value = txtDocNo.Text
                    cmdSubmit.Style("display") = "on"
                    btnContinue.Style("display") = "none"
                    btnContinueMultiGL.Style("display") = "none"
                    btnContinueMultiGL.Visible = False
                    strscript.Append("<script language=""javascript"">")
                    strFileName = dDispatcher.direct("IPP", "IPPBatchUpload.aspx", "pageid=" & strPageId & "&docno=" & txtDocNo.Text & "&doctype=" & Server.UrlEncode(ddlDocType.SelectedItem.Text) & "&vencomp=" & Server.UrlEncode(removeGSTCode(txtVendor.Text)) & "&olddocno=" & hid4.Value & "&docdate=" & txtDocDate.Value & "&mstrDoc=" & Me.rbtnMasterDoc.SelectedItem.Text)
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
                strscript.Append("<script language=""javascript"">")
                strFileName = dDispatcher.direct("IPP", "IPPBatchUpload.aspx", "pageid=" & strPageId & "&docno=" & txtDocNo.Text & "&doctype=" & Server.UrlEncode(ddlDocType.SelectedItem.Text) & "&vencomp=" & Server.UrlEncode(removeGSTCode(txtVendor.Text)) & "&olddocno=" & hid4.Value & "&docdate=" & txtDocDate.Value & "&mstrDoc=" & Me.rbtnMasterDoc.SelectedItem.Text)
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
        SaveBillingDoc("")
        cmdSubmit.Style("display") = "on"
        cmdSave.Style("display") = ""
        btnContinue.Style("display") = "none"
        btnContinueMultiGL.Style("display") = "none"
        btnContinueMultiGL.Visible = False
        Dim strFileName As String
        Dim strscript As New System.Text.StringBuilder
        hid4.Value = txtDocNo.Text
        cmdSubmit.Style("display") = "on"
        btnContinue.Style("display") = "none"
        btnContinueMultiGL.Style("display") = "none"
        btnContinueMultiGL.Visible = False
        strscript.Append("<script language=""javascript"">")
        strFileName = dDispatcher.direct("IPP", "IPPBatchUpload.aspx", "pageid=" & strPageId & "&docno=" & txtDocNo.Text & "&doctype=" & Server.UrlEncode(ddlDocType.SelectedItem.Text) & "&vencomp=" & Server.UrlEncode(removeGSTCode(txtVendor.Text)) & "&olddocno=" & hid4.Value & "&docdate=" & txtDocDate.Value)
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

        SaveBillingDoc("Update")
        If validateInput("a") Then
            ViewState("oldvenidx") = ViewState("venidx")
            hidVendorIndex.Value = ViewState("venidx")
            strscript.Append("<script language=""javascript"">")
            strFileName = dDispatcher.direct("IPP", "IPPBatchUpload.aspx", "pageid=" & strPageId & "&docno=" & txtDocNo.Text & "&doctype=" & Server.UrlEncode(ddlDocType.SelectedItem.Text) & "&vencomp=" & Server.UrlEncode(removeGSTCode(txtVendor.Text)) & "&olddocno=" & hid4.Value & "&docdate=" & txtDocDate.Value & "&venidx=" & ViewState("oldvenidx") & "&mstrDoc=" & Me.rbtnMasterDoc.SelectedItem.Text)
            strFileName = Server.UrlEncode(strFileName)
            strscript.Append("ShowDialog(""" & dDispatcher.direct("IPP", "Dialog.aspx", "page=" & strFileName) & """,'520px');")
            strscript.Append("document.getElementById('btnhidden3').click();")
            strscript.Append("</script>")
            RegisterStartupScript("script411", strscript.ToString())
            cmdSave.Style("display") = ""
        End If
    End Sub

    Private Sub rbtnCoyType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnCoyType.SelectedIndexChanged
        PopulateVenTypeAhead(rbtnCoyType.SelectedValue)
        'If rbtnCoyType.SelectedValue = "E" Then
        '    Me.rbtnMasterDoc.SelectedValue = 0
        '    Me.rbtnMasterDoc.Enabled = False
        '    Me.btnContinueMultiGL.Visible = False
        '    Me.cmdMultiGL.Visible = False
        'Else
        '    Me.rbtnMasterDoc.Enabled = True
        '    Me.btnContinueMultiGL.Visible = True
        '    Me.cmdMultiGL.Visible = True
        'End If
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
        strFileName = dDispatcher.direct("IPP", "IPPSubDocument.aspx", "pageid=" & strPageId & "&docno=" & txtDocNo.Text & "&doctype=" & Server.UrlEncode(ddlDocType.SelectedItem.Text) & "&Lineno=" & slineno & "&action=edit&vencomp=" & Server.UrlEncode(hid5.Value) & "&olddocno=" & hid4.Value & "&TotalAmt=" & Me.txtPaymentAmt.Text)
        strFileName = Server.UrlEncode(strFileName)
        strscript.Append("showModalDialog(""" & dDispatcher.direct("IPP", "Dialog.aspx", "page=" & strFileName) & """,'520px');")
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
        If Not hid6.Value Is Nothing Then
            If Not hid6.Value.Trim.Length = 0 Then
                'Jules 2018.04.28 - PAMB Scrum 2 - Change HLB to PAMB.
                'strComp = objDB.GetVal("SELECT IFNULL(ic_coy_name, '') 'ic_coy_name' FROM IPP_COMPANY WHERE IC_INDEX = '" & Common.parseNull(hid6.Value) & "' AND (IC_COY_TYPE = 'V' OR IC_COY_TYPE = 'E' OR IC_COY_TYPE = 'B') AND IC_COY_ID = 'hlb'")
                strComp = objDB.GetVal("SELECT IFNULL(ic_coy_name, '') 'ic_coy_name' FROM IPP_COMPANY WHERE IC_INDEX = '" & Common.parseNull(hid6.Value) & "' AND (IC_COY_TYPE = 'V' OR IC_COY_TYPE = 'E' OR IC_COY_TYPE = 'B') AND IC_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                'End modification.

                Return strComp.Trim
            Else
                Return strComp.Trim
            End If
            Return strComp.Trim
        End If
    End Function

End Class
