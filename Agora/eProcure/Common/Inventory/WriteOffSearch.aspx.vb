Imports AgoraLegacy
Imports eProcure.Component


Public Class WriteOffSearch
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    'Dim blnMsg As Boolean

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents txtWONo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_startdate As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_enddate As System.Web.UI.WebControls.TextBox
    Protected WithEvents dtg_WOList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents cmdClear As System.Web.UI.WebControls.Button
    Protected WithEvents chkSubmitted As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkPendingApproval As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkApproved As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkCancelled As System.Web.UI.WebControls.CheckBox
    'Protected WithEvents chkRejected As System.Web.UI.WebControls.CheckBox
    Protected WithEvents txtItemCode As System.Web.UI.WebControls.TextBox

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Public Enum EnumWOView
        icWONo = 0
        icWODate = 1
        icTQty = 2
        icStatus = 3
    End Enum

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        'CheckButtonAccess(True)
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtg_WOList)
        If Not Page.IsPostBack Then
            GenerateTab()
            txt_startdate.Text = DateAdd("m", -6, DateTime.Now.ToShortDateString())
            txt_enddate.Text = DateTime.Now.ToShortDateString()
        End If

        intPageRecordCnt = ViewState("intPageRecordCnt")
    End Sub

    Public Sub dtg_WOList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtg_WOList.CurrentPageIndex = e.NewPageIndex
        Bindgrid(True)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtg_WOList.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        ''//Retrieve Data from Database
        Dim strMsg As String
        Dim objGlobal As New AppGlobals
        Dim comparedt As Date
        comparedt = DateAdd("m", -6, CDate(txt_enddate.Text))

        If CDate(txt_startdate.Text) < comparedt Then
            strMsg = "Start/ End date " & objGlobal.GetErrorMessage("00015") & " 6 months"
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
            Return False
        End If

        Dim ds As New DataSet
        Dim objInventory As New Inventory
        Dim strStatus As String = ""
        If chkSubmitted.Checked Then
            strStatus = IIf(strStatus = "", WOStatus.Submitted, strStatus & "," & WOStatus.Submitted)
        End If

        'If chkPendingApproval.Checked Then
        '    strStatus = IIf(strStatus = "", WOStatus.PendingApproval, strStatus & "," & WOStatus.PendingApproval)
        'End If

        'If chkApproved.Checked Then
        '    strStatus = IIf(strStatus = "", WOStatus.Approved, strStatus & "," & WOStatus.Approved)
        'End If

        If chkCancelled.Checked Then
            strStatus = IIf(strStatus = "", WOStatus.Cancelled, strStatus & "," & WOStatus.Cancelled)
        End If

        'If chkRejected.Checked Then
        '    strStatus = IIf(strStatus = "", WOStatus.Rejected, strStatus & "," & WOStatus.Rejected)
        'End If

        If strStatus = "" Then
            strStatus = WOStatus.Submitted & "," & _
            WOStatus.Cancelled
        End If

        ds = objInventory.WOList(txtWONo.Text, txt_startdate.Text, txt_enddate.Text, strStatus, txtItemCode.Text)

        Dim dvViewPR As DataView
        dvViewPR = ds.Tables(0).DefaultView

        dvViewPR.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewPR.Sort += " DESC"

        ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count

        If ViewState("intPageRecordCnt") > 0 Then
            resetDatagridPageIndex(dtg_WOList, dvViewPR)
            dtg_WOList.DataSource = dvViewPR
            dtg_WOList.DataBind()
        Else
            dtg_WOList.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        ViewState("PageCount") = dtg_WOList.PageCount
    End Function

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtg_WOList.CurrentPageIndex = 0
        ViewState("SortAscending") = "no"
        ViewState("SortExpression") = "IWOM_WO_NO"
        Bindgrid(0)
    End Sub

    Private Sub dtg_WOList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_WOList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            e.Item.Cells(EnumWOView.icWODate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("IWOM_WO_DATE"))

            If e.Item.Cells(EnumWOView.icStatus).Text = "Submitted for Approval" Then
                e.Item.Cells(EnumWOView.icStatus).Text = "Submitted"
            End If

            Dim lnkWONo As HyperLink
            lnkWONo = e.Item.Cells(EnumWOView.icWONo).FindControl("lnkWONo")
            lnkWONo.Text = dv("IWOM_WO_NO")

            lnkWONo.NavigateUrl = dDispatcher.direct("Inventory", "WriteOffDetail.aspx", "pageid=" & strPageId & "&WONO=" & dv("IWOM_WO_NO") & "&frm=WOSearch")
        End If
    End Sub

    Private Sub dtg_WOList_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_WOList.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtg_WOList, e)
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_WriteOffSearch_tabs") = "<div class=""t_entity""><ul>" & _
                "<li><div class=""space""></div></li>" & _
                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "WriteOff.aspx", "pageid=" & strPageId) & """><span>Write Off</span></a></li>" & _
                "<li><div class=""space""></div></li>" & _
                "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "WriteOffSearch.aspx", "") & """><span>Write Off Listing</span></a></li>" & _
                "<li><div class=""space""></div></li>" & _
                "</ul><div></div></div>"
    End Sub

    Private Sub cmdClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        Me.txtWONo.Text = ""
        Me.txt_startdate.Text = DateAdd("m", -6, DateTime.Now.ToShortDateString())
        Me.txt_enddate.Text = DateTime.Now.ToShortDateString()
        Me.chkSubmitted.Checked = False
        'Me.chkPendingApproval.Checked = False
        'Me.chkApproved.Checked = False
        Me.chkCancelled.Checked = False
        'Me.chkRejected.Checked = False
        Me.txtItemCode.Text = ""
    End Sub
End Class
