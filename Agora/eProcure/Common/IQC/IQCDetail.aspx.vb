'//Outstanding
'//Calc By - Product /SubTotal
'// GST - in %
'//default - Subtotal , 0%
'//To find if the b_product_name is completely null for a PO (from D_PR),approveao.asp line 206
'//

Imports AgoraLegacy
Imports eProcure.Component
Imports System.Drawing

Public Class IQCDetail
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDb As New EAD.DBCom
    Dim objDO As New DeliveryOrder
    Dim dt As New DataTable
    Dim intCnt As Integer
    Dim strC() As String
    Dim dsAllInfo, ds As DataSet
    Dim blnCustomField As Boolean = False
    Dim intPRStatus As String
    Dim dblNoTaxTotal, dblTaxTotal, dblTotalGst As Double
    Dim intGSTcnt, intNoGSTcnt, intTotItem As Integer
    Dim strRFQIndex As String
    Dim CrDate As Date
    'Protected WithEvents trAdmin As System.Web.UI.HtmlControls.HtmlTableRow

    Protected WithEvents test As System.Web.UI.WebControls.Panel
    Dim strCaller As String
    Dim dtBCM As DataTable
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
        CheckButtonAccess(True)
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'If Not Page.IsPostBack Then
        MyBase.Page_Load(sender, e)

        Response.Expires = -1
        Response.AddHeader("cache-control", "private")
        Response.AddHeader("pragma", "no-cache")
        Response.CacheControl = "no-cache"

        blnPaging = False
        blnSorting = False
        SetGridProperty(dtgAppFlow)
        SetGridProperty(dtgAppFlowTracking)

        If Not Page.IsPostBack Then
            GenerateTab()
            Dim objInv As New Inventory
            Dim strOfficerType As String
            dsAllInfo = objInv.getIQCInfo(Request.QueryString("IQCNo"))
            objInv = Nothing

            renderIQCHeader()
            renderIQCApprFlow()
            renderIQCApprFlowTracking()

            strOfficerType = objDb.GetVal("SELECT IQCA_OFFICER_TYPE FROM IQC_APPROVAL WHERE IQCA_IQC_INDEX = " & ViewState("IVL_VERIFY_LOT_INDEX") & " AND IQCA_SEQ - 1 = IQCA_AO_ACTION")
            hidType.Value = strOfficerType

            If Session("urlreferer") = "Dashboard" Then
                lnkBack.NavigateUrl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId)
            ElseIf Session("urlreferer") = "SearchIQCAO" Then
                lnkBack.NavigateUrl = dDispatcher.direct("IQC", "SearchIQC_AO.aspx", "pageId=" & strPageId)
            ElseIf Session("urlreferer") = "SearchIQCAll" Then
                lnkBack.NavigateUrl = dDispatcher.direct("IQC", "SearchIQC_All.aspx", "pageId=" & strPageId)
            End If

            Image1.ImageUrl = dDispatcher.direct("Plugins/images", "collapse_up.gif")
            Me.cmd_preview.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewIQC.aspx", "IQCNo=" & Request.QueryString("IQCNo") & "&IQCIndex=" & Request.QueryString("index") & "&CoyID=" & Session("CompanyID")) & "')")
        End If

    End Sub

    Public Function CreateFileLinks(ByVal userID As Object, ByVal altUserID As Object, ByVal seq As String, ByVal tb_iqc As String, Optional ByVal index As String = "") As DataTable
        Dim id1 As String = ""
        '1. if the pass in user is is not same as current login user id, return nothing
        If Not IsDBNull(userID) Then
            id1 = CStr(userID)
        End If

        Dim id2 As String = ""
        If Not IsDBNull(altUserID) Then
            id2 = CStr(altUserID)
        End If

        Dim pr As PR = New PR
        '2. get data about the attachment
        Dim ds As DataSet
        If tb_iqc = "IQCL" Then
            ds = pr.getUserAttach("AO", tb_iqc, index, id1, id2, seq)
        Else
            ds = pr.getUserAttach("AO", tb_iqc, CStr(ViewState("IVL_VERIFY_LOT_INDEX")), id1, id2, seq)
        End If


        '3. get the first table in the returned data set
        Dim dt As DataTable = ds.Tables(0)

        '4. create a datatable, and add a column into the table
        Dim table As DataTable = New DataTable
        Dim urlCol As DataColumn = New DataColumn("Hyperlink")
        table.Columns.Add(urlCol)

        '5. loop each rows of the dataset
        Dim fileMgr As New FileManagement
        Dim count As Integer = 1
        For Each row As DataRow In dt.Rows

            '6. generate the url that download the file
            Dim strFile As String = row.Item("UA_ATTACH_FILENAME")
            Dim strFile1 As String = row.Item("UA_HUB_FILENAME")
            Dim url As String = fileMgr.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.UserAttachment, "", EnumUploadFrom.FrontOff)

            '7. create a row from the newly created table, and add the hyperlink string inside
            Dim r As DataRow = table.NewRow
            r.Item("Hyperlink") = CStr(count) + ") " + url
            table.Rows.Add(r)
            count = count + 1
        Next

        Return table
    End Function

    Private Sub GenerateAttachmentColumn()
        dtgAppFlow.Columns(dtgAppFlow.Columns.Count - 1).Visible = True
    End Sub

    Private Sub renderIQCHeader()
        Dim dtHeader As New DataTable
        Dim objInv2 As New Inventory
        Dim ds As New DataSet
        Dim intProIndex As Integer
        Dim strItemCode, strVCoyID, strSuppCode, strPONo As String
        'Dim strBillAddr As String

        dtHeader = dsAllInfo.Tables(0)
        If dtHeader.Rows.Count > 0 Then

            ViewState("userSKId") = Common.parseNull(dtHeader.Rows(0)("GM_CREATED_BY"))
            ViewState("IVL_IQC_NO") = Common.parseNull(dtHeader.Rows(0)("IVL_IQC_NO"))
            lblIQCNo.Text = ViewState("IVL_IQC_NO") 'IQC Number

            'Status
            If IsDBNull(dtHeader.Rows(0)("IVL_STATUS")) Then
                lblStatus.Text = "Outstanding"
            ElseIf dtHeader.Rows(0)("IVL_STATUS") = "1" Then
                lblStatus.Text = "Closed (Approved)"
            ElseIf dtHeader.Rows(0)("IVL_STATUS") = "2" Then
                lblStatus.Text = "Closed (Waived)"
            ElseIf dtHeader.Rows(0)("IVL_STATUS") = "3" Then
                lblStatus.Text = "Closed (Replacement)"
            ElseIf dtHeader.Rows(0)("IVL_STATUS") = "4" Then
                lblStatus.Text = "Rejected"
            End If

            lblPONo.Text = Common.parseNull(dtHeader.Rows(0)("POM_PO_NO")) 'PO Number
            strPONo = Common.parseNull(dtHeader.Rows(0)("POM_PO_NO"))
            lblItemCode.Text = Common.parseNull(dtHeader.Rows(0)("IM_ITEM_CODE")) 'Item Code
            strItemCode = Common.parseNull(dtHeader.Rows(0)("IM_ITEM_CODE"))
            strVCoyID = Common.parseNull(dtHeader.Rows(0)("DOL_COY_ID")) 'Vendor ID
            strSuppCode = Common.parseNull(dtHeader.Rows(0)("POM_VENDOR_CODE")) 'Supplier Code
            intProIndex = objDb.GetVal("SELECT PM_PRODUCT_INDEX FROM PRODUCT_MSTR WHERE PM_VENDOR_ITEM_CODE='" & strItemCode & "' AND PM_S_COY_ID='" & Session("CompanyId") & "'")

            'PO Date
            If IsDBNull(dtHeader.Rows(0)("POM_PO_DATE")) Then
                lblPODate.Text = ""
            Else
                lblPODate.Text = Format(CDate(dtHeader.Rows(0)("POM_PO_DATE")), "dd/MM/yyyy")
            End If

            lblItemName.Text = Common.parseNull(dtHeader.Rows(0)("IM_INVENTORY_NAME")) 'Item Name
            lblDONo.Text = Common.parseNull(dtHeader.Rows(0)("DOM_DO_NO")) 'DO Number
            lblInvNo.Text = Common.parseNull(dtHeader.Rows(0)("IM_INVOICE_NO")) 'Invoice Number
            lblRevision.Text = objDb.GetVal("SELECT PV_REVISION FROM PIM_VENDOR WHERE PV_PRODUCT_INDEX=" & intProIndex & " AND PV_S_COY_ID='" & strVCoyID & "' AND PV_SUPP_CODE='" & Common.Parse(strSuppCode) & "'") 'Revision
            ViewState("DOM_DO_NO") = Common.parseNull(dtHeader.Rows(0)("DOM_DO_NO")) 'DO Number
            ViewState("IVL_LOT_INDEX") = Common.parseNull(dtHeader.Rows(0)("IVL_LOT_INDEX")) 'Lot Index
            ViewState("IVL_VERIFY_LOT_INDEX") = Common.parseNull(dtHeader.Rows(0)("IVL_VERIFY_LOT_INDEX")) 'Verify IQC Index
            ViewState("DOL_COY_ID") = Common.parseNull(dtHeader.Rows(0)("DOL_COY_ID")) 'Vendor ID

            'Invoice Date
            If IsDBNull(dtHeader.Rows(0)("IM_CREATED_ON")) Then
                lblInvDate.Text = ""
            Else
                lblInvDate.Text = Format(CDate(dtHeader.Rows(0)("IM_CREATED_ON")), "dd/MM/yyyy")
            End If

            lblVendor.Text = objDb.GetVal("SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID='" & Common.parseNull(dtHeader.Rows(0)("DOL_COY_ID")) & "'") 'Vendor Name
            lblManu.Text = Common.parseNull(dtHeader.Rows(0)("DOL_DO_MANUFACTURER")) 'Manufacturer
            lblIQCType.Text = Common.parseNull(dtHeader.Rows(0)("PM_IQC_TYPE")) 'IQC Type
            ViewState("PM_IQC_TYPE") = Common.parseNull(dtHeader.Rows(0)("PM_IQC_TYPE"))

            'Manufacturer Date
            If IsDBNull(dtHeader.Rows(0)("DOL_IQC_MANU_DT")) Then
                lblManuDate.Text = ""
            Else
                lblManuDate.Text = Format(CDate(dtHeader.Rows(0)("DOL_IQC_MANU_DT")), "dd/MM/yyyy")
            End If

            ds = objInv2.getIQCInfoFromPO(strPONo, strItemCode)
            lblPurSpecNo.Text = Common.parseNull(ds.Tables(0).Rows(0)("POD_PUR_SPEC_NO")) 'Purchasing Spec No
            lblSpec1.Text = Common.parseNull(ds.Tables(0).Rows(0)("POD_SPEC1")) 'Specification 1
            lblSpec2.Text = Common.parseNull(ds.Tables(0).Rows(0)("POD_SPEC2")) 'Specification 2
            lblSpec3.Text = Common.parseNull(ds.Tables(0).Rows(0)("POD_SPEC3")) 'Specification 3

            'Expiry Date
            If IsDBNull(dtHeader.Rows(0)("DOL_IQC_EXP_DT")) Then
                lblExpDate.Text = ""
            Else
                lblExpDate.Text = Format(CDate(dtHeader.Rows(0)("DOL_IQC_EXP_DT")), "dd/MM/yyyy")
            End If

            lblLotNo.Text = Common.parseNull(dtHeader.Rows(0)("DOL_LOT_NO")) 'Lot No

            'GRN Date
            If IsDBNull(dtHeader.Rows(0)("GM_CREATED_DATE")) Then
                lblGRNDate.Text = ""
            Else
                lblGRNDate.Text = Format(CDate(dtHeader.Rows(0)("GM_CREATED_DATE")), "dd/MM/yyyy")
            End If

            lblContinueLot.Text = objInv2.IQCChkLotContinue(ViewState("DOL_COY_ID"), Common.parseNull(dtHeader.Rows(0)("DOL_LOT_NO")), ViewState("DOM_DO_NO")) 'Continue Lot
            lblQtyReceived.Text = Common.parseNull(dtHeader.Rows(0)("IVL_LOT_QTY")) 'Qty Received
            lblUOM.Text = Common.parseNull(dtHeader.Rows(0)("PM_UOM")) 'UOM
        End If

        Dim dvFile As DataView
        Dim intLoop, intCount As Integer
        Dim strFile, strFile1, strURL, strTemp, strTempInt As String
        dvFile = objDO.getLotAttachment(ViewState("DOM_DO_NO"), ViewState("IVL_LOT_INDEX"), ViewState("DOL_COY_ID")).Tables(0).DefaultView
        If dvFile.Count > 0 Then
            intLoop = 0
            For intLoop = 0 To dvFile.Count - 1
                strFile = dvFile(intLoop)("CDDA_ATTACH_FILENAME")
                strFile1 = dvFile(intLoop)("CDDA_HUB_FILENAME")

                Dim objFile As New FileManagement
                strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DOAttachment, "DO", EnumUploadFrom.FrontOff, , ViewState("DOL_COY_ID"))
                objFile = Nothing
                '*************************meilai************************************
                If strTemp = "" Then
                    strTemp = "&nbsp;" & intCount + 1 & ") " & strURL & " (" & dvFile(intLoop)("CDDA_FILESIZE") & "KB)"
                    intCount = intCount + 1
                Else
                    strTemp = strTemp & "<BR>&nbsp;" & intCount + 1 & ") " & strURL & " (" & dvFile(intLoop)("CDDA_FILESIZE") & "KB)"
                    intCount = intCount + 1
                End If

            Next
        Else
            strTemp = "No Files Attached"
        End If
        lblFile.Text = strTemp

    End Sub

    Private Sub renderIQCApprFlow()
        Dim objInv As New Inventory
        ds = objInv.getApprFlow(ViewState("IVL_VERIFY_LOT_INDEX"))
        objInv = Nothing
        dtgAppFlow.DataSource = ds.Tables(0)
        dtgAppFlow.DataBind()
    End Sub

    Private Sub renderIQCApprFlowTracking()
        Dim objInv As New Inventory
        ds = objInv.getApprFlow(ViewState("IVL_VERIFY_LOT_INDEX"), 1)
        objInv = Nothing

        If ds.Tables(0).Rows.Count > 0 Then
            dtgAppFlowTracking.DataSource = ds.Tables(0)
            dtgAppFlowTracking.DataBind()
            div_IQC2.Style("display") = ""
            div_IQC1.Style("display") = ""
        Else
            div_IQC2.Style("display") = "none"
            div_IQC1.Style("display") = "none"
        End If

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub dtgAppFlow_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAppFlow.ItemCreated
        Grid_ItemCreated(sender, e)
    End Sub

    Private Sub dtgAppFlowTracking_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAppFlowTracking.ItemCreated
        Grid_ItemCreated(sender, e)
    End Sub

    Private Sub dtgAppFlow_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAppFlow.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim intTotalCell, intLoop As Integer
            If dv("IQCA_Seq") - 1 = dv("IQCA_AO_Action") Then
                intTotalCell = e.Item.Cells.Count - 1
                For intLoop = 0 To intTotalCell
                    e.Item.Cells(intLoop).Font.Bold = True
                Next
            End If

            If dv("IQCA_APPROVAL_TYPE") = 1 Then
                e.Item.Cells(3).Text = "Approval"
            End If

            If IsDBNull(dv("AAO_NAME")) Then
                e.Item.Cells(2).Text = "-"
            End If

            If UCase(Common.parseNull(dv("IQCA_ACTIVE_AO"))) = UCase(Common.parseNull(dv("IQCA_AO"))) Then
                e.Item.Cells(1).Font.Bold = True
            ElseIf UCase(Common.parseNull(dv("IQCA_ACTIVE_AO"))) = UCase(Common.parseNull(dv("IQCA_A_AO"))) Then
                e.Item.Cells(2).Font.Bold = True
            End If

            If Not IsDBNull(dv("IQCA_ACTION_DATE")) Then
                'e.Item.Cells(4).Text = Format(CDate(dv("IQCA_ACTION_DATE")), "dd/mm/yyyy") & " " & Format(CDate(dv("IQCA_ACTION_DATE")), "HH:mm:ss")
                e.Item.Cells(4).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("IQCA_ACTION_DATE")) & " " & Format(CDate(dv("IQCA_ACTION_DATE")), "HH:mm:ss")
            End If
        End If
    End Sub

    Private Sub dtgAppFlowTracking_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAppFlowTracking.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            'Dim intTotalCell, intLoop As Integer
            'If dv("IQCA_Seq") - 1 = dv("IQCA_AO_Action") Then
            '    intTotalCell = e.Item.Cells.Count - 1
            '    For intLoop = 0 To intTotalCell
            '        e.Item.Cells(intLoop).Font.Bold = True
            '    Next
            'End If

            If dv("IQCA_APPROVAL_TYPE") = 1 Then
                e.Item.Cells(3).Text = "Approval"
            End If

            If IsDBNull(dv("AAO_NAME")) Then
                e.Item.Cells(2).Text = "-"
            End If

            If UCase(Common.parseNull(dv("IQCA_ACTIVE_AO"))) = UCase(Common.parseNull(dv("IQCA_AO"))) Then
                e.Item.Cells(1).Font.Bold = True
            ElseIf UCase(Common.parseNull(dv("IQCA_ACTIVE_AO"))) = UCase(Common.parseNull(dv("IQCA_A_AO"))) Then
                e.Item.Cells(2).Font.Bold = True
            End If

            If Not IsDBNull(dv("IQCA_ACTION_DATE")) Then
                'e.Item.Cells(4).Text = Format(CDate(dv("IQCA_ACTION_DATE")), "dd/mm/yyyy") & " " & Format(CDate(dv("IQCA_ACTION_DATE")), "HH:mm:ss")
                e.Item.Cells(4).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("IQCA_ACTION_DATE")) & " " & Format(CDate(dv("IQCA_ACTION_DATE")), "HH:mm:ss")
            End If
        End If
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_SearchPRAO_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IQC", "SearchIQC_AO.aspx", "pageid=" & strPageId) & """><span>Approval List</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("IQC", "SearchIQC_All.aspx", "pageid=" & strPageId) & """><span>Closed / Outstanding Listing</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "</ul><div></div></div>"
    End Sub
End Class


