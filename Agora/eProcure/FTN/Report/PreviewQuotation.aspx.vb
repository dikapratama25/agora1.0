Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient
Public Class PreviewQuotationFTN
    Inherits AgoraLegacy.AppBaseClass
    Dim dispatcher As New dispatcher

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PreviewQuotation()
    End Sub

    Private Sub PreviewQuotation()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim objGST As New GST
        Dim blnGST As Boolean = False
        Dim strImgSrc As String

        'strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyId"), System.AppDomain.CurrentDomain.BaseDirectory & "images\")
        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request.QueryString("SCoyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))


        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                '.CommandText = "SELECT (SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = COMPANY_MSTR_1.CM_STATE) AND (CODE_CATEGORY = 's') AND " _
                '            & "(CODE_VALUE = COMPANY_MSTR_1.CM_COUNTRY)) AS CMState, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = COMPANY_MSTR_1.CM_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS CMCtry, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_STATE) AND (CODE_CATEGORY = 's') AND " _
                '            & "(CODE_VALUE = COMPANY_MSTR.CM_COUNTRY)) AS SupplierAddrState, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_COUNTRY) AND (CODE_CATEGORY = 'ct')) " _
                '            & "AS SupplierAddrCtry, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Pay_Term_Code) AND (CODE_CATEGORY = 'pt')) " _
                '            & "AS PaymentTerm, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Payment_Type) AND (CODE_CATEGORY = 'pm')) " _
                '            & "AS PaymentMethod, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Ship_Term) AND (CODE_CATEGORY = 'St')) " _
                '            & "AS Ship_Term, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Ship_Mode) AND (CODE_CATEGORY = 'sm')) " _
                '            & "AS Ship_Mode, COMPANY_MSTR_1.CM_COY_ID AS Buyer_Coy_ID, " _
                '            & "COMPANY_MSTR_1.CM_COY_NAME AS Buyer_Coy_Name, " _
                '            & "COMPANY_MSTR_1.CM_COY_TYPE AS Buyer_Coy_Type, " _
                '            & "COMPANY_MSTR_1.CM_ADDR_LINE1 AS Buyer_Addr_Line1, " _
                '            & "COMPANY_MSTR_1.CM_ADDR_LINE2 AS Buyer_Addr_Line2, " _
                '            & "COMPANY_MSTR_1.CM_ADDR_LINE3 AS Buyer_Addr_Line3, " _
                '            & "COMPANY_MSTR_1.CM_POSTCODE AS Buyer_Postcode, COMPANY_MSTR_1.CM_CITY AS Buyer_City, " _
                '            & "COMPANY_MSTR_1.CM_STATE AS Buyer_State, COMPANY_MSTR_1.CM_COUNTRY AS Buyer_Country, " _
                '            & "COMPANY_MSTR_1.CM_PHONE AS Buyer_Phone, COMPANY_MSTR_1.CM_FAX AS Buyer_Fax, " _
                '            & "COMPANY_MSTR_1.CM_EMAIL AS Buyer_Email, " _
                '            & "COMPANY_MSTR_1.CM_BUSINESS_REG_NO AS Buyer_Business_Reg_No, " _
                '            & "COMPANY_MSTR_1.CM_STATUS AS Buyer_Coy_Status, " _
                '            & "COMPANY_MSTR_1.CM_DELETED AS Buyer_Coy_Deleted, RFQ_MSTR.RM_RFQ_ID, RFQ_MSTR.RM_Coy_ID, " _
                '            & "RFQ_MSTR.RM_RFQ_No, RFQ_MSTR.RM_RFQ_Name, RFQ_MSTR.RM_Expiry_Date, RFQ_MSTR.RM_Status, " _
                '            & "RFQ_MSTR.RM_Remark, RFQ_MSTR.RM_Created_By, RFQ_MSTR.RM_Created_On, " _
                '            & "RFQ_MSTR.RM_Currency_Code, RFQ_MSTR.RM_Payment_Term, RFQ_MSTR.RM_Payment_Type, " _
                '            & "RFQ_MSTR.RM_Shipment_Term, RFQ_MSTR.RM_Shipment_Mode, RFQ_MSTR.RM_Prefix, " _
                '            & "RFQ_MSTR.RM_B_Display_Status, RFQ_MSTR.RM_Reqd_Quote_Validity, RFQ_MSTR.RM_Contact_Person, " _
                '            & "RFQ_MSTR.RM_Contact_Number, RFQ_MSTR.RM_Email, RFQ_MSTR.RM_RFQ_OPTION, " _
                '            & "RFQ_MSTR.RM_VEN_DISTR_LIST_INDEX, RFQ_REPLIES_MSTR.RRM_RFQ_ID, " _
                '            & "RFQ_REPLIES_MSTR.RRM_V_Company_ID, RFQ_REPLIES_MSTR.RRM_Currency_Code, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Offer_Till, RFQ_REPLIES_MSTR.RRM_ETD, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Remarks, RFQ_REPLIES_MSTR.RRM_Pay_Term_Code, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Payment_Type, RFQ_REPLIES_MSTR.RRM_Ship_Mode, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Ship_Term, RFQ_REPLIES_MSTR.RRM_Created_On, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Created_By, RFQ_REPLIES_MSTR.RRM_GST, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Quot_Seq_No, RFQ_REPLIES_MSTR.RRM_Quot_Prefix, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Actual_Quot_Num, RFQ_REPLIES_MSTR.RRM_Contact_Person, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Contact_Number, RFQ_REPLIES_MSTR.RRM_Email, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Status, RFQ_REPLIES_MSTR.RRM_B_Display_Status, " _
                '            & "RFQ_REPLIES_MSTR.RRM_V_Display_Status, RFQ_REPLIES_MSTR.RRM_Indicator, " _
                '            & "RFQ_REPLIES_MSTR.RRM_TotalValue, RFQ_REPLIES_DETAIL.RRD_RFQ_ID, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_V_Coy_Id, RFQ_REPLIES_DETAIL.RRD_Line_No, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_Product_Code, RFQ_REPLIES_DETAIL.RRD_Vendor_Item_Code, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_Quantity, RFQ_REPLIES_DETAIL.RRD_Unit_Price, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_GST_Code, RFQ_REPLIES_DETAIL.RRD_GST, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_GST_Desc, RFQ_REPLIES_DETAIL.RRD_Product_Desc, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_UOM, RFQ_REPLIES_DETAIL.RRD_Delivery_Lead_Time, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_Warranty_Terms, RFQ_REPLIES_DETAIL.RRD_Min_Pack_Qty, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_Min_Order_Qty, RFQ_REPLIES_DETAIL.RRD_Remarks, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_Tolerance, COMPANY_MSTR.CM_COY_ID AS Supp_Coy_ID, " _
                '            & "COMPANY_MSTR.CM_COY_NAME AS Supp_Coy_Name, " _
                '            & "COMPANY_MSTR.CM_ADDR_LINE1 AS Supp_Addr_Line1, " _
                '            & "COMPANY_MSTR.CM_ADDR_LINE2 AS Supp_Addr_Line2, " _
                '            & "COMPANY_MSTR.CM_ADDR_LINE3 AS Supp_Addr_Line3, " _
                '            & "COMPANY_MSTR.CM_POSTCODE AS Supp_Coy_Postcode, COMPANY_MSTR.CM_CITY AS Supp_Coy_City, " _
                '            & "COMPANY_MSTR.CM_STATE AS Supp_Coy_State, COMPANY_MSTR.CM_COUNTRY AS Supp_Coy_Country, " _
                '            & "COMPANY_MSTR.CM_PHONE AS Supp_Coy_Phone, COMPANY_MSTR.CM_FAX AS Supp_Coy_Fax, " _
                '            & "COMPANY_MSTR.CM_EMAIL AS Supp_Coy_Email, " _
                '            & "COMPANY_MSTR.CM_BUSINESS_REG_NO AS Supp_Coy_BusinessRegNo " _
                '            & "FROM      RFQ_MSTR INNER JOIN " _
                '            & "RFQ_REPLIES_MSTR ON RFQ_MSTR.RM_RFQ_ID = RFQ_REPLIES_MSTR.RRM_RFQ_ID INNER JOIN " _
                '            & "RFQ_REPLIES_DETAIL ON RFQ_REPLIES_MSTR.RRM_RFQ_ID = RFQ_REPLIES_DETAIL.RRD_RFQ_ID AND " _
                '            & "RFQ_REPLIES_MSTR.RRM_V_Company_ID = RFQ_REPLIES_DETAIL.RRD_V_Coy_Id INNER JOIN " _
                '            & "COMPANY_MSTR ON RFQ_REPLIES_MSTR.RRM_V_Company_ID = COMPANY_MSTR.CM_COY_ID INNER JOIN " _
                '            & "COMPANY_MSTR AS COMPANY_MSTR_1 ON " _
                '            & "RFQ_MSTR.RM_Coy_ID = COMPANY_MSTR_1.CM_COY_ID " _
                '            & "WHERE   (RFQ_REPLIES_MSTR.RRM_V_Company_ID = @prmVCoyID) AND (RFQ_MSTR.RM_Coy_ID = @prmBCoyID) AND " _
                '            & "(RFQ_REPLIES_MSTR.RRM_Actual_Quot_Num = @prmQuoNum) AND " _
                '            & "(RFQ_MSTR.RM_RFQ_No = @prmRFQNum)"
                '.CommandText = "SELECT a.CODE_DESC AS CMState,b.CODE_DESC AS CMCtry,c.CODE_DESC AS SupplierAddrState,d.CODE_DESC AS SupplierAddrCtry," _
                '       & "e.CODE_DESC AS PaymentTerm,f.CODE_DESC AS PaymentMethod,g.CODE_DESC AS Ship_Term,h.CODE_DESC AS Ship_Mode," _
                '       & "COMPANY_MSTR_1.CM_COY_ID AS Buyer_Coy_ID, COMPANY_MSTR_1.CM_COY_NAME AS Buyer_Coy_Name, " _
                '       & "COMPANY_MSTR_1.CM_COY_TYPE AS Buyer_Coy_Type, COMPANY_MSTR_1.CM_ADDR_LINE1 AS Buyer_Addr_Line1, " _
                '       & "COMPANY_MSTR_1.CM_ADDR_LINE2 AS Buyer_Addr_Line2, COMPANY_MSTR_1.CM_ADDR_LINE3 AS Buyer_Addr_Line3, " _
                '       & "COMPANY_MSTR_1.CM_POSTCODE AS Buyer_Postcode, COMPANY_MSTR_1.CM_CITY AS Buyer_City, " _
                '       & "COMPANY_MSTR_1.CM_STATE AS Buyer_State, COMPANY_MSTR_1.CM_COUNTRY AS Buyer_Country, " _
                '       & "COMPANY_MSTR_1.CM_PHONE AS Buyer_Phone, COMPANY_MSTR_1.CM_FAX AS Buyer_Fax, " _
                '       & "COMPANY_MSTR_1.CM_EMAIL AS Buyer_Email, COMPANY_MSTR_1.CM_BUSINESS_REG_NO AS Buyer_Business_Reg_No, " _
                '       & "COMPANY_MSTR_1.CM_STATUS AS Buyer_Coy_Status, COMPANY_MSTR_1.CM_DELETED AS Buyer_Coy_Deleted, " _
                '       & "RFQ_MSTR.RM_RFQ_ID, RFQ_MSTR.RM_Coy_ID, RFQ_MSTR.RM_RFQ_No, RFQ_MSTR.RM_RFQ_Name, RFQ_MSTR.RM_Expiry_Date, " _
                '       & "RFQ_MSTR.RM_Status, RFQ_MSTR.RM_Remark, RFQ_MSTR.RM_Created_By, RFQ_MSTR.RM_Created_On, " _
                '       & "RFQ_MSTR.RM_Currency_Code, RFQ_MSTR.RM_Payment_Term, RFQ_MSTR.RM_Payment_Type, RFQ_MSTR.RM_Shipment_Term, " _
                '       & "RFQ_MSTR.RM_Shipment_Mode, RFQ_MSTR.RM_Prefix, RFQ_MSTR.RM_B_Display_Status, " _
                '       & "RFQ_MSTR.RM_Reqd_Quote_Validity, RFQ_MSTR.RM_Contact_Person, RFQ_MSTR.RM_Contact_Number, RFQ_MSTR.RM_Email," _
                '       & "RFQ_MSTR.RM_RFQ_OPTION, RFQ_MSTR.RM_VEN_DISTR_LIST_INDEX, RFQ_REPLIES_MSTR.RRM_RFQ_ID, " _
                '       & "RFQ_REPLIES_MSTR.RRM_V_Company_ID, RFQ_REPLIES_MSTR.RRM_Currency_Code, RFQ_REPLIES_MSTR.RRM_Offer_Till, " _
                '       & "RFQ_REPLIES_MSTR.RRM_ETD, RFQ_REPLIES_MSTR.RRM_Remarks, RFQ_REPLIES_MSTR.RRM_Pay_Term_Code, " _
                '       & "RFQ_REPLIES_MSTR.RRM_Payment_Type, RFQ_REPLIES_MSTR.RRM_Ship_Mode, RFQ_REPLIES_MSTR.RRM_Ship_Term, " _
                '       & "RFQ_REPLIES_MSTR.RRM_Created_On, RFQ_REPLIES_MSTR.RRM_Created_By, RFQ_REPLIES_MSTR.RRM_GST, " _
                '       & "RFQ_REPLIES_MSTR.RRM_Quot_Seq_No, RFQ_REPLIES_MSTR.RRM_Quot_Prefix, " _
                '       & "RFQ_REPLIES_MSTR.RRM_Actual_Quot_Num, RFQ_REPLIES_MSTR.RRM_Contact_Person, " _
                '       & "RFQ_REPLIES_MSTR.RRM_Contact_Number, RFQ_REPLIES_MSTR.RRM_Email, RFQ_REPLIES_MSTR.RRM_Status, " _
                '       & "RFQ_REPLIES_MSTR.RRM_B_Display_Status, RFQ_REPLIES_MSTR.RRM_V_Display_Status, " _
                '       & "RFQ_REPLIES_MSTR.RRM_Indicator, RFQ_REPLIES_MSTR.RRM_TotalValue, RFQ_REPLIES_DETAIL.RRD_RFQ_ID, " _
                '       & "RFQ_REPLIES_DETAIL.RRD_V_Coy_Id, RFQ_REPLIES_DETAIL.RRD_Line_No, RFQ_REPLIES_DETAIL.RRD_Product_Code, " _
                '       & "RFQ_REPLIES_DETAIL.RRD_Vendor_Item_Code, RFQ_REPLIES_DETAIL.RRD_Quantity, " _
                '       & "RFQ_REPLIES_DETAIL.RRD_Unit_Price, IFNULL(RFQ_REPLIES_DETAIL.RRD_Unit_Price,0) AS UnitPrice," _
                '       & "RFQ_REPLIES_DETAIL.RRD_GST_Code, RFQ_REPLIES_DETAIL.RRD_GST, RFQ_REPLIES_DETAIL.RRD_GST_Desc, " _
                '       & "RFQ_REPLIES_DETAIL.RRD_Product_Desc, RFQ_REPLIES_DETAIL.RRD_UOM, " _
                '       & "RFQ_REPLIES_DETAIL.RRD_Delivery_Lead_Time, RFQ_REPLIES_DETAIL.RRD_Warranty_Terms, " _
                '       & "RFQ_REPLIES_DETAIL.RRD_Min_Pack_Qty, RFQ_REPLIES_DETAIL.RRD_Min_Order_Qty, " _
                '       & "RFQ_REPLIES_DETAIL.RRD_Remarks, RFQ_REPLIES_DETAIL.RRD_Tolerance, COMPANY_MSTR.CM_COY_ID AS Supp_Coy_ID," _
                '       & "COMPANY_MSTR.CM_COY_NAME AS Supp_Coy_Name, COMPANY_MSTR.CM_ADDR_LINE1 AS Supp_Addr_Line1, " _
                '       & "COMPANY_MSTR.CM_ADDR_LINE2 AS Supp_Addr_Line2, COMPANY_MSTR.CM_ADDR_LINE3 AS Supp_Addr_Line3, " _
                '       & "COMPANY_MSTR.CM_POSTCODE AS Supp_Coy_Postcode, COMPANY_MSTR.CM_CITY AS Supp_Coy_City, " _
                '       & "COMPANY_MSTR.CM_STATE AS Supp_Coy_State, COMPANY_MSTR.CM_COUNTRY AS Supp_Coy_Country, " _
                '       & "COMPANY_MSTR.CM_PHONE AS Supp_Coy_Phone, COMPANY_MSTR.CM_FAX AS Supp_Coy_Fax, " _
                '       & "COMPANY_MSTR.CM_EMAIL AS Supp_Coy_Email, COMPANY_MSTR.CM_BUSINESS_REG_NO AS Supp_Coy_BusinessRegNo " _
                '       & "FROM RFQ_MSTR " _
                '       & "INNER JOIN RFQ_REPLIES_MSTR ON RFQ_MSTR.RM_RFQ_ID = RFQ_REPLIES_MSTR.RRM_RFQ_ID " _
                '       & "INNER JOIN RFQ_REPLIES_DETAIL ON RFQ_REPLIES_MSTR.RRM_RFQ_ID = RFQ_REPLIES_DETAIL.RRD_RFQ_ID " _
                '       & "AND RFQ_REPLIES_MSTR.RRM_V_Company_ID = RFQ_REPLIES_DETAIL.RRD_V_Coy_Id " _
                '       & "INNER JOIN COMPANY_MSTR ON RFQ_REPLIES_MSTR.RRM_V_Company_ID = COMPANY_MSTR.CM_COY_ID " _
                '       & "INNER JOIN COMPANY_MSTR AS COMPANY_MSTR_1 ON RFQ_MSTR.RM_Coy_ID = COMPANY_MSTR_1.CM_COY_ID " _
                '       & "INNER JOIN CODE_MSTR AS a ON   (a.CODE_ABBR = COMPANY_MSTR_1.CM_STATE) " _
                '       & "AND (a.CODE_CATEGORY = 's') AND (a.CODE_VALUE = COMPANY_MSTR_1.CM_COUNTRY)" _
                '       & "INNER JOIN CODE_MSTR b ON   (b.CODE_ABBR = COMPANY_MSTR_1.CM_COUNTRY) " _
                '       & "AND (b.CODE_CATEGORY = 'ct') " _
                '       & "INNER JOIN CODE_MSTR c ON   (c.CODE_ABBR = COMPANY_MSTR.CM_STATE) " _
                '       & "AND (c.CODE_CATEGORY = 's') AND (c.CODE_VALUE = COMPANY_MSTR.CM_COUNTRY)" _
                '       & "INNER JOIN CODE_MSTR d ON   (d.CODE_ABBR = COMPANY_MSTR.CM_COUNTRY) " _
                '       & "AND (d.CODE_CATEGORY = 'ct') " _
                '       & "INNER JOIN CODE_MSTR e ON   (e.CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Pay_Term_Code) " _
                '       & "AND (e.CODE_CATEGORY = 'pt') " _
                '       & "INNER JOIN CODE_MSTR f ON   (f.CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Payment_Type) " _
                '       & "AND (f.CODE_CATEGORY = 'pm') " _
                '       & "INNER JOIN CODE_MSTR g ON   (g.CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Ship_Term) " _
                '       & "AND (g.CODE_CATEGORY = 'St') " _
                '       & "INNER JOIN CODE_MSTR h ON   (h.CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Ship_Mode) " _
                '       & "AND (h.CODE_CATEGORY = 'sm') " _
                '       & "WHERE   (RFQ_REPLIES_MSTR.RRM_V_Company_ID = @prmVCoyID) AND (RFQ_MSTR.RM_Coy_ID = @prmBCoyID) " _
                '       & "AND (RFQ_REPLIES_MSTR.RRM_Actual_Quot_Num = @prmQuoNum) AND (RFQ_MSTR.RM_RFQ_No = @prmRFQNum)"
                .CommandText = "SELECT IF((TAX_PERC = '' OR TAX_PERC IS NULL),0,TAX_PERC) AS TaxPerc,IFNULL(i.CODE_DESC,'N/A') AS GSTRate,IFNULL(a.CODE_DESC,'') AS CMState,IFNULL(b.CODE_DESC,'') AS CMCtry,IFNULL(c.CODE_DESC,'') AS SupplierAddrState," _
                        & "IFNULL(d.CODE_DESC,'') AS SupplierAddrCtry,IFNULL(e.CODE_DESC,'') AS PaymentTerm,IFNULL(f.CODE_DESC,'') AS PaymentMethod," _
                        & "IFNULL(g.CODE_DESC,'') AS Ship_Term,IFNULL(h.CODE_DESC,'') AS Ship_Mode," _
                       & "COMPANY_MSTR_1.CM_COY_ID AS Buyer_Coy_ID, COMPANY_MSTR_1.CM_COY_NAME AS Buyer_Coy_Name, " _
                       & "COMPANY_MSTR_1.CM_COY_TYPE AS Buyer_Coy_Type, COMPANY_MSTR_1.CM_ADDR_LINE1 AS Buyer_Addr_Line1, " _
                       & "COMPANY_MSTR_1.CM_ADDR_LINE2 AS Buyer_Addr_Line2, COMPANY_MSTR_1.CM_ADDR_LINE3 AS Buyer_Addr_Line3, " _
                       & "COMPANY_MSTR_1.CM_POSTCODE AS Buyer_Postcode, COMPANY_MSTR_1.CM_CITY AS Buyer_City, " _
                       & "COMPANY_MSTR_1.CM_STATE AS Buyer_State, COMPANY_MSTR_1.CM_COUNTRY AS Buyer_Country, " _
                       & "COMPANY_MSTR_1.CM_PHONE AS Buyer_Phone, COMPANY_MSTR_1.CM_FAX AS Buyer_Fax, " _
                       & "COMPANY_MSTR_1.CM_EMAIL AS Buyer_Email, COMPANY_MSTR_1.CM_BUSINESS_REG_NO AS Buyer_Business_Reg_No, " _
                       & "COMPANY_MSTR_1.CM_STATUS AS Buyer_Coy_Status, COMPANY_MSTR_1.CM_DELETED AS Buyer_Coy_Deleted, " _
                       & "RFQ_MSTR.RM_RFQ_ID, RFQ_MSTR.RM_Coy_ID, RFQ_MSTR.RM_RFQ_No, RFQ_MSTR.RM_RFQ_Name, RFQ_MSTR.RM_Expiry_Date, " _
                       & "RFQ_MSTR.RM_Status, RFQ_MSTR.RM_Remark, RFQ_MSTR.RM_Created_By, RFQ_MSTR.RM_Created_On, " _
                       & "RFQ_MSTR.RM_Currency_Code, RFQ_MSTR.RM_Payment_Term, RFQ_MSTR.RM_Payment_Type, RFQ_MSTR.RM_Shipment_Term, " _
                       & "RFQ_MSTR.RM_Shipment_Mode, RFQ_MSTR.RM_Prefix, RFQ_MSTR.RM_B_Display_Status, " _
                       & "RFQ_MSTR.RM_Reqd_Quote_Validity, RFQ_MSTR.RM_Contact_Person, RFQ_MSTR.RM_Contact_Number, RFQ_MSTR.RM_Email," _
                       & "RFQ_MSTR.RM_RFQ_OPTION, RFQ_MSTR.RM_VEN_DISTR_LIST_INDEX, RFQ_REPLIES_MSTR.RRM_RFQ_ID, " _
                       & "RFQ_REPLIES_MSTR.RRM_V_Company_ID, RFQ_REPLIES_MSTR.RRM_Currency_Code, RFQ_REPLIES_MSTR.RRM_Offer_Till, " _
                       & "RFQ_REPLIES_MSTR.RRM_ETD, RFQ_REPLIES_MSTR.RRM_Remarks, RFQ_REPLIES_MSTR.RRM_Pay_Term_Code, " _
                       & "RFQ_REPLIES_MSTR.RRM_Payment_Type, RFQ_REPLIES_MSTR.RRM_Ship_Mode, RFQ_REPLIES_MSTR.RRM_Ship_Term, " _
                       & "RFQ_REPLIES_MSTR.RRM_Created_On, RFQ_REPLIES_MSTR.RRM_Created_By, RFQ_REPLIES_MSTR.RRM_GST, " _
                       & "RFQ_REPLIES_MSTR.RRM_Quot_Seq_No, RFQ_REPLIES_MSTR.RRM_Quot_Prefix, " _
                       & "RFQ_REPLIES_MSTR.RRM_Actual_Quot_Num, RFQ_REPLIES_MSTR.RRM_Contact_Person, " _
                       & "RFQ_REPLIES_MSTR.RRM_Contact_Number, RFQ_REPLIES_MSTR.RRM_Email, RFQ_REPLIES_MSTR.RRM_Status, " _
                       & "RFQ_REPLIES_MSTR.RRM_B_Display_Status, RFQ_REPLIES_MSTR.RRM_V_Display_Status, " _
                       & "RFQ_REPLIES_MSTR.RRM_Indicator, RFQ_REPLIES_MSTR.RRM_TotalValue, RFQ_REPLIES_DETAIL.RRD_RFQ_ID, " _
                       & "RFQ_REPLIES_DETAIL.RRD_V_Coy_Id, RFQ_REPLIES_DETAIL.RRD_Line_No, RFQ_REPLIES_DETAIL.RRD_Product_Code, " _
                       & "RFQ_REPLIES_DETAIL.RRD_Vendor_Item_Code, RFQ_REPLIES_DETAIL.RRD_Quantity, " _
                       & "RFQ_REPLIES_DETAIL.RRD_Unit_Price, IFNULL(RFQ_REPLIES_DETAIL.RRD_Unit_Price,0) AS UnitPrice," _
                       & "RFQ_REPLIES_DETAIL.RRD_GST_Code, RFQ_REPLIES_DETAIL.RRD_GST, RFQ_REPLIES_DETAIL.RRD_GST_RATE, RFQ_REPLIES_DETAIL.RRD_GST_Desc, " _
                       & "RFQ_REPLIES_DETAIL.RRD_Product_Desc, RFQ_REPLIES_DETAIL.RRD_UOM, " _
                       & "RFQ_REPLIES_DETAIL.RRD_Delivery_Lead_Time, RFQ_REPLIES_DETAIL.RRD_Warranty_Terms, " _
                       & "RFQ_REPLIES_DETAIL.RRD_Min_Pack_Qty, RFQ_REPLIES_DETAIL.RRD_Min_Order_Qty, " _
                       & "RFQ_REPLIES_DETAIL.RRD_Remarks, RFQ_REPLIES_DETAIL.RRD_Tolerance, COMPANY_MSTR.CM_COY_ID AS Supp_Coy_ID," _
                       & "COMPANY_MSTR.CM_COY_NAME AS Supp_Coy_Name, COMPANY_MSTR.CM_ADDR_LINE1 AS Supp_Addr_Line1, " _
                       & "COMPANY_MSTR.CM_ADDR_LINE2 AS Supp_Addr_Line2, COMPANY_MSTR.CM_ADDR_LINE3 AS Supp_Addr_Line3, " _
                       & "COMPANY_MSTR.CM_POSTCODE AS Supp_Coy_Postcode, COMPANY_MSTR.CM_CITY AS Supp_Coy_City, " _
                       & "COMPANY_MSTR.CM_STATE AS Supp_Coy_State, COMPANY_MSTR.CM_COUNTRY AS Supp_Coy_Country, " _
                       & "COMPANY_MSTR.CM_PHONE AS Supp_Coy_Phone, COMPANY_MSTR.CM_FAX AS Supp_Coy_Fax, " _
                       & "COMPANY_MSTR.CM_EMAIL AS Supp_Coy_Email, COMPANY_MSTR.CM_BUSINESS_REG_NO AS Supp_Coy_BusinessRegNo " _
                       & "FROM RFQ_MSTR " _
                       & "INNER JOIN RFQ_REPLIES_MSTR ON RFQ_MSTR.RM_RFQ_ID = RFQ_REPLIES_MSTR.RRM_RFQ_ID " _
                       & "INNER JOIN RFQ_REPLIES_DETAIL ON RFQ_REPLIES_MSTR.RRM_RFQ_ID = RFQ_REPLIES_DETAIL.RRD_RFQ_ID " _
                       & "AND RFQ_REPLIES_MSTR.RRM_V_Company_ID = RFQ_REPLIES_DETAIL.RRD_V_Coy_Id " _
                       & "INNER JOIN COMPANY_MSTR ON RFQ_REPLIES_MSTR.RRM_V_Company_ID = COMPANY_MSTR.CM_COY_ID " _
                       & "INNER JOIN COMPANY_MSTR AS COMPANY_MSTR_1 ON RFQ_MSTR.RM_Coy_ID = COMPANY_MSTR_1.CM_COY_ID " _
                       & "LEFT JOIN CODE_MSTR AS a ON   (a.CODE_ABBR = COMPANY_MSTR_1.CM_STATE) " _
                       & "AND (a.CODE_CATEGORY = 's') AND (a.CODE_VALUE = COMPANY_MSTR_1.CM_COUNTRY)" _
                       & "LEFT JOIN CODE_MSTR b ON   (b.CODE_ABBR = COMPANY_MSTR_1.CM_COUNTRY) " _
                       & "AND (b.CODE_CATEGORY = 'ct') " _
                       & "LEFT JOIN CODE_MSTR c ON   (c.CODE_ABBR = COMPANY_MSTR.CM_STATE) " _
                       & "AND (c.CODE_CATEGORY = 's') AND (c.CODE_VALUE = COMPANY_MSTR.CM_COUNTRY)" _
                       & "LEFT JOIN CODE_MSTR d ON   (d.CODE_ABBR = COMPANY_MSTR.CM_COUNTRY) " _
                       & "AND (d.CODE_CATEGORY = 'ct') " _
                       & "LEFT JOIN CODE_MSTR e ON   (e.CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Pay_Term_Code) " _
                       & "AND (e.CODE_CATEGORY = 'pt') " _
                       & "LEFT JOIN CODE_MSTR f ON   (f.CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Payment_Type) " _
                       & "AND (f.CODE_CATEGORY = 'pm') " _
                       & "LEFT JOIN CODE_MSTR g ON   (g.CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Ship_Term) " _
                       & "AND (g.CODE_CATEGORY = 'St') " _
                       & "LEFT JOIN CODE_MSTR h ON   (h.CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Ship_Mode) " _
                       & "AND (h.CODE_CATEGORY = 'sm') " _
                       & "LEFT JOIN TAX t ON (t.TAX_CODE = RFQ_REPLIES_DETAIL.RRD_GST_RATE) AND (t.TAX_COUNTRY_CODE = COMPANY_MSTR.CM_COUNTRY) " _
                       & "LEFT JOIN CODE_MSTR i ON   (i.CODE_ABBR = t.TAX_CODE) AND (i.CODE_CATEGORY = 'GST') AND (i.CODE_DELETED = 'N') " _
                       & "WHERE   (RFQ_REPLIES_MSTR.RRM_V_Company_ID = @prmVCoyID) AND (RFQ_MSTR.RM_Coy_ID = @prmBCoyID) " _
                       & "AND (RFQ_REPLIES_MSTR.RRM_Actual_Quot_Num = @prmQuoNum) AND (RFQ_MSTR.RM_RFQ_No = @prmRFQNum)"
            End With

            da = New MySqlDataAdapter(cmd)
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmVCoyID", Request.QueryString("SCoyID")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmBCoyID", Request.QueryString("BCoyID")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmQuoNum", Request.QueryString("quo_no")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmRFQNum", Request.QueryString("rfq_no")))

            da.Fill(ds)

            If ds.Tables(0).Rows.Count > 0 Then
                If objGST.chkGSTCOD(Format(ds.Tables(0).Rows(0)("RRM_Created_On"), "dd/MM/yyyy")) = True Or objGST.chkGSTByRate("QUO", ds.Tables(0).Rows(0)("RM_RFQ_ID"), Request.QueryString("SCoyID")) = True Then
                    blnGST = True
                Else
                    blnGST = False
                End If
            End If

            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewQuotation_FTN_DataTablePreviewQuotation", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            'localreport.ReportPath = dispatcher.direct("Report", "PreviewQuotation-FTN_ftn.rdlc", "FTNReport") ' appPath & "RFQ\PreviewQuotation-FTN.rdlc"

            If blnGST = True Then                
                localreport.ReportPath = dispatcher.direct("Report", "PreviewQuotation-FTN_ftn-GST.rdlc", "Report") ' appPath & "FTN\Report\PreviewQuotation-FTN_ftn.rdlc"
            Else
                localreport.ReportPath = dispatcher.direct("Report", "PreviewQuotation-FTN_ftn.rdlc", "Report") ' appPath & "FTN\Report\PreviewQuotation-FTN_ftn.rdlc"
            End If

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
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
            Dim strFileName As String = "QUO_" & Request.QueryString(Trim("quo_no")) & "_" & Session("CompanyID") & "_" & Session("UserId") & ".pdf"
            strFileName = Replace(strFileName, "/", "^")
            'Dim strTemp As String = dispatcher.direct("Report", "Temp", "FTNReport")
            Dim strTemp As String = appPath & "FTN\Report\Temp\"
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