'Copyright © 2011 STRATEQ GLOBAL SERVICES. All rights reserved.
Imports AgoraLegacy
Imports eProcure.Component


Public Class InventoryReqInfoFTN
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents lblIRNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblIssueDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblIssueTo As System.Web.UI.WebControls.Label
    Protected WithEvents lblQty As System.Web.UI.WebControls.Label

    Protected WithEvents lblRefNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblRemark As System.Web.UI.WebControls.Label
    Protected WithEvents lblDepartment As System.Web.UI.WebControls.Label

    Protected WithEvents dtgItem As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdPrint As System.Web.UI.HtmlControls.HtmlInputButton

    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink

    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objINV As New Inventory
    Dim objGLO As New AppGlobals
    Dim aryInvReq As New ArrayList()
    Dim intNumeric As Boolean = True
    Dim strMsg As String = ""
    Dim LocDesc, SubLocDesc As String

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
        ''cmd_save.Enabled = False
        ''Dim alButtonList As ArrayList
        ''alButtonList = New ArrayList
        ''alButtonList.Add(cmd_save)
        ''htPageAccess.Add("add", alButtonList)
        ''htPageAccess.Add("update", alButtonList)
        ''CheckButtonAccess()
        'cmd_Reset.Enabled = blnCanAdd Or blnCanUpdate Or blnCanDelete
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgItem)

        lblIRNo.Text = Me.Request.QueryString("IRNo")
        lblIssueDate.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, Me.Request.QueryString("IRDate"))
        lblIssueTo.Text = Me.Request.QueryString("IssueTo")
        lblDepartment.Text = Me.Request.QueryString("department")
        lblRefNo.Text = Me.Request.QueryString("RefNo")
        lblRemark.Text = Me.Request.QueryString("Remark")

        If Not Page.IsPostBack Then
            GenerateTab()
            Bindgrid()
        End If

        cmdPrint.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewIRSlip.aspx", "IRNo=" & Trim(lblIRNo.Text) & "&CoyID=" & Session("CompanyID") & "") & "')")
        lnkBack.NavigateUrl = dDispatcher.direct("Inventory", "InventoryReqList.aspx", "")
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        Session("w_InventoryReq_tabs") = "<div class=""t_entity""><ul>" & _
                                    "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "InventoryReq.aspx", "pageid=" & strPageId) & """><span>Inventory Requisition</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "InventoryReqList.aspx", "") & """><span>Inventory Requisition Listing</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                                   "</ul><div></div></div>"

    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgItem.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Public Sub MyDataGrid_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtgItem.CurrentPageIndex = e.NewPageIndex
        Bindgrid(0)
    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim ds As DataSet = New DataSet
        Dim strLoc, strSubLoc As String
        strLoc = ""
        strSubLoc = ""


        ds = objINV.getInventoryItemInfoFiltered(Request.QueryString("IRNo"))
        Dim dvViewItem As DataView
        dvViewItem = ds.Tables(0).DefaultView

        dvViewItem.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewItem.Sort += " DESC"

        ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count
        If ViewState("intPageRecordCnt") > 0 Then
            resetDatagridPageIndex(dtgItem, dvViewItem)

            dtgItem.DataSource = dvViewItem
            dtgItem.DataBind()
        Else
            dtgItem.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        ViewState("PageCount") = dtgItem.PageCount
    End Function

    Private Sub dtgItem_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgItem.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgItem, e)

        objINV.GetLocationDesc(LocDesc, SubLocDesc)
        LocDesc = IIf(LocDesc = "", "Location", "" + LocDesc + "")
        SubLocDesc = IIf(SubLocDesc = "", "Sub Location", "" + SubLocDesc + "")

        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(3).Text = "" + LocDesc
            e.Item.Cells(4).Text = "" + SubLocDesc
        End If

    End Sub

    Private Sub dtgItem_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgItem.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)



        End If
    End Sub
End Class
