Imports AgoraLegacy
Imports eProcure.Component

Public Class POVendorList
    Inherits AgoraLegacy.AppBaseClass

    Dim dDispatcher As New AgoraLegacy.dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents txt_po_no As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_start_date As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_end_date As System.Web.UI.WebControls.TextBox
    Protected WithEvents CompareValidator1 As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents chk_New As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chk_Cancel As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chk_close As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chk_Outstdg As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents dtg_POList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Table2 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSearchResult As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents txt_buyer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_buyer_com As System.Web.UI.WebControls.TextBox


    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Public Enum EnumPOList
        icPONo = 0
        icpoDate = 1
        icCoyName = 2
        icBuyer = 3
        icStatus = 4
        icFulfilment = 5
    End Enum

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CheckButtonAccess(True)
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page her
        MyBase.Page_Load(sender, e)
        blnPaging = True
        SetGridProperty(dtg_POList)

        If Not IsPostBack Then
            GenerateTab()
            'chk_New.Checked = True
            'cmdSearch_Click(sender, e)
        End If
    End Sub

    Public Sub dtg_POList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtg_POList.CurrentPageIndex = e.NewPageIndex
        Bindgrid(True)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtg_POList.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objPO As New PurchaseOrder

        '//Retrieve Data from Database
        Dim ds As DataSet
        Dim strStatus As String = ""
        Dim strFulfilment As String = ""
        Dim count_status As Integer
        '-- start cons strstatus -

        'Modified by Joon on 20 Oct 2010
        'Simplify the status: 
        'New = New, Open
        'Cancelled = Rejected, Cancelled, Cancel Order, Pending Cancellation Acknowledgement
        'Closed = Closed
        'Outstanding = Accepted, Open (fulfilment), Partial Delivered, Fully Delivered

        ''-----------------------------
        'If Me.chk_New.Checked Then
        '    strStatus = IIf(strStatus = "", POStatus_new.NewPO & "," & POStatus_new.Open, strStatus & "," & POStatus_new.NewPO & "," & POStatus_new.Open)
        'End If

        'If Me.chk_Cancel.Checked Then
        '    strStatus = IIf(strStatus = "", POStatus_new.Cancelled & "," & POStatus_new.Rejected & "," & Fulfilment.Cancel_Order & "," & Fulfilment.Pending_Cancel_Ack, strStatus & "," & POStatus_new.Cancelled & "," & POStatus_new.Rejected & "," & Fulfilment.Cancel_Order & "," & Fulfilment.Pending_Cancel_Ack)
        'End If

        'If Me.chk_close.Checked Then
        '    strStatus = IIf(strStatus = "", POStatus_new.Close, strStatus & "," & POStatus_new.Close)
        'End If

        'If Me.chk_Outstdg.Checked Then
        '    strStatus = IIf(strStatus = "", POStatus_new.Accepted & "," & Fulfilment.Open & "," & Fulfilment.Part_Delivered & "," & Fulfilment.Fully_Delivered, strStatus & "," & POStatus_new.Accepted & "," & Fulfilment.Open & "," & Fulfilment.Part_Delivered & "," & Fulfilment.Fully_Delivered)
        'End If
        ''-----------------------------
        If Me.chk_New.Checked Then
            strStatus = IIf(strStatus = "", POStatus_new.NewPO & "," & POStatus_new.Open, strStatus & "," & POStatus_new.NewPO & "," & POStatus_new.Open)
            strFulfilment = "0"
        End If

        If Me.chk_Cancel.Checked Then
            'strStatus = IIf(strStatus = "", POStatus_new.Cancelled & "," & POStatus_new.Rejected, strStatus & "," & POStatus_new.Cancelled & "," & POStatus_new.Rejected)
            strStatus = IIf(strStatus = "", POStatus_new.Rejected & "," & POStatus_new.Cancelled, strStatus & "," & POStatus_new.Rejected & "," & POStatus_new.Cancelled)
            If strFulfilment <> "" Then
                strFulfilment &= ",4,5,0"
            Else
                strFulfilment = "4,5,0"
            End If
        End If

        If Me.chk_close.Checked Then
            strStatus = IIf(strStatus = "", POStatus_new.Close & "," & POStatus_new.Accepted, strStatus & "," & POStatus_new.Close & "," & POStatus_new.Accepted)
            If strFulfilment <> "" Then
                strFulfilment &= ",3"
            Else
                strFulfilment = "3"
            End If
        End If

        If Me.chk_Outstdg.Checked Then
            strStatus = IIf(strStatus = "", POStatus_new.Accepted, strStatus & "," & POStatus_new.Accepted)
            If strFulfilment <> "" Then
                strFulfilment &= ",1,2"
            Else
                strFulfilment = "1,2"
            End If
        End If

        If (strFulfilment <> "" And count_status = 5) Or strFulfilment = "" Then
            strFulfilment = " " & Fulfilment.Cancel_Order & "," & Fulfilment.Fully_Delivered & "," & Fulfilment.Open & "," & Fulfilment.Part_Delivered & "," & Fulfilment.null & "," & Fulfilment.Pending_Cancel_Ack
        End If

        If Me.chk_New.Checked And Me.chk_Cancel.Checked And Me.chk_close.Checked And Me.chk_Outstdg.Checked Then
            strFulfilment = " " & Fulfilment.Cancel_Order & "," & Fulfilment.Fully_Delivered & "," & Fulfilment.Open & "," & Fulfilment.Part_Delivered & "," & Fulfilment.null & "," & Fulfilment.Pending_Cancel_Ack
        End If
        If strStatus = "" Then
            strStatus = " " & POStatus_new.NewPO & "," & POStatus_new.Open & "," & POStatus_new.Accepted & "," & POStatus_new.Rejected & ", " & POStatus_new.Cancelled & ", " & POStatus_new.Close
        End If
        ' -- con end 
        'ds = objPO.VIEW_POList(strStatus, strFulfilment, "v", "", Me.txt_po_no.Text, Me.txt_start_date.Text, Me.txt_end_date.Text, Me.txt_buyer_com.Text)
        ds = objPO.VIEW_POList_NoPR(strStatus, strFulfilment, "v", "", Me.txt_po_no.Text, Me.txt_start_date.Text, Me.txt_end_date.Text, Me.txt_buyer_com.Text)

        '//for sorting asc or desc
        Dim dvViewPR As DataView
        dvViewPR = ds.Tables(0).DefaultView
        dvViewPR.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewPR.Sort += " DESC"

        intPageRecordCnt = ds.Tables(0).Rows.Count

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtg_POList, dvViewPR)
            dtg_POList.DataSource = dvViewPR
            dtg_POList.DataBind()
            'ChangeStatus()
        Else
            dtg_POList.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If


        ' add for above checking
        ViewState("PageCount") = dtg_POList.PageCount
    End Function

    Private Sub ChangeStatus()
        Dim i As Integer = 0

        For i = 0 To dtg_POList.Items(4).ItemIndex.MaxValue
            Select Case dtg_POList.Items(4).Cells(i).Text
                Case "New", "Open"
                    dtg_POList.Items(4).Cells(i).Text = "New"

                Case "Rejected", "Cancelled"
                    dtg_POList.Items(4).Cells(i).Text = "Cancelled"

                Case "Closed"
                    dtg_POList.Items(4).Cells(i).Text = "Closed"

                Case "Accepted"
                    dtg_POList.Items(4).Cells(i).Text = "Outstanding"
            End Select
        Next
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtg_POList.CurrentPageIndex = 0
        ViewState("SortAscending") = "no"
        ViewState("SortExpression") = "POM_PO_DATE"
        Dim olbl As TextBox = Me.Form.Parent.FindControl("txt_end_date")
        Bindgrid()
    End Sub

    Private Sub dtg_POList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_POList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim objpo As New PurchaseOrder
            Dim objpo1 As New PurchaseOrder_Vendor
            e.Item.Cells(EnumPOList.icpoDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, Common.parseNull(dv("POM_PO_DATE")))

            If dv("POM_PO_STATUS") = "6" Or (dv("POM_PO_STATUS") = "3" And dv("POM_FULFILMENT") = "3") Then
                e.Item.Cells(EnumPOList.icStatus).Text = "Closed"
            ElseIf dv("POM_PO_STATUS") = "4" Or dv("POM_PO_STATUS") = "5" Then
                e.Item.Cells(EnumPOList.icStatus).Text = "Cancelled"
            ElseIf dv("POM_PO_STATUS") = POStatus_new.NewPO Or dv("POM_PO_STATUS") = POStatus_new.Open Then
                e.Item.Cells(EnumPOList.icStatus).Text = "New"
            Else
                e.Item.Cells(EnumPOList.icStatus).Text = "Outstanding"
            End If
            Dim lbl_po_no As Label

            If Common.parseNull(dv("POM_URGENT")) = "1" Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(EnumPOList.icPONo).Controls.Add(imgAttach)
            End If

            lbl_po_no = e.Item.FindControl("lbl_po_no")
            lbl_po_no.Text = "<A href=""" & dDispatcher.direct("PO", "PODetail.aspx", "Frm=POVendorList&pageid=" & strPageId & "&PO_INDEX=" & dv("POM_PO_INDEX") & "&PO_NO=" & Server.UrlEncode(Common.parseNull(dv("POM_PO_NO"))) & "&BCoyID=" & dv("POM_B_COY_ID") & "&status=" & Common.parseNull(dv("POM_PO_STATUS")) & "&side=v&filetype=2 """) & " ><font color=#0000ff>" & Common.parseNull(dv("POM_PO_NO")) & "</font></A>"
            If objpo.HasAttachment(dv("POM_PO_NO"), objpo1.get_comid(dv("POM_PO_INDEX"))) Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/images", "clip_icon.gif")
                e.Item.Cells(EnumPOList.icPONo).Controls.Add(imgAttach)
            End If

            Session("quoteurl") = strCallFrom
            Dim ds As DataSet
            If objpo.isConvertedFromRFQ(dv("POM_PO_INDEX"), ds) Then
                If ds.Tables(0).Rows.Count > 0 Then
                    Dim lnkRFQ As New HyperLink
                    lnkRFQ.ImageUrl = dDispatcher.direct("Plugins/images", "Q-3 Icon (10x10).jpg")
                    If ds.Tables(0).Rows.Count = 1 Then
                        lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "viewQoute.aspx", "Frm=POVendorList&pageid=" & strPageId & "&RFQ_ID=" & ds.Tables(0).Rows(0)("POM_RFQ_INDEX") & "&RFQ_Num=" & ds.Tables(0).Rows(0)("RM_RFQ_No") & "&vcomid=" & ds.Tables(0).Rows(0)("POM_S_COY_ID") & "&side=quote")
                    Else
                        lnkRFQ.NavigateUrl = dDispatcher.direct("PR", "PRList.aspx", "type=QUO&PageId=" & strPageId & "&DocNo=" & dv("POM_PO_No") & "&index=" & dv("POM_PO_INDEX"))
                    End If

                    e.Item.Cells(EnumPOList.icPONo).Controls.Add(lnkRFQ)
                End If
            End If

            'If objpo.isConvertedFromRFQ(dv("POM_PO_INDEX"), ds) Then
            '    Dim imgAttach As New System.Web.UI.WebControls.Image
            '    imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
            '    imgAttach.ImageUrl = "../Images/i_quotation2.gif"
            '    e.Item.Cells(EnumPOList.icPONo).Controls.Add(imgAttach)
            'End If
        End If
    End Sub

    Private Sub dtg_POList_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_POList.ItemCreated
        Grid_ItemCreated(dtg_POList, e)
    End Sub
    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        'Session("w_VendorPOList_tabs") = "<div class=""t_entity""><ul>" & _
        '    "<li><div class=""space""></div></li>" & _
        '             "<li><a class=""t_entity_btn"" href=""POViewB.aspx?pageid=" & strPageId & """><span>Purchase Order</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '             "<li><a class=""t_entity_btn_selected"" href=""POVendorList.aspx?pageid=" & strPageId & """><span>PO Listing</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '             "</ul><div></div></div>"
        Session("w_VendorPOList_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PO", "POViewB.aspx", "pageid=" & strPageId) & """><span>Purchase Order</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PO", "POVendorList.aspx", "pageid=" & strPageId) & """><span>PO Listing</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "</ul><div></div></div>"
    End Sub
End Class
