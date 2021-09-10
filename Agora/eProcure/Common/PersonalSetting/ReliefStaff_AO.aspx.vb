Imports AgoraLegacy
Imports eProcure.Component

Public Class ReliefStaff_AO
    Inherits AgoraLegacy.AppBaseClass
    Dim objDb As New EAD.DBCom
    Protected WithEvents cmdExtend As System.Web.UI.WebControls.Button
    Protected WithEvents txtDatenew As System.Web.UI.WebControls.TextBox
    Dim strstart As String
    Dim strend As String
    Protected WithEvents txtDateFr As System.Web.UI.WebControls.TextBox
    Protected WithEvents rfv_date As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents cmd_canceldiv As System.Web.UI.WebControls.Button
    Protected WithEvents vldDateFr As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldDateTo As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldDateFtDateTo As System.Web.UI.WebControls.CompareValidator
    Dim objpersonal As New PersonalSetting
    Protected WithEvents lbldate As System.Web.UI.WebControls.Label
    Protected WithEvents lblnewdate As System.Web.UI.WebControls.Label
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents txtDateTo As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents cal1 As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents cal2 As System.Web.UI.HtmlControls.HtmlGenericControl
    Dim blnSave As Boolean

    Public Enum EnumAO
        EGrpName
        EAoName
        ERelief
        EAoID
        EGrpIndex
    End Enum


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lbl_add_mod As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents hide As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdcancel As System.Web.UI.WebControls.Button
    Protected WithEvents cmdsave_newdate As System.Web.UI.WebControls.Button
    Protected WithEvents MyDataGrid As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdclear As System.Web.UI.WebControls.Button

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
        'cmdExtend.Enabled = False
        'cmdcancel.Enabled = False
        'cmdSave.Enabled = False
        'Dim alButtonList As ArrayList
        'alButtonList = New ArrayList
        'alButtonList.Add(cmdSave)
        'htPageAccess.Add("update", alButtonList)
        CheckButtonAccess(True)
        'cmdReset.Disabled = Not (blnCanAdd Or blnCanUpdate Or blnCanDelete)
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        rfv_date.Enabled = False

        MyBase.Page_Load(sender, e)
        blnCheckBox = False
        SetGridProperty(MyDataGrid)
        If Not IsPostBack Then
            validation()
            Bindgrid(0)
        End If
        cmdcancel.Attributes.Add("onclick", "return confirm('" & MsgForDeleteButton & "');")
    End Sub
    Function validation()
        blnSave = False
        Dim objAo As New ReliefAOValue
        objpersonal.get_StartEndDate(objAo)
        strstart = objAo.AO_StartDate
        strend = objAo.AO_EndDate
        viewstate("valIndex") = objAo.AO_Index
        cmdExtend.Enabled = False
        cmdcancel.Enabled = False
        cmdSave.Enabled = False
        If strstart <> "" Then
            cal1.Style("display") = "none"
            cal2.Style("display") = "none"
            txtDateFr.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, strstart)
            txtDateTo.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, strend)
            cmdExtend.Enabled = True
            cmdcancel.Enabled = True
            'If Format(CDate(txtDateFr.Text), "yyyy/MM/dd") > Format(Now.Today, "yyyy/MM/dd") Then
            '    cmdExtend.Enabled = True
            '    cmdcancel.Enabled = True
            'ElseIf Format(CDate(txtDateFr.Text), "yyyy/MM/dd") < Format(Now.Today, "yyyy/MM/dd") Then
            '    cmdExtend.Enabled = True
            '    cmdcancel.Enabled = True
            'End If
        Else
            blnSave = True
            cal1.Style("display") = "inline"
            cal2.Style("display") = "inline"
        End If
    End Function


    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String

        Dim ds As New DataSet

        ds = objpersonal.getAO_StaffRelief
        viewstate("intPageRecordCnt") = ds.Tables(0).Rows.Count
        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"
        If viewstate("action") = "del" Then
            If MyDataGrid.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod MyDataGrid.PageSize = 0 Then
                MyDataGrid.CurrentPageIndex = MyDataGrid.CurrentPageIndex - 1
                viewstate("action") = ""
            End If
        End If
        intPageRecordCnt = ds.Tables(0).Rows.Count

        If intPageRecordCnt > 0 Then
            If blnSave Then
                cmdSave.Enabled = True
            End If
            MyDataGrid.DataSource = dvViewSample
            MyDataGrid.DataBind()
        Else
            cmdSave.Enabled = False
            'MyDataGrid.DataSource = dvViewSample
            MyDataGrid.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)

        End If
    End Function

    Private Function isValidDate(ByRef strMsg As String) As Boolean
        isValidDate = True
        If Format(CDate(txtDateTo.Text), "yyyyMMdd") < Format(Date.Now, "yyyyMMdd") Then
            strMsg = "End Date Must >= Today Date"
            isValidDate = False
        ElseIf Format(CDate(txtDateFr.Text), "yyyyMMdd") < Format(Date.Now, "yyyyMMdd") Then
            strMsg = "Start Date Must >= Today Date"
            isValidDate = False
        End If
    End Function

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        ' ai chu remark on 08/09/2005
        'Dim dtReliefstaff As DataTable
        'dtReliefstaff = parseReliefDataGrid(CType(objpersonal.getAO_StaffRelief, DataSet))
        Dim strMsg As String
        If isValidDate(strMsg) = False Then
            lbldate.Visible = True
            lbldate.Text = strMsg
        Else
            lbldate.Visible = False
            'objpersonal.updateReliefStaff(dtReliefstaff, txtDateFr.Text, txtDateTo.Text)
            objpersonal.updateReliefStaff(txtDateFr.Text, txtDateTo.Text)
            Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
            cmdSave.Enabled = False
            Call validation()
        End If

    End Sub

    Private Function parseReliefDataGrid(ByVal pDs As DataSet) As DataTable
        Dim dtReliefstaff As New DataTable
        Dim dr As DataRow
        dtReliefstaff.Columns.Add("AGM_GRP_NAME", Type.GetType("System.String"))
        dtReliefstaff.Columns.Add("AAO_ID", Type.GetType("System.String"))
        dtReliefstaff.Columns.Add("AGA_RELIEF_IND", Type.GetType("System.String"))
        dtReliefstaff.Columns.Add("AGA_GRP_INDEX", Type.GetType("System.Double"))

        'Dim grdItem As DataGridItem
        'For Each grdItem In MyDataGrid.Items

        '    dr = dtReliefstaff.NewRow
        '    dr("AGA_GRP_INDEX") = grdItem.Cells(EnumAO.EGrpIndex).Text
        '    dr("AAO_ID") = grdItem.Cells(EnumAO.EAoID).Text
        '    dr("AGA_RELIEF_IND") = grdItem.Cells(EnumAO.ERelief).Text
        '    dr("AGM_GRP_NAME") = grdItem.Cells(EnumAO.EGrpName).Text
        '    dtReliefstaff.Rows.Add(dr)
        'Next

        Dim drItem As DataRow
        For Each drItem In pDs.Tables(0).Rows

            dr = dtReliefstaff.NewRow

            dr("AGA_GRP_INDEX") = drItem.Item("AGA_GRP_INDEX")
            dr("AAO_ID") = drItem.Item("AAO_ID")
            dr("AGA_RELIEF_IND") = drItem.Item("AGA_RELIEF_IND")
            dr("AGM_GRP_NAME") = drItem.Item("AGM_GRP_NAME")
            dtReliefstaff.Rows.Add(dr)
        Next
        Return dtReliefstaff

    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        Bindgrid(MyDataGrid.CurrentPageIndex, True)
    End Sub

    Private Sub cmdExtend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExtend.Click
        hide.Style("display") = "inline"
        lbl_add_mod.Text = "add"
    End Sub

    Private Sub MyDataGrid_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles MyDataGrid.ItemCreated
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(MyDataGrid, e)
    End Sub
    Public Sub MyData_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        MyDataGrid.CurrentPageIndex = e.NewPageIndex
        Bindgrid(0, True)
    End Sub

    Private Function isValidNewDate(ByRef strMsg As String) As Boolean
        isValidNewDate = True
        If txtDatenew.Text = "" Then
            isValidNewDate = False
            strMsg = "New End Date is required."
        Else
            If Format(CDate(txtDatenew.Text), "yyyyMMdd") < Format(CDate(txtDateTo.Text), "yyyyMMdd") Then
                isValidNewDate = False
                strMsg = "New End Date Must > End Date."
            ElseIf Format(CDate(txtDatenew.Text), "yyyyMMdd") > Format(CDate(txtDateTo.Text), "yyyyMMdd") Then
                isValidNewDate = True
            ElseIf Format(CDate(txtDatenew.Text), "yyyyMMdd") = Format(CDate(txtDateTo.Text), "yyyyMMdd") Then
                isValidNewDate = False
                strMsg = "New End Date Must > End Date."
            End If
        End If
    End Function

    Private Sub cmdsave_newdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdsave_newdate.Click
        Dim strvaldate As String = txtDatenew.Text
        Dim strMsg As String
        If isValidNewDate(strMsg) = False Then
            lblnewdate.Visible = True
            lblnewdate.Text = "<ul type='disc'><li>" & strMsg & "<ul type='disc'></ul></li></ul>"
        Else
            lblnewdate.Visible = False
            hide.Style("display") = "none"
            objpersonal.updateNewReliefDate(txtDatenew.Text)
            txtDateTo.Text = txtDatenew.Text
            txtDatenew.Text = ""
            Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub cmd_canceldiv_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_canceldiv.Click
        hide.Style("display") = "none"
        lblnewdate.Visible = False
    End Sub

    Private Sub cmdclear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdclear.Click
        txtDatenew.Text = ""
        lblnewdate.Visible = False
    End Sub

    Private Sub cmdcancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdcancel.Click
        Dim dgStaffRelief As DataGridItem
        Dim i As Integer = 0
        Dim strmsg As String
        Dim valGrp, valAAo, valrelief, strRauIndex, valseq As String

        For Each dgStaffRelief In MyDataGrid.Items
            valAAo = dgStaffRelief.Cells.Item(EnumAO.EAoID).Text
            ' ai chu remark on 08/09/2005
            ' RELIEF_ASSIGNMENT_USER is used for consolidator only
            'objpersonal.DelReliefStaff(viewstate("valIndex"))
            objpersonal.DelReliefAO_MSTR(viewstate("valIndex"))
        Next
        Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
        txtDateTo.Text = ""
        txtDateFr.Text = ""
        Call validation()
        cmdSave.Enabled = True
        lblnewdate.Visible = False
    End Sub

    Private Sub MyDataGrid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles MyDataGrid.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            e.Item.Cells(EnumAO.ERelief).Text = IIf(e.Item.Cells(EnumAO.ERelief).Text = "C", "Controlled", "Open")
        End If
    End Sub
End Class
