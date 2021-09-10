Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient

Public Class ReportFormat14
    Inherits AgoraLegacy.AppBaseClass
    Dim dispatcher As New dispatcher

    'Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    Dim ii_ddl, ii_ddl2, jj_ddl As Integer
    '    Dim lstItem As New ListItem
    '    Dim strCoyType As String
    '    Dim startDate As Date = DateAdd(DateInterval.Month, -17, DateTime.Now)
    '    Dim strdate As String
    '    Dim yr As Integer

    '    lblHeader.Text = Request.QueryString("rptname")
    '    strCoyType = lblHeader.Text.Substring(1, 1)
    '    If strCoyType = "V" Then
    '        ViewState("ReportType") = "Vendor"
    '    Else
    '        ViewState("ReportType") = "Buyer"
    '        cmbYearFrom.AutoPostBack = False
    '        cmbYearTo.AutoPostBack = False
    '        cmbMonthFrom.AutoPostBack = False
    '        cmbMonthTo.AutoPostBack = False
    '    End If

    '    ii_ddl2 = 1
    '    jj_ddl = Year(Date.Now)

    '    cmbMonthFrom.Items.Clear()
    '    cmbMonthTo.Items.Clear()
    '    lstItem.Value = ""
    '    lstItem.Text = "---Select---"
    '    cmbMonthFrom.Items.Insert(0, lstItem)
    '    cmbMonthTo.Items.Insert(0, lstItem)
    '    If Not Page.IsPostBack Then
    '        cmbYearFrom.Items.Insert(0, lstItem)
    '        cmbYearTo.Items.Insert(0, lstItem)

    '        ii_ddl2 = 1
    '        jj_ddl = Year(Date.Now)
    '        If ViewState("ReportType") = "Buyer" Then
    '            yr = 2002
    '        Else
    '            yr = startDate.Year
    '        End If
    '        For ii_ddl = yr To jj_ddl
    '            cmbYearFrom.Items.Insert(ii_ddl2, New ListItem(ii_ddl))
    '            cmbYearTo.Items.Insert(ii_ddl2, New ListItem(ii_ddl))
    '            ii_ddl2 = ii_ddl2 + 1
    '        Next

    '        ii_ddl = 1
    '        jj_ddl = 12
    '        For ii_ddl = 1 To jj_ddl
    '            Dim lst As New ListItem
    '            strdate = "01/" & ii_ddl & "/2005"
    '            lst.Value = ii_ddl
    '            lst.Text = Format(CDate(strdate), "MMMM")
    '            cmbMonthFrom.Items.Insert(ii_ddl, lst)
    '            cmbMonthTo.Items.Insert(ii_ddl, lst)
    '        Next
    '    End If

    '    If Page.IsPostBack Then
    '        If ViewState("ReportType") = "Vendor" Then
    '            Dim mthFrom As Integer = 1
    '            Dim mthTo As Integer = 12
    '            Dim count As Integer
    '            Dim kk_ddl As Integer = 12
    '            jj_ddl = 12

    '            If Not cmbYearFrom.SelectedValue = "" Then
    '                If startDate.Year = cmbYearFrom.SelectedValue Then
    '                    mthFrom = startDate.Month
    '                    mthTo = startDate.Month
    '                    jj_ddl = 12
    '                    kk_ddl = 12
    '                ElseIf Not startDate.Year = cmbYearFrom.SelectedValue Then
    '                    mthFrom = 1
    '                    mthTo = 1
    '                    jj_ddl = DateTime.Now.Month
    '                    kk_ddl = DateTime.Now.Month
    '                End If
    '            End If
    '            If Not cmbYearTo.SelectedValue = "" Then
    '                If startDate.Year = cmbYearTo.SelectedValue Then
    '                    mthFrom = startDate.Month
    '                    mthTo = startDate.Month
    '                    jj_ddl = 12
    '                    kk_ddl = 12
    '                ElseIf Not startDate.Year = cmbYearTo.SelectedValue Then
    '                    mthFrom = 1
    '                    mthTo = 1
    '                    jj_ddl = DateTime.Now.Month
    '                    kk_ddl = DateTime.Now.Month
    '                End If
    '            End If
    '            If Not cmbYearFrom.SelectedValue = "" And Not cmbYearTo.SelectedValue = "" Then
    '                If cmbYearFrom.SelectedValue = cmbYearTo.SelectedValue And startDate.Year = cmbYearFrom.SelectedValue Then
    '                    mthFrom = startDate.Month
    '                    mthTo = startDate.Month
    '                    jj_ddl = 12
    '                    kk_ddl = 12
    '                ElseIf cmbYearFrom.SelectedValue = cmbYearTo.SelectedValue And Not startDate.Year = cmbYearFrom.SelectedValue Then
    '                    mthFrom = 1
    '                    mthTo = 1
    '                    jj_ddl = DateTime.Now.Month
    '                    kk_ddl = DateTime.Now.Month
    '                ElseIf Not cmbYearFrom.SelectedValue = cmbYearTo.SelectedValue Then
    '                    mthFrom = startDate.Month
    '                    mthTo = 1
    '                    jj_ddl = 12
    '                    kk_ddl = DateTime.Now.Month
    '                End If
    '            End If

    '            For ii_ddl = mthFrom To jj_ddl
    '                count = cmbMonthFrom.Items.Count
    '                Dim lst As New ListItem
    '                strdate = "01/" & ii_ddl & "/" & startDate.Year
    '                lst.Value = ii_ddl
    '                lst.Text = Format(CDate(strdate), "MMMM")
    '                cmbMonthFrom.Items.Insert(count, lst)
    '            Next
    '            For ii_ddl = mthTo To kk_ddl
    '                count = cmbMonthTo.Items.Count
    '                Dim lst As New ListItem
    '                strdate = "01/" & ii_ddl & "/" & startDate.Year
    '                lst.Value = ii_ddl
    '                lst.Text = Format(CDate(strdate), "MMMM")
    '                cmbMonthTo.Items.Insert(count, lst)
    '            Next
    '            'Else 'For Buyer
    '            '    cmbYearFrom.Items.Clear()
    '            '    cmbYearTo.Items.Clear()
    '            '    lstItem.Value = ""
    '            '    lstItem.Text = "---Select---"
    '            '    cmbYearFrom.Items.Insert(0, lstItem)
    '            '    cmbYearTo.Items.Insert(0, lstItem)
    '            '    cmbMonthFrom.Items.Insert(0, lstItem)
    '            '    cmbMonthTo.Items.Insert(0, lstItem)

    '            '    ii_ddl2 = 1
    '            '    jj_ddl = Year(Date.Now)
    '            '    For ii_ddl = 2002 To jj_ddl
    '            '        cmbYearFrom.Items.Insert(ii_ddl2, New ListItem(ii_ddl))
    '            '        cmbYearTo.Items.Insert(ii_ddl2, New ListItem(ii_ddl))
    '            '        ii_ddl2 = ii_ddl2 + 1
    '            '    Next

    '            '    ii_ddl = 1
    '            '    jj_ddl = 12
    '            '    For ii_ddl = 1 To jj_ddl
    '            '        Dim lst As New ListItem
    '            '        strdate = "01/" & ii_ddl & "/2005"
    '            '        lst.Value = ii_ddl
    '            '        lst.Text = Format(CDate(strdate), "MMMM")
    '            '        cmbMonthFrom.Items.Insert(ii_ddl, lst)
    '            '        cmbMonthTo.Items.Insert(ii_ddl, lst)
    '            '    Next
    '        End If
    '    End If
    '    lnkBack.NavigateUrl = dispatcher.direct("Report", "ReportSelection.aspx", "pageid=" & strPageId & "&type=" & Request.QueryString("type"))
    '    cmdSubmit.Attributes.Add("onclick", "return compareDates();")
    'End Sub

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not (Page.IsPostBack) Then
            lblHeader.Text = Request.QueryString("rptname")
        End If

        lnkBack.NavigateUrl = "ReportSelection.aspx?pageid=" & strPageId 'dispatcher.direct("Report", "ReportSelection.aspx", "pageid=" & strPageId & "&type=" & Request.QueryString("type"))
    End Sub

    Private Sub ExportToPDF()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        'Dim objFile As New FileManagement
        Dim strImgSrc As String
        Dim strUserId As String
        Dim strCoyName As String
        Dim strFileName As String = ""
        Dim objFile As New FileManagement
        Dim strTemp As String
        Dim strsql As String = ""
        Dim strBuyerID As String = txtBuyerID.Text
        Dim strBuyerName As String = txtBuyerName.Text
        Dim strSKID As String = txtSKID.Text
        Dim strSKName As String = txtSKName.Text

        'strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyIdToken"), System.AppDomain.CurrentDomain.BaseDirectory & "Common\Plugins\images\", System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))
        'strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        Try
            If strBuyerID <> "" Then
                strTemp = Common.BuildWildCard(strBuyerID)
                strsql = strsql & " AND ua_b.ua_user_id " & Common.ParseSQL(strTemp)
            End If

            If strBuyerName <> "" Then
                strTemp = Common.BuildWildCard(strBuyerName)
                strsql = strsql & " AND usr.um_user_name " & Common.ParseSQL(strTemp)
            End If

            If strSKID <> "" Then
                strTemp = Common.BuildWildCard(strSKID)
                strsql = strsql & " AND ua_sk.ul_user_id " & Common.ParseSQL(strTemp)
            End If

            If strSKName <> "" Then
                strTemp = Common.BuildWildCard(strSKName)
                strsql = strsql & " AND usr2.um_user_name " & Common.ParseSQL(strTemp)
            End If

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT Buyer_ID, Buyer_Name, IFNULL(Group_Type, '') AS Approval_Group_Type, IFNULL(Approval_Group_Name, '') AS Approval_Group_Name, Storekeeper_ID, Storekeeper_Name, " &
                            "Delivery_Code, Address_Line_1, Address_Line_2, Address_Line_3, Postcode, City, State, Country FROM " &
                            "(SELECT ua_b.ua_user_id AS 'Buyer_ID', usr.um_user_name AS 'Buyer_Name', tb_ap.agm_type AS 'Group_Type', REPLACE(tb_ap.agm_grp_name,',','zzz') AS 'Approval_Group_Name', " &
                            "ua_sk.ul_user_id AS 'Storekeeper_ID', usr2.um_user_name AS 'Storekeeper_Name', " &
                            "ua_sk.ul_addr_code AS 'Delivery_Code', addr_m.am_addr_line1 AS 'Address_Line_1', addr_m.am_addr_line2 AS 'Address_Line_2', " &
                            "addr_m.am_addr_line3 AS 'Address_Line_3', addr_m.am_postcode AS 'Postcode', addr_m.am_city AS 'City', " &
                            "code_s.code_desc AS 'State', code_c.code_desc AS 'Country' FROM users_addr AS ua_b " &
                            "INNER JOIN users_usrgrp AS  usr_grp1 ON (usr_grp1.uu_coy_id = '" & Session("CompanyID") & "' AND usr_grp1.uu_user_id = ua_b.ua_user_id) " &
                            "AND usr_grp1.uu_usrgrp_id IN ('buyerCtr','buyer','buyerxRFQ','buyerS','pm','po') " &
                            "LEFT JOIN (SELECT * FROM approval_grp_buyer AS ag_b INNER JOIN approval_grp_mstr AS ag_m " &
                            "ON ag_m.agm_grp_index = ag_b.agb_grp_index) AS tb_ap ON ua_b.ua_user_id = tb_ap.agb_buyer, " &
                            "users_location AS ua_sk, user_mstr AS usr, user_mstr AS usr2, address_mstr AS addr_m, code_mstr AS code_c, code_mstr AS code_s " &
                            "WHERE (usr.um_deleted <> 'Y' AND usr2.um_deleted <> 'Y' AND ua_b.ua_coy_id = '" & Session("CompanyID") & "' " & strsql & ") " &
                            "AND (tb_ap.agm_coy_id IS NULL OR tb_ap.agm_coy_id = '" & Session("CompanyID") & "') " &
                            "AND usr.um_user_id = ua_b.ua_user_id AND usr.um_coy_id = ua_b.ua_coy_id AND ua_b.ua_user_id = ua_b.ua_user_id " &
                            "AND ua_b.ua_addr_type = 'D' AND ua_b.ua_addr_code = ua_sk.ul_addr_code AND ua_b.ua_coy_id = ua_sk.ul_coy_id " &
                            "AND usr2.um_user_id = ua_sk.ul_user_id AND usr2.um_coy_id = ua_sk.ul_coy_id AND addr_m.am_addr_code = ua_sk.ul_addr_code " &
                            "AND addr_m.am_coy_id = ua_sk.ul_coy_id AND code_c.code_abbr = addr_m.am_country AND (code_s.code_abbr = addr_m.am_state " &
                            "AND code_s.code_value = addr_m.am_country) " &
                            "UNION " &
                            "SELECT ua_b.ua_user_id AS 'Buyer_ID', usr.um_user_name AS 'Buyer_Name', tb_ap.agm_type AS 'Group_Type', REPLACE(tb_ap.agm_grp_name,',','zzz') AS 'Approval_Group_Name', " &
                            "ua_sk.ul_user_id AS 'Storekeeper_ID', usr2.um_user_name AS 'Storekeeper_Name', " &
                            "ua_sk.ul_addr_code AS 'Delivery_Code', addr_m.am_addr_line1 AS 'Address_Line_1', addr_m.am_addr_line2 AS 'Address_Line_2', " &
                            "addr_m.am_addr_line3 AS 'Address_Line_3', addr_m.am_postcode AS 'Postcode', addr_m.am_city AS 'City', " &
                            "code_s.code_desc AS 'State', code_c.code_desc AS 'Country' FROM users_addr AS ua_b " &
                            "INNER JOIN users_usrgrp AS  usr_grp1 ON (usr_grp1.uu_coy_id = '" & Session("CompanyID") & "' AND usr_grp1.uu_user_id = ua_b.ua_user_id) " &
                            "AND usr_grp1.uu_usrgrp_id IN ('buyerCtr','buyer','buyerxRFQ','buyerS','pm','po') " &
                            "LEFT JOIN (SELECT * FROM approval_grp_buyer AS ag_b INNER JOIN approval_grp_mstr AS ag_m " &
                            "ON ag_m.agm_grp_index = ag_b.agb_grp_index) AS tb_ap ON ua_b.ua_user_id = tb_ap.agb_buyer , " &
                            "users_location AS ua_sk, user_mstr AS usr, user_mstr AS usr2, address_mstr AS addr_m, code_mstr AS code_c, code_mstr AS code_s " &
                            "WHERE (usr.um_deleted <> 'Y' AND usr2.um_deleted <> 'Y' AND ua_b.ua_coy_id = '" & Session("CompanyID") & "' " & strsql & ") " &
                            "AND (tb_ap.agm_coy_id  IS NULL OR tb_ap.agm_coy_id = '" & Session("CompanyID") & "') " &
                            "AND usr.um_user_id = ua_b.ua_user_id AND usr.um_coy_id = ua_b.ua_coy_id AND ua_b.ua_user_id = ua_b.ua_user_id " &
                            "AND ua_b.ua_addr_code = '0' AND ua_b.ua_addr_type = 'D' AND ua_b.ua_coy_id = ua_sk.ul_coy_id AND usr2.um_user_id = ua_sk.ul_user_id " &
                            "AND usr2.um_coy_id = ua_sk.ul_coy_id AND addr_m.am_addr_code = ua_sk.ul_addr_code AND addr_m.am_coy_id = ua_sk.ul_coy_id " &
                            "AND code_c.code_abbr = addr_m.am_country AND (code_s.code_abbr = addr_m.am_state " &
                            "AND code_s.code_value = addr_m.am_country)) AS zzz, users_usrgrp AS usr_grp2, user_group_mstr AS usr_gpm " &
                            "WHERE (usr_grp2.uu_coy_id = '" & Session("CompanyID") & "' AND usr_grp2.uu_user_id = Storekeeper_ID AND usr_grp2.uu_usrgrp_id = usr_gpm.ugm_usrgrp_id " &
                            "AND usr_gpm.ugm_fixed_role = 'Store Keeper') " &
                            "ORDER BY Buyer_ID, Approval_Group_Name, Storekeeper_ID, Delivery_Code "


                '.CommandText = "SELECT Buyer_ID, Buyer_Name, IFNULL(Group_Type, '') AS Approval_Group_Type, IFNULL(Approval_Group_Name, '') AS Approval_Group_Name, Storekeeper_ID, Storekeeper_Name, " & _
                '            "Delivery_Code, Address_Line_1, Address_Line_2, Address_Line_3, Postcode, City, State, Country FROM " & _
                '            "(SELECT ua_b.ua_user_id AS 'Buyer_ID', usr.um_user_name AS 'Buyer_Name', tb_ap.agm_type AS 'Group_Type', REPLACE(tb_ap.agm_grp_name,',','zzz') AS 'Approval_Group_Name', " & _
                '            "ua_sk.ul_user_id AS 'Storekeeper_ID', usr2.um_user_name AS 'Storekeeper_Name', " & _
                '            "ua_sk.ul_addr_code AS 'Delivery_Code', addr_m.am_addr_line1 AS 'Address_Line_1', addr_m.am_addr_line2 AS 'Address_Line_2', " & _
                '            "addr_m.am_addr_line3 AS 'Address_Line_3', addr_m.am_postcode AS 'Postcode', addr_m.am_city AS 'City', " & _
                '            "code_s.code_desc AS 'State', code_c.code_desc AS 'Country' FROM users_addr AS ua_b " & _
                '            "LEFT JOIN (SELECT * FROM approval_grp_buyer AS ag_b INNER JOIN approval_grp_mstr AS ag_m " & _
                '            "ON ag_m.agm_grp_index = ag_b.agb_grp_index) AS tb_ap ON ua_b.ua_user_id = tb_ap.agb_buyer , " & _
                '            "users_location AS ua_sk, user_mstr AS usr, user_mstr AS usr2, address_mstr AS addr_m, code_mstr AS code_c, code_mstr AS code_s " & _
                '            "WHERE (usr.um_deleted <> 'Y' AND usr2.um_deleted <> 'Y' AND ua_b.ua_coy_id = '" & Session("CompanyID") & "' " & strsql & ") " & _
                '            "AND (tb_ap.agm_coy_id IS NULL OR tb_ap.agm_coy_id = '" & Session("CompanyID") & "') " & _
                '            "AND usr.um_user_id = ua_b.ua_user_id AND usr.um_coy_id = ua_b.ua_coy_id AND ua_b.ua_user_id = ua_b.ua_user_id " & _
                '            "AND ua_b.ua_addr_type = 'D' AND ua_b.ua_addr_code = ua_sk.ul_addr_code AND ua_b.ua_coy_id = ua_sk.ul_coy_id " & _
                '            "AND usr2.um_user_id = ua_sk.ul_user_id AND usr2.um_coy_id = ua_sk.ul_coy_id AND addr_m.am_addr_code = ua_sk.ul_addr_code " & _
                '            "AND addr_m.am_coy_id = ua_sk.ul_coy_id AND code_c.code_abbr = addr_m.am_country AND (code_s.code_abbr = addr_m.am_state " & _
                '            "AND code_s.code_value = addr_m.am_country) " & _
                '            "UNION " & _
                '            "SELECT ua_b.ua_user_id AS 'Buyer_ID', usr.um_user_name AS 'Buyer_Name', tb_ap.agm_type AS 'Group_Type', REPLACE(tb_ap.agm_grp_name,',','zzz') AS 'Approval_Group_Name'," & _
                '            "ua_sk.ul_user_id AS 'Storekeeper_ID', usr2.um_user_name AS 'Storekeeper_Name', " & _
                '            "ua_sk.ul_addr_code AS 'Delivery_Code', addr_m.am_addr_line1 AS 'Address_Line_1', addr_m.am_addr_line2 AS 'Address_Line_2', " & _
                '            "addr_m.am_addr_line3 AS 'Address_Line_3', addr_m.am_postcode AS 'Postcode', addr_m.am_city AS 'City', " & _
                '            "code_s.code_desc AS 'State', code_c.code_desc AS 'Country' FROM users_addr AS ua_b " & _
                '            "LEFT JOIN (SELECT * FROM approval_grp_buyer AS ag_b INNER JOIN approval_grp_mstr AS ag_m " & _
                '            "ON ag_m.agm_grp_index = ag_b.agb_grp_index) AS tb_ap ON ua_b.ua_user_id = tb_ap.agb_buyer , " & _
                '            "users_location AS ua_sk, user_mstr AS usr, user_mstr AS usr2, address_mstr AS addr_m, code_mstr AS code_c, code_mstr AS code_s " & _
                '            "WHERE (usr.um_deleted <> 'Y' AND usr2.um_deleted <> 'Y' AND ua_b.ua_coy_id = '" & Session("CompanyID") & "' " & strsql & ") " & _
                '            "AND (tb_ap.agm_coy_id  IS NULL OR tb_ap.agm_coy_id = '" & Session("CompanyID") & "') " & _
                '            "AND usr.um_user_id = ua_b.ua_user_id AND usr.um_coy_id = ua_b.ua_coy_id AND ua_b.ua_user_id = ua_b.ua_user_id " & _
                '            "AND ua_b.ua_addr_code = '0' AND ua_b.ua_addr_type = 'D' AND ua_b.ua_coy_id = ua_sk.ul_coy_id AND usr2.um_user_id = ua_sk.ul_user_id " & _
                '            "AND usr2.um_coy_id = ua_sk.ul_coy_id AND addr_m.am_addr_code = ua_sk.ul_addr_code AND addr_m.am_coy_id = ua_sk.ul_coy_id " & _
                '            "AND code_c.code_abbr = addr_m.am_country AND (code_s.code_abbr = addr_m.am_state " & _
                '            "AND code_s.code_value = addr_m.am_country)) AS zzz, users_usrgrp AS usr_grp1, user_group_mstr AS usr_gpm1, users_usrgrp AS usr_grp2, user_group_mstr AS usr_gpm2 " & _
                '            "WHERE (usr_grp1.uu_coy_id = '" & Session("CompanyID") & "' AND usr_grp1.uu_user_id = Buyer_ID AND usr_grp1.uu_usrgrp_id = usr_gpm1.ugm_usrgrp_id AND " & _
                '            "(usr_gpm1.ugm_fixed_role = 'Buyer' OR usr_gpm1.ugm_fixed_role = 'Purchasing Manager' OR usr_gpm1.ugm_fixed_role = 'Purchasing Officer')) " & _
                '            "AND (usr_grp2.uu_coy_id = '" & Session("CompanyID") & "' AND usr_grp2.uu_user_id = Storekeeper_ID AND usr_grp2.uu_usrgrp_id = usr_gpm2.ugm_usrgrp_id AND " & _
                '            "usr_gpm2.ugm_fixed_role = 'Store Keeper') " & _
                '            "ORDER BY Buyer_ID, Approval_Group_Name, Storekeeper_ID, Delivery_Code "

            End With

            da = New MySqlDataAdapter(cmd)
            strUserId = Session("UserName")
            strCoyName = Session("CompanyName")
            da.SelectCommand.CommandTimeout = 10000
            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("DeliveryAddressList_DataTable1", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            localreport.ReportPath = dispatcher.direct("Report", "DeliveryAddrList.rdlc", "Report") ' Server.MapPath("PODetails_pdf.rdlc")  'appPath & "Report\PODetails_pdf.rdlc"
            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            Dim strID As String = ""

            strID = System.Configuration.ConfigurationManager.AppSettings.Get("Env") & "ReportCoyLogoPath"
            TotalParameter = localreport.GetParameters.Count
            Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
            'Dim paramlist As New Generic.List(Of ReportParameter)
            For I = 0 To localreport.GetParameters.Count - 1
                GetParameter = localreport.GetParameters.Item(I).Name
                Select Case LCase(GetParameter)
                    Case "pmrequestedby"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strUserId)

                        'Case "logo"
                        '    par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings(strID) & strImgSrc)

                    Case "prmbuyercoyname"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strCoyName)
                    Case Else
                End Select
            Next
            localreport.SetParameters(par)
            localreport.Refresh()

            Dim deviceInfo As String =
                "<DeviceInfo>" +
                    "  <OutputFormat>EMF</OutputFormat>" +
                    "</DeviceInfo>"
            'System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            strFileName = "BuyerDeliveryAddressList.pdf"
            'Return PDF
            Me.Response.Clear()
            Me.Response.ContentType = "application/pdf"
            Me.Response.AddHeader("Content-disposition", "attachment; filename=" & strFileName)
            Me.Response.BinaryWrite(PDF)
            Me.Response.End()
            'Dim fs As New FileStream(appPath & "Report\" & strFileName, FileMode.Create)
            'fs.Write(PDF, 0, PDF.Length)
            'fs.Close()

            'Response.ContentType = "application/x-download"
            'Response.AddHeader("Content-Disposition", "attachment;filename=" & strFileName)
            'Response.WriteFile(Server.MapPath(strFileName))
            'Response.End()

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

    Private Sub ExportToExcel()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim dtFrom As Date
        Dim strEnd As String = ""
        Dim strFileName As String = ""
        Dim strTemp As String
        Dim strsql As String = ""
        Dim strBuyerID As String = txtBuyerID.Text
        Dim strBuyerName As String = txtBuyerName.Text
        Dim strSKID As String = txtSKID.Text
        Dim strSKName As String = txtSKName.Text
        Try
            If strBuyerID <> "" Then
                strTemp = Common.BuildWildCard(strBuyerID)
                strsql = strsql & " AND ua_b.ua_user_id " & Common.ParseSQL(strTemp)
            End If

            If strBuyerName <> "" Then
                strTemp = Common.BuildWildCard(strBuyerName)
                strsql = strsql & " AND usr.um_user_name " & Common.ParseSQL(strTemp)
            End If

            If strSKID <> "" Then
                strTemp = Common.BuildWildCard(strSKID)
                strsql = strsql & " AND ua_sk.ul_user_id " & Common.ParseSQL(strTemp)
            End If

            If strSKName <> "" Then
                strTemp = Common.BuildWildCard(strSKName)
                strsql = strsql & " AND usr2.um_user_name " & Common.ParseSQL(strTemp)
            End If

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"
            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT Buyer_ID, Buyer_Name, IFNULL(Group_Type, '') AS Approval_Group_Type, IFNULL(Approval_Group_Name, '') AS Approval_Group_Name, Storekeeper_ID, Storekeeper_Name, " & _
                            "Delivery_Code, Address_Line_1, Address_Line_2, Address_Line_3, Postcode, City, State, Country FROM " & _
                            "(SELECT ua_b.ua_user_id AS 'Buyer_ID', usr.um_user_name AS 'Buyer_Name', tb_ap.agm_type AS 'Group_Type', REPLACE(tb_ap.agm_grp_name,',','zzz') AS 'Approval_Group_Name', " & _
                            "ua_sk.ul_user_id AS 'Storekeeper_ID', usr2.um_user_name AS 'Storekeeper_Name', " & _
                            "ua_sk.ul_addr_code AS 'Delivery_Code', addr_m.am_addr_line1 AS 'Address_Line_1', addr_m.am_addr_line2 AS 'Address_Line_2', " & _
                            "addr_m.am_addr_line3 AS 'Address_Line_3', addr_m.am_postcode AS 'Postcode', addr_m.am_city AS 'City', " & _
                            "code_s.code_desc AS 'State', code_c.code_desc AS 'Country' FROM users_addr AS ua_b " & _
                            "INNER JOIN users_usrgrp AS  usr_grp1 ON (usr_grp1.uu_coy_id = '" & Session("CompanyID") & "' AND usr_grp1.uu_user_id = ua_b.ua_user_id) " & _
                            "AND usr_grp1.uu_usrgrp_id IN ('buyerCtr','buyer','buyerxRFQ','buyerS','pm','po') " & _
                            "LEFT JOIN (SELECT * FROM approval_grp_buyer AS ag_b INNER JOIN approval_grp_mstr AS ag_m " & _
                            "ON ag_m.agm_grp_index = ag_b.agb_grp_index) AS tb_ap ON ua_b.ua_user_id = tb_ap.agb_buyer, " & _
                            "users_location AS ua_sk, user_mstr AS usr, user_mstr AS usr2, address_mstr AS addr_m, code_mstr AS code_c, code_mstr AS code_s " & _
                            "WHERE (usr.um_deleted <> 'Y' AND usr2.um_deleted <> 'Y' AND ua_b.ua_coy_id = '" & Session("CompanyID") & "' " & strsql & ") " & _
                            "AND (tb_ap.agm_coy_id IS NULL OR tb_ap.agm_coy_id = '" & Session("CompanyID") & "') " & _
                            "AND usr.um_user_id = ua_b.ua_user_id AND usr.um_coy_id = ua_b.ua_coy_id AND ua_b.ua_user_id = ua_b.ua_user_id " & _
                            "AND ua_b.ua_addr_type = 'D' AND ua_b.ua_addr_code = ua_sk.ul_addr_code AND ua_b.ua_coy_id = ua_sk.ul_coy_id " & _
                            "AND usr2.um_user_id = ua_sk.ul_user_id AND usr2.um_coy_id = ua_sk.ul_coy_id AND addr_m.am_addr_code = ua_sk.ul_addr_code " & _
                            "AND addr_m.am_coy_id = ua_sk.ul_coy_id AND code_c.code_abbr = addr_m.am_country AND (code_s.code_abbr = addr_m.am_state " & _
                            "AND code_s.code_value = addr_m.am_country) " & _
                            "UNION " & _
                            "SELECT ua_b.ua_user_id AS 'Buyer_ID', usr.um_user_name AS 'Buyer_Name', tb_ap.agm_type AS 'Group_Type', REPLACE(tb_ap.agm_grp_name,',','zzz') AS 'Approval_Group_Name', " & _
                            "ua_sk.ul_user_id AS 'Storekeeper_ID', usr2.um_user_name AS 'Storekeeper_Name', " & _
                            "ua_sk.ul_addr_code AS 'Delivery_Code', addr_m.am_addr_line1 AS 'Address_Line_1', addr_m.am_addr_line2 AS 'Address_Line_2', " & _
                            "addr_m.am_addr_line3 AS 'Address_Line_3', addr_m.am_postcode AS 'Postcode', addr_m.am_city AS 'City', " & _
                            "code_s.code_desc AS 'State', code_c.code_desc AS 'Country' FROM users_addr AS ua_b " & _
                            "INNER JOIN users_usrgrp AS  usr_grp1 ON (usr_grp1.uu_coy_id = '" & Session("CompanyID") & "' AND usr_grp1.uu_user_id = ua_b.ua_user_id) " & _
                            "AND usr_grp1.uu_usrgrp_id IN ('buyerCtr','buyer','buyerxRFQ','buyerS','pm','po') " & _
                            "LEFT JOIN (SELECT * FROM approval_grp_buyer AS ag_b INNER JOIN approval_grp_mstr AS ag_m " & _
                            "ON ag_m.agm_grp_index = ag_b.agb_grp_index) AS tb_ap ON ua_b.ua_user_id = tb_ap.agb_buyer , " & _
                            "users_location AS ua_sk, user_mstr AS usr, user_mstr AS usr2, address_mstr AS addr_m, code_mstr AS code_c, code_mstr AS code_s " & _
                            "WHERE (usr.um_deleted <> 'Y' AND usr2.um_deleted <> 'Y' AND ua_b.ua_coy_id = '" & Session("CompanyID") & "' " & strsql & ") " & _
                            "AND (tb_ap.agm_coy_id  IS NULL OR tb_ap.agm_coy_id = '" & Session("CompanyID") & "') " & _
                            "AND usr.um_user_id = ua_b.ua_user_id AND usr.um_coy_id = ua_b.ua_coy_id AND ua_b.ua_user_id = ua_b.ua_user_id " & _
                            "AND ua_b.ua_addr_code = '0' AND ua_b.ua_addr_type = 'D' AND ua_b.ua_coy_id = ua_sk.ul_coy_id AND usr2.um_user_id = ua_sk.ul_user_id " & _
                            "AND usr2.um_coy_id = ua_sk.ul_coy_id AND addr_m.am_addr_code = ua_sk.ul_addr_code AND addr_m.am_coy_id = ua_sk.ul_coy_id " & _
                            "AND code_c.code_abbr = addr_m.am_country AND (code_s.code_abbr = addr_m.am_state " & _
                            "AND code_s.code_value = addr_m.am_country)) AS zzz, users_usrgrp AS usr_grp2, user_group_mstr AS usr_gpm " & _
                            "WHERE (usr_grp2.uu_coy_id = '" & Session("CompanyID") & "' AND usr_grp2.uu_user_id = Storekeeper_ID AND usr_grp2.uu_usrgrp_id = usr_gpm.ugm_usrgrp_id " & _
                            "AND usr_gpm.ugm_fixed_role = 'Store Keeper') " & _
                            "ORDER BY Buyer_ID, Approval_Group_Name, Storekeeper_ID, Delivery_Code "


                '.CommandText = "SELECT Buyer_ID, Buyer_Name, IFNULL(Group_Type, '') AS Approval_Group_Type, IFNULL(Approval_Group_Name, '') AS Approval_Group_Name, Storekeeper_ID, Storekeeper_Name, " & _
                '            "Delivery_Code, Address_Line_1, Address_Line_2, Address_Line_3, Postcode, City, State, Country FROM " & _
                '            "(SELECT ua_b.ua_user_id AS 'Buyer_ID', usr.um_user_name AS 'Buyer_Name', tb_ap.agm_type AS 'Group_Type', REPLACE(tb_ap.agm_grp_name,',','zzz') AS 'Approval_Group_Name', " & _
                '            "ua_sk.ul_user_id AS 'Storekeeper_ID', usr2.um_user_name AS 'Storekeeper_Name', " & _
                '            "ua_sk.ul_addr_code AS 'Delivery_Code', addr_m.am_addr_line1 AS 'Address_Line_1', addr_m.am_addr_line2 AS 'Address_Line_2', " & _
                '            "addr_m.am_addr_line3 AS 'Address_Line_3', addr_m.am_postcode AS 'Postcode', addr_m.am_city AS 'City', " & _
                '            "code_s.code_desc AS 'State', code_c.code_desc AS 'Country' FROM users_addr AS ua_b " & _
                '            "LEFT JOIN (SELECT * FROM approval_grp_buyer AS ag_b INNER JOIN approval_grp_mstr AS ag_m " & _
                '            "ON ag_m.agm_grp_index = ag_b.agb_grp_index) AS tb_ap ON ua_b.ua_user_id = tb_ap.agb_buyer , " & _
                '            "users_location AS ua_sk, user_mstr AS usr, user_mstr AS usr2, address_mstr AS addr_m, code_mstr AS code_c, code_mstr AS code_s " & _
                '            "WHERE (usr.um_deleted <> 'Y' AND usr2.um_deleted <> 'Y' AND ua_b.ua_coy_id = '" & Session("CompanyID") & "' " & strsql & ") " & _
                '            "AND (tb_ap.agm_coy_id IS NULL OR tb_ap.agm_coy_id = '" & Session("CompanyID") & "') " & _
                '            "AND usr.um_user_id = ua_b.ua_user_id AND usr.um_coy_id = ua_b.ua_coy_id AND ua_b.ua_user_id = ua_b.ua_user_id " & _
                '            "AND ua_b.ua_addr_type = 'D' AND ua_b.ua_addr_code = ua_sk.ul_addr_code AND ua_b.ua_coy_id = ua_sk.ul_coy_id " & _
                '            "AND usr2.um_user_id = ua_sk.ul_user_id AND usr2.um_coy_id = ua_sk.ul_coy_id AND addr_m.am_addr_code = ua_sk.ul_addr_code " & _
                '            "AND addr_m.am_coy_id = ua_sk.ul_coy_id AND code_c.code_abbr = addr_m.am_country AND (code_s.code_abbr = addr_m.am_state " & _
                '            "AND code_s.code_value = addr_m.am_country) " & _
                '            "UNION " & _
                '            "SELECT ua_b.ua_user_id AS 'Buyer_ID', usr.um_user_name AS 'Buyer_Name', tb_ap.agm_type AS 'Group_Type', REPLACE(tb_ap.agm_grp_name,',','zzz') AS 'Approval_Group_Name'," & _
                '            "ua_sk.ul_user_id AS 'Storekeeper_ID', usr2.um_user_name AS 'Storekeeper_Name', " & _
                '            "ua_sk.ul_addr_code AS 'Delivery_Code', addr_m.am_addr_line1 AS 'Address_Line_1', addr_m.am_addr_line2 AS 'Address_Line_2', " & _
                '            "addr_m.am_addr_line3 AS 'Address_Line_3', addr_m.am_postcode AS 'Postcode', addr_m.am_city AS 'City', " & _
                '            "code_s.code_desc AS 'State', code_c.code_desc AS 'Country' FROM users_addr AS ua_b " & _
                '            "LEFT JOIN (SELECT * FROM approval_grp_buyer AS ag_b INNER JOIN approval_grp_mstr AS ag_m " & _
                '            "ON ag_m.agm_grp_index = ag_b.agb_grp_index) AS tb_ap ON ua_b.ua_user_id = tb_ap.agb_buyer , " & _
                '            "users_location AS ua_sk, user_mstr AS usr, user_mstr AS usr2, address_mstr AS addr_m, code_mstr AS code_c, code_mstr AS code_s " & _
                '            "WHERE (usr.um_deleted <> 'Y' AND usr2.um_deleted <> 'Y' AND ua_b.ua_coy_id = '" & Session("CompanyID") & "' " & strsql & ") " & _
                '            "AND (tb_ap.agm_coy_id  IS NULL OR tb_ap.agm_coy_id = '" & Session("CompanyID") & "') " & _
                '            "AND usr.um_user_id = ua_b.ua_user_id AND usr.um_coy_id = ua_b.ua_coy_id AND ua_b.ua_user_id = ua_b.ua_user_id " & _
                '            "AND ua_b.ua_addr_code = '0' AND ua_b.ua_addr_type = 'D' AND ua_b.ua_coy_id = ua_sk.ul_coy_id AND usr2.um_user_id = ua_sk.ul_user_id " & _
                '            "AND usr2.um_coy_id = ua_sk.ul_coy_id AND addr_m.am_addr_code = ua_sk.ul_addr_code AND addr_m.am_coy_id = ua_sk.ul_coy_id " & _
                '            "AND code_c.code_abbr = addr_m.am_country AND (code_s.code_abbr = addr_m.am_state " & _
                '            "AND code_s.code_value = addr_m.am_country)) AS zzz, users_usrgrp AS usr_grp1, user_group_mstr AS usr_gpm1, users_usrgrp AS usr_grp2, user_group_mstr AS usr_gpm2 " & _
                '            "WHERE (usr_grp1.uu_coy_id = '" & Session("CompanyID") & "' AND usr_grp1.uu_user_id = Buyer_ID AND usr_grp1.uu_usrgrp_id = usr_gpm1.ugm_usrgrp_id AND " & _
                '            "(usr_gpm1.ugm_fixed_role = 'Buyer' OR usr_gpm1.ugm_fixed_role = 'Purchasing Manager' OR usr_gpm1.ugm_fixed_role = 'Purchasing Officer')) " & _
                '            "AND (usr_grp2.uu_coy_id = '" & Session("CompanyID") & "' AND usr_grp2.uu_user_id = Storekeeper_ID AND usr_grp2.uu_usrgrp_id = usr_gpm2.ugm_usrgrp_id AND " & _
                '            "usr_gpm2.ugm_fixed_role = 'Store Keeper') " & _
                '            "ORDER BY Buyer_ID, Approval_Group_Name, Storekeeper_ID, Delivery_Code "


            End With

            da = New MySqlDataAdapter(cmd)
            da.SelectCommand.CommandTimeout = 10000
            da.Fill(ds)
            strFileName = "BuyerDeliveryAddressList" & "(" & Format(dtFrom, "MMMyyyy") & ").xls"
            Dim attachment As String = "attachment;filename=" & strFileName
            Response.ClearContent()
            Response.AddHeader("Content-Disposition", attachment)
            Response.ContentType = "application/vnd.ms-excel"

            Dim dc As DataColumn
            Dim i As Integer

            i = 0
            For Each dc In ds.Tables(0).Columns
                If i > 0 Then
                    Response.Write(vbTab + dc.ColumnName)
                Else
                    Response.Write(dc.ColumnName)
                End If
                i += 1

            Next
            Response.Write(vbCrLf)

            Dim dr As DataRow
            For Each dr In ds.Tables(0).Rows
                For i = 0 To ds.Tables(0).Columns.Count - 1
                    If i > 0 Then
                        Response.Write(vbTab + dr.Item(i).ToString)
                    Else
                        Response.Write(dr.Item(i).ToString)
                    End If
                Next
                Response.Write(vbCrLf)
            Next
            Response.End()

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

    Protected Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        If cboReportType.SelectedValue = "Excel" Then
            ExportToExcel()

        ElseIf cboReportType.SelectedValue = "PDF" Then
            ExportToPDF()
        End If

    End Sub
End Class