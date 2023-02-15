using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NopoLock
{
    class Client : IDisposable
    {
        private Socket socket;
        private StreamWriter writer;
        private StreamReader reader;
        private bool running;
        private Form1 frame;


        public Client(Form1  frame)
        {
            this.frame = frame;
        }

        public void Run()
        {
            running = true;
            try
            {
                this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.socket.Connect("localhost", 6778);
                NetworkStream stream = new NetworkStream(this.socket);
                this.writer = new StreamWriter(stream);
                this.reader = new StreamReader(stream);
                string[] paramss = {"LIST"};
                SendRequest(paramss);

            }
            catch (Exception e1)
            {
                Console.Error.WriteLine(e1.Message);
            }

            while (running)
            {
                try
                {
                    if (reader != null)
                    {
                        string request = reader.ReadLine();
                        if (request != null)
                        {
                            Handle(request);
                        }
                        else
                        {
                            Application.Exit();
                            return;
                        }
                    }
                    else
                    {
                        Application.Exit();
                        return;
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e.Message);
                }
            }
        }

        private void Handle(string req)
        {
            string[] paramsArray = req.Split('<');
            switch (paramsArray[0])
            {
                case "LIST":
                    if (paramsArray.Length < 2)
                    {
                        Console.WriteLine("There are no files in the directory.");
                        return;
                    }
                    string[] files = paramsArray[1].Split('>');

                    Debug.WriteLine("got the list");
                    this.frame.listFiles(files);
                    break;
                case "RESPONSE":
                    frame.updateConsole(paramsArray[1]);
                    break;
            }
        }

        public void SendRequest(params string[] paramsArray)
        {
            string request = string.Join("<", paramsArray);
            writer.WriteLine(request);
            writer.Flush();
        }

        public void Dispose()
        {
            this.socket.Dispose();
            this.writer.Dispose();
            this.reader.Dispose();
            running = false;
        }
    }
}
