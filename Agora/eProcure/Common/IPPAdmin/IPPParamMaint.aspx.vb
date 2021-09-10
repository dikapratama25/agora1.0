Imports AgoraLegacy
Imports eProcure.Component

Public Class IPPParamMaint
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objGlobal As New AppGlobals



#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents lblHeader2 As System.Web.UI.WebControls.Label
    'Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents cmd_save As System.Web.UI.WebControls.Button
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents txt_LateSubmit As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_BCCASA As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_BCCheque As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_CASALimit As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_IBGLimit As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_BCIBG As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_BCRENTAS As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_DEFAULTDUEDATE As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_BCBC As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_BCTT1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_BCTT2 As System.Web.UI.WebControls.TextBox

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    Protected WithEvents revLateSubmit As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents revBCCASA As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents revBCCheque As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents revCASALimit As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents cmd_Reset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents revIBGLimit As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents revBCIBG As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents revBCRENTAS As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents hidresult As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected WithEvents lblLateSubmit As System.Web.UI.WebControls.Label
    Protected WithEvents lblBankCodeCASA As System.Web.UI.WebControls.Label
    Protected WithEvents lblBankCodeCheque As System.Web.UI.WebControls.Label
    Protected WithEvents lblCASALimit As System.Web.UI.WebControls.Label
    Protected WithEvents lblIBGLimit As System.Web.UI.WebControls.Label
    Protected WithEvents lblBankChargeIBG As System.Web.UI.WebControls.Label
    Protected WithEvents lblBankChargeRENTAS As System.Web.UI.WebControls.Label
    Protected WithEvents lblDefaultDueDate As System.Web.UI.WebControls.Label
	Protected WithEvents lblBankChargeBC As System.Web.UI.WebControls.Label
    Protected WithEvents lblBankChargeTT1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblBankChargeTT2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblGSTInputTC As System.Web.UI.WebControls.Label
    Protected WithEvents lblGSTOutputTC As System.Web.UI.WebControls.Label
    Protected WithEvents lblTCTaxInv As System.Web.UI.WebControls.Label
    Protected WithEvents lblTCDebitNote As System.Web.UI.WebControls.Label
    Protected WithEvents lblTCCreditNote As System.Web.UI.WebControls.Label
    Protected WithEvents lblBlockTC As System.Web.UI.WebControls.Label
    Protected WithEvents lblGiftLuckyInput As System.Web.UI.WebControls.Label
    Protected WithEvents lblGiftLuckyOutput As System.Web.UI.WebControls.Label
    'Zulham 01/04/2016 - IM5/IM6 enhancement
    Protected WithEvents lblIM5 As System.Web.UI.WebControls.Label
    Protected WithEvents lblIM6 As System.Web.UI.WebControls.Label

    Protected WithEvents ddl_GSTInputTC As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddl_GSTOutputTC As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddl_TCTaxInv As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddl_TCDebitNote As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddl_TCCreditNote As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddl_BlockTC As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddl_GiftLuckyInput As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddl_GiftLuckyOutput As System.Web.UI.WebControls.DropDownList
    'Zulham 01/04/2016 - im5/im6 enhancement
    Protected WithEvents ddl_IM5 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddl_IM6 As System.Web.UI.WebControls.DropDownList

    Protected WithEvents label1 As System.Web.UI.WebControls.Label
    Protected WithEvents label2 As System.Web.UI.WebControls.Label
    Protected WithEvents label3 As System.Web.UI.WebControls.Label
    Protected WithEvents label4 As System.Web.UI.WebControls.Label
    Protected WithEvents label5 As System.Web.UI.WebControls.Label
    Protected WithEvents label6 As System.Web.UI.WebControls.Label
    Protected WithEvents label7 As System.Web.UI.WebControls.Label




    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    'Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
    '    'cmd_save.Enabled = False
    '    'Dim alButtonList As ArrayList
    '    'alButtonList = New ArrayList
    '    'alButtonList.Add(cmd_save)
    '    'htPageAccess.Add("add", alButtonList)
    '    'htPageAccess.Add("update", alButtonList)
    '    'CheckButtonAccess()

    'End Sub
   
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        lblHeader.Text = "&nbsp;Description"
        lblHeader2.Text = "&nbsp;Value"
        If Not IsPostBack Then
           
            displayParam()
         
        End If

        Me.txt_LateSubmit.Attributes.Add("onkeypress", "return isNumberKey(event);")
        'Me.txt_BCCASA.Attributes.Add("onkeypress", "return isNumberKey(event);")
        'Me.txt_BCCheque.Attributes.Add("onkeypress", "return isNumberKey(event);")
        Me.txt_CASALimit.Attributes.Add("onkeypress", "return isDecimalKey(event);")
        Me.txt_IBGLimit.Attributes.Add("onkeypress", "return isDecimalKey(event);")
        Me.txt_BCIBG.Attributes.Add("onkeypress", "return isDecimalKey(event);")
        Me.txt_BCRENTAS.Attributes.Add("onkeypress", "return isDecimalKey(event);")
        'Me.txt_DEFAULTDUEDATE.Attributes.Add("onkeypress", "return isNumberKey(event);")
		Me.txt_BCBC.Attributes.Add("onkeypress", "return isDecimalKey(event);")
        Me.txt_BCTT1.Attributes.Add("onkeypress", "return isDecimalKey(event);")
        Me.txt_BCTT2.Attributes.Add("onkeypress", "return isDecimalKey(event);")
        ' aichu modify on 09/09/2005
        ' not allowed space for 1st character for Prefix
        ' previous validation expression = ^[^']*$; changed to ^[^ '][^']*$
        'Me.txt_CSRPre.Attributes.Add("onblur", "resetValue('" & txt_CSRPre.ClientID & "','');")
        'Me.txt_DoPre.Attributes.Add("onblur", "resetValue('" & txt_DoPre.ClientID & "','');")
        'Me.txt_InvPre.Attributes.Add("onblur", "resetValue('" & txt_InvPre.ClientID & "','');")
        'Me.txt_QuoPre.Attributes.Add("onblur", "resetValue('" & txt_QuoPre.ClientID & "','');")

        'Dim strMsg As String = "Changing document prefixes may affect Buyer Company interface files.  Please click Cancel to abort Or click OK to proceed."
        'Me.cmd_save.Attributes.Add("onClick", "return confirmChanged('" & strMsg & "');")

    End Sub

    'Private Function validateInput() As Boolean
    '    If Not Me.txt_QuoPre.Text = "" AndAlso Not IsValidAlphaNumericInput(Trim(txt_QuoPre.Text)) Then
    '        Common.NetMsgbox(Me, "Please enter valid characters for your Quotation Prefix.", MsgBoxStyle.Information)
    '        Return False
    '    End If
    '    If Not Me.txt_DoPre.Text = "" AndAlso Not IsValidAlphaNumericInput(Trim(txt_DoPre.Text)) Then
    '        Common.NetMsgbox(Me, "Please enter valid characters for your DO Prefix.", MsgBoxStyle.Information)
    '        Return False
    '    End If
    '    If Not Me.txt_CSRPre.Text = "" AndAlso Not IsValidAlphaNumericInput(Trim(txt_CSRPre.Text)) Then
    '        Common.NetMsgbox(Me, "Please enter valid characters for your CSR Prefix.", MsgBoxStyle.Information)
    '        Return False
    '    End If
    '    If Not Me.txt_InvPre.Text = "" AndAlso Not IsValidAlphaNumericInput(Trim(txt_InvPre.Text)) Then
    '        Common.NetMsgbox(Me, "Please enter valid characters for your Invoice Prefix.", MsgBoxStyle.Information)
    '        Return False
    '    End If

    '    Return True
    'End Function
    Function validateInput() As Boolean
        Dim count As Integer = 0
        Dim i As Integer
        Dim str As String
        Dim errmsg As String = ""
        'CASA LIMIT
        For i = 0 To txt_CASALimit.Text.Length - 1
            str = txt_CASALimit.Text.Substring(i, 1)
            If str = "." Then
                count = count + 1
            End If
        Next
        If count >= 2 Then
            errmsg = "Invalid CASA LIMIT"
            Common.NetMsgbox(Me, errmsg, MsgBoxStyle.Information)
            Return False
        End If
        count = 0
        'IBG LIMIT
        For i = 0 To txt_IBGLimit.Text.Length - 1
            str = txt_IBGLimit.Text.Substring(i, 1)
            If str = "." Then
                count = count + 1
            End If
        Next
        If count >= 2 Then
            errmsg = "Invalid IBG LIMIT"
            Common.NetMsgbox(Me, errmsg, MsgBoxStyle.Information)
            Return False
        End If
        count = 0
        'BC ING
        For i = 0 To txt_BCIBG.Text.Length - 1
            str = txt_BCIBG.Text.Substring(i, 1)
            If str = "." Then
                count = count + 1
            End If
        Next
        If count >= 2 Then
            errmsg = "Invalid Bank Charges For IBG"
            Common.NetMsgbox(Me, errmsg, MsgBoxStyle.Information)
            Return False
        End If
        count = 0
        'BC RENTAS
        For i = 0 To txt_BCRENTAS.Text.Length - 1
            str = txt_BCRENTAS.Text.Substring(i, 1)
            If str = "." Then
                count = count + 1
            End If
        Next
        If count >= 2 Then
            errmsg = "Invalid Bank Charges For RENTAS"
            Common.NetMsgbox(Me, errmsg, MsgBoxStyle.Information)
            Return False
        End If
        count = 0
		'BC CHARGE
        For i = 0 To txt_BCBC.Text.Length - 1
            str = txt_BCBC.Text.Substring(i, 1)
            If str = "." Then
                count = count + 1
            End If
        Next
        If count >= 2 Then
            errmsg = "Invalid Bank Charges For Banker's Cheque"
            Common.NetMsgbox(Me, errmsg, MsgBoxStyle.Information)
            Return False
        End If
        count = 0
        'TT1 CHARGE
        For i = 0 To txt_BCTT1.Text.Length - 1
            str = txt_BCTT1.Text.Substring(i, 1)
            If str = "." Then
                count = count + 1
            End If
        Next
        If count >= 2 Then
            errmsg = "Invalid Bank Charges For TT (Up to 10K)"
            Common.NetMsgbox(Me, errmsg, MsgBoxStyle.Information)
            Return False
        End If
        count = 0
        'TT2 CHARGE
        For i = 0 To txt_BCTT2.Text.Length - 1
            str = txt_BCTT2.Text.Substring(i, 1)
            If str = "." Then
                count = count + 1
            End If
        Next
        If count >= 2 Then
            errmsg = "Invalid Bank Charges For TT (Above 10K)"
            Common.NetMsgbox(Me, errmsg, MsgBoxStyle.Information)
            Return False
        End If
        count = 0
        Return True
    End Function
    Private Sub cmd_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_save.Click

        If validateInput() Then
            Dim htList As New Hashtable
            Dim htList2 As New Hashtable
            Dim strparamname As String
            Dim strparamvalue As String
            Dim strparamtype As String
            Dim strMsg As String
            Dim objParam As New IPP



            htList.Add("LATE_SUBMIT_CHECK", txt_LateSubmit.Text)
            htList.Add("CASA_BANKCODE", txt_BCCASA.Text)
            htList.Add("BC_BANKCODE", txt_BCCheque.Text)
            htList.Add("CASA_LIMIT", txt_CASALimit.Text)
            htList.Add("IBG_LIMIT", txt_IBGLimit.Text)
            htList.Add("IBG_CHARGE", txt_BCIBG.Text)
            htList.Add("RENTAS_CHARGE", txt_BCRENTAS.Text)            
			htList.Add("BC_CHARGE", txt_BCBC.Text)
            htList.Add("TT1_CHARGE", txt_BCTT1.Text)
            htList.Add("TT2_CHARGE", txt_BCTT2.Text)
            htList.Add("REVERSE_CHARGE_INPUT", ddl_GSTInputTC.SelectedValue)
            htList.Add("REVERSE_CHARGE_OUTPUT", ddl_GSTOutputTC.SelectedValue)
            htList.Add("OUTGOING_TAX_CODE_INVOICE", ddl_TCTaxInv.SelectedValue)
            htList.Add("OUTGOING_TAX_CODE_DN", ddl_TCDebitNote.SelectedValue)
            htList.Add("OUTGOING_TAX_CODE_CN", ddl_TCCreditNote.SelectedValue)
            htList.Add("BLOCK", ddl_BlockTC.SelectedValue)
            htList.Add("GIFT_LUCKY_INPUT", ddl_GiftLuckyInput.SelectedValue)
            htList.Add("GIFT_LUCKY_OUTPUT", ddl_GiftLuckyOutput.SelectedValue)
            'Zulham 01/04/2016 - IM5/IM6 Enhancement\
            htList.Add("REVERSE_CHARGE_INPUT_IM5", ddl_IM5.SelectedValue)
            htList.Add("REVERSE_CHARGE_OUTPUT_IM6", ddl_IM6.SelectedValue)

            'htList.Add("DEFAULT_DUE_DATE", txt_DEFAULTDUEDATE.Text)

            'htList2.Add("LateSubmitID", label1.Text)
            'htList2.Add("BCCASAID", label2.Text)
            'htList2.Add("BCCChequeID", label3.Text)
            'htList2.Add("CASALimitID", label4.Text)
            'htList2.Add("IBGLimitID", label5.Text)
            'htList2.Add("BCIBGID", label6.Text)
            'htList2.Add("BCRENTASID", label7.Text)


            If objParam.UpdateIPPParam(htList) Then

                objParam.Message(Me, "00003", MsgBoxStyle.Information)
            Else
                objParam.Message(Me, "00002", MsgBoxStyle.Information)
            End If


            displayParam()
        End If

    End Sub


    Private Sub displayParam()
        Dim dsParam As New DataSet
        Dim objParam As New IPP
        Dim intCount As Integer
        Dim i As Integer
        dsParam = objParam.GetIPPParam
        
        intCount = dsParam.Tables(0).Rows.Count()
        For i = 0 To intCount - 1

            Select Case dsParam.Tables(0).Rows(i)(0)
               

                Case "LATE_SUBMIT_CHECK"
                    'label1.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(0))
                    lblLateSubmit.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1)).ToString.Replace("IPP", "E2P") 'Zulham 10072018 - PAMB
                    txt_LateSubmit.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(2))
                Case "CASA_BANKCODE"
                    'label2.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(0))
                    lblBankCodeCASA.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    txt_BCCASA.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(2))
                Case "BC_BANKCODE"
                    'label3.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(0))
                    lblBankCodeCheque.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    txt_BCCheque.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(2))
                Case "CASA_LIMIT"
                    'label4.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(0))
                    lblCASALimit.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    If Common.parseNull(dsParam.Tables(0).Rows(i)(2)) = "" Then
                        txt_CASALimit.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(2))
                    Else
                        txt_CASALimit.Text = Format(CDbl(Common.parseNull(dsParam.Tables(0).Rows(i)(2))), "###,###,##0.00")
                    End If



                Case "IBG_LIMIT"
                    'label5.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(0))
                    lblIBGLimit.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    If Common.parseNull(dsParam.Tables(0).Rows(i)(2)) = "" Then
                        txt_IBGLimit.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(2))
                    Else
                        txt_IBGLimit.Text = Format(CDbl(Common.parseNull(dsParam.Tables(0).Rows(i)(2))), "###,###,##0.00")
                    End If

                Case "IBG_CHARGE"
                    'label6.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(0))
                    lblBankChargeIBG.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    If Common.parseNull(dsParam.Tables(0).Rows(i)(2)) = "" Then
                        txt_BCIBG.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(2))
                    Else
                        txt_BCIBG.Text = Format(CDbl(Common.parseNull(dsParam.Tables(0).Rows(i)(2))), "##0.00")
                    End If
                Case "RENTAS_CHARGE"
                    'label7.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(0))
                    lblBankChargeRENTAS.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    If Common.parseNull(dsParam.Tables(0).Rows(i)(2)) = "" Then
                        txt_BCRENTAS.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(2))
                    Else
                        txt_BCRENTAS.Text = Format(CDbl(Common.parseNull(dsParam.Tables(0).Rows(i)(2))), "##0.00")
                    End If              
				Case "BC_CHARGE"
                    lblBankChargeBC.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    If Common.parseNull(dsParam.Tables(0).Rows(i)(2)) = "" Then
                        txt_BCBC.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(2))
                    Else
                        txt_BCBC.Text = Format(CDbl(Common.parseNull(dsParam.Tables(0).Rows(i)(2))), "##0.00")
                    End If

                Case "TT1_CHARGE"
                    lblBankChargeTT1.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    If Common.parseNull(dsParam.Tables(0).Rows(i)(2)) = "" Then
                        txt_BCTT1.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(2))
                    Else
                        txt_BCTT1.Text = Format(CDbl(Common.parseNull(dsParam.Tables(0).Rows(i)(2))), "##0.00")
                    End If

                Case "TT2_CHARGE"
                    lblBankChargeTT2.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    If Common.parseNull(dsParam.Tables(0).Rows(i)(2)) = "" Then
                        txt_BCTT2.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(2))
                    Else
                        txt_BCTT2.Text = Format(CDbl(Common.parseNull(dsParam.Tables(0).Rows(i)(2))), "##0.00")
                    End If

                Case "REVERSE_CHARGE_INPUT"
                    lblGSTInputTC.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    objGlobal.FillTaxCode(ddl_GSTInputTC, "", "P", "IPP")
                    ddl_GSTInputTC.SelectedValue = Common.parseNull(dsParam.Tables(0).Rows(i)(2))
                    'txt_LateSubmit.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(2))

                Case "REVERSE_CHARGE_OUTPUT"
                    lblGSTOutputTC.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    objGlobal.FillTaxCode(ddl_GSTOutputTC, "", "S", "IPP")
                    ddl_GSTOutputTC.SelectedValue = Common.parseNull(dsParam.Tables(0).Rows(i)(2))
                    'txt_LateSubmit.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(2))

                    'mimi 18082017 - 
                Case "OUTGOING_TAX_CODE_INVOICE"
                    lblTCTaxInv.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    objGlobal.FillTaxCode(ddl_TCTaxInv, "", "S", "IPP")
                    ddl_TCTaxInv.SelectedValue = Common.parseNull(dsParam.Tables(0).Rows(i)(2))
                    'txt_LateSubmit.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(2))

                Case "OUTGOING_TAX_CODE_DN"
                    lblTCDebitNote.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    objGlobal.FillTaxCode(ddl_TCDebitNote, "", "S", "IPP")
                    ddl_TCDebitNote.SelectedValue = Common.parseNull(dsParam.Tables(0).Rows(i)(2))
                    'txt_LateSubmit.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(2))

                Case "OUTGOING_TAX_CODE_CN"
                    lblTCCreditNote.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    objGlobal.FillTaxCode(ddl_TCCreditNote, "", "S", "IPP")
                    ddl_TCCreditNote.SelectedValue = Common.parseNull(dsParam.Tables(0).Rows(i)(2))
                    'END

                Case "BLOCK"
                    lblBlockTC.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    objGlobal.FillTaxCode(ddl_BlockTC, "", "P", "IPP")
                    ddl_BlockTC.SelectedValue = Common.parseNull(dsParam.Tables(0).Rows(i)(2))

                Case "GIFT_LUCKY_INPUT"
                    lblGiftLuckyInput.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    objGlobal.FillTaxCode(ddl_GiftLuckyInput, "", "P", "IPP")
                    ddl_GiftLuckyInput.SelectedValue = Common.parseNull(dsParam.Tables(0).Rows(i)(2))

                Case "GIFT_LUCKY_OUTPUT"
                    lblGiftLuckyOutput.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    objGlobal.FillTaxCode(ddl_GiftLuckyOutput, "", "S", "IPP")
                    ddl_GiftLuckyOutput.SelectedValue = Common.parseNull(dsParam.Tables(0).Rows(i)(2))
                    'txt_LateSubmit.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(2))
                    'Case "DEFAULT_DUE_DATE"
                    '    'label7.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(0))
                    '    lblDefaultDueDate.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    '    txt_DEFAULTDUEDATE.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(2))

                    'Zulham 01/04/2016 - IM5/IM6 Enhancement
                Case "REVERSE_CHARGE_INPUT_IM5"
                    lblim5.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    objGlobal.FillTaxCode(ddl_IM5, "", "P", "IPP")
                    ddl_IM5.SelectedValue = Common.parseNull(dsParam.Tables(0).Rows(i)(2))

                Case "REVERSE_CHARGE_OUTPUT_IM6"
                    lblIM6.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    objGlobal.FillTaxCode(ddl_IM6, "", "S", "IPP")
                    ddl_IM6.SelectedValue = Common.parseNull(dsParam.Tables(0).Rows(i)(2))
            End Select

        Next

    End Sub
    
End Class
