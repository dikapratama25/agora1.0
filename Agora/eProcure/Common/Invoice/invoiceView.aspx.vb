Imports AgoraLegacy
Imports eProcure.Component

Public Class invoiceView1
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents txt_po_no As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_CRNO As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents chk_New As System.Web.UI.WebControls.CheckBox
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents Table4 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSearchResult As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents chk_Pending As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chk_paid As System.Web.UI.WebControls.CheckBox
    'Protected WithEvents cmd_createInv As System.Web.UI.WebControls.Button
    Protected WithEvents dtg_InvList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents chk_Approved As System.Web.UI.WebControls.CheckBox

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region


    Public Enum EnumInvSearch
        icInvNo = 0
        icCreateDate = 1
        icPONo = 2
        icCoyName = 3
        icCurrCode = 4
        icAmount = 5
        icStatus = 6
    End Enum

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CheckButtonAccess(True)
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        blnCheckBox = False
        SetGridProperty(dtg_InvList)
        GenerateTab()
        'If Not IsPostBack Then
        '    Me.lblTitle.Text = "Invoice"
        'End If
        intPageRecordCnt = ViewState("intPageRecordCnt")
    End Sub

    Public Sub dtg_InvList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtg_InvList.CurrentPageIndex = e.NewPageIndex
        Bindgrid(True)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtg_InvList.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objINV As New Invoice

        '//Retrieve Data from Database
        Dim ds As DataSet
        Dim strStatus As String = ""
        Dim NewInv As Integer = invStatus.NewInv
        Dim Approved As Integer = invStatus.Approved
        Dim Paid As Integer = invStatus.Paid
        Dim PendingAppr As Integer = invStatus.PendingAppr

        '-- start cons strstatus --
        If Me.chk_New.Checked Then
            strStatus = IIf(strStatus = "", invStatus.NewInv, strStatus & "," & invStatus.NewInv)
        End If

        If Me.chk_Approved.Checked Then
            strStatus = IIf(strStatus = "", invStatus.Approved, strStatus & "," & invStatus.Approved)
        End If

        If Me.chk_paid.Checked Then
            strStatus = IIf(strStatus = "", invStatus.Paid, strStatus & "," & invStatus.Paid)
        End If

        If Me.chk_Pending.Checked Then
            strStatus = IIf(strStatus = "", invStatus.PendingAppr, strStatus & "," & invStatus.PendingAppr)
        End If
        ' -- con end 

        Dim inv, com_id As String
        inv = txt_po_no.Text
        com_id = txt_CRNO.Text

        ds = objINV.get_invoiceview(strStatus, com_id, inv)

        '//for sorting asc or desc
        Dim dvViewPR As DataView
        dvViewPR = ds.Tables(0).DefaultView
        dvViewPR.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewPR.Sort += " DESC"

        ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count

        '//bind datagrid
        If ViewState("intPageRecordCnt") > 0 Then
            resetDatagridPageIndex(dtg_InvList, dvViewPR)
            dtg_InvList.DataSource = dvViewPR
            dtg_InvList.DataBind()
        Else
            dtg_InvList.DataBind()
            Common.NetMsgbox(Me, "No record found.")
        End If
        ' add for above checking
        ViewState("PageCount") = dtg_InvList.PageCount
    End Function

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtg_InvList.CurrentPageIndex = 0
        ViewState("SortAscending") = "no"
        ViewState("SortExpression") = "IM_CREATED_ON"
        Bindgrid()
    End Sub

    'Private Sub cmd_createInv_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_createInv.Click
    '    'Response.Redirect("InvGeneration1.aspx?pageid=" & strPageId)
    '    Response.Redirect("InvList.aspx?pageid=" & strPageId)
    'End Sub

    Private Sub dtg_InvList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_InvList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box

            e.Item.Cells(EnumInvSearch.icCreateDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("IM_CREATED_ON"))
            'e.Item.Cells(EnumInvSearch.icInvNo).Text = "<A href=""#"" onclick=""PopWindow('ViewInvoice.aspx?pageid=" & strPageId & "&INVNO=" & Common.parseNull(dv("IM_INVOICE_NO")) & "&vcomid=" & Common.parseNull(dv("POM_S_COY_ID")) & "&BCoyID=" & Common.parseNull(dv("POM_B_COY_ID")) & "')"" ><font color=#0000ff>" & Common.parseNull(dv("IM_INVOICE_NO")) & "</font></A>"
            e.Item.Cells(EnumInvSearch.icInvNo).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewInvoice.aspx", "pageid=" & strPageId & "&INVNO=" & Common.parseNull(dv("IM_INVOICE_NO")) & "&vcomid=" & Common.parseNull(dv("POM_S_COY_ID")) & "&BCoyID=" & Common.parseNull(dv("POM_B_COY_ID"))) & "')"" ><font color=#0000ff>" & Common.parseNull(dv("IM_INVOICE_NO")) & "</font></A>"
            'e.Item.Cells(EnumInvSearch.icPONo).Text = "<A href=""#"" onclick=""PopWindow('../PO/POReport.aspx?pageid=" & strPageId & "&po_no=" & Common.parseNull(dv("POM_PO_NO")) & "&BCoyID=" & Common.parseNull(dv("POM_B_COY_ID")) & "')"" ><font color=#0000ff>" & Common.parseNull(dv("POM_PO_NO")) & "</font></A>"
            e.Item.Cells(EnumInvSearch.icPONo).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewPO.aspx", "pageid=" & strPageId & "&PO_No=" & Common.parseNull(dv("POM_PO_NO")) & "&BCoyID=" & Common.parseNull(dv("POM_B_COY_ID")) & "')"" ><font color=#0000ff>" & Common.parseNull(dv("POM_PO_NO"))) & "</font></A>"

            e.Item.Cells(EnumInvSearch.icAmount).Text = Format(Common.parseNull(dv("IM_INVOICE_TOTAL")), "#,##0.00")
        End If
    End Sub

    Private Sub dtg_InvList_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_InvList.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtg_InvList, e)
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        'Session("w_SearchGInv_tabs") = "<div class=""t_entity""><ul>" & _
        '    "<li><div class=""space""></div></li>" & _
        '                    "<li><a class=""t_entity_btn"" href=""InvList.aspx?pageid=" & strPageId & """><span>Issue Invoice</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                    "<li><a class=""t_entity_btn_selected"" href=""invoiceView.aspx?pageid=" & strPageId & """><span>Invoice Listing</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                     "</ul><div></div></div>"
        Session("w_SearchGInv_tabs") = "<div class=""t_entity""><ul>" &
        "<li><div class=""space""></div></li>" &
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Invoice", "InvList.aspx", "pageid=" & strPageId) & """><span>Issue Invoice</span></a></li>" &
                    "<li><div class=""space""></div></li>" &
                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Invoice", "InvoiceView.aspx", "pageid=" & strPageId) & """><span>Invoice Listing</span></a></li>" &
                    "<li><div class=""space""></div></li>" &
                     "</ul><div></div></div>"

    End Sub
End Class
