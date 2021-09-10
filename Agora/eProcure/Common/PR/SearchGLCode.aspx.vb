' Parameters :
'	id :  the id of the parent's combo box.
Imports AgoraLegacy
Public Class SearchGLCode
	Inherits AgoraLegacy.AppBaseClass

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents txtCode As System.Web.UI.WebControls.TextBox
	Protected WithEvents txtDesc As System.Web.UI.WebControls.TextBox
	Protected WithEvents cmd_search As System.Web.UI.WebControls.Button
	Protected WithEvents trP As System.Web.UI.HtmlControls.HtmlTableRow
	Protected WithEvents cmdSelect As System.Web.UI.HtmlControls.HtmlInputButton
	Protected WithEvents dtgGLCode As System.Web.UI.WebControls.DataGrid
	Protected WithEvents targetID As System.Web.UI.HtmlControls.HtmlInputHidden
	Protected WithEvents tempValue As System.Web.UI.HtmlControls.HtmlInputHidden

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
		MyBase.Page_Load(sender, e)
		If Not Page.IsPostBack Then
			MyBase.SetGridProperty(Me.dtgGLCode)
			Me.targetID.Value = Request.Params("id")
			Page.DataBind()
		End If
	End Sub

	'Search GL Code from database. The result is base on the GL Code and GL Description enter
	'by user. If user enter nothing, all result will returned
	Public Function GetData() As DataView
        Dim objDb As New EAD.DBCom
        Dim companyID As String = Session("CompanyId")

		Dim sql As String = ""

		sql = "SELECT CBG_B_GL_CODE , CBG_B_GL_DESC FROM COMPANY_B_GL_CODE WHERE " & _
		"CBG_B_COY_ID = '" & companyID & "'"

		If Me.txtCode.Text.Length > 0 Then
            sql = sql & " AND CBG_B_GL_CODE " & Common.ParseSQL(Me.txtCode.Text)
		End If

		If Me.txtDesc.Text.Length > 0 Then
            sql = sql & " AND CBG_B_GL_DESC " & Common.ParseSQL(Me.txtDesc.Text)
		End If

		Dim dvViewSample As DataView
		Try
			Dim dt As DataTable = New DataTable
			dt = objDb.FillDt(sql)

			dvViewSample = dt.DefaultView
			dvViewSample.Sort = viewstate("SortExpression")

			intPageRecordCnt = dt.Rows.Count
			viewstate("intPageRecordCnt") = intPageRecordCnt

			If intPageRecordCnt > 0 Then
				Me.cmdSelect.Disabled = False
			Else
				Me.cmdSelect.Disabled = True
			End If

			If viewstate("SortAscending") = "no" Then
				dvViewSample.Sort += " DESC"
			End If

			viewstate("PageCount") = Me.dtgGLCode.PageCount
			If intPageRecordCnt > 0 Then
				resetDatagridPageIndex(dtgGLCode, dvViewSample)
			Else
				resetDatagridPageIndex(dtgGLCode, dvViewSample)
				AgoraLegacy.Common.NetMsgbox(Me, MsgNoRecord)
			End If

            'If Session("Env") = "FTN" Then
            '    Me.dtgGLCode.Columns(1).Visible = False
            'Else
            '    Me.dtgGLCode.Columns(1).Visible = True
            'End If
            Me.dtgGLCode.Columns(1).Visible = True

		Catch ex As Exception
			'process error here
			AgoraLegacy.Common.NetMsgbox(Me, "Error occur during retrieve data, please try again later")
		End Try

		Return dvViewSample
	End Function

	Private Sub dtgGLCode_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgGLCode.PageIndexChanged
		Me.dtgGLCode.CurrentPageIndex = e.NewPageIndex

		Page.DataBind()
	End Sub

	Private Sub dtgGLCode_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgGLCode.ItemDataBound
		Dim glRadioBtn As HtmlInputRadioButton = e.Item.FindControl("radiobtn")
		Dim glCode As String = e.Item.Cells(1).Text

		' "container" is ID of the <span> in  template.
		'22 Oct 2009
		'Due to unknowm reason, if juz adding literal control, htmlradiobutton into the <span>,
		'all the radio buttons will have different name.
		'To solve this issue, direct set the innerHTML of the <span>
		Dim contrl As HtmlGenericControl = e.Item.FindControl("container")
		If Not contrl Is Nothing Then
			'put a radio button here, when the radio button is clicked, it will set the GL Code on the same row
			' into the hidden field.
			contrl.InnerHtml = "<input type='radio' name='catRadio' onclick='setHiddenValue(""" & glCode & """);' />"
		End If
	End Sub

	Private Sub dtgGLCode_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dtgGLCode.SortCommand
		Grid_SortCommand(source, e)
		Page.DataBind()
	End Sub

	Private Sub cmd_search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_search.Click
		Page.DataBind()
	End Sub
End Class

