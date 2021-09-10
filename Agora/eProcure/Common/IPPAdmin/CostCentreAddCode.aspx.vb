Imports eProcure.Component
Imports AgoraLegacy

Public Class CostCentreAddCode
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objGlobal As New AppGlobals
    Dim intCostCentreIndicator As Integer
    Dim strMode As String
    Dim comfirmstatus As String
    Dim intCostCentreIndex As Integer
    Dim strCostCentreStatus As String

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
        cmdSave.Visible = False
        cmdAdd.Visible = False
        cmdClose.Visible = True
        cmdSave.Enabled = False
        cmdAdd.Enabled = False
        cmdClose.Enabled = True

        Dim alButtonList As ArrayList
        If strMode = "Modify" Then
            cmdSave.Visible = True
            cmdSave.Enabled = True
            'Me.txtCostCentreCode.Focus()
            'alButtonList = New ArrayList
            'alButtonList.Add(cmdSave)
            'htPageAccess.Add("Save", alButtonList)
            'alButtonList = New ArrayList
            'alButtonList.Add(cmdClose)
            'htPageAccess.Add("Close", alButtonList)

        ElseIf strMode = "Add" Then
            cmdSave.Visible = True
            cmdAdd.Visible = True
            cmdSave.Enabled = True
            cmdAdd.Enabled = True
            'Me.txtCostCentreCode.Focus()

            'alButtonList = New ArrayList
            'alButtonList.Add(cmdSave)
            'htPageAccess.Add("Save", alButtonList)
            'alButtonList = New ArrayList
            'alButtonList.Add(cmdAdd)
            'htPageAccess.Add("Add", alButtonList)
            'alButtonList = New ArrayList
            'alButtonList.Add(cmdClose)
            'htPageAccess.Add("Close", alButtonList)
        End If
        'CheckButtonAccess()
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim strCCCode As String
        Dim strCCDesc As String
        Dim objCC As New IPP

        MyBase.Page_Load(sender, e)
        strMode = Me.Request.QueryString("mode")
        'intCostCentreIndex = Me.Request.QueryString("dindex")

        If Not IsPostBack Then
            objCC.GetCostCentreInfo(strCCCode, strCCDesc, strCostCentreStatus)
            '20110628-Jules commented.
            'Me.Label1.Text = strCCCode
            'Me.Label2.Text = strCCDesc


            'Zulham 08/05/2018 - PAMB
            If Session("CompanyID").ToString.ToUpper <> "HLB" AndAlso Session("CompanyID").ToString.ToUpper <> "HLISB" Then
                dlCompany.Items.Clear()
                dlCompany.Items.Insert(0, New ListItem("--Select--", 0))
                dlCompany.Items.Insert(1, New ListItem(Session("CompanyID").ToString.ToUpper, 1))
            End If
            'End


            If strMode = "Modify" Then
                Me.txtCostCentreCode.Text = Me.Request.QueryString("CC_Code").Trim
                Me.txtCostCentreCode.Enabled = False
                Me.txtCostCentreDesc.Text = Replace(Me.Request.QueryString("CC_Desc").Trim, "&nbsp;", "")
                If (Me.Request.QueryString("CC_Status").Trim = "Active") Then
                    Me.rbtnstatus.SelectedValue = "A"
                Else
                    Me.rbtnstatus.SelectedValue = "I"
                End If
                'mimi 2018-04-24 : remove hardcode HLB to PAMB
                If Request.QueryString("CC_Comp").ToString.ToUpper = "PAMB" Then
                    'If Request.QueryString("CC_Comp") = "HLB" Then
                    dlCompany.SelectedIndex = 1
                ElseIf Request.QueryString("CC_Comp") = "HLISB" Then
                    dlCompany.SelectedIndex = 2
                Else
                    dlCompany.SelectedIndex = 0
                End If
            End If
        End If

        objCC = Nothing

    End Sub

    Private Sub cmdClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Response.Write("<script language='javascript'> { window.close();}</script>")

    End Sub

    Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        strMode = "Add"
        Me.txtCostCentreCode.Text = ""
        Me.txtCostCentreCode.Enabled = True
        Me.txtCostCentreCode.Focus()
        Me.txtCostCentreDesc.Text = ""
        Me.rbtnstatus.SelectedValue = "A"
        dlCompany.SelectedIndex = 0

    End Sub

    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim strMsg As String
        Dim objCC As New IPP

        If Page.IsValid Then
            lblMsg.Text = ""

            If dlCompany.SelectedIndex = 0 Then
                strMsg = "Please select a Company ID!"
                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                objCC = Nothing
                Exit Sub
            End If

            If strMode = "Add" Then
                comfirmstatus = objCC.AddCostCentre(Me.txtCostCentreCode.Text, Me.txtCostCentreDesc.Text, Me.rbtnstatus.SelectedValue, Me.dlCompany.SelectedItem.Text)
                Select Case comfirmstatus
                    Case WheelMsgNum.Duplicate
                        strMsg = objGlobal.GetErrorMessage("00002")

                    Case WheelMsgNum.NotSave
                        strMsg = objGlobal.GetErrorMessage("00007")

                    Case WheelMsgNum.Save
                        Session("action") = "Add"
                        strMsg = objGlobal.GetErrorMessage("00003")

                End Select

            ElseIf strMode = "Modify" Then
                comfirmstatus = objCC.ModCostCentre(Me.txtCostCentreCode.Text, Me.txtCostCentreDesc.Text, Me.rbtnstatus.SelectedValue, Me.dlCompany.SelectedItem.Text)
                Select Case comfirmstatus
                    Case -99
                        strMsg = objGlobal.GetErrorMessage("00018")

                    Case WheelMsgNum.Duplicate
                        strMsg = objGlobal.GetErrorMessage("00002")

                    Case WheelMsgNum.NotSave
                        strMsg = objGlobal.GetErrorMessage("00007")

                    Case WheelMsgNum.Save
                        Session("action") = "Modify"
                        strMsg = objGlobal.GetErrorMessage("00005")
                End Select
            End If

            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
            objCC = Nothing

        End If
        Me.txtCostCentreCode.Enabled = False
    End Sub

    Private Sub rbtnstatus_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtnstatus.SelectedIndexChanged
        If strMode = "Modify" Then
            Dim blnHasPending As Boolean
            Dim objCC As New IPP
            blnHasPending = objCC.CheckPending(txtCostCentreCode.Text, "CC")

            If blnHasPending = True Then
                Common.NetMsgbox(Me, "Modification is not allowed as there is pending record tied to it.", MsgBoxStyle.Information)
                rbtnstatus.SelectedValue = "A"
            End If
        End If
    End Sub

End Class