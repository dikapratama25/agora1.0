'//Outstanding
'//RFQ Ind
'//Attachment Ind

Imports AgoraLegacy
Imports eProcure.Component
Public Class SearchPO_AO
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

    Protected WithEvents txtPONo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVendor As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDateFr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDateTo As System.Web.UI.WebControls.TextBox
    'Protected WithEvents chkOpen As System.Web.UI.WebControls.CheckBox
    'Protected WithEvents chkAwaitAppr As System.Web.UI.WebControls.CheckBox
    'Protected WithEvents chkAppr As System.Web.UI.WebControls.CheckBox
    'Protected WithEvents chkPOCreate As System.Web.UI.WebControls.CheckBox
    'Protected WithEvents chkCancel As System.Web.UI.WebControls.CheckBox
    'Protected WithEvents chkHold As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents vldDateFtDateTo As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents vldDateFr As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents vldDateTo As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents dtgPOList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdMassApp As System.Web.UI.WebControls.Button
    Protected WithEvents Table2 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSearchResult As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents cboAO As System.Web.UI.WebControls.DropDownList
    Protected WithEvents tdAO As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Dim strCaller As String
    Public Enum EnumPO
        icCheckBox
        icPONoLink
        icSubmitDate
        icBuyer
        icVendor
        icCurrency
        icAmt
        icStatus
        icPONo
        icBuyerID
        icPRNo
    End Enum
    Dim strAO As String
    Protected WithEvents lblVendor As System.Web.UI.WebControls.Label
    Dim blnRelief As Boolean
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
        'yAP: 21May2014; 
        'Comment out this portion, this to avoid error page encouter when checking on strPageID when check on Menu
        ''CheckButtonAccess(True)
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here

        strCaller = UCase(Request.QueryString("caller"))
        'strCaller = Session("strCaller")
        SetGridProperty(dtgPOList)

        MyBase.Page_Load(sender, e)

        '//because No DataGrid display when page first loaded
        cmdReset.Disabled = True
        cmdMassApp.Enabled = True


        If cboAO.SelectedIndex <= 0 Then
            strAO = Session("UserID")
            blnRelief = False
        Else
            strAO = cboAO.SelectedValue
            blnRelief = True
        End If

        If Not Page.IsPostBack Then
            GenerateTab()
            Session("strurl") = Me.strCallFrom ' (gary add) url back from quote compare
            getReliefList()
            If cboAO.SelectedIndex < 0 Then
                cmdSearch_Click(sender, e)
            End If
            cmdSearch_Click(sender, e)
        End If

        'Check Access Status
        If Session("AccessStatus") = "Limited" Then
            cmdMassApp.Enabled = False
        End If

    End Sub

    Private Sub getReliefList()
        Dim dv As DataView
        Dim objPR As New PurchaseReq2
        dv = objPR.getReliefList("PO")
        If Not dv Is Nothing Then
            tdAO.Style("VISIBILITY") = "visible"
            tdAO.Style("display") = ""
            Common.FillDdl(cboAO, "NAME", "RAM_USER_ID", dv)
            Dim lstItem As New ListItem
            lstItem.Value = ""
            lstItem.Text = "---Select---"
            cboAO.Items.Insert(0, lstItem)
        Else
            tdAO.Style("display") = "none"
            tdAO.Style("VISIBILITY") = "hidden"
        End If
        objPR = Nothing
    End Sub
    Public Sub dtgPOList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgPOList.PageIndexChanged
        dtgPOList.CurrentPageIndex = e.NewPageIndex
        Bindgrid(True)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgPOList.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String

        Dim objPO As New PurchaseOrder_AO 'PurchaseOrder
        Dim ds As DataSet
        Dim strReliefOn As String
        strReliefOn = cboAO.SelectedValue
        ds = objPO.getPOListForApproval(txtPONo.Text, txtVendor.Text, txtDateFr.Text, txtDateTo.Text, strReliefOn, , POStatus_new.Submitted & "," & POStatus_new.PendingApproval & "," & POStatus_new.HeldBy, "", "", "strIncludeHold")

        '//for sorting asc or desc
        Dim dvViewPO As DataView
        dvViewPO = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewPO.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewPO.Sort += " DESC"
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgPOList, dvViewPO)
            cmdReset.Disabled = False
            Dim objApp As New ApprWorkFlow
            If objApp.checkMassApp(Session("UserID")) = 1 Then
                cmdMassApp.Enabled = True
            Else
                cmdMassApp.Enabled = False
                cmdMassApp.Visible = False
            End If
            'cmdMassApp.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection');")
            dtgPOList.DataSource = dvViewPO
            dtgPOList.DataBind()
        Else
            cmdReset.Disabled = True
            cmdMassApp.Enabled = False
            dtgPOList.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If
        ViewState("PageCount") = dtgPOList.PageCount
    End Function



    Private Sub dtgPOList_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPOList.ItemCreated
        '//this line must be included
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgPOList, e)
        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgPOList.CurrentPageIndex = 0
        ViewState("SortAscending") = "yes"
        ViewState("SortExpression") = "POM_SUBMIT_DATE"
        Bindgrid(True)
    End Sub

    Private Sub dtgPOList_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPOList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim chk As CheckBox
            chk = e.Item.Cells(EnumPO.icCheckBox).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
            '//to dynamic build hyperlink
            Dim lnkPONo As HyperLink
            lnkPONo = e.Item.Cells(EnumPO.icPONoLink).FindControl("lnkPoNo")
            lnkPONo.NavigateUrl = dDispatcher.direct("PO", "POApprDetail.aspx", "caller=approval&AO=" & strAO & "&relief=" & blnRelief & "&PageID=" & strPageId & "&index=" & dv("POM_PO_Index") & "&PONo=" & dv("POM_PO_No"))
            lnkPONo.Text = dv("POM_PO_No")
            e.Item.Cells(EnumPO.icSubmitDate).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("POM_SUBMIT_DATE"))
            e.Item.Cells(EnumPO.icVendor).Text = "<a href='" & dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "pageid=" & strPageId & "&v_com_id=" & Common.parseNull(dv("POM_S_Coy_ID"))) & "'>" & e.Item.Cells(EnumPO.icVendor).Text & "</a>"
            e.Item.Cells(EnumPO.icAmt).Text = Format(dv("PO_AMT"), "#,##0.00")

            If Common.parseNull(dv("POM_URGENT")) = "1" Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(EnumPO.icPONoLink).Controls.Add(imgAttach)
            End If

            Dim objPR As New PurchaseReq2
            If objPR.HasAttachment(dv("POM_PO_No"), "PO") Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/images", "clip_icon.gif")
                e.Item.Cells(EnumPO.icPONoLink).Controls.Add(imgAttach)
            End If

            If Not IsDBNull(dv("POM_RFQ_INDEX")) AndAlso CStr(dv("POM_RFQ_INDEX")) <> "" Then
                Dim lnkRFQ As New HyperLink
                Dim strRFQNo, strRFQName As String
                objPR.getRFQName(dv("POM_RFQ_INDEX"), strRFQNo, strRFQName)
                lnkRFQ.ImageUrl = dDispatcher.direct("Plugins/Images", "Q-3 Icon (10x10).jpg")
                lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Frm=SearchPO_AO&pageid=" & strPageId & "&side=other&RFQ_Num=" & strRFQNo & "&RFQ_ID=" & dv("POM_RFQ_INDEX") & "&RFQ_Name=" & Server.UrlEncode(strRFQName))
                'lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "viewQoute.aspx", "Frm=SearchPO_AO&pageid=" & strPageId & "&RFQ_ID=" & dv("POM_RFQ_INDEX") & "&RFQ_Num=" & strRFQNo & "&vcomid=" & Common.parseNull(dv("POM_S_Coy_ID")) & "&side=quote")
                lnkRFQ.ToolTip = "Click here to view quotation comparison"
                e.Item.Cells(EnumPO.icPONoLink).Controls.Add(lnkRFQ)
            End If
            objPR = Nothing

            e.Item.Cells(EnumPO.icStatus).Text = dv("STATUS_DESC")

            If dv("STATUS_DESC") = "Held By" Then
                e.Item.Cells(EnumPO.icStatus).Text = "Held By " & Common.parseNull(dv("NAME"))
            End If


            Dim objPO As New PurchaseOrder
            Dim objDB As New EAD.DBCom
            'Get PR Number for PO converted from PR
            Dim COUNT As Integer
            Dim ARRAY(100) As String
            Dim STR As String
            Dim i As Integer
            Dim strPR_Index, strPRD_RFQ_Index, strRFQ_No As String
            Dim lnkPRNo As HyperLink
            lnkPRNo = e.Item.Cells(EnumPO.icPRNo).FindControl("lnkPRNo")

            'Session("urlrefererPO") = "PRAll"
            strPRD_RFQ_Index = objDB.GetVal("SELECT IFNULL(POM_RFQ_INDEX,'') AS POM_RFQ_INDEX FROM po_mstr WHERE pom_po_no = '" & dv("POM_PO_No") & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")

            If strPRD_RFQ_Index <> "" Then
                strRFQ_No = objDB.GetVal("SELECT IFNULL(RM_RFQ_NO,'') AS RM_RFQ_NO  FROM rfq_mstr WHERE RM_RFQ_ID = '" & strPRD_RFQ_Index & "' AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")

                objPO.GetPRNoAllRFQ(strRFQ_No, ARRAY, COUNT)
                'objPO.GetPRNo1(dv("POM_PO_INDEX"), ARRAY, COUNT)

                If ARRAY(0) <> "" Then
                    For i = 0 To COUNT - 1
                        'lnkPRNo.Text = ARRAY(i)
                        strPR_Index = objDB.GetVal("SELECT PRM_PR_INDEX FROM PR_MSTR WHERE PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRM_PR_NO = '" & ARRAY(i) & "'")
                        If strPR_Index <> "" Then
                            'STR = STR & "<A>" & ARRAY(i) & "</A><br>"
                            'STR = STR & "<A href=""#"" onclick=" & dDispatcher.direct("PR", "PRDetail.aspx", "pageid=" & strPageId & "&index=" & strPR_Index & "&PRNO=" & ARRAY(i) & "&type=mod&mode=bc") & "><font color=#0000ff>" & ARRAY(i) & "</font></A><br/>"
                            'lnkPRNo.NavigateUrl = dDispatcher.direct("PR", "PRDetail.aspx", "pageid=" & strPageId & "&index=" & strPR_Index & "&PRNO=" & ARRAY(i) & "&type=mod&mode=bc") & "')"
                            STR = STR & "<A href=""" & dDispatcher.direct("PR", "PRDetail.aspx", "caller=SearchPO_AO&pageid=" & strPageId & "&index=" & strPR_Index & "&PRNO=" & ARRAY(i) & "&type=mod&mode=bc") & """ ><font color=#0000ff>" & ARRAY(i) & "</font></A><br/>"
                        End If
                    Next
                End If
            Else
                objPO.GetPRNoAll(dv("POM_PO_No"), ARRAY, COUNT)
                'objPO.GetPRNo1(dv("POM_PO_INDEX"), ARRAY, COUNT)

                If ARRAY(0) <> "" Then
                    For i = 0 To COUNT - 1
                        'lnkPRNo.Text = ARRAY(i)
                        strPR_Index = objDB.GetVal("SELECT PRM_PR_INDEX FROM PR_MSTR WHERE PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRM_PR_NO = '" & ARRAY(i) & "'")
                        If strPR_Index <> "" Then
                            'STR = STR & "<A>" & ARRAY(i) & "</A><br>"
                            'STR = STR & "<A href=""#"" onclick=" & dDispatcher.direct("PR", "PRDetail.aspx", "pageid=" & strPageId & "&index=" & strPR_Index & "&PRNO=" & ARRAY(i) & "&type=mod&mode=bc") & "><font color=#0000ff>" & ARRAY(i) & "</font></A><br/>"
                            'lnkPRNo.NavigateUrl = dDispatcher.direct("PR", "PRDetail.aspx", "pageid=" & strPageId & "&index=" & strPR_Index & "&PRNO=" & ARRAY(i) & "&type=mod&mode=bc") & "')"
                            STR = STR & "<A href=""" & dDispatcher.direct("PR", "PRDetail.aspx", "caller=SearchPO_AO&pageid=" & strPageId & "&index=" & strPR_Index & "&PRNO=" & ARRAY(i) & "&type=mod&mode=bc") & """ ><font color=#0000ff>" & ARRAY(i) & "</font></A><br/>"
                        End If
                    Next
                End If
            End If
            e.Item.Cells(EnumPO.icPRNo).Text = STR
        End If
    End Sub
    Private Sub cboAO_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboAO.SelectedIndexChanged
        cmdSearch_Click(sender, e)
    End Sub

    Private Sub cmdMassApp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdMassApp.Click
        Dim strAryPO(0), strMsg(0) As String
        Dim strAryPOIndex(0), strAO As String
        Dim objPO As New PurchaseOrder_AO 'PurchaseOrder
        Dim dgItem As DataGridItem
        Dim blnRelief As Boolean
        For Each dgItem In dtgPOList.Items
            Dim chkSel As CheckBox
            chkSel = dgItem.Cells(EnumPO.icCheckBox).FindControl("chkSelection")
            If chkSel.Checked Then
                Common.Insert2Ary(strAryPO, dgItem.Cells(EnumPO.icPONo).Text)
                Common.Insert2Ary(strAryPOIndex, dtgPOList.DataKeys(dgItem.ItemIndex))
            End If
        Next
        If cboAO.SelectedIndex <= 0 Then
            strAO = Session("UserId")
            blnRelief = False
        Else
            strAO = cboAO.SelectedValue
            blnRelief = True
        End If

        objPO.MassApprovalPO(strAryPO, strAryPOIndex, strAO, strMsg, blnRelief)
        If strMsg.Length > 0 Then
            Dim intLoop, intCnt As Integer
            Dim strMsg1 As String
            intCnt = strMsg.Length
            For intLoop = 0 To intCnt - 1
                If intLoop = 0 Then
                    strMsg1 = strMsg(intLoop)
                Else
                    strMsg1 = strMsg1 & """& vbCrLf & """ & strMsg(intLoop)
                End If
            Next
            Common.NetMsgbox(Me, strMsg1)
        End If
        Bindgrid()
    End Sub
    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_SearchPOAO_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PO", "SearchPO_AO.aspx", "pageid=" & strPageId) & """><span>Approval List</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PO", "SearchPO_ALL.aspx", "pageid=" & strPageId) & """><span>Approved / Rejected Listing</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "</ul><div></div></div>"


    End Sub

End Class


