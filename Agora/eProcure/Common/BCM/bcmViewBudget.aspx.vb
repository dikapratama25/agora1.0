Imports AgoraLegacy
Imports eProcure.Component
Imports System.Drawing
Public Class bcmViewBudget
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents lbl_result As System.Web.UI.WebControls.Label
    Protected WithEvents dgDept As System.Web.UI.WebControls.DataGrid
    Protected WithEvents txtSubAcc As System.Web.UI.WebControls.TextBox
    'Protected WithEvents cmdSearchUser As System.Web.UI.WebControls.Button
    Protected WithEvents txtProjCode As System.Web.UI.WebControls.TextBox
    'Protected WithEvents dgView As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents cboLevel As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents txtFind As System.Web.UI.WebControls.TextBox

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
        CheckButtonAccess()
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)
        blnPaging = False
        blnSorting = False
        SetGridProperty(dgView)


    End Sub

    Private Function Bindgrid(ByVal pLevel As Integer, Optional ByVal pSorted As Boolean = False) As String
        Dim strFind As String

        strFind = txtFind.Text


        Dim objBCM As New BudgetControl

        '//Retrieve Data from Database
        Dim ds As DataSet = New DataSet
        ds = objBCM.SearchBudget(strFind, pLevel)

        'Dim dr As DataRow
        'dr = ds.Tables(0).NewRow
        'dr(0) = "NEW"
        'dr(1) = ""
        'dr(2) = "0"
        'ds.Tables(0).Rows.Add(dr)

        '//for sorting asc or desc
        Dim dvViewDept As DataView
        dvViewDept = ds.Tables(0).DefaultView
        'dvViewSample(0)(0)
        If pSorted Then
            dvViewDept.Sort = viewstate("SortExpression")
            If viewstate("SortAscending") = "yes" Then dvViewDept.Sort += " DESC"
        End If

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If viewstate("action") = "del" Then
            If dgView.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dgView.PageSize = 0 Then
                dgView.CurrentPageIndex = dgView.CurrentPageIndex - 1
                viewstate("action") = ""
            End If
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count

        '//bind datagrid

        '//datagrid.pageCount only got value after databind

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dgView, dvViewDept)
            '//mean Enable, can't use button.Enabled because this is a HTML button
            dgView.DataSource = dvViewDept
            dgView.DataBind()

        Else
            'dgView.DataSource = ""
            dgView.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
            'intTotPage = 0
        End If
        ' add for above checking
        viewstate("PageCount") = dgView.PageCount
    End Function



    Private Sub cmdSearchUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearchUser.Click
        viewstate("Dept") = ""
        viewstate("Acc") = ""
        viewstate("Sub") = ""
        Bindgrid(cboLevel.SelectedValue())
    End Sub

    Private Sub dgView_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgView.ItemCreated
        Dim iLevel As Integer
        iLevel = cboLevel.SelectedValue()

        If e.Item.ItemType = ListItemType.Header Then

            dgView.Columns(3).Visible = True
            dgView.Columns(4).Visible = True
            dgView.Columns(5).Visible = True
            dgView.Columns(6).Visible = True
            Select Case iLevel
                Case 0
                    dgView.Columns(3).Visible = False
                    dgView.Columns(4).Visible = False
                    dgView.Columns(5).Visible = False
                    dgView.Columns(6).Visible = False
                Case 1
                    dgView.Columns(4).Visible = False
                    dgView.Columns(5).Visible = False
                Case 2
                    dgView.Columns(5).Visible = False
                Case 3
            End Select
        End If


       
    End Sub

    Private Function GetResBudget(ByVal pDatarow As System.Web.UI.WebControls.DataGridItemEventArgs, ByVal pLevel As String) As Double()
        Dim strSQL As String
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim objDbcom As New EAD.DBCom
        Dim tDS As DataSet
        Dim rtn(4) As Double

        'Michelle (3/5/2010) - To include BCF
        Select Case pLevel
            Case 0
                strSQL = "SELECT SUM(AM_RESERVED_AMT) AS SUM_RESERVED_AMT,SUM(AM_COMMITTED_AMT) AS SUM_COMMITTED_AMT, SUM(AM_UTILIZED_AMT) AS SUM_UTILIZED_AMT, SUM(AM_BCF) AS SUM_BCF_AMT " & _
                         "FROM ACCOUNT_MSTR WHERE AM_DELETED<>'Y' AND AM_DEPT_INDEX = " & pDatarow.Item.Cells(0).Text
                tDS = objDbcom.FillDs(strSQL)
                For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                    rtn(0) += IIf(IsDBNull(tDS.Tables(0).Rows(j).Item("SUM_RESERVED_AMT")), 0, tDS.Tables(0).Rows(j).Item("SUM_RESERVED_AMT"))
                    rtn(1) += IIf(IsDBNull(tDS.Tables(0).Rows(j).Item("SUM_COMMITTED_AMT")), 0, tDS.Tables(0).Rows(j).Item("SUM_COMMITTED_AMT"))
                    rtn(2) += IIf(IsDBNull(tDS.Tables(0).Rows(j).Item("SUM_UTILIZED_AMT")), 0, tDS.Tables(0).Rows(j).Item("SUM_UTILIZED_AMT"))
                    rtn(3) += IIf(IsDBNull(tDS.Tables(0).Rows(j).Item("SUM_BCF_AMT")), 0, tDS.Tables(0).Rows(j).Item("SUM_BCF_AMT"))
                    rtn(4) = rtn(0) + rtn(1) + rtn(2)
                Next

            Case 1, 2, 3
                Dim dblRes, dblCom, dblUti As Double
                Dim sParent As String

                sParent = "'" & pDatarow.Item.Cells(1).Text & "'"

                strSQL = "SELECT SUM(AM_RESERVED_AMT) AS SUM_RESERVED_AMT,SUM(AM_COMMITTED_AMT) AS SUM_COMMITTED_AMT, SUM(AM_UTILIZED_AMT) AS SUM_UTILIZED_AMT, SUM(AM_BCF) AS SUM_BCF_AMT " & _
                         "FROM ACCOUNT_MSTR WHERE AM_DELETED<>'Y' AND AM_LEVEL=" & pLevel & " ANd AM_DEPT_INDEX = " & pDatarow.Item.Cells(0).Text & " AND AM_ACCT_INDEX IN (" & sParent & ")"

                tDS = objDbcom.FillDs(strSQL)
                For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                    rtn(0) += IIf(IsDBNull(tDS.Tables(0).Rows(j).Item("SUM_RESERVED_AMT")), 0, tDS.Tables(0).Rows(j).Item("SUM_RESERVED_AMT"))
                    rtn(1) += IIf(IsDBNull(tDS.Tables(0).Rows(j).Item("SUM_COMMITTED_AMT")), 0, tDS.Tables(0).Rows(j).Item("SUM_COMMITTED_AMT"))
                    rtn(2) += IIf(IsDBNull(tDS.Tables(0).Rows(j).Item("SUM_UTILIZED_AMT")), 0, tDS.Tables(0).Rows(j).Item("SUM_UTILIZED_AMT"))
                    rtn(3) += IIf(IsDBNull(tDS.Tables(0).Rows(j).Item("SUM_BCF_AMT")), 0, tDS.Tables(0).Rows(j).Item("SUM_BCF_AMT"))
                    rtn(4) = rtn(0) + rtn(1) + rtn(2)
                Next

                For i = pLevel + 1 To 3
                    strSQL = "SELECT AM_ACCT_INDEX,SUM(AM_RESERVED_AMT) AS SUM_RESERVED_AMT,SUM(AM_COMMITTED_AMT) AS SUM_COMMITTED_AMT, SUM(AM_UTILIZED_AMT) AS SUM_UTILIZED_AMT, SUM(AM_BCF) AS SUM_BCF_AMT " & _
                             "FROM ACCOUNT_MSTR WHERE AM_DELETED<>'Y' AND AM_LEVEL=" & i & " ANd AM_DEPT_INDEX = " & pDatarow.Item.Cells(0).Text & " AND AM_PARENT_ACCT_INDEX IN (" & sParent & ") GROUP BY AM_ACCT_INDEX"

                    tDS = objDbcom.FillDs(strSQL)
                    sParent = ""
                    For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                        sParent &= IIf(IsDBNull(tDS.Tables(0).Rows(j).Item("SUM_RESERVED_AMT")), 0, "'" & tDS.Tables(0).Rows(j).Item("AM_ACCT_INDEX") & "',")
                        If i >= pLevel Then
                            rtn(0) += IIf(IsDBNull(tDS.Tables(0).Rows(j).Item("SUM_RESERVED_AMT")), 0, tDS.Tables(0).Rows(j).Item("SUM_RESERVED_AMT"))
                            rtn(1) += IIf(IsDBNull(tDS.Tables(0).Rows(j).Item("SUM_COMMITTED_AMT")), 0, tDS.Tables(0).Rows(j).Item("SUM_COMMITTED_AMT"))
                            rtn(2) += IIf(IsDBNull(tDS.Tables(0).Rows(j).Item("SUM_UTILIZED_AMT")), 0, tDS.Tables(0).Rows(j).Item("SUM_UTILIZED_AMT"))
                            rtn(3) += IIf(IsDBNull(tDS.Tables(0).Rows(j).Item("SUM_BCF_AMT")), 0, tDS.Tables(0).Rows(j).Item("SUM_BCF_AMT"))
                            rtn(4) = rtn(0) + rtn(1) + rtn(2)
                        End If
                    Next

                    If sParent <> "" Then
                        sParent = Left(sParent, Len(sParent) - 1)
                    Else
                        sParent = "' '"
                    End If
                Next
        End Select
        Return rtn

    End Function

    Private Sub dgView_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgView.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            If e.Item.Cells(0).Text = viewstate("Dept") Then
                'Erase the same dept name
                e.Item.Cells(2).Text = ""
                'Chk for the acc code
                If e.Item.Cells(3).Text = viewstate("Acc") Then
                    'Erase the same acc code
                    e.Item.Cells(3).Text = ""
                    'Chk for the sub acc code
                    If e.Item.Cells(4).Text = viewstate("Sub") Then
                        'Erase the same sub acc code
                        e.Item.Cells(4).Text = ""
                    Else
                        viewstate("Sub") = e.Item.Cells(4).Text
                    End If
                Else
                    viewstate("Acc") = e.Item.Cells(3).Text
                    viewstate("Sub") = e.Item.Cells(4).Text
                End If
            Else
                viewstate("Dept") = e.Item.Cells(0).Text
                viewstate("Acc") = e.Item.Cells(3).Text
                viewstate("Sub") = e.Item.Cells(4).Text
            End If


            Dim iLevel As Integer
            Dim IniBud As Double
            iLevel = cboLevel.SelectedValue()
            Dim iBudget() As Double = GetResBudget(e, iLevel)

            CType(e.Item.FindControl("lblResBudget"), Label).Text = Format(iBudget(0), "##,##0.00")
            CType(e.Item.FindControl("lblComBudget"), Label).Text = Format(iBudget(1), "##,##0.00")
            CType(e.Item.FindControl("lblUtiBudget"), Label).Text = Format(iBudget(2), "##,##0.00")
            CType(e.Item.FindControl("lblBCF"), Label).Text = Format(iBudget(3), "##,##0.00")
            IniBud = CDbl(e.Item.Cells(8).Text) + iBudget(3) - iBudget(4)
            If IniBud < 0 Then
                e.Item.Cells(10).ForeColor = Color.Red
            End If
            CType(e.Item.FindControl("lblOpBudget"), Label).Text = Format(IniBud, "##,##0.00")

        End If
    End Sub


End Class
