'//Attachment Ind
Imports AgoraLegacy
Imports eProcure.Component

Public Class ConvertPRListingFTN
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents txtBuyer As System.Web.UI.WebControls.TextBox
    Protected WithEvents Table2 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblStartDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblEndDate As System.Web.UI.WebControls.Label

    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objGlobal As New AppGlobals
    Dim dtBCM As DataTable

    Public Enum EnumPR
        'icCheckBox
        icPRNo
        icItemCode
        icItemName
        'icApprovalDate
        icConvDate
        icConvDoc
        icVendor
        icQuantity
        icCurrency
        icCost
        icAmount
        icTax
        icBudget
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
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CheckButtonAccess(True)
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objPR As New PR
        'Put user code to initialize the page here
        blnCheckBox = False
        SetGridProperty(dtgPRList)

        MyBase.Page_Load(sender, e)

        Dim objBCM As New BudgetControl
        ViewState("BCM") = CInt(objPR.checkBCM)
        'ViewState("BCM") = 1
        'Michelle (23/12/2011) - Issue 1405
        ' dtBCM = objBCM.getBCMListByCompany()
        dtBCM = objBCM.getBCMListByCompanyNew()
        objBCM = Nothing

        Dim objGST As New GST
        ViewState("isGST") = objGST.chkGSTCOD

        If Not Page.IsPostBack Then
            'objGlobal.FillCommodityType(Me.cboCommodityType)

            'Dim objBCM As New BudgetControl
            'ViewState("BCM") = CInt(objPR.checkBCM)
            ''ViewState("BCM") = 1
            'dtBCM = objBCM.getBCMListByCompany()
            'objBCM = Nothing

            GenerateTab()
        End If

        'If Session("Env") = "FTN" Then
        '    Me.dtgPRList.Columns(EnumPR.icBudget).Visible = False
        'Else
        '    Me.dtgPRList.Columns(EnumPR.icBudget).Visible = True
        'End If
        Me.dtgPRList.Columns(EnumPR.icBudget).Visible = False

        intPageRecordCnt = ViewState("intPageRecordCnt")

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
        Dim objPR As New PurchaseReq2

        ' Retrieve Data from Database
        Dim ds As DataSet
        Dim aryItemType As New ArrayList
        Dim strDoc As String

        'If Me.cboCommodityType.SelectedItem.Text <> "---Select---" Then
        '    strCommodity = Me.cboCommodityType.SelectedValue
        'End If

        If Me.chkSpot.Checked = True Then
            aryItemType.Add("PO")
        End If

        If Me.chkStock.Checked = True Then
            aryItemType.Add("RFQ")
        End If

        ' ds = objPR.getPRListForApproval(txtPRNo.Text, "", txtDateFr.Text, txtDateTo.Text, "", "app", "", strAOAction)
        ds = objPR.PRListForPORFQ(txtPRNo.Text, txtDateFr.Text, txtDateTo.Text, "BUYER", "", aryItemType, txtConvertDoc.Text)

        ' For Sorting Asc or Desc
        Dim dvViewPR As DataView
        dvViewPR = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewPR.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewPR.Sort += " DESC"
        End If

        ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count

        ' Bind Datagrid
        If ViewState("intPageRecordCnt") > 0 Then
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
        'If e.Item.ItemType = ListItemType.Header Then
        '    Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
        '    chkAll.Attributes.Add("onclick", "selectAll();")
        'End If
        If e.Item.ItemType = ListItemType.Header Then
            If ViewState("isGST") Then
                e.Item.Cells(EnumPR.icTax).Text = "GST Amount"
            End If
        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgPRList.CurrentPageIndex = 0
        ViewState("SortAscending") = "no"
        ViewState("SortExpression") = "PRM_PR_NO"
        Bindgrid(True)
    End Sub

    Private Sub dtgPRList_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPRList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim dblAmt, dblTaxAmt As Double
            Dim objDB As New EAD.DBCom

            e.Item.Cells(EnumPR.icConvDate).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("PRD_CONVERT_TO_DATE"))

            'If dv("PRD_CONVERT_TO_IND") = "PO" Then
            '    e.Item.Cells(EnumPR.icCost).Text = Format(Common.parseNull(dv("POD_UNIT_COST"), 0), "#,##0.0000")

            '    dblAmt = Common.parseNull(dv("POD_UNIT_COST"), 0) * Common.parseNull(dv("PRD_ORDERED_QTY"), 0)
            '    e.Item.Cells(EnumPR.icAmount).Text = Format(dblAmt, "#,##0.00")

            '    If Common.parseNull(dv("POD_GST"), 0) > 0 Then
            '        dblTaxAmt = (Common.parseNull(dv("POD_UNIT_COST"), 0) * Common.parseNull(dv("PRD_ORDERED_QTY"), 0)) * (Common.parseNull(dv("POD_GST"), 0) / 100)
            '        e.Item.Cells(EnumPR.icTax).Text = Format(dblTaxAmt, "#,##0.00")
            '    Else
            '        dblTaxAmt = 0
            '        e.Item.Cells(EnumPR.icTax).Text = Format(dblTaxAmt, "#,##0.00")
            '    End If
            'Else
            '    e.Item.Cells(EnumPR.icCost).Text = Format(Common.parseNull(dv("PRD_UNIT_COST"), 0), "#,##0.0000")

            '    dblAmt = Common.parseNull(dv("PRD_UNIT_COST"), 0) * Common.parseNull(dv("PRD_ORDERED_QTY"), 0)
            '    e.Item.Cells(EnumPR.icAmount).Text = Format(dblAmt, "#,##0.00")

            '    If Common.parseNull(dv("PRD_GST"), 0) > 0 Then
            '        dblTaxAmt = (Common.parseNull(dv("PRD_UNIT_COST"), 0) * Common.parseNull(dv("PRD_ORDERED_QTY"), 0)) * (Common.parseNull(dv("PRD_GST"), 0) / 100)
            '        e.Item.Cells(EnumPR.icTax).Text = Format(dblTaxAmt, "#,##0.00")
            '    Else
            '        dblTaxAmt = 0
            '        e.Item.Cells(EnumPR.icTax).Text = Format(dblTaxAmt, "#,##0.00")
            '    End If
            'End If

            e.Item.Cells(EnumPR.icCost).Text = Format(Common.parseNull(dv("PRD_UNIT_COST"), 0), "#,##0.0000")

            dblAmt = Common.parseNull(dv("PRD_UNIT_COST"), 0) * Common.parseNull(dv("PRD_ORDERED_QTY"), 0)
            e.Item.Cells(EnumPR.icAmount).Text = Format(dblAmt, "#,##0.00")

            If Common.parseNull(dv("PRD_GST"), 0) > 0 Then
                dblTaxAmt = (Common.parseNull(dv("PRD_UNIT_COST"), 0) * Common.parseNull(dv("PRD_ORDERED_QTY"), 0)) * (Common.parseNull(dv("PRD_GST"), 0) / 100)
                e.Item.Cells(EnumPR.icTax).Text = Format(dblTaxAmt, "#,##0.00")
            Else
                dblTaxAmt = 0
                e.Item.Cells(EnumPR.icTax).Text = Format(dblTaxAmt, "#,##0.00")
            End If

            ' ''    Dim lnkPRNo As HyperLink
            ' ''    Dim intStatus As String
            ' ''    Dim COUNT As Integer
            ' ''    Dim ARRAY(100) As String

            ' ''    intStatus = Common.parseNull(dv("PRM_PR_STATUS"))
            ' ''    If intStatus = PRStatus.RejectedBy Then
            ' ''        e.Item.Cells(EnumPR.icStatus).Text = "Rejected"
            ' ''    Else
            ' ''        e.Item.Cells(EnumPR.icStatus).Text = "Approved"
            ' ''    End If
            ' ''    lnkPRNo = e.Item.Cells(EnumPR.icPRNo).FindControl("lnkPRNo")
            ' ''    'lnkPRNo.NavigateUrl = dDispatcher.direct("PR", "PRDetail.aspx", "side=b&caller=SearchPO_ALL&status=" & intStatus & "&BCoyID=" & Session("CompanyID") & "&pageid=" & strPageId & "&index=" & dv("PRM_PR_Index") & "&PR_No=" & dv("PRM_PR_No"))
            ' ''    lnkPRNo.NavigateUrl = dDispatcher.direct("PR", "PRDetail.aspx", "pageid=" & strPageId & "&index=" & dv("PRM_PR_Index") & "&PRNO=" & dv("PRM_PR_No") & "&type=mod&mode=bc")
            ' ''    lnkPRNo.Text = dv("PRM_PR_No")

            ' ''    If Common.parseNull(dv("PRM_PR_STATUS")) = PRStatus.CancelledBy Or Common.parseNull(dv("PRM_PR_STATUS")) = PRStatus.HeldBy Then
            ' ''        If IsDBNull(dv("CHANGED_BY_NAME")) Then
            ' ''            e.Item.Cells(EnumPR.icStatus).Text = e.Item.Cells(EnumPR.icStatus).Text & " (" & Common.parseNull(dv("PRM_STATUS_CHANGED_BY")) & ")"
            ' ''        Else
            ' ''            e.Item.Cells(EnumPR.icStatus).Text = e.Item.Cells(EnumPR.icStatus).Text & " (" & dv("CHANGED_BY_NAME") & ")"
            ' ''        End If
            ' ''    End If

            ' ''    Dim objPR As New PurchaseReq2
            ' ''    If objPR.HasAttachment(dv("PRM_PR_No"), "PR") Then
            ' ''        Dim imgAttach As New System.Web.UI.WebControls.Image
            ' ''        imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
            ' ''        imgAttach.ImageUrl = dDispatcher.direct("Plugins/images", "clip_icon.gif")
            ' ''        e.Item.Cells(EnumPR.icPRNo).Controls.Add(imgAttach)
            ' ''    End If

            ' ''    Dim STR As String
            ' ''    Dim i As Integer

            ' ''    objPR = Nothing
            ' ''    e.Item.Cells(EnumPR.icCreationDate).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("PRM_SUBMIT_DATE"))

            '//For BCM
            'If Not IsDBNull(dv("PRD_ACCT_INDEX")) Then
            '    If Not dtBCM Is Nothing Then
            '        Dim drTemp As DataRow()
            '        drTemp = dtBCM.Select("Acct_Index=" & dv("PRD_ACCT_INDEX"))
            '        If drTemp.Length > 0 Then
            '            e.Item.Cells(EnumPR.icBudget).Text = Mid(drTemp(0)("Acct_List"), 1, 10)
            '            e.Item.Cells(EnumPR.icBudget).ToolTip = drTemp(0)("Acct_List")
            '        End If
            '    End If
            'End If
            'If Session("Env") <> "FTN" Then
            '    If ViewState("BCM") > 0 Then
            '        If Not IsDBNull(dv("PRD_ACCT_INDEX")) Then
            '            If Not dtBCM Is Nothing Then
            '                Dim drTemp As DataRow()
            '                drTemp = dtBCM.Select("Acct_Index=" & dv("PRD_ACCT_INDEX"))
            '                If drTemp.Length > 0 Then
            '                    e.Item.Cells(EnumPR.icBudget).Text = Mid(drTemp(0)("Acct_List"), 1, 10)
            '                    e.Item.Cells(EnumPR.icBudget).ToolTip = drTemp(0)("Acct_List")
            '                End If
            '            End If
            '        End If
            '    Else
            '        e.Item.Cells(EnumPR.icBudget).Visible = False
            '    End If
            'Else
            '    e.Item.Cells(EnumPR.icBudget).Visible = False
            'End If

            e.Item.Cells(EnumPR.icBudget).Visible = False

            'Dim lblPRNo As Label
            'lblPRNo = e.Item.FindControl("lblPRNo")
            'lblPRNo.Text = Common.parseNull(dv("PRM_PR_NO"))

            Dim lnkPRNo As HyperLink
            lnkPRNo = e.Item.Cells(EnumPR.icPRNo).FindControl("lnkPRNo")
            lnkPRNo.NavigateUrl = dDispatcher.direct("PR", "PRDetail.aspx", "caller=ConvertPRList&pageid=" & strPageId & "&index=" & dv("PRM_PR_Index") & "&PRNO=" & Common.parseNull(dv("PRM_PR_NO")) & "&type=mod&mode=bc")
            lnkPRNo.Text = Common.parseNull(dv("PRM_PR_NO"))


            Dim lnkConvert As HyperLink
            Dim strPOM_PO_Index, strRFQ_Index As String
            lnkConvert = e.Item.Cells(EnumPR.icPRNo).FindControl("lnkConvert")

            If Common.parseNull(dv("PRD_CONVERT_TO_IND")) = "PO" Then
                strPOM_PO_Index = objDB.GetVal("SELECT IFNULL(POM_PO_INDEX,'') AS POM_PO_NO FROM PO_MSTR WHERE POM_PO_NO = '" & Common.parseNull(dv("PRD_CONVERT_TO_DOC")) & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                lnkConvert.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx", "Frm=ConvertPRList&pageid=" & strPageId & "&PO_INDEX=" & strPOM_PO_Index & "&PO_NO=" & Common.parseNull(dv("PRD_CONVERT_TO_DOC")) & "&BCoyID=" & HttpContext.Current.Session("CompanyId") & "&status=&Caller=ConvertPRList&side=b&filetype=2&type=" & "" & "&poview=1&checkid=no")
            ElseIf Common.parseNull(dv("PRD_CONVERT_TO_IND")) = "RFQ" Then
                strRFQ_Index = objDB.GetVal("SELECT IFNULL(RM_RFQ_ID,'') AS RM_RFQ_ID FROM RFQ_MSTR WHERE RM_RFQ_NO = '" & Common.parseNull(dv("PRD_CONVERT_TO_DOC")) & "' AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                lnkConvert.NavigateUrl = dDispatcher.direct("RFQ", "RFQDetail.aspx", "Frm=ConvertPRList&page=1&RFQ_Num=" & Common.parseNull(dv("PRD_CONVERT_TO_DOC")) & "&RFQ_ID=" & strRFQ_Index)
            End If

            lnkConvert.Text = Common.parseNull(dv("PRD_CONVERT_TO_DOC"))


            If Common.parseNull(dv("PRM_URGENT")) = "1" Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(0).Controls.Add(imgAttach)
            End If
        ElseIf e.Item.ItemType = ListItemType.Header Then
            If ViewState("BCM") <= 0 Then
                e.Item.Cells(EnumPR.icBudget).Visible = False
            End If

        End If
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        Session("w_ConvertPR_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PR", "ConvertPR.aspx", "pageid=" & strPageId) & """><span>Convert PR</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PR", "ConvertPRListing.aspx", "pageid=" & strPageId) & """><span>Convert PR Listing</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "</ul><div></div></div>"
    End Sub
End Class


