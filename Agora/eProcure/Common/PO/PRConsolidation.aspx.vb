Imports AgoraLegacy
Imports eProcure.Component
Public Class PRConsolidation
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents dtgPRList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button
    Protected WithEvents cmdPreview As System.Web.UI.WebControls.Button
    Protected WithEvents tblSearchResult As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents trMessage As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txtRemark As System.Web.UI.WebControls.TextBox
    Protected WithEvents cboConso As System.Web.UI.WebControls.DropDownList
    Protected WithEvents tdConso As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents RegularExpressionValidator1 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents hidSummary As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidControl As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Dim dDispatcher As New AgoraLegacy.dispatcher

    Public Enum EnumPR
        icCheckBox
        icPRNoLink
        icCreatedDate
        icBuyer
        icRemark
        icVendorName
        icCurrency
        icAmt
        icFreight
        icPayType
        icShipTerm
        icShipMode
        icPayTerm
        icShipVia
        icBillAddr
        icPRNo
        icVendorID
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
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        blnSorting = True
        blnPaging = False
        SetGridProperty(dtgPRList)
        If Not Page.IsPostBack Then
            '//relief assignment?
            Dim objPR As New PurchaseReq2
            Dim dvRelief As DataView
            dvRelief = objPR.getReliefConsolidator()
            If Not dvRelief Is Nothing Then
                tdConso.Style("VISIBILITY") = "visible"
                tdConso.Style("display") = ""
                Common.FillDdl(cboConso, "NAME", "RAM_USER_ID", dvRelief)
                Dim lstItem As New ListItem
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                cboConso.Items.Insert(0, lstItem)
            Else
                tdConso.Style("display") = "none"
                tdConso.Style("VISIBILITY") = "hidden"
            End If
            objPR = Nothing
            hidSummary.Value = "Remarks-" & txtRemark.ClientID
            viewstate("SortAscending") = "no"
            viewstate("SortExpression") = "PRM_PR_NO"
            Bindgrid(True)
        Else
            hidSummary.Value = "Remarks-" & txtRemark.ClientID
        End If

        txtRemark.Attributes.Add("onKeyDown", "limitText (this, 1000);")
        cmdPreview.Attributes.Add("onClick", "return submitOnClick('chkSelection',1,0);")
        cmdSubmit.Attributes.Add("onClick", "return submitOnClick('chkSelection',1,0);")
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String

        Dim objPR As New PurchaseReq2

        '//Retrieve Data from Database
        Dim ds As DataSet

        If cboConso.SelectedIndex <= 0 Then
            ds = objPR.getPRConsolidation(Session("UserId"))
        Else
            ds = objPR.getPRConsolidation(cboConso.SelectedValue)
        End If

        '//for sorting asc or desc
        Dim dvViewPR As DataView
        dvViewPR = ds.Tables(0).DefaultView
        dvViewPR.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" And viewstate("SortExpression") <> "" Then dvViewPR.Sort += " DESC"

        intPageRecordCnt = ds.Tables(0).Rows.Count

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            cmdReset.Disabled = False
            cmdSubmit.Enabled = True
            cmdPreview.Enabled = True
            trMessage.Style("display") = "none"
            tblSearchResult.Style("display") = ""
            dtgPRList.DataSource = dvViewPR
            dtgPRList.DataBind()
        Else
            cmdReset.Disabled = True
            cmdSubmit.Enabled = False
            cmdPreview.Enabled = False
            dtgPRList.DataBind()
            trMessage.Style("display") = ""
            tblSearchResult.Style("display") = "none"
        End If
    End Function



    Private Sub dtgPRList_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPRList.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dtgPRList, e)
        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Private Sub dtgPRList_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPRList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box

            Dim chk As CheckBox
            chk = e.Item.Cells(EnumPR.icCheckBox).FindControl("chkSelection")
            If Not ViewState("strAryPRIndex") Is Nothing Then
                Dim intLoop As Integer
                Dim strAryPRIndex() As String
                strAryPRIndex = ViewState("strAryPRIndex")

                If strAryPRIndex.Length > 0 Then
                    For intLoop = 0 To strAryPRIndex.GetUpperBound(0)
                        If strAryPRIndex(intLoop) = dv("PRM_PR_Index") Then
                            chk.Checked = True
                            ' Exit For
                        End If
                    Next
                End If
            End If

            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
            '//to dynamic build hyperlink
            Dim lnkPRNo As HyperLink
            lnkPRNo = e.Item.Cells(EnumPR.icPRNoLink).FindControl("lnkPRNo")
            lnkPRNo.NavigateUrl = dDispatcher.direct("PO", "PRDetail.aspx", "caller=PR&PageID=" & strPageId & "&index=" & dv("PRM_PR_Index") & "&PRNo=" & dv("PRM_PR_No"))
            lnkPRNo.Text = dv("PRM_PR_No")
            If Not IsDBNull(dv("PRM_PR_COST")) Then
                e.Item.Cells(EnumPR.icAmt).Text = Format(dv("PRM_PR_COST"), "#,##0.00")
            Else
                e.Item.Cells(EnumPR.icAmt).Text = "0.00"
            End If

            Dim txtQ As TextBox
            txtQ = e.Item.FindControl("txtQ")

            Dim hidCode As HtmlInputHidden
            hidCode = e.Item.FindControl("hidCode")

            e.Item.Cells(EnumPR.icCreatedDate).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("PRM_PR_Date"))
        End If
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        ConsolidatePR(False)
    End Sub

    Private Sub ConsolidatePR(ByVal blnPreview As Boolean)
        Dim strAryPR(0), strMsg(0) As String
        Dim strAryPRIndex(0), strError, strVendor As String
        strError = ParseGrid(strAryPR, strAryPRIndex, strVendor)
        If strError <> "" Then
            Common.NetMsgbox(Me, strError, MsgBoxStyle.Exclamation)
        Else
            Dim objPR As New PurchaseReq2
            Dim blnSuccess As Boolean
            Dim strConso As String
            '//need to relief????
            If cboConso.SelectedIndex <= 0 Then
                strConso = Session("UserID")
            Else
                strConso = cboConso.SelectedValue
            End If
            blnSuccess = objPR.PRConsolidation(strAryPR, strAryPRIndex, strVendor, txtRemark.Text, strConso, strMsg, blnPreview, False)

            Dim intCnt As Integer
            For intCnt = LBound(strMsg) To UBound(strMsg)
                If strError = "" Then
                    strError = strMsg(intCnt)
                Else
                    strError = strError & """& vbCrLf & """ & strMsg(intCnt)
                End If
            Next

            If blnSuccess Then
                If blnPreview Then
                    Dim vbs As String
                    vbs = vbs & "<script language=""javascript"">" & vbCrLf
                    vbs = vbs & "window.open('" & dDispatcher.direct("PO", "POReport.aspx", "pageid=" & strPageId & "&prev=true&po_no=" & strMsg(0) & "&side=other&BCoyID=" & Session("CompanyID")) & "','','width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes')" & vbCrLf
                    vbs = vbs & "</script>"
                    Dim rndKey As New Random
                    Me.RegisterStartupScript(rndKey.Next.ToString, vbs)
                Else
                    Common.NetMsgbox(Me, strError, MsgBoxStyle.Exclamation)
                End If
            Else
                Common.NetMsgbox(Me, strError, MsgBoxStyle.Exclamation)
            End If
        End If
        viewstate("SortAscending") = "no"
        viewstate("SortExpression") = "PRM_PR_NO"
        Bindgrid(True)
        ParseGrid_CheckBox()
    End Sub

    Private Function ParseGrid_CheckBox()
        If Not ViewState("strAryPRIndex") Is Nothing Then
            Dim dgItem As DataGridItem
            Dim strAryPRIndex() As String
            Dim intCnt, intCnt1 As Integer
            intCnt1 = 0
            strAryPRIndex = ViewState("strAryPRIndex")
            intCnt = dtgPRList.Items.Count
            For Each dgItem In dtgPRList.Items
                Dim chk As CheckBox
                chk = dgItem.Cells(EnumPR.icCheckBox).FindControl("chkSelection")
                Dim intLoop As Integer
                If strAryPRIndex.Length > 0 Then
                    For intLoop = 0 To strAryPRIndex.GetUpperBound(0)
                        If strAryPRIndex(intLoop) = dtgPRList.DataKeys(dgItem.ItemIndex) Then
                            chk.Checked = True
                            intCnt1 += 1
                            ' Exit For
                        End If
                    Next
                End If
            Next

            If intCnt = intCnt1 Then
                '//CheckAll
                Dim vbs As String
                vbs = vbs & "<script language=""javascript"">" & vbCrLf
                vbs = vbs & "CheckAll('dtgPRList_ctl01_chkAll','chkSelection');" & vbCrLf
                vbs = vbs & "</script>"
                Dim rndKey As New Random
                Me.RegisterStartupScript(rndKey.Next.ToString, vbs)
            End If
        End If
    End Function

    Private Function ParseGrid(ByRef strAryPR() As String, ByRef strAryPRIndex() As String, ByRef strVendor As String) As String
        Dim dgItem As DataGridItem
        Dim strBillAddr, strCurrency, strPayTerm As String
        Dim strPayType, strShipTerm, strShipMode, strShipVia, strFreight As String
        Dim strError1, strError2, strError3, strError4, strError5 As String
        Dim strError6, strError7, strError8, strError9, strError As String
        Dim blnError As Boolean = False

        Dim intCnt As Integer = 0
        For Each dgItem In dtgPRList.Items
            Dim chkSel As CheckBox
            chkSel = dgItem.Cells(EnumPR.icCheckBox).FindControl("chkSelection")
            If chkSel.Checked Then
                Common.Insert2Ary(strAryPR, dgItem.Cells(EnumPR.icPRNo).Text)
                Common.Insert2Ary(strAryPRIndex, dtgPRList.DataKeys(dgItem.ItemIndex))
                ViewState("strAryPRIndex") = strAryPRIndex
                intCnt += 1
                If intCnt = 1 Then '//get first selected PR and stored into variable
                    strVendor = dgItem.Cells(EnumPR.icVendorID).Text
                    strBillAddr = dgItem.Cells(EnumPR.icBillAddr).Text
                    strCurrency = dgItem.Cells(EnumPR.icCurrency).Text
                    strPayTerm = dgItem.Cells(EnumPR.icPayTerm).Text
                    strPayType = dgItem.Cells(EnumPR.icPayType).Text
                    strShipTerm = dgItem.Cells(EnumPR.icShipTerm).Text
                    strShipMode = dgItem.Cells(EnumPR.icShipMode).Text
                    strShipVia = dgItem.Cells(EnumPR.icShipVia).Text
                    strFreight = dgItem.Cells(EnumPR.icFreight).Text
                Else
                    If strVendor.ToLower <> dgItem.Cells(EnumPR.icVendorID).Text.ToLower Then
                        strError1 = "Vendor Companies are different!"
                        blnError = True
                    End If

                    If strBillAddr.ToLower <> dgItem.Cells(EnumPR.icBillAddr).Text.ToLower Then
                        strError2 = "Billing Addresses are different !"
                        blnError = True
                    End If

                    If strCurrency.ToLower <> dgItem.Cells(EnumPR.icCurrency).Text.ToLower Then
                        strError3 = "Currencies are different !"
                        blnError = True
                    End If

                    If strPayTerm.ToLower <> dgItem.Cells(EnumPR.icPayTerm).Text.ToLower Then
                        strError4 = "Payment Term are different !"
                        blnError = True
                    End If

                    If strPayType.ToLower <> dgItem.Cells(EnumPR.icPayType).Text.ToLower Then
                        strError5 = "Payment Type are different !"
                        blnError = True
                    End If

                    If strShipTerm.ToLower <> dgItem.Cells(EnumPR.icShipTerm).Text.ToLower Then
                        strError6 = "Shipment Term are different !"
                        blnError = True
                    End If

                    If strShipMode.ToLower <> dgItem.Cells(EnumPR.icShipMode).Text.ToLower Then
                        strError7 = "Shipment Mode are different !"
                        blnError = True
                    End If

                    If strShipVia.ToLower <> dgItem.Cells(EnumPR.icShipVia).Text.ToLower Then
                        strError8 = "Ship Via are different !"
                        blnError = True
                    End If
                    If strFreight.ToLower <> dgItem.Cells(EnumPR.icFreight).Text.ToLower Then
                        strError9 = "Freight Term are different !"
                        blnError = True
                    End If
                End If
            End If
        Next

        If blnError Then
            strError = "The following problem occured.Consolidation of PR not successful. Please select again."
            If strError1 <> "" Then '//Vendor
                strError = strError & """& vbCrLf & ""-- " & strError1
            End If
            If strError2 <> "" Then '//Billing Addr
                strError = strError & """& vbCrLf & ""-- " & strError2
            End If
            If strError3 <> "" Then '//currency
                strError = strError & """& vbCrLf & ""-- " & strError3
            End If
            If strError4 <> "" Then '//payment Term
                strError = strError & """& vbCrLf & ""-- " & strError4
            End If
            If strError5 <> "" Then '//payment type
                strError = strError & """& vbCrLf & ""-- " & strError5
            End If
            If strError6 <> "" Then '//shipment term
                strError = strError & """& vbCrLf & ""-- " & strError6
            End If
            If strError7 <> "" Then '//shipment mode
                strError = strError & """& vbCrLf & ""-- " & strError7
            End If
            If strError8 <> "" Then '//ship via
                strError = strError & """& vbCrLf & ""-- " & strError8
            End If
            If strError9 <> "" Then '//freight term
                strError = strError & """& vbCrLf & ""-- " & strError9
            End If
        Else
            strError = ""
        End If
        Return strError
    End Function

    Private Sub cmdPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPreview.Click
        ConsolidatePR(True)
    End Sub

    Private Sub cboConso_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboConso.SelectedIndexChanged
        viewstate("SortAscending") = "no"
        viewstate("SortExpression") = "PRM_PR_NO"
        Bindgrid()
    End Sub

   
    Sub SortCommand_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgPRList.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub
End Class


