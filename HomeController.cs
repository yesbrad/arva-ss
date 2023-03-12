using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using System.Xml;

namespace Unleashed.API.TestClient.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private const string ApiHost = "https://api.unleashedsoftware.com"; 
        //private const string ApiHost = "https://local.unleashedsoftware.com/api"; // 

        public ActionResult Index()
        {
            ViewData["Message"] = "Unleashed Software API Test Client";
            return View();
        }

        public HomeController()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        // allow untrusted certificate
        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
            //if (sslPolicyErrors.HasFlag(SslPolicyErrors.RemoteCertificateNameMismatch))
            //    return true;
            //return false;
        }

        private static void SetAuthenticationHeaders(WebClient client, string query, RequestType requestType, string id, string key)
        {
            string signature = GetSignature(query, key);
            client.Headers.Add("api-auth-id", id);
            client.Headers.Add("api-auth-signature", signature);
            
            if (requestType == RequestType.Xml)
            {
                client.Headers.Add("Accept", "application/xml");
                client.Headers.Add("Content-Type", "application/xml; charset=" + client.Encoding.WebName);
            }
            else
            {
                client.Headers.Add("Accept", "application/json");
                client.Headers.Add("Content-Type", "application/json; charset=" + client.Encoding.WebName);
            }
        }

        private static string GetSignature(string args, string privatekey)
        {
            var encoding = new System.Text.UTF8Encoding();
            byte[] key = encoding.GetBytes(privatekey);
            var myhmacsha256 = new HMACSHA256(key);
            byte[] hashValue = myhmacsha256.ComputeHash(encoding.GetBytes(args));
            string hmac64 = Convert.ToBase64String(hashValue);
            myhmacsha256.Clear();
            return hmac64;
        }

        private static string Get(WebClient client, string uri)
        {
            string response = string.Empty;
            try
            {
                response = client.DownloadString(uri);
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    var stream = ex.Response.GetResponseStream();
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            response = reader.ReadToEnd();
                        }
                    }
                }
            }
            return response;
        }

        private static string Post(WebClient client, string uri, string postData)
        {
            string response = string.Empty;
            try
            {
                response = client.UploadString(uri, "POST", postData);
            }
            catch (WebException ex)
            {
                var stream = ex.Response.GetResponseStream();
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        response = reader.ReadToEnd();
                    }
                }
            }
            return response;
        }

        private static string GetXml(string resource, string id, string key, string guid)
        {
            string uri = guid != string.Empty ? string.Format("{0}/{1}/{2}", ApiHost, resource, guid) : string.Format("{0}/{1}", ApiHost, resource);
            
            var client = new WebClient();
            
            const string query = "format=xml";
            SetAuthenticationHeaders(client, query, RequestType.Xml, id, key);
            ServicePointManager.ServerCertificateValidationCallback += ValidateServerCertificate;

            string xml = Get(client, string.Format("{0}?{1}", uri, query));

            var xmlDocument = new XmlDocument { PreserveWhitespace = true };
            xmlDocument.LoadXml(xml);
            return xmlDocument.InnerXml;
        }

        private static string PostXml(string resource, string id, string key, string guid, string postData)
        {
            string uri = string.Format("{0}/{1}/{2}", ApiHost, resource, guid);
            var client = new WebClient();
            string query = string.Empty;
            SetAuthenticationHeaders(client, query, RequestType.Xml, id, key);
            ServicePointManager.ServerCertificateValidationCallback += ValidateServerCertificate;

            string xml = Post(client, uri, postData);

            var xmlDocument = new XmlDocument { PreserveWhitespace = true };
            xmlDocument.LoadXml(xml);
            return xmlDocument.InnerXml;
        }

        private static string GetJson(string resource, string id, string key, string guid)
        {
            string uri = guid != string.Empty ? string.Format("{0}/{1}/{2}", ApiHost, resource, guid) : string.Format("{0}/{1}", ApiHost, resource);

            var client = new WebClient();
            const string query = "format=json";
            SetAuthenticationHeaders(client, query, RequestType.Json, id, key);
            ServicePointManager.ServerCertificateValidationCallback += ValidateServerCertificate;

            return Get(client, string.Format("{0}?{1}", uri, query));
        }

        private static string PostJson(string resource, string id, string key, string guid, string postData)
        {
            string uri = string.Format("{0}/{1}/{2}", ApiHost, resource, guid);
            var client = new WebClient();
            string query = string.Empty;
            SetAuthenticationHeaders(client, query, RequestType.Json, id, key);
            ServicePointManager.ServerCertificateValidationCallback += ValidateServerCertificate;

            return Post(client, uri, postData);
        }
                
        public JsonResult AddProductXml(string id, string key, string guid)
        {
            Guid newGuid = Guid.NewGuid();
            string postData = ProductXmlData(newGuid);
            return Json(new { Xml = PostXml("Products", id, key, newGuid.ToString(), postData) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddCustomerXml(string id, string key, string guid)
        {
            Guid newGuid = Guid.NewGuid();
            string postData = CustomerXmlData(newGuid);
            return Json(new { Xml = PostXml("Customers", id, key, newGuid.ToString(), postData) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateProductXml(string id, string key, string guid)
        {
            string postData = ProductXmlUpdateData(guid);
            return Json(new { Xml = PostXml("Products", id, key, guid, postData) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddInvoiceXml(string id, string key, string guid)
        {
            Guid newGuid = Guid.NewGuid();
            Guid lineGuid = Guid.NewGuid();
            //Guid lineGuid2 = Guid.NewGuid();
            
            //string postData = InvoiceXmlData(newGuid, lineGuid);
            //string postData = InvoiceXmlDataTest1(newGuid, lineGuid);
            //string postData = InvoiceXmlDataTest2(newGuid, lineGuid);
            //string postData = InvoiceXmlDataTest3(newGuid, lineGuid);
            //string postData = InvoiceXmlDataTest4(newGuid, lineGuid, lineGuid2);
            string postData = InvoiceXmlDataTest5(newGuid, lineGuid);          

            return Json(new { Xml = PostXml("SalesInvoices", id, key, newGuid.ToString(), postData) }, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult ProductsXml(string id, string key, string guid)
        {
            return Json(new { Xml = GetXml("Products", id, key, string.Empty) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProductXml(string id, string key, string guid)
        {
            return Json(new { Xml = GetXml("Products", id, key, guid) }, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult CustomersXml(string id, string key, string guid)
        {
            return Json(new { Xml = GetXml("Customers", id, key, string.Empty) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CustomerXml(string id, string key, string guid)
        {
            return Json(new { Xml = GetXml("Customers", id, key, guid) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult InvoicesXml(string id, string key, string guid)
        {
            return Json(new { Xml = GetXml("SalesInvoices", id, key, string.Empty) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult InvoiceXml(string id, string key, string guid)
        {
            return Json(new { Xml = GetXml("SalesInvoices", id, key, guid) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProductsJson(string id, string key, string guid)
        {
            return Json(new { Json = GetJson("Products", id, key, string.Empty) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CustomersJson(string id, string key, string guid)
        {
            return Json(new { Json = GetJson("Customers", id, key, string.Empty) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CustomerJson(string id, string key, string guid)
        {
            return Json(new { Json = GetJson("Customers", id, key, guid) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult InvoiceJson(string id, string key, string guid)
        {
            return Json(new { Json = GetJson("SalesInvoices", id, key, guid) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult InvoicesJson(string id, string key, string guid)
        {
            return Json(new { Json = GetJson("SalesInvoices", id, key, string.Empty) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProductJson(string id, string key, string guid)
        {
            return Json(new { Json = GetJson("Products", id, key, guid) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddProductJson(string id, string key, string guid)
        {
            Guid newGuid = Guid.NewGuid();
            string postData = ProductJsonData(newGuid);
            return Json(new { Xml = PostXml("Products", id, key, newGuid.ToString(), postData) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateProductJson(string id, string key, string guid)
        {
            string postData = ProductJsonUpdateData(guid);
            return Json(new { Xml = PostXml("Products", id, key, guid, postData) }, JsonRequestBehavior.AllowGet);
        } 

        public JsonResult AddInvoiceJson()
        {
            // implementation of this method left as an exercise for the reader ;)
            return null;
        }

        public ActionResult About()
        {
            return View();
        }


        private static string CustomerXmlData(Guid id)
        {
            string xml = @"<?xml version='1.0'?>
<Customer xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns='http://api.unleashedsoftware.com/version/1'>
  <Guid>{0}</Guid>
  <CustomerCode>ACE001-{0}</CustomerCode>
  <CustomerName>Ace Outlets - {0}</CustomerName>
</Customer>";
            xml = xml.Replace("\r\n", "");
            xml = string.Format(xml, id);
            return xml;
        }

        private static string ProductXmlData(Guid id)
        {            
string xml = @"<?xml version='1.0'?>
<Product xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns='http://api.unleashedsoftware.com/version/1'>
  <Guid>{0}</Guid>
  <ProductCode>ANIMAL-{0}</ProductCode>
  <ProductDescription>Animal Shapes Box Set - xml - {0}</ProductDescription>  
</Product>
";
            xml = xml.Replace("\r\n", "");
            xml = string.Format(xml, id);
            return xml;
        }

        private static string ProductXmlUpdateData(string id)
        {            
            string xml = @"<?xml version='1.0'?>
<Product xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns='http://api.unleashedsoftware.com/version/1'>
  <Guid>{0}</Guid>
  <ProductCode>ANIMAL-{0}</ProductCode>
  <ProductDescription>UPDATED Animal Shapes Box Set - {0}</ProductDescription>  
</Product>";
            xml = xml.Replace("\r\n", "");
            xml = string.Format(xml, id);
            return xml;
        }

        private static string ProductJsonData(Guid id)
        {
            return string.Format("{{\"Guid\":\"{0}\",\"ProductCode\":\"ANIMAL-json-{0}\",\"ProductDescription\":\"Animal Shapes Box Set\"}}", id);
        }

        private static string ProductJsonUpdateData(string id)
        {
            return string.Format("{{\"Guid\":\"{0}\",\"ProductCode\":\"ANIMAL-{0}\",\"ProductDescription\":\"UPDATED Animal Shapes Box Set\"}}", id);
        }

        // this xml should create an invoice successfully
        private static string InvoiceXmlData(Guid id, Guid lineId)
        {
            string xml = @"<?xml version='1.0'?>
<SalesInvoice xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns='http://api.unleashedsoftware.com/version/1'>
  <Guid>{0}</Guid>
  <OrderNumber>SI-{1}</OrderNumber>
  <OrderDate>2011-08-03</OrderDate>
  <RequiredDate>2011-08-03</RequiredDate>
  <OrderStatus>Parked</OrderStatus>
  <Customer>
    <Guid>78c57132-27d4-4160-841f-deb4c1594fcb</Guid>
    <CustomerCode>ACE001</CustomerCode>
    <CustomerName>Ace Outlets</CustomerName>
  </Customer>
  <Warehouse>
    <Guid>1c783f0b-2c04-4de9-a7ee-a2ff5ded4480</Guid>
  </Warehouse>
  <Currency>
    <Guid>150cd526-93f9-4f61-9a55-c7077ef40422</Guid>
  </Currency>
  <ExchangeRate>1.000000</ExchangeRate>
  <DiscountRate>0.0000</DiscountRate>
  <Tax>
    <Guid>c6f56593-ced8-479d-a32b-19fef5c71e36</Guid>
  </Tax>
  <TaxRate>0.1500</TaxRate>
  <XeroTaxCode>OUTPUT2</XeroTaxCode>
  <SubTotal>150.000</SubTotal>
  <TaxTotal>22.500</TaxTotal>
  <Total>172.500</Total>
  <BCSubTotal>150.000</BCSubTotal>
  <BCTaxTotal>0.000</BCTaxTotal>
  <BCTotal>150.000</BCTotal>
  <PaymentDueDate>2011-09-20</PaymentDueDate>
  <SalesOrderLines>
    <SalesInvoiceLine>
      <Guid>{2}</Guid>
      <LineNumber>1</LineNumber>
      <Product>
        <Guid>89da0961-3ca8-4d41-b4f0-37648cb7274d</Guid>
      </Product>
      <DueDate>2011-08-03</DueDate>
      <OrderQuantity>10.000</OrderQuantity>
      <UnitPrice>15.000</UnitPrice>
      <LineTotal>150.00000000</LineTotal>
      <AverageLandedPriceAtTimeOfSale>9.50000000000000</AverageLandedPriceAtTimeOfSale>
      <StockOnHandBeforeSale>100.000</StockOnHandBeforeSale>
      <TaxRate>0.1500</TaxRate>
      <LineTax>22.500</LineTax>
      <XeroTaxCode>OUTPUT2</XeroTaxCode>
      <BCUnitPrice>15.000</BCUnitPrice>
      <BCLineTotal>150.000</BCLineTotal>
      <BCLineTax>0.000</BCLineTax>
    </SalesInvoiceLine>
  </SalesOrderLines>
</SalesInvoice>";

            xml = xml.Replace("\r\n", "");
            string[] parts = id.ToString().Split('-');
            xml = string.Format(xml, id, parts[0], lineId);
            return xml;
        }

        // This will fail because the root element must be SalesInvoice, not SalesInvoices
        private static string InvoiceXmlDataTest1(Guid id, Guid lineId)
        {
            string xml = @"<?xml version='1.0'?>
<SalesInvoices xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns='http://api.unleashedsoftware.com/version/1'>
<SalesInvoice>
  <Customer>
    <CustomerCode>ACE001</CustomerCode>
  </Customer>
  <PaymentDueDate>2011-09-20</PaymentDueDate>
  <SalesOrderLines>
<SalesInvoiceLine>
      <Product>
        <Guid>ddf5c0b3-919b-4274-9f43-ec1dee09225c</Guid>
      </Product>
      <OrderQuantity>1</OrderQuantity>
      <UnitPrice>50</UnitPrice>
    </SalesInvoiceLine>
  </SalsOrderLines>
</SalesInvoice>
</SalesInvoices>";
            xml = xml.Replace("\r\n", "");
            return xml;
        }

        // This will fail because currency + warehouse + tax are required
        private static string InvoiceXmlDataTest2(Guid id, Guid lineId)
        {
            string xml = @"<?xml version='1.0'?>
  <SalesInvoice xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns='http://api.unleashedsoftware.com/version/1'>
    <Guid>7982c062-db3c-4536-83ab-0bda4db399bb</Guid>
    <OrderNumber>SI-00000006</OrderNumber>
    <OrderStatus>Parked</OrderStatus>
    <Customer>
      <Guid>78c57132-27d4-4160-841f-deb4c1594fcb</Guid>
    </Customer>
  </SalesInvoice>";
            xml = xml.Replace("\r\n", "");
            return xml;
        }

        // This will fail because currency + warehouse + tax are required
        private static string InvoiceXmlDataTest3(Guid id, Guid lineId)
        {
            string xml = @"<?xml version='1.0'?>
<SalesInvoice xmlns='http://api.unleashedsoftware.com/version/1'>
  <Guid>ff1c460b-6926-44ca-9542-6b7866ba00fe</Guid>
  <OrderNumber>SI-00000010</OrderNumber>
  <OrderDate>2011-08-17T17:26:17.7610123+12:00</OrderDate>
  <QuoteExpiryDate>2011-08-17T17:26:17.7610123+12:00</QuoteExpiryDate>
  <RequiredDate>2011-08-18T17:26:17.7610123+12:00</RequiredDate>
  <OrderStatus>Parked</OrderStatus>
  <Customer>
    <Guid>8f77da9e-086c-4436-ac2d-4d997d78c865</Guid>
  </Customer>
  <CustomerRef>Customer ref here</CustomerRef>
  <Comments>Comments here</Comments>
  <DiscountRate>0.1</DiscountRate>
  <SalesOrderLines>
    <SalesInvoiceLine>
      <Guid>ff2880a2-02ce-4759-9a27-206bb46d49a4</Guid>
      <LineNumber>1</LineNumber>
      <Product>
        <Guid>ddf5c0b3-919b-4274-9f43-ec1dee09225c</Guid>
      </Product>
      <DueDate>2011-08-18T17:26:17.7610123+12:00</DueDate>
      <OrderQuantity>1</OrderQuantity>
      <UnitPrice>50.00</UnitPrice>
      <DiscountRate>0.1</DiscountRate>
    </SalesInvoiceLine>
    <SalesInvoiceLine>
      <Guid>dec21e14-0c69-474d-b306-58d44ec1ecca</Guid>
      <LineNumber>2</LineNumber>
      <Product>
        <Guid>edc37c8f-0eb5-435d-9aa2-691598cdae02</Guid>
      </Product>
      <DueDate>2011-08-18T17:26:17.7610123+12:00</DueDate>
      <OrderQuantity>5</OrderQuantity>
      <UnitPrice>4</UnitPrice>
      <DiscountRate>0.1</DiscountRate>
    </SalesInvoiceLine>
  </SalesOrderLines>
</SalesInvoice>";
            xml = xml.Replace("\r\n", "");
            return xml;
        }

        // This will fail because invoice linetotal + subtotal + tax + total are null
        private static string InvoiceXmlDataTest4(Guid id, Guid lineId, Guid lineId2)
        {
            string xml = @"<?xml version='1.0'?>
<SalesInvoice xmlns='http://api.unleashedsoftware.com/version/1'>
  <Guid>{0}</Guid>
  <OrderNumber>SI-{1}</OrderNumber>
  <OrderDate>2011-08-24T09:09:25.6397133+12:00</OrderDate>
  <QuoteExpiryDate>2011-08-24T09:09:25.6397133+12:00</QuoteExpiryDate>
  <RequiredDate>2011-08-25T09:09:25.6397133+12:00</RequiredDate>
  <OrderStatus>Parked</OrderStatus>
  <Customer>
    <Guid>78c57132-27d4-4160-841f-deb4c1594fcb</Guid>
  </Customer>
  <CustomerRef>Customer ref here</CustomerRef>
  <Comments>Comments here</Comments>
  <Warehouse>
    <Guid>1c783f0b-2c04-4de9-a7ee-a2ff5ded4480</Guid>
  </Warehouse>
  <Currency>
    <Guid>150cd526-93f9-4f61-9a55-c7077ef40422</Guid>
  </Currency>
  <ExchangeRate>0</ExchangeRate>
  <DiscountRate>0.1</DiscountRate>
  <Tax>
    <Guid>c6f56593-ced8-479d-a32b-19fef5c71e36</Guid>
  </Tax>
  <TaxRate>0.1500</TaxRate>
  <XeroTaxCode>OUTPUT2</XeroTaxCode>
  <PaymentDueDate>2011-09-03T09:09:25.6397133+12:00</PaymentDueDate>
  <SalesOrderLines>
    <SalesInvoiceLine>
      <Guid>{2}</Guid>
      <LineNumber>1</LineNumber>
      <Product>
        <Guid>9bb057ce-018b-4c52-b3d3-5f5bcfff59da</Guid>
      </Product>
      <DueDate>2011-08-25T09:09:25.6397133+12:00</DueDate>
      <OrderQuantity>50</OrderQuantity>
      <UnitPrice>500.00</UnitPrice>
      <DiscountRate>0.1</DiscountRate>
      <LineTax>0.15</LineTax>
      <BCUnitPrice>5</BCUnitPrice>
    </SalesInvoiceLine>
    <SalesInvoiceLine>
      <Guid>{3}</Guid>
      <LineNumber>2</LineNumber>
      <Product>
        <Guid>c508186f-3db5-48fa-807c-a50e69433058</Guid>
      </Product>
      <DueDate>2011-08-25T09:09:25.6397133+12:00</DueDate>
      <OrderQuantity>4</OrderQuantity>
      <UnitPrice>6000</UnitPrice>
      <DiscountRate>0.1</DiscountRate>
      <LineTax>0.15</LineTax>
      <BCUnitPrice>5</BCUnitPrice>
    </SalesInvoiceLine>
  </SalesOrderLines>
</SalesInvoice>";
            xml = xml.Replace("\r\n", "");

            string[] parts = id.ToString().Split('-');
            xml = string.Format(xml, id, parts[0], lineId, lineId2);

            return xml;
        }

        // this will create an invoice successfully
        private static string InvoiceXmlDataTest5(Guid id, Guid lineId)
        {
            string xml = @"<?xml version='1.0'?>
<SalesInvoice xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns='http://api.unleashedsoftware.com/version/1'>
  <Guid>{0}</Guid>
  <OrderNumber>SI-{1}</OrderNumber>
  <OrderDate>2011-08-03</OrderDate>
  <RequiredDate>2011-08-03</RequiredDate>
  <OrderStatus>Parked</OrderStatus>
  <Customer>
    <CustomerCode>Guest</CustomerCode>
  </Customer>
  <Warehouse>
    <WarehouseCode>W1</WarehouseCode>
  </Warehouse>
  <Currency>
    <CurrencyCode>NZD</CurrencyCode>
  </Currency>
  <ExchangeRate>1.000000</ExchangeRate>
  <DiscountRate>0.0000</DiscountRate>
  <Tax>
    <TaxCode>G.S.T.</TaxCode>
  </Tax>
  <TaxRate>0.1500</TaxRate>
  <XeroTaxCode>OUTPUT2</XeroTaxCode>
  <SubTotal>150.000</SubTotal>
  <TaxTotal>22.500</TaxTotal>
  <Total>172.500</Total>
  <BCSubTotal>150.000</BCSubTotal>
  <BCTaxTotal>0.000</BCTaxTotal>
  <BCTotal>150.000</BCTotal>
  <PaymentDueDate>2011-09-20</PaymentDueDate>
  <SalesOrderLines>
    <SalesInvoiceLine>
      <Guid>{2}</Guid>
      <LineNumber>1</LineNumber>
      <Product>
        <ProductCode>Freight</ProductCode>
      </Product>
      <DueDate>2011-08-03</DueDate>
      <OrderQuantity>10.000</OrderQuantity>
      <UnitPrice>15.000</UnitPrice>
      <LineTotal>150.00000000</LineTotal>
      <AverageLandedPriceAtTimeOfSale>9.50000000000000</AverageLandedPriceAtTimeOfSale>
      <StockOnHandBeforeSale>100.000</StockOnHandBeforeSale>
      <TaxRate>0.1500</TaxRate>
      <LineTax>22.500</LineTax>
      <XeroTaxCode>OUTPUT2</XeroTaxCode>
      <BCUnitPrice>15.000</BCUnitPrice>
      <BCLineTotal>150.000</BCLineTotal>
      <BCLineTax>0.000</BCLineTax>
    </SalesInvoiceLine>
  </SalesOrderLines>
</SalesInvoice>";

            xml = xml.Replace("\r\n", "");
            string[] parts = id.ToString().Split('-');
            xml = string.Format(xml, id, parts[0], lineId);
            return xml;
        }
   
    }

    public enum RequestType { Xml = 0, Json = 1 }

}