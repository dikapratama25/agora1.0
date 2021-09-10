Imports AgoraLegacy
Imports eProcure.Component

Public Class IRApprovalSetup
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objGlo As New AppGlobals
    Dim objDB As New EAD.DBCom
    Dim objInv As New Inventory

    Public Enum enumApp
        icLevel = 0
        icAO = 1
        icAOName = 2
        icAAO = 3
        icAAOName = 4
        icType = 5
    End Enum

    Public strAryConsolidator() As String
    Dim intRow As Integer

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cboApproval As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblIR As System.Web.UI.WebControls.Label
    Protected WithEvents dtgAO As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblRemark As System.Web.UI.WebControls.Label
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents rfvApproval As System.Web.UI.WebControls.RequiredFieldValidator
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
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        blnSorting = False
        blnPaging = False
        SetGridProperty(dtgAO)

        If Not IsPostBack Then
            intRow = 0
            lblTitle.Text = "Raise IR"
            lblIR.Text = "To Be Allocated By System"
            GenerateTab()

            Dim dsApprovalList As New DataSet
            dsApprovalList = objInv.getIRApprFlow()

            If dsApprovalList.Tables(0).Rows.Count > 0 Then
                Common.FillDdl(cboApproval, "AGM_GRP_NAME", "AGM_GRP_INDEX", dsApprovalList)
                'Dim lstItem As New ListItem
                ' Add ---Select---
                'lstItem.Value = ""
                'lstItem.Text = "---Select---"
                'cboApproval.Items.Insert(0, lstItem)
                lblMsg.Visible = False
                If dsApprovalList.Tables(0).Rows.Count = 1 Then '//Display approving officer if only has one approval workflow
                    cboApproval.SelectedIndex = 1
                    cboApproval_SelectedIndexChanged(sender, e)
                Else
                    If cboApproval.SelectedItem.Value <> "" Then
                        Bindgrid()
                    Else
                        lblRemark.Text = ""
                        dtgAO.DataBind()
                    End If
                End If
            Else
                cboApproval.Visible = False
                lblRemark.Visible = False
                lblMsg.Visible = True
                lblMsg.Text = "There is no Approval Flow defined for you."
                cmdSubmit.Visible = False
                rfvApproval.Enabled = False
            End If
        End If
        lnkBack.NavigateUrl = dDispatcher.direct("Inventory", "InventoryReq.aspx", "pageid=" & strPageId)
        Session("urlreferer") = "IRApprovalSetup"
    End Sub

    Private Function Bindgrid() As String
        Dim dsAO As New DataSet
        Dim dvViewSample As DataView

        dsAO = objInv.getIRAOList(cboApproval.SelectedItem.Value)
        ViewState("intPageRecordCnt") = dsAO.Tables(0).Rows.Count
        dvViewSample = dsAO.Tables(0).DefaultView
        intPageRecordCnt = ViewState("intPageRecordCnt")

        dtgAO.DataSource = dvViewSample
        dtgAO.DataBind()

        If intPageRecordCnt > 0 Then
            displayRemark()
        Else
            lblRemark.Text = ""
        End If

    End Function

    Public Sub dtgAO_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtgAO.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Private Sub cboApproval_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboApproval.SelectedIndexChanged
        intRow = 0

        If cboApproval.SelectedItem.Value <> "" Then
            Bindgrid()
        Else
            lblRemark.Text = ""
            dtgAO.DataBind()
        End If
    End Sub

    Private Sub dtgAO_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAO.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            intRow = intRow + 1
            e.Item.Cells(enumApp.icLevel).Text = intRow

            Dim lblType As Label
            lblType = e.Item.FindControl("lblType")
            lblType.Text = "Approval"

            Dim lblRelief As Label
            lblRelief = e.Item.FindControl("lblRelief")
            lblRelief.Text = Common.parseNull(dv("AGA_RELIEF_IND"))
            
        End If
    End Sub

    Private Sub dtgAO_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAO.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgAO, e)
    End Sub

    Private Sub displayRemark()
        Dim str As String
        
        str = "<ul type='disc'>"
        str &= "<li><strong>Approval</strong> - IR will be sent to the respective approving officer for approval </li></ul>"

        lblRemark.Text = str
    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Dim objPO As New PurchaseOrder_Buyer
        If cboApproval.SelectedItem.Value <> "" Then
            Dim objPR As New PR
            Dim objPurchaseReq2 As New PurchaseReq2
            Dim dtAO As New DataTable
            Dim intMsg As Integer
            Dim aryInv, aryInvH As New ArrayList()
            Dim strNewIR, strIssueTo, strRefNo, strRemark, strDept, strUrgent, strRequestor, strSection As String
            Dim RLoc, RSLoc, Code As String

            aryInvH = Session("aryInvHeader")
            strUrgent = aryInvH(0)
            strRequestor = aryInvH(1)
            strSection = aryInvH(2)
            strIssueTo = aryInvH(3)
            strDept = aryInvH(4)
            strRefNo = aryInvH(5)
            strRemark = aryInvH(6)
            If Session("blnLocSet") = "Y" Then
                intMsg = objInv.insertIR(Session("aryInvReq"), strNewIR, strIssueTo, strRefNo, strRemark, Code, strDept, RLoc, RSLoc, True, "Y", strRequestor, strSection, strUrgent, cboApproval.SelectedItem.Value)
            Else
                intMsg = objInv.insertIR(Session("aryInvReq"), strNewIR, strIssueTo, strRefNo, strRemark, Code, strDept, RLoc, RSLoc, True, "N", strRequestor, strSection, strUrgent, cboApproval.SelectedItem.Value)
            End If

            Select Case intMsg
                Case WheelMsgNum.Save
                    lblIR.Text = strNewIR
                    lblMsg.Text = ""
                    cmdSubmit.Visible = False
                    Session("urlreferer") = Nothing
                    Common.NetMsgbox(Me, "IR Number " & strNewIR & " " & objGlo.GetErrorMessage("00024"), dDispatcher.direct("Inventory", "InventoryReq.aspx", "pageid=" & strPageId))

                Case WheelMsgNum.NotSave
                    Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)

                Case 10
                    If RSLoc <> "---Select---" And RSLoc <> "" Then
                        Common.NetMsgbox(Me, Code & " From " & RLoc & " & " & RSLoc & " " & objGlo.GetErrorMessage("00022"), MsgBoxStyle.Information)
                    Else
                        Common.NetMsgbox(Me, Code & " From " & RLoc & " " & objGlo.GetErrorMessage("00022"), MsgBoxStyle.Information)
                    End If

            End Select

        End If
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        Session("w_InventoryReq_tabs") = "<div class=""t_entity""><ul>" & _
                                    "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "InventoryReq.aspx", "pageid=" & strPageId) & """><span>Inventory Requisition</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "InventoryReqList.aspx", "") & """><span>Inventory Requisition Listing</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "MRSAckListing.aspx", "pageid=" & strPageId) & """><span>MRS Acknowledge Listing</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "MRSListing_Requestor.aspx", "pageid=" & strPageId) & """><span>MRS Listing</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                                     "</ul><div></div></div>"


    End Sub

End Class
