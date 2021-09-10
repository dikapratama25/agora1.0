
Imports AgoraLegacy

Public Class Index2
    Inherits System.Web.UI.Page
    Dim dDispatcher As New AgoraLegacy.dispatcher
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim UserDetails As User

        Dim objComp As New Companies
        Dim objUser As New Users

        ' ai chu modified on 17/11/2005
        ' session timeout set during login was missing since eProcurement and eProcure are different application
        Dim strSessionTimeOut As String
        Dim objLoginPolicy As New LoginPolicy
        strSessionTimeOut = Common.parseNull(objLoginPolicy.getParamValue("SESSION_TIMEOUT"), HttpContext.Current.Session.Timeout)
        objLoginPolicy = Nothing
        HttpContext.Current.Session.Timeout = CInt(strSessionTimeOut)
        UserDetails = objUser.GetUserDetails(Request.Cookies("aHideUserId").Value, Request.Cookies("aHideCompanyId").Value) 'Lookup the customer's full account details
        PreLogin(UserDetails, objComp)
        objLoginPolicy = Nothing
    End Sub

    Private Sub PreLogin(ByVal pUserDetails As User, ByVal pCoyDetails As Companies)
        Session("UserId") = Request.Cookies("aHideUserId").Value
        Session("UserName") = pUserDetails.Name
        Session("PageCount") = pUserDetails.PageCount
        Session("CompanyId") = Request.Cookies("aHideCompanyId").Value
        Session("CompanyName") = pCoyDetails.GetCompanyName(Trim(Request.Cookies("aHideCompanyId").Value))
        'Session("Last_Login") = Common.FormatWheelDate(WheelDateFormat.LongDateTimeDay, pUserDetails.LastLogin)
        Session("Last_Login") = Common.FormatWheelDate(WheelDateFormat.LongDateTimeDay, Request.Cookies("aLastLogin").Value)
        Session("Env") = System.Configuration.ConfigurationManager.AppSettings.Get("Env")
        Session("FileSize") = System.Configuration.ConfigurationManager.AppSettings.Get("FileSize")
        Session("AccessStatus") = Request.Cookies("aAccessStatus").Value
        Session("GracePeriod") = Request.Cookies("aGracePeriod").Value

        'Get Company Type from billing
        Dim objaccess As New CheckAccess
        Dim comptype As String
        comptype = objaccess.getCompanyType(Request.Cookies("aHideCompanyId").Value)
        Session("CompanyType") = comptype


        Session("WheelScript") = "<script type=""text/javascript"" src = """ & dDispatcher.direct("Plugins/Include", "WheelScript.js") & """></script>"
        Session("eRFPScript") = "<script type=""text/javascript"" src = """ & dDispatcher.direct("Plugins/Include", "eRFPScript.js") & """></script>"
        Session("PNGFix") = "<script defer type=""text/javascript"" src=""" & dDispatcher.direct("Plugins/Include", "PNGFix.js") & """></script>"
        Session("eRFPCDClock") = "<script type=""text/javascript"" src = """ & dDispatcher.direct("Plugins/Include", "eRFPCDClock.js") & """></script>"
        Session("AutoComplete") = "<script type=""text/javascript"" src= """ & dDispatcher.direct("Plugins/Include", "JqueryAutoComplete.js") & """></script>"
        Session("JQuery") = "<script type=""text/javascript"" src= """ & dDispatcher.direct("Plugins/Include", "JQuery.js") & """></script>"
        Session("BgiFrame") = "<script type=""text/javascript"" src= """ & dDispatcher.direct("Plugins/Include", "JqueryBgiFrame.min.js") & """></script>"
        Session("Block") = "<script type=""text/javascript"" src= """ & dDispatcher.direct("Plugins/Include", "jquery.blockUI.js") & """></script>"
    End Sub

End Class
