Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient

Partial Public Class InterfaceCodeAudit
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim strFrm As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        strNewCSS = "true"
        MyBase.Page_Load(sender, e)
        ''SetGridProperty(dtgInterfaceCode)
        If Not IsPostBack Then
            GenerateTab()
        End If
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        If strFrm <> "ItemCat" Then
            Session("w_InterfaceCode_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                                      "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Interface", "InterfaceCode.aspx", "pageid=" & strPageId) & """><span>Interface Code</span></a></li>" & _
                                      "<li><div class=""space""></div></li>" & _
                                      "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Interface", "InterfaceCodeAudit.aspx", "pageid=" & strPageId) & """><span>Audit</span></a></li>" & _
                                      "<li><div class=""space""></div></li>" & _
                                      "</ul><div></div></div>"
        Else
            Session("w_InterfaceCode_tabs") = Nothing
        End If
    End Sub

    Private Sub ExportToPDF()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String
        Dim strUserId As String
        Dim strCoyName As String
        Dim strBeginDate As String
        Dim strEndDate As String
        Dim strStart As String
        Dim strEnd As String
        Dim strFileName As String = ""
        Dim s1 As String = ""
        Dim s2 As String = ""
        Dim strTitle As String = ""

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyID"), dDispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))
        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                '.CommandText = " SELECT DATE_FORMAT(AU_DATE,'%d/%m/%Y %T') AS 'Action Date', " _
                '         & "CASE WHEN AU_ACTION = 'N' THEN 'Add' ELSE 'Modify'	END AS 'Action Type', " _
                '         & "AU_BR_CODE AS 'Branch Code',AU_GL_CODE AS 'BR GL Code', AU_CC AS 'Cost Center', concat('=""', AU_CC_DESC, '""') AS 'Cost Center Description', " _
                '         & "AU_IM_MAPPING_CODE AS 'Interface Code' " _
                '         & "FROM(au_interface_mapping) " _
                '         & "WHERE AU_IM_COY_ID=@coyID "
                .CommandText = " SELECT AU_IM_INTERFACE_INDEX, AU_DATE, AU_ACTION, AU_BR_CODE, AU_GL_CODE, AU_CC, AU_CC_DESC, AU_IM_MAPPING_CODE, AU_LOG_INDEX " _
                            & "FROM au_interface_mapping " _
                            & "INNER JOIN user_mstr " _
                            & "ON au_interface_mapping.AU_USER = user_mstr.UM_USER_ID AND user_mstr.UM_COY_ID = AU_IM_COY_ID " _
                            & "WHERE AU_IM_COY_ID=@coyID "
                If txtDateFr.Text <> "" Then
                    strBeginDate = Format(CDate(Me.txtDateFr.Text), "yyyy-MM-dd" & " " & "00:00:00")
                    strStart = Format(CDate(Me.txtDateFr.Text), "ddMMMyyyy")
                    s1 = Format(CDate(Me.txtDateFr.Text), "dd MMM yyyy")
                    .CommandText &= "AND AU_DATE>='" & strBeginDate & "' "
                End If

                If txtDateTo.Text <> "" Then
                    strEndDate = Format(CDate(Me.txtDateTo.Text), "yyyy-MM-dd" & " " & "23:59:59")
                    strEnd = Format(CDate(Me.txtDateTo.Text), "ddMMMyyyy")
                    s2 = Format(CDate(Me.txtDateTo.Text), "dd MMM yyyy")
                    .CommandText &= "AND AU_DATE <='" & strEndDate & "' "
                End If

                If txtBranchCode.Text <> "" Then
                    .CommandText &= "AND AU_BR_CODE LIKE '%" & txtBranchCode.Text & "%' "
                End If

                If txtGLCode.Text <> "" Then
                    .CommandText &= "AND AU_GL_CODE LIKE '%" & txtGLCode.Text & "%' "
                End If

                If txtCostCenter.Text <> "" Then
                    .CommandText &= "AND AU_CC LIKE '%" & txtCostCenter.Text & "%' "
                End If

                If txtInterfaceCode.Text <> "" Then
                    .CommandText &= "AND AU_IM_MAPPING_CODE LIKE '%" & txtInterfaceCode.Text & "%' "
                End If

                .CommandText &= "ORDER BY AU_LOG_INDEX"
            End With

            da = New MySqlDataAdapter(cmd)

            da.SelectCommand.Parameters.Add(New MySqlParameter("@coyID", Session("CompanyID")))
            da.Fill(ds)

            If ds.Tables(0).Rows.Count = 0 Then
                AgoraLegacy.Common.NetMsgbox(Me, MsgNoRecord)
                Exit Sub
            End If

            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewInterfaceCodeAuditTrail_DataSetInterfaceCodeAudit", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            'localreport.ReportPath = dDispatcher.direct("Report", "PreviewInterfaceCodeAuditTrail.rdlc", "Report") ' Server.MapPath("POSummary_pdf.rdlc")  'dispatcher.direct("Report", "POSummary_pdf.rdlc")
            localreport.ReportPath = appPath & "Common\Report\PreviewInterfaceCodeAuditTrail.rdlc"
            localreport.EnableExternalImages = True

            If strStart = "" Then
                If strEnd = "" Then

                Else
                    strTitle = "TILL " & s2
                End If
            Else
                If strEnd = "" Then
                    strTitle = "FROM " & s1
                Else
                    strTitle = "FROM " & s1 & " TO " & s2
                End If
            End If

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
                    Case "prmdate"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strTitle)

                        'Case "prmgroupcode"
                        '    par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, txtCode.Text)

                    Case "prmlogo"
                        'par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings("ReportCoyLogoPath") & strImgSrc)
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings(strID) & strImgSrc)

                    Case Else
                End Select
            Next
            localreport.SetParameters(par)
            localreport.Refresh()

            Dim deviceInfo As String =
                "<DeviceInfo>" +
                    "  <OutputFormat>EMF</OutputFormat>" +
                    "</DeviceInfo>"
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds

            strFileName = "InterfaceMappingAuditTrail.pdf"
            'Return PDF
            Me.Response.Clear()
            Me.Response.ContentType = "application/pdf"
            Me.Response.AddHeader("Content-disposition", "attachment; filename=" & strFileName)
            Me.Response.BinaryWrite(PDF)
            Me.Response.End()

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

    Private Sub ExportToExcel()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim strBeginDate As String
        Dim strEndDate As String
        Dim strStart As String
        Dim strEnd As String = ""
        Dim strStartReport As String
        Dim strEndReport As String = ""
        Dim strFileName As String = ""
        Dim strDate As String = ""

        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";"
            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = " SELECT DATE_FORMAT(AU_DATE,'%d/%m/%Y %T') AS 'Action Date', " _
                           & "CASE WHEN AU_ACTION = 'N' THEN 'Add' ELSE 'Modify'	END AS 'Action Type', " _
                           & "AU_BR_CODE AS 'Branch Code',AU_GL_CODE AS 'BR GL Code', AU_CC AS 'Cost Center', concat('=""', AU_CC_DESC, '""') AS 'Cost Center Description', " _
                           & "AU_IM_MAPPING_CODE AS 'Interface Code' " _
                           & "FROM(au_interface_mapping) " _
                           & "WHERE AU_IM_COY_ID='" & Session("CompanyID") & "' "
                If txtDateFr.Text <> "" Then
                    strBeginDate = Format(CDate(Me.txtDateFr.Text), "yyyy-MM-dd" & " " & "00:00:00")
                    strStart = Format(CDate(Me.txtDateFr.Text), "ddMMMyyyy")
                    strStartReport = Format(CDate(Me.txtDateFr.Text), "dd MMM yyyy")
                    .CommandText &= "AND AU_DATE>='" & strBeginDate & "' "
                End If

                If txtDateTo.Text <> "" Then
                    strEndDate = Format(CDate(Me.txtDateTo.Text), "yyyy-MM-dd" & " " & "23:59:59")
                    strEnd = Format(CDate(Me.txtDateTo.Text), "ddMMMyyyy")
                    strEndReport = Format(CDate(Me.txtDateTo.Text), "dd MMM yyyy")
                    .CommandText &= "AND AU_DATE <='" & strEndDate & "' "
                End If

                If txtBranchCode.Text <> "" Then
                    .CommandText &= "AND AU_BR_CODE LIKE '%" & txtBranchCode.Text & "%' "
                End If

                If txtGLCode.Text <> "" Then
                    .CommandText &= "AND AU_GL_CODE LIKE '%" & txtGLCode.Text & "%' "
                End If

                If txtCostCenter.Text <> "" Then
                    .CommandText &= "AND AU_CC LIKE '%" & txtCostCenter.Text & "%' "
                End If

                If txtInterfaceCode.Text <> "" Then
                    .CommandText &= "AND AU_IM_MAPPING_CODE LIKE '%" & txtInterfaceCode.Text & "%' "
                End If

                .CommandText &= "ORDER BY AU_LOG_INDEX"
            End With

            da = New MySqlDataAdapter(cmd)
            da.Fill(ds)

            If ds.Tables(0).Rows.Count = 0 Then
                AgoraLegacy.Common.NetMsgbox(Me, MsgNoRecord)
                Exit Sub
            End If

            If strStart = "" Then
                If strEnd = "" Then

                Else
                    strDate = "(Till" & strEnd & ")"
                End If
            Else
                If strEnd = "" Then
                    strDate = "(From" & strStart & ")"
                Else
                    strDate = "(" & strStart & " - " & strEnd & ")"
                End If
            End If

            strFileName = "InterfaceCodeAuditTrail" & strDate & ".xls"
            Dim attachment As String = "attachment;filename=" & strFileName
            Response.ClearContent()
            Response.AddHeader("Content-Disposition", attachment)
            Response.ContentType = "application/vnd.ms-excel"

            Response.Write("Interface Code Audit Report")

            If strStart <> "" Then
                Response.Write(" From ")
                Response.Write(strStartReport)
            End If

            If strEnd <> "" Then
                Response.Write(" Till ")
                Response.Write(strEndReport)
            End If

            Response.Write(vbCrLf)
            Response.Write(vbCrLf)

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

    Private Sub cmdView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdView.Click
        Page.DataBind()
        If cboReportType.SelectedValue = "Excel" Then
            ExportToExcel()

        ElseIf cboReportType.SelectedValue = "PDF" Then
            ExportToPDF()
        End If
    End Sub
End Class