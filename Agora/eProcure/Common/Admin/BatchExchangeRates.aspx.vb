Imports AgoraLegacy
Imports eProcure.Component

Public Class BatchExchangeRates
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDb As New EAD.DBCom
    Dim objGlO As New AppGlobals
    Dim objAdmin As New Admin

    Protected WithEvents txtDtFrom As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDtTo As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmd_Save As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Close As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents dtgExRate As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents cvDateNow As System.Web.UI.WebControls.CompareValidator

    Dim strMsg As String = ""
    Dim iCount As Integer

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Public Enum EnumRate
        icNo
        icCode
        icName
        icRate
    End Enum

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        SetGridProperty(dtgExRate)
        If Not Page.IsPostBack Then
            cvDateNow.ValueToCompare = Date.Today.ToShortDateString
            DisplayCurrWithRate()
            Me.cmd_Close.Attributes.Add("onclick", "window.close(); ")
            'lblBalQty.Text = Request.QueryString("iqty")
            'ViewState("Row") = 5
            'BuildRow()
            'ReCal()
            'ConstructTable()

        End If

        lblMsg.Text = ""
    End Sub

    Private Sub cmd_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_Save.Click
        Dim ds As New DataSet
        BindRate(ds)
        If Page.IsValid And validateDatagrid(strMsg) Then
            BindRate(ds)

            If objAdmin.AddBatchExRate(ds) = True Then
                Common.NetMsgbox(Me, objGlO.GetErrorMessage("00003"), MsgBoxStyle.Information)
                cmd_Save.Enabled = False
            Else
                Common.NetMsgbox(Me, objGlO.GetErrorMessage("00007"), MsgBoxStyle.Information)
            End If
        Else
            If strMsg <> "" Then
                lblMsg.Text = strMsg
            Else
                lblMsg.Text = ""
            End If
        End If

    End Sub

    Sub DisplayCurrWithRate()
        Dim ds As DataSet
        Dim dvViewCurr As DataView

        ds = objAdmin.getAllCurrWithRate()
        dvViewCurr = ds.Tables(0).DefaultView

        iCount = 0
        intPageRecordCnt = ds.Tables(0).Rows.Count

        If intPageRecordCnt > 0 Then
            dtgExRate.DataSource = dvViewCurr
            dtgExRate.PageSize = 50
            resetDatagridPageIndex(dtgExRate, dvViewCurr)
            'dtgExRate.AllowPaging = False
            dtgExRate.DataBind()
        Else
            dtgExRate.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        ViewState("PageCount") = dtgExRate.PageCount
    End Sub

    Private Sub dtgExRate_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgExRate.ItemCreated
        Grid_ItemCreated(dtgExRate, e)
    End Sub

    Private Sub dtgExRate_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgExRate.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            iCount = iCount + 1
            e.Item.Cells(EnumRate.icNo).Text = iCount

            Dim valRate As RegularExpressionValidator
            valRate = e.Item.FindControl("valRate")
            valRate.ControlToValidate = "txtRate"
            'valRate.ValidationExpression = "(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,6})?$"
            valRate.ValidationExpression = "^\d{1,6}(?:\.(?:\d{0,5})?\d)?$"
            valRate.ErrorMessage = CStr(iCount) & ". Invalid Exchange Rate"
            valRate.Text = "?"
            valRate.Display = ValidatorDisplay.Dynamic

            Dim reqValRate As RequiredFieldValidator
            reqValRate = e.Item.FindControl("reqValRate")
            reqValRate.ControlToValidate = "txtRate"
            reqValRate.ErrorMessage = CStr(iCount) & ". Exchange Rate is required"
            reqValRate.Text = "?"
            reqValRate.Display = ValidatorDisplay.Dynamic

            Dim txtRate As TextBox
            txtRate = e.Item.Cells(EnumRate.icRate).FindControl("txtRate")
            txtRate.Text = Format(dv("RATE"), "####0.000000")
        End If
    End Sub

    Private Function validateDatagrid(ByRef strMsg As String) As Boolean
        Dim dgItem As DataGridItem
        Dim strsql As String = ""
        Dim dsDate As New DataSet
        Dim strDtFrom, strDtTo, DtFrom, DtTo As Date
        Dim i As Integer
        validateDatagrid = True
        strMsg = "<ul type='disc'>"
        strDtFrom = txtDtFrom.Text
        strDtTo = txtDtTo.Text

        validateDatagrid = True
        For Each dgItem In dtgExRate.Items
            strsql = "SELECT DATE_FORMAT(CE_VALID_FROM, '%d/%m/%Y') AS CE_VALID_FROM, DATE_FORMAT(CE_VALID_TO, '%d/%m/%Y') AS CE_VALID_TO " & _
                    "FROM COMPANY_EXCHANGERATE WHERE CE_CURRENCY_CODE = '" & dgItem.Cells(EnumRate.icCode).Text & "' AND CE_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CE_DELETED ='N' " & _
                    "AND CE_VALID_FROM IS NOT NULL AND CE_VALID_TO IS NOT NULL "

            dsDate = objDb.FillDs(strsql)

            If objDb.Exist(strsql) > 0 Then
                For i = 0 To dsDate.Tables(0).Rows.Count - 1
                    DtFrom = dsDate.Tables(0).Rows(i).Item("CE_VALID_FROM").ToString()
                    DtTo = dsDate.Tables(0).Rows(i).Item("CE_VALID_TO").ToString()

                    If (strDtFrom <= DtFrom And DtFrom <= strDtTo) Or (strDtFrom <= DtTo And DtTo <= strDtTo) Or (strDtFrom <= DtFrom And DtTo <= strDtTo) Or (DtFrom <= strDtFrom And strDtTo <= DtTo) Then
                        strMsg &= "<li>" & dgItem.Cells(EnumRate.icNo).Text & ". Valid Date From/To cannot be cross or within by other valid date from/to.<ul type='disc'></ul></li>"
                        validateDatagrid = False
                        Exit For
                    End If
                Next
            End If
        Next

        strMsg &= "</ul>"

    End Function

    Private Sub BindRate(ByRef ds As DataSet)
        Dim dgItem As DataGridItem
        Dim dtr As DataRow
        Dim dtExRate As New DataTable

        dtExRate.Columns.Add("CE_CURRENCY_CODE", Type.GetType("System.String"))
        dtExRate.Columns.Add("CE_RATE", Type.GetType("System.Decimal"))
        dtExRate.Columns.Add("CE_VALID_FROM", Type.GetType("System.String"))
        dtExRate.Columns.Add("CE_VALID_TO", Type.GetType("System.String"))

        For Each dgItem In dtgExRate.Items
            dtr = dtExRate.NewRow
            Dim txtRate As TextBox

            txtRate = dgItem.FindControl("txtRate")

            If CDec(txtRate.Text) > 0 Then
                dtr("CE_CURRENCY_CODE") = dgItem.Cells(EnumRate.icCode).Text
                dtr("CE_RATE") = txtRate.Text
                dtr("CE_VALID_FROM") = txtDtFrom.Text
                dtr("CE_VALID_TO") = txtDtTo.Text
                dtExRate.Rows.Add(dtr)
            End If

        Next
        ds.Tables.Add(dtExRate)

    End Sub
End Class
