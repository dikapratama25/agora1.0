Partial Public Class DialogPDF
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.ContentType = "application/pdf"
        Response.AddHeader("Content-Disposition", _
        "inline; filename=ReleaseNote.pdf")
        Response.Flush()
        Response.WriteFile(Server.MapPath("../Version/ReleaseNote.pdf"))
        Response.End()

    End Sub
End Class