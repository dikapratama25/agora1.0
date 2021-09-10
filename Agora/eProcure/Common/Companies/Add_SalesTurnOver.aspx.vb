Imports AgoraLegacy
Imports eProcure.Component

Public Class Add_SalesTurnOver
    Inherits AgoraLegacy.AppBaseClass

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MyBase.Page_Load(sender, e)
        If Not IsPostBack Then
            PopulateCodeTable()
        End If



    End Sub

    Private Sub PopulateCodeTable()
        Dim objGlobal As New AppGlobals

        objGlobal.FillCodeTable(ddlCurrency, CodeTable.Currency)




        'populate ddl year
        Dim ii_ddl, ii_ddl2, jj_ddl As Integer
        ii_ddl2 = 0
        jj_ddl = Year(Date.Now)
        For ii_ddl = jj_ddl To (jj_ddl - 100) Step -1

            ddlYear.Items.Insert(ii_ddl2, New ListItem(ii_ddl))

            ii_ddl2 = ii_ddl2 + 1
        Next
        Dim lstItem, firstCurrency As New ListItem
        Dim modify_year, modify_currency, modify_amount As String
        modify_year = Request.QueryString("itemid")
        modify_currency = Request.QueryString("itemcurrency")
        modify_amount = FormatNumber(Request.QueryString("itemamount"), , , , 0)

        If Session("userAction") = "Add" Then
            lstItem.Value = ""
            lstItem.Text = "---Select---"
            ddlYear.Items.Insert(0, lstItem)
            ddlYear.Enabled = True
        Else
            ddlCurrency.Items.FindByValue("MYR").Selected = False
            lstItem.Value = modify_year
            lstItem.Text = modify_year
            ddlYear.Items.Insert(0, lstItem)
            firstCurrency.Value = modify_currency
            firstCurrency.Text = modify_currency
            ddlCurrency.Items.FindByValue(modify_currency).Selected = True
            txtAmount.Text = modify_amount
            ddlYear.Enabled = False
        End If



    End Sub

    Protected Sub cmd_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Save.Click
        Dim objsales As New Admin
        Dim chkerror As Boolean
        Dim Year As String
        Dim Currency As String
        Dim amount As Decimal
        Dim itemid As String
        Dim strscript As New System.Text.StringBuilder
        If Page.IsValid Then
            Year = ddlYear.SelectedValue
            Currency = ddlCurrency.SelectedValue
            amount = txtAmount.Text


            If Session("userAction") = "Add" Then

                chkerror = objsales.AddSalesTurnOver(Session("CompanyId"), Year, Currency, amount)
                If chkerror = False Then
                    Common.NetMsgbox(Me, "The year " & Year & " is already exist", MsgBoxStyle.Information)
                Else
                    strscript.Append("<script language=""javascript"">")
                    strscript.Append("document.getElementById('btnclose').click();")
                    strscript.Append("</script>")
                    RegisterStartupScript("script3", strscript.ToString())
                End If


            End If

            If Session("userAction") = "Modify" Then
                itemid = Request.QueryString("itemid")
                objsales.ModifySalesTurnOver(Session("CompanyId"), Year, Currency, amount)
                strscript.Append("<script language=""javascript"">")
                strscript.Append("document.getElementById('btnclose').click();")
                strscript.Append("</script>")
                RegisterStartupScript("script3", strscript.ToString())
            End If
        End If




    End Sub

End Class