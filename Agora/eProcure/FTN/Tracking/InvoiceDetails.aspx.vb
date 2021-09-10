Imports AgoraLegacy
Imports eProcure.Component
Imports System.drawing
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient


Public Class InvoiceDetailsFTN
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim blnPrintRemark As Boolean
    Dim dblInvoiceAmount, prevAppType As String
    Dim aryPTaxCode As New ArrayList()
    Protected WithEvents cmdClear As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdPreviewInvoice1 As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hidPM As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cboPayMethod As System.Web.UI.WebControls.DropDownList

    Dim objGlobal As New AppGlobals
    Dim count As Integer = 0
    Dim strInvAppr As String

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Dim strFrm, strName As String

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Public Enum EnumInvDet
        icLineItem = 0
        icDesc = 1
        icUOM = 2
        icQty = 3
        icUPrice = 4
        icTotal = 5
        icTax = 6
        icGstRate = 7
        icGstAmt = 8
        icTaxCode = 9
        iTerms = 10
        iShipAmt = 11
        icHidGstRate = 12
    End Enum

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        If Request.QueryString("Frm") <> "Dashboard" Then
            CheckButtonAccess(True)
        End If
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        blnCheckBox = False
        blnPaging = False
        blnSorting = False

        MyBase.Page_Load(sender, e)
        SetGridProperty(dtg_invDetail)
        SetGridProperty(dtgAppFlow)

        Dim objCompany As New Companies
        Dim objGst As New GST
        Dim objTrac As New Tracking
        Dim strBComGst, strVComGst As String
        viewstate("line") = 0

        strFrm = Me.Request.QueryString("Frm")
        strName = Me.Request.QueryString("Name")

        If Not IsPostBack Then
            ' Yap: Use the TaxInvoice indicator for checking
            'strBComGst = objGst.chkGST()
            'strVComGst = objGst.chkGST(Request(Trim("vcomid")))

            'If strBComGst <> "" And strVComGst <> "" Then
            '    ViewState("strGSTRegNo") = strVComGst
            'Else
            '    ViewState("strGSTRegNo") = ""
            'End If

            renderFinanceApprFlow()
           
            Dim objval As New InvValue
            Dim objinv As New Invoice

            objval.doc_num = Request(Trim("doc"))
            objval.Inv_no = Request(Trim("INVNO"))
            objval.Vcom_id = Request(Trim("vcomid"))
            objinv.get_invmstr(objval)

            ViewState("InvoiceIndex") = objval.invoiceIndex

            ' Yap: Use the TaxInvoice indicator for checking
            If objTrac.chkGstInvoice(ViewState("InvoiceIndex")) = True Then
                ViewState("strGSTRegNo") = "GST"
            Else
                ViewState("strGSTRegNo") = ""
            End If
            'ViewState("GstInv") = objTrac.chkGstInvoice(objval.invoiceIndex)
            ViewState("vendorId") = Request(Trim("vcomid"))
            ViewState("po_index") = objval.po_index
            ViewState("InvContract") = objTrac.chkInvContract(objval.po_index)
            ViewState("billing_method") = objval.billingMethod
            ViewState("GstInv") = objGst.chkGSTCOD(Format(CDate(objval.create_on), "dd/MM/yyyy"))

            Bindgrid()
            If intPageRecordCnt > 0 Then
                AddRowtotal()
            End If

            Me.lblTel.Text = objval.vphone
            Me.lblEmail.Text = objval.email
            Me.lblInvNo.Text = Request(Trim("INVNO"))
            Me.lblDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, objval.create_on)
            Me.lblYourRef.Text = Replace(objval.your_ref, ",", "<BR>")
            Me.lblOurRef.Text = objval.our_ref
            Me.lblPayTerm.Text = objval.pt
            Me.lblPayMethod.Text = objval.pm
            Me.lblVenRemarks.Text = objval.remark
            Me.lblComName.Text = objval.v_com_name
            Me.lblBillTo.Text = objval.adds
            Me.lblShipType.Text = objval.st
            Me.lblShipMode.Text = objval.sm
            Me.lblAddr.Text = objval.ven_add
            Me.lblBusRegNo.Text = objval.bussiness_reg

            Me.lblVenRemarks.Text = objval.remark
            Me.lblBCoyName.Text = objval.BComName

            GenerateTab()

        End If
   
        Dim strInvAppr As String = objCompany.GetInvApprMode(Session("CompanyId"))

        If strInvAppr <> "Y" Then
            cmdHoldInv.Visible = False
        Else
            cmdHoldInv.Visible = True
        End If

        'cmdPreviewInvoice.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewInvoice.aspx", "vcomid=" & Request(Trim("vcomid")) & "&INVNO=" & Request(Trim("INVNO"))) & "&BCoyID=" & Session("CompanyId") & "')")
        'cmdPreviewInvoice.Attributes.Add("onclick", "PopWindow('../Invoice/PreviewInvoice.aspx?pageid=" & strPageId & "&INVNO=" & Request(Trim("INVNO")) & "&freeze=0&vcomid=" & Request(Trim("vcomid")) & "',  '', 'resizable=no,scrollbars=yes,width=600,height=600,status=no,menubar=no');return false;")
        'cmdPreviewInvoice1.Attributes.Add("onclick", "PopWindow('../Invoice/PreviewInvoice.aspx?pageid=" & strPageId & "&INVNO=" & Request(Trim("INVNO")) & "&freeze=0&vcomid=" & Request(Trim("vcomid")) & "',  '', 'resizable=no,scrollbars=yes,width=600,height=600,status=no,menubar=no')")
        'cmdPreviewInvoice.Attributes.Add("onclick", "PopWindow('../Invoice/PreviewInvoice.aspx?pageid=" & strPageId & "&INVNO=" & Request(Trim("INVNO")) & "&freeze=0&vcomid=" & Request(Trim("vcomid")) & "')")
        '            cmdPreviewDO1.Attributes.Add("onclick", "PopWindow('DOReport.aspx?pageid=" & strPageId & "&DONo=" & strDONo & "&SCoyID=" & strSCoyID & "')")

        If Request.QueryString("folder") = "N" Or Request.QueryString("folder") = "A" Then
            tblApproval.Style.Item("display") = "inline"

            If Request.QueryString("folder") = "N" Then
                cmdAppInv.Text = "Verify" '"Approve Invoice"
                cmdHoldInv.Text = "Hold Invoice"
                cmdRejectInv.Text = "Reject Invoice"
            Else
                cmdAppInv.Text = "Mark As Paid"
                cmdHoldInv.Text = "Hold Payment"
                cmdRejectInv.Text = "Reject Payment"
            End If
        Else
            tblApproval.Style.Item("display") = "none"
        End If

        'Select Case Request.QueryString("folder")
        '    Case "N" ' new
        '    Case "S" ' sent
        '    Case "A" ' approval
        '    Case "P" ' payment
        '    Case "T" ' trash
        '    Case Else
        'End Select
        cmdPreviewInvoice.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewInvoice.aspx", "vcomid=" & Request(Trim("vcomid")) & "&INVNO=" & Request(Trim("INVNO"))) & "&BCoyID=" & Session("CompanyId") & "')")

    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String

        Dim objinv As New Invoice
        Dim ds As DataSet
        Dim dvViewInv As DataView

        Dim strInv As String
        Dim blnAllowInv As Boolean

        ds = objinv.inv_detail(Request(Trim("INVNO")), Request(Trim("vcomid")))
        dvViewInv = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewInv.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" Then dvViewInv.Sort += " DESC"
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            dtg_invDetail.DataSource = dvViewInv
            dtg_invDetail.DataBind()
        Else
            dtg_invDetail.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If
  
        Me.dtg_invDetail.Columns(EnumInvDet.iTerms).Visible = False

        If ViewState("GstInv") = True Then
            Me.dtg_invDetail.Columns(EnumInvDet.icTax).Visible = False
            Me.dtg_invDetail.Columns(EnumInvDet.icGstRate).Visible = True
            Me.dtg_invDetail.Columns(EnumInvDet.icGstAmt).Visible = True
            Me.dtg_invDetail.Columns(EnumInvDet.icTaxCode).Visible = True
        Else
            Me.dtg_invDetail.Columns(EnumInvDet.icTax).Visible = True
            Me.dtg_invDetail.Columns(EnumInvDet.icGstRate).Visible = False
            Me.dtg_invDetail.Columns(EnumInvDet.icGstAmt).Visible = False
            Me.dtg_invDetail.Columns(EnumInvDet.icTaxCode).Visible = False
        End If

        'If Not IsDBNull(ds.Tables(0).Rows(0).Item("IM_PAYMENT_TERM")) Then
        '    Common.SelDdl(Common.parseNull(ds.Tables(0).Rows(0).Item("IM_PAYMENT_TERM")), cboPayMethod, True, True)
        'Else
        '    Common.SelDdl(Common.parseNull(ds.Tables(0).Rows(0).Item("POM_PAYMENT_METHOD")), cboPayMethod, False, True)
        'End If

    End Function

    Private Sub renderFinanceApprFlow()
        Dim objTrac As New Tracking

        Dim ds As DataSet = objTrac.getFinanceApprFlow(Request("INVNO"), Request("vcomid"))
        Dim strSql As String
        Dim objDB As New EAD.DBCom

        objTrac = Nothing

        'Michelle (3/10/2007) - Store the Invoice amt
        strSql = "SELECT IM_INVOICE_TOTAL FROM INVOICE_MSTR WHERE IM_Invoice_No= '" & Request("INVNO") & "'"
        strSql &= " AND IM_S_COY_ID='" & Request("vcomid") & "'"
        Dim tDS As DataSet = objDB.FillDs(strSql)
        If tDS.Tables(0).Rows.Count > 0 Then
            dblInvoiceAmount = tDS.Tables(0).Rows(0).Item("IM_INVOICE_TOTAL")
        Else
            dblInvoiceAmount = 0
        End If

        dtgAppFlow.DataSource = ds.Tables(0).DefaultView
        dtgAppFlow.DataBind()
    End Sub

    Sub AddRowtotal() 'add total row 
        Dim intL, intColToRemain As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim row1 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim row2 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim row3 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)

        For intL = 0 To Me.dtg_invDetail.Columns.Count - 2
            addCell(row)
        Next

        'row.Cells(EnumInvDet.icUPrice - 1).ColumnSpan = 2
        row.Cells(EnumInvDet.icUPrice).Text = "Sub Total"
        row.Cells(EnumInvDet.icUPrice).HorizontalAlign = HorizontalAlign.Right
        row.Cells(EnumInvDet.icUPrice).Font.Bold = True

        row.Cells(EnumInvDet.icTotal).Text = Format(ViewState("total"), "#,##0.00")
        row.Cells(EnumInvDet.icTotal).HorizontalAlign = HorizontalAlign.Right
        row.Cells(EnumInvDet.icTotal).Font.Bold = True

        'row.Cells(6).Text = Format(viewstate("gst"), "#,##0.00")
        'row.Cells(6).HorizontalAlign = HorizontalAlign.Right
        'row.Cells(6).Font.Bold = True

        row.BackColor = Color.FromName("#f4f4f4")
        Me.dtg_invDetail.Controls(0).Controls.Add(row)

        For intL = 0 To Me.dtg_invDetail.Columns.Count - 2
            addCell(row1)
        Next

        'row1.Cells(EnumInvDet.icUPrice - 1).ColumnSpan = 2
        If ViewState("GstInv") = True Then
            row1.Cells(EnumInvDet.icUPrice).Text = "GST Amount"
        Else
            row1.Cells(EnumInvDet.icUPrice).Text = "Tax"
        End If
        row1.Cells(EnumInvDet.icUPrice).HorizontalAlign = HorizontalAlign.Right
        row1.Cells(EnumInvDet.icUPrice).Font.Bold = True

        row1.Cells(EnumInvDet.icTotal).Text = Format(ViewState("gst"), "#,##0.00")
        row1.Cells(EnumInvDet.icTotal).HorizontalAlign = HorizontalAlign.Right
        row1.Cells(EnumInvDet.icTotal).Font.Bold = True

        row1.BackColor = Color.FromName("#f4f4f4")
        Me.dtg_invDetail.Controls(0).Controls.Add(row1)

        For intL = 0 To Me.dtg_invDetail.Columns.Count - 2
            addCell(row2)
        Next

        'row2.Cells(EnumInvDet.icUPrice - 1).ColumnSpan = 2
        row2.Cells(EnumInvDet.icUPrice).Text = "Shipping & Handling"
        row2.Cells(EnumInvDet.icUPrice).HorizontalAlign = HorizontalAlign.Right
        row2.Cells(EnumInvDet.icUPrice).Font.Bold = True

        row2.Cells(EnumInvDet.icTotal).Text = Format(CDbl(ViewState("ShipAmt")), "#,##0.00")
        row2.Cells(EnumInvDet.icTotal).HorizontalAlign = HorizontalAlign.Right
        row2.Cells(EnumInvDet.icTotal).Font.Bold = True

        row2.BackColor = Color.FromName("#f4f4f4")
        Me.dtg_invDetail.Controls(0).Controls.Add(row2)

        For intL = 0 To Me.dtg_invDetail.Columns.Count - 2
            addCell(row3)
        Next

        'row3.Cells(EnumInvDet.icUPrice - 1).ColumnSpan = 2
        row3.Cells(EnumInvDet.icUPrice).Text = "Grand Total"
        row3.Cells(EnumInvDet.icUPrice).HorizontalAlign = HorizontalAlign.Right
        row3.Cells(EnumInvDet.icUPrice).Font.Bold = True

        row3.Cells(EnumInvDet.icTotal).Text = Format(ViewState("total") + ViewState("gst") + ViewState("ShipAmt"), "#,##0.00")
        row3.Cells(EnumInvDet.icTotal).HorizontalAlign = HorizontalAlign.Right
        row3.Cells(EnumInvDet.icTotal).Font.Bold = True

        row3.BackColor = Color.FromName("#f4f4f4")
        Me.dtg_invDetail.Controls(0).Controls.Add(row3)

    End Sub

    Sub addCell(ByRef row As DataGridItem)
        Dim cell As TableCell = New TableCell
        row.Cells.Add(cell)
    End Sub

    Private Sub dtg_invDetail_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_invDetail.ItemCreated
        If e.Item.ItemType = ListItemType.Header Then
            Dim i As Integer
            Dim str As String
            Dim cell As TableCell
            Dim lbl As New Label

            If (Request.QueryString("folder") = "N" Or Request.QueryString("folder") = "A") And ViewState("strGSTRegNo") <> "" And ViewState("GstInv") = True Then
                cell = e.Item.Cells(EnumInvDet.icTaxCode)
                lbl.Text = "<A href='javascript:;' onclick=""window.open('" & dDispatcher.direct("Tracking", "InvTaxCode.aspx", "pageid=" & strPageId & "&id=ddlTaxCode") & "', '', 'scrollbars=yes,resizable=yes,width=500,height=300,status=no,menubar=no');"">" & e.Item.Cells(EnumInvDet.icTaxCode).Text & "</a>"
                cell.Controls.Add(lbl)
            End If
        End If

        dtg_invDetail.AllowSorting = False
        Grid_ItemCreated(dtg_invDetail, e)
    End Sub

    Private Sub dtg_invDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_invDetail.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim dblSubTotal As Double
            Dim objinv As New Invoice
            Dim dsCustom As New DataSet
            Dim dsCustomField As New DataSet
            Dim ds As New DataSet
            Dim rowcnt As Integer
            Dim i, j As Integer
            Dim lstItem As New ListItem
            Dim strPurchaseTC As String
            Dim objTrac As New Tracking

            Dim ddlTaxCode As DropDownList
            Dim lblTaxCode As Label
            ddlTaxCode = e.Item.Cells(EnumInvDet.icTaxCode).FindControl("ddlTaxCode")
            lblTaxCode = e.Item.Cells(EnumInvDet.icTaxCode).FindControl("lblTaxCode")

            If Request.QueryString("folder") = "N" Or Request.QueryString("folder") = "A" Then
                If ViewState("strGSTRegNo") <> "" Then
                    objGlobal.FillTaxCode(ddlTaxCode, Common.parseNull(dv("ID_GST_RATE")), "P", , False)
                    If aryPTaxCode.Count > 0 Then
                        For j = 0 To aryPTaxCode.Count - 1
                            If Common.parseNull(dv("ID_PO_LINE")) = aryPTaxCode(j)(0) Then
                                ddlTaxCode.SelectedValue = aryPTaxCode(j)(1)
                            End If
                        Next
                    Else
                        If Common.parseNull(dv("ID_GST_INPUT_TAX_CODE")) <> "" Then
                            ddlTaxCode.SelectedValue = dv("ID_GST_INPUT_TAX_CODE")
                        Else
                            If ViewState("InvContract") = True Then
                                strPurchaseTC = objTrac.DisplayDefaultTaxCode(ViewState("InvoiceIndex"), dv("ID_PO_LINE"))
                                ddlTaxCode.SelectedValue = strPurchaseTC
                            End If
                        End If
                    End If

                    If ddlTaxCode.SelectedValue = "N/A" Then
                        ddlTaxCode.Enabled = False
                    Else
                        ddlTaxCode.Enabled = True
                    End If
                Else
                    ddlTaxCode.Items.Clear()
                    lstItem.Value = "NR"
                    lstItem.Text = "NR"
                    ddlTaxCode.Items.Insert(0, lstItem)
                    ddlTaxCode.Enabled = False
                End If

                ddlTaxCode.Visible = True
                lblTaxCode.Visible = False
            Else
                ddlTaxCode.Visible = False
                lblTaxCode.Visible = True

                lblTaxCode.Text = Common.parseNull(dv("ID_GST_INPUT_TAX_CODE"))
            End If

            '2015-06-22: CH: Rounding issue (Prod issue)
            'dblSubTotal = Common.parseNull(dv("ID_RECEIVED_QTY"), 0) * Common.parseNull(dv("ID_UNIT_COST"), 0)
            dblSubTotal = CDec(Format(Common.parseNull(dv("ID_RECEIVED_QTY"), 0) * Common.parseNull(dv("ID_UNIT_COST"), 0), "###0.00"))
            e.Item.Cells(EnumInvDet.icTotal).Text = Format(dblSubTotal, "#,##0.00")
            e.Item.Cells(EnumInvDet.icUPrice).Text = Format(dv("ID_UNIT_COST"), "#,##0.0000")
            'ViewState("total") = ViewState("total") + dblSubTotal
            ViewState("total") = ViewState("total") + CDbl(Format(dblSubTotal, "#,##0.00"))
            e.Item.Cells(EnumInvDet.icTax).Text = Format(dblSubTotal / 100 * Common.parseNull(dv("ID_GST")), "#,##0.00")
            e.Item.Cells(EnumInvDet.icGstAmt).Text = Format(dblSubTotal / 100 * Common.parseNull(dv("ID_GST")), "#,##0.00")
            'viewstate("gst") = viewstate("gst") + dblSubTotal / 100 * Common.parseNull(dv("ID_GST"))
            ViewState("gst") = ViewState("gst") + CDbl(Format((dblSubTotal / 100 * Common.parseNull(dv("ID_GST"))), "#,##0.00"))
            ViewState("ShipAmt") = Format(dv("IM_SHIP_AMT"), "#,##0.00")
            e.Item.Cells(EnumInvDet.icLineItem).Text = Common.parseNull(dv("ID_PO_LINE"))


            'e.Item.Cells(EnumInvDet.icUPrice).Text = Format(dv("ID_UNIT_COST"), "#,##0.0000")
            ''ViewState("total") = ViewState("total") + dblSubTotal
            'ViewState("total") = ViewState("total") + CDbl(Format(dblSubTotal, "#,##0.00"))
            'e.Item.Cells(EnumInvDet.icTax).Text = Format(dblSubTotal / 100 * Common.parseNull(dv("ID_GST")), "#,##0.00")
            ''ViewState("gst") = ViewState("gst") + dblSubTotal / 100 * Common.parseNull(dv("ID_GST"))
            'ViewState("gst") = ViewState("gst") + CDbl(Format((dblSubTotal / 100 * Common.parseNull(dv("ID_GST"))), "#,##0.00"))
            'ViewState("ShipAmt") = Format(dv("IM_SHIP_AMT"), "#,##0.00")
            'e.Item.Cells(EnumInvDet.icLineItem).Text = Common.parseNull(dv("ID_PO_LINE"))


        End If
    End Sub

    Private Sub dtgAppFlow_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAppFlow.ItemCreated
        Grid_ItemCreated(sender, e)
    End Sub

    Private Sub dtgAppFlow_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAppFlow.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim intTotalCell, intLoop As Integer
            Dim strRole As String

            Dim objDB As New EAD.DBCom

            Dim objUserRoles As New UserRoles
            Dim UserRole = objUserRoles.get_UserFixedRoleByParameter(HttpContext.Current.Session("CompanyId"), Session("UserId"))

            If InStr(UserRole, "Finance Officer") = 1 Then
                strRole = "FO"
            Else
                strRole = "FM"
            End If

            'If count = 0 Then
            If dv("FA_AGA_TYPE") = strRole Then
                strInvAppr = objDB.GetVal("SELECT CM_INV_APPR FROM COMPANY_MSTR WHERE CM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                If strInvAppr = "N" And IsDBNull(dv("FA_ACTION_DATE")) Then
                    e.Item.Cells(1).Text = objDB.GetVal("SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND UM_USER_ID = '" & Session("UserId") & "'")
                End If
            End If
            '    count = 1
            'End If

            e.Item.Cells(3).Text = "Approval"

            'Michelle (3/10/2007) - To set the Approval Type as 'N/A' for those FOs with approval limit > Invoice amt
            '                       For 'FM', the Approval Type is 'Approval'. 
            If dv("FA_AGA_TYPE") = "FO" And (dblInvoiceAmount < Common.parseNull(dv("AO_LIMIT"), 0)) Then
                e.Item.Cells(3).Text = "N/A"
            End If
            If e.Item.Cells(3).Text = "N/A" And prevAppType = "Approval" Then
                e.Item.Cells(3).Text = "Approval"
                prevAppType = "Already Set"
            End If

            If dv("FA_Seq") - 1 = dv("FA_AO_Action") Then
                intTotalCell = e.Item.Cells.Count - 1

                If Request.QueryString("status") = invStatus.Hold Then
                    e.Item.Cells(3).Text = "Hold"
                End If

                For intLoop = 0 To intTotalCell
                    e.Item.Cells(intLoop).Font.Bold = True
                Next

                ViewState("CurrentAppSeq") = dv("FA_Seq")
                ViewState("ApprType") = dv("FA_APPROVAL_TYPE")

                If dv("FA_AGA_TYPE") = "FO" Then
                    'If UCase(dv("FA_AO")) = UCase(Request.QueryString("AO")) Then
                    If UCase(dv("FA_AO")) = UCase(Session("UserId")) Then
                        ViewState("ApprLimit") = Common.parseNull(dv("AO_LIMIT"), 0)
                    Else
                        If Not IsDBNull(dv("FA_A_AO")) Then
                            ViewState("ApprLimit") = Common.parseNull(dv("AAO_LIMIT"), 0)
                        Else
                            ViewState("ApprLimit") = 0
                        End If
                    End If
                End If

                If Not IsPostBack Then txtRemark.Text = Common.parseNull(dv("FA_AO_REMARK"), "")
            End If

            ViewState("HighestAppr") = dv("FA_Seq")

            If IsDBNull(dv("AAO_NAME")) Then
                e.Item.Cells(2).Text = "-"
            End If

            If Common.parseNull(dv("FA_ACTIVE_AO")) = Common.parseNull(dv("FA_AO")) Then
                e.Item.Cells(1).Font.Bold = True
            ElseIf Common.parseNull(dv("FA_ACTIVE_AO")) = Common.parseNull(dv("FA_A_AO")) Then
                e.Item.Cells(2).Font.Bold = True
            End If

            If Not IsDBNull(dv("FA_ACTION_DATE")) Then
                e.Item.Cells(4).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("FA_ACTION_DATE"))
            Else
                If dv("FA_Seq") <= dv("FA_AO_Action") Then
                    e.Item.Cells(3).Text = "N/A"
                End If
            End If

            If prevAppType <> "Already Set" Then
                prevAppType = e.Item.Cells(3).Text
            End If

        End If
    End Sub
    Private Sub cmdAppInv_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAppInv.Click
        'updating her to save and approve   Srihari
        Dim objTrac As New Tracking
        Dim strMsg As String

        lblMsg.Text = ""
        If chkPurchaseTaxCode(strMsg) = True Then
            SavePurchaseTaxCode()
            objTrac.updateAppRemark(ViewState("InvoiceIndex"), txtRemark.Text, , , aryPTaxCode)

            'end
            Dim blnApproved As Boolean

            If Request.QueryString("folder") = "N" Then
                blnApproved = approval("FO")
            ElseIf Request.QueryString("folder") = "A" Then
                blnApproved = approval("FM")
            End If
            Dim strurl As String = ""

            If blnApproved Then
                'Response.Redirect("TrackConfirm.aspx?pageid=" & strPageId & "&type=S")
                If strFrm = "Dashboard" Then
                    If Request.QueryString("folder") = "N" Then
                        strurl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "Frm=" & "Dashboard" & "&pageid=" & strPageId)
                    Else
                        strurl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "Frm=" & "Dashboard" & "&pageid=" & strPageId)
                    End If
                ElseIf strFrm = "InvoiceTrackingList" Then
                    strurl = dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "Frm=" & "InvoiceTrackingList" & "&pageid=" & strPageId)

                ElseIf strFrm = "InvoiceVerifiedTrackingList" Then
                    strurl = dDispatcher.direct("Tracking", "InvoiceVerifiedTrackingList.aspx", "Frm=" & "InvoiceVerifiedTrackingList" & "&pageid=" & strPageId)
                ElseIf strFrm = "InvoiceVerified" Then
                    strurl = dDispatcher.direct("Tracking", "InvoiceVerified.aspx", "pageid=" & strPageId)
                ElseIf strFrm = "InvoicePaidTrackingList" Then
                    strurl = dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "pageid=" & strPageId)
                End If
                cmdAppInv.Visible = False
                cmdVerify.Visible = False
                cmdRejectInv.Visible = False
                If Request.QueryString("folder") = "N" Then
                    Common.NetMsgbox(Me, "Invoice Number " & Request(Trim("INVNO")) & " has been submitted for approval.", strurl)
                Else
                    Common.NetMsgbox(Me, "Invoice Number " & Request(Trim("INVNO")) & " has been mark as paid.", strurl)
                End If
            End If
        Else
            lblMsg.Text = strMsg
        End If
        

        'Common.NetMsgbox(Me, MsgRecordSave, "InvoiceTracking.aspx?pageid=" & strPageId, MsgBoxStyle.Exclamation)
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        lblMsg.Text = ""

        Dim objTrac As New Tracking
        Dim strMsg As String
        'If strFrm = "InvoiceVerified" Then
        'objTrac.updateAppRemark(ViewState("InvoiceIndex"), txtRemark.Text, , cboPayMethod.SelectedValue)
        'Else
        SavePurchaseTaxCode()
        If chkPurchaseTaxCode(strMsg) = True Then
            objTrac.updateAppRemark(ViewState("InvoiceIndex"), txtRemark.Text, , , aryPTaxCode)
            Common.NetMsgbox(Me, MsgRecordSave, Request.Url.AbsoluteUri, MsgBoxStyle.Information)
        Else
            lblMsg.Text = strMsg
        End If

        'End If
        'ViewState("total") = 0
        'ViewState("gst") = 0
        resetDataGrid()

    End Sub

    Private Function approval(ByVal strRole As String) As Boolean

        Dim objTrac As New Tracking

        Dim blnHighestLevel As Boolean
        'Dim strMsg As String = objTrac.ApproveInvoice(Common.Parse(Me.lblInvNo.Text), Common.Parse(viewstate("vendorId")), strRole, Common.Parse(txtRemark.Text), False, blnHighestLevel) - Michelle (21/12/2007) To prevent double '' when user enters single '

        Dim strMsg As String
        If strInvAppr = "N" Then
            strMsg = objTrac.ApproveInvoice(Common.Parse(Me.lblInvNo.Text), Common.Parse(ViewState("vendorId")), strRole, txtRemark.Text, True, blnHighestLevel, False)
        Else
            strMsg = objTrac.ApproveInvoice(Common.Parse(Me.lblInvNo.Text), Common.Parse(ViewState("vendorId")), strRole, txtRemark.Text, False, blnHighestLevel, False)
        End If

        If strMsg <> "" Then
            Common.NetMsgbox(Me, strMsg)
            Return False
        End If

        If blnHighestLevel Then
            Dim dtItem As New DataTable
            dtItem.Columns.Add("InvNo", Type.GetType("System.String"))
            dtItem.Columns.Add("PoIndex", Type.GetType("System.String"))
            dtItem.Columns.Add("Vendor", Type.GetType("System.String"))
            dtItem.Columns.Add("FinRemark", Type.GetType("System.String"))
            dtItem.Columns.Add("InvStatus", Type.GetType("System.Int32"))
            dtItem.Columns.Add("Submitted", Type.GetType("System.String"))
            dtItem.Columns.Add("AppDate", Type.GetType("System.String"))
            dtItem.Columns.Add("PayTerm", Type.GetType("System.String"))
            dtItem.Columns.Add("BillMethod", Type.GetType("System.String"))

            Dim dtr As DataRow
            Dim blnSend As Boolean

            dtr = dtItem.NewRow
            dtr("InvNo") = Me.lblInvNo.Text

            dtr("PoIndex") = viewstate("po_index")
            dtr("Vendor") = viewstate("vendorId")
            dtr("FinRemark") = txtRemark.Text
            dtr("InvStatus") = 0
            dtr("Submitted") = ""
            dtr("AppDate") = ""
            dtr("PayTerm") = ""
            dtr("BillMethod") = viewstate("billing_method")

            dtr("AppDate") = Date.Today.ToString
            dtr("InvStatus") = invStatus.Paid
            blnSend = False

            dtItem.Rows.Add(dtr)
            objTrac.updateInvoice(dtItem, blnSend)
        End If

        Me.Session.Add("invList", Me.lblInvNo.Text & ",")

        Return True
    End Function


    Private Sub cmdHoldInv_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdHoldInv.Click
        lblMsg.Text = ""
        Dim objTrac As New Tracking
        ' Michelle (CR0010) - Force user to enter Remarks
        '  Dim strMsg As String = objTrac.HoldInvoice(lblInvNo.Text, viewstate("vendorId"), txtRemark.Text)

        ' Common.NetMsgbox(Me, strMsg, Request.Url.AbsoluteUri, MsgBoxStyle.Information)
        SavePurchaseTaxCode()
        If txtRemark.Text = "" Then
            Common.NetMsgbox(Me, "Please enter Remarks.", MsgBoxStyle.Information)
        Else
            cmdAppInv.Visible = False
            cmdVerify.Visible = False
            cmdRejectInv.Visible = False
            Dim strMsg As String = objTrac.HoldInvoice(lblInvNo.Text, ViewState("vendorId"), txtRemark.Text)
            Common.NetMsgbox(Me, strMsg, Request.Url.AbsoluteUri, MsgBoxStyle.Information)
        End If

        resetDataGrid()
    End Sub

    Public Sub cmd_next_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles back.ServerClick

        Dim strurl As String = Session("strurl")
        If strFrm = "Dashboard" Then
            If strName = "Buyer" Then
                strurl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "Frm=" & "Dashboard" & "&pageid=" & strPageId)

            ElseIf strName = "FMnAO" Then
                strurl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "Frm=" & "Dashboard" & "&pageid=" & strPageId)
            End If

        ElseIf strFrm = "InvoiceTrackingList" Then
            strurl = dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "Frm=" & "InvoiceTrackingList" & "&pageid=" & strPageId)

        ElseIf strFrm = "InvoiceVerifiedTrackingList" Then
            strurl = dDispatcher.direct("Tracking", "InvoiceVerifiedTrackingList.aspx", "Frm=" & "InvoiceVerifiedTrackingList" & "&pageid=" & strPageId)

        ElseIf strFrm = "InvoicePaidTrackingList" Then
            strurl = dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "Frm=" & "InvoicePaidTrackingList" & "&pageid=" & strPageId)

        End If

        Session("strurl") = ""
        Session("status_dis") = ""
        Response.Redirect(strurl)

    End Sub

    Private Sub cmdRejectInv_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRejectInv.Click
        lblMsg.Text = ""
        Dim objTrac As New Tracking
        objTrac.RejectInvoice(Me.lblInvNo.Text, ViewState("vendorId"), txtRemark.Text)
        cmdAppInv.Visible = False
        cmdVerify.Visible = False
        cmdRejectInv.Visible = False
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        'If strFrm = "Dashboard" And Request.QueryString("dpage") = "FMnAO" Then
        '    Session("w_InvTracking_tabs") = "<div class=""t_entity""><ul>" & _
        '    "<li><div class=""space""></div></li>" & _
        '                "<li><a class=""t_entity_btn_selected"" href=""InvoiceVerified.aspx?pageid=" & strPageId & """><span>Verified Invoice</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                "<li><a class=""t_entity_btn"" href=""InvoicePaidTrackingList.aspx?pageid=" & strPageId & """><span>Paid Invoice</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                    "</ul><div></div></div>"
        'ElseIf strFrm = "InvoiceTrackingList" Or strFrm = "Dashboard" Then
        '    Session("w_InvTracking_tabs") = "<div class=""t_entity""><ul>" & _
        '    "<li><div class=""space""></div></li>" & _
        '            "<li><a class=""t_entity_btn_selected"" href=""InvoiceTrackingList.aspx?pageid=" & strPageId & "&folder=N" & "&status=1" & """><span>New Invoice</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '            "<li><a class=""t_entity_btn"" href=""InvoiceVerifiedTrackingList.aspx?pageid=" & strPageId & """><span>Verified Invoice</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '            "<li><a class=""t_entity_btn"" href=""InvoicePaidTrackingList.aspx?pageid=" & strPageId & """><span>Paid Invoice</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '            "</ul><div></div></div>"

        'ElseIf strFrm = "InvoiceVerifiedTrackingList" Then
        '    Session("w_InvTracking_tabs") = "<div class=""t_entity""><ul>" & _
        '    "<li><div class=""space""></div></li>" & _
        '            "<li><a class=""t_entity_btn"" href=""InvoiceTrackingList.aspx?pageid=" & strPageId & "&folder=N" & "&status=1" & """><span>New Invoice</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '            "<li><a class=""t_entity_btn_selected"" href=""InvoiceVerifiedTrackingList.aspx?pageid=" & strPageId & """><span>Verified Invoice</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '            "<li><a class=""t_entity_btn"" href=""InvoicePaidTrackingList.aspx?pageid=" & strPageId & """><span>Paid Invoice</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '            "</ul><div></div></div>"

        'ElseIf strFrm = "InvoicePaidTrackingList" Then
        '    If Request.QueryString("role") = "3" Then 'ie FM
        '        Session("w_InvTracking_tabs") = "<div class=""t_entity""><ul>" & _
        '    "<li><div class=""space""></div></li>" & _
        '                    "<li><a class=""t_entity_btn"" href=""InvoiceVerified.aspx?pageid=" & strPageId & """><span>Verified Invoice</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                    "<li><a class=""t_entity_btn_selected"" href=""InvoicePaidTrackingList.aspx?pageid=" & strPageId & """><span>Paid Invoice</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                        "</ul><div></div></div>"
        '    Else
        '        Session("w_InvTracking_tabs") = "<div class=""t_entity""><ul>" & _
        '    "<li><div class=""space""></div></li>" & _
        '                "<li><a class=""t_entity_btn"" href=""InvoiceTrackingList.aspx?pageid=" & strPageId & "&folder=N" & "&status=1" & """><span>New Invoice</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                "<li><a class=""t_entity_btn"" href=""InvoiceVerifiedTrackingList.aspx?pageid=" & strPageId & """><span>Verified Invoice</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                "<li><a class=""t_entity_btn_selected"" href=""InvoicePaidTrackingList.aspx?pageid=" & strPageId & """><span>Paid Invoice</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                "</ul><div></div></div>"
        '    End If
        'ElseIf strFrm = "InvoiceVerified" Then
        '    Session("w_InvTracking_tabs") = "<div class=""t_entity""><ul>" & _
        '    "<li><div class=""space""></div></li>" & _
        '                    "<li><a class=""t_entity_btn_selected"" href=""InvoiceVerified.aspx?pageid=" & strPageId & """><span>Verified Invoice</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                    "<li><a class=""t_entity_btn"" href=""InvoicePaidTrackingList.aspx?pageid=" & strPageId & """><span>Paid Invoice</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                    "</ul><div></div></div>"
        'End If
        If strFrm = "Dashboard" And Request.QueryString("dpage") = "AllDashBoard" Then
            Session("w_InvTracking_tabs") = "<div class=""t_entity""><ul>" & _
            "<li><div class=""space""></div></li>" & _
                        "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Tracking", "InvoiceVerified.aspx", "pageid=" & strPageId) & """><span>Verified Invoice</span></a></li>" & _
                        "<li><div class=""space""></div></li>" & _
                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Invoice</span></a></li>" & _
                        "<li><div class=""space""></div></li>" & _
                            "</ul><div></div></div>"


        ElseIf strFrm = "InvoiceTrackingList" Or strFrm = "Dashboard" Then
            Session("w_InvTracking_tabs") = "<div class=""t_entity""><ul>" & _
            "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId & "&folder=" & "N" & "&status=" & "1") & """><span>New Invoice</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceVerifiedTrackingList.aspx", "pageid=" & strPageId) & """><span>Verified Invoice</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Invoice</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "</ul><div></div></div>"

        ElseIf strFrm = "InvoiceVerifiedTrackingList" Then
            Session("w_InvTracking_tabs") = "<div class=""t_entity""><ul>" & _
            "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId & "&folder=" & "N" & "&status=" & "1") & """><span>New Invoice</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Tracking", "InvoiceVerifiedTrackingList.aspx", "pageid=" & strPageId) & """><span>Verified Invoice</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Invoice</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "</ul><div></div></div>"

        ElseIf strFrm = "InvoicePaidTrackingList" Then
            If Request.QueryString("role") = "3" Then 'ie FM
                Session("w_InvTracking_tabs") = "<div class=""t_entity""><ul>" & _
                "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceVerified.aspx", "pageid=" & strPageId) & """><span>Verified Invoice</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Invoice</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                                "</ul><div></div></div>"
            Else
                Session("w_InvTracking_tabs") = "<div class=""t_entity""><ul>" & _
                "<li><div class=""space""></div></li>" & _
                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId & "&folder=" & "N" & "&status=" & "1") & """><span>New Invoice</span></a></li>" & _
                        "<li><div class=""space""></div></li>" & _
                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceVerifiedTrackingList.aspx", "pageid=" & strPageId) & """><span>Verified Invoice</span></a></li>" & _
                        "<li><div class=""space""></div></li>" & _
                        "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Invoice</span></a></li>" & _
                        "<li><div class=""space""></div></li>" & _
                        "</ul><div></div></div>"
            End If
        ElseIf strFrm = "InvoiceVerified" Then
            Session("w_InvTracking_tabs") = "<div class=""t_entity""><ul>" & _
            "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Tracking", "InvoiceVerified.aspx", "pageid=" & strPageId) & """><span>Verified Invoice</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Invoice</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                            "</ul><div></div></div>"


        End If


    End Sub

    Private Sub cboPayMethod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboPayMethod.SelectedIndexChanged
        Dim aa As String
    End Sub

    Sub resetDataGrid()
        ViewState("total") = 0
        ViewState("gst") = 0
        Bindgrid()
        If intPageRecordCnt > 0 Then
            AddRowtotal()
        End If
    End Sub

    Sub SavePurchaseTaxCode()
        Dim dgitem As DataGridItem
        Dim ddlTaxCode As DropDownList
        For Each dgitem In dtg_invDetail.Items
            ddlTaxCode = dgitem.FindControl("ddlTaxCode")
            aryPTaxCode.Add(New String() {dgitem.Cells(EnumInvDet.icLineItem).Text, ddlTaxCode.SelectedValue})
        Next

    End Sub

    Public Function chkPurchaseTaxCode(ByRef strMsg As String) As Boolean
        chkPurchaseTaxCode = True
        Dim dgitem As DataGridItem
        Dim ddlTaxCode As DropDownList
        Dim objGst As New GST

        'If ViewState("strGSTRegNo") = "" And ViewState("GstInv") = False Then
        '    Return True
        'End If

        If ViewState("GstInv") = True And ViewState("strGSTRegNo") <> "" Then
            strMsg = "<ul type='disc'>"

            For Each dgitem In dtg_invDetail.Items
                ddlTaxCode = dgitem.FindControl("ddlTaxCode")
                If objGst.chkValidTaxCode(dgitem.Cells(EnumInvDet.icHidGstRate).Text, ddlTaxCode.SelectedValue, "P") = False Then
                    strMsg &= "<li>" & dgitem.Cells(EnumInvDet.icLineItem).Text & ". Invalid Purchase Tax Code.<ul type='disc'></ul></li>"
                    chkPurchaseTaxCode = False
                End If
            Next

            strMsg &= "</ul>"
        Else
            Return True
        End If
        

    End Function

    'Protected Sub cmdPreviewInvoice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPreviewInvoice.Click
    '    'PreviewInvoice()
    'End Sub

    'Private Sub PreviewInvoice()
    '    Dim ds As New DataSet
    '    Dim conn As MySqlConnection = Nothing
    '    Dim cmd As MySqlCommand = Nothing
    '    Dim da As MySqlDataAdapter = Nothing
    '    Dim rdr As MySqlDataReader = Nothing
    '    Dim myConnectionString As String
    '    Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
    '    Dim objFile As New FileManagement
    '    Dim strImgSrc As String

    '    strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyId"), System.AppDomain.CurrentDomain.BaseDirectory & "images\")

    '    Try

    '        myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

    '        conn = New MySqlConnection(myConnectionString)
    '        conn.Open()

    '        cmd = New MySqlCommand
    '        With cmd
    '            .Connection = conn
    '            .CommandType = CommandType.Text
    '            .CommandText = "SELECT   INVOICE_DETAILS.ID_INVOICE_NO, INVOICE_DETAILS.ID_S_COY_ID, INVOICE_DETAILS.ID_INVOICE_LINE, " _
    '                        & "INVOICE_DETAILS.ID_PO_LINE, INVOICE_DETAILS.ID_PRODUCT_DESC, INVOICE_DETAILS.ID_B_ITEM_CODE, " _
    '                        & "INVOICE_DETAILS.ID_UOM, INVOICE_DETAILS.ID_GST, INVOICE_DETAILS.ID_RECEIVED_QTY, " _
    '                        & "INVOICE_DETAILS.ID_UNIT_COST, INVOICE_DETAILS.ID_WARRANTY_TERMS, " _
    '                        & "INVOICE_DETAILS.ID_ACCT_INDEX, INVOICE_DETAILS.ID_B_CATEGORY_CODE, " _
    '                        & "INVOICE_DETAILS.ID_B_GL_CODE, INVOICE_MSTR.IM_INVOICE_INDEX, INVOICE_MSTR.IM_INVOICE_NO, " _
    '                        & "INVOICE_MSTR.IM_S_COY_ID, INVOICE_MSTR.IM_S_COY_NAME, INVOICE_MSTR.IM_PO_INDEX, " _
    '                        & "INVOICE_MSTR.IM_B_COY_ID, INVOICE_MSTR.IM_PAYMENT_DATE, INVOICE_MSTR.IM_REMARK, " _
    '                        & "INVOICE_MSTR.IM_CREATED_BY, INVOICE_MSTR.IM_CREATED_ON, INVOICE_MSTR.IM_INVOICE_STATUS, " _
    '                        & "INVOICE_MSTR.IM_PAYMENT_NO, INVOICE_MSTR.IM_YOUR_REF, INVOICE_MSTR.IM_OUR_REF, " _
    '                        & "INVOICE_MSTR.IM_INVOICE_PREFIX, INVOICE_MSTR.IM_SUBMITTEDBY_FO, " _
    '                        & "INVOICE_MSTR.IM_EXCHANGE_RATE, INVOICE_MSTR.IM_FINANCE_REMARKS, INVOICE_MSTR.IM_PRINTED, " _
    '                        & "INVOICE_MSTR.IM_FOLDER, INVOICE_MSTR.IM_FM_APPROVED_DATE, " _
    '                        & "INVOICE_MSTR.IM_DOWNLOADED_DATE, INVOICE_MSTR.IM_EXTERNAL_IND, " _
    '                        & "INVOICE_MSTR.IM_REFERENCE_NO, INVOICE_MSTR.IM_INVOICE_TOTAL, " _
    '                        & "INVOICE_MSTR.IM_PAYMENT_TERM, INVOICE_MSTR.IM_STATUS_CHANGED_BY, " _
    '                        & "INVOICE_MSTR.IM_STATUS_CHANGED_ON, PO_DETAILS.POD_COY_ID, PO_DETAILS.POD_PO_NO, " _
    '                        & "PO_DETAILS.POD_PO_LINE, PO_DETAILS.POD_PRODUCT_CODE, PO_DETAILS.POD_VENDOR_ITEM_CODE, " _
    '                        & "PO_DETAILS.POD_PRODUCT_DESC, PO_DETAILS.POD_UOM, PO_DETAILS.POD_ORDERED_QTY, " _
    '                        & "PO_DETAILS.POD_RECEIVED_QTY, PO_DETAILS.POD_REJECTED_QTY, PO_DETAILS.POD_DELIVERED_QTY, " _
    '                        & "PO_DETAILS.POD_CANCELLED_QTY, PO_DETAILS.POD_MIN_PACK_QTY, PO_DETAILS.POD_MIN_ORDER_QTY, " _
    '                        & "PO_DETAILS.POD_ETD, PO_DETAILS.POD_WARRANTY_TERMS, PO_DETAILS.POD_UNIT_COST, " _
    '                        & "PO_DETAILS.POD_REMARK, PO_DETAILS.POD_GST, PO_DETAILS.POD_PR_INDEX, " _
    '                        & "PO_DETAILS.POD_PR_LINE, PO_DETAILS.POD_ACCT_INDEX, PO_DETAILS.POD_PRODUCT_TYPE, " _
    '                        & "PO_DETAILS.POD_B_ITEM_CODE, PO_DETAILS.POD_SOURCE, PO_DETAILS.POD_D_ADDR_CODE, " _
    '                        & "PO_DETAILS.POD_D_ADDR_LINE1, PO_DETAILS.POD_D_ADDR_LINE2, PO_DETAILS.POD_D_ADDR_LINE3, " _
    '                        & "PO_DETAILS.POD_D_POSTCODE, PO_DETAILS.POD_D_CITY, PO_DETAILS.POD_D_STATE, " _
    '                        & "PO_DETAILS.POD_D_COUNTRY, PO_DETAILS.POD_B_CATEGORY_CODE, PO_DETAILS.POD_B_GL_CODE, " _
    '                        & "PO_MSTR.POM_PO_INDEX, PO_MSTR.POM_PO_NO, PO_MSTR.POM_B_COY_ID, PO_MSTR.POM_BUYER_ID, " _
    '                        & "PO_MSTR.POM_BUYER_NAME, PO_MSTR.POM_BUYER_PHONE, PO_MSTR.POM_BUYER_FAX, " _
    '                        & "PO_MSTR.POM_S_COY_ID, PO_MSTR.POM_S_COY_NAME, PO_MSTR.POM_S_ATTN, " _
    '                        & "PO_MSTR.POM_S_REMARK, PO_MSTR.POM_S_ADDR_LINE1, PO_MSTR.POM_S_ADDR_LINE2, " _
    '                        & "PO_MSTR.POM_S_ADDR_LINE3, PO_MSTR.POM_S_POSTCODE, PO_MSTR.POM_S_CITY, " _
    '                        & "PO_MSTR.POM_S_STATE, PO_MSTR.POM_S_COUNTRY, PO_MSTR.POM_S_PHONE, PO_MSTR.POM_S_FAX, " _
    '                        & "PO_MSTR.POM_S_EMAIL, PO_MSTR.POM_PO_DATE, PO_MSTR.POM_FREIGHT_TERMS, " _
    '                        & "PO_MSTR.POM_PAYMENT_TERM, PO_MSTR.POM_PAYMENT_METHOD, PO_MSTR.POM_SHIPMENT_MODE, " _
    '                        & "PO_MSTR.POM_SHIPMENT_TERM, PO_MSTR.POM_CURRENCY_CODE, PO_MSTR.POM_EXCHANGE_RATE, " _
    '                        & "PO_MSTR.POM_PAYMENT_TERM_CODE, PO_MSTR.POM_SHIP_VIA, PO_MSTR.POM_PO_STATUS, " _
    '                        & "PO_MSTR.POM_STATUS_CHANGED_BY, PO_MSTR.POM_STATUS_CHANGED_ON, " _
    '                        & "PO_MSTR.POM_EXTERNAL_REMARK, PO_MSTR.POM_CREATED_BY, PO_MSTR.POM_PO_COST, " _
    '                        & "PO_MSTR.POM_BILLING_METHOD, PO_MSTR.POM_PO_PREFIX, PO_MSTR.POM_B_ADDR_CODE, " _
    '                        & "PO_MSTR.POM_B_ADDR_LINE1, PO_MSTR.POM_B_ADDR_LINE2, PO_MSTR.POM_B_ADDR_LINE3, " _
    '                        & "PO_MSTR.POM_B_POSTCODE, PO_MSTR.POM_B_CITY, PO_MSTR.POM_B_STATE, " _
    '                        & "PO_MSTR.POM_B_COUNTRY, PO_MSTR.POM_FULFILMENT, PO_MSTR.POM_DEPT_INDEX, " _
    '                        & "PO_MSTR.POM_ACCEPTED_DATE, PO_MSTR.POM_DOWNLOADED_DATE, PO_MSTR.POM_ARCHIVE_IND, " _
    '                        & "PO_MSTR.POM_TERMANDCOND, PO_MSTR.POM_REFERENCE_NO, PO_MSTR.POM_EXTERNAL_IND, " _
    '                        & "COMPANY_MSTR.CM_COY_ID, COMPANY_MSTR.CM_COY_NAME, COMPANY_MSTR.CM_COY_TYPE, " _
    '                        & "COMPANY_MSTR.CM_PARENT_COY_ID, COMPANY_MSTR.CM_ACCT_NO, COMPANY_MSTR.CM_BANK, " _
    '                        & "COMPANY_MSTR.CM_BRANCH, COMPANY_MSTR.CM_ADDR_LINE1, COMPANY_MSTR.CM_ADDR_LINE2, " _
    '                        & "COMPANY_MSTR.CM_ADDR_LINE3, COMPANY_MSTR.CM_POSTCODE, COMPANY_MSTR.CM_CITY, " _
    '                        & "COMPANY_MSTR.CM_STATE, COMPANY_MSTR.CM_COUNTRY, COMPANY_MSTR.CM_PHONE, " _
    '                        & "COMPANY_MSTR.CM_FAX, COMPANY_MSTR.CM_EMAIL, COMPANY_MSTR.CM_COY_LOGO, " _
    '                        & "COMPANY_MSTR.CM_BUSINESS_REG_NO, COMPANY_MSTR.CM_TAX_REG_NO, " _
    '                        & "COMPANY_MSTR.CM_PAYMENT_TERM, COMPANY_MSTR.CM_PAYMENT_METHOD, " _
    '                        & "COMPANY_MSTR.CM_ACTUAL_TERMSANDCONDFILE, COMPANY_MSTR.CM_HUB_TERMSANDCONDFILE, " _
    '                        & "COMPANY_MSTR.CM_PWD_DURATION, COMPANY_MSTR.CM_TAX_CALC_BY, " _
    '                        & "COMPANY_MSTR.CM_CURRENCY_CODE, COMPANY_MSTR.CM_BCM_SET, " _
    '                        & "COMPANY_MSTR.CM_BUDGET_FROM_DATE, COMPANY_MSTR.CM_BUDGET_TO_DATE, " _
    '                        & "COMPANY_MSTR.CM_RFQ_OPTION, COMPANY_MSTR.CM_LICENCE_PACKAGE, " _
    '                        & "COMPANY_MSTR.CM_LICENSE_USERS, COMPANY_MSTR.CM_SUB_START_DT, " _
    '                        & "COMPANY_MSTR.CM_SUB_END_DT, COMPANY_MSTR.CM_LICENSE_PRODUCTS, " _
    '                        & "COMPANY_MSTR.CM_FINDEPT_MODE, COMPANY_MSTR.CM_PRIV_LABELING, " _
    '                        & "COMPANY_MSTR.CM_SKINS_ID, COMPANY_MSTR.CM_TRAINING, COMPANY_MSTR.CM_STATUS, " _
    '                        & "COMPANY_MSTR.CM_DELETED, COMPANY_MSTR.CM_MOD_BY, COMPANY_MSTR.CM_MOD_DT, " _
    '                        & "COMPANY_MSTR.CM_ENT_BY, COMPANY_MSTR.CM_ENT_DT, COMPANY_MSTR.CM_SKU, " _
    '                        & "COMPANY_MSTR.CM_TRANS_NO, COMPANY_MSTR.CM_CONTACT, COMPANY_MSTR.CM_REPORT_USERS, " _
    '                        & "COMPANY_MSTR.CM_INV_APPR, COMPANY_MSTR.CM_MULTI_PO, COMPANY_MSTR.CM_BA_CANCEL, " _
    '                        & "(SELECT   CODE_DESC " _
    '                        & "FROM      CODE_MSTR AS a " _
    '                        & "WHERE   (CODE_ABBR = PO_MSTR.POM_B_STATE) AND (CODE_CATEGORY = 's') AND " _
    '                        & "(CODE_VALUE = PO_MSTR.POM_B_COUNTRY)) AS BillAddrState, " _
    '                        & "(SELECT   CODE_DESC " _
    '                        & "FROM      CODE_MSTR AS a " _
    '                        & "WHERE   (CODE_ABBR = PO_MSTR.POM_B_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS BillAddrCtry, " _
    '                        & "(SELECT   CODE_DESC " _
    '                        & "FROM      CODE_MSTR AS a " _
    '                        & "WHERE   (CODE_ABBR = PO_MSTR.POM_S_STATE) AND (CODE_CATEGORY = 's') AND " _
    '                        & "(CODE_VALUE = PO_MSTR.POM_S_COUNTRY)) AS SupplierAddrState, " _
    '                        & "(SELECT   CODE_DESC " _
    '                        & "FROM      CODE_MSTR AS a " _
    '                        & "WHERE   (CODE_ABBR = PO_MSTR.POM_S_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS SupplierAddrCtry, " _
    '                        & "(SELECT   CM_COY_NAME " _
    '                        & "FROM      COMPANY_MSTR AS b " _
    '                        & "WHERE   (CM_COY_ID = PO_MSTR.POM_B_COY_ID)) AS BuyerCompanyName, " _
    '                        & "PO_MSTR.POM_PRINT_REMARK, PO_MSTR.POM_PRINT_CUSTOM_FIELDS, PO_MSTR.POM_SHIP_AMT, " _
    '                        & "INVOICE_MSTR.IM_SHIP_AMT " _
    '                        & "FROM      INVOICE_DETAILS INNER JOIN " _
    '                        & "INVOICE_MSTR ON INVOICE_DETAILS.ID_INVOICE_NO = INVOICE_MSTR.IM_INVOICE_NO AND " _
    '                        & "INVOICE_DETAILS.ID_S_COY_ID = INVOICE_MSTR.IM_S_COY_ID " _
    '                        & "INNER JOIN PO_DETAILS ON INVOICE_DETAILS.ID_PO_LINE = PO_DETAILS.POD_PO_LINE " _
    '                        & "INNER JOIN PO_MSTR ON PO_DETAILS.POD_PO_NO = PO_MSTR.POM_PO_NO AND " _
    '                        & "PO_DETAILS.POD_COY_ID = PO_MSTR.POM_B_COY_ID AND " _
    '                        & "INVOICE_MSTR.IM_PO_INDEX = PO_MSTR.POM_PO_INDEX AND " _
    '                        & "INVOICE_MSTR.IM_S_COY_ID = PO_MSTR.POM_S_COY_ID " _
    '                        & "INNER JOIN COMPANY_MSTR ON PO_MSTR.POM_S_COY_ID = COMPANY_MSTR.CM_COY_ID " _
    '                        & "WHERE   (INVOICE_MSTR.IM_S_COY_ID = @prmSCoyID) AND (INVOICE_MSTR.IM_B_COY_ID = @prmBCoyID) AND " _
    '                        & "(INVOICE_MSTR.IM_INVOICE_NO = @prmInvNo)"
    '        End With

    '        da = New MySqlDataAdapter(cmd)
    '        da.SelectCommand.Parameters.Add(New MySqlParameter("@prmSCoyID", Request.QueryString("vcomid")))
    '        da.SelectCommand.Parameters.Add(New MySqlParameter("@prmBCoyID", Session("CompanyId")))
    '        da.SelectCommand.Parameters.Add(New MySqlParameter("@prmInvNo", Me.Request.QueryString("INVNO")))

    '        da.Fill(ds)
    '        Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("InvoiceDocument_FTN_DataSetPreviewInvoice", ds.Tables(0))
    '        Dim localreport As New LocalReport
    '        localreport.DataSources.Clear()
    '        localreport.DataSources.Add(rptDataSource)
    '        localreport.ReportPath = appPath & "Invoice\PreviewINVOICE-FTN.rdlc"
    '        localreport.EnableExternalImages = True

    '        Dim I As Byte
    '        Dim GetParameter As String = ""
    '        Dim TotalParameter As Byte
    '        TotalParameter = localreport.GetParameters.Count
    '        Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
    '        'Dim paramlist As New Generic.List(Of ReportParameter)
    '        For I = 0 To localreport.GetParameters.Count - 1
    '            GetParameter = localreport.GetParameters.Item(I).Name
    '            Select Case LCase(GetParameter)
    '                Case "par1"
    '                    par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings("ReportCoyLogoPath") & strImgSrc)
    '                Case Else
    '            End Select
    '        Next
    '        localreport.SetParameters(par)
    '        localreport.Refresh()

    '        'Dim deviceInfo As String = _
    '        '    "<DeviceInfo>" + _
    '        '        "  <OutputFormat>EMF</OutputFormat>" + _
    '        '        "  <PageWidth>8.27in</PageWidth>" + _
    '        '        "  <PageHeight>11in</PageHeight>" + _
    '        '        "  <MarginTop>0.25in</MarginTop>" + _
    '        '        "  <MarginLeft>0.25in</MarginLeft>" + _
    '        '        "  <MarginRight>0.25in</MarginRight>" + _
    '        '        "  <MarginBottom>0.25in</MarginBottom>" + _
    '        '        "</DeviceInfo>"
    '        Dim deviceInfo As String = _
    '                       "<DeviceInfo>" + _
    '                           "  <OutputFormat>EMF</OutputFormat>" + _
    '                           "</DeviceInfo>"
    '        Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)

    '        Dim fs As New FileStream(appPath & "Invoice\InvReport.PDF", FileMode.Create)
    '        fs.Write(PDF, 0, PDF.Length)
    '        fs.Close()

    '        Dim strJScript As String
    '        strJScript = "<script language=javascript>"
    '        strJScript += "window.open('" & dDispatcher.direct("Invoice", "InvReport.PDF") & "',null,'height=600, width=730,status= no, resizable= yes, scrollbars=yes, toolbar=no,location=center,menubar=no, top=10, left=100');"
    '        strJScript += "</script>"
    '        Response.Write(strJScript)

    '    Catch ex As Exception
    '    Finally
    '        cmd = Nothing
    '        If Not IsNothing(rdr) Then
    '            rdr.Close()
    '        End If
    '        If Not IsNothing(conn) Then
    '            If conn.State = ConnectionState.Open Then
    '                conn.Close()
    '            End If
    '        End If
    '        conn = Nothing
    '    End Try
    'End Sub

    Private Sub cmdPreviewInvoice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPreviewInvoice.Click
        resetDataGrid()
    End Sub
End Class
