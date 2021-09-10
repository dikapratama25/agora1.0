'//Attachment Ind
Imports AgoraLegacy
Imports eProcure.Component


Public Class ConvertPRFTN
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents txtBuyer As System.Web.UI.WebControls.TextBox
    Protected WithEvents Table2 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblStartDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblEndDate As System.Web.UI.WebControls.Label

    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objGlobal As New AppGlobals
    Dim dtBCM As DataTable
    Dim objDB As New EAD.DBCom

    Public Enum EnumPR
        icCheckBox
        icPRNo
        icItemCode
        icItemName
        icApprovalDate
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
        strNewCSS = "both"
        MyBase.Page_Load(sender, e)

        Dim objBCM As New BudgetControl
        ViewState("BCM") = CInt(objPR.checkBCM)
        'ViewState("BCM") = 1
        'Michelle (20/12/2011) - Issue 1405
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

            If Request.QueryString("Frm") = "Dashboard" Then
                cmdSearch_Click(sender, e)
            Else
                Session("urlreferer") = "ConvertPR"
            End If
        End If

        'cmdPO.Attributes.Add("onclick", "return raiserfqconfirm('PO');")
        'cmdRFQ.Attributes.Add("onclick", "return raiserfqconfirm('RFQ');")

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
        Dim strCommodity As String = ""

        'If Me.cboCommodityType.SelectedItem.Text <> "---Select---" Then
        '    strCommodity = Me.cboCommodityType.SelectedValue
        'End If

        If Me.txtCommodity.Text <> "" Then
            strCommodity = Me.hidCommodity.Value
        End If

        If Me.chkSpot.Checked = True Then
            aryItemType.Add("SP")
        End If

        If Me.chkStock.Checked = True Then
            aryItemType.Add("ST")
        End If

        If Me.chkMRO.Checked = True Then
            aryItemType.Add("MI")
        End If

        Dim PRNo As String
        If Request.QueryString("Frm") = "Dashboard" Then
            PRNo = Request.QueryString("PRNo")
        Else
            PRNo = txtPRNo.Text
        End If

        ' ds = objPR.getPRListForApproval(txtPRNo.Text, "", txtDateFr.Text, txtDateTo.Text, "", "app", "", strAOAction)
        ds = objPR.PRListForConvertPO(PRNo, txtDateFr.Text, txtDateTo.Text, "BUYER", "", aryItemType, strCommodity)

        ' For Sorting Asc or Desc
        Dim dvViewPR As DataView
        dvViewPR = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewPR.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewPR.Sort += " DESC"
        End If

        ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count

        ' Bind Datagrid
        If ViewState("intPageRecordCnt") > 0 Then 'intPageRecordCnt
            resetDatagridPageIndex(dtgPRList, dvViewPR)
            dtgPRList.DataSource = dvViewPR
            dtgPRList.DataBind()
        Else
            dtgPRList.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If
        ViewState("PageCount") = dtgPRList.PageCount
        cmdPO.Visible = True
        cmdRFQ.Visible = True
    End Function

    Private Sub dtgPRList_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPRList.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgPRList, e)
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")

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

            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            e.Item.Cells(EnumPR.icApprovalDate).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("PRM_PR_Date"))
            e.Item.Cells(EnumPR.icCost).Text = Format(Common.parseNull(dv("PM_LAST_TXN_PRICE"), 0), "#,##0.0000")

            dblAmt = Common.parseNull(dv("PM_LAST_TXN_PRICE"), 0) * Common.parseNull(dv("PRD_ORDERED_QTY"), 0)
            e.Item.Cells(EnumPR.icAmount).Text = Format(dblAmt, "#,##0.00")

            If Common.parseNull(dv("PM_LAST_TXN_TAX"), 0) > 0 Then
                'dblTaxAmt = (Common.parseNull(dv("PM_LAST_TXN_PRICE"), 0) * Common.parseNull(dv("PRD_ORDERED_QTY"), 0)) / Common.parseNull(dv("PM_LAST_TXN_TAX"), 0)
                dblTaxAmt = (Common.parseNull(dv("PM_LAST_TXN_PRICE"), 0) * Common.parseNull(dv("PRD_ORDERED_QTY"), 0)) * (Common.parseNull(dv("PM_LAST_TXN_TAX"), 0)) / 100
                e.Item.Cells(EnumPR.icTax).Text = Format(dblTaxAmt, "#,##0.00")
            Else
                dblTaxAmt = 0
                e.Item.Cells(EnumPR.icTax).Text = Format(dblTaxAmt, "#,##0.00")
            End If

            Dim lblProductDesc As Label
            lblProductDesc = e.Item.FindControl("lblProductDesc")
            lblProductDesc.Text = Common.parseNull(dv("PRD_PRODUCT_DESC"))

            Dim lblPRIndex As Label
            lblPRIndex = e.Item.FindControl("lblPRIndex")
            lblPRIndex.Text = Common.parseNull(dv("PRM_PR_Index"))

            Dim lblPRLine As Label
            lblPRLine = e.Item.FindControl("lblPRLine")
            lblPRLine.Text = Common.parseNull(dv("PRD_PR_LINE"))

            Dim lblBill As Label
            lblBill = e.Item.FindControl("lblBill")
            lblBill.Text = Common.parseNull(dv("PRM_B_ADDR_CODE"))

            Dim lblAtt As Label
            lblAtt = e.Item.FindControl("lblAtt")
            lblAtt.Text = Common.parseNull(dv("PRM_S_ATTN"))

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

            'Dim lblPRNo As Label
            'Dim STR As String
            'lblPRNo = e.Item.FindControl("lblPRNo")
            'STR = STR & "<A href=""" & dDispatcher.direct("PR", "PRDetail.aspx", "caller=ConvertPR&pageid=" & strPageId & "&index=" & dv("PRM_PR_Index") & "&PRNO=" & Common.parseNull(dv("PRM_PR_NO")) & "&type=mod&mode=bc") & """ ><font color=#0000ff>" & Common.parseNull(dv("PRM_PR_NO")) & "</font></A><br/>"
            'lblPRNo.Text = STR

            Dim lnkPRNo As HyperLink
            Dim lblPRNo As Label
            Dim STR As String

            lnkPRNo = e.Item.FindControl("lnkPRNo")
            STR = STR & "<A href=""" & dDispatcher.direct("PR", "PRDetail.aspx", "caller=ConvertPR&pageid=" & strPageId & "&index=" & dv("PRM_PR_Index") & "&PRNO=" & Common.parseNull(dv("PRM_PR_NO")) & "&type=mod&mode=bc") & """ ><font color=#0000ff>" & Common.parseNull(dv("PRM_PR_NO")) & "</font></A><br/>"
            lnkPRNo.Text = STR

            lblPRNo = e.Item.FindControl("lblPRNo")
            lblPRNo.Text = Common.parseNull(dv("PRM_PR_NO"))

            If Common.parseNull(dv("PRM_URGENT")) = "1" Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(1).Controls.Add(imgAttach)
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
                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PR", "ConvertPR.aspx", "pageid=" & strPageId) & """><span>Convert PR</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PR", "ConvertPRListing.aspx", "pageid=" & strPageId) & """><span>Convert PR Listing</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "</ul><div></div></div>"
    End Sub

    Private Sub cmdPO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPO.Click
        Dim strAryPR(0) As String
        Dim dgItem As DataGridItem
        Dim objPR As New PurchaseReq2
        Dim strPO, strPRNo As String
        Dim strMsg As String = ""
        Dim strSqlAry(0) As String
        Dim strPRItem As String

        Dim ChkVendor As String = ""
        Dim ChkCurr As String = ""
        Dim ChkCost As String = ""
        Dim ChkBill As String = ""
        Dim ChkAtt As String = ""
        Dim ChkPR As String = ""

        Dim multiplePR As Boolean = False
        Dim PRList As New ArrayList
        Dim strPRItemL As String

        For Each dgItem In dtgPRList.Items
            Dim chkSel As CheckBox
            chkSel = dgItem.Cells(EnumPR.icCheckBox).FindControl("chkSelection")
            If chkSel.Checked Then

                Dim lblBill As Label
                lblBill = dgItem.FindControl("lblBill")

                Dim lblAtt As Label
                lblAtt = dgItem.FindControl("lblAtt")

                If ChkVendor = "" Then
                    If dgItem.Cells(EnumPR.icVendor).Text = "&nbsp;" Then
                        Common.NetMsgbox(Me, "Selected item do not have vendor.") ' Error Cost
                        Exit Sub
                    End If
                    ChkVendor = dgItem.Cells(EnumPR.icVendor).Text
                    ChkCurr = dgItem.Cells(EnumPR.icCurrency).Text
                    ChkCost = dgItem.Cells(EnumPR.icCost).Text
                    ChkBill = lblBill.Text
                    ChkAtt = lblAtt.Text

                    If dgItem.Cells(EnumPR.icCost).Text = 0 Or dgItem.Cells(EnumPR.icCost).Text = "" Then
                        Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00029")) ' Error Cost
                        Exit Sub
                    End If
                Else
                    If dgItem.Cells(EnumPR.icVendor).Text = "&nbsp;" Then
                        Common.NetMsgbox(Me, "Selected item do not have vendor.") ' Error Cost
                        Exit Sub
                    End If

                    If ChkBill = lblBill.Text And lblBill.Text <> "" And lblBill.Text <> "&nbsp;" Then
                        If ChkAtt = lblAtt.Text Then
                            If ChkVendor = dgItem.Cells(EnumPR.icVendor).Text And dgItem.Cells(EnumPR.icVendor).Text <> "" And dgItem.Cells(EnumPR.icVendor).Text <> "&nbsp;" Then
                                If ChkCurr = dgItem.Cells(EnumPR.icCurrency).Text Then
                                    If dgItem.Cells(EnumPR.icCost).Text = 0 Or dgItem.Cells(EnumPR.icCost).Text = "" Then
                                        Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00029")) ' Error Cost
                                        Exit Sub
                                    End If
                                Else
                                    Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00030")) ' Error Curr
                                    Exit Sub
                                End If
                            Else
                                Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00030")) ' Error Vendor
                                Exit Sub
                            End If
                        Else
                            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00030")) ' Error Att
                            Exit Sub
                        End If
                    Else
                        Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00030")) ' Error Bill
                        Exit Sub
                    End If


                End If

                ' Common.Insert2Ary(strAryPR, dgItem.Cells(EnumPR.icPRNo).Text) 
                Dim lblPRLine As Label
                lblPRLine = dgItem.FindControl("lblPRLine")

                'If strPRItem = "" Then
                '    strPRItem = "'" & lblPRLine.Text & "'"
                'Else
                '    strPRItem &= ", '" & lblPRLine.Text & "'"
                'End If

                Dim lblPRNo As Label
                lblPRNo = dgItem.FindControl("lblPRNo")

                strPRNo = lblPRNo.Text 'dgItem.Cells(EnumPR.icPRNo).Text

                If ChkPR = "" Then
                    ChkPR = lblPRNo.Text 'dgItem.Cells(EnumPR.icPRNo).Text
                    strPRItem = "'" & lblPRLine.Text & "'"
                Else
                    If ChkPR = lblPRNo.Text Then 'dgItem.Cells(EnumPR.icPRNo).Text Then
                        strPRItem &= ", '" & lblPRLine.Text & "'"
                    Else
                        multiplePR = True
                        PRList.Add(New String() {ChkPR, ViewState("PRIndex"), strPRItem})
                        'strPRItemL = "'" & lblPRLine.Text & "'"
                        strPRItem = "'" & lblPRLine.Text & "'"
                    End If
                    ChkPR = lblPRNo.Text 'dgItem.Cells(EnumPR.icPRNo).Text
                End If

                Dim PR_No As String = objDB.GetVal("SELECT IFNULL(PRM_PR_NO,'') AS PRM_PR_NO FROM PR_MSTR, PR_DETAILS WHERE PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID AND PRD_PR_LINE = '" & lblPRLine.Text & "' AND PRM_PR_STATUS = '" & PRStatus.ConvertedToPO & "' AND PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRM_PR_NO = '" & lblPRNo.Text & "'")
                If PR_No <> "" Then
                    Common.NetMsgbox(Me, "Selected document " & lblPRNo.Text & " converted.")
                    Exit Sub
                End If

                Dim lblPRIndex As Label
                lblPRIndex = dgItem.FindControl("lblPRIndex")
                ViewState("PRIndex") = lblPRIndex.Text

            End If

        Next
        PRList.Add(New String() {ChkPR, ViewState("PRIndex"), strPRItem})

        ' strPO = objPR.CreatePO(strPRNo, intPRIndex, strVendor, strSqlAry, strBuyer)
        If multiplePR = True Then
            strPO = objPR.CreatePO(strPRNo, ViewState("PRIndex"), "", strSqlAry, Session("UserId"), strPRItem, True, PRList, , , False)
        Else
            strPO = objPR.CreatePO(strPRNo, ViewState("PRIndex"), "", strSqlAry, Session("UserId"), strPRItem, False, , , , False)
        End If

        If strPO = "error" Then
            strMsg = Common.MsgTransDup
            Common.NetMsgbox(Me, strMsg)
        End If

        Dim strPO_NO As String = ""
        If Not objDB.BatchExecuteNew(strSqlAry, , strPO, "T_NO") Then
            strMsg = "Error occurs. Kindly contact the Administrator to resolve this. "
        Else
            Dim arrPOInfo, arrPO As Array
            Dim intLowBound, intUpBound, intLoop As Integer
            Dim strPoNo, strVen As String
            Dim strSql1 As String
            Dim tDS As DataSet
            Dim strvendorname As String
            Dim strVendorNameList As String
            Dim objPR1 As New PR


            arrPOInfo = Split(strPO, ",")
            intLowBound = LBound(arrPOInfo)
            intUpBound = UBound(arrPOInfo)

            '' ''Dim strPO_NO As String = objDB.GetVal(" SELECT @PO_NO; ")
            '' ''If strPO_NO <> "" Then
            '' ''    strMsg = "Purchase Order Number " & strPO_NO & " has been created."
            '' ''End If

            For intLoop = 0 To intUpBound
                'To get the Vendor name
                arrPO = Split(arrPOInfo(intLoop), "!")
                strPoNo = arrPO(0) 'Capture the PO No.
                strVen = arrPO(1)  'Caputre the Vendor code


                strMsg = "Purchase Order Number " & strPoNo & " has been created."
                'strSql1 = "SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE  CM_COY_ID='" & strVen & "'"
                'tDS = objDB.FillDs(strSql1)
                'If tDS.Tables(0).Rows.Count > 0 Then
                '    strvendorname = tDS.Tables(0).Rows(0).Item("CM_COY_NAME")
                'End If
                'If intLoop = 0 Then
                '    ' strMsg = "Purchase Order Number " & strPoNo & " has been created for " & strvendorname & "."
                '    strMsg = "Purchase Order Number " & strPoNo & " has been created."
                '    strVendorNameList = strvendorname
                'Else
                '    strMsg = strMsg & """& vbCrLf & """ & "Purchase Order Number " & strPoNo & " has been created for " & strvendorname & "."
                '    strVendorNameList = strVendorNameList & ", " & strvendorname
                'End If

            Next

            If Session("urlreferer") = "Dashboard" Then
                Common.NetMsgbox(Me, strMsg, dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId), MsgBoxStyle.Exclamation)
            ElseIf Session("urlreferer") = "ConvertPR" Then
                Common.NetMsgbox(Me, strMsg)
                Bindgrid()
            End If
        End If

    End Sub

    Private Sub cmdRFQ_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRFQ.Click
        Dim strAryPR(0) As String
        Dim dgItem As DataGridItem
        Dim objPR As New PurchaseReq2
        Dim strRFQ, strPRNo As String
        Dim strMsg As String = ""
        Dim strSqlAry(0) As String
        Dim strPRItem As String
        Dim ChkPR As String = ""

        Dim ChkVendor As String = ""
        Dim ChkCurr As String = ""
        Dim ChkCost As String = ""

        Dim multiplePR As Boolean = False
        Dim PRList As New ArrayList

        For Each dgItem In dtgPRList.Items
            Dim chkSel As CheckBox
            chkSel = dgItem.Cells(EnumPR.icCheckBox).FindControl("chkSelection")
            If chkSel.Checked Then

                'If ChkVendor = "" Then
                '    ChkVendor = dgItem.Cells(EnumPR.icVendor).Text
                '    ChkCurr = dgItem.Cells(EnumPR.icCurrency).Text
                '    ChkCost = dgItem.Cells(EnumPR.icCost).Text
                'Else
                '    If ChkVendor = dgItem.Cells(EnumPR.icVendor).Text Then
                '        If ChkCurr <> dgItem.Cells(EnumPR.icCurrency).Text Then
                '            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00030")) ' Error Curr
                '            Exit Sub
                '        End If
                '    Else
                '        Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00030")) ' Error Vendor
                '        Exit Sub
                '    End If
                'End If

                '' ''Dim lblPRLine As Label
                '' ''lblPRLine = dgItem.FindControl("lblPRLine")

                '' ''If strPRItem = "" Then
                '' ''    strPRItem = "'" & lblPRLine.Text & "'"
                '' ''Else
                '' ''    strPRItem &= ", '" & lblPRLine.Text & "'"
                '' ''End If
                '' ''strPRNo = dgItem.Cells(EnumPR.icPRNo).Text

                Dim lblPRLine As Label
                lblPRLine = dgItem.FindControl("lblPRLine")

                Dim lblPRNo As Label
                lblPRNo = dgItem.FindControl("lblPRNo")

                strPRNo = lblPRNo.Text 'dgItem.Cells(EnumPR.icPRNo).Text

                If ChkPR = "" Then
                    ChkPR = lblPRNo.Text 'dgItem.Cells(EnumPR.icPRNo).Text

                    strPRItem = "'" & lblPRLine.Text & "'"
                Else
                    If ChkPR = lblPRNo.Text Then 'dgItem.Cells(EnumPR.icPRNo).Text Then
                        strPRItem &= ", '" & lblPRLine.Text & "'"
                    Else
                        multiplePR = True
                        PRList.Add(New String() {ChkPR, ViewState("PRIndex"), strPRItem})
                        strPRItem = "'" & lblPRLine.Text & "'"
                    End If
                    ChkPR = lblPRNo.Text 'dgItem.Cells(EnumPR.icPRNo).Text
                End If

                Dim lblPRIndex As Label
                lblPRIndex = dgItem.FindControl("lblPRIndex")
                ViewState("PRIndex") = lblPRIndex.Text

                Dim PR_No As String = objDB.GetVal("SELECT IFNULL(PRD_CONVERT_TO_DOC,'') AS PRM_PR_NO FROM PR_MSTR, PR_DETAILS WHERE PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID AND PRD_CONVERT_TO_IND = 'RFQ' AND PRD_PR_LINE = '" & lblPRLine.Text & "' AND PRM_PR_STATUS = '" & PRStatus.Approved & "' AND PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRM_PR_NO = '" & lblPRNo.Text & "'")
                If PR_No <> "" Then
                    Common.NetMsgbox(Me, "Selected document " & lblPRNo.Text & " converted.")
                    Exit Sub
                End If

            End If
        Next

        '' ''Dim lblPRIndex As Label
        '' ''lblPRIndex = dgItem.FindControl("lblPRIndex")
        '' ''ViewState("PRIndex") = lblPRIndex.Text

        PRList.Add(New String() {ChkPR, ViewState("PRIndex"), strPRItem})

        '' ''strRFQ = objPR.CreateRFQ(strPRNo, ViewState("PRIndex"), "", strSqlAry, Session("UserId"), strPRItem)

        If multiplePR = True Then
            strRFQ = objPR.CreateRFQMulti(strPRNo, ViewState("PRIndex"), "", strSqlAry, Session("UserId"), strPRItem, True, PRList, False)
        Else
            strRFQ = objPR.CreateRFQ(strPRNo, ViewState("PRIndex"), "", strSqlAry, Session("UserId"), strPRItem, False)
        End If

        If strRFQ = "error" Then
            strMsg = Common.MsgTransDup
            Common.NetMsgbox(Me, strMsg)
        End If

        If Not objDB.BatchExecute(strSqlAry) Then
            strMsg = "Error occurs. Kindly contact the Administrator to resolve this. "
        Else
            Dim arrRFQInfo, arrRFQ As Array
            Dim intLowBound, intUpBound, intLoop As Integer
            Dim strRFQNo, strVen As String
            Dim strSql1 As String
            Dim tDS As DataSet
            Dim strvendorname As String
            Dim strVendorNameList As String

            arrRFQInfo = Split(strRFQ, ",")
            intLowBound = LBound(arrRFQInfo)
            intUpBound = UBound(arrRFQInfo)

            For intLoop = 0 To intUpBound
                'To get the Vendor name
                arrRFQ = Split(arrRFQInfo(intLoop), "!")
                strRFQNo = arrRFQ(0) 'Capture the PO No.
                strVen = arrRFQ(1)  'Caputre the Vendor code

                strMsg = "Request For Quotation Number " & strRFQNo & " has been created."
            Next

            If Session("urlreferer") = "Dashboard" Then
                Common.NetMsgbox(Me, strMsg, dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId), MsgBoxStyle.Exclamation)
            ElseIf Session("urlreferer") = "ConvertPR" Then
                Common.NetMsgbox(Me, strMsg)
                Bindgrid()
            End If


        End If
    End Sub
End Class


