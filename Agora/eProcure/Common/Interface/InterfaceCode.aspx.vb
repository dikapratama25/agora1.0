Imports AgoraLegacy
Imports eProcure.Component

Partial Public Class InterfaceCode
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim strFrm As String
    Dim objBudgetControl As New BudgetControl
    Dim objDb As New EAD.DBCom
    Dim objGlobal As New AppGlobals

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        strNewCSS = "true"
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgInterfaceCode)
        If Not IsPostBack Then
            GenerateTab()
            'Bindgrid(0)
        End If
    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim ds As DataSet = New DataSet
        ds = objBudgetControl.GetBR_GL_CC(txtBranchCode.Text, txtGLCode.Text, txtCostCenter.Text, txtInterfaceCode.Text)

        Dim dvViewItem As DataView
        dvViewItem = ds.Tables(0).DefaultView
        dvViewItem.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewItem.Sort += " DESC"

        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt
        If intPageRecordCnt > 0 Then

            resetDatagridPageIndex(dtgInterfaceCode, dvViewItem)
            dtgInterfaceCode.DataSource = dvViewItem
            dtgInterfaceCode.DataBind()
        Else
            dtgInterfaceCode.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If
        ' add for above checking
        ViewState("PageCount") = dtgInterfaceCode.PageCount
        cmd_save.Visible = True
    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgInterfaceCode.CurrentPageIndex = 0
        Bindgrid(dtgInterfaceCode.CurrentPageIndex, True)
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        If strFrm <> "ItemCat" Then
            Session("w_InterfaceCode_tabs") = "<div class=""t_entity""><ul>" &
                    "<li><div class=""space""></div></li>" &
                                      "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Interface", "InterfaceCode.aspx", "pageid=" & strPageId) & """><span>Interface Code</span></a></li>" &
                                      "<li><div class=""space""></div></li>" &
                                      "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Interface", "InterfaceCodeAuditTrail.aspx", "pageid=" & strPageId) & """><span>Audit</span></a></li>" &
                                      "<li><div class=""space""></div></li>" &
                                      "</ul><div></div></div>"
        Else
            Session("w_InterfaceCode_tabs") = Nothing
        End If

    End Sub

    Private Sub cmd_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_save.Click
        Dim ds As DataSet = New DataSet
        Dim dsCheck As DataSet = New DataSet
        Dim strInterfaceCode As String = "", strSql As String = "", strTemp As String = "", strMsg As String = ""
        Dim strAryQuery(0) As String
        Dim i, iSavedRecords As Integer
        Dim blnDuplicate As Boolean = False
        Dim objDb As New EAD.DBCom

        For Each dgItem As DataGridItem In dtgInterfaceCode.Items
            'Jules commented on 2014.08.05 - Case 7146
            'strInterfaceCode = CType(dgItem.FindControl("txtInterfaceCodeInput"), TextBox).Text

            ''Check for duplicates onscreen
            'If strInterfaceCode <> "" Then
            '    For Each dgItem2 As DataGridItem In dtgInterfaceCode.Items
            '        strTemp = CType(dgItem2.FindControl("txtInterfaceCodeInput"), TextBox).Text
            '        If CType(dgItem2.Cells(5).Text, Long) <> CType(dgItem.Cells(5).Text, Long) And Common.Parse(strTemp) = Common.Parse(strInterfaceCode) Then
            '            blnDuplicate = True
            '            Exit For
            '        End If
            '    Next               

            '    ''Check for duplicates in DB
            '    If Not blnDuplicate Then
            '        strSql = "SELECT * FROM interface_mapping where IM_MAPPING_CODE='" & Common.Parse(strInterfaceCode) & "'"
            '        dsCheck = objDb.FillDs(strSql)
            '        If dsCheck.Tables(0).Rows.Count > 0 Then
            '            For i = 0 To dsCheck.Tables(0).Rows.Count - 1
            '                If dsCheck.Tables(0).Rows(i).Item("IM_ACCT_INDEX") <> CType(dgItem.Cells(5).Text, Long) Then
            '                    blnDuplicate = True
            '                    Exit For
            '                End If
            '            Next
            '        End If
            '    Else
            '        Exit For
            '    End If
            'End If
            'Next

            'If Not blnDuplicate Then 
            'If strInterfaceCode <> "" Then
            'For Each dgItem As DataGridItem In dtgInterfaceCode.Items
            strInterfaceCode = CType(dgItem.FindControl("txtInterfaceCodeInput"), TextBox).Text
            strSql = "SELECT * FROM interface_mapping where IM_ACCT_INDEX='" & CType(dgItem.Cells(5).Text, Long) & "'"
            ds = objDb.FillDs(strSql)
            If ds.Tables(0).Rows.Count > 0 Then

                For i = 0 To ds.Tables(0).Rows.Count - 1
                    If ds.Tables(0).Rows(i).Item("IM_MAPPING_CODE") <> "" And ds.Tables(0).Rows(i).Item("IM_MAPPING_CODE") <> Common.Parse(strInterfaceCode) Then 'to modify existing InterfaceCode
                        strSql = "UPDATE interface_mapping SET IM_MAPPING_CODE = '"
                        strSql &= Common.Parse(strInterfaceCode) & "' "
                        strSql &= "WHERE IM_ACCT_INDEX='" & CType(dgItem.Cells(5).Text, Long) & "'"
                        'Common.Insert2Ary(strAryQuery, strSql)
                        'objDb.BatchExecute(strAryQuery)                        
                        If objDb.Execute(strSql) Then
                            objBudgetControl.InsertAuditTrailInterfaceMapping(ds.Tables(0).Rows(0).Item(0).ToString, dgItem.Cells(5).Text, Common.Parse(strInterfaceCode), dgItem.Cells(0).Text, dgItem.Cells(1).Text, dgItem.Cells(2).Text, dgItem.Cells(3).Text, "M")
                            iSavedRecords = iSavedRecords + 1
                        End If
                    ElseIf ds.Tables(0).Rows(i).Item("IM_MAPPING_CODE") = "" And ds.Tables(0).Rows(i).Item("IM_MAPPING_CODE") <> Common.Parse(strInterfaceCode) Then 'to add InterfaceCode that was previously deleted
                        strSql = "UPDATE interface_mapping SET IM_MAPPING_CODE = '"
                        strSql &= Common.Parse(strInterfaceCode) & "' "
                        strSql &= "WHERE IM_ACCT_INDEX='" & CType(dgItem.Cells(5).Text, Long) & "'"
                        If objDb.Execute(strSql) Then
                            objBudgetControl.InsertAuditTrailInterfaceMapping(ds.Tables(0).Rows(0).Item(0).ToString, dgItem.Cells(5).Text, strInterfaceCode, dgItem.Cells(0).Text, dgItem.Cells(1).Text, dgItem.Cells(2).Text, dgItem.Cells(3).Text, "N")
                            iSavedRecords = iSavedRecords + 1
                        End If
                    End If
                Next
            Else
                If strInterfaceCode <> "" Then
                    strSql = "INSERT INTO interface_mapping (IM_COY_ID,IM_ACCT_INDEX,IM_MAPPING_CODE) " & _
                            "VALUES ('" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', " & CType(dgItem.Cells(5).Text, Long) & ", '" & Common.Parse(strInterfaceCode) & "')"
                    'Common.Insert2Ary(strAryQuery, strSql)
                    'objDb.BatchExecute(strAryQuery)
                    If objDb.Execute(strSql) Then
                        strSql = "SELECT * FROM interface_mapping where IM_ACCT_INDEX='" & CType(dgItem.Cells(5).Text, Long) & "'"
                        ds = objDb.FillDs(strSql)
                        If ds.Tables(0).Rows.Count > 0 Then
                            objBudgetControl.InsertAuditTrailInterfaceMapping(ds.Tables(0).Rows(0).Item(0).ToString, dgItem.Cells(5).Text, strInterfaceCode, dgItem.Cells(0).Text, dgItem.Cells(1).Text, dgItem.Cells(2).Text, dgItem.Cells(3).Text, "N")
                            iSavedRecords = iSavedRecords + 1
                        End If
                    End If
                End If
            End If
        Next

        If iSavedRecords > 0 Then
            AgoraLegacy.Common.NetMsgbox(Me, MsgRecordSave)
        Else
            AgoraLegacy.Common.NetMsgbox(Me, MsgRecordNotSave)
        End If
        'Else
        'strMsg = objGlobal.GetErrorMessage("00002")
        'Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
        'End If
        'End Case 7146.
        'Bindgrid()
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgInterfaceCode.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub dtgInterfaceCode_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgInterfaceCode.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgInterfaceCode, e)
    End Sub

    Private Sub dtgInterfaceCode_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgInterfaceCode.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim ds As DataSet = New DataSet
            Dim strsql As String = ""
            Dim txtInterfaceCodeInput As TextBox
            txtInterfaceCodeInput = e.Item.FindControl("txtInterfaceCodeInput")
            txtInterfaceCodeInput.Attributes.Add("onKeyDown", "limitText (this, 20);")

            strsql = "SELECT * FROM interface_mapping where IM_ACCT_INDEX='" & dv("Acct Index") & "'"
            ds = objDb.FillDs(strsql)
            If ds.Tables(0).Rows.Count > 0 Then
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("IM_MAPPING_CODE").ToString) And ds.Tables(0).Rows(0).Item("IM_MAPPING_CODE").ToString <> "" Then
                    txtInterfaceCodeInput.Text = ds.Tables(0).Rows(0).Item("IM_MAPPING_CODE").ToString
                End If
            End If

        End If
    End Sub
    Public Sub dtgInterfaceCode_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtgInterfaceCode.CurrentPageIndex = e.NewPageIndex
        Bindgrid(0)
    End Sub
End Class