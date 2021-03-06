Imports System.Drawing
Imports AgoraLegacy
Partial Public Class MenuAccessRight
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("PageCount") = 20
        blnPaging = False
        blnSorting = False

        ViewState("Mode") = Request.Params("mode")
        Me.rbCurFixedRole.Checked = True
        Me.txtNewFixedRole.Enabled = False
        'SetGridProperty(dtgMenuSeq)
        If ViewState("Mode") = "add" Then
            GenerateTab()
        End If

        If Not Page.IsPostBack Then
            Populate()
            'Bindgrid()
        End If
        lnkBack.NavigateUrl = dDispatcher.direct("Global", "MenuMtn.aspx")
    End Sub
    Sub Populate()
        Dim objMS As New MenuSeq
        objMS.FillModule(ddlModule, ConfigurationSettings.AppSettings("eProcurePath"))
        objMS.FillFixedRole(ddlFixedRole, ConfigurationSettings.AppSettings("eProcurePath"))
        ' objMS.FillUserGroup(ddlUsrGrp, ConfigurationSettings.AppSettings("eProcurePath"))
    End Sub
    

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim objM As New MenuSeq
        Dim SelectedModValue As Integer
        Dim FixedRole As String
        Dim ParentValue As Integer
        Dim iCount As Integer
        Dim newFixedRole As String
        Dim ds As New DataSet


        SelectedModValue = ddlModule.SelectedValue 'Level value
        FixedRole = ddlFixedRole.Text
        ParentValue = objM.GetModuleParent(SelectedModValue)
        iCount = objM.GetMenuCount(ParentValue)
        ds = objM.GetMenuID(ParentValue)
        newFixedRole = txtNewFixedRole.Text
        'UsrGrp = ddlUsrGrp.Text

        If FixedRole = "" Then
            objM.AddModuleToNewFixedRole(ds, newFixedRole, ParentValue, iCount)
        Else
            objM.AddModuleToFixedRole(ds, FixedRole, ParentValue, iCount)
        End If

        'objM.AddModuleToUAR(ds, UsrGrp, ParentValue, iCount)

        Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
        txtNewFixedRole.Text = ""
        Me.rbNewFixedRole.Checked = False
        Me.ddlFixedRole.Enabled = True
        Me.txtNewFixedRole.Enabled = False
        Populate()

    End Sub


    Sub GenerateTab()
        If ViewState("Mode") = "add" Then

            Session("MenuAdd_tabs") = "<div class=""t_entity""><ul>" & _
                       "<li><div class=""space""></div></li>" & _
                                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Global", "MenuAdd.aspx", "mode=add") & """><span>Add New Module</span></a></li>" & _
                                            "<li><div class=""space""></div></li>" & _
                                           "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Global", "MenuAddNode.aspx", "mode=add") & """><span>Add New Node</span></a></li>" & _
                                            "<li><div class=""space""></div></li>" & _
                                            "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Global", "MenuAccessRight.aspx", "mode=add") & """><span>Access Right</span></a></li>" & _
                                            "<li><div class=""space""></div></li>" & _
                                          "</ul><div></div></div>"

        End If
    End Sub

   
    Private Sub rbNewFixedRole_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbNewFixedRole.CheckedChanged
        Me.txtNewFixedRole.Enabled = True
        Me.ddlFixedRole.Enabled = False
        Me.rbCurFixedRole.Checked = False
    End Sub

    Private Sub rbCurFixedRole_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbCurFixedRole.CheckedChanged
        Me.txtNewFixedRole.Enabled = False

        Me.ddlFixedRole.Enabled = True
        Me.rbNewFixedRole.Checked = False
    End Sub
End Class