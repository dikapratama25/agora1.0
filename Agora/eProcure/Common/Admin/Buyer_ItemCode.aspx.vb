Imports AgoraLegacy
Imports eProcure.Component


Public Class Buyer_ItemCode
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblName As System.Web.UI.WebControls.Label
    Protected WithEvents ddl_Select As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents vldName2 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents dtg_BuyerCode As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmd_Save As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Reset2 As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
    Dim strVendorName As String
    Protected WithEvents txtID As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents cmdClear As System.Web.UI.WebControls.Button
    Protected WithEvents hidChange As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidItem As System.Web.UI.HtmlControls.HtmlInputHidden

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
        cmd_save.Enabled = False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmd_save)
        htPageAccess.Add("add", alButtonList)
        htPageAccess.Add("update", alButtonList)
        CheckButtonAccess()
        If viewstate("intPageRecordCnt") > 0 Then
            cmd_Save.Enabled = blnCanAdd Or blnCanUpdate
            cmd_Reset2.Disabled = Not (blnCanAdd Or blnCanUpdate)
        Else
            cmd_Save.Enabled = False
        End If
        'cmd_Reset.Enabled = blnCanAdd Or blnCanUpdate Or blnCanDelete
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtg_BuyerCode)
        If Not IsPostBack Then
            ddl_Select.Attributes.Add("onchange", "Change()")
            hidChange.Value = "0"
            BindVendor()
        End If

        intPageRecordCnt = viewstate("intPageRecordCnt")
        cmd_Save.Enabled = False
        cmd_Reset2.Disabled = True
    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim objAdmin As New Admin
        Dim ds As New DataSet
        Dim strSelect As String

        strSelect = ddl_Select.SelectedItem.Value

        ds = objAdmin.Populate_ProductCode(strSelect, txtID.Text, txtDesc.Text)
        viewstate("intPageRecordCnt") = ds.Tables(0).Rows.Count
        Dim dvViewSample As DataView

        dvViewSample = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewSample.Sort = viewstate("SortExpression")
            If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"
        End If

        If viewstate("intPageRecordCnt") > 0 Then
            resetDatagridPageIndex(dtg_BuyerCode, dvViewSample)
            cmd_Save.Enabled = True
            cmd_Reset2.Disabled = False
            dtg_BuyerCode.DataSource = dvViewSample
            dtg_BuyerCode.DataBind()
            dtg_BuyerCode.Visible = True

        Else
            cmd_Save.Enabled = False
            cmd_Reset2.Disabled = True
            dtg_BuyerCode.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If
        'If Session("Env") = "FTN" Then
        '    Me.dtg_BuyerCode.Columns(3).Visible = False
        '    Me.dtg_BuyerCode.Columns(4).Visible = False
        '    Me.dtg_BuyerCode.Columns(5).Visible = False
        'Else
        '    Me.dtg_BuyerCode.Columns(3).Visible = True
        '    Me.dtg_BuyerCode.Columns(4).Visible = True
        '    Me.dtg_BuyerCode.Columns(5).Visible = True
        'End If
        Me.dtg_BuyerCode.Columns(3).Visible = True
        Me.dtg_BuyerCode.Columns(4).Visible = True
        Me.dtg_BuyerCode.Columns(5).Visible = True
        ' add for above checking
        viewstate("PageCount") = dtg_BuyerCode.PageCount
    End Function
    Private Function BindVendor()
        Dim strSelect As String
        Dim objAdmin As New Admin
        Dim dvVendor As DataView
        dvVendor = objAdmin.searchvendor("AV", "", "").Tables(0).DefaultView
        ddl_Select.Items.Clear()       
        Dim lstItem As New ListItem
        If Not dvVendor Is Nothing Then
            Common.FillDdl(ddl_Select, "CM_COY_NAME", "CV_S_COY_ID", dvVendor)
            ' Add ---Select---
            lstItem.Value = ""
            lstItem.Text = "---Select---"
            ddl_Select.Items.Insert(0, lstItem)
        End If

    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtg_BuyerCode.CurrentPageIndex = 0
        Bindgrid(0, True)
    End Sub

    Public Sub dtg_BuyerCode_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtg_BuyerCode.PageIndexChanged
        dtg_BuyerCode.CurrentPageIndex = e.NewPageIndex
        Bindgrid(0, True)
    End Sub

    Private Sub dtg_BuyerCode_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_BuyerCode.ItemCreated
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(dtg_BuyerCode, e)
    End Sub

    Private Sub ddl_Select_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Select.SelectedIndexChanged
        hidItem.Value = ddl_Select.SelectedIndex

        If ddl_Select.SelectedIndex <> 0 Then
            Bindgrid(0, True)
        End If
    End Sub

    Private Sub cmd_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_Save.Click
        Dim dgItem As DataGridItem
        Dim objAdmin As New Admin
        Dim objDB As New EAD.DBCom
        Dim strAryQuery(0) As String
        Dim strSQL As String
        Dim txtCode, txtCategory, txtGL, txtTax As TextBox
        Dim strCode As String

        Dim i As Integer = 0

        For Each dgItem In dtg_BuyerCode.Items
            strCode = dgItem.Cells(0).Text
            txtCode = dgItem.FindControl("txt_Code")
            txtCategory = dgItem.FindControl("txtCategory")
            txtGL = dgItem.FindControl("txtGL")
            txtTax = dgItem.FindControl("txtTax")
            strSQL = objAdmin.updateItemCode(strCode, txtCode.Text, txtCategory.Text, txtGL.Text, txtTax.Text)
            Common.Insert2Ary(strAryQuery, strSQL)
        Next

        objDB.BatchExecute(strAryQuery)
        Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
        Bindgrid(0, True)
        hidChange.Value = "0"
    End Sub

    Private Sub dtg_BuyerCode_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_BuyerCode.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim txtCode As TextBox
            txtCode = e.Item.FindControl("txt_Code")
            txtCode.Text = Common.parseNull(dv("CBC_B_ITEM_CODE"))

            Dim hidCode As HtmlInputHidden
            hidCode = e.Item.FindControl("hidCode")
            hidCode.Value = Common.parseNull(dv("CBC_B_ITEM_CODE"))

            Dim txtCategory As TextBox
            txtCategory = e.Item.FindControl("txtCategory")
            txtCategory.Text = Common.parseNull(dv("CBC_B_CATEGORY_CODE"))

            Dim hidCategory As HtmlInputHidden
            hidCategory = e.Item.FindControl("hidCategory")
            hidCategory.Value = Common.parseNull(dv("CBC_B_CATEGORY_CODE"))

            Dim txtGL As TextBox
            txtGL = e.Item.FindControl("txtGL")
            txtGL.Text = Common.parseNull(dv("CBC_B_GL_CODE"))

            Dim hidGL As HtmlInputHidden
            hidGL = e.Item.FindControl("hidGL")
            hidGL.Value = Common.parseNull(dv("CBC_B_GL_CODE"))

            Dim txtTax As TextBox
            txtTax = e.Item.FindControl("txtTax")
            txtTax.Text = Common.parseNull(dv("CBC_B_TAX_CODE"))

            Dim hidTax As HtmlInputHidden
            hidTax = e.Item.FindControl("hidTax")
            hidTax.Value = Common.parseNull(dv("CBC_B_TAX_CODE"))

            txtCode.Attributes.Add("onblur", "CheckAll('" & txtCode.ClientID & "', '" & hidCode.ClientID & "');")
            txtCode.Attributes.Add("onblur", "CheckAll('" & txtCategory.ClientID & "', '" & hidCategory.ClientID & "');")
            txtCode.Attributes.Add("onblur", "CheckAll('" & txtGL.ClientID & "', '" & hidGL.ClientID & "');")
            txtCode.Attributes.Add("onblur", "CheckAll('" & txtTax.ClientID & "', '" & hidTax.ClientID & "');")
        End If
    End Sub

    Private Sub cmdSearch_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtg_BuyerCode.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub cmdClear_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        txtID.Text = ""
        txtDesc.Text = ""
    End Sub
End Class
