Imports AgoraLegacy
Imports eProcure.Component

Public Class CompanyAssign
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblCode As System.Web.UI.WebControls.Label
    Protected WithEvents lblDesc As System.Web.UI.WebControls.Label
    Protected WithEvents lblStartDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblEndDate As System.Web.UI.WebControls.Label
    Protected WithEvents lstBuyer As System.Web.UI.WebControls.ListBox
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Protected WithEvents cmdRemove As System.Web.UI.WebControls.Button
    Protected WithEvents lstBuyerSelected As System.Web.UI.WebControls.ListBox
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdReset As System.Web.UI.WebControls.Button
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdItem As System.Web.UI.WebControls.Button

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmdSave)
        'alButtonList.Add(cmdItem)
        htPageAccess.Add("add", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdSave)
        'alButtonList.Add(cmdItem)
        htPageAccess.Add("update", alButtonList)
        CheckButtonAccess()
        cmdSave.Enabled = (blnCanAdd Or blnCanUpdate) And viewstate("blnCmdSave")
        'cmdItem.Enabled = blnCanAdd And blnCanUpdate And blnCmdItem
        cmdReset.Enabled = blnCanAdd And blnCanUpdate
        alButtonList.Clear()
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)

        If Not IsPostBack Then
            viewstate("blnCmdSave") = True
            viewstate("blnCmdItem") = True
            lblTitle.Text = "Company Assignment"
            viewstate("index") = Request.QueryString("index")
            viewstate("caller") = Request.QueryString("caller")
            displayMaster()
            bindCompany()
        End If

        Select Case viewstate("caller")
            Case "cat"
                'lnkBack.NavigateUrl = "AddCatalogue.aspx?type=DC&index=" & viewstate("index") & "&mode=mod&pageid=" & strPageId
                lnkBack.NavigateUrl = dDispatcher.direct("Catalogue", "AddCatalogue.aspx", "type=DC&index=" & ViewState("index") & "&mode=mod&pageid=" & strPageId)
            Case "item"
                'lnkBack.NavigateUrl = "ContractCatalogue.aspx?type=DC&index=" & ViewState("index") & "&mode=mod&pageid=" & strPageId
                lnkBack.NavigateUrl = dDispatcher.direct("Catalogue", "ContractCatalogue.aspx", "type=DC&index=" & ViewState("index") & "&mode=mod&pageid=" & strPageId)
            Case "list"
                'lnkBack.NavigateUrl = "DiscountCatalogueList.aspx?type=DC&pageid=" & strPageId
                lnkBack.NavigateUrl = dDispatcher.direct("Catalogue", "DiscountCatalogueList.aspx", "type=DC&pageid=" & strPageId)
        End Select
    End Sub

    Private Function displayMaster()
        Dim objCat As New ContCat
        Dim ds As New DataSet
        ds = objCat.getSingleCatalogue(viewstate("index"), "D")

        If ds.Tables(0).Rows.Count > 0 Then
            lblCode.Text = Common.parseNull(ds.Tables(0).Rows(0)("CDM_GROUP_CODE"))
            lblDesc.Text = Common.parseNull(ds.Tables(0).Rows(0)("CDM_GROUP_DESC"))
            lblStartDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, ds.Tables(0).Rows(0)("CDM_START_DATE"))
            lblEndDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, ds.Tables(0).Rows(0)("CDM_END_DATE"))
        End If
        objCat = Nothing
    End Function

    Private Sub bindCompany()
        Dim objCat As New ContCat
        Dim dsCat As New DataSet

        dsCat = objCat.getCompanyAssign(viewstate("index"), Session("CompanyId"))
        lstBuyer.DataSource = dsCat.Tables(0).DefaultView
        lstBuyer.DataTextField = "CM_COY_NAME"
        lstBuyer.DataValueField = "CM_COY_ID"
        lstBuyer.DataBind()

        lstBuyerSelected.DataSource = dsCat.Tables(1).DefaultView
        lstBuyerSelected.DataTextField = "CM_COY_NAME"
        lstBuyerSelected.DataValueField = "CDC_B_COY_ID"
        lstBuyerSelected.DataBind()

        objCat = Nothing
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Dim lstItem As ListItem
        For Each lstItem In lstBuyer.Items
            If lstItem.Selected Then
                Dim lstNew As New ListItem
                lstNew.Text = lstItem.Text
                lstNew.Value = lstItem.Value
                lstBuyerSelected.Items.Insert(lstBuyerSelected.Items.Count, lstNew)
            End If
        Next

        Dim counter As Integer
        For counter = (lstBuyer.Items.Count - 1) To 0 Step -1
            If lstBuyer.Items(counter).Selected = True Then
                lstBuyer.Items.RemoveAt(counter)
            End If
        Next
    End Sub

    Private Sub cmdRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRemove.Click
        Dim lstItem As ListItem
        For Each lstItem In lstBuyerSelected.Items
            If lstItem.Selected Then
                Dim lstNew As New ListItem
                lstNew.Text = lstItem.Text
                lstNew.Value = lstItem.Value
                lstBuyer.Items.Insert(lstBuyer.Items.Count, lstNew)
            End If
        Next

        Dim counter As Integer
        For counter = (lstBuyerSelected.Items.Count - 1) To 0 Step -1
            If lstBuyerSelected.Items(counter).Selected = True Then
                lstBuyerSelected.Items.RemoveAt(counter)
            End If
        Next
    End Sub

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        bindCompany()
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim objCat As New ContCat
        Dim dtCat As New DataTable
        Dim strRedirect As String
        dtCat.Columns.Add("Buyer", Type.GetType("System.String"))

        Dim i As Integer
        For i = 0 To lstBuyerSelected.Items.Count - 1
            Dim dtr As DataRow
            dtr = dtCat.NewRow()
            dtr("Buyer") = lstBuyerSelected.Items(i).Value
            dtCat.Rows.Add(dtr)
        Next
        objCat.insertCatalogueCompany(viewstate("index"), dtCat)

        Select Case viewstate("caller")
            Case "cat"
                'strRedirect = "AddCatalogue.aspx?type=DC&index=" & viewstate("index") & "&mode=mod&pageid=" & strPageId
                strRedirect = dDispatcher.direct("Catalogue", "AddCatalogue.aspx", "type=DC&index=" & ViewState("index") & "&mode=mod&pageid=" & strPageId)
            Case "item"
                'strRedirect = "ContractCatalogue.aspx?type=DC&index=" & ViewState("index") & "&mode=mod&pageid=" & strPageId
                strRedirect = dDispatcher.direct("Catalogue", "ContractCatalogue.aspx", "type=DC&index=" & ViewState("index") & "&mode=mod&pageid=" & strPageId)
            Case "list"
                'strRedirect = "DiscountCatalogueList.aspx?type=DC&pageid=" & strPageId
                strRedirect = dDispatcher.direct("Catalogue", "DiscountCatalogueList.aspx", "type=DC&pageid=" & strPageId)
        End Select
        'Common.NetMsgbox(Me, MsgRecordSave, strRedirect, MsgBoxStyle.Information, "Wheel")
        Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
        objCat = Nothing
    End Sub

    Private Sub cmdItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdItem.Click
        'Response.Redirect("ContractCatalogue.aspx?mode=mod&type=DC&index=" & viewstate("index") & "&pageid=" & strPageId)
        Response.Redirect(dDispatcher.direct("Catalogue", "ContractCatalogue.aspx", "mode=mod&type=DC&index=" & ViewState("index") & "&pageid=" & strPageId))
    End Sub
End Class
