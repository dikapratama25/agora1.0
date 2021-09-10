Imports AgoraLegacy
Imports eProcure.Component
Public Class POViewB3
    Inherits AgoraLegacy.AppBaseClass
    ' Inherits System.Web.UI.Page
    Dim objDb As New EAD.DBCom
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    'Dim v_com_id As String = HttpContext.Current.Session("CompanyId")
    'Dim com_id As String = HttpContext.Current.Session("CompanyId")
    'Dim userid As String = HttpContext.Current.Session("UserId")
    '  Public strPageId As String

    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents LblPrNo As System.Web.UI.WebControls.Label
    Protected WithEvents back As System.Web.UI.HtmlControls.HtmlAnchor
    Dim strCaller As String
    Dim dDispatcher As New AgoraLegacy.dispatcher
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents dtg_POList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents tblSearchResult As System.Web.UI.HtmlControls.HtmlTable

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtg_POList)
        strCaller = UCase(Request.QueryString("caller"))
        Dim objval As New POValue
        viewstate("prid") = Request.QueryString("prid")
        objval.buyer_coy = Request(Trim("BCoyID"))
        'BCoyID = Session("CompanyID")
        lblPRNO.Text = viewstate("prid")
        Dim strsql As String
        Dim Fulfilment As String
        Dim ven_name As String
        Dim PO_Status As String
        Dim ds As New DataSet

        strsql = "select POM_PO_No,POM_S_COY_NAME,CONVERT(varchar(12),POM_PO_DATE,105) AS POM_PO_DATE, CONVERT(VARCHAR(12),POM_ACCEPTED_DATE,105) AS POM_ACCEPTED_DATE," & _
                 " POM_FULFILMENT,POM_PO_STATUS,POM_B_COY_ID,POM_PO_INDEX, ISNULL((SELECT STATUS_DESC FROM STATUS_MSTR B WHERE STATUS_TYPE ='Fulfilment'" & _
                 " AND B.STATUS_NO=POM.POM_FULFILMENT),'-') AS REMARK1," & _
                 " (SELECT STATUS_DESC FROM STATUS_MSTR C WHERE STATUS_TYPE ='PO' AND C.STATUS_NO=POM.POM_PO_STATUS)" & _
                 "  AS STATUS_DESC from po_mstr POM where pom_po_no in " & _
                 " (select distinct(pd.pod_po_no) from po_mstr pm,po_details pd, pr_mstr prm  " & _
                 " where prm.prm_pr_index = pd.POD_PR_INDEX and " & _
                 " pd.pod_po_no = pm.pom_po_no and pd.pod_coy_id = pm.pom_b_coy_id and prm.PRM_PR_NO='" & lblPRNO.Text & "')" & _
                 " and pom_b_coy_id = '" & Request(Trim("BCoyID")) & "'"

        'Dim objPO As New PurchaseOrder
        'ds = objPO.VIEW_POList(strStatus, strFulfilment, Request(Trim("side")), Me.txt_vendor.Text, Me.txt_po_no.Text, Me.txt_startdate.Text, Me.txt_enddate.Text)

        'strsql = "SELECT Distinct POM.POM_S_COY_ID, POM.POM_PO_INDEX,POM.POM_PO_NO,POM.POM_PO_DATE," & _
        '          "POM.POM_PO_STATUS, CM.CM_COY_NAME,POM.POM_BUYER_NAME,POM.POM_S_COY_NAME,POM.POM_ACCEPTED_DATE,POM.POM_B_COY_ID,POM.POM_FULFILMENT , " & _
        '          "ISNULL((SELECT STATUS_DESC FROM STATUS_MSTR B WHERE STATUS_TYPE ='Fulfilment' AND B.STATUS_NO=POM.POM_FULFILMENT),'-') " & _
        '          "AS REMARK1," & _
        '          "(SELECT STATUS_DESC FROM STATUS_MSTR C WHERE STATUS_TYPE ='PO' AND C.STATUS_NO=POM.POM_PO_STATUS) " & _
        '          "AS STATUS_DESC FROM PO_MSTR POM,COMPANY_MSTR CM " & _
        '          "WHERE POM.POM_FULFILMENT IN(" & Fulfilment & ")" & _
        '          " And POM.POM_PO_STATUS IN(" & PO_Status & ") AND CM_COY_ID=POM_B_COY_ID PRM_PR_NO='" & lblPRNO.Text & "'"

        'If side = "u" Then
        'strsql = strsql & " and POM.POM_B_COY_ID = '" & Common.Parse(com_id) & "' and POM_BUYER_ID = '" & Common.Parse(userid) & "'"
        'If ven_name <> "" Then
        '    strsql = strsql & " AND POM.POM_S_COY_NAME " & Common.ParseSQL(ven_name) & " "
        'End If
        ''End If
        'strsql = "select POM_PO_No,POM_S_COY_NAME,POM_PO_DATE,POM_ACCEPTED_DATE,POM_FULFILMENT from po_mstr where pom_po_no in (select distinct(pd.pod_po_no) from po_mstr pm,po_details pd, pr_mstr prm " _
        '          & "where prm.prm_pr_index = pd.POD_PR_INDEX and " _
        '          & "pd.pod_po_no = pm.pom_po_no and " _
        '          & "pd.pod_coy_id = pm.pom_b_coy_id and prm.PRM_PR_NO='" & lblPRNO.Text & "')"

        ds = objDb.FillDs(strsql)
        Dim i As Integer = ds.Tables(0).Rows.Count()
        dtg_POList.DataSource = ds
        dtg_POList.DataBind()
        ds = Nothing
        'lnkBack.NavigateUrl = "javascript:history.back();"
        Dim lnkPRNo As HyperLink
        lnkPRNo.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx")
    End Sub
    Public Enum EnumCRView
        icPONo
        icCreateDate
        icComName
        icAcceptDate
        icStatus
        icFulfilment
        icVIndex
        icVComID
    End Enum
    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtg_POList.CurrentPageIndex = 0
        '   Bindgrid(True)
    End Sub
    Public Sub dtg_POList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtg_POList.CurrentPageIndex = e.NewPageIndex
        '      Bindgrid(True)
    End Sub
    Public Sub cmd_next_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles back.ServerClick

        'Dim strurl As String = Session("strurl")
        'Session("strurl") = ""
        'Session("status_dis") = ""
        'If Session("strurl") = "" Then
        '    'Session("strurl") = "/eProcure/PO/SearchPR_All.aspx?caller=BUYER&pageid=7"
        '    'Michelle (1/8/2007) - To remove the hardcode of 'eProcure'
        '    'strurl = "/eProcure/PO/SearchPR_All.aspx?caller=" & strCaller & "&pageid=" & strPageId & ""
        '    strurl = "../PO/SearchPR_All.aspx?caller=" & strCaller & "&pageid=" & strPageId & ""
        'End If
        'Response.Redirect(strurl)

        '------New Chages to linkback path by praveen on 22.08.2007
        ' Dim strurl As String = Session("strurl")
        ' Session("strurl") = ""
        ' Session("status_dis") = ""
        ' Response.Redirect(strurl)
        '----End the Changes

        ' Michelle (3/9/2007) - To solve the error page when back from POViewB3
        Dim strurl As String = Session("strurl")
        Dim a As String
        If strurl = "" Then
            strurl = dDispatcher.direct("PO", "SearchPR_All.aspx", "caller=" & strCaller & "&pageid=" & strPageId & "")
        End If
        Response.Redirect(strurl)

    End Sub
    Private Sub dtg_POList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_POList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkPRNo As HyperLink
            lnkPRNo = e.Item.Cells(EnumCRView.icPONo).FindControl("lnkPRNo")
            lnkPRNo.Text = dv("POM_PO_No")
            lnkPRNo.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx", "caller=" & strCaller & "&pageid=" & strPageId & "&PO_INDEX=" & dv("POM_PO_INDEX") & "&PRNum=" & ViewState("prid") & "&PO_NO=" & dv("POM_PO_NO") & "&BCoyID=" & dv("POM_B_COY_ID") & "&status=" & dv("POM_PO_STATUS") & "&side=others&filetype=2")
            '   lnkPONo.NavigateUrl = "PODetail.aspx?pageid=" & strPageId & "&PO_INDEX=" & dv("POM_PO_INDEX") & "&PO_NO=" & dv("POM_PO_NO") & "&BCoyID=" & dv("POM_B_COY_ID") & "&status=" & dv("POM_PO_STATUS") & "&side=other&filetype=2&poview=1"

        End If
    End Sub
End Class
