Imports System
Imports System.Collections
Imports System.Web
Imports AgoraLegacy
Imports SSO.Component
Imports System.IO
Imports System.Web.UI.WebControls

Namespace AgoraLegacy

    Public Class RFQ_User_Ext
        Public bcom_id As String
        Public REG_NO As String
        Public list_index As String
        Public gst As String
        Public state As String
        Public country As String
        Public gst_desc As String
        Public Tolerance As String
        Public buyername As String
        Public adds As String
        Public qoute_valDate As String
        Public V_LIST As String
        Public indicat As String
        Public quo_num As String
        Public total As String
        Public Tax As String
        Public v_itemCode As String
        Public MOQ As String
        Public MPQ As String
        Public PRICE As String
        Public validaty As String
        Public RFQ_Req_date As String
        Public V_com_ID As String
        Public RFQ_ID As String
        Public dis_ID As String
        Public RM_Coy_ID As String
        Public product_ID As String
        Public product_name As String
        Public vendor_Id As String
        Public item_desc As String
        Public user_name As String
        Public vendor_name As String
        Public vendor_Con_num As String
        Public vendor_email As String
        Public vendor_person As String
        Public vendor_Addr As String
        Public RFQ_Name2 As String
        Public phone As String
        Public email As String
        Public RFQ_Name As String
        Public cur_desc As String
        Public exp_date As String
        Public remark As String
        Public create_by As String
        Public create_on As String
        Public cur_code As String
        Public pay_term As String
        Public pay_type As String
        Public ship_term As String
        Public ship_mode As String
        Public con_person As String
        Public req_qout As String
        Public uom As String
        Public Quantity As String
        Public Delivery_Lead_Time As String
        Public Warranty_Terms As String
        Public RFQIndex As String
        Public index As String
        Public Header_Ind As String
        Public Check() As String
        Public RFQ_Num As String
        Public prefix As String
        Public V_Com_Name As String
        Public date_today As String
        Public RFQ_Option As String
        Public val As String
        Public seq As String
        Public lineno As String
        Public unitprice As String
        Public productdesc As String
        Public addsline1 As String
        Public addsline2 As String
        Public addsline3 As String
        Public postcode As String
        Public city As String
        Public type As String
        Public del_code As String
        Public RFQStatus As String
        Public BDisplayStatus As String
    End Class

    Public Class RFQ_Ext
        Dim objDb As New EAD.DBCom
        Dim lsSql As String

#Region " Buyer RFQ "

        Public Function read_user(ByVal RFQ_user As RFQ_User_Ext, ByVal rfq_name As String)
            Dim tDS As DataSet
            Dim sqlstr2 As String
            sqlstr2 = "select * from RFQ_MSTR where RM_Created_By = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " & _
               "and RM_RFQ_Name ='" & Common.Parse(rfq_name) & "'"

            If objDb.Exist(sqlstr2) > 0 Then
                tDS = objDb.FillDs(sqlstr2)
                If tDS.Tables(0).Rows.Count > 0 Then
                    RFQ_user.exp_date = tDS.Tables(0).Rows(0).Item("RM_Expiry_Date").ToString.Trim
                    RFQ_user.pay_term = tDS.Tables(0).Rows(0).Item("RM_Payment_Term").ToString.Trim
                    RFQ_user.pay_type = tDS.Tables(0).Rows(0).Item("RM_Payment_Type").ToString.Trim
                    RFQ_user.ship_term = tDS.Tables(0).Rows(0).Item("RM_Shipment_Term").ToString.Trim
                    RFQ_user.ship_mode = tDS.Tables(0).Rows(0).Item("RM_Shipment_Mode").ToString.Trim
                    RFQ_user.con_person = tDS.Tables(0).Rows(0).Item("RM_Contact_Person").ToString.Trim
                    RFQ_user.phone = tDS.Tables(0).Rows(0).Item("RM_Contact_Number").ToString.Trim
                    RFQ_user.email = tDS.Tables(0).Rows(0).Item("RM_Email").ToString.Trim
                    RFQ_user.req_qout = tDS.Tables(0).Rows(0).Item("RM_Reqd_Quote_Validity").ToString.Trim
                    RFQ_user.remark = tDS.Tables(0).Rows(0).Item("RM_Remark").ToString.Trim
                    RFQ_user.cur_code = tDS.Tables(0).Rows(0).Item("RM_Currency_Code").ToString.Trim
                    RFQ_user.exp_date = tDS.Tables(0).Rows(0).Item("RM_Expiry_Date").ToString.Trim
                    RFQ_user.RFQ_Req_date = tDS.Tables(0).Rows(0).Item("RM_Reqd_Quote_Validity").ToString.Trim

                End If
            Else

            End If

            Dim sqlstr As String = New String("SELECT IsNull(UM_USER_NAME,'') AS User_Name," & _
                                           "IsNull(UM_TEL_NO,'') AS Phone, IsNull(UM_EMAIL,'') AS Email  FROM USER_MSTR " & _
                                           " WHERE USER_MSTR.UM_USER_ID = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " & _
                                           " AND USER_MSTR.UM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'")

            tDS = objDb.FillDs(sqlstr)
            If tDS.Tables(0).Rows.Count > 0 Then
                RFQ_user.user_name = tDS.Tables(0).Rows(0).Item("User_Name")
                RFQ_user.phone = tDS.Tables(0).Rows(0).Item("Phone")
                RFQ_user.email = tDS.Tables(0).Rows(0).Item("Email")
            End If

        End Function

        Public Function save_RFQ(ByVal strVendorList As String, ByVal value() As String, ByRef rfq_num As String, ByRef rfq_id As String, ByVal dt As DataTable, Optional ByVal dtList As DataTable = Nothing, Optional ByVal dtDetails As DataTable = Nothing, Optional ByVal submit As Boolean = True) As String
            Dim objglb As New AppGlobals
            Dim strsql, prefix As String
            Dim strAryQuery(0) As String
            Dim blnInsert As Boolean
            Dim i As Integer
            Dim drList As DataRow
            Dim dtrDetails As DataRow()
            Dim strSearch As String = ""
            Dim intIncrementNo As Integer = 0

            strsql = "select '*' from RFQ_MSTR "
            strsql &= "WHERE RM_Coy_ID= '" & HttpContext.Current.Session("CompanyID") & "' "
            strsql &= "AND RM_RFQ_NO='" & Common.Parse(rfq_num) & "' "
            strsql &= "AND RM_Created_By ='" & HttpContext.Current.Session("UserID") & "' "
            strsql &= "AND RM_B_DISPLAY_STATUS = '0' "
            If objDb.Exist(strsql) = 0 Then ' record not exists, so insert

                strsql = " SET @T_NO = ''; UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CP_PARAM_TYPE = 'RFQ' "
                Common.Insert2Ary(strAryQuery, strsql)

                intIncrementNo = 1

                rfq_num = " (SELECT CAST(GROUP_CONCAT(CASE WHEN cp_param_name = 'prefix' THEN cp_param_value ELSE " _
                           & " CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) END " _
                           & " SEPARATOR '') AS CHAR(1000)) AS cp_param_value FROM (SELECT * FROM company_param WHERE CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CP_PARAM_TYPE = 'RFQ' ORDER BY CP_PARAM_NAME DESC) ZZZ ) "

                prefix = " (SELECT cp_param_value FROM company_param WHERE CP_PARAM_NAME = 'prefix' AND CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CP_PARAM_TYPE = 'RFQ') "

                If chk_rfqidnew(rfq_num) = True Then
                    save_RFQ = "6"
                    Exit Function
                End If

                'Insert into RFQ_MSTR
                If submit = True Then
                    strsql = "insert into RFQ_MSTR" & _
                            "(RM_Prefix,RM_RFQ_No,RM_Coy_ID,RM_RFQ_Name, " & _
                            " RM_Expiry_Date,RM_Remark,RM_Created_By,RM_Created_On," & _
                            " RM_Currency_Code,RM_Payment_Term,RM_Payment_Type,RM_Shipment_Term ," & _
                            " RM_Shipment_Mode,RM_Contact_Person,RM_Contact_Number,RM_Email," & _
                            " RM_Reqd_Quote_Validity,RM_RFQ_OPTION,RM_Status,RM_B_Display_Status, RM_Submission_Date, RM_DEL_CODE)" & _
                            "values (" & prefix & "," & rfq_num & " ,'" & HttpContext.Current.Session("CompanyID") & "','" & Common.Parse(value(0)) & "' " & _
                            " ," & Common.ConvertDate(value(1)) & ",'" & Common.Parse(value(2)) & "','" & HttpContext.Current.Session("UserID") & "'," & Common.ConvertDate(value(4)) & ", " & _
                            " '" & Common.Parse(value(5)) & "','" & Common.Parse(value(6)) & " ','" & Common.Parse(value(7)) & "','" & Common.Parse(value(8)) & "'" & _
                            ",'" & Common.Parse(value(9)) & "','" & Common.Parse(value(3)) & "','" & Common.Parse(value(11)) & "','" & Common.Parse(value(12)) & "', " & _
                            " " & Common.ConvertDate(value(13)) & ",'" & Common.Parse(value(14)) & "','" & Common.Parse(value(15)) & "','" & Common.Parse(value(16)) & "','" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "','" & Common.Parse(value(17)) & "')"
                Else
                    strsql = "insert into RFQ_MSTR" & _
                            "(RM_Prefix,RM_RFQ_No,RM_Coy_ID,RM_RFQ_Name, " & _
                            " RM_Expiry_Date,RM_Remark,RM_Created_By,RM_Created_On," & _
                            " RM_Currency_Code,RM_Payment_Term,RM_Payment_Type,RM_Shipment_Term ," & _
                            " RM_Shipment_Mode,RM_Contact_Person,RM_Contact_Number,RM_Email," & _
                            " RM_Reqd_Quote_Validity,RM_RFQ_OPTION,RM_Status,RM_B_Display_Status, RM_DEL_CODE)" & _
                            "values (" & prefix & "," & rfq_num & " ,'" & HttpContext.Current.Session("CompanyID") & "','" & Common.Parse(value(0)) & "' " & _
                            " ," & Common.ConvertDate(value(1)) & ",'" & Common.Parse(value(2)) & "','" & HttpContext.Current.Session("UserID") & "'," & Common.ConvertDate(value(4)) & ", " & _
                            " '" & Common.Parse(value(5)) & "','" & Common.Parse(value(6)) & " ','" & Common.Parse(value(7)) & "','" & Common.Parse(value(8)) & "'" & _
                            ",'" & Common.Parse(value(9)) & "','" & Common.Parse(value(3)) & "','" & Common.Parse(value(11)) & "','" & Common.Parse(value(12)) & "', " & _
                            " " & Common.ConvertDate(value(13)) & ",'" & Common.Parse(value(14)) & "','" & Common.Parse(value(15)) & "','" & Common.Parse(value(16)) & "','" & Common.Parse(value(17)) & "')"
                End If
                Common.Insert2Ary(strAryQuery, strsql)
                blnInsert = True
            Else
                If rfq_id <> "" Then ' for update
                    strsql = "update RFQ_MSTR set RM_Expiry_Date=" & Common.ConvertDate(value(1)) & ","
                    strsql &= "RM_Remark='" & Common.Parse(value(2)) & "',RM_Created_By='" & Common.Parse(HttpContext.Current.Session("UserID")) & "',"
                    strsql &= "RM_RFQ_Name='" & Common.Parse(value(0)) & "', "
                    strsql &= "RM_Created_On=" & Common.ConvertDate(value(4)) & ",RM_Currency_Code='" & Common.Parse(value(5)) & "',"
                    strsql &= "RM_Payment_Term='" & Common.Parse(value(6)) & " ',"
                    strsql &= "RM_Payment_Type='" & Common.Parse(value(7)) & "',RM_Shipment_Term='" & Common.Parse(value(8)) & "',"
                    strsql &= "RM_Shipment_Mode='" & Common.Parse(value(9)) & "',RM_Contact_Person='" & Common.Parse(value(10)) & "',"
                    strsql &= "RM_DEL_CODE='" & Common.Parse(value(17)) & "',"
                    strsql &= "RM_Contact_Number='" & Common.Parse(value(11)) & "', "
                    strsql &= "RM_Email='" & Common.Parse(value(12)) & "',RM_Reqd_Quote_Validity=" & Common.ConvertDate(value(13)) & " "
                    If submit = True Then
                        strsql &= ", RM_Submission_Date='" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "' "
                        strsql &= "where rm_rfq_id = '" & rfq_id & "' and  RM_Coy_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' "
                    Else
                        strsql &= "where rm_rfq_id = '" & rfq_id & "' and  RM_Coy_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' "
                    End If
                    Common.Insert2Ary(strAryQuery, strsql)
                    blnInsert = False
                End If
            End If

            If rfq_id = "" Then 'New RFQ
                'Add vendor list
                For Each drList In dtList.Rows
                    If drList.Item("TYPE") = "list" Then
                        strSearch = "RVDLM_List_Index='" & drList.Item("RVDLM_List_Index") & "' AND TYPE = 'list'"
                        dtrDetails = dtDetails.Select(strSearch)
                        If dtrDetails.Length > 0 Then
                            For Each oRow As DataRow In dtrDetails
                                strsql = "insert into RFQ_INVITED_VENLIST_DETAIL(RTVDT_User_Id,RTVDT_RFQ_Name," _
                                           & "RTVDT_Distribution_List_Id,RTVDT_v_company_id,RTVDT_RFQ_ID) " _
                                           & "values ('" & Common.Parse(HttpContext.Current.Session("UserID")) & "','" _
                                           & Common.Parse(value(0)) & "','" & drList.Item("RVDLM_List_Index") & "','" _
                                           & Common.Parse(oRow.Item("CoyId")) & "'," & objDb.GetLatestInsertedID("RFQ_MSTR") & ")"
                                Common.Insert2Ary(strAryQuery, strsql)

                                strsql = "insert into RFQ_INVITED_VENLIST select distinct " _
                                        & objDb.GetLatestInsertedID("RFQ_MSTR") & " " _
                                        & ",CM.CM_COY_ID," _
                                        & "CM.CM_COY_NAME,CM.CM_ADDR_LINE1,CM.CM_ADDR_LINE2,CM.CM_ADDR_LINE3," _
                                        & "CM.CM_POSTCODE,CM.CM_CITY,CM.CM_STATE,CM.CM_COUNTRY,CM.CM_PHONE,CM.CM_FAX," _
                                        & "CM.CM_EMAIL " _
                                        & "FROM COMPANY_MSTR CM " _
                                        & "WHERE CM.CM_COY_ID='" & Common.Parse(oRow.Item("CoyId")) & "'"
                                Common.Insert2Ary(strAryQuery, strsql)
                            Next
                        End If
                        strsql = "insert into RFQ_INVITED_VENLIST_MSTR(RIVMT_User_ID,RIVMT_RFQ_Name," _
                                & "RIVMT_Distribution_list_id,RIVMT_RFQ_ID) " _
                                & "values ('" & Common.Parse(HttpContext.Current.Session("UserID")) & "','" _
                                & Common.Parse(value(0)) & "','" & drList.Item("RVDLM_List_Index") & "'," _
                                & objDb.GetLatestInsertedID("RFQ_MSTR") & ")"
                        Common.Insert2Ary(strAryQuery, strsql)

                    ElseIf drList.Item("TYPE") = "specific" Then
                        strSearch = "RVDLM_List_Index='" & drList.Item("RVDLM_List_Index") & "' AND TYPE = 'specific'"
                        dtrDetails = dtDetails.Select(strSearch)
                        If dtrDetails.Length > 0 Then
                            For Each oRow As DataRow In dtrDetails
                                strsql = "insert into RFQ_INVITED_VENLIST select DISTINCT " _
                                    & objDb.GetLatestInsertedID("RFQ_MSTR") & " " _
                                    & ", CM.CM_COY_ID, CM.CM_COY_NAME, " _
                                    & "CM.CM_ADDR_LINE1,CM.CM_ADDR_LINE2,CM.CM_ADDR_LINE3,CM.CM_POSTCODE,CM.CM_CITY," _
                                    & "CM.CM_STATE,CM.CM_COUNTRY,CM.CM_PHONE,CM.CM_FAX,CM.CM_EMAIL " _
                                    & "FROM RFQ_MSTR M,COMPANY_MSTR CM " _
                                    & "WHERE CM.CM_COY_ID='" & Common.Parse(oRow.Item("CoyId")) & "'"
                                Common.Insert2Ary(strAryQuery, strsql)
                            Next
                        End If
                    End If
                Next

            Else    'Update RFQ
                'Remove existing records
                strsql = "delete from RFQ_INVITED_VENLIST_MSTR " _
                       & "where RIVMT_User_ID='" & Common.Parse(HttpContext.Current.Session("UserID")) & "' " _
                       & "and RIVMT_RFQ_ID = " & rfq_id
                ' Common.Insert2Ary(strAryQuery, strsql)
                objDb.Execute(strsql)
                strsql = "delete from RFQ_INVITED_VENLIST_DETAIL " _
                      & "where RTVDT_User_Id='" & Common.Parse(HttpContext.Current.Session("UserID")) & "' " _
                      & "and RTVDT_RFQ_ID = " & rfq_id
                'Common.Insert2Ary(strAryQuery, strsql)
                objDb.Execute(strsql)
                strsql = "delete from RFQ_INVITED_VENLIST where riv_rfq_id = " & rfq_id
                'Common.Insert2Ary(strAryQuery, strsql)
                objDb.Execute(strsql)
                'Save new records
                For Each drList In dtList.Rows
                    If drList.Item("TYPE") = "list" Then
                        strsql = "insert into RFQ_INVITED_VENLIST_MSTR(RIVMT_User_ID,RIVMT_RFQ_Name," _
                              & "RIVMT_Distribution_list_id,RIVMT_RFQ_ID) " _
                              & "values ('" & Common.Parse(HttpContext.Current.Session("UserID")) & "','" _
                              & Common.Parse(value(0)) & "','" & drList.Item("RVDLM_List_Index") & "'," _
                              & rfq_id & ")"
                        Common.Insert2Ary(strAryQuery, strsql)

                        strSearch = "RVDLM_List_Index='" & drList.Item("RVDLM_List_Index") & "' AND TYPE = 'list'"
                        dtrDetails = dtDetails.Select(strSearch)
                        If dtrDetails.Length > 0 Then
                            For Each oRow As DataRow In dtrDetails
                                strsql = "insert into RFQ_INVITED_VENLIST_DETAIL(RTVDT_User_Id,RTVDT_RFQ_Name," _
                                         & "RTVDT_Distribution_List_Id,RTVDT_v_company_id,RTVDT_RFQ_ID) " _
                                         & "values ('" & Common.Parse(HttpContext.Current.Session("UserID")) & "','" _
                                         & Common.Parse(value(0)) & "','" & drList.Item("RVDLM_List_Index") & "','" _
                                         & Common.Parse(oRow.Item("CoyId")) & "'," & rfq_id & ")"
                                Common.Insert2Ary(strAryQuery, strsql)

                                strsql = "insert into RFQ_INVITED_VENLIST select distinct " _
                                        & rfq_id & ", " _
                                        & "CM.CM_COY_ID," _
                                        & "CM.CM_COY_NAME,CM.CM_ADDR_LINE1,CM.CM_ADDR_LINE2,CM.CM_ADDR_LINE3," _
                                        & "CM.CM_POSTCODE,CM.CM_CITY,CM.CM_STATE,CM.CM_COUNTRY,CM.CM_PHONE,CM.CM_FAX," _
                                        & "CM.CM_EMAIL " _
                                        & "FROM COMPANY_MSTR CM " _
                                        & "WHERE CM.CM_COY_ID='" & Common.Parse(oRow.Item("CoyId")) & "'"
                                Common.Insert2Ary(strAryQuery, strsql)
                            Next
                        End If

                    ElseIf drList.Item("TYPE") = "specific" Then
                        strSearch = "RVDLM_List_Index='" & drList.Item("RVDLM_List_Index") & "' AND TYPE = 'specific'"
                        dtrDetails = dtDetails.Select(strSearch)
                        If dtrDetails.Length > 0 Then
                            For Each oRow As DataRow In dtrDetails
                                strsql = "insert into RFQ_INVITED_VENLIST select DISTINCT " _
                                       & rfq_id & " " _
                                       & ", CM.CM_COY_ID, CM.CM_COY_NAME, " _
                                       & "CM.CM_ADDR_LINE1,CM.CM_ADDR_LINE2,CM.CM_ADDR_LINE3,CM.CM_POSTCODE,CM.CM_CITY," _
                                       & "CM.CM_STATE,CM.CM_COUNTRY,CM.CM_PHONE,CM.CM_FAX,CM.CM_EMAIL " _
                                       & "FROM RFQ_MSTR M,COMPANY_MSTR CM " _
                                       & "WHERE CM.CM_COY_ID='" & Common.Parse(oRow.Item("CoyId")) & "'"
                                Common.Insert2Ary(strAryQuery, strsql)
                            Next
                        End If
                    End If
                Next

            End If

            strsql = "delete from RFQ_DETAIL "
            strsql &= "where RD_RFQ_ID = '" & rfq_id & "'"
            Common.Insert2Ary(strAryQuery, strsql)

            ' _Yap
            For i = 0 To dt.Rows.Count - 1
                strsql = "insert into RFQ_DETAIL (RD_RFQ_ID,RD_Coy_ID,RD_RFQ_Line,"
                strsql &= "RD_Vendor_Item_Code,RD_Quantity,RD_Product_Desc, RD_PRODUCT_CODE, "
                strsql &= "RD_UOM,RD_Delivery_Lead_Time,RD_Warranty_Terms, "
                strsql &= "RD_Product_Name,RD_ITEM_TYPE,RD_PR_LINE_INDEX) VALUES ( "
                If blnInsert Then
                    strsql &= " " & objDb.GetLatestInsertedID("RFQ_MSTR") & " , "
                Else
                    strsql &= Common.Parse(rfq_id) & ", "
                End If

                strsql &= "'" & Common.Parse(dt.Rows(i)("VCoyId")) & "'," & (i + 1) & ","
                strsql &= "'" & Common.Parse(dt.Rows(i)("VIC")) & "',"
                strsql &= "" & Common.Parse(dt.Rows(i)("Qty")) & ",'" & Common.Parse(dt.Rows(i)("ProductDesc")) & "' ,"
                strsql &= "'" & Common.Parse(dt.Rows(i)("ProductID")) & "',"
                strsql &= "'" & Common.Parse(dt.Rows(i)("UOM")) & "'," & Common.Parse(dt.Rows(i)("DeliveryLeadTime")) & "," & Common.Parse(dt.Rows(i)("WarrantyTerm")) & ","
                strsql &= "'" & Common.Parse(dt.Rows(i)("ProductDesc")) & "', '" & Common.Parse(dt.Rows(i)("ItemType")) & "' ,"
                If IsDBNull(dt.Rows(i)("PR_LINE_INDEX")) Then
                    strsql &= "NULL)"
                Else
                    If dt.Rows(i)("PR_LINE_INDEX") = "" Then
                        strsql &= "NULL)"

                    Else
                        strsql &= "'" & Common.Parse(dt.Rows(i)("PR_LINE_INDEX")) & "' )"
                    End If
                End If

                Common.Insert2Ary(strAryQuery, strsql)
            Next

            If rfq_id = "" Then ' _Yap: New
                'update the Doc Attachement
                strsql = "update COMPANY_DOC_ATTACHMENT set CDA_DOC_NO = " & rfq_num & " "
                strsql &= "where CDA_DOC_NO = '" & HttpContext.Current.Session.SessionID & "' AND CDA_DOC_TYPE = 'RFQ' "
                Common.Insert2Ary(strAryQuery, strsql)

                strsql = " SET @T_NO = " & rfq_num & "; "
                Common.Insert2Ary(strAryQuery, strsql)

                strsql = "UPDATE company_param SET cp_param_value = CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CP_PARAM_TYPE = 'RFQ' "
                Common.Insert2Ary(strAryQuery, strsql)
            Else
                'update the Doc Attachement
                strsql = "update COMPANY_DOC_ATTACHMENT set CDA_DOC_NO = '" & rfq_num & "' "
                strsql &= "where CDA_DOC_NO = '" & HttpContext.Current.Session.SessionID & "' AND CDA_DOC_TYPE = 'RFQ' "
                Common.Insert2Ary(strAryQuery, strsql)

                strsql = " SET @T_NO = '" & rfq_num & "'; "
                Common.Insert2Ary(strAryQuery, strsql)
            End If


            If strAryQuery(0) <> String.Empty Then
                '' ''If objDb.BatchExecute(strAryQuery) Then
                Dim strTRFQNo As String = ""
                If objDb.BatchExecuteNew(strAryQuery, , strTRFQNo, "T_NO") Then
                    'strsql = "Select RM_RFQ_ID, RM_RFQ_No From RFQ_MSTR Where RM_RFQ_Name = '" & Common.Parse(value(0)) & "' "
                    strsql = "Select RM_RFQ_ID, RM_RFQ_No From RFQ_MSTR Where RM_RFQ_NO = '" & strTRFQNo & "' "
                    strsql &= "AND RM_Created_By = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' "
                    strsql &= "AND RM_Coy_ID = '" & HttpContext.Current.Session("CompanyID") & "' "
                    strsql &= "AND RM_B_DISPLAY_STATUS = '0'"

                    Dim tDS As DataSet = objDb.FillDs(strsql)
                    If tDS.Tables(0).Rows.Count > 0 Then
                        rfq_id = tDS.Tables(0).Rows(0).Item("RM_RFQ_ID")
                        rfq_num = tDS.Tables(0).Rows(0).Item("RM_RFQ_No")
                    End If
                End If
            End If
        End Function

        'Michelle (3/12/2010) - To store into the temp table
        Public Function add_RFQCat_TEMP(ByVal ds As DataSet) As String
            Dim i As Integer
            Dim strAryQuery(0) As String

            For i = 0 To ds.Tables(0).Rows.Count - 1
                Dim strsql As String = "insert into RFQ_DETAIL_TEMP " & _
                "(RD_USER_ID, RD_Coy_ID, RD_Product_Code," & _
                "RD_Quantity,RD_Product_Desc," & _
                "RD_UOM,RD_Delivery_Lead_Time, RD_Action, RD_ITEM_TYPE) VALUES " & _
                "( '" & HttpContext.Current.Session("UserID") & "', '" & Common.Parse(ds.Tables(0).Rows(i)("vendor_Id")) & "','" & Common.Parse(ds.Tables(0).Rows(i)("product_ID")) & " ', " & _
                "'" & Common.Parse(ds.Tables(0).Rows(i)("Quantity")) & "','" & Common.Parse(ds.Tables(0).Rows(i)("item_desc")) & "'," & _
                "'" & Common.Parse(ds.Tables(0).Rows(i)("uom")) & "','" & Common.Parse(ds.Tables(0).Rows(i)("Delivery_Lead_Time")) & "'," & _
                "'A','" & Common.Parse(ds.Tables(0).Rows(i)("stock_type")) & "')"
                Common.Insert2Ary(strAryQuery, strsql)
            Next

            objDb.BatchExecute(strAryQuery)
        End Function

        Public Function add_RFQCatSearch_TEMP(ByVal strProdcode As String) As String
            Dim i As Integer
            Dim strAryQuery(0) As String

            Dim strsql As String = "insert into RFQ_DETAIL_TEMP " & _
                "(RD_USER_ID, RD_Coy_ID, RD_Product_Code," & _
                "RD_Quantity,RD_Product_Desc," & _
                "RD_UOM,RD_Delivery_Lead_Time, RD_Action, RD_ITEM_TYPE) SELECT " & _
                "'" & HttpContext.Current.Session("UserID") & "', PM_S_COY_ID, PM_PRODUCT_CODE, " & _
                "0, PM_PRODUCT_DESC, PM_UOM, 0, 'A','' FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE IN (" & _
                strProdcode & ")"

            objDb.Execute(strsql)
        End Function

        Public Function copy_rfq(ByVal rfq_name As String, ByVal rfq_id As String, ByVal b_disStatus As String, ByVal rm_status As String) As String

            Dim rfq_no, newRFqId As String
            Dim objglb As New AppGlobals
            Dim prefix As String
            Dim Array(0) As String
            objglb.GetLatestDocNo("RFQ", Array, rfq_no, prefix)
            Dim strsql As String

            ' Ai Chu modified on 26/11/2005
            ' user may change last used no in company param - may cause duplication of RFQ NO
            If chk_rfqid(rfq_no) = True Then
                copy_rfq = "-1"
                Exit Function
            End If
            newRFqId = get_rfqid()

            strsql = "insert into RFQ_MSTR (RM_Coy_ID,RM_RFQ_No,RM_RFQ_Name,RM_Expiry_Date,RM_Status,RM_Remark,RM_Created_By,RM_Created_On,RM_Currency_Code," & _
            " RM_Payment_Term,RM_Payment_Type,RM_Shipment_Term,RM_Shipment_Mode,RM_Prefix,RM_B_Display_Status,RM_Reqd_Quote_Validity,RM_Contact_Person,RM_Contact_Number,RM_Email,RM_RFQ_OPTION,RM_VEN_DISTR_LIST_INDEX,RM_DEL_CODE )" & _
            "SELECT RM_Coy_ID,'" & rfq_no & " ','" & Replace(rfq_name, "'", "''") & "'," & Common.ConvertDate(Now.Today) & ",'" & rm_status & "',RM_Remark,RM_Created_By," & Common.ConvertDate(Now.Today) & ",RM_Currency_Code,RM_Payment_Term,RM_Payment_Type,RM_Shipment_Term,RM_Shipment_Mode, " & _
            " RM_Prefix,'" & b_disStatus & "' ," & Common.ConvertDate(Now.Today.AddDays(1)) & ",RM_Contact_Person,RM_Contact_Number,RM_Email,RM_RFQ_OPTION,RM_VEN_DISTR_LIST_INDEX,RM_DEL_CODE " & _
            "  FROM RFQ_MSTR WHERE RM_RFQ_ID='" & rfq_id & "'"
            'RM_RFQ_ID,RM_Coy_ID,RM_RFQ_No,RM_RFQ_Name,RM_Expiry_Date,RM_Status,RM_Remark,RM_Created_By,RM_Created_On,RM_Currency_Code,RM_Payment_Term,RM_Payment_Type,RM_Shipment_Term,RM_Shipment_Mode,RM_Prefix,RM_B_Display_Status,RM_Reqd_Quote_Validity,RM_Contact_Person,RM_Contact_Number,RM_Email,RM_RFQ_OPTION,RM_VEN_DISTR_LIST_INDEX
            Common.Insert2Ary(Array, strsql)

            strsql = "insert into RFQ_DETAIL ( RD_RFQ_ID,RD_Coy_ID,RD_RFQ_Line,RD_Product_Code,RD_Vendor_Item_Code,RD_Quantity,RD_Product_Desc,RD_UOM,RD_Delivery_Lead_Time,RD_Warranty_Terms,RD_Product_Name,RD_Item_Type)" & _
            "SELECT '" & newRFqId & "',RD_Coy_ID,RD_RFQ_Line,RD_Product_Code,RD_Vendor_Item_Code,RD_Quantity,RD_Product_Desc,RD_UOM,RD_Delivery_Lead_Time,RD_Warranty_Terms,RD_Product_Name,RD_Item_Type " & _
            "  from RFQ_DETAIL WHERE RD_RFQ_ID='" & rfq_id & "' "

            Common.Insert2Ary(Array, strsql)

            '  objFile.FileUpload(File1, EnumUploadType.DocAttachment, "RFQ", EnumUploadFrom.FrontOff, Session("RFQ_Num"))
            strsql = "insert into  COMPANY_DOC_ATTACHMENT(CDA_COY_ID,CDA_DOC_NO,CDA_DOC_TYPE," & _
            " CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE) SELECT CDA_COY_ID,'" & rfq_no & "'," & _
            "CDA_DOC_TYPE,CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE FROM COMPANY_DOC_ATTACHMENT " & _
            " WHERE CDA_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND CDA_DOC_NO='" & Me.get_docNUm(rfq_id) & "' AND CDA_DOC_TYPE='RFQ'"
            Common.Insert2Ary(Array, strsql)

            strsql = "insert into  RFQ_INVITED_VENLIST_MSTR(RIVMT_User_ID,RIVMT_RFQ_Name,RIVMT_Distribution_list_id,RIVMT_RFQ_ID)" & _
            "select RIVMT_User_ID,'" & Replace(rfq_name, "'", "''") & "',RIVMT_Distribution_list_id," & newRFqId & _
            " from RFQ_INVITED_VENLIST_MSTR where  " & _
            " RIVMT_User_ID='" & HttpContext.Current.Session("UserID") & "' and RIVMT_RFQ_Name='" & Replace(Me.rfqname(rfq_id), "'", "''") & "' AND RIVMT_RFQ_ID = '" & rfq_id & "' "
            Common.Insert2Ary(Array, strsql)

            strsql = "insert into  RFQ_INVITED_VENLIST_DETAIL(RTVDT_User_Id,RTVDT_RFQ_Name,RTVDT_Distribution_List_Id,RTVDT_v_company_id,RTVDT_RFQ_ID)" & _
                  "select RTVDT_User_Id,'" & Replace(rfq_name, "'", "''") & "',RTVDT_Distribution_List_Id,RTVDT_v_company_id," & newRFqId & " from RFQ_INVITED_VENLIST_DETAIL where RTVDT_User_Id='" & HttpContext.Current.Session("UserID") & "' and RTVDT_RFQ_Name='" & Replace(Me.rfqname(rfq_id), "'", "''") & "' " & _
                  "AND RTVDT_RFQ_ID = '" & rfq_id & "' "
            Common.Insert2Ary(Array, strsql)

            strsql = "insert into  RFQ_INVITED_VENLIST (RIV_RFQ_ID,RIV_S_Coy_ID,RIV_S_Coy_Name,RIV_S_Addr_Line1,RIV_S_Addr_Line2,RIV_S_Addr_Line3,RIV_S_PostCode,RIV_S_City,RIV_S_State,RIV_S_Country,RIV_S_Phone,RIV_S_Fax,RIV_S_Email)" & _
                "select '" & newRFqId & "',RIV_S_Coy_ID,RIV_S_Coy_Name,RIV_S_Addr_Line1,RIV_S_Addr_Line2,RIV_S_Addr_Line3,RIV_S_PostCode,RIV_S_City,RIV_S_State,RIV_S_Country,RIV_S_Phone,RIV_S_Fax,RIV_S_Email " & _
                " from RFQ_INVITED_VENLIST where RIV_RFQ_ID='" & rfq_id & "' "
            Common.Insert2Ary(Array, strsql)
            objglb = Nothing
            objDb.BatchExecute(Array)
            copy_rfq = newRFqId '- 1
        End Function

        Public Function get_items(ByVal rfq_id As String, Optional ByVal PRMode As String = "") As DataSet
            Dim ds As DataSet
            Dim strsql As String
            strsql = "SET @COUNTREC = 0;SELECT @COUNTREC := (@COUNTREC+1) AS COUNTREC, ZZZ.* FROM("

            If rfq_id <> "" Then
                'Michelle (16/9/2011) - To include in the Warranty Terms (871)
                strsql &= "select RD_COY_ID, IFNULL(PM.PM_PRODUCT_FOR,'') AS PM_PRODUCT_FOR, RD_PRODUCT_CODE, RD_QUANTITY, RD_PRODUCT_DESC, 'A' as Type, " & _
                            "RD_UOM, RD_DELIVERY_LEAD_TIME, RD_VENDOR_ITEM_CODE, RD_WARRANTY_TERMS, RD_RFQ_LINE, RD_ITEM_TYPE from RFQ_DETAIL " & _
                            "LEFT JOIN PRODUCT_MSTR PM ON PM.PM_PRODUCT_CODE =  RD_PRODUCT_CODE " & _
                            "where RD_RFQ_ID ='" & Common.Parse(rfq_id) & "' " & _
                            "and RD_PRODUCT_DESC not in (select RD_PRODUCT_DESC from RFQ_DETAIL_TEMP " & _
                            "where RD_USER_ID = '" & HttpContext.Current.Session("UserID") & "' ) "
                If PRMode = "pr" Then
                    strsql &= " union all "
                Else
                    strsql &= " union "
                End If
            End If
            strsql &= "select RD_COY_ID, IFNULL(PM.PM_PRODUCT_FOR,'') AS PM_PRODUCT_FOR, RD_PRODUCT_CODE, RD_QUANTITY, RD_PRODUCT_DESC, 'T' as Type, " & _
                            "RD_UOM, RD_DELIVERY_LEAD_TIME, RD_VENDOR_ITEM_CODE, 0 as RD_WARRANTY_TERMS, 0 AS RD_RFQ_LINE, " & _
                            "CASE PM.PM_PRODUCT_FOR WHEN 'B' THEN PM.PM_ITEM_TYPE ELSE RD_ITEM_TYPE END AS RD_ITEM_TYPE from RFQ_DETAIL_TEMP " & _
                            "LEFT JOIN PRODUCT_MSTR PM ON PM.PM_PRODUCT_CODE =  RD_PRODUCT_CODE " & _
                            "where RD_USER_ID = '" & HttpContext.Current.Session("UserID") & "' AND RD_ACTION='A' " & _
                            "and RD_PRODUCT_DESC not in (select RD_PRODUCT_DESC from RFQ_DETAIL_TEMP " & _
                            "where RD_USER_ID = '" & HttpContext.Current.Session("UserID") & "' AND RD_ACTION='D') "

            strsql &= " ) ZZZ GROUP BY ZZZ.RD_PRODUCT_DESC ORDER BY COUNTREC; "

            ds = objDb.FillDs(strsql)
            get_items = ds
        End Function

        Public Function get_rfqMstr(ByVal item As RFQ_User_Ext, ByVal RFQ_ID As String)
            Dim tDS As DataSet
            Dim strsql As String = "select * from RFQ_MSTR WHERE RM_RFQ_ID='" & Common.Parse(RFQ_ID) & "'  "
            tDS = objDb.FillDs(strsql)
            If tDS.Tables(0).Rows.Count > 0 Then

                item.RFQ_Req_date = tDS.Tables(0).Rows(0).Item("RM_Reqd_Quote_Validity")
                item.RFQ_ID = tDS.Tables(0).Rows(0).Item("RM_RFQ_ID")
                item.RM_Coy_ID = tDS.Tables(0).Rows(0).Item("RM_Coy_ID")
                item.con_person = tDS.Tables(0).Rows(0).Item("RM_Contact_Person")
                item.phone = tDS.Tables(0).Rows(0).Item("RM_Contact_Number")
                item.email = tDS.Tables(0).Rows(0).Item("RM_Email")
                item.pay_term = tDS.Tables(0).Rows(0).Item("RM_Payment_Term")
                item.pay_type = tDS.Tables(0).Rows(0).Item("RM_Payment_Type")
                item.ship_mode = tDS.Tables(0).Rows(0).Item("RM_Shipment_Mode")
                item.ship_term = tDS.Tables(0).Rows(0).Item("RM_Shipment_Term")
                item.remark = tDS.Tables(0).Rows(0).Item("RM_Remark")
                item.exp_date = tDS.Tables(0).Rows(0).Item("RM_Expiry_Date")
                item.cur_code = tDS.Tables(0).Rows(0).Item("RM_Currency_Code")

            End If

            'Dim sqlstr As String = New String("SELECT IsNull(UM_USER_NAME,'') AS User_Name," & _
            '                               "IsNull(UM_TEL_NO,'') AS Phone, IsNull(UM_EMAIL,'') AS Email  FROM USER_MSTR " & _
            '                               " WHERE USER_MSTR.UM_COMPANY_ID='" & common.parseNull (  HttpContext.Current.Session("V_CompanyId") & _
            '                               "' and USER_MSTR.UM_USER_ID = '" & common.parseNull (  HttpContext.Current.Session("VendorId") ) & "'")
            Dim sqlstr As String = "SELECT IsNull(UM_USER_NAME,'') AS User_Name," & _
                                       "IsNull(UM_TEL_NO,'') AS Phone, IsNull(UM_EMAIL,'') AS Email  FROM USER_MSTR " & _
                                       " WHERE USER_MSTR.UM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & _
                                       "' and USER_MSTR.UM_USER_ID = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "'"

            'Michelle (3/10/2010) - To rectify the problem of not able to create Quotation
            'tDS = objDb.FillDs(strsql)
            tDS = objDb.FillDs(sqlstr)
            If tDS.Tables(0).Rows.Count > 0 Then
                item.vendor_name = tDS.Tables(0).Rows(0).Item("User_Name")
                item.vendor_Con_num = tDS.Tables(0).Rows(0).Item("Phone")
                item.vendor_email = tDS.Tables(0).Rows(0).Item("Email")
            End If


            '//Session("edit")=1 mean 'Resubmit' else 'Reply'(first time)
            '//Summary Screen --(a)--> Step 1 -- Next(b)--> Step 2 -- Next(c) --> Step 3 -- Submit --> Final(f)--
            '//Step 3 -- Back(d)--> Step 2 -- Back(e) --> Step 1
            '//Refer to Function "get_quotation" on how data retrieve from DB for (b),(c),(d),(e)
            '//Program set session("edit")=1 at (b) if RFQ_REPLIES_DETAIL_TEMP have data
            '//Problem : when (d) and (e) --> should show temporary save data.
            '//          when (a)         --> should not show temporary save data     
            '//          (a) and (e) redirect to same screen and both are PostBack=false

            '//Temporary solution : everytime user select a RFQ at summary screen, delete the temporary saved data
            '//for Resubmit, have to copy last reply quotation from RFQ_REPLIES_DETAIL
            '//Session("BackToStep1")=true --> case (e) else case (a)
            '//Buyer can only see quotation that save to RFQ_REPLIES_DETAIL

            If Not HttpContext.Current.Session("BackToStep1") Then
                strsql = "Delete From RFQ_REPLIES_DETAIL_TEMP where RRDT_RFQ_ID='" & Common.Parse(RFQ_ID) & "' and RRDT_V_Company_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'"
                Dim strAry(0) As String
                Common.Insert2Ary(strAry, strsql)
                If HttpContext.Current.Session("edit") = "1" Then
                    strsql = "INSERT INTO RFQ_REPLIES_DETAIL_TEMP(RRDT_RFQ_ID,RRDT_V_Company_ID,RRDT_Line_No,RRDT_Product_ID,RRDT_Product_Name,RRDT_Quantity,RRDT_Unit_Price,RRDT_GST_Code,RRDT_GST,RRDT_GST_Desc,RRDT_Product_Desc,RRDT_UOM,RRDT_Delivery_Lead_Time,RRDT_Warranty_Terms,RRDT_Min_Pack_Qty,RRDT_Min_Order_Qty,RRDT_Remarks,RRDT_Tolerance)" & _
                    "SELECT RRD_RFQ_ID,RRD_V_Coy_Id,RRD_Line_No,RRD_Product_Code,RRD_Vendor_Item_Code,RRD_Quantity,RRD_Unit_Price,RRD_GST_Code,RRD_GST,RRD_GST_Desc,RRD_Product_Desc,RRD_UOM,RRD_Delivery_Lead_Time,RRD_Warranty_Terms,RRD_Min_Pack_Qty,RRD_Min_Order_Qty,RRD_Remarks,RRD_Tolerance " & _
                    "FROM RFQ_REPLIES_DETAIL WHERE RRD_RFQ_ID='" & Common.Parse(RFQ_ID) & "' AND RRD_V_Coy_Id='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'"
                    Common.Insert2Ary(strAry, strsql)
                Else
                    'Michelle (3/10/2010) - To insert a space before the From statement
                    'strsql = "INSERT INTO RFQ_REPLIES_DETAIL_TEMP(RRDT_RFQ_ID,RRDT_V_Company_ID,RRDT_Line_No,RRDT_Product_ID,RRDT_Product_Name,RRDT_Quantity,RRDT_Unit_Price,RRDT_GST_Code,RRDT_GST,RRDT_GST_Desc,RRDT_Product_Desc,RRDT_UOM,RRDT_Delivery_Lead_Time,RRDT_Warranty_Terms,RRDT_Min_Pack_Qty,RRDT_Min_Order_Qty,RRDT_Remarks,RRDT_Tolerance)" & _
                    '"SELECT RD_RFQ_ID,'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "',RD_RFQ_Line,RD_Product_Code,RD_Vendor_Item_Code,RD_Quantity,0,'NR',0,'Non-registered',RD_Product_Desc,RD_UOM,RD_Delivery_Lead_Time,RD_Warranty_Terms,1,1,'',0" & _
                    '"FROM RFQ_DETAIL WHERE RD_RFQ_ID='" & Common.Parse(RFQ_ID) & "'"
                    strsql = "INSERT INTO RFQ_REPLIES_DETAIL_TEMP(RRDT_RFQ_ID,RRDT_V_Company_ID,RRDT_Line_No,RRDT_Product_ID,RRDT_Product_Name,RRDT_Quantity,RRDT_Unit_Price,RRDT_GST_Code,RRDT_GST,RRDT_GST_Desc,RRDT_Product_Desc,RRDT_UOM,RRDT_Delivery_Lead_Time,RRDT_Warranty_Terms,RRDT_Min_Pack_Qty,RRDT_Min_Order_Qty,RRDT_Remarks,RRDT_Tolerance)" & _
                    "SELECT RD_RFQ_ID,'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "',RD_RFQ_Line,RD_Product_Code,RD_Vendor_Item_Code,RD_Quantity,NULL,'NR',0,'Non-registered',RD_Product_Desc,RD_UOM,RD_Delivery_Lead_Time,RD_Warranty_Terms,1,1,'',0" & _
                    " FROM RFQ_DETAIL WHERE RD_RFQ_ID='" & Common.Parse(RFQ_ID) & "'"
                    Common.Insert2Ary(strAry, strsql)
                End If
                objDb.BatchExecute(strAry) '//if fail, how??????
            End If
        End Function

        Public Function Download_RFQProductExcel_Common(ByVal rfqid As Integer, ByVal CompID As String, Optional ByVal strItemLine As String = "") As DataSet
            Dim dload As New DataSet
            Dim strSQL As String

            'strSQL = "SELECT RRD.RRD_VENDOR_ITEM_CODE AS 'Item Code', RRD.RRD_PRODUCT_DESC AS 'Item Name', " & _
            '        "'Stock (Direct material - Inventoried item)' AS 'Item Type', IFNULL(PM.PM_LONG_DESC,'') AS 'Description', " & _
            '        "IFNULL((SELECT CT_CODE FROM COMMODITY_TYPE WHERE CT_ID = PM.PM_CATEGORY_NAME),'') AS 'Commodity Type', " & _
            '        "RRD.RRD_UOM AS 'Uom', " & _
            '        "IFNULL((SELECT CBC_B_CATEGORY_CODE FROM COMPANY_B_CATEGORY_CODE WHERE CBC_B_CATEGORY_CODE = PM.PM_CAT_CODE  AND CBC_B_COY_ID = PM.PM_S_COY_ID),'') AS 'Category Code', " & _
            '        "IFNULL(PM.PM_REF_NO,'') AS 'Reference No', CASE PM.PM_IQC_IND WHEN 'Y' THEN 'Yes' WHEN 'N' THEN 'No' ELSE '' END AS 'IQC', " & _
            '        "CASE PM_PARTIAL_CD WHEN 'Y' THEN 'Yes' WHEN 'N' THEN 'No' ELSE '' END AS 'Partial CD', " & _
            '        "IFNULL((SELECT CBG_B_GL_CODE FROM COMPANY_B_GL_CODE WHERE CBG_B_GL_CODE = PM.PM_ACCT_CODE AND CBG_B_COY_ID = PM.PM_S_COY_ID),'') AS 'Account Code', " & _
            '        "IFNULL(PM.PM_ORD_MIN_QTY,0) AS 'Order Min Qty', IFNULL(PM.PM_ORD_MAX_QTY,0) AS 'Order Max Qty', IFNULL(PM.PM_SAFE_QTY,0) AS 'Safe Qty', " & _
            '        "IFNULL(PM.PM_MAX_INV_QTY,0) AS 'Max Inv Qty', IFNULL(PM.PM_REORDER_QTY,0) AS 'Reorder Qty', " & _
            '        "IFNULL(PM.PM_BUDGET_PRICE,0) AS 'Budget Price', PM.PM_IQC_TYPE AS 'IQC Type', PM.PM_EOQ AS 'EOQ', PM.PM_RATIO AS 'Ratio', " & _
            '        "CASE PM.PM_OVERSEA WHEN 'Y' THEN 'Yes' WHEN 'N' THEN 'No' ELSE " & _
            '        "(SELECT IF(CDT_DEL_OVERSEA = 'N', 'No', 'Yes') AS CDT_DEL_OVERSEA FROM COMPANY_DELIVERY_TERM WHERE CDT_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
            '        "AND CDT_DEL_CODE = RRD_DEL_CODE) END AS 'Oversea', IFNULL(PM.PM_PRODUCT_BRAND,'') AS 'Brand', " & _
            '        "IFNULL(PM.PM_DRAW_NO,'') AS 'Drawing No', IFNULL(PM.PM_PRODUCT_MODEL,'') AS 'Model', IFNULL(PM.PM_GROSS_WEIGHT,'') AS 'Weight', " & _
            '        "IFNULL(PM.PM_NET_WEIGHT,'') AS 'Net Weight', IFNULL(PM.PM_LENGHT,'') AS 'Length', IFNULL(PM.PM_VERS_NO,'') AS 'Version No', " & _
            '        "IFNULL(PM.PM_WIDTH,'') AS 'Width', IFNULL(PM.PM_COLOR_INFO,'') AS 'Color Info', " & _
            '        "IFNULL(PM.PM_VOLUME,'') AS 'Volume', IFNULL(PM.PM_HSC_CODE,'') AS 'HSC Code', IFNULL(PM.PM_HEIGHT,'') AS 'Height', " & _
            '        "IFNULL(PM.PM_SPEC1,'') AS 'Specification 1', IFNULL(PM.PM_SPEC2,'') AS 'Specification 2', IFNULL(PM.PM_SPEC3,'') AS 'Specification 3', " & _
            '        "IFNULL(PM.PM_PACKING_TYPE,'') AS 'Packing Type', PM.PM_PACKING_QTY AS 'Packing Qty', " & _
            '        "IFNULL(PM.PM_MANUFACTURER,'') AS 'Manufacturer', IFNULL(PM.PM_MANUFACTURER2,'') AS 'Manufacturer 2', IFNULL(PM.PM_MANUFACTURER3,'') AS 'Manufacturer 3', " & _
            '        "IFNULL(PM.PM_SECTION,'') AS 'Section', IFNULL(PM.PM_LOCATION,'') AS 'Location', IFNULL(PM.PM_NEW_ITEM_CODE,'') AS 'New Item Code', " & _
            '        "IFNULL(PM.PM_REMARKS,'') AS 'Remarks', " & _
            '        "CASE PM.PM_PRODUCT_FOR WHEN 'B' THEN 'M' ELSE 'N' END AS 'Action' " & _
            '        "FROM RFQ_REPLIES_DETAIL RRD " & _
            '        "LEFT JOIN PRODUCT_MSTR PM ON PM.PM_PRODUCT_CODE = RRD.RRD_PRODUCT_CODE " & _
            '        "WHERE RRD.RRD_RFQ_ID=" & rfqid & " AND RRD.RRD_V_Coy_Id='" & Common.Parse(CompID) & "' "

            strSQL = "SELECT RRD.RRD_VENDOR_ITEM_CODE AS 'Item Code', RRD.RRD_PRODUCT_DESC AS 'Item Name', " & _
                    "'Stock (Direct material - Inventoried item)' AS 'Item Type', IFNULL(PM.PM_LONG_DESC,'') AS 'Description', " & _
                    "IFNULL((SELECT CT_NAME FROM COMMODITY_TYPE WHERE CT_ID = PM.PM_CATEGORY_NAME),'SEH-STOCK') AS 'Commodity Type', " & _
                    "RRD.RRD_UOM AS 'Uom', " & _
                    "IFNULL((SELECT CBC_B_CATEGORY_CODE FROM COMPANY_B_CATEGORY_CODE WHERE CBC_B_CATEGORY_CODE = PM.PM_CAT_CODE  AND CBC_B_COY_ID = PM.PM_S_COY_ID),'') AS 'Category Code', " & _
                    "IFNULL(PM.PM_REF_NO,'') AS 'Reference No', CASE PM.PM_IQC_IND WHEN 'Y' THEN 'Yes' WHEN 'N' THEN 'No' ELSE '' END AS 'IQC', " & _
                    "CASE PM_PARTIAL_CD WHEN 'Y' THEN 'Yes' WHEN 'N' THEN 'No' ELSE 'Yes' END AS 'Partial CD', " & _
                    "IFNULL((SELECT CBG_B_GL_CODE FROM COMPANY_B_GL_CODE WHERE CBG_B_GL_CODE = PM.PM_ACCT_CODE AND CBG_B_COY_ID = PM.PM_S_COY_ID),'') AS 'Account Code', " & _
                    "IFNULL(PM.PM_ORD_MIN_QTY,0) AS 'Order Min Qty', IFNULL(PM.PM_ORD_MAX_QTY,0) AS 'Order Max Qty', IFNULL(PM.PM_SAFE_QTY,0) AS 'Safe Qty', " & _
                    "IFNULL(PM.PM_MAX_INV_QTY,0) AS 'Max Inv Qty', IFNULL(PM.PM_REORDER_QTY,0) AS 'Reorder Qty', " & _
                    "IFNULL(PM.PM_BUDGET_PRICE,0) AS 'Budget Price', IFNULL(PM.PM_IQC_TYPE,'') AS 'IQC Type', IFNULL(PM.PM_EOQ,'') AS 'EOQ', IFNULL(PM.PM_RATIO,'') AS 'Ratio', " & _
                    "CASE PM.PM_OVERSEA WHEN 'Y' THEN 'Yes' WHEN 'N' THEN 'No' ELSE " & _
                    "(SELECT IF(CDT_DEL_OVERSEA = 'N', 'No', 'Yes') AS CDT_DEL_OVERSEA FROM COMPANY_DELIVERY_TERM WHERE CDT_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
                    "AND CDT_DEL_CODE = RRD_DEL_CODE) END AS 'Oversea', IFNULL(PM.PM_PRODUCT_BRAND,'') AS 'Brand', " & _
                    "IFNULL(PM.PM_DRAW_NO,'') AS 'Drawing No', IFNULL(PM.PM_PRODUCT_MODEL,'') AS 'Model', IFNULL(PM.PM_GROSS_WEIGHT,'') AS 'Weight', " & _
                    "IFNULL(PM.PM_NET_WEIGHT,'') AS 'Net Weight', IFNULL(PM.PM_LENGHT,'') AS 'Length', IFNULL(PM.PM_VERS_NO,'') AS 'Version No', " & _
                    "IFNULL(PM.PM_WIDTH,'') AS 'Width', IFNULL(PM.PM_COLOR_INFO,'') AS 'Color Info', " & _
                    "IFNULL(PM.PM_VOLUME,'') AS 'Volume', IFNULL(PM.PM_HSC_CODE,'') AS 'HSC Code', IFNULL(PM.PM_HEIGHT,'') AS 'Height', " & _
                    "IFNULL(PM.PM_SPEC1,'') AS 'Specification 1', IFNULL(PM.PM_SPEC2,'') AS 'Specification 2', IFNULL(PM.PM_SPEC3,'') AS 'Specification 3', " & _
                    "(SELECT CONCAT(CPT_PACK_CODE, ' (', CPT_PACK_NAME, ')') FROM COMPANY_PACKING_TYPE WHERE CPT_PACK_CODE = PM.PM_PACKING_TYPE AND CPT_COY_ID = 'beta') AS 'Packing Type', " & _
                    "IFNULL(PM.PM_PACKING_QTY,0) AS 'Packing Qty', " & _
                    "IFNULL(PM.PM_MANUFACTURER,'') AS 'Manufacturer', IFNULL(PM.PM_MANUFACTURER2,'') AS 'Manufacturer 2', IFNULL(PM.PM_MANUFACTURER3,'') AS 'Manufacturer 3', " & _
                    "IFNULL(PM.PM_SECTION,'') AS 'Section', IFNULL(PM.PM_LOCATION,'') AS 'Location', IFNULL(PM.PM_NEW_ITEM_CODE,'') AS 'New Item Code', " & _
                    "IFNULL(PM.PM_REMARKS,'') AS 'Remarks', " & _
                    "CASE PM.PM_PRODUCT_FOR WHEN 'B' THEN 'M' ELSE 'N' END AS 'Action' " & _
                    "FROM RFQ_REPLIES_DETAIL RRD " & _
                    "LEFT JOIN PRODUCT_MSTR PM ON PM.PM_VENDOR_ITEM_CODE = RRD.RRD_VENDOR_ITEM_CODE AND PM.PM_S_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
                    "WHERE RRD.RRD_RFQ_ID=" & rfqid & " AND RRD.RRD_V_Coy_Id='" & Common.Parse(CompID) & "' "

            If strItemLine <> "" Then
                strSQL = strSQL & "AND RRD.RRD_Line_No IN (" & strItemLine & ") "
            End If


            dload = objDb.FillDs(strSQL)
            Download_RFQProductExcel_Common = dload
        End Function

        Public Function Download_RFQProductExcel_Common2(ByVal rfqid As Integer, ByVal CompID As String, Optional ByVal strItemLine As String = "") As DataSet
            Dim dload As New DataSet
            Dim strSQL, strCompId As String
            strCompId = HttpContext.Current.Session("CompanyId")

            'strSQL = "SELECT RRD.RRD_RFQ_ID AS 'RFQ ID', RRD.RRD_Vendor_Item_Code AS 'Item Code', " & _
            '        "(SELECT CM_COY_NAME FROM company_mstr WHERE CM_COY_ID = RRD.RRD_V_COY_ID) AS 'Company Name', '' AS 'Supplier Code', " & _
            '        "RRD.RRD_DEL_CODE AS 'Delivery Term', RRD.RRD_LINE_NO AS 'Item Line', " & _
            '        "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_ABBR = RRM.RRM_PAY_TERM_CODE AND CODE_CATEGORY = 'PT') AS 'Payment Term', " & _
            '        "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_ABBR = RRM.RRM_CURRENCY_CODE) AS 'Currency', " & _
            '        "'' AS 'Purchase Spec No', '' AS 'Revision', " & _
            '        "CAST((CASE RRD.RRD_GST_CODE WHEN '1' THEN 'N/A' ELSE CAST(RRD.RRD_GST_CODE AS UNSIGNED) + 1 END) AS CHAR) AS 'Tax', " & _
            '        "RRD.RRD_DELIVERY_LEAD_TIME AS 'EDD', '' AS 'Vendor Item Code' " & _
            '        "FROM RFQ_REPLIES_DETAIL RRD " & _
            '        "LEFT JOIN RFQ_REPLIES_MSTR RRM ON RRD.RRD_RFQ_ID = RRM.RRM_RFQ_ID " & _
            '        "LEFT JOIN PRODUCT_MSTR PM ON PM.PM_PRODUCT_CODE = RRD.RRD_PRODUCT_CODE " & _
            '        "WHERE RRD.RRD_RFQ_ID=" & rfqid & " AND RRD.RRD_V_Coy_Id='" & Common.Parse(CompID) & "' "

            strSQL = "SELECT ITEM_CODE, COY_NAME, SUPP_CODE, DEL_TERM, " & _
                    "CAST(UNIT_PRICE AS CHAR(1000)) AS UNIT_PRICE, " & _
                    "PAY_TERM, CURR, PUR_SPEC_NO, REVISION, TAX, CAST(LEAD_TIME AS CHAR(1000)) AS LEAD_TIME, VENDOR_ITEM_CODE FROM ( " & _
                    "SELECT CASE PV_VENDOR_TYPE WHEN 'P' THEN 0 ELSE PV_VENDOR_TYPE END AS TYPE_NO, " & _
                    "RRD.RRD_VENDOR_ITEM_CODE AS ITEM_CODE, " & _
                    "(SELECT CM_COY_NAME FROM company_mstr WHERE CM_COY_ID = PV.PV_S_COY_ID) AS COY_NAME, " & _
                    "IFNULL(PV.PV_SUPP_CODE,'') AS SUPP_CODE, " & _
                    "IFNULL(CONCAT(CDT.CDT_DEL_CODE, ' (', CDT.CDT_DEL_NAME, ')'),'') AS DEL_TERM, " & _
                    "(SELECT GROUP_CONCAT(PVP_VOLUME, ':', PVP_VOLUME_PRICE) FROM PRODUCT_VOLUME_PRICE WHERE PVP_PRODUCT_CODE = PM.PM_PRODUCT_CODE AND PVP_VENDOR_TYPE = PV.PV_VENDOR_TYPE) AS UNIT_PRICE, " & _
                    "IFNULL((SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_ABBR = PV_PAYMENT_CODE AND CODE_CATEGORY = 'PT'),'') AS PAY_TERM, " & _
                    "IFNULL((SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_ABBR = PV_CURR),'') AS CURR, " & _
                    "IFNULL(PV_PUR_SPEC_NO,'') AS PUR_SPEC_NO, IFNULL(PV_REVISION,'') AS REVISION, " & _
                    "CAST((CASE PV_S_COY_ID_TAX_ID WHEN '1' THEN 'N/A' ELSE CAST(PV_S_COY_ID_TAX_ID AS UNSIGNED) + 1 END) AS CHAR) AS TAX, " & _
                    "IFNULL(PV_LEAD_TIME,'') AS LEAD_TIME, IFNULL(PV_VENDOR_CODE,'') AS VENDOR_ITEM_CODE " & _
                    "FROM RFQ_REPLIES_DETAIL RRD " & _
                    "INNER JOIN RFQ_REPLIES_MSTR RRM ON RRD.RRD_RFQ_ID = RRM.RRM_RFQ_ID " & _
                    "INNER JOIN PRODUCT_MSTR PM ON PM.PM_VENDOR_ITEM_CODE = RRD.RRD_VENDOR_ITEM_CODE AND PM.PM_S_COY_ID = '" & strCompId & "' " & _
                    "INNER JOIN PIM_VENDOR PV ON PV.PV_PRODUCT_INDEX = PM.PM_PRODUCT_INDEX " & _
                    "LEFT JOIN COMPANY_DELIVERY_TERM CDT ON PV.PV_DELIVERY_TERM = CDT.CDT_DEL_CODE AND CDT.CDT_COY_ID = '" & strCompId & "' " & _
                    "WHERE RRD.RRD_RFQ_ID=" & rfqid & " AND RRD.RRD_V_Coy_Id='" & Common.Parse(CompID) & "' "

            If strItemLine <> "" Then
                strSQL &= "AND RRD.RRD_Line_No IN (" & strItemLine & ") "
            End If

            strSQL &= "UNION ALL " & _
                    "SELECT 9999 AS TYPE_NO, RRD.RRD_Vendor_Item_Code AS ITEM_CODE, " & _
                    "(SELECT CM_COY_NAME FROM company_mstr WHERE CM_COY_ID = RRD.RRD_V_COY_ID) AS COY_NAME, '' AS SUPP_CODE, " & _
                    "CONCAT(CDT.CDT_DEL_CODE, ' (', CDT.CDT_DEL_NAME, ')') AS DEL_TERM, " & _
                    "(SELECT GROUP_CONCAT(RRVP_VOLUME, ':', RRVP_VOLUME_PRICE) FROM RFQ_REPLIES_VOLUME_PRICE WHERE RRVP_RFQ_ID = " & rfqid & " AND RRVP_V_COY_ID = '" & Common.Parse(CompID) & "' AND RRVP_LINE_NO = RRD.RRD_LINE_NO) AS UNIT_PRICE, " & _
                    "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_ABBR = RRM.RRM_PAY_TERM_CODE AND CODE_CATEGORY = 'PT') AS PAY_TERM, " & _
                    "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_ABBR = RRM.RRM_CURRENCY_CODE) AS 'Currency', " & _
                    "'' AS PUR_SPEC_NO, '' AS REVISION, " & _
                    "CAST((CASE RRD.RRD_GST_CODE WHEN '1' THEN 'N/A' ELSE CAST(RRD.RRD_GST_CODE AS UNSIGNED) + 1 END) AS CHAR) AS TAX, " & _
                    "RRD.RRD_DELIVERY_LEAD_TIME AS LEAD_TIME, '' AS VENDOR_ITEM_CODE " & _
                    "FROM RFQ_REPLIES_DETAIL RRD " & _
                    "INNER JOIN RFQ_REPLIES_MSTR RRM ON RRD.RRD_RFQ_ID = RRM.RRM_RFQ_ID " & _
                    "LEFT JOIN COMPANY_DELIVERY_TERM CDT ON RRD.RRD_DEL_CODE = CDT.CDT_DEL_CODE AND CDT.CDT_COY_ID = '" & strCompId & "' " & _
                    "WHERE RRD.RRD_RFQ_ID=" & rfqid & " AND RRD.RRD_V_Coy_Id='" & Common.Parse(CompID) & "' "

            If strItemLine <> "" Then
                strSQL &= "AND RRD.RRD_Line_No IN (" & strItemLine & ") GROUP BY RRD_LINE_NO "
            End If

            strSQL &= ") tb " & _
                    "ORDER BY ITEM_CODE, CAST(TYPE_NO AS UNSIGNED) "

            'strSQL = strSQL & " GROUP BY RRD_LINE_NO "

            dload = objDb.FillDs(strSQL)
            Download_RFQProductExcel_Common2 = dload
        End Function

        Public Function read_rfqMstr(ByVal objread As RFQ_User_Ext, Optional ByVal rfq_name As String = "", Optional ByVal rfq_id As String = "", Optional ByVal rfq_num As String = "") As String

            Dim sqlstr2 As String
            Dim tDS As DataSet
            sqlstr2 = "select * from RFQ_MSTR WHERE 1=1"
            If rfq_name <> "" Then
                sqlstr2 = sqlstr2 & " and RM_RFQ_Name ='" & Common.Parse(rfq_name) & "'"
            End If
            If rfq_num <> "" Then
                sqlstr2 = sqlstr2 & " and RM_RFQ_No = '" & Common.Parse(rfq_num) & "'"
            End If
            If rfq_id <> "" Then
                sqlstr2 = sqlstr2 & " and RM_RFQ_ID = '" & Common.Parse(rfq_id) & "'"
            End If

            If objDb.Exist(sqlstr2) > 0 Then
                tDS = objDb.FillDs(sqlstr2)
                If tDS.Tables(0).Rows.Count > 0 Then
                    objread.RFQ_Name = tDS.Tables(0).Rows(0).Item("RM_RFQ_Name").ToString.Trim
                    objread.RFQ_ID = tDS.Tables(0).Rows(0).Item("RM_RFQ_ID").ToString.Trim
                    objread.bcom_id = tDS.Tables(0).Rows(0).Item("RM_Coy_ID").ToString.Trim
                    objread.RFQ_Num = tDS.Tables(0).Rows(0).Item("RM_RFQ_No").ToString.Trim
                    objread.exp_date = tDS.Tables(0).Rows(0).Item("RM_Expiry_Date").ToString.Trim
                    objread.create_on = tDS.Tables(0).Rows(0).Item("RM_Created_On").ToString.Trim
                    objread.pay_term = tDS.Tables(0).Rows(0).Item("RM_Payment_Term").ToString.Trim
                    objread.pay_type = tDS.Tables(0).Rows(0).Item("RM_Payment_Type").ToString.Trim
                    objread.ship_term = tDS.Tables(0).Rows(0).Item("RM_Shipment_Term").ToString.Trim
                    objread.ship_mode = tDS.Tables(0).Rows(0).Item("RM_Shipment_Mode").ToString.Trim
                    objread.con_person = tDS.Tables(0).Rows(0).Item("RM_Contact_Person").ToString.Trim
                    objread.user_name = tDS.Tables(0).Rows(0).Item("RM_Contact_Person").ToString.Trim
                    objread.phone = tDS.Tables(0).Rows(0).Item("RM_Contact_Number").ToString.Trim
                    objread.email = tDS.Tables(0).Rows(0).Item("RM_Email").ToString.Trim
                    objread.RFQ_Req_date = tDS.Tables(0).Rows(0).Item("RM_Reqd_Quote_Validity").ToString.Trim
                    objread.remark = tDS.Tables(0).Rows(0).Item("RM_Remark").ToString.Trim
                    objread.cur_code = tDS.Tables(0).Rows(0).Item("RM_Currency_Code").ToString.Trim
                    objread.RFQStatus = tDS.Tables(0).Rows(0).Item("RM_STATUS").ToString.Trim
                    objread.BDisplayStatus = tDS.Tables(0).Rows(0).Item("RM_B_Display_Status").ToString.Trim
                    objread.RFQ_Option = tDS.Tables(0).Rows(0).Item("RM_RFQ_Option").ToString.Trim
                    objread.del_code = tDS.Tables(0).Rows(0).Item("RM_DEL_CODE").ToString.Trim
                    '  objread.RFQ_Num = tDS.Tables(0).Rows(0).Item("RM_RFQ_No").TOSTRING.TRIM
                    If objread.exp_date = "" Then
                        Exit Function
                    End If
                End If
                read_rfqMstr = "1"
            Else
                Exit Function
            End If


            Dim sqlstr As String = New String("select CODE_DESC from CODE_MSTR where CODE_ABBR='" & Common.Parse(objread.cur_code) & "'")


            tDS = objDb.FillDs(sqlstr)
            If tDS.Tables(0).Rows.Count > 0 Then
                objread.cur_desc = tDS.Tables(0).Rows(0).Item("CODE_DESC").ToString.Trim
            End If
        End Function

        Public Function get_itemdis(ByVal rfq_id As String) As DataSet
            Dim strsql As String
            'strsql = "select Distinct RD_Product_Desc,RD_RFQ_Line,RD_Quantity,RD_UOM,CASE RD_ITEM_TYPE WHEN 'SP' THEN 'Spot' WHEN 'ST' THEN 'Stock' ELSE 'Mro' END AS ITEM_TYPE, RD_ITEM_TYPE " & _
            '                    "from RFQ_DETAIL where RD_RFQ_ID='" & Common.Parse(rfq_id) & "' ORDER BY RD_RFQ_Line "

            strsql = "SELECT DISTINCT RD_Product_Desc,RD_RFQ_Line,RD_Quantity,RD_UOM," & _
                    "CASE RD_ITEM_TYPE WHEN 'SP' THEN 'Spot' WHEN 'ST' THEN 'Stock' ELSE 'MRO' END AS ITEM_TYPE, RD_ITEM_TYPE," & _
                    "CASE WHEN RD_VENDOR_ITEM_CODE ='' AND (RD_PRODUCT_CODE='' OR RD_PRODUCT_CODE='&nbsp;') THEN " & _
                    "'' ELSE (SELECT PM_OVERSEA FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE=RD_PRODUCT_CODE) END AS OVERSEA " & _
                    "FROM RFQ_DETAIL WHERE RD_RFQ_ID='" & Common.Parse(rfq_id) & "' ORDER BY RD_RFQ_Line "

            Dim dscath As New DataSet
            dscath = objDb.FillDs(strsql)
            get_itemdis = dscath

        End Function

        Public Function get_itemtype(ByVal rfq_id As String, ByRef chkItemSP As Boolean, ByRef chkItemST As Boolean)
            Dim strsql As String

            strsql = "SELECT DISTINCT RD_Product_Desc, RD_ITEM_TYPE FROM RFQ_DETAIL WHERE RD_RFQ_ID='" & Common.Parse(rfq_id) & "' AND RD_ITEM_TYPE <> 'ST' ORDER BY RD_RFQ_Line"
            If objDb.Exist(strsql) Then
                chkItemSP = True
            End If

            strsql = "SELECT DISTINCT RD_Product_Desc, RD_ITEM_TYPE FROM RFQ_DETAIL WHERE RD_RFQ_ID='" & Common.Parse(rfq_id) & "' AND RD_ITEM_TYPE = 'ST' ORDER BY RD_RFQ_Line"
            If objDb.Exist(strsql) Then
                chkItemST = True
            End If

        End Function

        Public Function chk_itemtype(ByVal rfq_id As String, ByVal strItemLine As String, Optional ByVal blnSpot As Boolean = False) As Boolean
            Dim strsql, strTemp, strTemp2 As String
            Dim ds As New DataSet
            Dim i As Integer

            strsql = "SELECT DISTINCT RD_Product_Desc, RD_ITEM_TYPE, RD_RFQ_Line FROM RFQ_DETAIL " & _
                    "WHERE RD_RFQ_ID='" & Common.Parse(rfq_id) & "' AND RD_RFQ_Line IN (" & strItemLine & ") "

            If blnSpot = True Then
                strsql = strsql & "AND RD_ITEM_TYPE IN ('SP','MI') "

                If objDb.Exist(strsql) Then
                    Return False
                Else
                    Return True
                End If
            Else
                'strsql = strsql & "AND RD_ITEM_TYPE NOT IN ('SP','MI') "

                ds = objDb.FillDs(strsql)

                For i = 0 To ds.Tables(0).Rows.Count - 1
                    If i = 0 Then
                        strTemp = ds.Tables(0).Rows(i)("RD_ITEM_TYPE")

                        If strTemp = "ST" Then
                            Return False
                        End If
                    Else
                        strTemp2 = ds.Tables(0).Rows(i)("RD_ITEM_TYPE")
                        If strTemp <> strTemp2 Then
                            Return False
                        End If
                    End If
                Next

                Return True

            End If

        End Function

        Public Function chk_itemoversea(ByVal rfq_id As String, ByVal strItemLine As String, ByVal strSCoyId As String, ByRef strOversea As String, ByRef strDelTerm As String) As Integer
            Dim strsql, stroversea2, strDelterm2, strCoyId As String
            Dim ds As New DataSet
            Dim i As Integer
            strCoyId = HttpContext.Current.Session("CompanyId")

            strsql = "SELECT CASE WHEN PM_OVERSEA IS NULL THEN " & _
                    "(SELECT CDT_DEL_OVERSEA FROM COMPANY_DELIVERY_TERM WHERE CDT_DEL_CODE = RRD_DEL_CODE AND CDT_COY_ID = '" & strCoyId & "') " & _
                    "ELSE PM_OVERSEA END AS OVERSEA, RRD_DEL_CODE FROM RFQ_REPLIES_DETAIL RRD " & _
                    "LEFT JOIN PRODUCT_MSTR PM ON PM.PM_PRODUCT_CODE = RRD.RRD_PRODUCT_CODE " & _
                    "WHERE RRD_RFQ_ID='" & rfq_id & "' AND RRD_V_COY_ID = '" & strSCoyId & "' AND RRD_LINE_NO IN (" & strItemLine & ") "

            ds = objDb.FillDs(strsql)
            For i = 0 To ds.Tables(0).Rows.Count - 1
                If i = 0 Then
                    stroversea = ds.Tables(0).Rows(i)("OVERSEA")
                Else
                    stroversea2 = ds.Tables(0).Rows(i)("OVERSEA")

                    If stroversea <> stroversea2 Then
                        chk_itemoversea = 1
                        Exit Function
                    End If
                End If
            Next

            For i = 0 To ds.Tables(0).Rows.Count - 1
                If i = 0 Then
                    strDelterm = ds.Tables(0).Rows(i)("RRD_DEL_CODE")
                Else
                    strDelterm2 = ds.Tables(0).Rows(i)("RRD_DEL_CODE")

                    If strDelterm <> strDelterm2 Then
                        chk_itemoversea = 2
                        Exit Function
                    End If
                End If
            Next

            chk_itemoversea = 0

        End Function

        Public Function get_venInfo(ByVal rfq_id As Integer, ByVal vendorId As String, Optional ByVal stritemline As String = "") As DataSet
            Dim strsql, strCoyId As String
            Dim ds As New DataSet
            strCoyId = HttpContext.Current.Session("CompanyId")

            strsql = "SELECT RRD.RRD_Item_Type, RRD.RRD_Line_No, RRD.RRD_Quantity, RRD.RRD_Unit_Price, RRD.RRD_DEL_CODE, RRM.RRM_Currency_Code, RRD.RRD_Del_Code, " & _
                    "(RRD_Unit_Price*RRD_Quantity+((RRD_GST*RRD_Unit_Price*RRD_Quantity)/100)) AS Price, " & _
                    "CASE WHEN RRD.RRD_VENDOR_ITEM_CODE ='' THEN " & _
                    "(SELECT CDT_DEL_OVERSEA FROM COMPANY_DELIVERY_TERM WHERE CDT_DEL_CODE = RRD.RRD_DEL_CODE AND CDT_DELETED = 'N' AND CDT_COY_ID = '" & strCoyId & "') " & _
                    "ELSE (SELECT PM_OVERSEA FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE=RRD.RRD_PRODUCT_CODE) END AS OVERSEA " & _
                    "FROM rfq_replies_detail RRD, rfq_replies_mstr RRM WHERE RRD_V_Coy_Id = '" & Common.Parse(vendorId) & "' AND RRD_RFQ_ID = " & rfq_id & " " & _
                    "AND RRM.RRM_V_Company_ID = RRD.RRD_V_Coy_Id AND RRM.RRM_RFQ_ID = RRD.RRD_RFQ_ID "

            'strsql = "SELECT RRD.RRD_Item_Type, RRD.RRD_Line_No, RRD.RRD_Quantity, RRD.RRD_Unit_Price, RRD.RRD_DEL_CODE, RRM.RRM_Currency_Code, RRD.RRD_Del_Code, " & _
            '        "(RRD_Unit_Price*RRD_Quantity+((RRD_GST*RRD_Unit_Price*RRD_Quantity)/100)) AS Price " & _
            '        "FROM rfq_replies_detail RRD, rfq_replies_mstr RRM " & _
            '        "WHERE RRD_V_Coy_Id = '" & Common.Parse(vendorId) & "' AND RRD_RFQ_ID = " & rfq_id & " AND RRM.RRM_V_Company_ID = RRD.RRD_V_Coy_Id " & _
            '        "AND RRM.RRM_RFQ_ID = RRD.RRD_RFQ_ID "

            If stritemline <> "" Then
                strsql = strsql & "AND RRD_Line_No IN (" & stritemline & ")"
            End If

            ds = objDb.FillDs(strsql)
            get_venInfo = ds
        End Function

        Public Function get_quoteVen(ByVal RFQ_ID As String) As DataSet
            Dim dscath As New DataSet
            Dim strsql As String

            strsql = "SELECT RM_COY_ID, RM_RFQ_ID, RVM_V_COMPANY_ID, RRM_V_Company_ID, CM_COY_NAME, RRM_OFFER_TILL, RRM_CURRENCY_CODE, " & _
                    "RRM_ACTUAL_QUOT_NUM, RVM_V_RFQ_STATUS, RM_CREATED_ON, RM_REQD_QUOTE_VALIDITY, RM_RFQ_OPTION, RRM_RFQ_ID, RRM_TOTALVALUE, " & _
                    "RRM_INDICATOR, SUM(RRM_TOTALVALUE2) AS RRM_TOTALVALUE2 FROM " & _
                    "(SELECT tb.*, (PRICE * IFNULL(RATE,0)) + ((PRICE * IFNULL(RATE,0)) * IFNULL(GRNFACTOR,0) / 100) AS RRM_TOTALVALUE2, " & _
                    "(SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = RVM_V_COMPANY_ID) AS CM_COY_NAME FROM " & _
                    "(SELECT RM_COY_ID, RM_RFQ_ID, RM_RFQ_NO, RVM_V_COMPANY_ID, RRM_V_Company_ID, RVM_V_RFQ_STATUS, RRM_OFFER_TILL, " & _
                    "RM_CREATED_ON, RM_RFQ_OPTION, RRM_CURRENCY_CODE, RRM_RFQ_ID, RRM_ACTUAL_QUOT_NUM, RRM_TOTALVALUE, " & _
                    "RM_REQD_QUOTE_VALIDITY, RRM_INDICATOR, RRD_PRODUCT_CODE, RRD_VENDOR_ITEM_CODE, RRD_QUANTITY, RRD_UNIT_PRICE, RRD_GST, " & _
                    "RRD_DEL_CODE, RRD_ITEM_TYPE, (RRD_UNIT_PRICE * RRD_QUANTITY + ((RRD_GST * RRD_UNIT_PRICE * RRD_QUANTITY)/100)) AS PRICE, " & _
                    "(SELECT CDT_DEL_GRNFACTOR FROM COMPANY_DELIVERY_TERM WHERE CDT_DEL_CODE = RRD_DEL_CODE AND CDT_DELETED = 'N' " & _
                    "AND CDT_COY_ID = RM_COY_ID) AS GRNFACTOR, (SELECT IFNULL(CE_RATE,0) FROM COMPANY_EXCHANGERATE " & _
                    "WHERE CE_VALID_FROM <= CURRENT_DATE AND CE_VALID_TO >= CURRENT_DATE AND CE_DELETED = 'N' AND CE_COY_ID = RM_COY_ID " & _
                    "AND CE_CURRENCY_CODE = RRM_CURRENCY_CODE) AS RATE, " & _
                    "CASE WHEN RRD_VENDOR_ITEM_CODE ='' THEN " & _
                    "(SELECT CDT_DEL_OVERSEA FROM COMPANY_DELIVERY_TERM WHERE CDT_DEL_CODE = RRD_DEL_CODE AND CDT_DELETED = 'N' AND CDT_COY_ID = RM_COY_ID) " & _
                    "ELSE (SELECT PM_OVERSEA FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE=RRD_PRODUCT_CODE) END AS OVERSEA " & _
                    "FROM RFQ_MSTR RM LEFT JOIN RFQ_VENDOR_MSTR RVM ON RM.RM_RFQ_ID = RVM.RVM_RFQ_ID " & _
                    "LEFT JOIN RFQ_REPLIES_MSTR RRM ON RRM.RRM_RFQ_ID = RVM.RVM_RFQ_ID AND RRM.RRM_V_Company_ID = RVM.RVM_V_Company_ID " & _
                    "LEFT JOIN RFQ_REPLIES_DETAIL RRD ON RRD.RRD_RFQ_ID = RRM.RRM_RFQ_ID AND RRD.RRD_V_COY_ID = RRM.RRM_V_Company_ID " & _
                    "WHERE RM.RM_RFQ_ID = '" & RFQ_ID & "' AND RM.RM_Status <> 3 AND RM.RM_RFQ_No IS NOT NULL) tb) tb2 " & _
                    "GROUP BY RVM_V_COMPANY_ID ORDER BY RRM_TOTALVALUE2 "

            'strsql = "SELECT RM_COY_ID, RM_RFQ_ID, RVM_V_COMPANY_ID, RRM_V_Company_ID, CM_COY_NAME, RRM_OFFER_TILL, RRM_CURRENCY_CODE, " & _
            '        "RRM_ACTUAL_QUOT_NUM, RVM_V_RFQ_STATUS, RM_CREATED_ON, RM_REQD_QUOTE_VALIDITY, RM_RFQ_OPTION, RRM_RFQ_ID, RRM_TOTALVALUE, " & _
            '        "RRM_INDICATOR, SUM(RRM_TOTALVALUE2) AS RRM_TOTALVALUE2 FROM " & _
            '        "(SELECT tb.*, CASE WHEN OVERSEA <> 'Y' THEN (PRICE * IFNULL(RATE,0)) ELSE (PRICE * IFNULL(RATE,0)) + ((PRICE * IFNULL(RATE,0)) * IFNULL(GRNFACTOR,0) / 100) END AS RRM_TOTALVALUE2, " & _
            '        "(SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = RVM_V_COMPANY_ID) AS CM_COY_NAME FROM " & _
            '        "(SELECT RM_COY_ID, RM_RFQ_ID, RM_RFQ_NO, RVM_V_COMPANY_ID, RRM_V_Company_ID, RVM_V_RFQ_STATUS, RRM_OFFER_TILL, " & _
            '        "RM_CREATED_ON, RM_RFQ_OPTION, RRM_CURRENCY_CODE, RRM_RFQ_ID, RRM_ACTUAL_QUOT_NUM, RRM_TOTALVALUE, " & _
            '        "RM_REQD_QUOTE_VALIDITY, RRM_INDICATOR, RRD_PRODUCT_CODE, RRD_VENDOR_ITEM_CODE, RRD_QUANTITY, RRD_UNIT_PRICE, RRD_GST, " & _
            '        "RRD_DEL_CODE, RRD_ITEM_TYPE, (RRD_UNIT_PRICE * RRD_QUANTITY + ((RRD_GST * RRD_UNIT_PRICE * RRD_QUANTITY)/100)) AS PRICE, " & _
            '        "(SELECT CDT_DEL_GRNFACTOR FROM COMPANY_DELIVERY_TERM WHERE CDT_DEL_CODE = RRD_DEL_CODE AND CDT_DELETED = 'N' " & _
            '        "AND CDT_COY_ID = RM_COY_ID) AS GRNFACTOR, (SELECT IFNULL(CE_RATE,0) FROM COMPANY_EXCHANGERATE " & _
            '        "WHERE CE_VALID_FROM <= CURRENT_DATE AND CE_VALID_TO >= CURRENT_DATE And CE_COY_ID = RM_COY_ID " & _
            '        "AND CE_CURRENCY_CODE = RRM_CURRENCY_CODE) AS RATE, " & _
            '        "CASE WHEN RRD_VENDOR_ITEM_CODE ='' AND (RRD_PRODUCT_CODE='' OR RRD_PRODUCT_CODE='&nbsp;') THEN " & _
            '        "(SELECT CDT_DEL_OVERSEA FROM COMPANY_DELIVERY_TERM WHERE CDT_DEL_CODE = RRD_DEL_CODE AND CDT_DELETED = 'N' AND CDT_COY_ID = RM_COY_ID) " & _
            '        "ELSE (SELECT PM_OVERSEA FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE=RRD_PRODUCT_CODE) END AS OVERSEA " & _
            '        "FROM RFQ_MSTR RM LEFT JOIN RFQ_VENDOR_MSTR RVM ON RM.RM_RFQ_ID = RVM.RVM_RFQ_ID " & _
            '        "LEFT JOIN RFQ_REPLIES_MSTR RRM ON RRM.RRM_RFQ_ID = RVM.RVM_RFQ_ID AND RRM.RRM_V_Company_ID = RVM.RVM_V_Company_ID " & _
            '        "LEFT JOIN RFQ_REPLIES_DETAIL RRD ON RRD.RRD_RFQ_ID = RRM.RRM_RFQ_ID AND RRD.RRD_V_COY_ID = RRM.RRM_V_Company_ID " & _
            '        "WHERE RM.RM_RFQ_ID = '" & RFQ_ID & "' AND RM.RM_Status <> 3 AND RM.RM_RFQ_No IS NOT NULL) tb) tb2 " & _
            '        "GROUP BY RVM_V_COMPANY_ID ORDER BY RRM_TOTALVALUE2 "

            'strsql = "SELECT RM_COY_ID, RM_RFQ_ID, RVM_V_COMPANY_ID, RRM_V_Company_ID, CM_COY_NAME, RRM_OFFER_TILL, RRM_CURRENCY_CODE, RRM_ACTUAL_QUOT_NUM, " & _
            '        "RVM_V_RFQ_STATUS, RM_CREATED_ON, RM_REQD_QUOTE_VALIDITY, RM_RFQ_OPTION, RRM_RFQ_ID, " & _
            '        "RRM_TOTALVALUE, RRM_INDICATOR, SUM(RRM_TOTALVALUE2) AS RRM_TOTALVALUE2 " & _
            '        "FROM (SELECT tb.*, CASE WHEN RRD_ITEM_TYPE <> 'ST' THEN (PRICE * IFNULL(RATE,0)) " & _
            '        "ELSE (PRICE * IFNULL(RATE,0)) + ((PRICE * IFNULL(RATE,0)) * IFNULL(GRNFACTOR,0) / 100) END AS RRM_TOTALVALUE2, " & _
            '        "(SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = RVM_V_COMPANY_ID) AS CM_COY_NAME " & _
            '        "FROM (SELECT RM_COY_ID, RM_RFQ_ID, RM_RFQ_NO, RVM_V_COMPANY_ID, RRM_V_Company_ID, RVM_V_RFQ_STATUS, RRM_OFFER_TILL, RM_CREATED_ON, RM_RFQ_OPTION, " & _
            '        "RRM_CURRENCY_CODE, RRM_RFQ_ID, RRM_ACTUAL_QUOT_NUM, RRM_TOTALVALUE, RM_REQD_QUOTE_VALIDITY, RRM_INDICATOR, " & _
            '        "RRD_VENDOR_ITEM_CODE, RRD_QUANTITY, RRD_UNIT_PRICE, RRD_GST, RRD_DEL_CODE, RRD_ITEM_TYPE, " & _
            '        "(RRD_UNIT_PRICE * RRD_QUANTITY + ((RRD_GST * RRD_UNIT_PRICE * RRD_QUANTITY)/100)) AS PRICE, " & _
            '        "(SELECT CDT_DEL_GRNFACTOR FROM COMPANY_DELIVERY_TERM WHERE CDT_DEL_CODE = RRD_DEL_CODE AND CDT_DELETED = 'N' AND CDT_COY_ID = RM_COY_ID) AS GRNFACTOR, " & _
            '        "(SELECT IFNULL(CE_RATE,0) FROM COMPANY_EXCHANGERATE WHERE CE_VALID_FROM <= CURRENT_DATE AND CE_VALID_TO >= CURRENT_DATE AND CE_COY_ID = RM_COY_ID AND CE_CURRENCY_CODE = RRM_CURRENCY_CODE) AS RATE " & _
            '        "FROM RFQ_MSTR RM " & _
            '        "LEFT JOIN RFQ_VENDOR_MSTR RVM ON RM.RM_RFQ_ID = RVM.RVM_RFQ_ID " & _
            '        "LEFT JOIN RFQ_REPLIES_MSTR RRM ON RRM.RRM_RFQ_ID = RVM.RVM_RFQ_ID AND RRM.RRM_V_Company_ID = RVM.RVM_V_Company_ID " & _
            '        "LEFT JOIN RFQ_REPLIES_DETAIL RRD ON RRD.RRD_RFQ_ID = RRM.RRM_RFQ_ID AND RRD.RRD_V_COY_ID = RRM.RRM_V_Company_ID " & _
            '        "WHERE RM.RM_RFQ_ID = '" & RFQ_ID & "' AND RM.RM_Status <> 3 AND RM.RM_RFQ_No IS NOT NULL) tb) tb2 " & _
            '        "GROUP BY RVM_V_COMPANY_ID ORDER BY RRM_TOTALVALUE2 "

            dscath = objDb.FillDs(strsql)
            get_quoteVen = dscath
        End Function

        Public Function rfqname(ByVal rfq_id As String) As String
            Dim strsql As String = "select RM_RFQ_NAME FROM RFQ_MSTR WHERE RM_RFQ_ID='" & rfq_id & "'"
            rfqname = objDb.GetVal(strsql)

        End Function

        Public Function get_rfqid() As String
            Dim strsQL As String = "select max(RM_RFQ_ID) from RFQ_MSTR"

            get_rfqid = objDb.GetVal(strsQL) + 1
        End Function

        Function chk_rfqid(ByVal rfq_no As String) As Boolean
            If objDb.Exist("select '*' from RFQ_MSTR WHERE RM_Coy_ID= '" & HttpContext.Current.Session("CompanyID") & "' AND RM_RFQ_No='" & rfq_no & "' ") > 0 Then
                chk_rfqid = 1
            End If
        End Function

        Function chk_rfqidnew(ByVal rfq_no As String) As Boolean
            If objDb.Exist("select '*' from RFQ_MSTR WHERE RM_Coy_ID= '" & HttpContext.Current.Session("CompanyID") & "' AND RM_RFQ_No=" & rfq_no & " ") > 0 Then
                chk_rfqidnew = 1
            End If
        End Function

        Public Function get_docNUm(ByVal RFQ_ID As String) As String
            Dim strsQL As String = "select RM_RFQ_NO from RFQ_MSTR where RM_RFQ_ID ='" & RFQ_ID & "'"
            get_docNUm = objDb.GetVal(strsQL)

        End Function

        Public Function chk_DelTerm(ByVal strrfqId As String, ByVal strSCoyId As String) As Boolean
            Dim strSql, strDelTerm As String
            Dim intCount, intCount2 As Integer

            strSql = "SELECT IFNull(RM_DEL_CODE,'') AS RM_DEL_CODE FROM RFQ_MSTR WHERE RM_RFQ_ID='" & strrfqId & "'"
            strDelTerm = objDb.GetVal(strSql)

            If strDelTerm = "" Then
                Return True
            End If

            strSql = "SELECT COUNT('*') FROM RFQ_REPLIES_DETAIL WHERE RRD_RFQ_ID = '" & strrfqId & "' AND RRD_V_Coy_Id = '" & strSCoyId & "'"
            intCount = objDb.GetVal(strSql)
            intCount2 = objDb.GetVal(strSql & " AND RRD_DEL_CODE = '" & Common.Parse(strDelTerm) & "'")

            If intCount = intCount2 Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Function GetOverseaofItem() As String
            Dim strSql, strOversea As String

            strSql = "SELECT IFNULL(PM_OVERSEA,'') FROM RFQ_DETAIL_TEMP " & _
                    "LEFT JOIN PRODUCT_MSTR PM ON PM.PM_PRODUCT_CODE =  RD_PRODUCT_CODE " & _
                    "WHERE RD_USER_ID = '" & HttpContext.Current.Session("UserId") & "' AND RD_ACTION='A' AND RD_PRODUCT_CODE <> '' AND RD_PRODUCT_DESC NOT IN (SELECT RD_PRODUCT_DESC FROM RFQ_DETAIL_TEMP WHERE RD_USER_ID = '" & HttpContext.Current.Session("UserId") & "' AND RD_ACTION='D') GROUP BY RD_PRODUCT_DESC " & _
                    ""
            strOversea = objDb.GetVal(strSql)
            GetOverseaofItem = strOversea
        End Function
#End Region

#Region "vendor RFQ"

#End Region

    End Class



End Namespace



