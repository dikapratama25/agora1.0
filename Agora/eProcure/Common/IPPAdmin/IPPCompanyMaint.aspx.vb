Imports AgoraLegacy
Imports eProcure.Component

Public Class IPPCompanyMaint
    Inherits AgoraLegacy.AppBaseClass
    Dim objGlobal As New AppGlobals
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim strCoyCode As String
    Dim strCoyName As String
    Dim blnAsset As Boolean
    Dim blnSub As Boolean
    Dim strCoyType As String
    Dim strCoyStatus As String
    Dim blnchkActive As Boolean
    Dim blnchkInactive As Boolean
    Dim strPaymentMethod As String
    Dim strConIBSCode As String
    Dim strNonConIBSCode As String
    Dim strBusinessRegNo As String
    Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID") 'Chee Hong - 04/Feb/2015 (IPP GST Stage 2A)

    Enum enumComp
        icChk
        icIndex
        icCoyType
        icCoyAbbr
        icCoyName
        icCoyRegNo
        icBillGLCode1 'Zulham 29062015 - HLB-IPP GST Stage 4(CR)
        icGstRegNo
        icGstInputTaxCode
        icBillGLCode2 'Zulham 29062015 - HLB-IPP GST Stage 4(CR)
        icStaffDate
        icJobGrade
        icBranchCode
        icCostCentre
        icPayMode
        icBankName
        icBankAcc
        icIBSGLCode
        icNonIBSGLCode
        icStatus
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
        cmdAdd.Enabled = True '20110628-default False
        cmdDelete.Enabled = True '20110628-default False
        cmdModify.Enabled = True '20110628-default False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmdAdd)
        htPageAccess.Add("add", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdModify)
        htPageAccess.Add("modify", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdDelete)
        htPageAccess.Add("delete", alButtonList)
        alButtonList.Clear()

    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objIPPCoy As New IPP
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgIPPCompany)

        If Not Page.IsPostBack Then
            cmdDelete.Visible = False
            cmdModify.Visible = False
            Session("action") = ""
            objIPPCoy.GetIPPCompanyInfo(strCoyCode, strCoyName, strCoyType, strCoyStatus)

            'Chee Hong - 17/12/2014 - Only allow HLB IPP Admin for action (IPP GST Stage 2A)
            If strDefIPPCompID <> "" And strDefIPPCompID <> Session("CompanyId") Then
                cmdAdd.Visible = False
            Else
                cmdAdd.Visible = True
            End If
        End If

        If Session("action") = "Modify" Or Session("action") = "Add" Or Session("action") = "Update" Then
            objIPPCoy.GetIPPCompanyInfo(strCoyCode, strCoyName, strCoyType, strCoyStatus)
            Bindgrid()
            Session("action") = ""
        End If

        cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOneDtg('chkSelection','delete');")
        cmdModify.Attributes.Add("onclick", "return CheckAtLeastOneDtg('chkSelection','modify');")
        'cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
        objIPPCoy = Nothing

    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objIPPCoy As New IPP
        Dim ds As New DataSet
        strCoyCode = Me.txtCompanyCode.Text
        strCoyName = Me.txtCompanyName.Text
        strBusinessRegNo = Me.txtBusinessRegNo.Text
        'blnAsset = Me.chkAsset.Checked
        'blnSub = Me.chkSub.Checked
        'blnchkActive = Me.chkActive.Checked
        'blnchkInactive = Me.chkInactive.Checked

        If chkVendor.Checked = True And chkOtherBCoy.Checked = False And chkEmployee.Checked = False Then
            dtgIPPCompany.Columns(enumComp.icCoyAbbr).Visible = False
            dtgIPPCompany.Columns(enumComp.icIBSGLCode).Visible = False
            dtgIPPCompany.Columns(enumComp.icNonIBSGLCode).Visible = False
            dtgIPPCompany.Columns(enumComp.icPayMode).Visible = True
            dtgIPPCompany.Columns(enumComp.icBankName).Visible = True
            dtgIPPCompany.Columns(enumComp.icBankAcc).Visible = True
            dtgIPPCompany.Columns(enumComp.icGstRegNo).Visible = True
            dtgIPPCompany.Columns(enumComp.icGstInputTaxCode).Visible = True
            dtgIPPCompany.Columns(enumComp.icStaffDate).Visible = False
            dtgIPPCompany.Columns(enumComp.icJobGrade).Visible = False
            dtgIPPCompany.Columns(enumComp.icBranchCode).Visible = False
            dtgIPPCompany.Columns(enumComp.icCostCentre).Visible = False
            'Zulham 29062015 - HLB - IPP GST Stage 4(CR)
            dtgIPPCompany.Columns(enumComp.icBillGLCode1).Visible = False
            dtgIPPCompany.Columns(enumComp.icBillGLCode2).Visible = True
        ElseIf chkVendor.Checked = False And chkOtherBCoy.Checked = True And chkEmployee.Checked = False Then
            dtgIPPCompany.Columns(enumComp.icCoyAbbr).Visible = True
            dtgIPPCompany.Columns(enumComp.icIBSGLCode).Visible = True
            dtgIPPCompany.Columns(enumComp.icNonIBSGLCode).Visible = True
            dtgIPPCompany.Columns(enumComp.icPayMode).Visible = False
            dtgIPPCompany.Columns(enumComp.icBankName).Visible = False
            dtgIPPCompany.Columns(enumComp.icBankAcc).Visible = False
            dtgIPPCompany.Columns(enumComp.icGstRegNo).Visible = False
            dtgIPPCompany.Columns(enumComp.icGstInputTaxCode).Visible = False
            dtgIPPCompany.Columns(enumComp.icStaffDate).Visible = False
            dtgIPPCompany.Columns(enumComp.icJobGrade).Visible = False
            dtgIPPCompany.Columns(enumComp.icBranchCode).Visible = False
            dtgIPPCompany.Columns(enumComp.icCostCentre).Visible = False
            'Zulham 29062015 - HLB - IPP GST Stage 4(CR)
            dtgIPPCompany.Columns(enumComp.icBillGLCode2).Visible = False
            dtgIPPCompany.Columns(enumComp.icBillGLCode1).Visible = False
        ElseIf chkVendor.Checked = False And chkOtherBCoy.Checked = False And chkEmployee.Checked = True Then
            dtgIPPCompany.Columns(enumComp.icCoyAbbr).Visible = False
            dtgIPPCompany.Columns(enumComp.icIBSGLCode).Visible = False
            dtgIPPCompany.Columns(enumComp.icNonIBSGLCode).Visible = False
            dtgIPPCompany.Columns(enumComp.icPayMode).Visible = True
            dtgIPPCompany.Columns(enumComp.icBankName).Visible = True
            dtgIPPCompany.Columns(enumComp.icBankAcc).Visible = True
            dtgIPPCompany.Columns(enumComp.icGstRegNo).Visible = False
            dtgIPPCompany.Columns(enumComp.icGstInputTaxCode).Visible = False
            dtgIPPCompany.Columns(enumComp.icStaffDate).Visible = True
            dtgIPPCompany.Columns(enumComp.icJobGrade).Visible = True
            dtgIPPCompany.Columns(enumComp.icBranchCode).Visible = True
            dtgIPPCompany.Columns(enumComp.icCostCentre).Visible = True
            'Zulham 29062015 - HLB - IPP GST Stage 4(CR)
            dtgIPPCompany.Columns(enumComp.icBillGLCode2).Visible = False
            dtgIPPCompany.Columns(enumComp.icBillGLCode1).Visible = False
        Else
            dtgIPPCompany.Columns(enumComp.icCoyAbbr).Visible = True
            dtgIPPCompany.Columns(enumComp.icIBSGLCode).Visible = True
            dtgIPPCompany.Columns(enumComp.icNonIBSGLCode).Visible = True
            dtgIPPCompany.Columns(enumComp.icPayMode).Visible = True
            dtgIPPCompany.Columns(enumComp.icBankName).Visible = True
            dtgIPPCompany.Columns(enumComp.icBankAcc).Visible = True
            dtgIPPCompany.Columns(enumComp.icGstRegNo).Visible = False
            dtgIPPCompany.Columns(enumComp.icGstInputTaxCode).Visible = False
            dtgIPPCompany.Columns(enumComp.icStaffDate).Visible = False
            dtgIPPCompany.Columns(enumComp.icJobGrade).Visible = False
            dtgIPPCompany.Columns(enumComp.icBranchCode).Visible = False
            dtgIPPCompany.Columns(enumComp.icCostCentre).Visible = False
            'Zulham 29062015 - HLB - IPP GST Stage 4(CR)
            If chkVendor.Checked = True Then
                dtgIPPCompany.Columns(enumComp.icBillGLCode2).Visible = False
                dtgIPPCompany.Columns(enumComp.icBillGLCode1).Visible = True
            ElseIf (chkVendor.Checked = False And chkOtherBCoy.Checked = False And chkEmployee.Checked = False) Then
                dtgIPPCompany.Columns(enumComp.icBillGLCode2).Visible = True
                dtgIPPCompany.Columns(enumComp.icBillGLCode1).Visible = False
            Else
                dtgIPPCompany.Columns(enumComp.icBillGLCode2).Visible = False
                dtgIPPCompany.Columns(enumComp.icBillGLCode1).Visible = False
            End If
            
        End If

        If strCoyCode <> "" Or strCoyName <> "" Or chkVendor.Checked = True Or chkOtherBCoy.Checked = True Or chkEmployee.Checked = True Or chkActive.Checked = True Or chkInactive.Checked = True Or strBusinessRegNo <> "" Then
            ds = objIPPCoy.SearchIPPCoyInfo(strCoyCode, strCoyName, chkVendor.Checked, chkOtherBCoy.Checked, chkEmployee.Checked, chkActive.Checked, chkInactive.Checked, strBusinessRegNo)
        Else
            ds = objIPPCoy.PopulateIPPCompany(Session("CompanyID")) 'zULHAM 15072018 - PAMB
            cmdModify.Visible = True
            cmdDelete.Visible = True
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count

        ViewState("RecordCount") = intPageRecordCnt
        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewSample.Sort += " DESC"

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If ViewState("action") = "del" Then
            If dtgIPPCompany.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgIPPCompany.PageSize = 0 Then
                dtgIPPCompany.CurrentPageIndex = dtgIPPCompany.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If

        If intPageRecordCnt > 0 Then
            cmdDelete.Visible = True
            cmdModify.Visible = True
            NoRecord.Style("display") = "none"
            IPPCompany.Style("display") = "inline"
            resetDatagridPageIndex(dtgIPPCompany, dvViewSample)
            dtgIPPCompany.DataSource = dvViewSample
            dtgIPPCompany.DataBind()
        Else
            '20110628-Jules
            cmdModify.Visible = False
            cmdDelete.Visible = False
            dtgIPPCompany.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
            If Not Page.IsPostBack Then
                NoRecord.Style("display") = "inline"
            End If
            'AssetGroup.Style("display") = "none"
            'dtgIPPCompany.ShowHeader = True
            dtgIPPCompany.DataBind()
            'Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00006"))
        End If
        ' add for above checking
        ViewState("PageCount") = dtgIPPCompany.PageCount
        objIPPCoy = Nothing

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

    Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Session("action") = ""
        Me.Response.Redirect(dDispatcher.direct("IPPAdmin", "IPPCompanyDetail.aspx", "mode=Add"))
    End Sub

    Private Sub cmdModify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdModify.Click
        Dim grdItem As DataGridItem


        '//Loop datagrid item
        Session("action") = ""
        For Each grdItem In dtgIPPCompany.Items
            Dim chkSelection As CheckBox = grdItem.Cells(enumComp.icChk).FindControl("chkSelection")
            If chkSelection.Checked Then
                Dim lnkItem As HyperLink
                lnkItem = grdItem.Cells(enumComp.icCoyName).FindControl("lnkItem")

                If grdItem.Cells(enumComp.icCoyType).Text = "Vendor" Then
                    grdItem.Cells(enumComp.icCoyType).Text = "V"
                ElseIf grdItem.Cells(enumComp.icCoyType).Text = "Billing to RC" Then
                    grdItem.Cells(enumComp.icCoyType).Text = "B"
                ElseIf grdItem.Cells(enumComp.icCoyType).Text = "Employee" Then
                    grdItem.Cells(enumComp.icCoyType).Text = "E"
                End If

                Me.Response.Redirect(dDispatcher.direct("IPPAdmin", "IPPCompanyDetail.aspx", "mode=Modify&Coy_Code=" & Server.UrlEncode(grdItem.Cells(enumComp.icCoyAbbr).Text) & "&Coy_Name=" & Server.UrlEncode(lnkItem.Text) & "&Coy_Type=" & Server.UrlEncode(grdItem.Cells(2).Text) & "&Coy_Status=" & Server.UrlEncode(grdItem.Cells(enumComp.icStatus).Text) & "&Coy_Index=" & Server.UrlEncode(grdItem.Cells(enumComp.icIndex).Text)))
            End If
        Next

    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim dgItem As DataGridItem
        Dim chkItem As CheckBox
        Dim objAdmin As New Admin
        Dim intMsgNo As Integer
        Dim strMsg As String
        Dim objIPPCoy As New IPP
        Dim dt As New DataTable
        Dim dr As DataRow

        dt.Columns.Add("IPPCoyIndex", Type.GetType("System.String"))
        dt.Columns.Add("IPPCoyCode", Type.GetType("System.String"))
        dt.Columns.Add("IPPCoyName", Type.GetType("System.String"))
        dt.Columns.Add("IPPCoyType", Type.GetType("System.String"))
        Session("action") = ""
        For Each dgItem In dtgIPPCompany.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                Dim lnkItem As HyperLink
                'lnkItem = dgItem.Cells(4).FindControl("lnkItem")
                lnkItem = dgItem.Cells(enumComp.icCoyName).FindControl("lnkItem")

                dr = dt.NewRow
                'dr("IPPCoyIndex") = dtgIPPCompany.DataKeys.Item(dgItem.ItemIndex)
                'dr("IPPCoyCode") = dgItem.Cells.Item(3).Text
                'dr("IPPCoyName") = Server.UrlEncode(lnkItem.Text) 'dgItem.Cells.Item(4).Text
                'dr("IPPCoyType") = dgItem.Cells.Item(2).Text

                dr("IPPCoyIndex") = dtgIPPCompany.DataKeys.Item(dgItem.ItemIndex)
                dr("IPPCoyCode") = dgItem.Cells.Item(enumComp.icCoyAbbr).Text
                dr("IPPCoyName") = Server.UrlEncode(lnkItem.Text)
                dr("IPPCoyType") = dgItem.Cells.Item(enumComp.icCoyType).Text
                dt.Rows.Add(dr)
            End If
        Next
        intMsgNo = objIPPCoy.DeleteIPPCompany(dt)

        If intMsgNo = -99 Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00009"), MsgBoxStyle.Information)

        ElseIf intMsgNo = WheelMsgNum.Delete Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00004"), MsgBoxStyle.Information)

        ElseIf intMsgNo = WheelMsgNum.NotDelete Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00008"), MsgBoxStyle.Information)
        End If

        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)

        objIPPCoy.GetIPPCompanyInfo(strCoyCode, strCoyName, strCoyType, strCoyStatus)
        Bindgrid()
        Session("action") = ""

        objIPPCoy = Nothing

    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dtgIPPCompany.SortCommand
        Grid_SortCommand(sender, e)
        dtgIPPCompany.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub dtgIPPCompany_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgIPPCompany.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dtgIPPCompany, e)
        intPageRecordCnt = ViewState("RecordCount")
        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Private Sub dtgIPPCompany_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgIPPCompany.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            If e.Item.Cells(enumComp.icCoyType).Text = "V" Then
                e.Item.Cells(enumComp.icCoyType).Text = "Vendor"
            ElseIf e.Item.Cells(enumComp.icCoyType).Text = "B" Then
                e.Item.Cells(enumComp.icCoyType).Text = "Billing to RC"
            ElseIf e.Item.Cells(enumComp.icCoyType).Text = "E" Then
                e.Item.Cells(enumComp.icCoyType).Text = "Employee"
            End If

            'e.Item.Cells(4).Text = "<A href=""" & dDispatcher.direct("IPPAdmin", "IPPCompanyDetail.aspx", "mode=Modify&Coy_Code=" & dv("IC_OTHER_B_COY_CODE") & "&Coy_Name=" & dv("IC_COY_NAME") & "&Coy_Type=" & dv("IC_COY_TYPE") & "&Coy_Status=" & dv("IC_STATUS")) & """ ><font color=#0000ff>" & Common.parseNull(dv("IC_COY_NAME")) & "</font></A>"
            Dim lnkItem As HyperLink
            lnkItem = e.Item.FindControl("lnkItem")
            'Chee Hong - 17/12/2014 - Only allow HLB IPP Admin for action (IPP GST Stage 2A)
            'If strDefIPPCompID <> "" And strDefIPPCompID <> Session("CompanyId") Then
            'Else
            '    lnkItem.NavigateUrl = dDispatcher.direct("IPPAdmin", "IPPCompanyDetail.aspx", "mode=Modify&Coy_Code=" & Server.UrlEncode(dv("IC_OTHER_B_COY_CODE")) & "&Coy_Name=" & Server.UrlEncode(dv("IC_COY_NAME")) & "&Coy_Type=" & Server.UrlEncode(dv("IC_COY_TYPE")) & "&Coy_Status=" & dv("IC_STATUS") & "&Coy_Index=" & dv("IC_INDEX"))
            'End If
            lnkItem.NavigateUrl = dDispatcher.direct("IPPAdmin", "IPPCompanyDetail.aspx", "mode=Modify&Coy_Code=" & Server.UrlEncode(dv("IC_OTHER_B_COY_CODE")) & "&Coy_Name=" & Server.UrlEncode(dv("IC_COY_NAME")) & "&Coy_Type=" & Server.UrlEncode(dv("IC_COY_TYPE")) & "&Coy_Status=" & dv("IC_STATUS") & "&Coy_Index=" & dv("IC_INDEX"))
            lnkItem.Text = dv("IC_COY_NAME")

            If Common.parseNull(dv("IC_COY_TYPE")) = "E" And Not IsDBNull(dv("IC_ADDITIONAL_2")) Then
                e.Item.Cells(enumComp.icStaffDate).Text = Format(dv("IC_ADDITIONAL_2"), "dd/MM/yyyy")
            End If

            If e.Item.Cells(enumComp.icStatus).Text = "A" Then
                e.Item.Cells(enumComp.icStatus).Text = "Active"
            ElseIf e.Item.Cells(enumComp.icStatus).Text = "I" Then
                e.Item.Cells(enumComp.icStatus).Text = "Inactive"
            End If


            'Zulham 23112018
            Select Case Common.parseNull(dv("IC_PAYMENT_METHOD"), "")
                Case "IBG"
                    e.Item.Cells(enumComp.icPayMode).Text = "LOCAL BANK TRANSFER-(RM)"
                Case "TT"
                    e.Item.Cells(enumComp.icPayMode).Text = "TELEGRAPHIC TRANSFER-(FOREIGN CURRENCY)"
                Case "BC"
                    e.Item.Cells(enumComp.icPayMode).Text = "CHEQUE-(RM)"
                Case "BD"
                    e.Item.Cells(enumComp.icPayMode).Text = "BANK DRAFT-(FOREIGN CURRENCY)"
                Case "CO"
                    e.Item.Cells(enumComp.icPayMode).Text = "CASHIER'S ORDER-(RM)"
            End Select

            '//to dynamic build hyoerlink
            'Dim lnkDeptCode As HyperLink
            'lnkDeptCode = e.Item.FindControl("lnkDeptCode")
            'Dim aa As String
            'lnkDeptCode.NavigateUrl = "" & dDispatcher.direct("Admin", "addDept.aspx", "mode=update&pageid=" & strPageId & "&deptcode=" & dv("CDM_DEPT_CODE") & "&deptname=" & dv("CDM_DEPT_NAME") & "&appgrpindex=" & objDb.ReturnArrayValue(dv("CDM_APPROVAL_GRP_INDEX"), UBound(dv("CDM_APPROVAL_GRP_INDEX"))))
            'lnkDeptCode.Text = dv("CDM_DEPT_CODE")

        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgIPPCompany.CurrentPageIndex = 0
        ViewState("SortAscending") = "no"
        ViewState("SortExpression") = "IC_INDEX"
        Bindgrid()
    End Sub

    Private Sub dtgIPPCompany_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgIPPCompany.PageIndexChanged
        Dim objIPPCoy As New IPP

        dtgIPPCompany.CurrentPageIndex = e.NewPageIndex
        objIPPCoy.GetIPPCompanyInfo(strCoyCode, strCoyName, strCoyType, strCoyStatus)
        Bindgrid()
        Session("action") = ""

        objIPPCoy = Nothing

    End Sub
End Class