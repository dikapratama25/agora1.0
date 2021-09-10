'//This page is called by Buyer Company Only
'//QueryString 
'//filetype ==> 1- PO Cancellation, 2- PO
'//side - u-PO, b-View All PO
'Imports Wheel.Components
Imports AgoraLegacy
Imports eProcure.Component

Public Class POViewTrx
    Inherits AgoraLegacy.AppBaseClass

    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDB As New EAD.DBCom

    'Dim blnMsg As Boolean

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents txt_po_no As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txt_vendor As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txt_startdate As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txt_enddate As System.Web.UI.WebControls.TextBox
    'Protected WithEvents CompareValidator1 As System.Web.UI.WebControls.CompareValidator
    ''Protected WithEvents chk_Draft As System.Web.UI.WebControls.CheckBox
    ''Protected WithEvents chk_Submitted As System.Web.UI.WebControls.CheckBox
    ''Protected WithEvents chk_Approved As System.Web.UI.WebControls.CheckBox
    ''Protected WithEvents chk_Void As System.Web.UI.WebControls.CheckBox
    ''Protected WithEvents chk_Accept As System.Web.UI.WebControls.CheckBox
    ''Protected WithEvents chk_Reject As System.Web.UI.WebControls.CheckBox
    ''Protected WithEvents chk_Cancel As System.Web.UI.WebControls.CheckBox
    ''Protected WithEvents chk_close As System.Web.UI.WebControls.CheckBox
    'Protected WithEvents ChK_open2 As System.Web.UI.WebControls.CheckBox
    'Protected WithEvents chk_part2 As System.Web.UI.WebControls.CheckBox
    'Protected WithEvents chk_fully2 As System.Web.UI.WebControls.CheckBox
    ''Protected WithEvents chk_cancelorder2 As System.Web.UI.WebControls.CheckBox
    ''Protected WithEvents chk_pending2 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cmdClear As System.Web.UI.WebControls.Button
    Protected WithEvents cboPOStatus As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents Table2 As System.Web.UI.HtmlControls.HtmlTable
    'Protected WithEvents dtg_POList As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents tblSearchResult As System.Web.UI.HtmlControls.HtmlTable
    ''Protected WithEvents link As System.Web.UI.HtmlControls.HtmlAnchor
    ''Protected WithEvents hidlink As System.Web.UI.HtmlControls.HtmlGenericControl


    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    Dim objGlobal As New AppGlobals

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Public Enum EnumCRView
        icPONo = 0
        icCreateDate = 1
        icPODate = 2 '1
        icComName = 3 '2
        icAcceptDate = 4 '3
        icCreatedBy = 5
        icStatus = 6 '4
        'icFulfilment = 5
        icVIndex = 7
        icVComID = 8
        icPRNo = 9
    End Enum

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CheckButtonAccess(True)
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        blnCheckBox = False
        SetGridProperty(dtg_POList)
        If Not Page.IsPostBack Then
            'If Request(Trim("Type")) = "MyPO" Then
            'If Request(Trim("filetype")) = "1" Then
            ''chk_close.Visible = False
            'chk_Cancel.Checked = False
            'chk_Reject.Checked = False
            'chk_fully2.Checked = False
            'chk_pending2.Checked = False
            'chk_cancelorder2.Checked = False

            'Me.chk_close.Enabled = False
            'Me.chk_Cancel.Enabled = False
            'Me.chk_Reject.Enabled = False
            'Me.chk_fully2.Enabled = False
            '' Me.chk_pending2.Enabled = False
            'Me.chk_cancelorder2.Enabled = False

            'Michelle (6/2/2010) - If Buyer Admin logs in, don't display all the records
            'Dim objUsers As New Users
            'Dim IsBA As Boolean = objUsers.BAdminRole(HttpContext.Current.Session("UserId"), HttpContext.Current.Session("CompanyId"))
            'If Not IsBA Then
            '    cmdSearch_Click(sender, e)
            'End If
            'ElseIf Request(Trim("filetype")) = "2" Then
            '    Me.hidlink.Visible = False
            'End If
            'ElseIf Request(Trim("side")) = "u" Then
            '    If Request(Trim("filetype")) = "2" Then
            '        Me.hidlink.Visible = False
            '    End If
            'End If
            'If Buyer Admin logs in, don't display 'draft' status
            Dim objUsers As New Users
            Dim IsBA As Boolean = objUsers.BAdminRole(HttpContext.Current.Session("UserId"), HttpContext.Current.Session("CompanyId"))
            If IsBA Then
                Me.cboPOStatus.Items.RemoveAt(1)
            End If

            txt_startdate.Text = DateAdd("m", -6, DateTime.Now.ToShortDateString())
            txt_enddate.Text = DateTime.Now.ToShortDateString()
        End If

        intPageRecordCnt = ViewState("intPageRecordCnt")
        Session("urlreferer") = "POViewTrx"
        Session("ProdList") = ""
        Session("VendorList") = ""

        '' ''Me.cboPOStatus.Attributes.Add("OnSelectedIndexChanged", "setFulfilStatus();")
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
        '//Retrieve Data from Database
        Dim ds As New DataSet
        Dim strStatus As String = ""
        Dim strFulfilment As String = ""
        Dim count_status As Integer
        Dim objPO As New PurchaseOrder
        Dim strMsg As String

        Dim comparedt As Date
        'comparedt = DateAdd("m", -6, DateTime.Now.ToShortDateString())
        comparedt = DateAdd("m", -6, CDate(txt_enddate.Text))

        If CDate(txt_startdate.Text) < comparedt Then
            strMsg = "Start date " & objGlobal.GetErrorMessage("00015") & " 6 months"
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
            Return False
        End If

        chk_condition(strStatus, strFulfilment, count_status)
        ds = objPO.VIEW_POListTrx(strStatus, strFulfilment, "u", Me.txt_vendor.Text, Me.txt_po_no.Text, Me.txt_startdate.Text, Me.txt_enddate.Text)
        'End If

        Dim dvViewPR As DataView
        dvViewPR = ds.Tables(0).DefaultView

        dvViewPR.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewPR.Sort += " DESC"

        ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count

        If ViewState("intPageRecordCnt") > 0 Then
            resetDatagridPageIndex(dtg_POList, dvViewPR)
            dtg_POList.DataSource = dvViewPR
            dtg_POList.DataBind()
        Else
            dtg_POList.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        ' add for above checking
        ViewState("PageCount") = dtg_POList.PageCount
    End Function

    Sub chk_condition(ByRef strStatus As String, ByRef strFulfilment As String, ByRef count_status As Integer)
        Dim intStatus = cboPOStatus.SelectedValue

        If intStatus = "" Then intStatus = 0
        Select Case intStatus
            Case 1 'Draft
                strStatus = POStatus_new.Draft
                strFulfilment = "0"
            Case 2 'Submitted
                strStatus = POStatus_new.Submitted & "," & POStatus_new.PendingApproval & "," & POStatus_new.HeldBy
                strFulfilment = "0"
            Case 3 'Approved
                strStatus = POStatus_new.Approved & "," & POStatus_new.NewPO & "," & POStatus_new.Open
                strFulfilment = "0"
            Case 4 'Accepted
                strStatus = POStatus_new.Accepted
                '' ''If Not ChK_open2.Checked And Not chk_part2.Checked And Not chk_fully2.Checked Then
                '' ''    strFulfilment = Fulfilment.Fully_Delivered & ", " & Fulfilment.Open & ", " & Fulfilment.Part_Delivered & ", " & Fulfilment.Pending_Cancel_Ack
                '' ''Else
                '' ''    If ChK_open2.Checked Then
                '' ''        strFulfilment = Fulfilment.Open
                '' ''    End If
                '' ''    If chk_part2.Checked Then
                '' ''        strFulfilment = IIf(strFulfilment = "", Fulfilment.Part_Delivered, strFulfilment & "," & Fulfilment.Part_Delivered)
                '' ''    End If
                '' ''    If chk_fully2.Checked Then
                '' ''        strFulfilment = IIf(strFulfilment = "", Fulfilment.Fully_Delivered, strFulfilment & "," & Fulfilment.Fully_Delivered)
                '' ''    End If
                '' ''End If
            Case 5 'Closed
                strStatus = POStatus_new.Close
                strFulfilment = Fulfilment.Fully_Delivered & ", " & Fulfilment.Open & ", " & Fulfilment.Part_Delivered
            Case 6 'Cancelled
                strStatus = POStatus_new.Cancelled & "," & POStatus_new.CancelledBy
                strFulfilment = "0" & ", " & Fulfilment.Cancel_Order & ", " & Fulfilment.Pending_Cancel_Ack & ", " & Fulfilment.Open
            Case 7 'Rejected
                strStatus = POStatus_new.Rejected & "," & POStatus_new.RejectedBy
                strFulfilment = "0" & ", " & Fulfilment.Open
                'Case 8 'Void
                'strStatus = POStatus_new.Void
                'strFulfilment = "0"
            Case 8 'Held
                strStatus = POStatus_new.HeldBy
                strFulfilment = "0"
            Case Else
                Dim objUsers As New Users
                Dim IsBA As Boolean = objUsers.BAdminRole(HttpContext.Current.Session("UserId"), HttpContext.Current.Session("CompanyId"))
                If IsBA = True Then 'Buyer Admin fixed role
                    strStatus = POStatus_new.Accepted & "," & POStatus_new.Approved & "," & POStatus_new.Cancelled & "," & POStatus_new.CancelledBy & "," & POStatus_new.Close & "," & _
                                POStatus_new.HeldBy & "," & POStatus_new.NewPO & "," & POStatus_new.Open & "," & POStatus_new.PendingApproval & "," & POStatus_new.Rejected & "," & _
                                POStatus_new.RejectedBy & "," & POStatus_new.Submitted & "," & POStatus_new.Void & "," & POStatus_new.HeldBy

                Else
                    strStatus = POStatus_new.Accepted & "," & POStatus_new.Approved & "," & POStatus_new.Cancelled & "," & POStatus_new.CancelledBy & "," & POStatus_new.Close & "," & _
                                POStatus_new.HeldBy & "," & POStatus_new.NewPO & "," & POStatus_new.Open & "," & POStatus_new.PendingApproval & "," & POStatus_new.Rejected & "," & _
                                POStatus_new.RejectedBy & "," & POStatus_new.Submitted & "," & POStatus_new.HeldBy

                End If
                strFulfilment = "0" & ", " & Fulfilment.Fully_Delivered & ", " & Fulfilment.Open & ", " & Fulfilment.Part_Delivered & ", " & Fulfilment.Cancel_Order & ", " & Fulfilment.Pending_Cancel_Ack
        End Select
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtg_POList.CurrentPageIndex = 0
        ViewState("SortAscending") = "no"

        If cboPOStatus.SelectedItem.Text = "Draft" Then
            ViewState("SortExpression") = "POM_PO_No"
        ElseIf cboPOStatus.SelectedItem.Text = "Submitted for approval" Then
            ViewState("SortExpression") = "POM_PO_No"
        ElseIf cboPOStatus.SelectedItem.Text = "Void draft PO" Then
            ViewState("SortExpression") = "POM_PO_No"
        Else
            ViewState("SortExpression") = "POM_CREATED_DATE"
        End If


        Bindgrid(0)
    End Sub
    Private Sub dtg_POLis_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_POList.ItemDataBound
        Dim dsRFQ As New DataSet
        Dim objrfq As New RFQ
        Dim strRFQName As String = ""

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim strStatus As String

            e.Item.Cells(EnumCRView.icCreateDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("POM_CREATED_DATE"))
            e.Item.Cells(EnumCRView.icPODate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("POM_PO_DATE"))
            e.Item.Cells(EnumCRView.icAcceptDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("POM_ACCEPTED_DATE"))
            strStatus = e.Item.Cells(EnumCRView.icStatus).Text
            Select Case strStatus
                Case "Draft"
                    e.Item.Cells(EnumCRView.icStatus).Text = "Draft"
                Case "Submitted", "Pending Approval"
                    e.Item.Cells(EnumCRView.icStatus).Text = "Submitted for approval"
                Case "New", "Open", "Approved"
                    e.Item.Cells(EnumCRView.icStatus).Text = "Approved by management"
                Case "Accepted"
                    e.Item.Cells(EnumCRView.icStatus).Text = "Accepted by vendor"
                Case "Closed"
                    e.Item.Cells(EnumCRView.icStatus).Text = "Completed delivery and paid"
                Case "Cancelled", "Cancelled By"
                    e.Item.Cells(EnumCRView.icStatus).Text = "Cancelled by buyer"
                Case "Rejected", "Rejected By"
                    e.Item.Cells(EnumCRView.icStatus).Text = "Rejected by management / vendor"
                Case "Void"
                    e.Item.Cells(EnumCRView.icStatus).Text = "Void draft PO"
                Case "Held By"
                    e.Item.Cells(EnumCRView.icStatus).Text = "Held By " & Common.parseNull(dv("NAME"))
            End Select

            Dim lnkPONo As HyperLink
            lnkPONo = e.Item.Cells(EnumCRView.icPONo).FindControl("lnkPONo")
            lnkPONo.Text = dv("POM_PO_No")

            If strStatus = "Draft" Then
                lnkPONo.NavigateUrl = dDispatcher.direct("PO", "RaisePO.aspx", "pageid=" & strPageId & "&index=" & dv("POM_PO_INDEX") & "&poid=" & dv("POM_PO_NO") & "&mode=po&type=mod")
            Else
                lnkPONo.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx", "Frm=POVIEWTrx&pageid=" & strPageId & "&PO_INDEX=" & dv("POM_PO_INDEX") & "&PO_NO=" & dv("POM_PO_NO") & "&BCoyID=" & dv("POM_B_COY_ID") & "&status=" & dv("POM_PO_STATUS") & "&Caller=POviewTrx&side=b&filetype=2&type=" & Request(Trim("Type")) & "&poview=1")
            End If

            If Common.parseNull(dv("POM_URGENT")) = "1" Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(EnumCRView.icPONo).Controls.Add(imgAttach)
            End If

            Dim objPO As New PurchaseOrder
            If objPO.HasAttachment(dv("POM_PO_No"), dv("POM_B_COY_ID")) Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "clip_icon.gif")
                e.Item.Cells(EnumCRView.icPONo).Controls.Add(imgAttach)
            End If

            'Get PR Number for PO converted from PR
            Dim COUNT As Integer
            Dim ARRAY(100) As String
            Dim STR As String
            Dim i As Integer
            Dim strPR_Index, strPRD_RFQ_Index, strRFQ_No, strPR_Type As String
            Dim lnkPRNo As HyperLink
            lnkPRNo = e.Item.Cells(EnumCRView.icPRNo).FindControl("lnkPRNo")

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
                            STR = STR & "<A href=""" & dDispatcher.direct("PR", "PRDetail.aspx", "caller=POViewTrx&pageid=" & strPageId & "&index=" & strPR_Index & "&PRNO=" & ARRAY(i) & "&type=mod&mode=bc") & """ ><font color=#0000ff>" & ARRAY(i) & "</font></A><br/>"

                            strPR_Type = objDB.GetVal("SELECT PRM_PR_TYPE FROM PR_MSTR WHERE PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRM_PR_NO = '" & ARRAY(i) & "'")
                            If strPR_Type <> "CC" Then
                                e.Item.Cells(EnumCRView.icCreatedBy).Text = dv("POM_CREATED_BY")
                            End If
                        End If
                    Next
                Else
                    'e.Item.Cells(EnumCRView.icCreatedBy).Text = dv("POM_CREATED_BY")
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
                            STR = STR & "<A href=""" & dDispatcher.direct("PR", "PRDetail.aspx", "caller=POViewTrx&pageid=" & strPageId & "&index=" & strPR_Index & "&PRNO=" & ARRAY(i) & "&type=mod&mode=bc") & """ ><font color=#0000ff>" & ARRAY(i) & "</font></A><br/>"

                            strPR_Type = objDB.GetVal("SELECT PRM_PR_TYPE FROM PR_MSTR WHERE PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRM_PR_NO = '" & ARRAY(i) & "'")
                            If strPR_Type <> "CC" Then
                                e.Item.Cells(EnumCRView.icCreatedBy).Text = dv("POM_CREATED_BY")
                            End If
                        End If
                    Next
                Else
                    'e.Item.Cells(EnumCRView.icCreatedBy).Text = dv("POM_CREATED_BY")
                End If
            End If
            e.Item.Cells(EnumCRView.icPRNo).Text = STR

            'If STR = "" Then
            '    lnkPRNo.Text = Common.parseNull(dv("PR_No"))
            '    strPR_Index = objDB.GetVal("SELECT PRM_PR_INDEX FROM PR_MSTR WHERE PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRM_PR_NO = '" & Common.parseNull(dv("PR_No")) & "'")
            '    'STR = STR & "<A>" & dv("PR_No") & "</A><br>"
            '    'STR = STR & "<A href=""#"" onclick=" & dDispatcher.direct("PR", "PRDetail.aspx", "pageid=" & strPageId & "&index=" & strPR_Index & "&PRNO=" & dv("PR_No") & "&type=mod&mode=bc") & "><font color=#0000ff>" & dv("PR_No") & "</font></A><br/>"
            '    lnkPRNo.NavigateUrl = dDispatcher.direct("PR", "PRDetail.aspx", "pageid=" & strPageId & "&index=" & strPR_Index & "&PRNO=" & Common.parseNull(dv("PR_No")) & "&type=mod&mode=bc") & "')"
            'End If
            'e.Item.Cells(EnumCRView.icPRNo).Text = STR

            'Session("quoteurl") = strCallFrom
            Dim ds As DataSet
            Dim Temp_RFQ_INDEX, Temp_S_COY_ID, strRFQNo As String
            Dim objPR As New PurchaseReq2
            If objPO.isConvertedFromRFQ(dv("POM_PO_INDEX"), ds) Then
                If ds.Tables(0).Rows.Count > 0 Then
                    Dim lnkRFQ As New HyperLink
                    lnkRFQ.ImageUrl = dDispatcher.direct("Plugins/Images", "Q-3 Icon (10x10).jpg")
                    If ds.Tables(0).Rows.Count = 1 Then
                        'Michelle (2/11/2009) - To cater for those muti PO where there's no PRM_S_COY_ID
                        'lnkRFQ.NavigateUrl = "../RFQ/viewQoute.aspx?pageid=" & strPageId & "&RFQ_ID=" & ds.Tables(0).Rows(0)("PRM_RFQ_INDEX") & "&vcomid=" & ds.Tables(0).Rows(0)("PRM_S_COY_ID") & "&side=quote"
                        If dv("POM_PO_STATUS") = 1 Or dv("POM_PO_STATUS") = 2 Or dv("POM_PO_STATUS") = 3 _
                            Or dv("POM_PO_STATUS") = 4 Or dv("POM_PO_STATUS") = 5 Or dv("POM_PO_STATUS") = 6 Then
                            If ds.Tables(0).Rows(0)("POM_S_COY_ID") = "" Then
                                strRFQName = objrfq.Get_RFQ_Name(ds.Tables(0).Rows(0)("POM_RFQ_INDEX"))
                                lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "viewQoute.aspx", "Frm=POViewTrx&pageid=" & strPageId & "&RFQ_ID=" & ds.Tables(0).Rows(0)("POM_RFQ_INDEX") & "&RFQ_Num=" & ds.Tables(0).Rows(0)("RM_RFQ_No") & "&vcomid=" & dv("POM_S_COY_ID") & "&side=quote")
                                'lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Frm=POViewB2&RFQType=S&pageid=" & strPageId & "&RFQ_ID=" & ds.Tables(0).Rows(0)("POM_RFQ_INDEX") & "&RFQ_Num=" & ds.Tables(0).Rows(0)("RM_RFQ_No") & "&vcomid=" & dv("POM_S_COY_ID") & "&RFQ_Name=" & strRFQName & "&side=quote")

                            Else
                                strRFQName = objrfq.Get_RFQ_Name(ds.Tables(0).Rows(0)("POM_RFQ_INDEX"))
                                lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "viewQoute.aspx", "Frm=POViewTrx&pageid=" & strPageId & "&RFQ_ID=" & ds.Tables(0).Rows(0)("POM_RFQ_INDEX") & "&RFQ_Num=" & ds.Tables(0).Rows(0)("RM_RFQ_No") & "&vcomid=" & ds.Tables(0).Rows(0)("POM_S_COY_ID") & "&side=quote")
                                'lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Frm=POViewB2&RFQType=S&pageid=" & strPageId & "&RFQ_ID=" & ds.Tables(0).Rows(0)("POM_RFQ_INDEX") & "&RFQ_Num=" & ds.Tables(0).Rows(0)("RM_RFQ_No") & "&vcomid=" & ds.Tables(0).Rows(0)("POM_S_COY_ID") & "&RFQ_Name=" & strRFQName & "&side=quote")

                            End If
                        Else
                            If ds.Tables(0).Rows(0)("POM_S_COY_ID") = "" Then
                                strRFQName = objrfq.Get_RFQ_Name(ds.Tables(0).Rows(0)("POM_RFQ_INDEX"))
                                lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Frm=POViewTrx&RFQType=S&pageid=" & strPageId & "&RFQ_ID=" & ds.Tables(0).Rows(0)("POM_RFQ_INDEX") & "&RFQ_Num=" & ds.Tables(0).Rows(0)("RM_RFQ_No") & "&vcomid=" & dv("POM_S_COY_ID") & "&RFQ_Name=" & strRFQName & "&side=quote")

                            Else
                                strRFQName = objrfq.Get_RFQ_Name(ds.Tables(0).Rows(0)("POM_RFQ_INDEX"))
                                lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Frm=POViewTrx&RFQType=S&pageid=" & strPageId & "&RFQ_ID=" & ds.Tables(0).Rows(0)("POM_RFQ_INDEX") & "&RFQ_Num=" & ds.Tables(0).Rows(0)("RM_RFQ_No") & "&vcomid=" & ds.Tables(0).Rows(0)("POM_S_COY_ID") & "&RFQ_Name=" & strRFQName & "&side=quote")
                            End If
                        End If
                        'If ds.Tables(0).Rows(0)("POM_S_COY_ID") = "" Then
                        '    strRFQName = objrfq.Get_RFQ_Name(ds.Tables(0).Rows(0)("POM_RFQ_INDEX"))
                        '    lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "viewQoute.aspx", "Frm=POViewB2&pageid=" & strPageId & "&RFQ_ID=" & ds.Tables(0).Rows(0)("POM_RFQ_INDEX") & "&RFQ_Num=" & ds.Tables(0).Rows(0)("RM_RFQ_No") & "&vcomid=" & dv("POM_S_COY_ID") & "&side=quote")
                        '    lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Frm=POViewB2&RFQType=S&pageid=" & strPageId & "&RFQ_ID=" & ds.Tables(0).Rows(0)("POM_RFQ_INDEX") & "&RFQ_Num=" & ds.Tables(0).Rows(0)("RM_RFQ_No") & "&vcomid=" & dv("POM_S_COY_ID") & "&RFQ_Name=" & strRFQName & "&side=quote")

                        'Else
                        '    strRFQName = objrfq.Get_RFQ_Name(ds.Tables(0).Rows(0)("POM_RFQ_INDEX"))
                        '    lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "viewQoute.aspx", "Frm=POViewB2&pageid=" & strPageId & "&RFQ_ID=" & ds.Tables(0).Rows(0)("POM_RFQ_INDEX") & "&RFQ_Num=" & ds.Tables(0).Rows(0)("RM_RFQ_No") & "&vcomid=" & ds.Tables(0).Rows(0)("POM_S_COY_ID") & "&side=quote")
                        '    lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Frm=POViewB2&RFQType=S&pageid=" & strPageId & "&RFQ_ID=" & ds.Tables(0).Rows(0)("POM_RFQ_INDEX") & "&RFQ_Num=" & ds.Tables(0).Rows(0)("RM_RFQ_No") & "&vcomid=" & ds.Tables(0).Rows(0)("POM_S_COY_ID") & "&RFQ_Name=" & strRFQName & "&side=quote")

                        'End If
                    Else
                        lnkRFQ.NavigateUrl = dDispatcher.direct("PR", "PRList.aspx", "type=QUO&PageId=" & strPageId & "&DocNo=" & dv("POM_PO_No") & "&index=" & dv("POM_PO_INDEX"))
                    End If

                    e.Item.Cells(EnumCRView.icPONo).Controls.Add(lnkRFQ)
                End If
            Else

                Temp_RFQ_INDEX = objDB.GetVal("SELECT IFNULL(PR_MSTR.PRM_RFQ_INDEX,'') AS PRM_RFQ_INDEX  FROM PR_MSTR  WHERE PRM_PO_INDEX = (SELECT POM_PO_INDEX FROM PO_MSTR WHERE POM_PO_NO = '" & dv("POM_PO_No") & "' AND POM_B_COY_ID = '" & dv("POM_B_COY_ID") & "') ")

                If Not IsDBNull(Temp_RFQ_INDEX) AndAlso CStr(Temp_RFQ_INDEX) <> "" Then
                    Dim lnkRFQ As New HyperLink
                    lnkRFQ.ImageUrl = dDispatcher.direct("Plugins/Images", "Q-3 Icon (10x10).jpg")
                    Temp_S_COY_ID = objDB.GetVal("SELECT IFNULL(PRM_S_COY_ID,'') AS POM_S_COY_ID,  IFNULL(PR_MSTR.PRM_RFQ_INDEX,'') AS PRM_RFQ_INDEX  FROM PR_MSTR  WHERE PRM_PO_INDEX = (SELECT POM_PO_INDEX FROM PO_MSTR WHERE POM_PO_NO = '" & dv("POM_PO_No") & "')")
                    objPR.getRFQName(Temp_RFQ_INDEX, strRFQNo, strRFQName)
                    'lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Frm=POViewB2&RFQType=S&pageid=" & strPageId & "&RFQ_ID=" & Temp_RFQ_INDEX & "&RFQ_Num=" & strRFQNo & "&vcomid=" & dv("POM_S_COY_ID") & "&RFQ_Name=" & strRFQName & "&side=quote")
                    lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "viewQoute.aspx", "Frm=POViewTrx&pageid=" & strPageId & "&RFQ_ID=" & Temp_RFQ_INDEX & "&RFQ_Num=" & strRFQNo & "&vcomid=" & dv("POM_S_COY_ID") & "&side=quote")

                    e.Item.Cells(EnumCRView.icPONo).Controls.Add(lnkRFQ)
                End If
            End If
            objPO = Nothing
        End If
    End Sub

    Private Sub dtg_POList_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_POList.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtg_POList, e)
    End Sub

    Private Sub cboPOStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboPOStatus.SelectedIndexChanged
        '' ''If cboPOStatus.SelectedIndex = 4 Then
        '' ''    ChK_open2.Enabled = True
        '' ''    chk_part2.Enabled = True
        '' ''    chk_fully2.Enabled = True
        '' ''    ChK_open2.Checked = False
        '' ''    chk_part2.Checked = False
        '' ''    chk_fully2.Checked = False
        '' ''Else
        '' ''    ChK_open2.Enabled = False
        '' ''    chk_part2.Enabled = False
        '' ''    chk_fully2.Enabled = False
        '' ''    ChK_open2.Checked = False
        '' ''    chk_part2.Checked = False
        '' ''    chk_fully2.Checked = False
        '' ''End If
    End Sub

    Private Sub cmdClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        Me.txt_po_no.Text = ""
        Me.txt_vendor.Text = ""
        Me.txt_startdate.Text = DateAdd("m", -6, DateTime.Now.ToShortDateString())
        Me.txt_enddate.Text = DateTime.Now.ToShortDateString()
        Me.cboPOStatus.SelectedIndex = 0
    End Sub
End Class
