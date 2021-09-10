Imports System.IO
Imports System.Configuration
Imports System.Web
Imports System.Text.RegularExpressions
Imports System.Drawing

Namespace AgoraLegacy
    Public Class UploadRule
        Public Structure SQLType
            Public Query As String
            Public ErrMsg As String
        End Structure

        Public ColNo As String
        Public ColName As String
        Public DBField As String
        Public AllowNull As String
        Public Regex As String
        Public RegexErrMsg As String
        Public SQL(0) As SQLType

    End Class

    Public Class ExcelProduct
        Public RowNo As Integer
        Public ItemID As String
        Public CoyId As String
        Public ItemDesc As String
        Public UNSPSCCode As String
        Public UNSPSCDesc As String
        Public UnitCost As Double
        Public CurrencyCode As String
        Public UOM As String
        Public TaxCode As String
        Public MgmtCode As String
        Public MgmtText As String
        Public VendorItemCode As String
        Public Brand As String
        Public Model As String
        Public Action As Char
    End Class

    Public Class ExcelBIMProduct_Common
        Public No As Integer
        Public ItemCode As String
        Public ItemName As String
        Public ItemType As String
        Public Description As String
        Public CommType As String
        Public UOM As String
        Public Category As String 'Common
        Public RefNo As String
        Public QC As String
        Public AccCode As String
        Public OrderQtyMin As Double
        Public OrderQtyMax As Double
        Public SafetyLevelMin As Double
        Public MaxInvQty As Double
        Public Brand As String
        Public ManuName As String
        Public DrawingNo As String
        Public Model As String
        Public GrossWeight As String
        Public NetWeight As String
        Public Length As String
        Public VerNo As String
        Public PackSpec As String
        Public Width As String
        Public ColorInfo As String
        Public Volume As String
        Public HSCode As String
        Public Height As String
        Public Remarks As String
        Public PreferredVen As String
        Public Ven1st As String
        Public Ven2nd As String
        Public Ven3rd As String
        Public PreferredVenGST As String
        Public Ven1stGST As String
        Public Ven2ndGST As String
        Public Ven3rdGST As String
        Public PreferredVenLead As String
        Public Ven1stLead As String
        Public Ven2ndLead As String
        Public Ven3rdLead As String
        Public PreferredVenItemCode As String
        Public Ven1stItemCode As String
        Public Ven2ndItemCode As String
        Public Ven3rdItemCode As String
        Public Action As Char
    End Class

    Public Class ExcelBIMProduct
        Public No As Integer
        Public ItemCode As String
        Public ItemName As String
        Public ItemType As String
        Public Description As String
        Public CommType As String
        Public UOM As String
        Public RefNo As String
        Public QC As String
        Public AccCode As String
        Public OrderQtyMin As Double
        Public OrderQtyMax As Double
        Public SafetyLevelMin As Double
        Public MaxInvQty As Double
        Public Brand As String
        Public ManuName As String
        Public DrawingNo As String
        Public Model As String
        Public GrossWeight As String
        Public NetWeight As String
        Public Length As String
        Public VerNo As String
        Public PackSpec As String
        Public Width As String
        Public ColorInfo As String
        Public Volume As String
        Public HSCode As String
        Public Height As String
        Public Remarks As String
        Public PreferredVen As String
        Public Ven1st As String
        Public Ven2nd As String
        Public Ven3rd As String
        Public PreferredVenGST As String
        Public Ven1stGST As String
        Public Ven2ndGST As String
        Public Ven3rdGST As String
        Public PreferredVenLead As String
        Public Ven1stLead As String
        Public Ven2ndLead As String
        Public Ven3rdLead As String
        Public PreferredVenItemCode As String
        Public Ven1stItemCode As String
        Public Ven2ndItemCode As String
        Public Ven3rdItemCode As String
        Public Action As Char
    End Class

    Public Class ExcelVIMProduct
        Public No As Integer
        Public ItemCode As String
        Public ItemName As String
        Public Description As String
        Public CommType As String
        Public Tax As String
        Public Price As Decimal
        Public UOM As String
        Public Currency As String
        Public RefNo As String
        Public Brand As String
        Public DrawingNo As String
        Public Model As String
        Public GrossWeight As String
        Public NetWeight As String
        Public Length As String
        Public VerNo As String
        Public PackSpec As String
        Public Width As String
        Public ColorInfo As String
        Public Volume As String
        Public HSCode As String
        Public Height As String
        Public Remarks As String
        Public Action As Char
    End Class

    Public Class ExcelConCat
        Public No As Integer
        Public ItemCode As String
        Public ItemName As String
        Public UOM As String
        Public Currency As String
        Public Price As Decimal
        Public Tax As String
        Public GSTRate As String
        Public TaxCode As String
        Public Remarks As String
    End Class

    Public Class ExcelConCatHeader
        Public Action As String
        Public CoyName As String
        Public ConRefNo As String
        Public ConDesc As String
        Public ValidFrom As String
        Public ValidTo As String
        Public Version As String
    End Class
    Public Class ExcelIPPMultiGLDebits
        Public No As Integer
        Public PayFor As String
        Public Description As String
        Public UOM As String
        Public Quantity As String
        Public UnitPrice As String
        Public Amount As String
        Public GLCode As String
        Public HOBR As String
        Public CostCenter As String
        Public CostAllocation As String
        Public RulesCategory As String
        Public Reimbursement As String
        Public GSTAmount As String
        Public InputTax As String
        Public OutputTax As String

        'Jules 2018.07.19
        Public Gift As String
        Public Category As String
        Public FundType As String
        Public ProductType As String
        Public Channel As String
        Public ReinsuranceCo As String
        Public AssetCode As String
        Public ProjectCode As String
        Public PersonCode As String
        Public WHTOption As String
        Public WHTTax As String
        'End modification.
    End Class
    Public Class ExcelIPPMultiGLDebits_SubDoc
        Public No As Integer
        Public DocNo As String
        Public DocDate As String
        Public Amount As String
    End Class
    Public Class ExcelPOUpload
        Public No As Integer
        Public Description As String

        'Jules 2018.07.17 - PAMB
        Public Gift As String
        Public FundType As String
        Public PersonCode As String
        Public ProjectCode As String
        Public InputTax As String
        'End modification.

        Public GLCode As String
        Public CategoryCode As String
        Public AssetGroup As String
        Public Quantity As String
        Public UOM As String
        Public UnitPrice As String
        Public Tax As String
        Public BudgetAccount As String
        Public DeliveryAddress As String
        Public DeliveryDate As String
        Public WarrantyTerms As String
        Public Remarks As String
        Public GSTRate As String


    End Class
    Public Class Test
        Inherits ExcelPOUpload
        Public CustomField As String

    End Class
    Public Class ExcelIPPMultiGLDebitsHeader
        Public CoyName As String
        Public DocNo As String
        Public DocDate As String
    End Class
    'Zulham 11022019
    Public Class ExcelIPPMultiInvoices
        Public No As Integer
        Public InvoiceNo As String
        Public VendorName As String
        Public PayFor As String
        Public Description As String
        Public UOM As String
        Public Quantity As String
        Public UnitPrice As String
        Public Amount As String
        Public GLCode As String
        Public HOBR As String
        Public CostCenter As String
        Public CostAllocation As String
        Public RulesCategory As String
        Public Reimbursement As String
        Public GSTAmount As String
        Public InputTax As String
        Public OutputTax As String
        Public Gift As String
        Public Category As String
        Public FundType As String
        Public ProductType As String
        Public Channel As String
        Public ReinsuranceCo As String
        Public AssetCode As String
        Public ProjectCode As String
        Public PersonCode As String
        Public WHTOption As String
        Public WHTTax As String
    End Class
    Public Class AppExcel
        Dim dDispatcher As New AgoraLegacy.dispatcher
        Private ObjDb As New EAD.DBCom
        Dim strMassage As String

        Public Property Message() As String
            Get
                Message = strMassage
            End Get
            Set(ByVal Value As String)
                strMassage = Value
            End Set
        End Property

        Public Function OpenConnToExcel(ByVal pFilePath As String, Optional ByVal pHeader As Boolean = False, Optional ByVal strFrom As String = "") As Boolean
            Dim sConn As String
            Try
                If Right(pFilePath, 3) <> "xls" Then
                    strMassage = "Target file is expecting excel file format."
                    Return False
                End If

                If strFrom = "IPP" Then
                    sConn = " Provider=Microsoft.Jet.OLEDB.4.0;" &
                                           " Data Source=" & pFilePath & ";" &
                                           " Extended Properties=""Excel 8.0;IMEX=1;HDR=NO"""
                Else
                    sConn = " Provider=Microsoft.Jet.OLEDB.4.0;" &
                       " Data Source=" & pFilePath & ";" &
                       " Extended Properties=""Excel 8.0;HDR=NO"""
                End If

                If pHeader Then sConn = Replace(sConn, "HDR=NO", "HDR=YES")
                ObjDb.gstrConnOle = sConn

                If ObjDb.ConnStateOle = False Then
                    strMassage = "Connection Failed."
                Else
                    Return True
                End If
            Catch exp As Exception
                Common.TrwExp(exp, sConn)
            Finally
                '//THIS MUST..SO THE EXCEL FILE CAN OPEN IN ANY CASE
                ObjDb.DisConnOle()
                '//
                ' objDcom.DisConn()

            End Try
        End Function

        Public Function ReadExcelFormat(ByVal pXML As String, ByVal pExcel As String, ByRef pRules As myCollection) As DataSet
            Dim objXML As New AppXml
            Dim aa As UploadProduct
            Dim ds As DataSet
            Dim strSheet, strRangeFr, strRangeTo, strSql As String
            Dim dr As DataRow
            Dim dr1 As DataRow()
            Dim intTotalRow, intLoop As Integer
            Try
                ds = objXML.GetDsFromXML(pXML)

                If ds.Tables("Column").Rows.Count > 0 Then
                    intTotalRow = ds.Tables("Column").Rows.Count - 1
                    For intLoop = 0 To intTotalRow
                        Dim pMap As New UploadRule

                        With ds.Tables("Column").Rows(intLoop)
                            pMap.ColNo = .Item("ColNo")                  'Must not null
                            pMap.ColName = .Item("ColName")              'Must not null
                            pMap.DBField = .Item("DBField")              'Must not null
                            pMap.AllowNull = IIf(IsDBNull(.Item("AllowNull")), "False", .Item("AllowNull"))
                            pMap.Regex = IIf(IsDBNull(.Item("Regex")), "", .Item("Regex"))               'Must not null
                            pMap.RegexErrMsg = IIf(IsDBNull(.Item("RegexErrMsg")), "Error found.", .Item("RegexErrMsg"))
                        End With

                        Dim dtrow, dtRow2 As DataRow
                        For Each dtrow In ds.Tables("Column").Rows(intLoop).GetChildRows("Column_SQL")
                            For Each dtRow2 In dtrow.GetChildRows("SQL_SQLItem")
                                pMap.SQL(UBound(pMap.SQL)).Query = IIf(IsDBNull(dtRow2.ItemArray(0)), "", dtRow2.ItemArray(0))
                                pMap.SQL(UBound(pMap.SQL)).ErrMsg = IIf(IsDBNull(dtRow2.ItemArray(1)), "", dtRow2.ItemArray(1))
                                ReDim Preserve pMap.SQL(pMap.SQL.Length)
                            Next
                        Next
                        pRules.Add(pMap)

                    Next

                End If

                If ds.Tables("ReadArea").Rows.Count > 0 Then
                    'strSql = "SELECT * FROM [" & strSheet & "$A1:A1]"
                    'If OpenConnToExcel(pExcel) Then
                    '    If ObjDb.GetVal(strSql) <> "Wheel-Product List" Then
                    '        strMassage = "Please select Item List excel file."
                    '        Return Nothing
                    '    End If
                    'End If

                    dr = ds.Tables("ReadArea").Rows(0)
                    strSheet = Common.parseNull(dr("Sheet"))
                    strRangeFr = dr("ColFrom") & dr("RowFrom")
                    strRangeTo = dr("ColTo") & dr("RowTo")
                    strSql = "SELECT * FROM [" & strSheet & "$" & strRangeFr & ":" & strRangeTo & "]"
                    If OpenConnToExcel(pExcel) Then
                        ds = ObjDb.FillDsOle(strSql)
                        ds = ClearNull(ds)
                        If ds.Tables(0).Rows.Count > 0 Then
                            Return ds
                        Else
                            strMassage = "No record found."
                            Return Nothing
                        End If
                    Else
                        'strMassage = strMassage
                        Return Nothing
                    End If
                End If

            Catch expErr As Exception
                'Common.TrwExp(expErr)
                Throw New CustomException("Xml Reading Error. Please check " & pExcel & ".")
            End Try
        End Function

        Public Function ReadExcelVersion(ByVal pXML As String, ByVal pExcel As String) As String
            Dim objXML As New AppXml
            Dim ds As DataSet
            Dim strSheet, strRangeFr, strRangeTo, strSql As String
            Dim dr As DataRow
            Dim strVersion As String

            Try
                ds = objXML.GetDsFromXML(pXML)

                'Chee Hong - 2014/10/13 - GST Enhancement - Check Concat Excel File version
                If ds.Tables("Version").Rows.Count > 0 Then
                    dr = ds.Tables("Version").Rows(0)
                    strSheet = Common.parseNull(dr("Sheet"))
                    strRangeFr = dr("ColFrom") & dr("RowFrom")
                    strRangeTo = dr("ColTo") & dr("RowTo")
                    strSql = "SELECT * FROM [" & strSheet & "$" & strRangeFr & ":" & strRangeTo & "]"
                    If OpenConnToExcel(pExcel) Then
                        ds = ObjDb.FillDsOle(strSql)
                        If ds.Tables(0).Rows.Count > 0 Then
                            strVersion = Common.parseNull(ds.Tables(0).Rows(0).Item(0))
                        Else
                            strVersion = ""
                        End If

                        Return strVersion
                    End If
                End If

            Catch expErr As Exception
                'Common.TrwExp(expErr)
                Throw New CustomException("Xml Reading Error. Please check " & pExcel & ".")
            End Try
        End Function

        Public Function ReadConExcelFormat(ByVal pXML As String, ByVal pExcel As String, ByRef pRules As myCollection, ByRef dtHeader As DataTable) As DataSet
            Dim objXML As New AppXml
            Dim aa As UploadProduct
            Dim ds As DataSet
            Dim ds1 As New DataSet
            Dim strSheet, strRangeFr, strRangeTo, strSql As String
            Dim dr As DataRow
            Dim intTotalRow, intLoop As Integer
            Dim dt1 As New DataTable

            Try
                dt1.Columns.Add("Action", Type.GetType("System.String"))
                dt1.Columns.Add("CoyName", Type.GetType("System.String"))

                dt1.Columns.Add("ConRefNo", Type.GetType("System.String"))
                dt1.Columns.Add("ConDesc", Type.GetType("System.String"))
                dt1.Columns.Add("Version", Type.GetType("System.String"))
                dt1.Columns.Add("ValidFrom", Type.GetType("System.DateTime"))
                dt1.Columns.Add("ValidTo", Type.GetType("System.DateTime"))

                ds = objXML.GetDsFromXML(pXML)

                If ds.Tables("Column").Rows.Count > 0 Then
                    intTotalRow = ds.Tables("Column").Rows.Count - 1
                    For intLoop = 0 To intTotalRow
                        Dim pMap As New UploadRule

                        With ds.Tables("Column").Rows(intLoop)
                            pMap.ColNo = .Item("ColNo")                  'Must not null
                            pMap.ColName = .Item("ColName")              'Must not null
                            pMap.DBField = .Item("DBField")              'Must not null
                            pMap.AllowNull = IIf(IsDBNull(.Item("AllowNull")), "False", .Item("AllowNull"))
                            pMap.Regex = IIf(IsDBNull(.Item("Regex")), "", .Item("Regex"))               'Must not null
                            pMap.RegexErrMsg = IIf(IsDBNull(.Item("RegexErrMsg")), "Error found.", .Item("RegexErrMsg"))
                        End With

                        Dim dtrow, dtRow2 As DataRow
                        For Each dtrow In ds.Tables("Column").Rows(intLoop).GetChildRows("Column_SQL")
                            For Each dtRow2 In dtrow.GetChildRows("SQL_SQLItem")
                                pMap.SQL(UBound(pMap.SQL)).Query = IIf(IsDBNull(dtRow2.ItemArray(0)), "", dtRow2.ItemArray(0))
                                pMap.SQL(UBound(pMap.SQL)).ErrMsg = IIf(IsDBNull(dtRow2.ItemArray(1)), "", dtRow2.ItemArray(1))
                                ReDim Preserve pMap.SQL(pMap.SQL.Length)
                            Next
                        Next
                        pRules.Add(pMap)
                    Next

                End If

                Dim dtr As DataRow
                dtr = dt1.NewRow()

                If ds.Tables("Action").Rows.Count > 0 Then
                    dr = ds.Tables("Action").Rows(0)
                    strSheet = Common.parseNull(dr("Sheet"))
                    strRangeFr = dr("ColFrom") & dr("RowFrom")
                    strRangeTo = dr("ColTo") & dr("RowTo")
                    strSql = "SELECT * FROM [" & strSheet & "$" & strRangeFr & ":" & strRangeTo & "]"
                    If OpenConnToExcel(pExcel) Then
                        ds1 = ObjDb.FillDsOle(strSql)
                        ds1 = ClearNull(ds1)
                        If ds1.Tables(0).Rows.Count > 0 Then
                            'Return ds
                            dtr("Action") = ds1.Tables(0).Rows(0).Item(1)
                        Else
                            dtr("Action") = ""
                            'Return Nothing
                        End If
                    Else
                        'strMassage = strMassage
                        Return Nothing
                    End If
                End If

                If ds.Tables("CoyName").Rows.Count > 0 Then
                    dr = ds.Tables("CoyName").Rows(0)
                    strSheet = Common.parseNull(dr("Sheet"))
                    strRangeFr = dr("ColFrom") & dr("RowFrom")
                    strRangeTo = dr("ColTo") & dr("RowTo")
                    strSql = "SELECT * FROM [" & strSheet & "$" & strRangeFr & ":" & strRangeTo & "]"
                    If OpenConnToExcel(pExcel) Then
                        ds1 = ObjDb.FillDsOle(strSql)
                        ds1 = ClearNull(ds1)
                        If ds1.Tables(0).Rows.Count > 0 Then
                            'Return ds
                            dtr("CoyName") = ds1.Tables(0).Rows(0).Item(0)
                        Else
                            dtr("CoyName") = ""
                            'Return Nothing
                        End If
                    Else
                        'strMassage = strMassage
                        Return Nothing
                    End If
                End If

                If ds.Tables("ConRefNo").Rows.Count > 0 Then
                    dr = ds.Tables("ConRefNo").Rows(0)
                    strSheet = Common.parseNull(dr("Sheet"))
                    strRangeFr = dr("ColFrom") & dr("RowFrom")
                    strRangeTo = dr("ColTo") & dr("RowTo")
                    strSql = "SELECT * FROM [" & strSheet & "$" & strRangeFr & ":" & strRangeTo & "]"
                    If OpenConnToExcel(pExcel) Then
                        ds1 = ObjDb.FillDsOle(strSql)
                        ds1 = ClearNull(ds1)
                        If ds1.Tables(0).Rows.Count > 0 Then
                            'Return ds
                            dtr("ConRefNo") = ds1.Tables(0).Rows(0).Item(0)
                        Else
                            dtr("ConRefNo") = ""
                            'Return Nothing
                        End If
                    Else
                        'strMassage = strMassage
                        Return Nothing
                    End If
                End If

                If ds.Tables("ConDesc").Rows.Count > 0 Then
                    dr = ds.Tables("ConDesc").Rows(0)
                    strSheet = Common.parseNull(dr("Sheet"))
                    strRangeFr = dr("ColFrom") & dr("RowFrom")
                    strRangeTo = dr("ColTo") & dr("RowTo")
                    strSql = "SELECT * FROM [" & strSheet & "$" & strRangeFr & ":" & strRangeTo & "]"
                    If OpenConnToExcel(pExcel) Then
                        ds1 = ObjDb.FillDsOle(strSql)
                        ds1 = ClearNull(ds1)
                        If ds1.Tables(0).Rows.Count > 0 Then
                            'Return ds
                            dtr("ConDesc") = ds1.Tables(0).Rows(0).Item(1)
                        Else
                            dtr("ConDesc") = ""
                            'Return Nothing
                        End If
                    Else
                        'strMassage = strMassage
                        Return Nothing
                    End If
                End If

                'Chee Hong - 2014/10/10 - GST Enhancement - Check Concat Excel File version
                If ds.Tables("Version").Rows.Count > 0 Then
                    dr = ds.Tables("Version").Rows(0)
                    strSheet = Common.parseNull(dr("Sheet"))
                    strRangeFr = dr("ColFrom") & dr("RowFrom")
                    strRangeTo = dr("ColTo") & dr("RowTo")
                    strSql = "SELECT * FROM [" & strSheet & "$" & strRangeFr & ":" & strRangeTo & "]"
                    If OpenConnToExcel(pExcel) Then
                        ds1 = ObjDb.FillDsOle(strSql)
                        'ds1 = ClearNull(ds1)
                        If ds1.Tables(0).Rows.Count > 0 Then
                            dtr("Version") = Common.parseNull(ds1.Tables(0).Rows(0).Item(0))
                        Else
                            dtr("Version") = ""
                        End If
                    End If
                End If

                If ds.Tables("ValidFrom").Rows.Count > 0 Then
                    dr = ds.Tables("ValidFrom").Rows(0)
                    strSheet = Common.parseNull(dr("Sheet"))
                    strRangeFr = dr("ColFrom") & dr("RowFrom")
                    strRangeTo = dr("ColTo") & dr("RowTo")
                    strSql = "SELECT * FROM [" & strSheet & "$" & strRangeFr & ":" & strRangeTo & "]"
                    If OpenConnToExcel(pExcel) Then
                        ds1 = ObjDb.FillDsOle(strSql)
                        ds1 = ClearNull(ds1)
                        If ds1.Tables(0).Rows.Count > 0 Then
                            'Return ds
                            dtr("ValidFrom") = ds1.Tables(0).Rows(0).Item(6)
                        Else
                            dtr("ValidFrom") = ""
                            'Return Nothing
                        End If
                    Else
                        'strMassage = strMassage
                        Return Nothing
                    End If
                End If

                If ds.Tables("ValidTo").Rows.Count > 0 Then
                    dr = ds.Tables("ValidTo").Rows(0)
                    strSheet = Common.parseNull(dr("Sheet"))
                    strRangeFr = dr("ColFrom") & dr("RowFrom")
                    strRangeTo = dr("ColTo") & dr("RowTo")
                    strSql = "SELECT * FROM [" & strSheet & "$" & strRangeFr & ":" & strRangeTo & "]"
                    If OpenConnToExcel(pExcel) Then
                        ds1 = ObjDb.FillDsOle(strSql)
                        ds1 = ClearNull(ds1)
                        If ds1.Tables(0).Rows.Count > 0 Then
                            'Return ds
                            dtr("ValidTo") = ds1.Tables(0).Rows(0).Item(6)
                        Else
                            dtr("ValidTo") = ""
                            'Return Nothing
                        End If
                    Else
                        'strMassage = strMassage
                        Return Nothing
                    End If
                End If
                dt1.Rows.Add(dtr)
                dtHeader = dt1

                If ds.Tables("ReadArea").Rows.Count > 0 Then
                    dr = ds.Tables("ReadArea").Rows(0)
                    strSheet = Common.parseNull(dr("Sheet"))
                    strRangeFr = dr("ColFrom") & dr("RowFrom")
                    strRangeTo = dr("ColTo") & dr("RowTo")
                    strSql = "SELECT * FROM [" & strSheet & "$" & strRangeFr & ":" & strRangeTo & "]"
                    If OpenConnToExcel(pExcel) Then
                        ds = ObjDb.FillDsOle(strSql)
                        ds = ClearNull(ds)
                        If ds.Tables(0).Rows.Count > 0 Then
                            Return ds
                        Else
                            strMassage = "No record found."
                            Return Nothing
                        End If
                    Else
                        'strMassage = strMassage
                        Return Nothing
                    End If
                End If

            Catch expErr As Exception
                'Common.TrwExp(expErr)
                Throw New CustomException("Xml Reading Error. Please check " & pExcel & ".")
            End Try
        End Function

        Private Function ClearNull(ByVal ds As DataSet) As DataSet
            Dim a As DataRow
            Dim b As DataColumn
            Dim i As Integer

            For Each a In ds.Tables(0).Rows
                Dim bDelete As Boolean = True
                For i = 1 To a.ItemArray.Length - 1
                    If Not a.IsNull(i) Then
                        bDelete = False
                    End If

                Next
                If bDelete Then
                    a.Delete()
                End If
            Next

            ds.AcceptChanges()

            Return ds
        End Function
        Public Function ReadProductExcel_1(ByVal pXML As String, ByVal pExcel As String, ByRef a As myCollection) As DataSet
            Dim objXML As New AppXml
            Dim aa As UploadProduct
            Dim ds As DataSet
            Dim strSheet, strRangeFr, strRangeTo, strSql As String
            Dim dr As DataRow
            Dim dr1 As DataRow()
            Dim intTotalRow, intLoop As Integer
            'Dim a As myCollection
            Try
                '  dr1.IndexOf(
                ds = objXML.GetDsFromXML(pXML)

                If ds.Tables("Detail").Rows.Count > 0 Then
                    intTotalRow = ds.Tables("Detail").Rows.Count - 1
                    For intLoop = 0 To intTotalRow
                        Dim pMap As New UploadRule
                        pMap.ColNo = ds.Tables("Detail").Rows(intLoop)("ColNo")
                        pMap.ColName = ds.Tables("Detail").Rows(intLoop)("ColName")
                        pMap.DBField = ds.Tables("Detail").Rows(intLoop)("DBField")
                        pMap.AllowNull = ds.Tables("Detail").Rows(intLoop)("AllowNUll")
                        a.Add(pMap)
                    Next
                End If

                If ds.Tables("ReadArea").Rows.Count > 0 Then
                    dr = ds.Tables("ReadArea").Rows(0)
                    strSheet = Common.parseNull(dr("Sheet"))
                    strRangeFr = dr("ColFrom") & dr("RowFrom")
                    strRangeTo = dr("ColTo") & dr("RowTo")
                    strSql = "SELECT * FROM [" & strSheet & "$" & strRangeFr & ":" & strRangeTo & "]"


                    OpenConnToExcel(pExcel)
                    ds = ObjDb.FillDs(strSql)
                End If
                Return ds
            Catch expErr As Exception
                Common.TrwExp(expErr)
            End Try
        End Function

        Public Function ReadProductExcel(ByVal pXML As String, ByVal pExcel As String) As DataSet
            Dim objXML As New AppXml
            Dim aa As UploadProduct
            Dim ds As DataSet
            Dim strSheet, strRangeFr, strRangeTo, strSql As String
            Dim dr As DataRow
            Dim dr1 As DataRow()
            Dim pMap As UploadProduct
            Dim i, myRow As Integer
            Try
                '  dr1.IndexOf(
                ds = objXML.GetDsFromXML(pXML)

                'dr1 = ds.Tables("Catalogue").Select("Type ='List'")
                For i = 0 To ds.Tables("Catalogue").Rows.Count
                    If ds.Tables("Catalogue").Rows(i).Item(1) = "List" Then
                        myRow = i
                        Exit For
                    End If
                Next

                If ds.Tables("Detail").Rows.Count > 0 Then
                    dr = ds.Tables("Detail").Rows(myRow)
                    pMap.DBFiels = dr("DBField")
                    pMap.AllowNull = dr("AllowNUll")
                End If

                If ds.Tables("Header").Rows.Count > 0 Then
                    dr = ds.Tables("Header").Rows(myRow)
                    strSheet = Common.parseNull(dr("Sheet"))
                    strRangeFr = dr("ColFrom") & dr("RowFrom")
                    strRangeTo = dr("ColTo") & dr("RowTo")
                    'strSql = "SELECT * FROM [" & strSheet & "$" & strRangeFr & ":" & strRangeTo & "]"
                    strSql = "SELECT * FROM [" & strSheet & "$]"
                    OpenConnToExcel(pExcel)
                    ds = ObjDb.FillDs(strSql)
                    Return ds
                End If
            Catch expErr As Exception
                Common.TrwExp(expErr)
            End Try
        End Function

        'Disabled by Sam 13/9/2010
        'Public Function WriteDsToExcelUseDataAdapter(ByVal ds As DataSet)
        '    Dim ctx As Web.HttpContext = Web.HttpContext.Current
        '    OpenConnToExcel(ctx.Server.MapPath(ctx.Request.ApplicationPath) & "\xml\moo.xls")
        '    ObjDb.Execute("CREATE TABLE [Products123] ([ID]  VARCHAR(100), [Name] VARCHAR(255), a VARCHAR(100),B VARCHAR(100))")
        '    Dim cmd As New OleDb.OleDbCommand(String.Empty, ObjDb.gcnConn)
        '    Try
        '        ' Transfer the data to the worksheet
        '        'DS.
        '        cmd.CommandText = "INSERT INTO [Products123] ([ID],[Name],a,B) VALUES (?,?,?,?)"
        '        With cmd.Parameters
        '            .Add("@id", OleDb.OleDbType.VarChar, 100, "F1")
        '            .Add("@name", OleDb.OleDbType.VarChar, 255, "F2")
        '            .Add("@a", OleDb.OleDbType.VarChar, 100, "F3")
        '            .Add("@B", OleDb.OleDbType.VarChar, 100, "ErrorMessage")
        '        End With

        '        Dim da As New OleDb.OleDbDataAdapter

        '        da.InsertCommand = cmd
        '        da.Update(ds)
        '        '//ERROR OCCURED
        '        '//"Update requires a valid UpdateCommand when passed DataRow collection with modified rows"
        '    Finally
        '        'cn.Close()
        '        cmd.Dispose()
        '    End Try
        '    'objdb.Execute
        'End Function

        Function WriteToExcel() As String
            Dim ctx As Web.HttpContext = Web.HttpContext.Current

            Dim strSql, strAry(0) As String
            Dim tDS As DataSet
            tDS = ObjDb.FillDs("SELECT PM_PRODUCT_CODE,PM_PRODUCT_DESC,PM_CATEGORY_NAME FROM PRODUCT_MSTR WHERE PM_DELETED <> 'Y'")
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                strSql = "INSERT INTO PRODUCT VALUES('" & tDS.Tables(0).Rows(j).Item(0) & "','" & Common.Parse(tDS.Tables(0).Rows(j).Item(1)) & "','" & tDS.Tables(0).Rows(j).Item(2) & "')"
                Common.Insert2Ary(strAry, strSql)
            Next
            '//File
            'Dim o1 As IO.File
            'Dim o1 As IO.FileInfo
            Dim dDispatcher As New AgoraLegacy.dispatcher
            'Response.Redirect(dDispatcher.direct("DO", "AddDO.aspx", "coyID=" & vendorID & "&coyName=" & vendorName))
            'Dim strSourceFile As String = ctx.Server.MapPath(ctx.Request.ApplicationPath) & "\xml\ListPrice.xls"
            'Dim strDestFile As String = ctx.Server.MapPath(ctx.Request.ApplicationPath) & "\xml\Demo123.xls"
            Dim strSourceFile As String = dDispatcher.direct("XML", "ListPrice.xls")
            Dim strDestFile As String = dDispatcher.direct("XML", "Demo123.xls")

            If File.Exists(strDestFile) Then
                File.Delete(strDestFile)
            End If
            File.Copy(strSourceFile, strDestFile)
            '//File
            '//FileInfo
            Dim o2 As New IO.FileInfo(strSourceFile)
            If o2.Exists Then
            End If
            '//FileInfo
            OpenConnToExcel(strDestFile, True)
            ObjDb.BatchExecute(strAry)
            Return strDestFile
        End Function
        Public Function ReadIPPMultiGLExcelFormat(ByVal pXML As String, ByVal pExcel As String, ByRef pRules As myCollection, ByRef dtHeader As DataTable, ByRef version As String, Optional ByVal strIsGST As String = "") As DataSet
            Dim objXML As New AppXml
            Dim aa As UploadProduct
            Dim ds As DataSet
            Dim ds1, dsVersion As New DataSet
            Dim strSheet, strRangeFr, strRangeTo, strSql As String
            Dim dr As DataRow
            Dim intTotalRow, intLoop As Integer
            Dim dt1 As New DataTable

            Try
                'dt1.Columns.Add("Action", Type.GetType("System.String"))
                dt1.Columns.Add("CoyName", Type.GetType("System.String"))
                'dt1.Columns.Add("ConRefNo", Type.GetType("System.String"))
                dt1.Columns.Add("DocNo", Type.GetType("System.String"))
                dt1.Columns.Add("DocDate", Type.GetType("System.DateTime"))
                dt1.Columns.Add("MasterDoc", Type.GetType("System.String"))
                'dt1.Columns.Add("ValidTo", Type.GetType("System.DateTime"))

                ds = objXML.GetDsFromXML(pXML)

                If ds.Tables("Column").Rows.Count > 0 Then
                    intTotalRow = ds.Tables("Column").Rows.Count - 1
                    For intLoop = 0 To intTotalRow
                        Dim pMap As New UploadRule

                        With ds.Tables("Column").Rows(intLoop)
                            pMap.ColNo = .Item("ColNo")                  'Must not null
                            pMap.ColName = .Item("ColName")              'Must not null
                            pMap.DBField = .Item("DBField")              'Must not null
                            pMap.AllowNull = IIf(IsDBNull(.Item("AllowNull")), "False", .Item("AllowNull"))
                            pMap.Regex = IIf(IsDBNull(.Item("Regex")), "", .Item("Regex"))               'Must not null
                            pMap.RegexErrMsg = IIf(IsDBNull(.Item("RegexErrMsg")), "Error found.", .Item("RegexErrMsg"))
                        End With

                        Dim dtrow, dtRow2 As DataRow
                        For Each dtrow In ds.Tables("Column").Rows(intLoop).GetChildRows("Column_SQL")
                            For Each dtRow2 In dtrow.GetChildRows("SQL_SQLItem")
                                pMap.SQL(UBound(pMap.SQL)).Query = IIf(IsDBNull(dtRow2.ItemArray(0)), "", dtRow2.ItemArray(0))
                                pMap.SQL(UBound(pMap.SQL)).ErrMsg = IIf(IsDBNull(dtRow2.ItemArray(1)), "", dtRow2.ItemArray(1))
                                ReDim Preserve pMap.SQL(pMap.SQL.Length)
                            Next
                        Next
                        pRules.Add(pMap)
                    Next

                End If

                Dim dtr As DataRow
                dtr = dt1.NewRow()



                If ds.Tables("CoyName").Rows.Count > 0 Then
                    dr = ds.Tables("CoyName").Rows(0)
                    strSheet = Common.parseNull(dr("Sheet"))
                    strRangeFr = dr("ColFrom") & dr("RowFrom")
                    strRangeTo = dr("ColTo") & dr("RowTo")
                    strSql = "SELECT * FROM [" & strSheet & "$" & strRangeFr & ":" & strRangeTo & "]"
                    If OpenConnToExcel(pExcel, , "IPP") Then
                        ds1 = ObjDb.FillDsOle(strSql)
                        ds1 = ClearNull(ds1)
                        If ds1.Tables(0).Rows.Count > 0 Then
                            'Return ds
                            dtr("CoyName") = ds1.Tables(0).Rows(0).Item(1)
                        Else
                            dtr("CoyName") = ""
                            'Return Nothing
                        End If
                    Else
                        'strMassage = strMassage
                        Return Nothing
                    End If
                End If



                If ds.Tables("DocNo").Rows.Count > 0 Then
                    dr = ds.Tables("DocNo").Rows(0)
                    strSheet = Common.parseNull(dr("Sheet"))
                    strRangeFr = dr("ColFrom") & dr("RowFrom")
                    strRangeTo = dr("ColTo") & dr("RowTo")
                    strSql = "SELECT * FROM [" & strSheet & "$" & strRangeFr & ":" & strRangeTo & "]"
                    If OpenConnToExcel(pExcel, , "IPP") Then
                        ds1 = ObjDb.FillDsOle(strSql)
                        ds1 = ClearNull(ds1)
                        If ds1.Tables(0).Rows.Count > 0 Then
                            'Return ds
                            dtr("DocNo") = ds1.Tables(0).Rows(0).Item(1)
                        Else
                            dtr("DocNo") = ""
                            'Return Nothing
                        End If
                    Else
                        'strMassage = strMassage
                        Return Nothing
                    End If
                End If

                If ds.Tables("MasterDoc").Rows.Count > 0 Then
                    dr = ds.Tables("MasterDoc").Rows(0)
                    strSheet = Common.parseNull(dr("Sheet"))
                    If strIsGST = "Yes" Then strRangeFr = "G3" Else strRangeFr = dr("ColFrom") & dr("RowFrom")
                    strRangeTo = dr("ColTo") & dr("RowTo")
                    strSql = "SELECT * FROM [" & strSheet & "$" & strRangeFr & ":" & strRangeTo & "]"
                    If OpenConnToExcel(pExcel, , "IPP") Then
                        ds1 = ObjDb.FillDsOle(strSql)
                        ds1 = ClearNull(ds1)
                        If ds1.Tables(0).Rows.Count > 0 Then
                            'Return ds
                            dtr("MasterDoc") = ds1.Tables(0).Rows(0).Item(1)
                        Else
                            dtr("MasterDoc") = ""
                            'Return Nothing
                        End If
                    Else
                        'strMassage = strMassage
                        Return Nothing
                    End If
                End If

                If ds.Tables("DocDate").Rows.Count > 0 Then
                    dr = ds.Tables("DocDate").Rows(0)
                    strSheet = Common.parseNull(dr("Sheet"))
                    strRangeFr = dr("ColFrom") & dr("RowFrom")
                    strRangeTo = dr("ColTo") & dr("RowTo")
                    strSql = "SELECT * FROM [" & strSheet & "$" & strRangeFr & ":" & strRangeTo & "]"
                    If OpenConnToExcel(pExcel, , "IPP") Then
                        ds1 = ObjDb.FillDsOle(strSql)
                        ds1 = ClearNull(ds1)
                        If ds1.Tables(0).Rows.Count > 0 Then
                            'Return ds
                            dtr("DocDate") = ds1.Tables(0).Rows(0).Item(1)
                        Else
                            dtr("DocDate") = System.DBNull.Value
                            'Return Nothing
                        End If
                    Else
                        'strMassage = strMassage
                        Return Nothing
                    End If
                End If

                dt1.Rows.Add(dtr)
                dtHeader = dt1

                'Zulham 17/02/2015 IPP GST Stage 1
                If strIsGST = "Yes" Then
                    'Version checking for GST
                    If ds.Tables("Version").Rows.Count > 0 Then
                        dr = ds.Tables("Version").Rows(0)
                        strSheet = Common.parseNull(dr("Sheet"))
                        strRangeFr = dr("ColFrom") & dr("RowFrom")
                        strRangeTo = dr("ColTo") & dr("RowTo")
                        strSql = "SELECT * FROM [" & strSheet & "$" & strRangeFr & ":" & strRangeTo & "]"
                        If OpenConnToExcel(pExcel, , "IPP") Then
                            dsVersion = ObjDb.FillDsOle(strSql)
                            'dsVersion = ClearNull(dsVersion)
                            If dsVersion.Tables(0).Rows.Count > 0 Then
                                version = dsVersion.Tables(0).Rows(0).Item(0).ToString
                            Else
                                version = ""
                            End If
                        End If
                    Else
                        version = ""
                    End If
                Else
                    version = ""
                End If

                If ds.Tables("ReadArea").Rows.Count > 0 Then
                    dr = ds.Tables("ReadArea").Rows(0)
                    strSheet = Common.parseNull(dr("Sheet"))
                    strRangeFr = dr("ColFrom") & dr("RowFrom")
                    strRangeTo = dr("ColTo") & dr("RowTo")
                    strSql = "SELECT * FROM [" & strSheet & "$" & strRangeFr & ":" & strRangeTo & "]"
                    If OpenConnToExcel(pExcel, , "IPP") Then
                        ds = ObjDb.FillDsOle(strSql)
                        ds = ClearNull(ds)
                        If ds.Tables(0).Rows.Count > 0 Then
                            Return ds
                        Else
                            strMassage = "No record found."
                            Return Nothing
                        End If
                    Else
                        'strMassage = strMassage
                        Return Nothing
                    End If
                End If

            Catch expErr As Exception
                'Common.TrwExp(expErr)
                Throw New CustomException("Xml Reading Error. Please check " & pExcel & ".")
            End Try
        End Function
        Public Function ReadIPPMultiGLExcelFormat_SubDoc(ByVal pXML As String, ByVal pExcel As String, ByRef pRules As myCollection, ByRef dtHeader As DataTable) As DataSet
            Dim objXML As New AppXml
            Dim aa As UploadProduct
            Dim ds As DataSet
            Dim ds1 As New DataSet
            Dim strSheet, strRangeFr, strRangeTo, strSql As String
            Dim dr As DataRow
            Dim intTotalRow, intLoop As Integer
            Dim dt1 As New DataTable

            Try
                'dt1.Columns.Add("Action", Type.GetType("System.String"))
                dt1.Columns.Add("CoyName", Type.GetType("System.String"))
                'dt1.Columns.Add("ConRefNo", Type.GetType("System.String"))
                dt1.Columns.Add("DocNo", Type.GetType("System.String"))
                dt1.Columns.Add("DocDate", Type.GetType("System.DateTime"))
                'dt1.Columns.Add("ValidTo", Type.GetType("System.DateTime"))

                ds = objXML.GetDsFromXML(pXML)

                If ds.Tables("Column").Rows.Count > 0 Then
                    intTotalRow = ds.Tables("Column").Rows.Count - 1
                    For intLoop = 0 To intTotalRow
                        Dim pMap As New UploadRule

                        With ds.Tables("Column").Rows(intLoop)
                            pMap.ColNo = .Item("ColNo")                  'Must not null
                            pMap.ColName = .Item("ColName")              'Must not null
                            pMap.DBField = .Item("DBField")              'Must not null
                            pMap.AllowNull = IIf(IsDBNull(.Item("AllowNull")), "False", .Item("AllowNull"))
                            pMap.Regex = IIf(IsDBNull(.Item("Regex")), "", .Item("Regex"))               'Must not null
                            pMap.RegexErrMsg = IIf(IsDBNull(.Item("RegexErrMsg")), "Error found.", .Item("RegexErrMsg"))
                        End With

                        Dim dtrow, dtRow2 As DataRow
                        For Each dtrow In ds.Tables("Column").Rows(intLoop).GetChildRows("Column_SQL")
                            For Each dtRow2 In dtrow.GetChildRows("SQL_SQLItem")
                                pMap.SQL(UBound(pMap.SQL)).Query = IIf(IsDBNull(dtRow2.ItemArray(0)), "", dtRow2.ItemArray(0))
                                pMap.SQL(UBound(pMap.SQL)).ErrMsg = IIf(IsDBNull(dtRow2.ItemArray(1)), "", dtRow2.ItemArray(1))
                                ReDim Preserve pMap.SQL(pMap.SQL.Length)
                            Next
                        Next
                        pRules.Add(pMap)
                    Next

                End If

                Dim dtr As DataRow
                dtr = dt1.NewRow()

                If ds.Tables("IPPSubDocs").Rows.Count > 0 Then
                    dr = ds.Tables("IPPSubDocs").Rows(0)
                    strSheet = Common.parseNull(dr("Sheet"))
                    strRangeFr = dr("ColFrom") & dr("RowFrom")
                    strRangeTo = dr("ColTo") & dr("RowTo")
                    strSql = "SELECT * FROM [" & strSheet & "$" & strRangeFr & ":" & strRangeTo & "]"
                    If OpenConnToExcel(pExcel, , "IPP") Then
                        ds = ObjDb.FillDsOle(strSql)
                        ds = ClearNull(ds)
                        If ds.Tables(0).Rows.Count > 0 Then
                            Return ds
                        Else
                            strMassage = "No record found."
                            Return Nothing
                        End If
                    Else
                        'strMassage = strMassage
                        Return Nothing
                    End If
                End If

                'Versic checking for GST
                If ds.Tables("Version").Rows.Count > 0 Then
                    dr = ds.Tables("Version").Rows(0)
                    strSheet = Common.parseNull(dr("Sheet"))
                    strRangeFr = dr("ColFrom") & dr("RowFrom")
                    strRangeTo = dr("ColTo") & dr("RowTo")
                    strSql = "SELECT * FROM [" & strSheet & "$" & strRangeFr & ":" & strRangeTo & "]"
                    If OpenConnToExcel(pExcel, , "IPP") Then
                        ds = ObjDb.FillDsOle(strSql)
                        ds = ClearNull(ds)
                        If ds.Tables(0).Rows.Count > 0 Then
                            Return ds
                        Else
                            strMassage = "No record found."
                            Return Nothing
                        End If
                    Else
                        'strMassage = strMassage
                        Return Nothing
                    End If
                End If

            Catch expErr As Exception
                'Common.TrwExp(expErr)
                Throw New CustomException("Xml Reading Error. Please check " & pExcel & ".")
            End Try
        End Function
#Region "Contract Catalogue"
        Public Sub WriteConCatCell(ByVal ds As DataSet, ByVal ds1 As DataSet, ByVal dsGSTRate As DataSet, ByVal dsGSTTaxCode As DataSet, ByVal pPath As String)
            Dim ctx As Web.HttpContext = Web.HttpContext.Current
            Dim strSourceFile, strDestFile, filename As String
            Dim dDispatcher As New AgoraLegacy.dispatcher

            'strSourceFile = dDispatcher.direct("Template", "ContractCatalogueListingTemplate.xls", "Report")
            'strDestFile = dDispatcher.direct("Report\Temp", "ContractCatalogueListing_" & HttpContext.Current.Session("CompanyId") & "_" & HttpContext.Current.Session("UserId") & ".xls", "Report")

            'strSourceFile = dDispatcher.direct("Template", "ContractCatalogueListingTemplate.xls")
            strSourceFile = ctx.Server.MapPath(ctx.Request.ApplicationPath) & "\Common\Template\ContractCatalogueListingTemplate.xls"
            filename = "ContractCatalogueListing_" & HttpContext.Current.Session("CompanyId") & "_" & HttpContext.Current.Session("UserId") & ".xls"
            strDestFile = ConfigurationManager.AppSettings("TemplateTemp") & filename

            If File.Exists(strDestFile) Then
                File.Delete(strDestFile)
            End If
            File.Copy(strSourceFile, strDestFile)

            OpenConnToExcel(pPath, False)

            Dim SqlAry(0) As String
            Dim lsSql As String
            Dim i As Integer
            Dim intLoop, intLoop1, intTotRow, intTotCol, TotalRow As Integer

            If dsGSTRate.Tables(0).Rows.Count > 0 Or ds1.Tables(0).Rows.Count > 0 Then
                'GST Rate
                For i = 0 To dsGSTRate.Tables(0).Rows.Count - 1
                    lsSql = "Insert into [PredefineCode$E" & i + 1 & ":E" & i + 1 & "] Values ('" & Common.Parse(dsGSTRate.Tables(0).Rows(i).Item("SST Rate")) & "')"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next

                'GST Tax Code
                For i = 0 To dsGSTTaxCode.Tables(0).Rows.Count - 1
                    lsSql = "Insert into [PredefineCode$G" & i + 1 & ":G" & i + 1 & "] Values ('" & Common.Parse(dsGSTTaxCode.Tables(0).Rows(i).Item("SST Tax Code")) & "')"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next

            End If

            TotalRow = ds.Tables(0).Rows.Count
            intTotRow = ds.Tables(0).Rows.Count + 1
            intTotCol = ds.Tables(0).Columns.Count

            If ds1.Tables(0).Rows.Count < 1 And TotalRow < 1 Then
                Exit Sub
            End If

            If TotalRow > 0 Then
                For intLoop = 2 To intTotRow
                    'lsSql = "Insert into [Sheet1$A" & intLoop & ":D" & intLoop & "] Values ("
                    lsSql = "Insert into [ContractCatalogue$] Values ("
                    lsSql = lsSql & "'" & (intLoop - 1) & "',"
                    For intLoop1 = 0 To intTotCol - 1
                        If IsDBNull(ds.Tables(0).Rows(intLoop - 2)(intLoop1)) Then
                            lsSql = lsSql & "'" & ds.Tables(0).Rows(intLoop - 2)(intLoop1) & "',"
                        Else
                            If intLoop1 = 7 Or intLoop1 = 6 Or intLoop1 = 5 Then
                                If ds.Tables(0).Rows(intLoop - 2)(intLoop1) = "" Then
                                    lsSql = lsSql & "' ',"
                                Else
                                    lsSql = lsSql & "'" & Common.Parse(ds.Tables(0).Rows(intLoop - 2)(intLoop1)) & "',"

                                End If

                            Else
                                lsSql = lsSql & "'" & Common.Parse(ds.Tables(0).Rows(intLoop - 2)(intLoop1)) & "',"
                            End If

                            'If intLoop1 = 5 Then    'Tax
                            '    'IIf(ds.Tables(0).Rows(intLoop - 2)(intLoop1) <> "N/A", lsSql = lsSql & "'" & Common.Parse(Format(CDbl(ds.Tables(0).Rows(intLoop - 2)(intLoop1)), "##0")) & "',", lsSql = lsSql & "'" & Common.Parse(ds.Tables(0).Rows(intLoop - 2)(intLoop1)) & "',")
                            '    If ds.Tables(0).Rows(intLoop - 2)(intLoop1) <> "N/A" Then
                            '        lsSql = lsSql & "'" & Common.Parse(Format(CDbl(ds.Tables(0).Rows(intLoop - 2)(intLoop1)), "##0")) & "',"
                            '    Else
                            '        lsSql = lsSql & "'" & Common.Parse(ds.Tables(0).Rows(intLoop - 2)(intLoop1)) & "',"
                            '    End If

                            'ElseIf intLoop1 = 6 Then
                            '    If ds.Tables(0).Rows(intLoop - 2)(intLoop1) = "" Then
                            '        lsSql = lsSql & "' ',"
                            '    Else
                            '        lsSql = lsSql & "'" & Common.Parse(ds.Tables(0).Rows(intLoop - 2)(intLoop1)) & "',"

                            '    End If

                            'Else
                            '    lsSql = lsSql & "'" & Common.Parse(ds.Tables(0).Rows(intLoop - 2)(intLoop1)) & "',"

                            'End If
                            'lsSql = lsSql & "'" & Common.Parse(ds.Tables(0).Rows(intLoop - 2)(intLoop1)) & "',"
                        End If
                    Next
                    'lsSql = lsSql & "'M')"
                    lsSql = Mid(lsSql, 1, Len(lsSql) - 1)
                    lsSql = lsSql & ")"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next
            End If

            If ds1.Tables(0).Rows.Count > 0 Then
                lsSql = "Insert into [ContractCatalogue$B1:B1] Values ('Amendment')"
                Common.Insert2Ary(SqlAry, lsSql)
                lsSql = "Insert into [ContractCatalogue$B4:B4] Values ('" & ds1.Tables(0).Rows(0).Item("CM_COY_NAME") & "')"
                Common.Insert2Ary(SqlAry, lsSql)
                lsSql = "Insert into [ContractCatalogue$B5:B5] Values ('" & ds1.Tables(0).Rows(0).Item("CDM_GROUP_CODE") & "')"
                Common.Insert2Ary(SqlAry, lsSql)
                lsSql = "Insert into [ContractCatalogue$B6:B6] Values ('" & ds1.Tables(0).Rows(0).Item("CDM_GROUP_DESC") & "')"
                Common.Insert2Ary(SqlAry, lsSql)
                lsSql = "Insert into [ContractCatalogue$H4:H4] Values ('" & Common.FormatWheelDate(WheelDateFormat.ShortDate, ds1.Tables(0).Rows(0).Item("CDM_START_DATE")) & "')"
                Common.Insert2Ary(SqlAry, lsSql)
                lsSql = "Insert into [ContractCatalogue$H5:H5] Values ('" & Common.FormatWheelDate(WheelDateFormat.ShortDate, ds1.Tables(0).Rows(0).Item("CDM_END_DATE")) & "')"
                Common.Insert2Ary(SqlAry, lsSql)
            End If

            If SqlAry(0) <> String.Empty Then
                ObjDb.BatchExecuteOle(SqlAry)
                ObjDb = Nothing
            End If

        End Sub

        Public Sub TransferToConCatExcel(ByVal source As DataSet, ByVal fileName As String)
            'OpenConnToExcel(fileName)
            Dim cn As New OleDb.OleDbConnection(String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties={1}Excel 8.0;HDR=Yes{1}", fileName, ControlChars.Quote))
            Dim cmd As New OleDb.OleDbCommand(String.Empty, cn)

            Try
                ' Create the worksheet
                'If IO.File.Exists(fileName) Then IO.File.Delete(fileName)
                cn.Open()
                'cmd.CommandText = "CREATE TABLE [Products] ([ID]  VARCHAR(100), [Name] VARCHAR(255))"
                'cmd.ExecuteNonQuery()

                ' Transfer the data to the worksheet
                cmd.CommandText = "INSERT INTO [ContractCatalogue$] ([Name],[No]) VALUES (?,?)"
                With cmd.Parameters
                    .Add("@Name", OleDb.OleDbType.VarChar, 100, "CM_COY_NAME")
                    .Add("@No", OleDb.OleDbType.VarChar, 255, "CDM_GROUP_CODE")
                End With

                Dim da As New OleDb.OleDbDataAdapter
                da.InsertCommand = cmd
                da.Update(source)
            Finally
                cn.Close()
                cmd.Dispose()
            End Try
        End Sub

        'Public Sub WriteSpecificCell(ByVal fileName As String)
        '    Dim xlApp As New Excel.Application
        '    Dim wbBook As Excel.Workbook = xlApp.Workbooks.Open(fileName)
        '    Dim wsSheet As Excel.Worksheet = CType(wbBook.Worksheets("ContractCatalogue"), Excel.Worksheet)
        '    Dim rnRange As Excel.Range = wsSheet.Range("B5")

        '    rnRange.Value = "Testing"

        '    wbBook.Close(SaveChanges:=True)

        '    xlApp.Quit()

        '    rnRange = Nothing
        '    wsSheet = Nothing
        '    wbBook = Nothing
        '    xlApp = Nothing
        'End Sub

#End Region

#Region "Testing Only"
        Public Function TransferProductsToExcel() As DataSet
            ' Transfer data from SQL Server to Excel
            Try
                Dim data As DataSet = GetProductData()
                Dim file As String = "C:\Output.xls"
                TransferToExcel(data, file)
                'MsgBox("Product data has been transferred to Excel.")
                Return data
            Catch ex As Exception
                'MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Function


        Public Function GetProductData() As DataSet
            Dim ret As DataSet
            Dim sql As String = "SELECT UM_USER_ID, UM_USER_NAME FROM USER_MSTR"
            ret = ObjDb.FillDs(sql, False)
            Return ret
        End Function


        Public Sub TransferToExcel(ByVal source As DataSet, ByVal fileName As String)
            'OpenConnToExcel(fileName)
            Dim cn As New OleDb.OleDbConnection(String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties={1}Excel 8.0;HDR=Yes{1}", fileName, ControlChars.Quote))
            Dim cmd As New OleDb.OleDbCommand(String.Empty, cn)

            Try
                ' Create the worksheet
                If IO.File.Exists(fileName) Then IO.File.Delete(fileName)
                cn.Open()
                cmd.CommandText = "CREATE TABLE [Products] ([ID]  VARCHAR(100), [Name] VARCHAR(255))"
                cmd.ExecuteNonQuery()

                ' Transfer the data to the worksheet
                cmd.CommandText = "INSERT INTO [Products] ([ID],[Name]) VALUES (?,?)"
                With cmd.Parameters
                    .Add("@id", OleDb.OleDbType.VarChar, 100, "UM_USER_ID")
                    .Add("@name", OleDb.OleDbType.VarChar, 255, "UM_USER_NAME")
                End With

                Dim da As New OleDb.OleDbDataAdapter
                da.InsertCommand = cmd
                da.Update(source)
            Finally
                cn.Close()
                cmd.Dispose()
            End Try
        End Sub

        Public Sub WriteToExcelUseQueryTable()
            'Dim objExcel As Object
            Dim objExcel As New Excel.Application
            Dim objBook As Excel.Workbook
            Dim objSheet As Excel.Worksheet
            Dim strSQL As String
            'objExcel = CreateObject("Excel.Application")
            objBook = objExcel.Workbooks.Add
            objSheet = objBook.Worksheets(1)
            ' objSheet.QueryTables.Add()
            strSQL = "SELECT * FROM USER_MSTR"
            'OBJSHEET.
            'Set up the Query Table and tell it where to find the data. 
            Dim objQryTable As Excel.QueryTable
            objQryTable = objSheet.QueryTables.Add("OLEDB;Provider=sqloledb;Data Source=txserver" &
                ";Initial Catalog=Wheel" &
                ";Trusted Connection=Yes;Integrated Security=SSPI;", objSheet.Range("A1"), strSQL)

            objQryTable.RefreshStyle = 2 ' x1InsertEntire Rows = 2 
            objQryTable.Refresh(False)

            objBook.SaveAs("C:\myexcel.xls")
            'Clear everything so you can display it to the user 
            objQryTable = Nothing
            objSheet = Nothing
            objBook = Nothing
            objExcel.Quit()
            objExcel = Nothing

            ''If you want to open the saved Excel File:
            'Dim Response As String
            'Response = Common.NetMsgbox("Would you like to open " & "test.xls" & "?", MsgBoxStyle.Information.YesNo)

            'If Response = vbYes Then
            '    Dim xlApp As Excel.Application
            '    Dim xlMappe As Excel.Workbook
            '    xlApp = New Excel.Application()
            '    xlApp.Visible = True
            '    xlMappe = xlApp.Workbooks.Open("C:\test.xls")
            'Else 'If vbNo
            '    'Do nothing, just get's rid of the Msgbox.
            'End If

        End Sub

        Public Sub WriteDsToExcelUseLooping(ByVal ds As DataSet, ByVal pPath As String)
            Dim ctx As Web.HttpContext = Web.HttpContext.Current

            Dim strSourceFile As String = ctx.Server.MapPath(ctx.Request.ApplicationPath) & "\Template\ProductList.xls"
            Dim strDestFile As String = ctx.Server.MapPath(ctx.Request.ApplicationPath) & "\Template\ProductListLing.xls"
            If File.Exists(strDestFile) Then
                File.Delete(strDestFile)
            End If
            File.Copy(strSourceFile, strDestFile)

            OpenConnToExcel(pPath, True)

            Dim SqlAry(0) As String
            Dim lsSql As String
            Dim intLoop, intLoop1, intTotRow, intTotCol, TotalRow As Integer

            'TotalRow = ds.Tables(0).Rows.Count
            'intTotRow = ds.Tables(0).Rows.Count - 1
            'intTotCol = ds.Tables(0).Columns.Count

            '//AVOID Excel file kenot open and Company Id not exist
            'If TotalRow > 0 Then
            '//"UPDATE  [SHELL$B6:B6] SET F1='ARMADA 6  01/2005/V0023'"
            'For intLoop = 2 To 2
            'lsSql = "Insert into [Sheet1$] (Item_ID, Company_Id, Item_Desc, UNSPSC_Code, UNSPSC_Desc, Unit_Cost, Currency_Code, UOM, Tax_Code, Mgmt_Code, Mgmt_Text, Vendor_Item_Code, Brand, Model, Actions) Values ('1a#','2a#','3a#','4a#','a','a','a','a','a','a','a','a','a','a','a')"
            'lsSql = "Insert into [Sheet1$H3:I3] Values ('a','" + "hih#di" + "')" '///'" + "Bob" + "'
            lsSql = "Insert into [Sheet1$H3:I3] Values ('a','hi#h')"
            'lsSql = lsSql & "'" & (intLoop + 1) & "',"
            'For intLoop1 = 0 To 0
            '    lsSql = lsSql & "'" & ds.Tables(0).Rows(intLoop)(intLoop1) & "'"
            'Next
            'lsSql = lsSql & "'M')"
            'lsSql &= ")"

            'lsSql = "Insert into [Sheet1$B3:P3] Values ('1b1','1c','1d','1e','1f1','1g','1h','c','a','s','s','u','r','y','l')"
            Common.Insert2Ary(SqlAry, lsSql)
            'lsSql = "Insert into [Sheet1$] (Item_ID, Company_Id, Item_Desc, UNSPSC_Code, UNSPSC_Desc, Unit_Cost, Currency_Code, UOM, Tax_Code, Mgmt_Code, Mgmt_Text, Vendor_Item_Code, Brand, Model, Actions) Values ('1a##','2a##','3a##','4a##','a#','a#','a#','a#','a#','a','a','a','a','a','a')"
            'Common.Insert2Ary(SqlAry, lsSql)
            lsSql = "Insert into [Sheet1$A4:P4] Values ('b','c','d','e','f','g','h','c','a','s','s','u','r','y','g','a')"
            Common.Insert2Ary(SqlAry, lsSql)

            lsSql = "Insert into [Sheet1$B5:D5] Values ('1111111','LIFTING SUPPORT BELT 1  USING OR LIFTING HEAVY ITEMSdddddddddudddddddddddddddddddddddddddoooooooooood','OOOOOOOO')"
            Common.Insert2Ary(SqlAry, lsSql)

            'lsSql = "Insert into [Sheet1$H5:O5] Values ('[eee#]','rrr','zs','ds','dsd','fsf','ds','ds')"
            'lsSql = "Insert into [Sheet1$H3:I3] Values ('a#','da')"
            'Common.Insert2Ary(SqlAry, lsSql)

            'lsSql = "Insert into [Sheet1$A2:G2] Values ('eee','rrr','zs','ds','dsd','fsf','ds')"
            'Common.Insert2Ary(SqlAry, lsSql)
            'Next

            ObjDb.BatchExecute(SqlAry)
            ObjDb = Nothing
            'Else
            '    Exit Sub
            'End If
        End Sub

        Public Sub Writecell_Common(ByVal ds As DataSet, ByVal pPath As String)
            Dim ctx As Web.HttpContext = Web.HttpContext.Current
            Dim strSourceFile, strDestFile, filename As String
            strSourceFile = ctx.Server.MapPath(ctx.Request.ApplicationPath) & "\Common\Template\ItemBIMListingTemplate.xls"
            filename = "ItemBIMListing_" & HttpContext.Current.Session("CompanyId") & "_" & HttpContext.Current.Session("UserId") & ".xls"
            strDestFile = System.Configuration.ConfigurationManager.AppSettings("TemplateTemp") & filename

            If File.Exists(strDestFile) Then
                File.Delete(strDestFile)
            End If
            File.Copy(strSourceFile, strDestFile)

            OpenConnToExcel(pPath, False)

            Dim SqlAry(0) As String
            Dim lsSql As String
            Dim i As Integer
            Dim intLoop, intLoop1, intTotRow, intTotCol, TotalRow As Integer

            TotalRow = ds.Tables(0).Rows.Count
            intTotRow = ds.Tables(0).Rows.Count + 1
            intTotCol = ds.Tables(0).Columns.Count

            If TotalRow > 0 Then
                For intLoop = 2 To intTotRow
                    'lsSql = "Insert into [Sheet1$A" & intLoop & ":D" & intLoop & "] Values ("
                    lsSql = "Insert into [BIM$] Values ("
                    lsSql = lsSql & "'" & (intLoop - 1) & "',"
                    For intLoop1 = 0 To intTotCol - 1
                        If IsDBNull(ds.Tables(0).Rows(intLoop - 2)(intLoop1)) Then
                            lsSql = lsSql & "'" & ds.Tables(0).Rows(intLoop - 2)(intLoop1) & "',"
                        Else
                            lsSql = lsSql & "'" & Common.Parse(ds.Tables(0).Rows(intLoop - 2)(intLoop1)) & "',"
                        End If
                    Next
                    lsSql = lsSql & "'M')"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next
                ObjDb.BatchExecuteOle(SqlAry)
                ObjDb = Nothing
            Else
                Exit Sub
            End If
        End Sub

        Public Sub Writecell(ByVal ds As DataSet, ByVal pPath As String)
            Dim ctx As Web.HttpContext = Web.HttpContext.Current
            Dim strSourceFile, strDestFile, filename As String
            strSourceFile = ctx.Server.MapPath(ctx.Request.ApplicationPath) & "\FTN\Template\ItemBIMListingTemplate.xls"
            filename = "ItemBIMListing_" & HttpContext.Current.Session("CompanyId") & "_" & HttpContext.Current.Session("UserId") & ".xls"
            strDestFile = ConfigurationManager.AppSettings("TemplateTemp") & filename

            If File.Exists(strDestFile) Then
                File.Delete(strDestFile)
            End If
            File.Copy(strSourceFile, strDestFile)

            OpenConnToExcel(pPath, False)

            Dim SqlAry(0) As String
            Dim lsSql As String
            Dim i As Integer
            Dim intLoop, intLoop1, intTotRow, intTotCol, TotalRow As Integer

            TotalRow = ds.Tables(0).Rows.Count
            intTotRow = ds.Tables(0).Rows.Count + 1
            intTotCol = ds.Tables(0).Columns.Count

            If TotalRow > 0 Then
                For intLoop = 2 To intTotRow
                    'lsSql = "Insert into [Sheet1$A" & intLoop & ":D" & intLoop & "] Values ("
                    lsSql = "Insert into [BIM$] Values ("
                    lsSql = lsSql & "'" & (intLoop - 1) & "',"
                    For intLoop1 = 0 To intTotCol - 1
                        If IsDBNull(ds.Tables(0).Rows(intLoop - 2)(intLoop1)) Then
                            lsSql = lsSql & "'" & ds.Tables(0).Rows(intLoop - 2)(intLoop1) & "',"
                        Else
                            lsSql = lsSql & "'" & Common.Parse(ds.Tables(0).Rows(intLoop - 2)(intLoop1)) & "',"
                        End If
                    Next
                    lsSql = lsSql & "'M')"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next
                ObjDb.BatchExecuteOle(SqlAry)
                ObjDb = Nothing
            Else
                Exit Sub
            End If
        End Sub

        Public Sub WritecellVIM(ByVal ds As DataSet, ByVal dsGSTRate As DataSet, ByVal pPath As String)
            Dim ctx As Web.HttpContext = Web.HttpContext.Current
            Dim strSourceFile, strDestFile, filename As String
            'Dim strSourceFile As String = ctx.Server.MapPath(ctx.Request.ApplicationPath) & "\Common\Template\ItemVIMListingTemplate.xls"
            'Dim strDestFile As String = ctx.Server.MapPath(ctx.Request.ApplicationPath) & "\Common\Template\ItemVIMListing.xls"

            strSourceFile = ctx.Server.MapPath(ctx.Request.ApplicationPath) & "\Common\Template\ItemVIMListingTemplate.xls"
            filename = "ItemVIMListing_" & HttpContext.Current.Session("CompanyId") & "_" & HttpContext.Current.Session("UserId") & ".xls"
            strDestFile = ConfigurationManager.AppSettings("TemplateTemp") & filename

            If File.Exists(strDestFile) Then
                File.Delete(strDestFile)
            End If
            File.Copy(strSourceFile, strDestFile)

            OpenConnToExcel(pPath, False)

            Dim SqlAry(0) As String
            Dim lsSql As String
            Dim i As Integer
            Dim intLoop, intLoop1, intTotRow, intTotCol, TotalRow As Integer

            If dsGSTRate.Tables(0).Rows.Count > 0 Then
                'GST Rate
                For i = 0 To dsGSTRate.Tables(0).Rows.Count - 1
                    lsSql = "Insert into [PredefineCode$A" & i + 10 & ":A" & i + 10 & "] Values ('" & Common.Parse(dsGSTRate.Tables(0).Rows(i).Item("GST Rate")) & "')"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next

            End If

            TotalRow = ds.Tables(0).Rows.Count
            intTotRow = ds.Tables(0).Rows.Count + 1
            intTotCol = ds.Tables(0).Columns.Count

            If TotalRow > 0 Then
                For intLoop = 2 To intTotRow
                    'lsSql = "Insert into [Sheet1$A" & intLoop & ":D" & intLoop & "] Values ("
                    'lsSql = "Insert into [VIM$A" & intLoop & ":Y" & intLoop & "] Values ("
                    lsSql = "Insert into [VIM$] Values ("
                    lsSql = lsSql & "'" & (intLoop - 1) & "',"
                    For intLoop1 = 0 To intTotCol - 1
                        If IsDBNull(ds.Tables(0).Rows(intLoop - 2)(intLoop1)) Then
                            lsSql = lsSql & "'" & ds.Tables(0).Rows(intLoop - 2)(intLoop1) & "',"
                        Else
                            lsSql = lsSql & "'" & Common.Parse(ds.Tables(0).Rows(intLoop - 2)(intLoop1)) & "',"
                        End If
                    Next
                    lsSql = lsSql & "'M')"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next
                ObjDb.BatchExecuteOle(SqlAry)
                ObjDb = Nothing
            Else
                Exit Sub
            End If
        End Sub
        Public Function PreviewTT(ByVal dsdocdetail As DataSet, ByVal dslinedetail As DataSet, ByVal dsapplicantdetail As DataSet, ByVal dsbankdetail As DataSet, ByVal FIN As String, ByVal exchangerate As String, ByVal bankCharge As String) As String
            Dim ctx As Web.HttpContext = Web.HttpContext.Current
            Dim filename As String
            Dim todayDate As String
            Dim numb As String = "1"
            todayDate = Date.Today.ToString("yyMMdd")

            filename = "TT-" & todayDate.Replace("/", "") & "-" & numb
            Dim strSourceFile As String = ctx.Server.MapPath(ctx.Request.ApplicationPath) & "\Common\Template\TT_template.xls"
            Dim strDestFile As String = ctx.Server.MapPath(ctx.Request.ApplicationPath) & "\Common\Report\Temp\" & filename & ".xls"

            While File.Exists(strDestFile)
                numb = CInt(numb) + 1
                filename = filename.Substring(0, 10) & addLeadingZero(numb, "1")
                strDestFile = ctx.Server.MapPath(ctx.Request.ApplicationPath) & "\Common\Report\Temp\" & filename & ".xls"
            End While

            File.Copy(strSourceFile, strDestFile)

            OpenConnToExcel(strDestFile, False)
            'FILL IN DETAIL IN TT
            Dim SqlAry(0) As String
            Dim Sql As String
            Dim invoiceAmt As String
            Dim wthtax As Integer
            Dim wthtaxAmt As Decimal
            'Check with holding tax
            If dsdocdetail.Tables(0).Rows(0).Item("IM_WITHHOLDING_OPT") = "1" Then
                invoiceAmt = dsdocdetail.Tables(0).Rows(0).Item("IM_INVOICE_TOTAL")
            ElseIf dsdocdetail.Tables(0).Rows(0).Item("IM_WITHHOLDING_OPT") = "2" Then
                wthtax = dsdocdetail.Tables(0).Rows(0).Item("IM_WITHHOLDING_TAX")
                wthtaxAmt = dsdocdetail.Tables(0).Rows(0).Item("IM_INVOICE_TOTAL") * wthtax / 100
                invoiceAmt = CStr(dsdocdetail.Tables(0).Rows(0).Item("IM_INVOICE_TOTAL") - wthtaxAmt)
            Else
                invoiceAmt = dsdocdetail.Tables(0).Rows(0).Item("IM_INVOICE_TOTAL")
            End If

            'system date
            Sql = "insert into [TT$I1:I1] Values('" & Date.Today.ToString.Replace("/", "-").Substring(0, 10) & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'particular of applicant
            'name
            Sql = "insert into [TT$C2:C2] Values('" & Common.Parse(dsapplicantdetail.Tables(0).Rows(0).Item("CM_COY_NAME")) & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'kp
            'Sql = "insert into [TT$C4:C4] Values('" & dsapplicantdetail.Tables(0).Rows(0).Item("IM_INVOICE_TOTAL") & "')"
            'Common.Insert2Ary(SqlAry, Sql)
            'exchange rate
            Sql = "insert into [TT$E3:E3] Values('" & exchangerate & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'currency code 1
            Sql = "insert into [TT$F3:F3] Values('" & dsdocdetail.Tables(0).Rows(0).Item("IM_CURRENCY_CODE").ToString.Substring(0, 1) & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'currency code 2
            Sql = "insert into [TT$G3:G3] Values('" & dsdocdetail.Tables(0).Rows(0).Item("IM_CURRENCY_CODE").ToString.Substring(1, 1) & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'currency code 3
            Sql = "insert into [TT$H3:H3] Values('" & dsdocdetail.Tables(0).Rows(0).Item("IM_CURRENCY_CODE").ToString.Substring(2, 1) & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'invoice amount
            Sql = "insert into [TT$I3:I3] Values('" & FormatNumber(invoiceAmt) & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'address
            Dim address As String
            address = Common.Parse(dsapplicantdetail.Tables(0).Rows(0).Item("CM_ADDR_LINE1")) & ","
            If dsapplicantdetail.Tables(0).Rows(0).Item("CM_ADDR_LINE2") <> "" Then
                address &= Common.Parse(dsapplicantdetail.Tables(0).Rows(0).Item("CM_ADDR_LINE2")) & ","
            End If
            If dsapplicantdetail.Tables(0).Rows(0).Item("CM_ADDR_LINE3") <> "" Then
                address &= Common.Parse(dsapplicantdetail.Tables(0).Rows(0).Item("CM_ADDR_LINE3")) & ","
            End If
            If dsapplicantdetail.Tables(0).Rows(0).Item("CM_POSTCODE") <> "" Then
                address &= dsapplicantdetail.Tables(0).Rows(0).Item("CM_POSTCODE") & ","
            End If
            If dsapplicantdetail.Tables(0).Rows(0).Item("CM_CITY") <> "" Then
                address &= Common.Parse(dsapplicantdetail.Tables(0).Rows(0).Item("CM_CITY")) & ","
            End If
            Sql = "insert into [TT$C4:C4] Values('" & address.Substring(0, address.Length - 1) & "')"
            Common.Insert2Ary(SqlAry, Sql)

            'particular of beneficiary
            'name
            Sql = "insert into [TT$C7:C7] Values('" & Common.Parse(dsdocdetail.Tables(0).Rows(0).Item("IC_COY_NAME")) & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'amount in RM with 2 decimal place
            Dim amountRM As Decimal
            amountRM = CDec(exchangerate) * CDec(invoiceAmt)
            If bankCharge <> "" Then
                amountRM = amountRM - bankCharge
            End If
            Sql = "insert into [TT$I9:I9] Values('" & FormatNumber(amountRM, 2) & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'Address
            address = Common.Parse(dsdocdetail.Tables(0).Rows(0).Item("IM_ADDR_LINE1")) & ","
            If dsdocdetail.Tables(0).Rows(0).Item("IM_ADDR_LINE2") <> "" Then
                address &= Common.Parse(dsdocdetail.Tables(0).Rows(0).Item("IM_ADDR_LINE2")) & ","
            End If
            If dsdocdetail.Tables(0).Rows(0).Item("IM_ADDR_LINE3") <> "" Then
                address &= Common.Parse(dsdocdetail.Tables(0).Rows(0).Item("IM_ADDR_LINE3")) & ","
            End If
            If dsdocdetail.Tables(0).Rows(0).Item("IM_POSTCODE") <> "" Then
                address &= Common.Parse(dsdocdetail.Tables(0).Rows(0).Item("IM_POSTCODE")) & ","
            End If
            If dsdocdetail.Tables(0).Rows(0).Item("IM_CITY") <> "" Then
                address &= dsdocdetail.Tables(0).Rows(0).Item("IM_CITY") & ","
            End If
            Sql = "insert into [TT$C9:C9] Values('" & address.Substring(0, address.Length - 1) & "')"
            Common.Insert2Ary(SqlAry, Sql)

            'country of beneficiary
            Sql = "insert into [TT$D9:D9] Values('" & Common.Parse(dsdocdetail.Tables(0).Rows(0).Item("CODE_DESC")) & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'particular of beneficiary bank
            'name
            Sql = "insert into [TT$C12:C12] Values('" & Common.Parse(dsbankdetail.Tables(0).Rows(0).Item("BC_BANK_NAME")) & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'account
            Sql = "insert into [TT$C13:C13] Values('" & dsbankdetail.Tables(0).Rows(0).Item("IC_BANK_ACCT") & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'address
            address = Common.Parse(dsbankdetail.Tables(0).Rows(0).Item("IC_BANK_ADDR_LINE1")) & ","
            If dsbankdetail.Tables(0).Rows(0).Item("IC_BANK_ADDR_LINE2") <> "" Then
                address &= Common.Parse(dsbankdetail.Tables(0).Rows(0).Item("IC_BANK_ADDR_LINE2")) & ","
            End If
            If dsbankdetail.Tables(0).Rows(0).Item("IC_BANK_ADDR_LINE3") <> "" Then
                address &= Common.Parse(dsbankdetail.Tables(0).Rows(0).Item("IC_BANK_ADDR_LINE3")) & ","
            End If
            If dsbankdetail.Tables(0).Rows(0).Item("IC_BANK_POSTCODE") <> "" Then
                address &= dsbankdetail.Tables(0).Rows(0).Item("IC_BANK_POSTCODE") & ","
            End If
            If dsbankdetail.Tables(0).Rows(0).Item("IC_BANK_CITY") <> "" Then
                address &= dsbankdetail.Tables(0).Rows(0).Item("IC_BANK_CITY") & ","
            End If
            If dsbankdetail.Tables(0).Rows(0).Item("IC_BANK_STATE") <> "" Then
                address &= ObjDb.Get1Column("CODE_MSTR", "CODE_DESC", " WHERE CODE_ABBR='" & dsbankdetail.Tables(0).Rows(0).Item("IC_BANK_STATE") & "'") & ","
            End If
            If dsbankdetail.Tables(0).Rows(0).Item("CODE_DESC") <> "" Then
                address &= dsbankdetail.Tables(0).Rows(0).Item("CODE_DESC") & ","
            End If
            Sql = "insert into [TT$C14:C14] Values('" & address.Substring(0, address.Length - 1) & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'COUNTRY
            Sql = "insert into [TT$D14:D14] Values('" & dsbankdetail.Tables(0).Rows(0).Item("CODE_DESC") & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'invoice line description
            'ref
            'Sql = "insert into [TT$C17:C17] Values('" & FIN & "')"
            'Common.Insert2Ary(SqlAry, Sql)
            'invoice line description 1
            'Dim i As Integer
            'For i = 1 To dslinedetail.Tables(0).Rows.Count
            '    If i = 1 Then
            '        Sql = "insert into [TT$C16:C16] Values('" & dslinedetail.Tables(0).Rows(i - 1).Item("ID_PRODUCT_DESC") & "')"
            '        Common.Insert2Ary(SqlAry, Sql)
            '    ElseIf i = 2 Then
            '        Sql = "insert into [TT$C16:C16] Values('" & dslinedetail.Tables(0).Rows(i - 1).Item("ID_PRODUCT_DESC") & "')"
            '        Common.Insert2Ary(SqlAry, Sql)
            '    End If
            'Next
            'Sql = "insert into [TT$C17:C17] Values('" & FIN & "')"
            'Common.Insert2Ary(SqlAry, Sql)
            'INV no
            Sql = "insert into [TT$C15:C15] Values('" & dsdocdetail.Tables(0).Rows(0).Item("IM_INVOICE_NO") & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'invoice line description 1
            'Dim i As Integer
            'For i = 1 To dslinedetail.Tables(0).Rows.Count
            'If i = 1 And Len(dslinedetail.Tables(0).Rows(i - 1).Item("ID_PRODUCT_DESC")) >= 30 Then
            If Len(Common.Parse(dslinedetail.Tables(0).Rows(0).Item("ID_PRODUCT_DESC"))) >= 50 Then
                Sql = "insert into [TT$C16:C16] Values('" & Common.Parse(dslinedetail.Tables(0).Rows(0).Item("ID_PRODUCT_DESC").ToString.Substring(0, 30)) & "')"
            Else
                Sql = "insert into [TT$C16:C16] Values('" & Common.Parse(dslinedetail.Tables(0).Rows(0).Item("ID_PRODUCT_DESC")) & "')"

                'ElseIf i = 2 Then
                '    Sql = "insert into [TT$C18:C18] Values('" & dslinedetail.Tables(0).Rows(i - 1).Item("ID_PRODUCT_DESC") & "')"
                '    Common.Insert2Ary(SqlAry, Sql)
            End If
            Common.Insert2Ary(SqlAry, Sql)

            'Zulham
            'Sql = "insert into [TT$C17:C17] Values('" & dsdocdetail.Tables(0).Rows(0).Item("IM_REMARKS2") & "')"
            'Common.Insert2Ary(SqlAry, Sql)
            'End
            'Michael
            Sql = "insert into [TT$C17:C17] Values('" & dsdocdetail.Tables(0).Rows(0).Item("BC_BANK_CODE") & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'End

            ObjDb.BatchExecuteOle(SqlAry)
            ObjDb = Nothing
            Return filename


        End Function
        Public Sub WritecellTT(ByVal dsdocdetail As DataSet, ByVal dslinedetail As DataSet, ByVal dsapplicantdetail As DataSet, ByVal dsbankdetail As DataSet, ByVal FIN As String, ByVal bankCharge As String)
            Dim ctx As Web.HttpContext = Web.HttpContext.Current
            Dim filename As String
            Dim todayDate As String
            Dim numb As String = "1"
            todayDate = Date.Today.ToString("yyMMdd")

            filename = "TT-" & todayDate.Replace("/", "") & "-" & addLeadingZero(numb, "1")
            Dim strSourceFile As String = ctx.Server.MapPath(ctx.Request.ApplicationPath) & "\Common\Template\TT_template.xls"
            Dim strDestFile As String = ConfigurationManager.AppSettings("HLBPaymentFilePath") & "\" & filename & ".xls"

            While File.Exists(strDestFile)
                numb = CInt(numb) + 1
                filename = filename.Substring(0, 10) & addLeadingZero(numb, "1")
                strDestFile = ConfigurationManager.AppSettings("HLBPaymentFilePath") & "\" & filename & ".xls"
            End While

            File.Copy(strSourceFile, strDestFile)

            OpenConnToExcel(strDestFile, False)
            'FILL IN DETAIL IN TT
            Dim SqlAry(0) As String
            Dim Sql As String
            Dim invoiceAmt As String
            Dim wthtax As Integer
            Dim wthtaxAmt As Decimal
            'Check with holding tax

            If dsdocdetail.Tables(0).Rows(0).Item("IM_WITHHOLDING_OPT") = "1" Then
                invoiceAmt = dsdocdetail.Tables(0).Rows(0).Item("IM_INVOICE_TOTAL")
            ElseIf dsdocdetail.Tables(0).Rows(0).Item("IM_WITHHOLDING_OPT") = "2" Then
                wthtax = dsdocdetail.Tables(0).Rows(0).Item("IM_WITHHOLDING_TAX")
                wthtaxAmt = dsdocdetail.Tables(0).Rows(0).Item("IM_INVOICE_TOTAL") * wthtax / 100
                invoiceAmt = CStr(dsdocdetail.Tables(0).Rows(0).Item("IM_INVOICE_TOTAL") - wthtaxAmt)
            Else
                invoiceAmt = dsdocdetail.Tables(0).Rows(0).Item("IM_INVOICE_TOTAL")
            End If
            'system date
            Sql = "insert into [TT$I1:I1] Values('" & Format(CDate(Date.Today.ToString.Replace("/", "-").Substring(0, 10)), "d/MMM/yyyy") & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'particular of applicant
            'name
            Sql = "insert into [TT$C2:C2] Values('" & dsapplicantdetail.Tables(0).Rows(0).Item("CM_COY_NAME") & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'kp
            'Sql = "insert into [TT$C4:C4] Values('" & dsapplicantdetail.Tables(0).Rows(0).Item("IM_INVOICE_TOTAL") & "')"
            'Common.Insert2Ary(SqlAry, Sql)
            'exchange rate
            Sql = "insert into [TT$E3:E3] Values('" & dsdocdetail.Tables(0).Rows(0).Item("IM_EXCHANGE_RATE") & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'currency code 1
            Sql = "insert into [TT$F3:F3] Values('" & dsdocdetail.Tables(0).Rows(0).Item("IM_CURRENCY_CODE").ToString.Substring(0, 1) & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'currency code 2
            Sql = "insert into [TT$G3:G3] Values('" & dsdocdetail.Tables(0).Rows(0).Item("IM_CURRENCY_CODE").ToString.Substring(1, 1) & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'currency code 3
            Sql = "insert into [TT$H3:H3] Values('" & dsdocdetail.Tables(0).Rows(0).Item("IM_CURRENCY_CODE").ToString.Substring(2, 1) & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'invoice amount
            Sql = "insert into [TT$I3:I3] Values('" & FormatNumber(invoiceAmt) & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'address
            Dim address As String
            address = dsapplicantdetail.Tables(0).Rows(0).Item("CM_ADDR_LINE1") & ","
            If dsapplicantdetail.Tables(0).Rows(0).Item("CM_ADDR_LINE2") <> "" Then
                address &= dsapplicantdetail.Tables(0).Rows(0).Item("CM_ADDR_LINE2") & ","
            End If
            If dsapplicantdetail.Tables(0).Rows(0).Item("CM_ADDR_LINE3") <> "" Then
                address &= dsapplicantdetail.Tables(0).Rows(0).Item("CM_ADDR_LINE3") & ","
            End If
            If dsapplicantdetail.Tables(0).Rows(0).Item("CM_POSTCODE") <> "" Then
                address &= dsapplicantdetail.Tables(0).Rows(0).Item("CM_POSTCODE") & ","
            End If
            If dsapplicantdetail.Tables(0).Rows(0).Item("CM_CITY") <> "" Then
                address &= dsapplicantdetail.Tables(0).Rows(0).Item("CM_CITY") & ","
            End If
            Sql = "insert into [TT$C4:C4] Values('" & address.Substring(0, address.Length - 1) & "')"
            Common.Insert2Ary(SqlAry, Sql)

            'particular of beneficiary
            'name
            Sql = "insert into [TT$C7:C7] Values('" & Common.Parse(dsdocdetail.Tables(0).Rows(0).Item("IC_COY_NAME")) & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'amount in RM with 2 decimal place
            Dim amountRM As Decimal
            amountRM = dsdocdetail.Tables(0).Rows(0).Item("IM_EXCHANGE_RATE") * CDec(invoiceAmt)
            If bankCharge <> "" Then
                amountRM = amountRM - bankCharge
            End If
            Sql = "insert into [TT$I9:I9] Values('" & FormatNumber(amountRM, 2) & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'Address
            address = Common.Parse(dsdocdetail.Tables(0).Rows(0).Item("IM_ADDR_LINE1")) & ","
            If dsdocdetail.Tables(0).Rows(0).Item("IM_ADDR_LINE2") <> "" Then
                address &= Common.Parse(dsdocdetail.Tables(0).Rows(0).Item("IM_ADDR_LINE2")) & ","
            End If
            If dsdocdetail.Tables(0).Rows(0).Item("IM_ADDR_LINE3") <> "" Then
                address &= Common.Parse(dsdocdetail.Tables(0).Rows(0).Item("IM_ADDR_LINE3")) & ","
            End If
            If dsdocdetail.Tables(0).Rows(0).Item("IM_POSTCODE") <> "" Then
                address &= Common.Parse(dsdocdetail.Tables(0).Rows(0).Item("IM_POSTCODE")) & ","
            End If
            If dsdocdetail.Tables(0).Rows(0).Item("IM_CITY") <> "" Then
                address &= dsdocdetail.Tables(0).Rows(0).Item("IM_CITY") & ","
            End If
            Sql = "insert into [TT$C9:C9] Values('" & address.Substring(0, address.Length - 1) & "')"
            Common.Insert2Ary(SqlAry, Sql)

            'country of beneficiary
            Sql = "insert into [TT$D9:D9] Values('" & dsdocdetail.Tables(0).Rows(0).Item("CODE_DESC") & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'particular of beneficiary bank
            'name
            Sql = "insert into [TT$C12:C12] Values('" & dsbankdetail.Tables(0).Rows(0).Item("BC_BANK_NAME") & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'account
            Sql = "insert into [TT$C13:C13] Values('" & dsbankdetail.Tables(0).Rows(0).Item("IC_BANK_ACCT") & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'address
            address = Common.Parse(dsbankdetail.Tables(0).Rows(0).Item("IC_BANK_ADDR_LINE1")) & ","
            If dsbankdetail.Tables(0).Rows(0).Item("IC_BANK_ADDR_LINE2") <> "" Then
                address &= Common.Parse(dsbankdetail.Tables(0).Rows(0).Item("IC_BANK_ADDR_LINE2")) & ","
            End If
            If dsbankdetail.Tables(0).Rows(0).Item("IC_BANK_ADDR_LINE3") <> "" Then
                address &= Common.Parse(dsbankdetail.Tables(0).Rows(0).Item("IC_BANK_ADDR_LINE3")) & ","
            End If
            If dsbankdetail.Tables(0).Rows(0).Item("IC_BANK_POSTCODE") <> "" Then
                address &= dsbankdetail.Tables(0).Rows(0).Item("IC_BANK_POSTCODE") & ","
            End If
            If dsbankdetail.Tables(0).Rows(0).Item("IC_BANK_CITY") <> "" Then
                address &= dsbankdetail.Tables(0).Rows(0).Item("IC_BANK_CITY") & ","
            End If
            If dsbankdetail.Tables(0).Rows(0).Item("IC_BANK_STATE") <> "" Then
                address &= ObjDb.Get1Column("CODE_MSTR", "CODE_DESC", " WHERE CODE_ABBR='" & dsbankdetail.Tables(0).Rows(0).Item("IC_BANK_STATE") & "'") & ","
            End If
            If dsbankdetail.Tables(0).Rows(0).Item("CODE_DESC") <> "" Then
                address &= dsbankdetail.Tables(0).Rows(0).Item("CODE_DESC") & ","
            End If
            Sql = "insert into [TT$C14:C14] Values('" & address.Substring(0, address.Length - 1) & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'COUNTRY
            Sql = "insert into [TT$D14:D14] Values('" & dsbankdetail.Tables(0).Rows(0).Item("CODE_DESC") & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'invoice line description
            'ref
            Sql = "insert into [TT$C17:C17] Values('" & FIN & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'INV no
            Sql = "insert into [TT$C15:C15] Values('" & dsdocdetail.Tables(0).Rows(0).Item("IM_INVOICE_NO") & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'invoice line description 1
            'Dim i As Integer
            'For i = 1 To dslinedetail.Tables(0).Rows.Count
            'If i = 1 And Len(dslinedetail.Tables(0).Rows(i - 1).Item("ID_PRODUCT_DESC")) >= 30 Then
            If Len(Common.Parse(dslinedetail.Tables(0).Rows(0).Item("ID_PRODUCT_DESC"))) >= 50 Then
                Sql = "insert into [TT$C16:C16] Values('" & Common.Parse(dslinedetail.Tables(0).Rows(0).Item("ID_PRODUCT_DESC")).ToString.Substring(0, 30) & "')"
            Else
                Sql = "insert into [TT$C16:C16] Values('" & Common.Parse(dslinedetail.Tables(0).Rows(0).Item("ID_PRODUCT_DESC")) & "')"

                'ElseIf i = 2 Then
                '    Sql = "insert into [TT$C18:C18] Values('" & dslinedetail.Tables(0).Rows(i - 1).Item("ID_PRODUCT_DESC") & "')"
                '    Common.Insert2Ary(SqlAry, Sql)
            End If
            Common.Insert2Ary(SqlAry, Sql)
            'Next


            ObjDb.BatchExecuteOle(SqlAry)
            ObjDb = Nothing


        End Sub
        Public Sub WritecellRENTAS(ByVal dsdocdetail As DataSet, ByVal dslinedetail As DataSet, ByVal FIN As String, ByVal bankCharge As String)
            Dim ctx As Web.HttpContext = Web.HttpContext.Current
            Dim filename As String
            Dim todayDate As String
            Dim numb As String = "1"
            todayDate = Date.Today.ToString("yyMMdd")
            filename = "RENTAS-" & todayDate.Replace("/", "") & "-" & addLeadingZero(numb, "1")

            'Zulham 12052015 IPP GST Stage 1
            'Rentas form showing wrong login company
            Dim strSourceFile As String = ""
            'Zulham 12082018 - PAMB
            'If HttpContext.Current.Session("CompanyId").ToString.Trim.ToUpper = "HLISB" Then
            '    strSourceFile = ctx.Server.MapPath(ctx.Request.ApplicationPath) & "\Common\Template\RENTAS_HLISB_template.xls"
            'ElseIf HttpContext.Current.Session("CompanyId").ToString.Trim.ToUpper = "HLB" Then
            strSourceFile = ctx.Server.MapPath(ctx.Request.ApplicationPath) & "\Common\Template\RENTAS_template.xls"
            'End If

            Dim strDestFile As String = ConfigurationManager.AppSettings("HLBPaymentFilePath") & "\" & filename & ".xls"

            While File.Exists(strDestFile)
                numb = CInt(numb) + 1
                filename = filename.Substring(0, 14) & addLeadingZero(numb, "1")
                strDestFile = ConfigurationManager.AppSettings("HLBPaymentFilePath") & "\" & filename & ".xls"
            End While

            File.Copy(strSourceFile, strDestFile)

            OpenConnToExcel(strDestFile, False)
            'FILL IN DETAIL IN RENTAS
            Dim SqlAry(0) As String
            Dim Sql As String
            Dim i As Integer
            Dim today As String
            Dim space, space1 As String
            today = Format(CDate(Date.Today.ToString.Replace("/", "-").Substring(0, 10)), "d-MMM-yyyy") 'Date.Today.ToString.Replace("/", "-").Substring(0, 3) & MonthName(CInt(Date.Today.ToString.Substring(3, 2)), True) & "-" & Date.Today.ToString.Substring(8, 2)
            'date   Sql = "insert into [TT$I1:I1] Values('" & Format(CDate(Date.Today.ToString.Replace("/", "-").Substring(0, 10)), "d/MMM/yyyy") & "')"         
            Sql = "insert into [Rentas$D2:D2] Values('" & today & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'payment advice to
            Sql = "insert into [Rentas$E3:E3] Values('" & Common.Parse(dsdocdetail.Tables(0).Rows(0).Item("IC_COY_NAME")) & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'kindly pay a sum of RM
            Sql = "insert into [Rentas$E9:E9] Values('" & FormatNumber(CDbl(dsdocdetail.Tables(0).Rows(0).Item("IM_INVOICE_TOTAL")) - CDbl(bankCharge), 2) & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'description of item
            Sql = "insert into [Rentas$F9:F9] Values('" & Common.Parse(dslinedetail.Tables(0).Rows(0).Item("ID_PRODUCT_DESC")) & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'payment doc no
            Sql = "insert into [Rentas$D10:D10] Values('" & dsdocdetail.Tables(0).Rows(0).Item("IM_INVOICE_NO") & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'amount
            Sql = "insert into [Rentas$G10:G10] Values('" & FormatNumber(dsdocdetail.Tables(0).Rows(0).Item("IM_INVOICE_TOTAL"), 2) & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'note
            Sql = "insert into [Rentas$B11:B11] Values('(Please take note that RENTAS bank charges of RM" & bankCharge & " has been deducted from the total amount.)')"
            Common.Insert2Ary(SqlAry, Sql)
            'value date Sql = "insert into [TT$I1:I1] Values('" & Format(CDate(Date.Today.ToString.Replace("/", "-").Substring(0, 10)), "d/MMM/yyyy") & "')"
            Sql = "insert into [Rentas$D13:D13] Values('" & today & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'Favouring
            Sql = "insert into [Rentas$D14:D14] Values('" & Common.Parse(dsdocdetail.Tables(0).Rows(0).Item("IC_COY_NAME")) & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'Account no
            Sql = "insert into [Rentas$D15:D15] Values('" & dsdocdetail.Tables(0).Rows(0).Item("IC_BANK_ACCT") & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'BANK row 1
            Sql = "insert into [Rentas$D16:D16] Values('" & dsdocdetail.Tables(0).Rows(0).Item("BC_BANK_NAME") & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'BANK row 2            
            Sql = "insert into [Rentas$D17:D17] Values('" & Common.Parse(dsdocdetail.Tables(0).Rows(0).Item("IC_BANK_ADDR_LINE1")) & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'BANK row 3
            Sql = "insert into [Rentas$D18:D18] Values('" & Common.Parse(dsdocdetail.Tables(0).Rows(0).Item("IC_BANK_ADDR_LINE2")) & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'BANK row 4
            Sql = "insert into [Rentas$D19:D19] Values('" & Common.Parse(dsdocdetail.Tables(0).Rows(0).Item("IC_BANK_ADDR_LINE3")) & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'BANK row 5
            If dsdocdetail.Tables(0).Rows(0).Item("IC_BANK_POSTCODE") = "" Then
                space = ""
            Else
                space = " "
            End If
            If dsdocdetail.Tables(0).Rows(0).Item("IC_BANK_CITY") = "" Then
                space1 = ""
            Else
                space1 = " "
            End If
            If dsdocdetail.Tables(0).Rows(0).Item("IC_BANK_ADDR_LINE2") = "" Then
                Sql = "insert into [Rentas$D18:D18] Values('" & dsdocdetail.Tables(0).Rows(0).Item("IC_BANK_POSTCODE") & space & dsdocdetail.Tables(0).Rows(0).Item("IC_BANK_CITY") & space1 & ObjDb.Get1Column("CODE_MSTR", "CODE_DESC", " WHERE CODE_ABBR='" & dsdocdetail.Tables(0).Rows(0).Item("IC_BANK_STATE") & "'") & "')"
                Common.Insert2Ary(SqlAry, Sql)
            ElseIf dsdocdetail.Tables(0).Rows(0).Item("IC_BANK_ADDR_LINE3") = "" Then
                Sql = "insert into [Rentas$D19:D19] Values('" & dsdocdetail.Tables(0).Rows(0).Item("IC_BANK_POSTCODE") & space & dsdocdetail.Tables(0).Rows(0).Item("IC_BANK_CITY") & space1 & ObjDb.Get1Column("CODE_MSTR", "CODE_DESC", " WHERE CODE_ABBR='" & dsdocdetail.Tables(0).Rows(0).Item("IC_BANK_STATE") & "'") & "')"
                Common.Insert2Ary(SqlAry, Sql)
            Else
                Sql = "insert into [Rentas$D20:D20] Values('" & dsdocdetail.Tables(0).Rows(0).Item("IC_BANK_POSTCODE") & space & dsdocdetail.Tables(0).Rows(0).Item("IC_BANK_CITY") & space1 & ObjDb.Get1Column("CODE_MSTR", "CODE_DESC", " WHERE CODE_ABBR='" & dsdocdetail.Tables(0).Rows(0).Item("IC_BANK_STATE") & "'") & "')"
                Common.Insert2Ary(SqlAry, Sql)
            End If

            'contract note            
            Sql = "insert into [Rentas$D21:D21] Values('" & dsdocdetail.Tables(0).Rows(0).Item("IM_REMARKS2") & "')"
            Common.Insert2Ary(SqlAry, Sql)

            'amount - FOR rentas dept to pass entries
            'DR AMOUNT
            Sql = "insert into [Rentas$F39:F39] Values('" & FormatNumber(CDbl(dsdocdetail.Tables(0).Rows(0).Item("IM_INVOICE_TOTAL")) - CDbl(bankCharge), 2) & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'cr amount
            Sql = "insert into [Rentas$F41:F41] Values('" & FormatNumber(CDbl(dsdocdetail.Tables(0).Rows(0).Item("IM_INVOICE_TOTAL")) - CDbl(bankCharge), 2) & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'ref - FOR rentas dept to pass entries DR and CR
            Sql = "insert into [Rentas$G39:G39] Values('" & FIN & "')"
            Common.Insert2Ary(SqlAry, Sql)
            Sql = "insert into [Rentas$G41:G41] Values('" & FIN & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'desc
            Sql = "insert into [Rentas$H39:H39] Values('" & Common.Parse(dslinedetail.Tables(0).Rows(0).Item("ID_PRODUCT_DESC")) & "')"
            Common.Insert2Ary(SqlAry, Sql)
            Sql = "insert into [Rentas$H41:H41] Values('" & Common.Parse(dslinedetail.Tables(0).Rows(0).Item("ID_PRODUCT_DESC")) & "')"
            Common.Insert2Ary(SqlAry, Sql)
            'FOR FINANCE TO PASS ENTRIES
            'DR AMOUNT  
            'Sql = "insert into [Rentas$F47:F47] Values('" & FormatNumber(dsdocdetail.Tables(0).Rows(0).Item("IM_INVOICE_TOTAL"), 2) & "')"
            'Common.Insert2Ary(SqlAry, Sql)
            ''DR REF
            'Sql = "insert into [Rentas$G47:G47] Values('" & FIN & "')"
            'Common.Insert2Ary(SqlAry, Sql)
            ''DR desc
            'Sql = "insert into [Rentas$H47:H47] Values('" & Common.Parse(dslinedetail.Tables(0).Rows(0).Item("ID_PRODUCT_DESC")) & "')"
            'Common.Insert2Ary(SqlAry, Sql)
            ''CR AMOUNT  
            'Sql = "insert into [Rentas$F50:F50] Values('" & FormatNumber(CDbl(dsdocdetail.Tables(0).Rows(0).Item("IM_INVOICE_TOTAL")) - CDbl(bankCharge), 2) & "')"
            'Common.Insert2Ary(SqlAry, Sql)
            ''CR REF
            'Sql = "insert into [Rentas$G50:G50] Values('" & FIN & "')"
            'Common.Insert2Ary(SqlAry, Sql)
            ''CR Desc
            'Sql = "insert into [Rentas$H50:H50] Values('" & Common.Parse(dslinedetail.Tables(0).Rows(0).Item("ID_PRODUCT_DESC")) & "')"
            'Common.Insert2Ary(SqlAry, Sql)
            ''CR REF
            'Sql = "insert into [Rentas$G52:G52] Values('" & FIN & "')"
            'Common.Insert2Ary(SqlAry, Sql)
            ''bank charge            
            'Sql = "insert into [Rentas$F52:F52] Values('" & FormatNumber(bankCharge, 2) & "')"
            'Common.Insert2Ary(SqlAry, Sql)
            ''CR desc
            'Sql = "insert into [Rentas$H52:H52] Values('" & Common.Parse(dslinedetail.Tables(0).Rows(0).Item("ID_PRODUCT_DESC")) & "')"
            'Common.Insert2Ary(SqlAry, Sql)

            ObjDb.BatchExecuteOle(SqlAry)
            ObjDb = Nothing


        End Sub

        'Jules 2018.07.14 - PAMB - Modified to cater for Fund Type, Person Code, Project Code, Input Tax Code.
        Public Sub WriteCell_POUpload(ByVal ds As DataSet, ByVal pPath As String, ByVal ds1 As DataSet, ByVal ds3 As DataSet,
        ByVal ds4 As DataSet, ByVal ds5 As DataSet, ByVal ds6 As DataSet, Optional ByVal ds2 As DataSet = Nothing, Optional ByVal dsGSTTax As DataSet = Nothing, Optional ByVal isGST As String = "", Optional ByVal exceedCutOffDt As String = "",
        Optional ByVal dsFundType As DataSet = Nothing, Optional ByVal dsPersonCode As DataSet = Nothing, Optional ByVal dsProjectCode As DataSet = Nothing, Optional ByVal dsInputTax As DataSet = Nothing)
            Dim ctx As Web.HttpContext = Web.HttpContext.Current
            Dim SqlAry(0) As String
            Dim lsSql As String
            Dim i As Integer
            Dim intLoop, intLoop1, intTotRow, intTotCol, TotalRow As Integer
            Dim filename As String

            Dim strSourceFile As String = ctx.Server.MapPath(ctx.Request.ApplicationPath) & "\Common\Template\POTemplate.xls"

            'Jules 2018.07.14 - PAMB
            If HttpContext.Current.Session("CompanyId").ToString.ToLower = "pamb" Then
                strSourceFile = ctx.Server.MapPath(ctx.Request.ApplicationPath) & "\Common\Template\POTemplatePAMB.xls"
            End If


            filename = "POTemplate_" & HttpContext.Current.Session("CompanyId") & "_" & HttpContext.Current.Session("UserId") & ".xls"
            Dim strDestFile As String = ConfigurationManager.AppSettings("TemplateTemp") & filename

            If File.Exists(strDestFile) Then
                File.Delete(strDestFile)
            End If
            File.Copy(strSourceFile, strDestFile)
            OpenConnToExcel(pPath, False)

            TotalRow = ds.Tables(0).Rows.Count
            intTotRow = ds.Tables(0).Rows.Count + 1
            intTotCol = ds.Tables(0).Columns.Count

            If TotalRow > 0 Then
                lsSql = "Insert into [PO$C4:C4] Values ('" & ds.Tables(0).Rows(i).Item(0) & "')"
                Common.Insert2Ary(SqlAry, lsSql)
            End If

            'Billing Address Sept 25, 2013
            For _row As Integer = 0 To ds5.Tables(0).Rows.Count - 1
                If ds5.Tables(0).Rows(_row).Item("AM_ADDR_CODE") = HttpContext.Current.Session("FFPOAddrCode") Then
                    Dim _address As String
                    _address = Common.Parse(ds5.Tables(0).Rows(_row).Item("Address")) & ","
                    If Not ds5.Tables(0).Rows(_row).Item("AM_CITY") Is DBNull.Value Then
                        _address &= Common.Parse(ds5.Tables(0).Rows(_row).Item("AM_CITY")) & ","
                    End If
                    If Not ds5.Tables(0).Rows(_row).Item("STATE") Is DBNull.Value Then
                        _address &= Common.Parse(ds5.Tables(0).Rows(_row).Item("STATE")) & ","
                    End If
                    If Not ds5.Tables(0).Rows(_row).Item("AM_POSTCODE") Is DBNull.Value Then
                        _address &= Common.Parse(ds5.Tables(0).Rows(_row).Item("AM_POSTCODE")) & ","
                    End If
                    If Not ds5.Tables(0).Rows(_row).Item("COUNTRY") Is DBNull.Value Then
                        _address &= Common.Parse(ds5.Tables(0).Rows(_row).Item("COUNTRY")) & ","
                    End If

                    lsSql = "Insert into [PO$C5:C5] Values ('" & _address.Substring(0, _address.Length - 1) & "')"
                    Common.Insert2Ary(SqlAry, lsSql)

                End If
            Next

            'Renaming GST column

            ''GL Code
            For i = 0 To ds1.Tables(0).Rows.Count - 1
                'lsSql = "Insert into [PredefineCode$C" & i + 1 & ":C" & i + 1 & "] Values ('" & Common.Parse(ds1.Tables(0).Rows(i).Item("GL Code")) & "')"
                lsSql = "Insert into [PredefineCode$C" & i + 1 & ":C" & i + 1 & "] Values ('" & Common.Parse(ds1.Tables(0).Rows(i).Item("DESCRIPTION")) & "')" 'Jules 2018.09.05
                Common.Insert2Ary(SqlAry, lsSql)
            Next
            ''Category Code

            'Jules 2018.08.02 - Set to 'N/A'
            'For i = 0 To ds6.Tables(0).Rows.Count - 1
            '    lsSql = "Insert into [PredefineCode$D" & i + 1 & ":D" & i + 1 & "] Values ('" & Common.Parse(ds6.Tables(0).Rows(i).Item("Category Code")) & "')"
            '    Common.Insert2Ary(SqlAry, lsSql)
            'Next

            ''Budget Account
            For i = 0 To ds2.Tables(0).Rows.Count - 1
                lsSql = "Insert into [PredefineCode$G" & i + 1 & ":G" & i + 1 & "] Values ('" & Common.Parse(ds2.Tables(0).Rows(i).Item("Acct_List")) & "')"
                Common.Insert2Ary(SqlAry, lsSql)
            Next
            ''Asset Group
            For i = 0 To ds3.Tables(0).Rows.Count - 1
                lsSql = "Insert into [PredefineCode$E" & i + 1 & ":E" & i + 1 & "] Values ('" & Common.Parse(ds3.Tables(0).Rows(i).Item("AG_GROUP_DESC")) & "')"
                Common.Insert2Ary(SqlAry, lsSql)
            Next
            ''GST Tax
            If isGST = "Yes" Then
                For i = 0 To dsGSTTax.Tables(0).Rows.Count - 1
                    lsSql = "Insert into [PredefineCode$J" & i + 1 & ":J" & i + 1 & "] Values ('" & Common.Parse(dsGSTTax.Tables(0).Rows(i).Item("GST")) & "')"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next
            Else
                If exceedCutOffDt = "Yes" Then
                    lsSql = "Insert into [PredefineCode$J1:J1] Values ('N/A')"
                    Common.Insert2Ary(SqlAry, lsSql)
                End If
            End If

            ''Delivery Address
            For i = 0 To ds4.Tables(0).Rows.Count - 1
                Dim address As String
                address = Common.Parse(ds4.Tables(0).Rows(i).Item("Address")) & ","
                If Not ds4.Tables(0).Rows(i).Item("AM_CITY") Is DBNull.Value Then
                    address &= Common.Parse(ds4.Tables(0).Rows(i).Item("AM_CITY")) & ","
                End If
                If Not ds4.Tables(0).Rows(i).Item("STATE") Is DBNull.Value Then
                    address &= Common.Parse(ds4.Tables(0).Rows(i).Item("STATE")) & ","
                End If
                If Not ds4.Tables(0).Rows(i).Item("AM_POSTCODE") Is DBNull.Value Then
                    address &= Common.Parse(ds4.Tables(0).Rows(i).Item("AM_POSTCODE")) & ","
                End If
                If Not ds4.Tables(0).Rows(i).Item("COUNTRY") Is DBNull.Value Then
                    address &= Common.Parse(ds4.Tables(0).Rows(i).Item("COUNTRY")) & ","
                End If

                ''Jules 2013.12.10-begin.
                lsSql = "Insert into [PredefineCode$H" & i + 1 & ":H" & i + 1 & "] Values ('" & Common.Parse(ds4.Tables(0).Rows(i).Item("AM_ADDR_CODE")) & "')"
                Common.Insert2Ary(SqlAry, lsSql)

                lsSql = "Insert into [PredefineCode$I" & i + 1 & ":I" & i + 1 & "] Values ('" & address.Substring(0, address.Length - 1) & "')"
                Common.Insert2Ary(SqlAry, lsSql)
                ''Jules 2013.12.10-end.
            Next

            ''Bill To will always be relevant
            If Not ds5.Tables(0).Rows.Count = 0 Then
                For i = 0 To ds5.Tables(0).Rows.Count - 1
                    Dim address As String
                    address = Common.Parse(ds5.Tables(0).Rows(i).Item("Address")) & ","
                    If Not ds5.Tables(0).Rows(i).Item("AM_CITY") Is DBNull.Value Then
                        address &= Common.Parse(ds5.Tables(0).Rows(i).Item("AM_CITY")) & ","
                    End If
                    If Not ds5.Tables(0).Rows(i).Item("STATE") Is DBNull.Value Then
                        address &= Common.Parse(ds5.Tables(0).Rows(i).Item("STATE")) & ","
                    End If
                    If Not ds5.Tables(0).Rows(i).Item("AM_POSTCODE") Is DBNull.Value Then
                        address &= Common.Parse(ds5.Tables(0).Rows(i).Item("AM_POSTCODE")) & ","
                    End If
                    If Not ds5.Tables(0).Rows(i).Item("COUNTRY") Is DBNull.Value Then
                        address &= Common.Parse(ds5.Tables(0).Rows(i).Item("COUNTRY")) & ","
                    End If

                    lsSql = "Insert into [PredefineCode$B" & i + 1 & ":B" & i + 1 & "] Values ('" & address.Substring(0, address.Length - 1) & "')"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next

            End If

            'Jules 2018.07.14 - PAMB
            'Fund Type
            If Not dsFundType Is Nothing Then
                For i = 0 To dsFundType.Tables(0).Rows.Count - 1
                    lsSql = "Insert into [PredefineCode$L" & i + 1 & ":L" & i + 1 & "] Values ('" & Common.Parse(dsFundType.Tables(0).Rows(i).Item("AC_ANALYSIS_CODE_DESC")) & "')"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next
            End If

            'Person Code
            If Not dsPersonCode Is Nothing Then
                'Jules 2018.10.18
                'Jules 2018.09.05
                'lsSql = "Insert into [PredefineCode$M1:M1] Values ('N/A')"
                'Common.Insert2Ary(SqlAry, lsSql)
                'End modification.

                For i = 0 To dsPersonCode.Tables(0).Rows.Count - 1
                    'Jules 2018.10.18
                    'lsSql = "Insert into [PredefineCode$M" & i + 2 & ":M" & i + 2 & "] Values ('" & Common.Parse(dsPersonCode.Tables(0).Rows(i).Item("AC_ANALYSIS_CODE_DESC")) & "')"
                    lsSql = "Insert into [PredefineCode$M" & i + 1 & ":M" & i + 1 & "] Values ('" & Common.Parse(dsPersonCode.Tables(0).Rows(i).Item("AC_ANALYSIS_CODE_DESC")) & "')"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next
            End If

            'Project Code
            If Not dsProjectCode Is Nothing Then
                For i = 0 To dsProjectCode.Tables(0).Rows.Count - 1
                    lsSql = "Insert into [PredefineCode$N" & i + 1 & ":N" & i + 1 & "] Values ('" & Common.Parse(dsProjectCode.Tables(0).Rows(i).Item("AC_ANALYSIS_CODE_DESC")) & "')"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next
            End If

            'Input Tax
            If Not dsInputTax Is Nothing Then
                For i = 0 To dsInputTax.Tables(0).Rows.Count - 1
                    lsSql = "Insert into [PredefineCode$O" & i + 1 & ":O" & i + 1 & "] Values ('" & Common.Parse(dsInputTax.Tables(0).Rows(i).Item("GST")) & "')"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next
            End If
            'End modification

            ObjDb.BatchExecuteOle(SqlAry)

        End Sub

        Public Function ProtectWorkSheet_POUpload(ByVal pFileName As String, ByVal pGLCodeCnt As Integer, ByVal pCatCodeCnt As Integer,
        ByVal pAssetGroupCnt As Integer, ByVal pDeliveryAddrCnt As Integer, ByVal pBillToCnt As Integer, Optional ByVal dsCustName As DataSet = Nothing, Optional ByVal pBudgetAccCnt As Integer = Nothing, Optional ByVal pGSTTaxCnt As Integer = 0, Optional ByVal isGst As String = "", Optional ByVal exceedCutOffDt As String = "")
            Dim xlApp As New Excel.Application
            Dim xlWorkBook As Excel.Workbook
            Dim xlWorkSheet, xlWorkSheetPO As Excel.Worksheet
            Dim xlRange, xlRangeCatCode, xlRangeBCM, xlRangeAsset, xlRangeDelivery, xlRangeBill, xlRangePO, xlRowRange As Excel.Range
            Dim MyPassword As String = ConfigurationManager.AppSettings("ExcelPwd")
            Dim proc As System.Diagnostics.Process
            Dim intPID As Integer
            Dim intCustFields As Integer = 0
            Dim dsCustomField As DataSet

            Try
                xlWorkBook = xlApp.Workbooks.Open(pFileName)
                xlWorkSheet = xlWorkBook.Sheets("PredefineCode")
                xlWorkSheet.Range("A1:AU2000").Font.Name = "Arial Narrow"
                xlWorkSheet.Range("A1:AU1").Font.Bold = True
                xlWorkSheet.Range("A1:AU1").Interior.ColorIndex = 35

                If pGLCodeCnt > 0 Then
                    xlRange = xlWorkSheet.Range("C2:C" & pGLCodeCnt + 1)
                    xlRange.Name = "GLCode"
                End If
                If pCatCodeCnt > 0 Then
                    xlRangeCatCode = xlWorkSheet.Range("D2:D" & pCatCodeCnt + 1)
                    xlRangeCatCode.Name = "CatCode"
                End If
                If pBudgetAccCnt > 0 Then
                    xlRangeBCM = xlWorkSheet.Range("G2:G" & pBudgetAccCnt + 1)
                    xlRangeBCM.Name = "BudgetAct"
                End If
                If pGSTTaxCnt > 0 And isGst = "Yes" Then
                    xlRangeBCM = xlWorkSheet.Range("J2:J" & pGSTTaxCnt + 1)
                    xlRangeBCM.Name = "GSTRateTest"
                End If
                xlWorkSheetPO = xlWorkBook.Sheets("PO")

                If isGst = "No" Then
                    If exceedCutOffDt = "Yes" Then
                        xlWorkSheetPO.Range("I10:I59").Validation.InCellDropdown = False
                    Else
                        'normal
                        xlWorkSheetPO.Range("I9").Value = "Tax(%)"
                        xlWorkSheetPO.Range("I10:I59").Validation.InCellDropdown = False
                    End If
                End If
                ' Custom fields
                Dim i, j As Integer
                Dim xlRangeCustom As Excel.Range
                Dim strSql, lsSql As String
                Dim SqlAry(0) As String
                Dim NameBox As String
                Dim style As Excel.Style = xlWorkSheet.Application.ActiveWorkbook.Styles.Add("NewStyle")
                style.Font.Bold = True
                style.Font.Name = "Arial Narrow"
                style.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.PaleTurquoise)
                style.Borders.LineStyle = Excel.XlLineStyle.xlContinuous
                style.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.XlLineStyle.xlLineStyleNone
                style.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.XlLineStyle.xlLineStyleNone
                style.WrapText = True
                style.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter

                If Not dsCustName Is Nothing Then
                    intCustFields = dsCustName.Tables(0).Rows.Count
                    For i = 0 To dsCustName.Tables(0).Rows.Count - 1
                        Dim lbl As New System.Web.UI.WebControls.Label
                        lbl.ForeColor = Color.Red
                        lbl.Text = " * "
                        xlWorkSheetPO.Cells(9, 15 + i) = dsCustName.Tables(0).Rows(i).Item("CF_FIELD_NAME") & lbl.Text  'Zulham Okt 31, 2013
                        xlRowRange = xlWorkSheetPO.Range(Chr(79 + i) & "10:" & Chr(79 + i) & "10")

                        xlWorkSheetPO.Cells(9, 15 + i).Style = "NewStyle"
                        xlWorkSheet.Columns(11 + i).Insert()
                        xlWorkSheet.Cells(1, 11 + i) = dsCustName.Tables(0).Rows(i).Item("CF_FIELD_NAME")

                        strSql = "SELECT CF_FIELD_INDEX, CF_FIELD_VALUE, REPLACE(CF_FIELD_VALUE,' ','_') AS FIELDVALUE FROM CUSTOM_FIELDS WHERE CF_FIELD_NO ='" & dsCustName.Tables(0).Rows(i).Item("CF_FIELD_NO") & "' " &
                       "AND CF_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                        strSql &= " AND CF_MODULE='PO'"
                        strSql = strSql & " ORDER BY CF_FIELD_VALUE"
                        dsCustomField = ObjDb.FillDs(strSql)

                        If Not dsCustomField Is Nothing Then
                            For j = 0 To dsCustomField.Tables(0).Rows.Count - 1
                                xlWorkSheet.Cells(2 + j, 11 + i) = Common.Parse(dsCustomField.Tables(0).Rows(j).Item("CF_FIELD_VALUE"))
                                Dim r As Excel.Range = CType(xlWorkSheet.Cells(2 + j, 11 + i), Excel.Range)
                            Next
                            xlRangeCustom = xlWorkSheet.Range(Chr(75 + i) & "2:" & Chr(75 + i) & dsCustomField.Tables(0).Rows.Count + 1)
                            NameBox = Regex.Replace(dsCustName.Tables(0).Rows(i).Item("CF_FIELD_NAME"), "[^a-zA-Z0-9]", "_")
                            xlRangeCustom.Name = NameBox
                            'Zulham Oct 31, 2013
                            For _rowcount As Integer = 0 To 49
                                xlRowRange = xlWorkSheetPO.Range(Chr(79 + i) & 10 + _rowcount & ":" & Chr(79 + i) & 10 + _rowcount)
                                With xlRowRange.Validation
                                    .Add(Excel.XlDVType.xlValidateList, Excel.XlDVAlertStyle.xlValidAlertStop, Excel.XlFormatConditionOperator.xlBetween, "=" & NameBox)
                                    .InCellDropdown = True
                                End With
                            Next
                            'End
                        End If
                    Next
                End If

                For _rowcount As Integer = 0 To 49
                    xlWorkSheetPO.Cells(10 + _rowcount, 12) = Format(Now.AddDays(1), "dd/MM/yyyy") ''Jules 2013.12.10
                Next

                xlRangeAsset = xlWorkSheet.Range("E2:E" & pAssetGroupCnt + 1)
                xlRangeAsset.Name = "AssetGroup"
                If pAssetGroupCnt = 0 Then
                    xlWorkSheetPO = xlWorkBook.Sheets("PO")
                    xlRangePO = xlWorkSheetPO.Columns("E")
                    xlRangePO.Delete()
                End If
                xlRangeDelivery = xlWorkSheet.Range("H2:H" & pDeliveryAddrCnt + 1)
                xlRangeDelivery.Name = "DeliveryAddr"
                xlRangeBill = xlWorkSheet.Range("B2:B" & pBillToCnt + 1)
                xlRangeBill.Name = "Billing"

                xlWorkSheet.Protect(MyPassword, Contents:=True)
                xlWorkSheet.EnableAutoFilter = True
                xlApp.DisplayAlerts = False
                xlWorkBook.Save()
                xlWorkBook.Close()
                xlApp.Quit()
                If Not xlRange Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRange)
                End If
                If Not xlRangeCatCode Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangeCatCode)
                End If
                If Not xlRangeBCM Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangeBCM)
                End If
                If Not xlRangeAsset Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangeAsset)
                End If
                If Not xlRangeDelivery Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangeDelivery)
                End If
                If Not xlRangeBill Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangeBill)
                End If

                If Not xlRangePO Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangePO)
                End If
                If Not xlWorkSheetPO Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkSheetPO)
                End If
                If Not xlRangeCustom Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangeCustom)
                End If
                If Not xlRowRange Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRowRange)
                End If

                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkSheet)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp)
                xlRange = Nothing
                xlRangeCatCode = Nothing
                xlRangeBCM = Nothing
                xlRangeAsset = Nothing
                xlRangeDelivery = Nothing
                xlRangeBill = Nothing

                xlWorkSheet = Nothing
                xlWorkSheetPO = Nothing
                xlWorkBook = Nothing
                xlApp = Nothing

                ''To kill the current 'EXCEL' process, if not, it will always in Task Manager
                'intPID = System.Diagnostics.Process.GetProcessesByName("EXCEL")(0).Id
                'proc = System.Diagnostics.Process.GetProcessById(intPID)
                'proc.Kill()

            Catch exp As Exception
                Common.TrwExp(exp)
                intPID = System.Diagnostics.Process.GetProcessesByName("EXCEL")(0).Id
                proc = System.Diagnostics.Process.GetProcessById(intPID)
                proc.Kill()
            End Try
        End Function

        Public Sub WriteCell_ContractUpload(ByVal pFile As String, ByVal pPath As String, ByVal ds As DataSet, ByVal ds1 As DataSet)
            Dim ctx As Web.HttpContext = Web.HttpContext.Current
            Dim filename, strSourceFile, strDestFile As String

            If pFile = "ContractCatalogueTemplate" Then
                strSourceFile = ctx.Server.MapPath(ctx.Request.ApplicationPath) & "\Common\Template\ContractCatalogueTemplate.xls"
                filename = "ContractCatalogueTemplate_" & HttpContext.Current.Session("CompanyId") & "_" & HttpContext.Current.Session("UserId") & ".xls"
            Else
                strSourceFile = ctx.Server.MapPath(ctx.Request.ApplicationPath) & "\Common\Template\ContractCatalogueListingTemplate.xls"
                filename = "ContractCatalogueListing_" & HttpContext.Current.Session("CompanyId") & "_" & HttpContext.Current.Session("UserId") & ".xls"
            End If

            strDestFile = ConfigurationManager.AppSettings("TemplateTemp") & filename

            If File.Exists(strDestFile) Then
                File.Delete(strDestFile)
            End If
            File.Copy(strSourceFile, strDestFile)

            OpenConnToExcel(pPath, False)

            Dim SqlAry(0) As String
            Dim lsSql As String
            Dim i As Integer

            If ds.Tables(0).Rows.Count > 0 Or ds1.Tables(0).Rows.Count > 0 Then
                'GST Rate
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    lsSql = "Insert into [PredefineCode$E" & i + 1 & ":E" & i + 1 & "] Values ('" & Common.Parse(ds.Tables(0).Rows(i).Item("SST Rate")) & "')"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next

                'GST Tax Code
                For i = 0 To ds1.Tables(0).Rows.Count - 1
                    lsSql = "Insert into [PredefineCode$G" & i + 1 & ":G" & i + 1 & "] Values ('" & Common.Parse(ds1.Tables(0).Rows(i).Item("SST Tax Code")) & "')"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next

                ObjDb.BatchExecuteOle(SqlAry)
                ObjDb = Nothing
            End If

        End Sub

        Public Sub WriteCell_VIMUpload(ByVal pFile As String, ByVal pPath As String, ByVal ds As DataSet)
            Dim ctx As Web.HttpContext = Web.HttpContext.Current
            Dim filename, strSourceFile, strDestFile As String

            If pFile = "ItemVIMTemplate" Then
                strSourceFile = ctx.Server.MapPath(ctx.Request.ApplicationPath) & "\Common\Template\ItemVIMTemplate.xls"
                filename = "ItemVIMTemplate_" & HttpContext.Current.Session("CompanyId") & "_" & HttpContext.Current.Session("UserId") & ".xls"
            Else
                strSourceFile = ctx.Server.MapPath(ctx.Request.ApplicationPath) & "\Common\Template\ContractCatalogueListingTemplate.xls"
                filename = "ItemVIMListing_" & HttpContext.Current.Session("CompanyId") & "_" & HttpContext.Current.Session("UserId") & ".xls"
            End If

            strDestFile = ConfigurationManager.AppSettings("TemplateTemp") & filename

            If File.Exists(strDestFile) Then
                File.Delete(strDestFile)
            End If
            File.Copy(strSourceFile, strDestFile)

            OpenConnToExcel(pPath, False)

            Dim SqlAry(0) As String
            Dim lsSql As String
            Dim i As Integer

            If ds.Tables(0).Rows.Count > 0 Then
                'GST Rate
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    lsSql = "Insert into [PredefineCode$A" & i + 10 & ":A" & i + 10 & "] Values ('" & Common.Parse(ds.Tables(0).Rows(i).Item("GST Rate")) & "')"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next

                ObjDb.BatchExecuteOle(SqlAry)
                ObjDb = Nothing
            End If

        End Sub

        Public Function addLeadingZero(ByVal value As String, ByVal place As String) As String
            Dim leadingZero As String = ""
            Dim i As Integer
            Dim totalplace As Integer
            Dim valuelength As String
            valuelength = value.Length
            totalplace = CInt(place) - CInt(valuelength)
            For i = 0 To totalplace
                leadingZero &= "0"
            Next
            Return leadingZero & value.ToString
        End Function

        Public Function ProtectWorkSheet_ItemUpload(ByVal pFileName As String, Optional ByVal pNew As Boolean = False, Optional ByVal pEnterprise As Boolean = True)
            Dim xlApp As New Excel.Application
            Dim xlWorkBook As Excel.Workbook
            Dim xlWorkSheet, xlWorkSheetPO As Excel.Worksheet
            Dim MyPassword As String = ConfigurationManager.AppSettings("ExcelPwd")
            Dim proc As System.Diagnostics.Process
            Dim intPID As Integer
            Try
                xlWorkBook = xlApp.Workbooks.Open(pFileName)

                'Chee Hong: enhancement on 1st Nov 2013
                'Same format & style with ItemBIMTemplate.xls
                'BIM Sheet
                xlWorkSheet = xlWorkBook.Sheets("BIM")
                'Set Font Name to Arial Narrow
                'Set Font style to Bold
                'Set Background color 

                If pEnterprise = True Then
                    xlWorkSheet.Range("A1:AQ2000").Font.Name = "Arial Narrow"
                    xlWorkSheet.Range("A1:AQ1").Font.Bold = True
                    xlWorkSheet.Range("A1:AQ1").Interior.ColorIndex = 35
                Else
                    xlWorkSheet.Range("A1:AT2000").Font.Name = "Arial Narrow"
                    xlWorkSheet.Range("A1:AT1").Font.Bold = True
                    xlWorkSheet.Range("A1:AT1").Interior.ColorIndex = 35
                End If

                'Set * to red color
                xlWorkSheet.Range("B1").Characters(11, 1).Font.Color = RGB(255, 0, 0)
                xlWorkSheet.Range("C1").Characters(11, 1).Font.Color = RGB(255, 0, 0)
                xlWorkSheet.Range("D1").Characters(11, 1).Font.Color = RGB(255, 0, 0)
                xlWorkSheet.Range("F1").Characters(16, 1).Font.Color = RGB(255, 0, 0)
                xlWorkSheet.Range("G1").Characters(5, 1).Font.Color = RGB(255, 0, 0)
                If pEnterprise = True Then
                    xlWorkSheet.Range("J1").Characters(22, 1).Font.Color = RGB(255, 0, 0)
                Else
                    xlWorkSheet.Range("I1").Characters(22, 1).Font.Color = RGB(255, 0, 0)
                End If

                If pNew = True Then
                    xlWorkSheet.Protect(MyPassword, Contents:=True)
                End If

                xlWorkSheet.EnableAutoFilter = True
                xlApp.DisplayAlerts = False
                xlWorkBook.Save()
                xlWorkBook.Close()
                xlApp.Quit()
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkSheet)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp)
                xlWorkSheet = Nothing
                xlWorkBook = Nothing
                xlApp = Nothing



            Catch ex As Exception
                'To kill the current 'EXCEL' process, if not, it will always in Task Manager
                intPID = System.Diagnostics.Process.GetProcessesByName("EXCEL")(0).Id
                proc = System.Diagnostics.Process.GetProcessById(intPID)
                proc.Kill()
            End Try


        End Function

        Public Sub ProtectWorkSheet_ContractUpload(ByVal pFileName As String, ByVal pGSTRateCnt As Integer, ByVal pGSTTaxCodeCnt As Integer, Optional ByVal pNew As Boolean = False)
            Dim xlApp As New Excel.Application
            Dim xlWorkBook As Excel.Workbook
            Dim xlWorkSheet As Excel.Worksheet
            Dim xlRangeGSTRate, xlRangeGSTTaxCode As Excel.Range
            Dim MyPassword As String = ConfigurationManager.AppSettings("ExcelPwd")
            Dim proc As System.Diagnostics.Process
            Dim intPID As Integer

            Try

                xlWorkBook = xlApp.Workbooks.Open(pFileName)


                'Chee Hong: enhancement on 23th Oct 2013
                'Same format & style with ItemBIMTemplate.xls
                'BIM Sheet
                'xlWorkSheet = xlWorkBook.Sheets("BIM")
                ''Set Font Name to Arial Narrow
                'xlWorkSheet.Range("A1:AU2000").Font.Name = "Arial Narrow"
                ''Set Font style to Bold
                'xlWorkSheet.Range("A1:AU1").Font.Bold = True
                ''Set Background color 
                'xlWorkSheet.Range("A1:AU1").Interior.ColorIndex = 35
                ''Set * to red color
                'xlWorkSheet.Range("B1").Characters(11, 1).Font.Color = RGB(255, 0, 0)
                'xlWorkSheet.Range("C1").Characters(11, 1).Font.Color = RGB(255, 0, 0)
                'xlWorkSheet.Range("D1").Characters(11, 1).Font.Color = RGB(255, 0, 0)
                'xlWorkSheet.Range("F1").Characters(16, 1).Font.Color = RGB(255, 0, 0)
                'xlWorkSheet.Range("G1").Characters(5, 1).Font.Color = RGB(255, 0, 0)
                'xlWorkSheet.Range("J1").Characters(22, 1).Font.Color = RGB(255, 0, 0)
                'xlWorkSheet.Range("K1").Characters(21, 1).Font.Color = RGB(255, 0, 0)
                'xlWorkSheet.Range("V1").Characters(9, 1).Font.Color = RGB(255, 0, 0)
                'If pNew = True Then
                '    xlWorkSheet.Protect(MyPassword, Contents:=True)
                'End If

                'PredefineCode Sheet
                xlWorkSheet = xlWorkBook.Sheets("PredefineCode")
                xlWorkSheet.Range("E2:E100").Font.Bold = False
                xlWorkSheet.Range("G2:G200").Font.Bold = False

                xlRangeGSTRate = xlWorkSheet.Range("E2:E" & pGSTRateCnt + 1)
                xlRangeGSTRate.Name = "GST"
                xlRangeGSTTaxCode = xlWorkSheet.Range("G2:G" & pGSTTaxCodeCnt + 1)
                xlRangeGSTTaxCode.Name = "GSTTaxCode"
                xlWorkSheet.Protect(MyPassword, Contents:=True)
                xlWorkSheet.EnableAutoFilter = True
                xlApp.DisplayAlerts = False
                xlWorkBook.Save()
                xlWorkBook.Close()
                xlApp.Quit()
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangeGSTRate)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangeGSTTaxCode)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkSheet)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp)
                xlRangeGSTRate = Nothing
                xlRangeGSTTaxCode = Nothing
                xlWorkSheet = Nothing
                xlWorkBook = Nothing
                xlApp = Nothing


            Catch ex As Exception
                'To kill the current 'EXCEL' process, if not, it will always in Task Manager
                intPID = System.Diagnostics.Process.GetProcessesByName("EXCEL")(0).Id
                proc = System.Diagnostics.Process.GetProcessById(intPID)
                proc.Kill()
            End Try


        End Sub

        Public Sub ProtectWorkSheet_VIMUpload(ByVal pFileName As String, ByVal pGSTRateCnt As Integer, Optional ByVal pNew As Boolean = False)
            Dim xlApp As New Excel.Application
            Dim xlWorkBook As Excel.Workbook
            Dim xlWorkSheet As Excel.Worksheet
            Dim xlRangeGSTRate As Excel.Range
            Dim MyPassword As String = ConfigurationManager.AppSettings("ExcelPwd")
            Dim proc As System.Diagnostics.Process
            Dim intPID As Integer

            Try

                xlWorkBook = xlApp.Workbooks.Open(pFileName)


                'Chee Hong: enhancement on 23th Oct 2013
                'Same format & style with ItemBIMTemplate.xls
                'VIM Sheet
                xlWorkSheet = xlWorkBook.Sheets("VIM")
                ''Set Font Name to Arial Narrow
                'xlWorkSheet.Range("A1:AU2000").Font.Name = "Arial Narrow"
                ''Set Font style to Bold
                'xlWorkSheet.Range("A1:AU1").Font.Bold = True
                ''Set Background color 
                'xlWorkSheet.Range("A1:AU1").Interior.ColorIndex = 35
                'Set * to red color
                xlWorkSheet.Range("B1").Characters(11, 1).Font.Color = RGB(255, 0, 0)
                xlWorkSheet.Range("C1").Characters(11, 1).Font.Color = RGB(255, 0, 0)
                xlWorkSheet.Range("E1").Characters(16, 1).Font.Color = RGB(255, 0, 0)
                xlWorkSheet.Range("G1").Characters(7, 1).Font.Color = RGB(255, 0, 0)
                xlWorkSheet.Range("H1").Characters(5, 1).Font.Color = RGB(255, 0, 0)
                xlWorkSheet.Range("I1").Characters(10, 1).Font.Color = RGB(255, 0, 0)
                If pNew = True Then
                    xlWorkSheet.Protect(MyPassword, Contents:=True)
                End If

                'PredefineCode Sheet
                xlWorkSheet = xlWorkBook.Sheets("PredefineCode")
                xlWorkSheet.Range("A11:A" & pGSTRateCnt + 10).Font.Bold = False
                'xlWorkSheet.Range("G2:G200").Font.Bold = False

                xlRangeGSTRate = xlWorkSheet.Range("A11:A" & pGSTRateCnt + 10)
                xlRangeGSTRate.Name = "GTS"
                xlWorkSheet.Protect(MyPassword, Contents:=True)
                xlWorkSheet.EnableAutoFilter = True
                xlApp.DisplayAlerts = False
                xlWorkBook.Save()
                xlWorkBook.Close()
                xlApp.Quit()
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangeGSTRate)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkSheet)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp)
                xlRangeGSTRate = Nothing
                xlWorkSheet = Nothing
                xlWorkBook = Nothing
                xlApp = Nothing



            Catch ex As Exception
                'To kill the current 'EXCEL' process, if not, it will always in Task Manager
                intPID = System.Diagnostics.Process.GetProcessesByName("EXCEL")(0).Id
                proc = System.Diagnostics.Process.GetProcessById(intPID)
                proc.Kill()
            End Try


        End Sub

        'Jules 2018.07.19 - PAMB
        'Zulham 19102018 - PAMB
        Public Sub WriteCell_IPPUpload(ByVal ds As DataSet, ByVal pPath As String, Optional ByVal isMasterDoc As String = "No", Optional ByVal dsGLCode As DataSet = Nothing, Optional ByVal dsInputGSTTax As DataSet = Nothing, Optional ByVal dsOutputGSTTax As DataSet = Nothing,
                                       Optional ByVal dsFundType As DataSet = Nothing, Optional ByVal dsProductType As DataSet = Nothing, Optional ByVal dsChannel As DataSet = Nothing, Optional ByVal dsReinsuranceCo As DataSet = Nothing, Optional ByVal dsAssetCode As DataSet = Nothing,
                                       Optional ByVal dsProjectCode As DataSet = Nothing, Optional ByVal dsPersonCode As DataSet = Nothing, Optional ByVal dsPayFor As DataSet = Nothing, Optional ByVal dsCC As DataSet = Nothing, Optional ByVal docNo As String = "", Optional ByVal vendorName As String = "",
                                       Optional ByVal docDate As String = "", Optional ByVal from As String = "", Optional ByVal dsCurrency As DataSet = Nothing)
            Dim xlApp As New Excel.Application
            Dim xlWorkBook As Excel.Workbook
            Dim xlWorkSheet, xlWorkSheetPO As Excel.Worksheet
            Dim xlRange, xlRangeCatCode, xlRangeBCM, xlRangeAsset, xlRangeDelivery, xlRangeBill, xlRangePO, xlRowRange As Excel.Range
            Dim ctx As Web.HttpContext = Web.HttpContext.Current
            Dim SqlAry(0) As String
            Dim lsSql As String
            Dim i, record, counter, startIndex As Integer
            Dim intLoop, intLoop1, intTotRow, intTotCol, TotalRow As Integer
            Dim filename, remark, rangeName As String
            Dim _bool As Boolean = False
            Dim proc As System.Diagnostics.Process
            Dim intPID As Integer

            'Jules 2018.07.19
            Dim xlRangeFundType, xlRangeProductType, xlRangeChannel, xlRangeReinsuranceCo, xlRangeAssetCode, xlRangeProjectCode, xlRangePersonCode As Excel.Range

            'Zulham 29012019 
            Dim strSourceFile As String = ""
            If Not from = "multiInvoices" Then
                strSourceFile = ctx.Server.MapPath(ctx.Request.ApplicationPath) & "\Common\Template\MultipleGLDebitsPAMB.xls" 'Jules 2018.07.19
                filename = "MultipleGLDebits.xls"
            Else
                strSourceFile = ctx.Server.MapPath(ctx.Request.ApplicationPath) & "\Common\Template\MultipleInvoices.xls"
                filename = "MultipleInvoices.xls"
            End If

            Dim strDestFile As String = ConfigurationManager.AppSettings("TemplateTemp") & filename

            If File.Exists(strDestFile) Then
                File.Delete(strDestFile)
            End If
            File.Copy(strSourceFile, strDestFile)
            OpenConnToExcel(pPath, False)

            'Jules 2018.07.20 - PAMB - Commented out because we removed Sub Description.
            'TotalRow = ds.Tables(0).Rows.Count
            'intTotRow = ds.Tables(0).Rows.Count + 1
            'intTotCol = ds.Tables(0).Columns.Count            

            ''Gl Code
            'counter = 1
            'For i = 0 To dsGLCode.Tables(0).Rows.Count - 1

            '    lsSql = "Insert into [SubDescription(Ref)$A" & counter + 1 & ":A" & counter + 1 & "] Values ('" & Common.Parse(dsGLCode.Tables(0).Rows(i).Item("igg_gl_code")) & "')"

            '    Common.Insert2Ary(SqlAry, lsSql)
            '    ''Rules Category
            '    _bool = False
            '    For record = 0 To ds.Tables(0).Rows.Count - 1
            '        If Common.Parse(dsGLCode.Tables(0).Rows(i).Item("igg_gl_code")) = Common.Parse(ds.Tables(0).Rows(record).Item("igg_gl_code")) Then
            '            lsSql = "Insert into [SubDescription(Ref)$B" & counter + 1 & ":B" & counter + 1 & "] Values ('" & Common.Parse(ds.Tables(0).Rows(record).Item("igc_glrule_category")) & "')"
            '            Common.Insert2Ary(SqlAry, lsSql)
            '            If ds.Tables(0).Rows(record).Item("igc_glrule_category_remark").ToString.Length > 255 Then
            '                remark = ds.Tables(0).Rows(record).Item("igc_glrule_category_remark").ToString.Substring(0, 255)
            '            Else
            '                remark = ds.Tables(0).Rows(record).Item("igc_glrule_category_remark")
            '            End If
            '            lsSql = "Insert into [SubDescription(Ref)$C" & counter + 1 & ":C" & counter + 1 & "] Values ('" & Common.Parse(remark) & "')"
            '            Common.Insert2Ary(SqlAry, lsSql)
            '            counter += 1
            '            _bool = True
            '        End If
            '    Next
            '    If _bool = False Then
            '        counter += 1
            '    End If
            'Next
            'End modification.

            'Zulham 30012019 - PAMB
            Try
                If Not from = "multiInvoices" Then
                    lsSql = "Insert into [GL Debits$B1:B1] Values ('" & vendorName & "')"
                    Common.Insert2Ary(SqlAry, lsSql)
                    lsSql = "Insert into [GL Debits$B2:B2] Values ('" & docNo & "')"
                    Common.Insert2Ary(SqlAry, lsSql)
                    lsSql = "Insert into [GL Debits$B3:B3] Values ('" & docDate & "')"
                    Common.Insert2Ary(SqlAry, lsSql)
                Else
                    For i = 0 To dsCurrency.Tables(0).Rows.Count - 1
                        lsSql = "Insert into [PredefineCode$AC" & i + 1 & ":AC" & i + 1 & "] Values ('" & Common.Parse(dsCurrency.Tables(0).Rows(i).Item("CODE_ABBR")) & "')"
                        Common.Insert2Ary(SqlAry, lsSql)
                    Next
                    ''Zulham 19042019
                    'For i = 1 To 99
                    '    lsSql = "Insert into [Header$E" & i & ":E" & i & "] Values ('" & Common.Parse(vendorName) & "')"
                    '    Common.Insert2Ary(SqlAry, lsSql)
                    'Next

                    For i = 2 To 2001
                        lsSql = "Insert into [GL Debits$C" & i & ":C" & i & "] Values ('" & Common.Parse(vendorName) & "')"
                        Common.Insert2Ary(SqlAry, lsSql)
                    Next

                End If
            Catch ex As Exception

            End Try

            'Jules 2018.08.09 - Added Pay For data.
            Try
                lsSql = "Insert into [PredefineCode$A1:A1] Values ('Own Co.')"
                Common.Insert2Ary(SqlAry, lsSql)
                For i = 0 To dsPayFor.Tables(0).Rows.Count - 1
                    lsSql = "Insert into [PredefineCode$A" & i + 2 & ":A" & i + 2 & "] Values ('" & Common.Parse(dsPayFor.Tables(0).Rows(i).Item("IC_OTHER_B_COY_CODE")) & "')"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next
            Catch ex As Exception

            End Try
            'End modification.

            'Write InputTax/OutputTax list into Predefine
            ''GST Input Tax
            Try
                lsSql = "Insert into [PredefineCode$S1:S1] Values ('N/A')"
                Common.Insert2Ary(SqlAry, lsSql)
                For i = 0 To dsInputGSTTax.Tables(0).Rows.Count - 1
                    lsSql = "Insert into [PredefineCode$S" & i + 2 & ":S" & i + 2 & "] Values ('" & Common.Parse(dsInputGSTTax.Tables(0).Rows(i).Item("GST")) & "')"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next
            Catch ex As Exception

            End Try

            ''GST Output Tax
            Try
                lsSql = "Insert into [PredefineCode$U1:U1] Values ('N/A')"
                Common.Insert2Ary(SqlAry, lsSql)
                For i = 0 To dsOutputGSTTax.Tables(0).Rows.Count - 1
                    lsSql = "Insert into [PredefineCode$U" & i + 2 & ":U" & i + 2 & "] Values ('" & Common.Parse(dsOutputGSTTax.Tables(0).Rows(i).Item("GST")) & "')"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next
            Catch ex As Exception

            End Try


            'Zulham 18102018 - PAMB
            'CostCentre
            If Not dsGLCode Is Nothing AndAlso dsGLCode.Tables(0).Rows.Count > 0 Then
                For i = 0 To dsCC.Tables(0).Rows.Count - 1
                    lsSql = "Insert into [PredefineCode$I" & i + 1 & ":I" & i + 1 & "] Values ('" & Common.Parse(dsCC.Tables(0).Rows(i).Item("CC_CC_CODE")) & "')"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next
            End If

            'Jules 2018.07.19 - PAMB
            'Zulham 18102018 - PAMB
            If Not dsGLCode Is Nothing AndAlso dsGLCode.Tables(0).Rows.Count > 0 Then
                For i = 0 To dsGLCode.Tables(0).Rows.Count - 1
                    lsSql = "Insert into [PredefineCode$E" & i + 1 & ":E" & i + 1 & "] Values ('" & Common.Parse(dsGLCode.Tables(0).Rows(i).Item("CBG_B_GL_DESC")) & ":" & Common.Parse(dsGLCode.Tables(0).Rows(i).Item("CBG_B_GL_CODE")) & "')"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next
            End If

            If Not dsFundType Is Nothing AndAlso dsFundType.Tables(0).Rows.Count > 0 Then
                For i = 0 To dsFundType.Tables(0).Rows.Count - 1
                    lsSql = "Insert into [AnalysisCodes$A" & i + 1 & ":A" & i + 1 & "] Values ('" & Common.Parse(dsFundType.Tables(0).Rows(i).Item("AC_ANALYSIS_CODE_DESC")) & "')"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next
            End If

            If Not dsProductType Is Nothing AndAlso dsProductType.Tables(0).Rows.Count > 0 Then
                For i = 0 To dsProductType.Tables(0).Rows.Count - 1
                    lsSql = "Insert into [AnalysisCodes$B" & i + 1 & ":B" & i + 1 & "] Values ('" & Common.Parse(dsProductType.Tables(0).Rows(i).Item("AC_ANALYSIS_CODE_DESC")) & "')"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next
            End If

            If Not dsChannel Is Nothing AndAlso dsChannel.Tables(0).Rows.Count > 0 Then
                For i = 0 To dsChannel.Tables(0).Rows.Count - 1
                    lsSql = "Insert into [AnalysisCodes$C" & i + 1 & ":C" & i + 1 & "] Values ('" & Common.Parse(dsChannel.Tables(0).Rows(i).Item("AC_ANALYSIS_CODE_DESC")) & "')"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next
            End If

            If Not dsReinsuranceCo Is Nothing AndAlso dsReinsuranceCo.Tables(0).Rows.Count > 0 Then
                For i = 0 To dsReinsuranceCo.Tables(0).Rows.Count - 1
                    lsSql = "Insert into [AnalysisCodes$D" & i + 1 & ":D" & i + 1 & "] Values ('" & Common.Parse(dsReinsuranceCo.Tables(0).Rows(i).Item("AC_ANALYSIS_CODE_DESC")) & "')"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next
            End If

            If Not dsAssetCode Is Nothing AndAlso dsAssetCode.Tables(0).Rows.Count > 0 Then
                For i = 0 To dsAssetCode.Tables(0).Rows.Count - 1
                    lsSql = "Insert into [AnalysisCodes$E" & i + 1 & ":E" & i + 1 & "] Values ('" & Common.Parse(dsAssetCode.Tables(0).Rows(i).Item("AC_ANALYSIS_CODE_DESC")) & "')"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next
            End If

            If Not dsProjectCode Is Nothing AndAlso dsProjectCode.Tables(0).Rows.Count > 0 Then
                For i = 0 To dsProjectCode.Tables(0).Rows.Count - 1
                    lsSql = "Insert into [AnalysisCodes$F" & i + 1 & ":F" & i + 1 & "] Values ('" & Common.Parse(dsProjectCode.Tables(0).Rows(i).Item("AC_ANALYSIS_CODE_DESC")) & "')"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next
            End If

            If Not dsPersonCode Is Nothing AndAlso dsPersonCode.Tables(0).Rows.Count > 0 Then
                For i = 0 To dsPersonCode.Tables(0).Rows.Count - 1
                    lsSql = "Insert into [AnalysisCodes$G" & i + 1 & ":G" & i + 1 & "] Values ('" & Common.Parse(dsPersonCode.Tables(0).Rows(i).Item("AC_ANALYSIS_CODE_DESC")) & "')"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next
            End If
            'End modification.


            ObjDb.BatchExecuteOle(SqlAry)

            'Jules 2018.07.20 - PAMB - Removed Sub Description.
            'xlWorkBook = xlApp.Workbooks.Open(strDestFile)
            'xlWorkSheet = xlWorkBook.Sheets("SubDescription(Ref)")
            'xlWorkSheet.Range("A1:AU2000").Font.Name = "Arial Narrow"
            'xlWorkSheet.Range("A3:AU6").Font.Bold = False
            'xlWorkSheet.Range("A3:AU6").Interior.ColorIndex = 0

            'counter = 2
            'For i = 0 To dsGLCode.Tables(0).Rows.Count - 1
            '    ''Rules Category
            '    If counter = 1 Then
            '        startIndex = counter + 2
            '    Else
            '        startIndex = counter + 1
            '    End If

            '    _bool = False
            '    For record = 0 To ds.Tables(0).Rows.Count - 1
            '        If Common.Parse(dsGLCode.Tables(0).Rows(i).Item("igg_gl_code")) = Common.Parse(ds.Tables(0).Rows(record).Item("igg_gl_code")) Then
            '            counter += 1
            '            _bool = True
            '        End If
            '    Next
            '    If _bool = False Then
            '        counter += 1
            '    End If

            '    xlRange = xlWorkSheet.Range("B" & startIndex & ":B" & counter)
            '    rangeName = "GL_" & Common.Parse(dsGLCode.Tables(0).Rows(i).Item("igg_gl_code")).Trim
            '    xlRange.Name = rangeName

            'Next
            'End modification.

            'Set the listname for input/output tax
            xlWorkBook = xlApp.Workbooks.Open(strDestFile)

            ''Zulham 19042019 - 
            'If from = "multiInvoices" Then
            '    xlWorkSheet = xlWorkBook.Sheets("Header")

            '    xlRange = xlWorkSheet.Range("E2:E100")
            '    xlRange.Interior.ColorIndex = 0
            '    xlRange.Font.Bold = False
            '    xlRange.BorderAround(Excel.XlLineStyle.xlLineStyleNone, Excel.XlBorderWeight.xlHairline, Excel.XlColorIndex.xlColorIndexNone)
            'End If

            xlWorkSheet = xlWorkBook.Sheets("PredefineCode")

            'Jules 2018.07.19
            If Not dsGLCode Is Nothing AndAlso dsGLCode.Tables(0).Rows.Count > 0 Then
                xlRange = xlWorkSheet.Range("E2:E" & dsGLCode.Tables(0).Rows.Count + 1)
                xlRange.Name = "GLCode"
                xlWorkSheet.Range("E2:U2000").Font.Bold = False
            End If

            xlWorkSheet.Range("A2:U2000").Font.Name = "Arial Narrow"
            xlWorkSheet.Range("A1:U1").Font.Bold = True
            xlWorkSheet.Range("A2:U2000").Font.Bold = False
            xlWorkSheet.Range("A2:U2000").Interior.ColorIndex = 0

            If dsPayFor.Tables(0).Rows.Count > 0 Then
                xlRange = xlWorkSheet.Range("A2:A" & dsPayFor.Tables(0).Rows.Count + 2)
                xlRange.Name = "PayFor"
            End If

            xlRange = xlWorkSheet.Range("S2:S" & dsInputGSTTax.Tables(0).Rows.Count + 2)
            xlRange.Name = "InputTaxCode"

            xlRange = xlWorkSheet.Range("U2:U" & dsOutputGSTTax.Tables(0).Rows.Count + 2)
            xlRange.Name = "OutputTaxCode"

            'Zulham 18102018
            xlRange = xlWorkSheet.Range("I2:I" & dsCC.Tables(0).Rows.Count + 2)
            xlRange.Name = "CostCenter"

            '.end

            'Zulham 15022019
            If from = "multiInvoices" Then
                xlRange = xlWorkSheet.Range("AC2:AC" & dsCurrency.Tables(0).Rows.Count + 1)
                xlRange.Name = "Currency2"
            End If
            'End

            xlWorkSheet = xlWorkBook.Sheets("AnalysisCodes")
            xlWorkSheet.Range("A1:AU2000").Font.Name = "Arial Narrow"
            xlWorkSheet.Range("A2:AU2000").Font.Bold = False
            xlWorkSheet.Range("A2:AU2000").Interior.ColorIndex = 0

            If Not dsFundType Is Nothing AndAlso dsFundType.Tables(0).Rows.Count > 0 Then
                xlRangeFundType = xlWorkSheet.Range("A2:A" & dsFundType.Tables(0).Rows.Count + 1)
                xlRangeFundType.Name = "FundType"
            End If

            If Not dsProductType Is Nothing AndAlso dsProductType.Tables(0).Rows.Count > 0 Then
                xlRangeProductType = xlWorkSheet.Range("B2:B" & dsProductType.Tables(0).Rows.Count + 1)
                xlRangeProductType.Name = "ProductType"
            End If

            If Not dsChannel Is Nothing AndAlso dsChannel.Tables(0).Rows.Count > 0 Then
                xlRangeChannel = xlWorkSheet.Range("C2:C" & dsChannel.Tables(0).Rows.Count + 1)
                xlRangeChannel.Name = "Channel"
            End If

            If Not dsReinsuranceCo Is Nothing AndAlso dsReinsuranceCo.Tables(0).Rows.Count > 0 Then
                xlRangeReinsuranceCo = xlWorkSheet.Range("D2:D" & dsReinsuranceCo.Tables(0).Rows.Count + 1)
                xlRangeReinsuranceCo.Name = "ReinsuranceCo"
            End If

            If Not dsAssetCode Is Nothing AndAlso dsAssetCode.Tables(0).Rows.Count > 0 Then
                xlRangeAssetCode = xlWorkSheet.Range("E2:E" & dsAssetCode.Tables(0).Rows.Count + 1)
                xlRangeAssetCode.Name = "AssetCode"
            End If

            If Not dsProjectCode Is Nothing AndAlso dsProjectCode.Tables(0).Rows.Count > 0 Then
                xlRangeProjectCode = xlWorkSheet.Range("F2:F" & dsProjectCode.Tables(0).Rows.Count + 1)
                xlRangeProjectCode.Name = "ProjectCode"
            End If

            If Not dsPersonCode Is Nothing AndAlso dsPersonCode.Tables(0).Rows.Count > 0 Then
                xlRangePersonCode = xlWorkSheet.Range("G2:G" & dsPersonCode.Tables(0).Rows.Count + 1)
                xlRangePersonCode.Name = "PersonCode"
            End If
            'End modification.

            xlWorkSheet.EnableAutoFilter = True
            xlApp.DisplayAlerts = False
            xlWorkBook.Save()
            'Zulham 11112018
            xlWorkBook.Close(0)
            xlApp.Quit()
            If Not xlRange Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRange)
            End If
            If Not xlRangeCatCode Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangeCatCode)
            End If
            If Not xlRangeBCM Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangeBCM)
            End If
            If Not xlRangeAsset Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangeAsset)
            End If
            If Not xlRangeDelivery Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangeDelivery)
            End If
            If Not xlRangeBill Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangeBill)
            End If

            If Not xlRangePO Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangePO)
            End If
            If Not xlWorkSheetPO Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkSheetPO)
            End If
            If Not xlRowRange Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRowRange)
            End If

            'Jules 2018.07.19
            If Not xlRangeFundType Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangeFundType)
            End If
            If Not xlRangeProductType Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangeProductType)
            End If
            If Not xlRangeChannel Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangeChannel)
            End If
            If Not xlRangeReinsuranceCo Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangeReinsuranceCo)
            End If
            If Not xlRangeAssetCode Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangeAssetCode)
            End If
            If Not xlRangeProjectCode Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangeProjectCode)
            End If
            If Not xlRangePersonCode Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangePersonCode)
            End If
            'End modification.

            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkSheet)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp)
            xlRange = Nothing
            xlRangeCatCode = Nothing
            xlRangeBCM = Nothing
            xlRangeAsset = Nothing
            xlRangeDelivery = Nothing
            xlRangeBill = Nothing

            'Jules 2018.07.19
            xlRangeFundType = Nothing
            xlRangeProductType = Nothing
            xlRangeChannel = Nothing
            xlRangeReinsuranceCo = Nothing
            xlRangeAssetCode = Nothing
            xlRangeProductType = Nothing
            xlRangePersonCode = Nothing
            'End modification.

            xlWorkSheet = Nothing
            xlWorkSheetPO = Nothing
            xlWorkBook = Nothing
            xlApp = Nothing

            'Zulham 11112018
            ''To kill the current 'EXCEL' process, if not, it will always be in Task Manager
            'intPID = System.Diagnostics.Process.GetProcessesByName("EXCEL")(0).Id
            'proc = System.Diagnostics.Process.GetProcessById(intPID)
            'proc.Kill()

        End Sub

        'Jules 2018.07.14 - PAMB       
        Public Function ProtectWorkSheet_POUpload_PAMB(ByVal pFileName As String, ByVal pGLCodeCnt As Integer, ByVal pCatCodeCnt As Integer,
        ByVal pAssetGroupCnt As Integer, ByVal pDeliveryAddrCnt As Integer, ByVal pBillToCnt As Integer, ByVal pInputTaxCnt As Integer, ByVal pFundTypeCnt As Integer, pPersonCodeCnt As Integer, pProjectCodeCnt As Integer,
        Optional ByVal dsCustName As DataSet = Nothing, Optional ByVal pBudgetAccCnt As Integer = Nothing, Optional ByVal pGSTTaxCnt As Integer = 0, Optional ByVal isGst As String = "", Optional ByVal exceedCutOffDt As String = "")
            Dim xlApp As New Excel.Application
            Dim xlWorkBook As Excel.Workbook
            Dim xlWorkSheet, xlWorkSheetPO As Excel.Worksheet
            Dim xlRange, xlRangeCatCode, xlRangeBCM, xlRangeAsset, xlRangeDelivery, xlRangeBill, xlRangePO, xlRowRange, xlRangeFundType, xlRangePersonCode, xlRangeProjectCode, xlRangeInputTax, xlRangeGSTRate As Excel.Range
            Dim MyPassword As String = ConfigurationManager.AppSettings("ExcelPwd")
            Dim proc As System.Diagnostics.Process
            Dim intPID As Integer
            Dim intCustFields As Integer = 0
            Dim dsCustomField As DataSet

            Try
                xlWorkBook = xlApp.Workbooks.Open(pFileName)
                xlWorkSheet = xlWorkBook.Sheets("PredefineCode")
                xlWorkSheet.Range("A1:AU2000").Font.Name = "Arial Narrow"
                xlWorkSheet.Range("A1:AU1").Font.Bold = True
                xlWorkSheet.Range("A1:AU1").Interior.ColorIndex = 35

                If pGLCodeCnt > 0 Then
                    xlRange = xlWorkSheet.Range("C2:C" & pGLCodeCnt + 1)
                    xlRange.Name = "GLCode"
                End If

                'Jules 2018.08.02 - Only 1 value.
                'If pCatCodeCnt > 0 Then                
                'xlRangeCatCode = xlWorkSheet.Range("D2:D" & pCatCodeCnt + 1)
                xlRangeCatCode = xlWorkSheet.Range("D2:D2")
                xlRangeCatCode.Name = "CatCode"
                'End If
                'End modification.

                If pBudgetAccCnt > 0 Then
                    xlRangeBCM = xlWorkSheet.Range("G2:G" & pBudgetAccCnt + 1)
                    xlRangeBCM.Name = "BudgetAct"
                End If
                If pGSTTaxCnt > 0 And isGst = "Yes" Then
                    xlRangeGSTRate = xlWorkSheet.Range("J2:J" & pGSTTaxCnt + 1)
                    xlRangeGSTRate.Name = "GSTRate"
                Else 'Jules 2018.08.02
                    If exceedCutOffDt = "Yes" Then
                        xlRangeGSTRate = xlWorkSheet.Range("J2:J2")
                        xlRangeGSTRate.Name = "GSTRate"
                    End If
                End If

                'Jules 2018.07.14
                If pFundTypeCnt > 0 Then
                    xlRangeFundType = xlWorkSheet.Range("L2:L" & pFundTypeCnt + 1)
                    xlRangeFundType.Name = "FundType"
                End If
                If pPersonCodeCnt > 0 Then
                    'Jules 2018.10.18
                    'xlRangePersonCode = xlWorkSheet.Range("M2:M" & pPersonCodeCnt + 2) 'Add 2 to cater for N/A
                    xlRangePersonCode = xlWorkSheet.Range("M2:M" & pPersonCodeCnt + 1)
                    xlRangePersonCode.Name = "PersonCode"
                End If
                If pProjectCodeCnt > 0 Then
                    xlRangeProjectCode = xlWorkSheet.Range("N2:N" & pProjectCodeCnt + 1)
                    xlRangeProjectCode.Name = "ProjectCode"
                End If
                If pInputTaxCnt > 0 Then
                    xlRangeInputTax = xlWorkSheet.Range("O2:O" & pInputTaxCnt + 1)
                    xlRangeInputTax.Name = "InputTax"
                End If
                'End modification.

                xlWorkSheetPO = xlWorkBook.Sheets("PO")

                If isGst = "No" Then
                    If exceedCutOffDt = "Yes" Then
                        'xlWorkSheetPO.Range("M10:M59").Validation.InCellDropdown = False 'Jules 2018.08.02 - Allow user to select.
                    Else
                        'normal
                        xlWorkSheetPO.Range("M9").Value = "Tax(%)"
                        xlWorkSheetPO.Range("M10:M59").Validation.InCellDropdown = False
                    End If
                End If
                ' Custom fields
                Dim i, j As Integer
                Dim xlRangeCustom As Excel.Range
                Dim strSql, lsSql As String
                Dim SqlAry(0) As String
                Dim NameBox As String
                Dim style As Excel.Style = xlWorkSheet.Application.ActiveWorkbook.Styles.Add("NewStyle")
                style.Font.Bold = True
                style.Font.Name = "Arial Narrow"
                style.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.PaleTurquoise)
                style.Borders.LineStyle = Excel.XlLineStyle.xlContinuous
                style.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.XlLineStyle.xlLineStyleNone
                style.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.XlLineStyle.xlLineStyleNone
                style.WrapText = True
                style.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter

                If Not dsCustName Is Nothing Then
                    intCustFields = dsCustName.Tables(0).Rows.Count
                    For i = 0 To dsCustName.Tables(0).Rows.Count - 1
                        Dim lbl As New System.Web.UI.WebControls.Label
                        lbl.ForeColor = Color.Red
                        lbl.Text = " * "

                        'Jules 2018.08.02 - Added Purchase Tax Code
                        xlWorkSheetPO.Cells(9, 20 + i) = dsCustName.Tables(0).Rows(i).Item("CF_FIELD_NAME") & lbl.Text  'Zulham Okt 31, 2013
                        xlRowRange = xlWorkSheetPO.Range(Chr(80 + i) & "10:" & Chr(80 + i) & "10")

                        xlWorkSheetPO.Cells(9, 20 + i).Style = "NewStyle"
                        xlWorkSheet.Columns(16 + i).Insert()
                        xlWorkSheet.Cells(1, 16 + i) = dsCustName.Tables(0).Rows(i).Item("CF_FIELD_NAME")
                        'End modification.

                        strSql = "SELECT CF_FIELD_INDEX, CF_FIELD_VALUE, REPLACE(CF_FIELD_VALUE,' ','_') AS FIELDVALUE FROM CUSTOM_FIELDS WHERE CF_FIELD_NO ='" & dsCustName.Tables(0).Rows(i).Item("CF_FIELD_NO") & "' " &
                       "AND CF_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                        strSql &= " AND CF_MODULE='PO'"
                        strSql = strSql & " ORDER BY CF_FIELD_VALUE"
                        dsCustomField = ObjDb.FillDs(strSql)

                        If Not dsCustomField Is Nothing Then
                            'Jules 2018.08.02 - Added Purchase Tax Code.
                            For j = 0 To dsCustomField.Tables(0).Rows.Count - 1
                                xlWorkSheet.Cells(2 + j, 16 + i) = Common.Parse(dsCustomField.Tables(0).Rows(j).Item("CF_FIELD_VALUE"))
                                Dim r As Excel.Range = CType(xlWorkSheet.Cells(2 + j, 16 + i), Excel.Range)
                            Next
                            xlRangeCustom = xlWorkSheet.Range(Chr(80 + i) & "2:" & Chr(80 + i) & dsCustomField.Tables(0).Rows.Count + 1)
                            NameBox = Regex.Replace(dsCustName.Tables(0).Rows(i).Item("CF_FIELD_NAME"), "[^a-zA-Z0-9]", "_")
                            xlRangeCustom.Name = NameBox
                            'Zulham Oct 31, 2013
                            For _rowcount As Integer = 0 To 999 'Jules 2018.09.05 - Increased from 49.
                                xlRowRange = xlWorkSheetPO.Range(Chr(84 + i) & 10 + _rowcount & ":" & Chr(84 + i) & 10 + _rowcount)
                                With xlRowRange.Validation
                                    .Add(Excel.XlDVType.xlValidateList, Excel.XlDVAlertStyle.xlValidAlertStop, Excel.XlFormatConditionOperator.xlBetween, "=" & NameBox)
                                    .InCellDropdown = True
                                End With
                            Next
                            'End
                        End If
                    Next
                End If

                For _rowcount As Integer = 0 To 999 'Jules 2018.09.05 - Increased from 49.
                    xlWorkSheetPO.Cells(10 + _rowcount, 17) = Format(Now.AddDays(1), "dd/MM/yyyy") ''Jules 2018.08.02
                Next

                xlRangeAsset = xlWorkSheet.Range("E2:E" & pAssetGroupCnt + 1)
                xlRangeAsset.Name = "AssetGroup"
                If pAssetGroupCnt = 0 Then
                    xlWorkSheetPO = xlWorkBook.Sheets("PO")
                    xlRangePO = xlWorkSheetPO.Columns("I")
                    xlRangePO.Delete()
                End If
                xlRangeDelivery = xlWorkSheet.Range("H2:H" & pDeliveryAddrCnt + 1)
                xlRangeDelivery.Name = "DeliveryAddr"
                xlRangeBill = xlWorkSheet.Range("B2:B" & pBillToCnt + 1)
                xlRangeBill.Name = "Billing"

                xlWorkSheet.Protect(MyPassword, Contents:=True)
                xlWorkSheet.EnableAutoFilter = True
                xlApp.DisplayAlerts = False
                xlWorkBook.Save()
                xlWorkBook.Close()
                xlApp.Quit()
                If Not xlRange Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRange)
                End If
                If Not xlRangeCatCode Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangeCatCode)
                End If
                If Not xlRangeBCM Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangeBCM)
                End If
                If Not xlRangeAsset Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangeAsset)
                End If
                If Not xlRangeDelivery Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangeDelivery)
                End If
                If Not xlRangeBill Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangeBill)
                End If

                If Not xlRangePO Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangePO)
                End If
                If Not xlWorkSheetPO Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkSheetPO)
                End If
                If Not xlRangeCustom Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangeCustom)
                End If
                If Not xlRowRange Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRowRange)
                End If

                'Jules 2018.07.19
                If Not xlRangeFundType Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangeFundType)
                End If
                If Not xlRangeProjectCode Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangeProjectCode)
                End If
                If Not xlRangePersonCode Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangePersonCode)
                End If
                If Not xlRangeInputTax Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangeInputTax)
                End If
                If Not xlRangeGSTRate Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangeGSTRate)
                End If
                'End modification.

                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkSheet)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp)
                xlRange = Nothing
                xlRangeCatCode = Nothing
                xlRangeBCM = Nothing
                xlRangeAsset = Nothing
                xlRangeDelivery = Nothing
                xlRangeBill = Nothing

                'Jules 2018.07.19
                xlRangeFundType = Nothing
                xlRangePersonCode = Nothing
                xlRangeProjectCode = Nothing
                xlRangeInputTax = Nothing
                xlRangeGSTRate = Nothing
                'End modification.

                xlWorkSheet = Nothing
                xlWorkSheetPO = Nothing
                xlWorkBook = Nothing
                xlApp = Nothing

                ''To kill the current 'EXCEL' process, if not, it will always in Task Manager
                'intPID = System.Diagnostics.Process.GetProcessesByName("EXCEL")(0).Id
                'proc = System.Diagnostics.Process.GetProcessById(intPID)
                'proc.Kill()

            Catch exp As Exception
                Common.TrwExp(exp)
                intPID = System.Diagnostics.Process.GetProcessesByName("EXCEL")(0).Id
                proc = System.Diagnostics.Process.GetProcessById(intPID)
                proc.Kill()
            End Try
        End Function

#End Region
#Region "Zulham"
        Public Function ReadPOUploadFormat(ByVal pXML As String, ByVal pExcel As String, ByRef pRules As myCollection, ByRef dtHeader As DataTable) As DataSet
            Dim objXML As New AppXml
            Dim aa As UploadProduct
            Dim ds As DataSet
            Dim ds1 As New DataSet
            Dim strSheet, strRangeFr, strRangeTo, strSql As String
            Dim dr As DataRow
            Dim intTotalRow, intLoop As Integer
            Dim dt1 As New DataTable

            Try
                dt1.Columns.Add("CoyName", Type.GetType("System.String"))
                dt1.Columns.Add("BillTo", Type.GetType("System.String"))

                ds = objXML.GetDsFromXML(pXML)

                If ds.Tables("Column").Rows.Count > 0 Then
                    intTotalRow = ds.Tables("Column").Rows.Count - 1
                    For intLoop = 0 To intTotalRow
                        Dim pMap As New UploadRule

                        With ds.Tables("Column").Rows(intLoop)
                            pMap.ColNo = .Item("ColNo")                  'Must not null
                            pMap.ColName = .Item("ColName")              'Must not null
                            pMap.DBField = .Item("DBField")              'Must not null
                            pMap.AllowNull = IIf(IsDBNull(.Item("AllowNull")), "False", .Item("AllowNull"))
                            pMap.Regex = IIf(IsDBNull(.Item("Regex")), "", .Item("Regex"))               'Must not null
                            pMap.RegexErrMsg = IIf(IsDBNull(.Item("RegexErrMsg")), "Error found.", .Item("RegexErrMsg"))
                        End With

                        Dim dtrow, dtRow2 As DataRow
                        For Each dtrow In ds.Tables("Column").Rows(intLoop).GetChildRows("Column_SQL")
                            For Each dtRow2 In dtrow.GetChildRows("SQL_SQLItem")
                                pMap.SQL(UBound(pMap.SQL)).Query = IIf(IsDBNull(dtRow2.ItemArray(0)), "", dtRow2.ItemArray(0))
                                pMap.SQL(UBound(pMap.SQL)).ErrMsg = IIf(IsDBNull(dtRow2.ItemArray(1)), "", dtRow2.ItemArray(1))
                                ReDim Preserve pMap.SQL(pMap.SQL.Length)
                            Next
                        Next
                        pRules.Add(pMap)
                    Next

                End If

                Dim dtr As DataRow
                dtr = dt1.NewRow()



                If ds.Tables("CoyName").Rows.Count > 0 Then
                    dr = ds.Tables("CoyName").Rows(0)
                    strSheet = Common.parseNull(dr("Sheet"))
                    strRangeFr = dr("ColFrom") & dr("RowFrom")
                    strRangeTo = dr("ColTo") & dr("RowTo")
                    strSql = "SELECT * FROM [" & strSheet & "$" & strRangeFr & ":" & strRangeTo & "]"
                    If OpenConnToExcel(pExcel) Then
                        ds1 = ObjDb.FillDsOle(strSql)
                        ds1 = ClearNull(ds1)
                        If ds1.Tables(0).Rows.Count > 0 Then
                            'Return ds
                            dtr("CoyName") = ds1.Tables(0).Rows(0).Item(2)
                        Else
                            dtr("CoyName") = ""
                            'Return Nothing
                        End If
                    Else
                        'strMassage = strMassage
                        Return Nothing
                    End If
                End If



                If ds.Tables("DocNo").Rows.Count > 0 Then
                    dr = ds.Tables("DocNo").Rows(0)
                    strSheet = Common.parseNull(dr("Sheet"))
                    strRangeFr = dr("ColFrom") & dr("RowFrom")
                    strRangeTo = dr("ColTo") & dr("RowTo")
                    strSql = "SELECT * FROM [" & strSheet & "$" & strRangeFr & ":" & strRangeTo & "]"
                    If OpenConnToExcel(pExcel) Then
                        ds1 = ObjDb.FillDsOle(strSql)
                        ds1 = ClearNull(ds1)
                        If ds1.Tables(0).Rows.Count > 0 Then
                            'Return ds
                            dtr("BillTo") = ds1.Tables(0).Rows(0).Item(2)
                        Else
                            dtr("BillTo") = ""
                            'Return Nothing
                        End If
                    Else
                        'strMassage = strMassage
                        Return Nothing
                    End If
                End If

                'If ds.Tables("DocDate").Rows.Count > 0 Then
                '    dr = ds.Tables("DocDate").Rows(0)
                '    strSheet = Common.parseNull(dr("Sheet"))
                '    strRangeFr = dr("ColFrom") & dr("RowFrom")
                '    strRangeTo = dr("ColTo") & dr("RowTo")
                '    strSql = "SELECT * FROM [" & strSheet & "$" & strRangeFr & ":" & strRangeTo & "]"
                '    If OpenConnToExcel(pExcel) Then
                '        ds1 = ObjDb.FillDsOle(strSql)
                '        ds1 = ClearNull(ds1)
                '        If ds1.Tables(0).Rows.Count > 0 Then
                '            'Return ds
                '            dtr("DocDate") = ds1.Tables(0).Rows(0).Item(1)
                '        Else
                '            dtr("DocDate") = System.DBNull.Value
                '            'Return Nothing
                '        End If
                '    Else
                '        'strMassage = strMassage
                '        Return Nothing
                '    End If
                'End If

                dt1.Rows.Add(dtr)
                dtHeader = dt1

                If ds.Tables("ReadArea").Rows.Count > 0 Then
                    dr = ds.Tables("ReadArea").Rows(0)
                    strSheet = Common.parseNull(dr("Sheet"))
                    strRangeFr = dr("ColFrom") & dr("RowFrom")
                    strRangeTo = dr("ColTo") & dr("RowTo")
                    strSql = "SELECT * FROM [" & strSheet & "$" & strRangeFr & ":" & strRangeTo & "]"
                    If OpenConnToExcel(pExcel) Then
                        ds = ObjDb.FillDsOle(strSql)
                        ds = ClearNull(ds)
                        If ds.Tables(0).Rows.Count > 0 Then
                            Return ds
                        Else
                            strMassage = "No record found."
                            Return Nothing
                        End If
                    Else
                        'strMassage = strMassage
                        Return Nothing
                    End If
                End If


            Catch expErr As Exception
                'Common.TrwExp(expErr)
                Throw New CustomException("Xml Reading Error. Please check " & pExcel & ".")
            End Try
        End Function
#End Region

        Public Function ReadIPPMultiInvoiceExcelFormat(ByVal pXML As String, ByVal pExcel As String, ByRef pRules As myCollection, ByRef dsHeader As DataSet, ByRef version As String, Optional ByVal strIsGST As String = "", Optional ByRef dsDuplicate As DataSet = Nothing) As DataSet
            Dim objXML As New AppXml
            Dim aa As UploadProduct
            Dim ds As DataSet
            Dim ds1, dsVersion As New DataSet
            Dim strSheet, strRangeFr, strRangeTo, strSql As String
            Dim dr As DataRow
            Dim intTotalRow, intLoop As Integer
            Dim dt1 As New DataTable

            Try
                ds = objXML.GetDsFromXML(pXML)

                'Zulham 19042019
                'If ds.Tables("InvoiceMstrReadArea").Rows.Count > 0 Then
                '    dr = ds.Tables("InvoiceMstrReadArea").Rows(0)
                '    strSheet = Common.parseNull(dr("Sheet"))
                '    strRangeFr = dr("ColFrom") & dr("RowFrom")
                '    strRangeTo = dr("ColTo") & dr("RowTo")
                '    strSql = "SELECT * FROM [" & strSheet & "$" & strRangeFr & ":" & strRangeTo & "]"
                '    If OpenConnToExcel(pExcel, , "IPP") Then
                '        dsHeader = ObjDb.FillDsOle(strSql)
                '        dsHeader = ClearNull(dsHeader)
                '        If ds.Tables(0).Rows.Count = 0 Then
                '            strMassage = "No record found."
                '            Return Nothing
                '        End If
                '    Else
                '        'strMassage = strMassage
                '        Return Nothing
                '    End If
                'End If

                If ds.Tables("DuplicateChecker").Rows.Count > 0 Then
                    dr = ds.Tables("DuplicateChecker").Rows(0)
                    strSheet = Common.parseNull(dr("Sheet"))
                    strRangeFr = dr("ColFrom") & dr("RowFrom")
                    strRangeTo = dr("ColTo") & dr("RowTo")
                    strSql = "SELECT * FROM [" & strSheet & "$" & strRangeFr & ":" & strRangeTo & "]"
                    If OpenConnToExcel(pExcel, , "IPP") Then
                        dsDuplicate = ObjDb.FillDsOle(strSql)
                        dsDuplicate = ClearNull(dsDuplicate)
                    End If
                End If

                If ds.Tables("InvoiceDetailsReadArea").Rows.Count > 0 Then
                    dr = ds.Tables("InvoiceDetailsReadArea").Rows(0)
                    strSheet = Common.parseNull(dr("Sheet"))
                    strRangeFr = dr("ColFrom") & dr("RowFrom")
                    strRangeTo = dr("ColTo") & dr("RowTo")
                    strSql = "SELECT * FROM [" & strSheet & "$" & strRangeFr & ":" & strRangeTo & "]"
                    If OpenConnToExcel(pExcel, , "IPP") Then
                        ds = ObjDb.FillDsOle(strSql)
                        ds = ClearNull(ds)
                        If ds.Tables(0).Rows.Count > 0 Then
                            Return ds
                        Else
                            strMassage = "No record found."
                            Return Nothing
                        End If
                    Else
                        'strMassage = strMassage
                        Return Nothing
                    End If
                End If

            Catch expErr As Exception
                'Common.TrwExp(expErr)
                Throw New CustomException("Xml Reading Error. Please check " & pExcel & ".")
            End Try
        End Function

    End Class
    Public Class ExcelPOUplopadHeader
        Public CoyName As String
        Public BillTo As String
    End Class


End Namespace