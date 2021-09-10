Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient

Public Class PreviewDODetails
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDb As New EAD.DBCom

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
  
    Protected WithEvents cmdPreviewDO1 As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdClose As System.Web.UI.WebControls.Button
    Protected WithEvents lblFileAttach As System.Web.UI.WebControls.Label
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
        Dim objFile As New FileManagement
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
        Me.cmdClose.Attributes.Add("onclick", "window.close();")

        'Michelle (23/1/2013)- Issue 1727
        Dim dvFile As DataView
        Dim intLoop, intCount As Integer
        Dim strFile, strFile1, strURL, strTemp As String
        dvFile = objDO.getDOAttachment(strDONo, strSCoyID).Tables(0).DefaultView
        If dvFile.Count > 0 Then
            For intLoop = 0 To dvFile.Count - 1
                strFile = dvFile(intLoop)("CDDA_ATTACH_FILENAME")
                strFile1 = dvFile(intLoop)("CDDA_HUB_FILENAME")
                strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DOAttachment, "DO", EnumUploadFrom.FrontOff, , strSCoyID)
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

    Private Sub BindData(ByVal strDONo As String, ByVal intDOIdx As Integer, ByVal IntPOIdx As Integer)
        Dim dsAllInfo, dsDOSumm As DataSet
        Dim strDetails, strMstr As String
        Dim dtDetails As New DataTable
        Dim dtMstr As New DataTable
        Dim strETD As String
        Dim strDevlAdd, strBillAdd As String
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

                If LCase(Request.QueryString("caller")) <> "buyer" Then
                    tblDOSumm.Visible = True
                    dsDOSumm = objDO.GetDOSumm(IntPOIdx)
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
            '// Original one, suppose cells(4)rather than cells(5)
            'If dv("DOM_DO_Status") = DOStatus.Submitted Then
            '    e.Item.Cells(10).Text = 0
            'End If
            'If IsDBNull(e.Item.Cells(5).Text) Or e.Item.Cells(5).Text = "0" Then
            '    e.Item.Cells(5).Text = "Ex-Stock"
            'Else
            '    e.Item.Cells(5).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, DateAdd("D", dv("POD_ETD"), dv("POM_PO_Date")))
            'End If
            '//
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
            '//Original one
            'e.Item.Cells(0).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("date_created"))
            '//
            e.Item.Cells(EnumSumDo.Date_Create).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("date_created"))
            Dim lnkDONum, lnkPONum As HyperLink
            lnkDONum = e.Item.FindControl("lnkDONum")
            lnkDONum.Text = dv("DOM_DO_NO")
            lnkDONum.NavigateUrl = "javascript:;" '"DODetails.aspx?DONo=" & dv("DOM_DO_NO") & "&DOIdx=" & dv("DOM_DO_Index") & "&POIdx=" & dv("DOM_PO_Index") & "&pageid=" & strPageId & "&LocID=" & dv("DOM_D_ADDR_CODE") & "&BCoy=" & viewstate("BCoyID")
            'lnkDONum.Attributes.Add("onclick", "return PopWindow('PreviewDO.aspx?pageid=" & strPageId & "&DONo=" & dv("DOM_DO_NO") & "&SCoyID=" & Session("CompanyID") & "')")
            lnkDONum.Attributes.Add("onclick", "return PopWindow('" & dDispatcher.direct("Report", "PreviewDO.aspx", "pageid=" & strPageId & "&DONo=" & dv("DOM_DO_NO") & "&SCoyID=" & Session("CompanyID") & "&PO_NO=" & Server.UrlEncode(lblPONo.Text)) & "')")

            'lnkDONum.Attributes.Add("onclick", "return PopWindow('DOReport.aspx?pageid=" & strPageId & "&DONo=" & dv("DOM_DO_NO") & "&SCoyID=" & strSCoyID & "')")
        End If
    End Sub

End Class