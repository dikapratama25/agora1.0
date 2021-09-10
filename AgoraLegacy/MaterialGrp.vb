Imports System
Imports System.Configuration
Imports System.Text
Imports System.Web
Imports System.Web.UI.WebControls
Imports AgoraLegacy

Namespace AgoraLegacy
    Public Class MaterialGrp

        Dim objDb As New EAD.DBCom(ConfigurationSettings.AppSettings("eProcurePath"))
        Dim objGlobal As New AppGlobals

        Public Function getCategory() As DataSet
            Dim strsql As String
            Dim ds As New DataSet
            strsql = "SELECT CM_CATEGORY_DESC, CM_CATEGORY_NAME, (CONCAT(CM_CATEGORY_NAME, ' ', CM_CATEGORY_DESC)) AS CATEGORY_DESC "
            strsql &= "FROM MATERIAL_MSTR, MATERIAL_MATERIAL "
            strsql &= "WHERE CC_SUBCATEGORY_NAME = CM_CATEGORY_NAME "
            strsql &= "AND CC_CATEGORY_NAME = '[Root Category]'"
            strsql &= "ORDER BY CM_CATEGORY_NAME "
            ds = objDb.FillDs(strsql)
            getCategory = ds
        End Function

        Public Function getCategoryType(ByVal strType As String, ByVal strName As String, ByVal strDesc As String) As DataSet
            Dim strsql As String
            Dim ds As New DataSet
            Select Case strType
                Case "0"
                    strsql = "SELECT CM_CATEGORY_DESC, CM_CATEGORY_NAME, CM_CATEGORY_TYPE, (CONCAT(CM_CATEGORY_NAME, ' ', CM_CATEGORY_DESC)) AS CATEGORY_DESC "
                    strsql &= "FROM MATERIAL_MSTR, MATERIAL_MATERIAL "
                    strsql &= "WHERE CC_SUBCATEGORY_NAME = CM_CATEGORY_NAME "
                    strsql &= "AND CC_CATEGORY_NAME = '" & strName & "' "
                    strsql &= "ORDER BY CC_CATEGORY_NAME"
                Case "1"
                    strsql = "SELECT M1.CM_CATEGORY_DESC, M1.CM_CATEGORY_NAME, M1.CM_CATEGORY_TYPE, "
                    strsql &= "(" & objDb.Concat(" ", "", "M1.CM_CATEGORY_NAME", "M1.CM_CATEGORY_DESC") & ") AS CATEGORY_DESC "
                    strsql &= "FROM (SELECT * FROM MATERIAL_MSTR "
                    ' ai chu modified on 06/09/2005 - search criteria should follow standard
                    'strsql &= "WHERE CM_CATEGORY_NAME LIKE '" & Common.Parse(strName) & "%' "
                    'strsql &= "AND CM_CATEGORY_DESC LIKE '" & Common.Parse(strDesc) & "%' "
                    If strName = "" Then
                        strsql &= "WHERE CM_CATEGORY_NAME LIKE '" & Common.Parse(strName) & "%' "
                    Else
                        strsql &= "WHERE CM_CATEGORY_NAME " & Common.ParseSQL(strName) & " "
                    End If

                    If strDesc = "" Then
                        strsql &= "AND CM_CATEGORY_DESC LIKE '" & Common.Parse(strDesc) & "%' "
                    Else
                        strsql &= "AND CM_CATEGORY_DESC " & Common.ParseSQL(strDesc) & " "
                    End If
                    strsql &= "AND CM_UNSPSC_TYPE IS NOT NULL) M2, "
                    strsql &= "MATERIAL_MSTR M1 WHERE (" & objDb.Concat("", "", "M1.CM_CATEGORY_NAME = LEFT(M2.CM_CATEGORY_NAME,2)", "000000")
                    strsql &= " OR " & objDb.Concat("", "", "M1.CM_CATEGORY_NAME = LEFT(M2.CM_CATEGORY_NAME,4)", "0000")
                    strsql &= " OR " & objDb.Concat("", "", "M1.CM_CATEGORY_NAME = LEFT(M2.CM_CATEGORY_NAME,6)", "00")
                    strsql &= " OR M1.CM_CATEGORY_NAME = M2.CM_CATEGORY_NAME) AND M1.CM_STATUS = 'A' "
                    strsql &= "AND M1.CM_UNSPSC_TYPE IS NOT NULL "
                    strsql &= "GROUP BY M1.CM_CATEGORY_NAME, M1.CM_CATEGORY_DESC, M1.CM_CATEGORY_TYPE, M1.CM_UNSPSC_TYPE "
                    strsql &= "ORDER BY M1.CM_CATEGORY_NAME "
            End Select
            ds = objDb.FillDs(strsql)
            getCategoryType = ds
        End Function


    End Class
End Namespace

