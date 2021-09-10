Imports AgoraLegacy
Imports eProcure.Component
Public Class SearchPR_Cancellation
    Inherits AgoraLegacy.AppBaseClass

    Dim dDispatcher As New AgoraLegacy.dispatcher

    Public Enum EnumPR
        icPRNo
        icPRType
        icCreationDate
        icSubmissionDate
    End Enum

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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetGridProperty(dtgPRList)

        MyBase.Page_Load(sender, e)
        If Not Page.IsPostBack Then
            GenerateTab()
        End If

        Session("urlreferer") = "PRCancel"
        intPageRecordCnt = ViewState("intPageRecordCnt")
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_SearchPR_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PR", "RaisePR.aspx", "pageid=" & strPageId & "&type=new&mode=bc&frm=bc") & """><span>Purchase Request</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId) & """><span>Purchase Request Listing</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PR", "SearchPR_Cancellation.aspx", "pageid=" & strPageId) & """><span>Purchase Request Cancellation</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "</ul><div></div></div>"
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
        Dim strStatus As String = ""
        Dim ds As DataSet
        Dim objPR As New PurchaseReq2
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

        ds = objPR.SearchPRCancelList(txtPRNo.Text, txtDateFr.Text, txtDateTo.Text, strPRType)

        Dim dvViewPR As DataView
        dvViewPR = ds.Tables(0).DefaultView
        dvViewPR.Sort = ViewState("SortExpression")

        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewPR.Sort += " DESC"

        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgPRList, dvViewPR)
            dtgPRList.DataSource = dvViewPR
            dtgPRList.DataBind()
        Else
            dtgPRList.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        ViewState("PageCount") = dtgPRList.PageCount
    End Function

    Private Sub dtgPRList_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPRList.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgPRList, e)
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgPRList.CurrentPageIndex = 0
        viewstate("SortAscending") = "no"
        ViewState("SortExpression") = "PRM_CREATED_DATE"
        Bindgrid(True)
    End Sub

    Private Sub dtgPRList_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPRList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkPRNo As HyperLink

            lnkPRNo = e.Item.Cells(EnumPR.icPRNo).FindControl("lnkPRNo")
            lnkPRNo.NavigateUrl = dDispatcher.direct("PR", "PRDetail.aspx", "pageid=" & strPageId & "&index=" & dv("PRM_PR_Index") & "&PRNO=" & dv("PRM_PR_No") & "&type=mod&mode=bc")
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

            e.Item.Cells(EnumPR.icCreationDate).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("PRM_CREATED_DATE"))
            If Not IsDBNull(dv("PRM_SUBMIT_DATE")) Then
                e.Item.Cells(EnumPR.icSubmissionDate).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("PRM_SUBMIT_DATE"))
            End If

            If Not IsDBNull(dv("PRM_PR_TYPE")) Then
                If dv("PRM_PR_TYPE") = "CC" Then
                    e.Item.Cells(EnumPR.icPRType).Text = "Contract"

                Else
                    e.Item.Cells(EnumPR.icPRType).Text = "Non-Contract"

                End If
            Else
                e.Item.Cells(EnumPR.icPRType).Text = "Non-Contract"
            End If
        End If

    End Sub
End Class