Imports System
Imports System.Collections
Imports System.Web
Imports AgoraLegacy
Imports SSO.Component
Imports System.IO
Imports System.Web.UI.WebControls

Namespace AgoraLegacy
    Enum VendorDisStatus
        Deletetrash = 2
        trash = 1
        sent = 0
    End Enum

    Enum RFQStatus
        sent = 0
        quote = 1
    End Enum

    Public Class RFQ_User
        Public bcom_id As String
        Public REG_NO As String
        Public list_index As String
        Public gst As String
        Public gst_rate As String
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
        'Public rfq_num As String
        'Public bcoyid As String
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

    Public Class RFQ
        Dim objDb As New EAD.DBCom
        Dim lsSql As String

#Region " Buyer RFQ "

        Public Function get_comname(ByVal comid As String) As String
            Dim strsql As String = "SELECT CM_COY_NAME FROM COMPANY_MSTR  WHERE CM_COY_ID='" & comid & "'"
            get_comname = objDb.GetVal(strsql)
        End Function

        Public Function get_po_no(ByVal rfq_no As String) As String
            Dim strsql As String

            strsql = " SELECT CAST(GROUP_CONCAT(IFNULL(POM_PO_NO,'')) AS CHAR(1000)) AS POM_PO_NO FROM PO_MSTR " & _
                    " WHERE POM_RFQ_INDEX = (SELECT RM_RFQ_ID FROM RFQ_MSTR WHERE RM_RFQ_NO = '" & rfq_no & "' AND RM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "') " & _
                    " AND POM_B_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND POM_PO_STATUS NOT IN ('4', '5', '10', '12', '13') GROUP BY POM_PO_NO "

            get_po_no = objDb.GetVal(strsql)
        End Function

        Public Function send(ByVal rfq_id As String) As String
            Dim checksend As Boolean
            Dim ds As New DataSet
            Dim prefix As String
            Dim rfq_num As String

            ds = check_expire(rfq_id)
            If ds.Tables(0).Rows.Count <> 0 Then 'SEARCH FOR EXPIRE DATE 
                Exit Function
            End If

            If check_item1(rfq_id) = 0 Then 'SEARCH FOR no ITEM , IF NO ITEM RETURN =0
                Exit Function
            End If

            ds = check_noexpire(rfq_id)
            If ds.Tables(0).Rows.Count <> 0 Then
                Exit Function
            End If

            If check_vendor1(rfq_id) = 0 Then
                Exit Function
            End If
            send = Vendor_send(prefix, rfq_num, rfq_id)
        End Function

        Public Function check_expire(ByVal rfq_id As String) As DataSet
            Dim strsql As String = "select distinct * from RFQ_MSTR WHERE RM_RFQ_ID IN(" & rfq_id & ") AND RM_Expiry_Date<" & Common.ConvertDate(Now.Today) & ""
            check_expire = objDb.FillDs(strsql)
        End Function

        Public Function check_noexpire(ByVal rfq_id As String) As DataSet
            Dim strsql As String = "select distinct * from RFQ_MSTR WHERE RM_RFQ_ID IN(" & rfq_id & ") AND RM_Expiry_Date is null"
            check_noexpire = objDb.FillDs(strsql)
        End Function

        Public Function check_item(ByVal rfq_id As String) As DataSet
            Dim strsql As String = " select distinct RM_RFQ_No,RM_RFQ_Name,ISNULL(COUNT(RD_RFQ_ID),'0') AS ITEM from RFQ_MSTR left join rfq_detail on RM_RFQ_ID=RD_RFQ_ID " & _
            " WHERE RM_RFQ_ID IN(" & rfq_id & ")  GROUP BY  RM_RFQ_No, RM_RFQ_Name "
            check_item = objDb.FillDs(strsql)

        End Function

        Public Function check_item1(ByVal rfq_id As String) As String
            Dim ds As New DataSet
            Dim strsql As String = " select distinct RM_RFQ_No,RM_RFQ_Name,CAST(ISNULL(COUNT(RD_RFQ_ID),'0') AS CHAR) AS ITEM from RFQ_MSTR left join rfq_detail on RM_RFQ_ID=RD_RFQ_ID " & _
            " WHERE RM_RFQ_ID IN(" & rfq_id & ")  GROUP BY  RM_RFQ_No, RM_RFQ_Name "
            ds = objDb.FillDs(strsql)
            If ds.Tables(0).Rows.Count > 0 Then
                check_item1 = ds.Tables(0).Rows(0).Item("ITEM").ToString.Trim
            End If
       
        End Function

        Public Function check_send(ByVal rfq_id As String) As DataSet
            Dim strsql As String = "select DISTINCT RM_RFQ_NO,RM_RFQ_NAME from RFQ_VENDOR_MSTR, RFQ_MSTR where " & _
                                    " RVM_RFQ_ID IN(" & Common.Parse(rfq_id) & ") AND RM_RFQ_ID=RVM_RFQ_ID"
            check_send = objDb.FillDs(strsql)
        End Function

        Public Function check_vendor(ByVal rfq_id As String) As DataSet
            Dim ds As DataSet
            Dim strsql As String = "select distinct  RM_RFQ_No,RM_RFQ_Name,ISNULL(COUNT(RIV_RFQ_ID),'0') AS ITEM From RFQ_MSTR left join RFQ_INVITED_VENLIST" & _
            " on RM_RFQ_ID=RIV_RFQ_ID WHERE RM_RFQ_ID IN(" & Common.Parse(rfq_id) & ") GROUP BY  RM_RFQ_No,RM_RFQ_Name "
            ds = objDb.FillDs(strsql)
            Return ds
        End Function

        Public Function check_vendor1(ByVal rfq_id As String) As String
            Dim ds As New DataSet
            Dim strsql As String = "select distinct  RM_RFQ_No,RM_RFQ_Name,CAST(ISNULL(COUNT(RIV_RFQ_ID),'0') AS CHAR) AS ITEM From RFQ_MSTR left join RFQ_INVITED_VENLIST" & _
            " on RM_RFQ_ID=RIV_RFQ_ID WHERE RM_RFQ_ID IN(" & Common.Parse(rfq_id) & ") GROUP BY  RM_RFQ_No,RM_RFQ_Name "
            ds = objDb.FillDs(strsql)
            If ds.Tables(0).Rows.Count > 0 Then
                check_vendor1 = ds.Tables(0).Rows(0).Item("ITEM").ToString.Trim
            End If
        End Function

        Public Function get_paymeth(ByVal item As RFQ_User)

            Dim strsql As String = "select CM_PAYMENT_TERM,CM_PAYMENT_METHOD from COMPANY_MSTR " & _
            " where CM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'"
            Dim tDS As DataSet = objDb.FillDs(strsql)

            If tDS.Tables(0).Rows.Count > 0 Then
                item.pay_term = tDS.Tables(0).Rows(0).Item("CM_PAYMENT_TERM").ToString.Trim
                item.pay_type = tDS.Tables(0).Rows(0).Item("CM_PAYMENT_METHOD").ToString.Trim
            End If

        End Function

        Public Function get_comid(ByVal RFQ_ID As String) As String
            Dim strsql As String = "select RM_Coy_ID from RFQ_MSTR WHERE RM_RFQ_ID='" & RFQ_ID & "'"
            Dim tDS As DataSet = objDb.FillDs(strsql)
            If tDS.Tables(0).Rows.Count > 0 Then
                get_comid = tDS.Tables(0).Rows(0).Item("RM_Coy_ID").ToString.Trim
            End If

        End Function
        Public Function get_rfq_num(ByVal rfq_name As String) As String
            Dim strsql As String = "select RM_RFQ_NO FROM RFQ_MSTR WHERE RM_RFQ_Name='" & Common.Parse(rfq_name) & "' and RM_Created_By= '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' and RM_Coy_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "
            Dim tDS As DataSet = objDb.FillDs(strsql)

            If tDS.Tables(0).Rows.Count > 0 Then
                Return tDS.Tables(0).Rows(0).Item("RM_RFQ_NO").ToString.Trim
            End If
        End Function

        Public Function Get_RFQ_Name(ByVal rfq_id As String) As String
            Dim strsql As String = "select RM_RFQ_Name FROM RFQ_MSTR WHERE RM_RFQ_ID='" & Common.Parse(rfq_id) & "'"
            Dim tDS As DataSet = objDb.FillDs(strsql)

            If tDS.Tables(0).Rows.Count > 0 Then
                Get_RFQ_Name = tDS.Tables(0).Rows(0).Item("RM_RFQ_Name").ToString.Trim
            End If
        End Function

        Public Function deleteRFQAttachment(ByVal intIndex As Integer)
            Dim strsql As String
            strsql = "DELETE FROM COMPANY_DOC_ATTACHMENT "
            strsql &= "WHERE CDA_ATTACH_INDEX = " & intIndex
            objDb.Execute(strsql)
        End Function

        Public Function getRFQTempAttach(ByVal strDocNo As String, ByVal bcomid As String) As DataSet
            Dim strsql As String
            Dim ds As New DataSet
            strsql = "SELECT * FROM COMPANY_DOC_ATTACHMENT "
            strsql &= "WHERE CDA_COY_ID = '" & bcomid & "' "
            strsql &= "AND CDA_DOC_NO = '" & Common.Parse(strDocNo) & "' AND CDA_DOC_TYPE = 'RFQ' "
            ds = objDb.FillDs(strsql)
            getRFQTempAttach = ds
        End Function

        Public Function getRFQ_quoteTempAttach(ByVal strDocNo As String, ByVal vcomid As String) As DataSet
            Dim strsql As String
            Dim ds As New DataSet
            strsql = "SELECT * FROM COMPANY_DOC_ATTACHMENT "
            strsql &= "WHERE CDA_COY_ID = '" & vcomid & "' "
            strsql &= "AND CDA_DOC_NO = '" & Common.Parse(strDocNo) & "' AND CDA_DOC_TYPE = 'Quotation' "
            ds = objDb.FillDs(strsql)
            getRFQ_quoteTempAttach = ds
        End Function

        Public Function getRFQTempAttach2(ByVal strDocNo As String, Optional ByVal type As String = "QuotTemp") As DataSet
            Dim strsql As String
            Dim ds As New DataSet
            strsql = "SELECT * FROM COMPANY_DOC_ATTACHMENT "
            strsql &= "WHERE CDA_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "

            ' ai chu modified on 26/10/2005
            ' before user click submit, DOC_TYPE is saved as 'QuotTemp'
            'strsql &= "AND CDA_DOC_NO = '" & Common.Parse(strDocNo) & "' AND CDA_DOC_TYPE = 'Quotation'  "
            strsql &= "AND CDA_DOC_NO = '" & Common.Parse(strDocNo) & "'"
            ds = objDb.FillDs(strsql)
            getRFQTempAttach2 = ds
        End Function
        'Michelle (28/12/2010) - To delete those temp attachment 
        Public Function delRFQTempAttach2(ByVal strDocNo As String)
            Dim strsql As String
            strsql = "DELETE FROM COMPANY_DOC_ATTACHMENT "
            strsql &= "WHERE CDA_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "
            strsql &= "AND CDA_DOC_NO = '" & Common.Parse(strDocNo) & "' AND CDA_DOC_TYPE = 'QuotTemp' "
            objDb.Execute(strsql)
        End Function

        ' ai chu add on 26/10/2005
        ' if user click from summary screen
        ' need to delete all temp attachment for selected RFQ and 
        ' copy 1 set of attachment and set its type to QuotTemp
        Public Sub reCopyRFQTempAttach(ByVal strDocNo As String)
            Dim strsql As String
            Dim strAryQuery(0) As String
            ' delete first then insert 
            strsql = "DELETE FROM COMPANY_DOC_ATTACHMENT "
            strsql &= "WHERE CDA_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "
            strsql &= "AND CDA_DOC_NO = '" & Common.Parse(strDocNo) & "' AND CDA_DOC_TYPE = 'QuotTemp' "
            Common.Insert2Ary(strAryQuery, strsql)

            strsql = "INSERT INTO COMPANY_DOC_ATTACHMENT (CDA_COY_ID,CDA_DOC_NO,CDA_DOC_TYPE,CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE)"
            strsql &= "SELECT CDA_COY_ID,CDA_DOC_NO,'QuotTemp',CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE "
            strsql &= "FROM COMPANY_DOC_ATTACHMENT "
            strsql &= "WHERE CDA_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "
            strsql &= "AND CDA_DOC_NO = '" & Common.Parse(strDocNo) & "' AND CDA_DOC_TYPE = 'Quotation' "
            Common.Insert2Ary(strAryQuery, strsql)

            objDb.BatchExecute(strAryQuery)
        End Sub

        Function HasAttachment2(ByVal rfq_no As String, ByVal strCoyID As String, Optional ByVal file_type As String = "") As Boolean
            Dim strSql As String

            If file_type = "E" Then
                strSql = " AND CDA_TYPE = 'E'"
            Else
                strSql = ""
            End If

            strSql = "SELECT '*' FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & "' AND CDA_DOC_NO='" & rfq_no & "' AND CDA_DOC_TYPE='RFQ'" & strSql
            If objDb.Exist(strSql) >= 1 Then
                Return True
            Else
                Return False
            End If
        End Function

        Function HasAttachment(ByVal rfq_no As String, Optional ByVal file_type As String = "") As Boolean
            Dim strSql, strCoyID As String

            If file_type = "E" Then
                strSql = " AND CDA_TYPE = 'E'"
            Else
                strSql = ""
            End If

            strCoyID = HttpContext.Current.Session("CompanyId")
            strSql = "SELECT '*' FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & "' AND CDA_DOC_NO='" & rfq_no & "' AND CDA_DOC_TYPE='RFQ'" & strSql
            If objDb.Exist(strSql) >= 1 Then
                Return True
            Else
                Return False
            End If
        End Function

        Function HasAttachmentQuote(ByVal rfq_no As String, ByVal strCoyID As String) As Boolean
            Dim strSql As String
            strSql = "SELECT '*' FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & "' AND CDA_DOC_NO='" & rfq_no & "' AND CDA_DOC_TYPE='Quotation'"
            If objDb.Exist(strSql) >= 1 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Function searchlist() As DataSet
            Dim StrSelRfq As String
            Dim dsRfq As DataSet
            StrSelRfq = "SELECT Distinct m_rfq.rfq_id,Rfq_name ,actual_RFQ_Number," & _
                        "Created_on,Valid_Till,Status,RFQ_OPTION " & _
                        "FROM M_RFQ,M_RFQ_Vendor " & _
                        "WHERE M_RFQ.RFQ_ID = M_RFQ_Vendor.RFQ_ID and " & _
                        "M_RFQ.Company_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'" & _
                        "AND M_RFQ.Created_By = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "'" & _
                        "And M_RFQ.B_Display_Status = '0'" & _
                        "and M_RFQ.Status <> '3'"

            dsRfq = objDb.FillDs(StrSelRfq)
            searchlist = dsRfq
        End Function

        Public Function delete_trash(ByVal ds As DataSet)
            Dim i As Integer
            Dim strsql As String
            Dim STRARRAY(0) As String

            For i = 0 To ds.Tables(0).Rows.Count - 1

                strsql = "UPDATE RFQ_MSTR  SET RM_B_Display_Status='" & Common.Parse(ds.Tables(0).Rows(i)("B_NEW_STATUS")) & "' WHERE " & _
                          " RM_Coy_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND " & _
                          " RM_Created_By='" & Common.Parse(HttpContext.Current.Session("UserId")) & "' AND RM_B_Display_Status='" & Common.Parse(ds.Tables(0).Rows(i)("B_CURR_STATUS")) & " '"
                If ds.Tables(0).Rows(i)("rfq_no") <> "" Then
                    strsql = strsql & " and RM_RFQ_NO='" & Common.Parse(ds.Tables(0).Rows(i)("rfq_no")) & "'"
                    Common.Insert2Ary(STRARRAY, strsql)
                End If

                strsql = " INSERT INTO PR_CONVERTED_DOC (PCD_PR_NO, PCD_COY_ID, PCD_PR_LINE_INDEX, PCD_RFQ_LINE, PCD_CONVERT_TO_DATE, PCD_CONVERT_TO_DOC, PCD_CONVERT_BY_ID, PCD_DELETED_DATE, PCD_DELETED_BY_ID) " & _
                        " SELECT PRD_PR_NO, PRD_COY_ID, " & _
                        " (SELECT RD_PR_LINE_INDEX FROM RFQ_DETAIL WHERE RD_RFQ_id = " & _
                        " (SELECT RM_RFQ_ID FROM RFQ_MSTR WHERE RM_RFQ_NO = '" & Common.Parse(ds.Tables(0).Rows(i)("rfq_no")) & "' AND RM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "') " & _
                        " AND RD_PR_LINE_INDEX = PR_DETAILS.PRD_PR_LINE_INDEX) AS RD_PR_LINE_INDEX, " & _
                        " (SELECT RD_RFQ_LINE FROM RFQ_DETAIL WHERE RD_RFQ_id = " & _
                        " (SELECT RM_RFQ_ID FROM RFQ_MSTR WHERE RM_RFQ_NO = '" & Common.Parse(ds.Tables(0).Rows(i)("rfq_no")) & "' AND RM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "') " & _
                        " AND RD_PR_LINE_INDEX = PR_DETAILS.PRD_PR_LINE_INDEX) AS RD_RFQ_LINE, " & _
                        " PRD_CONVERT_TO_DATE, PRD_CONVERT_TO_DOC, PRD_CONVERT_BY_ID, " & Common.ConvertDate(Now.Today) & ", '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' " & _
                        " FROM PR_DETAILS " & _
                        " WHERE PRD_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND PRD_CONVERT_TO_DOC = '" & Common.Parse(ds.Tables(0).Rows(i)("rfq_no")) & "' "

                If ds.Tables(0).Rows(i)("rfq_no") <> "" Then
                    Common.Insert2Ary(STRARRAY, strsql)
                End If

                strsql = " UPDATE PR_DETAILS, PR_MSTR " & _
                        " SET PR_MSTR.PRM_PR_STATUS = " & PRStatus.Approved & "" & _
                        " WHERE PR_DETAILS.PRD_COY_ID = PR_MSTR.PRM_COY_ID AND PR_DETAILS.PRD_PR_NO = PR_MSTR.PRM_PR_NO AND PR_DETAILS.PRD_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " & _
                        " AND PR_DETAILS.PRD_CONVERT_TO_DOC = '" & Common.Parse(ds.Tables(0).Rows(i)("rfq_no")) & "'"
                Common.Insert2Ary(STRARRAY, strsql)

                strsql = " UPDATE PR_DETAILS " & _
                        " SET PRD_CONVERT_TO_IND = NULL, PRD_CONVERT_TO_DATE = NULL, PRD_CONVERT_TO_DOC = NULL, PRD_CONVERT_BY_ID = NULL " & _
                        " WHERE PRD_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " & _
                        " AND PRD_CONVERT_TO_DOC = '" & Common.Parse(ds.Tables(0).Rows(i)("rfq_no")) & "'"
                Common.Insert2Ary(STRARRAY, strsql)

                
            Next

            objDb.BatchExecute(STRARRAY)
        End Function

        ' ai chu modified on 05/05/2006
        ' need to retrieve data by RFQ_ID, not RFQ_name coz name is not unique
        'Public Function rfq_cath(ByVal rfq_name As String) As DataSet
        Public Function rfq_cath(ByVal rfq_id As String) As DataSet
            Dim strsql As String
            Dim dscath As New DataSet

            strsql = " Select IsNull(RD_Product_Desc,'') as Product_Desc, RD_UOM, RD_Quantity, IsNull(RD_Delivery_Lead_Time,'') as Delivery_Lead_Time, RD_Warranty_Terms " & _
                     " From RFQ_DETAIL,RFQ_MSTR " & _
                     " Where RD_RFQ_ID=RM_RFQ_ID AND RM_Created_By='" & Common.Parse(HttpContext.Current.Session("UserId")) & "' And RM_RFQ_ID = '" & Common.Parse(rfq_id) & "' and RM_Coy_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "

            dscath = objDb.FillDs(strsql)
            rfq_cath = dscath
        End Function

        Public Function get_codemstr(ByVal abbr As String, ByVal code_cat As String) As String
            Dim strsql As String
            Dim count As Integer

            strsql = "select CODE_DESC from CODE_MSTR where CODE_ABBR ='" & abbr & "' and CODE_CATEGORY='" & code_cat & "'"
            Dim tDS As DataSet = objDb.FillDs(strsql)
            If tDS.Tables(0).Rows.Count > 0 Then
                get_codemstr = tDS.Tables(0).Rows(0).Item("CODE_DESC")
            End If
        End Function
        Public Function dg_view(ByVal RFQ_name As String, ByVal RFQ_No As String) As DataSet
            Dim strSQLUpd As String
            Dim dscath As New DataSet
            strSQLUpd = " select RFQ_DETAIL.* from RFQ_DETAIL,RFQ_MSTR " &
                        " where RM_Created_By ='" & Common.Parse(HttpContext.Current.Session("UserId")) & "' and RM_RFQ_Name = '" & Common.Parse(RFQ_name) & "'" &
                        " AND RM_Coy_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' and RD_RFQ_ID=RM_RFQ_ID"

            If RFQ_No <> "" Then
                strSQLUpd = strSQLUpd & " AND RM_RFQ_No='" & RFQ_No & "'"
            End If
            dscath = objDb.FillDs(strSQLUpd)
            dg_view = dscath
        End Function

        'Michelle (2/12/2010) - replacing dg_view
        Public Function get_items(ByVal rfq_id As String, Optional ByVal PRMode As String = "") As DataSet
            Dim ds As DataSet
            Dim strsql As String
            If rfq_id <> "" Then
                'Michelle (16/9/2011) - To include in the Warranty Terms (871)
                strsql = "select RD_COY_ID, RD_PRODUCT_CODE, RD_QUANTITY, RD_PRODUCT_DESC, 'A' as Type, " & _
                            "RD_UOM, RD_DELIVERY_LEAD_TIME, RD_COY_ID, RD_VENDOR_ITEM_CODE, RD_WARRANTY_TERMS, RD_RFQ_LINE from RFQ_DETAIL " & _
                            "where RD_RFQ_ID ='" & Common.Parse(rfq_id) & "' " & _
                            "and RD_PRODUCT_DESC not in (select RD_PRODUCT_DESC from RFQ_DETAIL_TEMP " & _
                            "where RD_USER_ID = '" & HttpContext.Current.Session("UserID") & "' ) "
                If PRMode = "pr" Then
                    strsql &= " union all "
                Else
                    strsql &= " union "
                End If
            End If
            strsql &= "select RD_COY_ID, RD_PRODUCT_CODE, RD_QUANTITY, RD_PRODUCT_DESC, 'T' as Type, " & _
                            "RD_UOM, RD_DELIVERY_LEAD_TIME, RD_COY_ID, RD_VENDOR_ITEM_CODE, 0 as RD_WARRANTY_TERMS, 0 AS RD_RFQ_LINE from RFQ_DETAIL_TEMP " & _
                            "where RD_USER_ID = '" & HttpContext.Current.Session("UserID") & "' AND RD_ACTION='A' " & _
                            "and RD_PRODUCT_DESC not in (select RD_PRODUCT_DESC from RFQ_DETAIL_TEMP " & _
                            "where RD_USER_ID = '" & HttpContext.Current.Session("UserID") & "' AND RD_ACTION='D') " & _
                            "group by RD_PRODUCT_DESC "

            ds = objDb.FillDs(strsql)
            get_items = ds
        End Function

        Public Function Delete_item(ByVal strDelType As String, ByVal strDelStr As String, Optional ByVal strPrdCode As String = "", Optional ByVal strPrdCodeDesc As String = "")
            Dim strsql As String

            If strDelType = "temp" Then 'ie. remove the line item before the RFQ no. is created
                'strsql = "insert into RFQ_DETAIL_TEMP " & _
                '         "(RD_USER_ID, RD_Product_Desc, RD_Action) VALUES " & _
                '        "( '" & HttpContext.Current.Session("UserID") & "', '" & Common.Parse(strDelStr) & "','D')"
                strsql = "delete from RFQ_DETAIL_TEMP " & _
                        "WHERE RD_USER_ID = '" & HttpContext.Current.Session("UserID") & "' AND RD_PRODUCT_DESC = '" & Common.Parse(strDelStr) & "'"
            Else
                If strPrdCode = " " Then
                    strsql = "Delete from RFQ_DETAIL where RD_RFQ_ID = '" & Common.Parse(strDelStr) & "' and RD_PRODUCT_DESC = '" & Common.Parse(strPrdCodeDesc) & "' "
                Else
                    strsql = "Delete from RFQ_DETAIL where RD_RFQ_ID = '" & Common.Parse(strDelStr) & "' and RD_PRODUCT_CODE = '" & Common.Parse(strPrdCode) & "' "
                End If
            End If

            objDb.Execute(strsql)
        End Function

        Public Function update_RFQMSTR(ByVal item As RFQ_User, ByVal pre_rfqname As String) As String
            Dim strSQLUpd As String

            Dim strid As String = "Select RM_RFQ_ID,rm_rfq_name From RFQ_MSTR " & _
            " Where RM_RFQ_Name = '" & Common.Parse(item.RFQ_Name) & "' " & _
            "AND RM_Created_By = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " & _
            "AND RM_Coy_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " & _
            "AND RM_B_DISPLAY_STATUS = '0'"

            Dim tDS As DataSet = objDb.FillDs(strid)
            If tDS.Tables(0).Rows.Count > 0 Then
                If tDS.Tables(0).Rows(0).Item("rm_rfq_name") = pre_rfqname Then
                Else
                    update_RFQMSTR = "1" 'rfq_name exist
                    Exit Function
                End If
            End If

            strSQLUpd = " UPDATE RFQ_MSTR SET RM_RFQ_Name ='" & Common.Parse(item.RFQ_Name) & "',RM_Currency_Code='" & Common.Parse(item.cur_code) & "' WHERE " & _
                            " RM_Created_By ='" & Common.Parse(HttpContext.Current.Session("UserId")) & "' and RM_RFQ_ID ='" & Common.Parse(item.index) & "'"

            Return strSQLUpd
        End Function

        Public Function update_item(ByVal item As RFQ_User) As String
            Dim strSQLUpd2 As String
            strSQLUpd2 = "UPDATE RFQ_DETAIL set RD_Product_Desc='" & Common.Parse(item.productdesc) & "',RD_UOM= '" & Common.Parse(item.uom) & "'," & _
                                          " RD_Quantity = '" & Common.Parse(item.Quantity) & "'," & _
                                          " RD_Delivery_Lead_Time ='" & Common.Parse(item.Delivery_Lead_Time) & "'," & _
                                          " RD_Warranty_Terms = '" & Common.Parse(item.Warranty_Terms) & "' " & _
                                          " WHERE  RD_RFQ_ID ='" & Common.Parse(item.index) & "' and RD_RFQ_Line='" & Common.Parse(item.lineno) & "'"

            update_item = strSQLUpd2
        End Function

        'ai chu remark on 11/01/2006
        'this function is moved to under save_rfq_mstr function
        'Public Function add_RFQ(ByVal item As RFQ_User)
        '    Dim strsql As String = "insert into RFQ_DETAIL (RD_RFQ_ID,RD_Coy_ID,RD_RFQ_Line,RD_Product_Code," & _
        '                  "RD_Vendor_Item_Code,RD_Quantity,RD_Product_Desc," & _
        '                  "RD_UOM,RD_Delivery_Lead_Time,RD_Warranty_Terms," & _
        '                  "RD_Product_Name) values " & _
        '                  "('" & Common.Parse(item.RFQ_ID) & "','" & Common.Parse(item.V_com_ID) & "','" & Common.Parse(item.lineno) & "','" & Common.Parse(item.product_ID) & "'," & _
        '                  "'" & Common.Parse(item.v_itemCode) & "','" & Common.Parse(item.Quantity) & "','" & Common.Parse(item.productdesc) & "' ," & _
        '                  "'" & Common.Parse(item.uom) & "','" & Common.Parse(item.Delivery_Lead_Time) & "','" & Common.Parse(item.Warranty_Terms) & "', " & _
        '                  "'" & Common.Parse(item.product_name) & "')"

        '    ' ai chu add on 09/01/2006
        '    ' to trace duplication of primary key for RFQ_DETAIL
        '    Dim ctx As HttpContext = HttpContext.Current
        '    Dim objStreamWriter As StreamWriter
        '    Try
        '        objStreamWriter = New StreamWriter(ctx.Server.MapPath(ctx.Request.ApplicationPath & "\" & Format(Now.Date, "yyyyMMdd") & "_RFQDetail.log"), True)
        '        objStreamWriter.WriteLine(strsql)
        '    Catch Meexp As Exception
        '    Finally
        '        objStreamWriter.Close()
        '        objStreamWriter = Nothing
        '    End Try

        '    objDb.Execute(strsql)
        'End Function

        'Public Function add_RFQ2ll(ByVal value() As String)
        '    Dim str As String
        '    Dim i As Integer
        '    Dim index As String
        '    ' Dim read As OleDb.OleDbDataReader
        '    ' Dim objread As New RFQ_User()

        '    If objDb.Exist("Select '*' From RFQ_CART_ITEM_TEMP Where RCIT_Product_Desc='' and  RCIT_User_ID='" & Common.Parse(HttpContext.Current.Session("UserId")) & "' And RCIT_RFQ_Name = '" & Common.Parse(value(5)) & "'AND RCIT_Header_Ind='1' ") Then
        '        Exit Function
        '    End If

        '    Dim strsql As String = ("insert into RFQ_CART_ITEM_TEMP (RCIT_User_ID,RCIT_Product_Name," & _
        '                            "RCIT_Product_ID, RCIT_Product_Desc, RCIT_Quantity, RCIT_Delivery_Lead_Time," & _
        '                              "RCIT_Warranty_Terms,RCIT_UOM,RCIT_RFQ_Name,RCIT_Currency_Code,RCIT_Header_Ind,RCIT_Line_no) values ('" & Common.Parse(HttpContext.Current.Session("UserId")) & "','','99999', " & _
        '                                  " '" & Common.Parse(value(0)) & "','" & Common.Parse(value(2)) & "','" & Common.Parse(value(3)) & " ', " & _
        '                                          " '" & Common.Parse(value(4)) & "','" & Common.Parse(value(1)) & "','" & Common.Parse(value(5)) & "','" & Common.Parse(value(6)) & "','" & Common.Parse(value(7)) & "','" & Common.Parse(value(8)) & "'")
        '    objDb.Execute(strsql)

        'End Function

        Public Function count_item(ByVal rfq_name As String, ByVal RFQ_NO As String) As Integer
            Dim strsql As String
            Dim count As String

            strsql = " Select count('*') as count From RFQ_DETAIL,RFQ_MSTR Where " & _
            " RD_RFQ_ID = RM_RFQ_ID AND RM_Created_By='" & Common.Parse(HttpContext.Current.Session("UserId")) & "' And RM_RFQ_Name= '" & Common.Parse(rfq_name) & "'" & _
            " and RM_Coy_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "

            If RFQ_NO <> "" Then
                strsql = strsql & " AND RM_RFQ_No='" & RFQ_NO & "'"
            End If
            Dim tDS As DataSet = objDb.FillDs(strsql)
            If tDS.Tables(0).Rows.Count > 0 Then
                count = tDS.Tables(0).Rows(0).Item("count").ToString.Trim
                If count = "" Then
                    count = "0"
                End If
            End If
            count_item = count
        End Function

        Public Function count_item2(ByVal rfq_name As String) As Integer
            Dim strsql As String
            Dim count As String

            strsql = " Select MAX(RD_RFQ_Line) as count From RFQ_DETAIL,RFQ_MSTR Where  RD_RFQ_ID = RM_RFQ_ID AND RM_Created_By='" & Common.Parse(HttpContext.Current.Session("UserId")) & "' And RM_RFQ_Name= '" & Common.Parse(rfq_name) & "' and RM_Coy_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "
            Dim tDS As DataSet = objDb.FillDs(strsql)
            If tDS.Tables(0).Rows.Count > 0 Then
                count = tDS.Tables(0).Rows(0).Item("count").ToString.Trim
                If count = "" Then
                    count = "0"
                End If
            End If
            count_item2 = count
        End Function
        'Michelle (3/12/2010) - To store into the temp table
        Public Function add_RFQCat_TEMP(ByVal ds As DataSet) As String
            Dim i As Integer
            Dim strAryQuery(0) As String

            For i = 0 To ds.Tables(0).Rows.Count - 1
                Dim strsql As String = "insert into RFQ_DETAIL_TEMP " & _
                "(RD_USER_ID, RD_Coy_ID, RD_Product_Code," & _
                "RD_Quantity,RD_Product_Desc," & _
                "RD_UOM,RD_Delivery_Lead_Time, RD_Action) VALUES " & _
                "( '" & HttpContext.Current.Session("UserID") & "', '" & Common.Parse(ds.Tables(0).Rows(i)("vendor_Id")) & "','" & Common.Parse(ds.Tables(0).Rows(i)("product_ID")) & " ', " & _
                "'" & Common.Parse(ds.Tables(0).Rows(i)("Quantity")) & "','" & Common.Parse(ds.Tables(0).Rows(i)("item_desc")) & "'," & _
                "'" & Common.Parse(ds.Tables(0).Rows(i)("uom")) & "','" & Common.Parse(ds.Tables(0).Rows(i)("Delivery_Lead_Time")) & "'," & _
                "'A')"
                Common.Insert2Ary(strAryQuery, strsql)
            Next

            objDb.BatchExecute(strAryQuery)
        End Function
        'Michelle (5/12/2010) - To store the infor raise from the Vendor Item Search to the temp table
        Public Function add_RFQCatSearch_TEMP(ByVal strProdcode As String) As String
            Dim i As Integer
            Dim strAryQuery(0) As String

            Dim strsql As String = "insert into RFQ_DETAIL_TEMP " & _
                "(RD_USER_ID, RD_Coy_ID, RD_Product_Code," & _
                "RD_Quantity,RD_Product_Desc," & _
                "RD_UOM,RD_Delivery_Lead_Time, RD_Action) SELECT " & _
                "'" & HttpContext.Current.Session("UserID") & "', PM_S_COY_ID, PM_PRODUCT_CODE, " & _
                "0, PM_PRODUCT_DESC, PM_UOM, 0, 'A' FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE IN (" & _
                strProdcode & ")"

            objDb.Execute(strsql)
        End Function

        Public Function get_index(ByVal rfq_name As String) As String
            Dim strsql As String
            Dim index As String = ""

            strsql = " Select RM_RFQ_ID From RFQ_MSTR Where RM_Created_By='" & HttpContext.Current.Session("UserId") & "' And RM_RFQ_Name = '" & Common.Parse(rfq_name) & "' " & _
            " AND RM_Coy_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND RM_B_DISPLAY_STATUS = '0' "

            index = objDb.GetVal(strsql)
            get_index = index
        End Function

        Public Function chk_RfqOption2(ByVal rfq_id As String) As String
            Dim strsql As String = "select RM_RFQ_OPTION FROM RFQ_MSTR WHERE RM_RFQ_ID = '" & rfq_id & "'"
            Dim tDS As DataSet = objDb.FillDs(strsql)
            If tDS.Tables(0).Rows.Count > 0 Then
                chk_RfqOption2 = tDS.Tables(0).Rows(0).Item(Trim("RM_RFQ_OPTION"))
            End If

        End Function

        Public Function chk_RfqOption() As String
            Dim strsql As String = "select CM_RFQ_OPTION FROM COMPANY_MSTR WHERE CM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'"
            Dim tDS As DataSet = objDb.FillDs(strsql)
            If tDS.Tables(0).Rows.Count > 0 Then
                chk_RfqOption = tDS.Tables(0).Rows(0).Item(Trim("CM_RFQ_OPTION"))
            End If

        End Function

        Public Function check_venlist(ByVal RFQ As String) As Integer
            If objDb.Exist("select '*' from RFQ_INVITED_VENLIST where RIV_RFQ_ID in(select RM_RFQ_ID from RFQ_MSTR WHERE RM_Created_By='" & Common.Parse(HttpContext.Current.Session("UserId")) & " ' and RM_RFQ_Name='" & Common.Parse(RFQ) & "' AND RM_Coy_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & " ')") > 0 Then
                check_venlist = 1
            Else
                check_venlist = 0
            End If
        End Function

        Public Function read_user(ByVal RFQ_user As RFQ_User, ByVal rfq_name As String)
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

        Public Function check_venlistvenlist(ByVal rfq_id As String) As Boolean

            Dim strsql As String = "select count(*) as check from RFQ_INVITED_VENLIST RIV_RFQ_ID='" & Common.Parse(rfq_id) & "'"

            Dim tDS As DataSet = objDb.FillDs(strsql)
            If tDS.Tables(0).Rows.Count > 0 Then
                If tDS.Tables(0).Rows(0).Item("check").ToString.Trim > 0 Then
                    check_venlistvenlist = 1
                End If
            End If

        End Function

        Public Function read_rfqMstr(ByVal objread As RFQ_User, Optional ByVal rfq_name As String = "", Optional ByVal rfq_id As String = "", Optional ByVal rfq_num As String = "") As String

            Dim sqlstr2, strdelname As String
            Dim tDS As DataSet
            sqlstr2 = "select * from RFQ_MSTR WHERE 1=1"
            'Michelle (20/8/2013) - Issue 2238 
            'If rfq_name <> "" Then
            '    sqlstr2 = sqlstr2 & " and RM_RFQ_Name ='" & Common.Parse(rfq_name) & "'"
            'End If
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

                    If IsDBNull(tDS.Tables(0).Rows(0).Item("RM_DEL_CODE").ToString.Trim) Then
                        objread.del_code = ""
                    Else
                        Dim strDelCode, strCoyId As String
                        strDelCode = tDS.Tables(0).Rows(0).Item("RM_DEL_CODE").ToString.Trim
                        strCoyId = tDS.Tables(0).Rows(0).Item("RM_COY_ID").ToString.Trim
                        strdelname = objDb.GetVal("SELECT CDT_DEL_NAME FROM COMPANY_DELIVERY_TERM WHERE CDT_DEL_CODE = '" & Common.Parse(strDelCode) & "' AND CDT_COY_ID = '" & strCoyId & "'")

                        If strdelname <> "" Then
                            objread.del_code = strDelCode & " (" & strdelname & ")"
                        Else
                            objread.del_code = strDelCode
                        End If
                    End If
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

        Public Function chkPurchasingRole() As Boolean
            Dim PURCHASING_ROLE As Boolean = False
            Dim objUser As New Users
            PURCHASING_ROLE = objUser.IsPurchasing()
            objUser = Nothing
            chkPurchasingRole = PURCHASING_ROLE
        End Function

        Public Function Vendor_count() As Integer

            Dim strvdr As String = "select count(distinct substring(CV_S_COY_ID,1,1)) as count"
            Dim str As String = getAvailableVendor(chkPurchasingRole)
            strvdr = strvdr & str
            Dim tDS As DataSet = objDb.FillDs(strvdr)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                Vendor_count = tDS.Tables(0).Rows(j).Item("count").ToString.Trim
            Next
        End Function

        Public Function delete_venList(ByVal item As RFQ_User)
            Dim strAryQuery(0) As String
            If item.type = "list" Then
                Dim strsql As String = "delete from RFQ_INVITED_VENLIST_MSTR where RIVMT_User_ID='" & Common.Parse(HttpContext.Current.Session("UserID")) & "'" & _
                            " and RIVMT_RFQ_Name='" & Common.Parse(item.RFQ_Name) & "' and RIVMT_Distribution_list_id='" & Common.Parse(item.dis_ID) & "' " & _
                            " and RIVMT_RFQ_ID = " & item.RFQ_ID
                Dim strsql2 As String = "delete from RFQ_INVITED_VENLIST_DETAIL where RTVDT_User_Id='" & Common.Parse(HttpContext.Current.Session("UserID")) & "'" & _
                " and RTVDT_RFQ_Name= '" & Common.Parse(item.RFQ_Name) & "' and RTVDT_Distribution_List_Id='" & Common.Parse(item.dis_ID) & "' and RTVDT_RFQ_ID = '" & item.RFQ_ID & "' "
                Dim strsql3 As String = "delete from RFQ_INVITED_VENLIST where  RIV_RFQ_ID in(select RM_RFQ_ID from RFQ_MSTR WHERE RM_Coy_ID= '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'" & _
                " and  RM_RFQ_Name = '" & Common.Parse(item.RFQ_Name) & "' AND RM_Created_By='" & Common.Parse(HttpContext.Current.Session("UserID")) & "')"
                If item.V_com_ID <> "" Then
                    strsql3 = strsql3 & " and (RIV_S_Coy_ID not in(" & item.V_com_ID & ") "
                End If
                If item.list_index <> "" Then
                    strsql3 = strsql3 & "  and RIV_S_Coy_ID not in(select RCDLD_V_Coy_ID from RFQ_VEN_DISTR_LIST_DETAIL where RVDLD_LIST_INDEX in(" & item.list_index & "))"
                End If
                If item.V_com_ID <> "" Then
                    strsql3 = strsql3 & ")"
                End If
                Common.Insert2Ary(strAryQuery, strsql)
                Common.Insert2Ary(strAryQuery, strsql2)
                Common.Insert2Ary(strAryQuery, strsql3)
            ElseIf item.type = "specific" Then
                Dim strsql3 As String = "delete from RFQ_INVITED_VENLIST where  RIV_RFQ_ID in(select RM_RFQ_ID from RFQ_MSTR WHERE RM_Coy_ID= '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'" & _
                " and  RM_RFQ_Name = '" & Common.Parse(item.RFQ_Name) & "' AND RM_Created_By='" & Common.Parse(HttpContext.Current.Session("UserID")) & "')" & _
                " and RIV_S_Coy_ID='" & item.dis_ID & "'"
                Common.Insert2Ary(strAryQuery, strsql3)
            End If

            objDb.BatchExecute(strAryQuery)
        End Function

        'Public Function get_VenList(ByVal rfq_name As String, ByVal rfq_id As String) As DataSet
        Public Function get_VenList(ByVal rfq_id As String) As DataSet
            Dim ds As DataSet
            Dim strsql As String

            'strsql = "select DISM.RVDLM_List_Name,cast(DISM.RVDLM_List_Index as char) as RVDLM_List_Index ,'list' as type from RFQ_VEN_DISTR_LIST_MSTR DISM, " & _
            '" RFQ_INVITED_VENLIST_MSTR DISMT WHERE " & _
            '" DISMT.RIVMT_User_ID='" & Common.Parse(HttpContext.Current.Session("UserId")) & "' and " & _
            '" DISMT.RIVMT_RFQ_Name='" & Common.Parse(rfq_name) & "' and DISMT.RIVMT_Distribution_list_id = DISM.RVDLM_List_Index " & _
            '" AND DISMT.RIVMT_RFQ_ID = " & rfq_id & _
            '" union all " & _
            '" select RIV_S_Coy_Name,RIV_S_Coy_ID,'specific' as type from RFQ_INVITED_VENLIST" & _
            '" where RIV_RFQ_ID in (select RM_RFQ_ID from RFQ_MSTR WHERE RM_Coy_ID= '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'" & _
            '" and  RM_RFQ_Name = '" & Common.Parse(rfq_name) & "' AND RM_Created_By='" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND RM_B_DISPLAY_STATUS = '0')" & _
            '" and RIV_S_Coy_ID not in(select  RTVDT_v_company_id  from " & _
            '" RFQ_VEN_DISTR_LIST_MSTR DISM,  RFQ_INVITED_VENLIST_MSTR DISMT ,RFQ_INVITED_VENLIST_DETAIL " & _
            '" WHERE DISMT.RIVMT_User_ID='" & Common.Parse(HttpContext.Current.Session("UserId")) & "' and " & _
            '" DISMT.RIVMT_RFQ_Name='" & Common.Parse(rfq_name) & "' and DISMT.RIVMT_RFQ_ID = " & rfq_id & _
            '" and DISMT.RIVMT_RFQ_ID = RTVDT_RFQ_ID " & _
            '" and DISMT.RIVMT_Distribution_list_id = DISM.RVDLM_List_Index " & _
            '" AND RTVDT_Distribution_List_Id =DISM.RVDLM_List_Index)" '  AND DISMT.RIVMT_RFQ_Name = RTVDT_RFQ_Name

            strsql = " select RIV_S_Coy_Name,RIV_S_Coy_ID,'specific' as type from RFQ_INVITED_VENLIST" & _
                    " where RIV_RFQ_ID = '" & Common.Parse(rfq_id) & "'"
            'Exclude those vendors that are deleted from the screen but not save to database yet
            strsql &= " and RIV_S_Coy_ID not in (select RIV_S_Coy_ID from RFQ_INVITED_VENLIST_TEMP" & _
                    " where RIV_USER_ID = '" & HttpContext.Current.Session("UserID") & "' AND RIV_ACTION='D') "
            'Include those vendors that are selected from the vendor list but not save in the RFQ_INVITED_VENLIST yet
            strsql &= " UNION " & _
                    " select RIV_S_Coy_Name, RIV_S_Coy_ID, 'specific' as type from RFQ_INVITED_VENLIST_TEMP" & _
                    " where RIV_USER_ID = '" & HttpContext.Current.Session("UserID") & "' AND RIV_ACTION='A' " & _
                    " group by RIV_S_Coy_ID "

            '" and RIV_S_Coy_ID not in (select RIV_S_Coy_ID from RFQ_INVITED_VENLIST_TEMP"
            '" where RIV_USER_ID = '" & HttpContext.Current.Session("UserID") & "' AND RIV_ACTION='D') "

            ds = objDb.FillDs(strsql)
            get_VenList = ds
        End Function

        'Public Function get_VenList(ByVal rfq_name As String, ByVal rfq_id As String) As DataSet
        'Public Function get_RFQVenList(ByVal rfq_id As String, ByRef aryVendorList As ArrayList, ByRef aryVendor As ArrayList) As DataSet
        '    Dim ds As DataSet
        '    Dim strsql As String
        '    Dim i As Integer

        '    strsql = "select DISM.RVDLM_List_Name,cast(DISM.RVDLM_List_Index as char) as RVDLM_List_Index ,'list' as TYPE,'N' AS Added from RFQ_VEN_DISTR_LIST_MSTR DISM, " & _
        '    " RFQ_INVITED_VENLIST_MSTR DISMT WHERE " & _
        '    " DISMT.RIVMT_User_ID='" & Common.Parse(HttpContext.Current.Session("UserId")) & "' and " & _
        '    " DISMT.RIVMT_Distribution_list_id = DISM.RVDLM_List_Index " & _
        '    " AND DISMT.RIVMT_RFQ_ID = '" & Common.Parse(rfq_id) & "' " & _
        '    " union all " & _
        '    " select RIV_S_Coy_Name,RIV_S_Coy_ID,'specific' as TYPE,'N' AS Added from RFQ_INVITED_VENLIST" & _
        '    " where RIV_RFQ_ID='" & Common.Parse(rfq_id) & "' AND RIV_RFQ_ID in (select RM_RFQ_ID from RFQ_MSTR WHERE RM_Coy_ID= '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'" & _
        '    " AND RM_Created_By='" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND RM_B_DISPLAY_STATUS = '0')" & _
        '    " and RIV_S_Coy_ID not in(select  RTVDT_v_company_id  from " & _
        '    " RFQ_VEN_DISTR_LIST_MSTR DISM,  RFQ_INVITED_VENLIST_MSTR DISMT ,RFQ_INVITED_VENLIST_DETAIL " & _
        '    " WHERE DISMT.RIVMT_User_ID='" & Common.Parse(HttpContext.Current.Session("UserId")) & "' and " & _
        '    " DISMT.RIVMT_RFQ_ID = '" & Common.Parse(rfq_id) & "' " & _
        '    " and DISMT.RIVMT_RFQ_ID = RTVDT_RFQ_ID " & _
        '    " and DISMT.RIVMT_Distribution_list_id = DISM.RVDLM_List_Index " & _
        '    " AND RTVDT_Distribution_List_Id =DISM.RVDLM_List_Index)" '  AND DISMT.RIVMT_RFQ_Name = RTVDT_RFQ_Name

        '    'strsql = " select RIV_S_Coy_Name,RIV_S_Coy_ID,'specific' as type from RFQ_INVITED_VENLIST" & _
        '    '        " where RIV_RFQ_ID = '" & Common.Parse(rfq_id) & "'"
        '    ''Exclude those vendors that are deleted from the screen but not save to database yet
        '    'strsql &= " and RIV_S_Coy_ID not in (select RIV_S_Coy_ID from RFQ_INVITED_VENLIST_TEMP" & _
        '    '        " where RIV_USER_ID = '" & HttpContext.Current.Session("UserID") & "' AND RIV_ACTION='D') "
        '    ''Include those vendors that are selected from the vendor list but not save in the RFQ_INVITED_VENLIST yet
        '    'strsql &= " UNION " & _
        '    '        " select RIV_S_Coy_Name, RIV_S_Coy_ID, 'specific' as type from RFQ_INVITED_VENLIST_TEMP" & _
        '    '        " where RIV_USER_ID = '" & HttpContext.Current.Session("UserID") & "' AND RIV_ACTION='A' " & _
        '    '        " group by RIV_S_Coy_ID "

        '    '" and RIV_S_Coy_ID not in (select RIV_S_Coy_ID from RFQ_INVITED_VENLIST_TEMP"
        '    '" where RIV_USER_ID = '" & HttpContext.Current.Session("UserID") & "' AND RIV_ACTION='D') "

        '    ds = objDb.FillDs(strsql)
        '    For i = 0 To ds.Tables(0).Rows.Count - 1
        '        If ds.Tables(0).Rows(i).Item("TYPE") = "list" Then
        '            aryVendorList.Add(ds.Tables(0).Rows(i).Item("RVDLM_List_Index"))

        '        ElseIf ds.Tables(0).Rows(i).Item("TYPE") = "specific" Then
        '            aryVendor.Add(ds.Tables(0).Rows(i).Item("RVDLM_List_Index"))
        '        End If
        '    Next

        '    get_RFQVenList = ds
        '    ds = Nothing
        'End Function

        Public Function get_RFQVenList(ByVal rfq_id As String) As DataSet
            Dim ds As DataSet
            Dim strsql As String
            Dim i As Integer

            strsql = "select DISM.RVDLM_List_Name,cast(DISM.RVDLM_List_Index as char) as RVDLM_List_Index ,'list' as TYPE,'N' AS Added from RFQ_VEN_DISTR_LIST_MSTR DISM, " & _
                    " RFQ_INVITED_VENLIST_MSTR DISMT WHERE " & _
                    " DISMT.RIVMT_User_ID='" & Common.Parse(HttpContext.Current.Session("UserId")) & "' and " & _
                    " DISMT.RIVMT_Distribution_list_id = DISM.RVDLM_List_Index " & _
                    " AND DISMT.RIVMT_RFQ_ID = '" & Common.Parse(rfq_id) & "' " & _
                    " union all " & _
                    " select RIV_S_Coy_Name,RIV_S_Coy_ID,'specific' as TYPE,'N' AS Added from RFQ_INVITED_VENLIST" & _
                    " where RIV_RFQ_ID='" & Common.Parse(rfq_id) & "' AND RIV_RFQ_ID in (select RM_RFQ_ID from RFQ_MSTR WHERE RM_Coy_ID= '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'" & _
                    " AND RM_Created_By='" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND RM_B_DISPLAY_STATUS = '0')" & _
                    " and RIV_S_Coy_ID not in(select  RTVDT_v_company_id  from " & _
                    " RFQ_VEN_DISTR_LIST_MSTR DISM,  RFQ_INVITED_VENLIST_MSTR DISMT ,RFQ_INVITED_VENLIST_DETAIL " & _
                    " WHERE DISMT.RIVMT_User_ID='" & Common.Parse(HttpContext.Current.Session("UserId")) & "' and " & _
                    " DISMT.RIVMT_RFQ_ID = '" & Common.Parse(rfq_id) & "' " & _
                    " and DISMT.RIVMT_RFQ_ID = RTVDT_RFQ_ID " & _
                    " and DISMT.RIVMT_Distribution_list_id = DISM.RVDLM_List_Index " & _
                    " AND RTVDT_Distribution_List_Id =DISM.RVDLM_List_Index)"

            ds = objDb.FillDs(strsql)

            get_RFQVenList = ds
            ds = Nothing
        End Function

        Public Function VendorAlreadyExist(ByVal dtOri As DataTable, ByVal dtNew As DataTable, ByVal intList As Integer) As Boolean
            Dim i As Integer
            Dim j As Integer
            Dim count As Integer

            VendorAlreadyExist = False
            For i = 0 To dtNew.Rows.Count - 1
                For j = 0 To dtOri.Rows.Count - 1
                    If dtOri.Rows(i).Item("RVDLM_List_Index") = intList And dtOri.Rows(i).Item("TYPE") = "list" Then
                        If dtOri.Rows(i).Item("CoyId") = dtNew.Rows(i).Item("RCDLD_V_Coy_ID") Then
                            count = count + 1
                            Exit For
                        End If
                    End If

                Next
            Next

            If dtNew.Rows.Count = count Then
                If dtNew.Rows.Count > 0 Then
                    VendorAlreadyExist = True
                End If
            End If
        End Function

        'Public Sub get_RFQVenListDetails(ByVal rfq_id As String, ByRef aryVendor As ArrayList)
        '    Dim ds As DataSet
        '    Dim strsql As String
        '    Dim i As Integer

        '    strsql = "SELECT RTVDT_v_company_id " _
        '        & "FROM RFQ_INVITED_VENLIST_DETAIL " _
        '        & "WHERE RTVDT_RFQ_ID= '" & Common.Parse(rfq_id) & "' " _
        '        & "AND RTVDT_User_Id='" & Common.Parse(HttpContext.Current.Session("UserID")) & "'"
        '    '& "AND RTVDT_Distribution_List_Id=47"

        '    ds = objDb.FillDs(strsql)
        '    For i = 0 To ds.Tables(0).Rows.Count - 1
        '        aryVendor.Add(ds.Tables(0).Rows(i).Item("RVDLM_List_Index"))
        '    Next
        '    ds = Nothing

        'End Sub

        Public Function get_RFQVenListDetails(ByVal rfq_id As String) As DataSet
            Dim ds As DataSet
            Dim strsql As String
            Dim i As Integer

            'strsql = "SELECT RTVDT_v_company_id " _
            '    & "FROM RFQ_INVITED_VENLIST_DETAIL " _
            '    & "WHERE RTVDT_RFQ_ID= '" & Common.Parse(rfq_id) & "' " _
            '    & "AND RTVDT_User_Id='" & Common.Parse(HttpContext.Current.Session("UserID")) & "'"
            ''& "AND RTVDT_Distribution_List_Id=47"
            strsql = "SELECT RTVDT_v_company_id AS CoyId,DISM.RVDLM_List_Name,CAST(DISM.RVDLM_List_Index AS CHAR) AS RVDLM_List_Index ,'list' AS TYPE,'N' AS Added " _
                   & "FROM RFQ_VEN_DISTR_LIST_MSTR DISM " _
                   & "INNER JOIN RFQ_INVITED_VENLIST_MSTR DISMT ON DISMT.RIVMT_Distribution_list_id = DISM.RVDLM_List_Index " _
                   & "INNER JOIN RFQ_INVITED_VENLIST_DETAIL ON RIVMT_RFQ_ID = RTVDT_RFQ_ID AND RTVDT_Distribution_List_Id=RIVMT_Distribution_list_id " _
                   & "And RTVDT_User_Id = RIVMT_User_ID " _
                   & "WHERE DISMT.RIVMT_User_ID='" & Common.Parse(HttpContext.Current.Session("UserId")) & "' AND DISMT.RIVMT_RFQ_ID = '" & Common.Parse(rfq_id) & "' " _
                   & "union all " _
                   & "SELECT RIV_S_Coy_ID AS CoyId,RIV_S_Coy_Name,RIV_S_Coy_ID,'specific' AS TYPE,'N' AS Added " _
                   & "FROM RFQ_INVITED_VENLIST WHERE RIV_RFQ_ID='" & Common.Parse(rfq_id) & "' " _
                   & "AND RIV_RFQ_ID IN (SELECT RM_RFQ_ID FROM RFQ_MSTR WHERE RM_Coy_ID=  '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " _
                   & "AND RM_Created_By='" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " _
                   & "AND RM_B_DISPLAY_STATUS = '0') AND RIV_S_Coy_ID NOT IN " _
                   & "(SELECT  RTVDT_v_company_id  FROM  RFQ_VEN_DISTR_LIST_MSTR DISM,  RFQ_INVITED_VENLIST_MSTR DISMT ," _
                   & "RFQ_INVITED_VENLIST_DETAIL  WHERE DISMT.RIVMT_User_ID='" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " _
                   & "AND  DISMT.RIVMT_RFQ_ID = '" & Common.Parse(rfq_id) & "' " _
                   & "And DISMT.RIVMT_RFQ_ID = RTVDT_RFQ_ID And DISMT.RIVMT_Distribution_list_id = DISM.RVDLM_List_Index " _
                   & "AND RTVDT_Distribution_List_Id =DISM.RVDLM_List_Index)"

            ds = objDb.FillDs(strsql)

            get_RFQVenListDetails = ds
            ds = Nothing

        End Function

        Public Function get_RFQVenDetails(ByVal SearchId As String, ByVal type As String) As DataSet
            Dim ds As DataSet
            Dim strsql As String

            If type = "S" Then
                strsql = "SELECT CM_COY_NAME AS RVDLM_List_Name,CM_COY_ID AS RVDLM_List_Index,'specific' AS TYPE,'Y' AS Added " _
                        & "FROM company_mstr " _
                        & "WHERE CM_COY_ID='" & SearchId & "'"

            ElseIf type = "L" Then
                strsql = "SELECT RVDLM_List_Name,CAST(RVDLM_List_Index AS CHAR) AS RVDLM_List_Index,'list' AS TYPE,'Y' AS Added " _
                        & "FROM rfq_ven_distr_list_mstr " _
                        & "WHERE RVDLM_List_Index=" & SearchId
            End If

            ds = objDb.FillDs(strsql)
            get_RFQVenDetails = ds
            ds = Nothing
        End Function

        Public Function getVendorDetails(ByVal srchString As ArrayList) As DataSet
            Dim sqlSrchVendor As String
            Dim ds As New DataSet
            Dim i As Integer = 0
            Dim strSearch As String = ""

            'sqlSrchVendor = "SELECT COMPANY_MSTR.CM_COY_ID,COMPANY_MSTR.CM_COY_NAME," & _
            '              "COMPANY_MSTR.CM_ADDR_LINE1,COMPANY_MSTR.CM_ADDR_LINE2,COMPANY_MSTR.CM_ADDR_LINE3,COMPANY_MSTR.CM_POSTCODE" & _
            '               " ,COMPANY_MSTR.CM_EMAIL,COMPANY_MSTR.CM_PHONE,CM_COUNTRY,CM_STATE,CM_CITY "

            'sqlSrchVendor = sqlSrchVendor & " from RFQ_INVITED_VENLIST_DETAIL RTVDT, COMPANY_MSTR " & _
            '" WHERE RTVDT.RTVDT_User_Id='" & Common.Parse(HttpContext.Current.Session("UserId")) & "' and RTVDT.RTVDT_RFQ_Name ='" & Common.Parse(srchString) & "' " & _
            '" and RTVDT.RTVDT_Distribution_List_Id='" & Common.Parse(dis_id) & "' and COMPANY_MSTR.CM_COY_ID = RTVDT.RTVDT_v_company_id AND COMPANY_MSTR.CM_STATUS = 'A' AND RTVDT.RTVDT_RFQ_ID = " & rfq_id

            For i = 0 To srchString.Count - 1
                If strSearch = "" Then
                    strSearch = "('" & srchString(i) & "'"

                Else
                    strSearch = strSearch & ",'" & srchString(i) & "'"
                End If
            Next
            strSearch = strSearch & ")"

            sqlSrchVendor = "SELECT COMPANY_MSTR.CM_COY_ID,COMPANY_MSTR.CM_COY_NAME," _
                        & "COMPANY_MSTR.CM_ADDR_LINE1,COMPANY_MSTR.CM_ADDR_LINE2,COMPANY_MSTR.CM_ADDR_LINE3," _
                        & "COMPANY_MSTR.CM_POSTCODE, COMPANY_MSTR.CM_EMAIL, COMPANY_MSTR.CM_PHONE, CM_COUNTRY, CM_STATE, CM_CITY " _
                        & "FROM COMPANY_MSTR " _
                        & "WHERE COMPANY_MSTR.CM_COY_ID IN " & strSearch & " AND COMPANY_MSTR.CM_STATUS = 'A'"

            ds = objDb.FillDs(sqlSrchVendor)
            getVendorDetails = ds
        End Function

        Public Function getVendorView(ByVal srchString As String, ByVal edit As String, Optional ByVal dis_id As String = "", Optional ByVal rfq_id As String = "") As DataSet
            Dim sqlSrchVendor As String
            Dim ds As New DataSet

            ' sqlSrchVendor = "SELECT CM_COY_ID,CM_COY_NAME,CM_ADDR_LINE1,CM_ADDR_LINE2,CM_ADDR_LINE3," & _
            '"CM_POSTCODE,CM_EMAIL,CM_PHONE, B.CODE_DESC AS STATE, C.CODE_DESC AS COUNTRY  FROM COMPANY_MSTR" & _
            '"LEFT JOIN CODE_MSTR AS B ON CM_STATE = B.CODE_ABBR AND B.CODE_CATEGORY = 'S' AND B.CODE_VALUE = CM_COUNTRY " & _
            ' "LEFT JOIN CODE_MSTR AS C ON CM_COUNTRY = C.CODE_ABBR AND C.CODE_CATEGORY = 'CT' "

            sqlSrchVendor = "SELECT COMPANY_MSTR.CM_COY_ID,COMPANY_MSTR.CM_COY_NAME," & _
                          "COMPANY_MSTR.CM_ADDR_LINE1,COMPANY_MSTR.CM_ADDR_LINE2,COMPANY_MSTR.CM_ADDR_LINE3,COMPANY_MSTR.CM_POSTCODE" & _
                           " ,COMPANY_MSTR.CM_EMAIL,COMPANY_MSTR.CM_PHONE,CM_COUNTRY,CM_STATE,CM_CITY "

            If edit = "0" Then
                Dim str As String = getAvailableVendor(chkPurchasingRole)
                sqlSrchVendor = sqlSrchVendor & str

                If Not System.Text.RegularExpressions.Regex.IsMatch(srchString, "[A-Za-z]") Then
                    sqlSrchVendor = sqlSrchVendor & _
                                      "and NOT (left(cm_coy_name,1) between 'A' AND 'Z')"
                Else
                    sqlSrchVendor = sqlSrchVendor & _
                                      " AND CM_COY_NAME " & Common.ParseSQL(srchString & "*")
                End If
            ElseIf edit = "1" Then
                sqlSrchVendor = sqlSrchVendor & " from RFQ_INVITED_VENLIST_DETAIL RTVDT, COMPANY_MSTR " & _
                " WHERE RTVDT.RTVDT_User_Id='" & Common.Parse(HttpContext.Current.Session("UserId")) & "' and RTVDT.RTVDT_RFQ_Name ='" & Common.Parse(srchString) & "' " & _
                " and RTVDT.RTVDT_Distribution_List_Id='" & Common.Parse(dis_id) & "' and COMPANY_MSTR.CM_COY_ID = RTVDT.RTVDT_v_company_id AND COMPANY_MSTR.CM_STATUS = 'A' AND RTVDT.RTVDT_RFQ_ID = " & rfq_id
            End If

            ds = objDb.FillDs(sqlSrchVendor)
            getVendorView = ds
        End Function

        Public Function Vendor_check(ByVal value() As String)
            Dim i As Integer
            Dim strvdr As String = "select Distinct UPPER(substring(CM_COY_NAME,1,1)) as vendor" 'select from com_vendor and join with company_mstr if data in company mstr not exist errer with occur
            Dim str As String = getAvailableVendor(chkPurchasingRole)
            strvdr = strvdr & str & "order by vendor"
            Dim tDS As DataSet = objDb.FillDs(strvdr)
            i = 0
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                value(i) = tDS.Tables(0).Rows(j).Item("vendor")
                i = i + 1
            Next
        End Function

        Public Function Vendor_check_AZ(ByVal value() As String)
            Dim i As Integer
            Dim strvdr As String = "select Distinct UPPER(substring(CM_COY_NAME,1,1)) as vendor" 'select from com_vendor and join with company_mstr if data in company mstr not exist errer with occur
            Dim str As String = getAvailableVendor_AZ(chkPurchasingRole)
            strvdr = strvdr & str & "order by vendor"
            Dim tDS As DataSet = objDb.FillDs(strvdr)
            i = 0
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                value(i) = tDS.Tables(0).Rows(j).Item("vendor")
                i = i + 1
            Next
        End Function

        Public Function update_option(ByVal rfq_id As String, ByVal rfq_option As String)
            Dim strsql As String = "update RFQ_MSTR set RM_RFQ_OPTION = '" & rfq_option & "' where RM_RFQ_ID = '" & rfq_id & "'"
            objDb.Execute(strsql)
        End Function

        Public Function GET_BUYERNAME() As String
            Dim strsql As String = "select UM_USER_NAME from USER_MSTR where UM_USER_id='" & HttpContext.Current.Session("UserId") & "' and UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "'"
            GET_BUYERNAME = objDb.GetVal(strsql)
        End Function

        ' ai chu modified on 11/01/2006
        Public Function save_RFQ_MSTR(ByVal value() As String, ByRef rfq_num As String, ByRef rfq_id As String, ByVal dt As DataTable) As String
            Dim objglb As New AppGlobals
            Dim strsql, rfq_no, prefix As String
            Dim strAryQuery(0) As String
            Dim blnInsert As Boolean
            Dim i As Integer

            ' ----- ai chu remark on 24/01/2006 -----
            'strsql = "select RVM_V_RFQ_Status from RFQ_VENDOR_MSTR where " & _
            '        " RVM_RFQ_ID='" & Common.Parse(rfq_id) & "'"
            'read = objDb.GetReader(strsql)
            'If read.Read Then
            '    save_RFQ_MSTR = "2"
            '    Exit Function
            'End If
            'read.Close()
            ' ----- end remark ----------------------

            ' to check RFQ name exists, only RFQ Name in Trash folder can be reused;
            ' Draft RFQ name also need to be checked
            ' RM_B_DISPLAY_STATUS = 1(Trash), 2(Remove From Trash) 
            'strsql = "SELECT '*' FROM RFQ_MSTR WHERE RM_RFQ_ID = '" & rfq_id & "' AND RM_B_DISPLAY_STATUS = '0'"
            'If objDb.Exist(strsql) > 0 Then
            '    save_RFQ_MSTR = "2"
            '    Exit Function
            'End If

            strsql = "select '*' from RFQ_MSTR "
            strsql &= "WHERE RM_Coy_ID= '" & HttpContext.Current.Session("CompanyID") & "' "
            strsql &= "AND RM_RFQ_Name='" & Common.Parse(value(0)) & "' "
            strsql &= "AND RM_Created_By ='" & HttpContext.Current.Session("UserID") & "' "
            strsql &= "AND RM_B_DISPLAY_STATUS = '0' "
            If objDb.Exist(strsql) = 0 Then ' record not exists, so insert
                objglb.GetLatestDocNo("RFQ", strAryQuery, rfq_num, prefix)
                If chk_rfqid(rfq_num) = True Then
                    save_RFQ_MSTR = "6"
                    Exit Function
                End If

                strsql = "insert into RFQ_MSTR" &
                "(RM_Prefix,RM_RFQ_No,RM_Coy_ID,RM_RFQ_Name, " &
                " RM_Expiry_Date,RM_Remark,RM_Created_By,RM_Created_On," &
                " RM_Currency_Code,RM_Payment_Term,RM_Payment_Type,RM_Shipment_Term ," &
                " RM_Shipment_Mode,RM_Contact_Person,RM_Contact_Number,RM_Email," &
                " RM_Reqd_Quote_Validity,RM_RFQ_OPTION,RM_Status,RM_B_Display_Status)" &
                "values ('" & Common.Parse(prefix) & "','" & Common.Parse(rfq_num) & "' ,'" & HttpContext.Current.Session("CompanyID") & "','" & Common.Parse(value(0)) & "' " &
                " ," & Common.ConvertDate(value(1)) & ",'" & Common.Parse(value(2)) & "','" & HttpContext.Current.Session("UserID") & "'," & Common.ConvertDate(value(4)) & ", " &
                " '" & Common.Parse(value(5)) & "','" & Common.Parse(value(6).Trim()) & " ','" & Common.Parse(value(7).Trim()) & "','" & Common.Parse(value(8)) & "'" &
                ",'" & Common.Parse(value(9)) & "','" & Common.Parse(value(3)) & "','" & Common.Parse(value(11)) & "','" & Common.Parse(value(12)) & "', " &
                " " & Common.ConvertDate(value(13)) & ",'" & Common.Parse(value(14)) & "','" & Common.Parse(value(15)) & "','" & Common.Parse(value(16)) & "')"
                Common.Insert2Ary(strAryQuery, strsql)
                blnInsert = True
            Else
                If rfq_id <> "" Then ' for update
                    strsql = "update RFQ_MSTR set RM_Expiry_Date=" & Common.ConvertDate(value(1)) & ","
                    strsql &= "RM_Remark='" & Common.Parse(value(2)) & "',RM_Created_By='" & Common.Parse(HttpContext.Current.Session("UserID")) & "',"

                    'If rfq_num <> "" Then
                    '    strsql &= "RM_RFQ_No = '" & rfq_num & "', "
                    'End If

                    strsql &= "RM_Created_On=" & Common.ConvertDate(value(4)) & ",RM_Currency_Code='" & Common.Parse(value(5)) & "',"
                    strsql &= "RM_Payment_Term='" & Common.Parse(value(6)) & " ',"
                    strsql &= "RM_Payment_Type='" & Common.Parse(value(7)) & "',RM_Shipment_Term='" & Common.Parse(value(8)) & "',"
                    strsql &= "RM_Shipment_Mode='" & Common.Parse(value(9)) & "',RM_Contact_Person='" & Common.Parse(value(10)) & "',"
                    strsql &= "RM_Contact_Number='" & Common.Parse(value(11)) & "', "
                    strsql &= "RM_Email='" & Common.Parse(value(12)) & "',RM_Reqd_Quote_Validity=" & Common.ConvertDate(value(13)) & " "
                    strsql &= "where rm_rfq_id = '" & rfq_id & "' and  RM_Coy_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' "
                    strsql &= "and RM_RFQ_Name='" & Common.Parse(value(0)) & "' "
                    Common.Insert2Ary(strAryQuery, strsql)
                    'If blnSaveDetail = False Then
                    '    objDb.BatchExecute(strAryQuery)
                    '    Exit Function
                    'End If
                    blnInsert = False
                End If
            End If


            Dim ctx As HttpContext = HttpContext.Current
            Dim objStreamWriter As StreamWriter

            'If blnSaveDetail Then
            For i = 0 To dt.Rows.Count - 1
                strsql = "insert into RFQ_DETAIL (RD_RFQ_ID,RD_Coy_ID,RD_RFQ_Line,RD_Product_Code,"
                strsql &= "RD_Vendor_Item_Code,RD_Quantity,RD_Product_Desc,"
                strsql &= "RD_UOM,RD_Delivery_Lead_Time,RD_Warranty_Terms,"
                strsql &= "RD_Product_Name) VALUES ( "
                If blnInsert Then
                    'Sam add on 21/09/2010 - Query to be accepted in MYSQL
                    'strsql &= objDb.GetLatestInsertedID("RFQ_MSTR", "RM_RFQ_ID") & " , "
                    strsql &= " " & objDb.GetLatestInsertedID("RFQ_MSTR") & " , "
                Else
                    strsql &= Common.Parse(rfq_id) & ", "
                End If

                'Michelle (3/10/2010) - To rectify the problem where line_no is always 1
                'strsql &= "'" & Common.Parse(dt.Rows(i)("VCoyId")) & "'," & objDb.GetVal(" Select ISNULL(MAX(RD_RFQ_Line),0) + 1 as count From RFQ_DETAIL,RFQ_MSTR Where RD_RFQ_ID = RM_RFQ_ID AND RM_Created_By='" & Common.Parse(HttpContext.Current.Session("UserId")) & "' And RM_RFQ_Name= '" & Common.Parse(value(0)) & "' and RM_Coy_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' ") & ","
                strsql &= "'" & Common.Parse(dt.Rows(i)("VCoyId")) & "'," & " (Select ISNULL(MAX(RD.RD_RFQ_Line),0) + 1 as count From RFQ_DETAIL RD,RFQ_MSTR Where RD.RD_RFQ_ID = RM_RFQ_ID AND RM_Created_By='" & Common.Parse(HttpContext.Current.Session("UserId")) & "' And RM_RFQ_Name= '" & Common.Parse(value(0)) & "' and RM_Coy_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "') " & ","
                strsql &= "'" & Common.Parse(dt.Rows(i)("ProductId")) & "'," & "'" & Common.Parse(dt.Rows(i)("VIC")) & "',"
                strsql &= "" & Common.Parse(dt.Rows(i)("Qty")) & ",'" & Common.Parse(dt.Rows(i)("ProductDesc")) & "' ,"
                strsql &= "'" & Common.Parse(dt.Rows(i)("UOM")) & "'," & Common.Parse(dt.Rows(i)("DeliveryLeadTime")) & ","
                'Michelle (3/10/2010) - To remove the ' from the WarrantyTermsstrsql &= "'" & Common.Parse(dt.Rows(i)("WarrantyTerms")) & "', " & "'" & Common.Parse(dt.Rows(i)("ProductDesc")) & "' )"
                'strsql &= "'" & Common.Parse(dt.Rows(i)("WarrantyTerms")) & "', " & "'" & Common.Parse(dt.Rows(i)("ProductDesc")) & "' )"
                strsql &= Common.Parse(dt.Rows(i)("WarrantyTerms")) & ", " & "'" & Common.Parse(dt.Rows(i)("ProductDesc")) & "' )"

                Common.Insert2Ary(strAryQuery, strsql)

                ' ai chu add on 09/01/2006
                ' to trace duplication of primary key for RFQ_DETAIL
                Try
                    objStreamWriter = New StreamWriter(ctx.Server.MapPath(ctx.Request.ApplicationPath & "\" & Format(Now.Date, "yyyyMMdd") & "_RFQDetail.log"), True)
                    objStreamWriter.WriteLine(strsql)
                Catch Meexp As Exception
                Finally
                    objStreamWriter.Close()
                    objStreamWriter = Nothing
                End Try
            Next
            'End If

            If strAryQuery(0) <> String.Empty Then
                If objDb.BatchExecute(strAryQuery) Then
                    strsql = "Select RM_RFQ_ID, RM_RFQ_No From RFQ_MSTR Where RM_RFQ_Name = '" & Common.Parse(value(0)) & "' "
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
        'Michelle (3/11/2010) - Replacing the save_RFQ_MSTR
        'Public Function save_RFQ(ByVal strVendorList As String, ByVal value() As String, ByRef rfq_num As String, ByRef rfq_id As String, ByVal dt As DataTable, Optional ByVal dsVendor As DataSet = Nothing) As String
        '    Dim objglb As New AppGlobals
        '    Dim strsql, rfq_no, prefix As String
        '    Dim strAryQuery(0) As String
        '    Dim blnInsert As Boolean
        '    Dim i As Integer
        '    Dim dtList As DataTable
        '    Dim dtDetails As DataTable
        '    Dim dr As DataRow

        '    dtList = dsVendor.Tables(0)
        '    dtDetails = dsVendor.Tables(1)

        '    strsql = "select '*' from RFQ_MSTR "
        '    strsql &= "WHERE RM_Coy_ID= '" & HttpContext.Current.Session("CompanyID") & "' "
        '    'strsql &= "AND RM_RFQ_Name='" & Common.Parse(value(0)) & "' "
        '    strsql &= "AND RM_RFQ_NO='" & Common.Parse(rfq_num) & "' "
        '    strsql &= "AND RM_Created_By ='" & HttpContext.Current.Session("UserID") & "' "
        '    strsql &= "AND RM_B_DISPLAY_STATUS = '0' "
        '    If objDb.Exist(strsql) = 0 Then ' record not exists, so insert
        '        objglb.GetLatestDocNo("RFQ", strAryQuery, rfq_num, prefix)
        '        If chk_rfqid(rfq_num) = True Then
        '            save_RFQ = "6"
        '            Exit Function
        '        End If

        '        'Insert into RFQ_MSTR
        '        strsql = "insert into RFQ_MSTR" & _
        '        "(RM_Prefix,RM_RFQ_No,RM_Coy_ID,RM_RFQ_Name, " & _
        '        " RM_Expiry_Date,RM_Remark,RM_Created_By,RM_Created_On," & _
        '        " RM_Currency_Code,RM_Payment_Term,RM_Payment_Type,RM_Shipment_Term ," & _
        '        " RM_Shipment_Mode,RM_Contact_Person,RM_Contact_Number,RM_Email," & _
        '        " RM_Reqd_Quote_Validity,RM_RFQ_OPTION,RM_Status,RM_B_Display_Status)" & _
        '        "values ('" & Common.Parse(prefix) & "','" & Common.Parse(rfq_num) & "' ,'" & HttpContext.Current.Session("CompanyID") & "','" & Common.Parse(value(0)) & "' " & _
        '        " ," & Common.ConvertDate(value(1)) & ",'" & Common.Parse(value(2)) & "','" & HttpContext.Current.Session("UserID") & "'," & Common.ConvertDate(value(4)) & ", " & _
        '        " '" & Common.Parse(value(5)) & "','" & Common.Parse(value(6)) & " ','" & Common.Parse(value(7)) & "','" & Common.Parse(value(8)) & "'" & _
        '        ",'" & Common.Parse(value(9)) & "','" & Common.Parse(value(3)) & "','" & Common.Parse(value(11)) & "','" & Common.Parse(value(12)) & "', " & _
        '        " " & Common.ConvertDate(value(13)) & ",'" & Common.Parse(value(14)) & "','" & Common.Parse(value(15)) & "','" & Common.Parse(value(16)) & "')"
        '        Common.Insert2Ary(strAryQuery, strsql)
        '        blnInsert = True
        '    Else
        '        If rfq_id <> "" Then ' for update
        '            strsql = "update RFQ_MSTR set RM_Expiry_Date=" & Common.ConvertDate(value(1)) & ","
        '            strsql &= "RM_Remark='" & Common.Parse(value(2)) & "',RM_Created_By='" & Common.Parse(HttpContext.Current.Session("UserID")) & "',"
        '            strsql &= "RM_RFQ_Name='" & Common.Parse(value(0)) & "', "
        '            strsql &= "RM_Created_On=" & Common.ConvertDate(value(4)) & ",RM_Currency_Code='" & Common.Parse(value(5)) & "',"
        '            strsql &= "RM_Payment_Term='" & Common.Parse(value(6)) & " ',"
        '            strsql &= "RM_Payment_Type='" & Common.Parse(value(7)) & "',RM_Shipment_Term='" & Common.Parse(value(8)) & "',"
        '            strsql &= "RM_Shipment_Mode='" & Common.Parse(value(9)) & "',RM_Contact_Person='" & Common.Parse(value(10)) & "',"
        '            strsql &= "RM_Contact_Number='" & Common.Parse(value(11)) & "', "
        '            strsql &= "RM_Email='" & Common.Parse(value(12)) & "',RM_Reqd_Quote_Validity=" & Common.ConvertDate(value(13)) & " "
        '            strsql &= "where rm_rfq_id = '" & rfq_id & "' and  RM_Coy_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' "
        '            Common.Insert2Ary(strAryQuery, strsql)
        '            blnInsert = False
        '        End If
        '    End If

        '    If strVendorList <> "" Then
        '        'Update the records in RFQ_INVITED_VENLIST by
        '        ' i) delete the records that user has mark for deletion
        '        ' ii) add the new records that user has selected
        '        If rfq_id = "" Then 'add the new records that user has selected
        '            strsql = "insert into RFQ_INVITED_VENLIST "
        '            strsql &= "select " & objDb.GetLatestInsertedID("RFQ_MSTR") & ", "
        '            strsql &= "CM.CM_COY_ID,CM.CM_COY_NAME,CM.CM_ADDR_LINE1,CM.CM_ADDR_LINE2,CM.CM_ADDR_LINE3,CM.CM_POSTCODE,CM.CM_CITY,CM.CM_STATE,CM.CM_COUNTRY,CM.CM_PHONE,CM.CM_FAX,CM.CM_EMAIL "
        '            strsql &= "FROM COMPANY_MSTR CM WHERE CM.CM_COY_ID IN (" & strVendorList & ")"
        '            Common.Insert2Ary(strAryQuery, strsql)
        '        Else 'ie Draft RFQ, delete the existing records and add in the new records
        '            strsql = "delete from RFQ_INVITED_VENLIST "
        '            strsql &= "where RIV_RFQ_ID = '" & rfq_id & "'"
        '            Common.Insert2Ary(strAryQuery, strsql)
        '            strsql = "insert into RFQ_INVITED_VENLIST "
        '            strsql &= "select " & rfq_id & ", "
        '            strsql &= "CM.CM_COY_ID,CM.CM_COY_NAME,CM.CM_ADDR_LINE1,CM.CM_ADDR_LINE2,CM.CM_ADDR_LINE3,CM.CM_POSTCODE,CM.CM_CITY,CM.CM_STATE,CM.CM_COUNTRY,CM.CM_PHONE,CM.CM_FAX,CM.CM_EMAIL "
        '            strsql &= "FROM COMPANY_MSTR CM WHERE CM.CM_COY_ID IN (" & strVendorList & ")"
        '            Common.Insert2Ary(strAryQuery, strsql)
        '        End If
        '    End If

        '    strsql = "delete from RFQ_DETAIL "
        '    strsql &= "where RD_RFQ_ID = '" & rfq_id & "'"
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    For i = 0 To dt.Rows.Count - 1
        '        strsql = "insert into RFQ_DETAIL (RD_RFQ_ID,RD_Coy_ID,RD_RFQ_Line,"
        '        strsql &= "RD_Vendor_Item_Code,RD_Quantity,RD_Product_Desc, RD_PRODUCT_CODE, "
        '        strsql &= "RD_UOM,RD_Delivery_Lead_Time, "
        '        strsql &= "RD_Product_Name) VALUES ( "
        '        If blnInsert Then
        '            strsql &= " " & objDb.GetLatestInsertedID("RFQ_MSTR") & " , "
        '        Else
        '            strsql &= Common.Parse(rfq_id) & ", "
        '        End If

        '        strsql &= "'" & Common.Parse(dt.Rows(i)("VCoyId")) & "'," & (i + 1) & ","
        '        strsql &= "'" & Common.Parse(dt.Rows(i)("VIC")) & "',"
        '        strsql &= "" & Common.Parse(dt.Rows(i)("Qty")) & ",'" & Common.Parse(dt.Rows(i)("ProductDesc")) & "' ,"
        '        strsql &= "'" & Common.Parse(dt.Rows(i)("ProductID")) & "',"
        '        strsql &= "'" & Common.Parse(dt.Rows(i)("UOM")) & "'," & Common.Parse(dt.Rows(i)("DeliveryLeadTime")) & ","
        '        strsql &= "'" & Common.Parse(dt.Rows(i)("ProductDesc")) & "' )"

        '        Common.Insert2Ary(strAryQuery, strsql)
        '    Next

        '    'update the Doc Attachement
        '    strsql = "update COMPANY_DOC_ATTACHMENT set CDA_DOC_NO = '" & Common.Parse(rfq_num) & "' "
        '    strsql &= "where CDA_DOC_NO = '" & HttpContext.Current.Session.SessionID & "' AND CDA_DOC_TYPE = 'RFQ' "
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    'Update vendor list
        '    For Each dr In dtList.Rows


        '    Next

        '    For Each dr In dtDetails.Rows


        '    Next

        '    If strAryQuery(0) <> String.Empty Then
        '        If objDb.BatchExecute(strAryQuery) Then
        '            'strsql = "Select RM_RFQ_ID, RM_RFQ_No From RFQ_MSTR Where RM_RFQ_Name = '" & Common.Parse(value(0)) & "' "
        '            strsql = "Select RM_RFQ_ID, RM_RFQ_No From RFQ_MSTR Where RM_RFQ_NO = '" & Common.Parse(rfq_num) & "' "
        '            strsql &= "AND RM_Created_By = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' "
        '            strsql &= "AND RM_Coy_ID = '" & HttpContext.Current.Session("CompanyID") & "' "
        '            strsql &= "AND RM_B_DISPLAY_STATUS = '0'"

        '            Dim tDS As DataSet = objDb.FillDs(strsql)
        '            If tDS.Tables(0).Rows.Count > 0 Then
        '                rfq_id = tDS.Tables(0).Rows(0).Item("RM_RFQ_ID")
        '                rfq_num = tDS.Tables(0).Rows(0).Item("RM_RFQ_No")
        '            End If
        '        End If
        '    End If
        'End Function

        Public Function save_RFQ(ByVal strVendorList As String, ByVal value() As String, ByRef rfq_num As String, ByRef rfq_id As String, ByVal dt As DataTable, Optional ByVal dtList As DataTable = Nothing, Optional ByVal dtDetails As DataTable = Nothing, Optional ByVal submit As Boolean = True) As String
            'Modified by Joon on 27th May 2011
            Dim objglb As New AppGlobals
            Dim strsql, rfq_no, prefix As String
            Dim strAryQuery(0) As String
            Dim blnInsert As Boolean
            Dim i As Integer
            'Dim dtList As DataTable
            'Dim dtDetails As DataTable
            Dim drList As DataRow
            Dim drDetails As DataRow
            Dim dtrList As DataRow()
            Dim dtrDetails As DataRow()
            Dim strSearch As String = ""
            'dtList = dsVendor.Tables(0)
            'dtDetails = dsVendor.Tables(1)
            Dim intIncrementNo As Integer = 0

            strsql = "select '*' from RFQ_MSTR "
            strsql &= "WHERE RM_Coy_ID= '" & HttpContext.Current.Session("CompanyID") & "' "
            'strsql &= "AND RM_RFQ_Name='" & Common.Parse(value(0)) & "' "
            strsql &= "AND RM_RFQ_NO='" & Common.Parse(rfq_num) & "' "
            strsql &= "AND RM_Created_By ='" & HttpContext.Current.Session("UserID") & "' "
            strsql &= "AND RM_B_DISPLAY_STATUS = '0' "
            If objDb.Exist(strsql) = 0 Then ' record not exists, so insert

                strsql = " SET @T_NO = ''; UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CP_PARAM_TYPE = 'RFQ' "
                Common.Insert2Ary(strAryQuery, strsql)

                '' ''objglb.GetLatestDocNo("RFQ", strAryQuery, rfq_num, prefix)

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
                            " RM_Reqd_Quote_Validity,RM_RFQ_OPTION,RM_Status,RM_B_Display_Status, RM_Submission_Date)" & _
                            "values (" & prefix & "," & rfq_num & " ,'" & HttpContext.Current.Session("CompanyID") & "','" & Common.Parse(value(0)) & "' " & _
                            " ," & Common.ConvertDate(value(1)) & ",'" & Common.Parse(value(2)) & "','" & HttpContext.Current.Session("UserID") & "'," & Common.ConvertDate(value(4)) & ", " & _
                            " '" & Common.Parse(value(5)) & "','" & Common.Parse(value(6)) & " ','" & Common.Parse(value(7)) & "','" & Common.Parse(value(8)) & "'" & _
                            ",'" & Common.Parse(value(9)) & "','" & Common.Parse(value(3)) & "','" & Common.Parse(value(11)) & "','" & Common.Parse(value(12)) & "', " & _
                            " " & Common.ConvertDate(value(13)) & ",'" & Common.Parse(value(14)) & "','" & Common.Parse(value(15)) & "','" & Common.Parse(value(16)) & "','" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "')"
                Else
                    strsql = "insert into RFQ_MSTR" & _
                            "(RM_Prefix,RM_RFQ_No,RM_Coy_ID,RM_RFQ_Name, " & _
                            " RM_Expiry_Date,RM_Remark,RM_Created_By,RM_Created_On," & _
                            " RM_Currency_Code,RM_Payment_Term,RM_Payment_Type,RM_Shipment_Term ," & _
                            " RM_Shipment_Mode,RM_Contact_Person,RM_Contact_Number,RM_Email," & _
                            " RM_Reqd_Quote_Validity,RM_RFQ_OPTION,RM_Status,RM_B_Display_Status)" & _
                            "values (" & prefix & "," & rfq_num & " ,'" & HttpContext.Current.Session("CompanyID") & "','" & Common.Parse(value(0)) & "' " & _
                            " ," & Common.ConvertDate(value(1)) & ",'" & Common.Parse(value(2)) & "','" & HttpContext.Current.Session("UserID") & "'," & Common.ConvertDate(value(4)) & ", " & _
                            " '" & Common.Parse(value(5)) & "','" & Common.Parse(value(6)) & " ','" & Common.Parse(value(7)) & "','" & Common.Parse(value(8)) & "'" & _
                            ",'" & Common.Parse(value(9)) & "','" & Common.Parse(value(3)) & "','" & Common.Parse(value(11)) & "','" & Common.Parse(value(12)) & "', " & _
                            " " & Common.ConvertDate(value(13)) & ",'" & Common.Parse(value(14)) & "','" & Common.Parse(value(15)) & "','" & Common.Parse(value(16)) & "')"
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

                ''Update vendor list
                'Dim dsTemp As New DataSet

                ''Insert in to database if record not found
                'For Each drList In dtList.Rows
                '    If drList.Item("TYPE") = "list" Then
                '        strsql = "SELECT '*' FROM RFQ_INVITED_VENLIST_MSTR " _
                '                & "WHERE RIVMT_User_ID='" & Common.Parse(HttpContext.Current.Session("UserID")) & "' " _
                '                & "AND RIVMT_RFQ_Name='" & Common.Parse(value(0)) & "' " _
                '                & "AND RIVMT_Distribution_list_id='" & drList.Item("RVDLM_List_Index") & "' " _
                '                & "AND RIVMT_RFQ_ID=" & rfq_id
                '        If objDb.Exist(strsql) = 0 Then ' record not exists, so insert
                '            strsql = "insert into RFQ_INVITED_VENLIST_MSTR(RIVMT_User_ID,RIVMT_RFQ_Name," _
                '                    & "RIVMT_Distribution_list_id,RIVMT_RFQ_ID) " _
                '                    & "values ('" & Common.Parse(HttpContext.Current.Session("UserID")) & "','" _
                '                    & Common.Parse(value(0)) & "','" & drList.Item("RVDLM_List_Index") & "'," _
                '                    & rfq_id & ")"
                '            Common.Insert2Ary(strAryQuery, strsql)
                '        End If

                '        strSearch = "RVDLM_List_Index='" & drList.Item("RVDLM_List_Index") & "' AND TYPE = 'list'"
                '        dtrDetails = dtDetails.Select(strSearch)
                '        If dtrDetails.Length > 0 Then
                '            For Each oRow As DataRow In dtrDetails
                '                strsql = "SELECT '*' FROM RFQ_INVITED_VENLIST_DETAIL " _
                '                    & "WHERE RTVDT_User_Id='" & Common.Parse(HttpContext.Current.Session("UserID")) & "' " _
                '                    & "AND RTVDT_RFQ_Name='" & Common.Parse(value(0)) & "' " _
                '                    & "AND RTVDT_Distribution_List_Id='" & drList.Item("RVDLM_List_Index") & "' " _
                '                    & "AND RTVDT_v_company_id = '" & Common.Parse(oRow.Item("CoyId")) & "' " _
                '                    & "AND RTVDT_RFQ_ID = " & rfq_id
                '                If objDb.Exist(strsql) = 0 Then ' record not exists, so insert
                '                    strsql = "insert into RFQ_INVITED_VENLIST_DETAIL(RTVDT_User_Id,RTVDT_RFQ_Name," _
                '                           & "RTVDT_Distribution_List_Id,RTVDT_v_company_id,RTVDT_RFQ_ID) " _
                '                           & "values ('" & Common.Parse(HttpContext.Current.Session("UserID")) & "','" _
                '                           & Common.Parse(value(0)) & "','" & drList.Item("RVDLM_List_Index") & "','" _
                '                           & Common.Parse(oRow.Item("CoyId")) & "'," & rfq_id & ")"
                '                    Common.Insert2Ary(strAryQuery, strsql)
                '                End If

                '                strsql = "SELECT '*' FROM RFQ_INVITED_VENLIST " _
                '                    & "WHERE RIV_RFQ_ID= " & rfq_id & " " _
                '                    & "AND RIV_RFQ_ID IN " _
                '                    & "(SELECT RM_RFQ_ID FROM RFQ_MSTR " _
                '                    & "WHERE rm_rfq_no='" & Common.Parse(rfq_num) & "' " _
                '                    & "AND RM_RFQ_Name='" & Common.Parse(value(0)) & "' " _
                '                    & "AND RM_Coy_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "') " _
                '                    & "AND RIV_S_Coy_ID='" & Common.Parse(oRow.Item("CoyId")) & "'"
                '                If objDb.Exist(strsql) = 0 Then ' record not exists, so insert
                '                    strsql = "insert into RFQ_INVITED_VENLIST select distinct " _
                '                         & rfq_id & ", " _
                '                         & "CM.CM_COY_ID," _
                '                         & "CM.CM_COY_NAME,CM.CM_ADDR_LINE1,CM.CM_ADDR_LINE2,CM.CM_ADDR_LINE3," _
                '                         & "CM.CM_POSTCODE,CM.CM_CITY,CM.CM_STATE,CM.CM_COUNTRY,CM.CM_PHONE,CM.CM_FAX," _
                '                         & "CM.CM_EMAIL " _
                '                         & "FROM COMPANY_MSTR CM " _
                '                         & "WHERE CM.CM_COY_ID='" & Common.Parse(oRow.Item("CoyId")) & "'"
                '                    Common.Insert2Ary(strAryQuery, strsql)
                '                End If
                '            Next
                '        End If

                '    ElseIf drList.Item("TYPE") = "specific" Then
                '        strSearch = "RVDLM_List_Index='" & drList.Item("RVDLM_List_Index") & "' AND TYPE = 'specific'"
                '        dtrDetails = dtDetails.Select(strSearch)
                '        If dtrDetails.Length > 0 Then
                '            For Each oRow As DataRow In dtrDetails
                '                strsql = "SELECT '*' FROM RFQ_INVITED_VENLIST " _
                '                    & "WHERE RIV_RFQ_ID= " & rfq_id & " " _
                '                    & "AND RIV_RFQ_ID IN " _
                '                    & "(SELECT RM_RFQ_ID FROM RFQ_MSTR " _
                '                    & "WHERE rm_rfq_no='" & Common.Parse(rfq_num) & "' " _
                '                    & "AND RM_RFQ_Name='" & Common.Parse(value(0)) & "' " _
                '                    & "AND RM_Coy_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "') " _
                '                    & "AND RIV_S_Coy_ID='" & Common.Parse(oRow.Item("CoyId")) & "'"
                '                If objDb.Exist(strsql) = 0 Then ' record not exists, so insert
                '                    strsql = "insert into RFQ_INVITED_VENLIST select distinct " _
                '                         & rfq_id & ", " _
                '                         & "CM.CM_COY_ID," _
                '                         & "CM.CM_COY_NAME,CM.CM_ADDR_LINE1,CM.CM_ADDR_LINE2,CM.CM_ADDR_LINE3," _
                '                         & "CM.CM_POSTCODE,CM.CM_CITY,CM.CM_STATE,CM.CM_COUNTRY,CM.CM_PHONE,CM.CM_FAX," _
                '                         & "CM.CM_EMAIL " _
                '                         & "FROM COMPANY_MSTR CM " _
                '                         & "WHERE CM.CM_COY_ID='" & Common.Parse(oRow.Item("CoyId")) & "'"
                '                    Common.Insert2Ary(strAryQuery, strsql)
                '                End If
                '            Next
                '        End If
                '    End If
                'Next

                ''Remove from database
                'Dim j As Integer
                ''RFQ_INVITED_VENLIST_MSTR
                'strsql = "SELECT * FROM RFQ_INVITED_VENLIST_MSTR " _
                '    & "WHERE RIVMT_RFQ_ID=" & rfq_id & " " _
                '    & "AND RIVMT_RFQ_Name='" & Common.Parse(value(0)) & "' " _
                '    & "AND RIVMT_User_ID='" & Common.Parse(HttpContext.Current.Session("UserID")) & "'"
                'Dim tDS2 As DataSet = objDb.FillDs(strsql)

                'For j = 0 To tDS2.Tables(0).Rows.Count - 1
                '    strSearch = "RVDLM_List_Index='" & tDS2.Tables(0).Rows(j).Item("RIVMT_Distribution_list_id") & "' AND TYPE = 'list'"
                '    dtrList = dtList.Select(strSearch)
                '    If dtrList.Length < 1 Then
                '        strsql = "delete from RFQ_INVITED_VENLIST_MSTR " _
                '            & "where RIVMT_User_ID='" & Common.Parse(HttpContext.Current.Session("UserID")) & "' " _
                '            & "and RIVMT_RFQ_Name='" & Common.Parse(value(0)) & "' " _
                '            & "and RIVMT_Distribution_list_id='" & tDS2.Tables(0).Rows(j).Item("RIVMT_Distribution_list_id") & "' " _
                '            & "and RIVMT_RFQ_ID = " & rfq_id
                '        Common.Insert2Ary(strAryQuery, strsql)
                '    End If
                'Next

                ''RFQ_INVITED_VENLIST_DETAIL
                'strsql = "SELECT * FROM RFQ_INVITED_VENLIST_DETAIL " _
                '    & "WHERE RTVDT_User_Id='" & Common.Parse(HttpContext.Current.Session("UserID")) & "' " _
                '    & "AND RTVDT_RFQ_Name='" & Common.Parse(value(0)) & "' " _
                '    & "And RTVDT_RFQ_ID = " & rfq_id
                'Dim tDS1 As DataSet = objDb.FillDs(strsql)

                'For j = 0 To tDS1.Tables(0).Rows.Count - 1
                '    strSearch = "RVDLM_List_Index='" & drList.Item("RVDLM_List_Index") & "' AND TYPE = 'specific'"
                '    dtrDetails = dtDetails.Select(strSearch)
                '    If dtrDetails.Length < 1 Then
                '        strsql = "delete from RFQ_INVITED_VENLIST_DETAIL " _
                '               & "where RTVDT_User_Id='" & Common.Parse(HttpContext.Current.Session("UserID")) & "' " _
                '               & "and RTVDT_RFQ_Name= '" & Common.Parse(value(0)) & "' " _
                '               & "and RTVDT_Distribution_List_Id='" & tDS1.Tables(0).Rows(j).Item("RIVMT_Distribution_list_id") & "' " _
                '               & "and RTVDT_RFQ_ID = " & rfq_id
                '        Common.Insert2Ary(strAryQuery, strsql)
                '    End If
                'Next


                'strsql = "delete from RFQ_INVITED_VENLIST " _
                '  & "where riv_rfq_id = '" & item.RFQ_ID & "' " _
                '  & " and RIV_S_Coy_ID in(" & str_vcom & ") and RIV_S_Coy_ID " _
                '  & "not in(select RTVDT_v_company_id from RFQ_INVITED_VENLIST_DETAIL " _
                '  & "where RTVDT_User_Id='" & Common.Parse(HttpContext.Current.Session("UserID")) & "' " _
                '  & "and RTVDT_RFQ_ID = '" & item.RFQ_ID & "' " _
                '  & " and RTVDT_RFQ_Name='" & Common.Parse(item.RFQ_Name) & "' " _
                '  & "and RTVDT_Distribution_List_Id <> '" & Common.Parse(item.dis_ID) & "')"
                'Common.Insert2Ary(strAryQuery, strsql)
                'strsql = "SELECT RIV_S_Coy_ID FROM RFQ_INVITED_VENLIST WHERE riv_rfq_id=" & rfq_id
                'Dim tDS As DataSet = objDb.FillDs(strsql)

                'For j = 0 To tDS.Tables(0).Rows.Count - 1
                '    strSearch = "CoyId='" & tDS.Tables(0).Rows(j).Item("RIV_S_Coy_ID") & "'"
                '    dtrDetails = dtDetails.Select(strSearch)
                '    If dtrDetails.Length < 1 Then
                '        strsql = "delete from dbo.RFQ_INVITED_VENLIST " _
                '            & "where riv_rfq_id = '" & item.RFQ_ID & "' " _
                '            & " and RIV_S_Coy_ID in(" & str_vcom & ") and RIV_S_Coy_ID " _
                '            & "not in(select RTVDT_v_company_id from RFQ_INVITED_VENLIST_DETAIL " _
                '            & "where RTVDT_User_Id='" & Common.Parse(HttpContext.Current.Session("UserID")) & "' " _
                '            & "and RTVDT_RFQ_ID = '" & item.RFQ_ID & "' " _
                '            & " and RTVDT_RFQ_Name='" & Common.Parse(item.RFQ_Name) & "' " _
                '            & "and RTVDT_Distribution_List_Id <> '" & Common.Parse(item.dis_ID) & "')"
                '    End If

                'Next




                'For Each drList In dtList.Rows
                '    If drList.Item("TYPE") = "list" Then
                '        Dim strsql1 As String = "delete from RFQ_INVITED_VENLIST_MSTR where RIVMT_User_ID='" & Common.Parse(HttpContext.Current.Session("UserID")) & "'" & _
                '                    " and RIVMT_RFQ_Name='" & Common.Parse(item.RFQ_Name) & "' and RIVMT_Distribution_list_id='" & Common.Parse(item.dis_ID) & "' " & _
                '                    " and RIVMT_RFQ_ID = " & item.RFQ_ID
                '        Dim strsql2 As String = "delete from RFQ_INVITED_VENLIST_DETAIL where RTVDT_User_Id='" & Common.Parse(HttpContext.Current.Session("UserID")) & "'" & _
                '        " and RTVDT_RFQ_Name= '" & Common.Parse(item.RFQ_Name) & "' and RTVDT_Distribution_List_Id='" & Common.Parse(item.dis_ID) & "' and RTVDT_RFQ_ID = '" & item.RFQ_ID & "' "
                '        Dim strsql3 As String = "delete from RFQ_INVITED_VENLIST where  RIV_RFQ_ID in(select RM_RFQ_ID from RFQ_MSTR WHERE RM_Coy_ID= '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'" & _
                '        " and  RM_RFQ_Name = '" & Common.Parse(item.RFQ_Name) & "' AND RM_Created_By='" & Common.Parse(HttpContext.Current.Session("UserID")) & "')"
                '        If item.V_com_ID <> "" Then
                '            strsql3 = strsql3 & " and (RIV_S_Coy_ID not in(" & item.V_com_ID & ") "
                '        End If
                '        If item.list_index <> "" Then
                '            strsql3 = strsql3 & "  and RIV_S_Coy_ID not in(select RCDLD_V_Coy_ID from RFQ_VEN_DISTR_LIST_DETAIL where RVDLD_LIST_INDEX in(" & item.list_index & "))"
                '        End If
                '        If item.V_com_ID <> "" Then
                '            strsql3 = strsql3 & ")"
                '        End If
                '        Common.Insert2Ary(strAryQuery, strsql1)
                '        Common.Insert2Ary(strAryQuery, strsql2)
                '        Common.Insert2Ary(strAryQuery, strsql3)
                '    ElseIf drList.Item("TYPE") = "specific" Then
                '        Dim strsql3 As String = "delete from RFQ_INVITED_VENLIST where  RIV_RFQ_ID in(select RM_RFQ_ID from RFQ_MSTR WHERE RM_Coy_ID= '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'" & _
                '        " and  RM_RFQ_Name = '" & Common.Parse(item.RFQ_Name) & "' AND RM_Created_By='" & Common.Parse(HttpContext.Current.Session("UserID")) & "')" & _
                '        " and RIV_S_Coy_ID='" & item.dis_ID & "'"
                '        Common.Insert2Ary(strAryQuery, strsql3)
                '    End If
                '    Common.Insert2Ary(strAryQuery, strsql)
                '    Common.Insert2Ary(strAryQuery, strsql)
                'Next


            End If
            'If strVendorList <> "" Then
            '    'Update the records in RFQ_INVITED_VENLIST by
            '    ' i) delete the records that user has mark for deletion
            '    ' ii) add the new records that user has selected
            '    If rfq_id = "" Then 'add the new records that user has selected
            '        strsql = "insert into RFQ_INVITED_VENLIST "
            '        strsql &= "select " & objDb.GetLatestInsertedID("RFQ_MSTR") & ", "
            '        strsql &= "CM.CM_COY_ID,CM.CM_COY_NAME,CM.CM_ADDR_LINE1,CM.CM_ADDR_LINE2,CM.CM_ADDR_LINE3,CM.CM_POSTCODE,CM.CM_CITY,CM.CM_STATE,CM.CM_COUNTRY,CM.CM_PHONE,CM.CM_FAX,CM.CM_EMAIL "
            '        strsql &= "FROM COMPANY_MSTR CM WHERE CM.CM_COY_ID IN (" & strVendorList & ")"
            '        Common.Insert2Ary(strAryQuery, strsql)
            '    Else 'ie Draft RFQ, delete the existing records and add in the new records
            '        strsql = "delete from RFQ_INVITED_VENLIST "
            '        strsql &= "where RIV_RFQ_ID = '" & rfq_id & "'"
            '        Common.Insert2Ary(strAryQuery, strsql)
            '        strsql = "insert into RFQ_INVITED_VENLIST "
            '        strsql &= "select " & rfq_id & ", "
            '        strsql &= "CM.CM_COY_ID,CM.CM_COY_NAME,CM.CM_ADDR_LINE1,CM.CM_ADDR_LINE2,CM.CM_ADDR_LINE3,CM.CM_POSTCODE,CM.CM_CITY,CM.CM_STATE,CM.CM_COUNTRY,CM.CM_PHONE,CM.CM_FAX,CM.CM_EMAIL "
            '        strsql &= "FROM COMPANY_MSTR CM WHERE CM.CM_COY_ID IN (" & strVendorList & ")"
            '        Common.Insert2Ary(strAryQuery, strsql)
            '    End If
            'End If

            strsql = "delete from RFQ_DETAIL "
            strsql &= "where RD_RFQ_ID = '" & rfq_id & "'"
            Common.Insert2Ary(strAryQuery, strsql)

            ' _Yap
            For i = 0 To dt.Rows.Count - 1
                strsql = "insert into RFQ_DETAIL (RD_RFQ_ID,RD_Coy_ID,RD_RFQ_Line,"
                strsql &= "RD_Vendor_Item_Code,RD_Quantity,RD_Product_Desc, RD_PRODUCT_CODE, "
                strsql &= "RD_UOM,RD_Delivery_Lead_Time,RD_Warranty_Terms, "
                strsql &= "RD_Product_Name,RD_PR_LINE_INDEX) VALUES ( "
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
                strsql &= "'" & Common.Parse(dt.Rows(i)("ProductDesc")) & "', "
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

        Function getAvailableVendor(ByVal PURCHASING_ROLE As Boolean) As String
            Dim sqlSrchVendor As String

            ' ai chu modified on 09/12/2005
            ' SR AS0048 - when buyer create RFQ and select vendor to send to, deactivated vendor company
            ' will not be able to see and serached by user
            If PURCHASING_ROLE Then
                '//remark by Moo
                'sqlSrchVendor = sqlSrchVendor & _
                '        " FROM COMPANY_MSTR" & _
                '        " WHERE CM_COY_ID in" & _
                '        "(SELECT distinct UM_COMPANY_ID FROM , USERS_USRGRP" & _
                '        " WHERE USERS_MSTR.UM_USER_ID = USERS_USRGRP.UU_USER_ID" & _
                '        " AND USERS_USRGRP.UU_USRGRP_ID like ('%Vendor%')"

                sqlSrchVendor = sqlSrchVendor & _
                     " FROM COMPANY_MSTR" & _
                     " WHERE CM_COY_TYPE IN ('VENDOR','BOTH') AND CM_DELETED='N' AND CM_STATUS='A'"
            Else
                'sqlSrchVendor = sqlSrchVendor & _
                '        " FROM COMPANY_MSTR, COMPANY_VENDOR" & _
                '        " WHERE COMPANY_MSTR.CM_COY_ID = COMPANY_VENDOR.CV_S_COY_ID" & _
                '        " AND COMPANY_VENDOR.CV_B_COY_ID = '" & common.parseNull (  HttpContext.Current.Session("CompanyId") ) & "'" & _
                '        " AND COMPANY_VENDOR.CV_S_COY_ID in" & _
                '        "(SELECT distinct UM_COMPANY_ID from USERS_MSTR, USERS_USRGRP" & _
                '        " WHERE USERS_MSTR.UM_COMPANY_ID = COMPANY_VENDOR.CV_S_COY_ID" & _
                '        " AND .UM_USER_ID = USERS_USRGRP.UU_USER_ID" & _
                '        " AND USERS_USRGRP.UU_USRGRP_ID like ('%Vendor%')) "
                sqlSrchVendor = sqlSrchVendor & _
                      " FROM COMPANY_MSTR, COMPANY_VENDOR" & _
                      " WHERE COMPANY_MSTR.CM_COY_ID = COMPANY_VENDOR.CV_S_COY_ID" & _
                      " AND COMPANY_VENDOR.CV_B_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
                      " AND CM_DELETED='N' AND CM_STATUS='A' "
            End If

            getAvailableVendor = sqlSrchVendor
        End Function

        Function getAvailableVendor_AZ(ByVal PURCHASING_ROLE As Boolean) As String
            Dim sqlSrchVendor As String

            sqlSrchVendor = sqlSrchVendor & _
                      " FROM COMPANY_MSTR, COMPANY_VENDOR" & _
                      " WHERE COMPANY_MSTR.CM_COY_ID = COMPANY_VENDOR.CV_S_COY_ID" & _
                      " AND COMPANY_VENDOR.CV_B_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
                      " AND CM_DELETED='N' AND CM_STATUS='A' "

            getAvailableVendor_AZ = sqlSrchVendor
        End Function

        ' ai chu remark on 25/01/2006
        ' not used in the system
        'Public Function vendor_check_list(ByVal item As RFQ_User)
        '    Dim read As OleDb.OleDbDataReader
        '    Dim strchk As String = "select RIVMT_Distribution_list_id from RFQ_INVITED_VENLIST_MSTR where RIVMT_Distribution_list_id='" & Common.Parse(item.dis_ID) & "'"
        '    read = objDb.GetReader(strchk)
        '    Do While read.Read
        '        If item.dis_ID = read("RIVMT_Distribution_list_id") Then
        '            Exit Function
        '        End If
        '    Loop
        '    read.Close()


        '    Dim strsql As String = "insert into RFQ_INVITED_VENLIST_MSTR(RIVMT_User_ID,RIVMT_RFQ_Name,RIVMT_Distribution_list_id,RIVMT_RFQ_ID)" & _
        '                                     "values ('" & Common.Parse(HttpContext.Current.Session("UserID")) & "','" & Common.Parse(item.RFQ_Name) & "','" & Common.Parse(item.dis_ID) & "','" & item.RFQ_ID & "')"
        '    objDb.Execute(strsql)

        'End Function

        Public Function Vendor_AddDistMstr(ByVal item As RFQ_User)

            'Dim i As Integer
            'Dim read As OleDb.OleDbDataReader
            'Dim strsql As String = "select count(*) as count from RFQ_VEN_DISTR_LIST_MSTR where RVDLM_User_Id='" & common.parseNull (  HttpContext.Current.Session("UserID") ) & "'and RVDLM_Coy_Id='" & common.parseNull (  HttpContext.Current.Session("CompanyId") ) & "'and RVDLM_List_Name='specific vendor' "
            'read = objDb.GetReader(strsql)

            'Do While read.Read
            '    If read("count") > 0 Then
            '        i = 1
            '    End If
            'Loop

            'If i = 1 Then
            '    Dim strsql2 As String = "insert into RFQ_VEN_DISTR_LIST_MSTR(RVDLM_User_Id,RVDLM_Coy_Id,RVDLM_List_Name)" & _
            '                            "values ('" & common.parseNull (  HttpContext.Current.Session("UserID") ) & "','" & common.parseNull (  HttpContext.Current.Session("CompanyId") ) & "','specific vendor')"
            '    objDb.Execute(strsql2)
            'End If

            'Dim read2 As OleDb.OleDbDataReader
            'Dim strsql3 As String = "select RVDLM_List_Index from RFQ_VEN_DISTR_LIST_MSTR where RVDLM_User_Id='" & common.parseNull (  HttpContext.Current.Session("UserID") ) & "'and RVDLM_Coy_Id='" & common.parseNull (  HttpContext.Current.Session("CompanyId") ) & "'and RVDLM_List_Name='specific vendor' "
            'read = objDb.GetReader(strsql3)

            'Do While read2.Read
            '    Vendor_AddDistMstr = read2("RVDLM_List_Index")
            'Loop

        End Function
        Public Function Vendor_send(ByVal prefix As String, ByRef rfq_num As String, ByVal RFQ_ID As String) As String

            Dim getRFQNum As String
            Dim strAryQuery(0) As String
            Dim strsql(2) As String
            Dim tDS As DataSet

            Dim strsql4 As String = "select RVM_V_RFQ_Status from RFQ_VENDOR_MSTR where " & _
                        " RVM_RFQ_ID='" & Common.Parse(RFQ_ID) & "'"
            tDS = objDb.FillDs(strsql4)
            If tDS.Tables(0).Rows.Count > 0 Then
                Vendor_send = "2"
                Exit Function

            End If
            strsql(0) = "update RFQ_MSTR SET RM_B_Display_Status='0',RM_Status='0' where RM_RFQ_ID= '" & Common.Parse(RFQ_ID) & "'"

            'strsql(1) = "insert into RFQ_DETAIL select riv.RIV_RFQ_ID,riv.RIV_S_Coy_ID,c.RCIT_Line_no," & _
            '"c.RCIT_Product_ID,c.RCIT_Product_Name,c.RCIT_Quantity,c.RCIT_Product_Desc," & _
            '"c.RCIT_UOM, c.RCIT_Delivery_Lead_Time, c.RCIT_Warranty_Terms,c.RCIT_Product_Name" & _
            '" from RFQ_CART_ITEM_TEMP c, RFQ_INVITED_VENLIST riv " & _
            '" where RCIT_RFQ_Name='" & common.parseNull (  item.RFQ_Name ) & "' and RCIT_Header_Ind <> '1' and RIV_RFQ_ID=(select RM_RFQ_ID FROM RFQ_MSTR WHERE RM_RFQ_Name= '" & common.parseNull (  item.RFQ_Name ) & "' )"

            Common.Insert2Ary(strAryQuery, strsql(0))
            'Common.Insert2Ary(strAryQuery, strsql(1))
            Dim str_rfqDetial As String = "SELECT RFQ_MSTR.RM_RFQ_ID AS ID,V.RIV_S_Coy_ID AS COMID,RFQ_MSTR.RM_RFQ_no " & _
                                        "FROM RFQ_MSTR,RFQ_INVITED_VENLIST V " & _
                                        "INNER JOIN COMPANY_MSTR ON V.RIV_S_Coy_ID = CM_COY_ID " & _
                                        "WHERE RFQ_MSTR.RM_RFQ_ID='" & Common.Parse(RFQ_ID) & "' AND V.RIV_RFQ_ID=RFQ_MSTR.RM_RFQ_ID AND CM_STATUS = 'A'"

            tDS = objDb.FillDs(str_rfqDetial)
            Dim strsql3 As String
            Dim count As Integer
            Dim i As Integer

            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                Dim S1 As String = tDS.Tables(0).Rows(j).Item("ID")
                Dim S2 As String = tDS.Tables(0).Rows(j).Item("COMID")
                getRFQNum = tDS.Tables(0).Rows(j).Item("RM_RFQ_no")
                i = i + 1
                If objDb.Exist("select '*' from RFQ_VENDOR_MSTR where RVM_V_Company_ID='" & Common.Parse(S2) & "' and RVM_RFQ_ID='" & Common.Parse(S1) & "' ") > 0 Then
                    count = count + 1
                    If Vendor_send = "" Then
                        Vendor_send = S2
                    Else
                        Vendor_send = Vendor_send & "," & S2
                    End If
                Else
                    strsql3 = "insert into RFQ_VENDOR_MSTR (RVM_RFQ_ID,RVM_V_Company_ID,RVM_V_RFQ_Status,RVM_V_Display_Status)Values('" & Common.Parse(S1) & "','" & Common.Parse(S2) & "','0','0' ) "
                    Common.Insert2Ary(strAryQuery, strsql3)
                End If
                If i = count Then
                    Exit Function
                End If
            Next

            Dim objUsers As New Users
            objUsers.Log_UserActivity(strAryQuery, WheelModule.RFQ, WheelUserActivity.B_SubmitRFQ, getRFQNum)
            objUsers = Nothing

            If objDb.BatchExecute(strAryQuery) Then
                Dim objMail As New Email
                Dim strMail As String
                strMail = "SELECT RM_RFQ_NO, RM_REQD_QUOTE_VALIDITY,RM_Expiry_Date, RIV_S_COY_ID FROM RFQ_MSTR "
                strMail &= "LEFT JOIN RFQ_INVITED_VENLIST ON RIV_RFQ_ID = RM_RFQ_ID "
                strMail &= "INNER JOIN COMPANY_MSTR ON RIV_S_COY_ID = CM_COY_ID "
                strMail &= "WHERE RM_RFQ_ID = '" & Common.Parse(RFQ_ID) & "' "
                strMail &= "AND CM_STATUS = 'A' "

                tDS = objDb.FillDs(strMail)
                For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1 'comfirm send mail
                    objMail.sendNotification(EmailType.RFQRequested, HttpContext.Current.Session("UserID"), HttpContext.Current.Session("CompanyID"), tDS.Tables(0).Rows(j).Item("RIV_S_COY_ID"), tDS.Tables(0).Rows(j).Item("RM_RFQ_NO"), Common.Parse(tDS.Tables(0).Rows(j).Item("RM_Expiry_Date")))
                Next
                objMail = Nothing
            End If
        End Function

        Public Function Vendor_AddListMstr(ByVal item As RFQ_User)
            Dim i As Integer
            Dim strsql As String
            strsql = "select count(*) as count from RFQ_INVITED_VENLIST_MSTR " & _
                    "where RIVMT_User_ID='" & Common.Parse(HttpContext.Current.Session("UserID")) & "' " & _
                    "and RIVMT_RFQ_Name='" & Common.Parse(item.RFQ_Name) & "' " & _
                    "and RIVMT_Distribution_list_id='" & Common.Parse(item.dis_ID) & "' " & _
                    "and RIVMT_RFQ_ID = " & item.RFQ_ID
            Dim tDS As DataSet = objDb.FillDs(strsql)

            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                If tDS.Tables(0).Rows(j).Item("count") = 1 Then
                    Exit Function
                End If
            Next
            Dim strsql2 As String = "insert into RFQ_INVITED_VENLIST_MSTR(RIVMT_User_ID,RIVMT_RFQ_Name,RIVMT_Distribution_list_id,RIVMT_RFQ_ID)" & _
                                    "values ('" & Common.Parse(HttpContext.Current.Session("UserID")) & "','" & Common.Parse(item.RFQ_Name) & "','" & Common.Parse(item.dis_ID) & "'," & item.RFQ_ID & ")"
            objDb.Execute(strsql2)
        End Function

        ' ai chu remark on 25/01/2006
        ' not used in the system
        'Public Function Vendor_getlist(ByVal item As RFQ_User)
        '    Dim strsql As String = "select from RFQ_INVITED_VENLIST_MSTR where " & _
        '                            "RIVMT_User_ID='" & Common.Parse(HttpContext.Current.Session("UserID")) & "' " & _
        '                            "and RIVMT_RFQ_Name='" & Common.Parse(item.RFQ_Name) & "' " & _
        '                            "and RIVMT_Distribution_list_id='" & Common.Parse(item.dis_ID) & "' " & _
        '                            "AND RIVMT_RFQ_ID = " & item.RFQ_ID

        'End Function

        Public Function Vendor_Add_Inv_Ven_List(ByVal item As RFQ_User) As String

            Dim strsql As String
            Dim strsql2 As String
            Dim tDS As DataSet

            If objDb.Exist("Select * From RFQ_INVITED_VENLIST Where RIV_RFQ_ID in(select RM_RFQ_ID from RFQ_MSTR where RM_RFQ_Name = '" & Common.Parse(item.RFQ_Name) & "' and  rm_rfq_no='" & HttpContext.Current.Session("rfq_num") & "' and RM_Coy_ID='" & HttpContext.Current.Session("CompanyID") & "'  )") > 0 Then

                strsql = "Select * From RFQ_INVITED_VENLIST_DETAIL "
                strsql &= "Where RTVDT_Distribution_List_Id='" & item.dis_ID & "' "
                strsql &= "and RTVDT_RFQ_Name =  '" & Common.Parse(item.RFQ_Name) & "' "
                strsql &= "and RTVDT_User_Id='" & HttpContext.Current.Session("UserId") & "' "
                strsql &= "and RTVDT_RFQ_ID = '" & item.RFQ_ID & "' "

                tDS = objDb.FillDs(strsql)
                For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                    If objDb.Exist("select '*' from RFQ_INVITED_VENLIST WHERE RIV_RFQ_ID in(select RM_RFQ_ID from RFQ_MSTR WHERE  rm_rfq_no='" & HttpContext.Current.Session("rfq_num") & "' and  RM_RFQ_Name='" & Common.Parse(tDS.Tables(0).Rows(j).Item("RTVDT_RFQ_Name")) & "' and RM_Coy_ID='" & HttpContext.Current.Session("CompanyID") & "')  and RIV_S_Coy_ID='" & tDS.Tables(0).Rows(j).Item("RTVDT_v_company_id").ToString.Trim & "' ") = 0 Then
                        strsql2 = "insert into RFQ_INVITED_VENLIST select distinct M.RM_RFQ_ID,CM.CM_COY_ID,CM.CM_COY_NAME,CM.CM_ADDR_LINE1," & _
                        " CM.CM_ADDR_LINE2,CM.CM_ADDR_LINE3,CM.CM_POSTCODE,CM.CM_CITY,CM.CM_STATE,CM.CM_COUNTRY,CM.CM_PHONE,CM.CM_FAX,CM.CM_EMAIL" & _
                                  " FROM RFQ_MSTR M,COMPANY_MSTR CM,RFQ_INVITED_VENLIST_DETAIL V" & _
                                  " WHERE M.RM_RFQ_Name = '" & Common.Parse(item.RFQ_Name) & "' AND V.RTVDT_RFQ_Name=M.RM_RFQ_Name  and CM.CM_COY_ID=V.RTVDT_v_company_id " & _
                                  " and  rm_rfq_no='" & HttpContext.Current.Session("rfq_num") & "' and  CM.CM_COY_ID='" & tDS.Tables(0).Rows(j).Item("RTVDT_v_company_id").ToString.Trim & "' and RM_Coy_ID='" & HttpContext.Current.Session("CompanyId") & "'" & _
                                   " and RTVDT_Distribution_List_Id='" & item.dis_ID & "' and RTVDT_RFQ_ID = '" & item.RFQ_ID & "' and M.RM_RFQ_ID = V.RTVDT_RFQ_ID "
                        objDb.Execute(strsql2)

                    End If
                Next
            Else
                strsql2 = "insert into RFQ_INVITED_VENLIST select distinct M.RM_RFQ_ID,CM.CM_COY_ID,CM.CM_COY_NAME,CM.CM_ADDR_LINE1,CM.CM_ADDR_LINE2,CM.CM_ADDR_LINE3,CM.CM_POSTCODE,CM.CM_CITY,CM.CM_STATE,CM.CM_COUNTRY,CM.CM_PHONE,CM.CM_FAX,CM.CM_EMAIL" & _
                                             " FROM RFQ_MSTR M,COMPANY_MSTR CM,RFQ_INVITED_VENLIST_DETAIL V" & _
                                             " WHERE M.RM_RFQ_Name = '" & Common.Parse(item.RFQ_Name) & "' AND V.RTVDT_RFQ_Name= '" & Common.Parse(item.RFQ_Name) & "'and CM.CM_COY_ID=V.RTVDT_v_company_id" & _
                                             " and RM_Coy_ID='" & HttpContext.Current.Session("CompanyId") & "' and rm_rfq_no='" & HttpContext.Current.Session("rfq_num") & "'" & _
                                             " and RTVDT_Distribution_List_Id='" & item.dis_ID & "' and RTVDT_RFQ_ID = '" & item.RFQ_ID & "' and M.RM_RFQ_ID = V.RTVDT_RFQ_ID "
                objDb.Execute(strsql2)
            End If

        End Function

        'Public Function Vendor_Add_Inv_Ven_List2(ByVal item As RFQ_User) As String
        '    Dim strsql As String
        '    Dim strsql2 As String

        '    If objDb.Exist("Select * From RFQ_INVITED_VENLIST Where RIV_RFQ_ID in(select RM_RFQ_ID from RFQ_MSTR where rm_rfq_no='" & HttpContext.Current.Session("rfq_num") & "' and  RM_RFQ_Name = '" & Common.Parse(item.RFQ_Name) & "' and RM_Coy_ID='" & HttpContext.Current.Session("CompanyId") & "') and RIV_S_Coy_ID='" & Common.Parse(item.V_com_ID) & "' ") > 0 Then
        '        Vendor_Add_Inv_Ven_List2 = "Record exist!!!"
        '    Else
        '        Vendor_Add_Inv_Ven_List2 = "insert into RFQ_INVITED_VENLIST select M.RM_RFQ_ID,CM.CM_COY_ID,CM.CM_COY_NAME,CM.CM_ADDR_LINE1,CM.CM_ADDR_LINE2,CM.CM_ADDR_LINE3,CM.CM_POSTCODE,CM.CM_CITY,CM.CM_STATE,CM.CM_COUNTRY,CM.CM_PHONE,CM.CM_FAX,CM.CM_EMAIL" & _
        '                                     " FROM RFQ_MSTR M,COMPANY_MSTR CM" & _
        '                                     " WHERE M.rm_rfq_no='" & HttpContext.Current.Session("rfq_num") & "' and M.RM_RFQ_Name = '" & Common.Parse(item.RFQ_Name) & "' and RM_Coy_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'and CM.CM_COY_ID='" & Common.Parse(item.V_com_ID) & "'"

        '    End If

        'End Function
        'Michelle (1/12/2010) - To add records to RFQ_INVITED_VENLIST_TEMP, replace the Vendor_Add_Inv_Ven_List2 function
        Public Function Vendor_Add_Inv_Ven_List2_TEMP(ByVal item As RFQ_User, ByVal action As String) As String
            Vendor_Add_Inv_Ven_List2_TEMP = "insert into RFQ_INVITED_VENLIST_TEMP (RIV_USER_ID, RIV_S_COY_ID, RIV_S_COY_NAME, RIV_ACTION)" & _
                                          " VALUES ('" & HttpContext.Current.Session("UserID") & "', '" & Common.Parse(item.V_com_ID) & "', '" & Common.Parse(item.V_Com_Name) & "', '" & Common.Parse(action) & "')"
        End Function

        Public Function Vendor_Upd_Inv_Ven_List2_TEMP(ByVal item As RFQ_User, ByVal action As String) As String
            Dim strsql As String
            Dim query(0) As String

            If Common.Parse(action) = "D" Then
                If Not objDb.Exist("select * from RFQ_INVITED_VENLIST_TEMP wHERE RIV_USER_ID = '" & HttpContext.Current.Session("UserID") & "' And RIV_S_COY_ID = '" & Common.Parse(item.V_com_ID) & "'") > 0 Then
                    strsql = " INSERT INTO RFQ_INVITED_VENLIST_TEMP (RIV_USER_ID, RIV_S_COY_ID, RIV_S_COY_NAME, RIV_ACTION) " & _
                             " VALUES ('" & HttpContext.Current.Session("UserID") & "', '" & Common.Parse(item.V_com_ID) & "', '" & Common.Parse(item.V_Com_Name) & "', 'A')"
                    objDb.Execute(strsql)
                End If
            End If
            

            strsql = " Update RFQ_INVITED_VENLIST_TEMP" & _
                     " Set RIV_ACTION = '" & Common.Parse(action) & "'" & _
                     " where RIV_USER_ID = '" & HttpContext.Current.Session("UserID") & "' And RIV_S_COY_ID = '" & Common.Parse(item.V_com_ID) & "'"

            Common.Insert2Ary(query, strsql)
            objDb.BatchExecute(query)
        End Function

        'Michelle (1/12/2010) - To delete records from RFQ_INVITED_VENLIST_TEMP once user go in to the Raise RFQ page
        Public Function delete_TempVenListnItem() As String
            Dim strsql As String
            Dim query(0) As String
            strsql = "Delete from RFQ_INVITED_VENLIST_TEMP" & _
                   " where RIV_USER_ID = '" & HttpContext.Current.Session("UserID") & "'"
            Common.Insert2Ary(query, strsql)
            strsql = "Delete from RFQ_DETAIL_TEMP" & _
                   " where RD_USER_ID = '" & HttpContext.Current.Session("UserID") & "'"
            Common.Insert2Ary(query, strsql)
            strsql = "Delete from COMPANY_DOC_ATTACHMENT" & _
                   " where CDA_DOC_NO = '" & HttpContext.Current.Session.SessionID & "'  AND CDA_DOC_TYPE = 'RFQ' "
            Common.Insert2Ary(query, strsql)
            objDb.BatchExecute(query)
        End Function

        Public Function checkItemDeleted(ByVal rfqId As String) As Boolean
            Dim strsql As String
            ' ------- check item not deleted from List Price Catalogue ----------
            strsql = "SELECT '*' FROM RFQ_DETAIL "
            strsql &= "WHERE RD_RFQ_ID = '" & rfqId & "' "
            strsql &= "AND RD_Product_Code IN ("
            strsql &= "SELECT PM_PRODUCT_CODE FROM PRODUCT_MSTR "
            strsql &= "WHERE PM_DELETED = 'Y') "
            If objDb.Exist(strsql) > 0 Then
                checkItemDeleted = True
            Else
                checkItemDeleted = False
            End If
            ' -------------------------------------------------------------------
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
            " RM_Payment_Term,RM_Payment_Type,RM_Shipment_Term,RM_Shipment_Mode,RM_Prefix,RM_B_Display_Status,RM_Reqd_Quote_Validity,RM_Contact_Person,RM_Contact_Number,RM_Email,RM_RFQ_OPTION,RM_VEN_DISTR_LIST_INDEX )" & _
            "SELECT RM_Coy_ID,'" & rfq_no & " ','" & Replace(rfq_name, "'", "''") & "'," & Common.ConvertDate(Now.Today) & ",'" & rm_status & "',RM_Remark,RM_Created_By," & Common.ConvertDate(Now.Today) & ",RM_Currency_Code,RM_Payment_Term,RM_Payment_Type,RM_Shipment_Term,RM_Shipment_Mode, " & _
            " RM_Prefix,'" & b_disStatus & "' ," & Common.ConvertDate(Now.Today.AddDays(1)) & ",RM_Contact_Person,RM_Contact_Number,RM_Email,RM_RFQ_OPTION,RM_VEN_DISTR_LIST_INDEX " & _
            "  FROM RFQ_MSTR WHERE RM_RFQ_ID='" & rfq_id & "'"
            'RM_RFQ_ID,RM_Coy_ID,RM_RFQ_No,RM_RFQ_Name,RM_Expiry_Date,RM_Status,RM_Remark,RM_Created_By,RM_Created_On,RM_Currency_Code,RM_Payment_Term,RM_Payment_Type,RM_Shipment_Term,RM_Shipment_Mode,RM_Prefix,RM_B_Display_Status,RM_Reqd_Quote_Validity,RM_Contact_Person,RM_Contact_Number,RM_Email,RM_RFQ_OPTION,RM_VEN_DISTR_LIST_INDEX
            Common.Insert2Ary(Array, strsql)

            strsql = "insert into RFQ_DETAIL ( RD_RFQ_ID,RD_Coy_ID,RD_RFQ_Line,RD_Product_Code,RD_Vendor_Item_Code,RD_Quantity,RD_Product_Desc,RD_UOM,RD_Delivery_Lead_Time,RD_Warranty_Terms,RD_Product_Name)" & _
            "SELECT '" & newRFqId & "',RD_Coy_ID,RD_RFQ_Line,RD_Product_Code,RD_Vendor_Item_Code,RD_Quantity,RD_Product_Desc,RD_UOM,RD_Delivery_Lead_Time,RD_Warranty_Terms,RD_Product_Name " & _
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

        Public Function rfqname(ByVal rfq_id As String) As String
            Dim strsql As String = "select RM_RFQ_NAME FROM RFQ_MSTR WHERE RM_RFQ_ID='" & rfq_id & "'"
            rfqname = objDb.GetVal(strsql)

        End Function
        Public Function get_docNUm(ByVal RFQ_ID As String) As String
            Dim strsQL As String = "select RM_RFQ_NO from RFQ_MSTR where RM_RFQ_ID ='" & RFQ_ID & "'"
            get_docNUm = objDb.GetVal(strsQL)

        End Function

        ' ai chu modified on 25/01/2006 
        ' no need to pass the rfq_name parameter
        'Public Function get_rfqid(ByVal rfq_name As String) As String
        Public Function get_rfqid() As String
            Dim strsQL As String = "select max(RM_RFQ_ID) from RFQ_MSTR"

            get_rfqid = objDb.GetVal(strsQL) + 1
        End Function

        Public Function get_rfqName(ByVal rfq_name As String) As Boolean
            If objDb.Exist("select '*' from RFQ_MSTR WHERE RM_RFQ_Name='" & Replace(rfq_name, "'", "''") & "' AND RM_Coy_ID='" & HttpContext.Current.Session("CompanyID") & "'") = 0 Then
                get_rfqName = True
            End If
        End Function

        Public Function Vendor_remove_inv(ByVal item As RFQ_User, ByVal str_vcom As String) As String

            Dim strsql As String = "delete from dbo.RFQ_INVITED_VENLIST where riv_rfq_id = '" & item.RFQ_ID & "'" & _
                                    " and RIV_S_Coy_ID in(" & str_vcom & ") and RIV_S_Coy_ID not in(select RTVDT_v_company_id from RFQ_INVITED_VENLIST_DETAIL " & _
                                    " where RTVDT_User_Id='" & Common.Parse(HttpContext.Current.Session("UserID")) & "' and RTVDT_RFQ_ID = '" & item.RFQ_ID & "' " & _
                                    " and RTVDT_RFQ_Name='" & Common.Parse(item.RFQ_Name) & "' and RTVDT_Distribution_List_Id <> '" & Common.Parse(item.dis_ID) & "')"
            Return strsql
        End Function
        Public Function Vendor_deleteList(ByVal item As RFQ_User) As String
            Dim strsql As String = "delete from RFQ_INVITED_VENLIST_DETAIL where" & _
                                           " RTVDT_User_Id='" & Common.Parse(HttpContext.Current.Session("UserID")) & "' and RTVDT_RFQ_Name='" & Common.Parse(item.RFQ_Name) & "' and RTVDT_Distribution_List_Id='" & Common.Parse(item.dis_ID) & "' and RTVDT_v_company_id = '" & Common.Parse(item.V_com_ID) & "' and RTVDT_RFQ_ID = '" & item.RFQ_ID & "' "

            Vendor_deleteList = strsql

        End Function

        Public Function Vendor_CheckList(ByVal item As RFQ_User)

            Dim strsql, strsql2 As String
            strsql = "select count(RTVDT_User_Id) as count from RFQ_INVITED_VENLIST_DETAIL where" & _
                    " RTVDT_User_Id='" & Common.Parse(HttpContext.Current.Session("UserID")) & "' " & _
                    " and RTVDT_Distribution_List_Id='" & Common.Parse(item.dis_ID) & "' " & _
                    " and RTVDT_RFQ_Name ='" & Common.Parse(item.RFQ_Name) & "' " & _
                    " and RTVDT_RFQ_ID = '" & item.RFQ_ID & "' "

            Dim tDS As DataSet = objDb.FillDs(strsql)

            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                If tDS.Tables(0).Rows(j).Item("count") = 0 Then
                    strsql2 = "delete from RFQ_INVITED_VENLIST_MSTR " & _
                            "where RIVMT_Distribution_list_id='" & Common.Parse(item.dis_ID) & "' " & _
                            " and RIVMT_User_ID= '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " & _
                            " and RIVMT_RFQ_Name= '" & Common.Parse(item.RFQ_Name) & "' " & _
                            " and RIVMT_RFQ_ID = " & item.RFQ_ID
                    objDb.Execute(strsql2)
                End If
            Next
        End Function

        Public Function Vendor_add2trash(ByVal item As RFQ_User, ByVal folder As String, Optional ByVal DOCTYPE As String = "") As String
            Dim strsql As String

            If folder = "0" Then 'RVM_V_Display_Status "SENT FOLDER"
                strsql = "update RFQ_VENDOR_MSTR set RVM_V_Display_Status= '" & Common.Parse(VendorDisStatus.trash) & "' where RVM_RFQ_ID='" & Common.Parse(item.RFQ_ID) & "' and RVM_V_Company_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "
            ElseIf folder = "1" Then ' QOUTE FOLDER 
                strsql = "update RFQ_REPLIES_MSTR set RRM_V_Display_Status='" & Common.Parse(VendorDisStatus.trash) & "' where RRM_RFQ_ID='" & Common.Parse(item.RFQ_ID) & "' and  RRM_V_Company_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'"
            ElseIf folder = "2" Then ' TRASH FOLDER
                If DOCTYPE = "QOUTE" Then 'WHEN DOC IS qoute
                    strsql = "update RFQ_REPLIES_MSTR set RRM_V_Display_Status='" & Common.Parse(VendorDisStatus.Deletetrash) & "'where RRM_RFQ_ID='" & Common.Parse(item.RFQ_ID) & "' and  RRM_V_Company_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'"
                ElseIf DOCTYPE = "RFQ" Then 'WHEN DOC IS RFQ
                    strsql = "update RFQ_VENDOR_MSTR set RVM_V_Display_Status= '" & Common.Parse(VendorDisStatus.Deletetrash) & "' where RVM_RFQ_ID='" & Common.Parse(item.RFQ_ID) & "' and RVM_V_Company_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "
                End If

            End If
            Vendor_add2trash = strsql

        End Function

        Public Function Vendor_AddList(ByVal item As RFQ_User) As String

            Dim strsql As String = "select count(RTVDT_User_Id) as count from RFQ_INVITED_VENLIST_DETAIL where" & _
                                           " RTVDT_User_Id='" & Common.Parse(HttpContext.Current.Session("UserID")) & "' and RTVDT_RFQ_Name='" & Common.Parse(item.RFQ_Name) & "' and RTVDT_v_company_id = '" & Common.Parse(item.V_com_ID) & "' and RTVDT_RFQ_ID = '" & item.RFQ_ID & "' "
            'and RTVDT_Distribution_List_Id='" & Common.Parse(item.dis_ID) & "' 
            Dim tDS As DataSet = objDb.FillDs(strsql)

            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                If tDS.Tables(0).Rows(j).Item("count") = 1 Then
                    Vendor_AddList = "Record exist!!!"
                    Exit Function
                End If
            Next
            Dim strsql2 As String = "insert into RFQ_INVITED_VENLIST_DETAIL(RTVDT_User_Id,RTVDT_RFQ_Name,RTVDT_Distribution_List_Id,RTVDT_v_company_id,RTVDT_RFQ_ID)" & _
                                    "values ('" & Common.Parse(HttpContext.Current.Session("UserID")) & "','" & Common.Parse(item.RFQ_Name) & "','" & Common.Parse(item.dis_ID) & "','" & Common.Parse(item.V_com_ID) & "','" & item.RFQ_ID & "')"


            Vendor_AddList = strsql2
        End Function

        Public Function GetPredefinedVendor() As DataSet
            Dim ds As DataSet
            Dim strsql As String = "SELECT RVDLM_List_Index,RVDLM_List_Name,RVDLM_User_Id,RVDLM_Coy_Id " _
                            & "FROM RFQ_VEN_DISTR_LIST_MSTR " _
                            & "WHERE RVDLM_Coy_Id='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                            & "AND RVDLM_USER_ID='" & Common.Parse(HttpContext.Current.Session("UserID")) & "'"
            ds = objDb.FillDs(strsql)
            Return ds
        End Function
#End Region

#Region "vendor RFQ"

        Public Function get_qoute1(ByVal item As RFQ_User, ByVal rfq_id As String, ByVal vcomid As String)
            'Dim strsql As String = "select RFQ_REPLIES_MSTR.*,CM_BUSINESS_REG_NO,CM_COY_NAME,CM_STATE,CM_COUNTRY, " & _
            '    "COMPANY_MSTR.CM_ADDR_LINE1,COMPANY_MSTR.CM_ADDR_LINE2,CM_CITY,CM_ADDR_LINE3,CM_POSTCODE " & _
            '" from RFQ_REPLIES_MSTR,COMPANY_MSTR " & _
            '         "  where CM_COY_ID=RRM_V_Company_ID AND RRM_RFQ_ID='" & Common.Parse(rfq_id) & "' and " & _
            '" RRM_V_Company_ID ='" & vcomid & "'"
            Dim strsql As String = "SELECT RFQ_REPLIES_MSTR.*,RM_RFQ_No,RM_COY_ID,CM_BUSINESS_REG_NO,CM_COY_NAME,CM_STATE,CM_COUNTRY, " _
                            & "COMPANY_MSTR.CM_ADDR_LINE1,COMPANY_MSTR.CM_ADDR_LINE2, CM_CITY, CM_ADDR_LINE3, CM_POSTCODE " _
                            & "FROM RFQ_REPLIES_MSTR, COMPANY_MSTR, RFQ_MSTR " _
                            & "WHERE CM_COY_ID=RRM_V_Company_ID AND RFQ_REPLIES_MSTR.RRM_RFQ_ID=RFQ_MSTR.RM_RFQ_ID " _
                            & "AND RRM_RFQ_ID='" & Common.Parse(rfq_id) & "' AND RRM_V_Company_ID ='" & vcomid & "'"
            Dim tDS As DataSet = objDb.FillDs(strsql)

            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1

                item.con_person = tDS.Tables(0).Rows(j).Item("RRM_Contact_Person").ToString.Trim
                item.phone = tDS.Tables(0).Rows(j).Item("RRM_Contact_Number").ToString.Trim
                item.create_on = tDS.Tables(0).Rows(j).Item("RRM_Created_On").ToString.Trim
                item.email = tDS.Tables(0).Rows(j).Item("RRM_Email").ToString.Trim
                item.validaty = tDS.Tables(0).Rows(j).Item("RRM_Offer_Till").ToString.Trim
                item.pay_term = tDS.Tables(0).Rows(j).Item("RRM_Pay_Term_Code").ToString.Trim
                item.pay_type = tDS.Tables(0).Rows(j).Item("RRM_Payment_Type").ToString.Trim
                item.ship_term = tDS.Tables(0).Rows(j).Item("RRM_Ship_Term").ToString.Trim
                item.ship_mode = tDS.Tables(0).Rows(j).Item("RRM_Ship_Mode").ToString.Trim
                item.remark = tDS.Tables(0).Rows(j).Item("RRM_Remarks").ToString.Trim
                item.cur_code = tDS.Tables(0).Rows(j).Item("RRM_Currency_Code").ToString.Trim
                item.vendor_person = tDS.Tables(0).Rows(j).Item("RRM_Contact_Person").ToString.Trim
                item.addsline1 = tDS.Tables(0).Rows(j).Item("CM_ADDR_LINE1").ToString.Trim
                item.addsline2 = tDS.Tables(0).Rows(j).Item("CM_ADDR_LINE2").ToString.Trim
                item.addsline3 = tDS.Tables(0).Rows(j).Item("CM_ADDR_LINE3").ToString.Trim
                item.postcode = tDS.Tables(0).Rows(j).Item("CM_POSTCODE").ToString.Trim
                item.city = tDS.Tables(0).Rows(j).Item("CM_CITY").ToString.Trim
                item.REG_NO = tDS.Tables(0).Rows(j).Item("CM_BUSINESS_REG_NO").ToString.Trim
                item.V_Com_Name = tDS.Tables(0).Rows(j).Item("CM_COY_NAME").ToString.Trim
                item.qoute_valDate = tDS.Tables(0).Rows(j).Item("RRM_Offer_Till").ToString.Trim
                item.remark = tDS.Tables(0).Rows(j).Item("RRM_Remarks").ToString.Trim
                item.quo_num = tDS.Tables(0).Rows(j).Item("RRM_Actual_Quot_Num").ToString.Trim
                item.RFQ_Num = tDS.Tables(0).Rows(j).Item("RM_RFQ_No").ToString.Trim
                item.bcom_id = tDS.Tables(0).Rows(j).Item("RM_COY_ID").ToString.Trim
                item.V_com_ID = tDS.Tables(0).Rows(j).Item("RRM_V_Company_ID").ToString.Trim
                item.country = tDS.Tables(0).Rows(j).Item("CM_COUNTRY").ToString.Trim
                item.state = tDS.Tables(0).Rows(j).Item("CM_STATE").ToString.Trim
            Next
        End Function

        Public Function rfqnum(ByVal rfqid As String) As String
            Dim strsql As String = "select RM_RFQ_NO FROM RFQ_MSTR WHERE RM_RFQ_ID = '" & rfqid & "'"
            rfqnum = objDb.GetVal(strsql)
        End Function

        Public Function get_quotation(ByVal RFQ_ID As String) As DataSet

            Dim strsql As String
            '//Remark by Moo (25/10/2005), No need to checking, checking done at get_rfqmstr function
            'If HttpContext.Current.Session("edit") = "1" Then
            strsql = "select * from RFQ_REPLIES_DETAIL_TEMP where RRDT_RFQ_ID='" & Common.Parse(RFQ_ID) & "' and RRDT_V_Company_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' order by RRDT_Line_No "
            'ElseIf HttpContext.Current.Session("edit") = "0" Then
            '    strsql = "select * from RFQ_DETAIL,RFQ_INVITED_VENLIST where RD_RFQ_ID=RIV_RFQ_ID and RIV_RFQ_ID='" & Common.Parse(RFQ_ID) & "'  and RIV_S_Coy_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' order by RD_RFQ_Line"
            'End If

            Dim dscath As New DataSet
            dscath = objDb.FillDs(strsql)
            get_quotation = dscath

        End Function

        Public Function get_quotation2(ByVal RFQ_ID As String, ByVal vcomid As String) As DataSet
            Dim strsql, bcomid As String
            Dim dscath As New DataSet

            strsql = "SELECT RM_COY_ID FROM RFQ_MSTR WHERE RM_RFQ_ID = '" & RFQ_ID & "'"
            bcomid = objDb.GetVal(strsql)

            'strsql= "select * from RFQ_REPLIES_DETAIL where RRD_RFQ_ID='" & Common.Parse(RFQ_ID) & "' and RRD_V_Coy_ID='" & vcomid & "'"
            strsql = "SELECT RFQ_REPLIES_DETAIL.*, " &
                    "IFNULL(IF((TAX_PERC = '' OR TAX_PERC IS NULL), CODE_DESC, CONCAT(CODE_DESC, ' (', TAX_PERC, '%)')),'N/A') AS GSTRATE, IFNULL(TAX_PERC,0) AS TAX_PERC " &
                    "FROM RFQ_REPLIES_DETAIL " &
                    "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'SST' AND CODE_ABBR = RRD_GST_RATE " &
                    "LEFT JOIN COMPANY_MSTR ON CM_COY_ID = '" & bcomid & "' " &
                    "LEFT JOIN TAX ON TAX_CODE = CODE_ABBR AND TAX_COUNTRY_CODE = CM_COUNTRY " &
                    "WHERE RRD_RFQ_ID= '" & RFQ_ID & "' AND RRD_V_Coy_ID= '" & vcomid & "'"

            dscath = objDb.FillDs(strsql)
            get_quotation2 = dscath

        End Function

        'Public Function add_mstrreplied(ByVal item As RFQ_User, Optional ByVal EDIT As String = "0", Optional ByRef Quotation_num As String = "") As String
        '    Dim blnGetNewNo As Boolean = True
        '    Dim strAryQuery(0) As String
        '    Dim strdelete As String
        '    Dim strsql(4) As String
        '    Dim prefix, strNewNo As String
        '    Dim objglb As New AppGlobals

        '    Do While blnGetNewNo
        '        If EDIT = "1" Then
        '            strsql(0) = "update RFQ_REPLIES_MSTR set RRM_Currency_Code='" & Common.Parse(item.cur_code) & "',RRM_Offer_Till= " & Common.ConvertDate(item.validaty) & ",RRM_Remarks='" & Common.Parse(item.remark) & "'" & _
        '            " ,RRM_Pay_Term_Code='" & Common.Parse(item.pay_term) & "',RRM_Payment_Type='" & Common.Parse(item.pay_type) & "',RRM_Ship_Term=" & Common.Parse(item.ship_term) & ",RRM_Ship_Mode=" & Common.Parse(item.ship_mode) & "," & _
        '            " RRM_Contact_Person= '" & Common.Parse(item.con_person) & "',RRM_Contact_Number='" & Common.Parse(item.phone) & "',RRM_Email=' " & item.email & "',RRM_TotalValue=" & item.total & ",RRM_Indicator='" & Common.Parse(item.indicat) & "'" & _
        '            " where RRM_RFQ_ID='" & Common.Parse(item.RFQ_ID) & "' and RRM_V_Company_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "
        '            Common.Insert2Ary(strAryQuery, strsql(0))
        '            strdelete = "delete from RFQ_REPLIES_DETAIL where RRD_RFQ_ID='" & Common.Parse(item.RFQ_ID) & "' and RRD_V_Coy_Id='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'"
        '            strsql(1) = " insert into RFQ_REPLIES_DETAIL select * from RFQ_REPLIES_DETAIL_TEMP where RRDT_RFQ_ID='" & Common.Parse(item.RFQ_ID) & "' and RRDT_V_Company_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "
        '            Common.Insert2Ary(strAryQuery, strdelete)
        '            Common.Insert2Ary(strAryQuery, strsql(1))
        '            Dim strsql33 As String = "select RRM_Actual_Quot_Num from RFQ_REPLIES_MSTR WHERE RRM_RFQ_ID='" & Common.Parse(item.RFQ_ID) & "'and RRM_V_Company_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'"
        '            Quotation_num = objDb.GetVal(strsql33)
        '        ElseIf EDIT = "0" Then
        '            Dim strQuote As String
        '            objglb.GetLatestDocNo("Quotation", strAryQuery, Quotation_num, prefix, , , strNewNo)
        '            If get_quote(Quotation_num) = True Then
        '                add_mstrreplied = "1"
        '                Exit Function
        '            End If

        '            If get_rfqQuote(item.RFQ_ID, strQuote) = True Then
        '                Quotation_num = strQuote
        '                add_mstrreplied = "2"
        '                Exit Function
        '            End If

        '            strsql(0) = "insert into RFQ_REPLIES_MSTR (RRM_RFQ_ID,RRM_V_Company_ID,RRM_Currency_Code,RRM_Offer_Till," & _
        '                       "RRM_Remarks,RRM_Pay_Term_Code,RRM_Payment_Type,RRM_Ship_Mode,RRM_Ship_Term,RRM_Created_On,RRM_Created_By," & _
        '                       "RRM_Contact_Person,RRM_Contact_Number,RRM_Email,RRM_Status,RRM_B_Display_Status,RRM_V_Display_Status,RRM_Indicator,RRM_TotalValue,RRM_Actual_Quot_Num,RRM_Quot_Prefix) values " & _
        '                       "('" & Common.Parse(item.RFQ_ID) & "','" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "','" & Common.Parse(item.cur_code) & "'," & Common.ConvertDate(item.validaty) & "," & _
        '                       "'" & Common.Parse(item.remark) & "','" & Common.Parse(item.pay_term) & "','" & Common.Parse(item.pay_type) & "'," & Common.Parse(item.ship_mode) & "," & Common.Parse(item.ship_term) & "," & Common.ConvertDate(item.create_on) & ",'" & Common.Parse(HttpContext.Current.Session("UserId")) & "'," & _
        '                       "'" & Common.Parse(item.con_person) & "','" & Common.Parse(item.phone) & "',' " & item.email & " ','0','0','0','" & Common.Parse(item.indicat) & "'," & item.total & ",'" & Common.Parse(Quotation_num) & "','" & Common.Parse(prefix) & "')"
        '            Common.Insert2Ary(strAryQuery, strsql(0))
        '            strsql(1) = " insert into RFQ_REPLIES_DETAIL select * from RFQ_REPLIES_DETAIL_TEMP where RRDT_RFQ_ID='" & Common.Parse(item.RFQ_ID) & "' and RRDT_V_Company_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "
        '            Common.Insert2Ary(strAryQuery, strsql(1))
        '        End If

        '        strsql(2) = "update RFQ_VENDOR_MSTR set RVM_V_RFQ_Status='1' where RVM_V_Company_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' and RVM_RFQ_ID='" & Common.Parse(item.RFQ_ID) & "'"
        '        Common.Insert2Ary(strAryQuery, strsql(2))
        '        strsql(3) = "update RFQ_MSTR set RM_Status='1' WHERE RM_RFQ_ID='" & Common.Parse(item.RFQ_ID) & "' "
        '        Common.Insert2Ary(strAryQuery, strsql(3))

        '        ' ai chu add on 26/10/2005
        '        ' since during Step 1 to Step 3, all transaction related to attachment is saved as 'QuotTemp'
        '        ' is user submit the quotation, need to delete attachment with doc_type = 'Quotation' 
        '        ' then update COMPANY_DOC_ATTACHMENT table CDA_DOC_TYPE column from 'QuotTemp' to 'Quotation'
        '        ' for respective RFQ no
        '        'Modified by Craven 25/04/2011 for saving more then 1 attachment purpose
        '        'strsql(4) = "DELETE FROM COMPANY_DOC_ATTACHMENT "
        '        'strsql(4) &= "WHERE CDA_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "
        '        'strsql(4) &= "AND CDA_DOC_NO = '" & Common.Parse(item.RFQ_Num) & "' "
        '        'strsql(4) &= "AND CDA_DOC_TYPE = 'Quotation'"
        '        'Common.Insert2Ary(strAryQuery, strsql(4))

        '        strsql(4) = "UPDATE COMPANY_DOC_ATTACHMENT "
        '        strsql(4) &= "SET CDA_DOC_TYPE = 'Quotation' "
        '        strsql(4) &= "WHERE CDA_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "
        '        strsql(4) &= "AND CDA_DOC_NO = '" & Common.Parse(item.RFQ_Num) & "' "
        '        strsql(4) &= "AND CDA_DOC_TYPE = 'QuotTemp'"
        '        Common.Insert2Ary(strAryQuery, strsql(4))

        '        Dim objUsers As New Users
        '        objUsers.Log_UserActivity(strAryQuery, WheelModule.RFQ, WheelUserActivity.V_SubmitQuotation, Quotation_num, item.RFQ_ID)
        '        objUsers = Nothing

        '        'Michelle (2/11/2011) - check whether the new document no. has been taken
        '        If EDIT = "0" Then
        '            Dim strLastNo As String
        '            strLastNo = objDb.GetVal("SELECT CP_PARAM_VALUE FROM company_param WHERE cp_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND cp_param_name = 'Last Used No' AND cp_param_type = 'Quotation'")
        '            If Convert.ToInt64(strNewNo) <= Convert.ToInt64(strLastNo) Then
        '                ReDim strAryQuery(0)
        '                strdelete = ""
        '                ReDim strsql(4)
        '                prefix = ""
        '                strNewNo = ""
        '                blnGetNewNo = True
        '            Else
        '                blnGetNewNo = False
        '            End If
        '        Else
        '            blnGetNewNo = False
        '        End If
        '    Loop

        '    If objDb.BatchExecute(strAryQuery) Then
        '        add_mstrreplied = "4"
        '        Dim objMail As New Email
        '        Dim strMail As String

        '        strMail = "SELECT rm_rfq_no, RRM_Actual_Quot_Num, RRM_Offer_Till, RM_Coy_ID FROM RFQ_MSTR,RFQ_REPLIES_MSTR  "
        '        strMail &= " where RM_RFQ_ID='" & Common.Parse(item.RFQ_ID) & "' and RRM_RFQ_ID ='" & Common.Parse(item.RFQ_ID) & "' AND RRM_V_Company_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'"
        '        'strMail &= "WHERE RM_RFQ_ID = '" & Common.Parse(RFQ_ID) & "'"
        '        Dim tDS As DataSet = objDb.FillDs(strMail)
        '        For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1 'comfirm send mail
        '            objMail.sendNotification(EmailType.RFQReply, HttpContext.Current.Session("UserID"), tDS.Tables(0).Rows(j).Item("RM_Coy_ID"), HttpContext.Current.Session("CompanyID"), tDS.Tables(0).Rows(j).Item("rm_rfq_no"), tDS.Tables(0).Rows(j).Item("RRM_Actual_Quot_Num"), Common.Parse(tDS.Tables(0).Rows(j).Item("RRM_Offer_Till")))
        '        Next
        '        objMail = Nothing

        '    End If

        'End Function

        Public Function add_mstrreplied(ByVal item As RFQ_User, Optional ByVal EDIT As String = "0", Optional ByRef Quotation_num As String = "", Optional ByVal aryPrice As ArrayList = Nothing) As String
            Dim blnGetNewNo As Boolean = True
            Dim strAryQuery(0) As String
            Dim strdelete As String
            Dim strsql As String
            Dim prefix As String
            Dim objglb As New AppGlobals
            Dim intIncrementNo As Integer = 0
            Dim strNewQuoNo As String = ""

            If EDIT = "1" Then  'Resubmit
                strsql = "update RFQ_REPLIES_MSTR set RRM_Currency_Code='" & Common.Parse(item.cur_code) & "',RRM_Offer_Till= " & Common.ConvertDate(item.validaty) & ",RRM_Remarks='" & Common.Parse(item.remark) & "'" & _
                        " ,RRM_Pay_Term_Code='" & Common.Parse(item.pay_term) & "',RRM_Payment_Type='" & Common.Parse(item.pay_type) & "',RRM_Ship_Term=" & Common.Parse(item.ship_term) & ",RRM_Ship_Mode=" & Common.Parse(item.ship_mode) & "," & _
                        " RRM_Contact_Person= '" & Common.Parse(item.con_person) & "',RRM_Contact_Number='" & Common.Parse(item.phone) & "',RRM_Email=' " & item.email & "',RRM_TotalValue=" & item.total & ",RRM_Indicator='" & Common.Parse(item.indicat) & "'" & _
                        " where RRM_RFQ_ID='" & Common.Parse(item.RFQ_ID) & "' and RRM_V_Company_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "
                Common.Insert2Ary(strAryQuery, strsql)

                strdelete = "delete from RFQ_REPLIES_DETAIL where RRD_RFQ_ID='" & Common.Parse(item.RFQ_ID) & "' and RRD_V_Coy_Id='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'"
                Common.Insert2Ary(strAryQuery, strdelete)

                strsql = " insert into RFQ_REPLIES_DETAIL select * from RFQ_REPLIES_DETAIL_TEMP where RRDT_RFQ_ID='" & Common.Parse(item.RFQ_ID) & "' and RRDT_V_Company_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "
                Common.Insert2Ary(strAryQuery, strsql)

                Dim strsql33 As String = "select RRM_Actual_Quot_Num from RFQ_REPLIES_MSTR WHERE RRM_RFQ_ID='" & Common.Parse(item.RFQ_ID) & "'and RRM_V_Company_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'"
                Quotation_num = objDb.GetVal(strsql33)

                ''Insert Volume Price
                If Not aryPrice Is Nothing Then
                    InsertUnitPriceQuotation(EDIT, Common.Parse(item.RFQ_ID), aryPrice, strAryQuery)
                End If

            ElseIf EDIT = "0" Then  'New
                Dim strQuote As String
                'objglb.GetLatestDocNo("Quotation", strAryQuery, Quotation_num, prefix)
                strsql = " SET @T_NO ='';UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'Quotation' "
                Common.Insert2Ary(strAryQuery, strsql)

                intIncrementNo = 1
                Quotation_num = " (SELECT CAST(GROUP_CONCAT(CASE WHEN cp_param_name = 'prefix' THEN cp_param_value ELSE " _
                       & " CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) END " _
                       & " SEPARATOR '') AS CHAR(1000)) AS cp_param_value FROM (SELECT * FROM company_param WHERE CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'Quotation' ORDER BY CP_PARAM_NAME DESC) ZZZ ) "

                prefix = " (SELECT cp_param_value FROM company_param WHERE CP_PARAM_NAME = 'prefix' AND CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'Quotation') "

                'If get_quote(Quotation_num) = True Then 'Duplicate
                '    add_mstrreplied = "1"
                '    Exit Function
                'End If

                If get_rfqQuote(item.RFQ_ID, strQuote) = True Then  'Already sent
                    Quotation_num = strQuote
                    add_mstrreplied = "2"
                    Exit Function
                End If

                strsql = "insert into RFQ_REPLIES_MSTR (RRM_RFQ_ID,RRM_V_Company_ID,RRM_Currency_Code,RRM_Offer_Till," & _
                           "RRM_Remarks,RRM_Pay_Term_Code,RRM_Payment_Type,RRM_Ship_Mode,RRM_Ship_Term,RRM_Created_On,RRM_Created_By," & _
                           "RRM_Contact_Person,RRM_Contact_Number,RRM_Email,RRM_Status,RRM_B_Display_Status,RRM_V_Display_Status,RRM_Indicator,RRM_TotalValue,RRM_Actual_Quot_Num,RRM_Quot_Prefix) values " & _
                           "('" & Common.Parse(item.RFQ_ID) & "','" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "','" & Common.Parse(item.cur_code) & "'," & Common.ConvertDate(item.validaty) & "," & _
                           "'" & Common.Parse(item.remark) & "','" & Common.Parse(item.pay_term) & "','" & Common.Parse(item.pay_type) & "'," & Common.Parse(item.ship_mode) & "," & Common.Parse(item.ship_term) & "," & Common.ConvertDate(item.create_on) & ",'" & Common.Parse(HttpContext.Current.Session("UserId")) & "'," & _
                           "'" & Common.Parse(item.con_person) & "','" & Common.Parse(item.phone) & "',' " & item.email & " ','0','0','0','" & Common.Parse(item.indicat) & "'," & item.total & "," & Quotation_num & "," & prefix & ")"
                Common.Insert2Ary(strAryQuery, strsql)

                strsql = " insert into RFQ_REPLIES_DETAIL select * from RFQ_REPLIES_DETAIL_TEMP where RRDT_RFQ_ID='" & Common.Parse(item.RFQ_ID) & "' and RRDT_V_Company_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "
                Common.Insert2Ary(strAryQuery, strsql)

                ''Insert Volume Price
                If Not aryPrice Is Nothing Then
                    InsertUnitPriceQuotation(EDIT, Common.Parse(item.RFQ_ID), aryPrice, strAryQuery)
                End If

            End If

            strsql = "update RFQ_VENDOR_MSTR set RVM_V_RFQ_Status='1' where RVM_V_Company_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' and RVM_RFQ_ID='" & Common.Parse(item.RFQ_ID) & "'"
            Common.Insert2Ary(strAryQuery, strsql)

            strsql = "update RFQ_MSTR set RM_Status='1' WHERE RM_RFQ_ID='" & Common.Parse(item.RFQ_ID) & "' "
            Common.Insert2Ary(strAryQuery, strsql)

            ' ai chu add on 26/10/2005
            ' since during Step 1 to Step 3, all transaction related to attachment is saved as 'QuotTemp'
            ' is user submit the quotation, need to delete attachment with doc_type = 'Quotation' 
            ' then update COMPANY_DOC_ATTACHMENT table CDA_DOC_TYPE column from 'QuotTemp' to 'Quotation'
            ' for respective RFQ no
            'Modified by Craven 25/04/2011 for saving more then 1 attachment purpose
            'strsql(4) = "DELETE FROM COMPANY_DOC_ATTACHMENT "
            'strsql(4) &= "WHERE CDA_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "
            'strsql(4) &= "AND CDA_DOC_NO = '" & Common.Parse(item.RFQ_Num) & "' "
            'strsql(4) &= "AND CDA_DOC_TYPE = 'Quotation'"
            'Common.Insert2Ary(strAryQuery, strsql(4))

            strsql = "UPDATE COMPANY_DOC_ATTACHMENT "
            strsql &= "SET CDA_DOC_TYPE = 'Quotation' "
            strsql &= "WHERE CDA_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "
            strsql &= "AND CDA_DOC_NO = '" & Common.Parse(item.RFQ_Num) & "' "
            strsql &= "AND CDA_DOC_TYPE = 'QuotTemp'"
            Common.Insert2Ary(strAryQuery, strsql)

            Dim objUsers As New Users
            If EDIT = "0" Then  'New
                objUsers.Log_UserActivityNew(strAryQuery, WheelModule.RFQ, WheelUserActivity.V_SubmitQuotation, Quotation_num, item.RFQ_ID)

                strsql = " SET @T_NO = " & Quotation_num & "; "
                Common.Insert2Ary(strAryQuery, strsql)

                strsql = "UPDATE company_param SET cp_param_value = CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CP_PARAM_TYPE = 'Quotation' "
                Common.Insert2Ary(strAryQuery, strsql)

            Else
                objUsers.Log_UserActivity(strAryQuery, WheelModule.RFQ, WheelUserActivity.V_SubmitQuotation, Quotation_num, item.RFQ_ID)

                strsql = " SET @T_NO = '" & Quotation_num & "'; "
                Common.Insert2Ary(strAryQuery, strsql)
            End If
            objUsers = Nothing

            If objDb.BatchExecuteNew(strAryQuery, , strNewQuoNo, "T_NO") Then 'objDb.BatchExecute(strAryQuery) Then
                Quotation_num = strNewQuoNo

                add_mstrreplied = "4"
                Dim objMail As New Email
                Dim strMail As String

                strMail = "SELECT rm_rfq_no, RRM_Actual_Quot_Num, RRM_Offer_Till, RM_Coy_ID FROM RFQ_MSTR,RFQ_REPLIES_MSTR  "
                strMail &= " where RM_RFQ_ID='" & Common.Parse(item.RFQ_ID) & "' and RRM_RFQ_ID ='" & Common.Parse(item.RFQ_ID) & "' AND RRM_V_Company_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'"
                'strMail &= "WHERE RM_RFQ_ID = '" & Common.Parse(RFQ_ID) & "'"
                Dim tDS As DataSet = objDb.FillDs(strMail)
                For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1 'comfirm send mail
                    objMail.sendNotification(EmailType.RFQReply, Common.Parse(HttpContext.Current.Session("UserID")), tDS.Tables(0).Rows(j).Item("RM_Coy_ID"), Common.Parse(HttpContext.Current.Session("CompanyID")), tDS.Tables(0).Rows(j).Item("rm_rfq_no"), tDS.Tables(0).Rows(j).Item("RRM_Actual_Quot_Num"), Common.Parse(tDS.Tables(0).Rows(j).Item("RRM_Offer_Till")))
                Next
                objMail = Nothing

            End If

        End Function

        Public Function update_replied(ByVal RFQ_ID As String, Optional ByVal EDIT As String = "0") As String
            Dim strAryQuery(0) As String
            Dim strsql As String
            Dim prefix As String

            strsql = "update RFQ_VENDOR_MSTR set RVM_V_RFQ_Status='1' where RVM_V_Company_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' and RVM_RFQ_ID='" & Common.Parse(RFQ_ID) & "'"
            Common.Insert2Ary(strAryQuery, strsql)

            strsql = "update RFQ_MSTR set RM_Status='1' WHERE RM_RFQ_ID='" & Common.Parse(RFQ_ID) & "' "
            Common.Insert2Ary(strAryQuery, strsql)

            If objDb.BatchExecute(strAryQuery) Then 'objDb.BatchExecute(strAryQuery) Then

                update_replied = "1"
                Dim objMail As New Email
                Dim strMail As String

                strMail = "SELECT RM_RFQ_NO, RM_Coy_ID FROM RFQ_MSTR "
                strMail &= " WHERE RM_RFQ_ID='" & Common.Parse(RFQ_ID) & "'"
                Dim tDS As DataSet = objDb.FillDs(strMail)
                For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1 'comfirm send mail
                    objMail.sendNotification(EmailType.RFQSupply, Common.Parse(HttpContext.Current.Session("UserID")), tDS.Tables(0).Rows(j).Item("RM_Coy_ID"), Common.Parse(HttpContext.Current.Session("CompanyID")), tDS.Tables(0).Rows(j).Item("RM_RFQ_NO"), "")
                Next
                objMail = Nothing

            End If

        End Function

        Public Function InsertUnitPriceQuotation(ByVal strMode As String, ByRef strRFQID As String, ByVal aryVolume As ArrayList, ByRef pQuery() As String)
            Dim strAryQuery(0) As String
            Dim arySetUnitPrice, aryTemp As New ArrayList()
            arySetUnitPrice = aryVolume
            Dim i As Integer = 0
            Dim j As Integer = 0
            Dim blnFound As Boolean
            Dim iTotal As Integer = 0
            Dim strQuery As String = ""
            Dim strLine
            Dim objdb As New EAD.DBCom

            If arySetUnitPrice.Count > 0 Then
                If strMode = "0" Then
                    For i = 0 To arySetUnitPrice.Count - 1
                        If arySetUnitPrice(i)(0) <> "" And arySetUnitPrice(i)(1) <> "" Then
                            strQuery = "INSERT INTO RFQ_REPLIES_VOLUME_PRICE "
                            strQuery &= "(RRVP_RFQ_ID, RRVP_V_COY_ID, RRVP_LINE_NO, RRVP_VOLUME, RRVP_VOLUME_PRICE, RRVP_ENT_BY, RRVP_ENT_DT) VALUES "
                            strQuery &= "(" & Common.Parse(strRFQID) & ", "
                            strQuery &= "'" & HttpContext.Current.Session("CompanyId") & "', "
                            strQuery &= arySetUnitPrice(i)(3) & ", " 'Line No
                            strQuery &= arySetUnitPrice(i)(0) & ", " 'Volume
                            strQuery &= arySetUnitPrice(i)(1) & ", " 'Volume Price
                            strQuery &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
                            strQuery &= "getdate()) "

                            Common.Insert2Ary(pQuery, strQuery)
                        End If

                    Next
                Else
                    For i = 0 To arySetUnitPrice.Count - 1
                        If arySetUnitPrice(i)(0) <> "" And arySetUnitPrice(i)(1) <> "" Then
                            If aryTemp.Count > 0 Then
                                blnFound = False
                                For j = 0 To aryTemp.Count - 1
                                    If aryTemp(j)(0) = arySetUnitPrice(i)(3) Then
                                        blnFound = True
                                        Exit For
                                    End If
                                Next

                                If blnFound = False Then
                                    aryTemp.Add(New String() {arySetUnitPrice(i)(3)})
                                End If
                            Else
                                aryTemp.Add(New String() {arySetUnitPrice(i)(3)})
                            End If
                        End If
                    Next

                    For i = 0 To aryTemp.Count - 1
                        strQuery = " DELETE FROM RFQ_REPLIES_VOLUME_PRICE WHERE RRVP_RFQ_ID = '" & Common.Parse(strRFQID) & "' " & _
                                " AND RRVP_V_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND RRVP_LINE_NO = '" & aryTemp(i)(0) & "' "
                        Common.Insert2Ary(pQuery, strQuery)
                    Next

                    For i = 0 To arySetUnitPrice.Count - 1
                        If arySetUnitPrice(i)(0) <> "" And arySetUnitPrice(i)(1) <> "" Then
                            strQuery = "INSERT INTO RFQ_REPLIES_VOLUME_PRICE "
                            strQuery &= "(RRVP_RFQ_ID, RRVP_V_COY_ID, RRVP_LINE_NO, RRVP_VOLUME, RRVP_VOLUME_PRICE, RRVP_ENT_BY, RRVP_ENT_DT) VALUES "
                            strQuery &= "(" & Common.Parse(strRFQID) & ", "
                            strQuery &= "'" & HttpContext.Current.Session("CompanyId") & "', "
                            strQuery &= arySetUnitPrice(i)(3) & ", " 'Line No
                            strQuery &= arySetUnitPrice(i)(0) & ", " 'Volume
                            strQuery &= arySetUnitPrice(i)(1) & ", " 'Volume Price
                            strQuery &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
                            strQuery &= "getdate()) "

                            Common.Insert2Ary(pQuery, strQuery)
                        End If

                    Next
                End If

                


            End If

        End Function

        Public Function GetUnitPriceQuotation(ByVal strRfqId As String, ByVal strLine As String, Optional ByVal strComp As String = "") As DataSet
            Dim strsql As String = ""
            Dim strtemp As String
            Dim ds As New DataSet

            If strComp <> "" Then
                strtemp = Common.Parse(strComp)
            Else
                strtemp = Common.Parse(HttpContext.Current.Session("CompanyId"))
            End If

            strsql = "SELECT RRVP_VOLUME, RRVP_VOLUME_PRICE, RRVP_LINE_NO, RRVP_V_COY_ID FROM RFQ_REPLIES_VOLUME_PRICE "
            strsql &= "WHERE RRVP_RFQ_ID = " & strRfqId & " AND RRVP_V_COY_ID = '" & strtemp & "' AND RRVP_LINE_NO = " & strLine & ""

            ds = objDb.FillDs(strsql)
            Return ds

        End Function

        Public Function get_rfqname2(ByVal rfq_id As String) As DataSet

            Dim DS As DataSet

            Dim strsql As String = "select RM_RFQ_NAME,RM_RFQ_No FROM RFQ_MSTR WHERE RM_RFQ_ID IN(" & rfq_id & ") "

            DS = objDb.FillDs(strsql)

            get_rfqname2 = DS


        End Function


        Function get_rfqQuote(ByVal rfq_id As String, ByRef qtn As String) As Boolean
            'Michelle (3/11/2011) - Issue 1153
            'If objDb.Exist("select '*' from RFQ_REPLIES_MSTR  WHERE RRM_V_Company_ID= '" & HttpContext.Current.Session("CompanyID") & "' AND RRM_RFQ_ID='" & rfq_id & "' ") > 0 Then
            '    get_rfqQuote = 1

            qtn = objDb.GetVal("select rrm_actual_quot_num from RFQ_REPLIES_MSTR  WHERE RRM_V_Company_ID= '" & HttpContext.Current.Session("CompanyID") & "' AND RRM_RFQ_ID='" & rfq_id & "' ")
            If qtn <> "" Then get_rfqQuote = 1

        End Function
        Function get_quote(ByVal quote As String) As Boolean

            If objDb.Exist("select '*' from RFQ_REPLIES_MSTR  WHERE RRM_V_Company_ID= '" & HttpContext.Current.Session("CompanyID") & "' AND RRM_Actual_Quot_Num='" & quote & "' ") > 0 Then
                get_quote = 1
            End If

        End Function
        Public Function get_gst(ByVal CODE_ABBR As String) As String
            Dim strsql As String = "select CODE_VALUE from CODE_MSTR where CODE_CATEGORY='SST' AND CODE_ABBR='" & CODE_ABBR & "'"
            Dim tDS As DataSet = objDb.FillDs(strsql)

            If tDS.Tables(0).Rows.Count > 0 Then
                get_gst = tDS.Tables(0).Rows(0).Item("CODE_VALUE").ToString.Trim
            End If


        End Function

        'Public Function Fill_Tax(ByRef pDropDownList As UI.WebControls.DropDownList) As String
        '    Dim strsql As String = "SELECT TAX_PERC FROM tax"
        '    Dim tDS As DataSet = objDb.FillDs(strsql)

        '    If tDS.Tables(0).Rows.Count > 0 Then
        '        get_Tax = tDS.Tables(0).Rows(0).Item("TAX_PERC").ToString.Trim
        '    End If


        'End Function

        Public Sub Fill_Tax(ByRef pDropDownList As UI.WebControls.DropDownList)
            'Dim myData As New Wheel.Data.WheelDataProvider()
            'Dim drvCodeMstr As DataView
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom
            strSql = "SELECT * FROM tax"
            drw = objDB.GetView(strSql)
            'If pAllStatus Then
            '    drvCodeMstr = myData.GetAllMasterCode(pCodeTableEnum)
            '    'Common.FillDdl(pDropDownList, "CODE_DESC", "CODE_CODE", myData.GetAllMasterCode(pCodeTableEnum))
            'Else
            '    drvCodeMstr = myData.GetMasterCodeByStatus(pCodeTableEnum, "N")
            '    'Common.FillDdl(pDropDownList, "CODE_DESC", "CODE_CODE", myData.GetMasterCodeByStatus(pCodeTableEnum, "N"))
            'End If

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "TAX_PERC", "TAX_AUTO_NO", drw)

                ' Add ---Select---
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                strDefaultValue = lstItem.Text
                pDropDownList.Items.Insert(0, lstItem)

                'pDropDownList.DataBind()
                '//System Default
                'If Not IsDBNull(drw.Item(0)("CC_DEFAULT_CODE")) Then
                '    strDefaultValue = drw.Item(0)("CC_DEFAULT_CODE")
                '    Common.SelDdl(strDefaultValue, pDropDownList, True, True)
                'Else
                'strDefaultValue = ""
                'Common.SelDdl(strDefaultValue, pDropDownList, True, True)
                'End If
                '//Company Default
                '//User Default
            Else
                ' Add ---Select---
                lstItem.Value = "n.a."
                lstItem.Text = "---Not Applicable---"
                strDefaultValue = lstItem.Value
                pDropDownList.Items.Clear()
                pDropDownList.Items.Insert(0, lstItem)

                'strDefaultValue = "---Not Applicable---"
                'pDropDownList.Items.Insert(0, lstItem)
                Common.SelDdl(strDefaultValue, pDropDownList, True, True)

                '//no suppose to happen
            End If
            objDB = Nothing
        End Sub

        Public Function add_2repliedTemp(ByVal item As RFQ_User, Optional ByVal blnTrue As Boolean = False) As String
            Dim srtsql As String
            Dim strGST As String
            Dim strTaxAutoNo As String = ""
            Dim strGSTRate As String = ""
            Dim strCodeDesc As String = ""
            Dim objDB As New EAD.DBCom
            Dim ds As DataSet

            strGST = "SELECT TAX_AUTO_NO, CODE_ABBR, CODE_DESC " &
                    "FROM CODE_MSTR " &
                    "INNER JOIN TAX ON TAX_CODE = CODE_ABBR " &
                    "INNER JOIN COMPANY_MSTR ON CM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " &
                    "WHERE CODE_CATEGORY = 'SST' AND CODE_DELETED = 'N' AND TAX_COUNTRY_CODE = CM_COUNTRY " &
                    "AND TAX_CODE='" & Common.Parse(item.Tax) & "'"

            ds = objDB.FillDs(strGST)
            If ds.Tables(0).Rows.Count > 0 Then
                strTaxAutoNo = CType(ds.Tables(0).Rows(0).Item("TAX_AUTO_NO"), Integer)
                strGSTRate = CType(ds.Tables(0).Rows(0).Item("CODE_ABBR"), String)
                strCodeDesc = CType(ds.Tables(0).Rows(0).Item("CODE_DESC"), String)
            End If
            
            If objDb.Exist("select '*' from RFQ_REPLIES_DETAIL_TEMP where RRDT_RFQ_ID='" & Common.Parse(item.RFQ_ID) & "' and RRDT_V_Company_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' and RRDT_Line_No= '" & Common.Parse(item.lineno) & "' and RRDT_Product_ID='" & Common.Parse(item.product_ID) & "'") Then
                'If IsDBNull(item.Warranty_Terms) Then   'Warranty_Terms=null
                'srtsql = "update RFQ_REPLIES_DETAIL_TEMP set RRDT_Product_Name='" & Common.Parse(item.v_itemCode) & "',RRDT_Min_Pack_Qty='" & Common.Parse(item.MPQ) & "',RRDT_Min_Order_Qty='" & Common.Parse(item.MOQ) & "',RRDT_GST_Code='" & Common.Parse(item.Tax) & "',RRDT_Delivery_Lead_Time='" & Common.Parse(item.Delivery_Lead_Time) & "'" & _
                '", RRDT_Warranty_Terms='" & CInt(Common.Parse(item.Warranty_Terms)) & "',RRDT_Remarks=' " & Common.Parse(item.remark) & "'," & _
                '" RRDT_Unit_Price=" & item.PRICE & " ,RRDT_Tolerance='" & Common.Parse(item.Tolerance) & "',RRDT_GST='" & Common.Parse(item.gst) & "',RRDT_GST_Desc='" & Common.Parse(item.gst_desc) & "'" & _
                '" where RRDT_RFQ_ID='" & Common.Parse(item.RFQ_ID) & "' " & _
                '" and RRDT_V_Company_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' and RRDT_Line_No= '" & Common.Parse(item.lineno) & "' " & _
                '" and RRDT_Product_ID='" & Common.Parse(item.product_ID) & "'"
                srtsql = "update RFQ_REPLIES_DETAIL_TEMP set RRDT_Product_Name='" & Common.Parse(item.v_itemCode) & "',"
                If IsNumeric(item.MPQ) Then
                    srtsql = srtsql & "RRDT_Min_Pack_Qty=" & item.MPQ

                Else
                    srtsql = srtsql & " RRDT_Min_Pack_Qty=NULL"
                End If
                If IsNumeric(item.MOQ) Then
                    srtsql = srtsql & ",RRDT_Min_Order_Qty=" & item.MOQ

                Else
                    srtsql = srtsql & ",RRDT_Min_Order_Qty=NULL"
                End If
                srtsql = srtsql & ",RRDT_GST_Code='" & Common.Parse(item.Tax) & "',RRDT_Delivery_Lead_Time='" & CInt(Common.Parse(item.Delivery_Lead_Time)) & "'" & _
                       ", RRDT_Warranty_Terms='" & CInt(Common.Parse(item.Warranty_Terms)) & "',RRDT_Remarks=' " & Common.Parse(item.remark) & "',"
                If IsNumeric(item.PRICE) Then
                    srtsql = srtsql & "RRDT_Unit_Price=" & item.PRICE

                Else
                    srtsql = srtsql & "RRDT_Unit_Price=NULL"
                End If

                If blnTrue = True Then
                    srtsql = srtsql & ",RRDT_DEL_CODE='" & Common.Parse(item.del_code) & "'"
                End If

                If IsNumeric(item.gst) Then
                    srtsql = srtsql & ",RRDT_GST=" & item.gst

                Else
                    srtsql = srtsql & ",RRDT_GST=NULL"
                End If

                'srtsql = srtsql & ",RRDT_GST='" & Common.Parse(item.gst) & "',RRDT_GST_Desc='" & Common.Parse(item.gst_desc) & "'" & _
                srtsql = srtsql & ",RRDT_GST_Code='" & Common.Parse(strTaxAutoNo) & "',RRDT_GST_RATE='" & Common.Parse(strGSTRate) & "',RRDT_GST_Desc='" & Common.Parse(strCodeDesc) & "' " & _
                               " where RRDT_RFQ_ID='" & Common.Parse(item.RFQ_ID) & "' " & _
                               " and RRDT_V_Company_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' and RRDT_Line_No= '" & Common.Parse(item.lineno) & "' " & _
                               " and RRDT_Product_ID='" & Common.Parse(item.product_ID) & "'"

                'If IsNumeric(item.PRICE) Then
                '    srtsql = "update RFQ_REPLIES_DETAIL_TEMP set RRDT_Product_Name='" & Common.Parse(item.v_itemCode) & "',RRDT_Min_Pack_Qty='" & Common.Parse(item.MPQ) & "',RRDT_Min_Order_Qty='" & Common.Parse(item.MOQ) & "',RRDT_GST_Code='" & Common.Parse(item.Tax) & "',RRDT_Delivery_Lead_Time='" & CInt(Common.Parse(item.Delivery_Lead_Time)) & "'" & _
                '               ", RRDT_Warranty_Terms='" & CInt(Common.Parse(item.Warranty_Terms)) & "',RRDT_Remarks=' " & Common.Parse(item.remark) & "'," & _
                '               " RRDT_Unit_Price=" & item.PRICE & ",RRDT_GST='" & Common.Parse(item.gst) & "',RRDT_GST_Desc='" & Common.Parse(item.gst_desc) & "'" & _
                '               " where RRDT_RFQ_ID='" & Common.Parse(item.RFQ_ID) & "' " & _
                '               " and RRDT_V_Company_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' and RRDT_Line_No= '" & Common.Parse(item.lineno) & "' " & _
                '               " and RRDT_Product_ID='" & Common.Parse(item.product_ID) & "'"
                'Else
                '    srtsql = "update RFQ_REPLIES_DETAIL_TEMP set RRDT_Product_Name='" & Common.Parse(item.v_itemCode) & "',RRDT_Min_Pack_Qty='" & Common.Parse(item.MPQ) & "',RRDT_Min_Order_Qty='" & Common.Parse(item.MOQ) & "',RRDT_GST_Code='" & Common.Parse(item.Tax) & "',RRDT_Delivery_Lead_Time='" & CInt(Common.Parse(item.Delivery_Lead_Time)) & "'" & _
                '           ", RRDT_Warranty_Terms='" & CInt(Common.Parse(item.Warranty_Terms)) & "',RRDT_Remarks=' " & Common.Parse(item.remark) & "'," & _
                '           " RRDT_Unit_Price=NULL,RRDT_GST='" & Common.Parse(item.gst) & "',RRDT_GST_Desc='" & Common.Parse(item.gst_desc) & "'" & _
                '           " where RRDT_RFQ_ID='" & Common.Parse(item.RFQ_ID) & "' " & _
                '           " and RRDT_V_Company_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' and RRDT_Line_No= '" & Common.Parse(item.lineno) & "' " & _
                '           " and RRDT_Product_ID='" & Common.Parse(item.product_ID) & "'"
                'End If


                'Else
                '    srtsql = "update RFQ_REPLIES_DETAIL_TEMP set RRDT_Product_Name='" & Common.Parse(item.v_itemCode) & "',RRDT_Min_Pack_Qty='" & Common.Parse(item.MPQ) & "',RRDT_Min_Order_Qty='" & Common.Parse(item.MOQ) & "',RRDT_GST_Code='" & Common.Parse(item.Tax) & "',RRDT_Delivery_Lead_Time='" & Common.Parse(item.Delivery_Lead_Time) & "'" & _
                '    ", RRDT_Warranty_Terms='" & Common.Parse(item.Warranty_Terms) & "',RRDT_Remarks=' " & Common.Parse(item.remark) & "'," & _
                '    " RRDT_Unit_Price=" & item.PRICE & " ,RRDT_Tolerance='" & Common.Parse(item.Tolerance) & "',RRDT_GST='" & Common.Parse(item.gst) & "',RRDT_GST_Desc='" & Common.Parse(item.gst_desc) & "'" & _
                '    " where RRDT_RFQ_ID='" & Common.Parse(item.RFQ_ID) & "' " & _
                '    " and RRDT_V_Company_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' and RRDT_Line_No= '" & Common.Parse(item.lineno) & "' " & _
                '    " and RRDT_Product_ID='" & Common.Parse(item.product_ID) & "'"

                'End If
            Else
                'srtsql = "insert into RFQ_REPLIES_DETAIL_TEMP (RRDT_RFQ_ID,RRDT_V_Company_ID,RRDT_Line_No,RRDT_Product_ID" & _
                '",RRDT_Unit_Price,RRDT_Product_desc,RRDT_Quantity,RRDT_UOM,RRDT_Delivery_Lead_Time,RRDT_Warranty_Terms,RRDT_Remarks, " & _
                '" RRDT_Min_Pack_Qty,RRDT_Min_Order_Qty,RRDT_GST_Code,RRDT_Product_Name,RRDT_Tolerance,RRDT_GST,RRDT_GST_Desc)" & _
                '"values ('" & Common.Parse(item.RFQ_ID) & "','" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "','" & Common.Parse(item.lineno) & "','" & Common.Parse(item.product_ID) & "'" & _
                '"," & item.PRICE & ",'" & Common.Parse(item.item_desc) & "','" & Common.Parse(item.Quantity) & "','" & Common.Parse(item.uom) & "','" & Common.Parse(item.Delivery_Lead_Time) & "', " & _
                '" '" & CInt(Common.Parse(item.Warranty_Terms)) & "','" & Common.Parse(item.remark) & "','" & Common.Parse(item.MPQ) & "','" & Common.Parse(item.MOQ) & "'," & _
                '"  '" & Common.Parse(item.Tax) & "','" & Common.Parse(item.v_itemCode) & "','" & Common.Parse(item.Tolerance) & "','" & Common.Parse(item.gst) & "','" & Common.Parse(item.gst_desc) & "')"
                srtsql = "insert into RFQ_REPLIES_DETAIL_TEMP (RRDT_RFQ_ID,RRDT_V_Company_ID,RRDT_Line_No,RRDT_Product_ID,RRDT_Unit_Price,RRDT_Product_desc,RRDT_Quantity,RRDT_UOM,RRDT_Delivery_Lead_Time,RRDT_Warranty_Terms,RRDT_Remarks, " & _
                        " RRDT_Min_Pack_Qty,RRDT_Min_Order_Qty,RRDT_GST_Code,RRDT_Product_Name,RRDT_Tolerance,RRDT_GST,RRDT_GST_Desc "
                If blnTrue = True Then
                    srtsql &= ",RRDT_DEL_CODE "
                End If
                srtsql &= ")values ('" & Common.Parse(item.RFQ_ID) & "','" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "','" & Common.Parse(item.lineno) & "','" & Common.Parse(item.product_ID) & "',"
                If IsNumeric(item.PRICE) Then
                    srtsql = srtsql & item.PRICE

                Else
                    srtsql = srtsql & "NULL"
                End If
                srtsql = srtsql & ",'" & Common.Parse(item.item_desc) & "','" & Common.Parse(item.Quantity) & "','" & Common.Parse(item.uom) & "','" & Common.Parse(item.Delivery_Lead_Time) & "', " & _
                        " '" & CInt(Common.Parse(item.Warranty_Terms)) & "','" & Common.Parse(item.remark) & "',"
                If IsNumeric(item.MPQ) Then
                    srtsql = srtsql & item.MPQ

                Else
                    srtsql = srtsql & "NULL"
                End If
                If IsNumeric(item.MOQ) Then
                    srtsql = srtsql & "," & item.MOQ

                Else
                    srtsql = srtsql & ",NULL"
                End If
                srtsql = srtsql & ",'" & Common.Parse(item.Tax) & "','" & Common.Parse(item.v_itemCode) & "','" & Common.Parse(item.Tolerance) & "','" & Common.Parse(item.gst) & "','" & Common.Parse(item.gst_desc) & "'"
                If blnTrue = True Then
                    srtsql &= ",'" & Common.Parse(item.del_code) & "' "
                End If
                srtsql &= ")"
            End If
            add_2repliedTemp = srtsql
        End Function
        Public Function check_po(ByVal rfq_id As String) As Boolean
            If objDb.Exist("select '*' from PR_mstr where PRM_RFQ_INDEX='" & rfq_id & "' and PRM_PR_STATUS='" & PRStatus.ConvertedToPO & "' AND PRM_S_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'") > 0 Then
                Return True
            Else
                Return False
            End If

        End Function
        Public Function get_qoute(ByVal rfq_num As String, ByVal com_name As String, ByVal v_DisStatus As String, ByVal V_RFQ_Status As String) As DataSet

            Dim strsql As String = "select distinct RRM.RRM_Actual_Quot_Num,RRM.RRM_Created_On,RRM.RRM_Offer_Till,RM.RM_RFQ_Name,RM.RM_Status,rm.RM_RFQ_ID" & _
            " ,RM.RM_RFQ_No,RM_RFQ_ID,CM.CM_COY_NAME,RRM.RRM_V_Display_Status,RRM.RRM_RFQ_ID from RFQ_REPLIES_MSTR RRM,RFQ_MSTR RM,COMPANY_MSTR CM where RRM.RRM_V_Company_ID= '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' and " & _
                                    "RRM.RRM_V_Display_Status='0' and RRM_Created_By='" & Common.Parse(HttpContext.Current.Session("UserID")) & "' and RM.RM_RFQ_ID=RRM.RRM_RFQ_ID and CM.CM_COY_ID=RM.RM_Coy_ID "
            If rfq_num <> "" Then
                strsql = strsql & " AND (RRM_Actual_Quot_Num " & Common.ParseSQL(rfq_num) & " or RRM_RFQ_ID in(select RM_RFQ_ID from RFQ_MSTR WHERE RM_RFQ_No" & Common.ParseSQL(rfq_num) & "))"
            End If

            If com_name <> "" Then
                strsql = strsql & " AND CM.CM_COY_NAME " & Common.ParseSQL(com_name)
            End If
            Dim dscath As New DataSet
            dscath = objDb.FillDs(strsql)
            get_qoute = dscath

        End Function

        Public Function get_RFQ(ByVal rfq_num As String, ByVal com_name As String, ByVal v_DisStatus As String, ByVal V_RFQ_Status As String) As DataSet
            Dim strTemp As String
            'Dim strsql As String = "SELECT '' AS RRM_Actual_Quot_Num,RM.RM_RFQ_Name ,RM.RM_RFQ_ID,RM.RM_RFQ_No, " _
            '        & "RM.RM_Created_On, RM.RM_Status,CAST(RVM_V_RFQ_Status AS CHAR) AS RVM_V_RFQ_Status,'' AS RRM_Offer_Till," _
            '        & "CM.CM_COY_NAME,RM_Coy_ID,RM_B_Display_Status AS RStatus " _
            '        & "FROM RFQ_MSTR RM,RFQ_VENDOR_MSTR RVM,COMPANY_MSTR CM " _
            '        & "WHERE RM.RM_Status<>'3' AND RVM.RVM_V_Company_ID='" & HttpContext.Current.Session("CompanyID") & "' AND RM.RM_RFQ_ID=RVM.RVM_RFQ_ID " _
            '        & "AND CM.CM_COY_ID=RM.RM_Coy_ID AND RM_EXPIRY_DATE >= CURRENT_DATE() AND RVM.RVM_V_Display_Status='" & v_DisStatus & "' " _
            '        & "AND RM.RM_RFQ_ID NOT IN (SELECT RRM_RFQ_ID FROM RFQ_REPLIES_MSTR WHERE RRM_V_COMPANY_ID = '" & HttpContext.Current.Session("CompanyId") & "')"

            'If V_RFQ_Status = "1" Then
            '    strsql = strsql & " AND RVM.RVM_V_RFQ_Status = '" & Common.Parse(Common.Parse(V_RFQ_Status)) & "' "
            'End If
            'If rfq_num <> "" Then 
            '    strsql = strsql & " AND RM.RM_RFQ_No" & Common.ParseSQL(rfq_num)
            'End If

            'If com_name <> "" Then
            '    strsql = strsql & " AND CM.CM_COY_NAME " & Common.ParseSQL(com_name)
            'End If

            'Dim strsql1 = "SELECT DISTINCT RRM.RRM_Actual_Quot_Num,RM.RM_RFQ_Name,RM_RFQ_ID,RM.RM_RFQ_No, " _
            '        & "RRM.RRM_Created_On AS RM_Created_On,RM.RM_Expiry_Date,RM.RM_Status,'' AS RVM_V_RFQ_Status,CAST(RRM.RRM_Offer_Till AS CHAR) AS RRM_Offer_Till," _
            '        & "CM.CM_COY_NAME,RM_Coy_ID,RRM.RRM_V_Display_Status AS RStatus " _
            '        & "FROM RFQ_REPLIES_MSTR RRM,RFQ_MSTR RM,COMPANY_MSTR CM " _
            '        & "WHERE RRM.RRM_V_Company_ID= '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND RRM.RRM_V_Display_Status='0' " _
            '        & "AND RRM_Created_By='" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND RM.RM_RFQ_ID=RRM.RRM_RFQ_ID AND CM.CM_COY_ID=RM.RM_Coy_ID "
            Dim strsql1 = "SELECT DISTINCT RRM.RRM_Actual_Quot_Num,RM.RM_RFQ_Name,RM_RFQ_ID,RM.RM_RFQ_No," _
                    & "RRM.RRM_Created_On AS RM_Created_On,RM.RM_Expiry_Date,RM.RM_Status,RVM_V_RFQ_Status,RRM.RRM_Offer_Till," _
                    & "CM.CM_COY_NAME, RM_Coy_ID, RRM.RRM_V_Display_Status, RM.RM_B_DISPLAY_STATUS " _
                    & "FROM RFQ_REPLIES_MSTR RRM,RFQ_MSTR RM,RFQ_VENDOR_MSTR RVM,COMPANY_MSTR CM " _
                    & "WHERE RRM.RRM_V_Company_ID= '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND RRM.RRM_V_Display_Status='0' and RM.RM_RFQ_ID=RVM.RVM_RFQ_ID " _
                    & "AND RRM.RRM_V_Company_ID=RVM.RVM_V_Company_ID AND RRM_Created_By='" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND RM.RM_RFQ_ID=RRM.RRM_RFQ_ID AND CM.CM_COY_ID=RM.RM_Coy_ID"
            'Dim strsql1 As String = "SELECT '' AS RRM_Actual_Quot_Num,RM.RM_RFQ_Name ,RM.RM_RFQ_ID,RM.RM_RFQ_No, " _
            '        & "RM.RM_Created_On, RM.RM_Status,'' AS RRM_Offer_Till," _
            '        & "CM.CM_COY_NAME,RM_Coy_ID,RM_B_Display_Status AS RStatus " _
            '        & "FROM RFQ_MSTR RM,RFQ_VENDOR_MSTR RVM,COMPANY_MSTR CM " _
            '        & "WHERE RM.RM_Status<>'3' AND RVM.RVM_V_Company_ID='carol' AND  RM.RM_RFQ_ID=RVM.RVM_RFQ_ID " _
            '        & "AND CM.CM_COY_ID=RM.RM_Coy_ID AND RM_EXPIRY_DATE >= CURRENT_DATE() AND RVM.RVM_V_Display_Status='" & v_DisStatus & "' " _
            '        & "AND RM.RM_RFQ_ID NOT IN (SELECT RRM_RFQ_ID FROM RFQ_REPLIES_MSTR WHERE RRM_V_COMPANY_ID = 'carol') " _
            '        & "UNION " _
            '        & "SELECT DISTINCT RRM.RRM_Actual_Quot_Num,RM.RM_RFQ_Name,RM_RFQ_ID,RM.RM_RFQ_No, " _
            '        & "RRM.RRM_Created_On,RM.RM_Status,RRM.RRM_Offer_Till," _
            '        & "CM.CM_COY_NAME,RM_Coy_ID,RRM.RRM_V_Display_Status AS RStatus " _
            '        & "FROM RFQ_REPLIES_MSTR RRM,RFQ_MSTR RM,COMPANY_MSTR CM " _
            '        & "WHERE RRM.RRM_V_Company_ID= '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND RRM.RRM_V_Display_Status='0' " _
            '        & "AND RRM_Created_By='" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND RM.RM_RFQ_ID=RRM.RRM_RFQ_ID AND CM.CM_COY_ID=RM.RM_Coy_ID "
            If rfq_num <> "" Then
                strTemp = Common.BuildWildCard(rfq_num)
                strsql1 = strsql1 & " AND (RRM_Actual_Quot_Num " & Common.ParseSQL(strTemp) & " or RRM_RFQ_ID in(select RM_RFQ_ID from RFQ_MSTR WHERE RM_RFQ_No" & Common.ParseSQL(strTemp) & "))"
            End If

            If com_name <> "" Then
                strTemp = Common.BuildWildCard(com_name)
                strsql1 = strsql1 & " AND CM.CM_COY_NAME " & Common.ParseSQL(strTemp)
            End If

            'strsql = strsql & " UNION " & strsql1
            Dim dscath As New DataSet
            dscath = objDb.FillDs(strsql1)
            get_RFQ = dscath

        End Function

        Public Function get_trash(ByVal rfq_num As String, ByVal com_name As String, ByVal v_DisStatus As String, ByVal V_RFQ_Status As String) As DataSet

            Dim strsql As String = "select RM.RM_RFQ_No,RM.RM_Created_On,CM.CM_COY_NAME,RVM.RVM_V_RFQ_Status,RM.RM_Expiry_Date,RM.RM_RFQ_ID,RM_Prefix,'RFQ' AS DocType   " & _
                                    " FROM RFQ_MSTR RM,RFQ_VENDOR_MSTR RVM,COMPANY_MSTR CM " & _
                                    " WHERE " & _
                                    "  RM.RM_Status<>'3' AND RVM.RVM_V_Company_ID= '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'" & _
                                    " and RM.RM_RFQ_ID=RVM.RVM_RFQ_ID  AND CM.CM_COY_ID=RM.RM_Coy_ID " & _
                                    " and RVM.RVM_V_Display_Status='1' "
            If rfq_num <> "" Then
                strsql = strsql & " AND RM.RM_RFQ_No" & Common.ParseSQL(rfq_num)
            End If



            If com_name <> "" Then
                strsql = strsql & " AND (CM.CM_COY_NAME " & Common.ParseSQL(com_name) & "  or CM.CM_COY_NAME " & Common.ParseSQL(com_name) & ")"
            End If

            strsql = strsql & " union all select  RRM.RRM_Actual_Quot_Num,RRM.RRM_Created_On,CM.CM_COY_NAME,RRM_V_Display_Status,RRM.RRM_Offer_Till,RRM.RRM_RFQ_ID,RRM_Quot_Prefix,'Quotation' AS DocType   " & _
                                    " from RFQ_REPLIES_MSTR RRM,RFQ_MSTR RM,COMPANY_MSTR CM" & _
                                    " where RRM.RRM_V_Company_ID= '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' and RRM.RRM_V_Display_Status='1' " & _
                                    " and RM.RM_RFQ_ID=RRM.RRM_RFQ_ID and CM.CM_COY_ID=RM.RM_Coy_ID and  RRM_Created_By='" & HttpContext.Current.Session("UserID") & "'"
            If rfq_num <> "" Then
                strsql = strsql & " AND RRM_Actual_Quot_Num = '" & Common.Parse(Replace(rfq_num, "'", "''")) & "' "
            End If

            If com_name <> "" Then
                strsql = strsql & " AND (CM.CM_COY_NAME " & Common.ParseSQL(com_name) & "  or CM.CM_COY_NAME " & Common.ParseSQL(com_name) & ")"
            End If
            Dim dscath As New DataSet
            dscath = objDb.FillDs(strsql)
            get_trash = dscath
        End Function

        Public Function get_expiredate(ByVal rfq_id As String) As DateTime

            Dim strsql As String = "select RM_Expiry_Date from RFQ_MSTR WHERE RM_RFQ_ID='" & rfq_id & "'"

            Dim tDS As DataSet = objDb.FillDs(strsql)

            If tDS.Tables(0).Rows.Count > 0 Then
                get_expiredate = tDS.Tables(0).Rows(0).Item("RM_Expiry_Date").ToString.Trim
            End If

        End Function

        Public Function get_rfqstatus(ByVal rfq_id As String) As String

            Dim strsql As String = "select RM_B_Display_Status from RFQ_MSTR where RM_RFQ_ID='" & rfq_id & "'"
            Dim tDS As DataSet = objDb.FillDs(strsql)

            If tDS.Tables(0).Rows.Count > 0 Then
                get_rfqstatus = tDS.Tables(0).Rows(0).Item("RM_B_Display_Status").ToString.Trim
            End If

        End Function

        Public Function get_itemrank(ByVal rfq_id As String, ByVal chk As Integer, ByVal array() As String, Optional ByVal sort As String = "ASC", Optional ByVal chk_compare As Integer = 0, Optional ByVal len As Integer = 0, Optional ByVal compLevel As Boolean = False) As DataSet
            Dim strsql As String
            If chk = "0" Then ' for Max Min


                '//by Moo, add "CM.CM_COY_ID,0 AS RRD_Line_No,RRM_Offer_Till"
                'strsql = "SELECT CM.CM_COY_NAME, IsNull( RRM.RRM_Actual_Quot_Num, 'n.a.') as " & _
                '                                     " RRM_Actual_Quot_Num, RRM_V_Company_ID,RRM_Currency_Code , RRM.RRM_TotalValue, CM.CM_COY_ID" & _
                '                                     " FROM COMPANY_MSTR CM,RFQ_REPLIES_MSTR RRM " & _
                '                                     " WHERE RRM.RRM_RFQ_ID = '" & Common.Parse(rfq_id) & "' AND CM.CM_COY_ID  =RRM.RRM_V_Company_ID " & _
                '                                     " AND RRM.RRM_Indicator = 0 order by RRM_TotalValue "
                strsql = "SELECT CM.CM_COY_NAME, IsNull( RRM.RRM_Actual_Quot_Num, 'n.a.') as " & _
                                                " RRM_Actual_Quot_Num, RRM_Currency_Code,RRM_V_Company_ID,RRM_RFQ_ID,RRM_Currency_Code , RRM.RRM_TotalValue, CM.CM_COY_ID,0 AS RRD_Line_No,RRM_Offer_Till " & _
                                                " FROM COMPANY_MSTR CM,RFQ_REPLIES_MSTR RRM " & _
                                                " WHERE RRM.RRM_RFQ_ID = '" & Common.Parse(rfq_id) & "' AND CM.CM_COY_ID  =RRM.RRM_V_Company_ID " & _
                                                " AND RRM.RRM_Indicator = 0 order by RRM_TotalValue "
                If sort = "ASC" Then
                    strsql = strsql & "ASC"
                Else
                    strsql = strsql & "DESC"
                End If
            Else

                If chk = "1" Then 'for selected item
                    'strsql = "Select CM.CM_COY_NAME, IsNull( RRM.RRM_Actual_Quot_Num, 'n.a.') as  RRM_Actual_Quot_Num " & _
                    '        " ,RRM_V_Company_ID,RRM_Currency_Code,Sum((RRD.RRD_Quantity*RRD.RRD_Unit_Price)+(RRD.RRD_Quantity*RRD.RRD_Unit_Price*RRD_GST/100)) as RRM_TotalValue," & _
                    '        "  RRM_RFQ_ID,RRM_Currency_Code,RRD.RRD_V_Coy_Id,RRM_Offer_Till " & _
                    '        " From RFQ_REPLIES_DETAIL RRD,  COMPANY_MSTR CM, RFQ_REPLIES_MSTR RRM " & _
                    '        " Where RRD.RRD_RFQ_ID = '" & Common.Parse(rfq_id) & "' And RRD.RRD_Unit_Price <> 0.0 And RRM.RRM_RFQ_ID = RRD.RRD_RFQ_ID " & _
                    '        " And RRD.RRD_V_Coy_Id in " & _
                    '        " (select RRM_V_Company_ID from  RFQ_REPLIES_MSTR  where RRM_RFQ_ID = '" & Common.Parse(rfq_id) & "' "
                    'Jules 2014.08.06
                    'strsql = "Select CM.CM_COY_NAME, IsNull( RRM.RRM_Actual_Quot_Num, 'n.a.') as  RRM_Actual_Quot_Num " & _
                    '        " ,RRM_V_Company_ID,RRM_Currency_Code,Sum((RRD.RRD_Quantity*RRD.RRD_Unit_Price)+(RRD.RRD_Quantity*RRD.RRD_Unit_Price*RRD_GST/100)) as RRM_TotalValue," & _
                    '        "  RRM_RFQ_ID,RRM_Currency_Code,RRD.RRD_V_Coy_Id,RRM_Offer_Till " & _
                    '        " From RFQ_REPLIES_DETAIL RRD,  COMPANY_MSTR CM, RFQ_REPLIES_MSTR RRM " & _
                    '        " Where RRD.RRD_RFQ_ID = '" & Common.Parse(rfq_id) & "' And RRM.RRM_RFQ_ID = RRD.RRD_RFQ_ID " & _
                    '        " AND RRD_Unit_Price IS NOT NULL And RRD.RRD_V_Coy_Id in " & _
                    '        " (select RRM_V_Company_ID from  RFQ_REPLIES_MSTR  where RRM_RFQ_ID = '" & Common.Parse(rfq_id) & "' "
                    '2015-06-24: CH: Rounding issue (Prod issue)
                    'strsql = "SELECT a.CM_COY_NAME, a.CM_COUNTRY, IFNULL( a.RRM_Actual_Quot_Num, 'n.a.') AS  RRM_Actual_Quot_Num  ,a.RRM_V_Company_ID,a.RRM_Currency_Code, " & _
                    '        "a.RRM_TotalValue AS RRM_TotalValue,  a.RRM_RFQ_ID,a.RRM_Currency_Code,a.RRD_V_Coy_Id,a.RRM_Offer_Till " & _
                    '        "FROM (SELECT CM.CM_COY_NAME, CM.CM_COUNTRY, RRM.RRM_Actual_Quot_Num,RRM_V_Company_ID,RRM_Currency_Code, " & _
                    '        "IF(RRD.RRD_GST_RATE = '' OR RRD.RRD_GST_RATE IS NULL, SUM(RRD.RRD_Quantity*RRD.RRD_Unit_Price), " & _
                    '        "SUM((RRD.RRD_Quantity*RRD.RRD_Unit_Price)+(RRD.RRD_Quantity*RRD.RRD_Unit_Price*(SELECT IF(TAX_PERC = '' OR TAX_PERC IS NULL, 0, TAX_PERC) AS TAX_PERC FROM CODE_MSTR " & _
                    '        "INNER JOIN TAX ON TAX_CODE = CODE_ABBR " & _
                    '        "WHERE CODE_CATEGORY = 'GST' AND CODE_DELETED = 'N' AND CODE_ABBR=RRD_GST_RATE)/100))) AS RRM_TotalValue,RRD_GST_RATE,RRM_RFQ_ID,RRD.RRD_V_Coy_Id,RRM_Offer_Till " & _
                    '        "FROM RFQ_REPLIES_DETAIL RRD,  COMPANY_MSTR CM, RFQ_REPLIES_MSTR RRM  WHERE RRD.RRD_RFQ_ID = '" & Common.Parse(rfq_id) & "' AND RRM.RRM_RFQ_ID = RRD.RRD_RFQ_ID " & _
                    '        "AND RRD_Unit_Price IS NOT NULL AND RRD.RRD_V_Coy_Id IN  (SELECT RRM_V_Company_ID FROM  RFQ_REPLIES_MSTR  WHERE RRM_RFQ_ID = '" & Common.Parse(rfq_id) & "'  "
                    strsql = "SELECT a.CM_COY_NAME, a.CM_COUNTRY, IFNULL( a.RRM_Actual_Quot_Num, 'n.a.') AS  RRM_Actual_Quot_Num  ,a.RRM_V_Company_ID,a.RRM_Currency_Code, " &
                            "a.RRM_TotalValue AS RRM_TotalValue,  a.RRM_RFQ_ID,a.RRM_Currency_Code,a.RRD_V_Coy_Id,a.RRM_Offer_Till " &
                            "FROM (SELECT CM.CM_COY_NAME, CM.CM_COUNTRY, RRM.RRM_Actual_Quot_Num,RRM_V_Company_ID,RRM_Currency_Code, " &
                            "IF(RRD.RRD_GST_RATE = '' OR RRD.RRD_GST_RATE IS NULL, SUM(ROUND(RRD.RRD_Quantity*RRD.RRD_Unit_Price,2)), " &
                            "SUM((ROUND(RRD.RRD_Quantity*RRD.RRD_Unit_Price,2))+ROUND((ROUND(RRD.RRD_Quantity*RRD.RRD_Unit_Price,2)*(SELECT IF(TAX_PERC = '' OR TAX_PERC IS NULL, 0, TAX_PERC) AS TAX_PERC FROM CODE_MSTR " &
                            "INNER JOIN TAX ON TAX_CODE = CODE_ABBR " &
                            "WHERE CODE_CATEGORY = 'SST' AND CODE_DELETED = 'N' AND CODE_ABBR=RRD_GST_RATE)/100),2))) AS RRM_TotalValue,RRD_GST_RATE,RRM_RFQ_ID,RRD.RRD_V_Coy_Id,RRM_Offer_Till " &
                            "FROM RFQ_REPLIES_DETAIL RRD,  COMPANY_MSTR CM, RFQ_REPLIES_MSTR RRM  WHERE RRD.RRD_RFQ_ID = '" & Common.Parse(rfq_id) & "' AND RRM.RRM_RFQ_ID = RRD.RRD_RFQ_ID " &
                            "AND RRD_Unit_Price IS NOT NULL AND RRD.RRD_V_Coy_Id IN  (SELECT RRM_V_Company_ID FROM  RFQ_REPLIES_MSTR  WHERE RRM_RFQ_ID = '" & Common.Parse(rfq_id) & "'  "
                    '"AND CM.CM_COY_ID = RRD.RRD_V_Coy_Id AND RRM.RRM_V_Company_ID = RRD.RRD_V_Coy_Id  AND RRD.RRD_Line_No IN ( SELECT RD_RFQ_Line FROM RFQ_DETAIL "
                ElseIf chk = "2" Then
                    'strsql = "Select CM.CM_COY_NAME, IsNull( RRM.RRM_Actual_Quot_Num, 'n.a.') as  RRM_Actual_Quot_Num " & _
                    '                           " ,RRM_V_Company_ID,RRD_Unit_Price,RRM_Currency_Code,Sum((RRD.RRD_Quantity*RRD.RRD_Unit_Price)+(RRD.RRD_Quantity*RRD.RRD_Unit_Price*RRD_GST/100)) as RRM_TotalValue," & _
                    '                           "  RRM_RFQ_ID,RRM_Currency_Code,RRD.RRD_V_Coy_Id,RRM_Offer_Till " & _
                    '                           " From RFQ_REPLIES_DETAIL RRD,  COMPANY_MSTR CM, RFQ_REPLIES_MSTR RRM " & _
                    '                           " Where RRD.RRD_RFQ_ID = '" & Common.Parse(rfq_id) & "' And RRD.RRD_Unit_Price <> 0.0 And RRM.RRM_RFQ_ID = RRD.RRD_RFQ_ID " & _
                    '                           " And RRD.RRD_V_Coy_Id in " & _
                    '                           " (select RRM_V_Company_ID from  RFQ_REPLIES_MSTR  where RRM_RFQ_ID = '" & Common.Parse(rfq_id) & "' "
                    '2015-06-24: CH: Rounding issue (Prod issue)
                    'strsql = "Select CM.CM_COY_NAME, IsNull( RRM.RRM_Actual_Quot_Num, 'n.a.') as  RRM_Actual_Quot_Num " & _
                    '       " ,RRM_V_Company_ID,RRD_Unit_Price,RRM_Currency_Code,Sum((RRD.RRD_Quantity*RRD.RRD_Unit_Price)+(RRD.RRD_Quantity*RRD.RRD_Unit_Price*RRD_GST/100)) as RRM_TotalValue," & _
                    '       "  RRM_RFQ_ID,RRM_Currency_Code,RRD.RRD_V_Coy_Id,RRM_Offer_Till " & _
                    '       " From RFQ_REPLIES_DETAIL RRD,  COMPANY_MSTR CM, RFQ_REPLIES_MSTR RRM " & _
                    '       " Where RRD.RRD_RFQ_ID = '" & Common.Parse(rfq_id) & "' And RRM.RRM_RFQ_ID = RRD.RRD_RFQ_ID " & _
                    '       " AND RRD_Unit_Price IS NOT NULL And RRD.RRD_V_Coy_Id in " & _
                    '       " (select RRM_V_Company_ID from  RFQ_REPLIES_MSTR  where RRM_RFQ_ID = '" & Common.Parse(rfq_id) & "' "
                    strsql = "Select CM.CM_COY_NAME, IsNull( RRM.RRM_Actual_Quot_Num, 'n.a.') as  RRM_Actual_Quot_Num " & _
                           " ,RRM_V_Company_ID,RRD_Unit_Price,RRM_Currency_Code,SUM(ROUND(RRD.RRD_Quantity*RRD.RRD_Unit_Price,2)+ROUND(ROUND(RRD.RRD_Quantity*RRD.RRD_Unit_Price,2)*RRD_GST/100,2)) as RRM_TotalValue," & _
                           "  RRM_RFQ_ID,RRM_Currency_Code,RRD.RRD_V_Coy_Id,RRM_Offer_Till " & _
                           " From RFQ_REPLIES_DETAIL RRD,  COMPANY_MSTR CM, RFQ_REPLIES_MSTR RRM " & _
                           " Where RRD.RRD_RFQ_ID = '" & Common.Parse(rfq_id) & "' And RRM.RRM_RFQ_ID = RRD.RRD_RFQ_ID " & _
                           " AND RRD_Unit_Price IS NOT NULL And RRD.RRD_V_Coy_Id in " & _
                           " (select RRM_V_Company_ID from  RFQ_REPLIES_MSTR  where RRM_RFQ_ID = '" & Common.Parse(rfq_id) & "' "

                End If


                If chk_compare = 1 Then
                    strsql = strsql & " and RRM_Indicator = 0) "
                Else
                    strsql = strsql & ")"
                End If

                strsql = strsql & " And CM.CM_COY_ID = RRD.RRD_V_Coy_Id And RRM.RRM_V_Company_ID = RRD.RRD_V_Coy_Id " & _
                        " AND RRD.RRD_Line_No in ( SELECT RD_RFQ_Line FROM RFQ_DETAIL"
                Dim i As Integer

                For i = 0 To len - 1
                    If i > 0 Then
                        strsql = strsql & " or RD_RFQ_Line = '" & Common.Parse(array(i)) & "' "
                    Else
                        strsql = strsql & " WHERE RD_RFQ_Line = '" & Common.Parse(array(i)) & "'  "
                    End If
                Next



                If chk = "1" Then
                    'strsql = strsql & " ) GROUP BY CM.CM_COY_NAME, RRD.RRD_V_Coy_Id, RRM_Actual_Quot_Num,RRM_RFQ_ID," & _
                    '                " RRM_Currency_Code,RRM_Offer_Till,RRM_V_Company_ID ORDER BY RRM_TotalValue ASC "
                    strsql = strsql & ") GROUP BY CM.CM_COY_NAME, RRD.RRD_V_Coy_Id, RRM_Actual_Quot_Num,RRM_RFQ_ID, RRM_Currency_Code,RRM_Offer_Till,RRM_V_Company_ID ) a " &
                            "LEFT JOIN (SELECT TAX_CODE,TAX_PERC,TAX_COUNTRY_CODE FROM CODE_MSTR " &
                            "INNER JOIN TAX ON TAX_CODE = CODE_ABBR " &
                            "WHERE CODE_CATEGORY = 'SST' AND CODE_DELETED = 'N') b " &
                            "ON b.TAX_CODE = a.RRD_GST_RATE AND b.TAX_COUNTRY_CODE = a.CM_COUNTRY ORDER BY a.RRM_TotalValue ASC"

                ElseIf chk = "2" Then
                    'Michelle (24/2/2011) - To group by company
                    If compLevel Then
                        strsql = strsql & " ) GROUP BY CM.CM_COY_NAME ORDER BY RRM_TotalValue ASC "
                    Else
                        strsql = strsql & " ) GROUP BY CM.CM_COY_NAME, RRD_Unit_Price,RRD.RRD_V_Coy_Id, RRM_Actual_Quot_Num,RRM_RFQ_ID," & _
                                                            " RRM_Currency_Code,RRM_Offer_Till,RRM_V_Company_ID ORDER BY RRM_TotalValue ASC "
                    End If

                    End If


            End If


            Dim dscath As New DataSet
            dscath = objDb.FillDs(strsql)
            get_itemrank = dscath


        End Function

        Public Function get_Price(ByVal rfq_id As String, ByVal line_no As Integer, ByVal scoy_id As String) As DataSet
            Dim strsql As String
            Dim dscath As New DataSet

            '2015-06-24: CH: Rounding issue (Prod issue)
            'strsql = "SELECT RRD_Unit_Price,RRD_GST,(RRD_Unit_Price*RRD_Quantity+((RRD_GST*RRD_Unit_Price*RRD_Quantity)/100)) AS Price " _
            '        & "FROM rfq_replies_detail " _
            '        & "WHERE RRD_RFQ_ID=" & rfq_id & " AND RRD_V_Coy_Id='" & scoy_id & "' AND RRD_Line_No=" & line_no
            strsql = "SELECT RRD_Unit_Price,RRD_GST,ROUND(RRD_Unit_Price * RRD_Quantity, 2) + ROUND((RRD_GST * ROUND(RRD_Unit_Price * RRD_Quantity,2))/100, 2) AS Price " _
                    & "FROM rfq_replies_detail " _
                    & "WHERE RRD_RFQ_ID=" & rfq_id & " AND RRD_V_Coy_Id='" & scoy_id & "' AND RRD_Line_No=" & line_no

            dscath = objDb.FillDs(strsql)
            get_Price = dscath

        End Function

        Public Function get_PriceGST(ByVal rfq_id As String, ByVal line_no As Integer, ByVal scoy_id As String) As DataSet
            Dim strsql As String
            Dim dscath As New DataSet

            '2015-07-03: CH: Rounding issue (Prod issue)
            'strsql = "SELECT a.RRD_Unit_Price,a.RRD_GST_RATE,a.RRD_Quantity,b.TAX_PERC,IF(b.TAX_PERC IS NULL OR b.TAX_PERC='',(a.RRD_Unit_Price*a.RRD_Quantity),(a.RRD_Unit_Price*a.RRD_Quantity+((b.TAX_PERC*a.RRD_Unit_Price*a.RRD_Quantity)/100))) AS Price  FROM " _
            '        & "(SELECT RRD_UNIT_PRICE,RRD_GST_RATE,RRD_QUANTITY " _
            '        & "FROM RFQ_REPLIES_DETAIL WHERE RRD_RFQ_ID=" & rfq_id & " AND RRD_V_Coy_Id='" & scoy_id & "' AND RRD_Line_No=" & line_no & ") a " _
            '        & "LEFT JOIN " _
            '        & "(SELECT TAX_CODE,TAX_PERC FROM CODE_MSTR " _
            '        & "INNER JOIN TAX ON TAX_CODE = CODE_ABBR " _
            '        & "INNER JOIN COMPANY_MSTR ON CM_COY_ID ='" & scoy_id & "' " _
            '        & "WHERE CODE_CATEGORY = 'GST' AND CODE_DELETED = 'N' AND TAX_COUNTRY_CODE = CM_COUNTRY ) b " _
            '        & "ON b.TAX_CODE = a.RRD_GST_RATE "
            strsql = "SELECT a.RRD_Unit_Price,a.RRD_GST_RATE,a.RRD_Quantity,b.TAX_PERC,IF(b.TAX_PERC IS NULL OR b.TAX_PERC='',ROUND(a.RRD_Unit_Price*a.RRD_Quantity,2),(ROUND(a.RRD_Unit_Price*a.RRD_Quantity,2)+ROUND((b.TAX_PERC*ROUND(a.RRD_Unit_Price*a.RRD_Quantity,2))/100,2))) AS Price FROM " _
                    & "(SELECT RRD_UNIT_PRICE,RRD_GST_RATE,RRD_QUANTITY " _
                    & "FROM RFQ_REPLIES_DETAIL WHERE RRD_RFQ_ID=" & rfq_id & " AND RRD_V_Coy_Id='" & scoy_id & "' AND RRD_Line_No=" & line_no & ") a " _
                    & "LEFT JOIN " _
                    & "(SELECT TAX_CODE,TAX_PERC FROM CODE_MSTR " _
                    & "INNER JOIN TAX ON TAX_CODE = CODE_ABBR " _
                    & "INNER JOIN COMPANY_MSTR ON CM_COY_ID ='" & scoy_id & "' " _
                    & "WHERE CODE_CATEGORY = 'SST' AND CODE_DELETED = 'N' AND TAX_COUNTRY_CODE = CM_COUNTRY ) b " _
                    & "ON b.TAX_CODE = a.RRD_GST_RATE "

            dscath = objDb.FillDs(strsql)
            get_PriceGST = dscath

        End Function

        Public Function get_QuoTotalValue(ByVal rfq_id As String, ByVal scoy_id As String) As String
            Dim strsql As String
            Dim dscath As New DataSet

            strsql = "SELECT RRM_TotalValue FROM rfq_replies_mstr " _
                    & "WHERE RRM_RFQ_ID=" & rfq_id & " AND RRM_V_Company_ID='" & scoy_id & "'"

            dscath = objDb.FillDs(strsql)
            If dscath.Tables(0).Rows.Count > 0 Then
                get_QuoTotalValue = Format(dscath.Tables(0).Rows(0).Item(0), "0.0000")
            Else
                get_QuoTotalValue = "0.0000"
            End If

        End Function

        Public Function delete_trash_VEN(ByVal deletetrash As Integer, ByVal trash As Integer)

            Dim strsql As String = "update RFQ_VENDOR_MSTR set RVM_V_Display_Status='" & Common.Parse(deletetrash) & "' where RVM_V_Display_Status='" & Common.Parse(trash) & "' and RVM_V_Company_ID='" & Common.Parse(HttpContext.Current.Session("UserId")) & "'"
            objDb.Execute(strsql)
        End Function
        Public Function get_itemdis(ByVal rfq_id As String) As DataSet
            Dim strsql As String = "select Distinct RD_Product_Desc,RD_RFQ_Line,RD_Quantity,RD_UOM from RFQ_DETAIL where RD_RFQ_ID='" & Common.Parse(rfq_id) & " ' ORDER BY RD_RFQ_Line"

            Dim dscath As New DataSet
            dscath = objDb.FillDs(strsql)
            get_itemdis = dscath

        End Function

        Public Function check_v_status(ByVal rfq_id As String) As Boolean

            Dim strsql As String = "select RVM_V_RFQ_Status from RFQ_VENDOR_MSTR where " & _
            " RVM_RFQ_ID='" & Common.Parse(rfq_id) & "'"
            Dim tDS As DataSet = objDb.FillDs(strsql)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                If tDS.Tables(0).Rows(j).Item("RVM_V_RFQ_Status") <> 0 Then ' response 
                    check_v_status = 1
                End If
            Next
        End Function

        Public Function get_quoteVen(ByVal RFQ_ID As String) As DataSet
            Dim dscath As New DataSet
            Dim strsql As String
            '//Remark By Moo (24/02/2005) - take out And RM.RM_B_Display_Status = '0' and RM_Status=" & RFQStatus.quote & 
            '//User can still view Response even RFQ in Trash folder
            '//User can still view Response if RFQ status = Reply,PR
            'strsql = " SELECT Distinct  RM.RM_RFQ_ID,RM.RM_RFQ_Name,RM.RM_RFQ_No,RM.RM_Created_On," & _
            '                " CM.CM_COY_NAME,RVM_V_Company_ID,RM.RM_Currency_Code, RVM.RVM_V_Company_ID,RM.RM_RFQ_OPTION,RM.RM_Reqd_Quote_Validity,RM.RM_Expiry_Date," & _
            '          "RM.RM_VEN_DISTR_LIST_INDEX,RRM_Currency_Code,RRM.RRM_Actual_Quot_Num, RRM.RRM_Offer_Till,RRM_RFQ_ID, RRM_V_Company_ID,RRM.RRM_TotalValue,RRM.RRM_Indicator" & _
            '            " FROM COMPANY_MSTR CM, RFQ_MSTR RM left join RFQ_VENDOR_MSTR RVM on RM.RM_RFQ_ID=RVM.RVM_RFQ_ID	" & _
            '            " left join RFQ_REPLIES_MSTR RRM on RRM.RRM_RFQ_ID=RVM.RVM_RFQ_ID and " & _
            '            " RRM.RRM_V_Company_ID = RVM.RVM_V_Company_ID" & _
            '            " WHERE RM.RM_RFQ_ID='" & RFQ_ID & "'" & _
            '            " And (RM.RM_Status <>3 )" & _
            '            " And RM.RM_RFQ_No is not null" & _
            '            " and CM.CM_COY_ID=RVM.RVM_V_Company_ID"
            '' " And RM.RM_B_Display_Status = '0' and RM_Status=" & RFQStatus.quote & " " & _

            strsql = "SELECT COUNT(RM.RM_RFQ_ID) AS CNT " _
                    & "FROM COMPANY_MSTR CM, RFQ_MSTR RM " _
                    & "LEFT JOIN RFQ_VENDOR_MSTR RVM ON RM.RM_RFQ_ID=RVM.RVM_RFQ_ID " _
                    & "LEFT JOIN RFQ_REPLIES_MSTR RRM ON RRM.RRM_RFQ_ID=RVM.RVM_RFQ_ID AND  RRM.RRM_V_Company_ID = RVM.RVM_V_Company_ID " _
                    & "WHERE RM.RM_RFQ_ID = '" & RFQ_ID & "' And RM.RM_Status <> 3 And RM.RM_RFQ_No Is Not NULL " _
                    & "AND CM.CM_COY_ID=RVM.RVM_V_Company_ID AND RRM.RRM_TotalValue IS NULL"
            dscath = objDb.FillDs(strsql)
            If dscath.Tables(0).Rows.Count > 0 Then
                If dscath.Tables(0).Rows(0).Item(0) > 0 Then    ''No quote' found
                    strsql = "(SELECT DISTINCT 1 AS sort_col, RM.RM_RFQ_ID,RM.RM_RFQ_Name,RM.RM_RFQ_No,RM.RM_Created_On, CM.CM_COY_NAME," _
                            & "RVM_V_Company_ID,RM.RM_Currency_Code, RVM.RVM_V_Company_ID,RM.RM_RFQ_OPTION,RM.RM_Reqd_Quote_Validity," _
                            & "RM.RM_Expiry_Date,RM.RM_VEN_DISTR_LIST_INDEX,RRM_Currency_Code,RRM.RRM_Actual_Quot_Num, RRM.RRM_Offer_Till," _
                            & "RRM_RFQ_ID, RRM_V_Company_ID, RRM.RRM_TotalValue, RRM.RRM_Indicator, RVM.RVM_V_RFQ_STATUS " _
                            & "FROM COMPANY_MSTR CM, RFQ_MSTR RM " _
                            & "LEFT JOIN RFQ_VENDOR_MSTR RVM ON RM.RM_RFQ_ID=RVM.RVM_RFQ_ID " _
                            & "LEFT JOIN RFQ_REPLIES_MSTR RRM ON RRM.RRM_RFQ_ID=RVM.RVM_RFQ_ID AND RRM.RRM_V_Company_ID = RVM.RVM_V_Company_ID " _
                            & "WHERE RM.RM_RFQ_ID = '" & RFQ_ID & "' And RM.RM_Status <> 3 And RM.RM_RFQ_No Is Not NULL " _
                            & "AND CM.CM_COY_ID=RVM.RVM_V_Company_ID AND RRM.RRM_TotalValue IS NOT NULL ORDER BY RRM.RRM_TotalValue) " _
                            & "UNION " _
                            & "(SELECT DISTINCT  2, RM.RM_RFQ_ID,RM.RM_RFQ_Name,RM.RM_RFQ_No,RM.RM_Created_On, CM.CM_COY_NAME," _
                            & "RVM_V_Company_ID,RM.RM_Currency_Code, RVM.RVM_V_Company_ID,RM.RM_RFQ_OPTION,RM.RM_Reqd_Quote_Validity," _
                            & "RM.RM_Expiry_Date,RM.RM_VEN_DISTR_LIST_INDEX,RRM_Currency_Code,RRM.RRM_Actual_Quot_Num, RRM.RRM_Offer_Till," _
                            & "RRM_RFQ_ID, RRM_V_Company_ID, RRM.RRM_TotalValue, RRM.RRM_Indicator, RVM.RVM_V_RFQ_STATUS " _
                            & "FROM COMPANY_MSTR CM, RFQ_MSTR RM " _
                            & "LEFT JOIN RFQ_VENDOR_MSTR RVM ON RM.RM_RFQ_ID=RVM.RVM_RFQ_ID " _
                            & "LEFT JOIN RFQ_REPLIES_MSTR RRM ON RRM.RRM_RFQ_ID=RVM.RVM_RFQ_ID AND  RRM.RRM_V_Company_ID = RVM.RVM_V_Company_ID " _
                            & "WHERE RM.RM_RFQ_ID = '" & RFQ_ID & "' And RM.RM_Status <> 3 And RM.RM_RFQ_No Is Not NULL " _
                            & "AND CM.CM_COY_ID=RVM.RVM_V_Company_ID AND RRM.RRM_TotalValue IS NULL) " _
                            & "ORDER BY sort_col , RRM_TotalValue"
                Else
                    strsql = "SELECT DISTINCT RM.RM_RFQ_ID,RM.RM_RFQ_Name,RM.RM_RFQ_No,RM.RM_Created_On, CM.CM_COY_NAME," _
                            & "RVM_V_Company_ID,RM.RM_Currency_Code, RVM.RVM_V_Company_ID,RM.RM_RFQ_OPTION,RM.RM_Reqd_Quote_Validity," _
                            & "RM.RM_Expiry_Date,RM.RM_VEN_DISTR_LIST_INDEX,RRM_Currency_Code,RRM.RRM_Actual_Quot_Num, RRM.RRM_Offer_Till," _
                            & "RRM_RFQ_ID, RRM_V_Company_ID, RRM.RRM_TotalValue, RRM.RRM_Indicator, RVM.RVM_V_RFQ_STATUS " _
                            & "FROM COMPANY_MSTR CM, RFQ_MSTR RM " _
                            & "LEFT JOIN RFQ_VENDOR_MSTR RVM ON RM.RM_RFQ_ID=RVM.RVM_RFQ_ID " _
                            & "LEFT JOIN RFQ_REPLIES_MSTR RRM ON RRM.RRM_RFQ_ID=RVM.RVM_RFQ_ID AND RRM.RRM_V_Company_ID = RVM.RVM_V_Company_ID " _
                            & "WHERE RM.RM_RFQ_ID = '" & RFQ_ID & "' And RM.RM_Status <> 3 And RM.RM_RFQ_No Is Not NULL " _
                            & "AND CM.CM_COY_ID=RVM.RVM_V_Company_ID ORDER BY RRM.RRM_TotalValue"

                End If
            End If

            dscath = objDb.FillDs(strsql)
            get_quoteVen = dscath
        End Function

        Public Function GetOutstandingRFQList(ByVal Doc_num As String) As DataSet
            Dim ds As DataSet
            Dim strsql As String
            Dim strTemp As String

            strsql = "SELECT RM_RFQ_ID, RM_RFQ_No, RM_RFQ_Name, RM_CREATED_ON," _
                & "RM_EXPIRY_DATE,RM_Status,RM_RFQ_OPTION " _
                & "FROM RFQ_MSTR " _
                & "WHERE RM_EXPIRY_DATE >= CURRENT_DATE() " _
                & "AND RM_B_Display_Status='0' " _
                & "AND RM_Created_By='" & HttpContext.Current.Session("UserID") & "' " _
                & "AND RM_Coy_ID='" & HttpContext.Current.Session("CompanyID") & "' " _
                & "AND RM_RFQ_ID NOT IN " _
                & "(SELECT POM_RFQ_INDEX FROM PO_MSTR WHERE POM_RFQ_INDEX IS NOT NULL AND POM_B_COY_ID ='" & HttpContext.Current.Session("CompanyID") & "')"

            If Doc_num <> "" Then
                strTemp = Common.BuildWildCard(Doc_num)
                strsql = strsql & " AND RM_RFQ_No " & Common.ParseSQL(strTemp)
            End If

            strsql = strsql & "UNION SELECT RM_RFQ_ID, RM_RFQ_No, RM_RFQ_Name, RM_CREATED_ON," _
                & "RM_EXPIRY_DATE,RM_Status,RM_RFQ_OPTION " _
                & "FROM RFQ_MSTR " _
                & "WHERE RM_EXPIRY_DATE < CURRENT_DATE() " _
                & "AND RM_STATUS = 3 " _
                & "AND RM_B_Display_Status='0' " _
                & "AND RM_Created_By='" & HttpContext.Current.Session("UserID") & "' " _
                & "AND RM_Coy_ID='" & HttpContext.Current.Session("CompanyID") & "' " _
                & "AND RM_RFQ_ID NOT IN " _
                & "(SELECT POM_RFQ_INDEX FROM PO_MSTR WHERE POM_RFQ_INDEX IS NOT NULL AND POM_B_COY_ID ='" & HttpContext.Current.Session("CompanyID") & "')"


            If Doc_num <> "" Then
                strTemp = Common.BuildWildCard(Doc_num)
                strsql = strsql & " AND RM_RFQ_No " & Common.ParseSQL(strTemp)
            End If
            ds = objDb.FillDs(strsql)

            Return ds
        End Function

        Public Function GetOutstandingRFQListWithVendor(ByVal Doc_num As String, ByVal VenName As String) As DataSet
            Dim ds As DataSet
            Dim strsql As String
            Dim strTemp As String
            strsql = "SELECT DISTINCT RM_RFQ_ID, RM_RFQ_No , RM_RFQ_Name , RM_RFQ_OPTION," _
                    & "RM_CREATED_ON, RM_EXPIRY_DATE, RM_Status " _
                    & "FROM RFQ_MSTR LEFT JOIN RFQ_INVITED_VENLIST ON RIV_RFQ_ID = RM_RFQ_ID " _
                    & "WHERE RM_B_Display_Status='0' " _
                    & "And RM_EXPIRY_DATE >= CURRENT_DATE() " _
                    & "AND RM_Created_By='" & HttpContext.Current.Session("UserID") & "' " _
                    & "AND RM_Coy_ID='" & HttpContext.Current.Session("CompanyID") & "' " _
                    & "AND RM_RFQ_ID NOT IN " _
                    & "(SELECT POM_RFQ_INDEX FROM PO_MSTR FORCE INDEX(IDX_POM_B_COY_ID) WHERE POM_RFQ_INDEX IS NOT NULL AND POM_B_COY_ID ='" & HttpContext.Current.Session("CompanyID") & "')"

            If Doc_num <> "" Then
                strTemp = Common.BuildWildCard(Doc_num)
                strsql = strsql & " AND RM_RFQ_No " & Common.ParseSQL(strTemp)
            End If

            If VenName <> "" Then
                strTemp = Common.BuildWildCard(VenName)
                strsql = strsql & " AND RIV_S_Coy_Name" & Common.ParseSQL(strTemp)
                'strsql = strsql & " AND RIV_S_Coy_Name " & Common.ParseSQL(VenName)
            End If

            strsql = strsql & " UNION SELECT DISTINCT RM_RFQ_ID, RM_RFQ_No , RM_RFQ_Name , RM_RFQ_OPTION," _
                    & "RM_CREATED_ON, RM_EXPIRY_DATE, RM_Status " _
                    & "FROM RFQ_MSTR LEFT JOIN RFQ_INVITED_VENLIST ON RIV_RFQ_ID = RM_RFQ_ID " _
                    & "WHERE RM_B_Display_Status='0' " _
                    & "AND RM_EXPIRY_DATE < CURRENT_DATE() " _
                    & "AND RM_STATUS = 3 " _
                    & "AND RM_Created_By='" & HttpContext.Current.Session("UserID") & "' " _
                    & "AND RM_Coy_ID='" & HttpContext.Current.Session("CompanyID") & "' " _
                    & "AND RM_RFQ_ID NOT IN " _
                    & "(SELECT POM_RFQ_INDEX FROM PO_MSTR FORCE INDEX(IDX_POM_B_COY_ID) WHERE POM_RFQ_INDEX IS NOT NULL AND POM_B_COY_ID ='" & HttpContext.Current.Session("CompanyID") & "')"

            If Doc_num <> "" Then
                strTemp = Common.BuildWildCard(Doc_num)
                strsql = strsql & " AND RM_RFQ_No " & Common.ParseSQL(strTemp)
            End If

            If VenName <> "" Then
                strTemp = Common.BuildWildCard(VenName)
                strsql = strsql & " AND RIV_S_Coy_Name" & Common.ParseSQL(strTemp)
                'strsql = strsql & " AND RIV_S_Coy_Name " & Common.ParseSQL(VenName)
            End If


            ds = objDb.FillDs(strsql)

            Return ds
        End Function

        Public Function GetQuoteListAll(ByVal Doc_num As String) As DataSet
            Dim ds As DataSet
            Dim strsql As String
            Dim strTemp As String
            strsql = " SELECT Distinct  RM.RM_RFQ_ID,RM.RM_RFQ_Name,RM.RM_RFQ_No,RM.RM_Created_On," & _
                        " CM.CM_COY_NAME,RVM_V_Company_ID,RM.RM_Currency_Code, RVM.RVM_V_Company_ID,RM.RM_RFQ_OPTION,RM.RM_Reqd_Quote_Validity,RM.RM_Expiry_Date," & _
                        "RM.RM_VEN_DISTR_LIST_INDEX,RRM_Currency_Code,RRM.RRM_Actual_Quot_Num, RRM.RRM_Offer_Till,RRM_RFQ_ID, RRM_V_Company_ID,RRM.RRM_TotalValue,RRM.RRM_Indicator" & _
                        " FROM COMPANY_MSTR CM, RFQ_MSTR RM INNER join RFQ_VENDOR_MSTR RVM on RM.RM_RFQ_ID=RVM.RVM_RFQ_ID	" & _
                        " INNER join RFQ_REPLIES_MSTR RRM on RRM.RRM_RFQ_ID=RVM.RVM_RFQ_ID and " & _
                        " RRM.RRM_V_Company_ID = RVM.RVM_V_Company_ID" & _
                        " WHERE RM.RM_Coy_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
                        " AND RM.RM_Created_By = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " & _
                        " And (RM.RM_Status>='1' and RM.RM_Status<='2')" & _
                        " And RM.RM_RFQ_No is not null" & _
                        " And RM.RM_B_Display_Status='0' " & _
                        " and CM.CM_COY_ID=RVM.RVM_V_Company_ID AND CM.CM_STATUS = 'A' "

            If Doc_num <> "" Then
                strTemp = Common.BuildWildCard(Doc_num)
                strsql = strsql & " AND (RM_RFQ_No " & Common.ParseSQL(strTemp) & " OR RRM.RRM_Actual_Quot_Num " & Common.ParseSQL(strTemp) & ")"
            End If
            ds = objDb.FillDs(strsql)

            Return ds
        End Function

        Public Function GetQuoteListWithVendor(ByVal Doc_num As String, ByVal VenName As String) As DataSet
            Dim ds As DataSet
            Dim strsql As String
            Dim strTemp As String
            strsql = " SELECT Distinct  RM.RM_RFQ_ID,RM.RM_RFQ_Name,RM.RM_RFQ_No,RM.RM_Created_On," & _
                        " CM.CM_COY_NAME,RVM_V_Company_ID,RM.RM_Currency_Code, RVM.RVM_V_Company_ID,RM.RM_RFQ_OPTION,RM.RM_Reqd_Quote_Validity,RM.RM_Expiry_Date," & _
                        "RM.RM_VEN_DISTR_LIST_INDEX,RRM_Currency_Code,RRM.RRM_Actual_Quot_Num, RRM.RRM_Offer_Till,RRM_RFQ_ID, RRM_V_Company_ID,RRM.RRM_TotalValue,RRM.RRM_Indicator" & _
                        " FROM COMPANY_MSTR CM, RFQ_MSTR RM INNER join RFQ_VENDOR_MSTR RVM on RM.RM_RFQ_ID=RVM.RVM_RFQ_ID	" & _
                        " INNER join RFQ_REPLIES_MSTR RRM on RRM.RRM_RFQ_ID=RVM.RVM_RFQ_ID and " & _
                        " RRM.RRM_V_Company_ID = RVM.RVM_V_Company_ID" & _
                        " WHERE RM.RM_Coy_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
                        " AND RM.RM_Created_By = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " & _
                        " And (RM.RM_Status>='1' and RM.RM_Status<='2')" & _
                        " And RM.RM_RFQ_No is not null" & _
                        " And RM.RM_B_Display_Status='0' " & _
                        " and CM.CM_COY_ID=RVM.RVM_V_Company_ID AND CM.CM_STATUS = 'A' "

            If Doc_num <> "" Then
                strTemp = Common.BuildWildCard(Doc_num)
                strsql = strsql & " AND (RM_RFQ_No " & Common.ParseSQL(strTemp) & " OR RRM.RRM_Actual_Quot_Num " & Common.ParseSQL(strTemp) & ")"
            End If

            If VenName <> "" Then
                strTemp = Common.BuildWildCard(VenName)
                strsql = strsql & " AND CM_COY_NAME" & Common.ParseSQL(strTemp)
            End If

            ds = objDb.FillDs(strsql)

            Return ds
        End Function

        'Public Function GetRFQListAll(ByVal Doc_num As String) As DataSet
        '    Dim ds As DataSet
        '    Dim strsql As String
        '    'strsql = "SELECT RM_RFQ_ID, RM_RFQ_No, RM_RFQ_Name, RRM_Actual_Quot_Num,RM_CREATED_ON," _
        '    '        & "RM_EXPIRY_DATE,RM_Status,RM_RFQ_OPTION " _
        '    '        & "FROM RFQ_MSTR " _
        '    '        & "LEFT JOIN rfq_replies_mstr ON rfq_replies_mstr.RRM_RFQ_ID=RFQ_MSTR.RM_RFQ_ID " _
        '    '        & "WHERE RM_EXPIRY_DATE >= CURRENT_DATE() AND RM_B_Display_Status='0' " _
        '    '        & "AND RM_Created_By='" & HttpContext.Current.Session("UserID") & "' " _
        '    '        & "AND RM_Coy_ID='" & HttpContext.Current.Session("CompanyID") & "'"
        '    strsql = "SELECT DISTINCT RM.RM_RFQ_Name, RM.RM_Status,RM.RM_RFQ_ID,RM.RM_RFQ_No,RM.RM_Coy_ID,RM.RM_CREATED_ON," _
        '            & "RM.RM_EXPIRY_DATE, RM.RM_B_Display_Status, RM_RFQ_OPTION " _
        '            & "FROM RFQ_MSTR RM, RFQ_INVITED_VENLIST, COMPANY_MSTR " _
        '            & "WHERE RM_EXPIRY_DATE >= CURRENT_DATE() AND RM.RM_Created_By='" & HttpContext.Current.Session("UserID") & "' " _
        '            & "AND RM.RM_B_Display_Status=0 AND RM.RM_Coy_ID='" & HttpContext.Current.Session("CompanyID") & "' " _
        '            & "AND RIV_S_COY_ID = CM_COY_ID AND CM_DELETED = 'N' AND CM_STATUS = 'A'"
        '    If Doc_num <> "" Then
        '        strsql = strsql & " AND RM_RFQ_No " & Common.ParseSQL(Doc_num)
        '    End If
        '    ds = objDb.FillDs(strsql)

        '    Return ds
        'End Function

        Public Function GetRFQListAllWithVendor(ByVal Doc_num As String, ByVal VenName As String, ByVal Valid As String, ByVal Expired As String, Optional ByVal startdate As String = "", Optional ByVal enddate As String = "") As DataSet
            Dim ds As DataSet
            Dim strsql As String
            Dim strTemp As String
            'strsql = "SELECT RM_RFQ_ID, RM_RFQ_No, RM_RFQ_Name, RRM_Actual_Quot_Num,RM_CREATED_ON," _
            '        & "RM_EXPIRY_DATE,RM_Status,RM_RFQ_OPTION " _
            '        & "FROM RFQ_MSTR " _
            '        & "LEFT JOIN rfq_replies_mstr ON rfq_replies_mstr.RRM_RFQ_ID=RFQ_MSTR.RM_RFQ_ID " _
            '        & "WHERE RM_EXPIRY_DATE >= CURRENT_DATE() AND RM_B_Display_Status='0' " _
            '        & "AND RM_Created_By='" & HttpContext.Current.Session("UserID") & "' " _
            '        & "AND RM_Coy_ID='" & HttpContext.Current.Session("CompanyID") & "'"
            'strsql = "SELECT DISTINCT RM.RM_RFQ_Name, RM.RM_Status,RM.RM_RFQ_ID,RM.RM_RFQ_No,RM.RM_Coy_ID,RM.RM_CREATED_ON," _
            '        & "RM.RM_EXPIRY_DATE, RM.RM_B_Display_Status, RM_RFQ_OPTION " _
            '        & "FROM RFQ_MSTR RM, RFQ_INVITED_VENLIST, COMPANY_MSTR " _
            '        & "WHERE RM_EXPIRY_DATE >= CURRENT_DATE() AND RM.RM_Created_By='" & HttpContext.Current.Session("UserID") & "' " _
            '        & "AND RM.RM_B_Display_Status=0 AND RM.RM_Coy_ID='" & HttpContext.Current.Session("CompanyID") & "' " _
            '        & "AND RIV_S_COY_ID = CM_COY_ID AND CM_DELETED = 'N' AND CM_STATUS = 'A'"

            'Modified by Joon on 04 Apr 2011
            'strsql = "SELECT DISTINCT RM.RM_RFQ_Name, RM.RM_Status,RM.RM_RFQ_ID,RM.RM_RFQ_No,RM.RM_Coy_ID,RM.RM_CREATED_ON," _
            '        & "RM.RM_EXPIRY_DATE, RM.RM_B_Display_Status, RM_RFQ_OPTION " _
            '        & "FROM RFQ_MSTR RM, RFQ_INVITED_VENLIST, COMPANY_MSTR " _
            '        & "WHERE RM.RM_Created_By='" & HttpContext.Current.Session("UserID") & "' AND RM.RM_B_Display_Status=0 " _
            '        & "AND RM.RM_Coy_ID='" & HttpContext.Current.Session("CompanyID") & "' AND RIV_S_COY_ID = CM_COY_ID AND CM_DELETED = 'N' AND CM_STATUS = 'A' " _
            '        & "AND RM.RM_RFQ_ID NOT IN " _
            '        & "(SELECT RM_RFQ_ID " _
            '        & "FROM RFQ_MSTR " _
            '        & "WHERE RM_EXPIRY_DATE >= CURRENT_DATE() AND RM_B_Display_Status='0' " _
            '        & "AND RM_Created_By='" & HttpContext.Current.Session("UserID") & "' " _
            '        & "AND RM_Coy_ID='" & HttpContext.Current.Session("CompanyID") & "' " _
            '        & "AND RM_RFQ_ID NOT IN (SELECT POM_RFQ_INDEX FROM PO_MSTR WHERE POM_RFQ_INDEX IS NOT NULL))"

            'Modified by Joon on 29 Apr 2011 / Modified by Craven on 15 dec 2011 (exclude the vendor list)
            ''''strsql = "SELECT XXX.RM_RFQ_Name, XXX.RM_Status, XXX.RM_RFQ_ID, XXX.RM_RFQ_No, XXX.RM_Coy_ID, XXX.RM_CREATED_ON, XXX.RM_EXPIRY_DATE, XXX.RM_B_Display_Status, XXX.RM_RFQ_OPTION, XXX.RM_Reqd_Quote_Validity, XXX.STAT FROM (SELECT DISTINCT RM.RM_RFQ_Name, RM.RM_Status,RM.RM_RFQ_ID,RM.RM_RFQ_No,RM.RM_Coy_ID,RM.RM_CREATED_ON," _
            ''''        & "RM.RM_EXPIRY_DATE, RM.RM_B_Display_Status, RM_RFQ_OPTION,RM_Reqd_Quote_Validity, 0 AS STAT " _
            ''''        & "FROM RFQ_MSTR RM INNER JOIN COMPANY_MSTR " _
            ''''        & "LEFT JOIN RFQ_INVITED_VENLIST ON RIV_RFQ_ID = RM.RM_RFQ_ID WHERE " _
            ''''        & "RM.RM_STATUS <> 3 AND RM.RM_Created_By='" & HttpContext.Current.Session("UserID") & "' AND RM.RM_B_Display_Status=0 " _
            ''''        & "AND RM.RM_Coy_ID='" & HttpContext.Current.Session("CompanyID") & "' AND CM_DELETED = 'N' AND CM_STATUS = 'A' " _
            ''''        & "AND RM.RM_RFQ_ID NOT IN " _
            ''''        & "(SELECT RM_RFQ_ID " _
            ''''        & "FROM RFQ_MSTR " _
            ''''        & "WHERE RM.RM_EXPIRY_DATE >= CURRENT_DATE() AND RM_B_Display_Status='0' " _
            ''''        & "AND RM_Created_By='" & HttpContext.Current.Session("UserID") & "' " _
            ''''        & "AND RM_Coy_ID='" & HttpContext.Current.Session("CompanyID") & "' " _
            ''''        & "AND RM_RFQ_ID NOT IN (SELECT POM_RFQ_INDEX FROM PO_MSTR WHERE POM_RFQ_INDEX IS NOT NULL)) "

            ''''If Doc_num <> "" Then
            ''''    strTemp = Common.BuildWildCard(Doc_num)
            ''''    strsql = strsql & " AND RM_RFQ_No " & Common.ParseSQL(strTemp)
            ''''End If
            ''''If VenName <> "" Then
            ''''    strTemp = Common.BuildWildCard(VenName)
            ''''    strsql = strsql & " AND RIV_S_Coy_Name" & Common.ParseSQL(strTemp)
            ''''End If
            ''''If (Valid <> "" And Expired <> "") Then
            ''''Else
            ''''    If Valid <> "" Then
            ''''        strsql = strsql & " AND RM_EXPIRY_DATE >= CURRENT_DATE() "
            ''''    End If
            ''''    If Expired <> "" Then
            ''''        strsql = strsql & " AND RM_EXPIRY_DATE < CURRENT_DATE() "
            ''''    End If
            ''''End If

            ''''strsql = strsql & " ) XXX WHERE XXX.RM_RFQ_ID IN (SELECT POM_RFQ_INDEX FROM PO_MSTR WHERE POM_RFQ_INDEX IS NOT NULL AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "')"

            ''''strsql = strsql & " UNION SELECT ZZZ.RM_RFQ_Name, ZZZ.RM_Status, ZZZ.RM_RFQ_ID, ZZZ.RM_RFQ_No, ZZZ.RM_Coy_ID, ZZZ.RM_CREATED_ON, ZZZ.RM_EXPIRY_DATE, ZZZ.RM_B_Display_Status, ZZZ.RM_RFQ_OPTION, ZZZ.RM_Reqd_Quote_Validity, ZZZ.STAT FROM (SELECT DISTINCT RM.RM_RFQ_Name, RM.RM_Status,RM.RM_RFQ_ID,RM.RM_RFQ_No,RM.RM_Coy_ID,RM.RM_CREATED_ON," _
            ''''        & "RM.RM_EXPIRY_DATE, RM.RM_B_Display_Status, RM_RFQ_OPTION,RM_Reqd_Quote_Validity, 1 AS STAT " _
            ''''        & "FROM RFQ_MSTR RM INNER JOIN COMPANY_MSTR " _
            ''''        & "LEFT JOIN RFQ_INVITED_VENLIST ON RIV_RFQ_ID = RM.RM_RFQ_ID WHERE " _
            ''''        & "RM.RM_STATUS <> 3 AND  RM.RM_Created_By='" & HttpContext.Current.Session("UserID") & "' AND RM.RM_B_Display_Status=0 " _
            ''''        & "AND RM.RM_Coy_ID='" & HttpContext.Current.Session("CompanyID") & "' AND CM_DELETED = 'N' AND CM_STATUS = 'A' " _
            ''''        & "AND RM.RM_RFQ_ID NOT IN " _
            ''''        & "(SELECT RM_RFQ_ID " _
            ''''        & "FROM RFQ_MSTR " _
            ''''        & "WHERE RM.RM_EXPIRY_DATE >= CURRENT_DATE() AND RM_B_Display_Status='0' " _
            ''''        & "AND RM_Created_By='" & HttpContext.Current.Session("UserID") & "' " _
            ''''        & "AND RM_Coy_ID='" & HttpContext.Current.Session("CompanyID") & "' " _
            ''''        & "AND RM_RFQ_ID NOT IN (SELECT POM_RFQ_INDEX FROM PO_MSTR WHERE POM_RFQ_INDEX IS NOT NULL)) "

            ''''If Doc_num <> "" Then
            ''''    strTemp = Common.BuildWildCard(Doc_num)
            ''''    strsql = strsql & " AND RM_RFQ_No " & Common.ParseSQL(strTemp)
            ''''End If
            ''''If VenName <> "" Then
            ''''    strTemp = Common.BuildWildCard(VenName)
            ''''    strsql = strsql & " AND RIV_S_Coy_Name" & Common.ParseSQL(strTemp)
            ''''End If
            ''''If (Valid <> "" And Expired <> "") Then
            ''''Else
            ''''    If Valid <> "" Then
            ''''        strsql = strsql & " AND RM_EXPIRY_DATE >= CURRENT_DATE() "
            ''''    End If
            ''''    If Expired <> "" Then
            ''''        strsql = strsql & " AND RM_EXPIRY_DATE < CURRENT_DATE() "
            ''''    End If
            ''''End If

            ''''strsql = strsql & " ) ZZZ WHERE ZZZ.RM_RFQ_ID NOT IN (SELECT POM_RFQ_INDEX FROM PO_MSTR WHERE POM_RFQ_INDEX IS NOT NULL AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "')"

            strsql = "SELECT DISTINCT RM.RM_RFQ_Name, RM.RM_Status,RM.RM_RFQ_ID,RM.RM_RFQ_No,RM.RM_Coy_ID,RM.RM_CREATED_ON," _
                    & "RM.RM_EXPIRY_DATE, RM.RM_B_Display_Status, RM_RFQ_OPTION,RM_Reqd_Quote_Validity, 0 AS STAT " _
                    & "FROM RFQ_MSTR RM INNER JOIN COMPANY_MSTR " _
                    & "LEFT JOIN RFQ_INVITED_VENLIST ON RIV_RFQ_ID = RM.RM_RFQ_ID WHERE " _
                    & "RM.RM_STATUS <> 3 AND RM.RM_Created_By='" & HttpContext.Current.Session("UserID") & "' AND RM.RM_B_Display_Status=0 " _
                    & "AND RM.RM_Coy_ID='" & HttpContext.Current.Session("CompanyID") & "' AND CM_DELETED = 'N' AND CM_STATUS = 'A' " _
                    & "AND RM.RM_RFQ_ID IN (SELECT POM_RFQ_INDEX FROM PO_MSTR FORCE INDEX(idx_po_mstr2) WHERE POM_RFQ_INDEX IS NOT NULL AND " _
                    & "POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "') "

            If Doc_num <> "" Then
                strTemp = Common.BuildWildCard(Doc_num)
                strsql = strsql & " AND RM_RFQ_No " & Common.ParseSQL(strTemp)
            End If
            If VenName <> "" Then
                strTemp = Common.BuildWildCard(VenName)
                strsql = strsql & " AND RIV_S_Coy_Name" & Common.ParseSQL(strTemp)
            End If
            If (Valid <> "" And Expired <> "") Then
            Else
                If Valid <> "" Then
                    strsql = strsql & " AND RM_EXPIRY_DATE >= CURRENT_DATE() "
                End If
                If Expired <> "" Then
                    strsql = strsql & " AND RM_EXPIRY_DATE < CURRENT_DATE() "
                End If
            End If

            If startdate <> "" Then
                strsql &= "AND RM.RM_CREATED_ON >= " & Common.ConvertDate(startdate) & " "
            End If

            If enddate <> "" Then
                strsql &= "AND RM.RM_CREATED_ON <= " & Common.ConvertDate(enddate & " 23:59:59.000") & " "
            End If

            strsql = strsql & "UNION SELECT DISTINCT RM.RM_RFQ_Name, RM.RM_Status,RM.RM_RFQ_ID,RM.RM_RFQ_No,RM.RM_Coy_ID,RM.RM_CREATED_ON," _
                    & "RM.RM_EXPIRY_DATE, RM.RM_B_Display_Status, RM_RFQ_OPTION,RM_Reqd_Quote_Validity, 0 AS STAT " _
                    & "FROM RFQ_MSTR RM INNER JOIN COMPANY_MSTR " _
                    & "LEFT JOIN RFQ_INVITED_VENLIST ON RIV_RFQ_ID = RM.RM_RFQ_ID WHERE " _
                    & "RM.RM_STATUS <> 3 AND ((RM.RM_RFQ_OPTION = 1 AND RM.RM_EXPIRY_DATE < CURRENT_DATE()) OR (RM.RM_RFQ_OPTION = 0 AND RM.RM_EXPIRY_DATE < CURRENT_DATE())) AND RM.RM_Created_By='" & HttpContext.Current.Session("UserID") & "' AND RM.RM_B_Display_Status=0 " _
                    & "AND RM.RM_Coy_ID='" & HttpContext.Current.Session("CompanyID") & "' AND CM_DELETED = 'N' AND CM_STATUS = 'A' "

            If Doc_num <> "" Then
                strTemp = Common.BuildWildCard(Doc_num)
                strsql = strsql & " AND RM_RFQ_No " & Common.ParseSQL(strTemp)
            End If
            If VenName <> "" Then
                strTemp = Common.BuildWildCard(VenName)
                strsql = strsql & " AND RIV_S_Coy_Name" & Common.ParseSQL(strTemp)
            End If
            If (Valid <> "" And Expired <> "") Then
            Else
                If Valid <> "" Then
                    strsql = strsql & " AND RM_EXPIRY_DATE >= CURRENT_DATE() "
                End If
                If Expired <> "" Then
                    strsql = strsql & " AND RM_EXPIRY_DATE < CURRENT_DATE() "
                End If
            End If

            If startdate <> "" Then
                strsql &= "AND RM.RM_CREATED_ON >= " & Common.ConvertDate(startdate) & " "
            End If

            If enddate <> "" Then
                strsql &= "AND RM.RM_CREATED_ON <= " & Common.ConvertDate(enddate & " 23:59:59.000") & " "
            End If

            ds = objDb.FillDs(strsql)

            Return ds
        End Function

        Public Function get_RFQList(ByVal Doc_num As String, ByVal VenName As String, ByVal Status_start As String, ByVal Status_end As String, ByVal B_Status As String, ByVal folder As String) As DataSet

            Dim strsql As String
            Dim strTemp As String

            If folder = "0" Then '//sent

                strsql = "select Distinct RM.RM_RFQ_Name, RM.RM_Status,RM.RM_RFQ_ID,RM.RM_RFQ_No,RM.RM_Coy_ID,RM.RM_Created_On," & _
                                        " RM.RM_Expiry_Date, RM.RM_B_Display_Status ,RM_RFQ_OPTION  " & _
                                         " FROM RFQ_MSTR RM, RFQ_INVITED_VENLIST, COMPANY_MSTR " & _
                                        " WHERE  (RM.RM_Status>='" & Common.Parse(Status_start) & "' and RM.RM_Status<='" & Common.Parse(Status_end) & "') " & _
                                        " and RM.RM_Created_By='" & Common.Parse(HttpContext.Current.Session("UserId")) & "' and " & _
                                        " RM.RM_B_Display_Status='" & Common.Parse(B_Status) & "' and RM.RM_Coy_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
                                        " AND RIV_S_COY_ID = CM_COY_ID AND CM_DELETED = 'N' AND CM_STATUS = 'A' "

            ElseIf folder = "1" Then '//draft
                ', RM.RM_VEN_DISTR_LIST_INDEX
                ' Michelle (CR0055) - Remove RFQ_VEN_DISTR_LIST_MSTR DIS as it is not being referenced
                'strsql = "select Distinct RM.RM_RFQ_Name, RM.RM_Status,RM.RM_RFQ_ID,RM.RM_RFQ_No,RM.RM_Coy_ID,RM.RM_Created_On," & _
                '         " RM.RM_Expiry_Date, RM.RM_B_Display_Status, RM.RM_VEN_DISTR_LIST_INDEX " & _
                '         " FROM RFQ_MSTR RM,RFQ_VEN_DISTR_LIST_MSTR DIS,RFQ_INVITED_VENLIST, COMPANY_MSTR " & _
                '         " WHERE  (RM.RM_Status>='" & Common.Parse(Status_start) & "' and RM.RM_Status<='" & Common.Parse(Status_end) & "') and RM.RM_B_Display_Status='" & Common.Parse(B_Status) & "' " & _
                '         " and  RM.RM_Created_By='" & Common.Parse(HttpContext.Current.Session("UserId")) & "' and RM.RM_Coy_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
                '         " AND RIV_S_COY_ID = CM_COY_ID AND CM_DELETED = 'N' AND CM_STATUS = 'A' "

                strsql = "select Distinct RM.RM_RFQ_Name, RM.RM_Status,RM.RM_RFQ_ID,RM.RM_RFQ_No,RM.RM_Coy_ID,RM.RM_Created_On," & _
                         " RM.RM_Expiry_Date, RM.RM_B_Display_Status, RM.RM_VEN_DISTR_LIST_INDEX " & _
                         " FROM RFQ_MSTR RM, RFQ_INVITED_VENLIST, COMPANY_MSTR " & _
                         " WHERE  (RM.RM_Status>='" & Common.Parse(Status_start) & "' and RM.RM_Status<='" & Common.Parse(Status_end) & "') and RM.RM_B_Display_Status='" & Common.Parse(B_Status) & "' " & _
                         " and  RM.RM_Created_By='" & Common.Parse(HttpContext.Current.Session("UserId")) & "' and RM.RM_Coy_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
                         " AND RIV_S_COY_ID = CM_COY_ID AND CM_DELETED = 'N' AND CM_STATUS = 'A' "

            ElseIf folder = "2" Then '//quote
                '//Remark By Moo (24/02/2005)
                'strsql = " SELECT Distinct  RM.RM_RFQ_ID,RM.RM_RFQ_Name,RM.RM_RFQ_No,RM.RM_Created_On," & _
                '        " CM.CM_COY_NAME,RVM_V_Company_ID,RM.RM_Currency_Code, RVM.RVM_V_Company_ID,RM.RM_RFQ_OPTION,RM.RM_Reqd_Quote_Validity,RM.RM_Expiry_Date," & _
                '        "RM.RM_VEN_DISTR_LIST_INDEX,RRM_Currency_Code,RRM.RRM_Actual_Quot_Num, RRM.RRM_Offer_Till,RRM_RFQ_ID, RRM_V_Company_ID,RRM.RRM_TotalValue,RRM.RRM_Indicator" & _
                '        " FROM COMPANY_MSTR CM, RFQ_MSTR RM left join RFQ_VENDOR_MSTR RVM on RM.RM_RFQ_ID=RVM.RVM_RFQ_ID	" & _
                '        " left join RFQ_REPLIES_MSTR RRM on RRM.RRM_RFQ_ID=RVM.RVM_RFQ_ID and " & _
                '        " RRM.RRM_V_Company_ID = RVM.RVM_V_Company_ID" & _
                '        " WHERE RM.RM_Coy_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
                '        " AND RM.RM_Created_By = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " & _
                '        " And (RM.RM_Status <>3 )" & _
                '        " And RM.RM_RFQ_No is not null" & _
                '        " And RM.RM_B_Display_Status = '0' and RM_Status=" & RFQStatus.quote & " " & _
                '        " and CM.CM_COY_ID=RVM.RVM_V_Company_ID"

                strsql = " SELECT Distinct  RM.RM_RFQ_ID,RM.RM_RFQ_Name,RM.RM_RFQ_No,RM.RM_Created_On," & _
                        " CM.CM_COY_NAME,RVM_V_Company_ID,RM.RM_Currency_Code, RVM.RVM_V_Company_ID,RM.RM_RFQ_OPTION,RM.RM_Reqd_Quote_Validity,RM.RM_Expiry_Date," & _
                        "RM.RM_VEN_DISTR_LIST_INDEX,RRM_Currency_Code,RRM.RRM_Actual_Quot_Num, RRM.RRM_Offer_Till,RRM_RFQ_ID, RRM_V_Company_ID,RRM.RRM_TotalValue,RRM.RRM_Indicator" & _
                        " FROM COMPANY_MSTR CM, RFQ_MSTR RM left join RFQ_VENDOR_MSTR RVM on RM.RM_RFQ_ID=RVM.RVM_RFQ_ID	" & _
                        " left join RFQ_REPLIES_MSTR RRM on RRM.RRM_RFQ_ID=RVM.RVM_RFQ_ID and " & _
                        " RRM.RRM_V_Company_ID = RVM.RVM_V_Company_ID" & _
                        " WHERE RM.RM_Coy_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
                        " AND RM.RM_Created_By = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " & _
                        " And (RM.RM_Status>='" & Common.Parse(Status_start) & "' and RM.RM_Status<='" & Common.Parse(Status_end) & "')" & _
                        " And RM.RM_RFQ_No is not null" & _
                        " And RM.RM_B_Display_Status='" & Common.Parse(B_Status) & "' " & _
                        " and CM.CM_COY_ID=RVM.RVM_V_Company_ID AND CM.CM_STATUS = 'A' "

                If VenName <> "" Then
                    strTemp = Common.BuildWildCard(VenName)
                    strsql = strsql & " and CM_COY_NAME" & Common.ParseSQL(strTemp)
                End If


            ElseIf folder = "3" Then '//trash

                strsql = "select distinct RM.RM_RFQ_Name, RM.RM_Status,RM.RM_RFQ_ID,RM.RM_RFQ_No,RM.RM_Coy_ID,RM.RM_Created_On," & _
                                                       " RM.RM_Expiry_Date,RM.RM_Status, RM.RM_B_Display_Status," & _
                                                        " RM.RM_VEN_DISTR_LIST_INDEX" & _
                                                       " FROM RFQ_MSTR RM ,RFQ_INVITED_VENLIST, COMPANY_MSTR " & _
                                                       " WHERE  (RM.RM_Status>='" & Common.Parse(Status_start) & "' and RM.RM_Status<='" & Common.Parse(Status_end) & "') and " & _
                                                       " RM.RM_B_Display_Status='" & Common.Parse(B_Status) & "' and RM.RM_Coy_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
                                                       " and RM.RM_Created_By='" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " & _
                                                       " AND RIV_S_COY_ID = CM_COY_ID AND CM_DELETED = 'N' AND CM_STATUS = 'A' "
            End If


            If Doc_num <> "" Then
                strTemp = Common.BuildWildCard(Doc_num)
                strsql = strsql & " AND RM.RM_RFQ_No " & Common.ParseSQL(strTemp)
            End If

            If folder <> "2" Then


                If VenName <> "" Then
                    strTemp = Common.BuildWildCard(VenName)
                    strsql = strsql & " and  RM_RFQ_ID = RIV_RFQ_ID AND RIV_S_Coy_Name " & Common.ParseSQL(strTemp)
                End If
            End If

            Dim dscath As New DataSet
            dscath = objDb.FillDs(strsql)
            get_RFQList = dscath

        End Function

        Public Function GET_VENLISTCOM(ByVal list_no As String) As DataSet
            Dim DS As DataSet

            Dim strsql As String = "SELECT CM_COY_NAME," & _
                                    "CM_ADDR_LINE1,CM_ADDR_LINE2,CM_ADDR_LINE3,CM_POSTCODE," & _
                                    "CM_EMAIL,CM_PHONE,CM_COUNTRY,CM_STATE,CM_CITY from COMPANY_MSTR, RFQ_VEN_DISTR_LIST_DETAIL  WHERE  " & _
                                    " RVDLD_LIST_INDEX='" & Common.Parse(list_no) & "'  and CM_COY_ID = RCDLD_V_Coy_ID AND CM_STATUS = 'A' "

            DS = objDb.FillDs(strsql)

            Return DS

        End Function

        Public Function rfq_venlist(ByVal list_no As String, ByVal rfq_name As String, Optional ByVal rfq_id As String = "") As DataSet
            Dim ds As DataSet

            Dim strsql As String = "SELECT CM_COY_ID,CM_COY_NAME," & _
                          "CM_ADDR_LINE1,CM_ADDR_LINE2,CM_ADDR_LINE3,CM_POSTCODE," & _
                          "CM_EMAIL,CM_PHONE,CM_COUNTRY,CM_STATE,CM_CITY  from COMPANY_MSTR, RFQ_INVITED_VENLIST_DETAIL  WHERE CM_COY_ID=RTVDT_v_company_id AND RTVDT_RFQ_Name='" & Common.Parse(rfq_name) & "'" & _
                          " AND RTVDT_Distribution_List_Id='" & Common.Parse(list_no) & "' AND RTVDT_User_Id='" & Common.Parse(HttpContext.Current.Session("UserId")) & "'"

            If rfq_id <> "" Then
                strsql = strsql & " AND RTVDT_RFQ_ID = '" & rfq_id & "' "
            End If

            ds = objDb.FillDs(strsql)
            Return ds

        End Function

        Public Function rfq_COMMSTR(ByVal ITEM As RFQ_User)


            Dim strsql As String = "SELECT CM_COY_NAME,CM_BUSINESS_REG_NO," & _
                          "CM_ADDR_LINE1,CM_ADDR_LINE2,CM_ADDR_LINE3,CM_POSTCODE," & _
                          "CM_EMAIL,CM_PHONE,CM_COUNTRY,CM_STATE,CM_CITY  " & _
                          " from COMPANY_MSTR  WHERE  CM_COY_ID='" & ITEM.V_com_ID & "'"
            Dim tDS As DataSet = objDb.FillDs(strsql)

            If tDS.Tables(0).Rows.Count > 0 Then
                ITEM.vendor_name = tDS.Tables(0).Rows(0).Item("CM_COY_NAME").ToString.Trim
                ITEM.REG_NO = tDS.Tables(0).Rows(0).Item("CM_BUSINESS_REG_NO").ToString.Trim
                ITEM.addsline1 = tDS.Tables(0).Rows(0).Item("CM_ADDR_LINE1").ToString.Trim
                ITEM.addsline2 = tDS.Tables(0).Rows(0).Item("CM_ADDR_LINE2").ToString.Trim
                ITEM.addsline3 = tDS.Tables(0).Rows(0).Item("CM_ADDR_LINE3").ToString.Trim
                ITEM.postcode = tDS.Tables(0).Rows(0).Item("CM_POSTCODE").ToString.Trim
                ITEM.city = tDS.Tables(0).Rows(0).Item("CM_CITY").ToString.Trim
                '   ITEM.vendor_person = tDS.Tables(0).Rows(0).Item("RRM_Contact_Person").ToString.Trim
                ITEM.vendor_Con_num = tDS.Tables(0).Rows(0).Item("CM_PHONE").ToString.Trim
                ITEM.vendor_email = tDS.Tables(0).Rows(0).Item("CM_EMAIL").ToString.Trim
                ITEM.state = tDS.Tables(0).Rows(0).Item("CM_STATE").ToString.Trim
                ITEM.country = tDS.Tables(0).Rows(0).Item("CM_COUNTRY").ToString.Trim
            End If

        End Function

        Public Function venaprovchk(ByVal RFQ_INDEX As String) As String
            Dim strsql As String = "SELECT RIV_S_Coy_ID, RIV_S_Coy_Name from RFQ_INVITED_VENLIST WHERE RIV_RFQ_ID='" & Common.Parse(RFQ_INDEX) & "'" & _
            " and RIV_S_Coy_ID not in(select CV_S_COY_ID from COMPANY_VENDOR where CV_B_COY_ID='" & HttpContext.Current.Session("CompanyId") & "')"
            Dim tDS As DataSet = objDb.FillDs(strsql)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                If venaprovchk = "" Then
                    'venaprovchk = READ("RIV_S_Coy_ID").ToString.Trim
                    venaprovchk = tDS.Tables(0).Rows(j).Item("RIV_S_Coy_Name").ToString.Trim
                Else
                    'venaprovchk = venaprovchk & "," & READ("RIV_S_Coy_ID").ToString.Trim
                    venaprovchk = venaprovchk & "," & tDS.Tables(0).Rows(j).Item("RIV_S_Coy_Name").ToString.Trim
                End If
            Next
        End Function
        Public Function rfq_COMNAME(ByVal RFQ_INDEX As String, ByVal com_id As String) As DataSet
            Dim ds As DataSet

            Dim strsql As String = "SELECT RIV_S_Coy_ID,RIV_S_Coy_Name,RIV_S_Addr_Line1,RIV_S_Addr_Line2,RIV_S_Addr_Line3,RIV_S_PostCode" & _
                          " ,RIV_S_City,RIV_S_State,RIV_S_Country,RIV_S_Email,RIV_S_Phone" & _
                          "  from RFQ_INVITED_VENLIST WHERE RIV_RFQ_ID='" & Common.Parse(RFQ_INDEX) & "' "
            If com_id <> "" Then
                strsql = strsql & " and RIV_S_Coy_ID='" & com_id & "'"
            End If

            ds = objDb.FillDs(strsql)
            Return ds

        End Function

        Public Function rfq_polist(ByVal RFQ_No As String, ByVal RFQ_ID As String) As DataSet
            Dim ds As DataSet
            Dim strsql As String

            strsql = " SELECT '" & RFQ_No & "' AS PRD_CONVERT_TO_DOC, " & _
                    " PO_MSTR.POM_PO_NO, PO_MSTR.POM_PO_DATE , PO_MSTR.POM_S_COY_NAME, " & _
                    " (SELECT CODE_DESC FROM code_mstr WHERE code_abbr = PO_MSTR.POM_CURRENCY_CODE AND CODE_CATEGORY = 'CU' AND CODE_DELETED = 'N') AS POM_CURRENCY_CODE, " & _
                    " (SELECT STATUS_DESC FROM STATUS_MSTR WHERE STATUS_TYPE = 'PO' AND STATUS_NO = PO_MSTR.POM_PO_STATUS) AS POM_PO_STATUS, " & _
                    " '' AS PRD_PR_NO " & _
                    " FROM PO_MSTR, PO_DETAILS " & _
                    " WHERE(POM_B_COY_ID = POD_COY_ID And POM_PO_NO = POD_PO_NO) " & _
                    " AND POM_RFQ_INDEX = '" & RFQ_ID & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " & _
                    " GROUP BY PRD_CONVERT_TO_DOC, POM_PO_NO, POM_PO_DATE, POM_S_COY_NAME, POM_CURRENCY_CODE, POM_PO_STATUS, PRD_PR_NO"

            ds = objDb.FillDs(strsql)
            Return ds

        End Function

        Public Function GetPR(ByVal strRFQNo As String, ByRef RFQNo() As String, ByRef COUNT As Integer)
            COUNT = 0
            Dim strSQL As String

            strSQL = "SELECT PR_DETAILS.PRD_PR_NO FROM PR_DETAILS WHERE PRD_CONVERT_TO_DOC = '" & strRFQNo & "' AND " _
                & "PRD_CONVERT_TO_IND = 'RFQ' AND PRD_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " _
                & "GROUP BY PR_DETAILS.PRD_PR_NO "


            Dim tDS As DataSet = objDb.FillDs(strSQL)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                RFQNo(COUNT) = tDS.Tables(0).Rows(j).Item("PRD_PR_NO")
                COUNT = COUNT + 1
            Next
        End Function

        Public Function rfq_venDetails(ByVal item As RFQ_User) As String
            ''select * from RFQ_REPLIES_MSTR ,RFQ_MSTR,COMPANY_MSTR WHERE 
            ''RM_RFQ_NO='RFQ-04-139' AND RRM_RFQ_ID=RM_RFQ_ID AND
            ''RRM_V_COMPANY_ID ='ARIELEC' AND CM_COY_ID ='ARIELEC'

            Dim strsql As String = "SELECT distinct RIV_S_Coy_Name," & _
                          " RIV_S_Addr_Line1,RIV_S_Addr_Line2,RIV_S_Addr_Line3,RIV_S_PostCode " & _
                          " ,RIV_S_City,RIV_S_State,RIV_S_Country, " & _
                          " RRM_Contact_Person, RRM_Contact_Number, RRM_Email from RFQ_REPLIES_MSTR ,RFQ_MSTR,RFQ_INVITED_VENLIST WHERE " & _
                          " RM_RFQ_NO='" & Common.Parse(item.RFQ_Num) & "' and RRM_RFQ_ID= RM_RFQ_ID AND RRM_V_Company_ID='" & Common.Parse(item.V_com_ID) & "'" & _
                          " and RIV_RFQ_ID=RM_RFQ_ID and RIV_S_Coy_ID='" & Common.Parse(item.V_com_ID) & "'  "

            Dim tDS As DataSet = objDb.FillDs(strsql)
            If tDS.Tables(0).Rows.Count > 0 Then
                item.vendor_name = tDS.Tables(0).Rows(0).Item("RIV_S_Coy_Name").ToString.Trim
                ' item.vendor_Addr = read("Adds").ToString.Trim

                item.vendor_person = tDS.Tables(0).Rows(0).Item("RRM_Contact_Person").ToString.Trim
                item.addsline1 = tDS.Tables(0).Rows(0).Item("RIV_S_Addr_Line1").ToString.Trim
                item.addsline2 = tDS.Tables(0).Rows(0).Item("RIV_S_Addr_Line2").ToString.Trim
                item.addsline3 = tDS.Tables(0).Rows(0).Item("RIV_S_Addr_Line3").ToString.Trim
                item.postcode = tDS.Tables(0).Rows(0).Item("RIV_S_PostCode").ToString.Trim
                item.city = tDS.Tables(0).Rows(0).Item("RIV_S_City").ToString.Trim

                item.vendor_Con_num = tDS.Tables(0).Rows(0).Item("RRM_Contact_Number").ToString.Trim
                item.vendor_email = tDS.Tables(0).Rows(0).Item("RRM_Email").ToString.Trim
                item.state = tDS.Tables(0).Rows(0).Item("RIV_S_State").ToString.Trim
                item.country = tDS.Tables(0).Rows(0).Item("RIV_S_Country").ToString.Trim
            Else
                rfq_venDetails = "1"
            End If

            ' ds = objDb.FillDs(strsql)
            ' Return ds

        End Function

        Public Function get_buyerInfo(ByVal rfq_num As String, ByVal com_name As String, ByVal v_DisStatus As String, ByVal V_RFQ_Status As String) As DataSet

            'Dim strsql As String = "select RM.RM_RFQ_Name,RM.RM_Status,RM.RM_RFQ_ID,RM.RM_RFQ_No,RM.RM_Coy_ID,RM.RM_Created_On,RM.RM_Expiry_Date,RM.RM_B_Display_Status,CM.CM_COY_NAME,RVM.RVM_V_RFQ_Status" & _
            '                        " FROM RFQ_MSTR RM,RFQ_VENDOR_MSTR RVM,COMPANY_MSTR CM ,RFQ_REPLIES_MSTR WHERE " & _
            '                        " RM.RM_Status<>'3' AND RRM_Created_By='" & Common.Parse(HttpContext.Current.Session("UserID")) & "' and  RVM.RVM_V_Company_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'" & _
            '                        " and RRM_V_Company_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'" & _
            '                        " and RM.RM_RFQ_ID=RRM_RFQ_ID AND RVM_RFQ_ID=RRM_RFQ_ID and  CM.CM_COY_ID=RM.RM_Coy_ID" & _
            '                       " and RVM.RVM_V_Display_Status=' " & v_DisStatus & " ' "


            Dim strsql As String = "select RM.RM_RFQ_Name,RM.RM_Status,RM.RM_RFQ_ID,RM.RM_RFQ_No,RM.RM_Coy_ID,RM.RM_Created_On,RM.RM_Expiry_Date,RM.RM_B_Display_Status,CM.CM_COY_NAME,RVM.RVM_V_RFQ_Status" & _
                               " FROM RFQ_MSTR RM,RFQ_VENDOR_MSTR RVM,COMPANY_MSTR CM WHERE " & _
                               " RM.RM_Status<>'3' AND RVM.RVM_V_Company_ID='" & HttpContext.Current.Session("CompanyId") & "'" & _
                               " and RM.RM_RFQ_ID=RVM.RVM_RFQ_ID AND CM.CM_COY_ID=RM.RM_Coy_ID" & _
                              " and RVM.RVM_V_Display_Status='" & v_DisStatus & "' "

            If V_RFQ_Status = "1" Then
                strsql = strsql & " AND RVM.RVM_V_RFQ_Status = '" & Common.Parse(Common.Parse(V_RFQ_Status)) & "' "
            End If
            If rfq_num <> "" Then
                strsql = strsql & " AND RM.RM_RFQ_No" & Common.ParseSQL(rfq_num)
            End If

            If com_name <> "" Then
                strsql = strsql & " AND CM.CM_COY_NAME " & Common.ParseSQL(com_name)
            End If
            Dim dscath As New DataSet
            dscath = objDb.FillDs(strsql)
            get_buyerInfo = dscath

        End Function

        Public Function get_buyerInfo2(ByVal rfq_num As String, ByVal com_name As String, ByVal v_DisStatus As String, ByVal V_RFQ_Status As String) As DataSet

            'Dim strsql As String = "select RM.RM_RFQ_Name,RM.RM_Status,RM.RM_RFQ_ID,RM.RM_RFQ_No,RM.RM_Coy_ID,RM.RM_Created_On,RM.RM_Expiry_Date,RM.RM_B_Display_Status,CM.CM_COY_NAME,RVM.RVM_V_RFQ_Status" & _
            '                        " FROM RFQ_MSTR RM,RFQ_VENDOR_MSTR RVM,COMPANY_MSTR CM ,RFQ_REPLIES_MSTR WHERE " & _
            '                        " RM.RM_Status<>'3' AND RRM_Created_By='" & Common.Parse(HttpContext.Current.Session("UserID")) & "' and  RVM.RVM_V_Company_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'" & _
            '                        " and RRM_V_Company_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'" & _
            '                        " and RM.RM_RFQ_ID=RRM_RFQ_ID AND RVM_RFQ_ID=RRM_RFQ_ID and  CM.CM_COY_ID=RM.RM_Coy_ID" & _
            '                       " and RVM.RVM_V_Display_Status=' " & v_DisStatus & " ' "


            'Dim strsql As String = "select RM.RM_RFQ_Name,RM.RM_Status,RM.RM_RFQ_ID,RM.RM_RFQ_No,RM.RM_Coy_ID,RM.RM_Created_On,RM.RM_Expiry_Date,RM.RM_B_Display_Status,CM.CM_COY_NAME,RVM.RVM_V_RFQ_Status" & _
            '                   " FROM RFQ_MSTR RM,RFQ_VENDOR_MSTR RVM,COMPANY_MSTR CM WHERE " & _
            '                   " RM.RM_Status<>'3' AND RVM.RVM_V_Company_ID='" & HttpContext.Current.Session("CompanyId") & "'" & _
            '                   " and RM.RM_RFQ_ID=RVM.RVM_RFQ_ID AND CM.CM_COY_ID=RM.RM_Coy_ID" & _
            '                  " and RVM.RVM_V_Display_Status='" & v_DisStatus & "' "

            'Michelle (13/1/2011) - Issue 1354
            'Dim strsql As String = "SELECT RM.RM_RFQ_Name AS 'RFQ Name',RM.RM_RFQ_ID,RM.RM_RFQ_No AS 'RFQ Number', " _
            '                & "RM.RM_Created_On AS 'Creation Date',RM.RM_Expiry_Date AS 'Expiry Date',CM.CM_COY_NAME AS 'Buyer Company'," _
            '                & "RM_Coy_ID FROM RFQ_MSTR RM,RFQ_VENDOR_MSTR RVM,COMPANY_MSTR CM WHERE  RM.RM_Status<>'3' " _
            '                & "AND RVM.RVM_V_Company_ID='" & HttpContext.Current.Session("CompanyID") & "' AND  RM.RM_RFQ_ID=RVM.RVM_RFQ_ID AND CM.CM_COY_ID=RM.RM_Coy_ID " _
            '                & "AND RVM.RVM_V_Display_Status='" & v_DisStatus & "' " _
            '                & "AND RM.RM_RFQ_ID NOT IN " _
            '                & "(SELECT RRM_RFQ_ID FROM RFQ_REPLIES_MSTR WHERE RRM_V_COMPANY_ID = '" & HttpContext.Current.Session("CompanyId") & "')"

            Dim strsql As String = "SELECT RM.RM_RFQ_Name AS 'RFQ Name',RM.RM_RFQ_ID,RM.RM_RFQ_No AS 'RFQ Number', " _
                & "RM.RM_Created_On AS 'Creation Date',RM.RM_Expiry_Date AS 'Expiry Date',CM.CM_COY_NAME AS 'Buyer Company'," _
                & "RM_Coy_ID, RM_B_DISPLAY_STATUS FROM RFQ_MSTR RM,RFQ_VENDOR_MSTR RVM,COMPANY_MSTR CM WHERE  RM.RM_Status<>'3' " _
                & "AND RVM.RVM_V_Company_ID='" & HttpContext.Current.Session("CompanyID") & "' AND  RM.RM_RFQ_ID=RVM.RVM_RFQ_ID AND CM.CM_COY_ID=RM.RM_Coy_ID " _
                & "AND RVM.RVM_V_Display_Status='" & v_DisStatus & "' AND RM.RM_EXPIRY_DATE >= GETDATE() " _
                & "AND (RM.RM_RFQ_ID NOT IN " _
                & "(SELECT RRM_RFQ_ID FROM RFQ_REPLIES_MSTR WHERE RRM_V_COMPANY_ID = '" & HttpContext.Current.Session("CompanyId") & "') AND RVM.RVM_V_RFQ_STATUS = 0) "

            If V_RFQ_Status = "1" Then
                strsql = strsql & " AND RVM.RVM_V_RFQ_Status = '" & Common.Parse(Common.Parse(V_RFQ_Status)) & "' "
            End If
            If rfq_num <> "" Then
                strsql = strsql & " AND RM.RM_RFQ_No" & Common.ParseSQL(rfq_num)
            End If

            If com_name <> "" Then
                strsql = strsql & " AND CM.CM_COY_NAME " & Common.ParseSQL(com_name)
            End If
            Dim dscath As New DataSet
            dscath = objDb.FillDs(strsql)
            get_buyerInfo2 = dscath

        End Function
        Public Function get_RFQExp(ByVal rfq_num As String, ByVal com_name As String, ByVal v_DisStatus As String) As DataSet
            Dim strTemp As String = ""
            Dim strsql As String = "SELECT RM.RM_RFQ_Name AS 'RFQ Name',RM.RM_RFQ_ID,RM.RM_RFQ_No AS 'RFQ Number', " _
                & "RM.RM_Created_On AS 'Creation Date',RM.RM_Expiry_Date AS 'Expiry Date',CM.CM_COY_NAME AS 'Buyer Company'," _
                & "RM_Coy_ID, RM.RM_B_DISPLAY_STATUS, RVM_V_RFQ_STATUS FROM RFQ_MSTR RM,RFQ_VENDOR_MSTR RVM,COMPANY_MSTR CM WHERE  RM.RM_Status<>'3' " _
                & "AND RVM.RVM_V_Company_ID='" & HttpContext.Current.Session("CompanyID") & "' AND  RM.RM_RFQ_ID=RVM.RVM_RFQ_ID AND CM.CM_COY_ID=RM.RM_Coy_ID " _
                & "AND RM.RM_EXPIRY_DATE < GETDATE() AND RVM.RVM_V_Display_Status='0' " _
                & "AND RM.RM_RFQ_ID NOT IN " _
                & "(SELECT RRM_RFQ_ID FROM RFQ_REPLIES_MSTR WHERE RRM_V_COMPANY_ID = '" & HttpContext.Current.Session("CompanyId") & "')"

            If rfq_num <> "" Then
                strTemp = Common.BuildWildCard(rfq_num)
                strsql = strsql & " AND RM.RM_RFQ_No" & Common.ParseSQL(strTemp)
            End If

            If com_name <> "" Then
                strTemp = Common.BuildWildCard(com_name)
                strsql = strsql & " AND CM.CM_COY_NAME " & Common.ParseSQL(strTemp)
            End If

            strsql = strsql & "UNION ALL SELECT RM.RM_RFQ_Name AS 'RFQ Name',RM.RM_RFQ_ID,RM.RM_RFQ_No AS 'RFQ Number', " _
                & "RM.RM_Created_On AS 'Creation Date',RM.RM_Expiry_Date AS 'Expiry Date',CM.CM_COY_NAME AS 'Buyer Company'," _
                & "RM_Coy_ID, RM.RM_B_DISPLAY_STATUS, RVM_V_RFQ_STATUS FROM RFQ_MSTR RM,RFQ_VENDOR_MSTR RVM,COMPANY_MSTR CM WHERE  RM.RM_Status<>'3' " _
                & "AND RVM.RVM_V_Company_ID='" & HttpContext.Current.Session("CompanyID") & "' AND  RM.RM_RFQ_ID=RVM.RVM_RFQ_ID AND CM.CM_COY_ID=RM.RM_Coy_ID " _
                & "AND RM.RM_EXPIRY_DATE >= GETDATE() AND RVM.RVM_V_Display_Status='0' " _
                & "AND RVM_V_RFQ_STATUS = 1 "

            If rfq_num <> "" Then
                strTemp = Common.BuildWildCard(rfq_num)
                strsql = strsql & " AND RM.RM_RFQ_No" & Common.ParseSQL(strTemp)
            End If

            If com_name <> "" Then
                strTemp = Common.BuildWildCard(com_name)
                strsql = strsql & " AND CM.CM_COY_NAME " & Common.ParseSQL(strTemp)
            End If

            Dim dscath As New DataSet
            dscath = objDb.FillDs(strsql)
            get_RFQExp = dscath

        End Function

        Public Function get_rfqMstr(ByVal item As RFQ_User, ByVal RFQ_ID As String)
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
                item.del_code = Common.parseNull(tDS.Tables(0).Rows(0).Item("RM_DEL_CODE"))

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
                    strsql = "INSERT INTO RFQ_REPLIES_DETAIL_TEMP(RRDT_RFQ_ID,RRDT_V_Company_ID,RRDT_Line_No,RRDT_Product_ID,RRDT_Product_Name,RRDT_Quantity,RRDT_Unit_Price,RRDT_GST_Code,RRDT_GST,RRDT_GST_Desc,RRDT_GST_RATE,RRDT_Product_Desc,RRDT_UOM,RRDT_Delivery_Lead_Time,RRDT_Warranty_Terms,RRDT_Min_Pack_Qty,RRDT_Min_Order_Qty,RRDT_Remarks,RRDT_Tolerance,RRDT_DEL_CODE,RRDT_ITEM_TYPE)" & _
                    "SELECT RRD_RFQ_ID,RRD_V_Coy_Id,RRD_Line_No,RRD_Product_Code,RRD_Vendor_Item_Code,RRD_Quantity,RRD_Unit_Price,RRD_GST_Code,RRD_GST,RRD_GST_Desc,RRD_GST_RATE,RRD_Product_Desc,RRD_UOM,RRD_Delivery_Lead_Time,RRD_Warranty_Terms,RRD_Min_Pack_Qty,RRD_Min_Order_Qty,RRD_Remarks,RRD_Tolerance,RRD_DEL_CODE,RRD_ITEM_TYPE " & _
                    "FROM RFQ_REPLIES_DETAIL WHERE RRD_RFQ_ID='" & Common.Parse(RFQ_ID) & "' AND RRD_V_Coy_Id='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'"
                    Common.Insert2Ary(strAry, strsql)
                Else
                    'Michelle (3/10/2010) - To insert a space before the From statement
                    'strsql = "INSERT INTO RFQ_REPLIES_DETAIL_TEMP(RRDT_RFQ_ID,RRDT_V_Company_ID,RRDT_Line_No,RRDT_Product_ID,RRDT_Product_Name,RRDT_Quantity,RRDT_Unit_Price,RRDT_GST_Code,RRDT_GST,RRDT_GST_Desc,RRDT_Product_Desc,RRDT_UOM,RRDT_Delivery_Lead_Time,RRDT_Warranty_Terms,RRDT_Min_Pack_Qty,RRDT_Min_Order_Qty,RRDT_Remarks,RRDT_Tolerance)" & _
                    '"SELECT RD_RFQ_ID,'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "',RD_RFQ_Line,RD_Product_Code,RD_Vendor_Item_Code,RD_Quantity,0,'NR',0,'Non-registered',RD_Product_Desc,RD_UOM,RD_Delivery_Lead_Time,RD_Warranty_Terms,1,1,'',0" & _
                    '"FROM RFQ_DETAIL WHERE RD_RFQ_ID='" & Common.Parse(RFQ_ID) & "'"
                    strsql = "INSERT INTO RFQ_REPLIES_DETAIL_TEMP(RRDT_RFQ_ID,RRDT_V_Company_ID,RRDT_Line_No,RRDT_Product_ID,RRDT_Product_Name,RRDT_Quantity,RRDT_Unit_Price,RRDT_GST_Code,RRDT_GST,RRDT_GST_Desc,RRDT_Product_Desc,RRDT_UOM,RRDT_Delivery_Lead_Time,RRDT_Warranty_Terms,RRDT_Min_Pack_Qty,RRDT_Min_Order_Qty,RRDT_Remarks,RRDT_Tolerance,RRDT_DEL_CODE,RRDT_ITEM_TYPE)" & _
                    "SELECT RD_RFQ_ID,'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "',RD_RFQ_Line,RD_Product_Code,RD_Vendor_Item_Code,RD_Quantity,NULL,'NR',0,'Non-registered',RD_Product_Desc,RD_UOM,RD_Delivery_Lead_Time,RD_Warranty_Terms,1,1,'',0,RM_DEL_CODE,RD_ITEM_TYPE " & _
                    " FROM RFQ_DETAIL LEFT JOIN RFQ_MSTR ON RM_RFQ_ID = RD_RFQ_ID WHERE RD_RFQ_ID='" & Common.Parse(RFQ_ID) & "'"
                    Common.Insert2Ary(strAry, strsql)
                End If
                objDb.BatchExecute(strAry) '//if fail, how??????
            End If
        End Function

        Public Function get_UOMcode(ByVal CODE_DESC As String, ByVal CODE_CATEGORY As String) As String
            Dim strsql As String
            Dim count As Integer

            strsql = "select CODE_ABBR from CODE_MSTR where CODE_DESC ='" & CODE_DESC & "' and CODE_CATEGORY='" & CODE_CATEGORY & "'"
            Dim tDS As DataSet = objDb.FillDs(strsql)
            If tDS.Tables(0).Rows.Count > 0 Then
                get_UOMcode = tDS.Tables(0).Rows(0).Item("CODE_ABBR")
            End If

        End Function
        Public Function get_RFQDetail(ByVal RFQ_ID As String) As DataSet

            Dim DS1 As System.Data.DataSet
            Dim strsql As String = "select Distinct RD_Product_Desc,RD_Coy_ID, RD_RFQ_Line,RD_Quantity,RD_UOM,RD_Delivery_Lead_Time,RD_Warranty_Terms,RD_ITEM_TYPE from RFQ_DETAIL where RD_RFQ_ID='" & Common.Parse(RFQ_ID) & " ' order by RD_RFQ_Line"
            'Dim strsql As String = "select * from RFQ_DETAIL WHERE RD_RFQ_ID='" & common.parseNull (  RFQ_ID ) & "'"
            Dim objDb As New EAD.DBCom
            DS1 = objDb.FillDs(strsql)
            objDb = Nothing
            get_RFQDetail = DS1

        End Function

        Public Function get_RFQDetail1(ByVal RFQ_ID As String) As DataSet

            Dim DS1 As System.Data.DataSet
            Dim strsql As String = "select Distinct RD_Product_Desc,RD_Coy_ID, RD_RFQ_Line,RD_Quantity,RD_UOM,RD_Delivery_Lead_Time,RD_Warranty_Terms from RFQ_DETAIL where RD_RFQ_ID='" & Common.Parse(RFQ_ID) & " ' order by RD_RFQ_Line"
            'Dim strsql As String = "select * from RFQ_DETAIL WHERE RD_RFQ_ID='" & common.parseNull (  RFQ_ID ) & "'"
            Dim objDb As New EAD.DBCom
            DS1 = objDb.FillDs(strsql)
            objDb = Nothing
            get_RFQDetail1 = DS1

        End Function

        Public Function get_vendorlist(ByVal RFQ_ID As String, ByRef list_name() As String, ByRef list_id() As String, ByRef i As Integer, ByRef com_str As String)
            Dim comid_str As String
            Dim strsql As String = "select RVDLM_List_Name,RVDLM_List_Index " & _
            " from RFQ_MSTR RM,RFQ_VEN_DISTR_LIST_MSTR ,RFQ_INVITED_VENLIST_MSTR " & _
            " WHERE RIVMT_User_ID='" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " & _
            " AND RIVMT_RFQ_ID = RM_RFQ_ID " & _
            " and RM_RFQ_ID='" & Common.Parse(RFQ_ID) & "' and RVDLM_List_Index=RIVMT_Distribution_list_id" ' and RIVMT_RFQ_Name= RM_RFQ_Name
            Dim tDS As DataSet

            tDS = objDb.FillDs(strsql)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                list_name(i) = tDS.Tables(0).Rows(j).Item("RVDLM_List_Name")
                list_id(i) = tDS.Tables(0).Rows(j).Item("RVDLM_List_Index")
                If comid_str = "" Then
                    comid_str = "'" & tDS.Tables(0).Rows(j).Item("RVDLM_List_Index") & "'"
                Else
                    comid_str = comid_str & ",'" & tDS.Tables(0).Rows(j).Item("RVDLM_List_Index") & "'"
                End If
                i = i + 1
            Next
            If comid_str <> "" Then
                Dim strsql2 As String
                strsql2 = "select RTVDT_v_company_id " & _
                        "from RFQ_INVITED_VENLIST_DETAIL, RFQ_INVITED_VENLIST_MSTR " & _
                        "where RTVDT_Distribution_List_Id in(" & comid_str & ") " & _
                        "AND RTVDT_Distribution_List_Id = RIVMT_Distribution_list_id " & _
                        "AND RTVDT_User_Id = RIVMT_User_ID AND RIVMT_RFQ_ID = '" & Common.Parse(RFQ_ID) & "' " & _
                        "AND RTVDT_RFQ_ID = RIVMT_RFQ_ID "
                'AND RTVDT_RFQ_Name = RIVMT_RFQ_Name 
                tDS = objDb.FillDs(strsql2)
                For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                    If com_str = "" Then
                        com_str = "'" & tDS.Tables(0).Rows(j).Item("RTVDT_v_company_id") & "'"
                    Else
                        com_str = com_str & ",'" & tDS.Tables(0).Rows(j).Item("RTVDT_v_company_id") & "'"
                    End If
                Next
            End If
        End Function

        Public Function get_vendorName(ByVal RFQ_ID As String, ByRef com_name() As String, ByRef com_id() As String, ByRef COUNT As Integer, ByVal com_str As String)
            COUNT = 0
            Dim strsql As String = "select DISTINCT CM.CM_COY_NAME,CM.CM_COY_ID " & _
                                   " FROM COMPANY_MSTR CM,RFQ_INVITED_VENLIST RIV" & _
                                   " WHERE CM.CM_COY_ID=RIV.RIV_S_Coy_ID AND RIV_RFQ_ID='" & Common.Parse(RFQ_ID) & " '" & _
                                   " AND CM_STATUS = 'A'"
            If com_str <> "" Then
                strsql = strsql & " and RIV_S_Coy_ID not in(" & com_str & ")"
            End If

            Dim tDS As DataSet = objDb.FillDs(strsql)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                com_name(COUNT) = tDS.Tables(0).Rows(j).Item("CM_COY_NAME")
                com_id(COUNT) = tDS.Tables(0).Rows(j).Item("CM_COY_ID")
                COUNT = COUNT + 1
            Next
        End Function
        Public Function getSearchVendor(Optional ByVal byname As String = "") As DataSet
            Dim strsql As String
            Dim ds As DataSet
            strsql = "SELECT CM_COY_ID, CM_COY_NAME,CM_ADDR_LINE1,CM_ADDR_LINE2,CM_ADDR_LINE3,CM_POSTCODE,CM_CITY,CM_STATE,CM_COUNTRY,CM_EMAIL,CM_PHONE" & _
            " FROM COMPANY_MSTR WHERE (CM_COY_TYPE='BOTH' OR CM_COY_TYPE='VENDOR') AND CM_STATUS = 'A' AND CM_DELETED='N' "
            If byname <> "" Then
                strsql &= " AND CM_COY_NAME='" & byname & "' "
            End If
            strsql &= "ORDER BY CM_COY_NAME"
            ds = objDb.FillDs(strsql)
            Return ds
        End Function

        'Added by Joon on 31th Oct 2011 for issue 1097
        Public Function GetApprVendor(Optional ByVal byname As String = "") As DataSet
            Dim strsql_svendor As String
            Dim dssvendor As DataSet

            strsql_svendor &= "select CM_COY_ID,m.CM_COY_NAME,CM_ADDR_LINE1,CM_ADDR_LINE2,CM_ADDR_LINE3,CM_POSTCODE,CM_CITY,CM_STATE,CM_COUNTRY,CM_EMAIL,CM_PHONE " & _
                 " from Company_Vendor v, Company_Mstr m " & _
                 " where v.CV_S_COY_ID=m.CM_COY_ID and v.CV_B_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' AND CM_STATUS = 'A' AND m.CM_DELETED <> 'Y' "

            If byname <> "" Then
                strsql_svendor &= " AND CM_COY_NAME='" & byname & "' "
            End If
            strsql_svendor &= "ORDER BY CM_COY_NAME"
            dssvendor = objDb.FillDs(strsql_svendor)
            GetApprVendor = dssvendor
        End Function

        Public Function GetApprVendor_AZ(Optional ByVal byname As String = "") As DataSet
            Dim strsql_svendor As String
            Dim dssvendor As DataSet

            strsql_svendor &= "select CM_COY_ID,m.CM_COY_NAME,CM_ADDR_LINE1,CM_ADDR_LINE2,CM_ADDR_LINE3,CM_POSTCODE,CM_CITY,CM_STATE,CM_COUNTRY,CM_EMAIL,CM_PHONE " & _
                 " from Company_Vendor v, Company_Mstr m " & _
                 " where v.CV_S_COY_ID=m.CM_COY_ID and v.CV_B_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' AND CM_STATUS = 'A' AND m.CM_DELETED <> 'Y' "

            If byname <> "" Then
                strsql_svendor &= " AND CM_COY_NAME LIKE '" & byname & "%' "
            End If
            strsql_svendor &= "ORDER BY CM_COY_NAME"
            dssvendor = objDb.FillDs(strsql_svendor)
            GetApprVendor_AZ = dssvendor
        End Function
        ' ai chu add on 16/11/2005
        ' to retrieve shipment mode and shipment term from RFQ_REPLIES_MSTR for raise PR purpose
        Public Function getQuotationShipment(ByVal strRFQId As String, ByRef strShipMode As String, ByRef strShipTerm As String)
            Dim strsql As String

            strsql = "SELECT RRM_Ship_Term, RRM_Ship_Mode FROM RFQ_REPLIES_MSTR "
            strsql &= "WHERE RRM_RFQ_ID = '" & Common.Parse(strRFQId) & "' "
            Dim tDS As DataSet = objDb.FillDs(strsql)
            If IsDBNull(tDS.Tables(0).Rows(0).Item("RRM_Ship_Mode")) Then
                strShipMode = ""
            Else
                strShipMode = tDS.Tables(0).Rows(0).Item("RRM_Ship_Mode")
            End If

            If IsDBNull(tDS.Tables(0).Rows(0).Item("RRM_Ship_Term")) Then
                strShipTerm = ""
            Else
                strShipTerm = tDS.Tables(0).Rows(0).Item("RRM_Ship_Term")
            End If

        End Function
        Function HasAttachmentVen(ByVal rfq_no As String, ByVal strCoyID As String, Optional ByVal file_type As String = "") As Boolean
            Dim strSql As String
            'strCoyID = HttpContext.Current.Session("CompanyId")

            If file_type = "E" Then
                strSql = " AND CDA_TYPE = 'E'"
            Else
                strSql = ""
            End If

            strSql = "SELECT '*' FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID= '" & Common.Parse(strCoyID) & "' AND CDA_DOC_NO='" & Common.Parse(rfq_no) & "' AND CDA_DOC_TYPE='RFQ'" & strSql
            If objDb.Exist(strSql) >= 1 Then
                Return True
            Else
                Return False
            End If
        End Function

        

#End Region

    End Class



End Namespace



