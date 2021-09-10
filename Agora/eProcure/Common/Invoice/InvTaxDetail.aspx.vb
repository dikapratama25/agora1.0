Imports AgoraLegacy
Imports eProcure.Component


Public Class InvTaxDetail
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim blnPrintRemark As Boolean
    Dim strMsg As String = ""
    Dim arySTaxCode As New ArrayList()
    Dim blnPrintCustField As Boolean
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtRemark As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRef As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_st As System.Web.UI.WebControls.Label
    Protected WithEvents dtg_invDetail As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lbl_InvNum As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_req As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_date As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_contect As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_gst As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_cur_code As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_attention As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_YourRef As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Payterm As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_sm As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_bill As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_pm As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_dt As System.Web.UI.WebControls.Label
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents File1 As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents tr_dt As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents hidInvTax As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidShipAmt As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidBalShip As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidresult As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button
    Protected WithEvents cmdUpload As System.Web.UI.WebControls.Button
    Protected WithEvents cmdView As System.Web.UI.WebControls.Button
    Protected WithEvents cmdClear As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents btnHidden As System.Web.UI.WebControls.Button
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents pnlAttach As System.Web.UI.WebControls.Panel

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
        icGSTRate = 6
        icGSTAmt = 7
        icGSTTaxCode = 8
        iTerms = 9
    End Enum
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CheckButtonAccess()
        'Check Access Status
        If Session("AccessStatus") = "Limited" Then
            cmdSubmit.Enabled = False
        End If
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'ViewState("line") = 0

        MyBase.Page_Load(sender, e)
        'blnCheckBox = False
        blnPaging = False
        blnSorting = False
        SetGridProperty(dtg_invDetail)
        If Not IsPostBack Then

            Dim objval As New InvValue
            Dim objinv As New Invoice
            Dim BILL_METH As String = Request(Trim("bill_meth"))

            'First delete temp attachment for invoice 
            objinv.delInvAttachTemp(, Session.SessionID)

            'ViewState("ShipAmt") = 0
            ViewState("Tax") = 0
            ViewState("BILL_METH") = BILL_METH
            'Dim objComp As New Companies
            ViewState("pono") = Request(Trim("pono"))
            ViewState("dono") = Request(Trim("dono"))
            Dim objDO As New DeliveryOrder
            Dim strDODate As String
            strDODate = objDO.getDODate(ViewState("dono"))
            ViewState("grnno") = Request(Trim("grnno"))
            Dim objGst As New GST
            ViewState("strGSTRegNo") = objGst.chkGST(, strDODate)
            objval.doc_num = Request(Trim("doc"))
           
            objval.Inv_no = ""
            objval.B_com_id = Request(Trim("bcomid"))
            objinv.get_invmstr2(objval, BILL_METH, Request(Trim("bcomid")))
            Dim yourref As String = Request(Trim("billdoc"))
            ViewState("billdoc") = yourref
            Me.lbl_InvNum.Text = "<B>To Be Assigned</B>"
            Me.lbl_date.Text = Date.Today
            Me.lbl_YourRef.Text = yourref
            Me.lbl_Payterm.Text = objval.pt
            Me.lbl_pm.Text = objval.pm
            Me.lbl_bill.Text = objval.adds
            'Me.txt_remark.Text = objval.remark????
            Me.lbl_req.Text = objval.create_by
            Me.lbl_contect.Text = objval.phone
            Me.lbl_cur_code.Text = objval.cur
            Me.lbl_attention.Text = ""
            Me.lbl_st.Text = objval.st
            Me.lbl_sm.Text = objval.sm
            Me.lbl_gst.Text = objval.tax_reg_no
            Me.lbl_attention.Text = objval.V_attn
            Me.hidBalShip.Value = objval.balShip
            ViewState("ShipAmt") = objval.balShip
            ViewState("payDay") = objval.payDay

            'Zulham 17072018 - PAMB
            'get cm_resident value
            Dim objDb As New EAD.DBCom
            ViewState("isResident") = objDb.GetVal("select IFNULL(cm_resident,'') from company_mstr where cm_coy_id ='" & Session("CompanyID") & "'")

            Bindgrid()
            AddRowtotal()

            If objval.deliveryTerm <> "" Then
                tr_dt.Style("display") = ""
                Me.lbl_dt.Text = objval.deliveryTerm
            Else
                tr_dt.Style("display") = "none"
                Me.lbl_dt.Text = objval.deliveryTerm
            End If

            cmdView.Visible = False
            lnkBack.NavigateUrl = dDispatcher.direct("Invoice", "InvList.aspx", "pageid=" & strPageId)
        End If

        displayAttachFile()

        'Me.cmd_back.Attributes.Add("onclick", "window.close(); ")
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String

        Dim objINV As New Invoice
        Dim ds As DataSet
        Dim strInv As String
        Dim blnAllowInv As Boolean

        If Request(Trim("bill_meth")) = "FPO" Then
            ds = objINV.getUnInvoicePOLine(Request(Trim("Doc")), Request(Trim("bcomid")), blnAllowInv, strInv)
        ElseIf Request(Trim("bill_meth")) = "DO" Then
            ds = objINV.getUnInvoiceDOLine(Request(Trim(" poidx")), Request(Trim("Doc")), Request(Trim("bcomid")), blnAllowInv, strInv)
        ElseIf Request(Trim("bill_meth")) = "GRN" Then
            ds = objINV.getUnInvoiceGRNLine(Request(Trim(" poidx")), Request(Trim("Doc")), Request(Trim("bcomid")), blnAllowInv, strInv)
        End If

        Dim dvViewPR As DataView
        dvViewPR = ds.Tables(0).DefaultView
        If pSorted Then
            dvViewPR.Sort = viewstate("SortExpression")
            If viewstate("SortAscending") = "no" Then dvViewPR.Sort += " DESC"
        End If

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured

        intPageRecordCnt = ds.Tables(0).Rows.Count

        '//bind datagrid
        If intPageRecordCnt > 0 Then

            dtg_invDetail.DataSource = dvViewPR
            Dim objPR As New PurchaseReq2

            'Modified by Joon on 18th Oct 2011 for issue 1001
            If objPR.Need2PrintPOCField(ds.Tables(0).Rows(0)("POM_PO_INDEX"), IIf(IsDBNull(ds.Tables(0).Rows(0)("POD_PR_INDEX")), 0, ds.Tables(0).Rows(0)("POD_PR_INDEX"))) Then '.Rows(0)("POD_PR_INDEX")) Then
                blnPrintCustField = True
            Else
                blnPrintCustField = False
            End If
            If objPR.Need2PrintPORemark(ds.Tables(0).Rows(0)("POM_PO_INDEX")) Then '
                blnPrintRemark = True
            Else
                blnPrintRemark = False
            End If

            If blnPrintCustField Then
                'addDataGridColumn(ds.Tables(0).Rows(0)("POD_PR_LINE"), ds.Tables(0).Rows(0)("POD_PR_INDEX"))
                addDataGridColumn(ds.Tables(0).Rows(0)("POD_PO_LINE"), ds.Tables(0).Rows(0)("POM_PO_INDEX"), IIf(IsDBNull(ds.Tables(0).Rows(0)("POD_PR_INDEX")), 0, ds.Tables(0).Rows(0)("POD_PR_INDEX")))
            End If
            dtg_invDetail.DataBind()
        Else
            dtg_invDetail.DataBind()

            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        Me.dtg_invDetail.Columns(EnumInvDet.iTerms).Visible = True
    End Function

    Private Sub cmdUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpload.Click
        If File1.Value <> "" Then
            Dim objFile As New FileManagement
            'Dim objPR As New PR

            ' Restrict user upload size
            'Dim dblMaxFileSize As Double = Val(objDB.GetVal("SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_TYPE = 'PR' AND SP_PARAM_NAME = 'DocAttachSize'"))
            Dim dblMaxFileSize As Double = Session("FileSize")

            Dim sFileName As String
            sFileName = System.IO.Path.GetFileName(File1.PostedFile.FileName)

            'Jules 2018.09.13 - Increase length from 46 to 200.
            If Len(sFileName) > 205 Then
                Common.NetMsgbox(Me, "File name exceeds 200 characters")
            ElseIf File1.PostedFile.ContentLength > 0 And File1.PostedFile.ContentLength / 1024 <= dblMaxFileSize Then
                objFile.FileUpload(File1, EnumUploadType.DocAttachmentTemp, "INV", EnumUploadFrom.FrontOff, Session.SessionID)
            ElseIf File1.PostedFile.ContentLength = 0 Then
                Common.NetMsgbox(Me, "0 byte document or file not found")
            Else
                Common.NetMsgbox(Me, "File exceeds maximum file size")
            End If

            'objPR = Nothing
            objFile = Nothing
        End If
        displayAttachFile()

    End Sub

    Private Sub displayAttachFile()
        Dim objInv As New Invoice
        Dim dsAttach As New DataSet
        Dim drvAttach As DataView
        Dim i As Integer
        Dim strFile, strFile1, strURL, strTemp As String

        dsAttach = objInv.getInvTempAttach(Session.SessionID)

        pnlAttach.Controls.Clear()
        drvAttach = dsAttach.Tables(0).DefaultView
        If drvAttach.Count > 0 Then
            For i = 0 To drvAttach.Count - 1
                strFile = drvAttach(i)("CDA_ATTACH_FILENAME")
                strFile1 = drvAttach(i)("CDA_HUB_FILENAME")
                Dim objFile As New FileManagement
                strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DocAttachment, "INV", EnumUploadFrom.FrontOff)
                objFile = Nothing
                '**********************meilai*****************************
                Dim lblBr As New Label
                Dim lblFile As New Label
                Dim lnk As New ImageButton
                lblFile.Text = "&nbsp;" & i + 1 & ") " & strURL & " (" & drvAttach(i)("CDA_FILESIZE") & "KB) "
                lblBr.Text = "<BR>"
                lnk.ImageUrl = dDispatcher.direct("Plugins/images", "i_delete2.gif")
                lnk.ID = drvAttach(i)("CDA_ATTACH_INDEX")
                lnk.CausesValidation = False
                AddHandler lnk.Click, AddressOf deleteAttach

                pnlAttach.Controls.Add(lblFile)
                pnlAttach.Controls.Add(lnk)
                pnlAttach.Controls.Add(lblBr)
            Next
        Else
            Dim lblFile As New Label
            lblFile.Text = "No Files Attached"
            pnlAttach.Controls.Add(lblFile)
        End If
        objInv = Nothing
    End Sub

    Private Sub deleteAttach(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim objInv As New Invoice

        objInv.delInvAttachTemp(CType(sender, ImageButton).ID)
        displayAttachFile()

        objInv = Nothing
    End Sub

    Private Sub addDataGridColumn(ByVal pr_line As String, ByVal pr_num As String, Optional ByVal pr_idx As Integer = 0)
        Dim objinv As New Invoice
        Dim dsCustom As New DataSet
        Dim dsCustomField As New DataSet
        Dim ds As New DataSet
        Dim rowcnt As Integer
        Dim i As Integer
        ds = objinv.get_CustFieldGrn(pr_line, pr_num, pr_idx)
        ViewState("pr_line") = pr_line
        ViewState("pr_num") = pr_num
        rowcnt = ds.Tables(0).Rows.Count

        'ReDim dvwCustom(rowcnt)
        'ReDim strCustomDefault(rowcnt)

        '//dynamicly add template column
        For i = 0 To rowcnt - 1
            Dim col As BoundColumn = New BoundColumn
            ' col.DataField = "PCD_FIELD_VALUE"
            col.HeaderText = ds.Tables(0).Rows(i)("PCM_FIELD_NAME")
            Me.dtg_invDetail.Columns.AddAt(dtg_invDetail.Columns.Count - 1, col)
        Next

    End Sub


    Sub AddRowtotal() 'add total row 
        Dim intL, intColToRemain As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim row2 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim row3 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim row4 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim row5 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim row6 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)

        '1st Row
        For intL = 0 To Me.dtg_invDetail.Columns.Count - 2
            addCell(row)
        Next

        row.Cells(EnumInvDet.icUPrice - 1).ColumnSpan = 2
        row.Cells(EnumInvDet.icUPrice - 1).Text = "Sub Total"
        row.Cells(EnumInvDet.icUPrice - 1).HorizontalAlign = HorizontalAlign.Right
        row.Cells(EnumInvDet.icUPrice - 1).Font.Bold = True

        row.Cells(EnumInvDet.icTotal - 1).Text = Format(ViewState("total"), "#,##0.00")
        row.Cells(EnumInvDet.icTotal - 1).HorizontalAlign = HorizontalAlign.Right
        row.Cells(EnumInvDet.icTotal - 1).Font.Bold = True

        Me.dtg_invDetail.Controls(0).Controls.Add(row)

        '2nd Row
        For intL = 0 To Me.dtg_invDetail.Columns.Count - 2
            addCell(row2)
        Next

        row2.Cells(EnumInvDet.icUPrice - 1).ColumnSpan = 2
        row2.Cells(EnumInvDet.icUPrice - 1).Text = "SST Amount"
        row2.Cells(EnumInvDet.icUPrice - 1).HorizontalAlign = HorizontalAlign.Right
        row2.Cells(EnumInvDet.icUPrice - 1).Font.Bold = True

        row2.Cells(EnumInvDet.icTotal - 1).Text = Format(ViewState("gst"), "#,##0.00")
        row2.Cells(EnumInvDet.icTotal - 1).HorizontalAlign = HorizontalAlign.Right
        row2.Cells(EnumInvDet.icTotal - 1).Font.Bold = True

        Me.dtg_invDetail.Controls(0).Controls.Add(row2)

        '3rd Row
        For intL = 0 To Me.dtg_invDetail.Columns.Count - 2
            addCell(row3)
        Next

        'row3.Cells(4).Text = "GST Amount"
        row3.Cells(EnumInvDet.icUPrice - 1).ColumnSpan = 2
        row3.Cells(EnumInvDet.icUPrice - 1).Text = "Sub Total (incl. Tax)" 'Jules 2018.10.19
        row3.Cells(EnumInvDet.icUPrice - 1).HorizontalAlign = HorizontalAlign.Right
        row3.Cells(EnumInvDet.icUPrice - 1).Font.Bold = True

        Dim lblTotal As New Label
        'lblTotal.ID = "lblTotal"
        'lblTotal.Text = Format((ViewState("total") + ViewState("gst")), "#,##0.00")
        'row3.Cells(EnumInvDet.icTotal - 1).Controls.Add(lblTotal)
        row3.Cells(EnumInvDet.icTotal - 1).Text = Format((ViewState("total") + ViewState("gst")), "#,##0.00")
        row3.Cells(EnumInvDet.icTotal - 1).HorizontalAlign = HorizontalAlign.Right
        'row3.Cells(5).BorderStyle = BorderStyle.Solid
        'row3.Cells(5).BorderWidth = "1"
        'row3.Cells(EnumInvDet.icTotal - 1).BackColor = Drawing.Color.LightSkyBlue
        row3.Cells(EnumInvDet.icTotal - 1).Font.Bold = True

        Me.dtg_invDetail.Controls(0).Controls.Add(row3)


        For intL = 0 To Me.dtg_invDetail.Columns.Count - 2
            addCell(row4)
        Next

        row4.Cells(EnumInvDet.icUPrice - 1).ColumnSpan = 2
        row4.Cells(EnumInvDet.icUPrice - 1).Text = "Shipping & Handling"
        row4.Cells(EnumInvDet.icUPrice - 1).HorizontalAlign = HorizontalAlign.Right
        row4.Cells(EnumInvDet.icUPrice - 1).Font.Bold = True


        Dim txtShipAmt As New TextBox
        txtShipAmt.ID = "txtShipAmt"
        txtShipAmt.CssClass = "numerictxtbox"
       
        txtShipAmt.Text = Replace(ViewState("ShipAmt"), ",", "")
        If Not IsNumeric(ViewState("ShipAmt")) Then
            txtShipAmt.Text = ViewState("ShipAmt")
            hidShipAmt.Value = 0
        Else
            If CDbl(txtShipAmt.Text) < 0 Then
                txtShipAmt.Text = "0.00"
                hidShipAmt.Value = 0
            Else
                txtShipAmt.Text = Format(CDbl(txtShipAmt.Text), "###0.00")
                hidShipAmt.Value = txtShipAmt.Text
            End If
        End If
        If Not IsNumeric(ViewState("ShipAmt")) Then
            ViewState("ShipAmt") = 0
        End If

        txtShipAmt.Attributes.Add("onkeypress", "return isDecimalKey(event);")
        'row4.Cells(5).Text = Format(CDbl(ViewState("ShipAmt")), "#,##0.00")
        row4.Cells(EnumInvDet.icTotal - 1).Controls.Add(txtShipAmt)
        row4.Cells(EnumInvDet.icTotal - 1).HorizontalAlign = HorizontalAlign.Right
        row4.Cells(EnumInvDet.icTotal - 1).Font.Bold = True

        Me.dtg_invDetail.Controls(0).Controls.Add(row4)

        For intL = 0 To Me.dtg_invDetail.Columns.Count - 2
            addCell(row5)
        Next

        Dim lblInvTax As New Label
        Dim txtInvTax As New TextBox
        Dim lblInvTax2 As New Label
        Dim lblInvTax3 As New Label
        lblInvTax.Text = "Invoice Tax "
        lblInvTax2.Text = "("
        lblInvTax3.Text = "%)"
        txtInvTax.ID = "txtInvTax"
        txtInvTax.CssClass = "numerictxtbox"

        ' Yap:2015-03-06: SEH want to remove it. Lock the TextBox first, remove the TextBox after GST Proj Over.
        txtInvTax.Enabled = False

        txtInvTax.Text = ViewState("Tax")
        If Not IsNumeric(ViewState("Tax")) Then
            txtInvTax.Text = ViewState("Tax")
            hidInvTax.Value = 0
        Else
            If CInt(txtInvTax.Text) < 0 Then
                txtInvTax.Text = "0.00"
                hidInvTax.Value = 0
            Else
                txtInvTax.Text = CInt(txtInvTax.Text)
                hidInvTax.Value = txtInvTax.Text
            End If
        End If
        If Not IsNumeric(ViewState("Tax")) Then
            ViewState("Tax") = 0
        End If

        txtInvTax.Width = "40"
        txtInvTax.Attributes.Add("onkeypress", "return isNumericKey(event);")

        'row5.Cells(4).Text = "Invoice Tax (%)"
        row5.Cells(EnumInvDet.icUPrice - 1).ColumnSpan = 2
        row5.Cells(EnumInvDet.icUPrice - 1).Controls.Add(lblInvTax)
        row5.Cells(EnumInvDet.icUPrice - 1).Controls.Add(lblInvTax2)
        row5.Cells(EnumInvDet.icUPrice - 1).Controls.Add(txtInvTax)
        row5.Cells(EnumInvDet.icUPrice - 1).Controls.Add(lblInvTax3)
        row5.Cells(EnumInvDet.icUPrice - 1).HorizontalAlign = HorizontalAlign.Right
        row5.Cells(EnumInvDet.icUPrice - 1).Font.Bold = True

        Dim lblTaxAmt As New Label
        lblTaxAmt.ID = "lblTaxAmt"
        If CInt(ViewState("Tax")) > 0 Then
            lblTaxAmt.Text = Format(((ViewState("total") + ViewState("gst") + ViewState("ShipAmt")) * CInt(ViewState("Tax"))) / 100, "#,##0.00")
        Else
            lblTaxAmt.Text = "0.00"
        End If
        row5.Cells(EnumInvDet.icTotal - 1).Controls.Add(lblTaxAmt)
        row5.Cells(EnumInvDet.icTotal - 1).HorizontalAlign = HorizontalAlign.Right
        row5.Cells(EnumInvDet.icTotal - 1).Font.Bold = True

        'Me.dtg_invDetail.Controls(0).Controls.Add(row5) 'Jules commented on 2018.10.19

        For intL = 0 To Me.dtg_invDetail.Columns.Count - 2
            addCell(row6)
        Next

        Dim lblGrandTotal As New Label
        lblGrandTotal.ID = "lblGrandTotal"
        lblGrandTotal.Text = Format((((ViewState("total") + ViewState("gst") + ViewState("ShipAmt")) / 100) * ViewState("Tax")) + (ViewState("total") + ViewState("gst") + ViewState("ShipAmt")), "#,##0.00")
        row6.Cells(EnumInvDet.icUPrice - 1).ColumnSpan = 2
        row6.Cells(EnumInvDet.icUPrice - 1).Text = "Grand Total"
        row6.Cells(EnumInvDet.icUPrice - 1).HorizontalAlign = HorizontalAlign.Right
        row6.Cells(EnumInvDet.icUPrice - 1).Font.Bold = True

        'row6.Cells(EnumInvDet.icTotal - 1).Text = Format((((ViewState("total") + ViewState("gst") + ViewState("ShipAmt")) / 100) * ViewState("Tax")) + (ViewState("total") + ViewState("gst") + ViewState("ShipAmt")), "#,##0.00")
        row6.Cells(EnumInvDet.icTotal - 1).Controls.Add(lblGrandTotal)
        row6.Cells(EnumInvDet.icTotal - 1).HorizontalAlign = HorizontalAlign.Right
        row6.Cells(EnumInvDet.icTotal - 1).Font.Bold = True

        Me.dtg_invDetail.Controls(0).Controls.Add(row6)

        txtShipAmt.Attributes.Add("onfocus", "return focusControl('" & _
                            (ViewState("total") + ViewState("gst")) & "', '" & txtInvTax.ClientID & "', '" & txtShipAmt.ClientID & "', '" & lblTaxAmt.ClientID & "', '" & lblGrandTotal.ClientID & "');")

        txtShipAmt.Attributes.Add("onblur", "return calculateGrandTotal('" & _
                        (ViewState("total") + ViewState("gst")) & "', '" & txtInvTax.ClientID & "', '" & txtShipAmt.ClientID & "', '" & lblTaxAmt.ClientID & "', '" & lblGrandTotal.ClientID & "');")

        txtInvTax.Attributes.Add("onfocus", "return focusControl('" & _
                            (ViewState("total") + ViewState("gst")) & "', '" & txtInvTax.ClientID & "', '" & txtShipAmt.ClientID & "', '" & lblTaxAmt.ClientID & "', '" & lblGrandTotal.ClientID & "');")

        txtInvTax.Attributes.Add("onblur", "return calculateGrandTotal('" & _
                        (ViewState("total") + ViewState("gst")) & "', '" & txtInvTax.ClientID & "', '" & txtShipAmt.ClientID & "', '" & lblTaxAmt.ClientID & "', '" & lblGrandTotal.ClientID & "');")

        'Jules 2018.07.18 - To ensure calculation is correct every time grid is bound.
        ViewState("total") = 0
        ViewState("gst") = 0
    End Sub

    Sub addCell(ByRef row As DataGridItem)
        Dim cell As TableCell = New TableCell
        row.Cells.Add(cell)
    End Sub

    Private Sub dtg_invDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_invDetail.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim dblSubTotal As Decimal
            Dim objinv As New Invoice
            Dim dsCustom As New DataSet
            Dim dsCustomField As New DataSet
            Dim ds As New DataSet
            Dim rowcnt As Integer
            Dim i, j As Integer
            Dim lstItem As New ListItem
            Dim objGlobal As New AppGlobals

            '2015-06-22: CH: Rounding issue (Prod issue)
            'dblSubTotal = Common.parseNull(dv("QTY"), 0) * Common.parseNull(dv("POD_UNIT_COST"), 0)
            dblSubTotal = CDec(Format(Common.parseNull(dv("QTY"), 0) * Common.parseNull(dv("POD_UNIT_COST"), 0), "###0.00"))
            e.Item.Cells(EnumInvDet.icTotal).Text = Format(dblSubTotal, "#,##0.00")
            e.Item.Cells(EnumInvDet.icUPrice).Text = Format(dv("POD_UNIT_COST"), "#,##0.0000")
            ViewState("total") = ViewState("total") + dblSubTotal
            e.Item.Cells(EnumInvDet.icGSTRate).Text = Common.parseNull(dv("GST_RATE"))
            If Common.parseNull(dv("POD_GST"), 0) > 0 Then
                e.Item.Cells(EnumInvDet.icGSTAmt).Text = Format(dblSubTotal * Common.parseNull(dv("POD_GST"), 0) / 100, "##,###,##0.00")
                ViewState("gst") = ViewState("gst") + CDec(Format(dblSubTotal * Common.parseNull(dv("POD_GST"), 0) / 100, "##0.00"))
            Else
                e.Item.Cells(EnumInvDet.icGSTAmt).Text = "0.00"
                ViewState("gst") = ViewState("gst") + 0
            End If
            'ViewState("gst") = ViewState("gst") + Common.parseNull(dv("POD_TAX_VALUE"), 0)
            e.Item.Cells(EnumInvDet.icLineItem).Text = Common.parseNull(dv("POD_PO_LINE"))

            Dim ddlTaxCode As DropDownList
            ddlTaxCode = e.Item.Cells(EnumInvDet.icGSTTaxCode).FindControl("ddlTaxCode")
            If ViewState("strGSTRegNo") <> "" Then
                objGlobal.FillTaxCode(ddlTaxCode, Common.parseNull(dv("POD_GST_RATE")), "S", , False)

                If arySTaxCode.Count > 0 Then
                    For j = 0 To arySTaxCode.Count - 1
                        If Common.parseNull(dv("POD_PO_LINE")) = arySTaxCode(j)(0) Then
                            ddlTaxCode.SelectedValue = arySTaxCode(j)(1)
                        End If
                    Next
                Else
                    ddlTaxCode.SelectedValue = "SR1"
                End If

                If ddlTaxCode.SelectedValue = "N/A" Then
                    ddlTaxCode.Enabled = False
                Else
                    ddlTaxCode.Enabled = True
                End If
            Else
                ddlTaxCode.Items.Clear()
                lstItem.Value = "N/A"
                lstItem.Text = "N/A"
                ddlTaxCode.Items.Insert(0, lstItem)
                ddlTaxCode.Enabled = False
            End If

            '??? why call viewstate (by Moo)
            'ds = objinv.get_CustFieldGrn(VIEWSTATE("pr_line"), VIEWSTATE("pr_num"))
            If blnPrintCustField Then
                ds = objinv.get_CustFieldGrn(dv("POD_PO_LINE"), dv("POM_PO_INDEX"), IIf(IsDBNull(dv("POD_PR_INDEX")), 0, dv("POD_PR_INDEX")))
                rowcnt = ds.Tables(0).Rows.Count

                For i = 0 To rowcnt - 1
                    e.Item.Cells(EnumInvDet.iTerms + i).Text = ds.Tables(0).Rows(i)("PCD_FIELD_VALUE")
                Next
            End If
        End If
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_SearchGInv_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Invoice", "InvList.aspx", "pageid=" & strPageId) & """><span>Issue Invoice</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Invoice", "invoiceView.aspx", "pageid=" & strPageId) & """><span>Invoice Listing</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                            "</ul><div></div></div>"
    End Sub

    Private Function validateDatagrid(ByRef strMsg As String) As Boolean
        validateDatagrid = True
        strMsg = "<ul type='disc'>"

        If Not Common.checkMaxLength(txtRemark.Text, 1000) Then
            strMsg &= "<li>Remarks is over limit.<ul type='disc'></ul></li>"
            validateDatagrid = False
        End If

        If hidInvTax.Value <> "" Then
            If Not System.Text.RegularExpressions.Regex.IsMatch(hidInvTax.Value, "^\d+$") Then
                strMsg &= "<li>Invalid Invoice Tax.<ul type='disc'></ul></li>"
                validateDatagrid = False
            End If
        End If

        If hidShipAmt.Value <> "" Then
            If Not System.Text.RegularExpressions.Regex.IsMatch(hidShipAmt.Value, "^\d{1,3}(?:\.(?:\d{0,3})?\d)?$") Then
                strMsg &= "<li>Invalid Shipping & Handling.<ul type='disc'></ul></li>"
                validateDatagrid = False
            End If
        End If

        'Jules 2019.03.12
        If txtRef.Text = "" Then
            strMsg &= "<li>Reference is required.<ul type='disc'></ul></li>"
            validateDatagrid = False
        Else
            If Not Common.checkMaxLength(txtRef.Text, 30) Then
                strMsg &= "<li>Reference length is over the limit.<ul type='disc'></ul></li>"
                validateDatagrid = False
            End If
        End If
        'End modification.

        strMsg &= "</ul>"

    End Function

    Sub SaveSupplyTaxCode()
        Dim dgitem As DataGridItem
        Dim ddlTaxCode As DropDownList
        For Each dgitem In dtg_invDetail.Items
            ddlTaxCode = dgitem.FindControl("ddlTaxCode")
            arySTaxCode.Add(New String() {dgitem.Cells(EnumInvDet.icLineItem).Text, ddlTaxCode.SelectedValue})
        Next

    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click

        SaveSupplyTaxCode()
        lblMsg.Text = ""
        If validateDatagrid(strMsg) Then
            strMsg = ""
            If ShipAmtTally() Then
                Submit()
            Else
                Dim strscript As New System.Text.StringBuilder
                strscript.Append("<script language=""javascript"">")
                'strscript.Append("document.getElementById('cmd_submit').style.display='none';")
                strscript.Append("PromptMsg('" & strMsg & "');")
                strscript.Append("document.getElementById('btnHidden').click();")
                strscript.Append("</script>")
                RegisterStartupScript("script1", strscript.ToString())
                cmdSubmit.Visible = False
            End If
        Else
            lblMsg.Text = strMsg
        End If

        resetDataGrid()
    End Sub

    Private Sub Submit()
        'back_view.Visible = False
        Dim objinv As New Invoice
        Dim objval As New InvValue
        Dim DS As New DataSet
        Dim dtr As DataRow
        Dim crdetail As New DataTable
        Dim strinv As String
        Dim strgrn As String
        Dim strbcomid As String

        objval.po_no = Common.parseNull(ViewState("pono"))
        objval.GRN_NO = Common.parseNull(ViewState("grnno"))
        If Request(Trim("bill_meth")) = "GRN" Then
            ViewState("TOTAL") = objinv.get_grnprice(ViewState("pono"), Request(Trim("bcomid")), Request(Trim("poidx")), ViewState("grnno"))
        ElseIf Request(Trim("bill_meth")) = "FPO" Then
            objval.po_no = ViewState("pono")
            objval.B_com_id = Request(Trim("bcomid"))
            objinv.get_POprice(objval)
            ViewState("TOTAL") = objval.Ordered_amount
        ElseIf Request(Trim("bill_meth")) = "DO" Then
            ViewState("TOTAL") = objinv.get_DOprice(ViewState("dono"), Request(Trim("bcomid")), Request(Trim("poidx")))
        End If

        crdetail.Columns.Add("doc", Type.GetType("System.String"))
        crdetail.Columns.Add("ref", Type.GetType("System.String"))
        crdetail.Columns.Add("remark", Type.GetType("System.String"))
        crdetail.Columns.Add("amount", Type.GetType("System.String"))
        crdetail.Columns.Add("b_com_id", Type.GetType("System.String"))
        crdetail.Columns.Add("INV_STATUS", Type.GetType("System.String"))
        crdetail.Columns.Add("bill_meth", Type.GetType("System.String"))
        crdetail.Columns.Add("po_no", Type.GetType("System.String"))
        crdetail.Columns.Add("grn_no", Type.GetType("System.String"))
        crdetail.Columns.Add("do_no", Type.GetType("System.String"))
        crdetail.Columns.Add("pay_day", Type.GetType("System.String"))
        crdetail.Columns.Add("tax", Type.GetType("System.String"))
        crdetail.Columns.Add("ShipAmt", Type.GetType("System.String"))
        crdetail.Columns.Add("POM_PO_INDEX", Type.GetType("System.String"))
        Dim status As Integer = invStatus.NewInv

        dtr = crdetail.NewRow()
        dtr("doc") = ViewState("billdoc") 'Request(Trim("doc")) 'dgitem.Cells(EnumInv2.icDoc1).Text
        dtr("ref") = txtRef.Text
        dtr("remark") = txtRemark.Text
        dtr("amount") = Format(ViewState("TOTAL"), "#,##0.00")  'VIEWSTATE("TOTAL")
        dtr("b_com_id") = Request(Trim("bcomid"))
        dtr("inv_status") = status
        dtr("bill_meth") = Request(Trim("bill_meth"))
        dtr("po_no") = ViewState("pono")
        dtr("grn_no") = ViewState("grnno")
        dtr("do_no") = ViewState("dono")
        dtr("pay_day") = ViewState("payDay")
        dtr("tax") = IIf(hidInvTax.Value = "", 0, hidInvTax.Value)
        dtr("ShipAmt") = IIf(hidShipAmt.Value = "", 0, hidShipAmt.Value) 'Replace(hidShipAmt.Value, ",", "")
        dtr("POM_PO_INDEX") = Request(Trim("poidx"))
        crdetail.Rows.Add(dtr)
        If strgrn = "" Then
            strgrn = "'" & ViewState("grnno") & "'"
        Else
            strgrn = strgrn & ",'" & ViewState("grnno") & "'"
        End If

        DS.Tables.Add(crdetail)
        Dim GRN_STATUS As Integer
        GRN_STATUS = GRNStatus.Invoiced
        Dim do_status As Integer
        do_status = DOStatus.Invoiced
        Dim url As String
        Dim strMsgLine As String = ""
        'Zulham 17072018 - PAMB 
        If Not objinv.Update_InvMstr(DS, GRN_STATUS, do_status, strinv, strbcomid, arySTaxCode, IIf(ViewState("strGSTRegNo") <> "", "Y", "N"), isResident:=ViewState("isResident")) Then
            Common.NetMsgbox(Me, MsgTransDup, dDispatcher.direct("Invoice", "InvList.aspx", "pageid=" & strPageId))
        Else
            'strMsgLine = "The Following invoice(s) have been generated."
            If InStr(strinv, ",") > 0 Then
                strMsgLine = "Invoice Number " & strinv & " have been generated."

            Else
                If strinv = "Generated" Then
                    strMsgLine = "Invoice Number has been generated."
                Else
                    strMsgLine = "Invoice Number " & strinv & " has been generated."
                End If

            End If

            Me.lbl_InvNum.Text = strinv
            Common.NetMsgbox(Me, strMsgLine)
            cmdSubmit.Visible = False
            cmdClear.Visible = False
            cmdView.Visible = True
            cmdView.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewInvoice.aspx", "vcomid=" & Session("CompanyId") & "&INVNO=" & strinv & "&BCoyID=" & Request(Trim("bcomid"))) & "')")
            Me.cmdSubmit.Enabled = False
            Me.cmdView.Enabled = True

        End If
    End Sub

    Private Function ShipAmtTally() As Boolean
        Dim dblShipAmt, dblBalShip As Double

        dblShipAmt = CDbl(hidShipAmt.Value)
        dblBalShip = CDbl(hidBalShip.Value)
        If dblBalShip < 0 Then dblBalShip = 0
        If dblShipAmt <> dblBalShip Then
            If strMsg = "" Then
                strMsg = "The shipping & handling amount of the following PO(s) varies from the Invoice:"
            End If
            strMsg = strMsg + "\nAccumulated shipping & handling amount for " + ViewState("pono") + " is " + Format(dblBalShip, "###,###,##0.00")
        End If
        If strMsg <> "" Then
            strMsg = strMsg + "\nWant to proceed? "
            Return False
        Else
            Return True
        End If
    End Function

    Public Sub btnHidden_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHidden.Click
        If hidresult.Value = "1" Then
            Submit()
        Else
            cmdSubmit.Visible = True
        End If

        resetDataGrid()

    End Sub

    Sub resetDataGrid()

        ViewState("Tax") = IIf(hidInvTax.Value = "", 0, hidInvTax.Value)
        ViewState("ShipAmt") = IIf(hidShipAmt.Value = "", 0, hidShipAmt.Value)
        Bindgrid() 'Jules 2018.07.18 - To ensure custom fields are also displayed
        AddRowtotal()
    End Sub
End Class
