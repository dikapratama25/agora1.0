Imports AgoraLegacy
Imports eProcure.Component
Imports System.Drawing
Imports System.IO
Imports MySql.Data.MySqlClient
Imports Microsoft.Reporting.WebForms

Public Class VendorRFQListExp
    Inherits AgoraLegacy.AppBaseClass
    Dim objrfq As New RFQ
    Dim dDispatcher As New AgoraLegacy.dispatcher
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    ''Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    'Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    ''Protected WithEvents Table4 As System.Web.UI.HtmlControls.HtmlTable
    'Dim objinv As New Invoice
    'Dim paid As Double
    'Dim strMode As String
    'Dim ordered_amount As Double
    'Protected WithEvents dtg_InvList As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents dtg_inv2 As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents cmd_createInv As System.Web.UI.WebControls.Button
    'Protected WithEvents cmd_submit As System.Web.UI.WebControls.Button
    'Protected WithEvents tblSearchResult As System.Web.UI.HtmlControls.HtmlTable
    ''Protected WithEvents txt_DocNo As System.Web.UI.WebControls.TextBox
    'Protected WithEvents back_view As System.Web.UI.HtmlControls.HtmlAnchor
    'Protected WithEvents back As System.Web.UI.HtmlControls.HtmlAnchor

    ''Protected WithEvents txt_bcom As System.Web.UI.WebControls.TextBox
    'Protected WithEvents lblStep1 As System.Web.UI.WebControls.Label
    'Protected WithEvents lblStep2 As System.Web.UI.WebControls.Label
    'Protected WithEvents cmdClear As System.Web.UI.HtmlControls.HtmlInputButton
    'Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    'Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    'Protected WithEvents hidSummary As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents hidControl As System.Web.UI.HtmlControls.HtmlInputHidden
    'Dim strDONo, strVMode, strLocID, strGRNNo, strBCoyID, strPONo, strFrm, strtemp As String
    'Dim intPOIdx As Integer\
    Protected WithEvents dtgOutstandingRFQ As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Search As System.Web.UI.WebControls.Button
    Protected WithEvents txt_Num As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_com_name As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmd_clear As System.Web.UI.WebControls.Button
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
        MyBase.Page_Load(sender, e)

        If Not Page.IsPostBack Then
            GenerateTab()
            Me.cmdDelete.Attributes.Add("onclick", "return cmdAddClick();")
            SetGridProperty(dtgOutstandingRFQ)
            dtgOutstandingRFQ.CurrentPageIndex = 0
            ViewState("SortAscendingOutstandingRFQ") = "no"
            ViewState("SortExpressionOutstandingRFQ") = "Creation Date"
            ViewState("SortAscending") = "no"
            ViewState("SortExpression") = "Creation Date"
            BindgridOutstandingRFQ()

        End If
    End Sub

    Private Function BindgridOutstandingRFQ(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        'Dim objDO As New Dashboard
        Dim com_name = txt_com_name.Text
        Dim docnum As String = txt_Num.Text
        Dim v_display As Integer = 0
        Dim V_RFQ_Status As Integer = 0

        cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")

        '//Retrieve Data from Database
        Dim dsOutStandingRFQ As DataSet = New DataSet
        dsOutStandingRFQ = objrfq.get_RFQExp(docnum, com_name, v_display)

        Dim dvViewOutStandingRFQ As DataView
        dvViewOutStandingRFQ = dsOutStandingRFQ.Tables(0).DefaultView
        dvViewOutStandingRFQ.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewOutStandingRFQ.Sort += " DESC"
        If ViewState("actionOutstandingRFQ") = "del" Then
            If dtgOutstandingRFQ.CurrentPageIndex > 0 And dsOutStandingRFQ.Tables(0).Rows.Count Mod dtgOutstandingRFQ.PageSize = 0 Then
                dtgOutstandingRFQ.CurrentPageIndex = dtgOutstandingRFQ.CurrentPageIndex - 1
                ViewState("actionOutstandingRFQ") = ""
            End If
        End If
        intTotRecord = dsOutStandingRFQ.Tables(0).Rows.Count
        Session("PageRecordOutstandingRFQ") = intTotRecord
        If Session("PageRecordOutstandingRFQ") > 0 Then
            resetDatagridPageIndex(dtgOutstandingRFQ, dvViewOutStandingRFQ)
            dtgOutstandingRFQ.DataSource = dvViewOutStandingRFQ
            dtgOutstandingRFQ.DataBind()
        Else
            dtgOutstandingRFQ.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
            cmdDelete.Enabled = False
        End If

        ViewState("PageCountOutstandingRFQ") = dtgOutstandingRFQ.PageCount
    End Function

    Sub dtgOutstandingRFQ_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgOutstandingRFQ.PageIndexChanged
        dtgOutstandingRFQ.CurrentPageIndex = e.NewPageIndex
        BindgridOutstandingRFQ()
    End Sub

    Private Sub dtgOutStandingRFQ_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstandingRFQ.ItemCreated
        intPageRecordCnt = Session("PageRecordOutstandingRFQ")
        Grid_ItemCreated(sender, e)
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Sub SortCommandOutStandingRFQ_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgOutstandingRFQ.CurrentPageIndex = 0
        BindgridOutstandingRFQ(True)
    End Sub

    Protected Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        goto_trash()
    End Sub

    Function goto_trash()
        Dim dgItem As DataGridItem
        Dim dg_tem As New DataGrid
        Dim objrfq As New RFQ
        Dim objval As New RFQ_User
        Dim objDB As New EAD.DBCom
        Dim strAryQuery(0) As String
        Dim strSQL As String
        Dim txtPrice As TextBox
        Dim lblPrice As Label
        Dim chkItem As CheckBox
        Dim ckhtemp As CheckBox
        Dim DOCTYPE As String
        Dim j As Integer
        Dim i As Integer = 0

        dg_tem = Me.dtgOutstandingRFQ
        j = 2

        For Each dgItem In dg_tem.Items
            Dim dv As DataRowView = CType(dgItem.DataItem, DataRowView)
            objval.RFQ_ID = dgItem.Cells(j).Text
            chkItem = dgItem.FindControl("chkSelection")

            If chkItem.Checked Then
                strSQL = objrfq.Vendor_add2trash(objval, "0", DOCTYPE)
                Common.Insert2Ary(strAryQuery, strSQL)
            End If

            i = i + 1
        Next

        objDB.BatchExecute(strAryQuery)
        Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
        If i = dg_tem.Items.Count Then
            If dg_tem.CurrentPageIndex <> 0 Then
                dg_tem.CurrentPageIndex = dg_tem.CurrentPageIndex - 1
            End If
        End If
        BindgridOutstandingRFQ()
    End Function


    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_RFQ_tabs") = "<div class=""t_entity""><ul>" &
        "<li><div class=""space""></div></li>" &
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("RFQ", "VendorRFQList.aspx", "pageid=" & strPageId) & """><span>Outstanding RFQ</span></a></li>" &
                            "<li><div class=""space""></div></li>" &
                            "<li><a class=""t_entity_btn_selected"" href=""#""><span>Expired / Rejected RFQ</span></a></li>" &
                            "<li><div class=""space""></div></li>" &
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("RFQ", "RFQSearch.aspx", "pageid=" & strPageId) & """><span>Quotation Listing</span></a></li>" &
                            "<li><div class=""space""></div></li>" &
                           "</ul><div></div></div>"
    End Sub

    Private Sub dtgOutstandingRFQ_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstandingRFQ.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim chk As CheckBox

            Dim lnkRFQNo
            lnkRFQNo = e.Item.FindControl("lnkRFQNum")
            lnkRFQNo.Text = dv("RFQ Number")

            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
            strPageId = 98

            If e.Item.Cells(5).Text >= Date.Today And dv("RM_B_DISPLAY_STATUS") = "0" Then
                lnkRFQNo.NavigateUrl = dDispatcher.direct("RFQ", "CreateQuotationNew.aspx", "Frm=VendorRFQListExp&pageid=" & strPageId & "&RFQ_No=" & Server.UrlEncode(dv("RFQ Number")) & "&RFQ_ID=" & dv("RM_RFQ_ID") & "&bcomid=" & dv("RM_Coy_ID"))
                If dv("RVM_V_RFQ_STATUS") = "1" Then
                    lnkRFQNo.NavigateUrl = dDispatcher.direct("RFQ", "RFQDetail.aspx", "Frm=VendorRFQListExp&pageid=" & strPageId & "&page=1&goto=2&RFQ_Num=" & Server.UrlEncode(dv("RFQ Number")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&vcom_id=" & Session("CompanyId"))

                    Dim objDB As New EAD.DBCom
                    Dim strRFQ As String = objDB.GetVal("SELECT IFNULL(RRM_RFQ_ID,'') AS RRM_RFQ_ID FROM RFQ_REPLIES_MSTR WHERE RRM_RFQ_ID = '" & Common.parseNull(dv("RM_RFQ_ID")) & "' AND RRM_V_COMPANY_ID = '" & Session("CompanyId") & "'")

                    If strRFQ = "" Then
                        Dim imgAttach As New System.Web.UI.WebControls.Image
                        imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                        imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "Unable To Supply Icon 2.gif")
                        e.Item.Cells(1).Controls.Add(imgAttach)
                    End If
                End If
            Else
                If dv("RM_B_DISPLAY_STATUS") = "0" Then
                    lnkRFQNo.NavigateUrl = dDispatcher.direct("RFQ", "RFQDetail.aspx", "Frm=VendorRFQListExp&pageid=" & strPageId & "&page=1&goto=2&RFQ_Num=" & Server.UrlEncode(dv("RFQ Number")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&vcom_id=" & Session("CompanyId"))

                    If dv("RVM_V_RFQ_STATUS") = "1" Then
                        Dim objDB As New EAD.DBCom
                        Dim strRFQ As String = objDB.GetVal("SELECT IFNULL(RRM_RFQ_ID,'') AS RRM_RFQ_ID FROM RFQ_REPLIES_MSTR WHERE RRM_RFQ_ID = '" & Common.parseNull(dv("RM_RFQ_ID")) & "' AND RRM_V_COMPANY_ID = '" & Session("CompanyId") & "'")

                        If strRFQ = "" Then
                            Dim imgAttach As New System.Web.UI.WebControls.Image
                            imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                            imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "Unable To Supply Icon 2.gif")
                            e.Item.Cells(1).Controls.Add(imgAttach)
                        End If
                    End If
                Else
                    Dim imgAttach As New System.Web.UI.WebControls.Image
                    imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                    imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "Deleted RFQ Icon.gif")
                    e.Item.Cells(1).Controls.Add(imgAttach)
                End If

            End If

            e.Item.Cells(4).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Creation Date"))
            e.Item.Cells(5).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Expiry Date"))
        End If
    End Sub

    Private Sub cmd_Search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_Search.Click
        dtgOutstandingRFQ.CurrentPageIndex = 0
        ViewState("SortAscending") = "no"
        ViewState("SortExpression") = "Creation Date"
        SetGridProperty(Me.dtgOutstandingRFQ)
        BindgridOutstandingRFQ()
    End Sub

    Private Sub cmd_clear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_clear.Click
        Me.txt_Num.Text = ""
        Me.txt_com_name.Text = ""
    End Sub

End Class
