using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopoWall
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Security.Principal;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    namespace Proxy
    {
        class FireWall
        {

            private bool running;
            private String[] whitelistArray =
            {"C:\\Program Files (x86)",
            "C:\\Program Files",
            "C:\\Windows",
            "C:\\PerfLogs",
            "C:\\PROGRA~2",
            "C:\\PROGRA~1"};


            static void Main(string[] args)
            {
                FireWall wall = new FireWall();
                wall.run();
                Console.ReadLine();
            }
            static string[] GetAdmins()
            {
                Process process = new Process();
                process.StartInfo.FileName = "net";
                process.StartInfo.Arguments = "localgroup Administrators";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();

                StreamReader reader = process.StandardOutput;
                String line;
                bool readingUsers = false;
                List<string> admins = new List<string>();
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();


                    if (line.Equals("The command completed successfully."))
                    {
                        readingUsers = false;
                        break;
                    }

                    
                    if (readingUsers)
                    {
                        admins.Add(line);
                    }
                    if (line.Equals("Administrator"))
                    {
                        readingUsers = true;
                    }
                    

                }
                return admins.ToArray();
            }
            public void run()
            {

                running = true;



                string[] admins = GetAdmins();
                string[] admin_dirs = new string[admins.Length];
                for (int i = 0; i < admins.Length; i++)
                {
                    admin_dirs[i] = "C:\\Users\\" + admins[i];
                }
                this.whitelistArray = this.whitelistArray.Concat(admin_dirs).ToArray();

                List<int> whitelistedPids = new List<int>();
                while (running)
                {
                    List<string> rules = new List<string>();
                    List<string> blocked = new List<String>();
                    Process process = new Process();
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = "/c netsh advfirewall firewall show rule name=all dir=out | find \"Block\"";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();

                    StreamReader reader = process.StandardOutput;
                    String line;

                    while (!reader.EndOfStream)
                    {

                        line = reader.ReadLine();
                        if (line.Contains("Rule Name:"))
                        {
                            int startIndex = line.IndexOf("Block ") + 6;
                            int endIndex = line.IndexOf(" Outbound Connections");
                            string path = line.Substring(startIndex, endIndex - startIndex);

                            rules.Add(path);
                        }
                    }

                    process.Dispose();


                    process = new Process();
                    process.StartInfo.FileName = "netstat";
                    process.StartInfo.Arguments = "-ano";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();

                    reader = process.StandardOutput;

                    Process process2;
                    while (!reader.EndOfStream)
                    {
                        line = reader.ReadLine();
                        string[] parms = System.Text.RegularExpressions.Regex.Split(line, @"\s{2,}");
                        string pidString = parms[parms.Length - 1];

                        int pid;
                        bool success = int.TryParse(pidString, out pid);
                        if (success)
                        {
                            if (!whitelistedPids.Contains(pid))
                                try
                                {
                                    process2 = Process.GetProcessById(pid);
                                    string exePath = process2.MainModule.FileName;
                                    Console.Out.WriteLine(exePath);
                                    if (!rules.Contains(exePath) && !blocked.Contains(exePath))
                                    {
                                        bool whiteListed = false;

                                        foreach (string white in whitelistArray)
                                        {
                                            if (exePath.StartsWith(white))
                                            {
                                                //Console.WriteLine(programPath);
                                                whiteListed = true;
                                            }
                                        }

                                        if (!whiteListed)
                                        {
                                            blocked.Add(exePath);

                                        }

                                    }
                                    process2.Dispose();
                                    if (!whitelistedPids.Contains(pid))
                                        whitelistedPids.Add(pid);
                                }
                                catch (System.ComponentModel.Win32Exception ex)
                                {
                                    //Console.WriteLine(ex.Message);
                                }
                        }

                    }

                    process.Dispose();

                    foreach (String path in blocked)
                    {
                        process = new Process();
                        Console.WriteLine(path + " added to firewall");
                        process.StartInfo.FileName = "netsh";
                        process.StartInfo.Arguments = "advfirewall firewall add rule name=\"Block " + path + " Outbound Connections\" dir=out program=\"" + path + "\" action=block";
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.RedirectStandardOutput = true;
                        process.StartInfo.CreateNoWindow = true;
                        process.Start();
                        process.WaitForExit();
                        process.Dispose();
                    }

                    Thread.Sleep(3000);
                }


            }
            public void stop()
            {
                this.running = false;
            }
        }

    }

}
