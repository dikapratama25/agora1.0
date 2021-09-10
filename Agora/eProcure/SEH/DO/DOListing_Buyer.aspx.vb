Imports AgoraLegacy
Imports eProcure.Component

Public Class DOListing_Buyer
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents lbl_result As System.Web.UI.WebControls.Label
    Protected WithEvents dtgDO As System.Web.UI.WebControls.DataGrid
    Dim ObjDO As New DeliveryOrder
    Dim dDispatcher As New AgoraLegacy.dispatcher
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
    Public Enum EnumDo
        Do_No
        Ref_No
        Ref_Date
        DO_Creation_Date
        DO_Date
    End Enum

    Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        htPageAccess.Add("add", alButtonList)
        CheckButtonAccess()
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgDO)
        If Not Page.IsPostBack Then
            Bindgrid(True)
        End If

        Session("urlreferer") = "DOListing_Buyer"
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgDO.CurrentPageIndex = 0
        ViewState("SortAscending") = "no"
        ViewState("SortExpression") = "DOM_DO_Date"
        Bindgrid()
    End Sub

    Sub dtgDO_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgDO.PageIndexChanged
        dtgDO.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Sub SortCommand_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles dtgDO.SortCommand
        Grid_SortCommand(sender, e)
        dtgDO.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objDO_Ext As New DeliveryOrder_Ext

        '//Retrieve Data from Database
        Dim dsDO As DataSet = New DataSet
        dsDO = objDO_Ext.GetDOFromVendor(txtDoNo.Text, txtStartDate.Text, txtEndDate.Text)

        '//for sorting asc or desc
        Dim dvViewDO As DataView
        dvViewDO = dsDO.Tables(0).DefaultView
        dvViewDO.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewDO.Sort += " DESC"
        If ViewState("action") = "del" Then
            If dtgDO.CurrentPageIndex > 0 And dsDO.Tables(0).Rows.Count Mod dtgDO.PageSize = 0 Then
                dtgDO.CurrentPageIndex = dtgDO.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If
        intTotRecord = dsDO.Tables(0).Rows.Count
        intPageRecordCnt = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind

        If intTotRecord > 0 Then
            resetDatagridPageIndex(dtgDO, dvViewDO)
            dtgDO.DataSource = dvViewDO
            dtgDO.DataBind()
        Else
            dtgDO.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        ViewState("PageCount") = dtgDO.PageCount
    End Function

    Private Sub dtgDO_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgDO.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dtgDO, e)
    End Sub

    Private Sub dtgDO_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgDO.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            ''//to dynamic build hyoerlink
            Dim lnkDONum As HyperLink
            lnkDONum = e.Item.FindControl("lnkDONum")
            lnkDONum.Text = dv("DOM_DO_NO")
            lnkDONum.NavigateUrl = dDispatcher.direct("DO", "DODetails.aspx", "DONo=" & dv("DOM_DO_NO") & "&PONo=" & dv("POM_PO_NO") & "&POIdx=" & dv("POM_PO_INDEX") & "&pageid=" & strPageId & "&SCoyID=" & dv("DOM_S_COY_ID"))
            'lnkDONum.NavigateUrl = "DODetails.aspx?Frm=Listing&DONo=" & dv("DOM_DO_NO") & "&POIdx=" & dv("POM_PO_INDEX") & "&pageid=" & strPageId & "&SCoyID=" & dv("DOM_S_COY_ID")

            If IsDBNull(dv("DOM_S_REF_DATE")) Then
                e.Item.Cells(EnumDo.Ref_Date).Text = ""
            Else
                e.Item.Cells(EnumDo.Ref_Date).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("DOM_S_REF_DATE"))
            End If

            e.Item.Cells(EnumDo.DO_Date).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("DOM_DO_DATE"))
            e.Item.Cells(EnumDo.DO_Creation_Date).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("DOM_CREATED_DATE"))

            If ObjDO.withAttach(dv("DOM_DO_NO"), dv("DOM_S_COY_ID")) Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/images", "clip_icon.gif")
                lnkDONum.Controls.Add(imgAttach)
                e.Item.Cells(EnumDo.Do_No).Controls.Add(imgAttach)
            End If
        End If
    End Sub
End Class
