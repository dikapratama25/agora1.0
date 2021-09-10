Imports AgoraLegacy
Imports eProcure.Component

Partial Public Class AccCode
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim strFrm As String
    Dim objGlobal As New AppGlobals

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        strNewCSS = "true"
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgAccountCode)
        If Not IsPostBack Then
            Me.cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
            Me.cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
            Me.cmdModify.Style("display") = "none"
            Me.cmdDelete.Style("display") = "none"
            GenerateTab()
        End If
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgAccountCode.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgAccountCode.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim objBudgetControl As New BudgetControl
        Dim ds As DataSet = New DataSet
        ds = objBudgetControl.GetAccountMapping(Me.txtBranchCode.Text, Me.txtGLCode.Text, Me.txtCostCenter.Text, Me.txtInterfaceCode.Text, Me.txtBranchCodeTo.Text, Me.txtGLCodeTo.Text, Me.txtCostCenterTo.Text, Me.txtInterfaceCodeTo.Text, "", "")

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt
        If intPageRecordCnt > 0 Then

            resetDatagridPageIndex(dtgAccountCode, dvViewSample)
            dtgAccountCode.DataSource = dvViewSample
            dtgAccountCode.DataBind()
            Me.cmdModify.Style("display") = ""
            Me.cmdDelete.Style("display") = ""
        Else
            dtgAccountCode.DataBind()
            Me.cmdModify.Style("display") = "none"
            Me.cmdDelete.Style("display") = "none"
            Common.NetMsgbox(Me, MsgNoRecord)
        End If
        ' add for above checking
        ViewState("PageCount") = dtgAccountCode.PageCount
    End Function

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        If strFrm <> "ItemCat" Then
            Session("w_AccountCode_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                                      "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Interface", "AccCode.aspx", "pageid=" & strPageId) & """><span>Account Code</span></a></li>" & _
                                      "<li><div class=""space""></div></li>" & _
                                      "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Interface", "AccCodeAudit.aspx", "pageid=" & strPageId) & """><span>Audit</span></a></li>" & _
                                      "<li><div class=""space""></div></li>" & _
                                      "</ul><div></div></div>"
        Else
            Session("w_AccountCode_tabs") = Nothing
        End If

    End Sub

    Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Dim strscript As New System.Text.StringBuilder
        Dim objBC As New BudgetControl
        Dim strFileName, rowcount As String

        rowcount = objBC.GetLatestLineNoForAccountMapping()
        strscript.Append("<script language=""javascript"">")
        strFileName = dDispatcher.direct("Interface", "AccCodePop.aspx", "rowcount=" & rowcount)
        strFileName = Server.UrlEncode(strFileName)
        strscript.Append("ShowDialog(""" & dDispatcher.direct("Initial", "Dialog.aspx", "page=" & strFileName) & """,'300px');")
        strscript.Append("document.getElementById('btnhidden').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script1", strscript.ToString())
    End Sub

    Private Sub cmdModify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdModify.Click
        Dim strGLCode As String = ""
        Dim rowcount As Integer = 0
        Dim strFileName As String
        Dim fromacctindex, toacctindex As String
        Dim strscript As New System.Text.StringBuilder

        For Each row As DataGridItem In Me.dtgAccountCode.Items
            Dim chk As CheckBox = row.FindControl("chkSelection")
            If chk.Checked Then
                rowcount = rowcount + 1

                If rowcount = 1 Then
                    fromacctindex = row.Cells(2).Text
                    toacctindex = row.Cells(4).Text
                End If
            End If
        Next

        If rowcount > 1 Then
            AgoraLegacy.Common.NetMsgbox(Me, "You may edit only 1 record at a time.")
            Exit Sub
        ElseIf rowcount = 1 Then
            strscript.Append("<script language=""javascript"">")
            strFileName = dDispatcher.direct("Interface", "AccCodePop.aspx", "action=edit&fromacctindex=" & fromacctindex & "&toacctindex=" & toacctindex)
            strFileName = Server.UrlEncode(strFileName)
            strscript.Append("ShowDialog(""" & dDispatcher.direct("Initial", "Dialog.aspx", "page=" & strFileName) & """,'300px');")
            strscript.Append("document.getElementById('btnhidden').click();")
            strscript.Append("</script>")
            RegisterStartupScript("script2", strscript.ToString())
        End If
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim objBC As New BudgetControl
        Dim objDB As New EAD.DBCom
        Dim ds As New DataSet
        Dim aryDoc As New ArrayList
        Dim strsql, strmsg As String
        Dim strFromBR, strFromGL, strFromCC, strFromInterface As String
        Dim strToBR, strToGL, strToCC, strToInterface As String
        Dim i As Integer = 0

        For Each row As DataGridItem In Me.dtgAccountCode.Items
            Dim chk As CheckBox = row.FindControl("chkSelection")
            If chk.Checked Then
                Dim strSplitFrom() As String = Split(row.Cells(3).Text, " : ")
                If strSplitFrom.Length > 1 Then
                    'strFromBR = strSplitFrom(0)
                    Dim strSplitTo() As String = Split(row.Cells(5).Text, " : ")
                    If strSplitTo.Length > 1 Then
                        ds = objBC.GetAccountMapping(strSplitFrom(0), strSplitFrom(1), strSplitFrom(2), strSplitFrom(3), strSplitTo(0), strSplitTo(1), strSplitTo(2), strSplitTo(3), row.Cells(2).Text, row.Cells(4).Text)
                        If ds.Tables(0).Rows.Count > 0 Then
                            aryDoc.Add(New String() {row.Cells(6).Text})
                        End If
                    End If
                End If


            End If
        Next
        If aryDoc.Count > 0 Then
            If objBC.DeleteAccountMapping(aryDoc) Then
                strmsg = objGlobal.GetErrorMessage("00004")
                Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)
            Else
                strmsg = objGlobal.GetErrorMessage("00008")
                Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)
            End If
            Bindgrid()
        End If
    End Sub

    Private Sub btnhidden_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhidden.Click
        Bindgrid()
    End Sub

    Private Sub dtgAccountCode_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAccountCode.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgAccountCode, e)
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Public Sub dtgAccountCode_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtgAccountCode.CurrentPageIndex = e.NewPageIndex
        Bindgrid(0)
    End Sub
End Class