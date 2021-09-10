Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient
Public Class PreviewDO1
    Inherits AgoraLegacy.AppBaseClass
    Dim dispatcher As New dispatcher
    Dim objDb As New EAD.DBCom

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PreviewDO()
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
        Dim strImgSrc, strItemType, strDelCode, strSql As String
        Dim strFreightAmount As String

        'strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request.QueryString("SCoyID"), System.AppDomain.CurrentDomain.BaseDirectory & "images\")
        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request.QueryString("SCoyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        'Check buyer 
        strSql = "SELECT '*' FROM COMPANY_DELIVERY_TERM WHERE CDT_COY_ID =" &
                "(SELECT POM_B_COY_ID FROM DO_MSTR, PO_MSTR WHERE DOM_PO_INDEX=POM_PO_INDEX AND DOM_DO_NO = '" & Request.QueryString("DONo") & "' AND DOM_S_COY_ID = '" & Request.QueryString("SCoyID") & "') "
        strDelCode = objDb.GetVal(strSql)

        strSql = "SELECT IFNULL(POD_ITEM_TYPE,'') FROM DO_MSTR, PO_MSTR, PO_DETAILS " &
                "WHERE DOM_PO_INDEX = POM_PO_INDEX AND POD_PO_NO = POM_PO_NO AND POD_COY_ID = POM_B_COY_ID " &
                "AND DOM_DO_NO = '" & Request.QueryString("DONo") & "' AND DOM_S_COY_ID = '" & Request.QueryString("SCoyID") & "' "
        strItemType = objDb.GetVal(strSql)
        Try

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                '.CommandText = "SELECT *, (SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_MSTR.POM_S_STATE) AND (CODE_CATEGORY = 's') AND (CODE_VALUE = PO_DETAILS.POD_D_COUNTRY)) AS SupplierAddrState," _
                '        & "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_MSTR.POM_S_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS SupplierAddrCtry, " _
                '        & "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_MSTR.POM_B_STATE) AND (CODE_CATEGORY = 's') AND (CODE_VALUE = PO_DETAILS.POD_D_COUNTRY)) AS BillAddrState, " _
                '        & "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_MSTR.POM_B_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS BillAddrCtry," _
                '        & "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_DETAILS.POD_D_STATE) AND (CODE_CATEGORY = 's') AND (CODE_VALUE = PO_DETAILS.POD_D_COUNTRY)) AS DelvAddrState, " _
                '        & "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_DETAILS.POD_D_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS DelvAddrCtry," _
                '        & "(SELECT CM_COY_NAME FROM COMPANY_MSTR AS b WHERE (CM_COY_ID = PO_MSTR.POM_B_COY_ID)) AS BuyerCompanyName " _
                '        & "FROM PO_MSTR INNER JOIN PO_DETAILS ON PO_MSTR.POM_PO_NO = PO_DETAILS.POD_PO_NO AND PO_MSTR.POM_B_COY_ID = PO_DETAILS.POD_COY_ID INNER JOIN DO_MSTR ON PO_MSTR.POM_PO_INDEX = DO_MSTR.DOM_PO_INDEX INNER JOIN DO_DETAILS ON DO_MSTR.DOM_DO_NO = DO_DETAILS.DOD_DO_NO AND DO_MSTR.DOM_S_COY_ID = DO_DETAILS.DOD_S_COY_ID AND PO_DETAILS.POD_PO_LINE = DO_DETAILS.DOD_PO_LINE INNER JOIN COMPANY_MSTR ON PO_MSTR.POM_S_COY_ID = COMPANY_MSTR.CM_COY_ID " _
                '        & "WHERE (PO_MSTR.POM_S_COY_ID = @prmCoyID) AND (DO_MSTR.DOM_DO_NO = @prmDONo)"
                If strItemType = "ST" And strDelCode <> "" Then
                    .CommandText = "SELECT *, (SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_MSTR.POM_S_STATE) " &
                                "AND (CODE_CATEGORY = 's') AND (CODE_VALUE = PO_DETAILS.POD_D_COUNTRY)) AS SupplierAddrState, " &
                                "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_MSTR.POM_S_COUNTRY) AND (CODE_CATEGORY = 'ct')) " &
                                "AS SupplierAddrCtry, (SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_MSTR.POM_B_STATE) " &
                                "AND (CODE_CATEGORY = 's') AND (CODE_VALUE = PO_DETAILS.POD_D_COUNTRY)) AS BillAddrState, " &
                                "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_MSTR.POM_B_COUNTRY) AND (CODE_CATEGORY = 'ct')) " &
                                "AS BillAddrCtry,(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_DETAILS.POD_D_STATE) " &
                                "AND (CODE_CATEGORY = 's') AND (CODE_VALUE = PO_DETAILS.POD_D_COUNTRY)) AS DelvAddrState, " &
                                "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_DETAILS.POD_D_COUNTRY) AND (CODE_CATEGORY = 'ct')) " &
                                "AS DelvAddrCtry,(SELECT CM_COY_NAME FROM COMPANY_MSTR AS b WHERE (CM_COY_ID = PO_MSTR.POM_B_COY_ID)) " &
                                "AS BuyerCompanyName, " &
                                "(SELECT IFNULL(CONCAT(CDT_DEL_CODE, ' (', CDT_DEL_NAME, ')'),'') FROM COMPANY_DELIVERY_TERM WHERE CDT_COY_ID=PO_MSTR.POM_B_COY_ID AND CDT_DEL_CODE=PO_MSTR.POM_DEL_CODE) AS DeliveryTerm, " &
                                "(SELECT CM_BUSINESS_REG_NO FROM COMPANY_MSTR WHERE po_mstr.POM_S_COY_ID = company_mstr.CM_COY_ID) AS CM_BUSINESS_REG_NO," &
                                "(SELECT CM_PHONE FROM COMPANY_MSTR WHERE po_mstr.POM_S_COY_ID = company_mstr.CM_COY_ID) AS CM_PHONE," &
                                "(SELECT CM_EMAIL FROM COMPANY_MSTR WHERE po_mstr.POM_S_COY_ID = company_mstr.CM_COY_ID) AS CM_EMAIL," &
                                "DATEDIFF(DOL_DO_EXP_DT, DOL_DO_MANU_DT) AS DOL_LIFETIME " &
                                "FROM PO_MSTR " &
                                "INNER JOIN PO_DETAILS ON PO_MSTR.POM_PO_NO = PO_DETAILS.POD_PO_NO " &
                                "AND PO_MSTR.POM_B_COY_ID = PO_DETAILS.POD_COY_ID " &
                                "INNER JOIN DO_MSTR ON PO_MSTR.POM_PO_INDEX = DO_MSTR.DOM_PO_INDEX " &
                                "INNER JOIN DO_DETAILS ON DO_MSTR.DOM_DO_NO = DO_DETAILS.DOD_DO_NO " &
                                "AND DO_MSTR.DOM_S_COY_ID = DO_DETAILS.DOD_S_COY_ID AND PO_DETAILS.POD_PO_LINE = DO_DETAILS.DOD_PO_LINE " &
                                "LEFT JOIN pr_mstr ON POD_PR_INDEX=PRM_PR_Index AND POD_COY_ID=PRM_COY_ID " &
                                "LEFT JOIN DO_LOT ON DOL_COY_ID=POM_S_COY_ID AND DOL_DO_NO=DOM_DO_NO AND DOL_ITEM_CODE=POD_VENDOR_ITEM_CODE " &
                                "WHERE (po_mstr.POM_S_COY_ID = @prmCoyID) AND (do_mstr.DOM_DO_NO = @prmDONo) "
                Else
                    .CommandText = "SELECT *, (SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_MSTR.POM_S_STATE) " _
                                & "AND (CODE_CATEGORY = 's') AND (CODE_VALUE = PO_DETAILS.POD_D_COUNTRY)) AS SupplierAddrState, " _
                                & "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_MSTR.POM_S_COUNTRY) AND (CODE_CATEGORY = 'ct')) " _
                                & "AS SupplierAddrCtry, (SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_MSTR.POM_B_STATE) " _
                                & "AND (CODE_CATEGORY = 's') AND (CODE_VALUE = PO_DETAILS.POD_D_COUNTRY)) AS BillAddrState, " _
                                & "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_MSTR.POM_B_COUNTRY) AND (CODE_CATEGORY = 'ct')) " _
                                & "AS BillAddrCtry,(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_DETAILS.POD_D_STATE) " _
                                & "AND (CODE_CATEGORY = 's') AND (CODE_VALUE = PO_DETAILS.POD_D_COUNTRY)) AS DelvAddrState, " _
                                & "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_DETAILS.POD_D_COUNTRY) AND (CODE_CATEGORY = 'ct')) " _
                                & "AS DelvAddrCtry,(SELECT CM_COY_NAME FROM COMPANY_MSTR AS b WHERE (CM_COY_ID = PO_MSTR.POM_B_COY_ID)) " _
                                & "AS BuyerCompanyName, " _
                                & "(SELECT IFNULL(CONCAT(CDT_DEL_CODE, ' (', CDT_DEL_NAME, ')'),'') FROM COMPANY_DELIVERY_TERM WHERE CDT_COY_ID=PO_MSTR.POM_B_COY_ID AND CDT_DEL_CODE=PO_MSTR.POM_DEL_CODE) AS DeliveryTerm, " _
                                & "(SELECT CM_BUSINESS_REG_NO FROM COMPANY_MSTR WHERE po_mstr.POM_S_COY_ID = company_mstr.CM_COY_ID) AS CM_BUSINESS_REG_NO," _
                                & "(SELECT CM_PHONE FROM COMPANY_MSTR WHERE po_mstr.POM_S_COY_ID = company_mstr.CM_COY_ID) AS CM_PHONE," _
                                & "(SELECT CM_EMAIL FROM COMPANY_MSTR WHERE po_mstr.POM_S_COY_ID = company_mstr.CM_COY_ID) AS CM_EMAIL " _
                                & "FROM PO_MSTR " _
                                & "INNER JOIN PO_DETAILS ON PO_MSTR.POM_PO_NO = PO_DETAILS.POD_PO_NO " _
                                & "AND PO_MSTR.POM_B_COY_ID = PO_DETAILS.POD_COY_ID " _
                                & "INNER JOIN DO_MSTR ON PO_MSTR.POM_PO_INDEX = DO_MSTR.DOM_PO_INDEX " _
                                & "INNER JOIN DO_DETAILS ON DO_MSTR.DOM_DO_NO = DO_DETAILS.DOD_DO_NO " _
                                & "AND DO_MSTR.DOM_S_COY_ID = DO_DETAILS.DOD_S_COY_ID AND PO_DETAILS.POD_PO_LINE = DO_DETAILS.DOD_PO_LINE " _
                                & "LEFT JOIN pr_mstr ON POD_PR_INDEX=PRM_PR_Index AND POD_COY_ID=PRM_COY_ID " _
                                & "WHERE (po_mstr.POM_S_COY_ID = @prmCoyID) AND (do_mstr.DOM_DO_NO = @prmDONo)"
                End If


            End With

            da = New MySqlDataAdapter(cmd)
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Request.QueryString("SCoyID")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmDONo", Me.Request.QueryString("DONo")))

            da.Fill(ds)
            If IsDBNull(ds.Tables(0).Rows(0).Item("DOM_FREIGHT_AMT")) Then
                strFreightAmount = ""
            Else
                strFreightAmount = ds.Tables(0).Rows(0).Item("DOM_FREIGHT_AMT")
            End If

            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewDO_DataSetPreviewDO", ds.Tables(0))
            Dim localreport As New LocalReport

            'Michelle (30/3/2012) - Issue 1566
            'To check whether the PO was raised from PR(s)
            'Dim strSQL As String = "SELECT * FROM pr_details INNER JOIN pr_mstr ON PRD_PR_NO = PRM_PR_NO " _
            '                        & "INNER JOIN po_mstr ON POM_PO_NO = PRD_CONVERT_TO_DOC AND POM_B_COY_ID = PRD_COY_ID " _
            '                        & "WHERE PRD_CONVERT_TO_IND = 'PO' AND PRD_CONVERT_TO_DOC ='" & Request.QueryString("PO_NO") & "' " _
            '                        & "AND POM_S_COY_ID ='" & Request.QueryString("SCoyID") & "'"

            'If objDb.Exist(strSQL) > 0 Then
            '    localreport.ReportPath = dispatcher.direct("Report", "PreviewDO.rdlc", "Report") ' appPath & "DO\PreveiwDO-FTN.rdlc"
            'Else
            '    localreport.ReportPath = dispatcher.direct("Report", "PreveiwDO-FTN.rdlc", "Report") ' appPath & "DO\PreveiwDO-FTN.rdlc"
            'End If
            If strItemType = "ST" And strDelCode <> "" Then
                localreport.ReportPath = dispatcher.direct("Report", "PreveiwDO-FTN-SEH.rdlc", "Report") ' appPath & "DO\PreveiwDO-FTN.rdlc"
            Else
                If Common.parseNull(ds.Tables(0).Rows(0)("DeliveryTerm")) <> "" Then
                    localreport.ReportPath = dispatcher.direct("Report", "PreveiwDO-FTN-DT.rdlc", "Report") ' appPath & "DO\PreveiwDO-FTN.rdlc"
                Else
                    localreport.ReportPath = dispatcher.direct("Report", "PreveiwDO-FTN.rdlc", "Report") ' appPath & "DO\PreveiwDO-FTN.rdlc"
                End If

            End If

            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            'localreport.ReportPath = dispatcher.direct("Report", "PreveiwDO-FTN.rdlc", "Report") ' appPath & "DO\PreveiwDO-FTN.rdlc"
            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            TotalParameter = localreport.GetParameters.Count
            Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
            'Dim paramlist As New Generic.List(Of ReportParameter)
            Dim strID As String = ""

            strID = System.Configuration.ConfigurationManager.AppSettings.Get("Env") & "ReportCoyLogoPath"
            For I = 0 To localreport.GetParameters.Count - 1
                GetParameter = localreport.GetParameters.Item(I).Name
                Select Case LCase(GetParameter)
                    Case "par1"
                        'par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings("ReportCoyLogoPath") & strImgSrc)
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings(strID) & strImgSrc)

                    Case "freightamt"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strFreightAmount)

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
            Dim strFileName As String = "DO_" & Request.QueryString(Trim("DONo")) & "_" & Session("CompanyID") & "_" & Session("UserId") & ".pdf"
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
