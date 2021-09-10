Imports AgoraLegacy
Imports eProcure.Component
Imports System.IO

Public Class InvoiceReport
    Inherits AgoraLegacy.AppBaseClass
    Dim strPRIndex As String = ""
    Dim blnPrintRemark As Boolean
    Dim blnDrawLine As Boolean = False
    Dim i, jLoop, iLoop As Integer
    Protected WithEvents lblComName As System.Web.UI.WebControls.Label
    Protected WithEvents lblPayTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lblPayMethod As System.Web.UI.WebControls.Label
    Protected WithEvents lblShipType As System.Web.UI.WebControls.Label
    Protected WithEvents lblShipMode As System.Web.UI.WebControls.Label
    Protected WithEvents lblInvNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblOurRef As System.Web.UI.WebControls.Label
    Protected WithEvents lblYourRef As System.Web.UI.WebControls.Label
    Protected WithEvents lblVenRemarks As System.Web.UI.WebControls.Label
    Protected WithEvents Image1 As System.Web.UI.WebControls.Image
    Protected WithEvents lblTel As System.Web.UI.WebControls.Label
    Protected WithEvents lblAddr As System.Web.UI.WebControls.Label
    Protected WithEvents lblBusRegNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblEmail As System.Web.UI.WebControls.Label
    Protected WithEvents lblBillTo As System.Web.UI.WebControls.Label
    Protected WithEvents lblBCoyName As System.Web.UI.WebControls.Label
    Dim blnPrintCustField As Boolean
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents dtg_PrevInvoice As System.Web.UI.WebControls.DataGrid
    Dim value1(100) As String
    Dim value2(100) As String
    '    Dim i As Integer

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Public Enum EnumPrevInv
        icPOLine = 0
        icProDesc = 1
        icUOM = 2
        icRecQty = 3
        icUCost = 4
        icAmount = 5
        icGST = 6
        icTerms = 7
    End Enum

    Dim blnFreeze As Boolean
    Dim strNewUrl As String

    Public Property NewUrl() As String
        Get
            NewUrl = strNewUrl
        End Get
        Set(ByVal Value As String)
            strNewUrl = Value
        End Set
    End Property

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CheckButtonAccess()
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If (blnFreeze) Then
            Dim htmlFile As New AppHtml
            ' let Asp.net render the output, catch it in the file creator
            MyBase.Render(htmlFile.RenderHere)
            htmlFile.WriteHTMLFile(Server.MapPath(NewUrl))

            Dim objFile As New FileManagement
            Dim strFilePath, strMovePath As String
            objFile.getFilePath(EnumUploadFrom.FrontOff, EnumUploadType.PDFDownload, strFilePath, strMovePath, False, "INV")
            objFile = Nothing
            Dim strFileName As String
            strFileName = strFilePath & Session("InvFileName")

            If Not File.Exists(strFileName) Then
                File.Copy(Server.MapPath(NewUrl), strFileName)
            Else
                ' if file exist, then add the newUrl file as the next page of strFileName
                htmlFile.WriteHTMLFile(Server.MapPath(NewUrl), strFileName)
            End If
            ' ai chu add on 21/09/2005
            ' to remove generated file in ExtraFunc folder or respective module folder
            File.Delete(Server.MapPath(NewUrl))
        Else
            MyBase.Render(writer)
        End If
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here

        MyBase.Page_Load(sender, e)
        ' SetGridProperty(dtg_PrevInvoice)

        If Not IsPostBack Then
            If Request(Trim("inv_from")) = "admin" Then
                ViewState("inv_from") = Request(Trim("inv_from"))
                displayPOReport2()
                Bindgrid()
            Else
                Bindgrid()
                ' ai chu add
                If Request.QueryString("freeze") = "1" Then
                    blnFreeze = True
                    Freeze(Session.SessionID & "_Invoice.htm")
                Else
                    blnFreeze = False
                End If
                ' -------------------------------

                Dim objval As New InvValue
                Dim objinv As New Invoice

                Dim objPO As New PurchaseOrder
                Dim objPOVal As New POValue
                ' Dim bill_meth As String = Request(Trim("bill_meth"))
                ' bill_meth = bill_meth.Substring(1)
                objval.doc_num = Request(Trim("doc"))
                objval.Inv_no = Request(Trim("INVNO"))
                objval.Vcom_id = Request(Trim("vcomid"))
                objinv.get_invmstr(objval)
                'Dim yourref() As String = Split(objval.your_ref, ",")
                'objPOVal.PO_Number = objval.po_no 'Request(Trim("POM_PO_NO"))
                'objPOVal.buyer_coy =' Request(Trim("POM_COY_NO"))


                'objPO.get_PODetail(objPOVal, "", False)

                Me.lblTel.Text = objval.vphone
                Me.lblEmail.Text = objval.email
                Me.lblInvNo.Text = Request(Trim("INVNO"))
                Me.lblDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, objval.create_on)
                Me.lblYourRef.Text = Replace(objval.your_ref, ",", "<BR>")
                'Me.lblYourRef.Text = "&nbsp;" & Me.lblYourRef.Text
                Me.lblOurRef.Text = objval.our_ref
                Me.lblPayTerm.Text = objval.pt
                Me.lblPayMethod.Text = objval.pm
                ' Me.lbl_bill.Text = objval.adds
                Me.lblVenRemarks.Text = objval.remark
                ' Me.lbl_req.Text = objval.create_by
                ' Me.lbl_contect.Text = objval.phone
                '  Me.lbl_cur_code.Text = objval.cur
                ' Me.lbl_attention.Text = ""
                Me.lblComName.Text = objval.v_com_name
                Me.lblBillTo.Text = objval.adds
                Me.lblShipType.Text = objval.st
                Me.lblShipMode.Text = objval.sm
                Me.lblAddr.Text = objval.ven_add
                Me.lblBusRegNo.Text = objval.bussiness_reg

                'me.lblEmail.Text=objval.e
                Me.lblVenRemarks.Text = objval.remark
                Me.lblBCoyName.Text = objval.BComName

                Dim objFile As New FileManagement
                Dim strImgSrc As String
                'If Request.QueryString("img") <> "" Then
                '    Image1.ImageUrl = Request.QueryString("img")
                'Else
                strImgSrc = objFile.getCoyLogo(EnumUploadFrom.FrontOff, Request(Trim("vcomid")))
                If strImgSrc <> "" Then
                    Image1.Visible = True
                    '//add by Moo.
                    '//convert imagefile to bitmap. 
                    '//then compare physical image width with httpimage control width.
                    '//purpose : to prevent small image from stretch
                    '//but image with width > 140px will become blur/samller. height is maintain.
                    'Dim bitmapImg As New Bitmap(Server.MapPath(strImgSrc))
                    Dim bitmapImg As System.Drawing.Image = System.Drawing.Image.FromFile(Server.MapPath(strImgSrc))
                    If bitmapImg.Width < Image1.Width.Value Then
                        Image1.Width = System.Web.UI.WebControls.Unit.Pixel(bitmapImg.Width)
                    End If
                    '*****Testing Only, for resizing based on fixed image width and height *********
                    'Dim currentImage As System.Drawing.Image = System.Drawing.Image.FromFile(Server.MapPath(strImgSrc))
                    'Dim imgHeight, imgWidth As Integer
                    'Const maxWidth As Integer = 200
                    'Const maxHeight As Integer = 200
                    'imgHeight = currentImage.Height
                    'imgWidth = currentImage.Width
                    'If imgWidth > maxWidth Or imgHeight > maxHeight Then
                    '    'Determine what dimension is off by more
                    '    Dim deltaWidth As Integer = imgWidth - maxWidth
                    '    Dim deltaHeight As Integer = imgHeight - maxHeight
                    '    Dim scaleFactor As Double

                    '    If deltaHeight > deltaWidth Then
                    '        'Scale by the height
                    '        scaleFactor = maxHeight / imgHeight
                    '    Else
                    '        'Scale by the Width
                    '        scaleFactor = maxWidth / imgWidth
                    '    End If

                    '    imgWidth *= scaleFactor
                    '    imgHeight *= scaleFactor
                    '    Image1.Width = System.Web.UI.WebControls.Unit.Pixel(imgWidth)
                    '    Image1.Height = System.Web.UI.WebControls.Unit.Pixel(imgHeight)
                    'End If
                    '*****Testing Only, for resizing based on fixed image width and height *********
                    If blnFreeze Then
                        Image1.ImageUrl = "file:///" & Server.MapPath(strImgSrc)
                    Else
                        Image1.ImageUrl = strImgSrc
                    End If
                Else
                    Image1.Visible = False
                End If
                objFile = Nothing
                'End If
            End If
        End If
    End Sub

    Protected Sub Freeze(ByVal strtoUrl As String)
        blnFreeze = True
        NewUrl = strtoUrl
    End Sub
    Private Function displayPOReport2()

        Dim objrfq As New RFQ
        lblComName.Text = Request(Trim("com_id")) 'objrfq.get_comname(Session("CompanyID"))
        lblAddr.Text = "XXXXX"
        lblInvNo.Text = "INV/XXXXX"
        lblDate.Text = "DD/MM/YYYY"
        lblPayTerm.Text = "XX Days"
        lblPayMethod.Text = "XXXXX"
        lblShipType.Text = "XXXXX"
        lblShipMode.Text = "XXXXX"
        lblBusRegNo.Text = "XXXXX"
        lblEmail.Text = "XXXXX"
        lblTel.Text = "XXXXX"
        lblTel.Text = "XXXXX"
        lblBillTo.Text = "XXXXX"
        lblBCoyName.Text = "XXXXX"
        lblVenRemarks.Text = "XXXXX"
        lblOurRef.Text = "XXXXX"
        lblYourRef.Text = "XXXXX"

        If Request.QueryString("img") <> "" Then
            Image1.ImageUrl = Request.QueryString("img")
        Else
            Dim objFile As New FileManagement
            Dim strImgSrc As String
            strImgSrc = objFile.getCoyLogo(EnumUploadFrom.FrontOff, Request(Trim("BCoyID")))
            If strImgSrc <> "" Then
                Image1.Visible = True
                Image1.ImageUrl = strImgSrc
            Else
                Image1.Visible = False
            End If
            objFile = Nothing
        End If
    End Function
    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String

        Dim objinv As New Invoice
        'Dim objval As New POValue
        Dim ds As New DataSet
        Dim strInvCode As String
        Dim bill_meth As String
        Dim dvViewSample As DataView

        Dim dtr As DataRow
        Dim crdetail As New DataTable
        If ViewState("inv_from") = "admin" Then


            crdetail.Columns.Add("ID_PRODUCT_DESC", Type.GetType("System.String"))
            crdetail.Columns.Add("ID_UOM", Type.GetType("System.String"))
            crdetail.Columns.Add("ID_RECEIVED_QTY", Type.GetType("System.String"))
            crdetail.Columns.Add("ID_UNIT_COST", Type.GetType("System.String"))
            crdetail.Columns.Add("ID_GST", Type.GetType("System.String"))
            crdetail.Columns.Add("ID_WARRANTY_TERMS", Type.GetType("System.String"))



            dtr = crdetail.NewRow()

            dtr("ID_PRODUCT_DESC") = "XXXXX"
            dtr("ID_UOM") = "XXXXX"
            dtr("ID_RECEIVED_QTY") = "XXXXX"
            dtr("ID_UNIT_COST") = "XXXXX"
            dtr("ID_GST") = "XXXXX"
            dtr("ID_WARRANTY_TERMS") = "XXXXX"

            crdetail.Rows.Add(dtr)
            ds.Tables.Add(crdetail)
            i = 0
            dvViewSample = ds.Tables(0).DefaultView
            dtg_PrevInvoice.DataSource = dvViewSample
            dtg_PrevInvoice.DataBind()
            If ViewState("inv_from") = "admin" Then
                ViewState("total") = 0
                ViewState("gst") = 0
            End If
            AddRowtotal(5)
        Else

            ds = objinv.inv_detail(Request(Trim("INVNO")), Request(Trim("vcomid")))
            'ds.Tables(0).Rows(1)(2)
            dvViewSample = ds.Tables(0).DefaultView
            'dvViewSample(0)(0)
            If pSorted Then
                dvViewSample.Sort = ViewState("SortExpression")
                If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"
            End If
            i = 0
            dtg_PrevInvoice.DataSource = dvViewSample
            dtg_PrevInvoice.DataBind()
            AddRowtotal(5)

        End If


    End Function

    Private Sub dtg_PrevInvoice_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_PrevInvoice.ItemDataBound
        If i = 0 Then
            Dim money As String
            Dim ds As DataSet
            Dim strInv As String
            Dim objinv As New Invoice

            If ViewState("inv_from") = "admin" Then
                money = "XXX"
            Else
                strInv = Request(Trim("INVNO"))
                Dim objpo As New PurchaseOrder
                ds = objinv.get_poitem(strInv, Request(Trim("vcomid")))
                money = ds.Tables(0).Rows(0)("POM_CURRENCY_CODE")
            End If

            AddRowmoney(4, money)
            DrawLine()
        End If

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim objinv As New Invoice
            Dim objval As New InvValue
            Dim po_line As String
            Dim po_index As String
            Dim j As Integer
            ' Dim lineno As Integer

            If ViewState("inv_from") = "admin" Then
                e.Item.Cells(EnumPrevInv.icPOLine).Text = "XX"
                e.Item.Cells(EnumPrevInv.icAmount).Text = "XXXXX"
                e.Item.Cells(EnumPrevInv.icUCost).Text = "XXXXX"
                AddRow2(1, 1, "XXXXX")
            Else

                ViewState("lineno") = ViewState("lineno") + 1
                e.Item.Cells(EnumPrevInv.icPOLine).Text = ViewState("lineno")
                e.Item.Cells(EnumPrevInv.icUCost).Text = Format(dv("ID_UNIT_COST"), "#,##0.0000")
                e.Item.Cells(EnumPrevInv.icAmount).Text = Format(dv("ID_RECEIVED_QTY") * dv("ID_UNIT_COST"), "#,##0.0000")
                e.Item.Cells(EnumPrevInv.icGST).Text = Format((dv("ID_GST") / 100) * dv("ID_RECEIVED_QTY") * dv("ID_UNIT_COST"), "#,##0.0000")

                If strPRIndex = "" Then
                    objinv.get_podetail(Common.parseNull(dv("ID_INVOICE_NO")), Common.parseNull(dv("IM_B_COY_ID")), Common.parseNull(dv("ID_PO_LINE")), Request(Trim("vcomid")), objval)
                    strPRIndex = objval.PRINDEX
                    Dim objPR As New PurchaseReq2
                    If objPR.Need2PrintCField(CInt(strPRIndex)) Then
                        blnPrintCustField = True
                    Else
                        blnPrintCustField = False
                    End If
                    If objPR.Need2PrintRemark(CInt(strPRIndex)) Then
                        blnPrintRemark = True
                    Else
                        blnPrintRemark = False
                    End If
                    objPR = Nothing
                Else
                    If blnPrintCustField Then
                        objinv.get_podetail(Common.parseNull(dv("ID_INVOICE_NO")), Common.parseNull(dv("IM_B_COY_ID")), Common.parseNull(dv("ID_PO_LINE")), Request(Trim("vcomid")), objval)
                    End If
                End If
                '//TO PRINT CUSTOM FIELD
                '//ONLY SHOW CUSTOM FIELD IF IT IS CHECKED IN PR
                If blnPrintCustField Then
                    objinv.get_custgrn2(value1, value2, objval.LINE, objval.PRINDEX, j)
                End If

                ViewState("gst") = ViewState("gst") + (dv("ID_GST") / 100) * dv("ID_RECEIVED_QTY") * dv("ID_UNIT_COST")
                ViewState("total") = ViewState("total") + (dv("ID_RECEIVED_QTY") * dv("ID_UNIT_COST"))

                '//for PR consolidation, 1 PO -> many PR
                '//assume PR with different setting can't be consolidated
                If blnPrintRemark Then
                    AddRow(1, value1, value2, j, objval.remark)
                End If
            End If
        End If
        i = i + 1
    End Sub
    Sub AddRow2(ByVal intCell As Integer, ByVal j As Integer, ByVal remark As String)
        'adding totals row
        Dim intL, intColToRemain As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim row2 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim intTotalCol As Integer
        Dim count As Integer
        Dim strtext As String

        For count = 0 To j - 1
            strtext = strtext & "<B>XXXXX</B> : XXXXX "
            strtext = strtext & "<br>"
        Next
        For intL = 0 To Me.dtg_PrevInvoice.Columns.Count - 1
            addCell(row)
        Next

        For intL = 0 To dtg_PrevInvoice.Columns.Count - 3
            row.Cells.RemoveAt(1)
        Next

        'row.Cells(0).ColumnSpan = intCell - 1
        Dim lbl_test As New Label
        lbl_test.ID = "test"
        lbl_test.Text = strtext
        lbl_test.CssClass = "txtbox"
        lbl_test.Width = System.Web.UI.WebControls.Unit.Pixel(500)
        lbl_test.Font.Bold = False
        row.Cells(1).Controls.Add(lbl_test)
        row.Cells(1).HorizontalAlign = HorizontalAlign.Left
        row.Cells(1).ColumnSpan = 10

        Me.dtg_PrevInvoice.Controls(0).Controls.Add(row)

        For intL = 0 To dtg_PrevInvoice.Columns.Count - 1
            addCell(row2)
        Next

        For intL = 0 To dtg_PrevInvoice.Columns.Count - 3
            row2.Cells.RemoveAt(1)
        Next

        ' row.Cells(0).ColumnSpan = intCell - 1
        Dim lbl_test2 As New Label
        lbl_test2.ID = "test2"
        lbl_test2.Text = "<B>Remarks</B> : " & remark & ""
        lbl_test2.CssClass = "txtbox"
        lbl_test2.Width = System.Web.UI.WebControls.Unit.Pixel(500)
        lbl_test2.Font.Bold = False
        lbl_test2.Font.Bold = False

        row2.Cells(1).Controls.Add(lbl_test2)
        row2.Cells(1).ColumnSpan = 10
        row2.Cells(1).HorizontalAlign = HorizontalAlign.Left
        Me.dtg_PrevInvoice.Controls(0).Controls.Add(row2)
    End Sub
    Sub AddRow(ByVal intCell As Integer, ByVal value1() As String, ByVal value2() As String, ByVal j As Integer, ByVal remark As String)
        'adding totals row
        Dim intL, intColToRemain As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim row2 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim intTotalCol As Integer
        Dim count As Integer
        Dim strtext As String
        ' value2(count)
        ' value1(count)

        'Dim test1 As String = "&^TEST1"
        'Dim test2 As String = "04 Repair & Maintenance-:6600"

        ' Dim DECODETEST2 As String
        ' DECODETEST2 = Me.Server.HtmlDecode(test2)
        For count = 0 To j - 1
            strtext = strtext & "<B>" & value1(count) & "</B> : " & value2(count) & ""
            strtext = strtext & "<br>"
        Next
        For intL = 0 To Me.dtg_PrevInvoice.Columns.Count - 1
            addCell(row)
        Next


        For intL = 0 To dtg_PrevInvoice.Columns.Count - 3
            row.Cells.RemoveAt(1)
        Next

        'row.Cells(0).ColumnSpan = intCell - 1
        Dim lbl_test As New Label
        lbl_test.ID = "test"
        lbl_test.Text = strtext
        ' lbl_test.CssClass = "lblnumerictxtbox"
        lbl_test.Width = System.Web.UI.WebControls.Unit.Pixel(500)
        lbl_test.Font.Bold = False
        row.Cells(1).Controls.Add(lbl_test)
        row.Cells(1).HorizontalAlign = HorizontalAlign.Left
        row.Cells(1).ColumnSpan = 10

        Me.dtg_PrevInvoice.Controls(0).Controls.Add(row)


        For intL = 0 To dtg_PrevInvoice.Columns.Count - 1
            addCell(row2)
        Next

        For intL = 0 To dtg_PrevInvoice.Columns.Count - 3
            row2.Cells.RemoveAt(1)
        Next

        ' row.Cells(0).ColumnSpan = intCell - 1
        Dim lbl_test2 As New Label
        lbl_test2.ID = "test2"
        lbl_test2.Text = "<B>Remarks</B> : " & remark & ""
        ' lbl_test2.CssClass = "lblnumerictxtbox"
        lbl_test2.Width = System.Web.UI.WebControls.Unit.Pixel(500)
        lbl_test2.Font.Bold = False
        row2.Cells(1).Controls.Add(lbl_test2)
        row2.Cells(1).ColumnSpan = 10
        row2.Cells(1).HorizontalAlign = HorizontalAlign.Left
        Me.dtg_PrevInvoice.Controls(0).Controls.Add(row2)


    End Sub

    Sub AddRowtotal(ByVal intCell As Integer) 'add total row 
        Dim intL, intColToRemain As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim row2 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)

        For intL = 0 To Me.dtg_PrevInvoice.Columns.Count - 1
            addCell(row)
        Next

        'For intL = 0 To 1
        '    row.Cells.RemoveAt(1)
        'Next
        'row.Cells(0).ColumnSpan = intCell - 1
        Dim lbl_test As New Label
        lbl_test.ID = "test"
        lbl_test.Text = "TEST "
        lbl_test.CssClass = "lblnumerictxtbox"
        lbl_test.Width = System.Web.UI.WebControls.Unit.Pixel(80)
        lbl_test.Font.Bold = True
        row.Cells(4).Text = "Sub Total"
        row.Cells(4).Wrap = False

        row.Cells(4).HorizontalAlign = HorizontalAlign.Right
        row.Cells(4).Font.Bold = True

        '//added By Moo, to prevent error page displayed when Invoice_detail hv no data
        If ViewState("total") Is Nothing Then
            ViewState("total") = 0
        End If
        If ViewState("gst") Is Nothing Then
            ViewState("gst") = 0
        End If

        row.Cells(5).Text = Format(ViewState("total"), "#,##0.00")
        row.Cells(5).HorizontalAlign = HorizontalAlign.Right
        ' row.Cells(7).Font.Bold = True

        row.Cells(6).Text = Format(ViewState("gst"), "#,##0.00")
        row.Cells(6).HorizontalAlign = HorizontalAlign.Right
        'row.Cells(8).ColumnSpan = 10

        Me.dtg_PrevInvoice.Controls(0).Controls.Add(row)


        For intL = 0 To Me.dtg_PrevInvoice.Columns.Count - 1
            addCell(row2)
        Next

        row2.Cells(4).Text = "Total (w/Tax)"
        row2.Cells(4).HorizontalAlign = HorizontalAlign.Right
        row2.Cells(4).Font.Bold = True
        row2.Cells(4).Wrap = False

        row2.Cells(5).Text = Format(CDbl(Format(ViewState("total"), "#0.00")) + CDbl(Format(ViewState("gst"), "#0.00")), "#,##0.00")
        row2.Cells(5).HorizontalAlign = HorizontalAlign.Right

        Me.dtg_PrevInvoice.Controls(0).Controls.Add(row2)

    End Sub
    Sub AddRowmoney(ByVal intCell As Integer, ByVal money As String) ' add curr

        Dim intL, intColToRemain As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)

        For intL = 0 To Me.dtg_PrevInvoice.Columns.Count - 1
            addCell(row)
        Next

        'For intL = 0 To 1
        '    row.Cells.RemoveAt(1)
        'Next
        'row.Cells(0).ColumnSpan = intCell - 1
        row.Cells(4).Text = "(" & money & ")"
        row.Cells(4).HorizontalAlign = HorizontalAlign.Right
        'row.Cells(6).ColumnSpan = 10

        row.Cells(5).Text = "(" & money & ")"
        row.Cells(5).HorizontalAlign = HorizontalAlign.Right
        ' row.Cells(7).ColumnSpan = 10

        row.Cells(6).Text = "(" & money & ")"
        row.Cells(6).HorizontalAlign = HorizontalAlign.Right
        'row.Cells(8).ColumnSpan = 10

        Me.dtg_PrevInvoice.Controls(0).Controls.Add(row)
    End Sub

    Sub DrawLine()
        'adding totals row
        Dim intL, intColToRemain As Integer
        Dim row2 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)

        Dim intTotalCol As Integer
        Dim count As Integer
        Dim strtext As String

        For intL = 0 To dtg_PrevInvoice.Columns.Count - 1
            addCell(row2)
        Next

        For intL = 1 To dtg_PrevInvoice.Columns.Count - 1
            row2.Cells.RemoveAt(1)
        Next
        ' row.Cells(0).ColumnSpan = intCell - 1

        Dim lbl_test2 As New Label
        lbl_test2.ID = "lblHeaderLine"
        lbl_test2.Text = "<HR>"

        'lbl_test2.CssClass = "txtbox"
        'lbl_test2.Width = System.Web.UI.WebControls.Unit.Pixel(500)
        lbl_test2.Font.Bold = True
        row2.Cells(0).ColumnSpan = 12
        row2.Cells(0).Controls.Add(lbl_test2)
        row2.Cells(0).HorizontalAlign = HorizontalAlign.Left
        Me.dtg_PrevInvoice.Controls(0).Controls.Add(row2)

    End Sub
    Sub addCell(ByRef row As DataGridItem)
        Dim cell As TableCell = New TableCell
        row.Cells.Add(cell)

    End Sub


End Class
