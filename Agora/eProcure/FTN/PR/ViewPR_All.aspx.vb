'//Attachment Ind
Imports AgoraLegacy
Imports eProcure.Component
Public Class ViewPR_AllFTN
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    '   Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    '   Protected WithEvents txtPRNo As System.Web.UI.WebControls.TextBox
    '   Protected WithEvents txtVendor As System.Web.UI.WebControls.TextBox
    '   Protected WithEvents txtDateFr As System.Web.UI.WebControls.TextBox
    '   Protected WithEvents txtDateTo As System.Web.UI.WebControls.TextBox
    '   Protected WithEvents txtBuyer As System.Web.UI.WebControls.TextBox
    '   Protected WithEvents chkOpen As System.Web.UI.WebControls.CheckBox
    '   Protected WithEvents chkSubmitted As System.Web.UI.WebControls.CheckBox
    '   Protected WithEvents chkPendingAppr As System.Web.UI.WebControls.CheckBox
    '   Protected WithEvents chkApproved As System.Web.UI.WebControls.CheckBox
    '   Protected WithEvents chkPOCreate As System.Web.UI.WebControls.CheckBox
    '   Protected WithEvents chkCancel As System.Web.UI.WebControls.CheckBox
    '   Protected WithEvents chkHold As System.Web.UI.WebControls.CheckBox
    '   Protected WithEvents chkReject As System.Web.UI.WebControls.CheckBox
    '   Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    '   Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    '   Protected WithEvents vldDateFtDateTo As System.Web.UI.WebControls.CompareValidator
    '   Protected WithEvents vldDateFr As System.Web.UI.WebControls.CustomValidator
    '   Protected WithEvents vldDateTo As System.Web.UI.WebControls.CustomValidator
    '   Protected WithEvents dtgPRList As System.Web.UI.WebControls.DataGrid
    '   Protected WithEvents trAdmin As System.Web.UI.HtmlControls.HtmlTableRow
    '   Protected WithEvents Table2 As System.Web.UI.HtmlControls.HtmlTable
    '   Protected WithEvents tblSearchResult As System.Web.UI.HtmlControls.HtmlTable
    '   Protected WithEvents lblVendor As System.Web.UI.WebControls.Label
    'Protected WithEvents lblStartDate As System.Web.UI.WebControls.Label
    'Protected WithEvents lblEndDate As System.Web.UI.WebControls.Label


    Public Enum EnumPR
        icPRNo
        icCreationDate
        icBuyer
        icBuyerDept
        'icVendor
        'icCurrency
        'icAmt
        icPRType
        icStatus
        icPRNO1
    End Enum
    Dim strCaller As String
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
    'Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
    '    CheckButtonAccess(True)
    'End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        blnCheckBox = False
        SetGridProperty(dtgPRList)
        ' strNewCSS = "true"
        MyBase.Page_Load(sender, e)

        strCaller = UCase(Request.QueryString("caller"))
        If strCaller = "ADMIN" Then
            lblTitle.Text = "View All PR"
            chkOpen.Enabled = False
            'chkAwaitAppr.Enabled = True
            'trAdmin.Style("display") = ""
            'dtgPRList.Columns(EnumPR.icBuyer).HeaderText = "Requisitioner"
        End If

        ' Michelle (CR0004) - To hide search by 'Vendor Name' if the 'PR To Multiple POs' is on
        Dim strMultiPO As String
        Dim strSql1 As String
        Dim objDb As New EAD.DBCom

        strSql1 = "Select  CM_MULTI_PO FROM COMPANY_MSTR where CM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "'"
        Dim tDS As DataSet = objDb.FillDs(strSql1)
        If tDS.Tables(0).Rows.Count > 0 Then
            strMultiPO = tDS.Tables(0).Rows(0).Item("CM_MULTI_PO")
        End If
        'If strMultiPO = "Y" Then
        '    lblVendor.Enabled = False
        '    txtVendor.Enabled = False
        'Else
        '    lblVendor.Enabled = True
        '    txtVendor.Enabled = True
        'End If
        strPageId = Request.QueryString("pageid")
        If Not Page.IsPostBack Then
            Session("strURL") = strCallFrom
        End If
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

        '//Retrieve Data from Database
        Dim ds As DataSet

        Dim strStatus As String = ""
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

        chk_condition(strStatus)
        ds = objPR.SearchPRListAll(txtPRNo.Text, txtDateFr.Text, txtDateTo.Text, "ADMIN", strStatus, strPRType, txtBuyer.Text, txtDept.Text)
        '//for sorting asc or desc
        Dim dvViewPR As DataView
        dvViewPR = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewPR.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewPR.Sort += " DESC"
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgPRList, dvViewPR)
            dtgPRList.DataSource = dvViewPR
            dtgPRList.DataBind()
        Else
            'dtgDept.DataSource = ""
            dtgPRList.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
            'intTotPage = 0
        End If
        ViewState("PageCount") = dtgPRList.PageCount
    End Function



    Private Sub dtgPRList_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPRList.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dtgPRList, e)
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
            Dim strURL As String
            '//to add JavaScript to Check Box
            '//to dynamic build hyperlink
            Dim lnkPRNo As HyperLink
            Dim intStatus As String

            intStatus = Common.parseNull(dv("PRM_PR_STATUS"))
            lnkPRNo = e.Item.Cells(EnumPR.icPRNo).FindControl("lnkPRNo")


            If strCaller <> "BUYER" Then
                lnkPRNo.NavigateUrl = dDispatcher.direct("PR", "PRDetail.aspx", "pageid=" & strPageId & "&index=" & dv("PRM_PR_Index") & "&PRNO=" & dv("PRM_PR_No") & "&caller=ADMIN")

                'Else
                '    If intStatus = PRStatus.Draft Then
                '        lnkPRNo.NavigateUrl = dDispatcher.direct("PR", "RaisePR.aspx", "pageid=" & strPageId & "&index=" & dv("PRM_PR_Index") & "&prid=" & dv("PRM_PR_No") & "&type=list")

                '    Else
                '        lnkPRNo.NavigateUrl = dDispatcher.direct("PR", "PRDetail.aspx", "pageid=" & strPageId & "&index=" & dv("PRM_PR_Index") & "&PRNO=" & dv("PRM_PR_No"))

                '    End If
            End If
            lnkPRNo.Text = dv("PRM_PR_No")

            If Common.parseNull(dv("PRM_URGENT")) = "1" Then
                Dim lnkUrgent As New HyperLink
                lnkUrgent.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(EnumPR.icPRNo).Controls.Add(lnkUrgent)
            End If

            Dim objPR As New PurchaseReq2
            If objPR.HasAttachment(dv("PRM_PR_No")) Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "clip_icon.gif")
                e.Item.Cells(EnumPR.icPRNo).Controls.Add(imgAttach)
            End If

            '<A href=""RFQComSummary.aspx?RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&RFQ_Name=" & Common.parseNull(dv("RM_RFQ_Name")) & """ ><font color=#0000ff>View Responsed</font></A>, Expired"
            If Not IsDBNull(dv("PRM_RFQ_INDEX")) AndAlso CStr(dv("PRM_RFQ_INDEX")) <> "" Then
                'Dim lblC As New Label
                'lblC.Text = "<a href='aaa.aspx'><img src=../images/INFO.GIF border=0></a>"
                'e.Item.Cells(EnumPR.icPRNo).Controls.Add(lblC)
                Dim lnkRFQ As New HyperLink
                Dim strRFQNo, strRFQName As String
                objPR.getRFQName(dv("PRM_RFQ_INDEX"), strRFQNo, strRFQName)
                lnkRFQ.ImageUrl = dDispatcher.direct("Plugins/Images", "i_quotation2.gif")
                lnkRFQ.NavigateUrl = "../../Common/RFQ/RFQComSummary.aspx?Frm=ViewPR_All&pageid=" & strPageId & "&RFQ_Num=" & strRFQNo & "&RFQ_ID=" & dv("PRM_RFQ_INDEX") & "&RFQ_Name=" & Server.UrlEncode(strRFQName) & "&side=other"
                lnkRFQ.ToolTip = "Click here to view quotation comparison"
                e.Item.Cells(EnumPR.icPRNo).Controls.Add(lnkRFQ)
                Session("strurl") = strCallFrom
            End If
            objPR = Nothing

            e.Item.Cells(EnumPR.icCreationDate).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("PRM_SUBMIT_DATE"))

            'If intStatus = PRStatus.ConvertedToPO Then


            '    '-----------Added Name to Url By Praveen on 23/07/2007 and on 01/08/2007(Addesd Caller)
            '    e.Item.Cells(EnumPR.icStatus).Text = e.Item.Cells(EnumPR.icStatus).Text & Space(4) & _

            '    '               "<a href='../../Common/PO/POViewB3.aspx?caller=" & strCaller & "&pageid=" & strPageId & "&PO_INDEX=" & Common.parseNull(dv("PRM_PO_INDEX")) & "&prid=" & dv("PRM_PR_No") & "&PO_NO=" & Common.parseNull(dv("PO_NO")) & "&BCoyID=" & Session("CompanyID") & "&side=other&filetype=2'>" & "(Multiple POs)<font color=#0000ff></font></a>"



            'End If

            'PRM_STATUS_CHANGED_BY()
            If Not IsDBNull(dv("PRM_PR_TYPE")) Then
                If dv("PRM_PR_TYPE") = "CC" Then
                    e.Item.Cells(EnumPR.icPRType).Text = "Contract"

                Else
                    e.Item.Cells(EnumPR.icPRType).Text = "Non-Contract"

                End If
            Else
                e.Item.Cells(EnumPR.icPRType).Text = "Non-Contract"
            End If
            'If intStatus = PRStatus.CancelledBy Or intStatus = PRStatus.RejectedBy Or intStatus = PRStatus.HeldBy Then
            If intStatus = PRStatus.CancelledBy Or intStatus = PRStatus.RejectedBy Then
                If IsDBNull(dv("CHANGED_BY_NAME")) Then
                    e.Item.Cells(EnumPR.icStatus).Text = e.Item.Cells(EnumPR.icStatus).Text & " (" & Common.parseNull(dv("PRM_STATUS_CHANGED_BY")) & ")"
                Else
                    e.Item.Cells(EnumPR.icStatus).Text = e.Item.Cells(EnumPR.icStatus).Text & " (" & dv("CHANGED_BY_NAME") & ")"
                End If
            End If
            If (intStatus = PRStatus.HeldBy Or intStatus = PRStatus.PendingApproval) Then e.Item.Cells(EnumPR.icStatus).Text = "Submitted"

            If (intStatus = PRStatus.Void) Then
                e.Item.Cells(EnumPR.icStatus).Text = "Void"
                lnkPRNo.NavigateUrl = dDispatcher.direct("PR", "PRDetail.aspx", "pageid=" & strPageId & "&index=" & dv("PRM_PR_Index") & "&PRNO=" & dv("PRM_PR_No") & "&caller=ADMIN&status=" & e.Item.Cells(EnumPR.icStatus).Text & "")
            End If
            'dblTotal = dblTotal + dv("PR_AMT")
        End If
    End Sub
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Sub chk_condition(ByRef strStatus As String)
        If chkOpen.Checked = True Then
            strStatus = IIf(strStatus = "", PRStatus.Draft, strStatus & "," & PRStatus.Draft)
        End If
        If chkSubmitted.Checked Then
            strStatus = IIf(strStatus = "", PRStatus.Submitted & "," & PRStatus.PendingApproval, strStatus & "," & PRStatus.Submitted & "," & PRStatus.PendingApproval & "," & PRStatus.HeldBy)
        End If
        If chkApproved.Checked Then
            strStatus = IIf(strStatus = "", PRStatus.Approved, strStatus & "," & PRStatus.Approved)
        End If
        If chkConToPO.Checked Then
            strStatus = IIf(strStatus = "", PRStatus.ConvertedToPO, strStatus & "," & PRStatus.ConvertedToPO)
        End If
        If chkVoid.Checked Then
            strStatus = IIf(strStatus = "", PRStatus.Void, strStatus & "," & PRStatus.Void)
        End If
        If chkCancel.Checked Then
            strStatus = IIf(strStatus = "", PRStatus.CancelledBy, strStatus & "," & PRStatus.CancelledBy)
        End If
        If chkReject.Checked Then
            strStatus = IIf(strStatus = "", PRStatus.RejectedBy, strStatus & "," & PRStatus.RejectedBy)
        End If
        If strStatus = "" Then
            strStatus = PRStatus.Draft & "," & _
             PRStatus.Submitted & "," & _
             PRStatus.PendingApproval & "," & _
             PRStatus.Approved & "," & _
             PRStatus.ConvertedToPO & "," & _
             PRStatus.Void & "," & _
             PRStatus.CancelledBy & "," & _
             PRStatus.HeldBy & "," & _
             PRStatus.RejectedBy

        End If
    End Sub
End Class


