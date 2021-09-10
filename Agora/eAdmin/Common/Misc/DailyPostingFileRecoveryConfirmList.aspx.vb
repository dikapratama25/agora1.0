Imports AgoraLegacy
Imports System.Data

Partial Class DailyPostingFileRecoveryConfirmList
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDb As  EAD.DBCom
    Dim msg As New Message
    Dim objipp As New IPP
    Dim objGlobal As New AppGlobals

    Protected WithEvents dgRecoveryConfirmList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents txtPaymentSDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPaymentEDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRecoverySDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRecoveryEDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button


    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strNewCSS = "true"
        MyBase.Page_Load(sender, e)
        GenerateTab()


        If Not IsPostBack Then
            Bindgrid()
        End If
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String

        '//Retrieve Data from Database
        Dim ds As DataSet = New DataSet
        Dim paySdate, payEdate, revSdate, revEdate As String

        'If txtPaymentSDate.Text <> "" Then
        '    paySdate = txtPaymentSDate.Text
        'Else
        paySdate = ""
        'End If
        'If txtPaymentEDate.Text <> "" Then
        '    payEdate = txtPaymentEDate.Text
        'Else
        payEdate = ""
        'End If
        'If txtRecoverySDate.Text <> "" Then
        '    revSdate = txtRecoverySDate.Text
        'Else
        revSdate = ""
        'End If

        'If txtRecoveryEDate.Text <> "" Then
        '    revEdate = txtRecoveryEDate.Text
        'Else
        revEdate = ""
        'End If

        ds = objipp.GetRecoveryConfirmList(paySdate, payEdate, revSdate, revEdate)

        '//for sorting asc or desc
        Dim dvRecoveryList As DataView
        dvRecoveryList = ds.Tables(0).DefaultView

        dgRecoveryConfirmList.DataSource = dvRecoveryList
        dgRecoveryConfirmList.DataBind()

        dvRecoveryList.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvRecoveryList.Sort += " DESC"


        intPageRecordCnt = ds.Tables(0).Rows.Count

        '//datagrid.pageCount only got value after databind
        If intPageRecordCnt > 0 Then

            ' check when user re-enter search criteria and click on other page without click search button
            If dgRecoveryConfirmList.CurrentPageIndex > (dvRecoveryList.Count \ dgRecoveryConfirmList.PageSize) Then
                dgRecoveryConfirmList.CurrentPageIndex = IIf((dvRecoveryList.Count \ dgRecoveryConfirmList.PageSize) = 1, 0, (dvRecoveryList.Count \ dgRecoveryConfirmList.PageSize))
            ElseIf dgRecoveryConfirmList.CurrentPageIndex = (dvRecoveryList.Count \ dgRecoveryConfirmList.PageSize) Then
                If ViewState("PageCount") = (dvRecoveryList.Count \ dgRecoveryConfirmList.PageSize) Then
                    'user does not re-enter search criteria 
                    dgRecoveryConfirmList.CurrentPageIndex = IIf((dvRecoveryList.Count \ dgRecoveryConfirmList.PageSize) = 0, 0, (dvRecoveryList.Count \ dgRecoveryConfirmList.PageSize) - 1)
                Else
                    If (dvRecoveryList.Count Mod dgRecoveryConfirmList.PageSize) = 0 Then
                        ' total record = 10, 20, ...
                        dgRecoveryConfirmList.CurrentPageIndex = IIf((dvRecoveryList.Count \ dgRecoveryConfirmList.PageSize) = 1, 0, (dvRecoveryList.Count \ dgRecoveryConfirmList.PageSize))
                    Else
                        ' total record = 11, 12, ...
                        dgRecoveryConfirmList.CurrentPageIndex = (dvRecoveryList.Count \ dgRecoveryConfirmList.PageSize)
                    End If
                End If
            End If
            '--------------------------------
            'cmdDelete.Enabled = True
            'cmdSendPreview.Enabled = True
            'cmdConfirmRecovery.Enabled = True
            '//mean Enable, can't use button.Enabled because this is a HTML button
            'cmdReset.Disabled = False
            dgRecoveryConfirmList.DataSource = dvRecoveryList
            dgRecoveryConfirmList.DataBind()

        Else
            'cmdSendPreview.Enabled = False
            'cmdConfirmRecovery.Enabled = False
            'cmdDelete.Enabled = False
            'cmdReset.Disabled = True

            dgRecoveryConfirmList.DataBind()
            'Common.NetMsgbox(Me, msg.GetMessage(1004))
        End If
        ' add for above checking
        ViewState("PageCount") = dgRecoveryConfirmList.PageCount
        'ShowStats()
    End Function

    Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dgRecoveryConfirmList.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub dgRecoveryConfirmList_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgRecoveryConfirmList.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dgRecoveryConfirmList, e)
        '//to add a JavaScript to CheckAll button

    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgRecoveryConfirmList.SortCommand
        Grid_SortCommand(sender, e)
        dgRecoveryConfirmList.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Public Sub dgRecoveryConfirmList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRecoveryConfirmList.PageIndexChanged
        dgRecoveryConfirmList.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Sub New()

    End Sub


    Private Sub dgRecoveryConfirmList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgRecoveryConfirmList.ItemDataBound

        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            e.Item.Cells(0).Text = CDate(e.Item.Cells(0).Text).ToString("d/M/yyyy")

            If dv("frt_posted_date") IsNot System.DBNull.Value Then
                e.Item.Cells(1).Text = CDate(e.Item.Cells(1).Text).ToString("d/M/yyyy")
            End If

            e.Item.Cells(3).Text = (Format(CDbl(e.Item.Cells(3).Text), "#,###.00"))
            e.Item.Cells(4).Text = (Format(CDbl(e.Item.Cells(4).Text), "#,###.00"))


        End If

    End Sub
    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If


        Session("w_FileRecovery_tabs") = "<div class=""t_entity""><ul>" &
     "<li><div class=""space""></div></li>" &
                 "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Misc", "DailyPostingFileRecovery.aspx", "pageid=" & strPageId) & """><span>File Recovery</span></a></li>" &
                 "<li><div class=""space""></div></li>" &
                 "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Misc", "DailyPostingFileRecoveryConfirmList.aspx", "pageid=" & strPageId) & """><span>Confirmed Listing</span></a></li>" &
                 "</ul></div>"


    End Sub

End Class