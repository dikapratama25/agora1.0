Imports AgoraLegacy
Imports eProcure.Component

Public Class AddGLCode
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents txtCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVendor As System.Web.UI.WebControls.TextBox
    Protected WithEvents hidVendor As System.Web.UI.WebControls.TextBox
    Protected WithEvents revCode As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents txtDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents revDesc As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents txtStartDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents revStartDate As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents txtEndDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdClose As System.Web.UI.WebControls.Button
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    'Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents hidDelete As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    'Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    'Protected WithEvents cboBuyer As System.Web.UI.WebControls.DropDownList
    Protected WithEvents revBuyer As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents cvDate As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents trStatus As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents revEndDate As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents lblEndDateMsg As System.Web.UI.WebControls.Label
    Protected WithEvents lblCodeLabel As System.Web.UI.WebControls.Label
    Protected WithEvents trAG As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trCC As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trCode As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trDesc As System.Web.UI.HtmlControls.HtmlTableRow
    'Protected WithEvents cmdItem As System.Web.UI.WebControls.Button
    'Protected WithEvents cmdCompany As System.Web.UI.WebControls.Button
    Protected WithEvents cvDateNow As System.Web.UI.WebControls.CompareValidator
    'Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents lblClear As System.Web.UI.WebControls.Label
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region



    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        strNewCSS = "true"
        MyBase.Page_Load(sender, e)

        If Not IsPostBack Then
            ViewState("mode") = Request.QueryString("mode")
            'ViewState("cattype") = Request.QueryString("cattype")
            'cvDateNow.ValueToCompare = Date.Today.ToShortDateString


            If ViewState("mode") = "mod" Then
            
                lblTitle.Text = "Modify GL Code"
                lblHeader.Text = "GL Code"
                txtCode.Enabled = False


                ViewState("index") = Request.QueryString("index")
                displaySelectedGLCode()
            Else
                lblTitle.Text = "Add GL Code"
                lblHeader.Text = "GL Code"
                txtCode.Enabled = True

                Me.txtCode.Focus()
            End If
        End If
      
    End Sub

    Private Function displaySelectedGLCode()
        Dim objGL As New IPP
        Dim ds As New DataSet
        ds = objGL.getSelectedGLCode(ViewState("index"))

        If ds.Tables(0).Rows.Count > 0 Then
            'ViewState("oldCode") = Common.parseNull(ds.Tables(0).Rows(0)("CBG_B_GL_CODE"))
            txtCode.Text = Common.parseNull(ds.Tables(0).Rows(0)("CBG_B_GL_CODE"))
            txtDesc.Text = Common.parseNull(ds.Tables(0).Rows(0)("CBG_B_GL_DESC"))
            If Common.parseNull(ds.Tables(0).Rows(0)("CBG_CC_REQ")) = "Y" Then
                rdbCC.SelectedIndex = 1
            Else
                rdbCC.SelectedIndex = 0
            End If
            If Common.parseNull(ds.Tables(0).Rows(0)("CBG_AG_REQ")) = "Y" Then
                rdbAG.SelectedIndex = 1
            Else
                rdbAG.SelectedIndex = 0
            End If
            If Common.parseNull(ds.Tables(0).Rows(0)("CBG_STATUS")) = "A" Then
                rdbStatus.SelectedIndex = 0
            Else
                rdbStatus.SelectedIndex = 1
            End If


        End If
    End Function

    Private Function saveGLCode() As Integer
        Dim dtGLCode As New DataTable
        Dim dsGLCode As New DataSet

        dtGLCode.Columns.Add("CompID", Type.GetType("System.String"))
        dtGLCode.Columns.Add("GLCode", Type.GetType("System.String"))
        dtGLCode.Columns.Add("GLDesc", Type.GetType("System.String"))
        dtGLCode.Columns.Add("GLCCReq", Type.GetType("System.String"))
        dtGLCode.Columns.Add("GLAGReq", Type.GetType("System.String"))
        dtGLCode.Columns.Add("GLStatus", Type.GetType("System.String"))
        If ViewState("mode") = "add" Then
            dtGLCode.Columns.Add("GLEntBy", Type.GetType("System.String"))
            dtGLCode.Columns.Add("GLEntDate", Type.GetType("System.DateTime"))
        End If
        If ViewState("mode") = "mod" Then
            dtGLCode.Columns.Add("GLModBy", Type.GetType("System.String"))
            dtGLCode.Columns.Add("GLModDate", Type.GetType("System.DateTime"))
        End If


        Dim dtr As DataRow
        dtr = dtGLCode.NewRow()
        dtr("CompID") = Common.Parse(HttpContext.Current.Session("CompanyId"))
        dtr("GLCode") = txtCode.Text
        dtr("GLDesc") = txtDesc.Text
        dtr("GLCCReq") = rdbCC.SelectedItem.Value
        dtr("GLAGReq") = rdbAG.SelectedItem.Value
        dtr("GLStatus") = rdbStatus.SelectedItem.Value
        If ViewState("mode") = "add" Then
            dtr("GLEntBy") = Common.Parse(HttpContext.Current.Session("UserId"))
            dtr("GLEntDate") = DateTime.Now
        End If
        If ViewState("mode") = "mod" Then
            dtr("GLModBy") = Common.Parse(HttpContext.Current.Session("UserId"))
            dtr("GLModDate") = DateTime.Now
        End If

        dtGLCode.Rows.Add(dtr)
        dsGLCode.Tables.Add(dtGLCode)

        Dim objGL As New IPP
        Dim intMsg As Integer
        Select Case ViewState("mode")
            Case "add"
                'intMsg = objGL.InsertGLCode(dsGLCode)
                If objGL.InsertGLCode(dsGLCode) Then
                    objGL.Message(Me, "00003", MsgBoxStyle.Information)
                Else
                    objGL.Message(Me, "00002", MsgBoxStyle.Information)
                End If
            Case "mod"
                If objGL.UpdateGLCode(dsGLCode) Then
                    objGL.Message(Me, "00005", MsgBoxStyle.Information)
                Else
                    objGL.Message(Me, "00018", MsgBoxStyle.Information)
                End If
        End Select
        'saveGLCode = intMsg
    End Function

    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        'Dim intMsg As Integer
        Call saveGLCode()
        txtCode.Enabled = False
        'Select Case intMsg   

        '    Case WheelMsgNum.NotSave
        '        Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
        '    Case WheelMsgNum.Duplicate
        '        Common.NetMsgbox(Me, MsgRecordDuplicate, MsgBoxStyle.Information)
        'End Select
    End Sub

    

    Private Sub cmdClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Response.Write("<script language='javascript'> {window.close(); window.opener.reloadPage(); }</script>")
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        txtCode.Text = ""
        txtDesc.Text = ""
        rdbCC.SelectedIndex = 0
        rdbAG.SelectedIndex = 0
        rdbStatus.SelectedIndex = 0
        txtCode.Enabled = True
        ViewState("mode") = "add"
    End Sub    
End Class
