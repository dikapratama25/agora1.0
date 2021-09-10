Imports AgoraLegacy
Imports eProcure.Component

Public Class ExchangeRate
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Protected WithEvents txt_code As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_description As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmd_search As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_clear As System.Web.UI.WebControls.Button
    Protected WithEvents dtg_exrate As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmd_Save As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_reset As System.Web.UI.HtmlControls.HtmlInputButton

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
        cmd_Save.Enabled = False
        Dim alButtonList As ArrayList       
        alButtonList = New ArrayList
        alButtonList.Add(cmd_Save)
        htPageAccess.Add("add", alButtonList)
        htPageAccess.Add("update", alButtonList)
        CheckButtonAccess()
        If intPageRecordCnt > 0 Then
            cmd_Save.Enabled = blnCanAdd Or blnCanUpdate
            cmd_reset.Disabled = Not (blnCanAdd Or blnCanUpdate)
        Else
            cmd_Save.Enabled = False
            cmd_reset.Disabled = True 'blnCanAdd Or blnCanUpdate
        End If
        'cmd_reset.Disabled = Not (blnCanAdd Or blnCanUpdate)
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        'Session("CompanyId") = "demo"
        'Session("UserId") = "moofh"
        'lblTitle.Text = "Shopping Cart"
        'blnPaging = False
        'blnCheckBox = False
        SetGridProperty(dtg_exrate)

        intPageRecordCnt = viewstate("intPageRecordCnt")
        If Not Page.IsPostBack Then
            '    Bindgrid(0)
            cmd_reset.Disabled = blnCanAdd Or blnCanUpdate
        End If
    End Sub

    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    'Put user code to initialize the page here
    '    MyBase.Page_Load(sender, e)
    '    SetGridProperty(dtg_exrate)

    '    If Not Page.IsPostBack Then
    '        Bindgrid(0)
    '    End If

    'End Sub

    Private Function Bindgrid() As String
        Dim objpro As New Admin
        Dim ds As New DataSet

        ds = objpro.getexrate(Me.txt_code.Text, Me.txt_description.Text)
        'ds = objpro.getexrate("VE", Me.txt_description.Text)OnPageIndexChanged

        viewstate("intPageRecordCnt") = ds.Tables(0).Rows.Count

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView

        dvViewSample.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        intPageRecordCnt = ds.Tables(0).Rows.Count
        '//bind datagrid
        '//datagrid.pageCount only got value after databind
        If intPageRecordCnt > 0 Then

            resetDatagridPageIndex(dtg_exrate, dvViewSample)

            dtg_exrate.DataSource = dvViewSample
            dtg_exrate.DataBind()
        Else
            dtg_exrate.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
            'intTotPage = 0
        End If
        '//datagrid.pageCount only got value after databind
        ' add for above checking
        viewstate("PageCount") = dtg_exrate.PageCount
        objpro = Nothing

    End Function

    Private Sub dtg_exrate_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_exrate.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            'Dim chk As CheckBox
            'chk = e.Item.Cells(0).FindControl("chkSelection")
            'chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            'Dim txt_delivery As TextBox
            'txt_delivery = e.Item.FindControl("txt_delivery")
            'txt_delivery.Text = "0"

            Dim txt_rate As TextBox
            txt_rate = e.Item.FindControl("txt_rate")

            '//TO assign value to textbox
            If IsDBNull(dv("CE_RATE")) = False Then
                txt_rate.Text = Common.parseNull(dv("CE_RATE"))
            Else
                txt_rate.Text = Common.parseNull(dv("CODE_VALUE"))
            End If
            'txt_rate.Text = Common.parseNull(dv("CE_RATE"))
            'txt_rate.Attributes.Add("onblur", "Test()")
        End If
    End Sub
    Private Sub cmd_search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_search.Click
        dtg_exrate.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub cmd_clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_clear.Click
        Me.txt_code.Text = ""
        Me.txt_description.Text = ""
    End Sub

    Private Sub cmd_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Save.Click

        Dim dgItem As DataGridItem
        Dim objval As New Admin
        Dim objDB As New EAD.DBCom
        Dim strAryQuery(0) As String
        Dim strSQL As String
        Dim txtRate As TextBox
        Dim strCode As String

        Dim i As Integer = 0

        For Each dgItem In dtg_exrate.Items
            strCode = dgItem.Cells(0).Text
            txtRate = dgItem.FindControl("txt_Rate")


            'intQty = CDbl(CType(dgItem.FindControl("txtQty"), TextBox).Text)
            'strRemark = CType(dgItem.FindControl("txtRemark"), TextBox).Text
            If txtRate.Text = "" Then
                strSQL = objval.modexrate(strCode, 1)
            Else
                strSQL = objval.modexrate(strCode, CDbl(txtRate.Text))
            End If
            Common.Insert2Ary(strAryQuery, strSQL)
        Next
        objDB.BatchExecute(strAryQuery)
        Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
        Bindgrid()
    End Sub


    Public Sub dtg_exrate_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtg_exrate.PageIndexChanged
        dtg_exrate.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Private Sub dtg_exrate_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_exrate.ItemCreated
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(dtg_exrate, e)

    End Sub


    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtg_exrate.CurrentPageIndex = 0
        Bindgrid()
    End Sub

End Class
