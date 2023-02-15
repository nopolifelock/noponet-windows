using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Http;
using Titanium.Web.Proxy.Models;

namespace Proxy
{
   
    class Noposerver
    {

        private string[] whiteList = {"github.com"};
        private string[] keywords = { "github"};
        private string[] localWhiteList;
        private string[] localKeywords;
        private string trollpage;
        private FeedServer feedServer;
        static void Main(string[] args)
        {
            LocalWeb localweb = new LocalWeb();
            new Thread(localweb.run).Start();
            Noposerver p = new Noposerver();
            p.run();
            
            Console.ReadLine();
        }
        public string[] LoadArrayFromFile(string path)
        {
            List<string> lines = new List<string>();
            try
            {
                if (!File.Exists(path))
                {
                    Console.WriteLine("File not found at: " + path);
                    return null;
                }

               
                using (StreamReader reader = new StreamReader(path))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                }
            }catch(Exception e)
            {
                Console.WriteLine("Error reading file " + e.Message);
            }

            return lines.ToArray();
        }
        public void loadWhiteList()
        {
            using (var client = new WebClient())
            {
                try
                {
                    client.Proxy = null;
                    string data = client.DownloadString("https://raw.githubusercontent.com/nopolifelock/noponet/main/whitelist.txt");
                    string[] lines = data.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                    this.whiteList = lines;
                    //Console.WriteLine(lines.Length + " Whitelisted sites loaded");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred loading keywords: " + ex.Message);
                }
            }
           this.localWhiteList =  this.LoadArrayFromFile("C:\\Program Files (x86)\\noponet\\lists\\whitelist.txt");
        }

        public void refresh()
        {
            loadWhiteList();
            loadKeywords();
        }
        public void downloadTrollPage()
        {
            try
            {
                string url = "https://raw.githubusercontent.com/nopolifelock/noponet/main/trollpage/troll.html";
                var client = new WebClient();
                client.Proxy = null;
                this.trollpage = client.DownloadString(url);
            }catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
                
        }
        public void loadKeywords()
        {
            using (var client = new WebClient())
            {
                try
                {
                    client.Proxy = null;
                    string data = client.DownloadString("https://raw.githubusercontent.com/nopolifelock/noponet/main/keywords.txt");
                    string[] lines = data.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                    for(int i = 0; i<lines.Length; i++)
                    {
                        if(lines[i].Equals(""))
                            lines[i] = "wiki";
                    }
                    this.keywords = lines;
                    //Console.WriteLine(lines.Length + " Keywords loaded");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred loading keywords: " + ex.Message);
                }
            }
           this.localKeywords =  this.LoadArrayFromFile("C:\\Program Files (x86)\\noponet\\lists\\keywords.txt");
        }

        private Boolean isSafe(string url)
        {
            for (int i = 0; i < whiteList.Length; i++)
            {
                if (whiteList[i].Equals(url))
                {
                    return true;
                }
                else
                {
                    
                }
                
            }
            for (int j = 0; j < keywords.Length; j++)
            {
                if (url.Contains(keywords[j]) && !keywords[j].Equals(""))
                {
                    return true;
                }
            }

            if(localWhiteList!=null)
            for (int i = 0; i < localWhiteList.Length; i++)
            {
                if (localWhiteList[i].Equals(url))
                {
                    return true;
                }
                else
                {

                }

            }
            if(localKeywords!=null)
            for (int j = 0; j < localKeywords.Length; j++)
            {
                if (url.Contains(localKeywords[j]) && !localKeywords[j].Equals(""))
                {
                    return true;
                }
            }

            return false;

        }

        public void run()
        {
            downloadTrollPage();

            this.feedServer = new FeedServer(this);
            new Thread(this.feedServer.run).Start();
            do
            {
                
                this.loadKeywords();
                this.loadWhiteList();
                Console.WriteLine("whitelist loaded");
                Thread.Sleep(3000);
            } while ((this.keywords.Length==1) || (this.whiteList.Length==1));
            var proxyServer = new ProxyServer();

            // locally trust root certificate used by this proxy 
            proxyServer.CertificateManager.TrustRootCertificate(true);

            // optionally set the Certificate Engine
            // Under Mono only BouncyCastle will be supported
            //proxyServer.CertificateManager.CertificateEngine = Network.CertificateEngine.BouncyCastle;

            proxyServer.BeforeRequest += OnRequest;
            proxyServer.BeforeResponse += OnResponse;
            proxyServer.ServerCertificateValidationCallback += OnCertificateValidation;
            proxyServer.ClientCertificateSelectionCallback += OnCertificateSelection;


            var explicitEndPoint = new ExplicitProxyEndPoint(IPAddress.Any, 8000, true)
            {
                // Use self-issued generic certificate on all https requests
                // Optimizes performance by not creating a certificate for each https-enabled domain
                // Useful when certificate trust is not required by proxy clients
                //GenericCertificate = new X509Certificate2(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "genericcert.pfx"), "password")
            };

            // Fired when a CONNECT request is received
            explicitEndPoint.BeforeTunnelConnectRequest += OnBeforeTunnelConnectRequest;

            // An explicit endpoint is where the client knows about the existence of a proxy
            // So client sends request in a proxy friendly manner
            proxyServer.AddEndPoint(explicitEndPoint);
            proxyServer.Start();

            // Transparent endpoint is useful for reverse proxy (client is not aware of the existence of proxy)
            // A transparent endpoint usually requires a network router port forwarding HTTP(S) packets or DNS
            // to send data to this endPoint
            var transparentEndPoint = new TransparentProxyEndPoint(IPAddress.Any, 8001, true)
            {
                // Generic Certificate hostname to use
                // when SNI is disabled by client
                GenericCertificateName = "google.com"
            };

            proxyServer.AddEndPoint(transparentEndPoint);

            //proxyServer.UpStreamHttpProxy = new ExternalProxy() { HostName = "localhost", Port = 8888 };
            //proxyServer.UpStreamHttpsProxy = new ExternalProxy() { HostName = "localhost", Port = 8888 };

            foreach (var endPoint in proxyServer.ProxyEndPoints)
                Console.WriteLine("Listening on '{0}' endpoint at Ip {1} and port: {2} ",
                    endPoint.GetType().Name, endPoint.IpAddress, endPoint.Port);

            // Only explicit proxies can be set as system proxy!
            proxyServer.SetAsSystemHttpProxy(explicitEndPoint);
            proxyServer.SetAsSystemHttpsProxy(explicitEndPoint);

            // wait here (You can use something else as a wait function, I am using this as a demo)
            Console.Read();

            // Unsubscribe & Quit
            explicitEndPoint.BeforeTunnelConnectRequest -= OnBeforeTunnelConnectRequest;
            proxyServer.BeforeRequest -= OnRequest;
            proxyServer.BeforeResponse -= OnResponse;
            proxyServer.ServerCertificateValidationCallback -= OnCertificateValidation;
            proxyServer.ClientCertificateSelectionCallback -= OnCertificateSelection;

            proxyServer.Stop();


        }
        private async Task OnBeforeTunnelConnectRequest(object sender, TunnelConnectSessionEventArgs e)
        {
            string hostname = e.HttpClient.Request.RequestUri.Host;

            if (hostname.Contains("dropbox.com"))
            {
                // Exclude Https addresses you don't want to proxy
                // Useful for clients that use certificate pinning
                // for example dropbox.com
                e.DecryptSsl = false;
            }
        }

        public async Task OnRequest(object sender, SessionEventArgs e)
        {
            //Console.WriteLine(e.HttpClient.Request.Url);

            // read request headers
            var requestHeaders = e.HttpClient.Request.Headers;

            var method = e.HttpClient.Request.Method.ToUpper();
            if ((method == "POST" || method == "PUT" || method == "PATCH"))
            {
                // Get/Set request body bytes
                byte[] bodyBytes = await e.GetRequestBody();
                e.SetRequestBody(bodyBytes);

                // Get/Set request body as string
                string bodyString = await e.GetRequestBodyAsString();
                e.SetRequestBodyString(bodyString);

                // store request 
                // so that you can find it from response handler 
                e.UserData = e.HttpClient.Request;
            }

            // To cancel a request with a custom HTML content
            // Filter URL
            
            else if (!this.isSafe(e.HttpClient.Request.RequestUri.Host))
            {
                Console.WriteLine(e.HttpClient.Request.RequestUri.Host);
                this.feedServer.send(e.HttpClient.Request.RequestUri.Host);
                e.Ok("<!DOCTYPE html>\r\n"
                + "<html>\r\n"
                + "  <head>\r\n"
                + "    <style>\r\n"
                + "       body {\r\n"
                + "        background-image: url(\"http://localhost:8525/brick.png\");\r\n"
                + "        background-repeat: repeat;\r\n"
                + "      }\r\n"
                + "\r\n"
                + "\r\n"
                + "      \r\n"
                + "      .center {\r\n"
                + "        display: flex;\r\n"
                + "        justify-content: center;\r\n"
                + "        align-items: center;\r\n"
                + "        height: 100vh;\r\n"
                + "      }\r\n"
                + "\r\n"
                + "    </style>\r\n"
                + "  </head>\r\n"
                + "  <body>\r\n"
                + "    <div class=\"center\">\r\n"
                + "      <img src=\"http://localhost:8525/sanic.gif\" alt=\"Animated GIF\">\r\n"
                + "    </div>\r\n"
                + "  </body>\r\n"
                + "</html>");
            }

            // Redirect example

        }

        // Modify response
        public async Task OnResponse(object sender, SessionEventArgs e)
        {
            // read response headers
            var responseHeaders = e.HttpClient.Response.Headers;

            //if (!e.ProxySession.Request.Host.Equals("medeczane.sgk.gov.tr")) return;
            if (e.HttpClient.Request.Method == "GET" || e.HttpClient.Request.Method == "POST")
            {
                if (e.HttpClient.Response.StatusCode == 200)
                {
                    if (e.HttpClient.Response.ContentType != null && e.HttpClient.Response.ContentType.Trim().ToLower().Contains("text/html"))
                    {
                        byte[] bodyBytes = await e.GetResponseBody();
                        e.SetResponseBody(bodyBytes);

                        string body = await e.GetResponseBodyAsString();
                        e.SetResponseBodyString(body);
                    }
                }
            }

            if (e.UserData != null)
            {
                // access request from UserData property where we stored it in RequestHandler
                var request = (Request)e.UserData;
            }
        }

        // Allows overriding default certificate validation logic
        public Task OnCertificateValidation(object sender, CertificateValidationEventArgs e)
        {
            // set IsValid to true/false based on Certificate Errors
            if (e.SslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
                e.IsValid = true;

            return Task.CompletedTask;
        }

        // Allows overriding default client certificate selection logic during mutual authentication
        public Task OnCertificateSelection(object sender, CertificateSelectionEventArgs e)
        {
            // set e.clientCertificate to override
            return Task.CompletedTask;
        }
    }
}
