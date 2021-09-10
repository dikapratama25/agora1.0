Imports eProcure.Component
Imports AgoraLegacy

Public Class Homepage
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblComp As System.Web.UI.WebControls.Label
    Protected WithEvents lblWelMsg As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim dDispatcher As New AgoraLegacy.dispatcher
    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    Dim objComp As New Companies
    '    Dim sCompType As String = objComp.GetCompanyType
    '    Dim objUserRoles As New UserRoles
    '    Dim strFixRole As String


    '    'If sCompType = "BUYER" Then
    '    '    lblWelMsg.Text = "Welcome to eBuyer"
    '    'ElseIf sCompType = "VENDOR" Then
    '    '    lblWelMsg.Text = "Welcome to eVendor"
    '    'Else
    '    '    lblWelMsg.Text = "Welcome to eProcure"
    '    'End If
    '    'AgoraLegacy.Common.NetMsgbox(Me, Session.SessionID)
    '    'lblComp.Text = Session("CompanyIdToken")

    '    'Michelle - To direct to the respective Dashboard according to their fix role
    '    strFixRole = objUserRoles.get_UserFixedRole()
    '    If sCompType = "VENDOR" Then
    '        If strFixRole.IndexOf("Super Admin") <> 0 Then Response.Redirect(dDispatcher.direct("Dashboard", "Vendor.aspx"))
    '    Else 'Buyer Company
    '        If strFixRole.IndexOf("Purchasing Manager") <> -1 Then 'Purchasing Manager
    '            Response.Redirect(dDispatcher.direct("Dashboard", "PurMgr.aspx"))
    '        ElseIf strFixRole.IndexOf("Approving Officer") <> -1 Or strFixRole.IndexOf("Finance Manager") <> -1 Then 'Approval Officer
    '            Response.Redirect(dDispatcher.direct("Dashboard", "FMnAO.aspx"))
    '        ElseIf strFixRole.IndexOf("Buyer") <> -1 Or strFixRole.IndexOf("Purchasing Officer") <> -1 Then 'Buyer
    '            Response.Redirect(dDispatcher.direct("Dashboard", "Buyer.aspx"))
    '        ElseIf strFixRole.IndexOf("Store Keeper") <> -1 Then 'StoreKeeper
    '            Response.Redirect(dDispatcher.direct("Dashboard", "StoreKeeper.aspx"))
    '        ElseIf strFixRole.IndexOf("Second Level Receiver") <> -1 Then 'IQC
    '            Response.Redirect(dDispatcher.direct("Dashboard", "IQC.aspx"))
    '        End If
    '    End If








    'End Sub
    '##################################craven new dashboard#####################################
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objComp As New Companies
        Dim sCompType As String = objComp.GetCompanyType
        Dim objUserRoles As New UserRoles
        Dim strFixRole As DataSet
        Dim aryFixRole As New ArrayList

        'If sCompType = "BUYER" Then
        '    lblWelMsg.Text = "Welcome to eBuyer"
        'ElseIf sCompType = "VENDOR" Then
        '    lblWelMsg.Text = "Welcome to eVendor"
        'Else
        '    lblWelMsg.Text = "Welcome to eProcure"
        'End If
        'AgoraLegacy.Common.NetMsgbox(Me, Session.SessionID)
        'lblComp.Text = Session("CompanyIdToken")

        'Michelle - To direct to the respective Dashboard according to their fix role
        strFixRole = objUserRoles.get_DashboardUserFixedRole()
        Dim dtgItem As DataRow
        For Each dtgItem In strFixRole.Tables(0).Rows
            aryFixRole.Add(dtgItem("UGM_FIXED_ROLE"))
        Next
        Session("MixUserRole") = aryFixRole
        Response.Redirect(dDispatcher.direct("Dashboard", "AllDashBoard.aspx"))
        'If sCompType = "VENDOR" Then

        '    If aryFixRole.IndexOf("Super Admin") <> 0 Then
        '        Session("UserRole") = "Vendor"
        '        Response.Redirect(dDispatcher.direct("Dashboard", "Vendor.aspx"))
        '    End If
        'Else 'Buyer Company
        '    If aryFixRole.IndexOf("Purchasing Manager") <> -1 Then 'Purchasing Manager
        '        Session("UserRole") = "Purchasing Manager"
        '        Response.Redirect(dDispatcher.direct("Dashboard", "PurMgr.aspx"))
        '    ElseIf aryFixRole.IndexOf("Approving Officer") <> -1 Or aryFixRole.IndexOf("Finance Manager") <> -1 Then 'Approval Officer
        '        If aryFixRole.IndexOf("Approving Officer") <> -1 Then
        '            Session("UserRole") = "Approving Officer"
        '        ElseIf aryFixRole.IndexOf("Finance Manager") <> -1 Then
        '            Session("UserRole") = "Finance Manager"
        '        End If
        '        Response.Redirect(dDispatcher.direct("Dashboard", "FMnAO.aspx"))
        '    ElseIf aryFixRole.IndexOf("Buyer") <> -1 Or aryFixRole.IndexOf("Purchasing Officer") <> -1 Then 'Buyer
        '        If aryFixRole.IndexOf("Buyer") <> -1 Then
        '            Session("UserRole") = "Buyer"
        '        ElseIf aryFixRole.IndexOf("Purchasing Officer") <> -1 Then
        '            Session("UserRole") = "Purchasing Officer"
        '        End If
        '        Response.Redirect(dDispatcher.direct("Dashboard", "Buyer.aspx"))
        '    ElseIf aryFixRole.IndexOf("Store Keeper") <> -1 Then 'StoreKeeper
        '        Session("UserRole") = "SK"
        '        Response.Redirect(dDispatcher.direct("Dashboard", "StoreKeeper.aspx"))
        '    ElseIf aryFixRole.IndexOf("Second Level Receiver") <> -1 Then 'IQC
        '        Session("UserRole") = "IQC"
        '        Response.Redirect(dDispatcher.direct("Dashboard", "IQC.aspx"))
        '    End If
        'End If




        '    If sCompType = "VENDOR" Then

        '        If strFixRole.IndexOf("Super Admin") <> 0 Then
        '            Session("UserRole") = "Vendor"
        '            Response.Redirect(dDispatcher.direct("Dashboard", "AllDashBoard.aspx"))
        '        End If
        '    Else 'Buyer Company
        '        If strFixRole.IndexOf("Purchasing Manager") <> -1 Then 'Purchasing Manager
        '            Session("UserRole") = "PM"
        '            Response.Redirect(dDispatcher.direct("Dashboard", "AllDashBoard.aspx"))
        '        ElseIf strFixRole.IndexOf("Approving Officer") <> -1 Or strFixRole.IndexOf("Finance Manager") <> -1 Then 'Approval Officer
        '            Session("UserRole") = "FMAO"
        '            Response.Redirect(dDispatcher.direct("Dashboard", "AllDashBoard.aspx"))
        '        ElseIf strFixRole.IndexOf("Buyer") <> -1 Or strFixRole.IndexOf("Purchasing Officer") <> -1 Then 'Buyer
        '            Session("UserRole") = "PO"
        '            Response.Redirect(dDispatcher.direct("Dashboard", "AllDashBoard.aspx"))
        '        ElseIf strFixRole.IndexOf("Store Keeper") <> -1 Then 'StoreKeeper
        '            Session("UserRole") = "SK"
        '            Response.Redirect(dDispatcher.direct("Dashboard", "AllDashBoard.aspx"))
        '        ElseIf strFixRole.IndexOf("Second Level Receiver") <> -1 Then 'IQC
        '            Session("UserRole") = "IQC"
        '            Response.Redirect(dDispatcher.direct("Dashboard", "AllDashBoard.aspx"))
        '        End If
        '    End If
    End Sub


End Class
