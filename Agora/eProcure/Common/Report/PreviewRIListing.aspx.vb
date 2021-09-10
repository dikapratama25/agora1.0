Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient

Partial Public Class PreviewRIListing1

    Inherits AgoraLegacy.AppBaseClass
    Dim dispatcher As New dispatcher
    Dim strStatus As String
    Dim strRINo As String
    Dim strMRSNo As String
    Dim strStartDate As String
    Dim strEndDate As String



    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PreviewRIListing()

    End Sub

    Private Sub PreviewRIListing()
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
        Dim objDb As New EAD.DBCom
        'Dim strMRSNo, strSql, strStartDate, strStatus, strEndDate, strRINo, strTemp As String


        strRINo = Request.QueryString("RINO")
        strMRSNo = Request.QueryString("MRSNO")
        strStartDate = Request.QueryString("StartDate")
        strEndDate = Request.QueryString("EndDate")
        strStatus = Request.QueryString("Status")

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
                .CommandText = "SELECT IRIM_RI_NO, IRIM_RI_DATE, SUM(IRID_QTY) AS TQTY, IRIM_RI_REJECT_REMARK, " &
                     "(SELECT IRSM_IRS_ACCOUNT_CODE FROM INVENTORY_REQUISITION_SLIP_MSTR WHERE IRSM_IRS_COY_ID = IRIM_RI_COY_ID AND IRSM_IRS_NO = IRIM_IR_NO) AS ACCCODE, IRIM_IR_NO, " &
                     "(SELECT STATUS_REMARK FROM STATUS_MSTR WHERE STATUS_TYPE = 'RI' AND M.IRIM_RI_STATUS = STATUS_NO) AS IRIM_RI_STATUS, D.*, INVENTORY_MSTR.*, INVENTORY_DETAIL.*" &
                     "FROM INVENTORY_RETURN_INWARD_MSTR M INNER JOIN INVENTORY_RETURN_INWARD_DETAILS D " &
                     "INNER JOIN INVENTORY_RETURN_INWARD_LOT ON IRIL_RI_COY_ID = D.IRID_RI_COY_ID AND IRIL_RI_NO = D.IRID_RI_NO AND IRIL_RI_LINE = D.IRID_RI_LINE " &
                     "INNER JOIN INVENTORY_MSTR ON IM_COY_ID = M.IRIM_RI_COY_ID AND IM_INVENTORY_INDEX = D.IRID_INVENTORY_INDEX " &
                     "INNER JOIN INVENTORY_DETAIL ON ID_INVENTORY_INDEX = IM_INVENTORY_INDEX AND ID_LOCATION_INDEX = IRIL_LOCATION_INDEX " &
                     "WHERE M.IRIM_RI_COY_ID = D.IRID_RI_COY_ID AND M.IRIM_RI_NO = D.IRID_RI_NO AND M.IRIM_RI_COY_ID =@prmCoyID " &
                     "AND M.IRIM_CREATED_BY =@prmUserID "


                If strRINo <> "" Then
                    strTemp = Common.BuildWildCard(strRINo)
                    .CommandText &= " AND IRIM_RI_NO " & Common.ParseSQL(strTemp) & " "
                End If


                If strMRSNo <> "" Then
                    strTemp = Common.BuildWildCard(strMRSNo)
                    .CommandText &= " AND IRIM_IR_NO " & Common.ParseSQL(strTemp) & " "
                End If

                If strStartDate <> "" Then
                    .CommandText = .CommandText & " AND IRIM_RI_DATE >=" & Common.ConvertDate(strStartDate) & ""
                End If

                If strEndDate <> "" Then
                    .CommandText = .CommandText & " AND IRIM_RI_DATE <=" & Common.ConvertDate(strEndDate & " 23:59:59.000") & ""
                End If

                If strStatus <> "" Then
                    .CommandText = .CommandText & " AND IRIM_RI_STATUS IN (" & strStatus & ")" '" & Common.Parse(strStatus) & "'"
                End If

                .CommandText = .CommandText & " GROUP BY IRIM_RI_NO, IRIM_RI_DATE, IRIM_RI_STATUS, IRIM_IR_NO "






            End With

            da = New MySqlDataAdapter(cmd)
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Request.QueryString("CoyID")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmUserID", Request.QueryString("UserID")))
            da.Fill(ds)


            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewRIListing_DataSetRIListing", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            'localreport.ReportPath = appPath & "Report\PreviewWriteOffDetail.rdlc"
            localreport.ReportPath = dispatcher.direct("Report", "PreviewRIListing.rdlc", "Report")
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
            Dim strFileName As String = "RINo_" & Request.QueryString(Trim("RINO")) & "_" & Session("CompanyID") & "_" & Session("UserId") & ".pdf"
            strFileName = Replace(strFileName, "/", "^")

            strtemp = dispatcher.direct("Report", "Temp", "Report")
            If Dir(strTemp, FileAttribute.Directory) = "" Then
                MkDir(strTemp)
            End If
            'Dim fs As New FileStream(Server.MapPath("Temp/" & strFileName), FileMode.Create)
            Dim fs As New FileStream(appPath & "Common\Report\Temp\" & strFileName, FileMode.Create)
            'Dim fs As New FileStream(appPath & "Common/Inventory/WONo__beta_sk.PDF", FileMode.Create)
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

