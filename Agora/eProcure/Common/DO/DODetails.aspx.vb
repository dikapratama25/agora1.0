Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient

Public Class DODetails
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDb As New EAD.DBCom
    Dim objFile As New FileManagement
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents lblDONo As System.Web.UI.WebControls.Label
    Protected WithEvents dtgDODtl As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DtgDoSumm As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblStatus As System.Web.UI.WebControls.Label
    Protected WithEvents lblBillTo As System.Web.UI.WebControls.Label
    Protected WithEvents lblShipTo As System.Web.UI.WebControls.Label
    Protected WithEvents lblDevlDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblOurRefNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblPayTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lblCustName As System.Web.UI.WebControls.Label
    Protected WithEvents lblOurRefDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblShipTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lblPayMthd As System.Web.UI.WebControls.Label
    Protected WithEvents lblAirWayBillNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblShipMthd As System.Web.UI.WebControls.Label
    Protected WithEvents lblFrieghtCarrier As System.Web.UI.WebControls.Label
    Protected WithEvents lblFreightAmt As System.Web.UI.WebControls.Label
    Protected WithEvents lblRemarks As System.Web.UI.WebControls.Label
    Protected WithEvents lblPONo As System.Web.UI.WebControls.Label
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblSummPONum As System.Web.UI.WebControls.Label
    Protected WithEvents cmdPreviewDO1 As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents tblDOSumm As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblFileAttach As System.Web.UI.WebControls.Label
    Protected WithEvents lblDelTerm As System.Web.UI.WebControls.Label
    Protected WithEvents tr_delTerm As System.Web.UI.HtmlControls.HtmlTableRow
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim IntDoIdx As Integer
    Dim strDONo, strLocID, PONo, strSCoyID As String
    Dim IntPOIdx As Integer
    Dim objDO As New DeliveryOrder
    

    Public Enum EnumSumDo
        Date_Create
        Do_Num
        U_Name
    End Enum
    Public Enum EnumSumDoDtl
        D_line
        I_Code
        P_Desc
        P_Uom
        P_Etd
        P_Waran
        P_Qty
        M_Order_Qty
        Ordered_Qty
        Ship_Qty
        Do_Lot
        Do_Qty
        D_Remark
        P_Date

    End Enum

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CheckButtonAccess(True)
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        If Not Page.IsPostBack Then
            Session("blnLotNo") = False
            If Me.Request.QueryString("Frm") = "Listing" Then GenerateTab()
        End If

        blnPaging = False
        SetGridProperty(dtgDODtl)
        blnPaging = False
        blnSorting = False
        If LCase(Request.QueryString("caller")) <> "buyer" Then SetGridProperty(DtgDoSumm)
        'Session("CompanyID") = "PPSB"
        'IntDoIdx = Me.Request.QueryString("DOIdx")
        strDONo = Me.Request.QueryString("DONo")
        IntPOIdx = Me.Request.QueryString("POIdx")
        'strLocID = Me.Request.QueryString("LocID")
        strSCoyID = Me.Request.QueryString("SCoyID")
        PONo = Me.Request.QueryString("PONo")
        BindData(strDONo, IntDoIdx, IntPOIdx)
        'lnkBack.NavigateUrl = "searchDO.aspx?pageid=" & strPageId

        If Session("urlreferer") = "DOListing_Buyer" Then
            lnkBack.NavigateUrl = dDispatcher.direct("DO", "DOListing_Buyer.aspx", "pageid=" & strPageId)
            Me.cmdPreviewDO1.Visible = False
        Else
            lnkBack.NavigateUrl = dDispatcher.direct("DO", "searchDO.aspx", "pageid=" & strPageId)
            Me.cmdPreviewDO1.Visible = True
        End If

        Me.cmdPreviewDO1.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewDO.aspx", "SCoyID=" & strSCoyID & "&DONo=" & strDONo & "&PO_NO=" & Server.UrlEncode(lblPONo.Text)) & "')")

        'Michelle (23/1/2013)- Issue 1727
        Dim dvFile As DataView
        Dim intLoop, intCount As Integer
        Dim strFile, strFile1, strURL, strTemp, strTempInt As String
        If Session("urlreferer") = "DOListing_Buyer" Then
            dvFile = objDO.getDOAttachment(strDONo, strSCoyID).Tables(0).DefaultView
        Else
            dvFile = objDO.getDOAttachment(strDONo).Tables(0).DefaultView
        End If

        If dvFile.Count > 0 Then
            For intLoop = 0 To dvFile.Count - 1
                strFile = dvFile(intLoop)("CDDA_ATTACH_FILENAME")
                strFile1 = dvFile(intLoop)("CDDA_HUB_FILENAME")
                If Session("urlreferer") = "DOListing_Buyer" Then
                    strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DOAttachment, "DO", EnumUploadFrom.FrontOff, , strSCoyID)
                Else
                    strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DOAttachment, "DO", EnumUploadFrom.FrontOff)
                End If
                If strTemp = "" Then
                    strTemp = intCount + 1 & ") " & strURL & " (" & dvFile(intLoop)("CDDA_FILESIZE") & "KB)"
                    intCount = intCount + 1
                Else
                    strTemp = strTemp & "<BR>&nbsp;" & intCount + 1 & ") " & strURL & " (" & dvFile(intLoop)("CDDA_FILESIZE") & "KB)"
                    intCount = intCount + 1
                End If

            Next
        Else
            strTemp = "No Files Attached"
        End If
        lblFileAttach.Text = strTemp

    End Sub

    Private Sub PreviewDO()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, strSCoyID, System.AppDomain.CurrentDomain.BaseDirectory & "images\")

        Try

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT *, (SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_MSTR.POM_S_STATE) AND (CODE_CATEGORY = 's') AND (CODE_VALUE = PO_DETAILS.POD_D_COUNTRY)) AS SupplierAddrState," _
                        & "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_MSTR.POM_S_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS SupplierAddrCtry, " _
                        & "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_MSTR.POM_B_STATE) AND (CODE_CATEGORY = 's') AND (CODE_VALUE = PO_DETAILS.POD_D_COUNTRY)) AS BillAddrState, " _
                        & "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_MSTR.POM_B_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS BillAddrCtry," _
                        & "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_DETAILS.POD_D_STATE) AND (CODE_CATEGORY = 's') AND (CODE_VALUE = PO_DETAILS.POD_D_COUNTRY)) AS DelvAddrState, " _
                        & "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_DETAILS.POD_D_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS DelvAddrCtry," _
                        & "(SELECT CM_COY_NAME FROM COMPANY_MSTR AS b WHERE (CM_COY_ID = PO_MSTR.POM_B_COY_ID)) AS BuyerCompanyName " _
                        & "FROM PO_MSTR INNER JOIN PO_DETAILS ON PO_MSTR.POM_PO_NO = PO_DETAILS.POD_PO_NO AND PO_MSTR.POM_B_COY_ID = PO_DETAILS.POD_COY_ID INNER JOIN DO_MSTR ON PO_MSTR.POM_PO_INDEX = DO_MSTR.DOM_PO_INDEX INNER JOIN DO_DETAILS ON DO_MSTR.DOM_DO_NO = DO_DETAILS.DOD_DO_NO AND DO_MSTR.DOM_S_COY_ID = DO_DETAILS.DOD_S_COY_ID AND PO_DETAILS.POD_PO_LINE = DO_DETAILS.DOD_PO_LINE INNER JOIN COMPANY_MSTR ON PO_MSTR.POM_S_COY_ID = COMPANY_MSTR.CM_COY_ID " _
                        & "WHERE (PO_MSTR.POM_S_COY_ID = @prmCoyID) AND (DO_MSTR.DOM_DO_NO = @prmDONo)"
            End With

            da = New MySqlDataAdapter(cmd)
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", strSCoyID))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmDONo", strDONo))


            da.Fill(ds)

            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewDO_DataSetPreviewDO", ds.Tables(0))
            Dim localreport As New LocalReport
            Dim strSQL As String = "SELECT * FROM pr_details WHERE PRD_CONVERT_TO_IND = 'PO' AND PRD_CONVERT_TO_DOC = '" & PONo & "' AND PRD_COY_ID ='" & strSCoyID & "'"

            If objDb.Exist(strSQL) < 0 Then
                localreport.ReportPath = appPath & "DO\PreviewDO.rdlc"
            Else
                localreport.ReportPath = appPath & "DO\PreveiwDO-FTN.rdlc"
            End If
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            'localreport.ReportPath = appPath & "DO\PreveiwDO-FTN.rdlc"
            'localreport.ReportPath = appPath & "DO\PreviewDO.rdlc"
            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            TotalParameter = localreport.GetParameters.Count
            Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
            'Dim paramlist As New Generic.List(Of ReportParameter)
            For I = 0 To localreport.GetParameters.Count - 1
                GetParameter = localreport.GetParameters.Item(I).Name
                Select Case LCase(GetParameter)
                    Case "par1"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings("ReportCoyLogoPath") & strImgSrc)
                    Case "freightamt"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, Me.lblFreightAmt.Text)
                    Case Else
                End Select
            Next
            localreport.SetParameters(par)
            localreport.Refresh()

            'Dim deviceInfo As String = _
            ' "<DeviceInfo>" + _
            '     "  <OutputFormat>EMF</OutputFormat>" + _
            '     "  <PageWidth>11in</PageWidth>" + _
            '     "  <PageHeight>8.5in</PageHeight>" + _
            '     "  <MarginTop>0.25in</MarginTop>" + _
            '     "  <MarginLeft>0.25in</MarginLeft>" + _
            '     "  <MarginRight>0.25in</MarginRight>" + _
            '     "  <MarginBottom>0.25in</MarginBottom>" + _
            '     "</DeviceInfo>"
            'Dim deviceInfo As String = _
            '     "<DeviceInfo>" + _
            '         "  <OutputFormat>EMF</OutputFormat>" + _
            '         "  <PageWidth>8.27in</PageWidth>" + _
            '         "  <PageHeight>11in</PageHeight>" + _
            '         "  <MarginTop>0.25in</MarginTop>" + _
            '         "  <MarginLeft>0.25in</MarginLeft>" + _
            '         "  <MarginRight>0.25in</MarginRight>" + _
            '         "  <MarginBottom>0.25in</MarginBottom>" + _
            '         "</DeviceInfo>"
            Dim deviceInfo As String = _
                "<DeviceInfo>" + _
                    "  <OutputFormat>EMF</OutputFormat>" + _
                    "</DeviceInfo>"
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)

            Dim fs As New FileStream(appPath & "DO\DOReport.PDF", FileMode.Create)
            fs.Write(PDF, 0, PDF.Length)
            fs.Close()

            Dim strJScript As String
            strJScript = "<script language=javascript>"
            strJScript += "window.open('DOReport.PDF',null,'height=600, width=730,status= no, resizable= yes, scrollbars=yes, toolbar=no,location=center,menubar=no, top=10, left=100');"
            strJScript += "</script>"
            Response.Write(strJScript)

        Catch ex As Exception
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try
    End Sub

    Private Sub BindData(ByVal strDONo As String, ByVal intDOIdx As Integer, ByVal IntPOIdx As Integer)
        Dim dsAllInfo, dsDOSumm As DataSet
        Dim strDetails, strMstr As String
        Dim dtDetails As New DataTable
        Dim dtMstr As New DataTable
        Dim strETD As String
        Dim strDevlAdd, strBillAdd, strDelCode, strDelTerm As String
        Dim dtHeader As New DataTable

        ''dsAllInfo = objDO.filterDevlAdd(cboDelvAdd, PONo, POIndex)
        'If Not dsAllInfo Is Nothing Then
        '    dtHeader = dsAllInfo.Tables(0)
        '    If dtHeader.Rows.Count > 0 And dtHeader.Rows.Count = 1 Then
        '    ElseIf dtHeader.Rows.Count > 1 Then
        '        'lblDelvAdd.Visible = False
        '    End If
        'End If

        'dsAllInfo = objDO.ShowDOdetails(strDONo, PONo, IntPOIdx, strLocID, strBCoyID)
        dsAllInfo = objDO.DOReport(strDONo, strSCoyID)
        If Not dsAllInfo Is Nothing Then
            dtMstr = dsAllInfo.Tables(0)
            If dtMstr.Rows.Count > 0 Then

                'Check for Delivery Term setup
                If Not IsDBNull(dsAllInfo.Tables(0).Rows(0)("POD_ITEM_TYPE")) Then
                    If objDb.Exist("SELECT '*' FROM COMPANY_DELIVERY_TERM WHERE CDT_COY_ID = '" & Common.parseNull(dsAllInfo.Tables(0).Rows(0)("POM_B_Coy_ID")) & "'") And dsAllInfo.Tables(0).Rows(0)("POD_ITEM_TYPE") = "ST" Then
                        Session("blnLotNo") = True
                    Else
                        Session("blnLotNo") = False
                    End If
                Else
                    Session("blnLotNo") = False
                End If
                
                lblDONo.Text = strDONo
                lblPONo.Text = dtMstr.Rows(0)("POM_PO_NO")

                strDevlAdd = Common.parseNull(dtMstr.Rows(0)("POD_D_Addr_Line1"))
                If Not IsDBNull(dtMstr.Rows(0)("POD_D_Addr_Line2")) AndAlso dtMstr.Rows(0)("POD_D_Addr_Line2") <> "" Then
                    strDevlAdd = strDevlAdd & "<BR>" & dtMstr.Rows(0)("POD_D_Addr_Line2")
                End If

                If Not IsDBNull(dtMstr.Rows(0)("POD_D_Addr_Line3")) AndAlso dtMstr.Rows(0)("POD_D_Addr_Line3") <> "" Then
                    strDevlAdd = strDevlAdd & "<BR>" & dtMstr.Rows(0)("POD_D_Addr_Line3")
                End If
                If Not IsDBNull(dtMstr.Rows(0)("POD_D_State")) AndAlso dtMstr.Rows(0)("POD_D_State") <> "" Then
                    strDevlAdd = strDevlAdd & "<BR>" & dtMstr.Rows(0)("POD_D_State")
                End If

                If Not IsDBNull(dtMstr.Rows(0)("POD_D_PostCode")) AndAlso dtMstr.Rows(0)("POD_D_PostCode") <> "" Then
                    strDevlAdd = strDevlAdd & "<BR>" & dtMstr.Rows(0)("POD_D_PostCode")
                End If

                If Not IsDBNull(dtMstr.Rows(0)("POD_D_City")) AndAlso dtMstr.Rows(0)("POD_D_City") <> "" Then
                    strDevlAdd = strDevlAdd & " " & dtMstr.Rows(0)("POD_D_City")
                End If
                If Not IsDBNull(dtMstr.Rows(0)("POD_D_Country")) AndAlso dtMstr.Rows(0)("POD_D_Country") <> "" Then
                    strDevlAdd = strDevlAdd & " " & dtMstr.Rows(0)("POD_D_Country")
                End If
                lblShipTo.Text = strDevlAdd

                strBillAdd = Common.parseNull(dtMstr.Rows(0)("POM_B_Addr_Line1"))
                If Not IsDBNull(dtMstr.Rows(0)("POM_B_Addr_Line2")) AndAlso dtMstr.Rows(0)("POM_B_Addr_Line2") <> "" Then
                    strBillAdd = strBillAdd & "<BR>" & dtMstr.Rows(0)("POM_B_Addr_Line2")
                End If

                If Not IsDBNull(dtMstr.Rows(0)("POM_B_Addr_Line3")) AndAlso dtMstr.Rows(0)("POM_B_Addr_Line3") <> "" Then
                    strBillAdd = strBillAdd & "<BR>" & dtMstr.Rows(0)("POM_B_Addr_Line3")
                End If
                If Not IsDBNull(dtMstr.Rows(0)("POM_B_State")) AndAlso dtMstr.Rows(0)("POM_B_State") <> "" Then
                    strBillAdd = strBillAdd & "<BR>" & dtMstr.Rows(0)("POM_B_State")
                End If

                If Not IsDBNull(dtMstr.Rows(0)("POM_B_PostCode")) Then
                    strBillAdd = strBillAdd & "<BR>" & dtMstr.Rows(0)("POM_B_PostCode")
                End If

                If Not IsDBNull(dtMstr.Rows(0)("POM_B_City")) Then
                    strBillAdd = strBillAdd & " " & dtMstr.Rows(0)("POM_B_City")
                End If
                If Not IsDBNull(dtMstr.Rows(0)("POM_B_Country")) Then
                    strBillAdd = strBillAdd & " " & dtMstr.Rows(0)("POM_B_Country")
                End If

                lblBillTo.Text = strBillAdd
                lblPayTerm.Text = Common.parseNull(dtMstr.Rows(0)("POM_Payment_TERM"), "Not Applicable")
                lblShipTerm.Text = Common.parseNull(dtMstr.Rows(0)("POM_Shipment_Term"), "Not Applicable")
                lblPayMthd.Text = Common.parseNull(dtMstr.Rows(0)("POM_PAYMENT_METHOD"), "Not Applicable")
                lblShipMthd.Text = Common.parseNull(dtMstr.Rows(0)("POM_Shipment_Mode"), "Not Applicable")
                lblAirWayBillNo.Text = Common.parseNull(dtMstr.Rows(0)("DOM_Waybill_No"))
                lblFrieghtCarrier.Text = Common.parseNull(dtMstr.Rows(0)("DOM_Freight_Carrier"))
                lblRemarks.Text = Common.parseNull(dtMstr.Rows(0)("DOM_DO_Remarks"))
                lblOurRefNo.Text = Common.parseNull(dtMstr.Rows(0)("DOM_S_Ref_No"))
                lblOurRefDate.Text = Common.parseNull(dtMstr.Rows(0)("DOM_S_Ref_Date"))
                lblFreightAmt.Text = Common.parseNull(dtMstr.Rows(0)("DOM_Freight_Amt"))
                lblCustName.Text = Common.parseNull(dtMstr.Rows(0)("CM_Coy_Name"), "Not Applicable")
                lblDevlDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dtMstr.Rows(0)("DOM_DO_DATE"))

                lblStatus.Text = Common.parseNull(dtMstr.Rows(0)("Status_DESC"))
                lblSummPONum.Text = dtMstr.Rows(0)("POM_PO_NO")

                If Common.parseNull(dtMstr.Rows(0)("POM_DEL_CODE")) = "" Then
                    tr_delTerm.Style("display") = "none"
                Else
                    strDelCode = Common.parseNull(dtMstr.Rows(0)("POM_DEL_CODE"))
                    strDelTerm = objDb.GetVal("SELECT IFNULL(CONCAT(CDT_DEL_CODE, ' (', CDT_DEL_NAME, ')'),'') FROM COMPANY_DELIVERY_TERM " & _
                                            "WHERE CDT_COY_ID = '" & dtMstr.Rows(0)("POM_B_COY_ID") & "' AND CDT_DEL_CODE='" & Common.Parse(strDelCode) & "'")
                    lblDelTerm.Text = strDelTerm
                End If

                Dim dvViewDO As DataView
                dvViewDO = dsAllInfo.Tables(0).DefaultView
                dvViewDO.Sort = ViewState("SortExpression")
                If ViewState("SortAscending") = "no" Then dvViewDO.Sort += " DESC"
                dtgDODtl.DataSource = dvViewDO
                dtgDODtl.DataBind()
                Dim AddCode As String
                AddCode = Common.parseNull(dtMstr.Rows(0)("DOM_D_Addr_Code"))

                'If Session("Env") = "FTN" Then
                '    Me.dtgDODtl.Columns(5).Visible = False
                '    Me.dtgDODtl.Columns(6).Visible = False
                '    Me.dtgDODtl.Columns(7).Visible = False
                'Else
                '    Me.dtgDODtl.Columns(5).Visible = True
                '    Me.dtgDODtl.Columns(6).Visible = True
                '    Me.dtgDODtl.Columns(7).Visible = True
                'End If

                Me.dtgDODtl.Columns(5).Visible = True
                Me.dtgDODtl.Columns(6).Visible = True
                Me.dtgDODtl.Columns(7).Visible = True

                If Session("blnLotNo") = True Then
                    Me.dtgDODtl.Columns(EnumSumDoDtl.Do_Lot).Visible = True
                Else
                    Me.dtgDODtl.Columns(EnumSumDoDtl.Do_Lot).Visible = False
                End If

                If LCase(Request.QueryString("caller")) <> "buyer" Then
                    tblDOSumm.Visible = True
                    dsDOSumm = objDO.GetDOSumm(IntPOIdx, strSCoyID)
                    Dim dvViewDOSumm As DataView
                    dvViewDOSumm = dsDOSumm.Tables(0).DefaultView
                    DtgDoSumm.DataSource = dvViewDOSumm
                    DtgDoSumm.DataBind()
                Else
                    tblDOSumm.Visible = False
                End If
            End If
        End If
    End Sub


    Sub dtgDODtl_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgDODtl.PageIndexChanged
        dtgDODtl.CurrentPageIndex = e.NewPageIndex
        BindData(strDONo, IntDoIdx, IntPOIdx)
    End Sub

    Sub SortCommand_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles dtgDODtl.SortCommand
        Grid_SortCommand(sender, e)
        dtgDODtl.CurrentPageIndex = 0

    End Sub
    Private Sub dtgDODtl_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgDODtl.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dtgDODtl, e)
    End Sub

    Private Sub dtgDODtl_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgDODtl.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dt As DataTable
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim lbl_LotNo As Label
            lbl_LotNo = e.Item.FindControl("lblLotNo")
            If Session("urlreferer") = "DOListing_Buyer" Then
                If objDO.chkLotWithAO(dv("POD_Vendor_Item_code")) Then
                    lbl_LotNo.Text = Common.parseNull(dv("DOD_DO_LOT_NO")) & "<A onclick=""PopWindow('" & dDispatcher.direct("DO", "DOLotList.aspx", "pageid=" & strPageId) & "&DONo=" & dv("DOM_DO_NO") & "&callfrom=buyer" & "&itemcode=" & dv("POD_Vendor_Item_code") & "&poline=" & dv("POD_PO_LINE") & "&SCoyID=" & strSCoyID & "&BCoyID=" & dv("POM_B_COY_ID") & "');"" ><IMG style=""CURSOR: hand"" src=" & dDispatcher.direct("Plugins/Images", "d_icon.gif") & "></A>"
                Else
                    lbl_LotNo.Text = Common.parseNull(dv("DOD_DO_LOT_NO"))
                End If
            Else
                lbl_LotNo.Text = Common.parseNull(dv("DOD_DO_LOT_NO")) & "<A onclick=""PopWindow('" & dDispatcher.direct("DO", "DOLotList.aspx", "pageid=" & strPageId) & "&DONo=" & dv("DOM_DO_NO") & "&callfrom=vendor" & "&itemcode=" & dv("POD_Vendor_Item_code") & "&poline=" & dv("POD_PO_LINE") & "&SCoyID=" & strSCoyID & "&BCoyID=" & dv("POM_B_COY_ID") & "');"" ><IMG style=""CURSOR: hand"" src=" & dDispatcher.direct("Plugins/Images", "d_icon.gif") & "></A>"
            End If
           
            If dv("DOM_DO_Status") = DOStatus.Submitted Then
                e.Item.Cells(EnumSumDoDtl.Do_Qty).Text = 0
            End If
            If IsDBNull(e.Item.Cells(EnumSumDoDtl.P_Etd).Text) Or e.Item.Cells(EnumSumDoDtl.P_Etd).Text = "0" Then
                e.Item.Cells(EnumSumDoDtl.P_Etd).Text = "Ex-Stock"
            Else
                e.Item.Cells(EnumSumDoDtl.P_Etd).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, DateAdd("D", dv("POD_ETD"), dv("POM_CREATED_DATE")))
            End If



        End If
    End Sub
    Private Sub DtgDoSumm_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DtgDoSumm.ItemCreated
        '//this line must be included
        Grid_ItemCreated(DtgDoSumm, e)
    End Sub
    Private Sub DtgDoSumm_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DtgDoSumm.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dt As DataTable
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkDONum, lnkPONum As HyperLink
            '//Original one
            'e.Item.Cells(0).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("date_created"))
            '//
            e.Item.Cells(EnumSumDo.Date_Create).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("date_created"))
            If Session("urlreferer") = "DOListing_Buyer" Then
                lnkDONum = e.Item.FindControl("lnkDONum")
                lnkDONum.Text = dv("DOM_DO_NO")
            Else
                lnkDONum = e.Item.FindControl("lnkDONum")
                lnkDONum.Text = dv("DOM_DO_NO")
                lnkDONum.NavigateUrl = "javascript:;" '"DODetails.aspx?DONo=" & dv("DOM_DO_NO") & "&DOIdx=" & dv("DOM_DO_Index") & "&POIdx=" & dv("DOM_PO_Index") & "&pageid=" & strPageId & "&LocID=" & dv("DOM_D_ADDR_CODE") & "&BCoy=" & viewstate("BCoyID")
                'lnkDONum.Attributes.Add("onclick", "return PopWindow('PreviewDO.aspx?pageid=" & strPageId & "&DONo=" & dv("DOM_DO_NO") & "&SCoyID=" & Session("CompanyID") & "')")
                lnkDONum.Attributes.Add("onclick", "return PopWindow('" & dDispatcher.direct("Report", "PreviewDO.aspx", "pageid=" & strPageId & "&DONo=" & dv("DOM_DO_NO") & "&SCoyID=" & Session("CompanyID") & "&PO_NO=" & Server.UrlEncode(lblPONo.Text)) & "')")

                'lnkDONum.Attributes.Add("onclick", "return PopWindow('DOReport.aspx?pageid=" & strPageId & "&DONo=" & dv("DOM_DO_NO") & "&SCoyID=" & strSCoyID & "')")
            End If
           
        End If
    End Sub
    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        'Session("w_DO_tabs") = "<div class=""t_entity""><ul>" & _
        '    "<li><div class=""space""></div></li>" & _
        '             "<li><a class=""t_entity_btn"" href=""POList.aspx?pageid=" & strPageId & """><span>Issue DO</span></a></li>" & _
        '                            "<li><div class=""space""></div></li>" & _
        '             "<li><a class=""t_entity_btn_selected"" href=""searchDO.aspx?pageid=" & strPageId & """><span>DO Listing</span></a></li>" & _
        '                            "<li><div class=""space""></div></li>" & _
        '            "</ul><div></div></div>"
        'Session("w_DO_tabs") = "<div class=""t_entity"">" & _
        '     "<a class=""t_entity_btn"" href=""POList.aspx?pageid=" & strPageId & """><span>Issue DO</span></a>" & _
        '     "<a class=""t_entity_btn_selected"" href=""searchDO.aspx?pageid=" & strPageId & """><span>DO Listing</span></a>" & _
        '    "</div>"
        Session("w_DO_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DO", "POList.aspx", "pageid=" & strPageId) & """><span>Issue DO</span></a></li>" & _
     "<li><div class=""space""></div></li>" & _
     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("DO", "searchDO.aspx", "pageid=" & strPageId) & """><span>DO Listing</span></a></li>" & _
     "<li><div class=""space""></div></li>" & _
    "</ul><div></div></div>"

    End Sub

    'Protected Sub cmdPreviewDO1_ServerClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPreviewDO1.ServerClick
    '    'PreviewDO()
    '    dDispatcher.direct("Report", "PreviewDO.aspx", "SCoyID=" & Session("CompanyID") & "&DONo=" & strDONo)
    'End Sub
End Class
