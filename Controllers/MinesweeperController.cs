using Microsoft.AspNetCore.Mvc;
using MinesweeperMilestone.Models;

namespace MinesweeperMilestone.Controllers
{
    public class MinesweeperController : Controller
    {
        public IActionResult Index()
        {
            // Initializes a 10x10 board on Easy (Difficulty 1)
            Board gameBoard = new Board(10, 1);

            return View(gameBoard);
        }
    }
}