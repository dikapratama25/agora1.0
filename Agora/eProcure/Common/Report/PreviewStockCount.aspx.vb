
Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient

Public Class PreviewStockCount1
    Inherits AgoraLegacy.AppBaseClass
    Dim dispatcher As New dispatcher

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PreviewStockCountList()
    End Sub

    Private Sub PreviewStockCountList()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String
        Dim strTemp As String
        'Dim strFreightAmount As String

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request.QueryString("CoyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT ID_INVENTORY_INDEX, IM_ITEM_CODE, IM_INVENTORY_NAME," _
                & "ID_LOCATION_INDEX, LM_LOCATION, LM_SUB_LOCATION, ID_INVENTORY_QTY, IM_COY_ID " _
                & "FROM inventory_detail " _
                & "INNER JOIN inventory_mstr ON ID_INVENTORY_INDEX=IM_INVENTORY_INDEX " _
                & "INNER JOIN location_mstr ON ID_LOCATION_INDEX=LM_LOCATION_INDEX " _
                & "WHERE IM_COY_ID=@prmCoyID"
                If Request.QueryString("ItemCode") <> "" Then
                    strTemp = Common.BuildWildCard(Request.QueryString("ItemCode"))
                    .CommandText = .CommandText & " AND IM_ITEM_CODE" & Common.ParseSQL(strTemp)
                End If

                If Request.QueryString("ItemName") <> "" Then
                    strTemp = Common.BuildWildCard(Request.QueryString("ItemName"))
                    .CommandText = .CommandText & " AND IM_INVENTORY_NAME" & Common.ParseSQL(strTemp)
                End If

                If Request.QueryString("Loc") <> "" And Request.QueryString("Loc") <> "---Select---" Then
                    .CommandText = .CommandText & " AND LM_LOCATION ='" & Common.Parse(Request.QueryString("Loc")) & "'"
                End If

                If Request.QueryString("SubLoc") <> "" And Request.QueryString("SubLoc") <> "---Select---" Then
                    .CommandText = .CommandText & " AND LM_SUB_LOCATION ='" & Common.Parse(Request.QueryString("SubLoc")) & "'"
                End If

                If Request.QueryString("QC") <> Nothing Then
                    .CommandText = .CommandText & " AND IM_IQC_IND ='" & Request.QueryString("QC") & "'"
                End If
                .CommandText = .CommandText & " ORDER BY " & Request.QueryString("SortedBy")
            End With

            da = New MySqlDataAdapter(cmd)
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Request.QueryString("CoyID")))
            da.Fill(ds)

            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewStockCount_DataSetStockCount", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            localreport.ReportPath = dispatcher.direct("Report", "PreviewInvStockCount.rdlc", "Report") ' appPath & "DO\PreveiwDO-FTN.rdlc"
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

            Dim deviceInfo As String =
                            "<DeviceInfo>" +
                                "  <OutputFormat>EMF</OutputFormat>" +
                                "</DeviceInfo>"
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
            Dim strFileName As String = "StockCount_" & Session("CompanyID") & "_" & Session("UserId") & "_" & Format(Now, "yyyyMMddHHmmss") & ".pdf"
            strFileName = Replace(strFileName, "/", "^")
            strTemp = dispatcher.direct("Report", "Temp", "Report")
            If Dir(strTemp, FileAttribute.Directory) = "" Then
                MkDir(strTemp)
            End If
            Dim fs As New FileStream(Server.MapPath("Temp/" & strFileName), FileMode.Create)
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

    'Private Sub PreviewStockCountList_Temp()
    '    Dim ds As New DataSet
    '    Dim conn As MySqlConnection = Nothing
    '    Dim cmd As MySqlCommand = Nothing
    '    Dim da As MySqlDataAdapter = Nothing
    '    Dim rdr As MySqlDataReader = Nothing
    '    Dim myConnectionString As String
    '    Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
    '    'Dim objFile As New FileManagement
    '    Dim strImgSrc As String
    '    Dim strFreightAmount As String

    '    'strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request.QueryString("SCoyID"), System.AppDomain.CurrentDomain.BaseDirectory & "images\")
    '    'strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request.QueryString("SCoyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

    '    Try
    '        myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

    '        conn = New MySqlConnection(myConnectionString)
    '        conn.Open()

    '        cmd = New MySqlCommand
    '        With cmd
    '            .Connection = conn
    '            .CommandType = CommandType.Text
    '            .CommandText = "SELECT ID_INVENTORY_INDEX, IM_ITEM_CODE, IM_INVENTORY_NAME," _
    '            & "ID_LOCATION_INDEX, LM_LOCATION, LM_SUB_LOCATION, ID_INVENTORY_QTY, IM_COY_ID " _
    '            & "FROM inventory_detail " _
    '            & "INNER JOIN inventory_mstr ON ID_INVENTORY_INDEX=IM_INVENTORY_INDEX " _
    '            & "INNER JOIN location_mstr ON ID_LOCATION_INDEX=LM_LOCATION_INDEX " _
    '            & "WHERE IM_COY_ID=@prmCoyID"
    '            If Request.QueryString("ItemCode") <> "" Then
    '                .CommandText = .CommandText & " AND IM_ITEM_CODE='" & Common.Parse(Request.QueryString("ItemCode")) & "'"
    '            End If

    '            If Request.QueryString("ItemName") <> "" Then
    '                .CommandText = .CommandText & " AND IM_INVENTORY_NAME LIKE '%" & Common.Parse(Request.QueryString("ItemName")) & "%'"
    '            End If

    '            If Request.QueryString("Loc") <> "" And Request.QueryString("Loc") <> "---Select---" Then
    '                .CommandText = .CommandText & " AND LM_LOCATION ='" & Common.Parse(Request.QueryString("Loc")) & "'"
    '            End If

    '            If Request.QueryString("SubLoc") <> "" And Request.QueryString("SubLoc") <> "---Select---" Then
    '                .CommandText = .CommandText & " AND LM_SUB_LOCATION ='" & Common.Parse(Request.QueryString("SubLoc")) & "'"
    '            End If
    '            .CommandText = .CommandText & " ORDER BY " & Request.QueryString("SortedBy")
    '        End With

    '        da = New MySqlDataAdapter(cmd)
    '        da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Request.QueryString("CoyID")))
    '        da.Fill(ds)

    '        Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewStockCount_DataSetStockCount", ds.Tables(0))
    '        Dim localreport As New LocalReport
    '        localreport.DataSources.Clear()
    '        localreport.DataSources.Add(rptDataSource)
    '        localreport.ReportPath = appPath & "Common\Report\PreviewInvStockCount.rdlc"
    '        localreport.Refresh()

    '        Dim deviceInfo As String = _
    '                        "<DeviceInfo>" + _
    '                            "  <OutputFormat>EMF</OutputFormat>" + _
    '                            "</DeviceInfo>"
    '        Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
    '        Dim strFileName As String = "StockCount_" & Session("CompanyID") & "_" & Session("UserId") & "_" & Format(Now, "yyyyMMddHHmmss") & ".pdf"
    '        Dim strTemp As String = appPath & "Common\Report\Temp"
    '        If Dir(strTemp, FileAttribute.Directory) = "" Then
    '            MkDir(strTemp)
    '        End If
    '        Dim fs As New FileStream(appPath & "Common\Report\Temp\" & strFileName, FileMode.Create)
    '        fs.Write(PDF, 0, PDF.Length)
    '        fs.Close()

    '        Dim strJScript As String
    '        strJScript = "<script language=javascript>"
    '        strJScript += "window.open('Temp/" & strFileName & "',null,'height=600, width=730,status= no, resizable= yes, scrollbars=yes, toolbar=no,location=center,menubar=no, top=10, left=100');"
    '        strJScript += "</script>"
    '        Response.Write(strJScript)

    '    Catch ex As Exception
    '    Finally
    '        cmd = Nothing
    '        If Not IsNothing(rdr) Then
    '            rdr.Close()
    '        End If
    '        If Not IsNothing(conn) Then
    '            If conn.State = ConnectionState.Open Then
    '                conn.Close()
    '            End If
    '        End If
    '        conn = Nothing
    '    End Try
    'End Sub
End Class