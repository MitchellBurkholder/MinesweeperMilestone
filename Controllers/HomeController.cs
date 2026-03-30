using Microsoft.AspNetCore.Mvc;
using MinesweeperMilestone.Models;
using System.Diagnostics;

namespace MinesweeperMilestone.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Launches the home page
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Used for when an error has occurred on the website
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
