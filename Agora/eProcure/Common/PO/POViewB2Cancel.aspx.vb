'//This page is called by Buyer Company Only
'//QueryString 
'//filetype ==> 1- PO Cancellation, 2- PO
'//side - u-PO, b-View All PO
'Imports Wheel.Components
Imports AgoraLegacy
Imports eProcure.Component

Public Class POViewB2Cancel
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDB As New EAD.DBCom
    'Dim blnMsg As Boolean

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txt_po_no As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_vendor As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_startdate As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_enddate As System.Web.UI.WebControls.TextBox
    Protected WithEvents CompareValidator1 As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents Table2 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents dtg_POList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents tblSearchResult As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents link As System.Web.UI.HtmlControls.HtmlAnchor
    Protected WithEvents hidlink As System.Web.UI.HtmlControls.HtmlGenericControl


    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Public Enum EnumCRView
        icPONo = 0
        icCreateDate = 1
        icComName = 2
        icAcceptDate = 3
        icStatus = 4
        icFulfilment = 5
        icVIndex = 6
        icVComID = 7
        icPRNo = 8
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
            GenerateTab()
            'If Session("Env") <> "FTN" Then
            '    'Michelle (6/2/2010) - If Buyer Admin logs in, don't display all the records
            '    Dim objUsers As New Users
            '    Dim IsBA As Boolean = objUsers.BAdminRole(HttpContext.Current.Session("UserId"), HttpContext.Current.Session("CompanyId"))
            '    If Not IsBA Then
            '        cmdSearch_Click(sender, e)
            '    End If
            'End If
            'Michelle (6/2/2010) - If Buyer Admin logs in, don't display all the records
            If Session("POType") = "AllPO" Then
                Dim objUsers As New Users
                Dim IsBA As Boolean = objUsers.BAdminRole(HttpContext.Current.Session("UserId"), HttpContext.Current.Session("CompanyId"))
                If Not IsBA Then
                    cmdSearch_Click(sender, e)
                End If
            Else
                cmdSearch_Click(sender, e)
            End If
        End If

        intPageRecordCnt = ViewState("intPageRecordCnt")
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
        Dim objPO As New PurchaseOrder_Buyer

        strStatus = POStatus_new.Submitted & "," & POStatus_new.Accepted & "," & POStatus_new.NewPO & "," & POStatus_new.Open
        strFulfilment = "0," & Fulfilment.Open & ", " & Fulfilment.Part_Delivered & ", " & Fulfilment.Pending_Cancel_Ack
        If Session("POType") = "AllPO" Then
            ds = objPO.VIEW_POList2(strStatus, strFulfilment, "b", Me.txt_vendor.Text, Me.txt_po_no.Text, Me.txt_startdate.Text, Me.txt_enddate.Text)
        Else
            ds = objPO.VIEW_POList2(strStatus, strFulfilment, "b", Me.txt_vendor.Text, Me.txt_po_no.Text, Me.txt_startdate.Text, Me.txt_enddate.Text, , "MyPO")
        End If

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

        ViewState("PageCount") = dtg_POList.PageCount
    End Function

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtg_POList.CurrentPageIndex = 0
        ViewState("SortAscending") = "no"
        ViewState("SortExpression") = "POM_PO_DATE"
        Bindgrid(0)
    End Sub

    Private Sub link_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles link.ServerClick
        Response.Redirect(dDispatcher.direct("PO", "ViewCancel.aspx", "pageid=" & strPageId & "&side=b"))
    End Sub

    Private Sub dtg_POList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_POList.ItemDataBound
        Dim dsRFQ As New DataSet
        Dim objrfq As New RFQ
        Dim strRFQName As String = ""
        Dim objDB As New EAD.DBCom
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim strStatus As String

            e.Item.Cells(EnumCRView.icCreateDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("POM_PO_DATE"))
            e.Item.Cells(EnumCRView.icAcceptDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("POM_ACCEPTED_DATE"))

            Dim lnkPONo As HyperLink
            lnkPONo = e.Item.Cells(EnumCRView.icPONo).FindControl("lnkPONo")
            lnkPONo.Text = dv("POM_PO_No")

            If Common.parseNull(dv("POM_URGENT")) = "1" Then
                Dim lnkUrgent As New HyperLink
                lnkUrgent.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(EnumCRView.icPONo).Controls.Add(lnkUrgent)
            End If

            strStatus = e.Item.Cells(EnumCRView.icStatus).Text
            Select Case strStatus
                Case "Submitted"
                    e.Item.Cells(EnumCRView.icStatus).Text = "Submitted for approval"
                Case "New", "Open", "Approved"
                    e.Item.Cells(EnumCRView.icStatus).Text = "Approved by management"
                Case "Accepted"
                    e.Item.Cells(EnumCRView.icStatus).Text = "Accepted by vendor"
            End Select
            lnkPONo.NavigateUrl = dDispatcher.direct("PO", "cancelitem.aspx", "pageid=" & strPageId & "&PO_NO=" & dv("POM_PO_NO") & "&status=" & dv("POM_PO_STATUS") & "&side=b&INDEX=" & e.Item.Cells(EnumCRView.icVIndex).Text & "&vendor=" & dv("POM_S_COY_ID") & "&BCoyID=" & dv("POM_B_COY_ID"))
            Dim objPO As New PurchaseOrder
            If objPO.HasAttachment(dv("POM_PO_No"), dv("POM_B_COY_ID")) Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/images", "clip_icon.gif")
                e.Item.Cells(EnumCRView.icPONo).Controls.Add(imgAttach)
            End If

            'Get PR Number for PO converted from PR
            Dim COUNT As Integer
            Dim ARRAY(100) As String
            Dim STR As String
            Dim i As Integer
            Dim strPR_Index, strPRD_RFQ_Index, strRFQ_No As String
            Dim lnkPRNo As HyperLink
            lnkPRNo = e.Item.Cells(EnumCRView.icPRNo).FindControl("lnkPRNo")

            Session("urlreferer") = "POViewB2Cancel"
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
                            STR = STR & "<A href=""" & dDispatcher.direct("PR", "PRDetail.aspx", "caller=POViewB2Cancel&pageid=" & strPageId & "&index=" & strPR_Index & "&PRNO=" & ARRAY(i) & "&type=mod&mode=bc") & """ ><font color=#0000ff>" & ARRAY(i) & "</font></A><br/>"
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
                            STR = STR & "<A href=""" & dDispatcher.direct("PR", "PRDetail.aspx", "caller=POViewB2Cancel&pageid=" & strPageId & "&index=" & strPR_Index & "&PRNO=" & ARRAY(i) & "&type=mod&mode=bc") & """ ><font color=#0000ff>" & ARRAY(i) & "</font></A><br/>"
                        End If
                    Next
                End If
            End If
            e.Item.Cells(EnumCRView.icPRNo).Text = STR

            'objPO.getPRNo(dv("POM_PO_No"), ARRAY, COUNT)
            ''objPO.GetPRNo1(dv("POM_PO_INDEX"), ARRAY, COUNT)
            'If ARRAY(0) <> "" Then
            '    For i = 0 To COUNT - 1
            '        STR = STR & "<A>" & ARRAY(i) & "</A><br>"
            '    Next
            'End If
            'If STR = "" Then
            '    STR = STR & "<A>" & dv("PR_No") & "</A><br>"
            'End If
            'e.Item.Cells(EnumCRView.icPRNo).Text = STR

            Session("quoteurl") = strCallFrom
            Dim ds As DataSet
            If objPO.isConvertedFromRFQ(dv("POM_PO_INDEX"), ds) Then
                If ds.Tables(0).Rows.Count > 0 Then
                    Dim lnkRFQ As New HyperLink
                    lnkRFQ.ImageUrl = dDispatcher.direct("Plugins/images", "Q-3 Icon (10x10).jpg")
                    If ds.Tables(0).Rows.Count = 1 Then
                        'Michelle (2/11/2009) - To cater for those muti PO where there's no PRM_S_COY_ID
                        'lnkRFQ.NavigateUrl = "../RFQ/viewQoute.aspx?pageid=" & strPageId & "&RFQ_ID=" & ds.Tables(0).Rows(0)("PRM_RFQ_INDEX") & "&vcomid=" & ds.Tables(0).Rows(0)("PRM_S_COY_ID") & "&side=quote"
                        If dv("POM_PO_STATUS") = 1 Or dv("POM_PO_STATUS") = 2 Or dv("POM_PO_STATUS") = 3 _
                            Or dv("POM_PO_STATUS") = 4 Or dv("POM_PO_STATUS") = 5 Or dv("POM_PO_STATUS") = 6 Then
                            If ds.Tables(0).Rows(0)("POM_S_COY_ID") = "" Then
                                'lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "viewQoute.aspx", "Frm=POViewB2Cancel&pageid=" & strPageId & "&RFQ_ID=" & ds.Tables(0).Rows(0)("POM_RFQ_INDEX") & "&RFQ_Num=" & ds.Tables(0).Rows(0)("RM_RFQ_No") & "&vcomid=" & dv("POM_S_COY_ID") & "&side=quote")
                                strRFQName = objrfq.Get_RFQ_Name(ds.Tables(0).Rows(0)("POM_RFQ_INDEX"))
                                lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "viewQoute.aspx", "Frm=POViewB2Cancel&pageid=" & strPageId & "&RFQ_ID=" & ds.Tables(0).Rows(0)("POM_RFQ_INDEX") & "&RFQ_Num=" & ds.Tables(0).Rows(0)("RM_RFQ_No") & "&vcomid=" & dv("POM_S_COY_ID") & "&side=quote")

                            Else
                                'lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "viewQoute.aspx", "Frm=POViewB2Cancel&pageid=" & strPageId & "&RFQ_ID=" & ds.Tables(0).Rows(0)("POM_RFQ_INDEX") & "&RFQ_Num=" & ds.Tables(0).Rows(0)("RM_RFQ_No") & "&vcomid=" & ds.Tables(0).Rows(0)("POM_S_COY_ID") & "&side=quote")
                                strRFQName = objrfq.Get_RFQ_Name(ds.Tables(0).Rows(0)("POM_RFQ_INDEX"))
                                lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "viewQoute.aspx", "Frm=POViewB2Cancel&pageid=" & strPageId & "&RFQ_ID=" & ds.Tables(0).Rows(0)("POM_RFQ_INDEX") & "&RFQ_Num=" & ds.Tables(0).Rows(0)("RM_RFQ_No") & "&vcomid=" & ds.Tables(0).Rows(0)("POM_S_COY_ID") & "&side=quote")

                            End If
                        Else
                            If ds.Tables(0).Rows(0)("POM_S_COY_ID") = "" Then
                                'lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "viewQoute.aspx", "Frm=POViewB2Cancel&pageid=" & strPageId & "&RFQ_ID=" & ds.Tables(0).Rows(0)("POM_RFQ_INDEX") & "&RFQ_Num=" & ds.Tables(0).Rows(0)("RM_RFQ_No") & "&vcomid=" & dv("POM_S_COY_ID") & "&side=quote")
                                strRFQName = objrfq.Get_RFQ_Name(ds.Tables(0).Rows(0)("POM_RFQ_INDEX"))
                                lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Frm=POViewB2Cancel&RFQType=S&pageid=" & strPageId & "&RFQ_ID=" & ds.Tables(0).Rows(0)("POM_RFQ_INDEX") & "&RFQ_Num=" & ds.Tables(0).Rows(0)("RM_RFQ_No") & "&vcomid=" & dv("POM_S_COY_ID") & "&RFQ_Name=" & strRFQName & "&side=quote")

                            Else
                                'lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "viewQoute.aspx", "Frm=POViewB2Cancel&pageid=" & strPageId & "&RFQ_ID=" & ds.Tables(0).Rows(0)("POM_RFQ_INDEX") & "&RFQ_Num=" & ds.Tables(0).Rows(0)("RM_RFQ_No") & "&vcomid=" & ds.Tables(0).Rows(0)("POM_S_COY_ID") & "&side=quote")
                                strRFQName = objrfq.Get_RFQ_Name(ds.Tables(0).Rows(0)("POM_RFQ_INDEX"))
                                lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Frm=POViewB2Cancel&RFQType=S&pageid=" & strPageId & "&RFQ_ID=" & ds.Tables(0).Rows(0)("POM_RFQ_INDEX") & "&RFQ_Num=" & ds.Tables(0).Rows(0)("RM_RFQ_No") & "&vcomid=" & ds.Tables(0).Rows(0)("POM_S_COY_ID") & "&RFQ_Name=" & strRFQName & "&side=quote")

                            End If
                        End If
                        'If ds.Tables(0).Rows(0)("POM_S_COY_ID") = "" Then
                        '    'lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "viewQoute.aspx", "Frm=POViewB2Cancel&pageid=" & strPageId & "&RFQ_ID=" & ds.Tables(0).Rows(0)("POM_RFQ_INDEX") & "&RFQ_Num=" & ds.Tables(0).Rows(0)("RM_RFQ_No") & "&vcomid=" & dv("POM_S_COY_ID") & "&side=quote")
                        '    strRFQName = objrfq.Get_RFQ_Name(ds.Tables(0).Rows(0)("POM_RFQ_INDEX"))
                        '    'lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Frm=POViewB2Cancel&RFQType=S&pageid=" & strPageId & "&RFQ_ID=" & ds.Tables(0).Rows(0)("POM_RFQ_INDEX") & "&RFQ_Num=" & ds.Tables(0).Rows(0)("RM_RFQ_No") & "&vcomid=" & dv("POM_S_COY_ID") & "&RFQ_Name=" & strRFQName & "&side=quote")
                        '    lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "viewQoute.aspx", "Frm=POViewB2Cancel&pageid=" & strPageId & "&RFQ_ID=" & ds.Tables(0).Rows(0)("POM_RFQ_INDEX") & "&RFQ_Num=" & ds.Tables(0).Rows(0)("RM_RFQ_No") & "&vcomid=" & dv("POM_S_COY_ID") & "&side=quote")

                        'Else
                        '    'lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "viewQoute.aspx", "Frm=POViewB2Cancel&pageid=" & strPageId & "&RFQ_ID=" & ds.Tables(0).Rows(0)("POM_RFQ_INDEX") & "&RFQ_Num=" & ds.Tables(0).Rows(0)("RM_RFQ_No") & "&vcomid=" & ds.Tables(0).Rows(0)("POM_S_COY_ID") & "&side=quote")
                        '    strRFQName = objrfq.Get_RFQ_Name(ds.Tables(0).Rows(0)("POM_RFQ_INDEX"))
                        '    'lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Frm=POViewB2Cancel&RFQType=S&pageid=" & strPageId & "&RFQ_ID=" & ds.Tables(0).Rows(0)("POM_RFQ_INDEX") & "&RFQ_Num=" & ds.Tables(0).Rows(0)("RM_RFQ_No") & "&vcomid=" & ds.Tables(0).Rows(0)("POM_S_COY_ID") & "&RFQ_Name=" & strRFQName & "&side=quote")
                        '    lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "viewQoute.aspx", "Frm=POViewB2Cancel&pageid=" & strPageId & "&RFQ_ID=" & ds.Tables(0).Rows(0)("POM_RFQ_INDEX") & "&RFQ_Num=" & ds.Tables(0).Rows(0)("RM_RFQ_No") & "&vcomid=" & ds.Tables(0).Rows(0)("POM_S_COY_ID") & "&side=quote")

                        'End If
                    Else
                        lnkRFQ.NavigateUrl = dDispatcher.direct("PR", "PRList.aspx", "type=QUO&PageId=" & strPageId & "&DocNo=" & dv("POM_PO_No") & "&index=" & dv("POM_PO_INDEX"))

                    End If

                    e.Item.Cells(EnumCRView.icPONo).Controls.Add(lnkRFQ)
                End If
            End If
            objPO = Nothing
        End If
    End Sub

    Private Sub dtg_POList_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_POList.ItemCreated
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(dtg_POList, e)
    End Sub
    Private Sub GenerateTab()
        Dim _role As New UserRoles
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        'Session("w_POViewBuyerCancel_tabs") = "<div class=""t_entity""><ul>" & _
        '"<li><div class=""space""></div></li>" & _
        '             "<li><a class=""t_entity_btn"" href=""POViewB2.aspx?pageid=" & strPageId & """><span>Purchase Order</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '             "<li><a class=""t_entity_btn_selected"" href=""POViewB2Cancel.aspx?type=Listing&pageid=" & strPageId & """><span>Purchase Order Cancellation</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '             "</ul><div></div></div>"
        'Session("w_POViewBuyerCancel_tabs") = "<div class=""t_entity""><ul>" & _
        '"<li><div class=""space""></div></li>" & _
        '            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PO", "POViewB2.aspx", "pageid=" & strPageId) & """><span>Purchase Order</span></a></li>" & _
        '            "<li><div class=""space""></div></li>" & _
        '            "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PO", "POViewB2Cancel.aspx", "type=Listing&pageid=" & strPageId) & """><span>Purchase Order Cancellation</span></a></li>" & _
        '            "<li><div class=""space""></div></li>" & _
        '            "</ul><div></div></div>"

        Dim showFFPO = objDB.GetVal("SELECT CM_FFPO_CONTROL FROM company_mstr WHERE cm_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'")
        Dim _compType = objDB.GetVal("SELECT CM_COY_TYPE FROM company_mstr WHERE cm_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'")

        'Jules 2018.07.23 PAMB/P2P/0028
        'Dim _userRole = _role.get_UserFixedRole
        Dim _userRole = _role.get_UserFixedRole()
        'End modification.

        If Session("POType") = "AllPO" Then
            Session("w_POViewBuyerCancel_tabs") = "<div class=""t_entity""><ul>" & _
                           "<li><div class=""space""></div></li>" & _
                           "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PO", "POViewB2.aspx", "pageid=" & strPageId) & """><span>Purchase Order</span></a></li>" & _
                           "<li><div class=""space""></div></li>" & _
                           "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PO", "POViewB2Cancel.aspx", "type=Listing&pageid=" & strPageId) & """><span>Purchase Order Cancellation</span></a></li>" & _
                           "<li><div class=""space""></div></li>" & _
                           "</ul><div></div></div>"
        Else
            If showFFPO.ToString.ToUpper = "Y" And (_userRole.ToString.ToUpper.Contains("PURCHASING MANAGER") Or _userRole.ToString.ToUpper.Contains("PURCHASING OFFICER")) And _compType.ToString.ToUpper = "BUYER" Then
                Session("w_POViewBuyerCancel_tabs") = "<div class=""t_entity""><ul>" & _
                          "<li><div class=""space""></div></li>" & _
                                      "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PO", "POViewB2.aspx", "pageid=" & strPageId) & """><span>Purchase Order</span></a></li>" & _
                                      "<li><div class=""space""></div></li>" & _
                                      "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PO", "POViewB2Cancel.aspx", "type=Listing&pageid=" & strPageId) & """><span>Purchase Order Cancellation</span></a></li>" & _
                                      "<li><div class=""space""></div></li>" & _
                                      "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PO", "RAISEFFPO.aspx", "type=Listing&pageid=" & strPageId) & """><span>Free Form Purchase Order</span></a></li>" & _
                                      "<li><div class=""space""></div></li>" & _
                                      "</ul><div></div></div>"
            Else
                Session("w_POViewBuyerCancel_tabs") = "<div class=""t_entity""><ul>" & _
                "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PO", "POViewB2.aspx", "pageid=" & strPageId) & """><span>Purchase Order</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PO", "POViewB2Cancel.aspx", "type=Listing&pageid=" & strPageId) & """><span>Purchase Order Cancellation</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                            "</ul><div></div></div>"
            End If
        End If
        
    End Sub
End Class
