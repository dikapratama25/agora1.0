'//Attachment Ind
Imports AgoraLegacy
Imports eProcure.Component
Public Class SearchApp_All
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents txtBuyer As System.Web.UI.WebControls.TextBox
    Protected WithEvents Table2 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblStartDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblEndDate As System.Web.UI.WebControls.Label
    Dim objGlobal As New AppGlobals
    Dim dDispatcher As New AgoraLegacy.dispatcher

    Public Enum EnumPR
        icPRNo
        icPRType
        icCreationDate
        icBuyer
        icBuyerDept
        icVendor
        'icCurrency
        'icAmt
        icStatus
        icPONo
    End Enum
    'Dim strCaller As String
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
        blnCheckBox = False
        SetGridProperty(dtgPRList)

        MyBase.Page_Load(sender, e)

        If Not Page.IsPostBack Then
            Session("strURL") = strCallFrom
            txtDateFr.Text = DateAdd("m", -6, DateTime.Now.ToShortDateString())
            hidDateS.Value = txtDateFr.Text
            txtDateTo.Text = DateTime.Now.ToShortDateString()
            hidDateE.Value = txtDateTo.Text
            GenerateTab()
        End If
        Session("urlreferer") = "PRRej"
    End Sub

    Public Sub dtgPOList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgPRList.PageIndexChanged
        dtgPRList.CurrentPageIndex = e.NewPageIndex
        Bindgrid(True)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgPRList.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objPR As New PurchaseReq2

        '//Retrieve Data from Database
        Dim ds As DataSet
        Dim strAOAction As String = ""
        Dim strInclude As String = ""
        Dim strIncludeHold As String = ""
        Dim strPRType As String = ""
        Dim strMsg As String
        Dim comparedt As Date
        comparedt = DateAdd("m", -6, CDate(txtDateTo.Text))

        If CDate(txtDateFr.Text) < comparedt Then
            strMsg = "Start date " & objGlobal.GetErrorMessage("00015") & " 6 months"
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
            Return False
        End If

        If chkApproved.Checked Then
            strAOAction = "Approved"
        End If

        If chkReject.Checked Then
            strAOAction = "Rejected"
        End If

        If chkInclude.Checked Then
            strInclude = "Included"
        End If

        If chkIncludeHold.Checked Then
            strIncludeHold = "IncludedHold"
        End If

        If chkApproved.Checked And chkReject.Checked Then strAOAction = ""

        If chkContPR.Checked = True And chkNonContPR.Checked = True Then
            strPRType = ""
        ElseIf chkContPR.Checked = True Then
            strPRType = "CC"
        ElseIf chkNonContPR.Checked = True Then
            strPRType = "NonCont"
        Else
            strPRType = ""
        End If

        ds = objPR.getPRListForApproval(txtPRNo.Text, txtVendor.Text, txtDateFr.Text, txtDateTo.Text, "", "app", "", strAOAction, strPRType, strInclude, strIncludeHold)

        '//for sorting asc or desc
        Dim dvViewPR As DataView
        dvViewPR = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewPR.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewPR.Sort += " DESC"
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count

        '//bind datagrid
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

    Private Sub dtgPOList_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPRList.ItemCreated
        Grid_ItemCreated(dtgPRList, e)
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgPRList.CurrentPageIndex = 0
        ViewState("SortAscending") = "no"
        ViewState("SortExpression") = "PRM_SUBMIT_DATE"
        Bindgrid(True)
    End Sub

    Private Sub dtgPRList_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPRList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkPRNo, lnkPONo As HyperLink
            Dim intStatus, strPOM_PO_Index As String
            Dim COUNT As Integer
            Dim ARRAY(100) As String
            Dim objDB As New EAD.DBCom

            'intStatus = Common.parseNull(dv("PRM_PR_STATUS"))
            'If intStatus = PRStatus.RejectedBy Then
            '    e.Item.Cells(EnumPR.icStatus).Text = "Rejected"
            'ElseIf intStatus = PRStatus.HeldBy Then
            '    e.Item.Cells(EnumPR.icStatus).Text = "On Hold"
            'Else
            '    e.Item.Cells(EnumPR.icStatus).Text = "Approved"
            'End If
            lnkPRNo = e.Item.Cells(EnumPR.icPRNo).FindControl("lnkPRNo")
            lnkPONo = e.Item.Cells(EnumPR.icPONo).FindControl("lnkPONo")
            'lnkPRNo.NavigateUrl = dDispatcher.direct("PR", "PRDetail.aspx", "side=b&caller=SearchPO_ALL&status=" & intStatus & "&BCoyID=" & Session("CompanyID") & "&pageid=" & strPageId & "&index=" & dv("PRM_PR_Index") & "&PR_No=" & dv("PRM_PR_No"))
            lnkPRNo.NavigateUrl = dDispatcher.direct("PR", "PRDetail.aspx", "pageid=" & strPageId & "&index=" & dv("PRM_PR_Index") & "&PRNO=" & dv("PRM_PR_No") & "&type=mod&mode=bc")
            lnkPRNo.Text = dv("PRM_PR_No")

            If Common.parseNull(dv("PRM_PR_STATUS")) = PRStatus.CancelledBy Or Common.parseNull(dv("PRM_PR_STATUS")) = PRStatus.HeldBy Then
                If IsDBNull(dv("CHANGED_BY_NAME")) Then
                    e.Item.Cells(EnumPR.icStatus).Text = e.Item.Cells(EnumPR.icStatus).Text & " (" & Common.parseNull(dv("PRM_STATUS_CHANGED_BY")) & ")"
                Else
                    e.Item.Cells(EnumPR.icStatus).Text = e.Item.Cells(EnumPR.icStatus).Text & " (" & dv("CHANGED_BY_NAME") & ")"
                End If
            End If

            If Common.parseNull(dv("PRM_PR_STATUS")) = PRStatus.HeldBy Then
                e.Item.Cells(EnumPR.icStatus).Text = "Held by " & dv("NAME") & ""
            End If

            If e.Item.Cells(EnumPR.icVendor).Text <> "Multiple Vendors" Then
                'e.Item.Cells(EnumPR.icVendor).Text = "<a href='../RFQ/RFQ_VendorDetail.aspx?pageid=" & strPageId & "&v_com_id=" & Common.parseNull(dv("PRM_S_Coy_ID")) & "'>" & e.Item.Cells(EnumPR.icVendor).Text & "</a>"
                e.Item.Cells(EnumPR.icVendor).Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "Frm=RFQ_Outstg_List&pageid=" & strPageId & "&RFQ_Num=" & Server.UrlEncode(Common.parseNull(dv("PRM_PR_No"))) & "&v_com_id=" & Common.parseNull(dv("SNAMEID"))) & """ ><font color=#0000ff>" & Common.parseNull(dv("SNAME")) & "</font></A>"
            Else

                'Dim strSName, strSNameID As String
                'strSNameID = objDb.GetVal("SELECT IFNULL(PRD_S_COY_ID, '') PRD_S_COY_ID FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PR_NO = '" & Common.parseNull(dv("PRM_PR_No")) & "' AND PRD_VENDOR_ITEM_CODE <> '' HAVING COUNT(DISTINCT IFNULL(PRD_VENDOR_ITEM_CODE, '')) = 1")
                'strSName = objDb.GetVal("SELECT IFNULL((SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = PR_DETAILS.PRD_S_COY_ID), '') PRD_S_COY_ID FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PR_NO = '" & Common.parseNull(dv("PRM_PR_No")) & "' AND PRD_VENDOR_ITEM_CODE <> '' HAVING COUNT(DISTINCT IFNULL(PRD_VENDOR_ITEM_CODE, '')) = 1")

                'If strSNameID <> "" Then
                '    e.Item.Cells(EnumPR.icVendor).Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "Frm=RFQ_Outstg_List&pageid=" & strPageId & "&RFQ_Num=" & Server.UrlEncode(Common.parseNull(dv("PRM_PR_No"))) & "&v_com_id=" & strSNameID) & """ ><font color=#0000ff>" & strSName & "</font></A>"
                'Else
                '    'e.Item.Cells(EnumPR.icVendor).Text = "<a href='../PR/Multi_VendorDetails.aspx?caller=AO&pageid=" & strPageId & "&PRNum=" & Common.parseNull(dv("PRM_PR_NO")) & "'>" & e.Item.Cells(EnumPR.icVendor).Text & "</A>"
                '    e.Item.Cells(EnumPR.icVendor).Text = "<a href=""" & dDispatcher.direct("PR", "Multi_VendorDetails.aspx", "caller=AO&pageid=" & strPageId & "&PRNum=" & Common.parseNull(dv("PRM_PR_NO")) & "") & """ ><font color=#0000ff>" & Common.parseNull(dv("SNAME")) & "</font></A>"
                'End If

                e.Item.Cells(EnumPR.icVendor).Text = "<a href=""" & dDispatcher.direct("PR", "Multi_VendorDetails.aspx", "caller=AO&pageid=" & strPageId & "&PRNum=" & Common.parseNull(dv("PRM_PR_NO")) & "") & """ ><font color=#0000ff>" & Common.parseNull(dv("SNAME")) & "</font></A>"
            End If

            If Common.parseNull(dv("PRM_URGENT")) = "1" Then
                Dim lnkUrgent As New HyperLink
                lnkUrgent.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(EnumPR.icPRNo).Controls.Add(lnkUrgent)
            End If

            Dim objPR As New PurchaseReq2
            If objPR.HasAttachment(dv("PRM_PR_No"), "PR") Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/images", "clip_icon.gif")
                e.Item.Cells(EnumPR.icPRNo).Controls.Add(imgAttach)
            End If

            If Not IsDBNull(dv("PRM_RFQ_INDEX")) AndAlso CStr(dv("PRM_RFQ_INDEX")) <> "" Then
                Dim lnkRFQ As New HyperLink
                Dim strRFQNo, strRFQName As String
                objPR.getRFQName(dv("PRM_RFQ_INDEX"), strRFQNo, strRFQName)
                lnkRFQ.ImageUrl = dDispatcher.direct("Plugins/Images", "Q-3 Icon (10x10).jpg")
                lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Frm=SearchApp_All&pageid=" & strPageId & "&side=other&RFQ_Num=" & strRFQNo & "&RFQ_ID=" & dv("PRM_RFQ_INDEX") & "&RFQ_Name=" & Server.UrlEncode(strRFQName))
                lnkRFQ.ToolTip = "Click here to view quotation comparison"
                e.Item.Cells(EnumPR.icPRNo).Controls.Add(lnkRFQ)
            End If

            Dim STR As String
            Dim i, j As Integer
            Dim strPOM_PO_No, strRM_RFQ_Id As String

            'objPR.GetPONo(dv("PRM_PR_No"), ARRAY, COUNT)   'Get PO Number
            'If ARRAY(0) <> "" Then
            '    For i = 0 To COUNT - 1
            '        STR = STR & "<A>" & ARRAY(i) & "</A><br>"
            '    Next
            'End If
            'e.Item.Cells(EnumPR.icPONo).Text = STR

            'If Not IsDBNull(dv("PRM_PR_TYPE")) Then
            '    If dv("PRM_PR_TYPE") = "CC" Then    'Contract Catalogue PR
            '        e.Item.Cells(EnumPR.icPRType).Text = "Contract"
            '        objPR.GetPONo(dv("PRM_PR_No"), ARRAY, COUNT)   'Get PO Number
            '        If ARRAY(0) <> "" Then
            '            For i = 0 To COUNT - 1
            '                'STR = STR & "<A>" & ARRAY(i) & "</A><br>"
            '                STR = STR & "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("PO", "PODetail.aspx", "side=b&caller=PRAPP&status=" & dv("POM_PO_STATUS") & "&BCoyID=" & Session("CompanyID") & "&pageid=" & strPageId & "&index=" & dv("POM_PO_Index") & "&PO_No=" & ARRAY(i)) & "')"" ><font color=#0000ff>" & ARRAY(i) & "</font></A><br/>"
            '            Next
            '        End If
            '        ' STR = STR & "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "Frm=RFQ_VenList&pageid=" & strPageId & "&v_com_id=" & vendid(i)) & "')"" ><font color=#0000ff>" & com_name(i) & "</font></A><br/>"
            '        If STR = "" Then
            '            STR = STR & "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("PO", "PODetail.aspx", "side=b&caller=PRAPP&status=" & dv("POM_PO_STATUS") & "&BCoyID=" & Session("CompanyID") & "&pageid=" & strPageId & "&index=" & dv("POM_PO_Index") & "&PO_No=" & dv("PO_NO")) & "')"" ><font color=#0000ff>" & dv("PO_NO") & "</font></A><br/>"
            '        End If
            '        e.Item.Cells(EnumPR.icPONo).Text = STR
            '    Else
            '        e.Item.Cells(EnumPR.icPONo).Text = ""
            '        e.Item.Cells(EnumPR.icPRType).Text = "Non-Contract"
            '    End If
            'Else
            '    e.Item.Cells(EnumPR.icPONo).Text = ""
            '    e.Item.Cells(EnumPR.icPRType).Text = "Non-Contract"
            'End If

            If Not IsDBNull(dv("PRM_PR_TYPE")) Then
                If dv("PRM_PR_TYPE") = "CC" Then    'Contract Catalogue PR
                    e.Item.Cells(EnumPR.icPRType).Text = "Contract"
                    objPR.GetPONoCC(dv("PRM_PR_No"), ARRAY, COUNT)   'Get PO Number
                    If ARRAY(0) <> "" Then
                        For i = 0 To COUNT - 1
                            'lnkPONo.Text = ARRAY(i)
                            'lnkPONo.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx", "side=b&caller=PRAPPALL&status=" & dv("POM_PO_STATUS") & "&BCoyID=" & Session("CompanyID") & "&pageid=" & strPageId & "&index=" & dv("POM_PO_Index") & "&PO_No=" & ARRAY(i))
                            'STR = STR & "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("PO", "PODetail.aspx", "side=b&caller=PRAPPALL&status=" & dv("POM_PO_STATUS") & "&BCoyID=" & Session("CompanyID") & "&pageid=" & strPageId & "&index=" & dv("POM_PO_Index") & "&PO_No=" & ARRAY(i)) & "')"" ><font color=#0000ff>" & ARRAY(i) & "</font></A><br/>"
                            STR = STR & "<A href=""" & dDispatcher.direct("PO", "PODetail.aspx", "side=b&caller=PRAPPALL&status=&BCoyID=" & Session("CompanyID") & "&pageid=" & strPageId & "&PO_INDEX=" & dv("POM_PO_Index") & "&PO_No=" & ARRAY(i)) & """ ><font color=#0000ff>" & ARRAY(i) & "</font></A><br/>"
                        Next
                    End If
                    'If STR = "" Then
                    '    STR = STR & "<A>" & dv("PO_NO") & "</A><br>"
                    'End If
                    If STR = "" And dv("PRM_PR_STATUS") <> 4 Then
                        STR = STR & "<A href=""" & dDispatcher.direct("PO", "PODetail.aspx", "side=b&caller=PRAPPALL&status=&BCoyID=" & Session("CompanyID") & "&pageid=" & strPageId & "&PO_INDEX=" & dv("POM_PO_Index") & "&PO_No=" & dv("PO_NO")) & """ ><font color=#0000ff>" & dv("PO_NO") & "</font></A><br/>"
                    End If
                    e.Item.Cells(EnumPR.icPONo).Text = STR
                Else
                    e.Item.Cells(EnumPR.icPRType).Text = "Non-Contract"
                    'e.Item.Cells(EnumPR.icPONO).Text = ""

                    objPR.GetPONoNonCC(dv("PRM_PR_No"), ARRAY, COUNT)   'Get PO Number
                    If ARRAY(0) <> "" Then
                        For i = 0 To COUNT - 1
                            strPOM_PO_No = objDB.GetVal("SELECT IFNULL(POM_PO_NO,'') AS POM_PO_NO FROM PO_MSTR WHERE POM_PO_NO = '" & ARRAY(i) & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                            If strPOM_PO_No <> "" Then
                                strPOM_PO_Index = objDB.GetVal("SELECT IFNULL(POM_PO_INDEX,'') AS POM_PO_NO FROM PO_MSTR WHERE POM_PO_NO = '" & ARRAY(i) & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                                'lnkPONo.Text = ARRAY(i)
                                'lnkPONo.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx", "side=b&caller=PRAPPALL&status=5&BCoyID=" & Session("CompanyID") & "&pageid=" & strPageId & "&index=" & "" & "&PO_No=" & ARRAY(i))
                                'STR = STR & "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("PO", "PODetail.aspx", "side=b&caller=PRAPPALL&status=5&BCoyID=" & Session("CompanyID") & "&pageid=" & strPageId & "&index=" & "" & "&PO_No=" & ARRAY(i)) & "')"" ><font color=#0000ff>" & ARRAY(i) & "</font></A><br/>"
                                STR = STR & "<A href=""" & dDispatcher.direct("PO", "PODetail.aspx", "side=b&caller=PRAPPALL&status=&BCoyID=" & Session("CompanyID") & "&pageid=" & strPageId & "&PO_INDEX=" & strPOM_PO_Index & "&PO_No=" & ARRAY(i)) & """ ><font color=#0000ff>" & ARRAY(i) & "</font></A><br/>"
                            Else
                                strRM_RFQ_Id = objDB.GetVal("SELECT DISTINCT IFNULL(RM_RFQ_ID,'') AS RM_RFQ_NO  FROM rfq_mstr WHERE RM_RFQ_NO = '" & ARRAY(i) & "' AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                                'strPOM_PO_No = objDB.GetVal("SELECT IFNULL(POM_PO_NO,'') AS POM_PO_NO FROM PO_MSTR WHERE POM_RFQ_INDEX = '" & strRM_RFQ_Id & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")

                                objPR.GetPONoNonCC2(strRM_RFQ_Id, ARRAY, COUNT)
                                If ARRAY(0) <> "" Then
                                    For j = 0 To COUNT - 1
                                        strPOM_PO_Index = objDB.GetVal("SELECT IFNULL(POM_PO_INDEX,'') AS POM_PO_NO FROM PO_MSTR WHERE POM_PO_NO = '" & ARRAY(j) & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                                        'lnkPONo.Text = strPOM_PO_No
                                        'lnkPONo.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx", "side=b&caller=PRAPPALL&status=5&BCoyID=" & Session("CompanyID") & "&pageid=" & strPageId & "&index=" & "" & "&PO_No=" & strPOM_PO_No)
                                        'STR = STR & "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("PO", "PODetail.aspx", "side=b&caller=PRAPPALL&status=5&BCoyID=" & Session("CompanyID") & "&pageid=" & strPageId & "&index=" & "" & "&PO_No=" & strPOM_PO_No) & "')"" ><font color=#0000ff>" & strPOM_PO_No & "</font></A><br/>"
                                        STR = STR & "<A href=""" & dDispatcher.direct("PO", "PODetail.aspx", "side=b&caller=PRAPPALL&status=&BCoyID=" & Session("CompanyID") & "&pageid=" & strPageId & "&PO_INDEX=" & strPOM_PO_Index & "&PO_No=" & ARRAY(j)) & """ ><font color=#0000ff>" & ARRAY(j) & "</font></A><br/>"
                                    Next
                                End If
                                
                            End If
                        Next
                    End If
                    'If STR = "" Then
                    '    STR = STR & "<A>" & dv("PO_NO") & "</A><br>"
                    'End If
                    If STR = "" And dv("PRM_PR_STATUS") <> 4 Then
                        strPOM_PO_Index = objDB.GetVal("SELECT IFNULL(POM_PO_INDEX,'') AS POM_PO_NO FROM PO_MSTR WHERE POM_PO_NO = '" & dv("PO_NO") & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                        STR = STR & "<A href=""" & dDispatcher.direct("PO", "PODetail.aspx", "side=b&caller=PRAPPALL&status=&BCoyID=" & Session("CompanyID") & "&pageid=" & strPageId & "&PO_INDEX=" & strPOM_PO_Index & "&PO_No=" & dv("PO_NO")) & """ ><font color=#0000ff>" & dv("PO_NO") & "</font></A><br/>"
                    End If
                    e.Item.Cells(EnumPR.icPONo).Text = STR
                End If
            Else
                e.Item.Cells(EnumPR.icPONo).Text = ""
                e.Item.Cells(EnumPR.icPRType).Text = "Non-Contract"
            End If



            objPR = Nothing
            e.Item.Cells(EnumPR.icCreationDate).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("PRM_SUBMIT_DATE"))

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

        Session("w_SearchPRAO_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PR", "SearchPR_AO.aspx", "pageid=" & strPageId) & """><span>Approval List</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PR", "SearchApp_All.aspx", "pageid=" & strPageId) & """><span>Approved / Rejected Listing</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "</ul><div></div></div>"
    End Sub
End Class


