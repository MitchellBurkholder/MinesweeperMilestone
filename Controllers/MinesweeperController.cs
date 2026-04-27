using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MinesweeperMilestone.Extensions;
using MinesweeperMilestone.Filters;
using MinesweeperMilestone.Models;
using MinesweeperMilestone.Services;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;

namespace MinesweeperMilestone.Controllers
{
    public class MinesweeperController : Controller
    {
        // Accepts the form data. Defaults to 5x5, Easy if no data is passed yet.
        [SessionCheckFilter]
        public IActionResult Index()
        {
            // get the board from the session
            Board gameBoard = HttpContext.Session.GetObjectFromJson<Board>("CurrentGame");

            // If it's null (first time visiting), create a default one
            if (gameBoard == null)
            {
                gameBoard = new Board(5, 1);
                HttpContext.Session.SetObjectAsJson("CurrentGame", gameBoard);
            }

            return View(gameBoard);
        }

        // method specifically for the form submission
        [HttpPost]
        public IActionResult ProcessCreate(int size, int difficulty)
        {
            Board gameBoard = new Board(size, difficulty);

            GameService gameService = new GameService(gameBoard);

            gameService.InitializeBoard();

            HttpContext.Session.SetObjectAsJson("CurrentGame", gameBoard);

            return RedirectToAction("Index");
        }


        // Handles the left click from the grid
        /*public IActionResult Visit(int row, int col)
        {
            Board gameBoard = HttpContext.Session.GetObjectFromJson<Board>("CurrentGame");

            if (gameBoard != null)
            {
                GameService gameService = new GameService(gameBoard);

                if (gameService.CheckGameState() == Board.GameState.InProgress)
                {
                    var targetCell = gameBoard.cells[row, col];

                    gameService.ProcessMove(targetCell, "Visit", row, col);

                    HttpContext.Session.SetObjectAsJson("CurrentGame", gameBoard);
                }
            }

            return RedirectToAction("Index");
        }*/

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Handles the right click from the grid
        public IActionResult Flag(int row, int col)
        {
            Board gameBoard = HttpContext.Session.GetObjectFromJson<Board>("CurrentGame");

            if (gameBoard != null)
            {
                GameService gameService = new GameService(gameBoard);

                if (gameService.CheckGameState() == Board.GameState.InProgress)
                {
                    var targetCell = gameBoard.cells[row, col];

                    gameService.ProcessMove(targetCell, "Flag", row, col);

                    HttpContext.Session.SetObjectAsJson("CurrentGame", gameBoard);
                }
            }

            return PartialView("_board", gameBoard);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PartialPageCellUpdate(int row, int col)
        {
            Board gameBoard = HttpContext.Session.GetObjectFromJson<Board>("CurrentGame");

            if (gameBoard == null)
            {
                return BadRequest();
            }
            else
            {
                GameService gameService = new GameService(gameBoard);

                if (gameService.CheckGameState() == Board.GameState.InProgress)
                {
                    var targetCell = gameBoard.cells[row, col];

                    gameService.ProcessMove(targetCell, "Visit", row, col);

                    HttpContext.Session.SetObjectAsJson("CurrentGame", gameBoard);
                }
            }
                  
            return PartialView("_board", gameBoard);
                
        }
    }
}