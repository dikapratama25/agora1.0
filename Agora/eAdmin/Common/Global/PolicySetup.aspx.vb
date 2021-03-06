Imports AgoraLegacy
Imports SSO.Component

Public Class PolicySetup
    Inherits AgoraLegacy.AppBaseClass
    Private iMinLen As Integer
    Protected WithEvents lbl_result As System.Web.UI.WebControls.Label
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Dim objDb As New  EAD.DBCom
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtSearch As System.Web.UI.WebControls.TextBox
    Protected WithEvents imbSearch As System.Web.UI.WebControls.ImageButton
    Protected WithEvents cmbGroup As System.Web.UI.WebControls.DropDownList
    Protected WithEvents dgPolicy As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents ddl As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblMsg2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
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
        'Put user code to initialize the page here
        blnSorting = False
        blnPaging = False
        SetGridProperty(dgPolicy)
        MyBase.Page_Load(sender, e)
        If Not Page.IsPostBack Then
            Bindgrid()
        End If

    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String

        Dim objPolicy As New LoginPolicy

        '//Retrieve Data from Database
        Dim ds As DataSet = New DataSet
        ds = objPolicy.SearchPolicy()

        '//for sorting asc or desc
        Dim dvPolicy As DataView
        dvPolicy = ds.Tables(0).DefaultView
        'dvViewSample(0)(0)
        'If pSorted Then
        dvPolicy.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" Then dvPolicy.Sort += " DESC"
        'End If

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If viewstate("action") = "del" Then
            If dgPolicy.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dgPolicy.PageSize = 0 Then
                dgPolicy.CurrentPageIndex = dgPolicy.CurrentPageIndex - 1
                viewstate("action") = ""
            End If
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count
        '//bind datagrid

        '//datagrid.pageCount only got value after databind

        If intPageRecordCnt > 0 Then
            'intTotPage = dtgDept.PageCount
            cmdSave.Enabled = True
            '//mean Enable, can't use button.Enabled because this is a HTML button
            cmdReset.Disabled = False
            dgPolicy.DataSource = dvPolicy
            dgPolicy.DataBind()
        Else
            'dtgDept.DataSource = ""
            cmdSave.Enabled = False
            cmdReset.Disabled = True
            dgPolicy.DataBind()
            'Common.NetMsgbox(Me, MsgNoRecord)
            'intTotPage = 0
        End If
        'ShowStats()
    End Function


    Private Function ValidateFields(ByRef pMsg As String, ByVal pVal As String, ByVal pPolicy As String) As Boolean
        If pVal = "" Then
            pMsg = "<LI type=square>" & pPolicy & "is required."
            Return False
        End If

        If pPolicy = "PASSWORD_LENGTH_MIN" Then
            iMinLen = pVal
        ElseIf pPolicy = "PASSWORD_LENGTH_MAX" Then
            If iMinLen >= pVal Then
                pMsg = "<LI type=square>" & pPolicy & " must greater than password min length."
                Return False
            End If
        End If

        If pPolicy <> "DEFAULT_PASSWORD" AndAlso pPolicy <> "NEW_LOGIN" Then
            If Not IsNumeric(pVal) Then
                pMsg = "<LI type=square>" & pPolicy & " is over limit/expecting numeric value."
                Return False
            ElseIf CDbl(pVal) <= 0 Then
                pMsg = "<LI type=square>" & pPolicy & " must greater than 0."
                Return False
            End If
        End If

        If pPolicy = "CONTRACT_CATALOGUE_PERIOD_EXPIRATION" Then
            If Not IsNumeric(pVal) Then
                pMsg = "<LI type=square>" & pPolicy & " is over limit/expecting numeric value."
                Return False
            ElseIf CDbl(pVal) <= 0 Then
                pMsg = "<LI type=square>" & pPolicy & " must greater than 0."
                Return False
            End If
        End If
        Return True
    End Function

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim sPolicy, strSQL, strValue, sInd As String
        Dim item As DataGridItem
        Dim objLP As New LoginPolicy
        Dim sErrMsg, pQuery(0) As String

        For Each item In dgPolicy.Items
            sPolicy = CType(item, DataGridItem).Cells(1).Text
            strValue = CType(CType(item, DataGridItem).Cells(6).Controls(1), TextBox).Text

            If ValidateFields(sErrMsg, strValue, sPolicy) Then
                Dim cbo As DropDownList
                cbo = CType(CType(item, DataGridItem).Cells(5).Controls(1), DropDownList)
                sInd = IIf(cbo.SelectedIndex = 0, "YES", "NO")
                If objLP.UpdatePolicy(sPolicy, sInd, strValue) = False Then
                    Exit For
                End If
            Else
                lbl_result.Text = sErrMsg
                Exit Sub
            End If

        Next
        Common.NetMsgbox(Me, objLP.Message, MsgBoxStyle.Information)
        Bindgrid()

        lbl_result.Text = ""
    End Sub

    Private Sub dgPolicy_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgPolicy.ItemCreated
        Grid_ItemCreated(dgPolicy, e)
    End Sub


    Private Sub dgPolicy_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgPolicy.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            CType(e.Item.Cells(6).Controls(1), TextBox).Text = e.Item.Cells(4).Text
            CType(e.Item.Cells(6).Controls(1), TextBox).Text = e.Item.Cells(4).Text
            CType(e.Item.Cells(6).Controls(1), TextBox).MaxLength = 5
            If e.Item.Cells(4).Text = "N/A" Then
                CType(e.Item.Cells(6).Controls(1), TextBox).Enabled = False
            End If

            If e.Item.Cells(3).Text = "YES" Or e.Item.Cells(3).Text = "NO" Then
                Dim i As Integer = IIf(e.Item.Cells(3).Text = "YES", 0, 1)
                CType(e.Item.Cells(5).Controls(1), DropDownList).SelectedIndex = i
            ElseIf e.Item.Cells(3).Text = "FIXED" Then
                'FIXED mean force to YES
                CType(e.Item.Cells(5).Controls(1), DropDownList).SelectedIndex = 0
                CType(e.Item.Cells(5).Controls(1), DropDownList).Enabled = False
            Else
                e.Item.Cells(5).Text = e.Item.Cells(3).Text
            End If
        End If
    End Sub
End Class
