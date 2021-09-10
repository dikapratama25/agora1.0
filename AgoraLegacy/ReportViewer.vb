Imports System
Imports System.Configuration
Imports System.Data
Imports System.Text
Imports System.Web
Imports System.Web.UI.WebControls

Namespace AgoraLegacy
    Public Class ReportViewer
#Region " popCrystalReport function "

        Public Shared Sub popCrystalReport(ByRef pg As System.Web.UI.Page, ByVal ReportName As String)
            popCrystalReport(pg, ReportName, String.Empty, False)
        End Sub

        Public Shared Sub popCrystalReport(ByRef pg As System.Web.UI.Page, ByVal ReportName As String, ByVal SelectionFormula As String)
            popCrystalReport(pg, ReportName, SelectionFormula, False)
        End Sub

        Public Shared Sub popCrystalReport(ByRef pg As System.Web.UI.Page, ByVal ReportName As String, ByVal DisplayGroupTree As Boolean)
            popCrystalReport(pg, ReportName, String.Empty, DisplayGroupTree)
        End Sub

        Public Shared Sub popCrystalReport(ByRef pg As System.Web.UI.Page, ByVal ReportName As String, ByVal SelectionFormula As String, ByVal DisplayGroupTree As Boolean)
            Dim reportURL As String
            'reportURL = "CRViewer/viewrpt.asp?rptname=" & pg.Server.UrlEncode(ReportName)
            Dim strAppName As String = pg.Request.ApplicationPath
            reportURL = strAppName & "/Report/viewrpt.asp?rptname=" & pg.Server.UrlEncode(ReportName)
            If SelectionFormula <> String.Empty Then
                reportURL &= "&sf=" & pg.Server.UrlEncode(SelectionFormula)
            End If
            If DisplayGroupTree Then
                reportURL &= "&dgt=1"
            End If
            Dim jscript As String = String.Empty
            jscript &= "<script language=""Javascript"">" & vbCrLf
            jscript &= "x = screen.width -10;" & vbCrLf
            jscript &= "y = screen.height - 55;" & vbCrLf
            jscript &= "var props = 'scrollBars=no, resizable=yes, toolbar=no, menubar=no, location=no, directories=no, top=0, left=0, width=' + x + ', height=' + y ;" & vbCrLf
            jscript &= "window.open(""" & reportURL & """, '', props)" & vbCrLf
            jscript &= "//-->" & vbCrLf
            jscript &= "</script>" & vbCrLf
            'pg.RegisterStartupScript("CrystalReport", jscript)
            Dim rndKey As New Random
            pg.RegisterStartupScript(rndKey.Next.ToString, jscript)
        End Sub

#End Region

#Region " popCrystalReport with parameter support "

        Public Shared Sub popCrystalReport(ByRef pg As System.Web.UI.Page, ByVal ReportName As String, ByVal ParamTable As Hashtable)
            popCrystalReportWithParameter(pg, ReportName, String.Empty, ParamTable, False)
        End Sub

        Public Shared Sub popCrystalReport(ByRef pg As System.Web.UI.Page, ByVal ReportName As String, ByVal SelectionFormula As String, ByVal ParamTable As Hashtable)
            popCrystalReportWithParameter(pg, ReportName, SelectionFormula, ParamTable, False)
        End Sub

        Public Shared Sub popCrystalReport(ByRef pg As System.Web.UI.Page, ByVal ReportName As String, ByVal ParamTable As Hashtable, ByVal DisplayGroupTree As Boolean)
            popCrystalReportWithParameter(pg, ReportName, String.Empty, ParamTable, DisplayGroupTree)
        End Sub

        Public Shared Sub popCrystalReport(ByRef pg As System.Web.UI.Page, ByVal ReportName As String, ByVal SelectionFormula As String, ByVal ParamTable As Hashtable, ByVal DisplayGroupTree As Boolean)
            popCrystalReportWithParameter(pg, ReportName, SelectionFormula, ParamTable, DisplayGroupTree)
        End Sub

        Private Shared Sub popCrystalReportWithParameter(ByRef pg As System.Web.UI.Page, ByVal ReportName As String, ByVal SelectionFormula As String, ByVal ParamTable As Hashtable, ByVal DisplayGroupTree As Boolean)
            Dim reportURL As String
            'MsgBox(pg, pg.Request.ApplicationPath)
            'MsgBox(pg, pg.Request.Path)
            'MsgBox(pg, "UU=" & pg.Request.Url.)

            '//Use Virtual path, so report can be called anywhere
            Dim strAppName As String = pg.Request.ApplicationPath
            reportURL = strAppName & "/Report/viewrpt.asp?rptname=" & pg.Server.UrlEncode(ReportName)
            '//

            If SelectionFormula <> String.Empty Then
                reportURL &= "&sf=" & pg.Server.UrlEncode(SelectionFormula)
            End If
            If DisplayGroupTree Then
                reportURL &= "&dgt=1"
            End If

            Dim myEnumerator As IDictionaryEnumerator = ParamTable.GetEnumerator()
            Dim ParamString As String = String.Empty
            While myEnumerator.MoveNext()
                ParamString &= "&" & myEnumerator.Key & "=" & pg.Server.UrlEncode(myEnumerator.Value)
            End While

            If ParamString <> String.Empty Then
                reportURL &= ParamString
            End If

            Dim jscript As String = String.Empty
            jscript &= "<script language=""Javascript"">" & vbCrLf
            jscript &= "x = screen.width -10;" & vbCrLf
            jscript &= "y = screen.height - 55;" & vbCrLf
            jscript &= "var props = 'scrollBars=no, resizable=yes, toolbar=no, menubar=no, location=no, directories=no, top=0, left=0, width=' + x + ', height=' + y ;" & vbCrLf
            jscript &= "window.open(""" & reportURL & """, '', props)" & vbCrLf
            jscript &= "//-->" & vbCrLf
            jscript &= "</script>" & vbCrLf
            'pg.RegisterStartupScript("CrystalReport", jscript)
            Dim rndKey As New Random
            pg.RegisterStartupScript(rndKey.Next.ToString, jscript)
        End Sub


#End Region

    End Class
End Namespace

