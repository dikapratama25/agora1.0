Imports AgoraLegacy
Imports eProcure.Component

Public Class AViewSubReq
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Protected WithEvents MyDataGrid As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmd_clear As System.Web.UI.WebControls.Button
    Protected WithEvents cmdsearch As System.Web.UI.WebControls.Button
    Protected WithEvents cbo_typeofreq As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cbo_status As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txt_subon As System.Web.UI.WebControls.TextBox



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

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)
        SetGridProperty(MyDataGrid)

        If Not IsPostBack Then
            Dim objGlobal As New AppGlobals
            'Dim lstItem As New ListItem
            'lstItem.Value = "CU"
            'lstItem.Text = "Currency"
            'cbo_typeofreq.Items.Add(lstItem)
            'objGlobal.FillCodeTable(cbo_typeofreq, CodeTable.Uom)
            'objGlobal.FillCodeTable(cbo_status, CodeTable.Uom)
            'Dim objcall As New Wheel.Components.Companies
            'Dim dsCoy As DataSet
            'Dim strcode As String
            'Dim strtypeofreq As String
            'Dim strstatus As String
            'Dim strdatetime As Date

            'strtypeofreq = Me.cbo_typeofreq.SelectedItem.Value
            'strstatus = Me.cbo_status.SelectedItem.Value
            'strdatetime = Me.txt_subon.Text


            'dsCoy = objcall.getviewsubreq(strcode, strtypeofreq, strstatus, strdatetime)
            'If dsCoy.Tables(0).Rows.Count > 0 Then
            '    txt_subon.Text = Common.parseNull(dsCoy.Tables(0).Rows(0)("VR_ENT_DATETIME"))
            '    Common.SelDdl(Common.parseNull(dsCoy.Tables(0).Rows(0)("VR_REQ_CATEGORY")), cbo_typeofreq, True, True)
            '    Common.SelDdl(Common.parseNull(dsCoy.Tables(0).Rows(0)("VR_STATUS")), cbo_status, True, True)

            'End If
        End If

        'Put user code to initialize the page here
    End Sub

    Public Sub MyDataGrid_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        MyDataGrid.CurrentPageIndex = e.NewPageIndex
        Bindgrid(0, True)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        Bindgrid(MyDataGrid.CurrentPageIndex, True)
    End Sub



    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim objdb As New Companies
        '   Dim objRFQ As String
        Dim ds As DataSet
        Dim record As String
        'Dim strcode As String
        Dim strtypeofreq As String
        Dim strstatus As String
        Dim strdatetime As String

        strtypeofreq = Me.cbo_typeofreq.SelectedItem.Value
        strstatus = Me.cbo_status.SelectedItem.Value
        strdatetime = Me.txt_subon.Text


        ds = objdb.getviewsubreq(strtypeofreq, strstatus, strdatetime)
        'ds = objdb.getviewsubreq(strdatetime as Date) 

        ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"



        If ViewState("action") = "del" Then
            If MyDataGrid.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod MyDataGrid.PageSize = 0 Then
                MyDataGrid.CurrentPageIndex = MyDataGrid.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If

        MyDataGrid.DataSource = dvViewSample
        MyDataGrid.DataBind()

        'ShowStats()
    End Function


    Private Sub cmd_clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_clear.Click
        'Me.cbo_typeofreq.SelectedItem.Value = ""
        'Me.cbo_status.SelectedItem.Value = ""
        Me.txt_subon.Text = ""
        Me.cbo_typeofreq.SelectedIndex = 0
        Me.cbo_status.SelectedIndex = 0


    End Sub

    Private Sub cmdsearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdsearch.Click
        MyDataGrid.CurrentPageIndex = 0
        Bindgrid(0)
    End Sub

    Private Sub MyDataGrid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles MyDataGrid.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            e.Item.Cells(0).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("VR_ENT_DATETIME"))


        End If
    End Sub

    Private Sub MyDataGrid_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyDataGrid.SelectedIndexChanged

    End Sub

    Private Sub txt_subon_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_subon.TextChanged

    End Sub

    Private Sub MyDataGrid_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles MyDataGrid.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(MyDataGrid, e)
    End Sub

End Class
