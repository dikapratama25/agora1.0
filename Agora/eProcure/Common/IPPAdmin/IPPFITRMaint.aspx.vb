Imports AgoraLegacy
Imports eProcure.Component


Public Class IPPFITRMaint
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDb As New EAD.DBCom
    'Dim objAdm As New Admin
    Dim objIPPMain As New IPPMain
    Dim objGlobal As New AppGlobals

    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents cmdModify As System.Web.UI.WebControls.Button
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents cmdCancel As System.Web.UI.WebControls.Button
    Protected WithEvents cmdClear As System.Web.UI.WebControls.Button
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdClear1 As System.Web.UI.WebControls.Button
    Protected WithEvents dtgFITR As System.Web.UI.WebControls.DataGrid
    Protected WithEvents txtFITRCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAddFITRCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAddDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAddFITRR As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAddFITRI As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAddValidSDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAddValidEDate As System.Web.UI.WebControls.TextBox
    'Protected WithEvents ddlTaxType As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents ddlAddRate As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents ddlAddTaxType As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents ddlAddBranchCode As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents ddlAddCostCentre As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Hide_Add2 As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents hidMode As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_add_mod As System.Web.UI.WebControls.Label
    Protected WithEvents validateFITRCode As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents validateFITRDesc As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents validateFITRR As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents validateFITRI As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents validateValidSDate As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents validateValidEDate As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents revFITRR As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents revFITRI As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents cvDateNow As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents cvDate As System.Web.UI.WebControls.CompareValidator
    Dim ds As DataSet
    Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID") 'Modified for IPP GST Stage 2A

    Enum EnumFITR
        icChk
        icFITRCode
        icDesc
        icFITRR
        icFITRI
        icValidSDate
        icValidEDate
        icIndex
    End Enum
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

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgFITR)
        If Not IsPostBack Then
            cvDateNow.ValueToCompare = Date.Today.ToShortDateString
            cmdSearch.Enabled = True
            cmdClear1.Enabled = True
            cmdModify.Enabled = False
            cmdDelete.Enabled = False
            Hide_Add2.Style("display") = "none"

            'Dim dt As DateTime = New DateTime(2025, 1, 1)
            'txtAddValidEDate.Text = Format(dt, "yyyy-MM-dd")
            'txtAddValidEDate.Enabled = False
        End If

        cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
        cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")

    End Sub

    Sub dtgFITR_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgFITR.PageIndexChanged
        dtgFITR.CurrentPageIndex = e.NewPageIndex
        Bindgrid(True)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgFITR.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub Bindgrid(Optional ByVal pSorted As Boolean = False)

        Dim ds As DataSet

        ds = objIPPMain.RetFITR(txtFITRCode.Text)

        '//for sorting asc or desc
        Dim dvViewFITR As DataView
        dvViewFITR = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewFITR.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewFITR.Sort += " DESC"
        End If


        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            cmdModify.Enabled = True
            cmdDelete.Enabled = True
            resetDatagridPageIndex(dtgFITR, dvViewFITR)
            dtgFITR.DataSource = dvViewFITR
            dtgFITR.DataBind()
            dtgFITR.Visible = True
        Else
            cmdModify.Enabled = False
            cmdDelete.Enabled = False
            dtgFITR.Visible = False
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00006"))
        End If

        ViewState("PageCount") = dtgFITR.PageCount
    End Sub

    Private Sub dtgFITR_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgFITR.ItemCreated

        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgFITR, e)
        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If

    End Sub

    Private Sub dtgFITR_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgFITR.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(EnumFITR.icChk).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            If IsDBNull(dv("FM_FITR_RECOVERABLE")) Then
                e.Item.Cells(EnumFITR.icFITRR).Text = ""
            Else
                e.Item.Cells(EnumFITR.icFITRR).Text = Format(dv("FM_FITR_RECOVERABLE"), "###0.00")
            End If

            If IsDBNull(dv("FM_FITR_IRRECOVERABLE")) Then
                e.Item.Cells(EnumFITR.icFITRI).Text = ""
            Else
                e.Item.Cells(EnumFITR.icFITRI).Text = Format(dv("FM_FITR_IRRECOVERABLE"), "###0.00")
            End If

            If IsDBNull(dv("FM_VALID_FROM")) Then
                e.Item.Cells(EnumFITR.icValidSDate).Text = ""
            Else
                e.Item.Cells(EnumFITR.icValidSDate).Text = Format(CDate(dv("FM_VALID_FROM")), "dd/MM/yyyy")
            End If

            If IsDBNull(dv("FM_VALID_TO")) Then
                e.Item.Cells(EnumFITR.icValidEDate).Text = ""
            Else
                e.Item.Cells(EnumFITR.icValidEDate).Text = Format(CDate(dv("FM_VALID_TO")), "dd/MM/yyyy")
            End If
        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgFITR.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Dim dgItem As DataGridItem
        Dim chk As CheckBox

        Hide_Add2.Style("display") = "inline"
        validateFITRCode.Enabled = True
        validateFITRR.Enabled = True
        validateFITRDesc.Enabled = True
        validateFITRI.Enabled = True
        validateValidSDate.Enabled = True
        validateValidEDate.Enabled = True
        revFITRI.Enabled = True
        revFITRR.Enabled = True
        cvDateNow.Enabled = False
        cvDate.Enabled = True
        txtAddFITRCode.Enabled = True
        txtAddFITRCode.Text = ""
        txtAddDesc.Text = ""
        txtAddFITRR.Text = ""
        txtAddFITRI.Text = ""
        txtAddValidSDate.Text = ""
        'txtAddValidEDate.Text = ""
        Dim dt As DateTime = New DateTime(2025, 1, 1)
        txtAddValidEDate.Text = Format(dt, "dd/MM/yyyy")
        txtAddValidEDate.Enabled = False
        lblMsg.Text = ""
        Me.lbl_add_mod.Text = "add"
        cmdClear.Text = "Clear"
        hidMode.Value = "a"
        hidIndex.Value = ""

        For Each dgItem In dtgFITR.Items
            chk = dgItem.FindControl("chkSelection")
            chk.Checked = False
        Next

    End Sub

    Private Sub cmdClear1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClear1.Click

        txtFITRCode.Text = ""

    End Sub

    Private Sub cmd_clear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        If hidMode.Value = "a" Then
            txtAddFITRCode.Text = ""
            txtAddDesc.Text = ""
            txtAddFITRR.Text = ""
            txtAddFITRI.Text = ""
            txtAddValidSDate.Text = ""
            'txtAddValidEDate.Text = ""
            Dim dt As DateTime = New DateTime(2025, 1, 1)
            txtAddValidEDate.Text = Format(dt, "dd/MM/yyyy")
            txtAddValidEDate.Enabled = False
        Else
            txtAddFITRCode.Text = ViewState("oldFITRCode")
            txtAddDesc.Text = ViewState("oldFITRDesc")
            txtAddFITRR.Text = ViewState("oldFITRR")
            txtAddFITRI.Text = ViewState("oldFITRI")
            txtAddValidSDate.Text = ViewState("oldValidSDate")
            txtAddValidEDate.Text = ViewState("oldValidEDate")
        End If

        lblMsg.Text = ""

    End Sub

    Private Sub cmd_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click

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
                intmsgno = objIPPMain.AddFITRInfo(txtAddFITRCode.Text, txtAddDesc.Text, txtAddFITRR.Text, txtAddFITRI.Text, txtAddValidSDate.Text, txtAddValidEDate.Text, "add")
            ElseIf selected = "mod" Then
                Me.lbl_add_mod.Text = "modify"
                intmsgno = objIPPMain.AddFITRInfo(txtAddFITRCode.Text, txtAddDesc.Text, txtAddFITRR.Text, txtAddFITRI.Text, txtAddValidSDate.Text, txtAddValidEDate.Text, "mod", hidIndex.Value)
            End If

            Select Case intmsgno
                Case WheelMsgNum.Save
                    Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00003"), MsgBoxStyle.Information)
                    txtFITRCode.Text = ""
                    Bindgrid(0)
                    Hide_Add2.Style("display") = "none"
                    hidMode.Value = ""
                Case WheelMsgNum.NotSave
                    Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00007"), MsgBoxStyle.Information)
                    Hide_Add2.Style("display") = "none"
                    hidMode.Value = ""
                Case -99
                    Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00002"), MsgBoxStyle.Information)
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
        validateFITRCode.Enabled = True
        validateFITRDesc.Enabled = True
        validateFITRR.Enabled = True
        validateFITRI.Enabled = True
        validateValidEDate.Enabled = True
        validateValidSDate.Enabled = True
        revFITRI.Enabled = True
        revFITRR.Enabled = True
        cvDateNow.Enabled = False
        cvDate.Enabled = True
        txtAddFITRCode.Enabled = False
        Me.lbl_add_mod.Text = "modify"
        cmdClear.Text = "Reset"
        lblMsg.Text = ""
        hidMode.Value = "m"
        For Each dgItem In dtgFITR.Items
            chk = dgItem.FindControl("chkSelection")
            If chk.Checked Then
                txtAddFITRCode.Text = dgItem.Cells(EnumFITR.icFITRCode).Text
                txtAddDesc.Text = dgItem.Cells(EnumFITR.icDesc).Text
                txtAddFITRR.Text = dgItem.Cells(EnumFITR.icFITRR).Text
                txtAddFITRI.Text = dgItem.Cells(EnumFITR.icFITRI).Text
                txtAddValidSDate.Text = dgItem.Cells(EnumFITR.icValidSDate).Text
                txtAddValidEDate.Text = dgItem.Cells(EnumFITR.icValidEDate).Text
                hidIndex.Value = dgItem.Cells(EnumFITR.icIndex).Text

                ViewState("oldFITRCode") = dgItem.Cells(EnumFITR.icFITRCode).Text
                ViewState("oldFITRDesc") = dgItem.Cells(EnumFITR.icDesc).Text
                ViewState("oldFITRR") = dgItem.Cells(EnumFITR.icFITRR).Text
                ViewState("oldFITRI") = dgItem.Cells(EnumFITR.icFITRI).Text
                ViewState("oldValidSDate") = dgItem.Cells(EnumFITR.icValidSDate).Text
                ViewState("oldValidEDate") = dgItem.Cells(EnumFITR.icValidEDate).Text

                Exit For
            End If
            i = i + 1
        Next

    End Sub

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Dim dgItem As DataGridItem
        Dim chk As CheckBox

        Hide_Add2.Style("display") = "none"
        hidMode.Value = ""

        For Each dgItem In dtgFITR.Items
            chk = dgItem.FindControl("chkSelection")
            chk.Checked = False
        Next
    End Sub

    Private Function validateDatagrid(ByRef strMsg As String, ByVal strMode As String) As Boolean
        Dim strsql As String = ""
        Dim dsDate As New DataSet
        Dim strDtFrom, strDtTo, DtFrom, DtTo As Date
        Dim decFITRR, decFITRI As Decimal
        Dim i As Integer
        validateDatagrid = True
        strMsg = "<ul type='disc'>"
        strDtFrom = txtAddValidSDate.Text
        strDtTo = txtAddValidEDate.Text

        strsql = "SELECT DATE_FORMAT(FM_VALID_FROM, '%d/%m/%Y') AS FM_VALID_FROM, DATE_FORMAT(FM_VALID_TO, '%d/%m/%Y') AS FM_VALID_TO " & _
                "FROM FITR_MSTR WHERE FM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND FM_DELETED ='N' "

        If strMode = "mod" Then
            strsql &= "AND FM_FITR_INDEX <> " & hidIndex.Value & " "
        Else
            strsql &= "AND FM_VALID_FROM IS NOT NULL AND FM_VALID_TO IS NOT NULL "
        End If

        dsDate = objDb.FillDs(strsql)

        If objDb.Exist(strsql) Then
            For i = 0 To dsDate.Tables(0).Rows.Count - 1
                DtFrom = dsDate.Tables(0).Rows(i).Item("FM_VALID_FROM").ToString()
                DtTo = dsDate.Tables(0).Rows(i).Item("FM_VALID_TO").ToString()

                If (strDtFrom <= DtFrom And DtFrom <= strDtTo) Or (strDtFrom <= DtTo And DtTo <= strDtTo) Or (strDtFrom <= DtFrom And DtTo <= strDtTo) Or (DtFrom <= strDtFrom And strDtTo <= DtTo) Then
                    strMsg &= "<li>Valid Start/ End Date must not overlap with or within range of other Valid Start/ End Date.<ul type='disc'></ul></li>"
                    validateDatagrid = False
                    Exit For
                End If
            Next
        End If

        'Both field 
        decFITRR = CDec(txtAddFITRR.Text) * 100
        decFITRI = CDec(txtAddFITRI.Text) * 100

        'mimi 13/01/2017 - Fitr Enhancement
        If 100 <> (decFITRI + decFITRR) Then
            strMsg &= "<li>Total of FITR Recoverable and FITR Irrecoverable must be 100%<ul type='disc'></ul></li>"
            validateDatagrid = False
        End If
        'end-mimi

        'If (decFITRR <= 0) Or (decFITRI <= 0) Then
        '    strMsg &= "<li>FITR Recoverable /Irrecoverable cannot be lesser than or equal to zero.<ul type='disc'></ul></li>"
        '    validateDatagrid = False
        'ElseIf (decFITRI > decFITRR) Then
        '    strMsg &= "<li>FITR Irrecoverable cannot be greater than FITR Recoverable.<ul type='disc'></ul></li>"
        '    validateDatagrid = False
        'ElseIf 100 <> (decFITRI + decFITRR) Then
        '    strMsg &= "<li>Total of FITR Recoverable and FITR Irrecoverable must be 100%<ul type='disc'></ul></li>"
        '    validateDatagrid = False
        'End If

    End Function

    Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim dgItem As DataGridItem
        Dim intMsg As Integer
        Dim dtNewFITR As New DataTable
        Dim drFITR As DataRow
        dtNewFITR.Columns.Add("FITRIndex", Type.GetType("System.String")) 'Tax Index

        For Each dgItem In dtgFITR.Items
            Dim chkSelection As CheckBox = dgItem.Cells(EnumFITR.icChk).FindControl("chkSelection")

            If chkSelection.Checked Then
                drFITR = dtNewFITR.NewRow
                drFITR("FITRIndex") = dgItem.Cells(EnumFITR.icIndex).Text
                dtNewFITR.Rows.Add(drFITR)
            End If
        Next

        Dim objIPPMain As New IPPMain
        intMsg = objIPPMain.delFITRInfo(dtNewFITR)
        lblMsg.Text = ""
        If intMsg = -99 Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00009") & " " & objGlobal.GetErrorMessage("00018"), MsgBoxStyle.Information)
        ElseIf intMsg = WheelMsgNum.Delete Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00004"), MsgBoxStyle.Information)
            txtFITRCode.Text = ""
            Hide_Add2.Style("display") = "none"
            hidMode.Value = ""
            Bindgrid()
        ElseIf intMsg = WheelMsgNum.NotDelete Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00008"), MsgBoxStyle.Information)
        End If
    End Sub

    'Private Function chkGLCode() As Boolean
    '    'Modified for IPP GST Stage 2A
    '    Dim strChkCompId As String

    '    If strDefIPPCompID <> "" Then
    '        strChkCompId = strDefIPPCompID
    '    Else
    '        strChkCompId = Session("CompanyId")
    '    End If
    '    '---------------------------------
    '    If txtAddGLCode.Text = "N/A" Then
    '        chkGLCode = True
    '    Else
    '        If txtAddGLCode.Text <> "" Then
    '            If objDb.Exist("SELECT '*' FROM COMPANY_B_GL_CODE WHERE CBG_B_COY_ID = '" & strChkCompId & "' AND CBG_STATUS = 'A' AND CBG_B_GL_CODE = '" & Common.Parse(txtAddGLCode.Text) & "'") = 0 Then 'Modified for IPP GST Stage 2A
    '                chkGLCode = False
    '            Else
    '                chkGLCode = True
    '            End If
    '        End If
    '    End If
    'End Function
End Class
