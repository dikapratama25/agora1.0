'//Attachment Ind
Imports AgoraLegacy
Imports eProcure.Component

Public Class SearchPR_AllFTN
    Inherits AgoraLegacy.AppBaseClass

    Dim dDispatcher As New AgoraLegacy.dispatcher

    Public Enum EnumPR
        icPRNo
        icPRType
        icCreationDate
        icSubmissionDate
        icApprovedDate
        icStatus
        icPONO
    End Enum

    Dim strCaller As String
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CheckButtonAccess(True)
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        SetGridProperty(dtgPRList)

        MyBase.Page_Load(sender, e)
        If Not Page.IsPostBack Then
            GenerateTab()
        End If

        Session("urlreferer") = "PRAll"
        intPageRecordCnt = ViewState("intPageRecordCnt")

        ' Role
    End Sub

    Public Sub dtgPRList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgPRList.PageIndexChanged
        dtgPRList.CurrentPageIndex = e.NewPageIndex
        Bindgrid(True)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgPRList.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Sub chk_condition(ByRef strStatus As String, ByRef strStatus2 As String)
        If chkOpen.Checked = True Then
            strStatus = IIf(strStatus = "", PRStatus.Draft, strStatus & "," & PRStatus.Draft)
            strStatus2 = IIf(strStatus2 = "", PRStatus.Draft, strStatus2 & "," & PRStatus.Draft)
        End If
        If chkSubmitted.Checked Then
            strStatus = IIf(strStatus = "", PRStatus.Submitted & "," & PRStatus.PendingApproval, strStatus & "," & PRStatus.Submitted & "," & PRStatus.PendingApproval)
            strStatus2 = IIf(strStatus2 = "", PRStatus.Submitted & "," & PRStatus.PendingApproval, strStatus2 & "," & PRStatus.Submitted & "," & PRStatus.PendingApproval)
        End If
        If chkApproved.Checked Then
            strStatus = IIf(strStatus = "", PRStatus.Approved, strStatus & "," & PRStatus.Approved)
            strStatus2 = IIf(strStatus2 = "", PRStatus.Approved, strStatus2 & "," & PRStatus.Approved)
        End If
        If chkConverted.Checked Then
            strStatus = IIf(strStatus = "", PRStatus.ConvertedToPO, strStatus & "," & PRStatus.ConvertedToPO)
            strStatus2 = IIf(strStatus2 = "", PRStatus.ConvertedToPO, strStatus2 & "," & PRStatus.ConvertedToPO)
        End If
        If chkVoid.Checked Then
            strStatus = IIf(strStatus = "", PRStatus.Void, strStatus & "," & PRStatus.Void)
            strStatus2 = IIf(strStatus2 = "", PRStatus.Void, strStatus2 & "," & PRStatus.Void)
        End If
        If chkCancel.Checked Then
            strStatus = IIf(strStatus = "", PRStatus.CancelledBy, strStatus & "," & PRStatus.CancelledBy)
            strStatus2 = IIf(strStatus2 = "", PRStatus.CancelledBy, strStatus2 & "," & PRStatus.CancelledBy)
        End If
        If chkReject.Checked Then
            strStatus = IIf(strStatus = "", PRStatus.RejectedBy, strStatus & "," & PRStatus.RejectedBy)
            strStatus2 = IIf(strStatus2 = "", PRStatus.RejectedBy, strStatus2 & "," & PRStatus.RejectedBy)
        End If
        If chkSource.Checked Then
            strStatus = IIf(strStatus = "", PRStatus.Approved, strStatus & "," & PRStatus.Approved)
            strStatus2 = IIf(strStatus2 = "", 99, strStatus2 & "," & 99)
        End If
        If strStatus = "" Then
            strStatus = PRStatus.Draft & "," & _
             PRStatus.Submitted & "," & _
             PRStatus.PendingApproval & "," & _
             PRStatus.Approved & "," & _
             PRStatus.ConvertedToPO & "," & _
             PRStatus.Void & "," & _
             PRStatus.CancelledBy & "," & _
             PRStatus.RejectedBy

        End If
        If strStatus2 = "" Then
            strStatus2 = PRStatus.Draft & "," & _
             PRStatus.Submitted & "," & _
             PRStatus.PendingApproval & "," & _
             PRStatus.Approved & "," & _
             PRStatus.ConvertedToPO & "," & _
             PRStatus.Void & "," & _
             PRStatus.CancelledBy & "," & _
             PRStatus.RejectedBy & "," & _
             99

        End If
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim strStatus As String = ""
        Dim strStatus2 As String = ""
        Dim ds As DataSet
        Dim objPR As New PurchaseReq2
        Dim strPRType As String = ""

        If chkContPR.Checked = True And chkNonContPR.Checked = True Then
            strPRType = ""
        ElseIf chkContPR.Checked = True Then
            strPRType = "CC"
        ElseIf chkNonContPR.Checked = True Then
            strPRType = "NonCont"
        Else
            strPRType = ""
        End If

        chk_condition(strStatus, strStatus2)
        ds = objPR.SearchPRList(txtPRNo.Text, txtItemCode.Text, txtDateFr.Text, txtDateTo.Text, "BUYER", "", strStatus, strPRType, strStatus2)

        Dim dvViewPR As DataView
        dvViewPR = ds.Tables(0).DefaultView
        dvViewPR.Sort = ViewState("SortExpression")

        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewPR.Sort += " DESC"

        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgPRList, dvViewPR)
            dtgPRList.DataSource = dvViewPR
            dtgPRList.DataBind()
        Else
            dtgPRList.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        ViewState("PageCount") = dtgPRList.PageCount
    End Function

    Private Sub dtgPRList_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPRList.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgPRList, e)
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgPRList.CurrentPageIndex = 0
        ViewState("SortAscending") = "no"
        ViewState("SortExpression") = "PRM_CREATED_DATE"
        Bindgrid(True)
    End Sub

    Private Sub dtgPRList_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPRList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkPRNo, lnkPONo As HyperLink
            Dim COUNT As Integer
            Dim objPR2 As New PurchaseReq2
            Dim ARRAY(100) As String
            Dim objDB As New EAD.DBCom

            lnkPRNo = e.Item.Cells(EnumPR.icPRNo).FindControl("lnkPRNo")
            lnkPONo = e.Item.Cells(EnumPR.icPONO).FindControl("lnkPONo")

            'If dv("PRM_PR_STATUS") = 1 Then 'Draft PR - RaisePR screen
            '    lnkPRNo.NavigateUrl = dDispatcher.direct("PR", "RaisePR.aspx", "pageid=" & strPageId & "&index=" & dv("PRM_PR_Index") & "&prid=" & dv("PRM_PR_No") & "&type=mod&mode=bc")
            'Else    'PR Detail screen
            '    lnkPRNo.NavigateUrl = dDispatcher.direct("PR", "PRDetail.aspx", "pageid=" & strPageId & "&index=" & dv("PRM_PR_Index") & "&PRNO=" & dv("PRM_PR_No") & "&type=mod&mode=bc&checkid=no&status=" & e.Item.Cells(EnumPR.icStatus).Text & "")
            'End If
            lnkPRNo.Text = dv("PRM_PR_No")

            If Common.parseNull(dv("PRM_URGENT")) = "1" Then
                Dim lnkUrgent As New HyperLink
                lnkUrgent.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(EnumPR.icPRNo).Controls.Add(lnkUrgent)
            End If

            Dim objPR As New PurchaseReq2
            If objPR.HasAttachment(dv("PRM_PR_No")) Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "clip_icon.gif")
                e.Item.Cells(EnumPR.icPRNo).Controls.Add(imgAttach)
            End If

            Dim STR, strPOM_PO_No, strRM_RFQ_Id, strPOM_PO_Index, strPOM_PO_Status As String
            Dim i, j As Integer

            If Not IsDBNull(dv("PRM_PR_TYPE")) Then
                If dv("PRM_PR_TYPE") = "CC" Then    'Contract Catalogue PR
                    e.Item.Cells(EnumPR.icPRType).Text = "Contract"
                    objPR2.GetPONoCC(dv("PRM_PR_No"), ARRAY, COUNT)   'Get PO Number
                    If ARRAY(0) <> "" Then
                        For i = 0 To COUNT - 1
                            strPOM_PO_Index = objDB.GetVal("SELECT IFNULL(POM_PO_INDEX,'') AS POM_PO_NO FROM PO_MSTR WHERE POM_PO_NO = '" & ARRAY(i) & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                            strPOM_PO_Status = objDB.GetVal("SELECT IFNULL(POM_PO_STATUS,'') AS POM_PO_NO FROM PO_MSTR WHERE POM_PO_NO = '" & ARRAY(i) & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")

                            If strPOM_PO_Status = "1" Or strPOM_PO_Status = "2" Or strPOM_PO_Status = "3" Or strPOM_PO_Status = "4" Or strPOM_PO_Status = "5" Or strPOM_PO_Status = "6" Then
                                strPOM_PO_Status = " (Official)"
                            ElseIf strPOM_PO_Status = "" Then
                                strPOM_PO_Status = ""
                            Else
                                strPOM_PO_Status = " (Internal)"
                            End If

                            'lnkPONo.Text = ARRAY(i)
                            'STR = STR & "<A>" & ARRAY(i) & "</A><br>"
                            'STR = STR & "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("PO", "PODetail.aspx", "Frm=POVIEWB2&pageid=" & strPageId & "&PO_INDEX=" & dv("POM_PO_INDEX") & "&PO_NO=" & dv("POM_PO_NO") & "&BCoyID=" & dv("POM_B_COY_ID") & "&status=" & dv("POM_PO_STATUS") & "&Caller=POviewB2&side=b&filetype=2&type=" & Request(Trim("Type")) & "&poview=1") & "')"" ><font color=#0000ff>" & ARRAY(i) & "</font></A><br/>"
                            'STR = STR & "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("PO", "PODetail.aspx", "Frm=POVIEWB2&pageid=" & strPageId & "&PO_INDEX=" & "" & "&PO_NO=" & ARRAY(i) & "&BCoyID=" & "" & "&status=" & "" & "&Caller=POviewB2&side=b&filetype=2&type=" & "" & "&poview=1") & "')"" ><font color=#0000ff>" & ARRAY(i) & "</font></A><br/>"
                            'lnkPONo.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx", "Frm=POVIEWB2&pageid=" & strPageId & "&PO_INDEX=" & "" & "&PO_NO=" & ARRAY(i) & "&BCoyID=" & HttpContext.Current.Session("CompanyId") & "&status=5&Caller=PRAll&side=b&filetype=2&type=" & "" & "&poview=1")
                            STR = STR & "<A href=""" & dDispatcher.direct("PO", "PODetail.aspx", "Frm=POVIEWB2&pageid=" & strPageId & "&PO_INDEX=" & strPOM_PO_Index & "&PO_NO=" & ARRAY(i) & "&BCoyID=" & HttpContext.Current.Session("CompanyId") & "&status=&Caller=PRAll&side=b&filetype=2&type=" & "" & "&poview=1&checkid=no") & """ ><font color=#0000ff>" & ARRAY(i) & strPOM_PO_Status & "</font></A><br/>"
                        Next
                    End If
                    'If STR = "" Then
                    '    STR = STR & "<A>" & dv("PO_NO") & "</A><br>"
                    'End If
                    If STR = "" And dv("PRM_PR_STATUS") <> 4 And dv("PRM_PR_STATUS") <> 99 Then
                        strPOM_PO_Index = objDB.GetVal("SELECT IFNULL(POM_PO_INDEX,'') AS POM_PO_NO FROM PO_MSTR WHERE POM_PO_NO = '" & dv("PO_NO") & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                        strPOM_PO_Status = objDB.GetVal("SELECT IFNULL(POM_PO_STATUS,'') AS POM_PO_NO FROM PO_MSTR WHERE POM_PO_NO = '" & dv("PO_NO") & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")

                        If strPOM_PO_Status = "1" Or strPOM_PO_Status = "2" Or strPOM_PO_Status = "3" Or strPOM_PO_Status = "4" Or strPOM_PO_Status = "5" Or strPOM_PO_Status = "6" Then
                            strPOM_PO_Status = " (Official)"
                        ElseIf strPOM_PO_Status = "" Then
                            strPOM_PO_Status = ""
                        Else
                            strPOM_PO_Status = " (Internal)"
                        End If

                        STR = STR & "<A href=""" & dDispatcher.direct("PO", "PODetail.aspx", "Frm=POVIEWB2&pageid=" & strPageId & "&PO_INDEX=" & strPOM_PO_Index & "&PO_NO=" & dv("PO_NO") & "&BCoyID=" & HttpContext.Current.Session("CompanyId") & "&status=&Caller=PRAll&side=b&filetype=2&type=" & "" & "&poview=1&checkid=no") & """ ><font color=#0000ff>" & dv("PO_NO") & "</font></A><br/>"
                    End If
                    e.Item.Cells(EnumPR.icPONO).Text = STR
                Else
                    e.Item.Cells(EnumPR.icPRType).Text = "Non-Contract"
                    'e.Item.Cells(EnumPR.icPONO).Text = ""

                    objPR2.GetPONoNonCC(dv("PRM_PR_No"), ARRAY, COUNT)   'Get PO Number
                    If ARRAY(0) <> "" Then
                        For i = 0 To COUNT - 1
                            strPOM_PO_No = objDB.GetVal("SELECT IFNULL(POM_PO_NO,'') AS POM_PO_NO FROM PO_MSTR WHERE POM_PO_NO = '" & ARRAY(i) & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                            If strPOM_PO_No <> "" Then
                                strPOM_PO_Index = objDB.GetVal("SELECT IFNULL(POM_PO_INDEX,'') AS POM_PO_NO FROM PO_MSTR WHERE POM_PO_NO = '" & ARRAY(i) & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                                strPOM_PO_Status = objDB.GetVal("SELECT IFNULL(POM_PO_STATUS,'') AS POM_PO_NO FROM PO_MSTR WHERE POM_PO_NO = '" & ARRAY(i) & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")

                                If strPOM_PO_Status = "1" Or strPOM_PO_Status = "2" Or strPOM_PO_Status = "3" Or strPOM_PO_Status = "4" Or strPOM_PO_Status = "5" Or strPOM_PO_Status = "6" Then
                                    strPOM_PO_Status = " (Official)"
                                ElseIf strPOM_PO_Status = "" Then
                                    strPOM_PO_Status = ""
                                Else
                                    strPOM_PO_Status = " (Internal)"
                                End If

                                'lnkPONo.Text = ARRAY(i)
                                ' STR = STR & "<A>" & ARRAY(i) & "</A><br>"
                                'STR = STR & "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("PO", "PODetail.aspx", "Frm=POVIEWB2&pageid=" & strPageId & "&PO_INDEX=" & dv("POM_PO_INDEX") & "&PO_NO=" & dv("POM_PO_NO") & "&BCoyID=" & dv("POM_B_COY_ID") & "&status=" & dv("POM_PO_STATUS") & "&Caller=POviewB2&side=b&filetype=2&type=" & Request(Trim("Type")) & "&poview=1") & "')"" ><font color=#0000ff>" & ARRAY(i) & "</font></A><br/>"
                                'STR = STR & "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("PO", "PODetail.aspx", "Frm=POVIEWB2&pageid=" & strPageId & "&PO_INDEX=" & "" & "&PO_NO=" & ARRAY(i) & "&BCoyID=" & "" & "&status=" & "" & "&Caller=POviewB2&side=b&filetype=2&type=" & "" & "&poview=1") & "')"" ><font color=#0000ff>" & ARRAY(i) & "</font></A><br/>"
                                'lnkPONo.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx", "Frm=POVIEWB2&pageid=" & strPageId & "&PO_INDEX=" & "" & "&PO_NO=" & ARRAY(i) & "&BCoyID=" & HttpContext.Current.Session("CompanyId") & "&status=5&Caller=PRAll&side=b&filetype=2&type=" & "" & "&poview=1")
                                STR = STR & "<A href=""" & dDispatcher.direct("PO", "PODetail.aspx", "Frm=POVIEWB2&pageid=" & strPageId & "&PO_INDEX=" & strPOM_PO_Index & "&PO_NO=" & ARRAY(i) & "&BCoyID=" & HttpContext.Current.Session("CompanyId") & "&status=&Caller=PRAll&side=b&filetype=2&type=" & "" & "&poview=1&checkid=no") & """ ><font color=#0000ff>" & ARRAY(i) & strPOM_PO_Status & "</font></A><br/>"
                            Else
                                strRM_RFQ_Id = objDB.GetVal("SELECT DISTINCT IFNULL(RM_RFQ_ID,'') AS RM_RFQ_NO  FROM rfq_mstr WHERE RM_RFQ_NO = '" & ARRAY(i) & "' AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                                objPR2.GetPONoNonCC2(strRM_RFQ_Id, ARRAY, COUNT)

                                If ARRAY(0) <> "" Then
                                    For j = 0 To COUNT - 1
                                        strPOM_PO_Index = objDB.GetVal("SELECT IFNULL(POM_PO_INDEX,'') AS POM_PO_NO FROM PO_MSTR WHERE POM_PO_NO = '" & ARRAY(j) & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                                        strPOM_PO_Status = objDB.GetVal("SELECT IFNULL(POM_PO_STATUS,'') AS POM_PO_NO FROM PO_MSTR WHERE POM_PO_NO = '" & ARRAY(j) & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")

                                        If strPOM_PO_Status = "1" Or strPOM_PO_Status = "2" Or strPOM_PO_Status = "3" Or strPOM_PO_Status = "4" Or strPOM_PO_Status = "5" Or strPOM_PO_Status = "6" Then
                                            strPOM_PO_Status = " (Official)"
                                        ElseIf strPOM_PO_Status = "" Then
                                            strPOM_PO_Status = ""
                                        Else
                                            strPOM_PO_Status = " (Internal)"
                                        End If

                                        STR = STR & "<A href=""" & dDispatcher.direct("PO", "PODetail.aspx", "Frm=POVIEWB2&pageid=" & strPageId & "&PO_INDEX=" & strPOM_PO_Index & "&PO_NO=" & ARRAY(j) & "&BCoyID=" & HttpContext.Current.Session("CompanyId") & "&status=&Caller=PRAll&side=b&filetype=2&type=" & "" & "&poview=1&checkid=no") & """ ><font color=#0000ff>" & ARRAY(j) & strPOM_PO_Status & "</font></A><br/>"
                                    Next
                                End If
                                'strPOM_PO_No = objDB.GetVal("SELECT IFNULL(POM_PO_NO,'') AS POM_PO_NO FROM PO_MSTR WHERE POM_RFQ_INDEX = '" & strRM_RFQ_Id & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                                'lnkPONo.Text = strPOM_PO_No
                                'lnkPONo.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx", "Frm=POVIEWB2&pageid=" & strPageId & "&PO_INDEX=" & "" & "&PO_NO=" & strPOM_PO_No & "&BCoyID=" & HttpContext.Current.Session("CompanyId") & "&status=5&Caller=PRAll&side=b&filetype=2&type=" & "" & "&poview=1")
                                'STR = STR & "<A href=""" & dDispatcher.direct("PO", "PODetail.aspx", "Frm=POVIEWB2&pageid=" & strPageId & "&PO_INDEX=" & "" & "&PO_NO=" & strPOM_PO_No & "&BCoyID=" & HttpContext.Current.Session("CompanyId") & "&status=5&Caller=PRAll&side=b&filetype=2&type=" & "" & "&poview=1") & """ ><font color=#0000ff>" & strPOM_PO_No & "</font></A><br/>"
                            End If
                        Next
                    End If
                    'If STR = "" Then
                    '    STR = STR & "<A>" & dv("PO_NO") & "</A><br>"
                    'End If
                    If STR = "" And dv("PRM_PR_STATUS") <> 4 And dv("PRM_PR_STATUS") <> 99 Then
                        strPOM_PO_Index = objDB.GetVal("SELECT IFNULL(POM_PO_INDEX,'') AS POM_PO_NO FROM PO_MSTR WHERE POM_PO_NO = '" & dv("PO_NO") & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                        strPOM_PO_Status = objDB.GetVal("SELECT IFNULL(POM_PO_STATUS,'') AS POM_PO_NO FROM PO_MSTR WHERE POM_PO_NO = '" & dv("PO_NO") & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")

                        If strPOM_PO_Status = "1" Or strPOM_PO_Status = "2" Or strPOM_PO_Status = "3" Or strPOM_PO_Status = "4" Or strPOM_PO_Status = "5" Or strPOM_PO_Status = "6" Then
                            strPOM_PO_Status = " (Official)"
                        ElseIf strPOM_PO_Status = "" Then
                            strPOM_PO_Status = ""
                        Else
                            strPOM_PO_Status = " (Internal)"
                        End If

                        STR = STR & "<A href=""" & dDispatcher.direct("PO", "PODetail.aspx", "Frm=POVIEWB2&pageid=" & strPageId & "&PO_INDEX=" & strPOM_PO_Index & "&PO_NO=" & dv("PO_NO") & "&BCoyID=" & HttpContext.Current.Session("CompanyId") & "&status=&Caller=PRAll&side=b&filetype=2&type=" & "" & "&poview=1&checkid=no") & """ ><font color=#0000ff>" & dv("PO_NO") & strPOM_PO_Status & "</font></A><br/>"
                    End If
                    e.Item.Cells(EnumPR.icPONO).Text = STR

                    'If strPOM_PO_No = "" Then
                    '    e.Item.Cells(EnumPR.icStatus).Text = "Sourcing"
                    'End If
                End If
            Else
                e.Item.Cells(EnumPR.icPONO).Text = ""
                e.Item.Cells(EnumPR.icPRType).Text = "Non-Contract"
            End If

            If dv("PRM_PR_STATUS") = 3 Then 'if status=Pending approval(3) showed as 'Submitted'
                e.Item.Cells(EnumPR.icStatus).Text = "Submitted"
            ElseIf dv("PRM_PR_STATUS") = 6 Then 'if status=Cancelled By(6) showed as 'Cancelled'
                e.Item.Cells(EnumPR.icStatus).Text = "Cancelled"
            ElseIf dv("PRM_PR_STATUS") = 8 Then 'if status=Rejected By(8) showed as 'Rejected'
                e.Item.Cells(EnumPR.icStatus).Text = "Rejected"
            ElseIf dv("PRM_PR_STATUS") = 9 Then 'if status=Void Draft PR(9) showed as 'Void'
                e.Item.Cells(EnumPR.icStatus).Text = "Void"
            ElseIf dv("PRM_PR_STATUS") = 99 Then
                e.Item.Cells(EnumPR.icStatus).Text = "Sourcing"
            End If
            If Not IsDBNull(dv("PRM_CREATED_DATE")) Then
                e.Item.Cells(EnumPR.icCreationDate).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("PRM_CREATED_DATE"))
            End If
            If Not IsDBNull(dv("PRM_SUBMIT_DATE")) Then
                e.Item.Cells(EnumPR.icSubmissionDate).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("PRM_SUBMIT_DATE"))
            End If
            If Not IsDBNull(dv("PRM_PR_Date")) Then
                e.Item.Cells(EnumPR.icApprovedDate).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("PRM_PR_Date"))
            End If

            If dv("PRM_PR_STATUS") = 1 Then 'Draft PR - RaisePR screen
                If Common.parseNull(dv("PRM_PR_TYPE")) = "CC" Then
                    lnkPRNo.NavigateUrl = dDispatcher.direct("PR", "RaisePR.aspx", "pageid=" & strPageId & "&index=" & dv("PRM_PR_Index") & "&prid=" & dv("PRM_PR_No") & "&type=mod&mode=cc")
                Else
                    lnkPRNo.NavigateUrl = dDispatcher.direct("PR", "RaisePR.aspx", "pageid=" & strPageId & "&index=" & dv("PRM_PR_Index") & "&prid=" & dv("PRM_PR_No") & "&type=mod&mode=bc")
                End If
            Else    'PR Detail screen
                lnkPRNo.NavigateUrl = dDispatcher.direct("PR", "PRDetail.aspx", "pageid=" & strPageId & "&index=" & dv("PRM_PR_Index") & "&PRNO=" & dv("PRM_PR_No") & "&type=mod&mode=bc&checkid=no&status=" & e.Item.Cells(EnumPR.icStatus).Text & "")

                If dv("PRM_PR_STATUS") = 99 Then
                    lnkPRNo.NavigateUrl = dDispatcher.direct("PR", "PRDetail.aspx", "pageid=" & strPageId & "&index=" & dv("PRM_PR_Index") & "&PRNO=" & dv("PRM_PR_No") & "&type=mod&mode=bc&checkid=no&status=" & "Sourcing" & "")
                End If

            End If
        End If
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_SearchPR_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PR", "RaisePR.aspx", "pageid=" & strPageId & "&type=new&mode=bc&frm=bc") & """><span>Purchase Request</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId) & """><span>Purchase Request Listing</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PR", "SearchPR_Cancellation.aspx", "pageid=" & strPageId) & """><span>Purchase Request Cancellation</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "</ul><div></div></div>"
    End Sub
End Class


