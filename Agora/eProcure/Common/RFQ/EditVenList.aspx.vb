Imports AgoraLegacy
Imports eProcure.Component

Public Class EditVenList
    Inherits AgoraLegacy.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lbl_List_Name As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_name As System.Web.UI.WebControls.Label
    Protected WithEvents dtg_vendor As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmd_exit As System.Web.UI.WebControls.Button
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents cmd_Remove As System.Web.UI.WebControls.Button

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Public Enum ListEnum
        chk = 0
        Coy_Name = 1
        Contact = 2
        Coy_ID = 3
       
    End Enum
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        cmd_Remove.Enabled = False

        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmd_Remove)
        htPageAccess.Add("delete", alButtonList)
        CheckButtonAccess()
        cmd_Remove.Enabled = blnCanDelete

        alButtonList.Clear()
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim objrfq As New RFQ
        MyBase.Page_Load(sender, e)
        SetGridProperty(Me.dtg_vendor)
        Me.lbl_name.Text = Trim(Request("RFQ_Name"))
        Me.lbl_List_Name.Text = Trim(Request("RFQ_list"))
        Me.cmd_exit.Attributes.Add("onclick", "window.close()")
        ' Me.cmd_save.Attributes.Add("onclick", "window.opener.location.href = ""Create_RFQ2.aspx?RFQ_Name=" & lbl_name.Text & "&checkven=1""")
        Dim search As String = Trim(Request("search"))
        If Not Page.IsPostBack Then
            VIEWSTATE("search") = search
            If (search <> "") Then
                Bindgrid(0, True, search)
            End If
        End If



    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False, Optional ByVal search As String = "")

        Dim objrfq As New RFQ
        Dim ds As New DataSet
        ' ds = objrfq.getVendorView(search)
        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewSample.Sort = viewstate("SortExpression")
            If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"
        End If

        If viewstate("action") = "del" Then
            If dtg_vendor.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtg_vendor.PageSize = 0 Then
                dtg_vendor.CurrentPageIndex = dtg_vendor.CurrentPageIndex - 1
                viewstate("action") = ""
            End If
        End If

        dtg_vendor.DataSource = dvViewSample
        dtg_vendor.DataBind()

        '//datagrid.pageCount only got value after databind

    End Function

    Public Sub dtg_vendor_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtg_vendor.CurrentPageIndex = e.NewPageIndex
        Bindgrid(0)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        Bindgrid(dtg_vendor.CurrentPageIndex, True, VIEWSTATE("search"))
    End Sub


    Private Sub dtg_vendor_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_vendor.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim country As String
            Dim state As String
            Dim objrfq As New RFQ

            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(ListEnum.chk).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            Dim lbl_adds2 As Label
            lbl_adds2 = e.Item.FindControl("lbl_adds")

            Dim stradds As String
            If (Not IsDBNull(dv("CM_ADDR_LINE1"))) Or Common.parseNull(dv("CM_ADDR_LINE1")) <> "" Then
                stradds = "" & dv("CM_ADDR_LINE1") & ""
            End If

            If (Not IsDBNull(dv("CM_ADDR_LINE2"))) Or Common.parseNull(dv("CM_ADDR_LINE2")) <> "" Then
                If stradds = "" Then
                    stradds = "" & dv("CM_ADDR_LINE2") & ""
                Else
                    stradds = stradds & "<br>" & dv("CM_ADDR_LINE2") & ""
                End If
            End If

            If (Not IsDBNull(dv("CM_ADDR_LINE3"))) Or Common.parseNull(dv("CM_ADDR_LINE3")) <> "" Then
                If stradds = "" Then
                    stradds = "" & dv("CM_ADDR_LINE3") & ""
                Else
                    stradds = stradds & "<br>" & dv("CM_ADDR_LINE3") & ""
                End If
            End If


            If (Not IsDBNull(dv("CM_POSTCODE"))) Or Common.parseNull(dv("CM_POSTCODE")) <> "" Or (Not IsDBNull(dv("CM_CITY"))) Or Common.parseNull(dv("CM_CITY")) <> "" Then
                If stradds = "" Then
                    stradds = "" & dv("CM_POSTCODE") & " " & dv("CM_CITY")
                Else
                    stradds = stradds & "<br>" & dv("CM_POSTCODE") & " " & dv("CM_CITY")
                End If
            End If

            state = objrfq.get_codemstr(dv("CM_STATE"), "S")

            If state <> "" Then
                If stradds = "" Then
                    stradds = state
                Else
                    stradds = stradds & "<br> " & state
                End If
            End If

            country = objrfq.get_codemstr(dv("CM_COUNTRY"), "CT")
            If country <> "" Then
                If stradds = "" Then
                    stradds = country
                Else
                    stradds = stradds & "<br>" & country
                End If
            End If

            If (Not IsDBNull(dv("CM_EMAIL"))) Or Common.parseNull(dv("CM_EMAIL")) <> "" Then
                If stradds = "" Then
                    stradds = "" & dv("CM_EMAIL") & ""
                Else
                    stradds = stradds & "<br>" & dv("CM_EMAIL") & ""
                End If
            End If

            If (Not IsDBNull(dv("CM_PHONE"))) Or Common.parseNull(dv("CM_PHONE")) <> "" Then
                If stradds = "" Then
                    stradds = "Tel: " & dv("CM_PHONE") & ""
                Else
                    stradds = stradds & "<br>Tel: " & dv("CM_PHONE") & ""
                End If
            End If

            '************************************************************************************
            lbl_adds2.Text = stradds
        End If
    End Sub

    Private Sub dtg_vendor_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_vendor.ItemCreated
        Grid_ItemCreated(dtg_vendor, e)
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Private Sub cmd_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim dgItem As DataGridItem
        Dim objrfq As New RFQ
        Dim objval As New RFQ_User
        Dim objDB As New EAD.DBCom
        Dim strAryQuery(0) As String
        Dim strSQL As String
        Dim txtPrice As TextBox
        Dim lblPrice As Label
        Dim chkItem As CheckBox
        '  rfq_name = Me.Request.QueryString("RFQ_Name")
        Dim cur As String = Request.QueryString("RFQ_Cur_value")
        Dim i As Integer = 0

        objval.dis_ID = "7" 'objrfq.Vendor_AddDistMstr(objval)
        objval.RFQ_Name = Me.lbl_name.Text
        objrfq.Vendor_AddListMstr(objval)
        For Each dgItem In dtg_vendor.Items

            objval.V_com_ID = Me.dtg_vendor.DataKeys.Item(i)
            chkItem = dgItem.FindControl("chkSelection")

            If chkItem.Checked Then

                strSQL = objrfq.Vendor_AddList(objval)
                Common.Insert2Ary(strAryQuery, strSQL)

            End If
            i = i + 1
        Next
        If strSQL = "Record exit!!!" Then
            Common.NetMsgbox(Me, strSQL, MsgBoxStyle.Information)
        Else
            objDB.BatchExecute(strAryQuery)
            Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
        End If

        Bindgrid(0, True, VIEWSTATE("search"))
        ' Me.onchange.Value = "0"
    End Sub

    Private Sub cmd_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_exit.Click

    End Sub

    Private Sub dtg_vendor_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtg_vendor.SelectedIndexChanged

    End Sub
End Class
