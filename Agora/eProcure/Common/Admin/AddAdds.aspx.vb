Imports System.Data.SqlClient
Imports AgoraLegacy
Imports eProcure.Component

Public Class AddAdds
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents cmdadd As System.Web.UI.HtmlControls.HtmlInputButton
    'Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Dim strMode As String
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents txt_AddCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents vldAddCode As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents txt_Add1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents vldAdd As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents txt_Add2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_Add3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_City As System.Web.UI.WebControls.TextBox
    Protected WithEvents cbo_State As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txt_PostCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents cbo_Country As System.Web.UI.WebControls.DropDownList
    Dim strType As String
    Dim strCode As String
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents vldCity As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldPostCode As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldState As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldCountry As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents revPostcode As System.Web.UI.WebControls.RegularExpressionValidator
    Dim objAdmin As New Admin
    Dim objGlobal As New AppGlobals

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
        cmdSave.Enabled = False
        cmdDelete.Enabled = False
        Dim alButtonList As ArrayList
        If strMode = "update" Then
            alButtonList = New ArrayList
            alButtonList.Add(cmdSave)
            htPageAccess.Add("update", alButtonList)
            alButtonList = New ArrayList
            alButtonList.Add(cmdDelete)
            htPageAccess.Add("delete", alButtonList)
        ElseIf strMode = "add" Then
            alButtonList = New ArrayList
            alButtonList.Add(cmdSave)
            htPageAccess.Add("add", alButtonList)
        End If
        CheckButtonAccess()
        cmdReset.Disabled = Not (blnCanAdd Or blnCanUpdate Or blnCanDelete)
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        If ViewState("mode") = "add" Then
            strMode = "add"
        ElseIf ViewState("mode") = "modify" Then
            strMode = "update"
        Else
            strMode = Me.Request.QueryString("mode")
        End If

        'strMode = Me.Request.QueryString("mode")
        strType = Me.Request.QueryString("type")


        If Not IsPostBack Then
            ViewState("Side") = Request.Params("side")
            GenerateTab()
        End If
        'If strType = "B" Then
        '    lblTitle.Text = "Billing Address"
        'Else
        '    lblTitle.Text = "Delivery Address"
        'End If

        If Not Page.IsPostBack Then
            isNotPostBack()
        End If
        lnkBack.NavigateUrl = "" & dDispatcher.direct("Admin", "AddressMaster.aspx", "type=" & strType & "&side=" & ViewState("Side") & "&mod=" & "T" & "&pageid=" & strPageId)
        cmdReset.CausesValidation = False
        cmdadd.CausesValidation = False
        cmdDelete.Attributes.Add("onclick", "return confirm('" & MsgForDeleteButton & "');")
    End Sub

    Private Sub isNotPostBack()
        objGlobal.FillCodeTable(cbo_Country, CodeTable.Country)
        objGlobal.FillState(cbo_State, cbo_Country.SelectedItem.Value)

        If strMode = "update" Then
            strCode = Me.Request.QueryString("code")
            Dim dsAddress As New DataSet
            dsAddress = objAdmin.getAddress(strCode, strType)

            txt_AddCode.Text = strCode
            txt_AddCode.Enabled = False

            If dsAddress.Tables(0).Rows.Count > 0 Then
                txt_Add1.Text = Common.parseNull(dsAddress.Tables(0).Rows(0)("AM_ADDR_LINE1"))
                txt_Add2.Text = Common.parseNull(dsAddress.Tables(0).Rows(0)("AM_ADDR_LINE2"))
                txt_Add3.Text = Common.parseNull(dsAddress.Tables(0).Rows(0)("AM_ADDR_LINE3"))
                txt_City.Text = Common.parseNull(dsAddress.Tables(0).Rows(0)("AM_CITY"))
                txt_PostCode.Text = Common.parseNull(dsAddress.Tables(0).Rows(0)("AM_POSTCODE"))
                Common.SelDdl(Common.parseNull(dsAddress.Tables(0).Rows(0)("AM_COUNTRY")), cbo_Country, True, True)
                objGlobal.FillState(cbo_State, Common.parseNull(dsAddress.Tables(0).Rows(0)("AM_COUNTRY")))
                Common.SelDdl(Common.parseNull(dsAddress.Tables(0).Rows(0)("AM_STATE")), cbo_State, True, True)
            End If
            Me.cmdDelete.Visible = True
            Me.cmdSave.Visible = True
            Me.cmdReset.Value = "Reset"
            lblHeader.Text = "Modify Address"
            cmdadd.Visible = False
        Else
            clearTextBox()
            lblHeader.Text = "Add Address"
            'objGlobal.FillCodeTable(cbo_State, CodeTable.State)
            Me.cmdReset.Visible = False
        End If
    End Sub

    Private Sub clearTextBox()
        Me.txt_Add1.Text = ""
        Me.txt_Add2.Text = ""
        Me.txt_Add3.Text = ""
        Me.txt_AddCode.Text = ""
        Me.txt_City.Text = ""
        Me.txt_PostCode.Text = ""
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim strRedirect As String
        Dim intMsgNo As Integer
        Dim strMsg As String

        If strMode = "add" Then
            intMsgNo = objAdmin.addAddress(txt_AddCode.Text, txt_Add1.Text, txt_Add2.Text, txt_Add3.Text, _
                                txt_PostCode.Text, txt_City.Text, cbo_State.SelectedItem.Value, _
                                cbo_Country.SelectedItem.Value, strType)
            Select Case intMsgNo
                Case WheelMsgNum.Save
                    strMsg = MsgRecordSave
                    'txt_AddCode.Text = ""
                    'txt_Add1.Text = ""
                    'txt_Add2.Text = ""
                    'txt_Add3.Text = ""
                    'txt_City.Text = ""
                    'txt_PostCode.Text = ""
                    'cbo_State.SelectedIndex = 0
                    ViewState("mode") = "modify"
                    txt_AddCode.Enabled = False
                Case WheelMsgNum.Duplicate
                    strMsg = MsgRecordDuplicate
                    'strRedirect = "addAdds.aspx?mode=update&pageid=" & strPageId & "&code=" & txt_AddCode.Text & "&type=" & strType
                    'Common.NetMsgbox(Me, strMsg, strRedirect, MsgBoxStyle.Information)
                Case WheelMsgNum.NotSave
                    strMsg = MsgRecordNotSave
                    'Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
            End Select
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
        Else
            intMsgNo = objAdmin.modAddress(txt_AddCode.Text, txt_Add1.Text, txt_Add2.Text, txt_Add3.Text, _
                                txt_PostCode.Text, txt_City.Text, cbo_State.SelectedItem.Value, _
                                cbo_Country.SelectedItem.Value, strType)
            'strRedirect = "AddressMaster.aspx?type=" & strType & "&mod=T&pageid=" & strPageId

            Select Case intMsgNo
                Case WheelMsgNum.Save
                    strMsg = MsgRecordSave
                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    'Common.NetMsgbox(Me, strMsg, strRedirect, MsgBoxStyle.Information)
                Case WheelMsgNum.NotSave
                    strMsg = MsgRecordNotSave
                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
            End Select
        End If
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim strRedirect As String
        Dim strMsg As String
        Dim intMsgNo As Integer
        Dim strNotDeleted As String

        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add("Code", Type.GetType("System.String"))
        dr = dt.NewRow
        dr("Code") = txt_AddCode.Text
        dt.Rows.Add(dr)

        intMsgNo = objAdmin.delAddress(dt, strType, strNotDeleted)

        If strNotDeleted = "" Then
            Select Case intMsgNo
                Case WheelMsgNum.Delete
                    strMsg = MsgRecordDelete
                    strRedirect = "" & dDispatcher.direct("Admin", "AddressMaster.aspx", "type=" & strType & "&side=" & ViewState("Side") & "&mod=" & "T" & "&pageid=" & strPageId)
                    Common.NetMsgbox(Me, strMsg, strRedirect, MsgBoxStyle.Information)
                Case WheelMsgNum.NotDelete
                    strMsg = MsgRecordNotDelete
                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
            End Select
        Else
            strMsg = "Delivery address " & txt_AddCode.Text & " not deleted successfully as it has outstanding DO(s)."
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
        End If

    End Sub

    Private Sub cbo_Country_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Country.SelectedIndexChanged
        Dim objGlobal As New AppGlobals
        If cbo_Country.SelectedItem.Value <> "" Then
            objGlobal.FillState(cbo_State, cbo_Country.SelectedItem.Value)
        End If
    End Sub

    Private Sub cmdReset_ServerClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.ServerClick
        isNotPostBack()
    End Sub
    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        'If strType = "B" Then
        '    Session("w_AddAddress_tabs") = "<div class=""t_entity""><ul>" & _
        '    "<li><div class=""space""></div></li>" & _
        '                          "<li><a class=""t_entity_btn"" href=""../Companies/coCompanyDetail.aspx?side=BUYER&mode=modify&pageid=" & strPageId & """><span>Company Details</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                          "<li><a class=""t_entity_btn"" href=""../Admin/BComParam.aspx?side=BUYER&pageid=" & strPageId & """><span>Parameters</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                          "<li><a class=""t_entity_btn"" href=""../Admin/AddressMaster.aspx?side=BUYER&type=D&mod=T&pageid=" & strPageId & """><span>Delivery Address</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                          "<li><a class=""t_entity_btn_selected"" href=""../Admin/AddressMaster.aspx?side=BUYER&type=B&mod=T&pageid=" & strPageId & """><span>Billing Address</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                      "<li><a class=""t_entity_btn"" href=""../Admin/DeptSetup.aspx?side=BUYER&pageid=" & strPageId & """><span>Department</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                         "</ul><div></div></div>"
        'ElseIf strType = "D" Then
        '    Session("w_AddAddress_tabs") = "<div class=""t_entity""><ul>" & _
        '    "<li><div class=""space""></div></li>" & _
        '                          "<li><a class=""t_entity_btn"" href=""../Companies/coCompanyDetail.aspx?side=BUYER&mode=modify&pageid=" & strPageId & """><span>Company Details</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                          "<li><a class=""t_entity_btn"" href=""../Admin/BComParam.aspx?side=BUYER&pageid=" & strPageId & """><span>Parameters</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                          "<li><a class=""t_entity_btn_selected"" href=""../Admin/AddressMaster.aspx?side=BUYER&type=D&mod=T&pageid=" & strPageId & """><span>Delivery Address</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                          "<li><a class=""t_entity_btn"" href=""../Admin/AddressMaster.aspx?side=BUYER&type=B&mod=T&pageid=" & strPageId & """><span>Billing Address</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                      "<li><a class=""t_entity_btn"" href=""../Admin/DeptSetup.aspx?side=BUYER&pageid=" & strPageId & """><span>Department</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                         "</ul><div></div></div>"
        'End If
        If strType = "B" Then
            Session("w_AddAddress_tabs") = "<div class=""t_entity""><ul>" & _
            "<li><div class=""space""></div></li>" & _
                                  "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "coCompanyDetail.aspx", "side=BUYER&mode=modify&pageid=" & strPageId) & """><span>Company Details</span></a></li>" & _
                                  "<li><div class=""space""></div></li>" & _
                                  "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "BComParam.aspx", "side=BUYER&pageid=" & strPageId) & """><span>Parameters</span></a></li>" & _
                                  "<li><div class=""space""></div></li>" & _
                                  "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=D&mod=T&pageid=" & strPageId) & """><span>Delivery Address</span></a></li>" & _
                                  "<li><div class=""space""></div></li>" & _
                                  "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=B&mod=T&pageid=" & strPageId) & """><span>Billing Address</span></a></li>" & _
                                  "<li><div class=""space""></div></li>" & _
                                  "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "DeptSetup.aspx", "side=BUYER&pageid=" & strPageId) & """><span>Department</span></a></li>" & _
                                  "<li><div class=""space""></div></li>" & _
                                  "</ul><div></div></div>"
        ElseIf strType = "D" Then
            Session("w_AddAddress_tabs") = "<div class=""t_entity""><ul>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "coCompanyDetail.aspx", "side=BUYER&mode=modify&pageid=" & strPageId) & """><span>Company Details</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "BComParam.aspx", "side=BUYER&pageid=" & strPageId) & """><span>Parameters</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=D&mod=T&pageid=" & strPageId) & """><span>Delivery Address</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=B&mod=T&pageid=" & strPageId) & """><span>Billing Address</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "DeptSetup.aspx", "side=BUYER&pageid=" & strPageId) & """><span>Department</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "</ul><div></div></div>"
        End If
    End Sub

    Private Sub cmdadd_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdadd.ServerClick
        ViewState("mode") = "add"
        strMode = "add"
        txt_AddCode.Enabled = True
        isNotPostBack()

    End Sub
End Class
