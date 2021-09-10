Imports System
Imports System.Configuration
Imports System.Web
Imports System.Web.UI.WebControls
Imports AgoraLegacy
Imports SSO.Component

Namespace AgoraLegacy
    Public Class GLCodeIPP
        Dim objDb As New EAD.DBCom
        Dim strMessage As String
        Public Function GetGLCode(ByVal strGLCode As String, ByVal strDesc As String, ByVal strStatus As String) As DataSet
            Dim ds As DataSet
            Dim strsql As String


            strsql = "SELECT CBG_B_GL_CODE, CBG_B_GL_DESC, CBG_CC_REQ, CBG_AG_REQ, CBG_STATUS FROM COMPANY_B_GL_CODE WHERE CBG_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CBG_STATUS = '" & strStatus & "'"

            If strGLCode <> "" Then
                strsql &= " AND CBG_B_GL_CODE LIKE '%" & strGLCode & "%'"
            End If
            If strDesc <> "" Then
                strsql &= " AND CBG_B_GL_DESC Like '%" & strDesc & "%'"
            End If



            ds = objDb.FillDs(strsql)
            Return ds
        End Function
        Public Function GetSelectedGLCode(ByVal strGLCode As String) As DataSet
            Dim ds As DataSet
            Dim strsql As String


            strsql = "SELECT CBG_B_GL_CODE, CBG_B_GL_DESC, CBG_CC_REQ, CBG_AG_REQ, CBG_STATUS FROM COMPANY_B_GL_CODE WHERE CBG_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CBG_B_GL_CODE = '" & strGLCode & "'"

            ds = objDb.FillDs(strsql)
            Return ds
        End Function
        Public Function DelGLCode(ByVal dtGLCode As DataTable) As Boolean
            Dim i As Integer
            Dim strSQL As String

            If objDb.Exist("SELECT '*' FROM INVOICE_DETAILS WHERE ID_B_GL_CODE =  '" & Common.Parse(dtGLCode.Rows(i)("index")) & "'") > 0 Then
                Return False
                Exit Function
            End If

            For i = 0 To dtGLCode.Rows.Count - 1
                strSQL = " DELETE FROM COMPANY_B_GL_CODE WHERE CBG_B_GL_CODE = '" & Common.Parse(dtGLCode.Rows(i)("index")) & "'"

                objDb.Execute(strSQL)
            Next
            Return True

        End Function

        Public Function UpdateGLCode(ByVal dsGLCode As DataSet) As Boolean
            Dim Query(0) As String
            Dim strSQL As String

            strSQL = "UPDATE COMPANY_B_GL_CODE SET " & _
                  "CBG_B_GL_DESC='" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLDesc")) & "'," & _
                  "CBG_CC_REQ='" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLCCReq")) & "'," & _
                  "CBG_AG_REQ='" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLAGReq")) & "'," & _
                  "CBG_STATUS='" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLStatus")) & "'," & _
                  "CBG_MOD_BY='" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLModBy")) & "'," & _
                  "CBG_MOD_DATETIME=" & Common.ConvertDate(dsGLCode.Tables(0).Rows(0)("GLModDate")) & "" & _
                  " WHERE CBG_B_GL_CODE = '" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLCode")) & "'"

            objDb.Execute(strSQL)

            Return True

        End Function
        Public Function InsertGLCode(ByVal dsGLCode As DataSet) As Boolean
            Dim Query(0) As String
            Dim strSQL As String

            If objDb.Exist("SELECT '*' FROM COMPANY_B_GL_CODE WHERE CBG_B_COY_ID = '" & Common.Parse(dsGLCode.Tables(0).Rows(0)("CompID")) & "' AND CBG_B_GL_CODE =  '" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLCode")) & "'") > 0 Then
                Return False
                Exit Function
            End If

            strSQL = " INSERT INTO COMPANY_B_GL_CODE(" & _
                    "CBG_B_COY_ID,CBG_B_GL_CODE,CBG_B_GL_DESC," & _
                    "CBG_CC_REQ,CBG_AG_REQ,CBG_STATUS," & _
                    "CBG_ENT_BY,CBG_ENT_DATETIME) " & _
                    "VALUES('" & Common.Parse(dsGLCode.Tables(0).Rows(0)("CompID")) & "','" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLCode")) & "','" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLDesc")) & "'," & _
                    "'" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLCCReq")) & "','" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLAGReq")) & "','" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLStatus")) & "'," & _
                     "'" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLEntBy")) & "'," & Common.ConvertDate(dsGLCode.Tables(0).Rows(0)("GLEntDate")) & ")"


            objDb.Execute(strSQL)
            Return True

        End Function
      
        Public Function Message(ByVal pg As System.Web.UI.Page, ByVal MsgID As String, Optional ByVal style As MsgBoxStyle = MsgBoxStyle.Exclamation, Optional ByVal title As String = "Invoice Payment")
            Dim strSQL As String
            Dim strMsg As String

            strSQL = "SELECT MM_MESSAGE FROM MESSAGE_MSTR WHERE MM_CODE = '" & MsgID & "'"
            strMsg = objDb.GetVal(strSQL)

            Dim vbs As String
            vbs = vbs & "<script language=""vbs"">"
            vbs = vbs & "Call MsgBox(""" & strMsg & """, " & style & ", """ & title & """)"
            vbs = vbs & "</script>"
            Dim rndKey As New Random
            pg.RegisterStartupScript(rndKey.Next.ToString, vbs)

        End Function
    End Class
End Namespace