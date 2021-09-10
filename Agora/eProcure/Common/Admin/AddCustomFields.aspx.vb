Imports AgoraLegacy
Imports eProcure.Component

Public Class AddCustomFields
    Inherits AgoraLegacy.AppBaseClass
    Dim objdb As New EAD.DBCom
    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MyBase.Page_Load(sender, e)
        If Not Page.IsPostBack Then
            ddlModule.Items.Insert(0, "PO")
            ddlModule.Items.Insert(1, "PR")
            If Request.QueryString("index") <> "" And Request.QueryString("module") <> "" Then
                populate()
                If Request.QueryString("module") = "PO" Then
                    ddlModule.SelectedIndex = 0
                ElseIf Request.QueryString("module") = "PR" Then
                    ddlModule.SelectedIndex = 1
                End If
                hidModule.Value = ddlModule.SelectedItem.Text
                hidName.Value = txtName.Text
                hidValue.Value = txtValue.Text
            End If
            label()
        End If

    End Sub
    Sub populate()
        Dim index, CFmodule As String
        index = Request.QueryString("index")
        CFmodule = Request.QueryString("module")
        txtName.Text = objdb.Get1Column("custom_fields", "CF_FIELD_NAME", " WHERE CF_COY_ID='" & Session("CompanyId") & "' AND CF_FIELD_INDEX = '" & index & "' AND CF_MODULE='" & CFmodule & "'")
        txtValue.Text = objdb.Get1Column("custom_fields", "CF_FIELD_VALUE", " WHERE CF_COY_ID='" & Session("CompanyId") & "' AND CF_FIELD_INDEX = '" & index & "' AND CF_MODULE='" & CFmodule & "'")
    End Sub
    Sub label()
        If Session("userAction") = "Add" Then
            lblHeader.Text = "Add Custom Field :"
            ddlModule.Enabled = True
            txtName.Enabled = True

        ElseIf Session("userAction") = "Modify" Then
            lblHeader.Text = "Modify Custom Field :"
            txtName.Text = hidName.Value
            txtValue.Text = hidValue.Value
            If hidModule.Value = "PO" Then
                ddlModule.SelectedIndex = 0
            ElseIf hidModule.Value = "PR" Then
                ddlModule.SelectedIndex = 1
            End If
            ddlModule.Enabled = False
            txtName.Enabled = False
        End If
    End Sub
    Protected Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        vldsum.InnerHtml = ""
        Session("userAction") = "Add"
        hidModule.Value = ""
        hidName.Value = ""
        hidValue.Value = ""
        txtValue.Text = ""
        label()

    End Sub
    Function validate() As Boolean
        If txtName.Text = "" Then
            vldsum.InnerHtml = "<li>Required Field Name</li>"
            Return False
        ElseIf txtValue.Text = "" Then
            vldsum.InnerHtml = "<li>Required Field Value</li>"
            Return False
        End If
        Return True

    End Function
    Protected Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If validate() Then
            Dim intMsgNo As Integer
            Dim strMsg As String
            Dim objPS As New PersonalSetting

            If Session("userAction") = "Add" Then
                intMsgNo = objPS.addCustomField(txtName.Text, txtValue.Text, ddlModule.SelectedItem.Text, "Add")
                Select Case intMsgNo
                    Case WheelMsgNum.Save
                        strMsg = MsgRecordSave
                        hidModule.Value = ddlModule.SelectedItem.Text
                        hidName.Value = txtName.Text
                        hidValue.Value = txtValue.Text
                        Session("userAction") = "Modify"

                    Case WheelMsgNum.Duplicate
                        strMsg = MsgRecordDuplicate
                End Select
                'hidModule.Value = ddlModule.SelectedItem.Text
                'hidName.Value = txtName.Text
                'hidValue.Value = txtValue.Text
                'Session("userAction") = "Modify"

            ElseIf Session("userAction") = "Modify" Then
                intMsgNo = objPS.addCustomField(txtName.Text, txtValue.Text, ddlModule.SelectedItem.Text, "Modify", hidModule.Value, hidName.Value, hidValue.Value)
                Select Case intMsgNo
                    Case WheelMsgNum.Save
                        strMsg = MsgRecordSave
                        hidModule.Value = ddlModule.SelectedItem.Text
                        hidName.Value = txtName.Text
                        hidValue.Value = txtValue.Text

                    Case WheelMsgNum.Duplicate
                        strMsg = MsgRecordDuplicate
                    Case WheelMsgNum.NotSave
                        strMsg = MsgRecordNotSave
                End Select
                'hidModule.Value = ddlModule.SelectedItem.Text
                'hidName.Value = txtName.Text
                'hidValue.Value = txtValue.Text
            End If
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
            label()
        End If
    End Sub


End Class