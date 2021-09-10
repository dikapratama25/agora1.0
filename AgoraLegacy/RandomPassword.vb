Imports System
Imports System.Security.Cryptography
Imports AgoraLegacy
Imports SSO.Component

Namespace AgoraLegacy
    Public Class RandomPassword

        ' Define default min and max password lengths.
        Private Shared DEFAULT_MIN_PASSWORD_LENGTH As Integer = 8
        Private Shared DEFAULT_MAX_PASSWORD_LENGTH As Integer = 10

        ' Define supported password characters divided into groups.
        Private Shared PASSWORD_CHARS_LCASE As String = "abcdefgijkmnopqrstwxyz"
        Private Shared PASSWORD_CHARS_UCASE As String = "ABCDEFGHJKLMNPQRSTWXYZ"
        Private Shared PASSWORD_CHARS_NUMERIC As String = "23456789"
        'Private Shared PASSWORD_CHARS_SPECIAL As String = "&_.()"

        'Name       : GenerateAutoPwd
        'Author     : kk
        'Descption  : genatrate auto password base on MIN length in wheel policy setup
        'LastUpadte : 24 Nov 2004
        Public Function GenerateAutoPwd()
            Dim objLPolicy As New LoginPolicy
            Dim strNewPwd As String
            Dim iPwdLen As Integer

            iPwdLen = objLPolicy.getParamValue("PASSWORD_LENGTH_MIN")
            If iPwdLen > 0 Then
                strNewPwd = Generate(iPwdLen)
            Else
                strNewPwd = Generate(6)
            End If

            ' to prevent generate password with single quot
            strNewPwd = strNewPwd.Replace("'", "a")
            Return strNewPwd

        End Function

        Private Shared Function Generate() As String
            Generate = Generate(DEFAULT_MIN_PASSWORD_LENGTH, _
                                DEFAULT_MAX_PASSWORD_LENGTH)
        End Function

        Private Shared Function Generate(ByVal length As Integer) As String
            Generate = Generate(length, length)
        End Function

        Private Shared Function Generate(ByVal minLength As Integer, _
        ByVal maxLength As Integer) _
            As String

            ' Make sure that input parameters are valid.
            If (minLength <= 0 Or maxLength <= 0 Or minLength > maxLength) Then
                Generate = Nothing
            End If

            ' Create a local array containing supported password characters
            ' grouped by types.
            Dim charGroups As Char()() = New Char()() _
            { _
                PASSWORD_CHARS_LCASE.ToCharArray(), _
                PASSWORD_CHARS_UCASE.ToCharArray(), _
                PASSWORD_CHARS_NUMERIC.ToCharArray()}

            ' Use this array to track the number of unused characters in each
            ' character group.
            Dim charsLeftInGroup As Integer() = New Integer(charGroups.Length - 1) {}

            ' Initially, all characters in each group are not used.
            Dim I As Integer
            For I = 0 To charsLeftInGroup.Length - 1
                charsLeftInGroup(I) = charGroups(I).Length
            Next

            ' Use this array to track (iterate through) unused character groups.
            Dim leftGroupsOrder As Integer() = New Integer(charGroups.Length - 1) {}

            ' Initially, all character groups are not used.
            For I = 0 To leftGroupsOrder.Length - 1
                leftGroupsOrder(I) = I
            Next

            ' Because we cannot use the default randomizer, which is based on the
            ' current time (it will produce the same "random" number within a
            ' second), we will use a random number generator to seed the
            ' randomizer.

            ' Use a 4-byte array to fill it with random bytes and convert it then
            ' to an integer value.
            Dim randomBytes As Byte() = New Byte(3) {}

            ' Generate 4 random bytes.
            Dim rng As RNGCryptoServiceProvider = New RNGCryptoServiceProvider

            rng.GetBytes(randomBytes)

            ' Convert 4 bytes into a 32-bit integer value.
            Dim seed As Integer = ((randomBytes(0) And &H7F) << 24 Or _
                                    randomBytes(1) << 16 Or _
                                    randomBytes(2) << 8 Or _
                                    randomBytes(3))

            ' Now, this is real randomization.
            Dim random As random = New random(seed)

            ' This array will hold password characters.
            Dim password As Char() = Nothing

            ' Allocate appropriate memory for the password.
            If (minLength < maxLength) Then
                password = New Char(random.Next(minLength - 1, maxLength)) {}
            Else
                password = New Char(minLength - 1) {}
            End If

            ' Index of the next character to be added to password.
            Dim nextCharIdx As Integer

            ' Index of the next character group to be processed.
            Dim nextGroupIdx As Integer

            ' Index which will be used to track not processed character groups.
            Dim nextLeftGroupsOrderIdx As Integer

            ' Index of the last non-processed character in a group.
            Dim lastCharIdx As Integer

            ' Index of the last non-processed group. Initially, we will skip
            ' special characters.
            Dim lastLeftGroupsOrderIdx As Integer = leftGroupsOrder.Length - 1

            ' Generate password characters one at a time.
            For I = 0 To password.Length - 1

                ' If only one character group remained unprocessed, process it;
                ' otherwise, pick a random character group from the unprocessed
                ' group list.
                If (lastLeftGroupsOrderIdx = 0) Then
                    nextLeftGroupsOrderIdx = 0
                Else
                    nextLeftGroupsOrderIdx = random.Next(0, lastLeftGroupsOrderIdx)
                End If

                ' Get the actual index of the character group, from which we will
                ' pick the next character.
                nextGroupIdx = leftGroupsOrder(nextLeftGroupsOrderIdx)

                ' Get the index of the last unprocessed characters in this group.
                lastCharIdx = charsLeftInGroup(nextGroupIdx) - 1

                ' If only one unprocessed character is left, pick it; otherwise,
                ' get a random character from the unused character list.
                If (lastCharIdx = 0) Then
                    nextCharIdx = 0
                Else
                    nextCharIdx = random.Next(0, lastCharIdx + 1)
                End If

                ' Add this character to the password.
                password(I) = charGroups(nextGroupIdx)(nextCharIdx)

                ' If we processed the last character in this group, start over.
                If (lastCharIdx = 0) Then
                    charsLeftInGroup(nextGroupIdx) = _
                                    charGroups(nextGroupIdx).Length
                    ' There are more unprocessed characters left.
                Else
                    ' Swap processed character with the last unprocessed character
                    ' so that we don't pick it until we process all characters in
                    ' this group.
                    If (lastCharIdx <> nextCharIdx) Then
                        Dim temp As Char = charGroups(nextGroupIdx)(lastCharIdx)
                        charGroups(nextGroupIdx)(lastCharIdx) = _
                                    charGroups(nextGroupIdx)(nextCharIdx)
                        charGroups(nextGroupIdx)(nextCharIdx) = temp
                    End If

                    ' Decrement the number of unprocessed characters in
                    ' this group.
                    charsLeftInGroup(nextGroupIdx) = _
                               charsLeftInGroup(nextGroupIdx) - 1
                End If

                ' If we processed the last group, start all over.
                If (lastLeftGroupsOrderIdx = 0) Then
                    lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1
                    ' There are more unprocessed groups left.
                Else
                    ' Swap processed group with the last unprocessed group
                    ' so that we don't pick it until we process all groups.
                    If (lastLeftGroupsOrderIdx <> nextLeftGroupsOrderIdx) Then
                        Dim temp As Integer = _
                                    leftGroupsOrder(lastLeftGroupsOrderIdx)
                        leftGroupsOrder(lastLeftGroupsOrderIdx) = _
                                    leftGroupsOrder(nextLeftGroupsOrderIdx)
                        leftGroupsOrder(nextLeftGroupsOrderIdx) = temp
                    End If

                    ' Decrement the number of unprocessed groups.
                    lastLeftGroupsOrderIdx = lastLeftGroupsOrderIdx - 1
                End If
            Next

            ' Convert password characters into a string and return the result.
            Generate = New String(password)
        End Function

    End Class
End Namespace





