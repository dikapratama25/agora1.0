Imports AgoraLegacy
Imports eProcure.Component
Public Class AsgAppOfficer
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objApp As New ApprWorkFlow
    Dim valstrgrp, valgrpName, valstrgrpindex, valstrmode As String
    Dim strIndex, strlevel, strAO, strAAO As String
    Dim strEmpty As String
    Protected WithEvents cmd_delete As System.Web.UI.WebControls.Button
    Protected WithEvents cmdsave As System.Web.UI.WebControls.Button
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents cmd_reset As System.Web.UI.WebControls.Button
    Protected WithEvents lblDisAppGroup As System.Web.UI.WebControls.Label
    Protected WithEvents Hidcbocheck As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents RangeValidator1 As System.Web.UI.WebControls.RangeValidator
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents validate_AAO As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents hidchk As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblRelief As System.Web.UI.WebControls.Label
    Protected WithEvents hidMode As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents Table3 As System.Web.UI.HtmlControls.HtmlTable


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txt_AddCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents txtLevel As System.Web.UI.WebControls.TextBox
	'Modified for IPP Stage 2A - CH
    Protected WithEvents txtBranchCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCostCenter As System.Web.UI.WebControls.TextBox
    Protected WithEvents validate_AO As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents validate_BranchCode As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents validate_CostCenter As System.Web.UI.WebControls.RequiredFieldValidator
	'-------------------------------
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents rdRelief As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents cboAO As System.Web.UI.WebControls.DropDownList
    Protected WithEvents chkmass As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cboAAO As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboAAO2 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboAAO3 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboAAO4 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents validate_Level As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents hidAAO2 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents hidAAO3 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents hidAAO4 As System.Web.UI.HtmlControls.HtmlTableRow
	'Modified for IPP Stage 2A - CH
    Protected WithEvents tr1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr2 As System.Web.UI.HtmlControls.HtmlTableRow
	'-------------------------------

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        cmdsave.Enabled = False
        cmd_delete.Enabled = False
        'cmd_reset.Enabled = False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmdsave)
        htPageAccess.Add("add", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdsave)
        htPageAccess.Add("update", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmd_delete)
        htPageAccess.Add("delete", alButtonList)
        CheckButtonAccess()
        '//additional checking       
        alButtonList.Clear()
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)
        If Not IsPostBack Then

            Me.cboAAO.Attributes.Add("onchange", "check();")
            Me.cboAO.Attributes.Add("onchange", "check();")

            ViewState("Mode") = Me.Request.QueryString("Mode")
            ViewState("strchk") = Request.QueryString("chk")
            ViewState("strfrm") = Request.QueryString("frm")
            ViewState("type") = Request.QueryString("type")
            ViewState("apprType") = Request.QueryString("apprType")

            If ViewState("apprType") = "INV" Then
                rdRelief.Style.Item("display") = "none"
                lblRelief.Style.Item("display") = "none"
            ElseIf ViewState("apprType") = "E2P" Then
                rdRelief.Style.Item("display") = "none"
                lblRelief.Style.Item("display") = "none"
                'mimi 2018/05/14 : removes level approval of Ao for IPP
                'Michelle (27/2/2013) - Only allow 1 level of AO for IPP (Issue 1694)
                'If ViewState("type") = "AO" Then
                '    txtLevel.Text = "1"
                '    txtLevel.Enabled = False
                'Else
                '    hidAAO2.Style.Item("display") = "inline"
                '    hidAAO3.Style.Item("display") = "inline"
                '    hidAAO4.Style.Item("display") = "inline"
                'End If
            ElseIf ViewState("apprType") = "IQC" Then
                chkmass.Visible = False
                rdRelief.Style.Item("display") = "inline"
                lblRelief.Style.Item("display") = "inline"

			'Modified for IPP Stage 2A - CH
            ElseIf ViewState("apprType") = "BIL" Then
                rdRelief.Style.Item("display") = "none"
                lblRelief.Style.Item("display") = "none"
                txtLevel.Text = "1"
                txtLevel.Enabled = False
                tr1.Style("display") = "inline"
                tr2.Style("display") = "inline"
                validate_BranchCode.Enabled = True
                validate_CostCenter.Enabled = True
			'------------------------------
            Else
                rdRelief.Style.Item("display") = "inline"
                lblRelief.Style.Item("display") = "inline"

            End If

            If ViewState("Mode") = "A" Then
                ViewState("strgrpName") = Request.QueryString("AppGrp")
                ViewState("valstrgrpindex") = Request.QueryString("AppGrpIndex")
                ViewState("valgrpName") = Request.QueryString("AppGrp")
                lblDisAppGroup.Text = ViewState("valgrpName")
                cmd_reset.Text = "Clear"
                cmd_delete.Visible = False
            Else
                ViewState("strgrpName") = Request.QueryString("grpName")
                lblDisAppGroup.Text = ViewState("strgrpName")
                ViewState("valstrgrpindex") = Request.QueryString("Index")
                ViewState("strlevel") = Request.QueryString("Level")
                ViewState("strAO") = Request.QueryString("AO")
                ViewState("strAAO") = Request.QueryString("AAO")
				'Modified for IPP Stage 2A - CH
                ViewState("strBC") = Request.QueryString("bc")
                ViewState("strCC") = Request.QueryString("cc")
                '------------------------------
                If ViewState("apprType") = "E2P" Then
                    ViewState("strAAO2") = Request.QueryString("AAO2")
                    ViewState("strAAO3") = Request.QueryString("AAO3")
                    ViewState("strAAO4") = Request.QueryString("AAO4")
                End If
                ViewState("strRelief") = Request.QueryString("Relief")

                If IsNumeric(ViewState("strlevel")) Then
                    txtLevel.Text = ViewState("strlevel")
                Else
                    txtLevel.Text = ViewState("strlevel")
                    txtLevel.Text = txtLevel.Text.Substring(5)
                End If

				'Modified for IPP Stage 2A - CH
                txtBranchCode.Text = ViewState("strBC")
                txtCostCenter.Text = ViewState("strCC")
				'------------------------------
                txtLevel.Enabled = False
                cmd_reset.Enabled = True
                cmd_reset.Text = "Reset"
                'cmd_delete.Visible = True

                If ViewState("strRelief") = "Open" Then
                    rdRelief.SelectedValue = "O"
                ElseIf ViewState("strRelief") = "Controlled" Then
                    rdRelief.SelectedValue = "C"
                Else
                    rdRelief.SelectedValue = "O"
                End If

                Select Case ViewState("type")
                    Case "AO", "IQCASTS", "IQCV", "IQCPA", "IQCA"
                        lblTitle.Text = "Select Approving Officer"
                    Case "FO"
                        lblTitle.Text = "Select Approving Financial Officer"
                    Case "FM"
                        lblTitle.Text = "Select Approving Financial Manager"
					'Modified for IPP Stage 2A - CH
                    Case "BILAO"
                        lblTitle.Text = "Select Billing Approving Officer"
					'-------------------------------
                End Select
            End If
            If ViewState("apprType") = "E2P" Then
                BindGroupIPPAO()
                BindGroupIPPAAO()
                BindGroupIPPAAO2()
            Else
                BindGroupAAO()
                BindGroupAO()
                CheckMassApp()
            End If

			'Modified for IPP Stage 2A - CH
            PopulateTypeAhead()
			'-------------------------------
        End If

        'cmd_delete.Attributes.Add("onclick", "return confirm('" & MsgForDeleteButton & "');")
    End Sub
    Public Function BindGroupAO()

        'modified by esther on 30/09/05
        'when there was no value in cboAO errors found when called checkmassapp    
        '
        Dim cbolist As New ListItem
        Dim dvCustom As DataView

        Select Case viewstate("type")
            Case "AO", "MRS"
                dvCustom = objApp.getcboAO()
			'Modified for IPP Stage 2A - CH
            Case "BILAO"
                dvCustom = objApp.getcboAO("BIL")
			'--------------------------------
            Case "FO"
                dvCustom = objApp.getcboFO()
            Case "FM"
                dvCustom = objApp.getcboFM()
            Case "IQCASTS", "IQCV", "IQCPA", "IQCA"
                dvCustom = objApp.getcboQCO()
                'Zulham 02082018 - PAMB
            Case "PAO"
                dvCustom = objApp.getcboPAO()
        End Select

        cboAO.Items.Clear()
        If Not dvCustom Is Nothing Then
            Common.FillDdl(cboAO, "two", "UM_USER_ID", dvCustom)
            If viewstate("strAO") <> "" Then
                Common.SelDdl(viewstate("strAO"), cboAO, True, False)
            End If
        End If
        cbolist.Value = ""
        cbolist.Text = "---Select---"
        cboAO.Items.Insert(0, cbolist)

    End Function

    Public Function BindGroupAAO()
        Dim cbolist As New ListItem
        Dim dvCustom As DataView

        Select Case viewstate("type")
            Case "AO", "MRS"
                dvCustom = objApp.getcboAO()
			'Modified for IPP Stage 2A - CH
            Case "BILAO"
                dvCustom = objApp.getcboAO("BIL")
			'-------------------------------
            Case "FO"
                dvCustom = objApp.getcboFO()
            Case "FM"
                dvCustom = objApp.getcboFM()
            Case "IQCASTS", "IQCV", "IQCPA", "IQCA"
                dvCustom = objApp.getcboQCO()
            'Zulham 02082018 - PAMB
            Case "PAO"
                dvCustom = objApp.getcboPAO()
        End Select

        cboAAO.Items.Clear()
        If Not dvCustom Is Nothing Then
            Common.FillDdl(cboAAO, "two", "UM_USER_ID", dvCustom)
            If viewstate("strAAO") <> "" Then
                Common.SelDdl(viewstate("strAAO"), cboAAO, True, False)
                'cbolist.Value = ""
                'cbolist.Text = "---Select---"
                'cboAAO.Items.Insert(0, cbolist)

            End If
        End If
        cbolist.Value = ""
        cbolist.Text = "---Select---"
        cboAAO.Items.Insert(0, cbolist)


    End Function
    Public Function BindGroupIPPAAO2()
        Dim cbolist As New ListItem
        Dim dvCustom As DataView

        Select Case ViewState("type")
            Case "AO"
                dvCustom = objApp.getcboAO(ViewState("apprType"))
            Case "FO"
                dvCustom = objApp.getcboFO()
            Case "FM"
                dvCustom = objApp.getcboFM()
        End Select

        cboAAO2.Items.Clear()
        cboAAO3.Items.Clear()
        cboAAO4.Items.Clear()
        If Not dvCustom Is Nothing Then
            Common.FillDdl(cboAAO2, "two", "UM_USER_ID", dvCustom)
            If ViewState("strAAO2") <> "" Then
                Common.SelDdl(ViewState("strAAO2"), cboAAO2, True, False)
                'cbolist.Value = ""
                'cbolist.Text = "---Select---"
                'cboAAO.Items.Insert(0, cbolist)

            End If
            Common.FillDdl(cboAAO3, "two", "UM_USER_ID", dvCustom)
            If ViewState("strAAO3") <> "" Then
                Common.SelDdl(ViewState("strAAO3"), cboAAO3, True, False)
                'cbolist.Value = ""
                'cbolist.Text = "---Select---"
                'cboAAO.Items.Insert(0, cbolist)

            End If
            Common.FillDdl(cboAAO4, "two", "UM_USER_ID", dvCustom)
            If ViewState("strAAO4") <> "" Then
                Common.SelDdl(ViewState("strAAO4"), cboAAO4, True, False)
                'cbolist.Value = ""
                'cbolist.Text = "---Select---"
                'cboAAO.Items.Insert(0, cbolist)

            End If
        End If
        cbolist.Value = ""
        cbolist.Text = "---Select---"
        cboAAO2.Items.Insert(0, cbolist)

        cbolist.Value = ""
        cbolist.Text = "---Select---"
        cboAAO3.Items.Insert(0, cbolist)

        cbolist.Value = ""
        cbolist.Text = "---Select---"
        cboAAO4.Items.Insert(0, cbolist)

    End Function
    Public Function BindGroupIPPAAO()
        Dim cbolist As New ListItem
        Dim dvCustom As DataView

        Select Case ViewState("type")
            Case "AO"
                dvCustom = objApp.getcboAO(ViewState("apprType"))
            Case "FO"
                dvCustom = objApp.getcboFO()
            Case "FM"
                dvCustom = objApp.getcboFM()
        End Select

        cboAAO.Items.Clear()
        If Not dvCustom Is Nothing Then
            Common.FillDdl(cboAAO, "two", "UM_USER_ID", dvCustom)
            If ViewState("strAAO") <> "" Then
                Common.SelDdl(ViewState("strAAO"), cboAAO, True, False)
                'cbolist.Value = ""
                'cbolist.Text = "---Select---"
                'cboAAO.Items.Insert(0, cbolist)

            End If
        End If
        cbolist.Value = ""
        cbolist.Text = "---Select---"
        cboAAO.Items.Insert(0, cbolist)


    End Function
    Public Function BindGroupIPPAO()
        Dim cbolist As New ListItem
        Dim dvCustom As DataView

        Select Case ViewState("type")
            Case "AO"
                dvCustom = objApp.getcboAO(ViewState("apprType"))
            Case "FAO"
                dvCustom = objApp.getcboFAO()
            Case "FO"
                dvCustom = objApp.getcboFO()
            Case "FM"
                dvCustom = objApp.getcboFM()
        End Select

        cboAAO.Items.Clear()
        If Not dvCustom Is Nothing Then
            Common.FillDdl(cboAO, "two", "UM_USER_ID", dvCustom)
            If ViewState("strAO") <> "" Then
                Common.SelDdl(ViewState("strAO"), cboAO, True, False)
                'cbolist.Value = ""
                'cbolist.Text = "---Select---"
                'cboAAO.Items.Insert(0, cbolist)

            End If

        End If
        cbolist.Value = ""
        cbolist.Text = "---Select---"
        cboAO.Items.Insert(0, cbolist)


    End Function
    'CHECK FOR APP LIMIT AMOUNT IN USER MSTR
    Function CheckAPPAmount() As Boolean
        Dim objApp As New ApprWorkFlow
        Dim i, intcount, intLevel As Integer
        Dim dsLimit As New DataSet
        intLevel = CInt(txtLevel.Text)

        Dim objAppAmount As New AppAmountCurrent

        If ViewState("apprType") = "SC" Then
            CheckAPPAmount = True
            Exit Function
        End If

        Select Case ViewState("type")
            'Zulham 02082017 - PAMB
            Case "AO", "PAO"
                If ViewState("apprType") = "MRS" Then
                    CheckAPPAmount = True
                    Exit Function
                ElseIf ViewState("apprType") = "E2P" Then
                    'Return True 'mimi 2018-05-16 : modified checking for AO app limit
                    dsLimit = objApp.get_AppIPPLIMIT(ViewState("AO_ID"), ViewState("valstrgrpindex"), txtLevel.Text, ViewState("apprType"), ViewState("type"))
                    objApp.get_IPPAppLimit(objAppAmount, cboAO.SelectedItem.Value)
                Else
                    'zulham 03082018 - PAMB
                    If ViewState("type") = "PAO" Then
                        dsLimit = objApp.get_PAOAppLIMIT(ViewState("AO_ID"), ViewState("valstrgrpindex"), txtLevel.Text, ViewState("apprType"))
                    Else
                        dsLimit = objApp.get_AppLIMIT(ViewState("AO_ID"), ViewState("valstrgrpindex"), txtLevel.Text, ViewState("apprType"))
                    End If
                    objApp.get_AppLimitAO(objAppAmount, cboAO.SelectedItem.Value, ViewState("apprType"))
                End If
            Case "FO"
                If ViewState("apprType") = "E2P" Then
                    'If FO role, No checking for Approval Limit
                    Return True
                    dsLimit = objApp.get_AppIPPLIMIT(ViewState("AO_ID"), ViewState("valstrgrpindex"), txtLevel.Text, ViewState("apprType"), ViewState("type"))
                    objApp.get_IPPAppLimit(objAppAmount, cboAO.SelectedItem.Value)
                Else
                    dsLimit = objApp.get_AppFOLIMIT(ViewState("AO_ID"), ViewState("valstrgrpindex"), txtLevel.Text)
                    objApp.get_AppLimitFO(objAppAmount, cboAO.SelectedItem.Value)
                End If
            Case "FM", "IQCASTS", "IQCV", "IQCPA", "IQCA", "BILAO" 'Modified for IPP Stage 2A - CH
                CheckAPPAmount = True
                Exit Function
        End Select

        Dim currentLimit As String = objAppAmount.Appval
        CheckAPPAmount = True

        For i = 0 To dsLimit.Tables(0).Rows.Count - 1
            '//Check whether got redundant level

            If Not IsDBNull(dsLimit.Tables(0).Rows(i)("UM_APP_LIMIT")) Then
                If intLevel > CInt(dsLimit.Tables(0).Rows(i)("AGA_SEQ")) Then
                    If (dsLimit.Tables(0).Rows(i)("UM_APP_LIMIT")) > currentLimit Then
                        CheckAPPAmount = False
                        Exit Function
                    End If
                End If

                If intLevel < CInt(dsLimit.Tables(0).Rows(i)("AGA_SEQ")) Then
                    If (dsLimit.Tables(0).Rows(i)("UM_APP_LIMIT")) < currentLimit Then
                        CheckAPPAmount = False
                        Exit Function
                    End If
                End If
            Else
                CheckAPPAmount = False
                Exit Function
            End If

        Next
    End Function

    Private Sub cmdsave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdsave.Click
        Dim strmsg As String
        Dim intmsgno As Integer
        Dim strRedirect, strWhose, strMsg1 As String
        Dim blnBuyer As Boolean

        If cboAO.SelectedItem.Text = "---Select---" Then
            validate_AO.Enabled = True
        ElseIf cboAO.SelectedItem.Text = cboAAO.SelectedItem.Text Then
            Common.NetMsgbox(Me, "The Approving Officer must not be the same as the Alternative Approving Officer", MsgBoxStyle.Information)
        ElseIf ViewState("Mode") = "A" Then
            'Zulham 02082018 - PAMB
            If ViewState("type") = "AO" Or ViewState("type") = "BILAO" Or ViewState("type") = "PAO" Then 'Modified for IPP Stage 2A - CH
                If checkIsBuyer() Then
                    Exit Sub
                End If
            End If

            'Modified for IPP Stage 2A - CH
            If ViewState("type") = "BILAO" Then
                If checkFields() = True Then
                    Exit Sub
                End If
            End If
            '-------------------------------

            If CheckAPPAmount() = True Then
                If ViewState("apprType") = "E2P" Then
                    Select Case ViewState("type")
                        Case "AO"
                            intmsgno = objApp.save_Ao_AAO(ViewState("Mode"), ViewState("valstrgrpindex"), txtLevel.Text, cboAO.SelectedItem.Value, cboAAO.SelectedItem.Value, rdRelief.SelectedItem.Value)
                        Case "FO"
                            intmsgno = objApp.save_FO_AFO_IPP(ViewState("Mode"), ViewState("valstrgrpindex"), txtLevel.Text, cboAO.SelectedItem.Value, cboAAO.SelectedItem.Value, cboAAO2.SelectedItem.Value, cboAAO3.SelectedItem.Value, cboAAO4.SelectedItem.Value, rdRelief.SelectedItem.Value)
                        Case "FM"
                            intmsgno = objApp.save_FM_AFM_IPP(ViewState("Mode"), ViewState("valstrgrpindex"), txtLevel.Text, cboAO.SelectedItem.Value, cboAAO.SelectedItem.Value, cboAAO2.SelectedItem.Value, cboAAO3.SelectedItem.Value, cboAAO4.SelectedItem.Value, rdRelief.SelectedItem.Value)
                    End Select
                ElseIf ViewState("apprType") = "IQC" Then
                    intmsgno = objApp.save_Ao_AAO_IQC(ViewState("Mode"), ViewState("valstrgrpindex"), txtLevel.Text, cboAO.SelectedItem.Value, cboAAO.SelectedItem.Value, rdRelief.SelectedItem.Value, ViewState("type"))
                Else
                    Select Case ViewState("type")
                        Case "AO"
                            intmsgno = objApp.save_Ao_AAO(ViewState("Mode"), ViewState("valstrgrpindex"), txtLevel.Text, cboAO.SelectedItem.Value, cboAAO.SelectedItem.Value, rdRelief.SelectedItem.Value)
                            'Modified for IPP Stage 2A - CH
                        Case "BILAO"
                            intmsgno = objApp.save_Ao_AAO(ViewState("Mode"), ViewState("valstrgrpindex"), txtLevel.Text, cboAO.SelectedItem.Value, cboAAO.SelectedItem.Value, rdRelief.SelectedItem.Value, , , txtBranchCode.Text, txtCostCenter.Text)
                            '--------------------------------
                        Case "FO"
                            intmsgno = objApp.save_FO_AFO(ViewState("Mode"), ViewState("valstrgrpindex"), txtLevel.Text, cboAO.SelectedItem.Value, cboAAO.SelectedItem.Value, rdRelief.SelectedItem.Value)
                        Case "FM"
                            intmsgno = objApp.save_FM_AFM(ViewState("Mode"), ViewState("valstrgrpindex"), txtLevel.Text, cboAO.SelectedItem.Value, cboAAO.SelectedItem.Value, rdRelief.SelectedItem.Value)
                            'ZULHAM 02082018 - PAMB
                        Case "PAO"
                            intmsgno = objApp.save_PAO_APAO(ViewState("Mode"), ViewState("valstrgrpindex"), txtLevel.Text, cboAO.SelectedItem.Value, cboAAO.SelectedItem.Value, rdRelief.SelectedItem.Value)
                    End Select
                End If


                Select Case intmsgno
                    Case WheelMsgNum.Save
                        strmsg = MsgRecordSave
                        txtLevel.Text = ""
                        'Modified for IPP Stage 2A - CH
                        txtBranchCode.Text = ""
                        txtCostCenter.Text = ""
                        '-----------------------------
                        cboAO.SelectedIndex = 0
                        cboAAO.SelectedIndex = 0
                        cboAAO2.SelectedIndex = 0
                        cboAAO3.SelectedIndex = 0
                        cboAAO4.SelectedIndex = 0
                    Case WheelMsgNum.Duplicate
                        strmsg = MsgRecordDuplicate
                    Case WheelMsgNum.NotSave
                        strmsg = MsgRecordNotSave
                    Case -1
                        strmsg = "A"
                    Case -2
                        strmsg = "B"
                End Select
                Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)
            Else
                ' strmsg = "Please select an Approving Officer who has higher approval limit."
                strmsg = "Please select an Approving Officer whose approval limit is > the previous AO and < the next AO."
                Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)
            End If
        Else 'Modify
            If ViewState("type") = "AO" Or ViewState("type") = "BILAO" Then 'Modified for IPP Stage 2A - CH
                If checkIsBuyer() Then
                    Exit Sub
                End If
            End If

			'Modified for IPP Stage 2A - CH
            If ViewState("type") = "BILAO" Then
                If checkFields() = True Then
                    Exit Sub
                End If
            End If
			'-------------------------------

            If CheckAPPAmount() = True Then
                'strRedirect = "AppGrpAsg.aspx?pageid=" & strPageId
                'strRedirect = "AppGrpAsg.aspx?grp=" & Server.UrlEncode(ViewState("strgrpName")) & "&chk=" & ViewState("strchk") & "&frm=" & ViewState("strfrm") & "&apprType=" & ViewState("apprType") & "&pageid=" & strPageId
                If ViewState("apprType") = "E2P" Then
                    Select Case ViewState("type")
                        Case "AO"
                            intmsgno = objApp.save_Ao_AAO(ViewState("Mode"), ViewState("valstrgrpindex"), txtLevel.Text, cboAO.SelectedItem.Value, cboAAO.SelectedItem.Value, rdRelief.SelectedItem.Value, ViewState("strAO"), ViewState("strAAO"))
                        Case "FO"
                            intmsgno = objApp.save_FO_AFO_IPP(ViewState("Mode"), ViewState("valstrgrpindex"), txtLevel.Text, cboAO.SelectedItem.Value, cboAAO.SelectedItem.Value, cboAAO2.SelectedItem.Value, cboAAO3.SelectedItem.Value, cboAAO4.SelectedItem.Value, rdRelief.SelectedItem.Value, ViewState("strAO"), ViewState("strAAO"))
                        Case "FM"
                            intmsgno = objApp.save_FM_AFM_IPP(ViewState("Mode"), ViewState("valstrgrpindex"), txtLevel.Text, cboAO.SelectedItem.Value, cboAAO.SelectedItem.Value, cboAAO2.SelectedItem.Value, cboAAO3.SelectedItem.Value, cboAAO4.SelectedItem.Value, rdRelief.SelectedItem.Value, ViewState("strAO"), ViewState("strAAO"))
                    End Select
                ElseIf ViewState("apprType") = "IQC" Then
                    intmsgno = objApp.save_Ao_AAO_IQC(ViewState("Mode"), ViewState("valstrgrpindex"), txtLevel.Text, cboAO.SelectedItem.Value, cboAAO.SelectedItem.Value, rdRelief.SelectedItem.Value, ViewState("type"), ViewState("strAO"), ViewState("strAAO"))
                Else
                    Select Case ViewState("type")
                        Case "AO"
                            intmsgno = objApp.save_Ao_AAO(ViewState("Mode"), ViewState("valstrgrpindex"), txtLevel.Text, cboAO.SelectedItem.Value, cboAAO.SelectedItem.Value, rdRelief.SelectedItem.Value, ViewState("strAO"), ViewState("strAAO"))
						'Modified for IPP Stage 2A - CH
                        Case "BILAO"
                            intmsgno = objApp.save_Ao_AAO(ViewState("Mode"), ViewState("valstrgrpindex"), txtLevel.Text, cboAO.SelectedItem.Value, cboAAO.SelectedItem.Value, rdRelief.SelectedItem.Value, ViewState("strAO"), ViewState("strAAO"), txtBranchCode.Text, txtCostCenter.Text)
						'------------------------------
                        Case "FO"
                            intmsgno = objApp.save_FO_AFO(ViewState("Mode"), ViewState("valstrgrpindex"), txtLevel.Text, cboAO.SelectedItem.Value, cboAAO.SelectedItem.Value, rdRelief.SelectedItem.Value, ViewState("strAO"), ViewState("strAAO"))
                        Case "FM"
                            intmsgno = objApp.save_FM_AFM(ViewState("Mode"), ViewState("valstrgrpindex"), txtLevel.Text, cboAO.SelectedItem.Value, cboAAO.SelectedItem.Value, rdRelief.SelectedItem.Value, ViewState("strAO"), ViewState("strAAO"))
                            'Zulham 04082018 - PAMB
                        Case "PAO"
                            intmsgno = objApp.save_PAO_APAO(ViewState("Mode"), ViewState("valstrgrpindex"), txtLevel.Text, cboAO.SelectedItem.Value, cboAAO.SelectedItem.Value, rdRelief.SelectedItem.Value, ViewState("strAO"), ViewState("strAAO"))
                    End Select
                End If
                Select Case intmsgno
                    Case WheelMsgNum.Save
                        strmsg = MsgRecordSave
                        'txtLevel.Text = ""
                        'cboAO.SelectedIndex = 0
                        'cboAAO.SelectedIndex = 0
                        'Common.NetMsgbox(Me, strmsg, strRedirect, MsgBoxStyle.Information)
                        Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)

                    Case WheelMsgNum.Duplicate
                        strmsg = MsgRecordDuplicate
                        Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)
                    Case WheelMsgNum.NotSave
                        strmsg = MsgRecordNotSave
                        Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)
                End Select
            Else
                strmsg = "Please select an Approving Officer whose approval limit is > the previous AO and < the next AO."
                Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)
            End If
            End If
    End Sub

    Private Sub cmd_reset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_reset.Click
        If viewstate("Mode") = "A" Then
            If ViewState("apprType") <> "E2P" And ViewState("type") <> "AO" Then txtLevel.Text = ""
            cboAO.SelectedIndex = 0
            cboAAO.SelectedIndex = 0
			'Modified for IPP Stage 2A - CH
            txtBranchCode.Text = ""
            txtCostCenter.Text = ""
            '-------------------------------
            If ViewState("apprType") = "E2P" Then
                cboAAO2.SelectedIndex = 0
                cboAAO3.SelectedIndex = 0
                cboAAO4.SelectedIndex = 0
            End If

        Else
			'Modified for IPP Stage 2A - CH
            txtBranchCode.Text = ViewState("strBC")
            txtCostCenter.Text = ViewState("strCC")
            Common.SelDdl(ViewState("strAO"), cboAO, True, False)
			'-------------------------------
            Common.SelDdl(ViewState("strAAO"), cboAAO, True, False)
            If ViewState("apprType") = "E2P" Then
                Common.SelDdl(ViewState("strAAO2"), cboAAO, True, False)
                Common.SelDdl(ViewState("strAAO3"), cboAAO, True, False)
                Common.SelDdl(ViewState("strAAO4"), cboAAO, True, False)
            End If

            If ViewState("strRelief") = "Open" Then
                rdRelief.SelectedValue = "O"
            ElseIf ViewState("strRelief") = "Controlled" Then
                rdRelief.SelectedValue = "C"
            Else
                rdRelief.SelectedValue = "O"
            End If
        End If

    End Sub

    Private Sub cboAO_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboAO.SelectedIndexChanged
        CheckMassApp()
    End Sub

    Function CheckMassApp()
        Dim intchkmsg As Integer

        Select Case viewstate("type")
            Case "AO"
                intchkmsg = objApp.checkMassApp(cboAO.SelectedItem.Value)
            Case "FO"
                intchkmsg = objApp.checkFOMassApp(cboAO.SelectedItem.Value)
            Case "FM"
                intchkmsg = 1
            Case "IQCASTS", "IQCV", "IQCPA", "IQCA", "BILAO" 'Modified for IPP Stage 2A - CH
                intchkmsg = 0
        End Select

        Select Case intchkmsg
            Case 0
                chkmass.Checked = False
            Case 1
                chkmass.Checked = True
        End Select
    End Function

    Function checkIsBuyer() As Boolean
        Dim strWhose, strMsg1 As String
        Dim blnBuyer As Boolean
        strWhose = ""
        blnBuyer = objApp.isAssignedBuyer(viewstate("valstrgrpindex"), cboAO.SelectedItem.Value, cboAAO.SelectedItem.Value, strWhose)
        If blnBuyer Then
            If Split(strWhose, ",").Length = 2 Then 'both ao and aao
                strMsg1 = "Both selected Approving Officer and Alternative Approving Officer cannot be assigned because they are already assigned as Buyer in the same list."
            Else 'either ao or aao
                If strWhose = "ao" Then
                    strMsg1 = "Selected Approving Officer cannot be assigned because they are already assigned as Buyer in the same list."
                Else
                    strMsg1 = "Selected Alternative Approving Officer cannot be assigned because they are already assigned as Buyer in the same list."
                End If
            End If
            Common.NetMsgbox(Me, strMsg1, MsgBoxStyle.Information)
        End If
        checkIsBuyer = blnBuyer
    End Function

	'Modified for IPP Stage 2A - CH
    Function checkFields() As Boolean
        Dim objIPPMain As New IPPMain

        If objIPPMain.ChkValidBranchCode(txtBranchCode.Text) = False Then
            Common.NetMsgbox(Me, "Invalid Branch Code.", MsgBoxStyle.Information)
            Return True
        End If

        If objIPPMain.ChkValidCostCenter(txtCostCenter.Text) = False Then
            Common.NetMsgbox(Me, "Invalid Cost Center.", MsgBoxStyle.Information)
            Return True
        End If
       
        checkFields = False
    End Function

    Sub PopulateTypeAhead()
        Dim typeahead As String
        Dim i, count As Integer
        Dim content, content2 As String
        Dim strCompID As String
        Dim typeahead1 As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=CostCentre2&frm=AsgAppOfficer&btn=CCCode")
        Dim typeahead2 As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=BranchCode&frm=AsgAppOfficer")


        content &= "$(""#txtCostCenter"").autocomplete(""" & typeahead1 & """, {" & vbCrLf & _
        "width: 200," & vbCrLf & _
        "scroll: true," & vbCrLf & _
        "selectFirst: false" & vbCrLf & _
         "});" & vbCrLf & _
        "$(""#txtBranchCode"").autocomplete(""" & typeahead2 & """, {" & vbCrLf & _
        "width: 200," & vbCrLf & _
        "scroll: true," & vbCrLf & _
        "selectFirst: false" & vbCrLf & _
        "});" & vbCrLf

        typeahead = "<script language=""javascript"">" & vbCrLf & _
      "<!--" & vbCrLf & _
        "$(document).ready(function(){" & vbCrLf & _
        content & vbCrLf & _
        "});" & vbCrLf & _
        "-->" & vbCrLf & _
        "</script>"

        Session("typeahead") = typeahead
    End Sub
	'-------------------------------------
End Class
