Imports AgoraLegacy
Imports eProcure.Component
Imports System.IO

Public Class QuoReport
    Inherits AgoraLegacy.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Image1 As System.Web.UI.WebControls.Image
    Protected WithEvents lblQuoNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblBCoyName As System.Web.UI.WebControls.Label
    Protected WithEvents lblBillTo As System.Web.UI.WebControls.Label
    Protected WithEvents lblDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblPayTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lblPayMethod As System.Web.UI.WebControls.Label
    Protected WithEvents dg_viewitem As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lbl_bcontact As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_bconnum As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_bemail As System.Web.UI.WebControls.Label
    Protected WithEvents lblvalidtill As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_sm As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_st As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_bRegNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblComName As System.Web.UI.WebControls.Label
    Protected WithEvents lblBusRegNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblAddr As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Contact As System.Web.UI.WebControls.Label
    Protected WithEvents lblEmail As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Vremark As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Dim objrfq As New RFQ
    Dim objval As New RFQ_User
    Dim strCurrCode As String

    Public Enum ItemEnum
        NO = 0
        ProName = 1
        ProDesc = 2
        UOM = 3
        QTY = 4
        QtyTolen = 5
        Price = 6
        Amt = 7
        Tax = 8
        MOQ = 9
        MPQ = 10
        Time = 11
        Warranty = 12
    End Enum

    Public Enum cur
        Unitprice = 6
        Amt = 7
        tax = 8
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

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If (blnFreeze) Then
            Dim htmlFile As New AppHtml
            ' let Asp.net render the output, catch it in the file creator
            MyBase.Render(htmlFile.RenderHere)
            htmlFile.WriteHTMLFile(Server.MapPath(NewUrl))

            Dim objFile As New FileManagement
            Dim strFilePath, strMovePath As String
            objFile.getFilePath(EnumUploadFrom.FrontOff, EnumUploadType.PDFDownload, strFilePath, strMovePath, False, "QUO")
            objFile = Nothing
            Dim strFileName As String
            strFileName = strFilePath & Session("InvFileName")
            If Not File.Exists(strFileName) Then
                File.Copy(Server.MapPath(NewUrl), strFileName)
            Else
                htmlFile.WriteHTMLFile(Server.MapPath(NewUrl), strFileName)
            End If
            ' ai chu add on 21/09/2005
            ' to remove generated file in ExtraFunc folder or respective module folder
            File.Delete(Server.MapPath(NewUrl))
        Else
            MyBase.Render(writer)
        End If
    End Sub

    Protected Sub Freeze(ByVal strtoUrl As String)
        blnFreeze = True
        NewUrl = strtoUrl
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)
        If Not Page.IsPostBack Then
            blnFreeze = True
            Freeze(Session.SessionID & "_QUO.htm")
            getadds()
            Bindgrid()
            AddRow(5, "Sub Total", True)
            AddRow(5, "Total (W/Tax)", False) 'Total(W/Tax) is use to dec ID and Viewstate
        End If
        intPageRecordCnt = viewstate("intPageRecordCnt")
    End Sub

    Sub getadds() 'vendor
        Dim objrfq1 As New RFQ
        Dim state As String
        Dim country As String
        Dim vcomid3 As String = Request(Trim("vcomid"))
        Dim objval As New RFQ_User
        objrfq.get_qoute1(objval, Me.Request(Trim("RFQ_ID")), vcomid3)
        state = objrfq.get_codemstr(objval.state, "S")
        country = objrfq.get_codemstr(objval.country, "CT")

        Dim stradds As String

        If objval.addsline1 <> "" Then
            stradds = objval.addsline1
        End If

        If objval.addsline2 <> "" Then
            If stradds = "" Then
                stradds = " " & objval.addsline2 & ""
            Else
                stradds = stradds & "<br> " & objval.addsline2 & ""
            End If
        End If

        If objval.addsline3 <> "" Then
            If stradds = "" Then
                stradds = " " & objval.addsline3 & ""
            Else
                stradds = stradds & "<br> " & objval.addsline3 & ""
            End If
        End If

        If objval.postcode <> "" Or objval.city <> "" Then
            If stradds = "" Then
                stradds = "" & objval.postcode & " " & objval.city
            Else
                stradds = stradds & "<br>" & objval.postcode & " " & objval.city
            End If

        End If

        If state <> "" Then
            If stradds = "" Then
                stradds = "" & state & ""
            Else
                stradds = stradds & "<br>" & state & ""
            End If
        End If

        If country <> "" Then
            If stradds = "" Then
                stradds = "" & country & ""
            Else
                stradds = stradds & ", " & country & ""
            End If

        End If
        Dim objval1 As New RFQ_User


        lblBusRegNo.Text = objval.REG_NO
        strCurrCode = objval.cur_code
        'Me.lbl_bconnum.Text = objval1.phone
        'Me.lbl_bcontact.Text = objval1.con_person
        Me.lblQuoNo.Text = objval.quo_num ' Me.Request(Trim("RFQ_ID"))
        Me.lblComName.Text = objval.V_Com_Name
        Me.lbl_Vremark.Text = objval.remark
        Me.lblAddr.Text = stradds
        Me.lbl_Contact.Text = objval.phone
        Me.lblEmail.Text = objval.email
        Me.lblDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, objval.create_on)
        Me.lblvalidtill.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, objval.validaty)
        Dim objven As New RFQ_User
        objrfq.rfq_COMMSTR(objven)

        If lblEmail.Text = "" Then
            lblEmail.Text = objven.vendor_email
        End If
        If lbl_Contact.Text = "" Then
            lbl_Contact.Text = objven.vendor_Con_num
        End If
        Dim objbuyer As New RFQ_User


        objrfq.read_rfqMstr(objval1, "", Me.Request(Trim("RFQ_ID")), "")
        objbuyer.V_com_ID = objval1.bcom_id
        objrfq.rfq_COMMSTR(objbuyer)
        Me.lbl_bcontact.Text = objval1.con_person
        Me.lbl_bconnum.Text = objval1.phone
        Me.lbl_bemail.Text = objval1.email
        'If lbl_bcontact.Text = "" Then
        '    lbl_bcontact.Text = objbuyer.vendor_person
        'End If
        If lbl_bconnum.Text = "" Then
            lbl_bconnum.Text = objbuyer.vendor_Con_num
        End If
        If lbl_bemail.Text = "" Then
            lbl_bemail.Text = objbuyer.vendor_email
        End If
        Me.lblPayTerm.Text = objrfq.get_codemstr(objval1.pay_term, "PT")
        Me.lbl_sm.Text = objrfq.get_codemstr(objval1.ship_mode, "SM")
        Me.lbl_st.Text = objrfq.get_codemstr(objval1.ship_term, "ST")
        Me.lblPayMethod.Text = objrfq.get_codemstr(objval1.pay_type, "PM")
        get_badds(objval1.bcom_id)

        Dim objFile As New FileManagement
        Dim strImgSrc As String
        strImgSrc = objFile.getCoyLogo(EnumUploadFrom.FrontOff, objval1.bcom_id)
        If strImgSrc <> "" Then
            Image1.Visible = True

            Dim bitmapImg As System.Drawing.Image = System.Drawing.Image.FromFile(Server.MapPath(strImgSrc))
            If bitmapImg.Width < Image1.Width.Value Then
                Image1.Width = System.Web.UI.WebControls.Unit.Pixel(bitmapImg.Width)
            End If

            If blnFreeze Then
                Image1.ImageUrl = "file:///" & Server.MapPath(strImgSrc)
            Else
                Image1.ImageUrl = strImgSrc
            End If
        Else
            Image1.Visible = False
        End If

        objFile = Nothing
    End Sub
    Sub get_badds(ByVal bcom_id As String) 'buyer
        Dim objval As New RFQ_User
        objval.V_com_ID = bcom_id
        Dim state As String
        Dim country As String
        objrfq.rfq_COMMSTR(objval)
        lblBCoyName.Text = objval.vendor_name
        Dim stradds As String
        state = objrfq.get_codemstr(objval.state, "S")
        country = objrfq.get_codemstr(objval.country, "CT")
        If objval.addsline1 <> "" Then
            stradds = objval.addsline1
        End If

        If objval.addsline2 <> "" Then
            If stradds = "" Then
                stradds = " " & objval.addsline2 & ""
            Else
                stradds = stradds & "<br> " & objval.addsline2 & ""
            End If
        End If

        If objval.addsline3 <> "" Then
            If stradds = "" Then
                stradds = " " & objval.addsline3 & ""
            Else
                stradds = stradds & "<br> " & objval.addsline3 & ""
            End If
        End If

        If objval.postcode <> "" Or objval.city <> "" Then
            If stradds = "" Then
                stradds = "" & objval.postcode & " " & objval.city
            Else
                stradds = stradds & "<br>" & objval.postcode & " " & objval.city
            End If

        End If

        If state <> "" Then
            If stradds = "" Then
                stradds = "" & state & ""
            Else
                stradds = stradds & "<br>" & state & ""
            End If
        End If

        If country <> "" Then
            If stradds = "" Then
                stradds = "" & country & ""
            Else
                stradds = stradds & ", " & country & ""
            End If

        End If
        lblBillTo.Text = stradds
        Me.lbl_bRegNo.Text = objval.REG_NO
        
    End Sub
    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim objview As New RFQ
        Dim ds As New DataSet
        Dim RFQ_ID As String
        Dim vcomid3 As String = Request(Trim("vcomid"))
        RFQ_ID = Request(Trim("RFQ_ID"))
        ds = objview.get_quotation2(RFQ_ID, vcomid3)

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        intPageRecordCnt = ds.Tables(0).Rows.Count

        If viewstate("action") = "del" Then
            If dg_viewitem.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dg_viewitem.PageSize = 0 Then
                dg_viewitem.CurrentPageIndex = dg_viewitem.CurrentPageIndex - 1
                viewstate("action") = ""
            End If
        End If

        dg_viewitem.DataSource = dvViewSample
        dg_viewitem.DataBind()
    End Function

    Sub AddRowmoney(ByVal money As String)
        Dim intL, intColToRemain As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)

        For intL = 0 To Me.dg_viewitem.Columns.Count - 1
            addCell(row)
        Next

        row.Cells(cur.Unitprice).Text = "(" & money & ")"
        row.Cells(cur.Unitprice).HorizontalAlign = HorizontalAlign.Right
        row.Cells(cur.Unitprice).Wrap = False
        row.Cells(cur.Amt).Text = "(" & money & ")"
        row.Cells(cur.Amt).HorizontalAlign = HorizontalAlign.Right
        row.Cells(cur.Amt).Wrap = False
        row.Cells(cur.tax).Text = "(" & money & ")"
        row.Cells(cur.tax).HorizontalAlign = HorizontalAlign.Right
        row.Cells(cur.tax).Wrap = False


        Me.dg_viewitem.Controls(0).Controls.Add(row)
    End Sub

    Sub AddRemark(ByVal remark As String)

        Dim intL, intColToRemain As Integer
        Dim row2 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)

        Dim intTotalCol As Integer
        Dim count As Integer
        Dim strtext As String

        For intL = 0 To dg_viewitem.Columns.Count - 1
            addCell(row2)
        Next

        For intL = 2 To dg_viewitem.Columns.Count - 1
            row2.Cells.RemoveAt(1)
        Next

        Dim lbl_bremark As New Label
        lbl_bremark.ID = "test2"
        lbl_bremark.Text = "<b>Remarks </b>:" & remark
        lbl_bremark.CssClass = "txtbox"
        row2.Cells(1).ColumnSpan = dg_viewitem.Columns.Count - 2
        row2.Cells(1).Controls.Add(lbl_bremark)
        row2.Cells(1).HorizontalAlign = HorizontalAlign.Left
        Me.dg_viewitem.Controls(0).Controls.Add(row2)

    End Sub

    Private Sub dg_viewitem_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg_viewitem.ItemDataBound
        If viewstate("count") = "" Then


            AddRowmoney(strCurrCode)
            DrawLine()
            viewstate("count") = "1"
        End If
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lbl_Delivery As Label
            viewstate("NO") = viewstate("NO") + 1
            Dim NO As Integer = viewstate("NO")
            e.Item.Cells(0).Text = NO

            Dim lbl_moq As Label
            lbl_moq = e.Item.FindControl("lbl_moq")
            lbl_moq.Text = Common.parseNull(dv("RRD_Min_Order_Qty"))

            Dim lbl_mpq As Label
            lbl_mpq = e.Item.FindControl("lbl_mpq")
            lbl_mpq.Text = Common.parseNull(dv("RRD_Min_Pack_Qty"))


            AddRemark(Common.parseNull(dv("RRD_Remarks")))
            lbl_Delivery = e.Item.FindControl("lbl_Delivery")
            If dv("RRD_Delivery_Lead_Time") = "0" Then
                lbl_Delivery.Text = "Ex-Stock"
            Else
                lbl_Delivery.Text = Common.parseNull(dv("RRD_Delivery_Lead_Time"))
            End If

            Dim lbl_warranty As Label
            lbl_warranty = e.Item.FindControl("lbl_warranty")

            Dim lbl_unit_price As Label
            lbl_unit_price = e.Item.FindControl("lbl_unit_price")
            Dim lbl_price As Label
            lbl_price = e.Item.FindControl("lbl_price")
            Dim a As Double = dv("RRD_Unit_Price") * dv("RRD_Quantity")
            If a = 0 Then
                lbl_price.Text = "No Quote"
                lbl_unit_price.Text = "No Quote"
            Else
                lbl_price.Text = Format(a, "###,###,##0.0000")
                lbl_unit_price.Text = Format(dv("RRD_Unit_Price"), "###,###,##0.0000")
            End If


            Dim lbl_tax As Label
            lbl_tax = e.Item.FindControl("lbl_tax")
            Dim i As Integer = objrfq.get_gst(dv("RRD_GST_Code"))
            Dim j As Double = dv("RRD_Unit_Price") * dv("RRD_Quantity") * i / 100
            lbl_tax.Text = Format(j, "###,###,##0.0000")
            e.Item.Cells(ItemEnum.Price).HorizontalAlign = HorizontalAlign.Right
            viewstate("tax") = viewstate("tax") + j
            viewstate("Sub Total") = viewstate("Sub Total") + a
            viewstate("Total (W/Tax)") = viewstate("Total (W/Tax)") + a + j

            If dv("RRD_Warranty_Terms") = "0" Then
                lbl_warranty.Text = "0"
            Else
                lbl_warranty.Text = Common.parseNull(dv("RRD_Warranty_Terms"))
            End If
        End If

    End Sub
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CheckButtonAccess()
    End Sub
    Sub AddRow(ByVal intCell As Integer, ByVal strLabel As String, ByVal check As Boolean)
        Dim intL, intColToRemain As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim intTotalCol As Integer
        For intL = 0 To 9
            addCell(row)
        Next
        Dim txtTotal As New TextBox
        txtTotal.ID = strLabel
        txtTotal.Text = Format(viewstate(strLabel), "#,##0.00")
        txtTotal.CssClass = "lblnumerictxtbox"
        txtTotal.ReadOnly = True
        txtTotal.Width = System.Web.UI.WebControls.Unit.Percentage(100%)
        row.Cells(cur.Amt).Controls.Add(txtTotal)
        row.Cells(cur.Unitprice).Text = strLabel
        row.Cells(cur.Unitprice).Wrap = False
        row.Cells(cur.Amt).HorizontalAlign = HorizontalAlign.Right
        row.Cells(cur.Unitprice).Width = System.Web.UI.WebControls.Unit.Percentage(10%)
        row.Cells(cur.Unitprice).Font.Bold = True
        row.Cells(cur.Unitprice).BorderWidth = System.Web.UI.WebControls.Unit.Pixel(0)
        row.Cells(cur.Amt).BorderWidth = System.Web.UI.WebControls.Unit.Pixel(0)

        If check = True Then
            Dim txtTax As New TextBox

            txtTax.ID = "Tax"
            txtTax.CssClass = "lblnumerictxtbox"
            txtTax.ReadOnly = True
            txtTax.Width = System.Web.UI.WebControls.Unit.Percentage(100%)
            row.Cells(cur.tax).HorizontalAlign = HorizontalAlign.Right
            txtTax.Text = Format(viewstate("tax"), "#,##0.00")
            row.Cells(cur.tax).Controls.Add(txtTax)
        End If

        Me.dg_viewitem.Controls(0).Controls.Add(row)

    End Sub

    Sub addCell(ByRef row As DataGridItem)
        Dim cell As TableCell = New TableCell
        row.Cells.Add(cell)
    End Sub

    Sub DrawLine()

        Dim intL, intColToRemain As Integer
        Dim row2 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)

        Dim intTotalCol As Integer
        Dim count As Integer
        Dim strtext As String

        For intL = 0 To Me.dg_viewitem.Columns.Count - 1
            addCell(row2)
        Next

        For intL = 1 To dg_viewitem.Columns.Count - 1
            row2.Cells.RemoveAt(1)
        Next

        Dim lbl_test2 As New Label
        lbl_test2.ID = "lblHeaderLine"
        lbl_test2.Text = "<HR>"
        lbl_test2.Font.Bold = True
        row2.Cells(0).ColumnSpan = 13
        row2.Cells(0).Controls.Add(lbl_test2)
        row2.Cells(0).HorizontalAlign = HorizontalAlign.Left
        Me.dg_viewitem.Controls(0).Controls.Add(row2)

    End Sub

    Private Sub dg_viewitem_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dg_viewitem.SelectedIndexChanged

    End Sub
End Class
