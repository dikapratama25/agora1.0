Imports eProcure.Component
Imports AgoraLegacy

Partial Public Class AddPaymentType
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objGlobal As New AppGlobals
    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strNewCSS = "true"
        MyBase.Page_Load(sender, e)
        If ViewState("mode") = "" Or ViewState("mode") Is Nothing Then
            ViewState("mode") = Request.QueryString("mode")
        End If        
        If Not IsPostBack Then
            If ViewState("mode") = "modify" Then
                txtPaymentType.Enabled = False
                populatedata()
                ViewState("index") = Request.QueryString("index")
            End If
        End If

        cmdSave.Attributes.Add("onclick", "clearvld();")
        cmdAdd.Attributes.Add("onclick", "clearvld();")
    End Sub
    Sub populatedata()
        Dim objdb As New EAD.DBCom
        Dim condition As String
        condition = " WHERE PT_COY_ID='" & Session("CompanyId") & "' AND " &
        "PT_INDEX='" & Request.QueryString("index") & "'"
        txtPaymentType.Text = objdb.Get1ColumnCheckNull("PAYMENT_TYPE", "PT_PT_CODE", condition)
        txtDesc.Text = objdb.Get1ColumnCheckNull("PAYMENT_TYPE", "PT_PT_DESC", condition)
        If objdb.Get1ColumnCheckNull("PAYMENT_TYPE", "PT_STATUS", condition) = "A" Then
            rdbStatus.SelectedIndex = 0
        Else
            rdbStatus.SelectedIndex = 1
        End If
    End Sub
    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim objippmain As New IPPMain
        Dim objipp As New IPP
        Dim objdb As New EAD.DBCom
        If validate() Then
            If ViewState("mode") = "new" Then
                If objippmain.addPaymentType(Common.Parse(txtPaymentType.Text), Common.Parse(txtDesc.Text), rdbStatus.SelectedItem.Value) Then
                    vldsummary.InnerHtml = ""
                    objipp.Message(Me, "00003", MsgBoxStyle.Information)
                Else
                    objipp.Message(Me, "00002", MsgBoxStyle.Information)
                End If
                ViewState("mode") = "modify"
                Dim condition As String
                condition = " WHERE pt_pt_code='" & Common.Parse(txtPaymentType.Text) & "' AND pt_coy_id='" & Session("CompanyId") & "'"
                ViewState("index") = objdb.Get1ColumnCheckNull("payment_type", "pt_index",condition )
            Else
                ' for modification
                If objippmain.modPaymentType(ViewState("index"), Common.Parse(txtPaymentType.Text), Common.Parse(txtDesc.Text), rdbStatus.SelectedItem.Value) Then
                    vldsummary.InnerHtml = ""
                    objipp.Message(Me, "00005", MsgBoxStyle.Information)
                Else
                    objipp.Message(Me, "00007", MsgBoxStyle.Information)
                End If
                ViewState("mode") = "modify"
            End If
        End If
        txtPaymentType.Enabled = False
    End Sub
    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        ViewState("mode") = "new"
        txtPaymentType.Enabled = True
        txtDesc.Text = ""
        txtPaymentType.Text = ""
        rdbStatus.SelectedIndex = 0
    End Sub
    Function validate() As Boolean
        Dim objippmain As New IPPMain
        If txtDesc.Text = "" Then
            vldsummary.InnerHtml = "<li>Description is required.</li>"
            Return False
        End If
        If txtPaymentType.Text = "" Then
            vldsummary.InnerHtml = "<li>Payment Type Code is required.</li>"
            Return False
        End If
        '' for symbol checking
        'If objippmain.IsAlphaNum(txtDesc.Text) = False Then
        '    vldsummary.InnerHtml = "<li>Description is required.</li>"
        'End If
        'If objippmain.IsAlphaNum(txtPaymentType.Text) = False Then
        '    vldsummary.InnerHtml = "<li>Payment Type is required.</li>"
        'End If

        'CHECK PENDING RECORD
        If rdbStatus.SelectedItem.Value = "I" Then
            If objippmain.chkPymtPendingRec(Request.QueryString("index")) = False Then
                vldsummary.InnerHtml = "<li>Payment Type cannot change to Inactive, There is still pending records.</li>"
                Return False
            End If
        End If
        Return True
    End Function
End Class