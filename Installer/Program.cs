using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NoponetInstaller
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
            Installer installer = new Installer();
            Application.Run(new Form1(installer));
        }
    }
    public class Installer
    {
        private Form1 frame;

        public void setFrame(Form1 frame)
        {
            this.frame = frame;
        }
        public void run(String user, bool javaChecked, bool pythonChecked, bool libraryChecked, bool jailbreakChecked)
        {
            bool successful = disable_proxy(user);
            if (successful)
            {
                installStartup(user);
                if (libraryChecked)
                        libraryComputerMode(user);
                if (downloadFiles())
                {
                    


                    if (javaChecked)
                        disableJava(user);

                    if (pythonChecked)
                        disablePython(user);

                    if (jailbreakChecked)
                        chromeJailbreak(user);
                    lockInstall(user);

                    shortcuts(user);
                    this.frame.setLabel("Keep the change you filthy animal.");

                }
                else
                {
                    frame.setError("Error downloading files.");
                }
            }
            else
            {
                frame.setError("Make sure user:" + user + " is logged out.\n" +
                    "If the user is logged out, restart your computer.");
            }

                

        }
        public bool disable_proxy(string user)
        {
            this.frame.getProgressBar().Value = 0;
            int increment = 100 / 7;
            this.frame.getProgressBar().Increment(increment);


            Process process = new Process();
            process.StartInfo.FileName = "reg";
            process.StartInfo.Arguments = "load HKU\\" + user + " C:\\Users\\" + user + "\\ntuser.dat";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            string response = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            process.Dispose();
            this.frame.getProgressBar().Increment(increment);

            if (response.Contains("success"))
            {
                Console.WriteLine(response);
                string[] commands = new string[] {
    "REG ADD \"HKEY_USERS\\" + user + "\\Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\" /v \"ProxyServer\" /t REG_SZ /d \"127.0.0.1:8000\" /f",
    "REG ADD \"HKEY_USERS\\" + user + "\\Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\" /v ProxyEnable /t REG_DWORD /d 1 /f",
    "REG ADD \"HKEY_USERS\\" + user + "\\SOFTWARE\\Policies\\Microsoft\\Internet Explorer\"",
    "REG ADD \"HKEY_USERS\\" + user + "\\SOFTWARE\\Policies\\Microsoft\\Internet Explorer\\Control Panel\"",
    "REG ADD \"HKEY_USERS\\" + user + "\\SOFTWARE\\Policies\\Microsoft\\Internet Explorer\\Control Panel\" /v Proxy /t REG_DWORD /d 1"
};

                foreach (string command in commands)
                {
                    process = new Process();
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = "/C " + command;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                    this.frame.getProgressBar().Increment(increment);
                }
                return true;

            }

            return false;
        }

        public void installStartup(string user)
        {
            string hostname = Environment.MachineName;
            string username = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            username = username.Substring(username.IndexOf("\\") + 1);

            String xmlContents = "<?xml version=\"1.0\" encoding=\"UTF-16\"?>\r\n"
                    + "<Task version=\"1.2\" xmlns=\"http://schemas.microsoft.com/windows/2004/02/mit/task\">\r\n"
                    + "  <RegistrationInfo>\r\n"
                    + "    <Date>2023-01-28T15:18:23</Date>\r\n"
                    + "    <Author>" + hostname + "\\" + username + "</Author>\r\n"
                    + "    <Description>runs nopo</Description>\r\n"
                    + "    <URI>\\NoPoLauncher_" + user + "</URI>\r\n"
                    + "  </RegistrationInfo>\r\n"
                    + "  <Triggers>\r\n"
                    + "    <LogonTrigger>\r\n"
                    + "      <StartBoundary>2023-01-28T15:18:00</StartBoundary>\r\n"
                    + "      <Enabled>true</Enabled>\r\n"
                    + "      <UserId>" + hostname + "\\" + user + "</UserId>\r\n"
                    + "    </LogonTrigger>\r\n"
                    + "  </Triggers>\r\n"
                    + "  <Principals>\r\n"
                    + "    <Principal id=\"Author\">\r\n"
                    + "      <RunLevel>HighestAvailable</RunLevel>\r\n"
                    + "      <UserId>S-1-5-18</UserId>\r\n"
                    + "      <LogonType>InteractiveToken</LogonType>\r\n"
                    + "    </Principal>\r\n"
                    + "  </Principals>\r\n"
                    + "  <Settings>\r\n"
                    + "    <MultipleInstancesPolicy>IgnoreNew</MultipleInstancesPolicy>\r\n"
                    + "    <DisallowStartIfOnBatteries>false</DisallowStartIfOnBatteries>\r\n"
                    + "    <StopIfGoingOnBatteries>false</StopIfGoingOnBatteries>\r\n"
                    + "    <AllowHardTerminate>true</AllowHardTerminate>\r\n"
                    + "    <StartWhenAvailable>false</StartWhenAvailable>\r\n"
                    + "    <RunOnlyIfNetworkAvailable>false</RunOnlyIfNetworkAvailable>\r\n"
                    + "    <IdleSettings>\r\n"
                    + "      <StopOnIdleEnd>true</StopOnIdleEnd>\r\n"
                    + "      <RestartOnIdle>false</RestartOnIdle>\r\n"
                    + "    </IdleSettings>\r\n"
                    + "    <AllowStartOnDemand>true</AllowStartOnDemand>\r\n"
                    + "    <Enabled>true</Enabled>\r\n"
                    + "    <Hidden>false</Hidden>\r\n"
                    + "    <RunOnlyIfIdle>false</RunOnlyIfIdle>\r\n"
                    + "    <WakeToRun>false</WakeToRun>\r\n"
                    + "    <ExecutionTimeLimit>P3D</ExecutionTimeLimit>\r\n"
                    + "    <Priority>7</Priority>\r\n"
                    + "  </Settings>\r\n"
                    + "  <Actions Context=\"Author\">\r\n"
                    + "    <Exec>\r\n"
                    + "      <Command>C:\\PROGRA~2\\noponet\\Proxy.exe</Command>\r\n"
                    + "      <Arguments></Arguments>\r\n"
                    + "    </Exec>\r\n"
                    + "  </Actions>\r\n"
                    + "</Task>";

            StreamWriter taskFileWriter = new StreamWriter(File.Create(@"C:/Windows/System32/Tasks/NoPoLauncher_" + user));
            taskFileWriter.Write(xmlContents);
            taskFileWriter.Flush();
            taskFileWriter.Close();

            Process process = new Process();
            process.StartInfo.FileName = "schtasks";
            process.StartInfo.Arguments = "/create /xml \"C:\\Windows\\System32\\Tasks\\NoPoLauncher_" + user + "\" /tn \"NoPoLauncher_" + user + "\"";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            string response = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            process.Dispose();
            Console.WriteLine(response);

            //""
            process = new Process();
            process.StartInfo.FileName = "cmd";
            process.StartInfo.Arguments = "/C Reagentc /disable";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
            process.Dispose();
        }
        public bool downloadFiles()
        {
            try
            {
                WebClient client = new WebClient();
                string pagedata = client.DownloadString("https://github.com/nopolifelock/noponet-windows/releases/download/v1.2/manifest");
                Console.WriteLine(pagedata);
                string[] lines = pagedata.Split(new[] { "\n" }, StringSplitOptions.None);
                Console.WriteLine(lines.Length);
                // Define the file URL


                // Define the file path

                string directory = "C:\\Program Files (x86)\\noponet\\";
                // Create the directory if it does not exist
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                this.frame.getProgressBar().Value = 0;
                int increment = 100 / lines.Length;
                this.frame.setLabel("Downloading files...");
                for (int i = 0; i < lines.Length; i++)
                {
                    string file = lines[i];
                    string filePath = "C:\\Program Files (x86)\\noponet\\" + i;
                    string fileUrl = "https://github.com/nopolifelock/noponet-windows/releases/download/v1.2/" + file;
                    using (WebClient webClient = new WebClient())
                    {
                        Debug.WriteLine(filePath);
                        webClient.DownloadFile(fileUrl, filePath);
                        if (File.Exists("C:\\Program Files (x86)\\noponet\\" + file))
                        {
                            File.Delete("C:\\Program Files (x86)\\noponet\\" + file);
                        }

                        File.Move(filePath, "C:\\Program Files (x86)\\noponet\\" + file);
                        this.frame.getProgressBar().Increment(increment);
                    }

                }
                string path = @"C:\Program Files (x86)\noponet\lists";

                // Check if the directory exists, if not, create it
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                // Create the two files
                File.Create(Path.Combine(path, "whitelist.txt")).Close();
                File.Create(Path.Combine(path, "keywords.txt")).Close();

                //"C:\PROGRA~2\noponet\assetts"
                if (!Directory.Exists(@"C:\PROGRA~2\noponet\assets"))
                {
                    Directory.CreateDirectory(@"C:\PROGRA~2\noponet\assets");
                }
                Image brick = Resource1.brickwall;
                Image sanic = Resource1.sanic;

                brick.Save(@"C:\PROGRA~2\noponet\assets\brick.png");
                sanic.Save(@"C:\PROGRA~2\noponet\assets\sanic.gif");

            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }

        public void chromeJailbreak(string user)
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/C dir /s /b C:\\PROGRA~2\\msedge.exe " +
                                                        "C:\\PROGRA~2\\iexplore.exe " +
                                                        "C:\\PROGRA~2\\firefox.exe " +
                                                        "C:\\PROGRA~2\\Safari.exe " +
                                                        "C:\\PROGRA~2\\brave.exe " +
                                                        "C:\\PROGRA~2\\opera.exe " +
                                                        "C:\\PROGRA~1\\msedge.exe " +
                                                        "C:\\PROGRA~1\\iexplore.exe " +
                                                        "C:\\PROGRA~1\\firefox.exe " +
                                                        "C:\\PROGRA~1\\Safari.exe " +
                                                        "C:\\PROGRA~1\\brave.exe " +
                                                        "C:\\PROGRA~1\\opera.exe";

            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            this.frame.setLabel("Jailbreaking chrome...");
            this.frame.getProgressBar().Value = 0;
            while (!process.StandardOutput.EndOfStream)
            {
                string line = process.StandardOutput.ReadLine();
                if (line.Contains(":"))
                {

                    Console.WriteLine(line);
                    Process process2 = new Process();
                    process2.StartInfo.FileName = "cmd.exe";
                    process2.StartInfo.Arguments = "/C takeown /f " + "\"" + line + "\"";
                    process2.StartInfo.RedirectStandardOutput = true;
                    process2.StartInfo.UseShellExecute = false;
                    process2.StartInfo.CreateNoWindow = true;
                    process2.Start();
                    Console.WriteLine(process2.StandardOutput.ReadToEnd());
                    process2.WaitForExit();
                    process2.Dispose();

                    Console.WriteLine(line);
                    process2 = new Process();
                    process2.StartInfo.FileName = "cmd.exe";
                    process2.StartInfo.Arguments = "/C icacls \"" + line + "\" /deny " + user + ":(RX)";
                    process2.StartInfo.RedirectStandardOutput = true;
                    process2.StartInfo.UseShellExecute = false;
                    process2.StartInfo.CreateNoWindow = true;
                    process2.Start();
                    //Console.WriteLine(process2.StandardOutput.ReadToEnd());
                    process2.WaitForExit();
                    process2.Dispose();

                    if (this.frame.getProgressBar().Value >= 100)
                        this.frame.getProgressBar().Value = 0;
                    this.frame.getProgressBar().Increment(5);
                }
            }
            process.WaitForExit();
            process.Dispose();


            process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/C " + "reg add \"HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Google\\Chrome\\ExtensionInstallForcelist\" /v \"1\" /t REG_SZ /d \"cfhdojbkjhnklbpkdaibdccddilifddb\" /f";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
            process.Dispose();

            process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/C " + "reg add \"HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Google\\Chrome\\ExtensionInstallForcelist\" /v \"2\" /t REG_SZ /d \"kpkpealfgioplclbgbfiibgccjgjhmdd\" /f";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
            process.Dispose();

            process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/C " + "reg add \"HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Google\\Chrome\\ExtensionInstallBlocklist\" /v \"1\" /t REG_SZ /d \"*\" /f";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
            process.Dispose();

            process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/C REG ADD HKLM\\SOFTWARE\\Classes\\ChromeHTML\\shell\\open\\command /ve /d \"\\\"C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe\\\" -- \\\"%1\\\"\" /f";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
            process.Dispose();


        }
        public void disableJava(string user)
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/C dir /s /b C:\\PROGRA~2\\java.exe " +
                                                       "C:\\PROGRA~2\\javaw.exe " +
                                                       "C:\\PROGRA~2\\javac.exe " +

                                                       "C:\\PROGRA~1\\java.exe " +
                                                       "C:\\PROGRA~1\\javaw.exe " +
                                                       "C:\\PROGRA~1\\javac.exe";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            this.frame.setLabel("Disabling java...");
            while (!process.StandardOutput.EndOfStream)
            {
                string line = process.StandardOutput.ReadLine();
                if (line.Contains(":"))
                {

                    Console.WriteLine(line);
                    Process process2 = new Process();
                    process2.StartInfo.FileName = "cmd.exe";
                    process2.StartInfo.Arguments = "/C takeown /f " + "\"" + line + "\"";
                    process2.StartInfo.RedirectStandardOutput = true;
                    process2.StartInfo.UseShellExecute = false;
                    process2.StartInfo.CreateNoWindow = true;
                    process2.Start();
                    Console.WriteLine(process2.StandardOutput.ReadToEnd());
                    process2.WaitForExit();
                    process2.Dispose();

                    Console.WriteLine(line);
                    process2 = new Process();
                    process2.StartInfo.FileName = "cmd.exe";
                    process2.StartInfo.Arguments = "/C icacls \"" + line + "\" /deny " + user + ":(RX)";
                    process2.StartInfo.RedirectStandardOutput = true;
                    process2.StartInfo.UseShellExecute = false;
                    process2.StartInfo.CreateNoWindow = true;
                    process2.Start();
                    //Console.WriteLine(process2.StandardOutput.ReadToEnd());
                    process2.WaitForExit();
                    process2.Dispose();
                    if (this.frame.getProgressBar().Value >= 100)
                        this.frame.getProgressBar().Value = 0;
                    this.frame.getProgressBar().Increment(5);
                }
            }
            process.WaitForExit();
            process.Dispose();
        }

        public void libraryComputerMode(string user)
        {
            string hostname = Environment.MachineName;
            string username = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            username = username.Substring(username.IndexOf("\\") + 1);

            String xmlContents = "<?xml version=\"1.0\" encoding=\"UTF-16\"?>\r\n"
                    + "<Task version=\"1.2\" xmlns=\"http://schemas.microsoft.com/windows/2004/02/mit/task\">\r\n"
                    + "  <RegistrationInfo>\r\n"
                    + "    <Date>2023-01-28T15:18:23</Date>\r\n"
                    + "    <Author>" + hostname + "\\" + username + "</Author>\r\n"
                    + "    <Description>runs nopo firewall</Description>\r\n"
                    + "    <URI>\\NopoWallLauncher_" + user + "</URI>\r\n"
                    + "  </RegistrationInfo>\r\n"
                    + "  <Triggers>\r\n"
                    + "    <LogonTrigger>\r\n"
                    + "      <StartBoundary>2023-01-28T15:18:00</StartBoundary>\r\n"
                    + "      <Enabled>true</Enabled>\r\n"
                    + "      <UserId>" + hostname + "\\" + user + "</UserId>\r\n"
                    + "    </LogonTrigger>\r\n"
                    + "  </Triggers>\r\n"
                    + "  <Principals>\r\n"
                    + "    <Principal id=\"Author\">\r\n"
                    + "      <RunLevel>HighestAvailable</RunLevel>\r\n"
                    + "      <UserId>S-1-5-18</UserId>\r\n"
                    + "      <LogonType>InteractiveToken</LogonType>\r\n"
                    + "    </Principal>\r\n"
                    + "  </Principals>\r\n"
                    + "  <Settings>\r\n"
                    + "    <MultipleInstancesPolicy>IgnoreNew</MultipleInstancesPolicy>\r\n"
                    + "    <DisallowStartIfOnBatteries>false</DisallowStartIfOnBatteries>\r\n"
                    + "    <StopIfGoingOnBatteries>false</StopIfGoingOnBatteries>\r\n"
                    + "    <AllowHardTerminate>true</AllowHardTerminate>\r\n"
                    + "    <StartWhenAvailable>false</StartWhenAvailable>\r\n"
                    + "    <RunOnlyIfNetworkAvailable>false</RunOnlyIfNetworkAvailable>\r\n"
                    + "    <IdleSettings>\r\n"
                    + "      <StopOnIdleEnd>true</StopOnIdleEnd>\r\n"
                    + "      <RestartOnIdle>false</RestartOnIdle>\r\n"
                    + "    </IdleSettings>\r\n"
                    + "    <AllowStartOnDemand>true</AllowStartOnDemand>\r\n"
                    + "    <Enabled>true</Enabled>\r\n"
                    + "    <Hidden>false</Hidden>\r\n"
                    + "    <RunOnlyIfIdle>false</RunOnlyIfIdle>\r\n"
                    + "    <WakeToRun>false</WakeToRun>\r\n"
                    + "    <ExecutionTimeLimit>P3D</ExecutionTimeLimit>\r\n"
                    + "    <Priority>7</Priority>\r\n"
                    + "  </Settings>\r\n"
                    + "  <Actions Context=\"Author\">\r\n"
                    + "    <Exec>\r\n"
                    + "      <Command>C:\\PROGRA~2\\noponet\\NopoWall.exe</Command>\r\n"
                    + "      <Arguments></Arguments>\r\n"
                    + "    </Exec>\r\n"
                    + "  </Actions>\r\n"
                    + "</Task>";

            StreamWriter taskFileWriter = new StreamWriter(File.Create(@"C:/Windows/System32/Tasks/NopoWallLauncher_" + user));
            taskFileWriter.Write(xmlContents);
            taskFileWriter.Flush();
            taskFileWriter.Close();

            Process process = new Process();
            process.StartInfo.FileName = "schtasks";
            process.StartInfo.Arguments = "/create /xml \"C:\\Windows\\System32\\Tasks\\NopoWallLauncher_" + user + "\" /tn \"NopoWallLauncher_" + user + "\"";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            string response = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            process.Dispose();
            Console.WriteLine(response);


        }

        public void disablePython(string user)
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/C dir /s /b C:\\PROGRA~2\\python.exe " +
                                                       "C:\\PROGRA~2\\python2.exe " +
                                                       "C:\\PROGRA~2\\python3.exe " +

                                                       "C:\\PROGRA~1\\python.exe " +
                                                       "C:\\PROGRA~1\\python2.exe " +
                                                       "C:\\PROGRA~1\\python3.exe";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            this.frame.setLabel("Disabling python...");
            while (!process.StandardOutput.EndOfStream)
            {
                string line = process.StandardOutput.ReadLine();
                if (line.Contains(":"))
                {

                    Console.WriteLine(line);
                    Process process2 = new Process();
                    process2.StartInfo.FileName = "cmd.exe";
                    process2.StartInfo.Arguments = "/C takeown /f " + "\"" + line + "\"";
                    process2.StartInfo.RedirectStandardOutput = true;
                    process2.StartInfo.UseShellExecute = false;
                    process2.StartInfo.CreateNoWindow = true;
                    process2.Start();
                    Console.WriteLine(process2.StandardOutput.ReadToEnd());
                    process2.WaitForExit();
                    process2.Dispose();

                    Console.WriteLine(line);
                    process2 = new Process();
                    process2.StartInfo.FileName = "cmd.exe";
                    process2.StartInfo.Arguments = "/C icacls \"" + line + "\" /deny " + user + ":(RX)";
                    process2.StartInfo.RedirectStandardOutput = true;
                    process2.StartInfo.UseShellExecute = false;
                    process2.StartInfo.CreateNoWindow = true;
                    process2.Start();
                    //Console.WriteLine(process2.StandardOutput.ReadToEnd());
                    process2.WaitForExit();
                    process2.Dispose();
                    if (this.frame.getProgressBar().Value >= 100)
                        this.frame.getProgressBar().Value = 0;
                    this.frame.getProgressBar().Increment(5);
                }
            }
            process.WaitForExit();
            process.Dispose();
        }

        public void shortcuts(string user)
        {
            string consolePath = "C:\\PROGRA~2\\noponet\\AdminConsole.exe";
            string proxyPath = "C:\\PROGRA~2\\noponet\\Proxy.exe";
            string lockPath = "C:\\PROGRA~2\\noponet\\NopoLock.exe";
            makeShortcut(consolePath, "AdminConsole");
            makeShortcut(proxyPath, "NopoNet Server");
            makeShortcut(lockPath, "NopoLock");
            makeShortcut(lockPath, "NopoLock", user);
        }
        private void makeShortcut(string file, string namme)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\";

            Process process2 = new Process();
            process2.StartInfo.FileName = "cmd.exe";
            process2.StartInfo.Arguments = "/C mklink \"" + desktopPath + namme + "\" \"" + file + "\"";
            process2.StartInfo.RedirectStandardOutput = true;
            process2.StartInfo.UseShellExecute = false;
            process2.StartInfo.CreateNoWindow = true;
            process2.Start();
            Console.WriteLine(process2.StandardOutput.ReadToEnd());
            process2.WaitForExit();
            process2.Dispose();
        }
        private void makeShortcut(string file, string name, string user)
        {
            string desktopPath = GetDesktopPath(user) + "\\";
            Debug.WriteLine(desktopPath);
            Process process2 = new Process();
            process2.StartInfo.FileName = "cmd.exe";
            process2.StartInfo.Arguments = "/C mklink \"" + desktopPath + name + "\" \"" + file + "\"";
            process2.StartInfo.RedirectStandardOutput = true;
            process2.StartInfo.UseShellExecute = false;
            process2.StartInfo.CreateNoWindow = true;
            process2.Start();
            Console.WriteLine(process2.StandardOutput.ReadToEnd());
            process2.WaitForExit();
            process2.Dispose();
        }

        private void lockInstall(string user)
        {
            string hostname = Environment.MachineName;
            string username = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            username = username.Substring(username.IndexOf("\\") + 1);

            String xmlContents = "<?xml version=\"1.0\" encoding=\"UTF-16\"?>\r\n"
                    + "<Task version=\"1.2\" xmlns=\"http://schemas.microsoft.com/windows/2004/02/mit/task\">\r\n"
                    + "  <RegistrationInfo>\r\n"
                    + "    <Date>2023-01-28T15:18:23</Date>\r\n"
                    + "    <Author>" + hostname + "\\" + username + "</Author>\r\n"
                    + "    <Description>runs nopolock server</Description>\r\n"
                    + "    <URI>\\NopoLockLauncher_" + user + "</URI>\r\n"
                    + "  </RegistrationInfo>\r\n"
                    + "  <Triggers>\r\n"
                    + "    <LogonTrigger>\r\n"
                    + "      <StartBoundary>2023-01-28T15:18:00</StartBoundary>\r\n"
                    + "      <Enabled>true</Enabled>\r\n"
                    + "      <UserId>" + hostname + "\\" + user + "</UserId>\r\n"
                    + "    </LogonTrigger>\r\n"
                    + "  </Triggers>\r\n"
                    + "  <Principals>\r\n"
                    + "    <Principal id=\"Author\">\r\n"
                    + "      <RunLevel>HighestAvailable</RunLevel>\r\n"
                    + "      <UserId>S-1-5-18</UserId>\r\n"
                    + "      <LogonType>InteractiveToken</LogonType>\r\n"
                    + "    </Principal>\r\n"
                    + "  </Principals>\r\n"
                    + "  <Settings>\r\n"
                    + "    <MultipleInstancesPolicy>IgnoreNew</MultipleInstancesPolicy>\r\n"
                    + "    <DisallowStartIfOnBatteries>false</DisallowStartIfOnBatteries>\r\n"
                    + "    <StopIfGoingOnBatteries>false</StopIfGoingOnBatteries>\r\n"
                    + "    <AllowHardTerminate>true</AllowHardTerminate>\r\n"
                    + "    <StartWhenAvailable>false</StartWhenAvailable>\r\n"
                    + "    <RunOnlyIfNetworkAvailable>false</RunOnlyIfNetworkAvailable>\r\n"
                    + "    <IdleSettings>\r\n"
                    + "      <StopOnIdleEnd>true</StopOnIdleEnd>\r\n"
                    + "      <RestartOnIdle>false</RestartOnIdle>\r\n"
                    + "    </IdleSettings>\r\n"
                    + "    <AllowStartOnDemand>true</AllowStartOnDemand>\r\n"
                    + "    <Enabled>true</Enabled>\r\n"
                    + "    <Hidden>false</Hidden>\r\n"
                    + "    <RunOnlyIfIdle>false</RunOnlyIfIdle>\r\n"
                    + "    <WakeToRun>false</WakeToRun>\r\n"
                    + "    <ExecutionTimeLimit>P3D</ExecutionTimeLimit>\r\n"
                    + "    <Priority>7</Priority>\r\n"
                    + "  </Settings>\r\n"
                    + "  <Actions Context=\"Author\">\r\n"
                    + "    <Exec>\r\n"
                    + "      <Command>C:\\PROGRA~2\\noponet\\LockServer.exe</Command>\r\n"
                    + "      <Arguments></Arguments>\r\n"
                    + "    </Exec>\r\n"
                    + "  </Actions>\r\n"
                    + "</Task>";

            StreamWriter taskFileWriter = new StreamWriter(File.Create(@"C:/Windows/System32/Tasks/NopoLockLauncher_" + user));
            taskFileWriter.Write(xmlContents);
            taskFileWriter.Flush();
            taskFileWriter.Close();

            string filesDir = "C:\\PROGRA~2\\noponet\\files";
            if (!Directory.Exists(filesDir))
            {
                Directory.CreateDirectory(filesDir);
            }
            Process process = new Process();
            process.StartInfo.FileName = "schtasks";
            process.StartInfo.Arguments = "/create /xml \"C:\\Windows\\System32\\Tasks\\NopoLockLauncher_" + user + "\" /tn \"NopoLockLauncher_" + user + "\"";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            string response = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            process.Dispose();
            Console.WriteLine(response);


            process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/C icacls \"" + filesDir + "\" /deny " + user + ":(RX)";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            //Console.WriteLine(process2.StandardOutput.ReadToEnd());
            process.WaitForExit();
            process.Dispose();

        }

        public static string GetDesktopPath(string username)
        {
            string userProfilePath = string.Format(@"C:\Users\{0}", username);
            DirectoryInfo userProfileDirectory = new DirectoryInfo(userProfilePath);
            DirectoryInfo desktopDirectory = new DirectoryInfo(Path.Combine(userProfileDirectory.FullName, "Desktop"));
            return desktopDirectory.FullName;
        }



    }


}

