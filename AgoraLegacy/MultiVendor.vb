Imports System
Imports System.Collections
Imports System.Web
Imports AgoraLegacy
Imports SSO.Component
Imports System.IO
Namespace AgoraLegacy
    Public Class MultiVendor
        Public V_com_ID As String
        Public PRNum As String
        Public Vcompany As String
        Public REG_NO As String
        Public state As String
        Public country As String
        Public buyername As String
        Public adds As String
        Public total As String
        Public Tax As String
        Public v_itemCode As String
        Public PRICE As String
        Public product_ID As String
        Public product_name As String
        Public vendor_Id As String
        Public item_desc As String
        Public user_name As String
        Public vendor_name As String
        Public vendor_Contact_no As String
        Public vendor_email As String
        Public vendor_person As String
        Public phone As String
        Public email As String
        Public con_person As String
        Public Quantity As String
        Public val As String
        Public lineno As String
        Public addsline1 As String
        Public addsline2 As String
        Public addsline3 As String
        Public postcode As String
        Public city As String
        Public type As String
    End Class
    Public Class Mulitple
        Dim objDb As New EAD.DBCom
        'Public Function Get_CompId(ByVal item As MultiVendor) As String
        '    Dim strsql As String
        '    Dim read As OleDb.OleDbDataReader
        '    Dim vid As DataSet
        '    strsql = "select distinct PRD_S_COY_ID from PR_Details where PRD_PR_NO='" & Common.Parse(item.PRNum) & "'"
        '    read = objDb.GetReader(strsql)
        '    vid = objDb.FillDs(strsql)
        '    'Viewstate("vid") = vid.Tables(0).Rows(0).Item(0)
        '    'If read.Read Then
        '    Do While read.Read
        '        item.V_com_ID = read("PRD_S_COY_ID").ToString.Trim
        '        'dim 
        '        'End If
        '        Get_CompName(item)
        '    Loop
        '    read.Close()
        'End Function
        'Public Function Get_CompName(ByVal item As MultiVendor) As String
        '    Dim read As OleDb.OleDbDataReader
        '    Dim strsql As String
        '    'Dim i As Integer
        '    'CM_FAX 
        '    strsql = "SELECT CM_COY_NAME,CM_EMAIL,CM_PHONE from COMPANY_MSTR WHERE  CM_COY_ID='" & item.V_com_ID & "'"
        '    read = objDb.GetReader(strsql)
        '    'If read.Read Then
        '    Do While read.Read
        '        item.Vcompany = read("CM_COY_NAME")
        '        item.vendor_email = read("CM_EMAIL")
        '        item.vendor_Contact_no = read("CM_PHONE")
        '        'End If
        '    Loop
        '    ' Return Get_CompId(item)
        '    read.Close()
        ''End Function
        'Public Function Get_CompAddress(ByVal item As MultiVendor) As String
        '    Dim read As OleDb.OleDbDataReader
        '    Dim strsql As String
        '    strsql = "Select PRD_D_ADDR_LINE1,PRD_D_ADDR_LINE2,PRD_D_ADDR_LINE3,PRD_D_POSTCODE,PRD_D_CITY,PRD_D_STATE,PRD_D_COUNTRY from PR_Details where PRD_PR_NO='" & Common.Parse(item.PRNum) & "'"
        '    read = objDb.GetReader(strsql)
        '    'If read.Read Then
        '    Do While read.Read
        '        item.addsline1 = read("PRD_D_ADDR_LINE1")
        '        item.addsline2 = read("PRD_D_ADDR_LINE2")
        '        item.addsline3 = read("PRD_D_ADDR_LINE3")
        '        item.postcode = read("PRD_D_POSTCODE")
        '        item.city = read("PRD_D_CITY")
        '        item.state = read("PRD_D_STATE")
        '        item.country = read("PRD_D_COUNTRY")
        '    Loop
        '    read.Close()
        '    'End If
        'End Function

    End Class

End Namespace



