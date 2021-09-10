Imports AgoraLegacy
Imports eProcure.Component
Public Class AsgAppOfficerFTN
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
    Protected WithEvents validate_AO As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents rdRelief As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents cboAO As System.Web.UI.WebControls.DropDownList
    Protected WithEvents chkmass As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cboAAO As System.Web.UI.WebControls.DropDownList
    Protected WithEvents validate_Level As System.Web.UI.WebControls.RequiredFieldValidator

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
            'If Session("Env") = "FTN" Then
            '    txtLevel.Text = 1
            '    txtLevel.Enabled = False
            '    rdRelief.Style.Item("display") = "none"
            '    lblRelief.Style.Item("display") = "none"
            '    ViewState("strgrpName") = Request.QueryString("AppGrp")
            '    ViewState("valstrgrpindex") = Request.QueryString("AppGrpIndex")
            '    ViewState("valgrpName") = Request.QueryString("AppGrp")
            '    lblDisAppGroup.Text = ViewState("valgrpName")
            '    cmd_reset.Visible = False
            '    cmd_delete.Visible = False
            '    Table3.Rows(0).Visible = False
            '    chkmass.Visible = False
            '    ViewState("type") = "AO"
            '    BindGroupAO()
            'Else

            '    Me.cboAAO.Attributes.Add("onchange", "check();")
            '    Me.cboAO.Attributes.Add("onchange", "check();")

            '    ViewState("Mode") = Me.Request.QueryString("Mode")
            '    ViewState("strchk") = Request.QueryString("chk")
            '    ViewState("strfrm") = Request.QueryString("frm")
            '    ViewState("type") = Request.QueryString("type")
            '    ViewState("apprType") = Request.QueryString("apprType")

            '    If ViewState("apprType") = "INV" Then
            '        rdRelief.Style.Item("display") = "none"
            '        lblRelief.Style.Item("display") = "none"
            '    Else
            '        rdRelief.Style.Item("display") = "inline"
            '        lblRelief.Style.Item("display") = "inline"
            '    End If

            '    If ViewState("Mode") = "A" Then
            '        ViewState("strgrpName") = Request.QueryString("AppGrp")
            '        ViewState("valstrgrpindex") = Request.QueryString("AppGrpIndex")
            '        ViewState("valgrpName") = Request.QueryString("AppGrp")
            '        lblDisAppGroup.Text = ViewState("valgrpName")
            '        cmd_reset.Text = "Clear"
            '        cmd_delete.Visible = False
            '    Else
            '        ViewState("strgrpName") = Request.QueryString("grpName")
            '        lblDisAppGroup.Text = ViewState("strgrpName")
            '        ViewState("valstrgrpindex") = Request.QueryString("Index")
            '        ViewState("strlevel") = Request.QueryString("Level")
            '        ViewState("strAO") = Request.QueryString("AO")
            '        ViewState("strAAO") = Request.QueryString("AAO")
            '        ViewState("strRelief") = Request.QueryString("Relief")

            '        If IsNumeric(ViewState("strlevel")) Then
            '            txtLevel.Text = ViewState("strlevel")
            '        Else
            '            txtLevel.Text = ViewState("strlevel")
            '            txtLevel.Text = txtLevel.Text.Substring(5)
            '        End If

            '        txtLevel.Enabled = False
            '        cmd_reset.Enabled = True
            '        cmd_reset.Text = "Reset"
            '        'cmd_delete.Visible = True

            '        If ViewState("strRelief") = "Open" Then
            '            rdRelief.SelectedValue = "O"
            '        ElseIf ViewState("strRelief") = "Controlled" Then
            '            rdRelief.SelectedValue = "C"
            '        Else
            '            rdRelief.SelectedValue = "O"
            '        End If

            '        Select Case ViewState("type")
            '            Case "AO"
            '                lblTitle.Text = "Select Approving Officer"
            '            Case "FO"
            '                lblTitle.Text = "Select Approving Financial Officer"
            '            Case "FM"
            '                lblTitle.Text = "Select Approving Financial Manager"
            '        End Select
            '    End If
            '    BindGroupAAO()
            '    BindGroupAO()
            '    CheckMassApp()
            'End If
            txtLevel.Text = 1
            txtLevel.Enabled = False
            rdRelief.Style.Item("display") = "none"
            lblRelief.Style.Item("display") = "none"
            ViewState("strgrpName") = Request.QueryString("AppGrp")
            ViewState("valstrgrpindex") = Request.QueryString("AppGrpIndex")
            ViewState("valgrpName") = Request.QueryString("AppGrp")
            lblDisAppGroup.Text = ViewState("valgrpName")
            cmd_reset.Visible = False
            cmd_delete.Visible = False
            Table3.Rows(0).Visible = False
            chkmass.Visible = False
            ViewState("type") = "AO"
            BindGroupAO()
        End If

        'cmd_delete.Attributes.Add("onclick", "return confirm('" & MsgForDeleteButton & "');")
    End Sub
    Public Function BindGroupAO()

        'modified by esther on 30/09/05
        'when there was no value in cboAO errors found when called checkmassapp    
        '
        Dim cbolist As New ListItem
        Dim dvCustom As DataView

        Select Case ViewState("type")
            Case "AO"
                dvCustom = objApp.getcboAO()
            Case "FO"
                dvCustom = objApp.getcboFO()
            Case "FM"
                dvCustom = objApp.getcboFM()
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

    Public Function BindGroupAAO()
        Dim cbolist As New ListItem
        Dim dvCustom As DataView

        Select Case ViewState("type")
            Case "AO"
                dvCustom = objApp.getcboAO()
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

    'Private Sub cmd_delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_delete.Click
    '    Dim strMsg As String
    '    Dim intMsgNo As Integer
    '    If txtLevel.Text = "" Then
    '        validate_Level.Enabled = True

    '    ElseIf viewstate("Mode") = "A" Then

    '        Select Case viewstate("type")
    '            Case "AO"
    '                intMsgNo = objApp.delAo_AAO(viewstate("valstrgrpindex"), txtLevel.Text)
    '            Case "FO"
    '                intMsgNo = objApp.delAo_AFO(viewstate("valstrgrpindex"), txtLevel.Text)
    '            Case "FM"
    '                intMsgNo = objApp.delAo_AFM(viewstate("valstrgrpindex"), txtLevel.Text)
    '        End Select

    '        Select Case intMsgNo
    '            Case WheelMsgNum.Delete
    '                strMsg = MsgRecordDelete
    '                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
    '            Case WheelMsgNum.NotDelete
    '                strMsg = MsgRecordNotDelete
    '                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
    '        End Select

    '    Else
    '        Select Case viewstate("type")
    '            Case "AO"
    '                intMsgNo = objApp.delAo_AAO(viewstate("valstrgrpindex"), txtLevel.Text)
    '            Case "FO"
    '                intMsgNo = objApp.delAo_AFO(viewstate("valstrgrpindex"), txtLevel.Text)
    '            Case "FM"
    '                intMsgNo = objApp.delAo_AFM(viewstate("valstrgrpindex"), txtLevel.Text)
    '        End Select

    '        Select Case intMsgNo
    '            Case WheelMsgNum.Delete
    '                strMsg = MsgRecordDelete
    '                'Common.NetMsgbox(Me, strMsg, "AppGrpAsg.aspx?grp=" & Server.UrlEncode(ViewState("strgrpName")) & "&chk=" & ViewState("strchk") & "&frm=" & ViewState("strfrm") & "&apprType=" & ViewState("apprType") & "&pageid=" & strPageId, MsgBoxStyle.Information)
    '                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)

    '            Case WheelMsgNum.NotDelete
    '                strMsg = MsgRecordNotDelete
    '                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
    '        End Select

    '    End If
    'End Sub
    'CHECK FOR APP LIMIT AMOUNT IN USER MSTR
    Function CheckAPPAmount() As Boolean
        Dim objApp As New ApprWorkFlow
        Dim i, intcount, intLevel As Integer
        Dim dsLimit As New DataSet
        intLevel = CInt(txtLevel.Text)

        Dim objAppAmount As New AppAmountCurrent

        Select Case ViewState("type")
            Case "AO"
                dsLimit = objApp.get_AppLIMIT(ViewState("AO_ID"), ViewState("valstrgrpindex"), txtLevel.Text)
                objApp.get_AppLimitAO(objAppAmount, cboAO.SelectedItem.Value)
            Case "FO"
                dsLimit = objApp.get_AppFOLIMIT(ViewState("AO_ID"), ViewState("valstrgrpindex"), txtLevel.Text)
                objApp.get_AppLimitFO(objAppAmount, cboAO.SelectedItem.Value)
            Case "FM"
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




        'If Session("Env") = "FTN" Then
        '    If CheckAPPAmount() = True Then

        '        Select Case ViewState("type")
        '            Case "AO"
        '                'intmsgno = objApp.save_Ao_AAO(ViewState("Mode"), ViewState("valstrgrpindex"), txtLevel.Text, cboAO.SelectedItem.Value, cboAAO.SelectedItem.Value, rdRelief.SelectedItem.Value)
        '                intmsgno = objApp.save_Ao_AAO("A", ViewState("valstrgrpindex"), txtLevel.Text, cboAO.SelectedItem.Value, "", "")
        '            Case "FO"
        '                intmsgno = objApp.save_FO_AFO(ViewState("Mode"), ViewState("valstrgrpindex"), txtLevel.Text, cboAO.SelectedItem.Value, cboAAO.SelectedItem.Value, rdRelief.SelectedItem.Value)
        '            Case "FM"
        '                intmsgno = objApp.save_FM_AFM(ViewState("Mode"), ViewState("valstrgrpindex"), txtLevel.Text, cboAO.SelectedItem.Value, cboAAO.SelectedItem.Value, rdRelief.SelectedItem.Value)
        '        End Select

        '        Select Case intmsgno
        '            Case WheelMsgNum.Save
        '                strmsg = MsgRecordSave
        '                'txtLevel.Text = ""
        '                cboAO.SelectedIndex = 0
        '                'cboAAO.SelectedIndex = 0
        '            Case WheelMsgNum.Duplicate
        '                strmsg = MsgRecordDuplicate
        '            Case WheelMsgNum.NotSave

        '        End Select
        '        If intmsgno = 0 Then

        '            strmsg = "The following buyer cannot be assigned because it was already assigned as Purchaser in the same list -- " & cboAO.SelectedItem.Text

        '        End If
        '        Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)
        '    Else
        '        ' strmsg = "Please select an Approving Officer who has higher approval limit."
        '        strmsg = "Please select an Approving Officer whose approval limit is > the previous AO and < the next AO."
        '        Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)
        '    End If
        'Else 'For non FTN
        '    If cboAO.SelectedItem.Text = "---Select---" Then
        '        validate_AO.Enabled = True
        '    ElseIf cboAO.SelectedItem.Text = cboAAO.SelectedItem.Text Then
        '        Common.NetMsgbox(Me, "The Approving Officer must not be the same as the Alternative Approving Officer", MsgBoxStyle.Information)
        '    ElseIf ViewState("Mode") = "A" Then
        '        If ViewState("type") = "AO" Then
        '            If checkIsBuyer() Then
        '                Exit Sub
        '            End If
        '        End If

        '        If CheckAPPAmount() = True Then

        '            Select Case ViewState("type")
        '                Case "AO"
        '                    intmsgno = objApp.save_Ao_AAO(ViewState("Mode"), ViewState("valstrgrpindex"), txtLevel.Text, cboAO.SelectedItem.Value, cboAAO.SelectedItem.Value, rdRelief.SelectedItem.Value)
        '                Case "FO"
        '                    intmsgno = objApp.save_FO_AFO(ViewState("Mode"), ViewState("valstrgrpindex"), txtLevel.Text, cboAO.SelectedItem.Value, cboAAO.SelectedItem.Value, rdRelief.SelectedItem.Value)
        '                Case "FM"
        '                    intmsgno = objApp.save_FM_AFM(ViewState("Mode"), ViewState("valstrgrpindex"), txtLevel.Text, cboAO.SelectedItem.Value, cboAAO.SelectedItem.Value, rdRelief.SelectedItem.Value)
        '            End Select

        '            Select Case intmsgno
        '                Case WheelMsgNum.Save
        '                    strmsg = MsgRecordSave
        '                    txtLevel.Text = ""
        '                    cboAO.SelectedIndex = 0
        '                    cboAAO.SelectedIndex = 0
        '                Case WheelMsgNum.Duplicate
        '                    strmsg = MsgRecordDuplicate
        '                Case WheelMsgNum.NotSave
        '                    strmsg = MsgRecordNotSave
        '            End Select
        '            Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)
        '        Else
        '            ' strmsg = "Please select an Approving Officer who has higher approval limit."
        '            strmsg = "Please select an Approving Officer whose approval limit is > the previous AO and < the next AO."
        '            Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)
        '        End If
        '    Else 'Modify
        '        If ViewState("type") = "AO" Then
        '            If checkIsBuyer() Then
        '                Exit Sub
        '            End If
        '        End If

        '        If CheckAPPAmount() = True Then
        '            'strRedirect = "AppGrpAsg.aspx?pageid=" & strPageId
        '            'strRedirect = "AppGrpAsg.aspx?grp=" & Server.UrlEncode(ViewState("strgrpName")) & "&chk=" & ViewState("strchk") & "&frm=" & ViewState("strfrm") & "&apprType=" & ViewState("apprType") & "&pageid=" & strPageId

        '            Select Case ViewState("type")
        '                Case "AO"
        '                    intmsgno = objApp.save_Ao_AAO(ViewState("Mode"), ViewState("valstrgrpindex"), txtLevel.Text, cboAO.SelectedItem.Value, cboAAO.SelectedItem.Value, rdRelief.SelectedItem.Value, ViewState("strAO"), ViewState("strAAO"))
        '                Case "FO"
        '                    intmsgno = objApp.save_FO_AFO(ViewState("Mode"), ViewState("valstrgrpindex"), txtLevel.Text, cboAO.SelectedItem.Value, cboAAO.SelectedItem.Value, rdRelief.SelectedItem.Value, ViewState("strAO"), ViewState("strAAO"))
        '                Case "FM"
        '                    intmsgno = objApp.save_FM_AFM(ViewState("Mode"), ViewState("valstrgrpindex"), txtLevel.Text, cboAO.SelectedItem.Value, cboAAO.SelectedItem.Value, rdRelief.SelectedItem.Value, ViewState("strAO"), ViewState("strAAO"))
        '            End Select

        '            Select Case intmsgno
        '                Case WheelMsgNum.Save
        '                    strmsg = MsgRecordSave
        '                    'txtLevel.Text = ""
        '                    'cboAO.SelectedIndex = 0
        '                    'cboAAO.SelectedIndex = 0
        '                    'Common.NetMsgbox(Me, strmsg, strRedirect, MsgBoxStyle.Information)
        '                    Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)

        '                Case WheelMsgNum.Duplicate
        '                    strmsg = MsgRecordDuplicate
        '                    Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)
        '                Case WheelMsgNum.NotSave
        '                    strmsg = MsgRecordNotSave
        '                    Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)
        '            End Select
        '        Else
        '            strmsg = "Please select an Approving Officer whose approval limit is > the previous AO and < the next AO."
        '            Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)
        '        End If
        '    End If
        'End If
        If CheckAPPAmount() = True Then

            Select Case ViewState("type")
                Case "AO"
                    'intmsgno = objApp.save_Ao_AAO(ViewState("Mode"), ViewState("valstrgrpindex"), txtLevel.Text, cboAO.SelectedItem.Value, cboAAO.SelectedItem.Value, rdRelief.SelectedItem.Value)
                    intmsgno = objApp.save_Ao_AAO("A", ViewState("valstrgrpindex"), txtLevel.Text, cboAO.SelectedItem.Value, "", "")
                Case "FO"
                    intmsgno = objApp.save_FO_AFO(ViewState("Mode"), ViewState("valstrgrpindex"), txtLevel.Text, cboAO.SelectedItem.Value, cboAAO.SelectedItem.Value, rdRelief.SelectedItem.Value)
                Case "FM"
                    intmsgno = objApp.save_FM_AFM(ViewState("Mode"), ViewState("valstrgrpindex"), txtLevel.Text, cboAO.SelectedItem.Value, cboAAO.SelectedItem.Value, rdRelief.SelectedItem.Value)
            End Select

            Select Case intmsgno
                Case WheelMsgNum.Save
                    strmsg = MsgRecordSave
                    'txtLevel.Text = ""
                    cboAO.SelectedIndex = 0
                    'cboAAO.SelectedIndex = 0
                Case WheelMsgNum.Duplicate
                    strmsg = MsgRecordDuplicate
                Case WheelMsgNum.NotSave

            End Select
            If intmsgno = 0 Then

                strmsg = "The following buyer cannot be assigned because it was already assigned as Purchaser in the same list -- " & cboAO.SelectedItem.Text

            End If
            Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)
        Else
            ' strmsg = "Please select an Approving Officer who has higher approval limit."
            strmsg = "Please select an Approving Officer whose approval limit is > the previous AO and < the next AO."
            Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub cmd_reset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_reset.Click
        If ViewState("Mode") = "A" Then
            txtLevel.Text = ""
            cboAO.SelectedIndex = 0
            cboAAO.SelectedIndex = 0
        Else
            Common.SelDdl(ViewState("strAO"), cboAO, True, False)
            Common.SelDdl(ViewState("strAAO"), cboAAO, True, False)
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

        Select Case ViewState("type")
            Case "AO"
                intchkmsg = objApp.checkMassApp(cboAO.SelectedItem.Value)
            Case "FO"
                intchkmsg = objApp.checkFOMassApp(cboAO.SelectedItem.Value)
            Case "FM"
                intchkmsg = 1
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
        blnBuyer = objApp.isAssignedBuyer(ViewState("valstrgrpindex"), cboAO.SelectedItem.Value, cboAAO.SelectedItem.Value, strWhose)
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
End Class
