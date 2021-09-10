Imports eProcure.Coponent
Imports AgoraLegacy

Partial Public Class WithHoldingTax
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim strHoldingTax As String = ""

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        MyBase.Page_Load(sender, e)
        'SetGridProperty(dtgGLCode)
        If Me.Request.QueryString("id") <> "" Or Me.Request.QueryString("id") <> Nothing Then
            If InStr(Me.Request.QueryString("id").ToString, ":") Then
                strHoldingTax = Me.Request.QueryString("id").ToString.Substring(0, InStr(Me.Request.QueryString("id").ToString, ":") - 1)
            Else
                strHoldingTax = Common.Parse(Me.Request.QueryString("id")) 'get from Raise IPP Screen
            End If
        End If


        hidopenerID.Value = Me.Request.QueryString("txtid") 'get from Raise IPP Screen
        hidopenerHIDID.Value = Me.Request.QueryString("hidid")
        hidopenerbtn.Value = Me.Request.QueryString("hidbtnid")
        hidopenerValID.Value = Me.Request.QueryString("hidvalid")
        hidprevopt.Value = Me.Request.QueryString("opt")
        hidprevtax.Value = Me.Request.QueryString("id")

        If Not IsPostBack Then

            If Request.QueryString("opt") IsNot Nothing Then
                If Request.QueryString("opt") = "1" Then
                    rdbWHTComp.Checked = True
                    txtWHT.Enabled = True
                    txtWHT.Text = strHoldingTax
                ElseIf Request.QueryString("opt") = "2" Then
                    rdbWHTVendor.Checked = True
                    txtWHT.Enabled = True
                    txtWHT.Text = strHoldingTax
                ElseIf Request.QueryString("opt") = "3" Then
                    rdbNoWHT.Checked = True
                    txtWHT.Enabled = False
                    txtNoWHtreason.Text = strHoldingTax
                    txtNoWHtreason.Enabled = True 'Jules 2018.07.05
                End If
            End If

            cmdClose.Attributes.Add("onclick", "selectOne();")
        End If
    End Sub
    Private Sub rdbNoWHT_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbNoWHT.CheckedChanged
        rdbWHTComp.Checked = False
        rdbWHTVendor.Checked = False
        txtWHT.Enabled = False
        txtNoWHtreason.Enabled = True
        txtWHT.Text = ""
    End Sub

    Private Sub rdbWHTComp_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbWHTComp.CheckedChanged
        rdbNoWHT.Checked = False
        rdbWHTVendor.Checked = False
        txtNoWHtreason.Enabled = False
        txtWHT.Enabled = True
        txtNoWHtreason.Text = ""
    End Sub

    Private Sub rdbWHTVendor_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbWHTVendor.CheckedChanged
        rdbNoWHT.Checked = False
        rdbWHTComp.Checked = False
        txtNoWHtreason.Enabled = False
        txtWHT.Enabled = True
        txtNoWHtreason.Text = ""
    End Sub
    Private Sub cmdClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Response.Write("<script language=""javascript"">window.close();</script>")
    End Sub


End Class