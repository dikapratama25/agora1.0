Imports AgoraLegacy
Imports eProcure.Component

Public Class CR_DETAIL
    Inherits AgoraLegacy.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents dtg_POList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents back As System.Web.UI.HtmlControls.HtmlAnchor
    Protected WithEvents lblCRNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblCRRemarks As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Public Enum EnumCRDet
        icPOLine = 0
        '-------New Column Added  to Grid "dtg_POList" By Praveen on 18/07/2007
        icLineNo = 1
        '-------End
        icProDesc = 2 'Original IcProdesc=1 
        icUOM = 3
        icMPQ = 4
        icWaranty = 5
        icOrderQty = 6
        icRecQty = 7
        icRejQty = 8
        icOutstanding = 9
        icQtyCanl = 10
        icRemarks = 11
    End Enum
    Dim dDispatcher As New AgoraLegacy.dispatcher

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CheckButtonAccess(True)
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)

        If Not IsPostBack Then
            blnPaging = False
            blnSorting = False
            SetGridProperty(dtg_POList)
            Dim check As Boolean
            Dim objPO As New PurchaseOrder
            Bindgrid(Me.dtg_POList)
            GenerateTab()

        End If
        intPageRecordCnt = viewstate("intPageRecordCnt")


    End Sub

    Private Function Bindgrid(ByVal dgid As DataGrid, Optional ByVal pSorted As Boolean = False) As String


        Dim objPO As New PurchaseOrder

        '//Retrieve Data from Database
        Dim ds As DataSet
        'Dim objdb As New EAD.DBCom()
        Dim b_com_id As String = Request(Trim("BCoyID"))
        Dim side As String = Request(Trim("side"))
        Dim cr_no As String = Request(Trim("cr_no"))
        ds = objPO.get_cancelLineitem(Request(Trim("po_no")), b_com_id, side, cr_no, False)



        Dim PO_No As String = ""

        Dim dvViewPR As DataView
        dvViewPR = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewPR.Sort = viewstate("SortExpression")
            If viewstate("SortAscending") = "no" Then dvViewPR.Sort += " DESC"
        End If

        viewstate("intPageRecordCnt") = ds.Tables(0).Rows.Count

        '//bind datagrid
        If viewstate("intPageRecordCnt") > 0 Then
            'intTotPage = dtgDept.PageCount
            lblCRNo.Text = ds.Tables(0).Rows(0)("PCM_CR_NO")
            lblCRRemarks.Text = Common.parseNull(ds.Tables(0).Rows(0)("PCM_CR_REMARKS"))
            dgid.DataSource = dvViewPR
            dgid.DataBind()
        Else
            'dtgDept.DataSource = ""
            dgid.DataBind()
            ' Common.NetMsgbox(Me, "No record found.")
            'intTotPage = 0
        End If
        'If Session("Env") = "FTN" Then
        '    Me.dtg_POList.Columns(5).Visible = False
        'Else
        '    Me.dtg_POList.Columns(5).Visible = True
        'End If
        Me.dtg_POList.Columns(5).Visible = True
    End Function

    Public Sub dtg_POList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        sender.CurrentPageIndex = e.NewPageIndex
        Dim s As String = sender.ToString.Trim
        If s = "dtg_POList" Then
            Bindgrid(sender, True)
        ElseIf s = "dtg_doc" Then
            Bindgrid(sender, True)
        End If
        ' Bindgrid(sender, )
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtg_POList.CurrentPageIndex = 0

        Bindgrid(sender, True)

    End Sub

    Private Sub dtgCustomField_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_POList.ItemCreated
        '//this line must be included
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(dtg_POList, e)
        '//to add a JavaScript to CheckAll button
        'If e.Item.ItemType = ListItemType.Header Then
        '    Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
        '    chkAll.Attributes.Add("onclick", "selectAll();")
        'End If
    End Sub

    Private Sub dtg_POList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_POList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            '-------New code Adding to Get The LineNo in The Grid "dtg_POList" by Praveen on 18/07/2007
            e.Item.Cells(1).Text = e.Item.DataSetIndex + 1
            '-------End

            '//to add JavaScript to Check Box
            Dim cancel_item As Integer
            If IsDBNull(dv("POD_CANCELLED_QTY")) Then
                cancel_item = 0
            Else
                cancel_item = CInt(dv("POD_CANCELLED_QTY"))

            End If
            Dim intmax As Integer = CInt(dv("POD_ORDERED_QTY")) - cancel_item - CInt(dv("POD_DELIVERED_QTY"))

            e.Item.Cells(8).Text = intmax

        End If
    End Sub

    Private Sub back_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles back.ServerClick
        Dim url As String = Session("backtodetail")
        Session("backtodetail") = ""
        Response.Redirect(url)
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        Select Case Session("side")
            Case "v"
                '    Session("w_CRDetail_tabs") = "<div class=""t_entity""><ul>" & _
                '"<li><div class=""space""></div></li>" & _
                '         "<li><a class=""t_entity_btn"" href=""POViewB.aspx?pageid=" & strPageId & """><span>Purchase Order</span></a></li>" & _
                '                     "<li><div class=""space""></div></li>" & _
                '         "<li><a class=""t_entity_btn_selected"" href=""POVendorList.aspx?pageid=" & strPageId & """><span>PO Listing</span></a></li>" & _
                '                     "<li><div class=""space""></div></li>" & _
                '         "</ul><div></div></div>"
                Session("w_CRDetail_tabs") = "<div class=""t_entity""><ul>" & _
                "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PO", "POViewB.aspx", "pageid=" & strPageId) & """><span>Purchase Order</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PO", "POVendorList.aspx", "pageid=" & strPageId) & """><span>PO Listing</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "</ul><div></div></div>"

            Case Else
                If Request.QueryString("Frm") = "InvList" Then
                    '        Session("w_CRDetail_tabs") = "<div class=""t_entity""><ul>" & _
                    '"<li><div class=""space""></div></li>" & _
                    '                                    "<li><a class=""t_entity_btn_selected"" href=""../Invoice/InvList.aspx?pageid=" & strPageId & """><span>Issue Invoice</span></a></li>" & _
                    '                     "<li><div class=""space""></div></li>" & _
                    '                                    "<li><a class=""t_entity_btn"" href=""../Invoice/invoiceView.aspx?pageid=" & strPageId & """><span>Invoice Listing</span></a></li>" & _
                    '                     "<li><div class=""space""></div></li>" & _
                    '                                    "</ul><div></div></div>"
                    Session("w_CRDetail_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                                                "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Invoice", "InvList.aspx", "pageid=" & strPageId) & """><span>Issue Invoice</span></a></li>" & _
                                                "<li><div class=""space""></div></li>" & _
                                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Invoice", "invoiceView.aspx", "pageid=" & strPageId) & """><span>Invoice Listing</span></a></li>" & _
                                                "<li><div class=""space""></div></li>" & _
                                                "</ul><div></div></div>"
                End If
        End Select
    End Sub
End Class