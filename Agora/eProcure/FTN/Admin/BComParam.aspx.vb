Imports AgoraLegacy
Imports eProcure.Component
Public Class BComParamFTN
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
    Protected WithEvents cmd_Reset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents ChkFreeForm As System.Web.UI.WebControls.CheckBox
    Protected WithEvents ChkConsolidation As System.Web.UI.WebControls.CheckBox
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents ChkLevelsRec As System.Web.UI.WebControls.CheckBox
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents cboAO As System.Web.UI.WebControls.DropDownList
    Dim valchkstate As String
    Dim chkval As Boolean
    Dim intstate As Integer
    Protected WithEvents hidMode As System.Web.UI.HtmlControls.HtmlInputHidden
    Dim clsAdmin As New Admin

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
            displayParam()
            ViewState("Side") = Request.Params("side")
            ViewState("type") = "PO"
            BindGroupAO()
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

        'If Session("Env") = "FTN" Then
        '    ChkConsolidation.Style("display") = "none"
        '    ChkLevelsRec.Style("display") = "none"
        'End If
        ChkConsolidation.Style("display") = "none"
        ChkLevelsRec.Style("display") = "none"
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
        If Radiobuttonlist1.SelectedValue = "P" And Me.cboAO.SelectedItem.Text = "---Select---" Then
            Common.NetMsgbox(Me, "Please select purchasing officer.", MsgBoxStyle.Information)
            Return False
        End If
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

    Private Sub cmd_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_save.Click

        If Not validateInput() Then Exit Sub

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

        htList.Add("RFQLastUsedNo", txt_RfqLun.Text)
        htList.Add("PRLastUsedNo", txt_PrLun.Text)
        htList.Add("POLastUsedNo", txt_PoLun.Text)
        htList.Add("GRNLastUsedNo", txt_GrnLun.Text)
        htList.Add("SRNLastUsedNo", txt_SrnLun.Text)
        htList.Add("PaymentLastUsedNo", txt_PayLun.Text)
        htList.Add("TenderLastUsedNo", txt_TenLun.Text)
        htList.Add("CRLastUsedNo", txt_DebitLun.Text)
        htList.Add("IRLastUsedNo", txt_IRLun.Text)
        htList.Add("ITLastUsedNo", txt_ITLun.Text)

        If hidMode.Value = "1" And ChkConsolidation.Checked = False Then
            clsAdmin.SetNULL_consol()
        End If

        clsAdmin.updateParam(htList, opl_RfqOp.SelectedItem.Value, IIf(ChkFreeForm.Checked, 1, 0), IIf(ChkConsolidation.Checked, 1, 0), IIf(ChkLevelsRec.Checked, 1, 0), Radiobuttonlist1.SelectedItem.Value, cboAO.SelectedItem.Value)
        Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
        displayParam()
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
                End Select
            Else
                Select Case dsParam.Tables(0).Rows(i)(2)
                    Case "PR"
                        txt_PrLun.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    Case "PO"
                        txt_PoLun.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    Case "GRN"
                        txt_GrnLun.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    Case "RFQ"
                        txt_RfqLun.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    Case "Payment"
                        txt_PayLun.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    Case "Tender"
                        txt_TenLun.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    Case "SRN"
                        txt_SrnLun.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    Case "CR"
                        txt_DebitLun.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    Case "IR"
                        txt_IRLun.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                    Case "IT"
                        txt_ITLun.Text = Common.parseNull(dsParam.Tables(0).Rows(i)(1))
                End Select
            End If
        Next

        If dsParam.Tables(1).Rows.Count > 0 Then
            opl_RfqOp.SelectedValue = dsParam.Tables(1).Rows(0)("CM_RFQ_OPTION")
            Radiobuttonlist1.SelectedValue = dsParam.Tables(1).Rows(0)("CM_CONTR_PR_SETTING")

            If dsParam.Tables(1).Rows(0)("CM_CONTR_PR_SETTING") = "P" Then
                cboAO.Enabled = True
                cboAO.SelectedValue = dsParam.Tables(1).Rows(0)("CM_CONTR_PR_PO_OWNER_ID")
            Else

                cboAO.Enabled = False
            End If
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
                ViewState("valchkstate") = dsParam.Tables(2).Rows(i)("CS_FLAG_VALUE")
                If dsParam.Tables(2).Rows(i)("CS_FLAG_VALUE") = 1 Then
                    ChkConsolidation.Checked = True
                    hidMode.Value = ViewState("valchkstate")
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
End Class
