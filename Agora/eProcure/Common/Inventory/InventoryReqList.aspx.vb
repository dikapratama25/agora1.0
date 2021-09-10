'Copyright © 2011 STRATEQ GLOBAL SERVICES. All rights reserved.
Imports AgoraLegacy
Imports eProcure.Component
Imports System.Text.RegularExpressions
Public Class InventoryReqList
    Inherits AgoraLegacy.AppBaseClass
    Dim objGlobal As New AppGlobals
    Dim objINV As New Inventory
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim aryInvReq As New ArrayList()
    Dim aryIR, aryMRS As New ArrayList()

    Enum EnumInvReq
        icIRNo
        icCreatedDt
        icAppDt
        icIssueTo
        icDept
        icRefNo
        icRemark
        icIRStatus
        icMRSNo
        icMRSStatus
    End Enum

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgItem)
        If Not IsPostBack Then
            GenerateTab()

            txt_startdate.Text = DateAdd("m", -6, DateTime.Now.ToShortDateString())
            hidDateS.Value = txt_startdate.Text
            txt_enddate.Text = DateTime.Now.ToShortDateString()
            hidDateE.Value = txt_enddate.Text
        End If
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        Session("w_InventoryReq_tabs") = "<div class=""t_entity""><ul>" & _
                                    "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "InventoryReq.aspx", "pageid=" & strPageId) & """><span>Inventory Requisition</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "InventoryReqList.aspx", "") & """><span>Inventory Requisition Listing</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "MRSAckListing.aspx", "pageid=" & strPageId) & """><span>MRS Acknowledge Listing</span></a></li>" & _
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
        Bindgrid(0)
    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim ds As DataSet = New DataSet
        Dim strMsg As String

        Dim comparedt As Date
        comparedt = DateAdd("m", -6, CDate(txt_enddate.Text))

        If CDate(txt_startdate.Text) < comparedt Then
            strMsg = "Start date " & objGlobal.GetErrorMessage("00015") & " 6 months"
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
            Return False
        End If

        'IR Status
        aryIR.Clear()
        aryIR.Add(IIf(chkSubmit.Checked = True, "Y", "N"))
        aryIR.Add(IIf(chkApprove.Checked = True, "Y", "N"))
        aryIR.Add(IIf(chkPending.Checked = True, "Y", "N"))
        aryIR.Add(IIf(chkReject.Checked = True, "Y", "N"))

        'MRS Status
        aryMRS.Clear()
        aryMRS.Add(IIf(chkNew.Checked = True, "Y", "N"))
        aryMRS.Add(IIf(chkIssue.Checked = True, "Y", "N"))
        aryMRS.Add(IIf(chkPartialIssue.Checked = True, "Y", "N"))
        aryMRS.Add(IIf(chkAcknowledge.Checked = True, "Y", "N"))
        aryMRS.Add(IIf(chkAuto.Checked = True, "Y", "N"))
        aryMRS.Add(IIf(chkCancel.Checked = True, "Y", "N"))
        aryMRS.Add(IIf(chkRejected.Checked = True, "Y", "N"))

        ds = objINV.getInventoryReqFiltered(Me.txtIRNumber.Text, Me.txtIssue.Text, Me.txtDepartment.Text, Me.txt_startdate.Text, Me.txt_enddate.Text, aryIR, aryMRS)
        'ds = objINV.getInventoryTransFiltered(Me.txtIRNumber.Text, Me.txtIssue.Text, Me.txt_startdate.Text, Me.txt_enddate.Text, Me.txtDepartment.Text)
        Dim dvViewItem As DataView
        dvViewItem = ds.Tables(0).DefaultView

        dvViewItem.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewItem.Sort += " DESC"

        ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count
        If ViewState("intPageRecordCnt") > 0 Then
            resetDatagridPageIndex(dtgItem, dvViewItem)

            dtgItem.DataSource = dvViewItem
            dtgItem.DataBind()
        Else
            dtgItem.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        ViewState("PageCount") = dtgItem.PageCount
    End Function

    Private Sub dtgItem_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgItem.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgItem, e)
    End Sub

    Private Sub dtgItem_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgItem.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim dsMrs As New DataSet
            Dim i As Integer
            Dim strMRS, strMRS2, strStatus As String
            Dim lnkCode As HyperLink
            Dim IRNo, IRDate, IssueTo, RefNo, Remark As String

            IRNo = Common.parseNull(dv("IRM_IR_NO"))
            IRDate = Common.parseNull(dv("IRM_CREATED_DATE"))
            IssueTo = Common.parseNull(dv("IRM_IR_ISSUE_TO"))
            RefNo = Common.parseNull(dv("IRM_IR_REF_NO"))
            Remark = Common.parseNull(dv("IRM_IR_REMARK"))

            lnkCode = e.Item.Cells(EnumInvReq.icIRNo).FindControl("lnkCode")
            'lnkCode.NavigateUrl = "" & dDispatcher.direct("Inventory", "InventoryReqInfo.aspx", "IRNo=" & Server.UrlEncode(IRNo) & "&IRDate=" & IRDate & "&IssueTo=" & Server.UrlEncode(IssueTo) & "&RefNo=" & Server.UrlEncode(RefNo) & "&Remark=" & Server.UrlEncode(Remark) & "")
            lnkCode.NavigateUrl = "" & dDispatcher.direct("Inventory", "InventoryReqInfo.aspx", "caller=InventoryReqList&pageid=" & strPageId & "&index=" & dv("IRM_IR_INDEX") & "&IRNo=" & dv("IRM_IR_NO"))
            lnkCode.Text = dv("IRM_IR_NO")

            If Common.parseNull(dv("IRM_IR_URGENT")) = "Y" Then
                Dim lnkUrgent As New HyperLink
                lnkUrgent.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(EnumInvReq.icIRNo).Controls.Add(lnkUrgent)
            End If

            e.Item.Cells(EnumInvReq.icCreatedDt).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("IRM_CREATED_DATE"))

            If IsDBNull(dv("IRM_IR_APPROVED_DATE")) Then
                e.Item.Cells(EnumInvReq.icAppDt).Text = ""
            Else
                e.Item.Cells(EnumInvReq.icAppDt).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("IRM_IR_APPROVED_DATE"))
            End If

            If dv("IRM_IR_STATUS") = "1" Then
                e.Item.Cells(EnumInvReq.icIRStatus).Text = "Submitted"
            ElseIf dv("IRM_IR_STATUS") = "2" Then
                e.Item.Cells(EnumInvReq.icIRStatus).Text = "Approved"
            ElseIf dv("IRM_IR_STATUS") = "3" Then
                e.Item.Cells(EnumInvReq.icIRStatus).Text = "Pending Approval"
            ElseIf dv("IRM_IR_STATUS") = "4" Then
                e.Item.Cells(EnumInvReq.icIRStatus).Text = "Rejected"
            Else
                e.Item.Cells(EnumInvReq.icIRStatus).Text = ""
            End If

            dsMrs = objINV.GetMRSNoAllIR(IRNo, aryMRS)
            If dsMrs.Tables(0).Rows.Count > 0 Then
                For i = 0 To dsMrs.Tables(0).Rows.Count - 1
                    strMRS = strMRS & "<A href=""" & dDispatcher.direct("Inventory", "MRSDetail.aspx", "caller=InventoryReqList&pageid=" & strPageId & "&index=" & dsMrs.Tables(0).Rows(i)("IRSM_IRS_INDEX") & "&MRSNo=" & dsMrs.Tables(0).Rows(i)("IRSM_IRS_NO")) & """ ><font color=#0000ff>" & dsMrs.Tables(0).Rows(i)("IRSM_IRS_NO") & "</font></A><br/>"

                    If dsMrs.Tables(0).Rows(i)("IRSM_IRS_STATUS") = "1" Then
                        strStatus = "New"
                    ElseIf dsMrs.Tables(0).Rows(i)("IRSM_IRS_STATUS") = "2" Then
                        strStatus = "Issued"
                    ElseIf dsMrs.Tables(0).Rows(i)("IRSM_IRS_STATUS") = "3" Then
                        strStatus = "Acknowledged"
                    ElseIf dsMrs.Tables(0).Rows(i)("IRSM_IRS_STATUS") = "4" Then
                        strStatus = "Auto-Acknowledged"
                    ElseIf dsMrs.Tables(0).Rows(i)("IRSM_IRS_STATUS") = "5" Then
                        strStatus = "Cancelled"
                    ElseIf dsMrs.Tables(0).Rows(i)("IRSM_IRS_STATUS") = "6" Then
                        strStatus = "Rejected"
                    ElseIf dsMrs.Tables(0).Rows(i)("IRSM_IRS_STATUS") = "7" Then
                        strStatus = "Partial Issued"
                    Else
                        strStatus = ""
                    End If
                    strMRS2 = strMRS2 & strStatus & "<br/>"
                Next

                e.Item.Cells(EnumInvReq.icMRSNo).Text = strMRS
                e.Item.Cells(EnumInvReq.icMRSStatus).Text = strMRS2
            Else
                e.Item.Cells(EnumInvReq.icMRSStatus).Text = ""
                e.Item.Cells(EnumInvReq.icMRSNo).Text = ""
            End If
        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgItem.CurrentPageIndex = 0
        ViewState("SortAscending") = "yes"
        ViewState("SortExpression") = "IRM_CREATED_DATE"
        Bindgrid()
    End Sub
End Class