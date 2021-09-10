'Jules 2015.02.02 Agora Stage 2
Imports AgoraLegacy
Imports eProcure.Component

Public Class DebitCreditNoteList
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents cmd_RaiseDN As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_RaiseCN As System.Web.UI.WebControls.Button
    Protected WithEvents txt_InvNum As System.Web.UI.WebControls.TextBox
#End Region

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)

        If Not Page.IsPostBack Then
            GenerateTab()
        End If
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_Debit_Credit_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn_selected"" href=""#""><span>Debit Note / Credit Note</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteListing.aspx", "pageid=" & strPageId) & """><span>Debit Note Listing</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("CreditNote", "CreditNoteListing.aspx", "pageid=" & strPageId) & """><span>Credit Note Listing</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                           "</ul><div></div></div>"
    End Sub

    Private Sub cmd_RaiseDN_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_RaiseDN.Click
        ''Need to check if Inv No is valid. If yes, then raise.
        Dim objInv As New Invoice
        Dim ds As New DataSet
        Dim objDB As New EAD.DBCom 'Jules 2015-Feb-23 Agora Stage 2

        If txt_InvNum.Text = "" Then
            Common.NetMsgbox(Me, "Invoice No. is required.")
        Else
            'Jules 2015-Feb-23 Agora Stage 2
            'ds = objInv.get_invoiceview(invStatus.Approved, "", txt_InvNum.Text)
            'If ds.Tables(0).Rows.Count > 0 Then
            '    Session("urlreferer") = "RaiseDebitNote"
            '    Response.Redirect(dDispatcher.direct("DebitNote", "RaiseDebitNote.aspx", "INVNO=" & txt_InvNum.Text))
            'Else
            '    Common.NetMsgbox(Me, "Record not found.")
            'End If

            Dim invStatus = objDB.GetVal("SELECT IM_INVOICE_STATUS FROM INVOICE_MSTR WHERE IM_INVOICE_NO='" & txt_InvNum.Text & "' AND IM_S_COY_ID='" & Session("CompanyId") & "'")
            'Issue 7480 - CH - 23 Mar 2015
            Dim strInvNo As String = objDB.GetVal("SELECT IFNULL(IM_INVOICE_NO, '') FROM INVOICE_MSTR WHERE IM_INVOICE_NO='" & txt_InvNum.Text & "' AND IM_S_COY_ID='" & Session("CompanyId") & "'")
            If invStatus <> "" Then
                'If invStatus = 3 Then
                '    Response.Redirect(dDispatcher.direct("DebitNote", "RaiseDebitNote.aspx", "INVNO=" & txt_InvNum.Text))
                'ElseIf invStatus = 1 Or invStatus = 2 Then
                '    Common.NetMsgbox(Me, "Cannot raise Debit Note. Invoice is pending approval.")
                'ElseIf invStatus = 4 Then
                '    Common.NetMsgbox(Me, "Cannot raise Debit Note. Invoice has been paid.")
                'Else
                '    Common.NetMsgbox(Me, "Cannot raise Debit Note. Invoice has been approved.")
                'End If

                If invStatus = 4 Or invStatus = 3 Then
                    'Common.NetMsgbox(Me, "Cannot raise Debit Note. Invoice has been paid.")
                    Common.NetMsgbox(Me, "Unable to raise Debit Note due to invoice has been approved/ paid.")
                Else
                    Response.Redirect(dDispatcher.direct("DebitNote", "RaiseDebitNote.aspx", "INVNO=" & strInvNo))
                End If
            Else
                Common.NetMsgbox(Me, "Record not found.")
            End If
        End If
    End Sub

    Private Sub cmd_RaiseCN_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_RaiseCN.Click
        ''Need to check if Inv No is valid. If yes, then raise.
        Dim objInv As New Invoice
        Dim ds As New DataSet
        Dim objDB As New EAD.DBCom

        If txt_InvNum.Text = "" Then
            Common.NetMsgbox(Me, "Invoice No. is required.")
        Else
            'ds = objInv.get_invoiceview(invStatus.Approved, "", txt_InvNum.Text)
            'If ds.Tables(0).Rows.Count > 0 Then
            '    Session("urlreferer") = "RaiseDebitNote"
            '    Response.Redirect(dDispatcher.direct("CreditNote", "RaiseCreditNote.aspx", "INVNO=" & txt_InvNum.Text))
            'Else
            '    Common.NetMsgbox(Me, "Record not found.")
            'End If

            Dim invStatus = objDB.GetVal("SELECT IM_INVOICE_STATUS FROM INVOICE_MSTR WHERE IM_INVOICE_NO='" & Common.Parse(txt_InvNum.Text) & "' AND IM_S_COY_ID='" & Session("CompanyId") & "'")
            'Issue 7480 - CH - 23 Mar 2015
            Dim strInvNo As String = objDB.GetVal("SELECT IFNULL(IM_INVOICE_NO, '') FROM INVOICE_MSTR WHERE IM_INVOICE_NO='" & Common.Parse(txt_InvNum.Text) & "' AND IM_S_COY_ID='" & Session("CompanyId") & "'")
            If invStatus <> "" Then
                If invStatus = 1 Or invStatus = 2 Then
                    Response.Redirect(dDispatcher.direct("CreditNote", "RaiseCreditNote.aspx", "INVNO=" & strInvNo))
                Else
                    Common.NetMsgbox(Me, "Unable to raise Credit Note due to invoice has been approved/ paid.")
                End If
            Else
                Common.NetMsgbox(Me, "Record not found.")
            End If
        End If
    End Sub

End Class