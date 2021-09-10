Imports eProcure.Component
Imports AgoraLegacy

Partial Public Class WHTReceipt
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objdb As New EAD.DBCom
    Dim objGlobal As New AppGlobals

    Public Enum EnumInv
        icSCoyName
        icDocNo
        icDocDate
        icCurrency
        icWhtAmt
        icSection
        icReceiptNo
        icReceiptDate
        icIndex
    End Enum

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        blnPaging = True
        'strNewCSS = "true"
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgWHTList)

        If Not IsPostBack Then
            Bindgrid()
        End If

    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objIPP As New IPP
        Dim ds As New DataSet

        ds = objIPP.GetWHTReceipt(txtDocNo.Text, txtReceiptNo.Text, txtVendor.Text)

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewSample.Sort += " DESC"

        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgWHTList, dvViewSample)
            dtgWHTList.DataSource = dvViewSample
            dtgWHTList.DataBind()
            btnSave.Enabled = True
        Else
            dtgWHTList.DataSource = dvViewSample
            dtgWHTList.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
            btnSave.Enabled = False
        End If

        ' add for above checking
        ViewState("PageCount") = dtgWHTList.PageCount
        objIPP = Nothing
        'ds = Nothing
    End Function
    Private Sub dtgWHTList_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgWHTList.ItemCreated
        'If e.Item.ItemType = ListItemType.Header Then
        '    Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
        '    chkAll.Attributes.Add("onclick", "selectAll();")
        'End If

        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgWHTList, e)
    End Sub
    Sub dtgWHTList_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgWHTList.PageIndexChanged
        dtgWHTList.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Private Sub dtgWHTList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgWHTList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            '//to add JavaScript to Check Box
            'Dim chk As CheckBox
            'chk = e.Item.Cells(EnumInv.icChk).FindControl("chkSelection")
            'chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            e.Item.Cells(EnumInv.icDocNo).Text = "<A href=""" & dDispatcher.direct("IPP", "IPPDocument.aspx", "Frm=WHTReceipt&index=" & dv("IM_INVOICE_INDEX") & "&DocumentNo=" & dv("IM_INVOICE_NO")) & """ ><font color=#0000ff>" & Common.parseNull(dv("IM_INVOICE_NO")) & "</font></A>"
            e.Item.Cells(EnumInv.icDocDate).Text = Format(dv("IM_DOC_DATE"), "dd/MM/yyyy")

            If IsDBNull(dv("WHT_AMT")) Then
                e.Item.Cells(EnumInv.icWhtAmt).Text = "0.00"
            Else
                e.Item.Cells(EnumInv.icWhtAmt).Text = Format(dv("WHT_AMT"), "#,###,##0.00")
            End If

            Dim ddlSection As DropDownList
            ddlSection = e.Item.FindControl("ddlSection")
            If IsDBNull(dv("IM_SECTION")) Then
                ddlSection.SelectedValue = ""
            Else
                ddlSection.SelectedValue = dv("IM_SECTION")
            End If

            Dim txtReceipt As TextBox
            txtReceipt = e.Item.FindControl("txtReceipt")
            If IsDBNull(dv("IM_RECEIPT_NO")) Then
                txtReceipt.Text = ""
            Else
                txtReceipt.Text = dv("IM_RECEIPT_NO")
            End If

            Dim txtDate As TextBox
            txtDate = e.Item.FindControl("txtDate")
            If IsDBNull(dv("IM_RECEIPT_DATE")) Then
                txtDate.Text = ""
            Else
                txtDate.Text = Format(dv("IM_RECEIPT_DATE"), "dd/MM/yyyy")
            End If

            Dim lblDate As Label
            lblDate = e.Item.FindControl("lblDate")
            lblDate.Text = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=" & txtDate.ClientID) & "','cal','width=180,height=155,left=270,top=180')""><IMG style=""CURSOR: hand"" height=""15"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & " width=""15""></A>"
        End If

    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgWHTList.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        lblMsg.Text = ""
        dtgWHTList.CurrentPageIndex = 0
        Bindgrid()
    End Sub
    Private Sub cmdClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        txtReceiptNo.Text = ""
        txtDocNo.Text = ""
        txtVendor.Text = ""
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim objIPP As New IPP
        Dim ds As DataSet
        Dim strMsg As String
        lblMsg.Text = ""

        If ValidateInputs(strMsg) Then
            ds = bindWHT()
            strMsg = objIPP.UpdateWHTReceipt(ds)
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
            Bindgrid()
        Else
            lblMsg.Text = strMsg
        End If

    End Sub

    Private Function validateInputs(ByRef strMsg As String) As Boolean

        Dim dgItem As DataGridItem
        For Each dgItem In dtgWHTList.Items
            Dim ddlSection As DropDownList
            Dim txtReceipt As TextBox
            Dim txtDate As TextBox
            ddlSection = dgItem.FindControl("ddlSection")
            txtReceipt = dgItem.FindControl("txtReceipt")
            txtDate = dgItem.FindControl("txtDate")

            If ddlSection.SelectedValue <> "" Or txtReceipt.Text <> "" Or txtDate.Text <> "" Then
                If ddlSection.SelectedValue = "" Then
                    strMsg = "<ul type='disc'>"
                    strMsg &= "<li>" & dgItem.Cells(EnumInv.icDocNo).Text & " - Section is required.<ul type='disc'></ul></li>"
                    strMsg &= "</ul>"
                    Return False
                End If

                If txtReceipt.Text = "" Then
                    strMsg = "<ul type='disc'>"
                    strMsg &= "<li>" & dgItem.Cells(EnumInv.icDocNo).Text & " - Receipt No is required.<ul type='disc'></ul></li>"
                    strMsg &= "</ul>"
                    Return False
                End If

                If txtDate.Text = "" Then
                    strMsg = "<ul type='disc'>"
                    strMsg &= "<li>" & dgItem.Cells(EnumInv.icDocNo).Text & " - Receipt Date is required.<ul type='disc'></ul></li>"
                    strMsg &= "</ul>"
                    Return False
                End If
            End If
        Next

        Return True
    End Function

    Private Function bindWHT() As DataSet
        Dim ds As New DataSet
        Dim dtDetails As New DataTable

        dtDetails.Columns.Add("IM_INVOICE_INDEX", Type.GetType("System.String"))
        dtDetails.Columns.Add("IM_SECTION", Type.GetType("System.String"))
        dtDetails.Columns.Add("IM_RECEIPT_NO", Type.GetType("System.String"))
        dtDetails.Columns.Add("IM_RECEIPT_DATE", Type.GetType("System.String"))

        Dim dtrd As DataRow
        Dim dgItem As DataGridItem
        For Each dgItem In dtgWHTList.Items
            Dim ddlSection As DropDownList
            Dim txtReceipt As TextBox
            Dim txtDate As TextBox
            ddlSection = dgItem.FindControl("ddlSection")
            txtReceipt = dgItem.FindControl("txtReceipt")
            txtDate = dgItem.FindControl("txtDate")

            If ddlSection.SelectedValue <> "" And txtReceipt.Text <> "" And txtDate.Text <> "" Then
                dtrd = dtDetails.NewRow()
                dtrd("IM_INVOICE_INDEX") = dgItem.Cells(EnumInv.icIndex).Text
                dtrd("IM_SECTION") = ddlSection.SelectedValue
                dtrd("IM_RECEIPT_NO") = txtReceipt.Text
                dtrd("IM_RECEIPT_DATE") = txtDate.Text

                dtDetails.Rows.Add(dtrd)
            End If
        Next

        ds.Tables.Add(dtDetails)
        bindWHT = ds
    End Function
End Class