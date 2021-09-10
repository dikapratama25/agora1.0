Imports System.IO
Imports AgoraLegacy
Imports eProcure.Component

Public Class GGRNReport
    Inherits AgoraLegacy.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Image1 As System.Web.UI.WebControls.Image
    Protected WithEvents lblComName As System.Web.UI.WebControls.Label
    Protected WithEvents lblComAddr As System.Web.UI.WebControls.Label
    Protected WithEvents lblOrganiz As System.Web.UI.WebControls.Label
    Protected WithEvents lblCreateOn As System.Web.UI.WebControls.Label
    Protected WithEvents lblGRecFrom As System.Web.UI.WebControls.Label
    Protected WithEvents lblRequestor As System.Web.UI.WebControls.Label
    Protected WithEvents lblRecipiet As System.Web.UI.WebControls.Label
    Protected WithEvents lblPORef As System.Web.UI.WebControls.Label
    Protected WithEvents lblDelAddr As System.Web.UI.WebControls.Label
    Protected WithEvents lblGRecOn As System.Web.UI.WebControls.Label
    Protected WithEvents lblGRNNo As System.Web.UI.WebControls.Label
    Protected WithEvents dtgPOReport As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblVerified As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblTel As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblEmail As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tb_user As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblBusReg As System.Web.UI.WebControls.Label
    Protected WithEvents lblTelReceived As System.Web.UI.WebControls.Label
    Protected WithEvents lblEmailReceived As System.Web.UI.WebControls.Label
    Protected WithEvents lblVerifiedBy As System.Web.UI.WebControls.Label
    Protected WithEvents lblTelVerified As System.Web.UI.WebControls.Label
    Protected WithEvents lblEmailVerified As System.Web.UI.WebControls.Label
    Protected WithEvents lblAttention As System.Web.UI.WebControls.Label
    Protected WithEvents lblPONo As System.Web.UI.WebControls.Label
    Protected WithEvents lblDONo As System.Web.UI.WebControls.Label
    Dim objrfq As New RFQ


    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim objGRN As New GRN
    Dim strGRNNo, strBCoyID As String
    Dim strPRIndex As String
    Dim blnPrintRemark As Boolean
    Dim blnPrintCustField As Boolean
    Dim objPR As New PurchaseReq2
    Dim value1(100) As String
    Dim value2(100) As String
    Dim dsGRNDtl As DataSet
    Dim i, jLoop, iLoop As Integer

    Public Enum EnumGRNDet
        icPOLine
        icPOLine2
        icVendorItemCode
        icDescription
        icUOM
        icMPQ
        icOrdered
        icDeliverQty
        icReceivedQty
        icRejQty
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
            objFile.getFilePath(EnumUploadFrom.FrontOff, EnumUploadType.PDFDownload, strFilePath, strMovePath, False, "GRN")
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

        'SetGridProperty(dtgPOReport)
        'viewstate("valIndex") = Request.QueryString("DOIndex")
        If Not IsPostBack Then
            blnFreeze = True
            Freeze(Session.SessionID & "_GRN.htm")
            BindGrid()
        End If

    End Sub

    Sub table_chk()
        'if 
    End Sub

    Private Function BindGrid()
        Dim objUser As New Users
        Dim objUserDetails As New User
        Dim objComps As New Companies
        Dim intTotRecord As Integer
        Dim strDONo As Integer '= 398
        Dim state As String
        Dim objrfq As New RFQ
        Dim country As String
        'Dim objval As New RFQ_User
        Dim i As Integer
        'Dim objComp As New Companies
        Dim intPOIndex As Integer
        Dim objPR1 As New PR

        strGRNNo = Me.Request.QueryString("GRNNo1")
        strBCoyID = Me.Request.QueryString("BCoyID1")
        strDONo = Me.Request.QueryString("DOIndex")
        dsGRNDtl = objGRN.GetGRNDetails(strDONo) 'objGRN.GetGRNHistory(strGRNNo, strBCoyID) '
        get_badds(strBCoyID)

        Dim objCompDetails As New Company
        objCompDetails = objComps.GetCompanyDetails(Session("CompanyId")) '// BUYER 

        If dsGRNDtl.Tables(0).Rows.Count > 0 Then
            If dsGRNDtl.Tables(0).Rows(0)("GM_GRN_LEVEL") = "2" Then
                For i = 4 To 6
                    Me.tb_user.Rows(i).Visible = True
                Next

                If Common.parseNull(dsGRNDtl.Tables(0).Rows(0)("GM_LEVEL2_USER")) <> "" Then
                    objUserDetails = objUser.GetUserDetails(dsGRNDtl.Tables(0).Rows(0)("GM_LEVEL2_USER"), Session("CompanyId"))
                    lblVerifiedBy.Text = objUserDetails.Name
                    lblTelVerified.Text = objUserDetails.PhoneNo
                    lblEmailVerified.Text = objUserDetails.Email
                Else
                    lblVerifiedBy.Text = ""
                    lblTelVerified.Text = ""
                    lblEmailVerified.Text = ""
                End If
            ElseIf dsGRNDtl.Tables(0).Rows(0)("GM_GRN_LEVEL") = "1" Then
                For i = 4 To 6
                    Me.tb_user.Rows(i).Visible = False
                Next
            End If

            intPOIndex = dsGRNDtl.Tables(0).Rows(0)("POM_PO_Index")
            lblGRNNo.Text = strGRNNo
            state = objrfq.get_codemstr(dsGRNDtl.Tables(0).Rows(0)("DOM_D_STATE"), "S")
            country = objrfq.get_codemstr(dsGRNDtl.Tables(0).Rows(0)("DOM_D_COUNTRY"), "CT")

            lblComName.Text = objCompDetails.CoyName
            lblOrganiz.Text = objCompDetails.CoyName

            lblAttention.Text = objPR1.getPOAttn(dsGRNDtl.Tables(0).Rows(0)("POM_PO_Index"))
            lblRecipiet.Text = dsGRNDtl.Tables(0).Rows(0)("GRN_Created_Name")
            lblRequestor.Text = dsGRNDtl.Tables(0).Rows(0)("POM_BUYER_NAME")
            lblTelReceived.Text = dsGRNDtl.Tables(0).Rows(0)("UM_TEL_NO")
            lblEmailReceived.Text = dsGRNDtl.Tables(0).Rows(0)("UM_EMAIL")
            lblPONo.Text = dsGRNDtl.Tables(0).Rows(0)("POM_PO_NO")
            lblDONo.Text = dsGRNDtl.Tables(0).Rows(0)("DOM_DO_NO")
            lblBusReg.Text = objCompDetails.BusinessRegNo
            lblGRecOn.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dsGRNDtl.Tables(0).Rows(0)("GM_DATE_RECEIVED"))

            If objPR.Need2PrintRemark(dsGRNDtl.Tables(0).Rows(0)("POD_PR_INDEX")) Then
                blnPrintRemark = True
            Else
                blnPrintRemark = False
            End If

            Dim strAddr As String
            strAddr = Common.parseNull(dsGRNDtl.Tables(0).Rows(0)("DOM_D_ADDR_LINE1"))
            If Not IsDBNull(dsGRNDtl.Tables(0).Rows(0)("DOM_D_ADDR_LINE2")) AndAlso dsGRNDtl.Tables(0).Rows(0)("DOM_D_ADDR_LINE2") <> "" Then
                strAddr = strAddr & "<BR>" & dsGRNDtl.Tables(0).Rows(0)("DOM_D_ADDR_LINE2")
            End If

            If Not IsDBNull(dsGRNDtl.Tables(0).Rows(0)("DOM_D_ADDR_LINE3")) AndAlso dsGRNDtl.Tables(0).Rows(0)("DOM_D_ADDR_LINE3") <> "" Then
                strAddr = strAddr & "<BR>" & dsGRNDtl.Tables(0).Rows(0)("DOM_D_ADDR_LINE3")
            End If

            If Not IsDBNull(dsGRNDtl.Tables(0).Rows(0)("DOM_D_POSTCODE")) Then
                strAddr = strAddr & "<BR>" & dsGRNDtl.Tables(0).Rows(0)("DOM_D_POSTCODE")
            End If

            If Not IsDBNull(dsGRNDtl.Tables(0).Rows(0)("DOM_D_CITY")) Then
                strAddr = strAddr & " " & dsGRNDtl.Tables(0).Rows(0)("DOM_D_CITY")
            End If

            If Not IsDBNull(state) Then
                strAddr = strAddr & "<BR>" & state
            End If

            If Not IsDBNull(country) Then
                strAddr = strAddr & " " & country
            End If

            lblDelAddr.Text = strAddr
            lblCreateOn.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dsGRNDtl.Tables(0).Rows(0)("GM_CREATED_DATE"))

        End If

        Dim dvViewGrnAck As DataView
        dvViewGrnAck = dsGRNDtl.Tables(0).DefaultView

        dvViewGrnAck.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewGrnAck.Sort += " DESC"

        intPageRecordCnt = dsGRNDtl.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        If ViewState("intPageRecordCnt") > 0 Then
            dtgPOReport.DataSource = dvViewGrnAck
            dtgPOReport.DataBind()
        End If

        Dim objFile As New FileManagement
        Dim strImgSrc As String
        strImgSrc = objFile.getCoyLogo(EnumUploadFrom.FrontOff, Request(Trim("BCoyID1")))
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
        objComps = Nothing
        objCompDetails = Nothing
        objPR1 = Nothing
    End Function

    Sub get_badds(ByVal bcom_id As String)
        Dim objrfq As New RFQ
        Dim objComp As New Company
        Dim objComps As New Companies
        Dim state As String
        Dim country As String
        objComp = objComps.GetCompanyDetails(bcom_id)
        lblComName.Text = objComp.CoyName
        Dim stradds As String
        state = objrfq.get_codemstr(objComp.State, "S")
        country = objrfq.get_codemstr(objComp.Country, "CT")
        If objComp.Address1 <> "" Then
            stradds = objComp.Address1
        End If

        If objComp.Address2 <> "" Then
            If stradds = "" Then
                stradds = " " & objComp.Address2 & ""
            Else
                stradds = stradds & "<br> " & objComp.Address2 & ""
            End If
        End If

        If objComp.Address3 <> "" Then
            If stradds = "" Then
                stradds = " " & objComp.Address3 & ""
            Else
                stradds = stradds & "<br> " & objComp.Address3 & ""
            End If
        End If

        If objComp.PostCode <> "" Or objComp.City <> "" Then
            If stradds = "" Then
                stradds = "" & objComp.PostCode & " " & objComp.City
            Else
                stradds = stradds & "<br>" & objComp.PostCode & " " & objComp.City
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
        lblComAddr.Text = stradds
        objComp = Nothing
        objComps = Nothing
    End Sub

    Sub DrawLine()
        Dim intL, intColToRemain As Integer
        Dim row2 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim intTotalCol As Integer
        Dim count As Integer
        Dim strtext As String

        For intL = 0 To dtgPOReport.Columns.Count - 1
            addCell(row2)
        Next

        For intL = 1 To dtgPOReport.Columns.Count - 1
            row2.Cells.RemoveAt(1)
        Next

        Dim lbl_test2 As New Label
        lbl_test2.ID = "lblHeaderLine"
        lbl_test2.Text = "<HR>"
        lbl_test2.Font.Bold = True
        row2.Cells(0).ColumnSpan = dtgPOReport.Columns.Count
        row2.Cells(0).Controls.Add(lbl_test2)
        row2.Cells(0).HorizontalAlign = HorizontalAlign.Left
        Me.dtgPOReport.Controls(0).Controls.Add(row2)

    End Sub
    Sub AddRow(ByVal intCell As Integer, ByVal value1() As String, ByVal value2() As String, ByVal j As Integer, ByVal remark As String)

        Dim intL, intColToRemain As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim row2 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim intTotalCol As Integer
        Dim count As Integer
        Dim strtext As String



        If blnPrintRemark Then
            For intL = 0 To dtgPOReport.Columns.Count - 1
                addCell(row2)
            Next

            For intL = 2 To dtgPOReport.Columns.Count - 2
                row2.Cells.RemoveAt(1)
            Next

            Dim lbl_test2 As New Label
            lbl_test2.ID = "test2"
            lbl_test2.Text = "<B>Remarks</B> : " & remark & ""
            lbl_test2.CssClass = "txtbox"
            lbl_test2.Width = System.Web.UI.WebControls.Unit.Pixel(500)
            lbl_test2.Font.Bold = False

            row2.Cells(2).ColumnSpan = dtgPOReport.Columns.Count
            row2.Cells(2).Controls.Add(lbl_test2)
            row2.Cells(2).HorizontalAlign = HorizontalAlign.Left


            Me.dtgPOReport.Controls(0).Controls.Add(row2)
        End If
    End Sub
    Sub addCell(ByRef row As DataGridItem)
        Dim cell As TableCell = New TableCell
        row.Cells.Add(cell)

    End Sub
    Private Sub dtgPOReport_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPOReport.ItemDataBound
        If i = 0 Then
            DrawLine()
        End If

        Dim intPRLine As Integer
        Dim strFieldNo, strDAddr As String
        Dim dr() As DataRow
        Dim dblAmt, dblGstAmt As Double
        Dim row2 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim j As Integer
        Dim objpo As New PurchaseOrder

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            e.Item.Cells(EnumGRNDet.icPOLine2).Text = ""

            If blnPrintRemark Or blnPrintCustField Then
                If blnPrintCustField Then
                    objpo.get_customfield(value1, value2, dv("GD_PO_LINE"), dv("POD_PR_INDEX"), j) 'dsItem.Tables(1).Rows(0)("PRD_PR_LINE"), dsItem.Tables(0).Rows(0)("PRM_PR_INDEX"), j) 'dv("PRD_PR_LINE"), dv("PRM_PR_INDEX"), j)
                End If
                AddRow(2, value1, value2, j, dv("GD_REMARKS"))
            End If
        End If
        i = i + 1
    End Sub
End Class
