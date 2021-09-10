Imports AgoraLegacy
Imports eProcure.Component

Public Class BComVendor
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents cmd_save As System.Web.UI.WebControls.Button
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents txt_QuoPre As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_QuoLun As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_DoPre As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_DoLun As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_CSRPre As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_CSRLun As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_InvPre As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_InvLun As System.Web.UI.WebControls.TextBox
    'Jules 2015.02.02 Agora Stage 2
    Protected WithEvents txt_DebitPre As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_DebitLun As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_CreditPre As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_CreditLun As System.Web.UI.WebControls.TextBox

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    Protected WithEvents revQuoLun As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents revDoLun As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents revCSRLun As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents revInvLun As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents cmd_Reset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents revQuoPre As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents revDoPre As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents revCSRPre As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents revInvPre As System.Web.UI.WebControls.RegularExpressionValidator
    'Jules 2015.02.02 Agora Stage 2
    Protected WithEvents revDebitPre As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents revCreditPre As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents hidresult As System.Web.UI.HtmlControls.HtmlInputHidden
    Dim clsAdmin As New Admin


    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        cmd_save.Enabled = False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmd_save)
        htPageAccess.Add("add", alButtonList)
        htPageAccess.Add("update", alButtonList)
        CheckButtonAccess()
        'cmd_Reset.Enabled = blnCanAdd Or blnCanUpdate Or blnCanDelete
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        lblHeader.Text = "&nbsp;Document Prefix And Last Used Number"
        If Not IsPostBack Then
            Dim objGlobal As New AppGlobals
            'objGlobal.FillCodeTable(cbo_PayTerm, CodeTable.PaymentTerm)
            'objGlobal.FillCodeTable(cbo_PayMeth, CodeTable.PaymentMethod)
            displayParam()
            ViewState("Side") = Request.Params("side")
            GenerateTab()
        End If
        ' aichu modify on 09/09/2005
        ' not allowed space for 1st character for Prefix
        ' previous validation expression = ^[^']*$; changed to ^[^ '][^']*$
        Me.txt_CSRPre.Attributes.Add("onblur", "resetValue('" & txt_CSRPre.ClientID & "','');")
        Me.txt_DoPre.Attributes.Add("onblur", "resetValue('" & txt_DoPre.ClientID & "','');")
        Me.txt_InvPre.Attributes.Add("onblur", "resetValue('" & txt_InvPre.ClientID & "','');")
        Me.txt_QuoPre.Attributes.Add("onblur", "resetValue('" & txt_QuoPre.ClientID & "','');")
        'Jules 2015.02.02 Agora Stage 2
        Me.txt_DebitPre.Attributes.Add("onblur", "resetValue('" & txt_DebitPre.ClientID & "','');")
        Me.txt_CreditPre.Attributes.Add("onblur", "resetValue('" & txt_CreditPre.ClientID & "','');")

        Dim strMsg As String = "Changing document prefixes may affect Buyer Company interface files.  Please click Cancel to abort Or click OK to proceed."
        Me.cmd_save.Attributes.Add("onClick", "return confirmChanged('" & strMsg & "');")

    End Sub

    Private Function validateInput() As Boolean
        If Not Me.txt_QuoPre.Text = "" AndAlso Not IsValidAlphaNumericInput(Trim(txt_QuoPre.Text)) Then
            Common.NetMsgbox(Me, "Please enter valid characters for your Quotation Prefix.", MsgBoxStyle.Information)
            Return False
        End If
        If Not Me.txt_DoPre.Text = "" AndAlso Not IsValidAlphaNumericInput(Trim(txt_DoPre.Text)) Then
            Common.NetMsgbox(Me, "Please enter valid characters for your DO Prefix.", MsgBoxStyle.Information)
            Return False
        End If
        If Not Me.txt_CSRPre.Text = "" AndAlso Not IsValidAlphaNumericInput(Trim(txt_CSRPre.Text)) Then
            Common.NetMsgbox(Me, "Please enter valid characters for your CSR Prefix.", MsgBoxStyle.Information)
            Return False
        End If
        If Not Me.txt_InvPre.Text = "" AndAlso Not IsValidAlphaNumericInput(Trim(txt_InvPre.Text)) Then
            Common.NetMsgbox(Me, "Please enter valid characters for your Invoice Prefix.", MsgBoxStyle.Information)
            Return False
        End If
        'Jules 2015.02.02 Agora Stage 2
        If Not Me.txt_DebitPre.Text = "" AndAlso Not IsValidAlphaNumericInput(Trim(txt_DebitPre.Text)) Then
            Common.NetMsgbox(Me, "Please enter valid characters for your Debit Note / Debit Advice Prefix.", MsgBoxStyle.Information)
            Return False
        End If
        If Not Me.txt_CreditPre.Text = "" AndAlso Not IsValidAlphaNumericInput(Trim(txt_CreditPre.Text)) Then
            Common.NetMsgbox(Me, "Please enter valid characters for your Credit Note / Credit Advice Prefix.", MsgBoxStyle.Information)
            Return False
        End If
        
        Return True
    End Function

    Private Sub cmd_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_save.Click

        If Not validateInput() Then Exit Sub

        Dim htList As New Hashtable
        Dim strparamname As String
        Dim strparamvalue As String
        Dim strparamtype As String
        Dim strMsg As String

        htList.Add("QuotationPrefix", txt_QuoPre.Text)
        htList.Add("DOPrefix", txt_DoPre.Text)
        htList.Add("CSRPrefix", txt_CSRPre.Text)
        htList.Add("InvoicePrefix", txt_InvPre.Text)
        'Jules 2015.02.02 Agora Stage 2
        htList.Add("DN_EPROCPrefix", txt_DebitPre.Text)
        htList.Add("CN_EPROCPrefix", txt_CreditPre.Text)
        'htList.Add("InvPrefix", txt_SrnPre.Text)
        'htList.Add("PaymentPrefix", txt_PayPre.Text)
        'htList.Add("TenderPrefix", txt_TenPre.Text)
        'htList.Add("Debit NotePrefix", txt_DebitPre.Text)

        htList.Add("QuotationLastUsedNo", txt_QuoLun.Text)
        htList.Add("DOLastUsedNo", txt_DoLun.Text)
        htList.Add("CSRLastUsedNo", txt_CSRLun.Text)
        htList.Add("InvoiceLastUsedNo", txt_InvLun.Text)
        'Jules 2015.02.02 Agora Stage 2
        htList.Add("DN_EPROCLastUsedNo", txt_DebitLun.Text)
        htList.Add("CN_EPROCLastUsedNo", txt_CreditLun.Text)
        'htList.Add("SrnLastUsedNo", txt_SrnLun.Text)
        'htList.Add("PaymentLastUsedNo", txt_PayLun.Text)
        'htList.Add("TenderLastUsedNo", txt_TenLun.Text)
        'htList.Add("Debit NoteLastUsedNo", txt_DebitLun.Text


        
        'If hidresult.Value = "1" Then

        clsAdmin.updatevenparam(htList)
        Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)

        displayParam()
        ' End If

        'displayParamMaster()
    End Sub

    Private Sub displayParam()
        Dim dsParam As New DataSet
        Dim intCount As Integer
        Dim i As Integer
        dsParam = clsAdmin.getvenparam
        intCount = dsParam.Tables(0).Rows.Count()
        For i = 0 To intCount - 1
            If UCase(dsParam.Tables(0).Rows(i)(0)) = "PREFIX" Then
                Select Case dsParam.Tables(0).Rows(i)(2)
                    Case "Quotation"
                        txt_QuoPre.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    Case "DO"
                        txt_DoPre.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    Case "CSR"
                        txt_CSRPre.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    Case "Invoice"
                        txt_InvPre.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                        'Case "Payment"
                        '    txt_PayPre.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                        'Case "Tender"
                        '    txt_TenPre.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                        'Case "SRN"
                        '    txt_SrnPre.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                        'Case "Debit Note"
                        '    txt_DebitPre.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))

                        'Jules 2015.02.02 Agora Stage 2
                        '09/07/2015 - Stage 2 Issue - CH - Change DN to DN_EPROC
                    Case "DN_EPROC"
                        txt_DebitPre.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                        '09/07/2015 - Stage 2 Issue - CH - Change CN to CN_EPROC
                    Case "CN_EPROC"
                        txt_CreditPre.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                End Select
            Else
                Select Case dsParam.Tables(0).Rows(i)(2)
                    Case "Quotation"
                        txt_QuoLun.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    Case "DO"
                        txt_DoLun.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    Case "CSR"
                        txt_CSRLun.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    Case "Invoice"
                        txt_InvLun.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                        'Case "Payment"
                        '    txt_PayLun.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                        'Case "Tender"
                        '    txt_TenLun.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                        'Case "SRN"
                        '    txt_SrnLun.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                        'Case "Debit Note"
                        '    txt_DebitLun.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))

                        'Jules 2015.02.02 Agora Stage 2
                        '09/07/2015 - Stage 2 Issue - CH - Change DN to DN_EPROC
                    Case "DN_EPROC"
                        txt_DebitLun.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                        '09/07/2015 - Stage 2 Issue - CH - Change CN to CN_EPROC
                    Case "CN_EPROC"
                        txt_CreditLun.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                End Select
            End If
        Next

        'Dim objcall As New Admin()
        'Dim dsparamcoy As DataSet
        'dsparamcoy = objcall.getParamMaster
        'If dsParam.Tables(1).Rows.Count > 0 Then
        '    opl_RfqOp.SelectedValue = dsParam.Tables(1).Rows(0)("CM_RFQ_OPTION")
        '    'cbo_PayTerm.SelectedItem.Value = dsparamcoy.Tables(0).Rows(0)("CM_PAYMENT_TERM")
        '    'cbo_PayMeth.SelectedItem.value = dsparamcoy.Tables(0).Rows(0)("CM_PAYMENT_METHOD")

        '    Common.SelDdl(Common.parseNull(dsParam.Tables(1).Rows(0)("CM_PAYMENT_TERM")), cbo_PayTerm, True, True)
        '    Common.SelDdl(Common.parseNull(dsParam.Tables(1).Rows(0)("CM_PAYMENT_METHOD")), cbo_PayMeth, True, True)
        '    txt_PwdDur.Text = dsParam.Tables(1).Rows(0)("CM_PWD_DURATION")
        'End If

        'Dim intcount As Integer
        'Dim i As Integer
        'intCount = dsParam.Tables(2).Rows.Count
        'For i = 0 To intCount - 1
        '    If UCase(dsParam.Tables(2).Rows(i)("CS_FLAG_NAME")) = "ALLOW FREE FORM BILLING ADDRESS" Then
        '        If dsParam.Tables(2).Rows(i)("CS_FLAG_VALUE") = 1 Then
        '            ChkFreeForm.Checked = True
        '        Else
        '            ChkFreeForm.Checked = False
        '        End If

        '    ElseIf UCase(dsParam.Tables(2).Rows(i)("CS_FLAG_NAME")) = "CONSOLIDATION REQUIRED" Then
        '        If dsParam.Tables(2).Rows(i)("CS_FLAG_VALUE") = 1 Then
        '            ChkConsolidation.Checked = True
        '        Else
        '            ChkConsolidation.Checked = False
        '        End If

        '    ElseIf UCase(dsParam.Tables(2).Rows(i)("CS_FLAG_NAME")) = "2 LEVEL RECEIVING" Then
        '        If dsParam.Tables(2).Rows(i)("CS_FLAG_VALUE") = 1 Then
        '            ChkLevelsRec.Checked = True
        '        Else
        '            ChkLevelsRec.Checked = False
        '        End If


        '    End If
        'put displayparammaster 
        'Next
    End Sub
    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        Session("w_CompVParam_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "coCompanyDetail.aspx", "side=VENDOR&mode=modify&pageid=" & strPageId) & """><span>Company Details</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Admin", "BComVendor.aspx", "side=VENDOR&pageid=" & strPageId) & """><span>Parameters</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "SalesInfo.aspx", "side=VENDOR&pageid=" & strPageId) & """><span>Sales Info</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "SoftwareApp.aspx", "side=VENDOR&pageid=" & strPageId) & """><span>Software</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                              "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "QualityStd.aspx", "side=VENDOR&pageid=" & strPageId) & """><span>Quality Standards</span></a></li>" & _
                              "<li><div class=""space""></div></li>" & _
                          "</ul><div></div></div>"



        'Session("w_CompVParam_tabs") = "<div class=""t_entity""><ul>" & _
        '"<li><div class=""space""></div></li>" & _
        '                     "<li><a class=""t_entity_btn"" href=""../Companies/coCompanyDetail.aspx?side=VENDOR&mode=modify&pageid=" & strPageId & """><span>Company Details</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                     "<li><a class=""t_entity_btn_selected"" href=""../Admin/BComVendor.aspx?side=VENDORpageid=" & strPageId & """><span>Parameters</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                        "<li><a class=""t_entity_btn"" href=""../Companies/SoftwareApp.aspx?side=VENDORpageid=" & strPageId & """><span>Software</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                      "<li><a class=""t_entity_btn"" href=""../Companies/SalesInfo.aspx?side=VENDORpageid=" & strPageId & """><span>Sales Info</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                      "<li><a class=""t_entity_btn"" href=""../Companies/QualityStd.aspx?side=VENDORpageid=" & strPageId & """><span>Quality Standards</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                   "</ul><div></div></div>"
    End Sub
End Class
