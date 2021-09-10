Imports System
Imports System.Configuration
Imports System.Web
Imports System.Web.UI.WebControls
Imports AgoraLegacy
Imports SSO.Component

Namespace AgoraLegacy
    Public Class MenuSeq
        Dim objDb As New EAD.DBCom
        Dim strMenuMassage As String
        Public Function GetMenu() As DataSet
            Dim ds As DataSet
            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))
            Dim strsql As String

            strsql = "SELECT MM_MENU_NAME, MM_MENU_IDX FROM menu_mstr WHERE MM_MENU_LEVEL NOT LIKE '%,%' AND MM_MENU_IDX IS NOT NULL GROUP BY MM_MENU_IDX ORDER BY MM_MENU_IDX "

            ds = objDb.FillDs(strsql)
            Return ds
        End Function
        Public Function GetAllMenu(ByVal intMenuName As String) As DataSet
            Dim ds As DataSet
            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))
            Dim strsql As String

            If intMenuName <> "" Then
                strsql = "SELECT MM_MENU_ID, MM_MENU_NAME, MM_MENU_LEVEL, MM_MENU_PARENT, MM_MENU_URL, MM_MENU_IDX FROM MENU_MSTR WHERE MM_MENU_IDX = " & intMenuName & " ORDER BY MM_MENU_IDX  "
            Else
                strsql = "SELECT MM_MENU_ID, MM_MENU_NAME, MM_MENU_LEVEL, MM_MENU_PARENT, MM_MENU_URL, MM_MENU_IDX FROM MENU_MSTR ORDER BY MM_MENU_IDX  "
            End If
            ds = objDb.FillDs(strsql)

            Return ds
        End Function
        Public Sub FillModule(ByRef pDropDownList As UI.WebControls.DropDownList, Optional ByVal strConn As String = "")
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView
            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))

            strSql = "SELECT MM_MENU_NAME, MM_MENU_IDX FROM menu_mstr WHERE MM_MENU_LEVEL NOT LIKE '%,%' "
            strSql &= " GROUP BY MM_MENU_IDX ORDER BY MM_MENU_IDX "
            drw = objDb.GetView(strSql)

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "MM_MENU_NAME", "MM_MENU_IDX", drw)
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                strDefaultValue = lstItem.Text
                pDropDownList.Items.Insert(0, lstItem)
            Else
                '//no suppose to happen
                lstItem.Text = "---Select---"
                pDropDownList.Items.Insert(0, lstItem)
            End If
            objDb = Nothing
        End Sub
        Public Sub FillSeqNo(ByRef pDropDownList As UI.WebControls.DropDownList, Optional ByVal strConn As String = "")
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView
            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))

            strSql = "SELECT MM_MENU_PARENT FROM menu_mstr WHERE MM_MENU_LEVEL NOT LIKE '%,%' "
            strSql &= " GROUP BY MM_MENU_PARENT ORDER BY MM_MENU_PARENT "
            drw = objDb.GetView(strSql)

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "MM_MENU_PARENT", "MM_MENU_PARENT", drw)
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                strDefaultValue = lstItem.Text
                pDropDownList.Items.Insert(0, lstItem)
            Else
                '//no suppose to happen
                lstItem.Text = "---Select---"
                pDropDownList.Items.Insert(0, lstItem)
            End If
            objDb = Nothing
        End Sub

        Public Function GetModuleParent(ByVal iSelectedModValue As Integer, Optional ByVal strConn As String = "") As Integer
            Dim strSQL As String
            Dim SelectedParentValue As Integer

            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))


            strSQL = "SELECT MM_MENU_PARENT FROM MENU_MSTR WHERE MM_MENU_IDX = " & iSelectedModValue & ""
            SelectedParentValue = objDb.GetVal(strSQL)

            Return SelectedParentValue

        End Function
        Public Function GetMaxModule(Optional ByVal strConn As String = "") As Integer
            Dim strSQL As String
            Dim MaxValue As Integer

            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))


            strSQL = "SELECT MAX(MM_MENU_PARENT)AS MAX FROM MENU_MSTR"
            MaxValue = objDb.GetVal(strSQL)

            Return MaxValue

        End Function
        Public Function UpdateModuleSeqNo(ByVal iNewSeqNo As Integer, ByVal iSelectedParentVal As Integer, ByVal iSelectedModValue As String, ByVal iMaxVal As Integer)
            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))
            Dim strsql As String

            strsql = "UPDATE MENU_MSTR SET MM_MENU_IDX = -1 WHERE MM_MENU_PARENT= '" & iSelectedParentVal & "'"
            objDb.Execute(strsql)

            'UP
            If iSelectedModValue - iNewSeqNo > 1 Then ' any selected to any node -- ok
                strsql = "UPDATE MENU_MSTR SET MM_MENU_IDX = MM_MENU_IDX + 1 WHERE (MM_MENU_IDX BETWEEN " & iNewSeqNo & " AND " & (iSelectedModValue - 1) & ") "

            ElseIf iSelectedModValue - iNewSeqNo = 1 Then 'any selected to above 1 node -- ok
                strsql = "UPDATE MENU_MSTR SET MM_MENU_IDX = MM_MENU_IDX + 1 WHERE (MM_MENU_IDX = " & iNewSeqNo & " ) "

                'down
                'ElseIf iNewSeqNo = iMaxVal Then 'any selected to last node -- ok
                '    strsql = "UPDATE MENU_MSTR SET MM_MENU_IDX = MM_MENU_IDX - 1 WHERE (MM_MENU_IDX BETWEEN " & (iSelectedModValue + 1) & " AND " & (iNewSeqNo) & ") "
                '    objDb.Execute(strsql)
            ElseIf iNewSeqNo - iSelectedModValue > 1 Then 'any selected to mid node
                strsql = "UPDATE MENU_MSTR SET MM_MENU_IDX = MM_MENU_IDX - 1 WHERE (MM_MENU_IDX BETWEEN " & (iSelectedModValue + 1) & " AND " & (iNewSeqNo) & ") "

                'strsql = "UPDATE MENU_MSTR SET MM_MENU_IDX = MM_MENU_IDX + 1 WHERE MM_MENU_IDX = " & iSelectedModValue & " " 'AND " & iMaxVal & ")
                'objDb.Execute(strsql)
            ElseIf iNewSeqNo - iSelectedModValue = 1 Then ' any selected to below 1 node -- ok
                strsql = "UPDATE MENU_MSTR SET MM_MENU_IDX = MM_MENU_IDX - 1 WHERE (MM_MENU_IDX = " & iNewSeqNo & " ) "

            End If
            objDb.Execute(strsql)

            strsql = "UPDATE MENU_MSTR SET MM_MENU_IDX = '" & iNewSeqNo & "' WHERE MM_MENU_PARENT= '" & iSelectedParentVal & "'"

            objDb.Execute(strsql)
        End Function
        Public Function AddNewModule(ByVal dsMenu As DataSet) As Boolean

            Dim test As String
            Dim Query(0) As String
            Dim strSQL, strCompid As String
            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))


            strSQL = " INSERT INTO MENU_MSTR(" &
                    "MM_MENU_ID,MM_MENU_NAME,MM_MENU_IMAGE,MM_MENU_LEVEL," &
                    "MM_MENU_PARENT, MM_MENU_URL, MM_MENU_IMAGE_EXP, MM_MENU_TIPS," &
                    "MM_GROUP, MM_MENU_TARGET, MM_LAST_NODE, MM_MENU_IDX) " &
                    "VALUES('" & Common.Parse(dsMenu.Tables(0).Rows(0)("MenuID")) & "','" & Common.Parse(dsMenu.Tables(0).Rows(0)("MenuName")) & "','" & Common.Parse(dsMenu.Tables(0).Rows(0)("MenuImg")) & "','" & Common.Parse(dsMenu.Tables(0).Rows(0)("MenuLvl")) & "'," &
                    "'" & Common.Parse(dsMenu.Tables(0).Rows(0)("MenuParent")) & "','" & Common.Parse(dsMenu.Tables(0).Rows(0)("MenuURL")) & "','" & Common.Parse(dsMenu.Tables(0).Rows(0)("MenuImgExp")) & "','" & Common.Parse(dsMenu.Tables(0).Rows(0)("MenuTips")) & "'," &
                     "'" & Common.Parse(dsMenu.Tables(0).Rows(0)("MenuGroup")) & "','" & Common.Parse(dsMenu.Tables(0).Rows(0)("MenuTarget")) & "','" & Common.Parse(dsMenu.Tables(0).Rows(0)("MenuLastNode")) & "','" & Common.Parse(dsMenu.Tables(0).Rows(0)("MenuIdx")) & "')"


            If objDb.Execute(strSQL) Then
                strMenuMassage = Common.RecordSave
                Return True
            End If


        End Function
        Public Function GetLastMenuID(Optional ByVal strConn As String = "") As Integer
            Dim strSQL As String
            Dim LastMenuID As Integer

            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))


            strSQL = "SELECT MAX(CAST(MM_MENU_ID AS UNSIGNED)) FROM MENU_MSTR"
            LastMenuID = objDb.GetVal(strSQL)

            Return LastMenuID

        End Function
        Public Function GetLastParent(Optional ByVal strConn As String = "") As Integer
            Dim strSQL As String
            Dim LastPR As Integer

            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))


            strSQL = "SELECT MAX(MM_MENU_PARENT) FROM MENU_MSTR"
            LastPR = objDb.GetVal(strSQL)

            Return LastPR

        End Function

        Public Sub FillParent(ByRef pDropDownList As UI.WebControls.DropDownList, Optional ByVal strConn As String = "")
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView
            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))

            strSql = "SELECT MM_MENU_NAME, MM_MENU_PARENT FROM MENU_MSTR WHERE MM_MENU_LEVEL NOT LIKE '%,%' ORDER BY MM_MENU_IDX "

            drw = objDb.GetView(strSql)

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "MM_MENU_NAME", "MM_MENU_PARENT", drw)
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                strDefaultValue = lstItem.Text
                pDropDownList.Items.Insert(0, lstItem)
            Else
                '//no suppose to happen
                lstItem.Text = "---Select---"
                pDropDownList.Items.Insert(0, lstItem)
            End If
            objDb = Nothing
        End Sub
        Public Function GetLastLevel(ByVal iLastParent As Integer, Optional ByVal strConn As String = "") As Integer
            Dim strSQL As String
            Dim LastLvl As Integer

            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))

            If iLastParent < 9 Then

                strSQL = "SELECT COALESCE(MAX(CAST(MID(mm_menu_leveL,3,2) AS UNSIGNED)),0) AS LastLevel FROM MENU_MSTR WHERE MM_MENU_PARENT = " & iLastParent & ""

            Else
                strSQL = "SELECT COALESCE(MAX(CAST(MID(mm_menu_leveL,4,2) AS UNSIGNED)),0) AS LastLevel FROM MENU_MSTR WHERE MM_MENU_PARENT = " & iLastParent & ""
            End If
            LastLvl = objDb.GetVal(strSQL)
            Return LastLvl

        End Function
        Public Function DelModule(ByVal strMenuID As String) As Boolean

            Dim test As String
            Dim Query(0) As String
            Dim strSQL, strCompid As String
            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))


            strSQL = " DELETE FROM MENU_MSTR WHERE MM_MENU_ID = '" & strMenuID & "'"



            If objDb.Execute(strSQL) Then
                strMenuMassage = Common.RecordDelete
                Return True
            End If


        End Function

        Public Function GetMenuDetails(ByVal pMenuId As String) As DataView
            'Dim objUsrGrp As New UserGroup
            Dim dv As DataView
            Dim strSQL As String
            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))
            strSQL = "SELECT MM_MENU_ID, MM_MENU_NAME, MM_MENU_IMAGE, MM_MENU_LEVEL, MM_MENU_PARENT, MM_MENU_URL, MM_MENU_IDX FROM MENU_MSTR WHERE MM_MENU_ID = '" & pMenuId & "' "

            dv = objDb.GetView(strSQL)

            Return dv


        End Function

        Public Function UpdateModule(ByVal dsMenu As DataSet) As Boolean

            Dim test As String
            Dim Query(0) As String
            Dim strSQL, strCompid As String
            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))


            strSQL = "UPDATE MENU_MSTR SET " &
                  "MM_MENU_NAME='" & Common.Parse(dsMenu.Tables(0).Rows(0)("MenuName")) & "'," &
                  "MM_MENU_URL='" & Common.Parse(dsMenu.Tables(0).Rows(0)("MenuURL")) & "'," &
                  "MM_MENU_IMAGE='" & Common.Parse(dsMenu.Tables(0).Rows(0)("MenuImg")) & "'" &
                  " WHERE MM_MENU_ID = '" & Common.Parse(dsMenu.Tables(0).Rows(0)("MenuID")) & "'"

            If objDb.Execute(strSQL) Then
                strMenuMassage = Common.RecordSave
                Return True
            End If


        End Function
        Public Function AddModuleToFixedRole(ByVal dsNewMenuID As DataSet, ByVal sFixedRole As String, ByVal iSelectedParentVal As Integer, ByVal iCount As Integer)

            Dim i As Integer
            Dim strSQL, strCompid As String
            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))



            For i = 0 To iCount - 1
                If ChkFixedRole(Common.Parse(dsNewMenuID.Tables(0).Rows(i)("MM_MENU_ID")), sFixedRole) Then
                    strSQL = "INSERT INTO FIXED_ROLE(" &
                            "FR_ROLE_ID,FR_MENU_ID) " &
                            "VALUES('" & sFixedRole & "','" & Common.Parse(dsNewMenuID.Tables(0).Rows(i)("MM_MENU_ID")) & "')"


                    objDb.Execute(strSQL)
                End If
            Next
            strMenuMassage = Common.RecordSave
            Return True
        End Function
        Public Function AddModuleToNewFixedRole(ByVal dsNewMenuID As DataSet, ByVal sFixedRole As String, ByVal iSelectedParentVal As Integer, ByVal iCount As Integer)

            Dim i As Integer
            Dim strSQL, strCompid As String
            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))

            For i = 0 To iCount - 1
                If ChkFixedRole(Common.Parse(dsNewMenuID.Tables(0).Rows(i)("MM_MENU_ID")), sFixedRole) Then
                    strSQL = "INSERT INTO FIXED_ROLE(" &
                            "FR_ROLE_ID,FR_MENU_ID) " &
                            "VALUES('" & sFixedRole & "','" & Common.Parse(dsNewMenuID.Tables(0).Rows(i)("MM_MENU_ID")) & "')"


                    objDb.Execute(strSQL)
                End If
            Next
            strMenuMassage = Common.RecordSave
            Return True
        End Function
        Public Function GetMenuID(ByVal strParent As Integer) As DataSet
            Dim ds As DataSet
            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))
            Dim strsql As String

            strsql = "SELECT MM_MENU_ID FROM MENU_MSTR WHERE MM_MENU_PARENT = '" & strParent & "'"

            ds = objDb.FillDs(strsql)
            Return ds
        End Function
        Public Function AddModuleToUAR(ByVal dsNewMenuID As DataSet, ByVal sUsrGrp As String, ByVal iSelectedParentVal As Integer, ByVal iCount As Integer)

            Dim MaxUARID As String
            Dim i As Integer
            Dim strSQL, strCompid As String
            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))

            For i = 0 To iCount - 1
                If ChkUAR(Common.Parse(dsNewMenuID.Tables(0).Rows(i)("MM_MENU_ID")), sUsrGrp) Then
                    strSQL = " SELECT MAX(UAR_INDEX) FROM USER_ACCESS_RIGHT"

                    MaxUARID = (objDb.GetVal(strSQL) + 1)

                    strSQL = "INSERT INTO USER_ACCESS_RIGHT(" &
                            "UAR_INDEX, UAR_USRGRP_ID, UAR_MENU_ID, UAR_ALLOW_INSERT, UAR_ALLOW_UPDATE, UAR_ALLOW_DELETE, UAR_ALLOW_VIEW, UAR_DELETE_IND, UAR_MOD_BY, UAR_MOD_DT, UAR_ENT_BY, UAR_ENT_DT, UAR_APP_PKG) " &
                            " VALUE ('" & MaxUARID & "', '" & sUsrGrp & "','" & Common.Parse(dsNewMenuID.Tables(0).Rows(i)("MM_MENU_ID")) & "', 'Y', 'Y', 'Y','Y','N',NULL,NULL,'system', CURRENT_DATE(),'eProcure')"


                    objDb.Execute(strSQL)
                End If
            Next
            strMenuMassage = Common.RecordSave
            Return True
        End Function
        Public Function ChkUAR(ByVal sNewMenuID As String, ByVal sUsrGrp As String) As Boolean
            Dim strVal As String
            Dim strSQL As String
            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))

            strSQL = "SELECT UAR_MENU_ID FROM USER_ACCESS_RIGHT WHERE UAR_MENU_ID = '" & sNewMenuID & "' and UAR_USRGRP_ID = '" & sUsrGrp & "'"

            strVal = objDb.GetVal(strSQL)

            If strVal = "" Or strVal Is System.DBNull.Value Then

                Return True
            Else
                Return False
            End If
        End Function
        Public Function ChkFixedRole(ByVal sNewMenuID As String, ByVal sFixedRole As String) As Boolean
            Dim strVal As String
            Dim strSQL As String
            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))

            strSQL = "SELECT FR_MENU_ID FROM FIXED_ROLE WHERE FR_MENU_ID = '" & sNewMenuID & "' and FR_ROLE_ID = '" & sFixedRole & "'"

            strVal = objDb.GetVal(strSQL)

            If strVal = "" Or strVal Is System.DBNull.Value Then

                Return True
            Else
                Return False
            End If
        End Function
        Public Function GetMenuCount(ByVal strParent As Integer) As Integer
            Dim Count As String
            Dim strSQL As String
            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))

            strSQL = "SELECT COUNT(*) FROM MENU_MSTR WHERE MM_MENU_PARENT = " & strParent & ""

            Count = objDb.GetVal(strSQL)
            Return Count
        End Function
        Public Sub FillFixedRole(ByRef pDropDownList As UI.WebControls.DropDownList, Optional ByVal strConn As String = "")
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView
            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))

            strSql = "SELECT  DISTINCT(FR_ROLE_ID) FROM FIXED_ROLE"
            drw = objDb.GetView(strSql)

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "FR_ROLE_ID", "FR_ROLE_ID", drw)
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                strDefaultValue = lstItem.Text
                pDropDownList.Items.Insert(0, lstItem)
            Else
                '//no suppose to happen
                lstItem.Text = "---Select---"
                pDropDownList.Items.Insert(0, lstItem)
            End If
            objDb = Nothing
        End Sub

        Public Sub FillUserGroup(ByRef pDropDownList As UI.WebControls.DropDownList, Optional ByVal strConn As String = "")
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView
            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))

            strSql = " SELECT UGM_USRGRP_ID FROM USER_GROUP_MSTR"
            drw = objDb.GetView(strSql)

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "UGM_USRGRP_ID", "UGM_USRGRP_ID", drw)
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                strDefaultValue = lstItem.Text
                pDropDownList.Items.Insert(0, lstItem)
            Else
                '//no suppose to happen
                lstItem.Text = "---Select---"
                pDropDownList.Items.Insert(0, lstItem)
            End If
            objDb = Nothing
        End Sub

        Public Property Message() As String
            Get
                Message = strMenuMassage
            End Get
            Set(ByVal Value As String)
                strMenuMassage = Value
            End Set
        End Property
    End Class
End Namespace