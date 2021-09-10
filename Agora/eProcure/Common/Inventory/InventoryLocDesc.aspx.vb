Imports eProcure.Component
Imports AgoraLegacy

Public Class InventoryLocDesc
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objGlobal As New AppGlobals
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
        'cmdSave.Enabled = False
        'Dim alButtonList As ArrayList
        'alButtonList = New ArrayList
        'alButtonList.Add(cmdSave)
        'htPageAccess.Add("update", alButtonList)
        'CheckButtonAccess()

    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objInv As New Inventory
        MyBase.Page_Load(sender, e)

        If Not IsPostBack Then
            objInv.GetLocationDesc(Me.txtLocationDesc.Text, Me.txtSubLocationDesc.Text)
        End If
        objInv = Nothing

    End Sub

    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim strStatus As String
        Dim objInv As New Inventory

        If Page.IsValid Then
            Dim aryProdCode As New ArrayList

            strStatus = objInv.SaveLocationDesc(Me.txtLocationDesc.Text, Me.txtSubLocationDesc.Text)
            aryProdCode.Add(Me.txtLocationDesc.Text)
            aryProdCode.Add(Me.txtSubLocationDesc.Text)
            Session("LocDescription") = aryProdCode

            If strStatus = "I" Then
                Session("action") = "Update"
                Common.NetMsgbox(Me, "Location Description " & objGlobal.GetErrorMessage("00003"), MsgBoxStyle.Information)

            ElseIf strStatus = "U" Then
                Session("action") = "Update"
                Common.NetMsgbox(Me, "Location Description " & objGlobal.GetErrorMessage("00005"), MsgBoxStyle.Information)
            End If
        End If

        objInv = Nothing

    End Sub

    Private Sub cmdClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Response.Write("<script language='javascript'> { window.close();}</script>")

    End Sub
End Class