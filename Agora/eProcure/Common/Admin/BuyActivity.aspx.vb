Imports AgoraLegacy
Imports eProcure.Component

Public Class BuyActivity
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents chkList As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkContract As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkDisc As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button

    Dim strMode As String
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
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmdSave)
        htPageAccess.Add("add", alButtonList)
        htPageAccess.Add("update", alButtonList)
        CheckButtonAccess()
        'cmd_Reset.Enabled = blnCanAdd Or blnCanUpdate Or blnCanDelete
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        'strMode = Me.Request.QueryString("mode")
        If Not Page.IsPostBack Then
            displayFlag()
        End If

    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Dim objAdmin As New Admin
        objAdmin.updateComSetting(IIf(chkList.Checked, 1, 0), IIf(chkDisc.Checked, 1, 0), IIf(chkContract.Checked, 1, 0))



        'strDCode = Me.chkList.Checked
        'strDName = Me.txt_deptName.Text

        'strRedirect = "DeptSetup.aspx?pageid=" & strPageId
        'If strMode = "add" Then
        '    objAdmin.addDept(strDCode, strDName)
        'Else
        '    objAdmin.moddept(strDCode, strDName)
        'End If
        Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
        objAdmin = Nothing
    End Sub

    Private Sub displayFlag()
        Dim dsFlag As New DataSet
        Dim objAdmin As New Admin
        Dim intCount As Integer
        Dim i As Integer
        dsFlag = objAdmin.getFlag
        intCount = dsFlag.Tables(0).Rows.Count()
        For i = 0 To intCount - 1
            '  If dsFlag.Tables(0).Rows(i) = " Then
            Select Case dsFlag.Tables(0).Rows(i)("CS_FLAG_NAME")
                Case "Buy Fixed Price"
                    If dsFlag.Tables(0).Rows(i)("CS_FLAG_VALUE") = 1 Then
                        chkList.Checked = True
                    Else
                        chkList.Checked = False

                    End If
                Case "Buy Discount Price"
                    If dsFlag.Tables(0).Rows(i)("CS_FLAG_VALUE") = 1 Then
                        chkDisc.Checked = True
                    Else
                        chkDisc.Checked = False

                    End If

                Case "Buy Contract Price"
                    If dsFlag.Tables(0).Rows(i)("CS_FLAG_VALUE") = 1 Then
                        chkContract.Checked = True
                    Else
                        chkContract.Checked = False
                    End If

                    'Case "Free Format Item"
                    '    If dsFlag.Tables(0).Rows(i)("CS_FLAG_VALUE") = 1 Then
                    '        chkFormat.Checked = True
                    '    Else
                    '        chkFormat.Checked = False
                    '    End If

            End Select

        Next
    End Sub

End Class
