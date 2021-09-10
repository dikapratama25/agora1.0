Imports AgoraLegacy
Imports eProcure.Component


Public Class Others
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDb As New EAD.DBCom
    Dim objAdm As New Admin

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub


    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        MyBase.Page_Load(sender, e)
        If Not IsPostBack Then

        End If

    End Sub

    Private Sub ddl_Select_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Select.SelectedIndexChanged

        If ddl_Select.SelectedItem.Value = "1" Then
            Me.Response.Redirect(dDispatcher.direct("Admin", "DeliveryTerm.aspx", "side=BUYER" & "&pageid=" & strPageId))
        ElseIf ddl_Select.SelectedItem.Value = "2" Then
            Me.Response.Redirect(dDispatcher.direct("Admin", "PackingType.aspx", "side=BUYER" & "&pageid=" & strPageId))
        ElseIf ddl_Select.SelectedItem.Value = "3" Then
            Me.Response.Redirect(dDispatcher.direct("Admin", "Section.aspx", "side=BUYER" & "&pageid=" & strPageId))
        ElseIf ddl_Select.SelectedItem.Value = "4" Then
            Me.Response.Redirect(dDispatcher.direct("Admin", "SectionAssign.aspx", "side=BUYER" & "&pageid=" & strPageId))
        Else
            Me.Response.Redirect(dDispatcher.direct("Admin", "Others.aspx", "side=BUYER" & "&pageid=" & strPageId))
        End If
    End Sub
End Class
