'Copyright © 2013 STRATEQ GLOBAL SERVICES. All rights reserved.
Imports AgoraLegacy
Imports eProcure.Component
Imports System.Text.RegularExpressions
Public Class MRSAckListing
    Inherits AgoraLegacy.AppBaseClass
    Dim objGlobal As New AppGlobals
    Dim objINV As New Inventory
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim aryInvReq As New ArrayList()
    Dim aryIR, aryMRS As New ArrayList()
    Dim blnRelief As Boolean = False

    Enum EnumMRS
        icCheckBox
        icMRSNo
        icMRSDt
        icIssueDt
        icAccCode
        icIssueTo
        icDept
        icRefNo
        icRemark
        icIndex
    End Enum

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgItem)

        cmdMassAck.Enabled = True

        If Not IsPostBack Then
            txt_startdate.Text = DateAdd("m", -6, DateTime.Now.ToShortDateString())
            hidDateS.Value = txt_startdate.Text
            txt_enddate.Text = DateTime.Now.ToShortDateString()
            hidDateE.Value = txt_enddate.Text
            txt_issuestartdate.Text = DateAdd("m", -6, DateTime.Now.ToShortDateString())
            hidIssueDateS.Value = txt_issuestartdate.Text
            txt_issueenddate.Text = DateTime.Now.ToShortDateString()
            hidIssueDateE.Value = txt_issueenddate.Text
            GenerateTab()
            Bindgrid()
        End If

        'Check Access Status
        If Session("AccessStatus") = "Limited" Then
            cmdMassAck.Enabled = False
        End If

        Session("urlreferer") = "MRSAckListing"
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        Session("w_MRSAckListing_tabs") = "<div class=""t_entity""><ul>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "InventoryReq.aspx", "pageid=" & strPageId) & """><span>Inventory Requisition</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "InventoryReqList.aspx", "pageid=" & strPageId) & """><span>Inventory Requisition Listing</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "MRSAckListing.aspx", "pageid=" & strPageId) & """><span>MRS Acknowledge Listing</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "MRSListing_Requestor.aspx", "pageid=" & strPageId) & """><span>MRS Listing</span></a></li>" & _
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

        comparedt = DateAdd("m", -6, CDate(txt_enddate.Text))

        If CDate(txt_issuestartdate.Text) < comparedt Then
            strMsg = "Issued Start/ End date " & objGlobal.GetErrorMessage("00015") & " 6 months"
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
            Return False
        End If

        ds = objInv.GetAckMRSList(txtMRSNumber.Text, txtAccCode.Text, txtIssue.Text, txtDepartment.Text, txt_startdate.Text, txt_enddate.Text, txt_issuestartdate.Text, txt_issueenddate.Text)

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
            cmdMassAck.Enabled = True
            'Dim objApp As New ApprWorkFlow
            'If objApp.checkMassApp(Session("UserID"), "sk") = 1 Then
            '    cmdMassAck.Enabled = True
            'Else
            '    cmdMassAck.Enabled = False
            '    cmdMassAck.Visible = False
            'End If
        Else
            cmdMassAck.Enabled = False
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
            chk = e.Item.Cells(EnumMRS.icCheckBox).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            '//to dynamic build hyperlink
            Dim lnkCode As HyperLink
            lnkCode = e.Item.Cells(EnumMRS.icMRSNo).FindControl("lnkCode")
            lnkCode.NavigateUrl = "" & dDispatcher.direct("Inventory", "MRSAckDetail.aspx", "PageID=" & strPageId & "&MRSNo=" & dv("IRSM_IRS_NO") & "&index=" & dv("IRSM_IRS_INDEX") & "")
            lnkCode.Text = dv("IRSM_IRS_NO")

            If Common.parseNull(dv("IRSM_IRS_URGENT")) = "Y" Then
                Dim lnkUrgent As New HyperLink
                lnkUrgent.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(EnumMRS.icMRSNo).Controls.Add(lnkUrgent)
            End If

            e.Item.Cells(EnumMRS.icMRSDt).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("IRSM_IRS_DATE"))
            e.Item.Cells(EnumMRS.icIssueDt).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("IRSM_IRS_APPROVED_DATE"))

        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgItem.CurrentPageIndex = 0
        ViewState("SortAscending") = "yes"
        ViewState("SortExpression") = "IRSM_IRS_DATE"
        Bindgrid()
    End Sub

    Private Sub cmdMassAck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdMassAck.Click
        Dim strMsg(0) As String
        Dim strAryMRSIndex(0), strAO As String
        Dim objInv As New Inventory
        Dim dgItem As DataGridItem

        For Each dgItem In dtgItem.Items
            Dim chkSel As CheckBox
            chkSel = dgItem.Cells(EnumMRS.icCheckBox).FindControl("chkSelection")
            If chkSel.Checked Then
                Common.Insert2Ary(strAryMRSIndex, dgItem.Cells(EnumMRS.icIndex).Text)
            End If
        Next

        strAO = Session("UserId")

        If objInv.MassAcknowledgeMRS(strAryMRSIndex, strMsg) Then
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
        End If
        
        Bindgrid()
    End Sub
End Class