Imports AgoraLegacy
Imports eProcure.Component
Public Class VendorListFTN
    Inherits AgoraLegacy.AppBaseClass
  
    Protected WithEvents dt_V_com As System.Web.UI.WebControls.Table

    Dim dDispatcher As New AgoraLegacy.dispatcher

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
        cmd_save.Enabled = False
        'cmd_remove.Enabled = False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmd_save)
        htPageAccess.Add("add", alButtonList)

        'alButtonList = New ArrayList
        'alButtonList.Add(cmd_remove)
        'htPageAccess.Add("delete", alButtonList)

        CheckButtonAccess()

        If intPageRecordCnt = 0 Then intPageRecordCnt = ViewState("intPageRecordCnt")
        If intPageRecordCnt > 0 Then
            cmd_save.Enabled = blnCanAdd
            'cmd_remove.Enabled = blnCanDelete
        Else
            cmd_save.Enabled = False
            'cmd_remove.Enabled = False
        End If

        alButtonList.Clear()
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)
        SetGridProperty(Me.dtg_vendor)
        'Michelle - Note : session("edit") = 0

        Dim objrfq As New RFQ

        'If Session("edit") = "2" Then
        If Session("list_index") = "" Then
            Session("list_index") = Request(Trim("index_list"))
        Else
            If Session("list_index") <> Request(Trim("index_list")) And Request(Trim("index_list")) <> "" And Request(Trim("index_list")) <> " " Then
                If Not IsNothing(Request(Trim("index_list"))) Then
                    Session("list_index") = Request(Trim("index_list"))
                End If
            End If
        End If

        'Me.cmdexit_addlist1.Attributes.Add("onclick", "window.opener.location.href=""../personalSetting/RFQ_DisplayVen.aspx?pageid=" & strPageId & "&value=" & _
        'Session("list_index") & "&VenName=" & _
        'Server.UrlEncode(Session("RFQ_NAME")) & """;window.close();")
        'End If

        If Not Page.IsPostBack Then
            '#### For VendorList ####
            Dim search As String = Trim(Request("search"))
            If strPageId Is Nothing Then strPageId = Request(Trim("pageid"))
            ViewState("search") = search
            If (search <> "") Then
                'Bindgrid(True, search)
                dtg_vendor.CurrentPageIndex = 0
                Bindgrid(False, search, True)
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

            'Me.lbl_no.Text = Session("RFQ_NAME")
            'Me.lbl_List_No.Text = Session("RFQ_LIST")

            If Session("edit") <> "2" Then
                'Me.cmd_exit.Attributes.Add("onclick", "window.opener.location.href = ""Create_RFQ2.aspx?RFQ_Name=" & Server.UrlEncode(lbl_no.Text) & "&checkven=1&pageid=" & strPageId & """;window.close();")
                Me.cmd_exit.Attributes.Add("onclick", "window.close();")
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

                'Me.cmdexit_addlist1.Attributes.Add("onclick", "window.opener.location.href=""../personalSetting/RFQ_DisplayVen.aspx?pageid=" & strPageId & "&value=" & _
                'Session("list_index") & "&VenName=" & _
                'Session("RFQ_NAME") & """;window.close();")
            End If

            'If Session("edit") = "1" Then ' side gary, edit vendor
            '    Me.cmdexit_addlist1.Visible = False
            '    Me.cmd_save.Visible = False
            '    Me.cmd_remove.Visible = True
            '    Me.lbl_title.Text = "Selectsss Vendor Companies"
            '    If Not Page.IsPostBack Then
            '        Bindgrid(True, ViewState("search"))
            '    End If
            '    Me.cmd_remove.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','');")
            'ElseIf Session("edit") = "0" Then ' gary , add vendor list 
            'Me.cmdexit_addlist1.Visible = False
            'Me.cmd_remove.Visible = False
            Me.lbl_title.Text = "Select Vendor Companies"
            Me.cmd_save.Visible = True
            bind_table()
            'Me.cmd_remove.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','');")
            'Me.cmd_remove.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','');")
        End If
    End Sub

    Sub bind_table()
        Dim row As Integer
        Dim dr As TableRow
        Dim objrfq As New RFQ
        Dim objval As New RFQ_User
        Dim count, a, c, i, j As Integer
        Dim value(29) As String

        count = 1
        dr = New TableRow
        objrfq.Vendor_check_AZ(value)
        c = 0
        a = 0
        Dim search09 As String

        For j = 0 To 26
            dr.Cells.Add(New TableCell)
            If j = 0 Then
                If Not IsNothing(value(c)) Then
                    Do While Not System.Text.RegularExpressions.Regex.IsMatch(value(c), "[A-Za-z]")

                        If search09 = "" Then
                            search09 = value(c)
                        Else
                            search09 = search09 & "," & value(c)
                        End If
                        c = c + 1
                        i = 1
                    Loop
                End If

                If i = 1 Then
                    Dim str As String
                    'str = "VendorList.aspx?pageid=" & strPageId & "&search=" & Server.UrlEncode(search09) & "&RFQ_Name=" & Server.UrlEncode(lbl_no.Text) & "&RFQ_list=" & Server.UrlEncode(lbl_List_No.Text) & "&checkven=1"
                    str = dDispatcher.direct("RFQ", "VendorList.aspx", "pageid=" & strPageId & "&search=" & Server.UrlEncode(search09) & "&checkven=1")
                    dr.Cells(j).Text = "<A href=" & str & "><font color=#0000ff>0-9</font></A>"
                Else
                    dr.Cells(j).Text = "0-9"
                End If
            Else
                If Chr(65 + (j - 1)) = value(c) Then
                    'dr.Cells(j).Text = "<A href=""VendorList.aspx?pageid=" & strPageId & "&search=" & Chr(65 + (j - 1)) & "&RFQ_Name=" & Server.UrlEncode(lbl_no.Text) & "&RFQ_list=" & lbl_List_No.Text & "&checkven=1"" ><font color=#0000ff>" & Chr(65 + (j - 1)) & "</font></A>"
                    dr.Cells(j).Text = "<A href=""" & dDispatcher.direct("RFQ", "VendorList.aspx", "pageid=" & strPageId & "&search=" & Chr(65 + (j - 1)) & "&checkven=1") & " "" ><font color=#0000ff>" & Chr(65 + (j - 1)) & "</font></A>"

                    c = c + 1
                Else
                    dr.Cells(j).Text = Chr(65 + (j - 1))
                End If
            End If
        Next

        dt_V_com.Rows.Add(dr)
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False, Optional ByVal searchbyname As String = "", Optional ByVal searchbylike As Boolean = False)
        Dim objrfq As New RFQ
        Dim ds As New DataSet
        'ds = objrfq.getSearchVendor(searchbyname)

        If searchbylike = True Then
            ds = objrfq.GetApprVendor_AZ(searchbyname)
        Else
            ds = objrfq.GetApprVendor(searchbyname)
        End If

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt
        'If pSorted Then
        '    dvViewSample.Sort = ViewState("SortExpression")
        '    If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"
        'End If

        If ViewState("action") = "del" Then
            If dtg_vendor.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtg_vendor.PageSize = 0 Then
                dtg_vendor.CurrentPageIndex = dtg_vendor.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If

        dtg_vendor.DataSource = dvViewSample
        dtg_vendor.DataBind()

        cmd_save.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','');")
    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        Bindgrid(True, txtVendorName.Text)
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


    'Private Sub cmd_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_save.Click
    '    Dim dgItem As DataGridItem
    '    Dim objrfq As New RFQ
    '    Dim objval As New RFQ_User
    '    Dim objDB As New EAD.DBCom
    '    Dim strAryQuery(0) As String
    '    Dim strSQL As String
    '    Dim chkItem As CheckBox

    '    For Each dgItem In dtg_vendor.Items
    '        objval.V_com_ID = dgItem.Cells(3).Text
    '        objval.V_Com_Name = dgItem.Cells(1).Text
    '        chkItem = dgItem.FindControl("chkSelection")

    '        If chkItem.Checked Then
    '            strSQL = objrfq.Vendor_Add_Inv_Ven_List2_TEMP(objval, "A")
    '            Common.Insert2Ary(strAryQuery, strSQL)
    '        End If
    '    Next

    '    objDB.BatchExecute(strAryQuery)
    '    Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
    '    'dtg_vendor.DataSource = Nothing
    '    'bind_table()
    '    dtg_vendor.DataBind()

    'End Sub

    Private Sub cmd_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_save.Click
        Dim dgItem As DataGridItem
        Dim objrfq As New RFQ
        Dim objval As New RFQ_User
        Dim objDB As New EAD.DBCom
        Dim strAryQuery(0) As String
        Dim strSQL As String
        Dim chkItem As CheckBox
        'Dim aryPreVendorListNew As New ArrayList
        'Dim aryPreVendorNew As New ArrayList
        'Dim arySVendorNew As New ArrayList

        Dim i As Integer = 0
        Dim objGLO As New AppGlobals
        Dim check As Integer
        Dim dtList As DataTable
        Dim dtDetails As DataTable
        Dim dtListTemp As New DataTable
        Dim dtDetailsTemp As New DataTable
        Dim dtrList As DataRow()
        Dim dtrDetails As DataRow()
        Dim dtrSpec As DataRow()
        Dim dtrSpecList As DataRow()
        Dim dtrNewList As DataRow
        Dim dtrNewDetails As DataRow
        Dim j As Integer
        Dim strSearch As String = ""
        Dim intFound As Integer = 0
        Dim count As Integer = 0
        'If Not IsNothing(Session("PreVendorListNew")) Then aryPreVendorListNew = Session("PreVendorListNew")
        'If Not IsNothing(Session("PreVendorNew")) Then aryPreVendorNew = Session("PreVendorNew")
        'If Not IsNothing(Session("SVendorNew")) Then arySVendorNew = Session("SVendorNew")
        dtListTemp.Columns.Add("RVDLM_List_Name", Type.GetType("System.String"))
        dtListTemp.Columns.Add("RVDLM_List_Index", Type.GetType("System.String"))
        dtListTemp.Columns.Add("TYPE", Type.GetType("System.String"))
        dtListTemp.Columns.Add("Added", Type.GetType("System.String"))

        dtDetailsTemp.Columns.Add("CoyId", Type.GetType("System.String"))
        dtDetailsTemp.Columns.Add("RVDLM_List_Name", Type.GetType("System.String"))
        dtDetailsTemp.Columns.Add("RVDLM_List_Index", Type.GetType("System.String"))
        dtDetailsTemp.Columns.Add("TYPE", Type.GetType("System.String"))
        dtDetailsTemp.Columns.Add("Added", Type.GetType("System.String"))

        count = 0
        intFound = 0
        dtList = Session("VendorList")
        dtDetails = Session("VendorListDetails")

        For Each dgItem In dtg_vendor.Items
            objval.V_com_ID = dgItem.Cells(3).Text
            objval.V_Com_Name = dgItem.Cells(1).Text
            chkItem = dgItem.FindControl("chkSelection")

            If chkItem.Checked Then
                strSearch = "RVDLM_List_Index='" & objval.V_com_ID & "' AND TYPE = 'specific'"
                dtrSpecList = dtList.Select(strSearch)
                If dtrSpecList.Length > 0 Then  'if specific vendor found
                    intFound = 1

                Else 'if specific vendor list not found
                    strSearch = "TYPE = 'list' AND CoyId = '" & objval.V_com_ID & "'"
                    dtrDetails = dtDetails.Select(strSearch)
                    If dtrDetails.Length > 0 Then   'if vendor found in vendor list
                        intFound = 1

                    Else
                        check = 1
                        dtrNewDetails = dtDetailsTemp.NewRow
                        dtrNewDetails("CoyId") = objval.V_com_ID
                        dtrNewDetails("RVDLM_List_Name") = objval.V_Com_Name
                        dtrNewDetails("RVDLM_List_Index") = objval.V_com_ID
                        dtrNewDetails("TYPE") = "specific"
                        dtrNewDetails("Added") = "Y"
                        dtDetailsTemp.Rows.Add(dtrNewDetails)

                        dtrNewList = dtListTemp.NewRow
                        dtrNewList("RVDLM_List_Name") = objval.V_Com_Name
                        dtrNewList("RVDLM_List_Index") = objval.V_com_ID
                        dtrNewList("TYPE") = "specific"
                        dtrNewList("Added") = "Y"
                        dtListTemp.Rows.Add(dtrNewList)
                    End If

                End If
                'If aryPreVendorNew.IndexOf(objval.V_com_ID) > -1 Then  'If already exists in pre-defined vendor list

                'Else
                '    If arySVendorNew.Count > 0 Then
                '        If arySVendorNew.IndexOf(objval.V_com_ID) > -1 Then
                '        Else
                '            check = 1
                '            arySVendorNew.Add(objval.V_com_ID)
                '        End If

                '    Else
                '        check = 1
                '        arySVendorNew.Add(objval.V_com_ID)

                '    End If
                'End If

            End If
        Next

        If check <> 1 Then
            Common.NetMsgbox(Me, objGLO.GetErrorMessage("00027"), MsgBoxStyle.Information)

        Else
            'For i = 0 To aryVendor.Count - 1
            '    arySVendorCurrent.Add(aryVendor(i))
            'Next
            dtList.Merge(dtListTemp)
            dtDetails.Merge(dtDetailsTemp)
            Session("VendorList") = dtList
            Session("VendorListDetails") = dtDetails
            Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
        End If

        dtg_vendor.DataBind()
        bind_table()
    End Sub
    'Private Sub cmd_remove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_remove.Click
    '    Dim dgItem As DataGridItem
    '    Dim objrfq As New RFQ
    '    Dim objval As New RFQ_User
    '    Dim objDB As New EAD.DBCom
    '    Dim strAryQuery(0) As String
    '    Dim strSQL As String
    '    Dim txtPrice As TextBox
    '    Dim lblPrice As Label
    '    Dim chkItem As CheckBox
    '    Dim cur As String = Request.QueryString("RFQ_Cur_value")
    '    Dim i As Integer = 0
    '    Dim str_vcom As String = ""

    '    objval.dis_ID = Request(Trim("RFQ_venlist_num"))
    '    'objval.RFQ_Name = Me.lbl_no.Text
    '    objval.RFQ_ID = Session("rfq_id")

    '    For Each dgItem In dtg_vendor.Items
    '        objval.V_com_ID = Me.dtg_vendor.DataKeys.Item(i)
    '        chkItem = dgItem.FindControl("chkSelection")

    '        If chkItem.Checked Then
    '            If str_vcom = "" Then
    '                str_vcom = "'" & Me.dtg_vendor.DataKeys.Item(i) & "'"
    '            Else
    '                str_vcom = str_vcom & ",'" & Me.dtg_vendor.DataKeys.Item(i) & "'"
    '            End If
    '            strSQL = objrfq.Vendor_deleteList(objval)
    '            Common.Insert2Ary(strAryQuery, strSQL)
    '        End If
    '        i = i + 1
    '    Next

    '    strSQL = objrfq.Vendor_remove_inv(objval, str_vcom)
    '    Common.Insert2Ary(strAryQuery, strSQL)

    '    If objDB.BatchExecute(strAryQuery) Then
    '        objrfq.Vendor_CheckList(objval)
    '        Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
    '    End If

    '    'Bindgrid(True, ViewState("search"))
    '    dtg_vendor.DataSource = Nothing
    '    dtg_vendor.DataBind()
    'End Sub

    Private Sub cmd_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Session("RFQ_venlist_num") = ""
        Session("RFQ_LIST") = ""
        Session("RFQ_NAME") = ""
    End Sub


    Private Sub dtg_vendor_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtg_vendor.PageIndexChanged
        dtg_vendor.CurrentPageIndex = e.NewPageIndex
        Bindgrid(True, txtVendorName.Text)
        'bind_table()
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        dtg_vendor.CurrentPageIndex = 0
        Bindgrid(False, txtVendorName.Text)
        bind_table()
    End Sub
    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtVendorName.Text = ""
        bind_table()
    End Sub
End Class
