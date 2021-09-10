Imports AgoraLegacy
Imports eProcure.Component

Public Class BankCodeMaint
    Inherits AgoraLegacy.AppBaseClass
    Dim objGlobal As New AppGlobals
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim strBankCode As String
    Dim strBankName As String
    Dim strBankStatus As String
    Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID") 'Modified for IPP GST Stage 2A

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
        Dim objBC As New IPP
        strNewCSS = "true"
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgBankCode)

        If Not Page.IsPostBack Then
            cmdDelete.Visible = False
            cmdModify.Visible = False
            Session("action") = ""
            objBC.GetBankInfo(strBankCode, strBankName, strBankStatus)

            'Chee Hong - 17/12/2014 - Only allow HLB IPP Admin for action (IPP GST Stage 2A)
            If strDefIPPCompID <> "" And strDefIPPCompID <> Session("CompanyId") Then
                cmdAdd.Visible = False
            Else
                cmdAdd.Visible = True
            End If
        End If

        If Session("action") = "Modify" Or Session("action") = "Add" Or Session("action") = "Update" Then
            objBC.GetBankInfo(strBankCode, strBankName, strBankStatus)
            Bindgrid()
            Session("action") = ""
        End If

        cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOneDtg('chkSelection','delete');")
        cmdModify.Attributes.Add("onclick", "return CheckAtLeastOneDtg('chkSelection','modify');")
        objBC = Nothing

    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objBC As New IPP
        Dim ds As New DataSet
        strBankCode = Me.txtBankCode.Text
        strBankName = Me.txtName.Text

        If strBankCode <> "" Or strBankName <> "" Or chkActive.Checked = True Or chkInactive.Checked = True Then
            ds = objBC.SearchBankCodeInfo(strBankCode, strBankName, chkActive.Checked, chkInactive.Checked)
        Else
            ds = objBC.PopulateBankCode()
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
            If dtgBankCode.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgBankCode.PageSize = 0 Then
                dtgBankCode.CurrentPageIndex = dtgBankCode.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If

        If intPageRecordCnt > 0 Then
            cmdModify.Visible = True
            cmdDelete.Visible = True
            NoRecord.Style("display") = "none"
            BankCode.Style("display") = "inline"
            resetDatagridPageIndex(dtgBankCode, dvViewSample)
            dtgBankCode.DataSource = dvViewSample
            dtgBankCode.DataBind()
        Else
            '20110628-Jules
            cmdModify.Visible = False
            cmdDelete.Visible = False
            dtgBankCode.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
            If Not Page.IsPostBack Then
                NoRecord.Style("display") = "inline"
            End If
            dtgBankCode.DataBind()
        End If
        ' add for above checking
        ViewState("PageCount") = dtgBankCode.PageCount
        objBC = Nothing

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

        Dim strFileName As String
        Dim strscript As New System.Text.StringBuilder
        strscript.Append("<script language=""javascript"">")
        strFileName = dDispatcher.direct("IPPAdmin", "BankCodeAdd.aspx", "mode=Add")
        strFileName = Replace(Server.UrlEncode(strFileName), "'", "%27")
        strscript.Append("ShowDialog('" & dDispatcher.direct("Initial", "Dialog.aspx", "page=" & strFileName) & "','300px');")
        strscript.Append("document.getElementById('btnHidden1').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script13", strscript.ToString())

    End Sub

    Private Sub cmdModify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdModify.Click
        Dim grdItem As DataGridItem
        '//Loop datagrid item
        Session("action") = ""
        For Each grdItem In dtgBankCode.Items
            Dim chkSelection As CheckBox = grdItem.Cells(0).FindControl("chkSelection")
            If chkSelection.Checked Then
                Dim strFileName As String
                Dim strscript As New System.Text.StringBuilder
                strscript.Append("<script language=""javascript"">")
                strFileName = dDispatcher.direct("IPPAdmin", "BankCodeAdd.aspx", "mode=Modify&BC_Code=" & Server.UrlEncode(grdItem.Cells(2).Text) & "&BC_Name=" & Server.UrlEncode(grdItem.Cells(3).Text) & "&BC_Usage=" & Server.UrlEncode(grdItem.Cells(4).Text) & "&BC_Status=" & Server.UrlEncode(grdItem.Cells(5).Text))
                strFileName = Replace(Server.UrlEncode(strFileName), "'", "%27")
                strscript.Append("ShowDialog('" & dDispatcher.direct("Initial", "Dialog.aspx", "page=" & strFileName) & "','300px');")
                strscript.Append("document.getElementById('btnHidden1').click();")
                strscript.Append("</script>")
                RegisterStartupScript("script13", strscript.ToString())
            End If
        Next
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim dgItem As DataGridItem
        Dim strCode As String
        Dim chkItem As CheckBox
        Dim objAdmin As New Admin
        Dim intMsgNo As Integer
        Dim strMsg As String
        Dim strNotDeleted As String
        Dim objBC As New IPP
        Dim dt As New DataTable
        Dim dr As DataRow

        dt.Columns.Add("BCCode", Type.GetType("System.String"))
        Session("action") = ""
        For Each dgItem In dtgBankCode.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                dr = dt.NewRow
                dr("BCCode") = dgItem.Cells.Item(2).Text
                dt.Rows.Add(dr)
            End If
        Next
        intMsgNo = objBC.DeleteBankCode(dt)

        If intMsgNo = -99 Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00018"), MsgBoxStyle.Information)
        ElseIf intMsgNo = WheelMsgNum.Delete Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00004"), MsgBoxStyle.Information)
        ElseIf intMsgNo = WheelMsgNum.NotDelete Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00008"), MsgBoxStyle.Information)
        End If

        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)

        objBC.GetBankInfo(strBankCode, strBankName, strBankStatus)
        Bindgrid()
        Session("action") = ""

        objBC = Nothing

    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dtgBankCode.SortCommand
        Grid_SortCommand(sender, e)
        dtgBankCode.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub dtgBankCode_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgBankCode.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dtgBankCode, e)
        intPageRecordCnt = ViewState("RecordCount")
        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Private Sub dtgBankCode_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgBankCode.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            If e.Item.Cells(5).Text = "A" Then
                e.Item.Cells(5).Text = "Active"
            ElseIf e.Item.Cells(5).Text = "I" Then
                e.Item.Cells(5).Text = "Inactive"
            End If

            If e.Item.Cells(4).Text = "RT" Then
                e.Item.Cells(4).Text = "RENTAS"
            End If

            'Zulham 15112018
            Select Case e.Item.Cells(4).Text.Trim
                Case "IBG"
                    e.Item.Cells(4).Text = "LOCAL BANK TRANSFER-(RM)"
                Case "TT"
                    e.Item.Cells(4).Text = "TELEGRAPHIC TRANSFER-(FOREIGN CURRENCY)"
                Case "BC"
                    e.Item.Cells(4).Text = "CHEQUE-(RM)"
                Case "BD"
                    e.Item.Cells(4).Text = "BANK DRAFT-(FOREIGN CURRENCY)"
                Case "CO"
                    e.Item.Cells(4).Text = "CASHIER'S ORDER-(RM)"
            End Select

        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgBankCode.CurrentPageIndex = 0
        ViewState("SortAscending") = "yes"
        ViewState("SortExpression") = "BC_BANK_CODE"
        Bindgrid()
    End Sub

    Private Sub dtgBankCode_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgBankCode.PageIndexChanged
        Dim objBC As New IPP

        dtgBankCode.CurrentPageIndex = e.NewPageIndex
        objBC.GetBankInfo(strBankCode, strBankName, strBankStatus)
        Bindgrid()
        Session("action") = ""
        objBC = Nothing

    End Sub
End Class