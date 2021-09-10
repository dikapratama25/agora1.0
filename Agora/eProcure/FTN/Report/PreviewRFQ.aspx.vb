Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient
'Imports System.Web.UI

Public Class PreviewRFQFTN
    Inherits AgoraLegacy.AppBaseClass
    Dim dispatcher As New dispatcher

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        previewRFQ()
    End Sub

    Private Sub previewRFQ()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String

        'strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request.QueryString("BCoyID"), System.AppDomain.CurrentDomain.BaseDirectory & "images\")
        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request.QueryString("BCoyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT RFQ_MSTR.RM_RFQ_ID, RFQ_MSTR.RM_Coy_ID, RFQ_MSTR.RM_RFQ_No, RFQ_MSTR.RM_RFQ_Name, " _
                            & "RFQ_MSTR.RM_Expiry_Date, RFQ_MSTR.RM_Status, RFQ_MSTR.RM_Remark, RFQ_MSTR.RM_Created_By, " _
                            & "RFQ_MSTR.RM_Created_On, RFQ_MSTR.RM_Currency_Code, RFQ_MSTR.RM_Payment_Term, " _
                            & "RFQ_MSTR.RM_Payment_Type, RFQ_MSTR.RM_Shipment_Term, RFQ_MSTR.RM_Shipment_Mode, " _
                            & "RFQ_MSTR.RM_Prefix, RFQ_MSTR.RM_B_Display_Status, RFQ_MSTR.RM_Reqd_Quote_Validity, " _
                            & "RFQ_MSTR.RM_Contact_Person, RFQ_MSTR.RM_Contact_Number, RFQ_MSTR.RM_Email, " _
                            & "RFQ_MSTR.RM_RFQ_OPTION, RFQ_MSTR.RM_VEN_DISTR_LIST_INDEX, RFQ_DETAIL.RD_RFQ_ID, " _
                            & "RFQ_DETAIL.RD_Coy_ID, RFQ_DETAIL.RD_RFQ_Line, RFQ_DETAIL.RD_Product_Code, " _
                            & "RFQ_DETAIL.RD_Vendor_Item_Code, RFQ_DETAIL.RD_Quantity, RFQ_DETAIL.RD_Product_Desc, " _
                            & "RFQ_DETAIL.RD_UOM, RFQ_DETAIL.RD_Delivery_Lead_Time, RFQ_DETAIL.RD_Warranty_Terms, " _
                            & "RFQ_DETAIL.RD_Product_Name, RFQ_INVITED_VENLIST.RIV_RFQ_ID, RFQ_INVITED_VENLIST.RIV_S_Coy_ID, " _
                            & "RFQ_INVITED_VENLIST.RIV_S_Coy_Name, RFQ_INVITED_VENLIST.RIV_S_Addr_Line1, " _
                            & "RFQ_INVITED_VENLIST.RIV_S_Addr_Line2, RFQ_INVITED_VENLIST.RIV_S_Addr_Line3, " _
                            & "RFQ_INVITED_VENLIST.RIV_S_PostCode, RFQ_INVITED_VENLIST.RIV_S_City, " _
                            & "RFQ_INVITED_VENLIST.RIV_S_State, RFQ_INVITED_VENLIST.RIV_S_Country, " _
                            & "RFQ_INVITED_VENLIST.RIV_S_Phone, RFQ_INVITED_VENLIST.RIV_S_Fax, " _
                            & "RFQ_INVITED_VENLIST.RIV_S_Email, COMPANY_MSTR.CM_COY_ID, COMPANY_MSTR.CM_COY_NAME, " _
                            & "COMPANY_MSTR.CM_COY_TYPE, COMPANY_MSTR.CM_PARENT_COY_ID, COMPANY_MSTR.CM_ACCT_NO, " _
                            & "COMPANY_MSTR.CM_BANK, COMPANY_MSTR.CM_BRANCH, COMPANY_MSTR.CM_ADDR_LINE1, " _
                            & "COMPANY_MSTR.CM_ADDR_LINE2, COMPANY_MSTR.CM_ADDR_LINE3, COMPANY_MSTR.CM_POSTCODE, " _
                            & "COMPANY_MSTR.CM_CITY, COMPANY_MSTR.CM_STATE, COMPANY_MSTR.CM_COUNTRY, " _
                            & "COMPANY_MSTR.CM_PHONE, COMPANY_MSTR.CM_FAX, COMPANY_MSTR.CM_EMAIL, " _
                            & "COMPANY_MSTR.CM_COY_LOGO, COMPANY_MSTR.CM_BUSINESS_REG_NO, " _
                            & "COMPANY_MSTR.CM_TAX_REG_NO, COMPANY_MSTR.CM_PAYMENT_TERM, " _
                            & "COMPANY_MSTR.CM_PAYMENT_METHOD, COMPANY_MSTR.CM_ACTUAL_TERMSANDCONDFILE, " _
                            & "COMPANY_MSTR.CM_HUB_TERMSANDCONDFILE, COMPANY_MSTR.CM_PWD_DURATION, " _
                            & "COMPANY_MSTR.CM_TAX_CALC_BY, COMPANY_MSTR.CM_CURRENCY_CODE, " _
                            & "COMPANY_MSTR.CM_BCM_SET, COMPANY_MSTR.CM_BUDGET_FROM_DATE, " _
                            & "COMPANY_MSTR.CM_BUDGET_TO_DATE, COMPANY_MSTR.CM_RFQ_OPTION, " _
                            & "COMPANY_MSTR.CM_LICENCE_PACKAGE, COMPANY_MSTR.CM_LICENSE_USERS, " _
                            & "COMPANY_MSTR.CM_SUB_START_DT, COMPANY_MSTR.CM_SUB_END_DT, " _
                            & "COMPANY_MSTR.CM_LICENSE_PRODUCTS, COMPANY_MSTR.CM_FINDEPT_MODE, " _
                            & "COMPANY_MSTR.CM_PRIV_LABELING, COMPANY_MSTR.CM_SKINS_ID, COMPANY_MSTR.CM_TRAINING, " _
                            & "COMPANY_MSTR.CM_STATUS, COMPANY_MSTR.CM_DELETED, COMPANY_MSTR.CM_MOD_BY, " _
                            & "COMPANY_MSTR.CM_MOD_DT, COMPANY_MSTR.CM_ENT_BY, COMPANY_MSTR.CM_ENT_DT, " _
                            & "COMPANY_MSTR.CM_SKU, COMPANY_MSTR.CM_TRANS_NO, COMPANY_MSTR.CM_CONTACT, " _
                            & "COMPANY_MSTR.CM_REPORT_USERS, COMPANY_MSTR.CM_INV_APPR, COMPANY_MSTR.CM_MULTI_PO, " _
                            & "COMPANY_MSTR.CM_BA_CANCEL, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_STATE) AND (CODE_CATEGORY = 's') AND " _
                            & "(CODE_VALUE = COMPANY_MSTR.CM_COUNTRY)) AS CMState, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS CMCtry, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = RFQ_INVITED_VENLIST.RIV_S_State) AND (CODE_CATEGORY = 's') AND " _
                            & "(CODE_VALUE = RFQ_INVITED_VENLIST.RIV_S_Country)) AS SupplierAddrState, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = RFQ_INVITED_VENLIST.RIV_S_Country) AND (CODE_CATEGORY = 'ct')) " _
                            & "AS SupplierAddrCtry, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = RFQ_MSTR.RM_Payment_Term) AND (CODE_CATEGORY = 'pt')) " _
                            & "AS RFQ_PaymentTerm, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = RFQ_MSTR.RM_Payment_Type) AND (CODE_CATEGORY = 'pm')) " _
                            & "AS RFQ_PaymentMethod, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = RFQ_MSTR.RM_Shipment_Term) AND (CODE_CATEGORY = 'St')) " _
                            & "AS RFQ_ShipmentTerm, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = RFQ_MSTR.RM_Shipment_Mode) AND (CODE_CATEGORY = 'sm')) " _
                            & "AS RFQ_ShipmentMode, " _
                            & "(SELECT   CM_BUSINESS_REG_NO " _
                            & "FROM      COMPANY_MSTR AS B " _
                            & "WHERE   (RFQ_INVITED_VENLIST.RIV_S_Coy_ID = CM_COY_ID)) AS sUPPBUSSREGNO " _
                            & "FROM      RFQ_MSTR INNER JOIN " _
                            & "RFQ_DETAIL ON RFQ_MSTR.RM_RFQ_ID = RFQ_DETAIL.RD_RFQ_ID INNER JOIN " _
                            & "RFQ_INVITED_VENLIST ON RFQ_MSTR.RM_RFQ_ID = RFQ_INVITED_VENLIST.RIV_RFQ_ID INNER JOIN " _
                            & "COMPANY_MSTR ON RFQ_MSTR.RM_Coy_ID = COMPANY_MSTR.CM_COY_ID " _
                            & "WHERE   (RFQ_MSTR.RM_RFQ_No = @prmRFQNum) AND (RFQ_MSTR.RM_Coy_ID =@prmBCoyID)"

                If Request.QueryString("VendorRequired") = "T" Then
                    .CommandText = .CommandText & " AND (RFQ_INVITED_VENLIST.RIV_S_Coy_ID = @prmVCoyID)"
                End If
                .CommandText = .CommandText & " ORDER BY RD_RFQ_Line"
            End With

            da = New MySqlDataAdapter(cmd)
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmBCoyID", Request.QueryString("BCoyID")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmRFQNum", Request.QueryString("RFQ_Num")))
            If Request.QueryString("VendorRequired") = "T" Then
                da.SelectCommand.Parameters.Add(New MySqlParameter("@prmVCoyID", Request.QueryString("SCoyID")))
            End If
            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewRFQ_FTN_DataTablePreviewRFQ", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            localreport.ReportPath = dispatcher.direct("Report", "PreviewRFQ-FTN2_ftn.rdlc", "FTNReport") ' appPath & "RFQ\PreviewRFQ-FTN2.rdlc"
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
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings(strID) & strImgSrc)
                    Case Else
                End Select
            Next
            localreport.SetParameters(par)
            localreport.Refresh()

            'Dim deviceInfo As String = _
            '     "<DeviceInfo>" + _
            '         "  <OutputFormat>EMF</OutputFormat>" + _
            '         "  <PageWidth>8.27in</PageWidth>" + _
            '         "  <PageHeight>11in</PageHeight>" + _
            '         "  <MarginTop>0.25in</MarginTop>" + _
            '         "  <MarginLeft>0.25in</MarginLeft>" + _
            '         "  <MarginRight>0.25in</MarginRight>" + _
            '         "  <MarginBottom>0.25in</MarginBottom>" + _
            '         "</DeviceInfo>"
            Dim deviceInfo As String = _
               "<DeviceInfo>" + _
                   "  <OutputFormat>EMF</OutputFormat>" + _
                   "</DeviceInfo>"
            ' While System.IO.File.Exists(toFileName) = False
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            'End While

            'Common.NetMsgbox(Me, "Before render")
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
            'Common.NetMsgbox(Me, "After render")
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            Dim strFileName As String = "RFQ_" & Request.QueryString(Trim("RFQ_Num")) & "_" & Session("CompanyID") & "_" & Session("UserId") & ".pdf"
            strFileName = Replace(strFileName, "/", "^")
            Dim strTemp As String = dispatcher.direct("Report", "Temp", "FTNReport")
            If Dir(strTemp, FileAttribute.Directory) = "" Then
                MkDir(strTemp)
            End If
            Dim fs As New FileStream(Server.MapPath("Temp/" & strFileName), FileMode.Create)
            'Dim fs As New FileStream(Server.MapPath("BuyerRFQReport.PDF"), FileMode.Create)
            fs.Write(PDF, 0, PDF.Length)
            fs.Close()
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            'Common.NetMsgbox(Me, "Write")
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