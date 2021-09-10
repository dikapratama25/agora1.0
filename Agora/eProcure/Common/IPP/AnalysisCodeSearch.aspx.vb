Imports eProcure.Component
Imports AgoraLegacy

Partial Public Class AnalysisCodeSearch
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim strAnalysisCode As String = ""
    Dim strAnalysisDesc As String = ""
    Dim strAnalysisStatus As String = "O"
    Dim strDeptCode As String = ""
    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objAnalysisCode As New IPP

        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgAnalysisCode)
        If Me.Request.QueryString("id") <> "" Or Me.Request.QueryString("id") <> Nothing Then
            If InStr(Me.Request.QueryString("id").ToString, ":") Then
                strAnalysisCode = Me.Request.QueryString("id").ToString.Substring(0, InStr(Me.Request.QueryString("id").ToString, ":") - 1)
            Else
                strAnalysisCode = Common.Parse(Me.Request.QueryString("id"))
            End If
        End If

        hidopenerID.Value = Me.Request.QueryString("txtid")
        hidopenerHIDID.Value = Me.Request.QueryString("hidid")
        hidopenerbtn.Value = Me.Request.QueryString("hidbtnid")
        hidopenerValID.Value = Me.Request.QueryString("hidvalid")

        Dim intLength As Integer = 0
        strDeptCode = hidopenerID.Value.ToString.Substring(0, InStr(hidopenerID.Value.ToString, "_") - 1)
        intLength = Len(strDeptCode)
        If intLength > 0 Then
            strDeptCode = strDeptCode.Substring(15, intLength - 15)
            strDeptCode = "L" & strDeptCode
        End If

        Select Case strDeptCode
            Case "L1"
                lblScreenName.Text = "Select Fund Type"
            Case "L2"
                lblScreenName.Text = "Select Product"
            Case "L3"
                lblScreenName.Text = "Select Channel"
            Case "L4"
                lblScreenName.Text = "Select Reinsurance Company"
            Case "L5"
                lblScreenName.Text = "Select Asset Fund"
            Case "L8"
                lblScreenName.Text = "Select Project Code"
            Case "L9"
                lblScreenName.Text = "Select Person Code"
        End Select

        If Not IsPostBack Then
            cmdClose.Attributes.Add("onclick", "selectOne();")
            Me.txtAnalysisCode.Text = strAnalysisCode
            objAnalysisCode.GetAnalysisCode(strAnalysisCode, strAnalysisDesc, strDeptCode, strAnalysisStatus)
            Bindgrid()
            dtgAnalysisCode.CurrentPageIndex = 0
            cmdClose.Attributes.Add("onclick", "selectOne();")
        End If
        objAnalysisCode = Nothing
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objAnalysisCode As New IPP
        Dim ds As New DataSet

        strAnalysisCode = Me.txtAnalysisCode.Text
        strAnalysisDesc = Me.txtAnalysisCodeDesc.Text

        ds = objAnalysisCode.GetAnalysisCode(strAnalysisCode, strAnalysisDesc, strDeptCode, strAnalysisStatus)


        Dim dgItem As DataGridItem
        Dim i As Integer = 1

        intPageRecordCnt = ds.Tables(0).Rows.Count

        ViewState("RecordCount") = intPageRecordCnt
        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewSample.Sort += " DESC"

        If intPageRecordCnt > 0 Then
            NoRecord.Style("display") = "none"
            AnalysisCode.Style("display") = "inline"
            resetDatagridPageIndex(dtgAnalysisCode, dvViewSample)
            dtgAnalysisCode.DataSource = dvViewSample
            dtgAnalysisCode.DataBind()
        Else
            dtgAnalysisCode.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
            If Not Page.IsPostBack Then
                NoRecord.Style("display") = "inline"
            End If
            dtgAnalysisCode.DataBind()

        End If
        ' add for above checking
        ViewState("PageCount") = dtgAnalysisCode.PageCount

        objAnalysisCode = Nothing
        ds = Nothing
    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dtgAnalysisCode.SortCommand
        Grid_SortCommand(sender, e)
        dtgAnalysisCode.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub dtgAnalysisCode_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAnalysisCode.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dtgAnalysisCode, e)
        intPageRecordCnt = ViewState("RecordCount")
    End Sub

    Private Sub dtgAnalysisCode_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAnalysisCode.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim rb As RadioButton = e.Item.FindControl("rbtnSelection")
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            If strAnalysisCode <> "" And strAnalysisCode = dv("AC_ANALYSIS_CODE") Then
                rb.Checked = True
                hidAnalysisCode.Value = strAnalysisCode & ":" & dv("AC_ANALYSIS_CODE_DESC").ToString.Replace("'", "")
            End If

            rb.Attributes.Add("OnClick", "SelectOneOnly(" & rb.ClientID & ", " & "'dtgAnalysisCode'" & ",'" & dv("AC_ANALYSIS_CODE").ToString.Replace("'", "\'") & "','" & dv("AC_ANALYSIS_CODE_DESC").ToString.Replace("'", "\'") & "')")
        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgAnalysisCode.CurrentPageIndex = 0
        ViewState("SortAscending") = "yes"
        ViewState("SortExpression") = "AC_ANALYSIS_CODE"
        strAnalysisCode = Me.txtAnalysisCode.Text
        strAnalysisDesc = Me.txtAnalysisCodeDesc.Text
        Bindgrid()
    End Sub



    Private Sub dtgAnalysisCode_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgAnalysisCode.PageIndexChanged
        Dim objAnalysisCode As New IPP

        dtgAnalysisCode.CurrentPageIndex = e.NewPageIndex
        objAnalysisCode.GetAnalysisCode(strAnalysisCode, strAnalysisDesc, strDeptCode, strAnalysisStatus)
        Bindgrid()
        Session("action") = ""
        objAnalysisCode = Nothing
    End Sub

    Private Sub cmdClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Response.Write("<script language=""javascript"">window.close();</script>")
    End Sub
End Class