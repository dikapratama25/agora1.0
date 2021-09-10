Imports eProcure.Component
Imports AgoraLegacy

Partial Public Class CostAllocDetailSearch
    Inherits AgoraLegacy.AppBaseClass
    Dim objCostAlloc As New IPP
    Dim strCostAllocCode As String
    Dim objCostAllocDetail As New CostAllocDetail
    Dim objDB As New EAD.DBCom
    Dim strCA As String
    Dim j As Integer
    Dim ttlAmount As Decimal

    Public Enum CostAloc
        branchCode = 0
        costCentre = 1
        desc = 2
        branchName = 3
        percentage = 4
        allocAmt = 5
    End Enum

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objCostAllocDetail As New IPP

        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgCostAllocDetail)
        Session("CostAlloc") = Request.QueryString("CostAllocCode")
        Session("Amount") = Request.QueryString("Amount")
        Session("VenIdx") = Request.QueryString("VenIdx")

        If Request.QueryString("frm") = "approval" Then
            Session("InvIdx") = Request.QueryString("invindex")
        Else
            Session("InvIdx") = Request.QueryString("InvIdx")

        End If

        ' Session("InvIdx") = Request.QueryString("InvIdx")
        Session("InvLine") = Request.QueryString("InvLine")

        If Session("Amount") <> "" Then
            lblAmount.Text = FormatNumber(Format(CDbl(Session("Amount")), "###,##0.00"))
        End If


        If Not Page.IsPostBack Then
            Bindgrid()
            dtgCostAllocDetail.CurrentPageIndex = 0
        End If
        objCostAllocDetail = Nothing
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim ds, ds2 As New DataSet
        Dim objCostAllocDetail As New IPP
        Dim strCostCentreName As String
        Dim strBranchName As String
        Dim chkCAsql As String


        If Session("InvIdx") <> "" Then
            chkCAsql = "SELECT ID.ID_COST_ALLOC_CODE " & _
                               "FROM INVOICE_MSTR IM " & _
                               "LEFT JOIN INVOICE_DETAILS ID ON ID_INVOICE_NO=IM_INVOICE_NO AND IM_S_COY_ID=ID_S_COY_ID " & _
                               "WHERE im_invoice_index ='" & Session("InvIdx") & "' " & _
                               "AND IM_B_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND id_invoice_line = '" & Session("InvLine") & "'"
            'and im_created_by = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'"

            strCA = objDB.GetVal(chkCAsql)

        End If
       
        If Request.QueryString("frm") = "approval" Then
            ds = objCostAllocDetail.GetApprCostAllocDetail(Session("CostAlloc"), Request.QueryString("invindex"), Session("InvLine"), Session("VenIdx"))

        Else
      
            If strCA = "" Then
                ds = objCostAllocDetail.GetCostAllocDetail(Session("CostAlloc"))
            Else
                ds = objCostAllocDetail.GetInvCostAllocDetail(Session("InvIdx"), Session("InvLine"), Session("VenIdx"))
            End If


        End If


        If ds.Tables(0).Rows.Count > 0 Then
            Dim dgItem As DataGridItem
            Dim i As Integer = 1


            lblCostAllocCode.Text = Common.parseNull(ds.Tables(0).Rows(0)("CA_Code"))            
            lblCostAllocCodeDesc.Text = Common.parseNull(ds.Tables(0).Rows(0)("CA_Desc"))

            intPageRecordCnt = ds.Tables(0).Rows.Count

            ViewState("RecordCount") = intPageRecordCnt
            Dim dvViewSample As DataView
            dvViewSample = ds.Tables(0).DefaultView
            dvViewSample.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewSample.Sort += " DESC"

            If intPageRecordCnt > 0 Then
                NoRecord.Style("display") = "none"
                CostAllocDetail.Style("display") = "inline"
                resetDatagridPageIndex(dtgCostAllocDetail, dvViewSample)
                dtgCostAllocDetail.DataSource = dvViewSample
                dtgCostAllocDetail.DataBind()
            Else
                dtgCostAllocDetail.DataBind()
                Common.NetMsgbox(Me, MsgNoRecord)
                If Not Page.IsPostBack Then
                    NoRecord.Style("display") = "inline"
                End If
                dtgCostAllocDetail.DataBind()

            End If
            ' add for above checking
            ViewState("PageCount") = dtgCostAllocDetail.PageCount
        End If
        objCostAlloc = Nothing
        ds = Nothing
    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dtgCostAllocDetail.SortCommand
        Grid_SortCommand(sender, e)
        dtgCostAllocDetail.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub dtgCostAllocDetail_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCostAllocDetail.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dtgCostAllocDetail, e)
        intPageRecordCnt = ViewState("RecordCount")
    End Sub

    Private Sub dtgCostAllocDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCostAllocDetail.ItemDataBound
        Dim dgItem As DataGridItem
        Dim InvLineAmt, Amount As Decimal

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            e.Item.Cells(CostAloc.costCentre).Text = e.Item.Cells(CostAloc.costCentre).Text & " : " & e.Item.Cells(CostAloc.branchCode).Text
            e.Item.Cells(CostAloc.desc).Text = e.Item.Cells(CostAloc.desc).Text & " : " & e.Item.Cells(CostAloc.branchName).Text

            If Session("Amount") <> "" Then
                InvLineAmt = Session("Amount")
                Amount = FormatNumber(Format(CDbl(Session("Amount")), "###,##0.00") * Common.parseNull(dv("Percentage")) / 100, 2)
                e.Item.Cells(CostAloc.allocAmt).Text = FormatNumber(Format(CDbl(Amount), "###,##0.00"))
                ttlAmount = ttlAmount + Amount
                j = j + 1
                If j = ViewState("RecordCount") And ttlAmount <> InvLineAmt Then
                    Amount = FormatNumber(Format(CDbl(Session("Amount")), "###,##0.00") * Common.parseNull(dv("Percentage")) / 100, 2) _
                    + (InvLineAmt - ttlAmount)
                End If
                If Session("Amount") <> "" And strCA = "" Then
                    e.Item.Cells(CostAloc.allocAmt).Text = FormatNumber(Format(CDbl(Amount), "###,##0.00")) 'FormatNumber(Format(CDbl(Session("Amount")), "###,##0.00") * Common.parseNull(dv("Percentage")) / 100, 2)

                End If
            End If

        End If
    End Sub
    Private Sub dtgCostAllocDetail_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgCostAllocDetail.PageIndexChanged
        'Dim objCostAlloc As New CostAllocIPP

        dtgCostAllocDetail.CurrentPageIndex = e.NewPageIndex
        objCostAlloc.GetCostAllocDetail(strCostAllocCode)
        Bindgrid()
        Session("action") = ""
        objCostAlloc = Nothing
    End Sub

End Class