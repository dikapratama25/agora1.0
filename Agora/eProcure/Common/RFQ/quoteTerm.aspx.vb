Imports AgoraLegacy
Imports eProcure.Component

Public Class quoteTerm
    Inherits AgoraLegacy.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lbl_QuoteVal As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_ContactPer As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_ContNum As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Email As System.Web.UI.WebControls.Label
    Protected WithEvents txt_remark As System.Web.UI.WebControls.TextBox
    Protected WithEvents pnlAttach2 As System.Web.UI.WebControls.Panel
    Protected WithEvents lbl_pay_term As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_paymeth As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_ship_term As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_shipmode As System.Web.UI.WebControls.Label
    Dim objFile As New FileManagement

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
        CheckButtonAccess(True)
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' cmd_pre2 = false rfq buyer side 
        ' cmd_pre=true  history.back can use in both side 
        MyBase.Page_Load(sender, e)

        If Not Page.IsPostBack Then

            Dim objrfq As New RFQ
            Dim vcomid3 As String = Request(Trim("vcomid"))
            viewstate("vcomid") = vcomid3
            Me.txt_remark.Text = Session("remark")
            Me.lbl_QuoteVal.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, Request(Trim("validity")))
            '    Me.txt_remark.Text = Request(Trim("remark"))
            Dim pay_term As String = objrfq.get_codemstr(Request(Trim("payTerm")), "PT")
            Dim pay_meth As String = objrfq.get_codemstr(Request(Trim("payMeth")), "PM")
            Dim ship_mode As String = objrfq.get_codemstr(Request(Trim("shipMode")), "SM")
            Dim ship_term As String = objrfq.get_codemstr(Request(Trim("ShipTerm")), "ST")

            Me.lbl_pay_term.Text = pay_term
            Me.lbl_paymeth.Text = pay_meth
            Me.lbl_ship_term.Text = ship_mode
            Me.lbl_shipmode.Text = ship_term

            Me.lbl_ContactPer.Text = Request(Trim("con_person"))
            Me.lbl_ContNum.Text = Request(Trim("con_num"))
            Me.lbl_Email.Text = Request(Trim("email"))

            If pay_term = "" Then
                lbl_pay_term.Text = "Not Applicable"
            End If

            If pay_meth = "" Then
                lbl_paymeth.Text = "Not Applicable"
            End If

            If ship_mode = "" Then
                Me.lbl_ship_term.Text = "Not Applicable"
               
            End If

            If ship_term = "" Then
                lbl_shipmode.Text = "Not Applicable"
               
            End If

            displayAttachFile2()

        Else

        End If

    End Sub


    Private Sub displayAttachFile2()
        Dim objRFQ As New RFQ
        Dim dsAttach As New DataSet
        Dim drvAttach As DataView
        Dim i As Integer
        Dim strFile, strFile1, strURL, strTemp As String

        dsAttach = objRFQ.getRFQTempAttach2(Request(Trim("RFQ_Num")))

        pnlAttach2.Controls.Clear()
        drvAttach = dsAttach.Tables(0).DefaultView
        If drvAttach.Count > 0 Then
            For i = 0 To drvAttach.Count - 1
                strFile = drvAttach(i)("CDA_ATTACH_FILENAME")
                strFile1 = drvAttach(i)("CDA_HUB_FILENAME")
                '*************************meilai 25/2/05****************************
                'strURL = "<A HREF=../FileDownload.aspx?file=" & Server.UrlEncode(strFile1) & "&actualfile=" & Server.UrlEncode(strFile) & "&type=" & EnumDownLoadType.DocAttachment & "&doctype=Quotation>" & strFile & "</A>"
                strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DocAttachment, "Quotation", EnumUploadFrom.FrontOff)
                '*************************meilai************************************
                Dim lblBr As New Label
                Dim lblFile As New Label

                lblFile.Text = "&nbsp;" & i + 1 & ") " & strURL & " (" & drvAttach(i)("CDA_FILESIZE") & "KB) "
                lblBr.Text = "<BR>"
                pnlAttach2.Controls.Add(lblFile)
                pnlAttach2.Controls.Add(lblBr)
            Next
        Else
            Dim lblFile As New Label
            lblfile.Text = "No Files Attached"
            pnlAttach2.Controls.Add(lblFile)
        End If

    End Sub

End Class
