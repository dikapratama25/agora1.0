Imports AgoraLegacy
Imports eProcure.Component


Public Class GSTTaxCodeMaint
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDb As New EAD.DBCom
    Dim objAdm As New Admin
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
    Protected WithEvents txtTaxCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAddTaxCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAddDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAddGLCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents ddlTaxType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlAddRate As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlAddTaxType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlAddBranchCode As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlAddCostCentre As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Hide_Add2 As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents hidMode As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lbl_add_mod As System.Web.UI.WebControls.Label
    Protected WithEvents validateTaxType As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents validateTaxCode As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents validateTaxCodeDesc As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents validateRate As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents validateGLCode As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents validateBranchCode As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents validateCostCentre As System.Web.UI.WebControls.RequiredFieldValidator
    Dim ds As DataSet
    Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID") 'Modified for IPP GST Stage 2A
    'Zulham 08/05/2018 - PAMB
    Protected trWithBR As System.Web.UI.HtmlControls.HtmlTableRow
    Protected trNoBR As System.Web.UI.HtmlControls.HtmlTableRow
    'Protected txtAddGLCodeNoBR As System.Web.UI.WebControls.TextBox
    Protected ddlAddCostCentreNoBR As System.Web.UI.WebControls.DropDownList
    Protected validateGLCodeNoBR As System.Web.UI.WebControls.RequiredFieldValidator
    Protected validateCostCentreNoBR As System.Web.UI.WebControls.RequiredFieldValidator
    Protected Label9 As System.Web.UI.WebControls.Label


    Enum EnumTax
        icChk
        icTaxCode
        icDesc
        icTypeDesc
        icTaxRate
        icGLCode
        icGLDesc
        icBranchCode
        icCostCentre
        icType
        icRate
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
            'objGlobal.FillCodeTable(ddlAddCountry, CodeTable.Country)
            fillddl()
            cmdSearch.Enabled = True
            cmdClear1.Enabled = True
            cmdModify.Enabled = False
            cmdDelete.Enabled = False
            Hide_Add2.Style("display") = "none"
        End If

        cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
        cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")

        'Zulham 08/05/2018 - PAMB
        If Session("CompanyID").ToString.ToUpper = "PAMB" Then
            validateBranchCode.ErrorMessage = ""
            validateGLCode.ErrorMessage = ""
            validateCostCentre.ErrorMessage = ""
            validateCostCentreNoBR.ErrorMessage = "Cost Centre is required."
        Else
            validateBranchCode.ErrorMessage = "Branch Code is required."
            validateGLCode.ErrorMessage = "GL Code is required."
            validateCostCentre.ErrorMessage = "Cost Centre is required."
            validateGLCodeNoBR.ErrorMessage = ""
            validateCostCentreNoBR.ErrorMessage = ""
        End If
        'End

    End Sub

    Sub fillddl()
        Dim cbolist As New ListItem
        Dim cbolist2 As New ListItem

        objGlobal.FillGST(ddlAddRate, False)

        cbolist.Value = ""
        cbolist.Text = "---Select---"
        cbolist2.Value = "N/A"
        cbolist2.Text = "N/A"
        Common.FillDefault(ddlAddBranchCode, "COMPANY_BRANCH_MSTR", "CBM_BRANCH_CODE", "CBM_BRANCH_CODE", , " CBM_COY_ID = '" & Session("CompanyId") & "' AND CBM_STATUS = 'A'")
        ddlAddBranchCode.Items.Insert(0, cbolist)
        ddlAddBranchCode.Items.Insert(1, cbolist2)
        Common.FillDefault(ddlAddCostCentre, "COST_CENTRE", "CC_CC_CODE", "CC_CC_CODE", , " CC_COY_ID = '" & Session("CompanyId") & "' AND CC_STATUS = 'A'")
        ddlAddCostCentre.Items.Insert(0, cbolist)
        ddlAddCostCentre.Items.Insert(1, cbolist2)
        'Zulham 08/05/2018 - PAMB
        Common.FillDefault(ddlAddCostCentreNoBR, "COST_CENTRE", "CC_CC_CODE", "CC_CC_CODE", , " CC_COY_ID = '" & Session("CompanyId") & "' AND CC_STATUS = 'A'")
        ddlAddCostCentreNoBR.Items.Insert(0, cbolist)
        ddlAddCostCentreNoBR.Items.Insert(1, cbolist2)
        'End
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

        ds = objAdm.getTaxCodeInfo(txtTaxCode.Text, "IPP", Session("CompanyId"))

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

            'Zulham 08/05/2018 - PAMB
            If Session("CompanyID").ToString.ToUpper = "PAMB" Then
                e.Item.Cells(EnumTax.icGLCode).Visible = False
                e.Item.Cells(EnumTax.icGLDesc).Visible = False
                e.Item.Cells(EnumTax.icBranchCode).Visible = False
            End If
            'End 

        End If

    End Sub

    Private Sub dtgTax_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgTax.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            'Zulham 08/05/2018 - PAMB
            If Session("CompanyID").ToString.ToUpper = "PAMB" Then
                e.Item.Cells(EnumTax.icGLCode).Visible = False
                e.Item.Cells(EnumTax.icGLDesc).Visible = False
                e.Item.Cells(EnumTax.icBranchCode).Visible = False
            End If
            'End 

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
        validateTaxType.Enabled = True
        validateTaxCode.Enabled = True
        validateTaxCodeDesc.Enabled = True
        validateRate.Enabled = True
        validateCostCentre.Enabled = True

        'Zulham 08/05/2018 - PAMB
        'Toggles table rows visibility
        If Session("CompanyID").ToString.ToUpper = "PAMB" Then
            trWithBR.Visible = False
            trNoBR.Visible = True
        Else
            trWithBR.Visible = True
            trNoBR.Visible = False
        End If
        'End

        validateGLCode.Enabled = True
        ddlAddTaxType.Enabled = True
        txtAddTaxCode.Enabled = True
        ddlAddTaxType.SelectedIndex = 0
        fillddl()
        txtAddGLCode.Text = ""
        'txtAddGLCodeNoBR.Text = "" 'Zulham 08/05/2018 - PAMB
        txtAddTaxCode.Text = ""
        txtAddDesc.Text = ""
        Me.lbl_add_mod.Text = "add"
        cmdClear.Text = "Clear"
        hidMode.Value = "a"
        hidIndex.Value = ""

    End Sub

    Private Sub cmdClear1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClear1.Click

        txtTaxCode.Text = ""

    End Sub

    Private Sub cmd_clear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        If hidMode.Value = "a" Then
            txtAddTaxCode.Text = ""
            txtAddDesc.Text = ""
            txtAddGLCode.Text = ""
            'txtAddGLCodeNoBR.Text = "" 'Zulham 08/05/2018 - PAMB
            ddlAddTaxType.SelectedIndex = 0
            fillddl()
        Else
            txtAddTaxCode.Text = ViewState("oldTaxCode")
            txtAddDesc.Text = ViewState("oldTaxDesc")
            txtAddGLCode.Text = ViewState("oldGLCode")
            'txtAddGLCodeNoBR.Text = ViewState("oldGLCode") 'Zulham 08/05/2018 - PAMB
            ddlAddRate.SelectedValue = ViewState("oldTaxRate")
            ddlAddTaxType.SelectedValue = ViewState("oldTaxType")
            ddlAddBranchCode.SelectedValue = ViewState("oldBranchCode")
            ddlAddCostCentre.SelectedValue = ViewState("oldCostCentre")
            ddlAddCostCentreNoBR.SelectedValue = ViewState("oldCostCentre") 'Zulham 08/05/2018 - PAMB
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

        'Zulham 08/05/2018 - PAMB
        If Session("CompanyID").ToString.ToUpper = "PAMB" Then
            txtAddGLCode.Text = "N/A"
            ddlAddCostCentre.SelectedValue = ddlAddCostCentreNoBR.SelectedValue
        End If
        'End

        If chkGLCode() = True Then
            If selected = "add" Then
                Me.lbl_add_mod.Text = "add"
                intmsgno = objAdm.AddTaxCodeInfo(txtAddTaxCode.Text, txtAddDesc.Text, "MY", ddlAddTaxType.SelectedValue, ddlAddRate.SelectedValue, "add", , "IPP", txtAddGLCode.Text, ddlAddBranchCode.SelectedValue, ddlAddCostCentre.SelectedValue)
            ElseIf selected = "mod" Then
                Me.lbl_add_mod.Text = "modify"
                intmsgno = objAdm.AddTaxCodeInfo(txtAddTaxCode.Text, txtAddDesc.Text, "MY", ddlAddTaxType.SelectedValue, ddlAddRate.SelectedValue, "mod", hidIndex.Value, "IPP", txtAddGLCode.Text, ddlAddBranchCode.SelectedValue, ddlAddCostCentre.SelectedValue)
            End If

            Select Case intmsgno
                Case WheelMsgNum.Save
                    Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00003"), MsgBoxStyle.Information)
                    txtTaxCode.Text = ""
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
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00353"), MsgBoxStyle.Information)
        End If
        
    End Sub

    Private Sub cmdModify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdModify.Click
        Dim dgItem As DataGridItem
        Dim chk As CheckBox
        Dim i As Integer
        i = 0

        Hide_Add2.Style("display") = "inline"
        validateTaxType.Enabled = True
        validateTaxCode.Enabled = True
        validateTaxCodeDesc.Enabled = True
        validateRate.Enabled = True
        validateCostCentre.Enabled = True
        validateBranchCode.Enabled = True
        validateGLCode.Enabled = True
        txtAddTaxCode.Enabled = False
        Me.lbl_add_mod.Text = "modify"
        cmdClear.Text = "Reset"
        hidMode.Value = "m"
        For Each dgItem In dtgTax.Items
            chk = dgItem.FindControl("chkSelection")
            If chk.Checked Then
                ddlAddTaxType.SelectedValue = dgItem.Cells(EnumTax.icType).Text
                ddlAddRate.SelectedValue = dgItem.Cells(EnumTax.icRate).Text
                If Session("CompanyID").ToString.ToUpper <> "PAMB" Then ddlAddBranchCode.SelectedValue = dgItem.Cells(EnumTax.icBranchCode).Text 'Zulham 14/05/2018 - PAMB
                ddlAddCostCentre.SelectedValue = dgItem.Cells(EnumTax.icCostCentre).Text
                ddlAddCostCentreNoBR.SelectedValue = dgItem.Cells(EnumTax.icCostCentre).Text 'Zulham 08/05/2018 - PAMB
                txtAddTaxCode.Text = dgItem.Cells(EnumTax.icTaxCode).Text
                txtAddDesc.Text = dgItem.Cells(EnumTax.icDesc).Text
                txtAddGLCode.Text = dgItem.Cells(EnumTax.icGLCode).Text
                'txtAddGLCodeNoBR.Text = dgItem.Cells(EnumTax.icGLCode).Text 'Zulham 08/05/2018
                'txtAddFITR.Text = dgItem.Cells(EnumTax.icFITR).Text
                hidIndex.Value = dgItem.Cells(EnumTax.icIndex).Text

                ViewState("oldTaxType") = dgItem.Cells(EnumTax.icType).Text
                ViewState("oldTaxRate") = dgItem.Cells(EnumTax.icRate).Text
                ViewState("oldTaxCode") = dgItem.Cells(EnumTax.icTaxCode).Text
                ViewState("oldTaxDesc") = dgItem.Cells(EnumTax.icDesc).Text
                ViewState("oldGLCode") = dgItem.Cells(EnumTax.icGLCode).Text
                'Zulham 14/05/2018 - PAMB
                If Session("CompanyID").ToString.ToUpper <> "PAMB" Then
                    ViewState("oldBranchCode") = dgItem.Cells(EnumTax.icBranchCode).Text
                Else
                    ViewState("oldBranchCode") = ""
                End If
                'End
                ViewState("oldCostCentre") = dgItem.Cells(EnumTax.icCostCentre).Text

                Exit For
            End If
            i = i + 1
        Next

        'Zulham 08/05/2018 - PAMB
        'Toggles table rows visibility
        If Session("CompanyID").ToString.ToUpper = "PAMB" Then
            trWithBR.Visible = False
            trNoBR.Visible = True
        End If
        'End

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
                drTax = dtNewTax.NewRow
                drTax("TaxIndex") = dgItem.Cells(EnumTax.icIndex).Text
                dtNewTax.Rows.Add(drTax)
            End If
        Next

        Dim objAdmin As New Admin
        intMsg = objAdmin.delTaxCodeInfo(dtNewTax, "IPP")
        If intMsg = -99 Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00009") & " " & objGlobal.GetErrorMessage("00018"), MsgBoxStyle.Information)
        ElseIf intMsg = WheelMsgNum.Delete Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00004"), MsgBoxStyle.Information)
            txtTaxCode.Text = ""
            Hide_Add2.Style("display") = "none"
            hidMode.Value = ""
            Bindgrid()
        ElseIf intMsg = WheelMsgNum.NotDelete Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00008"), MsgBoxStyle.Information)
        End If
    End Sub

    Private Function chkGLCode() As Boolean
		'Modified for IPP GST Stage 2A
        Dim strChkCompId As String

        If strDefIPPCompID <> "" Then
            strChkCompId = strDefIPPCompID
        Else
            strChkCompId = Session("CompanyId")
        End If
        '---------------------------------

        If txtAddGLCode.Text = "N/A" Then
            chkGLCode = True
        Else
            If txtAddGLCode.Text <> "" Then
                If objDb.Exist("SELECT '*' FROM COMPANY_B_GL_CODE WHERE CBG_B_COY_ID = '" & strChkCompId & "' AND CBG_STATUS = 'A' AND CBG_B_GL_CODE = '" & Common.Parse(txtAddGLCode.Text) & "'") = 0 Then 'Modified for IPP GST Stage 2A
                    chkGLCode = False
                Else
                    chkGLCode = True
                End If
            End If
        End If
    End Function
End Class
