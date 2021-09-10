Imports AgoraLegacy
Imports eProcure.Component

Public Class cofCopy
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents dtg_rfqcopy As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmd_pre As System.Web.UI.HtmlControls.HtmlAnchor

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
        CheckButtonAccess(True)
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' cmd_pre2 = false rfq buyer side 
        ' cmd_pre=true  history.back can use in both side 
        MyBase.Page_Load(sender, e)
        SetGridProperty(Me.dtg_rfqcopy)
        Bindgrid()
    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        'If (Me.TextBox1.Text = "") Then
        '    MyDataGrid.PageSize = 8
        'Else
        '    Me.MyDataGrid.PageSize = TextBox1.Text
        'End If
        Dim ds As DataSet = New DataSet
        Dim objrfq As New RFQ

        ds = objrfq.get_rfqname2(Request(Trim("rfq_id")))
        'record = ds.Tables(0).Rows.Count
        intPageRecordCnt = ds.Tables(0).Rows.Count
        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        'dvViewSample(0)(0)


        dtg_rfqcopy.DataSource = dvViewSample

        dtg_rfqcopy.DataBind()

        'ShowStats()
    End Function
    Private Sub dtg_rfqcopy_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtg_rfqcopy.SelectedIndexChanged

    End Sub

    Private Sub cmd_pre_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_pre.ServerClick
        Response.Redirect(dDispatcher.direct("Product", "RFQ_List.aspx", "edit=1&pageid=" & strPageId & " "))
    End Sub
End Class

