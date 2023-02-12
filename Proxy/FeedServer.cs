using System;
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
                try
                {
                    Socket accepted = serverSocket.Accept();
                    FeedConnection con = new FeedConnection(this, accepted);
                    connections.Add(con);
                    new Thread(con.run).Start();
                }catch(Exception e)
                {
                    Console.WriteLine("Error intitlizing server: " + e.Message);
                }

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
        public void removeConnection(FeedConnection con)
        {
            this.connections.Remove(con);
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
                Console.WriteLine("Error sending message to client.");
                this.close();
            }
        }
        public void close() {
            this.socket.Close();
            this.reader.Close();
            this.writer.Close();
            this.running = false;
            this.server.removeConnection(this);

    }

}
 
}
