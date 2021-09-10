Imports AgoraLegacy
Imports eProcure.Component


Public Class IPPAnalysisCodeMaint
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDb As New EAD.DBCom
    Dim objIPPMAin As New IPPMain
    Dim objGlobal As New AppGlobals

    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents cmdModify As System.Web.UI.WebControls.Button
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents cmdCancel As System.Web.UI.WebControls.Button
    Protected WithEvents cmdClear As System.Web.UI.WebControls.Button
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdClear1 As System.Web.UI.WebControls.Button
    Protected WithEvents dtgTax As System.Web.UI.WebControls.DataGrid
    Protected WithEvents txtAnalysisCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAddAnalysisCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAddDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents ddlAddCodeType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlAnalysisCodeType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Hide_Add2 As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents hidMode As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lbl_add_mod As System.Web.UI.WebControls.Label
    Protected WithEvents validateCodeType As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents validateAnalysisCode As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents validateAnalysisCodeDesc As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents txtDesc As System.Web.UI.WebControls.TextBox 'Jules 2018.07.11
    Dim ds As DataSet
    Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID")


    Enum EnumTax
        icChk
        icAnalysisCode
        icCodeDesc
        icType
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
        SetGridProperty(dtgTax)
        If Not IsPostBack Then
            fillddl()
            cmdSearch.Enabled = True
            cmdClear1.Enabled = True
            cmdModify.Enabled = False
            cmdDelete.Enabled = False
            Hide_Add2.Style("display") = "none"
        End If

        cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
        cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")

    End Sub

    Sub fillddl()
        Dim cbolist As New ListItem
        Dim cbolist2 As New ListItem

        objGlobal.FillAnalysisCodeType(ddlAddCodeType, Session("CompanyID"))
        objGlobal.FillAnalysisCodeType(ddlAnalysisCodeType, Session("CompanyID"))

        cbolist.Value = ""
        cbolist.Text = "---Select---"
        ddlAddCodeType.Items.Insert(0, cbolist)
        ddlAnalysisCodeType.Items.Insert(0, cbolist)
    End Sub

    Sub dtgTax_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgTax.PageIndexChanged
        dtgTax.CurrentPageIndex = e.NewPageIndex
        Bindgrid(True)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgTax.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String

        Dim ds As DataSet

        'Jules 2018.07.11 - Allow search by Description
        If ddlAnalysisCodeType.SelectedIndex = 0 Then
            ds = objIPPMAin.getAnalysisCode(txtAnalysisCode.Text, "", txtDesc.Text)
        Else
            ds = objIPPMAin.getAnalysisCode(txtAnalysisCode.Text, ddlAnalysisCodeType.SelectedValue, txtDesc.Text)
        End If

        '//for sorting asc or desc
        Dim dvViewTax As DataView
        dvViewTax = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewTax.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewTax.Sort += " DESC"
        End If


        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            cmdModify.Enabled = True
            cmdDelete.Enabled = True
            resetDatagridPageIndex(dtgTax, dvViewTax)
            dtgTax.DataSource = dvViewTax
            dtgTax.DataBind()
            dtgTax.Visible = True
        Else
            cmdModify.Enabled = False
            cmdDelete.Enabled = False
            dtgTax.Visible = False
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00006"))
        End If

        ViewState("PageCount") = dtgTax.PageCount
    End Function

    Private Sub dtgTax_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgTax.ItemCreated

        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgTax, e)
        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If

    End Sub

    Private Sub dtgTax_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgTax.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            Select Case dv("AC_DEPT_CODE")
                Case "L1"
                    e.Item.Cells(EnumTax.icType).Text = "Fund Type"
                Case "L2"
                    e.Item.Cells(EnumTax.icType).Text = "Product Type"
                Case "L3"
                    e.Item.Cells(EnumTax.icType).Text = "Channel"
                Case "L4"
                    e.Item.Cells(EnumTax.icType).Text = "Reinsurance Company"
                Case "L5"
                    e.Item.Cells(EnumTax.icType).Text = "Asset Fund"
                'Case "L6"
                '    e.Item.Cells(EnumTax.icType).Text = "Tax Code"
                'Case "L7"
                '    e.Item.Cells(EnumTax.icType).Text = "Cost Centre"
                Case "L8"
                    e.Item.Cells(EnumTax.icType).Text = "Project Code"
                Case Else
                    e.Item.Cells(EnumTax.icType).Text = "Person Code"
            End Select

        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgTax.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        'Dim dgItem As DataGridItem
        'Dim chk As CheckBox

        Hide_Add2.Style("display") = "inline"
        validateCodeType.Enabled = True
        validateAnalysisCode.Enabled = True
        validateAnalysisCodeDesc.Enabled = True
        ddlAddCodeType.SelectedIndex = 0
        txtAddAnalysisCode.Text = ""
        txtAddDesc.Text = ""
        Me.lbl_add_mod.Text = "add"
        cmdClear.Text = "Clear"
        hidMode.Value = "a"
        hidIndex.Value = ""
        txtAddAnalysisCode.ReadOnly = False 'Jules 2018.07.12
    End Sub

    Private Sub cmdClear1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClear1.Click
        txtAnalysisCode.Text = ""
        ddlAnalysisCodeType.SelectedIndex = 0
    End Sub

    Private Sub cmd_clear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        If hidMode.Value = "a" Then
            'txtAddGLCodeNoBR.Text = "" 'Zulham 08/05/2018 - PAMB
            fillddl()
        Else
            'txtAddGLCodeNoBR.Text = ViewState("oldGLCode") 'Zulham 08/05/2018 - PAMB
            'ddlAddCostCentreNoBR.SelectedValue = ViewState("oldCostCentre") 'Zulham 08/05/2018 - PAMB
        End If

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


        If selected = "add" Then
            Me.lbl_add_mod.Text = "add"
            intmsgno = objIPPMAin.AddAnalysisCode(txtAddAnalysisCode.Text, txtAddDesc.Text, ddlAddCodeType.SelectedValue)
        ElseIf selected = "mod" Then
            Me.lbl_add_mod.Text = "modify"
            intmsgno = objIPPMAin.updateAnalysisCode(txtAddAnalysisCode.Text, txtAddDesc.Text, ddlAddCodeType.SelectedValue, ViewState("oldAnalysisCode"), ViewState("oldCodeType"))
        End If

        Select Case intmsgno
            Case WheelMsgNum.Save
                Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00003"), MsgBoxStyle.Information)
                txtAnalysisCode.Text = ""
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

    End Sub

    Private Sub cmdModify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdModify.Click
        Dim dgItem As DataGridItem
        Dim chk As CheckBox
        Dim i As Integer
        i = 0

        Hide_Add2.Style("display") = "inline"
        validateCodeType.Enabled = True
        validateAnalysisCode.Enabled = True
        validateAnalysisCodeDesc.Enabled = True
        Me.lbl_add_mod.Text = "modify"
        cmdClear.Text = "Reset"
        hidMode.Value = "m"
        txtAddAnalysisCode.ReadOnly = True 'Jules 2018.07.12
        For Each dgItem In dtgTax.Items
            chk = dgItem.FindControl("chkSelection")
            If chk.Checked Then
                ddlAddCodeType.SelectedValue = dgItem.Cells(EnumTax.icIndex).Text
                txtAddAnalysisCode.Text = dgItem.Cells(EnumTax.icAnalysisCode).Text
                txtAddDesc.Text = dgItem.Cells(EnumTax.icCodeDesc).Text
                hidIndex.Value = dgItem.Cells(EnumTax.icIndex).Text

                ViewState("oldAnalysisCode") = dgItem.Cells(EnumTax.icAnalysisCode).Text
                'ViewState("oldDesc") = dgItem.Cells(EnumTax.icCodeDesc).Text
                ViewState("oldCodeType") = dgItem.Cells(EnumTax.icIndex).Text
                Exit For
            End If
            i = i + 1
        Next

    End Sub

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Hide_Add2.Style("display") = "none"
        hidMode.Value = ""
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim dgItem As DataGridItem
        Dim intMsg As Integer
        Dim dtNewTax As New DataTable
        Dim drTax As DataRow
        dtNewTax.Columns.Add("TaxIndex", Type.GetType("System.String")) 'Tax Index

        For Each dgItem In dtgTax.Items
            Dim chkSelection As CheckBox = dgItem.Cells(0).FindControl("chkSelection")

            If chkSelection.Checked Then
                'drTax = dtNewTax.NewRow
                'drTax("TaxIndex") = dgItem.Cells(EnumTax.icIndex).Text
                'dtNewTax.Rows.Add(drTax)
                intMsg = objIPPMAin.deleteAnalysisCode(dgItem.Cells(EnumTax.icAnalysisCode).Text, dgItem.Cells(EnumTax.icIndex).Text)
            End If
        Next

        Dim objAdmin As New Admin
        If intMsg = -99 Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00009") & " " & objGlobal.GetErrorMessage("00018"), MsgBoxStyle.Information)
        ElseIf intMsg = WheelMsgNum.Delete Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00004"), MsgBoxStyle.Information)
            txtAnalysisCode.Text = ""
            Hide_Add2.Style("display") = "none"
            hidMode.Value = ""
            Bindgrid()
        ElseIf intMsg = WheelMsgNum.NotDelete Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00008"), MsgBoxStyle.Information)
        End If
    End Sub

End Class
