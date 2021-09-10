<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PreviewGRN-FTN.aspx.vb" Inherits="_Default" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        &nbsp;<rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana"
            Font-Size="8pt" Height="632px" Width="784px">
            <LocalReport ReportPath="PreviewGRN-FTN.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource2" Name="PreviewGRN_DataSetPreviewGRN" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" SelectMethod="GetDataPreviewGRN"
            TypeName="PreviewGRNTableAdapters.DataSetPreviewGRNTableAdapter"></asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData"
            TypeName="DataSet1TableAdapters.COMPANY_MSTRTableAdapter"></asp:ObjectDataSource>
    
    </div>
    </form>
</body>
</html>
