Imports AgoraLegacy
Imports SSO.Component
Public Class usUsrGrp
    Inherits AgoraLegacy.AppBaseClass
    Dim objDb As New  EAD.DBCom
    Protected WithEvents rolechangedstatus As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents originalrole As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents cmdCancel As System.Web.UI.WebControls.Button
    Protected WithEvents rfv_cboPackage As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rfv_cboRole As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rfv_cboType As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents onchange As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents txOriUsrGrpName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtOriRole As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtOriType As System.Web.UI.WebControls.TextBox
    Dim objUsrGrpDetails As New UserGroup
    Dim dDispatcher As New AgoraLegacy.dispatcher
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents rdAct As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rdDeAct As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chkAccLock As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents txtUsrGrpId As System.Web.UI.WebControls.TextBox
    Protected WithEvents txUsrGrpName As System.Web.UI.WebControls.TextBox
    Protected WithEvents cboRole As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents dgAR As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdSaveAR As System.Web.UI.WebControls.Button
    Protected WithEvents tblButton As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents rfv_txtUsrGrpId As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rfv_txUsrGrpName As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldsumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents cmdGrant As System.Web.UI.WebControls.Button
    Protected WithEvents rfc_cboRole As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents cboType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents rfc_cboType As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents rfc_cboPackage As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents cboPackage As System.Web.UI.WebControls.DropDownList

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
        blnPaging = False
        blnSorting = False
        MyBase.Page_Load(sender, e)

        SetGridProperty(dgAR)
        viewstate("Mode") = Request.Params("mode")
        viewstate("UsrGrpId") = Request.Params("usrgrpid")
        viewstate("Package") = Request.Params("Package")

        If Not Page.IsPostBack Then
            Common.FillDefault(cboPackage, "Application_Mstr", "AP_APP_NAME", "AP_APP_ID", "---Select---")

            'Dim objGlobal As New AppGlobals
            'objGlobal.FillFixedRole(cboRole)

            If viewstate("Mode") = "add" Then
                lblHeader.Text = "Add User Group Details"
                cmdReset.Value = "Clear"
                txtUsrGrpId.Enabled = True
                cmdDelete.Visible = False
                cboPackage.Enabled = True

                cmdReset.Attributes.Add("onclick", "ValidatorReset();")

            ElseIf viewstate("Mode") = "modify" Then
                lblHeader.Text = "Modify User Group Details"
                txtUsrGrpId.Text = viewstate("UsrGrpId")
                cmdReset.Value = "Reset"
                cmdGrant.Visible = True
                cboPackage.Enabled = False

                Populate()

                cmdReset.Attributes.Add("onclick", "CustomReset();")
                cmdSave.Attributes.Add("onclick", "return rolechangedsaveconfirm();")
                cmdDelete.Attributes.Add("onclick", "return confirm('" & MsgForDeleteButton & "');")
                txUsrGrpName.Attributes.Add("onchange", "onchange();")
                cboRole.Attributes.Add("onchange", "onchange();")
                cboType.Attributes.Add("onchange", "onchange();")
            End If
        End If
        lnkBack.NavigateUrl = dDispatcher.direct("Global", "UsSearchUsrGrp.aspx")
    End Sub

    Private Sub Populate()
        Dim objUsrGrp As New UserGroups

        objUsrGrpDetails = objUsrGrp.GetUserGroupDetails(viewstate("UsrGrpId"), viewstate("Package"))
        txUsrGrpName.Text = objUsrGrpDetails.Name
        bindRoleBasedOnPackage()
        Common.SelDdl(objUsrGrpDetails.FixedRole, cboRole, True, True)
        Common.SelDdl(objUsrGrpDetails.Type, cboType, False, True)
        Common.SelDdl(objUsrGrpDetails.Package, cboPackage, True, True)
        cboType.Items(0).Value = ""

        'changeAdminRole

        txOriUsrGrpName.Text = txUsrGrpName.Text
        txtOriRole.Text = cboRole.SelectedValue
        txtOriType.Text = cboType.SelectedValue
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Call UpdateUserGroup()
    End Sub

    Private Sub EnableInput(ByVal pBoo As Boolean)
        'cboScreen.Enabled = Not pBoo
        txUsrGrpName.Enabled = pBoo
        cboRole.Enabled = pBoo
    End Sub
    Private Function GetInput() As UserGroup
        Dim objUsrGrpDetails As New UserGroup

        objUsrGrpDetails.Id = txtUsrGrpId.Text
        objUsrGrpDetails.Name = txUsrGrpName.Text
        objUsrGrpDetails.FixedRole = cboRole.SelectedItem.Value
        objUsrGrpDetails.Package = cboPackage.SelectedItem.Value
        objUsrGrpDetails.Type = cboType.SelectedItem.Value

        Return objUsrGrpDetails
    End Function

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim strUsrGrpId, strScreenName, strAppPackageID As String

        strUsrGrpId = Me.txtUsrGrpId.Text
        strAppPackageID = Me.cboPackage.SelectedValue
        'strScreenName = Me.cboScreen.SelectedItem.Text

        Dim objUsrGrp As New UserGroups

        ''//Retrieve Data from Database
        Dim ds As DataSet = New DataSet
        ds = objUsrGrp.SearchAccessRight(strUsrGrpId, strScreenName, strAppPackageID)

        ''//for sorting asc or desc
        Dim dvViewDept As DataView
        dvViewDept = ds.Tables(0).DefaultView
        ''dvViewSample(0)(0)
        ''If pSorted Then
        dvViewDept.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" Then dvViewDept.Sort += " DESC"
        ''End If

        ''//these only needed if you can select a grid item and click delete button
        ''//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        ''//then user delete one record. //total record = 20 (2 pages), 
        ''//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If viewstate("action") = "del" Then
            If dgAR.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dgAR.PageSize = 0 Then
                dgAR.CurrentPageIndex = dgAR.CurrentPageIndex - 1
                viewstate("action") = ""
            End If
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count
        ''//bind datagrid

        ''//datagrid.pageCount only got value after databind

        If intPageRecordCnt > 0 Then
            'intTotPage = dtgDept.PageCount
            'cmdDelete.Enabled = True
            'cmdModify.Enabled = True
            '//mean Enable, can't use button.Enabled because this is a HTML button
            cmdReset.Disabled = False
            dgAR.DataSource = dvViewDept
            dgAR.DataBind()
        Else
            'dtgDept.DataSource = ""
            'cmdDelete.Enabled = False
            'cmdModify.Enabled = False
            'cmdReset.Disabled = True
            dgAR.DataBind()
            'Common.NetMsgbox(Me, MsgNoRecord)
            'intTotPage = 0
        End If
        ''ShowStats()
    End Function

    Protected Function IsDefunct(ByVal obj As Object)
        If obj Is DBNull.Value Then
            Return False
        ElseIf obj.ToString() = "N" Then
            Return True
        Else
            Return False
        End If
    End Function

    Protected Function IsCheck(ByVal obj As Object)
        If obj Is DBNull.Value Then
            Return False
        ElseIf obj.ToString() = "N" Then
            Return False
        Else
            Return True
        End If
    End Function

    Private Function BuildSym(ByVal pIn As Int16) As String
        Dim intI As Int16
        Dim strOut As String = "&nbsp;"
        If pIn = 1 Then
            Return String.Empty
        Else
            For intI = 0 To (pIn - 2)
                If intI <> (pIn - 2) Then
                    strOut &= "<font size=4 color=silver>&#8226;&nbsp;</font>"
                Else
                    strOut &= "<font size=4 color='#696969'>&#8226;&nbsp;</font>"
                End If
            Next
        End If
        Return strOut.ToString
    End Function
    Private Sub ChangeColor()
        Dim intCnt As Integer = dgAR.Items.Count
        Dim strParent As String
        Dim i As Integer
        For i = 0 To intCnt - 1
            Dim strSym As String
            Dim strLevel As String = dgAR.Items(i).Cells(3).Text


            If GetLen(strLevel) = 1 Then
                dgAR.Items(i).BackColor = Color.Wheat
                dgAR.Items(i).Font.Bold = True
                dgAR.Items(i).Attributes.Add("class", "group")
                strParent = dgAR.Items(i).Cells(1).Text
                'dtgFunctional.Items(i).Attributes.Add("class", "main")
                'dtgFunctional.Items(i).Cells(5).Attributes.Add("style", "visibility:hidden;")
            Else
                dgAR.Items(i).Cells(1).Text = BuildSym(GetLen(strLevel)) + dgAR.Items(i).Cells(1).Text
                dgAR.Items(i).Attributes.Add("class", strParent)
            End If
        Next
    End Sub

    Private Function GetLen(ByVal pstr As String) As Integer
        Return Split(pstr, ",").Length
    End Function
    'Private Sub ChangeColor()
    '    Dim intCnt As Integer = dgAR.Items.Count
    '    Dim strParent As String
    '    Dim i As Integer
    '    For i = 0 To intCnt - 1
    '        Dim strSym As String
    '        Dim strLevel As String = dgAR.Items(i).Cells(3).Text.Length


    '        If strLevel = 3 Then
    '            dgAR.Items(i).BackColor = Color.Wheat
    '            dgAR.Items(i).Font.Bold = True
    '            dgAR.Items(i).Attributes.Add("class", "group")
    '            strParent = dgAR.Items(i).Cells(1).Text
    '            'dtgFunctional.Items(i).Attributes.Add("class", "main")
    '            'dtgFunctional.Items(i).Cells(5).Attributes.Add("style", "visibility:hidden;")
    '        Else
    '            dgAR.Items(i).Cells(1).Text = BuildSym(Replace(strLevel, ",", "").Length) + dgAR.Items(i).Cells(1).Text
    '            dgAR.Items(i).Attributes.Add("class", strParent)
    '        End If
    '    Next
    'End Sub

    'Private Sub cboScreen_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboScreen.SelectedIndexChanged

    '    If cboScreen.SelectedIndex = 0 Then
    '        tblButton.Style.Item("Display") = ""
    '        cmdSaveAR.Visible = False
    '    Else
    '        tblButton.Style.Item("Display") = "none"
    '        cmdSaveAR.Visible = True
    '    End If
    '    Bindgrid()
    '    ChangeColor()
    'End Sub

    Private Sub cmdSaveAR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSaveAR.Click
        'Dim strModuleCode As String = cboScreen.SelectedItem.Value
        Dim strID As String = Common.Parse(txtUsrGrpId.Text)
        Dim strAppPackageId As String = Common.Parse(cboPackage.SelectedItem.Value)
        'Dim objDbAppPackage As New  EAD.DBCom(ConfigurationSettings.AppSettings(strAppPackageId & "Path"))

        Dim strDefunct As String
        Dim strAllowView As String
        Dim strAllowInsert As String
        Dim strAllowUpdate As String
        Dim strAllowDelete As String
        Dim strMenuId As String

        Dim dgi As DataGridItem

        Dim cnt As Integer = dgAR.Items.Count
        Dim i As Integer

        Dim strSQL As String

        For i = 0 To cnt - 1
            dgi = dgAR.Items(i)

            strMenuId = dgi.Cells(4).Text()
            strDefunct = IIf(CType(dgi.Cells(0).Controls(1), CheckBox).Checked, "N", "Y")
            strAllowUpdate = IIf(CType(dgi.Cells(6).Controls(1), CheckBox).Checked, "Y", "N")
            strAllowInsert = IIf(CType(dgi.Cells(5).Controls(1), CheckBox).Checked, "Y", "N")
            strAllowDelete = IIf(CType(dgi.Cells(7).Controls(1), CheckBox).Checked, "Y", "N")
            strAllowView = IIf(CType(dgi.Cells(8).Controls(1), CheckBox).Checked, "Y", "N")

            strSQL = "SELECT UAR_MENU_ID "
            strSQL &= " FROM USER_ACCESS_RIGHT "
            strSQL &= " WHERE UAR_MENU_ID = '" & strMenuId & "' "
            strSQL &= " AND UAR_USRGRP_ID = '" & strID & "' "
            strSQL &= " AND UAR_APP_PKG = '" & strAppPackageId & "'"

            If objDb.Exist(strSQL) = "1" Then
                '===== case: functional exists 
                '===== action: update record  
                strSQL = " UPDATE USER_ACCESS_RIGHT "
                strSQL &= " SET UAR_ALLOW_VIEW        = '" & strAllowView & "',"
                strSQL &= "     UAR_ALLOW_INSERT      = '" & strAllowInsert & "',"
                strSQL &= "     UAR_ALLOW_UPDATE      = '" & strAllowUpdate & "',"
                strSQL &= "     UAR_ALLOW_DELETE      = '" & strAllowDelete & "',"
                strSQL &= "     UAR_DELETE_IND        = '" & strDefunct & "',"
                strSQL &= "     UAR_MOD_BY            = '" & HttpContext.Current.Session("UserId") & "',"
                strSQL &= "     UAR_MOD_DT            = " & Common.ConvertDate(Now())
                strSQL &= " WHERE UAR_USRGRP_ID = '" & strID & "'"
                strSQL &= " AND UAR_MENU_ID = '" & strMenuId & "'"
                strSQL &= " AND UAR_APP_PKG = '" & strAppPackageId & "'"
                objDb.Execute(strSQL)
            Else
                '===== case: functional not exist
                '===== action: create a new record
                If strDefunct = "N" Then
                    '===== if row is not selected, insertion is unneeded

                    strSQL = " INSERT INTO USER_ACCESS_RIGHT " & _
                             " (UAR_USRGRP_ID,UAR_MENU_ID,UAR_ALLOW_INSERT,UAR_ALLOW_UPDATE," & _
                             " UAR_ALLOW_DELETE,UAR_ALLOW_VIEW,UAR_DELETE_IND,UAR_ENT_BY,UAR_ENT_DT,UAR_APP_PKG) Values('" & _
                             strID & "','" & _
                             strMenuId & "','" & _
                             strAllowInsert & "','" & _
                             strAllowUpdate & "','" & _
                             strAllowDelete & "','" & _
                             strAllowView & "','N','" & HttpContext.Current.Session("UserId") & "'," & Common.ConvertDate(Now()) & ",'" & strAppPackageId & "')"

                    objDb.Execute(strSQL)
                End If
            End If
        Next
        '        Common.NetMsgbox(Me, "Record Saved", Page.Request.RawUrl, MsgBoxStyle.Information)
        'Common.NetMsgbox(Me, "Record Saved", "UsUsrGrp.aspx?mode=modify&usrgrpid=" & Server.UrlEncode(txtUsrGrpId.Text) & "&Package=" & Server.UrlEncode(cboPackage.SelectedValue), MsgBoxStyle.Information)
        Common.NetMsgbox(Me, "Record Saved", dDispatcher.direct("Global", "UsUsrGrp.aspx", "mode=modify&usrgrpid=" & Server.UrlEncode(txtUsrGrpId.Text) & "&Package=" & Server.UrlEncode(cboPackage.SelectedValue)), MsgBoxStyle.Information)
        'Common.NetMsgbox(Me, "Record Saved", "UsSearchUsrGrp.aspx", MsgBoxStyle.Information)
        'tblButton.Style.Item("Display") = ""
        'cmdSaveAR.Visible = False
        'dgAR.DataBind()
        ''Bindgrid()
    End Sub


    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim strMsg As String
        Dim objUsrGrp As New UserGroups
        If objUsrGrp.DelUserGroup(viewstate("UsrGrpId"), viewstate("Package")) Then
            'EnableInput(False)
            'cmdDelete.Enabled = False
            'cmdSave.Enabled = False
            'cmdReset.Disabled = True
            'cmdGrant.Enabled = False
            ''cboScreen.Enabled = False

        End If
        'Common.NetMsgbox(Me, objUsrGrp.Message, "UsSearchUsrGrp.aspx", MsgBoxStyle.Information)
        Common.NetMsgbox(Me, objUsrGrp.Message, dDispatcher.direct("Global", "UsSearchUsrGrp.aspx", ""), MsgBoxStyle.Information)

    End Sub

    'Private Sub cmdGrant_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGrant.Click
    '    cboScreen.Enabled = True
    'End Sub

    Private Sub cmdGrant_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdGrant.Click
        If rolechangedstatus.Value = "true" Then
            Call UpdateUserGroup()
        Else
            Call OpenGrantMode()
        End If
    End Sub

    Private Sub cboPackage_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboPackage.SelectedIndexChanged
        bindRoleBasedOnPackage()
    End Sub

    Function bindRoleBasedOnPackage()
        Dim objGlobal As New AppGlobals
        Dim valPackage As String
        Dim lstItem As ListItem

        If viewstate("Mode") = "modify" Then
            valPackage = objUsrGrpDetails.Package
        Else
            valPackage = cboPackage.SelectedValue
        End If

        cboRole.Items.Clear()
        lstItem = New ListItem("---Select---", "")
        cboRole.Items.Insert(0, lstItem)

        Select Case valPackage
            Case "eProcure"
                objGlobal.FillFixedRole(cboRole, ConfigurationSettings.AppSettings("eProcurePath"))
                'changeAdminRole()
            Case "eRFP"
                objGlobal.FillFixedRole(cboRole, ConfigurationSettings.AppSettings("eRFPPath"))
                'changeAdminRole()
            Case "eAuc"
                ' No have application yet
            Case "eCon"
                ' No have application yet
        End Select
    End Function

    Private Sub OpenGrantMode()
        tblButton.Style.Item("Display") = "none"
        cmdSaveAR.Visible = True
        cmdCancel.Visible = True
        txUsrGrpName.Enabled = False
        cboRole.Enabled = False
        cboType.Enabled = False
        cboPackage.Enabled = False
        Bindgrid()
        ChangeColor()
    End Sub

    Private Sub UpdateUserGroup()
        Dim objUsrGrp As New UserGroups
        Dim objUsrGrpDetails As New UserGroup

        objUsrGrpDetails = GetInput()

        If viewstate("Mode") = "add" Then
            If objUsrGrp.AddUserGroup(objUsrGrpDetails) Then
                'txtUsrGrpId.Enabled = False
                'txUsrGrpName.Enabled = False
                'cboRole.Enabled = False
                'cmdGrant.Visible = True
                'Common.NetMsgbox(Me, objUsrGrp.Message, "UsUsrGrp.aspx?mode=modify&usrgrpid=" & Server.UrlEncode(objUsrGrpDetails.Id) & "&Package=" & Server.UrlEncode(objUsrGrpDetails.Package), MsgBoxStyle.Information)
                Common.NetMsgbox(Me, objUsrGrp.Message, dDispatcher.direct("Global", "UsUsrGrp.aspx", "mode=modify&usrgrpid=" & Server.UrlEncode(objUsrGrpDetails.Id) & "&Package=" & Server.UrlEncode(objUsrGrpDetails.Package)), MsgBoxStyle.Information)

            Else
                Common.NetMsgbox(Me, objUsrGrp.Message, MsgBoxStyle.Information)
            End If

        ElseIf viewstate("Mode") = "modify" Then
            objUsrGrp.UpdateUserGroup(objUsrGrpDetails)

            If onchange.Value = "1" Then
                Common.NetMsgbox(Me, objUsrGrp.Message, MsgBoxStyle.Information)
            Else
                'Common.NetMsgbox(Me, objUsrGrp.Message, "UsSearchUsrGrp.aspx", MsgBoxStyle.Information)
                Common.NetMsgbox(Me, objUsrGrp.Message, dDispatcher.direct("Global", "UsSearchUsrGrp.aspx", ""), MsgBoxStyle.Information)
            End If
        End If

        txOriUsrGrpName.Text = txUsrGrpName.Text
        txtOriRole.Text = cboRole.SelectedValue
        txtOriType.Text = cboType.SelectedValue

        rolechangedstatus.Value = "false"
        onchange.Value = "0"
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        cmdReset.Value = "Reset"
        cmdGrant.Visible = True
        cmdDelete.Visible = True
        cboPackage.Enabled = False

        tblButton.Style.Item("Display") = ""
        txUsrGrpName.Enabled = True
        cboRole.Enabled = True
        cboType.Enabled = True

        dgAR.DataSource = Nothing
        dgAR.DataBind()

        cmdSaveAR.Visible = False
        cmdCancel.Visible = False
    End Sub

    '' ai chu modify on 30/08/2005
    '' user request change fixed role from Admin to Super Admin
    'Private Sub changeAdminRole()
    '    Dim lstItem As ListItem
    '    For Each lstItem In cboRole.Items
    '        If lstItem.Value = FixedRole.Admin.ToString Then
    '            lstItem.Text = "Super Admin"
    '            Exit For
    '        End If
    '    Next
    'End Sub
End Class
