'Imports Wheel.Components
Imports AgoraLegacy
Imports eProcure.Component
Public Class TransTracking
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim dtMatch As DataTable
    Dim objTrac As New Tracking
    'Protected WithEvents txtBuyer As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtDept As System.Web.UI.WebControls.TextBox
    'Protected WithEvents dtgRFQ As System.Web.UI.WebControls.DataGrid
    Dim dtPR As DataTable
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    'Protected WithEvents vldDateFr As System.Web.UI.WebControls.RequiredFieldValidator
    'Protected WithEvents vldDateTo As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    'Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents cmdPdf As System.Web.UI.WebControls.ImageButton
    Protected WithEvents trExtra As System.Web.UI.HtmlControls.HtmlTableRow
    'Protected WithEvents cvSearch As System.Web.UI.WebControls.CustomValidator
    Dim dtgShare As DataGrid
    Dim intPoIdx, intDocIdx, intPRIdx, intDOIdx As Long
    Dim strPONo, strPRNo, strDONo, strGRNNo, strInvNo, strBuyerCoy, strVenCoy, strBCoyID As String
    Dim strURLPR, strURLPO, strURLDO, strURLINV, strURLINV2, strURLGRN As String
    Dim sPrevPONo As String = "", sPrevDONo As String = ""
    'Protected WithEvents Table4 As System.Web.UI.HtmlControls.HtmlTable
    'Protected WithEvents lblCoyType As System.Web.UI.WebControls.Label
    'Protected WithEvents dtgTrans1 As System.Web.UI.WebControls.DataGrid

    Dim objDB As New EAD.DBCom

    Public Enum EnumRfq
        icRfqNo = 0
        icPRNo = 1
        icBuyerName = 2
        icRfqName = 3
        'icBuyerName = 3
        icCreatedOn = 4
        icExpiryDate = 5
        icVenName = 6
        icActualQuotNum = 7
        icPONo = 8
        icReqName = 9
        icDeptName = 10
        icVenId = 11
    End Enum

    Public Enum EnumTrans
        icDocNo = 0
        icDocDate = 1
        icCoyName = 2
        icCost = 3
        icRelatedDoc = 4
        icBuyer = 5
        icDept = 6
        icBuyerCoy = 7
        icHideShow = 8
        icPONumber = 9
    End Enum
    Public Enum EnumTrans1
        icDocNo = 0
        icPRNo = 1
        icBuyerName = 2
        icDocDate = 3
        icCoyName = 4
        icCost = 5
        icBuyer = 6
        icDept = 7
        icCRNumber = 8
        icDONumber = 9
        icGRNNumber = 10
        icINVNumber = 11
		'Modified for Agora GST Stage 2 - CH - 2/2/2015
        icDNDANumber = 12
        icCNDANumber = 13
		'-------------------------------
    End Enum

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblDesc As System.Web.UI.WebControls.Label
    Protected WithEvents txtPRNo As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtVendor As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtDateFr As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtDateTo As System.Web.UI.WebControls.TextBox
    'Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    'Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    'Protected WithEvents vldDateFtDateTo As System.Web.UI.WebControls.CompareValidator
    'Protected WithEvents cboViewBy As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents dtgTrans As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents txtDocNo As System.Web.UI.WebControls.TextBox

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
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgRFQ)
        Session("w_RFQ_tabs") = Nothing
        If strPageId Is Nothing Then
            strPageId = Request.QueryString("pageid")
        End If
        If Not Page.IsPostBack Then
            'cmdPdf.Enabled = False
            cboViewBy.Items.Clear()
            If Request.QueryString("coytype").ToUpper = "VENDOR" Then
                addListItem("Purchase Order", "PO")
                addListItem("Delivery Order", "DO")
                addListItem("Goods Receipt Note", "GRN")
                addListItem("Invoice", "INV")
                lblCoyType.Text = "Buyer Company"
                Me.Table4.Rows(3).Cells(1).ColSpan = 4
                Me.Table4.Rows(3).Cells(2).Style("display") = "none"
                Me.Table4.Rows(3).Cells(3).Style("display") = "none"
                ' Me.Table4.Rows(4).Cells(2).InnerHtml = "&nbsp;" & Me.Table4.Rows(4).Cells(2).InnerHtml
            Else
                addListItem("Request For Quotation", "RFQ")
                addListItem("Contract PR", "PR")
                addListItem("Non Contract PR", "NPR")
                addListItem("Purchase Order", "PO")
                addListItem("Goods Receipt Note", "GRN")
                lblCoyType.Text = "Vendor Company"
            End If
            Session("strurl") = strCallFrom 'go back from PO
        End If
        If cboViewBy.SelectedValue = "RFQ" Or cboViewBy.SelectedValue = "PR" Or cboViewBy.SelectedValue = "NPR" Then
            lblName.Text = "Buyer Name :"
        Else
            lblName.Text = "Purchaser Name :"
        End If
        If cboViewBy.SelectedValue = "PR" Or cboViewBy.SelectedValue = "NPR" Then
            lblCoyType.Style("display") = "none"
            txtVendor.Style("display") = "none"
            txtVendor.Text = ""
        Else
            lblCoyType.Style("display") = "inline"
            txtVendor.Style("display") = "inline"
        End If

        '//Testing On One Function handles 2 events
        If cboViewBy.SelectedValue = "RFQ" Then
            dtgShare = dtgRFQ
            dtgTrans.Visible = False
            dtgRFQ.Visible = True
        Else
            dtgShare = dtgTrans
            dtgTrans.Visible = True
            dtgRFQ.Visible = False
        End If
        'cmdPdf.ImageUrl = dDispatcher.direct("Plugins/images", "pdf.bmp")
        cboViewBy.Attributes.Add("onchange", "chkViewBy();")
    End Sub

    Private Sub addListItem(ByVal pText, ByVal pVal)
        Dim lstItem As New ListItem
        lstItem.Value = pVal
        lstItem.Text = pText
        cboViewBy.Items.Add(lstItem)
    End Sub

    Public Sub Grid_Paging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgTrans.PageIndexChanged, dtgTrans1.PageIndexChanged, dtgRFQ.PageIndexChanged
        'dtgTrans.CurrentPageIndex = e.NewPageIndex
        'dtgRFQ.CurrentPageIndex = e.NewPageIndex
        'dtgShare.CurrentPageIndex = e.NewPageIndex
        sender.CurrentPageIndex = e.NewPageIndex
        Bindgrid(True)
        'ReBindgrid()
    End Sub

    Public Sub Grid_Sorting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dtgTrans1.SortCommand, dtgTrans.SortCommand, dtgRFQ.SortCommand
        Grid_SortCommand(sender, e)
        sender.CurrentPageIndex = 0
        Bindgrid(True)
        'ReBindgrid()
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        SetGridProperty(dtgShare)
        SetGridProperty(dtgTrans1)

        '//Retrieve Data from Database
        Dim ds As DataSet

        If Request.QueryString("coytype").ToUpper = "VENDOR" Then
            dtgTrans.Columns(EnumTrans.icCoyName).HeaderText = "Buyer Company"
            dtgTrans.Columns(EnumTrans.icDept).Visible = False
            dtgTrans1.Columns(EnumTrans1.icCoyName).HeaderText = "Buyer Company"
            dtgTrans1.Columns(EnumTrans1.icDept).Visible = False
        Else
            dtgTrans.Columns(EnumTrans.icCoyName).HeaderText = "Vendor Company"
            dtgTrans.Columns(EnumTrans.icDept).Visible = True
            dtgTrans1.Columns(EnumTrans1.icCoyName).HeaderText = "Vendor Company"
            dtgTrans1.Columns(EnumTrans1.icDept).Visible = True
        End If
        Select Case cboViewBy.SelectedValue
            Case "PR"
                dtgTrans.Visible = True
                dtgTrans1.Visible = False
                dtgTrans.Columns(EnumTrans.icDocNo).HeaderText = "PR Number"
                dtgTrans.Columns(EnumTrans.icBuyerCoy).HeaderText = "Buyer Name"
                dtgTrans.Columns(EnumTrans.icCoyName).Visible = False
                dtgTrans.Columns(EnumTrans.icCost).Visible = False
                dtgTrans.Columns(EnumTrans.icRelatedDoc).Visible = False
                dtgTrans.Columns(EnumTrans.icPONumber).Visible = True
                dtgTrans.Columns(EnumTrans.icHideShow).Visible = False
                dtgTrans.Columns(EnumTrans.icPONumber).SortExpression = "PO_NUMBER"
                ds = objTrac.TransTracking(cboViewBy.SelectedValue, txtDocNo.Text, txtVendor.Text, txtDateFr.Text, txtDateTo.Text, txtBuyer.Text, txtDept.Text)
            Case "NPR"
                dtgTrans.Visible = True
                dtgTrans1.Visible = False
                dtgTrans.Columns(EnumTrans.icDocNo).HeaderText = "PR Number"
                dtgTrans.Columns(EnumTrans.icBuyerCoy).HeaderText = "Buyer Name"
                dtgTrans.Columns(EnumTrans.icCoyName).Visible = False
                dtgTrans.Columns(EnumTrans.icCost).Visible = False
                dtgTrans.Columns(EnumTrans.icRelatedDoc).Visible = False
                dtgTrans.Columns(EnumTrans.icHideShow).Visible = True
                dtgTrans.Columns(EnumTrans.icHideShow).HeaderText = "RFQ Number"
                dtgTrans.Columns(EnumTrans.icPONumber).Visible = True
                dtgTrans.Columns(EnumTrans.icPONumber).SortExpression = "PO_NUMBER"
                dtgTrans.Columns(EnumTrans.icHideShow).SortExpression = "RFQ_NO"
                ds = objTrac.TransTracking(cboViewBy.SelectedValue, txtDocNo.Text, txtVendor.Text, txtDateFr.Text, txtDateTo.Text, txtBuyer.Text, txtDept.Text)
            Case "PO"
                dtgTrans1.Columns(EnumTrans1.icDocNo).HeaderText = "PO Number"
                dtgTrans1.Columns(EnumTrans1.icCRNumber).HeaderText = "CR Number"
                dtgTrans1.Columns(EnumTrans1.icGRNNumber).Visible = True
                'dtgTrans.Columns(EnumTrans.icCoyName).Visible = True
                dtgTrans1.Columns(EnumTrans1.icCost).Visible = True
                dtgTrans1.Columns(EnumTrans1.icINVNumber).Visible = True
                dtgTrans1.Columns(EnumTrans1.icDONumber).Visible = True

				'Modified for Agora GST Stage 2 - CH - 2/2/2015
                If Request.QueryString("coytype").ToUpper = "VENDOR" Then
                    dtgTrans1.Columns(EnumTrans1.icPRNo).Visible = False
                    dtgTrans1.Columns(EnumTrans1.icBuyerName).Visible = False
                    dtgTrans1.Columns(EnumTrans1.icDNDANumber).Visible = True
                    dtgTrans1.Columns(EnumTrans1.icCNDANumber).Visible = True
                Else
                    dtgTrans1.Columns(EnumTrans1.icDNDANumber).Visible = False
                    dtgTrans1.Columns(EnumTrans1.icCNDANumber).Visible = False
                End If
				'------------------------------------------

                dtgTrans.Visible = False
                dtgTrans1.Visible = True
                If Request.QueryString("coytype").ToUpper = "VENDOR" Then
                    ds = objTrac.TransTracking_Vendor(cboViewBy.SelectedValue, txtDocNo.Text, txtVendor.Text, txtDateFr.Text, txtDateTo.Text, txtBuyer.Text)
                Else
                    ds = objTrac.TransTracking(cboViewBy.SelectedValue, txtDocNo.Text, txtVendor.Text, txtDateFr.Text, txtDateTo.Text, txtBuyer.Text, txtDept.Text)
                End If
            Case "GRN"
                'Michelle (30/9/2011)
                'dtgTrans.Columns(EnumTrans.icCost).Visible = False
                'dtgTrans.Columns(EnumTrans.icCoyName).Visible = True
                'dtgTrans.Columns(EnumTrans.icRelatedDoc).Visible = True
                'dtgTrans.Columns(EnumTrans.icDocNo).HeaderText = "GRN Number"
                'dtgTrans.Visible = True
                'dtgTrans1.Visible = False
                dtgTrans1.Columns(EnumTrans.icDocNo).HeaderText = "GRN Number"
                dtgTrans1.Columns(EnumTrans1.icCRNumber).HeaderText = "PO Number"
                dtgTrans1.Columns(EnumTrans1.icCRNumber).Visible = True
                dtgTrans1.Columns(EnumTrans1.icCost).Visible = False
                dtgTrans1.Columns(EnumTrans1.icGRNNumber).Visible = False
                dtgTrans1.Columns(EnumTrans1.icINVNumber).Visible = True
                dtgTrans1.Columns(EnumTrans1.icDONumber).Visible = True
				
				'Modified for Agora GST Stage 2 - CH - 2/2/2015
                If Request.QueryString("coytype").ToUpper = "VENDOR" Then
                    dtgTrans1.Columns(EnumTrans1.icDNDANumber).Visible = True
                    dtgTrans1.Columns(EnumTrans1.icCNDANumber).Visible = True
                Else
                    dtgTrans1.Columns(EnumTrans1.icDNDANumber).Visible = False
                    dtgTrans1.Columns(EnumTrans1.icCNDANumber).Visible = False
                End If
				'------------------------------------------

                dtgTrans.Visible = False
                dtgTrans1.Visible = True
                If Request.QueryString("coytype").ToUpper = "VENDOR" Then
                    ds = objTrac.TransTracking_Vendor(cboViewBy.SelectedValue, txtDocNo.Text, txtVendor.Text, txtDateFr.Text, txtDateTo.Text, txtBuyer.Text)
                Else
                    ds = objTrac.TransTracking(cboViewBy.SelectedValue, txtDocNo.Text, txtVendor.Text, txtDateFr.Text, txtDateTo.Text, txtBuyer.Text, txtDept.Text)
                End If
            Case "RFQ"
                dtgTrans.Visible = False
                dtgTrans1.Visible = False

                If Request.QueryString("coytype").ToUpper = "VENDOR" Then
                    dtgRFQ.Columns(EnumRfq.icPRNo).Visible = False
                    dtgRFQ.Columns(EnumRfq.icBuyerName).Visible = False
                End If

                ds = objTrac.TrackRFQ(txtDocNo.Text, txtVendor.Text, txtDateFr.Text, txtDateTo.Text, txtBuyer.Text, txtDept.Text)

            Case "DO"
                dtgTrans1.Visible = True
                dtgTrans.Visible = False
                dtgTrans1.Columns(EnumTrans1.icDONumber).Visible = False
                dtgTrans1.Columns(EnumTrans1.icCost).Visible = True
                dtgTrans1.Columns(EnumTrans1.icINVNumber).Visible = True
                dtgTrans1.Columns(EnumTrans1.icGRNNumber).Visible = True
                dtgTrans1.Columns(EnumTrans.icDocNo).HeaderText = "DO Number"
                dtgTrans1.Columns(EnumTrans1.icCRNumber).HeaderText = "PO Number"

				'Modified for Agora GST Stage 2 - CH - 2/2/2015
                If Request.QueryString("coytype").ToUpper = "VENDOR" Then
                    dtgTrans1.Columns(EnumTrans1.icDNDANumber).Visible = True
                    dtgTrans1.Columns(EnumTrans1.icCNDANumber).Visible = True
                Else
                    dtgTrans1.Columns(EnumTrans1.icDNDANumber).Visible = False
                    dtgTrans1.Columns(EnumTrans1.icCNDANumber).Visible = False
                End If
				'-------------------------------------------

                ds = objTrac.TransTracking_Vendor(cboViewBy.SelectedValue, txtDocNo.Text, txtVendor.Text, txtDateFr.Text, txtDateTo.Text, txtBuyer.Text)
            Case "INV"
                dtgTrans1.Visible = True
                dtgTrans.Visible = False
                dtgTrans1.Columns(EnumTrans1.icINVNumber).Visible = False
                dtgTrans1.Columns(EnumTrans1.icDONumber).Visible = True
                dtgTrans1.Columns(EnumTrans1.icGRNNumber).Visible = True
                dtgTrans1.Columns(EnumTrans1.icCost).Visible = True
                dtgTrans1.Columns(EnumTrans1.icDocNo).HeaderText = "Invoice Number"
                dtgTrans1.Columns(EnumTrans1.icCRNumber).HeaderText = "PO Number"

				'Modified for Agora GST Stage 2 - CH - 2/2/2015
                If Request.QueryString("coytype").ToUpper = "VENDOR" Then
                    dtgTrans1.Columns(EnumTrans1.icDNDANumber).Visible = True
                    dtgTrans1.Columns(EnumTrans1.icCNDANumber).Visible = True
                Else
                    dtgTrans1.Columns(EnumTrans1.icDNDANumber).Visible = False
                    dtgTrans1.Columns(EnumTrans1.icCNDANumber).Visible = False
                End If
				'--------------------------------------------

                ds = objTrac.TransTracking_Vendor(cboViewBy.SelectedValue, txtDocNo.Text, txtVendor.Text, txtDateFr.Text, txtDateTo.Text, txtBuyer.Text)
        End Select
        '//for sorting asc or desc
        Dim dvViewPR As DataView
        dvViewPR = ds.Tables(0).DefaultView
        'If cboViewBy.SelectedValue <> "RFQ" Then
        '    dtMatch = ds.Tables(1)
        'End If
        'dvViewSample(0)(0)
        If pSorted Then
            dvViewPR.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" Then dvViewPR.Sort += " DESC"
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCount") = intPageRecordCnt

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            If dtgTrans1.Visible = False Then
                resetDatagridPageIndex(dtgShare, dvViewPR)
                dtgShare.DataSource = dvViewPR
                dtgShare.DataBind()
                ViewState("PageCount") = dtgShare.PageCount
            Else
                resetDatagridPageIndex(dtgTrans1, dvViewPR)
                dtgTrans1.DataSource = dvViewPR
                dtgTrans1.DataBind()
                ViewState("PageCount") = dtgTrans1.PageCount
            End If

            'cmdPdf.Enabled = True
        Else
            'cmdPdf.Enabled = False
            If dtgTrans1.Visible = False Then
                dtgShare.DataBind()
                ViewState("PageCount") = dtgShare.PageCount
            Else
                dtgTrans1.DataBind()
                ViewState("PageCount") = dtgTrans1.PageCount
            End If

            Common.NetMsgbox(Me, MsgNoRecord)
            'intTotPage = 0
        End If

        ' add for above checking        
        ViewState("PrevDocNO") = ""
        ViewState("CurrDocNO") = ""

    End Function

    Private Sub dtgTrans_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgTrans.ItemCreated
        '//this line must be included
        intPageRecordCnt = ViewState("intPageRecordCount")
        Grid_ItemCreated(sender, e)
        'If e.Item.ItemType = ListItemType.Header Then
        '    Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
        '    chkAll.Attributes.Add("onclick", "selectAll();")
        'End If
        'dtgTrans.AlternatingItemStyle.BackColor = dtgTrans.BackColor
        'dtgTrans.GridLines = GridLines.None
        'dtgTrans.AlternatingItemStyle.
        '//to add a JavaScript to CheckAll button
        'If e.Item.ItemType = ListItemType.Header Then
        '    Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
        '    chkAll.Attributes.Add("onclick", "selectAll();")
        'End If
    End Sub
    Private Sub dtgTrans1_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgTrans1.ItemCreated
        '//this line must be included
        intPageRecordCnt = ViewState("intPageRecordCount")
        Grid_ItemCreated(sender, e)
        'dtgTrans.AlternatingItemStyle.
        '//to add a JavaScript to CheckAll button
        'If e.Item.ItemType = ListItemType.Header Then
        '    Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
        '    chkAll.Attributes.Add("onclick", "selectAll();")
        'End If
    End Sub

    Private Sub dtgRFQ_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgRFQ.ItemCreated
        '//this line must be included
        intPageRecordCnt = ViewState("intPageRecordCount")
        Grid_ItemCreated(sender, e)
        'If e.Item.ItemType = ListItemType.Header Then
        '    Dim chkAll As CheckBox = e.Item.FindControl("chkAll2")
        '    chkAll.Attributes.Add("onclick", "selectAll2();")
        'End If
        'dtgRFQ.AlternatingItemStyle.BackColor = dtgTrans.BackColor
        'dtgRFQ.GridLines = GridLines.None
        '//to add a JavaScript to CheckAll button
        'If e.Item.ItemType = ListItemType.Header Then
        '    Dim chkAll As CheckBox = e.Item.FindControl("chkAll_1")
        '    chkAll.Attributes.Add("onclick", "selectAll_1();")
        'End If
    End Sub
    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        If cboViewBy.SelectedValue = "RFQ" Then
            ViewState("SortExpression") = "RM_RFQ_NO"
            ViewState("SortAscending") = "no"
            'cmdPdf.Attributes.Add("onclick", "return CheckAtLeastOnePdf('chkSelection2');")
        Else
            ViewState("SortExpression") = "DOC_No"
            ViewState("SortAscending") = "no"
            'cmdPdf.Attributes.Add("onclick", "return CheckAtLeastOnePdf('chkSelection');")
        End If
        ViewState("viewBy") = cboViewBy.SelectedValue
        dtgShare.CurrentPageIndex = 0
        Bindgrid(True)
        'ReBindgrid()
    End Sub
    Sub ReBindgrid()
        Dim dgItem As DataGridItem
        Dim strHideShow As String
        Dim i As Integer

        For Each dgItem In dtgTrans.Items
            strHideShow = dgItem.Cells(EnumTrans.icHideShow).Text
            If strHideShow = "0" Then
                For i = 0 To dgItem.Cells.Count - 1
                    dgItem.Cells.RemoveAt(0)
                Next
                ViewState("intPageRecordCount") = ViewState("intPageRecordCount") - 1
                intPageRecordCnt = intPageRecordCnt - 1
            End If
        Next
        resetDatagridPageIndexM(dtgTrans, intPageRecordCnt)

    End Sub
    Sub BuildURL()
        If Request.QueryString("coytype").ToUpper = "BUYER" Then
            'strURLPR = dDispatcher.direct("PR", "PRDetail.aspx", "type=mod&mode=bc&coytype=" & Request.QueryString("coytype") & "&Frm=transtracking" & "&PageID=115&index=" & intPRIdx & "&PRNo=" & strPRNo)
            strURLPR = dDispatcher.direct("PR", "PRDetailTransTracking.aspx", "type=mod&mode=bc&coytype=" & Request.QueryString("coytype") & "&Frm=transtracking" & "&PageID=" & strPageId & "&index=" & intPRIdx & "&PRNo=" & strPRNo)
            'strURLPO = dDispatcher.direct("PO", "PODetail.aspx", "caller=" & "buyer" & "&PO_INDEX=" & intPoIdx & "&PO_NO=" & strPONo & "&BCoyID=" & strBuyerCoy & "&side=" & "other" & "&filetype=" & "2" & "&PageID=" & strPageId)
            strURLPO = dDispatcher.direct("Report", "PreviewPO.aspx", "PO_NO=" & strPONo & "&BCoyID=" & strBuyerCoy & "&side=" & "other" & "&filetype=" & "2" & "&PageID=" & strPageId)
            strURLDO = dDispatcher.direct("DO", "PreviewDODetails.aspx", "caller=" & "buyer" & "&DONo=" & strDONo & "&POIdx=" & intPoIdx & "&SCoyID=" & strVenCoy & "&PageID=" & strPageId)
            'strURLDO = dDispatcher.direct("Report", "PreviewDO.aspx", "SCoyID=" & strVenCoy & "&DONo=" & strDONo & "&PO_NO=" & strPONo)
            'strURLINV = ""
            'strURLGRN = dDispatcher.direct("GRN", "GRNDetails.aspx", "GRNNo=" & strGRNNo & "&BCoyID=" & strBuyerCoy & "&type=" & "other" & "&pageid=" & strPageId)
            strURLGRN = dDispatcher.direct("Report", "PreviewGRN.aspx", "pageid=" & strPageId & "&GRNNo=" & strGRNNo & "&PONo=" & strPONo & "&DONo=" & strDONo & "&BCoyID=" & strBuyerCoy)
        Else
            strURLPR = ""
            strURLPO = dDispatcher.direct("Report", "PreviewPO.aspx", "PO_No=" & strPONo & "&BCoyID=" & strBuyerCoy & "&PageID=" & strPageId)
            'strURLDO = dDispatcher.direct("Report", "PreviewDO.aspx", "DONo=" & strDONo & "&POIdx=" & intPoIdx & "&SCoyID=" & strVenCoy & "&PageID=" & strPageId)
            strURLDO = dDispatcher.direct("Report", "PreviewDO.aspx", "SCoyID=" & strVenCoy & "&DONo=" & strDONo & "&PO_NO=" & strPONo)
            strURLINV = dDispatcher.direct("Report", "PreviewInvoice.aspx", "INVNO=" & strInvNo & "&vcomid=" & strVenCoy & "&BCoyID=" & strBuyerCoy & "&pageid=" & strPageId)
            'strURLGRN = dDispatcher.direct("GRN", "GRNDetails.aspx", "GRNNo=" & strGRNNo & "&BCoyID=" & strBuyerCoy & "&type=" & "v" & "&pageid=" & strPageId)
            strURLGRN = dDispatcher.direct("Report", "PreviewGRN.aspx", "pageid=" & strPageId & "&GRNNo=" & strGRNNo & "&PONo=" & strPONo & "&DONo=" & strDONo & "&BCoyID=" & strBuyerCoy)
        End If
    End Sub
    Sub WritePRURL(ByRef strTemp As String)
        Dim drRow As DataRow
        Dim strURL As String
        'Michelle (1/8/2007) - To cater for mutliple POs
        'dtPR = objTrac.getRelatedPR(intPoIdx)
        dtPR = objTrac.getRelatedPR_PO(strPONo, strBCoyID)
        For Each drRow In dtPR.Rows
            intPRIdx = drRow("PRM_PR_INDEX")
            strPRNo = drRow("PRM_PR_NO")
            BuildURL()
            strURL = "<a href=" & strURLPR & ">" & strPRNo & "</a>"
            If strTemp = "" Then
                strTemp = strURL
            Else
                strTemp = strTemp & "<BR>" & strURL
            End If
        Next
    End Sub
    Sub WriteInvURL(ByRef strTemp As String)
        Dim strTemp2 As String = ""
        If cboViewBy.SelectedValue = "PR" Or cboViewBy.SelectedValue = "PO" Then
            strTemp2 = " --><br/>"
        Else
            strTemp2 = "<br/>"
        End If
        If Request.QueryString("coytype").ToUpper = "BUYER" Then
            strTemp = strTemp & strTemp2 & strInvNo
        Else
            BuildURL()
            strTemp = strTemp & strTemp2 & "<a href=""#"" onclick=""PopWindow('" & strURLINV & "')"">" & strInvNo & "</a>"
        End If
    End Sub
    Private Sub dtgTrans_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgTrans.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim STR3, strPOM_PO_Index As String

            'Dim chk As CheckBox
            'chk = e.Item.Cells(0).FindControl("chkSelection")
            'chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            Dim drMatch() As DataRow
            Dim drRow As DataRow
            Dim strTemp As String = ""
            Dim strTemp1 As String = ""
            Dim strURL As String

            '//******************************          
            Dim lnkDocNo As HyperLink
            '//******************************

            'First Column, may Be -> PR, PO,DO, GRN,Inv,RFQ
            lnkDocNo = e.Item.Cells(EnumTrans.icDocNo).FindControl("lnkDocNo")
            lnkDocNo.Text = dv("DOC_No")

            '//////////////////////////
            'ViewState("CurrDocNO") = lnkDocNo.Text
            'If ViewState("PrevDocNO") = "" Then
            '    ViewState("PrevDocNO") = ViewState("CurrDocNO")
            '    e.Item.Cells(EnumTrans.icHideShow).Text = "1" 'ie show
            'ElseIf ViewState("PrevDocNO") = ViewState("CurrDocNO") Then
            '    'dv.DataView.Item(0).Delete()
            '    e.Item.Cells(EnumTrans.icHideShow).Text = "0" 'ie hide
            '    Exit Sub
            'Else
            '    e.Item.Cells(EnumTrans.icHideShow).Text = "1" 'ie show
            'End If
            'ViewState("PrevDocNO") = ViewState("CurrDocNO")
            'DataGridView1.Rows.Remove(DataGridView1.CurrentRow)
            '///////////////////////////////

            'ai chu add
            Dim hidDocNo As HtmlInputHidden
            hidDocNo = e.Item.FindControl("hidDocNo")

            If Request.QueryString("coytype").ToUpper = "BUYER" Then
                strBuyerCoy = Session("CompanyID")
                If Not IsDBNull(dv("VEN_ID")) Then
                    strVenCoy = dv("VEN_ID")
                End If
            Else
                strBuyerCoy = dv("BUYER_COY")
                strVenCoy = Session("CompanyID")
            End If

            e.Item.Cells(EnumTrans.icBuyerCoy).Text = strBuyerCoy

            'Sam start here
            'Dim relPR As DataRelation
            'relPR = New DataRelation("tableChapter1", parentCol, childCol)



            'Dim dvMatch As DataView = dv.CreateChildView("tableChapter1")
            Dim dvMatch As DataView = dv.DataView
            Dim intLoop, intCnt As Integer

            intCnt = dvMatch.Count - 1
            If cboViewBy.SelectedValue = "PR" Then 'Only accessed by Buyer
                intPoIdx = dv("POM_PO_INDEX")
                strPONo = dv("PO_NUMBER")
                intPRIdx = dv("DOC_INDEX")
                strPRNo = dv("DOC_No")
                strBCoyID = dv("POM_B_COY_ID")
                BuildURL()
                lnkDocNo.NavigateUrl = strURLPR
                hidDocNo.Value = intPRIdx
                'strTemp = "<a href='#' onclick=""PopWindow('" & strURLPO & "')"">" & dv("PO_NUMBER") & "</a>"

                If Request.QueryString("coytype").ToUpper = "BUYER" Then
                    strPOM_PO_Index = objDB.GetVal("SELECT IFNULL(POM_PO_INDEX,'') AS POM_PO_NO FROM PO_MSTR WHERE POM_PO_NO = '" & dv("PO_NUMBER") & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                    strTemp = strTemp & "<A href=""" & dDispatcher.direct("PO", "PODetail.aspx", "Frm=transtrack&pageid=" & strPageId & "&PO_INDEX=" & strPOM_PO_Index & "&PO_NO=" & dv("PO_NUMBER") & "&BCoyID=" & HttpContext.Current.Session("CompanyId") & "&status=&Caller=transtrack&side=b&filetype=2&type=" & "" & "&poview=1&checkid=no") & """ ><font color=#0000ff>" & dv("PO_NUMBER") & "</font></A><br/>"
                Else
                    strTemp = "<a href='#' onclick=""PopWindow('" & strURLPO & "')"">" & dv("PO_NUMBER") & "</a>"
                End If

                e.Item.Cells(EnumTrans.icPONumber).Text = strTemp
            End If

            If cboViewBy.SelectedValue = "NPR" Then 'Only accessed by Buyer
                Dim strPOStatus = dv("PO_STATUS")
                Dim strRFQStatus = dv("RM_STATUS")
                intPRIdx = dv("DOC_INDEX")
                strPRNo = dv("DOC_No")
                strBCoyID = Session("CompanyID")
                If Not IsDBNull(dv("POM_PO_INDEX")) Then
                    strPONo = dv("PO_NUMBER")
                End If
                BuildURL()
                lnkDocNo.NavigateUrl = strURLPR
                hidDocNo.Value = intPRIdx
                If Not IsDBNull(dv("POM_PO_INDEX")) Then
                    If strPOStatus <> "0" Then
                        intPoIdx = dv("POM_PO_INDEX")
                        ' strPONo = dv("PO_NUMBER")
                        'strTemp = "<a href='#' onclick=""PopWindow('" & strURLPO & "')"">" & dv("PO_NUMBER") & "</a>"

                        If Request.QueryString("coytype").ToUpper = "BUYER" Then
                            strPOM_PO_Index = objDB.GetVal("SELECT IFNULL(POM_PO_INDEX,'') AS POM_PO_NO FROM PO_MSTR WHERE POM_PO_NO = '" & dv("PO_NUMBER") & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                            strTemp = strTemp & "<A href=""" & dDispatcher.direct("PO", "PODetail.aspx", "Frm=transtrack&pageid=" & strPageId & "&PO_INDEX=" & strPOM_PO_Index & "&PO_NO=" & dv("PO_NUMBER") & "&BCoyID=" & HttpContext.Current.Session("CompanyId") & "&status=&Caller=transtrack&side=b&filetype=2&type=" & "" & "&poview=1&checkid=no") & """ ><font color=#0000ff>" & dv("PO_NUMBER") & "</font></A><br/>"
                        Else
                            strTemp = "<a href='#' onclick=""PopWindow('" & strURLPO & "')"">" & dv("PO_NUMBER") & "</a>"
                        End If

                        e.Item.Cells(EnumTrans.icPONumber).Text = strTemp
                    Else
                        e.Item.Cells(EnumTrans.icPONumber).Text = dv("PO_NUMBER") + " (Draft)"
                    End If
                End If
                If dv("RFQ_No") <> "" Then
                    If strRFQStatus <> "3" Then
                        e.Item.Cells(EnumTrans.icHideShow).Text = "<a href=" & dDispatcher.direct("RFQ", "RFQDetail.aspx", "FRM=transtracking&page=1&RFQ_Num=" & dv("RFQ_No") & "&RFQ_ID=" & dv("RFQ_ID")) & ">" & dv("RFQ_No") & "</a>"
                    Else
                        e.Item.Cells(EnumTrans.icHideShow).Text = dv("RFQ_No") + " (Draft)"
                    End If
                End If
            End If

                ' If cboViewBy.SelectedValue = "PR" Or cboViewBy.SelectedValue = "PO" Then
                If cboViewBy.SelectedValue = "PO" Then
                    If cboViewBy.SelectedValue = "PR" Then 'Only accessed by Buyer
                        intPoIdx = dv("POM_PO_INDEX")
                        strPONo = dv("PO_NO")
                        intPRIdx = dv("DOC_INDEX")
                        strPRNo = dv("DOC_No")
                        strBCoyID = dv("POM_B_COY_ID")
                        BuildURL()
                        lnkDocNo.NavigateUrl = strURLPR
                        hidDocNo.Value = intPRIdx
                    'strTemp = "<a href=" & strURLPO & ">" & dv("PO_NO") & "</a>"

                    If Request.QueryString("coytype").ToUpper = "BUYER" Then
                        strPOM_PO_Index = objDB.GetVal("SELECT IFNULL(POM_PO_INDEX,'') AS POM_PO_NO FROM PO_MSTR WHERE POM_PO_NO = '" & dv("PO_NO") & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                        strTemp = strTemp & "<A href=""" & dDispatcher.direct("PO", "PODetail.aspx", "Frm=transtrack&pageid=" & strPageId & "&PO_INDEX=" & strPOM_PO_Index & "&PO_NO=" & dv("PO_NO") & "&BCoyID=" & HttpContext.Current.Session("CompanyId") & "&status=&Caller=transtrack&side=b&filetype=2&type=" & "" & "&poview=1&checkid=no") & """ ><font color=#0000ff>" & dv("PO_NO") & "</font></A><br/>"
                    Else
                        strTemp = "<a href=" & strURLPO & ">" & dv("PO_NO") & "</a>"
                    End If

                    Else
                        intPoIdx = dv("DOC_INDEX")
                        strPONo = dv("DOC_No")
                        strBCoyID = dv("POM_B_COY_ID")
                        hidDocNo.Value = intPoIdx
                        If Request.QueryString("coytype").ToUpper = "BUYER" Then
                            BuildURL()
                            'lnkDocNo.NavigateUrl = strURLPO
                            'WritePRURL(strTemp)
                            lnkDocNo.NavigateUrl = "javascript:;"
                            lnkDocNo.Attributes.Add("onclick", "return PopWindow('" & strURLPO & "')")
                        Else
                            BuildURL()
                            lnkDocNo.NavigateUrl = "javascript:;"
                            lnkDocNo.Attributes.Add("onclick", "return PopWindow('" & strURLPO & "')")
                        End If
                    End If
                    'If Not sPrevPONo = strPONo Then
                    '    sPrevPONo = strPONo
                    'End If
                    'iFixBugs, remove the looping   sPrevDONo
                    For intLoop = 0 To intCnt
                        If Not IsDBNull(dvMatch(intLoop)("CDM_DO_NO")) Then
                            If ViewState("CurrDocNO") = dvMatch(intLoop)("DOC_NO") Then
                                strDONo = dvMatch(intLoop)("CDM_DO_NO")

                                ''########## START #############
                                If Not sPrevDONo = strDONo Then
                                    sPrevDONo = strDONo
                                Else
                                    Exit For
                                End If
                                '########## END #############

                                strVenCoy = dvMatch(intLoop)("CDM_S_COY_ID")

                                '//SR : AS0021, Moo, 26/10/2005     
                                BuildURL()
                                If strTemp <> "" Then
                                    strTemp = strTemp & "<BR>"
                                End If
                                strTemp = strTemp & "<a href=""#"" onclick=""PopWindow('" & strURLDO & "')"">" & strDONo & "</a>"

                                If Not IsDBNull(dvMatch(intLoop)("CDM_GRN_NO")) Then
                                    strGRNNo = dvMatch(intLoop)("CDM_GRN_NO")
                                    BuildURL()
                                    'strTemp = strTemp & " --> <br/><a href=" & strURLGRN & ">" & strGRNNo & "</a>"
                                    strTemp = strTemp & " --> <br/><a href=""#"" onclick=""PopWindow('" & strURLGRN & "')"">" & strGRNNo & "</a>"
                                End If
                                If Not IsDBNull(dvMatch(intLoop)("CDM_INVOICE_NO")) Then
                                    strInvNo = dvMatch(intLoop)("CDM_INVOICE_NO")
                                    WriteInvURL(strTemp)
                                End If
                            End If
                        End If
                    Next
                ElseIf cboViewBy.SelectedValue = "GRN" Then
                    strGRNNo = dv("DOC_No")
                    hidDocNo.Value = Common.parseNull(dv("DO_INDEX"))
                    intPoIdx = dv("POM_PO_INDEX")
                    strPONo = dv("PO_NO")
                    strDONo = dv("CDM_DO_NO")
                    BuildURL()
                    'lnkDocNo.NavigateUrl = strURLGRN
                    lnkDocNo.NavigateUrl = "javascript:;"
                    lnkDocNo.Attributes.Add("onclick", "return PopWindow('" & strURLGRN & "')")

                    If Request.QueryString("coytype").ToUpper = "BUYER" Then
                        WritePRURL(strTemp)
                        'strTemp = strTemp & "<BR>"
                        'strTemp = strTemp & "<a href=" & strURLPO & ">" & strPONo & "</a>"
                    'strTemp = strTemp & "<a href=""#"" onclick=""PopWindow('" & strURLPO & "')"">" & strPONo & "</a>"

                    strPOM_PO_Index = objDB.GetVal("SELECT IFNULL(POM_PO_INDEX,'') AS POM_PO_NO FROM PO_MSTR WHERE POM_PO_NO = '" & strPONo & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                    strTemp = strTemp & "<A href=""" & dDispatcher.direct("PO", "PODetail.aspx", "Frm=transtrack&pageid=" & strPageId & "&PO_INDEX=" & strPOM_PO_Index & "&PO_NO=" & strPONo & "&BCoyID=" & HttpContext.Current.Session("CompanyId") & "&status=&Caller=transtrack&side=b&filetype=2&type=" & "" & "&poview=1&checkid=no") & """ ><font color=#0000ff>" & strPONo & "</font></A><br/>"

                    Else
                        strTemp = strTemp & "<a href=""#"" onclick=""PopWindow('" & strURLPO & "')"">" & strPONo & "</a>"
                    End If

                    'xxxxxx - need to change it to For loop and assign the no. only if the current doc_no = doc_no
                    For intLoop = 0 To intCnt
                        'If intCnt >= 0 Then
                        'If ViewState("CurrDocNO") = dvMatch(intLoop)("DOC_NO") Then
                        'If Not IsDBNull(dvMatch(intLoop)("CDM_DO_NO")) Then
                        strDONo = dvMatch(intLoop)("CDM_DO_NO")
                        strVenCoy = dvMatch(intLoop)("CDM_S_COY_ID")
                        '//SR : AS0021, MOo, 26/10/2005
                        BuildURL()
                        strTemp = strTemp & "<BR><a href=""#"" onclick=""PopWindow('" & strURLDO & "')"">" & strDONo & "</a>"

                        If Not IsDBNull(dvMatch(intLoop)("CDM_INVOICE_NO")) Then
                            strInvNo = dvMatch(intLoop)("CDM_INVOICE_NO")
                            WriteInvURL(strTemp)
                        End If
                        'End If
                        'End If
                    Next
                ElseIf cboViewBy.SelectedValue = "DO" Then
                    strDONo = dv("DOC_No")
                    hidDocNo.Value = Common.parseNull(dv("DOC_INDEX"))
                    intPoIdx = dv("POM_PO_INDEX")
                    strPONo = dv("PO_NO")
                    BuildURL()
                    'lnkDocNo.NavigateUrl = strURLDO
                    lnkDocNo.NavigateUrl = "javascript:;"
                    lnkDocNo.Attributes.Add("onclick", "return PopWindow('" & strURLDO & "')")
                    If Request.QueryString("coytype").ToUpper = "BUYER" Then
                    'strTemp = strTemp & "<a href=" & strURLPO & ">" & strPONo & "</a>"

                    If Request.QueryString("coytype").ToUpper = "BUYER" Then
                        strPOM_PO_Index = objDB.GetVal("SELECT IFNULL(POM_PO_INDEX,'') AS POM_PO_NO FROM PO_MSTR WHERE POM_PO_NO = '" & strPONo & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                        strTemp = strTemp & "<A href=""" & dDispatcher.direct("PO", "PODetail.aspx", "Frm=transtrack&pageid=" & strPageId & "&PO_INDEX=" & strPOM_PO_Index & "&PO_NO=" & strPONo & "&BCoyID=" & HttpContext.Current.Session("CompanyId") & "&status=&Caller=transtrack&side=b&filetype=2&type=" & "" & "&poview=1&checkid=no") & """ ><font color=#0000ff>" & strPONo & "</font></A><br/>"
                    Else
                        strTemp = strTemp & "<a href=" & strURLPO & ">" & strPONo & "</a>"
                    End If

                    Else
                        strTemp = strTemp & "<a href=""#"" onclick=""PopWindow('" & strURLPO & "')"">" & strPONo & "</a>"
                    End If

                    'If intCnt >= 0 Then
                    'If Not IsDBNull(dvMatch(intLoop)("CDM_GRN_NO")) Then
                    For intLoop = 0 To intCnt
                        If ViewState("CurrDocNO") = dvMatch(intLoop)("DOC_NO") Then
                            If Not IsDBNull(dvMatch(intLoop)("CDM_GRN_NO")) Then
                                strGRNNo = dvMatch(intLoop)("CDM_GRN_NO")
                                strVenCoy = dvMatch(intLoop)("CDM_S_COY_ID")
                                '//SR : AS0021, MOo, 26/10/2005
                                BuildURL()
                                'strTemp = strTemp & "<BR><a href=" & strURLGRN & ">" & strGRNNo & "</a>"
                                strTemp = strTemp & "<BR><a href=""#"" onclick=""PopWindow('" & strURLGRN & "')"">" & strGRNNo & "</a>"
                            End If
                            If Not IsDBNull(dvMatch(intLoop)("CDM_INVOICE_NO")) Then
                                strInvNo = dvMatch(intLoop)("CDM_INVOICE_NO")
                                WriteInvURL(strTemp)
                            End If
                        End If
                    Next
                ElseIf cboViewBy.SelectedValue = "INV" Then
                    strInvNo = dv("DOC_No")
                    hidDocNo.Value = Common.parseNull(dv("DOC_INDEX"))
                    intPoIdx = dv("POM_PO_INDEX")
                    strPONo = dv("PO_NO")
                    BuildURL()
                    'lnkDocNo.NavigateUrl = strURLDO
                    lnkDocNo.NavigateUrl = "javascript:;"
                    lnkDocNo.Attributes.Add("onclick", "return PopWindow('" & strURLINV & "')")

                    If Request.QueryString("coytype").ToUpper = "BUYER" Then
                    'strTemp = strTemp & "<a href=" & strURLPO & ">" & strPONo & "</a>"

                    If Request.QueryString("coytype").ToUpper = "BUYER" Then
                        strPOM_PO_Index = objDB.GetVal("SELECT IFNULL(POM_PO_INDEX,'') AS POM_PO_NO FROM PO_MSTR WHERE POM_PO_NO = '" & strPONo & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                        strTemp = strTemp & "<A href=""" & dDispatcher.direct("PO", "PODetail.aspx", "Frm=transtrack&pageid=" & strPageId & "&PO_INDEX=" & strPOM_PO_Index & "&PO_NO=" & strPONo & "&BCoyID=" & HttpContext.Current.Session("CompanyId") & "&status=&Caller=transtrack&side=b&filetype=2&type=" & "" & "&poview=1&checkid=no") & """ ><font color=#0000ff>" & strPONo & "</font></A><br/>"
                    Else
                        strTemp = strTemp & "<a href=" & strURLPO & ">" & strPONo & "</a>"
                    End If

                    Else
                        strTemp = strTemp & "<a href=""#"" onclick=""PopWindow('" & strURLPO & "')"">" & strPONo & "</a>"
                    End If

                    For intLoop = 0 To intCnt
                        If ViewState("CurrDocNO") = dvMatch(intLoop)("DOC_NO") Then
                            If Not IsDBNull(dvMatch(intLoop)("CDM_DO_NO")) Then
                                strDONo = dvMatch(intLoop)("CDM_DO_NO")
                                strVenCoy = dvMatch(intLoop)("CDM_S_COY_ID")

                                '//SR : AS0021, Moo, 26/10/2005     
                                BuildURL()
                                strTemp = strTemp & "<BR><a href=""#"" onclick=""PopWindow('" & strURLDO & "')"">" & strDONo & "</a>"

                                If Not IsDBNull(dvMatch(intLoop)("CDM_GRN_NO")) Then
                                    strGRNNo = dvMatch(intLoop)("CDM_GRN_NO")
                                    BuildURL()
                                    'strTemp = strTemp & "<BR><a href=" & strURLGRN & ">" & strGRNNo & "</a>"
                                    strTemp = strTemp & "<BR><a href=""#"" onclick=""PopWindow('" & strURLGRN & "')"">" & strGRNNo & "</a>"
                                End If
                            End If
                        End If
                    Next
                End If

                'Michelle (1/8/2007) - To cater for multiple POs
                'dtPR = objTrac.getRelatedCR(intPoIdx)
                dtPR = objTrac.getRelatedCR_PO(strPONo, strBCoyID)
                If Not dtPR Is Nothing AndAlso dtPR.Rows.Count > 0 Then
                    For Each drRow In dtPR.Rows
                        'Michelle (20/7/2007) - To rectify the problem
                        ' strTemp = strTemp & "<BR><a href=../PO/PODetail.aspx?caller=buyer&cr_no=" & drRow("PCM_CR_NO") & "&PO_INDEX=" & intPoIdx & "&PO_NO=" & strPONo & "&BCoyID=" & Session("CompanyID") & "&side=other&filetype=1&PageID=" & strPageId & ">" & drRow("PCM_CR_NO") & "</a>"
                        If strTemp <> "" Then
                            strTemp = strTemp & "<BR><a href=" & dDispatcher.direct("PO", "PODetail.aspx", "caller=" & "vendor" & "&cr_no=" & drRow("PCM_CR_NO") & "&PO_INDEX=" & intPoIdx & "&PO_NO=" & strPONo & "&BCoyID=" & strBuyerCoy & "&side=" & "other" & "&filetype=" & "1" & "&PageID=" & strPageId) & ">" & drRow("PCM_CR_NO") & "</a>"
                        Else
                            strTemp = strTemp & "<a href=" & dDispatcher.direct("PO", "PODetail.aspx", "caller=" & "vendor" & "&cr_no=" & drRow("PCM_CR_NO") & "&PO_INDEX=" & intPoIdx & "&PO_NO=" & strPONo & "&BCoyID=" & strBuyerCoy & "&side=" & "other" & "&filetype=" & "1" & "&PageID=" & strPageId) & ">" & drRow("PCM_CR_NO") & "</a>"
                        End If
                    Next
                End If
                e.Item.Cells(EnumTrans.icRelatedDoc).Text = strTemp
                e.Item.Cells(EnumTrans.icDocDate).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, e.Item.Cells(EnumTrans.icDocDate).Text)
                e.Item.Cells(EnumTrans.icCost).Text = Format(CDbl(e.Item.Cells(EnumTrans.icCost).Text), "###,##0.00")
                'ElseIf e.Item.ItemType = ListItemType.Header Then
                '   e.Item.Cells(5).Text = "Related Document"
            End If
    End Sub
    Private Sub dtgTrans1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgTrans1.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim STR3, strPOM_PO_Index As String
            Dim StrDN, StrCN As String 'Modified for Agora GST Stage 2 - CH - 2/2/2015
            intPoIdx = dv("DOC_INDEX")
            'strPONo = dv("DOC_No")
            strPONo = dv("CDM_PO_NO")
            strBCoyID = dv("POM_B_COY_ID")
            If cboViewBy.SelectedValue = "INV" Then strInvNo = dv("CDM_INVOICE_NO")
            If Not IsDBNull(dv("CDM_GRN_NO")) Then
                strGRNNo = dv("CDM_GRN_NO")
            End If
            If Not IsDBNull(dv("CDM_DO_NO")) Then
                strDONo = dv("CDM_DO_NO")
            End If
            If Request.QueryString("coytype").ToUpper = "BUYER" Then
                strBuyerCoy = Session("CompanyID")
                strVenCoy = dv("VEN_ID")
            Else
                strBuyerCoy = dv("BUYER_COY")
                strVenCoy = Session("CompanyID")
                'e.Item.Cells(EnumTrans1.icCoyName).Text = 
            End If
            'Michelle (10/2/2012) - Issue 1478
            If cboViewBy.SelectedValue = "PO" Then
                If strPONo = "" Then
                    strPONo = dv("DOC_NO")
                End If
            End If
            BuildURL()

            'Michelle (5/10/2011)
            Select Case cboViewBy.SelectedValue
                Case "PO"
                    'e.Item.Cells(EnumTrans1.icDocNo).Text = "<a href='#' onclick=""PopWindow('" & strURLPO & "')"">" & dv("DOC_NO") & "</a>"
                    If Request.QueryString("coytype").ToUpper = "BUYER" Then
                        strPOM_PO_Index = objDB.GetVal("SELECT IFNULL(POM_PO_INDEX,'') AS POM_PO_NO FROM PO_MSTR WHERE POM_PO_NO = '" & dv("DOC_NO") & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")

                        STR3 = STR3 & "<A href=""" & dDispatcher.direct("PO", "PODetail.aspx", "Frm=transtrack&pageid=" & strPageId & "&PO_INDEX=" & strPOM_PO_Index & "&PO_NO=" & dv("DOC_NO") & "&BCoyID=" & HttpContext.Current.Session("CompanyId") & "&status=&Caller=transtrack&side=b&filetype=2&type=" & "" & "&poview=1&checkid=no") & """ ><font color=#0000ff>" & dv("DOC_NO") & "</font></A><br/>"
                        e.Item.Cells(EnumTrans1.icDocNo).Text = STR3
                    Else
                        e.Item.Cells(EnumTrans1.icDocNo).Text = "<a href='#' onclick=""PopWindow('" & strURLPO & "')"">" & dv("DOC_NO") & "</a>"
                    End If
                Case "DO"
                    e.Item.Cells(EnumTrans1.icDocNo).Text = "<a href='#' onclick=""PopWindow('" & strURLDO & "')"">" & dv("DOC_NO") & "</a>"
                Case "GRN"
                    e.Item.Cells(EnumTrans1.icDocNo).Text = "<a href='#' onclick=""PopWindow('" & strURLGRN & "')"">" & dv("DOC_NO") & "</a>"
                Case "INV"
                    e.Item.Cells(EnumTrans1.icDocNo).Text = "<a href='#' onclick=""PopWindow('" & strURLINV & "')"">" & dv("DOC_NO") & "</a>"
            End Select
            'e.Item.Cells(EnumTrans1.icDocNo).Text = "<a href='#' onclick=""PopWindow('" & strURLPO & "')"">" & dv("DOC_NO") & "</a>"
            'If cboViewBy.SelectedValue <> "GRN" Then
            If Not IsDBNull(dv("PCM_CR_NO")) Then
                If dv("PCM_CR_NO") <> "" Then
                    e.Item.Cells(EnumTrans1.icCRNumber).Text = "<a href=" & dDispatcher.direct("PO", "PODetail.aspx", "caller=" & "vendor" & "&cr_no=" & dv("PCM_CR_NO") & "&PO_INDEX=" & intPoIdx & "&PO_NO=" & strPONo & "&BCoyID=" & strBuyerCoy & "&side=" & "other" & "&filetype=" & "1" & "&PageID=" & strPageId) & ">" & dv("PCM_CR_NO") & "</a>"
                Else
                    'e.Item.Cells(EnumTrans1.icCRNumber).Text = "<a href='#' onclick=""PopWindow('" & strURLPO & "')"">" & dv("CDM_PO_NO") & "</a>"
                    Select Case cboViewBy.SelectedValue
                        Case "GRN"
                            If Request.QueryString("coytype").ToUpper = "BUYER" Then
                                strPOM_PO_Index = objDB.GetVal("SELECT IFNULL(POM_PO_INDEX,'') AS POM_PO_NO FROM PO_MSTR WHERE POM_PO_NO = '" & dv("CDM_PO_NO") & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")

                                STR3 = STR3 & "<A href=""" & dDispatcher.direct("PO", "PODetail.aspx", "Frm=transtrack&pageid=" & strPageId & "&PO_INDEX=" & strPOM_PO_Index & "&PO_NO=" & dv("CDM_PO_NO") & "&BCoyID=" & HttpContext.Current.Session("CompanyId") & "&status=&Caller=transtrack&side=b&filetype=2&type=" & "" & "&poview=1&checkid=no") & """ ><font color=#0000ff>" & dv("CDM_PO_NO") & "</font></A><br/>"
                                e.Item.Cells(EnumTrans1.icCRNumber).Text = STR3
                            Else
                                e.Item.Cells(EnumTrans1.icCRNumber).Text = "<a href='#' onclick=""PopWindow('" & strURLPO & "')"">" & dv("CDM_PO_NO") & "</a>"
                            End If
                        Case Else
                            e.Item.Cells(EnumTrans1.icCRNumber).Text = "<a href='#' onclick=""PopWindow('" & strURLPO & "')"">" & dv("CDM_PO_NO") & "</a>"
                    End Select
                End If
            Else
                If cboViewBy.SelectedValue <> "PO" Then
                    e.Item.Cells(EnumTrans1.icCRNumber).Text = "<a href='#' onclick=""PopWindow('" & strURLPO & "')"">" & dv("CDM_PO_NO") & "</a>"
                End If

            End If

            e.Item.Cells(EnumTrans1.icDONumber).Text = "<a href='#' onclick=""PopWindow('" & strURLDO & "')"">" & dv("CDM_DO_NO") & "</a>"
            e.Item.Cells(EnumTrans1.icGRNNumber).Text = "<a href='#' onclick=""PopWindow('" & strURLGRN & "')"">" & dv("CDM_GRN_NO") & "</a>"

            If Request.QueryString("coytype").ToUpper <> "BUYER" Then
                strURLINV = dDispatcher.direct("Report", "PreviewInvoice.aspx", "INVNO=" & dv("CDM_INVOICE_NO") & "&vcomid=" & strVenCoy & "&BCoyID=" & strBuyerCoy & "&pageid=" & strPageId)
                e.Item.Cells(EnumTrans1.icINVNumber).Text = "<a href='#' onclick=""PopWindow('" & strURLINV & "')"">" & dv("CDM_INVOICE_NO") & "</a>"
            End If

            e.Item.Cells(EnumTrans1.icDocDate).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, e.Item.Cells(EnumTrans1.icDocDate).Text)
            e.Item.Cells(EnumTrans1.icCost).Text = Format(CDbl(e.Item.Cells(EnumTrans1.icCost).Text), "###,##0.00")

            Dim COUNT, i As Integer
            Dim ARRAY(100), ARRAY2(100), Str, Str2 As String
            Dim strPR_Index, strPR_No, strPR_Name, strPRD_RFQ_Index, strRFQ_No As String
            Dim objPO As New PurchaseOrder

            objTrac.getRelatedPR_PO(dv("DOC_NO"), ARRAY, ARRAY2, COUNT)   'Get PO Number
            If ARRAY(0) <> "" Then
                For i = 0 To COUNT - 1
                    strPR_Index = objDB.GetVal("SELECT PRM_PR_INDEX FROM PR_MSTR WHERE PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRM_PR_NO = '" & ARRAY(i) & "'")
                    If strPR_Index <> "" Then

                        Str = Str & "<A href=""" & dDispatcher.direct("PR", "PRDetail.aspx", "pageid=" & strPageId & "&Frm=transtracking2&index=" & strPR_Index & "&PRNO=" & ARRAY(i) & "&type=mod&mode=bc") & """ ><font color=#0000ff>" & ARRAY(i) & "</font></A><br/>"
                        Str2 = Str2 & "<A>" & ARRAY2(i) & "</A><br>"
                    End If

                Next
            Else
                strPR_Index = objDB.GetVal("SELECT IFNULL(POD_PR_INDEX, '') AS POD_PR_INDEX FROM PO_DETAILS WHERE POD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND POD_PO_NO = '" & dv("DOC_NO") & "'")
                If strPR_Index <> "" Then
                    strPR_No = objDB.GetVal("SELECT PRM_PR_NO FROM PR_MSTR WHERE PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRM_PR_INDEX = '" & strPR_Index & "'")
                    strPR_Name = objDB.GetVal("SELECT (SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_USER_ID = PR_MSTR.PRM_BUYER_ID AND UM_COY_ID = PR_MSTR.PRM_COY_ID) AS PRM_BUYER_ID FROM PR_MSTR WHERE PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRM_PR_INDEX = '" & strPR_Index & "'")
                    If strPR_No <> "" Then
                        Str = Str & "<A href=""" & dDispatcher.direct("PR", "PRDetail.aspx", "pageid=" & strPageId & "&Frm=transtracking2&index=" & strPR_Index & "&PRNO=" & strPR_No & "&type=mod&mode=bc") & """ ><font color=#0000ff>" & strPR_No & "</font></A><br/>"
                        Str2 = Str2 & "<A>" & strPR_Name & "</A><br>"
                    End If
                Else
                    strPRD_RFQ_Index = objDB.GetVal("SELECT IFNULL(POM_RFQ_INDEX,'') AS POM_RFQ_INDEX FROM po_mstr WHERE pom_po_no = '" & dv("DOC_NO") & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                    If strPRD_RFQ_Index <> "" Then
                        strRFQ_No = objDB.GetVal("SELECT IFNULL(RM_RFQ_NO,'') AS RM_RFQ_NO  FROM rfq_mstr WHERE RM_RFQ_ID = '" & strPRD_RFQ_Index & "' AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                        objPO.GetPRNoAllRFQ(strRFQ_No, ARRAY, COUNT)

                        If ARRAY(0) <> "" Then
                            For i = 0 To COUNT - 1
                                strPR_Index = objDB.GetVal("SELECT PRM_PR_Index FROM PR_MSTR WHERE PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRM_PR_NO = '" & ARRAY(i) & "'")
                                strPR_Name = objDB.GetVal("SELECT (SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_USER_ID = PR_MSTR.PRM_BUYER_ID AND UM_COY_ID = PR_MSTR.PRM_COY_ID) AS PRM_BUYER_ID FROM PR_MSTR WHERE PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRM_PR_INDEX = '" & strPR_Index & "'")
                                If strPR_Index <> "" Then
                                    Str = Str & "<A href=""" & dDispatcher.direct("PR", "PRDetail.aspx", "pageid=" & strPageId & "&Frm=transtracking2&index=" & strPR_Index & "&PRNO=" & ARRAY(i) & "&type=mod&mode=bc") & """ ><font color=#0000ff>" & ARRAY(i) & "</font></A><br/>"
                                    Str2 = Str2 & "<A>" & strPR_Name & "</A><br>"
                                End If
                            Next
                        End If
                    End If
                End If
            End If
            e.Item.Cells(EnumTrans1.icPRNo).Text = Str
            e.Item.Cells(EnumTrans1.icBuyerName).Text = Str2

			'Modified for Agora GST Stage 2 - CH - 2/2/2015
            'Debit Note & Credit Note
            StrDN = ""
            StrCN = ""
            If Request.QueryString("coytype").ToUpper = "VENDOR" Then
                'dv("CDM_INVOICE_NO")
                Dim dsDn, dsCn As DataSet
                'Issue 7480 - CH - 23 Mar 2015 (No.48)
                dsDn = objDB.FillDs("SELECT DNM_DN_NO, DNM_DN_B_COY_ID, DNM_DN_S_COY_ID FROM DEBIT_NOTE_MSTR WHERE DNM_DN_S_COY_ID = '" & Session("CompanyId") & "' AND DNM_INV_NO = '" & Common.parseNull(dv("CDM_INVOICE_NO")) & "' ORDER BY DNM_DN_NO")
                dsCn = objDB.FillDs("SELECT CNM_CN_NO, CNM_CN_B_COY_ID, CNM_CN_S_COY_ID FROM CREDIT_NOTE_MSTR WHERE CNM_CN_S_COY_ID = '" & Session("CompanyId") & "' AND CNM_INV_NO = '" & Common.parseNull(dv("CDM_INVOICE_NO")) & "' ORDER BY CNM_CN_NO")

                If dsDn.Tables(0).Rows.Count > 0 Then
                    For i = 0 To dsDn.Tables(0).Rows.Count - 1
                        If StrDN = "" Then
                            StrDN &= "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewDebitNote.aspx", "pageid=" & strPageId & "&DN_No=" & Common.parseNull(dsDn.Tables(0).Rows(i)("DNM_DN_NO")) & "&SCoyID=" & Common.parseNull(dsDn.Tables(0).Rows(i)("DNM_DN_S_COY_ID")) & "&BCoyID=" & Common.parseNull(dsDn.Tables(0).Rows(i)("DNM_DN_B_COY_ID")) & "')"" ><font color=#0000ff>" & Common.parseNull(dsDn.Tables(0).Rows(i)("DNM_DN_NO"))) & "</font></A>"
                        Else
                            StrDN &= "<BR><A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewDebitNote.aspx", "pageid=" & strPageId & "&DN_No=" & Common.parseNull(dsDn.Tables(0).Rows(i)("DNM_DN_NO")) & "&SCoyID=" & Common.parseNull(dsDn.Tables(0).Rows(i)("DNM_DN_S_COY_ID")) & "&BCoyID=" & Common.parseNull(dsDn.Tables(0).Rows(i)("DNM_DN_B_COY_ID")) & "')"" ><font color=#0000ff>" & Common.parseNull(dsDn.Tables(0).Rows(i)("DNM_DN_NO"))) & "</font></A>"
                        End If
                    Next
                End If
                If dsCn.Tables(0).Rows.Count > 0 Then
                    For i = 0 To dsCn.Tables(0).Rows.Count - 1
                        If StrCN = "" Then
                            StrCN &= "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewCreditNote.aspx", "pageid=" & strPageId & "&CN_No=" & Common.parseNull(dsCn.Tables(0).Rows(i)("CNM_CN_NO")) & "&SCoyID=" & Common.parseNull(dsCn.Tables(0).Rows(i)("CNM_CN_S_COY_ID")) & "&BCoyID=" & Common.parseNull(dsCn.Tables(0).Rows(i)("CNM_CN_B_COY_ID")) & "')"" ><font color=#0000ff>" & Common.parseNull(dsCn.Tables(0).Rows(i)("CNM_CN_NO"))) & "</font></A>"
                        Else
                            StrCN &= "<BR><A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewCreditNote.aspx", "pageid=" & strPageId & "&CN_No=" & Common.parseNull(dsCn.Tables(0).Rows(i)("CNM_CN_NO")) & "&SCoyID=" & Common.parseNull(dsCn.Tables(0).Rows(i)("CNM_CN_S_COY_ID")) & "&BCoyID=" & Common.parseNull(dsCn.Tables(0).Rows(i)("CNM_CN_B_COY_ID")) & "')"" ><font color=#0000ff>" & Common.parseNull(dsCn.Tables(0).Rows(i)("CNM_CN_NO"))) & "</font></A>"
                        End If
                    Next
                End If
            End If
            e.Item.Cells(EnumTrans1.icDNDANumber).Text = StrDN
            e.Item.Cells(EnumTrans1.icCNDANumber).Text = StrCN
			'-----------------------------------------------
        End If
    End Sub
    Private Sub dtgRFQ_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgRFQ.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            'Dim dvMatch As DataView = dv.CreateChildView("match")
            'RFQ/ViewQoute.aspx?qou_num=a11&RFQ_ID=485&vcomid=arielec
            'PO/PRDetail.aspx?caller=OTHER&PageID=16&index=224&PRNo=PR/0040042
            'RFQ/RFQDetail.aspx?page=1&RFQ_Num=RFQ-demo-8&RFQ_ID=485
            Dim strURL As String
            Dim STR3, strPOM_PO_Index As String

            'Dim chk As CheckBox
            'chk = e.Item.Cells(0).FindControl("chkSelection2")
            'chk.Attributes.Add("onclick", "checkChild2('" & chk.ClientID & "')")

            'ai chu add
            Dim hidDocNo2 As HtmlInputHidden
            hidDocNo2 = e.Item.FindControl("hidDocNo2")
            hidDocNo2.Value = dv("RM_RFQ_ID")

            Dim lnkDocNo As HyperLink
            lnkDocNo = e.Item.Cells(EnumRfq.icRfqName).FindControl("lnkRFQNo")
            lnkDocNo.Text = dv("RM_RFQ_NO")
            lnkDocNo.NavigateUrl = dDispatcher.direct("RFQ", "RFQDetail.aspx", "FRM=transtracking&page=" & "1" & "&RFQ_Num=" & dv("RM_RFQ_No") & "&RFQ_ID=" & dv("RM_RFQ_ID"))

            e.Item.Cells(EnumRfq.icExpiryDate).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, e.Item.Cells(EnumRfq.icExpiryDate).Text)
            e.Item.Cells(EnumRfq.icCreatedOn).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, e.Item.Cells(EnumRfq.icCreatedOn).Text)
            'e.Item.Cells(1).Text = "<a href=../RFQ/RFQDetail.aspx?page=1&RFQ_Num=" & dv("RM_RFQ_No") & "&RFQ_ID=" & dv("RM_RFQ_ID") & ">" & dv("RM_RFQ_NO") & "</a>"
            e.Item.Cells(EnumRfq.icActualQuotNum).Text = "<a href=" & dDispatcher.direct("RFQ", "ViewQoute.aspx", "Frm=transtracking&coytype=" & Request.QueryString("coytype") & "&pageid=" & strPageId & "&qou_num=" & dv("RRM_ACTUAL_QUOT_NUM") & "&RFQ_ID=" & dv("RM_RFQ_ID") & "&vcomid=" & dv("RRM_V_Company_ID")) & ">" & dv("RRM_ACTUAL_QUOT_NUM") & "</a>"
            'e.Item.Cells(EnumRfq.icPONo).Text = "<a href='#' onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewPO.aspx", "PO_NO=" & dv("POM_PO_NO") & "&BCoyID=" & dv("RM_Coy_ID") & "&side=" & "other" & "&filetype=" & "2" & "&PageID=" & strPageId) & "')"">" & dv("POM_PO_NO") & "</a>"

            If Request.QueryString("coytype").ToUpper = "BUYER" Then
                strPOM_PO_Index = objDB.GetVal("SELECT IFNULL(POM_PO_INDEX,'') AS POM_PO_NO FROM PO_MSTR WHERE POM_PO_NO = '" & dv("POM_PO_NO") & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")

                STR3 = STR3 & "<A href=""" & dDispatcher.direct("PO", "PODetail.aspx", "Frm=transtrack&pageid=" & strPageId & "&PO_INDEX=" & strPOM_PO_Index & "&PO_NO=" & dv("POM_PO_NO") & "&BCoyID=" & HttpContext.Current.Session("CompanyId") & "&status=&Caller=transtrack&side=b&filetype=2&type=" & "" & "&poview=1&checkid=no") & """ ><font color=#0000ff>" & dv("POM_PO_NO") & "</font></A><br/>"
                e.Item.Cells(EnumRfq.icPONo).Text = STR3
            Else
                e.Item.Cells(EnumRfq.icPONo).Text = "<a href='#' onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewPO.aspx", "PO_NO=" & dv("POM_PO_NO") & "&BCoyID=" & dv("RM_Coy_ID") & "&side=" & "other" & "&filetype=" & "2" & "&PageID=" & strPageId) & "')"">" & dv("POM_PO_NO") & "</a>"
            End If

            Dim COUNT, i As Integer
            Dim ARRAY(100), ARRAY2(100), Str, Str2 As String
            Dim strPR_Index As String

            If Common.parseNull(CStr(dv("POM_RFQ_INDEX"))) <> "" Then
                'If Not IsDBNull(dv("POM_RFQ_INDEX")) Then
                objTrac.getRelatedPRAndOld(dv("POM_RFQ_INDEX"), ARRAY, ARRAY2, COUNT)   'Get PO Number
                If ARRAY(0) <> "" Then
                    For i = 0 To COUNT - 1
                        strPR_Index = objDB.GetVal("SELECT PRM_PR_INDEX FROM PR_MSTR WHERE PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRM_PR_NO = '" & ARRAY(i) & "'")
                        If strPR_Index <> "" Then
                            Str = Str & "<A href=""" & dDispatcher.direct("PR", "PRDetail.aspx", "pageid=" & strPageId & "&Frm=transtracking2&index=" & strPR_Index & "&PRNO=" & ARRAY(i) & "&type=mod&mode=bc") & """ ><font color=#0000ff>" & ARRAY(i) & "</font></A><br/>"
                            Str2 = Str2 & "<A>" & ARRAY2(i) & "</A><br>"
                        End If

                    Next
                End If
            End If

            e.Item.Cells(EnumRfq.icPRNo).Text = Str
            e.Item.Cells(EnumRfq.icBuyerName).Text = Str2

            'If Not IsDBNull(dv("PRM_PR_TYPE")) Then
            '    If dv("PRM_PR_TYPE") = "CC" Then    'Contract Catalogue PR
            '        e.Item.Cells(EnumPR.icPRType).Text = "Contract"
            '        objPR2.GetPONoCC(dv("PRM_PR_No"), Array, COUNT)   'Get PO Number
            '        If Array(0) <> "" Then
            '            For i = 0 To COUNT - 1
            '                'lnkPONo.Text = ARRAY(i)
            '                'STR = STR & "<A>" & ARRAY(i) & "</A><br>"
            '                'STR = STR & "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("PO", "PODetail.aspx", "Frm=POVIEWB2&pageid=" & strPageId & "&PO_INDEX=" & dv("POM_PO_INDEX") & "&PO_NO=" & dv("POM_PO_NO") & "&BCoyID=" & dv("POM_B_COY_ID") & "&status=" & dv("POM_PO_STATUS") & "&Caller=POviewB2&side=b&filetype=2&type=" & Request(Trim("Type")) & "&poview=1") & "')"" ><font color=#0000ff>" & ARRAY(i) & "</font></A><br/>"
            '                'STR = STR & "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("PO", "PODetail.aspx", "Frm=POVIEWB2&pageid=" & strPageId & "&PO_INDEX=" & "" & "&PO_NO=" & ARRAY(i) & "&BCoyID=" & "" & "&status=" & "" & "&Caller=POviewB2&side=b&filetype=2&type=" & "" & "&poview=1") & "')"" ><font color=#0000ff>" & ARRAY(i) & "</font></A><br/>"
            '                'lnkPONo.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx", "Frm=POVIEWB2&pageid=" & strPageId & "&PO_INDEX=" & "" & "&PO_NO=" & ARRAY(i) & "&BCoyID=" & HttpContext.Current.Session("CompanyId") & "&status=5&Caller=PRAll&side=b&filetype=2&type=" & "" & "&poview=1")
            '                Str = Str() & "<A href=""" & dDispatcher.direct("PO", "PODetail.aspx", "Frm=POVIEWB2&pageid=" & strPageId & "&PO_INDEX=" & "" & "&PO_NO=" & Array(i) & "&BCoyID=" & HttpContext.Current.Session("CompanyId") & "&status=5&Caller=PRAll&side=b&filetype=2&type=" & "" & "&poview=1") & """ ><font color=#0000ff>" & Array(i) & "</font></A><br/>"
            '            Next
            '        End If
            '        'If STR = "" Then
            '        '    STR = STR & "<A>" & dv("PO_NO") & "</A><br>"
            '        'End If
            '        If Str() = "" And dv("PRM_PR_STATUS") <> 4 And dv("PRM_PR_STATUS") <> 99 Then
            '            Str = Str() & "<A href=""" & dDispatcher.direct("PO", "PODetail.aspx", "Frm=POVIEWB2&pageid=" & strPageId & "&PO_INDEX=" & "" & "&PO_NO=" & dv("PO_NO") & "&BCoyID=" & HttpContext.Current.Session("CompanyId") & "&status=5&Caller=PRAll&side=b&filetype=2&type=" & "" & "&poview=1") & """ ><font color=#0000ff>" & dv("PO_NO") & "</font></A><br/>"
            '        End If
            '        e.Item.Cells(EnumPR.icPONO).Text = Str()
            '    Else
            '        e.Item.Cells(EnumPR.icPRType).Text = "Non-Contract"
            '        'e.Item.Cells(EnumPR.icPONO).Text = ""

            '        objPR2.GetPONoNonCC(dv("PRM_PR_No"), Array, COUNT)   'Get PO Number
            '        If Array(0) <> "" Then
            '            For i = 0 To COUNT - 1
            '                strPOM_PO_No = objDB.GetVal("SELECT IFNULL(POM_PO_NO,'') AS POM_PO_NO FROM PO_MSTR WHERE POM_PO_NO = '" & Array(i) & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
            '                If strPOM_PO_No <> "" Then
            '                    'lnkPONo.Text = ARRAY(i)
            '                    ' STR = STR & "<A>" & ARRAY(i) & "</A><br>"
            '                    'STR = STR & "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("PO", "PODetail.aspx", "Frm=POVIEWB2&pageid=" & strPageId & "&PO_INDEX=" & dv("POM_PO_INDEX") & "&PO_NO=" & dv("POM_PO_NO") & "&BCoyID=" & dv("POM_B_COY_ID") & "&status=" & dv("POM_PO_STATUS") & "&Caller=POviewB2&side=b&filetype=2&type=" & Request(Trim("Type")) & "&poview=1") & "')"" ><font color=#0000ff>" & ARRAY(i) & "</font></A><br/>"
            '                    'STR = STR & "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("PO", "PODetail.aspx", "Frm=POVIEWB2&pageid=" & strPageId & "&PO_INDEX=" & "" & "&PO_NO=" & ARRAY(i) & "&BCoyID=" & "" & "&status=" & "" & "&Caller=POviewB2&side=b&filetype=2&type=" & "" & "&poview=1") & "')"" ><font color=#0000ff>" & ARRAY(i) & "</font></A><br/>"
            '                    'lnkPONo.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx", "Frm=POVIEWB2&pageid=" & strPageId & "&PO_INDEX=" & "" & "&PO_NO=" & ARRAY(i) & "&BCoyID=" & HttpContext.Current.Session("CompanyId") & "&status=5&Caller=PRAll&side=b&filetype=2&type=" & "" & "&poview=1")
            '                    Str = Str() & "<A href=""" & dDispatcher.direct("PO", "PODetail.aspx", "Frm=POVIEWB2&pageid=" & strPageId & "&PO_INDEX=" & "" & "&PO_NO=" & Array(i) & "&BCoyID=" & HttpContext.Current.Session("CompanyId") & "&status=5&Caller=PRAll&side=b&filetype=2&type=" & "" & "&poview=1") & """ ><font color=#0000ff>" & Array(i) & "</font></A><br/>"
            '                Else
            '                    strRM_RFQ_Id = objDB.GetVal("SELECT DISTINCT IFNULL(RM_RFQ_ID,'') AS RM_RFQ_NO  FROM rfq_mstr WHERE RM_RFQ_NO = '" & Array(i) & "' AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
            '                    strPOM_PO_No = objDB.GetVal("SELECT IFNULL(POM_PO_NO,'') AS POM_PO_NO FROM PO_MSTR WHERE POM_RFQ_INDEX = '" & strRM_RFQ_Id & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
            '                    'lnkPONo.Text = strPOM_PO_No
            '                    'lnkPONo.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx", "Frm=POVIEWB2&pageid=" & strPageId & "&PO_INDEX=" & "" & "&PO_NO=" & strPOM_PO_No & "&BCoyID=" & HttpContext.Current.Session("CompanyId") & "&status=5&Caller=PRAll&side=b&filetype=2&type=" & "" & "&poview=1")
            '                    Str = Str() & "<A href=""" & dDispatcher.direct("PO", "PODetail.aspx", "Frm=POVIEWB2&pageid=" & strPageId & "&PO_INDEX=" & "" & "&PO_NO=" & strPOM_PO_No & "&BCoyID=" & HttpContext.Current.Session("CompanyId") & "&status=5&Caller=PRAll&side=b&filetype=2&type=" & "" & "&poview=1") & """ ><font color=#0000ff>" & strPOM_PO_No & "</font></A><br/>"
            '                End If
            '            Next
            '        End If
            '        'If STR = "" Then
            '        '    STR = STR & "<A>" & dv("PO_NO") & "</A><br>"
            '        'End If
            '        If Str() = "" And dv("PRM_PR_STATUS") <> 4 And dv("PRM_PR_STATUS") <> 99 Then
            '            Str = Str() & "<A href=""" & dDispatcher.direct("PO", "PODetail.aspx", "Frm=POVIEWB2&pageid=" & strPageId & "&PO_INDEX=" & "" & "&PO_NO=" & dv("PO_NO") & "&BCoyID=" & HttpContext.Current.Session("CompanyId") & "&status=5&Caller=PRAll&side=b&filetype=2&type=" & "" & "&poview=1") & """ ><font color=#0000ff>" & dv("PO_NO") & "</font></A><br/>"
            '        End If
            '        e.Item.Cells(EnumPR.icPONO).Text = Str()

            '        'If strPOM_PO_No = "" Then
            '        '    e.Item.Cells(EnumPR.icStatus).Text = "Sourcing"
            '        'End If
            '    End If
            'Else
            '    e.Item.Cells(EnumPR.icPONO).Text = ""
            '    e.Item.Cells(EnumPR.icPRType).Text = "Non-Contract"
            'End If

        End If
    End Sub

    Private Sub cboViewBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboViewBy.SelectedIndexChanged
        'cmdSearch_Click(sender, e)
        'If cboViewBy.SelectedValue = "PR" Or cboViewBy.SelectedValue = "NPR" Then
        '    lblName.Text = "Buyer Name :"
        'Else
        '    lblName.Text = "Purchaser Name :"
        'End If
    End Sub
End Class
