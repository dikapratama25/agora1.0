Imports System.Web
Imports System.Web.Security

Public Class Package
    Public packageName As Array = Array.CreateInstance(GetType(String), 4)
    Public packageURL As Array = Array.CreateInstance(GetType(String), 4)
    Public packageID As Array = Array.CreateInstance(GetType(String), 4)
    Dim ctx As HttpContext = HttpContext.Current

    Public Sub getPackageDetails(ByVal coyID As String, ByVal uID As String)
        Dim objDb As New EAD.DBCom
        Dim strSQL As String
        Dim i As Integer = 0

        strSQL = "SELECT DISTINCT(AP.AP_APP_NAME), AP.AP_DB_NAME, AP.AP_URL, AP.AP_INDEX, AP.AP_APP_ID " &
                "FROM USERS_USRGRP AS UU, APPLICATION_MSTR AS AP " &
                "WHERE(AP.AP_APP_ID = UU.UU_APP_PKG) " &
                "AND UU.UU_COY_ID = '" & coyID & "' " &
                "AND UU.UU_USER_ID = '" & uID & "' " &
                "ORDER BY AP.AP_INDEX "

        Dim tDS As DataSet = objDb.FillDs(strSQL)
        For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
            packageName.SetValue(tDS.Tables(0).Rows(j).Item(0), i)
            packageURL.SetValue(tDS.Tables(0).Rows(j).Item(2), i)
            packageID.SetValue(tDS.Tables(0).Rows(j).Item(4), i)
            i += 1
        Next
        objDb = Nothing
    End Sub

    Public Function getCountTotalPackages(ByVal coyID As String, ByVal uid As String) As Integer
        Dim objDb As New EAD.DBCom()
        Dim strSQL As String
        Dim aTotalPackage As Integer

        'statement is base on users_usrgrp table
        strSQL = "SELECT COUNT(DISTINCT(UU_APP_PKG)) " &
                    "FROM USERS_USRGRP " &
                    "WHERE UU_COY_ID = '" & coyID & "' " &
                    " AND UU_USER_ID = '" & uid & "'"


        'statement is base on company_application table 
        'strsql = "SELECT Count(*) as A " & _
        '                "FROM COMPANY_APPLICATION AS CA, APPLICATION_MSTR AS AP " & _
        '                "WHERE(AP.AP_APP_ID = CA.CA_APP_ID) " & _
        '                "AND CA.CA_COY_ID ='" & coyID & "'"
        objDb.ConnState()
        Dim tDS As DataSet = objDb.FillDs(strSQL)
        aTotalPackage = tDS.Tables(0).Rows(0).Item(0)
        Return aTotalPackage
    End Function

    Public Function checkPackageAuthorization(ByVal currentPack As String) As Boolean
        Dim isAuthorized As Boolean = False
        Dim allPackageName As String
        Dim packageName() As String
        Dim i As Integer

        allPackageName = ctx.Request.Cookies("aPackageID").Value
        packageName = allPackageName.Split("|")

        For i = 0 To ctx.Request.Cookies("aTotalPackage").Value - 1
            If packageName(i) = currentPack Then
                isAuthorized = True
                Exit For
            End If
        Next
        Return isAuthorized

    End Function

    Public Sub signOut()
        ClearUpSession()
        ClearUpCookies()
        FormsAuthentication.SignOut()
        'ctx.Response.Redirect("../login.aspx")
    End Sub

    Private Sub ClearUpSession()
        ctx.Session.Abandon()
        ctx.Session.Clear()
        ctx.Session("UserId") = Nothing
        ctx.Session("UserName") = Nothing
        ctx.Session("CompanyId") = Nothing
        ctx.Session("CompanyName") = Nothing
        ctx.Session("Last_Login") = Nothing
        ctx.Session("Env") = Nothing
    End Sub

    Private Sub ClearUpCookies()
        ctx.Response.Cookies.Clear()
        ctx.Request.Cookies.Clear()
        'ctx.Request.Cookies.Remove("aHideUserId")
        'ctx.Request.Cookies.Remove("aHideCompanyId")
        'ctx.Request.Cookies.Remove("aPackageName")
        'ctx.Request.Cookies.Remove("aTotalPackage")
        ctx.Response.Cookies("aHideUserId").Expires = DateTime.Now.AddDays(-1)
        ctx.Response.Cookies("aHideCompanyId").Expires = DateTime.Now.AddDays(-1)
        ctx.Response.Cookies("aPackageName").Expires = DateTime.Now.AddDays(-1)
        ctx.Response.Cookies("aPackageID").Expires = DateTime.Now.AddDays(-1)
        ctx.Response.Cookies("aTotalPackage").Expires = DateTime.Now.AddDays(-1)
    End Sub

    Public Function getAllPackageDetails(ByVal strCoyId As String) As DataSet
        Dim ds As New DataSet
        Dim objDb As New EAD.DBCom()
        Dim strsql As String
        strsql = "SELECT DISTINCT AP_APP_ID, AP_APP_NAME, AP_COLOR FROM COMPANY_APPLICATION "
        strsql &= "INNER JOIN APPLICATION_MSTR ON CA_APP_ID = AP_APP_ID "
        strsql &= "INNER JOIN USERS_USRGRP ON UU_COY_ID = CA_COY_ID "
        strsql &= "WHERE CA_COY_ID = '" & strCoyId & "'  "
        ds = objDb.FillDs(strsql)
        getAllPackageDetails = ds
        objDb = Nothing
    End Function

    'Public Function getCompanyPackages(ByVal strCoyId As String) As String
    '    Dim strsql As String
    '    Dim objdb As New EAD.DBCom()
    '    strsql = "SELECT "
    'End Function
End Class
