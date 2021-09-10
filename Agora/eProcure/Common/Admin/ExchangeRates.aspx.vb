Imports AgoraLegacy
Imports eProcure.Component


Public Class ExchangeRates
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDb As New EAD.DBCom
    Dim objAdm As New Admin
    Dim objGlobal As New AppGlobals
    Protected WithEvents cmd_search As System.Web.UI.WebControls.Button
    Protected WithEvents cmdModify As System.Web.UI.WebControls.Button
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents cmdBatch As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_cancel As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_clear As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_save As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_clear1 As System.Web.UI.WebControls.Button
    Protected WithEvents btnHidden As System.Web.UI.WebControls.Button
    Protected WithEvents cboCurr1 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboCurr2 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddl_Select As System.Web.UI.WebControls.DropDownList
    Protected WithEvents dtgExRate As System.Web.UI.WebControls.DataGrid
    Protected WithEvents txt_add_ExRate As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_DateFrom As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_DateTo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_SDateFrom As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_SDateTo As System.Web.UI.WebControls.TextBox

    Protected WithEvents Hide_Add2 As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents hidMode As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lbl_add_mod As System.Web.UI.WebControls.Label
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents revCurrCode As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents revDateFrom As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents revDateTo As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents validate_ex_rate As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rev_ex_rate As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents cvDateNow As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents cvDate As System.Web.UI.WebControls.CompareValidator


    Dim ds As DataSet

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub


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
        cmdModify.Enabled = False
        cmdDelete.Enabled = False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmdAdd)
        htPageAccess.Add("add", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdModify)
        htPageAccess.Add("update", alButtonList)
        CheckButtonAccess()
        '//additional checking
        If intPageRecordCnt > 0 Then
            cmdModify.Enabled = blnCanUpdate
            cmdDelete.Enabled = blnCanUpdate
            '//mean Enable, can't use button.Enabled because this is a HTML button
            'cmdReset.Disabled = False
        Else
            cmdModify.Enabled = False
            cmdDelete.Enabled = False
            'cmdReset.Disabled = True
        End If
        alButtonList.Clear()
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgExRate)
        If Not IsPostBack Then
            cvDateNow.ValueToCompare = Date.Today.ToShortDateString
            cmd_search.Enabled = True
            cmd_clear1.Enabled = True
            cmdModify.Enabled = False
            cmdDelete.Enabled = False
            Hide_Add2.Style("display") = "none"

            objGlobal.FillCodeTable(cboCurr1, CodeTable.Currency)
            objGlobal.FillCodeTable(cboCurr2, CodeTable.Currency)
        End If

        cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
        cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")

    End Sub

    Sub dtgExRate_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgExRate.PageIndexChanged
        dtgExRate.CurrentPageIndex = e.NewPageIndex
        Bindgrid(True)
    End Sub

    Sub SortCommand_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles dtgExRate.SortCommand
        Grid_SortCommand(sender, e)
        dtgExRate.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String

        Dim objSI As New Admin
        Dim ds As DataSet
        Dim objAdmin As New Admin

        ds = objAdmin.RetExRate(cboCurr1.SelectedItem.Value, txt_SDateFrom.Text, txt_SDateTo.Text)

        '//for sorting asc or desc
        Dim dvViewPack As DataView
        dvViewPack = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewPack.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewPack.Sort += " DESC"
        End If


        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            cmdModify.Enabled = True
            cmdDelete.Enabled = True
            resetDatagridPageIndex(dtgExRate, dvViewPack)
            dtgExRate.DataSource = dvViewPack
            dtgExRate.DataBind()
            dtgExRate.Visible = True
        Else
            cmdModify.Enabled = False
            cmdDelete.Enabled = False
            resetDatagridPageIndex(dtgExRate, dvViewPack)
            dtgExRate.DataSource = dvViewPack
            dtgExRate.DataBind()
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00006"))
            dtgExRate.Visible = False
        End If

        ViewState("PageCount") = dtgExRate.PageCount
    End Function

    Private Sub dtgExRate_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgExRate.ItemCreated

        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgExRate, e)
        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If

    End Sub

    Private Sub dtgExRate_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgExRate.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            If IsDBNull(dv("CE_RATE")) Then
                e.Item.Cells(5).Text = ""
            Else
                e.Item.Cells(5).Text = Format(dv("CE_RATE"), "###0.000000")
            End If

            If IsDBNull(dv("CE_VALID_FROM")) Then
                e.Item.Cells(6).Text = ""
            Else
                e.Item.Cells(6).Text = Format(CDate(dv("CE_VALID_FROM")), "dd/MM/yyyy")
            End If

            If IsDBNull(dv("CE_VALID_TO")) Then
                e.Item.Cells(7).Text = ""
            Else
                e.Item.Cells(7).Text = Format(CDate(dv("CE_VALID_TO")), "dd/MM/yyyy")
            End If

        End If
    End Sub

    Private Sub cmd_search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_search.Click
        dtgExRate.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Dim dgItem As DataGridItem
        Dim chk As CheckBox

        Hide_Add2.Style("display") = "inline"
        revCurrCode.Enabled = True
        revDateFrom.Enabled = True
        revDateTo.Enabled = True
        validate_ex_rate.Enabled = True
        rev_ex_rate.Enabled = True
        cvDateNow.Enabled = True
        cvDate.Enabled = True
        cboCurr2.Enabled = True
        cboCurr2.SelectedIndex = 0
        txt_add_ExRate.Text = ""
        txt_DateFrom.Text = ""
        txt_DateTo.Text = ""
        lblMsg.Text = ""
        Me.lbl_add_mod.Text = "add"
        cmd_clear.Text = "Clear"
        hidMode.Value = "a"

        For Each dgItem In dtgExRate.Items
            chk = dgItem.FindControl("chkSelection")
            chk.Checked = False
        Next

    End Sub

    Private Sub cmd_clear1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_clear1.Click

        cboCurr1.SelectedIndex = 0
        txt_SDateFrom.Text = ""
        txt_SDateTo.Text = ""
    End Sub

    Private Sub cmd_clear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_clear.Click
        If hidMode.Value = "a" Then
            cboCurr2.SelectedIndex = 0
            txt_add_ExRate.Text = ""
            txt_DateFrom.Text = ""
            txt_DateTo.Text = ""
        Else
            cboCurr2.SelectedItem.Value = ViewState("oldCurrCode")
            txt_add_ExRate.Text = ViewState("oldExRate")
            txt_DateFrom.Text = ViewState("oldDateFrom")
            txt_DateTo.Text = ViewState("oldDateTo")
        End If

        lblMsg.Text = ""
    End Sub

    Private Sub cmd_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_save.Click

        If hidMode.Value = "a" Then
            Me.view("add")
        ElseIf hidMode.Value = "m" Then
            Me.view("mod")
        End If

    End Sub

    Sub view(ByVal selected As String)
        Dim intmsgno As Integer
        Dim strMsg As String = ""

        If validateDatagrid(strMsg, selected) Then
            lblMsg.Text = ""
            If selected = "add" Then
                Me.lbl_add_mod.Text = "add"
                intmsgno = objAdm.AddExRate(cboCurr2.SelectedItem.Value, txt_add_ExRate.Text, txt_DateFrom.Text, txt_DateTo.Text)
            ElseIf selected = "mod" Then
                Me.lbl_add_mod.Text = "modify"
                intmsgno = objAdm.UpdateExRate(hidIndex.Value, cboCurr2.SelectedItem.Value, txt_add_ExRate.Text, txt_DateFrom.Text, txt_DateTo.Text)
            End If

            Select Case intmsgno
                Case WheelMsgNum.Save
                    Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00003"), MsgBoxStyle.Information)
                    cboCurr1.SelectedIndex = 0
                    txt_SDateFrom.Text = ""
                    txt_SDateTo.Text = ""
                    Bindgrid(0)
                    If selected = "add" Then
                        cboCurr2.SelectedIndex = 0
                        txt_add_ExRate.Text = ""
                        hidMode.Value = "a"
                    Else
                        Hide_Add2.Style("display") = "none"
                        hidMode.Value = ""
                    End If

                Case WheelMsgNum.NotSave
                    Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00007"), MsgBoxStyle.Information)
                    Bindgrid(0)
                    'Hide_Add2.Style("display") = "none"
                    hidMode.Value = ""
            End Select
        Else
            If strMsg <> "" Then
                lblMsg.Text = strMsg
            Else
                lblMsg.Text = ""
            End If
        End If
    End Sub

    Private Sub cmdModify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdModify.Click
        Dim dgItem As DataGridItem
        Dim chk As CheckBox
        Dim i As Integer
        i = 0

        Hide_Add2.Style("display") = "inline"
        revCurrCode.Enabled = True
        rev_ex_rate.Enabled = True
        validate_ex_rate.Enabled = True
        revDateFrom.Enabled = True
        revDateTo.Enabled = True
        cvDateNow.Enabled = True
        cvDate.Enabled = True
        cboCurr2.Enabled = False
        Me.lbl_add_mod.Text = "modify"
        cmd_clear.Text = "Reset"
        lblMsg.Text = ""
        hidMode.Value = "m"
        For Each dgItem In dtgExRate.Items
            chk = dgItem.FindControl("chkSelection")
            If chk.Checked Then
                cboCurr2.SelectedValue = dgItem.Cells(1).Text
                txt_add_ExRate.Text = dgItem.Cells(5).Text
                txt_DateFrom.Text = dgItem.Cells(6).Text
                txt_DateTo.Text = dgItem.Cells(7).Text
                hidIndex.Value = dgItem.Cells(2).Text
                ViewState("oldCurrCode") = dgItem.Cells(1).Text
                ViewState("oldExRate") = dgItem.Cells(5).Text
                ViewState("oldDateFrom") = dgItem.Cells(6).Text
                ViewState("oldDateTo") = dgItem.Cells(7).Text
                Exit For
            End If
            i = i + 1
        Next

    End Sub

    Private Sub cmd_cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_cancel.Click
        Dim dgItem As DataGridItem
        Dim chk As CheckBox

        Hide_Add2.Style("display") = "none"
        hidMode.Value = ""

        For Each dgItem In dtgExRate.Items
            chk = dgItem.FindControl("chkSelection")
            chk.Checked = False
        Next
    End Sub

    Private Function validateDatagrid(ByRef strMsg As String, ByVal strMode As String) As Boolean
        Dim strsql As String = ""
        Dim dsDate As New DataSet
        Dim strDtFrom, strDtTo, DtFrom, DtTo As Date
        Dim i As Integer
        validateDatagrid = True
        strMsg = "<ul type='disc'>"
        strDtFrom = txt_DateFrom.Text
        strDtTo = txt_DateTo.Text

        strsql = "SELECT DATE_FORMAT(CE_VALID_FROM, '%d/%m/%Y') AS CE_VALID_FROM, DATE_FORMAT(CE_VALID_TO, '%d/%m/%Y') AS CE_VALID_TO " & _
                "FROM COMPANY_EXCHANGERATE WHERE CE_CURRENCY_CODE = '" & Common.Parse(cboCurr2.SelectedItem.Value) & "' AND CE_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CE_DELETED ='N' "

        If strMode = "mod" Then
            strsql &= "AND CE_CURRENCY_INDEX <> " & hidIndex.Value & " "
        Else
            strsql &= "AND CE_VALID_FROM IS NOT NULL AND CE_VALID_TO IS NOT NULL "
        End If

        dsDate = objDb.FillDs(strsql)

        If objDb.Exist(strsql) Then
            For i = 0 To dsDate.Tables(0).Rows.Count - 1
                DtFrom = dsDate.Tables(0).Rows(i).Item("CE_VALID_FROM").ToString()
                DtTo = dsDate.Tables(0).Rows(i).Item("CE_VALID_TO").ToString()

                If (strDtFrom <= DtFrom And DtFrom <= strDtTo) Or (strDtFrom <= DtTo And DtTo <= strDtTo) Or (strDtFrom <= DtFrom And DtTo <= strDtTo) Or (DtFrom <= strDtFrom And strDtTo <= DtTo) Then
                    strMsg &= "<li>Valid Date From/To cannot be cross or within by other valid date from/to.<ul type='disc'></ul></li>"
                    validateDatagrid = False
                End If
                'If strDtFrom <= DtFrom And DtFrom <= strDtTo Then
                '    strMsg &= "<li>Valid Date From is already set between Date From and Date To.<ul type='disc'></ul></li>"
                '    validateDatagrid = False
                'End If

                'If strDtFrom <= DtTo And DtTo <= strDtTo Then
                '    strMsg &= "<li>Valid Date To is already set between Date From and Date To.<ul type='disc'></ul></li>"
                '    validateDatagrid = False
                'End If

                'If strDtFrom <= DtFrom And DtTo <= strDtTo Then
                '    strMsg &= "<li>Valid Date From and Valid Date To already inside between new Valid Date From and Valid Date To.<ul type='disc'></ul></li>"
                '    validateDatagrid = False
                'End If

                'If DtFrom <= strDtFrom And strDtTo <= DtTo Then
                '    strMsg &= "<li>Valid Date From and Valid Date To cannot be inside between new Valid Date From and Valid Date To.<ul type='disc'></ul></li>"
                '    validateDatagrid = False
                'End If

                If validateDatagrid = False Then
                    Exit For
                End If
            Next

        Else
            validateDatagrid = True
        End If
        


    End Function

    Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click

        Dim dgItem As DataGridItem
        Dim intMsg As Integer
        Dim dtNewRate As New DataTable
        Dim drRate As DataRow
        dtNewRate.Columns.Add("Curr_Code", Type.GetType("System.String")) '//product code
        dtNewRate.Columns.Add("Curr_Index", Type.GetType("System.Int32")) '//supplier id

        For Each dgItem In dtgExRate.Items
            Dim chkSelection As CheckBox = dgItem.Cells(0).FindControl("chkSelection")

            If chkSelection.Checked Then
                Dim deptcode As String = dgItem.Cells(1).Text
                drRate = dtNewRate.NewRow
                drRate("Curr_Code") = dgItem.Cells(1).Text
                drRate("Curr_Index") = dgItem.Cells(2).Text
                dtNewRate.Rows.Add(drRate)
            End If
        Next

        Dim objAdmin As New Admin
        intMsg = objAdmin.delExRate(dtNewRate)
        lblMsg.Text = ""
        If intMsg = WheelMsgNum.Delete Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00004"), MsgBoxStyle.Information)
            cboCurr1.SelectedIndex = 0
            txt_SDateFrom.Text = ""
            txt_SDateTo.Text = ""
            dtgExRate.CurrentPageIndex = 0
            Hide_Add2.Style("display") = "none"
            hidMode.Value = ""
            Bindgrid()
        ElseIf intMsg = WheelMsgNum.NotDelete Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00008"), MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub cmdBatch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdBatch.Click
        Dim strscript As New System.Text.StringBuilder
        Dim strFileName As String
        Dim strName As String

        strscript.Append("<script language=""javascript"">")
        strFileName = dDispatcher.direct("Admin", "BatchExchangeRates.aspx", "&pageid=" & strPageId)
        strFileName = Server.UrlEncode(strFileName)
        strscript.Append("ShowDialog('" & dDispatcher.direct("Initial", "Dialog.aspx", "page=" & strFileName) & "','580px');")
        strscript.Append("document.getElementById('btnHidden').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script3", strscript.ToString())
    End Sub

    Private Sub btnHidden_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHidden.Click
        dtgExRate.CurrentPageIndex = 0
        Bindgrid()
    End Sub
End Class
