Imports AgoraLegacy
Imports eProcure.Component


Public Class POViewB
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDB As New EAD.DBCom

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
    Protected WithEvents dtgPO As System.Web.UI.WebControls.DataGrid

    Public Enum EnumPOList
        icPONo = 0
        icPOIdx = 1
        icPODate = 2
        icBCoy = 3
        icBuyer = 4
        icStatus = 5
        icCRNo = 6
    End Enum

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        blnPaging = True
        If Not IsPostBack Then
            GenerateTab()
            SetGridProperty(dtgPO)
            dtgPO.CurrentPageIndex = 0
            ViewState("SortAscending") = "no"
            ViewState("SortExpression") = "POM_PO_DATE"
        End If
        BindPO(True)
    End Sub

    Private Sub BindPO(Optional ByVal pSorted As Boolean = False)

        Dim objPO As New PurchaseOrder_Vendor 'PurchaseOrder
        Dim ds As DataSet
        Dim dvViewPO As DataView

        ds = objPO.getPOForAck()
        dvViewPO = ds.Tables(0).DefaultView
        dvViewPO.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewPO.Sort += " DESC"

        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgPO, dvViewPO)
            dtgPO.DataSource = dvViewPO
            dtgPO.DataBind()
        Else
            dtgPO.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If
        ViewState("PageCount") = dtgPO.PageCount
    End Sub
    Private Sub dtgPO_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPO.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgPO, e)
    End Sub
    Private Sub dtgPO_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPO.ItemDataBound
        Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
        Dim objpo As New PurchaseOrder
        Dim objpo1 As New PurchaseOrder_Vendor
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

            e.Item.Cells(EnumPOList.icPODate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, Common.parseNull(dv("POM_PO_DATE")))

            If dv("POM_PO_STATUS") = POStatus_new.NewPO Or dv("POM_PO_STATUS") = POStatus_new.Open Then
                e.Item.Cells(EnumPOList.icStatus).Text = "New"
            Else
                e.Item.Cells(EnumPOList.icStatus).Text = "Cancelled"
            End If

            Dim lbl_po_no As HyperLink
            lbl_po_no = e.Item.Cells(EnumPOList.icPONo).FindControl("lbl_po_no")
            If dv("POM_PO_STATUS") = "5" Or (dv("POM_PO_STATUS") = "3" And dv("POM_FULFILMENT") = "4") Then
                lbl_po_no.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx", "pageid=" & strPageId & "&PO_INDEX=" & dv("POM_PO_INDEX") & "&PO_NO=" & dv("POM_PO_NO") & "&BCoyID=" & dv("POM_B_COY_ID") & "&status=" & Common.parseNull(dv("POM_PO_STATUS")) & "&side=v&filetype=1&caller=POViewB&cr_no=" & Common.parseNull(dv("CR_NO")))
            Else
                lbl_po_no.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx", "pageid=" & strPageId & "&PO_INDEX=" & dv("POM_PO_INDEX") & "&PO_NO=" & dv("POM_PO_NO") & "&BCoyID=" & dv("POM_B_COY_ID") & "&status=" & Common.parseNull(dv("POM_PO_STATUS")) & "&side=v&filetype=2&caller=POViewB&cr_no=" & Common.parseNull(dv("CR_NO")))
            End If
            lbl_po_no.Text = dv("POM_PO_No")

            If Common.parseNull(dv("POM_URGENT")) = "1" Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(EnumPOList.icPONo).Controls.Add(imgAttach)
            End If

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
                    lnkRFQ.ImageUrl = dDispatcher.direct("Plugins/Images", "Q-3 Icon (10x10).jpg")
                    If ds.Tables(0).Rows.Count = 1 Then
                        lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "viewQoute.aspx", "Frm=POViewB&pageid=" & strPageId & "&RFQ_ID=" & ds.Tables(0).Rows(0)("POM_RFQ_INDEX") & "&RFQ_Num=" & ds.Tables(0).Rows(0)("RM_RFQ_No") & "&vcomid=" & ds.Tables(0).Rows(0)("POM_S_COY_ID") & "&side=quote")
                    End If
                    e.Item.Cells(EnumPOList.icPONo).Controls.Add(lnkRFQ)
                End If
            End If
        End If
    End Sub
    Sub SortCommand_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles dtgPO.SortCommand
        Grid_SortCommand(sender, e)
        dtgPO.CurrentPageIndex = 0
        BindPO(True)
    End Sub
    Private Sub dtgPO_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgPO.PageIndexChanged
        dtgPO.CurrentPageIndex = e.NewPageIndex
        BindPO(True)
    End Sub
    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        'Session("w_VendorPOAck_tabs") = "<div class=""t_entity""><ul>" & _
        '    "<li><div class=""space""></div></li>" & _
        '             "<li><a class=""t_entity_btn_selected"" href=""POViewB.aspx?pageid=" & strPageId & """><span>Purchase Order</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '             "<li><a class=""t_entity_btn"" href=""POVendorList.aspx?pageid=" & strPageId & """><span>PO Listing</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '             "</ul><div></div></div>"
        'Session("w_VendorPOAck_tabs") = "<div class=""t_entity""><ul>" & _
        '"<li><div class=""space""></div></li>" & _
        '             "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PO", "POViewB.aspx", "pageid=" & strPageId) & """><span>Purchase Order</span></a></li>" & _
        '             "<li><div class=""space""></div></li>" & _
        '             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PO", "POVendorList.aspx", "pageid=" & strPageId) & """><span>PO Listing</span></a></li>" & _
        '             "<li><div class=""space""></div></li>" & _
        '             "</ul><div></div></div>"
        Dim _role As New UserRoles
        Dim showFFPO = objDB.GetVal("SELECT CM_FFPO_CONTROL FROM company_mstr WHERE cm_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'")
        Dim _compType = objDB.GetVal("SELECT CM_COY_TYPE FROM company_mstr WHERE cm_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'")

        Dim _userRole = _role.get_UserFixedRole
        If showFFPO.ToString.ToUpper = "Y" And (_userRole.ToString.ToUpper.Contains("PURCHASING MANAGER") Or _userRole.ToString.ToUpper.Contains("PURCHASING OFFICER")) And _compType.ToString.ToUpper = "BUYER" Then
            Session("w_VendorPOAck_tabs") = "<div class=""t_entity""><ul>" & _
                      "<li><div class=""space""></div></li>" & _
                                  "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PO", "POViewB.aspx", "pageid=" & strPageId) & """><span>Purchase Order</span></a></li>" & _
                                  "<li><div class=""space""></div></li>" & _
                                  "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PO", "POVendorList.aspx", "type=Listing&pageid=" & strPageId) & """><span>PO Listing</span></a></li>" & _
                                  "<li><div class=""space""></div></li>" & _
                                  "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PO", "RAISEFFPO.aspx", "type=Listing&pageid=" & strPageId) & """><span>Free Form Purchase Order</span></a></li>" & _
                                  "<li><div class=""space""></div></li>" & _
                                  "</ul><div></div></div>"
        Else
            Session("w_VendorPOAck_tabs") = "<div class=""t_entity""><ul>" & _
            "<li><div class=""space""></div></li>" & _
                        "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PO", "POViewB.aspx", "pageid=" & strPageId) & """><span>Purchase Order</span></a></li>" & _
                        "<li><div class=""space""></div></li>" & _
                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PO", "POVendorList.aspx", "type=Listing&pageid=" & strPageId) & """><span>PO Listing</span></a></li>" & _
                        "<li><div class=""space""></div></li>" & _
                        "</ul><div></div></div>"
        End If
    End Sub

End Class
