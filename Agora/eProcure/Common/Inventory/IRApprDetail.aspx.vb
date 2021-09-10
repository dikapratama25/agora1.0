'//Outstanding
'//Calc By - Product /SubTotal
'// GST - in %
'//default - Subtotal , 0%
'//To find if the b_product_name is completely null for a PO (from D_PR),approveao.asp line 206
'//

Imports AgoraLegacy
Imports eProcure.Component
Imports System.Drawing

Public Class IRApprDetail
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

    Enum EnumIR
        icItemCode
        icItemName
        icUom
        icMthStkIssued
        icLast3MthAve
        icQty
        icLine
    End Enum
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
        'CheckButtonAccess(True)
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
        SetGridProperty(dtgItem)

        If Not Page.IsPostBack Then
            GenerateTab()
            Dim objInv As New Inventory
            dsAllInfo = objInv.getIRForAppr(Request.QueryString("IRNo"))
            objInv = Nothing

            renderIRHeader()
            renderIRApprFlow()
            renderIRDetail()

            If ViewState("CurrentAppSeq") = ViewState("HighestAppr") Then
                ViewState("ISHighestLevel") = True
            Else
                ViewState("ISHighestLevel") = False
            End If

            cmdUpload.Attributes.Add("onclick", "return checkDocFile('doc','" & File1.ClientID & "');")

            If Session("urlreferer") = "Dashboard" Then
                lnkBack.NavigateUrl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId)
            ElseIf Session("urlreferer") = "SearchInventoryReq_AO" Then
                lnkBack.NavigateUrl = dDispatcher.direct("Inventory", "SearchInventoryReq_AO.aspx", "pageId=" & strPageId)
            End If

            hidSummary.Value = "Remarks-" & txtRemark.ClientID
            txtRemark.Attributes.Add("onKeyDown", "limitText (this, 900);")
            cmdAppIR.Attributes.Add("onClick", "return confirmApprove();")
            cmdRejectIR.Attributes.Add("onClick", "return confirmReject();")

        End If


        displayAttachFile()

        'Check Access Status
        If Session("AccessStatus") = "Limited" Then
            cmdAppIR.Enabled = False
            cmdRejectIR.Enabled = False
        End If

    End Sub

    Public Function CreateFileLinks(ByVal userID As Object, ByVal altUserID As Object, ByVal seq As String, ByVal tb_iqc As String) As DataTable
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
        Dim ds As DataSet = pr.getUserAttach("AO", tb_iqc, CStr(ViewState("IRM_IR_INDEX")), id1, id2, seq)

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

    Private Sub renderIRHeader()
        Dim dtHeader As New DataTable
        Dim objInv2 As New Inventory
        Dim strDeptName As String
        dtHeader = dsAllInfo.Tables(0)
        If dtHeader.Rows.Count > 0 Then

            'Session("userSKId") = Common.parseNull(dtHeader.Rows(0)("GM_CREATED_BY"))
            'Session("lblIQCNo") = Common.parseNull(dtHeader.Rows(0)("IVL_IQC_NO"))
            'IR Number
            If Common.parseNull(dtHeader.Rows(0)("IRM_IR_URGENT")) = "Y" Then
                lblIRNo.Text = Me.Request.QueryString("IRNo") & " (Urgent)"
            Else
                lblIRNo.Text = Me.Request.QueryString("IRNo")
            End If
            ViewState("IRNo") = Request.QueryString("IRNo")
            ViewState("IRM_IR_INDEX") = Common.parseNull(dsAllInfo.Tables(0).Rows(0)("IRM_IR_INDEX"))

            'Status
            If Common.parseNull(dtHeader.Rows(0)("IRM_IR_STATUS")) = "1" Then
                lblStatus.Text = "Submitted"
            ElseIf Common.parseNull(dtHeader.Rows(0)("IRM_IR_STATUS")) = "2" Then
                lblStatus.Text = "Approved"
            ElseIf Common.parseNull(dtHeader.Rows(0)("IRM_IR_STATUS")) = "3" Then
                lblStatus.Text = "Pending Approval"
            ElseIf Common.parseNull(dtHeader.Rows(0)("IRM_IR_STATUS")) = "4" Then
                lblStatus.Text = "Rejected"
            Else
                lblStatus.Text = ""
            End If

            lblIRDate.Text = Format(CDate(dtHeader.Rows(0)("IRM_IR_DATE")), "dd/MM/yyyy") 'IR Date
            lblReqName.Text = Common.parseNull(dtHeader.Rows(0)("IRM_IR_REQUESTOR_NAME")) 'Requestor Name
            lblIssue.Text = Common.parseNull(dtHeader.Rows(0)("IRM_IR_ISSUE_TO")) 'Issue To
            If IsDBNull(dtHeader.Rows(0)("CS_SEC_NAME")) Then
                lblSection.Text = Common.parseNull(dtHeader.Rows(0)("IRM_IR_SECTION")) 'Section Code
            Else
                lblSection.Text = Common.parseNull(dtHeader.Rows(0)("IRM_IR_SECTION")) & " : " & dtHeader.Rows(0)("CS_SEC_NAME")  'Section Code : Section Name
            End If
            lblRefNo.Text = Common.parseNull(dtHeader.Rows(0)("IRM_IR_REF_NO")) 'Reference No
            ViewState("IRM_IR_DEPARTMENT") = dsAllInfo.Tables(0).Rows(0)("IRM_IR_DEPARTMENT")
            ViewState("IRM_CREATED_BY") = dsAllInfo.Tables(0).Rows(0)("IRM_CREATED_BY")
            strDeptName = objDb.GetVal("SELECT CDM_DEPT_NAME FROM COMPANY_DEPT_MSTR WHERE CDM_COY_ID = '" & Session("CompanyID") & "' " &
                    "AND CDM_DEPT_CODE = '" & dsAllInfo.Tables(0).Rows(0)("IRM_IR_DEPARTMENT") & "' ")
            lblDept.Text = strDeptName 'Department
            lblRemark.Text = Common.parseNull(dtHeader.Rows(0)("IRM_IR_REMARK")) 'Remark

        End If

    End Sub

    Private Sub renderIRApprFlow()
        Dim objInv As New Inventory
        ds = objInv.getApprFlow(ViewState("IRM_IR_INDEX"), , "MRS")
        objInv = Nothing
        dtgAppFlow.DataSource = ds.Tables(0)
        dtgAppFlow.DataBind()
    End Sub

    Private Sub renderIRDetail()
        Bindgrid(False)
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String

        Dim dvViewIR As DataView
        dvViewIR = dsAllInfo.Tables("INVENTORY_REQUISITION_DETAILS").DefaultView
        'dvViewSample(0)(0)
        If pSorted Then
            dvViewIR.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" Then dvViewIR.Sort += " DESC"
        End If

        intPageRecordCnt = dsAllInfo.Tables("INVENTORY_REQUISITION_DETAILS").Rows.Count

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            dtgItem.DataSource = dvViewIR
            dtgItem.DataBind()
        Else
            dtgItem.DataBind()
        End If

    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub dtgAppFlow_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAppFlow.ItemCreated
        Grid_ItemCreated(sender, e)
    End Sub

    Private Sub dtgAppFlow_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAppFlow.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim intTotalCell, intLoop As Integer
            If dv("IRA_Seq") - 1 = dv("IRA_AO_Action") Then
                intTotalCell = e.Item.Cells.Count - 1
                For intLoop = 0 To intTotalCell
                    e.Item.Cells(intLoop).Font.Bold = True
                Next

                ViewState("CurrentAppSeq") = dv("IRA_Seq")
                ViewState("ApprType") = dv("IRA_APPROVAL_TYPE")

                ''//if relief ao, how???, CANNOT USER SESSION("USERID")/
            End If

            ViewState("HighestAppr") = dv("IRA_Seq")
            If dv("IRA_APPROVAL_TYPE") = 1 Then
                e.Item.Cells(3).Text = "Approval"
            End If

            If IsDBNull(dv("AAO_NAME")) Then
                e.Item.Cells(2).Text = "-"
            End If

            If UCase(Common.parseNull(dv("IRA_ACTIVE_AO"))) = UCase(Common.parseNull(dv("IRA_AO"))) Then
                e.Item.Cells(1).Font.Bold = True
            ElseIf UCase(Common.parseNull(dv("IRA_ACTIVE_AO"))) = UCase(Common.parseNull(dv("IRA_A_AO"))) Then
                e.Item.Cells(2).Font.Bold = True
            End If

            If Not IsDBNull(dv("IRA_ACTION_DATE")) Then
                e.Item.Cells(4).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("IRA_ACTION_DATE")) & " " & Format(dv("IRA_ACTION_DATE"), "HH:mm:ss")
            End If
        End If
    End Sub

    Private Sub dtgItem_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgItem.ItemCreated
        Grid_ItemCreated(dtgItem, e)
    End Sub

    Private Sub dtgItem_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgItem.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim objInv As New Inventory
            Dim dblMthStkIssed, dblLast3MthAve As Decimal

            Dim lblMthStock As Label
            lblMthStock = e.Item.FindControl("lblMthStock")
            dblMthStkIssed = objInv.getMonthStockBalance(dv("IM_ITEM_CODE"), ViewState("IRM_CREATED_BY"))
            lblMthStock.Text = Format(dblMthStkIssed, "#####0.00")

            Dim lbl3MthAve As Label
            lbl3MthAve = e.Item.FindControl("lbl3MthAve")
            dblLast3MthAve = objInv.getLast3MthAve(dv("IM_ITEM_CODE"), ViewState("IRM_IR_DEPARTMENT"))
            lbl3MthAve.Text = Format(dblLast3MthAve, "#####0.00")

            objInv = Nothing

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
                strRemark = "Rejected " & strTempRemark & ": "
            Case "hold"
                strRemark = "Held" & strTempRemark & ": "
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
                objFile.FileUpload(File1, EnumUploadType.UserAttachment, "IR", EnumUploadFrom.FrontOff, ViewState("IRM_IR_INDEX"), , "AO", Nothing, ViewState("CurrentAppSeq"))
            ElseIf File1.PostedFile.ContentLength = 0 Then
                Common.NetMsgbox(Me, "0 byte document or file not found")
            Else
                Common.NetMsgbox(Me, "File exceeds maximum file size")
            End If

            objPR = Nothing
            objFile = Nothing
        End If
        displayAttachFile()
        renderIRApprFlow()

    End Sub

    Private Sub displayAttachFile()
        Dim objPR As New PR
        Dim dsAttach As New DataSet
        Dim drvAttach As DataView
        Dim i As Integer
        Dim strFile, strFile1, strURL, strTemp As String

        dsAttach = objPR.getUserAttach("AO", "IR", ViewState("IRM_IR_INDEX"), ViewState("CurrentAppSeq"))

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
        renderIRApprFlow()

    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_SearchIRAO_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "SearchInventoryReq_AO.aspx", "pageid=" & strPageId) & """><span>Approval List</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "SearchInventoryReq_All.aspx", "pageid=" & strPageId) & """><span>Approved / Rejected Listing</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "</ul><div></div></div>"
    End Sub

    Private Sub cmdAppIR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAppIR.Click
        Dim strRemark, strMsg As String
        Dim objInv As New Inventory
        Dim dsIRDetail As New DataSet
        Dim chk As Boolean

        If validateInputs(strMsg) Then
            strRemark = FormatAORemark("approve")
            strRemark = strRemark & txtRemark.Text
            dsIRDetail = bindIRDetail()
            strMsg = objInv.ApproveIR(Request.QueryString("IRNo"), ViewState("IRM_IR_INDEX"), ViewState("CurrentAppSeq"), ViewState("ISHighestLevel"), strRemark, Request.QueryString("relief"), ViewState("ApprType"), dsIRDetail)

            If Session("urlreferer") = "Dashboard" Then
                Common.NetMsgbox(Me, strMsg, dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId), MsgBoxStyle.Exclamation)
            Else
                Common.NetMsgbox(Me, strMsg, dDispatcher.direct("Inventory", "SearchInventoryReq_AO.aspx", "pageid=" & strPageId), MsgBoxStyle.Exclamation)
            End If

            cmdRejectIR.Visible = False
            cmdAppIR.Visible = False
        Else
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
        End If

    End Sub

    Private Sub cmdRejectIR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRejectIR.Click
        Dim objInv As New Inventory
        Dim dsIRDetail As New DataSet
        Dim strRemark, strMsg As String
        Dim strscript As New System.Text.StringBuilder
        strRemark = FormatAORemark("reject")
        strRemark = strRemark & txtRemark.Text

        If txtRemark.Text = "" Then
            Common.NetMsgbox(Me, "Please enter Remarks.", MsgBoxStyle.Information)
        Else
            dsIRDetail = bindIRDetail()
            strMsg = objInv.RejectIR(Request.QueryString("IRNo"), ViewState("IRM_IR_INDEX"), ViewState("CurrentAppSeq"), strRemark, Request.QueryString("relief"), dsIRDetail)
            If Session("urlreferer") = "Dashboard" Then
                Common.NetMsgbox(Me, strMsg, dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId), MsgBoxStyle.Exclamation)
            Else
                Common.NetMsgbox(Me, strMsg, dDispatcher.direct("Inventory", "SearchInventoryReq_AO.aspx", "pageid=" & strPageId), MsgBoxStyle.Exclamation)
            End If

            cmdRejectIR.Visible = False
            cmdAppIR.Visible = False
        End If

    End Sub

    Private Function validateInputs(ByRef strMsg As String) As Boolean
        Dim dgItem As DataGridItem
        Dim objInv As New Inventory
        Dim dblMthStkIssed, dblLast3MthAve, dblReqQty, dblStkBal As Decimal
        Dim i As Integer
        Dim lblMthStock As Label
        Dim lbl3MthAve As Label

        For i = 0 To dtgItem.Items.Count() - 1
            lblMthStock = dtgItem.Items(i).Cells(EnumIR.icMthStkIssued).FindControl("lblMthStock")
            lbl3MthAve = dtgItem.Items(i).Cells(EnumIR.icLast3MthAve).FindControl("lbl3MthAve")

            dblMthStkIssed = objInv.getMonthStockBalance(dtgItem.Items(i).Cells(EnumIR.icItemCode).Text)
            dblLast3MthAve = objInv.getLast3MthAve(dtgItem.Items(i).Cells(EnumIR.icItemCode).Text, ViewState("IRM_IR_DEPARTMENT"))
            dblStkBal = objInv.getStockBalance(dtgItem.Items(i).Cells(EnumIR.icItemCode).Text)
            dblReqQty = CDec(dtgItem.Items(i).Cells(EnumIR.icQty).Text)

            If ((dblMthStkIssed > dblLast3MthAve) Or (dblReqQty > dblLast3MthAve)) And txtRemark.Text = "" Then
                lblMthStock.Text = Format(dblMthStkIssed, "#####0.00")
                lbl3MthAve.Text = Format(dblLast3MthAve, "#####0.00")
                strMsg = "Please enter Remarks."
                Return False
            End If

            If dblReqQty > dblStkBal Then
                strMsg = "Quantity requested cannot be more than stock balance quantity."
                Return False
            End If
        Next

        Return True
    End Function

    Private Function bindIRDetail() As DataSet
        Dim ds As New DataSet
        Dim dtDetails As New DataTable

        dtDetails.Columns.Add("IRNo", Type.GetType("System.String"))
        dtDetails.Columns.Add("Line", Type.GetType("System.Int32"))
        dtDetails.Columns.Add("ItemCode", Type.GetType("System.String"))
        dtDetails.Columns.Add("MonthlyStockIssued", Type.GetType("System.String"))
        dtDetails.Columns.Add("Last3MthAve", Type.GetType("System.String"))

        Dim dtrd As DataRow
        Dim dgItem As DataGridItem
        For Each dgItem In dtgItem.Items
            dtrd = dtDetails.NewRow()
            dtrd("IRNo") = ViewState("IRNo")
            dtrd("Line") = dgItem.Cells(EnumIR.icLine).Text
            dtrd("ItemCode") = dgItem.Cells(EnumIR.icItemCode).Text
            dtrd("MonthlyStockIssued") = CType(dgItem.FindControl("lblMthStock"), Label).Text
            dtrd("Last3MthAve") = CType(dgItem.FindControl("lbl3MthAve"), Label).Text
            dtDetails.Rows.Add(dtrd)
        Next
        ds.Tables.Add(dtDetails)
        bindIRDetail = ds
    End Function
End Class


