Imports System.Data.SqlClient
Imports AgoraLegacy


Public Class VendorMapping
    Inherits AgoraLegacy.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lbl_result As System.Web.UI.WebControls.Label
    Protected WithEvents dtg_VendorMap As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_reset As System.Web.UI.HtmlControls.HtmlInputButton

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim objHubAdmin As New  HubAdmin


    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here()
        MyBase.Page_Load(sender, e)
        'blnPaging = False
        SetGridProperty(dtg_VendorMap)

        If Not Page.IsPostBack Then
            Bindgrid(0, True)
            'Else
            '    If viewstate("SortExpression") = "" Then
            '        Bindgrid(0)
            '    Else
            '        Bindgrid(0, True)
            '    End If
        End If
        'cmdSubmit.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection');")
        'Dim dsVendorMap As New DataSet
        'dsVendorMap = objHubAdmin.getVendorMap
        intPageRecordCnt = viewstate("intPageRecordCnt")

    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String

        Dim objHubAdmin As New  HubAdmin
        Dim ds As DataSet = New DataSet
        ds = objHubAdmin.getVendorMap()
        'viewstate("intPageRecordCnt") = ds.Tables(0).Rows.Count

        intPageRecordCnt = ds.Tables(0).Rows.Count
        viewstate("intPageRecordCnt") = intPageRecordCnt

        Dim dvViewVendor As DataView
        dvViewVendor = ds.Tables(0).DefaultView

        'If pSorted Then
        dvViewVendor.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" Then dvViewVendor.Sort += " DESC"

        If intPageRecordCnt > 0 Then
            ' check when user re-enter search criteria and click on other page without click search button
            If dtg_VendorMap.CurrentPageIndex > (dvViewVendor.Count \ dtg_VendorMap.PageSize) Then
                dtg_VendorMap.CurrentPageIndex = IIf((dvViewVendor.Count \ dtg_VendorMap.PageSize) = 1, 0, (dvViewVendor.Count \ dtg_VendorMap.PageSize))
            ElseIf dtg_VendorMap.CurrentPageIndex = (dvViewVendor.Count \ dtg_VendorMap.PageSize) Then
                If viewstate("PageCount") = (dvViewVendor.Count \ dtg_VendorMap.PageSize) Then
                    'user does not re-enter search criteria 
                    dtg_VendorMap.CurrentPageIndex = IIf((dvViewVendor.Count \ dtg_VendorMap.PageSize) = 0, 0, (dvViewVendor.Count \ dtg_VendorMap.PageSize) - 1)
                Else
                    If (dvViewVendor.Count Mod dtg_VendorMap.PageSize) = 0 Then
                        ' total record = 10, 20, ...
                        dtg_VendorMap.CurrentPageIndex = IIf((dvViewVendor.Count \ dtg_VendorMap.PageSize) = 1, 0, (dvViewVendor.Count \ dtg_VendorMap.PageSize))
                    Else
                        ' total record = 11, 12, ...
                        dtg_VendorMap.CurrentPageIndex = (dvViewVendor.Count \ dtg_VendorMap.PageSize)
                    End If
                End If
            End If
            dtg_VendorMap.DataSource = dvViewVendor
            dtg_VendorMap.DataBind()
        Else
            dtg_VendorMap.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
            cmdSave.Enabled = False
            cmd_reset.Disabled = True

        End If

        'dtg_VendorMap.AllowPaging = False



    End Function

    Private Sub dtg_VendorMap_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_VendorMap.ItemDataBound

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            'Dim chk As CheckBox
            ' chk = e.Item.Cells(0).FindControl("chkSelection")
            'chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            'Dim txt_delivery As TextBox
            'txt_delivery = e.Item.FindControl("txt_delivery")
            'txt_delivery.Text = "0"

            Dim txt_code As TextBox
            txt_code = e.Item.FindControl("txt_code")
            txt_code.Text = Common.parseNull(dv("VM_VENDOR_MAPPING"))
            'Common.parseNull(dv("CE_RATE"))'txt_code.Attributes.Add("onblur", "Test()")
        End If 
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim dgItem As DataGridItem
        Dim objhubAdmin As New  HubAdmin
        Dim objDB As New  EAD.DBCom(ConfigurationSettings.AppSettings("eProcurePath"))
        Dim strAryQuery(0) As String
        Dim strSQL As String
        Dim txtCode As TextBox
        Dim strName As String
        Dim strSqlDel As String


        Dim i As Integer = 0

        For Each dgItem In dtg_VendorMap.Items
            strName = dgItem.Cells(0).Text
            txtCode = dgItem.FindControl("txt_Code")

            strSQL = objhubAdmin.updateVendorCode(txtCode.Text, strName)


            'strSQL = objval.modexrate(strCode, CDbl(txtRate.Text))
            Common.Insert2Ary(strAryQuery, strSQL)
        Next 'Dim i As Integer = 0

        'For Each dgItem In dtg_VendorMap.Items
        '    Dim chk As CheckBox
        '    chk = dgItem.FindControl("chkSelection")
        '    If chk.Checked Then
        '        strName = dgItem.Cells(1).Text
        '        txtCode = dgItem.FindControl("txt_Code")
        '        strSQL = objhubAdmin.updateVendorCode(txtCode.Text, strName)
        '        Common.Insert2Ary(strAryQuery, strSQL)
        '    End If


        '    intQty = CDbl(CType(dgItem.FindControl("txtQty"), TextBox).Text)
        '    strRemark = CType(dgItem.FindControl("txtRemark"), TextBox).Text

        '    strSQL = objval.modexrate(strCode, (txt_Code.Text))

        'Next
        objDB.BatchExecute(strAryQuery)

        Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
        Bindgrid()
    End Sub
    Sub SortCommand_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles dtg_VendorMap.SortCommand
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_SortCommand(sender, e)
        dtg_VendorMap.CurrentPageIndex = 0
        Bindgrid()
    End Sub


    Private Sub dtg_VendorMap_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_VendorMap.ItemCreated
        intPageRecordCnt = viewstate("intPageRecordCnt")
        'blnPaging = False
        Grid_ItemCreated(dtg_VendorMap, e)

    End Sub
    Public Sub dtg_VendorMap_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtg_VendorMap.PageIndexChanged
        dtg_VendorMap.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub
End Class
