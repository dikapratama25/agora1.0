'//Outstanding
'//RFQ Ind
'//Attachment Ind

Imports AgoraLegacy
Imports eProcure.Component
Public Class SearchPR_AOFTN
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim strCaller As String
    Public Enum EnumPR
        icCheckBox
        icPRNoLink
        icPRType
        icCreationDate
        icBuyer
        icBuyerDept
        'icVendor
        'icCurrency
        'icAmt
        'icStatus
        icPRNo
        icBuyerID
    End Enum
    Dim strAO As String
    'Protected WithEvents lblVendor As System.Web.UI.WebControls.Label
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
        CheckButtonAccess(True)
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here

        strCaller = UCase(Request.QueryString("caller"))
        'strCaller = Session("strCaller")
        SetGridProperty(dtgPRList)

        MyBase.Page_Load(sender, e)

        '//because No DataGrid display when page first loaded
        cmdReset.Disabled = True
        cmdMassApp.Enabled = False


        If cboAO.SelectedIndex <= 0 Then
            strAO = Session("UserID")
            blnRelief = False
        Else
            strAO = cboAO.SelectedValue
            blnRelief = True
        End If

        '' Michelle (CR0004) - To hide search by 'Vendor Name' if the 'PR To Multiple POs' is on
        'Dim strMultiPO As String
        'Dim strSql1 As String

        'Dim objDb As New EAD.DBCom

        'strSql1 = "Select  CM_MULTI_PO FROM COMPANY_MSTR where CM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "'"
        'Dim tDS As DataSet = objDb.FillDs(strSql1)

        'If tDS.Tables(0).Rows.Count > 0 Then
        '    strMultiPO = tDS.Tables(0).Rows(0).Item("CM_MULTI_PO")
        'End If
        'If strMultiPO = "Y" Then
        '    lblVendor.Visible = False
        '    txtVendor.Visible = False
        'Else
        '    lblVendor.Visible = True
        '    txtVendor.Visible = True
        'End If

        If Not Page.IsPostBack Then
            GenerateTab()
            Session("strurl") = Me.strCallFrom ' (gary add) url back from quote compare
            'getReliefList()   no relief for FTN
            tdAO.Style("display") = "none"
            tdAO.Style("VISIBILITY") = "hidden"
            If cboAO.SelectedIndex < 0 Then
                cmdSearch_Click(sender, e)
            End If
        End If

        'Check Access Status
        If Session("AccessStatus") = "Limited" Then
            cmdMassApp.Enabled = False
        End If

        Session("urlreferer") = "SearchPRAO"
    End Sub

    Private Sub getReliefList()
        Dim dv As DataView
        Dim objPR As New PurchaseReq2
        dv = objPR.getReliefList("PR")
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

    Public Sub dtgPRList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgPRList.PageIndexChanged
        dtgPRList.CurrentPageIndex = e.NewPageIndex
        Bindgrid(True)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgPRList.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objPR As New PurchaseReq2
        Dim ds As DataSet
        Dim strPRType As String = ""

        If chkContPR.Checked = True And chkNonContPR.Checked = True Then
            strPRType = ""
        ElseIf chkContPR.Checked = True Then
            strPRType = "CC"
        ElseIf chkNonContPR.Checked = True Then
            strPRType = "NonCont"
        Else
            strPRType = ""
        End If

        Dim strReliefOn As String
        strReliefOn = cboAO.SelectedValue
        ds = objPR.getPRListForApproval(txtPRNo.Text, "", txtDateFr.Text, txtDateTo.Text, strReliefOn, , "", "", strPRType)

        '//for sorting asc or desc
        Dim dvViewPR As DataView
        dvViewPR = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewPR.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewPR.Sort += " DESC"
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgPRList, dvViewPR)
            cmdReset.Disabled = False
            Dim objApp As New ApprWorkFlow
            If objApp.checkMassApp(Session("UserID")) = 1 Then
                cmdMassApp.Enabled = True
            Else
                cmdMassApp.Enabled = False
                cmdMassApp.Visible = False
            End If
            'cmdMassApp.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection');")
            dtgPRList.DataSource = dvViewPR
            dtgPRList.DataBind()
        Else
            cmdReset.Disabled = True
            cmdMassApp.Enabled = False
            dtgPRList.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If
        ViewState("PageCount") = dtgPRList.PageCount
    End Function

    Private Sub dtgPRList_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPRList.ItemCreated
        '//this line must be included
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgPRList, e)
        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgPRList.CurrentPageIndex = 0
        ViewState("SortAscending") = "no"
        ViewState("SortExpression") = "PRM_SUBMIT_DATE"
        Bindgrid(True)
    End Sub

    Private Sub dtgPRList_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPRList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            ' To add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(EnumPR.icCheckBox).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
            ' To dynamic build hyperlink
            Dim lnkPRNo As HyperLink
            lnkPRNo = e.Item.Cells(EnumPR.icPRNoLink).FindControl("lnkPRNo")
            lnkPRNo.NavigateUrl = dDispatcher.direct("PR", "PRApprDetail.aspx", "caller=approval&AO=" & strAO & "&relief=" & blnRelief & "&PageID=" & strPageId & "&index=" & dv("PRM_PR_Index") & "&PRNo=" & dv("PRM_PR_No"))
            lnkPRNo.Text = dv("PRM_PR_No")
            e.Item.Cells(EnumPR.icCreationDate).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("PRM_SUBMIT_DATE"))

            If Not IsDBNull(dv("PRM_PR_TYPE")) Then
                If dv("PRM_PR_TYPE") = "CC" Then
                    e.Item.Cells(EnumPR.icPRType).Text = "Contract"

                Else
                    e.Item.Cells(EnumPR.icPRType).Text = "Non-Contract"

                End If
            Else
                e.Item.Cells(EnumPR.icPRType).Text = "Non-Contract"
            End If
            ''Michelle (23/7/2007) - To cater for multiple vendors
            'If e.Item.Cells(EnumPR.icVendor).Text <> "Multiple Vendors" Then
            '    e.Item.Cells(EnumPR.icVendor).Text = "<a href='../RFQ/RFQ_VendorDetail.aspx?pageid=" & strPageId & "&v_com_id=" & Common.parseNull(dv("PRM_S_Coy_ID")) & "'>" & e.Item.Cells(EnumPR.icVendor).Text & "</a>"
            'Else
            '    e.Item.Cells(EnumPR.icVendor).Text = "<a href='../PR/Multi_VendorDetails.aspx?caller=AO&pageid=" & strPageId & "&PRNum=" & Common.parseNull(dv("PRM_PR_NO")) & "'>" & e.Item.Cells(EnumPR.icVendor).Text & "</A>"
            'End If

            ''PRM_S_Coy_ID
            'e.Item.Cells(EnumPR.icAmt).Text = Format(dv("PR_AMT"), "#,##0.00")

            ''If Common.parseNull(dv("PRM_PR_STATUS")) = PRStatus.CancelledBy Then
            ''e.Item.Cells(EnumPR.icStatus).Text = e.Item.Cells(EnumPR.icStatus).Text & Space(4) & "<a href='addDept.aspx'>" & "(PO #" & Common.parseNull(dv("PO_NO")) & ")</a>"
            ''End If
            'If Common.parseNull(dv("PRM_PR_STATUS")) = PRStatus.CancelledBy Or Common.parseNull(dv("PRM_PR_STATUS")) = PRStatus.HeldBy Then
            '    If IsDBNull(dv("CHANGED_BY_NAME")) Then
            '        e.Item.Cells(EnumPR.icStatus).Text = e.Item.Cells(EnumPR.icStatus).Text & " (" & Common.parseNull(dv("PRM_STATUS_CHANGED_BY")) & ")"
            '    Else
            '        e.Item.Cells(EnumPR.icStatus).Text = e.Item.Cells(EnumPR.icStatus).Text & " (" & dv("CHANGED_BY_NAME") & ")"
            '    End If
            'End If
            '' dblTotal = dblTotal + dv("PR_AMT")

            If Common.parseNull(dv("PRM_URGENT")) = "1" Then
                Dim lnkUrgent As New HyperLink
                Dim strRFQNo, strRFQName As String
                'objPR.getRFQName(dv("PRM_RFQ_INDEX"), strRFQNo, strRFQName)
                lnkUrgent.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                'lnkUrgent.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Frm=SearchPR_AO&pageid=" & strPageId & "&side=other&RFQ_Num=" & strRFQNo & "&RFQ_ID=" & dv("PRM_RFQ_INDEX") & "&RFQ_Name=" & Server.UrlEncode(strRFQName))
                'lnkUrgent.ToolTip = "Click here to view purchase request"
                e.Item.Cells(EnumPR.icPRNoLink).Controls.Add(lnkUrgent)
            End If

            Dim objPR As New PurchaseReq2
            If objPR.HasAttachment(dv("PRM_PR_No")) Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                'imgAttach.ImageUrl = "../Images/clip_icon.gif"
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/images", "clip_icon.gif")
                e.Item.Cells(EnumPR.icPRNoLink).Controls.Add(imgAttach)
            End If

            'If Not IsDBNull(dv("PRM_RFQ_INDEX")) AndAlso CStr(dv("PRM_RFQ_INDEX")) <> "" Then
            '    'Dim lblC As New Label
            '    'lblC.Text = "<a href='aaa.aspx'><img src=../images/INFO.GIF border=0></a>"
            '    'e.Item.Cells(EnumPR.icPRNo).Controls.Add(lblC)
            '    Dim lnkRFQ As New HyperLink
            '    Dim strRFQNo, strRFQName As String
            '    objPR.getRFQName(dv("PRM_RFQ_INDEX"), strRFQNo, strRFQName)
            '    lnkRFQ.ImageUrl = "../Images/i_quotation2.gif"
            '    lnkRFQ.NavigateUrl = "../RFQ/RFQComSummary.aspx?pageid=" & strPageId & "&side=other&RFQ_Num=" & strRFQNo & "&RFQ_ID=" & dv("PRM_RFQ_INDEX") & "&RFQ_Name=" & Server.UrlEncode(strRFQName)
            '    lnkRFQ.ToolTip = "Click here to view quotation comparison"
            '    e.Item.Cells(EnumPR.icPRNoLink).Controls.Add(lnkRFQ)
            'End If
            objPR = Nothing
        End If
    End Sub

    Private Sub cboAO_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboAO.SelectedIndexChanged
        cmdSearch_Click(sender, e)
    End Sub

    Private Sub cmdMassApp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdMassApp.Click
        Dim strAryPR(0), strMsg(0) As String
        Dim strAryPRIndex(0), strAO As String
        Dim objPR As New PurchaseReq2
        Dim dgItem As DataGridItem
        Dim blnRelief As Boolean
        Dim chk As Boolean
        Dim PR_Type As String

        For Each dgItem In dtgPRList.Items
            Dim chkSel As CheckBox
            chkSel = dgItem.Cells(EnumPR.icCheckBox).FindControl("chkSelection")
            If chkSel.Checked Then
                If dgItem.Cells(EnumPR.icPRType).Text <> "Contract" Then
                    chk = objPR.ValidatePOUserAssign(dgItem.Cells(EnumPR.icPRNo).Text)
                Else
                    chk = True
                End If

                'If objPR.ValidatePOUserAssign(dgItem.Cells(EnumPR.icPRNo).Text) Then
                If chk = True Then
                    Common.Insert2Ary(strAryPR, dgItem.Cells(EnumPR.icPRNo).Text)
                    Common.Insert2Ary(strAryPRIndex, dtgPRList.DataKeys(dgItem.ItemIndex))
                Else
                    Common.NetMsgbox(Me, "No Commodity Type assign to Purchase Officer.")
                    cmdMassApp.Enabled = True
                    Exit Sub
                End If
                'End If
            End If
        Next
        If cboAO.SelectedIndex <= 0 Then
            strAO = Session("UserId")
            blnRelief = False
        Else
            strAO = cboAO.SelectedValue
            blnRelief = True
        End If

        objPR.MassApproval(strAryPR, strAryPRIndex, strAO, strMsg, blnRelief)
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
        Session("w_SearchPRAO_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PR", "SearchPR_AO.aspx", "pageid=" & strPageId) & """><span>Approval List</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PR", "SearchApp_All.aspx", "pageid=" & strPageId) & """><span>Approved / Rejected Listing</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "</ul><div></div></div>"
    End Sub
End Class


