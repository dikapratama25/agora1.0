'Jules 2015.02.02 Agora Stage 2
Imports AgoraLegacy
Imports eProcure.Component

Public Class DebitNoteListing
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objGlobal As New AppGlobals

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents txtDNNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtInvNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents chk_New As System.Web.UI.WebControls.CheckBox
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents Table4 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSearchResult As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents chk_Pending As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chk_paid As System.Web.UI.WebControls.CheckBox
    Protected WithEvents dtg_DNList As System.Web.UI.WebControls.DataGrid
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

    Public Enum EnumDNSearch
        icDNNo = 0
        icDNCreatedDate = 1
        icInvNo = 2
        icCoyName = 3
        icCurrency = 4
        icDNTotal = 5
        icStatus = 6
    End Enum

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CheckButtonAccess(True)
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        blnCheckBox = False
        SetGridProperty(dtg_DNList)
        GenerateTab()
        If Not IsPostBack Then
            'Me.lblTitle.Text = "Invoice"
            txt_startdate.Text = DateAdd("m", -6, DateTime.Now.ToShortDateString())
            txt_enddate.Text = DateTime.Now.ToShortDateString()
        End If

        intPageRecordCnt = ViewState("intPageRecordCnt")
    End Sub

    Public Sub dtg_DNList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtg_DNList.CurrentPageIndex = e.NewPageIndex
        Bindgrid(True)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtg_DNList.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objINV As New Invoice
        Dim objDN As New DebitNote

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

        Dim dn_no, inv, com_id, strDate, endDate, strMsg As String

        Dim comparedt As Date
        'comparedt = DateAdd("m", -6, DateTime.Now.ToShortDateString())
        comparedt = DateAdd("m", -6, CDate(txt_enddate.Text))

        If CDate(txt_startdate.Text) < comparedt Then
            strMsg = "Start date " & objGlobal.GetErrorMessage("00015") & " 6 months"
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
            Return False
        End If

        'If txt_startdate.Text <> "" Then
        '    strDate = Format(CDate(txt_startdate.Text), "MM/dd/yyyy")
        'End If

        'If txt_enddate.Text <> "" Then
        '    endDate = Format(CDate(txt_enddate.Text), "MM/dd/yyyy")
        'End If
        dn_no = txtDNNo.Text
        inv = txtInvNo.Text
        'com_id = txtInvNo.Text

        'Jules 2015.02.22 - Agora Stage 2
        ds = objDN.getDebitNoteView(dn_no, inv, strStatus, com_id, txt_startdate.Text, txt_enddate.Text)

        '//for sorting asc or desc
        Dim dvViewPR As DataView
        dvViewPR = ds.Tables(0).DefaultView
        dvViewPR.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewPR.Sort += " DESC"

        ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count

        '//bind datagrid
        If ViewState("intPageRecordCnt") > 0 Then
            resetDatagridPageIndex(dtg_DNList, dvViewPR)
            dtg_DNList.DataSource = dvViewPR
            dtg_DNList.DataBind()
            'dtg_DNList.Columns(EnumDNSearch.icStatus).Visible = False 'Jules 2015-Feb-23 Agora Stage 2
        Else
            dtg_DNList.DataBind()
            Common.NetMsgbox(Me, "No record found.")
        End If
        ' add for above checking
        ViewState("PageCount") = dtg_DNList.PageCount
    End Function

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        'Check date range. Must be within 6 months
        'If txt_startdate.Text <> "" And txt_enddate.Text <> "" Then

        'End If

        dtg_DNList.CurrentPageIndex = 0
        ViewState("SortAscending") = "no"
        ViewState("SortExpression") = "DNM_CREATED_DATE"
        Bindgrid()
    End Sub

    'Private Sub cmd_createInv_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_createInv.Click
    '    'Response.Redirect("InvGeneration1.aspx?pageid=" & strPageId)
    '    Response.Redirect("InvList.aspx?pageid=" & strPageId)
    'End Sub

    Private Sub dtg_DNList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_DNList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box

            e.Item.Cells(EnumDNSearch.icDNNo).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewDebitNote.aspx", "pageid=" & strPageId & "&DN_No=" & Common.parseNull(dv("DNM_DN_NO")) & "&BCoyID=" & Common.parseNull(dv("DNM_DN_B_COY_ID")) & "&SCoyID=" & Common.parseNull(dv("DNM_DN_S_COY_ID")) & "')"" ><font color=#0000ff>" & Common.parseNull(dv("DNM_DN_NO"))) & "</font></A>"
            e.Item.Cells(EnumDNSearch.icDNCreatedDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("DNM_CREATED_DATE"))
            e.Item.Cells(EnumDNSearch.icInvNo).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewInvoice.aspx", "pageid=" & strPageId & "&INVNO=" & Common.parseNull(dv("DNM_INV_NO")) & "&vcomid=" & Common.parseNull(dv("DNM_DN_S_COY_ID")) & "&BCoyID=" & Common.parseNull(dv("DNM_DN_B_COY_ID"))) & "')"" ><font color=#0000ff>" & Common.parseNull(dv("DNM_INV_NO")) & "</font></A>" 'Jules 2015-Feb-23 Agora Stage 2
            e.Item.Cells(EnumDNSearch.icDNTotal).Text = Format(Common.parseNull(dv("DNM_DN_TOTAL")), "#,##0.00")
        End If
    End Sub

    Private Sub dtg_DNList_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_DNList.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")        
        Grid_ItemCreated(dtg_DNList, e)
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
        Session("w_Debit_Credit_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                           "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitCreditNoteList.aspx", "pageid=" & strPageId) & """><span>Debit Note / Credit Note</span></span></a></li>" & _
                           "<li><div class=""space""></div></li>" & _
                           "<li><a class=""t_entity_btn_selected"" href=""#""><span>Debit Note Listing</span></a></li>" & _
                           "<li><div class=""space""></div></li>" & _
                           "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("CreditNote", "CreditNoteListing.aspx", "pageid=" & strPageId) & """><span>Credit Note Listing</span></a></li>" & _
                           "<li><div class=""space""></div></li>" & _
                          "</ul><div></div></div>"

    End Sub

    Private Sub cmdClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        Me.txtDNNo.Text = ""
        Me.txtInvNo.Text = ""
        Me.txt_startdate.Text = DateAdd("m", -6, DateTime.Now.ToShortDateString())
        Me.txt_enddate.Text = DateTime.Now.ToShortDateString()
        Me.chk_New.Checked = False
        Me.chk_Pending.Checked = False
        Me.chk_Approved.Checked = False
        Me.chk_paid.Checked = False
    End Sub
End Class
