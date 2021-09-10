Imports AgoraLegacy
Imports eProcure.Component
Imports System.Text.RegularExpressions
Public Class SearchInventoryReq_AO
    Inherits AgoraLegacy.AppBaseClass
    Dim objGlobal As New AppGlobals
    Dim objINV As New Inventory
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim aryInvReq As New ArrayList()
    Dim aryIR, aryMRS As New ArrayList()
    Dim blnRelief As Boolean = False

    Enum EnumInvReq
        icCheckBox
        icIRNo
        icIRDt
        icReqName
        icIssueTo
        icDept
        icRefNo
        icRemark
        icIndex
    End Enum

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgItem)

        cmdMassApp.Enabled = True

        If Not IsPostBack Then
            txt_startdate.Text = DateAdd("m", -6, DateTime.Now.ToShortDateString())
            hidDateS.Value = txt_startdate.Text
            txt_enddate.Text = DateTime.Now.ToShortDateString()
            hidDateE.Value = txt_enddate.Text
            GenerateTab()
            Bindgrid()
        End If

        'Check Access Status
        If Session("AccessStatus") = "Limited" Then
            cmdMassApp.Enabled = False
        End If

        Session("urlreferer") = "SearchInventoryReq_AO"
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        Session("w_InventoryReq_AO_tabs") = "<div class=""t_entity""><ul>" & _
                                    "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "SearchInventoryReq_AO.aspx", "pageid=" & strPageId) & """><span>Approval List</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "SearchInventoryReq_All.aspx", "pageid=" & strPageId) & """><span>Approved / Rejected Listing</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                                   "</ul><div></div></div>"


    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgItem.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Public Sub MyDataGrid_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtgItem.CurrentPageIndex = e.NewPageIndex
        Bindgrid(True)
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objInv As New Inventory
        Dim ds As DataSet
        Dim strMsg As String
        Dim comparedt As Date
        comparedt = DateAdd("m", -6, CDate(txt_enddate.Text))

        If CDate(txt_startdate.Text) < comparedt Then
            strMsg = "Start/ End date " & objGlobal.GetErrorMessage("00015") & " 6 months"
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
            Return False
        End If

        ds = objInv.GetInvReqApprList(txtIRNumber.Text, txtReqName.Text, txtIssue.Text, txtDepartment.Text, txt_startdate.Text, txt_enddate.Text)

        '//for sorting asc or desc
        Dim dvViewItem As DataView
        dvViewItem = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewItem.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewItem.Sort += " DESC"
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgItem, dvViewItem)
            dtgItem.DataSource = dvViewItem
            dtgItem.DataBind()
            Dim objApp As New ApprWorkFlow
            If objApp.checkMassApp(Session("UserID")) = 1 Then
                cmdMassApp.Enabled = True
            Else
                cmdMassApp.Enabled = False
                cmdMassApp.Visible = False
            End If
        Else
            cmdMassApp.Enabled = False
            dtgItem.DataBind()
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00006"))
        End If
        ViewState("PageCount") = dtgItem.PageCount
    End Function

    Private Sub dtgItem_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgItem.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgItem, e)

        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Private Sub dtgItem_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgItem.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim chk As CheckBox
            chk = e.Item.Cells(EnumInvReq.icCheckBox).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            '//to dynamic build hyperlink
            Dim lnkCode As HyperLink
            lnkCode = e.Item.Cells(EnumInvReq.icIRNo).FindControl("lnkCode")
            lnkCode.NavigateUrl = "" & dDispatcher.direct("Inventory", "IRApprDetail.aspx", "caller=approval&AO=" & Session("UserId") & "&relief=" & blnRelief & "&PageID=" & strPageId & "&IRNo=" & dv("IRM_IR_NO") & "&index=" & dv("IRM_IR_INDEX") & "")
            lnkCode.Text = dv("IRM_IR_NO")

            If Common.parseNull(dv("IRM_IR_URGENT")) = "Y" Then
                Dim lnkUrgent As New HyperLink
                lnkUrgent.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(EnumInvReq.icIRNo).Controls.Add(lnkUrgent)
            End If

            e.Item.Cells(EnumInvReq.icIRDt).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("IRM_IR_DATE"))

        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgItem.CurrentPageIndex = 0
        ViewState("SortAscending") = "yes"
        ViewState("SortExpression") = "IRM_IR_DATE"
        Bindgrid()
    End Sub

    Private Sub cmdMassApp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdMassApp.Click
        Dim strAryIR(0), strMsg(0) As String
        Dim strAryIRIndex(0), strAO As String
        Dim objInv As New Inventory
        Dim dgItem As DataGridItem

        For Each dgItem In dtgItem.Items
            Dim chkSel As CheckBox
            chkSel = dgItem.Cells(EnumInvReq.icCheckBox).FindControl("chkSelection")
            If chkSel.Checked Then
                Common.Insert2Ary(strAryIRIndex, dgItem.Cells(EnumInvReq.icIndex).Text)
            End If
        Next

        strAO = Session("UserId")

        objInv.MassApprovalIR(strAryIRIndex, strAO, strMsg, blnRelief)
        If strMsg.Length > 0 Then
            Dim intLoop, intCnt As Integer
            Dim strMsg1 As String
            intCnt = strMsg.Length
            For intLoop = 0 To intCnt - 1
                If intLoop = 0 Then
                    strMsg1 = strMsg(intLoop)
                Else
                    strMsg1 = strMsg1 & """& vbCrLf & """ & strMsg(intLoop)
                End If
            Next
            Common.NetMsgbox(Me, strMsg1)
        End If
        Bindgrid()
    End Sub
End Class