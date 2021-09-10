Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient
Public Class PreviewITSlip1
    Inherits AgoraLegacy.AppBaseClass
    Dim dispatcher As New dispatcher

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PreviewITSlip()
    End Sub

    Private Sub PreviewITSlip()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String
        Dim intLocIndicator As Integer
        Dim strLoc As String
        Dim strSLoc As String
        Dim objInv As New Inventory

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request.QueryString("CoyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        objInv.GetLocationInfo(strLoc, strSLoc, intLocIndicator)
        objInv = Nothing

        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT IT_TRANS_REF_NO,IT_TRANS_DATE," _
                         & "IT_REF_NO,IT_REMARK,IM_ITEM_CODE,IT_INVENTORY_NAME AS IM_INVENTORY_NAME,IT_TRANS_QTY," _
                         & "IT_FRM_LOCATION_INDEX, a.LM_LOCATION AS FrLoc, a.LM_SUB_LOCATION AS FrSubLoc," _
                         & "IT_TO_LOCATION_INDEX, b.LM_LOCATION AS ToLoc, b.LM_SUB_LOCATION AS ToSubLoc " _
                         & "FROM inventory_trans " _
                         & "INNER JOIN inventory_mstr ON IM_INVENTORY_INDEX=IT_INVENTORY_INDEX " _
                         & "INNER JOIN location_mstr a ON IT_FRM_LOCATION_INDEX=a.LM_LOCATION_INDEX " _
                         & "INNER JOIN location_mstr b ON IT_TO_LOCATION_INDEX=b.LM_LOCATION_INDEX " _
                         & "WHERE IT_TRANS_REF_NO=@prmITNo " _
                         & "AND IT_TRANS_TYPE = 'TR' AND IM_COY_ID = @prmCoyID"
            End With

            da = New MySqlDataAdapter(cmd)
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmITNo", Request.QueryString("ITNo")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Request.QueryString("CoyID")))

            da.Fill(ds)

            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewITSlip_DataSetPreviewITSlip", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            If intLocIndicator > 1 Then 'Sub Location is defined
                localreport.ReportPath = dispatcher.direct("Report", "PreviewITSlip.rdlc", "Report") ' appPath & "DO\PreveiwDO-FTN.rdlc"

            Else
                localreport.ReportPath = dispatcher.direct("Report", "PreviewITSlip_WithoutSubLoc.rdlc", "Report") ' appPath & "DO\PreveiwDO-FTN.rdlc"

            End If
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
                    Case "pmfrmloc"
                        'par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings("ReportCoyLogoPath") & strImgSrc)
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strLoc)

                    Case "pmfrmsubloc"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strSLoc)

                    Case "pmtoloc"
                        'par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings("ReportCoyLogoPath") & strImgSrc)
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strLoc)

                    Case "pmtosubloc"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strSLoc)

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
            Dim strFileName As String = "IT_" & Request.QueryString(Trim("ITNo")) & "_" & Session("CompanyID") & "_" & Session("UserId") & ".pdf"
            strFileName = Replace(strFileName, "/", "^")

            Dim strTemp As String = dispatcher.direct("Report", "Temp", "Report")
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
End Class