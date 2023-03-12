$(document).ready(function () {

    $.ajaxSetup({
        data: "{}",
        dataFilter: function (data) {
            var msg;

            if (data == "") {
                msg = data;
            }
            else if (typeof (JSON) !== 'undefined' && typeof (JSON.parse) === 'function') {
                msg = JSON.parse(data);
            }
            else {
                msg = eval('(' + data + ')');
            }

            if (msg.hasOwnProperty('d')) {
                return msg.d;
            }
            else {
                return msg;
            }
        }
    });

    $("#ProductsXml").click(function () { Start(); var url = "/Home/ProductsXml"; $.get(url, { id: $("#id").val(), key: $("#key").val(), guid: $("#guid").val() }, function (data) { ShowXml(data.Xml); }); });
    $("#ProductXml").click(function () { Start(); var url = "/Home/ProductXml"; $.get(url, { id: $("#id").val(), key: $("#key").val(), guid: $("#guid").val() }, function (data) { ShowXml(data.Xml); }); });
    $("#AddProductXml").click(function () { Start(); var url = "/Home/AddProductXml"; $.get(url, { id: $("#id").val(), key: $("#key").val(), guid: $("#guid").val() }, function (data) { ShowXml(data.Xml); }); });
    $("#UpdateProductXml").click(function () { Start(); var url = "/Home/UpdateProductXml"; $.get(url, { id: $("#id").val(), key: $("#key").val(), guid: $("#guid").val() }, function (data) { ShowXml(data.Xml); }); });
    $("#CustomersXml").click(function () { Start(); var url = "/Home/CustomersXml"; $.get(url, { id: $("#id").val(), key: $("#key").val(), guid: $("#guid").val() }, function (data) { ShowXml(data.Xml); }); });
    $("#CustomerXml").click(function () { Start(); var url = "/Home/CustomerXml"; $.get(url, { id: $("#id").val(), key: $("#key").val(), guid: $("#guid").val() }, function (data) { ShowXml(data.Xml); }); });
    $("#AddCustomerXml").click(function () { Start(); var url = "/Home/AddCustomerXml"; $.get(url, { id: $("#id").val(), key: $("#key").val(), guid: $("#guid").val() }, function (data) { ShowXml(data.Xml); }); });
    $("#InvoicesXml").click(function () { Start(); var url = "/Home/InvoicesXml"; $.get(url, { id: $("#id").val(), key: $("#key").val(), guid: $("#guid").val() }, function (data) { ShowXml(data.Xml); }); });
    $("#InvoiceXml").click(function () { Start(); var url = "/Home/InvoiceXml"; $.get(url, { id: $("#id").val(), key: $("#key").val(), guid: $("#guid").val() }, function (data) { ShowXml(data.Xml); }); });
    $("#AddInvoiceXml").click(function () { Start(); var url = "/Home/AddInvoiceXml"; $.get(url, { id: $("#id").val(), key: $("#key").val(), guid: $("#guid").val() }, function (data) { ShowXml(data.Xml); }); });

    $("#ProductsJson").click(function () { Start(); var url = "/Home/ProductsJson"; $.get(url, { id: $("#id").val(), key: $("#key").val(), guid: $("#guid").val() }, function (data) { ShowJson(data.Json); }); });
    $("#ProductJson").click(function () { Start(); var url = "/Home/ProductJson"; $.get(url, { id: $("#id").val(), key: $("#key").val(), guid: $("#guid").val() }, function (data) { ShowJson(data.Json); }); });
    $("#AddProductJson").click(function () { Start(); var url = "/Home/AddProductJson"; $.get(url, { id: $("#id").val(), key: $("#key").val(), guid: $("#guid").val() }, function (data) { ShowJson(data.Json); }); });
    $("#UpdateProductJson").click(function () { Start(); var url = "/Home/UpdateProductJson"; $.get(url, { id: $("#id").val(), key: $("#key").val(), guid: $("#guid").val() }, function (data) { ShowJson(data.Json); }); });
    $("#CustomersJson").click(function () { Start(); var url = "/Home/CustomersJson"; $.get(url, { id: $("#id").val(), key: $("#key").val(), guid: $("#guid").val() }, function (data) { ShowJson(data.Json); }); });
    $("#CustomerJson").click(function () { Start(); var url = "/Home/CustomerJson"; $.get(url, { id: $("#id").val(), key: $("#key").val(), guid: $("#guid").val() }, function (data) { ShowJson(data.Json); }); });
    $("#AddCustomerJson").click(function () { Start(); var url = "/Home/AddCustomerJson"; $.get(url, { id: $("#id").val(), key: $("#key").val(), guid: $("#guid").val() }, function (data) { ShowJson(data.Json); }); });
    $("#InvoicesJson").click(function () { Start(); var url = "/Home/InvoicesJson"; $.get(url, { id: $("#id").val(), key: $("#key").val(), guid: $("#guid").val() }, function (data) { ShowJson(data.Json); }); });
    $("#InvoiceJson").click(function () { Start(); var url = "/Home/InvoiceJson"; $.get(url, { id: $("#id").val(), key: $("#key").val(), guid: $("#guid").val() }, function (data) { ShowJson(data.Json); }); });
    $("#AddInvoiceJson").click(function () { Start(); var url = "/Home/AddInvoiceJson"; $.get(url, { id: $("#id").val(), key: $("#key").val(), guid: $("#guid").val() }, function (data) { ShowJson(data.Json); }); });

    function Start() {
        $("#Results").html("Loading, please wait ... ");
    }

    function ShowXml(xml) {
        if (xml) {
            $("#Results").html(htmlEncode(xml));
        }
    }

    function ShowJson(json) {
        if (json) {
            $("#Results").html(htmlEncode(json));
        }
    }

    function htmlEncode(value) {
        return $('<div/>').text(value).html();
    }

    function htmlDecode(value) {
        return $('<div/>').html(value).text();
    }


});
