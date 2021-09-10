Imports AgoraLegacy
Imports eProcure.Component


Public Class SalesInfo
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Protected WithEvents txtLocalSales As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtExportSales As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblAction As System.Web.UI.WebControls.Label
    Protected WithEvents cmd_save As System.Web.UI.WebControls.Button
    Protected WithEvents dtgSalesTurnover As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Protected WithEvents cmdModify As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_delete As System.Web.UI.WebControls.Button
    Protected WithEvents btnHidden As System.Web.UI.WebControls.Button
    Protected WithEvents vldSalesArea As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents revLocalSales As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents revExportSales As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents Image1 As System.Web.UI.WebControls.Image

    Dim ds As DataSet




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


    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SetGridProperty(dtgSalesTurnover)
        If Not IsPostBack Then
            Populate()


        End If

        If Not Page.IsPostBack Then
            ViewState("Side") = Request.Params("side")
            GenerateTab()
            Bindgrid()
            vldSalesArea.ShowSummary = False
        End If


        MyBase.Page_Load(sender, e)
        cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
        cmd_delete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection');")
        Image1.ImageUrl = dDispatcher.direct("Plugins/images", "collapse_up.gif")
    End Sub

    Public Sub dtgSalesTurnover_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgSalesTurnover.PageIndexChanged
        dtgSalesTurnover.CurrentPageIndex = e.NewPageIndex
        Bindgrid(True)
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String

        Dim objSI As New Admin
        Dim ds As DataSet



        ds = objSI.getSalesInfoList(Session("CompanyId"))


        '//for sorting asc or desc
        Dim dvViewSI As DataView
        dvViewSI = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewSI.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewSI.Sort += " DESC"
        End If


        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            cmd_delete.Visible = True
            cmdModify.Visible = True
            resetDatagridPageIndex(dtgSalesTurnover, dvViewSI)
            dtgSalesTurnover.DataSource = dvViewSI
            dtgSalesTurnover.DataBind()
        Else
            cmd_delete.Visible = False
            cmdModify.Visible = False
            resetDatagridPageIndex(dtgSalesTurnover, dvViewSI)
            dtgSalesTurnover.DataSource = dvViewSI
            dtgSalesTurnover.DataBind()

        End If

        ViewState("PageCount") = dtgSalesTurnover.PageCount
    End Function



    Private Sub dtgSalesTurnover_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgSalesTurnover.ItemCreated
        '//this line must be included
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgSalesTurnover, e)
        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub


    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Dim strFileName As String
        Dim strscript As New System.Text.StringBuilder
        Session("userAction") = "Add"

        strscript.Append("<script language=""javascript"">")
        strFileName = dDispatcher.direct("Companies", "Add_SalesTurnOver.aspx", "pageid=" & strPageId)
        strFileName = Server.UrlEncode(strFileName)
        strscript.Append("ShowDialog('" & dDispatcher.direct("Companies", "Dialog.aspx", "page=" & strFileName) & "','250px');")
        strscript.Append("document.getElementById('btnHidden').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script3", strscript.ToString())
    End Sub

    Private Sub Populate()
        Dim objSales As New Admin
        Dim local As String
        Dim export As String
        local = objSales.GetLocalSalesArea(Session("CompanyId"))
        If local IsNot Nothing Then
            local = Fix(local)
        Else
            local = ""
        End If
        export = objSales.GetExportSalesArea(Session("CompanyId"))
        If export IsNot Nothing Then
            export = Fix(export)
        Else
            export = ""
        End If
        txtLocalSales.Text = local.ToString
        If txtLocalSales.Text = "0.00" Then txtLocalSales.Text = ""
        txtExportSales.Text = export.ToString
        If txtExportSales.Text = "0.00" Then txtExportSales.Text = ""

    End Sub
    Public Sub btnHidden_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHidden.Click

        Bindgrid(True)
    End Sub

    Protected Sub cmd_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_save.Click
        vldSalesArea.ShowSummary = True
        Dim objsave As New Admin
        Dim LocalSales, ExportSales As String
        If Page.IsValid Then

            LocalSales = txtLocalSales.Text
            ExportSales = txtExportSales.Text
            If LocalSales = "" Then
                LocalSales = 0
            End If
            If ExportSales = "" Then
                ExportSales = 0
            End If
            objsave.SalesArea(LocalSales, ExportSales)
            Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)





        End If
        Populate()

    End Sub
    Protected Sub cmdModify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdModify.Click
        Dim dgItem As DataGridItem
        Dim chkItem As CheckBox
        Dim strFileName As String
        Dim strscript As New System.Text.StringBuilder
        Dim strItemID, strItemCurrency, strItemAmount As String


        Session("userAction") = "Modify"
        For Each dgItem In dtgSalesTurnover.Items
            chkItem = dgItem.FindControl("chkSelection")
            strItemID = dgItem.Cells(1).Text
            strItemCurrency = dgItem.Cells(2).Text
            strItemAmount = dgItem.Cells(3).Text

            If chkItem.Checked Then
                strscript.Append("<script language=""javascript"">")
                strFileName = dDispatcher.direct("Companies", "Add_SalesTurnOver.aspx", "pageid=" & strPageId & "&itemid=" & strItemID & "&itemcurrency=" & strItemCurrency & "&itemamount=" & strItemAmount)
                strFileName = Server.UrlEncode(strFileName)
                strscript.Append("ShowDialog('" & dDispatcher.direct("Companies", "Dialog.aspx", "page=" & strFileName) & "','250px');")
                strscript.Append("document.getElementById('btnHidden').click();")
                strscript.Append("</script>")
                RegisterStartupScript("script3", strscript.ToString())

            End If
        Next



    End Sub

    Protected Sub cmd_delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_delete.Click
        Dim dgItem As DataGridItem
        Dim checkitem As CheckBox
        Dim strItemID As String
        Dim objdel As New Admin



        For Each dgItem In dtgSalesTurnover.Items
            checkitem = dgItem.FindControl("chkSelection")
            strItemID = dgItem.Cells(1).Text

            If checkitem.Checked Then
                objdel.delSalesTurnOver(Session("CompanyId"), strItemID)



            End If
        Next
        Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
        Bindgrid()

    End Sub
    Private Sub dtgSalesTurnover_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgSalesTurnover.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
            If IsDBNull(dv("CS_Amount")) Then
                e.Item.Cells(3).Text = "N/A"
            Else
                e.Item.Cells(3).Text = Format(dv("CS_Amount"), "#,##0.00")

            End If
        End If
    End Sub
    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgSalesTurnover.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        If ViewState("Side") = "BUYER" Then
            '    Session("w_SalesInfo_tabs") = "<div class=""t_entity""><ul>" & _
            '    "<li><div class=""space""></div></li>" & _
            '                           "<li><a class=""t_entity_btn"" href=""../Companies/coCompanyDetail.aspx?side=BUYER&mode=modify&pageid=" & strPageId & """><span>Company Details</span></a></li>" & _
            '                           "<li><div class=""space""></div></li>" & _
            '                           "<li><a class=""t_entity_btn"" href=""../Admin/BComParam.aspx?side=BUYER&pageid=" & strPageId & """><span>Parameters</span></a></li>" & _
            '                           "<li><div class=""space""></div></li>" & _
            '                           "<li><a class=""t_entity_btn"" href=""../Admin/AddressMaster.aspx?side=BUYER&type=D&mod=T&pageid=" & strPageId & """><span>Delivery Address</span></a></li>" & _
            '                           "<li><div class=""space""></div></li>" & _
            '                           "<li><a class=""t_entity_btn"" href=""../Admin/AddressMaster.aspx?side=BUYER&type=B&mod=T&pageid=" & strPageId & """><span>Billing Address</span></a></li>" & _
            '                           "<li><div class=""space""></div></li>" & _
            '                           "<li><a class=""t_entity_btn"" href=""../Admin/DeptSetup.aspx?side=BUYER&pageid=" & strPageId & """><span>Department</span></a></li>" & _
            '                           "<li><div class=""space""></div></li>" & _
            '                           "<li><a class=""t_entity_btn"" href=""../Companies/SoftwareApp.aspx?side=BUYER&pageid=" & strPageId & """><span>Software</span></a></li>" & _
            '                           "<li><div class=""space""></div></li>" & _
            '                           "<li><a class=""t_entity_btn_selected"" href=""../Companies/SalesInfo.aspx?side=BUYER&pageid=" & strPageId & """><span>Sales Info</span></a></li>" & _
            '                           "<li><div class=""space""></div></li>" & _
            '                            "<li><a class=""t_entity_btn"" href=""../Companies/QualityStd.aspx?side=BUYER&pageid=" & strPageId & """><span>Quality Standards</span></a></li>" & _
            '                            "<li><div class=""space""></div></li>" & _
            '                 "</ul><div></div></div>"
            'Else
            '    Session("w_SalesInfo_tabs") = "<div class=""t_entity""><ul>" & _
            '    "<li><div class=""space""></div></li>" & _
            '                     "<li><a class=""t_entity_btn"" href=""../Companies/coCompanyDetail.aspx?side=VENDOR&mode=modify&pageid=" & strPageId & """><span>Company Details</span></a></li>" & _
            '                     "<li><div class=""space""></div></li>" & _
            '                     "<li><a class=""t_entity_btn"" href=""../Admin/BComVendor.aspx?side=VENDOR&pageid=" & strPageId & """><span>Parameters</span></a></li>" & _
            '                     "<li><div class=""space""></div></li>" & _
            '                        "<li><a class=""t_entity_btn"" href=""../Companies/SoftwareApp.aspx?side=VENDOR&pageid=" & strPageId & """><span>Software</span></a></li>" & _
            '                        "<li><div class=""space""></div></li>" & _
            '                        "<li><a class=""t_entity_btn_selected"" href=""../Companies/SalesInfo.aspx?side=VENDOR&pageid=" & strPageId & """><span>Sales Info</span></a></li>" & _
            '                        "<li><div class=""space""></div></li>" & _
            '                        "<li><a class=""t_entity_btn"" href=""../Companies/QualityStd.aspx?side=VENDOR&pageid=" & strPageId & """><span>Quality Standards</span></a></li>" & _
            '                        "<li><div class=""space""></div></li>" & _
            '"</ul><div></div></div>"
            Session("w_SalesInfo_tabs") = "<div class=""t_entity""><ul>" & _
            "<li><div class=""space""></div></li>" & _
                                   "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "coCompanyDetail.aspx", "side=BUYER&mode=modify" & "&pageid=" & strPageId) & """><span>Company Details</span></a></li>" & _
                                   "<li><div class=""space""></div></li>" & _
                                   "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "BComParam.aspx", "side=BUYER" & "&pageid=" & strPageId) & """><span>Parameters</span></a></li>" & _
                                   "<li><div class=""space""></div></li>" & _
                                   "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=D&mod=T" & "&pageid=" & strPageId) & """><span>Delivery Address</span></a></li>" & _
                                   "<li><div class=""space""></div></li>" & _
                                   "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=B&mod=T" & "&pageid=" & strPageId) & """><span>Billing Address</span></a></li>" & _
                                   "<li><div class=""space""></div></li>" & _
                                   "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "DeptSetup.aspx", "side=BUYER" & "&pageid=" & strPageId) & """><span>Department</span></a></li>" & _
                                   "<li><div class=""space""></div></li>" & _
                                   "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Companies", "SalesInfo.aspx", "side=BUYER" & "&pageid=" & strPageId) & """><span>Sales Info</span></a></li>" & _
                                   "<li><div class=""space""></div></li>" & _
                                   "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "SoftwareApp.aspx", "side=BUYER" & "&pageid=" & strPageId) & """><span>Software</span></a></li>" & _
                                   "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "QualityStd.aspx", "side=BUYER" & "&pageid=" & strPageId) & """><span>Quality Standards</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                         "</ul><div></div></div>"
        Else
            Session("w_SalesInfo_tabs") = "<div class=""t_entity""><ul>" & _
            "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "coCompanyDetail.aspx", "side=VENDOR&mode=modify" & "&pageid=" & strPageId) & """><span>Company Details</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "BComVendor.aspx", "side=VENDOR" & "&pageid=" & strPageId) & """><span>Parameters</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Companies", "SalesInfo.aspx", "side=VENDOR" & "&pageid=" & strPageId) & """><span>Sales Info</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "SoftwareApp.aspx", "side=VENDOR" & "&pageid=" & strPageId) & """><span>Software</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "QualityStd.aspx", "side=VENDOR" & "&pageid=" & strPageId) & """><span>Quality Standards</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                          "</ul><div></div></div>"

        End If
    End Sub

End Class
