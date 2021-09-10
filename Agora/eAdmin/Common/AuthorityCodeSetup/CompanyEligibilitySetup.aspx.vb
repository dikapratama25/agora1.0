'*************************************************************************************
'Created By:  Louise
'Date:  17/05/2005
'Screen:  Company Eligibility Setup
'Purpose:  Company eligibility setup for RFP Vendor - RFP Admin 

'**************************************************************************************

Imports ERFP.Components
Imports AgoraLegacy


Public Class CompanyEligibilitySetup
    Inherits AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtVendorName As System.Web.UI.WebControls.TextBox
    Protected WithEvents cboRegAutho As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboClassification As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboGrade As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblCoyEligibility As System.Web.UI.WebControls.Label
    Protected WithEvents dtgCoyEligibility As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents cmdModify As System.Web.UI.WebControls.Button
    Protected WithEvents cmdCancel As System.Web.UI.WebControls.Button
    Protected WithEvents cmdReset As System.Web.UI.WebControls.Button
    Protected WithEvents cmdUpdate As System.Web.UI.WebControls.Button
    Protected WithEvents cmdClear As System.Web.UI.WebControls.Button
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Dim objinv As New invClass
    'Dim objdb As New  EAD.DBCom
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents vldsumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents rfv_cboRegAutho As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rfv_Classi As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rfv_Grade As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents SS As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents UU As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents trCoyEligibility As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
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
        Dim d_set As New DataSet

        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgCoyEligibility)

        If Not Page.IsPostBack Then
            SS.Visible = True
            UU.Visible = False

            txtVendorName.Text = Request.QueryString("coyName")
            Bindgrid()
            Common.FillDefault(cboRegAutho, "code_mstr", "code_desc", "code_abbr", "", "code_category ='RA'")
            cboClassification.Items.Insert(0, New ListItem("--- Select ---", ""))
            cboGrade.Items.Insert(0, New ListItem("--- Select ---", ""))

            Dim lstItem As New ListItem
            lstItem.Value = ""
            lstItem.Text = "--- Select ---"
            cboRegAutho.Items.Insert(0, lstItem)

            cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
            cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
        End If

    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String

        Dim ds As DataSet = New DataSet
        Dim record As Integer
        ds = objinv.getVendorEligibility(Request.QueryString("coyID"))
        record = ds.Tables(0).Rows.Count

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        'dvViewSample(0)(0)
        'If pSorted Then
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"
        'End If

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If ViewState("action") = "del" Then
            If dtgCoyEligibility.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgCoyEligibility.PageSize = 0 Then
                dtgCoyEligibility.CurrentPageIndex = dtgCoyEligibility.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If

        ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count
        '//bind datagrid

        '//datagrid.pageCount only got value after databind
        If ViewState("intPageRecordCnt") > 0 Then

            ' check when user re-enter search criteria and click on other page without click search button
            If dtgCoyEligibility.CurrentPageIndex > (dvViewSample.Count \ dtgCoyEligibility.PageSize) Then
                dtgCoyEligibility.CurrentPageIndex = IIf((dvViewSample.Count \ dtgCoyEligibility.PageSize) = 1, 0, (dvViewSample.Count \ dtgCoyEligibility.PageSize))
            ElseIf dtgCoyEligibility.CurrentPageIndex = (dvViewSample.Count \ dtgCoyEligibility.PageSize) Then
                If ViewState("PageCount") = (dvViewSample.Count \ dtgCoyEligibility.PageSize) Then
                    'user does not re-enter search criteria 
                    dtgCoyEligibility.CurrentPageIndex = IIf((dvViewSample.Count \ dtgCoyEligibility.PageSize) = 0, 0, (dvViewSample.Count \ dtgCoyEligibility.PageSize) - 1)
                Else
                    If (dvViewSample.Count Mod dtgCoyEligibility.PageSize) = 0 Then
                        ' total record = 10, 20, ...
                        dtgCoyEligibility.CurrentPageIndex = IIf((dvViewSample.Count \ dtgCoyEligibility.PageSize) = 1, 0, (dvViewSample.Count \ dtgCoyEligibility.PageSize))
                    Else
                        ' total record = 11, 12, ...
                        dtgCoyEligibility.CurrentPageIndex = (dvViewSample.Count \ dtgCoyEligibility.PageSize)
                    End If
                End If
            End If
            '--------------------------------

            dtgCoyEligibility.DataSource = dvViewSample
            dtgCoyEligibility.DataBind()

            lblCoyEligibility.Visible = True
            cmdModify.Visible = True
            cmdDelete.Visible = True
            trCoyEligibility.Visible = True
        Else
            dtgCoyEligibility.DataBind()
            lblCoyEligibility.Visible = False
            cmdModify.Visible = False
            cmdDelete.Visible = False
            trCoyEligibility.Visible = False
        End If

        ' add for above checking
        ViewState("PageCount") = dtgCoyEligibility.PageCount

    End Function

    Public Sub OnPageIndexChanged_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        sender.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        Bindgrid(True)
    End Sub

    Private Sub dtgCoyEligibility_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCoyEligibility.ItemCreated
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgCoyEligibility, e)
    End Sub

    Private Sub dtgCoyEligibility_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCoyEligibility.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            e.Item.Cells(2).Text = Common.parseNull(dv("RQC_REG_AUTHORITY")) & " - " & Common.parseNull(dv("CODE_DESC"))
            e.Item.Cells(3).Text = Common.parseNull(dv("RQC_CLASSIFICATION")) & " - " & Common.parseNull(dv("RC_DESCRIPTION"))
            e.Item.Cells(4).Text = Common.parseNull(dv("RQC_GRADE_ID")) & " - " & Common.parseNull(dv("RG_CAPACITY"))
        End If
    End Sub

    Private Sub cboRegAutho_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboRegAutho.SelectedIndexChanged
        Dim rA As String
        Dim ds_classi As New DataSet
        Dim i As Integer

        rA = cboRegAutho.SelectedItem.Value

        cboClassification.Enabled = True
        cboGrade.Enabled = False

        cboClassification.Items.Clear()
        cboGrade.Items.Clear()

        ds_classi = objinv.getClassification(rA)
        For i = 0 To ds_classi.Tables(0).Rows.Count
            If i = 0 Then
                cboClassification.Items.Insert(i, New ListItem("--- Select ---", ""))
                cboGrade.Items.Insert(0, New ListItem("--- Select ---", ""))
            Else
                cboClassification.Items.Insert(i, New ListItem(ds_classi.Tables(0).Rows(i - 1)("RC_CLASS_ID") & "-" & ds_classi.Tables(0).Rows(i - 1)("RC_DESCRIPTION"), ds_classi.Tables(0).Rows(i - 1)("RC_CLASS_ID")))
            End If
        Next

        If cmdSave.Visible = True Then
            SS.Visible = True
            UU.Visible = False
        Else
            SS.Visible = False
            UU.Visible = True
            cmdUpdate.Visible = True
            cmdReset.Visible = True
            cmdCancel.Visible = True
        End If
    End Sub

    Private Sub cboClassification_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboClassification.SelectedIndexChanged
        Dim cls As String
        Dim ds_grade As New DataSet
        Dim i As Integer
        Dim rA As String

        rA = cboRegAutho.SelectedItem.Value
        cls = cboClassification.SelectedItem.Value

        cboGrade.Enabled = True
        cboGrade.Items.Clear()

        ds_grade = objinv.getGrade(rA)

        For i = 0 To ds_grade.Tables(0).Rows.Count
            If i = 0 Then
                cboGrade.Items.Insert(i, New ListItem("--- Select ---", ""))
            Else
                cboGrade.Items.Insert(i, New ListItem(ds_grade.Tables(0).Rows(i - 1)("RG_GRADE_ID") & "-" & ds_grade.Tables(0).Rows(i - 1)("RG_CAPACITY"), ds_grade.Tables(0).Rows(i - 1)("RG_GRADE_ID")))
            End If
        Next

        If cmdSave.Visible = True Then
            SS.Visible = True
            UU.Visible = False
        Else
            SS.Visible = False
            UU.Visible = True
            cmdUpdate.Visible = True
            cmdReset.Visible = True
            cmdCancel.Visible = True
        End If
    End Sub


    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim objDb As New EAD.DBCom(ConfigurationSettings.AppSettings("eRFPPath"))

        Dim VendorID As String
        Dim saveRA As String
        Dim saveClassi As String
        Dim saveGrade As String
        Dim strtemp As String
        Dim endsql As Boolean

        saveRA = cboRegAutho.SelectedItem.Value
        VendorID = Request.QueryString("coyID")

        If cboClassification.Enabled = False Then
            saveClassi = ""
        Else
            saveClassi = cboClassification.SelectedItem.Value
        End If

        If cboGrade.Enabled = False Then
            saveGrade = ""
        Else
            saveGrade = cboGrade.SelectedItem.Value
        End If

        If saveRA = "" Or saveClassi = "" Or saveGrade = "" Then
            Common.NetMsgbox(Me, "Please select a value for each field.", MsgBoxStyle.Information)
        Else
            strtemp = objinv.saveEligibilitySetup(VendorID, saveRA, saveClassi, saveGrade)
            If strtemp = "1" Then
                Common.NetMsgbox(Me, MsgRecordDuplicate, MsgBoxStyle.Information)
                endsql = True
            Else
                objDb.Execute(strtemp)
                Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
                cboRegAutho.ClearSelection()
                cboClassification.ClearSelection()
                cboGrade.ClearSelection()
                cboClassification.Enabled = False
                cboGrade.Enabled = False
            End If

            Bindgrid()

        End If
        objDb = Nothing

    End Sub

    Private Sub cmdClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        cboRegAutho.ClearSelection()
        cboClassification.ClearSelection()
        cboGrade.ClearSelection()

        cboClassification.Enabled = False
        cboGrade.Enabled = False
    End Sub

    Private Sub cmdModify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdModify.Click

        Dim objDb As New EAD.DBCom(ConfigurationSettings.AppSettings("eRFPPath"))
        Dim strtemp As String
        Dim chk As CheckBox
        Dim dtgitem As DataGridItem
        Dim strAryQuery(0) As String
        Dim i As Integer
        Dim m_rA As String
        Dim rA As String
        Dim m_classi As String
        Dim classi As String
        Dim m_grade As String
        Dim grade As String
        Dim dt As DataTable
        Dim strRA(2) As String
        Dim strClassi(2) As String
        Dim strgrade(2) As String

        For Each dtgitem In dtgCoyEligibility.Items
            chk = dtgitem.FindControl("chkSelection")

            If chk.Checked Then
                m_rA = dtgitem.Cells(2).Text
                strRA = Split(m_rA, "-")
                rA = Trim(strRA(0))

                m_classi = dtgitem.Cells(3).Text
                strClassi = Split(m_classi, "-")
                classi = Trim(strClassi(0))

                m_grade = dtgitem.Cells(4).Text
                strgrade = Split(m_grade, "-")
                grade = Trim(strgrade(0))

            End If
        Next
        Common.FillDefault(Me.cboRegAutho, "code_mstr", "code_desc", "code_abbr", "--- Select ---", "code_category ='RA'")
        Common.SelDdl(rA, cboRegAutho)
        bind_cboClassification(classi, rA)
        bind_cboGrade(classi, rA, grade)
        savetodt()
        cmdSave.Visible = False
        cmdClear.Visible = False
        cmdModify.Enabled = False
        cmdDelete.Enabled = False
        cmdUpdate.Visible = True
        cmdReset.Visible = True
        cmdCancel.Visible = True
        SS.Visible = False
        UU.Visible = True
        objDb = Nothing

    End Sub

    Sub bind_cboClassification(ByVal m_classi As String, ByVal m_rA As String)

        Dim rA As String
        Dim ds_classi As New DataSet
        Dim i As Integer

        rA = m_rA

        If rA = "--- Select ---" Then
            cboClassification.Enabled = False
            cboGrade.Enabled = False
        Else
            cboClassification.Enabled = True
            cboGrade.Enabled = False

            cboClassification.Items.Clear()
            cboGrade.Items.Clear()

            ds_classi = objinv.getClassification(rA)
            For i = 0 To ds_classi.Tables(0).Rows.Count
                If i = 0 Then
                    cboClassification.Items.Insert(i, New ListItem("--- Select ---", ""))
                Else
                    cboClassification.Items.Insert(i, New ListItem(ds_classi.Tables(0).Rows(i - 1)("RC_CLASS_ID") & "-" & ds_classi.Tables(0).Rows(i - 1)("RC_DESCRIPTION"), ds_classi.Tables(0).Rows(i - 1)("RC_CLASS_ID")))
                End If
            Next
        End If

        Common.SelDdl(m_classi, cboClassification)
    End Sub

    Sub bind_cboGrade(ByVal m_classi As String, ByVal m_rA As String, ByVal m_grade As String)

        Dim cls As String
        Dim ds_grade As New DataSet
        Dim i As Integer
        Dim rA As String

        rA = m_rA
        cls = m_classi

        If cls = "" Then
            cboGrade.Enabled = False
        Else
            cboGrade.Enabled = True
            cboGrade.Items.Clear()

            ds_grade = objinv.getGrade(rA)

            For i = 0 To ds_grade.Tables(0).Rows.Count
                If i = 0 Then
                    cboGrade.Items.Insert(i, New ListItem("--- Select ---", ""))
                Else
                    cboGrade.Items.Insert(i, New ListItem(ds_grade.Tables(0).Rows(i - 1)("RG_GRADE_ID") & "-" & ds_grade.Tables(0).Rows(i - 1)("RG_CAPACITY"), ds_grade.Tables(0).Rows(i - 1)("RG_GRADE_ID")))
                End If
            Next
        End If
        Common.SelDdl(m_grade, cboGrade)
    End Sub

    Function getDefaultValue() As DataTable

        Dim test As String
        Dim dbt As New DataTable
        Dim dtr As DataRow
        Dim i As Integer

        '  Dim ds As New DataSet
        dbt.Columns.Add("RQC_VENDORID")
        dbt.Columns.Add("RQC_REGAUTHORITY")
        dbt.Columns.Add("RQC_CLASSI")
        dbt.Columns.Add("RQC_GRADEID")

        dtr = dbt.NewRow()
        dtr("RQC_VENDORID") = Request.QueryString("coyID")
        dtr("RQC_REGAUTHORITY") = Me.cboRegAutho.SelectedItem.Value  'ds.Tables(0).Rows(i) ("RD_CheckBox")
        dtr("RQC_CLASSI") = Me.cboClassification.SelectedItem.Value  'ds.Tables(0).Rows(i) ("RS_RFP_LINE")
        dtr("RQC_GRADEID") = Me.cboGrade.SelectedItem.Value  'ds.Tables(0).Rows(i) ("RS_CONTRACT_TYPE")

        dbt.Rows.Add(dtr)

        Return dbt
    End Function

    Function get_DataFromDt() As DataTable
        Return CType(ViewState("dt"), DataTable)
    End Function

    Function savetodt()
        ViewState("dt") = getDefaultValue()
    End Function

    Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
        Dim objDb As New EAD.DBCom(ConfigurationSettings.AppSettings("eRFPPath"))

        Dim vendorID As String
        Dim u_RA As String
        Dim u_Classi As String
        Dim u_Grade As String
        Dim strtemp As String
        Dim endsql As Boolean
        Dim dt As DataTable

        u_RA = cboRegAutho.SelectedItem.Value
        vendorID = Request.QueryString("coyID")

        If cboClassification.Enabled = False Then
            u_Classi = ""
        Else
            u_Classi = cboClassification.SelectedItem.Value
        End If

        If cboGrade.Visible = False Then
            u_Grade = ""
        Else
            u_Grade = cboGrade.SelectedItem.Value
        End If

        dt = Me.get_DataFromDt()

        strtemp = objinv.updateEligibilitySetup(dt, vendorID, u_RA, u_Classi, u_Grade)
        If strtemp = "1" Then
            Common.NetMsgbox(Me, MsgRecordDuplicate, MsgBoxStyle.Information)
            endsql = True
        Else
            objDb.Execute(strtemp)
            Common.NetMsgbox(Me, "Record Updated.", MsgBoxStyle.Information)
            'cmdReset.Enabled = False
            'savetodt()
            cboRegAutho.ClearSelection()
            cboClassification.ClearSelection()
            cboGrade.ClearSelection()

            cboClassification.Enabled = False
            cboGrade.Enabled = False

            UU.Visible = False
            SS.Visible = True
            cmdSave.Visible = True
            cmdClear.Visible = True
            cmdUpdate.Visible = False
            cmdReset.Visible = False
            cmdCancel.Visible = False
            cmdModify.Enabled = True
            cmdDelete.Enabled = True
        End If

        Bindgrid()
        objDb = Nothing

    End Sub

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        Dim dt As DataTable

        dt = Me.get_DataFromDt()
        Common.FillDefault(Me.cboRegAutho, "code_mstr", "code_desc", "code_abbr", "--- Select ---", "code_category ='RA'")
        Common.SelDdl(dt.Rows(0)(1), cboRegAutho)
        bind_cboClassification(dt.Rows(0)(2), dt.Rows(0)(1))
        bind_cboGrade(dt.Rows(0)(2), dt.Rows(0)(1), dt.Rows(0)(3))

        UU.Visible = True
        SS.Visible = False
        cmdUpdate.Visible = True
        cmdReset.Visible = True
        cmdCancel.Visible = True
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Dim vendorID As String
        Dim vendorName As String

        vendorID = Request.QueryString("coyID")
        vendorName = Request.QueryString("coyName")
        Response.Redirect(dDispatcher.direct("AuthorityCodeSetup", "CompanyEligibilitySetup.aspx", "coyID=" & vendorID & "&coyName=" & vendorName))

    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        'modified the dbaccess connetion for eadmin use
        ' esther 19/08/2005
        Dim objDb As New EAD.DBCom(ConfigurationSettings.AppSettings("eRFPPath"))
        Dim strtemp As String
        Dim chk As CheckBox
        Dim dtgitem As DataGridItem
        Dim strAryQuery(0) As String
        Dim i As Integer
        Dim d_vendorID As String
        Dim d_rA As String
        Dim d_classi As String
        Dim d_grade As String
        Dim rA As String
        Dim classi As String
        Dim grade As String
        Dim strRA(2) As String
        Dim strClassi(2) As String
        Dim strgrade(2) As String

        For Each dtgitem In dtgCoyEligibility.Items
            chk = dtgitem.FindControl("chkSelection")

            d_vendorID = dtgitem.Cells(1).Text

            d_rA = dtgitem.Cells(2).Text
            strRA = Split(d_rA, "-")
            rA = Trim(strRA(0))

            d_classi = dtgitem.Cells(3).Text
            strClassi = Split(d_classi, "-")
            classi = Trim(strClassi(0))

            d_grade = dtgitem.Cells(4).Text
            strgrade = Split(d_grade, "-")
            grade = Trim(strgrade(0))

            If chk.Checked Then
                objinv.deleteCoyEligibilitySetup(d_vendorID, rA, classi, grade)
            End If
        Next

        Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)

        Bindgrid()

        If dtgCoyEligibility.Items.Count = 0 Then
            lblCoyEligibility.Visible = False

            cmdDelete.Visible = False
            cmdModify.Visible = False
            viewstate("blnCmdDelete") = False
            viewstate("blnCmdModify") = False
        End If
        SS.Visible = True
        UU.Visible = False
        objDb = Nothing

    End Sub
End Class
