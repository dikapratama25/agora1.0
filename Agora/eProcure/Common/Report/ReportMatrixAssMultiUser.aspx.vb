'Copyright © 2013 STRATEQ GLOBAL SERVICES. All rights reserved.
Imports AgoraLegacy
Imports eProcure.Component

Public Class ReportMatrixAssMultiUser

    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    '' ''Protected WithEvents cboCatalogueBuyer As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents lstBox2 As System.Web.UI.WebControls.ListBox
    Protected WithEvents lstBox4 As System.Web.UI.WebControls.ListBox
    Protected WithEvents cmdReset As System.Web.UI.WebControls.Button
    Protected WithEvents cmdAdd1 As System.Web.UI.WebControls.Button
    Protected WithEvents cmdAdd2 As System.Web.UI.WebControls.Button
    Protected WithEvents cmdAdd3 As System.Web.UI.WebControls.Button
    Protected WithEvents cmdAdd4 As System.Web.UI.WebControls.Button
    Protected WithEvents cmdsave As System.Web.UI.WebControls.Button
    Protected WithEvents lstBox1 As System.Web.UI.WebControls.ListBox
    Protected WithEvents lstBox3 As System.Web.UI.WebControls.ListBox
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblname As System.Web.UI.WebControls.Label
    Protected WithEvents lblcode As System.Web.UI.WebControls.Label
    Protected WithEvents cmdItemAsg As System.Web.UI.WebControls.Button
    Dim strMode As String
    Dim strRedirect As String
    Dim strMsg As String
    Dim strCatName, v, arr(0) As String
    Private ary1(), ary2() As String
    Dim ds As DataSet
    Dim li, li2 As ListItem
    Dim rpt As ListItem
    Dim objDb As New EAD.DBCom
    Dim objRpt As New Report
    Dim valArr1 As String
    Dim val As String
    Dim struri As String
    Dim cbolist As New ListItem
    Dim blnlstboxChg As Boolean


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
        cmdsave.Enabled = False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmdsave)
        htPageAccess.Add("add", alButtonList)
        htPageAccess.Add("update", alButtonList)
        'htPageAccess.Add("delete", alButtonList)
        CheckButtonAccess()
        'cmdsave.Enabled = blnCanAdd Or blnCanUpdate
        If IsPostBack Then
            '' ''If lstBox2.Items.Count > 0 Then
            '' ''    cmdsave.Enabled = blnCanAdd Or blnCanUpdate
            '' ''End If

            If lstBox2.Items.Count > 0 And lstBox4.Items.Count > 0 Then
                cmdsave.Enabled = True
                cmdReset.Enabled = True
            Else
                cmdsave.Enabled = False
                cmdReset.Enabled = False
            End If

        Else
            cmdsave.Enabled = False
        End If

    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Session("CompanyId") = "demo"
        'Session("UserId") = "moo"
        MyBase.Page_Load(sender, e)

        strMode = Me.Request.QueryString("mode")
        If Not IsPostBack Then

            'Clear the listbox
            lstbox1.Items.Clear()
            lstBox2.Items.Clear()
            lstbox3.Items.Clear()
            lstBox4.Items.Clear()

            'Diplay data to listbox
            SearchDataRpt()
            SearchDataUsr()

        End If

        If lstBox2.Items.Count > 0 And lstBox4.Items.Count > 0 Then
            cmdsave.Enabled = True
            cmdReset.Enabled = True
        Else
            cmdsave.Enabled = False
            cmdReset.Enabled = False
        End If

        lnkBack.NavigateUrl = dDispatcher.direct("Report", "ReportMatrix.aspx", "pageid=" & strPageId)
    End Sub

    Public Function SearchDataRpt()
        Dim dvAvailBuyer As DataView

        dvAvailBuyer = objRpt.BindListBox_ReportMatrixSearchDataRpt()

        lstBox3.DataSource = dvAvailBuyer
        lstBox3.DataTextField = "RM_REPORT_NAME"
        lstBox3.DataValueField = "RM_REPORT_INDEX"
        lstBox3.DataBind()
        cmdAdd3.Enabled = True
        cmdAdd4.Enabled = True
    End Function

    Public Function SearchDataUsr()
        Dim dvAvailBuyer As DataView

        dvAvailBuyer = objRpt.BindListBox_ReportMatrixSearchDataUsr()

        lstBox1.DataSource = dvAvailBuyer
        lstBox1.DataTextField = "THREE"
        lstBox1.DataValueField = "UM_USER_ID"
        lstBox1.DataBind()
        cmdAdd1.Enabled = True
        cmdAdd2.Enabled = True
    End Function

    Public Function Add1()
        For Each li In lstbox1.Items
            If li.Selected = True Then
                Dim lstItem As New ListItem
                lstItem.Text = li.Text
                lstItem.Value = li.Value
                lstBox2.Items.Add(lstItem)

            End If
        Next
        Dim counter As Integer
        For counter = (lstbox1.Items.Count - 1) To 0 Step -1
            If lstbox1.Items(counter).Selected = True Then
                lstbox1.Items.RemoveAt(counter)
            End If
        Next
        cmdAdd1.Enabled = True
        cmdAdd2.Enabled = True
    End Function

    Public Function Add3()
        For Each li In lstBox3.Items
            If li.Selected = True Then
                Dim lstItem As New ListItem
                lstItem.Text = li.Text
                lstItem.Value = li.Value
                lstBox4.Items.Add(lstItem)

            End If
        Next
        Dim counter As Integer
        For counter = (lstBox3.Items.Count - 1) To 0 Step -1
            If lstBox3.Items(counter).Selected = True Then
                lstBox3.Items.RemoveAt(counter)
            End If
        Next
        cmdAdd3.Enabled = True
        cmdAdd4.Enabled = True
    End Function

    Public Function lst2Ary(ByRef pListbox As System.Web.UI.WebControls.ListBox) As String()
        Dim ary(pListbox.Items.Count - 1) As String
        Dim lngLoop As Long
        For lngLoop = 0 To pListbox.Items.Count - 1
            ary(lngLoop) = pListbox.Items(lngLoop).Value
        Next
        lst2Ary = ary
    End Function

    Private Function Add2()
        Dim li As ListItem
        For Each li In lstBox2.Items
            If li.Selected = True Then
                Dim lstItem As New ListItem
                lstItem.Text = li.Text
                lstItem.Value = li.Value
                lstbox1.Items.Add(lstItem)
            End If
        Next
        Dim counter As Integer
        For counter = (lstBox2.Items.Count - 1) To 0 Step -1
            If lstBox2.Items(counter).Selected = True Then
                lstBox2.Items.RemoveAt(counter)
            End If
        Next
        cmdAdd1.Enabled = True
        cmdAdd2.Enabled = True
    End Function

    Private Function Add4()
        Dim li As ListItem
        For Each li In lstBox4.Items
            If li.Selected = True Then
                Dim lstItem As New ListItem
                lstItem.Text = li.Text
                lstItem.Value = li.Value
                lstBox3.Items.Add(lstItem)
            End If
        Next
        Dim counter As Integer
        For counter = (lstBox4.Items.Count - 1) To 0 Step -1
            If lstBox4.Items(counter).Selected = True Then
                lstBox4.Items.RemoveAt(counter)
            End If
        Next
        cmdAdd3.Enabled = True
        cmdAdd4.Enabled = True
    End Function

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        lstBox1.Items.Clear()
        lstBox2.Items.Clear()
        lstBox3.Items.Clear()
        lstBox4.Items.Clear()

        SearchDataRpt()
        SearchDataUsr()

        cmdAdd1.Enabled = True
        cmdAdd2.Enabled = True

        If lstBox2.Items.Count > 0 And lstBox4.Items.Count > 0 Then
            cmdsave.Enabled = True
            cmdReset.Enabled = True
        Else
            cmdsave.Enabled = False
            cmdReset.Enabled = False
        End If
    End Sub

    Private Sub cmdAdd1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd1.Click
        Add1()
        If lstBox2.Items.Count > 0 And lstBox4.Items.Count > 0 Then
            cmdsave.Enabled = True
            cmdReset.Enabled = True
        Else
            cmdsave.Enabled = False
            cmdReset.Enabled = False
        End If

    End Sub

    Private Sub cmdAdd2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd2.Click
        Add2()
        If lstBox2.Items.Count > 0 And lstBox4.Items.Count > 0 Then
            cmdsave.Enabled = True
            cmdReset.Enabled = True
        Else
            cmdsave.Enabled = False
            cmdReset.Enabled = False
        End If
    End Sub

    Private Sub cmdAdd3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd3.Click
        Add3()
        If lstBox2.Items.Count > 0 And lstBox4.Items.Count > 0 Then
            cmdsave.Enabled = True
            cmdReset.Enabled = True
        Else
            cmdsave.Enabled = False
            cmdReset.Enabled = False
        End If

    End Sub

    Private Sub cmdAdd4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd4.Click
        Add4()
        If lstBox2.Items.Count > 0 And lstBox4.Items.Count > 0 Then
            cmdsave.Enabled = True
            cmdReset.Enabled = True
        Else
            cmdsave.Enabled = False
            cmdReset.Enabled = False
        End If
    End Sub

    Private Sub cmdsave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdsave.Click
        Dim query(0) As String

        For Each li In lstBox2.Items
            For Each rpt In lstBox4.Items
                Dim dv As DataView = objRpt.CheckReportMatrixAsgUser(rpt.Value, li.Value)
                If Not dv Is Nothing Then
                    If dv.Count < 0 Then
                        objRpt.AddReportMatrixAsg(rpt.Value, li.Value)
                    End If
                Else
                    objRpt.AddReportMatrixAsg(rpt.Value, li.Value)
                End If
            Next
        Next

        lstBox1.Items.Clear()
        lstBox2.Items.Clear()
        lstBox3.Items.Clear()
        lstBox4.Items.Clear()

        SearchDataRpt()
        SearchDataUsr()
        Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
    End Sub

End Class
