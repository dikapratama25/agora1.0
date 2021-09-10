Imports System
Imports System.Configuration
Imports System.Text
Imports System.Web
Imports System.Collections
Imports AgoraLegacy
Imports SSO.Component
Imports System.IO

Namespace AgoraLegacy
    Public Class PurchaseOrder_Vendor
        Dim objDb As New EAD.DBCom
        Dim v_com_id As String = HttpContext.Current.Session("CompanyId")
        Dim userid As String = HttpContext.Current.Session("UserId")

        Public Function getPOForAck() As DataSet
            Dim ds As DataSet
            Dim strsql As String

            strsql = "SELECT POM.POM_S_COY_ID, POM.POM_PO_INDEX,POM.POM_PO_NO,POM.POM_PO_DATE, " & _
            "PCM_CR_NO AS CR_NO, " & _
            "POM.POM_PO_STATUS, CM.CM_COY_NAME,POM.POM_BUYER_NAME,POM.POM_S_COY_NAME,POM.POM_ACCEPTED_DATE,POM.POM_B_COY_ID,POM.POM_FULFILMENT , " & _
            "(SELECT STATUS_DESC FROM STATUS_MSTR C WHERE STATUS_TYPE ='PO' AND C.STATUS_NO=POM.POM_PO_STATUS) " & _
            "AS STATUS_DESC, POM.POM_URGENT FROM PO_MSTR POM INNER JOIN COMPANY_MSTR CM ON POM.POM_B_COY_ID = CM.CM_COY_ID " & _
            "LEFT OUTER JOIN PO_CR_MSTR ON PCM_PO_INDEX = POM.POM_PO_INDEX " & _
            "WHERE (POM.POM_PO_STATUS IN('1','2') OR (POM.POM_PO_STATUS = '5' AND POM.POM_FULFILMENT = '4') OR (POM.POM_PO_STATUS = '3' AND POM.POM_FULFILMENT = '4')) " & _
            "AND POM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' ORDER BY POM.POM_PO_DATE "

            ds = objDb.FillDs(strsql)
            Return ds

        End Function

        Public Function get_comid(ByVal po_index As String) As String
            Dim strsql As String = "select POM_B_COY_ID from PO_MSTR WHERE POM_PO_INDEX= '" & Common.Parse(po_index) & "'"
            get_comid = objDb.GetVal(strsql)
        End Function

        Public Function get_po_StatusNo(ByVal po_id As String) As String
            Dim strsql As String = "select STATUS_NO from STATUS_MSTR,PO_MSTR WHERE STATUS_TYPE='PO' AND STATUS_NO=POM_PO_STATUS and POM_PO_INDEX= '" & Common.Parse(po_id) & "' "
            Return objDb.GetVal(strsql)
        End Function

        Public Function update_POStatus(ByVal ds As DataSet) As String
            Dim strSql As String
            Dim date2day As String = Date.Now
            Dim remark As String
            Dim strarray(0), strMsg As String
            Dim objBCM As New BudgetControl
            Dim objMail As New Email
            Dim objUsers As New Users
            Dim i As Integer
            Dim strPOStatus As String
            Dim fulfilopen As Integer = Fulfilment.Open

            For i = 0 To ds.Tables(0).Rows.Count - 1
                strSql = "SELECT ISNULL(POM_PO_STATUS,1) FROM PO_MSTR WHERE POM_PO_NO= '" & Common.Parse(ds.Tables(0).Rows(i)("datakey")) & "' AND POM_S_COY_ID= '" & Common.Parse(v_com_id) & "' AND POM_B_COY_ID= '" & Common.Parse(ds.Tables(0).Rows(i)("BCoyID")) & "'"
                strPOStatus = objDb.GetVal(strSql)

                If ds.Tables(0).Rows(i)("status") = POStatus_new.Accepted Or ds.Tables(0).Rows(i)("status") = POStatus_new.Rejected Then
                    If CInt(strPOStatus) = POStatus_new.Accepted Then
                        strMsg = "You have already accepted this PO."
                    ElseIf CInt(strPOStatus) = POStatus_new.Rejected Then
                        strMsg = "You have already rejected this PO."
                    ElseIf CInt(strPOStatus) = POStatus_new.Cancelled Or CInt(strPOStatus) = POStatus_new.CancelledBy Then
                        strMsg = "This PO already cancelled."
                    End If
                End If

                If strMsg <> "" Then Return strMsg
                remark = ds.Tables(0).Rows(i)("remark")
                If remark = "" Then
                    If ds.Tables(0).Rows(i)("status") <> POStatus_new.Accepted Then
                        strSql = "update PO_MSTR SET POM_PO_STATUS= '" & Common.Parse(ds.Tables(0).Rows(i)("status")) & "',POM_STATUS_CHANGED_ON=" & Common.ConvertDate(date2day) & ",POM_STATUS_CHANGED_BY= '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' "
                    Else
                        strSql = "update PO_MSTR SET POM_PO_STATUS= '" & Common.Parse(ds.Tables(0).Rows(i)("status")) & "',POM_STATUS_CHANGED_ON=" & Common.ConvertDate(date2day) & ",POM_ACCEPTED_DATE= " & Common.ConvertDate(date2day) & " ,POM_STATUS_CHANGED_BY= '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' , POM_FULFILMENT= '" & Common.Parse(fulfilopen) & "'"
                    End If

                Else
                    If ds.Tables(0).Rows(i)("status") <> POStatus_new.Accepted Then
                        strSql = "update PO_MSTR SET POM_PO_STATUS= '" & Common.Parse(ds.Tables(0).Rows(i)("status")) & "',POM_STATUS_CHANGED_ON=" & Common.ConvertDate(date2day) & ",POM_S_REMARK= '" & Common.Parse(remark) & "',POM_ACCEPTED_DATE= " & Common.ConvertDate(date2day) & " ,POM_STATUS_CHANGED_BY= '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' "

                    Else
                        strSql = "update PO_MSTR SET POM_PO_STATUS= '" & Common.Parse(ds.Tables(0).Rows(i)("status")) & "',POM_STATUS_CHANGED_ON=" & Common.ConvertDate(date2day) & " " & _
                        " ,POM_S_REMARK= '" & Common.Parse(remark) & "',POM_ACCEPTED_DATE= " & Common.ConvertDate(date2day) & " ,POM_STATUS_CHANGED_BY= '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' , POM_FULFILMENT= '" & Common.Parse(fulfilopen) & "'"
                    End If

                End If
                strSql = strSql & " WHERE POM_PO_NO= '" & Common.Parse(ds.Tables(0).Rows(i)("datakey")) & "' and POM_S_COY_ID= '" & Common.Parse(v_com_id) & "' AND POM_B_COY_ID= '" & Common.Parse(ds.Tables(0).Rows(i)("BCoyID")) & "'"
                Common.Insert2Ary(strarray, strSql)

                '//ADD BY MOO,
                If ds.Tables(0).Rows(i)("status") = POStatus_new.Accepted Then
                    objBCM.BCMCalc("PO", ds.Tables(0).Rows(i)("datakey"), EnumBCMAction.AcceptPO, strarray, ds.Tables(0).Rows(i)("BCoyID"))
                    '//for audit trail
                    objUsers.Log_UserActivity(strarray, WheelModule.OrderMgnt, WheelUserActivity.V_AcceptPO, ds.Tables(0).Rows(i)("datakey"))
                ElseIf ds.Tables(0).Rows(i)("status") = POStatus_new.Rejected Then
                    objBCM.BCMCalc("PO", ds.Tables(0).Rows(i)("datakey"), EnumBCMAction.RejectPO, strarray, ds.Tables(0).Rows(i)("BCoyID"))
                    objUsers.Log_UserActivity(strarray, WheelModule.OrderMgnt, WheelUserActivity.V_RejectPO, ds.Tables(0).Rows(i)("datakey"))
                End If
            Next

            If objDb.BatchExecute(strarray) Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    If ds.Tables(0).Rows(i)("status") = POStatus_new.Accepted Then
                        Dim objPR As New PR
                        Dim strName As String
                        strName = objPR.getRequestorName("PO", ds.Tables(0).Rows(i)("datakey"), ds.Tables(0).Rows(i)("BCoyID"))

                        objMail.sendNotification(EmailType.POAccepted, "", ds.Tables(0).Rows(i)("BCoyID"), v_com_id, ds.Tables(0).Rows(i)("datakey"), strName)
                        strMsg = "PO Number " & Common.Parse(ds.Tables(0).Rows(i)("datakey")) & " has been accepted."
                        objPR = Nothing
                    ElseIf ds.Tables(0).Rows(i)("status") = POStatus_new.Rejected Then
                        objMail.sendNotification(EmailType.PORejected, "", ds.Tables(0).Rows(i)("BCoyID"), v_com_id, ds.Tables(0).Rows(i)("datakey"), "")
                        strMsg = "PO Number " & Common.Parse(ds.Tables(0).Rows(i)("datakey")) & " has been rejected."
                    End If
                Next
            Else
                strMsg = "Error occurs. Kindly contact the Administrator to resolve this."
            End If
            objBCM = Nothing
            objMail = Nothing
            objUsers = Nothing
            Return strMsg
        End Function

        Public Function update_ack(ByVal DS As DataSet, Optional ByVal blnEnterpriseVersion As Boolean = True) As String
            Dim strsql As String
            Dim I As Integer
            Dim strarray(0) As String
            Dim objDO As New DeliveryOrder
            For I = 0 To DS.Tables(0).Rows.Count - 1
                strsql = "update PO_CR_MSTR set PCM_CR_STATUS= '" & Common.Parse(DS.Tables(0).Rows(I)("CRStatus")) & "' " & _
                        " where PCM_CR_NO= '" & Common.Parse(DS.Tables(0).Rows(I)("cr_num")) & "' and PCM_S_COY_ID = '" & Common.Parse(v_com_id) & "' " & _
                        " and PCM_B_COY_ID= '" & Common.Parse(DS.Tables(0).Rows(I)("bcomid")) & "'"
                Common.Insert2Ary(strarray, strsql)
                strsql = objDO.SetPOFulFilment(DS.Tables(0).Rows(I)("po_no"), DS.Tables(0).Rows(I)("bcomid"))
                Common.Insert2Ary(strarray, strsql)
            Next
            objDO = Nothing
            If objDb.BatchExecute(strarray) Then
                '//lacking -- PO No and Buyer Company
                Dim objMail As New Email
                Dim objPO As New PurchaseOrder
                Dim objUsers As New Users

                For I = 0 To DS.Tables(0).Rows.Count - 1
                    'Michelle (6/2/2010) - If PO is cancelled by Buyer Admin, then send email to Buyer Admin and Buyer
                    Dim strCRUser As String = objPO.get_cancelReq(DS.Tables(0).Rows(I)("cr_num"), DS.Tables(0).Rows(I)("bcomid"))
                    Dim IsBA As Boolean
                    'If HttpContext.Current.Session("Env") <> "FTN" Then
                    If blnEnterpriseVersion = True Then
                        IsBA = objUsers.BAdminRole(strCRUser, DS.Tables(0).Rows(I)("bcomid"))
                    Else
                        IsBA = False
                    End If

                    If IsBA Then
                        objMail.sendNotification(EmailType.AckPOCancellationRequest, userid, DS.Tables(0).Rows(I)("bcomid"), v_com_id, DS.Tables(0).Rows(I)("po_no"), DS.Tables(0).Rows(I)("cr_num"), "ToBuyer", strCRUser)
                        objMail.sendNotification(EmailType.AckPOCancellationRequest, userid, DS.Tables(0).Rows(I)("bcomid"), v_com_id, DS.Tables(0).Rows(I)("po_no"), DS.Tables(0).Rows(I)("cr_num"), "BACancel", strCRUser)
                    Else
                        objMail.sendNotification(EmailType.AckPOCancellationRequest, userid, DS.Tables(0).Rows(I)("bcomid"), v_com_id, DS.Tables(0).Rows(I)("po_no"), DS.Tables(0).Rows(I)("cr_num"), "BuyerCancel", "")
                    End If

                    If update_ack = "" Then
                        update_ack = DS.Tables(0).Rows(I)("cr_num")
                    Else
                        update_ack = update_ack & "," & Common.Parse(DS.Tables(0).Rows(I)("cr_num")) & ""
                    End If
                    'update_ack = update_ack & " has been Acknowledged."
                Next
                objMail = Nothing
            Else
                update_ack = "Error occurs. Kindly contact the Administrator to resolve this."
            End If
        End Function
    End Class




End Namespace

