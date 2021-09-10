Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Configuration.ConfigurationManager
Public Class ReportViewer
    Inherits System.Web.UI.Page
    Dim strServerName As String
    Dim strDBName As String
    Dim strUN As String
    Dim strPWD As String
    Dim rpt As String
    Dim rptType As String
    Private m_sFileName As String = ""
    Private m_sDocName As String = ""
    Dim ReportDocument As New ReportDocument
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = 0
        'Response.AddHeader("pragma", "no-cache")
        'Response.AddHeader("cache-content", "private")
        'Response.CacheControl = "no-cache"

        strServerName = AppSettings.Item("ServerName")
        strDBName = AppSettings.Item("DatabaseName")
        strUN = AppSettings.Item("UserName")
        strPWD = AppSettings.Item("Password")
        rpt = Request.QueryString("rpt")
        rptType = Request.QueryString("rptType")

        'ConfigureCrystalReports() 'BindReport()
        Try
            If Not IsPostBack() Then
                Session("doc") = Nothing
                Session("docname") = Nothing
                Session("intPageNo") = Nothing
            End If
            m_sDocName = rpt
            m_sFileName = Server.MapPath(".") & "\" & m_sDocName

            If InStr(m_sDocName, "\") > 0 Then
                m_sDocName = Right(m_sDocName, Len(m_sDocName) - InStr(m_sDocName, "\"))
            End If

            If Session("doc") Is Nothing Then
                LoadReportDocument()
            Else
                CrystalReportViewer1.ReportSource = CType(Session("doc"), ReportDocument)
            End If

            If Not Session("intPageNo") Is Nothing Then
                CrystalReportViewer1.ShowNthPage(Session("intPageNo"))
            End If

        Catch ex As Exception
            ReportDocument.Dispose()
            ReportDocument.Close()
            CrystalReportViewer1.Dispose()
        End Try
    End Sub

    Public Function LoadReportDocument() As ReportDocument

        ' Load the selected report file.
        '
        'Dim ReportDocument As New ReportDocument
        Dim Index, intCounter As Integer

        ReportDocument.Load(m_sFileName)
        'Set Table Login Info
        SetLogin(ReportDocument)

        'Sub report object of crystal report.
        Dim mySubReportObject As CrystalDecisions.CrystalReports.Engine.SubreportObject
        Dim mySubRepDoc As New CrystalDecisions.CrystalReports.Engine.ReportDocument

        For Index = 0 To ReportDocument.ReportDefinition.Sections.Count - 1
            For intCounter = 0 To _
                ReportDocument.ReportDefinition.Sections(Index).ReportObjects.Count - 1
                With ReportDocument.ReportDefinition.Sections(Index)
                    If .ReportObjects(intCounter).Kind = CrystalDecisions.Shared.ReportObjectKind.SubreportObject Then
                        mySubReportObject = CType(.ReportObjects(intCounter), CrystalDecisions.CrystalReports.Engine.SubreportObject)
                        mySubRepDoc = mySubReportObject.OpenSubreport(mySubReportObject.SubreportName)

                        SetLogin(mySubRepDoc)
                    End If
                End With
            Next
        Next

        ReportDocument.Refresh()
        SetParameter(ReportDocument)

        '
        ' Set the Crytal Report Viewer control's source to the report document.
        '
        CrystalReportViewer1.EnableDatabaseLogonPrompt = False
        CrystalReportViewer1.EnableParameterPrompt = False
        CrystalReportViewer1.ReportSource = ReportDocument
        'CrystalReportViewer1.RefreshReport()

        Session("doc") = ReportDocument

        Session("docname") = m_sDocName
        Return ReportDocument

    End Function

    Private Sub SetLogin(ByRef oReport As ReportDocument)
        Dim LogonInfo As CrystalDecisions.Shared.TableLogOnInfo
        Dim Index As Integer

        For Index = 0 To oReport.Database.Tables.Count - 1
            LogonInfo = oReport.Database.Tables(Index).LogOnInfo
            LogonInfo.ConnectionInfo.UserID = strUN
            LogonInfo.ConnectionInfo.Password = strPWD
            LogonInfo.ConnectionInfo.ServerName = strServerName
            LogonInfo.ConnectionInfo.DatabaseName = strDBName

            oReport.Database.Tables(Index).ApplyLogOnInfo(LogonInfo)
            'oReport.Database.Tables(Index).Location = sInitialCatalogue & ".dbo." & oReport.Database.Tables(Index).Location
            'oReport.Database.Tables(Index).Location = oReport.Database.Tables(Index).Location
        Next
        'Dim ConnInfo As New ConnectionInfo

        'With ConnInfo
        '    .ServerName = strServerName
        '    .DatabaseName = strDBName
        '    .UserID = strUN
        '    .Password = strPWD
        'End With

        'Dim RepTbls As Tables = oReport.Database.Tables
        'For Each RepTbl As Table In RepTbls
        '    Dim RepTblLogonInfo As TableLogOnInfo = RepTbl.LogOnInfo
        '    RepTblLogonInfo.ConnectionInfo = ConnInfo
        '    RepTbl.ApplyLogOnInfo(RepTblLogonInfo)
        'Next
    End Sub

    Private Sub SetParameter(ByRef oReport As ReportDocument)

        Dim sParameter As String = ""
        Dim sValue As String = ""
        Dim crParameterFieldDefinition As ParameterFieldDefinition
        Dim crParameterValues As ParameterValues
        Dim crParameterDiscreteValue As ParameterDiscreteValue
        Dim crParameterFieldDefinitions As ParameterFieldDefinitions

        Dim i As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim crExportOptions As ExportOptions
        Dim crDiskFileDestinationOptions As DiskFileDestinationOptions
        Dim fname As String
        Dim MyReportDocument As New ReportDocument

        crParameterFieldDefinitions = oReport.DataDefinition.ParameterFields

        For Each crParameterFieldDefinition In crParameterFieldDefinitions
            sParameter = Replace(crParameterFieldDefinition.ParameterFieldName, "@", "")
            sValue = Request(sParameter)

            If sValue Like "????-??-??T??:??:??" Then sValue = Replace(sValue, "T", " ")

            'If crParameterFieldDefinition.ReportName = "" AndAlso crParameterFieldDefinition.IsLinked = False Then
            If crParameterFieldDefinition.IsLinked = False Then
                'If crParameterFieldDefinition.ReportName = "" Then
                If Not IsNothing(sValue) Then

                    'MsgBox(crParameterFieldDefinition.ReportName & " : " & sParameter & " - " & sValue)
                    crParameterValues = crParameterFieldDefinition.CurrentValues
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue.Value = sValue
                    crParameterValues.Add(crParameterDiscreteValue)

                    crParameterFieldDefinition.ApplyCurrentValues(crParameterValues)
                End If
            End If
        Next

        If rptType <> Nothing Then

            fname = m_sDocName '"usedTimeRpt.pdf"

            Select Case rptType
                Case 0

                    oReport.ExportToHttpResponse(ExportFormatType.Excel, Response, True, fname)

                Case 1
                    oReport.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, fname)
            End Select


        End If


    End Sub

    Protected Sub CrystalReportViewer1_Navigate(ByVal source As Object, ByVal e As CrystalDecisions.Web.NavigateEventArgs) Handles CrystalReportViewer1.Navigate
        If Session("intPageNo") Is Nothing Then
            Session("intPageNo") = e.NewPageNumber
        End If


        If Session("intPageNo") <> e.NewPageNumber Then
            Session("intPageNo") = e.NewPageNumber
        End If
    End Sub

    'Private Sub BindReport()
    '    Dim crPrm As Hashtable

    '    crPrm = Session("Param")
    '    Dim crDatabase As CrystalDecisions.CrystalReports.Engine.Database
    '    Dim crTable As CrystalDecisions.CrystalReports.Engine.Table
    '    Dim dbConn As TableLogOnInfo = New TableLogOnInfo
    '    Dim oRpt As ReportDocument = New ReportDocument
    '    Dim arrTables As Object() = New Object(0) {}
    '    ReDim arrTables(20)

    '    oRpt.Load(Server.MapPath(rpt)) '("D:\Project\Shell Sarawak\eTravel\Report\viewtr.rpt")
    '    crDatabase = oRpt.Database
    '    crDatabase.Tables.CopyTo(arrTables, 0)
    '    crTable = DirectCast(arrTables(0), CrystalDecisions.CrystalReports.Engine.Table)
    '    dbConn = crTable.LogOnInfo
    '    dbConn.ConnectionInfo.ServerName = strServerName
    '    dbConn.ConnectionInfo.DatabaseName = strDBName
    '    dbConn.ConnectionInfo.UserID = strUN
    '    dbConn.ConnectionInfo.Password = strPWD
    '    crTable.ApplyLogOnInfo(dbConn)
    '    Select Case rpt
    '        Case "detailOfPOByCompany2.rpt", "detailOfPRByCompany.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("vendorCompanyIDParam", crPrm("vendorCompanyIDParam"))
    '            oRpt.SetParameterValue("buyerCompanyIDParam", crPrm("buyerCompanyIDParam"))
    '            oRpt.SetParameterValue("dateFromParam", crPrm("dateFromParam"))
    '            oRpt.SetParameterValue("dateToParam", crPrm("dateToParam"))

    '        Case "SummaryOfPOPRRFQByCompany2.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("vendorCompanyIDParam", crPrm("vendorCompanyIDParam"))
    '            oRpt.SetParameterValue("BCOYID", crPrm("BCOYID"))
    '            oRpt.SetParameterValue("dateFrom", crPrm("dateFrom"))
    '            oRpt.SetParameterValue("dateTo", crPrm("dateTo"))

    '        Case "outstandPOFrSuppl2.rpt", "outstandPRByItem2.rpt", "outstandGRNFrSupplHLB.rpt", "outstandGRNFrSuppl2.rpt", "PurchaseToInvoiceCycleRpt2.rpt", "outstandPOFrSupplSeh2.rpt", "consumptionFromVendor.rpt", "outstandingPoByDept.rpt", "outstandDOByDept.rpt", "consumptionByCategory.rpt", "PurchaseToInvoiceCycleByCategory.rpt", "invoiceApproval20KTo100K.rpt", "invoiceApproval100KTo500K.rpt", "detailOfPOByPurchaseSite.rpt", "outstandGRNByPurchaseSite.rpt", "outstandInvoiceByDept.rpt", "SummaryApprovedInvoicesByVendor.rpt", "R5SummaryApprovedPaidInvoicesSortedByItem-BR.rpt", "R6SummaryApprovedPaidInvoicesSortedByItem-HQ.rpt", "PurchaseToInvoiceCyclePrintStatSecurity-HQ.rpt", "PurchaseToInvoiceCyclePrintStatSecurity-BR.rpt", "R11DetailOutstandGRNReprotSortedByBranch-BR.rpt", "R12DetailOutstandGRNReprotSortedByBranch-HQ.rpt", "DebitNote.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("dateFromParam", crPrm("dateFromParam"))
    '            oRpt.SetParameterValue("dateToParam", crPrm("dateToParam"))
    '            oRpt.SetParameterValue("buyerCompanyIDParam", crPrm("buyerCompanyIDParam"))

    '        Case "vendorRevenueForecast.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("dateFromParam", crPrm("dateFromParam"))
    '            oRpt.SetParameterValue("dateToParam", crPrm("dateToParam"))

    '        Case "InvoiceSummaryBySupplier.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("dateFromParam", crPrm("dateFromParam"))
    '            oRpt.SetParameterValue("dateToParam", crPrm("dateToParam"))

    '        Case "pmeExpenses2.rpt", "pmeExpensesDetail2.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("dateFromParam", crPrm("dateFromParam"))
    '            oRpt.SetParameterValue("dateToParam", crPrm("dateToParam"))
    '            oRpt.SetParameterValue("companyIDParam", crPrm("companyIDParam"))

    '        Case "PORaised.rpt", "PurchaseToGRNCycleOEFF.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("dateFromParam", crPrm("dateFromParam"))
    '            oRpt.SetParameterValue("dateToParam", crPrm("dateToParam"))
    '            oRpt.SetParameterValue("BCOYID", crPrm("BCOYID"))

    '        Case "DOvsGRNByDept.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("dateFromParam", crPrm("dateFromParam"))
    '            oRpt.SetParameterValue("dateToParam", crPrm("dateToParam"))
    '            oRpt.SetParameterValue("buyerCompanyIDParam", crPrm("buyerCompanyIDParam"))
    '            oRpt.SetParameterValue("doVsGrnDayParam", crPrm("doVsGrnDayParam"))

    '        Case "monthlyConsumptionRpt2.rpt", "SummCostCenterByDept.rpt", "monthlyNonCapexExpenditureActualVsBudget.rpt", "monthlyExceptionExpendByCostCenter.rpt", "monthlyExpendComparePrevYrByCostCenter.rpt", "monthlyNonCapexDepartmentalSummaryExpenditure.rpt", "R1detailPaidInvoiceByGLCode-BR.rpt", "R2detailPaidInvoiceByGLCode-HO.rpt", "R4detailCommitedOrdersByGLCode-HQ.rpt", "ExpenseChargeOutToCostCentre.rpt", "eSystemMonthlyExpense.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("monthParam", crPrm("monthParam"))
    '            oRpt.SetParameterValue("yearParam", crPrm("yearParam"))
    '            oRpt.SetParameterValue("buyerCompanyIDParam", crPrm("buyerCompanyIDParam"))

    '        Case "R3detailCommitedOrdersByGLCode-BR.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("monthParam", crPrm("monthParam"))
    '            oRpt.SetParameterValue("yearParam", crPrm("yearParam"))
    '            oRpt.SetParameterValue("buyerCompanyIDParam", crPrm("buyerCompanyIDParam"))
    '            oRpt.SetParameterValue("BCOYID", crPrm("BCOYID"))

    '        Case "directChargeItem2.rpt", "monthlyInvoiceMIP2.rpt", "monthlyPOCummSpecProject2.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("monthParam", crPrm("monthParam"))
    '            oRpt.SetParameterValue("yearParam", crPrm("yearParam"))
    '            oRpt.SetParameterValue("companyIDParam", crPrm("companyIDParam"))

    '        Case "SupplierInApprovedList2.rpt", "SupplierNotInApprovedList2.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("BCOYID", crPrm("BCOYID"))

    '        Case "topSkuValues.rpt", "topSkuVolume.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("CurrencyCode", crPrm("CurrencyCode"))

    '        Case "allcompInvoiceSummaryReport2.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("buyerCompanyIDParam", crPrm("buyerCompanyIDParam"))
    '            oRpt.SetParameterValue("MonthFromParam", crPrm("MonthFromParam"))
    '            oRpt.SetParameterValue("MonthToParam", crPrm("MonthToParam"))
    '            oRpt.SetParameterValue("YearFromParam", crPrm("YearFromParam"))
    '            oRpt.SetParameterValue("YearToParam", crPrm("YearToParam"))

    '        Case "skuSummaryReport.rpt", "totalOfsupplier.rpt", "hubunspscsummary.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))

    '        Case "SummaryInvoicePendingFinalApproval.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("buyerCompanyIDParam", crPrm("buyerCompanyIDParam"))

    '        Case "AuditTrailProductActivityLog.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("companyIDParam", crPrm("companyIDParam"))

    '        Case "userDetailModificationAudit.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("companyIDParam", crPrm("companyIDParam"))
    '            oRpt.SetParameterValue("dateFromParam", crPrm("dateFromParam"))
    '            oRpt.SetParameterValue("dateToParam", crPrm("dateToParam"))
    '            oRpt.SetParameterValue("companyUserParam", crPrm("companyUserParam"))

    '        Case "UserInforModificationLog.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("companyIDParam", crPrm("companyIDParam"))
    '            oRpt.SetParameterValue("dateFromParam", crPrm("dateFromParam"))
    '            oRpt.SetParameterValue("dateToParam", crPrm("dateToParam"))

    '        Case "perfectOrderFulfillment.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("DateFrom", crPrm("dateFromParam"))
    '            oRpt.SetParameterValue("DateTo", crPrm("dateToParam"))
    '            oRpt.SetParameterValue("perfectDays", crPrm("perfectDays"))
    '            'oRpt.SetParameterValue("companyIDParam", crPrm("companyIDParam"))
    '            oRpt.SetParameterValue("BcoyID", crPrm("buyercompanyIDParam"))

    '        Case "vendorCycleTime.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("DateFrom", crPrm("dateFromParam"))
    '            oRpt.SetParameterValue("DateTo", crPrm("dateToParam"))
    '            oRpt.SetParameterValue("expectedCycleTime", crPrm("expectedCycleTime"))
    '            oRpt.SetParameterValue("BcoyID", crPrm("companyIDParam"))

    '        Case "R7SummaryApprovedPaidInvoicesSortedByBranch-BR.rpt", "R8SummaryApprovedPaidInvoicesSortedByBranch-HQ.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("dateFromParam", crPrm("dateFromParam"))
    '            oRpt.SetParameterValue("dateToParam", crPrm("dateToParam"))
    '            oRpt.SetParameterValue("itemIDParam", crPrm("itemIDParam"))
    '            oRpt.SetParameterValue("buyerCompanyIDParam", crPrm("buyerCompanyIDParam"))

    '        Case "PurchaseRequisitionDetailLogByPRPO.rpt", "PurchaseRequisitionDetailLogByPRStatus.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("companyIDParam", crPrm("companyIDParam"))
    '            oRpt.SetParameterValue("dateFromParam", crPrm("dateFromParam"))
    '            oRpt.SetParameterValue("dateToParam", crPrm("dateToParam"))

    '        Case "VendorSubcrpRenewal.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("dateToParam", crPrm("dateToParam"))

    '        Case "R9DetailPRReportSortByBranchCode-BR.rpt", "R10DetailPRReportSortByBranchCode-HQ.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("dateToParam", crPrm("dateToParam"))
    '            oRpt.SetParameterValue("buyerCompanyIDParam", crPrm("buyerCompanyIDParam"))

    '        Case "R13OpexPRPendingApprvByBranch-Branch.rpt", "R13OpexPRPendingApprvByBranch-Branch.rpt", "R13CapexPRPendingApprvByBranch-Branch.rpt", "R14OpexPRPendingApprvByBranch-HQ.rpt", "R14CapexPRPendingApprvByBranch-HQ.rpt", "R15OpexInvoicePendingApproval.rpt", "R15CapexInvoicePendingApproval.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("buyerCompanyIDParam", crPrm("buyerCompanyIDParam"))
    '            oRpt.SetParameterValue("FOFMID", crPrm("FOFMID"))

    '    End Select

    '    Me.CrystalReportViewer1.ReportSource = oRpt

    'End Sub

    'Private Sub BindReport()
    '    Dim crPrm As Hashtable

    '    crPrm = Session("Param")
    '    Dim crDatabase As CrystalDecisions.CrystalReports.Engine.Database
    '    Dim crTable As CrystalDecisions.CrystalReports.Engine.Table
    '    Dim dbConn As TableLogOnInfo = New TableLogOnInfo
    '    oRpt = New ReportDocument
    '    Dim arrTables As Object() = New Object(0) {}
    '    ReDim arrTables(20)

    '    oRpt.Load(Server.MapPath(rpt)) '("D:\Project\Shell Sarawak\eTravel\Report\viewtr.rpt")
    '    crDatabase = oRpt.Database
    '    crDatabase.Tables.CopyTo(arrTables, 0)
    '    crTable = DirectCast(arrTables(0), CrystalDecisions.CrystalReports.Engine.Table)
    '    dbConn = crTable.LogOnInfo
    '    dbConn.ConnectionInfo.ServerName = strServerName
    '    dbConn.ConnectionInfo.DatabaseName = strDBName
    '    dbConn.ConnectionInfo.UserID = strUN
    '    dbConn.ConnectionInfo.Password = strPWD
    '    crTable.ApplyLogOnInfo(dbConn)
    '    Select Case rpt
    '        Case "detailOfPOByCompany2.rpt", "detailOfPRByCompany.rpt"
    '            oRpt.SetParameterValue("buyerCompanyIDParam", crPrm("buyerCompanyIDParam"))
    '            oRpt.SetParameterValue("vendorCompanyIDParam", crPrm("vendorCompanyIDParam"))
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("dateFromParam", crPrm("dateFromParam"))
    '            oRpt.SetParameterValue("dateToParam", crPrm("dateToParam"))

    '        Case "outstandPOFrSuppl2.rpt", "outstandPRByItem2.rpt", "outstandGRNFrSupplHLB.rpt", "outstandGRNFrSuppl2.rpt", _
    '            "PurchaseToInvoiceCycleRpt2.rpt", "outstandPOFrSupplSeh2.rpt", "consumptionFromVendor.rpt", _
    '            "outstandingPoByDept.rpt", "outstandDOByDept.rpt", "consumptionByCategory.rpt", "PurchaseToInvoiceCycleByCategory.rpt", _
    '            "invoiceApproval20KTo100K.rpt", "invoiceApproval100KTo500K.rpt", "detailOfPOByPurchaseSite.rpt", _
    '            "outstandGRNByPurchaseSite.rpt", "outstandInvoiceByDept.rpt", "SummaryApprovedInvoicesByVendor.rpt", _
    '            "R5SummaryApprovedPaidInvoicesSortedByItem-BR.rpt", "R6SummaryApprovedPaidInvoicesSortedByItem-HQ.rpt", _
    '            "PurchaseToInvoiceCyclePrintStatSecurity-HQ.rpt", "PurchaseToInvoiceCyclePrintStatSecurity-BR.rpt", _
    '            "R11DetailOutstandGRNReprotSortedByBranch-BR.rpt", "R12DetailOutstandGRNReprotSortedByBranch-HQ.rpt"
    '            oRpt.SetParameterValue("buyerCompanyIDParam", crPrm("buyerCompanyIDParam"))
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("dateFromParam", crPrm("dateFromParam"))
    '            oRpt.SetParameterValue("dateToParam", crPrm("dateToParam"))

    '        Case "SummaryOfPOPRRFQByCompany2.rpt"
    '            oRpt.SetParameterValue("BCOYID", crPrm("buyerCompanyIDParam"))
    '            oRpt.SetParameterValue("vendorCompanyIDParam", crPrm("vendorCompanyIDParam"))
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("DateFrom", crPrm("dateFromParam"))
    '            oRpt.SetParameterValue("DateTo", crPrm("dateToParam"))

    '        Case "pmeExpenses2.rpt", "pmeExpensesDetail2.rpt"
    '            oRpt.SetParameterValue("companyIDParam", crPrm("companyIDParam"))
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("dateFromParam", crPrm("dateFromParam"))
    '            oRpt.SetParameterValue("dateToParam", crPrm("dateToParam"))

    '        Case "InvoiceSummaryBySupplier.rpt" ', "vendorRevenueForecast.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("dateFromParam", crPrm("dateFromParam"))
    '            oRpt.SetParameterValue("dateToParam", crPrm("dateToParam"))

    '            'Case "vendorRevenueForecast.rpt"
    '            '    oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            '    oRpt.SetParameterValue("dateFromParam", crPrm("dateFromParam"))
    '            '    oRpt.SetParameterValue("dateToParam", crPrm("dateToParam"))
    '            '    oRpt.SetParameterValue("monthParam", 0)
    '            '    oRpt.SetParameterValue("yearParam", 0)

    '        Case "PORaised.rpt", "PurchaseToGRNCycleOEFF.rpt"
    '            oRpt.SetParameterValue("BCOYID", crPrm("BCOYID"))
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("dateFromParam", crPrm("dateFromParam"))
    '            oRpt.SetParameterValue("dateToParam", crPrm("dateToParam"))

    '        Case "DOvsGRNByDept.rpt"
    '            oRpt.SetParameterValue("buyerCompanyIDParam", crPrm("buyerCompanyIDParam"))
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("dateFromParam", crPrm("dateFromParam"))
    '            oRpt.SetParameterValue("dateToParam", crPrm("dateToParam"))
    '            oRpt.SetParameterValue("doVsGrnDayParam", crPrm("doVsGrnDayParam"))

    '        Case "DebitNote.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("buyerCompanyIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("dateFromParam", crPrm("dateFromParam"))
    '            oRpt.SetParameterValue("dateToParam", crPrm("dateToParam"))

    '        Case "monthlyConsumptionRpt2.rpt", "SummCostCenterByDept.rpt", _
    '            "R3detailCommitedOrdersByGLCode-BR.rpt", "R4detailCommitedOrdersByGLCode-HQ.rpt", "ExpenseChargeOutToCostCentre.rpt", "eSystemMonthlyExpense.rpt", _
    '             "monthlyExceptionExpendByCostCenter.rpt", "monthlyExpendComparePrevYrByCostCenter.rpt", _
    '              "monthlyNonCapexDepartmentalSummaryExpenditure.rpt", "monthlyNonCapexExpenditureActualVsBudget.rpt"
    '            oRpt.SetParameterValue("buyerCompanyIDParam", crPrm("buyerCompanyIDParam"))
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("monthParam", crPrm("monthParam"))
    '            oRpt.SetParameterValue("yearParam", crPrm("yearParam"))

    '        Case "R1detailPaidInvoiceByGLCode-BR.rpt", "R2detailPaidInvoiceByGLCode-HO.rpt"
    '            oRpt.SetParameterValue("buyerCompanyIDParam", crPrm("buyerCompanyIDParam"))
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("monthParam", crPrm("monthParam"))
    '            oRpt.SetParameterValue("yearParam", crPrm("yearParam"))
    '            oRpt.SetParameterValue("InvDtFrom", crPrm("InvDtFrom"))
    '            oRpt.SetParameterValue("InvDtTo", crPrm("InvDtTo"))

    '        Case "directChargeItem2.rpt", "monthlyInvoiceMIP2.rpt", "monthlyPOCummSpecProject2.rpt"
    '            oRpt.SetParameterValue("companyIDParam", crPrm("companyIDParam"))
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("monthParam", crPrm("monthParam"))
    '            oRpt.SetParameterValue("yearParam", crPrm("yearParam"))

    '        Case "SupplierInApprovedList2.rpt", "SupplierNotInApprovedList2.rpt"
    '            oRpt.SetParameterValue("BCOYID", crPrm("BCOYID"))
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))

    '        Case "topSkuValues.rpt", "topSkuVolume.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("CurrencyCode", crPrm("CurrencyCode"))

    '            'Case "allcompInvoiceSummaryReport2.rpt"
    '            '    oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            '    oRpt.SetParameterValue("monthFromParam", crPrm("MonthFromParam"))
    '            '    oRpt.SetParameterValue("monthToParam", crPrm("MonthToParam"))
    '            '    oRpt.SetParameterValue("yearFromParam", crPrm("YearFromParam"))
    '            '    oRpt.SetParameterValue("yearToParam", crPrm("YearToParam"))

    '        Case "skuSummaryReport.rpt", "totalOfsupplier.rpt", "hubunspscsummary.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))

    '        Case "SummaryInvoicePendingFinalApproval.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("buyerCompanyIDParam", crPrm("buyerCompanyIDParam"))

    '        Case "AuditTrailProductActivityLog.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("companyIDParam", crPrm("companyIDParam"))

    '        Case "userDetailModificationAudit.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("companyIDParam", crPrm("companyIDParam"))
    '            oRpt.SetParameterValue("dateFromParam", crPrm("dateFromParam"))
    '            oRpt.SetParameterValue("dateToParam", crPrm("dateToParam"))
    '            oRpt.SetParameterValue("companyUserParam", crPrm("companyUserParam"))

    '        Case "UserInforModificationLog.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("companyIDParam", crPrm("companyIDParam"))
    '            oRpt.SetParameterValue("dateFromParam", crPrm("dateFromParam"))
    '            oRpt.SetParameterValue("dateToParam", crPrm("dateToParam"))

    '        Case "vendorCycleTime.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("expectedCycleTime", crPrm("expectedCycleTime"))
    '            oRpt.SetParameterValue("BcoyID", crPrm("buyercompanyIDParam"))
    '            oRpt.SetParameterValue("DateFrom", crPrm("dateFromParam"))
    '            oRpt.SetParameterValue("DateTo", crPrm("dateToParam"))

    '        Case "perfectOrderFulfillment.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("perfectDays", crPrm("perfectDays"))
    '            oRpt.SetParameterValue("BcoyID", crPrm("buyercompanyIDParam"))
    '            oRpt.SetParameterValue("DateFrom", crPrm("dateFromParam"))
    '            oRpt.SetParameterValue("DateTo", crPrm("dateToParam"))

    '        Case "R7SummaryApprovedPaidInvoicesSortedByBranch-BR.rpt", "R8SummaryApprovedPaidInvoicesSortedByBranch-HQ.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("buyerCompanyIDParam", crPrm("buyercompanyIDParam"))
    '            oRpt.SetParameterValue("dateFromParam", crPrm("dateFromParam"))
    '            oRpt.SetParameterValue("dateToParam", crPrm("dateToParam"))
    '            oRpt.SetParameterValue("ItemIDParam", crPrm("ItemIDParam"))

    '        Case "PurchaseRequisitionDetailLogByPRPO.rpt", "PurchaseRequisitionDetailLogByPRStatus.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("CompanyIDParam", crPrm("CompanyIDParam"))
    '            oRpt.SetParameterValue("dateFromParam", crPrm("dateFromParam"))
    '            oRpt.SetParameterValue("dateToParam", crPrm("dateToParam"))

    '            'Case "VendorSubcrpRenewal.rpt" , "supplierFacilityRenewalForm.rpt" , "salesOrderForm.rpt"
    '            '    oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            '    oRpt.SetParameterValue("monthParam", crPrm("monthParam"))
    '            '    oRpt.SetParameterValue("yearParam", crPrm("yearParam"))
    '            '    oRpt.SetParameterValue("dateToParam", crPrm("dateToParam"))

    '        Case "R9DetailPRReportSortByBranchCode-BR.rpt", "R10DetailPRReportSortByBranchCode-HQ.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("buyerCompanyIDParam", crPrm("buyerCompanyIDParam"))
    '            oRpt.SetParameterValue("dateToParam", crPrm("dateToParam"))

    '        Case "R13OpexPRPendingApprvByBranch-Branch.rpt", "R13CapexPRPendingApprvByBranch-Branch.rpt", "R14OpexPRPendingApprvByBranch-HQ.rpt", "R14CapexPRPendingApprvByBranch-HQ.rpt", "R15OpexInvoicePendingApproval.rpt", "R15CapexInvoicePendingApproval.rpt"
    '            oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            oRpt.SetParameterValue("buyerCompanyIDParam", crPrm("buyerCompanyIDParam"))
    '            oRpt.SetParameterValue("FOFMID", crPrm("FOFMID"))

    '            'Case "R3detailCommitedOrdersByGLCode-BR.rpt", "R4detailCommitedOrdersByGLCode-HQ.rpt", "ExpenseChargeOutToCostCentre.rpt", "eSystemMonthlyExpense.rpt"
    '            '    oRpt.SetParameterValue("userIDParam", crPrm("userIDParam"))
    '            '    oRpt.SetParameterValue("monthParam", crPrm("monthParam"))
    '            '    oRpt.SetParameterValue("yearParam", crPrm("yearParam"))
    '            '    oRpt.SetParameterValue("buyerCompanyIDParam", crPrm("buyerCompanyIDParam"))

    '    End Select

    '    Me.CrystalReportViewer1.ReportSource = oRpt

    'End Sub

    'Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
    '    oRpt.Dispose()
    '    oRpt.Close()
    '    CrystalReportViewer1.Dispose()
    'End Sub

    'Protected Sub CrystalReportViewer1_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles CrystalReportViewer1.Unload
    '    oRpt.Dispose()
    '    oRpt.Clone()
    '    oRpt.Close()
    '    CrystalReportViewer1.Dispose()
    'End Sub
End Class