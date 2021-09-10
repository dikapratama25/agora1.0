Imports AgoraLegacy
Imports eProcure.Component

Public Class RFQSubmit
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lbl_sent As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_vendor As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_item As System.Web.UI.WebControls.Label
    Protected WithEvents dtg_send As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dtg_Vendor As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dtg_item As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dtg_expire As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lbl_expire As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_duplicatesend As System.Web.UI.WebControls.Label
    Protected WithEvents Table2 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents dtg_Noexpire As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lbl_Noexpire As System.Web.UI.WebControls.Label
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

    Public Enum SendEnum
        RfqNo = 0
        RfqName = 1
    End Enum
    Public Enum VendorEnum
        RfqNo = 0
        RfqName = 1
    End Enum
    Public Enum ItemEnum
        RfqNo = 0
        RfqName = 1
    End Enum
    Public Enum ExpEnum
        RfqNo = 0
        RfqName = 1
    End Enum
    Public Enum NoexpEnum
        RfqNo = 0
        RfqName = 1
    End Enum

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CheckButtonAccess(True)
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)
        viewstate("countven") = 0
        viewstate("countItem") = 0
        viewstate("rfqid") = Request(Trim("rfq_id "))
        If IsNothing(Request(Trim("rfqsend"))) Or Request(Trim("rfqsend")) = "" Then
            Me.Table2.Visible = False
        Else
            viewstate("rfqsend") = Request(Trim("rfqsend"))
            Me.Table2.Visible = True
            lbl_duplicatesend.Visible = True
            Me.lbl_duplicatesend.Text = "RFQ Number : <FONT color=""#000000"">" & viewstate("rfqsend") & "</FONT> has/have been sent by another buyer."
        End If

        Dim i As Integer
        For i = 0 To 4
            Bindgrid(i)
        Next
        'RFQ_List.aspx?pageid=" & strPageId & "&edit=1"
        Me.cmd_pre.Attributes.Add("onclick", "window.opener.location.href=""" & dDispatcher.direct("RFQ", "RFQSearch.aspx", "pageid=" & strPageId & "&edit=1"";window.close();"))

        'Me.cmd_pre.Attributes.Add("onclick", "window.close();")
        lbl_vendor.Text = viewstate("countven")
        Me.lbl_item.Text = viewstate("countItem")
        Me.lbl_expire.Text = viewstate("expire")
        Me.lbl_sent.Text = viewstate("sent")
        Me.lbl_Noexpire.Text = viewstate("noexpire")
    End Sub

    Private Function Bindgrid(ByVal gridnum As Integer)
        Dim dg_id As DataGrid
        Dim ds As New DataSet
        Dim DocNum As String
        Dim objrfq As New RFQ
        Dim i As Integer

        If gridnum = 0 Then 'for no expire date 
            dg_id = dtg_expire
            ds = objrfq.check_expire(viewstate("rfqid"))
            viewstate("expire") = ds.Tables(0).Rows.Count
        ElseIf (gridnum = 1) Then 'for no vendor
            dg_id = dtg_Vendor
            ds = objrfq.check_vendor(viewstate("rfqid"))

        ElseIf (gridnum = 2) Then 'for no item 
            dg_id = dtg_item
            ds = objrfq.check_item(viewstate("rfqid"))

        ElseIf (gridnum = 3) Then ' for send
            dg_id = dtg_send
            ds = objrfq.check_send(viewstate("rfqid"))
            viewstate("sent") = ds.Tables(0).Rows.Count
        ElseIf (gridnum = 4) Then
            dg_id = dtg_Noexpire
            ds = objrfq.check_noexpire(viewstate("rfqid"))
            viewstate("noexpire") = ds.Tables(0).Rows.Count
        End If



        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        'dvViewSample(0)(0)
        'If pSorted Then
        dvViewSample.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"
        'End If

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured





        dg_id.DataSource = dvViewSample
        dg_id.DataBind()

        check(dg_id)


        '//datagrid.pageCount only got value after databind

    End Function

    Sub check(ByVal dg As DataGrid)
        Dim i As Integer

        If dg.Items.Count = 0 Then
            If dg.ID.ToString.Trim = "dtg_send" Then '
                For i = 3 To 5
                    Table1.Rows(i).Visible = False
                Next
            ElseIf dg.ID.ToString.Trim = "dtg_expire" Then

                For i = 12 To 14
                    Table1.Rows(i).Visible = False
                Next
            ElseIf dg.ID.ToString.Trim = "dtg_Noexpire" Then

                For i = 15 To 17
                    Table1.Rows(i).Visible = False
                Next

            End If

        Else

            If dg.ID.ToString.Trim = "dtg_Vendor" Then
                If viewstate("countven") = 0 Then
                    For i = 6 To 8
                        Table1.Rows(i).Visible = False
                    Next
                End If
            ElseIf dg.ID.ToString.Trim = "dtg_item" Then
                If viewstate("countItem") = 0 Then
                    For i = 9 To 11
                        Table1.Rows(i).Visible = False
                    Next
                End If
            End If
        End If
    End Sub
    Private Sub dtg_Vendor_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtg_Vendor.SelectedIndexChanged



    End Sub

    Private Sub dtg_Vendor_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_Vendor.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            If dv("ITEM") = 0 Then
                e.Item.Cells(VendorEnum.RfqName).Text = dv("RM_RFQ_NAME")
                e.Item.Cells(VendorEnum.RfqNo).Text = dv("RM_RFQ_No")
                viewstate("countven") = viewstate("countven") + 1
            End If

        End If
    End Sub

    Private Sub dtg_item_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtg_item.SelectedIndexChanged

    End Sub

    Private Sub dtg_item_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_item.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            If dv("ITEM") = 0 Then
                e.Item.Cells(ItemEnum.RfqName).Text = dv("RM_RFQ_NAME")
                e.Item.Cells(ItemEnum.RfqNo).Text = dv("RM_RFQ_No")
                viewstate("countItem") = viewstate("countItem") + 1
            End If

        End If
    End Sub
End Class
