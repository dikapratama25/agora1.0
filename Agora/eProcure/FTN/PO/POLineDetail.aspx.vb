Imports AgoraLegacy
Imports eProcure.Component

Public Class POLineDetailFTN
    Inherits AgoraLegacy.AppBaseClass
    Dim intTotCnt As Integer
    Dim strLargestPOLine, strPrePOLine As String
    Dim strCaller As String
    Dim dDispatcher As New AgoraLegacy.dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents dtg_POList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lbl_Po_No As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_date As System.Web.UI.WebControls.Label
    Protected WithEvents dtg_doc As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dtg_cr As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lbl_do_grn As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_cancel As System.Web.UI.WebControls.Label
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

    Public Enum EnumPOList
        icPOLine = 0
        icItemCode = 1
        icProDesc = 2
        icUOM = 3
        icExtDate = 4
        icMPQ = 5
        icWaranty = 6
        icOrderQty = 7
        icOutstanding = 8
        icRecQty = 9
        icRejQty = 10
        icRemarks = 11
    End Enum

    Public Enum EnumDoc
        icDONo = 0
        icCreateDate = 1
        icSubmitDate = 2
        icCreatedBy = 3
        icGRNNo = 4
        icGRDDate = 5
        icRecDate = 6
        icGRNCreateBy = 7

    End Enum
    Public Enum EnumCR
        icCRNo = 0
        icCreateDate = 1
        icCreatedBy = 2
        icVCRNo = 3

    End Enum
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CheckButtonAccess(True)
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim PO_STATUS1 As String
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtg_POList)
        SetGridProperty(dtg_doc)
        SetGridProperty(dtg_cr)

        strCaller = UCase(Request.QueryString("caller"))

        ViewState("prid") = Request.QueryString("prid")

        ' Session("po_index") = Request("PO_INDEX")
        PO_STATUS1 = Session("PO_STATUS1")

        If Session("filetype") = "" Then
            Session("filetype") = Request(Trim("filetype"))
        Else
            If Request(Trim("filetype")) <> "" Then
                Session("filetype") = Request(Trim("filetype"))
            End If
        End If
        If Session("side") = "" Then
            Session("side") = Request(Trim("side"))
        End If
        If Request(Trim("foce")) = "1" Then
            Session("side") = Request(Trim("side"))
        End If

        If Not IsPostBack Then
            If Session("side") = "b" Or Session("side") = "u" Then
                Me.dtg_POList.Columns(EnumPOList.icExtDate).Visible = False
            ElseIf (Session("side") = "v" Or Session("side") = "ohterv") Then
                Me.dtg_POList.Columns(EnumPOList.icWaranty).Visible = False
                Me.dtg_POList.Columns(EnumPOList.icRejQty).Visible = True
                Me.dtg_POList.Columns(EnumPOList.icExtDate).Visible = True
                Me.dtg_POList.Columns(EnumPOList.icRemarks).Visible = False
            End If
            lbl_Po_No.Text = Request(Trim("po_no"))
            lbl_date.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, Request(Trim("date")))
            Bindgrid(Me.dtg_POList, 1)
            Bindgrid(Me.dtg_doc, 2)
            Bindgrid(Me.dtg_cr, 3)
        End If
    End Sub

    Private Function Bindgrid(ByVal dgid As DataGrid, ByVal turn As String, Optional ByVal pSorted As Boolean = False) As String
        Dim objPO As New PurchaseOrder

        '//Retrieve Data from Database
        Dim ds As DataSet

        'Dim objdb As New EAD.DBCom()
        If turn = "1" Then
            ds = objPO.getlineitem(Me.lbl_Po_No.Text, Session("side"), False, Request(Trim("BCoyID")), False)

        ElseIf turn = "2" Then
            ds = objPO.get_docitem(Me.lbl_Po_No.Text, Session("side"), Request(Trim("BCoyID")))
        ElseIf turn = "3" Then
            ds = objPO.get_CRView(Me.lbl_Po_No.Text, "", Session("side"), Request(Trim("BCoyID")), "")
        End If

        Dim PO_No As String = ""

        Dim dvViewPR As DataView
        dvViewPR = ds.Tables(0).DefaultView

        ViewState("SortExpression") = ViewState("SortExpression_" & dgid.ID)
        If pSorted Then
            dvViewPR.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And Not ViewState("SortExpression") Is Nothing Then dvViewPR.Sort += " DESC"
        End If

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured

        'intPageRecordCnt = ds.Tables(0).Rows.Count
        'intPageRecordCnt = viewstate("intPageRecordCnt")
        If turn = "1" Then
            ViewState("intPageRecordCnt1") = ds.Tables(0).Rows.Count
        ElseIf turn = "2" Then
            ViewState("intPageRecordCnt2") = ds.Tables(0).Rows.Count
        ElseIf turn = "3" Then
            ViewState("intPageRecordCnt3") = ds.Tables(0).Rows.Count
        End If
        'viewstate("intPageRecordCnt") = ds.Tables(0).Rows.Count

        '//bind datagrid
        'If intPageRecordCnt > 0 Then
        If ds.Tables(0).Rows.Count > 0 Then
            If dgid.ID.ToString.Trim = "dtg_POList" Then
                intTotCnt = ds.Tables(0).Rows.Count
                strPrePOLine = "0"
                strLargestPOLine = ds.Tables(0).Rows(intTotCnt - 1).Item("POD_PO_LINE")
            End If
            'intTotPage = dtgDept.PageCount
            dgid.DataSource = dvViewPR
            dgid.DataBind()
            If dgid.ID.ToString.Trim = "dtg_POList" Then
                If Session("side") = "v" Or Session("side") = "otherv" Then
                    dgid.Columns(EnumPOList.icOutstanding).Visible = False
                ElseIf Session("side") = "b" Then
                    dgid.Columns(EnumPOList.icExtDate).Visible = False
                End If
            End If
        Else
            'dtgDept.DataSource = ""
            dgid.DataBind()
            If turn = "2" Then
                Me.lbl_do_grn.Text = ""
            ElseIf turn = "3" Then
                lbl_cancel.Text = ""
            End If
            ' Common.NetMsgbox(Me, "No record found.")
            'intTotPage = 0
        End If
        'If Session("Env") = "FTN" Then
        '    Me.dtg_POList.Columns(6).Visible = False
        'Else
        '    Me.dtg_POList.Columns(6).Visible = True
        'End If
        Me.dtg_POList.Columns(6).Visible = False
    End Function

    Public Sub dtg_POList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        sender.CurrentPageIndex = e.NewPageIndex
        Dim s As DataGrid = sender
        Dim id As String = s.ID.ToString.Trim
        If id = "dtg_POList" Then
            Bindgrid(sender, 1, True)
        ElseIf id = "dtg_doc" Then
            Bindgrid(sender, 2, True)
        ElseIf id = "dtg_cr" Then
            Bindgrid(sender, 3, True)
        End If
        ' Bindgrid(sender, )
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        ' dtg_POList.CurrentPageIndex = e.NewPageIndex
        ViewState("SortExpression_" & sender.ID) = ViewState("SortExpression")
        Dim s As DataGrid = sender
        Dim id As String = s.ID.ToString.Trim
        If id = "dtg_POList" Then
            Bindgrid(sender, 1, True)
        ElseIf id = "dtg_doc" Then
            Bindgrid(sender, 2, True)
        ElseIf id = "dtg_cr" Then
            Bindgrid(sender, 3, True)
        End If
    End Sub

    Private Sub dtg_POList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_POList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim objPO As New PurchaseOrder

            'New Code Added for Line No By Praveen  on 17/07/07 for gird values
            ' e.Item.Cells(0).Text = e.Item.DataSetIndex + 1
            'Michelle (31/7/2007) - To allow sorting of the resequence PO line no
            If strLargestPOLine = dv("POD_PO_LINE") Then 'ie in descending order
                e.Item.Cells(0).Text = intTotCnt
                intTotCnt = intTotCnt - 1
            Else
                If strPrePOLine > dv("POD_PO_LINE") Then
                    e.Item.Cells(0).Text = intTotCnt
                    intTotCnt = intTotCnt - 1
                Else
                    e.Item.Cells(0).Text = e.Item.DataSetIndex + 1
                End If
            End If
            strPrePOLine = dv("POD_PO_LINE")


            '     If viewstate("SortAscending") = "no" Then
            '     e.Item.Cells(0).Text = intTotCnt
            '     intTotCnt = intTotCnt - 1
            'End the code
            '   End If


            '   Dim objdb As New EAD.DBCom
            If Session("side") = "b" Or Session("side") = "u" Then
                Dim cancel_item As Integer
                If IsDBNull(dv("POD_CANCELLED_QTY")) Then
                    cancel_item = 0
                Else
                    cancel_item = CInt(dv("POD_CANCELLED_QTY"))
                End If

                Dim intmax As Integer = CInt(dv("POD_ORDERED_QTY")) - cancel_item - CInt(dv("POD_DELIVERED_QTY"))

                'New Code Added for Line No By Praveen  on 16/07/07 for gird values
                'e.Item.Cells(EnumPOList.icPOLine).Text = Common.parseNull(dv("POD_PO_LINE"))
                'e.Item.Cells(0).Text = e.Item.DataSetIndex + 1
                'End the code

                e.Item.Cells(EnumPOList.icOutstanding).Text = intmax
                'e.Item.Cells(4).Visible = False

                'If IsDBNull(dv("POD_CANCELLED_QTY")) Then
                '    cancel_item = 0
                'Else
                '    cancel_item = CInt(dv("POD_CANCELLED_QTY"))

                'End If
                'Dim intmax As Integer = CInt(dv("POD_ORDERED_QTY")) - cancel_item - CInt(dv("POD_DELIVERED_QTY"))

            ElseIf Session("side") = "v" Or Session("side") = "otherv" Then
                'e.Item.Cells(EnumPOList.icPOLine).Text = "<A href=""POLineListing.aspx?pageid=" & strPageId & "&PO_NO=" & dv("POD_PO_NO") & "&po_line=" & dv("POD_PO_LINE") & "&side=v&BCoyID=" & Request(Trim("BCoyID")) & " "" ><font color=#0000ff>" & dv("POD_PO_LINE") & "</font></A>"

                '-----New Code Added for Line No By Praveen  on 17/07/07 for gird values
                'e.Item.Cells(0).Text = e.Item.DataSetIndex + 1
                'e.Item.Cells(EnumPOList.icPOLine).Text = "<A href=""POLineListing.aspx?pageid=" & strPageId & "&PO_NO=" & dv("POD_PO_NO") & "&po_line=" & dv("POD_PO_LINE") & "&side=v&BCoyID=" & Request(Trim("BCoyID")) & " "" ><font color=#0000ff>" & e.Item.Cells(0).Text & "</font></A>"
                e.Item.Cells(EnumPOList.icPOLine).Text = "<A href=""" & dDispatcher.direct("PO", "POLineListing.aspx.aspx", "pageid=" & strPageId & "&lineval=" & e.Item.Cells(0).Text & "&PO_NO=" & dv("POD_PO_NO") & "&po_line=" & dv("POD_PO_LINE") & "&side=v&BCoyID=" & Request(Trim("BCoyID"))) & " "" ><font color=#0000ff>" & e.Item.Cells(0).Text & "</font></A>"
                '---End The Code 

                If IsDBNull(dv("POD_ETD")) Or Common.parseNull(dv("POD_ETD")) = "0" Then
                    e.Item.Cells(EnumPOList.icExtDate).Text = "Ex-Stock"
                Else
                    Dim edd As Date
                    edd = Common.parseNull(dv("POM_PO_DATE"))
                    e.Item.Cells(EnumPOList.icExtDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, edd.AddDays(Common.parseNull(dv("POD_ETD"))))
                End If
            End If
        End If

    End Sub

    Private Sub dtg_doc_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_doc.ItemDataBound
        Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim side As String
            Dim objpo As New PurchaseOrder
            If Session("side") = "v" Or Session("side") = "otherv" Then
                side = "v"
            Else
                side = "b"
            End If
            e.Item.Cells(EnumDoc.icCreateDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("CREATIONDATE")) 'dv("CREATIONDATE")
            e.Item.Cells(EnumDoc.icSubmitDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("SUBMITIONDATE"))
            'e.Item.Cells(3).Text = objpo

            If IsDBNull(dv("GM_CREATED_DATE")) Then
                e.Item.Cells(EnumDoc.icGRDDate).Text = "-"
            Else
                e.Item.Cells(EnumDoc.icGRDDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("GM_CREATED_DATE"))
            End If

            If IsDBNull(dv("GM_DATE_RECEIVED")) Then
                e.Item.Cells(EnumDoc.icRecDate).Text = "-"
            Else
                e.Item.Cells(EnumDoc.icRecDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("GM_DATE_RECEIVED"))
            End If

            If Common.parseNull(dv("GM_GRN_NO")) <> "" Then
                e.Item.Cells(EnumDoc.icGRNNo).Text = "<A href=""" & dDispatcher.direct("GRN", "GRNDetails.aspx", "pageid=" & strPageId & "&GRNNO=" & Common.parseNull(dv("GM_GRN_NO")) & "&side=" & side & " &BCoyID=" & Common.parseNull(dv("GM_B_COY_ID"))) & """ ><font color=#0000ff>" & _
                                   Common.parseNull(dv("GM_GRN_NO")) & "</font></A>"
            Else
                e.Item.Cells(EnumDoc.icGRNNo).Text = "-"
            End If

            If Common.parseNull(dv("DOM_DO_NO")) <> "" Then

                'If Session("side") = "b" Or Session("side") = "u" Or (Session("side") = "other" And Request.QueryString("caller") = "buyer") Then  'buyer                

                '-----New code adding for side=others by praveen on 25/07/2007 getting side="others" from Podetails.aspx
                'Michelle (CR0050) 
                ' If Session("side") = "b" Or Session("side") = "others" Or Session("side") = "u" Or (Session("side") = "other" And Request.QueryString("caller") = "buyer") Then
                If Session("side") = "b" Or Session("side") = "others" Or Session("side") = "u" Or (Session("side") = "other" And Request.QueryString("caller") = "buyer") Or (Session("side") = "other" And Request.QueryString("caller") = "") Then
                    '-----end the code by praveen   
                    e.Item.Cells(EnumDoc.icDONo).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("DO", "DODetails.aspx", "caller=buyer&poidx=" & Request.QueryString("poidx") & "&pageid=" & strPageId & "&DONO=" & dv("DOM_DO_NO") & "&SCoyID=" & dv("DOM_S_COY_ID")) & "')"" ><font color=#0000ff>" & Common.parseNull(dv("DOM_DO_NO")) & "</font></A>"
                ElseIf Session("side") = "otherv" Or Session("side") = "v" Or (Session("side") = "other" And Request.QueryString("caller") <> "buyer") Then 'vendor               
                    'e.Item.Cells(EnumDoc.icDONo).Text = "<A href=""#"" onclick=""PopWindow('../DO/DOReport.aspx?pageid=" & strPageId & "&DONO=" & dv("DOM_DO_NO") & "&SCoyID=" & dv("DOM_S_COY_ID") & "')"" ><font color=#0000ff>" & Common.parseNull(dv("DOM_DO_NO")) & "</font></A>"
                    e.Item.Cells(EnumDoc.icDONo).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewDO.aspx", "pageid=" & strPageId & "&DONO=" & dv("DOM_DO_NO") & "&SCoyID=" & dv("DOM_S_COY_ID") & "&PO_NO=" & Me.lbl_Po_No.Text) & "')"" ><font color=#0000ff>" & Common.parseNull(dv("DOM_DO_NO")) & "</font></A>"
                End If
            Else
                e.Item.Cells(EnumDoc.icDONo).Text = "-"
            End If

            If e.Item.Cells(EnumDoc.icGRNCreateBy).Text = "&nbsp;" Then
                e.Item.Cells(EnumDoc.icGRNCreateBy).Text = "-"
            End If

            If e.Item.Cells(EnumDoc.icCreatedBy).Text = "&nbsp;" Then
                e.Item.Cells(EnumDoc.icCreatedBy).Text = "-"
            End If

            ' "<A href=""../GRN/GRNDetails.aspx?pageid=" & strPageId & "&GRNNO=" & Common.parseNull(dv("grn_no")) & "&side=v&BCoyID=" & Common.parseNull(dv("POM_B_COY_ID")) & " "" ><font color=#0000ff>" & dv("grn_no") & "</font></A><br>"
        End If
    End Sub

    Private Sub dtg_cr_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_cr.ItemDataBound
        Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            e.Item.Cells(EnumCR.icCRNo).Text = "<A href=""" & dDispatcher.direct("PO", "CR_DETAIL.aspx", "pageid=" & strPageId & "&PO_NO=" & Request(Trim("po_no")) & "&side=" & Session("side") & "&BCoyId=" & Common.parseNull(dv("PCM_B_COY_ID")) & "&cr_no=" & Common.parseNull(dv("PCM_CR_NO"))) & " "" ><font color=#0000ff>" & dv("PCM_CR_NO") & "</font></A>"
            e.Item.Cells(EnumCR.icCreateDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, Common.parseNull(dv("PCM_REQ_DATE")))
        End If
    End Sub

    'meilai 20041228 Back To PODetail.aspx
    'modify by gary 31122004
    Private Sub back_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles back.ServerClick

        '*******************meilai 5/1/2005 add page id****************************
        'Response.Redirect("PODetail.aspx?po_no=" & Request(Trim("po_no")) & " &side=" & Session("side") & "&BCoyID=" & Request(Trim("BCoyID")) & "&filetype=" & Session("filetype") & " ")
        If Request.QueryString("caller") = "buyer" Then  'buyer                
            Response.Redirect(dDispatcher.direct("PO", "PODetail.aspx", "caller=" & strCaller & "&po_no=" & Request(Trim("po_no")) & " &side=" & Session("side") & "&PRNum=" & ViewState("prid") & "&BCoyID=" & Request(Trim("BCoyID")) & "&filetype=" & Session("filetype") & "&pageid=" & strPageId & " "))
        Else
            Response.Redirect(dDispatcher.direct("PO", "PODetail.aspx", "caller=" & strCaller & "&po_no=" & Request(Trim("po_no")) & " &side=" & Session("side") & "&PRNum=" & ViewState("prid") & "&BCoyID=" & Request(Trim("BCoyID")) & "&filetype=" & Session("filetype") & "&pageid=" & strPageId & ""))
        End If
    End Sub

    'meilai 20041228 display total numbers of records in dtg_POList

    'Praveen
    Private Sub dtg_POList_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_POList.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt1")
        Grid_ItemCreated(dtg_POList, e)
    End Sub

    'meilai 20041228 display total numbers of records in dtg_doc
    Private Sub dtg_doc_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_doc.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt2")
        Grid_ItemCreated(dtg_doc, e)
    End Sub

    'meilai 20041228 display total numbers of records in dtg_cr
    Private Sub dtg_cr_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_cr.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt3")
        Grid_ItemCreated(dtg_cr, e)
    End Sub


End Class
