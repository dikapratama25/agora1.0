Imports AgoraLegacy
Imports eProcure.Component
Imports Microsoft.Web.UI.WebControls
Imports System
Imports System.Security.Cryptography
Imports System.Text
Public Class Menu
    Inherits System.Web.UI.Page
    Protected WithEvents TreeView1 As Microsoft.Web.UI.WebControls.TreeView
    Protected WithEvents Panel1 As System.Web.UI.WebControls.Panel
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents cboComp As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lbl1 As System.Web.UI.WebControls.Label
    Protected WithEvents tree As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents imgMenu As System.Web.UI.HtmlControls.HtmlImage
    Public modName As String = "ehub"
    Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents pnlIcon As System.Web.UI.WebControls.Panel
    Protected WithEvents imgHome As System.Web.UI.HtmlControls.HtmlImage
    Protected WithEvents tblIcon As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tcHome As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents hidControl As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lblIcon As System.Web.UI.WebControls.Label
    Protected WithEvents lnkbilling1 As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents lbRN As System.Web.UI.WebControls.LinkButton

    Dim objDb As New EAD.DBCom
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
        '===bind tree
        Dim strConn As String = ConfigurationManager.AppSettings("Path")

        If CStr(Session("UserId")) = String.Empty Then
            Dim vbs As String
            vbs = vbs & "<script language=""vbs"">"
            'vbs = vbs & "Call MsgBox(""" & msg & """, " & style & ", """ & title & """)"
            vbs = vbs & vbLf & "parent.location=""../../../eProcurement/Common/Initial/Unauthorized.aspx"""
            vbs = vbs & "</script>"
            Dim rndKey As New Random
            Me.RegisterStartupScript(rndKey.Next.ToString, vbs)
        Else
            Dim Menux As New MenuEAD(TreeView1, strConn)
            Menux.BindCollectionToTreeView("AND MM_GROUP='ehub'")

            CutTree(Menux)
            'Any special condition that need to cut the tree
            ConditionCut(Menux)
            'AddMenuItem()
            '===bind tree
            imgMenu.Attributes.Add("Onclick", "Display()") ''''''''''
            imgMenu.Src = dDispatcher.direct("Plugins/Images", "m_hide.jpg")
        End If

        'Michelle (4/5/2012) - Issue 1604
        'Dim objUserRoles As New UserRoles
        'Dim UserRole = objUserRoles.get_UserFixedRole()
        'If UserRole = "Super Admin" Then
        '    GenerateLinkToBilling()
        'End If
        Dim objUsers As New Users
        If objUsers.checkUserFixedRole("'SUPER ADMIN'") Then
            GenerateLinkToBilling()
        End If

        genHeader()
        GenerateUIMenu()
        GenerateReleaseNote()


        'genHeader2()
    End Sub
    Private Sub GenerateLinkToBilling()
        Dim user As String
        Dim lnkbilling As String
        user = UCase(Session("UserID"))
        user &= Replace(Date.Today, "/", "")
        user = getMd5Hash(user)
        Dim url As String = System.Configuration.ConfigurationManager.AppSettings("BillingURL") & "/?user=" & user & "&uid=" & Session("UserID") & "&cid=" & Session("CompanyID")
        lnkbilling = "<div id=""lnkbilling"" class=""main_menu"" onclick=""top.location.href='" & url & "'"" style=""background:#5C87B2;font-weight:bold;display"">Billing</div>"
        Session("lnkbilling") = lnkbilling
    End Sub

    Private Sub GenerateUIMenu()
        Dim iParent As Integer, iChild As Integer, iArrayCounter As Integer = 0
        Dim sMenu As String = "", sPara As New ArrayList
        For iParent = 0 To TreeView1.Nodes.Count - 1

            If TreeView1.Nodes.Count = 1 Then
                sMenu = sMenu & "<div class=""main_menu"" id=""mmenu" & TreeView1.Nodes.Item(iParent).ID & """ onclick=""showHideMenu('mmenu" & TreeView1.Nodes.Item(iParent).ID & "', 'smenu" & TreeView1.Nodes.Item(iParent).ID & "')"">" & TreeView1.Nodes.Item(iParent).Text & "</div>"
                sMenu = sMenu & "<div id=""smenu" & TreeView1.Nodes.Item(iParent).ID & """>"

            ElseIf iParent = TreeView1.Nodes.Count - 1 Then
                sMenu = sMenu & "<div class=""main_menu_drop"" id=""mmenu" & TreeView1.Nodes.Item(iParent).ID & """ onclick=""showHideMenu('mmenu" & TreeView1.Nodes.Item(iParent).ID & "', 'smenu" & TreeView1.Nodes.Item(iParent).ID & "')"">" & TreeView1.Nodes.Item(iParent).Text & "</div>"                
                sMenu = sMenu & "<div id=""smenu" & TreeView1.Nodes.Item(iParent).ID & """ style=""display:none;"" >"

            Else
                sMenu = sMenu & "<div class=""main_menu"" id=""mmenu" & TreeView1.Nodes.Item(iParent).ID & """ onclick=""showHideMenu('mmenu" & TreeView1.Nodes.Item(iParent).ID & "', 'smenu" & TreeView1.Nodes.Item(iParent).ID & "')"">" & TreeView1.Nodes.Item(iParent).Text & "</div>"
                sMenu = sMenu & "<div id=""smenu" & TreeView1.Nodes.Item(iParent).ID & """>"
            End If
            iArrayCounter = iArrayCounter + 1
            For iChild = 0 To TreeView1.Nodes.Item(iParent).Nodes.Count - 1
                'sMenu = sMenu & "<div class=""sub_menu""><a target=""body"" class="""" href=""" & TreeView1.Nodes.Item(iParent).Nodes.Item(iChild).NavigateUrl & """>" & TreeView1.Nodes.Item(iParent).Nodes.Item(iChild).Text & "</a></div>"
                sPara = dDispatcher.splitter(TreeView1.Nodes.Item(iParent).Nodes.Item(iChild).NavigateUrl)
                sMenu = sMenu & "<div class=""sub_menu""><a target=""body"" class="""" href=""" & dDispatcher.direct(sPara(0), sPara(1), sPara(2)) & """>" & TreeView1.Nodes.Item(iParent).Nodes.Item(iChild).Text & "</a></div>"
                iArrayCounter = iArrayCounter + 1
            Next
            sMenu = sMenu & "</div>"
        Next
        sMenu = sMenu & Session("lnkbilling")
        Session("w_menu") = "<div id=""menu_box"">" & sMenu & "</div>"
    End Sub
    Private Sub GenerateReleaseNote()

        Dim RNVersionFile As String
        RNVersionFile = Server.MapPath("../Version/ReleaseNoteVersion.txt")
        If System.IO.File.Exists(RNVersionFile) = True Then
            'Read the textfile:
            Dim objReader As New System.IO.StreamReader(RNVersionFile)
            Dim FileLine As String = objReader.ReadToEnd
            objReader.Close()

            Dim Arg() As String = Split(FileLine, ",")    'Split file contents at *
            Dim strRNVersion As String = Arg(0) 'First part

            lbRN.Text = strRNVersion
            lbRN.Attributes.Add("onClick", "window.open('" & dDispatcher.direct("Initial", "DialogPDF.aspx") & "',  '', 'resizable=yes,scrollbars=yes,width=800,height=600,status=no,menubar=no');return false;")
        End If

    End Sub

    Public Function CutTree(ByRef pMenu As MenuEAD) As Boolean
        'Dim ObjSec As New Security

        Dim objCompany As New Companies
        Dim objUsers As New Users
        Dim strSQL, strBACan As String

        'Michelle (6/2/2010) - Check whether the BA is allow to access the PO Cancellation Request screen
        strBACan = objCompany.GetBACanPOMode(Session("CompanyId"))

        If strBACan = "Y" Then
            strSQL = "SELECT MM_MENU_ID,  CASE WHEN SUM(CNTview)=0 THEN 'N' ELSE 'Y' END  as sumView " &
                     "FROM(  SELECT       MM_MENU_ID ,      case when UAR_allow_view='Y' then 1 else 0 end as cntView  " &
                     "FROM     MENU_MSTR M,     USER_MSTR U,    USER_ACCESS_RIGHT R ,USERS_USRGRP G " &
                     "WHERE M.MM_GROUP='ehub' " &
                     "AND    U.UM_USER_ID=G.UU_USER_ID " &
                     "AND    UU_USRGRP_ID=R.UAR_USRGRP_ID " &
                     "AND MM_MENU_ID=UAR_MENU_ID  " &
                     "AND (UM_COY_ID='" & Session("CompanyId") & "' " &
                     "AND UU_COY_ID='" & Session("CompanyId") & "') " &
                     "AND G.uu_USER_ID='" & Session("UserID") & "')a " &
                     "GROUP BY MM_MENU_ID  " &
                     "ORDER BY  MM_MENU_ID "
        ElseIf objUsers.BAdminRole(Session("UserID"), Session("CompanyId")) Then
            strSQL = "SELECT MM_MENU_ID,  CASE WHEN SUM(CNTview)=0 THEN 'N' ELSE 'Y' END  as sumView " &
                     "FROM(  SELECT       MM_MENU_ID ,      case when UAR_allow_view='Y' then 1 else 0 end as cntView  " &
                     "FROM     MENU_MSTR M,     USER_MSTR U,    USER_ACCESS_RIGHT R ,USERS_USRGRP G " &
                     "WHERE M.MM_GROUP='ehub' " &
                     "AND    U.UM_USER_ID=G.UU_USER_ID  AND M.MM_MENU_NAME <> 'PO Cancellation Request' " &
                     "AND    UU_USRGRP_ID=R.UAR_USRGRP_ID " &
                     "AND MM_MENU_ID=UAR_MENU_ID  " &
                     "AND (UM_COY_ID='" & Session("CompanyId") & "' " &
                     "AND UU_COY_ID='" & Session("CompanyId") & "') " &
                     "AND G.uu_USER_ID='" & Session("UserID") & "')a " &
                     "GROUP BY MM_MENU_ID  " &
                     "ORDER BY  MM_MENU_ID "
        Else
            strSQL = "SELECT MM_MENU_ID,  CASE WHEN SUM(CNTview)=0 THEN 'N' ELSE 'Y' END  as sumView " &
                     "FROM(  SELECT       MM_MENU_ID ,      case when UAR_allow_view='Y' then 1 else 0 end as cntView  " &
                     "FROM     MENU_MSTR M,     USER_MSTR U,    USER_ACCESS_RIGHT R ,USERS_USRGRP G " &
                     "WHERE M.MM_GROUP='ehub' " &
                     "AND    U.UM_USER_ID=G.UU_USER_ID " &
                     "AND    UU_USRGRP_ID=R.UAR_USRGRP_ID " &
                     "AND MM_MENU_ID=UAR_MENU_ID  " &
                     "AND (UM_COY_ID='" & Session("CompanyId") & "' " &
                     "AND UU_COY_ID='" & Session("CompanyId") & "') " &
                     "AND G.uu_USER_ID='" & Session("UserID") & "')a " &
                     "GROUP BY MM_MENU_ID  " &
                     "ORDER BY  MM_MENU_ID "
        End If

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

        strSQL = " SELECT MM_MENU_ID FROM MENU_MSTR WHERE MM_MENU_ID NOT IN " & strMenu &
                 " AND MM_GROUP='" & modName & "'"

        tDS = objDb.FillDs(strSQL)

        For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
            pMenu.DeleteNode(tDS.Tables(0).Rows(j).Item(0))
        Next
    End Function

    Public Function ConditionCut(ByRef pMenu As MenuEAD) As Boolean
        Dim objComp As New Companies
        Dim objCompDetails As New Company

        objCompDetails = objComp.GetCompanyDetails(Session("CompanyId"))
        If objCompDetails.BCMSetting = "0" Then
            pMenu.DeleteNode("60")
        End If

        'Zulham 05072018 - PAMB
        '-Remove BranchCode menus
        If objCompDetails.CoyId.ToUpper = "PAMB" Then
            pMenu.DeleteNode("126")
            pMenu.DeleteNode("150")
			pMenu.DeleteNode("151")
        End If

    End Function

    Public Function AddMenuItem() As Boolean
        Dim itemNode As New TreeNode
        itemNode.Text = "Logout"
        itemNode.ImageUrl = dDispatcher.direct("Plugins/Images", "i_logout.gif")
        itemNode.NodeData = "Menu.MM_MENU_TIPS"
        itemNode.Target = "_top"
        itemNode.NavigateUrl = dDispatcher.direct("Initial", "SignOut.aspx")
        TreeView1.Nodes.Add(itemNode)

    End Function

    Private Sub genHeader() 'optional header
        Dim packageName() As String = {"eProcure", "eRFP", "eAuction", "eContract"}
        Dim authPackageName() As String
        Dim authPackageID() As String
        Dim authPackageUrl() As String
        Dim totalpackage As Integer
        Dim j As Integer
        Dim strOutput As String = ""

        authPackageName = Request.Cookies("aPackageName").Value.Split("|")
        authPackageUrl = Request.Cookies("aPackageUrl").Value.Split("|")
        authPackageID = Request.Cookies("aPackageID").Value.Split("|")
        totalpackage = Request.Cookies("aTotalPackage").Value

        strOutput &= "<a target='body' href ='" & dDispatcher.direct("Initial", "Homepage.aspx") & "'>"
        strOutput &= "<img border=0 class='menu_icon' src='" & dDispatcher.direct("Plugins/Images", "m_home.jpg") & "' style='width:66px; height:24px; '"
        strOutput &= "alt='Click here to go to welcome page.'>"
        strOutput &= "</a>"
        If totalpackage > 1 Then
            For j = 0 To authPackageName.Length - 1
                strOutput &= "<a target='_parent' href =" & authPackageUrl(j) & ">"
                strOutput &= "<img border=0 src='images\" & authPackageID.GetValue(j) & "Button.gif' "
                strOutput &= "alt='Click here to go to " & authPackageName.GetValue(j) & "'>"
                strOutput &= "</a>"
            Next
        End If
        strOutput &= "<a target='_top' href ='SignOut.aspx'>"
        strOutput &= "<img border=0 class='menu_icon' src='" & dDispatcher.direct("Plugins/Images", "m_Logout_red.png") & "' style='width:66px; height:24px; '"
        strOutput &= "alt='Logout'>"
        strOutput &= "</a><div style='clear:both;'></div>"
        lblIcon.Text = strOutput '''''
        'lbl1.Text = "<hr>"

    End Sub

    Private Sub redirectPage(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'Select Case viewstate("type")
        '    Case "cart", "rfq"
        '        objPR.deletePRAttachment(CType(sender, ImageButton).ID)
        '    Case "list"
        '        objPR.deletePRAttachment(CType(sender, ImageButton).ID)
        'End Select
        'displayAttachFile()
        'objPR = Nothing
        Response.Redirect("")
    End Sub
    Function getMd5Hash(ByVal input As String) As String
        ' Create a new instance of the MD5 object.
        Dim md5Hasher As MD5 = MD5.Create()
        ' Convert the input string to a byte array and compute the hash.
        Dim data As Byte() = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input))
        ' Create a new Stringbuilder to collect the byte
        ' and create a string.
        Dim sBuilder As New StringBuilder()
        ' Loop through each byte of the hashed data
        ' and format each one as a hexadecimal string.
        Dim i As Integer
        For i = 0 To data.Length - 1
            sBuilder.Append(data(i).ToString("x2"))
        Next i
        ' Return the hexadecimal string.
        Return sBuilder.ToString()
    End Function
End Class
