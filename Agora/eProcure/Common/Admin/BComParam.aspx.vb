Imports AgoraLegacy
Imports eProcure.Component
Public Class BComParam
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Protected WithEvents txt_RfqPre As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_RfqLun As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_PrPre As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_PrLun As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_PoPre As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_PoLun As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_GrnPre As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_GrnLun As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_SrnPre As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_SrnLun As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_PayPre As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_PayLun As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_TenPre As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_TenLun As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_DebitPre As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_DebitLun As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_IRPre As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_IRLun As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_ITPre As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_ITLun As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_RIPre As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_RILun As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_ROPre As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_ROLun As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_WOPre As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_WOLun As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_Stk_Received As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_Smart_Pay As System.Web.UI.WebControls.TextBox 'mimi : 21/03/2017 - enhancement smart pay ref.

    Protected WithEvents revRFQPre As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents revPRPre As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents revPOPre As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents revGRNPre As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents revSRNPre As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents revPayPre As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents revTenPre As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents revDebitPre As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents revIRPre As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents revITPre As System.Web.UI.WebControls.RegularExpressionValidator

    Protected WithEvents revRFQLun As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents revPRLun As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents revPOLun As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents revGRNLun As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents revSRNLun As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents revPayLun As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents revTenLun As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents revDebitLun As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents revIRLun As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents revITLun As System.Web.UI.WebControls.RegularExpressionValidator

    Protected WithEvents cmd_save As System.Web.UI.WebControls.Button
    Protected WithEvents opl_RfqOp As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents Radiobuttonlist1 As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents Radiobuttonlist2 As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents Radiobuttonlist3 As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents cmd_Reset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents btnIQC As System.Web.UI.WebControls.Button
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents ChkFreeForm As System.Web.UI.WebControls.CheckBox
    Protected WithEvents ChkConsolidation As System.Web.UI.WebControls.CheckBox
    Protected WithEvents ChkAccCode As System.Web.UI.WebControls.CheckBox
    Protected WithEvents ChkUrgentIREmail As System.Web.UI.WebControls.CheckBox
    Protected WithEvents ChkSafetyLvlEmail As System.Web.UI.WebControls.CheckBox
    Protected WithEvents ChkRejectIREmail As System.Web.UI.WebControls.CheckBox
    Protected WithEvents ChkReorderLvlEmail As System.Web.UI.WebControls.CheckBox
    Protected WithEvents ChkLocSecEmail As System.Web.UI.WebControls.CheckBox
    Protected WithEvents ChkMaxInvLvlEmail As System.Web.UI.WebControls.CheckBox
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents ChkLevelsRec As System.Web.UI.WebControls.CheckBox
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents cboAO As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Image1 As System.Web.UI.WebControls.Image
    'Protected WithEvents cboNAO As System.Web.UI.WebControls.DropDownList
    Dim valchkstate As String
    Dim chkval As Boolean
    Dim intstate As Integer
    Dim aryIQC As New ArrayList()
    Protected WithEvents hidMode As System.Web.UI.HtmlControls.HtmlInputHidden
    Dim clsAdmin As New Admin

    'Dim txt_PrLunSC, txt_PoLunSC, txt_GrnLunSC, txt_RfqLunSC, txt_PayLunSC, txt_TenLunSC As String
    'Dim txt_SrnLunSC, txt_DebitLunSC, txt_IRLunSC, txt_ITLunSC, txt_RILunSC, txt_ROLunSC As String
    'Dim txt_WOLunSC As String

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
        cmd_save.Enabled = False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmd_save)
        htPageAccess.Add("add", alButtonList)
        htPageAccess.Add("update", alButtonList)
        CheckButtonAccess()
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        
        lblHeader.Text = "&nbsp;Document Prefix And Last Used Number"
        If Not IsPostBack Then
            Dim objGlobal As New AppGlobals
            ViewState("Row") = 3
            BuildRow()
            displayParam()
            ViewState("Side") = Request.Params("side")
            ViewState("type") = "PO"
            BindGroupAO()
            'BindGroupNAO()
            GenerateTab()
        End If
        ChkConsolidation.Attributes.Add("onclick", "return Verifycheck();")
        ' aichu modify on 09/09/2005
        ' not allowed space for 1st character for Prefix
        ' previous validation expression = ^[^']*$; changed to ^[^ '][^']*$
        Me.txt_RfqPre.Attributes.Add("onblur", "resetValue('" & txt_RfqPre.ClientID & "','');")
        Me.txt_PrPre.Attributes.Add("onblur", "resetValue('" & txt_PrPre.ClientID & "','');")
        Me.txt_PoPre.Attributes.Add("onblur", "resetValue('" & txt_PoPre.ClientID & "','');")
        Me.txt_PayPre.Attributes.Add("onblur", "resetValue('" & txt_PayPre.ClientID & "','');")
        Me.txt_GrnPre.Attributes.Add("onblur", "resetValue('" & txt_GrnPre.ClientID & "','');")
        Me.txt_DebitPre.Attributes.Add("onblur", "resetValue('" & txt_DebitPre.ClientID & "','');")
        Me.txt_SrnPre.Attributes.Add("onblur", "resetValue('" & txt_SrnPre.ClientID & "','');")
        Me.txt_TenPre.Attributes.Add("onblur", "resetValue('" & txt_TenPre.ClientID & "','');")
        Me.txt_IRPre.Attributes.Add("onblur", "resetValue('" & txt_IRPre.ClientID & "','');")
        Me.txt_ITPre.Attributes.Add("onblur", "resetValue('" & txt_ITPre.ClientID & "','');")
        Me.txt_RIPre.Attributes.Add("onblur", "resetValue('" & txt_RIPre.ClientID & "','');")
        Me.txt_ROPre.Attributes.Add("onblur", "resetValue('" & txt_ROPre.ClientID & "','');")
        Me.txt_WOPre.Attributes.Add("onblur", "resetValue('" & txt_WOPre.ClientID & "','');")

        Image1.ImageUrl = dDispatcher.direct("Plugins/images", "collapse_up.gif")
        'If Session("Env") = "FTN" Then
        '    ChkConsolidation.Style("display") = "none"
        '    ChkLevelsRec.Style("display") = "none"
        'End If

    End Sub


    Private Function validateInput() As Boolean
        If Not Me.txt_RfqPre.Text = "" AndAlso Not IsValidAlphaNumericInput(Trim(txt_RfqPre.Text)) Then
            Common.NetMsgbox(Me, "Please enter valid characters for your RFQ Prefix.", MsgBoxStyle.Information)
            Return False
        End If
        If Not Me.txt_PrPre.Text = "" AndAlso Not IsValidAlphaNumericInput(Trim(txt_PrPre.Text)) Then
            Common.NetMsgbox(Me, "Please enter valid characters for your PR Prefix.", MsgBoxStyle.Information)
            Return False
        End If
        If Not Me.txt_PoPre.Text = "" AndAlso Not IsValidAlphaNumericInput(Trim(txt_PoPre.Text)) Then
            Common.NetMsgbox(Me, "Please enter valid characters for your PO Prefix.", MsgBoxStyle.Information)
            Return False
        End If
        If Not Me.txt_GrnPre.Text = "" AndAlso Not IsValidAlphaNumericInput(Trim(txt_GrnPre.Text)) Then
            Common.NetMsgbox(Me, "Please enter valid characters for your GRN Prefix.", MsgBoxStyle.Information)
            Return False
        End If
        If Not Me.txt_SrnPre.Text = "" AndAlso Not IsValidAlphaNumericInput(Trim(txt_SrnPre.Text)) Then
            Common.NetMsgbox(Me, "Please enter valid characters for your SRN Prefix.", MsgBoxStyle.Information)
            Return False
        End If
        If Not Me.txt_PayPre.Text = "" AndAlso Not IsValidAlphaNumericInput(Trim(txt_PayPre.Text)) Then
            Common.NetMsgbox(Me, "Please enter valid characters for your Payment Prefix.", MsgBoxStyle.Information)
            Return False
        End If
        If Not Me.txt_TenPre.Text = "" AndAlso Not IsValidAlphaNumericInput(Trim(txt_TenPre.Text)) Then
            Common.NetMsgbox(Me, "Please enter valid characters for your Tender Prefix.", MsgBoxStyle.Information)
            Return False
        End If

        If Not Me.txt_DebitPre.Text = "" AndAlso Not IsValidAlphaNumericInput(Trim(txt_DebitPre.Text)) Then
            Common.NetMsgbox(Me, "Please enter valid characters for your Cancellation Request Prefix.", MsgBoxStyle.Information)
            Return False
        End If
        If Not Me.txt_IRPre.Text = "" AndAlso Not IsValidAlphaNumericInput(Trim(txt_IRPre.Text)) Then
            Common.NetMsgbox(Me, "Please enter valid characters for your Inventory Requisition Prefix.", MsgBoxStyle.Information)
            Return False
        End If
        If Not Me.txt_ITPre.Text = "" AndAlso Not IsValidAlphaNumericInput(Trim(txt_ITPre.Text)) Then
            Common.NetMsgbox(Me, "Please enter valid characters for your Inventory Transfer Prefix.", MsgBoxStyle.Information)
            Return False
        End If
        If Not Me.txt_RIPre.Text = "" AndAlso Not IsValidAlphaNumericInput(Trim(txt_RIPre.Text)) Then
            Common.NetMsgbox(Me, "Please enter valid characters for your Return Inward Prefix.", MsgBoxStyle.Information)
            Return False
        End If
        If Not Me.txt_ROPre.Text = "" AndAlso Not IsValidAlphaNumericInput(Trim(txt_ROPre.Text)) Then
            Common.NetMsgbox(Me, "Please enter valid characters for your Return Outward Prefix.", MsgBoxStyle.Information)
            Return False
        End If
        If Not Me.txt_WOPre.Text = "" AndAlso Not IsValidAlphaNumericInput(Trim(txt_WOPre.Text)) Then
            Common.NetMsgbox(Me, "Please enter valid characters for your Write Off Prefix.", MsgBoxStyle.Information)
            Return False
        End If
        If Radiobuttonlist1.SelectedValue = "P" And Me.cboAO.SelectedItem.Text = "---Select---" Then
            Common.NetMsgbox(Me, "Please select purchasing officer.", MsgBoxStyle.Information)
            Return False
        End If
        If Not Me.txt_Stk_Received.Text = "" AndAlso Not IsNumeric(Trim(txt_Stk_Received.Text)) Then
            Common.NetMsgbox(Me, "Please enter numeric value for Stock Received Auto Acknowledge Working Days.", MsgBoxStyle.Information)
            Return False
        End If
        'mimi : 21/03/2017 - enhancement smart pay ref.
        If Not Me.txt_Smart_Pay.Text = "" AndAlso Not IsNumeric(Trim(txt_Smart_Pay.Text)) Then
            Common.NetMsgbox(Me, "Please enter numeric value for Smart Pay Reference.", MsgBoxStyle.Information)
            Return False
        End If
        'mimi : 23/03/2017 - enhancement smart pay ref.
        If Not Me.txt_Smart_Pay.Text = "" AndAlso IsNumeric(Trim(txt_Smart_Pay.Text)) = True Then
            If Trim(txt_Smart_Pay.Text) < 0 Then
                Common.NetMsgbox(Me, "Smart Pay Cap Limit must be greater than or equal to zero.", MsgBoxStyle.Information)
                Return False
            End If
        End If
        'end
        'If Radiobuttonlist2.SelectedValue = "NP" And Me.cboNAO.SelectedItem.Text = "---Select---" Then
        '    Common.NetMsgbox(Me, "Please select purchasing officer.", MsgBoxStyle.Information)
        '    Return False
        'End If
        Return True
    End Function

    Public Function BindGroupAO()
        Dim objApp As New ApprWorkFlow
        Dim cbolist As New ListItem
        Dim dvCustom As DataView

        Select Case ViewState("type")
            Case "PO"
                dvCustom = objApp.getcboPO()
                'Case "FO"
                '    dvCustom = objApp.getcboFO()
                'Case "FM"
                '    dvCustom = objApp.getcboFM()
        End Select

        cboAO.Items.Clear()
        If Not dvCustom Is Nothing Then
            Common.FillDdl(cboAO, "two", "UM_USER_ID", dvCustom)
            If ViewState("strAO") <> "" Then
                Common.SelDdl(ViewState("strAO"), cboAO, True, False)
            End If
        End If
        cbolist.Value = ""
        cbolist.Text = "---Select---"
        cboAO.Items.Insert(0, cbolist)

    End Function

    'Public Function BindGroupNAO()
    '    Dim objApp As New ApprWorkFlow
    '    Dim cbolist As New ListItem
    '    Dim dvCustom As DataView

    '    Select Case ViewState("type")
    '        Case "PO"
    '            dvCustom = objApp.getcboPO()
    '            'Case "FO"
    '            '    dvCustom = objApp.getcboFO()
    '            'Case "FM"
    '            '    dvCustom = objApp.getcboFM()
    '    End Select

    '    cboNAO.Items.Clear()
    '    If Not dvCustom Is Nothing Then
    '        Common.FillDdl(cboNAO, "two", "UM_USER_ID", dvCustom)
    '        If ViewState("strAO") <> "" Then
    '            Common.SelDdl(ViewState("strAO"), cboNAO, True, False)
    '        End If
    '    End If
    '    cbolist.Value = ""
    '    cbolist.Text = "---Select---"
    '    cboNAO.Items.Insert(0, cbolist)

    'End Function

    Private Sub cmd_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_save.Click

        If Not Rebuild() Then Exit Sub
        If Not validateInput() Then Exit Sub
        'If Not Rebuild() Then Exit Sub
        Dim htList As New Hashtable
        htList.Add("RFQPrefix", txt_RfqPre.Text)
        htList.Add("PRPrefix", txt_PrPre.Text)
        htList.Add("POPrefix", txt_PoPre.Text)
        htList.Add("GRNPrefix", txt_GrnPre.Text)
        htList.Add("SRNPrefix", txt_SrnPre.Text)
        htList.Add("PaymentPrefix", txt_PayPre.Text)
        htList.Add("TenderPrefix", txt_TenPre.Text)
        htList.Add("CRPrefix", txt_DebitPre.Text)
        htList.Add("IRPrefix", txt_IRPre.Text)
        htList.Add("ITPrefix", txt_ITPre.Text)
        htList.Add("RIPrefix", txt_RIPre.Text)
        htList.Add("ROPrefix", txt_ROPre.Text)
        htList.Add("WOPrefix", txt_WOPre.Text)

        Dim checkParam As Boolean = True
        Dim dsParam As New DataSet
        Dim i As Integer
        Dim intCount As Integer
        dsParam = clsAdmin.getParam
        intCount = dsParam.Tables(0).Rows.Count()
        For i = 0 To intCount - 1
            If UCase(dsParam.Tables(0).Rows(i)(0)) = "LAST USED NO" Then
                Select Case dsParam.Tables(0).Rows(i)(2)
                    Case "PR"
                        If Session("txt_PrLunSC") <> txt_PrLun.Text Then
                            If txt_PrLun.Text < Common.parseNull(dsParam.Tables(0).Rows(i)(1)) Then
                                Common.NetMsgbox(Me, "Record not saved. PR Last Used No has been used.", MsgBoxStyle.Information)
                                Exit Sub
                            Else
                                htList.Add("PRLastUsedNo", txt_PrLun.Text)
                            End If
                        Else
                            htList.Add("PRLastUsedNo", Common.parseNull(dsParam.Tables(0).Rows(i)(1)))
                        End If
                    Case "PO"
                        If Session("txt_PoLunSC") <> txt_PoLun.Text Then
                            If txt_PoLun.Text < Common.parseNull(dsParam.Tables(0).Rows(i)(1)) Then
                                Common.NetMsgbox(Me, "Record not saved. PO Last Used No has been used.", MsgBoxStyle.Information)
                                Exit Sub
                            Else
                                htList.Add("POLastUsedNo", txt_PoLun.Text)
                            End If
                        Else
                            htList.Add("POLastUsedNo", Common.parseNull(dsParam.Tables(0).Rows(i)(1)))
                        End If
                    Case "GRN"
                        If Session("txt_GrnLunSC") <> txt_GrnLun.Text Then
                            If txt_GrnLun.Text < Common.parseNull(dsParam.Tables(0).Rows(i)(1)) Then
                                Common.NetMsgbox(Me, "Record not saved. GRN Last Used No has been used.", MsgBoxStyle.Information)
                                Exit Sub
                            Else
                                htList.Add("GRNLastUsedNo", txt_GrnLun.Text)
                            End If
                        Else
                            htList.Add("GRNLastUsedNo", Common.parseNull(dsParam.Tables(0).Rows(i)(1)))
                        End If
                    Case "RFQ"
                        If Session("txt_RfqLunSC") <> txt_RfqLun.Text Then
                            If txt_RfqLun.Text < Common.parseNull(dsParam.Tables(0).Rows(i)(1)) Then
                                Common.NetMsgbox(Me, "Record not saved. RFQ Last Used No has been used.", MsgBoxStyle.Information)
                                Exit Sub
                            Else
                                htList.Add("RFQLastUsedNo", txt_RfqLun.Text)
                            End If
                        Else
                            htList.Add("RFQLastUsedNo", Common.parseNull(dsParam.Tables(0).Rows(i)(1)))
                        End If
                    Case "Payment"
                        If Session("txt_PayLunSC") <> txt_PayLun.Text Then
                            If txt_PayLun.Text < Common.parseNull(dsParam.Tables(0).Rows(i)(1)) Then
                                Common.NetMsgbox(Me, "Record not saved. Payment Last Used No has been used.", MsgBoxStyle.Information)
                                Exit Sub
                            Else
                                htList.Add("PaymentLastUsedNo", txt_PayLun.Text)
                            End If
                        Else
                            htList.Add("PaymentLastUsedNo", Common.parseNull(dsParam.Tables(0).Rows(i)(1)))
                        End If
                    Case "Tender"
                        If Session("txt_TenLunSC") <> txt_TenLun.Text Then
                            If txt_TenLun.Text < Common.parseNull(dsParam.Tables(0).Rows(i)(1)) Then
                                Common.NetMsgbox(Me, "Record not saved. Tender Last Used No has been used.", MsgBoxStyle.Information)
                                Exit Sub
                            Else
                                htList.Add("TenderLastUsedNo", txt_TenLun.Text)
                            End If
                        Else
                            htList.Add("TenderLastUsedNo", Common.parseNull(dsParam.Tables(0).Rows(i)(1)))
                        End If
                    Case "SRN"
                        If Session("txt_SrnLunSC") <> txt_SrnLun.Text Then
                            If txt_SrnLun.Text < Common.parseNull(dsParam.Tables(0).Rows(i)(1)) Then
                                Common.NetMsgbox(Me, "Record not saved. SRN Last Used No has been used.", MsgBoxStyle.Information)
                                Exit Sub
                            Else
                                htList.Add("SRNLastUsedNo", txt_SrnLun.Text)
                            End If
                        Else
                            htList.Add("SRNLastUsedNo", Common.parseNull(dsParam.Tables(0).Rows(i)(1)))
                        End If
                    Case "CR"
                        If Session("txt_DebitLunSC") <> txt_DebitLun.Text Then
                            If txt_DebitLun.Text < Common.parseNull(dsParam.Tables(0).Rows(i)(1)) Then
                                Common.NetMsgbox(Me, "Record not saved. Cancellation Request Last Used No has been used.", MsgBoxStyle.Information)
                                Exit Sub
                            Else
                                htList.Add("CRLastUsedNo", txt_DebitLun.Text)
                            End If
                        Else
                            htList.Add("CRLastUsedNo", Common.parseNull(dsParam.Tables(0).Rows(i)(1)))
                        End If
                    Case "IR"
                        If Session("txt_IRLunSC") <> txt_IRLun.Text Then
                            If txt_IRLun.Text < Common.parseNull(dsParam.Tables(0).Rows(i)(1)) Then
                                Common.NetMsgbox(Me, "Record not saved. Inventory Requisition Last Used No has been used.", MsgBoxStyle.Information)
                                Exit Sub
                            Else
                                htList.Add("IRLastUsedNo", txt_IRLun.Text)
                            End If
                        Else
                            htList.Add("IRLastUsedNo", Common.parseNull(dsParam.Tables(0).Rows(i)(1)))
                        End If
                    Case "IT"
                        If Session("txt_ITLunSC") <> txt_ITLun.Text Then
                            If txt_ITLun.Text < Common.parseNull(dsParam.Tables(0).Rows(i)(1)) Then
                                Common.NetMsgbox(Me, "Record not saved. Inventory Transfer Last Used No has been used.", MsgBoxStyle.Information)
                                Exit Sub
                            Else
                                htList.Add("ITLastUsedNo", txt_ITLun.Text)
                            End If
                        Else
                            htList.Add("ITLastUsedNo", Common.parseNull(dsParam.Tables(0).Rows(i)(1)))
                        End If
                    Case "RI"
                        If Session("txt_RILunSC") <> txt_RILun.Text Then
                            If txt_RILun.Text < Common.parseNull(dsParam.Tables(0).Rows(i)(1)) Then
                                Common.NetMsgbox(Me, "Record not saved. Return Inward Last Used No has been used.", MsgBoxStyle.Information)
                                Exit Sub
                            Else
                                htList.Add("RILastUsedNo", txt_RILun.Text)
                            End If
                        Else
                            htList.Add("RILastUsedNo", Common.parseNull(dsParam.Tables(0).Rows(i)(1)))
                        End If
                    Case "RO"
                        If Session("txt_ROLunSC") <> txt_ROLun.Text Then
                            If txt_ROLun.Text < Common.parseNull(dsParam.Tables(0).Rows(i)(1)) Then
                                Common.NetMsgbox(Me, "Record not saved. Return Outward Last Used No has been used.", MsgBoxStyle.Information)
                                Exit Sub
                            Else
                                htList.Add("ROLastUsedNo", txt_ROLun.Text)
                            End If
                        Else
                            htList.Add("ROLastUsedNo", Common.parseNull(dsParam.Tables(0).Rows(i)(1)))
                        End If
                    Case "WO"
                        If Session("txt_WOLunSC") <> txt_WOLun.Text Then
                            If txt_WOLun.Text < Common.parseNull(dsParam.Tables(0).Rows(i)(1)) Then
                                Common.NetMsgbox(Me, "Record not saved. Write Off Last Used No has been used.", MsgBoxStyle.Information)
                                Exit Sub
                            Else
                                htList.Add("WOLastUsedNo", txt_WOLun.Text)
                            End If
                        Else
                            htList.Add("WOLastUsedNo", Common.parseNull(dsParam.Tables(0).Rows(i)(1)))
                        End If
                End Select
            End If
        Next

        'htList.Add("RFQLastUsedNo", txt_RfqLun.Text)
        'htList.Add("PRLastUsedNo", txt_PrLun.Text)
        'htList.Add("POLastUsedNo", txt_PoLun.Text)
        'htList.Add("GRNLastUsedNo", txt_GrnLun.Text)
        'htList.Add("SRNLastUsedNo", txt_SrnLun.Text)
        'htList.Add("PaymentLastUsedNo", txt_PayLun.Text)
        'htList.Add("TenderLastUsedNo", txt_TenLun.Text)
        'htList.Add("CRLastUsedNo", txt_DebitLun.Text)
        'htList.Add("IRLastUsedNo", txt_IRLun.Text)
        'htList.Add("ITLastUsedNo", txt_ITLun.Text)
        'htList.Add("RILastUsedNo", txt_RILun.Text)
        'htList.Add("ROLastUsedNo", txt_ROLun.Text)
        'htList.Add("WOLastUsedNo", txt_WOLun.Text)

        If hidMode.Value = "1" And ChkConsolidation.Checked = False Then
            clsAdmin.SetNULL_consol()
        End If

        clsAdmin.updateParam(htList, opl_RfqOp.SelectedItem.Value, IIf(ChkFreeForm.Checked, 1, 0), IIf(ChkConsolidation.Checked, 1, 0), IIf(ChkLevelsRec.Checked, 1, 0), Radiobuttonlist1.SelectedItem.Value, cboAO.SelectedItem.Value, Radiobuttonlist2.SelectedItem.Value, Radiobuttonlist3.SelectedItem.Value, IIf(ChkAccCode.Checked, "Y", "N"), Session("aryIQC"), _
                            txt_Stk_Received.Text, IIf(ChkUrgentIREmail.Checked, "Y", "N"), IIf(ChkRejectIREmail.Checked, "Y", "N"), IIf(ChkSafetyLvlEmail.Checked, "Y", "N"), IIf(ChkReorderLvlEmail.Checked, "Y", "N"), IIf(ChkMaxInvLvlEmail.Checked, "Y", "N"), IIf(ChkLocSecEmail.Checked, "Y", "N"), txt_Smart_Pay.Text)
        Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
        displayParam()
        aryIQC.Clear()
        BuildRow()
    End Sub

    Private Sub displayParam()
        Dim dsParam As New DataSet
        Dim intCount As Integer
        Dim i As Integer
        dsParam = clsAdmin.getParam
        intCount = dsParam.Tables(0).Rows.Count()
        For i = 0 To intCount - 1
            If UCase(dsParam.Tables(0).Rows(i)(0)) = "PREFIX" Then
                Select Case dsParam.Tables(0).Rows(i)(2)
                    Case "PR"
                        txt_PrPre.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    Case "PO"
                        txt_PoPre.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    Case "GRN"
                        txt_GrnPre.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    Case "RFQ"
                        txt_RfqPre.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    Case "Payment"
                        txt_PayPre.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    Case "Tender"
                        txt_TenPre.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    Case "SRN"
                        txt_SrnPre.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    Case "CR"
                        txt_DebitPre.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    Case "IR"
                        txt_IRPre.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    Case "IT"
                        txt_ITPre.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    Case "RI"
                        txt_RIPre.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    Case "RO"
                        txt_ROPre.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    Case "WO"
                        txt_WOPre.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                End Select
            Else
                Select Case dsParam.Tables(0).Rows(i)(2)
                    Case "PR"
                        txt_PrLun.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                        Session("txt_PrLunSC") = txt_PrLun.Text
                    Case "PO"
                        txt_PoLun.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                        Session("txt_PoLunSC") = txt_PoLun.Text
                    Case "GRN"
                        txt_GrnLun.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                        Session("txt_GrnLunSC") = txt_GrnLun.Text
                    Case "RFQ"
                        txt_RfqLun.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                        Session("txt_RfqLunSC") = txt_RfqLun.Text
                    Case "Payment"
                        txt_PayLun.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                        Session("txt_PayLunSC") = txt_PayLun.Text
                    Case "Tender"
                        txt_TenLun.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                        Session("txt_TenLunSC") = txt_TenLun.Text
                    Case "SRN"
                        txt_SrnLun.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                        Session("txt_SrnLunSC") = txt_SrnLun.Text
                    Case "CR"
                        txt_DebitLun.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                        Session("txt_DebitLunSC") = txt_DebitLun.Text
                    Case "IR"
                        txt_IRLun.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                        Session("txt_IRLunSC") = txt_IRLun.Text
                    Case "IT"
                        txt_ITLun.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                        Session("txt_ITLunSC") = txt_ITLun.Text
                    Case "RI"
                        txt_RILun.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                        Session("txt_RILunSC") = txt_RILun.Text
                    Case "RO"
                        txt_ROLun.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                        Session("txt_ROLunSC") = txt_ROLun.Text
                    Case "WO"
                        txt_WOLun.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                        Session("txt_WOLunSC") = txt_WOLun.Text
                End Select
            End If
        Next

        If dsParam.Tables(1).Rows.Count > 0 Then
            opl_RfqOp.SelectedValue = dsParam.Tables(1).Rows(0)("CM_RFQ_OPTION")
            Radiobuttonlist1.SelectedValue = dsParam.Tables(1).Rows(0)("CM_CONTR_PR_SETTING")
            Radiobuttonlist2.SelectedValue = dsParam.Tables(1).Rows(0)("CM_NCONTR_PR_SETTING")
            Radiobuttonlist3.SelectedValue = dsParam.Tables(1).Rows(0)("CM_GRN_CONTROL")

            If dsParam.Tables(1).Rows(0)("CM_CONTR_PR_SETTING") = "P" Then
                cboAO.Enabled = True
                cboAO.SelectedValue = dsParam.Tables(1).Rows(0)("CM_CONTR_PR_PO_OWNER_ID")
            Else

                cboAO.Enabled = False
            End If

            If dsParam.Tables(1).Rows(0)("CM_DISPLAY_ACCT") = "Y" Then
                ChkAccCode.Checked = True
            Else
                ChkAccCode.Checked = False
            End If

            If IsDBNull(dsParam.Tables(1).Rows(0)("CM_ACTIVATE_STOCK")) Then
                txt_Stk_Received.Text = ""
            Else
                txt_Stk_Received.Text = dsParam.Tables(1).Rows(0)("CM_ACTIVATE_STOCK")
            End If

            If dsParam.Tables(1).Rows(0)("CM_URGENT_STOCK_EMAIL") = "Y" Then
                ChkUrgentIREmail.Checked = True
            Else
                ChkUrgentIREmail.Checked = False
            End If

            If dsParam.Tables(1).Rows(0)("CM_REJECT_STOCK_EMAIL") = "Y" Then
                ChkRejectIREmail.Checked = True
            Else
                ChkRejectIREmail.Checked = False
            End If

            If dsParam.Tables(1).Rows(0)("CM_SAFETY_STOCK_EMAIL") = "Y" Then
                ChkSafetyLvlEmail.Checked = True
            Else
                ChkSafetyLvlEmail.Checked = False
            End If

            If dsParam.Tables(1).Rows(0)("CM_REORDER_STOCK_EMAIL") = "Y" Then
                ChkReorderLvlEmail.Checked = True
            Else
                ChkReorderLvlEmail.Checked = False
            End If

            If dsParam.Tables(1).Rows(0)("CM_MAXIMUM_STOCK_EMAIL") = "Y" Then
                ChkMaxInvLvlEmail.Checked = True
            Else
                ChkMaxInvLvlEmail.Checked = False
            End If

            If dsParam.Tables(1).Rows(0)("CM_LOCATION_STOCK") = "Y" Then
                ChkLocSecEmail.Checked = True
            Else
                ChkLocSecEmail.Checked = False
            End If
            'If dsParam.Tables(1).Rows(0)("CM_NCONTR_PR_SETTING") = "NP" Then
            '    cboNAO.Enabled = True
            '    cboNAO.SelectedValue = dsParam.Tables(1).Rows(0)("CM_NCONTR_PR_PO_OWNER_ID")
            'Else

            '    cboNAO.Enabled = False
            'End If
            'mimi : 21/03/2017 - enhancement smart pay ref.
            If IsDBNull(dsParam.Tables(1).Rows(0)("CM_SMART_PAY")) Then
                txt_Smart_Pay.Text = "0"
            Else
                txt_Smart_Pay.Text = dsParam.Tables(1).Rows(0)("CM_SMART_PAY")
            End If
            'end
        End If

        intCount = dsParam.Tables(2).Rows.Count
        For i = 0 To intCount - 1
            If UCase(dsParam.Tables(2).Rows(i)("CS_FLAG_NAME")) = "ALLOW FREE FORM BILLING ADDRESS" Then
                If dsParam.Tables(2).Rows(i)("CS_FLAG_VALUE") = 1 Then
                    ChkFreeForm.Checked = True
                Else
                    ChkFreeForm.Checked = False
                End If

            ElseIf UCase(dsParam.Tables(2).Rows(i)("CS_FLAG_NAME")) = "CONSOLIDATION REQUIRED" Then
                viewstate("valchkstate") = dsParam.Tables(2).Rows(i)("CS_FLAG_VALUE")
                If dsParam.Tables(2).Rows(i)("CS_FLAG_VALUE") = 1 Then
                    ChkConsolidation.Checked = True
                    hidMode.Value = viewstate("valchkstate")
                Else
                    ChkConsolidation.Checked = False
                End If

            ElseIf UCase(dsParam.Tables(2).Rows(i)("CS_FLAG_NAME")) = "2 LEVEL RECEIVING" Then
                If dsParam.Tables(2).Rows(i)("CS_FLAG_VALUE") = 1 Then
                    ChkLevelsRec.Checked = True
                Else
                    ChkLevelsRec.Checked = False
                End If


            End If
        Next
    End Sub
    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        'If ViewState("Side") = "BUYER" Then
        '    Session("w_CompBParam_tabs") = "<div class=""t_entity""><ul>" & _
        '     "<li><div class=""space""></div></li>" & _
        '                         "<li><a class=""t_entity_btn"" href=""../Companies/coCompanyDetail.aspx?side=BUYER&mode=modify&pageid=" & strPageId & """><span>Company Details</span></a></li>" & _
        '                             "<li><div class=""space""></div></li>" & _
        '                         "<li><a class=""t_entity_btn_selected"" href=""../Admin/BComParam.aspx?side=BUYER&pageid=" & strPageId & """><span>Parameters</span></a></li>" & _
        '                             "<li><div class=""space""></div></li>" & _
        '                         "<li><a class=""t_entity_btn"" href=""../Admin/AddressMaster.aspx?side=BUYER&type=D&mod=T&pageid=" & strPageId & """><span>Delivery Address</span></a></li>" & _
        '                             "<li><div class=""space""></div></li>" & _
        '                         "<li><a class=""t_entity_btn"" href=""../Admin/AddressMaster.aspx?side=BUYER&type=B&mod=T&pageid=" & strPageId & """><span>Billing Address</span></a></li>" & _
        '                             "<li><div class=""space""></div></li>" & _
        '                           "<li><a class=""t_entity_btn"" href=""../Admin/DeptSetup.aspx?side=BUYER&pageid=" & strPageId & """><span>Department</span></a></li>" & _
        '                             "<li><div class=""space""></div></li>" & _
        '                      "</ul><div></div></div>"
        'Else
        '    Session("w_CompBParam_tabs") = "<div class=""t_entity""><ul>" & _
        '            "<li><div class=""space""></div></li>" & _
        '             "<li><a class=""t_entity_btn"" href=""../Companies/coCompanyDetail.aspx?side=BUYER&mode=modify&pageid=" & strPageId & """><span>Company Details</span></a></li>" & _
        '                 "<li><div class=""space""></div></li>" & _
        '             "<li><a class=""t_entity_btn_selected"" href=""../Admin/BComParam.aspx?side=BUYER&pageid=" & strPageId & """><span>Parameters</span></a></li>" & _
        '                 "<li><div class=""space""></div></li>" & _
        '             "<li><a class=""t_entity_btn"" href=""../Admin/AddressMaster.aspx?side=BUYER&type=D&mod=T&pageid=" & strPageId & """><span>Delivery Address</span></a></li>" & _
        '                 "<li><div class=""space""></div></li>" & _
        '             "<li><a class=""t_entity_btn"" href=""../Admin/AddressMaster.aspx?side=BUYER&type=B&mod=T&pageid=" & strPageId & """><span>Billing Address</span></a></li>" & _
        '                 "<li><div class=""space""></div></li>" & _
        '               "<li><a class=""t_entity_btn"" href=""../Admin/DeptSetup.aspx?side=BUYER&pageid=" & strPageId & """><span>Department</span></a></li>" & _
        '                 "<li><div class=""space""></div></li>" & _
        '                            "<li><a class=""t_entity_btn"" href=""SalesInfo.aspx?side=VENDOR&pageid=" & strPageId & """><span>Sales Info</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                        "<li><a class=""t_entity_btn"" href=""SoftwareApp.aspx?side=VENDOR&pageid=" & strPageId & """><span>Software</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                        "<li><a class=""t_entity_btn"" href=""QualityStd.aspx?side=VENDOR&pageid=" & strPageId & """><span>Quality Standards</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '          "</ul><div></div></div>"
        'End If
        'If ViewState("Side") = "BUYER" Then
        Session("w_CompBParam_tabs") = "<div class=""t_entity""><ul>" & _
"<li><div class=""space""></div></li>" & _
                              "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "coCompanyDetail.aspx", "side=BUYER&mode=modify&pageid=" & strPageId) & """><span>Company Details</span></a></li>" & _
                              "<li><div class=""space""></div></li>" & _
                              "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Admin", "BComParam.aspx", "side=BUYER&pageid=" & strPageId) & """><span>Parameters</span></a></li>" & _
                              "<li><div class=""space""></div></li>" & _
                              "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=D&mod=T&pageid=" & strPageId) & """><span>Delivery Address</span></a></li>" & _
                              "<li><div class=""space""></div></li>" & _
                              "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=B&mod=T&pageid=" & strPageId) & """><span>Billing Address</span></a></li>" & _
                              "<li><div class=""space""></div></li>" & _
                              "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "DeptSetup.aspx", "side=BUYER&pageid=" & strPageId) & """><span>Department</span></a></li>" & _
                              "<li><div class=""space""></div></li>" & _
                              "</ul><div></div></div>"
        'Else
        'Session("w_CompBParam_tabs") = "<div class=""t_entity"">" & _
        '                      "<a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "coCompanyDetail.aspx", "side=BUYER&mode=modify&pageid=" & strPageId) & """><span>Company Details</span></a>" & _
        '                      "<a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Admin", "BComParam.aspx", "side=BUYER&pageid=" & strPageId) & """><span>Parameters</span></a>" & _
        '                      "<a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=D&mod=T&pageid=" & strPageId) & """><span>Delivery Address</span></a>" & _
        '                      "<a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=B&mod=T&pageid=" & strPageId) & """><span>Billing Address</span></a>" & _
        '                  "<a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "DeptSetup.aspx", "side=BUYER&pageid=" & strPageId) & """><span>Department</span></a>" & _
        '                        "<a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "SalesInfo.aspx", "side=VENDOR&pageid=" & strPageId) & """><span>Sales Info</span></a>" & _
        '                    "<a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "SoftwareApp.aspx", "side=VENDOR&pageid=" & strPageId) & """><span>Software</span></a>" & _
        '                    "<a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "QualityStd.aspx", "side=VENDOR&pageid=" & strPageId) & """><span>Quality Standards</span></a>" & _
        '      "</div>"
        'End If

    End Sub

    Private Sub Radiobuttonlist1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Radiobuttonlist1.SelectedIndexChanged
        If Radiobuttonlist1.SelectedValue = "P" Then
            cboAO.Enabled = True
        Else
            cboAO.SelectedIndex = 0
            cboAO.Enabled = False
        End If
    End Sub

    'Private Sub Radiobuttonlist2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Radiobuttonlist2.SelectedIndexChanged
    '    If Radiobuttonlist2.SelectedValue = "NP" Then
    '        cboNAO.Enabled = True
    '    Else
    '        cboNAO.SelectedIndex = 0
    '        cboNAO.Enabled = False
    '    End If
    'End Sub

    Private Function BuildRow()
        Dim i, j, c As Integer
        Dim ds As New DataSet
        Dim objAdmin As New Admin

        ds = objAdmin.getIQCSetting

        If ds.Tables(0).Rows.Count > 0 Then
            If ds.Tables(0).Rows.Count > 3 Then
                ViewState("Row") = ds.Tables(0).Rows.Count
            Else
                ViewState("Row") = 3
            End If

            For i = 0 To ViewState("Row") - 1
                aryIQC.Add(New String() {"", "", "", "", ""})
            Next

            For j = 0 To ds.Tables(0).Rows.Count - 1
                aryIQC(j)(0) = ds.Tables(0).Rows(j)("CPA_PARAM_LABEL")
                aryIQC(j)(1) = ds.Tables(0).Rows(j)("CPA_PARAM_PREFIX")
                aryIQC(j)(2) = ds.Tables(0).Rows(j)("CPA_PARAM_VALUE")
                aryIQC(j)(3) = IIf(ds.Tables(0).Rows(j)("CPA_PARAM_ATTACHMENT") = "Y", "on", "")
                aryIQC(j)(4) = ds.Tables(0).Rows(j)("CPA_INDEX")
            Next

            Session("aryIQC") = aryIQC
        Else
            For i = 0 To ViewState("Row") - 1
                aryIQC.Add(New String() {"", "", "", "", ""})
            Next

            Session("aryIQC") = aryIQC
        End If



        ConstructTable()

    End Function

    Private Function ConstructTable(Optional ByVal aryTemp As ArrayList = Nothing)
        Dim strrow As String = ""
        Dim i, j, count As Integer
        Dim table As String

        If Not aryTemp Is Nothing Then
            aryIQC = aryTemp
        End If

        For i = 0 To aryIQC.Count - 1
            strrow &= "<tr>"
            strrow &= "<td class=""tablecol"" style=""width: 15%""><strong>&nbsp;IQC Test Type Label :</strong></td>"
            strrow &= "<td style=""width: 15%"">"
            strrow &= "<input type=""hidden"" id=""hidIndex" & i & """ name=""hidIndex" & i & """ value=""" & aryIQC(i)(4) & """ runat=""server"">"
            strrow &= "<input style=""width:100%; margin-left:0px; "" class=""txtbox"" type=""text"" id=""txtIQCLabel" & i & """ name=""txtIQCLabel" & i & """ value=""" & aryIQC(i)(0) & """>"
            strrow &= "</td>"
            strrow &= "<td class=""tablecol"" style=""width: 10%""><strong>&nbsp;Prefix :</strong></td>"
            strrow &= "<td style=""width: 15%"">"
            strrow &= "<input style=""width:100%; margin-left:0px; "" class=""txtbox"" type=""text"" id=""txtIQCPrefix" & i & """ name=""txtIQCPrefix" & i & """ value=""" & aryIQC(i)(1) & """>"
            strrow &= "</td>"
            strrow &= "<td class=""tablecol"" style=""width: 15%""><strong>&nbsp;Last Used No :</strong></td>"
            strrow &= "<td style=""width: 15%"">"
            strrow &= "<input style=""width:100%; margin-left:0px; "" class=""txtbox"" type=""text"" id=""txtIQCLastNo" & i & """ name=""txtIQCLastNo" & i & """ value=""" & aryIQC(i)(2) & """>"
            strrow &= "</td>"
            strrow &= "<td style=""width: 15%"">"
            If aryIQC(i)(3) = "on" Then
                strrow &= "<input type=""checkbox"" id=""chkSelection" & i & """ name=""chkSelection" & i & """ checked>Check Attachment(DO)"
            Else
                strrow &= "<input type=""checkbox"" id=""chkSelection" & i & """ name=""chkSelection" & i & """>Check Attachment(DO)"
            End If

            strrow &= "</td>"
        Next

        table = "<table class=""alltable"" id=""Table10"" width=""100%""cellspacing=""0"" cellpadding=""0"" border=""0"">" & _
                strrow & _
                "</table>"

        Session("ConstructTableIQC") = table

    End Function

    Private Sub btnIQC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnIQC.Click
        BuildLine()
        ConstructTable()
    End Sub

    Private Function BuildLine()
        Dim i As Integer
        Dim found As Integer
        found = 0

        aryIQC = Session("aryIQC")
        For i = 0 To aryIQC.Count - 1
            aryIQC(i)(0) = Request.Form("txtIQCLabel" & i)
            aryIQC(i)(1) = Request.Form("txtIQCPrefix" & i)
            aryIQC(i)(2) = Request.Form("txtIQCLastNo" & i)
            aryIQC(i)(3) = Request.Form("chkSelection" & i)
            aryIQC(i)(4) = Request.Form("hidIndex" & i)

            found = found + 1
        Next

        aryIQC.Add(New String() {Request.Form("txtIQCLabel" & found), Request.Form("txtIQCPrefix" & found), Request.Form("txtIQCLastNo" & found), Request.Form("chkSelection" & found), Request.Form("hidIndex" & found)})
        Session("aryIQC") = Nothing
        Session("aryIQC") = aryIQC

    End Function

    Private Function Rebuild() As Boolean
        Dim i, j As Integer
        Dim c As Integer = 0
        Dim blnContinue As Boolean = True
        Dim aryTemp As New ArrayList()
        Dim aryTemp2 As New ArrayList()
        Dim strMsg As String = ""

        aryIQC = Session("aryIQC")

        For i = 0 To aryIQC.Count - 1
            aryIQC(i)(0) = Request.Form("txtIQCLabel" & i)
            aryIQC(i)(1) = Request.Form("txtIQCPrefix" & i)
            aryIQC(i)(2) = Request.Form("txtIQCLastNo" & i)
            aryIQC(i)(3) = Request.Form("chkSelection" & i)
            aryIQC(i)(4) = Request.Form("hidIndex" & i)
        Next

        For i = 0 To aryIQC.Count - 1
            If aryIQC(i)(0) <> "" Or aryIQC(i)(1) <> "" Or aryIQC(i)(2) <> "" Then
                aryTemp.Add(New String() {aryIQC(i)(0), aryIQC(i)(1), aryIQC(i)(2), aryIQC(i)(3), aryIQC(i)(4)})
            End If
        Next

        aryTemp = aryTemp
        For i = 0 To aryTemp.Count - 1
            If aryTemp(i)(0) = "" Then
                strMsg = "IQC Test Type Label is required."
                blnContinue = False
                Exit For
            Else
                If Not IsValidAlphaNumericInput(Trim(aryTemp(i)(0))) Then
                    strMsg = "Please enter valid characters for your IQC Test Type Label."
                    blnContinue = False
                    Exit For
                End If
            End If

            If aryTemp(i)(1) = "" Then
                strMsg = "IQC Prefix is required."
                blnContinue = False
                Exit For
            Else
                If Not IsValidAlphaNumericInput(Trim(aryTemp(i)(1))) Then
                    strMsg = "Please enter valid characters for your IQC Prefix."
                    blnContinue = False
                    Exit For
                End If
            End If


            If aryTemp(i)(2) = "" Then
                strMsg = "IQC Last Used No is required."
                blnContinue = False
                Exit For
            Else
                If Not IsNumeric(Trim(aryTemp(i)(2))) Then
                    strMsg = "IQC Last Used No is expecting numeric value."
                    blnContinue = False
                    Exit For
                Else
                    If Not Regex.IsMatch(Trim(aryTemp(i)(2)), "^[0-9]{0,10}$") Then
                        strMsg = "IQC Last Used No is expecting numeric value."
                        blnContinue = False
                        Exit For
                    End If

                End If
            End If

        Next

        For i = 0 To aryTemp.Count - 1
            For j = 0 To aryTemp.Count - 1
                If i <> j And aryTemp(i)(0) = aryTemp(j)(0) Then
                    strMsg = "IQC Test Type label must be unique."
                    blnContinue = False
                    Exit For
                End If
            Next
        Next

        If blnContinue = False Then
            Session("aryIQC") = aryIQC
            ConstructTable(aryIQC)
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
            Return False
        End If

        c = 3
        If c > aryTemp.Count Then
            c = 3
        Else
            c = aryTemp.Count
        End If

        For i = 0 To c - 1
            aryTemp2.Add(New String() {"", "", "", "", ""})
        Next

        j = 0
        For i = 0 To aryTemp.Count - 1
            If aryTemp(i)(0) <> "" And aryTemp(i)(1) <> "" And aryTemp(i)(2) <> "" Then
                aryTemp2(j)(0) = aryTemp(i)(0)
                aryTemp2(j)(1) = aryTemp(i)(1)
                aryTemp2(j)(2) = aryTemp(i)(2)
                aryTemp2(j)(3) = aryTemp(i)(3)
                aryTemp2(j)(4) = aryTemp(i)(4)
                j = j + 1
            End If
        Next

        Session("aryIQC") = aryTemp2
        ConstructTable(aryTemp2)

        Return True
    End Function
End Class
