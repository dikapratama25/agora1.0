Imports AgoraLegacy
Imports Microsoft.Web.UI.WebControls
Imports SSO.Component
Imports MySql.Data.MySqlClient
Public Class Menu
    Inherits System.Web.UI.Page
    Protected WithEvents TreeView1 As Microsoft.Web.UI.WebControls.TreeView
    Protected WithEvents Panel1 As System.Web.UI.WebControls.Panel
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    'Protected WithEvents cboComp As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents pnlComp As System.Web.UI.WebControls.Panel
    Protected WithEvents imgMenu As System.Web.UI.HtmlControls.HtmlImage
    Dim objDb As New  EAD.DBCom
    Protected WithEvents lbl1 As System.Web.UI.WebControls.Label
    Protected WithEvents label As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents tree As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents lblIcon As System.Web.UI.WebControls.Label
    Protected WithEvents btnSearch As System.Web.UI.WebControls.Button
    Protected WithEvents txtCompany As System.Web.UI.WebControls.TextBox
    ' Protected WithEvents txtCompany1 As System.Web.UI.HtmlControls.HtmlInputText

    Protected WithEvents cmd_New As System.Web.UI.WebControls.Button
    'Protected WithEvents cmd_New As System.Web.UI.WebControls.Button
    Public ModName As String = "hubadmin"

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
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ViewState("Mode") = Request.Params("mode")

        ''<cc1:AutoCompleteExtender runat="server" ID="autoComplete1" TargetControlID="txtCompany" ServiceMethod="GetCompany" ServicePath="AutoComplete/AutoComplete.asmx" MinimumPrefixLength="0" CompletionSetCount="10"></cc1:AutoCompleteExtender>
        'autoComplete1.ServicePath()
        If CStr(Session("UserId")) = String.Empty Then
            Dim vbs As String
            vbs = vbs & "<script language=""vbs"">"
            'vbs = vbs & "Call MsgBox(""" & msg & """, " & style & ", """ & title & """)"
            vbs = vbs & vbLf & "parent.location=""" & dDispatcher.direct("Initial", "Unauthorized.aspx") & """"
            vbs = vbs & "</script>"
            Dim rndKey As New Random
            Me.RegisterStartupScript(rndKey.Next.ToString, vbs)
        Else
            If Not Page.IsPostBack Then
                '### Default Company Value ###
                Dim ds As New DataSet
                Dim objPO As New Companies
                ds = objPO.getAllCompanyTypeAhead("y")
                Dim CoyName As String = ds.Tables("CompanyListName").Rows(0).Item("CM_COY_NAME")
                txtCompany.Text = CoyName






                '===bind tree
                Dim strConn As String = ConfigurationSettings.AppSettings("Path")
                Dim Menux As New MenuEAD.EAD.MenuEAD(TreeView1, strConn)
                Menux.BindCollectionToTreeView("AND MM_GROUP='hubadmin'")
                CutTree(Menux)
                'AddMenuItem()
                '===bind tree

                'BindCompanay()
                'select first company if more than 0 comp found
                ''If cboComp.Items.Count > 1 Then
                ''    cboComp.SelectedIndex = 1
                ''Else
                ''    cboComp.SelectedIndex = 0
                ''    Session("CompanyIdToken") = ""
                ''End If
                Dim objComp As New Companies
                'txtCompany.Text = txtCompany1.Value




                Session("CompanyIdToken") = objComp.GetCompanyID(txtCompany.Text)
                'Session("CompanyIdToken") = cboComp.SelectedValue
                imgMenu.Attributes.Add("Onclick", "Display()")
                imgMenu.Src = dDispatcher.direct("Plugins/Images", "m_back.jpg")


                If ViewState("Mode") = "show" Then
                    'cboComp.SelectedIndex = 0
                    txtCompany.Text = ""

                    TreeView1.Visible = False
                    Session("CompanyIdToken") = ""
                    Response.Write("<script>window.parent.frames['body'].location='" & dDispatcher.direct("Companies", "coCompanyDetail.aspx", "mode=add") & "';</script>")

                    'Dim strscript As New System.Text.StringBuilder
                    'strscript.Append("<script language=""javascript"">")
                    'strscript.Append("Display();")
                    'strscript.Append("Display();")
                    'strscript.Append("</script>")
                    'RegisterStartupScript("script3", strscript.ToString())
                    'viewstate("Mode") = ""
                End If
            End If
            If Session("CompanyIdToken") = "" Then
                Session("CompanyIdToken") = Session("CompanyIdBkup")
            End If
        End If
        genHeader()
        GenerateUIMenu()
        'txtCompany.Attributes.Add("onChange", "submit();")
    End Sub

    Private Sub genHeader() 'optional header
        'Dim packageName() As String = {"eProcure", "eRFP", "eAuction", "eContract"}
        'Dim authPackageName() As String
        'Dim authPackageID() As String
        'Dim authPackageUrl() As String
        'Dim totalpackage As Integer
        'Dim j As Integer
        Dim strOutput As String = ""

        'authPackageName = Request.Cookies("aPackageName").Value.Split("|")
        'authPackageUrl = Request.Cookies("aPackageUrl").Value.Split("|")
        'authPackageID = Request.Cookies("aPackageID").Value.Split("|")
        'totalpackage = Request.Cookies("aTotalPackage").Value
        ''' ----- Remark end for icons with description displayed together -----
        strOutput &= "<a target='body' href ='" & dDispatcher.direct("Initial", "Homepage.aspx") & "'>"
        strOutput &= "<img border=0 class='menu_icon' src='" & dDispatcher.direct("Plugins/Images", "m_home.jpg") & "' style='width:66px; height:24px; '"
        strOutput &= "alt='Click here to go to welcome page.'>"
        strOutput &= "</a>"
        'If totalpackage > 1 Then
        '    For j = 0 To authPackageName.Length - 1
        '        strOutput &= "<a target='_parent' href =" & authPackageUrl(j) & ">"
        '        strOutput &= "<img border=0 src='images\" & authPackageID.GetValue(j) & "Button.gif' "
        '        strOutput &= "alt='Click here to go to " & authPackageName.GetValue(j) & "'>"
        '        strOutput &= "</a>"
        '    Next
        'End If
        strOutput &= "<a target='_top' href ='" & dDispatcher.direct("Initial", "Login.aspx") & "'>"
        strOutput &= "<img border=0 class='menu_icon' src='" & dDispatcher.direct("Plugins/Images", "m_logout.png") & "' style='width:66px; height:24px; '"
        strOutput &= "alt='Logout'>"
        'strOutput &= "</a><div style='clear:both;'></div>"
        strOutput &= "</a><div style='clear:both;'></div>"
        lblIcon.Text = strOutput '''''
        'lbl1.Text = "<hr>"

    End Sub

    Private Sub GenerateUIMenu()
        Dim iParent As Integer, iChild As Integer, iArrayCounter As Integer = 0
        Dim sMenu As String = "", sPara As New ArrayList
        For iParent = 0 To TreeView1.Nodes.Count - 1
            
            'If iParent = 0 Then
            '    sMenu = sMenu & "<div class=""main_menu_drop"" id=""mmenu" & TreeView1.Nodes.Item(iParent).ID & """ onclick=""showHide('mmenu" & TreeView1.Nodes.Item(iParent).ID & "', 'smenu" & TreeView1.Nodes.Item(iParent).ID & "')"">" & TreeView1.Nodes.Item(iParent).Text & "</div>"
            '    sMenu = sMenu & "<div id=""smenu" & TreeView1.Nodes.Item(iParent).ID & """>"
            'Else
            '    sMenu = sMenu & "<div class=""main_menu"" id=""mmenu" & TreeView1.Nodes.Item(iParent).ID & """ onclick=""showHide('mmenu" & TreeView1.Nodes.Item(iParent).ID & "', 'smenu" & TreeView1.Nodes.Item(iParent).ID & "')"">" & TreeView1.Nodes.Item(iParent).Text & "</div>"
            '    sMenu = sMenu & "<div id=""smenu" & TreeView1.Nodes.Item(iParent).ID & """ style=""display:none;"" >"
            If TreeView1.Nodes.Count = 1 Then
                sMenu = sMenu & "<div class=""main_menu"" id=""mmenu" & TreeView1.Nodes.Item(iParent).ID & """ onclick=""showHide('mmenu" & TreeView1.Nodes.Item(iParent).ID & "', 'smenu" & TreeView1.Nodes.Item(iParent).ID & "')"">" & TreeView1.Nodes.Item(iParent).Text & "</div>"
                sMenu = sMenu & "<div id=""smenu" & TreeView1.Nodes.Item(iParent).ID & """>"
            ElseIf iParent = TreeView1.Nodes.Count - 1 Then
                sMenu = sMenu & "<div class=""main_menu_drop"" id=""mmenu" & TreeView1.Nodes.Item(iParent).ID & """ onclick=""showHide('mmenu" & TreeView1.Nodes.Item(iParent).ID & "', 'smenu" & TreeView1.Nodes.Item(iParent).ID & "')"">" & TreeView1.Nodes.Item(iParent).Text & "</div>"
                sMenu = sMenu & "<div id=""smenu" & TreeView1.Nodes.Item(iParent).ID & """ style=""display:none;"" >"
            Else
                sMenu = sMenu & "<div class=""main_menu"" id=""mmenu" & TreeView1.Nodes.Item(iParent).ID & """ onclick=""showHide('mmenu" & TreeView1.Nodes.Item(iParent).ID & "', 'smenu" & TreeView1.Nodes.Item(iParent).ID & "')"">" & TreeView1.Nodes.Item(iParent).Text & "</div>"
                sMenu = sMenu & "<div id=""smenu" & TreeView1.Nodes.Item(iParent).ID & """>"
            End If
            'End If
            iArrayCounter = iArrayCounter + 1
            For iChild = 0 To TreeView1.Nodes.Item(iParent).Nodes.Count - 1
                sPara = dDispatcher.splitter(TreeView1.Nodes.Item(iParent).Nodes.Item(iChild).NavigateUrl)
                sMenu = sMenu & "<div class=""sub_menu""><a target=""body"" class="""" href=""" & dDispatcher.direct(sPara(0), sPara(1), sPara(2)) & """>" & TreeView1.Nodes.Item(iParent).Nodes.Item(iChild).Text & "</a></div>"
                iArrayCounter = iArrayCounter + 1
            Next
            sMenu = sMenu & "</div>"
        Next
        Session("w_menu") = "<div id=""menu_box"">" & sMenu & "</div>"
    End Sub

    Public Function CutTree(ByRef pMenu As MenuEAD.EAD.MenuEAD) As Boolean
        'Dim ObjSec As New Security

        Dim strSQL As String

        'Michelle (21/1/2011) - To hide some of the menu for FTN and Enterprise version
        'strSQL = "SELECT MM_MENU_ID,  CASE WHEN SUM(CNTview)=0 THEN 'N' ELSE 'Y' END  as sumView " & _
        '             "FROM( SELECT MM_MENU_ID ,case when UAR_allow_view='Y' then 1 else 0 end as cntView  " & _
        '             "FROM  MENU_MSTR M, USER_MSTR U,USER_ACCESS_RIGHT R ,USERS_USRGRP G " & _
        '             "WHERE M.MM_GROUP='hubadmin' " & _
        '             "AND U.UM_USER_ID=G.UU_USER_ID " & _
        '             "AND UU_USRGRP_ID=R.UAR_USRGRP_ID " & _
        '             "AND MM_MENU_ID=UAR_MENU_ID  " & _
        '             "AND (UM_COY_ID='" & Session("CompanyId") & "' " & _
        '             "AND UU_COY_ID='" & Session("CompanyId") & "') " & _
        '             "AND G.uu_USER_ID='" & Session("UserID") & "')a " & _
        '             "GROUP BY MM_MENU_ID  " & _
        '             "ORDER BY  MM_MENU_ID "

        strSQL = "SELECT MM_MENU_ID,  CASE WHEN SUM(CNTview)=0 THEN 'N' ELSE 'Y' END  as sumView " & _
                         "FROM( SELECT MM_MENU_ID ,case when UAR_allow_view='Y' then 1 else 0 end as cntView  " & _
                         "FROM  MENU_MSTR M, USER_MSTR U,USER_ACCESS_RIGHT R ,USERS_USRGRP G " & _
                         "WHERE M.MM_GROUP='hubadmin' " & _
                         "AND U.UM_USER_ID=G.UU_USER_ID " & _
                         "AND UU_USRGRP_ID=R.UAR_USRGRP_ID " & _
                         "AND MM_MENU_ID=UAR_MENU_ID  "
        If Session("Env") = "FTN" Then
            strSQL &= "AND MM_ENV = 'F' " 'ie for FTN
        Else
            strSQL &= "AND MM_ENV <> 'D' " 'ie for non-FTN and exclude those that are being deffered or currently not working
        End If
        strSQL &= "AND (UM_COY_ID='" & Session("CompanyId") & "' " & _
                  "AND UU_COY_ID='" & Session("CompanyId") & "') " & _
                  "AND G.uu_USER_ID='" & Session("UserID") & "')a " & _
                  "GROUP BY MM_MENU_ID  " & _
                  "ORDER BY  MM_MENU_ID "

        Dim tDS As DataSet = objDb.FillDs(strSQL)
        Dim strMenu As String = "("
        For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
            If tDS.Tables(0).Rows(j).Item(1) = "Y" Then
                strMenu &= "'" & tDS.Tables(0).Rows(j).Item(0) & "',"
            End If
        Next
        If strMenu = "(" Then
            strMenu = "(' ')"
        Else
            strMenu = Mid(strMenu, 1, strMenu.Length - 1)
            strMenu &= ")"
        End If

        strSQL = " SELECT MM_MENU_ID FROM MENU_MSTR WHERE MM_MENU_ID NOT IN " & strMenu & _
                 " AND MM_GROUP='" & ModName & "'"

        tDS = objDb.FillDs(strSQL)
        For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
            pMenu.DeleteNode(tDS.Tables(0).Rows(j).Item(0))
        Next

    End Function

    'Private Sub cboComp_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboComp.SelectedIndexChanged


    '    If cboComp.SelectedIndex <> 0 Then
    '        TreeView1.Visible = True
    '        Session("CompanyIdToken") = cboComp.SelectedValue
    '        'Dim strscript As New System.Text.StringBuilder
    '        'strscript.Append("<script language=""javascript"">")
    '        'strscript.Append("parent.frames['body'].location='Homepage.aspx';")
    '        ''strscript.Append("Display();")
    '        'strscript.Append("</script>")
    '        'RegisterStartupScript("script3", strscript.ToString())
    '        Response.Write("<script>window.parent.frames['body'].location='homepage.aspx';</script>")

    '    Else
    '        TreeView1.Visible = False
    '        Session("CompanyIdToken") = ""
    '        'Dim strscript1 As New System.Text.StringBuilder
    '        'strscript1.Append("<script language=""javascript"">")
    '        'strscript1.Append("parent.frames['body'].location='Companies/coCompanyDetail.aspx?mode=add';")
    '        ''strscript1.Append("Display();")
    '        'strscript1.Append("</script>")
    '        'RegisterStartupScript("script3", strscript1.ToString())
    '        Response.Write("<script>window.parent.frames['body'].location='Companies/coCompanyDetail.aspx?mode=add';</script>")
    '    End If
    'End Sub
    'Private Sub BindCompanay()
    '    Dim objComp As New Companies
    '    cboComp.Items.Clear()
    '    Common.FillDdl(cboComp, "CM_COY_NAME", "CM_COY_ID", objComp.GetAllCompany())

    '    Dim dvComp As DataView
    '    dvComp = objComp.GetAllCompany()

    '    'dvComp.ToTable.Rows(0)(1).ToString()

    '    cboComp.Items.Insert(0, "[New Company]..")
    'End Sub


    Public Function AddMenuItem() As Boolean
        Dim itemNode As New TreeNode
        itemNode.Text = "Logout"
        itemNode.ImageUrl = dDispatcher.direct("Plugins/Images", "i_logout.gif")
        itemNode.NodeData = "Menu.MM_MENU_TIPS"
        itemNode.Target = "_top"
        itemNode.NavigateUrl = dDispatcher.direct("Initial", "Login.aspx")
        TreeView1.Nodes.Add(itemNode)

    End Function

    Public Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim objComp As New Companies
        Dim strDelStatus As String
        ' txtCompany.Text = txtCompany1.Value
        If txtCompany.Text <> "" Then
            If objComp.GetCompanyDeletedStatus(txtCompany.Text) = "Y" Then
                Common.NetMsgbox(Me, "Company record has been deleted.", MsgBoxStyle.Information)
            Else
                Session("CompanyIdToken") = objComp.GetCompanyID(txtCompany.Text)
                Response.Write("<script>window.parent.frames['body'].location='" & dDispatcher.direct("Companies", "coCompanyDetail.aspx", "mode=modify") & "';</script>")
                'Response.Write("<script>window.parent.frames['body'].location='homepage.aspx';</script>")
            End If
        Else
            Session("CompanyIdToken") = ""
            Response.Write("<script>window.parent.frames['body'].location='" & dDispatcher.direct("Companies", "coCompanyDetail.aspx", "mode=add") & "';</script>")
        End If
    End Sub

    Private Sub cmd_New_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_New.Click
        TreeView1.Visible = False
        Session("CompanyIdBkup") = Session("CompanyIdToken")
        Session("CompanyIdToken") = ""
        'Response.Write("<script>window.parent.frames['body'].location='Companies/coCompanyDetail.aspx?mode=add';</script>")
        Response.Write("<script>window.parent.frames['body'].location='" & dDispatcher.direct("Companies", "coCompanyDetail.aspx", "mode=add") & "';</script>")

    End Sub

    'Protected Sub txtCompany_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCompany.TextChanged
    '    Dim objComp As New Companies
    '    Dim strDelStatus As String
    '    ' txtCompany.Text = txtCompany1.Value
    '    If txtCompany.Text <> "" Then
    '        If objComp.GetCompanyDeletedStatus(txtCompany.Text) = "Y" Then
    '            Common.NetMsgbox(Me, "Company record has been deleted.", MsgBoxStyle.Information)
    '        Else
    '            Session("CompanyIdToken") = objComp.GetCompanyID(txtCompany.Text)
    '            Response.Write("<script>window.parent.frames['body'].location='" & dDispatcher.direct("Companies", "coCompanyDetail.aspx", "mode=modify") & "';</script>")
    '            'Response.Write("<script>window.parent.frames['body'].location='homepage.aspx';</script>")
    '        End If
    '    Else
    '        Session("CompanyIdToken") = ""
    '        Response.Write("<script>window.parent.frames['body'].location='" & dDispatcher.direct("Companies", "coCompanyDetail.aspx", "mode=add") & "';</script>")
    '    End If
    'End Sub
End Class
