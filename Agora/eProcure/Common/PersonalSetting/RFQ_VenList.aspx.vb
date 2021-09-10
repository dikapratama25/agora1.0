Imports AgoraLegacy
Imports eProcure.Component

Public Class RFQ_VenList
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Protected WithEvents dtgVendList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents btnHidden As System.Web.UI.WebControls.Button

    Public Enum VenEnum
        Chk = 0
        RFQ_No1 = 1
        VenList = 2
    End Enum
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents MyDataGrid As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Protected WithEvents cmdModify As System.Web.UI.WebControls.Button
    Protected WithEvents hide As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents txtAddVendor As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdClear As System.Web.UI.WebControls.Button
    Protected WithEvents cmdCancel As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents cmdVendor As System.Web.UI.WebControls.Button
    Protected WithEvents txtSearch As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents lblAddVendor As System.Web.UI.WebControls.Label
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidMode As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents vldVenList As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents cmdSearch_Clear As System.Web.UI.WebControls.Button
    Protected WithEvents ValidationSummary1 As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        cmdAdd.Enabled = False
        cmdDelete.Enabled = False
        cmdModify.Enabled = False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmdAdd)
        htPageAccess.Add("add", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdModify)
        htPageAccess.Add("update", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdDelete)
        htPageAccess.Add("delete", alButtonList)
        CheckButtonAccess()
        '//additional checking
        If intPageRecordCnt > 0 Then
            cmdDelete.Enabled = blnCanDelete
            cmdModify.Enabled = blnCanUpdate
            '//mean Enable, can't use button.Enabled because this is a HTML button
        Else
            cmdDelete.Enabled = False
            cmdModify.Enabled = False
        End If
        alButtonList.Clear()
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'Session("CompanyId") = "demo"
        'Session("UserId") = "moofh"
        MyBase.Page_Load(sender, e)

        SetGridProperty(dtgVendList)

        If Not Page.IsPostBack Then
            cmdDelete.Enabled = False
            cmdModify.Enabled = False
        End If
        cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
        cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
    End Sub

    Public Sub dtgVendList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtgVendList.CurrentPageIndex = e.NewPageIndex
        Bindgrid(0, True)
    End Sub


    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgVendList.CurrentPageIndex = 0
        Bindgrid(0)
    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim objRFQ As New PersonalSetting
        Dim ds As DataSet = New DataSet
        Dim record As String
        Dim strlistname As String

        strlistname = Me.txtSearch.Text

        ds = objRFQ.getVenList(strlistname)

        ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView

        dvViewSample.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        If viewstate("action") = "del" Then
            If dtgVendList.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgVendList.PageSize = 0 Then
                dtgVendList.CurrentPageIndex = dtgVendList.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If

        If viewstate("intPageRecordCnt") > 0 Then
            resetDatagridPageIndex(dtgVendList, dvViewSample)
            cmdDelete.Enabled = True
            cmdModify.Enabled = True
            '//mean Enable, can't use button.Enabled because this is a HTML button
            cmdReset.Disabled = False
            dtgVendList.DataSource = dvViewSample
            dtgVendList.DataBind()
        Else

            cmdDelete.Enabled = False
            cmdModify.Enabled = False
            cmdReset.Disabled = True
            dtgVendList.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)

        End If
        ' add for above checking
        ViewState("PageCount") = dtgVendList.PageCount
        objRFQ = Nothing
    End Function
    Private Sub cmdClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        txtSearch.Text = ""

    End Sub
    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        Bindgrid(dtgVendList.CurrentPageIndex = 0, True)
    End Sub
    Private Sub dtgVendList_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgVendList.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgVendList, e)
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Private Sub dtgVendList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgVendList.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim chk As CheckBox
            chk = e.Item.FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            '<fali>
            Dim STR As String
            Dim com_str As String
            Dim objRFQ As New PersonalSetting
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim com_name(100) As String
            Dim com_id(100) As String
            Dim COUNT2 As Integer
            Dim ARRAY(100) As String
            Dim array2(100) As String
            Dim COUNT As Integer
            Dim i As Integer
            Dim vendid(100) As String
            'Dim lnkCode As HyperLink
            
            objRFQ.get_vendorlist(Common.parseNull(dv("RVDLM_LIST_INDEX")), com_name, com_id, COUNT2, com_str, vendid)

            If com_name(0) <> "" Then
                For i = 0 To COUNT2 - 1
                    'STR = STR & "<A href=""" & dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "Frm=RFQ_VenList&pageid=" & strPageId & "&v_com_id=" & vendid(i)) & """ ><font color=#0000ff>" & com_name(i) & "</font></A><br>"
                    'e.Item.Cells(0).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "frm=Comlist&side=vendorsum&pageid=" & strPageId & " &RFQ_Num=" & ViewState("rfq_num") & "&RFQ_ID=" & Request(Trim("RFQ_ID")) & "&RFQ_Name=" & Request(Trim("RFQ_Name")) & "&v_com_id=" & Common.parseNull(dv("CM_COY_ID"))) & "')"" ><font color=#0000ff>" & Common.parseNull(dv("CM_COY_NAME")) & "</font></A>"
                    'STR = STR & "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "Frm=RFQ_VenList&pageid=" & strPageId & "&v_com_id=" & vendid(i)) & """ ><font color=#0000ff>" & com_name(i) & "</font></A><br/>"
                    STR = STR & "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "Frm=RFQ_VenList&pageid=" & strPageId & "&v_com_id=" & vendid(i)) & "')"" ><font color=#0000ff>" & com_name(i) & "</font></A><br/>"
                Next
            End If

            If STR = "" Then
                STR = "No vendors"
            End If
            e.Item.Cells(VenEnum.VenList).Text = STR
            '</fali>


        End If
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Dim strFileName As String
        Dim strscript As New System.Text.StringBuilder
        Session("userAction") = "Add"

        strscript.Append("<script language=""javascript"">")
        strFileName = dDispatcher.direct("PersonalSetting", "RFQAddVendList.aspx", "pageid=" & strPageId)
        strFileName = Server.UrlEncode(strFileName)
        strscript.Append("ShowDialog('" & dDispatcher.direct("PersonalSetting", "Dialog.aspx", "page=" & strFileName) & "','300px');")
        strscript.Append("document.getElementById('btnHidden').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script3", strscript.ToString())
    End Sub


    'Sub view(ByVal selected As String)
    '    Dim objPersonal As New PersonalSetting
    '    Dim strmsg As String
    '    Dim intmsgno As Integer
    '    Dim strsqladdfavlist As String

    '    If selected = "add" Then
    '        lblAddVendor.Text = "Please add the following value"
    '        'intmsgno = objPersonal.addVenList(Common.Parse(txtAddVendor.Text))
    '        intmsgno = objPersonal.addVenList(txtAddVendor.Text)

    '        Select Case intmsgno
    '            Case WheelMsgNum.Save
    '                txtAddVendor.Text = ""
    '                strmsg = MsgRecordSave
    '                ' lblAddVendor.Text = ""

    '            Case WheelMsgNum.Duplicate
    '                strmsg = MsgRecordDuplicate
    '            Case WheelMsgNum.NotSave
    '                strmsg = MsgRecordNotSave
    '        End Select
    '        Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)


    '    ElseIf selected = "mod" Then
    '        lblAddVendor.Text = "Please modify the following value"
    '        'intmsgno = objPersonal.modVendorList(Common.Parse(hidIndex.Value), Common.Parse(txtAddVendor.Text), viewstate("oldvalue"))
    '        intmsgno = objPersonal.modVendorList(hidIndex.Value, (txtAddVendor.Text), viewstate("oldvalue"))
    '        'Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information, "Wheel")
    '        txtAddVendor.Text = ViewState("oldvalue")

    '        'non-display the modify listing header 
    '        hide.Style("display") = "none"

    '        Select Case intmsgno
    '            Case WheelMsgNum.Save
    '                strmsg = MsgRecordSave
    '                txtAddVendor.Text = ""
    '                hide.Style("display") = "none"
    '                ViewState("oldvalue") = ""
    '            Case WheelMsgNum.Duplicate
    '                strmsg = MsgRecordDuplicate
    '                hide.Style("display") = ""
    '            Case WheelMsgNum.NotSave
    '                strmsg = MsgRecordNotSave
    '                hide.Style("display") = ""
    '        End Select
    '        'custom.Style("display") = "none"
    '    End If

    '    Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)
    '    Bindgrid()
    '    objPersonal = Nothing

    'End Sub



    Private Sub cmdModify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdModify.Click
        'Dim dgItem As DataGridItem
        'Dim chk As CheckBox
        'Dim strlistindex As String
        'Dim i As Integer
        'i = 0

        'hide.Style("display") = ""
        'cmdClear.Text = "Reset"
        'hidMode.Value = "m"
        'For Each dgItem In MyDataGrid.Items
        '    chk = dgItem.FindControl("chkSelection")
        '    If chk.Checked Then
        '        hidIndex.Value = MyDataGrid.DataKeys.Item(i)
        '        txtAddVendor.Text = dgItem.Cells(1).Text
        '        ViewState("oldvalue") = dgItem.Cells(1).Text
        '        Exit For
        '    End If
        '    i = i + 1
        'Next
        'lblAddVendor.Text = "Please modify the following value"
        Dim listname As String
        Dim chk As CheckBox
        Dim dgItem As DataGridItem
        For Each dgItem In dtgVendList.Items
            chk = dgItem.FindControl("chkSelection")
            If chk.Checked Then
                listname = dgItem.Cells(1).Text
                Exit For
            End If
        Next

        Dim strFileName As String
        Dim strscript As New System.Text.StringBuilder
        Session("userAction") = "Modify"

        '<fali> added to avoid & and # symbol filtering during passing the listname value
        strscript.Append("<script language=""javascript"">")
        If listname.IndexOf(" ") Then
            listname = listname.Replace(" ", "SPSPCE")
        End If
        '</fali>

        strFileName = dDispatcher.direct("PersonalSetting", "RFQAddVendList.aspx", "pageid=" & strPageId & "&name=" & listname)
        strFileName = Server.UrlEncode(strFileName)

        strscript.Append("ShowDialog('" & dDispatcher.direct("PersonalSetting", "Dialog.aspx", "page=" & strFileName) & "','300px');")
        strscript.Append("document.getElementById('btnHidden').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script3", strscript.ToString())
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim objps As New PersonalSetting
        Dim dgItem As DataGridItem
        Dim chk As CheckBox

        For Each dgItem In dtgVendList.Items
            chk = dgItem.FindControl("chkSelection")
            If chk.Checked Then
                objps.delVenList(dgItem.Cells(1).Text, dgItem.Cells(2).Text)
            End If

        Next
        Bindgrid(0)
        Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
        'Dim objCo As New PersonalSetting
        'Dim dgItem As DataGridItem
        'Dim chk As CheckBox
        'Dim strlistindex As String
        'Dim i As Integer = 0

        ''//To prevent "No Record Found" msg
        'txtSearch.Text = ""

        'For Each dgItem In MyDataGrid.Items

        '    chk = dgItem.FindControl("chkSelection")
        '    strlistindex = MyDataGrid.DataKeys.Item(i)
        '    If chk.Checked Then
        '        objCo.delVendorList(strlistindex)
        '    End If
        '    i = i + 1
        'Next

        'Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
        'viewstate("action") = "del"
        'Bindgrid(0)
        'objCo = Nothing
    End Sub
    Public Sub btnHidden_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHidden.Click
        Bindgrid(True)
    End Sub
End Class
