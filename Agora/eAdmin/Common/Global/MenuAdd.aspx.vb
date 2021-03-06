Imports AgoraLegacy
Imports SSO.Component
Public Class MenuAdd
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

    Protected WithEvents txtMenuID As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtMenuName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtMenuImg As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtMenuLevel As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtMenuParent As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtMenuURL As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblAction As System.Web.UI.WebControls.Label
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object


    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim objMenu As New MenuSeq
        Dim strLastMenuID As String
        Dim intLastParent As Integer

        blnPaging = False
        blnSorting = False
        'MyBase.Page_Load(sender, e)



        'SetGridProperty(dgAR)
        ViewState("Mode") = Request.Params("mode")
        ViewState("MenuId") = Request.Params("menuid")
        ViewState("Level") = Request.Params("level")

        If ViewState("Mode") = "add" Then
            GenerateTab()
        End If
        If ViewState("Mode") = "modify" Then
            Session("MenuAdd_tabs") = ""
        End If
        If Not Page.IsPostBack Then
            '  Common.FillDefault(cboPackage, "Application_Mstr", "AP_APP_NAME", "AP_APP_ID", "---Select---")

            'Dim objGlobal As New AppGlobals
            'objGlobal.FillFixedRole(cboRole)

            If ViewState("Mode") = "add" Then
                lblHeader.Text = "Add Menu Details"
                cmdReset.Value = "Clear"

                cmdDelete.Visible = False
                'Package.Enabled = True
                strLastMenuID = (objMenu.GetLastMenuID()) + 1
                intLastParent = (objMenu.GetLastParent()) + 1
                txtMenuID.Enabled = False
                txtMenuID.Text = strLastMenuID
                txtMenuParent.Enabled = False
                txtMenuParent.Text = intLastParent
                txtMenuLevel.Enabled = False
                txtMenuLevel.Text = intLastParent
                cmdReset.Attributes.Add("onclick", "ValidatorReset();")
                '
            ElseIf ViewState("Mode") = "modify" Then
                lblHeader.Text = "Modify Menu Details"
                txtMenuID.Text = ViewState("MenuId")
                cmdReset.Value = "Reset"
                lblAction.Text = "Edit Module details and click save"
                cmdDelete.Enabled = False
                cmdReset.Disabled = True
                Populate()

                ' cmdReset.Attributes.Add("onclick", "CustomReset();")
                'cmdSave.Attributes.Add("onclick", "return rolechangedsaveconfirm();")
                'cmdDelete.Attributes.Add("onclick", "return confirm('" & MsgForDeleteButton & "');")
                'txUsrGrpName.Attributes.Add("onchange", "onchange();")
                'ddlAdd.Attributes.Add("onchange", "onchange(ddladd.selectedvalue);")
                'cboType.Attributes.Add("onchange", "onchange();")
            End If
        End If
        lnkBack.NavigateUrl = dDispatcher.direct("Global", "MenuMtn.aspx")
    End Sub
    Sub GenerateTab()
        If ViewState("Mode") = "add" Then
            Session("MenuAdd_tabs") = "<div class=""t_entity""><ul>" & _
                   "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Global", "MenuAdd.aspx", "mode=add") & """><span>Add New Module</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                       "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Global", "MenuAddNode.aspx", "mode=add") & """><span>Add New Node</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Global", "MenuAccessRight.aspx", "mode=add") & """><span>Access Right</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                      "</ul><div></div></div>"
       
        End If


    End Sub
    Private Sub Populate()
        Dim objMenu As New MenuSeq
        Dim dvMenu As New DataView

        dvMenu = objMenu.GetMenuDetails(ViewState("MenuId"))
        txtMenuName.Text = dvMenu.Table.Rows(0)("MM_MENU_NAME")
        txtMenuImg.Text = dvMenu.Table.Rows(0)("MM_MENU_IMAGE")
        txtMenuParent.Text = dvMenu.Table.Rows(0)("MM_MENU_PARENT")
        txtMenuURL.Text = dvMenu.Table.Rows(0)("MM_MENU_URL")
        txtMenuLevel.Text = dvMenu.Table.Rows(0)("MM_MENU_LEVEL")


        txtMenuParent.Enabled = False
        txtMenuLevel.Enabled = False
        

       
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Call UpdateMenu()
    End Sub

    Private Sub EnableInput(ByVal pBoo As Boolean)
        'cboScreen.Enabled = Not pBoo
        txUsrGrpName.Enabled = pBoo
        cboRole.Enabled = pBoo
    End Sub
 

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
        dvViewDept.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewDept.Sort += " DESC"
        ''End If

        ''//these only needed if you can select a grid item and click delete button
        ''//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        ''//then user delete one record. //total record = 20 (2 pages), 
        ''//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If ViewState("action") = "del" Then
            If dgAR.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dgAR.PageSize = 0 Then
                dgAR.CurrentPageIndex = dgAR.CurrentPageIndex - 1
                ViewState("action") = ""
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
            'dgAR.DataBind()
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
    

    Private Function GetLen(ByVal pstr As String) As Integer
        Return Split(pstr, ",").Length
    End Function
 


    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim strMsg As String
        Dim objMenu As New MenuSeq
        If objMenu.DelModule(ViewState("MenuId")) Then
            'EnableInput(False)
            'cmdDelete.Enabled = False
            'cmdSave.Enabled = False
            'cmdReset.Disabled = True
            'cmdGrant.Enabled = False
            ''cboScreen.Enabled = False

        End If
        'Common.NetMsgbox(Me, objUsrGrp.Message, "UsSearchUsrGrp.aspx", MsgBoxStyle.Information)
        Common.NetMsgbox(Me, objMenu.Message, dDispatcher.direct("Global", "MenuAdd.aspx", ""), MsgBoxStyle.Information)

    End Sub




    Private Sub UpdateMenu()
        Dim objMenu As New MenuSeq
        Dim dvMenuDetails As New DataView
        Dim ds As New DataSet
        Dim strLastMenuID As String

        Dim dtMenuMaster As New DataTable
        dtMenuMaster.Columns.Add("MenuID", Type.GetType("System.Int32"))
        dtMenuMaster.Columns.Add("MenuName", Type.GetType("System.String"))
        dtMenuMaster.Columns.Add("MenuImg", Type.GetType("System.String"))
        dtMenuMaster.Columns.Add("MenuLvl", Type.GetType("System.String"))
        dtMenuMaster.Columns.Add("MenuParent", Type.GetType("System.Double"))
        dtMenuMaster.Columns.Add("MenuURL", Type.GetType("System.String"))
        dtMenuMaster.Columns.Add("MenuImgExp", Type.GetType("System.String"))
        dtMenuMaster.Columns.Add("MenuTips", Type.GetType("System.String"))
        dtMenuMaster.Columns.Add("MenuGroup", Type.GetType("System.String"))
        dtMenuMaster.Columns.Add("MenuTarget", Type.GetType("System.String"))
        dtMenuMaster.Columns.Add("MenuLastNode", Type.GetType("System.String"))
        dtMenuMaster.Columns.Add("MenuIdx", Type.GetType("System.Double"))


        Dim dtr As DataRow
        dtr = dtMenuMaster.NewRow()
        dtr("MenuID") = txtMenuID.Text
        dtr("MenuName") = txtMenuName.Text
        dtr("MenuImg") = txtMenuImg.Text
        dtr("MenuLvl") = txtMenuLevel.Text
        dtr("MenuParent") = txtMenuParent.Text
        dtr("MenuURL") = txtMenuURL.Text  'DBNull.Value
        dtr("MenuImgExp") = "" 'DBNull.Value
        dtr("MenuTips") = "" 'DBNull.Value
        dtr("MenuGroup") = "ehub"
        dtr("MenuTarget") = "body"
        dtr("MenuLastNode") = "" 'DBNull.Value
        dtr("MenuIdx") = txtMenuParent.Text

        dtMenuMaster.Rows.Add(dtr)
        ds.Tables.Add(dtMenuMaster)

        If ViewState("Mode") = "add" Then
            If objMenu.AddNewModule(ds) Then
                'txtUsrGrpId.Enabled = False
                'txUsrGrpName.Enabled = False
                'cboRole.Enabled = False
                'cmdGrant.Visible = True
                'Common.NetMsgbox(Me, objUsrGrp.Message, "UsUsrGrp.aspx?mode=modify&usrgrpid=" & Server.UrlEncode(objUsrGrpDetails.Id) & "&Package=" & Server.UrlEncode(objUsrGrpDetails.Package), MsgBoxStyle.Information)
                Common.NetMsgbox(Me, objMenu.Message, dDispatcher.direct("Global", "MenuAdd.aspx", MsgBoxStyle.Information))

            Else
                Common.NetMsgbox(Me, objMenu.Message, MsgBoxStyle.Information)
            End If

        ElseIf ViewState("Mode") = "modify" Then
            If objMenu.UpdateModule(ds) Then

                ' If onchange.Value = "1" Then
                Common.NetMsgbox(Me, objMenu.Message, dDispatcher.direct("Global", "MenuAdd.aspx", ""), MsgBoxStyle.Information)

            Else
                'Common.NetMsgbox(Me, objUsrGrp.Message, "UsSearchUsrGrp.aspx", MsgBoxStyle.Information)
                Common.NetMsgbox(Me, objMenu.Message, MsgBoxStyle.Information)
            End If
        End If

    End Sub

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        cmdReset.Value = "Reset"
        cmdGrant.Visible = True
        cmdDelete.Visible = True
        cboPackage.Enabled = False

        tblButton.Style.Item("Display") = ""
        'txUsrGrpName.Enabled = True
        'cboRole.Enabled = True
        'cboType.Enabled = True

        dgAR.DataSource = Nothing
        ' dgAR.DataBind()

        'cmdSaveAR.Visible = False
        cmdCancel.Visible = False
    End Sub


End Class
