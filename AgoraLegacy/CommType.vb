Imports AgoraLegacy
Namespace AgoraLegacy
    Public Class CommType
        Dim objDb As New EAD.DBCom

        Public Function getCommodityTypeList() As DataTable
            Dim strsql As String
            strsql = "SELECT CT_ID, CT_NAME, CT_LAST_LVL, (SELECT COUNT(*) FROM COMMODITY_TYPE WHERE CT_PARENT_ID=sc.CT_ID) childnodecount FROM COMMODITY_TYPE sc WHERE CT_PARENT_ID IS NULL"
            Return objDb.FillDt(strsql)
        End Function

        Public Function getCommodityTypeListChild(ByVal parentID As Integer) As DataTable
            Dim strsql As String
            strsql = "SELECT CT_ID, CT_NAME, CT_LAST_LVL, (SELECT COUNT(*) FROM COMMODITY_TYPE WHERE CT_PARENT_ID=sc.CT_ID) childnodecount FROM COMMODITY_TYPE sc where CT_PARENT_ID=" & parentID
            Return objDb.FillDt(strsql)
        End Function

        Function searchBuyerCatItems(ByVal sSearch As String) As DataSet
            'Dim strSQL As String, sLevel1 As String, sLevel2 As String, sLevel3 As String, sLevel4 As String
            'Dim arrParent As ArrayList
            Dim ds As DataSet
            Dim strSQL As String
            'Dim constructDS As New DataSet("DataSet"), dt As New DataTable("DataTable"), dr As DataRow

            'Dim cl1 As New DataColumn("CT_CODE")
            'cl1.DataType = GetType(Integer)
            'Dim cl2 As New DataColumn("Level_1")
            'cl2.DataType = GetType(String)
            'Dim cl3 As New DataColumn("Level_2")
            'cl3.DataType = GetType(String)
            'Dim cl4 As New DataColumn("Level_3")
            'cl4.DataType = GetType(String)
            'Dim cl5 As New DataColumn("Level_4")
            'cl5.DataType = GetType(String)
            'Dim cl6 As New DataColumn("CT_ID")
            'cl6.DataType = GetType(Integer)
            'dt.Columns.Add(cl1)
            'dt.Columns.Add(cl2)
            'dt.Columns.Add(cl3)
            'dt.Columns.Add(cl4)
            'dt.Columns.Add(cl5)
            'dt.Columns.Add(cl6)

            'constructDS.Tables.Add(dt)

            strSQL = "SELECT CT_ID, CT_PARENT_ID, CT_NAME AS Level_4, CT_LAST_LVL, CT_ROOT_PREFIX, CT_CODE,CT_LEVEL_1 AS Level_1,CT_LEVEL_2 AS Level_2,CT_LEVEL_3  AS Level_3 FROM commodity_type WHERE CT_LAST_LVL = 1 AND CT_NAME LIKE " & """%" & sSearch & "%"""
            ds = objDb.FillDs(strSQL)


            'For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1                
            '    arrParent = constructHigherLevel(tDS.Tables(0).Rows(j).Item("CT_PARENT_ID"))
            '    dr = constructDS.Tables("DataTable").NewRow
            '    dr("CT_CODE") = tDS.Tables(0).Rows(j).Item("CT_CODE")
            '    dr("Level_1") = arrParent(0)
            '    dr("Level_2") = arrParent(1)
            '    dr("Level_3") = arrParent(2)
            '    dr("Level_4") = tDS.Tables(0).Rows(j).Item("CT_NAME")
            '    dr("CT_ID") = tDS.Tables(0).Rows(j).Item("CT_ID")
            '    constructDS.Tables("DataTable").Rows.Add(dr)
            'Next

            Return ds
        End Function

        Function constructHigherLevel(ByVal parentId As Integer) As ArrayList
            Dim strsql As String, tDS As DataSet, levelName3 As String, levelName2 As String, levelName1 As String

            'Get third level
            tDS = objDb.Fill1Ds("commodity_type", "CT_PARENT_ID, CT_NAME", "WHERE CT_ID = " & parentId)
            levelName3 = tDS.Tables(0).Rows(0).Item("CT_NAME")

            'Get second level
            tDS = objDb.Fill1Ds("commodity_type", "CT_PARENT_ID, CT_NAME", "WHERE CT_ID = " & tDS.Tables(0).Rows(0).Item("CT_PARENT_ID"))
            levelName2 = tDS.Tables(0).Rows(0).Item("CT_NAME")
                      
            'Get first level
            If Not IsDBNull(tDS.Tables(0).Rows(0).Item("CT_PARENT_ID")) Then
                levelName1 = objDb.GetVal("SELECT CT_NAME FROM commodity_type WHERE CT_ID = " & tDS.Tables(0).Rows(0).Item("CT_PARENT_ID"))
            End If

            Dim lvlArray As System.Collections.ArrayList = New System.Collections.ArrayList
            lvlArray.Add(levelName1)
            lvlArray.Add(levelName2)
            lvlArray.Add(levelName3)

            constructHigherLevel = lvlArray

        End Function
    End Class

End Namespace