<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript" src="/Scripts/Home/Home.js"></script>

    <h2><%: ViewData["Message"] %></h2>
    <br />ID:&nbsp;<input id="id" type="text" value=""/>
    &nbsp;KEY:&nbsp;<input id="key" type="text" value=""/>
    &nbsp;ITEM GUID:&nbsp;<input id="guid" type="text" value=""/>
    <br /><br />
    XML:&nbsp;&nbsp;&nbsp;<a id="ProductsXml" href="#">GET Products</a>
    &nbsp;<a id="ProductXml" href="#">GET Product</a>
    &nbsp;<a id="AddProductXml" href="#">ADD Product</a>
    &nbsp;<a id="UpdateProductXml" href="#">UPDATE Product</a>
    &nbsp;<a id="CustomersXml" href="#">GET Customers</a>
    &nbsp;<a id="CustomerXml" href="#">GET Customer</a>
    &nbsp;<a id="AddCustomerXml" href="#">Add Customer</a>
    &nbsp;<a id="InvoicesXml" href="#">GET Invoices</a>
    &nbsp;<a id="InvoiceXml" href="#">GET Invoice</a>
    &nbsp;<a id="AddInvoiceXml" href="#">ADD Invoice</a>
    <br /><br />
    JSON:&nbsp;<a id="ProductsJson" href="#">GET Products</a>
    &nbsp;<a id="ProductJson" href="#">GET Product</a>
    &nbsp;<a id="AddProductJson" href="#">ADD Product</a>
    &nbsp;<a id="UpdateProductJson" href="#">UPDATE Product</a>
    &nbsp;<a id="CustomersJson" href="#">GET Customers</a>
    &nbsp;<a id="CustomerJson" href="#">GET Customer</a>
    &nbsp;<a id="AddCustomerJson" href="#">Add Customer</a>
    &nbsp;<a id="InvoicesJson" href="#">GET Invoices</a>
    &nbsp;<a id="InvoiceJson" href="#">GET Invoice</a>
    &nbsp;<a id="AddInvoiceJson" href="#">ADD Invoice</a>
    <br /><br />
    <textarea id="Results" rows="20" cols="120"></textarea>
</asp:Content>
