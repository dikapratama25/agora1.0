Imports eProcure.Component
Imports AgoraLegacy

Public Class AssetGroupAdd
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objGlobal As New AppGlobals
    Dim strMode As String
    Dim comfirmstatus As String
    Dim strAGroupCode As String
    Dim strAGroupDesc As String
    Dim strCodeType As String
    Dim strAGStatus As String

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

        'Dim alButtonList As ArrayList
        If strMode = "Modify" Then
            cmdSave.Visible = True
            cmdSave.Enabled = True
            txtAGroupCode.Enabled = False
            'Me.txtBranchCode.Focus()
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
            'Me.txtBranchCode.Focus()

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
        Dim objAGroup As New Admin
        Dim ds As New DataSet

        MyBase.Page_Load(sender, e)
        strMode = Me.Request.QueryString("mode")

        If Not IsPostBack Then
            ds = objAGroup.GetAssetGroupInfo(strAGroupCode, strAGroupDesc, strCodeType, strAGStatus)
            If strMode = "Modify" Then
                If (Me.Request.QueryString("Code_Type").Trim = "Asset Group") Then
                    Me.rbtnCodeType.SelectedValue = "A"
                Else
                    Me.rbtnCodeType.SelectedValue = "S"
                End If
                Me.txtAGroupCode.Text = Me.Request.QueryString("AG_Code").Trim
                Me.txtAGroupCode.Enabled = False
                Me.txtAGroupDesc.Text = Replace(Me.Request.QueryString("AG_Desc").Trim, "&nbsp;", "")
                If (Me.Request.QueryString("AG_Status").Trim = "Active") Then
                    Me.rbtnstatus.SelectedValue = "A"
                Else
                    Me.rbtnstatus.SelectedValue = "I"
                End If

            End If
        End If

        objAGroup = Nothing

    End Sub

    Private Sub cmdClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Response.Write("<script language='javascript'> { window.close();}</script>")

    End Sub

    Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        strMode = "Add"
        Me.rbtnCodeType.SelectedValue = "A"
        Me.txtAGroupCode.Text = ""
        Me.txtAGroupCode.Enabled = True
        Me.txtAGroupCode.Focus()
        Me.txtAGroupDesc.Text = ""
        Me.rbtnstatus.SelectedValue = "A"
    End Sub

    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim strMsg As String = ""
        Dim objAGroup As New Admin
        If Page.IsValid Then
            lblMsg.Text = ""
            If strMode = "Add" Then
                comfirmstatus = objAGroup.AddAssetGroup(Me.txtAGroupCode.Text, Me.txtAGroupDesc.Text, Me.rbtnCodeType.SelectedValue, Me.rbtnstatus.SelectedValue)
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
                comfirmstatus = objAGroup.ModAssetGroup(Me.txtAGroupCode.Text, Me.txtAGroupDesc.Text, Me.rbtnCodeType.SelectedValue, Me.rbtnstatus.SelectedValue)
                Select Case comfirmstatus
                    Case WheelMsgNum.Duplicate
                        strMsg = objGlobal.GetErrorMessage("00002")

                    Case WheelMsgNum.NotSave
                        strMsg = objGlobal.GetErrorMessage("00007")

                    Case WheelMsgNum.Save
                        Session("action") = "Modify"
                        strMsg = objGlobal.GetErrorMessage("00005")

                    Case -99
                        strMsg = "Deactivation is not allowed as there is transaction record tied to it."
                End Select
            End If

            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
            objAGroup = Nothing

        End If
        Me.txtAGroupCode.Enabled = False
    End Sub

    Private Sub rbtnstatus_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtnstatus.SelectedIndexChanged
        If strMode = "Modify" Then
            Dim blnHasPending As Boolean
            Dim objCC As New Admin
            blnHasPending = objCC.CheckPending(txtAGroupCode.Text, "AG")

            If blnHasPending = True Then
                Common.NetMsgbox(Me, "Modification is not allowed as there is pending record tied to it.", MsgBoxStyle.Information)
                rbtnstatus.SelectedValue = "A"
            End If
        End If
    End Sub

End Class