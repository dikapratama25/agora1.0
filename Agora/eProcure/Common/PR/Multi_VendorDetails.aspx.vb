Imports AgoraLegacy
Imports eProcure.Component
Public Class Multi_VendorDetails
    Inherits AgoraLegacy.AppBaseClass
    Public l As Integer
    Public i As Integer
    Public j As Integer
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents LblPrNum As System.Web.UI.WebControls.Label
    Protected WithEvents dtgrid As System.Web.UI.WebControls.DataGrid
    Protected WithEvents back As System.Web.UI.HtmlControls.HtmlAnchor
    Public ds As New DataSet
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents DynamicControls As System.Web.UI.WebControls.PlaceHolder

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim strCaller As String
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        SetGridProperty(Me.dtgrid)
        MyBase.Page_Load(sender, e)
        strCaller = UCase(Request.QueryString("caller"))
        Me.lblTitle.Text = "Vendors Details"
        Bindgrid()
    End Sub
    Private Sub Bindgrid(Optional ByVal pPage As Integer = -1)
        Dim objMulti As New MultiVendor
        Dim objval As New Mulitple
        Dim state As String
        Dim country As String
        Dim valite_ven As String
        Dim objDb As New EAD.DBCom
        objMulti.PRNum = Request(Trim("PRNum"))
        LblPrNum.Text = objMulti.PRNum
        Dim strsql As String

        Dim vid As String
        Dim i As Integer
        strsql = "select CM_COY_ID, CM_COY_NAME,CM_ADDR_LINE1,CM_ADDR_LINE2,CM_ADDR_LINE3," _
                 & " CM_CITY,CM_STATE,CM_COUNTRY,CM_PHONE,CM_EMAIL,CM_POSTCODE " _
                 & "  from company_mstr where cm_coy_id in  (select distinct(prd_s_coy_id) " _
                 & " from pr_details where PRD_PR_NO='" & LblPrNum.Text & "'" _
                 & " and  prd_coy_id='" & Session("CompanyID") & "')"

        ds = objDb.FillDs(strsql)
        intPageRecordCnt = ds.Tables(0).Rows.Count
        dtgrid.DataSource = ds
        dtgrid.DataBind()

    End Sub
    Private Sub dtgrid_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgrid.ItemDataBound
        Dim country As String
        Dim state As String
        Dim objrfq As New RFQ
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lbl_adds2 As Label
            lbl_adds2 = e.Item.FindControl("lbl_adds")
            Dim stradds As String

            e.Item.Cells(0).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "frm=Comlist&side=vendorsum&pageid=" & strPageId & " &RFQ_Num=" & ViewState("rfq_num") & "&RFQ_ID=" & Request(Trim("RFQ_ID")) & "&RFQ_Name=" & Request(Trim("RFQ_Name")) & "&v_com_id=" & Common.parseNull(dv("CM_COY_ID"))) & "')"" ><font color=#0000ff>" & Common.parseNull(dv("CM_COY_NAME")) & "</font></A>"

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
            'If (Not IsDBNull(dv("CM_STATE"))) And Common.parseNull(dv("CM_STATE")) <> "" Or (Not IsDBNull(dv("CM_STATE"))) Or Common.parseNull(dv("CM_STATE")) <> "" Then
            '    If stradds = "" Then
            '        stradds = "" & Common.parseNull(dv("CM_STATE")) & " " & Common.parseNull(dv("CM_STATE"))
            '    Else
            '        stradds = stradds & "<br>" & Common.parseNull(dv("CM_STATE")) '& " " & Common.parseNull(dv("POM_S_STATE"))
            '    End If
            'End If
            'If (Not IsDBNull(dv("CM_COUNTRY"))) And Common.parseNull(dv("CM_COUNTRY")) <> "" Or (Not IsDBNull(dv("CM_COUNTRY"))) Or Common.parseNull(dv("CM_COUNTRY")) <> "" Then
            '    If stradds = "" Then
            '        stradds = "" & Common.parseNull(dv("CM_COUNTRY")) & " " & Common.parseNull(dv("CM_COUNTRY"))
            '    Else
            '        stradds = stradds & "<br>" & Common.parseNull(dv("CM_COUNTRY")) '& " " & Common.parseNull(dv("POM_S_COUNTRY"))
            '    End If
            'End If

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
            lbl_adds2.Text = stradds
        End If
    End Sub
    Sub dtgrid_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgrid.PageIndexChanged
        dtgrid.CurrentPageIndex = e.NewPageIndex
        Bindgrid(0)
    End Sub
    Private Sub dtgrid_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgrid.ItemCreated
        Grid_ItemCreated(dtgrid, e)
    End Sub
    Public Sub cmd_next_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles back.ServerClick
        '------New Chages to linkback path by praveen on 22.08.2007
        Dim strurl As String = Session("strurl")
        Session("strurl") = ""
        Session("status_dis") = ""
        Response.Redirect(strurl)
        '----End the Changes
    End Sub
End Class
