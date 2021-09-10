Imports AgoraLegacy
Imports eProcure.Component


Public Class QualityStd
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDb As New EAD.DBCom
    Dim objAdm As New Admin
    Protected WithEvents dtgQS As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Protected WithEvents cmdModify As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_delete As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_upload As System.Web.UI.WebControls.Button
    Protected WithEvents File1 As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents pnlAttach As System.Web.UI.WebControls.Panel
    Dim ds As DataSet
    Dim objFile As New FileManagement

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

    'Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
    '    cmdAdd.Enabled = False
    '    cmd_delete.Enabled = False
    '    cmdModify.Enabled = False
    '    Dim alButtonList As ArrayList
    '    alButtonList = New ArrayList
    '    alButtonList.Add(cmdAdd)
    '    htPageAccess.Add("add", alButtonList)
    '    alButtonList = New ArrayList
    '    alButtonList.Add(cmdModify)
    '    htPageAccess.Add("update", alButtonList)
    '    alButtonList = New ArrayList
    '    alButtonList.Add(cmd_delete)
    '    htPageAccess.Add("delete", alButtonList)
    '    CheckButtonAccess()
    '    If intPageRecordCnt > 0 Then
    '        cmd_delete.Enabled = blnCanDelete
    '        cmdModify.Enabled = blnCanUpdate
    '    Else
    '        cmd_delete.Enabled = False
    '        cmdModify.Enabled = False
    '    End If
    '    alButtonList.Clear()
    '    'displayAttachFile()
    'End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objUser As New Users
        MyBase.Page_Load(sender, e)
        'SetGridProperty(dtgQS)
        If Not IsPostBack Then
            ViewState("Side") = Request.Params("side")
            GenerateTab()
            'cmd_search.Enabled = True
            'cmd_clear1.Enabled = True
            'cmd_delete.Enabled = False
            'cmdModify.Enabled = False
            'cmd_Reset.Disabled = True
            'Hide_Add2.Style("display") = "none"
            cmd_upload.Attributes.Add("onclick", "return checkDocFile('doc','" & File1.ClientID & "');")
        End If
        'cmd_delete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
        'cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
        displayAttachFile()
    End Sub
    'Function bindgridApp(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
    '    Dim ds As New DataSet

    '    ds = objAdm.getQS()

    '    viewstate("intPageRecordCnt") = ds.Tables(0).Rows.Count

    '    Dim dvViewSample As DataView
    '    dvViewSample = ds.Tables(0).DefaultView
    '    dvViewSample.Sort = viewstate("SortExpression")
    '    If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"


    '    If viewstate("action") = "del" Then
    '        If dtgQS.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgQS.PageSize = 0 Then
    '            dtgQS.CurrentPageIndex = dtgQS.CurrentPageIndex - 1
    '            viewstate("action") = ""
    '        End If
    '    End If

    '    intPageRecordCnt = ds.Tables(0).Rows.Count

    '    If intPageRecordCnt > 0 Then
    '        resetDatagridPageIndex(dtgQS, dvViewSample)
    '        cmd_delete.Enabled = True
    '        cmdModify.Enabled = True
    '        dtgQS.DataSource = dvViewSample
    '        dtgQS.DataBind()
    '    Else
    '        cmd_delete.Enabled = False
    '        cmdModify.Enabled = False
    '        dtgQS.DataBind()
    '        Common.NetMsgbox(Me, MsgNoRecord)
    '    End If
    '    ViewState("PageCount") = dtgQS.PageCount

    'End Function

    'Public Sub MyDataGrid_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
    '    dtgQS.CurrentPageIndex = e.NewPageIndex
    '    bindgridApp(0)
    'End Sub
    'Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
    '    Grid_SortCommand(sender, e)
    '    dtgQS.CurrentPageIndex = 0
    '    bindgridApp(0, True)
    'End Sub

    'Private Sub cmd_delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_delete.Click
    '    'txtSWSearch.Text = ""

    '    Dim dgItem As DataGridItem
    '    Dim chk As CheckBox
    '    Dim strIndex As String
    '    Dim intMsgNo As Integer

    '    Dim dt As New DataTable
    '    Dim dr As DataRow
    '    dt.Columns.Add("Index", Type.GetType("System.String"))

    '    For Each dgItem In dtgQS.Items
    '        chk = dgItem.FindControl("chkSelection")
    '        strIndex = dgItem.Cells.Item(2).Text
    '        If chk.Checked Then
    '            dr = dt.NewRow
    '            dr("Index") = strIndex
    '            dt.Rows.Add(dr)
    '        End If
    '    Next

    '    If objAdm.delQS(dt) Then
    '        Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
    '    Else
    '        Common.NetMsgbox(Me, MsgRecordNotDelete, MsgBoxStyle.Information)
    '    End If

    '    ViewState("action") = "del"
    '    bindgridApp(0)
    'End Sub

    'Private Sub dtgQS_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgQS.ItemCreated
    '    intPageRecordCnt = viewstate("intPageRecordCnt")
    '    Grid_ItemCreated(dtgQS, e)
    '    If e.Item.ItemType = ListItemType.Header Then
    '        Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
    '        chkAll.Attributes.Add("onclick", "selectAll();")
    '    End If
    'End Sub

    'Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click

    '    'Hide_Add2.Style("display") = "inline"
    '    'txt_add_mod.Text = ""
    '    'Me.lbl_add_mod.Text = "add"
    '    'cmd_clear.Text = "Clear"
    '    'hidMode.Value = "a"
    'End Sub

    'Private Sub cmdModify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdModify.Click

    '    Dim dgItem As DataGridItem
    '    Dim chk As CheckBox
    '    Dim i As Integer
    '    i = 0

    '    'Hide_Add2.Style("display") = "inline"
    '    'cmd_clear.Text = "Reset"
    '    'hidMode.Value = "m"
    '    'For Each dgItem In dtgQS.Items
    '    '    chk = dgItem.FindControl("chkSelection")
    '    '    If chk.Checked Then
    '    '        hidIndex.Value = dgItem.Cells(2).Text
    '    '        txt_add_mod.Text = dgItem.Cells(1).Text
    '    '        ViewState("oldvalue") = dgItem.Cells(1).Text
    '    '        Exit For
    '    '    End If
    '    '    i = i + 1
    '    'Next

    '    'Me.lbl_add_mod.Text = "modify"
    '    cmdModify.Enabled = True
    'End Sub
    Private Sub cmd_upload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_upload.Click
        Dim objFile As New FileManagement
        If File1.Value = "" Then
        Else
            Dim objDB As New EAD.DBCom
            Dim dblMaxFileSize As Double = Session("FileSize")

            Dim sFileName As String
            sFileName = System.IO.Path.GetFileName(File1.PostedFile.FileName)

            If Len(sFileName) > 50 Then
                Common.NetMsgbox(Me, "File name exceeds 46 character")
            ElseIf File1.PostedFile.ContentLength > 0 And File1.PostedFile.ContentLength / 1024 <= dblMaxFileSize Then
                objFile.FileUpload(File1, EnumUploadType.DocAttachment, "QS", EnumUploadFrom.FrontOff, Session("CompanyId"))
            ElseIf File1.PostedFile.ContentLength = 0 Then
                Common.NetMsgbox(Me, "0 byte document or file not found")
            Else
                Common.NetMsgbox(Me, "File exceeds maximum file size")
            End If

        End If
        displayAttachFile()
        objFile = Nothing
    End Sub
    Private Sub displayAttachFile()
        Dim objFile As New FileManagement
        Dim dsAttach As New DataSet
        Dim drvAttach As DataView
        Dim i As Integer
        Dim strFile, strFile1, strURL, strTemp As String

        dsAttach = objFile.getAttachment(Session("CompanyId"), Session("CompanyId"), "QS")

        pnlAttach.Controls.Clear()
        drvAttach = dsAttach.Tables(0).DefaultView
        If drvAttach.Count > 0 Then
            For i = 0 To drvAttach.Count - 1
                strFile = drvAttach(i)("CDA_ATTACH_FILENAME")
                strFile1 = drvAttach(i)("CDA_HUB_FILENAME")
                strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DocAttachment, "QS", EnumUploadFrom.FrontOff)
                Dim lblBr As New Label
                Dim lblFile As New Label
                Dim lnk As New ImageButton
                lblFile.Text = "&nbsp;" & i + 1 & ") " & strURL & " (" & drvAttach(i)("CDA_FILESIZE") & "KB) "
                lblBr.Text = "<BR>"
                lnk.ImageUrl = dDispatcher.direct("Plugins/images", "i_delete.gif")
                lnk.ID = drvAttach(i)("CDA_ATTACH_INDEX")
                lnk.CausesValidation = False
                AddHandler lnk.Click, AddressOf deleteattach_Click

                pnlAttach.Controls.Add(lblFile)
                pnlAttach.Controls.Add(lnk)
                pnlAttach.Controls.Add(lblBr)
            Next
        Else
            Dim lblFile As New Label
            lblfile.Text = "No Files Attached"
            pnlAttach.Controls.Add(lblFile)
        End If
        objFile = Nothing
    End Sub
    Private Sub deleteattach_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim objrfq As New RFQ

        objrfq.deleteRFQAttachment(CType(sender, ImageButton).ID)
        displayAttachFile()

        objrfq = Nothing
    End Sub
    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        If ViewState("Side") = "BUYER" Then
            '    Session("w_QualityStd_tabs") = "<div class=""t_entity""><ul>" & _
            '    "<li><div class=""space""></div></li>" & _
            '                         "<li><a class=""t_entity_btn"" href=""../Companies/coCompanyDetail.aspx?side=BUYER&mode=modify&pageid=" & strPageId & """><span>Company Details</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                         "<li><a class=""t_entity_btn"" href=""../Admin/BComParam.aspx?side=BUYER&pageid=" & strPageId & """><span>Parameters</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                         "<li><a class=""t_entity_btn"" href=""../Admin/AddressMaster.aspx?side=BUYER&type=D&mod=T&pageid=" & strPageId & """><span>Delivery Address</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                         "<li><a class=""t_entity_btn"" href=""../Admin/AddressMaster.aspx?side=BUYER&type=B&mod=T&pageid=" & strPageId & """><span>Billing Address</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                          "<li><a class=""t_entity_btn"" href=""../Admin/DeptSetup.aspx?side=BUYER&pageid=" & strPageId & """><span>Department</span></a></li>" & _
            '                          "<li><div class=""space""></div></li>" & _
            '                 "</ul><div></div></div>"
            'Else
            '    Session("w_QualityStd_tabs") = "<div class=""t_entity""><ul>" & _
            '    "<li><div class=""space""></div></li>" & _
            '                     "<li><a class=""t_entity_btn"" href=""../Companies/coCompanyDetail.aspx?side=VENDOR&mode=modify&pageid=" & strPageId & """><span>Company Details</span></a></li>" & _
            '                     "<li><div class=""space""></div></li>" & _
            '                     "<li><a class=""t_entity_btn"" href=""../Admin/BComVendor.aspx?side=VENDOR&pageid=" & strPageId & """><span>Parameters</span></a></li>" & _
            '                     "<li><div class=""space""></div></li>" & _
            '                        "<li><a class=""t_entity_btn"" href=""SalesInfo.aspx?side=VENDOR&pageid=" & strPageId & """><span>Sales Info</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                        "<li><a class=""t_entity_btn"" href=""SoftwareApp.aspx?side=VENDOR&pageid=" & strPageId & """><span>Software</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                        "<li><a class=""t_entity_btn_selected"" href=""QualityStd.aspx?side=VENDOR&pageid=" & strPageId & """><span>Quality Standards</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            ' "</ul><div></div></div>"
            Session("w_QualityStd_tabs") = "<div class=""t_entity""><ul>" & _
            "<li><div class=""space""></div></li>" & _
                                   "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "coCompanyDetail.aspx", "side=BUYER&mode=modify" & "&pageid=" & strPageId) & """><span>Company Details</span></a></li>" & _
                                   "<li><div class=""space""></div></li>" & _
                                   "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "BComParam.aspx", "side=BUYER" & "&pageid=" & strPageId) & """><span>Parameters</span></a></li>" & _
                                   "<li><div class=""space""></div></li>" & _
                                   "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=D&mod=T" & "&pageid=" & strPageId) & """><span>Delivery Address</span></a></li>" & _
                                   "<li><div class=""space""></div></li>" & _
                                   "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=B&mod=T" & "&pageid=" & strPageId) & """><span>Billing Address</span></a></li>" & _
                                   "<li><div class=""space""></div></li>" & _
                                   "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "DeptSetup.aspx", "side=BUYER" & "&pageid=" & strPageId) & """><span>Department</span></a></li>" & _
                                   "<li><div class=""space""></div></li>" & _
             "</ul><div></div></div>"
        Else
            Session("w_QualityStd_tabs") = "<div class=""t_entity""><ul>" & _
                             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "coCompanyDetail.aspx", "side=VENDOR&mode=modify" & "&pageid=" & strPageId) & """><span>Company Details</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "BComVendor.aspx", "side=VENDOR" & "&pageid=" & strPageId) & """><span>Parameters</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "SalesInfo.aspx", "side=VENDOR" & "&pageid=" & strPageId) & """><span>Sales Info</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "SoftwareApp.aspx", "side=VENDOR" & "&pageid=" & strPageId) & """><span>Software</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Companies", "QualityStd.aspx", "side=VENDOR" & "&pageid=" & strPageId) & """><span>Quality Standards</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
         "</ul><div></div></div>"

        End If
    End Sub

End Class
