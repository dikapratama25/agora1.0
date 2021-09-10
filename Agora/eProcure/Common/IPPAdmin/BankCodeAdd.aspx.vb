Imports eProcure.Component
Imports AgoraLegacy

Public Class BankCodeAdd
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objGlobal As New AppGlobals
    Dim strMode As String
    Dim comfirmstatus As String
    Dim strBankCode As String
    Dim strBankName As String
    Dim strBCodeUsage As String
    Dim strBCStatus As String

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

        ElseIf strMode = "Add" Then
            cmdSave.Visible = True
            cmdAdd.Visible = True
            cmdSave.Enabled = True
            cmdAdd.Enabled = True

        End If

    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objBankCode As New IPP
        Dim ds As New DataSet

        MyBase.Page_Load(sender, e)
        strMode = Me.Request.QueryString("mode")

        If Not IsPostBack Then
            ds = objBankCode.GetBankInfo(strBankCode, strBankName, strBCStatus)
            If strMode = "Modify" Then
                'Zulham 23112018
                Me.txtBankCode.Text = Me.Request.QueryString("BC_Code").Trim
                Me.txtBankCode.Enabled = False
                Me.txtBankName.Text = Replace(Me.Request.QueryString("BC_Name").Trim, "&nbsp;", "")
                If (Me.Request.QueryString("BC_Usage").Trim = "IBG" Or Request.QueryString("BC_Usage").Trim = "LOCAL BANK TRANSFER-(RM)") Then
                    Me.rbtnBCodeUsage.SelectedValue = "IBG"
                ElseIf (Me.Request.QueryString("BC_Usage").Trim = "TT" Or Request.QueryString("BC_Usage").Trim = "TELEGRAPHIC TRANSFER-(FOREIGN CURRENCY)") Then
                    Me.rbtnBCodeUsage.SelectedValue = "TT"
                ElseIf (Me.Request.QueryString("BC_Usage").Trim = "BC" Or Request.QueryString("BC_Usage").Trim = "CHEQUE-(RM)") Then
                    Me.rbtnBCodeUsage.SelectedValue = "BC"
                ElseIf (Me.Request.QueryString("BC_Usage").Trim = "NT") Then
                    Me.rbtnBCodeUsage.SelectedValue = "NT"
                ElseIf (Me.Request.QueryString("BC_Usage").Trim = "CO" Or Request.QueryString("BC_Usage").Trim = "CASHIER'S ORDER-(RM)") Then
                    Me.rbtnBCodeUsage.SelectedValue = "CO"
                ElseIf (Me.Request.QueryString("BC_Usage").Trim = "BD" Or Request.QueryString("BC_Usage").Trim = "BANK DRAFT-(FOREIGN CURRENCY)") Then
                    Me.rbtnBCodeUsage.SelectedValue = "BD"
                End If
                If (Me.Request.QueryString("BC_Status").Trim = "Active") Then
                    Me.rbtnstatus.SelectedValue = "A"
                Else
                    Me.rbtnstatus.SelectedValue = "I"
                End If

            End If
        End If

        objBankCode = Nothing

    End Sub

    Private Sub cmdClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Response.Write("<script language='javascript'> { window.close();}</script>")

    End Sub

    Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        strMode = "Add"

        Me.txtBankCode.Text = ""
        Me.txtBankCode.Enabled = True
        Me.txtBankCode.Focus()
        Me.txtBankName.Text = ""
        Me.rbtnBCodeUsage.SelectedValue = "IBG"
        Me.rbtnstatus.SelectedValue = "A"
    End Sub

    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim strMsg As String
        Dim objAGroup As New IPP
        If Page.IsValid Then
            lblMsg.Text = ""
            If strMode = "Add" Then
                comfirmstatus = objAGroup.AddBankCode(Me.txtBankCode.Text, Me.txtBankName.Text, Me.rbtnBCodeUsage.SelectedValue, Me.rbtnstatus.SelectedValue)
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
                comfirmstatus = objAGroup.ModBankCode(Me.txtBankCode.Text, Me.txtBankName.Text, Me.rbtnBCodeUsage.SelectedValue, Me.rbtnstatus.SelectedValue)
                Select Case comfirmstatus
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
            objAGroup = Nothing

        End If
        Me.txtBankCode.Enabled = False
    End Sub

End Class