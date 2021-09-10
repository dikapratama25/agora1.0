Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports Microsoft.Web.Script.Services
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
Public Class AutoComplete
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function GetCompany(ByVal prefixText As String, _
                                  ByVal count As Integer) As String()

        Dim objDb As New  EAD.DBCom(ConfigurationSettings.AppSettings("eProcurePath"))
        Dim tDS As DataSet = objDb.FillDs("SELECT CM_COY_NAME FROM company_mstr WHERE CM_DELETED <> 'Y' AND CM_COY_NAME LIKE '%" & prefixText & "%'")


        Dim Results As New ArrayList
        Try
            Dim Counter As Integer

            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                If (Counter = count) Then Exit For
                Results.Add(tDS.Tables(0).Rows(j).Item("CM_COY_NAME"))
                Counter += 1
            Next

            Dim ResultsArray(Results.Count - 1) As String
            ResultsArray = Results.ToArray(GetType(System.String))
            Return ResultsArray

        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class