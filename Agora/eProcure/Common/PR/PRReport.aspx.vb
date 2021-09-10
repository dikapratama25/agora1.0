Imports System.IO
Imports AgoraLegacy
Imports eProcure.Component

Public Class PRReport
    Inherits AgoraLegacy.AppBaseClass
    Dim intRow As Integer
    Dim dsAllInfo As DataSet
    Dim i, jLoop, iLoop As Integer
    Dim dsItem As New DataSet
    Dim dtAddr As DataTable
    Dim dtCust1, dtCust2 As DataTable
    Dim objGlobal As New AppGlobals
    Protected WithEvents lblBillTo As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents lblBillCompName As System.Web.UI.WebControls.Label
    Dim strDelCode As String = ""
    Dim strGSTBy As String
    Dim dblNoTaxTotal, dblTaxTotal, dblTotalGst As Double
    Dim intGSTcnt, intNoGSTcnt, intTotItem As Integer
    Dim blnPrintRemark As Boolean
    Dim blnPrintCustField As Boolean
    Dim value1(100) As String
    Dim value2(100) As String
    Protected WithEvents lblAttn As System.Web.UI.WebControls.Label
    Protected WithEvents lblAppBy As System.Web.UI.WebControls.Label
    Protected WithEvents lblHeaderRemarks As System.Web.UI.WebControls.Label
    Protected WithEvents lblRequestor As System.Web.UI.WebControls.Label
    Protected WithEvents lblTaxNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblReqEmail As System.Web.UI.WebControls.Label
    Protected WithEvents lblReqTel As System.Web.UI.WebControls.Label
    Protected WithEvents lblVendorReg As System.Web.UI.WebControls.Label
    Protected WithEvents lblVendorEmail As System.Web.UI.WebControls.Label
    Protected WithEvents lblVendorTel As System.Web.UI.WebControls.Label
    Dim objPR As New PurchaseReq2
    Dim strCurrCode As String
  
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Image1 As System.Web.UI.WebControls.Image
    Protected WithEvents lblComName As System.Web.UI.WebControls.Label
    Protected WithEvents lblAddr As System.Web.UI.WebControls.Label
    Protected WithEvents lblPONo As System.Web.UI.WebControls.Label
    Protected WithEvents lblDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblPayTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lblPayMethod As System.Web.UI.WebControls.Label
    Protected WithEvents lblShipTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lblShipMode As System.Web.UI.WebControls.Label
    Protected WithEvents lblFreighTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lblShipVia As System.Web.UI.WebControls.Label
    Protected WithEvents lblBuyer As System.Web.UI.WebControls.Label
    Protected WithEvents lblVendorName As System.Web.UI.WebControls.Label
    Protected WithEvents lblVAddr As System.Web.UI.WebControls.Label
    Protected WithEvents dtgShopping As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblCurrency As System.Web.UI.WebControls.Label
    Protected WithEvents lblRate As System.Web.UI.WebControls.Label
    Protected WithEvents lblPrStatus As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Public Enum EnumPR
        eSNo
        eDesc
        eMOQ
        eMPQ
        eQTY
        euom2
        eUOM
        eCOST
        eAmt
        eTax
        eDay
        eWaran
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
            objFile.getFilePath(EnumUploadFrom.FrontOff, EnumUploadType.PDFDownload, strFilePath, strMovePath, False, "PR")
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

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)

        If Not IsPostBack Then
            blnFreeze = True
            Freeze(Session.SessionID & "_PR.htm")
            Bindgrid()

            Dim objFile As New FileManagement
            Dim strImgSrc As String

            strImgSrc = objFile.getCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyId"))
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
        End If
    End Sub

    Protected Sub Freeze(ByVal strtoUrl As String)
        blnFreeze = True
        NewUrl = strtoUrl
    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String

        Dim dsItem2 As New DataSet
        Dim dsItem3 As New DataSet
        Dim objShopping As New ShoppingCart
        Dim dvViewSample As DataView
        Dim strBillAddr, venAddress, BuyerAddress As String
        Dim dvwCustomItem As DataView
        Dim objComp As New Companies
        Dim objCompDetails As New Company
        objCompDetails = objComp.GetCompanyDetails(Session("CompanyId")) '// BUYER 

        Dim objUser As New Users
        Dim objUserDetails As New User

        dsItem = objPR.getPR(Request.QueryString("PRNO"), Request.QueryString("index"))
        dsItem2 = objPR.getApprFlow(Request.QueryString("index"))

        If dsItem.Tables(0).Rows.Count > 0 Then
            strCurrCode = dsItem.Tables(0).Rows(0)("PRM_CURRENCY_CODE")
            objUserDetails = objUser.GetUserDetails(dsItem.Tables(0).Rows(0)("PRM_BUYER_ID"), Session("CompanyId"))
            lblReqEmail.Text = objUserDetails.Email
            blnPrintCustField = True
            blnPrintRemark = True
            ' no need to check, all must print custom field and remark
            'If objPR.Need2PrintCField(dsItem.Tables(0).Rows(0)("PRM_PR_INDEX")) Then
            '    blnPrintCustField = True
            'Else
            '    blnPrintCustField = False
            'End If
            'If objPR.Need2PrintRemark(dsItem.Tables(0).Rows(0)("PRM_PR_INDEX")) Then
            '    blnPrintRemark = True
            'Else
            '    blnPrintRemark = False
            'End If

            strBillAddr = Common.parseNull(dsItem.Tables(0).Rows(0)("PRM_B_ADDR_LINE1"))
            If Not IsDBNull(dsItem.Tables(0).Rows(0)("PRM_B_ADDR_LINE2")) AndAlso dsItem.Tables(0).Rows(0)("PRM_B_ADDR_LINE2") <> "" Then
                strBillAddr = strBillAddr & "<BR>" & dsItem.Tables(0).Rows(0)("PRM_B_ADDR_LINE2")
            End If
            If Not IsDBNull(dsItem.Tables(0).Rows(0)("PRM_B_ADDR_LINE3")) AndAlso dsItem.Tables(0).Rows(0)("PRM_B_ADDR_LINE3") <> "" Then
                strBillAddr = strBillAddr & "<BR>" & dsItem.Tables(0).Rows(0)("PRM_B_ADDR_LINE3")
            End If
            If Not IsDBNull(dsItem.Tables(0).Rows(0)("PRM_B_POSTCODE")) Then
                strBillAddr = strBillAddr & "<BR>" & dsItem.Tables(0).Rows(0)("PRM_B_POSTCODE")
            End If
            If Not IsDBNull(dsItem.Tables(0).Rows(0)("PRM_B_CITY")) Then
                strBillAddr = strBillAddr & " " & dsItem.Tables(0).Rows(0)("PRM_B_CITY")
            End If
            If Not IsDBNull(dsItem.Tables(0).Rows(0)("STATE")) Then
                strBillAddr = strBillAddr & "<BR>" & dsItem.Tables(0).Rows(0)("STATE")
            End If
            If Not IsDBNull(dsItem.Tables(0).Rows(0)("CT")) Then
                strBillAddr = strBillAddr & " " & dsItem.Tables(0).Rows(0)("CT")
            End If
            lblBillTo.Text = strBillAddr '& "<P>"


            venAddress = Common.parseNull(dsItem.Tables(0).Rows(0)("PRM_S_ADDR_LINE1"))
            If Not IsDBNull(dsItem.Tables(0).Rows(0)("PRM_S_ADDR_LINE2")) AndAlso dsItem.Tables(0).Rows(0)("PRM_B_ADDR_LINE2") <> "" Then
                venAddress = venAddress & "<BR>" & dsItem.Tables(0).Rows(0)("PRM_S_ADDR_LINE2")
            End If
            If Not IsDBNull(dsItem.Tables(0).Rows(0)("PRM_S_ADDR_LINE3")) AndAlso dsItem.Tables(0).Rows(0)("PRM_B_ADDR_LINE3") <> "" Then
                venAddress = venAddress & "<BR>" & dsItem.Tables(0).Rows(0)("PRM_S_ADDR_LINE3")
            End If
            If Not IsDBNull(dsItem.Tables(0).Rows(0)("PRM_S_POSTCODE")) Then
                venAddress = venAddress & "<BR>" & dsItem.Tables(0).Rows(0)("PRM_S_POSTCODE")
            End If
            If Not IsDBNull(dsItem.Tables(0).Rows(0)("PRM_S_CITY")) Then
                venAddress = venAddress & " " & dsItem.Tables(0).Rows(0)("PRM_S_CITY")
            End If
            If Not IsDBNull(dsItem.Tables(0).Rows(0)("PRM_S_STATE")) Then
                venAddress = venAddress & "<BR>" & objGlobal.getCodeDesc(CodeTable.State, dsItem.Tables(0).Rows(0)("PRM_S_STATE"))
            End If
            If Not IsDBNull(dsItem.Tables(0).Rows(0)("PRM_S_COUNTRY")) Then
                venAddress = venAddress & " " & objGlobal.getCodeDesc(CodeTable.Country, dsItem.Tables(0).Rows(0)("PRM_S_COUNTRY"))
            End If
            lblVAddr.Text = venAddress '& "<P>"

            Dim objVenComp As New Companies
            Dim objVenCompDetails As New Company

            'Update here   Srihari
            If dsItem.Tables(0).Rows(0)("PRM_S_COY_ID") <> "" Then
                objVenCompDetails = objComp.GetCompanyDetails(Common.parseNull(dsItem.Tables(0).Rows(0).Item("PRM_S_COY_ID"), "")) '// vendor
            End If

            ' objVenCompDetails = objComp.GetCompanyDetails(Common.parseNull(dsItem.Tables(0).Rows(0)("PRM_S_COY_ID"))) '// vendor

            BuyerAddress = objCompDetails.Address1
            If objCompDetails.Address2 <> "" Then
                BuyerAddress = BuyerAddress & "<BR>" & objCompDetails.Address2
            End If

            If objCompDetails.Address3 <> "" Then
                BuyerAddress = BuyerAddress & "<BR>" & objCompDetails.Address3
            End If

            BuyerAddress = BuyerAddress & "<BR>" & objCompDetails.PostCode
            BuyerAddress = BuyerAddress & " " & objCompDetails.City
            BuyerAddress = BuyerAddress & "<BR>" & objGlobal.getCodeDesc(CodeTable.State, objCompDetails.State)
            BuyerAddress = BuyerAddress & " " & objGlobal.getCodeDesc(CodeTable.Country, objCompDetails.Country)
            lblAddr.Text = BuyerAddress '& "<P>"

            lblTaxNo.Text = objCompDetails.BusinessRegNo
            If dsItem.Tables(0).Rows(0)("PRM_S_COY_ID") <> "" Then 'Michelle 24/8/2007 - To cater for multiple vendors
                lblVendorReg.Text = objVenCompDetails.BusinessRegNo
            End If
            lblComName.Text = objCompDetails.CoyName
            lblBillCompName.Text = objCompDetails.CoyName
            lblPONo.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("PRM_PR_NO"))
            lblVendorName.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("PRM_S_COY_NAME"))
            lblVendorTel.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("PRM_S_PHONE"))
            lblVendorEmail.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("PRM_S_EMAIL"))
            lblPrStatus.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("STATUS_DESC"))
            lblDate.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dsItem.Tables(0).Rows(0)("PRM_CREATED_DATE"))
            lblRequestor.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("PRM_REQ_NAME"))
            lblReqTel.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("PRM_REQ_PHONE"))
            lblFreighTerm.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("PRM_FREIGHT_TERMS"))
            lblShipVia.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("PRM_SHIP_VIA"))
            lblAttn.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("PRM_S_ATTN"))
            lblCurrency.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("PRM_CURRENCY_CODE"))
            lblRate.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("PRM_EXCHANGE_RATE"))
            lblPayTerm.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("PT"))
            lblPayMethod.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("PM"))
            lblShipTerm.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("ST"))
            lblShipMode.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("SC"))
            lblAppBy.Text = Common.parseNull(dsItem2.Tables(0).Rows(0)("AO_NAME")) & "<BR>" & Common.parseNull(dsItem2.Tables(0).Rows(0)("AAO_NAME"))
            lblHeaderRemarks.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("PRM_EXTERNAL_REMARK"))
        End If

        viewstate("intPageRecordCnt") = dsItem.Tables(1).Rows.Count
        dtAddr = dsItem.Tables(1)
        dvViewSample = dsItem.Tables(1).DefaultView
        intPageRecordCnt = viewstate("intPageRecordCnt")

        intRow = 0
        dtgShopping.DataSource = dvViewSample
        dtgShopping.DataBind()
        AddRowtotal(6)
        objGlobal = Nothing
    End Function

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

        For intL = 0 To dtgShopping.Columns.Count - 1
            addCell(row2)
        Next

        For intL = 1 To dtgShopping.Columns.Count - 1
            row2.Cells.RemoveAt(1)
        Next

        Dim lbl_test2 As New Label
        lbl_test2.ID = "lblHeaderLine"
        lbl_test2.Text = "<HR>"
        lbl_test2.Font.Bold = True
        row2.Cells(0).ColumnSpan = dtgShopping.Columns.Count
        row2.Cells(0).Controls.Add(lbl_test2)
        row2.Cells(0).HorizontalAlign = HorizontalAlign.Left
        Me.dtgShopping.Controls(0).Controls.Add(row2)

    End Sub
    Sub AddRow(ByVal intCell As Integer, ByVal value1() As String, ByVal value2() As String, ByVal j As Integer, ByVal remark As String)

        Dim intL, intColToRemain As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim row2 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim intTotalCol As Integer
        Dim count As Integer
        Dim strtext As String

        If blnPrintCustField Then
            For count = 0 To j - 1
                strtext = strtext & "<B>" & value1(count) & "</B> : " & value2(count) & ""
                strtext = strtext & "<br>"
            Next
            For intL = 0 To Me.dtgShopping.Columns.Count - 1
                addCell(row)
            Next

            For intL = 1 To dtgShopping.Columns.Count - 2
                row.Cells.RemoveAt(1)
            Next

            Dim lbl_test As New Label
            lbl_test.ID = "test"
            lbl_test.Text = strtext
            lbl_test.CssClass = "txtbox"
            lbl_test.Width = System.Web.UI.WebControls.Unit.Pixel(500)
            lbl_test.Font.Bold = False

            row.Cells(1).ColumnSpan = 10
            row.Cells(1).Controls.Add(lbl_test)
            row.Cells(1).HorizontalAlign = HorizontalAlign.Left

            Me.dtgShopping.Controls(0).Controls.Add(row)
        End If

        If blnPrintRemark Then
            For intL = 0 To dtgShopping.Columns.Count - 1
                addCell(row2)
            Next

            For intL = 1 To dtgShopping.Columns.Count - 2
                row2.Cells.RemoveAt(1)
            Next

            Dim lbl_test2 As New Label
            lbl_test2.ID = "test2"
            lbl_test2.Text = "<B>Remarks</B> : " & remark & ""
            lbl_test2.CssClass = "txtbox"
            lbl_test2.Width = System.Web.UI.WebControls.Unit.Pixel(500)
            lbl_test2.Font.Bold = False

            row2.Cells(1).ColumnSpan = dtgShopping.Columns.Count
            row2.Cells(1).Controls.Add(lbl_test2)
            row2.Cells(1).HorizontalAlign = HorizontalAlign.Left

            Me.dtgShopping.Controls(0).Controls.Add(row2)
        End If
    End Sub

    Private Sub dtgShopping_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgShopping.ItemDataBound
        If i = 0 Then
            AddRowmoney(strCurrCode)
            DrawLine() '// TO DRAW line under datagrid header
            AddAddressRow(i)
            strDelCode = dtAddr.Rows(0)("PRD_D_ADDR_CODE")
        End If

        Dim intPRLine As Integer
        Dim strFieldNo, strDAddr As String
        Dim dr() As DataRow
        Dim dblAmt, dblGstAmt As Double
        Dim row2 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim j As Integer

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim objpo As New PurchaseOrder
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            e.Item.Cells(EnumPR.euom2).Text = ""

            e.Item.Cells(EnumPR.eCOST).Text = Format(dv("PRD_UNIT_COST"), "#,##0.0000")
            e.Item.Cells(EnumPR.eAmt).Text = Format(dv("PRD_ORDERED_QTY") * dv("PRD_UNIT_COST"), "#,##0.0000")
            e.Item.Cells(EnumPR.eTax).Text = Format((dv("PRD_GST") / 100) * dv("PRD_ORDERED_QTY") * dv("PRD_UNIT_COST"), "#,##0.0000")

            viewstate("gst") = viewstate("gst") + ((dv("PRD_GST") / 100) * dv("PRD_ORDERED_QTY") * dv("PRD_UNIT_COST"))
            viewstate("total") = viewstate("total") + (dv("PRD_ORDERED_QTY") * dv("PRD_UNIT_COST"))

            If blnPrintRemark Or blnPrintCustField Then
                If blnPrintCustField Then
                    objpo.get_customfield(value1, value2, dv("PRD_PR_LINE"), dsItem.Tables(0).Rows(0)("PRM_PR_INDEX"), j) 'dsItem.Tables(1).Rows(0)("PRD_PR_LINE"), dsItem.Tables(0).Rows(0)("PRM_PR_INDEX"), j) 'dv("PRD_PR_LINE"), dv("PRM_PR_INDEX"), j)
                End If
                AddRow(1, value1, value2, j, dv("PRD_REMARK"))
            End If
        End If
        i = i + 1
    End Sub

    Private Sub dtgShopping_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgShopping.ItemCreated
        If iLoop <> 0 Then
            '//display only address code if different from prev one
            If jLoop <> 0 And jLoop <= dtAddr.Rows.Count - 1 Then
                If strDelCode <> dtAddr.Rows(jLoop)("PRD_D_ADDR_CODE") Then
                    AddAddressRow(jLoop)
                    strDelCode = dtAddr.Rows(jLoop)("PRD_D_ADDR_CODE")
                End If
            End If
        End If

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            jLoop += 1
        End If
        iLoop += 1
    End Sub

    Sub AddRowmoney(ByVal money As String)
        Dim intL, intColToRemain As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)

        For intL = 0 To Me.dtgShopping.Columns.Count - 1
            addCell(row)
        Next

        row.Cells(EnumPR.eCOST).Text = "(" & money & ")"
        row.Cells(EnumPR.eCOST).HorizontalAlign = HorizontalAlign.Right
        row.Cells(EnumPR.eAmt).Text = "(" & money & ")"
        row.Cells(EnumPR.eAmt).HorizontalAlign = HorizontalAlign.Right
        row.Cells(EnumPR.eTax).Text = "(" & money & ")"
        row.Cells(EnumPR.eTax).HorizontalAlign = HorizontalAlign.Right

        Me.dtgShopping.Controls(0).Controls.Add(row)
    End Sub

    Sub AddAddressRow(ByVal intRow As Integer)
        'adding totals row
        Dim intL, intColToRemain As Integer
        Dim row2 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)

        Dim intTotalCol As Integer
        Dim count As Integer
        Dim strtext As String

        For intL = 0 To dtgShopping.Columns.Count - 1
            addCell(row2)
        Next

        For intL = 1 To dtgShopping.Columns.Count - 1
            row2.Cells.RemoveAt(1)
        Next

        Dim strTempAddr As String
        strTempAddr = dtAddr.Rows(intRow)("PRD_D_ADDR_LINE1")

        If Not IsDBNull(dtAddr.Rows(intRow)("PRD_D_ADDR_LINE2")) AndAlso dtAddr.Rows(intRow)("PRD_D_ADDR_LINE2") <> "" Then
            strTempAddr = strTempAddr & " " & dtAddr.Rows(intRow)("PRD_D_ADDR_LINE2")
        End If

        If Not IsDBNull(dtAddr.Rows(intRow)("PRD_D_ADDR_LINE3")) AndAlso dtAddr.Rows(intRow)("PRD_D_ADDR_LINE3") <> "" Then
            strTempAddr = strTempAddr & " " & dtAddr.Rows(intRow)("PRD_D_ADDR_LINE3")
        End If

        If Not IsDBNull(dtAddr.Rows(intRow)("PRD_D_POSTCODE")) AndAlso dtAddr.Rows(intRow)("PRD_D_POSTCODE") <> "" Then
            strTempAddr = strTempAddr & " " & dtAddr.Rows(intRow)("PRD_D_POSTCODE")
        End If

        If Not IsDBNull(dtAddr.Rows(intRow)("PRD_D_CITY")) AndAlso dtAddr.Rows(intRow)("PRD_D_CITY") <> "" Then
            strTempAddr = strTempAddr & " " & dtAddr.Rows(intRow)("PRD_D_CITY")
        End If

        If Not IsDBNull(dtAddr.Rows(intRow)("PRD_D_STATE")) AndAlso dtAddr.Rows(intRow)("PRD_D_STATE") <> "" Then
            strTempAddr = strTempAddr & " " & objGlobal.getCodeDesc(CodeTable.State, dtAddr.Rows(intRow)("PRD_D_STATE"))
        End If

        If Not IsDBNull(dtAddr.Rows(intRow)("PRD_D_COUNTRY")) AndAlso dtAddr.Rows(intRow)("PRD_D_COUNTRY") <> "" Then
            strTempAddr = strTempAddr & " " & objGlobal.getCodeDesc(CodeTable.Country, dtAddr.Rows(intRow)("PRD_D_COUNTRY"))
        End If

        Dim lbl_test2 As New Label
        lbl_test2.ID = "test2"
        lbl_test2.Text = "Ship To : " & dtAddr.Rows(intRow)("PRD_D_ADDR_CODE") & " - " & strTempAddr

        lbl_test2.CssClass = "txtbox"
        lbl_test2.Font.Bold = True
        row2.Cells(0).ColumnSpan = dtgShopping.Columns.Count '11
        row2.Cells(0).Controls.Add(lbl_test2)
        row2.Cells(0).HorizontalAlign = HorizontalAlign.Left
        Me.dtgShopping.Controls(0).Controls.Add(row2)

    End Sub
    Sub AddRowtotal(ByVal intCell As Integer)
        Dim intL, intColToRemain As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim row2 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)

        For intL = 0 To Me.dtgShopping.Columns.Count - 1
            addCell(row)
        Next

        Dim lbl_test As New Label
        lbl_test.ID = "test"
        lbl_test.Text = "TEST "
        lbl_test.CssClass = "lblnumerictxtbox"
        lbl_test.Width = System.Web.UI.WebControls.Unit.Pixel(80)
        lbl_test.Font.Bold = True
        row.Cells(EnumPR.eCOST).Text = "Sub Total"
        row.Cells(EnumPR.eCOST).HorizontalAlign = HorizontalAlign.Right
        row.Cells(EnumPR.eCOST).Font.Bold = True
        row.Cells(EnumPR.eCOST).Wrap = False

        row.Cells(EnumPR.eAmt).Text = Format(viewstate("total"), "#,##0.00")
        row.Cells(EnumPR.eAmt).HorizontalAlign = HorizontalAlign.Right
        row.Cells(EnumPR.eAmt).Wrap = False


        row.Cells(EnumPR.eTax).Text = Format(viewstate("gst"), "#,##0.00")
        row.Cells(EnumPR.eTax).HorizontalAlign = HorizontalAlign.Right

        Me.dtgShopping.Controls(0).Controls.Add(row)


        For intL = 0 To Me.dtgShopping.Columns.Count - 1
            addCell(row2)
        Next

        row2.Cells(EnumPR.eCOST).Text = "Total (w/Tax)"
        row2.Cells(EnumPR.eCOST).HorizontalAlign = HorizontalAlign.Right
        row2.Cells(EnumPR.eCOST).Font.Bold = True
        row2.Cells(EnumPR.eCOST).Wrap = False
        row2.Cells(EnumPR.eAmt).Text = Format(CDbl(viewstate("total")) + CDbl(viewstate("gst")), "#,##0.00")
        row2.Cells(EnumPR.eAmt).HorizontalAlign = HorizontalAlign.Right
        Me.dtgShopping.Controls(0).Controls.Add(row2)

    End Sub
End Class
