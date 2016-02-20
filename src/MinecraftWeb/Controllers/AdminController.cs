using System;
using Microsoft.AspNet.Mvc;
using System.Diagnostics;
using Microsoft.AspNet.Authorization;
using System.Threading.Tasks;
using System.Text;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MinecraftWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private static Process process;
        private static string consoleText = "";
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public void start()
        {
            if (process == null)
            {
                process = new Process();
                // Configure the process using the StartInfo properties.
                process.StartInfo.WorkingDirectory = "C:\\MCServer\\";
                process.StartInfo.FileName = "java.exe";
                process.StartInfo.Arguments = "-Xmx1024M -jar spigot-1.8.8.jar -o false";
                process.StartInfo.RedirectStandardInput = process.StartInfo.RedirectStandardError = process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                //configure console and events
                process.EnableRaisingEvents = true;
                process.ErrorDataReceived += new DataReceivedEventHandler(Process_ErrorDataReceived);
                process.OutputDataReceived += new DataReceivedEventHandler(Process_ErrorDataReceived);
                process.Exited += new EventHandler(Process_Exited);
                process.Start();
                consoleText = "Server Started\r\n";
                process.BeginErrorReadLine();
                process.BeginOutputReadLine();
            }
        }

        public void stop()
        {
            if(process != null)
            {
                process.Close();
                process = null;
            }
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            consoleText = "Process Exited";
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if(e.Data != null)
                consoleText += e.Data + "\r\n";
        }

        [HttpGet]
        public string getConsoleText()
        {
            return consoleText;
        }
        [HttpGet]
        public string getServerInfo()
        {
            MinecraftServerInfo info = HomeController.getMinecraftServerInfo("SimplifiedGamingSolutions.com", 25565);
            StringBuilder sb = new StringBuilder();
            if (info != null)
            {
                sb.AppendFormat(@"<p><b>Address:</b> {0}</p>
                                  <p><b>MOTD:</b> {1}</p>
                                  <p><b>Status:</b> {2}</p>
                                  <p><b>Players:</b> {3}/{4}</p> ", "SimplifiedGamingSolutions.com", info.ServerMotd, "<font color='green'>Online</font>", info.CurrentPlayerCount, info.MaxPlayerCount);
            }
            else
            {
                sb.AppendFormat(@"<p><b>Address:</b> {0}</p>
                              <p><b>MOTD:</b> {1}</p>
                              <p><b>Status:</b> {2}</p>
                              <p><b>Players:</b> {3}/{4}</p> ","SimplifiedGamingSolutions.com", "N/A", "<font color='red'>Offline</font>", "?", "?");
            }
            return sb.ToString();
        }
    }
}