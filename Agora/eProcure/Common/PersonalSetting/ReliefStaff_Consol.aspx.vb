Imports AgoraLegacy
Imports eProcure.Component

Public Class ReliefStaff_Consol
    Inherits AgoraLegacy.AppBaseClass
    Dim objDb As New EAD.DBCom
    Dim objPersonal As New PersonalSetting
    Dim strstart As String
    Protected WithEvents txtDateFr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDateTo As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmd_canceldiv As System.Web.UI.WebControls.Button
    Protected WithEvents txtStart As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtEnd As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmd_cancel As System.Web.UI.WebControls.Button
    Protected WithEvents txtDatenew As System.Web.UI.WebControls.TextBox
    Protected WithEvents cboConsolRelief As System.Web.UI.WebControls.DropDownList
    Protected WithEvents rfv_date As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents cmdExtend As System.Web.UI.WebControls.Button
    Protected WithEvents vldDateFtDateTo As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents vldDateFr As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldDateTo As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Validationsummary1 As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents lblnewdate As System.Web.UI.WebControls.Label
    Protected WithEvents lbldate As System.Web.UI.WebControls.Label
    Protected WithEvents cal1 As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents cal2 As System.Web.UI.HtmlControls.HtmlGenericControl
    Dim strend As String

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_add_mod As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents cmdsave_newdate As System.Web.UI.WebControls.Button
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdcancel As System.Web.UI.WebControls.Button
    Protected WithEvents hide As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents hidMode As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents cmdclear As System.Web.UI.WebControls.Button
    Protected WithEvents cboRelief As System.Web.UI.WebControls.DropDownList

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)

        rfv_date.Enabled = False

        If Not IsPostBack Then

            Dim objConsol As New ReliefConsolValue
            Dim onjconolID As New ReliefconsolId
            objPersonal.get_StartEndDateConsol(objConsol)


            strstart = objConsol.Consol_StartDate
            strend = objConsol.Consol_EndDate
            viewstate("valIndex") = objConsol.Consol_Index
            cmdExtend.Enabled = False
            cmdcancel.Enabled = False
            cmdSave.Enabled = False
            If strstart <> "" Then
                objPersonal.get_consolId(onjconolID, viewstate("valIndex"))
                viewstate("valconsol") = onjconolID.Consol_id

                cal1.Style("display") = "none"
                cal2.Style("display") = "none"
                txtDateFr.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, strstart)
                txtDateTo.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, strend)
                cmdExtend.Enabled = True
                cmdcancel.Enabled = True
                If Format(CDate(txtDateFr.Text), "yyyy/MM/dd") > Format(Now.Today, "yyyy/MM/dd") Then
                    cmdExtend.Enabled = True
                    cmdcancel.Enabled = True
                ElseIf Format(CDate(txtDateFr.Text), "yyyy/MM/dd") < Format(Now.Today, "yyyy/MM/dd") Then
                    cmdExtend.Enabled = True
                    cmdcancel.Enabled = True
                End If
            Else
                cmdSave.Enabled = True
                cal1.Style("display") = "inline"
                cal2.Style("display") = "inline"
            End If
            BindData()
            Common.SelDdl(viewstate("valconsol"), cboConsolRelief, True, False)
        End If
        cmdcancel.Attributes.Add("onclick", "return confirm('" & MsgForDeleteButton & "');")
    End Sub
    Function validation()
        Dim objConsol As New ReliefConsolValue
        Dim onjconolID As New ReliefconsolId
        objPersonal.get_StartEndDateConsol(objConsol)

        strstart = objConsol.Consol_StartDate
        strend = objConsol.Consol_EndDate
        viewstate("valIndex") = objConsol.Consol_Index
        cmdExtend.Enabled = False
        cmdcancel.Enabled = False
        cmdSave.Enabled = False
        If strstart <> "" Then
            objPersonal.get_consolId(onjconolID, viewstate("valIndex"))
            viewstate("valconsol") = onjconolID.Consol_id

            cal1.Style("display") = "none"
            cal2.Style("display") = "none"
            txtDateFr.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, strstart)
            txtDateTo.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, strend)
            cmdExtend.Enabled = True
            cmdcancel.Enabled = True
            If Format(CDate(txtDateFr.Text), "yyyy/MM/dd") > Format(Now.Today, "yyyy/MM/dd") Then
                cmdExtend.Enabled = True
                cmdcancel.Enabled = True
            ElseIf Format(CDate(txtDateFr.Text), "yyyy/MM/dd") < Format(Now.Today, "yyyy/MM/dd") Then
                cmdExtend.Enabled = True
                cmdcancel.Enabled = True
            End If
        Else
            cmdSave.Enabled = True
            cal1.Style("display") = "inline"
            cal2.Style("display") = "inline"
        End If
        Common.SelDdl(viewstate("valconsol"), cboConsolRelief, True, False)
    End Function

    Public Function BindData()
        Dim dvcustom As DataView
        dvcustom = objPersonal.bindReliefconsol()
        cboConsolRelief.Items.Clear()
        If Not dvcustom Is Nothing Then
            Dim cbolist As New ListItem
            Common.FillDdl(cboConsolRelief, "two", "UM_USER_ID", dvcustom)
            cbolist.Value = ""
            cbolist.Text = "---Select---"
            cboConsolRelief.Items.Insert(0, cbolist)
        Else
            cmdSave.Enabled = False
        End If

    End Function

    Private Sub cmd_cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_cancel.Click
        hide.Style("display") = "none"
        lblnewdate.Visible = False
    End Sub
    Private Function isValidNewDate(ByRef strMsg As String) As Boolean
        isValidNewDate = True
        If txtDatenew.Text = "" Then
            isValidNewDate = False
            strMsg = "New End Date is required."
        Else
            If Format(CDate(txtDatenew.Text), "yyyyMMdd") < Format(CDate(txtDateTo.Text), "yyyyMMdd") Then
                isValidNewDate = False
                strMsg = "New End Date Must > End Date."
            ElseIf Format(CDate(txtDatenew.Text), "yyyyMMdd") > Format(CDate(txtDateTo.Text), "yyyyMMdd") Then
                isValidNewDate = True
            ElseIf Format(CDate(txtDatenew.Text), "yyyyMMdd") = Format(CDate(txtDateTo.Text), "yyyyMMdd") Then
                isValidNewDate = False
                strMsg = "New End Date Must > End Date."
            End If
        End If
    End Function
    Private Sub cmdsave_newdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdsave_newdate.Click
        Dim strvaldate As String = txtDatenew.Text
        Dim strMsg As String
        If isValidNewDate(strMsg) = False Then
            lblnewdate.Visible = True
            lblnewdate.Text = "<ul type='disc'><li>" & strMsg & "<ul type='disc'></ul></li></ul>"
        Else
            lblnewdate.Visible = False
            objPersonal.updateNewReliefDateConsol(txtDatenew.Text)
            txtDateTo.Text = txtDatenew.Text
            txtDatenew.Text = ""
            Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
        End If

    End Sub

    Private Function isValidDate(ByRef strMsg As String) As Boolean
        isValidDate = True
        If Format(CDate(txtDateTo.Text), "yyyyMMdd") < Format(Date.Now, "yyyyMMdd") Then
            lbldate.Visible = True
            strMsg = "End Date Must >= Today Date"
            isValidDate = False
        ElseIf Format(CDate(txtDateFr.Text), "yyyyMMdd") < Format(Date.Now, "yyyyMMdd") Then
            lbldate.Visible = True
            strMsg = "Start Date Must >= Today Date"
            isValidDate = False
        End If
    End Function

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim dgStaffRelief As DataGridItem
        Dim i As Integer = 0
        Dim valGrp, valAAo, valrelief, strRauIndex, valseq As String
        valAAo = cboConsolRelief.SelectedItem.Value
        If cboConsolRelief.SelectedItem.Text = "---Select---" Then
            Common.NetMsgbox(Me, "Please Select to Proceed", MsgBoxStyle.Information)
        Else
            Dim strMsg As String
            If isValidDate(strMsg) = False Then
                lbldate.Visible = True
                lbldate.Text = strMsg
            Else
                lbldate.Visible = False
                objPersonal.updateReliefConsolStaff(viewstate("valIndex"), valAAo, txtDateFr.Text, txtDateTo.Text)
                Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
                cboConsolRelief.SelectedIndex = 0
                cmdSave.Enabled = False
                Call validation()
            End If
        End If
    End Sub

    Private Sub cmdcancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdcancel.Click
        Dim valGrp, valAAo, valrelief, strRauIndex, valseq As String
        valAAo = cboConsolRelief.SelectedItem.Value
        objPersonal.DelReliefStaff(viewstate("valIndex"))
        objPersonal.DelReliefConsol_MSTR(viewstate("valIndex"))
        Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
        txtDateTo.Text = ""
        txtDateFr.Text = ""
        Call validation()
        cboConsolRelief.SelectedIndex = 0
        cmdSave.Enabled = True
        lblnewdate.Visible = False
    End Sub

    Private Sub cmdclear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdclear.Click
        txtDatenew.Text = ""
        lblnewdate.Visible = False
    End Sub

    Private Sub cmdExtend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdExtend.Click
        hide.Style("display") = "inline"
        lbl_add_mod.Text = "add"
    End Sub
End Class
