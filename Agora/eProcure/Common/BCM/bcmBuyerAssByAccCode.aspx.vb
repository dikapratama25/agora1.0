Imports AgoraLegacy
Imports eProcure.Component
Public Class bcmBuyerAssByAccCode
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents cmdRight As System.Web.UI.WebControls.Button
    Protected WithEvents cmdLeft As System.Web.UI.WebControls.Button
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdReset As System.Web.UI.WebControls.Button
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblAccCode As System.Web.UI.WebControls.Label
    Protected WithEvents lblDesc As System.Web.UI.WebControls.Label
    Protected WithEvents lblDeptName As System.Web.UI.WebControls.Label
    Protected WithEvents lstUserAvail As System.Web.UI.WebControls.ListBox
    Protected WithEvents lstUserSelected As System.Web.UI.WebControls.ListBox

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

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
        htPageAccess.Add("update", alButtonList)

        CheckButtonAccess()
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)

        viewstate("AccIndex") = Request.Params("accindex")
        viewstate("AccList") = Request.Params("acclist")
        If Not Page.IsPostBack Then
            Populate()
        End If
        lnkBack.NavigateUrl = dDispatcher.direct("BCM", "bcmSearchBuyerAss.aspx", "pageid=" & strPageId & "&mode=" & "Acc")

    End Sub

    Private Sub Populate()
        Dim objBCM As New BudgetControl
        Dim dt As DataTable
        Dim ds As DataSet
        Dim DeptCode As String
        lblAccCode.Text = viewstate("AccList")

        dt = objBCM.GetBindInfo(viewstate("AccIndex"))
        If dt.Rows.Count > 0 Then
            DeptCode = IIf(IsDBNull(dt.Rows(0).Item("CDM_DEPT_CODE")), "", dt.Rows(0).Item("CDM_DEPT_CODE"))
            lblDeptName.Text = IIf(IsDBNull(dt.Rows(0).Item("CDM_DEPT_NAME")), "", dt.Rows(0).Item("CDM_DEPT_NAME"))
            lblDesc.Text = IIf(IsDBNull(dt.Rows(0).Item("AM_ACCT_DESC")), "", dt.Rows(0).Item("AM_ACCT_DESC"))
        End If

        ds = objBCM.getUserListbyAcc(viewstate("AccIndex"), DeptCode)

        Common.FillLst(lstUserAvail, "UM_USER_NAME", "UM_USER_ID", ds.Tables(1).DefaultView)
        Common.FillLst(lstUserSelected, "UM_USER_NAME", "UM_USER_ID", ds.Tables(0).DefaultView)
        Common.SortListControl(lstUserAvail, True)
        Common.SortListControl(lstUserSelected, True)


    End Sub

    Private Sub cmdLeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLeft.Click
        Dim item As ListItem
        Dim tempItem() As ListItem
        Dim i As Integer

        For Each item In lstUserSelected.Items
            If item.Selected = True Then
                lstUserAvail.Items.Insert(0, item)
                ReDim Preserve tempItem(i)
                tempItem(i) = item
                i = i + 1
            End If
        Next

        If lstUserSelected.SelectedIndex <> -1 Then
            For i = 0 To tempItem.Length - 1
                lstUserSelected.Items.Remove(tempItem(i))
            Next
        End If
        Common.SortListControl(lstUserAvail, True)

    End Sub

    Private Sub cmdRight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRight.Click
        Dim item As ListItem
        Dim tempItem() As ListItem
        'Dim objUser As Users
        Dim i As Integer

        For Each item In lstUserAvail.Items
            If item.Selected = True Then
                lstUserSelected.Items.Insert(0, item)
                ReDim Preserve tempItem(i)
                tempItem(i) = item
                i = i + 1
            End If
        Next

        If lstUserAvail.SelectedIndex <> -1 Then
            For i = 0 To tempItem.Length - 1
                lstUserAvail.Items.Remove(tempItem(i))
            Next
        End If
        Common.SortListControl(lstUserSelected, True)

    End Sub

    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim objBCM As New BudgetControl
        Dim strSelectedUser As String

        strSelectedUser = Lst2String(lstUserSelected, False, False)
        If objBCM.UpdateAccUserByAcc(strSelectedUser, viewstate("AccIndex")) Then
            Common.NetMsgbox(Me, objBCM.Message, dDispatcher.direct("BCM", "bcmSearchBuyerAss.aspx", "pageid=" & strPageId & "&mode=" & "Acc"), MsgBoxStyle.Information)
        Else
            Common.NetMsgbox(Me, objBCM.Message, MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub cmdReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        Populate()
    End Sub

 
End Class
