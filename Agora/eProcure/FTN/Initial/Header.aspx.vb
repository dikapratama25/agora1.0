
Imports eProcure.Component
Imports Microsoft.Web.UI.WebControls
Imports System
Imports System.Security.Cryptography
Imports System.Text
Imports AgoraLegacy
Public Class HeaderFTN1
    Inherits System.Web.UI.Page
    Protected WithEvents a As System.Web.UI.HtmlControls.HtmlAnchor
    Protected WithEvents lblHeaderLink As System.Web.UI.WebControls.Label
    Protected WithEvents lblWelcome As System.Web.UI.WebControls.Label
    Protected WithEvents lblLastlogin As System.Web.UI.WebControls.Label
    Protected WithEvents lblLogo As System.Web.UI.WebControls.Label
    Protected WithEvents Image1 As System.Web.UI.WebControls.Image
    Protected WithEvents lblstatus As System.Web.UI.WebControls.Label
    Protected WithEvents lnkstatus As System.Web.UI.WebControls.HyperLink
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
        'Dim UserDetails As User

        'Dim objComp As New Companies
        'Dim objUser As New Users
        'UserDetails = objUser.GetUserDetails(Request.Cookies("aHideUserId").Value, Request.Cookies("aHideCompanyId").Value) 'Lookup the customer's full account details
        'PreLogin(UserDetails, objComp)
        DislayLogo()
        genHeader()
        'lblHeaderLink.Visible = True
    End Sub

    Private Sub PreLogin(ByVal pUserDetails As User, ByVal pCoyDetails As Companies)
        'Session("UserId") = Request.Cookies("aHideUserId").Value
        'Session("UserName") = pUserDetails.Name
        'Session("CompanyId") = Request.Cookies("aHideCompanyId").Value
        'Session("CompanyName") = pCoyDetails.GetCompanyName(Trim(Request.Cookies("aHideCompanyId").Value))
        'Session("Last_Login") = Common.FormatWheelDate(WheelDateFormat.LongDateTimeDay, pUserDetails.LastLogin)
    End Sub
    Sub DislayLogo()
        Dim objFile As New FileManagement
        Dim strImgSrc As String
        strImgSrc = objFile.getCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyId"))
        If strImgSrc <> "" Then
            Image1.Visible = True
            Image1.ImageUrl = strImgSrc
            lblLogo.Visible = False
        Else
            Image1.Visible = False
            lblLogo.Visible = True
        End If
        objFile = Nothing
    End Sub

    Private Sub genHeader() 'optional header
        'Dim packageName() As String = {"eProcure", "eRFP", "eAuction", "eContract"}
        'Dim authPackageName() As String
        'Dim authPackageUrl() As String
        'Dim totalpackage As Integer
        'Dim j As Integer
        Dim strOutput As String = ""

        'authPackageName = Request.Cookies("aPackageName").Value.Split("|")
        'authPackageUrl = Request.Cookies("aPackageUrl").Value.Split("|")
        'totalpackage = Request.Cookies("aTotalPackage").Value

        'If totalpackage > 1 Then
        '    strOutput = "<table align='center' border='0' class='alltable'><tr width='100%'>"

        '    For j = 0 To authPackageName.Length - 1
        '        strOutput &= "<td align='center'><a target='_parent' href =" & authPackageUrl(j) & ">"
        '        strOutput &= "<IMG border=0 src='images\t_" & authPackageName.GetValue(j) & ".gif' "
        '        strOutput &= "alt='Click here to go to " & authPackageName.GetValue(j) & "' >"
        '        strOutput &= "</a></td>"
        '    Next

        '    strOutput &= "</tr></table>"
        'End If
        'lblHeaderLink.Text = strOutput
        Dim sHeadSideImg As String
        If Session("CompanyType") = "Buyer-Enterprise" Then
            'sHeadSideImg = "<a href=""http://www.strateq-bizhub.com"" target=""blank""><div style=""width:160;height:52;cursor:pointer;""><IMG src=""" & dDispatcher.direct("Plugins/Images", "Agora_Header.png") & """ height=""52"" width=""160"" style=""margin:0px;""></div></a>"
            sHeadSideImg = "<div style=""width:160;height:52;""><IMG src=""" & dDispatcher.direct("Plugins/Images", "Agora_Header.png") & """ height=""52"" width=""160"" style=""margin:0px;""></div>"

        ElseIf Session("CompanyType") = "Vendor" Then
            'If Session("Env") = "FTN" Then
            '    sHeadSideImg = "<a href=""http://www.myfairtradenet.com"" target=""blank""><div style=""width:160;height:52;cursor:pointer;""><IMG src=""" & dDispatcher.direct("Plugins/Images", "MyFairTradeNet_header.png") & """ height=""52"" width=""160"" style=""margin:0px;""></div></a>"
            'Else
            '    sHeadSideImg = "<a href=""http://www.strateq-bizhub.com"" target=""blank""><div style=""width:160;height:52;cursor:pointer;""><IMG src=""" & dDispatcher.direct("Plugins/Images", "Strateq_Header.png") & """ height=""52"" width=""160"" style=""margin:0px;""></div></a>"
            'End If
            sHeadSideImg = "<a href=""http://www.myfairtradenet.com"" target=""blank""><div style=""width:160;height:52;cursor:pointer;""><IMG src=""" & dDispatcher.direct("Plugins/Images", "Agora_Header.png") & """ height=""52"" width=""160"" style=""margin:0px;""></div></a>"
        Else
            'sHeadSideImg = "<a href=""http://www.myfairtradenet.com"" target=""blank""><div style=""width:160;height:52;cursor:pointer;""><IMG src=""" & dDispatcher.direct("Plugins/Images", "MyFairTradeNet_header.png") & """ height=""52"" width=""160"" style=""margin:0px;""></div></a>"
            sHeadSideImg = "<div style=""width:160;height:52;""><IMG src=""" & dDispatcher.direct("Plugins/Images", "Agora_Header.png") & """ height=""52"" width=""160"" style=""margin:0px;""></div>"
        End If
        Session("sHeadSideImg") = sHeadSideImg
        If Not Page.IsPostBack Then
            lblWelcome.Text = "Welcome, " & Session("UserName") & " from " & Session("CompanyName")
            lblLastlogin.Text = "Last Log On: " & Session("Last_Login")
            'Michelle (26/3/2013) - Issue 1876
            If Session("AccessStatus") = "Limited" Or Session("GracePeriod") = "True" Then
                GenerateLinkToBilling()
            End If
        End If



    End Sub
    Private Sub GenerateLinkToBilling()
        Dim user As String
        user = UCase(Session("UserID"))
        user &= Replace(Date.Today, "/", "")
        user = getMd5Hash(user)
        Dim url As String = System.Configuration.ConfigurationManager.AppSettings("BillingURL") & "/?user=" & user & "&uid=" & Session("UserID") & "&cid=" & Session("CompanyID")
        lnkstatus.NavigateUrl = url
        If Session("AccessStatus") = "Limited" Then
            lnkstatus.Text = "(Limited Access)"
        Else
            'Michelle (26/3/2013) - Issue 1876
            lnkstatus.Text = "(Grace Period)"
        End If
    End Sub
    Function getMd5Hash(ByVal input As String) As String
        ' Create a new instance of the MD5 object.
        Dim md5Hasher As MD5 = MD5.Create()
        ' Convert the input string to a byte array and compute the hash.
        Dim data As Byte() = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input))
        ' Create a new Stringbuilder to collect the byte
        ' and create a string.
        Dim sBuilder As New StringBuilder()
        ' Loop through each byte of the hashed data
        ' and format each one as a hexadecimal string.
        Dim i As Integer
        For i = 0 To data.Length - 1
            sBuilder.Append(data(i).ToString("x2"))
        Next i
        ' Return the hexadecimal string.
        Return sBuilder.ToString()
    End Function
End Class
