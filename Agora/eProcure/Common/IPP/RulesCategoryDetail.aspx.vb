Imports eProcure.Component
Imports AgoraLegacy

Partial Public Class RulesCategoryDetail
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
        If Not Request.QueryString("GLCode") = "" Then
            MyBase.Page_Load(sender, e)
            SetGridProperty(dtgCostAllocDetail)
            If Not Page.IsPostBack Then
                Bindgrid()
                dtgCostAllocDetail.CurrentPageIndex = 0
            End If
            objCostAllocDetail = Nothing
        End If
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim ds, ds2 As New DataSet
        Dim objRuleCategoryDetail As New IPPMain
        Dim strCostCentreName As String
        Dim strBranchName As String
        Dim chkCAsql As String

        lblGLCode.Text = Common.parseNull(Request.QueryString("GLCode").ToString.Split(":")(0))
        lblGLCodeDesc.Text = Common.parseNull(Request.QueryString("GLCode").ToString.Split(":")(1))
        If lblGLCodeDesc.Text.Substring(lblGLCodeDesc.Text.Length - 1, 1) = "," Then
            lblGLCodeDesc.Text = lblGLCodeDesc.Text.Substring(0, lblGLCodeDesc.Text.Length - 1)
        End If
        ds = objRuleCategoryDetail.getRuleCategory(lblGLCode.Text)

        If ds.Tables(0).Rows.Count > 0 Then
            Dim dgItem As DataGridItem
            Dim i As Integer = 1
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
            Dim i As Integer
            Dim strRemark() As String
            Dim strRemarks As String
            'e.Item.Cells(CostAloc.costCentre).Text = e.Item.Cells(CostAloc.costCentre).Text & " : " & e.Item.Cells(CostAloc.branchCode).Text
            'e.Item.Cells(CostAloc.desc).Text = e.Item.Cells(CostAloc.desc).Text & " : " & e.Item.Cells(CostAloc.branchName).Text

            'If Session("Amount") <> "" Then
            '    InvLineAmt = Session("Amount")
            '    Amount = FormatNumber(Format(CDbl(Session("Amount")), "###,##0.00") * Common.parseNull(dv("Percentage")) / 100, 2)
            '    e.Item.Cells(CostAloc.allocAmt).Text = FormatNumber(Format(CDbl(Amount), "###,##0.00"))
            '    ttlAmount = ttlAmount + Amount
            '    j = j + 1
            '    If j = ViewState("RecordCount") And ttlAmount <> InvLineAmt Then
            '        Amount = FormatNumber(Format(CDbl(Session("Amount")), "###,##0.00") * Common.parseNull(dv("Percentage")) / 100, 2) _
            '        + (InvLineAmt - ttlAmount)
            '    End If
            '    If Session("Amount") <> "" And strCA = "" Then
            '        e.Item.Cells(CostAloc.allocAmt).Text = FormatNumber(Format(CDbl(Amount), "###,##0.00")) 'FormatNumber(Format(CDbl(Session("Amount")), "###,##0.00") * Common.parseNull(dv("Percentage")) / 100, 2)

            '    End If
            'End If

            If Not IsDBNull(dv("igc_glrule_category_remark")) Then
                strRemark = Split(dv("igc_glrule_category_remark"), Chr(13))
                strRemarks = ""
                For i = 0 To strRemark.Length - 1
                    If strRemarks = "" Then
                        strRemarks &= strRemark(i)
                    Else
                        strRemarks &= "<br>" & strRemark(i)
                    End If
                Next
                e.Item.Cells(1).Text = strRemarks
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