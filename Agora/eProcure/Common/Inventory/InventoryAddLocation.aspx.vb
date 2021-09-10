Imports eProcure.Component
Imports AgoraLegacy

Public Class InventoryAddLocation
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objGlobal As New AppGlobals
    Dim intLocIndicator As Integer
    Dim strMode As String
    Dim comfirmstatus As String
    Dim intLocIndex As Integer

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
        If strMode = "Update" Then
            cmdSave.Visible = True
            cmdSave.Enabled = True
            'Me.txtLocation.Focus()
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
            'Me.txtLocation.Focus()

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
        Dim strLoc As String
        Dim strSLoc As String
        Dim objInv As New Inventory

        MyBase.Page_Load(sender, e)
        strMode = Me.Request.QueryString("mode")
        intLocIndex = Me.Request.QueryString("dindex")

        If Not IsPostBack Then
            objInv.GetLocationInfo(strLoc, strSLoc, intLocIndicator)
            Me.Label1.Text = strLoc
            Me.Label2.Text = strSLoc
            If strMode = "Update" Then
                Me.txtLocation.Text = Me.Request.QueryString("Loc").Trim
                Me.txtSubLocation.Text = Replace(Me.Request.QueryString("SubLoc").Trim, "&nbsp;", "")
            End If
        End If

        objInv = Nothing

    End Sub

    Private Sub cmdClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Response.Write("<script language='javascript'> { window.close();}</script>")

    End Sub

    Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        strMode = "Add"
        Me.txtSubLocation.Text = ""
        Me.txtSubLocation.Focus()

    End Sub

    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim strMsg As String
        Dim objInv As New Inventory

        If Page.IsValid Then
            lblMsg.Text = ""
            If strMode = "Add" Then
                comfirmstatus = objInv.AddLocation(Me.txtLocation.Text, Me.txtSubLocation.Text)
                Select Case comfirmstatus
                    Case WheelMsgNum.Duplicate
                        strMsg = objGlobal.GetErrorMessage("00002")

                    Case WheelMsgNum.NotSave
                        strMsg = objGlobal.GetErrorMessage("00007")

                    Case WheelMsgNum.Save
                        Session("action") = "Add"
                        strMsg = objGlobal.GetErrorMessage("00003")

                End Select
            ElseIf strMode = "Update" Then
                comfirmstatus = objInv.ModLocation(Me.txtLocation.Text, Me.txtSubLocation.Text, intLocIndex)
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
            objInv = Nothing

        End If

    End Sub

End Class