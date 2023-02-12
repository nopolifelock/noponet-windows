using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdminConsole
{

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 frame = new Form1();
            AdminConsole console = new AdminConsole(frame);
            new Thread(console.run).Start();
            Application.Run(frame);



        }
    }

    public class AdminConsole
    {
        public static string WHITELIST_DIR = "C:\\Program Files (x86)\\noponet\\lists\\whitelist.txt";
        public static string KEYWORDS_DIR = "C:\\Program Files (x86)\\noponet\\lists\\keywords.txt";
        private Socket socket;
        private StreamReader reader;
        private StreamWriter writer;
        private Form1 frame;
        private bool running = true;
        public AdminConsole(Form1 frame)
        {
            this.frame = frame;

        }

        public void run()
        {
            this.FileCheck(WHITELIST_DIR);
            this.FileCheck(KEYWORDS_DIR);
            this.frame.setAdminConsole(this);
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect("127.0.0.1", 13371);
                NetworkStream stream = new NetworkStream(socket);
                this.reader = new StreamReader(stream);
                this.writer = new StreamWriter(stream);
            }catch(Exception e)
            {
                frame.errorMessage("error connecting to local server");
            }
            if(reader != null)
            while (running)
            {

                    try
                    {
                        string website = reader.ReadLine();
                        frame.addToList(website);
                        
                    }catch(Exception e)
                    {
                        frame.errorMessage("server disconnect");
                    }
            }

        }
        public void send(String data)
        {
            try
            {
                this.writer.WriteLine(data);
                this.writer.Flush();
            }catch(Exception e)
            {
                frame.errorMessage("error sending message");
            }
        }

        public void updateList(string[] elements, string url)
        {
            HashSet<string> set = new HashSet<string>();

            if (File.Exists(url))
            {
                using (StreamReader reader = new StreamReader(url))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        set.Add(line);
                    }
                }
            }

            elements = elements.Distinct().ToArray();
            using (StreamWriter writer = new StreamWriter(new FileStream(url, FileMode.Append)))
            {
                foreach (string element in elements)
                {
                    if (!set.Contains(element))
                    {
                        writer.WriteLine(element);
                        set.Add(element);
                    }
                }
            }
        }
        public void FileCheck(string fileString)
        {
            if (!File.Exists(fileString))
            {
                using (FileStream fs = File.Create(fileString))
                {
                    Console.WriteLine("File created successfully at: " + fileString);
                }
            }
            else
            {
                Console.WriteLine("File already exists at: " + fileString);
            }
        }

    }
}
/*
 * using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Proxy
{
    class FeedServer
    {
        private bool running;
        private Socket serverSocket;
        private Noposerver noposerver;
        private List<FeedConnection> connections = new List<FeedConnection>();
        public FeedServer(Noposerver server)
        {
            this.noposerver = server;
        }
        public void run()
        {
            running = true;
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 13371);
            this.serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.serverSocket.Bind(endpoint);
            this.serverSocket.Listen(10);
            while (running)
            {
                Socket accepted = serverSocket.Accept();
                FeedConnection con = new FeedConnection(this, accepted);
                connections.Add(con);
                new Thread(con.run).Start();

            }


        }
        public Noposerver getNopoServer()
        {
            return this.noposerver;
        }

        public void send(string message)
        {
            foreach(FeedConnection con in connections)
            {
                con.send(message);
            }
        }

    }
    class FeedConnection
    {
        private Socket socket;
        private FeedServer server;
        private StreamReader reader;
        private StreamWriter writer;
        private Boolean running;
        public FeedConnection(FeedServer server, Socket sock)
        {
            this.socket = sock;
            NetworkStream stream = new NetworkStream(sock);
            this.reader = new StreamReader(stream);
            this.writer = new StreamWriter(stream);
            this.server = server;
        }
        public void run()
        {
            running = true;

            while (running)
            {
                try
                {
                    String request = reader.ReadLine();
                    handle(request);
                }catch(System.IO.IOException e)
                {
                    this.close();
                }
            }

        }
        private void handle(String request)
        {
            Console.WriteLine(request);
            switch (request)
            {

                case "refresh":
                    this.server.getNopoServer().refresh();
                    break;
            }

        }

        public void send(String message)
        {
            try
            {
                writer.WriteLine(message);
                writer.Flush();
            }
            catch (System.IO.IOException e)
            {
                this.close();
            }
        }
        public void close() {
            this.socket.Close();
            this.reader.Close();
            this.writer.Close();
            this.running = false;


    }

}
 
}
*/