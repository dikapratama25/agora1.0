Imports AgoraLegacy
Imports eProcure.Component
Imports System.IO

Public Class RFQReport1
    Inherits AgoraLegacy.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Image2 As System.Web.UI.WebControls.Image
    Protected WithEvents lblBusRegNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblRemarks As System.Web.UI.WebControls.Label
    Protected WithEvents lblRFQNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblExpDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblPayTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lblPayMethod As System.Web.UI.WebControls.Label
    Protected WithEvents lblShipTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lblShipMode1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblBComName As System.Web.UI.WebControls.Label
    Protected WithEvents lblBAddr As System.Web.UI.WebControls.Label
    Protected WithEvents lblContact As System.Web.UI.WebControls.Label
    Protected WithEvents lblConNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblEmail As System.Web.UI.WebControls.Label
    Protected WithEvents dtg_RFQReport As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblReqQuoDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblSAddrTo As System.Web.UI.WebControls.Label
    Protected WithEvents lblSCoyName As System.Web.UI.WebControls.Label
    Protected WithEvents lblVEmail As System.Web.UI.WebControls.Label
    Protected WithEvents lblVTel As System.Web.UI.WebControls.Label
    Protected WithEvents lblVBusRegNo As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim blnDrawLine As Boolean = False
    Dim i As Integer


    Public Enum PrevRfqEnum
        No = 0
        Desc = 1
        UOM = 2
        Quantity = 3
        Time = 4
        Warranty = 5
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
            objFile.getFilePath(EnumUploadFrom.FrontOff, EnumUploadType.PDFDownload, strFilePath, strMovePath, False, "RFQ")
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

    '2 query string => RFQ_ID 
    '               =>vcom_id
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        'SetGridProperty(dtg_RFQReport)
        If Not IsPostBack Then
            If Request.QueryString("freeze") = "0" Then
                blnFreeze = False
            Else
                blnFreeze = True
                Freeze(Session.SessionID & "_RFQ.htm")
            End If

            displayRFQReport()
            Bindgrid()
        End If


    End Sub

    Private Function displayRFQReport()

        Dim objval As New RFQ_User
        Dim objrfq As New RFQ
        Dim rfq_id As String = Me.Request(Trim("RFQ_ID"))
        Dim objval1 As New RFQ_User

        Dim dsVAddr As DataSet

        Dim state As String
        Dim country As String
        Dim strVenAddr As String
        Dim strComAddr As String

        Dim dtMstr As New DataTable


        objrfq.read_rfqMstr(objval1, "", rfq_id, "")


        Me.lblRFQNo.Text = objval1.RFQ_Num
        Me.lblDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, objval1.create_on)
        Me.lblExpDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, objval1.exp_date)
        Me.lblReqQuoDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, objval1.RFQ_Req_date)
        Dim pay_term As String = objrfq.get_codemstr(objval1.pay_term, "PT")
        Dim pay_type As String = objrfq.get_codemstr(objval1.pay_type, "PM")
        Dim ship_mode As String = objrfq.get_codemstr(objval1.ship_mode, "SM")
        Dim ship_term As String = objrfq.get_codemstr(objval1.ship_term, "ST")
        Me.lblPayTerm.Text = pay_term
        Me.lblPayMethod.Text = pay_type
        Me.lblShipMode1.Text = ship_mode
        Me.lblShipTerm.Text = ship_term
        Me.lblRemarks.Text = objval1.remark




        dsVAddr = objrfq.rfq_COMNAME(rfq_id, Request(Trim("vcom_id")))



        If Not dsVAddr Is Nothing Then
            dtMstr = dsVAddr.Tables(0)

            If dtMstr.Rows.Count > 0 Then

                Dim objval2 As New RFQ_User
                objval2.V_com_ID = dtMstr.Rows(0)("RIV_S_Coy_ID")
                objrfq.rfq_COMMSTR(objval2)
                Me.lblVBusRegNo.Text = objval2.REG_NO

                state = objrfq.get_codemstr(dtMstr.Rows(0)("RIV_S_State"), "S")
                country = objrfq.get_codemstr(dtMstr.Rows(0)("RIV_S_Country"), "CT")

                strVenAddr = Common.parseNull(dtMstr.Rows(0)("RIV_S_Addr_Line1"))
                If Not IsDBNull(dtMstr.Rows(0)("RIV_S_Addr_Line2")) AndAlso dtMstr.Rows(0)("RIV_S_Addr_Line2") <> "" Then
                    strVenAddr = strVenAddr & "<BR>" & dtMstr.Rows(0)("RIV_S_Addr_Line2")
                End If

                If Not IsDBNull(dtMstr.Rows(0)("RIV_S_Addr_Line3")) AndAlso dtMstr.Rows(0)("RIV_S_Addr_Line3") <> "" Then
                    strVenAddr = strVenAddr & "<BR>" & dtMstr.Rows(0)("RIV_S_Addr_Line3")
                End If


                If Not IsDBNull(dtMstr.Rows(0)("RIV_S_PostCode")) AndAlso dtMstr.Rows(0)("RIV_S_PostCode") <> "" Then
                    strVenAddr = strVenAddr & "<BR>" & dtMstr.Rows(0)("RIV_S_PostCode")
                End If
                If Not IsDBNull(dtMstr.Rows(0)("RIV_S_City")) AndAlso dtMstr.Rows(0)("RIV_S_City") <> "" Then
                    strVenAddr = strVenAddr & "&nbsp;" & dtMstr.Rows(0)("RIV_S_City")
                End If

                If Not IsDBNull(state) AndAlso state <> "" Then
                    strVenAddr = strVenAddr & "<BR>" & state
                End If
                If Not IsDBNull(country) AndAlso country <> "" Then
                    strVenAddr = strVenAddr & "&nbsp;" & country
                End If
            End If
        End If

        Me.lblSCoyName.Text = dtMstr.Rows(0)("RIV_S_Coy_Name")
        Me.lblVTel.Text = dtMstr.Rows(0)("RIV_S_Phone")
        Me.lblVEmail.Text = dtMstr.Rows(0)("RIV_S_Email")



        Me.lblSAddrTo.Text = strVenAddr

        objval.V_com_ID = objval1.bcom_id
        strComAddr = objrfq.rfq_COMMSTR(objval)
        state = objrfq.get_codemstr(objval.state, "S")
        country = objrfq.get_codemstr(objval.country, "CT")
        Dim buyeradds As String


        If objval.addsline1 <> "" Then
            buyeradds = objval.addsline1
        End If

        If objval.addsline2 <> "" Then
            If buyeradds = "" Then
                buyeradds = " " & objval.addsline2 & ""
            Else
                buyeradds = buyeradds & "<br>" & objval.addsline2
            End If

        End If
        If objval.addsline3 <> "" Then
            If buyeradds = "" Then
                buyeradds = " " & objval.addsline3 & ""
            Else
                buyeradds = buyeradds & "<br>" & objval.addsline3
            End If

        End If

        If objval.postcode <> "" Or objval.city <> "" Then
            If buyeradds = "" Then
                buyeradds = "" & objval.postcode & " " & objval.city
            Else
                buyeradds = buyeradds & "<br>" & objval.postcode & " " & objval.city
            End If
        End If

        If state <> "" Or country <> "" Then
            If buyeradds = "" Then
                buyeradds = "" & state & " " & country
            Else
                buyeradds = buyeradds & "<br>" & state & " " & country
            End If

        End If


        'If country <> "" Then
        '    If buyeradds = "" Then
        '        buyeradds = "" & country & ""
        '    Else
        '        buyeradds = buyeradds & "<br>" & country
        '    End If
        'End If

        Me.lblBComName.Text = objval.vendor_name
        Me.lblBAddr.Text = buyeradds
        Me.lblBusRegNo.Text = objval.REG_NO
        Me.lblContact.Text = objval1.con_person
        Me.lblConNo.Text = objval1.phone ' objval.vendor_Con_num
        Me.lblEmail.Text = objval1.email ' objval.vendor_email

        Dim objFile As New FileManagement
        Dim strImgSrc As String
        strImgSrc = objFile.getCoyLogo(EnumUploadFrom.FrontOff, objval1.bcom_id)
        If strImgSrc <> "" Then
            Image2.Visible = True

            Dim bitmapImg As System.Drawing.Image = System.Drawing.Image.FromFile(Server.MapPath(strImgSrc))
            If bitmapImg.Width < Image2.Width.Value Then
                Image2.Width = System.Web.UI.WebControls.Unit.Pixel(bitmapImg.Width)
            End If

            If blnFreeze Then
                Image2.ImageUrl = "file:///" & Server.MapPath(strImgSrc)
            Else
                Image2.ImageUrl = strImgSrc
            End If
        Else
            Image2.Visible = False
        End If

        objFile = Nothing

    End Function
    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False)

        Dim rfq_id As String = Me.Request(Trim("RFQ_ID"))
        'Dim vs As String = viewState("RFQ_ID")
        Dim objrfq As New RFQ
        Dim objval As New RFQ_User
        Dim ds As System.Data.DataSet
        ds = objrfq.get_RFQDetail(rfq_id)
        Dim dvViewSample As DataView
        ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count
        dvViewSample = ds.Tables(0).DefaultView
        intPageRecordCnt = ds.Tables(0).Rows.Count
        If pSorted Then
            dvViewSample.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"
        End If

        If ViewState("action") = "del" Then
            If dtg_RFQReport.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtg_RFQReport.PageSize = 0 Then
                dtg_RFQReport.CurrentPageIndex = dtg_RFQReport.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If
        i = 0
        dtg_RFQReport.DataSource = dvViewSample
        dtg_RFQReport.DataBind()
        objrfq = Nothing


    End Function


    Sub addCell(ByRef row As DataGridItem)
        Dim cell As TableCell = New TableCell
        row.Cells.Add(cell)
    End Sub

    Sub DrawLine()
        'adding totals row
        Dim intL, intColToRemain As Integer
        Dim row2 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)

        Dim intTotalCol As Integer
        Dim count As Integer
        Dim strtext As String

        For intL = 0 To dtg_RFQReport.Columns.Count - 1
            addCell(row2)
        Next

        For intL = 1 To dtg_RFQReport.Columns.Count - 1
            row2.Cells.RemoveAt(1)
        Next

        Dim lbl_test2 As New Label
        lbl_test2.ID = "lblHeaderLine"
        lbl_test2.Text = "<HR>"

        lbl_test2.Font.Bold = True
        row2.Cells(0).ColumnSpan = 6
        row2.Cells(0).Controls.Add(lbl_test2)
        row2.Cells(0).HorizontalAlign = HorizontalAlign.Left
        Me.dtg_RFQReport.Controls(0).Controls.Add(row2)

    End Sub

    Private Sub dtg_RFQReport_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_RFQReport.ItemDataBound
        If i = 0 Then
            DrawLine()
        End If
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            If dv("RD_Delivery_Lead_Time") = 0 Then
                e.Item.Cells(PrevRfqEnum.Time).Text = "Ex-Stock"

            End If

            e.Item.Cells(PrevRfqEnum.No).Text = e.Item.ItemIndex + 1 'dv("RD_RFQ_Line")
            If dv("RD_Warranty_Terms") = 0 Then
                e.Item.Cells(PrevRfqEnum.Warranty).Text = "0"
            End If
        End If
        'End If
        i = i + 1
    End Sub
End Class
