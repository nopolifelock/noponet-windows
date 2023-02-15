using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LockServer
{

    class Connection
    {

        private LockServer server;
        private Socket socket;
        private StreamReader reader;
        private StreamWriter writer;
        private bool running;

		public Connection(LockServer server, Socket socket)
		{
			this.server = server;
			this.socket = socket;

			try
			{
				NetworkStream stream = new NetworkStream(socket);
				this.reader = new StreamReader(stream);
				this.writer = new StreamWriter(stream);

			}
			catch (IOException e)
			{
				Console.WriteLine(e.Message);
			}
		}
		public void run()
		{
			// TODO Auto-generated method stub
			running = true;
			while (running)
			{

				try
				{
					String request = reader.ReadLine();
					Handle(request);
				}
				catch (IOException e)
				{

                    Console.WriteLine("Connection closed");
					this.Close();
				}
			}
		}
        private void Handle(string request)
        {
            Console.WriteLine(request);
            if (request != null)
            {
                string[] parameters = request.Split('<');

                switch (parameters[0])
                {

                    case "CHECK":
                        if (server.GetConfig().ContainsKey(parameters[1]))
                        {
                            Console.WriteLine(parameters[1]);
                            int timeLeft = server.GetConfig()[parameters[1]] - server.CurrentTimeMinutes();
                            if (timeLeft < 0)
                                timeLeft = 0;
                            Send("RESPONSE", CheckString(timeLeft));
                        }
                        else
                        {
                            Send("RESPONSE", "Invalid file name");
                        }
                        break;

                    case "LIST":
                        string list = "";
                        foreach (string file2 in Directory.GetFiles(LockServer.FILES_DIR))
                        {
                            list += Path.GetFileName(file2) + ">";
                        }
                        Send("LIST", list);
                        break;

                    case "SET":
                        string file = parameters[1];

                        int time = int.Parse(parameters[2]);
                        string response = server.SetTime(file, time + server.CurrentTimeMinutes());
                        Send("RESPONSE", response);
                        break;

                    case "PUSH":
                        string inputFile = parameters[1];
                        try
                        {
                            File.Copy(inputFile, LockServer.FILES_DIR + Path.GetFileName(inputFile), true);
                            string list2 = "";
                            foreach (string file2 in Directory.GetFiles(LockServer.FILES_DIR))
                            {
                                list2 += Path.GetFileName(file2) + ">";
                            }
                            Send("LIST", list2);
                        }
                        catch (IOException e)
                        {
                            Send("RESPONSE", "error copying file");
                            Console.WriteLine(e);
                        }
                        break;

                    case "CLONE":
                        string fileToCopy = LockServer.FILES_DIR + parameters[1];

                        if ((!server.GetConfig().ContainsKey(parameters[1])) || (server.CurrentTimeMinutes() - server.GetConfig()[parameters[1]]) >= 0)
                        {
                            try
                            {
                                if (File.Exists(fileToCopy) && Directory.Exists(parameters[2]))
                                {

                                    File.Copy(fileToCopy, parameters[2] + "/" + parameters[1], true);
                                    Send("RESPONSE", "ok");
                                }
                                else
                                {
                                    Send("RESPONSE", "error copying file");
                                }
                            }
                            catch (IOException e)
                            {
                                Send("RESPONSE", "error copying file");
                                Console.WriteLine(e);
                            }
                        }
                        else
                        {
                            Send("RESPONSE", "I can't let you do that, " + CheckString(server.GetConfig()[parameters[1]] - server.CurrentTimeMinutes()) + " left.");
                        }
                        break;
                }
            }
        }
        private string CheckString(int remainder)
        {
            int hoursLeft = remainder / 60;
            int daysLeft = hoursLeft / 24;

            int remainder_hours = hoursLeft - daysLeft * 24;
            int remainder_minutes = remainder - (daysLeft * 24 * 60 + remainder_hours * 60);
            return (daysLeft + " days " + remainder_hours + " hours " + remainder_minutes + " minutes");
        }

        private void Send(params string[] parameters)
        {
            string response = string.Join("<", parameters);
            writer.WriteLine(response);
            writer.Flush();
        }

        public void Close()
        {
            try
            {
                this.socket.Close();
                this.reader.Close();
                this.writer.Close();
                running = false;
            }
            catch (IOException e)
            {
                // TODO: Handle the IOException
                Console.WriteLine(e);
            }
        }


    }
}
