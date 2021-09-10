Imports AgoraLegacy
Imports eProcure.Component


Public Class IPPAnalysisCodeMatrix
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
    Protected WithEvents txtGLCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAddGLCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents cbxAnalysisType As System.Web.UI.WebControls.CheckBoxList
    Protected WithEvents Hide_Add2 As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents hidMode As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lbl_add_mod As System.Web.UI.WebControls.Label
    Protected WithEvents validateGLCode As System.Web.UI.WebControls.RequiredFieldValidator

    Dim ds As DataSet
    Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID")


    Enum EnumType
        icChk
        icGLCode
        icCodeAnalysis1
        icCodeAnalysis2
        icCodeAnalysis3
        icCodeAnalysis4
        icCodeAnalysis5
        icCodeAnalysis6
        icCodeAnalysis7
        icCodeAnalysis8
        icCodeAnalysis9
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
            fillCheckBoxList()
            cmdSearch.Enabled = True
            cmdClear1.Enabled = True
            cmdModify.Enabled = False
            cmdDelete.Enabled = False
            Hide_Add2.Style("display") = "none"
        End If

        'cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
        cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
        PopulateVenTypeAhead()

    End Sub

    Sub fillCheckBoxList()
        Dim cbolist As New ListItem
        Dim cbolist2 As New ListItem

        objGlobal.FillAnalysisCodeType(cbxAnalysisType, Session("CompanyID"))

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

        ds = objIPPMAin.getGLAnalysisCode(txtGLCode.Text)

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

            For I As Integer = 1 To 9
                If dv("cbgcac_analysis_code" & I) = "P" Then
                    chk = e.Item.Cells(EnumType.icCodeAnalysis9).FindControl("chkAnalysisCode" & I)
                    chk.Checked = False
                ElseIf dv("cbgcac_analysis_code" & I) = "M" Then
                    chk = e.Item.Cells(EnumType.icCodeAnalysis9).FindControl("chkAnalysisCode" & I)
                    chk.Checked = True
                End If

            Next

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
        validateGLCode.Enabled = True
        txtAddGLCode.Text = ""
        Me.lbl_add_mod.Text = "add"
        cmdClear.Text = "Clear"
        hidMode.Value = "a"
        hidIndex.Value = ""

    End Sub

    Private Sub cmdClear1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClear1.Click
        txtGLCode.Text = ""
    End Sub

    Private Sub cmd_clear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClear.Click

        If hidMode.Value = "a" Then
            txtAddGLCode.Text = ""
            fillCheckBoxList()
        End If

    End Sub

    Private Sub cmd_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        If hidMode.Value = "a" Then
            Me.view("add")
        ElseIf hidMode.Value = "m" Then
            Me.view("Mod")
        End If

    End Sub

    Sub view(ByVal selected As String)
        Dim intmsgno As Integer

        If chkGLCode() Then
            If selected = "add" Then
                Me.lbl_add_mod.Text = "add"
                intmsgno = objIPPMAin.addGLCodeMatrix(txtAddGLCode.Text)
            End If

            Select Case intmsgno
                Case WheelMsgNum.Save

                    'update analysis code columns
                    For i As Integer = 0 To cbxAnalysisType.Items.Count - 1
                        updateCodeAnalysisStatus(cbxAnalysisType.Items(i).Selected, i)
                    Next

                    Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00003"), MsgBoxStyle.Information)
                    txtGLCode.Text = ""
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
        End If

    End Sub

    Private Sub cmdModify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdModify.Click
        Dim dgItem As DataGridItem
        Dim chk As CheckBox

        validateGLCode.Enabled = True
        Me.lbl_add_mod.Text = "modify"
        cmdClear.Text = "Reset"
        hidMode.Value = "m"
        For Each dgItem In dtgTax.Items
            'chk = dgItem.FindControl("chkSelection")

            For i As Integer = 0 To cbxAnalysisType.Items.Count - 1
                chk = dgItem.FindControl("chkAnalysisCode" & i + 1)
                objIPPMAin.updateCodeAnalysisStatus(chk.Checked, i, dgItem.Cells(EnumType.icGLCode).Text)
            Next

        Next

        Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00003"), MsgBoxStyle.Information)

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
                intMsg = objIPPMAin.deleteGLCode(dgItem.Cells(EnumType.icGLCode).Text)
            End If
        Next

        Dim objAdmin As New Admin
        If intMsg = -99 Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00009") & " " & objGlobal.GetErrorMessage("00018"), MsgBoxStyle.Information)
        ElseIf intMsg = WheelMsgNum.Delete Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00004"), MsgBoxStyle.Information)
            txtGLCode.Text = ""
            Hide_Add2.Style("display") = "none"
            hidMode.Value = ""
            Bindgrid()
        ElseIf intMsg = WheelMsgNum.NotDelete Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00008"), MsgBoxStyle.Information)
        End If
    End Sub

    Private Function chkGLCode() As Boolean

        Dim strChkCompId As String

        If strDefIPPCompID <> "" Then
            strChkCompId = strDefIPPCompID
        Else
            strChkCompId = Session("CompanyId")
        End If
        '---------------------------------

        If txtAddGLCode.Text <> "" Then
            If objDb.Exist("SELECT '*' FROM COMPANY_B_GL_CODE WHERE CBG_B_COY_ID = '" & strChkCompId & "' AND CBG_STATUS = 'A' AND CBG_B_GL_CODE = '" & Common.Parse(txtAddGLCode.Text) & "'") = 0 Then
                    chkGLCode = False
            ElseIf objDb.Exist("SELECT '*' FROM company_b_gl_code_analysis_code WHERE CBGCAC_B_COY_ID = '" & strChkCompId & "' AND CBGCAC_B_GL_CODE = '" & Common.Parse(txtAddGLCode.Text) & "'") <> 0 Then
                chkGLCode = False
                validateGLCode.Enabled = True
                validateGLCode.Text = "GL Code already exist."
            Else
                chkGLCode = True
            End If
        End If

    End Function

    Sub PopulateVenTypeAhead()

        Dim ventypeahead As String
        Dim i, count As Integer
        Dim vencontent, content2 As String
        Dim strCompID As String
        Dim vtypeahead As String

        Dim gltypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=PAMBGLCode")


        vencontent &= "$(""#txtAddGLCode"").autocomplete(""" & gltypeahead & "&compid=" & Session("CompanyID") & """, {" & vbCrLf &
        "width: 200," & vbCrLf &
        "scroll: true," & vbCrLf &
        "selectFirst: false" & vbCrLf &
        "});" & vbCrLf &
        "var vendIdx = document.getElementById(""txtAddGLCode"").value;" & vbCrLf &
        "if(vendIdx == """")" & vbCrLf &
        "{" & vbCrLf &
        "};" & vbCrLf &
        "$(""#txtAddGLCode"").result(function(event,data,item) {" & vbCrLf &
        "if (data)" & vbCrLf &
        "$(""#txtAddGLCode"").val(data[1].split("":"")[0]);" & vbCrLf &
        "});" & vbCrLf &
        "$(""#txtAddGLCode"").blur(function() {" & vbCrLf &
        "});" & vbCrLf


        ' for edit purpose

        If Session("Action") = "Edit" Then
            ventypeahead = "<script language=""javascript"">" & vbCrLf &
                      "<!--" & vbCrLf &
                        "$(document).ready(function(){" & vbCrLf &
                        vencontent & vbCrLf &
                        "});" & vbCrLf &
                        "-->" & vbCrLf &
                        "</script>"
        Else
            ventypeahead = "<script language=""javascript"">" & vbCrLf &
          "<!--" & vbCrLf &
            "$(document).ready(function(){" & vbCrLf &
            vencontent & vbCrLf &
            "});" & vbCrLf &
            "-->" & vbCrLf &
            "</script>"
        End If

        Session("ventypeahead") = ventypeahead

    End Sub

    Public Sub updateCodeAnalysisStatus(ByVal selected As Boolean, ByVal index As Integer)
        'update analysis code columns
        objIPPMAin.updateCodeAnalysisStatus(selected, index, txtAddGLCode.Text)
    End Sub

    Protected Sub chkAll_CheckedChanged(sender As Object, e As EventArgs)
        Dim chkSelHeader As CheckBox
        chkSelHeader = sender
        If chkSelHeader.Checked Then
            For Each dgItem As DataGridItem In dtgTax.Items
                Dim chkSelection As CheckBox = dgItem.Cells(0).FindControl("chkSelection")
                chkSelection.Checked = True
            Next
        Else
            For Each dgItem As DataGridItem In dtgTax.Items
                Dim chkSelection As CheckBox = dgItem.Cells(0).FindControl("chkSelection")
                chkSelection.Checked = False
            Next
        End If
    End Sub
End Class
