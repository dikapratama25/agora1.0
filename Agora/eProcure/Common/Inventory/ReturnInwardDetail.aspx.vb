Imports AgoraLegacy
Imports eProcure.Component

Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient

Public Class ReturnInwardDetail
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents dtgRIDtl As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblRINo As System.Web.UI.WebControls.Label
    Protected WithEvents lblMRSNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblRIDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblStatus As System.Web.UI.WebControls.Label
    Protected WithEvents txtRemarkCR As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdView As System.Web.UI.WebControls.Button

    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim strFrm, strRINo As String
    Dim objINV As New Inventory
    Dim dsAllInfo, ds As DataSet

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
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        htPageAccess.Add("update", alButtonList)
        htPageAccess.Add("add", alButtonList)
        If Request.QueryString("frm") <> "Dashboard" Then
            CheckButtonAccess()
        End If
        If Not ViewState("blnButtonState") Then
            ButtonProperty(False)
        End If
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        blnPaging = False
        SetGridProperty(dtgRIDtl)

        strPageId = Session("strPageId")
        strRINo = Me.Request.QueryString("RI_NO")
        strFrm = Me.Request.QueryString("frm")
        If strFrm = "RISearch" Then
            lnkBack.NavigateUrl = dDispatcher.direct("Inventory", "ReturnInwardSearch.aspx", "pageid=" & strPageId)
        ElseIf strFrm = "RIAckSearch" Then
            lnkBack.NavigateUrl = dDispatcher.direct("Inventory", "ReturnInwardAckSearch.aspx", "pageid=" & strPageId)
        ElseIf strFrm = "Dashboard" Then
            lnkBack.NavigateUrl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "frm=Dashboard&pageid=0")
        End If
        lblRINo.Text = strRINo

        If Not Page.IsPostBack Then
            dsAllInfo = objINV.RIInfo(lblRINo.Text)
            PopulateRIHeader()
            GenerateTab()
            Bindgrid(True)
        End If

        Me.cmdView.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewRI.aspx", "RI_No=" & Request(Trim("RI_NO")) & "&CoyID=" & Session("CompanyID") & "") & "')")
    End Sub

    Sub ButtonProperty(ByVal blnEnable As Boolean)
        ViewState("blnButtonState") = blnEnable
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgRIDtl.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub dtgMRSDtl_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgRIDtl.ItemCreated
        Grid_ItemCreated(dtgRIDtl, e)
    End Sub

    Private Sub dtgRIDtl_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgRIDtl.ItemDataBound
        Dim dsRIDtl As DataSet
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            dsRIDtl = Nothing
        End If
    End Sub

    Public Sub dtgRIDtl_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgRIDtl.ItemCommand
        Dim strscript As New System.Text.StringBuilder
        Dim strFileName As String
        Dim strName As String
        Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

        If (e.CommandSource).CommandName = "" Then
            strFileName = ""
            strName = ""
        End If
    End Sub

    Private Sub PopulateRIHeader()
        Dim strMsg As String

        Dim dtHeader As New DataTable
        dtHeader = dsAllInfo.Tables("INVENTORY_RETURN_INWARD_MSTR_DETAILS")

        If dtHeader.Rows.Count > 0 Then
            lblRIDate.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dtHeader.Rows(0)("IRIM_RI_DATE"))
            lblStatus.Text = dtHeader.Rows(0)("IRIM_RI_STATUS")
            lblMRSNo.Text = dtHeader.Rows(0)("IRIM_IR_NO")
            txtRemarkCR.Text = Common.parseNull(dtHeader.Rows(0)("IRIM_RI_REJECT_REMARK"))
        End If
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        ''//Retrieve Data from Database
        Dim strMsg As String
        Dim objGlobal As New AppGlobals
        Dim ds As New DataSet

        Dim dvViewPR As DataView
        dvViewPR = dsAllInfo.Tables("INVENTORY_RETURN_INWARD_MSTR_DETAILS").DefaultView

        dvViewPR.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewPR.Sort += " DESC"

        ViewState("intPageRecordCnt") = dsAllInfo.Tables("INVENTORY_RETURN_INWARD_MSTR_DETAILS").Rows.Count

        If ViewState("intPageRecordCnt") > 0 Then
            'resetDatagridPageIndex(dtgMRSDtl, dvViewPR)
            dtgRIDtl.DataSource = dvViewPR
            dtgRIDtl.DataBind()
        Else
            dtgRIDtl.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        'ViewState("PageCount") = dtgMRSDtl.PageCount
    End Function

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        If strFrm = "RISearch" Then
            Session("w_ReturnInward_tabs") = "<div class=""t_entity""><ul>" & _
                "<li><div class=""space""></div></li>" & _
                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "ReturnInwardListing.aspx", "pageid=" & strPageId) & """><span>Return Inward</span></a></li>" & _
                "<li><div class=""space""></div></li>" & _
                "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "ReturnInwardSearch.aspx", "type=Listing&pageid=" & strPageId) & """><span>Return Inward Listing</span></a></li>" & _
                "<li><div class=""space""></div></li>" & _
                "</ul><div></div></div>"
        ElseIf strFrm = "RIAckSearch" Then
            Session("w_ReturnInward_tabs") = "<div class=""t_entity""><ul>" & _
                "<li><div class=""space""></div></li>" & _
                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "ReturnInwardAckListing.aspx", "pageid=" & strPageId) & """><span>Return Inward Ack</span></a></li>" & _
                "<li><div class=""space""></div></li>" & _
                "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "ReturnInwardAckSearch.aspx", "type=Listing&pageid=" & strPageId) & """><span>Return Inward Ack/Rejected Listing</span></a></li>" & _
                "<li><div class=""space""></div></li>" & _
                "</ul><div></div></div>"
        End If


    End Sub
End Class
