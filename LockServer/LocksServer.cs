using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LockServer
{
    class LockServer
    {
        private Socket serverSocket;
        private bool running;
        public static String FILES_DIR = "C:\\Program Files (x86)\\noponet\\files\\";
	    public static String DIR = "C:\\Program Files (x86)\\noponet";
	    public static String TIME_SERVER = "https://currentmillis.com/time/minutes-since-unix-epoch.php";
        private Dictionary<string, int> config = new Dictionary<string, int>();
        static void Main(string[] args)
        {
            LockServer server = new LockServer();
            new Thread(server.run).Start();

        }
        public LockServer()
        {
            LoadConfig();
        }
        public void run()
        {
            running = true;
            // Create an endpoint to listen on
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 6778);

            // Create a TCP socket to listen for incoming connections
            this.serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the endpoint
            serverSocket.Bind(endpoint);

            // Start listening for incoming connections
            serverSocket.Listen(5);

            Console.WriteLine("Server is running and listening for incoming connections...");

            while (running)
            {
                // Accept an incoming connection
                Socket clientSocket = serverSocket.Accept();
                new Thread(new Connection(this, clientSocket).run).Start();
            }

        }

        private void LoadConfig()
        {
            string configFile = Path.Combine(DIR, "config");

            if (File.Exists(configFile))
            {
                try
                {
                    using (StreamReader reader = new StreamReader(configFile))
                    {
                        string line;
                        string[] parameters;

                        while (!reader.EndOfStream)
                        {
                            line = reader.ReadLine();
                            parameters = line.Split('>');
                            config.Add(parameters[0], int.Parse(parameters[1]));
                        }
                        reader.Close();
                    }
                }
                catch (FileNotFoundException e)
                {
                    // TODO: Handle the FileNotFoundException
                    Console.WriteLine(e);
                }
                catch (IOException e)
                {
                    // TODO: Handle the IOException
                    Console.WriteLine(e);
                }
            }
            else
            {
                try
                {
                    File.Create(configFile);
                }
                catch (IOException e)
                {
                    // TODO: Handle the IOException
                    Console.WriteLine(e);
                }
            }
        }
        private void WriteConfig(string file, int time)
        {
            string configFile = Path.Combine(DIR, "config");

            try
            {
                StreamWriter writer = new StreamWriter(configFile);
                
                    config[file] = time;

                    foreach (string f in config.Keys)
                    {
                        writer.WriteLine(f + ">" + config[f].ToString());
                    }

                    config.Clear();
                    
                    writer.Close();
                LoadConfig();
            }
            
            catch (IOException e)
            {
                // TODO: Handle the IOException
                Console.WriteLine(e);
            }
        }

        public string SetTime(string fileName, int time)
        {
            lock (this)
            {
                if (config.ContainsKey(fileName))
                {
                    if (CurrentTimeMinutes() >= config[fileName])
                    {
                        WriteConfig(fileName, time);
                        return "ok";
                    }
                    else
                    {
                        return "I can't let you do that, time difference is " + (config[fileName] - CurrentTimeMinutes()).ToString() + " minutes.";
                    }
                }
                else
                {
                    WriteConfig(fileName, time);
                    return "ok";
                }
            }
        }

        public int CurrentTimeMinutes()
        {
            try
            {
                WebRequest request = WebRequest.Create(TIME_SERVER);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();


                StreamReader reader = new StreamReader(response.GetResponseStream());
                int time = int.Parse(reader.ReadLine());
                reader.Close();
                response.Close();
                return time;
            }
            catch (WebException e)
            {
                // TODO: Handle the WebException
                Console.WriteLine(e);
            }
            return 0;
        }

        public Dictionary<string, int> GetConfig()
        {
            return config;
        }


    }
}
