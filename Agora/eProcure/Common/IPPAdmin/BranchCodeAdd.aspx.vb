Imports eProcure.Component
Imports AgoraLegacy

Public Class BranchCodeAdd
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objGlobal As New AppGlobals
    Dim strMode As String
    Dim comfirmstatus As String
    Dim strBranchStatus As String
    Dim strBranchCode As String
    Dim strBranchName As String

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
        cmdSave.Visible = False
        cmdAdd.Visible = False
        cmdClose.Visible = True
        cmdSave.Enabled = False
        cmdAdd.Enabled = False
        cmdClose.Enabled = True

        Dim alButtonList As ArrayList
        If strMode = "Modify" Then
            cmdSave.Visible = True
            cmdSave.Enabled = True
            txtHOBR.ReadOnly = True 'All fields are editable except for ho/br code
            cmdSave.Visible = True
            cmdAdd.Visible = True

        ElseIf strMode = "Add" Then
            cmdSave.Visible = True
            cmdAdd.Visible = True
            cmdSave.Enabled = True
            cmdAdd.Enabled = True
            Me.ddlbranchType.Visible = True
            Me.ddlComp.Visible = True
            Me.txtCompany.Visible = False
            Me.txtHOBR.Visible = False

        End If
        'CheckButtonAccess()
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objCB As New IPP
        Dim blnHasPending As String
        strNewCSS = "true"
        MyBase.Page_Load(sender, e)
        strMode = Me.Request.QueryString("mode")

        If Not IsPostBack Then
            If strMode = "Modify" Then
                Me.txtHOBRCode.Text = Me.Request.QueryString("cbm_index").Trim
                populateData()
            End If
        End If

        objCB = Nothing

    End Sub

    Private Sub cmdClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Response.Write("<script language='javascript'> { window.close();}</script>")

    End Sub

    Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        strMode = "Add"
        Me.txtHOBRCode.Text = ""
        Me.txtHOBRCode.Focus()
        Me.txtHOBRName.Text = ""
        'Me.rbtnstatus.SelectedValue = "A"
    End Sub

    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim strMsg As String
        Dim objCB As New IPP
        Dim objDB As New EAD.DBCom

        If Page.IsValid Then
            lblMsg.Text = ""
            If strMode = "Add" Then
                comfirmstatus = objCB.AddCompBranchMstr(Me.ddlComp.SelectedItem.Text, Me.ddlbranchType.SelectedItem.Text, Me.txtHOBRCode.Text, Me.txtHOBRName.Text, Me.rbtnstatus.SelectedValue, Me.txtGLCode.Text)
                Select Case comfirmstatus
                    Case WheelMsgNum.Duplicate
                        strMsg = objGlobal.GetErrorMessage("00002")

                    Case WheelMsgNum.NotSave
                        strMsg = objGlobal.GetErrorMessage("00007")

                    Case WheelMsgNum.Save
                        Session("action") = "Add"
                        strMsg = objGlobal.GetErrorMessage("00003")

                End Select

            ElseIf strMode = "Modify" Then
                'two records of same comp & ho/br code shouldnt exist
                If txtCompany.Text = ddlComp.SelectedItem.Text Then 'No Changes
                    'Zulham 15062015 - Replaced the txtHOBR.Text to ddlbranchType.SelectedItem.Text
                    'comfirmstatus = objCB.ModCompBranchMstr(Me.txtHOBR.Text, Me.txtHOBRCode.Text, Me.txtHOBRName.Text, Me.rbtnstatus.SelectedValue, CInt(Request.QueryString("cbm_index")), Me.txtGLCode.Text)
                    comfirmstatus = objCB.ModCompBranchMstr(Me.ddlbranchType.SelectedItem.Text, Me.txtHOBRCode.Text, Me.txtHOBRName.Text, Me.rbtnstatus.SelectedValue, CInt(Request.QueryString("cbm_index")), Me.txtGLCode.Text)
                Else 'conflicting changes
                    Dim strSQL = "SELECT * FROM company_branch_mstr WHERE CBm_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                            & "AND CBm_BRANCH_CODE='" & Common.Parse(strBranchCode) & "' "
                    If objDB.Exist(strSQL) = 0 Then
                        'Zulham 15062015 - Replaced the txtHOBR.Text to ddlbranchType.SelectedItem.Text
                        'comfirmstatus = objCB.ModCompBranchMstr(Me.txtHOBR.Text, Me.txtHOBRCode.Text, Me.txtHOBRName.Text, Me.rbtnstatus.SelectedValue, CInt(Request.QueryString("cbm_index")), Me.txtGLCode.Text)
                        comfirmstatus = objCB.ModCompBranchMstr(Me.ddlbranchType.SelectedItem.Text, Me.txtHOBRCode.Text, Me.txtHOBRName.Text, Me.rbtnstatus.SelectedValue, CInt(Request.QueryString("cbm_index")), Me.txtGLCode.Text)
                    Else
                        comfirmstatus = WheelMsgNum.Duplicate
                    End If
                End If
                Select Case comfirmstatus
                    Case WheelMsgNum.Duplicate
                        strMsg = objGlobal.GetErrorMessage("00002")

                    Case WheelMsgNum.NotSave
                        strMsg = objGlobal.GetErrorMessage("00007")

                    Case WheelMsgNum.Save
                        Session("action") = "Modify"
                        strMsg = objGlobal.GetErrorMessage("00005")
                End Select
            End If

            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
            objCB = Nothing

        End If

    End Sub
    Public Sub populateData()
        Try
            Dim ds As New DataSet
            Dim dt As New DataTable
            Dim ipp As New IPP

            ds = ipp.GetHOBR("", "", "", "", 0, 0, "", CInt(Request.QueryString("cbm_index")))
            dt = ds.Tables(0)
            If dt.Rows.Count > 0 Then 'data exists
                Me.txtCompany.Text = dt.Rows(0).Item("cbm_coy_id").ToString
                'mimi 2018-04-24 : remove hardcode HLB to PAMB
                If dt.Rows(0).Item("cbm_coy_id").ToString.ToUpper = "PAMB" Then
                    Me.ddlComp.SelectedValue = "1"
                Else
                    Me.ddlComp.SelectedValue = "0"
                End If
                'If dt.Rows(0).Item("cbm_coy_id").ToString = "HLB" Then
                '    Me.ddlComp.SelectedValue = "1"
                'ElseIf dt.Rows(0).Item("cbm_coy_id").ToString = "HLISB" Then
                '    Me.ddlComp.SelectedValue = "2"
                'Else
                '    Me.ddlComp.SelectedValue = "0"
                'End If
                Me.txtHOBR.Text = dt.Rows(0).Item("cbm_branch_type").ToString
                If dt.Rows(0).Item("cbm_branch_type").ToString = "HO" Then
                    Me.ddlbranchType.SelectedValue = "1"
                ElseIf dt.Rows(0).Item("cbm_branch_type").ToString = "BR" Then
                    Me.ddlbranchType.SelectedValue = "2"
                Else
                    Me.ddlbranchType.SelectedValue = "0"
                End If
                Me.txtGLCode.Text = dt.Rows(0).Item("cbm_gl_code").ToString
                Me.txtHOBRCode.Text = dt.Rows(0).Item("cbm_branch_code").ToString
                Me.txtHOBRName.Text = dt.Rows(0).Item("cbm_branch_name").ToString
                If dt.Rows(0).Item("cbm_status").ToString = "A" Then
                    rbtnstatus.SelectedValue = "A"
                Else
                    rbtnstatus.SelectedValue = "I"
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class