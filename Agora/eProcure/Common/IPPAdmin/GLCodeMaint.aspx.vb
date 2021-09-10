Imports eProcure.Component
Imports AgoraLegacy


Public Class GLCodeMtn
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID") 'Modified for IPP GST Stage 2A

    Public Enum EnumGL
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
    'Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    'Protected WithEvents txtCode As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtVendor As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtDateFr As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtDateTo As System.Web.UI.WebControls.TextBox
    'Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    'Protected WithEvents dtgGLCode As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents txtDesc As System.Web.UI.WebControls.TextBox
    'Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents trContract As System.Web.UI.HtmlControls.HtmlTableRow
    'Protected WithEvents trDiscount As System.Web.UI.HtmlControls.HtmlTableRow
    'Protected WithEvents cmdAdd As System.Web.UI.HtmlControls.HtmlInputButton 'System.Web.UI.WebControls.Button
    'Protected WithEvents cmdModify As System.Web.UI.WebControls.Button
    'Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    'Protected WithEvents lblCodeLabel As System.Web.UI.WebControls.Label
    'Protected WithEvents lbl1 As System.Web.UI.WebControls.Label
    'Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    'Protected WithEvents cvDate As System.Web.UI.WebControls.CompareValidator
    'Protected WithEvents btnHidden As System.Web.UI.WebControls.Button

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
        strNewCSS = "true"
        MyBase.Page_Load(sender, e)

        'Dim objComs As New Companies
        'Dim strCompanyType As String
        'strCompanyType = objComs.GetCompanyType(Session("CompanyIdToken"))
        'If strCompanyType.ToUpper = "BUYER" Or strCompanyType.ToUpper = "BOTH" Then
        SetGridProperty(dtgGLCode)
        'GenerateTab()
        If Not IsPostBack Then
            cmdModify.Visible = False
            cmdDelete.Visible = False
            ' ViewState("cattype") = "C"
            cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
            cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")

            'Chee Hong - 17/12/2014 - Only allow HLB IPP Admin for action (IPP GST Stage 2A)
            If strDefIPPCompID <> "" And strDefIPPCompID <> Session("CompanyId") Then
                cmdAdd.Visible = False
            Else
                cmdAdd.Visible = True
            End If
        Else
            cmdModify.Visible = True
            cmdDelete.Visible = True
        End If

        'Else
        'ViewState("cattype") = "C" ' Request.QueryString("cattype")
        Dim strMsg As String
        'Dim objCat As New ContCat
        'strMsg = objCat.BuyerDisallowedMsg1(ViewState("cattype"), False)
        'Common.NetMsgbox(Me, strMsg, dDispatcher.direct("Initial", "Homepage.aspx"), MsgBoxStyle.Exclamation)
        'End If
        cmdAdd.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("IPPAdmin", "AddGLCode.aspx", "cattype=" & ViewState("cattype") & "&mode=add&pageid=" & strPageId) & "')")

    End Sub

    Private Function Bindgrid() As String
        Dim objGLCode As New IPP
        Dim ds As New DataSet
        Dim strStatus As String

        strStatus = rdbStatus.SelectedItem.Value

        ds = objGLCode.GetGLCode(Common.Parse(txtCode.Text), Common.Parse(txtDesc.Text), strStatus)
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
            If dtgGLCode.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgGLCode.PageSize = 0 Then
                dtgGLCode.CurrentPageIndex = dtgGLCode.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If

        If intPageRecordCnt > 0 Then
            cmdModify.Enabled = True
            cmdDelete.Enabled = True
            resetDatagridPageIndex(dtgGLCode, dvViewSample)
            dtgGLCode.DataSource = dvViewSample
            dtgGLCode.DataBind()
        Else
            cmdModify.Enabled = False
            cmdDelete.Enabled = False
            Common.NetMsgbox(Me, MsgNoRecord)
            dtgGLCode.DataBind()
        End If

        ' add for above checking
        ViewState("PageCount") = dtgGLCode.PageCount
        objGLCode = Nothing

        'Chee Hong - 17/12/2014 - Only allow HLB IPP Admin for action (IPP GST Stage 2A)
        If strDefIPPCompID <> "" And strDefIPPCompID <> Session("CompanyId") Then
            cmdAdd.Visible = False
            cmdModify.Visible = False
            cmdDelete.Visible = False
        Else
            cmdAdd.Visible = True
            cmdModify.Visible = True
            cmdDelete.Visible = True
        End If

    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgGLCode.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub dtgGLCode_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgGLCode.ItemCreated
        Grid_ItemCreated(dtgGLCode, e)
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Private Sub dtgGLCode_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgGLCode.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            Dim lnkCode As HyperLink
            lnkCode = e.Item.FindControl("lnkCode")
            lnkCode.Text = dv("CBG_B_GL_CODE")

            Dim lblIndex As Label
            lblIndex = e.Item.FindControl("lblIndex")
            lblIndex.Text = dv("CBG_B_GL_CODE")

            If dv("CBG_CC_REQ") = "Y" Then
                e.Item.Cells(3).Text = "Yes"
            Else
                e.Item.Cells(3).Text = "No"
            End If

            If dv("CBG_AG_REQ") = "Y" Then
                e.Item.Cells(4).Text = "Yes"
            Else
                e.Item.Cells(4).Text = "No"
            End If

            If dv("CBG_STATUS") = "A" Then
                e.Item.Cells(5).Text = "Active"
            Else
                e.Item.Cells(5).Text = "Inactive"
            End If

            'e.Item.Cells(EnumCat.icStartDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, e.Item.Cells(EnumCat.icStartDate).Text)
            'e.Item.Cells(EnumCat.icEndDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, e.Item.Cells(EnumCat.icEndDate).Text)

            'If ViewState("cattype") = "D" Then
            '    e.Item.Cells(EnumCat.icCoyName).Visible = False
            'Else
            '    'lnkCode.NavigateUrl = "HubCatalogueDetail.aspx?mode=mod&index=" & lblIndex.Text & "&cattype=" & viewstate("cattype") & "&pageid=" & strPageId
            'End If
            'ElseIf e.Item.ItemType = ListItemType.Header Then
            '    If ViewState("cattype") = "D" Then
            '        e.Item.Cells(EnumCat.icCoyName).Visible = False
            '    End If
        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        ViewState("SortExpression") = "CBG_B_GL_CODE"
        ViewState("SortAscending") = "yes"
        dtgGLCode.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub dtgGLCode_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgGLCode.PageIndexChanged
        dtgGLCode.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Private Sub cmdModify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdModify.Click
        Dim dgItem As DataGridItem
        Dim chkItem As CheckBox

        For Each dgItem In dtgGLCode.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                Dim strFileName As String
                Dim strscript As New System.Text.StringBuilder
                strscript.Append("<script language=""javascript"">")
                strFileName = dDispatcher.direct("IPPAdmin", "AddGLCode.aspx", "&mode=mod&index=" & CType(dgItem.FindControl("lblIndex"), Label).Text.Replace("'", "\'") & "&pageid=" & strPageId)
                'strFileName = Replace(Server.UrlEncode(strFileName), "'", "%27")
                strscript.Append("PopWindow('" & strFileName & "');")
                strscript.Append("</script>")
                RegisterStartupScript("script13", strscript.ToString())
            End If
        Next
    End Sub

  

    Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim dgItem As DataGridItem
        Dim objGL As New IPP
        Dim chkItem As CheckBox
        Dim dsGL As New DataSet
        Dim dtGL As New DataTable
        dtGL.Columns.Add("index", Type.GetType("System.String"))
        dtGL.Columns.Add("CoyId", Type.GetType("System.String"))

        For Each dgItem In dtgGLCode.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                Dim dtr As DataRow
                dtr = dtGL.NewRow()
                dtr("index") = CType(dgItem.FindControl("lblIndex"), Label).Text
                dtr("CoyId") = Session("CompanyId") 'dgItem.Cells(6).Text 
                dtGL.Rows.Add(dtr)

            End If
        Next
        ' dsGL.Tables.Add(dtGL)

        If objGL.DelGLCode(dtGL) Then
            objGL.Message(Me, "00004", MsgBoxStyle.Information)
        Else
            'strMsg = "Deletion is not allowed.""&vbCrLf&""It has outstanding PR(s)."
            'Common.NetMsgbox(Me, MsgRecordNotDelete, MsgBoxStyle.Information, "Wheel")
            objGL.Message(Me, "00009", MsgBoxStyle.Information)
        End If
        ViewState("action") = "del"
        Bindgrid()
    End Sub

    'Private Sub GenerateTab()
    '    If strPageId = Nothing Then
    '        strPageId = Session("strPageId")
    '    Else
    '        Session("strPageId") = strPageId
    '    End If
    '    Session("w_ConCat_tabs") = "<div class=""t_entity""><ul>" & _
    '                                "<li><div class=""space""></div></li>" & _
    '                                "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Catalogue", "ContractCatalogue.aspx", "pageid=" & strPageId) & """><span>Contract Catalogue</span></a></li>" & _
    '                                "<li><div class=""space""></div></li>" & _
    '                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Catalogue", "ContractItem.aspx", "pageid=" & strPageId) & """><span>Items Assignment</span></a></li>" & _
    '                                "<li><div class=""space""></div></li>" & _
    '                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Catalogue", "ConCatBatchUploadDownload.aspx", "pageid=" & strPageId) & """><span>Batch Upload/Download</span></a></li>" & _
    '                                "<li><div class=""space""></div></li>" & _
    '                                "</ul><div></div></div>"

    'End Sub

End Class