Imports eProcure.Component
Imports AgoraLegacy


Public Class CostAllocMaint
    Inherits AgoraLegacy.AppBaseClass

    Dim dDispatcher As New AgoraLegacy.dispatcher
    Public Enum EnumCL
        icChk = 0
        icCode = 1
        icDesc = 2
        icCoyName = 3
        icStartDate = 4
        icEndDate = 5
    End Enum

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
   

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        'Put user code to initialize the page here
        'MyBase.Page_Load(sender, e)
    
        'Dim objComs As New Companies
        'Dim strCompanyType As String
        'strCompanyType = objComs.GetCompanyType(Session("CompanyIdToken"))
        'If strCompanyType.ToUpper = "BUYER" Or strCompanyType.ToUpper = "BOTH" Then
        SetGridProperty(dtgCostAllocMtn)
        GenerateTab()
        If Not IsPostBack Then
            Bindgrid()

            'cmdModify.Visible = False
            'cmdDelete.Visible = False

            cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
            cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
        Else
            cmdModify.Visible = True
            cmdDelete.Visible = True
        End If

        cmdAdd.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("IPPAdmin", "AddCostAlloc.aspx", "&mode=add&pageid=" & strPageId) & "')")

    End Sub

    Private Function Bindgrid() As String
        Dim objCA As New IPP
        Dim ds As New DataSet


        ds = objCA.GetCostAlloc(txtCode.Text, txtDesc.Text)
        intPageRecordCnt = ds.Tables(0).Rows.Count

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        ''//these only needed if you can select a grid item and click delete button
        ''//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        ''//then user delete one record. //total record = 20 (2 pages), 
        ''//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If ViewState("action") = "del" Then
            If dtgCostAllocMtn.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgCostAllocMtn.PageSize = 0 Then
                dtgCostAllocMtn.CurrentPageIndex = dtgCostAllocMtn.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If

        If intPageRecordCnt > 0 Then


            cmdModify.Enabled = True
            cmdDelete.Enabled = True
            resetDatagridPageIndex(dtgCostAllocMtn, dvViewSample)
            dtgCostAllocMtn.DataSource = dvViewSample
            dtgCostAllocMtn.DataBind()
        Else
            cmdModify.Enabled = False
            cmdDelete.Enabled = False
            Common.NetMsgbox(Me, MsgNoRecord)
            dtgCostAllocMtn.DataBind()
            cmdModify.Visible = False
            cmdDelete.Visible = False
        End If
        ' add for above checking
        ViewState("PageCount") = dtgCostAllocMtn.PageCount
        objCA = Nothing
    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgCostAllocMtn.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub dtgCostAllocMtn_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCostAllocMtn.ItemCreated
        Grid_ItemCreated(dtgCostAllocMtn, e)
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If


    End Sub

    Private Sub dtgCostAllocMtn_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCostAllocMtn.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            Dim lblCode As Label
            lblCode = e.Item.FindControl("lblCode")
            lblCode.Text = dv("CAM_CA_CODE")

            Dim lblIndex As Label
            lblIndex = e.Item.FindControl("lblIndex")
            lblIndex.Text = dv("CAM_INDEX")

            
            
        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        ViewState("SortExpression") = "CAM_CA_CODE"
        ViewState("SortAscending") = "yes"
        dtgCostAllocMtn.CurrentPageIndex = 0
        Bindgrid()

    End Sub

    Private Sub dtgCostAllocMtn_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgCostAllocMtn.PageIndexChanged
        dtgCostAllocMtn.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub



    Private Sub cmdModify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdModify.Click
        Dim dgItem As DataGridItem
        Dim chkItem As CheckBox

        For Each dgItem In dtgCostAllocMtn.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                Dim strFileName As String
                Dim strscript As New System.Text.StringBuilder
                strscript.Append("<script language=""javascript"">")
                strFileName = dDispatcher.direct("IPPAdmin", "AddCostAlloc.aspx", "&mode=mod&index=" & CType(dgItem.FindControl("lblIndex"), Label).Text & "&cacode=" & CType(dgItem.FindControl("lblCode"), Label).Text & "&pageid=" & strPageId)
                'strFileName = Replace(Server.UrlEncode(strFileName), "'", "%27")
                strscript.Append("PopWindow('" & strFileName & "');")
                strscript.Append("</script>")
                RegisterStartupScript("script13", strscript.ToString())
            End If
        Next
    End Sub



    Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim dgItem As DataGridItem
        Dim objCA As New IPP
        Dim chkItem As CheckBox
        Dim dsCA As New DataSet
        Dim dtCA As New DataTable
        dtCA.Columns.Add("index", Type.GetType("System.String"))
        dtCA.Columns.Add("CACode", Type.GetType("System.String"))
        dtCA.Columns.Add("CoyId", Type.GetType("System.String"))
        dtCA.Columns.Add("UsrId", Type.GetType("System.String"))

        For Each dgItem In dtgCostAllocMtn.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                Dim dtr As DataRow
                dtr = dtCA.NewRow()
                dtr("index") = CType(dgItem.FindControl("lblIndex"), Label).Text
                dtr("CACode") = CType(dgItem.FindControl("lblCode"), Label).Text
                dtr("CoyId") = Session("CompanyId") 'dgItem.Cells(6).Text 
                dtr("UsrId") = Session("UserId")
                dtCA.Rows.Add(dtr)

            End If
        Next
        dsCA.Tables.Add(dtCA)

        If objCA.DelCostAllocCode(dtCA) Then
            objCA.Message(Me, "00004", MsgBoxStyle.Information)
        Else
            'strMsg = "Deletion is not allowed.""&vbCrLf&""It has outstanding PR(s)."
            'Common.NetMsgbox(Me, MsgRecordNotDelete, MsgBoxStyle.Information, "Wheel")
            objCA.Message(Me, "00018", MsgBoxStyle.Information)
        End If
        ViewState("action") = "del"
        Bindgrid()
    End Sub



    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("CostAlloc_tabs") = "<div class=""t_entity""><ul>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("IPPAdmin", "CostAllocMaint.aspx", "pageid=" & strPageId) & """><span>Cost Alloc. Code</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPPAdmin", "CostAllocDetail.aspx", "pageid=" & strPageId) & """><span>Cost Alloc. Details</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "</ul><div></div></div>"

    End Sub


End Class