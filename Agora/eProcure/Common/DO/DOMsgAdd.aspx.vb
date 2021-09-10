Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient
Public Class DOMsgAdd
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents lblDONum As System.Web.UI.WebControls.Label
    Protected WithEvents lblSts As System.Web.UI.WebControls.Label
    Protected WithEvents cmdPreviewDO As System.Web.UI.WebControls.Button
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Hyperlink1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents Pass As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents Fail As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents lblHD As System.Web.UI.WebControls.Label
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
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
        Dim StrDONo, strHD, strDtl, strStatus As String
        Dim blnPass As Boolean

        StrDONo = Me.Request.QueryString("DONo")
        strHD = Me.Request.QueryString("HD")
        strDtl = Me.Request.QueryString("Dtl")
        strStatus = Request.QueryString("status")
        blnPass = Me.Request.QueryString("Pass")
        lblDONum.Text = StrDONo
        lblHeader.Text = strHD
        lblSts.Text = strDtl
        lblHD.Text = strHD
        If StrDONo = "" And blnPass = False Then
            Pass.Style.Item("Display") = "none"
            Fail.Style.Item("Display") = "inline"
            cmdPreviewDO.Visible = False
        ElseIf StrDONo <> "" And blnPass = False Then
            Pass.Style.Item("Display") = "none"
            Fail.Style.Item("Display") = "inline"
            If strDtl = "99" Then
                lblMsg.Text = "You have already submitted Delivery Order Number " & StrDONo
            End If
            cmdPreviewDO.Visible = False
        Else
            Fail.Style.Item("Display") = "none"
            Pass.Style.Item("Display") = "Inline"
        End If
        If strHD = "" Then
            lblHeader.Text = "Delivery Order"
        End If
        lblTitle.Text = lblHeader.Text
        'lnkBack.NavigateUrl = "POList.aspx?pageid=" & strPageId
        lnkBack.NavigateUrl = dDispatcher.direct("DO", "POList.aspx", "pageid=" & strPageId)
        'cmdPreviewDO.Attributes.Add("onclick", "PopWindow('DOReport.aspx?pageid=" & strPageId & "&DONo=" & lblDONum.Text & "&SCoyID=" & Session("CompanyID") & "')")
        If strStatus = "2" Then
            cmdPreviewDO.Visible = True
        Else
            cmdPreviewDO.Visible = False
        End If
    End Sub

    Private Sub PreviewDO()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyID"), System.AppDomain.CurrentDomain.BaseDirectory & "images\")

        Try

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT *, (SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_MSTR.POM_S_STATE) AND (CODE_CATEGORY = 's') AND (CODE_VALUE = PO_DETAILS.POD_D_COUNTRY)) AS SupplierAddrState," _
                        & "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_MSTR.POM_S_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS SupplierAddrCtry, " _
                        & "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_MSTR.POM_B_STATE) AND (CODE_CATEGORY = 's') AND (CODE_VALUE = PO_DETAILS.POD_D_COUNTRY)) AS BillAddrState, " _
                        & "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_MSTR.POM_B_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS BillAddrCtry," _
                        & "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_DETAILS.POD_D_STATE) AND (CODE_CATEGORY = 's') AND (CODE_VALUE = PO_DETAILS.POD_D_COUNTRY)) AS DelvAddrState, " _
                        & "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_DETAILS.POD_D_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS DelvAddrCtry," _
                        & "(SELECT CM_COY_NAME FROM COMPANY_MSTR AS b WHERE (CM_COY_ID = PO_MSTR.POM_B_COY_ID)) AS BuyerCompanyName " _
                        & "FROM PO_MSTR INNER JOIN PO_DETAILS ON PO_MSTR.POM_PO_NO = PO_DETAILS.POD_PO_NO AND PO_MSTR.POM_B_COY_ID = PO_DETAILS.POD_COY_ID INNER JOIN DO_MSTR ON PO_MSTR.POM_PO_INDEX = DO_MSTR.DOM_PO_INDEX INNER JOIN DO_DETAILS ON DO_MSTR.DOM_DO_NO = DO_DETAILS.DOD_DO_NO AND DO_MSTR.DOM_S_COY_ID = DO_DETAILS.DOD_S_COY_ID AND PO_DETAILS.POD_PO_LINE = DO_DETAILS.DOD_PO_LINE INNER JOIN COMPANY_MSTR ON PO_MSTR.POM_S_COY_ID = COMPANY_MSTR.CM_COY_ID " _
                        & "WHERE (PO_MSTR.POM_S_COY_ID = @prmCoyID) AND (DO_MSTR.DOM_DO_NO = @prmDONo)"
            End With

            da = New MySqlDataAdapter(cmd)
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Session("CompanyID")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmDONo", Me.Request.QueryString("DONo")))

            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewDO_DataSetPreviewDO", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            localreport.ReportPath = appPath & "DO\PreveiwDO-FTN.rdlc"
            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            TotalParameter = localreport.GetParameters.Count
            Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
            'Dim paramlist As New Generic.List(Of ReportParameter)
            For I = 0 To localreport.GetParameters.Count - 1
                GetParameter = localreport.GetParameters.Item(I).Name
                Select Case LCase(GetParameter)
                    Case "par1"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings("ReportCoyLogoPath") & strImgSrc)
                    Case Else
                End Select
            Next
            localreport.SetParameters(par)
            localreport.Refresh()

            'Dim deviceInfo As String = _
            ' "<DeviceInfo>" + _
            '     "  <OutputFormat>EMF</OutputFormat>" + _
            '     "  <PageWidth>11in</PageWidth>" + _
            '     "  <PageHeight>8.5in</PageHeight>" + _
            '     "  <MarginTop>0.25in</MarginTop>" + _
            '     "  <MarginLeft>0.25in</MarginLeft>" + _
            '     "  <MarginRight>0.25in</MarginRight>" + _
            '     "  <MarginBottom>0.25in</MarginBottom>" + _
            '     "</DeviceInfo>"
            'Dim deviceInfo As String = _
            '     "<DeviceInfo>" + _
            '         "  <OutputFormat>EMF</OutputFormat>" + _
            '         "  <PageWidth>8.27in</PageWidth>" + _
            '         "  <PageHeight>11in</PageHeight>" + _
            '         "  <MarginTop>0.25in</MarginTop>" + _
            '         "  <MarginLeft>0.25in</MarginLeft>" + _
            '         "  <MarginRight>0.25in</MarginRight>" + _
            '         "  <MarginBottom>0.25in</MarginBottom>" + _
            '         "</DeviceInfo>"
            Dim deviceInfo As String = _
                "<DeviceInfo>" + _
                    "  <OutputFormat>EMF</OutputFormat>" + _
                    "</DeviceInfo>"
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)

            Dim fs As New FileStream(appPath & "DO\DOReport.PDF", FileMode.Create)
            fs.Write(PDF, 0, PDF.Length)
            fs.Close()

            Dim strJScript As String
            strJScript = "<script language=javascript>"
            strJScript += "window.open('DOReport.PDF',null,'height=600, width=730,status= no, resizable= yes, scrollbars=yes, toolbar=no,location=center,menubar=no, top=10, left=100');"
            strJScript += "</script>"
            Response.Write(strJScript)

        Catch ex As Exception
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try
    End Sub

    Protected Sub cmdPreviewDO_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPreviewDO.Click
        PreviewDO()
    End Sub
End Class
