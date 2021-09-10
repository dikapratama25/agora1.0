'Copyright © 2013 STRATEQ GLOBAL SERVICES. All rights reserved.
'//Outstanding
'//Calc By - Product /SubTotal
'// GST - in %
'//default - Subtotal , 0%
'//To find if the b_product_name is completely null for a PO (from D_PR),approveao.asp line 206
'//

Imports AgoraLegacy
Imports eProcure.Component
Imports System.Drawing

Public Class IQCApprDetail
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
            hidAction.Value = ""

            renderIQCHeader()
            renderIQCApprFlow()
            renderIQCApprFlowTracking()

            strOfficerType = objDb.GetVal("SELECT IQCA_OFFICER_TYPE FROM IQC_APPROVAL WHERE IQCA_IQC_INDEX = " & ViewState("IVL_VERIFY_LOT_INDEX") & " AND IQCA_SEQ - 1 = IQCA_AO_ACTION")
            hidType.Value = strOfficerType
            If strOfficerType = "IQCASTS" Then
                cmdVerifyIQC.Style("display") = ""
                cmdAppIQC.Style("display") = "none"
                cmdReTestIQC.Style("display") = "none"
                cmdVerifyIQC.Text = "Approve"
            Else
                If strOfficerType = "IQCV" Then
                    cmdVerifyIQC.Style("display") = ""
                    cmdAppIQC.Style("display") = "none"
                    cmdReTestIQC.Style("display") = "none"
                    cmdVerifyIQC.Text = "Verify"
                ElseIf strOfficerType = "IQCPA" Or strOfficerType = "IQCA" Then
                    cmdVerifyIQC.Style("display") = "none"
                    cmdAppIQC.Style("display") = ""
                    cmdReTestIQC.Style("display") = ""
                End If
            End If

            If ViewState("CurrentAppSeq") = ViewState("HighestAppr") Then
                ViewState("ISHighestLevel") = True
            Else
                ViewState("ISHighestLevel") = False
            End If

            'if level <> 1 then hide some fields
            If ViewState("CurrentAppSeq") <> 1 Then
                txtManuDate.Style("display") = "none"
                lblManuDate_M.Style("display") = "none"
                txtExpDate.Style("display") = "none"
                lblExpDate_M.Style("display") = "none"
                span_ManuDate.Style("display") = "none"
                span_ExpDate.Style("display") = "none"
                lblReceivedDate2.Style("display") = "none"
                lblGRNDate2.Style("display") = "none"
            Else
                lblReceivedDate.Style("display") = "none"
                lblGRNDate.Style("display") = "none"
            End If

            cmdUpload.Attributes.Add("onclick", "return checkDocFile('doc','" & File1.ClientID & "');")

            If Session("urlreferer") = "Dashboard" Then
                lnkBack.NavigateUrl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId)
            ElseIf Session("urlreferer") = "SearchIQCAO" Then
                lnkBack.NavigateUrl = dDispatcher.direct("IQC", "SearchIQC_AO.aspx", "pageId=" & strPageId)
            End If

            txtRemark.Attributes.Add("onKeyDown", "limitText (this, 900);")
            hidSummary.Value = "Remarks-" & txtRemark.ClientID
            cmdVerifyIQC.Attributes.Add("onClick", "return confirmApprove('" & LCase(cmdVerifyIQC.Text) & "');")
            cmdAppIQC.Attributes.Add("onClick", "return confirmApprove('" & LCase(cmdAppIQC.Text) & "');")

            If hidType.Value = "IQCPA" Or (hidType.Value = "IQCA" And ViewState("ISHighestLevel") = False) Then
            Else
                cmdRejectIQC.Attributes.Add("onClick", "return confirmReject();")
                cmdReTestIQC.Attributes.Add("onClick", "return confirmRetest();")
            End If

            Image1.ImageUrl = dDispatcher.direct("Plugins/images", "collapse_up.gif")
        End If


        displayAttachFile()

        'Check Access Status
        If Session("AccessStatus") = "Limited" Then
            cmdRejectIQC.Enabled = False
            cmdReTestIQC.Enabled = False
            cmdAppIQC.Enabled = False
            cmdVerifyIQC.Enabled = False
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
                lblGRNDate2.Text = ""
            Else
                lblGRNDate.Text = Format(CDate(dtHeader.Rows(0)("GM_CREATED_DATE")), "dd/MM/yyyy")
                lblGRNDate2.Text = Format(CDate(dtHeader.Rows(0)("GM_CREATED_DATE")), "dd/MM/yyyy")
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

                ViewState("CurrentAppSeq") = dv("IQCA_Seq")
                ViewState("ApprType") = dv("IQCA_APPROVAL_TYPE")

                ''//if relief ao, how???, CANNOT USER SESSION("USERID")/
            End If

            ViewState("HighestAppr") = dv("IQCA_Seq")
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

    Private Function FormatAORemark(ByVal strAction As String) As String
        Dim strRemark, strTempRemark, strUserName As String
        If Request.QueryString("relief") Then
            Dim objUsers As New Users
            Dim objUser As New User
            objUsers.GetUserDetail(Request.QueryString("AO"), Session("CompanyId"), objUser)
            strUserName = objUser.Name
            objUsers = Nothing
            objUser = Nothing
            strTempRemark = "(On Behalf of " & strUserName & ") "
        Else
            strTempRemark = ""
        End If

        Select Case strAction
            Case "approve"
                strRemark = "Approved " & strTempRemark & ": "
            Case "reject"
                If hidReject.Value = "r" Then
                    strRemark = "Replaced " & strTempRemark & ": "
                ElseIf hidReject.Value = "w" Then
                    strRemark = "Waived " & strTempRemark & ": "
                Else
                    strRemark = "Rejected " & strTempRemark & ": "
                End If
            Case "verify"
                If hidType.Value = "IQCASTS" Then
                    strRemark = "Approved " & strTempRemark & ": "
                Else
                    strRemark = "Verified " & strTempRemark & ": "
                End If
            Case "retest"
                strRemark = "Retest " & strTempRemark & ": "
            Case "hold"
                strRemark = "Held " & strTempRemark & ": "
        End Select
        Return strRemark
    End Function

    Private Sub cmdUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpload.Click
        If File1.Value <> "" Then
            Dim objFile As New FileManagement
            Dim objPR As New PR
            Dim objDB As New EAD.DBCom

            ' Restrict user upload size
            Dim dblMaxFileSize As Double = Val(objDB.GetVal("SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_TYPE = 'USER' AND SP_PARAM_NAME = 'DocAttachSize'"))

            Dim sFileName As String
            sFileName = System.IO.Path.GetFileName(File1.PostedFile.FileName)

            If Len(sFileName) > 50 Then
                Common.NetMsgbox(Me, "File name exceeds 46 character")
            ElseIf File1.PostedFile.ContentLength > 0 And File1.PostedFile.ContentLength / 1024 <= dblMaxFileSize Then
                objFile.FileUpload(File1, EnumUploadType.UserAttachment, "IQC", EnumUploadFrom.FrontOff, ViewState("IVL_VERIFY_LOT_INDEX"), , "AO", Nothing, ViewState("CurrentAppSeq"))
            ElseIf File1.PostedFile.ContentLength = 0 Then
                Common.NetMsgbox(Me, "0 byte document or file not found")
            Else
                Common.NetMsgbox(Me, "File exceeds maximum file size")
            End If

            objPR = Nothing
            objFile = Nothing
        End If
        displayAttachFile()
        renderIQCApprFlow()

    End Sub

    Private Sub displayAttachFile()
        Dim objPR As New PR
        Dim dsAttach As New DataSet
        Dim drvAttach As DataView
        Dim i As Integer
        Dim strFile, strFile1, strURL, strTemp As String

        dsAttach = objPR.getUserAttach("AO", "IQC", ViewState("IVL_VERIFY_LOT_INDEX"), ViewState("CurrentAppSeq"))

        pnlAttach.Controls.Clear()
        drvAttach = dsAttach.Tables(0).DefaultView
        If drvAttach.Count > 0 Then
            For i = 0 To drvAttach.Count - 1
                strFile = drvAttach(i)("UA_ATTACH_FILENAME")
                strFile1 = drvAttach(i)("UA_HUB_FILENAME")
                '**********************meilai 25/02/2005****************** 
                'strURL = "<A HREF=../FileDownload.aspx?pb=" & viewstate("postback") & "&file=" & Server.UrlEncode(strFile1) & "&actualfile=" & Server.UrlEncode(strFile) & "&type=" & EnumDownLoadType.DocAttachment & ">" & strFile & "</A>"
                Dim objFile As New FileManagement
                strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.UserAttachment, "", EnumUploadFrom.FrontOff)
                objFile = Nothing
                '**********************meilai*****************************
                Dim lblBrAO As New Label
                Dim lblBr As New Label
                Dim lblFileAO As New Label
                Dim lnk As New ImageButton

                lblFileAO.Text = "&nbsp;" & i + 1 & ") " & strURL & " (" & drvAttach(i)("UA_FILESIZE") & "KB) "
                lblBrAO.Text = "<BR>"
                lnk.ImageUrl = dDispatcher.direct("Plugins/images", "i_delete2.gif")
                lnk.ID = drvAttach(i)("UA_ATTACH_INDEX")
                lnk.CausesValidation = False
                AddHandler lnk.Click, AddressOf deleteAttach

                pnlAttach.Controls.Add(lblFileAO)
                pnlAttach.Controls.Add(lnk)
                pnlAttach.Controls.Add(lblBrAO)
            Next
            pnlAttach.Controls.Add(New System.Web.UI.HtmlControls.HtmlGenericControl)
        Else
            Dim lblFileAO As New Label
            lblFileAO.Text = "No Files Attached"
            pnlAttach.Controls.Add(lblFileAO)
        End If
        objPR = Nothing
    End Sub

    Private Sub deleteAttach(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim objPR As New PR
        objPR.deleteUserAttachment(CType(sender, ImageButton).ID)
        displayAttachFile()
        objPR = Nothing
        renderIQCApprFlow()

    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_SearchIQCAO_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("IQC", "SearchIQC_AO.aspx", "pageid=" & strPageId) & """><span>Approval List</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IQC", "SearchIQC_All.aspx", "pageid=" & strPageId) & """><span>Closed / Outstanding Listing</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "</ul><div></div></div>"
    End Sub

    Private Sub cmdAppIQC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAppIQC.Click
        Dim strRemark, strMsg As String
        Dim objInv As New Inventory
        Dim chk As Boolean

        If hidAction.Value = "" Then
            hidAction.Value = "approve"
        End If

        lblMsg.Text = ""
        strRemark = FormatAORemark(hidAction.Value)
        strRemark = strRemark & txtRemark.Text

        'If txtRemark.Text = "" And (txtManuDate.Text <> "" Or txtExpDate.Text <> "") Then
        '    Common.NetMsgbox(Me, "Please enter Remarks.", MsgBoxStyle.Information)
        'Else
        If hidAction.Value = "reject" And txtRemark.Text = "" Then
            Common.NetMsgbox(Me, "Please enter Remarks.", MsgBoxStyle.Information)
        Else
            If validateDatagrid(strMsg) Then
                strMsg = objInv.ApproveIQC(ViewState("IVL_IQC_NO"), ViewState("IVL_VERIFY_LOT_INDEX"), ViewState("CurrentAppSeq"), ViewState("ISHighestLevel"), strRemark, Request.QueryString("relief"), ViewState("ApprType"), hidType.Value, ViewState("userSKId"), hidAction.Value)
                If Session("urlreferer") = "Dashboard" Then
                    Common.NetMsgbox(Me, strMsg, dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId), MsgBoxStyle.Exclamation)
                Else
                    Common.NetMsgbox(Me, strMsg, dDispatcher.direct("IQC", "SearchIQC_AO.aspx", "pageid=" & strPageId), MsgBoxStyle.Exclamation)
                End If

                hidAction.Value = ""
                cmdRejectIQC.Visible = False
                cmdReTestIQC.Visible = False
                cmdAppIQC.Visible = False
                cmdVerifyIQC.Visible = False
            Else
                lblMsg.Text = strMsg
            End If
        End If
        hidAction.Value = ""

    End Sub

    Private Sub cmdRejectIQC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRejectIQC.Click
        Dim objInv As New Inventory
        Dim strRemark, strMsg As String

        If hidAction.Value = "" Then
            hidAction.Value = "reject"
        End If

        lblMsg.Text = ""
        strRemark = FormatAORemark(hidAction.Value)
        strRemark = strRemark & txtRemark.Text

        If hidType.Value = "IQCPA" Or (hidType.Value = "IQCA" And ViewState("ISHighestLevel") = False) Then
            cmdAppIQC_Click(sender, e)
        Else
            If validateDatagrid(strMsg) Then
                If txtRemark.Text = "" Then
                    Common.NetMsgbox(Me, "Please enter Remarks.", MsgBoxStyle.Information)
                Else
                    strMsg = objInv.RejectIQC(ViewState("IVL_IQC_NO"), ViewState("IVL_VERIFY_LOT_INDEX"), ViewState("CurrentAppSeq"), strRemark, Request.QueryString("relief"), hidReject.Value, txtManuDate.Text, txtExpDate.Text, ViewState("ISHighestLevel"), ViewState("userSKId"))

                    If Session("urlreferer") = "Dashboard" Then
                        Common.NetMsgbox(Me, strMsg, dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId), MsgBoxStyle.Exclamation)
                    Else
                        Common.NetMsgbox(Me, strMsg, dDispatcher.direct("IQC", "SearchIQC_AO.aspx", "pageid=" & strPageId), MsgBoxStyle.Exclamation)
                    End If

                    cmdRejectIQC.Visible = False
                    cmdReTestIQC.Visible = False
                    cmdAppIQC.Visible = False
                    cmdVerifyIQC.Visible = False

                End If
            Else
                lblMsg.Text = strMsg
            End If
        End If
        hidAction.Value = ""

    End Sub

    Private Sub cmdVerifyIQC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdVerifyIQC.Click
        Dim strRemark, strMsg As String
        Dim objInv As New Inventory
        Dim chk As Boolean

        If hidAction.Value = "" Then
            hidAction.Value = "verify"
        End If

        lblMsg.Text = ""
        strRemark = FormatAORemark(hidAction.Value)
        strRemark = strRemark & txtRemark.Text

        If validateDatagrid(strMsg) Then
            If txtRemark.Text = "" And (txtManuDate.Text <> "" Or txtExpDate.Text <> "") Then
                Common.NetMsgbox(Me, "Please enter Remarks.", MsgBoxStyle.Information)
            Else
                strMsg = objInv.VerifyIQC(ViewState("IVL_IQC_NO"), ViewState("IVL_VERIFY_LOT_INDEX"), ViewState("CurrentAppSeq"), ViewState("ISHighestLevel"), strRemark, Request.QueryString("relief"), ViewState("ApprType"), txtManuDate.Text, txtExpDate.Text, hidType.Value, ViewState("userSKId"))

                If Session("urlreferer") = "Dashboard" Then
                    Common.NetMsgbox(Me, strMsg, dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId), MsgBoxStyle.Exclamation)
                Else
                    Common.NetMsgbox(Me, strMsg, dDispatcher.direct("IQC", "SearchIQC_AO.aspx", "pageid=" & strPageId), MsgBoxStyle.Exclamation)
                End If

                cmdRejectIQC.Visible = False
                cmdReTestIQC.Visible = False
                cmdAppIQC.Visible = False
                cmdVerifyIQC.Visible = False
            End If
        Else
            lblMsg.Text = strMsg
        End If
        hidAction.Value = ""

    End Sub

    Private Sub cmdReTestIQC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdReTestIQC.Click
        Dim strRemark, strMsg As String
        Dim objInv As New Inventory
        Dim chk As Boolean

        If hidAction.Value = "" Then
            hidAction.Value = "retest"
        End If

        lblMsg.Text = ""
        strRemark = FormatAORemark(hidAction.Value)
        strRemark = strRemark & txtRemark.Text

        If hidType.Value = "IQCPA" Or (hidType.Value = "IQCA" And ViewState("ISHighestLevel") = False) Then
            cmdAppIQC_Click(sender, e)
        Else
            strMsg = objInv.ReTestIQC(ViewState("IVL_IQC_NO"), ViewState("IVL_VERIFY_LOT_INDEX"), ViewState("CurrentAppSeq"), strRemark, Request.QueryString("relief"))

            If Session("urlreferer") = "Dashboard" Then
                Common.NetMsgbox(Me, strMsg, dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId), MsgBoxStyle.Exclamation)
            Else
                Common.NetMsgbox(Me, strMsg, dDispatcher.direct("IQC", "SearchIQC_AO.aspx", "pageid=" & strPageId), MsgBoxStyle.Exclamation)
            End If

            cmdRejectIQC.Visible = False
            cmdReTestIQC.Visible = False
            cmdAppIQC.Visible = False
            cmdVerifyIQC.Visible = False
            hidAction.Value = ""
        End If

    End Sub

    Private Function validateDatagrid(ByRef strMsg As String) As Boolean
        validateDatagrid = True
        strMsg = "<ul type='disc'>"

        If ViewState("CurrentAppSeq") = 1 Then
            If txtManuDate.Text <> "" Then
                If CDate(txtManuDate.Text) > Today.Date Then
                    strMsg &= "<li>Mfg Date should be <= today date<ul type='disc'></ul></li>"
                    validateDatagrid = False
                End If
            End If

            If txtManuDate.Text <> "" And txtExpDate.Text <> "" Then
                If CDate(txtManuDate.Text) >= CDate(txtExpDate.Text) Then
                    strMsg &= "<li>Expiry Date should be > Mfg Date<ul type='disc'></ul></li>"
                    validateDatagrid = False
                End If
            End If
        End If

        strMsg &= "</ul>"

    End Function
End Class


