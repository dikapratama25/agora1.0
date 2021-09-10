Imports System
Imports System.Diagnostics
Imports System.Web
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration


Public Class CheckAccess
    Public Const INIFileName As String = "eProcurement.ini"
    Dim readINI As New Functions.INIFile
    Dim inipath = System.Configuration.ConfigurationSettings.AppSettings("eProcurementINIPath")
    Public billingPath As String = readINI.ReadIni(inipath, "SYSTEM", "BillingPath")

    Public Function getAccess(ByVal compID As String) As String
        Dim objSOAPClient As Object
        objSOAPClient = CreateObject("MSSOAP.SoapClient30")
        objSOAPClient.ClientProperty("ServerHTTPRequest") = True
        objSOAPClient.MSSoapInit(billingPath)
        Dim access = objSOAPClient.checkaccess(compID)
        getAccess = access
        objSOAPClient = Nothing
    End Function
    Public Function syncAccount(ByVal compID As String, ByVal passw As String) As String
        Dim objSOAPClient As Object
        objSOAPClient = CreateObject("MSSOAP.SoapClient30")
        objSOAPClient.ClientProperty("ServerHTTPRequest") = True
        objSOAPClient.MSSoapInit(billingPath)
        Dim sync = objSOAPClient.syncaccount(compID, passw)
        syncAccount = sync
        objSOAPClient = Nothing
    End Function
    Public Function syncDetail(ByVal xml As String) As String
        Dim objSOAPClient As Object
        objSOAPClient = CreateObject("MSSOAP.SoapClient30")
        objSOAPClient.ClientProperty("ServerHTTPRequest") = True
        objSOAPClient.MSSoapInit(billingPath)
        Dim sync = objSOAPClient.syncdetail(xml)
        syncDetail = sync
        objSOAPClient = Nothing
    End Function
    Public Function getCompanyType(ByVal compid As String) As String
        Dim objSOAPClient As Object
        objSOAPClient = CreateObject("MSSOAP.SoapClient30")
        objSOAPClient.ClientProperty("ServerHTTPRequest") = True
        objSOAPClient.MSSoapInit(billingPath)
        Dim sync = objSOAPClient.getCompanyType(compid)
        getCompanyType = sync
        objSOAPClient = Nothing
    End Function
End Class
