Imports AgoraLegacy
Imports eProcure.Component
Public Class ViewCancel
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents tblSearchResult As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents dtg_CancelList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents chk_New As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chk_ack As System.Web.UI.WebControls.CheckBox
    Protected WithEvents Table4 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents cmd_ack As System.Web.UI.WebControls.Button
    Protected WithEvents hidlink As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents back As System.Web.UI.HtmlControls.HtmlAnchor
    Protected WithEvents txt_CRNO As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_po_no As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdClear As System.Web.UI.HtmlControls.HtmlInputButton

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Public Enum EnumViewCanl
        iccheck = 0
        icCRNo = 1
        icPONo = 2
        icRecDate = 3
        icUserName = 4
        icStatus = 5
        icBuyName = 6
        icVenName = 7
        icVCRNo = 8
        icVIndex = 9
        icVComID = 10
        icVPONo = 11
    End Enum

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CheckButtonAccess(True)
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        blnPaging = True
        blnSorting = True

        SetGridProperty(dtg_CancelList)

        If Not IsPostBack Then
            If viewstate("side") = "" Then
                viewstate("side") = Request(Trim("side"))
            End If
            viewstate("count_ack") = 0
            viewstate("count_data") = 0
            If viewstate("side") = "b" Then
                Me.dtg_CancelList.Columns(3).HeaderText = "Request Date"
                If viewstate("cr_no") = "" Then
                    viewstate("cr_no") = Request(Trim("CR_NO"))
                End If
                dtg_CancelList.Columns(6).Visible = False
                ' ai chu modified on 02/12/2005
                ' SR U30016 - To have CR status option for the searching
                'Me.Table4.Rows(2).Visible = False
                Me.cmd_ack.Visible = False
                'Me.chk_New.Visible = False
                chk_New.Checked = False
                Me.chk_ack.Checked = False
                Me.lblTitle.Text = "PO Cancellation Request Listing"

            ElseIf viewstate("side") = "v" Then

                Me.hidlink.Visible = False
                Me.dtg_CancelList.Columns(3).HeaderText = "CR Creation Date"
                dtg_CancelList.Columns(7).Visible = False
                Me.lblTitle.Text = "Cancellation Acknowledgement"
            End If

            'If viewstate("side") = "b" Then
            '    viewstate("SortAscending") = "no"
            '    viewstate("SortExpression") = "PCM_REQ_DATE"
            '    Bindgrid()
            'Else
            cmdSearch_Click(sender, e)
            'End If
            Session("strurl") = strCallFrom
        End If

        Me.cmd_ack.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','Save');")
        intPageRecordCnt = viewstate("intPageRecordCnt")
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtg_CancelList.CurrentPageIndex = 0
        Bindgrid(dtg_CancelList.CurrentPageIndex, True)
    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = 0, Optional ByVal pSorted As Boolean = False) As String

        Dim objPO As New PurchaseOrder
        Dim ds As DataSet
        Dim strStatus As String = ""
        Dim CR_NO As String

        If Me.txt_CRNO.Text <> "" Then
            CR_NO = Me.txt_CRNO.Text
        Else
            CR_NO = VIEWSTATE("cr_no")
        End If

        '-- start cons strstatus --
        If Me.chk_New.Checked Then
            strStatus = IIf(strStatus = "", CRStatus.newCR, strStatus & "," & CRStatus.newCR)
        End If

        If Me.chk_ack.Checked Then
            strStatus = IIf(strStatus = "", CRStatus.Acknowledged, strStatus & "," & CRStatus.Acknowledged)
        End If

        If chk_New.Checked = False And chk_ack.Checked = False Then
            strStatus = CRStatus.Acknowledged & "," & CRStatus.newCR
        End If
        ' -- con end 

        Dim b_com_id As String
        If viewstate("side") = "b" Then
            b_com_id = Session("CompanyId")
        ElseIf viewstate("side") = "v" Then
            b_com_id = ""
        End If

        ds = objPO.get_CRView(Me.txt_po_no.Text, CR_NO, viewstate("side"), b_com_id, strStatus)
        objPO = Nothing
        Dim dvViewPR As DataView
        dvViewPR = ds.Tables(0).DefaultView
        dvViewPR.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" And viewstate("SortExpression") <> "" Then dvViewPR.Sort += " DESC"

        viewstate("intPageRecordCnt") = ds.Tables(0).Rows.Count

        '//bind datagrid
        If viewstate("intPageRecordCnt") > 0 Then
            resetDatagridPageIndex(dtg_CancelList, dvViewPR)
            dtg_CancelList.CurrentPageIndex = pPage
            dtg_CancelList.DataSource = dvViewPR
            dtg_CancelList.DataBind()
            cmd_ack.Enabled = True
            If viewstate("count_data") = viewstate("count_ack") Then
                Me.cmd_ack.Enabled = False
            Else
                Me.cmd_ack.Enabled = True
            End If
        Else
            dtg_CancelList.DataBind()
            cmd_ack.Enabled = False
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        ' add for above checking
        viewstate("PageCount") = dtg_CancelList.PageCount
    End Function

    Private Sub back_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles back.ServerClick
        Response.Redirect(dDispatcher.direct("PO", "POViewB2.aspx", "pageid=" & strPageId & "&side=b&filetype=1"))
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        viewstate("SortAscending") = "no"
        viewstate("SortExpression") = "PCM_REQ_DATE"
        Bindgrid()

        If viewstate("count_data") = viewstate("count_ack") Then
            Me.cmd_ack.Enabled = False
        Else
            Me.cmd_ack.Enabled = True
        End If

        viewstate("count_ack") = 0
        viewstate("count_data") = 0
        If viewstate("side") = "v" Then
            Me.cmd_ack.Visible = True
        End If
    End Sub

    Private Sub dtg_CancelList_ItemDataBound1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_CancelList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim status As Integer = CRStatus.Acknowledged
            If viewstate("side") = "v" Then
                Dim chk As CheckBox
                chk = e.Item.Cells(EnumViewCanl.iccheck).FindControl("chkSelection")
                chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

                If Common.parseNull(dv("PCM_CR_STATUS")) = status Then
                    chk.Enabled = False
                    viewstate("count_ack") = viewstate("count_ack") + 1
                End If
                viewstate("count_data") = viewstate("count_data") + 1
                e.Item.Cells(EnumViewCanl.icVenName).Visible = False
            ElseIf viewstate("side") = "b" Then
                e.Item.Cells(EnumViewCanl.iccheck).Visible = False
                Dim chk As CheckBox
                chk = e.Item.Cells(EnumViewCanl.iccheck).FindControl("chkSelection")
                chk.Enabled = False
                e.Item.Cells(EnumViewCanl.icBuyName).Visible = False
            End If

            e.Item.Cells(EnumViewCanl.icRecDate).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("PCM_REQ_DATE"))
            If viewstate("side") = "b" Then
                e.Item.Cells(EnumViewCanl.icCRNo).Text = "<A href=""" & dDispatcher.direct("PO", "PODetail.aspx", "pageid=" & strPageId & "&cr_no=" & Common.parseNull(dv("PCM_CR_NO")) & "&PO_INDEX=" & dv("POM_PO_INDEX") & "&PO_NO=" & dv("POM_PO_NO") & "&status=" & dv("POM_PO_STATUS") & "&side=other&filetype=1&BCoyID=" & dv("PCM_B_COY_ID") & " "" ><font color=#0000ff>" & Common.parseNull(dv("PCM_CR_NO"))) & "</font></A>"
            ElseIf ViewState("side") = "v" Then
                e.Item.Cells(EnumViewCanl.icCRNo).Text = "<A href=""" & dDispatcher.direct("PO", "PODetail.aspx", "pageid=" & strPageId & "&cr_no=" & Common.parseNull(dv("PCM_CR_NO")) & "&PO_INDEX=" & dv("POM_PO_INDEX") & "&PO_NO=" & dv("POM_PO_NO") & "&status=" & dv("POM_PO_STATUS") & "&side=v&filetype=1&ack=" & Common.parseNull(dv("PCM_CR_STATUS")) & "&BCoyID=" & dv("PCM_B_COY_ID") & " "" ><font color=#0000ff>" & dv("PCM_CR_NO")) & "</font></A>"
            End If

            If viewstate("side") = "b" Then
                e.Item.Cells(EnumViewCanl.icPONo).Text = "<A href=""" & dDispatcher.direct("PO", "PODetail.aspx", "pageid=" & strPageId & "&PO_INDEX=" & dv("POM_PO_INDEX") & "&PO_NO=" & dv("POM_PO_NO") & "&BCoyID=" & dv("POM_B_COY_ID") & "&status=0&side=other&filetype=2 "" ><font color=#0000ff>" & dv("POM_PO_NO")) & "</font></A>"
            ElseIf viewstate("side") = "v" Then
                e.Item.Cells(EnumViewCanl.icPONo).Text = "<A href=""" & dDispatcher.direct("PO", "PODetail.aspx", "pageid=" & strPageId & "&PO_INDEX=" & dv("POM_PO_INDEX") & "&PO_NO=" & dv("POM_PO_NO") & "&BCoyID=" & dv("POM_B_COY_ID") & "&status=0&side=other&filetype=2 "" ><font color=#0000ff>" & dv("POM_PO_NO")) & "</font></A>"
            End If
        End If
    End Sub

    Private Sub cmd_ack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_ack.Click
        Dim dgitem As DataGridItem
        Dim chk As CheckBox
        Dim objdb As New EAD.DBCom
        Dim objpo As New PurchaseOrder_Vendor
        Dim strsql As String
        Dim strarray(0) As String
        Dim cr_num As String
        Dim i As Integer
        Dim DTR As DataTable
        Dim crdetail As New DataTable
        Dim ds As New DataSet
        Dim dtr1 As DataRow
        Dim bcomid As String

        Dim CRSTATUS1 As Integer = CRStatus.Acknowledged
        crdetail.Columns.Add("cr_num", Type.GetType("System.String"))
        crdetail.Columns.Add("CRStatus", Type.GetType("System.String"))
        crdetail.Columns.Add("bcomid", Type.GetType("System.String"))
        crdetail.Columns.Add("po_no", Type.GetType("System.String"))
        For Each dgitem In Me.dtg_CancelList.Items
            dtr1 = crdetail.NewRow()
            chk = dgitem.FindControl("chkSelection")
            If chk.Checked Then
                dtr1("cr_num") = dtg_CancelList.DataKeys.Item(i)
                dtr1("CRStatus") = CRSTATUS1
                dtr1("bcomid") = dgitem.Cells(EnumViewCanl.icVComID).Text
                dtr1("po_no") = dgitem.Cells(EnumViewCanl.icVPONo).Text
                crdetail.Rows.Add(dtr1)
            End If
            i = i + 1
        Next
        ds.Tables.Add(crdetail)
        strsql = objpo.update_ack(ds)
        If InStr(strsql, "Error occurs") > 0 Then
            Common.NetMsgbox(Me, strsql, MsgBoxStyle.Information)
        Else
            Response.Redirect(dDispatcher.direct("PO", "errorpage.aspx", "action=ack&item=" & Server.UrlEncode(strsql) & "&pageid=" & strPageId))
        End If
    End Sub

    Private Sub dtg_CancelList_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_CancelList.ItemCreated
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(dtg_CancelList, e)

        '//to add a JavaScript to CheckAll button
        If viewstate("side") = "v" Then
            If e.Item.ItemType = ListItemType.Header Then
                Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
                chkAll.Attributes.Add("onclick", "selectAll();")
            End If
        ElseIf viewstate("side") = "b" Then
            If e.Item.ItemType = ListItemType.Header Then
                Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
                chkall.Enabled = False
                e.Item.Cells(EnumViewCanl.iccheck).Visible = False
            End If
        End If
    End Sub

    Private Sub dtg_CancelList_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtg_CancelList.PageIndexChanged
        dtg_CancelList.CurrentPageIndex = e.NewPageIndex
        Bindgrid(e.NewPageIndex, True)
        If viewstate("count_data") = viewstate("count_ack") Then
            Me.cmd_ack.Enabled = False
        Else
            Me.cmd_ack.Enabled = True
        End If
    End Sub
End Class
