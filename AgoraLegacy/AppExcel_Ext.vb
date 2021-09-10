Imports System
Imports System.IO
Imports System.Collections
Imports System.Configuration
Imports System.Web
Imports SSO.Component

Namespace AgoraLegacy

    Public Class ExcelBIMUnitPrice_Common_Ext
        Public ItemCode As String
        Public OldUnitPrice As String
        Public NewUnitPrice As String
    End Class

    Public Class ExcelBIMProduct_CommonNew_Ext
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
        Public PartDelCode As String
        Public AccCode As String
        Public OrderQtyMin As String
        Public OrderQtyMax As String
        Public SafetyLevelMin As String
        Public MaxInvQty As String
        Public RQL As String
        Public BudgetPrice As String
        Public IQC As String
        Public EOQ As String
        Public Ratio As String
        Public Oversea As String
        Public Brand As String
        Public Spec As String
        Public Spec2 As String
        Public Spec3 As String
        Public PackType As String
        Public PackQty As String
        Public ManuName As String
        Public Manu2Name As String
        Public Manu3Name As String
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
        Public SectionCode As String
        Public LocationCode As String
        Public NewItemCode As String
        Public Remarks As String
        Public PreferredVen As String
        Public Ven1st As String
        Public Ven2nd As String
        Public Ven3rd As String
        Public PreferredVenSuppCode As String
        Public Ven1stSuppCode As String
        Public Ven2ndSuppCode As String
        Public Ven3rdSuppCode As String
        Public PreferredVenDelTerm As String
        Public Ven1stDelTerm As String
        Public Ven2ndDelTerm As String
        Public Ven3rdDelTerm As String
        Public PreferredVenUnitPrice As Double
        Public Ven1stUnitPrice As Double
        Public Ven2ndUnitPrice As Double
        Public Ven3rdUnitPrice As Double
        Public PreferredVenVolQtyPrice As String
        Public Ven1stVolQtyPrice As String
        Public Ven2ndVolQtyPrice As String
        Public Ven3rdVolQtyPrice As String
        Public PreferredVenPayTerm As String
        Public Ven1stPayTerm As String
        Public Ven2ndPayTerm As String
        Public Ven3rdPayTerm As String
        Public PreferredVenCurr As String
        Public Ven1stCurr As String
        Public Ven2ndCurr As String
        Public Ven3rdCurr As String
        Public PreferredVenPurSpecNo As String
        Public Ven1stPurSpecNo As String
        Public Ven2ndPurSpecNo As String
        Public Ven3rdPurSpecNo As String
        Public PreferredVenRevision As String
        Public Ven1stRevision As String
        Public Ven2ndRevision As String
        Public Ven3rdRevision As String
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

    Public Class AppExcel_Ext
        Dim dDispatcher As New AgoraLegacy.dispatcher
        Private ObjDb As New EAD.DBCom
        Public Ven2ndLead As String
        Public Ven3rdLead As String
        Public PreferredVenItemCode As String
        Public Ven1stItemCode As String
        Public Ven2ndItemCode As String
        Public Ven3rdItemCode As String
        Dim strMassage As String

        Public Function OpenConnToExcel(ByVal pFilePath As String, Optional ByVal pHeader As Boolean = False) As Boolean
            Dim sConn As String
            Try
                If Right(pFilePath, 3) <> "xls" Then
                    strMassage = "Target file is expecting excel file format."
                    Return False
                End If

                sConn = " Provider=Microsoft.Jet.OLEDB.4.0;" &
                        " Data Source=" & pFilePath & ";" &
                        " Extended Properties=""Excel 8.0;HDR=NO"""

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

        Public Function ReadExcelFormatForBudget(ByVal pXML As String, ByVal pExcel As String, ByRef pRules As myCollection) As DataSet
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

                    dr = ds.Tables("ReadArea").Rows(0)
                    strSheet = Common.parseNull(dr("Sheet"))
                    strRangeFr = dr("ColFrom") & dr("RowFrom")
                    strRangeTo = dr("ColTo") & dr("RowTo")
                    'strSql = "SELECT * FROM [BudgetPrice$A32:I32]"
                    strSql = "SELECT * FROM [" & strSheet & "$" & strRangeFr & ":" & strRangeTo & "]"
                    If OpenConnToExcel(pExcel) Then
                        ds = ObjDb.FillDsOle(strSql)
                        ds = ClearNull(ds)
                        If ds.Tables(0).Rows.Count > 0 Then

                            'Get Item name of each item code from DB then insert to dataset (Only for BudgetPrice)
                            Dim i As Integer
                            Dim strName As String
                            For i = 0 To ds.Tables(0).Rows.Count - 1
                                If ds.Tables(0).Rows(i)("F2") <> "" Then
                                    strName = ObjDb.GetVal("SELECT PM_PRODUCT_DESC FROM PRODUCT_MSTR WHERE PM_VENDOR_ITEM_CODE = '" & Common.Parse(ds.Tables(0).Rows(i)("F2")) & "'")

                                    If strName <> "" Then
                                        ds.Tables(0).Rows(i)("F3") = strName
                                    Else
                                        ds.Tables(0).Rows(i)("F3") = ""
                                    End If
                                End If

                            Next

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


#Region "Testing Only"

        Public Sub Writecell_Common(ByVal ds As DataSet, ByVal ds2 As DataSet, ByVal dsPackType As DataSet, ByVal dsDelTerm As DataSet, ByVal dsCommodity As DataSet, ByVal pPath As String, Optional ByVal blnExport As Boolean = False)
            Dim ctx As Web.HttpContext = Web.HttpContext.Current
            Dim strSourceFile, strDestFile, filename As String
            strSourceFile = ctx.Server.MapPath(ctx.Request.ApplicationPath) & "\SEH\Template\ItemBIMListingTemplate.xls"
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

            If dsPackType.Tables(0).Rows.Count > 0 Or dsDelTerm.Tables(0).Rows.Count > 0 Then
                'Packing Type
                For i = 0 To dsPackType.Tables(0).Rows.Count - 1
                    lsSql = "Insert into [PredefineCode$C" & i + 1 & ":C" & i + 1 & "] Values ('" & Common.Parse(dsPackType.Tables(0).Rows(i).Item("Packing Type")) & "')"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next

                'Delivery Term
                For i = 0 To dsDelTerm.Tables(0).Rows.Count - 1
                    lsSql = "Insert into [PredefineCode$D" & i + 1 & ":D" & i + 1 & "] Values ('" & Common.Parse(dsDelTerm.Tables(0).Rows(i).Item("Delivery Term")) & "')"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next

                'Delivery Term
                For i = 0 To dsCommodity.Tables(0).Rows.Count - 1
                    lsSql = "Insert into [PredefineCode$B" & i + 1 & ":B" & i + 1 & "] Values ('" & Common.Parse(dsCommodity.Tables(0).Rows(i).Item("Commodity Type")) & "')"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next

            End If

            TotalRow = ds.Tables(0).Rows.Count
            intTotRow = ds.Tables(0).Rows.Count + 1
            intTotCol = ds.Tables(0).Columns.Count

            If TotalRow > 0 Then
                For intLoop = 2 To intTotRow
                    lsSql = "Insert into [BIM$] Values ("
                    lsSql = lsSql & "'" & (intLoop - 1) & "'"
                    For intLoop1 = 0 To intTotCol - 1
                        If IsDBNull(ds.Tables(0).Rows(intLoop - 2)(intLoop1)) Then
                            lsSql = lsSql & ",'" & ds.Tables(0).Rows(intLoop - 2)(intLoop1) & "'"
                        Else
                            lsSql = lsSql & ",'" & Common.Parse(ds.Tables(0).Rows(intLoop - 2)(intLoop1)) & "'"
                        End If
                    Next
                    If blnExport = True Then
                        lsSql = lsSql & ")"
                    Else
                        lsSql = lsSql & ",'M')"
                    End If

                    Common.Insert2Ary(SqlAry, lsSql)
                Next

            Else
                Exit Sub
            End If

            TotalRow = ds2.Tables(0).Rows.Count
            intTotRow = ds2.Tables(0).Rows.Count + 1
            intTotCol = ds2.Tables(0).Columns.Count


            If TotalRow > 0 Then
                For intLoop = 2 To intTotRow
                    lsSql = "Insert into [VENDOR$A" & intLoop - 1 & ":L" & intLoop - 1 & "] Values ("
                    lsSql = lsSql & "'" & (intLoop - 1) & "'"
                    For intLoop1 = 0 To intTotCol - 1
                        If IsDBNull(ds2.Tables(0).Rows(intLoop - 2)(intLoop1)) Then
                            lsSql = lsSql & ",'" & ds2.Tables(0).Rows(intLoop - 2)(intLoop1) & "'"
                        Else
                            lsSql = lsSql & ",'" & Common.Parse(ds2.Tables(0).Rows(intLoop - 2)(intLoop1)) & "'"
                        End If
                    Next
                    lsSql = lsSql & ")"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next

            End If

            ObjDb.BatchExecuteOle(SqlAry)
            ObjDb = Nothing
        End Sub

        Public Sub Writecell_Common2(ByVal ds As DataSet, ByVal pPath As String)
            Dim ctx As Web.HttpContext = Web.HttpContext.Current

            Dim strSourceFile As String = ctx.Server.MapPath(ctx.Request.ApplicationPath) & "\SEH\Template\BIMBudgetListingTemplate.xls"
            Dim strDestFile As String = ctx.Server.MapPath(ctx.Request.ApplicationPath) & "\SEH\Template\BIMBudgetListing.xls"
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
                    lsSql = "Insert into [BudgetPrice$A" & intLoop - 1 & ":G" & intLoop - 1 & "] Values ("
                    'lsSql = "Insert into [BudgetPrice$] Values ("
                    lsSql = lsSql & "'" & (intLoop - 1) & "',"
                    For intLoop1 = 0 To intTotCol - 1
                        If IsDBNull(ds.Tables(0).Rows(intLoop - 2)(intLoop1)) Then
                            lsSql = lsSql & "'" & ds.Tables(0).Rows(intLoop - 2)(intLoop1) & "',"
                        Else
                            lsSql = lsSql & "'" & Common.Parse(ds.Tables(0).Rows(intLoop - 2)(intLoop1)) & "',"
                        End If
                    Next
                    lsSql = lsSql & "'')"
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

            Dim strSourceFile As String = ctx.Server.MapPath(ctx.Request.ApplicationPath) & "\SEH\Template\ItemBIMListingTemplate.xls"
            Dim strDestFile As String = ctx.Server.MapPath(ctx.Request.ApplicationPath) & "\SEH\Template\ItemBIMListing.xls"
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

        Public Sub WriteCell_ItemUpload(ByVal pFile As String, ByVal pPath As String, ByVal ds As DataSet, ByVal ds1 As DataSet, ByVal ds2 As DataSet)
            Dim ctx As Web.HttpContext = Web.HttpContext.Current
            Dim filename, strSourceFile, strDestFile As String

            If pFile = "ItemBIMTemplate" Then
                strSourceFile = ctx.Server.MapPath(ctx.Request.ApplicationPath) & "\SEH\Template\ItemBIMTemplate.xls"
                filename = "ItemBIMTemplate_" & HttpContext.Current.Session("CompanyId") & "_" & HttpContext.Current.Session("UserId") & ".xls"
            Else
                strSourceFile = ctx.Server.MapPath(ctx.Request.ApplicationPath) & "\SEH\Template\ItemBIMListingTemplate.xls"
                filename = "ItemBIMListing_" & HttpContext.Current.Session("CompanyId") & "_" & HttpContext.Current.Session("UserId") & ".xls"
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
                'Packing Type
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    lsSql = "Insert into [PredefineCode$C" & i + 1 & ":C" & i + 1 & "] Values ('" & Common.Parse(ds.Tables(0).Rows(i).Item("Packing Type")) & "')"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next

                'Delivery Term
                For i = 0 To ds1.Tables(0).Rows.Count - 1
                    lsSql = "Insert into [PredefineCode$D" & i + 1 & ":D" & i + 1 & "] Values ('" & Common.Parse(ds1.Tables(0).Rows(i).Item("Delivery Term")) & "')"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next

                'Commodity Type
                For i = 0 To ds2.Tables(0).Rows.Count - 1
                    lsSql = "Insert into [PredefineCode$B" & i + 1 & ":B" & i + 1 & "] Values ('" & Common.Parse(ds2.Tables(0).Rows(i).Item("Commodity Type")) & "')"
                    Common.Insert2Ary(SqlAry, lsSql)
                Next

                ObjDb.BatchExecuteOle(SqlAry)
                ObjDb = Nothing
            End If

        End Sub

        Public Function ProtectWorkSheet_ItemUpload(ByVal pFileName As String, ByVal pPackTypeCnt As Integer, ByVal pDelTermCnt As Integer, ByVal pCommodityCnt As Integer, Optional ByVal pNew As Boolean = False)
            Dim xlApp As New Excel.Application
            Dim xlWorkBook As Excel.Workbook
            Dim xlWorkSheet, xlWorkSheetPO As Excel.Worksheet
            Dim xlRangePackType, xlRangeDelTerm, xlRangeCommodity As Excel.Range
            Dim MyPassword As String = ConfigurationManager.AppSettings("ExcelPwd")
            Dim proc As System.Diagnostics.Process
            Dim intPID As Integer

            xlWorkBook = xlApp.Workbooks.Open(pFileName)


            'Chee Hong: enhancement on 23th Oct 2013
            'Same format & style with ItemBIMTemplate.xls
            'BIM Sheet
            xlWorkSheet = xlWorkBook.Sheets("BIM")
            'Set Font Name to Arial Narrow
            xlWorkSheet.Range("A1:AU2000").Font.Name = "Arial Narrow"
            'Set Font style to Bold
            xlWorkSheet.Range("A1:AU1").Font.Bold = True
            'Set Background color 
            xlWorkSheet.Range("A1:AU1").Interior.ColorIndex = 35
            'Set * to red color
            xlWorkSheet.Range("B1").Characters(11, 1).Font.Color = RGB(255, 0, 0)
            xlWorkSheet.Range("C1").Characters(11, 1).Font.Color = RGB(255, 0, 0)
            xlWorkSheet.Range("D1").Characters(11, 1).Font.Color = RGB(255, 0, 0)
            xlWorkSheet.Range("F1").Characters(16, 1).Font.Color = RGB(255, 0, 0)
            xlWorkSheet.Range("G1").Characters(5, 1).Font.Color = RGB(255, 0, 0)
            xlWorkSheet.Range("J1").Characters(22, 1).Font.Color = RGB(255, 0, 0)
            xlWorkSheet.Range("K1").Characters(21, 1).Font.Color = RGB(255, 0, 0)
            xlWorkSheet.Range("V1").Characters(9, 1).Font.Color = RGB(255, 0, 0)
            If pNew = True Then
                xlWorkSheet.Protect(MyPassword, Contents:=True)
            End If

            'Vendor Sheet
            xlWorkSheet = xlWorkBook.Sheets("VENDOR")
            xlWorkSheet.Range("A1:L2000").Font.Name = "Arial Narrow"
            xlWorkSheet.Range("A1:L1").Font.Bold = True
            xlWorkSheet.Range("A1:L1").Interior.ColorIndex = 35
            'xlWorkSheet.Range("A1:M2000").Font.Name = "Arial Narrow"
            'xlWorkSheet.Range("A1:M1").Font.Bold = True
            'xlWorkSheet.Range("A1:M1").Interior.ColorIndex = 35
            If pNew = True Then
                xlWorkSheet.Protect(MyPassword, Contents:=True)
            End If


            'PredefineCode Sheet
            xlWorkSheet = xlWorkBook.Sheets("PredefineCode")

            xlWorkSheet.Range("B1:D1").Font.Bold = True

            xlRangePackType = xlWorkSheet.Range("C2:C" & pPackTypeCnt + 1)
            xlRangePackType.Name = "PackingType"
            xlRangeDelTerm = xlWorkSheet.Range("D2:D" & pDelTermCnt + 1)
            xlRangeDelTerm.Name = "DeliveryTerm"
            xlRangeCommodity = xlWorkSheet.Range("B2:B" & pCommodityCnt + 1)
            xlRangeCommodity.Name = "Commodity"
            xlWorkSheet.Protect(MyPassword, Contents:=True)
            xlWorkSheet.EnableAutoFilter = True
            xlApp.DisplayAlerts = False
            xlWorkBook.Save()
            xlWorkBook.Close()
            xlApp.Quit()
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangePackType)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangeDelTerm)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRangeCommodity)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkSheet)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp)
            xlRangePackType = Nothing
            xlRangeDelTerm = Nothing
            xlRangeCommodity = Nothing
            xlWorkSheet = Nothing
            xlWorkBook = Nothing
            xlApp = Nothing

            'To kill the current 'EXCEL' process, if not, it will always in Task Manager
            intPID = System.Diagnostics.Process.GetProcessesByName("EXCEL")(0).Id
            proc = System.Diagnostics.Process.GetProcessById(intPID)
            proc.Kill()

        End Function

#End Region
    End Class
End Namespace