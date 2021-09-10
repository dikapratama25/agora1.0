Imports AgoraLegacy
Imports eProcure.Component


Public Class ReturnInwardSearch
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    'Dim blnMsg As Boolean
    Protected WithEvents cmdPrint As System.Web.UI.WebControls.Button

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents txtMRSNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRINo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_startdate As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_enddate As System.Web.UI.WebControls.TextBox
    Protected WithEvents dtg_RIList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents cmdClear As System.Web.UI.WebControls.Button
    Protected WithEvents chkSubmitted As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkAcknowledged As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkRejected As System.Web.UI.WebControls.CheckBox

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Public Enum EnumRIView
        icRINo = 0
        icRIDate = 1
        icTQty = 2
        icStatus = 3
        icAccCode = 4
        icMRSNo = 5
    End Enum

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CheckButtonAccess(True)
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim strStatus2 As String

        MyBase.Page_Load(sender, e)
        SetGridProperty(dtg_RIList)
        If Not Page.IsPostBack Then
            GenerateTab()
            txt_startdate.Text = DateAdd("m", -6, DateTime.Now.ToShortDateString())
            txt_enddate.Text = DateTime.Now.ToShortDateString()
        End If

        intPageRecordCnt = ViewState("intPageRecordCnt")

        If chkSubmitted.Checked Then
            strStatus2 = IIf(strStatus2 = "", RIStatus.Submitted, strStatus2 & "," & RIStatus.Submitted)
        End If

        If chkAcknowledged.Checked Then
            strStatus2 = IIf(strStatus2 = "", RIStatus.Acknowledged, strStatus2 & "," & RIStatus.Acknowledged)
        End If

        If chkRejected.Checked Then
            strStatus2 = IIf(strStatus2 = "", RIStatus.Rejected, strStatus2 & "," & RIStatus.Rejected)
        End If

        If strStatus2 = "" Then
            strStatus2 = RIStatus.Submitted & "," & _
             RIStatus.Acknowledged & "," & _
             RIStatus.Rejected
        End If

        Me.cmdPrint.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewRIListing.aspx", "RINO=" & Me.txtRINo.Text & "&MRSNO=" & Me.txtMRSNo.Text & "&StartDate=" & Me.txt_startdate.Text & "&EndDate=" & Me.txt_enddate.Text & "&Status=" & strStatus2 & "&UserID=" & Session("UserId") & "&CoyID=" & Session("CompanyId")) & "')")

    End Sub

    Public Sub dtg_RIList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtg_RIList.CurrentPageIndex = e.NewPageIndex
        Bindgrid(True)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtg_RIList.CurrentPageIndex = 0
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
            strStatus = IIf(strStatus = "", RIStatus.Submitted, strStatus & "," & RIStatus.Submitted)
        End If

        If chkAcknowledged.Checked Then
            strStatus = IIf(strStatus = "", RIStatus.Acknowledged, strStatus & "," & RIStatus.Acknowledged)
        End If

        If chkRejected.Checked Then
            strStatus = IIf(strStatus = "", RIStatus.Rejected, strStatus & "," & RIStatus.Rejected)
        End If

        If strStatus = "" Then
            strStatus = RIStatus.Submitted & "," & _
             RIStatus.Acknowledged & "," & _
             RIStatus.Rejected
        End If

        ds = objInventory.RIList(txtRINo.Text, txtMRSNo.Text, txt_startdate.Text, txt_enddate.Text, strStatus, "req")

        Dim dvViewPR As DataView
        dvViewPR = ds.Tables(0).DefaultView

        dvViewPR.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewPR.Sort += " DESC"

        ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count

        If ViewState("intPageRecordCnt") > 0 Then
            resetDatagridPageIndex(dtg_RIList, dvViewPR)
            dtg_RIList.DataSource = dvViewPR
            dtg_RIList.DataBind()
            cmdPrint.Visible = True
        Else
            dtg_RIList.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
            cmdPrint.Visible = False
        End If

        ViewState("PageCount") = dtg_RIList.PageCount
    End Function

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtg_RIList.CurrentPageIndex = 0
        ViewState("SortAscending") = "no"
        ViewState("SortExpression") = "IRIM_RI_NO"
        Bindgrid(0)
        'cmdPrint.Visible = True
    End Sub

    Private Sub dtg_RIList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_RIList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
        
            e.Item.Cells(EnumRIView.icRIDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("IRIM_RI_DATE"))

            Dim lnkRINo As HyperLink
            lnkRINo = e.Item.Cells(EnumRIView.icRINo).FindControl("lnkRINo")
            lnkRINo.Text = dv("IRIM_RI_NO")

            lnkRINo.NavigateUrl = dDispatcher.direct("Inventory", "ReturnInwardDetail.aspx", "pageid=" & strPageId & "&RI_NO=" & dv("IRIM_RI_NO") & "&frm=RISearch")

            Dim lnkMRSNo As HyperLink
            lnkMRSNo = e.Item.Cells(EnumRIView.icMRSNo).FindControl("lnkMRSNo")
            lnkMRSNo.Text = dv("IRIM_IR_NO")

            lnkMRSNo.NavigateUrl = dDispatcher.direct("Inventory", "MRSDetail.aspx", "pageid=" & strPageId & "&MRSNo=" & dv("IRIM_IR_NO") & "&caller=RISearch")

            Dim objInv As New Inventory

          
            Dim lnkItemCode As LinkButton

            'lnkItemCode = e.Item.Cells(0).FindControl("lnkItemCode")
            'lnkItemCode.Text = dv("IRIM_RI_NO")
            'lnkItemCode.CommandArgument = dDispatcher.direct("Inventory", "InvTransHistory.aspx", "ItemCode=" & Server.UrlEncode(dv("IRIM_RI_NO")) & "&ItemName=" & Server.UrlEncode(dv("IM_INVENTORY_NAME")) & "&Loc=" & Server.UrlEncode(dv("LM_LOCATION")) & "&SubLoc=" & Server.UrlEncode(Common.parseNull(dv("LM_SUB_LOCATION"))) & "&Qty=" & Server.UrlEncode(dv("ID_INVENTORY_QTY")) & "&InvIndex=" & Server.UrlEncode(dv("ID_INVENTORY_INDEX")) & "&LocIndex=" & Server.UrlEncode(dv("ID_LOCATION_INDEX")) & "&NeedQC=" & dv("IM_IQC_IND") & "&pageid=" & strPageId)

        End If
    End Sub

    Private Sub dtg_RIList_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_RIList.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtg_RIList, e)
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_ReturnInward_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "ReturnInwardListing.aspx", "pageid=" & strPageId) & """><span>Return Inward</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "ReturnInwardSearch.aspx", "type=Listing&pageid=" & strPageId) & """><span>Return Inward Listing</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "</ul><div></div></div>"
    End Sub

    Private Sub cmdClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        Me.txtMRSNo.Text = ""
        Me.txtRIno.Text = ""
        Me.txt_startdate.Text = DateAdd("m", -6, DateTime.Now.ToShortDateString())
        Me.txt_enddate.Text = DateTime.Now.ToShortDateString()
        Me.chkSubmitted.Checked = False
        Me.chkAcknowledged.Checked = False
        Me.chkRejected.Checked = False
    End Sub
   
End Class
