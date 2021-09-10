Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient
Partial Public Class PreviewRODetails1

    Inherits AgoraLegacy.AppBaseClass
    Dim dispatcher As New dispatcher

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PreviewRODetails()
    End Sub

    Private Sub PreviewRODetails()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String
        Dim objDb As New EAD.DBCom


        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request.QueryString("CoyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))
        objDb = Nothing

        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT IROD_RO_LINE, IROD_RO_NO, POD_VENDOR_ITEM_CODE, POD_PRODUCT_DESC, POD_UOM, POD_MIN_PACK_QTY, " &
                            "POD_ORDERED_QTY, GD_RECEIVED_QTY, GD_REJECTED_QTY, IROD_QTY, IROD_REMARK, IROM_RO_NO, IROM_RO_DATE, " &
                            "GM_GRN_NO, GM_CREATED_DATE, POM_PO_NO, POM_CREATED_DATE, DOM_DO_NO, DOM_DO_DATE, GM_DATE_RECEIVED, " &
                            "(SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = DOM_S_COY_ID) AS VENDOR_NAME, " &
                            "IROL_LOT_QTY, DOL_LOT_NO, LM_LOCATION, IFNULL(LM_SUB_LOCATION, '-') AS LM_SUB_LOCATION " &
                            "FROM INVENTORY_RETURN_OUTWARD_MSTR, INVENTORY_RETURN_OUTWARD_DETAILS, " &
                            "INVENTORY_RETURN_OUTWARD_LOT, GRN_DETAILS, GRN_MSTR, DO_MSTR, PO_MSTR, PO_DETAILS, " &
                            "DO_LOT, LOCATION_MSTR " &
                            "WHERE IROM_RO_NO = IROD_RO_NO And IROM_RO_COY_ID = IROD_RO_COY_ID " &
                            "AND IROD_RO_NO = IROL_RO_NO AND IROD_RO_COY_ID = IROL_RO_COY_ID AND IROD_RO_LINE = IROL_RO_LINE " &
                            "AND IROD_PO_LINE = GD_PO_LINE AND IROM_GRN_NO = GD_GRN_NO AND IROD_RO_COY_ID = GD_B_COY_ID " &
                            "AND GD_B_COY_ID = GM_B_COY_ID AND GD_GRN_NO = GM_GRN_NO " &
                            "AND GM_DO_INDEX = DOM_DO_INDEX AND GM_PO_INDEX = POM_PO_INDEX " &
                            "AND POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = POD_COY_ID AND GD_PO_LINE = POD_PO_LINE " &
                            "AND IROL_LOT_INDEX = DOL_LOT_INDEX AND IROL_LOCATION_INDEX = LM_LOCATION_INDEX " &
                            "AND IROM_RO_NO = @prmRONo AND IROM_RO_COY_ID = @prmCoyID "

            End With

            da = New MySqlDataAdapter(cmd)
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Request.QueryString("CoyID")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmRONo", Request.QueryString("RO_NO")))
            da.Fill(ds)


            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewRO_DataSetRO", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            localreport.ReportPath = dispatcher.direct("Report", "PreviewRODetail.rdlc", "Report")
            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            TotalParameter = localreport.GetParameters.Count
            Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
            Dim strID As String = ""

            strID = System.Configuration.ConfigurationManager.AppSettings.Get("Env") & "ReportCoyLogoPath"
            For I = 0 To localreport.GetParameters.Count - 1
                GetParameter = localreport.GetParameters.Item(I).Name
                Select Case LCase(GetParameter)
                    Case "pmlogo"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings(strID) & strImgSrc)

                    Case "pmcoyname"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, Session("CompanyName").ToString)

                    Case Else
                End Select
            Next
            localreport.SetParameters(par)
            localreport.Refresh()

            Dim deviceInfo As String = _
                            "<DeviceInfo>" + _
                                "<OutputFormat>EMF</OutputFormat>" + _
                                "</DeviceInfo>"
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
            Dim strFileName As String = "RONo_" & Request.QueryString(Trim("RO_NO")) & "_" & Session("CompanyID") & "_" & Session("UserId") & ".pdf"
            strFileName = Replace(strFileName, "/", "^")

            Dim strTemp As String = dispatcher.direct("Report", "Temp", "Report")
            If Dir(strTemp, FileAttribute.Directory) = "" Then
                MkDir(strTemp)
            End If

            Dim fs As New FileStream(Server.MapPath("Temp/" & strFileName), FileMode.Create)
            'Dim fs As New FileStream(appPath & "Common\Report\Temp\" & strFileName, FileMode.Create)
            fs.Write(PDF, 0, PDF.Length)
            fs.Close()

            Dim strJScript As String
            strJScript = "<script language=javascript>"
            strJScript += "window.open('Temp/" & strFileName & "',null,'height=600, width=730,status= no, resizable= yes, scrollbars=yes, toolbar=no,location=center,menubar=no, top=10, left=100');"
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

End Class