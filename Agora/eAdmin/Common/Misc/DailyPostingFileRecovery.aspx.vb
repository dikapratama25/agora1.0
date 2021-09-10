Imports AgoraLegacy
Imports System.Data

Public Class DailyPostingFileRecovery
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDb As  EAD.DBCom
    Dim msg As New Message
    Dim objipp As New IPP
    Dim objGlobal As New AppGlobals

    Protected WithEvents dgRecoveryList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents txtPaymentDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSendPreview As System.Web.UI.WebControls.Button
    Protected WithEvents cmdConfirmRecovery As System.Web.UI.WebControls.Button
    Protected WithEvents cmdAddtoList As System.Web.UI.WebControls.Button
    Protected WithEvents btnremoveline As System.Web.UI.WebControls.Button
    Protected WithEvents hidEmail As System.Web.UI.HtmlControls.HtmlInputHidden


    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strNewCSS = "true"
        MyBase.Page_Load(sender, e)
        GenerateTab()

        If Not IsPostBack Then
            txtPaymentDate.Text = DateTime.Today.AddDays(-1)
            Bindgrid()
        End If
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String

        '//Retrieve Data from Database
        Dim ds As DataSet = New DataSet
        ds = objipp.GetRecoveryList()

        '//for sorting asc or desc
        Dim dvRecoveryList As DataView
        dvRecoveryList = ds.Tables(0).DefaultView

        dvRecoveryList.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvRecoveryList.Sort += " DESC"


        intPageRecordCnt = ds.Tables(0).Rows.Count

        '//datagrid.pageCount only got value after databind

        If intPageRecordCnt > 0 Then
            ' check when user re-enter search criteria and click on other page without click search button
            If dgRecoveryList.CurrentPageIndex > (dvRecoveryList.Count \ dgRecoveryList.PageSize) Then
                dgRecoveryList.CurrentPageIndex = IIf((dvRecoveryList.Count \ dgRecoveryList.PageSize) = 1, 0, (dvRecoveryList.Count \ dgRecoveryList.PageSize))
            ElseIf dgRecoveryList.CurrentPageIndex = (dvRecoveryList.Count \ dgRecoveryList.PageSize) Then
                If ViewState("PageCount") = (dvRecoveryList.Count \ dgRecoveryList.PageSize) Then
                    'user does not re-enter search criteria 
                    dgRecoveryList.CurrentPageIndex = IIf((dvRecoveryList.Count \ dgRecoveryList.PageSize) = 0, 0, (dvRecoveryList.Count \ dgRecoveryList.PageSize) - 1)
                Else
                    If (dvRecoveryList.Count Mod dgRecoveryList.PageSize) = 0 Then
                        ' total record = 10, 20, ...
                        dgRecoveryList.CurrentPageIndex = IIf((dvRecoveryList.Count \ dgRecoveryList.PageSize) = 1, 0, (dvRecoveryList.Count \ dgRecoveryList.PageSize))
                    Else
                        ' total record = 11, 12, ...
                        dgRecoveryList.CurrentPageIndex = (dvRecoveryList.Count \ dgRecoveryList.PageSize)
                    End If
                End If
            End If
            '--------------------------------
            cmdSendPreview.Enabled = True
            cmdConfirmRecovery.Enabled = True
            dgRecoveryList.DataSource = dvRecoveryList
            dgRecoveryList.DataBind()
        Else
            cmdSendPreview.Enabled = False
            cmdConfirmRecovery.Enabled = False
            dgRecoveryList.DataBind()
        End If
        ' add for above checking
        ViewState("PageCount") = dgRecoveryList.PageCount
        'ShowStats()
    End Function

    Private Sub cmdAddtoList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddtoList.Click

        Dim intMsg As Integer

        dgRecoveryList.CurrentPageIndex = 0

        If validateField() Then
            intMsg = objipp.InsertRecoveryList(Common.ConvertDate(txtPaymentDate.Text))

            Select Case intMsg
                Case WheelMsgNum.Duplicate
                    Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00324"), MsgBoxStyle.Information)
                Case WheelMsgNum.NotSave
                    Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00326"), MsgBoxStyle.Information)

            End Select
        End If


        Bindgrid()
    End Sub

    Private Sub dgRecoveryList_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgRecoveryList.ItemCreated
        Grid_ItemCreated(dgRecoveryList, e)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgRecoveryList.SortCommand
        Grid_SortCommand(sender, e)
        dgRecoveryList.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Public Sub dgRecoveryList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRecoveryList.PageIndexChanged
        dgRecoveryList.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub




    Private Sub cmdSendPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSendPreview.Click

        Dim dgItem As DataGridItem
        Dim ds As DataSet = New DataSet
        ds = objipp.GetRecoveryList()

        Try
            If hidEmail.Value <> "" Then
                objipp.sendRecoveryConfirmMail(ds, hidEmail.Value)
                Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00330"), MsgBoxStyle.Information)
            Else
                Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00329"), MsgBoxStyle.Information)
            End If
        Catch errExp As CustomException
            Common.TrwExp(errExp)
        Catch errExp1 As Exception
            Common.TrwExp(errExp1)
        End Try
    End Sub

    Private Sub cmdConfirmRecovery_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdConfirmRecovery.Click
        Dim dgItem As DataGridItem
        Dim intMsg As Integer
        Dim i As Integer
        Dim aryIdx As New ArrayList

        Try
            For Each dgItem In dgRecoveryList.Items
                aryIdx.Add(dgItem.Cells(5).Text)
            Next

            intMsg = objipp.ConfirmRecovery(aryIdx)

            Select Case intMsg
                Case WheelMsgNum.Save
                    Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00327"), MsgBoxStyle.Information)

            End Select

            Bindgrid()

        Catch errExp As CustomException
            Common.TrwExp(errExp)
        Catch errExp1 As Exception
            Common.TrwExp(errExp1)
        End Try
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If


        Session("w_FileRecovery_tabs") = "<div class=""t_entity""><ul>" &
             "<li><div class=""space""></div></li>" &
                         "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Misc", "DailyPostingFileRecovery.aspx", "pageid=" & strPageId) & """><span>File Recovery</span></a></li>" &
                         "<li><div class=""space""></div></li>" &
                         "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Misc", "DailyPostingFileRecoveryConfirmList.aspx", "pageid=" & strPageId) & """><span>Confirmed Listing</span></a></li>" &
                         "</ul></div>"


    End Sub

    Protected Sub dgRecoveryList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgRecoveryList.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim cmdremove As HtmlGenericControl
            cmdremove = e.Item.FindControl("cmdremove")
            cmdremove.InnerHtml = "<img src='" & dDispatcher.direct("Plugins/Images", "i_delete2.gif") & "' />"
            cmdremove.Attributes.Add("onclick", "remove('" & e.Item.Cells(5).Text & "')")

            e.Item.Cells(1).Text = CDate(e.Item.Cells(1).Text).ToString("dd/MM/yyyy") '.Substring(0, 10)
            e.Item.Cells(3).Text = (Format(CDbl(e.Item.Cells(3).Text), "#,###.00"))
            e.Item.Cells(4).Text = (Format(CDbl(e.Item.Cells(4).Text), "#,###.00"))

        End If
    End Sub
    Private Sub btnremoveline_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnremoveline.Click
        Dim slineno As String

        slineno = Request.Form("hidlinepointer")
        If objipp.DeleteRecoveryList(slineno) Then
            Common.NetMsgbox(Me, Common.RecordDelete, MsgBoxStyle.Information)

        End If
        Bindgrid()
    End Sub
    Private Function validateField(Optional ByVal strform As String = "") As Boolean
        Dim count, i As Integer
        Dim ds As New DataSet
        If txtPaymentDate.Text > System.DateTime.Today Then
            Return False
        End If
        Return True
    End Function
End Class