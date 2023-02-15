using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Proxy
{
    class LocalWeb
    {
        public void run()
        {
            var listener = new TcpListener(IPAddress.Any, 8525);
            listener.Start();

            //Console.WriteLine("Listening for incoming connections...");

            while (true)
            {
                var client = listener.AcceptTcpClient();
                //Console.WriteLine("Accepted connection from " + client.Client.RemoteEndPoint);

                var stream = client.GetStream();
                var buffer = new byte[4096];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                var request = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                //Console.WriteLine("Received request: " + request);

                if (request.StartsWith("GET /sanic.gif"))
                {
                    var fileBytes = File.ReadAllBytes(@"C:\PROGRA~2\noponet\assetts\sanic.gif");
                    var response = Encoding.ASCII.GetBytes("HTTP/1.1 200 OK\r\n" +
                        "Content-Type: image/gif\r\n" +
                        "Content-Length: " + fileBytes.Length + "\r\n" +
                        "\r\n");
                    stream.Write(response, 0, response.Length);
                    stream.Write(fileBytes, 0, fileBytes.Length);
                }
                else if (request.StartsWith("GET /brick.png"))
                {
                    var fileBytes = File.ReadAllBytes(@"C:\PROGRA~2\noponet\assetts\brick.png");
                    var response = Encoding.ASCII.GetBytes("HTTP/1.1 200 OK\r\n" +
                        "Content-Type: image/png\r\n" +
                        "Content-Length: " + fileBytes.Length + "\r\n" +
                        "\r\n");
                    stream.Write(response, 0, response.Length);
                    stream.Write(fileBytes, 0, fileBytes.Length);
                }
                else
                {
                    var response = Encoding.ASCII.GetBytes("HTTP/1.1 404 Not Found\r\n" +
                        "\r\n" +
                        "File not found.");
                    stream.Write(response, 0, response.Length);
                }

                stream.Close();
                client.Close();
            }
        }
    }
    }

