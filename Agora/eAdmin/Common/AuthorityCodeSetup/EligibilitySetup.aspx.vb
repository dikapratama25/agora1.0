'*************************************************************************************
'Created By:  Louise
'Date:  17/05/2005
'Screen:  Eligibility Setup
'Purpose:  Eligibility setup for RFP Vendor - RFP Admin 

'**************************************************************************************

Imports ERFP.Components
Imports AgoraLegacy

Public Class EligiblitySetup
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtVendorName As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdClear As System.Web.UI.WebControls.Button
    Protected WithEvents btnSearch As System.Web.UI.WebControls.Button
    Protected WithEvents lblListOfVendors As System.Web.UI.WebControls.Label
    Protected WithEvents dtgVendors As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Reset1 As System.Web.UI.HtmlControls.HtmlInputButton
    Dim objinv As New invClass
    Protected WithEvents cmdEligibilitySetup As System.Web.UI.WebControls.Button
    Protected WithEvents LV As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents LVlbl As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents ES As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents dtg As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents EE As System.Web.UI.HtmlControls.HtmlTableCell

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
        SetGridProperty(dtgVendors)

        If Not Page.IsPostBack Then
            Reset1.Visible = False
            cmdEligibilitySetup.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
            LV.Visible = False
            LVlbl.Visible = False
            ES.Visible = False
            dtg.Visible = False
            EE.Visible = False
        End If


    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String

        Dim ds As DataSet = New DataSet
        Dim record As Integer
        ds = objinv.getCoyNameEligibility(txtVendorName.Text)
        record = ds.Tables(0).Rows.Count

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = viewstate("SortExpression")

        If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"
        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If viewstate("action") = "del" Then
            If dtgVendors.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgVendors.PageSize = 0 Then
                dtgVendors.CurrentPageIndex = dtgVendors.CurrentPageIndex - 1
                viewstate("action") = ""
            End If
        End If

        viewstate("intPageRecordCnt") = ds.Tables(0).Rows.Count
        '//bind datagrid

        '//datagrid.pageCount only got value after databind
        If viewstate("intPageRecordCnt") > 0 Then

            ' check when user re-enter search criteria and click on other page without click search button
            If dtgVendors.CurrentPageIndex > (dvViewSample.Count \ dtgVendors.PageSize) Then
                dtgVendors.CurrentPageIndex = IIf((dvViewSample.Count \ dtgVendors.PageSize) = 1, 0, (dvViewSample.Count \ dtgVendors.PageSize))
            ElseIf dtgVendors.CurrentPageIndex = (dvViewSample.Count \ dtgVendors.PageSize) Then
                If viewstate("PageCount") = (dvViewSample.Count \ dtgVendors.PageSize) Then
                    'user does not re-enter search criteria 
                    dtgVendors.CurrentPageIndex = IIf((dvViewSample.Count \ dtgVendors.PageSize) = 0, 0, (dvViewSample.Count \ dtgVendors.PageSize) - 1)
                Else
                    If (dvViewSample.Count Mod dtgVendors.PageSize) = 0 Then
                        ' total record = 10, 20, ...
                        dtgVendors.CurrentPageIndex = IIf((dvViewSample.Count \ dtgVendors.PageSize) = 1, 0, (dvViewSample.Count \ dtgVendors.PageSize))
                    Else
                        ' total record = 11, 12, ...
                        dtgVendors.CurrentPageIndex = (dvViewSample.Count \ dtgVendors.PageSize)
                    End If
                End If
            End If
            '--------------------------------

            dtgVendors.DataSource = dvViewSample
            dtgVendors.DataBind()

            lblListOfVendors.Visible = True
            cmdEligibilitySetup.Visible = True
            Reset1.Visible = True
            dtg.Visible = True
            LV.Visible = True
            LVlbl.Visible = True
            ES.Visible = True
            EE.Visible = True
        Else
            dtgVendors.DataBind()
            lblListOfVendors.Visible = False
            cmdEligibilitySetup.Visible = False
            Reset1.Visible = False
            dtg.Visible = False
            LV.Visible = False
            LVlbl.Visible = False
            ES.Visible = False
            EE.Visible = False
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        ' add for above checking
        viewstate("PageCount") = dtgVendors.PageCount
        objinv = Nothing

    End Function

    Public Sub OnPageIndexChanged_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        sender.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        Bindgrid(True)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Bindgrid()
    End Sub

    Private Sub dtgVendors_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgVendors.ItemCreated
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(dtgVendors, e)
    End Sub

    Private Sub dtgVendors_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgVendors.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
        End If
    End Sub

    Private Sub cmdClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        txtVendorName.Text = ""
    End Sub

    Private Sub cmdEligibilitySetup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEligibilitySetup.Click
        'Dim objDb As New  EAD.DBCom(ConfigurationSettings.AppSettings("eRFPPath"))
        Dim strtemp As String
        Dim chk As CheckBox
        Dim dtgitem As DataGridItem
        'Dim strAryQuery(0) As String
        Dim i As Integer
        Dim e_VendorName As String
        Dim coyID As String
        Dim dt As DataTable

        For Each dtgitem In dtgVendors.Items
            chk = dtgitem.FindControl("chkSelection")

            If chk.Checked Then
                e_VendorName = dtgitem.Cells(1).Text
                coyID = dtgVendors.DataKeys.Item(i)
            End If
            i = i + 1
        Next

        Response.Redirect(dDispatcher.direct("AuthorityCodeSetup", "CompanyEligibilitySetup.aspx", "coyID=" & coyID & "&coyName=" & e_VendorName))
        'objDb = Nothing

    End Sub
End Class
