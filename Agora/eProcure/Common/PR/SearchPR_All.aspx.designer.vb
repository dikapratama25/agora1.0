'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:2.0.50727.3053
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On



'''<summary>
'''SearchPR_All class.
'''</summary>
'''<remarks>
'''Auto-generated class.
'''</remarks>
Partial Public Class SearchPR_All

    '''<summary>
    '''Form1 control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents Form1 As Global.System.Web.UI.HtmlControls.HtmlForm

    '''<summary>
    '''lblAction control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents lblAction As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''txtPRNo control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents txtPRNo As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''txtItemCode control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents txtItemCode As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''txtDateFr control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents txtDateFr As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''txtDateTo control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents txtDateTo As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''lblPRType control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents lblPRType As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''chkContPR control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents chkContPR As Global.System.Web.UI.WebControls.CheckBox

    '''<summary>
    '''chkNonContPR control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents chkNonContPR As Global.System.Web.UI.WebControls.CheckBox

    '''<summary>
    '''chkOpen control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents chkOpen As Global.System.Web.UI.WebControls.CheckBox

    '''<summary>
    '''chkSubmitted control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents chkSubmitted As Global.System.Web.UI.WebControls.CheckBox

    '''<summary>
    '''chkApproved control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents chkApproved As Global.System.Web.UI.WebControls.CheckBox

    '''<summary>
    '''chkConverted control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents chkConverted As Global.System.Web.UI.WebControls.CheckBox

    '''<summary>
    '''chkVoid control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents chkVoid As Global.System.Web.UI.WebControls.CheckBox

    '''<summary>
    '''chkCancel control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents chkCancel As Global.System.Web.UI.WebControls.CheckBox

    '''<summary>
    '''chkReject control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents chkReject As Global.System.Web.UI.WebControls.CheckBox

    '''<summary>
    '''chkSource control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents chkSource As Global.System.Web.UI.WebControls.CheckBox

    '''<summary>
    '''chkHold control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents chkHold As Global.System.Web.UI.WebControls.CheckBox

    '''<summary>
    '''cmdSearch control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents cmdSearch As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''tblSearchResult control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents tblSearchResult As Global.System.Web.UI.HtmlControls.HtmlTable

    '''<summary>
    '''dtgPRList control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents dtgPRList As Global.System.Web.UI.WebControls.DataGrid

    '''<summary>
    '''vldSumm control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents vldSumm As Global.System.Web.UI.WebControls.ValidationSummary

    '''<summary>
    '''vldDateFtDateTo control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents vldDateFtDateTo As Global.System.Web.UI.WebControls.CompareValidator

    '''<summary>
    '''vldDateFr control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents vldDateFr As Global.System.Web.UI.WebControls.CustomValidator

    '''<summary>
    '''vldDateTo control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents vldDateTo As Global.System.Web.UI.WebControls.CustomValidator
End Class
