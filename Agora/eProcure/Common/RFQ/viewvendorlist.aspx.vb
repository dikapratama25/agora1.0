Imports AgoraLegacy
Imports eProcure.Component

Public Class viewvendorlist
    Inherits AgoraLegacy.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_list As System.Web.UI.WebControls.Label
    Protected WithEvents dtg_vendor As System.Web.UI.WebControls.DataGrid
    Protected WithEvents back As System.Web.UI.HtmlControls.HtmlAnchor

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
        CheckButtonAccess(True)
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)
        SetGridProperty(Me.dtg_vendor)

        Bindgrid()
        ' viewstate("urlreferer") = Request.UrlReferrer.ToString
        'lnkBack.NavigateUrl = viewstate("urlreferer") ' "RFQ_List.aspx?pageId=" & strPageId
    End Sub

    Sub dtg_vendor_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtg_vendor.PageIndexChanged
        ' dtg_vendor.CurrentPageIndex = e.NewPageIndex
        ' startindex = e.NewPageIndex * dtg_vendor.PageSize
        dtg_vendor.CurrentPageIndex = e.NewPageIndex
        Bindgrid(0)
    End Sub

    Sub SortCommand_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles dtg_vendor.SortCommand
        Grid_SortCommand(sender, e)
        Bindgrid(dtg_vendor.CurrentPageIndex, True)
    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        'If (Me.TextBox1.Text = "") Then
        '    dtg_vendor.PageSize = 8
        'Else
        '    Me.dtg_vendor.PageSize = TextBox1.Text
        'End If
        Dim objrfq As New RFQ
        Dim list_name As String = Request(Trim("list_name"))
        Me.lbl_list.Text = list_name
        Dim rfq_name As String = Me.Request(Trim("RFQ_name"))
        Dim list_no As String = Me.Request(Trim("list_no"))
        Dim ds As DataSet

        ds = objrfq.GET_VENLISTCOM(list_no)
        'record = ds.Tables(0).Rows.Count
        intPageRecordCnt = ds.Tables(0).Rows.Count
        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        'dvViewSample(0)(0)
        If pSorted Then
            dvViewSample.Sort = viewstate("SortExpression")
            If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"
        End If

        dtg_vendor.DataSource = dvViewSample

        dtg_vendor.DataBind()

        'ShowStats()
    End Function

    Private Sub dtg_vendor_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_vendor.ItemCreated
        Grid_ItemCreated(dtg_vendor, e)

    End Sub

    Private Sub dtg_vendor_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_vendor.ItemDataBound
        Dim country As String
        Dim state As String
        Dim objrfq As New RFQ


        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lbl_adds2 As Label
            lbl_adds2 = e.Item.FindControl("lbl_adds")

            Dim stradds As String
            If (Not IsDBNull(dv("CM_ADDR_LINE1"))) And Common.parseNull(dv("CM_ADDR_LINE1")) <> "" Then
                stradds = "" & Common.parseNull(dv("CM_ADDR_LINE1")) & ""
            End If

            If (Not IsDBNull(dv("CM_ADDR_LINE2"))) And Common.parseNull(dv("CM_ADDR_LINE2")) <> "" Then
                If stradds = "" Then
                    stradds = "" & Common.parseNull(dv("CM_ADDR_LINE2")) & ""
                Else
                    stradds = stradds & "<br>" & Common.parseNull(dv("CM_ADDR_LINE2")) & ""
                End If
            End If

            If (Not IsDBNull(dv("CM_ADDR_LINE3"))) And Common.parseNull(dv("CM_ADDR_LINE3")) <> "" Then
                If stradds = "" Then
                    stradds = "" & Common.parseNull(dv("CM_ADDR_LINE3")) & ""
                Else
                    stradds = stradds & "<br>" & Common.parseNull(dv("CM_ADDR_LINE3")) & ""
                End If
            End If


            If (Not IsDBNull(dv("CM_POSTCODE"))) And Common.parseNull(dv("CM_POSTCODE")) <> "" Or (Not IsDBNull(dv("CM_CITY"))) Or Common.parseNull(dv("CM_CITY")) <> "" Then
                If stradds = "" Then
                    stradds = "" & Common.parseNull(dv("CM_POSTCODE")) & " " & Common.parseNull(dv("CM_CITY"))
                Else
                    stradds = stradds & "<br>" & Common.parseNull(dv("CM_POSTCODE")) & " " & Common.parseNull(dv("CM_CITY"))
                End If
            End If

            state = objrfq.get_codemstr(Common.parseNull(dv("CM_STATE")), "S")

            If state <> "" Then
                If stradds = "" Then
                    stradds = state
                Else
                    stradds = stradds & "<br> " & state
                End If
            End If

            country = objrfq.get_codemstr(Common.parseNull(dv("CM_COUNTRY")), "CT")
            If country <> "" Then
                If stradds = "" Then
                    stradds = country
                Else
                    stradds = stradds & "<br>" & country
                End If
            End If

            If (Not IsDBNull(dv("CM_EMAIL"))) And Common.parseNull(dv("CM_EMAIL")) <> "" Then
                If stradds = "" Then
                    stradds = "" & dv("CM_EMAIL") & ""
                Else
                    stradds = stradds & "<br>" & dv("CM_EMAIL") & ""
                End If
            End If

            If (Not IsDBNull(dv("CM_PHONE"))) And Common.parseNull(dv("CM_PHONE")) <> "" Then
                If stradds = "" Then
                    stradds = "Tel: " & dv("CM_PHONE") & ""
                Else
                    stradds = stradds & "<br>Tel: " & dv("CM_PHONE") & ""
                End If
            End If

            '************************************************************************************
            lbl_adds2.Text = stradds

        End If

    End Sub


    Private Sub back_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles back.ServerClick

        Dim strurl As String = Session("strurl")
        Session("strurl") = ""

        Response.Redirect(strurl)
    End Sub
End Class
