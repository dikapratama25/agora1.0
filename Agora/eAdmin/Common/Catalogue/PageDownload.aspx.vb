Imports System.IO

Imports System.Text.RegularExpressions
Imports System.Data.OleDb
Imports AgoraLegacy
Public Class PageDownload
    Inherits AgoraLegacy.AppBaseClass

    Dim objDb As New  EAD.DBCom
    Protected WithEvents cmdDownload As System.Web.UI.WebControls.Button
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Dim objdownload As New AppExcel

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents lblPath As System.Web.UI.WebControls.Label
    Protected WithEvents cmdBrowse As System.Web.UI.HtmlControls.HtmlInputFile

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'MyBase.Page_Load(sender, e)
        'Session("CompanyIdToken") = "demo"
        'Session("UserId") = "hoikk"
        Session("CompanyIdToken") = "TCAE"
    End Sub

    Private Sub cmdDownload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDownload.Click
        Dim pro As New Products
        Dim ds As New DataSet
        Dim objEx As New AppExcel
        Dim cRules As New myCollection

        ds = pro.Download_ProductExcel(Session("CompanyIdToken"))
        'Writecell(ds, Server.MapPath("../Template/ProductListLing.xls"))
        '///ACTUAL
        objEx.Writecell(ds, Server.MapPath("../Template/ProductListLing.xls"))
        'TestingSQL()
        '///PREVIOUS
        'objEx.WriteDsToExcelUseLooping(ds, Server.MapPath("../Template/ProductListLing.xls"))
        Filedownload()
    End Sub

    Function Filedownload()
        Dim strActualFile As String = "ProductList.xls"
        Response.ContentType = "application/octet-stream"
        Response.AddHeader("Content-Disposition", _
          "attachment; filename=""" & strActualFile & """")
        Response.Flush()
        Response.WriteFile(Server.MapPath("../Template/ProductListLing.xls"))
    End Function
    '//CURRENT FUNCTION INSERT BY CELL RANGE
    Function Writecell(ByVal ds As DataSet, ByVal pPath As String)

        Dim ctx As Web.HttpContext = Web.HttpContext.Current

        Dim strSourceFile As String = ctx.Server.MapPath(ctx.Request.ApplicationPath) & "\Template\ProductList.xls"
        Dim strDestFile As String = ctx.Server.MapPath(ctx.Request.ApplicationPath) & "\Template\ProductListLing.xls"
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
                lsSql = "Insert into [Sheet1$A" & intLoop & ":P" & intLoop & "] Values ("
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
            objDb.BatchExecute(SqlAry)
            objDb = Nothing
        Else
            Exit Function
        End If

    End Function
    '//PREVIOUS FUNCTION INSERT BY FIELD NAME
    Public Sub WriteDsToExcelUseLooping(ByVal ds As DataSet, ByVal pPath As String)
        Dim ctx As Web.HttpContext = Web.HttpContext.Current



        OpenConnToExcel(pPath, True)

        Dim SqlAry(0) As String
        Dim lsSql As String
        Dim intLoop, intLoop1, intTotRow, intTotCol, TotalRow As Integer

        TotalRow = ds.Tables(0).Rows.Count
        intTotRow = ds.Tables(0).Rows.Count - 1
        intTotCol = ds.Tables(0).Columns.Count

        '//AVOID Excel file kenot open and Company Id not exist
        If TotalRow > 0 Then
            '//
            For intLoop = 0 To intTotRow
                lsSql = "Insert into [Sheet1$] (Row_No, Item_ID, Company_Id, Item_Desc, UNSPSC_Code, UNSPSC_Desc, Unit_Cost, Currency_Code, UOM, Tax_Code, Mgmt_Code, Mgmt_Text, Vendor_Item_Code, Brand, Model, Actions) Values ("
                lsSql = lsSql & "'" & (intLoop + 1) & "',"
                For intLoop1 = 0 To intTotCol - 1
                    lsSql = lsSql & "'" & ds.Tables(0).Rows(intLoop)(intLoop1) & "',"
                Next
                lsSql = lsSql & "'M')"
                Common.Insert2Ary(SqlAry, lsSql)
            Next

            objDb.BatchExecute(SqlAry)
            objDb = Nothing
        Else
            Exit Sub
        End If
    End Sub
    Public Function OpenConnToExcel(ByVal pFilePath As String, Optional ByVal pHeader As Boolean = False) As Boolean
        Dim strMassage As String
        Dim sConn As String
        Try
            If Right(pFilePath, 3) <> "xls" Then
                strMassage = "Target file is expecting excel file format."
                Return False
            End If

            sConn = " Provider=Microsoft.Jet.OLEDB.4.0;" & _
                                 " Data Source=" & pFilePath & ";" & _
                                 " Extended Properties=""Excel 8.0;HDR=NO"""

            If pHeader Then sConn = Replace(sConn, "HDR=NO", "HDR=YES")
            objDb.gstrConn = sConn

            If objDb.ConnState = False Then
                strMassage = "Connection Failed."
            Else
                Return True
            End If
        Catch exp As Exception
            Common.TrwExp(exp, sConn)
        Finally
            '//THIS MUST..SO THE EXCEL FILE CAN OPEN IN ANY CASE
            objDb.DisConn()
            '//
            ' objDcom.DisConn()

        End Try
    End Function


    Function TestingSQL()
        ''////THIS ONE CAN USE ONE AS TESTING


        ''"Data Source=C:\Book3.xls;" & _
        Dim m_sConn1 As String = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
        "Data Source=C:\ProductListLing.xls;" & _
        "Extended Properties=""Excel 8.0;HDR=YES"""
        Dim conn As New OleDbConnection
        conn.ConnectionString = m_sConn1
        conn.Open()
        Dim cmd1 As New OleDbCommand
        cmd1.Connection = conn
        'cmd1.CommandText = "INSERT INTO [Sheet1$] (A, B, C, D) values ('PAD3', 'HP21', 'NEW3','N4')"
        'cmd1.CommandText = "INSERT INTO [Sheet1$] (RowNo, ItemID, CompanyId, ItemDesc., UNSPSCCode, UNSPSCDesc, UnitCost, CurrencyCode, UOM, TaxCode, Mgmt_Code, MgmtText, VendorItemCode, Brand, Model, Action) values ('a','a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'n')"
        'cmd1.CommandText = "Insert into [Sheet1$] (Row_No, Item_ID, Company_Id, Item_Desc, UNSPSC_Code, UNSPSC_Desc, Unit_Cost, Currency_Code, UOM, Tax_Code, Mgmt_Code, Mgmt_Text, Vendor_Item_Code, Brand, Model, Actions) Values ('3','TX5299','CAM','TOOL GENERAL AND MACHINERY: MINISET HANDRILL','27111515','Hand or push drill','250','MYR','SET','NR','','a','CIM029','a','21J-KR13A2','M')"
        'cmd1.CommandText = "INSERT INTO [Sheet1$B" & intLoop & ":P" & intLoop & ":] values ('a', 'b')"
        cmd1.CommandText = "INSERT INTO [Sheet1$B3:G3] values ('a#', 'b#', 'c#', 'd#', 'e#', '123.00#')"

        cmd1.ExecuteNonQuery()
        conn.Close()
        '//////
    End Function

End Class
