Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient
Public Class PreviewCostListingSEH
    Inherits AgoraLegacy.AppBaseClass
    Dim dispatcher As New dispatcher
    Dim strStartDate As String
    Dim strEndDate As String

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PreviewCostListing()
    End Sub
    Private Sub PreviewCostListing()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc, strTemp As String

        strStartDate = Request.QueryString("strStartDate")
        strEndDate = Request.QueryString("strEndDate")

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request.QueryString("CoyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT IC_COST_DATE, IC_INVENTORY_TYPE, IM_ITEM_CODE, IM_INVENTORY_NAME, IC_COST_OPEN_QTY, IC_COST_OPEN_UPRICE, " _
                    & "IC_COST_OPEN_COST, IC_INVENTORY_TYPE, IC_COST_CLOSE_QTY, IC_COST_CLOSE_UPRICE, IC_COST_CLOSE_COST, " _
                    & "CASE WHEN (IC_INVENTORY_TYPE = 'GRN' OR IC_INVENTORY_TYPE= 'IIC' OR IC_INVENTORY_TYPE = 'RI' OR IC_INVENTORY_TYPE = 'WOC') " _
                    & "THEN IC_COST_QTY ELSE 0 END AS RECEIVED_QTY, " _
                    & "CASE WHEN (IC_INVENTORY_TYPE = 'GRN' OR IC_INVENTORY_TYPE= 'IIC' OR IC_INVENTORY_TYPE = 'RI' OR IC_INVENTORY_TYPE = 'WOC') " _
                    & "THEN IC_COST_UPRICE ELSE 0 END AS RECEIVED_UPRICE, " _
                    & "CASE WHEN (IC_INVENTORY_TYPE = 'GRN' OR IC_INVENTORY_TYPE= 'IIC' OR IC_INVENTORY_TYPE = 'RI' OR IC_INVENTORY_TYPE = 'WOC') " _
                    & "THEN IC_COST_COST ELSE 0 END AS RECEIVED_COST, " _
                    & "CASE WHEN (IC_INVENTORY_TYPE = 'II' OR IC_INVENTORY_TYPE= 'RO' OR IC_INVENTORY_TYPE = 'WO') " _
                    & "THEN IC_COST_QTY ELSE 0 END AS ISSUED_QTY, " _
                    & "CASE WHEN (IC_INVENTORY_TYPE = 'II' OR IC_INVENTORY_TYPE= 'RO' OR IC_INVENTORY_TYPE = 'WO') " _
                    & "THEN IC_COST_UPRICE ELSE 0 END AS ISSUED_UPRICE, " _
                    & "CASE WHEN (IC_INVENTORY_TYPE = 'II' OR IC_INVENTORY_TYPE= 'RO' OR IC_INVENTORY_TYPE = 'WO') " _
                    & "THEN IC_COST_COST ELSE 0 END AS ISSUED_COST " _
                    & "FROM inventory_cost " _
                    & "INNER JOIN inventory_mstr ON IC_inventory_index = IM_INVENTORY_INDEX " _
                    & "WHERE IM_COY_ID=@prmCoyID "


                If Request.QueryString("ItemCode") <> "" Then
                    strTemp = Common.BuildWildCard(Request.QueryString("ItemCode"))
                    .CommandText = .CommandText & " AND IM_ITEM_CODE" & Common.ParseSQL(strTemp)
                End If

                If Request.QueryString("ItemName") <> "" Then
                    strTemp = Common.BuildWildCard(Request.QueryString("ItemName"))
                    .CommandText = .CommandText & " AND IM_INVENTORY_NAME" & Common.ParseSQL(strTemp)
                End If

                If strStartDate <> "" Then
                    .CommandText = .CommandText & " AND IC_COST_DATE >=" & Common.ConvertDate(strStartDate)
                End If

                If strEndDate <> "" Then
                    .CommandText = .CommandText & " AND IC_COST_DATE <=" & Common.ConvertDate(strEndDate & " 23:59:59.000")
                End If

                If Request.QueryString("strInvType") <> "" Then
                    .CommandText = .CommandText & " AND IC_INVENTORY_TYPE='" & Common.Parse(Request.QueryString("strInvType")) & "'"
                End If


            End With
            da = New MySqlDataAdapter(cmd)
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Request.QueryString("CoyID")))
            da.Fill(ds)

            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewCostListing_DataSetCostListing", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            localreport.ReportPath = dispatcher.direct("Report", "PreviewCostList.rdlc", "Report") ' appPath & "DO\PreveiwDO-FTN.rdlc"
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
                                "  <OutputFormat>EMF</OutputFormat>" + _
                                "</DeviceInfo>"
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
            Dim strFileName As String = "InvList_" & Session("CompanyID") & "_" & Session("UserId") & "_" & Format(Now, "yyyyMMddHHmmss") & ".pdf"
            strFileName = Replace(strFileName, "/", "^")
            strTemp = dispatcher.direct("Report", "Temp", "Report")
            If Dir(strTemp, FileAttribute.Directory) = "" Then
                MkDir(strTemp)
            End If
            Dim fs As New FileStream(Server.MapPath("Temp/" & strFileName), FileMode.Create)
            'Dim fs As New FileStream(strTemp & "\" & strFileName, FileMode.Create)
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


    