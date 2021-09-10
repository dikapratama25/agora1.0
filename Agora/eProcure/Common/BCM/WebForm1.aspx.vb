Imports AgoraLegacy

Public Class WebForm1
    Inherits System.Web.UI.Page
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents btnExpand As System.Web.UI.WebControls.ImageButton


    Dim ds As DataSet
    Dim objDb As New EAD.DBCom(System.Configuration.ConfigurationManager.AppSettings("SHAPE"))
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
        If Not IsPostBack Then
            Dim strSQL As String
            strSQL = "Shape{select * from account_mstr where AM_LEVEL=1} append " _
            & "({select * from account_mstr where am_parent_acct_index is not null AND AM_LEVEL=2} " _
            & "relate AM_ACCT_INDEX to AM_PARENT_ACCT_INDEX)"

            ds = objDb.FillDs(strSQL)
            DataGrid1.DataSource = ds.Tables(0).DefaultView
            DataGrid1.DataBind()


        End If
        btnExpand.ImageUrl = dDispatcher.direct("Plugins/images", "Plus.gif")
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        Select Case e.CommandName
            Case "Expand"
                Dim ExpandedContent As PlaceHolder = e.Item.Cells(DataGrid1.Columns.Count - 1).FindControl("ExpandedContent")
                ExpandedContent.Visible = Not ExpandedContent.Visible


                Dim btnExpand As ImageButton = e.Item.Cells(0).FindControl("btnExpand")
                If btnExpand.ImageUrl = dDispatcher.direct("Common/Images", "Plus.gif") Then
                    btnExpand.ImageUrl = dDispatcher.direct("Common/Images", "Minus.gif")
                Else
                    btnExpand.ImageUrl = dDispatcher.direct("Common/Images", "Plus.gif")
                End If
        End Select
    End Sub



    Private Sub DataGrid1_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemCreated

        Dim dg As DataGrid = Page.FindControl("DataGrid2")
        dg.DataSource = ds.Tables(0).DefaultView
        dg.DataBind()
    End Sub
End Class
