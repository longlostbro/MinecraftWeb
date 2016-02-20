using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using System.Net;

namespace MinecraftWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public static MinecraftServerInfo getMinecraftServerInfo(string address, int port)
        {
            try
            {
                return MinecraftServerInfo.GetServerInformation(new IPEndPoint(Dns.GetHostAddresses(address)[0], port));
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
