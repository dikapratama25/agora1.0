Imports AgoraLegacy
Imports eProcure.Component
Public Class PredefinedVendorList
    Inherits AgoraLegacy.AppBaseClass
    'Protected WithEvents lbl_List_Name As System.Web.UI.WebControls.Label
    'Protected WithEvents lbl_name As System.Web.UI.WebControls.Label
    'Protected WithEvents dt_V_com As System.Web.UI.WebControls.Table
    'Protected WithEvents dtg_vendor As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents CMD_SAVE1 As System.Web.UI.WebControls.Button
    'Protected WithEvents cmd_save As System.Web.UI.WebControls.Button
    'Protected WithEvents cmdRemove As System.Web.UI.WebControls.Button
    'Protected WithEvents lbl_title As System.Web.UI.WebControls.Label
    'Protected WithEvents cmd_exit As System.Web.UI.HtmlControls.HtmlAnchor
    'Protected WithEvents cmdexit_addlist1 As System.Web.UI.HtmlControls.HtmlAnchor

    Dim bindcreaterfq2 As New Create_RFQ

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Public Enum vendorEnum
        Chk = 0
        CoyName = 1
        Contact = 2
        CoyID = 3
    End Enum

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        cmdRemove.Enabled = False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
      
        htPageAccess.Add("add", alButtonList)

        alButtonList = New ArrayList
        alButtonList.Add(cmdRemove)
        htPageAccess.Add("delete", alButtonList)

        CheckButtonAccess()

        If intPageRecordCnt > 0 Then
            cmdRemove.Enabled = blnCanDelete
        Else
            cmdRemove.Enabled = False
        End If

        alButtonList.Clear()
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)
        SetGridProperty(Me.dtg_vendor)

        If Session("RFQ_venlist_num") = "" Then
            Session("RFQ_venlist_num") = Request(Trim("RFQ_venlist_num"))
        Else
            If Session("RFQ_venlist_num") <> Request(Trim("RFQ_venlist_num")) Or Request(Trim("RFQ_venlist_num")) <> "" Or Request(Trim("RFQ_venlist_num")) <> " " Then
                If Not IsNothing(Request(Trim("RFQ_venlist_num"))) Then
                    Session("RFQ_venlist_num") = Request(Trim("RFQ_venlist_num"))
                End If
            End If
        End If

        Dim objrfq As New RFQ

        If Session("edit") = "2" Then
            If Session("list_index") = "" Then
                Session("list_index") = Request(Trim("index_list"))
            Else
                If Session("list_index") <> Request(Trim("index_list")) And Request(Trim("index_list")) <> "" And Request(Trim("index_list")) <> " " Then
                    If Not IsNothing(Request(Trim("index_list"))) Then
                        Session("list_index") = Request(Trim("index_list"))
                    End If
                End If
            End If
        End If

        If Not Page.IsPostBack Then
            Dim search As String = Trim(Request("search"))
            VIEWSTATE("search") = search
            If (search <> "") Then
                Bindgrid(True, search)
            Else
                If Session("edit") = "" Then
                    Dim edit As String = Request(Trim("edit"))
                    Session("edit") = edit
                Else
                    If Session("edit") <> Request(Trim("edit")) Or Request(Trim("edit")) = "" Then
                        Dim edit As String = Request(Trim("edit"))
                        Session("edit") = edit
                    End If
                End If
            End If

            If Session("RFQ_LIST") = "" Then
                Session("RFQ_LIST") = Trim(Request("RFQ_list"))
            Else
                If Session("RFQ_LIST") <> Trim(Request("RFQ_list")) Or Trim(Request("RFQ_list")) <> "" Then
                    Session("RFQ_LIST") = Trim(Request("RFQ_list"))
                End If
            End If

            If Session("RFQ_NAME") = "" Then
                Session("RFQ_NAME") = Trim(Request("RFQ_Name"))
            Else
                If Session("RFQ_NAME") <> Trim(Request("RFQ_Name")) Or Trim(Request("RFQ_Name")) <> "" Then
                    Session("RFQ_NAME") = Trim(Request("RFQ_Name"))
                End If
            End If

            Me.lbl_name.Text = Request.QueryString("VendorListName")
            Me.lbl_List_Name.Text = "Pre-Defined List: " 'Session("RFQ_LIST")

            If Session("edit") <> "2" Then
                'Me.cmd_exit.Attributes.Add("onclick", "window.opener.location.href = ""Create_RFQ2.aspx?RFQ_Name=" & Server.UrlEncode(lbl_name.Text) & "&checkven=1&pageid=" & strPageId & """;window.close();")
            ElseIf Session("edit") = "2" Then
                If Session("list_index") = "" Then
                    Session("list_index") = Request(Trim("index_list"))
                Else
                    If Session("list_index") <> Request(Trim("index_list")) And Request(Trim("index_list")) <> "" And Request(Trim("index_list")) <> " " Then
                        If Not IsNothing(Request(Trim("index_list"))) Then
                            Session("list_index") = Request(Trim("index_list"))
                        End If
                    End If
                End If

            End If

            If Session("edit") = "1" Then ' side gary, edit vendor
                Me.cmdRemove.Visible = True
                Me.lbl_title.Text = "Select Vendor Companies"
                If Not Page.IsPostBack Then
                    Bindgrid(True, VIEWSTATE("search"))
                End If

            End If
        End If
        cmdRemove.Attributes.Add("onclick", "return RemoveItemCheck();")
    End Sub

    'Sub bind_table()
    '    Dim row As Integer
    '    Dim dr As TableRow
    '    Dim objrfq As New RFQ
    '    Dim objval As New RFQ_User
    '    Dim count, a, c, i, j As Integer
    '    Dim value(29) As String

    '    count = 1
    '    dr = New TableRow
    '    objrfq.Vendor_check(value)
    '    c = 0
    '    a = 0
    '    Dim search09 As String

    '    For j = 0 To 26
    '        dr.Cells.Add(New TableCell)
    '        If j = 0 Then
    '            If Not IsNothing(value(c)) Then
    '                Do While Not System.Text.RegularExpressions.Regex.IsMatch(value(c), "[A-Za-z]")

    '                    If search09 = "" Then
    '                        search09 = value(c)
    '                    Else
    '                        search09 = search09 & "," & value(c)
    '                    End If
    '                    c = c + 1
    '                    i = 1
    '                Loop
    '            End If

    '            If i = 1 Then
    '                Dim str As String
    '                str = "VendorList.aspx?pageid=" & strPageId & "&search=" & Server.UrlEncode(search09) & "&RFQ_Name=" & Server.UrlEncode(lbl_name.Text) & "&RFQ_list=" & Server.UrlEncode(lbl_List_Name.Text) & "&checkven=1"
    '                dr.Cells(j).Text = "<A href=" & str & "><font color=#0000ff>0-9</font></A>"
    '            Else
    '                dr.Cells(j).Text = "0-9"
    '            End If
    '        Else
    '            If Chr(65 + (j - 1)) = value(c) Then
    '                dr.Cells(j).Text = "<A href=""VendorList.aspx?pageid=" & strPageId & "&search=" & Chr(65 + (j - 1)) & "&RFQ_Name=" & Server.UrlEncode(lbl_name.Text) & "&RFQ_list=" & lbl_List_Name.Text & "&checkven=1"" ><font color=#0000ff>" & Chr(65 + (j - 1)) & "</font></A>"
    '                c = c + 1
    '            Else
    '                dr.Cells(j).Text = Chr(65 + (j - 1))
    '            End If
    '        End If
    '    Next

    '    dt_V_com.Rows.Add(dr)
    'End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False, Optional ByVal search As String = "")
        Dim objrfq As New RFQ
        Dim ds As New DataSet
        Dim dtDetails As DataTable
        Dim aryVendors As New ArrayList
        Dim dtrDetails As DataRow()
        Dim strSearch As String = ""

        dtDetails = Session("VendorListDetails")
        strSearch = "RVDLM_List_Index='" & Session("RFQ_venlist_num") & "' AND TYPE = 'list'"
        dtrDetails = dtDetails.Select(strSearch)
        If dtrDetails.Length > 0 Then
            For Each oRow As DataRow In dtrDetails
                aryVendors.Add(oRow.Item("CoyId"))
            Next
        End If

        ds = objrfq.getVendorDetails(aryVendors)
        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        intPageRecordCnt = ds.Tables(0).Rows.Count
        If pSorted Then
            dvViewSample.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"
        End If

        If ViewState("action") = "del" Then
            If dtg_vendor.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtg_vendor.PageSize = 0 Then
                dtg_vendor.CurrentPageIndex = dtg_vendor.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If

        dtg_vendor.DataSource = dvViewSample
        dtg_vendor.DataBind()

        If ViewState("edit") = "0" Then
        ElseIf ViewState("edit") = "1" Then
            Me.cmdRemove.Attributes.Add("onclick", "window.opener.location.href = ""Create_RFQ2.aspx?RFQ_Name=" & Server.UrlEncode(lbl_name.Text) & "&checkven=1&pageid=" & strPageId & """")
        End If
    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        Bindgrid(True, VIEWSTATE("search"))
        'bind_table()
    End Sub

    Private Sub dtg_vendor_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_vendor.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(vendorEnum.Chk).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
            Dim lbl_adds2 As Label
            Dim objrfq As New RFQ
            Dim country As String
            Dim state As String

            lbl_adds2 = e.Item.FindControl("lbl_adds")
            Dim stradds As String
            If (Not IsDBNull(dv("CM_ADDR_LINE1"))) And Common.parseNull(dv("CM_ADDR_LINE1")) <> "" Then
                stradds = "" & dv("CM_ADDR_LINE1") & ""
            End If

            If (Not IsDBNull(dv("CM_ADDR_LINE2"))) And Common.parseNull(dv("CM_ADDR_LINE2")) <> "" Then
                If stradds = "" Then
                    stradds = "" & dv("CM_ADDR_LINE2") & ""
                Else
                    stradds = stradds & "<br>" & dv("CM_ADDR_LINE2") & ""
                End If
            End If

            If (Not IsDBNull(dv("CM_ADDR_LINE3"))) And Common.parseNull(dv("CM_ADDR_LINE3")) <> "" Then
                If stradds = "" Then
                    stradds = "" & dv("CM_ADDR_LINE3") & ""
                Else
                    stradds = stradds & "<br>" & dv("CM_ADDR_LINE3") & ""
                End If
            End If

            If (Not IsDBNull(dv("CM_POSTCODE"))) And Common.parseNull(dv("CM_POSTCODE")) <> "" Or (Not IsDBNull(dv("CM_CITY"))) Or Common.parseNull(dv("CM_CITY")) <> "" Then
                If stradds = "" Then
                    stradds = "" & dv("CM_POSTCODE") & " " & dv("CM_CITY")
                Else
                    stradds = stradds & "<br>" & dv("CM_POSTCODE") & " " & dv("CM_CITY")
                End If
            End If

            state = objrfq.get_codemstr(Common.parseNull(dv("CM_STATE")), "S")

            If state <> "" Or (Not IsNothing(state)) Then
                If stradds = "" Then
                    stradds = state
                Else
                    stradds = stradds & "<br>" & state
                End If
            End If

            country = objrfq.get_codemstr(Common.parseNull(dv("CM_COUNTRY")), "CT")

            If country <> "" Or (Not IsNothing(country)) Then
                If stradds = "" Then
                    stradds = country
                Else
                    stradds = stradds & "<br>" & country
                End If
            End If

            If (Not IsDBNull(dv("CM_EMAIL"))) And Common.parseNull(dv("CM_EMAIL")) <> "" Then
                If stradds = "" Then
                    stradds = "" & dv("CM_EMAIL") & ""
                Else
                    stradds = stradds & "<br>" & dv("CM_EMAIL") & ""
                End If
            End If

            If (Not IsDBNull(dv("CM_PHONE"))) And Common.parseNull(dv("CM_PHONE")) <> "" Then
                If stradds = "" Then
                    stradds = "Tel: " & dv("CM_PHONE") & ""
                Else
                    stradds = stradds & "<br>Tel: " & dv("CM_PHONE") & ""
                End If
            End If
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

    Private Sub cmdRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRemove.Click
        Dim dgItem As DataGridItem
        Dim objrfq As New RFQ
        Dim objval As New RFQ_User
        Dim objDB As New EAD.DBCom
        Dim strAryQuery(0) As String
        Dim strSQL As String
        Dim txtPrice As TextBox
        Dim lblPrice As Label
        Dim chkItem As CheckBox
        Dim cur As String = Request.QueryString("RFQ_Cur_value")
        Dim i As Integer = 0
        Dim str_vcom As String = ""
        Dim dtDetails As DataTable
        Dim dtrDetails As DataRow()
        Dim strSearch As String = ""

        dtDetails = Session("VendorListDetails")

        objval.dis_ID = Request(Trim("RFQ_venlist_num"))
        objval.RFQ_Name = Me.lbl_name.Text
        objval.RFQ_ID = Session("rfq_id")

        For Each dgItem In dtg_vendor.Items
            objval.V_com_ID = Me.dtg_vendor.DataKeys.Item(i)
            chkItem = dgItem.FindControl("chkSelection")

            If chkItem.Checked Then
                If str_vcom = "" Then
                    str_vcom = "'" & Me.dtg_vendor.DataKeys.Item(i) & "'"
                Else
                    str_vcom = str_vcom & ",'" & Me.dtg_vendor.DataKeys.Item(i) & "'"
                End If
                'strSQL = objrfq.Vendor_deleteList(objval)
                'Common.Insert2Ary(strAryQuery, strSQL)
                strSearch = "RVDLM_List_Index='" & objval.dis_ID & "' AND TYPE = 'list' AND CoyId = '" & objval.V_com_ID & "'"
                dtrDetails = dtDetails.Select(strSearch)
                If dtrDetails.Length > 0 Then
                    For Each oRow As DataRow In dtrDetails
                        dtDetails.Rows.Remove(oRow)
                    Next
                End If
            End If
            i = i + 1
        Next

        Session("VendorListDetails") = dtDetails
        Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
        Bindgrid(True, ViewState("search"))
    End Sub

    Private Sub cmd_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Session("RFQ_venlist_num") = ""
        Session("RFQ_LIST") = ""
        Session("RFQ_NAME") = ""
    End Sub

    Private Sub dtg_vendor_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtg_vendor.PageIndexChanged
        dtg_vendor.CurrentPageIndex = e.NewPageIndex
        Bindgrid(True, ViewState("search"))
        'bind_table()
    End Sub

    Private Sub cmdClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Response.Write("<script language='javascript'> { window.close();}</script>")

    End Sub
End Class
