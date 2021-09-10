Imports AgoraLegacy
Imports eProcure.Component


Public Class IPPCompanyDetail
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objParam As New IPP
    Dim objIPPMain As New IPPMain
    Dim objDb As New EAD.DBCom
    Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID") 'Chee Hong - 04/Feb/2015 (IPP GST Stage 2A)
    'Dim VendorIndex As String


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Dim alButtonList As ArrayList
        If ViewState("Mode") = "Modify" Then
            alButtonList = New ArrayList
            alButtonList.Add(cmdSave)
            htPageAccess.Add("update", alButtonList)
            alButtonList = New ArrayList
            'no add button in buyer hub
        End If
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim objGlobal As New AppGlobals
        MyBase.Page_Load(sender, e)
        Dim checkEmp, EmpContent, checkCoy, checkGst As String
        ViewState("Coy_Type") = Request.QueryString("Coy_Type")
        If Request.QueryString("Coy_Index") <> "" Then
            ViewState("VendorIndex") = Request.QueryString("Coy_Index")
        End If

        If Not IsPostBack Then
            ViewState("Mode") = Request.QueryString("mode")
            isNotPostBack()
            ChkUserAccess() 'IPP Gst Stage 2A - CH - 12/02/2015
        End If
        If Me.rbtnCoyType.SelectedValue = "E" Then
            Label7.Text = "Staff ID"
            Label3.Text = "Staff Name"
            reqCoyName.ErrorMessage = "Staff Name is required."
            reqRegNo.ErrorMessage = "Staff ID is required."
            'Zulham 17072018 - PAMB
            ddlPayMethod.Enabled = True
        Else
            Label7.Text = "Business Registration No."
            Label3.Text = "Company Name"
            reqCoyName.ErrorMessage = "Company Name is required."
            reqRegNo.ErrorMessage = "Business registration no. is required."

        End If

        If ViewState("Mode") = "Add" Then
            checkEmp = " $(""#txtRegNo"").blur(function() {" & vbCrLf &
                        "if ($(""#txtRegNo"").val() != """") " & vbCrLf &
                        "{ " & vbCrLf &
                        "$(""#hidbtn2"").trigger('click'); " & vbCrLf &
                        "}" & vbCrLf &
                        "});  "

            Session("checkEmp") = checkEmp

            checkCoy = " $(""#txtCoyName"").blur(function() {" & vbCrLf &
                     "if ($(""#txtCoyName"").val() != """") " & vbCrLf &
                     "{ " & vbCrLf &
                     "$(""#hidbtn"").trigger('click'); " & vbCrLf &
                     "}" & vbCrLf &
                     "});  "
            Session("checkCoy") = checkCoy
        Else
            Session("checkCoy") = Nothing
            Session("checkEmp") = Nothing
        End If

        checkGst = " $(""#txtGstRegNo"").blur(function() {" & vbCrLf &
                "$(""#hidbtn3"").trigger('click'); " & vbCrLf &
                "});  "
        Session("checkGst") = checkGst

        lnkBack.NavigateUrl = "" & dDispatcher.direct("IPPAdmin", "IPPCompanyMaint.aspx")
        hidbtn.Attributes.Add("style", "display:none;")
        hidbtn2.Attributes.Add("style", "display:none;")
        hidbtn3.Attributes.Add("style", "display:none;")
        If ddlPayMethod.SelectedValue = "TT" Then
            Me.txtBankAcc.Attributes.Add("onkeypress", "return isNumberCharKey(event);")
            Me.txtBankAcc.Attributes.Add("onblur", "return isValidCheck2();")
        Else
            Me.txtBankAcc.Attributes.Add("onkeypress", "return isNumberKey(event);")
            Me.txtBankAcc.Attributes.Add("onblur", "return isValidCheckNumericOnly(Form1.txtBankAcc.value, txtBankAcc);")
        End If
        Me.txtRegNo.Attributes.Add("onkeypress", "return isNumberCharKey(event);")
        Me.txtRegNo.Attributes.Add("onblur", "return isValidCheck();")
        Me.txtPostcode.Attributes.Add("onkeypress", "return isNumberCharKey(event);")
        Me.txtPostcode.Attributes.Add("onblur", "return isValidCheck3();")
        Me.txtBPostcode.Attributes.Add("onkeypress", "return isNumberCharKey(event);")
        Me.txtBPostcode.Attributes.Add("onblur", "return isValidCheck3();")

        If Me.rbtnCoyType.SelectedValue = "V" Then
            Me.txtCterm.Attributes.Add("onkeypress", "return isNumberKey(event);")
            Me.txtCterm.Attributes.Add("onblur", "return isValidCheckNumericOnly(Form1.txtCterm.value, txtCterm);")
            Me.lblCTerm.Visible = True
            Me.txtCterm.Visible = True
            If Me.ddlPayMethod.SelectedValue = "RENTAS" Or Me.ddlPayMethod.SelectedValue = "TT" Or Me.ddlPayMethod.SelectedValue = "IBG" Or Me.ddlPayMethod.SelectedValue = "BC" Then
                lblBankCharge.Visible = True
                rbtnBankCharge.Visible = True

            End If
        End If

        If rbtnStatus.SelectedValue = "A" Then
            divInactiveReason.Visible = False
            txtInactiveReason.Text = ""
        Else
            divInactiveReason.Visible = True
        End If

        'Zulham 07042015 IPP GST Stage 2B
        If ddlPayMethod.SelectedValue = "NOSTRO" Then
            Me.txtBankAcc.Enabled = True
            Me.txtBankAcc.ReadOnly = False
            reqPayMethod.Enabled = False
            reqBankCode.Enabled = False
            reqBankAcc.Enabled = False
            lblerr2.Text = ""
            lblerr3.Text = ""
        End If

        'Zulham 16072018 - PAMB
        Label32.Visible = False
        'End

    End Sub

    Private Sub isNotPostBack()
        If ViewState("Mode") = "Add" Then
            clearTextBox()
            PopulateCodeTable()
            lblHeader.Text = "Add company"
            ddlPayMethod.SelectedIndex = -1
        ElseIf ViewState("Mode") = "Modify" Then
            PopulateCodeTable()
            cmdAdd.Visible = False
            Populate()
            lblHeader.Text = "Modify company"
            Session("IPPCompanyName") = txtCoyName.Text

            If ddlPayMethod.SelectedIndex = 4 Then
                ddlBankCode.Enabled = False
                txtBankName.Enabled = False
                txtBankAcc.Enabled = False
                txtBAddrLine1.Enabled = False
                txtBAddrLine2.Enabled = False
                txtBAddrLine3.Enabled = False
                txtBCity.Enabled = False
                ddlBState.Enabled = False
                txtBPostcode.Enabled = False
                ddlBCountry.Enabled = False
            End If

            If rbtnCoyType.SelectedValue = "B" Then
                ddlPayMethod.Enabled = False
                ddlBankCode.Enabled = False
                txtBAddrLine1.Enabled = False
                txtBAddrLine2.Enabled = False
                txtBAddrLine3.Enabled = False
                txtBCity.Enabled = False
                ddlBState.Enabled = False
                txtBPostcode.Enabled = False
                ddlBCountry.Enabled = False
                txtBankAcc.Enabled = False
                'Zulham 26112018
                'txtCterm.Enabled = False

                lblCurrency.Visible = True
                lblErrCurrency.Visible = True
                lblSemiCurrency.Visible = True
                ddlCurrency.Visible = True
                reqCurrency.Enabled = True

                'If txtCoyCode.Text = "HLISB" Then
                '    lblCurrency.Visible = False
                '    lblErrCurrency.Visible = False
                '    lblSemiCurrency.Visible = False
                '    ddlCurrency.Visible = False
                '    reqCurrency.Enabled = False
                'End If
            End If

            If rbtnCoyType.SelectedValue = "E" Then
                ddlPayMethod.Enabled = False
                ddlBankCode.Enabled = False
                txtBAddrLine1.Enabled = False
                txtBAddrLine2.Enabled = False
                txtBAddrLine3.Enabled = False
                txtBCity.Enabled = False
                ddlBState.Enabled = False
                txtBPostcode.Enabled = False
                ddlBCountry.Enabled = False
                txtBankAcc.Enabled = True
                'Zulham 26112018
                'txtCterm.Enabled = False
            End If

            If ddlPayMethod.SelectedValue <> "IBG" And ddlPayMethod.SelectedValue <> "RENTAS" And ddlPayMethod.SelectedValue <> "TT" And ddlPayMethod.SelectedValue <> "BC" Then
                rbtnBankCharge.Enabled = False
            End If

        End If
    End Sub

    Private Sub clearTextBox()
        Dim objGlobal As New AppGlobals
        Dim cbolist As New ListItem
        Dim objGst As New GST

        Me.rbtnCoyType.SelectedValue = "V"
        Me.txtCoyCode.Text = ""
        Me.txtCoyName.Text = ""
        Me.txtRegNo.Text = ""
        'Chee Hong - GST Enhancement - 9/9/2014
        Me.txtGstRegNo.Text = ""
        Me.txtGstRegDate.Text = "" 'Modified for IPP Gst Stage 2A - CH
        Me.hidTxtPrevGst.Text = ""
        Me.txtBillGL.Text = "" 'Modified for IPP Gst Stage 2A - CH
        cbolist.Value = "N/A"
        cbolist.Text = "N/A"
        If objGst.chkExistTaxCode("NR", "P", "IPP") = False Then
            ddlGstInputTaxCode.Items.Clear()
            ddlGstInputTaxCode.Items.Insert(0, cbolist)
        Else
            objGlobal.FillTaxCode(ddlGstInputTaxCode, , "P", "IPP")
            ddlGstInputTaxCode.SelectedValue = "NR"
        End If
        'ddlGstInputTaxCode.Items.Clear()
        ddlGstOutputTaxCode.Items.Clear()
        'ddlGstInputTaxCode.Items.Insert(0, cbolist)
        ddlGstOutputTaxCode.Items.Insert(0, cbolist)
        ddlGstOutputTaxCode.Enabled = False
        ddlGstInputTaxCode.Enabled = False
        'Zulham 10072018 - PAMB
        'txtDtLastStatus.Text = ""
        'lblDayOfLastStatus.Text = "(0 Day Lapsed)"
        'Ends
        '--------------------------------------
        Me.rbtnStatus.SelectedValue = "A"
        Me.txtAddrLine1.Text = ""
        Me.txtAddrLine2.Text = ""
        Me.txtAddrLine3.Text = ""
        Me.txtCity.Text = ""
        'Me.ddlState.SelectedIndex = 0
        'objGlobal.FillCodeTable(ddlState, CodeTable.State)
        Me.txtPostcode.Text = ""
        'Me.ddlCountry.SelectedIndex = 0
        objGlobal.FillCodeTable(ddlCountry, CodeTable.Country)
        objGlobal.FillState(ddlState, ddlCountry.SelectedValue)
        objGlobal.FillIPPCompanyCategory(ddlCategory)
        objGlobal.FillCodeTable(ddlResidentCountry, CodeTable.Country, , , False)
        Me.ddlResidentCountry.Enabled = False
        Me.rbtnResidentType.SelectedValue = "Y"
        Me.txtContact.Text = ""
        Me.txtPhone.Text = ""
        Me.txtFax.Text = ""
        Me.txtEmail.Text = ""
        Me.txtWebsite.Text = ""
        Me.ddlPayMethod.SelectedIndex = 0
        'Me.ddlBankCode.SelectedIndex = 0
        Me.ddlBankCode.Enabled = False
        If ddlBankCode.SelectedIndex > 0 Then
            Me.ddlBankCode.SelectedIndex = 0
        End If
        Me.txtBankName.Text = ""
        Me.txtBankName.Enabled = False
        Me.txtBankAcc.Text = ""
        Me.txtBAddrLine1.Text = ""
        Me.txtBAddrLine2.Text = ""
        Me.txtBAddrLine3.Text = ""
        Me.txtBCity.Text = ""
        'objGlobal.FillCodeTable(ddlBState, CodeTable.State)
        objGlobal.FillCodeTable(ddlBCountry, CodeTable.Country)
        objGlobal.FillState(ddlBState, ddlBCountry.SelectedValue)
        'Me.ddlBState.SelectedIndex = 0
        Me.txtBPostcode.Text = ""
        'Me.ddlBCountry.SelectedIndex = 0
        Me.ddlConGLCode.Enabled = False
        Me.ddlNonConGLCode.Enabled = False
        objGlobal.FillIBSGLCode(ddlConGLCode)
        objGlobal.FillIBSGLCode(ddlNonConGLCode)

        'Zulham 27112018
        'If rbtnCoyType.SelectedValue = "B" Then
        Me.trCompType.Visible = True
        'Else
        '    Me.trCompType.Visible = False
        'End If

        If rbtnCoyType.SelectedValue = "V" Then
            trIPP1.Style("display") = ""
            trIPP2.Style("display") = ""
            trIPPGST1.Style("display") = ""
            trIPPGST2.Style("display") = ""
            'Modified for IPP Gst Stage 2A - CH
            trIPPGST3.Style("display") = ""
            'Zulham 25032015 IPP GST Enhancement 
            'lblNostro.Visible = True
            'rbtnNostro.Visible = True
            Me.rbtnNostro.SelectedValue = "N"
            'If Me.rbtnNostro.SelectedValue = "Y" Then
            '    Me.ddlPayMethod.Enabled = False
            '    Me.ddlBankCode.Enabled = False
            '    Me.txtBankAcc.Enabled = False
            '    Me.txtBAddrLine1.Enabled = False
            '    Me.txtBAddrLine2.Enabled = False
            '    Me.txtBAddrLine3.Enabled = False
            '    Me.txtBCity.Enabled = False
            '    Me.txtBPostcode.Enabled = False
            '    Me.ddlBCountry.Enabled = False
            'Else
            '    Me.ddlPayMethod.Enabled = True
            '    Me.ddlBankCode.Enabled = True
            '    Me.txtBankAcc.Enabled = True
            '    Me.txtBAddrLine1.Enabled = True
            '    Me.txtBAddrLine2.Enabled = True
            '    Me.txtBAddrLine3.Enabled = True
            '    Me.txtBCity.Enabled = True
            '    Me.txtBPostcode.Enabled = True
            '    Me.ddlBCountry.Enabled = True
            'End If

            '------------------------
            trIPPS4P2.Style("display") = "" 'Added for IPP Stage 4 Phase 2 - Jules

            'Zulham 29062015 - HLB-IPP Stage 4 (CR)
            'To enable input/output tax code for employee
        ElseIf rbtnCoyType.SelectedValue = "E" Then
            trIPP1.Style("display") = ""
            trIPP2.Style("display") = ""
            trIPPGST1.Style("display") = ""
            trIPPGST2.Style("display") = ""
            Me.rbtnNostro.SelectedValue = "N"
            trIPPS4P2.Style("display") = "" 'Added for IPP Stage 4 Phase 2 - Jules
        Else
            trIPP1.Style("display") = "none"
            trIPP2.Style("display") = "none"
            trIPPGST1.Style("display") = "none"
            trIPPGST2.Style("display") = "none"
            'Modified for IPP Gst Stage 2A - CH
            trIPPGST3.Style("display") = "none"
            lblNostro.Visible = False
            rbtnNostro.Visible = False
            '---------------------------
            trIPPS4P2.Style("display") = "none" 'Added for IPP Stage 4 Phase 2 - Jules
        End If

        If rbtnCoyType.SelectedValue = "E" Then
            trIPP3.Style("display") = ""
            trIPP4.Style("display") = ""
            Me.reqJobGrade.Enabled = True
            Me.reqBC.Enabled = True
            Me.reqCC.Enabled = True
        Else
            trIPP3.Style("display") = "none"
            trIPP4.Style("display") = "none"
            Me.reqJobGrade.Enabled = False
            Me.reqBC.Enabled = False
            Me.reqCC.Enabled = False
        End If
        objGlobal.FillIPPBranchCode(ddlBC, True)
        objGlobal.FillIPPCostCentre(ddlCC, True)

        Me.lblerr1.Visible = True
        Me.lblerr2.Visible = True
        Me.lblerr3.Visible = True
        Me.reqPayMethod.Enabled = True
        Me.reqBankCode.Enabled = True
        Me.reqBankAcc.Enabled = True
        ViewState("Mode") = "Add"

        'If rbtnCoyType.SelectedValue = "B" Then
        '    Me.ddlConGLCode.SelectedIndex = 0
        '    Me.ddlNonConGLCode.SelectedIndex = 0
        'Else
        '    Me.ddlConGLCode.Enabled = False
        '    Me.ddlNonConGLCode.Enabled = False
        'End If
    End Sub


    Private Sub PopulateCodeTable()
        Dim objGlobal As New AppGlobals
        'objGlobal.FillCodeTable(ddlState, CodeTable.State)
        objGlobal.FillCodeTable(ddlCountry, CodeTable.Country)
        objGlobal.FillState(ddlState, ddlCountry.SelectedValue)
        objGlobal.FillIPPPaymentMethod(ddlPayMethod)
        objGlobal.FillIPPCompanyCategory(ddlCategory)
        objGlobal.FillCodeTable(ddlResidentCountry, CodeTable.Country, , , False)
        'objGlobal.FillBankCode(ddlBankCode, ddlPayMethod.SelectedValue)
        objGlobal.FillCodeTable(ddlCurrency, CodeTable.Currency)
        If Me.rbtnCoyType.SelectedValue = "B" Then
            trCompType.Visible = True
            ddlConGLCode.Enabled = True
            ddlNonConGLCode.Enabled = True
            objGlobal.FillIBSGLCode(ddlConGLCode)
            objGlobal.FillIBSGLCode(ddlNonConGLCode)
            lblCurrency.Visible = True
            lblErrCurrency.Visible = True
            lblSemiCurrency.Visible = True
            ddlCurrency.Visible = True
            reqCurrency.Enabled = True
        Else
            'Zulham 27112018
            trCompType.Visible = True
            ddlConGLCode.Enabled = False
            ddlNonConGLCode.Enabled = False
            lblCurrency.Visible = False
            lblErrCurrency.Visible = False
            lblSemiCurrency.Visible = False
            ddlCurrency.Visible = False
            reqCurrency.Enabled = False
        End If

        'If ddlPayMethod.SelectedValue = "CASA" Then
        '    MsgBox("CASA", MsgBoxStyle.Information, "")
        '    'Response.Write("CASA")
        'ElseIf ddlPayMethod.SelectedValue = "IBG" Then
        '    MsgBox("IBG", MsgBoxStyle.Information, "")
        '    'Response.Redirect("IBG")
        'Else
        '    'Nothing
        'End If

        'objGlobal.FillCodeTable(ddlBState, CodeTable.State)
        objGlobal.FillCodeTable(ddlBCountry, CodeTable.Country)
        objGlobal.FillState(ddlBState, ddlBCountry.SelectedValue)
        objGlobal.FillIPPBranchCode(ddlBC, True)
        objGlobal.FillIPPCostCentre(ddlCC, True)

    End Sub

    Private Sub Populate()
        Dim objComp As New Companies
        Dim objCompDetails As New Company
        Dim objGlobal As New AppGlobals
        Dim strBankName, strBCode As String
        Dim blnHasPending As Boolean
        Dim objIPP As New IPP
        Dim cbolist As New ListItem
        Dim objGst As New GST
        Dim strReverseChargeInput, strReverseChargeOutput As String

        objCompDetails.CoyType = ViewState("Coy_Type")
        If ViewState("Mode") = "Modify" Then 'Or ViewState("Mode") = "Add" Then
            If objCompDetails.CoyType = "B" Then
                'IPP Gst Stage 2A - CH - 2015-02-12 
                If strDefIPPCompID = "" Then
                    objCompDetails = objComp.GetIPPCompanyDetails(Session("CompanyId"), "B", Request.Params("Coy_Code"))
                Else
                    objCompDetails = objComp.GetIPPCompanyDetails(strDefIPPCompID, "B", Request.Params("Coy_Code"))
                End If
                'objCompDetails = objComp.GetIPPCompanyDetails(Session("CompanyId"), "B", Request.Params("Coy_Code"))
            ElseIf objCompDetails.CoyType = "E" Then
                'IPP Gst Stage 2A - CH - 2015-02-12
                If strDefIPPCompID = "" Then
                    objCompDetails = objComp.GetIPPCompanyDetails(Session("CompanyId"), "E", Request.QueryString("Coy_Name"), Request.QueryString("Coy_Index"))
                Else
                    objCompDetails = objComp.GetIPPCompanyDetails(strDefIPPCompID, "E", Request.QueryString("Coy_Name"), Request.QueryString("Coy_Index"))
                End If
                'objCompDetails = objComp.GetIPPCompanyDetails(Session("CompanyId"), "E", Request.QueryString("Coy_Name"), Request.QueryString("Coy_Index"))
            Else
                'IPP Gst Stage 2A - CH - 2015-02-12
                If strDefIPPCompID = "" Then
                    'Zulham 14072018 - PAMB
                    If Not Request.QueryString("Coy_Index") Is Nothing Then
                        If Not Request.QueryString("Coy_Index") = 0 And Not Request.QueryString("Coy_Index").Trim = "" Then
                            objCompDetails = objComp.GetIPPCompanyDetails(Session("CompanyId"), "V", Request.Params("Coy_Name"), Request.QueryString("Coy_Index"))
                        End If
                    Else
                        objCompDetails = objComp.GetIPPCompanyDetails(Session("CompanyId"), "V", Request.Params("Coy_Name"))
                    End If
                Else
                    objCompDetails = objComp.GetIPPCompanyDetails(strDefIPPCompID, "V", Request.Params("Coy_Name"))
                End If

            End If

            'Zulham 05122018
            'rbtnCoyType.Enabled = False
            'blnHasPending = objIPP.CheckPending(Common.Parse(objCompDetails.CoyName), "IPP")
            'If blnHasPending = True Then
            '    rbtnCoyType.Enabled = False
            'End If

            'Else
            '    objCompDetails = objComp.GetIPPCompanyDetails(Session("CompanyId"))
        End If

        If Not objCompDetails Is Nothing Then
            Me.rbtnCoyType.SelectedValue = objCompDetails.CoyType
            Me.txtCoyCode.Text = objCompDetails.BOtherCoy

            If Me.rbtnCoyType.SelectedValue = "V" Then
                trIPP1.Style("display") = ""
                trIPP2.Style("display") = ""
                trIPPGST1.Style("display") = ""
                trIPPGST2.Style("display") = ""
                trIPPGST3.Style("display") = "" 'Modified for IPP Gst Stage 2A - CH
                trIPPS4P2.Style("display") = "" 'Added for IPP Stage 4 Phase 2 - Jules
                Common.SelDdl(Common.parseNull(objCompDetails.CompanyCategory), ddlCategory, True, True)
                If Common.parseNull(objCompDetails.ResidentType) = "N" Then
                    rbtnResidentType.SelectedValue = "N"
                    ddlResidentCountry.Enabled = True
                    Common.SelDdl(Common.parseNull(objCompDetails.ResidentCountry), ddlResidentCountry, True, True)

                    'Zulham 25032015 Case 8603
                    'If Common.parseNull(objCompDetails.PaymentMethod) = "TT" Then
                    'ddlPayMethod.Enabled = False
                    'Else
                    ddlPayMethod.Enabled = True
                    'End If
                Else
                    ddlPayMethod.Enabled = True
                    rbtnResidentType.SelectedValue = "Y"
                    ddlResidentCountry.Enabled = False
                End If
                'Zulham IPP GST Enhancement
                'lblNostro.Visible = True 'Modified for IPP Gst Stage 2A - CH
                'rbtnNostro.Visible = True 'Modified for IPP Gst Stage 2A - CH
            Else
                trIPP1.Style("display") = "none"
                trIPP2.Style("display") = "none"
                trIPPGST1.Style("display") = "none"
                trIPPGST2.Style("display") = "none"
                'Modified for IPP Gst Stage 2A - CH
                trIPPGST3.Style("display") = "none"
                lblNostro.Visible = False
                rbtnNostro.Visible = False
                '---------------------------------
                trIPPS4P2.Style("display") = "none" 'Added for IPP Stage 4 Phase 2 - Jules
            End If

            'Chee Hong - GST Enhancement (Staff Maint.) - 11/09/2014
            If Me.rbtnCoyType.SelectedValue = "E" Then
                trIPP3.Style("display") = ""
                trIPP4.Style("display") = ""
                Me.reqJobGrade.Enabled = True
                Me.reqBC.Enabled = True
                Me.reqCC.Enabled = True
                txtJobGrade.Text = objCompDetails.JobGrade
                If objCompDetails.StaffCessationEffectiveDate <> "" Then
                    txtSCEDate.Text = Format(CDate(objCompDetails.StaffCessationEffectiveDate), "dd/MM/yyyy")
                Else
                    txtSCEDate.Text = ""
                End If
                Me.ddlBC.SelectedValue = objCompDetails.BranchCode
                Me.ddlCC.SelectedValue = objCompDetails.CostCentre
            Else
                trIPP3.Style("display") = "none"
                trIPP4.Style("display") = "none"
                Me.reqJobGrade.Enabled = False
                Me.reqBC.Enabled = False
                Me.reqCC.Enabled = False
            End If
            '---------------------------------------------------------

            If Me.rbtnCoyType.SelectedValue = "V" Or Me.rbtnCoyType.SelectedValue = "E" Then    'Vendor/Employee
                'Zulham 30012019
                'txtCoyCode.Enabled = False
                Me.trCompType.Visible = True
                ddlConGLCode.Enabled = False
                ddlNonConGLCode.Enabled = False
                lblerr1.Visible = True
                lblerr2.Visible = True
                reqPayMethod.Enabled = True
                reqBankCode.Enabled = True
                If Me.rbtnCoyType.SelectedValue = "V" Then
                    'Zulham 05112018
                    txtCterm.Text = IIf(Common.parseNull(objCompDetails.CreditTerms).ToString.Trim = "", "30", objCompDetails.CreditTerms)
                End If

            Else
                Me.trCompType.Visible = True
                'Zulham 30012019
                'Me.txtCoyCode.Enabled = False
                ddlConGLCode.Enabled = True
                ddlNonConGLCode.Enabled = True
                objGlobal.FillIBSGLCode(ddlConGLCode)
                objGlobal.FillIBSGLCode(ddlNonConGLCode)
                Common.SelDdl(Common.parseNull(objCompDetails.ConIBSGLCode), ddlConGLCode, True, True)
                Common.SelDdl(Common.parseNull(objCompDetails.NonConIBSGLCode), ddlNonConGLCode, True, True)
                lblerr1.Visible = False
                lblerr2.Visible = False
                lblerr3.Visible = False
                reqPayMethod.Enabled = False
                reqBankCode.Enabled = False
                reqBankAcc.Enabled = False

                lblCurrency.Visible = True
                lblErrCurrency.Visible = True
                lblSemiCurrency.Visible = True
                ddlCurrency.Visible = True
                reqCurrency.Enabled = True

            End If

            Me.txtCoyName.Text = objCompDetails.CoyName
            If Me.rbtnCoyType.SelectedValue = "E" Then
                Me.txtCoyName.Enabled = True
                Me.txtRegNo.Enabled = False
                'Zulham 26112018
                'Me.txtCterm.Visible = False
                'Me.lblCTerm.Visible = False
            ElseIf Me.rbtnCoyType.SelectedValue = "V" Then
                Me.txtRegNo.Enabled = False
                Me.txtCoyName.Enabled = True
            Else
                Me.txtCoyName.Enabled = True
                Me.txtRegNo.Enabled = False
            End If


            Me.txtRegNo.Text = objCompDetails.BusinessRegNo
            Me.rbtnStatus.SelectedValue = objCompDetails.Status
            Me.txtAddrLine1.Text = objCompDetails.Address1
            Me.txtAddrLine2.Text = objCompDetails.Address2
            Me.txtAddrLine3.Text = objCompDetails.Address3
            Me.txtCity.Text = objCompDetails.City
            If objCompDetails.State = "n.a." Then
                objGlobal.FillState(ddlState, objCompDetails.Country)
            Else
                objGlobal.FillState(ddlState, objCompDetails.Country)
                Me.ddlState.SelectedValue = objCompDetails.State
            End If

            Me.txtPostcode.Text = objCompDetails.PostCode
            Me.ddlCountry.SelectedValue = objCompDetails.Country
            Me.txtContact.Text = objCompDetails.ContactPerson
            Me.txtPhone.Text = objCompDetails.Phone
            Me.txtFax.Text = objCompDetails.Fax
            Me.txtEmail.Text = objCompDetails.Email
            Me.txtWebsite.Text = objCompDetails.WebSites
            'Me.ddlPayMethod.SelectedValue = objCompDetails.PaymentMethod
            'Common.SelDdl(Common.parseNull(objCompDetails.PaymentMethod), ddlPayMethod, True, True)

            'Fill Payment Method Dropdownlist
            If rbtnCoyType.SelectedValue = "B" Then
                If objCompDetails.PaymentMethod <> "" Then
                    Common.SelDdl(Common.parseNull(objCompDetails.PaymentMethod), ddlPayMethod, True, True)
                    objGlobal.FillBankCode(ddlBankCode, objCompDetails.PaymentMethod)
                    Common.SelDdl(Common.parseNull(objCompDetails.BankCode), ddlBankCode, True, True)
                    strBankName = objComp.GetBankName(ddlBankCode.SelectedValue)
                    Me.txtBankName.Text = strBankName
                Else
                    objGlobal.FillIPPPaymentMethod(ddlPayMethod)
                    ddlPayMethod.SelectedIndex = -1
                End If
                'Zulham 26112018
                'Me.txtCterm.Visible = False
                'Me.lblCTerm.Visible = False

            Else
                'Zulham 14072018 - PAMB
                If Not Common.parseNull(objCompDetails.PaymentMethod) = "" Then
                    Common.SelDdl(Common.parseNull(objCompDetails.PaymentMethod), ddlPayMethod, True, True)
                    objGlobal.FillBankCode(ddlBankCode, objCompDetails.PaymentMethod)
                    Common.SelDdl(Common.parseNull(objCompDetails.BankCode), ddlBankCode, True, True)
                    strBankName = objComp.GetBankName(ddlBankCode.SelectedValue)
                    Me.txtBankName.Text = strBankName
                End If
                'End
            End If

            'Zulham 23112018
            ''Fill BankCode Dropdownlist
            'If objCompDetails.PaymentMethod = "CASA" Or objCompDetails.PaymentMethod = "BC" Then
            '    strBankName = objParam.GetIPPValue(objCompDetails.PaymentMethod & "_BANKCODE")
            '    If strBankName <> "False" Then
            '        ddlBankCode.SelectedValue = strBankName
            '        Me.txtBankName.Text = strBankName
            '    Else
            '        Me.txtBankName.Text = ""
            '    End If
            'End If

            If rbtnCoyType.SelectedValue = "V" Or rbtnCoyType.SelectedValue = "E" Then  'Vendor/Employee
                If ddlPayMethod.SelectedValue = "BC" Then
                    lblerr3.Visible = False
                    reqBankAcc.Enabled = False
                Else
                    lblerr3.Visible = True
                    reqBankAcc.Enabled = True
                    'Zulham 06042015 Case 8603
                    reqPayMethod.Enabled = False
                    reqBankCode.Enabled = False
                    reqBankAcc.Enabled = False
                End If
            End If

            Me.txtBankName.Enabled = False
            Me.txtBankAcc.Text = objCompDetails.AccountNo
            Me.txtBAddrLine1.Text = objCompDetails.BankAddrLine1
            Me.txtBAddrLine2.Text = objCompDetails.BankAddrLine2
            Me.txtBAddrLine3.Text = objCompDetails.BankAddrLine3
            Me.txtBCity.Text = objCompDetails.BankCity
            If objCompDetails.BankState = "n.a." Then
                objGlobal.FillState(ddlBState, objCompDetails.BankCountry)
            Else
                objGlobal.FillState(ddlBState, objCompDetails.BankCountry)
                Me.ddlBState.SelectedValue = objCompDetails.BankState
            End If
            Me.ddlBState.SelectedValue = objCompDetails.BankState
            Me.txtBPostcode.Text = objCompDetails.BankPostcode
            Me.ddlBCountry.SelectedValue = objCompDetails.BankCountry

            If ddlPayMethod.SelectedValue = "IBG" Or ddlPayMethod.SelectedValue = "RENTAS" Or ddlPayMethod.SelectedValue = "TT" Or ddlPayMethod.SelectedValue = "BC" Then
                lblBankCharge.Visible = True
                rbtnBankCharge.Visible = True
                rbtnBankCharge.SelectedValue = objCompDetails.WaiveCharges
            End If

            Me.txtInactiveReason.Text = objCompDetails.InactiveReason

            'Chee Hong - IPP GST Enhancement - 9/9/2014
            If rbtnCoyType.SelectedValue = "V" Then
                ddlGstInputTaxCode.Items.Clear()
                ddlGstOutputTaxCode.Items.Clear()
                cbolist.Value = "N/A"
                cbolist.Text = "N/A"
                Me.txtGstRegNo.Text = objCompDetails.TaxRegNo
                'Yap: 2015-Feb-02: GST IPP Stage 2A
                Me.txtGstRegDate.Text = objCompDetails.GstRegDate
                Me.hidTxtPrevGst.Text = objCompDetails.TaxRegNo
                If objCompDetails.TaxRegNo = "" Then
                    If rbtnResidentType.SelectedValue = "N" Then
                        ddlGstInputTaxCode.Enabled = True
                        objGlobal.FillTaxCode(ddlGstInputTaxCode, , "P", "IPP")
                        ddlGstInputTaxCode.SelectedValue = objCompDetails.GstInputTaxCode

                        objIPPMain.getReverseCharge(strReverseChargeInput, strReverseChargeOutput)
                        If strReverseChargeInput = objCompDetails.GstInputTaxCode Then
                            objGlobal.FillTaxCode(ddlGstOutputTaxCode, , "S", "IPP")
                            ddlGstOutputTaxCode.SelectedValue = objCompDetails.GstOutputTaxCode
                        Else
                            ddlGstOutputTaxCode.Items.Clear()
                            ddlGstOutputTaxCode.Items.Insert(0, cbolist)
                        End If
                        ddlGstOutputTaxCode.Enabled = False

                        'ddlGstOutputTaxCode.Enabled = True
                        'objGlobal.FillTaxCode(ddlGstOutputTaxCode, , "S", "IPP")
                        'ddlGstOutputTaxCode.SelectedValue = objCompDetails.GstOutputTaxCode                    
                    Else
                        If objGst.chkExistTaxCode("NR", "P", "IPP") = False Then
                            'ddlGstInputTaxCode.Items.Clear()
                            ddlGstInputTaxCode.Items.Insert(0, cbolist)
                        Else
                            objGlobal.FillTaxCode(ddlGstInputTaxCode, , "P", "IPP")
                            ddlGstInputTaxCode.SelectedValue = "NR"
                        End If

                        ddlGstOutputTaxCode.Items.Insert(0, cbolist)
                        ddlGstOutputTaxCode.Enabled = False
                        ddlGstInputTaxCode.Enabled = False
                    End If
                Else
                    ddlGstInputTaxCode.Enabled = True
                    objGlobal.FillTaxCode(ddlGstInputTaxCode, , "P", "IPP")

                    'Jules 2018.10.29 - Default to ST0 if no tax code.
                    If objCompDetails.GstInputTaxCode = "" Then
                        Common.SelDdl("ST0", ddlGstInputTaxCode, True)
                    Else 'original code
                        ddlGstInputTaxCode.SelectedValue = objCompDetails.GstInputTaxCode
                    End If
                    'End modification.

                    If rbtnResidentType.SelectedValue = "N" Then
                        ddlGstOutputTaxCode.Enabled = True
                        objGlobal.FillTaxCode(ddlGstOutputTaxCode, , "S", "IPP")
                        ddlGstOutputTaxCode.SelectedValue = objCompDetails.GstOutputTaxCode
                    Else
                        ddlGstOutputTaxCode.Enabled = False
                        ddlGstOutputTaxCode.Items.Insert(0, cbolist)
                    End If
                End If

                'Jules 2015.08.17 - IPP Stage 4 Phase 2
                'If objCompDetails.GSTDateLastStatus = "" Then
                If objCompDetails.SysValDate = "" Then
                    'txtDtLastStatus.Text = "" 'Zulham 10072018 - PAMB

                    'Jules 2018.10.29 - Default to system date if SST-registered
                    If txtGstRegNo.Text <> "" Then
                        txtSystemValDate.Text = Format(Now, "dd/MM/yyyy")
                    Else
                        txtSystemValDate.Text = ""
                    End If
                    'End modification.

                    'lblDayOfLastStatus.Text = "(0 Day Lapsed)" 'Zulham 10072018 - PAMB
                Else
                    'Jules 2015.08.17 - IPP Stage 4 Phase 2
                    'txtDtLastStatus.Text = Format(CDate(objCompDetails.GSTDateLastStatus), "dd/MM/yyyy")
                    'Dim intDays As Integer = Date.Now.Subtract(CDate(objCompDetails.GSTDateLastStatus)).Days
                    'lblDayOfLastStatus.Text = Format(CDate(objCompDetails.GSTDateLastStatus), "dd/MM/yyyy") & " (" & intDays & " Days Lapsed)"
                    txtSystemValDate.Text = Format(CDate(objCompDetails.SysValDate), "dd/MM/yyyy")
                    'txtDtLastStatus.Text = Format(CDate(objCompDetails.SysValDate), "dd/MM/yyyy") 'Zulham 10072018 - PAMB
                    Dim intDays As Integer = Date.Now.Subtract(CDate(objCompDetails.SysValDate)).Days
                    'lblDayOfLastStatus.Text = Format(CDate(objCompDetails.SysValDate), "dd/MM/yyyy") & " (" & intDays & " Days Lapsed)" 'Zulham 10072018 - PAMB
                End If

                'Modified for IPP Gst Stage 2A - CH
                If objCompDetails.GstRegDate <> "" Then
                    txtGstRegDate.Text = Format(CDate(objCompDetails.GstRegDate), "dd/MM/yyyy")
                Else
                    'Jules 2018.10.29 - Default to system date if SST-registered
                    If txtGstRegNo.Text <> "" Then
                        txtGstRegDate.Text = Format(Now, "dd/MM/yyyy")
                    Else
                        txtGstRegDate.Text = ""
                    End If
                    'End modification.
                End If
                txtBillGL.Text = objCompDetails.BillGLCode
                If objCompDetails.NostroIncome = "" Or objCompDetails.NostroIncome = "N" Then
                    rbtnNostro.SelectedValue = "N"
                Else
                    rbtnNostro.SelectedValue = "Y"
                End If

                'Zulham 29062015 - HLB-IPP Stage 4 (CR)
                trEmployeeInputTaxCode.Visible = False
                trEmployeeOutputTaxCode.Visible = False

                'If rbtnNostro.SelectedValue = "Y" Then
                '    ddlPayMethod.Enabled = False
                '    ddlBankCode.Enabled = False
                '    txtBankAcc.Enabled = False
                '    txtBAddrLine1.Enabled = False
                '    txtBAddrLine2.Enabled = False
                '    txtBAddrLine3.Enabled = False
                '    txtBCity.Enabled = False
                '    txtBPostcode.Enabled = False
                '    ddlBCountry.Enabled = False
                'Else
                '    ddlPayMethod.Enabled = True
                '    ddlBankCode.Enabled = True
                '    txtBankAcc.Enabled = True
                '    txtBAddrLine1.Enabled = True
                '    txtBAddrLine2.Enabled = True
                '    txtBAddrLine3.Enabled = True
                '    txtBCity.Enabled = True
                '    txtBPostcode.Enabled = True
                '    ddlBCountry.Enabled = True
                'End If
                '-----------------------------------
                'Zulham 29062015 - HLB-IPP Stage 4 (CR)
            ElseIf rbtnCoyType.SelectedValue = "E" Then
                trIPP1.Style("display") = "none"
                trIPP2.Style("display") = "none"
                trIPPGST1.Style("display") = "none"
                trIPPGST2.Style("display") = "none"
                trIPPS4P2.Style("display") = "none" 'Added for IPP Stage 4 Phase 2 - Jules
                trEmployeeInputTaxCode.Visible = True
                trEmployeeOutputTaxCode.Visible = True
                objGlobal.FillTaxCode(ddlEmpGstInputTaxCode, , "P", "IPP")
                objGlobal.FillTaxCode(ddlEmpGstOutputTaxCode, , "S", "IPP")
                ddlEmpGstInputTaxCode.SelectedValue = Common.parseNull(objCompDetails.GstInputTaxCode)
                ddlEmpGstOutputTaxCode.SelectedValue = Common.parseNull(objCompDetails.GstOutputTaxCode)
            End If
            '--------------------------------------------

            '    Common.SelDdl(Common.parseNull(objCompDetails.Country), ddlCountry, True, True)
            '    If objCompDetails.Country <> "MY" Then
            '        Dim objGlobal As New AppGlobals
            '        objGlobal.FillState(ddlState, ddlCountry.SelectedItem.Value)
            '        objGlobal = Nothing
            '    End If

            '    If objCompDetails.CoyType = "VENDOR" Then
            '        ''tdVendor.Style.Item("Display") = ""
            '        'trPay.Style.Item("Display") = "none"
            '        'PreTemp.Value = "Preview Invoice Template"
            '        'ViewState("Side") = objCompDetails.CoyType
            '        'ElseIf objCompDetails.CoyType = "BUYER" Then
            '        '    trPay.Style.Item("Display") = ""
            '        '    tdVendor.Style.Item("Display") = "none"
            '        '    ViewState("CoyLogo") = objCompDetails.CoyLogo
            '        '    ViewState("Side") = objCompDetails.CoyType
            '        'ElseIf objCompDetails.CoyType = "BOTH" Then
            '        '    'tdVendor.Style.Item("Display") = ""
            '        '    trPay.Style.Item("Display") = ""
            '        '    ViewState("Side") = objCompDetails.CoyType
            '    End If
            'If rbtnCoyType.SelectedValue = "B" And txtCoyCode.Text <> "HLISB" Then
            '    If objCompDetails.Currency = "" Then
            '        ddlCurrency.SelectedValue = 0
            '    Else
            Me.ddlCurrency.SelectedValue = objCompDetails.Currency
            'End If

            ' End If

        End If
    End Sub

    Private Function GetInput() As Company
        Dim objCompDetails As New Company
        objCompDetails.CoyType = rbtnCoyType.SelectedValue
        If rbtnCoyType.SelectedValue = "B" Then
            objCompDetails.BOtherCoy = txtCoyCode.Text
            objCompDetails.ConIBSGLCode = ddlConGLCode.SelectedValue
            objCompDetails.NonConIBSGLCode = ddlNonConGLCode.SelectedValue
        Else
            objCompDetails.BOtherCoy = txtCoyCode.Text
        End If
        objCompDetails.CoyId = Session("CompanyID")
        objCompDetails.BOtherCoy = txtCoyCode.Text
        objCompDetails.CoyName = txtCoyName.Text
        objCompDetails.BusinessRegNo = txtRegNo.Text
        objCompDetails.Status = rbtnStatus.SelectedValue
        objCompDetails.Address1 = txtAddrLine1.Text
        objCompDetails.Address2 = txtAddrLine2.Text
        objCompDetails.Address3 = txtAddrLine3.Text
        objCompDetails.City = txtCity.Text
        objCompDetails.State = ddlState.SelectedValue
        objCompDetails.PostCode = txtPostcode.Text
        objCompDetails.Country = ddlCountry.SelectedValue
        objCompDetails.ContactPerson = txtContact.Text
        objCompDetails.Phone = txtPhone.Text
        objCompDetails.Fax = txtFax.Text
        objCompDetails.Email = txtEmail.Text
        objCompDetails.WebSites = txtWebsite.Text
        objCompDetails.PaymentMethod = ddlPayMethod.SelectedValue
        objCompDetails.BankCode = ddlBankCode.SelectedValue
        objCompDetails.AccountNo = txtBankAcc.Text
        objCompDetails.BankAddrLine1 = txtBAddrLine1.Text
        objCompDetails.BankAddrLine2 = txtBAddrLine2.Text
        objCompDetails.BankAddrLine3 = txtBAddrLine3.Text
        objCompDetails.BankCity = txtBCity.Text
        objCompDetails.BankState = ddlBState.SelectedValue
        objCompDetails.BankPostcode = txtBPostcode.Text
        objCompDetails.BankCountry = ddlBCountry.SelectedValue

        If ddlPayMethod.SelectedValue = "IBG" Or ddlPayMethod.SelectedValue = "RENTAS" Or ddlPayMethod.SelectedValue = "TT" Or ddlPayMethod.SelectedValue = "BC" Then
            objCompDetails.WaiveCharges = rbtnBankCharge.SelectedValue
        Else
            objCompDetails.WaiveCharges = ""
        End If

        'Zulham 26112018
        'If rbtnCoyType.SelectedValue = "V" Then
        objCompDetails.CreditTerms = IIf(txtCterm.Text.Trim = "", 30, txtCterm.Text)
        'Else
        'objCompDetails.CreditTerms = 0
        'End If

        If rbtnCoyType.SelectedValue = "B" Then
            objCompDetails.Currency = ddlCurrency.SelectedValue
        End If
        objCompDetails.InactiveReason = txtInactiveReason.Text
        If rbtnCoyType.SelectedValue = "V" Then
            objCompDetails.CompanyCategory = ddlCategory.SelectedValue
            objCompDetails.ResidentType = rbtnResidentType.SelectedValue
            If rbtnResidentType.SelectedValue = "Y" Then
                objCompDetails.ResidentCountry = ""
            Else
                objCompDetails.ResidentCountry = ddlResidentCountry.SelectedValue
            End If

            'Chee Hong - IPP GST Enhancement - 9/9/2014
            objCompDetails.TaxRegNo = txtGstRegNo.Text
            'objCompDetails.GSTDateLastStatus = txtDtLastStatus.Text 'Zulham 10072018 - PAMB
            objCompDetails.GstRegDate = txtGstRegDate.Text 'Modified for IPP Gst Stage 2A - CH
            'Zulham 10072018 - PAMB
            'If txtDtLastStatus.Text = "" Then
            '    lblDayOfLastStatus.Text = "(0 Day Lapsed)"
            'Else
            '    Dim dtLast As Date = CDate(txtDtLastStatus.Text)
            '    Dim intDays As Integer = Date.Now.Subtract(dtLast).Days
            '    lblDayOfLastStatus.Text = Format(CDate(txtDtLastStatus.Text), "dd/MM/yyyy") & " (" & intDays & " Days Lapsed)"
            'End If
            'End
            objCompDetails.GstInputTaxCode = ddlGstInputTaxCode.SelectedValue
            objCompDetails.GstOutputTaxCode = ddlGstOutputTaxCode.SelectedValue
            objCompDetails.BillGLCode = txtBillGL.Text 'Modified for IPP Gst Stage 2A - CH
            objCompDetails.NostroIncome = rbtnNostro.SelectedValue 'Modified for IPP Gst Stage 2A - CH

            objCompDetails.SysValDate = txtSystemValDate.Text 'Jules 2015.08.14 - IPP Stage 4 Phase 2
        End If

        If rbtnCoyType.SelectedValue = "E" Then
            objCompDetails.JobGrade = txtJobGrade.Text
            objCompDetails.StaffCessationEffectiveDate = txtSCEDate.Text
            objCompDetails.BranchCode = ddlBC.SelectedValue
            objCompDetails.CostCentre = ddlCC.SelectedValue

            'Zulham 29062015 - HLB-IPP GST Stage 4 (CR)
            objCompDetails.GstInputTaxCode = ddlEmpGstInputTaxCode.SelectedValue
            objCompDetails.GstOutputTaxCode = ddlEmpGstOutputTaxCode.SelectedValue

        End If

        Return objCompDetails
    End Function

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        clearTextBox()
        Label7.Text = "Business Registration No."
        Label3.Text = "Company Name"
        reqCoyName.ErrorMessage = "Company Name is required."
        reqRegNo.ErrorMessage = "Business registration no. is required."

        Me.txtCoyName.Enabled = True
        Me.txtRegNo.Enabled = True
        Me.ddlPayMethod.Enabled = True
        Me.txtBankAcc.Enabled = True
        Me.txtBAddrLine1.Enabled = True
        Me.txtBAddrLine2.Enabled = True
        Me.txtBAddrLine3.Enabled = True
        Me.txtBCity.Enabled = True
        Me.ddlBState.Enabled = True
        Me.txtBPostcode.Enabled = True
        Me.ddlBCountry.Enabled = True
        rbtnCoyType.Enabled = True

        Me.lblCTerm.Visible = True
        Me.txtCterm.Visible = True
        Me.txtCterm.Text = "30"
        Me.rbtnBankCharge.Visible = False
        Me.lblBankCharge.Visible = False

        ViewState("Mode") = "Add"
        lblHeader.Text = "Add company"
        Dim checkEmp, EmpContent, checkCoy, checkGst As String
        checkEmp = " $(""#txtRegNo"").blur(function() {" & vbCrLf &
                        "if ($(""#txtRegNo"").val() != """") " & vbCrLf &
                        "{ " & vbCrLf &
                        "$(""#hidbtn2"").trigger('click'); " & vbCrLf &
                        "}" & vbCrLf &
                        "});  "

        Session("checkEmp") = checkEmp

        checkCoy = " $(""#txtCoyName"").blur(function() {" & vbCrLf &
                 "if ($(""#txtCoyName"").val() != """") " & vbCrLf &
                 "{ " & vbCrLf &
                 "$(""#hidbtn"").trigger('click'); " & vbCrLf &
                 "}" & vbCrLf &
                 "});  "
        Session("checkCoy") = checkCoy

        checkGst = " $(""#txtGstRegNo"").blur(function() {" & vbCrLf &
                 "$(""#hidbtn3"").trigger('click'); " & vbCrLf &
                 "});  "
        Session("checkGst") = checkGst

        'Zulham 29062015 - HLB-IPP Stage 4 (CR)
        If Me.rbtnCoyType.SelectedValue = "E" Then
            trEmployeeInputTaxCode.Visible = True
            trEmployeeOutputTaxCode.Visible = True
        Else
            trEmployeeInputTaxCode.Visible = False
            trEmployeeOutputTaxCode.Visible = False
        End If

    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        'ViewState("Mode") = "Add"
        'vldsumm.ShowSummary = True

        'Zulham 30012019
        If rbtnCoyType.SelectedValue = "V" Or rbtnCoyType.SelectedValue = "E" Then
            'reqCoyCode.Enabled = False
            reqConIBSGLCode.Enabled = False
        End If

        If rbtnCoyType.SelectedValue = "E" Then
            reqJobGrade.Enabled = True
            reqBC.Enabled = True
            reqCC.Enabled = True
        Else
            reqJobGrade.Enabled = False
            reqBC.Enabled = False
            reqCC.Enabled = False
        End If

        Dim strErrMsg As String
        Dim objComp As New Companies
        Dim objCompDetails As New Company
        Dim intMsgNo, strInd As Integer
        Dim blnDuplicate As Boolean
        Dim objGlobal As New AppGlobals
        Dim strMsg As String
        Dim intMsg As Integer
        Dim objIPPMain As New IPPMain 'Modified for IPP Gst Stage 2A - CH

        If ddlState.SelectedValue = "" Then
            reqState.Enabled = True

        End If
        If ddlCountry.SelectedValue = "" Then
            reqCountry.Enabled = True
        End If

        If Page.IsValid Then
            Try
                If rbtnStatus.SelectedValue = "I" And txtInactiveReason.Text = "" Then
                    strMsg = "Reason for Company Inactivation " & objGlobal.GetErrorMessage("00001")
                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    Exit Try
                End If

                'Zulham 25032015 Case 8603
                If Not rbtnResidentType.SelectedValue = "Y" Then
                    'Zulham 17102018 - PAMB
                    If Not Me.ddlPayMethod.SelectedValue = "TT" And Not Me.ddlPayMethod.SelectedValue = "NOSTRO" And Not Me.ddlPayMethod.SelectedValue = "BD" Then
                        strMsg = "Invalid Payment Method."
                        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                        Exit Try
                    End If
                End If

                'Zulham 29062015 - HLB-IPP Stage 4(CR)
                'Additional validation for HLBB account no
                'If Me.ddlPayMethod.SelectedIndex > 0 Then 'Jules 2015.08.19
                '    If Me.ddlBankCode.SelectedItem.Text.Trim.ToUpper = "HLBB" And Me.ddlPayMethod.SelectedItem.Text.Trim.ToUpper <> "BC" Then 'Jules 2015.09.29
                '        If txtBankAcc.Text.Trim.Length <> 11 Then
                '            strMsg = "HLBB account number must be 11 digits."
                '            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                '            Exit Try
                '        End If
                '    End If
                'End If
                'mimi 2018-04-24 : remove hardcode HLB to PAMB
                If Me.ddlPayMethod.SelectedIndex > 0 Then 'Jules 2015.08.19
                    If Me.ddlBankCode.SelectedItem.Text.Trim.ToUpper = "PAMB" And Me.ddlPayMethod.SelectedItem.Text.Trim.ToUpper <> "BC" Then 'Jules 2015.09.29
                        If txtBankAcc.Text.Trim.Length <> 11 Then
                            strMsg = "PAMB account number must be 11 digits."
                            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                            Exit Try
                        End If
                    End If
                End If

                If rbtnCoyType.SelectedValue = "V" And rbtnResidentType.SelectedValue = "N" And ddlResidentCountry.SelectedValue = "MY" Then
                    strMsg = "Please re-select the Resident Country."
                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    Exit Try
                End If

                ''Zulham 03112018
                ''Chee Hong - IPP GST Enhancement - 9/9/2014
                'If rbtnCoyType.SelectedValue = "V" And ddlGstInputTaxCode.Enabled = True And ddlGstInputTaxCode.SelectedValue = "" Then
                '    strMsg = "Please select the SST Input Tax Code."
                '    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                '    Exit Try
                'End If

                'If rbtnCoyType.SelectedValue = "V" And ddlGstOutputTaxCode.Enabled = True And ddlGstOutputTaxCode.SelectedValue = "" Then
                '    strMsg = "Please select the SST Output Tax Code."
                '    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                '    Exit Try
                'End If

                'Chee Hong - IPP GST 2A Enhancement - 16/12/2014
                If rbtnCoyType.SelectedValue = "V" And txtGstRegNo.Text.Trim <> "" And txtGstRegDate.Text.Trim = "" Then
                    'strMsg = "Please select the GST Registration Date."
                    strMsg = "SST Effective Date is required." 'Jules 2015.08.19
                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    Exit Try
                End If

                'Zulham 10072018 - PAMB
                If rbtnCoyType.SelectedValue = "V" And txtGstRegNo.Text.Trim <> "" And txtSystemValDate.Text.Trim = "" Then
                    strMsg = "SST Validation Date is required."
                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    Exit Try
                End If
                'End

                If rbtnCoyType.SelectedValue = "V" And txtBillGL.Text <> "" And objIPPMain.ChkValidGLCode(txtBillGL.Text) = False Then
                    strMsg = "Invalid Billing GL Code."
                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    Exit Try
                End If
                '-----

                objCompDetails = GetInput()

                If ViewState("Mode") = "Add" Then
                    blnDuplicate = objComp.AddIPPCompany(objCompDetails)
                    If blnDuplicate = False Then
                        strMsg = objGlobal.GetErrorMessage("00002")
                        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)

                        ViewState("Mode") = "Add"
                    Else
                        strMsg = objGlobal.GetErrorMessage("00003")
                        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)

                        lblHeader.Text = "Modify company"
                        ViewState("Mode") = "Modify"
                        '    Me.Response.Redirect(dDispatcher.direct("IPPAdmin", "IPPCompanyDetail.aspx", "mode=Modify&Coy_Code=" & Server.UrlEncode(Trim(txtCoyCode.Text)) & "&Coy_Name=" & Server.UrlEncode(Trim(txtCoyName.Text)) & "&Coy_Type=" & Server.UrlEncode(rbtnCoyType.SelectedValue) & "&Coy_Status=" & Server.UrlEncode(ddlNonConGLCode.SelectedValue)))
                    End If

                    'If ViewState("Mode") = "Modify" And VendorIndex = "" Then
                    ViewState("VendorIndex") = objDb.GetVal("SELECT IC_INDEX FROM IPP_COMPANY WHERE IC_COY_NAME = '" & Common.Parse(txtCoyName.Text) & "' AND IC_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'")
                    'End If
                ElseIf ViewState("Mode") = "Modify" Then
                    'get the ic_index

                    'strInd = objParam.GetInd(objCompDetails.CoyName, objCompDetails.CoyId, objCompDetails.BusinessRegNo, objCompDetails.CoyType)
                    strInd = ViewState("VendorIndex")
                    'Zulham 05122018
                    Dim strSQL As String = ""
                    If objCompDetails.CoyType = "B" Then

                        strSQL = "SELECT * FROM invoice_mstr " &
                        "INNER JOIN invoice_details ON im_invoice_no = id_invoice_no AND im_s_coy_id = id_s_coy_id " &
                        "WHERE im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'  AND id_pay_for = '" & Common.Parse(objCompDetails.BOtherCoy) & "' AND (im_invoice_status NOT IN (4,14,15) OR ((im_invoice_status = 14 AND im_route_to IS NOT NULL)))"

                        If objDb.Exist(strSQL) <> 0 Then
                            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00018"), MsgBoxStyle.Information)
                        End If

                        intMsg = objComp.UpdateIPPCompany(objCompDetails, "B", objCompDetails.BOtherCoy, strInd)

                        If intMsg = -99 Then
                            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00018"), MsgBoxStyle.Information)
                        ElseIf intMsg = WheelMsgNum.Save Then
                            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00003"), MsgBoxStyle.Information)

                        ElseIf intMsg = WheelMsgNum.NotSave Then
                            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00007"), MsgBoxStyle.Information)
                        End If

                    Else    'Can cater for vendor/employee type

                        strSQL = "SELECT * FROM invoice_mstr " &
                        "INNER JOIN invoice_details ON im_invoice_no = id_invoice_no AND im_s_coy_id = id_s_coy_id " &
                        "WHERE im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'  AND  id_s_coy_id = '" & Common.Parse(strInd) & "' AND (im_invoice_status NOT IN (4,14,15) OR ((im_invoice_status = 14 AND im_route_to IS NOT NULL)))"

                        If objDb.Exist(strSQL) <> 0 Then
                            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00018"), MsgBoxStyle.Information)
                        End If

                        intMsg = objComp.UpdateIPPCompany(objCompDetails, "V", Session("IPPCompanyName"), strInd)

                        If intMsg = -99 Then
                            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00018"), MsgBoxStyle.Information)
                        ElseIf intMsg = WheelMsgNum.Save Then
                            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00003"), MsgBoxStyle.Information)

                        ElseIf intMsg = WheelMsgNum.NotSave Then
                            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00007"), MsgBoxStyle.Information)
                        End If
                    End If
                    'Populate()
                End If


                'Zulham 05122018
                If rbtnCoyType.SelectedValue = "E" Then
                    Me.txtCoyName.Enabled = True
                    Me.txtRegNo.Enabled = False
                    'rbtnCoyType.Enabled = False
                ElseIf rbtnCoyType.SelectedValue = "V" Then
                    Me.txtCoyName.Enabled = True
                    Me.txtRegNo.Enabled = False
                    'rbtnCoyType.Enabled = False
                Else
                    Me.txtCoyName.Enabled = True
                    Me.txtRegNo.Enabled = False
                    'rbtnCoyType.Enabled = False
                End If

                'Common.NetMsgbox(Me, objComp.Message, MsgBoxStyle.Information)
            Catch ex As Exception
                Common.NetMsgbox(Me, ex.Message)
            End Try
        End If
    End Sub

    Private Sub ddlPayMethod_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPayMethod.SelectedIndexChanged
        Dim objGlobal As New AppGlobals
        Dim objdb As New EAD.DBCom
        Dim dsBCode As DataSet
        Dim defaultbankcode, strBCode, strBankName As String
        Dim objCompDetails As New Companies
        txtBankName.Text = ""
        'txtBankAcc.Text = ""
        If ddlPayMethod.SelectedIndex <> 0 Then
            ddlBankCode.Enabled = True
            objGlobal.FillBankCode(ddlBankCode, ddlPayMethod.SelectedValue)
                If ddlBankCode.SelectedValue <> "n.a." Then
                ddlBankCode.SelectedIndex = 1
                If ddlPayMethod.SelectedValue = "CASA" Or ddlPayMethod.SelectedValue = "BC" Then
                    If ddlPayMethod.SelectedValue = "BC" Then
                        'Zulham 23112018
                        'ddlBankCode.Enabled = False
                        txtBankAcc.Enabled = False
                        txtBAddrLine1.Enabled = False
                        txtBAddrLine2.Enabled = False
                        txtBAddrLine3.Enabled = False
                        txtBCity.Enabled = False
                        ddlBState.Enabled = False
                        txtBPostcode.Enabled = False
                        ddlBCountry.Enabled = False
                    Else
                        ddlBankCode.Enabled = True
                        txtBankAcc.Enabled = True
                        txtBAddrLine1.Enabled = True
                        txtBAddrLine2.Enabled = True
                        txtBAddrLine3.Enabled = True
                        txtBCity.Enabled = True
                        ddlBState.Enabled = True
                        txtBPostcode.Enabled = True
                        ddlBCountry.Enabled = True
                    End If
                    txtBankName.Text = objParam.GetIPPValue(ddlPayMethod.SelectedValue & "_BANKCODE")
                Else
                    txtBankName.Text = objCompDetails.GetBankName(ddlBankCode.SelectedValue)
                End If

                If ddlPayMethod.SelectedValue = "IBG" Or ddlPayMethod.SelectedValue = "TT" Then
                    ddlBankCode.Enabled = True
                    txtBankAcc.Enabled = True
                    txtBAddrLine1.Enabled = True
                    txtBAddrLine2.Enabled = True
                    txtBAddrLine3.Enabled = True
                    txtBCity.Enabled = True
                    ddlBState.Enabled = True
                    txtBPostcode.Enabled = True
                    ddlBCountry.Enabled = True
                End If

                'Zulham 9/3/2015 IPP GST Stage 2B
                If ddlPayMethod.SelectedValue = "IBG" Or ddlPayMethod.SelectedValue = "RENTAS" Or ddlPayMethod.SelectedValue = "NOSTRO" Or ddlPayMethod.SelectedValue = "TT" Or ddlPayMethod.SelectedValue = "BC" Then
                    lblBankCharge.Visible = True
                    rbtnBankCharge.Visible = True
                    rbtnBankCharge.SelectedValue = "Y"
                    rbtnBankCharge.Enabled = True
                Else
                    lblBankCharge.Visible = False
                    rbtnBankCharge.Visible = False
                End If

                'Zulham 9/3/2015 IPP GST Stage 2B
                If ddlPayMethod.SelectedValue = "NOSTRO" Then
                    Me.txtBankAcc.Enabled = True
                    Me.txtBankAcc.ReadOnly = False
                    reqPayMethod.Enabled = False
                    reqBankCode.Enabled = False
                    reqBankAcc.Enabled = False
                    lblerr2.Text = ""
                    lblerr3.Text = ""
                End If

            End If
        Else
            ddlBankCode.SelectedIndex = 0
            ddlBankCode.Enabled = False
        End If

        If rbtnCoyType.SelectedValue = "V" Or rbtnCoyType.SelectedValue = "E" Then
            If ddlPayMethod.SelectedValue = "BC" Then
                lblerr3.Visible = False
                reqBankAcc.Enabled = False
            Else
                lblerr3.Visible = True
                reqBankAcc.Enabled = True
            End If
        End If

    End Sub

    Private Sub rbtnCoyType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtnCoyType.SelectedIndexChanged
        Dim objGlobal As New AppGlobals

        If rbtnCoyType.SelectedValue = "B" Then
            txtCoyCode.Enabled = True
            trCompType.Visible = True
            ddlConGLCode.Enabled = True
            ddlNonConGLCode.Enabled = True
            objGlobal.FillIBSGLCode(ddlConGLCode)
            objGlobal.FillIBSGLCode(ddlNonConGLCode)
            lblerr1.Visible = False
            lblerr2.Visible = False
            lblerr3.Visible = False
            reqPayMethod.Enabled = False
            reqBankCode.Enabled = False
            reqBankAcc.Enabled = False
            reqConIBSGLCode.Enabled = True
            reqCoyCode.Enabled = True

            ddlPayMethod.SelectedIndex = -1
            ddlBankCode.SelectedIndex = -1
            txtBankName.Text = ""
            ddlPayMethod.Enabled = False
            ddlBankCode.Enabled = False
            txtBAddrLine1.Enabled = False
            txtBAddrLine2.Enabled = False
            txtBAddrLine3.Enabled = False
            txtBCity.Enabled = False
            ddlBState.Enabled = False
            txtBPostcode.Enabled = False
            ddlBCountry.Enabled = False
            txtBankAcc.Enabled = False
            txtAddrLine1.Text = ""
            txtAddrLine2.Text = ""
            txtAddrLine3.Text = ""
            txtCity.Text = ""
            txtPostcode.Text = ""
            objGlobal.FillCodeTable(ddlCountry, CodeTable.Country)
            objGlobal.FillState(ddlState, ddlCountry.SelectedValue)
            'Zulham 26112018
            'lblCTerm.Visible = False
            'txtCterm.Visible = False
            txtCterm.Text = 30
            lblBankCharge.Visible = False
            rbtnBankCharge.Visible = False

            lblCurrency.Visible = True
            lblErrCurrency.Visible = True
            lblSemiCurrency.Visible = True
            lblNostro.Visible = False 'Modified for IPP Gst Stage 2A - CH
            rbtnNostro.Visible = False 'Modified for IPP Gst Stage 2A - CH
            ddlCurrency.Visible = True
            reqCurrency.Enabled = True

            ' ddlCurrency.SelectedValue = objDb.GetVal("SELECT cc_default_code FROM code_category WHERE cc_code = 'CU'")
            trIPP1.Style("display") = "none"
            trIPP2.Style("display") = "none"
            trIPPGST1.Style("display") = "none"
            trIPPGST2.Style("display") = "none"
            trIPPGST3.Style("display") = "none" 'Modified for IPP Gst Stage 2A - CH
            trIPPS4P2.Style("display") = "none" 'Added for IPP Stage 4 Phase 2 - Jules
            trIPP3.Style("display") = "none"
            trIPP4.Style("display") = "none"

            reqJobGrade.Enabled = False
            reqBC.Enabled = False
            reqCC.Enabled = False

            objGlobal.FillIPPBranchCode(ddlBC, True)
            objGlobal.FillIPPCostCentre(ddlCC, True)

            'Zulham 29062015 - HLB-IPP Stage 4 (CR)
            trEmployeeInputTaxCode.Visible = False
            trEmployeeOutputTaxCode.Visible = False

        ElseIf rbtnCoyType.SelectedValue = "E" Then
            'Zulham 27112018
            trCompType.Visible = True
            txtCoyCode.Enabled = True
            txtBankName.Enabled = False
            ddlConGLCode.SelectedIndex = -1
            ddlNonConGLCode.SelectedIndex = -1
            ddlConGLCode.Enabled = False
            ddlNonConGLCode.Enabled = False
            lblerr1.Visible = True
            lblerr2.Visible = True
            reqPayMethod.Enabled = True
            reqBankCode.Enabled = True
            If ddlBankCode.SelectedValue = "BC" Then
                lblerr3.Visible = False
                reqBankAcc.Enabled = False
            Else
                lblerr3.Visible = True
                reqBankAcc.Enabled = True
            End If

            'Zulham 14072018 - PAMB
            'Removed default value employee's default payment method
            'ddlPayMethod.SelectedValue = "CASA"
            'txtBankName.Text = objParam.GetIPPValue(ddlPayMethod.SelectedValue & "_BANKCODE")
            'objGlobal.FillBankCode(ddlBankCode, ddlPayMethod.SelectedValue)
            'ddlBankCode.SelectedIndex = 1
            'ddlPayMethod.Enabled = False
            'ddlBankCode.Enabled = False
            'End
            txtBAddrLine1.Enabled = False
            txtBAddrLine2.Enabled = False
            txtBAddrLine3.Enabled = False
            txtBCity.Enabled = False
            ddlBState.Enabled = False
            txtBPostcode.Enabled = False
            ddlBCountry.Enabled = False
            txtBankAcc.Enabled = True
            Dim objComps As New Companies
            Dim objComp As New Company
            objComp = objComps.GetCompanyDetails(Session("CompanyId"))
            txtAddrLine1.Text = objComp.Address1
            txtAddrLine2.Text = objComp.Address2
            txtAddrLine3.Text = objComp.Address3
            txtCity.Text = objComp.City
            txtPostcode.Text = objComp.PostCode
            objGlobal.FillState(ddlState, objComp.Country)
            ddlState.SelectedValue = objComp.State
            ddlCountry.SelectedValue = objComp.Country
            'Zulham 26112018
            'lblCTerm.Visible = False
            'txtCterm.Visible = False

            'lblCTerm.Visible = False
            'txtCterm.Visible = False
            txtCterm.Text = 30
            lblBankCharge.Visible = False
            rbtnBankCharge.Visible = False
            txtCoyCode.Text = ""

            lblCurrency.Visible = False
            lblErrCurrency.Visible = False
            lblSemiCurrency.Visible = False
            ddlCurrency.Visible = False
            reqCurrency.Enabled = False
            lblNostro.Visible = False 'Modified for IPP Gst Stage 2A - CH
            rbtnNostro.Visible = False 'Modified for IPP Gst Stage 2A - CH

            trIPP1.Style("display") = "none"
            trIPP2.Style("display") = "none"
            trIPPGST1.Style("display") = "none"
            trIPPGST2.Style("display") = "none"
            trIPPGST3.Style("display") = "none" 'Modified for IPP Gst Stage 2A - CH
            trIPPS4P2.Style("display") = "none" 'Added for IPP Stage 4 Phase 2 - Jules
            trIPP3.Style("display") = ""
            trIPP4.Style("display") = ""
            reqJobGrade.Enabled = True
            reqBC.Enabled = True
            reqCC.Enabled = True

            'Zulham 29062015 - HLB-IPP Stage 4 (CR)
            trIPP1.Style("display") = "none"
            trIPP2.Style("display") = "none"
            trIPPGST1.Style("display") = "none"
            trIPPGST2.Style("display") = "none"
            trEmployeeInputTaxCode.Visible = True
            trEmployeeOutputTaxCode.Visible = True
            objGlobal.FillTaxCode(ddlEmpGstInputTaxCode, , "P", "IPP")
            objGlobal.FillTaxCode(ddlEmpGstOutputTaxCode, , "S", "IPP")
            'Zulham 14072018 - PAMB
            'Zulham 27072015 - HLB-IPP Stage 4(CR)
            'For no.2, can we default the GST Input Tax code to TX1 and the GST Output Tax Code to SR1 
            'when the employee is setting up in the system?
            'ddlEmpGstInputTaxCode.SelectedValue = "TX1"
            'ddlEmpGstOutputTaxCode.SelectedValue = "SR1"
            'End
        Else

            'Zulham 29062015 - HLB-IPP Stage 4 (CR)
            trEmployeeInputTaxCode.Visible = False
            trEmployeeOutputTaxCode.Visible = False
            'Zulham 27112018
            'trCompType.Visible = False
            ddlConGLCode.SelectedIndex = -1
            ddlNonConGLCode.SelectedIndex = -1
            ddlConGLCode.Enabled = False
            ddlNonConGLCode.Enabled = False
            lblerr1.Visible = True
            lblerr2.Visible = True
            reqPayMethod.Enabled = True
            reqBankCode.Enabled = True
            If ddlBankCode.SelectedValue = "BC" Then
                lblerr3.Visible = False
                reqBankAcc.Enabled = False
            Else
                lblerr3.Visible = True
                reqBankAcc.Enabled = True
            End If

            ddlPayMethod.Enabled = True
            ddlBankCode.Enabled = False
            txtBankName.Enabled = False
            txtBAddrLine1.Enabled = True
            txtBAddrLine2.Enabled = True
            txtBAddrLine3.Enabled = True
            txtBCity.Enabled = True
            ddlBState.Enabled = True
            txtBPostcode.Enabled = True
            ddlBCountry.Enabled = True
            txtBankAcc.Enabled = True
            ddlPayMethod.SelectedIndex = -1
            ddlBankCode.SelectedIndex = -1
            txtBankName.Text = ""
            objGlobal.FillIPPPaymentMethod(ddlPayMethod)
            txtAddrLine1.Text = ""
            txtAddrLine2.Text = ""
            txtAddrLine3.Text = ""
            txtCity.Text = ""
            txtPostcode.Text = ""
            objGlobal.FillCodeTable(ddlCountry, CodeTable.Country)
            objGlobal.FillState(ddlState, ddlCountry.SelectedValue)
            txtCoyCode.Text = ""
            lblCTerm.Visible = True
            txtCterm.Visible = True
            If ddlPayMethod.SelectedValue = "IBG" Or ddlPayMethod.SelectedValue = "TT" Or ddlPayMethod.SelectedValue = "RENTAS" Then
                lblBankCharge.Visible = True
                rbtnBankCharge.Visible = True
            Else
                lblBankCharge.Visible = False
                rbtnBankCharge.Visible = False
            End If

            lblCurrency.Visible = False
            lblErrCurrency.Visible = False
            lblSemiCurrency.Visible = False
            ddlCurrency.Visible = False
            reqCurrency.Enabled = False
            'Zulham 25032015 IPP GST Enhancement
            'lblNostro.Visible = True 'Modified for IPP Gst Stage 2A - CH
            'rbtnNostro.Visible = True 'Modified for IPP Gst Stage 2A - CH

            trIPP1.Style("display") = ""
            trIPP2.Style("display") = ""
            trIPPGST1.Style("display") = ""
            trIPPGST2.Style("display") = ""
            trIPPGST3.Style("display") = "" 'Modified for IPP Gst Stage 2A - CH
            trIPPS4P2.Style("display") = "" 'Added for IPP Stage 4 Phase 2 - Jules
            trIPP3.Style("display") = "none"
            trIPP4.Style("display") = "none"
            reqJobGrade.Enabled = False
            reqBC.Enabled = False
            reqCC.Enabled = False
            objGlobal.FillIPPCompanyCategory(ddlCategory)
            rbtnResidentType.SelectedValue = "Y"
            objGlobal.FillCodeTable(ddlResidentCountry, CodeTable.Country, , , False)
            ddlResidentCountry.Enabled = False
        End If
    End Sub

    Private Sub ddlBankCode_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlBankCode.SelectedIndexChanged
        Dim objCompDetails As New Companies
        Dim strBankName As String

        If ddlBankCode.SelectedIndex <> 0 Then
            strBankName = objCompDetails.GetBankName(ddlBankCode.SelectedValue)
            txtBankName.Text = strBankName
        Else
            txtBankName.Text = ""
            txtBankName.Enabled = False
        End If

    End Sub

    Private Sub ddlCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCountry.SelectedIndexChanged
        Dim objGlobal As New AppGlobals
        objGlobal.FillState(ddlState, ddlCountry.SelectedItem.Value)
    End Sub

    Private Sub ddlBCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlBCountry.SelectedIndexChanged
        Dim objGlobal As New AppGlobals
        objGlobal.FillState(ddlBState, ddlBCountry.SelectedItem.Value)
    End Sub

    Private Sub hidbtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hidbtn.Click
        Dim objComp As New Companies
        Dim objGlobal As New AppGlobals
        Dim strExist As String

        If rbtnCoyType.SelectedValue <> "E" Then
            strExist = objComp.check_ipp_coyname(Trim(txtCoyName.Text), "", rbtnCoyType.SelectedValue)

            If strExist = "False" Then
                Common.NetMsgbox(Me, "Company name already exist!", MsgBoxStyle.Information)
            End If

        End If

    End Sub

    Private Sub hidbtn2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hidbtn2.Click
        Dim objComp As New Companies
        Dim objGlobal As New AppGlobals
        Dim strExist As String

        If rbtnCoyType.SelectedValue = "E" Then
            strExist = objComp.check_ipp_coyname(Trim(txtCoyName.Text), txtRegNo.Text, rbtnCoyType.SelectedValue)

            If strExist = "False" Then
                Common.NetMsgbox(Me, "Staff already exist!", MsgBoxStyle.Information)
            End If
        End If
    End Sub

    Private Sub hidbtn3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hidbtn3.Click
        displayTaxCode()
    End Sub

    Private Sub displayTaxCode()
        Dim objGlobal As New AppGlobals
        Dim cbolist As New ListItem
        Dim objGst As New GST
        Dim strReverseChargeInput, strReverseChargeOutput As String

        If txtGstRegNo.Text = "" Then
            If ddlGstInputTaxCode.Enabled = True Then
                cbolist.Value = "N/A"
                cbolist.Text = "N/A"

                If rbtnResidentType.SelectedValue = "Y" Then
                    If objGst.chkExistTaxCode("NR", "P", "IPP") = False Then
                        ddlGstInputTaxCode.Items.Clear()
                        ddlGstInputTaxCode.Items.Insert(0, cbolist)
                    Else
                        objGlobal.FillTaxCode(ddlGstInputTaxCode, , "P", "IPP")
                        ddlGstInputTaxCode.SelectedValue = "NR"
                    End If

                    ddlGstOutputTaxCode.Items.Clear()
                    ddlGstOutputTaxCode.Items.Insert(0, cbolist)
                    ddlGstOutputTaxCode.Enabled = False
                    ddlGstInputTaxCode.Enabled = False
                Else
                    objIPPMain.getReverseCharge(strReverseChargeInput, strReverseChargeOutput)
                    If strReverseChargeInput = ddlGstInputTaxCode.SelectedValue Then
                        ddlGstOutputTaxCode.SelectedValue = strReverseChargeOutput
                    Else
                        ddlGstOutputTaxCode.Items.Clear()
                        ddlGstOutputTaxCode.Items.Insert(0, cbolist)
                    End If
                    ddlGstOutputTaxCode.Enabled = False
                End If
            End If

            'Jules 2018.10.29
            txtSystemValDate.Text = ""
            txtGstRegDate.Text = ""
            'End modification.
        Else
            If hidTxtPrevGst.Text = "" Then
                If ddlGstInputTaxCode.Enabled = False Then
                    ddlGstInputTaxCode.Enabled = True
                    objGlobal.FillTaxCode(ddlGstInputTaxCode, , "P", "IPP")

                    If rbtnResidentType.SelectedValue = "Y" Then
                        'Jules 2018.10.29 - Default to ST0
                        'Common.SelDdl("TX1", ddlGstInputTaxCode, True)
                        Common.SelDdl("ST0", ddlGstInputTaxCode, True)
                        'End modification.
                    Else
                        Common.SelDdl("IM1", ddlGstInputTaxCode, True)
                    End If
                End If

                If rbtnCoyType.SelectedValue = "V" And rbtnResidentType.SelectedValue = "N" And ddlGstOutputTaxCode.Enabled = False Then
                    If ddlGstOutputTaxCode.SelectedValue = "N/A" Then
                        objGlobal.FillTaxCode(ddlGstOutputTaxCode, , "S", "IPP")
                    End If
                    ddlGstOutputTaxCode.Enabled = True
                End If
            End If

            'Jules 2018.10.29 - Set dates.
            txtSystemValDate.Text = Format(Now, "dd/MM/yyyy")
            txtGstRegDate.Text = Format(Now, "dd/MM/yyyy")
            'End modification.
        End If

        hidTxtPrevGst.Text = txtGstRegNo.Text
    End Sub

    Private Sub rbtnStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnStatus.SelectedIndexChanged
        If rbtnStatus.SelectedValue = "A" Then
            divInactiveReason.Visible = False
            txtInactiveReason.Text = ""
        Else
            divInactiveReason.Visible = True
        End If
    End Sub

    Private Sub rbtnResidentType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnResidentType.SelectedIndexChanged
        Dim objGlobal As New AppGlobals
        Dim objCompDetails As New Companies
        Dim cbolist As New ListItem
        Dim objGst As New GST

        displayTaxCode()
        If rbtnResidentType.SelectedValue = "Y" Then
            objGlobal.FillCodeTable(ddlResidentCountry, CodeTable.Country, , , False)
            ddlResidentCountry.Enabled = False
            ddlPayMethod.Enabled = True

            'If objGst.chkExistTaxCode("NR", "P", "IPP") = False Then
            '    cbolist.Value = "N/A"
            '    cbolist.Text = "N/A"
            '    ddlGstInputTaxCode.Items.Clear()
            '    ddlGstInputTaxCode.Items.Insert(0, cbolist)
            'Else
            '    objGlobal.FillTaxCode(ddlGstInputTaxCode, , "P", "IPP")
            '    ddlGstInputTaxCode.SelectedValue = "NR"
            'End If

            If txtGstRegNo.Text <> "" Then
                'Jules 2018.10.29 - Default to ST0.
                'Common.SelDdl("TX1", ddlGstInputTaxCode, True)
                Common.SelDdl("ST0", ddlGstInputTaxCode, True)
                'End modification.
            End If

            cbolist.Value = "N/A"
            cbolist.Text = "N/A"
            ddlGstOutputTaxCode.Items.Clear()
            ddlGstOutputTaxCode.Items.Insert(0, cbolist)
            ddlGstOutputTaxCode.Enabled = False
        Else
            If ddlPayMethod.SelectedValue <> "TT" Then
                ddlPayMethod.SelectedValue = "TT"
                objGlobal.FillBankCode(ddlBankCode, ddlPayMethod.SelectedValue)
                If ddlBankCode.SelectedValue <> "n.a." Then
                    ddlBankCode.SelectedIndex = 1
                    txtBankName.Text = objCompDetails.GetBankName(ddlBankCode.SelectedValue)
                End If
            End If

            Common.SelDdl("IM1", ddlGstInputTaxCode, True)

            ddlResidentCountry.Enabled = True

            'Zulham 25032015 Case 8603
            'ddlPayMethod.Enabled = False

            Me.txtBankAcc.Attributes.Add("onkeypress", "return isNumberCharKey(event);")
            Me.txtBankAcc.Attributes.Add("onblur", "return isValidCheck2();")
            ddlBankCode.Enabled = True
            txtBankAcc.Enabled = True
            txtBAddrLine1.Enabled = True
            txtBAddrLine2.Enabled = True
            txtBAddrLine3.Enabled = True
            txtBCity.Enabled = True
            ddlBState.Enabled = True
            txtBPostcode.Enabled = True
            ddlBCountry.Enabled = True

            lblBankCharge.Visible = True
            rbtnBankCharge.Visible = True
            rbtnBankCharge.SelectedValue = "Y"
            rbtnBankCharge.Enabled = True

            lblerr3.Visible = True
            reqBankAcc.Enabled = True
            'If ViewState("Mode") = "Add" Then
            '    ddlPayMethod_SelectedIndexChanged(sender, e)
            'End If
            ddlGstInputTaxCode.Enabled = True
            If txtGstRegNo.Text <> "" Then
                ddlGstOutputTaxCode.Enabled = True
                objGlobal.FillTaxCode(ddlGstOutputTaxCode, , "S", "IPP")
            Else
                cbolist.Value = "N/A"
                cbolist.Text = "N/A"
                ddlGstOutputTaxCode.Items.Clear()
                ddlGstOutputTaxCode.Items.Insert(0, cbolist)
                ddlGstOutputTaxCode.Enabled = False
            End If
        End If
    End Sub
    'Zulham 10072018 - PAMB
    'Not needed by PAMB
    'Private Sub cmdValidate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdValidate.Click
    '    'Yap: 2015-Feb-02: GST IPP Stage 2A
    '    Dim strResponse As String = ""
    '    Dim objSOAP As New IPPMain

    '    Dim strsql, strUserName, strPassword As String
    '    Dim intSeqNum As Integer = 0
    '    Dim intTranNum As Integer = 0

    '    strsql = "SELECT IFNULL(MAX(iwl_journal_seq),0) FROM IPP_WEBSERVTRANS_LOG WHERE CAST(iwl_request_date AS DATE) = DATE(NOW());"
    '    intSeqNum = CInt(objDb.GetVal(strsql))

    '    strsql = "SELECT IFNULL(COUNT(iwl_journal_seq),0) FROM IPP_WEBSERVTRANS_LOG WHERE CAST(iwl_request_date AS DATE) = DATE(NOW());"
    '    intTranNum = CInt(objDb.GetVal(strsql))

    '    'If intSeqNum <> 0 Then
    '    '    intSeqNum += 1
    '    'End If
    '    intSeqNum += 1
    '    intTranNum += 1

    '    'strUserName = "A1B6D01C30C2A8ACE3DC2ED758EEC0A2CAE3902FCD3FBCF0E0EA5BE9F64DE12A"
    '    'strPassword = "162E6054D2B7A959CE9CBADE01CC7D4C94D1A47F5149176F7138B026AAD44151"
    '    strUserName = System.Configuration.ConfigurationManager.AppSettings("strUserName")
    '    strPassword = System.Configuration.ConfigurationManager.AppSettings("strPassword")

    '    Dim XMLInit = objSOAP.GenXMLInit(intSeqNum, intTranNum, txtRegNo.Text, strUserName, strPassword)
    '    objSOAP.InsertWebLog(intSeqNum, txtRegNo.Text)
    '    strResponse = objSOAP.SendSoap(XMLInit, intSeqNum)

    '    Dim strResponseCode As String()
    '    'Dim strGSTName As String()
    '    Dim strGSTRegDate As String()
    '    Dim strErrorMsg As String()
    '    Dim strGSTNo As String()

    '    'Jules 2015.08.21 - commented out for IPP Stage 4 Phase 2.
    '    'If Trim(txtGstRegDate.Text) = "" Then
    '    '    Common.NetMsgbox(Me, "Invalid GST Registration Date!", MsgBoxStyle.Information)
    '    '    Exit Sub
    '    'End If

    '    txtSystemValDate.Text = DateTime.Now.ToString("dd/MM/yyyy")

    '    If strResponse = "ERROR" Then
    '        Common.NetMsgbox(Me, "Invalid Business Registration No.!", MsgBoxStyle.Information)
    '    Else
    '        strResponseCode = Split(strResponse, "<ResponseCode>")
    '        strResponseCode = Split(strResponseCode(1), "</ResponseCode>")
    '        objSOAP.UpdateWebLog(2, txtRegNo.Text, strResponseCode(0), "")
    '        If strResponseCode(0) = "GD" Then
    '            objSOAP.UpdateWebLog(2, txtRegNo.Text, strResponseCode(0), "")
    '            strGSTNo = Split(strResponse, "<Account>")
    '            strGSTNo = Split(strGSTNo(1), "</Account>")
    '            If strGSTNo(0) = txtGstRegNo.Text Then

    '                'strGSTName = Split(strResponse, "<ws:Name>")
    '                strGSTRegDate = Split(strResponse, "<EntryDate8>")
    '                strGSTRegDate = Split(strGSTRegDate(1), "</EntryDate8>")

    '                'txtCoyName.Text = strGSTName(0)
    '                If strGSTRegDate(0) <> "" Then
    '                    txtGstRegDate.Text = Format(CDate(Left(strGSTRegDate(0), 4) & "-" & Right(Left(strGSTRegDate(0), 6), 2) & "-" & Right(strGSTRegDate(0), 2)), "dd/MM/yyyy")
    '                End If

    '                Common.NetMsgbox(Me, "Valid GST Registration No.!", MsgBoxStyle.Information)
    '            Else
    '                'Jules 2015.08.13 - IPP Stage 4 Phase 2
    '                'Common.NetMsgbox(Me, "Invalid GST Registration No.!", MsgBoxStyle.Information)  

    '                strGSTRegDate = Split(strResponse, "<EntryDate8>")
    '                strGSTRegDate = Split(strGSTRegDate(1), "</EntryDate8>")

    '                If strGSTRegDate(0) <> "" Then
    '                    txtGstRegDate.Text = Format(CDate(Left(strGSTRegDate(0), 4) & "-" & Right(Left(strGSTRegDate(0), 6), 2) & "-" & Right(strGSTRegDate(0), 2)), "dd/MM/yyyy")
    '                End If

    '                ViewState("validate") = strGSTNo(0)

    '                Dim strscript1 As New System.Text.StringBuilder
    '                strscript1.Append("<script language=""javascript"">")
    '                strscript1.Append("confirmUpdate();")
    '                strscript1.Append("</script>")
    '                RegisterStartupScript("script1", strscript1.ToString())                    
    '                'End
    '            End If
    '        Else
    '            Common.NetMsgbox(Me, "Invalid Business Registration No.!", MsgBoxStyle.Information)
    '        End If
    '    End If
    'End Sub

    Private Sub ddlGstInputTaxCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlGstInputTaxCode.SelectedIndexChanged
        Dim strReverseChargeInput, strReverseChargeOutput As String
        Dim objGlobal As New AppGlobals
        Dim cbolist As New ListItem

        objIPPMain.getReverseCharge(strReverseChargeInput, strReverseChargeOutput)
        If ddlGstInputTaxCode.SelectedValue <> "" And ddlGstInputTaxCode.SelectedValue = strReverseChargeInput Then
            If strReverseChargeOutput <> "" Then
                objGlobal.FillTaxCode(ddlGstOutputTaxCode, , "S", "IPP")
                ddlGstOutputTaxCode.SelectedValue = strReverseChargeOutput
            End If
        Else
            If ddlGstOutputTaxCode.Enabled = False Then
                cbolist.Value = "N/A"
                cbolist.Text = "N/A"
                ddlGstOutputTaxCode.Items.Clear()
                ddlGstOutputTaxCode.Items.Insert(0, cbolist)
            End If
        End If
    End Sub

    Private Sub ChkUserAccess()
        'Chee Hong - 12/02/2015 - Only allow HLB IPP Admin for action (IPP GST Stage 2A)
        If strDefIPPCompID <> "" And strDefIPPCompID <> Session("CompanyId") Then
            cmdSave.Visible = False
            cmdAdd.Visible = False
            'cmdValidate.Visible = False 'Zulham 10072018 - PAMB
            rbtnStatus.Enabled = False
            rbtnResidentType.Enabled = False
            rbtnNostro.Enabled = False
            rbtnBankCharge.Enabled = False
        End If
    End Sub

    Private Sub HideLinkBt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HideLinkBt.Click
          
        If hidUpdate.Value = "1" Then
            If rbtnCoyType.SelectedValue = "V" Then
                Dim objComp As New Companies
                Dim objCompDetails As New Company
                Dim objGlobal As New AppGlobals
                Dim strMsg As String
                Dim intMsg, strInd As Integer

                If ViewState("Mode") = "Add" Then
                    strInd = 0
                    txtGstRegNo.Text = Trim(ViewState("validate"))
                Else
                    strInd = ViewState("VendorIndex")
                End If

                Try
                    objCompDetails = GetInput()
                    objCompDetails.TaxRegNo = Trim(ViewState("validate"))

                    intMsg = objComp.UpdateIPPCompanyGSTRegNo(objCompDetails, strInd)

                    If intMsg = -99 Then
                        Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00018"), MsgBoxStyle.Information)
                    ElseIf intMsg = WheelMsgNum.Save Then
                        Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00003"), MsgBoxStyle.Information)
                        Me.txtGstRegNo.Text = Trim(ViewState("validate"))
                        displayTaxCode() 'Jules 2015.09.29
                    ElseIf intMsg = WheelMsgNum.NotSave Then
                        Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00007"), MsgBoxStyle.Information)
                    End If
                Catch ex As Exception
                    Common.NetMsgbox(Me, ex.Message)
                End Try
            End If
        End If
    End Sub
End Class