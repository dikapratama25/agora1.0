Imports System.text
Imports AgoraLegacy
Imports SSO.Component
'Imports Quiksoft.FreeSMTP
Module UI


    'Name       : DdlChkCnt
    'Author     : Kk
    'Descption  : Return number of checked check box in a dg
    'LastUpadte : 22 Nov 2004
    Public Function DdlChkCnt(ByRef pDg As DataGrid, ByVal pChkName As String) As Int32
        Dim dgItem As DataGridItem
        Dim chk As CheckBox
        Dim strlistindex As String
        Dim i As Integer

        For Each dgItem In pDg.Items
            chk = dgItem.FindControl(pChkName)
            If chk.Checked Then
                i = i + 1
            End If
        Next
        DdlChkCnt = i
    End Function

    'Name       : DdlChkCnt
    'Author     : Kk
    'Descption  : Return number of checked check box in a dg
    'LastUpadte : 22 Nov 2004
    Public Function GetChkBoxID(ByRef pDg As DataGrid, ByVal pChkName As String, ByVal iCell As Integer) As String
        Dim dgItem As DataGridItem
        Dim chk As CheckBox
        Dim strlistindex As String
        Dim i As Integer

        For Each dgItem In pDg.Items
            chk = dgItem.FindControl(pChkName)
            If chk.Checked Then
                Return dgItem.Cells(iCell).Text
            End If
        Next
        Return ""
    End Function

   

    'Name       : GetPwdExpireDt
    'Author     : kk
    'Descption  : Get Pwd Expire Date
    'Remark     : 
    'ReturnValue: Get next logon expire date 
    'LastUpadte : 22 Nox 2004
    'Version    : 1.00
    Private Function GetPwdExpireDt() As String
        Dim iNewPwdSpan As Integer
        Dim objLPolicy As New LoginPolicy

        iNewPwdSpan = objLPolicy.getParamValue("NEW_PASSWORD_EXPIRING")
        If iNewPwdSpan = Nothing Then
            iNewPwdSpan = 7
        End If
        Return DateAdd(DateInterval.Day, iNewPwdSpan, Today)
    End Function

    'Public Function SendMail(ByVal pRcvEmail As String, ByVal pMsg As String, ByVal pSubject As String) As Boolean

    '    'Create the EmailMessage object
    '    Dim msgObj As New EmailMessage

    '    'Specify from address and display name
    '    msgObj.From.Email = "eBiz@kompakar.com"
    '    msgObj.From.Name = "Kompkar eBiz"

    '    'Add a normal recipient
    '    msgObj.Recipients.Add(pRcvEmail)

    '    'Specify the subject
    '    msgObj.Subject = pSubject

    '    'Add an HTML body part
    '    msgObj.BodyParts.Add(pMsg, BodyPartFormat.HTML)

    '    'Add a text body part to server as alternative text for non HTML mail readers
    '    msgObj.BodyParts.Add("Message body.", BodyPartFormat.Plain)

    '    'Add an attachment
    '    'msgObj.Attachments.Add("c:\attachment.txt")

    '    'Create the SMTP object using the constructor to specify the mail server
    '    Dim smtpObj As New SMTP("SMTP.kompakar.com.my")

    '    'Send the message
    '    smtpObj.Send(msgObj)



    'End Function

    Public Function GetMailFormat(ByVal pUserID As String, ByVal pPwd As String, ByVal pExp As String) As String
        Dim str As New StringBuilder
        str.Append("<HTML><BODY>")
        str.Append("<TABLE WIDTH=100%><TR><TD>")
        str.Append("<TABLE ALIGN=LEFT CELLPADDING=3 CELLSPACING=1 STYLE=""BORDER-RIGHT: BLACK THIN SOLID; BORDER-TOP: BLACK 1PX SOLID; BORDER-LEFT: BLACK 1PX SOLID; BORDER-BOTTOM: BLACK THIN SOLID"">")
        str.Append("<TR BGCOLOR=#3399FF><TD COLSPAN=3><FONT FACE=VERDANA COLOR=WHITE SIZE=2><B>SINGLE SIGN-ON INFORMATION</B></FONT></TD></TR>")
        str.Append("<TR BGCOLOR=ALICEBLUE><TD WIDTH=120><FONT FACE=VERDANA SIZE=1><B>SINGLE SIGN-ON ID</B></FONT></TD><TD WIDTH=5>:</TD><TD WIDTH=150><FONT FACE=VERDANA SIZE=1><B>" & pUserID & "</B></FONT></TD></TR>")
        str.Append("<TR BGCOLOR=ALICEBLUE><TD><FONT FACE=VERDANA SIZE=1><B>NEW PASSWORD</B></FONT></TD><TD>:</TD><TD><FONT FACE=VERDANA SIZE=1><B>" & pPwd & "</B></FONT></TD></TR>")
        str.Append("<TR BGCOLOR=ALICEBLUE><TD><FONT FACE=VERDANA SIZE=1><B>EXPIRE DATE</B></FONT></TD><TD>:</TD><TD><FONT FACE=VERDANA SIZE=1><B>" & pExp & "</B></FONT></TD></TR>")
        str.Append("</TABLE></TD></TR>")
        str.Append("<TR><TD><FONT FACE=VERDANA SIZE=1>NOTE: THIS IS A SYSTEM GENERATED EMAIL, PLEASE DO NOT REPLY TO THIS EMAIL.</FONT></TD></TR>")
        str.Append("</HTML></BODY>")

        Return str.ToString
    End Function

    'Name       : IsAlphaNum
    'Author     : Kaithim
    'Descption  : Validation check for AphlaNumeric
    'Remark     :
    'ReturnValue: True for alphanumeric
    'LastUpadte : 19 Sep 2002
    'Version    : 1.00
    Public Function IsAlphaNum(ByVal pCheck As String) As Boolean
        IsAlphaNum = True
        Dim i As Long, j As Long
        If Len(pCheck) <> 0 Then

            Dim strCheck As String = "0123456789"
            strCheck = strCheck & " " & "abcdefghijklmnopqrstuvwxyz"
            strCheck = strCheck & "ABCEDFGHIJKLMNOPQRSTUVWXYZ" + "&_.'()"

            Dim ch As Char
            For i = 1 To Len(pCheck)
                ch = Mid(pCheck, i, 1)
                For j = 1 To Len(strCheck)
                    If ch = Mid(strCheck, j, 1) Then Exit For
                Next
                If j = (Len(strCheck)) + 1 Then
                    IsAlphaNum = False
                    Exit Function
                End If
            Next
        End If
    End Function

    'Name       : IsTel
    'Author     : SiangNee
    'Descption  : Validation check for Numeric
    'Remark     :
    'ReturnValue: True for cost
    'LastUpadte : 24 Oct 2002
    'Version    : 1.00
    'Regex      :^\d{2,3}-\d{7,10}$ 
    Public Function IsTel(ByVal pCheck As String) As Boolean
        If Not System.Text.RegularExpressions.Regex.IsMatch(pCheck, "^\d{2,3}-\d{7,10}$") Then
            Return False
        End If
        Return True
    End Function

    

    'Name       : IsTel
    'Author     : SiangNee
    'Descption  : Validation check for Numeric
    'Remark     :
    'ReturnValue: True for cost
    'LastUpadte : 24 Oct 2002
    'Version    : 1.00
    'Regex      :^\d{2,3}-\d{7,10}$ 
    Public Function IsFax(ByVal pCheck As String) As Boolean

        If Not System.Text.RegularExpressions.Regex.IsMatch(pCheck, "^\d{2}-\d{7,10}$") Then
            Return False
        End If
        Return True
    End Function

    'Name       : IsCreditCard
    'Author     : kk
    'Descption  : Validation for CreditCard
    'Remark     :
    'ReturnValue: Boolean
    'LastUpadte : 26 Nov 2004
    'Version    : 1.00
    Public Function IsCreditCard(ByVal pCheck As String) As Boolean
        Dim strRegex As String
        strRegex = "^\d{4}((-\d{4}){3}$)|((\d{4}){3}$"
        If Not System.Text.RegularExpressions.Regex.IsMatch(pCheck, strRegex) Then
            Return False
        End If
        Return True
    End Function

    'Name       : IsPercent
    'Author     : kk
    'Descption  : Validation for %
    'Remark     :
    'ReturnValue: Boolean
    'LastUpadte : 26 Nov 2004
    'Version    : 1.00
    Public Function IsPercent(ByVal pCheck As String) As Boolean
        Dim strRegex As String
        strRegex = "^(0*100{1,1}\.?((?<=\.)0*)?%?$)|(^0*\d{0,2}\.?((?<=\.)\d*)?%?)$"
        If Not System.Text.RegularExpressions.Regex.IsMatch(pCheck, strRegex) Then
            Return False
        End If
        Return True
    End Function

    'Name       : IsEmail
    'Author     : kk
    'Descption  : Validation check for email without chk error type
    'Remark     :
    'ReturnValue: Boolean
    'LastUpadte : 26 Nov 2004
    'Version    : 1.00
    Public Function IsEmail(ByVal pCheck As String) As Boolean
        Dim strRegex As String
        strRegex = "^([a-zA-Z0-9_\-])([a-zA-Z0-9_\-\.]*)@(\[((25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\.){3}|((([a-zA-Z0-9\-]+)\.)+))([a-zA-Z]{2,}|(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\])$"
        If Not System.Text.RegularExpressions.Regex.IsMatch(pCheck, strRegex) Then
            Return False
        End If
        Return True
    End Function

    'Name       : IsAlphaNum
    'Author     : kk
    'Descption  : Email validation
    'Remark     :
    'ReturnValue: Boolean
    'LastUpadte : 25 Nov 2004
    'Version    : 1.00
    Public Function IsEmail(ByRef pErrMsg As String, ByVal Value As String, Optional ByVal MaxLength As Integer = 160) As Boolean

        If Value.Length > MaxLength Then
            pErrMsg &= "Email 's maximun length is 160.<br>"
            Return False
        End If

        If Not System.Text.RegularExpressions.Regex.IsMatch(Value, "^[-A-Za-z0-9_@.]+$") Then
            pErrMsg &= "Invalid character in the e-mail address."
            Return False
        End If

        ' Look @
        Dim i As Integer = Value.IndexOf("@"c)
        ' there must be at least three chars after the @
        If i <= 0 Or i >= Value.Length - 3 Then
            pErrMsg &= "Invalid e-mail by missing domain."
            Exit Function
        End If
        ' Ensure only one @
        If Value.IndexOf("@"c, i + 1) >= 0 Then
            pErrMsg &= "Invalid e-mail by '@' appear twice."
            Exit Function
        End If
        ' check that the domain portion contains at least one dot
        Dim j As Integer = Value.LastIndexOf("."c)
        ' it can't be before or immediately after the @ char
        If j < 0 Or j <= i + 1 Then
            pErrMsg &= " Invalid e-mail by missing '.'."
            Exit Function
        End If

        Return True
    End Function

    'Name       : IsCost
    'Author     : SiangNee
    'Descption  : Validation check for Cost
    'Remark     :
    'ReturnValue: True for cost
    'LastUpadte : 24 Oct 2002
    'Version    : 1.00
    Public Function IsCost(ByVal pCheck As String) As Boolean
        IsCost = True
        Dim i As Long, j As Long
        If Len(pCheck) <> 0 Then

            Dim strCheck As String = "0123456789"
            strCheck = strCheck & "."

            Dim ch As Char
            For i = 1 To Len(pCheck)
                ch = Mid(pCheck, i, 1)
                For j = 1 To Len(strCheck)
                    If ch = Mid(strCheck, j, 1) Then Exit For
                Next
                If j = (Len(strCheck)) + 1 Then
                    IsCost = False
                    Exit Function
                End If
            Next
        End If
    End Function

    'Name       : Chl2String
    'Author     : KK
    'Descption  : Convert Selected item from Checkbox List to String   
    'Remark     :
    'ReturnValue: String "vv,zz,xx,yy"
    'LastUpadte : 19 Sep 2004
    'Version    : 1.00
    Public Function Lst2String(ByRef pList As ListBox, Optional ByVal pSelectedOnly As Boolean = True, Optional ByVal pText As Boolean = False) As String
        Dim strReturn As New StringBuilder("")
        Dim varItem
        For Each varItem In pList.Items
            If pSelectedOnly Then
                If varItem.selected Then
                    If pText = False Then strReturn.Append(varItem.value) Else strReturn.Append(varItem.text)
                    strReturn.Append(",")
                End If
            Else
                If pText = False Then strReturn.Append(varItem.value) Else strReturn.Append(varItem.text)
                strReturn.Append(",")
            End If
        Next
        If strReturn.ToString <> "" Then
            Return Mid(strReturn.ToString, 1, Len(strReturn.ToString) - 1)
        Else
            Return Nothing
        End If
    End Function



End Module
