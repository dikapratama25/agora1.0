Imports System.Drawing
Imports AgoraLegacy
Imports eProcure.Component

Public Class AllDashBoardFTN
    Inherits AgoraLegacy.AppBaseClass
    Dim objDO As New Dashboard
    Dim objDB As New EAD.DBCom
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Protected WithEvents divPM As New System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents dtgPendingApprAO As New System.Web.UI.WebControls.DataGrid
    Dim objDO1 As New DeliveryOrder
    Public Enum MyApp
        icPONum = 0
        icSubDate = 1
        icVenName = 2
        icCurren = 3
        icAmt = 4
        icon = 5
        ictw = 6
        icth = 7
    End Enum
    Public Enum OutPO
        icSta = 0
        icPONum = 1
        icVenName = 2
        icAccDate = 3
        icTPQ = 4
        icOPQ = 5
    End Enum
    Public Enum VenPO
        icSta = 0
        icPONum = 1
        icPODate = 2
        icdueDate = 3
        icPurComp = 4
        icOrQ = 5
        icOrOut = 6
    End Enum
    Public Enum OvVenPO
        icPONum = 0
        icCreDate = 1
        icDueDate = 2
        icPurComp = 3
        icOrQ = 4
        icOrOut = 5
    End Enum

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Not Page.IsPostBack Then

            PopulatePanelName()
            SetDashboardGridProperty(dtgPendingApprPM)
            SetDashboardGridProperty(dtgPendingApprAO)
            SetDashboardGridProperty(dtgPendingMyAppr)
            SetDashboardGridProperty(dtgOutstdPO)
            SetDashboardGridProperty(dtgOutstandingRFQ)
            SetDashboardGridProperty(dtgInDOSK)
            SetDashboardGridProperty(dtgOutstandingPOVend)
            SetDashboardGridProperty(dtgOverduePOVend)
            SetDashboardGridProperty(dtgOutstandingRFQVend)
            SetDashboardGridProperty(dtgOutstandingInvoiceVend)
            SetDashboardGridProperty(dtgInPendingPymt)
            SetDashboardGridProperty(dtgInInv)
            SetDashboardGridProperty(dtgOutstdGRNQCVerify)
            SetDashboardGridProperty(dtgOutstdPR)
            SetDashboardGridProperty(dtgPendingMyAppPR)
            SetDashboardGridProperty(dtgPendingConvPR)
            PopulateGrid()
        End If

    End Sub
    Sub PopulatePanelName()
        Dim ds As New DataSet
        Dim objDash As New Dashboard
        Dim rows As DataRow
        Dim strary As New ArrayList
        ds = objDash.GetDashboardPanelName()
        For Each rows In ds.Tables(0).Rows
            strary.Add(rows("DM_PANEL_NAME"))
        Next

        POPendingMyAppr.Text = strary(0)
        POPendingApprPM.Text = strary(1)
        OutstdPO.Text = strary(2)
        OutstdRFQ.Text = strary(3)
        InInv.Text = strary(4)
        InPendingPymt.Text = strary(5)
        OutstdGRNQCVerify.Text = strary(6)
        InDO.Text = strary(7)
        PO.Text = strary(8)
        OverduePO.Text = strary(9)
        OutstdRFQVend.Text = strary(10)
        OutstdInv.Text = strary(11)
        OutstdPR.Text = strary(12)
        PendingMyAppPR.Text = strary(13)
        PendingConvPR.Text = strary(14)

    End Sub
    Sub PopulateGrid()
        Dim objDash As New Dashboard
        Dim ds As New DataSet
        Dim row As DataRow

        Dim fixRole As String
        Dim aryFixRole As New ArrayList
        aryFixRole = Session("MixUserRole")
        For i As Integer = 0 To aryFixRole.Count - 1
            ds = objDash.GetDashboardPanel(aryFixRole(i))
            If ds.Tables(0).Rows.Count > 0 Then
                For Each row In ds.Tables(0).Rows
                    If row("DM_PANEL_ID") = 1 Then
                        divPendingMyAppr.Style("display") = ""
                        dtgPendingMyAppr.CurrentPageIndex = 0
                        ViewState("SortAscendingPendingMyAppr") = "yes"
                        ViewState("SortExpressionPendingMyAppr") = "Submitted Date"
                        BindgridPendingMyAppr()
                    ElseIf row("DM_PANEL_ID") = 2 Then
                        divPendingApprPM.Style("display") = ""
                        dtgPendingApprPM.CurrentPageIndex = 0
                        ViewState("SortAscendingPendingApprPM") = "yes"
                        ViewState("SortExpressionPendingApprPM") = "Submitted Date"
                        BindgridPendingApprPM()
                    ElseIf row("DM_PANEL_ID") = 3 Then
                        divOutstdPO.Style("display") = ""
                        dtgOutstdPO.CurrentPageIndex = 0
                        ViewState("SortAscendingOutstdPO") = "yes"
                        ViewState("SortExpressionOutstdPO") = "Accepted Date"
                        BindgridOutstdPO()
                    ElseIf row("DM_PANEL_ID") = 4 Then
                        divOutstandingRFQ.Style("display") = ""
                        dtgOutstandingRFQ.CurrentPageIndex = 0
                        ViewState("SortAscendingOutstandingRFQ") = "yes"
                        ViewState("SortExpressionOutstandingRFQ") = "Creation Date"
                        BindgridOutstandingRFQ()
                    ElseIf row("DM_PANEL_ID") = 5 Then
                        divInInv.Style("display") = ""
                        dtgInInv.CurrentPageIndex = 0
                        ViewState("SortAscendingInInv") = "yes"
                        ViewState("SortExpressionInInv") = "Due Date"
                        BindgridInInv()
                    ElseIf row("DM_PANEL_ID") = 6 Then
                        divInPendingPymt.Style("display") = ""
                        dtgInPendingPymt.CurrentPageIndex = 0
                        ViewState("SortAscendingInPendingPymt") = "yes"
                        ViewState("SortExpressionInPendingPymt") = "Due Date"
                        BindgridInPendingPymt()
                    ElseIf row("DM_PANEL_ID") = 7 Then
                        divOutstandingGRNforQCVerify.Style("display") = ""
                        dtgOutstdGRNQCVerify.CurrentPageIndex = 0
                        ViewState("SortAscendingOutstandingGRNQCVerify") = "yes"
                        ViewState("SortExpressionOutstandingGRNQCVerify") = "IV_GRN_NO"
                        BindgridOutstandingGRNQCVerify()
                    ElseIf row("DM_PANEL_ID") = 8 Then
                        divInDOSK.Style("display") = ""
                        dtgInDOSK.CurrentPageIndex = 0
                        ViewState("SortAscendingInDOSK") = "yes"
                        ViewState("SortExpressionInDOSK") = "DOM_DO_DATE"
                        BindgridInDOSK()
                    ElseIf row("DM_PANEL_ID") = 9 Then
                        divOutstandingPOVend.Style("display") = ""
                        dtgOutstandingPOVend.CurrentPageIndex = 0
                        ViewState("SortAscendingOutstandingPOVend") = "yes"
                        ViewState("SortExpressionOutstandingPOVend") = "Due Date"
                        BindgridOutstandingPOVend()
                    ElseIf row("DM_PANEL_ID") = 10 Then
                        divOverduePOVend.Style("display") = ""
                        dtgOverduePOVend.CurrentPageIndex = 0
                        ViewState("SortAscendingOverduePOVend") = "yes"
                        ViewState("SortExpressionOverduePOVend") = "Due Date"
                        BindgridOverduePOVend()
                    ElseIf row("DM_PANEL_ID") = 11 Then
                        divOutstandingRFQVend.Style("display") = ""
                        dtgOutstandingRFQVend.CurrentPageIndex = 0
                        ViewState("SortAscendingOutstandingRFQVend") = "yes"
                        ViewState("SortExpressionOutstandingRFQVend") = "Creation Date"
                        BindgridOutstandingRFQVend()
                    ElseIf row("DM_PANEL_ID") = 12 Then
                        divOutstandingInvoiceVend.Style("display") = ""
                        dtgOutstandingInvoiceVend.CurrentPageIndex = 0
                        ViewState("SortAscendingOutstandingInvoiceVend") = "yes"
                        ViewState("SortExpressionOutstandingInvoiceVend") = "PO Number"
                        BindgridOutstandingInvoiceVend()
                    ElseIf row("DM_PANEL_ID") = 13 Then
                        divOutstdPR.Style("display") = ""
                        dtgOutstdPR.CurrentPageIndex = 0
                        ViewState("SortAscendingOutstdPR") = "yes"
                        ViewState("SortExpressionOutstdPR") = "Creation Date"
                        BindgridOutstdPR()
                    ElseIf row("DM_PANEL_ID") = 14 Then
                        divPendingMyAppPR.Style("display") = ""
                        dtgPendingMyAppPR.CurrentPageIndex = 0
                        ViewState("SortAscendingPendingMyAppPR") = "yes"
                        ViewState("SortExpressionPendingMyAppPR") = "Submitted Date"
                        BindgridPendingMyAppPR()
                    ElseIf row("DM_PANEL_ID") = 15 Then
                        divPendingConvPR.Style("display") = ""
                        dtgPendingConvPR.CurrentPageIndex = 0
                        ViewState("SortAscendingPendingConvPR") = "yes"
                        ViewState("SortExpressionPendingConvPR") = "Approved Date"
                        BindgridPendingConvPR()
                    End If
                Next
            End If
        Next

    End Sub
    Private Function BindgridPendingApprPM(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objDO As New Dashboard

        '//Retrieve Data from Database
        Dim dsPendingApprPM As DataSet = New DataSet
        dsPendingApprPM = objDO.GetPendingAppr2("Buyer")

        '//for sorting asc or desc
        Dim dvViewPendingApprPM As DataView
        dvViewPendingApprPM = dsPendingApprPM.Tables(0).DefaultView
        dvViewPendingApprPM.Sort = ViewState("SortExpressionPendingApprPM")
        If ViewState("SortAscendingPendingApprPM") = "no" And ViewState("SortExpressionPendingApprPM") <> "" Then dvViewPendingApprPM.Sort += " DESC"
        If ViewState("actionPendingApprPM") = "del" Then
            If dtgPendingApprPM.CurrentPageIndex > 0 And dsPendingApprPM.Tables(0).Rows.Count Mod dtgPendingApprPM.PageSize = 0 Then
                dtgPendingApprPM.CurrentPageIndex = dtgPendingApprPM.CurrentPageIndex - 1
                ViewState("actionPendingApprPM") = ""
            End If
        End If
        intTotRecord = dsPendingApprPM.Tables(0).Rows.Count
        Session("PageRecordPendingApprPM") = intTotRecord
        resetDashboardDatagridPageIndex(dtgPendingApprPM, dvViewPendingApprPM, "PendingApprPM")
        dtgPendingApprPM.DataSource = dvViewPendingApprPM
        dtgPendingApprPM.DataBind()

        If intTotRecord = 0 Then
            dtgPendingApprPM.ShowHeader = False
            dtgPendingApprPM.PagerStyle.CssClass = "db_displaynone"
        End If

        ' add for above checking
        ViewState("PageCountPendingApprPM") = dtgPendingApprPM.PageCount
    End Function
    Private Function BindgridPendingMyAppr(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objDO As New Dashboard

        '//Retrieve Data from Database
        Dim dsPendingMyAppr As DataSet = New DataSet
        dsPendingMyAppr = objDO.GetPendingAppr2("AO")

        '//for sorting asc or desc
        Dim dvViewPendingMyAppr As DataView
        dvViewPendingMyAppr = dsPendingMyAppr.Tables(0).DefaultView
        dvViewPendingMyAppr.Sort = ViewState("SortExpressionPendingMyAppr")
        If ViewState("SortAscendingPendingMyAppr") = "no" And ViewState("SortExpressionPendingMyAppr") <> "" Then dvViewPendingMyAppr.Sort += " DESC"
        If ViewState("actionPendingMyAppr") = "del" Then
            If dtgPendingMyAppr.CurrentPageIndex > 0 And dsPendingMyAppr.Tables(0).Rows.Count Mod dtgPendingMyAppr.PageSize = 0 Then
                dtgPendingMyAppr.CurrentPageIndex = dtgPendingMyAppr.CurrentPageIndex - 1
                ViewState("actionPendingMyAppr") = ""
            End If
        End If
        intTotRecord = dsPendingMyAppr.Tables(0).Rows.Count
        Session("PageRecordPendingMyAppr") = intTotRecord
        resetDashboardDatagridPageIndex(dtgPendingMyAppr, dvViewPendingMyAppr, "PendingMyAppr")
        dtgPendingMyAppr.DataSource = dvViewPendingMyAppr
        dtgPendingMyAppr.DataBind()

        If intTotRecord = 0 Then
            dtgPendingMyAppr.ShowHeader = False
            dtgPendingMyAppr.PagerStyle.CssClass = "db_displaynone"
        End If

        ' add for above checking
        ViewState("PageCountPendingMyAppr") = dtgPendingMyAppr.PageCount
    End Function
    Private Function BindgridOutstdPR(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objDO As New Dashboard

        '//Retrieve Data from Database
        Dim dsOutstdPR As DataSet = New DataSet
        dsOutstdPR = objDO.GetPendingApprPR("BUYER")

        '//for sorting asc or desc
        Dim dvViewOutstdPR As DataView
        dvViewOutstdPR = dsOutstdPR.Tables(0).DefaultView
        dvViewOutstdPR.Sort = ViewState("SortExpressionOutstdPR")
        If ViewState("SortAscendingOutstdPR") = "no" And ViewState("SortExpressionOutstdPR") <> "" Then dvViewOutstdPR.Sort += " DESC"
        If ViewState("actionOutstdPR") = "del" Then
            If dtgOutstdPR.CurrentPageIndex > 0 And dsOutstdPR.Tables(0).Rows.Count Mod dtgOutstdPR.PageSize = 0 Then
                dtgOutstdPR.CurrentPageIndex = dtgOutstdPR.CurrentPageIndex - 1
                ViewState("actionOutstdPR") = ""
            End If
        End If
        intTotRecord = dsOutstdPR.Tables(0).Rows.Count
        Session("PageRecordOutstdPR") = intTotRecord
        resetDashboardDatagridPageIndex(dtgOutstdPR, dvViewOutstdPR, "OutstdPR")
        dtgOutstdPR.DataSource = dvViewOutstdPR
        dtgOutstdPR.DataBind()

        If intTotRecord = 0 Then
            dtgOutstdPR.ShowHeader = False
            dtgOutstdPR.PagerStyle.CssClass = "db_displaynone"
        End If

        ' add for above checking
        ViewState("PageCountOutstdPR") = dtgOutstdPR.PageCount
    End Function
    Private Function BindgridPendingMyAppPR(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objDO As New Dashboard

        '//Retrieve Data from Database
        Dim dsPendingMyAppPR As DataSet = New DataSet
        dsPendingMyAppPR = objDO.GetPendingApprPR("AO")

        '//for sorting asc or desc
        Dim dvPendingMyAppPR As DataView
        dvPendingMyAppPR = dsPendingMyAppPR.Tables(0).DefaultView
        dvPendingMyAppPR.Sort = ViewState("SortExpressionPendingMyAppPR")
        If ViewState("SortAscendingPendingMyAppPR") = "no" And ViewState("SortExpressionPendingMyAppPR") <> "" Then dvPendingMyAppPR.Sort += " DESC"
        If ViewState("actionPendingMyAppPR") = "del" Then
            If dtgPendingMyAppPR.CurrentPageIndex > 0 And dsPendingMyAppPR.Tables(0).Rows.Count Mod dtgPendingMyAppPR.PageSize = 0 Then
                dtgPendingMyAppPR.CurrentPageIndex = dtgPendingMyAppPR.CurrentPageIndex - 1
                ViewState("actionPendingMyAppPR") = ""
            End If
        End If
        intTotRecord = dsPendingMyAppPR.Tables(0).Rows.Count
        Session("PageRecordPendingMyAppPR") = intTotRecord
        resetDashboardDatagridPageIndex(dtgPendingMyAppPR, dvPendingMyAppPR, "PendingMyAppPR")
        dtgPendingMyAppPR.DataSource = dvPendingMyAppPR
        dtgPendingMyAppPR.DataBind()

        If intTotRecord = 0 Then
            dtgPendingMyAppPR.ShowHeader = False
            dtgPendingMyAppPR.PagerStyle.CssClass = "db_displaynone"
        End If

        ' add for above checking
        ViewState("PageCountPendingMyAppPR") = dtgPendingMyAppPR.PageCount
    End Function

    Private Function BindgridPendingConvPR(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objDO As New Dashboard

        '//Retrieve Data from Database
        Dim dsPendingConvPR As DataSet = New DataSet
        dsPendingConvPR = objDO.GetPendingApprPR("PO")

        '//for sorting asc or desc
        Dim dvPendingConvPR As DataView
        dvPendingConvPR = dsPendingConvPR.Tables(0).DefaultView
        dvPendingConvPR.Sort = ViewState("SortExpressionPendingConvPR")
        If ViewState("SortAscendingPendingConvPR") = "no" And ViewState("SortExpressionPendingConvPR") <> "" Then dvPendingConvPR.Sort += " DESC"
        If ViewState("actionPendingConvPR") = "del" Then
            If dtgPendingConvPR.CurrentPageIndex > 0 And dsPendingConvPR.Tables(0).Rows.Count Mod dtgPendingConvPR.PageSize = 0 Then
                dtgPendingConvPR.CurrentPageIndex = dtgPendingConvPR.CurrentPageIndex - 1
                ViewState("actionPendingConvPR") = ""
            End If
        End If
        intTotRecord = dsPendingConvPR.Tables(0).Rows.Count
        Session("PageRecordPendingConvPR") = intTotRecord
        resetDashboardDatagridPageIndex(dtgPendingConvPR, dvPendingConvPR, "PendingConvPR")
        dtgPendingConvPR.DataSource = dvPendingConvPR
        dtgPendingConvPR.DataBind()

        If intTotRecord = 0 Then
            dtgPendingConvPR.ShowHeader = False
            dtgPendingConvPR.PagerStyle.CssClass = "db_displaynone"
        End If

        ' add for above checking
        ViewState("PageCountPendingConvPR") = dtgPendingConvPR.PageCount
    End Function


    Private Function BindgridOutstdPO(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objDO As New Dashboard

        '//Retrieve Data from Database
        Dim dsOutstdPO As DataSet = New DataSet
        dsOutstdPO = objDO.GetOutstdPO()

        '//for sorting asc or desc
        Dim dvViewOutstdPO As DataView
        dvViewOutstdPO = dsOutstdPO.Tables(0).DefaultView
        dvViewOutstdPO.Sort = ViewState("SortExpressionOutstdPO")
        If ViewState("SortAscendingOutstdPO") = "no" And ViewState("SortExpressionOutstdPO") <> "" Then dvViewOutstdPO.Sort += " DESC"
        If ViewState("actionOutstdPO") = "del" Then
            If dtgOutstdPO.CurrentPageIndex > 0 And dsOutstdPO.Tables(0).Rows.Count Mod dtgOutstdPO.PageSize = 0 Then
                dtgOutstdPO.CurrentPageIndex = dtgOutstdPO.CurrentPageIndex - 1
                ViewState("actionOutstdPO") = ""
            End If
        End If
        intTotRecord = dsOutstdPO.Tables(0).Rows.Count
        Session("PageRecordOutstdPO") = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind
        resetDashboardDatagridPageIndex(dtgOutstdPO, dvViewOutstdPO, "OutstdPO")
        dtgOutstdPO.DataSource = dvViewOutstdPO
        dtgOutstdPO.DataBind()

        If intTotRecord = 0 Then
            dtgOutstdPO.ShowHeader = False
            dtgOutstdPO.PagerStyle.CssClass = "db_displaynone"
        End If
        ' add for above checking
        ViewState("PageCountOutstdPO") = dtgOutstdPO.PageCount
    End Function
    Private Function BindgridOutstandingRFQ(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objDO As New Dashboard

        '//Retrieve Data from Database
        Dim dsOutStandingRFQ As DataSet = New DataSet
        dsOutStandingRFQ = objDO.GetOutstandingRFQBuyer()

        '//for sorting asc or desc
        Dim dvViewOutStandingRFQ As DataView
        dvViewOutStandingRFQ = dsOutStandingRFQ.Tables(0).DefaultView
        dvViewOutStandingRFQ.Sort = ViewState("SortExpressionOutstandingRFQ")
        If ViewState("SortAscendingOutstandingRFQ") = "no" And ViewState("SortExpressionOutstandingRFQ") <> "" Then dvViewOutStandingRFQ.Sort += " DESC"
        If ViewState("actionOutstandingRFQ") = "del" Then
            If dtgOutstandingRFQ.CurrentPageIndex > 0 And dsOutStandingRFQ.Tables(0).Rows.Count Mod dtgOutstandingRFQ.PageSize = 0 Then
                dtgOutstandingRFQ.CurrentPageIndex = dtgOutstandingRFQ.CurrentPageIndex - 1
                ViewState("actionOutstandingRFQ") = ""
            End If
        End If
        intTotRecord = dsOutStandingRFQ.Tables(0).Rows.Count
        'intPageRecordCnt3 = intTotRecord
        Session("PageRecordOutstandingRFQ") = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind
        resetDashboardDatagridPageIndex(dtgOutstandingRFQ, dvViewOutStandingRFQ, "OutstandingRFQ")
        dtgOutstandingRFQ.DataSource = dvViewOutStandingRFQ
        dtgOutstandingRFQ.DataBind()
        If intTotRecord = 0 Then
            dtgOutstandingRFQ.ShowHeader = False
            dtgOutstandingRFQ.PagerStyle.CssClass = "db_displaynone"
        End If
        ' add for above checking
        ViewState("PageCountOutstandingRFQ") = dtgOutstandingRFQ.PageCount
    End Function
    Private Function BindgridInPendingPymt(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objDO As New Dashboard

        '//Retrieve Data from Database
        Dim dsInPendingPymt As DataSet = New DataSet
        dsInPendingPymt = objDO.GetInPendingPymt()

        '//for sorting asc or desc
        Dim dvViewInPendingPymt As DataView
        dvViewInPendingPymt = dsInPendingPymt.Tables(0).DefaultView
        dvViewInPendingPymt.Sort = ViewState("SortExpressionInPendingPymt")
        If ViewState("SortAscendingInPendingPymt") = "no" And ViewState("SortExpressionInPendingPymt") <> "" Then dvViewInPendingPymt.Sort += " DESC"
        If ViewState("actionInPendingPymt") = "del" Then
            If dtgInPendingPymt.CurrentPageIndex > 0 And dsInPendingPymt.Tables(0).Rows.Count Mod dtgInPendingPymt.PageSize = 0 Then
                dtgInPendingPymt.CurrentPageIndex = dtgInPendingPymt.CurrentPageIndex - 1
                ViewState("actionInPendingPymt") = ""
            End If
        End If
        intTotRecord = dsInPendingPymt.Tables(0).Rows.Count
        Session("PageRecordInPendingPymt") = intTotRecord

        resetDashboardDatagridPageIndex(dtgInPendingPymt, dvViewInPendingPymt, "InPendingPymt")
        dtgInPendingPymt.DataSource = dvViewInPendingPymt
        dtgInPendingPymt.DataBind()
        If intTotRecord = 0 Then
            dtgInPendingPymt.ShowHeader = False
            dtgInPendingPymt.PagerStyle.CssClass = "db_displaynone"
        End If
        ' add for above checking
        ViewState("PageCountInPendingPymt") = dtgInPendingPymt.PageCount
    End Function
    Private Function BindgridInInv(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objDO As New Dashboard

        '//Retrieve Data from Database
        Dim dsInInv As DataSet = New DataSet
        dsInInv = objDO.GetInInv()

        '//for sorting asc or desc
        Dim dvViewInInv As DataView
        dvViewInInv = dsInInv.Tables(0).DefaultView
        dvViewInInv.Sort = ViewState("SortExpressionInInv")
        If ViewState("SortAscendingInInv") = "no" And ViewState("SortExpressionInInv") <> "" Then dvViewInInv.Sort += " DESC"
        If ViewState("actionInInv") = "del" Then
            If dtgInInv.CurrentPageIndex > 0 And dsInInv.Tables(0).Rows.Count Mod dtgInInv.PageSize = 0 Then
                dtgInInv.CurrentPageIndex = dtgInInv.CurrentPageIndex - 1
                ViewState("actionInInv") = ""
            End If
        End If
        intTotRecord = dsInInv.Tables(0).Rows.Count
        'intPageRecordCnt4 = intTotRecord
        Session("PageRecordInInv") = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind

        resetDashboardDatagridPageIndex(dtgInInv, dvViewInInv, "InPendingPymt")
        dtgInInv.DataSource = dvViewInInv
        dtgInInv.DataBind()
        If intTotRecord = 0 Then
            dtgInInv.ShowHeader = False
            dtgInInv.PagerStyle.CssClass = "db_displaynone"
        End If
        ' add for above checking
        ViewState("PageCountInInv") = dtgInInv.PageCount
    End Function
    '### StoreKeeper ###
    Private Function BindgridInDOSK(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objDO As New Dashboard

        '//Retrieve Data from Database
        Dim dsInDO As DataSet = New DataSet
        dsInDO = objDO.GetInDO()

        '//for sorting asc or desc
        Dim dvViewInDO As DataView
        dvViewInDO = dsInDO.Tables(0).DefaultView
        dvViewInDO.Sort = ViewState("SortExpressionInDOSK")
        If ViewState("SortAscendingInDOSK") = "no" And ViewState("SortExpressionInDOSK") <> "" Then dvViewInDO.Sort += " DESC"
        If ViewState("actionInDOSK") = "del" Then
            If dtgInDOSK.CurrentPageIndex > 0 And dsInDO.Tables(0).Rows.Count Mod dtgInDOSK.PageSize = 0 Then
                dtgInDOSK.CurrentPageIndex = dtgInDOSK.CurrentPageIndex - 1
                ViewState("actionInDOSK") = ""
            End If
        End If
        intTotRecord = dsInDO.Tables(0).Rows.Count
        Session("PageRecordInDOSK") = intTotRecord
        resetDashboardDatagridPageIndex(dtgInDOSK, dvViewInDO, "InDOSK")
        dtgInDOSK.DataSource = dvViewInDO
        dtgInDOSK.DataBind()

        If intTotRecord = 0 Then
            dtgInDOSK.ShowHeader = False
            dtgInDOSK.PagerStyle.CssClass = "db_displaynone"
        End If

        ' add for above checking
        ViewState("PageCountInDOSK") = dtgInDOSK.PageCount
    End Function
    '### Vendor ###
    Private Function BindgridOutstandingPOVend(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objDO As New Dashboard

        '//Retrieve Data from Database
        Dim dsOutstandingPO As DataSet = New DataSet
        dsOutstandingPO = objDO.GetOutstandingPODash()

        '//for sorting asc or desc
        Dim dvViewOutstandingPO As DataView
        dvViewOutstandingPO = dsOutstandingPO.Tables(0).DefaultView
        dvViewOutstandingPO.Sort = ViewState("SortExpressionOutstandingPOVend")
        If ViewState("SortAscendingOutstandingPOVend") = "no" And ViewState("SortExpressionOutstandingPOVend") <> "" Then dvViewOutstandingPO.Sort += " DESC"
        If ViewState("actionOutstandingPOVend") = "del" Then
            If dtgOutstandingPOVend.CurrentPageIndex > 0 And dsOutstandingPO.Tables(0).Rows.Count Mod dtgOutstandingPOVend.PageSize = 0 Then
                dtgOutstandingPOVend.CurrentPageIndex = dtgOutstandingPOVend.CurrentPageIndex - 1
                ViewState("actionOutstandingPOVend") = ""
            End If
        End If
        intTotRecord = dsOutstandingPO.Tables(0).Rows.Count
        'intPageRecordCnt = intTotRecord
        Session("PageRecordOutstandingPOVend") = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind
        resetDashboardDatagridPageIndex(dtgOutstandingPOVend, dvViewOutstandingPO, "OutstandingPOVend")
        dtgOutstandingPOVend.DataSource = dvViewOutstandingPO
        dtgOutstandingPOVend.DataBind()

        If intTotRecord = 0 Then
            dtgOutstandingPOVend.ShowHeader = False
            dtgOutstandingPOVend.PagerStyle.CssClass = "db_displaynone"
        End If

        ' add for above checking
        ViewState("PageCountOutstandingPOVend") = dtgOutstandingPOVend.PageCount
    End Function

    Private Function BindgridOverduePOVend(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objDO As New Dashboard

        '//Retrieve Data from Database
        Dim dsOverduePO As DataSet = New DataSet
        dsOverduePO = objDO.GetOverduePODash()

        '//for sorting asc or desc
        Dim dvViewOverduePO As DataView
        dvViewOverduePO = dsOverduePO.Tables(0).DefaultView
        dvViewOverduePO.Sort = ViewState("SortExpressionOverduePOVend")
        If ViewState("SortAscendingOverduePOVend") = "no" And ViewState("SortExpressionOverduePOVend") <> "" Then dvViewOverduePO.Sort += " DESC"
        If ViewState("actionOverduePOVend") = "del" Then
            If dtgOverduePOVend.CurrentPageIndex > 0 And dsOverduePO.Tables(0).Rows.Count Mod dtgOverduePOVend.PageSize = 0 Then
                dtgOverduePOVend.CurrentPageIndex = dtgOverduePOVend.CurrentPageIndex - 1
                ViewState("actionOverduePOVend") = ""
            End If
        End If
        intTotRecord = dsOverduePO.Tables(0).Rows.Count
        Session("PageRecordOverduePOVend") = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind
        resetDashboardDatagridPageIndex(dtgOverduePOVend, dvViewOverduePO, "OverduePOVend")
        dtgOverduePOVend.DataSource = dvViewOverduePO
        dtgOverduePOVend.DataBind()

        If intTotRecord = 0 Then
            dtgOverduePOVend.ShowHeader = False
            dtgOverduePOVend.PagerStyle.CssClass = "db_displaynone"
        End If
        ' add for above checking
        ViewState("PageCountOverduePOVend") = dtgOverduePOVend.PageCount
    End Function

    Private Function BindgridOutstandingRFQVend(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objDO As New Dashboard

        '//Retrieve Data from Database
        Dim dsOutStandingRFQ As DataSet = New DataSet
        dsOutStandingRFQ = objDO.GetOutstandingRFQ()

        '//for sorting asc or desc
        Dim dvViewOutStandingRFQ As DataView
        dvViewOutStandingRFQ = dsOutStandingRFQ.Tables(0).DefaultView
        dvViewOutStandingRFQ.Sort = ViewState("SortExpressionOutstandingRFQVend")
        If ViewState("SortAscendingOutstandingRFQVend") = "no" And ViewState("SortExpressionOutstandingRFQVend") <> "" Then dvViewOutStandingRFQ.Sort += " DESC"
        If ViewState("actionOutstandingRFQVend") = "del" Then
            If dtgOutstandingRFQVend.CurrentPageIndex > 0 And dsOutStandingRFQ.Tables(0).Rows.Count Mod dtgOutstandingRFQVend.PageSize = 0 Then
                dtgOutstandingRFQVend.CurrentPageIndex = dtgOutstandingRFQVend.CurrentPageIndex - 1
                ViewState("actionOutstandingRFQVend") = ""
            End If
        End If
        intTotRecord = dsOutStandingRFQ.Tables(0).Rows.Count
        'intPageRecordCnt3 = intTotRecord
        Session("PageRecordOutstandingRFQVend") = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind
        resetDashboardDatagridPageIndex(dtgOutstandingRFQVend, dvViewOutStandingRFQ, "OutstandingRFQVend")
        dtgOutstandingRFQVend.DataSource = dvViewOutStandingRFQ
        dtgOutstandingRFQVend.DataBind()
        If intTotRecord = 0 Then
            dtgOutstandingRFQVend.ShowHeader = False
            dtgOutstandingRFQVend.PagerStyle.CssClass = "db_displaynone"
        End If
        ' add for above checking
        ViewState("PageCountOutstandingRFQVend") = dtgOutstandingRFQVend.PageCount
    End Function

    Private Function BindgridOutstandingInvoiceVend(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objDO As New Dashboard

        '//Retrieve Data from Database
        Dim dsOutStandingInvoice As DataSet = New DataSet
        dsOutStandingInvoice = objDO.GetOutstandingInvoice()

        '//for sorting asc or desc
        Dim dvViewOutStandingInvoice As DataView
        dvViewOutStandingInvoice = dsOutStandingInvoice.Tables(0).DefaultView
        dvViewOutStandingInvoice.Sort = ViewState("SortExpressionOutstandingInvoiceVend")
        If ViewState("SortAscendingOutstandingInvoiceVend") = "no" And ViewState("SortExpressionOutstandingInvoiceVend") <> "" Then dvViewOutStandingInvoice.Sort += " DESC"
        If ViewState("actionOutstandingInvoiceVend") = "del" Then
            If dtgOutstandingInvoiceVend.CurrentPageIndex > 0 And dsOutStandingInvoice.Tables(0).Rows.Count Mod dtgOutstandingInvoiceVend.PageSize = 0 Then
                dtgOutstandingInvoiceVend.CurrentPageIndex = dtgOutstandingInvoiceVend.CurrentPageIndex - 1
                ViewState("actionOutstandingInvoiceVend") = ""
            End If
        End If
        intTotRecord = dsOutStandingInvoice.Tables(0).Rows.Count
        'intPageRecordCnt4 = intTotRecord
        Session("PageRecordOutstandingInvoice") = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind

        resetDashboardDatagridPageIndex(dtgOutstandingInvoiceVend, dvViewOutStandingInvoice, "OutstandingInvoiceVend")
        dtgOutstandingInvoiceVend.DataSource = dvViewOutStandingInvoice
        dtgOutstandingInvoiceVend.DataBind()
        If intTotRecord = 0 Then
            dtgOutstandingInvoiceVend.ShowHeader = False
            dtgOutstandingInvoiceVend.PagerStyle.CssClass = "db_displaynone"
        End If
        ' add for above checking
        ViewState("PageCountOutstandingInvoiceVend") = dtgOutstandingInvoiceVend.PageCount
    End Function

    Private Function BindgridOutstandingGRNQCVerify(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objDO As New Inventory

        '//Retrieve Data from Database
        Dim dsOutstandingGRNQCVerify As DataSet = New DataSet
        dsOutstandingGRNQCVerify = objDO.GetInvVerify()

        '//for sorting asc or desc
        Dim dvOutstandingGRNQCVerify As DataView
        dvOutstandingGRNQCVerify = dsOutstandingGRNQCVerify.Tables(0).DefaultView
        dvOutstandingGRNQCVerify.Sort = ViewState("SortExpressionOutstandingGRNQCVerify")
        If ViewState("SortAscendingOutstandingGRNQCVerify") = "no" And ViewState("SortExpressionOutstandingGRNQCVerify") <> "" Then dvOutstandingGRNQCVerify.Sort += " DESC"
        If ViewState("actionOutstandingGRNQCVerify") = "del" Then
            If dtgOutstdGRNQCVerify.CurrentPageIndex > 0 And dsOutstandingGRNQCVerify.Tables(0).Rows.Count Mod dtgOutstdGRNQCVerify.PageSize = 0 Then
                dtgOutstandingInvoiceVend.CurrentPageIndex = dtgOutstandingInvoiceVend.CurrentPageIndex - 1
                ViewState("actionOutstandingGRNQCVerify") = ""
            End If
        End If
        intTotRecord = dsOutstandingGRNQCVerify.Tables(0).Rows.Count
        'intPageRecordCnt4 = intTotRecord
        Session("PageRecordOutstandingGRNQCVerify") = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind

        resetDashboardDatagridPageIndex(dtgOutstdGRNQCVerify, dvOutstandingGRNQCVerify, "OutstandingGRNQCVerify")
        dtgOutstdGRNQCVerify.DataSource = dvOutstandingGRNQCVerify
        dtgOutstdGRNQCVerify.DataBind()
        If intTotRecord = 0 Then
            dtgOutstdGRNQCVerify.ShowHeader = False
            dtgOutstdGRNQCVerify.PagerStyle.CssClass = "db_displaynone"
        End If
        ' add for above checking
        ViewState("PageCountOutstandingGRNQCVerify") = dtgOutstdGRNQCVerify.PageCount
    End Function
    Sub dtgPendingApprPM_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgPendingApprPM.PageIndexChanged
        dtgPendingApprPM.CurrentPageIndex = e.NewPageIndex
        'Rebind to avoid the Quotation Icon disappearing
        BindgridPendingApprPM()
        BindgridInDOSK()
        BindgridInInv()
        BindgridInPendingPymt()
        BindgridOutstandingInvoiceVend()
        BindgridOutstandingPOVend()
        BindgridOutstandingRFQ()
        BindgridOutstandingRFQVend()
        BindgridOutstandingGRNQCVerify()
        BindgridOutstdPO()
        BindgridOverduePOVend()
        BindgridPendingMyAppr()
        BindgridPendingMyAppPR()
        BindgridOutstdPR()
        BindgridPendingConvPR()
    End Sub


    Sub dtgPendingMyAppr_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgPendingMyAppr.PageIndexChanged
        dtgPendingMyAppr.CurrentPageIndex = e.NewPageIndex
        'Rebind to avoid the Quotation Icon disappearing
        BindgridPendingMyAppr()
        BindgridInDOSK()
        BindgridInInv()
        BindgridInPendingPymt()
        BindgridOutstandingInvoiceVend()
        BindgridOutstandingPOVend()
        BindgridOutstandingRFQ()
        BindgridOutstandingRFQVend()
        BindgridOutstandingGRNQCVerify()
        BindgridOutstdPO()
        BindgridOverduePOVend()
        BindgridPendingApprPM()
        BindgridPendingMyAppPR()
        BindgridOutstdPR()
        BindgridPendingConvPR()
    End Sub
    Sub dtgOutstdPO_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgOutstdPO.PageIndexChanged
        dtgOutstdPO.CurrentPageIndex = e.NewPageIndex
        'Rebind to avoid the Quotation Icon disappearing
        BindgridOutstdPO()
        BindgridInDOSK()
        BindgridInInv()
        BindgridInPendingPymt()
        BindgridOutstandingInvoiceVend()
        BindgridOutstandingPOVend()
        BindgridOutstandingRFQ()
        BindgridOutstandingRFQVend()
        BindgridOutstandingGRNQCVerify()
        BindgridOverduePOVend()
        BindgridPendingApprPM()
        BindgridPendingMyAppr()
        BindgridPendingMyAppPR()
        BindgridOutstdPR()
        BindgridPendingConvPR()
    End Sub
    Sub dtgOutstdPR_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgOutstdPR.PageIndexChanged
        dtgOutstdPR.CurrentPageIndex = e.NewPageIndex
        'Rebind to avoid the Quotation Icon disappearing
        BindgridOutstdPR()
        BindgridPendingMyAppr()
        BindgridInDOSK()
        BindgridInInv()
        BindgridInPendingPymt()
        BindgridOutstandingInvoiceVend()
        BindgridOutstandingPOVend()
        BindgridOutstandingRFQ()
        BindgridOutstandingRFQVend()
        BindgridOutstandingGRNQCVerify()
        BindgridOutstdPO()
        BindgridOverduePOVend()
        BindgridPendingApprPM()
        BindgridPendingMyAppPR()
        BindgridPendingConvPR()
    End Sub
    Sub dtgOutstandingRFQ_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgOutstandingRFQ.PageIndexChanged
        dtgOutstandingRFQ.CurrentPageIndex = e.NewPageIndex
        'Rebind to avoid the Quotation Icon disappearing
        BindgridOutstandingRFQ()
        BindgridInDOSK()
        BindgridInInv()
        BindgridInPendingPymt()
        BindgridOutstandingInvoiceVend()
        BindgridOutstandingPOVend()
        BindgridOutstandingRFQVend()
        BindgridOutstandingGRNQCVerify()
        BindgridOutstdPO()
        BindgridOverduePOVend()
        BindgridPendingApprPM()
        BindgridPendingMyAppr()
        BindgridPendingMyAppPR()
        BindgridOutstdPR()
        BindgridPendingConvPR()
    End Sub
    Sub dtgInPendingPymt_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgInPendingPymt.PageIndexChanged
        dtgInPendingPymt.CurrentPageIndex = e.NewPageIndex
        'Rebind to avoid the Quotation Icon disappearing
        BindgridInPendingPymt()
        BindgridInDOSK()
        BindgridInInv()
        BindgridOutstandingInvoiceVend()
        BindgridOutstandingPOVend()
        BindgridOutstandingRFQ()
        BindgridOutstandingRFQVend()
        BindgridOutstandingGRNQCVerify()
        BindgridOutstdPO()
        BindgridOverduePOVend()
        BindgridPendingApprPM()
        BindgridPendingMyAppr()
        BindgridPendingMyAppPR()
        BindgridOutstdPR()
        BindgridPendingConvPR()
    End Sub
    Sub dtgInInv_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgInInv.PageIndexChanged
        dtgInInv.CurrentPageIndex = e.NewPageIndex
        'Rebind to avoid the Quotation Icon disappearing
        BindgridInInv()
        BindgridInDOSK()
        BindgridInPendingPymt()
        BindgridOutstandingInvoiceVend()
        BindgridOutstandingPOVend()
        BindgridOutstandingRFQ()
        BindgridOutstandingRFQVend()
        BindgridOutstandingGRNQCVerify()
        BindgridOutstdPO()
        BindgridOverduePOVend()
        BindgridPendingApprPM()
        BindgridPendingMyAppr()
        BindgridPendingMyAppPR()
        BindgridOutstdPR()
        BindgridPendingConvPR()
    End Sub
    '### StoreKeeper ###
    Sub dtgInDOSK_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgInDOSK.PageIndexChanged
        dtgInDOSK.CurrentPageIndex = e.NewPageIndex
        'Rebind to avoid the Quotation Icon disappearing
        BindgridInDOSK()
        BindgridInInv()
        BindgridInPendingPymt()
        BindgridOutstandingInvoiceVend()
        BindgridOutstandingPOVend()
        BindgridOutstandingRFQ()
        BindgridOutstandingRFQVend()
        BindgridOutstandingGRNQCVerify()
        BindgridOutstdPO()
        BindgridOverduePOVend()
        BindgridPendingApprPM()
        BindgridPendingMyAppr()
        BindgridPendingMyAppPR()
        BindgridOutstdPR()
        BindgridPendingConvPR()
    End Sub
    '### Vendor ###
    Sub dtgOutstandingPOVend_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgOutstandingPOVend.PageIndexChanged
        dtgOutstandingPOVend.CurrentPageIndex = e.NewPageIndex
        'Rebind to avoid the Quotation Icon disappearing
        BindgridOutstandingPOVend()
        BindgridInDOSK()
        BindgridInInv()
        BindgridInPendingPymt()
        BindgridOutstandingInvoiceVend()
        BindgridOutstandingRFQ()
        BindgridOutstandingRFQVend()
        BindgridOutstandingGRNQCVerify()
        BindgridOutstdPO()
        BindgridOverduePOVend()
        BindgridPendingApprPM()
        BindgridPendingMyAppr()
        BindgridPendingMyAppPR()
        BindgridOutstdPR()
        BindgridPendingConvPR()
    End Sub

    Sub dtgOverduePOVend_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgOverduePOVend.PageIndexChanged
        dtgOverduePOVend.CurrentPageIndex = e.NewPageIndex
        'Rebind to avoid the Quotation Icon disappearing
        BindgridOverduePOVend()
        BindgridInDOSK()
        BindgridInInv()
        BindgridInPendingPymt()
        BindgridOutstandingInvoiceVend()
        BindgridOutstandingPOVend()
        BindgridOutstandingRFQ()
        BindgridOutstandingRFQVend()
        BindgridOutstandingGRNQCVerify()
        BindgridOutstdPO()
        BindgridPendingApprPM()
        BindgridPendingMyAppr()
        BindgridPendingMyAppPR()
        BindgridOutstdPR()
        BindgridPendingConvPR()
    End Sub

    Sub dtgOutstandingRFQVend_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgOutstandingRFQVend.PageIndexChanged
        dtgOutstandingRFQVend.CurrentPageIndex = e.NewPageIndex
        'Rebind to avoid the Quotation Icon disappearing
        BindgridOutstandingRFQVend()
        BindgridInDOSK()
        BindgridInInv()
        BindgridInPendingPymt()
        BindgridOutstandingInvoiceVend()
        BindgridOutstandingPOVend()
        BindgridOutstandingRFQ()
        BindgridOutstandingGRNQCVerify()
        BindgridOutstdPO()
        BindgridOverduePOVend()
        BindgridPendingApprPM()
        BindgridPendingMyAppr()
        BindgridPendingMyAppPR()
        BindgridOutstdPR()
        BindgridPendingConvPR()
    End Sub

    Sub dtgOutstdGRNQCVerify_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgOutstdGRNQCVerify.PageIndexChanged
        dtgOutstdGRNQCVerify.CurrentPageIndex = e.NewPageIndex
        'Rebind to avoid the Quotation Icon disappearing
        BindgridOutstandingGRNQCVerify()
        BindgridInDOSK()
        BindgridInInv()
        BindgridInPendingPymt()
        BindgridOutstandingInvoiceVend()
        BindgridOutstandingPOVend()
        BindgridOutstandingRFQ()
        BindgridOutstandingRFQVend()
        BindgridOutstdPO()
        BindgridOverduePOVend()
        BindgridPendingApprPM()
        BindgridPendingMyAppr()
        BindgridPendingMyAppPR()
        BindgridOutstdPR()
        BindgridPendingConvPR()
    End Sub
    Sub dtgOutstandingInvoiceVend_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgOutstandingInvoiceVend.PageIndexChanged
        dtgOutstandingInvoiceVend.CurrentPageIndex = e.NewPageIndex
        'Rebind to avoid the Quotation Icon disappearing
        BindgridOutstandingInvoiceVend()
        BindgridInDOSK()
        BindgridInInv()
        BindgridInPendingPymt()
        BindgridOutstandingPOVend()
        BindgridOutstandingRFQ()
        BindgridOutstandingRFQVend()
        BindgridOutstandingGRNQCVerify()
        BindgridOutstdPO()
        BindgridOverduePOVend()
        BindgridPendingApprPM()
        BindgridPendingMyAppr()
        BindgridPendingMyAppPR()
        BindgridOutstdPR()
        BindgridPendingConvPR()
    End Sub
    Sub dtgPendingMyAppPR_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgPendingMyAppPR.PageIndexChanged
        dtgPendingMyAppPR.CurrentPageIndex = e.NewPageIndex
        'Rebind to avoid the Quotation Icon disappearing
        BindgridPendingMyAppPR()
        BindgridPendingMyAppr()
        BindgridInDOSK()
        BindgridInInv()
        BindgridInPendingPymt()
        BindgridOutstandingInvoiceVend()
        BindgridOutstandingPOVend()
        BindgridOutstandingRFQ()
        BindgridOutstandingRFQVend()
        BindgridOutstandingGRNQCVerify()
        BindgridOutstdPO()
        BindgridOverduePOVend()
        BindgridPendingApprPM()
        BindgridOutstdPR()
        BindgridPendingConvPR()
    End Sub
    Sub dtgPendingConvPR_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgPendingConvPR.PageIndexChanged
        dtgPendingConvPR.CurrentPageIndex = e.NewPageIndex
        'Rebind to avoid the Quotation Icon disappearing
        BindgridPendingConvPR()
        BindgridPendingMyAppPR()
        BindgridPendingMyAppr()
        BindgridInDOSK()
        BindgridInInv()
        BindgridInPendingPymt()
        BindgridOutstandingInvoiceVend()
        BindgridOutstandingPOVend()
        BindgridOutstandingRFQ()
        BindgridOutstandingRFQVend()
        BindgridOutstandingGRNQCVerify()
        BindgridOutstdPO()
        BindgridOverduePOVend()
        BindgridPendingApprPM()
        BindgridOutstdPR()

    End Sub
    Sub SortCommandPendingApprPM_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "PendingApprPM")
        dtgPendingApprPM.CurrentPageIndex = 0
        BindgridPendingApprPM(True)
        'Rebind to avoid the Quotation Icon disappearing
        BindgridOutstandingGRNQCVerify()
        BindgridInDOSK()
        BindgridInInv()
        BindgridInPendingPymt()
        BindgridOutstandingInvoiceVend()
        BindgridOutstandingPOVend()
        BindgridOutstandingRFQ()
        BindgridOutstandingRFQVend()
        BindgridOutstdPO()
        BindgridOverduePOVend()
        BindgridPendingMyAppr()
        BindgridPendingMyAppPR()
        BindgridOutstdPR()
        BindgridPendingConvPR()
    End Sub
    Sub SortCommandPendingMyAppr_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "PendingMyAppr")
        dtgPendingMyAppr.CurrentPageIndex = 0
        BindgridPendingMyAppr(True)
        'Rebind to avoid the Quotation Icon disappearing
        BindgridOutstandingGRNQCVerify()
        BindgridInDOSK()
        BindgridInInv()
        BindgridInPendingPymt()
        BindgridOutstandingInvoiceVend()
        BindgridOutstandingPOVend()
        BindgridOutstandingRFQ()
        BindgridOutstandingRFQVend()
        BindgridOutstdPO()
        BindgridOverduePOVend()
        BindgridPendingApprPM()
        BindgridPendingMyAppPR()
        BindgridOutstdPR()
        BindgridPendingConvPR()
    End Sub
    Sub SortCommandOutstdPR_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "OutstdPR")
        dtgOutstdPR.CurrentPageIndex = 0
        BindgridOutstdPR(True)
        BindgridPendingMyAppr()
        'Rebind to avoid the Quotation Icon disappearing
        BindgridOutstandingGRNQCVerify()
        BindgridInDOSK()
        BindgridInInv()
        BindgridInPendingPymt()
        BindgridOutstandingInvoiceVend()
        BindgridOutstandingPOVend()
        BindgridOutstandingRFQ()
        BindgridOutstandingRFQVend()
        BindgridOutstdPO()
        BindgridOverduePOVend()
        BindgridPendingApprPM()
        BindgridPendingMyAppPR()
        BindgridPendingConvPR()
    End Sub
    Sub SortCommandOutstdPO_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "OutstdPO")
        dtgOutstdPO.CurrentPageIndex = 0
        BindgridOutstdPO(True)
        'Rebind to avoid the Quotation Icon disappearing
        BindgridInDOSK()
        BindgridInInv()
        BindgridInPendingPymt()
        BindgridOutstandingInvoiceVend()
        BindgridOutstandingPOVend()
        BindgridOutstandingRFQ()
        BindgridOutstandingRFQVend()
        BindgridOutstandingGRNQCVerify()
        BindgridOverduePOVend()
        BindgridPendingApprPM()
        BindgridPendingMyAppr()
        BindgridPendingMyAppPR()
        BindgridOutstdPR()
        BindgridPendingConvPR()
    End Sub
    Sub SortCommandOutStandingRFQ_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "OutstandingRFQ")
        dtgOutstandingRFQ.CurrentPageIndex = 0
        BindgridOutstandingRFQ(True)
        'Rebind to avoid the Quotation Icon disappearing
        BindgridInDOSK()
        BindgridInInv()
        BindgridInPendingPymt()
        BindgridOutstandingInvoiceVend()
        BindgridOutstandingPOVend()
        BindgridOutstandingRFQVend()
        BindgridOutstandingGRNQCVerify()
        BindgridOutstdPO()
        BindgridOverduePOVend()
        BindgridPendingApprPM()
        BindgridPendingMyAppr()
        BindgridPendingMyAppPR()
        BindgridOutstdPR()
        BindgridPendingConvPR()
    End Sub
    Sub SortCommandInPendingPymt_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "InPendingPymt")
        dtgInPendingPymt.CurrentPageIndex = 0
        BindgridInPendingPymt(True)
        'Rebind to avoid the Quotation Icon disappearing
        BindgridInDOSK()
        BindgridInInv()
        BindgridOutstandingInvoiceVend()
        BindgridOutstandingPOVend()
        BindgridOutstandingRFQ()
        BindgridOutstandingRFQVend()
        BindgridOutstandingGRNQCVerify()
        BindgridOutstdPO()
        BindgridOverduePOVend()
        BindgridPendingApprPM()
        BindgridPendingMyAppr()
        BindgridPendingMyAppPR()
        BindgridOutstdPR()
        BindgridPendingConvPR()
    End Sub
    Sub SortCommandInInv_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "InInv")
        dtgInInv.CurrentPageIndex = 0
        BindgridInInv(True)
        'Rebind to avoid the Quotation Icon disappearing
        BindgridInDOSK()
        BindgridInPendingPymt()
        BindgridOutstandingInvoiceVend()
        BindgridOutstandingPOVend()
        BindgridOutstandingRFQ()
        BindgridOutstandingRFQVend()
        BindgridOutstandingGRNQCVerify()
        BindgridOutstdPO()
        BindgridOverduePOVend()
        BindgridPendingApprPM()
        BindgridPendingMyAppr()
        BindgridPendingMyAppPR()
        BindgridOutstdPR()
        BindgridPendingConvPR()
    End Sub
    '### StoreKeeper ###
    Sub SortCommandInDOSK_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "InDOSK")
        dtgInDOSK.CurrentPageIndex = 0
        BindgridInDOSK(True)
        'Rebind to avoid the Quotation Icon disappearing
        BindgridInInv()
        BindgridInPendingPymt()
        BindgridOutstandingInvoiceVend()
        BindgridOutstandingPOVend()
        BindgridOutstandingRFQ()
        BindgridOutstandingRFQVend()
        BindgridOutstandingGRNQCVerify()
        BindgridOutstdPO()
        BindgridOverduePOVend()
        BindgridPendingApprPM()
        BindgridPendingMyAppr()
        BindgridPendingMyAppPR()
        BindgridOutstdPR()
        BindgridPendingConvPR()
    End Sub
    '### Vendor ###
    Sub SortCommandOutStandingPOVend_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "OutstandingPOVend")
        dtgOutstandingPOVend.CurrentPageIndex = 0
        BindgridOutstandingPOVend(True)
        'Rebind to avoid the Quotation Icon disappearing
        BindgridInDOSK()
        BindgridInInv()
        BindgridInPendingPymt()
        BindgridOutstandingInvoiceVend()
        BindgridOutstandingRFQ()
        BindgridOutstandingRFQVend()
        BindgridOutstandingGRNQCVerify()
        BindgridOutstdPO()
        BindgridOverduePOVend()
        BindgridPendingApprPM()
        BindgridPendingMyAppr()
        BindgridPendingMyAppPR()
        BindgridOutstdPR()
        BindgridPendingConvPR()
    End Sub

    Sub SortCommandOverduePOVend_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "OverduePOVend")
        dtgOverduePOVend.CurrentPageIndex = 0
        BindgridOverduePOVend(True)
        'Rebind to avoid the Quotation Icon disappearing
        BindgridInDOSK()
        BindgridInInv()
        BindgridInPendingPymt()
        BindgridOutstandingInvoiceVend()
        BindgridOutstandingPOVend()
        BindgridOutstandingRFQ()
        BindgridOutstandingRFQVend()
        BindgridOutstandingGRNQCVerify()
        BindgridOutstdPO()
        BindgridPendingApprPM()
        BindgridPendingMyAppr()
        BindgridPendingMyAppPR()
        BindgridOutstdPR()
        BindgridPendingConvPR()
    End Sub

    Sub SortCommandOutStandingRFQVend_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "OutstandingRFQVend")
        dtgOutstandingRFQVend.CurrentPageIndex = 0
        BindgridOutstandingRFQVend(True)
        'Rebind to avoid the Quotation Icon disappearing
        BindgridInDOSK()
        BindgridInInv()
        BindgridInPendingPymt()
        BindgridOutstandingInvoiceVend()
        BindgridOutstandingPOVend()
        BindgridOutstandingRFQ()
        BindgridOutstandingGRNQCVerify()
        BindgridOutstdPO()
        BindgridOverduePOVend()
        BindgridPendingApprPM()
        BindgridPendingMyAppr()
        BindgridPendingMyAppPR()
        BindgridOutstdPR()
        BindgridPendingConvPR()
    End Sub
    Sub SortCommandOutStandingInvoiceVend_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "OutstandingInvoiceVend")
        dtgOutstandingInvoiceVend.CurrentPageIndex = 0
        BindgridOutstandingInvoiceVend(True)
        'Rebind to avoid the Quotation Icon disappearing
        BindgridInDOSK()
        BindgridInInv()
        BindgridInPendingPymt()
        BindgridOutstandingPOVend()
        BindgridOutstandingRFQ()
        BindgridOutstandingRFQVend()
        BindgridOutstandingGRNQCVerify()
        BindgridOutstdPO()
        BindgridOverduePOVend()
        BindgridPendingApprPM()
        BindgridPendingMyAppr()
        BindgridPendingMyAppPR()
        BindgridOutstdPR()
        BindgridPendingConvPR()
    End Sub
    Sub SortCommandOutstandingGRNQCVerify_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "OutstandingGRNQCVerify")
        dtgOutstdGRNQCVerify.CurrentPageIndex = 0
        BindgridOutstandingGRNQCVerify(True)
        'Rebind to avoid the Quotation Icon disappearing
        BindgridInDOSK()
        BindgridInInv()
        BindgridInPendingPymt()
        BindgridOutstandingInvoiceVend()
        BindgridOutstandingPOVend()
        BindgridOutstandingRFQ()
        BindgridOutstandingRFQVend()
        BindgridOutstdPO()
        BindgridOverduePOVend()
        BindgridPendingApprPM()
        BindgridPendingMyAppr()
        BindgridPendingMyAppPR()
        BindgridOutstdPR()
        BindgridPendingConvPR()
    End Sub
    Sub SortCommandPendingMyAppPR_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "PendingMyAppPR")
        dtgPendingMyAppPR.CurrentPageIndex = 0
        BindgridPendingMyAppPR(True)
        'Rebind to avoid the Quotation Icon disappearing
        BindgridOutstdPO()
        BindgridInDOSK()
        BindgridInInv()
        BindgridInPendingPymt()
        BindgridOutstandingInvoiceVend()
        BindgridOutstandingPOVend()
        BindgridOutstandingRFQ()
        BindgridOutstandingRFQVend()
        BindgridOutstandingGRNQCVerify()
        BindgridOverduePOVend()
        BindgridPendingApprPM()
        BindgridPendingMyAppr()
        BindgridOutstdPR()
        BindgridPendingConvPR()
    End Sub
    Sub SortCommandPendingConvPR_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "PendingConvPR")
        dtgPendingConvPR.CurrentPageIndex = 0
        BindgridPendingConvPR(True)
        'Rebind to avoid the Quotation Icon disappearing
        BindgridOutstdPO()
        BindgridInDOSK()
        BindgridInInv()
        BindgridInPendingPymt()
        BindgridOutstandingInvoiceVend()
        BindgridOutstandingPOVend()
        BindgridOutstandingRFQ()
        BindgridOutstandingRFQVend()
        BindgridOutstandingGRNQCVerify()
        BindgridOverduePOVend()
        BindgridPendingApprPM()
        BindgridPendingMyAppr()
        BindgridOutstdPR()
        BindgridPendingMyAppPR()
    End Sub
    Private Sub dtgPendingApprPM_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPendingApprPM.ItemCreated
        Grid_ItemCreatedDashboard(dtgPendingApprPM, e, "PendingApprPM")
    End Sub
    Private Sub dtgOutstdPR_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstdPR.ItemCreated
        Grid_ItemCreatedDashboard(dtgOutstdPR, e, "OutstdPR")
    End Sub
    Private Sub dtgPendingMyAppr_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPendingMyAppr.ItemCreated
        Grid_ItemCreatedDashboard(dtgPendingMyAppr, e, "PendingMyAppr")
    End Sub
    Private Sub dtgOutstdPO_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstdPO.ItemCreated
        Grid_ItemCreatedDashboard(dtgOutstdPO, e, "OutstdPO")
    End Sub
    Private Sub dtgOutStandingRFQ_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstandingRFQ.ItemCreated
        Grid_ItemCreatedDashboard(dtgOutstandingRFQ, e, "OutstandingRFQ")
    End Sub
    Private Sub dtgInPendingPymt_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgInPendingPymt.ItemCreated
        Grid_ItemCreatedDashboard(dtgInPendingPymt, e, "InPendingPymt")
    End Sub
    Private Sub dtgInInv_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgInInv.ItemCreated
        Grid_ItemCreatedDashboard(dtgInInv, e, "InInv")
    End Sub
    '### StoreKeeper ###
    Private Sub dtgInDOSK_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgInDOSK.ItemCreated
        Grid_ItemCreatedDashboard(dtgInDOSK, e, "InDOSK")
    End Sub
    '### Vendor ###
    Private Sub dtgOutStandingPOVend_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstandingPOVend.ItemCreated
        Grid_ItemCreatedDashboard(dtgOutstandingPOVend, e, "OutstandingPOVend")
    End Sub

    Private Sub dtgOverduePOVend_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOverduePOVend.ItemCreated
        Grid_ItemCreatedDashboard(dtgOverduePOVend, e, "OverduePOVend")
    End Sub

    Private Sub dtgOutStandingRFQVend_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstandingRFQVend.ItemCreated
        Grid_ItemCreatedDashboard(dtgOutstandingRFQVend, e, "OutstandingRFQVend")
    End Sub

    Private Sub dtgOutStandingInvoiceVend_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstandingInvoiceVend.ItemCreated
        Grid_ItemCreatedDashboard(dtgOutstandingInvoiceVend, e, "OutstandingInvoiceVend")
    End Sub
    Private Sub dtgOutstdGRNQCVerify_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstdGRNQCVerify.ItemCreated
        Grid_ItemCreatedDashboard(dtgOutstdGRNQCVerify, e, "OutstandingGRNQCVerify")
    End Sub
    Private Sub dtgPendingMyAppPR_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPendingMyAppPR.ItemCreated
        Grid_ItemCreatedDashboard(dtgPendingMyAppPR, e, "PendingMyAppPR")
    End Sub
    Private Sub dtgPendingConvPR_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPendingConvPR.ItemCreated
        Grid_ItemCreatedDashboard(dtgPendingConvPR, e, "PendingConvPR")
    End Sub
    Private Sub dtgPendingApprPM_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPendingApprPM.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim objPR As New PurchaseReq2
            Dim objPO As New PurchaseOrder
            Dim lnkPONo
            lnkPONo = e.Item.FindControl("lnkPONum")
            lnkPONo.Text = dv("PO Number")

            '       lnkPONo.NavigateUrl = "../PO/PODetail.aspx?Frm=Dashboard&caller=BUYER&index=" & dv("POM_PO_Index") & "&PONo=" & dv("PO Number")
            lnkPONo.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx", "Frm=Dashboard&side=b&filetype=2&dpage=AllDashBoard&PO_INDEX=" & dv("POM_PO_Index") & "&PO_NO=" & dv("PO Number") & "&status=" & dv("POM_PO_STATUS") & "&BCoyID=" & dv("POM_B_COY_ID"))

            e.Item.Cells(1).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Submitted Date"))
            e.Item.Cells(5).Text = Format(CDbl(dv("Amount")), "###,###,##0.00")

            If Common.parseNull(dv("POM_URGENT")) = "1" Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(0).Controls.Add(imgAttach)
            End If

            If Not IsDBNull(dv("POM_RFQ_INDEX")) AndAlso CStr(dv("POM_RFQ_INDEX")) <> "" Then
                Dim lnkRFQ As New HyperLink
                Dim strRFQNo, strRFQName As String
                objPR.getRFQName(dv("POM_RFQ_INDEX"), strRFQNo, strRFQName)
                lnkRFQ.ImageUrl = dDispatcher.direct("Plugins/Images", "Q-3 Icon (10x10).jpg")
                If dv("POM_PO_STATUS") = 1 Or dv("POM_PO_STATUS") = 2 Or dv("POM_PO_STATUS") = 3 _
                    Or dv("POM_PO_STATUS") = 4 Or dv("POM_PO_STATUS") = 5 Or dv("POM_PO_STATUS") = 6 Then
                    lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "viewQoute.aspx", "Frm=Dashboard&PageID=108&RFQ_ID=" & dv("POM_RFQ_INDEX") & "&RFQ_Num=" & strRFQNo & "&vcomid=" & dv("POM_S_COY_ID") & "&side=quote")
                    lnkRFQ.ToolTip = "Click here to view quotation details"
                Else
                    lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Appr=Y&Frm=Dashboard&RFQType=S&PageID=7&side=quote&RFQ_Num=" & strRFQNo & "&RFQ_ID=" & dv("POM_RFQ_INDEX") & "&vcomid=" & dv("POM_S_COY_ID") & "&RFQ_Name=" & Server.UrlEncode(strRFQName))
                    lnkRFQ.ToolTip = "Click here to view quotation comparison"
                End If
                ''lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Frm=Dashboard&RFQType=S&PageID=7&side=quote&RFQ_Num=" & strRFQNo & "&RFQ_ID=" & dv("POM_RFQ_INDEX") & "&vcomid=" & dv("POM_S_COY_ID") & "&RFQ_Name=" & Server.UrlEncode(strRFQName))
                'lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "viewQoute.aspx", "Frm=Dashboard&PageID=108&RFQ_ID=" & dv("POM_RFQ_INDEX") & "&RFQ_Num=" & strRFQNo & "&vcomid=" & dv("POM_S_COY_ID") & "&side=quote")
                'lnkRFQ.ToolTip = "Click here to view quotation comparison"
                e.Item.Cells(MyApp.icPONum).Controls.Add(lnkRFQ)
            End If

            If objPO.HasAttachment(dv("PO Number"), HttpContext.Current.Session("CompanyId")) Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "clip_icon.gif")
                e.Item.Cells(0).Controls.Add(imgAttach)
            End If
            objPR = Nothing
        End If
    End Sub
    Private Sub dtgPendingMyAppr_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPendingMyAppr.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim objPR As New PurchaseReq2
            Dim objPO As New PurchaseOrder
            Dim lnkPONo
            lnkPONo = e.Item.FindControl("lnkPONum")
            lnkPONo.Text = dv("PO Number")

            'lnkPONo.NavigateUrl = "../PO/POApprDetail.aspx?Frm=Dashboard&caller=BUYER&pageid=" & Session("strPageId") & "index=" & dv("POM_PO_Index") & "&relief=false&PONo=" & dv("PO Number")
            lnkPONo.NavigateUrl = dDispatcher.direct("PO", "POApprDetail.aspx", "Frm=Dashboard&dpage=AllDashBoard&AO=" & Session("UserID") & "&relief=false&PageID=8&index=" & dv("POM_PO_Index") & "&PONO=" & dv("PO Number"))

            e.Item.Cells(1).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Submitted Date"))
            e.Item.Cells(5).Text = Format(CDbl(dv("Amount")), "###,###,##0.00")

            If Common.parseNull(dv("POM_URGENT")) = "1" Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(0).Controls.Add(imgAttach)
            End If

            If Not IsDBNull(dv("POM_RFQ_INDEX")) AndAlso CStr(dv("POM_RFQ_INDEX")) <> "" Then
                Dim lnkRFQ As New HyperLink
                Dim strRFQNo, strRFQName As String
                objPR.getRFQName(dv("POM_RFQ_INDEX"), strRFQNo, strRFQName)
                lnkRFQ.ImageUrl = dDispatcher.direct("Plugins/Images", "Q-3 Icon (10x10).jpg")
                ''lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Frm=Dashboard&PageID=8&side=other&RFQ_Num=" & strRFQNo & "&RFQ_ID=" & dv("POM_RFQ_INDEX") & "&RFQ_Name=" & Server.UrlEncode(strRFQName))
                'lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "viewQoute.aspx", "Frm=Dashboard&PageID=108&RFQ_ID=" & dv("POM_RFQ_INDEX") & "&RFQ_Num=" & strRFQNo & "&vcomid=" & dv("POM_S_COY_ID") & "&side=quote")
                'lnkRFQ.ToolTip = "Click here to view quotation comparison"
                If dv("POM_PO_STATUS") = 1 Or dv("POM_PO_STATUS") = 2 Or dv("POM_PO_STATUS") = 3 _
                    Or dv("POM_PO_STATUS") = 4 Or dv("POM_PO_STATUS") = 5 Or dv("POM_PO_STATUS") = 6 Then
                    lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "viewQoute.aspx", "Frm=Dashboard&PageID=108&RFQ_ID=" & dv("POM_RFQ_INDEX") & "&RFQ_Num=" & strRFQNo & "&vcomid=" & dv("POM_S_COY_ID") & "&side=quote")
                    lnkRFQ.ToolTip = "Click here to view quotation details"
                Else
                    lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Appr=Y&Frm=Dashboard&RFQType=S&PageID=8&side=quote&RFQ_Num=" & strRFQNo & "&RFQ_ID=" & dv("POM_RFQ_INDEX") & "&vcomid=" & dv("POM_S_COY_ID") & "&RFQ_Name=" & Server.UrlEncode(strRFQName))
                    lnkRFQ.ToolTip = "Click here to view quotation comparison"
                End If
                e.Item.Cells(MyApp.icPONum).Controls.Add(lnkRFQ)
            End If

            If objPO.HasAttachment(dv("PO Number"), HttpContext.Current.Session("CompanyId")) Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "clip_icon.gif")
                e.Item.Cells(0).Controls.Add(imgAttach)
            End If
            objPR = Nothing
        End If
    End Sub
    Private Sub dtgOutstdPR_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstdPR.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim objPR As New PurchaseReq2
            Dim lnkPRNo
            lnkPRNo = e.Item.FindControl("lnkPRNo")
            lnkPRNo.Text = dv("PR Number")
            Session("urlreferer") = "Dashboard"
            If strPageId = "" Then
                strPageId = "113"
            End If
            If dv("PRM_PR_STATUS") = 1 Then 'Draft PR - RaisePR screen
                lnkPRNo.NavigateUrl = dDispatcher.direct("PR", "RaisePR.aspx", "Frm=Dashboard&pageid=" & strPageId & "&index=" & dv("PRM_PR_Index") & "&prid=" & dv("PR Number") & "&type=mod&mode=bc")

            Else    'PR Detail screen
                lnkPRNo.NavigateUrl = dDispatcher.direct("PR", "PRDetail.aspx", "Frm=Dashboard&pageid=" & strPageId & "&index=" & dv("PRM_PR_Index") & "&PRNO=" & dv("PR Number") & "&type=mod&mode=bc")

            End If

            If Common.parseNull(dv("PRM_URGENT")) = "1" Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(0).Controls.Add(imgAttach)
            End If

            If objPR.HasAttachment(dv("PR Number")) Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "clip_icon.gif")
                e.Item.Cells(0).Controls.Add(imgAttach)
            End If

            If Not IsDBNull(dv("Creation Date")) Then
                e.Item.Cells(1).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Creation Date"))
            End If
            If Not IsDBNull(dv("Submission Date")) Then
                e.Item.Cells(2).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Submission Date"))
            End If
            If Not IsDBNull(dv("Approved Date")) Then
                e.Item.Cells(3).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Approved Date"))
            End If

            If dv("PRM_PR_STATUS") = 3 Then 'if status=Pending approval(3) showed as 'Submitted'
                e.Item.Cells(4).Text = "Submitted"
            ElseIf dv("PRM_PR_STATUS") = 6 Then 'if status=Cancelled By(6) showed as 'Cancelled'
                e.Item.Cells(4).Text = "Cancelled"
            ElseIf dv("PRM_PR_STATUS") = 8 Then 'if status=Rejected By(8) showed as 'Rejected'
                e.Item.Cells(4).Text = "Rejected"
            ElseIf dv("PRM_PR_STATUS") = 9 Then 'if status=Void Draft PR(9) showed as 'Void'
                e.Item.Cells(4).Text = "Void"
            End If
            objPR = Nothing
        End If
    End Sub
    Private Sub dtgOutstdPO_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstdPO.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkRFQ As New HyperLink
            Dim strRFQNo, strRFQName As String
            Dim objPR As New PurchaseReq2
            Dim objPO As New PurchaseOrder
            Dim lnkPONo
            Dim lblStatus As Label

            lblStatus = e.Item.FindControl("lblStatus")
            If dv("POM_PO_STATUS") = "0" Then
                lblStatus.Text = "Draft"
            ElseIf dv("POM_PO_STATUS") = "3" Then
                lblStatus.Text = "Accepted"
            Else
                lblStatus.Text = "Approved"
            End If

            lnkPONo = e.Item.FindControl("lnkPONum2")
            lnkPONo.Text = dv("PO Number")

            If dv("POM_PO_STATUS") = "0" Then
                lnkPONo.NavigateUrl = dDispatcher.direct("PO", "RaisePO.aspx", "pageid=7&index=" & dv("POM_PO_INDEX") & "&poid=" & dv("PO Number") & "&mode=po&type=mod&Frm=Dashboard&dpage=AllDashBoard")
            Else
                lnkPONo.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx", "pageid=7&PO_INDEX=" & dv("POM_PO_INDEX") & "&PO_NO=" & dv("PO Number") & "&BCoyID=" & dv("POM_B_COY_ID") & "&status=" & dv("POM_PO_STATUS") & "&side=b&filetype=2&type=list&Frm=Dashboard&dpage=AllDashBoard")
            End If
            'e.Item.Cells(2).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("PO Date"))
            e.Item.Cells(4).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Accepted Date"))

            If Common.parseNull(dv("POM_URGENT")) = "1" Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(OutPO.icPONum).Controls.Add(imgAttach)
            End If

            If Not IsDBNull(dv("POM_RFQ_INDEX")) AndAlso CStr(dv("POM_RFQ_INDEX")) <> "" Then
                objPR.getRFQName(dv("POM_RFQ_INDEX"), strRFQNo, strRFQName)
                lnkRFQ.ImageUrl = dDispatcher.direct("Plugins/Images", "Q-3 Icon (10x10).jpg")
                'lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Frm=Dashboard&RFQType=S&PageID=7&side=quote&RFQ_Num=" & strRFQNo & "&RFQ_ID=" & dv("POM_RFQ_INDEX") & "&vcomid=" & dv("POM_S_COY_ID") & "&RFQ_Name=" & Server.UrlEncode(strRFQName))
                'lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "viewQoute.aspx", "Frm=Dashboard&PageID=108&RFQ_ID=" & dv("POM_RFQ_INDEX") & "&RFQ_Num=" & strRFQNo & "&vcomid=" & dv("POM_S_COY_ID") & "&side=quote")
                'lnkRFQ.ToolTip = "Click here to view quotation comparison"
                If dv("POM_PO_STATUS") = 1 Or dv("POM_PO_STATUS") = 2 Or dv("POM_PO_STATUS") = 3 _
                            Or dv("POM_PO_STATUS") = 4 Or dv("POM_PO_STATUS") = 5 Or dv("POM_PO_STATUS") = 6 Then
                    lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "viewQoute.aspx", "Frm=Dashboard&PageID=108&RFQ_ID=" & dv("POM_RFQ_INDEX") & "&RFQ_Num=" & strRFQNo & "&vcomid=" & dv("POM_S_COY_ID") & "&side=quote")
                    lnkRFQ.ToolTip = "Click here to view quotation details"
                Else
                    lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Appr=Y&Frm=Dashboard&RFQType=S&PageID=7&side=quote&RFQ_Num=" & strRFQNo & "&RFQ_ID=" & dv("POM_RFQ_INDEX") & "&vcomid=" & dv("POM_S_COY_ID") & "&RFQ_Name=" & Server.UrlEncode(strRFQName))
                    lnkRFQ.ToolTip = "Click here to view quotation comparison"
                End If
                e.Item.Cells(OutPO.icPONum).Controls.Add(lnkRFQ)
            End If

            If objPO.HasAttachment(dv("PO Number"), HttpContext.Current.Session("CompanyId")) Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "clip_icon.gif")
                e.Item.Cells(1).Controls.Add(imgAttach)
            End If
            objPR = Nothing
        End If
    End Sub
    Private Sub dtgOutStandingRFQ_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstandingRFQ.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim objrfq As New RFQ
            Dim lnkRFQNo, lnkRFQViewRes
            Dim i As Integer
            lnkRFQNo = e.Item.FindControl("lnkRFQNum")
            lnkRFQNo.Text = dv("RFQ Number")

            lnkRFQViewRes = e.Item.FindControl("lnkRFQViewRes")
            Dim check As Boolean = objrfq.check_v_status(Common.parseNull(dv("RM_RFQ_ID"))) ' 1=response
            If dv("RM_Status") = "3" Then
                lnkRFQViewRes.Text = "Draft"
                lnkRFQNo.NavigateUrl = dDispatcher.direct("RFQ", "Create_RFQ.aspx", "pageid=93&Frm=Dashboard&dpage=AllDashBoard" & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&RFQ_Num=" & Server.UrlEncode(Common.parseNull(dv("RFQ Number"))))
                'Response.Redirect("Create_RFQ.aspx?pageid=" & strPageId & "&chk_option=1&edit=0")
                'ElseIf dv("RM_RFQ_OPTION") = 1 Then
                '    lnkRFQViewRes.Text = "Response"
            Else
                If IsDBNull(dv("Expiry Date")) Then

                Else
                    lnkRFQNo.NavigateUrl = dDispatcher.direct("RFQ", "RFQDetail.aspx", "pageid=5&page=1&goto=1&RFQ_Num=" & Server.UrlEncode(dv("RFQ Number")) & "&Frm=Dashboard&dpage=AllDashBoard&RFQ_ID=" & dv("RM_RFQ_ID"))
                    'lnkRFQNo.Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQDetail.aspx", "pageid=5&page=1&goto=1&RFQ_Num=" & server.urlencode(dv("RFQ Number")) & "&Frm=Dashboard&dpage=AllDashBoard&RFQ_ID=" & dv("RM_RFQ_ID"))) & " "" ><font color=#0000ff>" & Common.parseNull(server.urlencode(dv("RFQ Number"))) & "</font></A>"
                    If Common.parseNull(dv("Expiry Date")) >= Date.Today Then
                        If check = False Then
                            lnkRFQViewRes.Text = "Sent"

                        ElseIf check = True Then
                            If dv("RM_RFQ_OPTION") = 0 Then 'open
                                'lnkRFQViewRes.Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Frm=RFQ_Outstg_List&RFQType=V&pageid=" & strPageId & "&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&RFQ_Name=" & Server.UrlEncode(Common.parseNull(dv("RM_RFQ_Name")))) & """ ><font color=#0000ff>View Response</font></A>"
                                lnkRFQViewRes.Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQComSummary.aspx", "disabled=N&RFQType=V&pageid=7&Frm=Dashboard&dpage=AllDashBoard&RFQ_Num=" & (Common.parseNull(Server.UrlEncode(dv("RFQ Number")))) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&RFQ_Name=" & Server.UrlEncode(Common.parseNull(dv("RFQ Name")))) & """ ><font color=#0000ff>View Response</font></A>"

                            ElseIf dv("RM_RFQ_OPTION") = 1 Then  'close
                                lnkRFQViewRes.Text = "Response"
                            End If
                        End If
                    Else
                        If check = False Then
                            lnkRFQViewRes.Text = "No Response, Expired"
                        ElseIf check = True Then
                            lnkRFQViewRes.Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQComSummary.aspx", "disabled=N&RFQType=V&pageid=7&Frm=Dashboard&dpage=AllDashBoard&RFQ_Num=" & Server.UrlEncode(Common.parseNull(dv("RFQ Number"))) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&RFQ_Name=" & Server.UrlEncode(Common.parseNull(dv("RFQ Name")))) & """ ><font color=#0000ff>View Response</font></A>, Expired"
                        End If
                    End If
                End If
                ''check whether there's any response for the RFQ
                'i = objDB.GetCount("rfq_replies_mstr", " where rrm_rfq_id = '" & dv("RM_RFQ_ID") & "'")
                'If i > 0 Then
                '    lnkRFQViewRes.Text = "View Response"
                '    lnkRFQViewRes.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "RFQType=V&pageid=7&Frm=Dashboard&dpage=AllDashBoard&RFQ_Num=" & Common.parseNull(dv("RFQ Number")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&RFQ_Name=" & Common.parseNull(dv("RFQ Name")))
                '    Session("dPage") = "PurMgr"
                '    'lbl_status.Text = "<A href=""RFQComSummary.aspx?pageid=" & strPageId & "&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&RFQ_Name=" & Server.UrlEncode(Common.parseNull(dv("RM_RFQ_Name"))) & """ ><font color=#0000ff>View Response</font></A>"
                'Else
                '    lnkRFQViewRes.Text = "Sent"
                'End If
                'lnkRFQNo.NavigateUrl = dDispatcher.direct("RFQ", "RFQDetail.aspx", "pageid=5&page=1&goto=1&RFQ_Num=" & dv("RFQ Number") & "&Frm=Dashboard&dpage=AllDashBoard&RFQ_ID=" & dv("RM_RFQ_ID"))
            End If

            If objrfq.HasAttachment(dv("RFQ Number")) Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "clip_icon.gif")
                e.Item.Cells(1).Controls.Add(imgAttach)
            End If

            e.Item.Cells(4).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Creation Date"))
            e.Item.Cells(5).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Expiry Date"))
        End If
    End Sub
    Private Sub dtgInPendingPymt_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgInPendingPymt.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim lnkInvNo
            lnkInvNo = e.Item.FindControl("lnkInvNum")
            lnkInvNo.Text = dv("Invoice Number")
            lnkInvNo.NavigateUrl = dDispatcher.direct("Tracking", "InvoiceDetails.aspx", "Frm=Dashboard&dpage=AllDashBoard&Name=FMnAO&pageid=15&INVNO=" & dv("Invoice Number") & "&vcomid=" & dv("IM_S_COY_ID") & "&folder=A&status=2")

            e.Item.Cells(2).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Due Date"))
            e.Item.Cells(5).Text = Format(CDbl(dv("Amount")), "###,###,##0.00")
        End If
    End Sub
    Private Sub dtgInInv_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgInInv.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim lnkInvNo
            lnkInvNo = e.Item.FindControl("lnkInv")
            lnkInvNo.Text = dv("Invoice Number")


            'Dim dsDO As DataSet = New DataSet
            'dsDO = objDO.GetDOStatus(1, dv("POM_PO_INDEX"))

            'If dsDO.Tables(0).Rows.Count > 0 Then
            '    lnkPONo.NavigateUrl = "../DO/AddDO.aspx?Frm=Dashboard&DONo=" & dsDO.Tables(0).Rows(0)("DOM_DO_NO") & "&DOIdx=" & dv("POM_PO_INDEX") & "&POIdx=" & dv("POM_PO_INDEX") & "&mode=Edit&pageid=22&LocID=" & dsDO.Tables(0).Rows(0)("DOM_D_ADDR_CODE") & "&PONo=" & dv("PO Number") & "&BCoy=" & dv("POM_B_Coy_ID")
            'Else
            '    lnkPONo.NavigateUrl = "../DO/AddDO.aspx?Frm=Dashboard&Mode=New&PONo=" & dv("PO Number") & "&BCoy=" & dv("Buyer Company") & "&POIdx=" & dv("POM_PO_INDEX") & "&pageid=22"
            'End If
            'lnkInvNo.NavigateUrl = "../Tracking/InvoiceDetails.aspx?Frm=Dashboard&BMode=New&pageid=" & strPageId & "&INVNO=" & Common.parseNull(dv("Invoice Number")) & "&vcomid=" & Common.parseNull(dv("IM_S_COY_ID")) & "&folder=N" & "&status=1"
            lnkInvNo.NavigateUrl = dDispatcher.direct("Tracking", "InvoiceDetails.aspx", "Frm=Dashboard&Name=Buyer&BMode=New&pageid=15&INVNO=" & Common.parseNull(dv("Invoice Number")) & "&vcomid=" & Common.parseNull(dv("IM_S_COY_ID")) & "&folder=N" & "&status=1")

            '
            e.Item.Cells(2).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Due Date"))
            e.Item.Cells(5).Text = Format(CDbl(dv("Amount")), "###,###,##0.00")
        End If
    End Sub
    Private Sub dtgPendingMyAppPR_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPendingMyAppPR.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim objPR As New PurchaseReq2
            Dim lnkPRNo
            lnkPRNo = e.Item.FindControl("lnkPRNo")
            lnkPRNo.Text = dv("PR Number")
            Session("urlreferer") = "Dashboard"

            'lnkPONo.NavigateUrl = "../PO/POApprDetail.aspx?Frm=Dashboard&caller=BUYER&pageid=" & Session("strPageId") & "index=" & dv("POM_PO_Index") & "&relief=false&PONo=" & dv("PO Number")
            lnkPRNo.NavigateUrl = dDispatcher.direct("PR", "PRApprDetail.aspx", "Frm=Dashboard&dpage=AllDashBoard&AO=" & Session("UserID") & "&relief=false&PageID=8&index=" & dv("PRM_PR_Index") & "&PRNo=" & dv("PR Number"))
            If Not IsDBNull(dv("Submitted Date")) Then
                e.Item.Cells(3).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Submitted Date"))
            End If

            If Common.parseNull(dv("PRM_URGENT")) = "1" Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(0).Controls.Add(imgAttach)
            End If

            If objPR.HasAttachment(dv("PR Number")) Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "clip_icon.gif")
                e.Item.Cells(0).Controls.Add(imgAttach)
            End If

            objPR = Nothing
        End If
    End Sub
    Private Sub dtgPendingConvPR_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPendingConvPR.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim objPR As New PurchaseReq2
            Dim lnkPRNo
            lnkPRNo = e.Item.FindControl("lnkPRNo")
            lnkPRNo.Text = dv("PR Number")
            Session("urlreferer") = "Dashboard"
            If strPageId = "" Then
                strPageId = "114"
            End If

            If Common.parseNull(dv("PRM_URGENT")) = "1" Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(0).Controls.Add(imgAttach)
            End If

            lnkPRNo.NavigateUrl = dDispatcher.direct("PR", "ConvertPR.aspx", "Frm=Dashboard&pageid=" & strPageId & "&index=" & dv("PRM_PR_Index") & "&PRNo=" & dv("PR Number"))
            If Not IsDBNull(dv("Approved Date")) Then
                e.Item.Cells(3).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Approved Date"))

            End If

            objPR = Nothing
        End If
    End Sub
    '### StoreKeeper ###
    Private Sub dtgInDOSK_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgInDOSK.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim lnkDONo
            lnkDONo = e.Item.FindControl("lnkDONum")
            lnkDONo.Text = dv("DOM_DO_NO")
            strPageId = 13
            Session("strPageId") = strPageId

            lnkDONo.NavigateUrl = dDispatcher.direct("GRN", "AddGRN1.aspx", "Frm=Dashboard&Mode=New&PONo=" & dv("POM_PO_No") & "&DONo=" & dv("DOM_DO_NO") & "&DOIndex=" & dv("DOM_DO_Index") & "&pageid=13&Level=created" & "&vendor=" & dv("CM_COY_NAME"))
            e.Item.Cells(1).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("DOM_DO_DATE"))
            e.Item.Cells(4).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("POM_PO_DATE"))
            If objDO1.withAttach(dv("DOM_DO_NO"), dv("DOM_S_COY_ID")) Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "clip_icon.gif")
                e.Item.Cells(0).Controls.Add(imgAttach)
            End If

        End If

    End Sub
    '### Vendor ###
    Private Sub dtgOutStandingPOVend_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstandingPOVend.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkRFQ As New HyperLink
            Dim strRFQNo, strRFQName As String
            Dim objPR As New PurchaseReq2
            Dim lnkPONo
            Dim lblStatus As Label
            Dim objPO As New PurchaseOrder
            Dim objPO1 As New PurchaseOrder_Vendor
            lnkPONo = e.Item.FindControl("lnkPONum")
            lnkPONo.Text = dv("PO Number")

            Dim dsDO As DataSet = New DataSet


            lblStatus = e.Item.FindControl("lblStatus")
            If dv("POM_PO_STATUS") = "1" Or dv("POM_PO_STATUS") = "2" Then
                lblStatus.Text = "New"
                lnkPONo.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx", "Frm=Dashboard&pageid=100&PO_INDEX=" & dv("POM_PO_INDEX") & "&PO_NO=" & dv("PO Number") & "&BCoyID=" & dv("POM_B_COY_ID") & "&status=" & Common.parseNull(dv("POM_PO_STATUS")) & "&side=v&filetype=2&dpage=AllDashBoard")

            ElseIf dv("POM_PO_STATUS") = "3" And dv("POM_FULFILMENT") = "4" Then 'with partial delivery
                lblStatus.Text = "Cancelled"
                lnkPONo.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx", "Frm=Dashboard&pageid=100&PO_INDEX=" & dv("POM_PO_INDEX") & "&PO_NO=" & dv("PO Number") & "&BCoyID=" & dv("POM_B_COY_ID") & "&status=" & Common.parseNull(dv("POM_PO_STATUS")) & "&side=v&filetype=1&dpage=AllDashBoard&cr_no=" & Common.parseNull(dv("CR_NO")))

            ElseIf dv("POM_PO_STATUS") = "3" Then
                lblStatus.Text = "Outstanding"
                dsDO = objDO.GetDOStatus(1, dv("POM_PO_INDEX"))
                If dsDO.Tables(0).Rows.Count > 0 Then
                    'lnkPONo.NavigateUrl = "../DO/AddDO.aspx?Frm=Dashboard&dpage=Vendor&Mode=Edit&PONo=" & dv("PO Number") & "&BCoy=" & dv("POM_B_COY_ID") & "&POIdx=" & dv("POM_PO_INDEX") & "&pageid=" & strPageId & "&LocID=" & dsDO.Tables(0).Rows(0)("DOM_D_ADDR_CODE") & "&DONo=" & dsDO.Tables(0).Rows(0)("DOM_DO_NO") & "&DOIdx=" & dsDO.Tables(0).Rows(0)("DOM_DO_Index")
                    lnkPONo.NavigateUrl = dDispatcher.direct("DO", "AddDO.aspx", "Frm=Dashboard&dpage=AllDashBoard&Mode=Edit&PONo=" & dv("PO Number") & "&BCoy=" & dv("POM_B_COY_ID") & "&POIdx=" & dv("POM_PO_INDEX") & "&pageid=100&LocID=" & dsDO.Tables(0).Rows(0)("DOM_D_ADDR_CODE") & "&DONo=" & dsDO.Tables(0).Rows(0)("DOM_DO_NO"))

                Else
                    lnkPONo.NavigateUrl = dDispatcher.direct("DO", "AddDO.aspx", "Frm=Dashboard&dpage=AllDashBoard&Mode=New&PONo=" & dv("PO Number") & "&BCoy=" & dv("POM_B_COY_ID") & "&POIdx=" & dv("POM_PO_INDEX") & "&pageid=100")
                End If

                'lnkPONo.NavigateUrl = "../DO/AddDO.aspx?Frm=Dashboard&dpage=Vendor&DONo=" & dsDO.Tables(0).Rows(0)("DOM_DO_NO") & "&DOIdx=" & dv("POM_PO_INDEX") & "&POIdx=" & dv("POM_PO_INDEX") & "&mode=Edit&LocID=" & dsDO.Tables(0).Rows(0)("DOM_D_ADDR_CODE") & "&PONo=" & dv("PO Number") & "&BCoy=" & dv("POM_B_Coy_ID") & "&pageid=" & strPageId

            Else
                lblStatus.Text = "Cancelled"
                lnkPONo.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx", "Frm=Dashboard&pageid=100&PO_INDEX=" & dv("POM_PO_INDEX") & "&PO_NO=" & dv("PO Number") & "&BCoyID=" & dv("POM_B_COY_ID") & "&status=" & Common.parseNull(dv("POM_PO_STATUS")) & "&side=v&filetype=1&dpage=AllDashBoard&cr_no=" & Common.parseNull(dv("CR_NO")))
            End If

            'If dsDO.Tables(0).Rows.Count > 0 Then
            '    lnkPONo.NavigateUrl = "../DO/AddDO.aspx?Frm=Dashboard&DONo=" & dsDO.Tables(0).Rows(0)("DOM_DO_NO") & "&DOIdx=" & dv("POM_PO_INDEX") & "&POIdx=" & dv("POM_PO_INDEX") & "&mode=Edit&LocID=" & dsDO.Tables(0).Rows(0)("DOM_D_ADDR_CODE") & "&PONo=" & dv("PO Number") & "&BCoy=" & dv("POM_B_Coy_ID") & "&pageid=" & strPageId
            'Else
            'End If

            e.Item.Cells(3).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("PO Date"))
            e.Item.Cells(4).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Due Date"))

            If Common.parseNull(dv("POM_URGENT")) = "1" Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(VenPO.icPONum).Controls.Add(imgAttach)
            End If

            If Not IsDBNull(dv("POM_RFQ_INDEX")) AndAlso CStr(dv("POM_RFQ_INDEX")) <> "" Then
                objPR.getRFQName(dv("POM_RFQ_INDEX"), strRFQNo, strRFQName)
                lnkRFQ.ImageUrl = dDispatcher.direct("Plugins/Images", "Q-3 Icon (10x10).jpg")
                lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "viewQoute.aspx", "Frm=Dashboard&PageID=108&RFQ_ID=" & dv("POM_RFQ_INDEX") & "&RFQ_Num=" & strRFQNo & "&vcomid=" & dv("POM_S_COY_ID") & "&side=quote")
                lnkRFQ.ToolTip = "Click here to view quotation comparison"
                e.Item.Cells(VenPO.icPONum).Controls.Add(lnkRFQ)
            End If
            'If objPO.HasAttachmentVen(dv("PO Number")) Then
            If objPO.HasAttachment(dv("PO Number"), objPO1.get_comid(dv("POM_PO_Index"))) Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "clip_icon.gif")
                e.Item.Cells(VenPO.icPONum).Controls.Add(imgAttach)
            End If
            objPR = Nothing
        End If
    End Sub

    Private Sub dtgOverduePOVend_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOverduePOVend.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkRFQ As New HyperLink
            Dim strRFQNo, strRFQName As String
            Dim objPR As New PurchaseReq2
            Dim objPO As New PurchaseOrder
            Dim lnkPONo
            Dim objPO1 As New PurchaseOrder_Vendor
            lnkPONo = e.Item.FindControl("lnkPONum2")
            lnkPONo.Text = dv("POM_PO_No")

            Dim dsDO As DataSet = New DataSet
            dsDO = objDO.GetDOStatus(1, dv("POM_PO_INDEX"))


            'If dsCurrency.Tables(0).Rows.Count > 0 Then
            'lblRate.Text = dsCurrency.Tables(0).Rows(0)("CE_RATE")


            'If dsDO.Tables(0).Rows.Count > 0 Then
            '    lnkPONo.NavigateUrl = "../DO/AddDO.aspx?Frm=Dashboard&DONo=" & dsDO.Tables(0).Rows(0)("DOM_DO_NO") & "&DOIdx=" & dv("POM_PO_INDEX") & "&POIdx=" & dv("POM_PO_INDEX") & "&mode=Edit&pageid=22&LocID=" & dsDO.Tables(0).Rows(0)("DOM_D_ADDR_CODE") & "&PONo=" & dv("POM_PO_No") & "&BCoy=" & dv("POM_B_Coy_ID")
            'Else
            '    lnkPONo.NavigateUrl = "../DO/AddDO.aspx?Frm=Dashboard&Mode=New&PONo=" & dv("POM_PO_No") & "&BCoy=" & dv("Buyer Company") & "&POIdx=" & dv("POM_PO_INDEX") & "&pageid=22"
            'End If



            If dsDO.Tables(0).Rows.Count > 0 Then
                'lnkPONo.NavigateUrl = "../DO/AddDO.aspx?Frm=Dashboard&Mode=Edit&PONo=" & dv("POM_PO_No") & "&BCoy=" & dv("POM_B_Coy_ID") & "&POIdx=" & dv("POM_PO_INDEX") & "&DONo=" & dsDO.Tables(0).Rows(0)("DOM_DO_NO") & "&DOIdx=" & dsDO.Tables(0).Rows(0)("DOM_DO_Index") & "&LocID=" & dsDO.Tables(0).Rows(0)("DOM_D_ADDR_CODE") & "&pageid=" & strPageId
                lnkPONo.NavigateUrl = dDispatcher.direct("DO", "AddDO.aspx", "Frm=Dashboard&Mode=Edit&PONo=" & dv("POM_PO_No") & "&BCoy=" & dv("POM_B_Coy_ID") & "&POIdx=" & dv("POM_PO_INDEX") & "&DONo=" & dsDO.Tables(0).Rows(0)("DOM_DO_NO") & "&LocID=" & dsDO.Tables(0).Rows(0)("DOM_D_ADDR_CODE") & "&pageid=100")

            Else
                lnkPONo.NavigateUrl = dDispatcher.direct("DO", "AddDO.aspx", "Frm=Dashboard&Mode=New&PONo=" & dv("POM_PO_No") & "&BCoy=" & dv("POM_B_Coy_ID") & "&POIdx=" & dv("POM_PO_INDEX") & "&pageid=100")
            End If

            e.Item.Cells(2).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("PO Date"))
            e.Item.Cells(3).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Due Date"))

            If Common.parseNull(dv("POM_URGENT")) = "1" Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(OvVenPO.icPONum).Controls.Add(imgAttach)
            End If

            If Not IsDBNull(dv("POM_RFQ_INDEX")) AndAlso CStr(dv("POM_RFQ_INDEX")) <> "" Then
                objPR.getRFQName(dv("POM_RFQ_INDEX"), strRFQNo, strRFQName)
                lnkRFQ.ImageUrl = dDispatcher.direct("Plugins/Images", "Q-3 Icon (10x10).jpg")
                lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "viewQoute.aspx", "Frm=Dashboard&PageID=108&RFQ_ID=" & dv("POM_RFQ_INDEX") & "&RFQ_Num=" & strRFQNo & "&vcomid=" & dv("POM_S_COY_ID") & "&side=quote")
                lnkRFQ.ToolTip = "Click here to view quotation comparison"
                e.Item.Cells(OvVenPO.icPONum).Controls.Add(lnkRFQ)
            End If

            'If objPO.HasAttachmentVen(dv("POM_PO_No")) Then
            If objPO.HasAttachment(dv("POM_PO_No"), objPO1.get_comid(dv("POM_PO_INDEX"))) Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "clip_icon.gif")
                e.Item.Cells(OvVenPO.icPONum).Controls.Add(imgAttach)
            End If
        End If
    End Sub

    Private Sub dtgOutStandingRFQVend_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstandingRFQVend.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim lnkRFQNo
            Dim objrfq As New RFQ
            lnkRFQNo = e.Item.FindControl("lnkRFQNum")
            lnkRFQNo.Text = dv("RFQ Number")


            'lnkRFQNo.NavigateUrl = "../RFQ/RFQDetail.aspx?pageid=21&page=1&goto=1&RFQ_Num=" & dv("RFQ Number") & "&RFQ_ID=" & dv("RM_RFQ_ID") & "&vcom_id=" & Session("CompanyId")


            'lnkRFQNo.NavigateUrl = "../RFQ/VendorRFQList1.aspx?pageid=" & strPageId & "&RFQ_No=" & dv("RFQ Number") & "&RFQ_ID=" & dv("RM_RFQ_ID") & "&bcomid=" & dv("RM_Coy_ID")
            lnkRFQNo.NavigateUrl = dDispatcher.direct("RFQ", "CreateQuotationNew.aspx", "Frm=Dashboard&pageid=98&RFQ_No=" & Server.UrlEncode(dv("RFQ Number")) & "&RFQ_ID=" & dv("RM_RFQ_ID") & "&bcomid=" & dv("RM_Coy_ID"))

            'lbl_status.Text = "New, <A href=""CreateQuotation.aspx?pageid=" & strPageId & " &bcomid=" & dv("RM_Coy_ID") & "&RFQ_No=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & " "" ><font color=#0000ff>Reply</font></A>"

            e.Item.Cells(2).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Creation Date"))
            e.Item.Cells(3).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Expiry Date"))

            If objrfq.HasAttachmentVen(dv("RFQ Number"), dv("RM_Coy_ID"), "E") Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "clip_icon.gif")
                e.Item.Cells(0).Controls.Add(imgAttach)
            End If
        End If
    End Sub

    Private Sub dtgOutStandingInvoiceVend_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstandingInvoiceVend.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim lnkInvNo
            lnkInvNo = e.Item.FindControl("lnkInv")
            lnkInvNo.Text = dv("PO Number")

            'Dim dsDO As DataSet = New DataSet
            'dsDO = objDO.GetDOStatus(1, dv("POM_PO_INDEX"))

            'If dsDO.Tables(0).Rows.Count > 0 Then
            '    lnkInvNo.NavigateUrl = "../Invoice/InvGeneration1.aspx?Frm=Dashboard&DONo=" & dsDO.Tables(0).Rows(0)("DO Number") & "&POIdx=" & dv("POM_PO_INDEX") & "&mode=Edit&pageid=" & strPageId & "&LocID=" & dsDO.Tables(0).Rows(0)("DOM_D_ADDR_CODE") & "&PONo=" & dv("PO Number") & "&BCoy=" & dv("POM_B_Coy_ID")
            'Else
            '    lnkInvNo.NavigateUrl = "../Invoice/InvGeneration1.aspx?Frm=Dashboard&Mode=New&PONo=" & dv("PO Number") & "&BCoy=" & dv("CM_COY_NAME") & "&POIdx=" & dv("POM_PO_INDEX") & "&pageid=" & strPageId
            'End If
            lnkInvNo.NavigateUrl = dDispatcher.direct("Invoice", "InvList.aspx", "Frm=Dashboard&VMode=New&PONo=" & dv("PO Number") & "&DONo=" & dv("DO Number") & "&GRNNo=" & dv("GRN Number") & "&BCoy=" & dv("CM_COY_NAME") & "&POIdx=" & dv("POM_PO_INDEX") & "&pageid=101")
            If IsDBNull(dv("Amount")) Then
                e.Item.Cells(6).Text = "0.00"
            Else
                e.Item.Cells(6).Text = Format(CDbl(dv("Amount")), "###,###,##0.00")
            End If
        End If
    End Sub
    Private Sub dtgOutstdGRNQCVerify_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstdGRNQCVerify.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkGRNNo As HyperLink
            lnkGRNNo = e.Item.Cells(0).FindControl("lnkGRNNo")
            lnkGRNNo.NavigateUrl = dDispatcher.direct("Inventory", "InventoryVerificationDetails.aspx", "Frm=Dashboard&Mode=New&GRNNo=" & dv("IV_GRN_NO") & "&Vendor=" & dv("CM_COY_NAME") & "&GRNDate=" & Common.FormatWheelDate(WheelDateFormat.LongDate, dv("GM_CREATED_DATE")) & "&ReceivedDate=" & Common.FormatWheelDate(WheelDateFormat.LongDate, dv("GM_DATE_RECEIVED")) & "&pageid=34")
            lnkGRNNo.Text = dv("IV_GRN_NO")
            e.Item.Cells(4).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("GM_CREATED_DATE"))
            e.Item.Cells(5).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("GM_DATE_RECEIVED"))
        End If
    End Sub

End Class