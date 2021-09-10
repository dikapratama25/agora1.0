Imports AgoraLegacy
Imports eProcure.Component

Public Class ConCat_AssignUser

    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Protected WithEvents cboCatalogueBuyer As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboBuyer As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents lstBox2 As System.Web.UI.WebControls.ListBox
    Protected WithEvents lstBox4 As System.Web.UI.WebControls.ListBox
    Protected WithEvents cmdReset As System.Web.UI.WebControls.Button
    Protected WithEvents cmdReset2 As System.Web.UI.WebControls.Button
    Protected WithEvents cmdAdd1 As System.Web.UI.WebControls.Button
    Protected WithEvents cmdAdd2 As System.Web.UI.WebControls.Button
    Protected WithEvents cmdAdd3 As System.Web.UI.WebControls.Button
    Protected WithEvents cmdAdd4 As System.Web.UI.WebControls.Button
    Protected WithEvents cmdsave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdsave2 As System.Web.UI.WebControls.Button
    Protected WithEvents lstbox1 As System.Web.UI.WebControls.ListBox
    Protected WithEvents lstbox3 As System.Web.UI.WebControls.ListBox
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblname As System.Web.UI.WebControls.Label
    Protected WithEvents lblcode As System.Web.UI.WebControls.Label
    Protected WithEvents lblSelect As System.Web.UI.WebControls.Label
    Protected WithEvents lbl1 As System.Web.UI.WebControls.Label
    Protected WithEvents cmdItemAsg As System.Web.UI.WebControls.Button
    Protected WithEvents Div_AA As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents Div_BB As System.Web.UI.HtmlControls.HtmlGenericControl

    Protected WithEvents rd1 As System.Web.UI.WebControls.RadioButtonList

    Dim strMode As String
    Dim strRedirect As String
    Dim strMsg As String
    Dim strCatName, v, arr(0) As String
    Private ary1(), ary2() As String
    Dim ds As DataSet
    Dim li, li2 As ListItem
    Dim objDb As New EAD.DBCom
    'Dim objcat As New BuyerCat
    Dim objCat As New ContCat
    Dim valArr1 As String
    Dim val As String
    Dim struri As String
    Dim cbolist As New ListItem
    Dim blnlstboxChg As Boolean


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
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        cmdsave.Enabled = False
        cmdsave2.Enabled = False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmdsave)
        alButtonList.Add(cmdsave2)
        htPageAccess.Add("add", alButtonList)
        htPageAccess.Add("update", alButtonList)
        'htPageAccess.Add("delete", alButtonList)
        CheckButtonAccess()
        'cmdsave.Enabled = blnCanAdd Or blnCanUpdate
        If IsPostBack Then
            If lstBox2.Items.Count > 0 Then
                cmdsave.Enabled = blnCanAdd Or blnCanUpdate
            End If

            If lstBox4.Items.Count > 0 Then
                cmdsave2.Enabled = blnCanAdd Or blnCanUpdate
            End If

        Else
            cmdsave.Enabled = False
            cmdsave2.Enabled = False
        End If

        If cboCatalogueBuyer.SelectedIndex = 0 Then
            cmdsave.Enabled = False
        End If

        If cboBuyer.SelectedIndex = 0 Then
            cmdsave2.Enabled = False
        End If
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Session("CompanyId") = "demo"
        'Session("UserId") = "moo"
        MyBase.Page_Load(sender, e)

        strMode = Me.Request.QueryString("mode")
        If Not IsPostBack Then

            'Michelle (22/10/2010) - Need to select the Buyer Catalogue
            'viewstate("valIndex") = Request.QueryString("index")
            'viewstate("valCode") = Request.QueryString("code")
            'viewstate("valName") = Request.QueryString("name")
            ' ''//THIS VIEWSTATE IS GET THE URL FROM PREVIOUS PAGE
            ''viewstate("from") = Request.UrlReferrer.ToString

            ''//remark by moo
            ''If viewstate("valIndex") <> "" Then
            'lblcode.Text = viewstate("valCode")
            'lblname.Text = viewstate("valName")
            GenerateTab()
            lstbox1.Items.Clear()
            lstBox2.Items.Clear()
            BindBuyerCat()
            BindBuyer()
            'Else 'Change of Buyer Catalogue
            '    If Not blnlstboxChg Then
            '        lstbox1.Items.Clear()
            '        lstBox2.Items.Clear()
            '        SearchData(cboCatalogueBuyer.SelectedItem.Value)
            '        selectedData(cboCatalogueBuyer.SelectedItem.Value)
            '        blnlstboxChg = False
            '    End If
        End If

        If lstbox1.Items.Count > 0 Or lstBox2.Items.Count > 0 Then
            cmdsave.Enabled = True
            cmdReset.Enabled = True
        Else
            cmdsave.Enabled = False
            cmdReset.Enabled = False
        End If

        If lstbox3.Items.Count > 0 Or lstBox4.Items.Count > 0 Then
            cmdsave2.Enabled = True
            cmdReset2.Enabled = True
        Else
            cmdsave2.Enabled = False
            cmdReset2.Enabled = False
        End If
        'lnkBack.NavigateUrl = "BuyerCatalogue.aspx?&pageid=" & strPageId
        lnkBack.Visible = False
    End Sub

    Public Function selectedData(ByVal val As String)
        Dim dvSelectlBuyer As DataView
        ' If val <> "" Then
        lstBox2.Items.Clear()
        dvSelectlBuyer = objCat.bindlistbox_ContCatSelectedData(val)
        ' Else
        ' strCatName = ViewState("valIndex")

        '  dvSelectlBuyer = objcat.bindlistbox_BuyerCatSelectedData(strCatName)
        ' End If

        lstBox2.DataSource = dvSelectlBuyer
        lstBox2.DataTextField = "three"
        lstBox2.DataValueField = "UM_USER_ID"
        lstBox2.DataBind()
        cmdAdd1.Enabled = True
        cmdAdd2.Enabled = True
    End Function

    Public Function selectedDataBuyer(ByVal val As String)
        Dim dvSelectlBuyer As DataView
        lstBox4.Items.Clear()
        dvSelectlBuyer = objCat.bindlistbox_BuyerSelectedData(val)
        
        lstBox4.DataSource = dvSelectlBuyer
        lstBox4.DataTextField = "CDM_GROUP_CODE"
        lstBox4.DataValueField = "CDM_GROUP_INDEX"
        lstBox4.DataBind()
        cmdAdd3.Enabled = True
        cmdAdd4.Enabled = True
    End Function

    Public Function SearchData(ByVal val As String)
        Dim dvAvailBuyer As DataView

        'If val <> "" Then
        lstbox1.Items.Clear()
        dvAvailBuyer = objCat.bindlistbox_ContCatSearchData(val)
        'Else
        'strCatName = ViewState("valIndex")
        'dvAvailBuyer = objcat.bindlistbox_BuyerCatSearchData(strCatName)
        'End If

        lstbox1.DataSource = dvAvailBuyer
        lstbox1.DataTextField = "three"
        lstbox1.DataValueField = "UM_USER_ID"
        lstbox1.DataBind()
        cmdAdd1.Enabled = True
        cmdAdd2.Enabled = True
    End Function

    Public Function SearchDataBuyer(ByVal val As String)
        Dim dvAvailCat As DataView

        'If val <> "" Then
        lstbox3.Items.Clear()
        dvAvailCat = objCat.bindlistbox_BuyerSearchData(val)
        'Else
        'strCatName = ViewState("valIndex")
        'dvAvailBuyer = objcat.bindlistbox_BuyerCatSearchData(strCatName)
        'End If

        lstbox3.DataSource = dvAvailCat
        lstbox3.DataTextField = "CDM_GROUP_CODE"
        lstbox3.DataValueField = "CDM_GROUP_INDEX"
        lstbox3.DataBind()
        cmdAdd3.Enabled = True
        cmdAdd4.Enabled = True
    End Function

    Public Function Add1()
        For Each li In lstbox1.Items
            If li.Selected = True Then
                Dim lstItem As New ListItem
                lstItem.Text = li.Text
                lstItem.Value = li.Value
                lstBox2.Items.Add(lstItem)

            End If
        Next
        Dim counter As Integer
        For counter = (lstbox1.Items.Count - 1) To 0 Step -1
            If lstbox1.Items(counter).Selected = True Then
                lstbox1.Items.RemoveAt(counter)
            End If
        Next
        cmdAdd1.Enabled = True
        cmdAdd2.Enabled = True
    End Function

    Public Function Add3()
        For Each li In lstbox3.Items
            If li.Selected = True Then
                Dim lstItem As New ListItem
                lstItem.Text = li.Text
                lstItem.Value = li.Value
                lstBox4.Items.Add(lstItem)

            End If
        Next
        Dim counter As Integer
        For counter = (lstbox3.Items.Count - 1) To 0 Step -1
            If lstbox3.Items(counter).Selected = True Then
                lstbox3.Items.RemoveAt(counter)
            End If
        Next
        cmdAdd3.Enabled = True
        cmdAdd4.Enabled = True
    End Function

    Private Function BindBuyerCat()
        Dim dsCat As New DataSet
        dsCat = objCat.getConRefNo

        cboCatalogueBuyer.Items.Clear()


        If Not dsCat Is Nothing Then
            cboCatalogueBuyer.Enabled = True
            Common.FillDdl(cboCatalogueBuyer, "CDM_GROUP_CODE", "CDM_GROUP_INDEX", dsCat)
        End If
        cbolist.Value = ""
        cbolist.Text = "---Select---"
        cboCatalogueBuyer.Items.Insert(0, cbolist)
    End Function

    Private Function BindBuyer()
        Dim dsBuyer As New DataSet
        dsBuyer = objCat.getBuyer

        cboBuyer.Items.Clear()


        If Not dsBuyer Is Nothing Then
            cboBuyer.Enabled = True
            Common.FillDdl(cboBuyer, "three", "UM_USER_ID", dsBuyer)
        End If
        cbolist.Value = ""
        cbolist.Text = "---Select---"
        cboBuyer.Items.Insert(0, cbolist)
    End Function

    Public Function lst2Ary(ByRef pListbox As System.Web.UI.WebControls.ListBox) As String()
        Dim ary(pListbox.Items.Count - 1) As String
        Dim lngLoop As Long
        For lngLoop = 0 To pListbox.Items.Count - 1
            ary(lngLoop) = pListbox.Items(lngLoop).Value
        Next
        lst2Ary = ary
    End Function

    Private Function Add2()
        Dim li As ListItem
        For Each li In lstBox2.Items
            If li.Selected = True Then
                Dim lstItem As New ListItem
                lstItem.Text = li.Text
                lstItem.Value = li.Value
                lstbox1.Items.Add(lstItem)
            End If
        Next
        Dim counter As Integer
        For counter = (lstBox2.Items.Count - 1) To 0 Step -1
            If lstBox2.Items(counter).Selected = True Then
                lstBox2.Items.RemoveAt(counter)
            End If
        Next
        cmdAdd1.Enabled = True
        cmdAdd2.Enabled = True
    End Function

    Private Function Add4()
        Dim li As ListItem
        For Each li In lstBox4.Items
            If li.Selected = True Then
                Dim lstItem As New ListItem
                lstItem.Text = li.Text
                lstItem.Value = li.Value
                lstbox3.Items.Add(lstItem)
            End If
        Next
        Dim counter As Integer
        For counter = (lstBox4.Items.Count - 1) To 0 Step -1
            If lstBox4.Items(counter).Selected = True Then
                lstBox4.Items.RemoveAt(counter)
            End If
        Next
        cmdAdd3.Enabled = True
        cmdAdd4.Enabled = True
    End Function

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        lstBox2.Items.Clear()
        lstbox1.Items.Clear()
        SearchData(cboCatalogueBuyer.SelectedItem.Value)
        selectedData(cboCatalogueBuyer.SelectedItem.Value)
        cmdAdd1.Enabled = True
        cmdAdd2.Enabled = True
    End Sub

    Private Sub cmdAdd1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd1.Click
        Add1()
        If lstBox2.Items.Count > 0 Then
            cmdsave.Enabled = True
        Else
            cmdsave.Enabled = False
        End If
    End Sub

    Private Sub cmdAdd2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd2.Click
        Add2()
        If lstBox2.Items.Count > 0 Then
            cmdsave.Enabled = True
        Else
            cmdsave.Enabled = False
        End If
    End Sub

    'Private Sub cmd_clear1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_clear1.Click
    '    cboCatalogueBuyer.SelectedIndex = 0
    'End Sub

    Private Sub cmdsave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdsave.Click
        Dim query(0) As String
        objCat.DelContcatAsg(cboCatalogueBuyer.SelectedItem.Value)
        For Each li In lstBox2.Items
            objCat.AddContCatAsgBuyer(cboCatalogueBuyer.SelectedItem.Value, li.Value)
        Next
        'strRedirect = "ItemCatalogue.aspx?index=" & ViewState("valIndex") & "&code=" & ViewState("valCode") & "&name=" & ViewState("valName") & "&pageid=" & strPageId
        'Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information, "Wheel")
        'Common.NetPrompt(Me, MsgRecordSave & """& vbCrLf & ""Proceed to Item Assignment?", strRedirect)
        Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
    End Sub

    'Private Sub cboDropList1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    '    lstbox1.Items.Clear()
    '    lstBox2.Items.Clear()
    '    SearchData(viewstate("valIndex"))
    '    selectedData(viewstate("valIndex"))

    'End Sub

    'Michelle (22/10/2010) - Remove the button
    'Private Sub cmdItemAsg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdItemAsg.Click
    '    Response.Redirect("ItemCatalogue.aspx?index=" & ViewState("valIndex") & "&code=" & Server.UrlEncode(ViewState("valCode")) & "&name=" & Server.UrlEncode(ViewState("valName")) & "&pageid=" & strPageId)
    'End Sub

    Private Sub cboCatalogueBuyer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboCatalogueBuyer.SelectedIndexChanged
        '    blnCatChange = True
        If cboCatalogueBuyer.SelectedItem.Value <> "" Then
            SearchData(cboCatalogueBuyer.SelectedItem.Value)
            selectedData(cboCatalogueBuyer.SelectedItem.Value)
        Else
            lstbox1.Items.Clear()
            lstBox2.Items.Clear()
            cmdsave.Enabled = False
            cmdReset.Enabled = False
        End If

    End Sub

    Private Sub cbobuyer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboBuyer.SelectedIndexChanged
        '    blnCatChange = True
        If cboBuyer.SelectedItem.Value <> "" Then
            SearchDataBuyer(cboBuyer.SelectedItem.Value)
            selectedDataBuyer(cboBuyer.SelectedItem.Value)
        Else
            lstbox3.Items.Clear()
            lstBox4.Items.Clear()
            cmdsave2.Enabled = False
            cmdReset2.Enabled = False
        End If

    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_ConCat_tabs") = "<div class=""t_entity""><ul>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Catalogue", "ContractCatalogue.aspx", "pageid=" & strPageId) & """><span>Contract Catalogue</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Catalogue", "ContractItem.aspx", "pageid=" & strPageId) & """><span>Items Assignment</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Catalogue", "ConCat_AssignUser.aspx", "pageid=" & strPageId) & """><span>Users Assignment</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Catalogue", "ConCat_AssignMultiUser.aspx", "pageid=" & strPageId) & """><span>Multi Users Assignment</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Catalogue", "ConCatBatchUploadDownload.aspx", "pageid=" & strPageId) & """><span>Batch Upload/Download</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Catalogue", "AuditTrail.aspx", "pageid=" & strPageId) & """><span>Audit</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "</ul><div></div></div>"

    End Sub

    Private Sub rd1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rd1.SelectedIndexChanged
        If rd1.SelectedItem.Value = "PU" Then
            Me.lblSelect.Text = "Please select Purchasers."
            Me.lbl1.Text = "a) To assign Contract Catalogue to the Purchasers, choose the name from 'Available Contract Catalogue' and click Assign button.<br>b) To remove/unassigned contract catalogue from the Purchaser, choose the 'Selected Contract Catalogue' and click Remove button."
            Div_AA.Style("display") = "None"
            Div_BB.Style("display") = "Inline"
        Else
            Me.lblSelect.Text = "Please select Contract Catalogue."
            Me.lbl1.Text = "a) To assign Purchasers to the Contract Catalogue, choose the name from 'Available Purchasers' and click Assign button.<br>b) To remove/unassigned purchaser from the Contract Catalogue, choose the 'Selected Purchasers' and click Remove button."
            Div_AA.Style("display") = "Inline"
            Div_BB.Style("display") = "None"
        End If
    End Sub

    Private Sub cmdAdd3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd3.Click
        Add3()
        If lstbox3.Items.Count > 0 Then
            cmdsave2.Enabled = True
        Else
            cmdsave2.Enabled = False
        End If
    End Sub

    Private Sub cmdAdd4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd4.Click
        Add4()
        If lstBox4.Items.Count > 0 Then
            cmdsave2.Enabled = True
        Else
            cmdsave2.Enabled = False
        End If
    End Sub

    Private Sub cmdReset2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdReset2.Click
        lstBox4.Items.Clear()
        lstbox3.Items.Clear()
        SearchDataBuyer(cboBuyer.SelectedItem.Value)
        selectedDataBuyer(cboBuyer.SelectedItem.Value)
        cmdAdd3.Enabled = True
        cmdAdd4.Enabled = True
    End Sub

    Private Sub cmdsave2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdsave2.Click
        Dim query(0) As String
        objCat.DelBuyerAsg(cboBuyer.SelectedItem.Value)
        For Each li In lstBox4.Items
            objCat.AddAsgBuyer(cboBuyer.SelectedItem.Value, li.Value)
        Next
        Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
    End Sub
End Class
