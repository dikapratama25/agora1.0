Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient

Public Class ReportFormatSEH1

    Inherits AgoraLegacy.AppBaseClass
    Dim dispatcher As New dispatcher
    Dim objDb As New EAD.DBCom

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim ii_ddl, ii_ddl2, jj_ddl As Integer
        Dim lstItem As New ListItem
        'get data from previous page
        Dim strCoyType, strRptType As String
        lblHeader.Text = Request.QueryString("rptname")
        strCoyType = lblHeader.Text.Substring(1, 1)
        strRptType = Request.QueryString("rpttype")
        ViewState("type") = Request.QueryString("type")
        ViewState("ReportType") = strRptType

        If Not (Page.IsPostBack) Then
            ' condition to select useable code to enable GUI needed
            If strRptType = "1" Then
                'Start Date, End Date
                Me.tr_3.Style("display") = ""
                rfvMonthFrom.Enabled = False
                rfvMonthTo.Enabled = False
                rfvYearFrom.Enabled = False
                rfvYearTo.Enabled = False
                rfvMonth.Enabled = False
                rfvYear.Enabled = False
                ValDate.Enabled = False
                ValDptCode.Enabled = False
                ValDptCode2.Enabled = False
                ValGRNSDate.Enabled = False
                ValGRNEDate.Enabled = False
                cvGRNDate.Enabled = False

            ElseIf strRptType = "2" Then
                'Month From, Month To, Year From, Year to
                Me.tr_1.Style("display") = ""
                Me.tr_2.Style("display") = ""
                rfvMonth.Enabled = False
                rfvYear.Enabled = False
                ValDate.Enabled = False
                ValDptCode.Enabled = False
                ValDptCode2.Enabled = False
                ValSDate.Enabled = False
                ValEDate.Enabled = False
                cvDate.Enabled = False
                ValGRNSDate.Enabled = False
                ValGRNEDate.Enabled = False
                cvGRNDate.Enabled = False

            ElseIf strRptType = "3" Then
                'Oversea/Local, Date
                Me.tr_5.Style("display") = ""
                Me.tr_6.Style("display") = ""
                rfvMonthFrom.Enabled = False
                rfvMonthTo.Enabled = False
                rfvYearFrom.Enabled = False
                rfvYearTo.Enabled = False
                rfvMonth.Enabled = False
                rfvYear.Enabled = False
                ValDptCode.Enabled = False
                ValDptCode2.Enabled = False
                ValSDate.Enabled = False
                ValEDate.Enabled = False
                cvDate.Enabled = False
                ValGRNSDate.Enabled = False
                ValGRNEDate.Enabled = False
                cvGRNDate.Enabled = False
                ValGRNSDate.Enabled = False
                ValGRNEDate.Enabled = False
                cvGRNDate.Enabled = False

                If ViewState("type") = "SMFA0612MTH" Then
                    txtDate.Text = Format(Today.Date, "dd/MM/yyyy")
                End If

            ElseIf strRptType = "4" Then
                'Month, Year, Oversea/Local
                Me.tr_4.Style("display") = ""
                Me.tr_5.Style("display") = ""
                rfvMonthFrom.Enabled = False
                rfvMonthTo.Enabled = False
                rfvYearFrom.Enabled = False
                rfvYearTo.Enabled = False
                ValDate.Enabled = False
                ValDptCode.Enabled = False
                ValDptCode2.Enabled = False
                ValSDate.Enabled = False
                ValEDate.Enabled = False
                cvDate.Enabled = False
                ValGRNSDate.Enabled = False
                ValGRNEDate.Enabled = False
                cvGRNDate.Enabled = False

            ElseIf strRptType = "5" Then
                'Month, Year
                Me.tr_4.Style("display") = ""
                rfvMonthFrom.Enabled = False
                rfvMonthTo.Enabled = False
                rfvYearFrom.Enabled = False
                rfvYearTo.Enabled = False
                ValDate.Enabled = False
                ValDptCode.Enabled = False
                ValDptCode2.Enabled = False
                ValSDate.Enabled = False
                ValEDate.Enabled = False
                cvDate.Enabled = False
                ValGRNSDate.Enabled = False
                ValGRNEDate.Enabled = False
                cvGRNDate.Enabled = False

                If ViewState("type") = "DCITEMBYDPT" Then
                    Me.tr_15.Style("display") = ""
                    ValDptCode2.Enabled = True
                    lstItem.Value = ""
                    lstItem.Text = "---Select---"
                    Common.FillDefault(ddlDptCode, "COMPANY_DEPT_MSTR", "CDM_DEPT_CODE", "CDM_DEPT_CODE", , " CDM_COY_ID = '" & Session("CompanyId") & "' AND CDM_DELETED = 'N'")
                    ddlDptCode.Items.Insert(0, lstItem)
                End If

            ElseIf strRptType = "6" Then
                'Month, Year, Department Code
                Me.tr_4.Style("display") = ""
                Me.tr_7.Style("display") = ""
                rfvMonthFrom.Enabled = False
                rfvMonthTo.Enabled = False
                rfvYearFrom.Enabled = False
                rfvYearTo.Enabled = False
                ValDate.Enabled = False
                ValSDate.Enabled = False
                ValEDate.Enabled = False
                cvDate.Enabled = False
                ValGRNSDate.Enabled = False
                ValGRNEDate.Enabled = False
                cvGRNDate.Enabled = False
                ValDptCode2.Enabled = False

            ElseIf strRptType = "7" Then
                'Start Date, End Date, Item Code, Supplier Code, PO Balance Qty, Local/Oversea
                Me.tr_3.Style("display") = ""
                Me.tr_5.Style("display") = ""
                Me.tr_12.Style("display") = ""
                Me.tr_13.Style("display") = ""
                rfvMonthFrom.Enabled = False
                rfvMonthTo.Enabled = False
                rfvYearFrom.Enabled = False
                rfvYearTo.Enabled = False
                rfvMonth.Enabled = False
                rfvYear.Enabled = False
                ValDate.Enabled = False
                ValDptCode.Enabled = False
                ValDptCode2.Enabled = False
                ValGRNSDate.Enabled = False
                ValGRNEDate.Enabled = False
                cvGRNDate.Enabled = False

            ElseIf strRptType = "8" Then
                'Start Date, End Date, Storekeeper Name, Local/Oversea
                Me.tr_3.Style("display") = ""
                Me.tr_5.Style("display") = ""
                Me.tr_8.Style("display") = ""
                rfvMonthFrom.Enabled = False
                rfvMonthTo.Enabled = False
                rfvYearFrom.Enabled = False
                rfvYearTo.Enabled = False
                rfvMonth.Enabled = False
                rfvYear.Enabled = False
                ValDate.Enabled = False
                ValDptCode.Enabled = False
                ValDptCode2.Enabled = False
                ValGRNSDate.Enabled = False
                ValGRNEDate.Enabled = False
                cvGRNDate.Enabled = False

            ElseIf strRptType = "9" Then
                'Search By, Start Date, End Date, PO Number, Supplier Name
                Me.tr_3.Style("display") = ""
                Me.tr_11.Style("display") = ""
                Me.tr_10.Style("display") = ""
                rfvMonthFrom.Enabled = False
                rfvMonthTo.Enabled = False
                rfvYearFrom.Enabled = False
                rfvYearTo.Enabled = False
                rfvMonth.Enabled = False
                rfvYear.Enabled = False
                ValDate.Enabled = False
                ValDptCode.Enabled = False
                ValDptCode2.Enabled = False
                ValGRNSDate.Enabled = False
                ValGRNEDate.Enabled = False
                cvGRNDate.Enabled = False

                If ViewState("type") = "GRNLIST" Then
                    Me.tr_14.Style("display") = ""
                    ValGRNSDate.Enabled = True
                    ValGRNEDate.Enabled = True
                    cvGRNDate.Enabled = True
                End If

            ElseIf strRptType = "10" Then
                'End Date
                Me.tr_6.Style("display") = ""
                rfvMonthFrom.Enabled = False
                rfvMonthTo.Enabled = False
                rfvYearFrom.Enabled = False
                rfvYearTo.Enabled = False
                rfvMonth.Enabled = False
                rfvYear.Enabled = False
                ValDptCode.Enabled = False
                ValDptCode2.Enabled = False
                ValSDate.Enabled = False
                ValEDate.Enabled = False
                cvDate.Enabled = False
                ValGRNSDate.Enabled = False
                ValGRNEDate.Enabled = False
                cvGRNDate.Enabled = False

            ElseIf strRptType = "11" Then
                'Oversea/Local
                Me.tr_5.Style("display") = ""
                rfvMonthFrom.Enabled = False
                rfvMonthTo.Enabled = False
                rfvYearFrom.Enabled = False
                rfvYearTo.Enabled = False
                rfvMonth.Enabled = False
                rfvYear.Enabled = False
                ValDate.Enabled = False
                ValDptCode.Enabled = False
                ValDptCode2.Enabled = False
                ValSDate.Enabled = False
                ValEDate.Enabled = False
                cvDate.Enabled = False
                ValGRNSDate.Enabled = False
                ValGRNEDate.Enabled = False
                cvGRNDate.Enabled = False

            ElseIf strRptType = "12" Then
                'Oversea/Local, Start Date, End Date
                Me.tr_5.Style("display") = ""
                Me.tr_3.Style("display") = ""
                rfvMonthFrom.Enabled = False
                rfvMonthTo.Enabled = False
                rfvYearFrom.Enabled = False
                rfvYearTo.Enabled = False
                rfvMonth.Enabled = False
                rfvYear.Enabled = False
                ValDate.Enabled = False
                ValDptCode.Enabled = False
                ValDptCode2.Enabled = False
                ValGRNSDate.Enabled = False
                ValGRNEDate.Enabled = False
                cvGRNDate.Enabled = False

            ElseIf strRptType = "13" Then
                'Start Date, End Date, Item Code, Section Code
                Me.tr_3.Style("display") = ""
                Me.tr_9.Style("display") = ""
                rfvMonthFrom.Enabled = False
                rfvMonthTo.Enabled = False
                rfvYearFrom.Enabled = False
                rfvYearTo.Enabled = False
                rfvMonth.Enabled = False
                rfvYear.Enabled = False
                ValDate.Enabled = False
                ValDptCode.Enabled = False
                ValDptCode2.Enabled = False
                ValGRNSDate.Enabled = False
                ValGRNEDate.Enabled = False
                cvGRNDate.Enabled = False

            End If

            lstItem.Value = ""
            lstItem.Text = "---Select---"
            cmbYearFrom.Items.Insert(0, lstItem)
            cmbYearTo.Items.Insert(0, lstItem)
            cmbMonthFrom.Items.Insert(0, lstItem)
            cmbMonthTo.Items.Insert(0, lstItem)
            cmbMonth.Items.Insert(0, lstItem)
            cmbYear.Items.Insert(0, lstItem)

            'Year
            ii_ddl2 = 1
            jj_ddl = Year(Date.Now)
            For ii_ddl = 2002 To jj_ddl
                cmbYearFrom.Items.Insert(ii_ddl2, New ListItem(ii_ddl))
                cmbYearTo.Items.Insert(ii_ddl2, New ListItem(ii_ddl))
                cmbYear.Items.Insert(ii_ddl2, New ListItem(ii_ddl))
                ii_ddl2 = ii_ddl2 + 1
            Next

            'Month
            ii_ddl = 1
            jj_ddl = 12
            Dim strdate As String
            For ii_ddl = 1 To jj_ddl
                Dim lst As New ListItem
                strdate = "01/" & ii_ddl & "/2005"
                lst.Value = ii_ddl
                lst.Text = Format(CDate(strdate), "MMMM")
                cmbMonthFrom.Items.Insert(ii_ddl, lst)
                cmbMonthTo.Items.Insert(ii_ddl, lst)
                cmbMonth.Items.Insert(ii_ddl, lst)
            Next
        End If
        lnkBack.NavigateUrl = dispatcher.direct("Report", "ReportSelection.aspx", "pageid=" & strPageId & "&type=" & Request.QueryString("type")) ' send back data
        'cmdSubmit.Attributes.Add("onclick", "return compareDates();") ' if submit click go to method
    End Sub

    Private Sub ExportToTop20VendorPDF()
        Dim strSql, strBeginDate, strEndDate, strFileName As String
        Dim objFile As New FileManagement
        Dim dtDate, dtFrom, dtTo As Date
        Dim ds As New DataSet
        'Parameter
        Dim strPrmUserId, strPrmImgSrc, strPrmStart, strPrmEnd, strPrmCoyName As String

        'Get Company Logo
        strPrmImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        dtFrom = Me.txtSDate.Text
        dtTo = Me.txtEndDate.Text
        strBeginDate = Format(dtFrom, "yyyy-MM-dd" & " " & "00:00:00")
        strEndDate = Format(dtTo, "yyyy-MM-dd" & " " & "23:59:59")

        'Parameter
        strPrmStart = Format(dtFrom, "dd-MM-yyyy")
        strPrmEnd = Format(dtTo, "dd-MM-yyyy")
        strSql = "SELECT ROWNUM, A.CM_COY_NAME AS CM_COY_NAME, POM_PO_LOCAL_COST, POM_CURRENCY_CODE, POM_PO_COST FROM " & _
                "(SELECT @rownum:=@rownum+1 AS ROWNUM, tb.* FROM (SELECT (@rownum := 0) AS r, CM_COY_NAME, SUM(POM_PO_LOCAL_COST) AS POM_PO_LOCAL_COST " & _
                "FROM (SELECT PO_MSTR.*, CM_COY_NAME, (POM_PO_COST * IFNULL(POM_EXCHANGE_RATE,1)) AS POM_PO_LOCAL_COST FROM PO_MSTR " & _
                "INNER JOIN PO_DETAILS ON POM_B_COY_ID = POD_COY_ID AND POM_PO_NO = POD_PO_NO " & _
                "INNER JOIN COMPANY_MSTR ON POM_S_COY_ID = CM_COY_ID WHERE POM_B_COY_ID = '" & Session("CompanyID") & "' AND POD_ITEM_TYPE = 'SP' " & _
                "AND (POM_PO_STATUS = '1' OR POM_PO_STATUS = '2' OR POM_PO_STATUS = '3' OR POM_PO_STATUS = '6') " & _
                "AND POM_PO_DATE >= '" & strBeginDate & "' AND POM_PO_DATE <= '" & strEndDate & "' GROUP BY POM_PO_NO) tb " & _
                "GROUP BY CM_COY_NAME ORDER BY POM_PO_LOCAL_COST DESC LIMIT 20) tb) a " & _
                "INNER JOIN (SELECT CM_COY_NAME, SUM(POM_PO_COST) AS POM_PO_COST, POM_CURRENCY_CODE FROM " & _
                "(SELECT PO_MSTR.*, CM_COY_NAME FROM PO_MSTR INNER JOIN PO_DETAILS ON POM_B_COY_ID = POD_COY_ID " & _
                "AND POM_PO_NO = POD_PO_NO INNER JOIN COMPANY_MSTR ON POM_S_COY_ID = CM_COY_ID " & _
                "WHERE POM_B_COY_ID = '" & Session("CompanyID") & "' AND POD_ITEM_TYPE = 'SP' AND (POM_PO_STATUS = '1' OR POM_PO_STATUS = '2' " & _
                "OR POM_PO_STATUS = '3' OR POM_PO_STATUS = '6') AND POM_PO_DATE >= '" & strBeginDate & "' " & _
                "AND POM_PO_DATE <= '" & strEndDate & "' GROUP BY POM_PO_NO) tb GROUP BY CM_COY_NAME, POM_CURRENCY_CODE) b " & _
                "ON a.CM_COY_NAME = b.CM_COY_NAME ORDER BY POM_PO_LOCAL_COST DESC "

        ds = objDb.FillDs(strSql)

        strPrmUserId = Session("UserName")
        strPrmCoyName = Session("CompanyName")

        Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("TopVendor_DataSetTopVendor", ds.Tables(0))
        Dim localreport As New LocalReport
        localreport.DataSources.Clear()
        localreport.DataSources.Add(rptDataSource)
        localreport.ReportPath = dispatcher.direct("Report", "Top20Vendor.rdlc", "Report")
        localreport.EnableExternalImages = True

        Dim I As Byte
        Dim GetParameter As String = ""
        Dim TotalParameter As Byte
        Dim strID As String = ""

        strID = System.Configuration.ConfigurationManager.AppSettings.Get("Env") & "ReportCoyLogoPath"
        TotalParameter = localreport.GetParameters.Count
        Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
        'Dim paramlist As New Generic.List(Of ReportParameter)
        For I = 0 To localreport.GetParameters.Count - 1
            GetParameter = localreport.GetParameters.Item(I).Name
            Select Case LCase(GetParameter)
                Case "pmrequestedby"
                    par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strPrmUserId)

                Case "dtfrom"
                    par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strPrmStart)

                Case "dtto"
                    par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strPrmEnd)

                Case "logo"
                    par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings(strID) & strPrmImgSrc)

                Case "prmbuyercoyname"
                    par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strPrmCoyName)

                Case Else
            End Select
        Next
        localreport.SetParameters(par)

        localreport.Refresh()

        Dim deviceInfo As String = _
            "<DeviceInfo>" + _
                "  <OutputFormat>EMF</OutputFormat>" + _
                "</DeviceInfo>"
        System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
        Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
        System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
        strFileName = "Top20VendorsReport.pdf"
        Me.Response.Clear()
        Me.Response.ContentType = "application/pdf"
        Me.Response.AddHeader("Content-disposition", "attachment; filename=" & strFileName)
        Me.Response.BinaryWrite(PDF)
        Me.Response.End()

    End Sub

    Private Sub ExportToMonthlyStockReport1PDF()
        Dim strSql, strOversea, strMonth, strYear, strFileName As String
        Dim objFile As New FileManagement
        Dim dtDate, dtFrom, dtTo As Date
        Dim ds, dsTemp, dsGRN, dsMRS, dsWO As New DataSet
        Dim i, j As Integer

        'Parameter
        Dim strPrmUserId, strPrmImgSrc, strPrmYear, strPrmMonth, strPrmCoyName As String

        'Get Company Logo
        strPrmImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        strOversea = Me.cmbOversea.SelectedValue
        strMonth = Me.cmbMonth.SelectedValue
        strYear = Me.cmbYear.SelectedValue

        'Parameter
        If Me.cmbMonth.SelectedIndex > 0 Then
            dtDate = New DateTime(Me.cmbYear.SelectedValue, Me.cmbMonth.SelectedValue, 1)
        End If

        strPrmYear = Format(dtDate, "yyyy")
        strPrmMonth = Format(dtDate, "MMM")

        'GRN
        strSql = "SELECT PM_ACCT_CODE, IM_ITEM_CODE, PM_PRODUCT_DESC, PM_SPEC1, PM_SPEC2, PM_SPEC3, PM_UOM, " & _
                "DATE_FORMAT(IC_COST_DATE, '%Y-%m-%d') AS IC_COST_DATE, GROUP_CONCAT(IC_INVENTORY_REF_DOC SEPARATOR '\n') AS IC_INVENTORY_REF_DOC, " & _
                "SUM(IC_COST_QTY) AS IC_COST_QTY, SUM(IC_COST_COST) AS IC_COST_COST, GROUP_CONCAT(POM_VENDOR_CODE SEPARATOR '\n') AS POM_VENDOR_CODE, " & _
                "(SELECT IC_COST_OPEN_QTY FROM INVENTORY_COST " & _
                "WHERE IC_COY_ID = '" & Session("CompanyId") & "' AND MONTH(IC_COST_DATE) = '" & strMonth & "' AND YEAR(IC_COST_DATE) = '" & strYear & "' " & _
                "AND IC_INVENTORY_INDEX = tb.IC_INVENTORY_INDEX LIMIT 1) AS OPEN_QTY, " & _
                "(SELECT IC_COST_OPEN_COST FROM INVENTORY_COST " & _
                "WHERE IC_COY_ID = '" & Session("CompanyId") & "' AND MONTH(IC_COST_DATE) = '" & strMonth & "' AND YEAR(IC_COST_DATE) = '" & strYear & "' " & _
                "AND IC_INVENTORY_INDEX = tb.IC_INVENTORY_INDEX LIMIT 1) AS OPEN_COST " & _
                "FROM " & _
                "(SELECT IC_COST_DATE, IC_INVENTORY_REF_DOC, IM_ITEM_CODE, SUM(IC_COST_QTY) AS IC_COST_QTY, " & _
                "SUM(IC_COST_COST) AS IC_COST_COST, IFNULL(POM_VENDOR_CODE,'') AS POM_VENDOR_CODE, " & _
                "PM_PRODUCT_DESC, PM_ACCT_CODE, PM_UOM, PM_SPEC1, PM_SPEC2, PM_SPEC3, IC_INVENTORY_INDEX " & _
                "FROM INVENTORY_COST " & _
                "INNER JOIN INVENTORY_MSTR ON IM_INVENTORY_INDEX = IC_INVENTORY_INDEX " & _
                "INNER JOIN PRODUCT_MSTR ON PM_VENDOR_ITEM_CODE = IM_ITEM_CODE AND PM_S_COY_ID = IM_COY_ID " & _
                "INNER JOIN GRN_MSTR ON GM_GRN_NO = IC_INVENTORY_REF_DOC AND GM_B_COY_ID = IC_COY_ID " & _
                "INNER JOIN PO_MSTR ON POM_PO_INDEX = GM_PO_INDEX " & _
                "WHERE IC_COY_ID = '" & Session("CompanyId") & "' AND IC_INVENTORY_TYPE = 'GRN' " & _
                "AND MONTH(IC_COST_DATE) = '" & strMonth & "' AND YEAR(IC_COST_DATE) = '" & strYear & "' " & _
                "AND PM_ITEM_TYPE = 'ST' AND PM_OVERSEA = '" & strOversea & "' " & _
                "GROUP BY IC_INVENTORY_REF_DOC, IM_ITEM_CODE) tb " & _
                "GROUP BY DATE_FORMAT(IC_COST_DATE, '%Y-%m-%d'), IM_ITEM_CODE "
        dsGRN = objDb.FillDs(strSql)

        'MRS
        strSql = "SELECT PM_ACCT_CODE, IM_ITEM_CODE, PM_PRODUCT_DESC, PM_SPEC1, PM_SPEC2, PM_SPEC3, PM_UOM, " & _
                "DATE_FORMAT(IC_COST_DATE, '%Y-%m-%d') AS IC_COST_DATE, GROUP_CONCAT(IC_INVENTORY_REF_DOC SEPARATOR '\n') AS IC_INVENTORY_REF_DOC, " & _
                "SUM(IC_COST_QTY) AS IC_COST_QTY, SUM(IC_COST_COST) AS IC_COST_COST, GROUP_CONCAT(IRSM_IRS_SECTION SEPARATOR '\n') AS IRSM_IRS_SECTION, " & _
                "(SELECT IC_COST_OPEN_QTY FROM INVENTORY_COST " & _
                "WHERE IC_COY_ID = '" & Session("CompanyId") & "' AND MONTH(IC_COST_DATE) = '" & strMonth & "' AND YEAR(IC_COST_DATE) = '" & strYear & "' " & _
                "AND IC_INVENTORY_INDEX = tb.IC_INVENTORY_INDEX LIMIT 1) AS OPEN_QTY, " & _
                "(SELECT IC_COST_OPEN_COST FROM INVENTORY_COST " & _
                "WHERE IC_COY_ID = '" & Session("CompanyId") & "' AND MONTH(IC_COST_DATE) = '" & strMonth & "' AND YEAR(IC_COST_DATE) = '" & strYear & "' " & _
                "AND IC_INVENTORY_INDEX = tb.IC_INVENTORY_INDEX LIMIT 1) AS OPEN_COST " & _
                "FROM " & _
                "(SELECT IC_COST_DATE, IC_INVENTORY_REF_DOC, IM_ITEM_CODE, SUM(IC_COST_QTY) AS IC_COST_QTY, " & _
                "SUM(IC_COST_COST) AS IC_COST_COST, IFNULL(IRSM_IRS_SECTION,'') AS IRSM_IRS_SECTION, " & _
                "PM_PRODUCT_DESC, PM_ACCT_CODE, PM_UOM, PM_SPEC1, PM_SPEC2, PM_SPEC3, IC_INVENTORY_INDEX " & _
                "FROM INVENTORY_COST " & _
                "INNER JOIN INVENTORY_MSTR ON IM_INVENTORY_INDEX = IC_INVENTORY_INDEX " & _
                "INNER JOIN INVENTORY_REQUISITION_SLIP_MSTR ON IRSM_IRS_COY_ID = IC_COY_ID AND IRSM_IRS_NO = IC_INVENTORY_REF_DOC " & _
                "INNER JOIN PRODUCT_MSTR ON PM_VENDOR_ITEM_CODE = IM_ITEM_CODE AND PM_S_COY_ID = IM_COY_ID " & _
                "WHERE IC_COY_ID = '" & Session("CompanyId") & "' AND IC_INVENTORY_TYPE = 'II' " & _
                "AND MONTH(IC_COST_DATE) = '" & strMonth & "' AND YEAR(IC_COST_DATE) = '" & strYear & "' " & _
                "AND PM_ITEM_TYPE = 'ST' AND PM_OVERSEA = '" & strOversea & "' " & _
                "GROUP BY IC_INVENTORY_REF_DOC, IM_ITEM_CODE) tb " & _
                "GROUP BY DATE_FORMAT(IC_COST_DATE, '%Y-%m-%d'), IM_ITEM_CODE"
        dsMRS = objDb.FillDs(strSql)

        'WO
        strSql = "SELECT PM_ACCT_CODE, IM_ITEM_CODE, PM_PRODUCT_DESC, PM_SPEC1, PM_SPEC2, PM_SPEC3, PM_UOM, " & _
                "DATE_FORMAT(IC_COST_DATE, '%Y-%m-%d') AS IC_COST_DATE, GROUP_CONCAT(IC_INVENTORY_REF_DOC SEPARATOR '\n') AS IC_INVENTORY_REF_DOC, SUM(IC_COST_QTY) AS IC_COST_QTY, SUM(IC_COST_COST) AS IC_COST_COST, " & _
                "(SELECT IC_COST_OPEN_QTY FROM INVENTORY_COST " & _
                "WHERE IC_COY_ID = '" & Session("CompanyId") & "' AND MONTH(IC_COST_DATE) = '" & strMonth & "' AND YEAR(IC_COST_DATE) = '" & strYear & "' " & _
                "AND IC_INVENTORY_INDEX = tb.IC_INVENTORY_INDEX LIMIT 1) AS OPEN_QTY, " & _
                "(SELECT IC_COST_OPEN_COST FROM INVENTORY_COST " & _
                "WHERE IC_COY_ID = '" & Session("CompanyId") & "' AND MONTH(IC_COST_DATE) = '" & strMonth & "' AND YEAR(IC_COST_DATE) = '" & strYear & "' " & _
                "AND IC_INVENTORY_INDEX = tb.IC_INVENTORY_INDEX LIMIT 1) AS OPEN_COST " & _
                "FROM " & _
                "(SELECT IC_COST_DATE, IC_INVENTORY_REF_DOC, IM_ITEM_CODE, SUM(IC_COST_QTY) AS IC_COST_QTY, SUM(IC_COST_COST) AS IC_COST_COST, " & _
                "PM_PRODUCT_DESC, PM_ACCT_CODE, PM_UOM, PM_SPEC1, PM_SPEC2, PM_SPEC3, IC_INVENTORY_INDEX " & _
                "FROM INVENTORY_COST " & _
                "INNER JOIN INVENTORY_MSTR ON IM_INVENTORY_INDEX = IC_INVENTORY_INDEX " & _
                "INNER JOIN PRODUCT_MSTR ON PM_VENDOR_ITEM_CODE = IM_ITEM_CODE AND PM_S_COY_ID = IM_COY_ID " & _
                "WHERE IC_COY_ID = '" & Session("CompanyId") & "' AND IC_INVENTORY_TYPE = 'WO' " & _
                "AND MONTH(IC_COST_DATE) = '" & strMonth & "' AND YEAR(IC_COST_DATE) = '" & strYear & "' " & _
                "AND PM_ITEM_TYPE = 'ST' AND PM_OVERSEA = '" & strOversea & "' " & _
                "GROUP BY IC_INVENTORY_REF_DOC, IM_ITEM_CODE) tb " & _
                "GROUP BY DATE_FORMAT(IC_COST_DATE, '%Y-%m-%d'), IM_ITEM_CODE"
        dsWO = objDb.FillDs(strSql)

        Dim dt As New DataTable
        Dim dtr As DataRow
        dt.Columns.Add("ACCT_CODE", Type.GetType("System.String"))
        dt.Columns.Add("ITEM_CODE", Type.GetType("System.String"))
        dt.Columns.Add("PRODUCT_DESC", Type.GetType("System.String"))
        dt.Columns.Add("SPEC1", Type.GetType("System.String"))
        dt.Columns.Add("SPEC2", Type.GetType("System.String"))
        dt.Columns.Add("SPEC3", Type.GetType("System.String"))
        dt.Columns.Add("UOM", Type.GetType("System.String"))
        'dt.Columns.Add("OPEN_QTY", Type.GetType("System.String"))
        'dt.Columns.Add("OPEN_COST", Type.GetType("System.String"))
        dt.Columns.Add("DOC_DATE", Type.GetType("System.String"))
        dt.Columns.Add("GRN_NO", Type.GetType("System.String"))
        dt.Columns.Add("MRS_NO", Type.GetType("System.String"))
        dt.Columns.Add("WO_NO", Type.GetType("System.String"))
        dt.Columns.Add("SEC_CODE", Type.GetType("System.String"))
        dt.Columns.Add("VENDOR_CODE", Type.GetType("System.String"))
        dt.Columns.Add("GRN_QTY", Type.GetType("System.String"))
        dt.Columns.Add("GRN_COST", Type.GetType("System.String"))
        dt.Columns.Add("MRS_QTY", Type.GetType("System.String"))
        dt.Columns.Add("MRS_COST", Type.GetType("System.String"))
        dt.Columns.Add("WO_QTY", Type.GetType("System.String"))
        dt.Columns.Add("WO_COST", Type.GetType("System.String"))

        For i = 0 To dsGRN.Tables(0).Rows.Count - 1
            dtr = dt.NewRow
            dtr("ACCT_CODE") = dsGRN.Tables(0).Rows(i)("PM_ACCT_CODE")
            dtr("ITEM_CODE") = dsGRN.Tables(0).Rows(i)("IM_ITEM_CODE")
            dtr("PRODUCT_DESC") = dsGRN.Tables(0).Rows(i)("PM_PRODUCT_DESC")
            dtr("SPEC1") = dsGRN.Tables(0).Rows(i)("PM_SPEC1")
            dtr("SPEC2") = dsGRN.Tables(0).Rows(i)("PM_SPEC2")
            dtr("SPEC3") = dsGRN.Tables(0).Rows(i)("PM_SPEC3")
            dtr("UOM") = dsGRN.Tables(0).Rows(i)("PM_UOM")
            'dtr("OPEN_QTY") = dsGRN.Tables(0).Rows(i)("OPEN_QTY")
            'dtr("OPEN_COST") = dsGRN.Tables(0).Rows(i)("OPEN_COST")
            dtr("DOC_DATE") = dsGRN.Tables(0).Rows(i)("IC_COST_DATE")
            dtr("GRN_NO") = dsGRN.Tables(0).Rows(i)("IC_INVENTORY_REF_DOC")
            dtr("VENDOR_CODE") = dsGRN.Tables(0).Rows(i)("POM_VENDOR_CODE")
            dtr("GRN_QTY") = dsGRN.Tables(0).Rows(i)("IC_COST_QTY")
            dtr("GRN_COST") = dsGRN.Tables(0).Rows(i)("IC_COST_COST")
            dt.Rows.Add(dtr)
        Next

        For i = 0 To dsMRS.Tables(0).Rows.Count - 1
            If dt.Select("ITEM_CODE='" & dsMRS.Tables(0).Rows(i)("IM_ITEM_CODE") & "' AND DOC_DATE = '" & dsMRS.Tables(0).Rows(i)("IC_COST_DATE") & "' AND MRS_NO IS NULL").Length > 0 Then
                For j = 0 To dt.Rows.Count - 1
                    If dt.Rows(j)("ITEM_CODE") = dsMRS.Tables(0).Rows(i)("IM_ITEM_CODE") And dt.Rows(j)("DOC_DATE") = dsMRS.Tables(0).Rows(i)("IC_COST_DATE") And Common.parseNull(dt.Rows(j)("MRS_NO")) = "" Then
                        dt.Rows(j)("MRS_NO") = dsMRS.Tables(0).Rows(i)("IC_INVENTORY_REF_DOC")
                        dt.Rows(j)("SEC_CODE") = dsMRS.Tables(0).Rows(i)("IRSM_IRS_SECTION")
                        dt.Rows(j)("MRS_QTY") = dsMRS.Tables(0).Rows(i)("IC_COST_QTY")
                        dt.Rows(j)("MRS_COST") = dsMRS.Tables(0).Rows(i)("IC_COST_COST")
                        dt.AcceptChanges()
                        Exit For
                    End If
                Next
            Else
                dtr = dt.NewRow
                dtr("ACCT_CODE") = dsMRS.Tables(0).Rows(i)("PM_ACCT_CODE")
                dtr("ITEM_CODE") = dsMRS.Tables(0).Rows(i)("IM_ITEM_CODE")
                dtr("PRODUCT_DESC") = dsMRS.Tables(0).Rows(i)("PM_PRODUCT_DESC")
                dtr("SPEC1") = dsMRS.Tables(0).Rows(i)("PM_SPEC1")
                dtr("SPEC2") = dsMRS.Tables(0).Rows(i)("PM_SPEC2")
                dtr("SPEC3") = dsMRS.Tables(0).Rows(i)("PM_SPEC3")
                dtr("UOM") = dsMRS.Tables(0).Rows(i)("PM_UOM")
                dtr("DOC_DATE") = dsMRS.Tables(0).Rows(i)("IC_COST_DATE")
                dtr("MRS_NO") = dsMRS.Tables(0).Rows(i)("IC_INVENTORY_REF_DOC")
                dtr("SEC_CODE") = dsMRS.Tables(0).Rows(i)("IRSM_IRS_SECTION")
                dtr("MRS_QTY") = dsMRS.Tables(0).Rows(i)("IC_COST_QTY")
                dtr("MRS_COST") = dsMRS.Tables(0).Rows(i)("IC_COST_COST")
                dt.Rows.Add(dtr)
            End If
        Next

        For i = 0 To dsWO.Tables(0).Rows.Count - 1
            If dt.Select("ITEM_CODE='" & dsWO.Tables(0).Rows(i)("IM_ITEM_CODE") & "' AND DOC_DATE = '" & dsWO.Tables(0).Rows(i)("IC_COST_DATE") & "' AND WO_NO IS NULL").Length > 0 Then
                For j = 0 To dt.Rows.Count - 1
                    If dt.Rows(j)("ITEM_CODE") = dsWO.Tables(0).Rows(i)("IM_ITEM_CODE") And dt.Rows(j)("DOC_DATE") = dsWO.Tables(0).Rows(i)("IC_COST_DATE") And Common.parseNull(dt.Rows(j)("WO_NO")) = "" Then
                        dt.Rows(j)("WO_NO") = dsWO.Tables(0).Rows(i)("IC_INVENTORY_REF_DOC")
                        dt.Rows(j)("WO_QTY") = dsWO.Tables(0).Rows(i)("IC_COST_QTY")
                        dt.Rows(j)("WO_COST") = dsWO.Tables(0).Rows(i)("IC_COST_COST")
                        dt.AcceptChanges()
                        Exit For
                    End If
                Next
            Else
                dtr = dt.NewRow
                dtr("ACCT_CODE") = dsWO.Tables(0).Rows(i)("PM_ACCT_CODE")
                dtr("ITEM_CODE") = dsWO.Tables(0).Rows(i)("IM_ITEM_CODE")
                dtr("PRODUCT_DESC") = dsWO.Tables(0).Rows(i)("PM_PRODUCT_DESC")
                dtr("SPEC1") = dsWO.Tables(0).Rows(i)("PM_SPEC1")
                dtr("SPEC2") = dsWO.Tables(0).Rows(i)("PM_SPEC2")
                dtr("SPEC3") = dsWO.Tables(0).Rows(i)("PM_SPEC3")
                dtr("UOM") = dsWO.Tables(0).Rows(i)("PM_UOM")
                dtr("DOC_DATE") = dsWO.Tables(0).Rows(i)("IC_COST_DATE")
                dtr("WO_NO") = dsWO.Tables(0).Rows(i)("IC_INVENTORY_REF_DOC")
                dtr("WO_QTY") = dsWO.Tables(0).Rows(i)("IC_COST_QTY")
                dtr("WO_COST") = dsWO.Tables(0).Rows(i)("IC_COST_COST")
                dt.Rows.Add(dtr)
            End If
        Next

        Dim dtrt() As DataRow
        Dim dr As DataRow
        dsTemp.Tables.Add(dt)
        dtrt = dsTemp.Tables(0).Select("", "ACCT_CODE ASC, ITEM_CODE ASC, DOC_DATE ASC")
        ds = dsTemp.Clone
        For Each dr In dtrt
            ds.Tables(0).ImportRow(dr)
        Next

        strPrmUserId = Session("UserName")
        strPrmCoyName = Session("CompanyName")

        Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("MonthlyStockReport1_DataSetMonthlyStockReport1", ds.Tables(0))
        Dim localreport As New LocalReport
        localreport.DataSources.Clear()
        localreport.DataSources.Add(rptDataSource)
        localreport.ReportPath = dispatcher.direct("Report", "MonthlyStockReport1.rdlc", "Report")
        localreport.EnableExternalImages = True

        Dim GetParameter As String = ""
        Dim TotalParameter As Byte
        Dim strID As String = ""

        strID = System.Configuration.ConfigurationManager.AppSettings.Get("Env") & "ReportCoyLogoPath"
        TotalParameter = localreport.GetParameters.Count
        Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
        'Dim paramlist As New Generic.List(Of ReportParameter)
        For i = 0 To localreport.GetParameters.Count - 1
            GetParameter = localreport.GetParameters.Item(i).Name
            Select Case LCase(GetParameter)
                Case "pmrequestedby"
                    par(i) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strPrmUserId)

                Case "prmyear"
                    par(i) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strPrmYear)

                Case "prmmonth"
                    par(i) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strPrmMonth)

                Case "logo"
                    par(i) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings(strID) & strPrmImgSrc)

                Case "prmbuyercoyname"
                    par(i) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strPrmCoyName)

                Case "oversea"
                    par(i) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, cmbOversea.SelectedItem.Text)

                Case Else
            End Select
        Next
        localreport.SetParameters(par)

        localreport.Refresh()

        Dim deviceInfo As String = _
            "<DeviceInfo>" + _
                "  <OutputFormat>EMF</OutputFormat>" + _
                "</DeviceInfo>"
        System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
        Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
        System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
        strFileName = "MonthlyStockReport1.pdf"
        Me.Response.Clear()
        Me.Response.ContentType = "application/pdf"
        Me.Response.AddHeader("Content-disposition", "attachment; filename=" & strFileName)
        Me.Response.BinaryWrite(PDF)
        Me.Response.End()

    End Sub

    Private Sub ExportToMonthlyStockReport2PDF()
        Dim strSql, strOversea, strMonth, strYear, strFileName As String
        Dim objFile As New FileManagement
        Dim dtDate, dtFrom, dtTo As Date
        Dim ds, dsTemp, dsGRN, dsMRS, dsWO As New DataSet
        Dim i, j As Integer

        'Parameter
        Dim strPrmUserId, strPrmImgSrc, strPrmYear, strPrmMonth, strPrmCoyName As String

        'Get Company Logo
        strPrmImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        strOversea = Me.cmbOversea.SelectedValue
        strMonth = Me.cmbMonth.SelectedValue
        strYear = Me.cmbYear.SelectedValue

        'Parameter
        If Me.cmbMonth.SelectedIndex > 0 Then
            dtDate = New DateTime(Me.cmbYear.SelectedValue, Me.cmbMonth.SelectedValue, 1)
        End If

        strPrmYear = Format(dtDate, "yyyy")
        strPrmMonth = Format(dtDate, "MMM")

        'GRN
        strSql = "SELECT PM_ACCT_CODE, POD_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_SPEC1, PM_SPEC2, PM_SPEC3, PM_UOM, " & _
                "DATE_FORMAT(GM_DATE_RECEIVED, '%Y-%m-%d') AS DOC_DATE, GROUP_CONCAT(GD_GRN_NO SEPARATOR '\n') AS GRN_NO, " & _
                "SUM(GRN_QTY) AS GRN_QTY, GROUP_CONCAT(DOM_DO_NO SEPARATOR '\n') AS DO_NO, GROUP_CONCAT(POM_VENDOR_CODE SEPARATOR '\n') AS VENDOR_CODE " & _
                "FROM " & _
                "(SELECT GM_DATE_RECEIVED, GD_GRN_NO, POD_VENDOR_ITEM_CODE, " & _
                "SUM(GD_RECEIVED_QTY - GD_REJECTED_QTY) AS GRN_QTY, DOM_DO_NO, IFNULL(POM_VENDOR_CODE,'') AS POM_VENDOR_CODE, " & _
                "PM_PRODUCT_DESC, PM_ACCT_CODE, PM_UOM, PM_SPEC1, PM_SPEC2, PM_SPEC3 " & _
                "FROM GRN_MSTR " & _
                "INNER JOIN GRN_DETAILS ON GD_GRN_NO = GM_GRN_NO AND GD_B_COY_ID = GM_B_COY_ID " & _
                "INNER JOIN DO_MSTR ON DOM_DO_INDEX = GM_DO_INDEX " & _
                "INNER JOIN PO_MSTR ON POM_PO_INDEX = GM_PO_INDEX " & _
                "INNER JOIN PO_DETAILS ON POD_COY_ID = POM_B_COY_ID AND POD_PO_NO = POM_PO_NO AND POD_PO_LINE = GD_PO_LINE " & _
                "INNER JOIN PRODUCT_MSTR ON PM_S_COY_ID = POM_B_COY_ID AND PM_VENDOR_ITEM_CODE = POD_VENDOR_ITEM_CODE " & _
                "WHERE GM_B_COY_ID = '" & Session("CompanyId") & "' AND MONTH(GM_DATE_RECEIVED) = '" & strMonth & "' AND YEAR(GM_DATE_RECEIVED) = '" & strYear & "' " & _
                "AND PM_ITEM_TYPE = 'ST' AND PM_OVERSEA = '" & strOversea & "' " & _
                "GROUP BY GD_GRN_NO, POD_VENDOR_ITEM_CODE) tb " & _
                "GROUP BY DATE_FORMAT(GM_DATE_RECEIVED, '%Y-%m-%d'), POD_VENDOR_ITEM_CODE "
        dsGRN = objDb.FillDs(strSql)

        'MRS
        strSql = "SELECT PM_ACCT_CODE, IM_ITEM_CODE, PM_PRODUCT_DESC, PM_SPEC1, PM_SPEC2, PM_SPEC3, PM_UOM, " & _
                "DATE_FORMAT(IRSM_IRS_APPROVED_DATE, '%Y-%m-%d') AS DOC_DATE, GROUP_CONCAT(IRSD_IRS_NO SEPARATOR '\n') AS MRS_NO, " & _
                "SUM(MRS_QTY) AS MRS_QTY, GROUP_CONCAT(IRSM_IRS_SECTION SEPARATOR '\n') AS SEC_CODE, GROUP_CONCAT(IRD_IR_NO SEPARATOR '\n') AS IR_NO " & _
                "FROM " & _
                "(SELECT IRSM_IRS_APPROVED_DATE, IRSD_IRS_NO, IM_ITEM_CODE, " & _
                "SUM(IRSD_QTY) AS MRS_QTY, IRSM_IRS_SECTION, IRD_IR_NO, " & _
                "PM_PRODUCT_DESC, PM_ACCT_CODE, PM_UOM, PM_SPEC1, PM_SPEC2, PM_SPEC3 " & _
                "FROM INVENTORY_REQUISITION_SLIP_MSTR " & _
                "INNER JOIN INVENTORY_REQUISITION_SLIP_DETAILS ON IRSD_IRS_COY_ID = IRSM_IRS_COY_ID AND IRSD_IRS_NO = IRSM_IRS_NO " & _
                "INNER JOIN INVENTORY_MSTR ON IM_INVENTORY_INDEX = IRSD_INVENTORY_INDEX " & _
                "INNER JOIN INVENTORY_REQUISITION_DETAILS ON IRD_IR_SLIP_INDEX = IRSM_IRS_INDEX AND IRD_IR_LINE = IRSD_IRS_LINE " & _
                "INNER JOIN PRODUCT_MSTR ON PM_VENDOR_ITEM_CODE = IM_ITEM_CODE AND PM_S_COY_ID = IM_COY_ID " & _
                "WHERE IRSM_IRS_COY_ID = '" & Session("CompanyId") & "' AND MONTH(IRSM_IRS_DATE) = '" & strMonth & "' AND YEAR(IRSM_IRS_DATE) = '" & strYear & "' AND IRSM_IRS_APPROVED_DATE IS NOT NULL " & _
                "AND PM_ITEM_TYPE = 'ST' AND PM_OVERSEA = '" & strOversea & "' " & _
                "GROUP BY IRSD_IRS_NO, IM_ITEM_CODE) tb " & _
                "GROUP BY DATE_FORMAT(IRSM_IRS_APPROVED_DATE, '%Y-%m-%d'), IM_ITEM_CODE "
        dsMRS = objDb.FillDs(strSql)

        'WO
        strSql = "SELECT DATE_FORMAT(IWOM_WO_DATE, '%Y-%m-%d') AS DOC_DATE, IM_ITEM_CODE, GROUP_CONCAT(IWOD_WO_NO SEPARATOR '\n') AS WO_NO, " & _
                "SUM(WO_QTY) AS WO_QTY, PM_PRODUCT_DESC, PM_ACCT_CODE, PM_UOM, PM_SPEC1, PM_SPEC2, PM_SPEC3 " & _
                "FROM " & _
                "(SELECT IWOM_WO_DATE, IWOD_WO_NO, IM_ITEM_CODE, SUM(IWOD_QTY_VAL) AS WO_QTY, " & _
                "PM_PRODUCT_DESC, PM_ACCT_CODE, PM_UOM, PM_SPEC1, PM_SPEC2, PM_SPEC3 " & _
                "FROM INVENTORY_WRITE_OFF_MSTR " & _
                "INNER JOIN INVENTORY_WRITE_OFF_DETAILS ON IWOD_WO_NO = IWOM_WO_NO AND IWOD_WO_COY_ID = IWOM_WO_COY_ID " & _
                "INNER JOIN INVENTORY_MSTR ON IM_INVENTORY_INDEX = IWOD_INVENTORY_INDEX " & _
                "INNER JOIN PRODUCT_MSTR ON PM_VENDOR_ITEM_CODE = IM_ITEM_CODE AND PM_S_COY_ID = IM_COY_ID " & _
                "WHERE IWOM_WO_COY_ID = '" & Session("CompanyId") & "' AND MONTH(IWOM_WO_DATE) = '" & strMonth & "' AND YEAR(IWOM_WO_DATE) = '" & strYear & "' " & _
                "AND PM_ITEM_TYPE = 'ST' AND PM_OVERSEA = '" & strOversea & "' " & _
                "GROUP BY IWOD_WO_NO, IM_ITEM_CODE) tb " & _
                "GROUP BY DATE_FORMAT(IWOM_WO_DATE, '%Y-%m-%d'), IM_ITEM_CODE "
        dsWO = objDb.FillDs(strSql)

        Dim dt As New DataTable
        Dim dtr As DataRow
        dt.Columns.Add("ACCT_CODE", Type.GetType("System.String"))
        dt.Columns.Add("ITEM_CODE", Type.GetType("System.String"))
        dt.Columns.Add("PRODUCT_DESC", Type.GetType("System.String"))
        dt.Columns.Add("SPEC1", Type.GetType("System.String"))
        dt.Columns.Add("SPEC2", Type.GetType("System.String"))
        dt.Columns.Add("SPEC3", Type.GetType("System.String"))
        dt.Columns.Add("UOM", Type.GetType("System.String"))
        dt.Columns.Add("DOC_DATE", Type.GetType("System.String"))
        dt.Columns.Add("GRN_NO", Type.GetType("System.String"))
        dt.Columns.Add("MRS_NO", Type.GetType("System.String"))
        dt.Columns.Add("WO_NO", Type.GetType("System.String"))
        dt.Columns.Add("SEC_CODE", Type.GetType("System.String"))
        dt.Columns.Add("VENDOR_CODE", Type.GetType("System.String"))
        dt.Columns.Add("IR_NO", Type.GetType("System.String"))
        dt.Columns.Add("DO_NO", Type.GetType("System.String"))
        dt.Columns.Add("GRN_QTY", Type.GetType("System.String"))
        dt.Columns.Add("MRS_QTY", Type.GetType("System.String"))
        dt.Columns.Add("WO_QTY", Type.GetType("System.String"))

        For i = 0 To dsGRN.Tables(0).Rows.Count - 1
            dtr = dt.NewRow
            dtr("ACCT_CODE") = dsGRN.Tables(0).Rows(i)("PM_ACCT_CODE")
            dtr("ITEM_CODE") = dsGRN.Tables(0).Rows(i)("POD_VENDOR_ITEM_CODE")
            dtr("PRODUCT_DESC") = dsGRN.Tables(0).Rows(i)("PM_PRODUCT_DESC")
            dtr("SPEC1") = dsGRN.Tables(0).Rows(i)("PM_SPEC1")
            dtr("SPEC2") = dsGRN.Tables(0).Rows(i)("PM_SPEC2")
            dtr("SPEC3") = dsGRN.Tables(0).Rows(i)("PM_SPEC3")
            dtr("UOM") = dsGRN.Tables(0).Rows(i)("PM_UOM")
            dtr("DOC_DATE") = dsGRN.Tables(0).Rows(i)("DOC_DATE")
            dtr("GRN_NO") = dsGRN.Tables(0).Rows(i)("GRN_NO")
            dtr("VENDOR_CODE") = dsGRN.Tables(0).Rows(i)("VENDOR_CODE")
            dtr("DO_NO") = dsGRN.Tables(0).Rows(i)("DO_NO")
            dtr("GRN_QTY") = dsGRN.Tables(0).Rows(i)("GRN_QTY")
            dt.Rows.Add(dtr)
        Next

        For i = 0 To dsMRS.Tables(0).Rows.Count - 1
            If dt.Select("ITEM_CODE='" & dsMRS.Tables(0).Rows(i)("IM_ITEM_CODE") & "' AND DOC_DATE = '" & dsMRS.Tables(0).Rows(i)("DOC_DATE") & "' AND MRS_NO IS NULL").Length > 0 Then
                For j = 0 To dt.Rows.Count - 1
                    If dt.Rows(j)("ITEM_CODE") = dsMRS.Tables(0).Rows(i)("IM_ITEM_CODE") And dt.Rows(j)("DOC_DATE") = dsMRS.Tables(0).Rows(i)("DOC_DATE") And Common.parseNull(dt.Rows(j)("MRS_NO")) = "" Then
                        dt.Rows(j)("MRS_NO") = dsMRS.Tables(0).Rows(i)("MRS_NO")
                        dt.Rows(j)("IR_NO") = dsMRS.Tables(0).Rows(i)("IR_NO")
                        dt.Rows(j)("SEC_CODE") = dsMRS.Tables(0).Rows(i)("SEC_CODE")
                        dt.Rows(j)("MRS_QTY") = dsMRS.Tables(0).Rows(i)("MRS_QTY")
                        dt.AcceptChanges()
                        Exit For
                    End If
                Next
            Else
                dtr = dt.NewRow
                dtr("ACCT_CODE") = dsMRS.Tables(0).Rows(i)("PM_ACCT_CODE")
                dtr("ITEM_CODE") = dsMRS.Tables(0).Rows(i)("IM_ITEM_CODE")
                dtr("PRODUCT_DESC") = dsMRS.Tables(0).Rows(i)("PM_PRODUCT_DESC")
                dtr("SPEC1") = dsMRS.Tables(0).Rows(i)("PM_SPEC1")
                dtr("SPEC2") = dsMRS.Tables(0).Rows(i)("PM_SPEC2")
                dtr("SPEC3") = dsMRS.Tables(0).Rows(i)("PM_SPEC3")
                dtr("UOM") = dsMRS.Tables(0).Rows(i)("PM_UOM")
                dtr("DOC_DATE") = dsMRS.Tables(0).Rows(i)("DOC_DATE")
                dtr("MRS_NO") = dsMRS.Tables(0).Rows(i)("MRS_NO")
                dtr("IR_NO") = dsMRS.Tables(0).Rows(i)("IR_NO")
                dtr("SEC_CODE") = dsMRS.Tables(0).Rows(i)("SEC_CODE")
                dtr("MRS_QTY") = dsMRS.Tables(0).Rows(i)("MRS_QTY")
                dt.Rows.Add(dtr)
            End If
        Next

        For i = 0 To dsWO.Tables(0).Rows.Count - 1
            If dt.Select("ITEM_CODE='" & dsWO.Tables(0).Rows(i)("IM_ITEM_CODE") & "' AND DOC_DATE = '" & dsWO.Tables(0).Rows(i)("DOC_DATE") & "' AND WO_NO IS NULL").Length > 0 Then
                For j = 0 To dt.Rows.Count - 1
                    If dt.Rows(j)("ITEM_CODE") = dsWO.Tables(0).Rows(i)("IM_ITEM_CODE") And dt.Rows(j)("DOC_DATE") = dsWO.Tables(0).Rows(i)("DOC_DATE") And Common.parseNull(dt.Rows(j)("WO_NO")) = "" Then
                        dt.Rows(j)("WO_NO") = dsWO.Tables(0).Rows(i)("WO_NO")
                        dt.Rows(j)("WO_QTY") = dsWO.Tables(0).Rows(i)("WO_QTY")
                        dt.AcceptChanges()
                        Exit For
                    End If
                Next
            Else
                dtr = dt.NewRow
                dtr("ACCT_CODE") = dsWO.Tables(0).Rows(i)("PM_ACCT_CODE")
                dtr("ITEM_CODE") = dsWO.Tables(0).Rows(i)("IM_ITEM_CODE")
                dtr("PRODUCT_DESC") = dsWO.Tables(0).Rows(i)("PM_PRODUCT_DESC")
                dtr("SPEC1") = dsWO.Tables(0).Rows(i)("PM_SPEC1")
                dtr("SPEC2") = dsWO.Tables(0).Rows(i)("PM_SPEC2")
                dtr("SPEC3") = dsWO.Tables(0).Rows(i)("PM_SPEC3")
                dtr("UOM") = dsWO.Tables(0).Rows(i)("PM_UOM")
                dtr("DOC_DATE") = dsWO.Tables(0).Rows(i)("DOC_DATE")
                dtr("WO_NO") = dsWO.Tables(0).Rows(i)("WO_NO")
                dtr("WO_QTY") = dsWO.Tables(0).Rows(i)("WO_QTY")
                dt.Rows.Add(dtr)
            End If
        Next

        Dim dtrt() As DataRow
        Dim dr As DataRow
        dsTemp.Tables.Add(dt)
        dtrt = dsTemp.Tables(0).Select("", "ACCT_CODE ASC, ITEM_CODE ASC, DOC_DATE ASC")
        ds = dsTemp.Clone
        For Each dr In dtrt
            ds.Tables(0).ImportRow(dr)
        Next

        strPrmUserId = Session("UserName")
        strPrmCoyName = Session("CompanyName")

        Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("MonthlyStockReport2_DataSetMonthlyStockReport2", ds.Tables(0))
        Dim localreport As New LocalReport
        localreport.DataSources.Clear()
        localreport.DataSources.Add(rptDataSource)
        localreport.ReportPath = dispatcher.direct("Report", "MonthlyStockReport2.rdlc", "Report")
        localreport.EnableExternalImages = True

        Dim GetParameter As String = ""
        Dim TotalParameter As Byte
        Dim strID As String = ""

        strID = System.Configuration.ConfigurationManager.AppSettings.Get("Env") & "ReportCoyLogoPath"
        TotalParameter = localreport.GetParameters.Count
        Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
        'Dim paramlist As New Generic.List(Of ReportParameter)
        For i = 0 To localreport.GetParameters.Count - 1
            GetParameter = localreport.GetParameters.Item(i).Name
            Select Case LCase(GetParameter)
                Case "pmrequestedby"
                    par(i) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strPrmUserId)

                Case "prmyear"
                    par(i) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strPrmYear)

                Case "prmmonth"
                    par(i) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strPrmMonth)

                Case "logo"
                    par(i) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings(strID) & strPrmImgSrc)

                Case "prmbuyercoyname"
                    par(i) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strPrmCoyName)

                Case "oversea"
                    par(i) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, cmbOversea.SelectedItem.Text)

                Case Else
            End Select
        Next
        localreport.SetParameters(par)

        localreport.Refresh()

        Dim deviceInfo As String = _
            "<DeviceInfo>" + _
                "  <OutputFormat>EMF</OutputFormat>" + _
                "</DeviceInfo>"
        System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
        Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
        System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
        strFileName = "MonthlyStockReport2.pdf"
        Me.Response.Clear()
        Me.Response.ContentType = "application/pdf"
        Me.Response.AddHeader("Content-disposition", "attachment; filename=" & strFileName)
        Me.Response.BinaryWrite(PDF)
        Me.Response.End()

    End Sub

    Private Sub ExportToPOReportSpotPDF()
        Dim strSql, strBeginDate, strEndDate, strFileName As String
        Dim objFile As New FileManagement
        Dim dtDate, dtFrom, dtTo As Date
        Dim ds As New DataSet
        'Parameter
        Dim strPrmUserId, strPrmImgSrc, strPrmStart, strPrmEnd, strPrmCoyName As String

        'Get Company Logo
        strPrmImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        dtFrom = Me.txtSDate.Text
        dtTo = Me.txtEndDate.Text
        strBeginDate = Format(dtFrom, "yyyy-MM-dd" & " " & "00:00:00")
        strEndDate = Format(dtTo, "yyyy-MM-dd" & " " & "23:59:59")
        ViewState("startdate") = strBeginDate
        ViewState("enddate") = strEndDate

        'Parameter
        strPrmStart = Format(dtFrom, "dd-MM-yyyy")
        strPrmEnd = Format(dtTo, "dd-MM-yyyy")
        strSql = "SELECT SUM(NO_PR) AS NO_PR, COUNT(POD_PO_NO) AS NO_PO, CM_COY_NAME, POD_OVERSEA, " & _
                "POM_CURRENCY_CODE, ACCT_CODE, AM_ACCT_DESC, SUM(POD_UNIT_AMT) AS POD_UNIT_AMT, SUM(POD_L_UNIT_AMT) AS POD_L_UNIT_AMT FROM " & _
                "(SELECT (IFNULL(NO_PR_PO,0) + IFNULL(NO_PR_RFQ,0)) AS NO_PR, POD_PO_NO, POM_S_COY_ID, POD_OVERSEA, " & _
                "POM_CURRENCY_CODE, POD_ACCT_CODE AS ACCT_CODE, AM_ACCT_DESC, SUM(POD_UNIT_AMT) AS POD_UNIT_AMT, " & _
                "SUM(POD_UNIT_AMT * POM_EXCHANGE_RATE) AS POD_L_UNIT_AMT FROM " & _
                "(SELECT POD_PO_NO, POD_VENDOR_ITEM_CODE, POD_PO_LINE, IFNULL(RM_RFQ_NO,'') AS POM_RFQ_NO, POM_S_COY_ID, POD_OVERSEA, POM_CURRENCY_CODE, " & _
                "IFNULL(AM_ACCT_CODE,'') AS POD_ACCT_CODE, IFNULL(AM_ACCT_DESC,'') AS AM_ACCT_DESC, " & _
                "(POD_UNIT_COST * POD_ORDERED_QTY) + ((POD_UNIT_COST * POD_ORDERED_QTY) / 100 * POD_GST) AS POD_UNIT_AMT, " & _
                "IFNULL(POM_EXCHANGE_RATE,1) AS POM_EXCHANGE_RATE " & _
                "FROM PO_DETAILS " & _
                "INNER JOIN PO_MSTR ON POD_PO_NO = POM_PO_NO AND POM_B_COY_ID = POD_COY_ID " & _
                "LEFT JOIN RFQ_MSTR ON POM_RFQ_INDEX = RM_RFQ_ID " & _
                "LEFT JOIN ACCOUNT_MSTR ON POD_ACCT_INDEX = AM_ACCT_INDEX " & _
                "WHERE POD_ITEM_TYPE = 'SP' AND POD_COY_ID = '" & Session("CompanyID") & "' AND POM_PO_DATE IS NOT NULL " & _
                "AND POM_PO_DATE >= '" & strBeginDate & "' AND POM_PO_DATE <= '" & strEndDate & "') tb_a " & _
                "LEFT JOIN " & _
                "(SELECT COUNT(PRM_PR_NO) AS NO_PR_PO, PRD_CONVERT_TO_DOC FROM " & _
                "(SELECT PRM_PR_NO, PRM_COY_ID, PRD_CONVERT_TO_DOC FROM PR_MSTR " & _
                "INNER JOIN PR_DETAILS ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID " & _
                "WHERE PRM_COY_ID = '" & Session("CompanyID") & "' AND PRD_CONVERT_TO_IND = 'PO' " & _
                "AND (PRM_PR_STATUS <> '6' AND PRM_PR_STATUS <> '8') " & _
                "GROUP BY PRM_PR_NO, PRD_CONVERT_TO_DOC) TB_A " & _
                "GROUP BY PRD_CONVERT_TO_DOC) tb_b " & _
                "ON tb_a.POD_PO_NO = tb_b.PRD_CONVERT_TO_DOC " & _
                "LEFT JOIN " & _
                "(SELECT COUNT(PRM_PR_NO) AS NO_PR_RFQ, PRD_CONVERT_TO_DOC FROM " & _
                "(SELECT PRM_PR_NO, PRM_COY_ID, PRD_CONVERT_TO_DOC FROM PR_MSTR " & _
                "INNER JOIN PR_DETAILS ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID " & _
                "WHERE PRM_COY_ID = '" & Session("CompanyID") & "' AND PRD_CONVERT_TO_IND = 'RFQ' " & _
                "AND (PRM_PR_STATUS <> '6' AND PRM_PR_STATUS <> '8') " & _
                "GROUP BY PRM_PR_NO, PRD_CONVERT_TO_DOC) TB_A " & _
                "GROUP BY PRD_CONVERT_TO_DOC) tb_c " & _
                "ON tb_a.POM_RFQ_NO = tb_c.PRD_CONVERT_TO_DOC " & _
                "GROUP BY POD_PO_NO, ACCT_CODE) tb_d " & _
                "LEFT JOIN COMPANY_MSTR ON CM_COY_ID = POM_S_COY_ID " & _
                "GROUP BY ACCT_CODE, POM_S_COY_ID, POD_OVERSEA, POM_CURRENCY_CODE " & _
                "UNION ALL " & _
                "SELECT COUNT(PRM_PR_NO) AS NO_PR, 0 AS NO_PO, '' AS CM_COY_NAME, PRD_OVERSEA AS POD_OVERSEA, " & _
                "'' AS POM_CURRENCY_CODE, IFNULL(AM_ACCT_CODE,'') AS ACCT_CODE, IFNULL(AM_ACCT_DESC,'') AS AM_ACCT_DESC, 0 AS POD_UNIT_AMT, 0 AS POD_L_UNIT_AMT " & _
                "FROM PR_MSTR " & _
                "INNER JOIN PR_DETAILS ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID " & _
                "LEFT JOIN ACCOUNT_MSTR ON AM_ACCT_INDEX = PRD_ACCT_INDEX " & _
                "WHERE PRM_COY_ID = '" & Session("CompanyID") & "' AND PRM_SUBMIT_DATE IS NOT NULL " & _
                "AND PRD_CONVERT_TO_DOC IS NULL AND PRD_ITEM_TYPE = 'SP' " & _
                "AND (PRM_PR_STATUS <> '6' AND PRM_PR_STATUS <> '8') " & _
                "AND PRM_SUBMIT_DATE >= '" & strBeginDate & "' AND PRM_SUBMIT_DATE <= '" & strEndDate & "' " & _
                "GROUP BY ACCT_CODE, POD_OVERSEA "

        ds = objDb.FillDs(strSql)

        strPrmUserId = Session("UserName")
        strPrmCoyName = Session("CompanyName")

        Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PRPOForSpot_DataSetPRPOForSpot", ds.Tables(0))
        Dim localreport As New LocalReport
        localreport.DataSources.Clear()
        localreport.DataSources.Add(rptDataSource)
        localreport.ReportPath = dispatcher.direct("Report", "SummaryPRPOForSpt.rdlc", "Report")
        localreport.EnableExternalImages = True

        Dim I As Byte
        Dim GetParameter As String = ""
        Dim TotalParameter As Byte
        Dim strID As String = ""

        strID = System.Configuration.ConfigurationManager.AppSettings.Get("Env") & "ReportCoyLogoPath"
        TotalParameter = localreport.GetParameters.Count
        Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
        'Dim paramlist As New Generic.List(Of ReportParameter)
        For I = 0 To localreport.GetParameters.Count - 1
            GetParameter = localreport.GetParameters.Item(I).Name
            Select Case LCase(GetParameter)
                Case "pmrequestedby"
                    par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strPrmUserId)

                Case "dtfrom"
                    par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strPrmStart)

                Case "dtto"
                    par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strPrmEnd)

                Case "logo"
                    par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings(strID) & strPrmImgSrc)

                Case "prmbuyercoyname"
                    par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strPrmCoyName)

                Case Else
            End Select
        Next
        localreport.SetParameters(par)
        AddHandler localreport.SubreportProcessing, AddressOf SetPOReportSpotSubReport
        localreport.Refresh()

        Dim deviceInfo As String = _
            "<DeviceInfo>" + _
                "  <OutputFormat>EMF</OutputFormat>" + _
                "</DeviceInfo>"
        System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
        Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
        System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
        strFileName = "POSummaryReport(Spot).pdf"
        Me.Response.Clear()
        Me.Response.ContentType = "application/pdf"
        Me.Response.AddHeader("Content-disposition", "attachment; filename=" & strFileName)
        Me.Response.BinaryWrite(PDF)
        Me.Response.End()

    End Sub

    Private Sub ExportToPDF()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory
        Dim dtFrom As Date
        Dim dtTo As Date
        Dim dtDate As Date
        Dim strBeginDate, strEndDate As String
        Dim strGRNBeginDate, strGRNEndDate As String
        Dim strMonth, strYear As String
        Dim strStart As String
        Dim strEnd As String
        Dim strDate As String
        Dim strDate2 As String
        Dim strDt As String
        Dim strOversea As String
        Dim strFileName As String = ""
        Dim objFile As New FileManagement
        Dim strYear1 As Integer
        Dim strSql As String
        Dim strTemp As String = ""
        Dim strMonth_1 As String = ""
        Dim strMonth_2 As String = ""
        Dim intActive As Integer
        Dim decClosingBal As Decimal

        'Parameter
        Dim strPrmUserId, strPrmImgSrc, strPrmStart, strPrmEnd, strPrmDate, strPrmCoyName, strPrmOversea, strPrmYear, strPrmMonth As String
        Dim strPrmPast1Mth, strPrmPast2Mth, strPrmPast3Mth, strPrmPast4Mth, strPrmPast5Mth, strPrmPast6Mth As String

        'Get Company Logo
        strPrmImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        Try
            ' get connection information
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"
            'open connection
            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                If ViewState("type") = "TOP20VEN" Then
                    'strSql = "SET @pos='0';"
                    'objDb.Execute(strSql)
                    .CommandText = "SELECT ROWNUM, A.CM_COY_NAME AS CM_COY_NAME, POM_PO_LOCAL_COST, POM_CURRENCY_CODE, POM_PO_COST FROM " & _
                                "(SELECT @rownum:=@rownum+1 AS ROWNUM, CM_COY_NAME, SUM(POM_PO_LOCAL_COST) AS POM_PO_LOCAL_COST FROM " & _
                                "(SELECT PO_MSTR.*, (@rownum := 0) AS r, CM_COY_NAME, (POM_PO_COST * IFNULL(POM_EXCHANGE_RATE,1)) AS POM_PO_LOCAL_COST " & _
                                "FROM PO_MSTR " & _
                                "INNER JOIN PO_DETAILS ON POM_B_COY_ID = POD_COY_ID AND POM_PO_NO = POD_PO_NO " & _
                                "INNER JOIN COMPANY_MSTR ON POM_S_COY_ID = CM_COY_ID " & _
                                "WHERE POM_B_COY_ID = @prmCoyID AND POD_ITEM_TYPE = 'SP' " & _
                                "AND (POM_PO_STATUS = '1' OR POM_PO_STATUS = '2' OR POM_PO_STATUS = '3' OR POM_PO_STATUS = '6') " & _
                                "AND POM_PO_DATE >= @prmStartDate AND POM_PO_DATE <= @prmEndDate " & _
                                "GROUP BY POM_PO_NO) tb " & _
                                "GROUP BY CM_COY_NAME ORDER BY POM_PO_LOCAL_COST DESC LIMIT 20) a " & _
                                "INNER JOIN " & _
                                "(SELECT CM_COY_NAME, SUM(POM_PO_COST) AS POM_PO_COST, POM_CURRENCY_CODE FROM " & _
                                "(SELECT PO_MSTR.*, CM_COY_NAME FROM PO_MSTR " & _
                                "INNER JOIN PO_DETAILS ON POM_B_COY_ID = POD_COY_ID AND POM_PO_NO = POD_PO_NO " & _
                                "INNER JOIN COMPANY_MSTR ON POM_S_COY_ID = CM_COY_ID " & _
                                "WHERE POM_B_COY_ID = @prmCoyID AND POD_ITEM_TYPE = 'SP' " & _
                                "AND (POM_PO_STATUS = '1' OR POM_PO_STATUS = '2' OR POM_PO_STATUS = '3' OR POM_PO_STATUS = '6') " & _
                                "AND POM_PO_DATE >= @prmStartDate AND POM_PO_DATE <= @prmEndDate " & _
                                "GROUP BY POM_PO_NO) tb " & _
                                "GROUP BY CM_COY_NAME, POM_CURRENCY_CODE) b " & _
                                "ON a.CM_COY_NAME = b.CM_COY_NAME " & _
                                "ORDER BY POM_PO_LOCAL_COST DESC "

                    'ElseIf ViewState("type") = "LOCALOVERSEAITEM" Then
                    '    .CommandText = "SELECT SUM(IFNULL(NO_SP_PR,0)) AS NO_SP_PR, SUM(IFNULL(NO_INV_PR,0)) AS NO_INV_PR, " & _
                    '                "SUM(NO_SP_PO) AS NO_SP_PO, SUM(NO_INV_PO) AS NO_INV_PO, SUM(PO_AMT_SP) AS PO_AMT_SP, SUM(PO_AMT_INV) AS PO_AMT_INV, " & _
                    '                "SUM(PM_LOCAL_SP_PO_AMT) AS PM_LOCAL_SP_PO_AMT, SUM(PM_LOCAL_INV_PO_AMT) AS PM_LOCAL_INV_PO_AMT, " & _
                    '                "SUM(PM_OVERSEA_SP_PO_AMT) AS PM_OVERSEA_SP_PO_AMT, SUM(PM_OVERSEA_INV_PO_AMT) AS PM_OVERSEA_INV_PO_AMT FROM " & _
                    '                "(SELECT POM_DATE, POM_PO_DATE, COUNT(NO_SP_PO) AS NO_SP_PO, COUNT(NO_INV_PO) AS NO_INV_PO, " & _
                    '                "SUM(PO_AMT_SP) AS PO_AMT_SP, SUM(PO_AMT_INV) AS PO_AMT_INV,  " & _
                    '                "SUM(PM_LOCAL_SP_PO_AMT) AS PM_LOCAL_SP_PO_AMT, SUM(PM_LOCAL_INV_PO_AMT) AS PM_LOCAL_INV_PO_AMT,  " & _
                    '                "SUM(PM_OVERSEA_SP_PO_AMT) AS PM_OVERSEA_SP_PO_AMT, SUM(PM_OVERSEA_INV_PO_AMT) AS PM_OVERSEA_INV_PO_AMT FROM " & _
                    '                "(SELECT DATE_FORMAT(POM_PO_DATE,'%b %y') AS POM_DATE, POM_PO_DATE, POM_PO_NO, " & _
                    '                "CASE WHEN POD_ITEM_TYPE = 'SP' THEN POM_PO_NO END AS NO_SP_PO, " & _
                    '                "CASE WHEN POD_ITEM_TYPE <> 'SP' THEN POM_PO_NO END AS NO_INV_PO, " & _
                    '                "CASE WHEN POD_ITEM_TYPE = 'SP' AND (POD_OVERSEA = 'Y' OR POD_OVERSEA = 'N') THEN (POM_PO_COST * IFNULL(POM_EXCHANGE_RATE,1)) ELSE 0 END AS PO_AMT_SP, " & _
                    '                "CASE WHEN POD_ITEM_TYPE <> 'SP' AND (POD_OVERSEA = 'Y' OR POD_OVERSEA = 'N') THEN (POM_PO_COST * IFNULL(POM_EXCHANGE_RATE,1)) ELSE 0 END AS PO_AMT_INV, " & _
                    '                "CASE WHEN (POD_ITEM_TYPE = 'SP' AND POD_OVERSEA = 'N') THEN POM_PO_COST * IFNULL(POM_EXCHANGE_RATE,1) ELSE 0 END AS PM_LOCAL_SP_PO_AMT, " & _
                    '                "CASE WHEN (POD_ITEM_TYPE <> 'SP' AND POD_OVERSEA = 'N') THEN POM_PO_COST * IFNULL(POM_EXCHANGE_RATE,1) ELSE 0 END AS PM_LOCAL_INV_PO_AMT, " & _
                    '                "CASE WHEN (POD_ITEM_TYPE = 'SP' AND POD_OVERSEA = 'Y') THEN POM_PO_COST * IFNULL(POM_EXCHANGE_RATE,1) ELSE 0 END AS PM_OVERSEA_SP_PO_AMT, " & _
                    '                "CASE WHEN (POD_ITEM_TYPE <> 'SP' AND POD_OVERSEA = 'Y') THEN POM_PO_COST * IFNULL(POM_EXCHANGE_RATE,1) ELSE 0 END AS PM_OVERSEA_INV_PO_AMT " & _
                    '                "FROM PO_MSTR POM, PO_DETAILS POD " & _
                    '                "WHERE(POD.POD_PO_NO = POM.POM_PO_NO And POD.POD_COY_ID = POM.POM_B_COY_ID) " & _
                    '                "AND POD.POD_COY_ID = @prmCoyID AND POM.POM_PO_DATE IS NOT NULL  " & _
                    '                "AND POM.POM_PO_DATE >= @prmStartDate AND POM.POM_PO_DATE <= @prmEndDate GROUP BY POM_PO_NO) AS TB_A " & _
                    '                "GROUP BY POM_DATE) AS TB_A " & _
                    '                "LEFT JOIN " & _
                    '                "(SELECT PRM_DATE, PRM_SUBMIT_DATE, COUNT(NO_SP_PR) AS NO_SP_PR, COUNT(NO_INV_PR) AS NO_INV_PR FROM  " & _
                    '                "(SELECT DATE_FORMAT(PRM_SUBMIT_DATE,'%b %y') AS PRM_DATE, PRM_SUBMIT_DATE,  " & _
                    '                "CASE WHEN PRD_ITEM_TYPE = 'SP' THEN PRM_PR_NO END AS NO_SP_PR, " & _
                    '                "CASE WHEN PRD_ITEM_TYPE <> 'SP' THEN PRM_PR_NO END AS NO_INV_PR, PRM_PR_NO  " & _
                    '                "FROM PR_MSTR PRM, PR_DETAILS PRD WHERE PRM_COY_ID = @prmCoyID AND  " & _
                    '                "PRM.PRM_COY_ID = PRD.PRD_COY_ID And PRM.PRM_PR_NO = PRD.PRD_PR_NO " & _
                    '                "AND PRM_SUBMIT_DATE IS NOT NULL  " & _
                    '                "AND PRM_SUBMIT_DATE >= @prmStartDate AND PRM_SUBMIT_DATE <= @prmEndDate " & _
                    '                "AND (PRM_PR_STATUS <> '6' AND PRM_PR_STATUS <> '8') GROUP BY PRM_PR_NO) AS TB_B  " & _
                    '                "GROUP BY PRM_DATE ORDER BY PRM_SUBMIT_DATE) AS TB_B " & _
                    '                "ON TB_A.POM_DATE = TB_B.PRM_DATE "

                ElseIf ViewState("type") = "LOCALOVERSEAPUR" Then
                    .CommandText = "SELECT POM_DATE, IFNULL(NO_PR,0) AS NO_PR, " & _
                               "IFNULL(NO_PO,0) AS NO_PO, POM_PO_COST, " & _
                               "PM_LOCAL_PO_AMT, PM_OVERSEA_PO_AMT " & _
                               "FROM " & _
                               "(SELECT POM_DATE, POM_PO_DATE, COUNT(POM_PO_NO) AS NO_PO, SUM(POM_PO_COST) AS POM_PO_COST, " & _
                               "SUM(PM_LOCAL_PO_AMT) AS PM_LOCAL_PO_AMT, SUM(PM_OVERSEA_PO_AMT) AS PM_OVERSEA_PO_AMT " & _
                               "FROM " & _
                               "(SELECT DATE_FORMAT(POM_PO_DATE,'%b %y') AS POM_DATE, POM_PO_NO, POM_PO_DATE, " & _
                               "(POM_PO_COST * IFNULL(POM_EXCHANGE_RATE,1)) AS POM_PO_COST, " & _
                               "CASE WHEN POD_OVERSEA = 'N' THEN POM_PO_COST * IFNULL(POM_EXCHANGE_RATE,1) ELSE 0 END AS PM_LOCAL_PO_AMT, " & _
                               "CASE WHEN POD_OVERSEA = 'Y' THEN POM_PO_COST * IFNULL(POM_EXCHANGE_RATE,1) ELSE 0 END AS PM_OVERSEA_PO_AMT " & _
                               "FROM PO_MSTR " & _
                               "INNER JOIN PO_DETAILS ON POD_PO_NO = POM_PO_NO AND POD_COY_ID = POM_B_COY_ID " & _
                               "WHERE POM_B_COY_ID = @prmCoyID AND POM_PO_DATE IS NOT NULL " & _
                               "AND (POD_OVERSEA = 'N' OR POD_OVERSEA = 'Y') " & _
                               "AND (POM_PO_STATUS = '1' OR POM_PO_STATUS = '2' OR POM_PO_STATUS = '3' OR POM_PO_STATUS = '4' " & _
                               "OR POM_PO_STATUS = '4' OR POM_PO_STATUS = '5' OR POM_PO_STATUS = '6') " & _
                               "AND POM_PO_DATE >= @prmStartDate AND POM_PO_DATE <= @prmEndDate GROUP BY POM_PO_NO) tb " & _
                               "GROUP BY POM_DATE) AS TB_A " & _
                               "LEFT JOIN " & _
                               "(SELECT DATE_FORMAT(PRM_SUBMIT_DATE,'%b %y') AS PRM_DATE, PRM_SUBMIT_DATE, " & _
                               "COUNT(PRM_PR_NO) AS NO_PR FROM PR_MSTR WHERE PRM_COY_ID = @prmCoyID AND PRM_SUBMIT_DATE IS NOT NULL " & _
                               "AND PRM_SUBMIT_DATE >= @prmStartDate AND PRM_SUBMIT_DATE <= @prmEndDate " & _
                               "AND (PRM_PR_STATUS <> '6' AND PRM_PR_STATUS <> '8') GROUP BY PRM_DATE " & _
                               "ORDER BY PRM_SUBMIT_DATE) AS TB_B " & _
                               "ON TB_A.POM_DATE = TB_B.PRM_DATE " & _
                               "ORDER BY POM_PO_DATE "

                ElseIf ViewState("type") = "PRPOFORINV" Then
                    .CommandText = "SELECT SUM(NO_PR) AS NO_PR, COUNT(POD_PO_NO) AS NO_PO, CM_COY_NAME, POD_OVERSEA, " & _
                                "POM_CURRENCY_CODE, ACCT_CODE, AM_ACCT_DESC, SUM(POD_UNIT_AMT) AS POD_UNIT_AMT, SUM(POD_L_UNIT_AMT) AS POD_L_UNIT_AMT FROM " & _
                                "(SELECT (IFNULL(NO_PR_PO,0) + IFNULL(NO_PR_RFQ,0)) AS NO_PR, POD_PO_NO, POM_S_COY_ID, POD_OVERSEA, " & _
                                "POM_CURRENCY_CODE, POD_ACCT_CODE AS ACCT_CODE, AM_ACCT_DESC, SUM(POD_UNIT_AMT) AS POD_UNIT_AMT, " & _
                                "SUM(POD_UNIT_AMT * POM_EXCHANGE_RATE) AS POD_L_UNIT_AMT FROM " & _
                                "(SELECT POD_PO_NO, POD_VENDOR_ITEM_CODE, POD_PO_LINE, IFNULL(RM_RFQ_NO,'') AS POM_RFQ_NO, POM_S_COY_ID, POD_OVERSEA, POM_CURRENCY_CODE, " & _
                                "IFNULL(AM_ACCT_CODE,'') AS POD_ACCT_CODE, IFNULL(AM_ACCT_DESC,'') AS AM_ACCT_DESC, " & _
                                "(POD_UNIT_COST * POD_ORDERED_QTY) + ((POD_UNIT_COST * POD_ORDERED_QTY) / 100 * POD_GST) AS POD_UNIT_AMT, " & _
                                "IFNULL(POM_EXCHANGE_RATE,1) AS POM_EXCHANGE_RATE " & _
                                "FROM PO_DETAILS " & _
                                "INNER JOIN PO_MSTR ON POD_PO_NO = POM_PO_NO AND POM_B_COY_ID = POD_COY_ID " & _
                                "LEFT JOIN RFQ_MSTR ON POM_RFQ_INDEX = RM_RFQ_ID " & _
                                "LEFT JOIN ACCOUNT_MSTR ON POD_ACCT_INDEX = AM_ACCT_INDEX " & _
                                "WHERE POD_ITEM_TYPE <> 'SP' AND POD_COY_ID = @prmCoyID AND POM_PO_DATE IS NOT NULL " & _
                                "AND (AM_ACCT_CODE = '90105A' OR AM_ACCT_CODE = '90105B' OR AM_ACCT_CODE = '50801A' " & _
                                "OR AM_ACCT_CODE = '50801B' OR AM_ACCT_CODE = '50802A' OR AM_ACCT_CODE = '50802B') " & _
                                "AND POM_PO_DATE >= @prmStartDate AND POM_PO_DATE <= @prmEndDate) tb_a " & _
                                "LEFT JOIN " & _
                                "(SELECT COUNT(PRM_PR_NO) AS NO_PR_PO, PRD_CONVERT_TO_DOC FROM " & _
                                "(SELECT PRM_PR_NO, PRM_COY_ID, PRD_CONVERT_TO_DOC FROM PR_MSTR " & _
                                "INNER JOIN PR_DETAILS ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID " & _
                                "WHERE PRM_COY_ID = @prmCoyID AND PRD_CONVERT_TO_IND = 'PO' " & _
                                "GROUP BY PRM_PR_NO, PRD_CONVERT_TO_DOC) TB_A " & _
                                "GROUP BY PRD_CONVERT_TO_DOC) tb_b " & _
                                "ON tb_a.POD_PO_NO = tb_b.PRD_CONVERT_TO_DOC " & _
                                "LEFT JOIN " & _
                                "(SELECT COUNT(PRM_PR_NO) AS NO_PR_RFQ, PRD_CONVERT_TO_DOC FROM " & _
                                "(SELECT PRM_PR_NO, PRM_COY_ID, PRD_CONVERT_TO_DOC FROM PR_MSTR " & _
                                "INNER JOIN PR_DETAILS ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID " & _
                                "WHERE PRM_COY_ID = @prmCoyID AND PRD_CONVERT_TO_IND = 'RFQ' " & _
                                "GROUP BY PRM_PR_NO, PRD_CONVERT_TO_DOC) TB_A " & _
                                "GROUP BY PRD_CONVERT_TO_DOC) tb_c " & _
                                "ON tb_a.POM_RFQ_NO = tb_c.PRD_CONVERT_TO_DOC " & _
                                "GROUP BY POD_PO_NO, ACCT_CODE) tb_d " & _
                                "LEFT JOIN COMPANY_MSTR ON CM_COY_ID = POM_S_COY_ID " & _
                                "GROUP BY ACCT_CODE, POM_S_COY_ID, POD_OVERSEA, POM_CURRENCY_CODE " & _
                                "UNION ALL " & _
                                "SELECT COUNT(PRM_PR_NO) AS NO_PR, 0 AS NO_PO, '' AS CM_COY_NAME, PRD_OVERSEA AS POD_OVERSEA, " & _
                                "'' AS POM_CURRENCY_CODE, IFNULL(AM_ACCT_CODE,'') AS ACCT_CODE, IFNULL(AM_ACCT_DESC,'') AS AM_ACCT_DESC, 0 AS POD_UNIT_AMT, 0 AS POD_L_UNIT_AMT " & _
                                "FROM PR_MSTR " & _
                                "INNER JOIN PR_DETAILS ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID " & _
                                "LEFT JOIN ACCOUNT_MSTR ON AM_ACCT_INDEX = PRD_ACCT_INDEX " & _
                                "WHERE PRM_COY_ID = @prmCoyID AND PRM_SUBMIT_DATE IS NOT NULL " & _
                                "AND PRD_CONVERT_TO_DOC IS NULL AND PRD_ITEM_TYPE <> 'SP' " & _
                                "AND (PRM_PR_STATUS <> '6' AND PRM_PR_STATUS <> '8') " & _
                                "AND (AM_ACCT_CODE = '90105A' OR AM_ACCT_CODE = '90105B' OR AM_ACCT_CODE = '50801A' " & _
                                "OR AM_ACCT_CODE = '50801B' OR AM_ACCT_CODE = '50802A' OR AM_ACCT_CODE = '50802B') " & _
                                "AND PRM_SUBMIT_DATE >= @prmStartDate AND PRM_SUBMIT_DATE <= @prmEndDate " & _
                                "GROUP BY ACCT_CODE, POD_OVERSEA "

                    '.CommandText = "SELECT NO_PR, 1 AS NO_PO, CM_COY_NAME, POD_OVERSEA, " & _
                    '            "POM_CURRENCY_CODE, ACCT_CODE, POD_UNIT_AMT, POD_L_UNIT_AMT FROM " & _
                    '            "(SELECT (IFNULL(NO_PR_PO,0) + IFNULL(NO_PR_RFQ,0)) AS NO_PR, POD_PO_NO, POM_S_COY_ID, POD_OVERSEA, " & _
                    '            "POM_CURRENCY_CODE, POD_ACCT_CODE AS ACCT_CODE, SUM(POD_UNIT_AMT) AS POD_UNIT_AMT, " & _
                    '            "SUM(POD_UNIT_AMT * POM_EXCHANGE_RATE) AS POD_L_UNIT_AMT FROM " & _
                    '            "(SELECT POD_PO_NO, POD_VENDOR_ITEM_CODE, POD_PO_LINE, IFNULL(RM_RFQ_NO,'') AS POM_RFQ_NO, POM_S_COY_ID, POD_OVERSEA, POM_CURRENCY_CODE, " & _
                    '            "IFNULL(AM_ACCT_CODE,'') AS POD_ACCT_CODE, " & _
                    '            "(POD_UNIT_COST * POD_ORDERED_QTY) + ((POD_UNIT_COST * POD_ORDERED_QTY) / 100 * POD_GST) AS POD_UNIT_AMT, " & _
                    '            "IFNULL(POM_EXCHANGE_RATE,1) AS POM_EXCHANGE_RATE " & _
                    '            "FROM PO_DETAILS " & _
                    '            "INNER JOIN PO_MSTR ON POD_PO_NO = POM_PO_NO AND POM_B_COY_ID = POD_COY_ID " & _
                    '            "LEFT JOIN RFQ_MSTR ON POM_RFQ_INDEX = RM_RFQ_ID " & _
                    '            "LEFT JOIN ACCOUNT_MSTR ON POD_ACCT_INDEX = AM_ACCT_INDEX " & _
                    '            "WHERE POD_ITEM_TYPE <> 'SP' AND POD_COY_ID = @prmCoyID AND POM_PO_DATE IS NOT NULL " & _
                    '            "AND (AM_ACCT_CODE = '3-3' OR AM_ACCT_CODE = '90105 A' OR AM_ACCT_CODE = '90105 B' OR AM_ACCT_CODE = '50801 A' " & _
                    '            "OR AM_ACCT_CODE = '50801 B' OR AM_ACCT_CODE = '50802 A' OR AM_ACCT_CODE = '50802 B') " & _
                    '            "AND POM_PO_DATE >= @prmStartDate AND POM_PO_DATE <= @prmEndDate) tb_a " & _
                    '            "LEFT JOIN " & _
                    '            "(SELECT COUNT(PRM_PR_NO) AS NO_PR_PO, PRD_CONVERT_TO_DOC FROM " & _
                    '            "(SELECT PRM_PR_NO, PRM_COY_ID, PRD_CONVERT_TO_DOC FROM PR_MSTR " & _
                    '            "INNER JOIN PR_DETAILS ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID " & _
                    '            "WHERE PRM_COY_ID = @prmCoyID AND PRD_CONVERT_TO_IND = 'PO' " & _
                    '            "GROUP BY PRM_PR_NO, PRD_CONVERT_TO_DOC) TB_A " & _
                    '            "GROUP BY PRD_CONVERT_TO_DOC) tb_b " & _
                    '            "ON tb_a.POD_PO_NO = tb_b.PRD_CONVERT_TO_DOC " & _
                    '            "LEFT JOIN " & _
                    '            "(SELECT COUNT(PRM_PR_NO) AS NO_PR_RFQ, PRD_CONVERT_TO_DOC FROM " & _
                    '            "(SELECT PRM_PR_NO, PRM_COY_ID, PRD_CONVERT_TO_DOC FROM PR_MSTR " & _
                    '            "INNER JOIN PR_DETAILS ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID " & _
                    '            "WHERE PRM_COY_ID = @prmCoyID AND PRD_CONVERT_TO_IND = 'RFQ' " & _
                    '            "GROUP BY PRM_PR_NO, PRD_CONVERT_TO_DOC) TB_A " & _
                    '            "GROUP BY PRD_CONVERT_TO_DOC) tb_c " & _
                    '            "ON tb_a.POM_RFQ_NO = tb_c.PRD_CONVERT_TO_DOC " & _
                    '            "GROUP BY POD_PO_NO, ACCT_CODE) tb_d " & _
                    '            "INNER JOIN COMPANY_MSTR ON CM_COY_ID = POM_S_COY_ID " & _
                    '            "UNION ALL " & _
                    '            "SELECT COUNT(PRM_PR_NO) AS NO_PR, 0 AS NO_PO, '' AS CM_COY_NAME, PRD_OVERSEA AS POD_OVERSEA, " & _
                    '            "'' AS POM_CURRENCY_CODE, IFNULL(AM_ACCT_CODE,'') AS ACCT_CODE, 0 AS POD_UNIT_AMT, 0 AS POD_L_UNIT_AMT " & _
                    '            "FROM PR_MSTR " & _
                    '            "INNER JOIN PR_DETAILS ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID " & _
                    '            "LEFT JOIN ACCOUNT_MSTR ON AM_ACCT_INDEX = PRD_ACCT_INDEX " & _
                    '            "WHERE PRM_COY_ID = @prmCoyID AND PRM_SUBMIT_DATE IS NOT NULL " & _
                    '            "AND PRD_CONVERT_TO_DOC IS NULL AND PRD_ITEM_TYPE <> 'SP' " & _
                    '            "AND (AM_ACCT_CODE = '3-3' OR AM_ACCT_CODE = '90105 A' OR AM_ACCT_CODE = '90105 B' OR AM_ACCT_CODE = '50801 A' " & _
                    '            "OR AM_ACCT_CODE = '50801 B' OR AM_ACCT_CODE = '50802 A' OR AM_ACCT_CODE = '50802 B') " & _
                    '            "AND PRM_SUBMIT_DATE >= @prmStartDate AND PRM_SUBMIT_DATE <= @prmEndDate " & _
                    '            "GROUP BY ACCT_CODE, POD_OVERSEA "

                    'ElseIf ViewState("type") = "PRPOFORSP" Then
                    '    .CommandText = "SELECT SUM(NO_PR) AS NO_PR, COUNT(POD_PO_NO) AS NO_PO, CM_COY_NAME, POD_OVERSEA, " & _
                    '                "POM_CURRENCY_CODE, ACCT_CODE, AM_ACCT_DESC, SUM(POD_UNIT_AMT) AS POD_UNIT_AMT, SUM(POD_L_UNIT_AMT) AS POD_L_UNIT_AMT FROM " & _
                    '                "(SELECT (IFNULL(NO_PR_PO,0) + IFNULL(NO_PR_RFQ,0)) AS NO_PR, POD_PO_NO, POM_S_COY_ID, POD_OVERSEA, " & _
                    '                "POM_CURRENCY_CODE, POD_ACCT_CODE AS ACCT_CODE, AM_ACCT_DESC, SUM(POD_UNIT_AMT) AS POD_UNIT_AMT, " & _
                    '                "SUM(POD_UNIT_AMT * POM_EXCHANGE_RATE) AS POD_L_UNIT_AMT FROM " & _
                    '                "(SELECT POD_PO_NO, POD_VENDOR_ITEM_CODE, POD_PO_LINE, IFNULL(RM_RFQ_NO,'') AS POM_RFQ_NO, POM_S_COY_ID, POD_OVERSEA, POM_CURRENCY_CODE, " & _
                    '                "IFNULL(AM_ACCT_CODE,'') AS POD_ACCT_CODE, IFNULL(AM_ACCT_DESC,'') AS AM_ACCT_DESC, " & _
                    '                "(POD_UNIT_COST * POD_ORDERED_QTY) + ((POD_UNIT_COST * POD_ORDERED_QTY) / 100 * POD_GST) AS POD_UNIT_AMT, " & _
                    '                "IFNULL(POM_EXCHANGE_RATE,1) AS POM_EXCHANGE_RATE " & _
                    '                "FROM PO_DETAILS " & _
                    '                "INNER JOIN PO_MSTR ON POD_PO_NO = POM_PO_NO AND POM_B_COY_ID = POD_COY_ID " & _
                    '                "LEFT JOIN RFQ_MSTR ON POM_RFQ_INDEX = RM_RFQ_ID " & _
                    '                "LEFT JOIN ACCOUNT_MSTR ON POD_ACCT_INDEX = AM_ACCT_INDEX " & _
                    '                "WHERE POD_ITEM_TYPE = 'SP' AND POD_COY_ID = @prmCoyID AND POM_PO_DATE IS NOT NULL " & _
                    '                "AND POM_PO_DATE >= @prmStartDate AND POM_PO_DATE <= @prmEndDate) tb_a " & _
                    '                "LEFT JOIN " & _
                    '                "(SELECT COUNT(PRM_PR_NO) AS NO_PR_PO, PRD_CONVERT_TO_DOC FROM " & _
                    '                "(SELECT PRM_PR_NO, PRM_COY_ID, PRD_CONVERT_TO_DOC FROM PR_MSTR " & _
                    '                "INNER JOIN PR_DETAILS ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID " & _
                    '                "WHERE PRM_COY_ID = @prmCoyID AND PRD_CONVERT_TO_IND = 'PO' " & _
                    '                "AND (PRM_PR_STATUS <> '6' AND PRM_PR_STATUS <> '8') " & _
                    '                "GROUP BY PRM_PR_NO, PRD_CONVERT_TO_DOC) TB_A " & _
                    '                "GROUP BY PRD_CONVERT_TO_DOC) tb_b " & _
                    '                "ON tb_a.POD_PO_NO = tb_b.PRD_CONVERT_TO_DOC " & _
                    '                "LEFT JOIN " & _
                    '                "(SELECT COUNT(PRM_PR_NO) AS NO_PR_RFQ, PRD_CONVERT_TO_DOC FROM " & _
                    '                "(SELECT PRM_PR_NO, PRM_COY_ID, PRD_CONVERT_TO_DOC FROM PR_MSTR " & _
                    '                "INNER JOIN PR_DETAILS ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID " & _
                    '                "WHERE PRM_COY_ID = @prmCoyID AND PRD_CONVERT_TO_IND = 'RFQ' " & _
                    '                "AND (PRM_PR_STATUS <> '6' AND PRM_PR_STATUS <> '8') " & _
                    '                "GROUP BY PRM_PR_NO, PRD_CONVERT_TO_DOC) TB_A " & _
                    '                "GROUP BY PRD_CONVERT_TO_DOC) tb_c " & _
                    '                "ON tb_a.POM_RFQ_NO = tb_c.PRD_CONVERT_TO_DOC " & _
                    '                "GROUP BY POD_PO_NO, ACCT_CODE) tb_d " & _
                    '                "LEFT JOIN COMPANY_MSTR ON CM_COY_ID = POM_S_COY_ID " & _
                    '                "GROUP BY ACCT_CODE, POM_S_COY_ID, POD_OVERSEA, POM_CURRENCY_CODE " & _
                    '                "UNION ALL " & _
                    '                "SELECT COUNT(PRM_PR_NO) AS NO_PR, 0 AS NO_PO, '' AS CM_COY_NAME, PRD_OVERSEA AS POD_OVERSEA, " & _
                    '                "'' AS POM_CURRENCY_CODE, IFNULL(AM_ACCT_CODE,'') AS ACCT_CODE, IFNULL(AM_ACCT_DESC,'') AS AM_ACCT_DESC, 0 AS POD_UNIT_AMT, 0 AS POD_L_UNIT_AMT " & _
                    '                "FROM PR_MSTR " & _
                    '                "INNER JOIN PR_DETAILS ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID " & _
                    '                "LEFT JOIN ACCOUNT_MSTR ON AM_ACCT_INDEX = PRD_ACCT_INDEX " & _
                    '                "WHERE PRM_COY_ID = @prmCoyID AND PRM_SUBMIT_DATE IS NOT NULL " & _
                    '                "AND PRD_CONVERT_TO_DOC IS NULL AND PRD_ITEM_TYPE = 'SP' " & _
                    '                "AND (PRM_PR_STATUS <> '6' AND PRM_PR_STATUS <> '8') " & _
                    '                "AND PRM_SUBMIT_DATE >= @prmStartDate AND PRM_SUBMIT_DATE <= @prmEndDate " & _
                    '                "GROUP BY ACCT_CODE, POD_OVERSEA "

                ElseIf ViewState("type") = "POBAL" Then
                    .CommandText = "SELECT * FROM (SELECT POD_VENDOR_ITEM_CODE, POD_PO_NO, POM_PO_DATE, " & _
                                "DATE_FORMAT(DATE_ADD(POM_PO_DATE,INTERVAL IFNULL(POD_ETD,0) DAY),'%d/%m/%Y') AS POD_EDD, " & _
                                "POD_ORDERED_QTY, POD_RECEIVED_QTY, (POD_ORDERED_QTY - POD_RECEIVED_QTY) AS POD_BAL_QTY, POM_VENDOR_CODE, " & _
                                "POM_CURRENCY_CODE, POD_UNIT_COST, POM_DEL_CODE, POD_OVERSEA " & _
                                "FROM PO_DETAILS POD, PO_MSTR POM WHERE POD.POD_COY_ID = POM.POM_B_COY_ID " & _
                                "AND POD.POD_PO_NO = POM.POM_PO_NO AND POD_COY_ID = @prmCoyID AND POD_ITEM_TYPE = 'ST' " & _
                                "AND POM_PO_DATE IS NOT NULL) AS tb " & _
                                "WHERE POD_OVERSEA = @prmOversea AND (POM_PO_DATE >= @prmStartDate AND POM_PO_DATE <= @prmEndDate) "

                    If Me.cmbPOBal.SelectedValue = "1" Then
                        .CommandText &= "AND POD_BAL_QTY > 0 "
                    ElseIf Me.cmbPOBal.SelectedValue = "0" Then
                        .CommandText &= "AND POD_BAL_QTY = 0 "
                    End If

                    If Me.txtItemCode.Text <> "" Then
                        .CommandText &= "AND POD_VENDOR_ITEM_CODE LIKE '%" & txtItemCode.Text & "%' "
                    End If

                    If Me.txtSuppCode.Text <> "" Then
                        .CommandText &= "AND POM_VENDOR_CODE LIKE '%" & txtSuppCode.Text & "%' "
                    End If

                ElseIf ViewState("type") = "DELTREND" Then
                    .CommandText = "SELECT POD_VENDOR_ITEM_CODE, SUM(PAST_6TH_DEL_QTY) AS PAST_6TH_DEL_QTY, SUM(PAST_5TH_DEL_QTY) AS PAST_5TH_DEL_QTY, " & _
                                "SUM(PAST_4TH_DEL_QTY) AS PAST_4TH_DEL_QTY, SUM(PAST_3RD_DEL_QTY) AS PAST_3RD_DEL_QTY, " & _
                                "SUM(PAST_2ND_DEL_QTY) AS PAST_2ND_DEL_QTY, SUM(PAST_1ST_DEL_QTY) AS PAST_1ST_DEL_QTY, " & _
                                "SUM(CURR_MTH_DEL_QTY) AS CURR_MTH_DEL_QTY, MAX(DOM_DO_DATE) AS DOM_DO_DATE " & _
                                "FROM (SELECT POD_VENDOR_ITEM_CODE, " & _
                                "CASE WHEN MONTH(DOM_DO_DATE) = MONTH(@prmDate - INTERVAL 6 MONTH) AND YEAR(DOM_DO_DATE) = YEAR(@prmDate - INTERVAL 6 MONTH) " & _
                                "THEN DOD_SHIPPED_QTY ELSE 0 END AS PAST_6TH_DEL_QTY, " & _
                                "CASE WHEN MONTH(DOM_DO_DATE) = MONTH(@prmDate - INTERVAL 5 MONTH) AND YEAR(DOM_DO_DATE) = YEAR(@prmDate - INTERVAL 5 MONTH) " & _
                                "THEN DOD_SHIPPED_QTY ELSE 0 END AS PAST_5TH_DEL_QTY, " & _
                                "CASE WHEN MONTH(DOM_DO_DATE) = MONTH(@prmDate - INTERVAL 4 MONTH) AND YEAR(DOM_DO_DATE) = YEAR(@prmDate - INTERVAL 4 MONTH) " & _
                                "THEN DOD_SHIPPED_QTY ELSE 0 END AS PAST_4TH_DEL_QTY, " & _
                                "CASE WHEN MONTH(DOM_DO_DATE) = MONTH(@prmDate - INTERVAL 3 MONTH) AND YEAR(DOM_DO_DATE) = YEAR(@prmDate - INTERVAL 3 MONTH) " & _
                                "THEN DOD_SHIPPED_QTY ELSE 0 END AS PAST_3RD_DEL_QTY, " & _
                                "CASE WHEN MONTH(DOM_DO_DATE) = MONTH(@prmDate - INTERVAL 2 MONTH) AND YEAR(DOM_DO_DATE) = YEAR(@prmDate - INTERVAL 2 MONTH) " & _
                                "THEN DOD_SHIPPED_QTY ELSE 0 END AS PAST_2ND_DEL_QTY, " & _
                                "CASE WHEN MONTH(DOM_DO_DATE) = MONTH(@prmDate - INTERVAL 1 MONTH) AND YEAR(DOM_DO_DATE) = YEAR(@prmDate - INTERVAL 1 MONTH) " & _
                                "THEN DOD_SHIPPED_QTY ELSE 0 END AS PAST_1ST_DEL_QTY, " & _
                                "CASE WHEN MONTH(DOM_DO_DATE) = MONTH(@prmDate) AND YEAR(DOM_DO_DATE) = YEAR(@prmDate) " & _
                                "THEN DOD_SHIPPED_QTY ELSE 0 END AS CURR_MTH_DEL_QTY, " & _
                                "DOM_DO_DATE FROM DO_DETAILS " & _
                                "INNER JOIN DO_MSTR ON DOD_S_COY_ID = DOM_S_COY_ID AND DOD_DO_NO = DOM_DO_NO " & _
                                "INNER JOIN PO_MSTR ON DOM_PO_INDEX = POM_PO_INDEX " & _
                                "INNER JOIN PO_DETAILS ON POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = POD_COY_ID AND DOD_PO_LINE = POD_PO_LINE " & _
                                "INNER JOIN PRODUCT_MSTR ON POD_VENDOR_ITEM_CODE = PM_VENDOR_ITEM_CODE AND POD_COY_ID = PM_S_COY_ID " & _
                                "WHERE POM_B_COY_ID = @prmCoyID AND PM_ITEM_TYPE = 'ST' AND (DOM_DO_DATE >= @prmDate2 AND DOM_DO_DATE <= @prmDate)) tb " & _
                                "GROUP BY POD_VENDOR_ITEM_CODE ORDER BY POD_VENDOR_ITEM_CODE "

                ElseIf ViewState("type") = "GRNLIST" Then
                    .CommandText = " SELECT POM_PO_NO, DOM_DO_NO, GM_INVOICE_NO, DOM_DO_DATE, IM_PAYMENT_DATE, GD_RECEIVED_QTY, " & _
                               " GM_GRN_NO, IM_S_COY_NAME FROM " & _
                               " (SELECT POM_PO_NO, SUM(GD_RECEIVED_QTY - GD_REJECTED_QTY) AS GD_RECEIVED_QTY, GM_GRN_NO, " & _
                               " GM_INVOICE_NO, GM_S_COY_ID, GM_DO_INDEX FROM GRN_MSTR " & _
                               " INNER JOIN GRN_DETAILS ON GM_GRN_NO = GD_GRN_NO AND GM_B_COY_ID = GD_B_COY_ID " & _
                               " INNER JOIN PO_MSTR ON GM_PO_INDEX = POM_PO_INDEX " & _
                               " INNER JOIN PO_DETAILS ON POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = POD_COY_ID AND GD_PO_LINE = POD_PO_LINE " & _
                               " INNER JOIN PRODUCT_MSTR ON POD_VENDOR_ITEM_CODE = PM_VENDOR_ITEM_CODE AND POD_COY_ID = PM_S_COY_ID " & _
                               " WHERE GM_B_COY_ID = @prmCoyID AND GM_GRN_STATUS <> '1' AND PM_ITEM_TYPE = 'ST' " & _
                               " AND (GM_CREATED_DATE >= @prmGRNStartDate AND GM_CREATED_DATE <= @prmGRNEndDate) " & _
                               " GROUP BY GM_GRN_NO) tb " & _
                               " INNER JOIN INVOICE_MSTR ON GM_INVOICE_NO = IM_INVOICE_NO AND GM_S_COY_ID = IM_S_COY_ID " & _
                               " INNER JOIN DO_MSTR ON GM_DO_INDEX = DOM_DO_INDEX "

                    If Me.dtRadioBtn.SelectedValue = "DO" Then
                        .CommandText &= " WHERE DOM_DO_DATE >= @prmStartDate AND DOM_DO_DATE <= @prmEndDate "
                    ElseIf Me.dtRadioBtn.SelectedValue = "INV" Then
                        .CommandText &= " WHERE IM_PAYMENT_DATE >= @prmStartDate AND IM_PAYMENT_DATE <= @prmEndDate "
                    End If

                    If Me.txtPONo.Text <> "" Then
                        strTemp = Common.BuildWildCard(txtPONo.Text)
                        .CommandText &= " AND POM_PO_NO " & Common.ParseSQL(strTemp)
                    End If

                    If Me.txtSuppName.Text <> "" Then
                        strTemp = Common.BuildWildCard(txtSuppName.Text)
                        .CommandText &= " AND IM_S_COY_NAME " & Common.ParseSQL(strTemp)
                    End If

                    .CommandText &= " ORDER BY POM_PO_NO, DOM_DO_NO, GM_INVOICE_NO "

                ElseIf ViewState("type") = "INDEXBOOK" Then
                    .CommandText = "SELECT PM_ACCT_CODE, PV_PUR_SPEC_NO, PM_PRODUCT_DESC, " & _
                                "PM_SPEC1, PM_SPEC2, PM_SPEC3, PM_VENDOR_ITEM_CODE, PM_PACKING_QTY, " & _
                                "PV_SUPP_CODE AS PV_VENDOR_CODE, (SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = PV_S_COY_ID) AS PV_S_COY_NAME, PV_DELIVERY_TERM, PV_CURR, " & _
                                "PVP_VOLUME_PRICE AS PV_UNIT_PRICE, PM_UOM, PV_LEAD_TIME, PM_MANUFACTURER, PM_MANUFACTURER2, PM_MANUFACTURER3 " & _
                                "FROM PRODUCT_MSTR " & _
                                "LEFT JOIN " & _
                                "(SELECT PV_PRODUCT_INDEX, PV_VENDOR_TYPE, CASE WHEN PV_VENDOR_TYPE = 'P' THEN '0' ELSE PV_VENDOR_TYPE END AS LINE, PV_DELIVERY_TERM, PV_CURR, " & _
                                "PV_SUPP_CODE, PV_LEAD_TIME, PV_PUR_SPEC_NO, PV_S_COY_ID FROM PIM_VENDOR " & _
                                "WHERE (PV_S_COY_ID IS NOT NULL OR PV_S_COY_ID <> '')) tb " & _
                                "ON PM_PRODUCT_INDEX = PV_PRODUCT_INDEX " & _
                                "LEFT JOIN " & _
                                "(SELECT PVP_PRODUCT_CODE, PVP_VENDOR_TYPE, PVP_VOLUME_PRICE FROM PRODUCT_VOLUME_PRICE " & _
                                "WHERE PVP_VOLUME = '1.00') tb_b " & _
                                "ON PM_PRODUCT_CODE = PVP_PRODUCT_CODE AND PV_VENDOR_TYPE = PVP_VENDOR_TYPE " & _
                                "WHERE PM_S_COY_ID = @prmCoyID AND PM_OVERSEA = @prmOversea AND PM_ITEM_TYPE = 'ST' " & _
                                "ORDER BY PM_ACCT_CODE, PM_VENDOR_ITEM_CODE, CAST(LINE AS UNSIGNED) "

                ElseIf ViewState("type") = "LASTKEYINNUM" Then
                    .CommandText = "SELECT " & _
                                "(SELECT MAX(POM_PO_NO) FROM PO_MSTR WHERE POM_B_COY_ID = @prmCoyID AND POM_CREATED_DATE <= @prmDate) AS POM_PO_NO, " & _
                                "(SELECT MAX(GM_GRN_NO) FROM GRN_MSTR WHERE GM_B_COY_ID = @prmCoyID AND GM_CREATED_DATE <= @prmDate) AS GM_GRN_NO, " & _
                                "(SELECT MAX(IRSM_IRS_NO) FROM INVENTORY_REQUISITION_SLIP_MSTR WHERE IRSM_IRS_COY_ID = @prmCoyID AND IRSM_CREATED_DATE <= @prmDate) AS IRSM_IRS_NO "

                ElseIf ViewState("type") = "ISSUETREND" Then
                    .CommandText = "SELECT IM_ITEM_CODE, SUM(PAST_6TH_IRSD_QTY) AS PAST_6TH_IRSD_QTY, " & _
                                "SUM(PAST_5TH_IRSD_QTY) AS PAST_5TH_IRSD_QTY, SUM(PAST_4TH_IRSD_QTY) AS PAST_4TH_IRSD_QTY, " & _
                                "SUM(PAST_3RD_IRSD_QTY) AS PAST_3RD_IRSD_QTY, SUM(PAST_2ND_IRSD_QTY) AS PAST_2ND_IRSD_QTY, " & _
                                "SUM(PAST_1ST_IRSD_QTY) AS PAST_1ST_IRSD_QTY, SUM(CURR_MTH_IRSD_QTY) AS CURR_MTH_IRSD_QTY, " & _
                                "MAX(IRSM_IRS_APPROVED_DATE) AS IRSM_IRS_APPROVED_DATE " & _
                                "FROM (SELECT IM_ITEM_CODE, " & _
                                "CASE WHEN MONTH(IRSM_IRS_APPROVED_DATE) = MONTH(@prmDate - INTERVAL 6 MONTH) AND YEAR(IRSM_IRS_APPROVED_DATE) = YEAR(@prmDate - INTERVAL 6 MONTH) " & _
                                "THEN IRSD_QTY ELSE 0 END AS PAST_6TH_IRSD_QTY, " & _
                                "CASE WHEN MONTH(IRSM_IRS_APPROVED_DATE) = MONTH(@prmDate - INTERVAL 5 MONTH) AND YEAR(IRSM_IRS_APPROVED_DATE) = YEAR(@prmDate - INTERVAL 5 MONTH) " & _
                                "THEN IRSD_QTY ELSE 0 END AS PAST_5TH_IRSD_QTY, " & _
                                "CASE WHEN MONTH(IRSM_IRS_APPROVED_DATE) = MONTH(@prmDate - INTERVAL 4 MONTH) AND YEAR(IRSM_IRS_APPROVED_DATE) = YEAR(@prmDate - INTERVAL 4 MONTH) " & _
                                "THEN IRSD_QTY ELSE 0 END AS PAST_4TH_IRSD_QTY, " & _
                                "CASE WHEN MONTH(IRSM_IRS_APPROVED_DATE) = MONTH(@prmDate - INTERVAL 3 MONTH) AND YEAR(IRSM_IRS_APPROVED_DATE) = YEAR(@prmDate - INTERVAL 3 MONTH) " & _
                                "THEN IRSD_QTY ELSE 0 END AS PAST_3RD_IRSD_QTY, " & _
                                "CASE WHEN MONTH(IRSM_IRS_APPROVED_DATE) = MONTH(@prmDate - INTERVAL 2 MONTH) AND YEAR(IRSM_IRS_APPROVED_DATE) = YEAR(@prmDate - INTERVAL 2 MONTH) " & _
                                "THEN IRSD_QTY ELSE 0 END AS PAST_2ND_IRSD_QTY, " & _
                                "CASE WHEN MONTH(IRSM_IRS_APPROVED_DATE) = MONTH(@prmDate - INTERVAL 1 MONTH) AND YEAR(IRSM_IRS_APPROVED_DATE) = YEAR(@prmDate - INTERVAL 1 MONTH) " & _
                                "THEN IRSD_QTY ELSE 0 END AS PAST_1ST_IRSD_QTY, " & _
                                "CASE WHEN MONTH(IRSM_IRS_APPROVED_DATE) = MONTH(@prmDate) AND YEAR(IRSM_IRS_APPROVED_DATE) = YEAR(@prmDate) THEN IRSD_QTY ELSE 0 END AS CURR_MTH_IRSD_QTY, " & _
                                "IRSM_IRS_APPROVED_DATE FROM INVENTORY_REQUISITION_SLIP_DETAILS " & _
                                "INNER JOIN INVENTORY_REQUISITION_SLIP_MSTR ON IRSD_IRS_COY_ID = IRSM_IRS_COY_ID AND IRSD_IRS_NO = IRSM_IRS_NO " & _
                                "INNER JOIN INVENTORY_MSTR ON IRSD_INVENTORY_INDEX = IM_INVENTORY_INDEX " & _
                                "WHERE IRSD_IRS_COY_ID = @prmCoyID AND IRSM_IRS_APPROVED_DATE <= @prmDate " & _
                                "AND (IRSM_IRS_STATUS = '2' OR IRSM_IRS_STATUS = '3' OR IRSM_IRS_STATUS = '4')) tb " & _
                                "GROUP BY IM_ITEM_CODE ORDER BY IM_ITEM_CODE "

                ElseIf ViewState("type") = "STKSTATUS" Then
                    .CommandText = "SELECT PM_ACCT_CODE, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_SPEC1, PM_SPEC2, PM_SPEC3, " & _
                                "PM_UOM, SUM(IC_COST_CLOSE_QTY) AS IC_COST_CLOSE_QTY, SUM(IC_COST_CLOSE_COST) AS IC_COST_CLOSE_COST " & _
                                "FROM INVENTORY_COST, INVENTORY_MSTR, PRODUCT_MSTR " & _
                                "WHERE IC_INVENTORY_INDEX = IM_INVENTORY_INDEX And IM_COY_ID = PM_S_COY_ID And IM_ITEM_CODE = PM_VENDOR_ITEM_CODE " & _
                                "AND IC_COY_ID = @prmCoyID AND IC_COST_DATE <= @prmDate " & _
                                "GROUP BY PM_VENDOR_ITEM_CODE " & _
                                "ORDER BY PM_ACCT_CODE, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC "

                ElseIf ViewState("type") = "STOCKBAL" Then
                    .CommandText = "SELECT PM_ACCT_CODE, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, SUM(IC_COST_OPEN_QTY) AS IC_COST_OPEN_QTY, " & _
                                "SUM(IC_COST_OPEN_COST) AS IC_COST_OPEN_COST, SUM(IC_COST_ISSUE_QTY) AS IC_COST_ISSUE_QTY, " & _
                                "SUM(IC_COST_ISSUE_COST) AS IC_COST_ISSUE_COST, SUM(IC_COST_DISPOSE_QTY) AS IC_COST_DISPOSE_QTY, " & _
                                "SUM(IC_COST_DISPOSE_COST) AS IC_COST_DISPOSE_COST, SUM(IC_COST_CLOSE_QTY) AS IC_COST_CLOSE_QTY, " & _
                                "SUM(IC_COST_CLOSE_COST) AS IC_COST_CLOSE_COST FROM " & _
                                "(SELECT PM_ACCT_CODE, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, IC_COST_OPEN_QTY, IC_COST_OPEN_COST, " & _
                                "CASE WHEN IC_INVENTORY_TYPE = 'II' THEN IC_COST_QTY ELSE 0 END AS IC_COST_ISSUE_QTY, " & _
                                "CASE WHEN IC_INVENTORY_TYPE = 'II' THEN IC_COST_COST ELSE 0 END AS IC_COST_ISSUE_COST, " & _
                                "CASE WHEN IC_INVENTORY_TYPE = 'WO' THEN IC_COST_QTY ELSE 0 END AS IC_COST_DISPOSE_QTY, " & _
                                "CASE WHEN IC_INVENTORY_TYPE = 'WO' THEN IC_COST_COST ELSE 0 END AS IC_COST_DISPOSE_COST, " & _
                                "IC_COST_CLOSE_QTY, IC_COST_CLOSE_COST " & _
                                "FROM INVENTORY_COST, INVENTORY_MSTR, PRODUCT_MSTR " & _
                                "WHERE IC_INVENTORY_INDEX = IM_INVENTORY_INDEX " & _
                                "AND IM_COY_ID = PM_S_COY_ID AND IM_ITEM_CODE = PM_VENDOR_ITEM_CODE " & _
                                "AND IC_COY_ID = @prmCoyID AND PM_OVERSEA = @prmOversea " & _
                                "AND IC_COST_DATE >= @prmStartDate AND IC_COST_DATE <= @prmEndDate) tb " & _
                                "GROUP BY PM_VENDOR_ITEM_CODE ORDER BY PM_ACCT_CODE, PM_VENDOR_ITEM_CODE "

                ElseIf ViewState("type") = "MRSAPPVD" Then
                    .CommandText = "SELECT IRSM_IRS_APPROVED_DATE, IRSM_IRS_NO, IRSM_IRS_SECTION, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, " & _
                                "PM_SPEC1, PM_SPEC2, PM_SPEC3, IRSD_QTY, IRSM_IRS_REMARK, IRSM_IRS_REQUESTOR_NAME, IRSM_IRS_DATE " & _
                                "FROM INVENTORY_REQUISITION_SLIP_DETAILS, INVENTORY_REQUISITION_SLIP_MSTR, INVENTORY_MSTR, PRODUCT_MSTR, USER_MSTR " & _
                                "WHERE IRSD_IRS_COY_ID = IRSM_IRS_COY_ID And IRSD_IRS_NO = IRSM_IRS_NO " & _
                                "AND IRSD_INVENTORY_INDEX = IM_INVENTORY_INDEX " & _
                                "AND IM_COY_ID = PM_S_COY_ID AND IM_ITEM_CODE = PM_VENDOR_ITEM_CODE " & _
                                "AND IM_COY_ID = UM_COY_ID AND IRSM_BUYER_ID = UM_USER_ID " & _
                                "AND IRSD_IRS_COY_ID = @prmCoyID " & _
                                "AND IRSM_IRS_APPROVED_DATE >= @prmStartDate AND IRSM_IRS_APPROVED_DATE <= @prmEndDate "

                    If Me.txtSKName.Text <> "" Then
                        strTemp = Common.BuildWildCard(txtSKName.Text)
                        .CommandText &= " AND UM_USER_NAME " & Common.ParseSQL(strTemp)
                    End If

                ElseIf ViewState("type") = "MRSLIST" Then
                    .CommandText = " SELECT IM_ITEM_CODE, IRSD_IRS_NO, IRSD_QTY, IRSM_IRS_APPROVED_DATE, IRSM_IRS_SECTION, " & _
                                " CS_SEC_NAME, IC_COST_CLOSE_COST FROM INVENTORY_REQUISITION_SLIP_MSTR " & _
                                " INNER JOIN INVENTORY_REQUISITION_SLIP_DETAILS ON IRSM_IRS_NO = IRSD_IRS_NO AND IRSM_IRS_COY_ID = IRSD_IRS_COY_ID " & _
                                " INNER JOIN INVENTORY_MSTR ON IRSD_INVENTORY_INDEX  = IM_INVENTORY_INDEX " & _
                                " INNER JOIN COMPANY_SECTION ON IRSM_IRS_COY_ID = CS_COY_ID AND IRSM_IRS_SECTION = CS_SEC_CODE " & _
                                " LEFT JOIN INVENTORY_COST ON IRSD_IRS_COY_ID = IC_COY_ID AND IRSD_INVENTORY_INDEX = IC_INVENTORY_INDEX " & _
                                " AND IRSD_IRS_NO = IC_INVENTORY_REF_DOC AND IC_INVENTORY_TYPE = 'II' " & _
                                " WHERE (IRSM_IRS_STATUS = '2' OR IRSM_IRS_STATUS = '3') AND IRSD_IRS_COY_ID =@prmCoyID AND IRSM_IRS_APPROVED_DATE >= @prmStartDate " & _
                                " AND IRSM_IRS_APPROVED_DATE <= @prmEndDate "

                    If Me.txtItemCode2.Text <> "" Then
                        strTemp = Common.BuildWildCard(txtItemCode2.Text)
                        .CommandText &= " AND IM_ITEM_CODE " & Common.ParseSQL(strTemp)
                    End If

                    If Me.txtSectionCode.Text <> "" Then
                        strTemp = Common.BuildWildCard(txtSectionCode.Text)
                        .CommandText &= " AND IRSM_IRS_SECTION " & Common.ParseSQL(strTemp)
                    End If

                    .CommandText &= " ORDER BY IM_ITEM_CODE, IRSD_IRS_NO "

                ElseIf ViewState("type") = "SUMMTHSTKBAL" Then
                    .CommandText = "SELECT PM_ACCT_CODE, CT_NAME, SUM(IC_COST_OPEN_COST) AS IC_COST_OPEN_COST, " & _
                                "SUM(IC_GRN_COST) AS IC_GRN_COST, SUM(IC_II_COST) AS IC_II_COST, SUM(IC_WO_COST) AS IC_WO_COST, " & _
                                "SUM(IC_COST_CLOSE_COST) AS IC_COST_CLOSE_COST, SUM(PO_VALUE) AS PO_VALUE " & _
                                "FROM " & _
                                "(SELECT PM_ACCT_CODE, CT_NAME, IC_COST_OPEN_COST, " & _
                                "CASE IC_INVENTORY_TYPE WHEN 'GRN' THEN IC_COST_COST ELSE 0.00 END AS IC_GRN_COST, " & _
                                "CASE IC_INVENTORY_TYPE WHEN 'II' THEN IC_COST_COST ELSE 0.00 END AS IC_II_COST, " & _
                                "CASE IC_INVENTORY_TYPE WHEN 'WO' THEN IC_COST_COST ELSE 0.00 END AS IC_WO_COST, " & _
                                "IC_COST_CLOSE_COST, IFNULL(PO_VALUE,0) AS PO_VALUE " & _
                                "FROM INVENTORY_COST " & _
                                "INNER JOIN INVENTORY_MSTR ON IC_INVENTORY_INDEX = IM_INVENTORY_INDEX " & _
                                "INNER JOIN PRODUCT_MSTR ON IM_COY_ID = PM_S_COY_ID AND IM_ITEM_CODE = PM_VENDOR_ITEM_CODE " & _
                                "INNER JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID " & _
                                "LEFT JOIN(SELECT SUM(POD_ORDERED_QTY - (POD_RECEIVED_QTY - POD_REJECTED_QTY)) AS PO_VALUE, " & _
                                "POD_VENDOR_ITEM_CODE, POD_COY_ID " & _
                                "FROM PO_DETAILS " & _
                                "INNER JOIN PO_MSTR ON POD_COY_ID = POM_B_COY_ID AND POD_PO_NO = POM_PO_NO " & _
                                "WHERE (POM_PO_STATUS <> 4 AND POM_PO_STATUS <> 5) " & _
                                "AND YEAR(POM_PO_DATE) = @prmYear AND MONTH(POM_PO_DATE) = @prmMonth " & _
                                "AND POD_COY_ID = @prmCoyID " & _
                                "GROUP BY POD_VENDOR_ITEM_CODE) tb_b " & _
                                "ON PM_VENDOR_ITEM_CODE = tb_b.POD_VENDOR_ITEM_CODE AND PM_S_COY_ID = tb_b.POD_COY_ID " & _
                                "WHERE YEAR(IC_COST_DATE) = @prmYear AND MONTH(IC_COST_DATE) = @prmMonth " & _
                                "AND IC_COY_ID = @prmCoyID AND PM_OVERSEA = @prmOversea) tb_c " & _
                                "GROUP BY PM_ACCT_CODE, CT_NAME ORDER BY PM_ACCT_CODE, CT_NAME "

                ElseIf ViewState("type") = "SMFA0612MTH" Then
                    .CommandText = "SELECT PM_ACCT_CODE, IM_ITEM_CODE, IM_INVENTORY_INDEX, IM_INVENTORY_NAME, " & _
                                "CONCAT(PM_SPEC1,' ',PM_SPEC2,' ',PM_SPEC3) AS SPEC, CS_SEC_NAME, IRSM_IRS_SECTION, IRSM_IRS_DATE, IC_COST_DATE, " & _
                                "IFNULL(IC_COST_CLOSE_QTY,0.00) AS IC_COST_CLOSE_QTY, IFNULL(IC_COST_CLOSE_COST,0.00) AS IC_COST_CLOSE_COST " & _
                                "FROM INVENTORY_MSTR " & _
                                "LEFT JOIN " & _
                                "(SELECT IRSD_INVENTORY_INDEX, IRSM_IRS_DATE, IRSM_IRS_SECTION, CS_SEC_NAME " & _
                                "FROM INVENTORY_REQUISITION_SLIP_DETAILS " & _
                                "INNER JOIN INVENTORY_REQUISITION_SLIP_MSTR ON IRSD_IRS_COY_ID = IRSM_IRS_COY_ID AND IRSD_IRS_NO = IRSM_IRS_NO " & _
                                "INNER JOIN COMPANY_SECTION ON CS_SEC_CODE = IRSM_IRS_SECTION AND IRSM_IRS_COY_ID = CS_COY_ID " & _
                                "INNER JOIN " & _
                                "(SELECT MAX(IRSM_IRS_INDEX) AS ID, IRSD_INVENTORY_INDEX AS ITEM_ID FROM INVENTORY_REQUISITION_SLIP_DETAILS " & _
                                "INNER JOIN INVENTORY_REQUISITION_SLIP_MSTR ON IRSD_IRS_COY_ID = IRSM_IRS_COY_ID AND IRSD_IRS_NO = IRSM_IRS_NO " & _
                                "WHERE IRSM_IRS_COY_ID = @prmCoyID AND IRSM_IRS_DATE <= DATE_ADD(@prmDate, INTERVAL -6 MONTH) " & _
                                "AND (IRSM_IRS_STATUS = '3' OR IRSM_IRS_STATUS = '4') GROUP BY IRSD_INVENTORY_INDEX) tb_b " & _
                                "ON IRSM_IRS_INDEX = ID AND IRSD_INVENTORY_INDEX = ITEM_ID) tb_b " & _
                                "ON IM_INVENTORY_INDEX = tb_b.IRSD_INVENTORY_INDEX " & _
                                "LEFT JOIN " & _
                                "(SELECT IC_INVENTORY_INDEX, IC_COST_DATE AS IC_COST_DATE, IC_COST_CLOSE_QTY, IC_COST_CLOSE_COST " & _
                                "FROM INVENTORY_COST " & _
                                "INNER JOIN " & _
                                "(SELECT MAX(IC_COST_INDEX) AS IC_INDEX, IC_INVENTORY_INDEX AS IC_ITEM " & _
                                "FROM INVENTORY_COST " & _
                                "WHERE IC_COY_ID = @prmCoyID AND IC_COST_DATE <= DATE_ADD(@prmDate, INTERVAL -6 MONTH) " & _
                                "AND IC_INVENTORY_TYPE = 'II' GROUP BY IC_INVENTORY_INDEX) tb_b " & _
                                "ON IC_COST_INDEX = IC_INDEX AND IC_INVENTORY_INDEX = IC_ITEM) tb_c " & _
                                "ON IC_INVENTORY_INDEX = IM_INVENTORY_INDEX " & _
                                "INNER JOIN PRODUCT_MSTR ON IM_ITEM_CODE = PM_VENDOR_ITEM_CODE AND IM_COY_ID = PM_S_COY_ID " & _
                                "WHERE IM_INVENTORY_INDEX NOT IN " & _
                                "(SELECT IRSD_INVENTORY_INDEX FROM INVENTORY_REQUISITION_SLIP_MSTR, INVENTORY_REQUISITION_SLIP_DETAILS " & _
                                "WHERE IRSM_IRS_NO = IRSD_IRS_NO And IRSM_IRS_COY_ID = IRSD_IRS_COY_ID " & _
                                "AND (IRSM_IRS_STATUS = '3' OR IRSM_IRS_STATUS = '4') AND IRSM_IRS_COY_ID = @prmCoyID " & _
                                "AND (IRSM_IRS_DATE BETWEEN DATE_ADD(@prmDate, INTERVAL -6 MONTH) AND @prmDate)) " & _
                                "AND IM_COY_ID = @prmCoyID AND PM_OVERSEA = @prmOversea " & _
                                "ORDER BY PM_ACCT_CODE, IM_ITEM_CODE "

                ElseIf ViewState("type") = "DEADSTK" Then
                    .CommandText = "SELECT PM_ACCT_CODE, IM_ITEM_CODE, IM_INVENTORY_INDEX, IM_INVENTORY_NAME, " & _
                                "CONCAT(PM_SPEC1,' ',PM_SPEC2,' ',PM_SPEC3) AS SPEC, CS_SEC_NAME, IRSM_IRS_SECTION, IRSM_IRS_DATE, IC_COST_DATE, " & _
                                "IC_COST_CLOSE_QTY, IC_COST_CLOSE_COST FROM INVENTORY_MSTR " & _
                                "LEFT JOIN " & _
                                "(SELECT IRSD_INVENTORY_INDEX, MAX(IRSM_IRS_DATE) AS IRSM_IRS_DATE, IRSM_IRS_SECTION, CS_SEC_NAME FROM INVENTORY_REQUISITION_SLIP_DETAILS " & _
                                "INNER JOIN INVENTORY_REQUISITION_SLIP_MSTR ON IRSM_IRS_COY_ID = IRSD_IRS_COY_ID AND IRSD_IRS_NO=IRSM_IRS_NO " & _
                                "INNER JOIN COMPANY_SECTION ON CS_SEC_CODE = IRSM_IRS_SECTION AND IRSM_IRS_COY_ID= CS_COY_ID " & _
                                "WHERE IRSM_IRS_DATE <= DATE_ADD(@prmDate, INTERVAL -12 MONTH) AND IRSM_IRS_COY_ID = @prmCoyID " & _
                                "GROUP BY IRSD_INVENTORY_INDEX) tb_b " & _
                                "ON IM_INVENTORY_INDEX = tb_b.IRSD_INVENTORY_INDEX " & _
                                "LEFT JOIN " & _
                                "(SELECT IC_INVENTORY_INDEX, MAX(IC_COST_DATE) AS IC_COST_DATE, IC_COST_CLOSE_QTY, IC_COST_CLOSE_COST " & _
                                "FROM INVENTORY_COST " & _
                                "INNER JOIN INVENTORY_REQUISITION_SLIP_MSTR ON IC_COY_ID = IRSM_IRS_COY_ID AND IC_INVENTORY_REF_DOC = IRSM_IRS_NO " & _
                                "INNER JOIN INVENTORY_REQUISITION_SLIP_DETAILS ON IRSM_IRS_COY_ID = IRSD_IRS_COY_ID AND IRSM_IRS_NO = IRSD_IRS_NO " & _
                                "WHERE IC_COST_CLOSE_UPRICE <= DATE_ADD(@prmDate, INTERVAL -12 MONTH) AND IC_COY_ID = @prmCoyID " & _
                                "GROUP BY IC_INVENTORY_INDEX) tb_c " & _
                                "ON IC_INVENTORY_INDEX = IM_INVENTORY_INDEX " & _
                                "INNER JOIN PRODUCT_MSTR ON IM_ITEM_CODE = PM_VENDOR_ITEM_CODE AND IM_COY_ID = PM_S_COY_ID " & _
                                "WHERE IM_INVENTORY_INDEX NOT IN " & _
                                "(SELECT DISTINCT IRSD_INVENTORY_INDEX FROM INVENTORY_REQUISITION_SLIP_MSTR " & _
                                "INNER JOIN INVENTORY_REQUISITION_SLIP_DETAILS ON  IRSM_IRS_NO = IRSD_IRS_NO AND IRSM_IRS_COY_ID = IRSD_IRS_COY_ID " & _
                                "WHERE IRSM_IRS_COY_ID = @prmCoyID AND DATE_ADD(@prmDate, INTERVAL -12 MONTH) <= IRSM_IRS_DATE AND @prmDate >= IRSM_IRS_DATE) " & _
                                "AND IM_COY_ID = @prmCoyID AND PM_OVERSEA = @prmOversea " & _
                                "ORDER BY PM_ACCT_CODE, IM_ITEM_CODE "

                ElseIf ViewState("type") = "MTHMGMT" Then
                    'Get total of Active value & Closing Balance
                    dtDate = New DateTime(Me.cmbYear.SelectedValue, Me.cmbMonth.SelectedValue, 1)
                    strDate = Format(dtDate, "yyyy-MM-dd")

                    strTemp = "SELECT SUM(IC_COST_CLOSE_COST) " & _
                                "FROM INVENTORY_MSTR " & _
                                "INNER JOIN INVENTORY_COST ON IC_INVENTORY_INDEX = IM_INVENTORY_INDEX " & _
                                "INNER JOIN PRODUCT_MSTR ON IM_COY_ID = PM_S_COY_ID AND IM_ITEM_CODE = PM_VENDOR_ITEM_CODE " & _
                                "INNER JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID WHERE IM_COY_ID = '" & Session("CompanyId") & "' AND PM_OVERSEA = '" & cmbOversea.SelectedValue & "' " & _
                                "AND MONTH(IC_COST_DATE) = MONTH('" & strDate & "') AND YEAR(IC_COST_DATE) = YEAR('" & strDate & "') " & _
                                "AND (IC_INVENTORY_TYPE = 'WO' OR IC_INVENTORY_TYPE='GRN' OR IC_INVENTORY_TYPE='II') "
                    decClosingBal = objDb.GetVal(strTemp)

                    strTemp = "SELECT SUM(1) FROM INVENTORY_MSTR " & _
                            "INNER JOIN PRODUCT_MSTR ON IM_ITEM_CODE = PM_VENDOR_ITEM_CODE AND IM_COY_ID = PM_S_COY_ID " & _
                            "INNER JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID " & _
                            "INNER JOIN (SELECT PM_ACCT_CODE AS ACCT_CODE, CT_NAME AS CT FROM " & _
                            "INVENTORY_MSTR " & _
                            "INNER JOIN INVENTORY_COST ON IC_INVENTORY_INDEX = IM_INVENTORY_INDEX " & _
                            "INNER JOIN PRODUCT_MSTR ON IM_COY_ID = PM_S_COY_ID AND IM_ITEM_CODE = PM_VENDOR_ITEM_CODE " & _
                            "INNER JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID WHERE IM_COY_ID = '" & Session("CompanyId") & "' AND PM_OVERSEA = '" & cmbOversea.SelectedValue & "' " & _
                            "AND MONTH(IC_COST_DATE) = MONTH('" & strDate & "') AND YEAR(IC_COST_DATE) = YEAR('" & strDate & "') " & _
                            "AND (IC_INVENTORY_TYPE = 'WO' OR IC_INVENTORY_TYPE='GRN' OR IC_INVENTORY_TYPE='II') " & _
                            "GROUP BY PM_ACCT_CODE, CT_NAME) tb " & _
                            "ON PM_ACCT_CODE = ACCT_CODE AND CT_NAME = CT " & _
                            "WHERE IM_INVENTORY_INDEX IN " & _
                            "(SELECT IRSD_INVENTORY_INDEX  FROM INVENTORY_REQUISITION_SLIP_MSTR " & _
                            "INNER JOIN INVENTORY_REQUISITION_SLIP_DETAILS ON IRSM_IRS_NO = IRSD_IRS_NO AND IRSM_IRS_COY_ID = IRSD_IRS_COY_ID " & _
                            "WHERE MONTH(IRSM_IRS_DATE) BETWEEN MONTH('" & strDate & "' - INTERVAL 12 MONTH) AND MONTH('" & strDate & "') " & _
                            "AND YEAR(IRSM_IRS_DATE) BETWEEN YEAR('" & strDate & "' - INTERVAL 12 MONTH) AND YEAR('" & strDate & "')) " & _
                            "AND IM_COY_ID = '" & Session("CompanyId") & "' AND PM_OVERSEA = '" & cmbOversea.SelectedValue & "' "
                    intActive = objDb.GetVal(strTemp)

                    .CommandText = "SELECT tb_a.PM_ACCT_CODE AS PM_ACCT_CODE, tb_a.CT_NAME AS CT_NAME, NOOFITEM AS IM_ITEM_CODE, IC_COST_OPEN_COST AS OPENINGBALANCE, " & _
                                "IC_COST_CLOSE_COST AS CLOSINGBALANCE, RECEIVED, WRITEOFF, ISSUED, IFNULL(SLOWMOVING,0) AS SLOWMOVING, IFNULL(DEADSTOCK,0) AS DEADSTOCK, IFNULL(ACTIVESTOCK,0) AS ACTIVESTOCK, " & _
                                "IFNULL((IC_COST_CLOSE_COST / " & decClosingBal & " * 100),0) AS CB_PERC, IFNULL((ACTIVESTOCK / " & intActive & " * 100),0) AS AS_PERC FROM " & _
                                "(SELECT PM_ACCT_CODE, CT_NAME, COUNT(IM_ITEM_CODE) AS NOOFITEM, SUM(IC_COST_OPEN_COST) AS IC_COST_OPEN_COST, " & _
                                "SUM(IC_COST_CLOSE_COST) AS IC_COST_CLOSE_COST, SUM(RECEIVED) AS RECEIVED, SUM(WRITEOFF) AS WRITEOFF, SUM(ISSUED) AS ISSUED " & _
                                "FROM " & _
                                "(SELECT PM_ACCT_CODE, CT_NAME, IM_ITEM_CODE, IC_COST_OPEN_COST, IC_COST_CLOSE_COST, " & _
                                "CASE WHEN IC_INVENTORY_TYPE='GRN' THEN IC_COST_COST ELSE 0 END AS RECEIVED, " & _
                                "CASE WHEN IC_INVENTORY_TYPE='WO' THEN IC_COST_COST ELSE 0 END AS WRITEOFF, " & _
                                "CASE WHEN IC_INVENTORY_TYPE='II' THEN IC_COST_COST ELSE 0 END AS ISSUED " & _
                                "FROM INVENTORY_MSTR " & _
                                "INNER JOIN INVENTORY_COST ON IC_INVENTORY_INDEX = IM_INVENTORY_INDEX " & _
                                "INNER JOIN PRODUCT_MSTR ON IM_COY_ID = PM_S_COY_ID AND IM_ITEM_CODE = PM_VENDOR_ITEM_CODE " & _
                                "INNER JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID WHERE IM_COY_ID = @prmCoyID AND PM_OVERSEA = @prmOversea " & _
                                "AND (IC_INVENTORY_TYPE = 'WO' OR IC_INVENTORY_TYPE='GRN' OR IC_INVENTORY_TYPE='II') " & _
                                "AND MONTH(IC_COST_DATE) = MONTH(@prmDate) AND YEAR(IC_COST_DATE) = YEAR(@prmDate)) tb " & _
                                "GROUP BY PM_ACCT_CODE, CT_NAME) tb_a " & _
                                "LEFT JOIN " & _
                                "(SELECT PM_ACCT_CODE, CT_NAME, SUM(1) AS SLOWMOVING FROM INVENTORY_MSTR " & _
                                "INNER JOIN PRODUCT_MSTR ON IM_ITEM_CODE = PM_VENDOR_ITEM_CODE AND IM_COY_ID = PM_S_COY_ID " & _
                                "INNER JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID " & _
                                "WHERE IM_INVENTORY_INDEX NOT IN " & _
                                "(SELECT IRSD_INVENTORY_INDEX FROM INVENTORY_REQUISITION_SLIP_MSTR " & _
                                "INNER JOIN INVENTORY_REQUISITION_SLIP_DETAILS ON IRSM_IRS_NO = IRSD_IRS_NO AND IRSM_IRS_COY_ID = IRSD_IRS_COY_ID " & _
                                "WHERE (MONTH(IRSM_IRS_DATE) BETWEEN MONTH(@prmDate - INTERVAL 6 MONTH) AND MONTH(@prmDate)) " & _
                                "AND (YEAR(IRSM_IRS_DATE) BETWEEN YEAR(@prmDate - INTERVAL 6 MONTH) AND YEAR(@prmDate))) " & _
                                "AND IM_COY_ID = @prmCoyID AND PM_OVERSEA = @prmOversea " & _
                                "GROUP BY PM_ACCT_CODE, CT_NAME) tb_b " & _
                                "ON tb_a.PM_ACCT_CODE = tb_b.PM_ACCT_CODE AND tb_a.CT_NAME = tb_b.CT_NAME " & _
                                "LEFT JOIN " & _
                                "(SELECT PM_ACCT_CODE, CT_NAME, SUM(1) AS DEADSTOCK FROM INVENTORY_MSTR " & _
                                "INNER JOIN PRODUCT_MSTR ON IM_ITEM_CODE = PM_VENDOR_ITEM_CODE AND IM_COY_ID = PM_S_COY_ID " & _
                                "INNER JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID " & _
                                "WHERE IM_INVENTORY_INDEX NOT IN " & _
                                "(SELECT DISTINCT IRSD_INVENTORY_INDEX  FROM INVENTORY_REQUISITION_SLIP_MSTR " & _
                                "INNER JOIN INVENTORY_REQUISITION_SLIP_DETAILS ON IRSM_IRS_NO = IRSD_IRS_NO AND IRSM_IRS_COY_ID = IRSD_IRS_COY_ID " & _
                                "WHERE (MONTH(IRSM_IRS_DATE) BETWEEN MONTH(@prmDate - INTERVAL 12 MONTH) AND MONTH(@prmDate)) " & _
                                "AND (YEAR(IRSM_IRS_DATE) BETWEEN YEAR(@prmDate - INTERVAL 12 MONTH) AND YEAR(@prmDate))) " & _
                                "AND IM_COY_ID = @prmCoyID AND PM_OVERSEA = @prmOversea " & _
                                "GROUP BY PM_ACCT_CODE, CT_NAME) tb_c " & _
                                "ON tb_a.PM_ACCT_CODE = tb_c.PM_ACCT_CODE AND tb_a.CT_NAME = tb_c.CT_NAME " & _
                                "LEFT JOIN " & _
                                "(SELECT PM_ACCT_CODE, CT_NAME, SUM(1) AS ACTIVESTOCK FROM INVENTORY_MSTR " & _
                                "INNER JOIN PRODUCT_MSTR ON IM_ITEM_CODE = PM_VENDOR_ITEM_CODE AND IM_COY_ID = PM_S_COY_ID " & _
                                "INNER JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID " & _
                                "WHERE IM_INVENTORY_INDEX IN " & _
                                "(SELECT IRSD_INVENTORY_INDEX  FROM INVENTORY_REQUISITION_SLIP_MSTR " & _
                                "INNER JOIN INVENTORY_REQUISITION_SLIP_DETAILS ON IRSM_IRS_NO = IRSD_IRS_NO AND IRSM_IRS_COY_ID = IRSD_IRS_COY_ID " & _
                                "WHERE MONTH(IRSM_IRS_DATE) BETWEEN MONTH(@prmDate - INTERVAL 12 MONTH) AND MONTH(@prmDate) " & _
                                "AND YEAR(IRSM_IRS_DATE) BETWEEN YEAR(@prmDate - INTERVAL 12 MONTH) AND YEAR(@prmDate)) " & _
                                "AND IM_COY_ID = @prmCoyID AND PM_OVERSEA = @prmOversea " & _
                                "GROUP BY PM_ACCT_CODE, CT_NAME) tb_d " & _
                                "ON tb_a.PM_ACCT_CODE = tb_d.PM_ACCT_CODE AND tb_a.CT_NAME = tb_d.CT_NAME "

                ElseIf ViewState("type") = "PREALERT" Then
                    .CommandText &= "SELECT tb_a.*, IFNULL(STOCKONHANDQTY,0) AS STOCKONHANDQTY FROM " & _
                                "(SELECT POD_VENDOR_ITEM_CODE AS POD_PRODUCT_CODE, POD_PRODUCT_DESC, PM_SPEC1, PM_SPEC2, PM_SPEC3, DOL_LOT_NO, DOL_LOT_INDEX, " & _
                                "SUM(GD_RECEIVED_QTY) AS GRNRECEIVEDQTY, DOL_DO_EXP_DT, IM_INVENTORY_INDEX " & _
                                "FROM grn_mstr " & _
                                "INNER JOIN grn_details ON GM_B_COY_ID = GD_B_COY_ID AND GM_GRN_NO = GD_GRN_NO " & _
                                "INNER JOIN do_mstr ON GM_DO_INDEX = DOM_DO_INDEX " & _
                                "INNER JOIN do_details ON DOM_S_COY_ID = DOD_S_COY_ID AND DOD_DO_NO = DOM_DO_NO " & _
                                "INNER JOIN po_mstr ON POM_PO_INDEX = GM_PO_INDEX " & _
                                "INNER JOIN po_details ON POD_COY_ID =POM_B_COY_ID AND POM_PO_NO = POD_PO_NO AND POD_PO_LINE = DOD_PO_LINE  " & _
                                "INNER JOIN do_lot ON DOL_COY_ID=DOD_S_COY_ID AND DOL_DO_NO=DOD_DO_NO AND DOL_ITEM_CODE = POD_VENDOR_ITEM_CODE " & _
                                "INNER JOIN product_mstr ON PM_S_COY_ID = GM_B_COY_ID AND PM_VENDOR_ITEM_CODE = POD_VENDOR_ITEM_CODE " & _
                                "INNER JOIN inventory_mstr ON IM_COY_ID = GM_B_COY_ID AND POD_VENDOR_ITEM_CODE = IM_ITEM_CODE " & _
                                "WHERE GM_B_COY_ID =@prmCoyID AND PM_IQC_IND = 'Y' " & _
                                "AND DOL_DO_EXP_DT >= DATE_ADD(@prmStartDate, INTERVAL -2 MONTH) AND DOL_DO_EXP_DT <= @prmEndDate " & _
                                "GROUP BY  DOL_DO_NO, POD_PRODUCT_CODE) tb_a " & _
                                "Left Join " & _
                                "(SELECT IL_INVENTORY_INDEX, IL_LOT_INDEX, SUM(IL_LOT_QTY) AS STOCKONHANDQTY " & _
                                "FROM inventory_lot " & _
                                "GROUP BY IL_INVENTORY_INDEX, IL_LOT_INDEX) tb_b " & _
                                "ON tb_a.IM_INVENTORY_INDEX = tb_b.IL_INVENTORY_INDEX AND tb_a.DOL_LOT_INDEX=tb_b.IL_LOT_INDEX "

                ElseIf ViewState("type") = "MTHISSUE" Then
                    .CommandText = "SELECT CS_SEC_INDEX, IRSM_IRS_SECTION, CS_SEC_NAME, COUNT(IRSD_IRS_NO) AS COUNT_MRS, " & _
                                "SUM(IC_COST_QTY) AS IC_COST_QTY, SUM(IC_COST_UPRICE) AS IC_COST_UPRICE, SUM(IC_COST_COST) AS IC_COST_COST " & _
                                "FROM INVENTORY_REQUISITION_SLIP_MSTR " & _
                                "INNER JOIN INVENTORY_REQUISITION_SLIP_DETAILS ON IRSM_IRS_COY_ID = IRSD_IRS_COY_ID AND IRSM_IRS_NO = IRSD_IRS_NO " & _
                                "INNER JOIN INVENTORY_MSTR ON IRSD_INVENTORY_INDEX = IM_INVENTORY_INDEX " & _
                                "INNER JOIN COMPANY_SECTION ON IRSM_IRS_SECTION = CS_SEC_CODE AND IRSM_IRS_COY_ID = CS_COY_ID " & _
                                "INNER JOIN INVENTORY_COST ON IRSD_IRS_COY_ID = IC_COY_ID AND IRSD_IRS_NO = IC_INVENTORY_REF_DOC AND IRSD_INVENTORY_INDEX = IC_INVENTORY_INDEX " & _
                                "AND IC_INVENTORY_TYPE = 'II' AND IRSD_QTY = IC_COST_QTY " & _
                                "WHERE (IRSM_IRS_STATUS = '3' OR IRSM_IRS_STATUS = '4') AND IRSM_IRS_COY_ID = @prmCoyID " & _
                                "AND IRSM_IRS_APPROVED_DATE >= @prmStartDate AND IRSM_IRS_APPROVED_DATE <= @prmEndDate " & _
                                "GROUP BY CS_SEC_INDEX ORDER BY IRSM_IRS_SECTION "

                ElseIf ViewState("type") = "ITEMMTRS" Then
                    .CommandText = "SELECT IM_INVENTORY_INDEX, IM_ITEM_CODE, IM_INVENTORY_NAME, " & _
                                "CAST(CONCAT(PM_SPEC1,' ',PM_SPEC2,' ',PM_SPEC3) AS CHAR(250)) AS ITEMSPEC, PM_SAFE_QTY, " & _
                                "IFNULL(MONTHCONSUMPTION,0.00) AS MONTHCONSUMPTION, IRSM_IRS_DATE, " & _
                                "IFNULL(SUM(POD_ORDERED_QTY - (POD_RECEIVED_QTY - POD_REJECTED_QTY)),0.00) AS POD_BAL_QTY, " & _
                                "IFNULL(OP_CURR_MTH_AMT,0.00) AS OP_CURR_MTH_AMT, IFNULL(OP_CURR_MTH_QTY,0.00) AS OP_CURR_MTH_QTY, IFNULL(OP_PAST_1_MTH_QTY,0.00) AS OP_PAST_1_MTH_QTY, IFNULL(OP_PAST_2_MTH_QTY,0.00) AS OP_PAST_2_MTH_QTY, " & _
                                "IFNULL(OP_PAST_3_MTH_QTY,0.00) AS OP_PAST_3_MTH_QTY, IFNULL(OP_PAST_4_MTH_QTY,0.00) AS OP_PAST_4_MTH_QTY, IFNULL(OP_PAST_5_MTH_QTY,0.00) AS OP_PAST_5_MTH_QTY, IFNULL(OP_PAST_6_MTH_QTY,0.00) AS OP_PAST_6_MTH_QTY, " & _
                                "IFNULL(GRN_CURR_MTH_AMT,0.00) AS GRN_CURR_MTH_AMT, IFNULL(GRN_CURR_MTH_QTY,0.00) AS GRN_CURR_MTH_QTY, IFNULL(GRN_PAST_1_MTH_QTY,0.00) AS GRN_PAST_1_MTH_QTY, IFNULL(GRN_PAST_2_MTH_QTY,0.00) AS GRN_PAST_2_MTH_QTY, " & _
                                "IFNULL(GRN_PAST_3_MTH_QTY,0.00) AS GRN_PAST_3_MTH_QTY, IFNULL(GRN_PAST_4_MTH_QTY,0.00) AS GRN_PAST_4_MTH_QTY, IFNULL(GRN_PAST_5_MTH_QTY,0.00) AS GRN_PAST_5_MTH_QTY, IFNULL(GRN_PAST_6_MTH_QTY,0.00) AS GRN_PAST_6_MTH_QTY, " & _
                                "(IFNULL(II_CURR_MTH_AMT,0.00) - IFNULL(IIC_CURR_MTH_AMT,0.00)) AS II_CURR_MTH_AMT, (IFNULL(II_CURR_MTH_QTY,0.00) - IFNULL(IIC_CURR_MTH_QTY,0.00)) AS II_CURR_MTH_QTY, " & _
                                "(IFNULL(II_PAST_1_MTH_QTY,0.00) - IFNULL(IIC_PAST_1_MTH_QTY,0.00)) AS II_PAST_1_MTH_QTY, (IFNULL(II_PAST_2_MTH_QTY,0.00) - IFNULL(IIC_PAST_2_MTH_QTY,0.00)) AS II_PAST_2_MTH_QTY, " & _
                                "(IFNULL(II_PAST_3_MTH_QTY,0.00) - IFNULL(IIC_PAST_3_MTH_QTY,0.00)) AS II_PAST_3_MTH_QTY, (IFNULL(II_PAST_4_MTH_QTY,0.00) - IFNULL(IIC_PAST_4_MTH_QTY,0.00)) AS II_PAST_4_MTH_QTY, " & _
                                "(IFNULL(II_PAST_5_MTH_QTY,0.00) - IFNULL(IIC_PAST_5_MTH_QTY,0.00)) AS II_PAST_5_MTH_QTY, (IFNULL(II_PAST_6_MTH_QTY,0.00) - IFNULL(IIC_PAST_6_MTH_QTY,0.00)) AS II_PAST_6_MTH_QTY, " & _
                                "IFNULL(RI_CURR_MTH_AMT,0.00) AS RI_CURR_MTH_AMT, IFNULL(RI_CURR_MTH_QTY,0.00) AS RI_CURR_MTH_QTY, IFNULL(RI_PAST_1_MTH_QTY,0.00) AS RI_PAST_1_MTH_QTY, IFNULL(RI_PAST_2_MTH_QTY,0.00) AS RI_PAST_2_MTH_QTY, " & _
                                "IFNULL(RI_PAST_3_MTH_QTY,0.00) AS RI_PAST_3_MTH_QTY, IFNULL(RI_PAST_4_MTH_QTY,0.00) AS RI_PAST_4_MTH_QTY, IFNULL(RI_PAST_5_MTH_QTY,0.00) AS RI_PAST_5_MTH_QTY, IFNULL(RI_PAST_6_MTH_QTY,0.00) AS RI_PAST_6_MTH_QTY, " & _
                                "IFNULL(WO_CURR_MTH_AMT,0.00) AS WO_CURR_MTH_AMT, IFNULL(WO_CURR_MTH_QTY,0.00) AS WO_CURR_MTH_QTY, IFNULL(WO_PAST_1_MTH_QTY,0.00) AS WO_PAST_1_MTH_QTY, IFNULL(WO_PAST_2_MTH_QTY,0.00) AS WO_PAST_2_MTH_QTY, " & _
                                "IFNULL(WO_PAST_3_MTH_QTY,0.00) AS WO_PAST_3_MTH_QTY, IFNULL(WO_PAST_4_MTH_QTY,0.00) AS WO_PAST_4_MTH_QTY, IFNULL(WO_PAST_5_MTH_QTY,0.00) AS WO_PAST_5_MTH_QTY, IFNULL(WO_PAST_6_MTH_QTY,0.00) AS WO_PAST_6_MTH_QTY, " & _
                                "IFNULL(SOH_CURR_MTH_AMT,0.00) AS SOH_CURR_MTH_AMT, IFNULL(SOH_CURR_MTH_QTY,0.00) AS SOH_CURR_MTH_QTY, IFNULL(SOH_PAST_1_MTH_QTY,0.00) AS SOH_PAST_1_MTH_QTY, IFNULL(SOH_PAST_2_MTH_QTY,0.00) AS SOH_PAST_2_MTH_QTY, " & _
                                "IFNULL(SOH_PAST_3_MTH_QTY,0.00) AS SOH_PAST_3_MTH_QTY, IFNULL(SOH_PAST_4_MTH_QTY,0.00) AS SOH_PAST_4_MTH_QTY, IFNULL(SOH_PAST_5_MTH_QTY,0.00) AS SOH_PAST_5_MTH_QTY, IFNULL(SOH_PAST_6_MTH_QTY,0.00) AS SOH_PAST_6_MTH_QTY " & _
                                "FROM (SELECT IM_INVENTORY_INDEX, IM_ITEM_CODE, IM_INVENTORY_NAME, IM_COY_ID, " & _
                                "(SELECT SUM(IC_COST_OPEN_COST) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='OPENING' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate) AND YEAR(IC_COST_DATE)=YEAR(@prmDate)) AS OP_CURR_MTH_AMT, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='OPENING' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate) AND YEAR(IC_COST_DATE)=YEAR(@prmDate)) AS OP_CURR_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='OPENING' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 1 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 1 MONTH)) AS OP_PAST_1_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='OPENING' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 2 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 2 MONTH)) AS OP_PAST_2_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='OPENING' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 3 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 3 MONTH)) AS OP_PAST_3_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='OPENING' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 4 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 4 MONTH)) AS OP_PAST_4_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='OPENING' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 5 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 5 MONTH)) AS OP_PAST_5_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='OPENING' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 6 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 6 MONTH)) AS OP_PAST_6_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_COST) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='GRN' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate) AND YEAR(IC_COST_DATE)=YEAR(@prmDate)) AS GRN_CURR_MTH_AMT, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='GRN' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate) AND YEAR(IC_COST_DATE)=YEAR(@prmDate)) AS GRN_CURR_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='GRN' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 1 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 1 MONTH)) AS GRN_PAST_1_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='GRN' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 2 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 2 MONTH)) AS GRN_PAST_2_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='GRN' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 3 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 3 MONTH)) AS GRN_PAST_3_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='GRN' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 4 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 4 MONTH)) AS GRN_PAST_4_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='GRN' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 5 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 5 MONTH)) AS GRN_PAST_5_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='GRN' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 6 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 6 MONTH)) AS GRN_PAST_6_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_COST) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate) AND YEAR(IC_COST_DATE)=YEAR(@prmDate)) AS II_CURR_MTH_AMT, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate) AND YEAR(IC_COST_DATE)=YEAR(@prmDate)) AS II_CURR_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 1 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 1 MONTH)) AS II_PAST_1_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 2 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 2 MONTH)) AS II_PAST_2_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 3 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 3 MONTH)) AS II_PAST_3_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 4 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 4 MONTH)) AS II_PAST_4_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 5 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 5 MONTH)) AS II_PAST_5_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 6 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 6 MONTH)) AS II_PAST_6_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_COST) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='IIC' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate) AND YEAR(IC_COST_DATE)=YEAR(@prmDate)) AS IIC_CURR_MTH_AMT, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='IIC' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate) AND YEAR(IC_COST_DATE)=YEAR(@prmDate)) AS IIC_CURR_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='IIC' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 1 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 1 MONTH)) AS IIC_PAST_1_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='IIC' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 2 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 2 MONTH)) AS IIC_PAST_2_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='IIC' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 3 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 3 MONTH)) AS IIC_PAST_3_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='IIC' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 4 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 4 MONTH)) AS IIC_PAST_4_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='IIC' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 5 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 5 MONTH)) AS IIC_PAST_5_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='IIC' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 6 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 6 MONTH)) AS IIC_PAST_6_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_COST) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='RI' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate) AND YEAR(IC_COST_DATE)=YEAR(@prmDate)) AS RI_CURR_MTH_AMT, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='RI' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate) AND YEAR(IC_COST_DATE)=YEAR(@prmDate)) AS RI_CURR_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='RI' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 1 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 1 MONTH)) AS RI_PAST_1_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='RI' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 2 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 2 MONTH)) AS RI_PAST_2_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='RI' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 3 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 3 MONTH)) AS RI_PAST_3_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='RI' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 4 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 4 MONTH)) AS RI_PAST_4_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='RI' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 5 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 5 MONTH)) AS RI_PAST_5_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='RI' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 6 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 6 MONTH)) AS RI_PAST_6_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_COST) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='WO' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate) AND YEAR(IC_COST_DATE)=YEAR(@prmDate)) AS WO_CURR_MTH_AMT, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='WO' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate) AND YEAR(IC_COST_DATE)=YEAR(@prmDate)) AS WO_CURR_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='WO' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 1 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 1 MONTH)) AS WO_PAST_1_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='WO' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 2 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 2 MONTH)) AS WO_PAST_2_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='WO' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 3 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 3 MONTH)) AS WO_PAST_3_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='WO' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 4 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 4 MONTH)) AS WO_PAST_4_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='WO' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 5 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 5 MONTH)) AS WO_PAST_5_MTH_QTY, " & _
                                "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='WO' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 6 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 5 MONTH)) AS WO_PAST_6_MTH_QTY " & _
                                "FROM INVENTORY_MSTR WHERE IM_COY_ID = @prmCoyID) tb_a " & _
                                "LEFT JOIN " & _
                                "(SELECT DISTINCT IC_INVENTORY_INDEX, " & _
                                "(SELECT IC_COST_OPEN_COST FROM INVENTORY_COST WHERE IC_INVENTORY_INDEX = IC.IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate) AND YEAR(IC_COST_DATE)=YEAR(@prmDate) ORDER BY IC_COST_INDEX DESC LIMIT 1) AS SOH_CURR_MTH_AMT, " & _
                                "(SELECT IC_COST_OPEN_QTY FROM INVENTORY_COST WHERE IC_INVENTORY_INDEX = IC.IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate) AND YEAR(IC_COST_DATE)=YEAR(@prmDate) ORDER BY IC_COST_INDEX DESC LIMIT 1) AS SOH_CURR_MTH_QTY, " & _
                                "(SELECT IC_COST_OPEN_QTY FROM INVENTORY_COST WHERE IC_INVENTORY_INDEX = IC.IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 1 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 1 MONTH) ORDER BY IC_COST_INDEX DESC LIMIT 1) AS SOH_PAST_1_MTH_QTY, " & _
                                "(SELECT IC_COST_OPEN_QTY FROM INVENTORY_COST WHERE IC_INVENTORY_INDEX = IC.IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 2 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 2 MONTH) ORDER BY IC_COST_INDEX DESC LIMIT 1) AS SOH_PAST_2_MTH_QTY, " & _
                                "(SELECT IC_COST_OPEN_QTY FROM INVENTORY_COST WHERE IC_INVENTORY_INDEX = IC.IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 3 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 3 MONTH) ORDER BY IC_COST_INDEX DESC LIMIT 1) AS SOH_PAST_3_MTH_QTY, " & _
                                "(SELECT IC_COST_OPEN_QTY FROM INVENTORY_COST WHERE IC_INVENTORY_INDEX = IC.IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 4 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 4 MONTH) ORDER BY IC_COST_INDEX DESC LIMIT 1) AS SOH_PAST_4_MTH_QTY, " & _
                                "(SELECT IC_COST_OPEN_QTY FROM INVENTORY_COST WHERE IC_INVENTORY_INDEX = IC.IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 5 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 5 MONTH) ORDER BY IC_COST_INDEX DESC LIMIT 1) AS SOH_PAST_5_MTH_QTY, " & _
                                "(SELECT IC_COST_OPEN_QTY FROM INVENTORY_COST WHERE IC_INVENTORY_INDEX = IC.IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 6 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 6 MONTH) ORDER BY IC_COST_INDEX DESC LIMIT 1) AS SOH_PAST_6_MTH_QTY " & _
                                "FROM INVENTORY_COST IC " & _
                                "WHERE IC_COY_ID = @prmCoyID) tb_b " & _
                                "ON tb_b.IC_INVENTORY_INDEX = tb_a.IM_INVENTORY_INDEX " & _
                                "INNER JOIN PRODUCT_MSTR ON IM_ITEM_CODE = PM_VENDOR_ITEM_CODE AND IM_COY_ID = PM_S_COY_ID " & _
                                "LEFT JOIN PO_DETAILS ON POD_COY_ID = IM_COY_ID AND POD_VENDOR_ITEM_CODE = IM_ITEM_CODE " & _
                                "LEFT JOIN " & _
                                "(SELECT IRSD_INVENTORY_INDEX, MAX(IRSM_IRS_DATE) AS IRSM_IRS_DATE, SUM(IRSD_QTY) / 3 AS MONTHCONSUMPTION " & _
                                "FROM INVENTORY_REQUISITION_SLIP_DETAILS INNER JOIN INVENTORY_REQUISITION_SLIP_MSTR ON IRSM_IRS_COY_ID = IRSD_IRS_COY_ID  AND IRSM_IRS_NO = IRSD_IRS_NO " & _
                                "WHERE IRSM_IRS_COY_ID =@prmCoyID " & _
                                "AND IRSM_IRS_DATE BETWEEN (@prmDate - INTERVAL 3 MONTH) AND (@prmDate - INTERVAL 1 MONTH) " & _
                                "AND YEAR(IRSM_IRS_DATE) BETWEEN YEAR(@prmDate - INTERVAL 3 MONTH) AND YEAR(@prmDate - INTERVAL 1 MONTH) " & _
                                "GROUP BY IRSD_INVENTORY_INDEX) tb_c " & _
                                "ON tb_c.IRSD_INVENTORY_INDEX = tb_a.IM_INVENTORY_INDEX " & _
                                "WHERE PM_OVERSEA = @prmOversea " & _
                                "GROUP BY IM_INVENTORY_INDEX ORDER BY IM_ITEM_CODE "

                ElseIf ViewState("type") = "MTHCOSTTREND" Then
                    .CommandText = "SELECT PM_ACCT_CODE, IM_ITEM_CODE, IM_INVENTORY_NAME AS IC_INVENTORY_NAME, PM_SPEC1, PM_SPEC2, PM_SPEC3, " & _
                                "(SELECT GROUP_CONCAT(PV_PUR_SPEC_NO) FROM PIM_VENDOR WHERE PV_PRODUCT_INDEX = PM_PRODUCT_INDEX AND PV_PUR_SPEC_NO <> '' GROUP BY PV_PRODUCT_INDEX) AS PUR_SPEC_NO, " & _
                                "PM_BUDGET_PRICE, " & _
                                "IFNULL(PAST_1_MTH_UG,0.00) AS PAST_1_MTH_UG, IFNULL(PAST_2_MTH_UG,0.00) AS PAST_2_MTH_UG, IFNULL(PAST_3_MTH_UG,0.00) AS PAST_3_MTH_UG, " & _
                                "IFNULL(PAST_4_MTH_UG,0.00) AS PAST_4_MTH_UG, IFNULL(PAST_5_MTH_UG,0.00) AS PAST_5_MTH_UG, IFNULL(PAST_6_MTH_UG,0.00) AS PAST_6_MTH_UG, " & _
                                "IFNULL(PAST_1_MTH_MA,0.00) AS PAST_1_MTH_MA, IFNULL(PAST_2_MTH_MA,0.00) AS PAST_2_MTH_MA, IFNULL(PAST_3_MTH_MA,0.00) AS PAST_3_MTH_MA, " & _
                                "IFNULL(PAST_4_MTH_MA,0.00) AS PAST_4_MTH_MA, IFNULL(PAST_5_MTH_MA,0.00) AS PAST_5_MTH_MA, IFNULL(PAST_6_MTH_MA,0.00) AS PAST_6_MTH_MA, " & _
                                "IFNULL(PAST_1_MTH_LC,0.00) AS PAST_1_MTH_LC, IFNULL(PAST_2_MTH_LC,0.00) AS PAST_2_MTH_LC, IFNULL(PAST_3_MTH_LC,0.00) AS PAST_3_MTH_LC, " & _
                                "IFNULL(PAST_4_MTH_LC,0.00) AS PAST_4_MTH_LC, IFNULL(PAST_5_MTH_LC,0.00) AS PAST_5_MTH_LC, IFNULL(PAST_6_MTH_LC,0.00) AS PAST_6_MTH_LC, " & _
                                "PM_UOM " & _
                                "FROM (SELECT IM_INVENTORY_INDEX, IM_ITEM_CODE, IM_INVENTORY_NAME, IM_COY_ID, " & _
                                "(SELECT SUM(IC_COST_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 1 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 1 MONTH)) AS PAST_1_MTH_UG, " & _
                                "(SELECT SUM(IC_COST_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 2 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 2 MONTH)) AS PAST_2_MTH_UG, " & _
                                "(SELECT SUM(IC_COST_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 3 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 3 MONTH)) AS PAST_3_MTH_UG, " & _
                                "(SELECT SUM(IC_COST_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 4 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 4 MONTH)) AS PAST_4_MTH_UG, " & _
                                "(SELECT SUM(IC_COST_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 5 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 5 MONTH)) AS PAST_5_MTH_UG, " & _
                                "(SELECT SUM(IC_COST_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 6 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 6 MONTH)) AS PAST_6_MTH_UG, " & _
                                "(SELECT SUM(IC_COST_UPRICE) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 1 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 1 MONTH)) AS PAST_1_MTH_MA, " & _
                                "(SELECT SUM(IC_COST_UPRICE) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 2 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 2 MONTH)) AS PAST_2_MTH_MA, " & _
                                "(SELECT SUM(IC_COST_UPRICE) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 3 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 3 MONTH)) AS PAST_3_MTH_MA, " & _
                                "(SELECT SUM(IC_COST_UPRICE) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 4 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 4 MONTH)) AS PAST_4_MTH_MA, " & _
                                "(SELECT SUM(IC_COST_UPRICE) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 5 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 5 MONTH)) AS PAST_5_MTH_MA, " & _
                                "(SELECT SUM(IC_COST_UPRICE) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 6 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 6 MONTH)) AS PAST_6_MTH_MA, " & _
                                "(SELECT SUM(IC_COST_COST/IC_COST_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='GRN' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 1 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 1 MONTH)) AS PAST_1_MTH_LC, " & _
                                "(SELECT SUM(IC_COST_COST/IC_COST_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='GRN' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 2 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 2 MONTH)) AS PAST_2_MTH_LC, " & _
                                "(SELECT SUM(IC_COST_COST/IC_COST_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='GRN' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 3 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 3 MONTH)) AS PAST_3_MTH_LC, " & _
                                "(SELECT SUM(IC_COST_COST/IC_COST_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='GRN' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 4 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 4 MONTH)) AS PAST_4_MTH_LC, " & _
                                "(SELECT SUM(IC_COST_COST/IC_COST_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='GRN' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 5 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 5 MONTH)) AS PAST_5_MTH_LC, " & _
                                "(SELECT SUM(IC_COST_COST/IC_COST_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='GRN' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH(@prmDate - INTERVAL 6 MONTH) AND YEAR(IC_COST_DATE)=YEAR(@prmDate - INTERVAL 6 MONTH)) AS PAST_6_MTH_LC " & _
                                "FROM INVENTORY_MSTR WHERE IM_COY_ID = @prmCoyID) tb_a " & _
                                "INNER JOIN PRODUCT_MSTR ON PM_S_COY_ID = IM_COY_ID AND IM_ITEM_CODE = PM_VENDOR_ITEM_CODE " & _
                                "WHERE PM_OVERSEA = @prmOversea " & _
                                "ORDER BY PM_ACCT_CODE, IM_ITEM_CODE "

                ElseIf ViewState("type") = "MTHGRN" Then
                    .CommandText &= "SELECT '1' AS tb, PM_ACCT_CODE, PM_VENDOR_ITEM_CODE, PM_OVERSEA, PM_ITEM_TYPE, PM_PRODUCT_DESC, CM_COY_ID, " & _
                                "CM_COY_NAME, '' AS INV_ITM_TTL, '' AS DC_ITM_TTL, '' AS MAINTAINENCE_ITM_TTL, '' AS M_ITM_TTL,'' AS SC_ITM_TTL, SUS_AMOUNT, SHS_AMOUNT, US_AMOUNT, PS_AMOUNT, " & _
                                "SM_AMOUNT, CS_AMOUNT, Vendor_country, '' AS L_MAINTAINENCE_ITM_TTL, '' AS L_M_ITM_TTL FROM ( " & _
                                "SELECT *, " & _
                                "CASE WHEN  CT_NAME = 'Supplies Stock' THEN GD_RECEIVED_QTY - GD_REJECTED_QTY ELSE 0 END AS SUS_AMOUNT, " & _
                                "CASE WHEN  CT_NAME = 'Shipping Stock' THEN GD_RECEIVED_QTY - GD_REJECTED_QTY ELSE 0 END AS SHS_AMOUNT, " & _
                                "CASE WHEN  CT_NAME = 'Uniform & Safety' THEN GD_RECEIVED_QTY - GD_REJECTED_QTY ELSE 0 END AS US_AMOUNT, " & _
                                "CASE WHEN  CT_NAME = 'Printing & Stationery' THEN GD_RECEIVED_QTY - GD_REJECTED_QTY ELSE 0 END AS PS_AMOUNT, " & _
                                "CASE WHEN  CT_NAME = 'Sub-Material Stock' THEN GD_RECEIVED_QTY - GD_REJECTED_QTY ELSE 0 END AS SM_AMOUNT, " & _
                                "CASE WHEN  CT_NAME = 'Chemical Stock' THEN GD_RECEIVED_QTY - GD_REJECTED_QTY ELSE 0 END AS CS_AMOUNT, " & _
                                "CASE WHEN CM_COUNTRY='MY' THEN 'LOCAL SUPPLIER' " & _
                                "WHEN CM_COY_NAME='SEH JAPAN' AND CM_COUNTRY='JP' THEN 'SEH JAPAN' " & _
                                "WHEN CM_COY_NAME='SEH AMERICA' AND CM_COUNTRY='US' THEN 'SEH AMERICA'  " & _
                                "WHEN CM_COUNTRY <> 'MY' AND (CM_COY_NAME='SEH JAPAN' AND CM_COUNTRY = 'JP')  " & _
                                "AND (CM_COY_NAME<>'SEH AMERICA' AND CM_COUNTRY = 'US') THEN 'OTHER OVERSEA SUPPLIER' END AS Vendor_country " & _
                                "FROM grn_details  " & _
                                "INNER JOIN grn_mstr ON GM_B_COY_ID  = GD_B_COY_ID AND GM_GRN_NO = GD_GRN_NO " & _
                                "INNER JOIN po_mstr ON POM_PO_INDEX = GM_PO_INDEX " & _
                                "INNER JOIN po_details ON POM_B_COY_ID = POD_COY_ID AND POD_PO_LINE = GD_PO_LINE AND pom_po_no = pod_po_no " & _
                                "INNER JOIN product_mstr ON PM_S_COY_ID = POM_S_COY_ID AND PM_VENDOR_ITEM_CODE = POD_VENDOR_ITEM_CODE AND POM_PO_INDEX = GM_GRN_INDEX " & _
                                "INNER JOIN commodity_type ON PM_CATEGORY_NAME = CT_ID " & _
                                "INNER JOIN company_mstr ON CM_COY_ID = GM_B_COY_ID " & _
                                "WHERE (CT_NAME = 'Supplies Stock' OR CT_NAME = 'Shipping Stock' OR CT_NAME = 'Uniform & Safety' " & _
                                "OR CT_NAME = 'Printing & Stationery' OR CT_NAME = 'Sub-Material Stock' OR CT_NAME = 'Chemical Stock') AND GM_B_COY_ID = @prmCoyID)tb_a " & _
                                "UNION " & _
                                "SELECT '2' AS tb, PM_ACCT_CODE, PM_VENDOR_ITEM_CODE, PM_OVERSEA, PM_ITEM_TYPE, PM_PRODUCT_DESC, CM_COY_ID, CM_COY_NAME, INV_ITM_TTL, " & _
                                "DC_ITM_TTL, MAINTAINENCE_ITM_TTL, M_ITM_TTL, SC_ITM_TTL, '' AS SUS_AMOUNT, '' AS SHS_AMOUNT, '' AS US_AMOUNT, '' AS PS_AMOUNT, " & _
                                "'' AS SM_AMOUNT, '' AS CS_AMOUNT, Vendor_country, L_MAINTAINENCE_ITM_TTL, L_M_ITM_TTL FROM ( " & _
                                "SELECT * , " & _
                                "CASE WHEN PM_ITEM_TYPE = 'ST'  THEN 1 ELSE 0 END AS INV_ITM_TTL, " & _
                                "CASE WHEN PM_ITEM_TYPE = 'SP' THEN 1 ELSE 0 END AS DC_ITM_TTL, " & _
                                "CASE WHEN PM_ACCT_CODE = 'E08' THEN 1 ELSE 0 END AS MAINTAINENCE_ITM_TTL, " & _
                                "CASE WHEN PM_ACCT_CODE = 'E10' THEN 1 ELSE 0 END AS M_ITM_TTL, " & _
                                "CASE WHEN PM_ACCT_CODE = 'E09' THEN 1 ELSE 0 END AS SC_ITM_TTL, " & _
                                "CASE WHEN PM_ACCT_CODE = 'C08' THEN 1 ELSE 0 END AS L_MAINTAINENCE_ITM_TTL, " & _
                                "CASE WHEN PM_ACCT_CODE = 'C10' THEN 1 ELSE 0 END AS L_M_ITM_TTL, " & _
                                "CASE WHEN CM_COUNTRY='MY' THEN 'LOCAL SUPPLIER'   " & _
                                "WHEN CM_COY_NAME='SEH JAPAN' AND CM_COUNTRY='JP' THEN 'SEH JAPAN' " & _
                                "WHEN CM_COY_NAME='SEH AMERICA' AND CM_COUNTRY='US' THEN 'SEH AMERICA'  " & _
                                "WHEN CM_COUNTRY <> 'MY' AND (CM_COY_NAME='SEH JAPAN' AND CM_COUNTRY = 'JP')  " & _
                                "AND (CM_COY_NAME<>'SEH AMERICA' AND CM_COUNTRY = 'US') THEN 'OTHER OVERSEA SUPPLIER' END AS Vendor_country " & _
                                "FROM grn_details " & _
                                "INNER JOIN grn_mstr ON GM_B_COY_ID  = GD_B_COY_ID AND GM_GRN_NO = GD_GRN_NO " & _
                                "INNER JOIN po_mstr ON POM_PO_INDEX = GM_PO_INDEX " & _
                                "INNER JOIN po_details ON POM_B_COY_ID = POD_COY_ID AND POD_PO_LINE = GD_PO_LINE AND pom_po_no = pod_po_no " & _
                                "INNER JOIN product_mstr ON PM_S_COY_ID = POM_S_COY_ID AND PM_VENDOR_ITEM_CODE = POD_VENDOR_ITEM_CODE AND POM_PO_INDEX = GM_GRN_INDEX " & _
                                "INNER JOIN commodity_type ON PM_CATEGORY_NAME = CT_ID " & _
                                "INNER JOIN company_mstr ON CM_COY_ID = GM_B_COY_ID " & _
                                "WHERE (CT_NAME = 'Supplies Stock' OR CT_NAME = 'Shipping Stock' OR CT_NAME = 'Uniform & Safety' " & _
                                "OR CT_NAME = 'Printing & Stationery' OR CT_NAME = 'Sub-Material Stock' OR CT_NAME = 'Chemical Stock') AND GM_B_COY_ID =@prmCoyID AND MONTH(GM_DATE_RECEIVED)=@prmMonth AND YEAR(GM_DATE_RECEIVED)=@prmYear) tb_b "
                End If

            End With

            da = New MySqlDataAdapter(cmd)

            'GRN Listing
            If ViewState("ReportType") = "1" Or ViewState("ReportType") = "9" Or ViewState("ReportType") = "13" Then
                dtFrom = Me.txtSDate.Text
                dtTo = Me.txtEndDate.Text
                strBeginDate = Format(dtFrom, "yyyy-MM-dd" & " " & "00:00:00")
                strEndDate = Format(dtTo, "yyyy-MM-dd" & " " & "23:59:59")

                If ViewState("type") = "GRNLIST" Then
                    dtFrom = Me.txtGRNSDate.Text
                    dtTo = Me.txtGRNEndDate.Text
                    strGRNBeginDate = Format(dtFrom, "yyyy-MM-dd" & " " & "00:00:00")
                    strGRNEndDate = Format(dtTo, "yyyy-MM-dd" & " " & "23:59:59")
                End If

                'Parameter
                strPrmStart = Format(dtFrom, "dd-MM-yyyy")
                strPrmEnd = Format(dtTo, "dd-MM-yyyy")
             
            ElseIf ViewState("ReportType") = "2" Then
                If Me.cmbMonthFrom.SelectedIndex > 0 Then
                    dtDate = New DateTime(Me.cmbYearFrom.SelectedValue, Me.cmbMonthFrom.SelectedValue, 1)
                    dtFrom = dtDate

                    If Me.cmbMonthTo.SelectedValue < 12 Then
                        dtDate = New DateTime(Me.cmbYearTo.SelectedValue, Me.cmbMonthTo.SelectedValue + 1, 1)
                        dtTo = DateAdd(DateInterval.Day, -1, dtDate)
                        dtTo = New DateTime(Me.cmbYearTo.SelectedValue, Me.cmbMonthTo.SelectedValue, dtTo.Day)

                    Else
                        dtDate = New DateTime(Me.cmbYearFrom.SelectedValue, Me.cmbMonthFrom.SelectedValue, 1)
                        dtFrom = dtDate
                        dtTo = New DateTime(Me.cmbYearTo.SelectedValue, Me.cmbMonthTo.SelectedValue, 31)
                    End If

                Else
                    dtDate = New DateTime(Me.cmbYearFrom.SelectedValue, 1, 1)
                    dtFrom = dtDate
                    dtTo = New DateTime(Me.cmbYearTo.SelectedValue, 12, 31)
                End If

                strBeginDate = Format(dtFrom, "yyyy-MM-dd" & " " & "00:00:00")
                strEndDate = Format(dtTo, "yyyy-MM-dd" & " " & "23:59:59")

                'Parameter
                strPrmStart = UCase(Format(dtFrom, "MMM yyyy"))
                strPrmEnd = UCase(Format(dtTo, "MMM yyyy"))

            ElseIf ViewState("ReportType") = "3" Then
                strOversea = Me.cmbOversea.SelectedValue
                dtDate = Me.txtDate.Text
                strDate = Format(dtDate, "yyyy-MM-dd")

                'Parameter
                strPrmDate = Format(dtDate, "dd-MM-yyyy")

            ElseIf ViewState("ReportType") = "4" Then  '// sum monthlt stock balance
                strOversea = Me.cmbOversea.SelectedValue

                If ViewState("type") = "SUMMTHSTKBAL" Then
                    strMonth = Me.cmbMonth.SelectedValue
                    strYear = Me.cmbYear.SelectedValue

                    'Parameter
                    If Me.cmbMonth.SelectedIndex > 0 Then
                        dtDate = New DateTime(Me.cmbYear.SelectedValue, Me.cmbMonth.SelectedValue, 1)
                    End If
                    strPrmYear = Format(dtDate, "yyyy")
                    strPrmMonth = Format(dtDate, "MMM")

                ElseIf ViewState("type") = "MTHCOSTTREND" Or ViewState("type") = "MTHMGMT" Then
                    dtDate = New DateTime(Me.cmbYear.SelectedValue, Me.cmbMonth.SelectedValue, 1)
                    strDate = Format(dtDate, "yyyy-MM-dd")

                    'Parameter
                    strPrmYear = Format(dtDate, "yyyy")
                    strPrmMonth = Format(dtDate, "MMM")

                    dtDate = DateAdd(DateInterval.Month, -1, dtDate)
                    strPrmPast1Mth = Format(dtDate, "MMM-yyyy")
                    dtDate = DateAdd(DateInterval.Month, -1, dtDate)
                    strPrmPast2Mth = Format(dtDate, "MMM-yyyy")
                    dtDate = DateAdd(DateInterval.Month, -1, dtDate)
                    strPrmPast3Mth = Format(dtDate, "MMM-yyyy")
                    dtDate = DateAdd(DateInterval.Month, -1, dtDate)
                    strPrmPast4Mth = Format(dtDate, "MMM-yyyy")
                    dtDate = DateAdd(DateInterval.Month, -1, dtDate)
                    strPrmPast5Mth = Format(dtDate, "MMM-yyyy")
                    dtDate = DateAdd(DateInterval.Month, -1, dtDate)
                    strPrmPast6Mth = Format(dtDate, "MMM-yyyy")

                Else

                End If

            ElseIf ViewState("ReportType") = "5" Then
                If Me.cmbMonth.SelectedIndex > 0 Then
                    dtDate = New DateTime(Me.cmbYear.SelectedValue, Me.cmbMonth.SelectedValue, 1)
                    dtFrom = dtDate

                    If Me.cmbMonth.SelectedValue < 12 Then
                        dtDate = New DateTime(Me.cmbYear.SelectedValue, Me.cmbMonth.SelectedValue + 1, 1)
                        dtTo = DateAdd(DateInterval.Day, -1, dtDate)
                        dtTo = New DateTime(Me.cmbYear.SelectedValue, Me.cmbMonth.SelectedValue, dtTo.Day)

                    Else    'December
                        dtTo = New DateTime(Me.cmbYear.SelectedValue, Me.cmbMonth.SelectedValue, 31)
                    End If

                Else
                End If

                strBeginDate = Format(dtFrom, "yyyy-MM-dd" & " " & "00:00:00")
                strEndDate = Format(dtTo, "yyyy-MM-dd" & " " & "23:59:59")
                ViewState("startdate") = strBeginDate
                ViewState("enddate") = strEndDate
                strMonth = Format(dtFrom, "MM")
                strYear = Format(dtFrom, "yyyy")
                strDate = Format(dtTo, "dd-MM-yyyy")

                'Parameter
                strPrmMonth = Format(dtFrom, "MMM")
                strPrmYear = Format(dtFrom, "yyyy")

            ElseIf ViewState("ReportType") = "7" Or ViewState("ReportType") = "12" Or ViewState("ReportType") = "8" Then
                dtFrom = Me.txtSDate.Text
                dtTo = Me.txtEndDate.Text
                strBeginDate = Format(dtFrom, "yyyy-MM-dd" & " " & "00:00:00")
                strEndDate = Format(dtTo, "yyyy-MM-dd" & " " & "23:59:59")
                strOversea = Me.cmbOversea.SelectedValue

                'Parameter
                strPrmStart = Format(dtFrom, "dd-MM-yyyy")
                strPrmEnd = Format(dtTo, "dd-MM-yyyy")

            ElseIf ViewState("ReportType") = "10" Then
                dtDate = Me.txtDate.Text
                strDate = Format(dtDate, "yyyy-MM-dd" & " " & "23:59:59")

                If ViewState("type") = "DELTREND" Then
                    strDate2 = Format(dtDate.AddMonths(-6), "yyyy-MM-dd" & " " & "00:00:00")
                End If

                'Parameter
                strPrmDate = Format(dtDate, "dd-MM-yyyy")

            ElseIf ViewState("ReportType") = "11" Then
                strOversea = Me.cmbOversea.SelectedValue

            End If

            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Session("CompanyID"))) 'Company ID
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmStartDate", strBeginDate)) 'Start Date
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmEndDate", strEndDate)) 'End Date
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmGRNStartDate", strGRNBeginDate)) 'GRN Start Date
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmGRNEndDate", strGRNEndDate)) 'GRN End Date
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmDate", strDate)) 'Date
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmDate2", strDate2)) 'Date
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmOversea", strOversea)) 'Oversea
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmYear", strYear)) 'Year
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmMonth", strMonth)) 'Month
            da.SelectCommand.Parameters.Add(New MySqlParameter("@rownum", "0")) 'Month

            'Default Value to Parameter
            strPrmUserId = Session("UserName")
            strPrmCoyName = Session("CompanyName")
            If strOversea = "Y" Then
                strPrmOversea = "OVERSEA"
            Else
                strPrmOversea = "LOCAL"
            End If

            da.Fill(ds)

            Dim strDataset As String = ""
            If ViewState("type") = "TOP20VEN" Then
                strDataset = "TopVendor_DataSetTopVendor"
                'ElseIf ViewState("type") = "LOCALOVERSEAITEM" Then
                '    strDataset = "LocalOverseaItem_DataSetLocalOverseaItem"
            ElseIf ViewState("type") = "LOCALOVERSEAPUR" Then
                strDataset = "LocalOverseaPurchasing_DataSetLocalOverseaPurchasing"
            ElseIf ViewState("type") = "PRPOFORINV" Then
                strDataset = "PRPOForInventory_DataSetPRPOForInventory"
                'ElseIf ViewState("type") = "PRPOFORSP" Then
                '    strDataset = "PRPOForSpot_DataSetPRPOForSpot"
            ElseIf ViewState("type") = "POBAL" Then
                strDataset = "POBalance_DataSetPOBalance"
            ElseIf ViewState("type") = "DELTREND" Then
                strDataset = "DeliveryTrend_DataSetDeliveryTrend"
            ElseIf ViewState("type") = "GRNLIST" Then
                strDataset = "GRNListing_DataSetGRNListing"
            ElseIf ViewState("type") = "INDEXBOOK" Then
                strDataset = "IndexBook_DataSetIndexBook"
            ElseIf ViewState("type") = "LASTKEYINNUM" Then
                strDataset = "LastKeyInNumber_DataSetLastKeyInNumber"
            ElseIf ViewState("type") = "ISSUETREND" Then
                strDataset = "IssuingTrend_DataSetIssuingTrend"
            ElseIf ViewState("type") = "STKSTATUS" Then
                strDataset = "StockStatus_DataSetStockStatus"
            ElseIf ViewState("type") = "STOCKBAL" Then
                strDataset = "StockBalance_DataSetStockBalance"
            ElseIf ViewState("type") = "MRSAPPVD" Then
                strDataset = "MRSApprovedList_DataSetMRSApprovedList"
            ElseIf ViewState("type") = "MRSLIST" Then
                strDataset = "MRSList_DataSetMRSList"
            ElseIf ViewState("type") = "SUMMTHSTKBAL" Then
                strDataset = "SummaryMonthlyStockBalance_DataSetSumStockBalance"
            ElseIf ViewState("type") = "SMFA0612MTH" Then
                strDataset = "SlowMoving_DataSetSlowMoving"
            ElseIf ViewState("type") = "DEADSTK" Then
                strDataset = "DeadStock_DataSetDeadStock"
            ElseIf ViewState("type") = "MTHMGMT" Then
                strDataset = "MonthlyMGMT_DataSetMonthlyMGMT"
            ElseIf ViewState("type") = "PREALERT" Then
                strDataset = "PreAlertReport_DataSetPreAlert"
            ElseIf ViewState("type") = "MTHISSUE" Then
                strDataset = "MonthlyIssue_DataSetMonthlyIssue"
            ElseIf ViewState("type") = "ITEMMTRS" Then
                strDataset = "ItemMasterList_DataSetItemMaster"
            ElseIf ViewState("type") = "MTHCOSTTREND" Then
                strDataset = "MonthlyCostTrendReport_DataSetMonthlyCostTrend"
            ElseIf ViewState("type") = "MTHGRN" Then
                strDataset = "MonthlyGoodsReceiptNote_DataSetMonthlyGoodsReceipt"
            End If

            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource(strDataset, ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)

            If ViewState("type") = "TOP20VEN" Then
                localreport.ReportPath = dispatcher.direct("Report", "Top20Vendor.rdlc", "Report")
                'ElseIf ViewState("type") = "LOCALOVERSEAITEM" Then
                '    localreport.ReportPath = dispatcher.direct("Report", "LocalOverseaInvSpt.rdlc", "Report")
            ElseIf ViewState("type") = "LOCALOVERSEAPUR" Then
                localreport.ReportPath = dispatcher.direct("Report", "LocalOverseaPurchasing.rdlc", "Report")
            ElseIf ViewState("type") = "PRPOFORINV" Then
                localreport.ReportPath = dispatcher.direct("Report", "SummaryPRPOForInv.rdlc", "Report")
                'ElseIf ViewState("type") = "PRPOFORSP" Then
                '    localreport.ReportPath = dispatcher.direct("Report", "SummaryPRPOForSpt.rdlc", "Report")
            ElseIf ViewState("type") = "POBAL" Then
                localreport.ReportPath = dispatcher.direct("Report", "POBalanceList.rdlc", "Report")
            ElseIf ViewState("type") = "DELTREND" Then
                localreport.ReportPath = dispatcher.direct("Report", "DeliveryTrend.rdlc", "Report")
            ElseIf ViewState("type") = "GRNLIST" Then
                localreport.ReportPath = dispatcher.direct("Report", "GRNListing.rdlc", "Report")
            ElseIf ViewState("type") = "INDEXBOOK" Then
                localreport.ReportPath = dispatcher.direct("Report", "IndexBook.rdlc", "Report")
            ElseIf ViewState("type") = "LASTKEYINNUM" Then
                localreport.ReportPath = dispatcher.direct("Report", "LastKeyInNumberList.rdlc", "Report")
            ElseIf ViewState("type") = "ISSUETREND" Then
                localreport.ReportPath = dispatcher.direct("Report", "IssuingTrend.rdlc", "Report")
            ElseIf ViewState("type") = "STKSTATUS" Then
                localreport.ReportPath = dispatcher.direct("Report", "StockStatus.rdlc", "Report")
            ElseIf ViewState("type") = "STOCKBAL" Then
                localreport.ReportPath = dispatcher.direct("Report", "StockBalance.rdlc", "Report")
            ElseIf ViewState("type") = "MRSAPPVD" Then
                localreport.ReportPath = dispatcher.direct("Report", "MRSApprovedList.rdlc", "Report")
            ElseIf ViewState("type") = "MRSLIST" Then
                localreport.ReportPath = dispatcher.direct("Report", "MRSListing.rdlc", "Report")
            ElseIf ViewState("type") = "SUMMTHSTKBAL" Then
                localreport.ReportPath = dispatcher.direct("Report", "SummaryMonthlyStockBalance.rdlc", "Report")
            ElseIf ViewState("type") = "SMFA0612MTH" Then
                localreport.ReportPath = dispatcher.direct("Report", "SlowMoving.rdlc", "Report")
            ElseIf ViewState("type") = "DEADSTK" Then
                localreport.ReportPath = dispatcher.direct("Report", "DeadStock.rdlc", "Report")
            ElseIf ViewState("type") = "MTHMGMT" Then
                localreport.ReportPath = dispatcher.direct("Report", "MonthlyManagement.rdlc", "Report")
            ElseIf ViewState("type") = "PREALERT" Then
                localreport.ReportPath = dispatcher.direct("Report", "PreAlertReport.rdlc", "Report")
            ElseIf ViewState("type") = "MTHISSUE" Then
                localreport.ReportPath = dispatcher.direct("Report", "MonthlyIssue.rdlc", "Report")
            ElseIf ViewState("type") = "ITEMMTRS" Then
                localreport.ReportPath = dispatcher.direct("Report", "ItemMasterList.rdlc", "Report")
            ElseIf ViewState("type") = "MTHCOSTTREND" Then
                localreport.ReportPath = dispatcher.direct("Report", "MonthlyCostTrend.rdlc", "Report")
            ElseIf ViewState("type") = "MTHGRN" Then
                localreport.ReportPath = dispatcher.direct("Report", "MonthlyGoodsReceiptNoteReport.rdlc", "Report")
            End If
            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            Dim strID As String = ""

            strID = System.Configuration.ConfigurationManager.AppSettings.Get("Env") & "ReportCoyLogoPath"
            TotalParameter = localreport.GetParameters.Count
            Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
            'Dim paramlist As New Generic.List(Of ReportParameter)
            For I = 0 To localreport.GetParameters.Count - 1
                GetParameter = localreport.GetParameters.Item(I).Name
                Select Case LCase(GetParameter)
                    Case "pmrequestedby"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strPrmUserId)

                    Case "dtfrom"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strPrmStart)

                    Case "dtto"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strPrmEnd)

                    Case "dt"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strPrmDate)

                    Case "logo"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings(strID) & strPrmImgSrc)

                    Case "prmbuyercoyname"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strPrmCoyName)

                    Case "oversea"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strPrmOversea)

                    Case "prmyear"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strPrmYear)

                    Case "prmmonth"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strPrmMonth)

                    Case "month"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strMonth_1)

                    Case "year"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strYear)

                    Case "past1mth"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strPrmPast1Mth)

                    Case "past2mth"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strPrmPast2Mth)

                    Case "past3mth"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strPrmPast3Mth)

                    Case "past4mth"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strPrmPast4Mth)

                    Case "past5mth"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strPrmPast5Mth)

                    Case "past6mth"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strPrmPast6Mth)

                    Case Else
                End Select
            Next
            localreport.SetParameters(par)

            If ViewState("type") = "MTHISSUE" Then
                AddHandler localreport.SubreportProcessing, AddressOf SetSubReport
            End If

            localreport.Refresh()

            Dim deviceInfo As String = _
                "<DeviceInfo>" + _
                    "  <OutputFormat>EMF</OutputFormat>" + _
                    "</DeviceInfo>"
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            If ViewState("type") = "TOP20VEN" Then
                strFileName = "Top20VendorsReport.pdf"
                'ElseIf ViewState("type") = "LOCALOVERSEAITEM" Then
                '    strFileName = "LocalOverseaItemsReport.pdf"
            ElseIf ViewState("type") = "LOCALOVERSEAPUR" Then
                strFileName = "LocalOverseaPurchasingReport.pdf"
            ElseIf ViewState("type") = "PRPOFORINV" Then
                strFileName = "POSummaryReport(Inventory).pdf"
                'ElseIf ViewState("type") = "PRPOFORSP" Then
                '    strFileName = "POSummaryReport(Spot).pdf"
            ElseIf ViewState("type") = "POBAL" Then
                strFileName = "POBalanceReport.pdf"
            ElseIf ViewState("type") = "DELTREND" Then
                strFileName = "DeliveryTrendReport.pdf"
            ElseIf ViewState("type") = "GRNLIST" Then
                strFileName = "GRNListingReport.pdf"
            ElseIf ViewState("type") = "INDEXBOOK" Then
                strFileName = "IndexBook.pdf"
            ElseIf ViewState("type") = "LASTKEYINNUM" Then
                strFileName = "LastKeyInNumberListReport.pdf"
            ElseIf ViewState("type") = "ISSUETREND" Then
                strFileName = "IssuingTrendReport.pdf"
            ElseIf ViewState("type") = "STKSTATUS" Then
                strFileName = "StockStatusReport.pdf"
            ElseIf ViewState("type") = "STOCKBAL" Then
                strFileName = "StockBalance.pdf"
            ElseIf ViewState("type") = "MRSAPPVD" Then
                strFileName = "MRSApprovedListReport.pdf"
            ElseIf ViewState("type") = "MRSLIST" Then
                strFileName = "MRSListingReport.pdf"
            ElseIf ViewState("type") = "SUMMTHSTKBAL" Then
                strFileName = "SummaryMonthlyStockBalanceReport.pdf"
            ElseIf ViewState("type") = "SMFA0612MTH" Then
                strFileName = "SlowMovingReport.pdf"
            ElseIf ViewState("type") = "DEADSTK" Then
                strFileName = "DeadStok.pdf"
            ElseIf ViewState("type") = "MTHMGMT" Then
                strFileName = "MonthlyManagement.pdf"
            ElseIf ViewState("type") = "PREALERT" Then
                strFileName = "PreAlert.pdf"
            ElseIf ViewState("type") = "MTHISSUE" Then
                strFileName = "MonthlyIssue.pdf"
            ElseIf ViewState("type") = "ITEMMTRS" Then
                strFileName = "ItemMasterList.pdf"
            ElseIf ViewState("type") = "MTHCOSTTREND" Then
                strFileName = "MonthlyCostTrend.pdf"
            ElseIf ViewState("type") = "MTHGRN" Then
                strFileName = "MonthlyGoodsReceiptNote.pdf"

            End If

            'Return PDF\

            Me.Response.Clear()
            Me.Response.ContentType = "application/pdf"
            Me.Response.AddHeader("Content-disposition", "attachment; filename=" & strFileName)
            Me.Response.BinaryWrite(PDF)
            Me.Response.End()
            'Dim fs As New FileStream(appPath & "Report\" & strFileName, FileMode.Create)
            'fs.Write(PDF, 0, PDF.Length)
            'fs.Close()

            'Response.ContentType = "application/x-download"
            'Response.AddHeader("Content-Disposition", "attachment;filename=" & strFileName)
            'Response.WriteFile(Server.MapPath(strFileName))
            'Response.End()

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

    Private Sub SetSubReport(ByVal sender As Object, ByVal e As SubreportProcessingEventArgs)
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim strStartDate As String = ""
        Dim strEndDate As String = ""

        strStartDate = ViewState("startdate")
        strEndDate = ViewState("enddate")
        myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";"

        conn = New MySqlConnection(myConnectionString)

        cmd = New MySqlCommand
        With cmd
            .Connection = conn
            .CommandType = CommandType.Text
            .CommandText = "SELECT CS_SEC_INDEX, PM_ACCT_CODE, IM_ITEM_CODE, IM_INVENTORY_NAME, PM_SPEC1, PM_SPEC2, PM_SPEC3, " & _
                        "IRSM_IRS_APPROVED_DATE, IRSM_IRS_NO, IRSM_BUYER_ID, " & _
                        "IC_COST_QTY, IC_COST_UPRICE, IC_COST_COST, IRSM_BUYER_ID, " & _
                        "(SELECT IRD_IR_NO FROM INVENTORY_REQUISITION_DETAILS WHERE IRD_IR_SLIP_INDEX = IRSM_IRS_INDEX LIMIT 1) AS IRD_IR_NO, PM_UOM, PM_OVERSEA " & _
                        "FROM INVENTORY_REQUISITION_SLIP_MSTR " & _
                        "INNER JOIN INVENTORY_REQUISITION_SLIP_DETAILS ON IRSM_IRS_COY_ID = IRSD_IRS_COY_ID AND IRSM_IRS_NO = IRSD_IRS_NO " & _
                        "INNER JOIN INVENTORY_MSTR ON IRSD_INVENTORY_INDEX = IM_INVENTORY_INDEX " & _
                        "INNER JOIN COMPANY_SECTION ON IRSM_IRS_SECTION = CS_SEC_CODE AND IRSM_IRS_COY_ID = CS_COY_ID " & _
                        "INNER JOIN PRODUCT_MSTR ON IM_ITEM_CODE = PM_VENDOR_ITEM_CODE AND IM_COY_ID = PM_S_COY_ID " & _
                        "INNER JOIN INVENTORY_COST ON IRSD_IRS_COY_ID = IC_COY_ID AND IRSD_IRS_NO = IC_INVENTORY_REF_DOC AND IRSD_INVENTORY_INDEX = IC_INVENTORY_INDEX AND IC_INVENTORY_TYPE = 'II' AND IRSD_QTY = IC_COST_QTY " & _
                        "WHERE (IRSM_IRS_STATUS = '3' OR IRSM_IRS_STATUS = '4') AND IRSM_IRS_COY_ID = '" & Session("CompanyId") & "' " & _
                        "AND IRSM_IRS_APPROVED_DATE >= '" & strStartDate & "' AND IRSM_IRS_APPROVED_DATE <= '" & strEndDate & "' " & _
                        "AND CS_SEC_INDEX = @SectionIndex " & _
                        "ORDER BY PM_ACCT_CODE, IM_ITEM_CODE "

        End With

        da = New MySqlDataAdapter(cmd)
        If e.Parameters.Count > 0 Then
            da.SelectCommand.Parameters.Add(New MySqlParameter("@SectionIndex", e.Parameters(0).Values(0).ToString))
        End If
        da.Fill(ds)

        Dim subDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("MonthlyIssueSubReport_MonthlyIssueSubReport_DataSetMonthlyIssueSubReport", ds.Tables(0))
        e.DataSources.Add(subDataSource)
    End Sub

    Private Sub SetPOReportSpotSubReport(ByVal sender As Object, ByVal e As SubreportProcessingEventArgs)
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim strStartDate As String = ""
        Dim strEndDate As String = ""

        strStartDate = ViewState("startdate")
        strEndDate = ViewState("enddate")
        myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";"

        conn = New MySqlConnection(myConnectionString)

        cmd = New MySqlCommand
        With cmd
            .Connection = conn
            .CommandType = CommandType.Text
            .CommandText = "SELECT SUM(IFNULL(NO_SP_PR,0)) AS NO_SP_PR, SUM(IFNULL(NO_INV_PR,0)) AS NO_INV_PR, " & _
                                "SUM(NO_SP_PO) AS NO_SP_PO, SUM(NO_INV_PO) AS NO_INV_PO, SUM(PO_AMT_SP) AS PO_AMT_SP, SUM(PO_AMT_INV) AS PO_AMT_INV, " & _
                                "SUM(PM_LOCAL_SP_PO_AMT) AS PM_LOCAL_SP_PO_AMT, SUM(PM_LOCAL_INV_PO_AMT) AS PM_LOCAL_INV_PO_AMT, " & _
                                "SUM(PM_OVERSEA_SP_PO_AMT) AS PM_OVERSEA_SP_PO_AMT, SUM(PM_OVERSEA_INV_PO_AMT) AS PM_OVERSEA_INV_PO_AMT FROM " & _
                                "(SELECT POM_DATE, POM_PO_DATE, COUNT(NO_SP_PO) AS NO_SP_PO, COUNT(NO_INV_PO) AS NO_INV_PO, " & _
                                "SUM(PO_AMT_SP) AS PO_AMT_SP, SUM(PO_AMT_INV) AS PO_AMT_INV,  " & _
                                "SUM(PM_LOCAL_SP_PO_AMT) AS PM_LOCAL_SP_PO_AMT, SUM(PM_LOCAL_INV_PO_AMT) AS PM_LOCAL_INV_PO_AMT,  " & _
                                "SUM(PM_OVERSEA_SP_PO_AMT) AS PM_OVERSEA_SP_PO_AMT, SUM(PM_OVERSEA_INV_PO_AMT) AS PM_OVERSEA_INV_PO_AMT FROM " & _
                                "(SELECT DATE_FORMAT(POM_PO_DATE,'%b %y') AS POM_DATE, POM_PO_DATE, POM_PO_NO, " & _
                                "CASE WHEN POD_ITEM_TYPE = 'SP' THEN POM_PO_NO END AS NO_SP_PO, " & _
                                "CASE WHEN POD_ITEM_TYPE <> 'SP' THEN POM_PO_NO END AS NO_INV_PO, " & _
                                "CASE WHEN POD_ITEM_TYPE = 'SP' AND (POD_OVERSEA = 'Y' OR POD_OVERSEA = 'N') THEN (POM_PO_COST * IFNULL(POM_EXCHANGE_RATE,1)) ELSE 0 END AS PO_AMT_SP, " & _
                                "CASE WHEN POD_ITEM_TYPE <> 'SP' AND (POD_OVERSEA = 'Y' OR POD_OVERSEA = 'N') THEN (POM_PO_COST * IFNULL(POM_EXCHANGE_RATE,1)) ELSE 0 END AS PO_AMT_INV, " & _
                                "CASE WHEN (POD_ITEM_TYPE = 'SP' AND POD_OVERSEA = 'N') THEN POM_PO_COST * IFNULL(POM_EXCHANGE_RATE,1) ELSE 0 END AS PM_LOCAL_SP_PO_AMT, " & _
                                "CASE WHEN (POD_ITEM_TYPE <> 'SP' AND POD_OVERSEA = 'N') THEN POM_PO_COST * IFNULL(POM_EXCHANGE_RATE,1) ELSE 0 END AS PM_LOCAL_INV_PO_AMT, " & _
                                "CASE WHEN (POD_ITEM_TYPE = 'SP' AND POD_OVERSEA = 'Y') THEN POM_PO_COST * IFNULL(POM_EXCHANGE_RATE,1) ELSE 0 END AS PM_OVERSEA_SP_PO_AMT, " & _
                                "CASE WHEN (POD_ITEM_TYPE <> 'SP' AND POD_OVERSEA = 'Y') THEN POM_PO_COST * IFNULL(POM_EXCHANGE_RATE,1) ELSE 0 END AS PM_OVERSEA_INV_PO_AMT " & _
                                "FROM PO_MSTR POM, PO_DETAILS POD " & _
                                "WHERE(POD.POD_PO_NO = POM.POM_PO_NO And POD.POD_COY_ID = POM.POM_B_COY_ID) " & _
                                "AND POD.POD_COY_ID = '" & Session("CompanyId") & "' AND POM.POM_PO_DATE IS NOT NULL  " & _
                                "AND POM.POM_PO_DATE >= '" & strStartDate & "' AND POM.POM_PO_DATE <= '" & strEndDate & "' GROUP BY POM_PO_NO) AS TB_A " & _
                                "GROUP BY POM_DATE) AS TB_A " & _
                                "LEFT JOIN " & _
                                "(SELECT PRM_DATE, PRM_SUBMIT_DATE, COUNT(NO_SP_PR) AS NO_SP_PR, COUNT(NO_INV_PR) AS NO_INV_PR FROM  " & _
                                "(SELECT DATE_FORMAT(PRM_SUBMIT_DATE,'%b %y') AS PRM_DATE, PRM_SUBMIT_DATE,  " & _
                                "CASE WHEN PRD_ITEM_TYPE = 'SP' THEN PRM_PR_NO END AS NO_SP_PR, " & _
                                "CASE WHEN PRD_ITEM_TYPE <> 'SP' THEN PRM_PR_NO END AS NO_INV_PR, PRM_PR_NO  " & _
                                "FROM PR_MSTR PRM, PR_DETAILS PRD WHERE PRM_COY_ID = '" & Session("CompanyId") & "' AND  " & _
                                "PRM.PRM_COY_ID = PRD.PRD_COY_ID And PRM.PRM_PR_NO = PRD.PRD_PR_NO " & _
                                "AND PRM_SUBMIT_DATE IS NOT NULL  " & _
                                "AND PRM_SUBMIT_DATE >= '" & strStartDate & "' AND PRM_SUBMIT_DATE <= '" & strEndDate & "' " & _
                                "AND (PRM_PR_STATUS <> '6' AND PRM_PR_STATUS <> '8') GROUP BY PRM_PR_NO) AS TB_B  " & _
                                "GROUP BY PRM_DATE ORDER BY PRM_SUBMIT_DATE) AS TB_B " & _
                                "ON TB_A.POM_DATE = TB_B.PRM_DATE "

        End With

        da = New MySqlDataAdapter(cmd)
        'If e.Parameters.Count > 0 Then
        '    da.SelectCommand.Parameters.Add(New MySqlParameter("@SectionIndex", e.Parameters(0).Values(0).ToString))
        'End If
        da.Fill(ds)

        Dim subDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("LocalOverseaItem_DataSetLocalOverseaItem", ds.Tables(0))
        'Dim subDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("MonthlyIssueSubReport_MonthlyIssueSubReport_DataSetMonthlyIssueSubReport", ds.Tables(0))
        e.DataSources.Add(subDataSource)
    End Sub

    Private Sub ExportToMonthlyStockReport1Excel()
        Dim strSql, strOversea, strMonth, strYear, strFileName As String
        Dim objFile As New FileManagement
        Dim dtDate, dtFrom, dtTo As Date
        Dim ds, dsTemp, dsGRN, dsMRS, dsWO As New DataSet
        Dim i, j As Integer

        strOversea = Me.cmbOversea.SelectedValue
        strMonth = Me.cmbMonth.SelectedValue
        strYear = Me.cmbYear.SelectedValue

        'Parameter
        If Me.cmbMonth.SelectedIndex > 0 Then
            dtDate = New DateTime(Me.cmbYear.SelectedValue, Me.cmbMonth.SelectedValue, 1)
        End If

        strSql = "SELECT PM_ACCT_CODE, IM_ITEM_CODE, PM_PRODUCT_DESC, PM_SPEC1, PM_SPEC2, PM_SPEC3, PM_UOM, " & _
                "DATE_FORMAT(IC_COST_DATE, '%Y-%m-%d') AS IC_COST_DATE, GROUP_CONCAT(IC_INVENTORY_REF_DOC SEPARATOR ', ') AS IC_INVENTORY_REF_DOC, " & _
                "SUM(IC_COST_QTY) AS IC_COST_QTY, SUM(IC_COST_COST) AS IC_COST_COST, GROUP_CONCAT(POM_VENDOR_CODE SEPARATOR ', ') AS POM_VENDOR_CODE, " & _
                "(SELECT IC_COST_OPEN_QTY FROM INVENTORY_COST " & _
                "WHERE IC_COY_ID = '" & Session("CompanyId") & "' AND MONTH(IC_COST_DATE) = '" & strMonth & "' AND YEAR(IC_COST_DATE) = '" & strYear & "' " & _
                "AND IC_INVENTORY_INDEX = tb.IC_INVENTORY_INDEX LIMIT 1) AS OPEN_QTY, " & _
                "(SELECT IC_COST_OPEN_COST FROM INVENTORY_COST " & _
                "WHERE IC_COY_ID = '" & Session("CompanyId") & "' AND MONTH(IC_COST_DATE) = '" & strMonth & "' AND YEAR(IC_COST_DATE) = '" & strYear & "' " & _
                "AND IC_INVENTORY_INDEX = tb.IC_INVENTORY_INDEX LIMIT 1) AS OPEN_COST " & _
                "FROM " & _
                "(SELECT IC_COST_DATE, IC_INVENTORY_REF_DOC, IM_ITEM_CODE, SUM(IC_COST_QTY) AS IC_COST_QTY, " & _
                "SUM(IC_COST_COST) AS IC_COST_COST, IFNULL(POM_VENDOR_CODE,'') AS POM_VENDOR_CODE, " & _
                "PM_PRODUCT_DESC, PM_ACCT_CODE, PM_UOM, PM_SPEC1, PM_SPEC2, PM_SPEC3, IC_INVENTORY_INDEX " & _
                "FROM INVENTORY_COST " & _
                "INNER JOIN INVENTORY_MSTR ON IM_INVENTORY_INDEX = IC_INVENTORY_INDEX " & _
                "INNER JOIN PRODUCT_MSTR ON PM_VENDOR_ITEM_CODE = IM_ITEM_CODE AND PM_S_COY_ID = IM_COY_ID " & _
                "INNER JOIN GRN_MSTR ON GM_GRN_NO = IC_INVENTORY_REF_DOC AND GM_B_COY_ID = IC_COY_ID " & _
                "INNER JOIN PO_MSTR ON POM_PO_INDEX = GM_PO_INDEX " & _
                "WHERE IC_COY_ID = '" & Session("CompanyId") & "' AND IC_INVENTORY_TYPE = 'GRN' " & _
                "AND MONTH(IC_COST_DATE) = '" & strMonth & "' AND YEAR(IC_COST_DATE) = '" & strYear & "' " & _
                "AND PM_ITEM_TYPE = 'ST' AND PM_OVERSEA = '" & strOversea & "' " & _
                "GROUP BY IC_INVENTORY_REF_DOC, IM_ITEM_CODE) tb " & _
                "GROUP BY DATE_FORMAT(IC_COST_DATE, '%Y-%m-%d'), IM_ITEM_CODE "
        dsGRN = objDb.FillDs(strSql)

        'MRS
        strSql = "SELECT PM_ACCT_CODE, IM_ITEM_CODE, PM_PRODUCT_DESC, PM_SPEC1, PM_SPEC2, PM_SPEC3, PM_UOM, " & _
                "DATE_FORMAT(IC_COST_DATE, '%Y-%m-%d') AS IC_COST_DATE, GROUP_CONCAT(IC_INVENTORY_REF_DOC SEPARATOR ', ') AS IC_INVENTORY_REF_DOC, " & _
                "SUM(IC_COST_QTY) AS IC_COST_QTY, SUM(IC_COST_COST) AS IC_COST_COST, GROUP_CONCAT(IRSM_IRS_SECTION SEPARATOR ', ') AS IRSM_IRS_SECTION, " & _
                "(SELECT IC_COST_OPEN_QTY FROM INVENTORY_COST " & _
                "WHERE IC_COY_ID = '" & Session("CompanyId") & "' AND MONTH(IC_COST_DATE) = '" & strMonth & "' AND YEAR(IC_COST_DATE) = '" & strYear & "' " & _
                "AND IC_INVENTORY_INDEX = tb.IC_INVENTORY_INDEX LIMIT 1) AS OPEN_QTY, " & _
                "(SELECT IC_COST_OPEN_COST FROM INVENTORY_COST " & _
                "WHERE IC_COY_ID = '" & Session("CompanyId") & "' AND MONTH(IC_COST_DATE) = '" & strMonth & "' AND YEAR(IC_COST_DATE) = '" & strYear & "' " & _
                "AND IC_INVENTORY_INDEX = tb.IC_INVENTORY_INDEX LIMIT 1) AS OPEN_COST " & _
                "FROM " & _
                "(SELECT IC_COST_DATE, IC_INVENTORY_REF_DOC, IM_ITEM_CODE, SUM(IC_COST_QTY) AS IC_COST_QTY, " & _
                "SUM(IC_COST_COST) AS IC_COST_COST, IFNULL(IRSM_IRS_SECTION,'') AS IRSM_IRS_SECTION, " & _
                "PM_PRODUCT_DESC, PM_ACCT_CODE, PM_UOM, PM_SPEC1, PM_SPEC2, PM_SPEC3, IC_INVENTORY_INDEX " & _
                "FROM INVENTORY_COST " & _
                "INNER JOIN INVENTORY_MSTR ON IM_INVENTORY_INDEX = IC_INVENTORY_INDEX " & _
                "INNER JOIN INVENTORY_REQUISITION_SLIP_MSTR ON IRSM_IRS_COY_ID = IC_COY_ID AND IRSM_IRS_NO = IC_INVENTORY_REF_DOC " & _
                "INNER JOIN PRODUCT_MSTR ON PM_VENDOR_ITEM_CODE = IM_ITEM_CODE AND PM_S_COY_ID = IM_COY_ID " & _
                "WHERE IC_COY_ID = '" & Session("CompanyId") & "' AND IC_INVENTORY_TYPE = 'II' " & _
                "AND MONTH(IC_COST_DATE) = '" & strMonth & "' AND YEAR(IC_COST_DATE) = '" & strYear & "' " & _
                "AND PM_ITEM_TYPE = 'ST' AND PM_OVERSEA = '" & strOversea & "' " & _
                "GROUP BY IC_INVENTORY_REF_DOC, IM_ITEM_CODE) tb " & _
                "GROUP BY DATE_FORMAT(IC_COST_DATE, '%Y-%m-%d'), IM_ITEM_CODE"
        dsMRS = objDb.FillDs(strSql)

        'WO
        strSql = "SELECT PM_ACCT_CODE, IM_ITEM_CODE, PM_PRODUCT_DESC, PM_SPEC1, PM_SPEC2, PM_SPEC3, PM_UOM, " & _
                "DATE_FORMAT(IC_COST_DATE, '%Y-%m-%d') AS IC_COST_DATE, GROUP_CONCAT(IC_INVENTORY_REF_DOC SEPARATOR ', ') AS IC_INVENTORY_REF_DOC, SUM(IC_COST_QTY) AS IC_COST_QTY, SUM(IC_COST_COST) AS IC_COST_COST, " & _
                "(SELECT IC_COST_OPEN_QTY FROM INVENTORY_COST " & _
                "WHERE IC_COY_ID = '" & Session("CompanyId") & "' AND MONTH(IC_COST_DATE) = '" & strMonth & "' AND YEAR(IC_COST_DATE) = '" & strYear & "' " & _
                "AND IC_INVENTORY_INDEX = tb.IC_INVENTORY_INDEX LIMIT 1) AS OPEN_QTY, " & _
                "(SELECT IC_COST_OPEN_COST FROM INVENTORY_COST " & _
                "WHERE IC_COY_ID = '" & Session("CompanyId") & "' AND MONTH(IC_COST_DATE) = '" & strMonth & "' AND YEAR(IC_COST_DATE) = '" & strYear & "' " & _
                "AND IC_INVENTORY_INDEX = tb.IC_INVENTORY_INDEX LIMIT 1) AS OPEN_COST " & _
                "FROM " & _
                "(SELECT IC_COST_DATE, IC_INVENTORY_REF_DOC, IM_ITEM_CODE, SUM(IC_COST_QTY) AS IC_COST_QTY, SUM(IC_COST_COST) AS IC_COST_COST, " & _
                "PM_PRODUCT_DESC, PM_ACCT_CODE, PM_UOM, PM_SPEC1, PM_SPEC2, PM_SPEC3, IC_INVENTORY_INDEX " & _
                "FROM INVENTORY_COST " & _
                "INNER JOIN INVENTORY_MSTR ON IM_INVENTORY_INDEX = IC_INVENTORY_INDEX " & _
                "INNER JOIN PRODUCT_MSTR ON PM_VENDOR_ITEM_CODE = IM_ITEM_CODE AND PM_S_COY_ID = IM_COY_ID " & _
                "WHERE IC_COY_ID = '" & Session("CompanyId") & "' AND IC_INVENTORY_TYPE = 'WO' " & _
                "AND MONTH(IC_COST_DATE) = '" & strMonth & "' AND YEAR(IC_COST_DATE) = '" & strYear & "' " & _
                "AND PM_ITEM_TYPE = 'ST' AND PM_OVERSEA = '" & strOversea & "' " & _
                "GROUP BY IC_INVENTORY_REF_DOC, IM_ITEM_CODE) tb " & _
                "GROUP BY DATE_FORMAT(IC_COST_DATE, '%Y-%m-%d'), IM_ITEM_CODE"
        dsWO = objDb.FillDs(strSql)

        Dim dt As New DataTable
        Dim dtr As DataRow
        dt.Columns.Add("Account_Code", Type.GetType("System.String"))
        dt.Columns.Add("Item_Code", Type.GetType("System.String"))
        dt.Columns.Add("Item_Name", Type.GetType("System.String"))
        dt.Columns.Add("Spec_1", Type.GetType("System.String"))
        dt.Columns.Add("Spec_2", Type.GetType("System.String"))
        dt.Columns.Add("Spec_3", Type.GetType("System.String"))
        dt.Columns.Add("Uom", Type.GetType("System.String"))
        'dt.Columns.Add("OPEN_QTY", Type.GetType("System.String"))
        'dt.Columns.Add("OPEN_COST", Type.GetType("System.String"))
        dt.Columns.Add("Date", Type.GetType("System.String"))
        dt.Columns.Add("GRN_No", Type.GetType("System.String"))
        dt.Columns.Add("Issue_No", Type.GetType("System.String"))
        dt.Columns.Add("Write_Off_No", Type.GetType("System.String"))
        dt.Columns.Add("Section_Code", Type.GetType("System.String"))
        dt.Columns.Add("Supplier_Code", Type.GetType("System.String"))
        dt.Columns.Add("Stock_In_Qty", Type.GetType("System.String"))
        dt.Columns.Add("Stock_In_Amount", Type.GetType("System.String"))
        dt.Columns.Add("Issue_Qty", Type.GetType("System.String"))
        dt.Columns.Add("Issue_Amount", Type.GetType("System.String"))
        dt.Columns.Add("Write_Off_Qty", Type.GetType("System.String"))
        dt.Columns.Add("Write_Off_Amount", Type.GetType("System.String"))

        For i = 0 To dsGRN.Tables(0).Rows.Count - 1
            dtr = dt.NewRow
            dtr("Account_Code") = dsGRN.Tables(0).Rows(i)("PM_ACCT_CODE")
            dtr("Item_Code") = dsGRN.Tables(0).Rows(i)("IM_ITEM_CODE")
            dtr("Item_Name") = dsGRN.Tables(0).Rows(i)("PM_PRODUCT_DESC")
            dtr("Spec_1") = dsGRN.Tables(0).Rows(i)("PM_SPEC1")
            dtr("Spec_2") = dsGRN.Tables(0).Rows(i)("PM_SPEC2")
            dtr("Spec_3") = dsGRN.Tables(0).Rows(i)("PM_SPEC3")
            dtr("Uom") = dsGRN.Tables(0).Rows(i)("PM_UOM")
            'dtr("OPEN_QTY") = dsGRN.Tables(0).Rows(i)("OPEN_QTY")
            'dtr("OPEN_COST") = dsGRN.Tables(0).Rows(i)("OPEN_COST")
            dtr("Date") = dsGRN.Tables(0).Rows(i)("IC_COST_DATE")
            dtr("GRN_No") = dsGRN.Tables(0).Rows(i)("IC_INVENTORY_REF_DOC")
            dtr("Supplier_Code") = dsGRN.Tables(0).Rows(i)("POM_VENDOR_CODE")
            dtr("Stock_In_Qty") = dsGRN.Tables(0).Rows(i)("IC_COST_QTY")
            dtr("Stock_In_Amount") = dsGRN.Tables(0).Rows(i)("IC_COST_COST")
            dt.Rows.Add(dtr)
        Next

        For i = 0 To dsMRS.Tables(0).Rows.Count - 1
            If dt.Select("Item_Code='" & dsMRS.Tables(0).Rows(i)("IM_ITEM_CODE") & "' AND Date = '" & dsMRS.Tables(0).Rows(i)("IC_COST_DATE") & "' AND Issue_No IS NULL").Length > 0 Then
                For j = 0 To dt.Rows.Count - 1
                    If dt.Rows(j)("Item_Code") = dsMRS.Tables(0).Rows(i)("IM_ITEM_CODE") And dt.Rows(j)("Date") = dsMRS.Tables(0).Rows(i)("IC_COST_DATE") And Common.parseNull(dt.Rows(j)("Issue_No")) = "" Then
                        dt.Rows(j)("Issue_No") = dsMRS.Tables(0).Rows(i)("IC_INVENTORY_REF_DOC")
                        dt.Rows(j)("Section_Code") = dsMRS.Tables(0).Rows(i)("IRSM_IRS_SECTION")
                        dt.Rows(j)("Issue_Qty") = dsMRS.Tables(0).Rows(i)("IC_COST_QTY")
                        dt.Rows(j)("Issue_Amount") = dsMRS.Tables(0).Rows(i)("IC_COST_COST")
                        dt.AcceptChanges()
                        Exit For
                    End If
                Next
            Else
                dtr = dt.NewRow
                dtr("Account_Code") = dsMRS.Tables(0).Rows(i)("PM_ACCT_CODE")
                dtr("Item_Code") = dsMRS.Tables(0).Rows(i)("IM_ITEM_CODE")
                dtr("Item_Name") = dsMRS.Tables(0).Rows(i)("PM_PRODUCT_DESC")
                dtr("Spec_1") = dsMRS.Tables(0).Rows(i)("PM_SPEC1")
                dtr("Spec_2") = dsMRS.Tables(0).Rows(i)("PM_SPEC2")
                dtr("Spec_3") = dsMRS.Tables(0).Rows(i)("PM_SPEC3")
                dtr("Uom") = dsMRS.Tables(0).Rows(i)("PM_UOM")
                dtr("Date") = dsMRS.Tables(0).Rows(i)("IC_COST_DATE")
                dtr("Issue_No") = dsMRS.Tables(0).Rows(i)("IC_INVENTORY_REF_DOC")
                dtr("Section_Code") = dsMRS.Tables(0).Rows(i)("IRSM_IRS_SECTION")
                dtr("Issue_Qty") = dsMRS.Tables(0).Rows(i)("IC_COST_QTY")
                dtr("Issue_Amount") = dsMRS.Tables(0).Rows(i)("IC_COST_COST")
                dt.Rows.Add(dtr)
            End If
        Next

        For i = 0 To dsWO.Tables(0).Rows.Count - 1
            If dt.Select("Item_Code='" & dsWO.Tables(0).Rows(i)("IM_ITEM_CODE") & "' AND Date = '" & dsWO.Tables(0).Rows(i)("IC_COST_DATE") & "' AND Write_Off_No IS NULL").Length > 0 Then
                For j = 0 To dt.Rows.Count - 1
                    If dt.Rows(j)("Item_Code") = dsWO.Tables(0).Rows(i)("IM_ITEM_CODE") And dt.Rows(j)("Date") = dsWO.Tables(0).Rows(i)("IC_COST_DATE") And Common.parseNull(dt.Rows(j)("Write_Off_No")) = "" Then
                        dt.Rows(j)("Write_Off_No") = dsWO.Tables(0).Rows(i)("IC_INVENTORY_REF_DOC")
                        dt.Rows(j)("Write_Off_Qty") = dsWO.Tables(0).Rows(i)("IC_COST_QTY")
                        dt.Rows(j)("Write_Off_Amount") = dsWO.Tables(0).Rows(i)("IC_COST_COST")
                        dt.AcceptChanges()
                        Exit For
                    End If
                Next
            Else
                dtr = dt.NewRow
                dtr("Account_Code") = dsWO.Tables(0).Rows(i)("PM_ACCT_CODE")
                dtr("Item_Code") = dsWO.Tables(0).Rows(i)("IM_ITEM_CODE")
                dtr("Item_Name") = dsWO.Tables(0).Rows(i)("PM_PRODUCT_DESC")
                dtr("Spec_1") = dsWO.Tables(0).Rows(i)("PM_SPEC1")
                dtr("Spec_2") = dsWO.Tables(0).Rows(i)("PM_SPEC2")
                dtr("Spec_3") = dsWO.Tables(0).Rows(i)("PM_SPEC3")
                dtr("Uom") = dsWO.Tables(0).Rows(i)("PM_UOM")
                dtr("Date") = dsWO.Tables(0).Rows(i)("IC_COST_DATE")
                dtr("Write_Off_No") = dsWO.Tables(0).Rows(i)("IC_INVENTORY_REF_DOC")
                dtr("Write_Off_Qty") = dsWO.Tables(0).Rows(i)("IC_COST_QTY")
                dtr("Write_Off_Amount") = dsWO.Tables(0).Rows(i)("IC_COST_COST")
                dt.Rows.Add(dtr)
            End If
        Next

        Dim dtrt() As DataRow
        Dim drTemp As DataRow
        dsTemp.Tables.Add(dt)
        dtrt = dsTemp.Tables(0).Select("", "Account_Code ASC, Item_Code ASC, Date ASC")
        ds = dsTemp.Clone
        For Each drTemp In dtrt
            ds.Tables(0).ImportRow(drTemp)
        Next

        strFileName = "MonthlyStockReport1"
        strFileName = strFileName & "(" & Format(dtDate, "MMMyyyy") & ").xls"
        Dim attachment As String = "attachment;filename=" & strFileName
        Response.ClearContent()
        Response.AddHeader("Content-Disposition", attachment)
        Response.ContentType = "application/vnd.ms-excel"

        Dim dc As DataColumn

        i = 0
        For Each dc In ds.Tables(0).Columns
            If i > 0 Then
                Response.Write(vbTab + Replace(dc.ColumnName, "_", " "))
            Else
                Response.Write(Replace(dc.ColumnName, "_", " "))
            End If
            i += 1

        Next
        Response.Write(vbCrLf)

        Dim dr As DataRow
        For Each dr In ds.Tables(0).Rows
            For i = 0 To ds.Tables(0).Columns.Count - 1
                If i > 0 Then
                    Response.Write(vbTab + dr.Item(i).ToString)
                Else
                    Response.Write(dr.Item(i).ToString)
                End If
            Next
            Response.Write(vbCrLf)
        Next
        Response.End()

    End Sub

    Private Sub ExportToMonthlyStockReport2Excel()
        Dim strSql, strOversea, strMonth, strYear, strFileName As String
        Dim objFile As New FileManagement
        Dim dtDate, dtFrom, dtTo As Date
        Dim ds, dsTemp, dsGRN, dsMRS, dsWO As New DataSet
        Dim i, j As Integer

        strOversea = Me.cmbOversea.SelectedValue
        strMonth = Me.cmbMonth.SelectedValue
        strYear = Me.cmbYear.SelectedValue

        'Parameter
        If Me.cmbMonth.SelectedIndex > 0 Then
            dtDate = New DateTime(Me.cmbYear.SelectedValue, Me.cmbMonth.SelectedValue, 1)
        End If

        'GRN
        strSql = "SELECT PM_ACCT_CODE, POD_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_SPEC1, PM_SPEC2, PM_SPEC3, PM_UOM, " & _
                "DATE_FORMAT(GM_DATE_RECEIVED, '%Y-%m-%d') AS DOC_DATE, GROUP_CONCAT(GD_GRN_NO SEPARATOR ', ') AS GRN_NO, " & _
                "SUM(GRN_QTY) AS GRN_QTY, GROUP_CONCAT(DOM_DO_NO SEPARATOR ', ') AS DO_NO, GROUP_CONCAT(POM_VENDOR_CODE SEPARATOR ', ') AS VENDOR_CODE " & _
                "FROM " & _
                "(SELECT GM_DATE_RECEIVED, GD_GRN_NO, POD_VENDOR_ITEM_CODE, " & _
                "SUM(GD_RECEIVED_QTY - GD_REJECTED_QTY) AS GRN_QTY, DOM_DO_NO, IFNULL(POM_VENDOR_CODE,'') AS POM_VENDOR_CODE, " & _
                "PM_PRODUCT_DESC, PM_ACCT_CODE, PM_UOM, PM_SPEC1, PM_SPEC2, PM_SPEC3 " & _
                "FROM GRN_MSTR " & _
                "INNER JOIN GRN_DETAILS ON GD_GRN_NO = GM_GRN_NO AND GD_B_COY_ID = GM_B_COY_ID " & _
                "INNER JOIN DO_MSTR ON DOM_DO_INDEX = GM_DO_INDEX " & _
                "INNER JOIN PO_MSTR ON POM_PO_INDEX = GM_PO_INDEX " & _
                "INNER JOIN PO_DETAILS ON POD_COY_ID = POM_B_COY_ID AND POD_PO_NO = POM_PO_NO AND POD_PO_LINE = GD_PO_LINE " & _
                "INNER JOIN PRODUCT_MSTR ON PM_S_COY_ID = POM_B_COY_ID AND PM_VENDOR_ITEM_CODE = POD_VENDOR_ITEM_CODE " & _
                "WHERE GM_B_COY_ID = '" & Session("CompanyId") & "' AND MONTH(GM_DATE_RECEIVED) = '" & strMonth & "' AND YEAR(GM_DATE_RECEIVED) = '" & strYear & "' " & _
                "AND PM_ITEM_TYPE = 'ST' AND PM_OVERSEA = '" & strOversea & "' " & _
                "GROUP BY GD_GRN_NO, POD_VENDOR_ITEM_CODE) tb " & _
                "GROUP BY DATE_FORMAT(GM_DATE_RECEIVED, '%Y-%m-%d'), POD_VENDOR_ITEM_CODE "
        dsGRN = objDb.FillDs(strSql)

        'MRS
        strSql = "SELECT PM_ACCT_CODE, IM_ITEM_CODE, PM_PRODUCT_DESC, PM_SPEC1, PM_SPEC2, PM_SPEC3, PM_UOM, " & _
                "DATE_FORMAT(IRSM_IRS_APPROVED_DATE, '%Y-%m-%d') AS DOC_DATE, GROUP_CONCAT(IRSD_IRS_NO SEPARATOR ', ') AS MRS_NO, " & _
                "SUM(MRS_QTY) AS MRS_QTY, GROUP_CONCAT(IRSM_IRS_SECTION SEPARATOR ', ') AS SEC_CODE, GROUP_CONCAT(IRD_IR_NO SEPARATOR ', ') AS IR_NO " & _
                "FROM " & _
                "(SELECT IRSM_IRS_APPROVED_DATE, IRSD_IRS_NO, IM_ITEM_CODE, " & _
                "SUM(IRSD_QTY) AS MRS_QTY, IRSM_IRS_SECTION, IRD_IR_NO, " & _
                "PM_PRODUCT_DESC, PM_ACCT_CODE, PM_UOM, PM_SPEC1, PM_SPEC2, PM_SPEC3 " & _
                "FROM INVENTORY_REQUISITION_SLIP_MSTR " & _
                "INNER JOIN INVENTORY_REQUISITION_SLIP_DETAILS ON IRSD_IRS_COY_ID = IRSM_IRS_COY_ID AND IRSD_IRS_NO = IRSM_IRS_NO " & _
                "INNER JOIN INVENTORY_MSTR ON IM_INVENTORY_INDEX = IRSD_INVENTORY_INDEX " & _
                "INNER JOIN INVENTORY_REQUISITION_DETAILS ON IRD_IR_SLIP_INDEX = IRSM_IRS_INDEX AND IRD_IR_LINE = IRSD_IRS_LINE " & _
                "INNER JOIN PRODUCT_MSTR ON PM_VENDOR_ITEM_CODE = IM_ITEM_CODE AND PM_S_COY_ID = IM_COY_ID " & _
                "WHERE IRSM_IRS_COY_ID = '" & Session("CompanyId") & "' AND MONTH(IRSM_IRS_DATE) = '" & strMonth & "' AND YEAR(IRSM_IRS_DATE) = '" & strYear & "' AND IRSM_IRS_APPROVED_DATE IS NOT NULL " & _
                "AND PM_ITEM_TYPE = 'ST' AND PM_OVERSEA = '" & strOversea & "' " & _
                "GROUP BY IRSD_IRS_NO, IM_ITEM_CODE) tb " & _
                "GROUP BY DATE_FORMAT(IRSM_IRS_APPROVED_DATE, '%Y-%m-%d'), IM_ITEM_CODE "
        dsMRS = objDb.FillDs(strSql)

        'WO
        strSql = "SELECT DATE_FORMAT(IWOM_WO_DATE, '%Y-%m-%d') AS DOC_DATE, IM_ITEM_CODE, GROUP_CONCAT(IWOD_WO_NO SEPARATOR ', ') AS WO_NO, " & _
                "SUM(WO_QTY) AS WO_QTY, PM_PRODUCT_DESC, PM_ACCT_CODE, PM_UOM, PM_SPEC1, PM_SPEC2, PM_SPEC3 " & _
                "FROM " & _
                "(SELECT IWOM_WO_DATE, IWOD_WO_NO, IM_ITEM_CODE, SUM(IWOD_QTY_VAL) AS WO_QTY, " & _
                "PM_PRODUCT_DESC, PM_ACCT_CODE, PM_UOM, PM_SPEC1, PM_SPEC2, PM_SPEC3 " & _
                "FROM INVENTORY_WRITE_OFF_MSTR " & _
                "INNER JOIN INVENTORY_WRITE_OFF_DETAILS ON IWOD_WO_NO = IWOM_WO_NO AND IWOD_WO_COY_ID = IWOM_WO_COY_ID " & _
                "INNER JOIN INVENTORY_MSTR ON IM_INVENTORY_INDEX = IWOD_INVENTORY_INDEX " & _
                "INNER JOIN PRODUCT_MSTR ON PM_VENDOR_ITEM_CODE = IM_ITEM_CODE AND PM_S_COY_ID = IM_COY_ID " & _
                "WHERE IWOM_WO_COY_ID = '" & Session("CompanyId") & "' AND MONTH(IWOM_WO_DATE) = '" & strMonth & "' AND YEAR(IWOM_WO_DATE) = '" & strYear & "' " & _
                "AND PM_ITEM_TYPE = 'ST' AND PM_OVERSEA = '" & strOversea & "' " & _
                "GROUP BY IWOD_WO_NO, IM_ITEM_CODE) tb " & _
                "GROUP BY DATE_FORMAT(IWOM_WO_DATE, '%Y-%m-%d'), IM_ITEM_CODE "
        dsWO = objDb.FillDs(strSql)

        Dim dt As New DataTable
        Dim dtr As DataRow
        dt.Columns.Add("Account_Code", Type.GetType("System.String"))
        dt.Columns.Add("Item_Code", Type.GetType("System.String"))
        dt.Columns.Add("Item_Name", Type.GetType("System.String"))
        dt.Columns.Add("Spec_1", Type.GetType("System.String"))
        dt.Columns.Add("Spec_2", Type.GetType("System.String"))
        dt.Columns.Add("Spec_3", Type.GetType("System.String"))
        dt.Columns.Add("Uom", Type.GetType("System.String"))
        dt.Columns.Add("Date", Type.GetType("System.String"))
        dt.Columns.Add("GRN_No", Type.GetType("System.String"))
        dt.Columns.Add("Issue_No", Type.GetType("System.String"))
        dt.Columns.Add("Write_Off_No", Type.GetType("System.String"))
        dt.Columns.Add("Section_Code", Type.GetType("System.String"))
        dt.Columns.Add("Supplier_Code", Type.GetType("System.String"))
        dt.Columns.Add("IR_No", Type.GetType("System.String"))
        dt.Columns.Add("DO_No", Type.GetType("System.String"))
        dt.Columns.Add("Stock_In_Qty", Type.GetType("System.String"))
        dt.Columns.Add("Issue_Qty", Type.GetType("System.String"))
        dt.Columns.Add("Write_Off_Qty", Type.GetType("System.String"))

        For i = 0 To dsGRN.Tables(0).Rows.Count - 1
            dtr = dt.NewRow
            dtr("Account_Code") = dsGRN.Tables(0).Rows(i)("PM_ACCT_CODE")
            dtr("Item_Code") = dsGRN.Tables(0).Rows(i)("POD_VENDOR_ITEM_CODE")
            dtr("Item_Name") = dsGRN.Tables(0).Rows(i)("PM_PRODUCT_DESC")
            dtr("Spec_1") = dsGRN.Tables(0).Rows(i)("PM_SPEC1")
            dtr("Spec_2") = dsGRN.Tables(0).Rows(i)("PM_SPEC2")
            dtr("Spec_3") = dsGRN.Tables(0).Rows(i)("PM_SPEC3")
            dtr("Uom") = dsGRN.Tables(0).Rows(i)("PM_UOM")
            dtr("Date") = dsGRN.Tables(0).Rows(i)("DOC_DATE")
            dtr("GRN_No") = dsGRN.Tables(0).Rows(i)("GRN_NO")
            dtr("Supplier_Code") = dsGRN.Tables(0).Rows(i)("VENDOR_CODE")
            dtr("DO_No") = dsGRN.Tables(0).Rows(i)("DO_NO")
            dtr("Stock_In_Qty") = dsGRN.Tables(0).Rows(i)("GRN_QTY")
            dt.Rows.Add(dtr)
        Next

        For i = 0 To dsMRS.Tables(0).Rows.Count - 1
            If dt.Select("ITEM_CODE='" & dsMRS.Tables(0).Rows(i)("IM_ITEM_CODE") & "' AND Date = '" & dsMRS.Tables(0).Rows(i)("DOC_DATE") & "' AND Issue_No IS NULL").Length > 0 Then
                For j = 0 To dt.Rows.Count - 1
                    If dt.Rows(j)("Item_Code") = dsMRS.Tables(0).Rows(i)("IM_ITEM_CODE") And dt.Rows(j)("Date") = dsMRS.Tables(0).Rows(i)("DOC_DATE") And Common.parseNull(dt.Rows(j)("Issue_No")) = "" Then
                        dt.Rows(j)("Issue_No") = dsMRS.Tables(0).Rows(i)("MRS_NO")
                        dt.Rows(j)("IR_No") = dsMRS.Tables(0).Rows(i)("IR_NO")
                        dt.Rows(j)("Section_Code") = dsMRS.Tables(0).Rows(i)("SEC_CODE")
                        dt.Rows(j)("Issue_Qty") = dsMRS.Tables(0).Rows(i)("MRS_QTY")
                        dt.AcceptChanges()
                        Exit For
                    End If
                Next
            Else
                dtr = dt.NewRow
                dtr("Account_Code") = dsMRS.Tables(0).Rows(i)("PM_ACCT_CODE")
                dtr("Item_Code") = dsMRS.Tables(0).Rows(i)("IM_ITEM_CODE")
                dtr("Item_Name") = dsMRS.Tables(0).Rows(i)("PM_PRODUCT_DESC")
                dtr("Spec_1") = dsMRS.Tables(0).Rows(i)("PM_SPEC1")
                dtr("Spec_2") = dsMRS.Tables(0).Rows(i)("PM_SPEC2")
                dtr("Spec_3") = dsMRS.Tables(0).Rows(i)("PM_SPEC3")
                dtr("Uom") = dsMRS.Tables(0).Rows(i)("PM_UOM")
                dtr("Date") = dsMRS.Tables(0).Rows(i)("DOC_DATE")
                dtr("Issue_No") = dsMRS.Tables(0).Rows(i)("MRS_NO")
                dtr("IR_No") = dsMRS.Tables(0).Rows(i)("IR_NO")
                dtr("Section_Code") = dsMRS.Tables(0).Rows(i)("SEC_CODE")
                dtr("Issue_Qty") = dsMRS.Tables(0).Rows(i)("MRS_QTY")
                dt.Rows.Add(dtr)
            End If
        Next

        For i = 0 To dsWO.Tables(0).Rows.Count - 1
            If dt.Select("Item_Code='" & dsWO.Tables(0).Rows(i)("IM_ITEM_CODE") & "' AND Date = '" & dsWO.Tables(0).Rows(i)("DOC_DATE") & "' AND Write_Off_No IS NULL").Length > 0 Then
                For j = 0 To dt.Rows.Count - 1
                    If dt.Rows(j)("Item_Code") = dsWO.Tables(0).Rows(i)("IM_ITEM_CODE") And dt.Rows(j)("Date") = dsWO.Tables(0).Rows(i)("DOC_DATE") And Common.parseNull(dt.Rows(j)("Write_Off_No")) = "" Then
                        dt.Rows(j)("Write_Off_No") = dsWO.Tables(0).Rows(i)("WO_NO")
                        dt.Rows(j)("Write_Off_Qty") = dsWO.Tables(0).Rows(i)("WO_QTY")
                        dt.AcceptChanges()
                        Exit For
                    End If
                Next
            Else
                dtr = dt.NewRow
                dtr("Account_Code") = dsWO.Tables(0).Rows(i)("PM_ACCT_CODE")
                dtr("Item_Code") = dsWO.Tables(0).Rows(i)("IM_ITEM_CODE")
                dtr("Item_Name") = dsWO.Tables(0).Rows(i)("PM_PRODUCT_DESC")
                dtr("Spec_1") = dsWO.Tables(0).Rows(i)("PM_SPEC1")
                dtr("Spec_2") = dsWO.Tables(0).Rows(i)("PM_SPEC2")
                dtr("Spec_3") = dsWO.Tables(0).Rows(i)("PM_SPEC3")
                dtr("Uom") = dsWO.Tables(0).Rows(i)("PM_UOM")
                dtr("Date") = dsWO.Tables(0).Rows(i)("DOC_DATE")
                dtr("Write_Off_No") = dsWO.Tables(0).Rows(i)("WO_NO")
                dtr("Write_Off_Qty") = dsWO.Tables(0).Rows(i)("WO_QTY")
                dt.Rows.Add(dtr)
            End If
        Next

        Dim dtrt() As DataRow
        Dim drTemp As DataRow
        dsTemp.Tables.Add(dt)
        dtrt = dsTemp.Tables(0).Select("", "Account_Code ASC, Item_Code ASC, Date ASC")
        ds = dsTemp.Clone
        For Each drTemp In dtrt
            ds.Tables(0).ImportRow(drTemp)
        Next

        strFileName = "MonthlyStockReport2"
        strFileName = strFileName & "(" & Format(dtDate, "MMMyyyy") & ").xls"
        Dim attachment As String = "attachment;filename=" & strFileName
        Response.ClearContent()
        Response.AddHeader("Content-Disposition", attachment)
        Response.ContentType = "application/vnd.ms-excel"

        Dim dc As DataColumn

        i = 0
        For Each dc In ds.Tables(0).Columns
            If i > 0 Then
                Response.Write(vbTab + Replace(dc.ColumnName, "_", " "))
            Else
                Response.Write(Replace(dc.ColumnName, "_", " "))
            End If
            i += 1

        Next
        Response.Write(vbCrLf)

        Dim dr As DataRow
        For Each dr In ds.Tables(0).Rows
            For i = 0 To ds.Tables(0).Columns.Count - 1
                If i > 0 Then
                    Response.Write(vbTab + dr.Item(i).ToString)
                Else
                    Response.Write(dr.Item(i).ToString)
                End If
            Next
            Response.Write(vbCrLf)
        Next
        Response.End()

    End Sub

    Private Sub ExportToPOReportSpotExcel()
        Dim strSql, strBeginDate, strEndDate, strFileName As String
        Dim objFile As New FileManagement
        Dim dtDate, dtFrom, dtTo As Date
        Dim ds1, ds2 As New DataSet
        Dim i, j As Integer

        dtFrom = Me.txtSDate.Text
        dtTo = Me.txtEndDate.Text
        strBeginDate = Format(dtFrom, "yyyy-MM-dd" & " " & "00:00:00")
        strEndDate = Format(dtTo, "yyyy-MM-dd" & " " & "23:59:59")

        'PO Report For Spot 
        strSql = "SELECT CONCAT('''', ACCT_CODE) AS 'Section Code', CASE WHEN POD_OVERSEA = 'N' THEN 'No' ELSE 'Yes' END AS 'Oversea', " & _
                "CM_COY_NAME AS 'Company Name', NO_PR AS 'No. of PR', NO_PO AS 'No. of PO', " & _
                "POM_CURRENCY_CODE AS 'Currency', POD_UNIT_AMT AS 'Total Amount (F)', POD_L_UNIT_AMT AS 'Total PO Amt (RM)' FROM " & _
                "(SELECT SUM(NO_PR) AS NO_PR, COUNT(POD_PO_NO) AS NO_PO, CM_COY_NAME, POD_OVERSEA, " & _
                "POM_CURRENCY_CODE, ACCT_CODE, SUM(POD_UNIT_AMT) AS POD_UNIT_AMT, SUM(POD_L_UNIT_AMT) AS POD_L_UNIT_AMT FROM " & _
                "(SELECT (IFNULL(NO_PR_PO,0) + IFNULL(NO_PR_RFQ,0)) AS NO_PR, POD_PO_NO, POM_S_COY_ID, POD_OVERSEA, " & _
                "POM_CURRENCY_CODE, POD_ACCT_CODE AS ACCT_CODE, SUM(POD_UNIT_AMT) AS POD_UNIT_AMT, " & _
                "SUM(POD_UNIT_AMT * POM_EXCHANGE_RATE) AS POD_L_UNIT_AMT FROM " & _
                "(SELECT POD_PO_NO, POD_VENDOR_ITEM_CODE, POD_PO_LINE, IFNULL(RM_RFQ_NO,'') AS POM_RFQ_NO, POM_S_COY_ID, POD_OVERSEA, POM_CURRENCY_CODE, " & _
                "IFNULL(AM_ACCT_CODE,'') AS POD_ACCT_CODE, " & _
                "(POD_UNIT_COST * POD_ORDERED_QTY) + ((POD_UNIT_COST * POD_ORDERED_QTY) / 100 * POD_GST) AS POD_UNIT_AMT, " & _
                "IFNULL(POM_EXCHANGE_RATE,1) AS POM_EXCHANGE_RATE " & _
                "FROM PO_DETAILS " & _
                "INNER JOIN PO_MSTR ON POD_PO_NO = POM_PO_NO AND POM_B_COY_ID = POD_COY_ID " & _
                "LEFT JOIN RFQ_MSTR ON POM_RFQ_INDEX = RM_RFQ_ID " & _
                "LEFT JOIN ACCOUNT_MSTR ON POD_ACCT_INDEX = AM_ACCT_INDEX " & _
                "WHERE POD_ITEM_TYPE = 'SP' AND POD_COY_ID = '" & Session("CompanyID") & "' AND POM_PO_DATE IS NOT NULL " & _
                "AND POM_PO_DATE >= '" & strBeginDate & "' AND POM_PO_DATE <= '" & strEndDate & "') tb_a " & _
                "LEFT JOIN " & _
                "(SELECT COUNT(PRM_PR_NO) AS NO_PR_PO, PRD_CONVERT_TO_DOC FROM " & _
                "(SELECT PRM_PR_NO, PRM_COY_ID, PRD_CONVERT_TO_DOC FROM PR_MSTR " & _
                "INNER JOIN PR_DETAILS ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID " & _
                "WHERE PRM_COY_ID = '" & Session("CompanyID") & "' AND PRD_CONVERT_TO_IND = 'PO' " & _
                "AND (PRM_PR_STATUS <> '6' AND PRM_PR_STATUS <> '8') " & _
                "GROUP BY PRM_PR_NO, PRD_CONVERT_TO_DOC) TB_A " & _
                "GROUP BY PRD_CONVERT_TO_DOC) tb_b " & _
                "ON tb_a.POD_PO_NO = tb_b.PRD_CONVERT_TO_DOC " & _
                "LEFT JOIN " & _
                "(SELECT COUNT(PRM_PR_NO) AS NO_PR_RFQ, PRD_CONVERT_TO_DOC FROM " & _
                "(SELECT PRM_PR_NO, PRM_COY_ID, PRD_CONVERT_TO_DOC FROM PR_MSTR " & _
                "INNER JOIN PR_DETAILS ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID " & _
                "WHERE PRM_COY_ID = '" & Session("CompanyID") & "' AND PRD_CONVERT_TO_IND = 'RFQ' " & _
                "AND (PRM_PR_STATUS <> '6' AND PRM_PR_STATUS <> '8') " & _
                "GROUP BY PRM_PR_NO, PRD_CONVERT_TO_DOC) TB_A " & _
                "GROUP BY PRD_CONVERT_TO_DOC) tb_c " & _
                "ON tb_a.POM_RFQ_NO = tb_c.PRD_CONVERT_TO_DOC " & _
                "GROUP BY POD_PO_NO, ACCT_CODE) tb_d " & _
                "LEFT JOIN COMPANY_MSTR ON CM_COY_ID = POM_S_COY_ID " & _
                "GROUP BY ACCT_CODE, POM_S_COY_ID, POD_OVERSEA, POM_CURRENCY_CODE " & _
                "UNION ALL " & _
                "SELECT COUNT(PRM_PR_NO) AS NO_PR, 0 AS NO_PO, '' AS CM_COY_NAME, PRD_OVERSEA AS POD_OVERSEA, " & _
                "'' AS POM_CURRENCY_CODE, IFNULL(AM_ACCT_CODE,'') AS ACCT_CODE, 0 AS POD_UNIT_AMT, 0 AS POD_L_UNIT_AMT " & _
                "FROM PR_MSTR " & _
                "INNER JOIN PR_DETAILS ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID " & _
                "LEFT JOIN ACCOUNT_MSTR ON AM_ACCT_INDEX = PRD_ACCT_INDEX " & _
                "WHERE PRM_COY_ID = '" & Session("CompanyID") & "' AND PRM_SUBMIT_DATE IS NOT NULL " & _
                "AND PRD_CONVERT_TO_DOC IS NULL AND PRD_ITEM_TYPE = 'SP' " & _
                "AND (PRM_PR_STATUS <> '6' AND PRM_PR_STATUS <> '8') " & _
                "AND PRM_SUBMIT_DATE >= '" & strBeginDate & "' AND PRM_SUBMIT_DATE <= '" & strEndDate & "' " & _
                "GROUP BY ACCT_CODE, POD_OVERSEA) tb " & _
                "ORDER BY ACCT_CODE, POD_OVERSEA, CM_COY_NAME "
        ds1 = objDb.FillDs(strSql)

        strSql = "SELECT SUM(IFNULL(NO_SP_PR,0)) AS 'Total No. of PR (Spot)', SUM(IFNULL(NO_INV_PR,0)) AS 'Total No. of PR (Inventory)', " & _
                "SUM(NO_SP_PO) AS 'Total No. of PO (Spot)', SUM(NO_INV_PO) AS 'Total No. of PO (Inventory)', " & _
                "SUM(PO_AMT_SP) AS 'Total PO (Spot) Amount (RM)', SUM(PO_AMT_INV) AS 'Total PO (Inventory) Amount (RM)', " & _
                "SUM(PM_LOCAL_SP_PO_AMT) AS 'Total PO Amount (RM) - Local Spot', SUM(PM_LOCAL_INV_PO_AMT) AS 'Total PO Amount (RM) - Local Inventory', " & _
                "SUM(PM_OVERSEA_SP_PO_AMT) AS 'Total PO Amount (RM) - Oversea Spot', SUM(PM_OVERSEA_INV_PO_AMT) AS 'Total PO Amount (RM) - Oversea Inventory' FROM " & _
                "(SELECT POM_DATE, POM_PO_DATE, COUNT(NO_SP_PO) AS NO_SP_PO, COUNT(NO_INV_PO) AS NO_INV_PO, " & _
                "SUM(PO_AMT_SP) AS PO_AMT_SP, SUM(PO_AMT_INV) AS PO_AMT_INV,  " & _
                "SUM(PM_LOCAL_SP_PO_AMT) AS PM_LOCAL_SP_PO_AMT, SUM(PM_LOCAL_INV_PO_AMT) AS PM_LOCAL_INV_PO_AMT,  " & _
                "SUM(PM_OVERSEA_SP_PO_AMT) AS PM_OVERSEA_SP_PO_AMT, SUM(PM_OVERSEA_INV_PO_AMT) AS PM_OVERSEA_INV_PO_AMT FROM " & _
                "(SELECT DATE_FORMAT(POM_PO_DATE,'%b %y') AS POM_DATE, POM_PO_DATE, POM_PO_NO, " & _
                "CASE WHEN POD_ITEM_TYPE = 'SP' THEN POM_PO_NO END AS NO_SP_PO, " & _
                "CASE WHEN POD_ITEM_TYPE <> 'SP' THEN POM_PO_NO END AS NO_INV_PO, " & _
                "CASE WHEN POD_ITEM_TYPE = 'SP' AND (POD_OVERSEA = 'Y' OR POD_OVERSEA = 'N') THEN (POM_PO_COST * IFNULL(POM_EXCHANGE_RATE,1)) ELSE 0 END AS PO_AMT_SP, " & _
                "CASE WHEN POD_ITEM_TYPE <> 'SP' AND (POD_OVERSEA = 'Y' OR POD_OVERSEA = 'N') THEN (POM_PO_COST * IFNULL(POM_EXCHANGE_RATE,1)) ELSE 0 END AS PO_AMT_INV, " & _
                "CASE WHEN (POD_ITEM_TYPE = 'SP' AND POD_OVERSEA = 'N') THEN POM_PO_COST * IFNULL(POM_EXCHANGE_RATE,1) ELSE 0 END AS PM_LOCAL_SP_PO_AMT, " & _
                "CASE WHEN (POD_ITEM_TYPE <> 'SP' AND POD_OVERSEA = 'N') THEN POM_PO_COST * IFNULL(POM_EXCHANGE_RATE,1) ELSE 0 END AS PM_LOCAL_INV_PO_AMT, " & _
                "CASE WHEN (POD_ITEM_TYPE = 'SP' AND POD_OVERSEA = 'Y') THEN POM_PO_COST * IFNULL(POM_EXCHANGE_RATE,1) ELSE 0 END AS PM_OVERSEA_SP_PO_AMT, " & _
                "CASE WHEN (POD_ITEM_TYPE <> 'SP' AND POD_OVERSEA = 'Y') THEN POM_PO_COST * IFNULL(POM_EXCHANGE_RATE,1) ELSE 0 END AS PM_OVERSEA_INV_PO_AMT " & _
                "FROM PO_MSTR POM, PO_DETAILS POD " & _
                "WHERE(POD.POD_PO_NO = POM.POM_PO_NO And POD.POD_COY_ID = POM.POM_B_COY_ID) " & _
                "AND POD.POD_COY_ID = '" & Session("CompanyID") & "' AND POM.POM_PO_DATE IS NOT NULL  " & _
                "AND POM.POM_PO_DATE >= '" & strBeginDate & "' AND POM.POM_PO_DATE <= '" & strEndDate & "' GROUP BY POM_PO_NO) AS TB_A " & _
                "GROUP BY POM_DATE) AS TB_A " & _
                "LEFT JOIN " & _
                "(SELECT PRM_DATE, PRM_SUBMIT_DATE, COUNT(NO_SP_PR) AS NO_SP_PR, COUNT(NO_INV_PR) AS NO_INV_PR FROM  " & _
                "(SELECT DATE_FORMAT(PRM_SUBMIT_DATE,'%b %y') AS PRM_DATE, PRM_SUBMIT_DATE,  " & _
                "CASE WHEN PRD_ITEM_TYPE = 'SP' THEN PRM_PR_NO END AS NO_SP_PR, " & _
                "CASE WHEN PRD_ITEM_TYPE <> 'SP' THEN PRM_PR_NO END AS NO_INV_PR, PRM_PR_NO  " & _
                "FROM PR_MSTR PRM, PR_DETAILS PRD WHERE PRM_COY_ID = '" & Session("CompanyID") & "' AND  " & _
                "PRM.PRM_COY_ID = PRD.PRD_COY_ID And PRM.PRM_PR_NO = PRD.PRD_PR_NO " & _
                "AND PRM_SUBMIT_DATE IS NOT NULL  " & _
                "AND PRM_SUBMIT_DATE >= '" & strBeginDate & "' AND PRM_SUBMIT_DATE <= '" & strEndDate & "' " & _
                "AND (PRM_PR_STATUS <> '6' AND PRM_PR_STATUS <> '8') GROUP BY PRM_PR_NO) AS TB_B  " & _
                "GROUP BY PRM_DATE ORDER BY PRM_SUBMIT_DATE) AS TB_B " & _
                "ON TB_A.POM_DATE = TB_B.PRM_DATE "
        ds2 = objDb.FillDs(strSql)

        strFileName = "POSummaryReport(Spot)"
        strFileName = strFileName & "(" & Format(dtDate, "MMMyyyy") & ").xls"
        Dim attachment As String = "attachment;filename=" & strFileName
        Response.ClearContent()
        Response.AddHeader("Content-Disposition", attachment)
        Response.ContentType = "application/vnd.ms-excel"

        i = 0
        Dim dc As DataColumn
        For Each dc In ds1.Tables(0).Columns
            If i > 0 Then
                Response.Write(vbTab + dc.ColumnName)
            Else
                Response.Write(dc.ColumnName)
            End If
            i += 1
        Next
        Response.Write(vbCrLf)

        Dim dr As DataRow
        For Each dr In ds1.Tables(0).Rows
            For i = 0 To ds1.Tables(0).Columns.Count - 1
                If i > 0 Then
                    Response.Write(vbTab + dr.Item(i).ToString)
                Else
                    Response.Write(dr.Item(i).ToString)
                End If
            Next
            Response.Write(vbCrLf)
        Next
        Response.Write(vbCrLf)

        i = 0
        Dim dc2 As DataColumn
        For Each dc2 In ds2.Tables(0).Columns
            If i > 0 Then
                Response.Write(vbTab + dc2.ColumnName)
            Else
                Response.Write(dc2.ColumnName)
            End If
            i += 1
        Next
        Response.Write(vbCrLf)

        Dim dr2 As DataRow
        For Each dr2 In ds2.Tables(0).Rows
            For i = 0 To ds2.Tables(0).Columns.Count - 1
                If i > 0 Then
                    Response.Write(vbTab + dr2.Item(i).ToString)
                Else
                    Response.Write(dr2.Item(i).ToString)
                End If
            Next
            Response.Write(vbCrLf)
        Next

        Response.End()

    End Sub

    Private Sub ExportToExcel()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim dtFrom As Date
        Dim dtTo As Date
        Dim dtDate As Date
        Dim strTemp As String = ""
        Dim strBeginDate, strEndDate As String
        Dim strGRNBeginDate, strGRNEndDate As String
        Dim strDate As String
        Dim strDate2 As String
        Dim strStart As String
        Dim strEnd As String = ""
        Dim strOversea As String
        Dim strFileName As String = ""
        Dim strYear1 As Integer
        Dim strMonth As String
        Dim strYear As String
        Dim strPastMth1, strPastMth2, strPastMth3, strPastMth4, strPastMth5, strPastMth6 As String
        Dim decClosingBal, intActive As Decimal

        Try
            If ViewState("ReportType") = "1" Or ViewState("ReportType") = "9" Or ViewState("ReportType") = "13" Then
                dtFrom = Me.txtSDate.Text
                dtTo = Me.txtEndDate.Text
                strBeginDate = Format(dtFrom, "yyyy-MM-dd" & " " & "00:00:00")
                strEndDate = Format(dtTo, "yyyy-MM-dd" & " " & "23:59:59")

                If ViewState("type") = "GRNLIST" Then
                    dtFrom = Me.txtGRNSDate.Text
                    dtTo = Me.txtGRNEndDate.Text
                    strGRNBeginDate = Format(dtFrom, "yyyy-MM-dd" & " " & "00:00:00")
                    strGRNEndDate = Format(dtTo, "yyyy-MM-dd" & " " & "23:59:59")
                End If

            ElseIf ViewState("ReportType") = "2" Then
                If Me.cmbMonthFrom.SelectedIndex > 0 Then
                    dtDate = New DateTime(Me.cmbYearFrom.SelectedValue, Me.cmbMonthFrom.SelectedValue, 1)
                    dtFrom = dtDate

                    If Me.cmbMonthTo.SelectedValue < 12 Then
                        dtDate = New DateTime(Me.cmbYearTo.SelectedValue, Me.cmbMonthTo.SelectedValue + 1, 1)
                        dtTo = DateAdd(DateInterval.Day, -1, dtDate)
                        dtTo = New DateTime(Me.cmbYearTo.SelectedValue, Me.cmbMonthTo.SelectedValue, dtTo.Day)

                    Else
                        dtDate = New DateTime(Me.cmbYearFrom.SelectedValue, Me.cmbMonthFrom.SelectedValue, 1)
                        dtFrom = dtDate
                        dtTo = New DateTime(Me.cmbYearTo.SelectedValue, Me.cmbMonthTo.SelectedValue, 31)
                    End If

                Else
                    dtDate = New DateTime(Me.cmbYearFrom.SelectedValue, 1, 1)
                    dtFrom = dtDate
                    dtTo = New DateTime(Me.cmbYearTo.SelectedValue, 12, 31)
                End If

                strBeginDate = Format(dtFrom, "yyyy-MM-dd" & " " & "00:00:00")
                strEndDate = Format(dtTo, "yyyy-MM-dd" & " " & "23:59:59")
                strStart = Format(dtFrom, "MMM yyyy")
                strEnd = Format(dtTo, "MMM yyyy")

            ElseIf ViewState("ReportType") = "3" Then
                strOversea = Me.cmbOversea.SelectedValue
                dtDate = Me.txtDate.Text
                strDate = Format(dtDate, "yyyy-MM-dd")

            ElseIf ViewState("ReportType") = "4" Then
                If ViewState("type") = "SUMMTHSTKBAL" Then
                    strOversea = Me.cmbOversea.SelectedValue
                    strMonth = cmbMonth.SelectedValue
                    strYear = cmbYear.SelectedValue

                    If Me.cmbMonth.SelectedIndex > 0 Then
                        dtFrom = New DateTime(Me.cmbYear.SelectedValue, Me.cmbMonth.SelectedValue, 1)
                    End If

                ElseIf ViewState("type") = "MTHCOSTTREND" Or ViewState("type") = "MTHMGMT" Then
                    strOversea = Me.cmbOversea.SelectedValue
                    dtDate = New DateTime(Me.cmbYear.SelectedValue, Me.cmbMonth.SelectedValue, 1)
                    strDate = Format(dtDate, "yyyy-MM-dd")

                    If Me.cmbMonth.SelectedIndex > 0 Then
                        dtFrom = New DateTime(Me.cmbYear.SelectedValue, Me.cmbMonth.SelectedValue, 1)
                    End If

                    'Header
                    dtDate = DateAdd(DateInterval.Month, -1, dtDate)
                    strPastMth1 = Format(dtDate, "MMM-yyyy")
                    dtDate = DateAdd(DateInterval.Month, -1, dtDate)
                    strPastMth2 = Format(dtDate, "MMM-yyyy")
                    dtDate = DateAdd(DateInterval.Month, -1, dtDate)
                    strPastMth3 = Format(dtDate, "MMM-yyyy")
                    dtDate = DateAdd(DateInterval.Month, -1, dtDate)
                    strPastMth4 = Format(dtDate, "MMM-yyyy")
                    dtDate = DateAdd(DateInterval.Month, -1, dtDate)
                    strPastMth5 = Format(dtDate, "MMM-yyyy")
                    dtDate = DateAdd(DateInterval.Month, -1, dtDate)
                    strPastMth6 = Format(dtDate, "MMM-yyyy")

                Else
                    'If Me.cmbMonth.SelectedIndex > 0 Then
                    '    dtDate = New DateTime(Me.cmbYear.SelectedValue, Me.cmbMonth.SelectedValue, 1)
                    '    dtFrom = dtDate

                    '    If Me.cmbMonth.SelectedValue < 12 Then
                    '        dtDate = New DateTime(Me.cmbYear.SelectedValue, Me.cmbMonth.SelectedValue + 1, 1)
                    '        dtTo = DateAdd(DateInterval.Day, -1, dtDate)
                    '        dtTo = New DateTime(Me.cmbYear.SelectedValue, Me.cmbMonth.SelectedValue, dtTo.Day)

                    '    Else    'December
                    '        dtTo = New DateTime(Me.cmbYear.SelectedValue, Me.cmbMonth.SelectedValue, 31)
                    '    End If

                    'Else
                    'End If

                    'strYear1 = Format(dtDate, "yyyy")
                    'strMonth1 = Format(dtDate, "MM")
                    'strOversea = Me.cmbOversea.SelectedValue
                    'strMonth = cmbMonth.SelectedValue
                    'strYear = cmbYear.SelectedValue
                    'strDate = Format(dtDate, "yyyy-MM-dd")
                    'strBeginDate = Format(dtFrom, "yyyy-MM-dd" & " " & "00:00:00")
                    'strEndDate = Format(dtTo, "yyyy-MM-dd" & " " & "23:59:59")
                    'dtDate = dtFrom
                    'strMonth1 = Format(dtDate, "MMM-yyyy")
                    'dtDate = DateAdd(DateInterval.Month, -1, dtDate)
                    'strMonth2 = Format(dtDate, "MMM-yyyy")
                    'dtDate = DateAdd(DateInterval.Month, -1, dtDate)
                    'strMonth3 = Format(dtDate, "MMM-yyyy")
                    'dtDate = DateAdd(DateInterval.Month, -1, dtDate)
                    'strMonth4 = Format(dtDate, "MMM-yyyy")
                    'dtDate = DateAdd(DateInterval.Month, -1, dtDate)
                    'strMonth5 = Format(dtDate, "MMM-yyyy")
                    'dtDate = DateAdd(DateInterval.Month, -1, dtDate)
                    'strMonth6 = Format(dtDate, "MMM-yyyy")
                    'dtDate = DateAdd(DateInterval.Month, -1, dtDate)
                    'strMonth7 = Format(dtDate, "MMM-yyyy")

                End If

            ElseIf ViewState("ReportType") = "5" Then
                If Me.cmbMonth.SelectedIndex > 0 Then
                    dtDate = New DateTime(Me.cmbYear.SelectedValue, Me.cmbMonth.SelectedValue, 1)
                    dtFrom = dtDate

                    If Me.cmbMonth.SelectedValue < 12 Then
                        dtDate = New DateTime(Me.cmbYear.SelectedValue, Me.cmbMonth.SelectedValue + 1, 1)
                        dtTo = DateAdd(DateInterval.Day, -1, dtDate)
                        dtTo = New DateTime(Me.cmbYear.SelectedValue, Me.cmbMonth.SelectedValue, dtTo.Day)

                    Else    'December
                        dtTo = New DateTime(Me.cmbYear.SelectedValue, Me.cmbMonth.SelectedValue, 31)
                    End If

                Else
                    dtDate = New DateTime(Me.cmbYear.SelectedValue, 1, 1)
                    dtFrom = dtDate
                    dtTo = New DateTime(Me.cmbYear.SelectedValue, 12, 31)
                End If

                strBeginDate = Format(dtFrom, "yyyy-MM-dd" & " " & "00:00:00")
                strEndDate = Format(dtTo, "yyyy-MM-dd" & " " & "23:59:59")
                strMonth = Format(strMonth, "MM")
                strYear = Format(strYear, "yyyy")

            ElseIf ViewState("ReportType") = "7" Or ViewState("ReportType") = "8" Or ViewState("ReportType") = "12" Then
                dtFrom = Me.txtSDate.Text
                dtTo = Me.txtEndDate.Text
                strBeginDate = Format(dtFrom, "yyyy-MM-dd" & " " & "00:00:00")
                strEndDate = Format(dtTo, "yyyy-MM-dd" & " " & "23:59:59")
                strOversea = Me.cmbOversea.SelectedValue

            ElseIf ViewState("ReportType") = "10" Then
                dtFrom = Me.txtDate.Text
                strDate = Format(dtFrom, "yyyy-MM-dd" & " " & "23:59:59")

                If ViewState("type") = "DELTREND" Then
                    strDate2 = Format(dtFrom.AddMonths(-6), "yyyy-MM-dd" & " " & "00:00:00")
                End If
            ElseIf ViewState("ReportType") = "11" Then
                strOversea = Me.cmbOversea.SelectedValue
                dtFrom = Today.Date

            End If

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"
            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                If ViewState("type") = "TOP20VEN" Then
                    .CommandText = "SELECT A.CM_COY_NAME AS 'Vendor Company', CAST(POM_PO_LOCAL_COST AS DECIMAL(12,2)) AS 'PO Amount (RM)', POM_CURRENCY_CODE AS 'Currency', POM_PO_COST AS 'PO Amount (Foreign)' FROM " & _
                                "(SELECT CM_COY_NAME, SUM(POM_PO_LOCAL_COST) AS POM_PO_LOCAL_COST FROM " & _
                                "(SELECT PO_MSTR.*, CM_COY_NAME, (POM_PO_COST * IFNULL(POM_EXCHANGE_RATE,1)) AS POM_PO_LOCAL_COST " & _
                                "FROM PO_MSTR INNER JOIN PO_DETAILS ON POM_B_COY_ID = POD_COY_ID AND POM_PO_NO = POD_PO_NO " & _
                                "INNER JOIN COMPANY_MSTR ON POM_S_COY_ID = CM_COY_ID WHERE POM_B_COY_ID = '" & Session("CompanyID") & "' " & _
                                "AND POD_ITEM_TYPE = 'SP' AND (POM_PO_STATUS = '1' OR POM_PO_STATUS = '2' OR POM_PO_STATUS = '3' " & _
                                "OR POM_PO_STATUS = '6') AND POM_PO_DATE >= '" & strBeginDate & "' AND POM_PO_DATE <= '" & strEndDate & "' " & _
                                "GROUP BY POM_PO_NO) tb GROUP BY CM_COY_NAME ORDER BY POM_PO_LOCAL_COST DESC LIMIT 20) a " & _
                                "INNER JOIN (SELECT CM_COY_NAME, SUM(POM_PO_COST) AS POM_PO_COST, POM_CURRENCY_CODE FROM " & _
                                "(SELECT PO_MSTR.*, CM_COY_NAME FROM PO_MSTR INNER JOIN PO_DETAILS ON POM_B_COY_ID = POD_COY_ID AND " & _
                                "POM_PO_NO = POD_PO_NO INNER JOIN COMPANY_MSTR ON POM_S_COY_ID = CM_COY_ID WHERE POM_B_COY_ID = '" & Session("CompanyID") & "' " & _
                                "AND POD_ITEM_TYPE = 'SP' AND (POM_PO_STATUS = '1' OR POM_PO_STATUS = '2' OR " & _
                                "POM_PO_STATUS = '3' OR POM_PO_STATUS = '6') AND POM_PO_DATE >= '" & strBeginDate & "' AND " & _
                                "POM_PO_DATE <= '" & strEndDate & "' GROUP BY POM_PO_NO) tb GROUP BY CM_COY_NAME, POM_CURRENCY_CODE) b " & _
                                "ON a.CM_COY_NAME = b.CM_COY_NAME ORDER BY POM_PO_LOCAL_COST DESC "

                    'ElseIf ViewState("type") = "LOCALOVERSEAITEM" Then
                    '    .CommandText = "SELECT SUM(IFNULL(NO_SP_PR,0)) AS 'Total No. of PR (Spot)', SUM(IFNULL(NO_INV_PR,0)) AS 'Total No. of PR (Inventory)', " & _
                    '                "SUM(NO_SP_PO) AS 'Total No. of PO (Spot)', SUM(NO_INV_PO) AS 'Total No. of PO (Inventory)', " & _
                    '                "SUM(PO_AMT_SP) AS 'Total PO (Spot) Amount (RM)', SUM(PO_AMT_INV) AS 'Total PO (Inventory) Amount (RM)', " & _
                    '                "SUM(PM_LOCAL_SP_PO_AMT) AS 'Total PO Amount (RM) - Local Spot', SUM(PM_LOCAL_INV_PO_AMT) AS 'Total PO Amount (RM) - Local Inventory', " & _
                    '                "SUM(PM_OVERSEA_SP_PO_AMT) AS 'Total PO Amount (RM) - Oversea Spot', SUM(PM_OVERSEA_INV_PO_AMT) AS 'Total PO Amount (RM) - Oversea Inventory' FROM " & _
                    '                "(SELECT POM_DATE, POM_PO_DATE, COUNT(NO_SP_PO) AS NO_SP_PO, COUNT(NO_INV_PO) AS NO_INV_PO, " & _
                    '                "SUM(PO_AMT_SP) AS PO_AMT_SP, SUM(PO_AMT_INV) AS PO_AMT_INV,  " & _
                    '                "SUM(PM_LOCAL_SP_PO_AMT) AS PM_LOCAL_SP_PO_AMT, SUM(PM_LOCAL_INV_PO_AMT) AS PM_LOCAL_INV_PO_AMT,  " & _
                    '                "SUM(PM_OVERSEA_SP_PO_AMT) AS PM_OVERSEA_SP_PO_AMT, SUM(PM_OVERSEA_INV_PO_AMT) AS PM_OVERSEA_INV_PO_AMT FROM " & _
                    '                "(SELECT DATE_FORMAT(POM_PO_DATE,'%b %y') AS POM_DATE, POM_PO_DATE, POM_PO_NO, " & _
                    '                "CASE WHEN POD_ITEM_TYPE = 'SP' THEN POM_PO_NO END AS NO_SP_PO, " & _
                    '                "CASE WHEN POD_ITEM_TYPE <> 'SP' THEN POM_PO_NO END AS NO_INV_PO, " & _
                    '                "CASE WHEN POD_ITEM_TYPE = 'SP' AND (POD_OVERSEA = 'Y' OR POD_OVERSEA = 'N') THEN (POM_PO_COST * IFNULL(POM_EXCHANGE_RATE,1)) ELSE 0 END AS PO_AMT_SP, " & _
                    '                "CASE WHEN POD_ITEM_TYPE <> 'SP' AND (POD_OVERSEA = 'Y' OR POD_OVERSEA = 'N') THEN (POM_PO_COST * IFNULL(POM_EXCHANGE_RATE,1)) ELSE 0 END AS PO_AMT_INV, " & _
                    '                "CASE WHEN (POD_ITEM_TYPE = 'SP' AND POD_OVERSEA = 'N') THEN POM_PO_COST * IFNULL(POM_EXCHANGE_RATE,1) ELSE 0 END AS PM_LOCAL_SP_PO_AMT, " & _
                    '                "CASE WHEN (POD_ITEM_TYPE <> 'SP' AND POD_OVERSEA = 'N') THEN POM_PO_COST * IFNULL(POM_EXCHANGE_RATE,1) ELSE 0 END AS PM_LOCAL_INV_PO_AMT, " & _
                    '                "CASE WHEN (POD_ITEM_TYPE = 'SP' AND POD_OVERSEA = 'Y') THEN POM_PO_COST * IFNULL(POM_EXCHANGE_RATE,1) ELSE 0 END AS PM_OVERSEA_SP_PO_AMT, " & _
                    '                "CASE WHEN (POD_ITEM_TYPE <> 'SP' AND POD_OVERSEA = 'Y') THEN POM_PO_COST * IFNULL(POM_EXCHANGE_RATE,1) ELSE 0 END AS PM_OVERSEA_INV_PO_AMT " & _
                    '                "FROM PO_MSTR POM, PO_DETAILS POD " & _
                    '                "WHERE(POD.POD_PO_NO = POM.POM_PO_NO And POD.POD_COY_ID = POM.POM_B_COY_ID) " & _
                    '                "AND POD.POD_COY_ID = '" & Session("CompanyID") & "' AND POM.POM_PO_DATE IS NOT NULL  " & _
                    '                "AND POM.POM_PO_DATE >= '" & strBeginDate & "' AND POM.POM_PO_DATE <= '" & strEndDate & "' GROUP BY POM_PO_NO) AS TB_A " & _
                    '                "GROUP BY POM_DATE) AS TB_A " & _
                    '                "LEFT JOIN " & _
                    '                "(SELECT PRM_DATE, PRM_SUBMIT_DATE, COUNT(NO_SP_PR) AS NO_SP_PR, COUNT(NO_INV_PR) AS NO_INV_PR FROM  " & _
                    '                "(SELECT DATE_FORMAT(PRM_SUBMIT_DATE,'%b %y') AS PRM_DATE, PRM_SUBMIT_DATE,  " & _
                    '                "CASE WHEN PRD_ITEM_TYPE = 'SP' THEN PRM_PR_NO END AS NO_SP_PR, " & _
                    '                "CASE WHEN PRD_ITEM_TYPE <> 'SP' THEN PRM_PR_NO END AS NO_INV_PR, PRM_PR_NO  " & _
                    '                "FROM PR_MSTR PRM, PR_DETAILS PRD WHERE PRM_COY_ID = '" & Session("CompanyID") & "' AND  " & _
                    '                "PRM.PRM_COY_ID = PRD.PRD_COY_ID And PRM.PRM_PR_NO = PRD.PRD_PR_NO " & _
                    '                "AND PRM_SUBMIT_DATE IS NOT NULL  " & _
                    '                "AND PRM_SUBMIT_DATE >= '" & strBeginDate & "' AND PRM_SUBMIT_DATE <= '" & strEndDate & "' " & _
                    '                "AND (PRM_PR_STATUS <> '6' AND PRM_PR_STATUS <> '8') GROUP BY PRM_PR_NO) AS TB_B  " & _
                    '                "GROUP BY PRM_DATE ORDER BY PRM_SUBMIT_DATE) AS TB_B " & _
                    '                "ON TB_A.POM_DATE = TB_B.PRM_DATE "

                ElseIf ViewState("type") = "LOCALOVERSEAPUR" Then
                    .CommandText = "SELECT POM_DATE AS 'Month/Year', IFNULL(NO_PR,0) AS 'Total No. of PR Received & Process', " & _
                                "IFNULL(NO_PO,0) AS 'Total No. of PO Issued', POM_PO_COST AS 'Total PO Amount (RM)', " & _
                                "PM_LOCAL_PO_AMT AS 'Total Local PO Amount (RM)', PM_OVERSEA_PO_AMT AS 'Total Overseas PO Amount (RM)' " & _
                                "FROM " & _
                                "(SELECT POM_DATE, POM_PO_DATE, COUNT(POM_PO_NO) AS NO_PO, SUM(POM_PO_COST) AS POM_PO_COST, " & _
                                "SUM(PM_LOCAL_PO_AMT) AS PM_LOCAL_PO_AMT, SUM(PM_OVERSEA_PO_AMT) AS PM_OVERSEA_PO_AMT " & _
                                "FROM " & _
                                "(SELECT DATE_FORMAT(POM_PO_DATE,'%b %y') AS POM_DATE, POM_PO_NO, POM_PO_DATE, " & _
                                "(POM_PO_COST * IFNULL(POM_EXCHANGE_RATE,1)) AS POM_PO_COST, " & _
                                "CASE WHEN POD_OVERSEA = 'N' THEN POM_PO_COST * IFNULL(POM_EXCHANGE_RATE,1) ELSE 0 END AS PM_LOCAL_PO_AMT, " & _
                                "CASE WHEN POD_OVERSEA = 'Y' THEN POM_PO_COST * IFNULL(POM_EXCHANGE_RATE,1) ELSE 0 END AS PM_OVERSEA_PO_AMT " & _
                                "FROM PO_MSTR " & _
                                "INNER JOIN PO_DETAILS ON POD_PO_NO = POM_PO_NO AND POD_COY_ID = POM_B_COY_ID " & _
                                "WHERE POM_B_COY_ID = '" & Session("CompanyID") & "' AND POM_PO_DATE IS NOT NULL " & _
                                "AND (POD_OVERSEA = 'N' OR POD_OVERSEA = 'Y') " & _
                                "AND (POM_PO_STATUS = '1' OR POM_PO_STATUS = '2' OR POM_PO_STATUS = '3' OR POM_PO_STATUS = '4' " & _
                                "OR POM_PO_STATUS = '4' OR POM_PO_STATUS = '5' OR POM_PO_STATUS = '6') " & _
                                "AND POM_PO_DATE >= '" & strBeginDate & "' AND POM_PO_DATE <= '" & strEndDate & "' GROUP BY POM_PO_NO) tb " & _
                                "GROUP BY POM_DATE) AS TB_A " & _
                                "LEFT JOIN " & _
                                "(SELECT DATE_FORMAT(PRM_SUBMIT_DATE,'%b %y') AS PRM_DATE, PRM_SUBMIT_DATE, " & _
                                "COUNT(PRM_PR_NO) AS NO_PR FROM PR_MSTR WHERE PRM_COY_ID = '" & Session("CompanyID") & "' AND PRM_SUBMIT_DATE IS NOT NULL " & _
                                "AND PRM_SUBMIT_DATE >= '" & strBeginDate & "' AND PRM_SUBMIT_DATE <= '" & strEndDate & "' " & _
                                "AND (PRM_PR_STATUS <> '6' AND PRM_PR_STATUS <> '8') GROUP BY PRM_DATE " & _
                                "ORDER BY PRM_SUBMIT_DATE) AS TB_B " & _
                                "ON TB_A.POM_DATE = TB_B.PRM_DATE " & _
                                "ORDER BY POM_PO_DATE "

                ElseIf ViewState("type") = "PRPOFORINV" Then
                    .CommandText = "SELECT CONCAT('''', ACCT_CODE) AS 'Section Code', CASE WHEN POD_OVERSEA = 'N' THEN 'No' ELSE 'Yes' END AS 'Oversea', " & _
                                "CM_COY_NAME AS 'Company Name', NO_PR AS 'No. of PR', NO_PO AS 'No. of PO', " & _
                                "POM_CURRENCY_CODE AS 'Currency', POD_UNIT_AMT AS 'Total Amount (F)', POD_L_UNIT_AMT AS 'Total PO Amt (RM)' FROM " & _
                                "(SELECT SUM(NO_PR) AS NO_PR, COUNT(POD_PO_NO) AS NO_PO, CM_COY_NAME, POD_OVERSEA, " & _
                                "POM_CURRENCY_CODE, ACCT_CODE, SUM(POD_UNIT_AMT) AS POD_UNIT_AMT, SUM(POD_L_UNIT_AMT) AS POD_L_UNIT_AMT FROM " & _
                                "(SELECT (IFNULL(NO_PR_PO,0) + IFNULL(NO_PR_RFQ,0)) AS NO_PR, POD_PO_NO, POM_S_COY_ID, POD_OVERSEA, " & _
                                "POM_CURRENCY_CODE, POD_ACCT_CODE AS ACCT_CODE, SUM(POD_UNIT_AMT) AS POD_UNIT_AMT, " & _
                                "SUM(POD_UNIT_AMT * POM_EXCHANGE_RATE) AS POD_L_UNIT_AMT FROM " & _
                                "(SELECT POD_PO_NO, POD_VENDOR_ITEM_CODE, POD_PO_LINE, IFNULL(RM_RFQ_NO,'') AS POM_RFQ_NO, POM_S_COY_ID, POD_OVERSEA, POM_CURRENCY_CODE, " & _
                                "IFNULL(AM_ACCT_CODE,'') AS POD_ACCT_CODE, " & _
                                "(POD_UNIT_COST * POD_ORDERED_QTY) + ((POD_UNIT_COST * POD_ORDERED_QTY) / 100 * POD_GST) AS POD_UNIT_AMT, " & _
                                "IFNULL(POM_EXCHANGE_RATE,1) AS POM_EXCHANGE_RATE " & _
                                "FROM PO_DETAILS " & _
                                "INNER JOIN PO_MSTR ON POD_PO_NO = POM_PO_NO AND POM_B_COY_ID = POD_COY_ID " & _
                                "LEFT JOIN RFQ_MSTR ON POM_RFQ_INDEX = RM_RFQ_ID " & _
                                "LEFT JOIN ACCOUNT_MSTR ON POD_ACCT_INDEX = AM_ACCT_INDEX " & _
                                "WHERE POD_ITEM_TYPE <> 'SP' AND POD_COY_ID = '" & Session("CompanyID") & "' AND POM_PO_DATE IS NOT NULL " & _
                                "AND (AM_ACCT_CODE = '90105A' OR AM_ACCT_CODE = '90105B' OR AM_ACCT_CODE = '50801A' " & _
                                "OR AM_ACCT_CODE = '50801B' OR AM_ACCT_CODE = '50802A' OR AM_ACCT_CODE = '50802B') " & _
                                "AND POM_PO_DATE >= '" & strBeginDate & "' AND POM_PO_DATE <= '" & strEndDate & "') tb_a " & _
                                "LEFT JOIN " & _
                                "(SELECT COUNT(PRM_PR_NO) AS NO_PR_PO, PRD_CONVERT_TO_DOC FROM " & _
                                "(SELECT PRM_PR_NO, PRM_COY_ID, PRD_CONVERT_TO_DOC FROM PR_MSTR " & _
                                "INNER JOIN PR_DETAILS ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID " & _
                                "WHERE PRM_COY_ID = '" & Session("CompanyID") & "' AND PRD_CONVERT_TO_IND = 'PO' " & _
                                "GROUP BY PRM_PR_NO, PRD_CONVERT_TO_DOC) TB_A " & _
                                "GROUP BY PRD_CONVERT_TO_DOC) tb_b " & _
                                "ON tb_a.POD_PO_NO = tb_b.PRD_CONVERT_TO_DOC " & _
                                "LEFT JOIN " & _
                                "(SELECT COUNT(PRM_PR_NO) AS NO_PR_RFQ, PRD_CONVERT_TO_DOC FROM " & _
                                "(SELECT PRM_PR_NO, PRM_COY_ID, PRD_CONVERT_TO_DOC FROM PR_MSTR " & _
                                "INNER JOIN PR_DETAILS ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID " & _
                                "WHERE PRM_COY_ID = '" & Session("CompanyID") & "' AND PRD_CONVERT_TO_IND = 'RFQ' " & _
                                "GROUP BY PRM_PR_NO, PRD_CONVERT_TO_DOC) TB_A " & _
                                "GROUP BY PRD_CONVERT_TO_DOC) tb_c " & _
                                "ON tb_a.POM_RFQ_NO = tb_c.PRD_CONVERT_TO_DOC " & _
                                "GROUP BY POD_PO_NO, ACCT_CODE) tb_d " & _
                                "LEFT JOIN COMPANY_MSTR ON CM_COY_ID = POM_S_COY_ID " & _
                                "GROUP BY ACCT_CODE, POM_S_COY_ID, POD_OVERSEA, POM_CURRENCY_CODE " & _
                                "UNION ALL " & _
                                "SELECT COUNT(PRM_PR_NO) AS NO_PR, 0 AS NO_PO, '' AS CM_COY_NAME, PRD_OVERSEA AS POD_OVERSEA, " & _
                                "'' AS POM_CURRENCY_CODE, IFNULL(AM_ACCT_CODE,'') AS ACCT_CODE, 0 AS POD_UNIT_AMT, 0 AS POD_L_UNIT_AMT " & _
                                "FROM PR_MSTR " & _
                                "INNER JOIN PR_DETAILS ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID " & _
                                "LEFT JOIN ACCOUNT_MSTR ON AM_ACCT_INDEX = PRD_ACCT_INDEX " & _
                                "WHERE PRM_COY_ID = '" & Session("CompanyID") & "' AND PRM_SUBMIT_DATE IS NOT NULL " & _
                                "AND PRD_CONVERT_TO_DOC IS NULL AND PRD_ITEM_TYPE <> 'SP' " & _
                                "AND (PRM_PR_STATUS <> '6' AND PRM_PR_STATUS <> '8') " & _
                                "AND (AM_ACCT_CODE = '90105A' OR AM_ACCT_CODE = '90105B' OR AM_ACCT_CODE = '50801A' " & _
                                "OR AM_ACCT_CODE = '50801B' OR AM_ACCT_CODE = '50802A' OR AM_ACCT_CODE = '50802B') " & _
                                "AND PRM_SUBMIT_DATE >= '" & strBeginDate & "' AND PRM_SUBMIT_DATE <= '" & strEndDate & "' " & _
                                "GROUP BY ACCT_CODE, POD_OVERSEA) tb " & _
                                "ORDER BY ACCT_CODE, POD_OVERSEA, CM_COY_NAME "

                    '.CommandText = "SELECT ACCT_CODE AS 'Section Code', CASE WHEN POD_OVERSEA = 'N' THEN 'No' ELSE 'Yes' END AS 'Oversea', " & _
                    '            "CM_COY_NAME AS 'Company Name', SUM(NO_PR) AS 'No. of PR', SUM(NO_PO) AS 'No. of PO', " & _
                    '            "POM_CURRENCY_CODE AS 'Currency', SUM(POD_UNIT_AMT) AS 'Total Amount (F)', SUM(POD_UNIT_AMT) AS 'Total PO Amt (RM)' FROM " & _
                    '            "(SELECT NO_PR, 1 AS NO_PO, CM_COY_NAME, POD_OVERSEA, " & _
                    '            "POM_CURRENCY_CODE, ACCT_CODE, POD_UNIT_AMT, POD_L_UNIT_AMT FROM " & _
                    '            "(SELECT (IFNULL(NO_PR_PO,0) + IFNULL(NO_PR_RFQ,0)) AS NO_PR, POD_PO_NO, POM_S_COY_ID, POD_OVERSEA, " & _
                    '            "POM_CURRENCY_CODE, POD_ACCT_CODE AS ACCT_CODE, SUM(POD_UNIT_AMT) AS POD_UNIT_AMT, " & _
                    '            "SUM(POD_L_UNIT_AMT) AS POD_L_UNIT_AMT FROM " & _
                    '            "(SELECT POD_PO_NO, POD_VENDOR_ITEM_CODE, POD_PO_LINE, IFNULL(RM_RFQ_NO,'') AS POM_RFQ_NO, POM_S_COY_ID, POD_OVERSEA, POM_CURRENCY_CODE, " & _
                    '            "IFNULL(AM_ACCT_CODE,'') AS POD_ACCT_CODE, " & _
                    '            "(POD_UNIT_COST * POD_ORDERED_QTY) + ((POD_UNIT_COST * POD_ORDERED_QTY) / 100 * POD_GST) AS POD_UNIT_AMT, " & _
                    '            "((POD_UNIT_COST * POD_ORDERED_QTY) + ((POD_UNIT_COST * POD_ORDERED_QTY) / 100 * POD_GST) * IFNULL(POM_EXCHANGE_RATE,1)) AS POD_L_UNIT_AMT " & _
                    '            "FROM PO_DETAILS " & _
                    '            "INNER JOIN PO_MSTR ON POD_PO_NO = POM_PO_NO AND POM_B_COY_ID = POD_COY_ID " & _
                    '            "LEFT JOIN RFQ_MSTR ON POM_RFQ_INDEX = RM_RFQ_ID " & _
                    '            "LEFT JOIN ACCOUNT_MSTR ON POD_ACCT_INDEX = AM_ACCT_INDEX " & _
                    '            "WHERE POD_ITEM_TYPE <> 'SP' AND POD_COY_ID = '" & Session("CompanyID") & "' AND POM_PO_DATE IS NOT NULL " & _
                    '            "AND (AM_ACCT_CODE = '90105 A' OR AM_ACCT_CODE = '90105 B' OR AM_ACCT_CODE = '50801 A' " & _
                    '            "OR AM_ACCT_CODE = '50801 B' OR AM_ACCT_CODE = '50802 A' OR AM_ACCT_CODE = '50802 B') " & _
                    '            "AND POM_PO_DATE >= '" & strBeginDate & "' AND POM_PO_DATE <= '" & strEndDate & "') tb_a " & _
                    '            "LEFT JOIN " & _
                    '            "(SELECT COUNT(PRM_PR_NO) AS NO_PR_PO, PRD_CONVERT_TO_DOC FROM " & _
                    '            "(SELECT PRM_PR_NO, PRM_COY_ID, PRD_CONVERT_TO_DOC FROM PR_MSTR " & _
                    '            "INNER JOIN PR_DETAILS ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID " & _
                    '            "WHERE PRM_COY_ID = '" & Session("CompanyID") & "' AND PRD_CONVERT_TO_IND = 'PO' " & _
                    '            "GROUP BY PRM_PR_NO, PRD_CONVERT_TO_DOC) TB_A " & _
                    '            "GROUP BY PRD_CONVERT_TO_DOC) tb_b " & _
                    '            "ON tb_a.POD_PO_NO = tb_b.PRD_CONVERT_TO_DOC " & _
                    '            "LEFT JOIN " & _
                    '            "(SELECT COUNT(PRM_PR_NO) AS NO_PR_RFQ, PRD_CONVERT_TO_DOC FROM " & _
                    '            "(SELECT PRM_PR_NO, PRM_COY_ID, PRD_CONVERT_TO_DOC FROM PR_MSTR " & _
                    '            "INNER JOIN PR_DETAILS ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID " & _
                    '            "WHERE PRM_COY_ID = '" & Session("CompanyID") & "' AND PRD_CONVERT_TO_IND = 'RFQ' " & _
                    '            "GROUP BY PRM_PR_NO, PRD_CONVERT_TO_DOC) TB_A " & _
                    '            "GROUP BY PRD_CONVERT_TO_DOC) tb_c " & _
                    '            "ON tb_a.POM_RFQ_NO = tb_c.PRD_CONVERT_TO_DOC " & _
                    '            "GROUP BY POD_PO_NO, ACCT_CODE) tb_d " & _
                    '            "INNER JOIN COMPANY_MSTR ON CM_COY_ID = POM_S_COY_ID " & _
                    '            "GROUP BY ACCT_CODE, POM_S_COY_ID, POD_OVERSEA, POM_CURRENCY_CODE " & _
                    '            "UNION ALL " & _
                    '            "SELECT COUNT(PRM_PR_NO) AS NO_PR, 0 AS NO_PO, '' AS CM_COY_NAME, PRD_OVERSEA AS POD_OVERSEA, " & _
                    '            "'' AS POM_CURRENCY_CODE, IFNULL(AM_ACCT_CODE,'') AS ACCT_CODE, 0 AS POD_UNIT_AMT, 0 AS POD_L_UNIT_AMT " & _
                    '            "FROM PR_MSTR " & _
                    '            "INNER JOIN PR_DETAILS ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID " & _
                    '            "LEFT JOIN ACCOUNT_MSTR ON AM_ACCT_INDEX = PRD_ACCT_INDEX " & _
                    '            "WHERE PRM_COY_ID = '" & Session("CompanyID") & "' AND PRM_SUBMIT_DATE IS NOT NULL " & _
                    '            "AND PRD_CONVERT_TO_DOC IS NULL AND PRD_ITEM_TYPE <> 'SP' " & _
                    '            "AND (AM_ACCT_CODE = '90105 A' OR AM_ACCT_CODE = '90105 B' OR AM_ACCT_CODE = '50801 A' " & _
                    '            "OR AM_ACCT_CODE = '50801 B' OR AM_ACCT_CODE = '50802 A' OR AM_ACCT_CODE = '50802 B') " & _
                    '            "AND PRM_SUBMIT_DATE >= '" & strBeginDate & "' AND PRM_SUBMIT_DATE <= '" & strEndDate & "' " & _
                    '            "GROUP BY ACCT_CODE, POD_OVERSEA) tb " & _
                    '            "GROUP BY ACCT_CODE, CM_COY_NAME, POD_OVERSEA, POM_CURRENCY_CODE " & _
                    '            "ORDER BY ACCT_CODE, POD_OVERSEA, CM_COY_NAME "

                ElseIf ViewState("type") = "PRPOFORSP" Then
                    .CommandText = "SELECT CONCAT('''', ACCT_CODE) AS 'Section Code', CASE WHEN POD_OVERSEA = 'N' THEN 'No' ELSE 'Yes' END AS 'Oversea', " & _
                                "CM_COY_NAME AS 'Company Name', NO_PR AS 'No. of PR', NO_PO AS 'No. of PO', " & _
                                "POM_CURRENCY_CODE AS 'Currency', POD_UNIT_AMT AS 'Total Amount (F)', POD_L_UNIT_AMT AS 'Total PO Amt (RM)' FROM " & _
                                "(SELECT SUM(NO_PR) AS NO_PR, COUNT(POD_PO_NO) AS NO_PO, CM_COY_NAME, POD_OVERSEA, " & _
                                "POM_CURRENCY_CODE, ACCT_CODE, SUM(POD_UNIT_AMT) AS POD_UNIT_AMT, SUM(POD_L_UNIT_AMT) AS POD_L_UNIT_AMT FROM " & _
                                "(SELECT (IFNULL(NO_PR_PO,0) + IFNULL(NO_PR_RFQ,0)) AS NO_PR, POD_PO_NO, POM_S_COY_ID, POD_OVERSEA, " & _
                                "POM_CURRENCY_CODE, POD_ACCT_CODE AS ACCT_CODE, SUM(POD_UNIT_AMT) AS POD_UNIT_AMT, " & _
                                "SUM(POD_UNIT_AMT * POM_EXCHANGE_RATE) AS POD_L_UNIT_AMT FROM " & _
                                "(SELECT POD_PO_NO, POD_VENDOR_ITEM_CODE, POD_PO_LINE, IFNULL(RM_RFQ_NO,'') AS POM_RFQ_NO, POM_S_COY_ID, POD_OVERSEA, POM_CURRENCY_CODE, " & _
                                "IFNULL(AM_ACCT_CODE,'') AS POD_ACCT_CODE, " & _
                                "(POD_UNIT_COST * POD_ORDERED_QTY) + ((POD_UNIT_COST * POD_ORDERED_QTY) / 100 * POD_GST) AS POD_UNIT_AMT, " & _
                                "IFNULL(POM_EXCHANGE_RATE,1) AS POM_EXCHANGE_RATE " & _
                                "FROM PO_DETAILS " & _
                                "INNER JOIN PO_MSTR ON POD_PO_NO = POM_PO_NO AND POM_B_COY_ID = POD_COY_ID " & _
                                "LEFT JOIN RFQ_MSTR ON POM_RFQ_INDEX = RM_RFQ_ID " & _
                                "LEFT JOIN ACCOUNT_MSTR ON POD_ACCT_INDEX = AM_ACCT_INDEX " & _
                                "WHERE POD_ITEM_TYPE = 'SP' AND POD_COY_ID = '" & Session("CompanyID") & "' AND POM_PO_DATE IS NOT NULL " & _
                                "AND POM_PO_DATE >= '" & strBeginDate & "' AND POM_PO_DATE <= '" & strEndDate & "') tb_a " & _
                                "LEFT JOIN " & _
                                "(SELECT COUNT(PRM_PR_NO) AS NO_PR_PO, PRD_CONVERT_TO_DOC FROM " & _
                                "(SELECT PRM_PR_NO, PRM_COY_ID, PRD_CONVERT_TO_DOC FROM PR_MSTR " & _
                                "INNER JOIN PR_DETAILS ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID " & _
                                "WHERE PRM_COY_ID = '" & Session("CompanyID") & "' AND PRD_CONVERT_TO_IND = 'PO' " & _
                                "AND (PRM_PR_STATUS <> '6' AND PRM_PR_STATUS <> '8') " & _
                                "GROUP BY PRM_PR_NO, PRD_CONVERT_TO_DOC) TB_A " & _
                                "GROUP BY PRD_CONVERT_TO_DOC) tb_b " & _
                                "ON tb_a.POD_PO_NO = tb_b.PRD_CONVERT_TO_DOC " & _
                                "LEFT JOIN " & _
                                "(SELECT COUNT(PRM_PR_NO) AS NO_PR_RFQ, PRD_CONVERT_TO_DOC FROM " & _
                                "(SELECT PRM_PR_NO, PRM_COY_ID, PRD_CONVERT_TO_DOC FROM PR_MSTR " & _
                                "INNER JOIN PR_DETAILS ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID " & _
                                "WHERE PRM_COY_ID = '" & Session("CompanyID") & "' AND PRD_CONVERT_TO_IND = 'RFQ' " & _
                                "AND (PRM_PR_STATUS <> '6' AND PRM_PR_STATUS <> '8') " & _
                                "GROUP BY PRM_PR_NO, PRD_CONVERT_TO_DOC) TB_A " & _
                                "GROUP BY PRD_CONVERT_TO_DOC) tb_c " & _
                                "ON tb_a.POM_RFQ_NO = tb_c.PRD_CONVERT_TO_DOC " & _
                                "GROUP BY POD_PO_NO, ACCT_CODE) tb_d " & _
                                "LEFT JOIN COMPANY_MSTR ON CM_COY_ID = POM_S_COY_ID " & _
                                "GROUP BY ACCT_CODE, POM_S_COY_ID, POD_OVERSEA, POM_CURRENCY_CODE " & _
                                "UNION ALL " & _
                                "SELECT COUNT(PRM_PR_NO) AS NO_PR, 0 AS NO_PO, '' AS CM_COY_NAME, PRD_OVERSEA AS POD_OVERSEA, " & _
                                "'' AS POM_CURRENCY_CODE, IFNULL(AM_ACCT_CODE,'') AS ACCT_CODE, 0 AS POD_UNIT_AMT, 0 AS POD_L_UNIT_AMT " & _
                                "FROM PR_MSTR " & _
                                "INNER JOIN PR_DETAILS ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID " & _
                                "LEFT JOIN ACCOUNT_MSTR ON AM_ACCT_INDEX = PRD_ACCT_INDEX " & _
                                "WHERE PRM_COY_ID = '" & Session("CompanyID") & "' AND PRM_SUBMIT_DATE IS NOT NULL " & _
                                "AND PRD_CONVERT_TO_DOC IS NULL AND PRD_ITEM_TYPE = 'SP' " & _
                                "AND (PRM_PR_STATUS <> '6' AND PRM_PR_STATUS <> '8') " & _
                                "AND PRM_SUBMIT_DATE >= '" & strBeginDate & "' AND PRM_SUBMIT_DATE <= '" & strEndDate & "' " & _
                                "GROUP BY ACCT_CODE, POD_OVERSEA) tb " & _
                                "ORDER BY ACCT_CODE, POD_OVERSEA, CM_COY_NAME "

                ElseIf ViewState("type") = "POBAL" Then
                    .CommandText = "SELECT POD_VENDOR_ITEM_CODE AS 'Item Code', POD_PO_NO AS 'PO No', DATE_FORMAT(POM_PO_DATE,'%d/%m/%Y') AS 'PO Date', " & _
                                "POD_EDD AS 'Expected Delivery Date', POD_ORDERED_QTY AS 'Ordered Qty', POD_RECEIVED_QTY AS 'Delivery Qty(Received Qty)', " & _
                                "POD_BAL_QTY AS 'PO Balance Qty', POM_VENDOR_CODE AS 'Vendor Code', POM_CURRENCY_CODE AS 'Currency', POD_UNIT_COST AS 'Unit Price', " & _
                                "POM_DEL_CODE AS 'Delivery Term' FROM (SELECT POD_VENDOR_ITEM_CODE, POD_PO_NO, POM_PO_DATE, " & _
                                "DATE_FORMAT(DATE_ADD(POM_PO_DATE,INTERVAL IFNULL(POD_ETD,0) DAY),'%d/%m/%Y') AS POD_EDD, " & _
                                "POD_ORDERED_QTY, POD_RECEIVED_QTY, (POD_ORDERED_QTY - POD_RECEIVED_QTY) AS POD_BAL_QTY, POM_VENDOR_CODE, " & _
                                "POM_CURRENCY_CODE, POD_UNIT_COST, POM_DEL_CODE, POD_OVERSEA " & _
                                "FROM PO_DETAILS POD, PO_MSTR POM WHERE POD.POD_COY_ID = POM.POM_B_COY_ID " & _
                                "AND POD.POD_PO_NO = POM.POM_PO_NO AND POD_COY_ID = '" & Session("CompanyID") & "' AND POD_ITEM_TYPE = 'ST' " & _
                                "AND POM_PO_DATE IS NOT NULL) AS tb " & _
                                "WHERE POD_OVERSEA = '" & strOversea & "' AND (POM_PO_DATE >= '" & strBeginDate & "' AND POM_PO_DATE <= '" & strEndDate & "') "

                    If Me.cmbPOBal.SelectedValue = "1" Then
                        .CommandText &= "AND POD_BAL_QTY > 0 "
                    ElseIf Me.cmbPOBal.SelectedValue = "0" Then
                        .CommandText &= "AND POD_BAL_QTY = 0 "
                    End If

                    If Me.txtItemCode.Text <> "" Then
                        .CommandText &= "AND POD_VENDOR_ITEM_CODE LIKE '%" & txtItemCode.Text & "%' "
                    End If

                    If Me.txtSuppCode.Text <> "" Then
                        .CommandText &= "AND POM_VENDOR_CODE LIKE '%" & txtSuppCode.Text & "%' "
                    End If

                ElseIf ViewState("type") = "DELTREND" Then
                    .CommandText = "SELECT POD_VENDOR_ITEM_CODE AS 'Item Code', SUM(PAST_6TH_DEL_QTY) AS 'Past 6 Month Delivery Qty', SUM(PAST_5TH_DEL_QTY) AS 'Past 5 Month Delivery Qty', " & _
                                "SUM(PAST_4TH_DEL_QTY) AS 'Past 4 Month Delivery Qty', SUM(PAST_3RD_DEL_QTY) AS 'Past 3 Month Delivery Qty', " & _
                                "SUM(PAST_2ND_DEL_QTY) AS 'Past 2 Month Delivery Qty',SUM(PAST_1ST_DEL_QTY) AS 'Past 1 Month Delivery Qty', " & _
                                "SUM(CURR_MTH_DEL_QTY) AS 'Current Month Delivery Qty', DATE_FORMAT(MAX(DOM_DO_DATE),'%d/%m/%Y') AS 'Last Delivery Date' " & _
                                "FROM (SELECT POD_VENDOR_ITEM_CODE, " & _
                                "CASE WHEN MONTH(DOM_DO_DATE) = MONTH('" & strDate & "' - INTERVAL 6 MONTH) AND YEAR(DOM_DO_DATE) = YEAR('" & strDate & "' - INTERVAL 6 MONTH) " & _
                                "THEN DOD_SHIPPED_QTY ELSE 0 END AS PAST_6TH_DEL_QTY, " & _
                                "CASE WHEN MONTH(DOM_DO_DATE) = MONTH('" & strDate & "' - INTERVAL 5 MONTH) AND YEAR(DOM_DO_DATE) = YEAR('" & strDate & "' - INTERVAL 5 MONTH) " & _
                                "THEN DOD_SHIPPED_QTY ELSE 0 END AS PAST_5TH_DEL_QTY, " & _
                                "CASE WHEN MONTH(DOM_DO_DATE) = MONTH('" & strDate & "' - INTERVAL 4 MONTH) AND YEAR(DOM_DO_DATE) = YEAR('" & strDate & "' - INTERVAL 4 MONTH) " & _
                                "THEN DOD_SHIPPED_QTY ELSE 0 END AS PAST_4TH_DEL_QTY, " & _
                                "CASE WHEN MONTH(DOM_DO_DATE) = MONTH('" & strDate & "' - INTERVAL 3 MONTH) AND YEAR(DOM_DO_DATE) = YEAR('" & strDate & "' - INTERVAL 3 MONTH) " & _
                                "THEN DOD_SHIPPED_QTY ELSE 0 END AS PAST_3RD_DEL_QTY, " & _
                                "CASE WHEN MONTH(DOM_DO_DATE) = MONTH('" & strDate & "' - INTERVAL 2 MONTH) AND YEAR(DOM_DO_DATE) = YEAR('" & strDate & "' - INTERVAL 2 MONTH) " & _
                                "THEN DOD_SHIPPED_QTY ELSE 0 END AS PAST_2ND_DEL_QTY, " & _
                                "CASE WHEN MONTH(DOM_DO_DATE) = MONTH('" & strDate & "' - INTERVAL 1 MONTH) AND YEAR(DOM_DO_DATE) = YEAR('" & strDate & "' - INTERVAL 1 MONTH) " & _
                                "THEN DOD_SHIPPED_QTY ELSE 0 END AS PAST_1ST_DEL_QTY, " & _
                                "CASE WHEN MONTH(DOM_DO_DATE) = MONTH('" & strDate & "') AND YEAR(DOM_DO_DATE) = YEAR('" & strDate & "') " & _
                                "THEN DOD_SHIPPED_QTY ELSE 0 END AS CURR_MTH_DEL_QTY, " & _
                                "DOM_DO_DATE FROM DO_DETAILS " & _
                                "INNER JOIN DO_MSTR ON DOD_S_COY_ID = DOM_S_COY_ID AND DOD_DO_NO = DOM_DO_NO " & _
                                "INNER JOIN PO_MSTR ON DOM_PO_INDEX = POM_PO_INDEX " & _
                                "INNER JOIN PO_DETAILS ON POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = POD_COY_ID AND DOD_PO_LINE = POD_PO_LINE " & _
                                "INNER JOIN PRODUCT_MSTR ON POD_VENDOR_ITEM_CODE = PM_VENDOR_ITEM_CODE AND POD_COY_ID = PM_S_COY_ID " & _
                                "WHERE POM_B_COY_ID = '" & Session("CompanyID") & "' AND PM_ITEM_TYPE = 'ST' AND (DOM_DO_DATE >= '" & strDate2 & "' AND DOM_DO_DATE <= '" & strDate & "')) tb " & _
                                "GROUP BY POD_VENDOR_ITEM_CODE ORDER BY POD_VENDOR_ITEM_CODE "

                ElseIf ViewState("type") = "GRNLIST" Then
                    .CommandText = " SELECT POM_PO_NO AS 'PO Number', DOM_DO_NO AS 'DO Number', GM_INVOICE_NO AS 'Invoice Number', " & _
                                " DATE_FORMAT(DOM_DO_DATE,'%d/%m/%Y') AS 'DO Date', DATE_FORMAT(IM_PAYMENT_DATE,'%d/%m/%Y') AS 'Invoice Date', GD_RECEIVED_QTY AS 'GRN Quantity', " & _
                                " GM_GRN_NO AS 'GRN Number', IM_S_COY_NAME AS 'Vendor Name' FROM " & _
                                " (SELECT POM_PO_NO, SUM(GD_RECEIVED_QTY - GD_REJECTED_QTY) AS GD_RECEIVED_QTY, GM_GRN_NO, " & _
                                " GM_INVOICE_NO, GM_S_COY_ID, GM_DO_INDEX FROM GRN_MSTR " & _
                                " INNER JOIN GRN_DETAILS ON GM_GRN_NO = GD_GRN_NO AND GM_B_COY_ID = GD_B_COY_ID " & _
                                " INNER JOIN PO_MSTR ON GM_PO_INDEX = POM_PO_INDEX " & _
                                " INNER JOIN PO_DETAILS ON POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = POD_COY_ID AND GD_PO_LINE = POD_PO_LINE " & _
                                " INNER JOIN PRODUCT_MSTR ON POD_VENDOR_ITEM_CODE = PM_VENDOR_ITEM_CODE AND POD_COY_ID = PM_S_COY_ID " & _
                                " WHERE GM_B_COY_ID = '" & Session("CompanyID") & "' AND GM_GRN_STATUS <> '1' AND PM_ITEM_TYPE = 'ST' " & _
                                " AND (GM_CREATED_DATE >= '" & strGRNBeginDate & "' AND GM_CREATED_DATE <= '" & strGRNEndDate & "') " & _
                                " GROUP BY GM_GRN_NO) tb " & _
                                " INNER JOIN INVOICE_MSTR ON GM_INVOICE_NO = IM_INVOICE_NO AND GM_S_COY_ID = IM_S_COY_ID " & _
                                " INNER JOIN DO_MSTR ON GM_DO_INDEX = DOM_DO_INDEX "

                    If Me.dtRadioBtn.SelectedValue = "DO" Then
                        .CommandText &= " WHERE DOM_DO_DATE >= '" & strBeginDate & "' AND DOM_DO_DATE <= '" & strEndDate & "' "
                    ElseIf Me.dtRadioBtn.SelectedValue = "INV" Then
                        .CommandText &= " WHERE IM_PAYMENT_DATE >= '" & strBeginDate & "' AND IM_PAYMENT_DATE <= '" & strEndDate & "' "
                    End If

                    If Me.txtPONo.Text <> "" Then
                        strTemp = Common.BuildWildCard(txtPONo.Text)
                        .CommandText &= " AND POM_PO_NO" & Common.ParseSQL(strTemp)
                    End If

                    If Me.txtSuppName.Text <> "" Then
                        strTemp = Common.BuildWildCard(txtSuppName.Text)
                        .CommandText &= " AND IM_S_COY_NAME" & Common.ParseSQL(strTemp)
                    End If

                    .CommandText &= " ORDER BY POM_PO_NO, DOM_DO_NO, GM_INVOICE_NO "

                ElseIf ViewState("type") = "INDEXBOOK" Then
                    .CommandText = "SELECT PM_ACCT_CODE AS 'Account Code', PV_PUR_SPEC_NO AS 'Purchase Spec No', PM_PRODUCT_DESC AS 'Item Name', " & _
                                "PM_SPEC1 AS 'Item Specification 1', PM_SPEC2 AS 'Item Specification 2', PM_SPEC3 AS 'Item Specification 3', " & _
                                "PM_VENDOR_ITEM_CODE AS 'Item Code', PM_PACKING_QTY AS 'Packing Qty', " & _
                                "PV_SUPP_CODE AS 'Vendor Code', (SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = PV_S_COY_ID) AS 'Supplier Name', " & _
                                "PV_DELIVERY_TERM AS 'Delivery Code', PV_CURR AS 'Currency', PVP_VOLUME_PRICE AS 'Unit Price', " & _
                                "PM_UOM AS 'UOM', PV_LEAD_TIME AS 'Lead Time (Day)', PM_MANUFACTURER AS 'Manufacturer 1', " & _
                                "PM_MANUFACTURER2 AS 'Manufacturer 2', PM_MANUFACTURER3 AS 'Manufacturer 3' " & _
                                "FROM PRODUCT_MSTR " & _
                                "LEFT JOIN " & _
                                "(SELECT PV_PRODUCT_INDEX, PV_VENDOR_TYPE, CASE WHEN PV_VENDOR_TYPE = 'P' THEN '0' ELSE PV_VENDOR_TYPE END AS LINE, PV_DELIVERY_TERM, PV_CURR, " & _
                                "PV_SUPP_CODE, PV_LEAD_TIME, PV_PUR_SPEC_NO, PV_S_COY_ID FROM PIM_VENDOR " & _
                                "WHERE (PV_S_COY_ID IS NOT NULL OR PV_S_COY_ID <> '')) tb " & _
                                "ON PM_PRODUCT_INDEX = PV_PRODUCT_INDEX " & _
                                "LEFT JOIN " & _
                                "(SELECT PVP_PRODUCT_CODE, PVP_VENDOR_TYPE, PVP_VOLUME_PRICE FROM PRODUCT_VOLUME_PRICE " & _
                                "WHERE PVP_VOLUME = '1.00') tb_b " & _
                                "ON PM_PRODUCT_CODE = PVP_PRODUCT_CODE AND PV_VENDOR_TYPE = PVP_VENDOR_TYPE " & _
                                "WHERE PM_S_COY_ID = '" & Session("CompanyID") & "' AND PM_OVERSEA = '" & strOversea & "' AND PM_ITEM_TYPE = 'ST' " & _
                                "ORDER BY PM_ACCT_CODE, PM_VENDOR_ITEM_CODE, CAST(LINE AS UNSIGNED) "

                ElseIf ViewState("type") = "LASTKEYINNUM" Then
                    .CommandText = "SELECT " & _
                                "(SELECT MAX(POM_PO_NO) FROM PO_MSTR WHERE POM_B_COY_ID = '" & Session("CompanyID") & "' AND POM_CREATED_DATE <= '" & strDate & "') AS 'PO Number', " & _
                                "(SELECT MAX(GM_GRN_NO) FROM GRN_MSTR WHERE GM_B_COY_ID = '" & Session("CompanyID") & "' AND GM_CREATED_DATE <= '" & strDate & "') AS 'GRN Number', " & _
                                "(SELECT MAX(IRSM_IRS_NO) FROM INVENTORY_REQUISITION_SLIP_MSTR WHERE IRSM_IRS_COY_ID = '" & Session("CompanyID") & "' AND IRSM_CREATED_DATE <= '" & strDate & "') AS 'MRS Number' "

                ElseIf ViewState("type") = "ISSUETREND" Then
                    .CommandText = "SELECT IM_ITEM_CODE AS 'Item Code', SUM(PAST_6TH_IRSD_QTY) AS 'Past 6 Month Issued Qty', " & _
                                "SUM(PAST_5TH_IRSD_QTY) AS 'Past 5 Month Issued Qty', SUM(PAST_4TH_IRSD_QTY) AS 'Past 4 Month Issued Qty', " & _
                                "SUM(PAST_3RD_IRSD_QTY) AS 'Past 3 Month Issued Qty', SUM(PAST_2ND_IRSD_QTY) AS 'Past 2 Month Issued Qty', " & _
                                "SUM(PAST_1ST_IRSD_QTY) AS 'Past 1 Month Issued Qty', SUM(CURR_MTH_IRSD_QTY) AS 'Current Month Issued Qty', " & _
                                "DATE_FORMAT(MAX(IRSM_IRS_APPROVED_DATE),'%d/%m/%Y') AS 'Last Issued Date' " & _
                                "FROM (SELECT IM_ITEM_CODE, " & _
                                "CASE WHEN MONTH(IRSM_IRS_APPROVED_DATE) = MONTH('" & strDate & "' - INTERVAL 6 MONTH) AND YEAR(IRSM_IRS_APPROVED_DATE) = YEAR('" & strDate & "' - INTERVAL 6 MONTH) " & _
                                "THEN IRSD_QTY ELSE 0 END AS PAST_6TH_IRSD_QTY, " & _
                                "CASE WHEN MONTH(IRSM_IRS_APPROVED_DATE) = MONTH('" & strDate & "' - INTERVAL 5 MONTH) AND YEAR(IRSM_IRS_APPROVED_DATE) = YEAR('" & strDate & "' - INTERVAL 5 MONTH) " & _
                                "THEN IRSD_QTY ELSE 0 END AS PAST_5TH_IRSD_QTY, " & _
                                "CASE WHEN MONTH(IRSM_IRS_APPROVED_DATE) = MONTH('" & strDate & "' - INTERVAL 4 MONTH) AND YEAR(IRSM_IRS_APPROVED_DATE) = YEAR('" & strDate & "' - INTERVAL 4 MONTH) " & _
                                "THEN IRSD_QTY ELSE 0 END AS PAST_4TH_IRSD_QTY, " & _
                                "CASE WHEN MONTH(IRSM_IRS_APPROVED_DATE) = MONTH('" & strDate & "' - INTERVAL 3 MONTH) AND YEAR(IRSM_IRS_APPROVED_DATE) = YEAR('" & strDate & "' - INTERVAL 3 MONTH) " & _
                                "THEN IRSD_QTY ELSE 0 END AS PAST_3RD_IRSD_QTY, " & _
                                "CASE WHEN MONTH(IRSM_IRS_APPROVED_DATE) = MONTH('" & strDate & "' - INTERVAL 2 MONTH) AND YEAR(IRSM_IRS_APPROVED_DATE) = YEAR('" & strDate & "' - INTERVAL 2 MONTH) " & _
                                "THEN IRSD_QTY ELSE 0 END AS PAST_2ND_IRSD_QTY, " & _
                                "CASE WHEN MONTH(IRSM_IRS_APPROVED_DATE) = MONTH('" & strDate & "' - INTERVAL 1 MONTH) AND YEAR(IRSM_IRS_APPROVED_DATE) = YEAR('" & strDate & "' - INTERVAL 1 MONTH) " & _
                                "THEN IRSD_QTY ELSE 0 END AS PAST_1ST_IRSD_QTY, " & _
                                "CASE WHEN MONTH(IRSM_IRS_APPROVED_DATE) = MONTH('" & strDate & "') AND YEAR(IRSM_IRS_APPROVED_DATE) = YEAR('" & strDate & "') THEN IRSD_QTY ELSE 0 END AS CURR_MTH_IRSD_QTY, " & _
                                "IRSM_IRS_APPROVED_DATE FROM INVENTORY_REQUISITION_SLIP_DETAILS " & _
                                "INNER JOIN INVENTORY_REQUISITION_SLIP_MSTR ON IRSD_IRS_COY_ID = IRSM_IRS_COY_ID AND IRSD_IRS_NO = IRSM_IRS_NO " & _
                                "INNER JOIN INVENTORY_MSTR ON IRSD_INVENTORY_INDEX = IM_INVENTORY_INDEX " & _
                                "WHERE IRSD_IRS_COY_ID = '" & Session("CompanyID") & "' AND IRSM_IRS_APPROVED_DATE <= '" & strDate & "' " & _
                                "AND (IRSM_IRS_STATUS = '2' OR IRSM_IRS_STATUS = '3' OR IRSM_IRS_STATUS = '4')) tb " & _
                                "GROUP BY IM_ITEM_CODE ORDER BY IM_ITEM_CODE "

                ElseIf ViewState("type") = "STKSTATUS" Then
                    .CommandText = "SELECT PM_ACCT_CODE AS 'Account Code', PM_VENDOR_ITEM_CODE AS 'Item Code', PM_PRODUCT_DESC AS 'Item Name', " & _
                                "PM_SPEC1 AS 'Specification 1', PM_SPEC2 AS 'Specification 2', PM_SPEC3 AS 'Specification 3', " & _
                                "PM_UOM AS 'UOM', SUM(IC_COST_CLOSE_QTY) AS 'Stock On Hand Quantity', SUM(IC_COST_CLOSE_COST) AS 'Stock On Hand Amount' " & _
                                "FROM INVENTORY_COST, INVENTORY_MSTR, PRODUCT_MSTR " & _
                                "WHERE IC_INVENTORY_INDEX = IM_INVENTORY_INDEX And IM_COY_ID = PM_S_COY_ID And IM_ITEM_CODE = PM_VENDOR_ITEM_CODE " & _
                                "AND IC_COY_ID = '" & Session("CompanyID") & "' AND IC_COST_DATE <= '" & strDate & "' " & _
                                "GROUP BY PM_VENDOR_ITEM_CODE " & _
                                "ORDER BY PM_ACCT_CODE, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC "

                ElseIf ViewState("type") = "STOCKBAL" Then
                    .CommandText = "SELECT PM_ACCT_CODE AS 'Account Code', PM_VENDOR_ITEM_CODE AS 'Item Code', PM_PRODUCT_DESC AS 'Item Name', " & _
                                "SUM(IC_COST_OPEN_QTY) AS 'Opening Qty', SUM(IC_COST_OPEN_COST) AS 'Opening Amt', " & _
                                "SUM(IC_COST_ISSUE_QTY) AS 'Issued Qty', SUM(IC_COST_ISSUE_COST) AS 'Issued Amt', " & _
                                "SUM(IC_COST_DISPOSE_QTY) AS 'Disposed Qty', SUM(IC_COST_DISPOSE_COST) AS 'Disposed Amt', " & _
                                "SUM(IC_COST_CLOSE_QTY) AS 'Balance Qty', SUM(IC_COST_CLOSE_COST) AS 'Balance Amt' FROM " & _
                                "(SELECT PM_ACCT_CODE, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, IC_COST_OPEN_QTY, IC_COST_OPEN_COST, " & _
                                "CASE WHEN IC_INVENTORY_TYPE = 'II' THEN IC_COST_QTY ELSE 0 END AS IC_COST_ISSUE_QTY, " & _
                                "CASE WHEN IC_INVENTORY_TYPE = 'II' THEN IC_COST_COST ELSE 0 END AS IC_COST_ISSUE_COST, " & _
                                "CASE WHEN IC_INVENTORY_TYPE = 'WO' THEN IC_COST_QTY ELSE 0 END AS IC_COST_DISPOSE_QTY, " & _
                                "CASE WHEN IC_INVENTORY_TYPE = 'WO' THEN IC_COST_COST ELSE 0 END AS IC_COST_DISPOSE_COST, " & _
                                "IC_COST_CLOSE_QTY, IC_COST_CLOSE_COST " & _
                                "FROM INVENTORY_COST, INVENTORY_MSTR, PRODUCT_MSTR " & _
                                "WHERE IC_INVENTORY_INDEX = IM_INVENTORY_INDEX " & _
                                "AND IM_COY_ID = PM_S_COY_ID AND IM_ITEM_CODE = PM_VENDOR_ITEM_CODE " & _
                                "AND IC_COY_ID = '" & Session("CompanyID") & "' AND PM_OVERSEA = '" & strOversea & "' " & _
                                "AND IC_COST_DATE >= '" & strBeginDate & "' AND IC_COST_DATE <= '" & strEndDate & "') tb " & _
                                "GROUP BY PM_VENDOR_ITEM_CODE ORDER BY PM_ACCT_CODE, PM_VENDOR_ITEM_CODE "

                ElseIf ViewState("type") = "MRSAPPVD" Then
                    .CommandText = "SELECT DATE_FORMAT(IRSM_IRS_APPROVED_DATE, '%d/%m/%Y') AS 'Approved Date', IRSM_IRS_SECTION AS 'Section Code', IRSM_IRS_NO AS 'MRS Number', PM_VENDOR_ITEM_CODE AS 'Item Code', " & _
                                "PM_PRODUCT_DESC AS 'Item Name', PM_SPEC1 AS 'Specification 1', PM_SPEC2 AS 'Specification 2', PM_SPEC3 AS 'Specification 3', " & _
                                "IRSD_QTY AS 'Change Out Qty', IRSM_IRS_REMARK AS 'Remarks', IRSM_IRS_REQUESTOR_NAME AS 'Requester Name', " & _
                                "DATE_FORMAT(IRSM_IRS_DATE, '%d/%m/%Y') AS 'Request Date' " & _
                                "FROM INVENTORY_REQUISITION_SLIP_DETAILS, INVENTORY_REQUISITION_SLIP_MSTR, INVENTORY_MSTR, PRODUCT_MSTR, USER_MSTR " & _
                                "WHERE IRSD_IRS_COY_ID = IRSM_IRS_COY_ID And IRSD_IRS_NO = IRSM_IRS_NO " & _
                                "AND IRSD_INVENTORY_INDEX = IM_INVENTORY_INDEX " & _
                                "AND IM_COY_ID = PM_S_COY_ID AND IM_ITEM_CODE = PM_VENDOR_ITEM_CODE " & _
                                "AND IM_COY_ID = UM_COY_ID AND IRSM_BUYER_ID = UM_USER_ID " & _
                                "AND IRSD_IRS_COY_ID = '" & Session("CompanyID") & "' " & _
                                "AND IRSM_IRS_APPROVED_DATE >= '" & strBeginDate & "' AND IRSM_IRS_APPROVED_DATE <= '" & strEndDate & "' "

                    If Me.txtSKName.Text <> "" Then
                        strTemp = Common.BuildWildCard(txtSKName.Text)
                        .CommandText &= " AND UM_USER_NAME " & Common.ParseSQL(strTemp)
                    End If

                ElseIf ViewState("type") = "MRSLIST" Then
                    .CommandText = " SELECT IM_ITEM_CODE AS 'Item Code', IRSD_IRS_NO AS 'MRS Number', DATE_FORMAT(IRSM_IRS_APPROVED_DATE, '%d/%m/%Y') AS 'Issue Date', IRSM_IRS_SECTION AS 'Section Code', " & _
                                " CS_SEC_NAME AS 'Section Name', IRSD_QTY AS 'Issue Quantity', IC_COST_CLOSE_COST AS 'Unit Cost' FROM INVENTORY_REQUISITION_SLIP_MSTR " & _
                                " INNER JOIN INVENTORY_REQUISITION_SLIP_DETAILS ON IRSM_IRS_NO = IRSD_IRS_NO AND IRSM_IRS_COY_ID = IRSD_IRS_COY_ID " & _
                                " INNER JOIN INVENTORY_MSTR ON IRSD_INVENTORY_INDEX  = IM_INVENTORY_INDEX " & _
                                " INNER JOIN COMPANY_SECTION ON IRSM_IRS_COY_ID = CS_COY_ID AND IRSM_IRS_SECTION = CS_SEC_CODE " & _
                                " LEFT JOIN INVENTORY_COST ON IRSD_IRS_COY_ID = IC_COY_ID AND IRSD_INVENTORY_INDEX = IC_INVENTORY_INDEX " & _
                                " AND IRSD_IRS_NO = IC_INVENTORY_REF_DOC AND IC_INVENTORY_TYPE = 'II' " & _
                                " WHERE (IRSM_IRS_STATUS = '2' OR IRSM_IRS_STATUS = '3') AND IRSD_IRS_COY_ID ='" & Session("CompanyID") & "' AND IRSM_IRS_APPROVED_DATE >= '" & strBeginDate & "' " & _
                                " AND IRSM_IRS_APPROVED_DATE <= '" & strEndDate & "' "

                    If Me.txtItemCode2.Text <> "" Then
                        strTemp = Common.BuildWildCard(txtItemCode2.Text)
                        .CommandText &= " AND IM_ITEM_CODE " & Common.ParseSQL(strTemp)
                    End If

                    If Me.txtSectionCode.Text <> "" Then
                        strTemp = Common.BuildWildCard(txtSectionCode.Text)
                        .CommandText &= " AND IRSM_IRS_SECTION " & Common.ParseSQL(strTemp)
                    End If

                    .CommandText &= " ORDER BY IM_ITEM_CODE, IRSD_IRS_NO "

                ElseIf ViewState("type") = "SUMMTHSTKBAL" Then
                    .CommandText = "SELECT PM_ACCT_CODE AS 'Account Code', CT_NAME AS 'Commodity Type', SUM(IC_COST_OPEN_COST) AS 'Opening Balance', " & _
                                "SUM(IC_GRN_COST) AS 'Received (+)', SUM(IC_II_COST) AS 'Issued (-)', SUM(IC_WO_COST) AS 'Write Off (-)', " & _
                                "SUM(IC_COST_CLOSE_COST) AS 'Closing Balance', SUM(PO_VALUE) AS 'PO Order Value' " & _
                                "FROM " & _
                                "(SELECT PM_ACCT_CODE, CT_NAME, IC_COST_OPEN_COST, " & _
                                "CASE IC_INVENTORY_TYPE WHEN 'GRN' THEN IC_COST_COST ELSE 0.00 END AS IC_GRN_COST, " & _
                                "CASE IC_INVENTORY_TYPE WHEN 'II' THEN IC_COST_COST ELSE 0.00 END AS IC_II_COST, " & _
                                "CASE IC_INVENTORY_TYPE WHEN 'WO' THEN IC_COST_COST ELSE 0.00 END AS IC_WO_COST, " & _
                                "IC_COST_CLOSE_COST, IFNULL(PO_VALUE,0) AS PO_VALUE " & _
                                "FROM INVENTORY_COST " & _
                                "INNER JOIN INVENTORY_MSTR ON IC_INVENTORY_INDEX = IM_INVENTORY_INDEX " & _
                                "INNER JOIN PRODUCT_MSTR ON IM_COY_ID = PM_S_COY_ID AND IM_ITEM_CODE = PM_VENDOR_ITEM_CODE " & _
                                "INNER JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID " & _
                                "LEFT JOIN(SELECT SUM(POD_ORDERED_QTY - (POD_RECEIVED_QTY - POD_REJECTED_QTY)) AS PO_VALUE, " & _
                                "POD_VENDOR_ITEM_CODE, POD_COY_ID " & _
                                "FROM PO_DETAILS " & _
                                "INNER JOIN PO_MSTR ON POD_COY_ID = POM_B_COY_ID AND POD_PO_NO = POM_PO_NO " & _
                                "WHERE (POM_PO_STATUS <> 4 And POM_PO_STATUS <> 5) " & _
                                "AND YEAR(POM_PO_DATE) = '" & strYear & "' AND MONTH(POM_PO_DATE) = '" & strMonth & "' " & _
                                "AND POD_COY_ID = '" & Session("CompanyID") & "' " & _
                                "GROUP BY POD_VENDOR_ITEM_CODE) tb_b " & _
                                "ON PM_VENDOR_ITEM_CODE = tb_b.POD_VENDOR_ITEM_CODE AND PM_S_COY_ID = tb_b.POD_COY_ID " & _
                                "WHERE YEAR(IC_COST_DATE) = '" & strYear & "' AND MONTH(IC_COST_DATE) = '" & strMonth & "' " & _
                                "AND IC_COY_ID = '" & Session("CompanyID") & "' AND PM_OVERSEA = '" & strOversea & "') tb_c " & _
                                "GROUP BY PM_ACCT_CODE, CT_NAME ORDER BY PM_ACCT_CODE, CT_NAME "

                ElseIf ViewState("type") = "SMFA0612MTH" Then
                    .CommandText = "SELECT PM_ACCT_CODE AS 'Account Code', DATE_FORMAT(IRSM_IRS_DATE, '%d/%m/%Y') AS 'Last Issue Date', IM_ITEM_CODE AS 'Item Code', IM_INVENTORY_NAME AS 'Item Name', " & _
                                "CONCAT(PM_SPEC1,' ',PM_SPEC2,' ',PM_SPEC3) AS 'Specification', " & _
                                "IFNULL(IC_COST_CLOSE_QTY,0.00) AS 'Stock Balance Quantity', IFNULL(IC_COST_CLOSE_COST,0.00) AS 'Stock Amount', " & _
                                "IRSM_IRS_SECTION AS 'Last Section Issued', CS_SEC_NAME AS 'Section Name' " & _
                                "FROM INVENTORY_MSTR " & _
                                "LEFT JOIN " & _
                                "(SELECT IRSD_INVENTORY_INDEX, IRSM_IRS_DATE, IRSM_IRS_SECTION, CS_SEC_NAME " & _
                                "FROM INVENTORY_REQUISITION_SLIP_DETAILS " & _
                                "INNER JOIN INVENTORY_REQUISITION_SLIP_MSTR ON IRSD_IRS_COY_ID = IRSM_IRS_COY_ID AND IRSD_IRS_NO = IRSM_IRS_NO " & _
                                "INNER JOIN COMPANY_SECTION ON CS_SEC_CODE = IRSM_IRS_SECTION AND IRSM_IRS_COY_ID = CS_COY_ID " & _
                                "INNER JOIN " & _
                                "(SELECT MAX(IRSM_IRS_INDEX) AS ID, IRSD_INVENTORY_INDEX AS ITEM_ID FROM INVENTORY_REQUISITION_SLIP_DETAILS " & _
                                "INNER JOIN INVENTORY_REQUISITION_SLIP_MSTR ON IRSD_IRS_COY_ID = IRSM_IRS_COY_ID AND IRSD_IRS_NO = IRSM_IRS_NO " & _
                                "WHERE IRSM_IRS_COY_ID = '" & Session("CompanyID") & "' AND IRSM_IRS_DATE <= DATE_ADD('" & strDate & "', INTERVAL -6 MONTH) " & _
                                "AND (IRSM_IRS_STATUS = '3' OR IRSM_IRS_STATUS = '4') GROUP BY IRSD_INVENTORY_INDEX) tb_b " & _
                                "ON IRSM_IRS_INDEX = ID AND IRSD_INVENTORY_INDEX = ITEM_ID) tb_b " & _
                                "ON IM_INVENTORY_INDEX = tb_b.IRSD_INVENTORY_INDEX " & _
                                "LEFT JOIN " & _
                                "(SELECT IC_INVENTORY_INDEX, IC_COST_DATE AS IC_COST_DATE, IC_COST_CLOSE_QTY, IC_COST_CLOSE_COST " & _
                                "FROM INVENTORY_COST " & _
                                "INNER JOIN " & _
                                "(SELECT MAX(IC_COST_INDEX) AS IC_INDEX, IC_INVENTORY_INDEX AS IC_ITEM " & _
                                "FROM INVENTORY_COST " & _
                                "WHERE IC_COY_ID = '" & Session("CompanyID") & "' AND IC_COST_DATE <= DATE_ADD('" & strDate & "', INTERVAL -6 MONTH) " & _
                                "AND IC_INVENTORY_TYPE = 'II' GROUP BY IC_INVENTORY_INDEX) tb_b " & _
                                "ON IC_COST_INDEX = IC_INDEX AND IC_INVENTORY_INDEX = IC_ITEM) tb_c " & _
                                "ON IC_INVENTORY_INDEX = IM_INVENTORY_INDEX " & _
                                "INNER JOIN PRODUCT_MSTR ON IM_ITEM_CODE = PM_VENDOR_ITEM_CODE AND IM_COY_ID = PM_S_COY_ID " & _
                                "WHERE IM_INVENTORY_INDEX NOT IN " & _
                                "(SELECT IRSD_INVENTORY_INDEX FROM INVENTORY_REQUISITION_SLIP_MSTR, INVENTORY_REQUISITION_SLIP_DETAILS " & _
                                "WHERE IRSM_IRS_NO = IRSD_IRS_NO And IRSM_IRS_COY_ID = IRSD_IRS_COY_ID " & _
                                "AND (IRSM_IRS_STATUS = '3' OR IRSM_IRS_STATUS = '4') AND IRSM_IRS_COY_ID = '" & Session("CompanyID") & "' " & _
                                "AND (IRSM_IRS_DATE BETWEEN DATE_ADD('" & strDate & "', INTERVAL -6 MONTH) AND '" & strDate & "')) " & _
                                "AND IM_COY_ID = '" & Session("CompanyID") & "' AND PM_OVERSEA = '" & strOversea & "' " & _
                                "ORDER BY PM_ACCT_CODE, IM_ITEM_CODE "

                ElseIf ViewState("type") = "DEADSTK" Then
                    .CommandText = "SELECT PM_ACCT_CODE AS 'Account Code', DATE_FORMAT(IRSM_IRS_DATE,'%d/%m/%Y') AS 'Last Issue Date', IM_ITEM_CODE AS 'Item Code', IM_INVENTORY_NAME AS 'Item Name', " & _
                                "CONCAT(PM_SPEC1,' ',PM_SPEC2,' ',PM_SPEC3) AS 'Specification', " & _
                                "IC_COST_CLOSE_QTY AS 'Stock Balance Quantity', IC_COST_CLOSE_COST AS 'Stock Amount', IRSM_IRS_SECTION AS 'Last Section Issue', CS_SEC_NAME AS 'Section Name' FROM INVENTORY_MSTR " & _
                                "LEFT JOIN " & _
                                "(SELECT IRSD_INVENTORY_INDEX, MAX(IRSM_IRS_DATE) AS IRSM_IRS_DATE, IRSM_IRS_SECTION, CS_SEC_NAME FROM INVENTORY_REQUISITION_SLIP_DETAILS " & _
                                "INNER JOIN INVENTORY_REQUISITION_SLIP_MSTR ON IRSM_IRS_COY_ID = IRSD_IRS_COY_ID AND IRSD_IRS_NO=IRSM_IRS_NO " & _
                                "INNER JOIN COMPANY_SECTION ON CS_SEC_CODE = IRSM_IRS_SECTION AND IRSM_IRS_COY_ID= CS_COY_ID " & _
                                "WHERE IRSM_IRS_DATE <= DATE_ADD('" & strDate & "', INTERVAL -12 MONTH) AND IRSM_IRS_COY_ID = '" & Session("CompanyID") & "' " & _
                                "GROUP BY IRSD_INVENTORY_INDEX) tb_b " & _
                                "ON IM_INVENTORY_INDEX = tb_b.IRSD_INVENTORY_INDEX " & _
                                "LEFT JOIN " & _
                                "(SELECT IC_INVENTORY_INDEX, MAX(IC_COST_DATE) AS IC_COST_DATE, IC_COST_CLOSE_QTY, IC_COST_CLOSE_COST " & _
                                "FROM INVENTORY_COST " & _
                                "INNER JOIN INVENTORY_REQUISITION_SLIP_MSTR ON IC_COY_ID = IRSM_IRS_COY_ID AND IC_INVENTORY_REF_DOC = IRSM_IRS_NO " & _
                                "INNER JOIN INVENTORY_REQUISITION_SLIP_DETAILS ON IRSM_IRS_COY_ID = IRSD_IRS_COY_ID AND IRSM_IRS_NO = IRSD_IRS_NO " & _
                                "WHERE IC_COST_CLOSE_UPRICE <= DATE_ADD('" & strDate & "', INTERVAL -12 MONTH) AND IC_COY_ID = '" & Session("CompanyID") & "' " & _
                                "GROUP BY IC_INVENTORY_INDEX) tb_c " & _
                                "ON IC_INVENTORY_INDEX = IM_INVENTORY_INDEX " & _
                                "INNER JOIN PRODUCT_MSTR ON IM_ITEM_CODE = PM_VENDOR_ITEM_CODE AND IM_COY_ID = PM_S_COY_ID " & _
                                "WHERE IM_INVENTORY_INDEX NOT IN " & _
                                "(SELECT DISTINCT IRSD_INVENTORY_INDEX FROM INVENTORY_REQUISITION_SLIP_MSTR " & _
                                "INNER JOIN INVENTORY_REQUISITION_SLIP_DETAILS ON  IRSM_IRS_NO = IRSD_IRS_NO AND IRSM_IRS_COY_ID = IRSD_IRS_COY_ID " & _
                                "WHERE IRSM_IRS_COY_ID = '" & Session("CompanyID") & "' AND DATE_ADD('" & strDate & "', INTERVAL -12 MONTH) <= IRSM_IRS_DATE AND '" & strDate & "' >= IRSM_IRS_DATE) " & _
                                "AND IM_COY_ID = '" & Session("CompanyID") & "' AND PM_OVERSEA = '" & strOversea & "' " & _
                                "ORDER BY PM_ACCT_CODE, IM_ITEM_CODE "

                ElseIf ViewState("type") = "MTHMGMT" Then
                    strTemp = "SELECT SUM(IC_COST_CLOSE_COST) " & _
                                "FROM INVENTORY_MSTR " & _
                                "INNER JOIN INVENTORY_COST ON IC_INVENTORY_INDEX = IM_INVENTORY_INDEX " & _
                                "INNER JOIN PRODUCT_MSTR ON IM_COY_ID = PM_S_COY_ID AND IM_ITEM_CODE = PM_VENDOR_ITEM_CODE " & _
                                "INNER JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID WHERE IM_COY_ID = '" & Session("CompanyId") & "' AND PM_OVERSEA = '" & cmbOversea.SelectedValue & "' " & _
                                "AND MONTH(IC_COST_DATE) = MONTH('" & strDate & "') AND YEAR(IC_COST_DATE) = YEAR('" & strDate & "') "
                    decClosingBal = objDb.GetVal(strTemp)

                    strTemp = "SELECT SUM(1) FROM INVENTORY_MSTR " & _
                            "INNER JOIN PRODUCT_MSTR ON IM_ITEM_CODE = PM_VENDOR_ITEM_CODE AND IM_COY_ID = PM_S_COY_ID " & _
                            "INNER JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID " & _
                            "INNER JOIN (SELECT PM_ACCT_CODE AS ACCT_CODE, CT_NAME AS CT FROM " & _
                            "INVENTORY_MSTR " & _
                            "INNER JOIN INVENTORY_COST ON IC_INVENTORY_INDEX = IM_INVENTORY_INDEX " & _
                            "INNER JOIN PRODUCT_MSTR ON IM_COY_ID = PM_S_COY_ID AND IM_ITEM_CODE = PM_VENDOR_ITEM_CODE " & _
                            "INNER JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID WHERE IM_COY_ID = '" & Session("CompanyId") & "' AND PM_OVERSEA = '" & cmbOversea.SelectedValue & "' " & _
                            "AND MONTH(IC_COST_DATE) = MONTH('" & strDate & "') AND YEAR(IC_COST_DATE) = YEAR('" & strDate & "') " & _
                            "AND (IC_INVENTORY_TYPE = 'WO' OR IC_INVENTORY_TYPE='GRN' OR IC_INVENTORY_TYPE='II') " & _
                            "GROUP BY PM_ACCT_CODE, CT_NAME) tb " & _
                            "ON PM_ACCT_CODE = ACCT_CODE AND CT_NAME = CT " & _
                            "WHERE IM_INVENTORY_INDEX IN " & _
                            "(SELECT IRSD_INVENTORY_INDEX  FROM INVENTORY_REQUISITION_SLIP_MSTR " & _
                            "INNER JOIN INVENTORY_REQUISITION_SLIP_DETAILS ON IRSM_IRS_NO = IRSD_IRS_NO AND IRSM_IRS_COY_ID = IRSD_IRS_COY_ID " & _
                            "WHERE MONTH(IRSM_IRS_DATE) BETWEEN MONTH('" & strDate & "' - INTERVAL 12 MONTH) AND MONTH('" & strDate & "') " & _
                            "AND YEAR(IRSM_IRS_DATE) BETWEEN YEAR('" & strDate & "' - INTERVAL 12 MONTH) AND YEAR('" & strDate & "')) " & _
                            "AND IM_COY_ID = '" & Session("CompanyId") & "' AND PM_OVERSEA = '" & cmbOversea.SelectedValue & "' "
                    intActive = objDb.GetVal(strTemp)

                    .CommandText = "SELECT tb_a.PM_ACCT_CODE AS 'Account Code', tb_a.CT_NAME AS 'Commodity Type', NOOFITEM AS 'No.Of Items', IC_COST_OPEN_COST AS 'Opening Balance', " & _
                                "RECEIVED AS 'Received (+)', ISSUED AS 'Issued (-)', WRITEOFF AS 'Write Off (-)', IC_COST_CLOSE_COST AS 'Closing Balance', " & _
                                "CAST((IC_COST_CLOSE_COST / " & decClosingBal & " * 100) AS DECIMAL(4,2)) AS 'Percent of Closing Balance', " & _
                                "IFNULL(SLOWMOVING,0) AS 'Slow Moving', IFNULL(DEADSTOCK,0) AS 'Dead Stock', IFNULL(ACTIVESTOCK,0) AS 'Active Stock', " & _
                                "CAST(IFNULL((ACTIVESTOCK / " & intActive & " * 100),0) AS DECIMAL(4,2)) AS 'Percent of Active Stock' FROM " & _
                                "(SELECT PM_ACCT_CODE, CT_NAME, COUNT(IM_ITEM_CODE) AS NOOFITEM, SUM(IC_COST_OPEN_COST) AS IC_COST_OPEN_COST, " & _
                                "SUM(IC_COST_CLOSE_COST) AS IC_COST_CLOSE_COST, SUM(RECEIVED) AS RECEIVED, SUM(WRITEOFF) AS WRITEOFF, SUM(ISSUED) AS ISSUED " & _
                                "FROM " & _
                                "(SELECT PM_ACCT_CODE, CT_NAME, IM_ITEM_CODE, IC_COST_OPEN_COST, IC_COST_CLOSE_COST, " & _
                                "CASE WHEN IC_INVENTORY_TYPE='GRN' THEN IC_COST_COST ELSE 0 END AS RECEIVED, " & _
                                "CASE WHEN IC_INVENTORY_TYPE='WO' THEN IC_COST_COST ELSE 0 END AS WRITEOFF, " & _
                                "CASE WHEN IC_INVENTORY_TYPE='II' THEN IC_COST_COST ELSE 0 END AS ISSUED " & _
                                "FROM INVENTORY_MSTR " & _
                                "INNER JOIN INVENTORY_COST ON IC_INVENTORY_INDEX = IM_INVENTORY_INDEX " & _
                                "INNER JOIN PRODUCT_MSTR ON IM_COY_ID = PM_S_COY_ID AND IM_ITEM_CODE = PM_VENDOR_ITEM_CODE " & _
                                "INNER JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID WHERE IM_COY_ID = '" & Session("CompanyId") & "' AND PM_OVERSEA = '" & cmbOversea.SelectedValue & "' " & _
                                "AND (IC_INVENTORY_TYPE = 'WO' OR IC_INVENTORY_TYPE='GRN' OR IC_INVENTORY_TYPE='II') " & _
                                "AND MONTH(IC_COST_DATE) = MONTH('" & strDate & "') AND YEAR(IC_COST_DATE) = YEAR('" & strDate & "')) tb " & _
                                "GROUP BY PM_ACCT_CODE, CT_NAME) tb_a " & _
                                "LEFT JOIN " & _
                                "(SELECT PM_ACCT_CODE, CT_NAME, SUM(1) AS SLOWMOVING FROM INVENTORY_MSTR " & _
                                "INNER JOIN PRODUCT_MSTR ON IM_ITEM_CODE = PM_VENDOR_ITEM_CODE AND IM_COY_ID = PM_S_COY_ID " & _
                                "INNER JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID " & _
                                "WHERE IM_INVENTORY_INDEX NOT IN " & _
                                "(SELECT IRSD_INVENTORY_INDEX FROM INVENTORY_REQUISITION_SLIP_MSTR " & _
                                "INNER JOIN INVENTORY_REQUISITION_SLIP_DETAILS ON IRSM_IRS_NO = IRSD_IRS_NO AND IRSM_IRS_COY_ID = IRSD_IRS_COY_ID " & _
                                "WHERE (MONTH(IRSM_IRS_DATE) BETWEEN MONTH('" & strDate & "' - INTERVAL 6 MONTH) AND MONTH('" & strDate & "')) " & _
                                "AND (YEAR(IRSM_IRS_DATE) BETWEEN YEAR('" & strDate & "' - INTERVAL 6 MONTH) AND YEAR('" & strDate & "'))) " & _
                                "AND IM_COY_ID = '" & Session("CompanyId") & "' AND PM_OVERSEA = '" & cmbOversea.SelectedValue & "' " & _
                                "GROUP BY PM_ACCT_CODE, CT_NAME) tb_b " & _
                                "ON tb_a.PM_ACCT_CODE = tb_b.PM_ACCT_CODE AND tb_a.CT_NAME = tb_b.CT_NAME " & _
                                "LEFT JOIN " & _
                                "(SELECT PM_ACCT_CODE, CT_NAME, SUM(1) AS DEADSTOCK FROM INVENTORY_MSTR " & _
                                "INNER JOIN PRODUCT_MSTR ON IM_ITEM_CODE = PM_VENDOR_ITEM_CODE AND IM_COY_ID = PM_S_COY_ID " & _
                                "INNER JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID " & _
                                "WHERE IM_INVENTORY_INDEX NOT IN " & _
                                "(SELECT DISTINCT IRSD_INVENTORY_INDEX  FROM INVENTORY_REQUISITION_SLIP_MSTR " & _
                                "INNER JOIN INVENTORY_REQUISITION_SLIP_DETAILS ON IRSM_IRS_NO = IRSD_IRS_NO AND IRSM_IRS_COY_ID = IRSD_IRS_COY_ID " & _
                                "WHERE (MONTH(IRSM_IRS_DATE) BETWEEN MONTH('" & strDate & "' - INTERVAL 12 MONTH) AND MONTH('" & strDate & "')) " & _
                                "AND (YEAR(IRSM_IRS_DATE) BETWEEN YEAR('" & strDate & "' - INTERVAL 12 MONTH) AND YEAR('" & strDate & "'))) " & _
                                "AND IM_COY_ID = '" & Session("CompanyId") & "' AND PM_OVERSEA = '" & cmbOversea.SelectedValue & "' " & _
                                "GROUP BY PM_ACCT_CODE, CT_NAME) tb_c " & _
                                "ON tb_a.PM_ACCT_CODE = tb_c.PM_ACCT_CODE AND tb_a.CT_NAME = tb_c.CT_NAME " & _
                                "LEFT JOIN " & _
                                "(SELECT PM_ACCT_CODE, CT_NAME, SUM(1) AS ACTIVESTOCK FROM INVENTORY_MSTR " & _
                                "INNER JOIN PRODUCT_MSTR ON IM_ITEM_CODE = PM_VENDOR_ITEM_CODE AND IM_COY_ID = PM_S_COY_ID " & _
                                "INNER JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID " & _
                                "WHERE IM_INVENTORY_INDEX IN " & _
                                "(SELECT IRSD_INVENTORY_INDEX  FROM INVENTORY_REQUISITION_SLIP_MSTR " & _
                                "INNER JOIN INVENTORY_REQUISITION_SLIP_DETAILS ON IRSM_IRS_NO = IRSD_IRS_NO AND IRSM_IRS_COY_ID = IRSD_IRS_COY_ID " & _
                                "WHERE MONTH(IRSM_IRS_DATE) BETWEEN MONTH('" & strDate & "' - INTERVAL 12 MONTH) AND MONTH('" & strDate & "') " & _
                                "AND YEAR(IRSM_IRS_DATE) BETWEEN YEAR('" & strDate & "' - INTERVAL 12 MONTH) AND YEAR('" & strDate & "')) " & _
                                "AND IM_COY_ID = '" & Session("CompanyId") & "' AND PM_OVERSEA = '" & cmbOversea.SelectedValue & "' " & _
                                "GROUP BY PM_ACCT_CODE, CT_NAME) tb_d " & _
                                "ON tb_a.PM_ACCT_CODE = tb_d.PM_ACCT_CODE AND tb_a.CT_NAME = tb_d.CT_NAME "

                    '.CommandText = "SELECT PM_ACCT_CODE AS 'Account Code', CT_NAME AS 'Commodity Type', NO_ITEM AS 'No.Of Items', OPENINGBALANCE AS 'Opening Balance', " & _
                    '            "RECEIVED AS 'Received (+)', ISSUED AS 'Issued (-)', WRITEOFF AS 'Write Off (-)', CLOSINGBALANCE AS 'Closing Balance', " & _
                    '            "(SUM(CLOSINGBALANCE) / CLOSINGBALANCE) AS 'Percent of Closing Balance', SLOWMOVING AS 'Slow Moving', DEADSTOCK AS 'Dead Stocks', " & _
                    '            "ACTIVESTOCK AS 'Active Stocks', (SUM(ACTIVESTOCK) / ACTIVESTOCK) AS 'Percent of Active Stock' " & _
                    '            "FROM (SELECT PM_ACCT_CODE, CT_NAME, COUNT(tb_a.IM_ITEM_CODE) AS NO_ITEM, SUM(IC_COST_OPEN_COST) AS OPENINGBALANCE, " & _
                    '            "SUM(IC_COST_CLOSE_COST) AS CLOSINGBALANCE, SUM(RECEIVED) AS RECEIVED, SUM(WRITEOFF) AS WRITEOFF, " & _
                    '            "SUM(ISSUED) AS ISSUED, SUM(SLOWMOVING) AS SLOWMOVING, SUM(DEADSTOCK) AS DEADSTOCK, " & _
                    '            "SUM(IFNULL(ACTIVESTOCK,0)) AS ACTIVESTOCK " & _
                    '            "FROM (SELECT PM_ACCT_CODE, CT_NAME, IM_ITEM_CODE, IC_COST_OPEN_COST, IC_COST_CLOSE_COST, " & _
                    '            "CASE WHEN IC_INVENTORY_TYPE='GRN' THEN IC_COST_COST ELSE 0 END AS 'RECEIVED', " & _
                    '            "CASE WHEN IC_INVENTORY_TYPE='WO' THEN IC_COST_COST ELSE 0 END AS 'WRITEOFF', " & _
                    '            "CASE WHEN IC_INVENTORY_TYPE='II' THEN IC_COST_COST ELSE 0 END AS 'ISSUED' FROM inventory_mstr " & _
                    '            "INNER JOIN INVENTORY_COST ON IC_INVENTORY_INDEX = IM_INVENTORY_INDEX " & _
                    '            "INNER JOIN PRODUCT_MSTR ON IM_COY_ID = PM_S_COY_ID AND IM_ITEM_CODE = PM_VENDOR_ITEM_CODE " & _
                    '            "INNER JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID WHERE IM_COY_ID = '" & Session("CompanyID") & "' AND PM_OVERSEA = '" & strOversea & "' " & _
                    '            "AND MONTH(IC_COST_DATE) = MONTH('" & strDate & "') AND YEAR(IC_COST_DATE) = YEAR('" & strDate & "')) tb_a " & _
                    '            "LEFT JOIN " & _
                    '            "(SELECT IM_ITEM_CODE, 1 AS SLOWMOVING FROM INVENTORY_MSTR " & _
                    '            "INNER JOIN PRODUCT_MSTR ON IM_ITEM_CODE = PM_VENDOR_ITEM_CODE AND IM_COY_ID = PM_S_COY_ID " & _
                    '            "WHERE IM_INVENTORY_INDEX NOT IN " & _
                    '            "(SELECT IRSD_INVENTORY_INDEX FROM INVENTORY_REQUISITION_SLIP_MSTR " & _
                    '            "INNER JOIN INVENTORY_REQUISITION_SLIP_DETAILS ON IRSM_IRS_NO = IRSD_IRS_NO AND IRSM_IRS_COY_ID = IRSD_IRS_COY_ID " & _
                    '            "WHERE (MONTH(IRSM_IRS_DATE) BETWEEN MONTH('" & strDate & "' - INTERVAL 12 MONTH) AND MONTH('" & strDate & "' - INTERVAL 12 MONTH)) " & _
                    '            "AND (YEAR(IRSM_IRS_DATE) BETWEEN YEAR('" & strDate & "' - INTERVAL 12 MONTH) AND YEAR('" & strDate & "' - INTERVAL 12 MONTH))) " & _
                    '            "AND IM_COY_ID = '" & Session("CompanyID") & "' AND PM_OVERSEA = '" & strOversea & "') tb_b " & _
                    '            "ON tb_a.IM_ITEM_CODE = tb_b.IM_ITEM_CODE " & _
                    '            "LEFT JOIN " & _
                    '            "(SELECT IM_ITEM_CODE, 1 AS DEADSTOCK FROM INVENTORY_MSTR " & _
                    '            "INNER JOIN PRODUCT_MSTR ON IM_ITEM_CODE = PM_VENDOR_ITEM_CODE AND IM_COY_ID = PM_S_COY_ID " & _
                    '            "WHERE IM_INVENTORY_INDEX NOT IN " & _
                    '            "(SELECT DISTINCT IRSD_INVENTORY_INDEX  FROM INVENTORY_REQUISITION_SLIP_MSTR " & _
                    '            "INNER JOIN INVENTORY_REQUISITION_SLIP_DETAILS ON IRSM_IRS_NO = IRSD_IRS_NO AND IRSM_IRS_COY_ID = IRSD_IRS_COY_ID " & _
                    '            "WHERE (MONTH(IRSM_IRS_DATE) BETWEEN MONTH('" & strDate & "' - INTERVAL 12 MONTH) AND MONTH('" & strDate & "' - INTERVAL 12 MONTH)) " & _
                    '            "AND (YEAR(IRSM_IRS_DATE) BETWEEN YEAR('" & strDate & "' - INTERVAL 12 MONTH) AND YEAR('" & strDate & "' - INTERVAL 12 MONTH))) " & _
                    '            "AND IM_COY_ID ='" & Session("CompanyID") & "' AND PM_OVERSEA = '" & strOversea & "') tb_c " & _
                    '            "ON tb_a.IM_ITEM_CODE = tb_c.IM_ITEM_CODE " & _
                    '            "LEFT JOIN " & _
                    '            "(SELECT IM_ITEM_CODE, 1 AS ACTIVESTOCK FROM INVENTORY_MSTR " & _
                    '            "INNER JOIN PRODUCT_MSTR ON IM_ITEM_CODE = PM_VENDOR_ITEM_CODE " & _
                    '            "AND IM_COY_ID = PM_S_COY_ID WHERE IM_INVENTORY_INDEX IN " & _
                    '            "(SELECT IRSD_INVENTORY_INDEX  FROM INVENTORY_REQUISITION_SLIP_MSTR " & _
                    '            "INNER JOIN INVENTORY_REQUISITION_SLIP_DETAILS ON IRSM_IRS_NO = IRSD_IRS_NO AND IRSM_IRS_COY_ID = IRSD_IRS_COY_ID " & _
                    '            "WHERE NOT MONTH(IRSM_IRS_DATE) BETWEEN MONTH('" & strDate & "' - INTERVAL 12 MONTH) AND MONTH('" & strDate & "') " & _
                    '            "AND NOT YEAR(IRSM_IRS_DATE) BETWEEN YEAR('" & strDate & "' - INTERVAL 12 MONTH) AND YEAR('" & strDate & "')) " & _
                    '            "AND IM_COY_ID = '" & Session("CompanyID") & "' AND PM_OVERSEA = '" & strOversea & "') tb_d " & _
                    '            "ON tb_a.IM_ITEM_CODE = tb_d.IM_ITEM_CODE " & _
                    '            "GROUP BY PM_ACCT_CODE, CT_NAME) tb_e "

                ElseIf ViewState("type") = "PREALERT" Then
                    .CommandText &= "SELECT POD_VENDOR_ITEM_CODE AS 'Item Code', POD_PRODUCT_DESC AS 'Item Name', PM_SPEC1 AS 'Specification 1', " & _
                                "PM_SPEC2 AS 'Specification 2', PM_SPEC3 AS 'Specification 3', DOL_LOT_NO AS 'Lot No', GD_RECEIVED_QTY AS 'GRN Received Qty', " & _
                                "IFNULL(STOCKONHANDQTY,0) AS 'Stock On Hand QTY', DATE_FORMAT(DOL_DO_EXP_DT, '%d/%m/%Y') AS 'Expiry Date' FROM " & _
                                "(SELECT POD_VENDOR_ITEM_CODE, POD_PRODUCT_DESC, PM_SPEC1, PM_SPEC2, PM_SPEC3, DOL_LOT_NO, DOL_LOT_INDEX, " & _
                                "SUM(GD_RECEIVED_QTY) AS GD_RECEIVED_QTY, DOL_DO_EXP_DT, IM_INVENTORY_INDEX " & _
                                "FROM grn_mstr " & _
                                "INNER JOIN grn_details ON GM_B_COY_ID = GD_B_COY_ID AND GM_GRN_NO = GD_GRN_NO " & _
                                "INNER JOIN do_mstr ON GM_DO_INDEX = DOM_DO_INDEX " & _
                                "INNER JOIN do_details ON DOM_S_COY_ID = DOD_S_COY_ID AND DOD_DO_NO = DOM_DO_NO " & _
                                "INNER JOIN po_mstr ON POM_PO_INDEX = GM_PO_INDEX " & _
                                "INNER JOIN po_details ON POD_COY_ID =POM_B_COY_ID AND POM_PO_NO = POD_PO_NO AND POD_PO_LINE = DOD_PO_LINE  " & _
                                "INNER JOIN do_lot ON DOL_COY_ID=DOD_S_COY_ID AND DOL_DO_NO=DOD_DO_NO AND DOL_ITEM_CODE = POD_VENDOR_ITEM_CODE " & _
                                "INNER JOIN product_mstr ON PM_S_COY_ID = GM_B_COY_ID AND PM_VENDOR_ITEM_CODE = POD_VENDOR_ITEM_CODE " & _
                                "INNER JOIN inventory_mstr ON IM_COY_ID = GM_B_COY_ID AND POD_VENDOR_ITEM_CODE = IM_ITEM_CODE " & _
                                "WHERE GM_B_COY_ID = '" & Session("CompanyID") & "' AND PM_IQC_IND = 'Y' " & _
                                "AND DOL_DO_EXP_DT >= DATE_ADD('" & strBeginDate & "', INTERVAL -2 MONTH) AND DOL_DO_EXP_DT <= '" & strEndDate & "' " & _
                                "GROUP BY  DOL_DO_NO, POD_PRODUCT_CODE) tb_a " & _
                                "Left Join " & _
                                "(SELECT IL_INVENTORY_INDEX, IL_LOT_INDEX, SUM(IL_LOT_QTY) AS STOCKONHANDQTY " & _
                                "FROM inventory_lot " & _
                                "GROUP BY IL_INVENTORY_INDEX, IL_LOT_INDEX) tb_b " & _
                                "ON tb_a.IM_INVENTORY_INDEX = tb_b.IL_INVENTORY_INDEX AND tb_a.DOL_LOT_INDEX=tb_b.IL_LOT_INDEX " & _
                                "ORDER BY POD_VENDOR_ITEM_CODE, DOL_LOT_NO"

                ElseIf ViewState("type") = "MTHISSUE" Then
                    .CommandText = "SELECT IRSM_IRS_SECTION AS 'Section Code', CS_SEC_NAME AS 'Section Name', PM_ACCT_CODE AS 'Account Code', " & _
                                "CASE WHEN PM_OVERSEA = 'N' THEN 'Local' ELSE 'Oversea' END AS 'Local/Oversea', IM_ITEM_CODE AS 'Item Code', IM_INVENTORY_NAME AS 'Item Name', " & _
                                "PM_SPEC1 AS 'Specification 1', PM_SPEC2 AS 'Specification 2', PM_SPEC3 AS 'Specification 3', " & _
                                "DATE_FORMAT(IRSM_IRS_APPROVED_DATE, '%d/%m/%Y') AS 'Issue Date', IRSM_IRS_NO AS 'Issue Number', IC_COST_QTY AS 'Issue Qty', " & _
                                "IC_COST_UPRICE AS 'Issue Price', IC_COST_COST AS 'Issue Amount', (IC_COST_QTY / IC_COST_COST) AS 'Average-Price', IRSM_BUYER_ID AS 'User ID', " & _
                                "(SELECT IRD_IR_NO FROM INVENTORY_REQUISITION_DETAILS WHERE IRD_IR_SLIP_INDEX = IRSM_IRS_INDEX LIMIT 1) AS 'Requisition No', PM_UOM AS 'UOM' " & _
                                "FROM INVENTORY_REQUISITION_SLIP_MSTR " & _
                                "INNER JOIN INVENTORY_REQUISITION_SLIP_DETAILS ON IRSM_IRS_COY_ID = IRSD_IRS_COY_ID AND IRSM_IRS_NO = IRSD_IRS_NO " & _
                                "INNER JOIN INVENTORY_MSTR ON IRSD_INVENTORY_INDEX = IM_INVENTORY_INDEX " & _
                                "INNER JOIN COMPANY_SECTION ON IRSM_IRS_SECTION = CS_SEC_CODE AND IRSM_IRS_COY_ID = CS_COY_ID " & _
                                "INNER JOIN PRODUCT_MSTR ON IM_ITEM_CODE = PM_VENDOR_ITEM_CODE AND IM_COY_ID = PM_S_COY_ID " & _
                                "INNER JOIN INVENTORY_COST ON IRSD_IRS_COY_ID = IC_COY_ID AND IRSD_IRS_NO = IC_INVENTORY_REF_DOC AND IRSD_INVENTORY_INDEX = IC_INVENTORY_INDEX AND IC_INVENTORY_TYPE = 'II' AND IRSD_QTY = IC_COST_QTY " & _
                                "WHERE (IRSM_IRS_STATUS = '3' OR IRSM_IRS_STATUS = '4') AND IRSM_IRS_COY_ID = '" & Session("CompanyId") & "' " & _
                                "AND IRSM_IRS_APPROVED_DATE >= '" & strBeginDate & "' AND IRSM_IRS_APPROVED_DATE <= '" & strEndDate & "' " & _
                                "ORDER BY IRSM_IRS_SECTION, PM_ACCT_CODE, PM_OVERSEA, IM_ITEM_CODE "

                ElseIf ViewState("type") = "ITEMMTRS" Then
                    .CommandText = "SELECT IM_ITEM_CODE AS 'Item Code', IM_INVENTORY_NAME AS 'Item Name', " & _
                               "CAST(CONCAT(PM_SPEC1,' ',PM_SPEC2,' ',PM_SPEC3) AS CHAR(250)) AS 'Item Specification', " & _
                               "IFNULL(MONTHCONSUMPTION,0.00) AS 'Monthly Consumption', IFNULL(PM_SAFE_QTY,0.00) AS 'Min Stock Qty', " & _
                               "IFNULL(SUM(POD_ORDERED_QTY - (POD_RECEIVED_QTY - POD_REJECTED_QTY)),0.00) AS 'PO Bal Qty', DATE_FORMAT(IRSM_IRS_DATE, '%d/%m/%Y') AS 'Last Issued Date', " & _
                               "IFNULL(OP_CURR_MTH_AMT,0.00) AS 'Opening Stock - Cur. Mth. Amt', IFNULL(OP_CURR_MTH_QTY,0.00) AS 'Opening Stock - Cur. Mth. Qty', IFNULL(OP_PAST_1_MTH_QTY,0.00) AS 'Opening Stock - Past 1 Mth Qty', IFNULL(OP_PAST_2_MTH_QTY,0.00) AS 'Opening Stock - Past 2 Mth Qty', " & _
                               "IFNULL(OP_PAST_3_MTH_QTY,0.00) AS 'Opening Stock - Past 3 Mth Qty', IFNULL(OP_PAST_4_MTH_QTY,0.00) AS 'Opening Stock - Past 4 Mth Qty', IFNULL(OP_PAST_5_MTH_QTY,0.00) AS 'Opening Stock - Past 5 Mth Qty', IFNULL(OP_PAST_6_MTH_QTY,0.00) AS 'Opening Stock - Past 6 Mth Qty', " & _
                               "IFNULL(GRN_CURR_MTH_AMT,0.00) AS 'Delivery - Cur. Mth. Amt', IFNULL(GRN_CURR_MTH_QTY,0.00) AS 'Delivery - Cur. Mth. Qty', IFNULL(GRN_PAST_1_MTH_QTY,0.00) AS 'Delivery - Past 1 Mth Qty', IFNULL(GRN_PAST_2_MTH_QTY,0.00) AS 'Delivery - Past 2 Mth Qty', " & _
                               "IFNULL(GRN_PAST_3_MTH_QTY,0.00) AS 'Delivery - Past 3 Mth Qty', IFNULL(GRN_PAST_4_MTH_QTY,0.00) AS 'Delivery - Past 4 Mth Qty', IFNULL(GRN_PAST_5_MTH_QTY,0.00) AS 'Delivery - Past 5 Mth Qty', IFNULL(GRN_PAST_6_MTH_QTY,0.00) AS 'Delivery - Past 6 Mth Qty', " & _
                               "(IFNULL(II_CURR_MTH_AMT,0.00) - IFNULL(IIC_CURR_MTH_AMT,0.00)) AS 'Issue - Cur. Mth. Amt', (IFNULL(II_CURR_MTH_QTY,0.00) - IFNULL(IIC_CURR_MTH_QTY,0.00)) AS 'Issue - Cur. Mth. Qty', " & _
                               "(IFNULL(II_PAST_1_MTH_QTY,0.00) - IFNULL(IIC_PAST_1_MTH_QTY,0.00)) AS 'Issue - Past 1 Mth Qty', (IFNULL(II_PAST_2_MTH_QTY,0.00) - IFNULL(IIC_PAST_2_MTH_QTY,0.00)) AS 'Issue - Past 2 Mth Qty', " & _
                               "(IFNULL(II_PAST_3_MTH_QTY,0.00) - IFNULL(IIC_PAST_3_MTH_QTY,0.00)) AS 'Issue - Past 3 Mth Qty', (IFNULL(II_PAST_4_MTH_QTY,0.00) - IFNULL(IIC_PAST_4_MTH_QTY,0.00)) AS 'Issue - Past 4 Mth Qty', " & _
                               "(IFNULL(II_PAST_5_MTH_QTY,0.00) - IFNULL(IIC_PAST_5_MTH_QTY,0.00)) AS 'Issue - Past 5 Mth Qty', (IFNULL(II_PAST_6_MTH_QTY,0.00) - IFNULL(IIC_PAST_6_MTH_QTY,0.00)) AS 'Issue - Past 6 Mth Qty', " & _
                               "IFNULL(RI_CURR_MTH_AMT,0.00) AS 'Return - Cur. Mth. Amt', IFNULL(RI_CURR_MTH_QTY,0.00) AS 'Return - Cur. Mth. Qty', IFNULL(RI_PAST_1_MTH_QTY,0.00) AS 'Return - Past 1 Mth Qty', IFNULL(RI_PAST_2_MTH_QTY,0.00) AS 'Return - Past 2 Mth Qty', " & _
                               "IFNULL(RI_PAST_3_MTH_QTY,0.00) AS 'Return - Past 3 Mth Qty', IFNULL(RI_PAST_4_MTH_QTY,0.00) AS 'Return - Past 4 Mth Qty', IFNULL(RI_PAST_5_MTH_QTY,0.00) AS 'Return - Past 5 Mth Qty', IFNULL(RI_PAST_6_MTH_QTY,0.00) AS 'Return - Past 6 Mth Qty', " & _
                               "IFNULL(WO_CURR_MTH_AMT,0.00) AS 'Disposal - Cur. Mth. Amt', IFNULL(WO_CURR_MTH_QTY,0.00) AS 'Disposal - Cur. Mth. Qty', IFNULL(WO_PAST_1_MTH_QTY,0.00) AS 'Disposal - Past 1 Mth Qty', IFNULL(WO_PAST_2_MTH_QTY,0.00) AS 'Disposal - Past 2 Mth Qty', " & _
                               "IFNULL(WO_PAST_3_MTH_QTY,0.00) AS 'Disposal - Past 3 Mth Qty', IFNULL(WO_PAST_4_MTH_QTY,0.00) AS 'Disposal - Past 4 Mth Qty', IFNULL(WO_PAST_5_MTH_QTY,0.00) AS 'Disposal - Past 5 Mth Qty', IFNULL(WO_PAST_6_MTH_QTY,0.00) AS 'Disposal - Past 6 Mth Qty', " & _
                               "IFNULL(SOH_CURR_MTH_AMT,0.00) AS 'Stock On Hand - Cur. Mth. Amt', IFNULL(SOH_CURR_MTH_QTY,0.00) AS 'Stock On Hand - Cur. Mth. Qty', IFNULL(SOH_PAST_1_MTH_QTY,0.00) AS 'Stock On Hand - Past 1 Mth Qty', IFNULL(SOH_PAST_2_MTH_QTY,0.00) AS 'Stock On Hand - Past 2 Mth Qty', " & _
                               "IFNULL(SOH_PAST_3_MTH_QTY,0.00) AS 'Stock On Hand - Past 3 Mth Qty', IFNULL(SOH_PAST_4_MTH_QTY,0.00) AS 'Stock On Hand - Past 4 Mth Qty', IFNULL(SOH_PAST_5_MTH_QTY,0.00) AS 'Stock On Hand - Past 5 Mth Qty', IFNULL(SOH_PAST_6_MTH_QTY,0.00) AS 'Stock On Hand - Past 6 Mth Qty' " & _
                               "FROM (SELECT IM_INVENTORY_INDEX, IM_ITEM_CODE, IM_INVENTORY_NAME, IM_COY_ID, " & _
                               "(SELECT SUM(IC_COST_OPEN_COST) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='OPENING' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "') AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "')) AS OP_CURR_MTH_AMT, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='OPENING' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "') AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "')) AS OP_CURR_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='OPENING' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 1 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 1 MONTH)) AS OP_PAST_1_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='OPENING' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 2 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 2 MONTH)) AS OP_PAST_2_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='OPENING' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 3 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 3 MONTH)) AS OP_PAST_3_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='OPENING' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 4 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 4 MONTH)) AS OP_PAST_4_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='OPENING' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 5 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 5 MONTH)) AS OP_PAST_5_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='OPENING' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 6 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 6 MONTH)) AS OP_PAST_6_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_COST) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='GRN' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "') AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "')) AS GRN_CURR_MTH_AMT, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='GRN' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "') AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "')) AS GRN_CURR_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='GRN' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 1 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 1 MONTH)) AS GRN_PAST_1_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='GRN' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 2 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 2 MONTH)) AS GRN_PAST_2_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='GRN' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 3 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 3 MONTH)) AS GRN_PAST_3_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='GRN' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 4 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 4 MONTH)) AS GRN_PAST_4_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='GRN' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 5 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 5 MONTH)) AS GRN_PAST_5_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='GRN' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 6 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 6 MONTH)) AS GRN_PAST_6_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_COST) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "') AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "')) AS II_CURR_MTH_AMT, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "') AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "')) AS II_CURR_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 1 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 1 MONTH)) AS II_PAST_1_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 2 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 2 MONTH)) AS II_PAST_2_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 3 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 3 MONTH)) AS II_PAST_3_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 4 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 4 MONTH)) AS II_PAST_4_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 5 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 5 MONTH)) AS II_PAST_5_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 6 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 6 MONTH)) AS II_PAST_6_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_COST) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='IIC' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "') AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "')) AS IIC_CURR_MTH_AMT, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='IIC' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "') AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "')) AS IIC_CURR_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='IIC' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 1 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 1 MONTH)) AS IIC_PAST_1_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='IIC' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 2 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 2 MONTH)) AS IIC_PAST_2_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='IIC' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 3 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 3 MONTH)) AS IIC_PAST_3_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='IIC' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 4 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 4 MONTH)) AS IIC_PAST_4_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='IIC' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 5 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 5 MONTH)) AS IIC_PAST_5_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='IIC' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 6 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 6 MONTH)) AS IIC_PAST_6_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_COST) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='RI' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "') AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "')) AS RI_CURR_MTH_AMT, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='RI' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "') AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "')) AS RI_CURR_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='RI' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 1 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 1 MONTH)) AS RI_PAST_1_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='RI' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 2 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 2 MONTH)) AS RI_PAST_2_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='RI' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 3 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 3 MONTH)) AS RI_PAST_3_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='RI' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 4 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 4 MONTH)) AS RI_PAST_4_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='RI' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 5 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 5 MONTH)) AS RI_PAST_5_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='RI' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 6 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 6 MONTH)) AS RI_PAST_6_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_COST) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='WO' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "') AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "')) AS WO_CURR_MTH_AMT, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='WO' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "') AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "')) AS WO_CURR_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='WO' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 1 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 1 MONTH)) AS WO_PAST_1_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='WO' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 2 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 2 MONTH)) AS WO_PAST_2_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='WO' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 3 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 3 MONTH)) AS WO_PAST_3_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='WO' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 4 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 4 MONTH)) AS WO_PAST_4_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='WO' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 5 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 5 MONTH)) AS WO_PAST_5_MTH_QTY, " & _
                               "(SELECT SUM(IC_COST_OPEN_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='WO' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 6 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 6 MONTH)) AS WO_PAST_6_MTH_QTY " & _
                               "FROM INVENTORY_MSTR WHERE IM_COY_ID = '" & Session("CompanyID") & "') tb_a " & _
                               "LEFT JOIN " & _
                               "(SELECT DISTINCT IC_INVENTORY_INDEX, " & _
                               "(SELECT IC_COST_OPEN_COST FROM INVENTORY_COST WHERE IC_INVENTORY_INDEX = IC.IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "') AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "') ORDER BY IC_COST_INDEX DESC LIMIT 1) AS SOH_CURR_MTH_AMT, " & _
                               "(SELECT IC_COST_OPEN_QTY FROM INVENTORY_COST WHERE IC_INVENTORY_INDEX = IC.IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "') AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "') ORDER BY IC_COST_INDEX DESC LIMIT 1) AS SOH_CURR_MTH_QTY, " & _
                               "(SELECT IC_COST_OPEN_QTY FROM INVENTORY_COST WHERE IC_INVENTORY_INDEX = IC.IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 1 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 1 MONTH) ORDER BY IC_COST_INDEX DESC LIMIT 1) AS SOH_PAST_1_MTH_QTY, " & _
                               "(SELECT IC_COST_OPEN_QTY FROM INVENTORY_COST WHERE IC_INVENTORY_INDEX = IC.IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 2 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 2 MONTH) ORDER BY IC_COST_INDEX DESC LIMIT 1) AS SOH_PAST_2_MTH_QTY, " & _
                               "(SELECT IC_COST_OPEN_QTY FROM INVENTORY_COST WHERE IC_INVENTORY_INDEX = IC.IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 3 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 3 MONTH) ORDER BY IC_COST_INDEX DESC LIMIT 1) AS SOH_PAST_3_MTH_QTY, " & _
                               "(SELECT IC_COST_OPEN_QTY FROM INVENTORY_COST WHERE IC_INVENTORY_INDEX = IC.IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 4 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 4 MONTH) ORDER BY IC_COST_INDEX DESC LIMIT 1) AS SOH_PAST_4_MTH_QTY, " & _
                               "(SELECT IC_COST_OPEN_QTY FROM INVENTORY_COST WHERE IC_INVENTORY_INDEX = IC.IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 5 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 5 MONTH) ORDER BY IC_COST_INDEX DESC LIMIT 1) AS SOH_PAST_5_MTH_QTY, " & _
                               "(SELECT IC_COST_OPEN_QTY FROM INVENTORY_COST WHERE IC_INVENTORY_INDEX = IC.IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 6 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 6 MONTH) ORDER BY IC_COST_INDEX DESC LIMIT 1) AS SOH_PAST_6_MTH_QTY " & _
                               "FROM INVENTORY_COST IC " & _
                               "WHERE IC_COY_ID = '" & Session("CompanyID") & "') tb_b " & _
                               "ON tb_b.IC_INVENTORY_INDEX = tb_a.IM_INVENTORY_INDEX " & _
                               "INNER JOIN PRODUCT_MSTR ON IM_ITEM_CODE = PM_VENDOR_ITEM_CODE AND IM_COY_ID = PM_S_COY_ID " & _
                               "LEFT JOIN PO_DETAILS ON POD_COY_ID = IM_COY_ID AND POD_VENDOR_ITEM_CODE = IM_ITEM_CODE " & _
                               "LEFT JOIN " & _
                               "(SELECT IRSD_INVENTORY_INDEX, MAX(IRSM_IRS_DATE) AS IRSM_IRS_DATE, SUM(IRSD_QTY) / 3 AS MONTHCONSUMPTION " & _
                               "FROM INVENTORY_REQUISITION_SLIP_DETAILS INNER JOIN INVENTORY_REQUISITION_SLIP_MSTR ON IRSM_IRS_COY_ID = IRSD_IRS_COY_ID  AND IRSM_IRS_NO = IRSD_IRS_NO " & _
                               "WHERE IRSM_IRS_COY_ID ='" & Session("CompanyID") & "' " & _
                               "AND IRSM_IRS_DATE BETWEEN ('" & strDate & "' - INTERVAL 3 MONTH) AND ('" & strDate & "' - INTERVAL 1 MONTH) " & _
                               "AND YEAR(IRSM_IRS_DATE) BETWEEN YEAR('" & strDate & "' - INTERVAL 3 MONTH) AND YEAR('" & strDate & "' - INTERVAL 1 MONTH) " & _
                               "GROUP BY IRSD_INVENTORY_INDEX) tb_c " & _
                               "ON tb_c.IRSD_INVENTORY_INDEX = tb_a.IM_INVENTORY_INDEX " & _
                               "WHERE PM_OVERSEA = '" & strOversea & "' " & _
                               "GROUP BY IM_INVENTORY_INDEX ORDER BY IM_ITEM_CODE "

                ElseIf ViewState("type") = "MTHCOSTTREND" Then
                    .CommandText = "SELECT PM_ACCT_CODE AS 'Account Code', IM_ITEM_CODE AS 'Item Code', IM_INVENTORY_NAME AS 'Item Name', PM_SPEC1 AS 'Specification 1', PM_SPEC2 AS 'Specification 2', PM_SPEC3 AS 'Specification 3', " & _
                                "(SELECT GROUP_CONCAT(PV_PUR_SPEC_NO) FROM PIM_VENDOR WHERE PV_PRODUCT_INDEX = PM_PRODUCT_INDEX AND PV_PUR_SPEC_NO <> '' GROUP BY PV_PRODUCT_INDEX) AS 'Purchasing Spec. No.', " & _
                                "FORMAT(IFNULL(PM_BUDGET_PRICE,0.000),3) AS 'Budget', " & _
                                "FORMAT(IFNULL(PAST_6_MTH_UG,0),2) AS 'Usage: " & strPastMth6 & "', FORMAT(IFNULL(PAST_5_MTH_UG,0),2) AS 'Usage: " & strPastMth5 & "', FORMAT(IFNULL(PAST_4_MTH_UG,0),2) AS 'Usage: " & strPastMth4 & "', " & _
                                "FORMAT(IFNULL(PAST_3_MTH_UG,0),2) AS 'Usage: " & strPastMth3 & "', FORMAT(IFNULL(PAST_2_MTH_UG,0),2) AS 'Usage: " & strPastMth2 & "', FORMAT(IFNULL(PAST_1_MTH_UG,0),2) AS 'Usage: " & strPastMth1 & "', " & _
                                "FORMAT(IFNULL(PAST_6_MTH_MA,0),3) AS 'Moving Average: " & strPastMth6 & "', FORMAT(IFNULL(PAST_5_MTH_MA,0),3) AS 'Moving Average: " & strPastMth5 & "', FORMAT(IFNULL(PAST_4_MTH_MA,0),3) AS 'Moving Average: " & strPastMth4 & "', " & _
                                "FORMAT(IFNULL(PAST_3_MTH_MA,0),3) AS 'Moving Average: " & strPastMth3 & "', FORMAT(IFNULL(PAST_2_MTH_MA,0),3) AS 'Moving Average: " & strPastMth2 & "', FORMAT(IFNULL(PAST_1_MTH_MA,0),3) AS 'Moving Average: " & strPastMth1 & "', " & _
                                "FORMAT(IFNULL(PAST_6_MTH_LC,0),3) AS 'Landed Cost: " & strPastMth6 & "', FORMAT(IFNULL(PAST_5_MTH_LC,0),3) AS 'Landed Cost: " & strPastMth5 & "', FORMAT(IFNULL(PAST_4_MTH_LC,0),3) AS 'Landed Cost: " & strPastMth4 & "', " & _
                                "FORMAT(IFNULL(PAST_3_MTH_LC,0),3) AS 'Landed Cost: " & strPastMth3 & "', FORMAT(IFNULL(PAST_2_MTH_LC,0),3) AS 'Landed Cost: " & strPastMth2 & "', FORMAT(IFNULL(PAST_1_MTH_LC,0),3) AS 'Landed Cost: " & strPastMth1 & "', " & _
                                "PM_UOM AS 'UOM' " & _
                                "FROM (SELECT IM_INVENTORY_INDEX, IM_ITEM_CODE, IM_INVENTORY_NAME, IM_COY_ID, " & _
                                "(SELECT SUM(IC_COST_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 1 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 1 MONTH)) AS PAST_1_MTH_UG, " & _
                                "(SELECT SUM(IC_COST_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 2 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 2 MONTH)) AS PAST_2_MTH_UG, " & _
                                "(SELECT SUM(IC_COST_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 3 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 3 MONTH)) AS PAST_3_MTH_UG, " & _
                                "(SELECT SUM(IC_COST_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 4 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 4 MONTH)) AS PAST_4_MTH_UG, " & _
                                "(SELECT SUM(IC_COST_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 5 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 5 MONTH)) AS PAST_5_MTH_UG, " & _
                                "(SELECT SUM(IC_COST_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 6 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 6 MONTH)) AS PAST_6_MTH_UG, " & _
                                "(SELECT SUM(IC_COST_UPRICE) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 1 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 1 MONTH)) AS PAST_1_MTH_MA, " & _
                                "(SELECT SUM(IC_COST_UPRICE) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 2 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 2 MONTH)) AS PAST_2_MTH_MA, " & _
                                "(SELECT SUM(IC_COST_UPRICE) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 3 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 3 MONTH)) AS PAST_3_MTH_MA, " & _
                                "(SELECT SUM(IC_COST_UPRICE) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 4 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 4 MONTH)) AS PAST_4_MTH_MA, " & _
                                "(SELECT SUM(IC_COST_UPRICE) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 5 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 5 MONTH)) AS PAST_5_MTH_MA, " & _
                                "(SELECT SUM(IC_COST_UPRICE) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='II' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 6 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 6 MONTH)) AS PAST_6_MTH_MA, " & _
                                "(SELECT SUM(IC_COST_COST/IC_COST_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='GRN' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 1 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 1 MONTH)) AS PAST_1_MTH_LC, " & _
                                "(SELECT SUM(IC_COST_COST/IC_COST_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='GRN' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 2 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 2 MONTH)) AS PAST_2_MTH_LC, " & _
                                "(SELECT SUM(IC_COST_COST/IC_COST_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='GRN' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 3 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 3 MONTH)) AS PAST_3_MTH_LC, " & _
                                "(SELECT SUM(IC_COST_COST/IC_COST_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='GRN' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 4 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 4 MONTH)) AS PAST_4_MTH_LC, " & _
                                "(SELECT SUM(IC_COST_COST/IC_COST_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='GRN' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 5 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 5 MONTH)) AS PAST_5_MTH_LC, " & _
                                "(SELECT SUM(IC_COST_COST/IC_COST_QTY) FROM INVENTORY_COST WHERE IC_INVENTORY_TYPE='GRN' AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX AND MONTH(IC_COST_DATE)=MONTH('" & strDate & "' - INTERVAL 6 MONTH) AND YEAR(IC_COST_DATE)=YEAR('" & strDate & "' - INTERVAL 6 MONTH)) AS PAST_6_MTH_LC " & _
                                "FROM INVENTORY_MSTR WHERE IM_COY_ID = '" & Session("CompanyID") & "') tb_a " & _
                                "INNER JOIN PRODUCT_MSTR ON PM_S_COY_ID = IM_COY_ID AND IM_ITEM_CODE = PM_VENDOR_ITEM_CODE " & _
                                "WHERE PM_OVERSEA = '" & strOversea & "' " & _
                                "ORDER BY PM_ACCT_CODE, IM_ITEM_CODE "

                ElseIf ViewState("type") = "MTHGRN" Then
                    .CommandText &= "SELECT PM_ACCT_CODE, PM_VENDOR_ITEM_CODE, PM_OVERSEA, PM_ITEM_TYPE, CM_COY_ID, CM_COY_NAME, INV_ITM_TTL AS 'Inventory Items Total', DC_ITM_TTL AS 'Direct Charge Items Total', " & _
                                "MAINTAINENCE_ITM_TTL AS 'Maintainence (E08) Items Total', M_ITM_TTL  AS 'Maintainence (E10) Items Total', SC_ITM_TTL AS 'Services & Capital Items Total', SUS_AMOUNT AS 'Supplies Stock', SHS_AMOUNT AS 'Shipping Stock', US_AMOUNT AS 'Uniform & Safety', PS_AMOUNT AS 'Printing & Stationery', " & _
                                "SM_AMOUNT AS 'Sub-Material Stock', CS_AMOUNT AS 'Chemical Stock', Vendor_country, " & _
                                "L_MAINTAINENCE_ITM_TTL AS 'Maintainence (C08) Items Total', L_M_ITM_TTL AS 'Maintainence (C10) Items Total'  FROM ( " & _
                                "SELECT '1' AS tb, PM_ACCT_CODE, PM_VENDOR_ITEM_CODE, PM_OVERSEA, PM_ITEM_TYPE, PM_PRODUCT_DESC, CM_COY_ID, " & _
                                "CM_COY_NAME, '' AS INV_ITM_TTL, '' AS DC_ITM_TTL, '' AS MAINTAINENCE_ITM_TTL, '' AS M_ITM_TTL,'' AS SC_ITM_TTL, SUS_AMOUNT, SHS_AMOUNT, US_AMOUNT, PS_AMOUNT, " & _
                                "SM_AMOUNT, CS_AMOUNT, Vendor_country, '' AS L_MAINTAINENCE_ITM_TTL, '' AS L_M_ITM_TTL FROM ( " & _
                                "SELECT *, " & _
                                "CASE WHEN  CT_NAME = 'Supplies Stock' THEN GD_RECEIVED_QTY - GD_REJECTED_QTY ELSE 0 END AS SUS_AMOUNT, " & _
                                "CASE WHEN  CT_NAME = 'Shipping Stock' THEN GD_RECEIVED_QTY - GD_REJECTED_QTY ELSE 0 END AS SHS_AMOUNT, " & _
                                "CASE WHEN  CT_NAME = 'Uniform & Safety' THEN GD_RECEIVED_QTY - GD_REJECTED_QTY ELSE 0 END AS US_AMOUNT, " & _
                                "CASE WHEN  CT_NAME = 'Printing & Stationery' THEN GD_RECEIVED_QTY - GD_REJECTED_QTY ELSE 0 END AS PS_AMOUNT, " & _
                                "CASE WHEN  CT_NAME = 'Sub-Material Stock' THEN GD_RECEIVED_QTY - GD_REJECTED_QTY ELSE 0 END AS SM_AMOUNT, " & _
                                "CASE WHEN  CT_NAME = 'Chemical Stock' THEN GD_RECEIVED_QTY - GD_REJECTED_QTY ELSE 0 END AS CS_AMOUNT, " & _
                                "CASE WHEN CM_COUNTRY='MY' THEN 'LOCAL SUPPLIER' " & _
                                "WHEN CM_COY_NAME='SEH JAPAN' AND CM_COUNTRY='JP' THEN 'SEH JAPAN' " & _
                                "WHEN CM_COY_NAME='SEH AMERICA' AND CM_COUNTRY='US' THEN 'SEH AMERICA'  " & _
                                "WHEN CM_COUNTRY <> 'MY' AND (CM_COY_NAME='SEH JAPAN' AND CM_COUNTRY = 'JP')  " & _
                                "AND (CM_COY_NAME<>'SEH AMERICA' AND CM_COUNTRY = 'US') THEN 'OTHER OVERSEA SUPPLIER' END AS Vendor_country " & _
                                "FROM grn_details  " & _
                                "INNER JOIN grn_mstr ON GM_B_COY_ID  = GD_B_COY_ID AND GM_GRN_NO = GD_GRN_NO " & _
                                "INNER JOIN po_mstr ON POM_PO_INDEX = GM_PO_INDEX " & _
                                "INNER JOIN po_details ON POM_B_COY_ID = POD_COY_ID AND POD_PO_LINE = GD_PO_LINE AND pom_po_no = pod_po_no " & _
                                "INNER JOIN product_mstr ON PM_S_COY_ID = POM_S_COY_ID AND PM_VENDOR_ITEM_CODE = POD_VENDOR_ITEM_CODE AND POM_PO_INDEX = GM_GRN_INDEX " & _
                                "INNER JOIN commodity_type ON PM_CATEGORY_NAME = CT_ID " & _
                                "INNER JOIN company_mstr ON CM_COY_ID = GM_B_COY_ID " & _
                                "WHERE (CT_NAME = 'Supplies Stock' OR CT_NAME = 'Shipping Stock' OR CT_NAME = 'Uniform & Safety' " & _
                                "OR CT_NAME = 'Printing & Stationery' OR CT_NAME = 'Sub-Material Stock' OR CT_NAME = 'Chemical Stock') AND GM_B_COY_ID = '" & Session("CompanyID") & "')tb_a " & _
                                "UNION " & _
                                "SELECT '2' AS tb, PM_ACCT_CODE, PM_VENDOR_ITEM_CODE, PM_OVERSEA, PM_ITEM_TYPE, PM_PRODUCT_DESC, CM_COY_ID, CM_COY_NAME, INV_ITM_TTL, " & _
                                "DC_ITM_TTL, MAINTAINENCE_ITM_TTL, M_ITM_TTL, SC_ITM_TTL, '' AS SUS_AMOUNT, '' AS SHS_AMOUNT, '' AS US_AMOUNT, '' AS PS_AMOUNT, " & _
                                "'' AS SM_AMOUNT, '' AS CS_AMOUNT, Vendor_country, L_MAINTAINENCE_ITM_TTL, L_M_ITM_TTL FROM ( " & _
                                "SELECT * , " & _
                                "CASE WHEN PM_ITEM_TYPE = 'ST'  THEN 1 ELSE 0 END AS INV_ITM_TTL, " & _
                                "CASE WHEN PM_ITEM_TYPE = 'SP' THEN 1 ELSE 0 END AS DC_ITM_TTL, " & _
                                "CASE WHEN PM_ACCT_CODE = 'E08' THEN 1 ELSE 0 END AS MAINTAINENCE_ITM_TTL, " & _
                                "CASE WHEN PM_ACCT_CODE = 'E10' THEN 1 ELSE 0 END AS M_ITM_TTL, " & _
                                "CASE WHEN PM_ACCT_CODE = 'E09' THEN 1 ELSE 0 END AS SC_ITM_TTL, " & _
                                "CASE WHEN PM_ACCT_CODE = 'C08' THEN 1 ELSE 0 END AS L_MAINTAINENCE_ITM_TTL, " & _
                                "CASE WHEN PM_ACCT_CODE = 'C10' THEN 1 ELSE 0 END AS L_M_ITM_TTL, " & _
                                "CASE WHEN CM_COUNTRY='MY' THEN 'LOCAL SUPPLIER'   " & _
                                "WHEN CM_COY_NAME='SEH JAPAN' AND CM_COUNTRY='JP' THEN 'SEH JAPAN' " & _
                                "WHEN CM_COY_NAME='SEH AMERICA' AND CM_COUNTRY='US' THEN 'SEH AMERICA'  " & _
                                "WHEN CM_COUNTRY <> 'MY' AND (CM_COY_NAME='SEH JAPAN' AND CM_COUNTRY = 'JP')  " & _
                                "AND (CM_COY_NAME<>'SEH AMERICA' AND CM_COUNTRY = 'US') THEN 'OTHER OVERSEA SUPPLIER' END AS Vendor_country " & _
                                "FROM grn_details " & _
                                "INNER JOIN grn_mstr ON GM_B_COY_ID  = GD_B_COY_ID AND GM_GRN_NO = GD_GRN_NO " & _
                                "INNER JOIN po_mstr ON POM_PO_INDEX = GM_PO_INDEX " & _
                                "INNER JOIN po_details ON POM_B_COY_ID = POD_COY_ID AND POD_PO_LINE = GD_PO_LINE AND pom_po_no = pod_po_no " & _
                                "INNER JOIN product_mstr ON PM_S_COY_ID = POM_S_COY_ID AND PM_VENDOR_ITEM_CODE = POD_VENDOR_ITEM_CODE AND POM_PO_INDEX = GM_GRN_INDEX " & _
                                "INNER JOIN commodity_type ON PM_CATEGORY_NAME = CT_ID " & _
                                "INNER JOIN company_mstr ON CM_COY_ID = GM_B_COY_ID " & _
                                "WHERE (CT_NAME = 'Supplies Stock' OR CT_NAME = 'Shipping Stock' OR CT_NAME = 'Uniform & Safety' " & _
                                "OR CT_NAME = 'Printing & Stationery' OR CT_NAME = 'Sub-Material Stock' OR CT_NAME = 'Chemical Stock') AND GM_B_COY_ID = '" & Session("CompanyID") & "' AND MONTH(GM_DATE_RECEIVED)= '" & strMonth & "' AND YEAR(GM_DATE_RECEIVED)='" & strYear & "') tb_b )tb "


                End If


            End With

            da = New MySqlDataAdapter(cmd)
            da.Fill(ds)

            If ViewState("type") = "TOP20VEN" Then
                strFileName = "Top20VendorsReport"
            ElseIf ViewState("type") = "LOCALOVERSEAITEM" Then
                strFileName = "LocalOverseaItemsReport"
            ElseIf ViewState("type") = "LOCALOVERSEAPUR" Then
                strFileName = "LocalOverseaPurchasingReport"
            ElseIf ViewState("type") = "PRPOFORINV" Then
                strFileName = "POSummaryReport(Inventory)"
            ElseIf ViewState("type") = "PRPOFORSP" Then
                strFileName = "POSummaryReport(Spot)"
            ElseIf ViewState("type") = "POBAL" Then
                strFileName = "POBalanceReport"
            ElseIf ViewState("type") = "DELTREND" Then
                strFileName = "DeliveryTrendReport"
            ElseIf ViewState("type") = "GRNLIST" Then
                strFileName = "GRNListingReport"
            ElseIf ViewState("type") = "INDEXBOOK" Then
                strFileName = "IndexBook"
            ElseIf ViewState("type") = "LASTKEYINNUM" Then
                strFileName = "LastKeyInNumberListReport"
            ElseIf ViewState("type") = "ISSUETREND" Then
                strFileName = "IssuingTrendReport"
            ElseIf ViewState("type") = "STKSTATUS" Then
                strFileName = "StockStatusReport"
            ElseIf ViewState("type") = "STOCKBAL" Then
                strFileName = "StockBalance"
            ElseIf ViewState("type") = "MRSAPPVD" Then
                strFileName = "MRSApprovedListReport"
            ElseIf ViewState("type") = "MRSLIST" Then
                strFileName = "MRSListingReport"
            ElseIf ViewState("type") = "SUMMTHSTKBAL" Then
                strFileName = "SummaryMonthlyStockBalanceReport"
            ElseIf ViewState("type") = "SMFA0612MTH" Then
                strFileName = "SlowMovingReport"
            ElseIf ViewState("type") = "DEADSTK" Then
                strFileName = "DeadStockListReport"
            ElseIf ViewState("type") = "MTHMGMT" Then
                strFileName = "MonthlyManagementReport"
            ElseIf ViewState("type") = "PREALERT" Then
                strFileName = "PreAlertReport"
            ElseIf ViewState("type") = "MTHISSUE" Then
                strFileName = "MonthlyIssueReport"
            ElseIf ViewState("type") = "ITEMMTRS" Then
                strFileName = "ItemMasterListReport"
            ElseIf ViewState("type") = "MTHCOSTTREND" Then
                strFileName = "MonthlyCostTrendReport"
            ElseIf ViewState("type") = "MTHGRN" Then
                strFileName = "MonthlyGoodsReceiptNoteReport"
            End If

            strFileName = strFileName & "(" & Format(dtFrom, "MMMyyyy") & ").xls"
            Dim attachment As String = "attachment;filename=" & strFileName
            Response.ClearContent()
            Response.AddHeader("Content-Disposition", attachment)
            Response.ContentType = "application/vnd.ms-excel"

            Dim dc As DataColumn
            Dim i As Integer

            i = 0
            For Each dc In ds.Tables(0).Columns
                If i > 0 Then
                    Response.Write(vbTab + dc.ColumnName)
                Else
                    Response.Write(dc.ColumnName)
                End If
                i += 1

            Next
            Response.Write(vbCrLf)

            Dim dr As DataRow
            For Each dr In ds.Tables(0).Rows
                For i = 0 To ds.Tables(0).Columns.Count - 1
                    If i > 0 Then
                        Response.Write(vbTab + dr.Item(i).ToString)
                    Else
                        Response.Write(dr.Item(i).ToString)
                    End If
                Next
                Response.Write(vbCrLf)
            Next
            Response.End()

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

    Protected Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        If cboReportType.SelectedValue = "Excel" Then
            If ViewState("type") = "MTHSTOCK1" Then
                'ExportToMonthlyStockReport1Excel()
            ElseIf ViewState("type") = "MTHSTOCK2" Then
                'ExportToMonthlyStockReport2Excel()
            ElseIf ViewState("type") = "PRPOFORSP" Then
                ExportToPOReportSpotExcel()
            Else
                ExportToExcel()
            End If
        ElseIf cboReportType.SelectedValue = "PDF" Then
            If ViewState("type") = "TOP20VEN" Then
                ExportToTop20VendorPDF()
            ElseIf ViewState("type") = "MTHSTOCK1" Then
                'ExportToMonthlyStockReport1PDF()
            ElseIf ViewState("type") = "MTHSTOCK2" Then
                'ExportToMonthlyStockReport2PDF()
            ElseIf ViewState("type") = "PRPOFORSP" Then
                ExportToPOReportSpotPDF()
            Else
                ExportToPDF()
            End If


        End If

    End Sub

    Private Sub cmdClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        cmbMonthFrom.SelectedIndex = 0
        cmbYearFrom.SelectedIndex = 0
        cmbMonthTo.SelectedIndex = 0
        cmbYearTo.SelectedIndex = 0
        dtRadioBtn.SelectedIndex = 0
        'dtDORadioBtn.Checked = True
        'dtINVRadioBtn.Checked = False
        txtSDate.Text = ""
        txtEndDate.Text = ""
        cmbMonth.SelectedIndex = 0
        cmbYear.SelectedIndex = 0
        txtDate.Text = ""
        txtDptCode.Text = ""
        txtSKName.Text = ""
        txtItemCode2.Text = ""
        txtSectionCode.Text = ""
        txtPONo.Text = ""
        txtSuppName.Text = ""
        txtItemCode.Text = ""
        txtSuppCode.Text = ""
        cmbPOBal.SelectedIndex = 0
        cmbOversea.SelectedIndex = 0
        cboReportType.SelectedIndex = 0

    End Sub
End Class