Imports AgoraLegacy
Imports eProcure.Component

Public Class AddCostAlloc
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    'Protected WithEvents txtCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVendor As System.Web.UI.WebControls.TextBox
    Protected WithEvents hidVendor As System.Web.UI.WebControls.TextBox
    'Protected WithEvents revCode As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    'Protected WithEvents txtDesc As System.Web.UI.WebControls.TextBox
    'Protected WithEvents revDesc As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents txtStartDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents revStartDate As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents txtEndDate As System.Web.UI.WebControls.TextBox
    'Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    'Protected WithEvents cmdClose As System.Web.UI.WebControls.Button
    'Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    'Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents hidDelete As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    'Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    'Protected WithEvents cboBuyer As System.Web.UI.WebControls.DropDownList
    Protected WithEvents revBuyer As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents cvDate As System.Web.UI.WebControls.CompareValidator
    ''Protected WithEvents trStatus As System.Web.UI.HtmlControls.HtmlTableRow
    'Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents revEndDate As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents lblEndDateMsg As System.Web.UI.WebControls.Label
    Protected WithEvents lblCodeLabel As System.Web.UI.WebControls.Label
    'Protected WithEvents trAG As System.Web.UI.HtmlControls.HtmlTableRow
    'Protected WithEvents trCC As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trCode As System.Web.UI.HtmlControls.HtmlTableRow
    'Protected WithEvents trDesc As System.Web.UI.HtmlControls.HtmlTableRow
    'Protected WithEvents cmdItem As System.Web.UI.WebControls.Button
    'Protected WithEvents cmdCompany As System.Web.UI.WebControls.Button
    Protected WithEvents cvDateNow As System.Web.UI.WebControls.CompareValidator
    'Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents lblClear As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    'Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
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

        If Not IsPostBack Then
            ViewState("mode") = Request.QueryString("mode")
            'ViewState("cattype") = Request.QueryString("cattype")
            'cvDateNow.ValueToCompare = Date.Today.ToShortDateString


            If ViewState("mode") = "mod" Then

                lblTitle.Text = "Modify Cost Alloc."
                lblHeader.Text = "Cost Allocation"


                ViewState("index") = Request.QueryString("index")
                ViewState("cacode") = Request.QueryString("cacode")
                txtCode.Enabled = False
                displaySelectedCACode()
            Else

                ViewState("cacode") = Request.QueryString("cacode")

                lblTitle.Text = "Add Cost Alloc."
                lblHeader.Text = "Cost Allocation"
                txtCode.Enabled = True
                Me.txtCode.Focus()
            End If
        End If

    End Sub

    Private Function displaySelectedCACode()
        Dim objCA As New IPP
        Dim ds As New DataSet
        ds = objCA.GetSelectedCACode(ViewState("cacode"))

        If ds.Tables(0).Rows.Count > 0 Then
            'ViewState("oldCode") = Common.parseNull(ds.Tables(0).Rows(0)("CBG_B_GL_CODE"))
            txtIndex.Text = Common.parseNull(ds.Tables(0).Rows(0)("CAM_INDEX"))
            txtCode.Text = Common.parseNull(ds.Tables(0).Rows(0)("CAM_CA_CODE"))
            txtDesc.Text = Common.parseNull(ds.Tables(0).Rows(0)("CAM_CA_DESC"))

        End If
    End Function

    Private Function saveCACode() As Integer
        Dim dtCCCode As New DataTable
        Dim dsCCCode As New DataSet
        Dim objDb As New EAD.DBCom

        dtCCCode.Columns.Add("Index", Type.GetType("System.String"))
        dtCCCode.Columns.Add("CACode", Type.GetType("System.String"))
        dtCCCode.Columns.Add("CADesc", Type.GetType("System.String"))
        dtCCCode.Columns.Add("CompID", Type.GetType("System.String"))
        dtCCCode.Columns.Add("CAUserID", Type.GetType("System.String"))


        Dim dtr As DataRow
        dtr = dtCCCode.NewRow()
        dtr("Index") = ViewState("index") 'txtIndex.Text
        dtr("CACode") = txtCode.Text
        dtr("CADesc") = txtDesc.Text
        dtr("CompID") = Common.Parse(HttpContext.Current.Session("CompanyId"))
        dtr("CAUserID") = Common.Parse(HttpContext.Current.Session("UserId"))


        dtCCCode.Rows.Add(dtr)
        dsCCCode.Tables.Add(dtCCCode)

       

        Dim objCA As New IPP

        Select Case ViewState("mode")
            Case "add"
                'intMsg = objGL.InsertGLCode(dsGLCode)
                If objCA.InsertCostAllocCode(dsCCCode) Then
                    objCA.Message(Me, "00003", MsgBoxStyle.Information)
                    ViewState("mode") = "mod"
                Else
                    objCA.Message(Me, "00002", MsgBoxStyle.Information)
                    ViewState("mode") = "add"
                End If
            Case "mod"
                If objCA.UpdateCostAllocCode(dsCCCode) Then
                    objCA.Message(Me, "00005", MsgBoxStyle.Information)
                    ViewState("mode") = "mod"
                Else
                    objCA.Message(Me, "00018", MsgBoxStyle.Information)
                    ViewState("mode") = "add"
                End If
        End Select

        'hid1.Value = txtCode.Text
        ViewState("index") = objDb.GetVal("select cam_index from cost_alloc_mstr where cam_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' and cam_ca_code = '" & txtCode.Text & "'")
        'saveGLCode = intMsg
    End Function

    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Call saveCACode()
        txtCode.Enabled = False
    End Sub



    Private Sub cmdClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Response.Write("<script language='javascript'> {window.close(); window.opener.reloadPage(); }</script>")
        ViewState("index") = Nothing
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click

        txtCode.Text = ""
        txtDesc.Text = ""
        txtCode.Enabled = True
        ViewState("mode") = "add"
    End Sub
End Class
