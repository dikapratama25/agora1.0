Public Class callReport
    Inherits AppCommon.AppBaseClass
    Protected WithEvents TextBox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBox2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBox3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBox4 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Button2 As System.Web.UI.WebControls.Button
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button

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

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        'Put user code to initialize the page here
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim crParam As New Hashtable
        'crParam.Add("buyerCompanyIDParam", "MBB")
        'crParam.Add("BCOYID", "MBB")
        'crParam.Add("userIDParam", "MOO")
        'crParam.Add("dateFromParam", "01/01/2004")
        'crParam.Add("dateToParam", Now.Today)
        'Wheel.Components.ReportViewer.popCrystalReport(Me, "SupplierInApprovedList.rpt", crParam)
        eProcure.Component.ReportViewer.popCrystalReport(Me, "report2.rpt")
        'CRviewer.popCrystalReport(Me, "SAMPLE.rpt", crParam)
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim crParam As New Hashtable
        'crParam.Add("buyerCompanyIDParam", "MBB")
        'crParam.Add("VendorCompanyIDParam", "All")
        crParam.Add("coy", TextBox1.Text)
        crParam.Add("re", TextBox2.Text)
        'crParam.Add("dateToParam", Now.Today)
        eProcure.Component.ReportViewer.popCrystalReport(Me, "report3.rpt", crParam)
        'CRviewer.popCrystalReport(Me, "SAMPLE.rpt", crParam)
    End Sub
End Class
