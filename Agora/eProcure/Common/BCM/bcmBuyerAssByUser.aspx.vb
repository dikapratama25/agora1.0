Imports AgoraLegacy
Imports eProcure.Component
Public Class bcmBuyerAssByUser

    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblDeptName As System.Web.UI.WebControls.Label
    Protected WithEvents lblUserName As System.Web.UI.WebControls.Label
    Protected WithEvents lblUserID As System.Web.UI.WebControls.Label
    Protected WithEvents cmdRight As System.Web.UI.WebControls.Button
    Protected WithEvents cmdLeft As System.Web.UI.WebControls.Button
    Protected WithEvents lstAccAvail As System.Web.UI.WebControls.ListBox
    Protected WithEvents lstAccSelected As System.Web.UI.WebControls.ListBox
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdReset As System.Web.UI.WebControls.Button

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
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        viewstate("DeptId") = Request.Params("Deptid")
        viewstate("DeptName") = Request.Params("DeptName")
        viewstate("UserId") = Request.Params("userid")
        viewstate("UserName") = Request.Params("username")
        If Not Page.IsPostBack Then
            Populate()
        End If
        lnkBack.NavigateUrl = dDispatcher.direct("BCM", "bcmSearchBuyerAss.aspx", "pageid=" & strPageId & "&mode=" & "User")

    End Sub

    Private Sub Populate()

        lblDeptName.Text = viewstate("DeptName")
        lblUserName.Text = viewstate("UserName")
        lblUserID.Text = viewstate("UserId")


        Dim objBCM As New BudgetControl
        Dim ds As DataSet
        ds = objBCM.getBCMListByUserNew(ViewState("UserId"))

        Common.FillLst(lstAccAvail, "ACCT_LIST", "ACCT_INDEX", ds.Tables(1).DefaultView)
        Common.FillLst(lstAccSelected, "ACCT_LIST", "ACCT_INDEX", ds.Tables(0).DefaultView)
        Common.SortListControl(lstAccAvail, True)
        Common.SortListControl(lstAccSelected, True)

    End Sub

    Private Sub cmdLeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLeft.Click
        Dim item As ListItem
        Dim tempItem() As ListItem
        Dim i As Integer

        For Each item In lstAccSelected.Items
            If item.Selected = True Then
                lstAccAvail.Items.Insert(0, item)
                ReDim Preserve tempItem(i)
                tempItem(i) = item
                i = i + 1
            End If
        Next

        If lstAccSelected.SelectedIndex <> -1 Then
            For i = 0 To tempItem.Length - 1
                lstAccSelected.Items.Remove(tempItem(i))
            Next
        End If
        Common.SortListControl(lstAccAvail, True)

    End Sub

    Private Sub cmdRight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRight.Click
        Dim item As ListItem
        Dim tempItem() As ListItem
        'Dim objUser As Users
        Dim i As Integer

        For Each item In lstAccAvail.Items
            If item.Selected = True Then
                lstAccSelected.Items.Insert(0, item)
                ReDim Preserve tempItem(i)
                tempItem(i) = item
                i = i + 1
            End If
        Next

        If lstAccAvail.SelectedIndex <> -1 Then
            For i = 0 To tempItem.Length - 1
                lstAccAvail.Items.Remove(tempItem(i))
            Next
        End If
        Common.SortListControl(lstAccSelected, True)

    End Sub



    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim objBCM As New BudgetControl
        Dim strSelectedAcc As String

        strSelectedAcc = Lst2String(lstAccSelected, False, False)
        If objBCM.UpdateAccUserByUser(strSelectedAcc, viewstate("UserId")) Then
            Common.NetMsgbox(Me, objBCM.Message, dDispatcher.direct("BCM", "bcmSearchBuyerAss.aspx", "pageid=" & strPageId & "&mode=" & "User"), MsgBoxStyle.Information)
        Else
            Common.NetMsgbox(Me, objBCM.Message, MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub cmdReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        Populate()
    End Sub
End Class






















