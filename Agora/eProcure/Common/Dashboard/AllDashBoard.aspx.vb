Imports System.Drawing

Imports AgoraLegacy
Imports eProcure.Component

Public Class AllDashBoard
    Inherits AgoraLegacy.AppBaseClass
    Dim objDO As New Dashboard
    Dim objDO1 As New DeliveryOrder
    Dim objDB As New EAD.DBCom
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Protected WithEvents divPM As New System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents dtgPendingApprPM As New System.Web.UI.WebControls.DataGrid
    Protected WithEvents dtgPendingApprAO As New System.Web.UI.WebControls.DataGrid
    Protected WithEvents dtgInPendingPymt As New System.Web.UI.WebControls.DataGrid
    Protected WithEvents dtgInInv As New System.Web.UI.WebControls.DataGrid
    Public Enum MyApp
        icPONum = 0
        icSubDate = 1
        icVenName = 2
        icCurren = 3
        icAmt = 4
        icon = 5
        ictw = 6
        icth = 7
    End Enum
    Public Enum OutPO
        icSta = 0
        icPONum = 1
        icVenName = 2
        icAccDate = 3
        icTPQ = 4
        icOPQ = 5
    End Enum
    Public Enum VenPO
        icSta = 0
        icPONum = 1
        icPODate = 2
        icdueDate = 3
        icPurComp = 4
        icOrQ = 5
        icOrOut = 6
    End Enum
    Public Enum OvVenPO
        icPONum = 0
        icCreDate = 1
        icDueDate = 2
        icPurComp = 3
        icOrQ = 4
        icOrOut = 5
    End Enum
    Public Enum EnumIPP
        docno = 0
        vendor = 1
        amt = 2
        createdt = 3
        submitdt = 4
        status = 5
    End Enum

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Not Page.IsPostBack Then

            PopulatePanelName()
            SetDashboardGridProperty(dtgPendingApprPM)
            SetDashboardGridProperty(dtgPendingApprAO)
            SetDashboardGridProperty(dtgPendingMyAppr)
            SetDashboardGridProperty(dtgOutstdPO)
            SetDashboardGridProperty(dtgOutstandingRFQ)
            SetDashboardGridProperty(dtgInDOSK)
            SetDashboardGridProperty(dtgOutstandingPOVend)
            SetDashboardGridProperty(dtgOverduePOVend)
            SetDashboardGridProperty(dtgOutstandingRFQVend)
            SetDashboardGridProperty(dtgOutstandingInvoiceVend)
            SetDashboardGridProperty(dtgInPendingPymt)
            SetDashboardGridProperty(dtgInInv)
            SetDashboardGridProperty(dtgOutstdGRNQCVerify)
            SetDashboardGridProperty(dtgOutstdPR)
            SetDashboardGridProperty(dtgPendingMyAppPR)
            SetDashboardGridProperty(dtgPendingConvPR)
            SetDashboardGridProperty(dtgOutstdIPPDoc)
            SetDashboardGridProperty(dtgIPPApproval)
            SetDashboardGridProperty(dtgIPPPendingPSDSentDate)
            SetDashboardGridProperty(dtgIPPPendingPSDRecvDate)
            SetDashboardGridProperty(dtgIQCApproval)
            SetDashboardGridProperty(dtgOutstandingIR)
            SetDashboardGridProperty(dtgPendingMRSAcknowledge)
            SetDashboardGridProperty(dtgPendingMyIRApproval)
            SetDashboardGridProperty(dtgIssueMRS)
            SetDashboardGridProperty(dtgOutRIAck)
			'Modified for IPP GST Stage 2A
            SetDashboardGridProperty(dtgOutBillDoc)
            SetDashboardGridProperty(dtgPendingBillApproval)
            '------------------------------

            'Yap: 2015-02-27: Modified for Agora GST Stage 2
            SetDashboardGridProperty(dtgFOIncomingDN)
            SetDashboardGridProperty(dtgFMIncomingPendingDN)
            SetDashboardGridProperty(dtgFMPendingAckCN)

            PopulateGrid()
        End If

    End Sub
    Sub PopulatePanelName()
        Dim ds As New DataSet
        Dim objDash As New Dashboard
        Dim rows As DataRow
        Dim strary As New ArrayList
        ds = objDash.GetDashboardPanelName()
        For Each rows In ds.Tables(0).Rows
            strary.Add(rows("DM_PANEL_NAME"))
        Next

        POPendingMyAppr.Text = strary(0)
        POPendingApprPM.Text = strary(1)
        OutstdPO.Text = strary(2)
        OutstdRFQ.Text = strary(3)
        InInv.Text = strary(4)
        InPendingPymt.Text = strary(5)
        OutstdGRNQCVerify.Text = strary(6)
        InDO.Text = strary(7)
        PO.Text = strary(8)
        OverduePO.Text = strary(9)
        OutstdRFQVend.Text = strary(10)
        OutstdInv.Text = strary(11)
        OutstdPR.Text = strary(12)
        PendingMyAppPR.Text = strary(13)
        PendingConvPR.Text = strary(14)
        'Zulham 09072018 - PAMB
        OutstdIPPDoc.Text = strary(15).ToString.Replace("IPP", "E2P")
        IPPApproval.Text = strary(16).ToString.Replace("IPP", "E2P")
        IPPPendingPSDSentDate.Text = strary(17).ToString.Replace("IPP", "E2P")
        IPPPendingPSDRecvDate.Text = strary(18).ToString.Replace("IPP", "E2P")
        'End
        IQCApproval.Text = strary(19)
        OutstandingIR.Text = strary(20)
        PendingMRSAcknowledge.Text = strary(21)
        PendingMyIRApproval.Text = strary(22)
        IssueMRS.Text = strary(23)
        OutRIAck.Text = strary(24)
		'Modified for IPP GST Stage 2A
        OutBillDoc.Text = strary(25)
        PendingBillApproval.Text = strary(26)
        '-----------------------------------

        'Yap: 2015-02-27: Modified for Agora GST Stage 2
        FOIncomingDN.Text = strary(27)
        FMIncomingPendingDN.Text = strary(28)
        FMPendingAckCN.Text = strary(29)

        'get user role
        Dim strFO, strFM As String
        Dim blnFO, blnFM As Boolean
        'Dim blnIPPOfficer As Boolean
        'Dim blnIPPOfficerS As Boolean
        'Dim blnIPPAO As Boolean

        Dim objUsers As New Users

        strFO = System.Enum.GetName(GetType(FixedRole), FixedRole.Finance_Officer)
        strFM = System.Enum.GetName(GetType(FixedRole), FixedRole.Finance_Manager)
        strFO = "'" & Replace(strFO, "_", " ") & "'"
        strFM = "'" & Replace(strFM, "_", " ") & "'"

        blnFO = objUsers.checkUserFixedRole(strFO)
        blnFM = objUsers.checkUserFixedRole(strFM)
        'blnIPPOfficer = objUsers.checkUserFixedRole("'IPP Officer(F)'")
        'blnIPPAO = objUsers.checkUserFixedRole("'IPP Approving Officer'")
        'blnIPPOfficerS = objUsers.checkUserFixedRole("'IPP Officer'")
        'Michelle (8/10/2011) - Issue 986 
        blnFM = objDash.ChkFMCanApprove()
        ViewState("blnFM") = blnFM
        ViewState("role") = getUserRole(blnFO, blnFM)
        'ViewState("role") = getUserRole(blnFO, blnFM, blnIPPOfficer, blnIPPOfficerS)
        Session("role") = ViewState("role")

    End Sub
    'Private Function getUserRole(ByVal blnFo As Boolean, ByVal blnFM As Boolean, ByVal blnIPPOfficer As Boolean, ByVal blnIPPOfficerS As Boolean) As Integer
    Private Function getUserRole(ByVal blnFo As Boolean, ByVal blnFM As Boolean) As Integer
        If blnFo And blnFM Then
            getUserRole = 4
        ElseIf blnFo = False And blnFM = True Then
            getUserRole = 3
        ElseIf blnFo = True And blnFM = False Then
            getUserRole = 2

            'ElseIf blnIPPOfficer = True Then ' finance IPP officer 
            '    getUserRole = 6
            'ElseIf blnIPPOfficerS = True Then 'Source IPP officer
            '    getUserRole = 7

        Else
            getUserRole = 1
        End If
    End Function
    Sub rebindgrid()
        Dim objDash As New Dashboard
        Dim ds As New DataSet
        Dim row As DataRow

        Dim fixRole As String
        Dim aryFixRole As New ArrayList
        aryFixRole = Session("MixUserRole")
        For i As Integer = 0 To aryFixRole.Count - 1
            ds = objDash.GetDashboardPanel(aryFixRole(i))
            If ds.Tables(0).Rows.Count > 0 Then
                For Each row In ds.Tables(0).Rows
                    Select Case row("DM_PANEL_ID")
                        Case 1
                            BindgridPendingMyAppr()
                        Case 2
                            'Jules 2018.08.03 - Need to check whether user actually has access rights, not just based on Fixed Role.
                            If objDB.Exist("SELECT '*' FROM MENU_MSTR " &
                                        "INNER JOIN USER_ACCESS_RIGHT ON UAR_MENU_ID = MM_MENU_ID AND UAR_ALLOW_INSERT = 'Y'" &
                                        "INNER JOIN USERS_USRGRP ON UU_USRGRP_ID= UAR_USRGRP_ID AND UU_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND UU_USER_ID='" & HttpContext.Current.Session("UserID") & "' " &
                                        "INNER JOIN USER_MSTR ON UM_USER_ID= UU_USER_ID AND UM_USER_ID = '" & HttpContext.Current.Session("UserID") & "' " &
                                        "WHERE MM_GROUP='ehub' AND UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND MM_MENU_NAME='Purchase Order' AND UAR_DELETE_IND = 'N' ") Then
                                BindgridPendingApprPM()
                            End If
                            'End modification.
                        Case 3
                            'Jules 2018.08.03 - Need to check whether user actually has access rights, not just based on Fixed Role.
                            If objDB.Exist("SELECT '*' FROM MENU_MSTR " &
                                        "INNER JOIN USER_ACCESS_RIGHT ON UAR_MENU_ID = MM_MENU_ID AND UAR_ALLOW_INSERT = 'Y'" &
                                        "INNER JOIN USERS_USRGRP ON UU_USRGRP_ID= UAR_USRGRP_ID AND UU_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND UU_USER_ID='" & HttpContext.Current.Session("UserID") & "' " &
                                        "INNER JOIN USER_MSTR ON UM_USER_ID= UU_USER_ID AND UM_USER_ID = '" & HttpContext.Current.Session("UserID") & "' " &
                                        "WHERE MM_GROUP='ehub' AND UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND MM_MENU_NAME='Purchase Order' AND UAR_DELETE_IND = 'N' ") Then
                                BindgridOutstdPO()
                            End If
                            'End modification.
                        Case 4
                            'Jules 2018.08.03 - Need to check whether user actually has access rights, not just based on Fixed Role.
                            If objDB.Exist("SELECT '*' FROM MENU_MSTR " &
                                        "INNER JOIN USER_ACCESS_RIGHT ON UAR_MENU_ID = MM_MENU_ID AND UAR_ALLOW_INSERT = 'Y'" &
                                        "INNER JOIN USERS_USRGRP ON UU_USRGRP_ID= UAR_USRGRP_ID AND UU_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND UU_USER_ID='" & HttpContext.Current.Session("UserID") & "' " &
                                        "INNER JOIN USER_MSTR ON UM_USER_ID= UU_USER_ID AND UM_USER_ID = '" & HttpContext.Current.Session("UserID") & "' " &
                                        "WHERE MM_GROUP='ehub' AND UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND MM_MENU_NAME='Request For Quotation' AND UAR_DELETE_IND = 'N' ") Then
                                BindgridOutstandingRFQ()
                            End If
                            'End modification.
                        Case 5
                            BindgridInInv()
                        Case 6 And ViewState("blnFM")
                            BindgridInPendingPymt()
                        Case 7
                            BindgridOutstandingGRNQCVerify()
                        Case 8
                            BindgridInDOSK()
                        Case 9
                            BindgridOutstandingPOVend()
                        Case 10
                            BindgridOverduePOVend()
                        Case 11
                            BindgridOutstandingRFQVend()
                        Case 12
                            BindgridOutstandingInvoiceVend()
                        Case 13
                            BindgridOutstdPR()
                        Case 14
                            'Jules 2018.08.06 - Need to check whether user actually has access rights, not just based on Fixed Role.
                            If objDB.Exist("SELECT '*' FROM MENU_MSTR " &
                                        "INNER JOIN USER_ACCESS_RIGHT ON UAR_MENU_ID = MM_MENU_ID AND UAR_ALLOW_INSERT = 'Y'" &
                                        "INNER JOIN USERS_USRGRP ON UU_USRGRP_ID= UAR_USRGRP_ID AND UU_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND UU_USER_ID='" & HttpContext.Current.Session("UserID") & "' " &
                                        "INNER JOIN USER_MSTR ON UM_USER_ID= UU_USER_ID AND UM_USER_ID = '" & HttpContext.Current.Session("UserID") & "' " &
                                        "WHERE MM_GROUP='ehub' AND UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND MM_MENU_NAME='Purchase Request Approval' AND UAR_DELETE_IND = 'N' ") Then
                                BindgridPendingMyAppPR()
                            End If
                            'End modification.
                        Case 15
                            'Jules 2018.08.03 - Need to check whether user actually has access rights, not just based on Fixed Role.
                            If objDB.Exist("SELECT '*' FROM MENU_MSTR " &
                                        "INNER JOIN USER_ACCESS_RIGHT ON UAR_MENU_ID = MM_MENU_ID AND UAR_ALLOW_INSERT = 'Y'" &
                                        "INNER JOIN USERS_USRGRP ON UU_USRGRP_ID= UAR_USRGRP_ID AND UU_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND UU_USER_ID='" & HttpContext.Current.Session("UserID") & "' " &
                                        "INNER JOIN USER_MSTR ON UM_USER_ID= UU_USER_ID AND UM_USER_ID = '" & HttpContext.Current.Session("UserID") & "' " &
                                        "WHERE MM_GROUP='ehub' AND UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND MM_MENU_NAME='Convert Purchase Request' AND UAR_DELETE_IND = 'N' ") Then
                                BindgridPendingConvPR()
                            End If
                            'End modification.
                        Case 16
                            BindgridOutstdIPPDoc()
                        Case 17
                            BindgridIPPApproval()
                        Case 18
                            BindgridIPPPendingPSDSentDate()
                        Case 19
                            BindgridIPPPendingPSDRecvDate()
                        Case 20
                            BindgridIQCApproval()
                        Case 21
                            BindgridOutstandingIR()
                        Case 22
                            BindgridPendingMRSAcknowledge()
                        Case 23
                            BindgridPendingMyIRApproval()
                        Case 24
                            BindgridIssueMRS()
                        Case 25
                            BindgridOutRIAck()
						'Modified for IPP GST Stage 2A
                        Case 26
                            BindgridOutBillDoc()
                        Case 27
                            BindgridPendingBillApproval()
                            '------------------------------

                            'Yap: 2015-02-27: Modified for Agora GST Stage 2
                        Case 28
                            'Jules 2018.08.06 - Need to check whether user actually has access rights, not just based on Fixed Role.
                            If objDB.Exist("SELECT '*' FROM MENU_MSTR " &
                                        "INNER JOIN USER_ACCESS_RIGHT ON UAR_MENU_ID = MM_MENU_ID AND UAR_ALLOW_INSERT = 'Y'" &
                                        "INNER JOIN USERS_USRGRP ON UU_USRGRP_ID= UAR_USRGRP_ID AND UU_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND UU_USER_ID='" & HttpContext.Current.Session("UserID") & "' " &
                                        "INNER JOIN USER_MSTR ON UM_USER_ID= UU_USER_ID AND UM_USER_ID = '" & HttpContext.Current.Session("UserID") & "' " &
                                        "WHERE MM_GROUP='ehub' AND UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND MM_MENU_NAME='Debit Note' AND UAR_DELETE_IND = 'N' ") Then
                                BindgridFOIncomingDN()
                            End If
                            'End modification.
                        Case 29
                            'Jules 2018.08.03 - Need to check whether user actually has access rights, not just based on Fixed Role.
                            If objDB.Exist("SELECT '*' FROM MENU_MSTR " &
                                        "INNER JOIN USER_ACCESS_RIGHT ON UAR_MENU_ID = MM_MENU_ID AND UAR_ALLOW_INSERT = 'Y'" &
                                        "INNER JOIN USERS_USRGRP ON UU_USRGRP_ID= UAR_USRGRP_ID AND UU_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND UU_USER_ID='" & HttpContext.Current.Session("UserID") & "' " &
                                        "INNER JOIN USER_MSTR ON UM_USER_ID= UU_USER_ID AND UM_USER_ID = '" & HttpContext.Current.Session("UserID") & "' " &
                                        "WHERE MM_GROUP='ehub' AND UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND MM_MENU_NAME LIKE '%Debit%' AND UAR_DELETE_IND = 'N' ") Then
                                BindgridFMIncomingPendingDN()
                            End If
                            'End modification.
                        Case 30
                            'Jules 2018.08.03 - Need to check whether user actually has access rights, not just based on Fixed Role.
                            If objDB.Exist("SELECT '*' FROM MENU_MSTR " &
                                        "INNER JOIN USER_ACCESS_RIGHT ON UAR_MENU_ID = MM_MENU_ID AND UAR_ALLOW_INSERT = 'Y'" &
                                        "INNER JOIN USERS_USRGRP ON UU_USRGRP_ID= UAR_USRGRP_ID AND UU_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND UU_USER_ID='" & HttpContext.Current.Session("UserID") & "' " &
                                        "INNER JOIN USER_MSTR ON UM_USER_ID= UU_USER_ID AND UM_USER_ID = '" & HttpContext.Current.Session("UserID") & "' " &
                                        "WHERE MM_GROUP='ehub' AND UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND MM_MENU_NAME LIKE '%Credit%' AND UAR_DELETE_IND = 'N' ") Then
                                BindgridFMPendingAckCN()
                            End If
                            'End modification.
                    End Select
                Next
            End If
        Next

    End Sub
    Sub PopulateGrid()
        Dim objDash As New Dashboard
        Dim ds As New DataSet
        Dim row As DataRow

        Dim fixRole As String
        Dim aryFixRole As New ArrayList
        aryFixRole = Session("MixUserRole")
        For i As Integer = 0 To aryFixRole.Count - 1
            ds = objDash.GetDashboardPanel(aryFixRole(i))
            If ds.Tables(0).Rows.Count > 0 Then
                For Each row In ds.Tables(0).Rows
                    Select Case row("DM_PANEL_ID")
                        Case 1
                            divPendingMyAppr.Style("display") = ""
                            dtgPendingMyAppr.CurrentPageIndex = 0
                            ViewState("SortAscendingPendingMyAppr") = "yes"
                            ViewState("SortExpressionPendingMyAppr") = "Submitted Date"
                            BindgridPendingMyAppr()
                        Case 2
                            'Jules 2018.08.03 - Need to check whether user actually has access rights, not just based on Fixed Role.
                            If objDB.Exist("SELECT '*' FROM MENU_MSTR " &
                                        "INNER JOIN USER_ACCESS_RIGHT ON UAR_MENU_ID = MM_MENU_ID AND UAR_ALLOW_INSERT = 'Y'" &
                                        "INNER JOIN USERS_USRGRP ON UU_USRGRP_ID= UAR_USRGRP_ID AND UU_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND UU_USER_ID='" & HttpContext.Current.Session("UserID") & "' " &
                                        "INNER JOIN USER_MSTR ON UM_USER_ID= UU_USER_ID AND UM_USER_ID = '" & HttpContext.Current.Session("UserID") & "' " &
                                        "WHERE MM_GROUP='ehub' AND UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND MM_MENU_NAME='Purchase Order' AND UAR_DELETE_IND = 'N' ") Then
                                divPendingApprPM.Style("display") = ""
                                dtgPendingApprPM.CurrentPageIndex = 0
                                ViewState("SortAscendingPendingApprPM") = "yes"
                                ViewState("SortExpressionPendingApprPM") = "Submitted Date"
                                BindgridPendingApprPM()
                            End If
                            'End modification.
                        Case 3
                            'Jules 2018.08.03 - Need to check whether user actually has access rights, not just based on Fixed Role.
                            If objDB.Exist("SELECT '*' FROM MENU_MSTR " &
                                        "INNER JOIN USER_ACCESS_RIGHT ON UAR_MENU_ID = MM_MENU_ID AND UAR_ALLOW_INSERT = 'Y'" &
                                        "INNER JOIN USERS_USRGRP ON UU_USRGRP_ID= UAR_USRGRP_ID AND UU_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND UU_USER_ID='" & HttpContext.Current.Session("UserID") & "' " &
                                        "INNER JOIN USER_MSTR ON UM_USER_ID= UU_USER_ID AND UM_USER_ID = '" & HttpContext.Current.Session("UserID") & "' " &
                                        "WHERE MM_GROUP='ehub' AND UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND MM_MENU_NAME='Purchase Order' AND UAR_DELETE_IND = 'N' ") Then
                                divOutstdPO.Style("display") = ""
                                dtgOutstdPO.CurrentPageIndex = 0
                                ViewState("SortAscendingOutstdPO") = "yes"
                                ViewState("SortExpressionOutstdPO") = "Accepted Date"
                                BindgridOutstdPO()
                            End If
                            'End modification.
                        Case 4
                            'Jules 2018.08.03 - Need to check whether user actually has access rights, not just based on Fixed Role.
                            If objDB.Exist("SELECT '*' FROM MENU_MSTR " &
                                        "INNER JOIN USER_ACCESS_RIGHT ON UAR_MENU_ID = MM_MENU_ID AND UAR_ALLOW_INSERT = 'Y'" &
                                        "INNER JOIN USERS_USRGRP ON UU_USRGRP_ID= UAR_USRGRP_ID AND UU_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND UU_USER_ID='" & HttpContext.Current.Session("UserID") & "' " &
                                        "INNER JOIN USER_MSTR ON UM_USER_ID= UU_USER_ID AND UM_USER_ID = '" & HttpContext.Current.Session("UserID") & "' " &
                                        "WHERE MM_GROUP='ehub' AND UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND MM_MENU_NAME='Request For Quotation' AND UAR_DELETE_IND = 'N' ") Then
                                divOutstandingRFQ.Style("display") = ""
                                dtgOutstandingRFQ.CurrentPageIndex = 0
                                ViewState("SortAscendingOutstandingRFQ") = "yes"
                                ViewState("SortExpressionOutstandingRFQ") = "Creation Date"
                                BindgridOutstandingRFQ()
                            End If
                            'End modification.
                        Case 5
                            divInInv.Style("display") = ""
                            dtgInInv.CurrentPageIndex = 0
                            ViewState("SortAscendingInInv") = "yes"
                            ViewState("SortExpressionInInv") = "Due Date"
                            BindgridInInv()
                        Case 6 And ViewState("blnFM")
                            divInPendingPymt.Style("display") = ""
                            dtgInPendingPymt.CurrentPageIndex = 0
                            ViewState("SortAscendingInPendingPymt") = "yes"
                            ViewState("SortExpressionInPendingPymt") = "Due Date"
                            BindgridInPendingPymt()
                        Case 7
                            divOutstandingGRNforQCVerify.Style("display") = ""
                            dtgOutstdGRNQCVerify.CurrentPageIndex = 0
                            ViewState("SortAscendingOutstandingGRNQCVerify") = "yes"
                            ViewState("SortExpressionOutstandingGRNQCVerify") = "IV_GRN_NO"
                            BindgridOutstandingGRNQCVerify()
                        Case 8
                            divInDOSK.Style("display") = ""
                            dtgInDOSK.CurrentPageIndex = 0
                            ViewState("SortAscendingInDOSK") = "yes"
                            ViewState("SortExpressionInDOSK") = "DOM_DO_DATE"
                            BindgridInDOSK()
                        Case 9
                            divOutstandingPOVend.Style("display") = ""
                            dtgOutstandingPOVend.CurrentPageIndex = 0
                            ViewState("SortAscendingOutstandingPOVend") = "yes"
                            ViewState("SortExpressionOutstandingPOVend") = "Due Date"
                            BindgridOutstandingPOVend()
                        Case 10
                            divOverduePOVend.Style("display") = ""
                            dtgOverduePOVend.CurrentPageIndex = 0
                            ViewState("SortAscendingOverduePOVend") = "yes"
                            ViewState("SortExpressionOverduePOVend") = "Due Date"
                            BindgridOverduePOVend()
                        Case 11
                            divOutstandingRFQVend.Style("display") = ""
                            dtgOutstandingRFQVend.CurrentPageIndex = 0
                            ViewState("SortAscendingOutstandingRFQVend") = "yes"
                            ViewState("SortExpressionOutstandingRFQVend") = "Creation Date"
                            BindgridOutstandingRFQVend()
                        Case 12
                            divOutstandingInvoiceVend.Style("display") = ""
                            dtgOutstandingInvoiceVend.CurrentPageIndex = 0
                            ViewState("SortAscendingOutstandingInvoiceVend") = "yes"
                            ViewState("SortExpressionOutstandingInvoiceVend") = "PO Number"
                            BindgridOutstandingInvoiceVend()
                        Case 13
                            divOutstdPR.Style("display") = ""
                            dtgOutstdPR.CurrentPageIndex = 0
                            ViewState("SortAscendingOutstdPR") = "yes"
                            ViewState("SortExpressionOutstdPR") = "Creation Date"
                            BindgridOutstdPR()
                        Case 14
                            'Jules 2018.08.06 - Need to check whether user actually has access rights, not just based on Fixed Role.
                            If objDB.Exist("SELECT '*' FROM MENU_MSTR " &
                                        "INNER JOIN USER_ACCESS_RIGHT ON UAR_MENU_ID = MM_MENU_ID AND UAR_ALLOW_INSERT = 'Y'" &
                                        "INNER JOIN USERS_USRGRP ON UU_USRGRP_ID= UAR_USRGRP_ID AND UU_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND UU_USER_ID='" & HttpContext.Current.Session("UserID") & "' " &
                                        "INNER JOIN USER_MSTR ON UM_USER_ID= UU_USER_ID AND UM_USER_ID = '" & HttpContext.Current.Session("UserID") & "' " &
                                        "WHERE MM_GROUP='ehub' AND UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND MM_MENU_NAME='Purchase Request Approval' AND UAR_DELETE_IND = 'N' ") Then
                                divPendingMyAppPR.Style("display") = ""
                                dtgPendingMyAppPR.CurrentPageIndex = 0
                                ViewState("SortAscendingPendingMyAppPR") = "yes"
                                ViewState("SortExpressionPendingMyAppPR") = "Submitted Date"
                                BindgridPendingMyAppPR()
                            End If
                            'End modification.
                        Case 15
                            'Jules 2018.08.03 - Need to check whether user actually has access rights, not just based on Fixed Role.
                            If objDB.Exist("SELECT '*' FROM MENU_MSTR " &
                                        "INNER JOIN USER_ACCESS_RIGHT ON UAR_MENU_ID = MM_MENU_ID AND UAR_ALLOW_INSERT = 'Y'" &
                                        "INNER JOIN USERS_USRGRP ON UU_USRGRP_ID= UAR_USRGRP_ID AND UU_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND UU_USER_ID='" & HttpContext.Current.Session("UserID") & "' " &
                                        "INNER JOIN USER_MSTR ON UM_USER_ID= UU_USER_ID AND UM_USER_ID = '" & HttpContext.Current.Session("UserID") & "' " &
                                        "WHERE MM_GROUP='ehub' AND UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND MM_MENU_NAME='Convert Purchase Request' AND UAR_DELETE_IND = 'N' ") Then
                                divPendingConvPR.Style("display") = ""
                                dtgPendingConvPR.CurrentPageIndex = 0
                                ViewState("SortAscendingPendingConvPR") = "yes"
                                ViewState("SortExpressionPendingConvPR") = "Approved Date"
                                BindgridPendingConvPR()
                            End If
                        Case 16
                            'If objDB.Exist("SELECT '*' FROM MENU_MSTR " & _
                            '            "INNER JOIN USER_ACCESS_RIGHT ON UAR_MENU_ID = MM_MENU_ID AND UAR_ALLOW_INSERT = 'Y'" & _
                            '            "INNER JOIN USERS_USRGRP ON UU_USRGRP_ID= UAR_USRGRP_ID AND UU_COY_ID='hlb' AND UU_USER_ID='" & HttpContext.Current.Session("UserID") & "' " & _
                            '            "INNER JOIN USER_MSTR ON UM_USER_ID= UU_USER_ID AND UM_USER_ID = '" & HttpContext.Current.Session("UserID") & "' " & _
                            '            "WHERE MM_GROUP='ehub'   AND UM_COY_ID='hlb'  AND MM_MENU_ID = 117") Then
                            '    divOutstdIPPDoc.Style("display") = ""
                            '    dtgOutstdIPPDoc.CurrentPageIndex = 0
                            '    ViewState("SortAscendingOutstdIPPDoc") = "yes"
                            '    ViewState("SortExpressionOutstdIPPDoc") = ""
                            '    BindgridOutstdIPPDoc()
                            'End If
                            'Modified for IPP GST 2A - CH - 10 Feb 2015
                            If objDB.Exist("SELECT '*' FROM MENU_MSTR " & _
                                        "INNER JOIN USER_ACCESS_RIGHT ON UAR_MENU_ID = MM_MENU_ID AND UAR_ALLOW_INSERT = 'Y'" & _
                                        "INNER JOIN USERS_USRGRP ON UU_USRGRP_ID= UAR_USRGRP_ID AND UU_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND UU_USER_ID='" & HttpContext.Current.Session("UserID") & "' " & _
                                        "INNER JOIN USER_MSTR ON UM_USER_ID= UU_USER_ID AND UM_USER_ID = '" & HttpContext.Current.Session("UserID") & "' " & _
                                        "WHERE MM_GROUP='ehub'   AND UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "'  AND MM_MENU_ID = 117") Then
                                divOutstdIPPDoc.Style("display") = ""
                                dtgOutstdIPPDoc.CurrentPageIndex = 0
                                ViewState("SortAscendingOutstdIPPDoc") = "yes"
                                ViewState("SortExpressionOutstdIPPDoc") = ""
                                BindgridOutstdIPPDoc()
                            End If
                            '--------------------------------------------------
                        Case 17
                            divIPPApproval.Style("display") = ""
                            dtgIPPApproval.CurrentPageIndex = 0
                            ViewState("SortAscendingIPPApproval") = "yes"
                            ViewState("SortExpressionIPPApproval") = ""
                            BindgridIPPApproval()
                        Case 18
                            'If objDB.Exist("SELECT '*' FROM MENU_MSTR " & _
                            '           "INNER JOIN USER_ACCESS_RIGHT ON UAR_MENU_ID = MM_MENU_ID AND UAR_ALLOW_INSERT = 'Y'" & _
                            '           "INNER JOIN USERS_USRGRP ON UU_USRGRP_ID= UAR_USRGRP_ID AND UU_COY_ID='hlb' AND UU_USER_ID='" & HttpContext.Current.Session("UserID") & "' " & _
                            '           "INNER JOIN USER_MSTR ON UM_USER_ID= UU_USER_ID AND UM_USER_ID = '" & HttpContext.Current.Session("UserID") & "' " & _
                            '           "WHERE MM_GROUP='ehub'   AND UM_COY_ID='hlb'  AND MM_MENU_ID = 130") Then
                            '    divIPPPendingPSDSentDate.Style("display") = ""
                            '    dtgIPPPendingPSDSentDate.CurrentPageIndex = 0
                            '    ViewState("SortAscendingIPPPendingPSDSentDate") = "yes"
                            '    ViewState("SortExpressionIPPPendingPSDSentDate") = ""
                            '    BindgridIPPPendingPSDSentDate()
                            'End If

                            'Modified for IPP GST 2A - CH - 9 Feb 2015
                            If objDB.Exist("SELECT '*' FROM MENU_MSTR " & _
                                       "INNER JOIN USER_ACCESS_RIGHT ON UAR_MENU_ID = MM_MENU_ID AND UAR_ALLOW_INSERT = 'Y'" & _
                                       "INNER JOIN USERS_USRGRP ON UU_USRGRP_ID= UAR_USRGRP_ID AND UU_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND UU_USER_ID='" & HttpContext.Current.Session("UserID") & "' " & _
                                       "INNER JOIN USER_MSTR ON UM_USER_ID= UU_USER_ID AND UM_USER_ID = '" & HttpContext.Current.Session("UserID") & "' " & _
                                       "WHERE MM_GROUP='ehub' AND UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "'  AND MM_MENU_ID = 130") Then
                                divIPPPendingPSDSentDate.Style("display") = ""
                                dtgIPPPendingPSDSentDate.CurrentPageIndex = 0
                                ViewState("SortAscendingIPPPendingPSDSentDate") = "yes"
                                ViewState("SortExpressionIPPPendingPSDSentDate") = ""
                                BindgridIPPPendingPSDSentDate()
                            End If
                            '--------------------------------------------
                        Case 19
                            'If objDB.Exist("SELECT '*' FROM MENU_MSTR " & _
                            '           "INNER JOIN USER_ACCESS_RIGHT ON UAR_MENU_ID = MM_MENU_ID AND UAR_ALLOW_INSERT = 'Y'" & _
                            '           "INNER JOIN USERS_USRGRP ON UU_USRGRP_ID= UAR_USRGRP_ID AND UU_COY_ID='hlb' AND UU_USER_ID='" & HttpContext.Current.Session("UserID") & "' " & _
                            '           "INNER JOIN USER_MSTR ON UM_USER_ID= UU_USER_ID AND UM_USER_ID = '" & HttpContext.Current.Session("UserID") & "' " & _
                            '           "WHERE MM_GROUP='ehub'   AND UM_COY_ID='hlb'  AND MM_MENU_ID = 131") Then
                            '    divIPPPendingPSDRecvDate.Style("display") = ""
                            '    dtgIPPPendingPSDRecvDate.CurrentPageIndex = 0
                            '    ViewState("SortAscendingIPPPendingPSDRecvDate") = "yes"
                            '    ViewState("SortExpressionIPPPendingPSDRecvDate") = ""
                            '    BindgridIPPPendingPSDRecvDate()
                            'End If
                            'Modified for IPP GST 2A - CH - 12 Feb 2015
                            If objDB.Exist("SELECT '*' FROM MENU_MSTR " & _
                                       "INNER JOIN USER_ACCESS_RIGHT ON UAR_MENU_ID = MM_MENU_ID AND UAR_ALLOW_INSERT = 'Y'" & _
                                       "INNER JOIN USERS_USRGRP ON UU_USRGRP_ID= UAR_USRGRP_ID AND UU_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND UU_USER_ID='" & HttpContext.Current.Session("UserID") & "' " & _
                                       "INNER JOIN USER_MSTR ON UM_USER_ID= UU_USER_ID AND UM_USER_ID = '" & HttpContext.Current.Session("UserID") & "' " & _
                                       "WHERE MM_GROUP='ehub'   AND UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "'  AND MM_MENU_ID = 131") Then
                                divIPPPendingPSDRecvDate.Style("display") = ""
                                dtgIPPPendingPSDRecvDate.CurrentPageIndex = 0
                                ViewState("SortAscendingIPPPendingPSDRecvDate") = "yes"
                                ViewState("SortExpressionIPPPendingPSDRecvDate") = ""
                                BindgridIPPPendingPSDRecvDate()
                            End If
                        Case 20
                            divIQCApproval.Style("display") = ""
                            dtgIQCApproval.CurrentPageIndex = 0
                            ViewState("SortAscendingIQCApproval") = "yes"
                            ViewState("SortExpressionIQCApproval") = ""
                            BindgridIQCApproval()
                        Case 21
                            divOutstandingIR.Style("display") = ""
                            dtgOutstandingIR.CurrentPageIndex = 0
                            ViewState("SortAscendingOutstandingIR") = "yes"
                            ViewState("SortExpressionOutstandingIR") = "IRM_IR_DATE"
                            BindgridOutstandingIR()
                        Case 22
                            divPendingMRSAcknowledge.Style("display") = ""
                            dtgPendingMRSAcknowledge.CurrentPageIndex = 0
                            ViewState("SortAscendingPendingMRSAcknowledge") = "yes"
                            ViewState("SortExpressionPendingMRSAcknowledge") = "IRSM_IRS_DATE"
                            BindgridPendingMRSAcknowledge()
                        Case 23
                            divPendingMyIRApproval.Style("display") = ""
                            dtgPendingMyIRApproval.CurrentPageIndex = 0
                            ViewState("SortAscendingPendingMyIRApproval") = "yes"
                            ViewState("SortExpressionPendingMyIRApproval") = "IRM_IR_DATE"
                            BindgridPendingMyIRApproval()
                        Case 24
                            divIssueMRS.Style("display") = ""
                            dtgIssueMRS.CurrentPageIndex = 0
                            ViewState("SortAscendingIssueMRS") = "yes"
                            ViewState("SortExpressionIssueMRS") = "IRSM_IRS_DATE"
                            BindgridIssueMRS()
                        Case 25
                            divOutRIAck.Style("display") = ""
                            dtgOutRIAck.CurrentPageIndex = 0
                            ViewState("SortAscendingOutRIAck") = "yes"
                            ViewState("SortExpressionOutRIAck") = ""
                            BindgridOutRIAck()
                            'Modified for IPP GST Stage 2A - CH - 12 Feb 2015
                        Case 26
                            If objDB.Exist("SELECT '*' FROM MENU_MSTR " & _
                                       "INNER JOIN USER_ACCESS_RIGHT ON UAR_MENU_ID = MM_MENU_ID AND UAR_ALLOW_INSERT = 'Y'" & _
                                       "INNER JOIN USERS_USRGRP ON UU_USRGRP_ID= UAR_USRGRP_ID AND UU_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND UU_USER_ID='" & HttpContext.Current.Session("UserID") & "' " & _
                                       "INNER JOIN USER_MSTR ON UM_USER_ID= UU_USER_ID AND UM_USER_ID = '" & HttpContext.Current.Session("UserID") & "' " & _
                                       "WHERE MM_GROUP='ehub'   AND UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "'  AND MM_MENU_ID = 155") Then
                                divOutBillDoc.Style("display") = ""
                                dtgOutBillDoc.CurrentPageIndex = 0
                                ViewState("SortAscendingOutBillDoc") = "yes"
                                ViewState("SortExpressionOutBillDoc") = ""
                                BindgridOutBillDoc()
                            End If
                            'divOutBillDoc.Style("display") = ""
                            'dtgOutBillDoc.CurrentPageIndex = 0
                            'ViewState("SortAscendingOutBillDoc") = "yes"
                            'ViewState("SortExpressionOutBillDoc") = ""
                            'BindgridOutBillDoc()
                        Case 27
                            divPendingBillApproval.Style("display") = ""
                            dtgPendingBillApproval.CurrentPageIndex = 0
                            ViewState("SortAscendingPendingBillApproval") = "yes"
                            ViewState("SortExpressionPendingBillApproval") = ""
                            BindgridPendingBillApproval()
                            '-------------------------------

                            'Yap: 2015-02-27: Modified for Agora GST Stage 2
                        Case 28
                            'Jules 2018.08.06 - Need to check whether user actually has access rights, not just based on Fixed Role.
                            If objDB.Exist("SELECT '*' FROM MENU_MSTR " &
                                        "INNER JOIN USER_ACCESS_RIGHT ON UAR_MENU_ID = MM_MENU_ID AND UAR_ALLOW_INSERT = 'Y'" &
                                        "INNER JOIN USERS_USRGRP ON UU_USRGRP_ID= UAR_USRGRP_ID AND UU_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND UU_USER_ID='" & HttpContext.Current.Session("UserID") & "' " &
                                        "INNER JOIN USER_MSTR ON UM_USER_ID= UU_USER_ID AND UM_USER_ID = '" & HttpContext.Current.Session("UserID") & "' " &
                                        "WHERE MM_GROUP='ehub' AND UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND MM_MENU_NAME='Debit Note' AND UAR_DELETE_IND = 'N' ") Then
                                divFOIncomingDN.Style("display") = ""
                                dtgFOIncomingDN.CurrentPageIndex = 0
                                ViewState("SortAscendingFOIncomingDN") = "yes"
                                ViewState("SortExpressionFOIncomingDN") = ""
                                BindgridFOIncomingDN()
                            End If
                            'End modification.
                        Case 29
                            'Jules 2018.08.03 - Need to check whether user actually has access rights, not just based on Fixed Role.
                            If objDB.Exist("SELECT '*' FROM MENU_MSTR " &
                                        "INNER JOIN USER_ACCESS_RIGHT ON UAR_MENU_ID = MM_MENU_ID AND UAR_ALLOW_INSERT = 'Y'" &
                                        "INNER JOIN USERS_USRGRP ON UU_USRGRP_ID= UAR_USRGRP_ID AND UU_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND UU_USER_ID='" & HttpContext.Current.Session("UserID") & "' " &
                                        "INNER JOIN USER_MSTR ON UM_USER_ID= UU_USER_ID AND UM_USER_ID = '" & HttpContext.Current.Session("UserID") & "' " &
                                        "WHERE MM_GROUP='ehub' AND UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND MM_MENU_NAME LIKE '%Debit%' AND UAR_DELETE_IND = 'N' ") Then
                                divFMIncomingPendingDN.Style("display") = ""
                                dtgFMIncomingPendingDN.CurrentPageIndex = 0
                                ViewState("SortAscendingFMIncomingPendingDN") = "yes"
                                ViewState("SortExpressionFMIncomingPendingDN") = ""
                                BindgridFMIncomingPendingDN()
                            End If
                            'End modification.
                        Case 30
                            'Jules 2018.08.03 - Need to check whether user actually has access rights, not just based on Fixed Role.
                            If objDB.Exist("SELECT '*' FROM MENU_MSTR " &
                                        "INNER JOIN USER_ACCESS_RIGHT ON UAR_MENU_ID = MM_MENU_ID AND UAR_ALLOW_INSERT = 'Y'" &
                                        "INNER JOIN USERS_USRGRP ON UU_USRGRP_ID= UAR_USRGRP_ID AND UU_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND UU_USER_ID='" & HttpContext.Current.Session("UserID") & "' " &
                                        "INNER JOIN USER_MSTR ON UM_USER_ID= UU_USER_ID AND UM_USER_ID = '" & HttpContext.Current.Session("UserID") & "' " &
                                        "WHERE MM_GROUP='ehub' AND UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND MM_MENU_NAME LIKE '%Credit%' AND UAR_DELETE_IND = 'N' ") Then
                                divFMPendingAckCN.Style("display") = ""
                                dtgFMPendingAckCN.CurrentPageIndex = 0
                                ViewState("SortAscendingFMPendingAckCN") = "yes"
                                ViewState("SortExpressionFMPendingAckCN") = ""
                                BindgridFMPendingAckCN()
                            End If
                            'End modification.
                    End Select
                Next
            End If
        Next

    End Sub
    Private Function BindgridPendingApprPM(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objDO As New Dashboard

        '//Retrieve Data from Database
        Dim dsPendingApprPM As DataSet = New DataSet
        dsPendingApprPM = objDO.GetPendingAppr2Ent("Buyer")

        '//for sorting asc or desc
        Dim dvViewPendingApprPM As DataView
        dvViewPendingApprPM = dsPendingApprPM.Tables(0).DefaultView
        dvViewPendingApprPM.Sort = ViewState("SortExpressionPendingApprPM")
        If ViewState("SortAscendingPendingApprPM") = "no" And ViewState("SortExpressionPendingApprPM") <> "" Then dvViewPendingApprPM.Sort += " DESC"
        If ViewState("actionPendingApprPM") = "del" Then
            If dtgPendingApprPM.CurrentPageIndex > 0 And dsPendingApprPM.Tables(0).Rows.Count Mod dtgPendingApprPM.PageSize = 0 Then
                dtgPendingApprPM.CurrentPageIndex = dtgPendingApprPM.CurrentPageIndex - 1
                ViewState("actionPendingApprPM") = ""
            End If
        End If
        intTotRecord = dsPendingApprPM.Tables(0).Rows.Count
        Session("PageRecordPendingApprPM") = intTotRecord
        resetDashboardDatagridPageIndex(dtgPendingApprPM, dvViewPendingApprPM, "PendingApprPM")
        dtgPendingApprPM.DataSource = dvViewPendingApprPM
        dtgPendingApprPM.DataBind()

        If intTotRecord = 0 Then
            dtgPendingApprPM.ShowHeader = False
            dtgPendingApprPM.PagerStyle.CssClass = "db_displaynone"
        End If

        ' add for above checking
        ViewState("PageCountPendingApprPM") = dtgPendingApprPM.PageCount
    End Function
    Private Function BindgridPendingMyAppr(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objDO As New Dashboard

        '//Retrieve Data from Database
        Dim dsPendingMyAppr As DataSet = New DataSet
        dsPendingMyAppr = objDO.GetPendingAppr2Ent("AO")

        '//for sorting asc or desc
        Dim dvViewPendingMyAppr As DataView
        dvViewPendingMyAppr = dsPendingMyAppr.Tables(0).DefaultView
        dvViewPendingMyAppr.Sort = ViewState("SortExpressionPendingMyAppr")
        If ViewState("SortAscendingPendingMyAppr") = "no" And ViewState("SortExpressionPendingMyAppr") <> "" Then dvViewPendingMyAppr.Sort += " DESC"
        If ViewState("actionPendingMyAppr") = "del" Then
            If dtgPendingMyAppr.CurrentPageIndex > 0 And dsPendingMyAppr.Tables(0).Rows.Count Mod dtgPendingMyAppr.PageSize = 0 Then
                dtgPendingMyAppr.CurrentPageIndex = dtgPendingMyAppr.CurrentPageIndex - 1
                ViewState("actionPendingMyAppr") = ""
            End If
        End If
        intTotRecord = dsPendingMyAppr.Tables(0).Rows.Count
        Session("PageRecordPendingMyAppr") = intTotRecord
        resetDashboardDatagridPageIndex(dtgPendingMyAppr, dvViewPendingMyAppr, "PendingMyAppr")
        dtgPendingMyAppr.DataSource = dvViewPendingMyAppr
        dtgPendingMyAppr.DataBind()

        If intTotRecord = 0 Then
            dtgPendingMyAppr.ShowHeader = False
            dtgPendingMyAppr.PagerStyle.CssClass = "db_displaynone"
        End If

        ' add for above checking
        ViewState("PageCountPendingMyAppr") = dtgPendingMyAppr.PageCount
    End Function
    Private Function BindgridOutstdPR(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objDO As New Dashboard

        '//Retrieve Data from Database
        Dim dsOutstdPR As DataSet = New DataSet
        dsOutstdPR = objDO.GetPendingApprPR("BUYER")

        '//for sorting asc or desc
        Dim dvViewOutstdPR As DataView
        dvViewOutstdPR = dsOutstdPR.Tables(0).DefaultView
        dvViewOutstdPR.Sort = ViewState("SortExpressionOutstdPR")
        If ViewState("SortAscendingOutstdPR") = "no" And ViewState("SortExpressionOutstdPR") <> "" Then dvViewOutstdPR.Sort += " DESC"
        If ViewState("actionOutstdPR") = "del" Then
            If dtgOutstdPR.CurrentPageIndex > 0 And dsOutstdPR.Tables(0).Rows.Count Mod dtgOutstdPR.PageSize = 0 Then
                dtgOutstdPR.CurrentPageIndex = dtgOutstdPR.CurrentPageIndex - 1
                ViewState("actionOutstdPR") = ""
            End If
        End If
        intTotRecord = dsOutstdPR.Tables(0).Rows.Count
        Session("PageRecordOutstdPR") = intTotRecord
        resetDashboardDatagridPageIndex(dtgOutstdPR, dvViewOutstdPR, "OutstdPR")
        dtgOutstdPR.DataSource = dvViewOutstdPR
        dtgOutstdPR.DataBind()

        If intTotRecord = 0 Then
            dtgOutstdPR.ShowHeader = False
            dtgOutstdPR.PagerStyle.CssClass = "db_displaynone"
        End If

        ' add for above checking
        ViewState("PageCountOutstdPR") = dtgOutstdPR.PageCount
    End Function
    Private Function BindgridPendingMyAppPR(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objDO As New Dashboard

        '//Retrieve Data from Database
        Dim dsPendingMyAppPR As DataSet = New DataSet
        dsPendingMyAppPR = objDO.GetPendingApprPR("AO")

        '//for sorting asc or desc
        Dim dvPendingMyAppPR As DataView
        dvPendingMyAppPR = dsPendingMyAppPR.Tables(0).DefaultView
        dvPendingMyAppPR.Sort = ViewState("SortExpressionPendingMyAppPR")
        If ViewState("SortAscendingPendingMyAppPR") = "no" And ViewState("SortExpressionPendingMyAppPR") <> "" Then dvPendingMyAppPR.Sort += " DESC"
        If ViewState("actionPendingMyAppPR") = "del" Then
            If dtgPendingMyAppPR.CurrentPageIndex > 0 And dsPendingMyAppPR.Tables(0).Rows.Count Mod dtgPendingMyAppPR.PageSize = 0 Then
                dtgPendingMyAppPR.CurrentPageIndex = dtgPendingMyAppPR.CurrentPageIndex - 1
                ViewState("actionPendingMyAppPR") = ""
            End If
        End If
        intTotRecord = dsPendingMyAppPR.Tables(0).Rows.Count
        Session("PageRecordPendingMyAppPR") = intTotRecord
        resetDashboardDatagridPageIndex(dtgPendingMyAppPR, dvPendingMyAppPR, "PendingMyAppPR")
        dtgPendingMyAppPR.DataSource = dvPendingMyAppPR
        dtgPendingMyAppPR.DataBind()

        If intTotRecord = 0 Then
            dtgPendingMyAppPR.ShowHeader = False
            dtgPendingMyAppPR.PagerStyle.CssClass = "db_displaynone"
        End If

        ' add for above checking
        ViewState("PageCountPendingMyAppPR") = dtgPendingMyAppPR.PageCount
    End Function

    Private Function BindgridPendingConvPR(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objDO As New Dashboard

        '//Retrieve Data from Database
        Dim dsPendingConvPR As DataSet = New DataSet
        dsPendingConvPR = objDO.GetPendingApprPR("PO")

        '//for sorting asc or desc
        Dim dvPendingConvPR As DataView
        dvPendingConvPR = dsPendingConvPR.Tables(0).DefaultView
        dvPendingConvPR.Sort = ViewState("SortExpressionPendingConvPR")
        If ViewState("SortAscendingPendingConvPR") = "no" And ViewState("SortExpressionPendingConvPR") <> "" Then dvPendingConvPR.Sort += " DESC"
        If ViewState("actionPendingConvPR") = "del" Then
            If dtgPendingConvPR.CurrentPageIndex > 0 And dsPendingConvPR.Tables(0).Rows.Count Mod dtgPendingConvPR.PageSize = 0 Then
                dtgPendingConvPR.CurrentPageIndex = dtgPendingConvPR.CurrentPageIndex - 1
                ViewState("actionPendingConvPR") = ""
            End If
        End If
        intTotRecord = dsPendingConvPR.Tables(0).Rows.Count
        Session("PageRecordPendingConvPR") = intTotRecord
        resetDashboardDatagridPageIndex(dtgPendingConvPR, dvPendingConvPR, "PendingConvPR")
        dtgPendingConvPR.DataSource = dvPendingConvPR
        dtgPendingConvPR.DataBind()

        If intTotRecord = 0 Then
            dtgPendingConvPR.ShowHeader = False
            dtgPendingConvPR.PagerStyle.CssClass = "db_displaynone"
        End If

        ' add for above checking
        ViewState("PageCountPendingConvPR") = dtgPendingConvPR.PageCount
    End Function


    Private Function BindgridOutstdPO(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objDO As New Dashboard

        '//Retrieve Data from Database
        Dim dsOutstdPO As DataSet = New DataSet
        dsOutstdPO = objDO.GetOutstdPO()

        '//for sorting asc or desc
        Dim dvViewOutstdPO As DataView
        dvViewOutstdPO = dsOutstdPO.Tables(0).DefaultView
        dvViewOutstdPO.Sort = ViewState("SortExpressionOutstdPO")
        If ViewState("SortAscendingOutstdPO") = "no" And ViewState("SortExpressionOutstdPO") <> "" Then dvViewOutstdPO.Sort += " DESC"
        If ViewState("actionOutstdPO") = "del" Then
            If dtgOutstdPO.CurrentPageIndex > 0 And dsOutstdPO.Tables(0).Rows.Count Mod dtgOutstdPO.PageSize = 0 Then
                dtgOutstdPO.CurrentPageIndex = dtgOutstdPO.CurrentPageIndex - 1
                ViewState("actionOutstdPO") = ""
            End If
        End If
        intTotRecord = dsOutstdPO.Tables(0).Rows.Count
        Session("PageRecordOutstdPO") = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind
        resetDashboardDatagridPageIndex(dtgOutstdPO, dvViewOutstdPO, "OutstdPO")
        dtgOutstdPO.DataSource = dvViewOutstdPO
        dtgOutstdPO.DataBind()

        If intTotRecord = 0 Then
            dtgOutstdPO.ShowHeader = False
            dtgOutstdPO.PagerStyle.CssClass = "db_displaynone"
        End If
        ' add for above checking
        ViewState("PageCountOutstdPO") = dtgOutstdPO.PageCount
    End Function
    Private Function BindgridOutstandingRFQ(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objDO As New Dashboard

        '//Retrieve Data from Database
        Dim dsOutStandingRFQ As DataSet = New DataSet
        dsOutStandingRFQ = objDO.GetOutstandingRFQBuyer()

        '//for sorting asc or desc
        Dim dvViewOutStandingRFQ As DataView
        dvViewOutStandingRFQ = dsOutStandingRFQ.Tables(0).DefaultView
        dvViewOutStandingRFQ.Sort = ViewState("SortExpressionOutstandingRFQ")
        If ViewState("SortAscendingOutstandingRFQ") = "no" And ViewState("SortExpressionOutstandingRFQ") <> "" Then dvViewOutStandingRFQ.Sort += " DESC"
        If ViewState("actionOutstandingRFQ") = "del" Then
            If dtgOutstandingRFQ.CurrentPageIndex > 0 And dsOutStandingRFQ.Tables(0).Rows.Count Mod dtgOutstandingRFQ.PageSize = 0 Then
                dtgOutstandingRFQ.CurrentPageIndex = dtgOutstandingRFQ.CurrentPageIndex - 1
                ViewState("actionOutstandingRFQ") = ""
            End If
        End If
        intTotRecord = dsOutStandingRFQ.Tables(0).Rows.Count
        'intPageRecordCnt3 = intTotRecord
        Session("PageRecordOutstandingRFQ") = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind
        resetDashboardDatagridPageIndex(dtgOutstandingRFQ, dvViewOutStandingRFQ, "OutstandingRFQ")
        dtgOutstandingRFQ.DataSource = dvViewOutStandingRFQ
        dtgOutstandingRFQ.DataBind()
        If intTotRecord = 0 Then
            dtgOutstandingRFQ.ShowHeader = False
            dtgOutstandingRFQ.PagerStyle.CssClass = "db_displaynone"
        End If
        ' add for above checking
        ViewState("PageCountOutstandingRFQ") = dtgOutstandingRFQ.PageCount
    End Function
    Private Function BindgridInPendingPymt(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objDO As New Dashboard

        '//Retrieve Data from Database
        Dim dsInPendingPymt As DataSet = New DataSet

        dsInPendingPymt = objDO.GetInPendingPymt()

        '//for sorting asc or desc
        Dim dvViewInPendingPymt As DataView
        dvViewInPendingPymt = dsInPendingPymt.Tables(0).DefaultView
        dvViewInPendingPymt.Sort = ViewState("SortExpressionInPendingPymt")
        If ViewState("SortAscendingInPendingPymt") = "no" And ViewState("SortExpressionInPendingPymt") <> "" Then dvViewInPendingPymt.Sort += " DESC"
        If ViewState("actionInPendingPymt") = "del" Then
            If dtgInPendingPymt.CurrentPageIndex > 0 And dsInPendingPymt.Tables(0).Rows.Count Mod dtgInPendingPymt.PageSize = 0 Then
                dtgInPendingPymt.CurrentPageIndex = dtgInPendingPymt.CurrentPageIndex - 1
                ViewState("actionInPendingPymt") = ""
            End If
        End If
        intTotRecord = dsInPendingPymt.Tables(0).Rows.Count
        Session("PageRecordInPendingPymt") = intTotRecord

        resetDashboardDatagridPageIndex(dtgInPendingPymt, dvViewInPendingPymt, "InPendingPymt")
        dtgInPendingPymt.DataSource = dvViewInPendingPymt
        dtgInPendingPymt.DataBind()
        If intTotRecord = 0 Then
            dtgInPendingPymt.ShowHeader = False
            dtgInPendingPymt.PagerStyle.CssClass = "db_displaynone"
        End If
        ' add for above checking
        ViewState("PageCountInPendingPymt") = dtgInPendingPymt.PageCount
    End Function
    Private Function BindgridInInv(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objDO As New Dashboard

        'Zulham 17012019
        Dim objIPPMain As New IPPMain
        If ViewState("role") = 2 Then
            objIPPMain.updateFinanceApproval(Common.Parse(Session("UserID")))
        End If

        '//Retrieve Data from Database
        Dim dsInInv As DataSet = New DataSet
        dsInInv = objDO.GetInInv()

        '//for sorting asc or desc
        Dim dvViewInInv As DataView
        dvViewInInv = dsInInv.Tables(0).DefaultView
        dvViewInInv.Sort = ViewState("SortExpressionInInv")
        If ViewState("SortAscendingInInv") = "no" And ViewState("SortExpressionInInv") <> "" Then dvViewInInv.Sort += " DESC"
        If ViewState("actionInInv") = "del" Then
            If dtgInInv.CurrentPageIndex > 0 And dsInInv.Tables(0).Rows.Count Mod dtgInInv.PageSize = 0 Then
                dtgInInv.CurrentPageIndex = dtgInInv.CurrentPageIndex - 1
                ViewState("actionInInv") = ""
            End If
        End If
        intTotRecord = dsInInv.Tables(0).Rows.Count
        'intPageRecordCnt4 = intTotRecord
        Session("PageRecordInInv") = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind

        resetDashboardDatagridPageIndex(dtgInInv, dvViewInInv, "InPendingPymt")
        dtgInInv.DataSource = dvViewInInv
        dtgInInv.DataBind()
        If intTotRecord = 0 Then
            dtgInInv.ShowHeader = False
            dtgInInv.PagerStyle.CssClass = "db_displaynone"
        End If
        ' add for above checking
        ViewState("PageCountInInv") = dtgInInv.PageCount
    End Function
    '### StoreKeeper ###
    Private Function BindgridInDOSK(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objDO As New Dashboard

        '//Retrieve Data from Database
        Dim dsInDO As DataSet = New DataSet
        dsInDO = objDO.GetInDO()

        '//for sorting asc or desc
        Dim dvViewInDO As DataView
        dvViewInDO = dsInDO.Tables(0).DefaultView
        dvViewInDO.Sort = ViewState("SortExpressionInDOSK")
        If ViewState("SortAscendingInDOSK") = "no" And ViewState("SortExpressionInDOSK") <> "" Then dvViewInDO.Sort += " DESC"
        If ViewState("actionInDOSK") = "del" Then
            If dtgInDOSK.CurrentPageIndex > 0 And dsInDO.Tables(0).Rows.Count Mod dtgInDOSK.PageSize = 0 Then
                dtgInDOSK.CurrentPageIndex = dtgInDOSK.CurrentPageIndex - 1
                ViewState("actionInDOSK") = ""
            End If
        End If
        intTotRecord = dsInDO.Tables(0).Rows.Count
        Session("PageRecordInDOSK") = intTotRecord
        resetDashboardDatagridPageIndex(dtgInDOSK, dvViewInDO, "InDOSK")
        dtgInDOSK.DataSource = dvViewInDO
        dtgInDOSK.DataBind()

        If intTotRecord = 0 Then
            dtgInDOSK.ShowHeader = False
            dtgInDOSK.PagerStyle.CssClass = "db_displaynone"
        End If

        ' add for above checking
        ViewState("PageCountInDOSK") = dtgInDOSK.PageCount
    End Function
    '### Vendor ###
    Private Function BindgridOutstandingPOVend(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objDO As New Dashboard

        '//Retrieve Data from Database
        Dim dsOutstandingPO As DataSet = New DataSet
        dsOutstandingPO = objDO.GetOutstandingPODash()

        '//for sorting asc or desc
        Dim dvViewOutstandingPO As DataView
        dvViewOutstandingPO = dsOutstandingPO.Tables(0).DefaultView
        dvViewOutstandingPO.Sort = ViewState("SortExpressionOutstandingPOVend")
        If ViewState("SortAscendingOutstandingPOVend") = "no" And ViewState("SortExpressionOutstandingPOVend") <> "" Then dvViewOutstandingPO.Sort += " DESC"
        If ViewState("actionOutstandingPOVend") = "del" Then
            If dtgOutstandingPOVend.CurrentPageIndex > 0 And dsOutstandingPO.Tables(0).Rows.Count Mod dtgOutstandingPOVend.PageSize = 0 Then
                dtgOutstandingPOVend.CurrentPageIndex = dtgOutstandingPOVend.CurrentPageIndex - 1
                ViewState("actionOutstandingPOVend") = ""
            End If
        End If
        intTotRecord = dsOutstandingPO.Tables(0).Rows.Count
        'intPageRecordCnt = intTotRecord
        Session("PageRecordOutstandingPOVend") = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind
        resetDashboardDatagridPageIndex(dtgOutstandingPOVend, dvViewOutstandingPO, "OutstandingPOVend")
        dtgOutstandingPOVend.DataSource = dvViewOutstandingPO
        dtgOutstandingPOVend.DataBind()

        If intTotRecord = 0 Then
            dtgOutstandingPOVend.ShowHeader = False
            dtgOutstandingPOVend.PagerStyle.CssClass = "db_displaynone"
        End If

        ' add for above checking
        ViewState("PageCountOutstandingPOVend") = dtgOutstandingPOVend.PageCount
    End Function

    Private Function BindgridOverduePOVend(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objDO As New Dashboard

        '//Retrieve Data from Database
        Dim dsOverduePO As DataSet = New DataSet
        dsOverduePO = objDO.GetOverduePODash()

        '//for sorting asc or desc
        Dim dvViewOverduePO As DataView
        dvViewOverduePO = dsOverduePO.Tables(0).DefaultView
        dvViewOverduePO.Sort = ViewState("SortExpressionOverduePOVend")
        If ViewState("SortAscendingOverduePOVend") = "no" And ViewState("SortExpressionOverduePOVend") <> "" Then dvViewOverduePO.Sort += " DESC"
        If ViewState("actionOverduePOVend") = "del" Then
            If dtgOverduePOVend.CurrentPageIndex > 0 And dsOverduePO.Tables(0).Rows.Count Mod dtgOverduePOVend.PageSize = 0 Then
                dtgOverduePOVend.CurrentPageIndex = dtgOverduePOVend.CurrentPageIndex - 1
                ViewState("actionOverduePOVend") = ""
            End If
        End If
        intTotRecord = dsOverduePO.Tables(0).Rows.Count
        Session("PageRecordOverduePOVend") = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind
        resetDashboardDatagridPageIndex(dtgOverduePOVend, dvViewOverduePO, "OverduePOVend")
        dtgOverduePOVend.DataSource = dvViewOverduePO
        dtgOverduePOVend.DataBind()

        If intTotRecord = 0 Then
            dtgOverduePOVend.ShowHeader = False
            dtgOverduePOVend.PagerStyle.CssClass = "db_displaynone"
        End If
        ' add for above checking
        ViewState("PageCountOverduePOVend") = dtgOverduePOVend.PageCount
    End Function

    Private Function BindgridOutstandingRFQVend(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objDO As New Dashboard

        '//Retrieve Data from Database
        Dim dsOutStandingRFQ As DataSet = New DataSet
        dsOutStandingRFQ = objDO.GetOutstandingRFQ()

        '//for sorting asc or desc
        Dim dvViewOutStandingRFQ As DataView
        dvViewOutStandingRFQ = dsOutStandingRFQ.Tables(0).DefaultView
        dvViewOutStandingRFQ.Sort = ViewState("SortExpressionOutstandingRFQVend")
        If ViewState("SortAscendingOutstandingRFQVend") = "no" And ViewState("SortExpressionOutstandingRFQVend") <> "" Then dvViewOutStandingRFQ.Sort += " DESC"
        If ViewState("actionOutstandingRFQVend") = "del" Then
            If dtgOutstandingRFQVend.CurrentPageIndex > 0 And dsOutStandingRFQ.Tables(0).Rows.Count Mod dtgOutstandingRFQVend.PageSize = 0 Then
                dtgOutstandingRFQVend.CurrentPageIndex = dtgOutstandingRFQVend.CurrentPageIndex - 1
                ViewState("actionOutstandingRFQVend") = ""
            End If
        End If
        intTotRecord = dsOutStandingRFQ.Tables(0).Rows.Count
        'intPageRecordCnt3 = intTotRecord
        Session("PageRecordOutstandingRFQVend") = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind
        resetDashboardDatagridPageIndex(dtgOutstandingRFQVend, dvViewOutStandingRFQ, "OutstandingRFQVend")
        dtgOutstandingRFQVend.DataSource = dvViewOutStandingRFQ
        dtgOutstandingRFQVend.DataBind()
        If intTotRecord = 0 Then
            dtgOutstandingRFQVend.ShowHeader = False
            dtgOutstandingRFQVend.PagerStyle.CssClass = "db_displaynone"
        End If
        ' add for above checking
        ViewState("PageCountOutstandingRFQVend") = dtgOutstandingRFQVend.PageCount
    End Function

    Private Function BindgridOutstandingInvoiceVend(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objDO As New Dashboard

        '//Retrieve Data from Database
        Dim dsOutStandingInvoice As DataSet = New DataSet
        dsOutStandingInvoice = objDO.GetOutstandingInvoice()

        '//for sorting asc or desc
        Dim dvViewOutStandingInvoice As DataView
        dvViewOutStandingInvoice = dsOutStandingInvoice.Tables(0).DefaultView
        dvViewOutStandingInvoice.Sort = ViewState("SortExpressionOutstandingInvoiceVend")
        If ViewState("SortAscendingOutstandingInvoiceVend") = "no" And ViewState("SortExpressionOutstandingInvoiceVend") <> "" Then dvViewOutStandingInvoice.Sort += " DESC"
        If ViewState("actionOutstandingInvoiceVend") = "del" Then
            If dtgOutstandingInvoiceVend.CurrentPageIndex > 0 And dsOutStandingInvoice.Tables(0).Rows.Count Mod dtgOutstandingInvoiceVend.PageSize = 0 Then
                dtgOutstandingInvoiceVend.CurrentPageIndex = dtgOutstandingInvoiceVend.CurrentPageIndex - 1
                ViewState("actionOutstandingInvoiceVend") = ""
            End If
        End If
        intTotRecord = dsOutStandingInvoice.Tables(0).Rows.Count
        'intPageRecordCnt4 = intTotRecord
        Session("PageRecordOutstandingInvoice") = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind

        resetDashboardDatagridPageIndex(dtgOutstandingInvoiceVend, dvViewOutStandingInvoice, "OutstandingInvoiceVend")
        dtgOutstandingInvoiceVend.DataSource = dvViewOutStandingInvoice
        dtgOutstandingInvoiceVend.DataBind()
        If intTotRecord = 0 Then
            dtgOutstandingInvoiceVend.ShowHeader = False
            dtgOutstandingInvoiceVend.PagerStyle.CssClass = "db_displaynone"
        End If
        ' add for above checking
        ViewState("PageCountOutstandingInvoiceVend") = dtgOutstandingInvoiceVend.PageCount
    End Function

    Private Function BindgridOutstandingGRNQCVerify(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objDO As New Inventory

        '//Retrieve Data from Database
        Dim dsOutstandingGRNQCVerify As DataSet = New DataSet
        dsOutstandingGRNQCVerify = objDO.GetInvVerify()

        '//for sorting asc or desc
        Dim dvOutstandingGRNQCVerify As DataView
        dvOutstandingGRNQCVerify = dsOutstandingGRNQCVerify.Tables(0).DefaultView
        dvOutstandingGRNQCVerify.Sort = ViewState("SortExpressionOutstandingGRNQCVerify")
        If ViewState("SortAscendingOutstandingGRNQCVerify") = "no" And ViewState("SortExpressionOutstandingGRNQCVerify") <> "" Then dvOutstandingGRNQCVerify.Sort += " DESC"
        If ViewState("actionOutstandingGRNQCVerify") = "del" Then
            If dtgOutstdGRNQCVerify.CurrentPageIndex > 0 And dsOutstandingGRNQCVerify.Tables(0).Rows.Count Mod dtgOutstdGRNQCVerify.PageSize = 0 Then
                dtgOutstandingInvoiceVend.CurrentPageIndex = dtgOutstandingInvoiceVend.CurrentPageIndex - 1
                ViewState("actionOutstandingGRNQCVerify") = ""
            End If
        End If
        intTotRecord = dsOutstandingGRNQCVerify.Tables(0).Rows.Count
        'intPageRecordCnt4 = intTotRecord
        Session("PageRecordOutstandingGRNQCVerify") = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind

        resetDashboardDatagridPageIndex(dtgOutstdGRNQCVerify, dvOutstandingGRNQCVerify, "OutstandingGRNQCVerify")
        dtgOutstdGRNQCVerify.DataSource = dvOutstandingGRNQCVerify
        dtgOutstdGRNQCVerify.DataBind()
        If intTotRecord = 0 Then
            dtgOutstdGRNQCVerify.ShowHeader = False
            dtgOutstdGRNQCVerify.PagerStyle.CssClass = "db_displaynone"
        End If
        ' add for above checking
        ViewState("PageCountOutstandingGRNQCVerify") = dtgOutstdGRNQCVerify.PageCount
    End Function
    Private Function BindgridOutstdIPPDoc(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objdash As New Dashboard

        '//Retrieve Data from Database
        Dim dsOutstdIPPDoc As DataSet = New DataSet
        dsOutstdIPPDoc = objdash.getOutstdIPPDoc()

        '//for sorting asc or desc
        Dim dvOutstdIPPDoc As DataView
        dvOutstdIPPDoc = dsOutstdIPPDoc.Tables(0).DefaultView
        dvOutstdIPPDoc.Sort = ViewState("SortExpressionOutstdIPPDoc")
        If ViewState("SortAscendingOutstdIPPDoc") = "no" And ViewState("SortExpressionOutstdIPPDoc") <> "" Then dvOutstdIPPDoc.Sort += " DESC"
        If ViewState("actionOutstdIPPDoc") = "del" Then
            If dtgOutstdIPPDoc.CurrentPageIndex > 0 And dsOutstdIPPDoc.Tables(0).Rows.Count Mod dtgOutstdIPPDoc.PageSize = 0 Then
                dtgOutstdIPPDoc.CurrentPageIndex = dtgOutstdIPPDoc.CurrentPageIndex - 1
                ViewState("actionOutstdIPPDoc") = ""
            End If
        End If
        intTotRecord = dsOutstdIPPDoc.Tables(0).Rows.Count
        'intPageRecordCnt4 = intTotRecord
        Session("PageRecordOutstdIPPDoc") = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind

        resetDashboardDatagridPageIndex(dtgOutstdIPPDoc, dvOutstdIPPDoc, "OutstdIPPDoc")
        dtgOutstdIPPDoc.DataSource = dvOutstdIPPDoc
        dtgOutstdIPPDoc.DataBind()
        If intTotRecord = 0 Then
            dtgOutstdIPPDoc.ShowHeader = False
            dtgOutstdIPPDoc.PagerStyle.CssClass = "db_displaynone"
        End If
        ' add for above checking
        ViewState("PageCountOutstdIPPDoc") = dtgOutstdIPPDoc.PageCount
    End Function
    Private Function BindgridIPPApproval(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objdash As New Dashboard

        '//Retrieve Data from Database
        Dim dsIPPApproval As DataSet = New DataSet
        dsIPPApproval = objdash.getIPPAOApproval()



        '//for sorting asc or desc
        Dim dvIPPApproval As DataView
        dvIPPApproval = dsIPPApproval.Tables(0).DefaultView
        dvIPPApproval.Sort = ViewState("SortExpressionIPPApproval")
        If ViewState("SortAscendingIPPApproval") = "no" And ViewState("SortExpressionIPPApproval") <> "" Then dvIPPApproval.Sort += " DESC"
        If ViewState("actionIPPApproval") = "del" Then
            If dtgIPPApproval.CurrentPageIndex > 0 And dsIPPApproval.Tables(0).Rows.Count Mod dtgIPPApproval.PageSize = 0 Then
                dtgIPPApproval.CurrentPageIndex = dtgIPPApproval.CurrentPageIndex - 1
                ViewState("actionIPPApproval") = ""
            End If
        End If
        intTotRecord = dsIPPApproval.Tables(0).Rows.Count
        'intPageRecordCnt4 = intTotRecord
        Session("PageRecordIPPApproval") = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind

        resetDashboardDatagridPageIndex(dtgIPPApproval, dvIPPApproval, "IPPApproval")
        dtgIPPApproval.DataSource = dvIPPApproval
        dtgIPPApproval.DataBind()
        If intTotRecord = 0 Then
            dtgIPPApproval.ShowHeader = False
            dtgIPPApproval.PagerStyle.CssClass = "db_displaynone"
        End If
        ' add for above checking
        ViewState("PageCountIPPApproval") = dtgIPPApproval.PageCount
    End Function
    Private Function BindgridIPPPendingPSDSentDate(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objdash As New Dashboard

        '//Retrieve Data from Database
        Dim dsIPPPendingPSDSentDate As DataSet = New DataSet
        dsIPPPendingPSDSentDate = objdash.PopulatePSDSent()

        '//for sorting asc or desc
        Dim dvIPPPendingPSDSentDate As DataView
        dvIPPPendingPSDSentDate = dsIPPPendingPSDSentDate.Tables(0).DefaultView
        dvIPPPendingPSDSentDate.Sort = ViewState("SortExpressionIPPPendingPSDSentDate")
        If ViewState("SortAscendingIPPPendingPSDSentDate") = "no" And ViewState("SortExpressionIPPPendingPSDSentDate") <> "" Then dvIPPPendingPSDSentDate.Sort += " DESC"
        If ViewState("actionIPPPendingPSDSentDate") = "del" Then
            If dtgIPPPendingPSDSentDate.CurrentPageIndex > 0 And dsIPPPendingPSDSentDate.Tables(0).Rows.Count Mod dtgIPPPendingPSDSentDate.PageSize = 0 Then
                dtgIPPPendingPSDSentDate.CurrentPageIndex = dtgIPPPendingPSDSentDate.CurrentPageIndex - 1
                ViewState("actionIPPPendingPSDSentDate") = ""
            End If
        End If
        intTotRecord = dsIPPPendingPSDSentDate.Tables(0).Rows.Count
        'intPageRecordCnt4 = intTotRecord
        Session("PageRecordIPPPendingPSDSentDate") = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind

        resetDashboardDatagridPageIndex(dtgIPPPendingPSDSentDate, dvIPPPendingPSDSentDate, "IPPPendingPSDSentDate")
        dtgIPPPendingPSDSentDate.DataSource = dvIPPPendingPSDSentDate
        dtgIPPPendingPSDSentDate.DataBind()
        If intTotRecord = 0 Then
            dtgIPPPendingPSDSentDate.ShowHeader = False
            dtgIPPPendingPSDSentDate.PagerStyle.CssClass = "db_displaynone"
        End If
        ' add for above checking
        ViewState("PageCountIPPPendingPSDSentDate") = dtgIPPPendingPSDSentDate.PageCount
    End Function
    Private Function BindgridIPPPendingPSDRecvDate(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objdash As New Dashboard

        '//Retrieve Data from Database
        Dim dsIPPPendingPSDRecvDate As DataSet = New DataSet
        dsIPPPendingPSDRecvDate = objdash.PopulatePSDRecv()

        '//for sorting asc or desc
        Dim dvIPPPendingPSDRecvDate As DataView
        dvIPPPendingPSDRecvDate = dsIPPPendingPSDRecvDate.Tables(0).DefaultView
        dvIPPPendingPSDRecvDate.Sort = ViewState("SortExpressionIPPPendingPSDRecvDate")
        If ViewState("SortAscendingIPPPendingPSDRecvDate") = "no" And ViewState("SortExpressionIPPPendingPSDRecvDate") <> "" Then dvIPPPendingPSDRecvDate.Sort += " DESC"
        If ViewState("actionIPPPendingPSDRecvDate") = "del" Then
            If dtgIPPPendingPSDRecvDate.CurrentPageIndex > 0 And dsIPPPendingPSDRecvDate.Tables(0).Rows.Count Mod dtgIPPPendingPSDRecvDate.PageSize = 0 Then
                dtgIPPPendingPSDRecvDate.CurrentPageIndex = dtgIPPPendingPSDRecvDate.CurrentPageIndex - 1
                ViewState("actionIPPPendingPSDRecvDate") = ""
            End If
        End If
        intTotRecord = dsIPPPendingPSDRecvDate.Tables(0).Rows.Count
        'intPageRecordCnt4 = intTotRecord
        Session("PageRecordIPPPendingPSDRecvDate") = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind

        resetDashboardDatagridPageIndex(dtgIPPPendingPSDRecvDate, dvIPPPendingPSDRecvDate, "IPPPendingPSDRecvDate")
        dtgIPPPendingPSDRecvDate.DataSource = dvIPPPendingPSDRecvDate
        dtgIPPPendingPSDRecvDate.DataBind()
        If intTotRecord = 0 Then
            dtgIPPPendingPSDRecvDate.ShowHeader = False
            dtgIPPPendingPSDRecvDate.PagerStyle.CssClass = "db_displaynone"
        End If
        ' add for above checking
        ViewState("PageCountIPPPendingPSDRecvDate") = dtgIPPPendingPSDRecvDate.PageCount
    End Function
    Private Function BindgridIQCApproval(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objdash As New Dashboard

        '//Retrieve Data from Database
        Dim dsIQCApproval As DataSet = New DataSet
        dsIQCApproval = objdash.getOutstdIQCDoc()

        '//for sorting asc or desc
        Dim dvIQCApproval As DataView
        dvIQCApproval = dsIQCApproval.Tables(0).DefaultView
        dvIQCApproval.Sort = ViewState("SortExpressionIQCApproval")
        If ViewState("SortAscendingIQCApproval") = "no" And ViewState("SortExpressionIQCApproval") <> "" Then dvIQCApproval.Sort += " DESC"
        If ViewState("actionIQCApproval") = "del" Then
            If dtgIQCApproval.CurrentPageIndex > 0 And dsIQCApproval.Tables(0).Rows.Count Mod dtgIQCApproval.PageSize = 0 Then
                dtgIQCApproval.CurrentPageIndex = dtgIQCApproval.CurrentPageIndex - 1
                ViewState("actionIQCApproval") = ""
            End If
        End If
        intTotRecord = dsIQCApproval.Tables(0).Rows.Count
        'intPageRecordCnt4 = intTotRecord
        Session("PageRecordIQCApproval") = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind

        resetDashboardDatagridPageIndex(dtgIQCApproval, dvIQCApproval, "IQCApproval")
        dtgIQCApproval.DataSource = dvIQCApproval
        dtgIQCApproval.DataBind()
        If intTotRecord = 0 Then
            dtgIQCApproval.ShowHeader = False
            dtgIQCApproval.PagerStyle.CssClass = "db_displaynone"
        End If
        ' add for above checking
        ViewState("PageCountIQCApproval") = dtgIQCApproval.PageCount
    End Function
    Private Function BindgridOutstandingIR(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objdash As New Dashboard

        '//Retrieve Data from Database
        Dim dsOutstandingIR As DataSet = New DataSet
        dsOutstandingIR = objdash.getOutstdIRDoc()

        '//for sorting asc or desc
        Dim dvOutstandingIR As DataView
        dvOutstandingIR = dsOutstandingIR.Tables(0).DefaultView
        dvOutstandingIR.Sort = ViewState("SortExpressionOutstandingIR")
        If ViewState("SortAscendingOutstandingIR") = "no" And ViewState("SortExpressionOutstandingIR") <> "" Then dvOutstandingIR.Sort += " DESC"
        If ViewState("actionOutstandingIR") = "del" Then
            If dtgOutstandingIR.CurrentPageIndex > 0 And dsOutstandingIR.Tables(0).Rows.Count Mod dtgIQCApproval.PageSize = 0 Then
                dtgOutstandingIR.CurrentPageIndex = dtgOutstandingIR.CurrentPageIndex - 1
                ViewState("actionOutstandingIR") = ""
            End If
        End If
        intTotRecord = dsOutstandingIR.Tables(0).Rows.Count
        'intPageRecordCnt4 = intTotRecord
        Session("PageRecordOutstandingIR") = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind

        resetDashboardDatagridPageIndex(dtgOutstandingIR, dvOutstandingIR, "OutstandingIR")
        dtgOutstandingIR.DataSource = dvOutstandingIR
        dtgOutstandingIR.DataBind()
        If intTotRecord = 0 Then
            dtgOutstandingIR.ShowHeader = False
            dtgOutstandingIR.PagerStyle.CssClass = "db_displaynone"
        End If
        ' add for above checking
        ViewState("PageCountIQCApproval") = dtgOutstandingIR.PageCount
    End Function
    Private Function BindgridPendingMRSAcknowledge(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objdash As New Dashboard

        '//Retrieve Data from Database
        Dim dsPendingMRSAcknowledge As DataSet = New DataSet
        dsPendingMRSAcknowledge = objdash.getPendingMRSAck()

        '//for sorting asc or desc
        Dim dvPendingMRSAcknowledge As DataView
        dvPendingMRSAcknowledge = dsPendingMRSAcknowledge.Tables(0).DefaultView
        dvPendingMRSAcknowledge.Sort = ViewState("SortExpressionPendingMRSAcknowledge")
        If ViewState("SortAscendingPendingMRSAcknowledge") = "no" And ViewState("SortExpressionPendingMRSAcknowledge") <> "" Then dvPendingMRSAcknowledge.Sort += " DESC"
        If ViewState("actionPendingMRSAcknowledge") = "del" Then
            If dtgPendingMRSAcknowledge.CurrentPageIndex > 0 And dsPendingMRSAcknowledge.Tables(0).Rows.Count Mod dtgPendingMRSAcknowledge.PageSize = 0 Then
                dtgPendingMRSAcknowledge.CurrentPageIndex = dtgPendingMRSAcknowledge.CurrentPageIndex - 1
                ViewState("actionPendingMRSAcknowledge") = ""
            End If
        End If
        intTotRecord = dsPendingMRSAcknowledge.Tables(0).Rows.Count
        'intPageRecordCnt4 = intTotRecord
        Session("PageRecordPendingMRSAcknowledge") = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind

        resetDashboardDatagridPageIndex(dtgPendingMRSAcknowledge, dvPendingMRSAcknowledge, "PendingMRSAcknowledge")
        dtgPendingMRSAcknowledge.DataSource = dvPendingMRSAcknowledge
        dtgPendingMRSAcknowledge.DataBind()
        If intTotRecord = 0 Then
            dtgPendingMRSAcknowledge.ShowHeader = False
            dtgPendingMRSAcknowledge.PagerStyle.CssClass = "db_displaynone"
        End If
        ' add for above checking
        ViewState("PageCountPendingMRSAcknowledge") = dtgPendingMRSAcknowledge.PageCount
    End Function
    Private Function BindgridPendingMyIRApproval(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objdash As New Dashboard

        '//Retrieve Data from Database
        Dim dsPendingMyIRApproval As DataSet = New DataSet
        dsPendingMyIRApproval = objdash.getPendingMyIRApproval()

        '//for sorting asc or desc
        Dim dvPendingMyIRApproval As DataView
        dvPendingMyIRApproval = dsPendingMyIRApproval.Tables(0).DefaultView
        dvPendingMyIRApproval.Sort = ViewState("SortExpressionPendingMyIRApproval")
        If ViewState("SortAscendingPendingMyIRApproval") = "no" And ViewState("SortExpressionPendingMyIRApproval") <> "" Then dvPendingMyIRApproval.Sort += " DESC"
        If ViewState("actionPendingMyIRApproval") = "del" Then
            If dtgPendingMyIRApproval.CurrentPageIndex > 0 And dsPendingMyIRApproval.Tables(0).Rows.Count Mod dtgPendingMyIRApproval.PageSize = 0 Then
                dtgPendingMyIRApproval.CurrentPageIndex = dtgPendingMyIRApproval.CurrentPageIndex - 1
                ViewState("actionPendingMyIRApproval") = ""
            End If
        End If
        intTotRecord = dsPendingMyIRApproval.Tables(0).Rows.Count
        'intPageRecordCnt4 = intTotRecord
        Session("PageRecordPendingMyIRApproval") = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind

        resetDashboardDatagridPageIndex(dtgPendingMyIRApproval, dvPendingMyIRApproval, "PendingMyIRApproval")
        dtgPendingMyIRApproval.DataSource = dvPendingMyIRApproval
        dtgPendingMyIRApproval.DataBind()
        If intTotRecord = 0 Then
            dtgPendingMyIRApproval.ShowHeader = False
            dtgPendingMyIRApproval.PagerStyle.CssClass = "db_displaynone"
        End If
        ' add for above checking
        ViewState("PageCountPendingMyIRApproval") = dtgPendingMyIRApproval.PageCount
    End Function
    Private Function BindgridIssueMRS(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objdash As New Dashboard

        '//Retrieve Data from Database
        Dim dsIssueMRS As DataSet = New DataSet
        dsIssueMRS = objdash.getIssueMRS()

        '//for sorting asc or desc
        Dim dvIssueMRS As DataView
        dvIssueMRS = dsIssueMRS.Tables(0).DefaultView
        dvIssueMRS.Sort = ViewState("SortExpressionIssueMRS")
        If ViewState("SortAscendingIssueMRS") = "no" And ViewState("SortExpressionIssueMRS") <> "" Then dvIssueMRS.Sort += " DESC"
        If ViewState("actionIssueMRS") = "del" Then
            If dtgIssueMRS.CurrentPageIndex > 0 And dsIssueMRS.Tables(0).Rows.Count Mod dtgIssueMRS.PageSize = 0 Then
                dtgIssueMRS.CurrentPageIndex = dtgIssueMRS.CurrentPageIndex - 1
                ViewState("actionIssueMRS") = ""
            End If
        End If
        intTotRecord = dsIssueMRS.Tables(0).Rows.Count
        'intPageRecordCnt4 = intTotRecord
        Session("PageRecordIssueMRS") = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind

        resetDashboardDatagridPageIndex(dtgIssueMRS, dvIssueMRS, "IssueMRS")
        dtgIssueMRS.DataSource = dvIssueMRS
        dtgIssueMRS.DataBind()
        If intTotRecord = 0 Then
            dtgIssueMRS.ShowHeader = False
            dtgIssueMRS.PagerStyle.CssClass = "db_displaynone"
        End If
        ' add for above checking
        ViewState("PageCountIssueMRS") = dtgIssueMRS.PageCount
    End Function

    Private Function BindgridOutRIAck(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objdash As New Dashboard

        '//Retrieve Data from Database
        Dim dsOutRIAck As DataSet = New DataSet
        dsOutRIAck = objdash.getOutRIAck()

        '//for sorting asc or desc
        Dim dvOutRIAck As DataView
        dvOutRIAck = dsOutRIAck.Tables(0).DefaultView
        dvOutRIAck.Sort = ViewState("SortExpressionOutRIAck")
        If ViewState("SortAscendingOutRIAck") = "no" And ViewState("SortExpressionOutRIAck") <> "" Then dvOutRIAck.Sort += " DESC"
        If ViewState("actionOutRIAck") = "del" Then
            If dtgOutRIAck.CurrentPageIndex > 0 And dsOutRIAck.Tables(0).Rows.Count Mod dtgOutRIAck.PageSize = 0 Then
                dtgOutRIAck.CurrentPageIndex = dtgOutRIAck.CurrentPageIndex - 1
                ViewState("actionOutRIAck") = ""
            End If
        End If
        intTotRecord = dsOutRIAck.Tables(0).Rows.Count
        'intPageRecordCnt4 = intTotRecord
        Session("PageRecordOutRIAck") = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind

        resetDashboardDatagridPageIndex(dtgOutRIAck, dvOutRIAck, "OutRIAck")
        dtgOutRIAck.DataSource = dvOutRIAck
        dtgOutRIAck.DataBind()
        If intTotRecord = 0 Then
            dtgOutRIAck.ShowHeader = False
            dtgOutRIAck.PagerStyle.CssClass = "db_displaynone"
        End If
        ' add for above checking
        ViewState("PageCountOutRIAck") = dtgOutRIAck.PageCount
    End Function
	'Modified for IPP GST Stage 2A
    Private Function BindgridOutBillDoc(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objdash As New Dashboard

        '//Retrieve Data from Database
        Dim dsOutBillDoc As DataSet = New DataSet
        dsOutBillDoc = objdash.getOutBillDoc()

        '//for sorting asc or desc
        Dim dvOutBillDoc As DataView
        dvOutBillDoc = dsOutBillDoc.Tables(0).DefaultView
        dvOutBillDoc.Sort = ViewState("SortExpressionOutBillDoc")
        If ViewState("SortAscendingOutBillDoc") = "no" And ViewState("SortExpressionOutBillDoc") <> "" Then dvOutBillDoc.Sort += " DESC"
        If ViewState("actionOutRIAck") = "del" Then
            If dtgOutBillDoc.CurrentPageIndex > 0 And dsOutBillDoc.Tables(0).Rows.Count Mod dtgOutBillDoc.PageSize = 0 Then
                dtgOutBillDoc.CurrentPageIndex = dtgOutBillDoc.CurrentPageIndex - 1
                ViewState("actionOutBillDoc") = ""
            End If
        End If
        intTotRecord = dsOutBillDoc.Tables(0).Rows.Count
        'intPageRecordCnt4 = intTotRecord
        Session("PageRecordOutBillDoc") = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind

        resetDashboardDatagridPageIndex(dtgOutBillDoc, dvOutBillDoc, "OutBillDoc")
        dtgOutBillDoc.DataSource = dvOutBillDoc
        dtgOutBillDoc.DataBind()
        If intTotRecord = 0 Then
            dtgOutBillDoc.ShowHeader = False
            dtgOutBillDoc.PagerStyle.CssClass = "db_displaynone"
        End If
        ' add for above checking
        ViewState("PageCountOutBillDoc") = dtgOutBillDoc.PageCount
    End Function

    Private Function BindgridPendingBillApproval(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objdash As New Dashboard

        '//Retrieve Data from Database
        Dim dsPendingBillApproval As DataSet = New DataSet
        dsPendingBillApproval = objdash.getPendingBillApproval()

        '//for sorting asc or desc
        Dim dvPendingBillApproval As DataView
        dvPendingBillApproval = dsPendingBillApproval.Tables(0).DefaultView
        dvPendingBillApproval.Sort = ViewState("SortExpressionPendingBillApproval")
        If ViewState("SortAscendingPendingBillApproval") = "no" And ViewState("SortExpressionPendingBillApproval") <> "" Then dvPendingBillApproval.Sort += " DESC"
        If ViewState("actionPendingBillApproval") = "del" Then
            If dtgPendingBillApproval.CurrentPageIndex > 0 And dsPendingBillApproval.Tables(0).Rows.Count Mod dtgPendingBillApproval.PageSize = 0 Then
                dtgPendingBillApproval.CurrentPageIndex = dtgPendingBillApproval.CurrentPageIndex - 1
                ViewState("actionPendingBillApproval") = ""
            End If
        End If
        intTotRecord = dsPendingBillApproval.Tables(0).Rows.Count
        'intPageRecordCnt4 = intTotRecord
        Session("PageRecordPendingBillApproval") = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind

        resetDashboardDatagridPageIndex(dtgOutBillDoc, dvPendingBillApproval, "PendingBillApproval")
        dtgPendingBillApproval.DataSource = dvPendingBillApproval
        dtgPendingBillApproval.DataBind()
        If intTotRecord = 0 Then
            dtgPendingBillApproval.ShowHeader = False
            dtgPendingBillApproval.PagerStyle.CssClass = "db_displaynone"
        End If
        ' add for above checking
        ViewState("PageCountPendingBillApproval") = dtgPendingBillApproval.PageCount
    End Function
    '--------------------------------------

    'Yap: 2015-02-27: Modified for Agora GST Stage 2
    Private Function BindgridFOIncomingDN(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objdash As New Dashboard

        '//Retrieve Data from Database
        Dim dsFOIncomingDN As DataSet = New DataSet
        dsFOIncomingDN = objdash.getFOIncomingDN()

        '//for sorting asc or desc
        Dim dvFOIncomingDN As DataView
        dvFOIncomingDN = dsFOIncomingDN.Tables(0).DefaultView
        dvFOIncomingDN.Sort = ViewState("SortExpressionFOIncomingDN")
        If ViewState("SortAscendingFOIncomingDN") = "no" And ViewState("SortExpressionFOIncomingDN") <> "" Then dvFOIncomingDN.Sort += " DESC"
        If ViewState("actionFOIncomingDN") = "del" Then
            If dtgFOIncomingDN.CurrentPageIndex > 0 And dsFOIncomingDN.Tables(0).Rows.Count Mod dtgFOIncomingDN.PageSize = 0 Then
                dtgFOIncomingDN.CurrentPageIndex = dtgFOIncomingDN.CurrentPageIndex - 1
                ViewState("actionFOIncomingDN") = ""
            End If
        End If
        intTotRecord = dsFOIncomingDN.Tables(0).Rows.Count
        'intPageRecordCnt4 = intTotRecord
        Session("PageRecordFOIncomingDN") = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind

        resetDashboardDatagridPageIndex(dtgFOIncomingDN, dvFOIncomingDN, "FOIncomingDN")
        dtgFOIncomingDN.DataSource = dvFOIncomingDN
        dtgFOIncomingDN.DataBind()
        If intTotRecord = 0 Then
            dtgFOIncomingDN.ShowHeader = False
            dtgFOIncomingDN.PagerStyle.CssClass = "db_displaynone"
        End If
        ' add for above checking
        ViewState("PageCountFOIncomingDN") = dtgFOIncomingDN.PageCount
    End Function

    Private Function BindgridFMIncomingPendingDN(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objdash As New Dashboard

        '//Retrieve Data from Database
        Dim dsFMIncomingPendingDN As DataSet = New DataSet
        dsFMIncomingPendingDN = objdash.getFMIncomingPendingDN()

        '//for sorting asc or desc
        Dim dvFMIncomingPendingDN As DataView
        dvFMIncomingPendingDN = dsFMIncomingPendingDN.Tables(0).DefaultView
        dvFMIncomingPendingDN.Sort = ViewState("SortExpressionFMIncomingPendingDN")
        If ViewState("SortAscendingFMIncomingPendingDN") = "no" And ViewState("SortExpressionFMIncomingPendingDN") <> "" Then dvFMIncomingPendingDN.Sort += " DESC"
        If ViewState("actionFMIncomingPendingDN") = "del" Then
            If dtgFMIncomingPendingDN.CurrentPageIndex > 0 And dsFMIncomingPendingDN.Tables(0).Rows.Count Mod dtgFMIncomingPendingDN.PageSize = 0 Then
                dtgFMIncomingPendingDN.CurrentPageIndex = dtgFMIncomingPendingDN.CurrentPageIndex - 1
                ViewState("actionFMIncomingPendingDN") = ""
            End If
        End If
        intTotRecord = dsFMIncomingPendingDN.Tables(0).Rows.Count
        'intPageRecordCnt4 = intTotRecord
        Session("PageRecordFMIncomingPendingDN") = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind

        resetDashboardDatagridPageIndex(dtgFMIncomingPendingDN, dvFMIncomingPendingDN, "FMIncomingPendingDN")
        dtgFMIncomingPendingDN.DataSource = dvFMIncomingPendingDN
        dtgFMIncomingPendingDN.DataBind()
        If intTotRecord = 0 Then
            dtgFMIncomingPendingDN.ShowHeader = False
            dtgFMIncomingPendingDN.PagerStyle.CssClass = "db_displaynone"
        End If
        ' add for above checking
        ViewState("PageCountFMIncomingPendingDN") = dtgFMIncomingPendingDN.PageCount
    End Function

    Private Function BindgridFMPendingAckCN(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        Dim objdash As New Dashboard

        '//Retrieve Data from Database
        Dim dsFMPendingAckCN As DataSet = New DataSet
        dsFMPendingAckCN = objdash.getFMPendingAckCN()

        '//for sorting asc or desc
        Dim dvFMPendingAckCN As DataView
        dvFMPendingAckCN = dsFMPendingAckCN.Tables(0).DefaultView
        dvFMPendingAckCN.Sort = ViewState("SortExpressionFMPendingAckCN")
        If ViewState("SortAscendingFMPendingAckCN") = "no" And ViewState("SortExpressionFMPendingAckCN") <> "" Then dvFMPendingAckCN.Sort += " DESC"
        If ViewState("actionFMPendingAckCN") = "del" Then
            If dtgFMPendingAckCN.CurrentPageIndex > 0 And dsFMPendingAckCN.Tables(0).Rows.Count Mod dtgFMPendingAckCN.PageSize = 0 Then
                dtgFMPendingAckCN.CurrentPageIndex = dtgFMPendingAckCN.CurrentPageIndex - 1
                ViewState("actionFMPendingAckCN") = ""
            End If
        End If
        intTotRecord = dsFMPendingAckCN.Tables(0).Rows.Count
        'intPageRecordCnt4 = intTotRecord
        Session("PageRecordFMPendingAckCN") = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind

        resetDashboardDatagridPageIndex(dtgFMPendingAckCN, dvFMPendingAckCN, "FMPendingAckCN")
        dtgFMPendingAckCN.DataSource = dvFMPendingAckCN
        dtgFMPendingAckCN.DataBind()
        If intTotRecord = 0 Then
            dtgFMPendingAckCN.ShowHeader = False
            dtgFMPendingAckCN.PagerStyle.CssClass = "db_displaynone"
        End If
        ' add for above checking
        ViewState("PageCountFMPendingAckCN") = dtgFMPendingAckCN.PageCount
    End Function

    Sub dtgPendingApprPM_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgPendingApprPM.PageIndexChanged
        dtgPendingApprPM.CurrentPageIndex = e.NewPageIndex
        'Rebind to avoid the Quotation Icon disappearing
       rebindgrid()
    End Sub

    Sub dtgPendingMyAppr_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgPendingMyAppr.PageIndexChanged
        dtgPendingMyAppr.CurrentPageIndex = e.NewPageIndex
        'Rebind to avoid the Quotation Icon disappearing
       rebindgrid()
    End Sub

    Private Sub dtgPendingMyIRApproval_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgPendingMyIRApproval.PageIndexChanged
        dtgPendingMyIRApproval.CurrentPageIndex = e.NewPageIndex
        'Rebind to avoid the Quotation Icon disappearing
        rebindgrid()
    End Sub

    Sub dtgOutstdPO_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgOutstdPO.PageIndexChanged
        dtgOutstdPO.CurrentPageIndex = e.NewPageIndex
        'Rebind to avoid the Quotation Icon disappearing
        rebindgrid()
    End Sub
    Sub dtgOutstdPR_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgOutstdPR.PageIndexChanged
        dtgOutstdPR.CurrentPageIndex = e.NewPageIndex
        'Rebind to avoid the Quotation Icon disappearing
        rebindgrid()
    End Sub
    Sub dtgOutstandingRFQ_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgOutstandingRFQ.PageIndexChanged
        dtgOutstandingRFQ.CurrentPageIndex = e.NewPageIndex
        'Rebind to avoid the Quotation Icon disappearing
        rebindgrid()
    End Sub
    Sub dtgInPendingPymt_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgInPendingPymt.PageIndexChanged
        dtgInPendingPymt.CurrentPageIndex = e.NewPageIndex
        'Rebind to avoid the Quotation Icon disappearing
        rebindgrid()
    End Sub
    Sub dtgInInv_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgInInv.PageIndexChanged
        dtgInInv.CurrentPageIndex = e.NewPageIndex
        'Rebind to avoid the Quotation Icon disappearing
        rebindgrid()
    End Sub
    '### StoreKeeper ###
    Sub dtgInDOSK_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgInDOSK.PageIndexChanged
        dtgInDOSK.CurrentPageIndex = e.NewPageIndex
        'Rebind to avoid the Quotation Icon disappearing
        rebindgrid()
    End Sub
    '### Vendor ###
    Sub dtgOutstandingPOVend_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgOutstandingPOVend.PageIndexChanged
        dtgOutstandingPOVend.CurrentPageIndex = e.NewPageIndex
        'Rebind to avoid the Quotation Icon disappearing
        rebindgrid()
    End Sub

    Sub dtgOverduePOVend_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgOverduePOVend.PageIndexChanged
        dtgOverduePOVend.CurrentPageIndex = e.NewPageIndex
        'Rebind to avoid the Quotation Icon disappearing
        rebindgrid()
    End Sub

    Sub dtgOutstandingRFQVend_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgOutstandingRFQVend.PageIndexChanged
        dtgOutstandingRFQVend.CurrentPageIndex = e.NewPageIndex
        'Rebind to avoid the Quotation Icon disappearing
        rebindgrid()
    End Sub

    Sub dtgOutstdGRNQCVerify_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgOutstdGRNQCVerify.PageIndexChanged
        dtgOutstdGRNQCVerify.CurrentPageIndex = e.NewPageIndex
        'Rebind to avoid the Quotation Icon disappearing
       rebindgrid()
    End Sub
    Sub dtgOutstandingInvoiceVend_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgOutstandingInvoiceVend.PageIndexChanged
        dtgOutstandingInvoiceVend.CurrentPageIndex = e.NewPageIndex
        'Rebind to avoid the Quotation Icon disappearing
        rebindgrid()
    End Sub
    Sub dtgPendingMyAppPR_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgPendingMyAppPR.PageIndexChanged
        dtgPendingMyAppPR.CurrentPageIndex = e.NewPageIndex
        'Rebind to avoid the Quotation Icon disappearing
        rebindgrid()
    End Sub
    Sub dtgPendingConvPR_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgPendingConvPR.PageIndexChanged
        dtgPendingConvPR.CurrentPageIndex = e.NewPageIndex
        'Rebind to avoid the Quotation Icon disappearing
       rebindgrid()

    End Sub
    Sub dtgOutstdIPPDoc_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgOutstdIPPDoc.PageIndexChanged
        dtgOutstdIPPDoc.CurrentPageIndex = e.NewPageIndex
        rebindgrid()
    End Sub
    Sub dtgIPPApproval_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgIPPApproval.PageIndexChanged
        dtgIPPApproval.CurrentPageIndex = e.NewPageIndex
        rebindgrid()
    End Sub

    Private Sub dtgIPPPendingPSDRecvDate_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgIPPPendingPSDRecvDate.PageIndexChanged
        dtgIPPPendingPSDRecvDate.CurrentPageIndex = e.NewPageIndex
        rebindgrid()
    End Sub
    Private Sub dtgIPPPendingPSDSentDate_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgIPPPendingPSDSentDate.PageIndexChanged
        dtgIPPPendingPSDSentDate.CurrentPageIndex = e.NewPageIndex
        rebindgrid()
    End Sub
    Sub dtgIQCApproval_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgIQCApproval.PageIndexChanged
        dtgIQCApproval.CurrentPageIndex = e.NewPageIndex
        rebindgrid()
    End Sub
    Sub dtgOutstandingIR_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgOutstandingIR.PageIndexChanged
        dtgOutstandingIR.CurrentPageIndex = e.NewPageIndex
        rebindgrid()
    End Sub
    Sub dtgPendingMRSAcknowledge_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgPendingMRSAcknowledge.PageIndexChanged
        dtgPendingMRSAcknowledge.CurrentPageIndex = e.NewPageIndex
        rebindgrid()
    End Sub
    Sub dtgIssueMRS_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgIssueMRS.PageIndexChanged
        dtgIssueMRS.CurrentPageIndex = e.NewPageIndex
        rebindgrid()
    End Sub
    Sub dtgOutRIAck_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgOutRIAck.PageIndexChanged
        dtgOutRIAck.CurrentPageIndex = e.NewPageIndex
        rebindgrid()
    End Sub
	'Modified for IPP GST Stage 2A
    Sub dtgOutBillDoc_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgOutBillDoc.PageIndexChanged
        dtgOutBillDoc.CurrentPageIndex = e.NewPageIndex
        rebindgrid()
    End Sub
    Sub dtgPendingBillApproval_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgPendingBillApproval.PageIndexChanged
        dtgPendingBillApproval.CurrentPageIndex = e.NewPageIndex
        rebindgrid()
    End Sub
    '-------------------------------

    'Yap: 2015-02-27: Modified for Agora GST Stage 2
    Sub dtgFOIncomingDN_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgFOIncomingDN.PageIndexChanged
        dtgFOIncomingDN.CurrentPageIndex = e.NewPageIndex
        rebindgrid()
    End Sub

    Sub dtgFMIncomingPendingDN_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgFMIncomingPendingDN.PageIndexChanged
        dtgFMIncomingPendingDN.CurrentPageIndex = e.NewPageIndex
        rebindgrid()
    End Sub

    Sub dtgFMPendingAckCN_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgFMPendingAckCN.PageIndexChanged
        dtgFMPendingAckCN.CurrentPageIndex = e.NewPageIndex
        rebindgrid()
    End Sub

    Sub SortCommandPendingApprPM_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "PendingApprPM")
        dtgPendingApprPM.CurrentPageIndex = 0        
        'Rebind to avoid the Quotation Icon disappearing
        rebindgrid()
        BindgridPendingApprPM(True)
    End Sub
    Sub SortCommandPendingMyAppr_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "PendingMyAppr")
        dtgPendingMyAppr.CurrentPageIndex = 0        
        'Rebind to avoid the Quotation Icon disappearing
        rebindgrid()
        BindgridPendingMyAppr(True)
    End Sub
    Sub SortCommandOutstdPR_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "OutstdPR")
        dtgOutstdPR.CurrentPageIndex = 0        
        'Rebind to avoid the Quotation Icon disappearing
        rebindgrid()
        BindgridOutstdPR(True)
    End Sub
    Sub SortCommandOutstdPO_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "OutstdPO")
        dtgOutstdPO.CurrentPageIndex = 0        
        'Rebind to avoid the Quotation Icon disappearing
        rebindgrid()
        BindgridOutstdPO(True)
    End Sub
    Sub SortCommandOutStandingRFQ_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "OutstandingRFQ")
        dtgOutstandingRFQ.CurrentPageIndex = 0        
        'Rebind to avoid the Quotation Icon disappearing
        rebindgrid()
        BindgridOutstandingRFQ(True)
    End Sub
    Sub SortCommandInPendingPymt_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "InPendingPymt")
        dtgInPendingPymt.CurrentPageIndex = 0
        'Rebind to avoid the Quotation Icon disappearing
        rebindgrid()
        BindgridInPendingPymt(True)
    End Sub
    Sub SortCommandInInv_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "InInv")
        dtgInInv.CurrentPageIndex = 0        
        'Rebind to avoid the Quotation Icon disappearing
        rebindgrid()
        BindgridInInv(True)
    End Sub
    '### StoreKeeper ###
    Sub SortCommandInDOSK_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "InDOSK")
        dtgInDOSK.CurrentPageIndex = 0        
        'Rebind to avoid the Quotation Icon disappearing
        rebindgrid()
        BindgridInDOSK(True)
    End Sub
    '### Vendor ###
    Sub SortCommandOutStandingPOVend_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "OutstandingPOVend")
        dtgOutstandingPOVend.CurrentPageIndex = 0
        'Rebind to avoid the Quotation Icon disappearing
        rebindgrid()
        BindgridOutstandingPOVend(True)
    End Sub

    Sub SortCommandOverduePOVend_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "OverduePOVend")
        dtgOverduePOVend.CurrentPageIndex = 0        
        'Rebind to avoid the Quotation Icon disappearing
        rebindgrid()
        BindgridOverduePOVend(True)
    End Sub

    Sub SortCommandOutStandingRFQVend_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "OutstandingRFQVend")
        dtgOutstandingRFQVend.CurrentPageIndex = 0        
        'Rebind to avoid the Quotation Icon disappearing
        rebindgrid()
        BindgridOutstandingRFQVend(True)
    End Sub
    Sub SortCommandOutStandingInvoiceVend_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "OutstandingInvoiceVend")
        dtgOutstandingInvoiceVend.CurrentPageIndex = 0        
        'Rebind to avoid the Quotation Icon disappearing
        rebindgrid()
        BindgridOutstandingInvoiceVend(True)
    End Sub
    Sub SortCommandOutstandingGRNQCVerify_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "OutstandingGRNQCVerify")
        dtgOutstdGRNQCVerify.CurrentPageIndex = 0        
        'Rebind to avoid the Quotation Icon disappearing
        rebindgrid()
        BindgridOutstandingGRNQCVerify(True)
    End Sub
    Sub SortCommandPendingMyAppPR_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "PendingMyAppPR")
        dtgPendingMyAppPR.CurrentPageIndex = 0        
        'Rebind to avoid the Quotation Icon disappearing
        rebindgrid()
        BindgridPendingMyAppPR(True)
    End Sub
    Sub SortCommandPendingConvPR_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "PendingConvPR")
        dtgPendingConvPR.CurrentPageIndex = 0        
        'Rebind to avoid the Quotation Icon disappearing
        rebindgrid()
        BindgridPendingConvPR(True)
    End Sub
    Sub SortCommandOutstdIPPDoc_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "OutstdIPPDoc")
        dtgOutstdIPPDoc.CurrentPageIndex = 0        
        rebindgrid()
        BindgridOutstdIPPDoc(True)
    End Sub
    Sub SortCommandIPPApproval_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "IPPApproval")
        dtgIPPApproval.CurrentPageIndex = 0        
        rebindgrid()
        BindgridIPPApproval(True)
    End Sub
    Sub SortCommandIPPPendingPSDRecvDate_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "IPPPendingPSDRecvDate")
        dtgIPPPendingPSDRecvDate.CurrentPageIndex = 0
        rebindgrid()
        BindgridIPPPendingPSDRecvDate(True)
    End Sub
    Sub SortCommandIPPPendingPSDSentDate_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "IPPPendingPSDSentDate")
        dtgIPPPendingPSDSentDate.CurrentPageIndex = 0
        rebindgrid()
        BindgridIPPPendingPSDSentDate(True)
    End Sub
    Sub SortCommandIQCApproval_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "IQCApproval")
        dtgIQCApproval.CurrentPageIndex = 0
        rebindgrid()
        BindgridIQCApproval(True)
    End Sub
    Sub SortCommandOutstandingIR_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "OutstandingIR")
        dtgOutstandingIR.CurrentPageIndex = 0
        rebindgrid()
        BindgridOutstandingIR(True)
    End Sub
    Sub SortCommandPendingMRSAcknowledge_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "PendingMRSAcknowledge")
        dtgPendingMRSAcknowledge.CurrentPageIndex = 0
        rebindgrid()
        BindgridPendingMRSAcknowledge(True)
    End Sub
    Sub SortCommandPendingMyIRApproval_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "PendingMyIRApproval")
        dtgPendingMyIRApproval.CurrentPageIndex = 0
        rebindgrid()
        BindgridPendingMyIRApproval(True)
    End Sub
    Sub SortCommandIssueMRS_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "IssueMRS")
        dtgIssueMRS.CurrentPageIndex = 0
        rebindgrid()
        BindgridIssueMRS(True)
    End Sub
    Sub SortCommandOutRIAck_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "OutRIAck")
        dtgOutRIAck.CurrentPageIndex = 0
        rebindgrid()
        BindgridOutRIAck(True)
    End Sub
	'Modified for IPP GST Stage 2A
    Sub SortCommandOutBillDoc_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "OutBillDoc")
        dtgOutBillDoc.CurrentPageIndex = 0
        rebindgrid()
        BindgridOutBillDoc(True)
    End Sub
    Sub SortCommandPendingBillApproval_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "PendingBillApproval")
        dtgPendingBillApproval.CurrentPageIndex = 0
        rebindgrid()
        BindgridPendingBillApproval(True)
    End Sub
    '-----------------------------------

    'Yap: 2015-02-27: Modified for Agora GST Stage 2
    Sub SortCommandFOIncomingDN_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "FOIncomingDN")
        dtgFOIncomingDN.CurrentPageIndex = 0
        rebindgrid()
        BindgridFOIncomingDN(True)
    End Sub

    Sub SortCommandFMIncomingPendingDN_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "FMIncomingPendingDN")
        dtgFMIncomingPendingDN.CurrentPageIndex = 0
        rebindgrid()
        BindgridFOIncomingDN(True)
    End Sub

    Sub SortCommandFMPendingAckCN_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        Grid_SortCommandDashboard(sender, e, "FMPendingAckCN")
        dtgFMPendingAckCN.CurrentPageIndex = 0
        rebindgrid()
        BindgridFMPendingAckCN(True)
    End Sub

    Private Sub dtgPendingApprPM_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPendingApprPM.ItemCreated
        Grid_ItemCreatedDashboard(dtgPendingApprPM, e, "PendingApprPM")
    End Sub
    Private Sub dtgOutstdPR_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstdPR.ItemCreated
        Grid_ItemCreatedDashboard(dtgOutstdPR, e, "OutstdPR")
    End Sub
    Private Sub dtgPendingMyAppr_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPendingMyAppr.ItemCreated
        Grid_ItemCreatedDashboard(dtgPendingMyAppr, e, "PendingMyAppr")
    End Sub
    Private Sub dtgOutstdPO_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstdPO.ItemCreated
        Grid_ItemCreatedDashboard(dtgOutstdPO, e, "OutstdPO")
    End Sub
    Private Sub dtgOutStandingRFQ_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstandingRFQ.ItemCreated
        Grid_ItemCreatedDashboard(dtgOutstandingRFQ, e, "OutstandingRFQ")
    End Sub
    Private Sub dtgInPendingPymt_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgInPendingPymt.ItemCreated
        Grid_ItemCreatedDashboard(dtgInPendingPymt, e, "InPendingPymt")
    End Sub
    Private Sub dtgInInv_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgInInv.ItemCreated
        Grid_ItemCreatedDashboard(dtgInInv, e, "InInv")
    End Sub
    '### StoreKeeper ###
    Private Sub dtgInDOSK_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgInDOSK.ItemCreated
        Grid_ItemCreatedDashboard(dtgInDOSK, e, "InDOSK")
    End Sub
    '### Vendor ###
    Private Sub dtgOutStandingPOVend_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstandingPOVend.ItemCreated
        Grid_ItemCreatedDashboard(dtgOutstandingPOVend, e, "OutstandingPOVend")
    End Sub

    Private Sub dtgOverduePOVend_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOverduePOVend.ItemCreated
        Grid_ItemCreatedDashboard(dtgOverduePOVend, e, "OverduePOVend")
    End Sub

    Private Sub dtgOutStandingRFQVend_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstandingRFQVend.ItemCreated
        Grid_ItemCreatedDashboard(dtgOutstandingRFQVend, e, "OutstandingRFQVend")
    End Sub

    Private Sub dtgOutStandingInvoiceVend_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstandingInvoiceVend.ItemCreated
        Grid_ItemCreatedDashboard(dtgOutstandingInvoiceVend, e, "OutstandingInvoiceVend")
    End Sub
    Private Sub dtgOutstdGRNQCVerify_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstdGRNQCVerify.ItemCreated
        Grid_ItemCreatedDashboard(dtgOutstdGRNQCVerify, e, "OutstandingGRNQCVerify")
    End Sub
    Private Sub dtgPendingMyAppPR_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPendingMyAppPR.ItemCreated
        Grid_ItemCreatedDashboard(dtgPendingMyAppPR, e, "PendingMyAppPR")
    End Sub
    Private Sub dtgPendingConvPR_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPendingConvPR.ItemCreated
        Grid_ItemCreatedDashboard(dtgPendingConvPR, e, "PendingConvPR")
    End Sub
    Private Sub dtgOutstdIPPDoc_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstdIPPDoc.ItemCreated
        Grid_ItemCreatedDashboard(dtgOutstdIPPDoc, e, "OutstdIPPDoc")
    End Sub
    Private Sub dtgIPPApproval_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgIPPApproval.ItemCreated
        Grid_ItemCreatedDashboard(dtgIPPApproval, e, "IPPApproval")
    End Sub
    Private Sub dtgIPPPendingPSDSentDate_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgIPPPendingPSDSentDate.ItemCreated
        Grid_ItemCreatedDashboard(dtgIPPPendingPSDSentDate, e, "IPPPendingPSDSentDate")
    End Sub
    Private Sub dtgIPPPendingPSDRecvDate_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgIPPPendingPSDRecvDate.ItemCreated
        Grid_ItemCreatedDashboard(dtgIPPPendingPSDRecvDate, e, "IPPPendingPSDRecvDate")
    End Sub
    Private Sub dtgIQCApproval_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgIQCApproval.ItemCreated
        Grid_ItemCreatedDashboard(dtgIQCApproval, e, "IQCApproval")
    End Sub
    Private Sub dtgOutstandingIR_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstandingIR.ItemCreated
        Grid_ItemCreatedDashboard(dtgOutstandingIR, e, "OutstandingIR")
    End Sub
    Private Sub dtgPendingMRSAcknowledge_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPendingMRSAcknowledge.ItemCreated
        Grid_ItemCreatedDashboard(dtgPendingMRSAcknowledge, e, "PendingMRSAcknowledge")
    End Sub
    Private Sub dtgPendingMyIRApproval_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPendingMyIRApproval.ItemCreated
        Grid_ItemCreatedDashboard(dtgPendingMyIRApproval, e, "PendingMyIRApproval")
    End Sub
    Private Sub dtgIssueMRS_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgIssueMRS.ItemCreated
        Grid_ItemCreatedDashboard(dtgIssueMRS, e, "IssueMRS")
    End Sub
    Private Sub dtgOutRIAck_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutRIAck.ItemCreated
        Grid_ItemCreatedDashboard(dtgOutRIAck, e, "OutRIAck")
    End Sub
	'Modified for IPP GST Stage 2A
    Private Sub dtgOutBillDoc_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutBillDoc.ItemCreated
        Grid_ItemCreatedDashboard(dtgOutBillDoc, e, "OutBillDoc")
    End Sub
    Private Sub dtgPendingBillApproval_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPendingBillApproval.ItemCreated
        Grid_ItemCreatedDashboard(dtgPendingBillApproval, e, "PendingBillApproval")
    End Sub
    '--------------------------------

    'Yap: 2015-02-27: Modified for Agora GST Stage 2
    Private Sub dtgFOIncomingDN_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgFOIncomingDN.ItemCreated
        Grid_ItemCreatedDashboard(dtgFOIncomingDN, e, "FOIncomingDN")
    End Sub

    Private Sub dtgFMIncomingPendingDN_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgFMIncomingPendingDN.ItemCreated
        Grid_ItemCreatedDashboard(dtgFMIncomingPendingDN, e, "FMIncomingPendingDN")
    End Sub

    Private Sub dtgFMPendingAckCN_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgFMPendingAckCN.ItemCreated
        Grid_ItemCreatedDashboard(dtgFMPendingAckCN, e, "FMPendingAckCN")
    End Sub

    Private Sub dtgPendingApprPM_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPendingApprPM.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim objPR As New PurchaseReq2
            Dim objPO As New PurchaseOrder
            Dim lnkPONo
            lnkPONo = e.Item.FindControl("lnkPONum")
            lnkPONo.Text = dv("PO Number")

            '       lnkPONo.NavigateUrl = "../PO/PODetail.aspx?Frm=Dashboard&caller=BUYER&index=" & dv("POM_PO_Index") & "&PONo=" & dv("PO Number")
            lnkPONo.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx", "Frm=Dashboard&side=b&filetype=2&dpage=AllDashBoard&PO_INDEX=" & dv("POM_PO_Index") & "&PO_NO=" & dv("PO Number") & "&status=" & dv("POM_PO_STATUS") & "&BCoyID=" & dv("POM_B_COY_ID"))

            e.Item.Cells(1).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Submitted Date"))
            e.Item.Cells(5).Text = Format(CDbl(dv("Amount")), "###,###,##0.00")

            If Common.parseNull(dv("POM_URGENT")) = "1" Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(0).Controls.Add(imgAttach)
            End If

            If Not IsDBNull(dv("POM_RFQ_INDEX")) AndAlso CStr(dv("POM_RFQ_INDEX")) <> "" Then
                Dim lnkRFQ As New HyperLink
                Dim strRFQNo, strRFQName As String
                objPR.getRFQName(dv("POM_RFQ_INDEX"), strRFQNo, strRFQName)
                lnkRFQ.ImageUrl = dDispatcher.direct("Plugins/Images", "Q-3 Icon (10x10).jpg")
                If dv("POM_PO_STATUS") = 1 Or dv("POM_PO_STATUS") = 2 Or dv("POM_PO_STATUS") = 3 _
                    Or dv("POM_PO_STATUS") = 4 Or dv("POM_PO_STATUS") = 5 Or dv("POM_PO_STATUS") = 6 Then
                    lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "viewQoute.aspx", "Frm=Dashboard&PageID=108&RFQ_ID=" & dv("POM_RFQ_INDEX") & "&RFQ_Num=" & strRFQNo & "&vcomid=" & dv("POM_S_COY_ID") & "&side=quote")
                    lnkRFQ.ToolTip = "Click here to view quotation details"
                Else
                    lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Appr=Y&Frm=Dashboard&RFQType=S&PageID=7&side=quote&RFQ_Num=" & strRFQNo & "&RFQ_ID=" & dv("POM_RFQ_INDEX") & "&vcomid=" & dv("POM_S_COY_ID") & "&RFQ_Name=" & Server.UrlEncode(strRFQName))
                    lnkRFQ.ToolTip = "Click here to view quotation comparison"
                End If
                e.Item.Cells(MyApp.icPONum).Controls.Add(lnkRFQ)
            End If

            If objPO.HasAttachment(dv("PO Number"), HttpContext.Current.Session("CompanyId")) Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "clip_icon.gif")
                e.Item.Cells(0).Controls.Add(imgAttach)
            End If
            objPR = Nothing
        End If
    End Sub
    Private Sub dtgPendingMyAppr_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPendingMyAppr.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim objPR As New PurchaseReq2
            Dim objPO As New PurchaseOrder
            Dim lnkPONo
            lnkPONo = e.Item.FindControl("lnkPONum")
            lnkPONo.Text = dv("PO Number")

            'lnkPONo.NavigateUrl = "../PO/POApprDetail.aspx?Frm=Dashboard&caller=BUYER&pageid=" & Session("strPageId") & "index=" & dv("POM_PO_Index") & "&relief=false&PONo=" & dv("PO Number")
            lnkPONo.NavigateUrl = dDispatcher.direct("PO", "POApprDetail.aspx", "Frm=Dashboard&dpage=AllDashBoard&AO=" & Session("UserID") & "&relief=false&PageID=8&index=" & dv("POM_PO_Index") & "&PONO=" & dv("PO Number"))

            e.Item.Cells(1).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Submitted Date"))
            e.Item.Cells(5).Text = Format(CDbl(dv("Amount")), "###,###,##0.00")

            If Common.parseNull(dv("POM_URGENT")) = "1" Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(0).Controls.Add(imgAttach)
            End If

            If Not IsDBNull(dv("POM_RFQ_INDEX")) AndAlso CStr(dv("POM_RFQ_INDEX")) <> "" Then
                Dim lnkRFQ As New HyperLink
                Dim strRFQNo, strRFQName As String
                objPR.getRFQName(dv("POM_RFQ_INDEX"), strRFQNo, strRFQName)
                lnkRFQ.ImageUrl = dDispatcher.direct("Plugins/Images", "Q-3 Icon (10x10).jpg")
                'lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Frm=Dashboard&PageID=8&side=other&RFQ_Num=" & strRFQNo & "&RFQ_ID=" & dv("POM_RFQ_INDEX") & "&RFQ_Name=" & Server.UrlEncode(strRFQName))
                'lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "viewQoute.aspx", "Frm=Dashboard&PageID=108&RFQ_ID=" & dv("POM_RFQ_INDEX") & "&RFQ_Num=" & strRFQNo & "&vcomid=" & dv("POM_S_COY_ID") & "&side=quote")
                'lnkRFQ.ToolTip = "Click here to view quotation comparison"
                If dv("POM_PO_STATUS") = 1 Or dv("POM_PO_STATUS") = 2 Or dv("POM_PO_STATUS") = 3 _
                    Or dv("POM_PO_STATUS") = 4 Or dv("POM_PO_STATUS") = 5 Or dv("POM_PO_STATUS") = 6 Then
                    lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "viewQoute.aspx", "Frm=Dashboard&PageID=108&RFQ_ID=" & dv("POM_RFQ_INDEX") & "&RFQ_Num=" & strRFQNo & "&vcomid=" & dv("POM_S_COY_ID") & "&side=quote")
                    lnkRFQ.ToolTip = "Click here to view quotation details"
                Else
                    lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Appr=Y&Frm=Dashboard&RFQType=S&PageID=8&side=quote&RFQ_Num=" & strRFQNo & "&RFQ_ID=" & dv("POM_RFQ_INDEX") & "&vcomid=" & dv("POM_S_COY_ID") & "&RFQ_Name=" & Server.UrlEncode(strRFQName))
                    lnkRFQ.ToolTip = "Click here to view quotation comparison"
                End If
                e.Item.Cells(MyApp.icPONum).Controls.Add(lnkRFQ)
            End If

            If objPO.HasAttachment(dv("PO Number"), HttpContext.Current.Session("CompanyId")) Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "clip_icon.gif")
                e.Item.Cells(0).Controls.Add(imgAttach)
            End If
            objPR = Nothing
        End If
    End Sub
    Private Sub dtgOutstdPR_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstdPR.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim objPR As New PurchaseReq2
            Dim lnkPRNo
            lnkPRNo = e.Item.FindControl("lnkPRNo")
            lnkPRNo.Text = dv("PR Number")
            Session("urlreferer") = "Dashboard"
            If strPageId = "" Then
                strPageId = "113"
            End If
            If dv("PRM_PR_STATUS") = 1 Then 'Draft PR - RaisePR screen
                lnkPRNo.NavigateUrl = dDispatcher.direct("PR", "RaisePR.aspx", "Frm=Dashboard&pageid=" & strPageId & "&index=" & dv("PRM_PR_Index") & "&prid=" & dv("PR Number") & "&type=mod&mode=bc")

            Else    'PR Detail screen
                lnkPRNo.NavigateUrl = dDispatcher.direct("PR", "PRDetail.aspx", "Frm=Dashboard&pageid=" & strPageId & "&index=" & dv("PRM_PR_Index") & "&PRNO=" & dv("PR Number") & "&type=mod&mode=bc")

            End If

            If Common.parseNull(dv("PRM_URGENT")) = "1" Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(0).Controls.Add(imgAttach)
            End If

            If objPR.HasAttachment(dv("PR Number")) Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "clip_icon.gif")
                e.Item.Cells(0).Controls.Add(imgAttach)
            End If

            If Not IsDBNull(dv("Creation Date")) Then
                e.Item.Cells(1).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Creation Date"))
            End If
            If Not IsDBNull(dv("Submission Date")) Then
                e.Item.Cells(2).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Submission Date"))
            End If
            If Not IsDBNull(dv("Approved Date")) Then
                e.Item.Cells(3).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Approved Date"))
            End If
         
            If dv("PRM_PR_STATUS") = 3 Then 'if status=Pending approval(3) showed as 'Submitted'
                e.Item.Cells(4).Text = "Submitted"
            ElseIf dv("PRM_PR_STATUS") = 6 Then 'if status=Cancelled By(6) showed as 'Cancelled'
                e.Item.Cells(4).Text = "Cancelled"
            ElseIf dv("PRM_PR_STATUS") = 8 Then 'if status=Rejected By(8) showed as 'Rejected'
                e.Item.Cells(4).Text = "Rejected"
            ElseIf dv("PRM_PR_STATUS") = 7 Then
                e.Item.Cells(4).Text = "Held By " & Common.parseNull(dv("NAME"))
            ElseIf dv("PRM_PR_STATUS") = 9 Then 'if status=Void Draft PR(9) showed as 'Void'
                e.Item.Cells(4).Text = "Void"
            End If
            objPR = Nothing
        End If
    End Sub
    Private Sub dtgOutstdPO_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstdPO.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkRFQ As New HyperLink
            Dim strRFQNo, strRFQName As String
            Dim objPR As New PurchaseReq2
            Dim objPO As New PurchaseOrder
            Dim lnkPONo
            Dim lblStatus As Label

            lblStatus = e.Item.FindControl("lblStatus")
            If dv("POM_PO_STATUS") = "0" Then
                lblStatus.Text = "Draft"
            ElseIf dv("POM_PO_STATUS") = "3" Then
                lblStatus.Text = "Accepted"
            Else
                lblStatus.Text = "Approved"
            End If

            lnkPONo = e.Item.FindControl("lnkPONum2")
            lnkPONo.Text = dv("PO Number")

            If dv("POM_PO_STATUS") = "0" Then
                Dim _POType = objDB.GetVal("Select isnull(POM_PO_TYPE,'') from po_mstr where pom_po_no = '" & lnkPONo.Text & "' and pom_b_coy_id = '" & Session("CompanyId") & "'")
                If _POType.ToString.Trim <> "" Then
                    'Jules 2018.07.14 - PAMB - Hit error if strPageId is empty.
                    If _POType.ToString.Trim = "Y" Then
                        'lnkPONo.NavigateUrl = dDispatcher.direct("PO", "RaiseFFPO.aspx", "pageid=" & strPageId & "&index=" & dv("POM_PO_INDEX") & "&poid=" & dv("PO Number") & "&mode=po&type=mod&Frm=Dashboard&dpage=AllDashBoard")
                        lnkPONo.NavigateUrl = dDispatcher.direct("PO", "RaiseFFPO.aspx", "pageid=" & IIf(strPageId = "", "7", strPageId) & "&index=" & dv("POM_PO_INDEX") & "&poid=" & dv("PO Number") & "&mode=po&type=mod&Frm=Dashboard&dpage=AllDashBoard")
                    Else
                        'lnkPONo.NavigateUrl = dDispatcher.direct("PO", "RaisePO.aspx", "pageid=" & strPageId & "&index=" & dv("POM_PO_INDEX") & "&poid=" & dv("PO Number") & "&mode=po&type=mod&Frm=Dashboard&dpage=AllDashBoard")
                        lnkPONo.NavigateUrl = dDispatcher.direct("PO", "RaisePO.aspx", "pageid=" & IIf(strPageId = "", "7", strPageId) & "&index=" & dv("POM_PO_INDEX") & "&poid=" & dv("PO Number") & "&mode=po&type=mod&Frm=Dashboard&dpage=AllDashBoard")
                    End If
                    'End modification.
                Else
                    lnkPONo.NavigateUrl = dDispatcher.direct("PO", "RaisePO.aspx", "pageid=7&index=" & dv("POM_PO_INDEX") & "&poid=" & dv("PO Number") & "&mode=po&type=mod&Frm=Dashboard&dpage=AllDashBoard")
                End If
                'lnkPONo.NavigateUrl = dDispatcher.direct("PO", "RaisePO.aspx", "pageid=7&index=" & dv("POM_PO_INDEX") & "&poid=" & dv("PO Number") & "&mode=po&type=mod&Frm=Dashboard&dpage=AllDashBoard")
            Else
                lnkPONo.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx", "pageid=7&PO_INDEX=" & dv("POM_PO_INDEX") & "&PO_NO=" & dv("PO Number") & "&BCoyID=" & dv("POM_B_COY_ID") & "&status=" & dv("POM_PO_STATUS") & "&side=b&filetype=2&type=list&Frm=Dashboard&dpage=AllDashBoard")
            End If
            'e.Item.Cells(2).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("PO Date"))
            e.Item.Cells(4).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Accepted Date"))

            If Common.parseNull(dv("POM_URGENT")) = "1" Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(OutPO.icPONum).Controls.Add(imgAttach)
            End If

            If Not IsDBNull(dv("POM_RFQ_INDEX")) AndAlso CStr(dv("POM_RFQ_INDEX")) <> "" Then
                objPR.getRFQName(dv("POM_RFQ_INDEX"), strRFQNo, strRFQName)
                lnkRFQ.ImageUrl = dDispatcher.direct("Plugins/Images", "Q-3 Icon (10x10).jpg")
                If dv("POM_PO_STATUS") = 1 Or dv("POM_PO_STATUS") = 2 Or dv("POM_PO_STATUS") = 3 _
                            Or dv("POM_PO_STATUS") = 4 Or dv("POM_PO_STATUS") = 5 Or dv("POM_PO_STATUS") = 6 Then
                    lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "viewQoute.aspx", "Frm=Dashboard&PageID=108&RFQ_ID=" & dv("POM_RFQ_INDEX") & "&RFQ_Num=" & strRFQNo & "&vcomid=" & dv("POM_S_COY_ID") & "&side=quote")
                    lnkRFQ.ToolTip = "Click here to view quotation details"
                Else
                    lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Appr=Y&Frm=Dashboard&RFQType=S&PageID=7&side=quote&RFQ_Num=" & strRFQNo & "&RFQ_ID=" & dv("POM_RFQ_INDEX") & "&vcomid=" & dv("POM_S_COY_ID") & "&RFQ_Name=" & Server.UrlEncode(strRFQName))
                    lnkRFQ.ToolTip = "Click here to view quotation comparison"
                End If

                e.Item.Cells(OutPO.icPONum).Controls.Add(lnkRFQ)
            End If

            If objPO.HasAttachment(dv("PO Number"), HttpContext.Current.Session("CompanyId")) Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "clip_icon.gif")
                e.Item.Cells(1).Controls.Add(imgAttach)
            End If
            objPR = Nothing
        End If
    End Sub
    Private Sub dtgOutStandingRFQ_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstandingRFQ.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim objrfq As New RFQ
            Dim lnkRFQNo, lnkRFQViewRes
            Dim i As Integer
            lnkRFQNo = e.Item.FindControl("lnkRFQNum")
            lnkRFQNo.Text = dv("RFQ Number")

            lnkRFQViewRes = e.Item.FindControl("lnkRFQViewRes")
            Dim check As Boolean = objrfq.check_v_status(Common.parseNull(dv("RM_RFQ_ID"))) ' 1=response
            If dv("RM_Status") = "3" Then

                If Common.parseNull(dv("Expiry Date")) < Date.Today Then
                    'Dim strTemp As String = "Draft" & "<BR>" & "Expired"
                    lnkRFQViewRes.Text = "Draft, Expired"
                Else
                    lnkRFQViewRes.Text = "Draft"
                End If
                lnkRFQNo.NavigateUrl = dDispatcher.direct("RFQ", "Create_RFQ.aspx", "pageid=93&Frm=Dashboard&dpage=AllDashBoard" & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&RFQ_Num=" & Server.UrlEncode(Common.parseNull(dv("RFQ Number"))))
                'Response.Redirect("Create_RFQ.aspx?pageid=" & strPageId & "&chk_option=1&edit=0")
                'ElseIf dv("RM_RFQ_OPTION") = 1 Then
                '    lnkRFQViewRes.Text = "Response"
            Else
                If IsDBNull(dv("Expiry Date")) Then

                Else
                    lnkRFQNo.NavigateUrl = dDispatcher.direct("RFQ", "RFQDetail.aspx", "pageid=5&page=1&goto=1&RFQ_Num=" & Server.UrlEncode(dv("RFQ Number")) & "&Frm=Dashboard&dpage=AllDashBoard&RFQ_ID=" & dv("RM_RFQ_ID"))
                    'lnkRFQNo.Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQDetail.aspx", "pageid=5&page=1&goto=1&RFQ_Num=" & server.urlencode(dv("RFQ Number")) & "&Frm=Dashboard&dpage=AllDashBoard&RFQ_ID=" & dv("RM_RFQ_ID"))) & " "" ><font color=#0000ff>" & Common.parseNull(server.urlencode(dv("RFQ Number"))) & "</font></A>"
                    If Common.parseNull(dv("Expiry Date")) >= Date.Today Then
                        If check = False Then
                            lnkRFQViewRes.Text = "Sent"

                        ElseIf check = True Then
                            If dv("RM_RFQ_OPTION") = 0 Then 'open
                                'lnkRFQViewRes.Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Frm=RFQ_Outstg_List&RFQType=V&pageid=" & strPageId & "&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&RFQ_Name=" & Server.UrlEncode(Common.parseNull(dv("RM_RFQ_Name")))) & """ ><font color=#0000ff>View Response</font></A>"
                                lnkRFQViewRes.Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQComSummary.aspx", "disabled=N&RFQType=V&pageid=7&Frm=Dashboard&dpage=AllDashBoard&RFQ_Num=" & (Common.parseNull(Server.UrlEncode(dv("RFQ Number")))) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&RFQ_Name=" & Server.UrlEncode(Common.parseNull(dv("RFQ Name")))) & """ ><font color=#0000ff>View Response</font></A>"

                            ElseIf dv("RM_RFQ_OPTION") = 1 Then  'close
                                lnkRFQViewRes.Text = "Response"
                            End If
                        End If
                    Else
                        If check = False Then
                            lnkRFQViewRes.Text = "No Response, Expired"
                        ElseIf check = True Then
                            lnkRFQViewRes.Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQComSummary.aspx", "disabled=N&RFQType=V&pageid=7&Frm=Dashboard&dpage=AllDashBoard&RFQ_Num=" & Server.UrlEncode(Common.parseNull(dv("RFQ Number"))) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&RFQ_Name=" & Server.UrlEncode(Common.parseNull(dv("RFQ Name")))) & """ ><font color=#0000ff>View Response</font></A>, Expired"
                        End If
                    End If
                End If
                ''check whether there's any response for the RFQ
                'i = objDB.GetCount("rfq_replies_mstr", " where rrm_rfq_id = '" & dv("RM_RFQ_ID") & "'")
                'If i > 0 Then
                '    lnkRFQViewRes.Text = "View Response"
                '    lnkRFQViewRes.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "RFQType=V&pageid=7&Frm=Dashboard&dpage=AllDashBoard&RFQ_Num=" & Common.parseNull(dv("RFQ Number")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&RFQ_Name=" & Common.parseNull(dv("RFQ Name")))
                '    Session("dPage") = "PurMgr"
                '    'lbl_status.Text = "<A href=""RFQComSummary.aspx?pageid=" & strPageId & "&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&RFQ_Name=" & Server.UrlEncode(Common.parseNull(dv("RM_RFQ_Name"))) & """ ><font color=#0000ff>View Response</font></A>"
                'Else
                '    lnkRFQViewRes.Text = "Sent"
                'End If
                'lnkRFQNo.NavigateUrl = dDispatcher.direct("RFQ", "RFQDetail.aspx", "pageid=5&page=1&goto=1&RFQ_Num=" & dv("RFQ Number") & "&Frm=Dashboard&dpage=AllDashBoard&RFQ_ID=" & dv("RM_RFQ_ID"))
            End If

            If objrfq.HasAttachment(dv("RFQ Number"), "E") Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "clip_icon.gif")
                e.Item.Cells(1).Controls.Add(imgAttach)
            End If

            e.Item.Cells(4).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Creation Date"))
            e.Item.Cells(5).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Expiry Date"))
        End If
    End Sub
    Private Sub dtgInPendingPymt_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgInPendingPymt.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim lnkInvNo
            lnkInvNo = e.Item.FindControl("lnkInvNum")
            lnkInvNo.Text = dv("Invoice Number")
            If dv("DOC_TYPE") = "EPROC" Then
                lnkInvNo.NavigateUrl = dDispatcher.direct("Tracking", "InvoiceDetails.aspx", "cmd=2&role=3&Frm=Dashboard&dpage=AllDashBoard&Name=FMnAO&pageid=15&INVNO=" & dv("Invoice Number") & "&vcomid=" & dv("IM_S_COY_ID") & "&folder=A&status=2")
            Else
                lnkInvNo.NavigateUrl = dDispatcher.direct("IPP", "IPPApprovalDetail.aspx", "pagefrm=Dashboard&DocumentNo=" & Server.UrlEncode(dv("Invoice Number")) & "&index=" & dv("IM_INVOICE_INDEX"))
            End If


            e.Item.Cells(2).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Due Date"))
            e.Item.Cells(5).Text = Format(CDbl(dv("Amount")), "###,###,##0.00")
        End If
    End Sub
    Private Sub dtgInInv_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgInInv.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim lnkInvNo
            lnkInvNo = e.Item.FindControl("lnkInv")
            lnkInvNo.Text = dv("Invoice Number")


            If dv("DOC_TYPE") = "EPROC" Then
                lnkInvNo.NavigateUrl = dDispatcher.direct("Tracking", "InvoiceDetails.aspx", "cmd=1&role=2&Frm=Dashboard&Name=Buyer&BMode=New&pageid=15&INVNO=" & Common.parseNull(dv("Invoice Number")) & "&vcomid=" & Common.parseNull(dv("IM_S_COY_ID")) & "&folder=N" & "&status=1")
            Else
                lnkInvNo.NavigateUrl = dDispatcher.direct("IPP", "IPPApprovalDetail.aspx", "pagefrm=Dashboard&DocumentNo=" & Server.UrlEncode(dv("Invoice Number")) & "&index=" & dv("IM_INVOICE_INDEX"))
            End If

            '
            e.Item.Cells(2).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Due Date"))
            e.Item.Cells(5).Text = Format(CDbl(dv("Amount")), "###,###,##0.00")
        End If
    End Sub
    Private Sub dtgPendingMyAppPR_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPendingMyAppPR.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim objPR As New PurchaseReq2
            Dim lnkPRNo
            Dim objdash As New Dashboard
            lnkPRNo = e.Item.FindControl("lnkPRNo")
            lnkPRNo.Text = dv("PR Number")
            Session("urlreferer") = "Dashboard"

            'lnkPONo.NavigateUrl = "../PO/POApprDetail.aspx?Frm=Dashboard&caller=BUYER&pageid=" & Session("strPageId") & "index=" & dv("POM_PO_Index") & "&relief=false&PONo=" & dv("PO Number")
            lnkPRNo.NavigateUrl = dDispatcher.direct("PR", "PRApprDetail.aspx", "Frm=Dashboard&dpage=AllDashBoard&AO=" & Session("UserID") & "&relief=false&PageID=8&index=" & dv("PRM_PR_Index") & "&PRNo=" & dv("PR Number"))
            If Not IsDBNull(dv("Submitted Date")) Then
                e.Item.Cells(4).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Submitted Date"))

            End If
            If dv("PRM_PR_TYPE") = "" Then
                e.Item.Cells(1).Text = "Non-Contract"
                e.Item.Cells(7).Text = ""
            ElseIf dv("PRM_PR_TYPE") = "CC" Then
                e.Item.Cells(1).Text = "Contract"
                'check vendor name                
                e.Item.Cells(5).Text = objdash.checkvendorname(dv("PR Number"))
                'check currency
                e.Item.Cells(6).Text = objdash.checkcurrency(dv("PR Number"))
                If e.Item.Cells(6).Text = "Multiple Currency" Then
                    e.Item.Cells(7).Text = ""
                Else
                    e.Item.Cells(7).Text = Format(CDbl(dv("PRM_PR_COST")), "###,###,##0.00")
                End If
            End If

            If Common.parseNull(dv("PRM_URGENT")) = "1" Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(0).Controls.Add(imgAttach)
            End If

            If objPR.HasAttachment(dv("PR Number")) Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "clip_icon.gif")
                e.Item.Cells(0).Controls.Add(imgAttach)
            End If

            objPR = Nothing
        End If
    End Sub
    Private Sub dtgPendingConvPR_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPendingConvPR.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim objPR As New PurchaseReq2
            Dim lnkPRNo
            lnkPRNo = e.Item.FindControl("lnkPRNo")
            lnkPRNo.Text = dv("PR Number")
            Session("urlreferer") = "Dashboard"
            If strPageId = "" Then
                strPageId = "114"
            End If

            If Common.parseNull(dv("PRM_URGENT")) = "1" Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(0).Controls.Add(imgAttach)
            End If

            lnkPRNo.NavigateUrl = dDispatcher.direct("PR", "ConvertPR.aspx", "Frm=Dashboard&pageid=" & strPageId & "&index=" & dv("PRM_PR_Index") & "&PRNo=" & dv("PR Number"))
            If Not IsDBNull(dv("Approved Date")) Then
                e.Item.Cells(3).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Approved Date"))

            End If

            objPR = Nothing
        End If
    End Sub
    '### StoreKeeper ###
    Private Sub dtgInDOSK_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgInDOSK.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim lnkDONo
            lnkDONo = e.Item.FindControl("lnkDONum")
            lnkDONo.Text = dv("DOM_DO_NO")
            strPageId = 13
            Session("strPageId") = strPageId

            lnkDONo.NavigateUrl = dDispatcher.direct("GRN", "AddGRN1.aspx", "Frm=Dashboard&Mode=New&PONo=" & dv("POM_PO_No") & "&DONo=" & dv("DOM_DO_NO") & "&DOIndex=" & dv("DOM_DO_Index") & "&pageid=13&Level=created" & "&vendor=" & dv("CM_COY_NAME"))
            e.Item.Cells(1).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("DOM_DO_DATE"))
            e.Item.Cells(4).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("POM_PO_DATE"))

            If objDO1.withAttach(dv("DOM_DO_NO"), dv("DOM_S_COY_ID")) Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "clip_icon.gif")
                e.Item.Cells(0).Controls.Add(imgAttach)
            End If
        End If

    End Sub
    '### Vendor ###
    Private Sub dtgOutStandingPOVend_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstandingPOVend.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkRFQ As New HyperLink
            Dim strRFQNo, strRFQName As String
            Dim objPR As New PurchaseReq2
            Dim lnkPONo
            Dim lblStatus As Label
            Dim objPO As New PurchaseOrder
            Dim objPO1 As New PurchaseOrder_Vendor
            lnkPONo = e.Item.FindControl("lnkPONum")
            lnkPONo.Text = dv("PO Number")

            Dim dsDO As DataSet = New DataSet


            lblStatus = e.Item.FindControl("lblStatus")
            If dv("POM_PO_STATUS") = "1" Or dv("POM_PO_STATUS") = "2" Then
                lblStatus.Text = "New"
                lnkPONo.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx", "Frm=Dashboard&pageid=100&PO_INDEX=" & dv("POM_PO_INDEX") & "&PO_NO=" & dv("PO Number") & "&BCoyID=" & dv("POM_B_COY_ID") & "&status=" & Common.parseNull(dv("POM_PO_STATUS")) & "&side=v&filetype=2&dpage=AllDashBoard")

            ElseIf dv("POM_PO_STATUS") = "3" And dv("POM_FULFILMENT") = "4" Then 'with partial delivery
                lblStatus.Text = "Cancelled"
                lnkPONo.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx", "Frm=Dashboard&pageid=100&PO_INDEX=" & dv("POM_PO_INDEX") & "&PO_NO=" & dv("PO Number") & "&BCoyID=" & dv("POM_B_COY_ID") & "&status=" & Common.parseNull(dv("POM_PO_STATUS")) & "&side=v&filetype=1&dpage=AllDashBoard&cr_no=" & Common.parseNull(dv("CR_NO")))

            ElseIf dv("POM_PO_STATUS") = "3" Then
                lblStatus.Text = "Outstanding"
                dsDO = objDO.GetDOStatus(1, dv("POM_PO_INDEX"))
                If dsDO.Tables(0).Rows.Count > 0 Then
                    'lnkPONo.NavigateUrl = "../DO/AddDO.aspx?Frm=Dashboard&dpage=Vendor&Mode=Edit&PONo=" & dv("PO Number") & "&BCoy=" & dv("POM_B_COY_ID") & "&POIdx=" & dv("POM_PO_INDEX") & "&pageid=" & strPageId & "&LocID=" & dsDO.Tables(0).Rows(0)("DOM_D_ADDR_CODE") & "&DONo=" & dsDO.Tables(0).Rows(0)("DOM_DO_NO") & "&DOIdx=" & dsDO.Tables(0).Rows(0)("DOM_DO_Index")
                    lnkPONo.NavigateUrl = dDispatcher.direct("DO", "AddDO.aspx", "Frm=Dashboard&dpage=AllDashBoard&Mode=Edit&PONo=" & dv("PO Number") & "&BCoy=" & dv("POM_B_COY_ID") & "&POIdx=" & dv("POM_PO_INDEX") & "&pageid=100&LocID=" & dsDO.Tables(0).Rows(0)("DOM_D_ADDR_CODE") & "&DONo=" & dsDO.Tables(0).Rows(0)("DOM_DO_NO"))

                Else
                    lnkPONo.NavigateUrl = dDispatcher.direct("DO", "AddDO.aspx", "Frm=Dashboard&dpage=AllDashBoard&Mode=New&PONo=" & dv("PO Number") & "&BCoy=" & dv("POM_B_COY_ID") & "&POIdx=" & dv("POM_PO_INDEX") & "&pageid=100")
                End If

                'lnkPONo.NavigateUrl = "../DO/AddDO.aspx?Frm=Dashboard&dpage=Vendor&DONo=" & dsDO.Tables(0).Rows(0)("DOM_DO_NO") & "&DOIdx=" & dv("POM_PO_INDEX") & "&POIdx=" & dv("POM_PO_INDEX") & "&mode=Edit&LocID=" & dsDO.Tables(0).Rows(0)("DOM_D_ADDR_CODE") & "&PONo=" & dv("PO Number") & "&BCoy=" & dv("POM_B_Coy_ID") & "&pageid=" & strPageId

            Else
                lblStatus.Text = "Cancelled"
                lnkPONo.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx", "Frm=Dashboard&pageid=100&PO_INDEX=" & dv("POM_PO_INDEX") & "&PO_NO=" & dv("PO Number") & "&BCoyID=" & dv("POM_B_COY_ID") & "&status=" & Common.parseNull(dv("POM_PO_STATUS")) & "&side=v&filetype=1&dpage=AllDashBoard&cr_no=" & Common.parseNull(dv("CR_NO")))
            End If

            'If dsDO.Tables(0).Rows.Count > 0 Then
            '    lnkPONo.NavigateUrl = "../DO/AddDO.aspx?Frm=Dashboard&DONo=" & dsDO.Tables(0).Rows(0)("DOM_DO_NO") & "&DOIdx=" & dv("POM_PO_INDEX") & "&POIdx=" & dv("POM_PO_INDEX") & "&mode=Edit&LocID=" & dsDO.Tables(0).Rows(0)("DOM_D_ADDR_CODE") & "&PONo=" & dv("PO Number") & "&BCoy=" & dv("POM_B_Coy_ID") & "&pageid=" & strPageId
            'Else
            'End If

            e.Item.Cells(3).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("PO Date"))
            e.Item.Cells(4).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Due Date"))

            If Common.parseNull(dv("POM_URGENT")) = "1" Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(VenPO.icPONum).Controls.Add(imgAttach)
            End If

            If Not IsDBNull(dv("POM_RFQ_INDEX")) AndAlso CStr(dv("POM_RFQ_INDEX")) <> "" Then
                objPR.getRFQName(dv("POM_RFQ_INDEX"), strRFQNo, strRFQName)
                lnkRFQ.ImageUrl = dDispatcher.direct("Plugins/Images", "Q-3 Icon (10x10).jpg")
                lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "viewQoute.aspx", "Frm=Dashboard&PageID=108&RFQ_ID=" & dv("POM_RFQ_INDEX") & "&RFQ_Num=" & strRFQNo & "&vcomid=" & dv("POM_S_COY_ID") & "&side=quote")
                lnkRFQ.ToolTip = "Click here to view quotation comparison"
                e.Item.Cells(VenPO.icPONum).Controls.Add(lnkRFQ)
            End If

            'If objPO.HasAttachmentVen(dv("PO Number")) Then
            If objPO.HasAttachment(dv("PO Number"), objPO1.get_comid(dv("POM_PO_Index"))) Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "clip_icon.gif")
                e.Item.Cells(VenPO.icPONum).Controls.Add(imgAttach)
            End If
            objPR = Nothing
        End If
    End Sub

    Private Sub dtgOverduePOVend_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOverduePOVend.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkRFQ As New HyperLink
            Dim strRFQNo, strRFQName As String
            Dim objPR As New PurchaseReq2
            Dim objPO As New PurchaseOrder
            Dim lnkPONo
            Dim objPO1 As New PurchaseOrder_Vendor
            lnkPONo = e.Item.FindControl("lnkPONum2")
            lnkPONo.Text = dv("POM_PO_No")

            Dim dsDO As DataSet = New DataSet
            dsDO = objDO.GetDOStatusWithDA(1, dv("POM_PO_INDEX"), dv("POD_D_ADDR_CODE"))


            'If dsCurrency.Tables(0).Rows.Count > 0 Then
            'lblRate.Text = dsCurrency.Tables(0).Rows(0)("CE_RATE")


            'If dsDO.Tables(0).Rows.Count > 0 Then
            '    lnkPONo.NavigateUrl = "../DO/AddDO.aspx?Frm=Dashboard&DONo=" & dsDO.Tables(0).Rows(0)("DOM_DO_NO") & "&DOIdx=" & dv("POM_PO_INDEX") & "&POIdx=" & dv("POM_PO_INDEX") & "&mode=Edit&pageid=22&LocID=" & dsDO.Tables(0).Rows(0)("DOM_D_ADDR_CODE") & "&PONo=" & dv("POM_PO_No") & "&BCoy=" & dv("POM_B_Coy_ID")
            'Else
            '    lnkPONo.NavigateUrl = "../DO/AddDO.aspx?Frm=Dashboard&Mode=New&PONo=" & dv("POM_PO_No") & "&BCoy=" & dv("Buyer Company") & "&POIdx=" & dv("POM_PO_INDEX") & "&pageid=22"
            'End If



            If dsDO.Tables(0).Rows.Count > 0 Then
                'lnkPONo.NavigateUrl = "../DO/AddDO.aspx?Frm=Dashboard&Mode=Edit&PONo=" & dv("POM_PO_No") & "&BCoy=" & dv("POM_B_Coy_ID") & "&POIdx=" & dv("POM_PO_INDEX") & "&DONo=" & dsDO.Tables(0).Rows(0)("DOM_DO_NO") & "&DOIdx=" & dsDO.Tables(0).Rows(0)("DOM_DO_Index") & "&LocID=" & dsDO.Tables(0).Rows(0)("DOM_D_ADDR_CODE") & "&pageid=" & strPageId
                lnkPONo.NavigateUrl = dDispatcher.direct("DO", "AddDO.aspx", "Frm=Dashboard&Mode=Edit&PONo=" & dv("POM_PO_No") & "&BCoy=" & dv("POM_B_Coy_ID") & "&POIdx=" & dv("POM_PO_INDEX") & "&DONo=" & dsDO.Tables(0).Rows(0)("DOM_DO_NO") & "&LocID=" & dsDO.Tables(0).Rows(0)("DOM_D_ADDR_CODE") & "&pageid=100")

            Else
                lnkPONo.NavigateUrl = dDispatcher.direct("DO", "AddDO.aspx", "Frm=Dashboard&Mode=New&PONo=" & dv("POM_PO_No") & "&BCoy=" & dv("POM_B_Coy_ID") & "&POIdx=" & dv("POM_PO_INDEX") & "&pageid=100&DA=" & dv("POD_D_ADDR_CODE"))
            End If

            e.Item.Cells(2).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("PO Date"))
            e.Item.Cells(3).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Due Date"))

            If Common.parseNull(dv("POM_URGENT")) = "1" Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(OvVenPO.icPONum).Controls.Add(imgAttach)
            End If

            If Not IsDBNull(dv("POM_RFQ_INDEX")) AndAlso CStr(dv("POM_RFQ_INDEX")) <> "" Then
                objPR.getRFQName(dv("POM_RFQ_INDEX"), strRFQNo, strRFQName)
                lnkRFQ.ImageUrl = dDispatcher.direct("Plugins/Images", "Q-3 Icon (10x10).jpg")
                lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "viewQoute.aspx", "Frm=Dashboard&PageID=108&RFQ_ID=" & dv("POM_RFQ_INDEX") & "&RFQ_Num=" & strRFQNo & "&vcomid=" & dv("POM_S_COY_ID") & "&side=quote")
                lnkRFQ.ToolTip = "Click here to view quotation comparison"
                e.Item.Cells(OvVenPO.icPONum).Controls.Add(lnkRFQ)
            End If

            'If objPO.HasAttachmentVen(dv("POM_PO_No")) Then
            If objPO.HasAttachment(dv("POM_PO_No"), objPO1.get_comid(dv("POM_PO_INDEX"))) Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "clip_icon.gif")
                e.Item.Cells(OvVenPO.icPONum).Controls.Add(imgAttach)
            End If
        End If
    End Sub

    Private Sub dtgOutStandingRFQVend_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstandingRFQVend.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim lnkRFQNo
            Dim objrfq As New RFQ
            lnkRFQNo = e.Item.FindControl("lnkRFQNum")
            lnkRFQNo.Text = dv("RFQ Number")


            'lnkRFQNo.NavigateUrl = "../RFQ/RFQDetail.aspx?pageid=21&page=1&goto=1&RFQ_Num=" & dv("RFQ Number") & "&RFQ_ID=" & dv("RM_RFQ_ID") & "&vcom_id=" & Session("CompanyId")


            'lnkRFQNo.NavigateUrl = "../RFQ/VendorRFQList1.aspx?pageid=" & strPageId & "&RFQ_No=" & dv("RFQ Number") & "&RFQ_ID=" & dv("RM_RFQ_ID") & "&bcomid=" & dv("RM_Coy_ID")
            lnkRFQNo.NavigateUrl = dDispatcher.direct("RFQ", "CreateQuotationNew.aspx", "Frm=Dashboard&pageid=98&RFQ_No=" & Server.UrlEncode(dv("RFQ Number")) & "&RFQ_ID=" & dv("RM_RFQ_ID") & "&bcomid=" & dv("RM_Coy_ID"))

            'lbl_status.Text = "New, <A href=""CreateQuotation.aspx?pageid=" & strPageId & " &bcomid=" & dv("RM_Coy_ID") & "&RFQ_No=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & " "" ><font color=#0000ff>Reply</font></A>"

            e.Item.Cells(2).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Creation Date"))
            e.Item.Cells(3).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Expiry Date"))

            If objrfq.HasAttachmentVen(dv("RFQ Number"), dv("RM_Coy_ID"), "E") Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "clip_icon.gif")
                e.Item.Cells(0).Controls.Add(imgAttach)
            End If
        End If
    End Sub

    Private Sub dtgOutStandingInvoiceVend_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstandingInvoiceVend.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim lnkInvNo
            lnkInvNo = e.Item.FindControl("lnkInv")
            lnkInvNo.Text = dv("PO Number")

            'Dim dsDO As DataSet = New DataSet
            'dsDO = objDO.GetDOStatus(1, dv("POM_PO_INDEX"))

            'If dsDO.Tables(0).Rows.Count > 0 Then
            '    lnkInvNo.NavigateUrl = "../Invoice/InvGeneration1.aspx?Frm=Dashboard&DONo=" & dsDO.Tables(0).Rows(0)("DO Number") & "&POIdx=" & dv("POM_PO_INDEX") & "&mode=Edit&pageid=" & strPageId & "&LocID=" & dsDO.Tables(0).Rows(0)("DOM_D_ADDR_CODE") & "&PONo=" & dv("PO Number") & "&BCoy=" & dv("POM_B_Coy_ID")
            'Else
            '    lnkInvNo.NavigateUrl = "../Invoice/InvGeneration1.aspx?Frm=Dashboard&Mode=New&PONo=" & dv("PO Number") & "&BCoy=" & dv("CM_COY_NAME") & "&POIdx=" & dv("POM_PO_INDEX") & "&pageid=" & strPageId
            'End If
            lnkInvNo.NavigateUrl = dDispatcher.direct("Invoice", "InvList.aspx", "Frm=Dashboard&VMode=New&PONo=" & dv("PO Number") & "&DONo=" & dv("DO Number") & "&GRNNo=" & dv("GRN Number") & "&BCoy=" & dv("CM_COY_NAME") & "&POIdx=" & dv("POM_PO_INDEX") & "&pageid=101")
            If IsDBNull(dv("Amount")) Then
                e.Item.Cells(6).Text = "0.00"
            Else
                e.Item.Cells(6).Text = Format(CDbl(dv("Amount")), "###,###,##0.00")
            End If
        End If
    End Sub
    Private Sub dtgOutstdGRNQCVerify_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstdGRNQCVerify.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkGRNNo As HyperLink
            lnkGRNNo = e.Item.Cells(0).FindControl("lnkGRNNo")
            lnkGRNNo.NavigateUrl = dDispatcher.direct("Inventory", "InventoryVerificationDetails.aspx", "Frm=Dashboard&Mode=New&GRNNo=" & dv("IV_GRN_NO") & "&Vendor=" & dv("CM_COY_NAME") & "&GRNDate=" & Common.FormatWheelDate(WheelDateFormat.LongDate, dv("GM_CREATED_DATE")) & "&ReceivedDate=" & Common.FormatWheelDate(WheelDateFormat.LongDate, dv("GM_DATE_RECEIVED")) & "&pageid=34")
            lnkGRNNo.Text = dv("IV_GRN_NO")
            e.Item.Cells(4).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("GM_CREATED_DATE"))
            e.Item.Cells(5).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("GM_DATE_RECEIVED"))
        End If
    End Sub
    Private Sub dtgOutstdIPPDoc_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstdIPPDoc.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkDocNo As HyperLink
            Dim strDocCreator As String
            lnkDocNo = e.Item.Cells(EnumIPP.docno).FindControl("lnkDocNo")
            lnkDocNo.Text = dv("IM_INVOICE_NO")
            strDocCreator = objDB.Get1Column("invoice_mstr", "im_created_by", " where im_invoice_index = " & dv("IM_INVOICE_INDEX") & "")

            'Jules 2018.07.11 - PAMB - Allow "\" and "#" in Document No.
            Select Case e.Item.Cells(EnumIPP.status).Text
                Case 10
                    lnkDocNo.NavigateUrl = dDispatcher.direct("IPP", "IPPEntry.aspx", "mode=modify&frm=dashboard&index=" & dv("IM_INVOICE_INDEX") & "&DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO")))
                Case 14
                    If dv("IM_ROUTE_TO") = "" Then
                        lnkDocNo.NavigateUrl = dDispatcher.direct("IPP", "IPPEntry.aspx", "mode=modify&status=reject&frm=dashboard&index=" & dv("IM_INVOICE_INDEX") & "&DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO")))
                    Else
                        If dv("IM_ROUTE_TO") = strDocCreator Then
                            lnkDocNo.NavigateUrl = dDispatcher.direct("IPP", "PSDAcceptanceDetails.aspx", "index=" & dv("IM_INVOICE_INDEX") & "&Frm=dashboard&DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO")))
                        ElseIf dv("IM_ROUTE_TO") = HttpContext.Current.Session("UserID") Then
                            lnkDocNo.NavigateUrl = dDispatcher.direct("IPP", "PSDAcceptanceDetails.aspx", "index=" & dv("IM_INVOICE_INDEX") & "&Frm=dashboard&DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO")))
                        Else

                            lnkDocNo.NavigateUrl = dDispatcher.direct("IPP", "IPPDocument.aspx", "index=" & dv("IM_INVOICE_INDEX") & "&frm=dashboard&DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO")))
                        End If
                    End If

                Case Else
                    lnkDocNo.NavigateUrl = dDispatcher.direct("IPP", "IPPDocument.aspx", "index=" & dv("IM_INVOICE_INDEX") & "&frm=dashboard&DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO")))
            End Select

            Select Case e.Item.Cells(EnumIPP.Status).Text
                Case 10
                    e.Item.Cells(EnumIPP.Status).Text = "Draft"
                Case 16
                    e.Item.Cells(EnumIPP.status).Text = "Submitted"
                    'Case 12
                    '    e.Item.Cells(EnumIPP.status).Text = "Submitted"
                Case 14
                    e.Item.Cells(EnumIPP.status).Text = "Rejected"
            End Select
            e.Item.Cells(EnumIPP.createdt).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("IM_CREATED_ON"))
            e.Item.Cells(EnumIPP.submitdt).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("IM_SUBMIT_DATE"))
            e.Item.Cells(EnumIPP.amt).Text = Format(CDbl(dv("IM_INVOICE_TOTAL")), "###,###,##0.00")

        End If
    End Sub
    Private Sub dtgIPPApproval_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgIPPApproval.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkDocNo As HyperLink
            lnkDocNo = e.Item.Cells(0).FindControl("lnkDocNo")
            lnkDocNo.Text = dv("IM_INVOICE_NO")
            lnkDocNo.NavigateUrl = dDispatcher.direct("IPP", "IPPAOApprovalDetail.aspx", "frm=dashboard&index=" & dv("IM_INVOICE_INDEX") & "&DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO")) & "&pageid=") 'Zulham 12072018 - PAMB
            e.Item.Cells(4).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("IM_SUBMIT_DATE"))
        End If
    End Sub


    Private Sub dtgIPPPendingPSDSentDate_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgIPPPendingPSDSentDate.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkDocNo As HyperLink
            lnkDocNo = e.Item.Cells(EnumIPP.docno).FindControl("lnkDocNo")
            lnkDocNo.Text = dv("IM_INVOICE_NO")
            Select Case e.Item.Cells(EnumIPP.status).Text
                Case 10
                    lnkDocNo.NavigateUrl = dDispatcher.direct("IPP", "IPPEntry.aspx", "mode=modify&frm=dashboard&index=" & dv("IM_INVOICE_INDEX") & "&DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO"))) 'Zulham 12072018 - PAMB
                Case 14
                    lnkDocNo.NavigateUrl = dDispatcher.direct("IPP", "IPPEntry.aspx", "mode=modify&status=reject&frm=dashboard&index=" & dv("IM_INVOICE_INDEX") & "&DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO"))) 'Zulham 12072018 - PAMB
                Case Else
                    lnkDocNo.NavigateUrl = dDispatcher.direct("IPP", "IPPDocument.aspx", "index=" & dv("IM_INVOICE_INDEX") & "&frm=dashboard&DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO"))) 'Zulham 12072018 - PAMB
            End Select

            Select Case e.Item.Cells(EnumIPP.status).Text
                Case 18
                    e.Item.Cells(EnumIPP.status).Text = "E2P Verified" 'Zulham 17072018 - PAMB
                    'Zulham 10072018 - PAMB
                    'Added status 17
                Case 17
                    'Zulham 07112018
                    e.Item.Cells(EnumIPP.status).Text = "Department Approved"

            End Select
            e.Item.Cells(EnumIPP.createdt).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("IM_CREATED_ON"))
            e.Item.Cells(EnumIPP.submitdt).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("IM_SUBMIT_DATE"))
            e.Item.Cells(EnumIPP.amt).Text = Format(CDbl(dv("IM_INVOICE_TOTAL")), "###,###,##0.00")

        End If
    End Sub

    Private Sub dtgIPPPendingPSDRecvDate_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgIPPPendingPSDRecvDate.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkDocNo As HyperLink
            lnkDocNo = e.Item.Cells(EnumIPP.docno).FindControl("lnkDocNo")
            lnkDocNo.Text = dv("IM_INVOICE_NO")
            Select Case e.Item.Cells(EnumIPP.status).Text
                Case 10
                    lnkDocNo.NavigateUrl = dDispatcher.direct("IPP", "IPPEntry.aspx", "mode=modify&frm=dashboard&index=" & dv("IM_INVOICE_INDEX") & "&DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO"))) 'Zulham 12072018 - PAMB
                Case 14
                    'lnkDocNo.NavigateUrl = dDispatcher.direct("IPP", "IPPEntry.aspx", "mode=modify&status=reject&frm=dashboard&index=" & dv("IM_INVOICE_INDEX") & "&DocumentNo=" & dv("IM_INVOICE_NO"))
                    lnkDocNo.NavigateUrl = dDispatcher.direct("IPP", "PSDAcceptanceDetails.aspx", "index=" & dv("IM_INVOICE_INDEX") & "&Frm=dashboard&DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO"))) 'Zulham 12072018 - PAMB
                Case Else
                    lnkDocNo.NavigateUrl = dDispatcher.direct("IPP", "PSDAcceptanceDetails.aspx", "index=" & dv("IM_INVOICE_INDEX") & "&Frm=dashboard&DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO"))) 'Zulham 12072018 - PAMB
                    'lnkDocNo.NavigateUrl = dDispatcher.direct("IPP", "IPPDocument.aspx", "Frm=PSDAcceptRejList&index=" & dv("IM_INVOICE_INDEX") & "&frm=dashboard&DocumentNo=" & dv("IM_INVOICE_NO"))                    
            End Select

            Select Case e.Item.Cells(EnumIPP.status).Text
                Case 17
                    'Zulham 07112018
                    e.Item.Cells(EnumIPP.status).Text = "Department Approved"
                    'Case 11
                    '    e.Item.Cells(EnumIPP.status).Text = "Submitted"
                    'Case 12
                    '    e.Item.Cells(EnumIPP.status).Text = "Submitted"
                Case 14
                    e.Item.Cells(EnumIPP.status).Text = "Rejected"
            End Select
            e.Item.Cells(EnumIPP.createdt).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("IM_CREATED_ON"))
            e.Item.Cells(EnumIPP.submitdt).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("IM_SUBMIT_DATE"))
            e.Item.Cells(EnumIPP.amt).Text = Format(CDbl(dv("IM_INVOICE_TOTAL")), "###,###,##0.00")

        End If
    End Sub

    Private Sub dtgIQCApproval_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgIQCApproval.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkIQCNo As HyperLink
            Session("urlreferer") = "Dashboard"

            lnkIQCNo = e.Item.Cells(0).FindControl("lnkIQCNo")
            lnkIQCNo.Text = dv("IVL_IQC_NO")
            lnkIQCNo.NavigateUrl = dDispatcher.direct("IQC", "IQCApprDetail.aspx", "Frm=Dashboard&dpage=AllDashBoard&caller=approval&AO=" & Session("UserID") & "&relief=false&PageID=135&index=" & dv("IVL_VERIFY_LOT_INDEX") & "&IQCNo=" & dv("IVL_IQC_NO"))

            e.Item.Cells(3).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("GM_DATE_RECEIVED"))
        End If
    End Sub

    Private Sub dtgOutstandingIR_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstandingIR.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkIRNo As HyperLink
            Session("urlreferer") = "Dashboard"

            lnkIRNo = e.Item.Cells(0).FindControl("lnkIRNo")
            lnkIRNo.Text = dv("IRM_IR_NO")
            lnkIRNo.NavigateUrl = dDispatcher.direct("Inventory", "InventoryReqInfo.aspx", "Frm=Dashboard&dpage=AllDashBoard&caller=Dashboard&PageID=27&index=" & dv("IRM_IR_INDEX") & "&IRNo=" & dv("IRM_IR_NO"))

            If Common.parseNull(dv("IRM_IR_URGENT")) = "Y" Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(0).Controls.Add(imgAttach)
            End If

            e.Item.Cells(1).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("IRM_IR_DATE"))

            If dv("IRM_IR_STATUS") = "1" Then
                e.Item.Cells(2).Text = "Submitted"
            ElseIf dv("IRM_IR_STATUS") = "3" Then
                e.Item.Cells(2).Text = "Pending Approval"
            Else
                e.Item.Cells(2).Text = ""
            End If

        End If
    End Sub

    Private Sub dtgPendingMRSAcknowledge_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPendingMRSAcknowledge.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkMRSNo As HyperLink
            Session("urlreferer") = "Dashboard"

            lnkMRSNo = e.Item.Cells(0).FindControl("lnkMRSNo")
            lnkMRSNo.Text = dv("IRSM_IRS_NO")
            lnkMRSNo.NavigateUrl = dDispatcher.direct("Inventory", "MRSAckDetail.aspx", "Frm=Dashboard&dpage=AllDashBoard&PageID=27&relief=false&index=" & dv("IRSM_IRS_INDEX") & "&MRSNo=" & dv("IRSM_IRS_NO"))

            If Common.parseNull(dv("IRSM_IRS_URGENT")) = "Y" Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(0).Controls.Add(imgAttach)
            End If

            e.Item.Cells(1).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("IRSM_IRS_DATE"))
            e.Item.Cells(3).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("IRSM_IRS_APPROVED_DATE"))

            If dv("IRSM_IRS_STATUS") = "2" Then
                e.Item.Cells(2).Text = "Issued"
            ElseIf dv("IRSM_IRS_STATUS") = "7" Then
                e.Item.Cells(2).Text = "Partial Issued"
            Else
                e.Item.Cells(2).Text = ""
            End If

        End If
    End Sub
    Private Sub dtgPendingMyIRApproval_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPendingMyIRApproval.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkIRNo As HyperLink
            Session("urlreferer") = "Dashboard"

            lnkIRNo = e.Item.Cells(0).FindControl("lnkIRNo")
            lnkIRNo.Text = dv("IRM_IR_NO")
            lnkIRNo.NavigateUrl = dDispatcher.direct("Inventory", "IRApprDetail.aspx", "Frm=Dashboard&dpage=AllDashBoard&PageID=138&relief=false&index=" & dv("IRM_IR_INDEX") & "&IRNo=" & dv("IRM_IR_NO"))

            If Common.parseNull(dv("IRM_IR_URGENT")) = "Y" Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(0).Controls.Add(imgAttach)
            End If

            e.Item.Cells(1).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("IRM_IR_DATE"))

            If dv("IRM_IR_STATUS") = "1" Then
                e.Item.Cells(2).Text = "Submitted"
            ElseIf dv("IRM_IR_STATUS") = "3" Then
                e.Item.Cells(2).Text = "Pending Approval"
            Else
                e.Item.Cells(2).Text = ""
            End If

        End If
    End Sub
    Private Sub dtgIssueMRS_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgIssueMRS.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkMRSNo As HyperLink
            Session("urlreferer") = "Dashboard"

            lnkMRSNo = e.Item.Cells(0).FindControl("lnkMRSNo")
            lnkMRSNo.Text = dv("IRSM_IRS_NO")
            lnkMRSNo.NavigateUrl = dDispatcher.direct("Inventory", "MRSApprDetail.aspx", "Frm=Dashboard&dpage=AllDashBoard&PageID=139&index=" & dv("IRSM_IRS_INDEX") & "&MRSNo=" & dv("IRSM_IRS_NO"))

            If Common.parseNull(dv("IRSM_IRS_URGENT")) = "Y" Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(0).Controls.Add(imgAttach)
            End If

            e.Item.Cells(1).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("IRSM_IRS_DATE"))

        End If
    End Sub
    Private Sub dtgOutRIAck_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutRIAck.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkRINo As HyperLink
            Session("urlreferer") = "Dashboard"

            lnkRINo = e.Item.Cells(0).FindControl("lnkRINo")
            lnkRINo.Text = dv("IRIM_RI_NO")
            lnkRINo.NavigateUrl = dDispatcher.direct("Inventory", "ReturnInwardAckDetail.aspx", "pageid=" & strPageId & "&RI_NO=" & dv("IRIM_RI_NO") & "&frm=Dashboard")

            e.Item.Cells(1).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("IRIM_RI_DATE"))
        End If
    End Sub
	'Modified for IPP GST Stage 2A
    Private Sub dtgOutBillDoc_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutBillDoc.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkBillNo As HyperLink
            Session("urlreferer") = "Dashboard"

            lnkBillNo = e.Item.Cells(0).FindControl("lnkBillNo")
            lnkBillNo.Text = dv("BM_INVOICE_NO")
            'IPP Gst Stage 2A - CH - 12 Feb 2015
            Select Case dv("BM_INVOICE_STATUS")
                Case 1
                    lnkBillNo.NavigateUrl = dDispatcher.direct("Billing", "BillingEntry.aspx", "Frm=Dashboard&mode=modify&urlrefereripp=" & Session("urlreferer") & "&index=" & dv("BM_INVOICE_INDEX") & "&DocumentNo=" & dv("BM_INVOICE_NO"))
                    'e.Item.Cells(EnumBilling.DocNo).Text = "<A href=""" & dDispatcher.direct("Billing", "BillingEntry.aspx", "mode=modify&urlrefereripp=" & Session("urlreferer") & "&index=" & dv("BM_INVOICE_INDEX") & "&DocumentNo=" & dv("BM_INVOICE_NO")) & """ ><font color=#0000ff>" & Common.parseNull(dv("BM_INVOICE_NO")) & "</font></A>"
                Case 4
                    lnkBillNo.NavigateUrl = dDispatcher.direct("Billing", "BillingEntry.aspx", "Frm=Dashboard&mode=modify&status=reject&&urlrefereripp=" & Session("urlreferer") & "&index=" & dv("BM_INVOICE_INDEX") & "&DocumentNo=" & dv("BM_INVOICE_NO"))
                    'e.Item.Cells(EnumBilling.DocNo).Text = "<A href=""" & dDispatcher.direct("Billing", "BillingEntry.aspx", "mode=modify&status=reject&urlrefereripp=" & Session("urlreferer") & "&index=" & dv("BM_INVOICE_INDEX") & "&DocumentNo=" & dv("BM_INVOICE_NO")) & """ ><font color=#0000ff>" & Common.parseNull(dv("BM_INVOICE_NO")) & "</font></A>"
                Case Else
                    lnkBillNo.NavigateUrl = dDispatcher.direct("Billing", "BillingDocument.aspx", "Frm=Dashboard&index=" & dv("BM_INVOICE_INDEX") & "&DocumentNo=" & dv("BM_INVOICE_NO"))
                    'e.Item.Cells(EnumBilling.DocNo).Text = "<A href=""" & dDispatcher.direct("Billing", "BillingDocument.aspx", "Frm=BillingList&index=" & dv("BM_INVOICE_INDEX") & "&DocumentNo=" & dv("BM_INVOICE_NO")) & """ ><font color=#0000ff>" & Common.parseNull(dv("BM_INVOICE_NO")) & "</font></A>"
            End Select
            'lnkBillNo.NavigateUrl = dDispatcher.direct("Billing", "ReturnInwardAckDetail.aspx", "pageid=" & strPageId & "&BILL_NO=" & dv("BM_INVOICE_NO") & "&frm=Dashboard")

            e.Item.Cells(3).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("BM_CREATED_ON"))
            If Not IsDBNull(dv("BM_SUBMIT_DATE")) Then
                e.Item.Cells(4).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("BM_SUBMIT_DATE"))
            End If
        End If
    End Sub
    Private Sub dtgPendingBillApproval_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPendingBillApproval.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkBillNo As HyperLink
            Session("urlreferer") = "Dashboard"

            lnkBillNo = e.Item.Cells(0).FindControl("lnkBillNo")
            lnkBillNo.Text = dv("BM_INVOICE_NO")
            'IPP Gst Stage 2A - CH - 12 Feb 2015
            lnkBillNo.NavigateUrl = dDispatcher.direct("Billing", "BillingAOApprovalDetail.aspx", "frm=Dashboard&pageid=" & strPageId & "&index=" & dv("BM_INVOICE_INDEX") & "&DocumentNo=" & dv("BM_INVOICE_NO"))

            If Not IsDBNull(dv("BM_SUBMIT_DATE")) Then
                e.Item.Cells(4).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("BM_SUBMIT_DATE"))
            End If
        End If
    End Sub
    '-------------------------------

    'Yap: 2015-02-27: Modified for Agora GST Stage 2
    Private Sub dtgFOIncomingDN_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgFOIncomingDN.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkDNNo As HyperLink
            Session("urlreferer") = "Dashboard"

            lnkDNNo = e.Item.Cells(0).FindControl("lnkDNNo")
            lnkDNNo.Text = dv("DNM_DN_NO")
            lnkDNNo.NavigateUrl = dDispatcher.direct("DebitNote", "DebitNoteDetails.aspx", "cmd=verify&role=2&Frm=Dashboard&Name=Buyer&BMode=New&pageid=" & strPageId & "&DNNO=" & Common.parseNull(dv("DNM_DN_NO")) & "&vcomid=" & Common.parseNull(dv("DNM_DN_S_COY_ID")) & "&folder=N" & "&status=2")

            e.Item.Cells(1).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("DNM_DN_DATE"))
            e.Item.Cells(4).Text = Format(CDbl(dv("AMOUNT")), "###,###,##0.00")
        End If
    End Sub

    Private Sub dtgFMIncomingPendingDN_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgFMIncomingPendingDN.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkDNNo As HyperLink
            Session("urlreferer") = "Dashboard"

            lnkDNNo = e.Item.Cells(0).FindControl("lnkDNNo")
            lnkDNNo.Text = dv("DNM_DN_NO")
            lnkDNNo.NavigateUrl = dDispatcher.direct("DebitNote", "DebitNoteDetails.aspx", "cmd=approve&role=3&Frm=Dashboard&Name=Buyer&BMode=New&pageid=" & strPageId & "&DNNO=" & Common.parseNull(dv("DNM_DN_NO")) & "&vcomid=" & Common.parseNull(dv("DNM_DN_S_COY_ID")) & "&folder=N" & "&status=1")

            e.Item.Cells(1).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("DNM_DN_DATE"))
            e.Item.Cells(4).Text = Format(CDbl(dv("AMOUNT")), "###,###,##0.00")
        End If
    End Sub

    Private Sub dtgFMPendingAckCN_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgFMPendingAckCN.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkCNNo As HyperLink
            Session("urlreferer") = "Dashboard"

            lnkCNNo = e.Item.Cells(0).FindControl("lnkCNNo")
            lnkCNNo.Text = dv("CNM_CN_NO")
            'lnkCNNo.NavigateUrl = dDispatcher.direct("CreditNote", "CreditNoteDetails.aspx", "pageid=" & strPageId & "&CNNO=" & dv("CNM_CN_NO") & "&frm=Dashboard")
            lnkCNNo.NavigateUrl = dDispatcher.direct("CreditNote", "CreditNoteDetails.aspx", "cmd=&role=&Frm=Dashboard&Name=Buyer&pageid=" & strPageId & "&CNNO=" & Common.parseNull(dv("CNM_CN_NO")) & "&vcomid=" & Common.parseNull(dv("CNM_CN_S_COY_ID")) & "&status=" & Common.parseNull(dv("CNM_CN_STATUS")))
            
            e.Item.Cells(1).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("CNM_CN_DATE"))
            e.Item.Cells(4).Text = Format(CDbl(dv("AMOUNT")), "###,###,##0.00")
        End If
    End Sub
End Class